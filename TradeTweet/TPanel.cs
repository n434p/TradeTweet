using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PTLRuntime.NETScript;
using TradeTweet.Properties;
using PTLRuntime.NETScript.Application;
using System.Diagnostics;

namespace TradeTweet
{
    internal partial class TPanel : UserControl
    {
        ToolTip tip;
        AutoSettings settingsPanel;
        TwittwerService ts;
        PicturePanel picPanel;
        NoticeP noticePanel;
        NoticeP2 nnnPanel;

        System.Threading.CancellationTokenSource ctss;
        System.Threading.CancellationToken ct;

        public string Status { get { return tweetText.Text; } }

        public Action OnLogout = null;

        const int MAX_TWEET_LENGTH = 140;
        const string ENTER_TWEET = "Type here...";
        const string BTN_TEXT = "Tweet";
        const int statusPanelHeight = 40;
        const int CARD_ZIZE = 64;
        const int MARGIN = 5;
        const int MAX_CARDS = 4;


        public bool AutoTweetFlag
        {
            get
            {
                return Settings.autoTweet;
            }
            set
            {
                autoTweetBtn.Image = (value) ? Properties.Resources.TradeTweet_20 : Properties.Resources.TradeTweet_21;
                Settings.autoTweet = value;
            }
        }

        public bool SettingsOpen
        {
            get { return so; }
            set
            {
                settingsBtn.Image = (value) ? Resources.TradeTweet_23 : Resources.TradeTweet_24;
                so = value;
            }
        }
        bool so = false;

        public TPanel()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            histPanelContainer.Controls.Add(historyPanel);

            scrollHistContainer.Controls.Add(histPanelContainer);
            scrollHistContainer.Controls.Add(scroll);

            historyPanel.Resize += (o, e) =>
            {
                Debug.Print("h:" + historyPanel.PreferredSize.Height +
                 " historyPanel.VerticalScroll.Maximum " + historyPanel.VerticalScroll.Maximum +
                 " scroll.Maximum" + scroll.Maximum +
                 " scroll.LargeChange" + scroll.LargeChange);

                var h = historyPanel.PreferredSize.Height;
                historyPanel.VerticalScroll.Maximum = h;
                scroll.Maximum = h;
                scroll.LargeChange = h / historyPanel.Height + scroll.Height;

                Debug.Print(">>> "+
                    " historyPanel.VerticalScroll.Maximum " + historyPanel.VerticalScroll.Maximum +
                    " scroll.Maximum" + scroll.Maximum +
                    " scroll.LargeChange" + scroll.LargeChange);
            };

            historyPanel.MouseWheel += Scroll_Scroll;

            scroll.Scroll += Scroll_Scroll;

            this.Margin = new Padding(0);
            this.Padding = new Padding(0);

            tip = new ToolTip();

            tip.SetToolTip(logoutLink, "Click to logout");
            tip.SetToolTip(autoTweetBtn, "AutoTweet turn on/off");
            tip.SetToolTip(settingsBtn, "AutoTweet Settings");
            tip.SetToolTip(addImageBtn, "Add Image");
            tip.SetToolTip(makeScreenshotBtn, "Make Screenshot");

            picPanelBase.Visible = false;

            picPanel = new PicturePanel();
            picPanelContainer.Controls.Add(picPanel);

            noticePanel = new NoticeP(historyPanel);
            noticePanel.mouseMoved += () => historyPanel.RefreshState(); 

            nnnPanel = new NoticeP2(this);
            this.Controls.Add(nnnPanel);
            nnnPanel.BringToFront();

            settingsPanel = new AutoSettings();
            settingsPanel.Visible = false;

            this.Controls.Add(settingsPanel);

            Settings.onSettingsChanged += () => 
            {
                AutoTweetFlag = Settings.autoTweet;
            };

            AutoTweetFlag = Settings.autoTweet;

            /// set notice panel location
            RelocationPanels();

        }

