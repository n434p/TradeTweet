using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TradeTweet.Properties;

namespace TradeTweet
{
    public partial class ConnectionPanel : UserControl
    {
        Button connectBtn;
        Label connectMessage;
        public Action OnConnect = null;

        public ConnectionPanel()
        {
            InitializeComponent();

            this.Dock = DockStyle.Fill;

            connectBtn.Click += (o, e) =>
            {
                if (OnConnect != null)
                    OnConnect.Invoke();
            };
        }
    }
}
