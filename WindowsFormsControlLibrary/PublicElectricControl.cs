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
    public partial class PublicElectricControl : UserControl
    {
        public PublicElectricControl()
        {
            InitializeComponent();
        }

        private void TemperatureControl_Load(object sender, EventArgs e)
        {
            InitChart();
        }

        private void InitChart()
        {
            userCurve1.SetLeftCurve("A", null, Color.DodgerBlue);
            userCurve1.SetLeftCurve("B", null, Color.DarkOrange);
        }

        public void ChartValueFill(CurrentElectricValue value)
        {
            userCurve1.AddCurveData(
               new string[] { "A", "B" },
               new float[]
               {
                    value.currentValue,
                    value.voltageValue
               }
           );
        }

    }

}
