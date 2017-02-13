using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeTweet
{
    class EnterPinPanel : Panel
    {
        Button connectBtn;
        Label connectMessage;
        MaskedTextBox maskedPin;

        public Action OnConnect = null;

        public EnterPinPanel()
        {
            this.Dock = DockStyle.Fill;
            // this.Anchor = AnchorStyles.None;

            Populate();

            connectBtn.Click += (o, e) =>
            {
                if (OnConnect != null)
                    OnConnect.Invoke();
            };

            maskedPin.GotFocus += MaskedPin_GotFocus;

            maskedPin.TextChanged += (o, e) => 
            {
                connectBtn.Enabled = maskedPin.Mask != "" && maskedPin.MaskCompleted;
            };
        }

        private void MaskedPin_GotFocus(object sender, EventArgs e)
        {
                if (maskedPin.Text == ENTER_PIN)
                {
                    maskedPin.Text = "";
                    maskedPin.Mask = "0000000";
                    maskedPin.GotFocus -= MaskedPin_GotFocus;
                }
        }

        string message = $"{Settings.appName} isn't authorized. \n\n Login in Twitter via Browser prompt than click on \"Authorize App\" button and write down PIN-code that will appear after:";
        string btnText = $"Authorize {Settings.appName}";
        const string ENTER_PIN = "Enter PIN here...";

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

            maskedPin = new MaskedTextBox()
            {
                Mask = "",
                ForeColor = Color.DimGray,
                BackColor = Color.LightGray,
                Font = Settings.mainFont,
                TextAlign = HorizontalAlignment.Center,
                Text = ENTER_PIN,
                Dock = DockStyle.Bottom
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
                BackColor = Settings.mainBackColor,
                ForeColor = Settings.mainFontColor,
                Enabled = false
            };

            this.Controls.Add(connectMessage);
            this.Controls.Add(maskedPin);
            this.Controls.Add(connectBtn);
        }
    }
}
