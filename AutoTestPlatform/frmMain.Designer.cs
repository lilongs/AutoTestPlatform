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
            this.cANConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.equipmentConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestTypeEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.TestSequenceEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.equipmentTestInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.ammeterConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.maualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sysConfigToolStripMenuItem,
            this.editToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1797, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sysConfigToolStripMenuItem
            // 
            this.sysConfigToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cANConfiguration,
            this.cOMConfiguration,
            this.equipmentConfiguration,
            this.temperatureConfiguration,
            this.maualToolStripMenuItem});
            this.sysConfigToolStripMenuItem.Name = "sysConfigToolStripMenuItem";
            this.sysConfigToolStripMenuItem.Size = new System.Drawing.Size(144, 21);
            this.sysConfigToolStripMenuItem.Text = "System Configuration";
            // 
            // cANConfiguration
            // 
            this.cANConfiguration.Name = "cANConfiguration";
            this.cANConfiguration.Size = new System.Drawing.Size(266, 22);
            this.cANConfiguration.Text = "CAN Configuration";
            this.cANConfiguration.Click += new System.EventHandler(this.cANConfiguration_Click);
            // 
            // cOMConfiguration
            // 
            this.cOMConfiguration.Name = "cOMConfiguration";
            this.cOMConfiguration.Size = new System.Drawing.Size(266, 22);
            this.cOMConfiguration.Text = "COM Configuration";
            this.cOMConfiguration.Click += new System.EventHandler(this.cOMConfiguration_Click);
            // 
            // equipmentConfiguration
            // 
            this.equipmentConfiguration.Name = "equipmentConfiguration";
            this.equipmentConfiguration.Size = new System.Drawing.Size(266, 22);
            this.equipmentConfiguration.Text = "Instrument Cluster Configuration";
            this.equipmentConfiguration.Click += new System.EventHandler(this.equipmentConfiguration_Click);
            // 
            // temperatureConfiguration
            // 
            this.temperatureConfiguration.Name = "temperatureConfiguration";
            this.temperatureConfiguration.Size = new System.Drawing.Size(266, 22);
            this.temperatureConfiguration.Text = "Temperature Configuration";
            this.temperatureConfiguration.Click += new System.EventHandler(this.temperatureConfiguration_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TestTypeEdit,
            this.TestSequenceEdit,
            this.equipmentTestInfo});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(66, 21);
            this.editToolStripMenuItem.Text = "TestEdit";
            // 
            // TestTypeEdit
            // 
            this.TestTypeEdit.Name = "TestTypeEdit";
            this.TestTypeEdit.Size = new System.Drawing.Size(225, 22);
            this.TestTypeEdit.Text = "TestTypeEdit";
            this.TestTypeEdit.Click += new System.EventHandler(this.TestTypeEdit_Click);
            // 
            // TestSequenceEdit
            // 
            this.TestSequenceEdit.Name = "TestSequenceEdit";
            this.TestSequenceEdit.Size = new System.Drawing.Size(225, 22);
            this.TestSequenceEdit.Text = "TestSequenceEdit";
            this.TestSequenceEdit.Click += new System.EventHandler(this.TestSequenceEdit_Click);
            // 
            // equipmentTestInfo
            // 
            this.equipmentTestInfo.Name = "equipmentTestInfo";
            this.equipmentTestInfo.Size = new System.Drawing.Size(225, 22);
            this.equipmentTestInfo.Text = "InstrumentClusterTestInfo";
            this.equipmentTestInfo.Click += new System.EventHandler(this.equipmentTestInfo_Click);
            // 
            // ammeterConfiguration
            // 
            this.ammeterConfiguration.Name = "ammeterConfiguration";
            this.ammeterConfiguration.Size = new System.Drawing.Size(32, 19);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1395, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(402, 646);
            this.panel2.TabIndex = 13;
            // 
            // panel4
            // 
            this.panel4.AutoScroll = true;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 324);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(402, 322);
            this.panel4.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(402, 324);
            this.panel3.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(0, 25);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.Size = new System.Drawing.Size(1395, 646);
            this.xtraTabControl1.TabIndex = 14;
            // 
            // maualToolStripMenuItem
            // 
            this.maualToolStripMenuItem.Name = "maualToolStripMenuItem";
            this.maualToolStripMenuItem.Size = new System.Drawing.Size(267, 22);
            this.maualToolStripMenuItem.Text = "Manual Instruction Configuration";
            this.maualToolStripMenuItem.Click += new System.EventHandler(this.maualToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1797, 671);
            this.Controls.Add(this.xtraTabControl1);
            this.Controls.Add(this.panel2);
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
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem TestSequenceEdit;
        private System.Windows.Forms.ToolStripMenuItem TestTypeEdit;
        private System.Windows.Forms.ToolStripMenuItem sysConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ammeterConfiguration;
        private System.Windows.Forms.ToolStripMenuItem temperatureConfiguration;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem cANConfiguration;
        private System.Windows.Forms.ToolStripMenuItem cOMConfiguration;
        private System.Windows.Forms.ToolStripMenuItem equipmentConfiguration;
        private System.Windows.Forms.ToolStripMenuItem equipmentTestInfo;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private System.Windows.Forms.ToolStripMenuItem maualToolStripMenuItem;
    }
}

