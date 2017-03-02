namespace TradeTweet
{
    partial class AutoSettings
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
            Settings.onSettingsChanged -= RefreshTree;

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
            this.components = new System.ComponentModel.Container();
            this.settingsTree = new TradeTweet.TriStateTreeView();
            this.SuspendLayout();
            // 
            // settingsTree
            // 
            this.settingsTree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.settingsTree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.settingsTree.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.settingsTree.Indent = 20;
            this.settingsTree.ItemHeight = 20;
            this.settingsTree.Location = new System.Drawing.Point(0, 0);
            this.settingsTree.Margin = new System.Windows.Forms.Padding(0);
            this.settingsTree.Name = "settingsTree";
            this.settingsTree.Size = new System.Drawing.Size(164, 162);
            this.settingsTree.TabIndex = 0;
            this.settingsTree.TriStateStyleProperty = TradeTweet.TriStateTreeView.TriStateStyles.Standard;
            // 
            // AutoSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.settingsTree);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "AutoSettings";
            this.Size = new System.Drawing.Size(164, 162);
            this.ResumeLayout(false);

        }

        #endregion

        private TradeTweet.TriStateTreeView settingsTree;
    }
}
