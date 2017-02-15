using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace TradeTweet
{
    class TweetPanel : Panel
    {
        TwittwerService ts;
        StatusPanel statusPanel;
        PicturePanel picPanel;
        Button connectBtn;
        TextBox messageText;

        public Action OnLogout = null;

        const int MAX_TWEET_LENGTH = 140;
        const string ENTER_TWEET = "Type here...";
        const string BTN_TEXT = "Tweet";
        const int statusPanelHeight = 40;
        const int CARD_ZIZE = 80;
        const int MARGIN = 5;

        public string Status { get { return messageText.Text; } }

        public void ToggleTweetButton()
        {
            connectBtn.Enabled = !connectBtn.Enabled;

            connectBtn.BackColor = (connectBtn.Enabled) ? Settings.mainFontColor : Color.Gray;
            connectBtn.ForeColor = (connectBtn.Enabled) ? Settings.mainBackColor : Color.Black;
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

        public Action OnConnect = null;

        public TweetPanel(TwittwerService ts)
        {
            this.ts = ts;
            this.Dock = DockStyle.Fill;

            Populate();

            connectBtn.Click += (o, e) =>
            {
                if (OnConnect != null)
                    OnConnect.Invoke();
            };

            messageText.KeyDown += MessageText_KeyDown;

            messageText.TextChanged += (o, e) =>
            {
                var length = messageText.TextLength;
                connectBtn.Text = BTN_TEXT + " [" + (length) + "]";
                connectBtn.Enabled = length > 0 && length <= MAX_TWEET_LENGTH;
                connectBtn.BackColor = (connectBtn.Enabled) ? Settings.mainFontColor : Color.Gray;
            };
        }

        private void MessageText_KeyDown(object sender, KeyEventArgs e)
        {
            messageText.Text = string.Empty;
            messageText.KeyDown -= MessageText_KeyDown;
        }

        private void Populate()
        {
            messageText = new TextBox()
            {
                ScrollBars = ScrollBars.Vertical,
                HideSelection = true,
                ForeColor = Color.DimGray,
                BackColor = Color.LightGray,
                Font = Settings.mainFont,
                Multiline = true,
                TextAlign = HorizontalAlignment.Left,
                Text = ENTER_TWEET,
                MaxLength = MAX_TWEET_LENGTH,
                Dock = DockStyle.Fill
            };
            this.Controls.Add(messageText);

            statusPanel = new StatusPanel(ts)
            {
                Height = statusPanelHeight,
                BackColor = Settings.mainBackColor,
                Dock = DockStyle.Top
            };

            statusPanel.onLogoutClicked = () =>
            {
                if (OnLogout != null)
                    OnLogout.Invoke();
            };

            this.Controls.Add(statusPanel);


            picPanel = new PicturePanel()
            {
                Height = CARD_ZIZE + 2 * MARGIN,
                Dock = DockStyle.Bottom
            };
            this.Controls.Add(picPanel);

            connectBtn = new Button()
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
            this.Controls.Add(connectBtn);

        } 
    }
}
