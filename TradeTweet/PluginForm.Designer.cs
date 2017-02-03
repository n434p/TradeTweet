namespace TradeTweet
{
    partial class TradeTweet
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cheersBtn = new PTLRuntime.NETScript.Controls.PTMCButton();
            this.ptmcLabel1 = new PTLRuntime.NETScript.Controls.PTMCLabel();
            ((System.ComponentModel.ISupportInitialize)(this.cheersBtn)).BeginInit();
            this.SuspendLayout();
            // 
            // cheersBtn
            // 
            this.cheersBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cheersBtn.Checked = false;
            this.cheersBtn.CustomForeColor = System.Drawing.Color.Empty;
            this.cheersBtn.CustomForeShadowColor = System.Drawing.Color.Empty;
            this.cheersBtn.DialogResult = System.Windows.Forms.DialogResult.None;
            this.cheersBtn.Group = null;
            this.cheersBtn.Image = null;
            this.cheersBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cheersBtn.ImageCheked = null;
            this.cheersBtn.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
            this.cheersBtn.Location = new System.Drawing.Point(11, 51);
            this.cheersBtn.MinimumDropDownSize = new System.Drawing.Size(0, 0);
            this.cheersBtn.Name = "cheersBtn";
            this.cheersBtn.Size = new System.Drawing.Size(261, 37);
            this.cheersBtn.TabIndex = 0;
            this.cheersBtn.TerceraButtonStyle = com.pfsoft.proftrading.controls.terceraStyle.TerceraButtonStyle.Standard;
            this.cheersBtn.Text = "Cheers";
            this.cheersBtn.Tooltip = null;
            this.cheersBtn.TooltipSubButton = null;
            this.cheersBtn.Click += new System.EventHandler(this.cheersBtn_Click);
            // 
            // ptmcLabel1
            // 
            this.ptmcLabel1.AutoSize = true;
            this.ptmcLabel1.CustomBackColor = System.Drawing.Color.Empty;
            this.ptmcLabel1.CustomForeColor = System.Drawing.Color.Empty;
            this.ptmcLabel1.CustomShadowColor = System.Drawing.Color.Empty;
            this.ptmcLabel1.Location = new System.Drawing.Point(95, 9);
            this.ptmcLabel1.Name = "ptmcLabel1";
            this.ptmcLabel1.ShowToolTip = false;
            this.ptmcLabel1.Size = new System.Drawing.Size(63, 13);
            this.ptmcLabel1.TabIndex = 1;
            this.ptmcLabel1.Text = "Hello, world";
            this.ptmcLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ptmcLabel1.TextAlignment = System.Drawing.StringAlignment.Near;
            this.ptmcLabel1.TextLineAlignment = System.Drawing.StringAlignment.Center;
            // 
            // PluginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ptmcLabel1);
            this.Controls.Add(this.cheersBtn);
            this.Name = "PluginForm";
            this.Text = "PluginForm";
            ((System.ComponentModel.ISupportInitialize)(this.cheersBtn)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PTLRuntime.NETScript.Controls.PTMCButton cheersBtn;
        private PTLRuntime.NETScript.Controls.PTMCLabel ptmcLabel1;
    }
}