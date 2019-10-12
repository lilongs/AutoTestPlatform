using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using AutoTestDLL.Model;

namespace WindowsFormsControlLibrary
{
    public partial class TemperatureControl : UserControl
    {
        public TemperatureControl()
        {
            InitializeComponent();
        }

        private void TemperatureControl_Load(object sender, EventArgs e)
        {
            InitChart();
        }

        private void InitChart()
        {
            this.groupControl1.Text = this.Tag.ToString() + " temperature curve";

            userCurve1.SetLeftCurve("A", null, Color.DodgerBlue);
            userCurve1.SetLeftCurve("B", null, Color.DarkOrange);

        }

        public void ChartValueFill(Temperature_humidity value)
        {
            userCurve1.AddCurveData(
               new string[] { "A","B" },
               new float[]
               {
                    value.temperatureValue,
                    value.humidtyValue
               }
           );
        }
    }

}
