using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using PTLRuntime.NETScript;
using System.Threading.Tasks;

namespace TradeTweet
{
    class TweetPanel : Panel
    {
        SettingsPanel settingsPanel;
        User user;
        StatusPanel statusPanel;
        PicturePanel picPanel;
        Button tweetBtn;
        TextBox tweetText;
        NETSDK PlatformEngine;

        System.Threading.CancellationTokenSource ctss;
        System.Threading.CancellationToken ct;

        Dictionary<EventType, bool> settings = new Dictionary<EventType, bool>();

        public string Status { get { return tweetText.Text; } }

        public Action OnLogout = null;
        public Func<Task<Response>> OnTweet = null;
      //  public Action OnTweet = null;
        public Action<Dictionary<EventType, bool>> OnSettingsApplied = null;
        public Action<bool> OnAutoTweetToggle = null;
        public Action<string> OnAutoTweetAction = null;
        public Action<string> OnNewNotice = null;

        const int MAX_TWEET_LENGTH = 140;
        const string ENTER_TWEET = "Type here...";
        const string BTN_TEXT = "Tweet";
        const int statusPanelHeight = 40;
        const int CARD_ZIZE = 80;
        const int MARGIN = 5;

        public void ToggleTweetButton()
        {
            tweetBtn.Enabled = !tweetBtn.Enabled;

            tweetBtn.BackColor = (tweetBtn.Enabled) ? Settings.mainFontColor : Color.Gray;
            tweetBtn.ForeColor = (tweetBtn.Enabled) ? Settings.mainBackColor : Color.Black;
        }

        public List<Image> GetImages()
        {
            return picPanel.GetImages();
        } 

        public TweetPanel(User tweetUser, NETSDK platformEngine,  Dictionary<EventType, bool> set)
        {
            this.DoubleBuffered = true;

            PlatformEngine = platformEngine;
            this.user = tweetUser;
            this.Dock = DockStyle.Fill;

            ctss = new System.Threading.CancellationTokenSource();
            ct = ctss.Token;

            //if (set != null)
            //    settings = Settings.Set;

            Populate();

            tweetBtn.Click += (o, e) =>
            {
                if (OnTweet != null)
                {
                    MM();
                }
            };

            tweetText.KeyDown += MessageText_KeyDown;

            tweetText.TextChanged += (o, e) =>
            {
                var length = tweetText.TextLength;
                tweetBtn.Text = BTN_TEXT + " [" + (length) + "]";
                tweetBtn.Enabled = length > 0 && length <= MAX_TWEET_LENGTH;
                tweetBtn.BackColor = (tweetBtn.Enabled) ? Settings.mainFontColor : Color.Gray;
            };

            statusPanel.onLogoutClicked = () =>
            {
                if (OnLogout != null)
                    OnLogout.Invoke();
            };

            statusPanel.onSettingsClicked = () =>
            {
                   settingsPanel.ShowSet();
                // await M();
            };

            statusPanel.onAutoTweetClicked = () =>
            {
                if (!settingsPanel.HasEvents)
                {
                    statusPanel.AutoTweet = false;
                    ShowNotice("There is no events to tweet!");
                    return;
                }

                statusPanel.AutoTweet = !statusPanel.AutoTweet;

                if (OnAutoTweetToggle != null)
                    OnAutoTweetToggle.Invoke(statusPanel.AutoTweet);

                ShowNotice((statusPanel.AutoTweet) ? "AutoTweet Enabled!" : "AutoTweet Disabled!");

                if (statusPanel.AutoTweet)
                {
                    AutoTweet.OnAutoTweetSend += ShowNotice;
                    AutoTweet.OnAutoTweetRespond += ShowNotice;
                }
                else
                {
                    AutoTweet.OnAutoTweetSend -= ShowNotice;
                    AutoTweet.OnAutoTweetRespond -= ShowNotice;
                }
            };

            settingsPanel.OnApply = () => 
            {
                //AutoTweet.LinkEvents();

                if (!settingsPanel.HasEvents)
                {
                    statusPanel.AutoTweet = false;
                    ShowNotice("AutoTweet Stopped!");
                    return;
                }

                foreach (EventType item in Enum.GetValues(typeof(EventType)))
                {
                    settings[item] = settingsPanel[item];
                }

                if (OnSettingsApplied != null)
                    OnSettingsApplied.Invoke(settings);

                ShowNotice("Settings applied!");
            };

            picPanel.OnMaxPics = () => 
            {
                ShowNotice("Only 4 pics are allowed for one tweet!");
            };
        }

        //private async Task M()
        //{
        //    await Task.Factory.StartNew(async () =>
        //    {
        //        for (int i = 0; i <= 500; i++)
        //        {
        //            tweetText.Text = "#4 Counting: " + i;

        //            await Task.Delay(1000);

        //            MM();
        //        }
        //    }, ct);
        //}

        private async void MM()
        {
            var rrr = await OnTweet.Invoke();

            if (!rrr.Failed)
            {
                ctss.Cancel();
                tweetText.Text = "";
                picPanel.Clear();
            }
        }

        private void ShowNotice(string text)
        {
            if (OnNewNotice != null)
                OnNewNotice(text);
        }

        private void MessageText_KeyDown(object sender, KeyEventArgs e)
        {
            tweetText.Text = string.Empty;
            tweetText.KeyDown -= MessageText_KeyDown;
        }

        private void Populate()
        {

            tweetText = new TextBox()
            {
                ScrollBars = ScrollBars.Vertical,
                HideSelection = true,
                ForeColor = Color.DimGray,
                BackColor = Color.LightGray,
                Font = Settings.mainFont,
                Multiline = true,
                SelectionLength = 0,
                TextAlign = HorizontalAlignment.Left,
                Text = ENTER_TWEET,
                MaxLength = MAX_TWEET_LENGTH,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(tweetText);

            statusPanel = new StatusPanel(user)
            {
                Height = statusPanelHeight,
                BackColor = Settings.mainBackColor,
                Dock = DockStyle.Top
            };

            this.Controls.Add(statusPanel);

            picPanel = new PicturePanel()
            {
                Height = 3*CARD_ZIZE/2 + 2 * MARGIN,
                Dock = DockStyle.Bottom
            };
            this.Controls.Add(picPanel);

            tweetBtn = new Button()
            {
                FlatStyle = FlatStyle.Flat,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = Settings.mainFont,
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom,
                Text = BTN_TEXT + " [0]",
                Height = Settings.btnHeight,
                BackColor = Color.Gray,
                ForeColor = Color.Black,
                Enabled = false
            };
            this.Controls.Add(tweetBtn);

            settingsPanel = new SettingsPanel();

            this.Controls.Add(settingsPanel);
            settingsPanel.Visible = false;
        }

    }
}
