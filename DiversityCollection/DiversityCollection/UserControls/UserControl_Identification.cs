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
    public partial class UserControl_Identification : UserControl__Data, iIdentification
    {

        #region Parameter

        private string _TaxonomicGroup = "";
        private bool _IsTaxonomyRelatedTaxonomicGroup = true;
        DiversityWorkbench.UserControls.RemoteValueBinding _RvbHierarchyCacheScientificTerm;
        int _ParentWidth = 0;
        int _ParentHeight = 0;

        #endregion

        #region Construction

        public UserControl_Identification(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            System.Windows.Forms.BindingSource ParentSource,
            string HelpNameSpace)
            : base(MainForm, Source, HelpNameSpace)
        {
            InitializeComponent();
            this._ParentSource = ParentSource;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
            //this.userControlModuleRelatedEntryTaxonomicName.Leave += this.UserControlModuleRelatedEntry_Leave;
        }

        #endregion

        #region Init control

        private void initControl()
        {
            try
            {
                // combo boxes
                this.comboBoxIdentificationQualifier.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "IdentificationQualifier", true));
                this.comboBoxIdentificationCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "IdentificationCategory", true));
                this.comboBoxTypeStatus.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "TypeStatus", true));
                this.comboBoxIdentificationDateCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "IdentificationDateCategory", true));
                this.comboBoxVernacularTerm.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "VernacularTerm", true));

                this._EnumComboBoxes = new Dictionary<ComboBox, string>();
                this._EnumComboBoxes.Add(this.comboBoxIdentificationCategory, "CollIdentificationCategory_Enum");
                this._EnumComboBoxes.Add(this.comboBoxTypeStatus, "CollTypeStatus_Enum");
                this._EnumComboBoxes.Add(this.comboBoxIdentificationQualifier, "CollIdentificationQualifier_Enum");
                this._EnumComboBoxes.Add(this.comboBoxIdentificationDateCategory, "CollIdentificationDateCategory_Enum");
                this.InitLookupSources();

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxVernacularTerm);

                // text boxes
                this.textBoxIdentificationNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
                this.textBoxTypeNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "TypeNotes", true));
                this.textBoxIdentificationReferenceDetails.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ReferenceDetails", true));

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxTypeNotes);

                // hierarchy controls
                this.userControlHierarchySelectorTypeStatus.initHierarchyForEnum("CollTypeStatus_Enum", "TypeStatus", this.comboBoxTypeStatus, this._Source);
                this.userControlHierarchySelectorIdentificationQualifier.initHierarchyForEnum("CollIdentificationQualifier_Enum", "IdentificationQualifier", this.comboBoxIdentificationQualifier, this._Source);
                this.userControlHierarchySelectorIdentificationCategory.initHierarchyForEnum("CollIdentificationCategory_Enum", "IdentificationCategory", this.comboBoxIdentificationCategory, this._Source);
                this.userControlHierarchySelectorIdentificationDateCategory.initHierarchyForEnum("CollIdentificationDateCategory_Enum", "IdentificationDateCategory", this.comboBoxIdentificationDateCategory, this._Source);
                //this.userControlModuleRelatedEntryTaxonomicName.textBoxValue.TextChanged += new System.EventHandler(this.updateHierarchyNodeByUserControlModuleRelatedEntry);
                //this.userControlModuleRelatedEntryTaxonomicName.textBoxValue.Leave += new System.EventHandler(this.updateHierarchyNodeByUserControlModuleRelatedEntry);
                this.userControlModuleRelatedEntryTaxonomicName.LinkDeleteConnectionToModuleToTableGrant = true;

                // module related controls
                //TaxonomicName
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryTaxonomicName, T, "Identification", "TaxonomicName", "NameURI", this._Source);

                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbIdentification = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();

                DiversityWorkbench.UserControls.RemoteValueBinding RvbFamily = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbFamily.BindingSource = this._ParentSource;
                RvbFamily.Column = "FamilyCache";
                RvbFamily.RemoteParameter = "Family";
                RvbIdentification.Add(RvbFamily);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbOrder = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbOrder.BindingSource = this._ParentSource;
                RvbOrder.Column = "OrderCache";
                RvbOrder.RemoteParameter = "Order";
                RvbIdentification.Add(RvbOrder);

                DiversityWorkbench.UserControls.RemoteValueBinding RvbHierarchyCache = new DiversityWorkbench.UserControls.RemoteValueBinding();
                RvbHierarchyCache.BindingSource = this._ParentSource;
                RvbHierarchyCache.Column = "HierarchyCache";
                RvbHierarchyCache.RemoteParameter = "Hierarchy";
                RvbIdentification.Add(RvbHierarchyCache);

                this.userControlModuleRelatedEntryTaxonomicName.setRemoteValueBindings(RvbIdentification);
                this.userControlModuleRelatedEntryTaxonomicName.SupressEmptyRemoteValues = true;
                //this.userControlModuleRelatedEntryTaxonomicName.ShowInfo = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames;
                this.SetOptions();
                //if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames) // &&!_InfoTextAcceptedNameEventHandlerSet)
                //{
                //    this.userControlModuleRelatedEntryTaxonomicName.labelURI.TextChanged += new System.EventHandler(this.setInfoTextAcceptedName);
                //    //this.userControlModuleRelatedEntryTaxonomicName.Height = 27;
                //}
                //else
                //{
                //    //this.userControlModuleRelatedEntryTaxonomicName.Height = 23;
                //}
                this.userControlModuleRelatedEntryTaxonomicName.EnableChart(true);
                //this.setModuleInfoTextOption();

                //ScientificTerm
                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RvbIdentificationScientificTerm = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();
                _RvbHierarchyCacheScientificTerm = new DiversityWorkbench.UserControls.RemoteValueBinding();
                _RvbHierarchyCacheScientificTerm.BindingSource = this._ParentSource;
                _RvbHierarchyCacheScientificTerm.Column = "HierarchyCache";
                _RvbHierarchyCacheScientificTerm.RemoteParameter = "Hierarchy Top-Down";
                RvbIdentificationScientificTerm.Add(_RvbHierarchyCacheScientificTerm);

                this.userControlModuleRelatedEntryIdentificationScientificTerm.setRemoteValueBindings(RvbIdentificationScientificTerm);
                DiversityWorkbench.ScientificTerm S = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryIdentificationScientificTerm, S, "Identification", "VernacularTerm", "TermURI", this._Source);
                //this.userControlModuleRelatedEntryIdentificationScientificTerm.ShowInfo = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames;
                //if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames) // &&!_InfoTextAcceptedNameEventHandlerSet)
                //{
                //    this.userControlModuleRelatedEntryIdentificationScientificTerm.labelURI.TextChanged += new System.EventHandler(this.setInfoTextPreferredName);
                //}
                this.userControlModuleRelatedEntryIdentificationScientificTerm.EnableChart(true);

                //IdentificationReference
                DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryIdentificationReference, R, "Identification", "ReferenceTitle", "ReferenceURI", this._Source);

                //IdentificationResponsible
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryIdentificationResponsible, A, "Identification", "ResponsibleName", "ResponsibleAgentURI", this._Source);
                this.userControlModuleRelatedEntryIdentificationResponsible.textBoxValue.TextChanged += new System.EventHandler(this.updateIdentificationResponsibleUserControlModuleRelatedEntry);

                this.userControlDatePanelIdentificationDate.setDataBindings(this._Source, "IdentificationDay", "IdentificationMonth", "IdentificationYear", "IdentificationDateSupplement");

                this.setUserControlModuleRelatedEntrySources();

                // buttons
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows.Count > 0)//  && this._iMainForm.AvailabilityState() == DiversityCollection.CollectionSpecimen.AvailabilityState.Available)
                    this.buttonCollectorIsResponsible.Enabled = true;
                else this.buttonCollectorIsResponsible.Enabled = false;

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

                DiversityWorkbench.Entity.setEntity(this, this.toolTip);

                this.CheckIfClientIsUpToDate();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Public interface

        public void SetOptions()
        {
            try
            {
                this.userControlModuleRelatedEntryTaxonomicName.ShowInfo = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames;
                this.userControlModuleRelatedEntryIdentificationScientificTerm.ShowInfo = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames;
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames) // &&!_InfoTextAcceptedNameEventHandlerSet)
                {
                    this.userControlModuleRelatedEntryTaxonomicName.labelURI.TextChanged += new System.EventHandler(this.setInfoTextAcceptedName);
                    this.userControlModuleRelatedEntryIdentificationScientificTerm.labelURI.TextChanged += new System.EventHandler(this.setInfoTextPreferredName);
                }
                else
                {
                    this.userControlModuleRelatedEntryTaxonomicName.labelURI.TextChanged -= this.setInfoTextAcceptedName;
                    this.userControlModuleRelatedEntryTaxonomicName.SetInfoText(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames, "", "", System.Drawing.SystemColors.WindowText);

                    this.userControlModuleRelatedEntryIdentificationScientificTerm.labelURI.TextChanged -= this.setInfoTextPreferredName;
                    this.userControlModuleRelatedEntryIdentificationScientificTerm.SetInfoText(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames, "", "", System.Drawing.SystemColors.WindowText);
                }
                this.userControlModuleRelatedEntryTaxonomicName.Height = this.userControlModuleRelatedEntryTaxonomicName.HeightOfControl();
                this.userControlModuleRelatedEntryIdentificationScientificTerm.Height = this.userControlModuleRelatedEntryIdentificationScientificTerm.HeightOfControl();
            }
            catch (System.Exception ex)
            { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

            this.SetOptionResponsible();
        }

        private void SetOptionResponsible()
        {
            try
            {
                this.userControlModuleRelatedEntryIdentificationResponsible.ShowInfo = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames;
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames) // &&!_InfoTextAcceptedNameEventHandlerSet)
                {
                    this.userControlModuleRelatedEntryIdentificationResponsible.labelURI.TextChanged += new System.EventHandler(this.setInfoTextResponsible);
                }
                else
                {
                    this.userControlModuleRelatedEntryIdentificationResponsible.labelURI.TextChanged -= this.setInfoTextResponsible;
                    this.userControlModuleRelatedEntryIdentificationResponsible.SetInfoText(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames, "", "", System.Drawing.SystemColors.WindowText);
                }
                this.userControlModuleRelatedEntryIdentificationResponsible.Height = this.userControlModuleRelatedEntryIdentificationResponsible.HeightOfControl();
            }
            catch (System.Exception ex)
            { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public override void InitLookupSources() { this.InitEnums(); }

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.setUserControlModuleRelatedEntrySources();
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames)
                this.setInfoTextAcceptedName(this.userControlModuleRelatedEntryTaxonomicName.labelURI, null);
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryIdentificationResponsible, "ResponsibleAgentURI");
            bool ShowNonAgentControls = true;

