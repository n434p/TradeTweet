using System.Windows.Forms;
using com.pfsoft.proftrading.commons.external;
using System.Drawing;
using PTLRuntime.NETScript;
using System.Threading;

namespace TradeTweet
{
    [Exportable]
    public partial class TradeTweet : Form, IExternalComponent, ITradeComponent
    {
        TwittwerService ts;

        const string LOGIN = "Login";
        const string LOGOUT = "Logout";

        public TradeTweet()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            ts = new TwittwerService("822113440844148738-s7MLex2gcSFKxzKZfBDwcwJqvJYk0LA", "8UYP6Ahmn5GjJXkr0bN3Jy2XmKBX8jT3Slxk8EhzLCEmO");
        }

        private async void cheersBtn_Click(object sender, System.EventArgs e)
        {
            if (ts == null) return;

            //OpenFileDialog ofd = new OpenFileDialog();

            string media = null;

            //if (ofd.ShowDialog() == DialogResult.OK)
            //{
            //    media = ofd.FileName;
            //}

            var text = messageTB.Text;
            Twitt t = new Twitt() { Text = text, Media = media };

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(3000);

            var task = ts.SendTweetAsync(t, cts.Token);
            var res = await task;

            if (task.IsCompleted)
            {
                MessageBox.Show("Result: \n"+res);
                messageTB.Text = string.Empty;
            }
        }

        #region IExternalComponent
        public Icon IconImage
        {
            get { return this.Icon; }
        }

        public string ComponentName
        {
            get { return "TradeTweet"; }
        }

        public string PanelHeader
        {
            get { return "TradeTweet"; }
        }

        public Control Content
        {
            get { return this; }
        }

        public NETSDK PlatformEngine
        {
            get;
            set;
        }

        public void Populate()
        {
            
        }

        #endregion

        private void loginLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (ts.Connected)
            {
                ts.Disconnect();
                loginLabel.Text = LOGIN;
                loginLabel.LinkArea = new LinkArea(0, LOGIN.Length);
            }
            else
            {
                Response resp = ts.Connect();

                if (resp.Failed)
                {
                    MessageBox.Show(resp.Text, "Connection error:");
                    return;
                }

                var pin = PinForm.ShowForm();

                resp = ts.SetToken(pin);

                if (resp.Failed)
                {
                    MessageBox.Show(resp.Text, "Pin error:");
                    return;
                }

                loginLabel.Text = ts.UserName +", "+ LOGOUT;
                loginLabel.LinkArea = new LinkArea(loginLabel.Text.Length - LOGOUT.Length, LOGOUT.Length);
            }
        }

        private async void button1_Click(object sender, System.EventArgs e)
        {
            messageTB.MaxLength = int.MaxValue;
            messageTB.Multiline = true;
            ts.M( mes => 
            { 
                messageTB.Text += mes + "\n";
            });
  
        }
    }
}
