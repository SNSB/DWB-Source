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
            this.labelDatabaseOri.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
            this.textBoxNameOfDatabaseCopy.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "_Copy";
            this.setPostgresApplicationDirectoryVisibility();
        }

        private void buttonCreateCopy_Click(object sender, EventArgs e)
        {
            if (!this.checkBoxIncludeData.Checked && this.textBoxPostgresApplicationDirectory.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("The directory for the Postgres applications is missing");
                return;
            }
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
                bool OK = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().CreateCopy(this.textBoxNameOfDatabaseCopy.Text, "postgres", this.checkBoxIncludeData.Checked, this.textBoxPostgresApplicationDirectory.Text, ref Message);
                if (OK)
                {
                    System.Windows.Forms.MessageBox.Show("Database copy created");
                }
                else
                    System.Windows.Forms.MessageBox.Show("Creation of copy failed:\r\n" + Message);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void checkBoxIncludeData_CheckedChanged(object sender, EventArgs e)
        {
            this.setPostgresApplicationDirectoryVisibility();
        }

        private void setPostgresApplicationDirectoryVisibility()
        {
            this.buttonPostgresApplicationDirectory.Visible = !this.checkBoxIncludeData.Checked;
            this.textBoxPostgresApplicationDirectory.Visible = !this.checkBoxIncludeData.Checked;
            this.labelPostgresApplicationDirectory.Visible = !this.checkBoxIncludeData.Checked;
            if (!this.checkBoxIncludeData.Checked && this.textBoxPostgresApplicationDirectory.Text.Length == 0)
            {
                if (this.textBoxPostgresApplicationDirectory.Text.Length == 0)
                    this.textBoxPostgresApplicationDirectory.Text = this.GetPostgresApplicationDirectory();
                //string WorkingDirectory = @"C:\Program Files\PostgreSQL\9.4\bin";
                //System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(WorkingDirectory);
                //if (Dir.Exists)
                //{

                //}
            }
        }

        private void buttonPostgresApplicationDirectory_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.ShowDialog();
            if (this.folderBrowserDialog.SelectedPath.Length > 0)
            {
                System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(this.folderBrowserDialog.SelectedPath);
                if (this.DirectoryContainsPostgresApplications(Dir))
                    this.textBoxPostgresApplicationDirectory.Text = Dir.FullName;
                else
                {
                    System.Windows.Forms.MessageBox.Show("The selected directory does not contain the applications pg_dump.exe and psql.exe necessary to copy the database");
                }
           }
        }

        private bool _GetPostgresApplicationDirectoryFailed = false;
        private string GetPostgresApplicationDirectory()
        {
            if (this._GetPostgresApplicationDirectoryFailed)
                return "";
            string WorkingDirectory = @"C:\";
            bool DirectoryFound = false;
            try
            {
                System.IO.DirectoryInfo DirRoot = new System.IO.DirectoryInfo(WorkingDirectory);
                if (DirRoot.Exists)
                {
                    foreach (System.IO.DirectoryInfo Dir2 in DirRoot.GetDirectories())
                    {
                        if (Dir2.Name.ToLower().StartsWith("progra"))
                        {
                            foreach (System.IO.DirectoryInfo Dir3 in Dir2.GetDirectories())
                            {
                                if (Dir3.Name.ToLower().StartsWith("postgre"))
                                {
                                    foreach (System.IO.DirectoryInfo Dir4 in Dir3.GetDirectories())
                                    {
                                        foreach (System.IO.DirectoryInfo Dir5 in Dir4.GetDirectories())
                                        {
                                            if (Dir5.Name.ToLower() == "bin")
                                            {
                                                if (this.DirectoryContainsPostgresApplications(Dir5))
                                                {
                                                    WorkingDirectory = Dir5.FullName;
                                                    DirectoryFound = true;
                                                    break;
                                                }
                                            }
                                            if (DirectoryFound)
                                                break;
                                        }
                                        if (DirectoryFound)
                                            break;
                                    }
                                }
                                if (DirectoryFound)
                                    break;
                            }
                        }
                        if (DirectoryFound)
                            break;
                    }
                }
                if (!DirectoryFound)
                {
                    WorkingDirectory = "";
                    this._GetPostgresApplicationDirectoryFailed = true;
                }
            }
            catch (System.Exception ex)
            { }
            return WorkingDirectory;
        }

        private bool DirectoryContainsPostgresApplications(System.IO.DirectoryInfo Dir)
        {
            bool pg_dump_found = false;
            bool psql_found = false;
            foreach (System.IO.FileInfo F in Dir.GetFiles())
            {
                if (F.Name.ToLower() == "pg_dump.exe")
                    pg_dump_found = true;
                if (F.Name.ToLower() == "psql.exe")
                    psql_found = true;
                if (pg_dump_found && psql_found)
                    break;
            }
            if (pg_dump_found && psql_found)
                return true;
            else
                return false;
        }

        private void textBoxPostgresApplicationDirectory_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBoxPostgresApplicationDirectory.Text.Length > 0)
                {
                    System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(this.textBoxPostgresApplicationDirectory.Text);
                    if (!this.DirectoryContainsPostgresApplications(Dir))
                    {
                        System.Windows.Forms.MessageBox.Show("The selected directory does not contain the applications pg_dump.exe and psql.exe necessary to copy the database");
                        this.textBoxPostgresApplicationDirectory.Text = "";
                    }
                    this.textBoxPostgresApplicationDirectory.BackColor = System.Drawing.SystemColors.Window;
                }
                else
                    this.textBoxPostgresApplicationDirectory.BackColor = System.Drawing.Color.Pink;
            }
            catch (System.Exception ex)
            {
                this.textBoxPostgresApplicationDirectory.BackColor = System.Drawing.Color.Pink;
                this.textBoxPostgresApplicationDirectory.Text = "";
            }
        }

    }
}
