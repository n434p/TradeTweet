using PTLRuntime.NETScript;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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

        static internal Dictionary<EventType, bool> dic;
        static internal List<bool> subSet;
        static internal string key;
        static internal bool autoTweet;
        static internal string atn;
        static internal string ast;

        public static Dictionary<EventType, bool> GetCurrentSet()
        {

            if (subSet == null || subSet.Count != Enum.GetValues(typeof(EventType)).Length)
                return null;

            Dictionary<EventType, bool> res = new Dictionary<EventType, bool>();

            foreach (EventType item in Enum.GetValues(typeof(EventType)))
            {
                res[item] = subSet[(int)item];
            }

            return res;
        }

        static Settings()
        {
            ClearSettings();
        }

        public static void ClearSettings()
        {
            ast = "";
            atn = "";
            autoTweet = false;
            subSet = new List<bool>();
            dic = new Dictionary<EventType, bool>();
            key = "default_set";
        }

        public static void SaveSettings()
        {
            TradeTweetSettings tts = new TradeTweetSettings()
            {
                ast = ast,
                atn = atn,
                autoTweet = autoTweet,
                dic = dic,
                key = key,
                subSet = subSet
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
                subSet = ts.subSet;
                dic = ts.dic;
                key = ts.key;
            }
        }

        [DataContract]
        class TradeTweetSettings
        {
            [DataMember]
            public Dictionary<EventType, bool> dic = Settings.dic;
            [DataMember]
            public List<bool> subSet = new List<bool>();
            [DataMember]
            public string key = "default_set1";
            [DataMember]
            public bool autoTweet = false;
            [DataMember]
            public string atn = "";
            [DataMember]
            public string ast = "";
        }
    }


}
