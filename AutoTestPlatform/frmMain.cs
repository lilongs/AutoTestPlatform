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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        Dictionary<string, List<TestStep>> ReadyTestInfo = null;
        private void frmMain_Load(object sender, EventArgs e)
        {
            List<TypeList> temp = LoadTypeList();
            if (temp != null)
            {
                list = temp;
                InitTree();
                ExpandTree();
            }
        }

        private List<TypeList> LoadTypeList()
        {
            List<TypeList> temp = new List<TypeList>();
            string path = Application.StartupPath + "\\TestInfo"; ;
            string json = JsonOperate.GetJson(path, "TypeList.json");
            temp=JsonConvert.DeserializeObject<List<TypeList>>(json);
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

        #region 信息显示
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
        #endregion
        Dictionary<string, string> WatchInfo = new Dictionary<string, string>()
        {
            {"dataGridView1","表1" },
            {"dataGridView2","表2" },
            {"dataGridView3","表3" },
            {"dataGridView4","表4" },
            {"dataGridView5","表5" },
            {"dataGridView6","表6" }
        };
        private void DataViewFill(DataGridView gridView,ElectricCurrent ele)
        {
            try
            {
                SysLog.CreateLog(WatchInfo[gridView.Name]+","+ele.electricity);
                gridView.BeginInvoke((MethodInvoker)delegate {
                    List<ElectricCurrent> list = (List<ElectricCurrent>)gridView.DataSource;
                    if (list == null)
                    {
                        list = new List<ElectricCurrent>();
                    }
                    list.Add(ele);
                    if (list.Count > 10)
                    {
                        list.RemoveAt(0);
                    }
                    gridView.DataSource = null;
                    gridView.DataSource = list;
                });                
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
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
                    ReadyTestInfo.Add(tp.typename,testInfo.Where(x => x.typename == tp.typename).ToList());
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
            foreach(var item in ReadyTestInfo)
            {
                string typename = item.Key;

                var max = item.Value.Max(t => t.cycletime);
                int n=Convert.ToInt32(max);
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
                        DataViewFill(dataGridView1, electric);
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
            foreach(TreeNode node in parentNode.Nodes)
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
    }
}
