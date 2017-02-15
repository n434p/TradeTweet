using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TradeTweet
{
    class PicturePanel : Panel
    {
        const int MAX_CARDS = 4;
        const int CARD_ZIZE = 80;
        const int MARGIN = 5;

        public PicturePanel()
        {
            PictureCard card = new PictureCard();

            card.callback = () => 
            {
                if (Controls.Count <= MAX_CARDS)
                {
                    OpenImage();
                    return;
                }


                MessageBox.Show("Only 4 pics are allowed for one tweet!");
            };

            this.Controls.Add(card);
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

                    pc.callback = () =>
                    {
                        this.Controls.Remove(pc);
                        ReplaceOrder();
                    };

                    pc.onClick = () =>
                    {
                        OpenImage(pc);
                    };

                    this.Controls.Add(pc);
                    ReplaceOrder();
                }
            }
        }

        static Image ResizeImage(Image image, int maxSize)
        {
            Image resizedImg = null;

            var width = image.Width;
            var height = image.Height;

            var smallerSize = (int) (Math.Min(width, height) * maxSize / Math.Max(width, height));

            if (width >= height)
            {
                resizedImg = (Image)(new Bitmap(image, maxSize, smallerSize));
            }
            else
            {
                resizedImg = (Image)(new Bitmap(image, smallerSize, maxSize));
            }

            return resizedImg;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            ReplaceOrder();
        }

        void ReplaceOrder()
        {
            int total = Controls.Count;

            for (int i = total; i > 0; i--)
            {
                PictureBox p = Controls[total - i] as PictureBox;
                if (p != null)
                {
                    p.Location = new Point((Width * (i) / (total + 1) - p.Width / 2), (Height - p.Height)/2 );
                }
                    
            }
        }

        class PictureCard : PictureBox
        {
            Label cross;

            public Action callback;
            public Action onClick;

            public PictureCard(Image img = null)
            {
                bool imageExists = img != null;

                int size = (imageExists) ? CARD_ZIZE : CARD_ZIZE / 2;

                Width = size;
                Height = size;

                this.SizeMode = PictureBoxSizeMode.Zoom;

                if (imageExists)
                {
                    Image = Properties.Resources.button;
                    BackgroundImage = img; 
                    BackgroundImageLayout = ImageLayout.Zoom;
                    BackColor = Color.Transparent;
                }
                else
                {
                    Image = Properties.Resources.AddButton;
                    BackColor = Color.Transparent;
                }

                cross = new Label()
                {
                    BackColor = Color.Transparent,
                    Width = size,
                    Height = size,
                    Anchor = AnchorStyles.Left | AnchorStyles.Top
                };

                if (imageExists)
                {
                    cross.Size = new Size(20, 20);
                    cross.Location = new Point(0, 0);
                }

                cross.Click += (o, e) => {
                    if (callback != null)
                        callback.Invoke();
                };

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
