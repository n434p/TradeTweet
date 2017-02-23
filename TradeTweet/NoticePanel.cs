using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeTweet
{
    class NoticePanel: Panel
    {
        Label notice;
        const int panelHeight = 40;

        public NoticePanel()
        {
            Dock = DockStyle.Top;
            Height = panelHeight;
            BackColor = Settings.mainFontColor;
            Visible = false;

            notice = new Label()
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.DimGray,
                Font = Settings.mainFont,
                TextAlign = ContentAlignment.MiddleCenter
            };

            this.Controls.Add(notice);
        }

        public async void ShowNotice(Control control, string message, int miliseconds = 2000, Action callback = null)
        {
            control.Invoke((MethodInvoker)delegate
            {
                this.notice.Text = message;
                this.Visible = true;
            });

            await Task.Delay(miliseconds).ContinueWith( (t) => 
            {
                if (callback != null)
                    callback.Invoke(); 
            });

            control.Invoke((MethodInvoker)delegate 
            {
                this.Visible = false;
            });
        }
    }
}
