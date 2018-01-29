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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1.subject = comboBox1.SelectedItem.ToString();
            Form1.number_of_q = int.Parse(numericUpDown1.Value.ToString());
            this.Close();
        }
    }
}
