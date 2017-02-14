using System.Windows.Forms;
using com.pfsoft.proftrading.commons.external;
using System.Drawing;
using PTLRuntime.NETScript;
using System.Threading;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TradeTweet
{
    [Exportable]
    public partial class TradeTweet : Form, IExternalComponent, ITradeComponent
    {
        TwittwerService ts;
        TweetPanel tw;
        static NoticePanel noticePanel;
        CancellationToken ct;

        const string LOGIN = "Login";
        const string LOGOUT = "Logout";

        public TradeTweet()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(3000);
            ct = cts.Token;

            ts = new TwittwerService("822113440844148738-s7MLex2gcSFKxzKZfBDwcwJqvJYk0LA", "8UYP6Ahmn5GjJXkr0bN3Jy2XmKBX8jT3Slxk8EhzLCEmO");
            ts.OnNewEvent += Ts_NewEvents;

            tw = new TweetPanel();

            tw.OnConnect = OnTweet;

            noticePanel = new NoticePanel();

            this.Controls.Add(noticePanel);
            this.Controls.Add(tw);
        }

        private async void OnTweet()
        {
            tw.ToggleTweetButton();
            noticePanel.ShowNotice("Sending...");

            var images = tw.GetImages();

            List<Task<string>> list = new List<Task<string>>();

            foreach (var img in images)
            {
                list.Add(ts.SendImageAsync(img, ct));
            }

            await Task.WhenAll(list);

            var mediaIds =
                (from tsk in list
                 select tsk.Result.Split(new char[] { ':', ',' })[1]);

            string mediaString = string.Join(",", mediaIds);

            byte[] media = (string.IsNullOrEmpty(mediaString)) ? null : new byte[1];

            await ts.SendTweetAsync(new Twitt { Text = tw.Status, Media = media }, mediaString, ct).ContinueWith((t) => 
            {
                noticePanel.ShowNotice("Done!");
                tw.ToggleTweetButton();
            });
        }

        private void Ts_NewEvents(object sender, System.EventArgs e)
        {
            
        }

        private async void cheersBtn_Click(object sender, System.EventArgs e)
        {
            //if (ts == null) return;

            ////OpenFileDialog ofd = new OpenFileDialog();

            //string media = null;

            ////if (ofd.ShowDialog() == DialogResult.OK)
            ////{
            ////    media = ofd.FileName;
            ////}

            //var text = messageTB.Text;
            //Twitt t = new Twitt() { Text = text, Media = media };

            //CancellationTokenSource cts = new CancellationTokenSource();
            //cts.CancelAfter(3000);

            //var task = ts.SendTweetAsync(t, cts.Token);
            //var res = await task;

            //if (task.IsCompleted)
            //{
            //    MessageBox.Show("Result: \n"+res);
            //    messageTB.Text = string.Empty;
            //}
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

        //private void loginLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    if (ts.Connected)
        //    {
        //        ts.Disconnect();
        //        loginLabel.Text = LOGIN;
        //        loginLabel.LinkArea = new LinkArea(0, LOGIN.Length);
        //    }
        //    else
        //    {
        //        Response resp = ts.Connect();

        //        if (resp.Failed)
        //        {
        //            MessageBox.Show(resp.Text, "Connection error:");
        //            return;
        //        }

        //        var pin = PinForm.ShowForm();

        //        resp = ts.SetToken(pin);

        //        if (resp.Failed)
        //        {
        //            MessageBox.Show(resp.Text, "Pin error:");
        //            return;
        //        }

        //        loginLabel.Text = ts.UserName +", "+ LOGOUT;
        //        loginLabel.LinkArea = new LinkArea(loginLabel.Text.Length - LOGOUT.Length, LOGOUT.Length);
        //    }
        //}

        //private async void button1_Click(object sender, System.EventArgs e)
        //{
        //    messageTB.MaxLength = int.MaxValue;
        //    messageTB.Multiline = true;
        //    messageTB.ScrollBars = ScrollBars.Vertical;
        //    ts.M( mes => 
        //    { 
        //        messageTB.Text += mes + "\n";
        //    });
  
        //}
    }
}
