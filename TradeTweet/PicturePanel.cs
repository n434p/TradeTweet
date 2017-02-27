using PTLRuntime.NETScript.Application;
using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using TradeTweet.Properties;

namespace TradeTweet
{
    enum picMode { Image, ScreenShot}

    class PicturePanel : Panel
    {
        const int MAX_CARDS = 4;
        const int CARD_ZIZE = 64;
        const int MARGIN = 7;

        public Action OnMaxPics = null;

        public void Clear()
        {
            this.Controls.Clear();
        }

        public List<Image> GetImages()
        {
            List<Image> list = new List<Image>();

            foreach (Control item in this.Controls)
            {
                if (item.BackgroundImage != null)
                    list.Add(item.BackgroundImage);
            }

            return list;
        }

        public PicturePanel()
        {
            Padding = new Padding(0, 5, 0, 5);
            Height = CARD_ZIZE + MARGIN;
            Dock = DockStyle.Top;
        }

        protected override void OnResize(EventArgs eventargs)
        {
            ReplaceOrder();
            base.OnResize(eventargs);
        }

        public void ReplaceOrder()
        {
            int total = this.Controls.Count;

            for (int i = total; i > 0; i--)
            {
                PictureBox p = this.Controls[total - i] as PictureBox;
                if (p != null)
                {
                    p.Location = new Point((Width * (i) / (total + 1) - p.Width / 2), MARGIN);
                }

            }
        }
        
    }
}
