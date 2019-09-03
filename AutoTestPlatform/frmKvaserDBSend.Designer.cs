namespace AutoTestPlatform
{
    partial class frmKvaserDBSend
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.loadDbButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.msgIdLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.boxItems = new System.Windows.Forms.ComboBox();
            this.loadMsgButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.channelBox = new System.Windows.Forms.TextBox();
            this.initButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.sendMsgButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.intervalBox = new System.Windows.Forms.TextBox();
            this.startAutoButton = new System.Windows.Forms.Button();
            this.stopTransmitButton = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.selectBlock = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // loadDbButton
            // 
            this.loadDbButton.Location = new System.Drawing.Point(50, 25);
            this.loadDbButton.Name = "loadDbButton";
            this.loadDbButton.Size = new System.Drawing.Size(102, 33);
            this.loadDbButton.TabIndex = 0;
            this.loadDbButton.Text = "Load DataBase";
            this.loadDbButton.UseVisualStyleBackColor = true;
            this.loadDbButton.Click += new System.EventHandler(this.loadDbButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 428);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(800, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusText
            // 
            this.statusText.Name = "statusText";
            this.statusText.Size = new System.Drawing.Size(131, 17);
            this.statusText.Text = "toolStripStatusLabel1";
            // 
            // msgIdLabel
            // 
            this.msgIdLabel.AutoSize = true;
            this.msgIdLabel.Location = new System.Drawing.Point(48, 199);
            this.msgIdLabel.Name = "msgIdLabel";
            this.msgIdLabel.Size = new System.Drawing.Size(0, 12);
            this.msgIdLabel.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Selected file:";
            // 
            // boxItems
            // 
            this.boxItems.FormattingEnabled = true;
            this.boxItems.Location = new System.Drawing.Point(50, 128);
            this.boxItems.Name = "boxItems";
            this.boxItems.Size = new System.Drawing.Size(121, 20);
            this.boxItems.TabIndex = 2;
            // 
            // loadMsgButton
            // 
            this.loadMsgButton.Enabled = false;
            this.loadMsgButton.Location = new System.Drawing.Point(50, 154);
            this.loadMsgButton.Name = "loadMsgButton";
            this.loadMsgButton.Size = new System.Drawing.Size(73, 33);
            this.loadMsgButton.TabIndex = 0;
            this.loadMsgButton.Text = "Load";
            this.loadMsgButton.UseVisualStyleBackColor = true;
            this.loadMsgButton.Click += new System.EventHandler(this.loadMsgButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(308, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Channel:";
            // 
            // channelBox
            // 
            this.channelBox.Location = new System.Drawing.Point(367, 32);
            this.channelBox.Name = "channelBox";
            this.channelBox.Size = new System.Drawing.Size(100, 21);
            this.channelBox.TabIndex = 6;
            this.channelBox.Text = "0";
            // 
            // initButton
            // 
            this.initButton.Location = new System.Drawing.Point(483, 27);
            this.initButton.Name = "initButton";
            this.initButton.Size = new System.Drawing.Size(90, 28);
            this.initButton.TabIndex = 7;
            this.initButton.Text = "Initiate";
            this.initButton.UseVisualStyleBackColor = true;
            this.initButton.Click += new System.EventHandler(this.initButton_Click);
            // 
            // closeButton
            // 
            this.closeButton.Enabled = false;
            this.closeButton.Location = new System.Drawing.Point(579, 27);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(90, 28);
            this.closeButton.TabIndex = 7;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // sendMsgButton
            // 
            this.sendMsgButton.Enabled = false;
            this.sendMsgButton.Location = new System.Drawing.Point(310, 65);
            this.sendMsgButton.Name = "sendMsgButton";
            this.sendMsgButton.Size = new System.Drawing.Size(90, 28);
            this.sendMsgButton.TabIndex = 7;
            this.sendMsgButton.Text = "Send message";
            this.sendMsgButton.UseVisualStyleBackColor = true;
            this.sendMsgButton.Click += new System.EventHandler(this.sendMsgButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(308, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Interval (ms)";
            // 
            // intervalBox
            // 
            this.intervalBox.Location = new System.Drawing.Point(310, 128);
            this.intervalBox.Name = "intervalBox";
            this.intervalBox.Size = new System.Drawing.Size(100, 21);
            this.intervalBox.TabIndex = 8;
            this.intervalBox.Text = "500";
            // 
            // startAutoButton
            // 
            this.startAutoButton.Enabled = false;
            this.startAutoButton.Location = new System.Drawing.Point(310, 169);
            this.startAutoButton.Name = "startAutoButton";
            this.startAutoButton.Size = new System.Drawing.Size(157, 28);
            this.startAutoButton.TabIndex = 9;
            this.startAutoButton.Text = "Start auto transmit";
            this.startAutoButton.UseVisualStyleBackColor = true;
            this.startAutoButton.Click += new System.EventHandler(this.startAutoButton_Click);
            // 
            // stopTransmitButton
            // 
            this.stopTransmitButton.Enabled = false;
            this.stopTransmitButton.Location = new System.Drawing.Point(473, 169);
            this.stopTransmitButton.Name = "stopTransmitButton";
            this.stopTransmitButton.Size = new System.Drawing.Size(157, 28);
            this.stopTransmitButton.TabIndex = 9;
            this.stopTransmitButton.Text = "stopTransmitButton";
            this.stopTransmitButton.UseVisualStyleBackColor = true;
            this.stopTransmitButton.Click += new System.EventHandler(this.stopTransmitButton_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(50, 226);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(619, 188);
            this.dataGridView1.TabIndex = 10;
            // 
            // selectBlock
            // 
            this.selectBlock.Location = new System.Drawing.Point(143, 75);
            this.selectBlock.Name = "selectBlock";
            this.selectBlock.Size = new System.Drawing.Size(89, 50);
            this.selectBlock.TabIndex = 3;
            // 
            // frmKvaserDBSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.stopTransmitButton);
            this.Controls.Add(this.startAutoButton);
            this.Controls.Add(this.intervalBox);
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.sendMsgButton);
            this.Controls.Add(this.initButton);
            this.Controls.Add(this.channelBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.selectBlock);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.msgIdLabel);
            this.Controls.Add(this.boxItems);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.loadMsgButton);
            this.Controls.Add(this.loadDbButton);
            this.Name = "frmKvaserDBSend";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmKvaserDBSend";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadDbButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.Label msgIdLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox boxItems;
        private System.Windows.Forms.Button loadMsgButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox channelBox;
        private System.Windows.Forms.Button initButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button sendMsgButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox intervalBox;
        private System.Windows.Forms.Button startAutoButton;
        private System.Windows.Forms.Button stopTransmitButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label selectBlock;
    }
}