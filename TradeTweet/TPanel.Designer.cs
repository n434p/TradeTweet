using System.Windows.Forms;

namespace TradeTweet
{
    partial class TPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TPanel));
            this.gridPanel = new System.Windows.Forms.TableLayoutPanel();
            this.headerPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.settingsBtn = new System.Windows.Forms.Label();
            this.autoTweetBtn = new System.Windows.Forms.Label();
            this.logoutLink = new System.Windows.Forms.LinkLabel();
            this.avatar = new System.Windows.Forms.PictureBox();
            this.accountName = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.tweetText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tweetBtn = new System.Windows.Forms.Label();
            this.makeScreenshotBtn = new System.Windows.Forms.Label();
            this.addImageBtn = new System.Windows.Forms.Label();
            this.tweetLengthLabel = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.separatorLine = new System.Windows.Forms.Panel();
            this.leftCorner = new System.Windows.Forms.Label();
            this.rightCorner = new System.Windows.Forms.Label();
            this.picPanelBase = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.picPanelContainer = new System.Windows.Forms.Label();
            this.historyPanel = new TradeTweet.CustomPanel();
            this.gridPanel.SuspendLayout();
            this.headerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.avatar)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.separatorLine.SuspendLayout();
            this.picPanelBase.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridPanel
            // 
            this.gridPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPanel.BackColor = System.Drawing.Color.Transparent;
            this.gridPanel.ColumnCount = 3;
            this.gridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.gridPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.gridPanel.Controls.Add(this.headerPanel, 1, 0);
            this.gridPanel.Controls.Add(this.panel3, 1, 2);
            this.gridPanel.Controls.Add(this.panel1, 1, 5);
            this.gridPanel.Controls.Add(this.panel5, 1, 4);
            this.gridPanel.Controls.Add(this.picPanelBase, 1, 3);
            this.gridPanel.Controls.Add(this.historyPanel, 1, 1);
            this.gridPanel.Location = new System.Drawing.Point(3, 4);
            this.gridPanel.Margin = new System.Windows.Forms.Padding(0);
            this.gridPanel.Name = "gridPanel";
            this.gridPanel.RowCount = 6;
            this.gridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.gridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.gridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 78F));
            this.gridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.gridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 6F));
            this.gridPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.gridPanel.Size = new System.Drawing.Size(464, 406);
            this.gridPanel.TabIndex = 0;
            // 
            // headerPanel
            // 
            this.headerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.headerPanel.Controls.Add(this.label2);
            this.headerPanel.Controls.Add(this.settingsBtn);
            this.headerPanel.Controls.Add(this.autoTweetBtn);
            this.headerPanel.Controls.Add(this.logoutLink);
            this.headerPanel.Controls.Add(this.avatar);
            this.headerPanel.Controls.Add(this.accountName);
            this.headerPanel.Location = new System.Drawing.Point(32, 0);
            this.headerPanel.Margin = new System.Windows.Forms.Padding(0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(400, 48);
            this.headerPanel.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(0, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(400, 1);
            this.label2.TabIndex = 7;
            // 
            // settingsBtn
            // 
            this.settingsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsBtn.Image = global::TradeTweet.Properties.Resources.TradeTweet_24;
            this.settingsBtn.Location = new System.Drawing.Point(357, 1);
            this.settingsBtn.Name = "settingsBtn";
            this.settingsBtn.Size = new System.Drawing.Size(31, 40);
            this.settingsBtn.TabIndex = 6;
            this.settingsBtn.Click += new System.EventHandler(this.settingsBtn_Click);
            // 
            // autoTweetBtn
            // 
            this.autoTweetBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoTweetBtn.Image = global::TradeTweet.Properties.Resources.TradeTweet_21;
            this.autoTweetBtn.Location = new System.Drawing.Point(226, 1);
            this.autoTweetBtn.Name = "autoTweetBtn";
            this.autoTweetBtn.Size = new System.Drawing.Size(131, 41);
            this.autoTweetBtn.TabIndex = 5;
            this.autoTweetBtn.Click += new System.EventHandler(this.autoTweetBtn_Click);
            // 
            // logoutLink
            // 
            this.logoutLink.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(161)))), ((int)(((byte)(242)))));
            this.logoutLink.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logoutLink.AutoSize = true;
            this.logoutLink.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.logoutLink.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.logoutLink.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(161)))), ((int)(((byte)(242)))));
            this.logoutLink.Location = new System.Drawing.Point(44, 22);
            this.logoutLink.Name = "logoutLink";
            this.logoutLink.Size = new System.Drawing.Size(33, 12);
            this.logoutLink.TabIndex = 4;
            this.logoutLink.TabStop = true;
            this.logoutLink.Text = "Logout";
            this.logoutLink.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(161)))), ((int)(((byte)(242)))));
            this.logoutLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.logoutLink_LinkClicked);
            // 
            // avatar
            // 
            this.avatar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.avatar.Image = global::TradeTweet.Properties.Resources.avatar;
            this.avatar.Location = new System.Drawing.Point(10, 6);
            this.avatar.Margin = new System.Windows.Forms.Padding(5);
            this.avatar.Name = "avatar";
            this.avatar.Size = new System.Drawing.Size(28, 28);
            this.avatar.TabIndex = 0;
            this.avatar.TabStop = false;
            // 
            // accountName
            // 
            this.accountName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountName.AutoSize = true;
            this.accountName.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.accountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(116)))), ((int)(((byte)(116)))));
            this.accountName.Location = new System.Drawing.Point(42, 6);
            this.accountName.Name = "accountName";
            this.accountName.Size = new System.Drawing.Size(64, 14);
            this.accountName.TabIndex = 3;
            this.accountName.Text = "nazar_ptmc";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel3.Controls.Add(this.label16);
            this.panel3.Controls.Add(this.tweetText);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Location = new System.Drawing.Point(32, 199);
            this.panel3.Margin = new System.Windows.Forms.Padding(0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(400, 78);
            this.panel3.TabIndex = 3;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(71)))), ((int)(((byte)(71)))));
            this.label16.Location = new System.Drawing.Point(0, -9);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(400, 1);
            this.label16.TabIndex = 8;
            // 
            // tweetText
            // 
            this.tweetText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tweetText.BackColor = System.Drawing.Color.Black;
            this.tweetText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tweetText.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.tweetText.ForeColor = System.Drawing.Color.White;
            this.tweetText.Location = new System.Drawing.Point(5, 15);
            this.tweetText.Margin = new System.Windows.Forms.Padding(5);
            this.tweetText.MaxLength = 140;
            this.tweetText.Multiline = true;
            this.tweetText.Name = "tweetText";
            this.tweetText.Size = new System.Drawing.Size(390, 58);
            this.tweetText.TabIndex = 0;
            this.tweetText.Text = "Type here...";
            this.tweetText.TextChanged += new System.EventHandler(this.tweetText_TextChanged);
            this.tweetText.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tweetText_KeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Image = global::TradeTweet.Properties.Resources.rd_corner;
            this.label1.Location = new System.Drawing.Point(396, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(4, 4);
            this.label1.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Black;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Image = global::TradeTweet.Properties.Resources.lu_corner;
            this.label4.Location = new System.Drawing.Point(0, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(4, 4);
            this.label4.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(0, 10);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(400, 68);
            this.label5.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel1.Controls.Add(this.tweetBtn);
            this.panel1.Controls.Add(this.makeScreenshotBtn);
            this.panel1.Controls.Add(this.addImageBtn);
            this.panel1.Controls.Add(this.tweetLengthLabel);
            this.panel1.Location = new System.Drawing.Point(32, 356);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 50);
            this.panel1.TabIndex = 1;
            // 
            // tweetBtn
            // 
            this.tweetBtn.Enabled = false;
            this.tweetBtn.Image = global::TradeTweet.Properties.Resources.TradeTweet_13;
            this.tweetBtn.Location = new System.Drawing.Point(287, 10);
            this.tweetBtn.Name = "tweetBtn";
            this.tweetBtn.Size = new System.Drawing.Size(102, 30);
            this.tweetBtn.TabIndex = 6;
            this.tweetBtn.Click += new System.EventHandler(this.tweetBtn_Click);
            // 
            // makeScreenshotBtn
            // 
            this.makeScreenshotBtn.Image = global::TradeTweet.Properties.Resources.TradeTweet_06;
            this.makeScreenshotBtn.Location = new System.Drawing.Point(66, 10);
            this.makeScreenshotBtn.Name = "makeScreenshotBtn";
            this.makeScreenshotBtn.Size = new System.Drawing.Size(47, 30);
            this.makeScreenshotBtn.TabIndex = 6;
            this.makeScreenshotBtn.Click += new System.EventHandler(this.makeScreenshotBtn_Click);
            this.makeScreenshotBtn.MouseEnter += new System.EventHandler(this.makeScreenshotBtn_MouseEnter);
            this.makeScreenshotBtn.MouseLeave += new System.EventHandler(this.makeScreenshotBtn_MouseLeave);
            // 
            // addImageBtn
            // 
            this.addImageBtn.Image = global::TradeTweet.Properties.Resources.TradeTweet_05;
            this.addImageBtn.Location = new System.Drawing.Point(13, 10);
            this.addImageBtn.Name = "addImageBtn";
            this.addImageBtn.Size = new System.Drawing.Size(47, 30);
            this.addImageBtn.TabIndex = 6;
            this.addImageBtn.Click += new System.EventHandler(this.addImageBtn_Click);
            this.addImageBtn.MouseEnter += new System.EventHandler(this.addImageBtn_MouseEnter);
            this.addImageBtn.MouseLeave += new System.EventHandler(this.addImageBtn_MouseLeave);
            // 
            // tweetLengthLabel
            // 
            this.tweetLengthLabel.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.tweetLengthLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(116)))), ((int)(((byte)(116)))), ((int)(((byte)(116)))));
            this.tweetLengthLabel.Location = new System.Drawing.Point(226, 16);
            this.tweetLengthLabel.Name = "tweetLengthLabel";
            this.tweetLengthLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tweetLengthLabel.Size = new System.Drawing.Size(40, 16);
            this.tweetLengthLabel.TabIndex = 3;
            this.tweetLengthLabel.Text = "140";
            this.tweetLengthLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.panel5.Controls.Add(this.separatorLine);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(32, 350);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(400, 6);
            this.panel5.TabIndex = 5;
            // 
            // separatorLine
            // 
            this.separatorLine.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separatorLine.BackColor = System.Drawing.Color.Black;
            this.separatorLine.Controls.Add(this.leftCorner);
            this.separatorLine.Controls.Add(this.rightCorner);
            this.separatorLine.Location = new System.Drawing.Point(0, 0);
            this.separatorLine.Margin = new System.Windows.Forms.Padding(0);
            this.separatorLine.Name = "separatorLine";
            this.separatorLine.Size = new System.Drawing.Size(400, 4);
            this.separatorLine.TabIndex = 8;
            // 
            // leftCorner
            // 
            this.leftCorner.BackColor = System.Drawing.Color.Transparent;
            this.leftCorner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.leftCorner.Image = global::TradeTweet.Properties.Resources.ld_corner;
            this.leftCorner.Location = new System.Drawing.Point(0, 0);
            this.leftCorner.Margin = new System.Windows.Forms.Padding(0);
            this.leftCorner.Name = "leftCorner";
            this.leftCorner.Size = new System.Drawing.Size(4, 4);
            this.leftCorner.TabIndex = 4;
            // 
            // rightCorner
            // 
            this.rightCorner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rightCorner.BackColor = System.Drawing.Color.Transparent;
            this.rightCorner.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rightCorner.Image = global::TradeTweet.Properties.Resources.ru_corner;
            this.rightCorner.Location = new System.Drawing.Point(396, 0);
            this.rightCorner.Margin = new System.Windows.Forms.Padding(0);
            this.rightCorner.Name = "rightCorner";
            this.rightCorner.Size = new System.Drawing.Size(4, 4);
            this.rightCorner.TabIndex = 2;
            // 
            // picPanelBase
            // 
            this.picPanelBase.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.picPanelBase.Controls.Add(this.label15);
            this.picPanelBase.Controls.Add(this.picPanelContainer);
            this.picPanelBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPanelBase.Location = new System.Drawing.Point(32, 277);
            this.picPanelBase.Margin = new System.Windows.Forms.Padding(0);
            this.picPanelBase.Name = "picPanelBase";
            this.picPanelBase.Size = new System.Drawing.Size(400, 73);
            this.picPanelBase.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.label15.Location = new System.Drawing.Point(0, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(400, 1);
            this.label15.TabIndex = 8;
            // 
            // picPanelContainer
            // 
            this.picPanelContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.picPanelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picPanelContainer.Location = new System.Drawing.Point(0, 0);
            this.picPanelContainer.Name = "picPanelContainer";
            this.picPanelContainer.Size = new System.Drawing.Size(400, 73);
            this.picPanelContainer.TabIndex = 7;
            // 
            // historyPanel
            // 
            this.historyPanel.AutoScroll = true;
            this.historyPanel.AutoSize = true;
            this.historyPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("historyPanel.BackgroundImage")));
            this.historyPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.historyPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.historyPanel.Location = new System.Drawing.Point(32, 48);
            this.historyPanel.Margin = new System.Windows.Forms.Padding(0);
            this.historyPanel.Name = "historyPanel";
            this.historyPanel.Size = new System.Drawing.Size(400, 151);
            this.historyPanel.TabIndex = 7;
            this.historyPanel.WrapContents = false;
            // 
            // TPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.gridPanel);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TPanel";
            this.Size = new System.Drawing.Size(470, 413);
            this.gridPanel.ResumeLayout(false);
            this.gridPanel.PerformLayout();
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.avatar)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.separatorLine.ResumeLayout(false);
            this.picPanelBase.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel gridPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tweetText;
        private System.Windows.Forms.Label leftCorner;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label rightCorner;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.PictureBox avatar;
        private System.Windows.Forms.Label accountName;
        private System.Windows.Forms.LinkLabel logoutLink;
        private System.Windows.Forms.Label settingsBtn;
        private System.Windows.Forms.Label autoTweetBtn;
        private System.Windows.Forms.Label makeScreenshotBtn;
        private System.Windows.Forms.Label addImageBtn;
        private System.Windows.Forms.Label tweetBtn;
        private System.Windows.Forms.Label tweetLengthLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label picPanelContainer;
        private System.Windows.Forms.Panel picPanelBase;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel separatorLine;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label2;
        private TradeTweet.CustomPanel historyPanel;
    }


}
