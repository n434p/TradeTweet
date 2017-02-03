using System;
using System.Collections.Generic;
using PTLRuntime.NETScript;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PTLRuntime.NETScript.Controls;
using com.pfsoft.proftrading.commons.external;
using System.Drawing;

namespace TradeTweet
{
    [Exportable]
    public partial class TradeTweet : ComponentForm, IExternalComponent
    {
        public TradeTweet()
        {
            InitializeComponent();
        }

        private void cheersBtn_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Hello, world");
        }

        #region IExternalComponent
        public Icon IconImage
        {
            get { return this.Icon; }
        }

        public string ComponentName
        {
            get { return "TradeTweet"; }
        }

        public string PanelHeader
        {
            get { return "TradeTweet"; }
        }

        public Control Content
        {
            get { return this; }
        }

        public void Populate()
        {
        }

        public void Dispose()
        {
        }
        #endregion
    }
}
