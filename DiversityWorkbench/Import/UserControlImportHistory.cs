using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Import
{
    public partial class UserControlImportHistory : UserControl
    {
        public UserControlImportHistory()
        {
            InitializeComponent();
            this.initControl();
        }

        private void initControl()
        {
            string Message = "";
            if (DiversityWorkbench.Import.Import.ImportActive(ref Message))
            {
                this.labelHeader.Text = "Last import has not ended";
                this.labelMessage.Visible = true;
                this.labelMessage.Text = Message;
                this.buttonForceEnd.Visible = true;
            }
            else
            {
                this.labelHeader.Text = "Last import ended";
                this.labelMessage.Visible = false;
                this.buttonForceEnd.Visible = false;
            }
            this.initImportHistory();
        }

        private void buttonForceEnd_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Please ensure, that the last import has ended. Click OK to enter a stop for the last import", "Stop?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                DiversityWorkbench.Import.Import.ForceImportEnd();
                this.initControl();
            }
        }

        private void initImportHistory()
        {
            this.dataGridViewHistory.DataSource = DiversityWorkbench.Import.Import.ImportStatusOverview();
            this.dataGridViewHistory.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            this.dataGridViewHistory.Columns[0].Visible = false;
        }
    }
}
