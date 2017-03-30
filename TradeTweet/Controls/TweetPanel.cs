﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeTweet.Properties;
using PTLRuntime.NETScript.Application;

namespace TradeTweet
{
    internal partial class TweetPanel : UserControl
    {
        ToolTip tip;
        AutoSettings settingsPanel;
        PicturePanel picPanel;
        MessagePanel messagePanel;
        NoticePanel noticePanel;

        System.Threading.CancellationTokenSource ctss;
        System.Threading.CancellationToken ct;

        public string Status { get { return tweetText.Text; } }

        public Action OnLogout = null;

        const int MAX_TWEET_LENGTH = 140;
        const string ENTER_TWEET = "Type here...";
        const string BTN_TEXT = "Tweet";
        const int STATUS_PANEL_HEIGHT = 40;
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
                settingsPanel.Visible = value;
                so = value;
            }
        }
        bool so = false;

        public TweetPanel()
        {
            InitializeComponent();

            this.DoubleBuffered = true;

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

            accountName.Text = AutoTweet.twitService.User.screen_name;
            avatar.BackgroundImage = AutoTweet.twitService.User.avatar;
            avatar.BackgroundImageLayout = ImageLayout.Zoom;

            messagePanel = new MessagePanel(scrollHistContainer.panel);
            messagePanel.mouseMoved += () => scrollHistContainer.RefreshState(); 

            noticePanel = new NoticePanel(this, ct);
            this.Controls.Add(noticePanel);
            noticePanel.BringToFront();

            settingsPanel = new AutoSettings();
            settingsPanel.Visible = false;

            this.Controls.Add(settingsPanel);

            Settings.onSettingsChanged += () => 
            {
                AutoTweetFlag = Settings.autoTweet;
            };

            AutoTweetFlag = Settings.autoTweet;

            ctss = new System.Threading.CancellationTokenSource();
            ct = ctss.Token;

            AutoTweet.OnAutoTweetSend += ShowInfoNotice;
            AutoTweet.OnAutoTweetRespond += ResponseNotice;

            /// set notice panel location
            RelocationPanels();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 528) //on left mouse button up
            {
                // settings is opened
                if (SettingsOpen)
                {
                    var loc = new Point(headerPanel.Left + autoTweetBtn.Left + 3, headerPanel.Bottom - 7);
                    var rect = new Rectangle(loc, settingsPanel.Size);

                    var loc2 = new Point(loc.X + autoTweetBtn.Width, loc.Y - settingsBtn.Height - 2);
                    var rect2 = new Rectangle(loc2, settingsBtn.Size);

                    if (!rect.Contains(PointToClient(Control.MousePosition)) && !rect2.Contains(PointToClient(Control.MousePosition)))
                    {
                        SettingsOpen = false;
                    }
                }
            }

            base.WndProc(ref m);
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

            if (noticePanel != null)
            {
                noticePanel.Location = new Point(headerPanel.Left+3, headerPanel.Bottom+3);
            }
        }

        private async Task<Response> OnTweet()
        {
            List<Task<Response>> list = new List<Task<Response>>();

            foreach (var img in images.Values)
            {
                list.Add(AutoTweet.twitService.SendImageAsync(img, ct));
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

            var ttt = await AutoTweet.twitService.SendTweetAsync(new Twitt { Text = Status, Media = media }, mediaString, ct);

            return ttt;
        }

        void ResponseNotice(TwitMessage msg, Response resp)
        {
            if (!resp.Failed)
            {
                msg.NoticeType = NoticeType.Success;
            }
            else
            {
                msg.NoticeType = NoticeType.Error;
                msg.Message = resp.Text;
            }

            messagePanel.ShowNotice(msg);
        }

        private void logoutLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(OnLogout != null)
                OnLogout.Invoke();
        }

        private async void SendTweetAsync()
        {
            ToggleTweetButton();

            noticePanel.ShowNotice("Sending...");

            var rrr = await OnTweet();

            ToggleTweetButton();

            if (!rrr.Failed)
            {
                var msg = new TwitMessage() { Message = Status, Time = DateTime.UtcNow };
                ResponseNotice(msg, rrr);

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
            RelocationPanels();
        }

        private void autoTweetBtn_Click(object sender, EventArgs e)
        {
            if (!Settings.Set.Values.Any(v => v.Active))
            {
                AutoTweetFlag = false;
                noticePanel.ShowNotice("There is no events to tweet!",1000);
                return;
            }

            AutoTweetFlag = !AutoTweetFlag;

            // Save settings
            Settings.OnSettingsChange();

            noticePanel.ShowNotice((Settings.autoTweet) ? "AutoTweet Enabled!" : "AutoTweet Disabled!", 1000, NoticeType.Info);
        }

        private void ShowInfoNotice(string text, EventType type)
        {
            noticePanel.ShowNotice(text, 1000, NoticeType.Info, type);
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

            noticePanel.ShowNotice("Only 4 pics are allowed to tweet!", 1000, NoticeType.Info);
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

        internal class HistoryPanel : FlowLayoutPanel 
        {
            public Action needRescrolling;

            public HistoryPanel(): base()
            {
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
                this.HorizontalScroll.Enabled = false;
                this.VerticalScroll.Maximum = 0;
                this.VerticalScroll.Visible = false;
                this.AutoScroll = true;
            }

            protected override void OnResize(EventArgs eventargs)
            {
                base.OnResize(eventargs);

                if (needRescrolling != null)
                    needRescrolling.Invoke();
            }

            protected override void OnControlAdded(ControlEventArgs e)
            {
                base.OnControlAdded(e);

                if (needRescrolling != null)
                    needRescrolling.Invoke();
            }

            protected override void OnControlRemoved(ControlEventArgs e)
            {
                base.OnControlRemoved(e);

                if (needRescrolling != null)
                    needRescrolling.Invoke();
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 133) // skip nonclient area repainting - for prevent scrollbars flickering on resizing
                    return;

                base.WndProc(ref m);
            }
        }

        internal class HistoryPanelContainer : DoubleBufferedPanel
        {
            const int SCROLL_WIDTH = 6;

            public HistoryPanel panel = new HistoryPanel();
            CustomVScrollbar scrollB = new CustomVScrollbar(SCROLL_WIDTH);

            public HistoryPanelContainer(): base()
            {
                DoubleBuffered = true;

                scrollB.Dock = System.Windows.Forms.DockStyle.Right;
                scrollB.LargeChange = 10;
                scrollB.Margin = new System.Windows.Forms.Padding(0);
                scrollB.Maximum = 100;
                scrollB.Minimum = 0;
                scrollB.Name = "scroll";
                scrollB.Size = new System.Drawing.Size(SCROLL_WIDTH, 150);
                scrollB.SmallChange = 1;
                scrollB.TabIndex = 0;
                scrollB.Value = 0;
                scrollB.Visible = false;

                scrollB.ThumbMoving += ScrollB_ThumbMoving;
                panel.MouseWheel += Panel_MouseWheel;
                panel.needRescrolling += needRescrolling;

                this.Controls.Add(panel);
                this.Controls.Add(scrollB);

                this.AutoScroll = false;
            }

            private void Panel_MouseWheel(object sender, MouseEventArgs e)
            {
                int wheelMovement = SystemInformation.MouseWheelScrollDelta;

                int v = (e.Delta < 0) ? 10 : -10;

                scrollB.Value = v;
                ScrollB_ThumbMoving(null, e);

                base.OnMouseWheel(e);
            }

            private void ScrollB_ThumbMoving(object sender, EventArgs e)
            {
                panel.AutoScrollPosition = new Point(0, scrollB.Value);
                scrollB.Invalidate();
            }

            void needRescrolling()
            {
                var h = panel.PreferredSize.Height;
                scrollB.Maximum = h;
                scrollB.LargeChange = h / scrollB.Height + scrollB.Height;

                scrollB.Visible = scrollB.ThumbHeight != 0;

                if (scrollB.Visible)
                {
                    // scroll to the bottom message
                    scrollB.Value = h;
                    ScrollB_ThumbMoving(null, EventArgs.Empty);
                }
            }

            internal void RefreshState()
            {
                foreach (MessagePanel item in panel.Controls)
                {
                    item.RefreshRemoveabled();
                }
            }
        }

        internal class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel(): base()
            {
                this.DoubleBuffered = true;

                this.AutoSize = true;
                this.BackgroundImage = Properties.Resources.factura;
                this.Dock = System.Windows.Forms.DockStyle.Fill;
                this.Location = new System.Drawing.Point(32, 48);
                this.Margin = new System.Windows.Forms.Padding(0);
                this.Size = new System.Drawing.Size(400, 98);
            }
        }
    }
}