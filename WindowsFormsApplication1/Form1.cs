using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        string path = Properties.Settings.Default.path;

        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            button2.Enabled = false;
            textBox1.Text = Properties.Settings.Default.username;
            textBox2.Text = Properties.Settings.Default.path;
        }

        struct DataParameter
        {
            public int Delay;
        }

        private DataParameter _inputparameter;

        private void button1_Click(object sender, EventArgs e)
        {
            if (path != null)
            {
                if (!backgroundWorker1.IsBusy)
                {
                    _inputparameter.Delay = 1000;
                    backgroundWorker1.RunWorkerAsync(_inputparameter);
                }

                button1.Enabled = false;
                button2.Enabled = true;
                button5.Enabled = false;
            }      
        }

        private void button2_Click(object sender, EventArgs e)
        {       
            backgroundWorker1.CancelAsync();
            button1.Enabled = true;
            button2.Enabled = false;
            button5.Enabled = true;
        }

        private void locationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
                path = path + "\\output.txt";
                Properties.Settings.Default.path = path;
                Properties.Settings.Default.Save();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.username = textBox1.Text;
            Properties.Settings.Default.Save();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label2.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
     
            label4.Visible = true;
            label3.Visible = true;
            button1.Visible = true;
            button2.Visible = true;
            button5.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            button2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            button5.Enabled = false;

            label1.Visible = true;
            label2.Visible = true;
            textBox1.Visible = true;
            textBox2.Visible = true;
            button3.Visible = true;
            button4.Visible = true;
        }

        private void textBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                path = folderBrowserDialog1.SelectedPath;
                path = path + "\\output.txt";
                Properties.Settings.Default.path = path;
                textBox2.Text = path;
            }
        }

        private void label3_TextChanged(object sender, EventArgs e)
        {
            while ((label3.Size.Width < System.Windows.Forms.TextRenderer.MeasureText(label3.Text, new Font(label3.Font.FontFamily, label3.Font.Size, label3.Font.Style)).Width) && (label3.Size.Width != 300))
            {
                int width = System.Windows.Forms.TextRenderer.MeasureText(label3.Text, new Font(label3.Font.FontFamily, label3.Font.Size, label3.Font.Style)).Width;
                if(width > 300)
                {
                    label3.Size = new Size(300, 20);
                }
                else
                {
                    label3.Size = new Size(width, 20);
                }
            }

            int newwidth = label3.Size.Width;

            if (newwidth < 300)
            {
                int newxleftpadding = ((320 - newwidth) / 2) - 10;
                label3.Location = new Point(
                    newxleftpadding,
                    103
                    );
            }

            else {

                label3.Location = new Point(
                    10,
                    103
                    );
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            int delay = ((DataParameter)e.Argument).Delay;
            e.Result = LastFMApiClient.GetNowPlaying(Properties.Settings.Default.username);                
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _inputparameter.Delay = 1000;
            string test = (string)e.Result;
            label3.Text = test.ToString();
            System.IO.File.WriteAllText(@path, test.ToString());
            if (button2.Enabled == true)
            {
                backgroundWorker1.RunWorkerAsync(_inputparameter);
            }
            if (button2.Enabled == false)
            {
                label3.Text = "Not Listening";
                System.IO.File.WriteAllText(@path, "No song currently playing.");
            }
        }
    }
}

