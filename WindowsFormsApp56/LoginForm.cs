using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp56
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {           
            InitializeComponent();
        }
        // Exit button
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        // "Clear All" button
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            label3.Visible = false;
        }
        // "Log In" button
        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        // When you press "Log In" button, the timer1 will be triggered
        private void timer1_Tick(object sender, EventArgs e)
        {
            
            progressBar1.Visible = true;
            progressBar1.Value = progressBar1.Value + 5;
            label3.Visible = true;
            label3.Text = "Logging in...";
            if(progressBar1.Value == progressBar1.Maximum)
            {
                if ((textBox1.Text == "admin") && (textBox2.Text =="123") )
                {
                    timer1.Enabled = false;
                    progressBar1.Visible = false;
                    progressBar1.Enabled = false;
                    progressBar1.Value = 0;
                    Form1 frm1 = new Form1();
                    this.Hide();
                    frm1.ShowDialog();
                }
                else if (textBox2.Text.Contains(textBox1.Text))
                {
                    progressBar1.Enabled = false;
                    timer1.Enabled = false;
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    label3.Text = "Password can not contain username!";
                    textBox1.Clear();
                    textBox2.Clear();
                }
                else 
                {
                    progressBar1.Enabled = false;
                    timer1.Enabled = false;
                    progressBar1.Visible = false;
                    progressBar1.Value = 0;
                    label3.Text = "Try again!";
                    textBox1.Clear();
                    textBox2.Clear();
                }
            }
        }
        // Help menu
        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Prepared by Kamil Dinleyici,\n         ID:2016513019 \nÇukurova University EEMB");
        }
        // Check box for show/hide password
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }
        // When you press Enter button at password part, "Log In" button will perform
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1.PerformClick();
                e.SuppressKeyPress = true;
            }

        }
        // The help button next to password
        private void button4_MouseHover(object sender, EventArgs e)
        {
            lblHelp.Visible = true;
        }
        // The help button next to password
        private void button4_MouseLeave(object sender, EventArgs e)
        {
            lblHelp.Visible = false;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
