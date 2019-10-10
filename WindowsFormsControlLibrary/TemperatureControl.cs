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
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Spline;
            series.IsXValueIndexed = true;
            series.XValueType = ChartValueType.Time;
            series.Name = this.Tag.ToString()+"TemperatureCurve";

            Series series2 = chart1.Series[1];
            series2.ChartType = SeriesChartType.Spline;
            series2.IsXValueIndexed = true;
            series2.XValueType = ChartValueType.Time;
            series2.Name = this.Tag.ToString() + "HumidityCurve";


            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss.fff";
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.Legends[0].Docking = Docking.Top;

        }

        public void ChartValueFill(Temperature_humidity value)
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
                    series.Points.AddXY(value.now, value.temperatureValue);
                    chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;

                    Series series2 = chart1.Series[1];
                    if (series2.Points.Count > 2000)
                    {
                        series2.Points.RemoveAt(0);
                    }
                    series2.Points.AddXY(value.now, value.humidtyValue);//电压
                    chart1.ChartAreas[0].AxisX.ScaleView.Position = series2.Points.Count - 5;
                });
            }
        }
    }

}
