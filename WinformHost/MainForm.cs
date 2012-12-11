using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinformHost
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            gravityTester1.Speed = this.trackBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gravityTester1.MakeBaby();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            gravityTester1.Nuke();

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            gravityTester1.GravitationalNotsoConstant = Convert.ToDouble(trackBar2.Value/10);
        }


    }
}
