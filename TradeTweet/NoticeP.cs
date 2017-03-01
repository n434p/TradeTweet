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
            DoubleBuffered = true;
            ParentWindow = attachTo;
        }

        internal async void ShowNotice(string message, int miliseconds = 2000, NoticeType type = NoticeType.Info, EventType evType = EventType.Empty, Action callback = null)
        {
            NoticeP notice = new NoticeP(ParentWindow);
            notice.noticeText.Text = message;

            switch (type)
            {
                case NoticeType.Info:
                    notice.statusPic.Image = Properties.Resources.TradeTweet_09;
                    break;
                case NoticeType.Error:
                    notice.statusPic.Image = Properties.Resources.TradeTweet_10;
                    break;
                case NoticeType.Success:
                    notice.statusPic.Image = Properties.Resources.TradeTweet_11;
                    break;
            }

            switch (evType)
            {
                case EventType.Empty:
                    notice.sidePic.Image = null;
                    break;
                case EventType.OrderPlaced:
                    notice.sidePic.Image = Properties.Resources.TradeTweet_16;
                    break;
                case EventType.OrderCancelled:
                    notice.sidePic.Image = Properties.Resources.TradeTweet_17;
                    break;
                case EventType.PositionClosed:
                    notice.sidePic.Image = Properties.Resources.TradeTweet_18;
                    break;
                case EventType.PositionOpened:
                    notice.sidePic.Image = Properties.Resources.TradeTweet_19;
                    break;
            }


            ParentWindow.Invoke((MethodInvoker)delegate
            {
                ParentWindow.Controls.Add(notice);
                ParentWindow.Controls.SetChildIndex(notice, 0);
            });

            await Task.Delay(miliseconds).ContinueWith((t) =>
            {
                if (callback != null)
                    callback.Invoke();

                ParentWindow.Invoke((MethodInvoker)delegate
                {
                    if (miliseconds != 0)
                        ParentWindow.Controls.Remove(notice);
                });
            });
        }
    }
}
