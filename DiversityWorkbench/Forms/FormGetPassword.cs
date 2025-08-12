using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetPassword : Form
    {
        public FormGetPassword(string UserName)
        {
            InitializeComponent();
            this.labelUser.Text = UserName;
            this.textBoxPassword.UseSystemPasswordChar = true;
        }

        private void checkBoxShowSigns_Click(object sender, EventArgs e)
        {
            if (this.checkBoxShowSigns.Checked)
                this.textBoxPassword.UseSystemPasswordChar = false;
            else this.textBoxPassword.UseSystemPasswordChar = true;
        }

        public string Password() { return this.textBoxPassword.Text; }
    }
}
