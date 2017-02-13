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

        public string Status { get { return messageText.Text; } }

        public List<Image> GetImages()
        {
            List<Image> list = new List<Image>();

            foreach (PictureBox item in picPanel.Controls.OfType<PictureBox>())
            {
                if (item.Image != null)
                    list.Add(item.Image);   
            }

            return list;
        } 

        const int maxLength = 140;
        const string ENTER_TWEET = "Type here...";
        const string btnText = "Tweet";

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
                connectBtn.Text = btnText + " [" + (length) + "]";
                connectBtn.Enabled = length > 0 && length <= maxLength;
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
                ForeColor = Color.DimGray,
                BackColor = Color.LightGray,
                Font = Settings.mainFont,
                Multiline = true,
                TextAlign = HorizontalAlignment.Left,
                Text = ENTER_TWEET,
                MaxLength = maxLength,
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
                Text = btnText,
                Height = Settings.btnHeight,
                BackColor = Settings.mainBackColor,
                ForeColor = Settings.mainFontColor,
                Enabled = false
            };


            this.Controls.Add(messageText);
            this.Controls.Add(picPanel);
            this.Controls.Add(connectBtn);
        }

        
    }
}
