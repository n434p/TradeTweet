using System;
using System.Drawing;
using System.Windows.Forms;

namespace TradeTweet
{
    class StatusPanel : Panel
    {
        LinkLabel login;
        PictureBox avatar;
        Label settings;

        const string LOGOUT = "Logout";
        const int panelHeight = 40;

        public Action onLogoutClicked = null;

        public StatusPanel(TwittwerService ts)
        {
            this.Padding = new Padding(5);

            string text = ts.UserName.screen_name + ", " + LOGOUT;

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
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                VisitedLinkColor = System.Drawing.Color.DarkOrange
            };

            avatar = new PictureBox()
            {
                BackgroundImageLayout = ImageLayout.Zoom,
                Image = Properties.Resources.avatar,
                BackgroundImage = ts.UserName.avatar,
                Size = new Size(panelHeight, panelHeight),
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Right
            };

            login.LinkArea = new LinkArea(login.Text.Length - LOGOUT.Length, LOGOUT.Length);

            login.LinkClicked += (o, e) => {
                if (onLogoutClicked != null)
                    onLogoutClicked.Invoke();
            };

            this.Controls.Add(avatar);
            this.Controls.Add(login);
        }
    
    }
}
