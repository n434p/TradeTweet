namespace TradeTweet
{
    partial class NoticeP
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.noticeText = new System.Windows.Forms.Label();
            this.crossLabel = new System.Windows.Forms.Label();
            this.statusPic = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // noticeText
            // 
            this.noticeText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.noticeText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noticeText.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.noticeText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(148)))), ((int)(((byte)(148)))));
            this.noticeText.Location = new System.Drawing.Point(45, 0);
            this.noticeText.Margin = new System.Windows.Forms.Padding(0);
            this.noticeText.Name = "noticeText";
            this.noticeText.Size = new System.Drawing.Size(320, 45);
            this.noticeText.TabIndex = 0;
            this.noticeText.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum ru" +
    "trum sodales. Nullam mattis fermentum libero, non volutpat.\r\n";
            this.noticeText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.noticeText.MouseEnter += new System.EventHandler(this.noticeText_MouseEnter);
            this.noticeText.MouseLeave += new System.EventHandler(this.noticeText_MouseLeave);
            // 
            // crossLabel
            // 
            this.crossLabel.Image = global::TradeTweet.Properties.Resources.TradeTweet_10;
            this.crossLabel.Location = new System.Drawing.Point(366, 15);
            this.crossLabel.Margin = new System.Windows.Forms.Padding(0);
            this.crossLabel.Name = "crossLabel";
            this.crossLabel.Size = new System.Drawing.Size(16, 16);
            this.crossLabel.TabIndex = 1;
            this.crossLabel.Click += new System.EventHandler(this.label1_Click);
            this.crossLabel.MouseEnter += new System.EventHandler(this.crossLabel_MouseEnter);
            this.crossLabel.MouseLeave += new System.EventHandler(this.crossLabel_MouseLeave);
            // 
            // statusPic
            // 
            this.statusPic.Image = global::TradeTweet.Properties.Resources.TradeTweet_11;
            this.statusPic.Location = new System.Drawing.Point(14, 14);
            this.statusPic.Margin = new System.Windows.Forms.Padding(0);
            this.statusPic.Name = "statusPic";
            this.statusPic.Size = new System.Drawing.Size(16, 16);
            this.statusPic.TabIndex = 2;
            // 
            // NoticeP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.crossLabel);
            this.Controls.Add(this.statusPic);
            this.Controls.Add(this.noticeText);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "NoticeP";
            this.Size = new System.Drawing.Size(400, 47);
            this.MouseEnter += new System.EventHandler(this.NoticeP_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.NoticeP_MouseLeave);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label noticeText;
        private System.Windows.Forms.Label statusPic;
        private System.Windows.Forms.Label crossLabel;
    }
}
