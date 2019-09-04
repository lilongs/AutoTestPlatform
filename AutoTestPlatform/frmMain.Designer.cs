namespace AutoTestPlatform
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sysConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ammeterConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestTypeEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.TestSequenceEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.txtInfo = new System.Windows.Forms.ListBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sysConfigToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1335, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sysConfigToolStripMenuItem
            // 
            this.sysConfigToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ammeterConfiguration,
            this.temperatureConfiguration});
            this.sysConfigToolStripMenuItem.Name = "sysConfigToolStripMenuItem";
            this.sysConfigToolStripMenuItem.Size = new System.Drawing.Size(77, 21);
            this.sysConfigToolStripMenuItem.Text = "SysConfig";
            // 
            // ammeterConfiguration
            // 
            this.ammeterConfiguration.Name = "ammeterConfiguration";
            this.ammeterConfiguration.Size = new System.Drawing.Size(230, 22);
            this.ammeterConfiguration.Text = "AmmeterConfiguration";
            this.ammeterConfiguration.Click += new System.EventHandler(this.ammeterConfiguration_Click);
            // 
            // temperatureConfiguration
            // 
            this.temperatureConfiguration.Name = "temperatureConfiguration";
            this.temperatureConfiguration.Size = new System.Drawing.Size(230, 22);
            this.temperatureConfiguration.Text = "TemperatureConfiguration";
            this.temperatureConfiguration.Click += new System.EventHandler(this.temperatureConfiguration_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TestTypeEdit,
            this.TestSequenceEdit});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(66, 21);
            this.editToolStripMenuItem.Text = "TestEdit";
            // 
            // TestTypeEdit
            // 
            this.TestTypeEdit.Name = "TestTypeEdit";
            this.TestTypeEdit.Size = new System.Drawing.Size(178, 22);
            this.TestTypeEdit.Text = "TestTypeEdit";
            this.TestTypeEdit.Click += new System.EventHandler(this.TestTypeEdit_Click);
            // 
            // TestSequenceEdit
            // 
            this.TestSequenceEdit.Name = "TestSequenceEdit";
            this.TestSequenceEdit.Size = new System.Drawing.Size(178, 22);
            this.TestSequenceEdit.Text = "TestSequenceEdit";
            this.TestSequenceEdit.Click += new System.EventHandler(this.TestSequenceEdit_Click);
            // 
            // txtInfo
            // 
            this.txtInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfo.FormattingEnabled = true;
            this.txtInfo.ItemHeight = 12;
            this.txtInfo.Location = new System.Drawing.Point(3, 17);
            this.txtInfo.Name = "txtInfo";
            this.txtInfo.Size = new System.Drawing.Size(1329, 245);
            this.txtInfo.TabIndex = 7;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(129, 36);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(83, 35);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "TestStart";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "Select Test Steps:";
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(0, 77);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(391, 323);
            this.treeView1.TabIndex = 10;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtInfo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 406);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1335, 265);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "MsgInfo";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(397, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(935, 364);
            this.panel1.TabIndex = 12;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1335, 671);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestPlatform";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TestSequenceEdit;
        private System.Windows.Forms.ToolStripMenuItem TestTypeEdit;
        private System.Windows.Forms.ListBox txtInfo;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem sysConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ammeterConfiguration;
        private System.Windows.Forms.ToolStripMenuItem temperatureConfiguration;
        private System.Windows.Forms.Panel panel1;
    }
}

