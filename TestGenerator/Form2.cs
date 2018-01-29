using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TestGenerator
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "\t\tTestGenerator\t\t\n\tThank you for choosing to work with this program.\n\tAfter entering the question and the available answers(blanck if an open answer)\n\tyou must enter a star (*) at the end or the beginning of the correct answer.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var a = new StreamWriter("started");
            a.WriteLine("=============================================================================");
            a.WriteLine("This program have already been run and you have choosen not to see the opening message again.");
            a.WriteLine("=============================================================================");
            a.Close();
            this.Close();
        }
    }
}
