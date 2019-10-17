﻿using System;
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

namespace WindowsFormsControlLibrary
{
    public partial class TestUnit : UserControl
    {
        public TestUnit()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private List<TypeList> list = new List<TypeList>();
        Dictionary<string, List<TestStep>> ReadyTestInfo = null;
        private List<TypeList> selectedList = new List<TypeList>();
        private List<TestStep> testInfo = new List<TestStep>();
        private DateTime testStartTime = new DateTime();

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
                LoadManualInstruction();
                this.groupControl3.Text = this.Tag.ToString() + " step info:";
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
            userCurve1.SetLeftCurve("A", null, Color.DodgerBlue);
        }

        public void ChartValueFill(float value)
        {
            userCurve1.AddCurveData(
               new string[] { "A" },
               new float[]
               {
                    value
               }
           );
        }
        TestThread1 TestThread_ = new TestThread1();
        Thread th;
        Thread th2;
        Thread th3;
        Thread th4;
        Thread th5;
        Thread thTestTime;
        PCBcontrol PB = new PCBcontrol();
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            btnPause.Enabled = true;
            TestThread_.MeterSourceName = GetPowerMeterName();
            if (TestThread_.MeterSourceName.Length == 0)
            {
                //  MessageBox.Show("Error Meter Source Name");
                MessageBox.Show(TestThread_.MeterSourceName);
            }
            try
            {
                testStartTime = DateTime.Now;

                CompletedList.Clear();
                this.dataGridView1.DataSource = null;
                if (!String.IsNullOrEmpty(richTextBox1.Text))
                {
                    richTextBox1.Text = string.Empty;
                }
                foreach (TreeNode node in treeView1.Nodes)
                {
                    GetAllSelectedNode(node);
                }
                LoadStepInfo();
                ReadyTestInfo = new Dictionary<string, List<TestStep>>();
                foreach (TypeList tp in selectedList)
                {
                    ReadyTestInfo.Add(tp.typename, testInfo.Where(x => x.typename == tp.typename).ToList());
                }
                //测试步骤线程
                th = new Thread(RunTest);
                th.IsBackground = true;
                th.Start();

                //循环发送BCM、Gateway信息
                IsTestEnd = false;
                th2 = new Thread(Send_BCM_Gateway_CanMessage);
                th2.IsBackground = true;
                th2.Start();
                //循环发送手动指令
                th3 = new Thread(Send_ManualInstruction);
                th3.IsBackground = true;
                th3.Start();
                //电流表
                // TestThread_.MeterFileName = this.Tag.ToString() + "_Current.txt";
                TestThread_.MeterFileRecordStep = Convert.ToInt32(GetAppConfig("MeterFileRecordStep"));
                th4 = new Thread(new ParameterizedThreadStart(TestThread_.MeasureMeterCurrent));
                th4.IsBackground = true;
                th4.Start(this);

                th5 = new Thread(TestThread_.PCBcontrol);
                th5.IsBackground = true;
                th5.Start();

                selectedList.Clear();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private DateTime PauseTime;
        private DateTime ResumeTime;
        private void btnPause_Click(object sender, EventArgs e)
        {
            try
            {
                PauseTime = DateTime.Now;
                ThreadState Nowstate = th.ThreadState;
                ThreadState TestTimestate = thTestTime.ThreadState;

                if (Nowstate == ThreadState.Running || Nowstate == ThreadState.WaitSleepJoin || Nowstate==ThreadState.Background)
                {
                    //Pause the thread
                    th.Suspend();
                    //Disable the Pause button
                    btnPause.Enabled = false;
                    //Enable the resume button
                    btnResume.Enabled = true;
                }

                if (TestTimestate == ThreadState.Running || TestTimestate == ThreadState.WaitSleepJoin || TestTimestate == ThreadState.Background)
                {
                    //Pause the thread
                    thTestTime.Suspend();
                    //Disable the Pause button
                    btnPause.Enabled = false;
                    //Enable the resume button
                    btnResume.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            try
            {
                ResumeTime = DateTime.Now;
                IsResume = true;
                if (th.ThreadState == ThreadState.SuspendRequested || th.ThreadState == ThreadState.Suspended)
                {
                    th.Resume();
                    btnResume.Enabled = false;
                    btnPause.Enabled = true;
                }

                if (thTestTime.ThreadState == ThreadState.SuspendRequested || thTestTime.ThreadState == ThreadState.Suspended)
                {
                    thTestTime.Resume();
                    btnResume.Enabled = false;
                    btnPause.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                IsStop = true;
                TestThread_.IsStop = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
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

        double CountTime = 0;
        private void RunTest()
        {
            TestStartTime = DateTime.Now;
            //获取待测试步骤的总CycleTime
            thTestTime = new Thread(UpdateTestTime);
            //thTestTime.IsBackground = true;
            thTestTime.Start();

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
                        string type = step.typename;
                        switch (step.typename)
                        {
                            case "PTL":
                                PTLTestStep(step);
                                break;
                            case "K03":

                                break;
                        }

                        //string stepname = step.stepname;
                        //string modelname = step.modelname;
                        //ShowInfo(richTextBox1, "正在进行：" + typename + "--" + stepname + "测试！", Color.Red);
                        ////Actual test steps
                        //ShowInfo(richTextBox2, typename + "-" + stepname);
                        ////Measuring value
                        //ShowInfo(richTextBox3, "OK");
                        ////Model
                        //ShowInfo(richTextBox4, modelname);

                        //TimeSpan sp = DateTime.Now - TestStartTime;
                        ////测试持续时间
                        //txttesttime.BeginInvoke((MethodInvoker)delegate
                        //{
                        //    this.txttesttime.Text = string.Format("{0:F2}s", sp.TotalSeconds);
                        //});
                        ////测试倒计时
                        //double diff = CountTime - sp.TotalSeconds;
                        //TimeSpan sp2 = TimeSpan.FromSeconds(diff);
                        //countdown1.BeginInvoke((MethodInvoker)delegate
                        //{
                        //    if (diff <= 0)
                        //    {
                        //        countdown1.SetText(string.Format("{0:D2}D{1:D2}H{2:D2}M{3:D2}S", 0, 0, 0, 0));
                        //    }
                        //    else
                        //    {
                        //        countdown1.SetText(string.Format("{0:D2}D{1:D2}H{2:D2}M{3:D2}S", sp2.Days, sp2.Hours, sp2.Minutes, sp2.Seconds));
                        //    }
                        //});

                    }
                }
            }
            btnStart.BeginInvoke((MethodInvoker)delegate { btnStart.Enabled = true; });
            btnPause.BeginInvoke((MethodInvoker)delegate { btnPause.Enabled = false; });

            //IsTestEnd = true;
            //TestThread_.IsTestEnd = true;
        }
        KvaserCommunication DIDV = new KvaserCommunication();
        string FazitString = "";
        private void InitHardware()
        {
            //Read HW version
            //string msg = "";
            //DIDV.CANmsgSend(msg);
            //byte[] received1 = DIDV.CANmsgReceived();
            //string HW=DIDV.HexstringToASCII(DIDV.ByteToHex(received1));
            string HW = "123";
            HWversion.BeginInvoke((MethodInvoker)delegate
            {
                this.HWversion.Text = HW;
            });
            // SW version
            //msg = "";
            //DIDV.CANmsgSend(msg);
            //byte[] received2 = DIDV.CANmsgReceived();
            //string SW = DIDV.HexstringToASCII(DIDV.ByteToHex(received2));
            string SW = "123";
            SWversion.BeginInvoke((MethodInvoker)delegate
            {
                this.SWversion.Text = SW;
            });
            //Fazit
            //msg = "";
            //DIDV.CANmsgSend(msg);
            //byte[] received3 = DIDV.CANmsgReceived();
            //string Fazit1 = DIDV.HexstringToASCII(DIDV.ByteToHex(received3));
            string Fazit1 = "123";
            FazitString = Fazit1;
            Fazit.BeginInvoke((MethodInvoker)delegate
            {
                this.Fazit.Text = Fazit1;
            });
            // part number
            //msg = "";
            //DIDV.CANmsgSend(msg);
            //byte[] received4 = DIDV.CANmsgReceived();
            //string PartNum = DIDV.HexstringToASCII(DIDV.ByteToHex(received4));
            string PartNum = "123";
            partnumber.BeginInvoke((MethodInvoker)delegate
            {
                this.partnumber.Text = PartNum;
            });
            //serial number
            //msg = "";
            //DIDV.CANmsgSend(msg);
            //byte[] received5 = DIDV.CANmsgReceived();
            //string SerialNum = DIDV.HexstringToASCII(DIDV.ByteToHex(received5));
            string SerialNum = "123";
            Serialnumber.BeginInvoke((MethodInvoker)delegate
            {
                this.Serialnumber.Text = SerialNum;
            });
        }
        private void SleepMode(double powerval, string step)
        {
            //enter sleep mode
            int cnt = 0;
            string msg = "";
            string compearestr = "";
            DIDV.CANmsgSend(msg);
            byte[] received1 = DIDV.CANmsgReceived();
            while (DIDV.Compare(DIDV.ByteToHex(received1), compearestr, 8) == 1 && cnt < 10)
            {
                DIDV.CANmsgSend(msg);
                received1 = DIDV.CANmsgReceived();
                Thread.Sleep(100);
                cnt++;
            }
            if (DIDV.Compare(DIDV.ByteToHex(received1), compearestr, 8) == 1)
            {
                MessageBox.Show("Enter Sleep Mode Error..");
                return;
            }
            string fazittemp = "";
            string Datesandhour = DateTime.Now.ToString("dd_MM_yyyy");
            string Datesandhour1 = DateTime.Now.ToString("_hh_mm_ss_");
            if (FazitString.Length != 0)
            {
                fazittemp = FazitString;
            }
            else fazittemp = "0000000000";
            TestThread_.PowerSourceVal = powerval;
            TestThread_.MeterFileName = step + "_SleepMode" + Datesandhour1 + ".txt";
            TestThread_.MeterFilePath = GetAppConfig("MeterFilePath") + fazittemp + "_" + Datesandhour + "\\";
            TestThread_.MeterRange = 0.01;
            TestThread_.MeterResolution = 0.0001;
            TestThread_.MeasureMetertSwitch = true;
        }

        private void OperationMode(double powerval, string step)
        {
            string fazittemp = "";
            if (FazitString.Length != 0)
            {
                fazittemp = FazitString;
            }
            else fazittemp = "0000000000";
            TestThread_.PowerSourceVal = powerval;
            string Datesandhour = DateTime.Now.ToString("dd_MM_yyyy");
            string Datesandhour1 = DateTime.Now.ToString("_hh_mm_ss_");
            TestThread_.MeterFileName = step + "_OperationMode" + Datesandhour1 + ".txt";
            TestThread_.MeterFilePath = GetAppConfig("MeterFilePath") + fazittemp + "_" + Datesandhour + "\\";
            TestThread_.MeterRange = 10;
            TestThread_.MeterResolution = 0.0001;
            TestThread_.MeasureMetertSwitch = true;
            TestThread_.Channal = Convert.ToByte(this.Tag.ToString().Substring(2, 1));
            TestThread_.PowerONOFF = false;
            TestThread_.Function = 0;
            Thread.Sleep(3000);
            TestThread_.PowerONOFF = true;
            TestThread_.Function = 0;
        }
        private void FunctionTest(double powerval, string step)
        {
            string fazittemp = "";
            if (FazitString.Length != 0)
            {
                fazittemp = FazitString;
            }
            else fazittemp = "0000000000";
            TestThread_.PowerSourceVal = powerval;
            string Datesandhour = DateTime.Now.ToString("dd_MM_yyyy");
            string Datesandhour1 = DateTime.Now.ToString("_hh_mm_ss_");
            TestThread_.MeterFileName = step + "_OperationMode" + Datesandhour1 + ".txt";
            TestThread_.MeterFilePath = GetAppConfig("MeterFilePath") + fazittemp + "_" + Datesandhour + "\\";
            TestThread_.MeterRange = 10;
            TestThread_.MeterResolution = 0.0001;
            TestThread_.MeasureMetertSwitch = true;
            //Check IC clock time;
            //Check Reset counter
            //Check Telletales
            //Check Gauges:
        }

        
        TimeSpan TimePause;
        bool IsResume = false;
        private void UpdateTestTime()
        {
            while (true)
            {
                if(IsTestEnd||IsStop)
                {
                    break;
                }

                if (IsResume)
                {
                    TimePause = TimePause + (ResumeTime - PauseTime);
                    IsResume=false;
                }

                TimeSpan sp = (DateTime.Now - TestStartTime)-(TimePause);
                //测试持续时间
                txttesttime.BeginInvoke((MethodInvoker)delegate
                {
                    this.txttesttime.Text = string.Format("{0:F2}s", sp.TotalSeconds);
                });
                //测试倒计时
                double diff = CountTime - sp.TotalSeconds;
                TimeSpan sp2 = TimeSpan.FromSeconds(diff);
                countdown1.BeginInvoke((MethodInvoker)delegate
                {
                    if (diff <= 0)
                    {
                        countdown1.SetText(string.Format("{0:D2}D{1:D2}H{2:D2}M{3:D2}S", 0, 0, 0, 0));
                    }
                    else
                    {
                        countdown1.SetText(string.Format("{0:D2}D{1:D2}H{2:D2}M{3:D2}S", sp2.Days, sp2.Hours, sp2.Minutes, sp2.Seconds));
                    }
                });
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

        private void PTLTestStep(TestStep step)
        {
            UpdateTestStepInfo(step);

            DateTime centuryBegin = DateTime.Now;
          
            TimeSpan elapsedSpan = new TimeSpan();
            if (step.stepname == "Step1")
            {
                InitHardware();
               
                while (true)
                {
                    DateTime currentDate = DateTime.Now;             
                    elapsedSpan = currentDate - centuryBegin;
                    if(elapsedSpan.TotalSeconds >= step.cycletime)
                    {
                        break;
                    }
                }
            }
            else if (step.stepname == "Step2")
            {
             //   SleepMode(14.0, step.stepname);
                while (true)
                {
                    DateTime currentDate = DateTime.Now;
                    elapsedSpan = currentDate - centuryBegin;
                    if (elapsedSpan.TotalSeconds >= step.cycletime)
                    {
                        break;
                    }
                }
            }
            else if (step.stepname == "Step3")
            {
              //  OperationMode(14.0, step.stepname);
                while (true)
                {
                    DateTime currentDate = DateTime.Now;
                    elapsedSpan = currentDate - centuryBegin;
                    if (elapsedSpan.TotalSeconds >= step.cycletime)
                    {
                        break;
                    }
                }
            }
            else if (step.stepname == "Step4")
            {
                while (true)
                {
                    DateTime currentDate = DateTime.Now;
                    elapsedSpan = currentDate - centuryBegin;
                    if (elapsedSpan.TotalSeconds >= step.cycletime)
                    {
                        break;
                    }
                }
            }
            else if (step.stepname == "Step5")
            {
                while (true)
                {
                    DateTime currentDate = DateTime.Now;
                    elapsedSpan = currentDate - centuryBegin;
                    if (elapsedSpan.TotalSeconds >= step.cycletime)
                    {
                        break;
                    }
                }
            }
            else if (step.stepname == "Step6")
            {
                while (true)
                {
                    DateTime currentDate = DateTime.Now;
                    elapsedSpan = currentDate - centuryBegin;
                    if (elapsedSpan.TotalSeconds >= step.cycletime)
                    {
                        break;
                    }
                }
            }
            else if (step.stepname == "Step7")
            {
                while (true)
                {
                    DateTime currentDate = DateTime.Now;
                    elapsedSpan = currentDate - centuryBegin;
                    if (elapsedSpan.TotalSeconds >= step.cycletime)
                    {
                        break;
                    }
                }
            }
            else if (step.stepname == "Step8")
            {
                while (true)
                {
                    DateTime currentDate = DateTime.Now;
                    elapsedSpan = currentDate - centuryBegin;
                    if (elapsedSpan.TotalSeconds >= step.cycletime)
                    {
                        break;
                    }
                }
            }
        }

        KvaserDbcMessage dBCCan = new KvaserDbcMessage();
        List<AutoTestDLL.Model.Message> message_List1 = new List<AutoTestDLL.Model.Message>();
        List<AutoTestDLL.Model.Message> message_List2 = new List<AutoTestDLL.Model.Message>();
        bool IsTestEnd = false;
        private List<InstrumentClusterConfiguration> LoadICInfo()
        {
            List<InstrumentClusterConfiguration> temp = new List<InstrumentClusterConfiguration>();
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "InstrumentClusterConfiguration.json");
            temp = JsonConvert.DeserializeObject<List<InstrumentClusterConfiguration>>(json);
            return temp;
        }
        private List<PowerMeter>LoadPowerInfo()
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
            message_List1 = message_List1.Where(x => (x.name != "Airbag_01" && x.name != "Dimmung_01" && x.name != "ESP_24" && x.name != "Klemmen_Status_01" && x.name != "LH_EPS_01" && x.name != "RKA_01" && x.name != "ESP_20")).ToList();

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
        }
        public bool IsStop = false;
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
                                dBCCan.sendMsg(message.dlc,signalList);
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

        List<ManualInstruction> listManualInstruction = new List<ManualInstruction>();
        private void LoadManualInstruction()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "ManualInstrustion.json");
            listManualInstruction = JsonConvert.DeserializeObject<List<ManualInstruction>>(json);
        }

        private void Send_ManualInstruction()
        {
            if (listManualInstruction.Count > 0)
            {
                while (!IsStop)
                {
                    if(!IsTestEnd)
                    {
                        foreach (ManualInstruction instruct in listManualInstruction)
                        {    //&& instruct.enable
                            if (instruct.cycletime == instruct.cyclecount )
                            {
                                int id = Convert.ToInt32(instruct.id,16);
                                int dlc = instruct.dlc;
                                byte[] data= Sys.HexStringToBytes(instruct.data);

                                dBCCan.send(id,dlc,data,0);
                                instruct.cyclecount = 0;
                            }
                            else
                            {
                                instruct.cyclecount += 10;
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

        private void FillStepInfo(TestStep step)
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
                this.txtvoltage.Text = step.voltage.ToString();
            });
            txtcycletime.BeginInvoke((MethodInvoker)delegate
            {
                this.txtcycletime.Text = step.cycletime.ToString();
            });
        }

        public void ShowInfo(RichTextBox richTextBox, string info, Color color)
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

        public void ShowInfo(RichTextBox richTextBox, string info)
        {
            richTextBox.BeginInvoke((MethodInvoker)delegate
            {
                richTextBox.Text = info;
            });
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
    }
}
