using PTLRuntime.NETScript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            ClearSettings();
        }

        public static void ClearSettings()
        {
            ast = ""; //822113440844148738-s7MLex2gcSFKxzKZfBDwcwJqvJYk0LA";
            atn = ""; //8UYP6Ahmn5GjJXkr0bN3Jy2XmKBX8jT3Slxk8EhzLCEmO";
            autoTweet = false;
            Set = new Dictionary<EventType, EventOperation>();
            key = "default_set05";

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
                ast = ts.ast;
                atn = ts.atn;
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
        public static bool isRunning { get { return ts != null && ts.Connected && PlatformEngine != null; } }
        static TwittwerService ts;
        static CancellationToken ct;
        static CancellationTokenSource cts;

        static internal NETSDK PlatformEngine;
        public static Action<string, EventType> OnAutoTweetSend = null;
        public static Action<string, Response, EventType> OnAutoTweetRespond = null;

        public static TwittwerService Run(NETSDK engine)
        {
            if (isRunning) return ts;

            cts = new CancellationTokenSource();
            ct = cts.Token;

            PlatformEngine = engine;

            ts = new TwittwerService(Settings.ast, Settings.atn);

            PlatformEngine.Orders.OrderAdded += Orders_OrderAdded;
            PlatformEngine.Orders.OrderRemoved += Orders_OrderRemoved;
            PlatformEngine.Positions.PositionAdded += Positions_PositionAdded;
            PlatformEngine.Positions.PositionRemoved += Positions_PositionRemoved;

            return ts;
        }

        public static void Stop()
        {
          //  LinkEvents(true);
            ts.Disconnect();
            ts = null;
        }

        public static void LinkEvents(bool unlinkAll = false)
        {
            foreach (EventType type in Settings.Set.Keys)
            {
                bool check = (unlinkAll) ? false : Settings.Set[type].Active;

                if (check)
                {
                    switch (type)
                    {
                        case EventType.OrderPlaced:
                            PlatformEngine.Orders.OrderAdded += Orders_OrderAdded;
                            break;
                        case EventType.OrderCancelled:
                            PlatformEngine.Orders.OrderRemoved += Orders_OrderRemoved;
                            break;
                        case EventType.PositionOpened:
                            PlatformEngine.Positions.PositionAdded += Positions_PositionAdded;
                            break;
                        case EventType.PositionClosed:
                            PlatformEngine.Positions.PositionRemoved += Positions_PositionRemoved;
                            break;
                    }
                }
                else
                {
                    switch (type)
                    {
                        case EventType.OrderPlaced:
                            PlatformEngine.Orders.OrderAdded -= Orders_OrderAdded;
                            break;
                        case EventType.OrderCancelled:
                            PlatformEngine.Orders.OrderRemoved -= Orders_OrderRemoved;
                            break;
                        case EventType.PositionOpened:
                            PlatformEngine.Positions.PositionAdded -= Positions_PositionAdded;
                            break;
                        case EventType.PositionClosed:
                            PlatformEngine.Positions.PositionRemoved -= Positions_PositionRemoved;
                            break;
                    }
                }
            }
        }

        private static async void SendAutoTweet(string text, EventType type)
        {
            if (ts == null || !ts.Connected)
                return;

            if(OnAutoTweetSend != null)
                OnAutoTweetSend.Invoke("Sending...", type);

            var resp = await ts.SendTweetAsync(new Twitt { Text = text, Media = null }, null, ct);

            if(OnAutoTweetRespond != null)
                OnAutoTweetRespond.Invoke(text, resp, type);
        }

        private static void Positions_PositionRemoved(Position obj)
        {
            if (!Settings.autoTweet) return;

            string text = "#PTMC_platform\nPosition closed:\n";

            string subText = text.PositionMessage(Settings.Set[EventType.PositionClosed], obj);

            if (text == subText) return;

            SendAutoTweet(subText, EventType.PositionClosed);
        }

        private static void Positions_PositionAdded(Position obj)
        {
            if (!Settings.autoTweet) return;

            string text = "#PTMC_platform\nPosition opened:\n";

            string subText = text.PositionMessage(Settings.Set[EventType.PositionOpened], obj);

            if (text == subText) return;

            SendAutoTweet(subText, EventType.PositionOpened);
        }

        private static void Orders_OrderRemoved(Order obj)
        {
            if (!Settings.autoTweet) return;

            string text = "#PTMC_platform\n" + obj.Type.ToString() + " Order cancelled:\n";

            string subText = text.OrderMessage(Settings.Set[EventType.OrderCancelled], obj);

            if (text == subText) return;

            SendAutoTweet(subText, EventType.OrderCancelled);
        }

        private static void Orders_OrderAdded(Order obj)
        {
            // skip order's establishing statuses - take only new
            if (!Settings.autoTweet || obj.Status != OrderStatus.New) return;

            string text = "#PTMC_platform\n" + obj.Type.ToString()+" Order placed:\n";

            string subText = text.OrderMessage(Settings.Set[EventType.OrderPlaced], obj);

            if (text == subText) return;

            SendAutoTweet(subText, EventType.OrderPlaced);
        }

        public static string PositionMessage(this string value, EventOperation op, Position obj)
        {
            string delimiter = "_";
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
                    default:
                        break;
                }

                part = string.Join(delimiter, part);

                if (value.Length + part.Length <= 140)
                    value += part;
            }

            return value;
        }

        public static string OrderMessage(this string value, EventOperation op, Order obj)
        {
            string delimiter = "_";
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

                part = string.Join(delimiter, part);

                if (value.Length + part.Length <= 140)
                    value += part;
            }

            return value;
        }

    }
}
