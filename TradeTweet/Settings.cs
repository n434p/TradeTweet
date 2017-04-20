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



    static class TweetMessenger
    {
        private static List<TwitMessage> tweets = new List<TwitMessage>();

        public static Action<string, EventType> OnAutoTweetSend = null;
        public static Action<TwitMessage, Response> OnAutoTweetRespond = null;

        public static void Add(TwitMessage m)
        {
            tweets.Add(m);
        }

        public static void Remove(TwitMessage m)
        {
            tweets.Remove(m);
        }

        public static void Notify()
        {
            foreach (TradeTweet tw in TweetManager.Instances)
            {
                //tw.UpdateHistory(this);
            }
        }

        public static async void SendAutoTweet(TwitMessage msg)
        {
            if (TweetManager.twitService == null || !TweetManager.twitService.Connected)
                return;

            if (OnAutoTweetSend != null)
                OnAutoTweetSend.Invoke("Sending...", msg.EventType);

            var resp = await TweetManager.twitService.SendTweetAsync(new Twitt { Text = msg.Message, Media = null }, null, TweetManager.cts.Token);

            if (OnAutoTweetRespond != null)
                OnAutoTweetRespond.Invoke(msg, resp);
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
                        part = "#" + obj.Id;
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


        public static TwitMessage CreateMessage(EventType type,object obj)
        {
            TwitMessage mess = null;
            string text = "";
            string subText = "";
            DateTime time = DateTime.UtcNow;

            //switch (type)
            //{
            //    case EventType.Empty:

            //        break;
            //    case EventType.OrderPlaced:
            //        text = "#PTMC_platform\n" + obj.Type.ToString() + " Order placed:\n";
            //        subText = text.OrderMessage(Settings.Set[type], obj as Order);
            //        time = obj.Time;
            //        break;
            //    case EventType.OrderCancelled:
            //        text = "#PTMC_platform\n" + obj.Type.ToString() + " Order cancelled:\n";
            //        subText = text.OrderMessage(Settings.Set[type], obj as Order);
            //        time = obj.CloseTime;
            //        break;
            //    case EventType.PositionOpened:
            //        text = "#PTMC_platform\nPosition opened:\n";
            //        subText = text.PositionMessage(Settings.Set[type], obj as Position);
            //        time = obj.OpenTime;
            //        break;
            //    case EventType.PositionClosed:
            //        text = "#PTMC_platform\nPosition closed:\n";
            //        subText = text.PositionMessage(Settings.Set[type], obj as Position);
            //        time = obj.CloseTime;
            //        break;
            //    default:
            //        break;
            //}


            return null;
        }

    }

    abstract class MessageStruct
    {
        public const string DELIMITER = "_";
        public const string PTMC_CAPTION = "#PTMC_platform\n";
        public string Caption { get; protected set; } = PTMC_CAPTION;
        public DateTime Time { get; protected set; } = DateTime.UtcNow;

        public abstract string GetMessageText(EventType type);
    }

    //class OrderMessageStruct : MessageStruct
    //{
    //    Order order = null;

    //    public OrderMessageStruct(EventType type, Order obj)
    //    {
    //        order = obj;

    //        switch (type)
    //        {
    //            case EventType.OrderPlaced:
    //                Caption += order.Type.ToString() + " Order placed:\n";
    //                Time = order.Time;
    //                break;
    //            case EventType.OrderCancelled:
    //                Caption += order.Type.ToString() + " Order cancelled:\n";
    //                Time = order.CloseTime;
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    //public override string GetMessageText(EventType type)
    //    //{
    //    //    if (order == null)
    //    //        return null;

    //    //    List<string> list = new List<string>();

    //    //    foreach (EventItem item in Settings.Set[type].Items.Keys)
    //    //    {
    //    //        if (!Settings.Set[type].Items[item].Checked) continue;

    //    //        string part = "";

    //    //        switch (item)
    //    //        {
    //    //            case EventItem.side:
    //    //                part = order.Side.ToString();
    //    //                break;
    //    //            case EventItem.qty:
    //    //                part = order.Amount.ToString();
    //    //                break;
    //    //            case EventItem.symbol:
    //    //                part = order.Instrument.Symbol.ToString();
    //    //                break;
    //    //            case EventItem.type:
    //    //                part = order.Type.ToString();
    //    //                break;
    //    //            case EventItem.price:
    //    //                part = order.Instrument.FormatPrice(order.Price);
    //    //                break;
    //    //            case EventItem.sl:
    //    //                part = (order.StopLossOrder != null) ? ("SL@" + order.Instrument.FormatPrice(order.StopLossOrder.Price)) : "";
    //    //                break;
    //    //            case EventItem.tp:
    //    //                part = (order.TakeProfitOrder != null) ? ("TP@" + order.Instrument.FormatPrice(order.TakeProfitOrder.Price)) : "";
    //    //                break;
    //    //            case EventItem.id:
    //    //                part = "#" + order.Id;
    //    //                break;
    //    //            default:
    //    //                break;
    //    //        }

    //    //        list.Add(part);
    //    //    }

    //    //    return Caption + string.Join(DELIMITER, list);

    //    //    if (text == subText) return mess;

    //    //    mess = new TwitMessage() { Message = subText, type, time };

    //    //    return mess;
    //    //}

    //    public TwitMessage GetMessage()
    //    {
    //        return null;
    //    }
    //}


    static class TweetManager
    {
        public static List<TradeTweet> Instances { get { return instances.Keys.ToList(); } }
        static Dictionary<TradeTweet, bool> instances = new Dictionary<TradeTweet, bool>();

        public static bool isRunning { get { return twitService != null && twitService.Connected && PlatformEngine != null; } }

        public static TwittwerService twitService;
        public static CancellationTokenSource cts = new CancellationTokenSource();

        static internal NETSDK PlatformEngine;

        public static void Run(TradeTweet instance)
        {
            // unsubscribe
            SubscribingEvents(false);

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

            SubscribingEvents(true);
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

                SubscribingEvents(true);
            }

            // completely unsubscribing
            if (instances.Count == 0)
            {
                cts.Cancel();
                SubscribingEvents(false);
                // renew cancellation token
                cts = new CancellationTokenSource();
            }
        }

        static void SubscribingEvents(bool status)
        {
            if (PlatformEngine == null) return;

            foreach (var eventOp in EventBuilder.EventsList)
            {
                if (status)
                {
                    eventOp.Subscribe();
                }
                else
                {
                    eventOp.Unsubscribe();
                }
            }
        }

        //private static void Orders_OrderAdded(Order obj)
        //{
        //    // skip order's establishing statuses - take only new
        //    if (!Settings.autoTweet || obj.Status != OrderStatus.New) return;

        //    TwitMessage msg = TweetMessenger.CreateMessage(EventType.OrderPlaced, obj);
        //    TweetMessenger.SendAutoTweet(msg);
        //}
    }
}
