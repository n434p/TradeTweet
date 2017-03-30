using PTLRuntime.NETScript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;

namespace TradeTweet
{
    static class Settings
    {
        static internal string appName = "TradeTweet";

        static internal Font mainFont = new System.Drawing.Font("Arial", 12);
        static internal Color mainFontColor = Color.DarkOrange;
        static internal Color mainBackColor = Color.DimGray;
        static internal int btnHeight = 35;

        static internal Dictionary<EventType,EventOperation> Set;
        static internal string key;
        static internal bool autoTweet;
        static internal string atn;
        static internal string ast;

        public static Action onLogInOut;
        public static Action onSettingsChanged;

        static Settings()
        {
            ClearSettings(false);
        }

        public static void ClearSettings(bool withFlush)
        {
            ast = ""; //822113440844148738-s7MLex2gcSFKxzKZfBDwcwJqvJYk0LA";
            atn = ""; //8UYP6Ahmn5GjJXkr0bN3Jy2XmKBX8jT3Slxk8EhzLCEmO";
            autoTweet = false;
            Set = new Dictionary<EventType, EventOperation>();
            key = "default_set333";

            if (withFlush)
                SaveSettings();

            Refresh();
        }

        public static void Refresh()
        {
            if (onLogInOut != null)
                onLogInOut.Invoke();
        }

        public static void OnSettingsChange()
        {
            SaveSettings();

            if (onSettingsChanged != null)
                onSettingsChanged.Invoke();
        }

        public static void SaveSettings()
        {
            TradeTweetSettings tts = new TradeTweetSettings()
            {
                ast = ast,
                atn = atn,
                autoTweet = autoTweet,
                Set = Set,
                key = key
            };  

            var str = string.Empty;

            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(TradeTweetSettings));
                serializer.WriteObject(memoryStream, tts);
                memoryStream.Position = 0;
                string res = reader.ReadToEnd();

