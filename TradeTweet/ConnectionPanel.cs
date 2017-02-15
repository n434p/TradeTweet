using System;
using System.Windows.Forms;

namespace TradeTweet
{
    class ConnectionPanel: Panel
    {
        Button connectBtn;
        Label connectMessage;
        public Action OnConnect = null;

        public ConnectionPanel()
        {
            this.Dock = DockStyle.Fill;
           // this.Anchor = AnchorStyles.None;

            Populate();

            connectBtn.Click += (o,e) => 
            {
                if (OnConnect != null)
                    OnConnect.Invoke();
            };
        }

        string message = $"{Settings.appName} is offline \n You need to connect to Twitter.";
        const string btnText = "Connect";

        private void Populate()
        {
            connectMessage = new Label()
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = Settings.mainFont,
                ForeColor = Settings.mainFontColor
            };

            connectBtn = new Button()
            {
                FlatStyle = FlatStyle.Flat,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = Settings.mainFont,
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Bottom,
                Text = btnText,
                Height = Settings.btnHeight,
                BackColor = Settings.mainFontColor,
                ForeColor = Settings.mainBackColor             
            };

            this.Controls.Add(connectMessage);
            this.Controls.Add(connectBtn);
        }
    }
}
