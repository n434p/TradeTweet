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

namespace TradeTweet
{
    static class Settings
    {
        static internal string appName = "TradeTweet";

        static internal Font mainFont = new System.Drawing.Font("Arial", 12);
        static internal Color mainFontColor = Color.DarkOrange;
        static internal Color mainBackColor = Color.DimGray;
        static internal int btnHeight = 35;

        static internal Dictionary<EventType, bool> Set;
        static internal string key;
        static internal bool autoTweet;
        static internal string atn;
        static internal string ast;

        public static Action onLogInOut;

        static Settings()
        {
            ClearSettings();
        }

        public static void ClearSettings()
        {
            ast = "";
            atn = "";
            autoTweet = false;
            Set = new Dictionary<EventType, bool>();
            key = "default_set13";

            Refresh();
        }

        public static void Refresh()
        {
            if (onLogInOut != null)
                onLogInOut.Invoke();
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
            public Dictionary<EventType, bool> Set;
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
        public static Action<string> OnAutoTweetSend = null;
        public static Action<string> OnAutoTweetRespond = null;

        public static bool Run(NETSDK engine)
        {
            if (isRunning) return true;

            cts = new CancellationTokenSource();
            ct = cts.Token;

            PlatformEngine = engine;
            ts = new TwittwerService(Settings.ast, Settings.atn);

            LinkEvents();

            return ts.Connected;
        }

        public static void Stop()
        {
            ts.Disconnect();
            ts = null;
        }

        public static void LinkEvents(bool unlinkAll = false)
        {
            foreach (EventType item in Enum.GetValues(typeof(EventType)))
            {
                bool check = (unlinkAll) ? false : Settings.Set[item];

                if (check)
                {
                    switch (item)
                    {
                        case EventType.OrderOpen:
                            PlatformEngine.Orders.OrderAdded += Orders_OrderAdded;
                            break;
                        case EventType.OrderClose:
                            PlatformEngine.Orders.OrderRemoved += Orders_OrderRemoved;
                            break;
                        case EventType.PositionOpen:
                            PlatformEngine.Positions.PositionAdded += Positions_PositionAdded;
                            break;
                        case EventType.PositionClose:
                            PlatformEngine.Positions.PositionRemoved += Positions_PositionRemoved;
                            break;
                    }
                }
                else
                {
                    switch (item)
                    {
                        case EventType.OrderOpen:
                            PlatformEngine.Orders.OrderAdded -= Orders_OrderAdded;
                            break;
                        case EventType.OrderClose:
                            PlatformEngine.Orders.OrderRemoved -= Orders_OrderRemoved;
                            break;
                        case EventType.PositionOpen:
                            PlatformEngine.Positions.PositionAdded -= Positions_PositionAdded;
                            break;
                        case EventType.PositionClose:
                            PlatformEngine.Positions.PositionRemoved -= Positions_PositionRemoved;
                            break;
                    }
                }
            }

        }

        private static async void SendAutoTweet(string text)
        {
            if (ts == null || !ts.Connected)
                return;

            OnAutoTweetSend?.Invoke(text);

            await ts.SendTweetAsync(new Twitt { Text = text, Media = null }, null, ct).ContinueWith((t) =>
            {
                OnAutoTweetRespond?.Invoke("Done!");
            });
        }

        private static void Positions_PositionRemoved(Position obj)
        {
            string text = $"Position removed: {obj.Account} {obj.Instrument} {obj.Id}";
            SendAutoTweet(text);
        }

        private static void Positions_PositionAdded(Position obj)
        {
            string text = $"Position added: {obj.Account} {obj.Instrument} {obj.Id}";
            SendAutoTweet(text);
        }

        private static void Orders_OrderRemoved(Order obj)
        {
            string text = $"Order removed: {obj.Account} {obj.Instrument} {obj.Id}";
            SendAutoTweet(text);
        }

        private static void Orders_OrderAdded(Order obj)
        {
            string text = $"Order added: {obj.Account} {obj.Instrument} {obj.Id}";
            SendAutoTweet(text);
        }

    }
}
