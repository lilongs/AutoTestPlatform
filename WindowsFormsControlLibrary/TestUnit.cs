using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoTestDLL.Model;
using System.Threading;
using AutoTestDLL.Util;
using Newtonsoft.Json;
using System.Windows.Forms.DataVisualization.Charting;
using AutoTestDLL.Module;
using TestThread;
using WindowsFormsControlLibrary.Module;
using System.Configuration;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace WindowsFormsControlLibrary
{
    public partial class TestUnit : UserControl
    {
        #region unlock cluster
        [DllImport("UnlockDllMQB_FPK_MP20 .dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public extern static int Generate_Response_KS(byte[] Challenge_M, byte[] Challenge_K, byte[] Response_M);

        private void UnlockCluster_CP()
        {
            byte[] Challenge_M = new byte[8];
            byte[] Challenge_K = new byte[8];
            byte[] Response_M = new byte[8];
            string str = "";
            string str1 = "01 02 03 04 05 06 07 08";
            string M = "80 01 01 02 03 04 05 06 07 08 00";
            Int32 id_send = 0x17FC0114;
            Int32 id_recv = 0x17FE0114;
            DIDV.CANmsgSend(id_send, M);
            byte[] recv = DIDV.CANmsgReceived(id_recv);
            byte[] K = recv.Skip(2).Take(8).ToArray();
            str = DIDV.ByteToHex(K).Replace(" ", "");

            Challenge_M = Sys.HexStringToBytes(str1);
            Challenge_K = K;
            //  Response_M.Append("");
            int i = Generate_Response_KS(Challenge_M, Challenge_K, Response_M);

            string send = "80 02 " + DIDV.ByteToHex(Response_M) + " 00";
            DIDV.CANmsgSend(id_send, send);
            byte[] ttt = DIDV.CANmsgReceived(id_recv);
        }

        #endregion

        #region not important
        public TestUnit()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private List<TypeList> list = new List<TypeList>();
        Dictionary<string, List<TestStep>> ReadyTestInfo = null;
        public List<TypeList> selectedList = new List<TypeList>();
        private List<TestStep> testInfo = new List<TestStep>();

        private List<TestStep> CompletedList = new List<TestStep>();

        public DateTime TestStartTime = new DateTime();
        private void TestUnit_Load(object sender, EventArgs e)
        {
            try
            {
                List<TypeList> temp = LoadEquipmentTestInfo();
                if (temp != null)
                {
                    list = temp;
                    InitTree();
                    ExpandTree();
                }
                InitChart();
                InitCan();
                //  LoadManualInstruction();
                TestCANmessageDefine();
                this.groupControl3.Text = this.Tag.ToString() + " step info:";
                Thread.Sleep(500);
                PB.LEDcontrol(0, false);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }
        private List<TypeList> LoadEquipmentTestInfo()
        {
            List<TypeList> result = new List<TypeList>();

            string path = Application.StartupPath + "\\TestInfo";
            string json = JsonOperate.GetJson(path, "InstrumentClusterTestInfo.json");
            List<EquipmentTestInfo> temp = JsonConvert.DeserializeObject<List<EquipmentTestInfo>>(json);

            string json2 = JsonOperate.GetJson(path, "TypeList.json");
            List<TypeList> temp2 = JsonConvert.DeserializeObject<List<TypeList>>(json2);
            if (temp != null && temp2 != null)
            {
                List<EquipmentTestInfo> list = temp.Where(t => t.InstrumentCluster == this.Tag.ToString()).ToList();

                foreach (EquipmentTestInfo info in list)
                {
                    result.AddRange(temp2.Where(t => t.typename == info.TypeName || t.parentname == info.TypeName).ToList());
                }
            }
            return result;
        }

        private List<TypeList> LoadTypeList()
        {
            List<TypeList> temp = new List<TypeList>();
            string path = Application.StartupPath + "\\TestInfo"; ;
            string json = JsonOperate.GetJson(path, "TypeList.json");
            temp = JsonConvert.DeserializeObject<List<TypeList>>(json);
            return temp;
        }

        private void InitChart()
        {
            userCurve1.SetLeftCurve("A", null, Color.DarkRed);
            userCurve1.SetRightCurve("B", null, Color.Black);
        }

        public void ChartValueFill(float value, float voltage)
        {
            userCurve1.AddCurveData(
               new string[] { "A", "B" },
               new float[]
               {
                    value,voltage
               }
           );
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
        private void LoadStepInfo()
        {
            string path = Application.StartupPath + "\\TestInfo";
            string json = JsonOperate.GetJson(path, "TestStep.json");
            testInfo = JsonConvert.DeserializeObject<List<TestStep>>(json);
        }
        //}

        public void Setnoticecolor(Color cl)
        {
            if (this.IsHandleCreated)
            {
                treeView1.BeginInvoke((MethodInvoker)delegate
                {
                    this.treeView1.BackColor = cl;
                });
            }

        }
        public void SetScale(float max, float min, string danwei)
        {
            //  userCurve1.ValueMaxLeft = max;
            //  userCurve1.ValueMinLeft = min;  
            if (this.IsHandleCreated)
            {
                Current.BeginInvoke((MethodInvoker)delegate
                {
                    this.Current.Text = danwei;
                });
            }

        }
        private void FillStepInfo(TestStep step)
        {
            if (this.IsHandleCreated)
            {

                txttypename.BeginInvoke((MethodInvoker)delegate
                {
                this.txttypename.Text = step.typename;
                });
                txtstepname.BeginInvoke((MethodInvoker)delegate
                {
                    this.txtstepname.Text = step.stepname;
                });
                txtmodelname.BeginInvoke((MethodInvoker)delegate
                {
                    this.txtmodelname.Text = step.modelname;
                });
                txtvoltage.BeginInvoke((MethodInvoker)delegate
                {
                    this.txtvoltage.Text = step.voltage.ToString() + "v";
                });
                txtcycletime.BeginInvoke((MethodInvoker)delegate
                {
                    this.txtcycletime.Text = step.cycletime.ToString() + "s";
                });
            }
        }
        string DebugFileName = "";

        FileOperate FileOperation = new FileOperate();
        public void ShowInfo(RichTextBox richTextBox, string info, Color color)
        {
            string Datesandhour = DateTime.Now.ToString("yyyy_MM_dd");

            string path = GetAppConfig("DebugFilePath") + this.Tag.ToString() + "_Debug\\" + Datesandhour + "\\";
            try
            {
                FileOperation.createFile(path, DebugFileName, info);
                if (this.IsHandleCreated)
                {
                    richTextBox.BeginInvoke((MethodInvoker)delegate
                {
                    richTextBox.SelectionStart = richTextBox.TextLength;
                    richTextBox.SelectionLength = 0;
                    richTextBox.SelectionColor = color;

                    if (String.IsNullOrEmpty(richTextBox.Text))
                        richTextBox.AppendText(info);
                    else
                        richTextBox.AppendText(Environment.NewLine + info);
                    richTextBox.SelectionColor = richTextBox.ForeColor;

                    richTextBox.ScrollToCaret();
                });
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        public void ShowInfo(RichTextBox richTextBox, string info)
        {
            if (this.IsHandleCreated)
            {
                richTextBox.BeginInvoke((MethodInvoker)delegate
            {
                richTextBox.Text = info;
            });
            }
        }

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
        #endregion

        #region not important

        KvaserDbcMessage dBCCan = new KvaserDbcMessage();
        List<AutoTestDLL.Model.Message> message_List1 = new List<AutoTestDLL.Model.Message>();
        List<AutoTestDLL.Model.Message> message_List2 = new List<AutoTestDLL.Model.Message>();
        bool IsTestEnd = false;
        bool IsStop = false;
        private List<InstrumentClusterConfiguration> LoadICInfo()
        {
            List<InstrumentClusterConfiguration> temp = new List<InstrumentClusterConfiguration>();
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "InstrumentClusterConfiguration.json");
            temp = JsonConvert.DeserializeObject<List<InstrumentClusterConfiguration>>(json);
            return temp;
        }
        private List<PowerMeter> LoadPowerInfo()
        {
            List<PowerMeter> temp = new List<PowerMeter>();
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "PowerMeter.json");
            temp = JsonConvert.DeserializeObject<List<PowerMeter>>(json);
            return temp;
        }
        private string GetPowerMeterName()
        {
            List<PowerMeter> powermeter = LoadPowerInfo();
            string temp = powermeter.Where(x => x.instrumentcluster == this.Tag.ToString()).ToList()[0].address;
            return temp;
        }
        private List<CAN> LoadCANInfo()
        {
            List<CAN> listCAN = new List<CAN>();
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "CAN.json");
            listCAN = JsonConvert.DeserializeObject<List<CAN>>(json);
            return listCAN;
        }
        private void InitCan()
        {
            //LoadDB            
            dBCCan.loadDb(Application.StartupPath + "\\DbcFile\\MQB2020.MQB_W_KCAN_KMatrix_V9.06.02F_20181213_MB_mod1(1).dbc");
            //LoadMessage and filter Node is  "BCM" and "Gateway" ,GenMsgSendType is "Cyclic"
            List<AutoTestDLL.Model.Message> list = dBCCan.LoadMessages();
            message_List1 = list.Where(x => (x.tx_node == "BCM" || x.tx_node == "Gateway") && x.GenMsgSendType == "Cyclic").ToList();
            //rule out manual instruct
            message_List1 = message_List1.Where(x => (x.name != "Airbag_01" && x.name != "Dimmung_01" && x.name != "Motor_09" && x.name != "Motor_26" && x.name != "Blinkmodi_02" && x.name != "Motor_04" && x.name != "ESP_24")).ToList();
            //   message_List1 = message_List1.Where(x => (x.name != "Airbag_01" && x.name != "Dimmung_01" && x.name != "ESP_24" && x.name != "LH_EPS_01" && x.name != "ESP_20" && x.name != "RKA_01" && x.name != "ESP_20" && x.name != "RKA_01" && x.name != "Klemmen_Status_01" && x.name != "LH_EPS_01")).ToList();//&& x.name != "ESP_20"&& x.name != "RKA_01"&& x.name != "Klemmen_Status_01" && x.name != "LH_EPS_01"  
            //get this ic configuration
            List<InstrumentClusterConfiguration> listIC = LoadICInfo();
            List<CAN> listCAN = LoadCANInfo();

            string channelName = listIC.Where(x => x.InstrumentCluster == this.Tag.ToString() && x.CommunicationType == "CAN").ToList()[0].Value;
            string bitrate = listCAN.Where(x => x.Channel == channelName).ToList()[0].Baudrate;

            int channelNo = -1;
            channelName = channelName.Replace("channel", "");
            int.TryParse(channelName, out channelNo);

            //initChannel
            dBCCan.initChannel(channelNo, bitrate);
            DIDV.initChannel(channelNo, bitrate);

        }
        #endregion

        #region Parameter define
        TestThread1 TestThread_ = new TestThread1();
        Thread thread_Maintest;
        Thread thread_SendDBCmessage;
        Thread thread_SendRealTimeMessage;
        Thread thread_RunMeterMeasure;
        Thread thread_PCBcontrol;
        Thread thTestTime;
        Thread thread_Digmsg;
        PCBcontrol PB = new PCBcontrol();
        _Lib_Power PowerControl = new _Lib_Power();
        private DateTime PauseTime;
        private DateTime ResumeTime;
        double CountTime = 0;
        string FazitString = "";
        private double ClockOffset = 0;
        KvaserCommunication DIDV = new KvaserCommunication();
        TimeSpan TimePause = TimeSpan.FromMilliseconds(0);
        bool IsResume = false;
        #endregion

        #region Button event
        private void PowerSourceControl()
        {
            PowerControl.PowerSourceControl(1, 14);  //PCB 供电
            PowerControl.PowerSourceControl(2, 14);
        }
        public void btnStart_Click(object sender, EventArgs e)
        {

            TestThread_.MeterSourceName = GetPowerMeterName();
            Setnoticecolor(Color.White);
            //PowerControl.PowerSourceControl(1, 14);  //PCB 供电
            //PowerControl.PowerSourceControl(2, 14);

            Thread powerTh = new Thread(PowerSourceControl);
            powerTh.IsBackground = true;
            powerTh.Start();

            string Datesandhour = DateTime.Now.ToString("yyyy_MM_dd");
            string Datesandhour1 = DateTime.Now.ToString("hh_mm_ss");
            DIDV.LogFileName = Datesandhour1 + ".txt";
            DIDV.LogFilePath = GetAppConfig("CANlogFilePath") + this.Tag.ToString() + "_CANlog\\" + Datesandhour + "\\";

            if (TestThread_.MeterSourceName.Length == 0)
            {
                MessageBox.Show(TestThread_.MeterSourceName);
            }
            try
            {
                CompletedList.Clear();
                this.dataGridView1.DataSource = null;
                if (!String.IsNullOrEmpty(richTextBox1.Text))
                {
                    richTextBox1.Text = string.Empty;
                }
                //foreach (TreeNode node in treeView1.Nodes)
                //{
                //    GetAllSelectedNode(node);
                //}
                if (selectedList.Count <= 0)
                {
                    MessageBox.Show("Please select test nodes!");
                    return;
                }
                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnStop.Enabled = true;

                LoadStepInfo();
                ReadyTestInfo = new Dictionary<string, List<TestStep>>();
                foreach (TypeList tp in selectedList)
                {
                    List<TestStep> tempList = testInfo.Where(x => x.typename == tp.typename).ToList();
                    ReadyTestInfo.Add(tp.typename, tempList.OrderBy(x => Convert.ToInt32(x.stepname.Replace("Step", ""))).ToList());
                }
                IsStop = false;
                IsTestEnd = false;
                TestThread_.IsStop = false;
                TestThread_.IsTestEnd = false;

                // TestThread_.MeterFileName = this.Tag.ToString() + "_Current.txt";
                TestThread_.MeterFileRecordStep = Convert.ToInt32(GetAppConfig("MeterFileRecordStep")); ////电流表
                thread_RunMeterMeasure = new Thread(new ParameterizedThreadStart(TestThread_.MeasureMeterCurrent));
                thread_RunMeterMeasure.Start(this);

                thread_PCBcontrol = new Thread(TestThread_.PCBcontrol);  //PCB 控制
                thread_PCBcontrol.Start();
                TestThread_.Channal = Convert.ToByte(this.Tag.ToString().Substring(2, 1));
                TestThread_.FuelEnable = false;

                thread_Maintest = new Thread(RunTest); //测试步骤
                thread_Maintest.Start();

                string Datesandhour2 = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss_");
                DebugFileName = Datesandhour2 + ".txt";
                btnStart.BackColor = Color.DimGray;
                this.selectedList.Clear();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        public void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                PauseTime = DateTime.Now;

                ThreadState TestTimestate = thTestTime.ThreadState;
                ThreadState Nowstate = thread_Maintest.ThreadState;

                if (Nowstate == ThreadState.Running || Nowstate == ThreadState.WaitSleepJoin || Nowstate == ThreadState.Background)
                {
                    //Pause the thread
                    thread_Maintest.Suspend();
                    //Disable the Pause button
                    btnPause.Enabled = false;
                    btnPause.BackColor = Color.Gray;
                    //Enable the resume button
                    btnResume.Enabled = true;
                    btnResume.BackColor = Color.Yellow;
                    btnStop.Enabled = false;
                    btnStop.BackColor = Color.Gray;
                }

                if (TestTimestate == ThreadState.Running || TestTimestate == ThreadState.WaitSleepJoin || TestTimestate == ThreadState.Background)
                {
                    //Pause the thread
                    thTestTime.Suspend();
                    //Disable the Pause button
                    btnPause.Enabled = false;
                    btnPause.BackColor = Color.Gray;
                    //Enable the resume button
                    btnResume.Enabled = true;
                    btnResume.BackColor = Color.Yellow;
                    btnStop.Enabled = false;
                    btnStop.BackColor = Color.Gray;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }
        public void btnResume_Click(object sender, EventArgs e)
        {
            try
            {
                ResumeTime = DateTime.Now;
                IsResume = true;
                if (thread_Maintest.ThreadState == ThreadState.SuspendRequested || thread_Maintest.ThreadState == ThreadState.Suspended)
                {
                    thread_Maintest.Resume();
                    btnResume.Enabled = false;
                    btnResume.BackColor = Color.Gray;
                    btnPause.Enabled = true;
                    btnPause.BackColor = Color.Yellow;
                    btnStop.Enabled = true;
                    btnStop.BackColor = Color.Red;
                }
                if (thTestTime.ThreadState == ThreadState.SuspendRequested || thTestTime.ThreadState == ThreadState.Suspended)
                {
                    thTestTime.Resume();
                    btnResume.Enabled = false;
                    btnResume.BackColor = Color.Gray;
                    btnPause.Enabled = true;
                    btnPause.BackColor = Color.Yellow;
                    btnStop.Enabled = true;
                    btnStop.BackColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        public void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                TestThread_.IsStop = true;
                thread_RunMeterMeasure.Abort();
                btnStop.Enabled = false;
                btnStop.BackColor = Color.Red;
                btnStart.Enabled = true;
                btnStart.BackColor = Color.Lime;
                btnPause.Enabled = false;
                btnPause.BackColor = Color.Yellow;
                btnResume.Enabled = false;
                btnResume.BackColor = Color.Yellow;
                btnStart.BackColor = Color.LimeGreen;
                TestThread_.PowerONOFF = false;
                TestThread_.Function = 0;

            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        #endregion

        #region TestStep

        private void InitHardware(TestStep ts)
        {
            DateTime centuryBegin = DateTime.Now;
            TestThread_.PowerSourceVal = ts.voltage;
            string msg;
            string HW = DIDV.ReadHWVersion(out msg);
            if (msg.Length != 0)
            {
                ShowInfo(richTextBox1, msg, Color.Red);
                Setnoticecolor(Color.Red);
            }
            if (this.IsHandleCreated)
            {
                txthw.BeginInvoke((MethodInvoker)delegate
            {
                this.txthw.Text = HW;
            });
            }
            string SW = DIDV.ReadSWVersion(out msg);
            if (msg.Length != 0)
            {
                ShowInfo(richTextBox1, msg, Color.Red);
                Setnoticecolor(Color.Red);
            }
            if (this.IsHandleCreated)
            {
                txtsw.BeginInvoke((MethodInvoker)delegate
            {
                this.txtsw.Text = SW;
            });
            }
            string Fazit1 = DIDV.ReadFazitnum(out msg);
            if (msg.Length != 0)
            {
                ShowInfo(richTextBox1, msg, Color.Red);
                Setnoticecolor(Color.Red);
            }
            FazitString = Fazit1;
            if (this.IsHandleCreated)
            {
                txtfazit.BeginInvoke((MethodInvoker)delegate
            {
                this.txtfazit.Text = Fazit1;
            });
            }
            string PartNum = DIDV.ReadPartNumber(out msg);
            if (msg.Length != 0)
            {
                ShowInfo(richTextBox1, msg, Color.Red);
                Setnoticecolor(Color.Red);
            }
            if (this.IsHandleCreated)
            {
                txtpart.BeginInvoke((MethodInvoker)delegate
                {
                    this.txtpart.Text = PartNum;
                });
            }

            UnlockCluster_CP();
            Timespend(centuryBegin, ts.cycletime);

        }
        private void SleepMode(TestStep ts)
        {
            DateTime centuryBegin = DateTime.Now;
            //enter sleep mode
            int cnt = 0;

            string fazittemp = "";
            string Datesandhour = DateTime.Now.ToString("dd_MM_yyyy");
            string Datesandhour1 = DateTime.Now.ToString("hh_mm_ss_");
            if (FazitString.Length != 0)
            {
                fazittemp = FazitString;
            }
            else fazittemp = "0000000000";
            TestThread_.PowerSourceVal = ts.voltage;
            TestThread_.MeterFileName = ts.stepname + "_SleepMode" + Datesandhour1 + ".txt";
            TestThread_.MeterFilePath = GetAppConfig("MeterFilePath") + this.Tag.ToString() + "_Current\\" + fazittemp + "_" + Datesandhour + "\\";
            TestThread_.MeterRange = 1;
            TestThread_.SetMeterScal = true;
            TestThread_.MeasureMetertSwitch = true;
            Timespend(centuryBegin, ts.cycletime);
        }

        private void OperationMode(TestStep ts)
        {
            try
            {
                thread_SendDBCmessage = new Thread(Send_BCM_Gateway_CanMessage); ////循环发送BCM、Gateway信息
                thread_SendDBCmessage.Start();

                thread_SendRealTimeMessage = new Thread(Send_ManualInstruction); ////循环发送手动指令
                thread_SendRealTimeMessage.Start();

                thread_Digmsg = new Thread(SendDigMessage);
                thread_Digmsg.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }

            DateTime centuryBegin = DateTime.Now;
            string fazittemp = "";
            if (FazitString.Length != 0)
            {
                fazittemp = FazitString;
            }
            else fazittemp = "0000000000";
            TestThread_.PowerSourceVal = ts.voltage;
            string Datesandhour = DateTime.Now.ToString("dd_MM_yyyy");
            string Datesandhour1 = DateTime.Now.ToString("_hh_mm_ss_");
            TestThread_.MeterFileName = ts.stepname + "_OperationMode" + Datesandhour1 + ".txt";
            TestThread_.MeterFilePath = GetAppConfig("MeterFilePath") + this.Tag.ToString() + "_Current\\" + fazittemp + "_" + Datesandhour + "\\";
            TestThread_.MeterRange = 1;
            TestThread_.SetMeterScal = true;
            TestThread_.MeasureMetertSwitch = true;
            TestThread_.Channal = Convert.ToByte(this.Tag.ToString().Substring(2, 1));

            Timespend(centuryBegin, ts.cycletime);
            try
            {
                thread_SendDBCmessage.Abort();
                thread_SendRealTimeMessage.Abort();
                thread_Digmsg.Abort();
            }
            catch (Exception ex)
            {
            }

        }

        private void Normalmode(TestStep ts)
        {
            try
            {
                thread_SendDBCmessage = new Thread(Send_BCM_Gateway_CanMessage); ////循环发送BCM、Gateway信息
                thread_SendDBCmessage.Start();

                thread_SendRealTimeMessage = new Thread(Send_ManualInstruction); ////循环发送手动指令
                thread_SendRealTimeMessage.Start();

                thread_Digmsg = new Thread(SendDigMessage);
                thread_Digmsg.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
            KL15ONOFF(true);
            DateTime centuryBegin = DateTime.Now;
            string fazittemp = "";
            if (FazitString.Length != 0)
            {
                fazittemp = FazitString;
            }
            else fazittemp = "0000000000";
            TestThread_.PowerSourceVal = ts.voltage;
            string Datesandhour = DateTime.Now.ToString("dd_MM_yyyy");
            string Datesandhour1 = DateTime.Now.ToString("_hh_mm_ss_");
            TestThread_.MeterFileName = ts.stepname + "_OperationMode" + Datesandhour1 + ".txt";
            TestThread_.MeterFilePath = GetAppConfig("MeterFilePath") + this.Tag.ToString() + "_Current\\" + fazittemp + "_" + Datesandhour + "\\";
            try
            {
                TestThread_.MeterRange = 1;
                TestThread_.SetMeterScal = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                ShowInfo(richTextBox1, ex.Message, Color.Red);
                Setnoticecolor(Color.Red);
            }
            TestThread_.MeasureMetertSwitch = true;


            TestThread_.Channal = Convert.ToByte(this.Tag.ToString().Substring(2, 1));

            Timespend(centuryBegin, ts.cycletime);
            try
            {
                thread_SendDBCmessage.Abort();
                thread_SendRealTimeMessage.Abort();
                thread_Digmsg.Abort();
            }
            catch (Exception ex)
            {
            }
        }


        private void UpdateTestTime()
        {
            while (true)
            {
                if (IsTestEnd || IsStop)
                {
                    break;
                }
                if (IsResume)
                {
                    TimePause = TimePause + (ResumeTime - PauseTime);
                    IsResume = false;
                }
                TimeSpan sp = (DateTime.Now - TestStartTime) - TimePause;

                //测试持续时间
                if (this.IsHandleCreated)
                {
                    txttesttime.BeginInvoke((MethodInvoker)delegate
                    {
                        this.txttesttime.Text = string.Format("{0:F2}s", sp.TotalSeconds);
                    });
                }
                //测试倒计时
                double diff = CountTime - sp.TotalSeconds;
                TimeSpan sp2 = TimeSpan.FromSeconds(diff);
                if (this.IsHandleCreated)
                {
                    countdown1.BeginInvoke((MethodInvoker)delegate
                    {
                        if (diff <= 0)
                        {
                            countdown1.SetText(string.Format("{0:D3}D{1:D2}H{2:D2}M{3:D2}S", 0, 0, 0, 0));
                        }
                        else
                        {
                            countdown1.SetText(string.Format("{0:D3}D{1:D2}H{2:D2}M{3:D2}S", sp2.Days, sp2.Hours, sp2.Minutes, sp2.Seconds));
                        }
                    });
                }
                Thread.Sleep(100);
            }
        }

        private void UpdateTestStepInfo(TestStep step)
        {
            #region 界面步骤信息赋值
            string typename = step.typename;
            string stepname = step.stepname;
            string modelname = step.modelname;
            ShowInfo(richTextBox1, "正在进行：" + typename + "--" + stepname + "测试！", Color.Red);
            //Actual test steps
            ShowInfo(richTextBox2, typename + "-" + stepname);
            //Measuring value
            ShowInfo(richTextBox3, "OK");
            //Model
            ShowInfo(richTextBox4, modelname);
            #endregion
        }
        private void Timespend(DateTime dt, double time)
        {
            TimeSpan elapsedSpan = new TimeSpan();
            // while (true)
            while (!IsStop || !IsTestEnd)
            {
                DateTime currentDate = DateTime.Now;
                elapsedSpan = currentDate - dt;
                if (elapsedSpan.TotalSeconds >= time)
                {
                    break;
                }
                if (elapsedSpan.TotalSeconds >= time - 50)
                {
                    TestThread_.TestStepFinish = true;
                }
                Thread.Sleep(10);
            }
        }

        List<ManualInstruction> listManualInstruction = new List<ManualInstruction>();
        ManualInstruction KL15 = new ManualInstruction();
        ManualInstruction Dimming = new ManualInstruction();
        ManualInstruction LED = new ManualInstruction();
        ManualInstruction Tacho = new ManualInstruction();
        ManualInstruction Speed = new ManualInstruction();
        ManualInstruction Temp = new ManualInstruction();
        ManualInstruction LH_EPS_01 = new ManualInstruction();
        ManualInstruction Lamp_OBD = new ManualInstruction();

        private void KL15ONOFF(bool onoff)
        {

            if (onoff)
            {
                KL15.enable = true;
                Dimming.enable = true;
                KL15.data = "00 00 02 00";
            }
            else
            {
                KL15.enable = false;
                Dimming.enable = false;
                KL15.data = "00 00 00 00";
            }

        }

        private void TestCANmessageDefine()
        {
            KL15.cycletime = 100;
            KL15.cyclecount = 0;
            KL15.data = "00 00 00 00";
            KL15.dlc = 4;
            KL15.enable = false;
            KL15.id = "3C0";
            KL15.type = "KL15";

            Dimming.cycletime = 200;
            Dimming.cyclecount = 0;
            Dimming.data = "C0 00 00 00 00 00 00 00";
            Dimming.dlc = 8;
            Dimming.enable = false;
            Dimming.id = "5F0";
            Dimming.type = "Dimming";

            LED.cycletime = 100;
            LED.cyclecount = 0;
            LED.data = "FF FF FF FF FF FF FF FF";//27 28
            LED.dlc = 8;
            LED.enable = false;
            LED.id = "366";
            LED.type = "LED";

            Lamp_OBD.cycletime = 100;
            Lamp_OBD.cyclecount = 0;
            Lamp_OBD.data = "00 00 00 00 00 00 00 00";//27 28
            Lamp_OBD.dlc = 8;
            Lamp_OBD.enable = false;
            Lamp_OBD.id = "3C7";
            Lamp_OBD.type = "LED";

            Tacho.cycletime = 200;
            Tacho.cyclecount = 0;
            Tacho.data = "55 00 00 05 FF 00 00 00";
            Tacho.dlc = 8;
            Tacho.enable = false;
            Tacho.id = "107";
            Tacho.type = "Tacho";

            Speed.cycletime = 100;
            Speed.cyclecount = 0;
            Speed.data = "00 00 00 00 00 00 00 00";
            Speed.dlc = 8;
            Speed.enable = false;
            Speed.id = "31B";
            Speed.type = "Speed";

            Temp.cycletime = 500;
            Temp.cyclecount = 0;
            Temp.data = "80 0E FF 7F 14 11 00 01";//bit 0-7
            Temp.dlc = 8;
            Temp.enable = false;
            Temp.id = "647";
            Temp.type = "Temp";

            LH_EPS_01.cycletime = 50;
            LH_EPS_01.cyclecount = 0;
            LH_EPS_01.data = "C0 00 00 00 00 00 00 00";//bit 0-7
            LH_EPS_01.dlc = 8;
            LH_EPS_01.enable = false;
            LH_EPS_01.id = "32A";
            LH_EPS_01.type = "LH_EPS_01";

            listManualInstruction.Add(KL15);
            listManualInstruction.Add(Dimming);
            listManualInstruction.Add(LED);
            listManualInstruction.Add(Tacho);
            listManualInstruction.Add(Speed);
            listManualInstruction.Add(Temp);
            listManualInstruction.Add(LH_EPS_01);
        }

        public void Speed_CRC_BZReflash()
        {
            byte speed_crc = DIDV.CalcCrc(Convert.ToInt64(Speed.id, 16), Speed.dlc, Sys.HexStringToBytes(Speed.data));
            byte speed_bz = DIDV.BZCount(Sys.HexStringToBytes(Speed.data)[1]);
            Speed.data = Convert.ToString(speed_crc, 16).PadLeft(2, '0') + " " + Convert.ToString(speed_bz, 16).PadLeft(2, '0') + Speed.data.Substring(5);
        }

        public void LH_EPS_CRC_BZReflash()
        {
            byte LH_EPS_crc = DIDV.CalcCrc(Convert.ToInt64(LH_EPS_01.id, 16), LH_EPS_01.dlc, Sys.HexStringToBytes(LH_EPS_01.data));
            byte LH_EPS_bz = DIDV.BZCount(Sys.HexStringToBytes(LH_EPS_01.data)[1]);
            LH_EPS_01.data = Convert.ToString(LH_EPS_crc, 16).PadLeft(2, '0') + " " + Convert.ToString(LH_EPS_bz, 16).PadLeft(2, '0') + LH_EPS_01.data.Substring(5);

        }
        #endregion

        #region Test Thread
        private void Send_BCM_Gateway_CanMessage()
        {
            if (message_List1.Count > 0)
            {
                while (!IsStop)
                {
                    if (!IsTestEnd)
                    {
                        //LoadSignal
                        foreach (AutoTestDLL.Model.Message message in message_List1)
                        {
                            if (message.GenMsgCycleTime == message.CycleCount)
                            {
                                List<Signal> signalList = new List<Signal>();
                                signalList = dBCCan.LoadSignalsById(message.id);
                                //  dBCCan.sendMsg(message.dlc,signalList);
                                message.CycleCount = 0;
                            }
                            else
                            {
                                message.CycleCount += 10;
                            }
                        }
                        Thread.Sleep(10);
                    }
                    else
                    {
                        break;
                    }
                }

            }

        }
        byte MessageReflahCounter = 0;
        private void TestCANmessageReflash()
        {

            byte[] EngineSpeed_H = new byte[] { 0, 0x02, 0, 0x04, 0, 0x06, 0, 0x08 };
            byte[] EngineSpeed_L = new byte[] { 0, 0x05, 0, 0x15, 0, 0x44, 0, 0xFF };
            byte[] Tachospeed = new byte[] { 0, 1, 2, 3, 4, 5, 0, 1 };

            Speed.data = Speed.data.Substring(0, 6) + Convert.ToString(EngineSpeed_H[MessageReflahCounter], 16).PadLeft(2, '0') + " " +
                                         Convert.ToString(EngineSpeed_L[MessageReflahCounter], 16).PadLeft(2, '0') + Speed.data.Substring(11);
            Tacho.data = Tacho.data.Substring(0, 3) + Convert.ToString(Tachospeed[MessageReflahCounter], 16).PadLeft(2, '0') + " " + Tacho.data.Substring(5);

            string[] turnLED = { "FF FF FF FF FF FF FF FF", "00 00 00 00 00 00 00 00" };
            LED.data = turnLED[MessageReflahCounter % 2];

            MessageReflahCounter++;
            if (MessageReflahCounter > 7)
            {
                MessageReflahCounter = 0;
            }

        }

        private void Send_ManualInstruction()
        {
            string msg;
            if (listManualInstruction.Count > 0)
            {
                DateTime timestart = DateTime.Now;
                TimeSpan sp = new TimeSpan();
                DateTime timestartflash = timestart;
                TimeSpan spflash = new TimeSpan();
                DateTime timestartfule = timestart;
                TimeSpan spfule = new TimeSpan();

                int j = 0;
                while (!IsStop)
                {
                    if (!IsTestEnd)
                    {
                        //更新 can信息
                        DateTime now = DateTime.Now;
                        DateTime now1 = DateTime.Now;
                        DateTime nowfule = DateTime.Now;

                        sp = now - timestart;
                        spflash = now1 - timestartflash;
                        spfule = nowfule - timestartfule;

                        switch (j)
                        {
                            case 0:
                                if (spfule.TotalMilliseconds >= 6000)
                                {
                                    TestThread_.FuelEnable = true;
                                    timestartfule = nowfule;
                                }
                                Speed.enable = false;
                                Tacho.enable = true;
                                LED.enable = true;
                                Temp.enable = true;
                                Lamp_OBD.enable = true;
                                break;
                            case 1:
                                TestThread_.FuelEnable = false;
                                Speed.enable = true;
                                Tacho.enable = true;
                                LED.enable = true;
                                Temp.enable = true;
                                Lamp_OBD.enable = true;
                                break;
                        }

                        if (spflash.TotalMilliseconds >= 3000)
                        {
                            TestCANmessageReflash();
                            timestartflash = now1;
                        }
                        if (sp.TotalMilliseconds >= 60000)
                        {
                            j++;
                            if (j > 1) j = 0;
                            timestart = now;
                        }
                        foreach (ManualInstruction instruct in listManualInstruction)
                        {
                            if (instruct.enable)//&& instruct.enable
                            {
                                if (instruct.cycletime == instruct.cyclecount)
                                {
                                    int id = Convert.ToInt32(instruct.id, 16);
                                    int dlc = instruct.dlc;
                                    byte[] data = Sys.HexStringToBytes(instruct.data);
                                    dBCCan.send(id, dlc, data, 0);
                                    instruct.cyclecount = 0;
                                    if (id == Convert.ToInt32(Speed.id, 16))
                                    {
                                        Speed_CRC_BZReflash();
                                    }
                                    else if (id == Convert.ToInt32(LH_EPS_01.id, 16))
                                    {
                                        LH_EPS_CRC_BZReflash();
                                    }

                                }
                                else
                                {
                                    instruct.cyclecount += 10;
                                }
                            }
                        }
                        Thread.Sleep(10);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void SendDigMessage()
        {
            string msg;
            string dtcmsg = "";
            string icclockmsg = "";
            string checkcounter = "";
            byte hour, min, second;
            int offset = 30000;


            while (!IsStop)
            {
                if (!IsTestEnd)
                {
                    //check dtc
                    DIDV.ReadDTC(out dtcmsg);
                    Thread.Sleep(50);
                    DIDV.IC_Clock_Readtime(out hour, out min, out second);
                    double starttime = hour * 3600 + min * 60 + second;
                    Thread.Sleep(offset);
                    DIDV.IC_Clock_Readtime(out hour, out min, out second);
                    double endtime = hour * 3600 + min * 60 + second;
                    if (endtime < starttime)
                    {
                        endtime = endtime + 24 * 3600;
                    }
                    if (endtime - starttime > 32)
                    {
                        string date = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss: ");

                        icclockmsg = date + " Check clock error . offset: " + (endtime - starttime).ToString() + Environment.NewLine;
                    }
                    ////Check Reset counter
                    DIDV.CheckResetCounter(out checkcounter);
                    Thread.Sleep(50);
                    msg = dtcmsg + icclockmsg + checkcounter;
                    //Check Telletales
                    //Check Gauges:
                    if (msg.Length != 0)
                    {
                        ShowInfo(richTextBox1, msg, Color.Red);
                        // Setnoticecolor(Color.Red);
                        msg = "";
                        icclockmsg = "";
                        dtcmsg = "";
                        checkcounter = "";
                    }
                }
                else
                {
                    break;
                }
            }
        }
        //private void LoadManualInstruction()
        //{
        //    string path = Application.StartupPath + "\\SysConfig";
        //    string json = JsonOperate.GetJson(path, "ManualInstrustion.json");
        //    listManualInstruction = JsonConvert.DeserializeObject<List<ManualInstruction>>(json);
        private void RunTest()
        {
            TestStartTime = DateTime.Now;
            TimePause = TimeSpan.FromMilliseconds(0);
            //获取待测试步骤的总CycleTime
            thTestTime = new Thread(UpdateTestTime);
            //thTestTime.IsBackground = true;
            thTestTime.Start();
            CountTime = 0;

            TestThread_.Function = 0;//打开30电继电器通道
            TestThread_.PowerONOFF = true;

            foreach (var it in ReadyTestInfo)
            {
                CountTime += it.Value.Sum(t => t.cycletime);
            }
            foreach (var item in ReadyTestInfo)
            {
                string typename = item.Key;
                var max = item.Value.Max(t => t.repeat);
                int n = Convert.ToInt32(max);
                for (int i = 0; i < n; i++)
                {
                    foreach (TestStep step in item.Value)
                    {
                        if (IsStop)
                        {
                            IsStop = false;
                            return;
                        }
                        if (i > step.repeat)
                        {
                            continue;
                        }
                        CompletedList.Add(step);
                        dataGridView1.BeginInvoke((MethodInvoker)delegate
                        {
                            this.dataGridView1.DataSource = null;
                            this.dataGridView1.DataSource = CompletedList;
                            this.dataGridView1.Rows[CompletedList.Count - 1].Selected = true;
                            this.dataGridView1.CurrentCell = this.dataGridView1.Rows[CompletedList.Count - 1].Cells[0];
                        });

                        FillStepInfo(step);
                        UpdateTestStepInfo(step);

                        switch (step.modelname)
                        {
                            case "Init Hardware":
                                PowerVoltageControl(step);
                                InitHardware(step);
                                break;
                            case "Sleep mode":
                                PowerVoltageControl(step);
                                SleepMode(step);
                                break;
                            case "Normal mode":
                                PowerVoltageControl(step);
                                Normalmode(step);
                                break;
                            case "Operation mode":
                                PowerVoltageControl(step);
                                OperationMode(step);
                                break;
                        }
                    }
                }
            }
            btnStart.BeginInvoke((MethodInvoker)delegate { btnStart.Enabled = true; });
            btnPause.BeginInvoke((MethodInvoker)delegate { btnPause.Enabled = false; });
            IsTestEnd = true;
            TestThread_.IsTestEnd = true;
            btnStart.BackColor = Color.LimeGreen;

            TestThread_.PowerONOFF = false;
            TestThread_.Function = 0;
        }
        private void PowerVoltageControl(TestStep ts)
        {
            if (ts.voltage < 6)
            {
                if (ts.voltage != 0)
                {
                    PowerControl.PowerSourceControl(1, 6);
                }
                else
                {
                    TestThread_.PowerONOFF = false;
                    TestThread_.Function = 0;
                }
            }
            else
            {
                PowerControl.PowerSourceControl(1, (int)ts.voltage);
                if (TestThread_.PowerONOFF == false)
                {
                    TestThread_.PowerONOFF = true;
                }
            }
        }
        #endregion
    }
}
