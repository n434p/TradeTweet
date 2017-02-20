using System.Windows.Forms;
using com.pfsoft.proftrading.commons.external;
using System.Drawing;
using PTLRuntime.NETScript;
using System.Threading;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using PTLRuntime.NETScript.Settings;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;

namespace TradeTweet
{
    [Exportable]
    public partial class TradeTweet : Form, IExternalComponent, ITradeComponent
    {
        TradeTweetSettings tts;
        TwittwerService ts;
        TweetPanel tw;
        ConnectionPanel connectionPanel;
        EnterPinPanel enterPinPanel;
        static NoticePanel noticePanel;
        CancellationToken ct;

        const string LOGIN = "Login";
        const string LOGOUT = "Logout";

        public TradeTweet()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void StartPlugin()
        {
            this.Controls.Clear();

            noticePanel = new NoticePanel();
            this.Controls.Add(noticePanel);

            tts = new TradeTweetSettings();
            tts.GetSettings();

            ts = new TwittwerService(tts.ast, tts.atn);

            ts.onAuthorized += (s1, s2) => {
                tts.ast = s1;
                tts.atn = s2;
            };

            if (!ts.Connected)
            {
                connectionPanel = new ConnectionPanel();
                connectionPanel.OnConnect = OnConnect;
                Controls.Add(connectionPanel);
            }
            else
            {
                this.Controls.Add(CreateTweetPanel());
            }
        }

        private TweetPanel CreateTweetPanel()
        {
            tw = new TweetPanel(ts.User, PlatformEngine, tts.GetCurrentSet());
            tw.OnLogout = OnLogout;
            tw.OnTweet = OnTweet;

            tw.OnAutoTweetAction = (n) =>
            {
                noticePanel.ShowNotice(n, 2000, null);
                OnAutoTweet(n);
            };

            tw.OnAutoTweetToggle = (autoTweet) =>
            {
                tts.autoTweet = autoTweet;
                tts.SetSettings();
            };

            tw.OnSettingsApplied = (set) =>
            {
                tts.subSet = set.Values.ToList();
                tts.SetSettings();
            };

            tw.OnNewNotice = (n) => { noticePanel.ShowNotice(n, 1000, null); };
            return tw;
        }

        private void OnLogout()
        {
            ts.Disconnect();
            StartPlugin();
        }

        private void OnConnect()
        {
            Response resp = ts.Connect();

            if (resp.Failed)
            {
                noticePanel.ShowNotice("Connection error!");
                return;
            }

            Controls.Remove(connectionPanel);

            enterPinPanel = new EnterPinPanel();
            enterPinPanel.OnPinEntered = OnPinEntered;

            Controls.Add(enterPinPanel);
        }

        private void OnPinEntered(string pin)
        {
            var resp = ts.SetToken(pin);

            if (resp.Failed)
            {
                noticePanel.ShowNotice("Pin error!",2000, ReturnToConnect);
                return;
            }

            this.Controls.Add(CreateTweetPanel());

            Controls.Remove(enterPinPanel);
        }

        private void ReturnToConnect()
        {
            this.Invoke((MethodInvoker)delegate
            {
                StartPlugin();
            });
        }

        private async void OnTweet()
        {
            tw.ToggleTweetButton();
            noticePanel.ShowNotice("Sending...");

            var images = tw.GetImages();

            List<Task<string>> list = new List<Task<string>>();

            foreach (var img in images)
            {
                list.Add(ts.SendImageAsync(img, ct));
            }

            await Task.WhenAll(list);

            var mediaIds =
                (from tsk in list
                 select tsk.Result.Split(new char[] { ':', ',' })[1]);

            string mediaString = string.Join(",", mediaIds);

            byte[] media = (string.IsNullOrEmpty(mediaString)) ? null : new byte[1];

            await ts.SendTweetAsync(new Twitt { Text = tw.Status, Media = media }, mediaString, ct).ContinueWith((t) => 
            {
                noticePanel.ShowNotice("Done!");
                tw.ToggleTweetButton();
            });
        }

