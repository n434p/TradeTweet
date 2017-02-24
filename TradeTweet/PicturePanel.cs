using PTLRuntime.NETScript.Application;
using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TradeTweet
{
    class PicturePanel : Panel
    {
        const int MAX_CARDS = 4;
        const int CARD_ZIZE = 80;
        const int MARGIN = 5;

        public Action OnMaxPics = null;

        public void Clear()
        {
            p2.Controls.Clear();
        }

        Panel p2;

        public List<Image> GetImages()
        {
            List<Image> list = new List<Image>();

            foreach (Control item in p2.Controls)
            {
                if (item.BackgroundImage != null)
                    list.Add(item.BackgroundImage);
            }

            return list;
        }

        public PicturePanel()
        {
            Padding = new Padding(0, 5, 0, 5);

            Panel p = new Panel()
            {
                Padding = new Padding(0,0,10,0),
                Height = CARD_ZIZE / 2,
                Dock = DockStyle.Bottom
            };

            p2 = new Panel()
            {
                Height = CARD_ZIZE,
                Dock = DockStyle.Top
            };

            PicturePanelButton addImageCard = new PicturePanelButton("Add Image");
            addImageCard.Image = Properties.Resources.AddButton;
            addImageCard.Dock = DockStyle.Left;

            addImageCard.onClick = () => 
            {
                ProcessClick(OpenImage);
            };

            p.Controls.Add(addImageCard);

            PicturePanelButton screenshotCard = new PicturePanelButton("Make Screenshot");
            screenshotCard.Image = Properties.Resources.screenShot;
            screenshotCard.Dock = DockStyle.Left;

            screenshotCard.onClick = () =>
            {
                ProcessClick(MakeScreen);
            };

            p.Controls.Add(screenshotCard);

            this.Controls.Add(p2);
            this.Controls.Add(p);
        }

        void ProcessClick(Action<PictureCard> act)
        {
            if (p2.Controls.Count < MAX_CARDS)
            {
                act.Invoke(null);
                return;
            }

            if (OnMaxPics != null)
                OnMaxPics.Invoke();
        } 

        private void MakeScreen(PictureCard c = null)
        {
            Image img = Terminal.MakeScreenshot( Form.ActiveForm.DisplayRectangle, Size.Empty);

            if (img == null) return;

            if (c != null)
            {
                c.BackgroundImage = img;
                return;
            }

            PictureCard pc = new PictureCard(img);

            pc.onClose = () =>
            {
                p2.Controls.Remove(pc);
                ReplaceOrder();
            };

            pc.onClick = () =>
            {
                MakeScreen(pc);
            };

            p2.Controls.Add(pc);
            ReplaceOrder();
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
                        c.BackgroundImage = img; 
                        return;
                    }

                    PictureCard pc = new PictureCard(img);

                    pc.onClose = () =>
                    {
                        p2.Controls.Remove(pc);
                        ReplaceOrder();
                    };

                    pc.onClick = () =>
                    {
                        OpenImage(pc);
                    };

                    p2.Controls.Add(pc);
                    ReplaceOrder();
                }
            }
        }

        static Image ResizeImage(Image image, int minSize)
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

        protected override void OnResize(EventArgs eventargs)
        {
            ReplaceOrder();

            base.OnResize(eventargs);
        }

        void ReplaceOrder()
        {
            int total = p2.Controls.Count;

            for (int i = total; i > 0; i--)
            {
                PictureBox p = p2.Controls[total - i] as PictureBox;
                if (p != null)
                {
                    p.Location = new Point((Width * (i) / (total + 1) - p.Width / 2), 0);
                }

            }
        }

        class PicturePanelButton : Label
        {
            ToolTip tip;
            public Action onClick;

            public PicturePanelButton(string onToolTip)
            {
                int size = CARD_ZIZE / 2;

                //Margin = new Padding(5,5,5,5);
  
                Width = size + 10;
                Height = size;
                Cursor = Cursors.Hand;

                FlatStyle = FlatStyle.Flat;
                this.BackgroundImageLayout = ImageLayout.Stretch;

                Image = Properties.Resources.AddButton;
                BackColor = Color.Transparent;

                if (!string.IsNullOrEmpty(onToolTip))
                {
                    tip = new ToolTip();
                    tip.ShowAlways = true;
                    tip.SetToolTip(this, onToolTip);
                }
            }

            protected override void OnClick(EventArgs e)
            {
                if (onClick != null)
                    onClick.Invoke();
            }
        }

        class PictureCard : PictureBox
        {
            ToolTip tip;
            Label cross;

            public Action onClose;
            public Action onClick;

            public PictureCard(Image img = null, string onToolTip = null)
            {
                Width = CARD_ZIZE;
                Height = CARD_ZIZE;
                Cursor = Cursors.Hand;

                this.SizeMode = PictureBoxSizeMode.Zoom;

                Image = Properties.Resources.button;
                BackgroundImage = ResizeImage(img, CARD_ZIZE); 
                BackgroundImageLayout = ImageLayout.Center;
                BackColor = Color.Transparent;


                cross = new Label()
                {
                    BackColor = Color.Transparent,
                    Size = new Size(20, 20),
                    Location = new Point(0, 0),
                    Anchor = AnchorStyles.Left | AnchorStyles.Top
                };

                cross.Click += (o, e) => {
                    if (onClose != null)
                        onClose.Invoke();
                };

                if (!string.IsNullOrEmpty(onToolTip))
                {
                    tip = new ToolTip();
                    tip.ShowAlways = true;
                    tip.SetToolTip(cross, onToolTip);
                }

                this.Controls.Add(cross);
            }

            protected override void OnClick(EventArgs e)
            {
                if (onClick != null)
                    onClick.Invoke();
            }
        }
    }
}
