﻿using AutoTestDLL.Model;
using AutoTestDLL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTestPlatform.SysConfig
{
    public partial class frmTemperatureConfiguration : Form
    {
        public frmTemperatureConfiguration()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private List<TempSensorConfiguration> list = new List<TempSensorConfiguration>();

        private void frmTestSequncenManager_Load(object sender, EventArgs e)
        {
            try
            {
                LoadInfo();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void LoadInfo()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "TempSensorConfiguration.json");
            List<TempSensorConfiguration> temp = JsonConvert.DeserializeObject<List<TempSensorConfiguration>>(json);
            if (temp != null)
            {
                list = temp;
                this.dataGridView1.DataSource = list;
            }
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                #region CheckInput
                if (String.IsNullOrEmpty(txtSensorName.Text.Trim()))
                {
                    MessageBox.Show("Equipment can't be empty!");
                    return;
                }
                if (String.IsNullOrEmpty(combParamter.Text))
                {
                    MessageBox.Show("Paramter can't be empty!");
                    return;
                }
                #endregion
                string path = Application.StartupPath + "\\SysConfig";
                var item= list.Where(c => c.sensorName == txtSensorName.Text.Trim() && c.paramter == combParamter.Text).FirstOrDefault();
                if (item != null)
                {
                    item.paramter= combParamter.Text;
                    item.value= combValue.Text;
                }
                else
                {
                    TempSensorConfiguration temp = new TempSensorConfiguration();
                    temp.sensorName = txtSensorName.Text.Trim();
                    temp.paramter = combParamter.Text;
                    temp.value = combValue.Text;
                    list.Add(temp);
                }               

                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "TempSensorConfiguration.json", json);
                LoadInfo();
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
                string equipment = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["sensorName"].Value.ToString();
                string paramter = this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["paramter"].Value.ToString();
                string value= this.dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["value"].Value.ToString();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].sensorName == equipment && list[i].paramter == paramter && list[i].value == value)
                    {
                        list.RemoveAt(i);
                    }
                }
                string path = Application.StartupPath + "\\SysConfig";
                string json = JsonConvert.SerializeObject(list, Formatting.Indented);
                JsonOperate.SaveJson(path, "TempSensorConfiguration.json", json);
                LoadInfo();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void quit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataGridViewRow row = this.dataGridView1.CurrentRow;
                    this.txtSensorName.Text = row.Cells["sensorName"].Value == null?"": row.Cells["sensorName"].Value.ToString();
                    this.combParamter.Text= row.Cells["paramter"].Value == null ? "" : row.Cells["paramter"].Value.ToString();
                    this.combValue.Text = row.Cells["value"].Value == null ? "" : row.Cells["value"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void frmAmmeterConfiguration_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void combParamter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = combParamter.Text;
            LoadValue(type);
        }

        private void LoadValue(string type)
        {
            this.combValue.DataSource = null;
            switch (type)
            {
                case "CAN":                  
                    string path = Application.StartupPath + "\\SysConfig";
                    string json = JsonOperate.GetJson(path, "CAN.json");
                    List<CAN> temp = JsonConvert.DeserializeObject<List<CAN>>(json);
                    this.combValue.DataSource = temp;
                    this.combValue.DisplayMember = "channel";
                    break;
                case "COM":
                    string path2 = Application.StartupPath + "\\SysConfig";
                    string json2 = JsonOperate.GetJson(path2, "COM.json");
                    List<COM> temp2 = JsonConvert.DeserializeObject<List<COM>>(json2);
                    this.combValue.DataSource = temp2;
                    this.combValue.DisplayMember = "portName";
                    break;
                default:
                    break;
            }
        }
    }
}
