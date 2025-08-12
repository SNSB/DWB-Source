using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControl_PartDescription : UserControl__Data
    {

        #region Parameter

        private System.Data.DataView _DvPartDescription;
        //private System.Windows.Forms.BindingSource _SourcePart;
        DiversityWorkbench.UserControls.RemoteValueBinding _RvbHierarchie;

        #endregion

        #region Construction

        public UserControl_PartDescription(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            System.Windows.Forms.BindingSource SourcePart,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            this._ParentSource = SourcePart;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region override base

        public override void SetPosition(int Position)
        {
            try
            {
                base.SetPosition(Position);
                this.setUserControlModuleRelatedEntrySources();
                System.Data.DataRowView R = (System.Data.DataRowView)this._ParentSource.Current;
                if (this._DvPartDescription == null)
                    this._DvPartDescription = new System.Data.DataView(this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPartDescription, "SpecimenPartID = " + R["SpecimenPartID"].ToString(), "PartDescriptionID", System.Data.DataViewRowState.CurrentRows);
                else this._DvPartDescription.RowFilter = "SpecimenPartID = " + R["SpecimenPartID"].ToString();
                this.listBoxPartDescription.DataSource = this._DvPartDescription;
                this.listBoxPartDescription.DisplayMember = "Description";
                this.listBoxPartDescription.ValueMember = "PartDescriptionID";

                this.setUserControlModuleRelatedEntrySources();

                setPartDescriptionSourcePosition();
                this.setUnitSource();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setPartPosition(System.Data.DataRow R)
        {
            if (this._DvPartDescription == null)
                this._DvPartDescription = new System.Data.DataView(this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPartDescription, "SpecimenPartID = " + R["SpecimenPartID"].ToString(), "PartDescriptionID", System.Data.DataViewRowState.CurrentRows);
            else this._DvPartDescription.RowFilter = "SpecimenPartID = " + R["SpecimenPartID"].ToString();
            this.listBoxPartDescription.DataSource = this._DvPartDescription;
            this.listBoxPartDescription.DisplayMember = "Description";
            this.listBoxPartDescription.ValueMember = "PartDescriptionID";
            setPartDescriptionSourcePosition();
            this.setUnitSource();
        }

        public override void setAvailability()
        {
            base.setAvailability();
            if (!this._iMainForm.ReadOnly())
            {
                this.toolStripButtonPartDescriptionAdd.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.INSERT];
                this.toolStripButtonPartDescriptionDelete.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.DELETE];
            }
        }
        
        #endregion

        #region private control events etc.

        private void initControl()
        {
            this.listBoxPartDescription.DataSource = this._Source;
            this.listBoxPartDescription.DisplayMember = "Description";
            this.listBoxPartDescription.ValueMember = "PartDescriptionID";
            this.textBoxPartDescriptionNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxDescriptionHierarchyCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DescriptionHierarchyCache", true));

            System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbProperty = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();
            _RvbHierarchie = new DiversityWorkbench.UserControls.RemoteValueBinding();
            _RvbHierarchie.BindingSource = this._Source;
            _RvbHierarchie.Column = "DescriptionHierarchyCache";
            _RvbHierarchie.RemoteParameter =  "Hierarchy Top-Down"; //DiversityWorkbench.Settings.ScientificTermsRemoteParameter; //
            RvbProperty.Add(_RvbHierarchie);
            this.userControlModuleRelatedEntryPartDescription.setRemoteValueBindings(RvbProperty);
            DiversityWorkbench.ScientificTerm T = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryPartDescription, T, "CollectionSpecimenPartDescription", "Description", "DescriptionTermURI", this._Source);
            this.userControlModuleRelatedEntryPartDescription.IsListInDatabase = true;
            this.userControlModuleRelatedEntryPartDescription.EnableChart(true);





            //DiversityWorkbench.ScientificTerm S = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
            //this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryPartDescription, S, "Identification", "VernacularTerm", "TermURI", this._Source);
            //this.userControlModuleRelatedEntryPartDescription.EnableChart(true);

            //ScientificTerm
            //System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbIdentificationScientificTerm = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();
            //_RvbHierarchyCacheScientificTerm = new DiversityWorkbench.UserControls.RemoteValueBinding();
            //_RvbHierarchyCacheScientificTerm.BindingSource = this._ParentSource;
            //_RvbHierarchyCacheScientificTerm.Column = "HierarchyCache";
            //_RvbHierarchyCacheScientificTerm.RemoteParameter = "Hierarchy Top-Down";
            ////switch (DiversityWorkbench.Settings.ScientificTermsRemoteParameter)
            ////{
            ////    case "Name":
            ////        _RvbHierarchyCacheScientificTerm.RemoteParameter = "Hierarchy";
            ////        break;
            ////    default:
            ////        _RvbHierarchyCacheScientificTerm.RemoteParameter = DiversityWorkbench.Settings.ScientificTermsRemoteParameter;
            ////        break;
            ////}
            //RvbIdentificationScientificTerm.Add(_RvbHierarchyCacheScientificTerm);

            //this.userControlModuleRelatedEntryIdentificationScientificTerm.setRemoteValueBindings(RvbIdentificationScientificTerm);
            //DiversityWorkbench.ScientificTerm S = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
            //this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryIdentificationScientificTerm, S, "Identification", "VernacularTerm", "TermURI", this._Source);
            //this.userControlModuleRelatedEntryIdentificationScientificTerm.ShowInfo = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames;
            //if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames) // &&!_InfoTextAcceptedNameEventHandlerSet)
            //{
            //    this.userControlModuleRelatedEntryIdentificationScientificTerm.labelURI.TextChanged += new System.EventHandler(this.setInfoTextPreferredName);
            //}
            //this.userControlModuleRelatedEntryIdentificationScientificTerm.EnableChart(true);






            this.setUserControlModuleRelatedEntrySources();

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void toolStripButtonPartDescriptionAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartDescriptionRow R = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPartDescription.NewCollectionSpecimenPartDescriptionRow();
                R.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                if (this._ParentSource.Current != null)
                {
                    System.Data.DataRowView RP = (System.Data.DataRowView)this._ParentSource.Current;
                    R.SpecimenPartID = int.Parse(RP["SpecimenPartID"].ToString());
                    R.Description = "New description";
                    if (this._iMainForm.SelectedPartHierarchyNode() != null && this._iMainForm.SelectedPartHierarchyNode().Tag != null)
                    {
                        System.Data.DataRow RTree = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                        if (RTree.Table.TableName == "IdentificationUnitInPart")
                        {
                            R.IdentificationUnitID = int.Parse(RTree["IdentificationUnitID"].ToString());
                        }
                    }
                    this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPartDescription.Rows.Add(R);
                    this.setPartDescriptionSourcePosition();
                    this.listBoxPartDescription.SelectedIndex = 0;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonPartDescriptionDelete_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R.Delete();
            this.setPartDescriptionSourcePosition();
        }

        private void listBoxPartDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setPartDescriptionSourcePosition();
        }

        private void listBoxPartDescription_DataSourceChanged(object sender, EventArgs e)
        {
            this.setPartDescriptionSourcePosition();
        }

        private void setPartDescriptionSourcePosition()
        {
            int i = -1;
            try
            {
                bool DataBaseChanged = false;
                try
                {
                    System.Object o = this.listBoxPartDescription.SelectedItem;
                }
                catch (System.Exception ex)
                {
                    DataBaseChanged = true;
                }
                if (this.listBoxPartDescription.Items.Count > 0 && DataBaseChanged)
                    this.listBoxPartDescription.SelectedIndex = 0;
                if (this._Source.Current != null && this.listBoxPartDescription.SelectedItem != null)
                {
                    System.Data.DataRowView Rlist = (System.Data.DataRowView)this.listBoxPartDescription.SelectedItem;
                    foreach (System.Data.DataRow RD in this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPartDescription.Rows)
                    {
                        i++;
                        if (RD[0].ToString() == Rlist[0].ToString()
                            && RD[1].ToString() == Rlist[1].ToString()
                            && RD[2].ToString() == Rlist[2].ToString())
                        {
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            this._Source.Position = i;

            bool EnablePartDescription = this.FormFunctions.getObjectPermissions("CollectionSpecimenPartDescription", "UPDATE");
            if (i == -1)
                EnablePartDescription = false;
            this.listBoxPartDescription.Enabled = EnablePartDescription;
            this.textBoxPartDescriptionNotes.Enabled = EnablePartDescription;
            this.userControlModuleRelatedEntryPartDescription.Enabled = EnablePartDescription;
            this.toolStripButtonPartDescriptionDelete.Enabled = EnablePartDescription;
            this.comboBoxIdentificationUnitID.Enabled = EnablePartDescription;
            if (this._Source.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (!R["DescriptionTermURI"].Equals(DBNull.Value) && R["DescriptionTermURI"].ToString().Length > 0)
                    EnablePartDescription = false;
            }
            this.textBoxDescriptionHierarchyCache.Enabled = EnablePartDescription;
        }

        System.Data.DataTable _DtUnits;
        private void setUnitSource()
        {
            this.comboBoxIdentificationUnitID.DataBindings.Clear();
            this.comboBoxIdentificationUnitID.DataSource = null;
            this._DtUnits = new DataTable();
            string SQL = "SELECT NULL AS IdentificationUnitID, NULL AS LastIdentificationCache UNION " +
                "SELECT IdentificationUnitID, LastIdentificationCache FROM IdentificationUnit U WHERE U.CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() +
                " ORDER BY LastIdentificationCache";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtUnits, ref Message);
            this.comboBoxIdentificationUnitID.DataSource = this._DtUnits;
            this.comboBoxIdentificationUnitID.DisplayMember = "LastIdentificationCache";
            this.comboBoxIdentificationUnitID.ValueMember = "IdentificationUnitID";
            this.comboBoxIdentificationUnitID.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "IdentificationUnitID", true));

        }

        private void setUserControlModuleRelatedEntrySources()
        {
            DiversityWorkbench.UserSettings U = new DiversityWorkbench.UserSettings();
            string Source = "";
            string ProjectID = "";
            string Project = "";
            string MaterialCategory = "";
            if (this._ParentSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._ParentSource.Current;
                MaterialCategory = R["MaterialCategory"].ToString();
                System.Collections.Generic.List<string> Settings = new List<string>();
                Settings.Add("ModuleSource");
                Settings.Add("CollectionSpecimenPartDescription");
                Settings.Add("MaterialCategory");
                Settings.Add(MaterialCategory);
                this.setUserControlModuleRelatedEntrySources(Settings,
                    ref userControlModuleRelatedEntryPartDescription); //, Width, Height);

                if (false)
                {

                    Source = U.GetSetting(Settings);
                    Source = U.GetSetting(Settings, "Database");
                    if (Source.Length == 0)
                    {
                        Source = U.GetSetting(Settings, "Webservice");
                        if (Source.Length == 0)
                            this.userControlModuleRelatedEntryPartDescription.ReleaseSource();
                        else
                        {
                            this.userControlModuleRelatedEntryPartDescription.FixSource(Source);
                        }
                    }
                    else
                    {
                        ProjectID = U.GetSetting(Settings, "ProjectID");
                        Project = U.GetSetting(Settings, "Project");
                        this.userControlModuleRelatedEntryPartDescription.FixSource(Source, ProjectID, Project);

                        string BaseURL =
                            DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"]
                                .ServerConnectionList()[Source].BaseURL;
                        string URL = BaseURL + ProjectID;

                        // Markus 4.11.2020: setting the chart if possible
                        //this.userControlModuleRelatedEntryPartDescription.setChart(false);
                        int PropertyID;
                        if (int.TryParse(ProjectID, out PropertyID) &&
                            Source.StartsWith("DiversityScientificTerms"))
                        {
                            if (URL.Length > 0)
                            {
                                DiversityWorkbench.Chart C = DiversityWorkbench.Terminology.GetChart(URL);
                                if (C != null)
                                {
                                    //this.userControlModuleRelatedEntryPartDescription.EnableChart(C, BaseURL, "Please select an item from " + Project);
                                }
                                else
                                {
                                    //this.userControlModuleRelatedEntryPartDescription.setChart(false, true);
                                }
                            }
                        }
                    }
                }
            }

        }
        // }


        #endregion

        #region ScientificTermsRemoteParameter

        //public void SetScientificTermsRemoteParameter()
        //{
        //    _RvbHierarchie.RemoteParameter = DiversityWorkbench.Settings.ScientificTermsRemoteParameter; // "Hierarchy";
        //}

        #endregion

    }
}
