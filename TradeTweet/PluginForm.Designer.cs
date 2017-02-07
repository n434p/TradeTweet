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
            this.cheersBtn = new System.Windows.Forms.Button();
            this.ptmcLabel1 = new System.Windows.Forms.Label();
            this.messageTB = new System.Windows.Forms.TextBox();
            this.loginLabel = new System.Windows.Forms.LinkLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cheersBtn
            // 
            this.cheersBtn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cheersBtn.BackColor = System.Drawing.Color.Orange;
            this.cheersBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cheersBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cheersBtn.Location = new System.Drawing.Point(5, 554);
            this.cheersBtn.Name = "cheersBtn";
            this.cheersBtn.Size = new System.Drawing.Size(1394, 35);
            this.cheersBtn.TabIndex = 0;
            this.cheersBtn.Text = "Tweet it!";
            this.cheersBtn.UseVisualStyleBackColor = false;
            this.cheersBtn.Click += new System.EventHandler(this.cheersBtn_Click);
            // 
            // ptmcLabel1
            // 
            this.ptmcLabel1.AutoSize = true;
            this.ptmcLabel1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.ptmcLabel1.Location = new System.Drawing.Point(2, 44);
            this.ptmcLabel1.Name = "ptmcLabel1";
            this.ptmcLabel1.Size = new System.Drawing.Size(53, 13);
            this.ptmcLabel1.TabIndex = 1;
            this.ptmcLabel1.Text = "Message:";
            this.ptmcLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // messageTB
            // 
            this.messageTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.messageTB.Location = new System.Drawing.Point(5, 60);
            this.messageTB.MaxLength = 140;
            this.messageTB.Multiline = true;
            this.messageTB.Name = "messageTB";
            this.messageTB.Size = new System.Drawing.Size(1395, 488);
            this.messageTB.TabIndex = 2;
            // 
            // loginLabel
            // 
            this.loginLabel.ActiveLinkColor = System.Drawing.Color.DarkOrange;
            this.loginLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loginLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.loginLabel.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.loginLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.loginLabel.LinkColor = System.Drawing.Color.DarkOrange;
            this.loginLabel.Location = new System.Drawing.Point(1128, 9);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.loginLabel.Size = new System.Drawing.Size(269, 20);
            this.loginLabel.TabIndex = 3;
            this.loginLabel.TabStop = true;
            this.loginLabel.Text = "Login";
            this.loginLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.loginLabel.UseMnemonic = false;
            this.loginLabel.VisitedLinkColor = System.Drawing.Color.DarkOrange;
            this.loginLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.loginLabel_LinkClicked);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(192, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TradeTweet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1409, 620);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.loginLabel);
            this.Controls.Add(this.messageTB);
            this.Controls.Add(this.ptmcLabel1);
            this.Controls.Add(this.cheersBtn);
            this.Name = "TradeTweet";
            this.Text = "PluginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cheersBtn;
        private System.Windows.Forms.Label ptmcLabel1;
        private System.Windows.Forms.TextBox messageTB;
        private System.Windows.Forms.LinkLabel loginLabel;
        private System.Windows.Forms.Button button1;
    }
}