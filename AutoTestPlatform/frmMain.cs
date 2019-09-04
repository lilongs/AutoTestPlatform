﻿using AutoTestDLL.Model;
using AutoTestDLL.Util;
using AutoTestPlatform.SysConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AutoTestPlatform
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private List<TypeList> list = new List<TypeList>();
        private List<TypeList> selectedList = new List<TypeList>();
        private List<TestStep> testInfo = new List<TestStep>();
        private List<AmmeterConfiguration> ammeterList = new List<AmmeterConfiguration>();

        Dictionary<string, List<TestStep>> ReadyTestInfo = null;
        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                List<TypeList> temp = LoadTypeList();
                if (temp != null)
                {
                    list = temp;
                    InitTree();
                    ExpandTree();
                }
                InitAmmeterConfigurationInfo();
                InitChart();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void InitChart()
        {
            DateTime time = DateTime.Now;
            System.Timers.Timer chartTimer = new System.Timers.Timer();
            chartTimer.Interval = 1000;
            chartTimer.Elapsed += chartTimer_Tick;

            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Spline;
            series.IsXValueIndexed = true;
            series.XValueType = ChartValueType.Time;
            series.Name = "温度曲线";

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            
            chartTimer.Start();
        }

        private void chartTimer_Tick(object sender, EventArgs e)
        {
            ChartValueFill(new Random().Next(1,100));
        }
        
        private void ammeter_Tick(object sender,EventArgs e)
        {
            foreach (AmmeterConfiguration ammeter in ammeterList)
            {
                AmmterFillData(ammeter.ammeterName, new Random().Next(1,200));
            }
        }

        private void ChartValueFill(double value)
        {
            SysLog.CreateTemperatureLog(value.ToString());
            chart1.BeginInvoke((MethodInvoker)delegate
            {
                Series series = chart1.Series[0];
                series.Points.AddXY(DateTime.Now, value);
                chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;
            });
        }

        private List<TypeList> LoadTypeList()
        {
            List<TypeList> temp = new List<TypeList>();
            string path = Application.StartupPath + "\\TestInfo"; ;
            string json = JsonOperate.GetJson(path, "TypeList.json");
            temp = JsonConvert.DeserializeObject<List<TypeList>>(json);
            return temp;
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

        private void InitAmmeterConfigurationInfo()
        {
            System.Timers.Timer ammeterTimer = new System.Timers.Timer();
            ammeterTimer.Interval = 1000;
            ammeterTimer.Elapsed += ammeter_Tick;

            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "AmmeterConfiguration.json");
            ammeterList = JsonConvert.DeserializeObject<List<AmmeterConfiguration>>(json);
            if (ammeterList != null)
            {
                this.panel1.Controls.Clear();
                foreach (AmmeterConfiguration ammeter in ammeterList)
                {
                    string ammeterName = ammeter.ammeterName;
                    LoadAmmeterConfigurationInfo(ammeterName);
                }
            }
            ammeterTimer.Start();
        }

        private void LoadAmmeterConfigurationInfo(string aGaugeName)
        {
            AGauge aGauge1 = new AGauge();
            System.Windows.Forms.AGaugeLabel aGaugeLabel1 = new System.Windows.Forms.AGaugeLabel();
            System.Windows.Forms.AGaugeRange aGaugeRange1 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange2 = new System.Windows.Forms.AGaugeRange();
            System.Windows.Forms.AGaugeRange aGaugeRange3 = new System.Windows.Forms.AGaugeRange();
            // 
            // aGauge1
            // 
            aGauge1.BackColor = System.Drawing.SystemColors.Control;
            aGauge1.BaseArcColor = System.Drawing.Color.Gray;
            aGauge1.BaseArcRadius = 80;
            aGauge1.BaseArcStart = 135;
            aGauge1.BaseArcSweep = 270;
            aGauge1.BaseArcWidth = 2;
            aGauge1.GaugeRanges.Add(aGaugeRange1);
            aGauge1.GaugeRanges.Add(aGaugeRange2);
            aGauge1.GaugeRanges.Add(aGaugeRange3);
            aGauge1.MaxValue = 200F;
            aGauge1.MinValue = 0F;
            aGauge1.Name = aGaugeName;
            aGauge1.NeedleColor1 = System.Windows.Forms.AGaugeNeedleColor.Yellow;
            aGauge1.NeedleColor2 = System.Drawing.Color.Olive;
            aGauge1.NeedleRadius = 80;
            aGauge1.NeedleType = System.Windows.Forms.NeedleType.Advance;
            aGauge1.NeedleWidth = 2;
            aGauge1.ScaleLinesInterColor = System.Drawing.Color.Black;
            aGauge1.ScaleLinesInterInnerRadius = 73;
            aGauge1.ScaleLinesInterOuterRadius = 80;
            aGauge1.ScaleLinesInterWidth = 1;
            aGauge1.ScaleLinesMajorColor = System.Drawing.Color.Black;
            aGauge1.ScaleLinesMajorInnerRadius = 70;
            aGauge1.ScaleLinesMajorOuterRadius = 80;
            aGauge1.ScaleLinesMajorStepValue = 20F;
            aGauge1.ScaleLinesMajorWidth = 2;
            aGauge1.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            aGauge1.ScaleLinesMinorInnerRadius = 75;
            aGauge1.ScaleLinesMinorOuterRadius = 80;
            aGauge1.ScaleLinesMinorTicks = 9;
            aGauge1.ScaleLinesMinorWidth = 1;
            aGauge1.ScaleNumbersColor = System.Drawing.Color.Black;
            aGauge1.ScaleNumbersFormat = null;
            aGauge1.ScaleNumbersRadius = 95;
            aGauge1.ScaleNumbersRotation = 0;
            aGauge1.ScaleNumbersStartScaleLine = 1;
            aGauge1.ScaleNumbersStepScaleLines = 1;
            aGauge1.Size = new System.Drawing.Size(250, 250);
            aGauge1.TabIndex = 0;
            aGauge1.Text = "aGauge1";
            aGauge1.Value = 0F;

            aGaugeLabel1.Color = System.Drawing.SystemColors.WindowText;
            aGaugeLabel1.Font = new System.Drawing.Font("Verdana", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            aGaugeLabel1.Name = "GaugeLabel1";
            aGaugeLabel1.Position = new System.Drawing.Point(70, 150);
            aGaugeLabel1.Text = aGaugeName + ":0";
            aGauge1.GaugeLabels.Add(aGaugeLabel1);
            aGaugeRange1.Color = System.Drawing.Color.Red;
            aGaugeRange1.EndValue = 200F;
            aGaugeRange1.InnerRadius = 70;
            aGaugeRange1.InRange = false;
            aGaugeRange1.Name = "AlertRange";
            aGaugeRange1.OuterRadius = 80;
            aGaugeRange1.StartValue = 160F;
            aGaugeRange2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            aGaugeRange2.EndValue = 160F;
            aGaugeRange2.InnerRadius = 70;
            aGaugeRange2.InRange = false;
            aGaugeRange2.Name = "GaugeRange3";
            aGaugeRange2.OuterRadius = 75;
            aGaugeRange2.StartValue = 0F;
            aGaugeRange3.Color = System.Drawing.Color.Lime;
            aGaugeRange3.EndValue = 160F;
            aGaugeRange3.InnerRadius = 75;
            aGaugeRange3.InRange = false;
            aGaugeRange3.Name = "GaugeRange2";
            aGaugeRange3.OuterRadius = 80;
            aGaugeRange3.StartValue = 0F;


            int x = panel1.Size.Width;
            int y = panel1.Size.Height;

            int x_interval = 10;
            int y_interval = 10;

            int x_max = (x - x_interval) / (250 + x_interval);
            int y_max = (y - y_interval) / (250 + y_interval);
            int n = panel1.Controls.Count;

            int x_n = n % x_max;
            int y_n = n / x_max;

            aGauge1.Location = new System.Drawing.Point(250 * x_n + x_interval, 250 * y_n + y_interval);
            panel1.Controls.Add(aGauge1);
        }

        public void AmmterFillData(string aGaugeName, float value)
        {
            SysLog.CreateAmmeterLog(aGaugeName+","+value.ToString());
            ((AGauge)this.Controls.Find(aGaugeName, true)[0]).BeginInvoke((MethodInvoker)delegate
            {
                ((AGauge)this.Controls.Find(aGaugeName, true)[0]).Value = value;
                ((AGauge)this.Controls.Find(aGaugeName, true)[0]).GaugeLabels.FindByName("GaugeLabel1").Text = aGaugeName + ":" + value;
            });
        }

        public void ShowInfo(string info)
        {
            txtInfo.BeginInvoke((MethodInvoker)delegate
            {
                if (txtInfo.Items.Count > 200)
                {
                    txtInfo.Items.RemoveAt(0);
                }
                txtInfo.Items.Add(info);
            });
        }

        private void TestTypeEdit_Click(object sender, EventArgs e)
        {
            TestSequence.frmTestTypeManager form = new TestSequence.frmTestTypeManager();
            form.ShowDialog();
        }

        private void TestSequenceEdit_Click(object sender, EventArgs e)
        {
            TestSequence.frmTestSequncenManager form = new TestSequence.frmTestSequncenManager();
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
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

        private void RunTest()
        {
            foreach (var item in ReadyTestInfo)
            {
                string typename = item.Key;

                var max = item.Value.Max(t => t.cycletime);
                int n = Convert.ToInt32(max);
                for (int i = 1; i < n; i++)
                {
                    foreach (TestStep step in item.Value)
                    {
                        if (i > step.repeat)
                        {
                            continue;
                        }
                        string stepname = step.stepname;
                        ShowInfo("正在进行：" + typename + "--" + stepname + "测试！");
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

        private void LoadStepInfo()
        {
            string path = Application.StartupPath + "\\TestInfo";
            string json = JsonOperate.GetJson(path, "TestStep.json");
            testInfo = JsonConvert.DeserializeObject<List<TestStep>>(json);
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

        private void ammeterConfiguration_Click(object sender, EventArgs e)
        {
            frmAmmeterConfiguration frm = new frmAmmeterConfiguration();
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.OK)
            {
                InitAmmeterConfigurationInfo();
            }
        }

        private void temperatureConfiguration_Click(object sender, EventArgs e)
        {
            frmTemperatureConfiguration frm = new frmTemperatureConfiguration();
            frm.ShowDialog();
        }
    }
}