        private void Scroll_Scroll(object sender, EventArgs e)
        {
            //prevent vertical scrollbar blinking on change
            //historyPanel.AutoScroll = false;
            //historyPanel.AutoScrollMinSize = new Size(0, 0);

            if (e is MouseEventArgs)
            {
                int wheelMovement = SystemInformation.MouseWheelScrollDelta;
                //scroll.Value = ((e as MouseEventArgs).Delta / wheelMovement) * 5;
                scroll.Value = (((e as MouseEventArgs).Delta < 0) ? 1 : -1);
            }

            //historyPanel.VerticalScroll.Visible = false;
            //historyPanel.VerticalScroll.Minimum = 0;
            
            historyPanel.AutoScrollPosition = new Point(0, scroll.Value);
            Debug.Print(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> " + historyPanel.AutoScrollPosition);


            //historyPanel.val = scroll.Value;
            //historyPanel.Refresh();

            //historyPanel.AutoScroll = false;
            //historyPanel.AutoScrollMinSize = new Size(1, 1);

            //historyPanel.AutoScroll = false;


            //  historyPanel.ChangeLocation(0, scroll.Value);

            scroll.Invalidate();
            Application.DoEvents();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (Width <= 400)
            {
                gridPanel.ColumnStyles[0].SizeType = SizeType.AutoSize;
                gridPanel.ColumnStyles[2].SizeType = SizeType.AutoSize;
            }
            else
            {
                gridPanel.ColumnStyles[0].SizeType = SizeType.Percent;
                gridPanel.ColumnStyles[0].Width = 100;
                gridPanel.ColumnStyles[2].SizeType = SizeType.Percent;
                gridPanel.ColumnStyles[2].Width = 100;
            }

            RelocationPanels();
        }

        void RelocationPanels()
        {
            if (settingsPanel != null && settingsPanel.Visible)
            {
                settingsPanel.Location = new Point(headerPanel.Left + autoTweetBtn.Left + 3, headerPanel.Bottom - 7);
                settingsPanel.BringToFront();
            }

            if (nnnPanel != null)
            {
                nnnPanel.Location = new Point(headerPanel.Left+3, headerPanel.Bottom+3);
            }
        }

        public void TweetPanel(NETSDK platformEngine)
        {
            ts = AutoTweet.Run(platformEngine);
    
            ctss = new System.Threading.CancellationTokenSource();
            ct = ctss.Token;

            accountName.Name = ts.User.screen_name;
            avatar.BackgroundImage = ts.User.avatar;
            avatar.BackgroundImageLayout = ImageLayout.Zoom;

            //Populate();

            //statusPanel.onSettingsClicked = () =>
            //{
            //    settingsPanel.ShowSet();
            //    // await M();
            //};


            //settingsPanel.OnApply = () =>
            //{
            //    AutoTweet.LinkEvents();

            //    if (!settingsPanel.HasEvents)
            //    {
            //        AutoTweetFlag = false;
            //        ShowNotice("AutoTweet Stopped!");
            //        return;
            //    }

            //    foreach (EventType item in Enum.GetValues(typeof(EventType)))
            //    {
            //        Settings.Set[item] = settingsPanel[item];
            //    }

            //    if (OnSettingsApplied != null)
            //        OnSettingsApplied.Invoke();

            //    ShowNotice("Settings applied!");
            //};
        }

        private async Task<Response> OnTweet()
        {
            ToggleTweetButton();
            nnnPanel.ShowNotice("Sending...");

            List<Task<Response>> list = new List<Task<Response>>();

            foreach (var img in images.Values)
            {
                list.Add(ts.SendImageAsync(img, ct));
            }

            await Task.WhenAll(list);

            if (list.Any(l => l.Result.Failed))
            {
                ctss.Cancel();
                return new Response() { Failed = true, Text = "Image sending error" };
            }

            var mediaIds =
                (from tsk in list
                 select tsk.Result.Text.Split(new char[] { ':', ',' })[1]);

            string mediaString = string.Join(",", mediaIds);

            byte[] media = (string.IsNullOrEmpty(mediaString)) ? null : new byte[1];

            var ttt = await ts.SendTweetAsync(new Twitt { Text = Status, Media = media }, mediaString, ct);

            ResponseNotice(Status, ttt, EventType.Empty);

            ToggleTweetButton();

            return ttt;
        }

        void ResponseNotice(string text, Response resp, EventType type)
        {
            if (!resp.Failed)
            {
                noticePanel.ShowNotice(text, NoticeType.Success, type);
            }
            else
            {
                noticePanel.ShowNotice(resp.Text, NoticeType.Error, type);
            }
        }

        private void logoutLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(OnLogout != null)
                OnLogout.Invoke();
        }

        private async void SendTweetAsync()
        {
            var rrr = await OnTweet();

            if (!rrr.Failed)
            {
                tweetText.Text = "";
                picPanel.Clear();
                images.Clear();
                TogglePicPanel();
            }
        }

        public void ToggleTweetButton()
        {
            tweetBtn.Enabled = tweetBtn.Enabled;
            tweetBtn.Image = (tweetBtn.Enabled) ? Resources.TradeTweet_13 : Resources.TradeTweet_12;
        }

