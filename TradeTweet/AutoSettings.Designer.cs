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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Node2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Node10");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Open Order", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Node5");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Node6");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Node7");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Node4", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8});
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.treeView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(6, -30);
            this.treeView1.Name = "treeView1";
            treeNode1.Checked = true;
            treeNode1.Name = "Node1";
            treeNode1.Text = "Node1";
            treeNode2.Checked = true;
            treeNode2.Name = "Node2";
            treeNode2.Text = "Node2";
            treeNode3.Checked = true;
            treeNode3.Name = "Node3";
            treeNode3.Text = "Node3";
            treeNode4.Name = "Node10";
            treeNode4.Text = "Node10";
            treeNode5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            treeNode5.Checked = true;
            treeNode5.ForeColor = System.Drawing.Color.LightGray;
            treeNode5.Name = "OpenOrder";
            treeNode5.NodeFont = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            treeNode5.Text = "Open Order";
            treeNode6.Checked = true;
            treeNode6.Name = "Node5";
            treeNode6.Text = "Node5";
            treeNode7.Checked = true;
            treeNode7.Name = "Node6";
            treeNode7.Text = "Node6";
            treeNode8.Checked = true;
            treeNode8.Name = "Node7";
            treeNode8.Text = "Node7";
            treeNode9.Checked = true;
            treeNode9.Name = "Node4";
            treeNode9.Text = "Node4";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode9});
            this.treeView1.ShowLines = false;
            this.treeView1.ShowRootLines = false;
            this.treeView1.Size = new System.Drawing.Size(162, 141);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label1
            // 
            this.label1.Image = global::TradeTweet.Properties.Resources.transp_left_corner;
            this.label1.Location = new System.Drawing.Point(0, 158);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(4, 4);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.Image = global::TradeTweet.Properties.Resources.transp_right_corner;
            this.label2.Location = new System.Drawing.Point(158, 158);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(4, 4);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // AutoSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.treeView1);
            this.Name = "AutoSettings";
            this.Size = new System.Drawing.Size(162, 162);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
