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

namespace AutoTestPlatform.PowerMeterConfiguration
{
    public partial class frmPowerMeterConfiguration : Form
    {
        public frmPowerMeterConfiguration()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<PowerMeter> list = new List<PowerMeter>();

        private void frmTestSequncenManager_Load(object sender, EventArgs e)
        {
            try
            {
                Fillinstrumentcluster();
                LoadInfo();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void Fillinstrumentcluster()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "InstrumentClusterConfiguration.json");
            List<InstrumentClusterConfiguration> temp = JsonConvert.DeserializeObject<List<InstrumentClusterConfiguration>>(json);
            if (temp != null)
            {
                foreach (InstrumentClusterConfiguration equipment in temp)
                {

                    if (!txtic.Items.Contains(equipment.InstrumentCluster))
                        this.txtic.Items.Add(equipment.InstrumentCluster);
                }
            }
        }

        private void LoadInfo()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "PowerMeter.json");
            List<PowerMeter> temp = JsonConvert.DeserializeObject<List<PowerMeter>>(json);
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
                if (String.IsNullOrEmpty(txtic.Text.Trim()))
                {
                    MessageBox.Show("instrumentcluster can't be empty!");
                    return;
                }
                if (String.IsNullOrEmpty(txtaddr.Text.Trim()))
                {
                    MessageBox.Show("address can't be empty!");
                    return;
                }
                #endregion
                string path = Application.StartupPath + "\\SysConfig";
                var item= list.Where(c => c.instrumentcluster == txtic.Text).FirstOrDefault();
                if (item != null)
                {
                    item.address = txtaddr.Text.Trim();
                }
                else
                {
                    PowerMeter Ins = new PowerMeter();
                    Ins.instrumentcluster = txtic.Text.Trim();
                    Ins.address = txtaddr.Text.Trim();
                    list.Add(Ins);
                }               

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "PowerMeter.json", json);
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
                string instrumentcluster = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["instrumentcluster"].Value.ToString();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].instrumentcluster.ToString() == instrumentcluster)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\SysConfig";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "PowerMeter.json", json);
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
                    this.txtic.Text = row.Cells["instrumentcluster"].Value == null ? "" : row.Cells["instrumentcluster"].Value.ToString();
                    this.txtaddr.Text = row.Cells["address"].Value == null?"": row.Cells["address"].Value.ToString();
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