        private void tweetText_KeyDown(object sender, KeyEventArgs e)
        {
            tweetText.Text = string.Empty;
            tweetText.KeyDown -= tweetText_KeyDown;
        }

        private void tweetBtn_Click(object sender, EventArgs e)
        {
             SendTweetAsync();
        }

        private void tweetText_TextChanged(object sender, EventArgs e)
        {
            var length = MAX_TWEET_LENGTH - tweetText.TextLength;
            tweetLengthLabel.Text = length.ToString();
            tweetBtn.Enabled = length >= 0 && length < MAX_TWEET_LENGTH;
            tweetBtn.Image = (tweetBtn.Enabled) ? Resources.TradeTweet_13 : Resources.TradeTweet_12;
        }

        private void settingsBtn_Click(object sender, EventArgs e)
        {
            SettingsOpen = !SettingsOpen;
            settingsPanel.Visible = SettingsOpen;

            RelocationPanels();
        }

        private void autoTweetBtn_Click(object sender, EventArgs e)
        {
            if (!Settings.Set.Values.Any(v => v.Active))
            {
                AutoTweetFlag = false;
                nnnPanel.ShowNotice("There is no events to tweet!",1000);
                return;
            }

            AutoTweetFlag = !AutoTweetFlag;

            // Save settings
            Settings.OnSettingsChange();

            nnnPanel.ShowNotice((Settings.autoTweet) ? "AutoTweet Enabled!" : "AutoTweet Disabled!", 1000, NoticeType.Info);
        }

        internal void NoticeSubscribing()
        {
            if (Settings.autoTweet)
            {
                AutoTweet.OnAutoTweetSend += ShowInfoNotice;
                AutoTweet.OnAutoTweetRespond += ResponseNotice;
            }
            else
            {
                AutoTweet.OnAutoTweetSend -= ShowInfoNotice;
                AutoTweet.OnAutoTweetRespond -= ResponseNotice;
            }
        }

        private void ShowInfoNotice(string text, EventType type)
        {
            nnnPanel.ShowNotice(text, 1000, NoticeType.Info, type);
        }
    
        private void addImageBtn_Click(object sender, EventArgs e)
        {
            ProcessClick(picMode.Image);
        }

        private void makeScreenshotBtn_Click(object sender, EventArgs e)
        {
            ProcessClick(picMode.ScreenShot);
        }

        void TogglePicPanel()
        {
            bool state = picPanel.Controls.Count != 0;
            picPanelBase.Visible = state;
            separatorLine.BackColor = (state) ? Color.FromArgb(34, 34, 34) : Color.FromArgb(0, 0, 0);
        }

        public void ProcessClick(picMode mode)
        {
            if (picPanel.Controls.Count < MAX_CARDS)
            {
                Action<PictureCard> act = null;

                switch (mode)
                {
                    case picMode.Image:
                        act = OpenImage;
                        break;
                    case picMode.ScreenShot:
                        act = MakeScreen;
                        break;
                }

                if(act != null)
                    act.Invoke(null);

                TogglePicPanel();
                return;
            }

            nnnPanel.ShowNotice("Only 4 pics are allowed to tweet!", 1000, NoticeType.Info);
        }

        void MakeScreen(PictureCard c = null)
        {
            Image img = Terminal.MakeScreenshot(Form.ActiveForm.DisplayRectangle, Size.Empty);

            if (img == null) return;

            if (c != null)
            {
                c.BackgroundImage = c.ResizeImage(img, CARD_ZIZE);
                images[c] = img;
                return;
            }

            PictureCard pc = new PictureCard(img);

            pc.onClose = () =>
            {
                RemoveImage(pc);
                TogglePicPanel();
            };

            pc.onClick = () =>
            {
                MakeScreen(pc);
            };

            AddImage(pc, img);
        }

        Dictionary<PictureCard, Image> images = new Dictionary<PictureCard, Image>();

        void AddImage(PictureCard pc, Image img)
        {
            picPanel.Controls.Add(pc);
            images[pc] = img;
            picPanel.ReplaceOrder();
        }

        void RemoveImage(PictureCard pc)
        {
            picPanel.Controls.Remove(pc);
            images.Remove(pc);
            picPanel.ReplaceOrder();
        }

