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

            notice = new Label()
            {
                Dock = DockStyle.Fill,
                ForeColor = Color.DimGray,
                Font = Settings.mainFont,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Visible = false;

            this.Controls.Add(notice);
        }


        public async void ShowNotice(string message, int miliseconds = 2000, Action callback = null)
        {
            notice.Text = message;

            this.Invoke((MethodInvoker)delegate
            {
                this.Visible = true;
            });

            await Task.Delay(miliseconds).ContinueWith( (t) => 
            {
                if (callback != null)
                    callback.Invoke(); 
            });

            this.Invoke((MethodInvoker)delegate 
            {
                this.Visible = false;
            });
        }
    }
}
