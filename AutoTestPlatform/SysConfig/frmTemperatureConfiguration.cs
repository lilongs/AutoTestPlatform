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
    public partial class frmTemperatureConfiguration : Form
    {
        public frmTemperatureConfiguration()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<TempSensorConfiguration> list = new List<TempSensorConfiguration>();

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
            string json = JsonOperate.GetJson(path, "TempSensorConfiguration.json");
            List<TempSensorConfiguration> temp = JsonConvert.DeserializeObject<List<TempSensorConfiguration>>(json);
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
                if (String.IsNullOrEmpty(txtTempSensorName.Text.Trim()))
                {
                    MessageBox.Show("SensorName can't be empty!");
                    return;
                }
                if (String.IsNullOrEmpty(txtPortName.Text))
                {
                    MessageBox.Show("PortName can't be empty!");
                    return;
                }
                #endregion
                string path = Application.StartupPath + "\\SysConfig";
                var item= list.Where(c => c.sensorName == txtTempSensorName.Text.Trim() && c.portName == txtPortName.Text).FirstOrDefault();
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
                    TempSensorConfiguration temp = new TempSensorConfiguration();
                    temp.sensorName = txtTempSensorName.Text.Trim();
                    temp.portName = txtPortName.Text.Trim();
                    temp.baudrate = String.IsNullOrEmpty(txtBaudRate.Text.Trim()) ? 0 : Convert.ToInt32(txtBaudRate.Text.Trim());
                    temp.parity = (Parity)Enum.Parse(typeof(Parity), txtParity.Text, true);
                    temp.dataBits = String.IsNullOrEmpty(txtDataBits.Text.Trim()) ? 0 : Convert.ToInt32(txtDataBits.Text.Trim());
                    temp.stopBits = (StopBits)Enum.Parse(typeof(StopBits), txtStopBits.Text,true);
                    temp.handshake = (Handshake)Enum.Parse(typeof(Handshake), txtHandshake.Text, true);
                    list.Add(temp);
                }               

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "TempSensorConfiguration.json", json);
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
                string sensorName = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["sensorName"].Value.ToString();
                string portName = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["portName"].Value.ToString();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].sensorName == sensorName && list[i].portName == portName)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\SysConfig";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "TempSensorConfiguration.json", json);
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
                    this.txtTempSensorName.Text = row.Cells["sensorName"].Value == null?"": row.Cells["sensorName"].Value.ToString();
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

        private void frmTempSensorConfiguration_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
