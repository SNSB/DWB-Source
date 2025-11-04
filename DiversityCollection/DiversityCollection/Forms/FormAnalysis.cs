using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormAnalysis : Form
    {

        #region Parameter

        private DiversityCollection.Analysis _Analysis;
        private System.Data.DataTable _DtTaxonomicGroup;

        #endregion
        
        #region Construction

        public FormAnalysis()
        {
            InitializeComponent();
            this.splitContainerData.Panel2Collapsed = true;
            this.splitContainerMain.Panel2.Visible = false;
            this.initForm();
            // online manual
            //this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }

        public FormAnalysis(int? ItemID)
            : this()
        {
            if (ItemID != null)
                this._Analysis.setItem((int)ItemID);
            this.userControlDialogPanel.Visible = true;
            this.panelHeader.Visible = true;
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                System.Data.DataSet Dataset = this.dataSetAnalysis1;
                if (this._Analysis == null)
                    this._Analysis = new Analysis(ref Dataset, this.dataSetAnalysis1.Analysis,
                        ref this.treeViewAnalysis, this, this.userControlQueryList, this.splitContainerMain,
                        this.splitContainerData, this.toolStripButtonSpecimenList, //this.imageListSpecimenList,
                        this.userControlSpecimenList, this.helpProvider, this.toolTip, ref this.analysisBindingSource);
                this._Analysis.initForm();
                this._Analysis.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
                this._Analysis.setToolStripButtonNewEvent(this.toolStripButtonNew);
                this._Analysis.setToolStripButtonSetParentEvent(this.toolStripButtonSetParent);
                this._Analysis.setToolStripButtonRemoveParentEvent(this.toolStripButtonRemoveParent);
                this.userControlQueryList.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_AddProject);
                this._Analysis.setToolStripButtonIncludeIDEvent(this.toolStripButtonIncludeID);
                this.setPermissions();
                this.userControlSpecimenList.toolStripButtonDelete.Visible = false;
                this.userControlQueryList.RememberSettingIsAvailable(true);
                this.userControlQueryList.RememberQuerySettingsIdentifier = "Analysis";
                this.userControlQueryList.RememberQueryConditionSettings_ReadFromFile();
                this.toolStripButtonSpecimenList.Visible = false;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void FormAnalysis_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this._Analysis != null)
                    this._Analysis.saveItem();
                if (this.userControlQueryList.RememberQuerySettings())
                    this.userControlQueryList.RememberQueryConditionSettings_SaveToFile();
                else
                    this.userControlQueryList.RememberQueryConditionSettings_RemoveFile();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void toolStripButtonNew_AddProject(object sender, EventArgs e)
        {
            if (this.userControlQueryList.ProjectIsSelected)
            {
                System.Data.DataRow R = this.dataSetAnalysis1.ProjectAnalysis.NewRow();
                System.Data.DataRow Rlist = this.dataSetAnalysis1.ProjectAnalysisList.NewProjectAnalysisListRow();
                R["AnalysisID"] = this.ID;
                R["ProjectID"] = this.userControlQueryList.ProjectID;
                Rlist["AnalysisID"] = this.ID;
                Rlist["Project"] = DiversityCollection.LookupTable.ProjectName(this.userControlQueryList.ProjectID);
                Rlist["ProjectID"] = this.userControlQueryList.ProjectID; ;
                try
                {
                    this.dataSetAnalysis1.ProjectAnalysis.Rows.Add(R);
                    this.dataSetAnalysis1.ProjectAnalysisList.Rows.Add(Rlist);
                }
                catch { }
            }
        }

        private void setPermissions()
        {
            DiversityWorkbench.Forms.FormFunctions FF = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
            this._Analysis.setToolbarPermission(ref this.toolStripButtonDelete, "Analysis", "Delete");
            this._Analysis.setToolbarPermission(ref this.toolStripButtonNew, "Analysis", "Insert");

            System.Collections.Generic.List<System.Windows.Forms.Control> ControlList = new List<Control>();
            System.Collections.Generic.List<System.Windows.Forms.ToolStripItem> TSIlist = new List<ToolStripItem>();
            TSIlist.Add(this.toolStripButtonTaxonomicGroupAddMany);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "AnalysisTaxonomicGroup", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonTaxonomicGroupDelete);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "AnalysisTaxonomicGroup", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
            
            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonProjectAddMany);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ProjectAnalysis", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonProjectsDelete);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ProjectAnalysis", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);

            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonResultNew);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "AnalysisResult", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonResultDelete);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "AnalysisResult", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
        }

        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());
        }

        private void FormAnalysis_Load(object sender, EventArgs e)
        {
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAnalysis1.AnalysisResult". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.analysisResultTableAdapter1.Fill(this.dataSetAnalysis1.AnalysisResult);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAnalysis1.ProjectAnalysisList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectAnalysisListTableAdapter1.Fill(this.dataSetAnalysis1.ProjectAnalysisList);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAnalysis1.Analysis". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.analysisTableAdapter.Fill(this.dataSetAnalysis1.Analysis);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAnalysis1.AnalysisResult". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.analysisResultTableAdapter.Fill(this.dataSetAnalysis1.AnalysisResult);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetAnalysis1.ProjectAnalysisList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectAnalysisListTableAdapter.Fill(this.dataSetAnalysis1.ProjectAnalysisList);

        }

        #endregion

        #region URI

        private void buttonUriOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                int i = this.analysisBindingSource.Position;
                this.dataSetAnalysis1.Analysis.Rows[i]["AnalysisURI"] = f.URL;
                this.textBoxURI.Text = f.URL;
            }
        }

        private void textBoxURI_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxURI.Text.Length > 0)
            {
                this.userControlWebViewURI.Visible = true;
                //this.webBr owserURI.Visible = true;
                try
                {
                    System.Uri URI = new Uri(this.textBoxURI.Text);
                    this.userControlWebViewURI.Url = null;
                    this.userControlWebViewURI.Navigate(URI);
                    //this.web BrowserURI.Url = URI;
                }
                catch { }
            }
            else
            {
                this.userControlWebViewURI.Visible = false;
                //this.webB rowserURI.Visible = false;
            }
        }
        
        #endregion

        #region Taxonomic Group

        private void toolStripButtonTaxonomicGroupAddMany_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = this.DtTaxonomicGroup.Copy();
            foreach (System.Data.DataRow R in this.dataSetAnalysis1.AnalysisTaxonomicGroup.Rows)
            {
                foreach (System.Data.DataRow RT in dt.Rows)
                {
                    if (RT.RowState == DataRowState.Unchanged && R.RowState == DataRowState.Unchanged)
                    {
                        if (RT[0].ToString() == R[1].ToString())
                            RT.Delete();
                    }
                }
            }
            dt.AcceptChanges();
            string Header = "Select taxonomic groups";
            try { Header = DiversityCollection.FormAnalysisText.Select_a_taxonomic_group; }
            catch { }
            System.Collections.Generic.Dictionary<string, bool> TG = new Dictionary<string, bool>();
            foreach (System.Data.DataRow R in dt.Rows)
                TG.Add(R["Code"].ToString(), false);
            DiversityWorkbench.Forms.FormGetMultiFromList f = new DiversityWorkbench.Forms.FormGetMultiFromList("Taxonomic groups", "Please select the taxonomic groups from the list", TG);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in f.Items)
                {
                    if (KV.Value)
                    {
                        System.Data.DataRow R = this.dataSetAnalysis1.AnalysisTaxonomicGroup.NewRow();
                        R["AnalysisID"] = this.ID;
                        R["TaxonomicGroup"] = KV.Key;
                        try
                        {
                            this.dataSetAnalysis1.AnalysisTaxonomicGroup.Rows.Add(R);
                        }
                        catch { }
                    }
                }
            }
        }

        private void toolStripButtonTaxonomicGroupAdd_Click(object sender, EventArgs e)
        {
            //string SQL = "SELECT Code FROM CollTaxonomicGroup_Enum WHERE Code NOT IN " +
            //    "( SELECT TaxonomicGroup FROM AnalysisTaxonomicGroup WHERE AnalysisID = " + this.ID.ToString() + ")";
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dt = this.DtTaxonomicGroup.Copy();
            //ad.Fill(dt);
            foreach (System.Data.DataRow R in this.dataSetAnalysis1.AnalysisTaxonomicGroup.Rows)
            {
                foreach (System.Data.DataRow RT in dt.Rows)
                {
                    if (RT.RowState == DataRowState.Unchanged)
                    {
                        if (RT[0].ToString() == R[1].ToString())
                            RT.Delete();
                    }
                }
            }
            //DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, this.Message("Select_a_taxonomic_group"));// "Select a taxonomic group");
            string Header = "Select a taxonomic group";
            try { Header = DiversityCollection.FormAnalysisText.Select_a_taxonomic_group; }
            catch { }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, Header);// );
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataRow R = this.dataSetAnalysis1.AnalysisTaxonomicGroup.NewRow();
                R["AnalysisID"] = this.ID;
                R["TaxonomicGroup"] = f.SelectedValue;
                //R["Code"] = f.SelectedValue;
                //R["DisplayText"] = f.SelectedString;
                try
                {
                    this.dataSetAnalysis1.AnalysisTaxonomicGroup.Rows.Add(R);
                }
                catch { }
            }
        }

        private void toolStripButtonTaxonomicGroupDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.analysisTaxonomicGroupBindingSource.Current != null)
                    this.analysisTaxonomicGroupBindingSource.RemoveCurrent();
            }
            catch (System.Exception ex)
            {
            }
        }

        public System.Data.DataTable DtTaxonomicGroup
        {
            get 
            {
                if (this._DtTaxonomicGroup == null)
                {
                    this._DtTaxonomicGroup = new DataTable();
                    string SQL = "SELECT Code FROM CollTaxonomicGroup_Enum ORDER BY Code";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._DtTaxonomicGroup);
                }
                return _DtTaxonomicGroup; 
            }
        }
        
        private void buttonTaxonomicGroupAddExisting_Click(object sender, EventArgs e)
        {
            string SQL = "INSERT INTO AnalysisTaxonomicGroup (AnalysisID, TaxonomicGroup) SELECT DISTINCT " + this.ID.ToString() + ", TaxonomicGroup FROM IdentificationUnit WHERE TaxonomicGroup NOT IN " +
                "(SELECT TaxonomicGroup FROM AnalysisTaxonomicGroup WHERE AnalysisID = " + this.ID.ToString() + ")";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
            {
                this._Analysis.setItem(this.ID);
            }
        }

        #endregion

        #region Properties

        public int ID 
        { 
            get 
            {
                if (this.dataSetAnalysis1.Analysis.Rows.Count > 0)
                    return int.Parse(this.dataSetAnalysis1.Analysis.Rows[this.analysisBindingSource.Position][0].ToString());
                else return -1;
            } 
        }
        public string DisplayText { get { return this.dataSetAnalysis1.Analysis.Rows[this.analysisBindingSource.Position][2].ToString(); } }
        public bool ChangeToSpecimen { get { return this.userControlSpecimenList.ChangeToSpecimen; } }
        public int CollectionSpecimenID { get { return this.userControlSpecimenList.CollectionSpecimenID; } }

        #endregion

        #region Menu

        private void showURIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.showURIToolStripMenuItem.Checked)
                {
                    this.showURIToolStripMenuItem.Checked = false;
                    this.splitContainerURI.Panel2Collapsed = true;
                }
                else
                {
                    this.showURIToolStripMenuItem.Checked = true;
                    this.splitContainerURI.Panel2Collapsed = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #region History

        private void toolStripMenuItemHistory_Click(object sender, EventArgs e)
        {
            if (this.dataSetAnalysis1.Analysis.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            string Title = "History of " + this.dataSetAnalysis1.Analysis.Rows[0]["DisplayText"].ToString() + " (AnalysisID: " + this.dataSetAnalysis1.Analysis.Rows[0]["AnalysisID"].ToString() + ")";
            try
            {
                bool HistoryPresent = false;
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();

                if (this.dataSetAnalysis1.Analysis.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AnalysisID", this.dataSetAnalysis1.Analysis.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetAnalysis1.AnalysisResult.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AnalysisID", this.dataSetAnalysis1.AnalysisResult.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetAnalysis1.AnalysisTaxonomicGroup.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AnalysisID", this.dataSetAnalysis1.AnalysisTaxonomicGroup.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetAnalysis1.ProjectAnalysis.Rows.Count > 0)
                {
                    LogTables.Add(this.dtProjectAnalysisHistory);
                    HistoryPresent = true;
                }
                if (this.dataSetAnalysis1.MethodForAnalysis.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "AnalysisID", this.dataSetAnalysis1.MethodForAnalysis.TableName, ""));
                    HistoryPresent = true;
                }
                if (HistoryPresent)
                {
                    DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());
                    f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "History");
                    f.ShowDialog();
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("No history data found");
                }

            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Data.DataTable dtProjectAnalysisHistory
        {
            get
            {
                System.Data.DataTable dtProjectAnalysis = new DataTable("ProjectAnalysis");
                string SqlCurrent = "SELECT P.Project, A.LogUpdatedWhen AS [Date of change], A.LogUpdatedBy AS [Responsible user] " +
                    "FROM ProjectAnalysis AS A INNER JOIN " +
                    "ProjectProxy AS P ON A.ProjectID = P.ProjectID WHERE A.AnalysisID = " + this.ID.ToString();
                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtProjectAnalysis);
                }

                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return dtProjectAnalysis;
            }
        }
        


        #endregion

        private void toolStripMenuItemTableEditor_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (this.userControlQueryList.ListOfIDs.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Nothing selected");
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    return;
                }
                DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(DiversityCollection.Resource.Analysis, "Analysis", "AnalysisID");
                f.initTable(this.userControlQueryList.ListOfIDs);
                f.StartPosition = FormStartPosition.CenterParent;
                f.Width = this.Width - 10;
                f.Height = this.Height - 10;
                try
                {
                    f.ShowDialog();
                }
                catch (System.Exception ex)
                {
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "toolStripMenuItemTableEditor_Click");
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region Projects

        private void toolStripButtonProjectAddMany_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT P.Project, P.ProjectID " +
                "FROM ProjectList P " +
                "WHERE P.ProjectID NOT IN (SELECT ProjectID FROM ProjectAnalysis WHERE AnalysisID = " + this.ID.ToString() + ") " +
                "ORDER BY P.Project";
            System.Data.DataTable dtProject = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtProject);
            foreach (System.Data.DataRow R in this.dataSetAnalysis1.ProjectAnalysisList.Rows)
            {
                foreach (System.Data.DataRow RT in dtProject.Rows)
                {
                    if (RT.RowState == DataRowState.Unchanged && R.RowState == DataRowState.Unchanged)
                    {
                        if (RT[0].ToString() == R[1].ToString())
                            RT.Delete();
                    }
                }
            }
            dtProject.AcceptChanges();
            if (dtProject.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("All project have been added");
                return;
            }
            string Header = "Select projects";
            try { Header = DiversityCollection.FormAnalysisText.Please_select_the_project_that; }
            catch { }
            System.Collections.Generic.Dictionary<string, bool> P = new Dictionary<string, bool>();
            foreach (System.Data.DataRow R in dtProject.Rows)
            {
                if (!P.ContainsKey(R["Project"].ToString())) // #264
                    P.Add(R["Project"].ToString(), false);
            }
            DiversityWorkbench.Forms.FormGetMultiFromList f = new DiversityWorkbench.Forms.FormGetMultiFromList("Projects", "Please select the projects from the list", P);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in f.Items)
                {
                    if (KV.Value)
                    {
                        int ProjectID;
                        System.Data.DataRow[] rr = dtProject.Select("Project = '" + KV.Key +"'");
                        if (int.TryParse(rr[0]["ProjectID"].ToString(), out ProjectID))
                        {
                            System.Data.DataRow R = this.dataSetAnalysis1.ProjectAnalysis.NewRow();
                            System.Data.DataRow Rlist = this.dataSetAnalysis1.ProjectAnalysisList.NewProjectAnalysisListRow();
                            R["AnalysisID"] = this.ID;
                            R["ProjectID"] = ProjectID.ToString();
                            Rlist["AnalysisID"] = this.ID;
                            Rlist["Project"] = KV.Key;
                            Rlist["ProjectID"] = ProjectID.ToString();
                            try
                            {
                                this.dataSetAnalysis1.ProjectAnalysis.Rows.Add(R);
                                this.dataSetAnalysis1.ProjectAnalysisList.Rows.Add(Rlist);
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private void toolStripButtonProjectsNew_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT P.Project, P.ProjectID " +
                "FROM ProjectList P " +
                "WHERE P.ProjectID NOT IN (SELECT ProjectID FROM ProjectAnalysis WHERE AnalysisID = " + this.ID.ToString() + ") " +
                "ORDER BY P.Project";
            System.Data.DataTable dtProject = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtProject);
            System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation("CollectionProject.Project");
            string Header = "Please select the project that should be added to the analysis";
            try { Header = DiversityCollection.FormAnalysisText.Please_select_the_project_that; }
            catch { }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity), Header);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataRow R = this.dataSetAnalysis1.ProjectAnalysis.NewRow();
                System.Data.DataRow Rlist = this.dataSetAnalysis1.ProjectAnalysisList.NewProjectAnalysisListRow();
                R["AnalysisID"] = this.ID;
                R["ProjectID"] = int.Parse(f.SelectedValue.ToString());
                Rlist["AnalysisID"] = this.ID;
                Rlist["Project"] = f.SelectedString;
                Rlist["ProjectID"] = int.Parse(f.SelectedValue.ToString());
                try
                {
                    this.dataSetAnalysis1.ProjectAnalysis.Rows.Add(R);
                    this.dataSetAnalysis1.ProjectAnalysisList.Rows.Add(Rlist);
                }
                catch { }
            }
        }

        private void toolStripButtonProjectsDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int ProjectID = int.Parse(this.listBoxProjects.SelectedValue.ToString());
                System.Data.DataRow[] RR = this.dataSetAnalysis1.ProjectAnalysis.Select("ProjectID = " + ProjectID.ToString());
                if (RR.Length > 0)
                {
                    RR[0].Delete();
                    this.projectAnalysisListBindingSource.RemoveCurrent();
                }
            }
            catch{ }
        }
        
        private void buttonProjectsAddExisting_Click(object sender, EventArgs e)
        {
            string SQL = "INSERT INTO ProjectAnalysis (AnalysisID, ProjectID) SELECT DISTINCT " + this.ID.ToString() + ", ProjectID FROM ProjectList WHERE ProjectID NOT IN " +
                "(SELECT ProjectID FROM ProjectAnalysis WHERE AnalysisID = " + this.ID.ToString() + ")";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
            {
                this._Analysis.setItem(this.ID);
            }
        }

        #endregion

        #region Methods
        
        private void toolStripButtonMethodsAdd_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT [MethodID] ,[DisplayText] " +
                "FROM [dbo].[Method] M WHERE M.OnlyHierarchy = 0 " +
                "AND M.MethodID NOT IN (SELECT MethodID FROM MethodForAnalysis A WHERE A.AnalysisID = " + this.ID.ToString() + ") " +
                "AND (M.ForCollectionEvent = 0 OR M.ForCollectionEvent is null) " +
                "ORDER BY M.DisplayText";
            System.Data.DataTable dtMethod = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtMethod);
            System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation("Method");
            string Header = "Please select the method that should be added to the analysis";
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtMethod, "DisplayText", "MethodID", DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity), Header);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataRow R = this.dataSetAnalysis1.MethodForAnalysis.NewRow();
                System.Data.DataRow Rlist = this.dataSetAnalysis1.MethodForAnalysisList.NewMethodForAnalysisListRow();
                R["AnalysisID"] = this.ID;
                R["MethodID"] = int.Parse(f.SelectedValue.ToString());
                Rlist["AnalysisID"] = this.ID;
                Rlist["DisplayText"] = f.SelectedString;
                Rlist["MethodID"] = int.Parse(f.SelectedValue.ToString());
                try
                {
                    this.dataSetAnalysis1.MethodForAnalysis.Rows.Add(R);
                    this.dataSetAnalysis1.MethodForAnalysisList.Rows.Add(Rlist);
                }
                catch { }
            }
        }

        private void toolStripButtonMethodsDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int MethodID = int.Parse(this.listBoxMethods.SelectedValue.ToString());
                System.Data.DataRow[] RR = this.dataSetAnalysis1.MethodForAnalysis.Select("MethodID = " + MethodID.ToString());
                if (RR.Length > 0)
                {
                    RR[0].Delete();
                    this.methodForAnalysisListBindingSource.RemoveCurrent();
                }
            }
            catch { }
        }
        
        #endregion

        #region Results

        private void toolStripButtonResultNew_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation("AnalysisResult.AnalysisResult");
                string Header = "Please enter the value for the new result";
                try { Header = DiversityCollection.FormAnalysisText.Please_enter_the_value_for_the_new_result; }
                catch { }
                DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString(DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity), Header, "");//, this.Message("Please_enter_the_value_for_the_new_result"), "");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    string Result = f.String;
                    System.Data.DataRow[] rr = this.dataSetAnalysis1.AnalysisResult.Select("AnalysisResult = '" + Result + "'");
                    if (rr.Length == 0)
                    {
                        DiversityCollection.Datasets.DataSetAnalysis.AnalysisResultRow R = this.dataSetAnalysis1.AnalysisResult.NewAnalysisResultRow();
                        R.AnalysisID = this.ID;
                        R.AnalysisResult = Result;
                        R.DisplayText = Result;
                        this.dataSetAnalysis1.AnalysisResult.Rows.Add(R);
                    }
                    else
                    {
                        string Message = "This entry allready exists";
                        try { Message = DiversityCollection.FormAnalysisText.This_entry_allready_exists; }
                        catch { }
                        System.Windows.Forms.MessageBox.Show(Message + ": '" + Result + "'"); // this.Message("This_entry_allready_exists") + ": '" + Result + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonResultDelete_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewResult.SelectedCells.Count > 0)
            {
                this.dataSetAnalysis1.AnalysisResult.Rows[this.dataGridViewResult.SelectedCells[0].RowIndex].Delete();
            }
        }
        
        private void toolStripButtonViewResultList_Click(object sender, EventArgs e)
        {
            string Title = "Result list";
            if (this.textBoxDisplayText.Text.Length > 0)
                Title = this.textBoxDisplayText.Text;
            DiversityWorkbench.Forms.FormTableContent f = new DiversityWorkbench.Forms.FormTableContent(Title, "Result list for the analysis " + this.textBoxDisplayText.Text, this.dataSetAnalysis1.AnalysisResult);
            f.ShowDialog();
        }

        #endregion

        #region Unit

        private void comboBoxMeasurementUnit_DropDown(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxMeasurementUnit.DataSource == null)
                {
                    System.Data.DataTable dt = new DataTable();
                    string SQL = "SELECT NULL AS MeasurementUnit UNION " +
                        "SELECT 'DNA' AS MeasurementUnit UNION " +
                        "SELECT DISTINCT MeasurementUnit " +
                        "FROM Analysis " +
                        "ORDER BY MeasurementUnit";
                    string Message = "";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                    this.comboBoxMeasurementUnit.DataSource = dt;
                    this.comboBoxMeasurementUnit.DisplayMember = "MeasurementUnit";
                    this.comboBoxMeasurementUnit.ValueMember = "MeasurementUnit";
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
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