using AutoTestDLL.Model;
using AutoTestDLL.Util;
using AutoTestPlatform.SysConfig;
using AutoTestPlatform.TestSequence;
using DevExpress.XtraTab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsControlLibrary;

namespace AutoTestPlatform
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private System.Timers.Timer chartTimer = new System.Timers.Timer();
        private List<AmmeterConfiguration> ammeterList = new List<AmmeterConfiguration>();
        Dictionary<string, TestUnit> DicEquipmentInfo = new Dictionary<string, TestUnit>();
        Dictionary<string, TemperatureControl> DicTemperatureInfo = new Dictionary<string, TemperatureControl>();
        Dictionary<string, PublicElectricControl> DicPublicElectricInfo = new Dictionary<string, PublicElectricControl>();
        List<Color> colors = new List<Color>() {
            Color.FromArgb(230,23,23),Color.FromArgb(246,98,98), Color.FromArgb(154, 61,61),//RED
            Color.FromArgb(115,114,57),Color.FromArgb(150,214,21),Color.FromArgb(193,241,96),//GREEN
            Color.FromArgb(37,92,92),Color.FromArgb(14,138,138),Color.FromArgb(85,215,215),//BLUE
            Color.FromArgb(234,129,41),Color.FromArgb(255,130,26),Color.FromArgb(255,177,113),//YELLOW
        };

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                chartTimer.Interval = 1000;
                chartTimer.Elapsed += chartTimer_Tick;
                chartTimer.Start();

                LoadEquipmentInfo();
                LoadTestUnitControl();

                LoadPublicElectricInfo();
                LoadPublicElectricControl();
                LoadTemperatureInfo();
                LoadTemperatureControl();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void LoadEquipmentInfo()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "InstrumentClusterConfiguration.json");
            List<InstrumentClusterConfiguration> temp = JsonConvert.DeserializeObject<List<InstrumentClusterConfiguration>>(json);
            if (temp != null)
            {
                foreach (InstrumentClusterConfiguration equipment in temp)
                {
                    string eq = equipment.InstrumentCluster;
                    TestUnit testUnit = new TestUnit();
                    testUnit.Name = eq;
                    testUnit.Tag = eq;
                    if (!DicEquipmentInfo.ContainsKey(eq))
                    {
                        DicEquipmentInfo.Add(eq, testUnit);
                    }
                }
            }
        }

        private void LoadTemperatureInfo()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "TempSensorConfiguration.json");
            List<TempSensorConfiguration> temp = JsonConvert.DeserializeObject<List<TempSensorConfiguration>>(json);
            if (temp != null)
            {
                foreach (TempSensorConfiguration t in temp)
                {
                    string sensorName = t.SensorName;
                    TemperatureControl temperature = new TemperatureControl();
                    temperature.Name = sensorName;
                    temperature.Tag = sensorName;
                    if (!DicTemperatureInfo.ContainsKey(sensorName))
                    {
                        DicTemperatureInfo.Add(sensorName, temperature);
                    }
                }
            }
        }

        private void LoadPublicElectricInfo()
        {
            PublicElectricControl electricControl = new PublicElectricControl();
            electricControl.Name = "electricControl";

            if (!DicPublicElectricInfo.ContainsKey(electricControl.Name))
            {
                DicPublicElectricInfo.Add(electricControl.Name, electricControl);
            }
        }

        private void LoadTestUnitControl()
        {
            
            foreach (var item in DicEquipmentInfo)
            {
                XtraTabPage xpage = new XtraTabPage();
                xpage.Name = item.Key;
                xpage.Text = item.Key+" test unit";
                xpage.Controls.Add(item.Value);
                item.Value.Dock = DockStyle.Fill;
                xtraTabControl1.TabPages.Add(xpage);
                
            }
            if(xtraTabControl1.TabPages.Count>0)
                xtraTabControl1.SelectedTabPageIndex = 0;
        }

        private void LoadTemperatureControl()
        {
            int i = 0;
            int x = this.panel2.Width;
            int y = this.panel2.Height;

            int _x = x - 22;
            int _y = (y - 20) / 3;

            foreach (var item in DicTemperatureInfo)
            {
                item.Value.Width = _x;
                item.Value.Height = _y;
                item.Value.Location = new System.Drawing.Point((i % 1) * (_x + 10) + 3, (i / 1) * (_y + 20) + 3);
                panel4.Controls.Add(item.Value);
                i++;
            }
        }

        private void LoadPublicElectricControl()
        {
            int i = 0;
            int x = this.panel2.Width;
            int y = this.panel2.Height;

            int _x = x ;
            int _y = (y - 20) / 3;

            foreach (var item in DicPublicElectricInfo)
            {
                string name = item.Key;

                item.Value.Width = _x;
                item.Value.Height = _y;
                item.Value.Location = new System.Drawing.Point((i % 1) * (_x + 10) + 3, (i / 1) * (_y + 20) + 3);
                panel3.Controls.Add(item.Value);
                i++;
            }
        }


        private void chartTimer_Tick(object sender, EventArgs e)
        {
            foreach (var it in DicTemperatureInfo)
            {
                Temperature_humidity temperature = new Temperature_humidity();
                temperature.now = DateTime.Now;
                temperature.temperatureValue = new Random().Next(1, 100);
                temperature.humidtyValue=new Random().Next(0, 50);

                it.Value.ChartValueFill(temperature);
            }

            foreach (var item in DicEquipmentInfo)
            {
                item.Value.ChartValueFill(new Random().Next(1, 100));
            }

            foreach(var its in DicPublicElectricInfo)
            {
                CurrentElectricValue value = new CurrentElectricValue();
                value.now = DateTime.Now;
                value.currentValue = new Random().Next(1, 1000);
                value.voltageValue = new Random().Next(0, 13000);

                its.Value.ChartValueFill(value);
            }
        }

        private void TestTypeEdit_Click(object sender, EventArgs e)
        {
            frmTestTypeManager form = new frmTestTypeManager();
            form.ShowDialog();
        }

        private void TestSequenceEdit_Click(object sender, EventArgs e)
        {
            frmTestSequncenManager form = new frmTestSequncenManager();
            form.ShowDialog();
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure exit system？", "notice", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void temperatureConfiguration_Click(object sender, EventArgs e)
        {
            frmTemperatureConfiguration frm = new frmTemperatureConfiguration();
            frm.ShowDialog();
        }

        private void cANConfiguration_Click(object sender, EventArgs e)
        {
            frmCANConfiguration frm = new frmCANConfiguration();
            frm.ShowDialog();
        }

        private void cOMConfiguration_Click(object sender, EventArgs e)
        {
            frmCOMConfiguration frm = new frmCOMConfiguration();
            frm.ShowDialog();
        }

        private void equipmentConfiguration_Click(object sender, EventArgs e)
        {
            frmInstrumentClusterConfiguration frm = new frmInstrumentClusterConfiguration();
            frm.ShowDialog();
        }

        private void equipmentTestInfo_Click(object sender, EventArgs e)
        {
            frmInstrumentClusterTestType frm = new frmInstrumentClusterTestType();
            frm.ShowDialog();
        }

        private void maualToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManualInstruction frm = new frmManualInstruction();
            frm.ShowDialog();
        }
    }
}
