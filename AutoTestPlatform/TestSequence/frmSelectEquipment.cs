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
    public partial class frmSelectEquipment : Form
    {
        public frmSelectEquipment()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<InstrumentClusterConfiguration> list = new List<InstrumentClusterConfiguration>();
        public string select_equipment = string.Empty;
        private void frmSelectEquipment_Load(object sender, EventArgs e)
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
                foreach(InstrumentClusterConfiguration equipment in temp)
                {
                    if(!this.listBox1.Items.Contains(equipment.InstrumentCluster))
                    this.listBox1.Items.Add(equipment.InstrumentCluster);
                }
                
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            select_equipment = this.listBox1.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
        }
    }
}
