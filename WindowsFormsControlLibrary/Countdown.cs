using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsControlLibrary
{
    public partial class Countdown : UserControl
    {
        public Countdown()
        {
            InitializeComponent();
        }

        public void SetText(string info)
        {
            this.digitalGauge1.Text = info;
        }
    }
}
