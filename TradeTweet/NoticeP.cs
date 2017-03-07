using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeTweet.Properties;

namespace TradeTweet
{
    enum NoticeType { Info, Error, Success}

    public partial class NoticeP : UserControl
    {
        Control ParentWindow;
        bool removeable = false;

        public NoticeP(Control attachTo)
        {
            InitializeComponent();
            DoubleBuffered = true;
            ParentWindow = attachTo;
            crossLabel.Visible = false;
        }

        internal virtual void ShowNotice(string message, NoticeType type = NoticeType.Info, EventType evType = EventType.Empty, Action callback = null)
        {
            NoticeP notice = new NoticeP(ParentWindow);

            notice.noticeText.Text = message.Replace("#PTMC_platform","").Replace('\n',' ');

            notice.removeable = type == NoticeType.Error;

            if (evType == EventType.Empty)
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
                case EventType.OrderPlaced:
                    notice.statusPic.Image = (type == NoticeType.Error)? Resources.open_order_red: Resources.open_order_green;
                    break;
                case EventType.OrderCancelled:
                    notice.statusPic.Image = (type == NoticeType.Error) ? Resources.close_order_red : Resources.close_order_green;
                    break;
                case EventType.PositionClosed:
                    notice.statusPic.Image = (type == NoticeType.Error) ? Resources.close_pos_red : Resources.close_pos_green;
                    break;
                case EventType.PositionOpened:
                    notice.statusPic.Image = (type == NoticeType.Error) ? Resources.open_pos_red : Resources.open_pos_green;
                    break;
            }
            
            ParentWindow.Invoke((MethodInvoker)delegate
            {
                ParentWindow.Controls.Add(notice);
            });       
        }


        private void label1_Click(object sender, EventArgs e)
        {
            if (crossLabel.Image != null)
            {
                ParentWindow.Controls.Remove(this);
                this.Dispose();
            }
        }


        //protected override void OnMouseLeave(EventArgs e)
        //{
        //    if (removeable && this.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
        //    {
        //        crossLabel.Visible = true;
        //        this.BackColor = Color.Black;
        //    }
        //    else
        //    {
        //        crossLabel.Visible = false;
        //        this.BackColor = Color.Transparent;
        //    }
        //}

        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    if (!removeable) return;

        //     crossLabel.Image = Properties.Resources.TradeTweet_10;
        //     this.BackColor = Color.Black;
        //}
    }
}
