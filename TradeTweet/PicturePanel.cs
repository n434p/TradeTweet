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

        public PicturePanel()
        {
            Height = CARD_ZIZE;
            Dock = DockStyle.Bottom;

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
                        c.Image = img;
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
                    p.Location = new Point((Width * (i) / (total + 1) - p.Width / 2), (Height - p.Height)/2 );
            }
        }

        class PictureCard : PictureBox
        {
            Label cross;

            const string Add = "+";
            const string Remove = "\u2716";

            public Action callback;
            public Action onClick;

            public PictureCard(Image img = null)
            {
                bool imageExists = img != null;

                Margin = new Padding(5);
                Padding = new Padding(5);

                int size = (imageExists) ? CARD_ZIZE : CARD_ZIZE / 2;

                Width = size;
                Height = size;

                this.SizeMode = PictureBoxSizeMode.Zoom;

                if (imageExists)
                {
                    Image = img;
                    BackColor = Color.Transparent;
                }
                else
                {
                    BackColor = Color.Orange;
                }

                cross = new Label()
                {
                    Width = size,
                    Height = size,
                    Font = new Font("Arial", 14),
                    TextAlign = ContentAlignment.TopRight,
                    Anchor = AnchorStyles.Left | AnchorStyles.Top
                };

                if (imageExists)
                {
                    cross.Size = new Size(20, 20);
                    cross.TextAlign = ContentAlignment.MiddleCenter;
                    cross.Location = new Point(0, 0);
                }

                cross.TextAlign = (imageExists) ? ContentAlignment.TopRight : ContentAlignment.MiddleCenter;
                cross.Margin = (imageExists) ? new Padding(5) : new Padding(0);
                cross.Text = (imageExists) ? Remove : Add;

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
