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
    public partial class frmInstrumentClusterConfiguration : Form
    {
        public frmInstrumentClusterConfiguration()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<InstrumentClusterConfiguration> list = new List<InstrumentClusterConfiguration>();

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
            string json = JsonOperate.GetJson(path, "InstrumentClusterConfiguration.json");
            List<InstrumentClusterConfiguration> temp = JsonConvert.DeserializeObject<List<InstrumentClusterConfiguration>>(json);
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
                if (String.IsNullOrEmpty(txtEquipment.Text.Trim()))
                {
                    MessageBox.Show("Equipment can't be empty!");
                    return;
                }
                if (String.IsNullOrEmpty(combParamter.Text))
                {
                    MessageBox.Show("Paramter can't be empty!");
                    return;
                }
                #endregion
                string path = Application.StartupPath + "\\SysConfig";
                var item= list.Where(c => c.InstrumentCluster == txtEquipment.Text.Trim() && c.CommunicationType == combParamter.Text).FirstOrDefault();
                if (item != null)
                {
                    item.CommunicationType= combParamter.Text;
                    item.Value= combValue.Text;
                }
                else
                {
                    InstrumentClusterConfiguration equipment = new InstrumentClusterConfiguration();
                    equipment.InstrumentCluster = txtEquipment.Text.Trim();
                    equipment.CommunicationType = combParamter.Text;
                    equipment.Value = combValue.Text;
                    list.Add(equipment);
                }               

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "InstrumentClusterConfiguration.json", json);
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
                string equipment = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["InstrumentCluster"].Value.ToString();
                string paramter = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["CommunicationType"].Value.ToString();
                string value= this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Value"].Value.ToString();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].InstrumentCluster == equipment && list[i].CommunicationType == paramter && list[i].Value == value)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\SysConfig";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "InstrumentClusterConfiguration.json", json);
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
                    this.txtEquipment.Text = row.Cells["InstrumentCluster"].Value == null?"": row.Cells["InstrumentCluster"].Value.ToString();
                    this.combParamter.Text= row.Cells["CommunicationType"].Value == null ? "" : row.Cells["CommunicationType"].Value.ToString();
                    this.combValue.Text = row.Cells["Value"].Value == null ? "" : row.Cells["Value"].Value.ToString();
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

        private void combParamter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = combParamter.Text;
            LoadValue(type);
        }

        private void LoadValue(string type)
        {
            this.combValue.DataSource = null;
            switch (type)
            {
                case "CAN":                  
                    string path = Application.StartupPath + "\\SysConfig";
                    string json = JsonOperate.GetJson(path, "CAN.json");
                    List<CAN> temp = JsonConvert.DeserializeObject<List<CAN>>(json);
                    this.combValue.DataSource = temp;
                    this.combValue.DisplayMember = "channel";
                    break;
                case "COM":
                    string path2 = Application.StartupPath + "\\SysConfig";
                    string json2 = JsonOperate.GetJson(path2, "COM.json");
                    List<COM> temp2 = JsonConvert.DeserializeObject<List<COM>>(json2);
                    this.combValue.DataSource = temp2;
                    this.combValue.DisplayMember = "portName";
                    break;
                default:
                    break;
            }
        }
    }
}
