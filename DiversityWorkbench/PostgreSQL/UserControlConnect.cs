using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.PostgreSQL
{
    public partial class UserControlConnect : UserControl
    {
        public UserControlConnect()
        {
            InitializeComponent();
        }

        private void initControl()
        {
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0)
                this.buttonConnect.BackColor = System.Drawing.Color.Red;
            else this.buttonConnect.BackColor = System.Drawing.Color.Transparent;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.PostgreSQL.FormConnect f = new FormConnect();
            f.ShowDialog();
            if(f.DialogResult == DialogResult.OK)
            {
                this.initControl();
            }
        }

    }
}
