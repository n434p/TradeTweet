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
    enum NoticeType { Info, Error, Success}

    public partial class NoticeP : UserControl
    {
        Control ParentWindow;

        public NoticeP(Control attachTo)
        {
            InitializeComponent();

            Visible = false;
            ParentWindow = attachTo;
        }

        internal async void ShowNotice(string message, int miliseconds = 2000, NoticeType type = NoticeType.Info, EventType evType = EventType.Empty, Action callback = null)
        {
            ParentWindow.Invoke((MethodInvoker)delegate
            {
                this.noticeText.Text = message;

                this.Visible = true;

                switch (type)
                {
                    case NoticeType.Info:
                        this.statusPic.Image = Properties.Resources.TradeTweet_09;
                        break;
                    case NoticeType.Error:
                        this.statusPic.Image = Properties.Resources.TradeTweet_10;
                        break;
                    case NoticeType.Success:
                        this.statusPic.Image = Properties.Resources.TradeTweet_11;
                        break;
                }

                switch (evType)
                {
                    case EventType.Empty:
                        this.sidePic.Image = null;
                        break;
                    case EventType.OrderOpen:
                        this.sidePic.Image = Properties.Resources.TradeTweet_16;
                        break;
                    case EventType.OrderClose:
                        this.sidePic.Image = Properties.Resources.TradeTweet_17;
                        break;
                    case EventType.PositionClose:
                        this.sidePic.Image = Properties.Resources.TradeTweet_18;
                        break;
                    case EventType.PositionOpen:
                        this.sidePic.Image = Properties.Resources.TradeTweet_19;
                        break;
                }

            });

            await Task.Delay(miliseconds).ContinueWith((t) =>
            {
                if (callback != null)
                    callback.Invoke();
            });

            ParentWindow.Invoke((MethodInvoker)delegate
            {
                this.Visible = false;
            });
        }
    }
}