        void OpenImage(PictureCard c = null)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (ofd.CheckFileExists)
                {
                    Image img = Image.FromFile(ofd.FileName);

                    if (c != null)
                    {
                        c.BackgroundImage = c.ResizeImage(img, CARD_ZIZE);
                        images[c] = img;
                        return;
                    }

                    PictureCard pc = new PictureCard(img);

                    pc.onClose = () =>
                    {
                        RemoveImage(pc);
                        TogglePicPanel();
                    };

                    pc.onClick = () =>
                    {
                        OpenImage(pc);
                    };

                    AddImage(pc, img);
                }
            }
        }
   
        class PictureCard : PictureBox
        {
            ToolTip tip;
            Label cross;

            public Action onClose;
            public Action onClick;

            public PictureCard(Image img = null)
            {
                Width = CARD_ZIZE;
                Height = CARD_ZIZE;
                Cursor = Cursors.Hand;

                this.SizeMode = PictureBoxSizeMode.Zoom;

                Image = Properties.Resources.TradeTweet_30;
                BackgroundImage = ResizeImage(img, CARD_ZIZE);
                BackgroundImageLayout = ImageLayout.Center;
                BackColor = Color.Transparent;


                cross = new Label()
                {
                    BackColor = Color.Transparent,
                    Size = new Size(10, 10),
                    Location = new Point(48, 5),
                    Anchor = AnchorStyles.Left | AnchorStyles.Top
                };

                cross.Click += (o, e) => {
                    if (onClose != null)
                        onClose.Invoke();
                };


                tip = new ToolTip();

                tip.SetToolTip(this, "Click to change");
                tip.SetToolTip(cross, "Remove");

                this.Controls.Add(cross);
            }

            public Image ResizeImage(Image image, int minSize)
            {
                Image resizedImg = null;

                var width = image.Width;
                var height = image.Height;

                var biggestSide = (int)(Math.Max(width, height) * minSize / Math.Min(width, height));

                if (width >= height)
                {
                    resizedImg = (Image)(new Bitmap(image, biggestSide, minSize));
                }
                else
                {
                    resizedImg = (Image)(new Bitmap(image, minSize, biggestSide));
                }

                var part = new Bitmap(minSize, minSize);

                using (var g = Graphics.FromImage(part))
                {
                    g.DrawImageUnscaled(resizedImg, (minSize - resizedImg.Width) / 2, (minSize - resizedImg.Height) / 2);
                }

                return part;
            }

            protected override void OnClick(EventArgs e)
            {
                if (onClick != null)
                    onClick.Invoke();
            }
        }

        private void makeScreenshotBtn_MouseEnter(object sender, EventArgs e)
        {
            makeScreenshotBtn.Image = Properties.Resources.TradeTweet_08;
        }

        private void makeScreenshotBtn_MouseLeave(object sender, EventArgs e)
        {
            makeScreenshotBtn.Image = Properties.Resources.TradeTweet_06;
        }

        private void addImageBtn_MouseEnter(object sender, EventArgs e)
        {
            addImageBtn.Image = Properties.Resources.TradeTweet_07;
        }

        private void addImageBtn_MouseLeave(object sender, EventArgs e)
        {
            addImageBtn.Image = Properties.Resources.TradeTweet_05;
        }

        internal class CustomPanel : FlowLayoutPanel
        {
            internal int val = 0;

            public CustomPanel()
            {
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

                this.FlowDirection = FlowDirection.TopDown;
                this.DoubleBuffered = true;
                this.AutoSize = true;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                this.Location = new System.Drawing.Point(32, 48);
                this.Margin = new System.Windows.Forms.Padding(0);
                this.Size = new System.Drawing.Size(400, 98);
                this.WrapContents = false;

                this.AutoScroll = false;
                this.HorizontalScroll.Maximum = 0;
                this.HorizontalScroll.Visible = false;
                this.VerticalScroll.Maximum = 0;
                this.VerticalScroll.Visible = false;
                this.AutoScroll = true;
            }

            internal void RefreshState()
            {
                foreach (NoticeP item in Controls)
                {
                    item.RefreshRemoveabled();
                }
            }

            internal void ChangeLocation(int x, int y)
            {
                this.SetDisplayRectLocation(x, y);
            }
        }

        internal class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
            {
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

                this.DoubleBuffered = true;

                this.AutoScroll = false;

                this.HorizontalScroll.Visible = false;
                this.HorizontalScroll.Minimum = 0;
                this.HorizontalScroll.Maximum = 0;

                this.VerticalScroll.Visible = false;
                this.VerticalScroll.Minimum = 0;
                this.VerticalScroll.Maximum = 100;

                this.AutoSize = true;
                this.BackgroundImage = global::TradeTweet.Properties.Resources.factura;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                this.Location = new System.Drawing.Point(32, 48);
                this.Margin = new System.Windows.Forms.Padding(0);
                this.Size = new System.Drawing.Size(400, 98);
            }


        }

    }
}
