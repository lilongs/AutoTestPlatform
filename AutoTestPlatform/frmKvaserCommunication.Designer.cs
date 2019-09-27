namespace AutoTestPlatform
{
    partial class frmKvaserCommunication
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
            this.btnInit = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnSetBit = new System.Windows.Forms.Button();
            this.btnBusOn = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnBusOff = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.combBit = new System.Windows.Forms.ComboBox();
            this.txtChannel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDlc = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dataBox0 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dataBox1 = new System.Windows.Forms.TextBox();
            this.dataBox3 = new System.Windows.Forms.TextBox();
            this.dataBox2 = new System.Windows.Forms.TextBox();
            this.dataBox7 = new System.Windows.Forms.TextBox();
            this.dataBox6 = new System.Windows.Forms.TextBox();
            this.dataBox5 = new System.Windows.Forms.TextBox();
            this.dataBox4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.outputBox = new System.Windows.Forms.RichTextBox();
            this.RTRBox = new System.Windows.Forms.CheckBox();
            this.STDBox = new System.Windows.Forms.CheckBox();
            this.EXTBox = new System.Windows.Forms.CheckBox();
            this.WakeUpBox = new System.Windows.Forms.CheckBox();
            this.NERRBox = new System.Windows.Forms.CheckBox();
            this.errorBox = new System.Windows.Forms.CheckBox();
            this.TXACKBox = new System.Windows.Forms.CheckBox();
            this.TXRQBox = new System.Windows.Forms.CheckBox();
            this.BRSBox = new System.Windows.Forms.CheckBox();
            this.EDLBox = new System.Windows.Forms.CheckBox();
            this.delayBox = new System.Windows.Forms.CheckBox();
            this.ESIBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(26, 26);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(127, 23);
            this.btnInit.TabIndex = 0;
            this.btnInit.Text = "Initialize library";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(333, 26);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(90, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "Open Channel";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSetBit
            // 
            this.btnSetBit.Location = new System.Drawing.Point(670, 25);
            this.btnSetBit.Name = "btnSetBit";
            this.btnSetBit.Size = new System.Drawing.Size(88, 23);
            this.btnSetBit.TabIndex = 0;
            this.btnSetBit.Text = "Set Bitrate";
            this.btnSetBit.UseVisualStyleBackColor = true;
            this.btnSetBit.Click += new System.EventHandler(this.btnSetBit_Click);
            // 
            // btnBusOn
            // 
            this.btnBusOn.Location = new System.Drawing.Point(26, 68);
            this.btnBusOn.Name = "btnBusOn";
            this.btnBusOn.Size = new System.Drawing.Size(75, 23);
            this.btnBusOn.TabIndex = 0;
            this.btnBusOn.Text = "Bus On";
            this.btnBusOn.UseVisualStyleBackColor = true;
            this.btnBusOn.Click += new System.EventHandler(this.btnBusOn_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(447, 176);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(97, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Send Message";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(429, 25);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(97, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close Channel";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnBusOff
            // 
            this.btnBusOff.Location = new System.Drawing.Point(116, 68);
            this.btnBusOff.Name = "btnBusOff";
            this.btnBusOff.Size = new System.Drawing.Size(75, 23);
            this.btnBusOff.TabIndex = 3;
            this.btnBusOff.Text = "Bus Off";
            this.btnBusOff.UseVisualStyleBackColor = true;
            this.btnBusOff.Click += new System.EventHandler(this.btnBusOff_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "ID:";
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(53, 115);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(75, 21);
            this.txtID.TabIndex = 5;
            // 
            // combBit
            // 
            this.combBit.FormattingEnabled = true;
            this.combBit.Items.AddRange(new object[] {
            "125 kb/s",
            "250 kb/s",
            "500 kb/s",
            "1 Mb/s"});
            this.combBit.Location = new System.Drawing.Point(543, 25);
            this.combBit.Name = "combBit";
            this.combBit.Size = new System.Drawing.Size(121, 20);
            this.combBit.TabIndex = 6;
            // 
            // txtChannel
            // 
            this.txtChannel.Location = new System.Drawing.Point(227, 26);
            this.txtChannel.Name = "txtChannel";
            this.txtChannel.Size = new System.Drawing.Size(100, 21);
            this.txtChannel.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "Channel:";
            // 
            // txtDlc
            // 
            this.txtDlc.Location = new System.Drawing.Point(193, 115);
            this.txtDlc.Name = "txtDlc";
            this.txtDlc.Size = new System.Drawing.Size(75, 21);
            this.txtDlc.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "DLC:";
            // 
            // dataBox0
            // 
            this.dataBox0.Location = new System.Drawing.Point(64, 178);
            this.dataBox0.Name = "dataBox0";
            this.dataBox0.Size = new System.Drawing.Size(39, 21);
            this.dataBox0.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Data:";
            // 
            // dataBox1
            // 
            this.dataBox1.Location = new System.Drawing.Point(108, 178);
            this.dataBox1.Name = "dataBox1";
            this.dataBox1.Size = new System.Drawing.Size(39, 21);
            this.dataBox1.TabIndex = 13;
            // 
            // dataBox3
            // 
            this.dataBox3.Location = new System.Drawing.Point(196, 178);
            this.dataBox3.Name = "dataBox3";
            this.dataBox3.Size = new System.Drawing.Size(39, 21);
            this.dataBox3.TabIndex = 15;
            // 
            // dataBox2
            // 
            this.dataBox2.Location = new System.Drawing.Point(152, 178);
            this.dataBox2.Name = "dataBox2";
            this.dataBox2.Size = new System.Drawing.Size(39, 21);
            this.dataBox2.TabIndex = 14;
            // 
            // dataBox7
            // 
            this.dataBox7.Location = new System.Drawing.Point(372, 178);
            this.dataBox7.Name = "dataBox7";
            this.dataBox7.Size = new System.Drawing.Size(39, 21);
            this.dataBox7.TabIndex = 19;
            // 
            // dataBox6
            // 
            this.dataBox6.Location = new System.Drawing.Point(328, 178);
            this.dataBox6.Name = "dataBox6";
            this.dataBox6.Size = new System.Drawing.Size(39, 21);
            this.dataBox6.TabIndex = 18;
            // 
            // dataBox5
            // 
            this.dataBox5.Location = new System.Drawing.Point(284, 178);
            this.dataBox5.Name = "dataBox5";
            this.dataBox5.Size = new System.Drawing.Size(39, 21);
            this.dataBox5.TabIndex = 17;
            // 
            // dataBox4
            // 
            this.dataBox4.Location = new System.Drawing.Point(240, 178);
            this.dataBox4.Name = "dataBox4";
            this.dataBox4.Size = new System.Drawing.Size(39, 21);
            this.dataBox4.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "Received messages:";
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(26, 231);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(732, 207);
            this.outputBox.TabIndex = 21;
            this.outputBox.Text = "";
            // 
            // RTRBox
            // 
            this.RTRBox.AutoSize = true;
            this.RTRBox.Location = new System.Drawing.Point(377, 96);
            this.RTRBox.Name = "RTRBox";
            this.RTRBox.Size = new System.Drawing.Size(60, 16);
            this.RTRBox.TabIndex = 22;
            this.RTRBox.Tag = "1";
            this.RTRBox.Text = "Remote";
            this.RTRBox.UseVisualStyleBackColor = true;
            // 
            // STDBox
            // 
            this.STDBox.AutoSize = true;
            this.STDBox.Location = new System.Drawing.Point(466, 96);
            this.STDBox.Name = "STDBox";
            this.STDBox.Size = new System.Drawing.Size(42, 16);
            this.STDBox.TabIndex = 22;
            this.STDBox.Tag = "2";
            this.STDBox.Text = "STD";
            this.STDBox.UseVisualStyleBackColor = true;
            // 
            // EXTBox
            // 
            this.EXTBox.AutoSize = true;
            this.EXTBox.Location = new System.Drawing.Point(545, 96);
            this.EXTBox.Name = "EXTBox";
            this.EXTBox.Size = new System.Drawing.Size(42, 16);
            this.EXTBox.TabIndex = 22;
            this.EXTBox.Tag = "4";
            this.EXTBox.Text = "EXT";
            this.EXTBox.UseVisualStyleBackColor = true;
            // 
            // WakeUpBox
            // 
            this.WakeUpBox.AutoSize = true;
            this.WakeUpBox.Location = new System.Drawing.Point(634, 96);
            this.WakeUpBox.Name = "WakeUpBox";
            this.WakeUpBox.Size = new System.Drawing.Size(60, 16);
            this.WakeUpBox.TabIndex = 22;
            this.WakeUpBox.Tag = "8";
            this.WakeUpBox.Text = "WakeUp";
            this.WakeUpBox.UseVisualStyleBackColor = true;
            // 
            // NERRBox
            // 
            this.NERRBox.AutoSize = true;
            this.NERRBox.Location = new System.Drawing.Point(377, 120);
            this.NERRBox.Name = "NERRBox";
            this.NERRBox.Size = new System.Drawing.Size(48, 16);
            this.NERRBox.TabIndex = 22;
            this.NERRBox.Tag = "16";
            this.NERRBox.Text = "NERR";
            this.NERRBox.UseVisualStyleBackColor = true;
            // 
            // errorBox
            // 
            this.errorBox.AutoSize = true;
            this.errorBox.Location = new System.Drawing.Point(466, 120);
            this.errorBox.Name = "errorBox";
            this.errorBox.Size = new System.Drawing.Size(54, 16);
            this.errorBox.TabIndex = 22;
            this.errorBox.Tag = "32";
            this.errorBox.Text = "Error";
            this.errorBox.UseVisualStyleBackColor = true;
            // 
            // TXACKBox
            // 
            this.TXACKBox.AutoSize = true;
            this.TXACKBox.Location = new System.Drawing.Point(545, 120);
            this.TXACKBox.Name = "TXACKBox";
            this.TXACKBox.Size = new System.Drawing.Size(54, 16);
            this.TXACKBox.TabIndex = 22;
            this.TXACKBox.Tag = "64";
            this.TXACKBox.Text = "TXACK";
            this.TXACKBox.UseVisualStyleBackColor = true;
            // 
            // TXRQBox
            // 
            this.TXRQBox.AutoSize = true;
            this.TXRQBox.Location = new System.Drawing.Point(634, 120);
            this.TXRQBox.Name = "TXRQBox";
            this.TXRQBox.Size = new System.Drawing.Size(48, 16);
            this.TXRQBox.TabIndex = 22;
            this.TXRQBox.Tag = "128";
            this.TXRQBox.Text = "TXRQ";
            this.TXRQBox.UseVisualStyleBackColor = true;
            // 
            // BRSBox
            // 
            this.BRSBox.AutoSize = true;
            this.BRSBox.Location = new System.Drawing.Point(543, 142);
            this.BRSBox.Name = "BRSBox";
            this.BRSBox.Size = new System.Drawing.Size(42, 16);
            this.BRSBox.TabIndex = 23;
            this.BRSBox.Tag = "131072";
            this.BRSBox.Text = "BRS";
            this.BRSBox.UseVisualStyleBackColor = true;
            // 
            // EDLBox
            // 
            this.EDLBox.AutoSize = true;
            this.EDLBox.Location = new System.Drawing.Point(464, 142);
            this.EDLBox.Name = "EDLBox";
            this.EDLBox.Size = new System.Drawing.Size(42, 16);
            this.EDLBox.TabIndex = 24;
            this.EDLBox.Tag = "65536";
            this.EDLBox.Text = "EDL";
            this.EDLBox.UseVisualStyleBackColor = true;
            // 
            // delayBox
            // 
            this.delayBox.AutoSize = true;
            this.delayBox.Location = new System.Drawing.Point(375, 142);
            this.delayBox.Name = "delayBox";
            this.delayBox.Size = new System.Drawing.Size(54, 16);
            this.delayBox.TabIndex = 25;
            this.delayBox.Tag = "256";
            this.delayBox.Text = "Delay";
            this.delayBox.UseVisualStyleBackColor = true;
            // 
            // ESIBox
            // 
            this.ESIBox.AutoSize = true;
            this.ESIBox.Location = new System.Drawing.Point(634, 142);
            this.ESIBox.Name = "ESIBox";
            this.ESIBox.Size = new System.Drawing.Size(42, 16);
            this.ESIBox.TabIndex = 26;
            this.ESIBox.Tag = "262144";
            this.ESIBox.Text = "ESI";
            this.ESIBox.UseVisualStyleBackColor = true;
            // 
            // frmKvaserCommunication
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 450);
            this.Controls.Add(this.ESIBox);
            this.Controls.Add(this.BRSBox);
            this.Controls.Add(this.EDLBox);
            this.Controls.Add(this.delayBox);
            this.Controls.Add(this.TXRQBox);
            this.Controls.Add(this.TXACKBox);
            this.Controls.Add(this.WakeUpBox);
            this.Controls.Add(this.errorBox);
            this.Controls.Add(this.EXTBox);
            this.Controls.Add(this.NERRBox);
            this.Controls.Add(this.STDBox);
            this.Controls.Add(this.RTRBox);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dataBox7);
            this.Controls.Add(this.dataBox6);
            this.Controls.Add(this.dataBox5);
            this.Controls.Add(this.dataBox4);
            this.Controls.Add(this.dataBox3);
            this.Controls.Add(this.dataBox2);
            this.Controls.Add(this.dataBox1);
            this.Controls.Add(this.dataBox0);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtDlc);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtChannel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.combBit);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnBusOff);
            this.Controls.Add(this.btnBusOn);
            this.Controls.Add(this.btnSetBit);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnInit);
            this.Name = "frmKvaserCommunication";
            this.Text = "frmKvaserCommunication";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnSetBit;
        private System.Windows.Forms.Button btnBusOn;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnBusOff;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.ComboBox combBit;
        private System.Windows.Forms.TextBox txtChannel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDlc;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox dataBox0;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox dataBox1;
        private System.Windows.Forms.TextBox dataBox3;
        private System.Windows.Forms.TextBox dataBox2;
        private System.Windows.Forms.TextBox dataBox7;
        private System.Windows.Forms.TextBox dataBox6;
        private System.Windows.Forms.TextBox dataBox5;
        private System.Windows.Forms.TextBox dataBox4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RichTextBox outputBox;
        private System.Windows.Forms.CheckBox RTRBox;
        private System.Windows.Forms.CheckBox STDBox;
        private System.Windows.Forms.CheckBox EXTBox;
        private System.Windows.Forms.CheckBox WakeUpBox;
        private System.Windows.Forms.CheckBox NERRBox;
        private System.Windows.Forms.CheckBox errorBox;
        private System.Windows.Forms.CheckBox TXACKBox;
        private System.Windows.Forms.CheckBox TXRQBox;
        private System.Windows.Forms.CheckBox BRSBox;
        private System.Windows.Forms.CheckBox EDLBox;
        private System.Windows.Forms.CheckBox delayBox;
        private System.Windows.Forms.CheckBox ESIBox;
    }
}