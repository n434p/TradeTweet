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

        public bool AutoTweet { get; private set; }

        public bool SettingsOpen { get; private set; }

        const string LOGOUT = "Logout";
        const int panelHeight = 40;

        public Action onLogoutClicked = null;
        public Action onSettingsClicked = null;
        public Action onAutoTweet = null;

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
                Image = Properties.Resources.autoTweetGrey,
                Size = new Size(panelHeight, panelHeight),
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Left
            };

            autoTweet.Click += (o, e) => {
                if (onAutoTweet != null)
                    onAutoTweet.Invoke();

                AutoTweet = !AutoTweet;
                autoTweet.Image = (AutoTweet) ? Properties.Resources.autoTweet : Properties.Resources.autoTweetGrey;
            };

            this.Controls.Add(autoTweet);

            settings = new PictureBox()
            {
                Image = Properties.Resources.settings,
                Size = new Size(panelHeight, panelHeight),
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Left
            };

            settings.Click += (o, e) => {
                if (onSettingsClicked != null)
                    onSettingsClicked.Invoke();

                SettingsOpen = !SettingsOpen;
                settings.Image = (!SettingsOpen) ? Properties.Resources.settings : Properties.Resources.settingsClose;
            };


            this.Controls.Add(settings);
            this.Controls.Add(avatar);
            this.Controls.Add(login);
        }

    }
}
