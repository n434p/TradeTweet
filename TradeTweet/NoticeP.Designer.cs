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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoticeP));
            this.noticeText = new System.Windows.Forms.Label();
            this.crossLabel = new System.Windows.Forms.Label();
            this.statusPic = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new DoubleBufferedTableLP();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // noticeText
            // 
            this.noticeText.AutoEllipsis = true;
            this.noticeText.AutoSize = true;
            this.noticeText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.noticeText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.noticeText.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.noticeText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(162)))), ((int)(((byte)(162)))), ((int)(((byte)(162)))));
            this.noticeText.Location = new System.Drawing.Point(26, 5);
            this.noticeText.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.noticeText.Name = "noticeText";
            this.noticeText.Size = new System.Drawing.Size(348, 45);
            this.noticeText.TabIndex = 0;
            this.noticeText.Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Pellentesque interdum ru" +
    "trum sodales. Nullam mattis fermentum libero, non volutpat.";
            // 
            // crossLabel
            // 
            this.crossLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.crossLabel.Image = ((System.Drawing.Image)(resources.GetObject("crossLabel.Image")));
            this.crossLabel.Location = new System.Drawing.Point(376, 5);
            this.crossLabel.Margin = new System.Windows.Forms.Padding(0, 5, 8, 0);
            this.crossLabel.Name = "crossLabel";
            this.crossLabel.Size = new System.Drawing.Size(16, 16);
            this.crossLabel.TabIndex = 1;
            this.crossLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // statusPic
            // 
            this.statusPic.Image = ((System.Drawing.Image)(resources.GetObject("statusPic.Image")));
            this.statusPic.Location = new System.Drawing.Point(5, 5);
            this.statusPic.Margin = new System.Windows.Forms.Padding(5);
            this.statusPic.Name = "statusPic";
            this.statusPic.Size = new System.Drawing.Size(16, 16);
            this.statusPic.TabIndex = 2;
            this.statusPic.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 348F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this.tableLayoutPanel1.Controls.Add(this.statusPic, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.noticeText, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.crossLabel, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 1);
            this.tableLayoutPanel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(400, 64);
            this.tableLayoutPanel1.TabIndex = 4;
            this.tableLayoutPanel1.MouseEnter += new System.EventHandler(this.tableLayoutPanel1_MouseEnter);
            this.tableLayoutPanel1.MouseLeave += new System.EventHandler(this.tableLayoutPanel1_MouseLeave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DimGray;
            this.label1.Location = new System.Drawing.Point(26, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "2222222222222";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NoticeP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "NoticeP";
            this.Size = new System.Drawing.Size(400, 64);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label noticeText;
        private System.Windows.Forms.Label statusPic;
        private System.Windows.Forms.Label crossLabel;
        private DoubleBufferedTableLP tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
    }
}
