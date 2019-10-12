namespace WindowsFormsControlLibrary
{
    partial class TemperatureControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TemperatureControl));
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.userCurve1 = new WindowsFormsControlLibrary.UserCurve();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.AllowBorderColorBlending = true;
            this.groupControl1.AppearanceCaption.BorderColor = System.Drawing.Color.Coral;
            this.groupControl1.AppearanceCaption.Options.UseBorderColor = true;
            this.groupControl1.CaptionImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("groupControl1.CaptionImageOptions.Image")));
            this.groupControl1.Controls.Add(this.userCurve1);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(513, 302);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "temperature curve";
            // 
            // userCurve1
            // 
            this.userCurve1.BackColor = System.Drawing.Color.Transparent;
            this.userCurve1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userCurve1.Location = new System.Drawing.Point(2, 39);
            this.userCurve1.Name = "userCurve1";
            this.userCurve1.Size = new System.Drawing.Size(509, 261);
            this.userCurve1.TabIndex = 2;
            this.userCurve1.ValueMaxRight = 50F;
            this.userCurve1.ValueMinLeft = -10F;
            // 
            // TemperatureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.groupControl1);
            this.Name = "TemperatureControl";
            this.Size = new System.Drawing.Size(513, 302);
            this.Load += new System.EventHandler(this.TemperatureControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private UserCurve userCurve1;
    }
}
