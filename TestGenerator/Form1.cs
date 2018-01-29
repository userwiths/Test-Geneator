using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestGenerator
{ 
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public static string directory = "";
        public static int number_of_q = 0;
        public static int variants = 0;
        public static int num = 0;
        public static int grade=0;
        public static bool key = true;
        public static string f_name = "";
        public static string subject="";
        public static Node[] test_content=new Node[100];
        public static string[] answ_v = new string[]{"A)","B)","C)","D)","E)","F)","G)","H)","I)","L)"};

        public static void msg(string a)
        {
            MessageBox.Show(a, "Title");
        }
        //public static string[] answers = new string[100];

        public static void Dialog(){
            var dial=new FolderBrowserDialog();
            var dial_res = dial.ShowDialog();
            if (dial_res == DialogResult.OK) 
            {
                directory = dial.SelectedPath;
            }
        }
        
        public static void Randomize<T>(T[] items)
        {
            Random rand = new Random();

            // For each spot in the array, pick
            // a random item to swap into that spot.
            for (int i = 0; i < items.Length - 1; i++)
            {
                int j = rand.Next(i, items.Length-1);
                T temp = items[i];
                items[i] = items[j];
                items[j] = temp;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.ReadOnly = true;
            richTextBox2.ReadOnly = true;
            button1.Visible = false;
            
            if(OnFirstStart())
            {
                Form2 frm = new Form2();
                frm.ShowDialog();
                frm.Focus();
            }
     
         }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            number_of_q = int.Parse(numericUpDown1.Value.ToString());
            richTextBox1.ReadOnly = false;
            richTextBox2.ReadOnly = false;
            label5.Text = "You have " + number_of_q.ToString() + " to enter .";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox2.Text == "") 
            {
                MessageBox.Show("You havent entered a question. Please do so.", "Error!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if(domainUpDown1.SelectedIndex<domainUpDown1.Items.Count){
                test_content[domainUpDown1.SelectedIndex]=new Node(richTextBox2.Text,richTextBox1.Text);
                return;
            }
            
            if (richTextBox1.Text == "") 
            {
                richTextBox1.Text = "__EMPTYLINE__";
            }

            test_content[num]=new Node(richTextBox2.Text, richTextBox1.Text);

            domainUpDown1.Items.Add(num + 1);
           
            num++;

            label5.Text = "You have " + (number_of_q-num).ToString() + " to enter .";
            richTextBox1.Text = "";
            richTextBox2.Text = "";

            if (num == number_of_q) 
            {
                button1.Visible = true;
                button2.Visible = false;
            }
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            int choosen = int.Parse(domainUpDown1.SelectedItem.ToString());
            richTextBox2.Text = test_content[choosen - 1].question;
            foreach (var i in test_content[choosen-1].answer) 
            {
                richTextBox1.Text += i + "\r\n";
            }
            if(!button2.Visible){
                button2.Visible=true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.ShowDialog();

            string key_g = "";
            string[] answ = new string[100];
            Random rand = new Random();
            int n = 0;
            string result = "";
            int count = 1;
            test_content = test_content.Where(x => x != null).ToArray();
            #region SHUFFLE_TEST
           for (int p = 0; p < variants; p++)
            {
                Randomize(test_content);
                foreach(var i in test_content.Where(r => r != null))
                {
                    answ = i.answer.Where(x => x != null).ToArray();//s.OrderBy(n => Guid.NewGuid()).ToArray();
                    n = rand.Next(2, 10);
                    while (n != 0)
                    {
                        Randomize(answ);
                        n--;
                    }
                    #region FIND_KEY
                    result += count.ToString()+".\t"+i.question+"\r\n";
                    for (int m = 0; m < answ.Length; m++)
                    {
                        if (answ.Length == 1) { result += "\r\n\r\n"; break; }
                        if (answ[m] == null) { break; }
                        else
                        {
                            if (answ[m].Contains('*'))
                            {
                                result += answ_v[m] + answ[m].Remove(answ[m].IndexOf('*', 1)) + "\r\n";
                            }
                            else { result += answ_v[m] + answ[m] + "\r\n"; }

                            if (answ[m]== i.key)
                            {
                                key_g +=count.ToString()+"." + answ_v[m]+"\r\n";
                            }
                        }
                    }
                    result += "\r\n";
                    #endregion FIND_KEY
                    count++;
                }
                PrintToFile(p+1,f_name,result,key_g);
                result = "";
                key_g = "";
                count = 1;
            }
            #endregion SHUFFLE_TEST            
        }
        /// <summary>
        /// //////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="a"></param>
        public void SaveToSubject(string a) {
            string content = "";
            if (a == "None") {
                return;
            }
            else
            {
                StreamReader r = new StreamReader("Subjects/"+a + ".rc");
                content = r.ReadToEnd();
                r.Close();  
                StreamWriter wr=new StreamWriter("Subjects/"+a+".rc");
                foreach (Node node in test_content.Where(x => x != null)) {
                    if (content.Contains(node.question)) { continue; }
                    else
                    {
                        wr.WriteLine(node.question);
                        foreach(var c in node.answer){
                            wr.Write("\t"+c);
                        }
                        wr.Write("\r\n");
                    }
                }
                wr.Close();
            }
        }

        public void ReadFromSubject() {
            string line = "";
            Form4 frm = new Form4();
            frm.ShowDialog();
            Random n = new Random();
            StreamReader rd=new StreamReader(".\\Subjects\\" + subject + ".rc");
            for(int i=0;i<number_of_q;i++){
                line=rd.ReadToEnd().Split('\n')[n.Next(0,rd.ReadToEnd().Split('\n').Length)];
                test_content[i]=new Node(line.Split('\t')[0],line.Substring(line.IndexOf("\n"),line.Length-line.IndexOf("\n")));
                
            }
            MessageBox.Show("The question sheet is ready you can view it using the top right control!","Title");
        }
        /// <summary>
        /// ///////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public void PrintToFile(int v, string n,string t, string k) 
        {
            StreamWriter writer = new StreamWriter(directory+"\\"+n+"-Variant-" + v.ToString()+".txt");
            writer.WriteLine(t);
            writer.Close();
            writer = new StreamWriter(directory + "\\" + n + "-Variant-" + v.ToString() + "-Key.txt");
            writer.WriteLine(k);
            writer.Close();
        }

        public bool OnFirstStart()
        {
            //Check if file "started" has value in it
            string str;
            StreamReader a;
            try
            {
                a = new StreamReader("started");
                str = a.ReadToEnd();
                a.Close();
            }
            catch (FileNotFoundException) { return true; }
            return str == "";
        }

        public void SaveData() 
        {
        //Data shall be saved in a text file in format:
        //Question[TAB]Answer1[TAB]Answer2[TAB]Answer3
            var dial = new SaveFileDialog();
            var dial_res = dial.ShowDialog();
            if (dial_res == DialogResult.OK)
            {
                directory = dial.FileName;
            }
            StreamWriter wr = new StreamWriter(directory);
            foreach (var i in test_content.Where(x => x!=null)) {
                wr.Write(i.question);
                foreach (var e in i.answer.Where(x => x != null))
                {
                    if(e=="__EMPTYLINE__"){
                        wr.WriteLine("\n");
                        continue;
                    }
                    wr.Write("\t"+e);
                }
                wr.Write("\n");
            }
            wr.Close();
        }

        public Node[] ReadTest(string a) {
            Node[] rtest = new Node[40];
            int tindex = 0;
            if(a==""){
                var dial=new OpenFileDialog();
                var dial_res = dial.ShowDialog();
                if (dial_res == DialogResult.OK) 
                {
                    directory = dial.FileName;
                }
            }else{
                directory=".\\Subjects\\"+a;
                }
            StreamReader rd = new StreamReader(directory);
            string cont = rd.ReadToEnd();
            string btab;
            int nindex = 0;
            rd.Close();
            string[] lines = cont.Split('\n');
            foreach (var i in lines.Where(x => x != null))
            {
                btab = i.Replace("\t","\n");
                nindex=btab.IndexOf("\n");
                if (nindex > 0)
                {
                    rtest[tindex]= new Node(btab.Substring(0, nindex),btab.Substring(nindex+1,(btab.Length-nindex)-1));
                    tindex += 1;
                }
                domainUpDown1.Items.Add(tindex);
            }
            number_of_q=tindex;
            return rtest;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            test_content=ReadTest("");
            numericUpDown1.Value=number_of_q;
            domainUpDown1.SelectedIndex=0;
            button1.Visible = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
        }
        
        private void createToolStripItem1_Click(object sender, EventArgs e)
        {
            ReadFromSubject();
        }

    }

       public class Node
    {
        public string question = "";
        public string[] answer = new string[15];
        public string key = "";
        public int answer_index = 0;

        public Node(string q, string a)
        {
            int n=0;
            this.answer = a.Split('\n');
            this.question = q;
            this.answer_index = this.answer.Length - 1;
            if(a=="__EMPTYLINE__"){
                this.key="__OPEN_ANSWER__";
                n=1;
            }
            if(n==0){
                foreach (var i in a.Split('\n'))
                {
                    if (i.Contains('*'))
                    {
                        this.key = i;//i.Remove(i.IndexOf('*', 1));
                    }
                    else { continue; }
                }
            }
        }
    }
}