        private async void OnAutoTweet(string status)
        {
            noticePanel.ShowNotice("AutoTweet...");

            await ts.SendTweetAsync(new Twitt { Text = status, Media = null}, null, ct).ContinueWith((t) =>
            {
                noticePanel.ShowNotice("Done!");
            });
        }

        private void Ts_NewEvents(object sender, System.EventArgs e)
        {
            
        }

        #region IExternalComponent
        public Icon IconImage
        {
            get { return this.Icon; }
        }

        public string ComponentName
        {
            get { return "TradeTweet"; }
        }

        public string PanelHeader
        {
            get { return "TradeTweet"; }
        }

        public Control Content
        {
            get { return this; }
        }

        public NETSDK PlatformEngine
        {
            get;
            set;
        }

        public void Populate()
        {
            if (PlatformEngine != null)
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                //cts.CancelAfter(5000);
                ct = cts.Token;

                M();

                //StartPlugin(); // "822113440844148738-s7MLex2gcSFKxzKZfBDwcwJqvJYk0LA", "8UYP6Ahmn5GjJXkr0bN3Jy2XmKBX8jT3Slxk8EhzLCEmO");
            }
        }

        #endregion

        private void M()
        {
            tts = new TradeTweetSettings();

            tts.ast = "1234";
            tts.atn = "abcd";
            tts.autoTweet = true;
            tts.subSet = new List<bool>() { true,false,false,true} ;

            GlobalVariablesManager.RemoveAll();
            

            tts.SetSettings();
            tts.GetSettings();
        }

        [Serializable]
        public class TradeTweetSettings
        {
            [NonSerialized]
            public List<bool> subSet = new List<bool>();

            public Dictionary<EventType, bool> GetCurrentSet()
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

            public string key = "set";
            public bool autoTweet = false;
            public string atn = "";
            public string ast = "";

            sealed class AllowAllAssemblyVersionsDeserializationBinder : System.Runtime.Serialization.SerializationBinder
            {
                public override Type BindToType(string assemblyName, string typeName)
                {
                    Type typeToDeserialize = null;

                    String currentAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName;

                    // In this case we are always using the current assembly
                    assemblyName = currentAssembly;

                    // Get the type using the typeName and assemblyName
                    typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                        typeName, assemblyName));

                    return typeToDeserialize;
                }
            }

            public void SetSettings()
            {
                var str = string.Empty;

                using (var ms = new System.IO.MemoryStream())
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formatter.Binder = new AllowAllAssemblyVersionsDeserializationBinder();
                    formatter.Serialize(ms, this);
                    var data = ms.ToArray();
                    str = System.Text.Encoding.Default.GetString(data);
                    GlobalVariablesManager.SetValue(key, str, VariableLifetime.SaveFile);
                }
            }

            public void GetSettings()
            {
                if (!GlobalVariablesManager.Exists(key)) return;

                string str = (string)GlobalVariablesManager.GetValue(key);

                byte[] buffer = System.Text.Encoding.Default.GetBytes(str);


                using (var ms = new System.IO.MemoryStream(buffer))
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    formatter.Binder = new AllowAllAssemblyVersionsDeserializationBinder();

                    AppDomain.CurrentDomain.AssemblyResolve +=
                    new ResolveEventHandler(CurrentDomain_AssemblyResolve);

                    var ts = formatter.Deserialize(ms) as TradeTweetSettings;

                    AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);

                    if (ts != null)
                    {
                        ast = ts.ast;
                        atn = ts.atn;
                        autoTweet = ts.autoTweet;
                        subSet = ts.subSet;
                    }

                }


            }

            private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
            {
                try
                {
                    Assembly assembly = System.Reflection.Assembly.Load(args.Name);
                    if (assembly != null)
                        return assembly;
                }
                catch {; }

                return Assembly.GetExecutingAssembly();
            }
        }
    }

}
