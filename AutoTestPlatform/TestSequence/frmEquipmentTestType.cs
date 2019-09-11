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
    public partial class frmEquipmentTestType : Form
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public frmEquipmentTestType()
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
            string json = JsonOperate.GetJson(path, "EquipmentTestInfo.json");
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
            string json = JsonOperate.GetJson(path, "EquipmentConfiguration.json");
            List<EquipmentConfiguration> temp = JsonConvert.DeserializeObject<List<EquipmentConfiguration>>(json);
            if (temp != null)
            {
                foreach(EquipmentConfiguration equipment in temp)
                {
                    
                    if(!combEquipment.Items.Contains(equipment.equipment))
                        this.combEquipment.Items.Add(equipment.equipment);
                }
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + "\\TestInfo";
                var item = list.Where(x => x.equipment == combEquipment.Text && x.typename == combtypename.Text).FirstOrDefault();
                if (item!=null)
                {
                    item.equipment = combEquipment.Text;
                    item.typename = combtypename.Text;
                }
                else
                {
                    EquipmentTestInfo testInfo = new EquipmentTestInfo();
                    testInfo.equipment = combEquipment.Text;
                    testInfo.typename = combtypename.Text;
                    list.Add(testInfo);
                }              

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "EquipmentTestInfo.json", json);
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
                string equipment = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["equipment"].Value.ToString();
                string typename = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["typename"].Value.ToString();
                for (int i=0;i< list.Count;i++)
                {
                    if (list[i].typename == typename && list[i].equipment==equipment)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\TestInfo";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "EquipmentTestInfo.json", json);
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
                this.combEquipment.Text = row.Cells["equipment"].Value.ToString();
                this.combtypename.Text = row.Cells["typename"].Value.ToString();
            }
        }
    }
}
