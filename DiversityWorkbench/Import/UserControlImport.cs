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
    public partial class UserControlImport : UserControl
    {
        #region Parameter

        private DiversityWorkbench.Import.iWizardInterface _WizardInterface;

        #endregion

        #region Construction & Control

        public UserControlImport(DiversityWorkbench.Import.iWizardInterface WizardInterface)
        {
            InitializeComponent();
            this._WizardInterface = WizardInterface;
            this.initControl();
        }

        private void initControl()
        {
            System.Uri U = new Uri("about:blank");
            this.webBrowser.Url = U;
            this.webBrowser.Navigate(U);
            this.textBoxSchemaFile.Text = this.SchemaName();
            this.checkBoxSaveFailedLines.Checked = DiversityWorkbench.Import.Import.SaveFailedLinesInErrorFile;
            DiversityWorkbench.Forms.FormFunctions.addEditOnDoubleClickToTextboxes(this.textBoxDescription);
            this.textBoxDescription.Text = DiversityWorkbench.Import.Import.SchemaDescription;
        }

        #endregion

        #region Import

        private void buttonStartImport_Click(object sender, EventArgs e)
        {
            if (!DiversityWorkbench.Import.Import.ImportPreconditionsOK())
                return;
            this.Cursor = Cursors.WaitCursor;
            _WizardInterface.SetLockState(true);

            try
            {
                DiversityWorkbench.Import.Import.ResetTableMessages();
                string SchemaFile = DiversityWorkbench.Import.Import.ImportData(this._WizardInterface, this.checkBoxIncludeDescription.Checked);
                this.textBoxSchemaFile.Text = SchemaFile;
                if (SchemaFile.Length > 0)
                {
                    //System.Uri U = new Uri(SchemaFile);
                    string HtmlFile = DiversityWorkbench.Import.Import.ShowConvertedFile(SchemaFile);
                    System.Uri U = new Uri(HtmlFile);
                    this.webBrowser.Url = U;
                }
                else this.webBrowser.Url = null;
                this.textBoxFailedLinesFileName.Text = DiversityWorkbench.Import.Import.ErrorFileName;
            }
            catch (Exception)
            { }
            _WizardInterface.SetLockState(false);
            this.Cursor = Cursors.Default;
        }

        #endregion

        #region Schema

        private void buttonShowSchema_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DiversityWorkbench.Import.Import.ImportPreconditionsOK())
                {
                    if (System.Windows.Forms.MessageBox.Show("Save incorrect schema?", "Save", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                string SchemaFile = DiversityWorkbench.Import.Import.SaveSchemaFile(false, this.checkBoxIncludeDescription.Checked, true, this.textBoxSchemaFile.Text);
                this.textBoxSchemaFile.Text = SchemaFile;
                string HtmlFile = DiversityWorkbench.Import.Import.ShowConvertedFile(SchemaFile);
                System.Uri U = new Uri(HtmlFile);
                this.webBrowser.Url = U;
            }
            catch (Exception ex)
            {
            }
        }

        private string SchemaName()
        {
            System.IO.FileInfo FI = new System.IO.FileInfo(DiversityWorkbench.Import.Import.FileName);
            string SchemaFileName = FI.FullName.Substring(0, FI.FullName.Length - FI.Extension.Length);
            SchemaFileName += "_" + System.DateTime.Now.Year.ToString();
            if (System.DateTime.Now.Month < 10) SchemaFileName += "0";
            SchemaFileName += System.DateTime.Now.Month.ToString();
            if (System.DateTime.Now.Day < 10) SchemaFileName += "0";
            SchemaFileName += System.DateTime.Now.Day.ToString() + "_";
            if (System.DateTime.Now.Hour < 10) SchemaFileName += "0";
            SchemaFileName += System.DateTime.Now.Hour.ToString();
            if (System.DateTime.Now.Minute < 10) SchemaFileName += "0";
            SchemaFileName += System.DateTime.Now.Minute.ToString();
            if (System.DateTime.Now.Second < 10) SchemaFileName += "0";
            SchemaFileName += System.DateTime.Now.Second.ToString();
            SchemaFileName += ".xml";
            return SchemaFileName;
        }

        private void textBoxSchemaFile_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBoxSchemaFile.Text.Length > 0)
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxSchemaFile.Text);
                    if (FI.Extension != ".xml")
                    {
                        System.Windows.Forms.MessageBox.Show("Only XML files are allowed here");
                        this.textBoxSchemaFile.Text = this.SchemaName();
                    }
                }
            }
            catch (System.Exception ex)
            {
                this.textBoxSchemaFile.Text = "";
            }
        }

        private void buttonCreateSchemaDescription_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DiversityWorkbench.Import.Import.ImportPreconditionsOK())
                {
                    if (System.Windows.Forms.MessageBox.Show("Create description for incorrect schema?", "Save", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
                System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxSchemaFile.Text);
                string Dir = FI.DirectoryName + "\\Description";
                System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(Dir);
                if (!D.Exists)
                    D.Create();
                string SchemaFile = D.FullName + "\\" + FI.Name;
                SchemaFile = DiversityWorkbench.Import.Import.SaveSchemaFile(false, true, false, SchemaFile);
                string HtmlFile = DiversityWorkbench.Import.Import.ShowConvertedDescriptionFile(SchemaFile);
                System.Uri U = new Uri(HtmlFile);
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(HtmlFile);
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
            catch (Exception ex)
            {
            }
        }

        private void textBoxDescription_Leave(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.SchemaDescription = this.textBoxDescription.Text;
        }

        #endregion

        #region Failed lines

        private void checkBoxSaveFailedLines_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.Import.SaveFailedLinesInErrorFile = this.checkBoxSaveFailedLines.Checked;
        }

        private void buttonOpenErrorFile_Click(object sender, EventArgs e)
        {
            if (this.textBoxFailedLinesFileName.Text.Length > 0)
            {
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(this.textBoxFailedLinesFileName.Text);
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
        }

        #endregion

    }
}
