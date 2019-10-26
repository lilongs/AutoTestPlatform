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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sysConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cANConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.equipmentConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.maualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iCRefreshCycleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureSensorConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cOMConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.temperatureConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerMeterAddressConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerMeterAddressConfigurationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TestTypeEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.TestSequenceEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.equipmentTestInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.histroyDataRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.historicalInformationReviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ammeterConfiguration = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.xtraTabControl1 = new DevExpress.XtraTab.XtraTabControl();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.combIC = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnResume = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.combIC.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sysConfigToolStripMenuItem,
            this.temperatureSensorConfigurationToolStripMenuItem,
            this.powerMeterAddressConfigurationToolStripMenuItem,
            this.editToolStripMenuItem,
            this.histroyDataRecordToolStripMenuItem});
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
            this.equipmentConfiguration,
            this.maualToolStripMenuItem,
            this.iCRefreshCycleToolStripMenuItem});
            this.sysConfigToolStripMenuItem.Name = "sysConfigToolStripMenuItem";
            this.sysConfigToolStripMenuItem.Size = new System.Drawing.Size(72, 21);
            this.sysConfigToolStripMenuItem.Text = "IC config";
            // 
            // cANConfiguration
            // 
            this.cANConfiguration.Name = "cANConfiguration";
            this.cANConfiguration.Size = new System.Drawing.Size(223, 22);
            this.cANConfiguration.Text = "CAN config";
            this.cANConfiguration.Click += new System.EventHandler(this.cANConfiguration_Click);
            // 
            // equipmentConfiguration
            // 
            this.equipmentConfiguration.Name = "equipmentConfiguration";
            this.equipmentConfiguration.Size = new System.Drawing.Size(223, 22);
            this.equipmentConfiguration.Text = "IC paramter config";
            this.equipmentConfiguration.Click += new System.EventHandler(this.equipmentConfiguration_Click);
            // 
            // maualToolStripMenuItem
            // 
            this.maualToolStripMenuItem.Name = "maualToolStripMenuItem";
            this.maualToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.maualToolStripMenuItem.Text = "Manual instruction config";
            this.maualToolStripMenuItem.Click += new System.EventHandler(this.maualToolStripMenuItem_Click);
            // 
            // iCRefreshCycleToolStripMenuItem
            // 
            this.iCRefreshCycleToolStripMenuItem.Name = "iCRefreshCycleToolStripMenuItem";
            this.iCRefreshCycleToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.iCRefreshCycleToolStripMenuItem.Text = "IC refresh cycle";
            this.iCRefreshCycleToolStripMenuItem.Click += new System.EventHandler(this.iCRefreshCycleToolStripMenuItem_Click);
            // 
            // temperatureSensorConfigurationToolStripMenuItem
            // 
            this.temperatureSensorConfigurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cOMConfigurationToolStripMenuItem,
            this.temperatureConfigurationToolStripMenuItem});
            this.temperatureSensorConfigurationToolStripMenuItem.Name = "temperatureSensorConfigurationToolStripMenuItem";
            this.temperatureSensorConfigurationToolStripMenuItem.Size = new System.Drawing.Size(178, 21);
            this.temperatureSensorConfigurationToolStripMenuItem.Text = "Temperature sensor config";
            this.temperatureSensorConfigurationToolStripMenuItem.Visible = false;
            // 
            // cOMConfigurationToolStripMenuItem
            // 
            this.cOMConfigurationToolStripMenuItem.Name = "cOMConfigurationToolStripMenuItem";
            this.cOMConfigurationToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.cOMConfigurationToolStripMenuItem.Text = "COM config";
            this.cOMConfigurationToolStripMenuItem.Click += new System.EventHandler(this.cOMConfiguration_Click);
            // 
            // temperatureConfigurationToolStripMenuItem
            // 
            this.temperatureConfigurationToolStripMenuItem.Name = "temperatureConfigurationToolStripMenuItem";
            this.temperatureConfigurationToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.temperatureConfigurationToolStripMenuItem.Text = "Temperature config";
            this.temperatureConfigurationToolStripMenuItem.Click += new System.EventHandler(this.temperatureConfiguration_Click);
            // 
            // powerMeterAddressConfigurationToolStripMenuItem
            // 
            this.powerMeterAddressConfigurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.powerMeterAddressConfigurationToolStripMenuItem1});
            this.powerMeterAddressConfigurationToolStripMenuItem.Name = "powerMeterAddressConfigurationToolStripMenuItem";
            this.powerMeterAddressConfigurationToolStripMenuItem.Size = new System.Drawing.Size(145, 21);
            this.powerMeterAddressConfigurationToolStripMenuItem.Text = "Powerm meter config";
            // 
            // powerMeterAddressConfigurationToolStripMenuItem1
            // 
            this.powerMeterAddressConfigurationToolStripMenuItem1.Name = "powerMeterAddressConfigurationToolStripMenuItem1";
            this.powerMeterAddressConfigurationToolStripMenuItem1.Size = new System.Drawing.Size(190, 22);
            this.powerMeterAddressConfigurationToolStripMenuItem1.Text = "Power meter config";
            this.powerMeterAddressConfigurationToolStripMenuItem1.Click += new System.EventHandler(this.powerMeterAddressConfigurationToolStripMenuItem1_Click);
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
            // equipmentTestInfo
            // 
            this.equipmentTestInfo.Name = "equipmentTestInfo";
            this.equipmentTestInfo.Size = new System.Drawing.Size(178, 22);
            this.equipmentTestInfo.Text = "IC test info";
            this.equipmentTestInfo.Click += new System.EventHandler(this.equipmentTestInfo_Click);
            // 
            // histroyDataRecordToolStripMenuItem
            // 
            this.histroyDataRecordToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.historicalInformationReviewToolStripMenuItem});
            this.histroyDataRecordToolStripMenuItem.Name = "histroyDataRecordToolStripMenuItem";
            this.histroyDataRecordToolStripMenuItem.Size = new System.Drawing.Size(186, 21);
            this.histroyDataRecordToolStripMenuItem.Text = "Historical information review";
            // 
            // historicalInformationReviewToolStripMenuItem
            // 
            this.historicalInformationReviewToolStripMenuItem.Name = "historicalInformationReviewToolStripMenuItem";
            this.historicalInformationReviewToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.historicalInformationReviewToolStripMenuItem.Text = "Historical information review";
            this.historicalInformationReviewToolStripMenuItem.Click += new System.EventHandler(this.historicalInformationReviewToolStripMenuItem_Click);
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
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(402, 646);
            this.panel4.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(402, 0);
            this.panel3.TabIndex = 0;
            // 
            // xtraTabControl1
            // 
            this.xtraTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xtraTabControl1.Location = new System.Drawing.Point(2, 2);
            this.xtraTabControl1.Name = "xtraTabControl1";
            this.xtraTabControl1.Size = new System.Drawing.Size(1177, 642);
            this.xtraTabControl1.TabIndex = 14;
            // 
            // groupControl4
            // 
            this.groupControl4.AllowBorderColorBlending = true;
            this.groupControl4.AppearanceCaption.BorderColor = System.Drawing.Color.LightGreen;
            this.groupControl4.AppearanceCaption.Font = new System.Drawing.Font("Arial", 10F);
            this.groupControl4.AppearanceCaption.Options.UseBorderColor = true;
            this.groupControl4.AppearanceCaption.Options.UseFont = true;
            this.groupControl4.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("groupControl4.CaptionImageOptions.Image")));
            this.groupControl4.Controls.Add(this.panel5);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl4.Location = new System.Drawing.Point(0, 25);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(214, 646);
            this.groupControl4.TabIndex = 15;
            this.groupControl4.Text = "Select test step:";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.combIC);
            this.panel5.Controls.Add(this.btnStop);
            this.panel5.Controls.Add(this.btnResume);
            this.panel5.Controls.Add(this.btnStart);
            this.panel5.Controls.Add(this.btnPause);
            this.panel5.Controls.Add(this.treeView1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(2, 39);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(210, 605);
            this.panel5.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select test IC:";
            // 
            // combIC
            // 
            this.combIC.Location = new System.Drawing.Point(6, 30);
            this.combIC.Name = "combIC";
            this.combIC.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.combIC.Size = new System.Drawing.Size(199, 20);
            this.combIC.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStop.BackColor = System.Drawing.Color.Red;
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new System.Drawing.Point(1, 556);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(90, 30);
            this.btnStop.TabIndex = 15;
            this.btnStop.Text = "TestStop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnResume
            // 
            this.btnResume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResume.BackColor = System.Drawing.Color.Yellow;
            this.btnResume.Enabled = false;
            this.btnResume.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResume.Location = new System.Drawing.Point(112, 556);
            this.btnResume.Name = "btnResume";
            this.btnResume.Size = new System.Drawing.Size(90, 30);
            this.btnResume.TabIndex = 15;
            this.btnResume.Text = "TestResume";
            this.btnResume.UseVisualStyleBackColor = false;
            this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new System.Drawing.Point(1, 517);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(90, 30);
            this.btnStart.TabIndex = 12;
            this.btnStart.Text = "TestStart";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPause.BackColor = System.Drawing.Color.Yellow;
            this.btnPause.Enabled = false;
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.Location = new System.Drawing.Point(112, 517);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(90, 30);
            this.btnPause.TabIndex = 12;
            this.btnPause.Text = "TestPause";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // treeView1
            // 
            this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treeView1.BackColor = System.Drawing.Color.White;
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(-2, 56);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(210, 455);
            this.treeView1.TabIndex = 14;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.xtraTabControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(214, 25);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1181, 646);
            this.panelControl1.TabIndex = 16;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1797, 671);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.groupControl4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Instrument cluster test bench";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.xtraTabControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.combIC.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
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
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem cANConfiguration;
        private System.Windows.Forms.ToolStripMenuItem equipmentConfiguration;
        private System.Windows.Forms.ToolStripMenuItem equipmentTestInfo;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraTab.XtraTabControl xtraTabControl1;
        private System.Windows.Forms.ToolStripMenuItem maualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powerMeterAddressConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powerMeterAddressConfigurationToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem temperatureSensorConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cOMConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem temperatureConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem histroyDataRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem historicalInformationReviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iCRefreshCycleToolStripMenuItem;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        public System.Windows.Forms.TreeView treeView1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.CheckedComboBoxEdit combIC;
        private System.Windows.Forms.Label label1;
    }
}

