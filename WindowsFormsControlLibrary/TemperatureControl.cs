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
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Spline;
            series.IsXValueIndexed = true;
            series.XValueType = ChartValueType.Time;
            series.Name = this.Tag.ToString()+"TemperatureCurve";

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss.fff";
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.Legends[0].Docking = Docking.Top;
        }

        public void ChartValueFill(double value)
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
                    series.Points.AddXY(DateTime.Now, value);
                    chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;
                });
            }
        }
    }

}
