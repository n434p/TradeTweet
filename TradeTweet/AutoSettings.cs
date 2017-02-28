using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TradeTweet
{
    public partial class AutoSettings : UserControl
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );


        public AutoSettings()
        {
            InitializeComponent();

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 1, 5));

            TreeNode orderPlaced_side = new TreeNode("Side");
            TreeNode orderPlaced_qty = new TreeNode("Quantity");
            TreeNode orderPlaced_symbol = new TreeNode("Symbol");
            TreeNode orderPlaced_type = new TreeNode("Order type");
            TreeNode orderPlaced_price = new TreeNode("Open price");
            TreeNode orderPlaced_sl = new TreeNode("Stop loss");
            TreeNode orderPlaced_tp = new TreeNode("Take profit");
            TreeNode orderPlaced_id = new TreeNode("Order ID");

            TreeNode orderPlaced = new TreeNode("Order placed", new TreeNode[] 
            {
                orderPlaced_side,
                orderPlaced_qty,
                orderPlaced_symbol,
                orderPlaced_type,
                orderPlaced_price,
                orderPlaced_sl,
                orderPlaced_tp,
                orderPlaced_id
            });

            settingsTree.Nodes.Add(orderPlaced);

            TreeNode positionOpenned_side = new TreeNode("Side");
            TreeNode positionOpenned_qty = new TreeNode("Quantity");
            TreeNode positionOpenned_symbol = new TreeNode("Symbol");
            TreeNode positionOpenned_price = new TreeNode("Open price");
            TreeNode positionOpenned_sl = new TreeNode("SL(Forex)");
            TreeNode positionOpenned_tp = new TreeNode("TP(Forex)");
            TreeNode positionOpenned_id = new TreeNode("Order ID");

            TreeNode positionOpenned = new TreeNode("Position openned", new TreeNode[]
            {
                positionOpenned_side,
                positionOpenned_qty,
                positionOpenned_symbol,
                positionOpenned_price,
                positionOpenned_sl,
                positionOpenned_tp,
                positionOpenned_id
            });

            settingsTree.Nodes.Add(positionOpenned);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }

    
}
