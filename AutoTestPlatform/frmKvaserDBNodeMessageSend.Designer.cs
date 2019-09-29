namespace AutoTestPlatform
{
    partial class frmKvaserDBNodeMessageSend
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
            this.label3 = new System.Windows.Forms.Label();
            this.channelBox = new System.Windows.Forms.TextBox();
            this.initButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.sendMsgButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.intervalBox = new System.Windows.Forms.TextBox();
            this.startAutoButton = new System.Windows.Forms.Button();
            this.stopTransmitButton = new System.Windows.Forms.Button();
            this.selectBlock = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // loadDbButton
            // 
            this.loadDbButton.Location = new System.Drawing.Point(21, 7);
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 571);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1060, 22);
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
            this.label2.Location = new System.Drawing.Point(129, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Selected file:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(692, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Channel:";
            // 
            // channelBox
            // 
            this.channelBox.Location = new System.Drawing.Point(751, 12);
            this.channelBox.Name = "channelBox";
            this.channelBox.Size = new System.Drawing.Size(100, 21);
            this.channelBox.TabIndex = 6;
            this.channelBox.Text = "0";
            // 
            // initButton
            // 
            this.initButton.Location = new System.Drawing.Point(867, 7);
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
            this.closeButton.Location = new System.Drawing.Point(963, 7);
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
            this.sendMsgButton.Location = new System.Drawing.Point(694, 45);
            this.sendMsgButton.Name = "sendMsgButton";
            this.sendMsgButton.Size = new System.Drawing.Size(157, 28);
            this.sendMsgButton.TabIndex = 7;
            this.sendMsgButton.Text = "Send all message once";
            this.sendMsgButton.UseVisualStyleBackColor = true;
            this.sendMsgButton.Click += new System.EventHandler(this.sendMsgButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(692, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Interval (ms)";
            // 
            // intervalBox
            // 
            this.intervalBox.Location = new System.Drawing.Point(694, 108);
            this.intervalBox.Name = "intervalBox";
            this.intervalBox.Size = new System.Drawing.Size(100, 21);
            this.intervalBox.TabIndex = 8;
            this.intervalBox.Text = "500";
            // 
            // startAutoButton
            // 
            this.startAutoButton.Enabled = false;
            this.startAutoButton.Location = new System.Drawing.Point(694, 149);
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
            this.stopTransmitButton.Location = new System.Drawing.Point(857, 149);
            this.stopTransmitButton.Name = "stopTransmitButton";
            this.stopTransmitButton.Size = new System.Drawing.Size(157, 28);
            this.stopTransmitButton.TabIndex = 9;
            this.stopTransmitButton.Text = "stopTransmitButton";
            this.stopTransmitButton.UseVisualStyleBackColor = true;
            this.stopTransmitButton.Click += new System.EventHandler(this.stopTransmitButton_Click);
            // 
            // selectBlock
            // 
            this.selectBlock.Location = new System.Drawing.Point(224, 7);
            this.selectBlock.Name = "selectBlock";
            this.selectBlock.Size = new System.Drawing.Size(213, 50);
            this.selectBlock.TabIndex = 3;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 77);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.Size = new System.Drawing.Size(619, 491);
            this.dataGridView2.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "All BCM and Gateway Nodes:";
            // 
            // frmKvaserDBNodeMessageSend
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 593);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView2);
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
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.loadDbButton);
            this.Name = "frmKvaserDBNodeMessageSend";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmKvaserDBSend";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadDbButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusText;
        private System.Windows.Forms.Label msgIdLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox channelBox;
        private System.Windows.Forms.Button initButton;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button sendMsgButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox intervalBox;
        private System.Windows.Forms.Button startAutoButton;
        private System.Windows.Forms.Button stopTransmitButton;
        private System.Windows.Forms.Label selectBlock;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label1;
    }
}