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
    public partial class TestUnit1: UserControl
    {
        public TestUnit1()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private List<TypeList> list = new List<TypeList>();
        Dictionary<string, List<TestStep>> ReadyTestInfo = null;
        private List<TypeList> selectedList = new List<TypeList>();
        private List<TestStep> testInfo = new List<TestStep>();

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
            string json = JsonOperate.GetJson(path, "EquipmentTestInfo.json");
            List<EquipmentTestInfo> temp = JsonConvert.DeserializeObject<List<EquipmentTestInfo>>(json);

            string json2 = JsonOperate.GetJson(path, "TypeList.json");
            List<TypeList>  temp2 = JsonConvert.DeserializeObject<List<TypeList>>(json2);
            if (temp != null && temp2!=null)
            {
                List<EquipmentTestInfo>  list=temp.Where(t => t.equipment == this.Tag.ToString()).ToList();

                foreach (EquipmentTestInfo info in list)
                {
                    result.AddRange(temp2.Where(t => t.typename == info.typename || t.parentname==info.typename).ToList());
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
                if(!String.IsNullOrEmpty(richTextBox1.Text))
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
            foreach (var item in ReadyTestInfo)
            {
                string typename = item.Key;

                var max = item.Value.Max(t => t.repeat);
                int n = Convert.ToInt32(max);
                for (int i = 0; i < n; i++)
                {
                    foreach (TestStep step in item.Value)
                    {
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
                        ShowInfo("正在进行：" + typename + "--" + stepname + "测试！",Color.Red);
                        ElectricCurrent electric = new ElectricCurrent();
                        electric.time = DateTime.Now.ToShortTimeString();
                        electric.electricity = "123";

                        #region can消息发送和com信息接收（待完成）
                        #endregion
                        Thread.Sleep(Convert.ToInt32(step.cycletime));
                    }
                }
            }
        }

        public void ShowInfo(string info, Color color)
        {
            richTextBox1.BeginInvoke((MethodInvoker)delegate
            {
                richTextBox1.SelectionStart = richTextBox1.TextLength;
                richTextBox1.SelectionLength = 0;
                richTextBox1.SelectionColor = color;
                if (String.IsNullOrEmpty(richTextBox1.Text))
                    richTextBox1.AppendText(info);
                else
                    richTextBox1.AppendText(Environment.NewLine + info);
                richTextBox1.SelectionColor = richTextBox1.ForeColor;

                richTextBox1.ScrollToCaret();
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
