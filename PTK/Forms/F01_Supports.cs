using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PTK.Forms
{
    public partial class F01_Supports : Form
    {
        public F01_Supports()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Cancel;
            this.CancelButton = button2;
        }

        // loading event
        private void F01_Supports_Load(object sender, EventArgs e)
        {
            this.checkBox1.Checked = Properties.Settings.Default.SupportsCB1;
            this.checkBox2.Checked = Properties.Settings.Default.SupportsCB2;
            this.checkBox3.Checked = Properties.Settings.Default.SupportsCB3;
            this.checkBox4.Checked = Properties.Settings.Default.SupportsCB4;
            this.checkBox5.Checked = Properties.Settings.Default.SupportsCB5;
            this.checkBox6.Checked = Properties.Settings.Default.SupportsCB6;
        }

        private void F01_Supports_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.SupportsCB1 = this.checkBox1.Checked;
            Properties.Settings.Default.SupportsCB2 = this.checkBox2.Checked;
            Properties.Settings.Default.SupportsCB3 = this.checkBox3.Checked;
            Properties.Settings.Default.SupportsCB4 = this.checkBox4.Checked;
            Properties.Settings.Default.SupportsCB5 = this.checkBox5.Checked;
            Properties.Settings.Default.SupportsCB6 = this.checkBox6.Checked;

            Properties.Settings.Default.Save();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {

        }

        // "OK" button
        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) { }
            if (checkBox2.Checked == true) { }
            if (checkBox3.Checked == true) { }
            if (checkBox4.Checked == true) { }
            if (checkBox5.Checked == true) { }
            if (checkBox6.Checked == true) { }

            this.DialogResult = DialogResult.OK;
        }

        // "Cancel" button
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
