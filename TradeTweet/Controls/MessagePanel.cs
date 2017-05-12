using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeTweet.Properties;

namespace TradeTweet
{
    enum EventStatus { Info, Error, Success}

    public partial class MessagePanel : UserControl
    {
        Control ParentWindow;
        public Action mouseMoved;

        public bool Removeable { get { return removeable; } }
        bool removeable = false;

        public MessagePanel(Control attachTo)
        {
            InitializeComponent();
            DoubleBuffered = true;
            ParentWindow = attachTo;
            crossLabel.Visible = false;
        }

        internal void ShowNotice(TwitMessage msg, Action callback = null)
        {
            MessagePanel notice = new MessagePanel(ParentWindow);
            notice.noticeText.Text = msg.Message.Replace(EventBuilder.PTMC_CAPTION,"").TrimStart(' ');
            notice.removeable = msg.status == EventStatus.Error;
            notice.label1.Text = msg.FormattedTime;

            notice.Dock = DockStyle.Top;

            if (notice.removeable)
            {
                notice.mouseMoved = mouseMoved;
            }

            notice.statusPic.Image = msg.Image;

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

        public class DoubleBufferedTableLP : TableLayoutPanel
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
