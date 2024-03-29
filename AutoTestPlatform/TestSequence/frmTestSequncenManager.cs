﻿using AutoTestDLL.Model;
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
    public partial class frmTestSequncenManager : Form
    {
        public frmTestSequncenManager()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<TestStep> stepList = new List<TestStep>();

        private void frmTestSequncenManager_Load(object sender, EventArgs e)
        {
            try
            {
                Init();
                LoadStepInfo(combtypename.Text);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void Init()
        {
            string path = Application.StartupPath + "\\TestInfo";
            string json = JsonOperate.GetJson(path, "TypeList.json");
            List<TypeList> temp = JsonConvert.DeserializeObject<List<TypeList>>(json);
            if (temp != null)
            {
                //filter parentname is empty
                for (int i = 0; i < temp.Count; i++)
                {
                    string parentname = temp[i].parentname;
                    if (parentname == "")
                    {
                        temp.RemoveAt(i);
                    }
                }
                List<TypeList> list = temp.Where(x => x.parentname == temp[0].parentname).ToList();
                FitComboAndLabel(list, 0);
            }
        }

        private void FitComboAndLabel(List<TypeList> list, int index)
        {
            if (list != null)
            {
                this.combtypename.DataSource = list;
                this.combtypename.DisplayMember = "typename";
                this.combtypename.ValueMember = "typename";

                this.label_parentname.Text = list[index].parentname;
                this.combtypename.Text = list[index].typename;
            }
        }

        private void LoadStepInfo(string typename)
        {
            string path = Application.StartupPath + "\\TestInfo";
            string json = JsonOperate.GetJson(path, "TestStep.json");
            List<TestStep> temp = JsonConvert.DeserializeObject<List<TestStep>>(json);
            List<TestStep> list = temp.Where(x=>x.typename==typename).ToList();
            if (temp != null)
            {
                stepList = temp;
                this.dataGridView1.DataSource = list.OrderBy(x => Convert.ToInt32(x.stepname.Replace("Step",""))).ToList();
            }
        }

        private void open_Click(object sender, EventArgs e)
        {
            try
            {
                frmSelectTestType frm = new frmSelectTestType();
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.OK)
                {
                    FitComboAndLabel(frm.selectedList, frm.index);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                #region CheckInput
                if (String.IsNullOrEmpty(txtstepname.Text.Trim()))
                {
                    MessageBox.Show("Stepname can't be empty!");
                    return;
                }
                if(!txtstepname.Text.Trim().Contains("Step"))
                {
                    MessageBox.Show("Stepname should be like 'Step1'!");
                    return;
                }
                if (String.IsNullOrEmpty(combtypename.Text))
                {
                    MessageBox.Show("Typename can't be empty!");
                    return;
                }
                #endregion
                double result = -1;

                string path = Application.StartupPath + "\\TestInfo";
                var item= stepList.Where(c => c.stepname == txtstepname.Text.Trim() && c.typename == combtypename.Text).FirstOrDefault();
                if (item != null)
                {
                    item.modelname = txtmodename.Text.Trim();
                    item.voltage = double.TryParse(txtvoltage.Text.Trim(),out result) ? double.Parse(txtvoltage.Text.Trim()):0;
                    item.cycletime = double.TryParse(txtcycletime.Text.Trim(), out result) ? double.Parse(txtcycletime.Text.Trim()) : 0;
                    int msg = -1;
                    item.repeat= int.TryParse(txtrepeat.Text.Trim(), out msg) ? int.Parse(txtrepeat.Text.Trim()) : 1;
                }
                else
                {
                    TestStep step = new TestStep();
                    step.typename = combtypename.Text;
                    step.stepname = txtstepname.Text.Trim();
                    step.modelname = txtmodename.Text.Trim();
                    step.voltage = double.TryParse(txtvoltage.Text.Trim(), out result) ? double.Parse(txtvoltage.Text.Trim()) : 0;
                    step.cycletime = double.TryParse(txtcycletime.Text.Trim(), out result) ? double.Parse(txtcycletime.Text.Trim()) : 0;
                    int msg = -1;
                    step.repeat = int.TryParse(txtrepeat.Text.Trim(), out msg) ? int.Parse(txtrepeat.Text.Trim()) : 1;
                    stepList.Add(step);
                }               

                string json = JsonConvert.SerializeObject(stepList, Formatting.Indented);
                JsonOperate.SaveJson(path, "TestStep.json", json);
                LoadStepInfo(combtypename.Text);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void delete_Click(object sender, EventArgs e)
        {
            try
            {
                string stepname = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["stepname"].Value.ToString();
                string typename = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["typename"].Value.ToString();
                for (int i = 0; i < stepList.Count; i++)
                {
                    if (stepList[i].stepname == stepname && stepList[i].typename == typename)
                    {
                        stepList.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\TestInfo";
                string json = JsonConvert.SerializeObject(stepList, Formatting.Indented);
                JsonOperate.SaveJson(path, "TestStep.json", json);
                LoadStepInfo(combtypename.Text);
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

        private void combtypename_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string typename = combtypename.Text;
                LoadStepInfo(typename);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = this.dataGridView1.CurrentRow;
                this.txtstepname.Text= row.Cells["stepname"].Value.ToString();
                this.txtmodename.Text = row.Cells["modelname"].Value.ToString();
                this.txtvoltage.Text= row.Cells["voltage"].Value.ToString();
                this.txtcycletime.Text = row.Cells["cycletime"].Value.ToString();
                this.txtrepeat.Text = row.Cells["repeat"].Value.ToString();
            }
        }
    }
}
