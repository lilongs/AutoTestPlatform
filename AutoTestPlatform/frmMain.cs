using AutoTestDLL.Model;
using AutoTestDLL.Util;
using AutoTestPlatform.HistoricalReview;
using AutoTestPlatform.InstrumentClusterConfigurations;
using AutoTestPlatform.PowerMeterConfiguration;
using AutoTestPlatform.TemperatureSensorConfiguration;
using AutoTestPlatform.TestSequence;
using DevExpress.XtraTab;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
//using WindowsFormsControlLibrary.Module.TestThread;
using WindowsFormsControlLibrary;

using System.Configuration;
using System.Threading;
using TestThread;
using System.Linq;

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
        private System.Timers.Timer changeSelectedPageTimer = new System.Timers.Timer();

        Dictionary<string, TestUnit> DicEquipmentInfo = new Dictionary<string, TestUnit>();
        Dictionary<string, TemperatureControl> DicTemperatureInfo = new Dictionary<string, TemperatureControl>();
        Dictionary<string, PublicElectricControl> DicPublicElectricInfo = new Dictionary<string, PublicElectricControl>();
        private List<TypeList> list = new List<TypeList>();
        


        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                string info = frmRefreshCycle.LoadRefreshCycle();
                string[] arr = info.Split(',');
                int result = 10000;
                int.TryParse(arr[0], out result);

                changeSelectedPageTimer.Interval = result;
                changeSelectedPageTimer.Elapsed += changeSelectedPage;
                if (Convert.ToBoolean(arr[1]))
                    changeSelectedPageTimer.Start();
                else
                    changeSelectedPageTimer.Stop();

                SetCombIC();
                List<TypeList> temp = LoadEquipmentTestInfo();
                if (temp != null)
                {
                    list = temp;
                    InitTree();
                    ExpandTree();
                }

                LoadEquipmentInfo();
                LoadTestUnitControl();

                //LoadPublicElectricInfo();
                //LoadPublicElectricControl();
                LoadTemperatureInfo();
                LoadTemperatureControl();
                ForcePageReflash();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
            //Thread Power Measure

            foreach (var item in DicPublicElectricInfo)
            {
                TestThread1 TestThread_ = new TestThread1();
                TestThread_.PowerSourceName = GetAppConfig("PowerSouceName");
                if( TestThread_.PowerSourceName.Length==0)
                {
                    MessageBox.Show("Error Power Source Name");
                }
                string Datesandhour = DateTime.Now.ToString("MMdd_hh_mm_ss");
                TestThread_.PowerFilePath = GetAppConfig("PowerFilePath");
                TestThread_.PowerFileName = Datesandhour + ".txt";
                if (GetAppConfig("PowerFileEnable") == "TRUE")
                {
                    TestThread_.MeasurePowerSwitch = true;
                }
                else TestThread_.MeasurePowerSwitch = false;
                Thread ThreadMeasureCurrent = new Thread(new ParameterizedThreadStart(TestThread_.MeasurePowerVoltageandCurrent));
                ThreadMeasureCurrent.IsBackground = true;
                ThreadMeasureCurrent.Start(item.Value);
            }

            //Thread Temp&Humidity
            TestThread1 TestThread_2 = new TestThread1();
            string Datesandhour2 = DateTime.Now.ToString("hh_mm_ss");
            string Datesandhour3 = DateTime.Now.ToString("yyyy_MM_dd");

            TestThread_2.TempFilePath = GetAppConfig("TempFilePath")+Datesandhour3+"\\";
            TestThread_2.TempFileName = Datesandhour2 + ".txt";
            TestThread_2.TempSourceName = GetAppConfig("TempSouceName");
            TestThread_2.TempSensor1Add = Convert.ToByte(GetAppConfig("TempSensor1Add"));
            TestThread_2.TempSensor2Add = Convert.ToByte(GetAppConfig("TempSensor2Add"));
            TestThread_2.TempSensor3Add = Convert.ToByte(GetAppConfig("TempSensor3Add"));
            TestThread_2.TempFileRecordStep = Convert.ToInt32(GetAppConfig("TempFileRecordStep"));
            if (GetAppConfig("TempFileEnable") == "TRUE")
            {
                TestThread_2.MeasurTempSwitch = true;
            }
            else TestThread_2.MeasurTempSwitch = false;
            Thread ThreadMeasureTemp = new Thread(new ParameterizedThreadStart(TestThread_2.MeasureTempandHumidity));
            ThreadMeasureTemp.IsBackground = true;

            List<TemperatureControl> tp = new List<TemperatureControl>();
            foreach (var item in DicTemperatureInfo)
            {
                tp.Add(item.Value);
            }
            ThreadMeasureTemp.Start(tp);

            //PCB control thread
            TestThread1 ThreadPCBcontrol = new TestThread1();
        }

        private void SetCombIC()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "InstrumentClusterConfiguration.json");
            List<InstrumentClusterConfiguration> list= JsonConvert.DeserializeObject<List<InstrumentClusterConfiguration>>(json);

            combIC.Properties.Items.Clear();
            foreach(InstrumentClusterConfiguration ic in list)
            {
                combIC.Properties.Items.Add(ic.InstrumentCluster);
            }
        }

        private List<TypeList> LoadEquipmentTestInfo()
        {
            string path = Application.StartupPath + "\\TestInfo";
            string json2 = JsonOperate.GetJson(path, "TypeList.json");

            List<TypeList> temp2 = JsonConvert.DeserializeObject<List<TypeList>>(json2);
            return temp2;
        }

        private void InitTree()
        {
            this.treeView1.Nodes.Clear();
            foreach (TypeList type in list)
            {
                if (type.parentname == "")
                {
                    TreeNode node = new TreeNode();
                    node.Name = type.parentname;
                    node.Text = type.typename;
                    node.Tag = type.typename;
                    this.treeView1.Nodes.Add(node);
                    addChildNode(type, node);
                }
            }
        }

        private void addChildNode(TypeList parenttype, TreeNode parentNode)
        {
            foreach (TypeList type in list)
            {
                if (type.parentname == parenttype.typename)
                {
                    TreeNode node = new TreeNode();
                    node.Name = type.parentname;
                    node.Text = type.typename + "," + type.typeinfo;
                    node.Tag = type.typename;
                    parentNode.Nodes.Add(node);
                    addChildNode(type, node);
                }
            }
        }

        private void ExpandTree()
        {
            foreach (TreeNode nodes in treeView1.Nodes)
            {
                nodes.ExpandAll();
            }
        }
        List<TypeList> selectedList = new List<TypeList>();
        private void GetAllSelectedNode(TreeNode parentNode)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                if (node.Name != "" && node.Checked)
                {
                    TypeList type = new TypeList();
                    type.parentname = node.Name;
                    type.typename = node.Tag.ToString();
                    selectedList.Add(type);
                }
            }
        }

        private void ForcePageReflash()
        {
            xtraTabControl1.BeginInvoke((MethodInvoker)delegate
            {
                for(int j=0;j< xtraTabControl1.TabPages.Count;j++)
                {
                    xtraTabControl1.SelectedTabPageIndex = j;
                    Thread.Sleep(50);
                }
                xtraTabControl1.SelectedTabPageIndex = 0;

            });
        }

        private void changeSelectedPage(object sender, EventArgs e)
        {
            xtraTabControl1.BeginInvoke((MethodInvoker)delegate
            {
                if (xtraTabControl1.TabPages.Count > 0)
                {
                    if (xtraTabControl1.SelectedTabPageIndex == xtraTabControl1.TabPages.Count - 1)
                    {
                        xtraTabControl1.SelectedTabPageIndex = 0;
                    }
                    else
                        xtraTabControl1.SelectedTabPageIndex = xtraTabControl1.SelectedTabPageIndex + 1;
                }
            });

        }
        public static string GetAppConfig(string strKey)
        {
            string Path = System.Windows.Forms.Application.StartupPath;// 获取路径
            string FileName = Path + "\\File.config";
            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = FileName;
            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            string key = config.AppSettings.Settings[strKey].Value;
            return key;
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
            int x = this.panel4.Width;
            int y = this.panel2.Height;

            int _x = x - 22;
            int _y = (y - 20) / 3;

            foreach (var item in DicTemperatureInfo)
            {
                item.Value.Width = _x;
                item.Value.Height = _y;
                item.Value.Location = new System.Drawing.Point((i % 1) * (_x + 10) + 3, (i / 1) * (_y + 10) + 3);
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

        #region FormClose
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
        #endregion

        #region FormMenu
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

        private void powerMeterAddressConfigurationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmPowerMeterConfiguration frm = new frmPowerMeterConfiguration();
            frm.ShowDialog();
        }

        private void historicalInformationReviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmHistoricalReview frm = new frmHistoricalReview();
            frm.ShowDialog();
        }

        private void iCRefreshCycleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRefreshCycle frm = new frmRefreshCycle();
            frm.changeSelectedPageTimer = this.changeSelectedPageTimer;
            if (frm.ShowDialog() == DialogResult.OK)
            {
                this.changeSelectedPageTimer = frm.changeSelectedPageTimer;
            }

        }
        #endregion


        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            treeView1.SelectedNode = e.Node;
            SetTreeNodeChecked(treeView1.SelectedNode);
        }

        private void SetTreeNodeChecked(TreeNode tn)
        {
            if (tn == null) return;
            if (tn.Nodes.Count > 0)
            {
                foreach (TreeNode item in tn.Nodes)
                {
                    item.Checked = tn.Checked;
                    SetTreeNodeChecked(item);
                }
            }
        }

        #region Button event

        private void btnStart_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in treeView1.Nodes)
            {
                GetAllSelectedNode(node);
            }
            if (selectedList.Count > 0)
            {
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnStop.Enabled = true;
                btnStart.BackColor = Color.DimGray;

                foreach (var item in DicEquipmentInfo)
                {
                    string SelectIC=combIC.EditValue.ToString().Replace(", ", ",");
                    if (SelectIC.Contains(item.Key))
                    {
                        TypeList[] arr = new TypeList[selectedList.Count()];
                        this.selectedList.CopyTo(arr);
                        item.Value.selectedList = arr.ToList();
                        item.Value.btnStart_Click(sender, e);
                    }
                   
                }
                selectedList.Clear();
            }
            else
            {
                MessageBox.Show("请选择要测试的项目!");
            }

        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            btnPause.Enabled = false;
            btnPause.BackColor = Color.Gray;
            btnResume.Enabled = true;
            btnResume.BackColor = Color.Yellow;
            btnStop.Enabled = false;
            btnStop.BackColor = Color.Gray;

            foreach (TreeNode node in treeView1.Nodes)
            {
                GetAllSelectedNode(node);
            }
            foreach (var item in DicEquipmentInfo)
            {
                string SelectIC = combIC.EditValue.ToString().Replace(", ", ",");
                if (SelectIC.Contains(item.Key))
                {
                    item.Value.btnPause_Click(sender, e);
                }

            }
            selectedList.Clear();
        }
        private void btnResume_Click(object sender, EventArgs e)
        {
            btnResume.Enabled = false;
            btnResume.BackColor = Color.Gray;
            btnPause.Enabled = true;
            btnPause.BackColor = Color.Yellow;
            btnStop.Enabled = true;
            btnStop.BackColor = Color.Red;

            foreach (TreeNode node in treeView1.Nodes)
            {
                GetAllSelectedNode(node);
            }
            foreach (var item in DicEquipmentInfo)
            {
                string SelectIC = combIC.EditValue.ToString().Replace(", ", ",");
                if (SelectIC.Contains(item.Key))
                {
                    item.Value.btnResume_Click(sender, e);
                }

            }
            selectedList.Clear();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            btnStop.BackColor = Color.Red;
            btnStart.Enabled = true;
            btnStart.BackColor = Color.Lime;
            btnPause.Enabled = false;
            btnPause.BackColor = Color.Yellow;
            btnResume.Enabled = false;
            btnResume.BackColor = Color.Yellow;
            btnStart.BackColor = Color.LimeGreen;

            foreach (TreeNode node in treeView1.Nodes)
            {
                GetAllSelectedNode(node);
            }
            foreach (var item in DicEquipmentInfo)
            {
                string SelectIC = combIC.EditValue.ToString().Replace(", ", ",");
                if (SelectIC.Contains(item.Key))
                {
                    item.Value.btnStop_Click(sender, e);
                }

            }
            selectedList.Clear();
        }

        #endregion
        
    }
}
