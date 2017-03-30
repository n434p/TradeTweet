using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeTweet
{
    public partial class EnterPinPanel : UserControl
    {
        Button connectBtn;
        MaskedTextBox maskedPin;

        public Action<string> OnPinEntered = null;

        public EnterPinPanel()
        {
            InitializeComponent();

            this.textBox1.Text = "Enter your account details in Browser prompt than click on\n \"Authorize App\" button.\nWrite down PIN-code that will appear after:";

            maskedPin.Mask = "";
            maskedPin.TextAlign = HorizontalAlignment.Center;
            maskedPin.Text = ENTER_PIN;

            this.Dock = DockStyle.Fill;

            connectBtn.Enabled = false;


            connectBtn.Click += (o, e) =>
            {
                connectBtn.Enabled = false;

                if (OnPinEntered != null)
                    OnPinEntered.Invoke(maskedPin.Text);
            };

            connectBtn.MouseEnter += (o, e) =>
            {
                connectBtn.BackgroundImage = Properties.Resources.TradeTweet_27;
            };

            connectBtn.MouseLeave += (o, e) =>
            {
                connectBtn.BackgroundImage = Properties.Resources.TradeTweet_26;
            };

            maskedPin.GotFocus += MaskedPin_GotFocus;

            maskedPin.Enter += (o, e) =>
            {
                maskedPin.Select(0, 0);
            };

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

        const string ENTER_PIN = "Enter PIN here...";

    }
}
