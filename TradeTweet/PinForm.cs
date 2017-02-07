using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeTweet
{
    public partial class PinForm : Form
    {
        public PinForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            inputTB.RejectInputOnFirstFailure = true;
        }

        public static string ShowForm()
        {
            PinForm form = new PinForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if(form.inputTB.MaskCompleted)
                return form.inputTB.Text;
            }
            return null;
        }
    }
}
