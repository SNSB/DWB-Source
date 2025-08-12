using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormCollectionEvent : Form, DiversityCollection.UserControls.iMainForm
    {
        #region Parameter

        private DiversityCollection.CollectionEvent _CollectionEvent;
        private bool _ShowDependentData;
        private bool _ReadOnly;
        private bool _ShowQuery;
        
        #endregion

        #region Construction

        public FormCollectionEvent()
        {
            try
            {
                InitializeComponent();
                this.splitContainerData.Panel2Collapsed = true;
                this.splitContainerMain.Panel2.Visible = false;
                this.initForm();
                this.panelHeader.Visible = true;
                this.userControlDialogPanel.Visible = true;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public FormCollectionEvent(int? ItemID)
            : this()
        {
            if (ItemID != null)
                this._CollectionEvent.setItem((int)ItemID);
            this.userControlDialogPanel.Visible = true;
            this.panelHeader.Visible = true;
        }

        public FormCollectionEvent(int ItemID, bool ReadOnly = false, bool ShowQuery = true, bool ShowDependentData = false)
        {
            try
            {
                InitializeComponent();
                this.ShowInTaskbar = true;
                this.treeViewDependent.ImageList = DiversityCollection.Specimen.ImageList;
                System.Windows.Forms.TreeView tv = new TreeView();
                System.Data.DataSet Dataset = this.dataSetCollectionSpecimen;
                if (this._CollectionEvent == null)
                    this._CollectionEvent = new CollectionEvent(ref Dataset, this.dataSetCollectionSpecimen.CollectionEvent,
                        ref tv, this, this.userControlQueryList,
                        this.splitContainerMain,
                        this.splitContainerData, null, this.userControlSpecimenList,
                        this.helpProvider, this.toolTip, ref this.collectionEventBindingSource);
                this._CollectionEvent.initForm();
                //this.initRemoteModules();
                this.setUserControlDatabindings();
                //this.userControlSpecimenList.toolStripButtonDelete.Visible = false;

                this._CollectionEvent.setItem((int)ItemID);
                this.userControlDialogPanel.Visible = true;
                this.panelHeader.Visible = true;
                this.splitContainerMain.Panel1Collapsed = !ShowQuery;
                this.splitContainerDataDependent.Panel2Collapsed = !ShowDependentData;
                if (ReadOnly)
                {
                    this.FormFunctions.setDataControlEnabled(this.splitContainerOverviewEventData.Panel1, false);
                    this.FormFunctions.setDataControlEnabled(this.splitContainerOverviewEventDescriptions, false);
                }
                this._ShowDependentData = ShowDependentData;
                this._ReadOnly = ReadOnly;
                this._ShowQuery = ShowQuery;
                if (this._ShowDependentData)
                {
                    this.fillDependentData();
                    this.buildOverviewHierarchyUnits();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Interface

        public void ShowDependentData(bool Show)
        {
            this._ShowDependentData = Show;
            this.initDependentData();
        }

        public CollectionSpecimen.AvailabilityState Availability()
        {
            return CollectionSpecimen.AvailabilityState.Unknown;
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                this.initQueryOptimizing();
                this.initUserControls();
                System.Windows.Forms.TreeView tv = new TreeView();
                System.Data.DataSet Dataset = this.dataSetCollectionSpecimen;
                if (this._CollectionEvent == null)
                    this._CollectionEvent = new CollectionEvent(ref Dataset, this.dataSetCollectionSpecimen.CollectionEvent,
                        ref tv, this, this.userControlQueryList, this.splitContainerMain,
                        this.splitContainerData, null, //this.imageListSpecimenList,
                        this.userControlSpecimenList, this.helpProvider, this.toolTip, ref this.collectionEventBindingSource);
                this._CollectionEvent.initForm();
                this.initRemoteModules();
                this.setUserControlDatabindings();
                this.userControlSpecimenList.toolStripButtonDelete.Visible = false;

                this.userControlQueryList.RememberSettingIsAvailable(true);
                this.userControlQueryList.RememberQuerySettingsIdentifier = "FormCollectionEvent";
                this.userControlQueryList.RememberQueryConditionSettings_ReadFromFile();

                this.userControlQueryList.setModuleAndForm(DiversityWorkbench.Settings.ModuleName, "FormCollectionEvent");
                this.userControlQueryList.ManyOrderByColumns_Allow(true);
                this.userControlQueryList.OptimizingAvailable(true);
                this.userControlQueryList.ManyOrderByColumns_InitControls();

                //this.userControlQueryList.AllowManyOrderByColumns(true, "FormCollectionEvent");
                //this.userControlQueryList.AllowOptimizing(true);
                //this.userControlQueryList.RememberSettingIsAvailable(true);

#if DEBUG
                this.userControlQueryList.listBoxQueryResult.SelectedIndexChanged  += new System.EventHandler(this.SetPosition);
#endif
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void initQueryOptimizing()
        {
            DiversityWorkbench.UserControls.UserControlQueryList.QueryMainTable = "CollectionEvent_Core2";
            this.userControlQueryList.QueryMainTableLocal = "CollectionEvent_Core2";
        }

        private void initUserControls()
        {
            this.userControl_Event = new UserControls.UserControl_Event(this, this.collectionEventBindingSource, this.helpProvider.HelpNamespace);
            //this.userControl_Event.setSource(this.collectionEventBindingSource); // waere doppelt
            this.userControl_Event.setAvailability();
            this.userControl_Event.SetPosition(0);
#if !DEBUG
            this.splitContainerSeriesEvent.Panel1Collapsed = true;
#else
            this.splitContainerSeriesEvent.Panel1Collapsed = true;//false;// true;//
#endif
        }

        private void SetPosition(object sender, EventArgs e)
        {
            this.userControl_Event.SetPosition(0);
        }



        private void setUserControlDatabindings()
        {
            try
            {
                // reference
                DiversityWorkbench.Reference L = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryEventReference.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)L;

                this.userControlModuleRelatedEntryEventReference.bindToData("CollectionEvent", "ReferenceTitle", "ReferenceURI", this.collectionEventBindingSource);

                this.userControlModuleRelatedEntryEventReference.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;

                // date
                this.userControlDatePanelEventDate.setDataBindings(this.collectionEventBindingSource, "CollectionDay", "CollectionMonth", "CollectionYear", "CollectionDateSupplement");

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FormCollectionEvent_Load(object sender, EventArgs e)
        {
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimen.CollectionEventParameterValue". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionEventParameterValueTableAdapter.Fill(this.dataSetCollectionSpecimen.CollectionEventParameterValue);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimen.CollectionEventMethod". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionEventMethodTableAdapter.Fill(this.dataSetCollectionSpecimen.CollectionEventMethod);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimen.CollectionEventImage". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionEventImageTableAdapter.Fill(this.dataSetCollectionSpecimen.CollectionEventImage);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimen.CollectionEventProperty". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionEventPropertyTableAdapter.Fill(this.dataSetCollectionSpecimen.CollectionEventProperty);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimen.CollectionEventLocalisation". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionEventLocalisationTableAdapter.Fill(this.dataSetCollectionSpecimen.CollectionEventLocalisation);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionSpecimen.CollectionEvent". Sie können sie bei Bedarf verschieben oder entfernen.
            ////this.collectionEventTableAdapter.Fill(this.dataSetCollectionSpecimen.CollectionEvent);
        }

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select any data");
                return;
            }
            string Title = "History of event with ID: " + this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionEventID"].ToString();
            try
            {
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();
                LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.ID, "CollectionEventID", this.dataSetCollectionSpecimen.CollectionEvent.TableName, this.dataSetCollectionSpecimen.CollectionSpecimen.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.ID, "CollectionEventID", this.dataSetCollectionSpecimen.CollectionEventLocalisation.TableName, this.dataSetCollectionSpecimen.CollectionEvent.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.ID, "CollectionEventID", this.dataSetCollectionSpecimen.CollectionEventProperty.TableName, this.dataSetCollectionSpecimen.CollectionEvent.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.ID, "CollectionEventID", this.dataSetCollectionSpecimen.CollectionEventMethod.TableName, this.dataSetCollectionSpecimen.CollectionEvent.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.ID, "CollectionEventID", this.dataSetCollectionSpecimen.CollectionEventParameterValue.TableName, this.dataSetCollectionSpecimen.CollectionEvent.TableName));
                LogTables.Add(DiversityWorkbench.Database.DtHistory((int)this.ID, "CollectionEventID", this.dataSetCollectionSpecimen.CollectionEventImage.TableName, this.dataSetCollectionSpecimen.CollectionEvent.TableName));

                // Identifer
                try
                {
                    if (this.dataSetCollectionSpecimen.ExternalIdentifier.Rows.Count > 0)
                        LogTables.Add(DiversityWorkbench.Database.DtHistory(this.dataSetCollectionSpecimen.ExternalIdentifier));
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }


                // Annotation
                try
                {
                    if (this.dataSetCollectionSpecimen.Annotation.Rows.Count > 0)
                        LogTables.Add(DiversityWorkbench.Database.DtHistory(this.dataSetCollectionSpecimen.Annotation));
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }

                DiversityWorkbench.Forms.FormHistory f = new DiversityWorkbench.Forms.FormHistory(Title, LogTables, DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace());

                f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "History");
                f.ShowDialog();
            }

            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());
        }

        private void FormCollectionEvent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.userControlQueryList.RememberQuerySettings())
                this.userControlQueryList.RememberQueryConditionSettings_SaveToFile("FormCollectionEvent");
            else
                this.userControlQueryList.RememberQueryConditionSettings_RemoveFile("FormCollectionEvent");

            if (this.userControlQueryList.ManyOrderByColumns_Allow())
            {
                DiversityWorkbench.Settings.ManyOrderColumnSave(DiversityWorkbench.Settings.ModuleName, "FormCollectionEvent", this.userControlQueryList.ManyOrderByColumns_Widths());
            }

        }

        private void buttonTableEditor_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> ReadOnlyColumns = new List<string>();
            ReadOnlyColumns.Add("CollectionEventID");
            string SQL = this.userControlQueryList.SqlForQuery();
            SQL = "SELECT * FROM CollectionEvent " + SQL.Substring( SQL.IndexOf(" WHERE "));
            if (this.sqlDataAdapterEvent == null)
            {
                this.sqlDataAdapterEvent = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.userControlQueryList.Connection.ConnectionString);
            }
            else this.sqlDataAdapterEvent.SelectCommand.CommandText = SQL;
            DiversityWorkbench.Forms.FormTableEditor formTableEditor = new DiversityWorkbench.Forms.FormTableEditor(DiversityCollection.Resource.Event, this.sqlDataAdapterEvent, ReadOnlyColumns);
            formTableEditor.Width = this.Width - 20;
            formTableEditor.Height = this.Height - 20;
            formTableEditor.StartPosition = FormStartPosition.CenterParent;
            formTableEditor.ShowDialog();
        }


