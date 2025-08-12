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
    public partial class FormRestricted : Form
    {
        public FormRestricted()
        {
            InitializeComponent();
#if DEBUG
            this.textBoxAnswer.Text = "-28,000000000";
#endif
        }

        private void FormRestricted_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                double result = 0.0;
                if (double.TryParse(this.textBoxAnswer.Text, out result))
                {
                    if (result == -28.0)
                        return;
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
