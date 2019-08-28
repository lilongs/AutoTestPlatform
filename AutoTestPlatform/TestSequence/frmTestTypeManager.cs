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
    public partial class frmTestTypeManager : Form
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public frmTestTypeManager()
        {
            InitializeComponent();
        }
        List<TypeList> list = new List<TypeList>();
        private void frmTestTypeManager_Load(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + "\\TestInfo";
                string json = JsonOperate.GetJson(path, "TypeList.json");
                List<TypeList> temp = JsonConvert.DeserializeObject<List<TypeList>>(json);
                if (temp != null)
                {
                    list = temp;
                    InitTree();
                    ExpandTree();
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void ExpandTree()
        {
            foreach (TreeNode nodes in treeView1.Nodes)
            {
                nodes.ExpandAll();
            }
        }

        private void InitTree()
        {
            this.treeView1.Nodes.Clear();
            foreach(TypeList type in list)
            {
                if (type.parentname == "")
                {
                    TreeNode node = new TreeNode();
                    node.Name = type.parentname;
                    node.Text = type.typename;
                    node.Tag = type.typename;
                    this.treeView1.Nodes.Add(node);
                    addChildNode(type,node);
                }
            }
        }

        private void addChildNode(TypeList parenttype,TreeNode parentNode)
        {
            foreach (TypeList type in list)
            {
                if (type.parentname == parenttype.typename)
                {
                    TreeNode node = new TreeNode();
                    node.Name = type.parentname;
                    node.Text = type.typename+","+ type.typeinfo;
                    node.Tag = type.typename;
                    parentNode.Nodes.Add(node);
                    addChildNode(type,node);
                }
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Application.StartupPath + "\\TestInfo";
                var item = list.Where(c => c.typename == txttypename.Text.Trim()).FirstOrDefault();
                if (item != null)
                {
                    item.parentname = txtparentname.Text.Trim();
                    item.typeinfo = txttypeinfo.Text.Trim();
                }
                else
                {
                    TypeList type = new TypeList();
                    type.parentname = txtparentname.Text.Trim();
                    type.typename = txttypename.Text.Trim();
                    type.typeinfo = txttypeinfo.Text.Trim();
                    list.Add(type);
                }               

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "TypeList.json", json);

                InitTree();
                ExpandTree();
                
            }
            catch(Exception ex)
            {
                logger.Error(ex,ex.Message);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = this.treeView1.SelectedNode;
                string typename = node.Tag.ToString();
                for(int i=0;i< list.Count;i++)
                {
                    if (list[i].typename == typename)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\TestInfo";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "TypeList.json", json);

                InitTree();
                ExpandTree();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void quit_Click(object sender, EventArgs e)
        {
           this.Close();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode node = this.treeView1.SelectedNode;
                this.txtparentname.Text = node.Name;
                this.txttypename.Text = node.Tag.ToString();
                string[] arr = node.Text.Split(',');
                if (arr.Length > 1)
                {
                    this.txttypeinfo.Text = arr[1];
                }
                else
                {
                    this.txttypeinfo.Text = string.Empty;
                }
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
    }
}
