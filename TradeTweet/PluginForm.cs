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
using System.Runtime.Serialization;

namespace TradeTweet
{
    [Exportable]
    public partial class TradeTweet : Form, IExternalComponent, ITradeComponent
    {
        ToolTip tip; 
        TwittwerService ts;
        TweetPanel tw;
        ConnectionPanel connectionPanel;
        EnterPinPanel enterPinPanel;
        CancellationToken ct;
        NoticePanel noticePanel;

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

            tip = new ToolTip();

            Settings.LoadSettings();

            ts = AutoTweet.Run(PlatformEngine);

            AutoTweet.OnAutoTweetSend += (s) =>
            {
                ShowNotice(s);
            };

            AutoTweet.OnAutoTweetRespond += (s) =>
            {
                ShowNotice(s);
            };

            noticePanel = new NoticePanel();
            this.Controls.Add(noticePanel);

            ts.onAuthorized = (s1, s2) => {
                Settings.ast = s1;
                Settings.atn = s2;
            };

            if (!AutoTweet.isRunning)
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

        public void ShowNotice(string text, int delay = 1000, Action callback = null)
        {
            noticePanel.ShowNotice(this, text, delay,callback);
        }

        private TPanel CreateTweetPanel()
        {
            var tt = new TPanel();
            tt.TweetPanel(PlatformEngine);

            tt.Padding = new Padding(0);
            tt.Margin = new Padding(0);
            tt.Dock = DockStyle.Fill;

            return tt;

            //tw = new TweetPanel(ts.User, PlatformEngine, Settings.Set);
            //tw.OnLogout = OnLogout;
            //tw.OnTweet = OnTweet;

            //tw.OnAutoTweetAction = (n) =>
            //{
            //    //ShowNotice(n, 2000, null);
            //    //OnAutoTweet(n);
            //};

            //tw.OnAutoTweetToggle = (autoTweet) =>
            //{
            //    Settings.autoTweet = autoTweet;
            //    Settings.SaveSettings();
            //};

            //tw.OnSettingsApplied = (set) =>
            //{
            //    Settings.Set = set;
            //    Settings.SaveSettings();
            //};

            //tw.OnNewNotice = (n) => { ShowNotice(n, 1000, null); };
            //return tw;
        }

        private void OnLogout()
        {
            ts.Disconnect();
            Settings.ClearSettings();
            StartPlugin();
        }

        private void OnConnect()
        {
            Response resp = ts.Connect();

            if (resp.Failed)
            {
                ShowNotice("Connection error!");
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
                ShowNotice("Pin error!",2000, ReturnToConnect);
                return;
            }

            this.Controls.Add(CreateTweetPanel());
            Controls.Remove(enterPinPanel);

            Settings.Refresh();
        }

        private void ReturnToConnect()
        {
            this.Invoke((MethodInvoker)delegate
            {
                StartPlugin();
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

                Settings.onLogInOut += () => StartPlugin();

                StartPlugin(); // "822113440844148738-s7MLex2gcSFKxzKZfBDwcwJqvJYk0LA", "8UYP6Ahmn5GjJXkr0bN3Jy2XmKBX8jT3Slxk8EhzLCEmO");
            }
        }

        #endregion
    }

}
