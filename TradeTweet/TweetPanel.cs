using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace TradeTweet
{
    class TweetPanel : Panel
    {
        PicturePanel picPanel;
        Button connectBtn;
        TextBox messageText;

        const int MAX_TWEET_LENGTH = 140;
        const string ENTER_TWEET = "Type here...";
        const string BTN_TEXT = "Tweet";

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

        public TweetPanel()
        {
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
                TabIndex = 0,
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

            picPanel = new PicturePanel();

            connectBtn = new Button()
            {
                TabIndex = 1,
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

            connectBtn.UseVisualStyleBackColor = true;

            this.Controls.Add(messageText);
            this.Controls.Add(picPanel);
            this.Controls.Add(connectBtn);
        } 
    }
}
