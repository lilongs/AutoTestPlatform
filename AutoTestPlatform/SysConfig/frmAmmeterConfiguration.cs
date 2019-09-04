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
    public partial class frmAmmeterConfiguration : Form
    {
        public frmAmmeterConfiguration()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<AmmeterConfiguration> list = new List<AmmeterConfiguration>();

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
            string json = JsonOperate.GetJson(path, "AmmeterConfiguration.json");
            List<AmmeterConfiguration> temp = JsonConvert.DeserializeObject<List<AmmeterConfiguration>>(json);
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
                if (String.IsNullOrEmpty(txtPortName.Text.Trim()))
                {
                    MessageBox.Show("AmmeterName can't be empty!");
                    return;
                }
                if (String.IsNullOrEmpty(txtPortName.Text))
                {
                    MessageBox.Show("PortName can't be empty!");
                    return;
                }
                #endregion
                string path = Application.StartupPath + "\\SysConfig";
                var item= list.Where(c => c.ammeterName == txtAmmeterName.Text.Trim() && c.portName == txtPortName.Text).FirstOrDefault();
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
                    AmmeterConfiguration ammeter = new AmmeterConfiguration();
                    ammeter.ammeterName = txtAmmeterName.Text.Trim();
                    ammeter.portName = txtPortName.Text.Trim();
                    ammeter.baudrate = String.IsNullOrEmpty(txtBaudRate.Text.Trim()) ? 0 : Convert.ToInt32(txtBaudRate.Text.Trim());
                    ammeter.parity = (Parity)Enum.Parse(typeof(Parity), txtParity.Text, true);
                    ammeter.dataBits = String.IsNullOrEmpty(txtDataBits.Text.Trim()) ? 0 : Convert.ToInt32(txtDataBits.Text.Trim());
                    ammeter.stopBits = (StopBits)Enum.Parse(typeof(StopBits), txtStopBits.Text,true);
                    ammeter.handshake = (Handshake)Enum.Parse(typeof(Handshake), txtHandshake.Text, true);
                    list.Add(ammeter);
                }               

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "AmmeterConfiguration.json", json);
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
                string ammeterName = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["ammeterName"].Value.ToString();
                string portName = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["portName"].Value.ToString();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ammeterName == ammeterName && list[i].portName == portName)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\SysConfig";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "AmmeterConfiguration.json", json);
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
                    this.txtAmmeterName.Text = row.Cells["ammeterName"].Value is null?"": row.Cells["ammeterName"].Value.ToString();
                    this.txtPortName.Text= row.Cells["portName"].Value is null ? "" : row.Cells["portName"].Value.ToString();
                    this.txtBaudRate.Text = row.Cells["baudrate"].Value is null ? "" : row.Cells["baudrate"].Value.ToString();
                    this.txtParity.Text= row.Cells["parity"].Value is null ? "" : row.Cells["parity"].Value.ToString();
                    this.txtDataBits.Text =row.Cells["dataBits"].Value is null ? "" : row.Cells["dataBits"].Value.ToString();
                    this.txtStopBits.Text =  row.Cells["stopBits"].Value is null ? "" : row.Cells["stopBits"].Value.ToString();
                    this.txtHandshake.Text =  row.Cells["handshake"].Value is null ? "" : row.Cells["handshake"].Value.ToString();
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
