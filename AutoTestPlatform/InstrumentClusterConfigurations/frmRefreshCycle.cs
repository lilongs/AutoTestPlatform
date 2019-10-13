using AutoTestDLL.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTestPlatform.InstrumentClusterConfigurations
{
    public partial class frmRefreshCycle : Form
    {
        public frmRefreshCycle()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public System.Timers.Timer changeSelectedPageTimer = new System.Timers.Timer();

        private void frmRefreshCycle_Load(object sender, EventArgs e)
        {
            try
            {
                string info = LoadRefreshCycle();
                string[] arr = info.Split(',');
                if (arr.Length > 0)
                {
                    this.txtOld.Text = arr[0];
                    this.checkBox1.Checked = Convert.ToBoolean(arr[1]);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
            
        }

        public static string LoadRefreshCycle()
        {
            string path = Application.StartupPath + "\\SysConfig\\RefreshCycle.txt";
            string info = FileOperate.ReadFile(path);
            return info;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                #region Checking input
                if (!String.IsNullOrEmpty(txtNew.Text.Trim()))
                {
                    int temp = -1;
                    if (!int.TryParse(txtNew.Text.Trim(), out temp))
                    {
                        MessageBox.Show("Please enter the correct value!");
                        return;
                    }
                }                
                if (txtNew.Text.Trim() == txtOld.Text)
                {
                    MessageBox.Show("The new refresh cycle is same as old refresh cycle!");
                    return;
                }
                #endregion
                string path = Application.StartupPath + "\\SysConfig";
                string newCycle = string.Empty;
                if (string.IsNullOrEmpty(txtNew.Text.Trim()))
                {
                    newCycle = txtOld.Text.Trim() + "," + Convert.ToString(checkBox1.Checked);
                }
                else
                {
                    newCycle = txtNew.Text.Trim() + "," + Convert.ToString(checkBox1.Checked);
                }
                FileOperate.CoverOldFile(path, "RefreshCycle.txt", newCycle);
                MessageBox.Show("Success!");

                int result = 10000;
                int.TryParse(newCycle.Split(',')[0], out result);
                changeSelectedPageTimer.Interval = result;
                if (checkBox1.Checked)
                    changeSelectedPageTimer.Start();
                else
                    changeSelectedPageTimer.Stop();
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }
    }
}