                GlobalVariablesManager.SetValue(key, res, VariableLifetime.SaveFile);
                GlobalVariablesManager.Flush();
            }
        }

        public static void LoadSettings()
        {
            if (!GlobalVariablesManager.Exists(key)) return;

            string str = (string)GlobalVariablesManager.GetValue(key);

            byte[] buffer = System.Text.Encoding.Default.GetBytes(str);

            TradeTweetSettings ts = null;

            using (Stream stream = new MemoryStream())
            {
                stream.Write(buffer, 0, buffer.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(typeof(TradeTweetSettings));
                ts = deserializer.ReadObject(stream) as TradeTweetSettings;
            }

            if (ts != null)
            {
                ast = string.IsNullOrEmpty(ts.ast) ? "" : ts.ast;
                atn = string.IsNullOrEmpty(ts.atn) ? "" : ts.atn;
                autoTweet = ts.autoTweet;
                Set = ts.Set;
                key = ts.key;
            }
        }

        [DataContract]
        class TradeTweetSettings
        {
            [DataMember]
            public Dictionary<EventType, EventOperation> Set;
            [DataMember]
            public string key;
            [DataMember]
            public bool autoTweet;
            [DataMember]
            public string atn;
            [DataMember]
            public string ast;
        }
    }

    static class AutoTweet
    {
        static Dictionary<TradeTweet, bool> instances = new Dictionary<TradeTweet, bool>();

        public static bool isRunning { get { return twitService != null && twitService.Connected && PlatformEngine != null; } }

        public static TwittwerService twitService;
        static CancellationTokenSource cts = new CancellationTokenSource();

        static internal NETSDK PlatformEngine;
        public static Action<string, EventType> OnAutoTweetSend = null;
        public static Action<TwitMessage, Response> OnAutoTweetRespond = null;

        public static void Run(TradeTweet instance)
        {
            // unsubscribe
            SubscribingEngine(false);

            // refresh connection flags
            foreach (var inst in instances.Keys.ToList())
            {
                instances[inst] = false;
            }

            instances[instance] = true;

            // set twitt service
            if(twitService == null)
                twitService = new TwittwerService(Settings.ast, Settings.atn);

            PlatformEngine = instance.PlatformEngine;

            SubscribingEngine(true);
        }

        public static void Stop(TradeTweet instance)
        {
            // instatnce is in connection state - erase TwitService data(request token)
            if (!isRunning && twitService != null)
            {
                twitService.EraseCridentials();
            }

            // nothing to stop?
            if (instances.Count == 0) return;

            var isConnectionKeeper = instances[instance];
            instances.Remove(instance);

            // need to redelegate connection keeper? 
            if (instances.Count > 0 && isConnectionKeeper)
            {
                instances[instances.Keys.First()] = true;

                PlatformEngine = instance.PlatformEngine;

                SubscribingEngine(true);
            }

            // completely unsubscribing
            if (instances.Count == 0)
            {
                cts.Cancel();
                SubscribingEngine(false);
                // renew cancellation token
                cts = new CancellationTokenSource();
            }
        }

        static void SubscribingEngine(bool status)
        {
            if (PlatformEngine == null) return;

            if (status)
            {
                PlatformEngine.Orders.OrderAdded += Orders_OrderAdded;
                PlatformEngine.Orders.OrderRemoved += Orders_OrderRemoved;
                PlatformEngine.Positions.PositionAdded += Positions_PositionAdded;
                PlatformEngine.Positions.PositionRemoved += Positions_PositionRemoved;
            }
            else
            {
                PlatformEngine.Orders.OrderAdded -= Orders_OrderAdded;
                PlatformEngine.Orders.OrderRemoved -= Orders_OrderRemoved;
                PlatformEngine.Positions.PositionAdded -= Positions_PositionAdded;
                PlatformEngine.Positions.PositionRemoved -= Positions_PositionRemoved;
            }
        }

        private static async void SendAutoTweet(TwitMessage msg)
        {
            if (twitService == null || !twitService.Connected)
                return;

            if(OnAutoTweetSend != null)
                OnAutoTweetSend.Invoke("Sending...", msg.EventType);

            var resp = await twitService.SendTweetAsync(new Twitt { Text = msg.Message, Media = null }, null, cts.Token);

            if(OnAutoTweetRespond != null)
                OnAutoTweetRespond.Invoke(msg, resp);
        }



        private static void Positions_PositionRemoved(Position obj)
        {
            if (!Settings.autoTweet) return;

            string text = "#PTMC_platform\nPosition closed:\n";

            string subText = text.PositionMessage(Settings.Set[EventType.PositionClosed], obj);

            if (text == subText) return;

            var msg = new TwitMessage() { Message = subText, EventType = EventType.PositionClosed, Time = obj.CloseTime };

            SendAutoTweet(msg);
        }

        private static void Positions_PositionAdded(Position obj)
        {
            if (!Settings.autoTweet) return;

            string text = "#PTMC_platform\nPosition opened:\n";

            string subText = text.PositionMessage(Settings.Set[EventType.PositionOpened], obj);

            if (text == subText) return;

            var msg = new TwitMessage() { Message = subText, EventType = EventType.PositionOpened, Time = obj.OpenTime };

            SendAutoTweet(msg);
        }

        private static void Orders_OrderRemoved(Order obj)
        {
            if (!Settings.autoTweet) return;

            string text = "#PTMC_platform\n" + obj.Type.ToString() + " Order cancelled:\n";

            string subText = text.OrderMessage(Settings.Set[EventType.OrderCancelled], obj);

            if (text == subText) return;

            var msg = new TwitMessage() { Message = subText, EventType = EventType.OrderCancelled, Time = obj.CloseTime };

            SendAutoTweet(msg);
        }

        private static void Orders_OrderAdded(Order obj)
        {
            // skip order's establishing statuses - take only new
            if (!Settings.autoTweet || obj.Status != OrderStatus.New) return;

            string text = "#PTMC_platform\n" + obj.Type.ToString()+" Order placed:\n";

            string subText = text.OrderMessage(Settings.Set[EventType.OrderPlaced], obj);

            if (text == subText) return;

            var msg = new TwitMessage() { Message = subText, EventType = EventType.OrderPlaced, Time = obj.Time };

            SendAutoTweet(msg);
        }

        public static string PositionMessage(this string value, EventOperation op, Position obj)
        {
            string delimiter = "_";
            List<string> list = new List<string>();

            foreach (EventItem item in op.Items.Keys)
            {
                if (!op.Items[item].Checked) continue;

                string part = "";

                switch (item)
                {
                    case EventItem.side:
                        part = obj.Side.ToString();
                        break;
                    case EventItem.qty:
                        part = obj.Amount.ToString();
                        break;
                    case EventItem.symbol:
                        part = obj.Instrument.Symbol.ToString();
                        break;
                    case EventItem.price:
                        part = obj.Instrument.FormatPrice(obj.OpenPrice);
                        break;
                    case EventItem.sl:
                        part = (obj.StopLossOrder != null) ? ("SL@" + obj.Instrument.FormatPrice(obj.StopLossOrder.Price)) : "";
                        break;
                    case EventItem.tp:
                        part = (obj.TakeProfitOrder != null) ? ("TP@" + obj.Instrument.FormatPrice(obj.TakeProfitOrder.Price)) : "";
                        break;
                    case EventItem.id:
                        part = "#"+obj.Id;
                        break;
                }

                list.Add(part);
            }

            return value + string.Join(delimiter, list);
        }

        public static string OrderMessage(this string value, EventOperation op, Order obj)
        {
            string delimiter = "_";
            List<string> list = new List<string>();

            foreach (EventItem item in op.Items.Keys)
            {
                if (!op.Items[item].Checked) continue;

                string part = "";

                switch (item)
                {
                    case EventItem.side:
                        part = obj.Side.ToString();
                        break;
                    case EventItem.qty:
                        part = obj.Amount.ToString();
                        break;
                    case EventItem.symbol:
                        part = obj.Instrument.Symbol.ToString();
                        break;
                    case EventItem.type:
                        part = obj.Type.ToString();
                        break;
                    case EventItem.price:
                        part = obj.Instrument.FormatPrice(obj.Price);
                        break;
                    case EventItem.sl:
                        part = (obj.StopLossOrder != null) ? ("SL@" + obj.Instrument.FormatPrice(obj.StopLossOrder.Price)) : "";
                        break;
                    case EventItem.tp:
                        part = (obj.TakeProfitOrder != null) ? ("TP@" + obj.Instrument.FormatPrice(obj.TakeProfitOrder.Price)) : "";
                        break;
                    case EventItem.id:
                        part = "#" + obj.Id;
                        break;
                    default:
                        break;
                }

                list.Add(part);
            }

            return value + string.Join(delimiter, list);
        }

    }
}