#endregion

#region Modules

        private void initRemoteModules()
        {
            try
            {
                // Agents
                //DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                //this.userControlModuleRelatedEntryAdministrativeContactName.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                //this.userControlModuleRelatedEntryAdministrativeContactName.bindToData("Collection", "AdministrativeContactName", "AdministrativeContactAgentURI", this.collectionEventSeriesBindingSource);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

#endregion

#region Properties

        public int FormWidth() { return this.Width; }
        public int FormHeight() { return this.Height; }


        public int? ID
        {
            get
            {
                if (this.userControlQueryList.listBoxQueryResult.SelectedIndex > -1)
                    return int.Parse(this.dataSetCollectionSpecimen.CollectionEvent.Rows[this.collectionEventBindingSource.Position][0].ToString());
                else
                    return null;
            }
        }
        public string DisplayText { get { return this.dataSetCollectionSpecimen.CollectionEvent.Rows[this.collectionEventBindingSource.Position]["LocalityDescription"].ToString(); } }
        public bool ChangeToSpecimen { get { return this.userControlSpecimenList.ChangeToSpecimen; } }
        public int CollectionSpecimenID { get { return this.userControlSpecimenList.CollectionSpecimenID; } }

        public int? CollectionEventID()
        {
            int ID;
            if (int.TryParse(this.dataSetCollectionSpecimen.CollectionEvent.Rows[this.collectionEventBindingSource.Position]["CollectionEventID"].ToString(), out ID))
                return ID;
            else
                return null;
        }

        public bool setSpecimenPart(int ID) { return false; }

        public int? getSpecimenPartID() { return null; }

#endregion

#region Dependent data
        private void initDependentData()
        {
            this.splitContainerDataDependent.Panel2Collapsed = !this._ShowDependentData;
            if (this._ShowDependentData)
            {
                this.fillDependentData();
                this.buildOverviewHierarchyUnits();
            }
        }

        private void treeViewDependent_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (this.treeViewDependent.SelectedNode != null && this.treeViewDependent.SelectedNode.Tag != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.treeViewDependent.SelectedNode.Tag;
                    this.panelDependent.Controls.Clear();
                    int iPos = 0;
                    switch (R.Table.TableName)
                    {
                        case "CollectionEventLocalisation":
                            iPos = 0;
                            for (int i = 0; i < this.dataSetCollectionSpecimen.CollectionEventLocalisation.Rows.Count; i++)
                            {
                                if (this.dataSetCollectionSpecimen.CollectionEventLocalisation.Rows[i]["LocalisationSystemID"].ToString() == R["LocalisationSystemID"].ToString())
                                {
                                    iPos = i;
                                    break;
                                }
                            }
                            this.UserControl_EventLocality.SetPosition(iPos);
                            this.panelDependent.Controls.Add(this.UserControl_EventLocality);
                            this.UserControl_EventLocality.Dock = DockStyle.Fill;
                            break;
                        case "CollectionEventProperty":
                            iPos = 0;
                            for (int i = 0; i < this.dataSetCollectionSpecimen.CollectionEventProperty.Rows.Count; i++)
                            {
                                if (this.dataSetCollectionSpecimen.CollectionEventProperty.Rows[i]["PropertyID"].ToString() == R["PropertyID"].ToString())
                                {
                                    iPos = i;
                                    break;
                                }
                            }
                            this.UserControl_EventProperty.SetPosition(iPos);
                            this.panelDependent.Controls.Add(this.UserControl_EventProperty);
                            this.UserControl_EventProperty.Dock = DockStyle.Fill;
                            break;
                        case "CollectionEventMethodListxx":
                            iPos = 0;
                            for (int i = 0; i < this.dataSetCollectionSpecimen.CollectionEventMethodList.Rows.Count; i++)
                            {
                                if (this.dataSetCollectionSpecimen.CollectionEventMethodList.Rows[i]["MethodID"].ToString() == R["MethodID"].ToString() &&
                                    this.dataSetCollectionSpecimen.CollectionEventMethodList.Rows[i]["MethodMarker"].ToString() == R["MethodMarker"].ToString())
                                {
                                    iPos = i;
                                    break;
                                }
                            }
                            this.UserControl_Method.SetPosition(iPos);
                            this.panelDependent.Controls.Add(this.UserControl_Method);
                            this.UserControl_Method.Dock = DockStyle.Fill;
                            break;
                        case "CollectionEventImage":
                            System.Windows.Forms.WebBrowser WB = new WebBrowser();
                            WB.Navigate(R["URI"].ToString());
                            this.panelDependent.Controls.Add(WB);
                            WB.Dock = DockStyle.Fill;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEvent;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterEventRegulation;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterLocalisation;
        private Microsoft.Data.SqlClient.SqlDataAdapter sqlDataAdapterProperty;


        private void fillDependentData()
        {
            try
            {
                this.dataSetCollectionSpecimen.CollectionEventImage.Clear();
                this.dataSetCollectionSpecimen.CollectionEventLocalisation.Clear();
                this.dataSetCollectionSpecimen.CollectionEventMethodList.Clear();
                this.dataSetCollectionSpecimen.CollectionEventParameterValueList.Clear();
                this.dataSetCollectionSpecimen.CollectionEventProperty.Clear();
                this.dataSetCollectionSpecimen.CollectionEventRegulation.Clear();

                int EventID = int.Parse(this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["CollectionEventID"].ToString());
                string WhereClause = " WHERE CollectionEventID = " + EventID.ToString();

                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterEventImage, DiversityCollection.CollectionSpecimen.SqlEventImage + WhereClause, this.dataSetCollectionSpecimen.CollectionEventImage);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterLocalisation, DiversityCollection.CollectionSpecimen.SqlEventLocalisation + WhereClause, this.dataSetCollectionSpecimen.CollectionEventLocalisation);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProperty, DiversityCollection.CollectionSpecimen.SqlEventProperty + WhereClause, this.dataSetCollectionSpecimen.CollectionEventProperty);
                this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterEventRegulation, DiversityCollection.CollectionSpecimen.SqlEventRegulation + WhereClause, this.dataSetCollectionSpecimen.CollectionEventRegulation);

                // METHOD
                string SQL = "";
                try
                {
                    this.dataSetCollectionSpecimen.CollectionEventMethodList.Clear();
                    SQL = "SELECT E.CollectionEventID, E.MethodID, M.DisplayText + ' ' + E.MethodMarker AS DisplayText, E.MethodMarker " +
                        "FROM CollectionEventMethod E, Method M  WHERE E.CollectionEventID = " + EventID.ToString() + " AND E.MethodID = M.MethodID";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(this.dataSetCollectionSpecimen.CollectionEventMethodList);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                }

                // METHOD PARAMETER
                try
                {
                    SQL = "SELECT E.CollectionEventID, E.MethodID, E.ParameterID, P.DisplayText, E.Value, E.MethodMarker " +
                        "FROM CollectionEventParameterValue E, Parameter P " +
                        "WHERE E.CollectionEventID = " + EventID.ToString() +
                        " AND E.MethodID = P.MethodID " +
                        " AND E.ParameterID = P.ParameterID ";//" AND E.MethodMarker = P.MethodMarker ";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(this.dataSetCollectionSpecimen.CollectionEventParameterValueList);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buildOverviewHierarchyUnits()
        {
            this.treeViewDependent.Nodes.Clear();
            try
            {
                if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
                {
                    System.Windows.Forms.TreeNode EventNode = this.OverviewHierarchyEventNode;
                    this.treeViewDependent.Nodes.Add(EventNode);
                    this.addOverviewHierarchyDependentData();
                    this.treeViewDependent.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.treeViewDependent.Visible = true;
            }
        }

        private void addOverviewHierarchyDependentData()
        {
            this.addOverviewHierarchyEventLocalisation();
            this.addOverviewHierarchyEventProperty();
            this.addOverviewHierarchyEventMethod();
            this.addOverviewHierarchyEventImage();
            this.addOverviewHierarchyEventRegulation();
        }


        private System.Windows.Forms.TreeNode OverviewHierarchyEventNode
        {
            get
            {
                if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
                {
                    DiversityCollection.HierarchyNode N = new HierarchyNode(this.dataSetCollectionSpecimen.CollectionEvent.Rows[0], false);
                    return N;
                }
                else
                {
                    System.Windows.Forms.TreeNode N = new System.Windows.Forms.TreeNode("?");
                    N.ImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Event;
                    N.SelectedImageIndex = (int)DiversityCollection.Specimen.OverviewImage.Event;
                    return N;
                }
            }
        }

        private void addOverviewHierarchyEventLocalisation()
        {
            try
            {
                System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
                this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeViewDependent, ref Nodes);
                if (Nodes.Count > 0)
                {
                    foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventLocalisation.Rows)
                    {
                        if (R.RowState != DataRowState.Deleted && R.RowState != DataRowState.Detached)
                        {
                            DiversityCollection.HierarchyNode NA = new HierarchyNode(R, false);
                            Nodes[0].Nodes.Add(NA);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void addOverviewHierarchyEventProperty()
        {
            try
            {
                System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
                this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeViewDependent, ref Nodes);
                if (Nodes.Count > 0)
                {
                    foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventProperty.Rows)
                    {
                        if (R.RowState != DataRowState.Detached && R.RowState != DataRowState.Deleted)
                        {
                            DiversityCollection.HierarchyNode NA = new HierarchyNode(R, false);
                            Nodes[0].Nodes.Add(NA);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void addOverviewHierarchyEventMethod()
        {
            try
            {
                System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
                this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeViewDependent, ref Nodes);
                if (Nodes.Count > 0)
                {
                    foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventMethodList.Rows)
                    {
                        if (R.RowState != DataRowState.Detached && R.RowState != DataRowState.Deleted)
                        {
                            DiversityCollection.HierarchyNode NA = new HierarchyNode(R, false);
                            Nodes[0].Nodes.Add(NA);
                            foreach (System.Data.DataRow RP in this.dataSetCollectionSpecimen.CollectionEventParameterValueList.Rows)
                            {
                                if (R["CollectionEventID"].ToString() == RP["CollectionEventID"].ToString() &&
                                    R["MethodID"].ToString() == RP["MethodID"].ToString() &&
                                    R["MethodMarker"].ToString() == RP["MethodMarker"].ToString())
                                {
                                    DiversityCollection.HierarchyNode NP = new HierarchyNode(RP, false);
                                    NA.Nodes.Add(NP);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void addOverviewHierarchyEventImage()
        {
            try
            {
                System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
                this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeViewDependent, ref Nodes);
                if (Nodes.Count > 0)
                {
                    foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventImage.Rows)
                    {
                        if (R.RowState != DataRowState.Detached && R.RowState != DataRowState.Deleted)
                        {
                            DiversityCollection.HierarchyNode NA = new HierarchyNode(R, false);
                            Nodes[0].Nodes.Add(NA);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void addOverviewHierarchyEventRegulation()
        {
            try
            {
                System.Collections.Generic.List<System.Windows.Forms.TreeNode> Nodes = new List<TreeNode>();
                this.getOverviewHierarchyNodes(null, "CollectionEvent", this.treeViewDependent, ref Nodes);
                if (Nodes.Count > 0)
                {
                    foreach (System.Data.DataRow R in this.dataSetCollectionSpecimen.CollectionEventRegulation.Rows)
                    {
                        if (R.RowState != DataRowState.Detached && R.RowState != DataRowState.Deleted)
                        {
                            DiversityCollection.HierarchyNode NA = new HierarchyNode(R, false);
                            Nodes[0].Nodes.Add(NA);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }


        private void getOverviewHierarchyNodes(System.Windows.Forms.TreeNode Node, string Table,
            System.Windows.Forms.TreeView Treeview,
            ref System.Collections.Generic.List<System.Windows.Forms.TreeNode> TreeNodes)
        {
            if (TreeNodes == null) TreeNodes = new List<TreeNode>();
            if (Node == null)
            {
                foreach (System.Windows.Forms.TreeNode N in Treeview.Nodes)
                {
                    if (N.Tag != null)
                    {
                        try
                        {
                            System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                            if (R.Table.TableName == Table)
                                TreeNodes.Add(N);
                            this.getOverviewHierarchyNodes(N, Table, Treeview, ref TreeNodes);
                        }
                        catch { }
                    }
                }
            }
            else
            {
                foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
                {
                    if (N.Tag != null)
                    {
                        try
                        {
                            System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                            if (R.Table.TableName == Table)
                                TreeNodes.Add(N);
                            this.getOverviewHierarchyNodes(N, Table, Treeview, ref TreeNodes);
                        }
                        catch { }
                    }
                }
            }
        }

        private DiversityCollection.UserControls.UserControl_EventLocality _UserControl_EventLocality;
        private DiversityCollection.UserControls.UserControl_EventLocality UserControl_EventLocality
        {
            get
            {
                if (this._UserControl_EventLocality == null)
                    this._UserControl_EventLocality = new UserControls.UserControl_EventLocality(this, this.collectionEventLocalisationBindingSource, this.collectionEventBindingSource, this.helpProvider.HelpNamespace);
                return _UserControl_EventLocality;
            }
        }

        private DiversityCollection.UserControls.UserControl_EventProperty _UserControl_EventProperty;
        private DiversityCollection.UserControls.UserControl_EventProperty UserControl_EventProperty
        {
            get
            {
                if (this._UserControl_EventProperty == null)
                    this._UserControl_EventProperty = new UserControls.UserControl_EventProperty(this, this.collectionEventPropertyBindingSource, this.helpProvider.HelpNamespace);
                return _UserControl_EventProperty;
            }
        }

        private DiversityCollection.UserControls.UserControl_Method _UserControl_Method;
        private DiversityCollection.UserControls.UserControl_Method UserControl_Method
        {
            get
            {
                if (this._UserControl_Method == null)
                    this._UserControl_Method = new UserControls.UserControl_Method(UserControls.UserControl_Method.Target.Event, this, this.collectionEventBindingSource, this.collectionEventMethodBindingSource, this.collectionEventParameterValueBindingSource, this.helpProvider.HelpNamespace);
                return _UserControl_Method;
            }
        }


#region Interface

        public bool ReadOnly()
        {
            if (!this.ClientUpToDate())
                return true;
            if (!this.DataAvailable())
                return true;
            return this._ReadOnly;
        }

        public bool ClientUpToDate()
        {
            return true;
        }

        public bool DataAvailable()
        {
            return false;
        }

        public DiversityCollection.Datasets.DataSetCollectionSpecimen DataSetCollectionSpecimen()
        {
            return this.dataSetCollectionSpecimen;
        }

        public DiversityCollection.Datasets.DataSetCollectionEventSeries DataSetCollectionEventSeries()
        {
            return null;
        }

        public int? ProjectID()
        {
            int? ProjectID = null;
            if (this.userControlQueryList.ProjectIsSelected)
                ProjectID = this.userControlQueryList.ProjectID;
            return ProjectID;
        }

        public void ReleaseImageRestriction()
        {
        }

        public System.Collections.Generic.List<int> SelectedIDs()
        {
            return this.userControlQueryList.ListOfSelectedIDs;
        }

        public void SelectAll()
        {
            this.userControlQueryList.SelectAllItems();
        }

        public int ID_Specimen()
        {
            return -1;
        }

        public void setSpecimen()
        {
        }

        public void SaveImagesSpecimen()
        {
        }

        public System.Windows.Forms.ImageList ImageListDataWithholding()
        {
            return null;
        }

        public int? EventSeriesID()
        {
            return null;
        }

        public System.Windows.Forms.TreeNode SelectedUnitHierarchyNode()
        {
            return this.treeViewDependent.SelectedNode;
        }

        public System.Windows.Forms.TreeNode SelectedPartHierarchyNode()
        {
            return this.treeViewDependent.SelectedNode;
        }

        public enum Tree { UnitTree, PartTree }
        public void SelectNode(System.Data.DataRow Row, Tree Tree)
        {
        }

        public void saveSpecimen() { }
        public bool setSpecimen(int ID) { return false; }

        public void CustomizeDisplay(DiversityCollection.Forms.FormCustomizeDisplay.Customization Customize) { }

        public void RestrictImagesToUnit(int IdentificationUnitID) { }
        public void RestrictImagesToPart(int SpecimenPartID) { }

        public void SelectNode(System.Data.DataRow Row, DiversityCollection.Forms.FormCollectionSpecimen.Tree Tree) { }


        #endregion

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