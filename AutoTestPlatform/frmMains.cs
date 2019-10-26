using AutoTestDLL.Model;
using AutoTestDLL.Util;
using AutoTestPlatform.InstrumentClusterConfigurations;
using AutoTestPlatform.SysConfig;
using AutoTestPlatform.TemperatureSensorConfiguration;
using AutoTestPlatform.TestSequence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsControlLibrary;

namespace AutoTestPlatform
{
    public partial class frmMains : Form
    {
        public frmMains()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private System.Timers.Timer chartTimer = new System.Timers.Timer();
        private List<AmmeterConfiguration> ammeterList = new List<AmmeterConfiguration>();
        Dictionary<string, TestUnit> DicEquipmentInfo = new Dictionary<string, TestUnit>();
        Dictionary<string, TemperatureControl> DicTemperatureInfo = new Dictionary<string, TemperatureControl>();
        Dictionary<string, PublicElectricControl> DicPublicElectricInfo = new Dictionary<string, PublicElectricControl>();
        List<Color> colors = new List<Color>() {
            Color.FromArgb(230,23,23),Color.FromArgb(246,98,98), Color.FromArgb(154, 61,61),//RED
            Color.FromArgb(115,114,57),Color.FromArgb(150,214,21),Color.FromArgb(193,241,96),//GREEN
            Color.FromArgb(37,92,92),Color.FromArgb(14,138,138),Color.FromArgb(85,215,215),//BLUE
            Color.FromArgb(234,129,41),Color.FromArgb(255,130,26),Color.FromArgb(255,177,113),//YELLOW
        };

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                chartTimer.Interval = 1000;
                chartTimer.Elapsed += chartTimer_Tick;
                

