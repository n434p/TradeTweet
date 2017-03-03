using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeTweet
{
    public partial class NoticeP2 : UserControl
    {
        Control ParentWindow;

        public NoticeP2(Control control)
        {
            InitializeComponent();

            ParentWindow = control;
            Visible = false;
        }

        internal async void ShowNotice(string message, int miliseconds = 2000, NoticeType type = NoticeType.Info, EventType evType = EventType.Empty, Action callback = null)
        {
            this.noticeText.Text = message;

            if (evType == EventType.Empty)
            switch (type)
            {
                case NoticeType.Info:
                    statusPic.Image = Properties.Resources.TradeTweet_09;
                    break;
                case NoticeType.Error:
                    statusPic.Image = Properties.Resources.TradeTweet_10;
                    break;
                case NoticeType.Success:
                    statusPic.Image = Properties.Resources.TradeTweet_11;
                    break;
            }

            switch (evType)
            {
                case EventType.OrderPlaced:
                    statusPic.Image = Properties.Resources.open_order_yellow;
                    break;
                case EventType.OrderCancelled:
                    statusPic.Image = Properties.Resources.close_order_yellow;
                    break;
                case EventType.PositionClosed:
                    statusPic.Image = Properties.Resources.close_pos_yellow;
                    break;
                case EventType.PositionOpened:
                    statusPic.Image = Properties.Resources.open_pos_yellow;
                    break;
            }

            await Task.Run(
                    async () =>
                    {
                        ParentWindow.Invoke((MethodInvoker)delegate
                        {
                            this.Visible = true;
                        });

                        await Task.Delay(miliseconds).ContinueWith((task) =>
                        {
                            if (callback != null)
                                callback.Invoke();
                        });

                        ParentWindow.Invoke((MethodInvoker)delegate
                        {
                            this.Visible = false;
                        });
                    });
        }
    }
}
