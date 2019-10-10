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
            List<TypeList>  temp2 = JsonConvert.DeserializeObject<List<TypeList>>(json2);
            if (temp != null && temp2!=null)
            {
                List<EquipmentTestInfo>  list=temp.Where(t => t.InstrumentCluster == this.Tag.ToString()).ToList();

                foreach (EquipmentTestInfo info in list)
                {
                    result.AddRange(temp2.Where(t => t.typename == info.TypeName || t.parentname==info.TypeName).ToList());
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
            this.groupControl1.Text = this.Tag.ToString() + " current curve";
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Spline;
            series.IsXValueIndexed = true;
            series.XValueType = ChartValueType.DateTime;
            series.Name = this.Tag.ToString() + "CurrentCurve";

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss.fff";
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 10;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            //chart1.Legends[0].Docking = Docking.Top;
        }

        private void InitChart2()
        {
            this.groupControl1.Text = this.Tag.ToString() + " current curve";
            // Zoom into the X axis
            //chart1.ChartAreas[0].AxisX.ScaleView.Zoom(1, 1);
            // Enable range selection and zooming end user interface
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            //将滚动内嵌到坐标轴中
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
            // 设置滚动条的大小
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 10;
            chart1.ChartAreas[0].AxisY.ScrollBar.Size = 10;
            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chart1.ChartAreas[0].AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            chart1.ChartAreas[0].AxisY.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 1;
        }

        public void ChartValueFill(double value)
        {
            if (this.IsHandleCreated)
            {
                chart1.BeginInvoke((MethodInvoker)delegate
                {
                    Series series = chart1.Series[0];
                    if (series.Points.Count > 2000)
                    {
                        series.Points.RemoveAt(0);
                    }
                    series.Points.AddXY(DateTime.Now, value);
                    chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 10;
                });
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
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
                Thread th = new Thread(RunTest);
                th.IsBackground = true;
                th.Start();

                //循环发送BCM、Gateway信息
                IsTestEnd = false;
                Thread th2 = new Thread(Send_BCM_Gateway_CanMessage);
                th2.IsBackground = true;
                th2.Start();

                //循环发送手动指令
                Thread th3 = new Thread(Send_ManualInstruction);
                th3.IsBackground = true;
                th3.Start();

                selectedList.Clear();
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

        private void RunTest()
        {
            TestStartTime = DateTime.Now;
            //获取待测试步骤的总CycleTime
            double CountTime = 0;
            foreach (var it in ReadyTestInfo)
            {
                CountTime+=it.Value.Sum(t => t.cycletime);
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
                        string type=step.typename;
                        switch(step.typename)
                        {
                            case "PTL":
                                //Step1 
                                //Step2
                                break;
                            case "K03":
                                //Step1 
                                //Step2
                                break;
                        }

                        string stepname = step.stepname;
                        string modelname = step.modelname;
                        ShowInfo(richTextBox1, "正在进行：" + typename + "--" + stepname + "测试！",Color.Red);
                        //Actual test steps
                        ShowInfo(richTextBox2, typename+"-"+stepname);
                        //Measuring value
                        ShowInfo(richTextBox3,"OK");
                        //Model
                        ShowInfo(richTextBox4, modelname);

                        TimeSpan sp = DateTime.Now - TestStartTime;
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
                    }
                }
            }
            IsTestEnd = true;
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

        private void Send_BCM_Gateway_CanMessage()
        {
            if (message_List1.Count > 0)
            {
                while (true)
                {
                    //if (!IsTestEnd)
                    if(true)
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
                while (true)
                {
                    //if(!IsTestEnd)
                    if(true)
                    {
                        foreach (ManualInstruction instruct in listManualInstruction)
                        {
                            if (instruct.cycletime == instruct.cyclecount && instruct.enable)
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

    }
}
