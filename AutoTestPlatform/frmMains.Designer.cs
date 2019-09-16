namespace AutoTestPlatform
{
    partial class frmMains
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
            this.cANConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.equipmentConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestTypeEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.TestSequenceEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.equipmentTestInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
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
            this.ammeterConfiguration,
            this.cANConfiguration,
            this.cOMConfiguration,
            this.equipmentConfiguration,
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
            this.ammeterConfiguration.Visible = false;
            this.ammeterConfiguration.Click += new System.EventHandler(this.ammeterConfiguration_Click);
            // 
            // temperatureConfiguration
            // 
            this.temperatureConfiguration.Name = "temperatureConfiguration";
            this.temperatureConfiguration.Size = new System.Drawing.Size(230, 22);
            this.temperatureConfiguration.Text = "TemperatureConfiguration";
            this.temperatureConfiguration.Click += new System.EventHandler(this.temperatureConfiguration_Click);
            // 
            // cANConfiguration
            // 
            this.cANConfiguration.Name = "cANConfiguration";
            this.cANConfiguration.Size = new System.Drawing.Size(230, 22);
            this.cANConfiguration.Text = "CAN Configuration";
            this.cANConfiguration.Click += new System.EventHandler(this.cANConfiguration_Click);
            // 
            // cOMConfiguration
            // 
            this.cOMConfiguration.Name = "cOMConfiguration";
            this.cOMConfiguration.Size = new System.Drawing.Size(230, 22);
            this.cOMConfiguration.Text = "COM Configuration";
            this.cOMConfiguration.Click += new System.EventHandler(this.cOMConfiguration_Click);
            // 
            // equipmentConfiguration
            // 
            this.equipmentConfiguration.Name = "equipmentConfiguration";
            this.equipmentConfiguration.Size = new System.Drawing.Size(230, 22);
            this.equipmentConfiguration.Text = "Equipment Configuration";
            this.equipmentConfiguration.Click += new System.EventHandler(this.equipmentConfiguration_Click);
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
            this.TestTypeEdit.Size = new System.Drawing.Size(185, 22);
            this.TestTypeEdit.Text = "TestTypeEdit";
            this.TestTypeEdit.Click += new System.EventHandler(this.TestTypeEdit_Click);
            // 
            // TestSequenceEdit
            // 
            this.TestSequenceEdit.Name = "TestSequenceEdit";
            this.TestSequenceEdit.Size = new System.Drawing.Size(185, 22);
            this.TestSequenceEdit.Text = "TestSequenceEdit";
            this.TestSequenceEdit.Click += new System.EventHandler(this.TestSequenceEdit_Click);
            // 
            // equipmentTestInfo
            // 
            this.equipmentTestInfo.Name = "equipmentTestInfo";
            this.equipmentTestInfo.Size = new System.Drawing.Size(185, 22);
            this.equipmentTestInfo.Text = "EquipmentTestInfo";
            this.equipmentTestInfo.Click += new System.EventHandler(this.equipmentTestInfo_Click);
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1395, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(402, 646);
            this.panel2.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1395, 646);
            this.panel1.TabIndex = 14;
            // 
            // frmMains
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1797, 671);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMains";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestPlatform";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem cANConfiguration;
        private System.Windows.Forms.ToolStripMenuItem cOMConfiguration;
        private System.Windows.Forms.ToolStripMenuItem equipmentConfiguration;
        private System.Windows.Forms.ToolStripMenuItem equipmentTestInfo;
    }
}

