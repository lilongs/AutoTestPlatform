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
    public partial class frmCOMConfiguration : Form
    {
        public frmCOMConfiguration()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<COM> list = new List<COM>();

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
            string json = JsonOperate.GetJson(path, "COM.json");
            List<COM> temp = JsonConvert.DeserializeObject<List<COM>>(json);
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
                if (String.IsNullOrEmpty(txtPortName.Text))
                {
                    MessageBox.Show("PortName can't be empty!");
                    return;
                }
                #endregion
                string path = Application.StartupPath + "\\SysConfig";
                var item= list.Where(c => c.portName == txtPortName.Text).FirstOrDefault();
                if (item != null)
                {
                    item.baudrate = String.IsNullOrEmpty(txtBaudRate.Text.Trim())?0:Convert.ToInt32(txtBaudRate.Text.Trim());
                    item.parity = (Parity)Enum.Parse(typeof(Parity), label11.Text);
                    item.dataBits= String.IsNullOrEmpty(txtDataBits.Text.Trim()) ? 0 : Convert.ToInt32(txtDataBits.Text.Trim());
                    item.stopBits= (StopBits)Enum.Parse(typeof(StopBits), txtStopBits.Text);
                    item.handshake= (Handshake)Enum.Parse(typeof(Handshake), txtHandshake.Text);
                }
                else
                {
                    COM com = new COM();
                    com.portName = txtPortName.Text.Trim();
                    com.baudrate = String.IsNullOrEmpty(txtBaudRate.Text.Trim()) ? 0 : Convert.ToInt32(txtBaudRate.Text.Trim());
                    com.parity = (Parity)Enum.Parse(typeof(Parity), txtParity.Text, true);
                    com.dataBits = String.IsNullOrEmpty(txtDataBits.Text.Trim()) ? 0 : Convert.ToInt32(txtDataBits.Text.Trim());
                    com.stopBits = (StopBits)Enum.Parse(typeof(StopBits), txtStopBits.Text,true);
                    com.handshake = (Handshake)Enum.Parse(typeof(Handshake), txtHandshake.Text, true);
                    list.Add(com);
                }               

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "COM.json", json);
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
                string portName = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["portName"].Value.ToString();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].portName == portName)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\SysConfig";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "COM.json", json);
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
                    this.txtPortName.Text= row.Cells["portName"].Value == null ? "" : row.Cells["portName"].Value.ToString();
                    this.txtBaudRate.Text = row.Cells["baudrate"].Value == null ? "" : row.Cells["baudrate"].Value.ToString();
                    this.txtParity.Text= row.Cells["parity"].Value == null ? "" : row.Cells["parity"].Value.ToString();
                    this.txtDataBits.Text =row.Cells["dataBits"].Value == null ? "" : row.Cells["dataBits"].Value.ToString();
                    this.txtStopBits.Text =  row.Cells["stopBits"].Value == null ? "" : row.Cells["stopBits"].Value.ToString();
                    this.txtHandshake.Text =  row.Cells["handshake"].Value == null ? "" : row.Cells["handshake"].Value.ToString();
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
