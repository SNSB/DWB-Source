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
    public partial class UserControlTesting : UserControl
    {
        #region Parameter

        private DiversityWorkbench.Import.iWizardInterface _WizardInterface;
        private DiversityWorkbench.Import.DataColumn _CheckForPresenceColumn;
        private static int? _TestLine;

        #endregion

        #region Construction & Control

        public UserControlTesting(DiversityWorkbench.Import.iWizardInterface WizardInterface)
        {
            InitializeComponent();
            this._WizardInterface = WizardInterface;
            this.initControl();
        }

        private void initControl()
        {
            try
            {
                if (DiversityWorkbench.Import.Import.StartLine <= this.numericUpDownAnalyseData.Maximum)
                {
                    if (this.numericUpDownAnalyseData.Maximum < DiversityWorkbench.Import.Import.StartLine)
                        this.numericUpDownAnalyseData.Maximum = DiversityWorkbench.Import.Import.StartLine;
                    if (this.numericUpDownAnalyseData.Value < DiversityWorkbench.Import.Import.StartLine)
                        this.numericUpDownAnalyseData.Value = DiversityWorkbench.Import.Import.StartLine;
                    this.numericUpDownAnalyseData.Minimum = DiversityWorkbench.Import.Import.StartLine;
                }
                if (DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition &&
                    this.numericUpDownAnalyseData.Minimum < 2)
                    this.numericUpDownAnalyseData.Minimum = 2;
                if (DiversityWorkbench.Import.UserControlTesting._TestLine != null)
                    this.numericUpDownAnalyseData.Value = (int)DiversityWorkbench.Import.UserControlTesting._TestLine;
                else
                    this.numericUpDownAnalyseData.Value = DiversityWorkbench.Import.Import.StartLine;
                this.numericUpDownAnalyseData.Maximum = DiversityWorkbench.Import.Import.EndLine;
                this.treeView.Nodes.Clear();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void textBoxMessage_DoubleClick(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Error", this.textBoxMessage.Text, true);
            f.ShowDialog();
        }

        private void numericUpDownAnalyseData_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                this._WizardInterface.setActiveDataGridRow((int)this.numericUpDownAnalyseData.Value);
                DiversityWorkbench.Import.UserControlTesting._TestLine = (int)this.numericUpDownAnalyseData.Value;
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Testing

        private void buttonTestData_Click(object sender, EventArgs e)
        {
            if (!DiversityWorkbench.Import.Import.ImportPreconditionsOK())
                return;
            try
            {
                string Message = "";
                DiversityWorkbench.Import.Import.ResetTableMessages();
                this.treeView.Nodes.Clear();
                if (!DiversityWorkbench.Import.Import.ImportData(this._WizardInterface, (int)this.numericUpDownAnalyseData.Value, true))
                {
                    string ErrorMessage = "";
                    foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<int, string> KV in DiversityWorkbench.Import.Import.Tables[T].Messages)
                        {
                            ErrorMessage += "\r\n" + T + ": " + KV.Value;
                        }
                    }
                    System.Windows.Forms.MessageBox.Show("Simulation of import failed:" + ErrorMessage);
                    return;
                }
                System.Windows.Forms.ImageList ImageList = new ImageList();
                ImageList.Images.Add(this.pictureBox1.Image);
                foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                {
                    ImageList.Images.Add(DiversityWorkbench.Import.Import.Tables[T].Image);
                }
                this.treeView.ImageList = ImageList;
                int i = 1;
                foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                {
                    // Toni: If no action is required and there was no error until now, do not report subsequent errors (NoData and NoDifferences)
                    bool reportError = DiversityWorkbench.Import.Import.Tables[T].Messages.Count > 0;
                    System.Windows.Forms.TreeNode N = new TreeNode(DiversityWorkbench.Import.Import.Tables[T].DisplayText, i, i);
                    N.BackColor = DiversityWorkbench.Import.DataTable.ActionColor(DiversityWorkbench.Import.Import.Tables[T].ActionNeeded());
                    switch (DiversityWorkbench.Import.Import.Tables[T].ActionNeeded())
                    {
                        case DataTable.NeededAction.Correction:
                            N.Text += " [data must be corrected]";
                            reportError = true;
                            break;
                        case DataTable.NeededAction.Error:
                            N.Text += " [error during import simulation]";
                            reportError = true;
                            break;
                        case DataTable.NeededAction.NoData:
                            N.Text += " [no data found]";
                            break;
                        case DataTable.NeededAction.NoDifferences:
                            N.Text += " [no differences found]";
                            break;
                        case DataTable.NeededAction.Update:
                            N.Text += " [UPDATE]";
                            reportError = true;
                            break;
                        case DataTable.NeededAction.Insert:
                            N.Text += " [INSERT]";
                            reportError = true;
                            break;
                        case DataTable.NeededAction.Attach:
                            N.Text += " [attach]";
                            reportError = true;
                            break;
                        case DataTable.NeededAction.Duplicate:
                            N.Text += " [duplicate]";
                            reportError = true;
                            break;
                    }
                    i++;
                    this.treeView.Nodes.Add(N);
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in DiversityWorkbench.Import.Import.Tables[T].DataColumns)
                    {
                        if (/*KV.Value.Value != null && KV.Value.Value.Length > 0 &&*/ KV.Value.IsSelected && KV.Value.TransformedValue().Length > 0)
                        {
                            string NodeText = KV.Value.DisplayText + ": " + KV.Value.ValueFormatedForSQL();
                            System.Windows.Forms.TreeNode NC = new TreeNode(NodeText, 0, 0);
                            if (KV.Value.IsDecisive)
                                NC.ForeColor = System.Drawing.Color.Green;
                            if (KV.Value.CompareKey)
                                NC.BackColor = System.Drawing.Color.Yellow;
                            N.Nodes.Add(NC);
                        }
                    }
                    if (DiversityWorkbench.Import.Import.Tables[T].Messages.Count > 0 && reportError) // Toni: See previous comment
                    {
                        if (Message.Length > 0) Message += "\r\n";
                        Message += T + ":\r\n";
                        foreach (System.Collections.Generic.KeyValuePair<int, string> M in DiversityWorkbench.Import.Import.Tables[T].Messages)
                            Message += M.Value + "\r\n";
                    }
                }
                if (Message.Length > 0)
                {
                    this.textBoxMessage.BackColor = System.Drawing.Color.Pink;
                    this.textBoxMessage.ForeColor = System.Drawing.Color.Black;
                    this.textBoxMessage.Text = Message;
                }
                else
                {
                    this.textBoxMessage.Text = "";
                    this.textBoxMessage.BackColor = System.Drawing.SystemColors.Control;
                }
                this.treeView.ExpandAll();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try { this._WizardInterface.DataGridView().FirstDisplayedScrollingRowIndex = (int)this.numericUpDownAnalyseData.Value - 1; }
            catch { }
            foreach (System.Windows.Forms.DataGridViewCell C in this._WizardInterface.DataGridView().SelectedCells)
                C.Selected = false;
            this._WizardInterface.DataGridView().Rows[(int)this.numericUpDownAnalyseData.Value - 1].Selected = true;
        }

        #endregion

        #region Check for presence

        private System.Collections.Generic.List<int> _MissingLines;

        public System.Collections.Generic.List<int> MissingLines
        {
            get
            {
                if (this._MissingLines == null)
                    this._MissingLines = new List<int>();
                return _MissingLines;
            }
            set { _MissingLines = value; }
        }

        private void buttonCheckForPresentData_Click(object sender, EventArgs e)
        {
            if (this._CheckForPresenceColumn == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the field for comparision");
                return;
            }
            try
            {
                this.MissingLines = null;
                System.Collections.Generic.List<int> LinesWithPresentData = new List<int>();
                int NumberOfPresentData = 0;
                for (int R = DiversityWorkbench.Import.Import.StartLine - 1; R < DiversityWorkbench.Import.Import.EndLine; R++)
                {
                    string Value = this._WizardInterface.DataGridView().Rows[R].Cells[(int)this._CheckForPresenceColumn.FileColumn].Value.ToString();
                    if (this._CheckForPresenceColumn.IsMultiColumn)
                    {
                        if (this._CheckForPresenceColumn.Postfix != null) Value += this._CheckForPresenceColumn.Postfix;
                        foreach (DiversityWorkbench.Import.ColumnMulti M in this._CheckForPresenceColumn.MultiColumns)
                        {
                            if (M.Prefix != null) Value += M.Prefix;
                            Value += this._WizardInterface.DataGridView().Rows[R].Cells[(int)M.ColumnInFile].Value.ToString();
                            if (M.Postfix != null) Value += M.Postfix;
                        }
                        // Bugfix 7.3.23 - Prefix wurde nicht berücksichtigt
                        if (this._CheckForPresenceColumn.Prefix != null && this._CheckForPresenceColumn.Prefix.Length > 0 && !Value.StartsWith(this._CheckForPresenceColumn.Prefix))
                        {
                            Value = this._CheckForPresenceColumn.Prefix + Value;
                        }
                    }
                    string SQL = "SELECT COUNT(*) FROM [" + this._CheckForPresenceColumn.DataTable.TableName + "] " +
                        "WHERE [" + this._CheckForPresenceColumn.ColumnName + "] = '" + Value + "'";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    con.Open();
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    if (int.TryParse(C.ExecuteScalar()?.ToString(), out NumberOfPresentData) && NumberOfPresentData > 0)
                    {
                        LinesWithPresentData.Add(R);
                        this._WizardInterface.setDataGridLineColor(R + 1, System.Drawing.Color.Pink);
                    }
                    else
                    {
                        this._WizardInterface.setDataGridLineColor(R + 1, System.Drawing.Color.White);
                        this.MissingLines.Add(R + 1);
                    }
                    con.Close();
                    con.Dispose();
                }
                this._WizardInterface.DataGridView().Refresh();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void comboBoxCheckForPresenceColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string[] TableColumn = comboBoxCheckForPresenceColumn.SelectedItem.ToString().Split(new char[] { '.' });
            string Table = TableColumn[0];
            string Column = TableColumn[1];
            this._CheckForPresenceColumn = DiversityWorkbench.Import.Import.Tables[Table].DataColumns[Column];
        }

        private void comboBoxCheckForPresenceColumn_DropDown(object sender, EventArgs e)
        {
            comboBoxCheckForPresenceColumn.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KVT in DiversityWorkbench.Import.Import.Tables)
            {
                foreach (string C in KVT.Value.AttachmentColumns)
                {
                    if (C.Length > 0 && KVT.Value.DataColumns[C].FileColumn != null)
                        comboBoxCheckForPresenceColumn.Items.Add(KVT.Key + "." + C);
                }
            }
        }

        private void buttonExportMissingData_Click(object sender, EventArgs e)
        {
            System.IO.FileInfo SourceFile = new System.IO.FileInfo(DiversityWorkbench.Import.Import.FileName);
            string FileNameMissing = SourceFile.FullName.Substring(0, SourceFile.FullName.Length - (SourceFile.Extension.Length)) + "_Missing" + SourceFile.Extension;
            try
            {
                bool MissingNameIsNew = false;
                System.IO.FileInfo Missing = new System.IO.FileInfo(FileNameMissing);
                while (!MissingNameIsNew)
                {
                    if (!Missing.Exists)
                        MissingNameIsNew = true;
                    else
                    {
                        DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Missing lines", "Please change the name of the file where the missing lines should be stored", Missing.Name.Substring(0, Missing.Name.IndexOf(".")));
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            FileNameMissing = SourceFile.FullName.Substring(0, SourceFile.FullName.Length - (SourceFile.Name.Length)) + f.String + SourceFile.Extension;
                            Missing = new System.IO.FileInfo(FileNameMissing);
                            if (!Missing.Exists)
                                MissingNameIsNew = true;
                        }
                        else
                        {
                            if (System.Windows.Forms.MessageBox.Show("The file " + Missing + " exists. This file will be deleted to save the errors. Rename or copy this file if you want to keep it", "Delete existing file?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                            {
                                Missing.Delete();
                                MissingNameIsNew = true;
                            }
                        }
                    }
                }

                int iLine = 0;
                int SavedLines = 0;
                System.IO.StreamReader sr = this.StreamReader(SourceFile.FullName, DiversityWorkbench.Import.Import.Encoding);
                System.IO.StreamWriter sw = this.StreamWriter(FileNameMissing, DiversityWorkbench.Import.Import.Encoding);
                using (sr)
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (iLine > DiversityWorkbench.Import.Import.EndLine)
                            break;
                        if (DiversityWorkbench.Import.Import.FirstLineContainsColumnDefinition && iLine == 0)
                            sw.WriteLine(line);
                        else if (this.MissingLines.Contains(iLine + 1))
                        {
                            sw.WriteLine(line);
                            SavedLines++;
                        }
                        iLine++;

                    }
                    sw.Close();
                }
                System.Windows.Forms.MessageBox.Show(SavedLines.ToString() + " Missing lines were saved in the file\r\n" + FileNameMissing);
            }
            catch (System.IO.IOException IOex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(IOex);
                //System.Windows.Forms.MessageBox.Show(IOex.Message);
                //return false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                //return false;
            }
        }

        private System.IO.StreamReader StreamReader(string File, System.Text.Encoding Encoding)
        {
            System.IO.StreamReader sr;
            sr = new System.IO.StreamReader(File, Encoding);
            return sr;
        }

        private System.IO.StreamWriter StreamWriter(string File, System.Text.Encoding Encoding)
        {
            System.IO.StreamWriter sr;
            sr = new System.IO.StreamWriter(File, false, Encoding);
            return sr;
        }

        #endregion

    }
}