                LoadEquipmentInfo();
                LoadTestUnitControl();
                LoadPublicElectricInfo();
                LoadPublicElectricControl();
                LoadTemperatureInfo();
                LoadTemperatureControl();
                chartTimer.Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        private void LoadEquipmentInfo()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "InstrumentClusterConfiguration.json");
            List<InstrumentClusterConfiguration> temp = JsonConvert.DeserializeObject<List<InstrumentClusterConfiguration>>(json);
            if (temp != null)
            {
                foreach (InstrumentClusterConfiguration equipment in temp)
                {
                    string eq = equipment.InstrumentCluster;
                    TestUnit testUnit = new TestUnit();
                    testUnit.Name = eq;
                    testUnit.Tag = eq;
                    if (!DicEquipmentInfo.ContainsKey(eq))
                    {
                        DicEquipmentInfo.Add(eq, testUnit);
                    }
                }
            }
        }

        private void LoadTemperatureInfo()
        {
            string path = Application.StartupPath + "\\SysConfig";
            string json = JsonOperate.GetJson(path, "TempSensorConfiguration.json");
            List<TempSensorConfiguration> temp = JsonConvert.DeserializeObject<List<TempSensorConfiguration>>(json);
            if (temp != null)
            {
                foreach (TempSensorConfiguration t in temp)
                {
                    string sensorName = t.SensorName;
                    TemperatureControl temperature = new TemperatureControl();
                    temperature.Name = sensorName;
                    temperature.Tag = sensorName;
                    if (!DicTemperatureInfo.ContainsKey(sensorName))
                    {
                        DicTemperatureInfo.Add(sensorName, temperature);
                    }
                }
            }
        }

        private void LoadPublicElectricInfo()
        {
            PublicElectricControl electricControl = new PublicElectricControl();
            electricControl.Name = "electricControl";

            if (!DicPublicElectricInfo.ContainsKey(electricControl.Name))
            {
                DicPublicElectricInfo.Add(electricControl.Name, electricControl);
            }
        }

        private void LoadTestUnitControl()
        {
            int i = 0;
            int x = this.panel1.Width;
            int y = this.panel1.Height;

            int _x = 944;
            int _y = 462;

            foreach (var item in DicEquipmentInfo)
            {
                string eq = item.Key;

                item.Value.Width = _x;
                item.Value.Height = _y;
                item.Value.Location = new System.Drawing.Point((i % 3) * (_x + 10) + 3, (i / 3) * (_y + 20) + 3);
                item.Value.treeView1.BackColor = colors[3];
                panel1.Controls.Add(item.Value);
                i++;
            }
        }

        private void LoadTemperatureControl()
        {
            int i = 0;
            int x = this.panel2.Width;
            int y = this.panel2.Height;

            int _x = x - 22;
            int _y = (y - 20) / 3;

            foreach (var item in DicTemperatureInfo)
            {
                item.Value.Width = _x;
                item.Value.Height = _y;
                item.Value.Location = new System.Drawing.Point((i % 1) * (_x + 10) + 3, (i / 1) * (_y + 20) + 3);
                panel4.Controls.Add(item.Value);
                i++;
            }
        }

        private void LoadPublicElectricControl()
        {
            int i = 0;
            int x = this.panel2.Width;
            int y = this.panel2.Height;

            int _x = x ;
            int _y = (y - 20) / 3;

            foreach (var item in DicPublicElectricInfo)
            {
                string name = item.Key;

                item.Value.Width = _x;
                item.Value.Height = _y;
                item.Value.Location = new System.Drawing.Point((i % 1) * (_x + 10) + 3, (i / 1) * (_y + 20) + 3);
                panel3.Controls.Add(item.Value);
                i++;
            }
        }


        private void chartTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (var it in DicTemperatureInfo)
                {
                    Temperature_humidity temperature = new Temperature_humidity();
                    temperature.now = DateTime.Now;
                    temperature.temperatureValue = new Random().Next(1, 100);
                    temperature.humidtyValue = new Random().Next(1, 50);

                    it.Value.ChartValueFill(temperature);
                }

                foreach (var item in DicEquipmentInfo)
                {
                  //  item.Value.ChartValueFill(new Random().Next(1, 100));
                }

                foreach (var its in DicPublicElectricInfo)
                {
                    CurrentElectricValue value = new CurrentElectricValue();
                    value.now = DateTime.Now;
                    value.currentValue = new Random().Next(1, 1000);
                    value.voltageValue = new Random().Next(0, 13000);

                    its.Value.ChartValueFill(value);
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
            
        }

        #region 电流表样式图列加载（暂未用）
        private void ammeter_Tick(object sender, EventArgs e)
        {
            foreach (AmmeterConfiguration ammeter in ammeterList)
            {
                AmmterFillData(ammeter.ammeterName, new Random().Next(1, 200));
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
            SysLog.CreateAmmeterLog(aGaugeName + "," + value.ToString());
            ((AGauge)this.Controls.Find(aGaugeName, true)[0]).BeginInvoke((MethodInvoker)delegate
            {
                ((AGauge)this.Controls.Find(aGaugeName, true)[0]).Value = value;
                ((AGauge)this.Controls.Find(aGaugeName, true)[0]).GaugeLabels.FindByName("GaugeLabel1").Text = aGaugeName + ":" + value;
            });
        }
        #endregion

        private void TestTypeEdit_Click(object sender, EventArgs e)
        {
            frmTestTypeManager form = new frmTestTypeManager();
            form.ShowDialog();
        }

        private void TestSequenceEdit_Click(object sender, EventArgs e)
        {
            frmTestSequncenManager form = new frmTestSequncenManager();
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

        private void cANConfiguration_Click(object sender, EventArgs e)
        {
            frmCANConfiguration frm = new frmCANConfiguration();
            frm.ShowDialog();
        }

        private void cOMConfiguration_Click(object sender, EventArgs e)
        {
            frmCOMConfiguration frm = new frmCOMConfiguration();
            frm.ShowDialog();
        }

        private void equipmentConfiguration_Click(object sender, EventArgs e)
        {
            frmInstrumentClusterConfiguration frm = new frmInstrumentClusterConfiguration();
            frm.ShowDialog();
        }

        private void equipmentTestInfo_Click(object sender, EventArgs e)
        {
            frmInstrumentClusterTestType frm = new frmInstrumentClusterTestType();
            frm.ShowDialog();
        }
    }
}
