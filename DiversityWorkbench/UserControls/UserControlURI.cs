using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlURI : UserControl
    {
        public UserControlURI()
        {
            InitializeComponent();
        }

        private void buttonOpenURI_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && !this.textBoxURI.ReadOnly)
            {
                if (f.URL != this.textBoxURI.Text)
                    this.textBoxURI.Text = f.URL;
            }
        }
    }
}
