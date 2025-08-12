using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class FormDependentData : Form
    {
        public FormDependentData(string Header, System.Collections.Generic.Dictionary<string, System.Data.DataTable> DependentData)
        {
            InitializeComponent();
            this.labelHeader.Text = Header;
            this.tabControl.TabPages.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, System.Data.DataTable> KV in DependentData)
            {
                System.Windows.Forms.TabPage TP = new TabPage(KV.Key);
                System.Windows.Forms.DataGridView DGV = new DataGridView();
                DGV.DataSource = KV.Value;
                TP.Controls.Add(DGV);
                DGV.Dock = DockStyle.Fill;
                DGV.ReadOnly = true;
                DGV.AllowUserToDeleteRows = false;
                DGV.AllowUserToAddRows = false;
                this.tabControl.TabPages.Add(TP);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
