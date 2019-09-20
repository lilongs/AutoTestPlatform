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
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Spline;
            series.IsXValueIndexed = true;
            series.XValueType = ChartValueType.Time;
            series.Name = "CurrentCurve";

            Series series2 = chart1.Series[1];
            series2.ChartType = SeriesChartType.Spline;
            series2.IsXValueIndexed = true;
            series2.XValueType = ChartValueType.Time;
            series2.Name = "VoltageCurve";

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss.fff";
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.Legends[0].Docking = Docking.Top;
        }

        public void ChartValueFill(CurrentElectricValue value)
        {
            if (this.IsHandleCreated)
            {
                chart1.BeginInvoke((MethodInvoker)delegate
                {
                    Series series = chart1.Series[0];
                    if (series.Points.Count > 200)
                    {
                        series.Points.RemoveAt(0);
                    }
                    series.Points.AddXY(value.now, value.currentValue);//电流
                    chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;

                    Series series2 = chart1.Series[1];
                    if (series2.Points.Count > 200)
                    {
                        series2.Points.RemoveAt(0);
                    }
                    series2.Points.AddXY(value.now, value.voltageValue);//电压
                    chart1.ChartAreas[0].AxisX.ScaleView.Position = series2.Points.Count - 5;
                });
            }
        }

    }

}