#if DEBUG // waere zum Ausblenden aller nicht Bestimmer Controls falls abhängige Bestimmungen bei TaxononyRelated dafür verwendet werden - vorerst zurückgestellt (Label, CacheDB, ... etc. müssten ebenfalls angepasst werden, Nutzen unklar und kein aktueller Bedarf)
            if(this._IsTaxonomyRelatedTaxonomicGroup)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                int i;
                if (int.TryParse(RV["DependsOnIdentificationSequence"].ToString(), out i))
                {
                    ShowNonAgentControls = false;
                }
                this.labelIdentificationQualifier.Visible = ShowNonAgentControls;
                this.comboBoxIdentificationQualifier.Visible = ShowNonAgentControls;
                this.userControlHierarchySelectorIdentificationQualifier.Visible = ShowNonAgentControls;

                this.labelTaxonomicName.Visible = ShowNonAgentControls;
                this.userControlModuleRelatedEntryTaxonomicName.Visible = ShowNonAgentControls;

                this.labelVernacularTerm.Visible = ShowNonAgentControls;
                this.comboBoxVernacularTerm.Visible = ShowNonAgentControls;

                this.labelTypeNotes.Visible = ShowNonAgentControls;
                this.textBoxTypeNotes.Visible = ShowNonAgentControls;

                this.comboBoxTypeStatus.Visible = ShowNonAgentControls;
                this.labelTypeStatus.Visible = ShowNonAgentControls;
                this.userControlHierarchySelectorTypeStatus.Visible = ShowNonAgentControls;

                if (this._IsTaxonomyRelatedTaxonomicGroup)
                {
                    this.labelIdentificationScientificTerm.Visible = false;
                    this.userControlModuleRelatedEntryIdentificationScientificTerm.Visible = false;
                }
                //else
                //{

                //}

                this.userControlDatePanelIdentificationDate.Visible = ShowNonAgentControls;
                this.labelIdentificationDate.Visible = ShowNonAgentControls;

                this.labelIdentificationDateCategory.Visible = ShowNonAgentControls;
                this.comboBoxIdentificationDateCategory.Visible = ShowNonAgentControls;
                this.userControlHierarchySelectorIdentificationDateCategory.Visible = ShowNonAgentControls;

                this.labelIdentificationNotes.Visible = ShowNonAgentControls;
                this.textBoxIdentificationNotes.Visible = ShowNonAgentControls;

                this.labelIdentificationCategory.Visible = ShowNonAgentControls;
                this.comboBoxIdentificationCategory.Visible = ShowNonAgentControls;
                this.userControlHierarchySelectorIdentificationCategory.Visible = ShowNonAgentControls;
            }
            else
            {
                this.setIdentificationTermControls(this._IsTaxonomyRelatedTaxonomicGroup);

                this.userControlDatePanelIdentificationDate.Visible = ShowNonAgentControls;
                this.labelIdentificationDate.Visible = ShowNonAgentControls;

                this.labelIdentificationDateCategory.Visible = ShowNonAgentControls;
                this.comboBoxIdentificationDateCategory.Visible = ShowNonAgentControls;
                this.userControlHierarchySelectorIdentificationDateCategory.Visible = ShowNonAgentControls;

                this.labelIdentificationNotes.Visible = ShowNonAgentControls;
                this.textBoxIdentificationNotes.Visible = ShowNonAgentControls;

                this.labelIdentificationCategory.Visible = ShowNonAgentControls;
                this.comboBoxIdentificationCategory.Visible = ShowNonAgentControls;
                this.userControlHierarchySelectorIdentificationCategory.Visible = ShowNonAgentControls;
            }
