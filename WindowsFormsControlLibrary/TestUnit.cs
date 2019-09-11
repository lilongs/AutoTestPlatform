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

namespace WindowsFormsControlLibrary
{
    public partial class TestUnit: UserControl
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
