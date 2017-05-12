using System;
using System.Drawing;
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

        internal void ShowNotice(string message, Image image, int miliseconds = 2000, EventStatus status = EventStatus.Info, Action callback = null)
        {
            ShowNotice(new TwitMessage() { Message = message, Image = image, status = status, Time = DateTime.UtcNow }, miliseconds, callback);
        }

        internal async void ShowNotice(TwitMessage msg, int miliseconds = 2000, Action callback = null)
        {
            this.noticeText.Text = msg.Message;
            statusPic.Image = msg.Image;

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
