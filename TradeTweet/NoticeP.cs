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
        public Action mouseMoved;

        public bool Removeable { get { return removeable; } }
        bool removeable = false;

        public NoticeP(Control attachTo)
        {
            InitializeComponent();
            DoubleBuffered = true;
            ParentWindow = attachTo;
            crossLabel.Visible = false;
        }

        internal void ShowNotice(TwitMessage msg, Action callback = null)
        {
            NoticeP notice = new NoticeP(ParentWindow);
            notice.noticeText.Text = msg.Message.Replace("#PTMC_platform\n","").TrimStart(' ');
            notice.removeable = msg.NoticeType == NoticeType.Error;
            notice.label1.Text = msg.FormattedTime;

            notice.Dock = DockStyle.Top;

            if (notice.removeable)
            {
                notice.mouseMoved = mouseMoved;
            }

            if (msg.EventType == EventType.Empty)
                switch (msg.NoticeType)
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

            switch (msg.EventType)
            {
                case EventType.OrderPlaced:
                    notice.statusPic.Image = (msg.NoticeType == NoticeType.Error)? Resources.open_order_red: Resources.open_order_green;
                    break;
                case EventType.OrderCancelled:
                    notice.statusPic.Image = (msg.NoticeType == NoticeType.Error) ? Resources.close_order_red : Resources.close_order_green;
                    break;
                case EventType.PositionClosed:
                    notice.statusPic.Image = (msg.NoticeType == NoticeType.Error) ? Resources.close_pos_red : Resources.close_pos_green;
                    break;
                case EventType.PositionOpened:
                    notice.statusPic.Image = (msg.NoticeType == NoticeType.Error) ? Resources.open_pos_red : Resources.open_pos_green;
                    break;
            }

            if (ParentWindow.IsHandleCreated)
            {
                ParentWindow.Invoke((MethodInvoker)delegate
                {
                    //    int i = 1;
                    //    foreach (Control item in ParentWindow.Controls)
                    //    {
                    //        ParentWindow.Controls.SetChildIndex(item, i++);
                    //    }

                    ParentWindow.Controls.Add(notice);
                    //    ParentWindow.Controls.SetChildIndex(notice, 0);
                });
            }  
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (crossLabel.Image != null)
            {
                ParentWindow.Controls.Remove(this);
                this.Dispose();
            }
        }

        void RefreshState()
        {
            if (mouseMoved != null)
                mouseMoved.Invoke();
        }

        internal void RefreshRemoveabled()
        {
            if (!removeable) return;

            if (tableLayoutPanel1.ClientRectangle.Contains(this.PointToClient(Cursor.Position)))
            {
                crossLabel.Visible = true;
                this.BackColor = Color.Black;
            }
            else
            {
                crossLabel.Visible = false;
                this.BackColor = Color.Transparent;
            }
        }

        internal class DoubleBufferedTableLP : TableLayoutPanel
        {
            public DoubleBufferedTableLP():base()
            {
                DoubleBuffered = true;
            }
        }

        private void tableLayoutPanel1_MouseEnter(object sender, EventArgs e)
        {
            RefreshState();
        }

        private void tableLayoutPanel1_MouseLeave(object sender, EventArgs e)
        {
            RefreshState();
        }
    }
}
