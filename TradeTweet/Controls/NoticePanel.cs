using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeTweet
{
    public partial class NoticePanel : UserControl
    {
        Control ParentWindow;
        CancellationToken ct;

        public NoticePanel(Control control, CancellationToken token)
        {
            InitializeComponent();
            ct = token;
            ParentWindow = control;
            ParentWindow.Controls.Add(this);
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

            if (ParentWindow.IsHandleCreated)
            {
                ParentWindow.Invoke((MethodInvoker)delegate
                {
                    this.Visible = true;
                });

                await Task.Delay(miliseconds,ct).ContinueWith((task) =>
                {
                    if (ct.IsCancellationRequested)
                    {
                        return;
                    }

                    ParentWindow.Invoke((MethodInvoker)delegate
                    {
                        this.Visible = false;
                    });
                
                    if (callback != null)
                        callback.Invoke();
                }, ct);
            }   
        }
    }
}
