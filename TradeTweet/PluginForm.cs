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

namespace TradeTweet
{
    [Exportable]
    public partial class TradeTweet : Form, IExternalComponent, ITradeComponent
    {
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

        private void StartPlugin(string consumerKey = "", string token = "")
        {
            this.Controls.Clear();

            noticePanel = new NoticePanel();
            this.Controls.Add(noticePanel);

            ts = new TwittwerService(consumerKey, token);

            if (!ts.Connected)
            {
                connectionPanel = new ConnectionPanel();
                connectionPanel.OnConnect = OnConnect;
                Controls.Add(connectionPanel);
            }
            else
            {
                tw = new TweetPanel(ts.User, PlatformEngine);
                tw.OnLogout = OnLogout;
                tw.OnTweet = OnTweet;
                tw.OnNotice = (n) => { noticePanel.ShowNotice(n,2000, null); };
                this.Controls.Add(tw);
            }
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

            tw = new TweetPanel(ts.User, PlatformEngine);
            tw.OnNotice = (n) => { noticePanel.ShowNotice(n, 2000, null); };
            tw.OnLogout = OnLogout;
            tw.OnTweet = OnTweet;
            this.Controls.Add(tw);

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

            string mediaString = "";

            byte[] media = (string.IsNullOrEmpty(mediaString)) ? null : new byte[1];

            await ts.SendTweetAsync(new Twitt { Text = tw.Status, Media = media }, mediaString, ct).ContinueWith((t) =>
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

                StartPlugin("822113440844148738-s7MLex2gcSFKxzKZfBDwcwJqvJYk0LA", "8UYP6Ahmn5GjJXkr0bN3Jy2XmKBX8jT3Slxk8EhzLCEmO");
            }
        }

        #endregion
    }
}
