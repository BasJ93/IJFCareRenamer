using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IJFCareRenamer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog outputBrowser = new FolderBrowserDialog();
            if (outputBrowser.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = outputBrowser.SelectedPath;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog inputBrowser = new FolderBrowserDialog();
            if (inputBrowser.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = inputBrowser.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked)
            {
                Program.Rename(textBox1.Text, textBox2.Text, false);
            }
            else
            {
                if(radioButton2.Checked)
                {
                    Program.Rename(textBox1.Text, textBox2.Text, true);
                }
            }
            
        }
    }
}
