using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using PTLRuntime.NETScript;

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

        Dictionary<EventType, bool> settings = null;

        public Action OnLogout = null;
        public Action OnTweet = null;
        public Action<String> OnAutoTweet = null;
        public Action<String> OnNewNotice = null;

        const int MAX_TWEET_LENGTH = 140;
        const string ENTER_TWEET = "Type here...";
        const string BTN_TEXT = "Tweet";
        const int statusPanelHeight = 40;
        const int CARD_ZIZE = 80;
        const int MARGIN = 5;

        public string Status { get { return tweetText.Text; } }

        public void ToggleTweetButton()
        {
            tweetBtn.Enabled = !tweetBtn.Enabled;

            tweetBtn.BackColor = (tweetBtn.Enabled) ? Settings.mainFontColor : Color.Gray;
            tweetBtn.ForeColor = (tweetBtn.Enabled) ? Settings.mainBackColor : Color.Black;
        }

        public List<Image> GetImages()
        {
            List<Image> list = new List<Image>();

            foreach (PictureBox item in picPanel.Controls.OfType<PictureBox>())
            {
                if (item.BackgroundImage != null)
                    list.Add(item.BackgroundImage);   
            }

            return list;
        } 

        public TweetPanel(User tweetUser, NETSDK platformEngine)
        {

            this.DoubleBuffered = true;

            PlatformEngine = platformEngine;
            this.user = tweetUser;
            this.Dock = DockStyle.Fill;

            Populate();

            tweetBtn.Click += (o, e) =>
            {
                if (OnTweet != null)
                    OnTweet.Invoke();
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
                M();
            };

            statusPanel.onAutoTweetClicked = () =>
            {
                if (OnNewNotice != null)
                        OnNewNotice.Invoke((!statusPanel.AutoTweet) ? "AutoTweet Enabled!" : "AutoTweet Disabled!");


                LinkEvents(!statusPanel.AutoTweet && settingsPanel.HasEvents);
            };

            settingsPanel.OnApply = () => 
            {
                if (OnNewNotice != null)
                    OnNewNotice.Invoke("Settings applied!");

                LinkEvents(!statusPanel.AutoTweet);
            };

            picPanel.OnMaxPics = () => 
            {
                if (OnNewNotice != null)
                    OnNewNotice.Invoke("Only 4 pics are allowed for one tweet!");
            };
        }

        private void M()
        {
            var sett = new TradeTweetSettings()
            {
                //Set = new Dictionary<EventType, bool>()
                //{
                //    { EventType.OrderClose, true},
                //    { EventType.PositionOpen, false}
                //},
                autotweet = false,
                ast = "abcd",
                atn = "123"
            };

            var str = string.Empty;

            using (var ms = new System.IO.MemoryStream())
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms, sett);
                var data = ms.ToArray();
                str = System.Text.Encoding.ASCII.GetString(data);
            }

            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(str);

            try
            {
                using (var ms = new System.IO.MemoryStream(buffer))
                {
                    var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                    var res = formatter.Deserialize(ms);
                }
            }
            catch (Exception ex)
            {

            }


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
                Height = CARD_ZIZE + 2 * MARGIN,
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

            settingsPanel = new SettingsPanel(settings);

            this.Controls.Add(settingsPanel);
            settingsPanel.Visible = false;
        }

        #region Trade Events

        private void LinkEvents(bool autoTweet)
        {
            if (!autoTweet) return;

            foreach (EventType item in Enum.GetValues(typeof(EventType)))
            {
                bool check = settingsPanel[item];

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

        private void Positions_PositionRemoved(Position obj)
        {
            string n = $"Position removed: {obj.Account} {obj.Instrument} {obj.Id}";
            OnAutoTweet(n);
        }

        private void Positions_PositionAdded(Position obj)
        {
            string n = $"Position added: {obj.Account} {obj.Instrument} {obj.Id}";
            OnAutoTweet(n);
        }

        private void Orders_OrderRemoved(Order obj)
        {
            string n = $"Order removed: {obj.Account} {obj.Instrument} {obj.Id}";
            OnAutoTweet(n);
        }

        private void Orders_OrderAdded(Order obj)
        {
            string n = $"Order added: {obj.Account} {obj.Instrument} {obj.Id}";
            OnAutoTweet(n);
        }


        [Serializable]
        public class TradeTweetSettings
        {
            //  public Dictionary<EventType, bool> Set;
            public bool autotweet = false;
            public string atn = "";
            public string ast = "";
        }

        #endregion
    }
}