#endif     
        }

        public void setIdentificationTermControls(bool IsTaxonomyRelatedTaxonomicGroup)
        {
            this._IsTaxonomyRelatedTaxonomicGroup = IsTaxonomyRelatedTaxonomicGroup;

            this.labelIdentificationQualifier.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.comboBoxIdentificationQualifier.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.userControlHierarchySelectorIdentificationQualifier.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelTaxonomicName.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.userControlModuleRelatedEntryTaxonomicName.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelVernacularTerm.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.comboBoxVernacularTerm.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelTypeNotes.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.textBoxTypeNotes.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.comboBoxTypeStatus.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.labelTypeStatus.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.userControlHierarchySelectorTypeStatus.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelIdentificationScientificTerm.Visible = !IsTaxonomyRelatedTaxonomicGroup;
            this.userControlModuleRelatedEntryIdentificationScientificTerm.Visible = !IsTaxonomyRelatedTaxonomicGroup;
        }

        public void setTaxonomicGroup(string TaxonomicGroup)
        {
            this._TaxonomicGroup = TaxonomicGroup;
        }

        #region ScientificTermsRemoteParameter

        //public void SetScientificTermsRemoteParameter()
        //{
        //    _RvbHierarchyCacheScientificTerm.RemoteParameter = DiversityWorkbench.Settings.ScientificTermsRemoteParameter; // "Hierarchy";
        //}

        #endregion


        #endregion

        #region Accepted and preferred name

        private void setInfoTextAcceptedName(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Label L = (System.Windows.Forms.Label)sender;
                string URI = L.Text;
                string UrlAcceptedName = "";
                string AcceptedName = this.GetAcceptedName(URI, ref UrlAcceptedName);
                this.userControlModuleRelatedEntryTaxonomicName.SetInfoText(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames, AcceptedName, UrlAcceptedName, System.Drawing.Color.Green);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setInfoTextResponsible(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Label L = (System.Windows.Forms.Label)sender;
                string URI = L.Text;
                string UrlResponsible = "";
                string Responsible = this.GetResponsibleName(URI, ref UrlResponsible);
                this.userControlModuleRelatedEntryIdentificationResponsible.SetInfoText(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowValidAgentName, Responsible, UrlResponsible, System.Drawing.Color.Green);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string GetResponsibleName(string URI, ref string UrlResponsible)
        {
            string Responsible = "";
            if (URI.Length > 0)
                Responsible = DiversityWorkbench.Agent.AcceptedName(URI, ref UrlResponsible);
            return Responsible;
        }



        private string GetAcceptedName(string URI, ref string UrlAcceptedName)
        {
            string AcceptedName = "";
            if (URI.Length > 0)
                AcceptedName = DiversityWorkbench.TaxonName.AcceptedName(URI, ref UrlAcceptedName);
            return AcceptedName;
        }

        private void setInfoTextPreferredName(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Label L = (System.Windows.Forms.Label)sender;
                string URI = L.Text;
                string UrlPreferredName = "";
                string PreferredName = this.GetPreferredName(URI, ref UrlPreferredName);
                this.userControlModuleRelatedEntryIdentificationScientificTerm.SetInfoText(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames, PreferredName, UrlPreferredName, System.Drawing.Color.Green);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string GetPreferredName(string URI, ref string UrlPreferredName)
        {
            string PreferredName = "";
            if (URI.Length > 0)
                PreferredName = DiversityWorkbench.ScientificTerm.PreferredName(URI, ref UrlPreferredName);
            return PreferredName;
        }

        #endregion

        #region Setting the responsible
        private void buttonCollectorIsResponsible_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows.Count > 0)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                    if (this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows.Count == 1)
                    {
                        RV["ResponsibleName"] = this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[0]["CollectorsName"].ToString();
                        RV["ResponsibleAgentURI"] = this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[0]["CollectorsAgentURI"].ToString();
                        this.userControlModuleRelatedEntryIdentificationResponsible.textBoxValue.Text = this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[0]["CollectorsName"].ToString();
                        this.userControlModuleRelatedEntryIdentificationResponsible.labelURI.Text = this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[0]["CollectorsAgentURI"].ToString();
                    }
                    else
                    {
                        string FirstCollector = this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[0]["CollectorsName"].ToString();
                        string AllCollectors = FirstCollector;
                        if (this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows.Count == 2)
                            AllCollectors += " & " + this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[1]["CollectorsName"].ToString();
                        else
                        {
                            AllCollectors = "";
                            for (int i = 0; i < this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows.Count - 1; i++)
                            {
                                if (i > 0) AllCollectors += ", ";
                                AllCollectors += this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[i]["CollectorsName"].ToString();
                            }
                            AllCollectors += " & " + this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows.Count - 1]["CollectorsName"].ToString();
                        }
                        DiversityCollection.Forms.FormCollectorIsResponsible f = new DiversityCollection.Forms.FormCollectorIsResponsible(FirstCollector, AllCollectors);
                        f.ShowDialog();
                        if (f.Responsible == DiversityCollection.Forms.FormCollectorIsResponsible.CollectorAsIdentifier.first)
                        {
                            RV["ResponsibleName"] = this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[0]["CollectorsName"].ToString();
                            RV["ResponsibleAgentURI"] = this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[0]["CollectorsAgentURI"].ToString();
                            this.userControlModuleRelatedEntryIdentificationResponsible.textBoxValue.Text = this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[0]["CollectorsName"].ToString();
                            this.userControlModuleRelatedEntryIdentificationResponsible.labelURI.Text = this._iMainForm.DataSetCollectionSpecimen().CollectionAgent.Rows[0]["CollectorsAgentURI"].ToString();
                        }
                        else if (f.Responsible == DiversityCollection.Forms.FormCollectorIsResponsible.CollectorAsIdentifier.all)
                        {
                            RV["ResponsibleName"] = AllCollectors;
                            RV["ResponsibleAgentURI"] = System.DBNull.Value;
                            this.userControlModuleRelatedEntryIdentificationResponsible.textBoxValue.Text = AllCollectors;
                            this.userControlModuleRelatedEntryIdentificationResponsible.labelURI.Text = "";
                        }
                    }

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("There is no collector");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Vernacular term
        private void comboBoxVernacularTerm_DropDown(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT DISTINCT Identification.VernacularTerm " +
                    "FROM Identification INNER JOIN " +
                    "IdentificationUnit ON Identification.CollectionSpecimenID = IdentificationUnit.CollectionSpecimenID AND  " +
                    "Identification.IdentificationUnitID = IdentificationUnit.IdentificationUnitID " +
                    "WHERE (Identification.VernacularTerm <> N'') ";
                if (this._TaxonomicGroup.Length > 0)
                    SQL += " AND (IdentificationUnit.TaxonomicGroup = N'" + this._TaxonomicGroup + "')";
                if (this.comboBoxVernacularTerm.Text.Length > 0)
                    SQL += "AND Identification.VernacularTerm LIKE '" + this.comboBoxVernacularTerm.Text + "%' ";
                SQL += "ORDER BY Identification.VernacularTerm";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                System.Data.DataTable dt = new DataTable();
                ad.Fill(dt);
                this.comboBoxVernacularTerm.DataSource = dt;
                this.comboBoxVernacularTerm.DisplayMember = "VernacularTerm";
                this.comboBoxVernacularTerm.ValueMember = "VernacularTerm";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxVernacularTerm_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
            RV["VernacularTerm"] = this.comboBoxVernacularTerm.SelectedValue;
            this.comboBoxVernacularTerm.Text = this.comboBoxVernacularTerm.SelectedValue.ToString();
        }

        #endregion

        #region Module related entries

        //private void UserControlModuleRelatedEntry_Leave(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (this._Source.Current != null)
        //        {
        //            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
        //            if (R["TaxonomicName"].ToString() != this.userControlModuleRelatedEntryTaxonomicName.textBoxValue.Text)
        //            {
        //                R.BeginEdit();
        //                R["TaxonomicName"] = this.userControlModuleRelatedEntryTaxonomicName.textBoxValue.Text;
        //                R.EndEdit();
        //                this._iMainForm.saveSpecimen();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void updateHierarchyNodeByUserControlModuleRelatedEntry(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
        //        if (T.DataBindings.Count == 0) return;
        //        System.Windows.Forms.Binding B = T.DataBindings[0];
        //        if (B.DataSource == null) return;
        //        System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)B.DataSource;
        //        if (BS.Current == null) return;
        //        System.Data.DataRowView RV = (System.Data.DataRowView)BS.Current;
        //        System.Data.DataRow R = RV.Row;
        //        System.Windows.Forms.TreeNode N = this._iMainForm.SelectedUnitHierarchyNode();
        //        if (N == null) return;
        //        DiversityCollection.HierarchyNode H = (DiversityCollection.HierarchyNode)N;
        //        if (H == null) return;
        //        H.setText();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private void updateIdentificationResponsibleUserControlModuleRelatedEntry(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)sender;
                if (T.DataBindings.Count == 0) return;
                if (T.Text.Length == 0)
                {
                    System.Windows.Forms.Binding B = T.DataBindings[0];
                    if (B.DataSource == null) return;
                    System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)B.DataSource;
                    if (BS.Current == null) return;
                    System.Data.DataRowView RV = (System.Data.DataRowView)BS.Current;
                    System.Data.DataRow R = RV.Row;
                    if (!R["ResponsibleAgentURI"].Equals(System.DBNull.Value) && R["ResponsibleAgentURI"].ToString().Length == 0)
                    {
                        R.BeginEdit();
                        R["ResponsibleAgentURI"] = System.DBNull.Value;
                        R.EndEdit();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setUserControlModuleRelatedEntrySources()
        {
            try
            {
                DiversityWorkbench.UserSettings U = new DiversityWorkbench.UserSettings();
                string Source = "";
                string ProjectID = "";
                string Project = "";
                DiversityWorkbench.UserControls.UserControlModuleRelatedEntry UCMRE = this.userControlModuleRelatedEntryTaxonomicName;
                if (!this._IsTaxonomyRelatedTaxonomicGroup)
                    UCMRE = this.userControlModuleRelatedEntryIdentificationScientificTerm;
                if (this._ParentSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    string URI = "";
                    if (!R["DependsOnIdentificationSequence"].Equals(System.DBNull.Value))
                    {
                        string SQL = "SELECT TermUri FROM Identification WHERE IdentificationUnitID = " + R["IdentificationUnitID"].ToString() + " AND IdentificationSequence = " + R["DependsOnIdentificationSequence"].ToString();
                        URI = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    }
                    UCMRE.DependsOnUri = URI;
                    //this.userControlModuleRelatedEntryIdentificationScientificTerm.DependsOnUri = URI;
                    //this.userControlModuleRelatedEntryTaxonomicName.DependsOnUri = URI;
                    if (_TaxonomicGroup == null || _TaxonomicGroup.Length == 0)
                    {
                        System.Data.DataRowView Parent = (System.Data.DataRowView)this._ParentSource.Current;
                        if (Parent.Row.Table.Columns.Contains("TaxonomicGroup") && !Parent["TaxonomicGroup"].Equals(System.DBNull.Value))
                            _TaxonomicGroup = Parent["TaxonomicGroup"].ToString();
                    }

                    //int? Width = null;
                    //int? Height = null;
                    //if (this.ParentForm != null)
                    //{
                    //    Width = this.ParentForm.Width;
                    //    Height = this.ParentForm.Height;
                    //}
                    //else if (this._iMainForm != null)
                    //{
                    //    Width = this._iMainForm.FormWidth();
                    //    Height = this._iMainForm.FormHeight();
                    //}
                    System.Collections.Generic.List<string> Settings = new List<string>();
                    Settings.Add("ModuleSource");
                    Settings.Add("Identification");
                    Settings.Add("TaxonomicGroup");
                    Settings.Add(_TaxonomicGroup);
                    this.setUserControlModuleRelatedEntrySources(Settings, ref UCMRE);//, Width, Height);

                    if (false) // use functionallity of userControlModuleRelatedEntry
                    {
                        Source = U.GetSetting(Settings);
                        // Ariane removed WebSerive for .net8
                        //if (Source.Length > 0)
                        //{
                        //    UCMRE.FixSource(Source);
                        //    //this.userControlModuleRelatedEntryTaxonomicName.FixSource(Source);
                        //    System.Collections.Generic.Dictionary<string, string> Options = U.GetSettingOptions(Settings, this.userControlModuleRelatedEntryTaxonomicName.SourceWebService());
                        //    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Options)
                        //    {
                        //        UCMRE.SetSourceWebserviceOptions(KV.Key, KV.Value);
                        //        //this.userControlModuleRelatedEntryTaxonomicName.SetSourceWebserviceOptions(KV.Key, KV.Value);
                        //    }
                        //}
                        //else
                        //{
                            Source = U.GetSetting(Settings, "Database");
                            if (Source.Length == 0)
                            {
                                Source = U.GetSetting(Settings, "Webservice");
                                if (Source.Length == 0)
                                {
                                    UCMRE.ReleaseSource();
                                    //this.userControlModuleRelatedEntryTaxonomicName.ReleaseSource();
                                }
                                else
                                {
                                    UCMRE.FixSource(Source);
                                    //this.userControlModuleRelatedEntryTaxonomicName.FixSource(Source);
                                    //this.userControlModuleRelatedEntryIdentificationScientificTerm.FixSource(Source);
                                }
                            }
                            else
                            {
                                ProjectID = U.GetSetting(Settings, "ProjectID");
                                Project = U.GetSetting(Settings, "Project");
                                string SectionID = U.GetSetting(Settings, "SectionID");
                                string Section = U.GetSetting(Settings, "Section");
                                int iSectionID = -1;
                                int? iiSectionID = null;
                                if (int.TryParse(SectionID, out iSectionID))
                                    iiSectionID = iSectionID;
                                iiSectionID = iSectionID;
                                UCMRE.FixSource(Source, ProjectID, Project, iiSectionID, Section, Width, Height);

                                //this.userControlModuleRelatedEntryTaxonomicName.FixSource(Source, ProjectID, Project, iSectionID, Section);
                                //this.userControlModuleRelatedEntryIdentificationScientificTerm.FixSource(Source, ProjectID, Project, iSectionID, Section);

                                //if (int.TryParse(SectionID, out iSectionID) && Section.Length > 0)
                                //{
                                //iiSectionID = iSectionID;
                                //    this.userControlModuleRelatedEntryTaxonomicName.FixSource(Source, ProjectID, Project, iSectionID, Section);
                                //    this.userControlModuleRelatedEntryIdentificationScientificTerm.FixSource(Source, ProjectID, Project, iSectionID, Section);
                                //}
                                //else
                                //{
                                //    this.userControlModuleRelatedEntryTaxonomicName.FixSource(Source, ProjectID, Project);
                                //    this.userControlModuleRelatedEntryIdentificationScientificTerm.FixSource(Source, ProjectID, Project);
                                //}

                                //this.userControlModuleRelatedEntryTaxonomicName.setChart(false);
                                //this.userControlModuleRelatedEntryIdentificationScientificTerm.setChart(false);
                                int PropertyID;
                                if (int.TryParse(ProjectID, out PropertyID) &&
                                    (Source.StartsWith("DiversityScientificTerms") ||
                                     Source.StartsWith("DiversityTaxonNames")) &&
                                    (R["DependsOnIdentificationSequence"].Equals(System.DBNull.Value) ||
                                     R["DependsOnIdentificationSequence"].ToString().Length == 0))
                                {
                                    DiversityWorkbench.WorkbenchUnit.ModuleType moduleType =
                                        DiversityWorkbench.WorkbenchUnit.ModuleType.None;
                                    if (Source.StartsWith("Diversity" +
                                                          DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames
                                                              .ToString()))
                                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames;
                                    else if (Source.StartsWith("Diversity" + DiversityWorkbench.WorkbenchUnit.ModuleType
                                                 .ScientificTerms.ToString()))
                                        moduleType = DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms;
                                    if (moduleType != DiversityWorkbench.WorkbenchUnit.ModuleType.None)
                                    {
                                        string Module = "Diversity" + moduleType.ToString();
                                        string URL = "";
                                        try
                                        {
                                            URL = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[Module]
                                                .ServerConnectionList()[Source].BaseURL;
                                            URL += ProjectID;
                                        }
                                        catch (System.Exception ex)
                                        {
                                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                                        }

                                        if (URL.Length == 0)
                                        {
                                            string PropertyURI =
                                                DiversityCollection.LookupTable.PropertyURI(PropertyID);
                                            if (PropertyURI.Length > 0)
                                                URL = PropertyURI;
                                        }

                                        if (URL.Length > 0)
                                        {
                                            string Property = "";
                                            string BaseURL = DiversityWorkbench.WorkbenchUnit.getBaseURIfromURI(URL);
                                            bool ResetChart = false;
                                            if (this._ParentWidth != this.ParentForm.Width)
                                            {
                                                this._ParentWidth = this.ParentForm.Width;
                                                this._ParentHeight = this.ParentForm.Height;
                                                ResetChart = true;
                                            }

                                            DiversityWorkbench.Chart C = null;
                                            switch (moduleType)
                                            {
                                                case DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms:
                                                    C = DiversityWorkbench.Terminology.GetChart(URL, iiSectionID,
                                                        this._ParentHeight, this._ParentWidth); //, null, ResetChart);
                                                    Property = DiversityCollection.LookupTable.PropertyName(PropertyID);
                                                    //this.userControlModuleRelatedEntryIdentificationScientificTerm.EnableChart(C, BaseURL, "Please select an item from " + Property);
                                                    break;
                                                case DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames:
                                                    C = DiversityWorkbench.TaxonName.GetChart(URL, iiSectionID,
                                                        this._ParentHeight, this._ParentWidth); //, null, ResetChart);
                                                    Property = DiversityCollection.LookupTable.PropertyName(PropertyID);
                                                    //this.userControlModuleRelatedEntryTaxonomicName.EnableChart(C, BaseURL, "Please select an item from " + Source);
                                                    break;
                                            }

                                            if (C == null)
                                            {
                                                switch (moduleType)
                                                {
                                                    case DiversityWorkbench.WorkbenchUnit.ModuleType.ScientificTerms:
                                                        //this.userControlModuleRelatedEntryIdentificationScientificTerm.setChart(false, true);
                                                        break;
                                                    case DiversityWorkbench.WorkbenchUnit.ModuleType.TaxonNames:
                                                        //this.userControlModuleRelatedEntryTaxonomicName.setChart(false, true);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                        }
                    }
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Template

        private void buttonTemplateIdentificationSet_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.TemplateForData T = new DiversityWorkbench.TemplateForData("Identification", TemplateIdentificationSuppressedColumns);
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            T.CopyTemplateToRow(R.Row);
        }

        private System.Collections.Generic.List<string> TemplateIdentificationSuppressedColumns
        {
            get
            {
                System.Collections.Generic.List<string> Suppress = new List<string>();
                Suppress.Add("CollectionSpecimenID");
                Suppress.Add("IdentificationUnitID");
                Suppress.Add("IdentificationSequence");
                Suppress.Add("IdentificationDate");
                Suppress.Add("LogCreatedWhen");
                Suppress.Add("LogCreatedBy");
                Suppress.Add("LogUpdatedWhen");
                Suppress.Add("LogUpdatedBy");
                Suppress.Add("RowGUID");
                return Suppress;
            }
        }

        private void buttonTemplateIdentificationEdit_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = ((System.Data.DataRowView)this._Source.Current).Row;
            DiversityWorkbench.Forms.FormTemplateEditor f = new DiversityWorkbench.Forms.FormTemplateEditor("Identification", R, this.TemplateIdentificationSuppressedColumns);
            f.setHelp("Template");
            f.ShowDialog();
        }

        #endregion

    }
}
