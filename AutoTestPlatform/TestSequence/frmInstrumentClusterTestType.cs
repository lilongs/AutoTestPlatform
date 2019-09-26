using AutoTestDLL.Model;
using AutoTestDLL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTestPlatform.TestSequence
{
    public partial class frmInstrumentClusterTestType : Form
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public frmInstrumentClusterTestType()
        {
            InitializeComponent();
        }
        List<EquipmentTestInfo> list = new List<EquipmentTestInfo>();
        private void frmTestTypeManager_Load(object sender, EventArgs e)
        {
            try
            {
                LoadEquipmentTestInfo();
                LoadTypeInfo();
                LoadEquipmentInfo();
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }        

        private void LoadEquipmentTestInfo()
        {
            string path = Application.StartupPath + "\\TestInfo";
            string json = JsonOperate.GetJson(path, "InstrumentClusterTestInfo.json");
            List<EquipmentTestInfo> temp = JsonConvert.DeserializeObject<List<EquipmentTestInfo>>(json);
            if (temp != null)
            {
                list = temp;
                this.dataGridView1.DataSource = list;
            }
        }

        private void LoadTypeInfo()
        {
            string path = Application.StartupPath + "\\TestInfo";
            string json = JsonOperate.GetJson(path, "TypeList.json");
            List<TypeList> temp = JsonConvert.DeserializeObject<List<TypeList>>(json);
            if (temp != null)
            {
                this.combtypename.DataSource=temp.Where(t => t.parentname == "").ToList();
                this.combtypename.DisplayMember = "typename";
            }
        }

        private void LoadEquipmentInfo()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "InstrumentClusterConfiguration.json");
            List<InstrumentClusterConfiguration> temp = JsonConvert.DeserializeObject<List<InstrumentClusterConfiguration>>(json);
            if (temp != null)
            {
                foreach(InstrumentClusterConfiguration equipment in temp)
                {
                    
                    if(!combEquipment.Items.Contains(equipment.InstrumentCluster))
                        this.combEquipment.Items.Add(equipment.InstrumentCluster);
                }
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + "\\TestInfo";
                var item = list.Where(x => x.InstrumentCluster == combEquipment.Text && x.TypeName == combtypename.Text).FirstOrDefault();
                if (item!=null)
                {
                    item.InstrumentCluster = combEquipment.Text;
                    item.TypeName = combtypename.Text;
                }
                else
                {
                    EquipmentTestInfo testInfo = new EquipmentTestInfo();
                    testInfo.InstrumentCluster = combEquipment.Text;
                    testInfo.TypeName = combtypename.Text;
                    list.Add(testInfo);
                }              

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "InstrumentClusterTestInfo.json", json);
                LoadEquipmentTestInfo();
            }
            catch(Exception ex)
            {
                logger.Error(ex,ex.Message);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            try
            {
                string equipment = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["InstrumentCluster"].Value.ToString();
                string typename = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["TypeName"].Value.ToString();
                for (int i=0;i< list.Count;i++)
                {
                    if (list[i].TypeName == typename && list[i].InstrumentCluster==equipment)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\TestInfo";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "InstrumentClusterTestInfo.json", json);
                LoadEquipmentTestInfo();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void quit_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = this.dataGridView1.CurrentRow;
                this.combEquipment.Text = row.Cells["InstrumentCluster"].Value.ToString();
                this.combtypename.Text = row.Cells["TypeName"].Value.ToString();
            }
        }
    }
}
