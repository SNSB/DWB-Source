using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.PostgreSQL
{
    public partial class FormCopyDatabase : Form
    {
        public FormCopyDatabase()
        {
            InitializeComponent();
            this.initForm();
        }

        private void initForm()
        {
            if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
            {
                this.labelDatabaseOri.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                this.textBoxNameOfDatabaseCopy.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "_Copy";
                
            }
        }

        private void buttonCreateCopy_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT count(*) FROM pg_database WHERE datname = '" + this.textBoxNameOfDatabaseCopy.Text + "'";
                string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                if (Result != "0")
                {
                    System.Windows.Forms.MessageBox.Show("There is allready a database with the name " + this.textBoxNameOfDatabaseCopy.Text);
                    return;
                }

                string Message = "";
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null) {
                    bool OK = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().CreateCopy(this.textBoxNameOfDatabaseCopy.Text, "postgres", this.checkBoxIncludeData.Checked, ref Message);
                    if (OK)
                    {
                        System.Windows.Forms.MessageBox.Show("Database copy created");
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Creation of copy failed:\r\n" + Message);
                } 
                else
                    System.Windows.Forms.MessageBox.Show("Creation of copy failed:\r\n" + Message);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                System.Windows.Forms.MessageBox.Show("Error: Creating Copy failed: " + ex.Message);
            }
        }
    }
}
