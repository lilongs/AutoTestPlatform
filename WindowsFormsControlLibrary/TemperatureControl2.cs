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
    public partial class TemperatureControl2 : UserControl
    {
        public TemperatureControl2()
        {
            InitializeComponent();
        }

        private void TemperatureControl_Load(object sender, EventArgs e)
        {
        }

        public void ChartValueFill(Temperature_humidity value)
        {
            if (this.IsHandleCreated)
            {
                label2.BeginInvoke((MethodInvoker)delegate
                {
                    this.label2.Text = value.temperatureValue.ToString()+" ℃";
                });
                label4.BeginInvoke((MethodInvoker)delegate
                {
                    this.label4.Text = value.humidtyValue.ToString()+ " %RH";
                });
            }
        }
    }

}
