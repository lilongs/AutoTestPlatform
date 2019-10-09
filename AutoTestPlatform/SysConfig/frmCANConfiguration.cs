using AutoTestDLL.Model;
using AutoTestDLL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTestPlatform.SysConfig
{
    public partial class frmCANConfiguration : Form
    {
        public frmCANConfiguration()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<CAN> list = new List<CAN>();

        private void frmTestSequncenManager_Load(object sender, EventArgs e)
        {
            try
            {
                LoadInfo();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void LoadInfo()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "CAN.json");
            List<CAN> temp = JsonConvert.DeserializeObject<List<CAN>>(json);
            if (temp != null)
            {
                list = temp;
                this.dataGridView1.DataSource = list;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                #region CheckInput
                if (String.IsNullOrEmpty(combChannel.Text.Trim()))
                {
                    MessageBox.Show("Channel can't be empty!");
                    return;
                }
                if (String.IsNullOrEmpty(combBaudRate.Text))
                {
                    MessageBox.Show("PortName can't be empty!");
                    return;
                }
                #endregion
                string path = Application.StartupPath + "\\SysConfig";
                var item= list.Where(c => c.Channel == combChannel.Text).FirstOrDefault();
                if (item != null)
                {
                    item.Baudrate = combBaudRate.Text.Trim();
                }
                else
                {
                    CAN can = new CAN();
                    can.Channel = String.IsNullOrEmpty(combChannel.Text.Trim()) ? "" : combChannel.Text.Trim();
                    can.Baudrate =combBaudRate.Text.Trim();
                    list.Add(can);
                }               

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "CAN.json", json);
                LoadInfo();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            try
            {
                string channel = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["channel"].Value.ToString();
                string baudrate = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["baudrate"].Value.ToString();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Channel.ToString() == channel && list[i].Baudrate.ToString() == baudrate)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\SysConfig";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "CAN.json", json);
                LoadInfo();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void quit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = this.dataGridView1.CurrentRow;
                    this.combChannel.Text = row.Cells["channel"].Value == null?"": row.Cells["channel"].Value.ToString();
                    this.combBaudRate.Text = row.Cells["baudrate"].Value == null ? "" : row.Cells["baudrate"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void frmAmmeterConfiguration_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
