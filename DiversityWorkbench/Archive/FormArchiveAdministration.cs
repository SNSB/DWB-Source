using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Archive
{
    public partial class FormArchiveAdministration : Form
    {
        #region Paramter

        private string _SourceTable;
        private string _DisplayColumn;
        private string _Keycolumn;
        private string _DecisionColumn;
        private string _ProtocolColumn;
        private string _Restriction;
        private string _TempIDTable;
        private string _TempIDColumn;
        private string _DirectoryForArchives;
        private bool _RunAsBackground = false;
        private System.Data.DataTable _DtArchive;
        private Microsoft.Data.SqlClient.SqlDataAdapter _DataAdapter;
        private System.Collections.Generic.Dictionary<string, string> _DataTables;
        
        #endregion

        #region Construction

        public FormArchiveAdministration(System.Collections.Generic.Dictionary<string, string> DataTables, 
            string HeaderText, 
            string SourceTable, 
            string DisplayColumn, 
            string KeyColumn, 
            string DecisionColumn, 
            string ProtocolColumn, 
            string Restriction)
        {
            InitializeComponent();
            this._DataTables = DataTables;
            this.labelHeader.Text = HeaderText;
            this._SourceTable = SourceTable;
            this._DisplayColumn = DisplayColumn;
            this._Keycolumn = KeyColumn;
            this._DecisionColumn = DecisionColumn;
            this._ProtocolColumn = ProtocolColumn;
            this._Restriction = Restriction;
            this.initForm();
        }

        public FormArchiveAdministration(
            string ArchiveDirectory, 
            string SourceTable, 
            string DisplayColumn,
            string KeyColumn, 
            string DecisionColumn, 
            string ProtocolColumn, 
            string TempIDTable, 
            string TempIDColumn, 
            System.Collections.Generic.Dictionary<string, string> DataTables)
        {
            try
            {
                InitializeComponent();
                this.Hide();
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormArchiveAdministration", "FormArchiveAdministration(...)", "Starting archive", DiversityWorkbench.Settings.ModuleName);
                this._RunAsBackground = true;
                this._SourceTable = SourceTable;
                this._DisplayColumn = DisplayColumn;
                this._Keycolumn = KeyColumn;
                this._DecisionColumn = DecisionColumn;
                this._ProtocolColumn = ProtocolColumn;
                this._TempIDColumn = TempIDColumn;
                this._TempIDTable = TempIDTable;
                this._Restriction = " WHERE " + this._DecisionColumn + " = 1 ";
                this._DirectoryForArchives = ArchiveDirectory;
                this._DataTables = DataTables;
                this.CreateArchives();
                this.Close();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormArchiveAdministration", "FormArchiveAdministration(...)", ex.Message, DiversityWorkbench.Settings.ModuleName);
            }
        }

        #endregion

        #region Form

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            // #35
            //KeyWord = "archive" + DiversityWorkbench.DwbManual.Hugo.KeywordPostfix.ToLower();

            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void initForm()
        {
            this._DirectoryForArchives = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Archive);
            //this._DirectoryForArchives = Application.StartupPath + "\\Archive";
            this.textBoxDirectory.Text = this._DirectoryForArchives;
            this.getData();
            this.buildTree();
        }

        private void buildTree()
        {
            this.treeViewArchives.Nodes.Clear();
            foreach (System.Data.DataRow R in this._DtArchive.Rows)
            {
                System.Windows.Forms.TreeNode N = new TreeNode(R[this._DisplayColumn].ToString());
                N.Tag = R;
                if (R[this._DecisionColumn].Equals(System.DBNull.Value))
                    N.Checked = false;
                else
                {
                    N.Checked = bool.Parse(R[this._DecisionColumn].ToString());
                }
                if (!R[this._ProtocolColumn].Equals(System.DBNull.Value) && R[this._ProtocolColumn].ToString().Length > 0)
                {
                    if (R[this._ProtocolColumn].ToString().IndexOf("Failure ") > -1 ||
                        R[this._ProtocolColumn].ToString().IndexOf("Error: ") > -1)
                        N.BackColor = System.Drawing.Color.Pink;
                    else N.BackColor = System.Drawing.Color.LightGreen;
                }
                this.treeViewArchives.Nodes.Add(N);
            }
        }

        private void getData()
        {
            string SQL = "";
            try
            {
                string WhereClause = this._Restriction;
                if (this._RunAsBackground)
                {
                    if (WhereClause.Length > 0) WhereClause += " AND ";
                    else WhereClause = "WHERE ";
                    WhereClause += this._DecisionColumn + " = 1";
                }
                SQL = "SELECT " + this._DisplayColumn + ", " + this._Keycolumn + ", " + this._DecisionColumn + ", " + this._ProtocolColumn +
                    " FROM " + this._SourceTable + " " + WhereClause + 
                    " ORDER BY " + this._DisplayColumn;
                this._DtArchive = new DataTable();
                this._DataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._DataAdapter.Fill(this._DtArchive);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormArchiveAdministration", "getData()", ex.Message + "\r\nQuery: " + SQL + "\r\nConnection: " + DiversityWorkbench.Settings.ConnectionString , DiversityWorkbench.Settings.ModuleName);
            }
        }

        private void treeViewArchives_AfterCheck(object sender, TreeViewEventArgs e)
        {
            string SQL = "";
            try
            {
                if (e.Node != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)e.Node.Tag;
                    SQL = "UPDATE T SET T.[" + this._DecisionColumn + "] = ";
                    if (e.Node.Checked)
                        SQL += "1";
                    else SQL += "0";
                    SQL += " FROM [" + this._SourceTable + "] AS T WHERE T." + this._Keycolumn + " = " + R[this._Keycolumn].ToString();
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                }
                else
                    System.Windows.Forms.MessageBox.Show("Please select a node");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        private void treeViewArchives_AfterSelect(object sender, TreeViewEventArgs e)
        {
            System.Data.DataRow R = (System.Data.DataRow)this.treeViewArchives.SelectedNode.Tag;
            if (R[this._ProtocolColumn].Equals(System.DBNull.Value))
            {
                this.labelProtocol.Text = "";
                this.textBoxProtocol.Text = "";
            }
            else
            {
                this.labelProtocol.Text = "Protocol for " + this.treeViewArchives.SelectedNode.Text;
                this.textBoxProtocol.Text = R[this._ProtocolColumn].ToString();
            }
        }
        
        private void buttonCreateArchives_Click(object sender, EventArgs e)
        {
            if (this.CreateArchives())
            {
                this.getData();
                this.buildTree();
                System.Windows.Forms.MessageBox.Show("Archives created");
            }
            else
                System.Windows.Forms.MessageBox.Show("Creating archives failed");
        }

        public bool CreateArchives()
        {
            bool OK = true;
            this.getData();
            foreach (System.Data.DataRow R in this._DtArchive.Rows)
            {
                bool DoCreate;
                if (bool.TryParse(R[this._DecisionColumn].ToString(), out DoCreate) && DoCreate)
                {
                    if (!this._RunAsBackground)
                    {
                        this.labelCurrentStep.Text = "Creating archive for " + R[this._DisplayColumn].ToString();
                        Application.DoEvents();
                    }
                    else
                        DiversityWorkbench.ExceptionHandling.WriteToLog("FormArchiveAdministration", "CreateArchives()", "Starting archive for " + R[this._DisplayColumn].ToString(), DiversityWorkbench.Settings.ModuleName);
                    Archive.FormCreateArchive f = new FormCreateArchive(this._DataTables, this._DirectoryForArchives, this._SourceTable, this._DisplayColumn, this._Keycolumn, R[this._Keycolumn].ToString(), this._TempIDTable, this._TempIDColumn, this._DecisionColumn, this._ProtocolColumn, "ProjectDataLastChanges");
                    if (!f.CreateArchive())
                        OK = false;
                }
            }
            if (!this._RunAsBackground)
            {
                this.labelCurrentStep.Text = "Finished";
                Application.DoEvents();
            }
            else
                DiversityWorkbench.ExceptionHandling.WriteToLog("FormArchiveAdministration", "CreateArchives()", "Finished", DiversityWorkbench.Settings.ModuleName);

            return OK;
        }

        //public void SetInrementialArchiveStartDate(System.DateTime DateTime)
        //{
        //    this.ar;
        //}


        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewArchives.Nodes)
            {
                N.Checked = true;
            }
        }

        private void buttonCheckNone_Click(object sender, EventArgs e)
        {
            foreach (System.Windows.Forms.TreeNode N in this.treeViewArchives.Nodes)
            {
                N.Checked = false;
            }
        }

        #region Protocol

        private void buttonClearProtocol_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.treeViewArchives.SelectedNode == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the entry of which the protocol should be cleared");
                    return;
                }
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewArchives.SelectedNode.Tag;
                string Restriction = " WHERE T." + this._Keycolumn + " = " + R[this._Keycolumn].ToString();
                this.ClearProtocol(Restriction);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonClearAllProtocols_Click(object sender, EventArgs e)
        {
            string Restriction = "";
            this.ClearProtocol(Restriction);
        }

        private void ClearProtocol(string Restriction)
        {
            try
            {
                string SQL = "UPDATE T SET T.[" + this._ProtocolColumn + "] = ''";
                SQL += " FROM [" + this._SourceTable + "] AS T " + Restriction;
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #endregion

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private void buttonDirectory_Click(object sender, EventArgs e)
        {
            string ArchDir = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Archive);
            if (System.IO.Directory.Exists(ArchDir))
                System.Diagnostics.Process.Start("explorer.exe", ArchDir);
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}
