using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormProcessing : Form
    {
        #region Parameter
        private DiversityCollection.Processing _Processing;
        private System.Data.DataTable _DtMaterialCategory;
        
        #endregion
        
        #region Construction

        public FormProcessing()
        {
            InitializeComponent();
            this.splitContainerData.Panel2Collapsed = true;
            this.splitContainerMain.Panel2.Visible = false;

            //online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
            this.initForm();
        }

        public FormProcessing(int? ItemID) : this()
        {
            if (ItemID != null)
                this._Processing.setItem((int)ItemID);
            this.userControlDialogPanel.Visible = true;
            this.panelHeader.Visible = true;
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                System.Data.DataSet Dataset = this.dataSetProcessing;
                if (this._Processing == null)
                    this._Processing = new Processing(ref Dataset, this.dataSetProcessing.Processing,
                        ref this.treeViewProcessing, this, this.userControlQueryList, this.splitContainerMain,
                        this.splitContainerData, this.toolStripButtonSpecimenList, //this.imageListSpecimenList,
                        this.userControlSpecimenList, this.helpProvider, this.toolTip, ref this.processingBindingSource);
                this._Processing.initForm();
                this._Processing.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
                this._Processing.setToolStripButtonNewEvent(this.toolStripButtonNew);
                this.userControlQueryList.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_AddProject);
                this._Processing.setToolStripButtonIncludeIDEvent(this.toolStripButtonIncludeID);
                this.userControlSpecimenList.toolStripButtonDelete.Visible = false;
                this.setPermissions();

                this.userControlQueryList.RememberSettingIsAvailable(true);
                this.userControlQueryList.RememberQuerySettingsIdentifier = "Processing";
                this.userControlQueryList.RememberQueryConditionSettings_ReadFromFile();

            }
            catch (System.Exception ex)
            {
            }
        }
        
        private void FormProcessing_FormClosing(object sender, FormClosingEventArgs e)
        {
            this._Processing.saveItem();

            if (this.userControlQueryList.RememberQuerySettings())
                this.userControlQueryList.RememberQueryConditionSettings_SaveToFile();
            else
                this.userControlQueryList.RememberQueryConditionSettings_RemoveFile();
        }

        private void setPermissions()
        {
            DiversityWorkbench.Forms.FormFunctions FF = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
            this._Processing.setToolbarPermission(ref this.toolStripButtonDelete, "Processing", "Delete");
            this._Processing.setToolbarPermission(ref this.toolStripButtonNew, "Processing", "Insert");

            System.Collections.Generic.List<System.Windows.Forms.Control> ControlList = new List<Control>();
            System.Collections.Generic.List<System.Windows.Forms.ToolStripItem> TSIlist = new List<ToolStripItem>();
            TSIlist.Add(this.toolStripButtonMaterialCategoryNew);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ProcessingMaterialCategory", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonMaterialCategoryDelete);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ProcessingMaterialCategory", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonProjectNew);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ProjectProcessing", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Insert);
            TSIlist.Clear();
            TSIlist.Add(this.toolStripButtonProjectRemove);
            FF.setObjectsAccordingToPermission(ControlList, TSIlist, "ProjectProcessing", DiversityWorkbench.Forms.FormFunctions.DatabaseGrant.Delete);
        }

        private void toolStripButtonNew_AddProject(object sender, EventArgs e)
        {
            if (this.userControlQueryList.ProjectIsSelected)
            {
                System.Data.DataRow R = this.dataSetProcessing.ProjectProcessing.NewRow();
                System.Data.DataRow Rlist = this.dataSetProcessing.ProjectProcessingList.NewProjectProcessingListRow();
                R["ProcessingID"] = this.ID;
                R["ProjectID"] = this.userControlQueryList.ProjectID;
                Rlist["ProcessingID"] = this.ID;
                Rlist["Project"] = DiversityCollection.LookupTable.ProjectName(this.userControlQueryList.ProjectID);
                Rlist["ProjectID"] = this.userControlQueryList.ProjectID; ;
                try
                {
                    this.dataSetProcessing.ProjectProcessing.Rows.Add(R);
                    this.dataSetProcessing.ProjectProcessingList.Rows.Add(Rlist);
                }
                catch { }
            }
        }

        private void FormProcessing_Load(object sender, EventArgs e)
        {
        }

        #endregion

        #region Processing
        //private void setProcessing(int ProcessingID)
        //{
        //    if (this.dataSetProcessing.Processing.Rows.Count > 0)
        //    {
        //        this.updateProcessing();
        //    }
        //    this.fillProcessing(ProcessingID);
        //    if (this.dataSetProcessing.Processing.Rows.Count > 0)
        //    {
        //        this.splitContainerMain.Panel2.Visible = true;
        //    }
        //    else
        //    {
        //        this.splitContainerMain.Panel2.Visible = false;
        //    }
        //}

        //private void updateProcessing()
        //{
        //    this._Processing.saveItem();
        //    //this.FormFunctions.updateTable(this.dataSetProcessing, "Processing", this.sqlDataAdapterLookupTable, this.BindingContext);
        //}

        //private void fillProcessing(int ID)
        //{
        //    try
        //    {
        //        this.dataSetProcessing.Processing.Clear();
        //        this.treeViewProcessing.Nodes.Clear();
        //        //string WhereClause = " WHERE 0 = 2";
        //        //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterLookupTable, this.sqlProcessing + WhereClause, this.dataSetProcessing.Processing);
        //        //string SQL = "SELECT ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI FROM dbo.ProcessingHierarchy (" + ID.ToString() + ")";
        //        //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //        //ad.Fill(this.dataSetProcessing.Processing);
        //        this._Processing.fillItem(ID);
        //        this._Processing.buildHierarchy();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the specimen", System.Windows.Forms.MessageBoxButtons.OK);
        //    }
        //}

        //private void textBoxDisplayText_Leave(object sender, EventArgs e)
        //{
        //}

        #endregion

        #region Query

        //private void initQuery()
        //{
        //    this.userControlQueryList.toolStripButtonConnection.Visible = false;
        //    this.userControlQueryList.toolStripButtonSwitchOrientation.Visible = false;
        //    this.userControlQueryList.toolStripSeparator1.Visible = false;
        //    this.setQueryControlEvents();
        //    //this.buildQueryConditions();
        //    this.setSearchOptions();
        //    this.setQueryDisplayColumns();
        //}

        //private void setQueryDisplayColumns()
        //{
        //    //DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
        //    //QueryDisplayColumns[0].DisplayText = "Processing";
        //    //QueryDisplayColumns[0].DisplayColumn = "DisplayText";
        //    //QueryDisplayColumns[0].OrderColumn = "DisplayText";
        //    //QueryDisplayColumns[0].IdentityColumn = "ProcessingID";
        //    //QueryDisplayColumns[0].TableName = "Processing";
        //    //this.userControlQueryList.QueryDisplayColumns = QueryDisplayColumns;
        //    this.userControlQueryList.QueryDisplayColumns = this._Processing.QueryDisplayColumns;
        //}

        //private void buildQueryConditions()
        //{
        //    this.QueryConditions = new DiversityWorkbench.QueryCondition[4];

        //    string Description = this.FormFunctions.ColumnDescription("Processing", "DisplayText");
        //    DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "Processing", "ProcessingID", "DisplayText", "Processing", "Display", "Display text", Description);
        //    this.QueryConditions[0] = q0;

        //    Description = this.FormFunctions.ColumnDescription("Processing", "Description");
        //    DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "Processing", "ProcessingID", "Description", "Processing", "Description", "Description", Description);
        //    this.QueryConditions[1] = q1;

        //    Description = this.FormFunctions.ColumnDescription("Processing", "Notes");
        //    DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "Processing", "ProcessingID", "Notes", "Processing", "Notes", "Notes", Description);
        //    this.QueryConditions[2] = q2;

        //    Description = this.FormFunctions.ColumnDescription("Processing", "ProcessingURI");
        //    DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "Processing", "ProcessingID", "ProcessingURI", "Processing", "URI", "URI", Description);
        //    this.QueryConditions[3] = q3;
        //}

        //private void setSearchOptions()
        //{
        //    this.userControlQueryList.setQueryConditions(this._Processing.QueryConditions);
        //}

        //private void setQueryControlEvents()
        //{
        //    try
        //    {
        //        // QueryList
        //        this.userControlQueryList.toolStripButtonCopy.Click += new System.EventHandler(this.copyProcessing);
        //        this.userControlQueryList.toolStripButtonDelete.Click += new System.EventHandler(this.deleteProcessing);
        //        this.userControlQueryList.toolStripButtonNew.Click += new System.EventHandler(this.createNewProcessing);
        //        this.userControlQueryList.toolStripButtonSave.Click += new System.EventHandler(this.saveProcessing);
        //        this.userControlQueryList.toolStripButtonUndo.Click += new System.EventHandler(this.undoChangesInProcessing);
        //        this.userControlQueryList.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResult_SelectedIndexChanged);
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void listBoxQueryResult_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        //        this.SuspendLayout();
        //        this.setProcessing(this.userControlQueryList.ID);
        //        this.ResumeLayout();
        //        this.Cursor = System.Windows.Forms.Cursors.Default;
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}


        #endregion

        #region URI
        private void buttonUriOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxURI.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                int i = this.processingBindingSource.Position;
                this.dataSetProcessing.Processing.Rows[i]["ProcessingURI"] = f.URL;
                this.textBoxURI.Text = f.URL;
            }
        }
        
        #endregion

        #region toolStrip

        #endregion

        #region Hierarchy

        #endregion

        #region Properties
        public int ID { get { return int.Parse(this.dataSetProcessing.Processing.Rows[this.processingBindingSource.Position][0].ToString()); } }
        public string DisplayText { get { return this.dataSetProcessing.Processing.Rows[this.processingBindingSource.Position][2].ToString(); } }
        public bool ChangeToSpecimen { get { return this.userControlSpecimenList.ChangeToSpecimen; } }
        public int CollectionSpecimenID { get { return this.userControlSpecimenList.CollectionSpecimenID; } }

        #region URI
        private void textBoxURI_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxURI.Text.Length > 0)
            {
                this.webBrowserURI.Visible = true;
                try
                {
                    System.Uri URI = new Uri(this.textBoxURI.Text);
                    this.webBrowserURI.Url = URI;
                }
                catch { }
            }
            else
                this.webBrowserURI.Visible = false;
        }
        
        #endregion

        #region Material Category
        private void toolStripButtonMaterialCategoryNew_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dt = this.DtMaterialCategory.Copy();
            foreach (System.Data.DataRow R in this.dataSetProcessing.ProcessingMaterialCategory.Rows)
            {
                foreach (System.Data.DataRow RT in dt.Rows)
                {
                    if ((R.RowState == DataRowState.Unchanged || R.RowState == DataRowState.Added) && RT.RowState == DataRowState.Unchanged)
                    {
                        if (RT[0].ToString() == R[1].ToString())
                        {
                            RT.BeginEdit();
                            RT.Delete();
                            RT.EndEdit();
                        }
                    }
                }
            }
            dt.AcceptChanges();
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Select a material category");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataRow R = this.dataSetProcessing.ProcessingMaterialCategory.NewRow();
                R["ProcessingID"] = this.ID;
                R["MaterialCategory"] = f.SelectedString;
                try
                {
                    R.BeginEdit();
                    this.dataSetProcessing.ProcessingMaterialCategory.Rows.Add(R);
                    R.EndEdit();
                }
                catch { }
            }
        }

        private void toolStripButtonMaterialCategoryDelete_Click(object sender, EventArgs e)
        {
            this.processingMaterialCategoryBindingSource.RemoveCurrent();
        }

        public System.Data.DataTable DtMaterialCategory
        {
            get
            {
                if (this._DtMaterialCategory == null)
                {
                    this._DtMaterialCategory = new DataTable();
                    string SQL = "SELECT Code FROM CollMaterialCategory_Enum ORDER BY Code";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._DtMaterialCategory);
                }
                return _DtMaterialCategory;
            }
        }

             
        #endregion

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
        
        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Title = "History of " + this.dataSetProcessing.Processing.Rows[0]["DisplayText"].ToString() + " (ProcessingID: " + this.dataSetProcessing.Processing.Rows[0]["ProcessingID"].ToString() + ")";
            try
            {
                bool HistoryPresent = false;
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();

                if (this.dataSetProcessing.Processing.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProcessingID", this.dataSetProcessing.Processing.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetProcessing.ProcessingMaterialCategory.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "ProcessingID", this.dataSetProcessing.ProcessingMaterialCategory.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetProcessing.ProjectProcessing.Rows.Count > 0)
                {
                    LogTables.Add(this.dtProjectProcessingHistory);
                    HistoryPresent = true;
                }
                if (this.dataSetProcessing.MethodForProcessing.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "MethodID", this.dataSetProcessing.MethodForProcessing.TableName, ""));
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

        private System.Data.DataTable dtProjectProcessingHistory
        {
            get
            {
                System.Data.DataTable dtProjectProcessing = new DataTable("ProjectProcessing");
                string SqlCurrent = "SELECT P.Project, A.LogUpdatedWhen AS [Date of change], A.LogUpdatedBy AS [Responsible user] " +
                    "FROM ProjectProcessing AS A INNER JOIN " +
                    "ProjectProxy AS P ON A.ProjectID = P.ProjectID WHERE A.ProcessingID = " + this.ID.ToString();
                try
                {
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlCurrent, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(dtProjectProcessing);
                }

                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                return dtProjectProcessing;
            }
        }

        private void toolStripMenuItemFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private void tableEditorToolStripMenuItem_Click(object sender, EventArgs e)
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
                DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(DiversityCollection.Resource.Processing, "Processing", "ProcessingID");
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
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "tableEditorToolStripMenuItem_Click");
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region Projects

        private void toolStripButtonProjectNew_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT P.Project, P.ProjectID " +
                "FROM ProjectList P " +
                "WHERE P.ProjectID NOT IN (SELECT ProjectID FROM ProjectProcessing WHERE ProcessingID = " + this.ID.ToString() + ") " +
                "ORDER BY P.Project";
            System.Data.DataTable dtProject = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtProject);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtProject, "Project", "ProjectID", "New Project", "Please select the project that should be added to the analysis");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataRow R = this.dataSetProcessing.ProjectProcessing.NewRow();
                System.Data.DataRow Rlist = this.dataSetProcessing.ProjectProcessingList.NewProjectProcessingListRow();
                R["ProcessingID"] = this.ID;
                R["ProjectID"] = int.Parse(f.SelectedValue.ToString());
                Rlist["ProcessingID"] = this.ID;
                Rlist["Project"] = f.SelectedString;
                Rlist["ProjectID"] = int.Parse(f.SelectedValue.ToString());
                try
                {
                    this.dataSetProcessing.ProjectProcessing.Rows.Add(R);
                    this.dataSetProcessing.ProjectProcessingList.Rows.Add(Rlist);
                }
                catch { }
            }
        }

        private void toolStripButtonProjectRemove_Click(object sender, EventArgs e)
        {
            try
            {
                int ProjectID = int.Parse(this.listBoxProjects.SelectedValue.ToString());
                System.Data.DataRow[] RR = this.dataSetProcessing.ProjectProcessing.Select("ProjectID = " + ProjectID.ToString());
                if (RR.Length > 0)
                {
                    RR[0].Delete();
                    this.projectProcessingListBindingSource.RemoveCurrent();
                }
            }
            catch { }
        }
        
        #endregion

        #region Methods

        private void toolStripButtonMethodsAdd_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT [MethodID] ,[DisplayText] " +
                "FROM [dbo].[Method] M WHERE M.OnlyHierarchy = 0 " +
                "AND M.MethodID NOT IN (SELECT MethodID FROM MethodForProcessing A WHERE A.ProcessingID = " + this.ID.ToString() + ") " +
                "AND (M.ForCollectionEvent = 0 OR M.ForCollectionEvent is null) " +
                "ORDER BY M.DisplayText";
            System.Data.DataTable dtMethod = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtMethod);
            System.Collections.Generic.Dictionary<string, string> Entity = DiversityWorkbench.Entity.EntityInformation("Method");
            string Header = "Please select the method that should be added to the processing";
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtMethod, "DisplayText", "MethodID", DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, Entity), Header);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataRow R = this.dataSetProcessing.MethodForProcessing.NewRow();
                System.Data.DataRow Rlist = this.dataSetProcessing.MethodForProcessingList.NewMethodForProcessingListRow();
                R["ProcessingID"] = this.ID;
                R["MethodID"] = int.Parse(f.SelectedValue.ToString());
                Rlist["ProcessingID"] = this.ID;
                Rlist["DisplayText"] = f.SelectedString;
                Rlist["MethodID"] = int.Parse(f.SelectedValue.ToString());
                try
                {
                    this.dataSetProcessing.MethodForProcessing.Rows.Add(R);
                    this.dataSetProcessing.MethodForProcessingList.Rows.Add(Rlist);
                }
                catch { }
            }
        }

        private void toolStripButtonMethodsDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // check CollectionSpecimenProcessingMethodParameter
                string SQL = "SELECT COUNT(*) FROM CollectionSpecimenProcessingMethodParameter AS P WHERE ProcessingID = " + ID.ToString();
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show(Result + " datasets in table CollectionSpecimenProcessingMethodParameter are linked to the parameters of this method. Do you want to remove these data?", "Remove parameter?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SQL = "DELETE FROM CollectionSpecimenProcessingMethodParameter WHERE ProcessingID = " + ID.ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                    else
                        return;
                }

                // check CollectionSpecimenProcessingMethod
                SQL = "SELECT COUNT(*) FROM CollectionSpecimenProcessingMethod AS M WHERE ProcessingID = " + ID.ToString();
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show(Result + " datasets in table CollectionSpecimenProcessingMethod are linked to this method. Do you want to remove these data?", "Remove Method?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        SQL = "DELETE FROM CollectionSpecimenProcessingMethod WHERE ProcessingID = " + ID.ToString();
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                    else
                        return;
                }

                // check MethodForProcessing
                //SQL = "SELECT COUNT(*) FROM MethodForProcessing AS M WHERE ProcessingID = " + ID.ToString();
                //Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                //if (Result != "0")
                //{
                //    if (System.Windows.Forms.MessageBox.Show(Result + " datasets in table MethodForProcessing are linked to this method. Do you want to remove these data?", "Remove method?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                //    {
                //        SQL = "DELETE FROM MethodForProcessing WHERE ProcessingID = " + ID.ToString();
                //        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                //    }
                //    else
                //        return;
                //}

                int MethodID = int.Parse(this.listBoxMethods.SelectedValue.ToString());
                System.Data.DataRow[] RR = this.dataSetProcessing.MethodForProcessing.Select("MethodID = " + MethodID.ToString());
                if (RR.Length > 0)
                {
                    RR[0].Delete();
                    this.methodForProcessingListBindingSource.RemoveCurrent();
                }
            }
            catch { }
        }

        #endregion

        #region Datahandling
        //private void copyProcessing(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        if (this.dataSetProcessing.Processing.Rows.Count > 0)
        //        {
        //            System.Data.DataRow R = this.dataSetProcessing.Processing.Rows[this.processingBindingSource.Position];
        //            string DisplayText = R["DisplayText"].ToString();
        //            if (System.Windows.Forms.MessageBox.Show("Do you want to create a copy of\r\n\r\n" + DisplayText, "Create copy?", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //            {
        //                int ID = this.CreateNewProcessing(null, R);
        //                //this.userControlQueryList.AddListItem(ID, DisplayText);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void deleteProcessing(object sender, System.EventArgs e)
        //{
        //    if (this.dataSetProcessing.Processing.Rows.Count > 0)
        //    {
        //        int ID = int.Parse(this.dataSetProcessing.Processing.Rows[0][0].ToString());
        //        //string SQL = "NameID = " + ID.ToString();
        //        string DisplayTextDel = this.dataSetProcessing.Processing.Rows[0]["DisplayText"].ToString();
        //        try
        //        {
        //            if (System.Windows.Forms.MessageBox.Show("Do you want to delete the analysis\r\n\r\n" + DisplayTextDel, "Delete analysis?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
        //            {
        //                this.userControlQueryList.RemoveSelectedListItem();
        //                this.processingBindingSource.RemoveCurrent();
        //                this._Processing.saveItem();
        //                //this.deleteTaxon(ID);
        //                //this.dataSetTaxonName.Clear();
        //                this.splitContainerMain.Panel2.Visible = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //    }
        //    else
        //    {
        //        System.Windows.Forms.MessageBox.Show("Please select an analysis");
        //    }
        //}

        //private void createNewProcessing(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        int ID = this.CreateNewProcessing(null, null);
        //        this.userControlQueryList.AddListItem(ID, "New Processing");
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void saveProcessing(object sender, System.EventArgs e)
        //{
        //    this._Processing.saveItem();
        //    if (this.userControlQueryList.ID.ToString() == this.dataSetProcessing.Processing.Rows[processingBindingSource.Position][0].ToString())
        //    {
        //        if (this.dataSetProcessing.Processing.Rows[this.processingBindingSource.Position]["DisplayText"].ToString() != userControlQueryList.DisplayTextSelectedItem)
        //            this.userControlQueryList.DisplayTextSelectedItem = this.dataSetProcessing.Processing.Rows[this.processingBindingSource.Position]["DisplayText"].ToString();
        //    }
        //}

        //private void undoChangesInProcessing(object sender, System.EventArgs e)
        //{

        //    int ID = int.Parse(this.dataSetProcessing.Processing.Rows[0][0].ToString());
        //    this.dataSetProcessing.Clear();
        //    this._Processing.setItem(ID);
        //}

        //private int CreateNewProcessing(int? ParentID, System.Data.DataRow R)
        //{
        //    //object[] rowVals = new object[2];
        //    string DisplayText = "";
        //    string SQL = "INSERT INTO Processing (ProcessingParentID, DisplayText, Description, Notes, ProcessingURI) " +
        //        "VALUES (";
        //    if (ParentID != null)
        //    {
        //        SQL += ParentID.ToString() + ", ";
        //        //rowVals[1] = ParentID;
        //    }
        //    else
        //    {
        //        SQL += " NULL,  ";
        //    }
        //    if (R == null)
        //    {
        //        DisplayText = "New Processing";
        //        SQL += " '" + DisplayText + "', NULL, NULL, NULL ";
        //    }
        //    else
        //    {
        //        DisplayText = "Copy of " + R[2].ToString();
        //        SQL += "'" + DisplayText + "', ";
        //        if (R[3].Equals(System.DBNull.Value)) SQL += " NULL, ";
        //        else SQL += "'" + R[3].ToString() + "', ";
        //        if (R[4].Equals(System.DBNull.Value)) SQL += " NULL, ";
        //        else SQL += "'" + R[4].ToString() + "', ";
        //        if (R[5].Equals(System.DBNull.Value)) SQL += " NULL ";
        //        else SQL += "'" + R[5].ToString() + "'";
        //    }
        //    SQL += ") SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
        //    Microsoft.Data.SqlClient.SqlConnection c = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, c);
        //    c.Open();
        //    int ID = int.Parse(cmd.ExecuteScalar().ToString());
        //    c.Close();
        //    //rowVals[0] = ID;
        //    //rowVals[2] = DisplayText;
        //    this.userControlQueryList.AddListItem(ID, DisplayText);
        //    //if (R != null)
        //    //{
        //    //    rowVals[1] = R[1];
        //    //    rowVals[2] = "Copy of " + R[2].ToString();
        //    //    rowVals[3] = R[3];
        //    //    rowVals[4] = R[4];
        //    //    rowVals[5] = R[5];
        //    //}
        //    //this.dataSetProcessing.Processing.Rows.Add(rowVals);
        //    return ID;


        //    //object[] rowVals = new object[6];
        //    //string SQL = "INSERT INTO Processing (DisplayText, ProcessingParentID) " +
        //    //    "VALUES ('New Processing', ";
        //    //if (ParentID != null)
        //    //{
        //    //    SQL += ParentID.ToString();
        //    //    rowVals[1] = ParentID;
        //    //}
        //    //else
        //    //{
        //    //    SQL += " NULL ";
        //    //}
        //    //SQL += ") SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
        //    //Microsoft.Data.SqlClient.SqlConnection c = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //    //Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, c);
        //    //c.Open();
        //    //int ID = int.Parse(cmd.ExecuteScalar().ToString());
        //    //c.Close();
        //    //rowVals[0] = ID;
        //    //rowVals[2] = "New Processing";
        //    //if (R != null)
        //    //{
        //    //    rowVals[1] = R[1];
        //    //    rowVals[2] = "Copy of " + R[2].ToString();
        //    //    rowVals[3] = R[3];
        //    //    rowVals[4] = R[4];
        //    //    rowVals[5] = R[5];
        //    //}
        //    //this.dataSetProcessing.Processing.Rows.Add(rowVals);
        //    //return ID;
        //}
        
        #endregion

    }
}