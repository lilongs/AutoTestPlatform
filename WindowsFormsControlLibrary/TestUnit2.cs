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

namespace WindowsFormsControlLibrary
{
    public partial class TestUnit2 : UserControl
    {
        public TestUnit2()
        {
            InitializeComponent();
        }
        private System.Timers.Timer testTimer = new System.Timers.Timer();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private List<TypeList> list = new List<TypeList>();
        Dictionary<string, List<TestStep>> ReadyTestInfo = null;
        private List<TypeList> selectedList = new List<TypeList>();
        private List<TestStep> testInfo = new List<TestStep>();
        private DateTime testStartTime = new DateTime();

        private List<TestStep> CompletedList = new List<TestStep>();
        double CountTime = 0d;

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

                testTimer.Interval = 1;
                testTimer.Elapsed += testTimer_Tick;
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
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Spline;
            series.IsXValueIndexed = true;
            series.XValueType = ChartValueType.DateTime;
            series.Name = this.Tag.ToString()+ "CurrentCurve";

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss.fff";
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.Legends[0].Docking= Docking.Top;
        }

        public void ChartValueFill(double value)
        {
            if (this.IsHandleCreated)
            {
                chart1.BeginInvoke((MethodInvoker)delegate
                {
                    Series series = chart1.Series[0];
                    if (series.Points.Count > 200)
                    {
                        series.Points.RemoveAt(0);
                    }
                    series.Points.AddXY(DateTime.Now, value);
                    chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;
                });
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                testStartTime = DateTime.Now;
                testTimer.Start();

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
                Thread th = new Thread(RunTest);
                th.IsBackground = true;
                th.Start();
                selectedList.Clear();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void testTimer_Tick(object sender, EventArgs e)
        {
            //测试持续时间=当前时间-测试开始时间
            TimeSpan sp = DateTime.Now - testStartTime;
            //倒计时时间
            double diff = CountTime - sp.TotalSeconds;
            TimeSpan sp2 = TimeSpan.FromSeconds(diff);

            if (this.IsHandleCreated)
            {
                txttesttime.BeginInvoke((MethodInvoker)delegate
                {
                    //this.txttesttime.Text = 
                    this.txttesttime.Text = string.Format("{0:F2}s", sp.TotalSeconds);
                });

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
            //获取待测试步骤的总CycleTime
            CountTime = 0;
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

                        if (i > step.repeat)
                        {
                            continue;
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

                        #region can消息发送和com信息接收（待完成）
                        #endregion
                        Thread.Sleep(Convert.ToInt32(step.cycletime)*1000);

                       
                    }
                }
            }
            testTimer.Stop();
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
