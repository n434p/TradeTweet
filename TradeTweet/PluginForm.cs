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
        NoticePanel noticePanel;

        CancellationTokenSource cts = new CancellationTokenSource();
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

            tip = new ToolTip();

            Settings.Set = EventManager.EventsList;

            Settings.SaveSettings();

            Settings.LoadSettings();

            TweetManager.Run(this);

            noticePanel = new NoticePanel(this, ct);
            noticePanel.Dock = DockStyle.Top;

            TweetManager.twitService.onAuthorized = (s1, s2) => {
                Settings.ast = s1;
                Settings.atn = s2;
                Settings.SaveSettings();
            };

            if (!TweetManager.isRunning)
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

        public void ShowNotice(string text, Image img, int delay = 2000, Action callback = null)
        {
            noticePanel.ShowNotice(text,img,delay,EventStatus.Error,callback);
        }

        private TweetPanel CreateTweetPanel()
        {
            var twitPanel = new TweetPanel();

            twitPanel.Padding = new Padding(0);
            twitPanel.Margin = new Padding(0);
            twitPanel.Dock = DockStyle.Fill;

            twitPanel.OnLogout = OnLogout;

            return twitPanel;
        }

        private void OnLogout()
        {
            TweetManager.twitService.EraseCridentials();
            Settings.ClearSettings(true);
        }

        private void OnConnect()
        {
            Response resp = TweetManager.twitService.Connect();

            if (resp.Failed)
            {
                ShowNotice(resp.Text, Properties.Resources.TradeTweet_10);
                return;
            }

            Controls.Remove(connectionPanel);

            enterPinPanel = new EnterPinPanel();
            enterPinPanel.OnPinEntered = OnPinEntered;

            Controls.Add(enterPinPanel);
        }

        private void OnPinEntered(string pin)
        {
            var resp = TweetManager.twitService.SetToken(pin);

            if (resp.Failed)
            {
                TweetManager.twitService.EraseCridentials();
                ShowNotice("Wrong PIN!", Properties.Resources.TradeTweet_10, 2000, ReturnToConnect);
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
                //cts.CancelAfter(5000);
                ct = cts.Token;

                Settings.onLogInOut += () => StartPlugin();        

                StartPlugin(); // "822113440844148738-s7MLex2gcSFKxzKZfBDwcwJqvJYk0LA", "8UYP6Ahmn5GjJXkr0bN3Jy2XmKBX8jT3Slxk8EhzLCEmO");
            }
        }

        #endregion

    }

}
