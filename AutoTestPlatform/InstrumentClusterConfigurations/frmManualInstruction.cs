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

namespace AutoTestPlatform.InstrumentClusterConfigurations
{
    public partial class frmManualInstruction : Form
    {
        public frmManualInstruction()
        {
            InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<ManualInstruction> list = new List<ManualInstruction>();

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
            string json = JsonOperate.GetJson(path, "ManualInstrustion.json");
            List<ManualInstruction> temp = JsonConvert.DeserializeObject<List<ManualInstruction>>(json);
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
                if (String.IsNullOrEmpty(txttype.Text.Trim()))
                {
                    MessageBox.Show("type can't be empty!");
                    return;
                }
                if (String.IsNullOrEmpty(txtid.Text.Trim()))
                {
                    MessageBox.Show("id can't be empty!");
                    return;
                }
                if (String.IsNullOrEmpty(txtdata.Text.Trim()))
                {
                    MessageBox.Show("data can't be empty!");
                    return;
                }
                #endregion
                string path = Application.StartupPath + "\\SysConfig";
                var item= list.Where(c => c.id == txtid.Text).FirstOrDefault();
                if (item != null)
                {
                    item.type = txttype.Text.Trim();
                    item.dlc = Convert.ToInt32(txtdlc.Text.Trim());
                    item.data = txtdata.Text.Trim();
                    item.cycletime = Convert.ToInt32(txtct.Text.Trim());
                }
                else
                {
                    ManualInstruction Ins = new ManualInstruction();
                    Ins.type = txttype.Text.Trim();
                    Ins.id = txtid.Text.Trim();
                    Ins.dlc = Convert.ToInt32(txtdlc.Text.Trim());
                    Ins.data = txtdata.Text.Trim();
                    Ins.cycletime = Convert.ToInt32(txtct.Text.Trim());
                    list.Add(Ins);
                }               

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "ManualInstrustion.json", json);
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
                string id = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["id"].Value.ToString();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].id.ToString() == id)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\SysConfig";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "ManualInstrustion.json", json);
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
                    this.txttype.Text = row.Cells["type"].Value == null ? "" : row.Cells["type"].Value.ToString();
                    this.txtid.Text = row.Cells["id"].Value == null?"": row.Cells["id"].Value.ToString();
                    this.txtdlc.Text = row.Cells["dlc"].Value == null ? "" : row.Cells["dlc"].Value.ToString();
                    this.txtdata.Text = row.Cells["data"].Value == null ? "" : row.Cells["data"].Value.ToString();
                    this.txtct.Text = row.Cells["cycletime"].Value == null ? "" : row.Cells["cycletime"].Value.ToString();
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
