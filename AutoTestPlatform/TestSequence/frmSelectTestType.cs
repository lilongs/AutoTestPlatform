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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTestPlatform.TestSequence
{
    public partial class frmSelectTestType : Form
    {
        public frmSelectTestType()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<TypeList> list = new List<TypeList>();
        public List<TypeList> selectedList = new List<TypeList>();
        public int index =0;

        private void frmSelectTestType_Load(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + "\\TestInfo";;
                string json = JsonOperate.GetJson(path, "TypeList.json");
                List<TypeList> temp = JsonConvert.DeserializeObject<List<TypeList>>(json);
                if (temp != null)
                {
                    list = temp;
                    InitTree();
                }
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = this.treeView1.SelectedNode;
                if (node.Name=="")
                {
                    selectedList = list.Where(x=>x.parentname==node.Tag.ToString()).ToList();
                }
                else
                {
                    selectedList = list.Where(x => x.parentname == node.Name).ToList();
                    for(int i = 0; i < selectedList.Count; i++)
                    {
                        if (selectedList[i].typename == node.Tag.ToString())
                        {
                            index = i;
                            break;
                        }
                    }
                }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((sender as TreeView) != null)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            btnOK_Click(sender,e);
        }
    }
}
