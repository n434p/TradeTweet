﻿namespace TradeTweet
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
            this.label1 = new System.Windows.Forms.Label();
            this.statusPic = new System.Windows.Forms.Label();
            this.sidePic = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // noticeText
            // 
            this.noticeText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.noticeText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noticeText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.noticeText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(148)))), ((int)(((byte)(148)))));
            this.noticeText.Location = new System.Drawing.Point(53, 4);
            this.noticeText.Name = "noticeText";
            this.noticeText.Size = new System.Drawing.Size(295, 38);
            this.noticeText.TabIndex = 0;
            this.noticeText.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum ru" +
    "trum sodales. Nullam mattis fermentum libero, non volutpat.\r\n";
            this.noticeText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 1);
            this.label1.TabIndex = 3;
            // 
            // statusPic
            // 
            this.statusPic.Image = global::TradeTweet.Properties.Resources.TradeTweet_11;
            this.statusPic.Location = new System.Drawing.Point(16, 15);
            this.statusPic.Margin = new System.Windows.Forms.Padding(0);
            this.statusPic.Name = "statusPic";
            this.statusPic.Size = new System.Drawing.Size(16, 16);
            this.statusPic.TabIndex = 2;
            // 
            // sidePic
            // 
            this.sidePic.Image = global::TradeTweet.Properties.Resources.TradeTweet_16;
            this.sidePic.Location = new System.Drawing.Point(351, 0);
            this.sidePic.Margin = new System.Windows.Forms.Padding(0);
            this.sidePic.Name = "sidePic";
            this.sidePic.Size = new System.Drawing.Size(47, 47);
            this.sidePic.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.label2.Location = new System.Drawing.Point(0, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(400, 1);
            this.label2.TabIndex = 4;
            // 
            // NoticeP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusPic);
            this.Controls.Add(this.sidePic);
            this.Controls.Add(this.noticeText);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "NoticeP";
            this.Size = new System.Drawing.Size(400, 47);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label noticeText;
        private System.Windows.Forms.Label sidePic;
        private System.Windows.Forms.Label statusPic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}