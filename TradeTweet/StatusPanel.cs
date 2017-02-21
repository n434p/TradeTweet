using System;
using System.Drawing;
using System.Windows.Forms;

namespace TradeTweet
{
    class StatusPanel : Panel
    {
        LinkLabel login;
        PictureBox avatar;
        PictureBox settings;
        PictureBox autoTweet;

        public bool AutoTweet
        {
            get
            {
                return Settings.autoTweet;
            }
            set
            {
                if(autoTweet != null)
                    autoTweet.Image = (value) ? Properties.Resources.autoTweet : Properties.Resources.autoTweetGrey;

                Settings.autoTweet = value;
            }
        }

        public bool SettingsOpen
        {
            get { return so; }
            set
            {
                if (settings != null)
                    settings.Image = (value) ? Properties.Resources.settings : Properties.Resources.settingsClose;

                so = value;
            }
        }
        bool so = true;

        const string LOGOUT = "Logout";
        const int panelHeight = 40;

        public Action onLogoutClicked = null;
        public Action onSettingsClicked = null;
        public Action onAutoTweetClicked = null;

        public StatusPanel(User user)
        {
            this.Padding = new Padding(0,5,0,5);

            string text = user.screen_name + ", " + LOGOUT;

            login = new LinkLabel()
            {
                ActiveLinkColor = System.Drawing.Color.DarkOrange,
                Font = new System.Drawing.Font("Arial", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204))),
                ForeColor = System.Drawing.Color.White,
                LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline,
                LinkColor = System.Drawing.Color.DarkOrange,
                Height = panelHeight,
                Text = text,
                AutoSize = true,
                Dock = DockStyle.Right,
                TextAlign = System.Drawing.ContentAlignment.BottomCenter,
                VisitedLinkColor = System.Drawing.Color.DarkOrange
            };

            avatar = new PictureBox()
            {
                BackgroundImageLayout = ImageLayout.Zoom,
                Image = Properties.Resources.avatar,
                BackgroundImage = user.avatar,
                Size = new Size(panelHeight, panelHeight),
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Right
            };

            login.LinkArea = new LinkArea(login.Text.Length - LOGOUT.Length, LOGOUT.Length);
            login.LinkClicked += (o, e) => {
                if (onLogoutClicked != null)
                    onLogoutClicked.Invoke();
            };

            autoTweet = new PictureBox()
            {
                Size = new Size(panelHeight, panelHeight),
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Left,
                Cursor = Cursors.Hand
            };

            autoTweet.Click += (o, e) => {
                if (onAutoTweetClicked != null)
                    onAutoTweetClicked.Invoke();
            };

            this.Controls.Add(autoTweet);

            settings = new PictureBox()
            {
                Image = Properties.Resources.settings,
                Size = new Size(panelHeight, panelHeight),
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Left,
                Cursor = Cursors.Hand
            };

            settings.Click += (o, e) => {
                if (onSettingsClicked != null)
                    onSettingsClicked.Invoke();

                SettingsOpen = !SettingsOpen;
            };


            this.Controls.Add(settings);
            this.Controls.Add(avatar);
            this.Controls.Add(login);
        }

    }
}
