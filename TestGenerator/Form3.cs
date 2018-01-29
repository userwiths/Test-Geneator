using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestGenerator
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.variants = int.Parse(numericUpDown1.Value.ToString());
            Form1.key = checkBox1.Checked;
            Form1.f_name = textBox1.Text;
            Form1.subject = textBox2.Text;
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
