using System.Windows.Forms;
using com.pfsoft.proftrading.commons.external;
using System.Drawing;
using PTLRuntime.NETScript;
using System.Threading;
using System;

namespace TradeTweet
{
    [Exportable]
    public partial class TradeTweet : Form, IExternalComponent, ITradeComponent
    {
        ToolTip tip; 
        ConnectionPanel connectionPanel;
        EnterPinPanel enterPinPanel;
        CancellationToken ct;
        NoticeP2 noticePanel;

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

            AutoTweet.Run(this);

            noticePanel = new NoticeP2(this);
            noticePanel.Dock = DockStyle.Top;


            AutoTweet.twitService.onAuthorized = (s1, s2) => {
                Settings.ast = s1;
                Settings.atn = s2;
                Settings.SaveSettings();
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

        public void ShowNotice(string text, int delay = 2000, Action callback = null)
        {
            noticePanel.ShowNotice(text,delay,NoticeType.Error,EventType.Empty,callback);
        }

        private TPanel CreateTweetPanel()
        {
            var tt = new TPanel();
            tt.TweetPanel(PlatformEngine);

            tt.NoticeSubscribing();

            tt.Padding = new Padding(0);
            tt.Margin = new Padding(0);
            tt.Dock = DockStyle.Fill;

            tt.OnLogout = OnLogout;

            return tt;
        }

        private void OnLogout()
        {
            AutoTweet.twitService.EraseCridentials();

            Settings.ClearSettings(true);
            
        }

        private void OnConnect()
        {
            Response resp = AutoTweet.twitService.Connect();

            if (resp.Failed)
            {
                ShowNotice(resp.Text);
                return;
            }

            Controls.Remove(connectionPanel);

            enterPinPanel = new EnterPinPanel();
            enterPinPanel.OnPinEntered = OnPinEntered;

            Controls.Add(enterPinPanel);
        }

        private void OnPinEntered(string pin)
        {
            var resp = AutoTweet.twitService.SetToken(pin);

            if (resp.Failed)
            {
                AutoTweet.twitService.EraseCridentials();
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
