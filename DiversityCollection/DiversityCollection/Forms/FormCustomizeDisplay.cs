using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace DiversityCollection.Forms
{
    public partial class FormCustomizeDisplay : Form
    {

        #region Parameter

        public enum Customization { All, StorageLocation, MaterialCategory, MapLocalisations, Transaction, Responsible, Autocomplete }
        private Customization _Customize;

        private System.Collections.Generic.Dictionary<string, bool> _ShowTaxa = new Dictionary<string, bool>();
        private System.Collections.Generic.Dictionary<string, bool> _ShowMaterial = new Dictionary<string, bool>();
        private System.Collections.Generic.Dictionary<string, bool> _ShowLocalisations = new Dictionary<string, bool>();
        private System.Collections.Generic.Dictionary<string, bool> _ShowMaps = new Dictionary<string, bool>();
        private System.Collections.Generic.Dictionary<string, bool> _ShowProperties = new Dictionary<string, bool>();

        private bool _ShowTaxaHasChanges = false;
        private bool _ShowMaterialHasChanges = false;
        private bool _ShowLocalisationsHasChanges = false;
        private bool _ShowMapsHasChanges = false;
        private bool _ShowPropertiesHasChanges = false;
        private bool _DefaultsHaveChanges = false;
        private string _Language;
        private string _Context;
        private System.Collections.Generic.List<System.Windows.Forms.CheckBox> _ResponsibleUsageChecks;
        private enum StorageLocationSource { Taxa, Database, Collection, Text };
        public enum SubcollectionContentDisplayText { AccessionNumber, PartNumber, PartSublabel, StorageLocation, Locality, CollectionDate, Identification }
      
        #endregion

        #region Construction

        public FormCustomizeDisplay(System.Windows.Forms.ImageList Images, Customization Customize)
        {
            InitializeComponent();
            this._Customize = Customize;
            this.treeViewTaxonomicGroups.ImageList = Images;
            this.treeViewMaterialCategories.ImageList = Images;
            this.treeViewLocalisationSystems.ImageList = Images;
            this.treeViewMaps.ImageList = Images;
            this.treeViewCollectionSiteProperties.ImageList = Images;
            this.initForm();
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }
        
        #endregion

        #region Form

        private void FormCustomizeDisplay_Load(object sender, EventArgs e)
        {
            this.transactionCommentTableAdapter.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            this.transactionCommentTableAdapter.Fill(this.dataSetTransactionComment.TransactionComment);
        }

        private void initForm()
        {
            try
            {
                this.imageList.Images.Add(DiversityCollection.Resource.DistributionMap);
                this.tabPageDistribution.ImageIndex = 10;

                this.initTaxonomicGroups();
                this.initMaterialCategories();
                this.initLocalisationSystems();
                this.initMaps();
                this.initCollectionSiteProperties();
                this.initStorageLocationSource();
                this.userControlDialogPanel.buttonOK.Click += new System.EventHandler(this.ReadTreesToDictionaries);
                this.setDefaultResponsibe();
                this.setMiscellaneous();
                this.initGazetteerHierarchy();
                this.initCountryListSource();
                //this.initSettings();
                this.initSubcollectionDiplayText();
                this.initDefaultCollection();
                this.initCollectionLocation();
                this.initContextIndication();
                //Handled in separate form
                this.tabControl.TabPages.Remove(this.tabPageContextLanguage);
                //this.initLanguage();
                //this.setLanguageImage();
                //this.initContext();
                //this.setContextInformations();
                //DiversityWorkbench.Entity.setEntity(this, this.toolTip);
                this.setTransactionSortingControls();
                this.setTransactionCommentControls();
                this.setTemplateControls();
                this.initResourcesDirectory();
                this.initScientificTermsRemoteParameter();
                this.initAutoComplete();
                this.initAutocompletion();
                this.initQueryCharts();
                this.initTooltipTime();
                this.initQuery();

                switch (this._Customize)
                {
                    case Customization.StorageLocation:
                        this.tabControl.TabPages.Remove(this.tabPageTransaction);
                        this.tabControl.TabPages.Remove(this.tabPageSpecimen);
                        this.tabControl.TabPages.Remove(this.tabPageEvent);
                        this.tabControl.TabPages.Remove(this.tabPageContextLanguage);
                        this.tabControl.TabPages.Remove(this.tabPageDistribution);
                        this.tabControl.TabPages.Remove(this.tabPageAutoComplete);

                        this.pictureBoxResponsible.Visible = false;
                        this.groupBoxResponsible.Visible = false;
                        this.pictureBoxTemplate.Visible = false;
                        this.groupBoxTemplate.Visible = false;
                        this.groupBoxMiscellaneous.Visible = false;
                        this.Height = 200;
                        break;
                    case Customization.MaterialCategory:
                        this.tabControl.TabPages.Remove(this.tabPageTransaction);
                        this.tabControl.TabPages.Remove(this.tabPageDefaults);
                        this.tabControl.TabPages.Remove(this.tabPageEvent);
                        this.tabControl.TabPages.Remove(this.tabPageContextLanguage);
                        this.tabControl.TabPages.Remove(this.tabPageDistribution);
                        this.tabControl.TabPages.Remove(this.tabPageAutoComplete);
                        this.buttonTaxaSelectAll.Enabled = false;
                        this.buttonTaxonSelectNone.Enabled = false;
                        this.groupBoxTaxonomicGroup.Enabled = false;
                        this.buttonTaxonomicGroupsForDiversityMobile.Enabled = false;
                        break;
                    case Customization.MapLocalisations:
                        this.tabControl.TabPages.Remove(this.tabPageTransaction);
                        this.tabControl.TabPages.Remove(this.tabPageDefaults);
                        this.tabControl.TabPages.Remove(this.tabPageSpecimen);
                        this.tabControl.TabPages.Remove(this.tabPageContextLanguage);
                        this.tabControl.TabPages.Remove(this.tabPageContextLanguage);
                        this.tabControl.TabPages.Remove(this.tabPageDistribution);
                        this.tabControl.TabPages.Remove(this.tabPageSettings);
                        this.tabControl.TabPages.Remove(this.tabPageAutoComplete);
                        this.splitContainerCollectionEvent.Panel2Collapsed = true;
                        this.Width = 550;
                        this.Height = 520;
                        this.groupBoxCountryListSource.Visible = false;
                        this.groupBoxGazetteerHierarchy.Visible = false;
                        this.buttonTaxaSelectAll.Enabled = false;
                        this.buttonTaxonSelectNone.Enabled = false;
                        this.groupBoxTaxonomicGroup.Enabled = false;
                        this.buttonTaxonomicGroupsForDiversityMobile.Enabled = false;
                        break;
                    case Customization.Transaction:
                        this.tabControl.TabPages.Remove(this.tabPageSpecimen);
                        this.tabControl.TabPages.Remove(this.tabPageDefaults);
                        this.tabControl.TabPages.Remove(this.tabPageEvent);
                        this.tabControl.TabPages.Remove(this.tabPageContextLanguage);
                        this.tabControl.TabPages.Remove(this.tabPageDistribution);
                        this.tabControl.TabPages.Remove(this.tabPageAutoComplete);
                        this.buttonTaxaSelectAll.Enabled = false;
                        this.buttonTaxonSelectNone.Enabled = false;
                        this.groupBoxTaxonomicGroup.Enabled = false;
                        this.buttonTaxonomicGroupsForDiversityMobile.Enabled = false;
                        break;
                    case Customization.Responsible:
                        this.tabControl.TabPages.Remove(this.tabPageTransaction);
                        this.tabControl.TabPages.Remove(this.tabPageSpecimen);
                        this.tabControl.TabPages.Remove(this.tabPageEvent);
                        this.tabControl.TabPages.Remove(this.tabPageContextLanguage);
                        this.tabControl.TabPages.Remove(this.tabPageDistribution);
                        this.tabControl.TabPages.Remove(this.tabPageSettings);
                        this.tabControl.TabPages.Remove(this.tabPageAutoComplete);
                        //Timeout
                        this.pictureBoxTimeout.Visible = false;
                        this.groupBoxTimeout.Visible = false;
                        // storage location
                        this.pictureBoxStorageLocation.Visible = false;
                        this.groupBoxStorageLocationSource.Visible = false;
                        // template
                        this.pictureBoxTemplate.Visible = false;
                        this.groupBoxTemplate.Visible = false;
                        //subcollection
                        this.pictureBoxSubcollection.Visible = false;
                        this.groupBoxSubcollectionContentDisplayText.Visible = false;
                        // default collection
                        this.pictureBoxDefaultCollection.Visible = false;
                        this.groupBoxDefaultCollection.Visible = false;
                        //misc
                        this.groupBoxMiscellaneous.Visible = false;
                        // resources
                        this.pictureBoxResourcesDirectory.Visible = false;
                        this.groupBoxResourcesDirectory.Visible = false;
                        this.Width = 550;
                        this.Height = 300;
                        break;
                    case Customization.Autocomplete:
                        this.tabControl.TabPages.Remove(this.tabPageTransaction);
                        this.tabControl.TabPages.Remove(this.tabPageDefaults);
                        this.tabControl.TabPages.Remove(this.tabPageSpecimen);
                        this.tabControl.TabPages.Remove(this.tabPageEvent);
                        this.tabControl.TabPages.Remove(this.tabPageContextLanguage);
                        this.tabControl.TabPages.Remove(this.tabPageDistribution);
                        this.tabControl.TabPages.Remove(this.tabPageSettings);

                        break;
                    default:
                        this.tabControl.TabPages.Remove(this.tabPageDistribution);
                        break;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void ReadTreesToDictionaries(object sender, EventArgs e)
        {
            string TestVisibility = this.VisibilityTaxonomicGroups;
            this.ReadTreeToDict(this.treeViewTaxonomicGroups, this._ShowTaxa);
            if (TestVisibility != this.VisibilityTaxonomicGroups) this._ShowTaxaHasChanges = true;

            TestVisibility = this.VisibilityMaterialCategories;
            this.ReadTreeToDict(this.treeViewMaterialCategories, this._ShowMaterial);
            if (TestVisibility != this.VisibilityMaterialCategories) this._ShowMaterialHasChanges = true;

            TestVisibility = this.VisibilityLocalisations;
            this.ReadTreeToDict(this.treeViewLocalisationSystems, this._ShowLocalisations);
            if (TestVisibility != this.VisibilityLocalisations) this._ShowLocalisationsHasChanges = true;

            TestVisibility = this.VisibilityMaps;
            this.ReadTreeToDict(this.treeViewMaps, this._ShowMaps);
            if (TestVisibility != this.VisibilityMaps) this._ShowMapsHasChanges = true;

            TestVisibility = this.VisibilityCollectionSiteProperties;
            this.ReadTreeToDict(this.treeViewCollectionSiteProperties, this._ShowProperties);
            if (TestVisibility != this.VisibilityCollectionSiteProperties) this._ShowPropertiesHasChanges = true;
        }

        private void ReadTreeToDict(System.Windows.Forms.TreeView Tree, System.Collections.Generic.Dictionary<string, bool> Dict)
        {
            try
            {
                foreach (System.Windows.Forms.TreeNode N in Tree.Nodes)
                {
                    if (N.Tag != null && Dict.ContainsKey(N.Tag.ToString()))
                    {
                        if (N.Checked)
                            Dict[N.Tag.ToString()] = true;
                        else Dict[N.Tag.ToString()] = false;
                    }
                    this.ReadTreeToDictChilds(N, Tree, Dict);
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ReadTreeToDictChilds(System.Windows.Forms.TreeNode Node, System.Windows.Forms.TreeView Tree, System.Collections.Generic.Dictionary<string, bool> Dict)
        {
            try
            {
                foreach (System.Windows.Forms.TreeNode N in Node.Nodes)
                {
                    if (N.Tag != null && Dict.ContainsKey(N.Tag.ToString()))
                    {
                        if (N.Checked)
                            Dict[N.Tag.ToString()] = true;
                        else Dict[N.Tag.ToString()] = false;
                    }
                    this.ReadTreeToDictChilds(N, Tree, Dict);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FormCustomizeDisplay_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.userControlModuleRelatedEntryDefaultResponsible.textBoxValue.Text != DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsible
                || this.userControlModuleRelatedEntryDefaultResponsible.labelURI.Text != DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleURI)
            {
                this._DefaultsHaveChanges = true;
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleURI = this.userControlModuleRelatedEntryDefaultResponsible.labelURI.Text;
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsible = this.userControlModuleRelatedEntryDefaultResponsible.textBoxValue.Text;
            }
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleCurrent != this.radioButtonResponsibleCurrent.Checked)
            {
                this._DefaultsHaveChanges = true;
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleCurrent = this.radioButtonResponsibleCurrent.Checked;
            }

            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups != this.VisibilityTaxonomicGroups)
            {
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups = this.VisibilityTaxonomicGroups;
                this._ShowTaxaHasChanges = true;
            }
            //DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups = this.TaxonomicGroups;

            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories != this.VisibilityMaterialCategories)
            {
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories = this.VisibilityMaterialCategories;
                this._ShowMaterialHasChanges = true;
            }
            //DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories = this.MaterialCategories;

            DiversityWorkbench.Settings.ScannedModuleDoScan(DiversityWorkbench.WorkbenchUnit.ModuleType.Descriptions, this.checkBoxScanDiversityDescriptons.Checked);
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ScanDiversityDescriptons = this.checkBoxScanDiversityDescriptons.Checked;

            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation = this.checkBoxUseLocationForCollection.Checked;

            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowExsiccata = this.checkBoxShowExsiccata.Checked;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames = this.checkBoxShowAcceptedNames.Checked;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowValidAgentName = this.checkBoxShowValidAgent.Checked;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            try
            {
                this.transactionCommentTableAdapter.Update(this.dataSetTransactionComment);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            if (DiversityWorkbench.Settings.ToolTipAutoPopDelay != this.toolTip.AutoPopDelay)
            {
                DiversityWorkbench.Settings.ToolTipAutoPopDelay = this.toolTip.AutoPopDelay;
                System.Windows.Forms.MessageBox.Show("Setting for tool tip time to \r\n" + this.labelToolTipTimeExplained.Text + "\r\n will be effectiv after restart");
            }

            DiversityWorkbench.Settings.UseEntity = this.checkBoxUseEntity.Checked;

            DiversityWorkbench.Settings.LoadConnections = this.checkBoxLoadConnections.Checked;

        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Defaults

        private void setDefaultResponsibe()
        {
            this.userControlModuleRelatedEntryDefaultResponsible.textBoxValue.Text = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsible;
            this.userControlModuleRelatedEntryDefaultResponsible.labelURI.Text = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleURI;
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleCurrent)
                this.radioButtonResponsibleCurrent.Checked = true;
            else
                this.radioButtonResponsibleSpecified.Checked = true;
            for (int i = 0; i < DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage.Length; i++)
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultResponsibleUsage[i] == '0')
                    this.ResponsibleUsageChecks[i].Checked = false;
                else
                    this.ResponsibleUsageChecks[i].Checked = true;
            }
            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.userControlModuleRelatedEntryDefaultResponsible.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
            this.userControlModuleRelatedEntryDefaultResponsible.setTableSource("CollectionAgent", "CollectorsName", "CollectorsAgentURI", "");
            this.labelResponsibleGivenName.Text = DiversityWorkbench.Settings.CurrentUserName();
            this.labelResponsibleGivenUri.Text = DiversityWorkbench.Settings.CurrentUserUri();
        }

        System.Collections.Generic.List<System.Windows.Forms.CheckBox> ResponsibleUsageChecks
        {
            get
            {
                if (this._ResponsibleUsageChecks == null)
                {
                    this._ResponsibleUsageChecks = new List<CheckBox>();
                    this._ResponsibleUsageChecks.Add(this.checkBoxDefaultResponsibleIdentitication); //0
                    this._ResponsibleUsageChecks.Add(this.checkBoxDefaultCollector); //1
                    this._ResponsibleUsageChecks.Add(this.checkBoxDefaultLocalisationResponsible); //2
                    this._ResponsibleUsageChecks.Add(this.checkBoxDefaultEventPropertyResponsible); //3
                    this._ResponsibleUsageChecks.Add(this.checkBoxDefaultAnalysisResponsible); //4
                    this._ResponsibleUsageChecks.Add(this.checkBoxDefaultProcessingResponsible); //5
                    this._ResponsibleUsageChecks.Add(this.checkBoxDefaultTaskResponsible); //6
                }
                return this._ResponsibleUsageChecks;
            }
        }

        public string DefaultResponsibe { get { return this.userControlModuleRelatedEntryDefaultResponsible.textBoxValue.Text; } }

        public string DefaultResponsibeUsage
        {
            get
            {
                string ResponsibleUsageCheck = "";
                foreach (System.Windows.Forms.CheckBox C in this.ResponsibleUsageChecks)
                {
                    if (C.Checked) ResponsibleUsageCheck += "1";
                    else ResponsibleUsageCheck += "0";
                }
                return ResponsibleUsageCheck;
            }
        }
        
        private void checkBoxDefaultResponsibleIdentitication_CheckedChanged(object sender, EventArgs e)
        {
            this._DefaultsHaveChanges = true;
        }

        private void checkBoxDefaultCollector_CheckedChanged(object sender, EventArgs e)
        {
            this._DefaultsHaveChanges = true;
        }

        private void checkBoxDefaultLocalisationResponsible_CheckedChanged(object sender, EventArgs e)
        {
            this._DefaultsHaveChanges = true;
        }

        private void checkBoxDefaultEventPropertyResponsible_CheckedChanged(object sender, EventArgs e)
        {
            this._DefaultsHaveChanges = true;
        }

        private void checkBoxDefaultAnalysisResponsible_CheckedChanged(object sender, EventArgs e)
        {
            this._DefaultsHaveChanges = true;
        }

        private void checkBoxDefaultProcessingResponsible_CheckedChanged(object sender, EventArgs e)
        {
            this._DefaultsHaveChanges = true;
        }

        private void checkBoxDefaultTaskResponsible_CheckedChanged(object sender, EventArgs e)
        {
            this._DefaultsHaveChanges = true;
        }

        private void radioButtonResponsibleCurrent_CheckedChanged(object sender, EventArgs e)
        {
            this._DefaultsHaveChanges = true;
        }

        #endregion

        #region Specimen

        private void initContextIndication()
        {
            if (DiversityWorkbench.Settings.Context != "General")
            {
                this.checkBoxSpecimenIndicateContext.Visible = true;
                this.checkBoxSpecimenIndicateContext.Text += " Current context: " + DiversityWorkbench.Settings.Context + ", current language: " + DiversityWorkbench.Settings.Language;
            }
            else
                this.checkBoxSpecimenIndicateContext.Visible = false;
        }

        private void checkBoxSpecimenIndicateContext_CheckedChanged(object sender, EventArgs e)
        {
            this._ShowMaterial.Clear();
            this.treeViewMaterialCategories.Nodes.Clear();
            this.initMaterialCategories();
            this._ShowTaxa.Clear();
            this.treeViewTaxonomicGroups.Nodes.Clear();
            this.initTaxonomicGroups();
        }

        #region Material Categories

        private void initMaterialCategories()
        {
            for (int i = 0; i < DiversityCollection.LookupTable.DtMaterialCategories.Rows.Count; i++)
            {
                bool Show = true;
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories.Length > 0)
                {
                    try
                    {
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories.Length > i)
                        {
                            string s = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories.Substring(i, 1);
                            if (s == "0") Show = false;
                        }
                    }
                    catch { }
                }
                this._ShowMaterial.Add(DiversityCollection.LookupTable.DtMaterialCategories.Rows[i]["Code"].ToString(), Show);
            }
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtMaterialCategories.Select("ParentCode IS NULL");

            foreach (System.Data.DataRow R in RR)
            {
                string Visibility = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.MaterialCategories;
                string Material = R["DisplayText"].ToString();
                bool ContextDiffers = false;
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("CollMaterialCategory_Enum.Code." + R["Code"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (DiversityWorkbench.Entity.IsDefined("CollMaterialCategory_Enum.Code." + R["Code"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language))
                        ContextDiffers = true;
                    Material = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Material);
                if (ContextDiffers && this.checkBoxSpecimenIndicateContext.Checked)
                {
                    N.ForeColor = System.Drawing.Color.Blue;
                    N.BackColor = System.Drawing.Color.Pink;
                }
                N.Tag = R["Code"].ToString();
                N.ImageIndex = DiversityCollection.Specimen.MaterialCategoryImage(R["Code"].ToString(), false);
                N.SelectedImageIndex = DiversityCollection.Specimen.MaterialCategoryImage(R["Code"].ToString(), false);
                if (Entity.Count == 0)
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = this._ShowMaterial[R["Code"].ToString()];
                this.treeViewMaterialCategories.Nodes.Add(N);
                this.addMaterialCategoryChilds(this._ShowMaterial, N);
            }
            this.treeViewMaterialCategories.ExpandAll();
        }

        private void addMaterialCategoryChilds(
            System.Collections.Generic.Dictionary<string, bool> ShowMaterial,
            System.Windows.Forms.TreeNode ParentNode)
        {
            string ParentCode = ParentNode.Tag.ToString();
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtMaterialCategories.Select("ParentCode = '" + ParentCode + "'");
            foreach (System.Data.DataRow R in RR)
            {
                bool ContextDiffers = false;
                string Material = R["DisplayText"].ToString();
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("CollMaterialCategory_Enum.Code." + R["Code"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (DiversityWorkbench.Entity.IsDefined("CollMaterialCategory_Enum.Code." + R["Code"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language))
                        ContextDiffers = true;
                    Material = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Material);
                if (ContextDiffers && this.checkBoxSpecimenIndicateContext.Checked)
                {
                    N.ForeColor = System.Drawing.Color.Blue;
                    N.BackColor = System.Drawing.Color.Pink;
                }
                N.Tag = R["Code"].ToString();
                N.ImageIndex = DiversityCollection.Specimen.MaterialCategoryImage(R["Code"].ToString(), false);
                N.SelectedImageIndex = DiversityCollection.Specimen.MaterialCategoryImage(R["Code"].ToString(), false);
                if (Entity.Count == 0)
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = ShowMaterial[R["Code"].ToString()];
                ParentNode.Nodes.Add(N);
                this.addMaterialCategoryChilds(ShowMaterial, N);
            }
        }

        private void treeViewMaterialCategories_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.labelMaterialCategoryDescrition.Text = this.treeViewMaterialCategories.SelectedNode.ToolTipText.ToString();
            this.initProjectMaterialCategory(this.treeViewMaterialCategories.SelectedNode.Text);
            //string Code = this.treeViewMaterialCategories.SelectedNode.Tag.ToString();
            //this.labelMaterialCategoryDescrition.Text = DiversityCollection.LookupTable.MaterialCategoryInfo(Code)["Description"];
        }

        private void treeViewMaterialCategories_AfterCheck(object sender, TreeViewEventArgs e)
        {
            System.Windows.Forms.TreeNode T = e.Node;
            if (T.Tag != null)
            {
                string Material = T.Tag.ToString();
                if (this._ShowMaterial.ContainsKey(Material))
                    this._ShowMaterial[Material] = T.Checked;
            }
        }

        private void buttonMaterialCategoryInDiversityMobile_Click(object sender, EventArgs e)
        {
            // read the current settings in the dictionary
            this.ReadTreeToDict(this.treeViewMaterialCategories, this._ShowMaterial);

            this.SaveSelectionToSettingsForDiversityMobile("MaterialCategories", this.MaterialCategories);
        }

        private void buttonMaterialSelectAll_Click(object sender, EventArgs e)
        {
            this.setSelectionOfChildNodes(null, null, this.treeViewMaterialCategories, true);
            // read the current settings in the dictionary
            this.ReadTreeToDict(this.treeViewMaterialCategories, this._ShowMaterial);
        }

        private void buttonMaterialSelectNone_Click(object sender, EventArgs e)
        {
            if (this.treeViewMaterialCategories.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select one node");
                return;
            }
            this.setSelectionOfChildNodes(this.treeViewMaterialCategories.SelectedNode, null, this.treeViewMaterialCategories, false);
            // read the current settings in the dictionary
            this.ReadTreeToDict(this.treeViewMaterialCategories, this._ShowMaterial);
        }

        private string MaterialCategories
        {
            get
            {
                string Groups = "";
                foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in this._ShowMaterial)
                {
                    if (!KV.Value)
                        continue;
                    if (Groups.Length > 0) Groups += "|";
                    Groups += KV.Key;
                }
                return Groups;
            }
        }


        #endregion

        #region Taxonomic groups

        private void initTaxonomicGroups()
        {
            for (int i = 0; i < DiversityCollection.LookupTable.DtTaxonomicGroups.Rows.Count; i++)
            {
                bool Show = true;
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups.Length > 0)
                {
                    try
                    {
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups.Length > i)
                        {
                            string s = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups.Substring(i, 1);
                            if (s == "0") Show = false;
                        }
                    }
                    catch { }
                }
                this._ShowTaxa.Add(DiversityCollection.LookupTable.DtTaxonomicGroups.Rows[i]["Code"].ToString(), Show);
            }
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTaxonomicGroups.Select("ParentCode IS NULL");

            foreach (System.Data.DataRow R in RR)
            {
                string Taxon = R["Code"].ToString();
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                bool ContextDiffers = false;
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("CollTaxonomicGroup_Enum.Code." + R["Code"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (DiversityWorkbench.Entity.IsDefined("CollTaxonomicGroup_Enum.Code." + R["Code"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language))
                        ContextDiffers = true;
                    Taxon = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Taxon);
                if (ContextDiffers && this.checkBoxSpecimenIndicateContext.Checked)
                {
                    N.ForeColor = System.Drawing.Color.Blue;
                    N.BackColor = System.Drawing.Color.Pink;
                }
                N.Tag = R["Code"].ToString();
                N.ImageIndex = DiversityCollection.Specimen.TaxonImage(R["Code"].ToString(), false);
                N.SelectedImageIndex = DiversityCollection.Specimen.TaxonImage(R["Code"].ToString(), false);
                if (Entity.Count == 0)
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = this._ShowTaxa[R["Code"].ToString()];
                this.treeViewTaxonomicGroups.Nodes.Add(N);
                this.addTaxonomicGroupChilds(this._ShowTaxa, N);
            }
            this.treeViewTaxonomicGroups.ExpandAll();
        }

        private void addTaxonomicGroupChilds(
            System.Collections.Generic.Dictionary<string, bool> ShowTaxa,
            System.Windows.Forms.TreeNode ParentNode)
        {
            string ParentCode = ParentNode.Tag.ToString();
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtTaxonomicGroups.Select("ParentCode = '" + ParentCode + "'");
            foreach (System.Data.DataRow R in RR)
            {
                string Taxon = R["Code"].ToString();
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                bool ContextDiffers = false;
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("CollTaxonomicGroup_Enum.Code." + R["Code"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (DiversityWorkbench.Entity.IsDefined("CollTaxonomicGroup_Enum.Code." + R["Code"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language))
                        ContextDiffers = true;
                    Taxon = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Taxon);
                if (ContextDiffers && this.checkBoxSpecimenIndicateContext.Checked)
                {
                    N.ForeColor = System.Drawing.Color.Blue;
                    N.BackColor = System.Drawing.Color.Pink;
                }
                N.Tag = R["Code"].ToString();
                N.ImageIndex = DiversityCollection.Specimen.TaxonImage(R["Code"].ToString(), false);
                N.SelectedImageIndex = DiversityCollection.Specimen.TaxonImage(R["Code"].ToString(), false);
                if (Entity.Count == 0)
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = ShowTaxa[R["Code"].ToString()];
                ParentNode.Nodes.Add(N);
                this.addTaxonomicGroupChilds(ShowTaxa, N);
            }
        }

        private void treeViewTaxonomicGroups_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.labelTaxonomicGroupDescription.Text = this.treeViewTaxonomicGroups.SelectedNode.ToolTipText.ToString();
            this.initProjectTaxonomicGroups(this.treeViewTaxonomicGroups.SelectedNode.Text);
            //string Code = this.treeViewTaxonomicGroups.SelectedNode.Tag.ToString();
            //this.labelTaxonomicGroupDescription.Text = DiversityCollection.LookupTable.TaxonomicGroupInfo(Code)["Description"];
        }

        private void treeViewTaxonomicGroups_AfterCheck(object sender, TreeViewEventArgs e)
        {
            System.Windows.Forms.TreeNode T = e.Node;
            if (T.Tag != null)
            {
                string Taxon = T.Tag.ToString();
                if (this._ShowTaxa.ContainsKey(Taxon))
                    this._ShowTaxa[Taxon] = T.Checked;
            }
        }

        private void buttonTaxonomicGroupsForDiversityMobile_Click(object sender, EventArgs e)
        {
            // read the current settings in the dictionary
            this.ReadTreeToDict(this.treeViewTaxonomicGroups, this._ShowTaxa);

            this.SaveSelectionToSettingsForDiversityMobile("TaxonomicGroups", this.TaxonomicGroups);
        }

        private void SaveSelectionToSettingsForDiversityMobile(string Setting, string SettingValue)
        {
            try
            {
                // check if settings contain anything
                string SQL = "SELECT CASE WHEN u.Settings IS NULL THEN 0 ELSE 1 END AS HasSettings " +
                    "FROM UserProxy AS u " +
                    "WHERE LoginName = USER_NAME()";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                string Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (Result == "0")
                {
                    C.CommandText = "UPDATE    U " +
                        "SET              Settings = '<Settings> <DiversityMobile> </DiversityMobile>  </Settings>' " +
                        "FROM         UserProxy AS U " +
                        "WHERE     (LoginName = USER_NAME())";
                }

                // check if settings have basic nodes 
                C.CommandText = "SELECT     Settings.exist('/Settings') AS Node " +
                    "FROM         UserProxy AS U " +
                    "WHERE     (LoginName = USER_NAME())";
                Result = C.ExecuteScalar()?.ToString();
                if (Result != "True")
                {
                    C.CommandText = "UPDATE    U " +
                        "SET              Settings = '<Settings> <DiversityMobile> </DiversityMobile>  </Settings>' " +
                        "FROM         UserProxy AS U " +
                        "WHERE     (LoginName = USER_NAME())";
                }

                // check if node exists
                C.CommandText = "SELECT     Settings.exist('/Settings/DiversityMobile/" + Setting + "') AS Node " +
                    "FROM         UserProxy AS U " +
                    "WHERE     (LoginName = USER_NAME())";
                Result = C.ExecuteScalar()?.ToString();
                if (Result != "True")
                {
                    C.CommandText = "DECLARE @DiversityMobile xml; " +
                        "SET @DiversityMobile = (SELECT Settings " +
                        "FROM UserProxy AS U " +
                        "WHERE LoginName = USER_NAME());" +
                        "set @DiversityMobile.modify('" +
                        "insert <" + Setting + ">" + SettingValue + "</" + Setting + ">   " +
                        "as first       " +
                        "into (/Settings/DiversityMobile)[1]'); " +
                        "update U set u.Settings = @DiversityMobile " +
                        "FROM [dbo].[UserProxy] U " +
                        "WHERE LoginName = USER_NAME();";
                    C.ExecuteNonQuery();
                }
                else
                {
                    char Hyphen = '"';
                    C.CommandText = "DECLARE @" + Setting + " xml; " +
                        "SET @" + Setting + " = (SELECT Settings " +
                        "FROM UserProxy AS U " +
                        "WHERE LoginName = USER_NAME()); " +
                        "set @" + Setting + ".modify('" +
                        "replace value of (/Settings/DiversityMobile/" + Setting + "/text())[1] with " + Hyphen + SettingValue + Hyphen + "'); " +
                        "update U set u.Settings = @" + Setting + " " +
                        "FROM [dbo].[UserProxy] U " +
                        "WHERE LoginName = USER_NAME();";
                    C.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private string TaxonomicGroups
        {
            get
            {
                string Groups = "";
                foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in this._ShowTaxa)
                {
                    if (!KV.Value)
                        continue;
                    if (Groups.Length > 0) Groups += "|";
                    Groups += KV.Key;
                }
                return Groups;
            }
        }

        private void buttonTaxaSelectAll_Click(object sender, EventArgs e)
        {
            this.setSelectionOfChildNodes(null, null, this.treeViewTaxonomicGroups, true);
            // read the current settings in the dictionary
            this.ReadTreeToDict(this.treeViewTaxonomicGroups, this._ShowTaxa);
        }

        private void buttonTaxonSelectNone_Click(object sender, EventArgs e)
        {
            if (this.treeViewTaxonomicGroups.SelectedNode == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select one node");
                return;
            }
            this.setSelectionOfChildNodes(this.treeViewTaxonomicGroups.SelectedNode, null, this.treeViewTaxonomicGroups, false);
            // read the current settings in the dictionary
            this.ReadTreeToDict(this.treeViewTaxonomicGroups, this._ShowTaxa);
        }

        private void setSelectionOfChildNodes(System.Windows.Forms.TreeNode SelectedNode
            , System.Windows.Forms.TreeNode ParentNode
            , System.Windows.Forms.TreeView TreeView
            , bool IsSelected)
        {

            try
            {
                if (SelectedNode == null && !IsSelected)
                    return;
                if (SelectedNode != null && !IsSelected && !SelectedNode.Checked)
                    SelectedNode.Checked = true;
                if (ParentNode == null)
                {
                    foreach (System.Windows.Forms.TreeNode N in TreeView.Nodes)
                    {
                        if (IsSelected || (N != SelectedNode && !IsSelected))
                            N.Checked = IsSelected;
                        this.setSelectionOfChildNodes(SelectedNode, N, TreeView, IsSelected);
                    }
                }
                else
                {
                    foreach (System.Windows.Forms.TreeNode N in ParentNode.Nodes)
                    {
                        if (IsSelected || (N != SelectedNode && !IsSelected))
                            N.Checked = IsSelected;
                        this.setSelectionOfChildNodes(SelectedNode, N, TreeView, IsSelected);
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Projects

        private void initProjectTaxonomicGroups(string Code)
        {
            this.initProjectEnum(this.panelProjectTaxonomicGroups, this.listBoxProjectTaxonomicGroups, Code, "CollTaxonomicGroup_Enum");
        }

        private void initProjectMaterialCategory(string Code)
        {
            this.initProjectEnum(this.panelProjectMaterialCategory, this.listBoxProjectMaterialCategory, Code, "CollMaterialCategory_Enum");
        }

        private void initProjectEnum(System.Windows.Forms.Panel panel, System.Windows.Forms.ListBox listBox, string Code, string EnumTable)
        {
            string SQL = "SELECT P.Project FROM ProjectProxy P INNER JOIN " + DiversityWorkbench.EnumTable.ProjectLinkTable(EnumTable) + " E ON P.ProjectID = E.ProjectID AND E." + DiversityWorkbench.EnumTable.ProjectLinkColumn(EnumTable) + " = '" + Code + "'";
            System.Data.DataTable dt = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            if (dt.Rows.Count > 0)
            {
                listBox.Visible = true;
                listBox.DataSource = dt;
                listBox.DisplayMember = "Project";
            }
            else listBox.Visible = false;
        }


        private void buttonProjectTaxonomicGroups_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
                    DiversityCollection.Resource.Plant,
                    "CollTaxonomicGroup_Enum",
                    "Administration of taxonomic groups",
                    "",
                    DiversityCollection.Specimen.TaxonomicGroup_Images);//, Directory);
                f.HierarchyChangesEnabled = true;
                f.setHelp("Taxonomic group");
                f.ShowDialog();
                if (f.DataHaveBeenChanged)
                {
                    DiversityWorkbench.CollectionSpecimen.TaxonomyRelatedTaxonomicGroups = null;
                    DiversityCollection.Specimen.TaxonomicGroup_Images = f.Images;
                    if (this.treeViewTaxonomicGroups.SelectedNode != null)
                        this.initProjectTaxonomicGroups(this.treeViewTaxonomicGroups.SelectedNode.Text);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonProjectMaterialCategory_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
                DiversityCollection.Resource.Specimen,
                "CollMaterialCategory_Enum",
                "Administration of material categories",
                "",
                DiversityCollection.Specimen.MaterialCategory_Images);
            f.HierarchyChangesEnabled = true;
            f.setHelp("Material Category");
            f.ShowDialog();
            if (f.DataHaveBeenChanged)
            {
                DiversityCollection.Specimen.MaterialCategory_Images = f.Images;
                if (this.treeViewMaterialCategories.SelectedNode != null)
                    this.initProjectMaterialCategory(this.treeViewMaterialCategories.SelectedNode.Text);
            }
        }

        #endregion

        #endregion

        #region Localisation systems

        private void initLocalisationSystems()
        {
            for (int i = 0; i < DiversityCollection.LookupTable.DtLocalisationSystem.Rows.Count; i++)
            {
                bool Show = true;
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems.Length > 0)
                {
                    try
                    {
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems.Length > i)
                        {
                            string s = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems.Substring(i, 1);
                            if (s == "0") Show = false;
                        }
                    }
                    catch { }
                }
                this._ShowLocalisations.Add(DiversityCollection.LookupTable.DtLocalisationSystem.Rows[i]["LocalisationSystemID"].ToString(), Show);
            }
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtLocalisationSystem.Select("LocalisationSystemParentID IS NULL");

            foreach (System.Data.DataRow R in RR)
            {
                string Visibility = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystems;
                string Locality = R["DisplayText"].ToString();
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID." + R["LocalisationSystemID"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (Entity.Count > 0 && Entity["DisplayTextOK"].ToString().ToLower() == "true")
                        Locality = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Locality);
                N.Tag = R["LocalisationSystemID"].ToString();
                if (R["ParsingMethodName"].ToString() == "Height")
                {
                    N.ImageIndex = DiversityCollection.Specimen.ImageForLocalisationSystem(R["LocalisationSystemName"].ToString(), false);
                    N.SelectedImageIndex = DiversityCollection.Specimen.ImageForLocalisationSystem(R["LocalisationSystemName"].ToString(), false);
                }
                else
                {
                    N.ImageIndex = DiversityCollection.Specimen.ImageForLocalisationSystem(R["ParsingMethodName"].ToString(), false);
                    N.SelectedImageIndex = DiversityCollection.Specimen.ImageForLocalisationSystem(R["ParsingMethodName"].ToString(), false);
                }
                if (Entity.Count == 0 || Entity["DescriptionOK"].ToString().ToLower() == "false")
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = this._ShowLocalisations[R["LocalisationSystemID"].ToString()];
                this.treeViewLocalisationSystems.Nodes.Add(N);
                this.addLocalisationSystemChilds(this._ShowLocalisations, N);
            }
            this.treeViewLocalisationSystems.ExpandAll();
        }

        private void addLocalisationSystemChilds(
            System.Collections.Generic.Dictionary<string, bool> ShowTaxa,
            System.Windows.Forms.TreeNode ParentNode)
        {
            string ParentCode = ParentNode.Tag.ToString();
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtLocalisationSystem.Select("LocalisationSystemParentID = '" + ParentCode + "'");
            foreach (System.Data.DataRow R in RR)
            {
                string Locality = R["DisplayText"].ToString();
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID." + R["LocalisationSystemID"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (Entity.Count > 0 && Entity["DisplayTextOK"].ToString().ToLower() == "true")
                        Locality = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Locality);
                N.Tag = R["LocalisationSystemID"].ToString();
                if (Entity.Count == 0 || Entity["DescriptionOK"].ToString().ToLower() == "false")
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = this._ShowLocalisations[R["LocalisationSystemID"].ToString()];
                ParentNode.Nodes.Add(N);
                this.addLocalisationSystemChilds(this._ShowLocalisations, N);
            }
        }

        private void treeViewLocalisationSystems_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.labelLocalisationDescription.Text = this.treeViewLocalisationSystems.SelectedNode.ToolTipText.ToString();
            //string Code = this.treeViewLocalisationSystems.SelectedNode.Tag.ToString();
            //this.labelLocalisationDescription.Text = DiversityCollection.LookupTable.LocalisationSystemInfo(Code)["Description"];
        }

        #endregion        

        #region Gazetteer - Hierarchy & Countrylist

        private void initGazetteerHierarchy()
        {
            this.radioButtonGazetteerHierarchyFromCountry.Checked = !DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry;
            this.radioButtonGazetteerHierarchyToCountry.Checked = DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry;
            this.textBoxGazetteerHierarchySeparator.Text = DiversityWorkbench.Settings.GazetteerHierarchySeparator;
        }

        private void initCountryListSource()
        {
            try
            {
                foreach (DiversityWorkbench.DatabaseService DS in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].DatabaseServices())
                {
                    this.comboBoxCountryListSource.Items.Add(DS.DisplayText);
                }
                this.comboBoxCountryListSource.Items.Add(DiversityWorkbench.Settings.ModuleName);
                if (DiversityWorkbench.Settings.CountryListSource.Length > 0)
                {
                    foreach (System.Object O in this.comboBoxCountryListSource.Items)
                    {
                        if (O.ToString() == DiversityWorkbench.Settings.CountryListSource)
                        {
                            this.comboBoxCountryListSource.SelectedItem = O;
                            break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void radioButtonGazetteerHierarchyFromCountry_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry = this.radioButtonGazetteerHierarchyToCountry.Checked;
        }

        private void radioButtonGazetteerHierarchyToCountry_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.GazetteerHierarchyPlaceToCountry = this.radioButtonGazetteerHierarchyToCountry.Checked;
        }

        private void textBoxGazetteerHierarchySeparator_TextChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.GazetteerHierarchySeparator = this.textBoxGazetteerHierarchySeparator.Text;
        }
        
        private void comboBoxCountryListSource_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DiversityWorkbench.Gazetteer.ResetCountries();
            LookupTable.ResetCountryFromGazetteer();
            if (this.comboBoxCountryListSource.SelectedItem != null)
                DiversityWorkbench.Settings.CountryListSource = this.comboBoxCountryListSource.SelectedItem.ToString();
        }

        #endregion

        #region Maps

        private void initMaps()
        {
            for (int i = 0; i < DiversityCollection.LookupTable.DtLocalisationSystem.Rows.Count; i++)
            {
                bool Show = true;
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystemsInMaps.Length > 0)
                {
                    try
                    {
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystemsInMaps.Length > i)
                        {
                            string s = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystemsInMaps.Substring(i, 1);
                            if (s == "0") Show = false;
                            string ParsingMethod = DiversityCollection.LookupTable.DtLocalisationSystem.Rows[i]["ParsingMethodName"].ToString();
                            if (ParsingMethod == "Altitude"
                                || ParsingMethod == "Height"
                                || ParsingMethod == "Exposition"
                                || ParsingMethod == "Slope")
                                Show = false;
                        }
                    }
                    catch { }
                }
                this._ShowMaps.Add(DiversityCollection.LookupTable.DtLocalisationSystem.Rows[i]["LocalisationSystemID"].ToString(), Show);
            }
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtLocalisationSystem.Select("LocalisationSystemParentID IS NULL");

            foreach (System.Data.DataRow R in RR)
            {
                string Visibility = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.LocalisationSystemsInMaps;
                string Locality = R["DisplayText"].ToString();
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID." + R["LocalisationSystemID"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (Entity.Count > 0 && Entity["DisplayTextOK"].ToString().ToLower() == "true")
                        Locality = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Locality);
                string ParsingMethod = R["ParsingMethodName"].ToString();
                if (ParsingMethod == "Altitude"
                    || ParsingMethod == "Height"
                    || ParsingMethod == "Exposition"
                    || ParsingMethod == "Slope")
                    continue;
                N.Tag = R["LocalisationSystemID"].ToString();
                N.ImageIndex = DiversityCollection.Specimen.ImageForLocalisationSystem(R["ParsingMethodName"].ToString(), false);
                N.SelectedImageIndex = DiversityCollection.Specimen.ImageForLocalisationSystem(R["ParsingMethodName"].ToString(), false);
                if (Entity.Count == 0 || Entity["DescriptionOK"].ToString().ToLower() == "false")
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = this._ShowMaps[R["LocalisationSystemID"].ToString()];
                this.treeViewMaps.Nodes.Add(N);
                this.addMapChilds(this._ShowMaps, N);
            }
            this.treeViewMaps.ExpandAll();
        }

        private void addMapChilds(
            System.Collections.Generic.Dictionary<string, bool> ShowTaxa,
            System.Windows.Forms.TreeNode ParentNode)
        {
            string ParentCode = ParentNode.Tag.ToString();
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtLocalisationSystem.Select("LocalisationSystemParentID = '" + ParentCode + "'");
            foreach (System.Data.DataRow R in RR)
            {
                string Locality = R["DisplayText"].ToString();
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID." + R["LocalisationSystemID"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (Entity.Count > 0 && Entity["DisplayTextOK"].ToString().ToLower() == "true")
                        Locality = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Locality);
                N.Tag = R["LocalisationSystemID"].ToString();
                if (Entity.Count == 0 || Entity["DescriptionOK"].ToString().ToLower() == "false")
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = this._ShowMaps[R["LocalisationSystemID"].ToString()];
                ParentNode.Nodes.Add(N);
                this.addMapChilds(this._ShowMaps, N);
            }
        }

        private void treeViewMaps_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.labelMapDescription.Text = this.treeViewMaps.SelectedNode.ToolTipText.ToString();
        }

        #endregion

        #region Collection site properties

        private void initCollectionSiteProperties()
        {
            for (int i = 0; i < DiversityCollection.LookupTable.DtProperty.Rows.Count; i++)
            {
                bool Show = true;
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.CollectionSiteProperties.Length > 0)
                {
                    try
                    {
                        if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.CollectionSiteProperties.Length > i)
                        {
                            string s = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.CollectionSiteProperties.Substring(i, 1);
                            if (s == "0") Show = false;
                        }
                    }
                    catch { }
                }
                this._ShowProperties.Add(DiversityCollection.LookupTable.DtProperty.Rows[i]["PropertyID"].ToString(), Show);
            }
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtProperty.Select("PropertyParentID IS NULL");

            foreach (System.Data.DataRow R in RR)
            {
                string Property = R["DisplayText"].ToString();
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("Property.PropertyID." + R["PropertyID"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (Entity.Count > 0 && Entity["DisplayTextOK"].ToString().ToLower() == "true")
                        Property = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Property);
                N.Tag = R["PropertyID"].ToString();
                N.ImageIndex = DiversityCollection.Specimen.ImageIndexForCollectionEventProperty(R["ParsingMethodName"].ToString(), false);
                N.SelectedImageIndex = DiversityCollection.Specimen.ImageIndexForCollectionEventProperty(R["ParsingMethodName"].ToString(), false);
                if (Entity.Count == 0 || Entity["DescriptionOK"].ToString().ToLower() == "false")
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = this._ShowProperties[R["PropertyID"].ToString()];
                this.treeViewCollectionSiteProperties.Nodes.Add(N);
                this.addCollectionSitePropertyChilds(this._ShowProperties, N);
            }
            this.treeViewCollectionSiteProperties.ExpandAll();
        }

        private void addCollectionSitePropertyChilds(
            System.Collections.Generic.Dictionary<string, bool> ShowTaxa,
            System.Windows.Forms.TreeNode ParentNode)
        {
            string ParentCode = ParentNode.Tag.ToString();
            System.Data.DataRow[] RR = DiversityCollection.LookupTable.DtProperty.Select("PropertyParentID = '" + ParentCode + "'");
            foreach (System.Data.DataRow R in RR)
            {
                string Property = R["DisplayText"].ToString();
                System.Collections.Generic.Dictionary<string, string> Entity = new Dictionary<string, string>();
                try
                {
                    Entity = DiversityWorkbench.Entity.EntityInformation("Property.PropertyID." + R["PropertyID"].ToString(), DiversityWorkbench.Settings.Context, DiversityWorkbench.Settings.Language);
                    if (Entity.Count > 0 && Entity["DisplayTextOK"].ToString().ToLower() == "true")
                        Property = Entity["DisplayText"];
                }
                catch { }
                System.Windows.Forms.TreeNode N = new TreeNode(Property);
                N.Tag = R["PropertyID"].ToString();
                if (Entity.Count == 0 || Entity["DescriptionOK"].ToString().ToLower() == "false")
                    N.ToolTipText = R["Description"].ToString();
                else
                {
                    N.ToolTipText = Entity["Description"];
                    if (Entity["Accessibility"] == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                        N.ForeColor = System.Drawing.Color.Gray;
                }
                N.Checked = ShowTaxa[R["PropertyID"].ToString()];
                ParentNode.Nodes.Add(N);
                this.addCollectionSitePropertyChilds(ShowTaxa, N);
            }
        }

        private void treeViewCollectionSiteProperties_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.labelPropertyDescription.Text = this.treeViewCollectionSiteProperties.SelectedNode.ToolTipText.ToString();
        }

        #endregion        

        #region Properties

        public bool DefaultsHaveChanges
        {
            get { return _DefaultsHaveChanges; }
        }

        public bool VisibilityOfTaxaHasChanges
        {
            get { return _ShowTaxaHasChanges; }
        }

        public bool VisibilityOfMaterialHasChanges
        {
            get { return _ShowMaterialHasChanges; }
        }

        public bool VisibilityOfLocalisationsHasChanges
        {
            get { return _ShowLocalisationsHasChanges; }
        }

        public bool VisibilityOfMapsHasChanges
        {
            get { return _ShowMapsHasChanges; }
        }

        public bool VisibilityOfPropertiesHasChanges
        {
            get { return _ShowPropertiesHasChanges; }
        }

        public bool LanguageHasChanged
        {
            get 
            {
                if (DiversityWorkbench.Settings.Language != this._Language)
                    return true;
                else return false;
            }
        }

        public bool ContextHasChanged
        {
            get
            {
                if (DiversityWorkbench.Settings.Context != this._Context)
                    return true;
                else return false;
            }
        }

        public string VisibilityTaxonomicGroups
        {
            get
            {
                return this.getVisibilityString(this._ShowTaxa);
            }
        }

        public string VisibilityMaterialCategories
        {
            get
            {
                return this.getVisibilityString(this._ShowMaterial);
            }
        }

        public string VisibilityLocalisations
        {
            get
            {
                return this.getVisibilityString(this._ShowLocalisations);
            }
        }

        public string VisibilityMaps
        {
            get
            {
                return this.getVisibilityString(this._ShowMaps);
            }
        }

        public string VisibilityCollectionSiteProperties
        {
            get
            {
                return this.getVisibilityString(this._ShowProperties);
            }
        }

        private string getVisibilityString(System.Collections.Generic.Dictionary<string, bool> Dict)
        {
            string V = "";
            foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in Dict)
            {
                if (KV.Value) V += "1";
                else V += "0";
            }
            return V;
        }

        #endregion

        #region Language and Context

        private void initLanguage()
        {
            this.comboBoxLanguage.DataSource = DiversityWorkbench.Entity.DtLanguage;
            try
            {
                this.comboBoxLanguage.DisplayMember = "DisplayText";
                this.comboBoxLanguage.ValueMember = "Code";
            }
            catch { }
            for (int i = 0; i < DiversityWorkbench.Entity.DtLanguage.Rows.Count; i++)
            {
                if (DiversityWorkbench.Entity.DtLanguage.Rows[i][0].ToString() == DiversityWorkbench.Settings.Language)
                {
                    this.comboBoxLanguage.SelectedIndex = i;
                    break;
                }
            }
            this._Language = DiversityWorkbench.Settings.Language;
        }

        private void setLanguageImage()
        {
            this.pictureBoxLanguage.Image = DiversityWorkbench.Entity.LanguageImage;
        }

        private void initContext()
        {
            //System.Data.DataTable dtContext = new DataTable();
            //string SQL = "SELECT DISTINCT EntityContext FROM dbo.EntityRepresentation " +
            //    " UNION SELECT DISTINCT EntityContext FROM EntityUsage ORDER BY EntityContext";
            //try
            //{
            //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //    ad.Fill(dtContext);
            //}
            //catch { }
            //if (dtContext.Rows.Count == 0)
            //{
            //    SQL = "SELECT 'General' AS [EntityContext] ";
            //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //    ad.Fill(dtContext);
            //}
            try
            {
                if (DiversityWorkbench.Entity.EntityTablesExist)
                {
                    this.comboBoxContext.DataSource = DiversityWorkbench.Entity.DtContext;
                    this.comboBoxContext.DisplayMember = DiversityWorkbench.Entity.DtContext.Columns[0].ColumnName; // "Code";
                    this.comboBoxContext.ValueMember = DiversityWorkbench.Entity.DtContext.Columns[1].ColumnName; // "DisplayText";
                    for (int i = 0; i < DiversityWorkbench.Entity.DtContext.Rows.Count; i++)
                    {
                        if (DiversityWorkbench.Entity.DtContext.Rows[i][0].ToString() == DiversityWorkbench.Settings.Context)
                        {
                            this.comboBoxContext.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
            catch { }
            this._Context = DiversityWorkbench.Settings.Context;
        }

        private void comboBoxLanguage_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBoxLanguage.SelectedValue.ToString() != "System.Data.DataRowView")
            {
                DiversityWorkbench.Settings.Language = this.comboBoxLanguage.SelectedValue.ToString();
                if (DiversityWorkbench.Settings.Language != System.Globalization.CultureInfo.CurrentUICulture.Name)
                {
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(DiversityWorkbench.Settings.Language);
                }
                this.setLanguageImage();
            }

        }

        private void comboBoxContext_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.Context = this.comboBoxContext.SelectedValue.ToString();
            this.setContextInformations();
        }

        private void setContextInformations()
        {
            string SQL = "SELECT Entity AS [" + DiversityWorkbench.Entity.EntityInformation("EntityUsage.Entity")["DisplayText"] + "]" +
                ", EntityUsage AS [" + DiversityWorkbench.Entity.EntityInformation("EntityUsage.EntityUsage")["DisplayText"] + "]" +
                ", PresetValue AS [" + DiversityWorkbench.Entity.EntityInformation("EntityUsage.PresetValue")["DisplayText"] + "]" +
                " FROM EntityUsage AS U " +
                "WHERE EntityContext = N'" + DiversityWorkbench.Settings.Context + "' " +
                "ORDER BY Entity, EntityUsage";
            System.Data.DataTable dtUsage = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                ad.Fill(dtUsage);
            }
            catch { }
            this.dataGridViewUsage.DataSource = dtUsage;
            string LanguageCode = DiversityWorkbench.Settings.Language;// System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0, 2);
            ad.SelectCommand.CommandText = "SELECT Entity AS [" + DiversityWorkbench.Entity.EntityInformation("EntityRepresentation.Entity")["DisplayText"] + "]" +
                ", DisplayText AS [" + DiversityWorkbench.Entity.EntityInformation("EntityRepresentation.DisplayText")["DisplayText"] + "]" +
                ", Abbreviation AS [" + DiversityWorkbench.Entity.EntityInformation("EntityRepresentation.Abbreviation")["DisplayText"] + "]" +
                ", Description AS [" + DiversityWorkbench.Entity.EntityInformation("EntityRepresentation.Description")["DisplayText"] + "]" +
                " FROM EntityRepresentation AS R " +
                "WHERE EntityContext = N'" + DiversityWorkbench.Settings.Context + "' AND LanguageCode = N'" +
                LanguageCode + "' ORDER BY Entity, DisplayText";
            System.Data.DataTable dtRepresentation = new DataTable();
            try
            {
                ad.Fill(dtRepresentation);
            }
            catch { }
            this.dataGridViewRepresentation.DataSource = dtRepresentation;
        }

        #endregion

        #region Miscellaneous
        
        private void setMiscellaneous()
        {
            this.checkBoxShowExsiccata.Checked = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowExsiccata;
            this.checkBoxShowAcceptedNames.Checked = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowAcceptedNames;
            this.checkBoxShowValidAgent.Checked = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ShowValidAgentName;

            this.checkBoxScanDiversityDescriptons.Checked = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ScanDiversityDescriptons;
            this.checkBoxScanDiversityDescriptons.Checked = DiversityWorkbench.Settings.ScannedModuleIsScanned(DiversityWorkbench.WorkbenchUnit.ModuleType.Descriptions);

            this.initEntity();
            this.initLoadConnections();

            try
            {
                this.numericUpDownTimeoutDatabase.Value = (decimal)DiversityWorkbench.Settings.TimeoutDatabase;///1000;
                this.numericUpDownTimeoutWeb.Value = (decimal)DiversityWorkbench.Settings.TimeoutWeb / 1000;
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #region Storage location source
        
        private void radioButtonStorageLocationFromTaxa_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationSource = StorageLocationSource.Taxa.ToString();
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            this.initStorageLocationSource();
        }

        private void radioButtonStorageLocationFromDatabase_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationSource = StorageLocationSource.Database.ToString();
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            this.initStorageLocationSource();
        }

        private void radioButtonStorageLocationFromCollection_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationSource = StorageLocationSource.Collection.ToString();
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            this.initStorageLocationSource();
        }

        private void radioButtonStorageLocationText_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationSource = StorageLocationSource.Text.ToString();
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            this.initStorageLocationSource();
        }

        private void textBoxStorageLocationText_Leave(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationText = this.textBoxStorageLocationText.Text;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
        }

        private void initStorageLocationSource()
        {
            switch (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationSource)
            {
                case "Taxa":
                    this.radioButtonStorageLocationFromTaxa.Checked = true;
                    break;
                case "Collection":
                    this.radioButtonStorageLocationFromCollection.Checked = true;
                    break;
                case "Database":
                    this.radioButtonStorageLocationFromDatabase.Checked = true;
                    break;
                case "Text":
                    this.radioButtonStorageLocationText.Checked = true;
                    break;
                default:
                    break;
            }
            this.textBoxStorageLocationText.Text = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationText;
        }

        #endregion

        #region Timeout

        private void numericUpDownTimeoutWeb_ValueChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.TimeoutWeb = (int)numericUpDownTimeoutWeb.Value*1000;
        }

        private void numericUpDownTimeoutDatabase_ValueChanged(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.TimeoutDatabase = (int)numericUpDownTimeoutDatabase.Value;// *1000;
        }

        #endregion

        #region AcceptedNames

        private void checkBoxShowAcceptedNames_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxShowAcceptedNames.Checked)
            {
                this.checkBoxShowAcceptedNames.ForeColor = System.Drawing.Color.Green;
                this.checkBoxShowAcceptedNames.BackColor = System.Drawing.SystemColors.Info;
            }
            else
            {
                this.checkBoxShowAcceptedNames.ForeColor = System.Drawing.SystemColors.WindowText;
                this.checkBoxShowAcceptedNames.BackColor = System.Drawing.SystemColors.ControlLightLight;
            }
        }
        #endregion

        #region Valid agent names

        private void checkBoxShowValidAgent_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxShowValidAgent.Checked)
            {
                this.checkBoxShowValidAgent.ForeColor = System.Drawing.Color.Green;
                this.checkBoxShowValidAgent.BackColor = System.Drawing.SystemColors.Info;
            }
            else
            {
                this.checkBoxShowValidAgent.ForeColor = System.Drawing.SystemColors.WindowText;
                this.checkBoxShowValidAgent.BackColor = System.Drawing.SystemColors.ControlLightLight;
            }
        }

        #endregion

        #region Entity

        private void initEntity()
        {
            this.checkBoxUseEntity.Checked = DiversityWorkbench.Settings.UseEntity;
        }

        #endregion

        #region LoadConnections

        private void initLoadConnections()
        {
            this.checkBoxLoadConnections.Checked = DiversityWorkbench.Settings.LoadConnections;
        }

        #endregion


        #endregion

        #region Autocompletion

        private void initAutocompletion()
        {
            if (DiversityWorkbench.Settings.FormLimitCharaterCount < 0)
                this.checkBoxAutocompletion.Checked = false;
            else
            {
                this.checkBoxAutocompletion.Checked = true;
                this.numericUpDownAutocompletion.Value = DiversityWorkbench.Settings.FormLimitCharaterCount;
            }
            this._AutoCompleteModes = new Dictionary<string, AutoCompleteMode>();
            _AutoCompleteModes.Add("None", AutoCompleteMode.None);
            _AutoCompleteModes.Add("Append", AutoCompleteMode.Append);
            _AutoCompleteModes.Add("Suggest", AutoCompleteMode.Suggest);
            _AutoCompleteModes.Add("Suggest & Append", AutoCompleteMode.SuggestAppend);
            int i = 0;
            int s = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, AutoCompleteMode> KV in _AutoCompleteModes)
            {
                this.comboBoxAutocompletionMode.Items.Add(KV.Key);
                if (KV.Value == DiversityWorkbench.Forms.FormFunctions.DefaultAutoCompleteMode()) s = i;
                i++;
            }
            this.comboBoxAutocompletionMode.SelectedIndex = s;
        }

        private System.Collections.Generic.Dictionary<string, System.Windows.Forms.AutoCompleteMode> _AutoCompleteModes;

        private void checkBoxAutocompletion_Click(object sender, EventArgs e)
        {
            if (this.checkBoxAutocompletion.Checked)
                DiversityWorkbench.Settings.FormLimitCharaterCount = (int)this.numericUpDownAutocompletion.Value;
            else DiversityWorkbench.Settings.FormLimitCharaterCount = -1;
            DiversityWorkbench.Settings.UseAutocompletion = this.checkBoxAutocompletion.Checked;
        }

        private void numericUpDownAutocompletion_Click(object sender, EventArgs e)
        {
            if (this.checkBoxAutocompletion.Checked)
                DiversityWorkbench.Settings.FormLimitCharaterCount = (int)this.numericUpDownAutocompletion.Value;
        }
        private void comboBoxAutocompletionMode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.SetDefaultAutoCompleteMode(this._AutoCompleteModes[this.comboBoxAutocompletionMode.SelectedItem.ToString()]);
        }

        #region Functions for autocompletion

        private void initAutoComplete()
        {
            try
            {
                this.initAutoCompleteTables();
                this.initAutoCompleteNotesControl();
                this.initAutoCompleteRestrictions();
                this.initAutoCompleteExclusions();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region TableColumns

        private void initAutoCompleteTables()
        {
            try
            {
                this.listBoxAutoCompleteTables.Items.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.AutoCompleteStringCollection> KV in DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionsOnDemand())
                {
                    this.listBoxAutoCompleteTables.Items.Add(KV.Key);
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonAutoCompleteResetAll_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //#275 - hier müssen auch die Felder in denen die Objekte verwendent werden zurückgesetzt werden
            //DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionExclusionClear();
            //this.initAutoCompleteTables();
        }

        private void buttonAutoCompleteResetTable_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            // #275  - hier müssen auch die Felder in denen die Objekte verwendent werden zurückgesetzt werden
            //if (this.listBoxAutoCompleteTables.SelectedItem != null) 
            //{ 
            //    string Key = this.listBoxAutoCompleteTables.SelectedItem.ToString();
            //    DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand_Remove(Key);
            //    this.initAutoCompleteTables();
            //}
        }

        private void buttonAutoCompleteTableContent_Click(object sender, EventArgs e)
        {
            if (this.listBoxAutoCompleteTables.SelectedItem != null)
            {
                string Key = this.listBoxAutoCompleteTables.SelectedItem.ToString();
                string Table = Key.Substring(0, Key.IndexOf('.'));
                string Column = Key.Substring(Key.IndexOf('.') + 1);
                System.Windows.Forms.AutoCompleteStringCollection autoComplete = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                int Count = autoComplete.Count;
                int Max = 50;
                //System.Windows.Forms.MessageBox.Show(Count.ToString() + " entries");
                System.Collections.IEnumerator enumerator = autoComplete.GetEnumerator();
                string Message = "";
                int i = 0;
                while(enumerator.MoveNext())
                {
                    i++;
                    Message += i.ToString() + ":\t";
                    Message += enumerator.Current.ToString() + "\r\n";
                    if (i >= 50)
                    {
                        Message += "...";
                        break;
                    }
                }
                if (Count > Max)
                    Message = "Content of column " + Column + " in table " + Table + "\r\n\r\nFirst " + Max.ToString() + " of " + Count.ToString() + " entries:\r\n\r\n" + Message;
                else
                    Message = "Content of column " + Column + " in table " + Table + "\r\n\r\n" + Count.ToString() + " entries:\r\n\r\n" + Message;
                System.Windows.Forms.MessageBox.Show(Message);
            }
        }

        #endregion

        #region Restrictions

        private void initAutoCompleteRestrictions()
        {
            this.listBoxAutoCompleteRestrictions.Items.Clear();
            foreach(System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.Forms.FormFunctions.getAutoCompleteRestrictions())
            {
                this.listBoxAutoCompleteRestrictions.Items.Add(KV.Key);
            }
        }

        private void buttonAutoCompleteRestrictionView_Click(object sender, EventArgs e)
        {
            if (this.listBoxAutoCompleteRestrictions.SelectedItem != null)
            {
                string Rest = this.listBoxAutoCompleteRestrictions.SelectedItem.ToString();
                string View = DiversityWorkbench.Forms.FormFunctions.getAutoCompleteRestrictions()[Rest];
                System.Data.DataTable dataTable = new DataTable();
                string SQL = "SELECT * FROM  " + View;
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
                DiversityWorkbench.Forms.FormTableContent formTableContent = new DiversityWorkbench.Forms.FormTableContent(View, "Restiction for column " + Rest, dataTable, true);
                formTableContent.ShowDialog();
            }
            else
                System.Windows.Forms.MessageBox.Show("Nothing selected");
        }

        #endregion

        #region Exclusions

        private void initAutoCompleteNotesControl()
        {
            this.checkBoxAutoCompleteExcludeNotes.Checked = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.AutoCompleteExcludes != null && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.AutoCompleteExcludes.Contains("Notes");
        }

        private void checkBoxAutoCompleteExcludeNotes_Click(object sender, EventArgs e)
        {
            if (this.checkBoxAutoCompleteExcludeInternalNotes.Checked && !DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.AutoCompleteExcludes.Contains("Notes"))
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.AutoCompleteExcludes.Add("Notes");
            else if (!this.checkBoxAutoCompleteExcludeInternalNotes.Checked && DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.AutoCompleteExcludes.Contains("Notes"))
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.AutoCompleteExcludes.Remove("Notes");
            this.initAutoCompleteNotesControl();
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.AutoCompleteExcludes.Contains("Notes"))
                DiversityCollection.LookupTable.AutoCompletionExcludeNotes();
            else
                DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionExclusionClear();
            this.initAutoCompleteExclusions();
        }

        private void checkBoxAutoCompleteExcludeInternalNotes_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        private void initAutoCompleteExclusions()
        {
            try
            {
                this.listBoxAutoCompleteExclusions.Items.Clear();
                foreach (string E in DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionExclusions)
                {
                    this.listBoxAutoCompleteExclusions.Items.Add(E);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }
        private void buttonAutoCompleteRemoveExclusion_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        #endregion

        #endregion

        #endregion

        #region Charts

        private void initQueryCharts()
        {
            this.checkBoxUseQueryCharts.Checked = DiversityWorkbench.Settings.UseQueryCharts;
        }
        private void checkBoxUseQueryCharts_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.UseQueryCharts = this.checkBoxUseQueryCharts.Checked;
        }

        #endregion

        #region Transaction

        private void setTransactionCommentControls()
        {
            string TableName = "TransactionComment";
            DiversityWorkbench.Forms.FormFunctions F = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
            bool OK = F.getObjectPermissions(TableName, "INSERT");
            this.dataGridViewTransactionComments.ReadOnly = !OK;
            this.toolStripTransactionComment.Enabled = OK;
            //if (DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator"))
            //{
            //    this.toolStripTransactionComment.Enabled = true;
            //    this.dataGridViewTransactionComments.ReadOnly = false;
            //}
            //else
            //{
            //    this.toolStripTransactionComment.Enabled = false;
            //}
        }
        
        private void toolStripButtonTransactionCommentDelete_Click(object sender, EventArgs e)
        {
            try
            {
                this.dataSetTransactionComment.TransactionComment.Rows[this.dataGridViewTransactionComments.SelectedCells[0].RowIndex].Delete();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Transaction sorting

        private void setTransactionSortingControls()
        {
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence != null &&
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionDisplaySequence.Count > 0)
            {
                this.toolStripButtonTransactionDisplaySequenceFill.Visible = false;
                this.toolStripButtonTransactionDisplaySequenceReset.Visible = true;
            }
            else
            {
                this.toolStripButtonTransactionDisplaySequenceFill.Visible = true;
                this.toolStripButtonTransactionDisplaySequenceReset.Visible = false;
            }
            this.listBoxTransactionDisplaySequence.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.LookupTable.TransactionDisplaySorter> KV in DiversityCollection.LookupTable.TransactionDisplaySorting())
            {
                this.listBoxTransactionDisplaySequence.Items.Add(KV.Key);
            }
            this.checkBoxTransactionIncludeParentTitle.Checked = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeParentTitle;
            this.checkBoxTransactionIncludeDateInTitle.Checked = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeDateInTitle;
            this.checkBoxTransactionShowAllAddressFields.Checked = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields;
        }

        private void toolStripButtonTransactionDisplaySequenceFill_Click(object sender, EventArgs e)
        {
            DiversityCollection.LookupTable.TransactionDisplaySortingSave();
            this.setTransactionSortingControls();
        }

        private void toolStripButtonTransactionDisplaySequenceUp_Click(object sender, EventArgs e)
        {
            if (this.listBoxTransactionDisplaySequence.SelectedItem != null && this.listBoxTransactionDisplaySequence.SelectedIndex > 0)
            {
                int i = DiversityCollection.LookupTable.TransactionDisplaySortingMove(this.listBoxTransactionDisplaySequence.SelectedItem.ToString(), true);
                this.setTransactionSortingControls();
                this.listBoxTransactionDisplaySequence.SelectedIndex = i;
            }
        }

        private void toolStripButtonTransactionDisplaySequenceDown_Click(object sender, EventArgs e)
        {
            if (this.listBoxTransactionDisplaySequence.SelectedItem != null && this.listBoxTransactionDisplaySequence.SelectedIndex < this.listBoxTransactionDisplaySequence.Items.Count - 1)
            {
                int i = DiversityCollection.LookupTable.TransactionDisplaySortingMove(this.listBoxTransactionDisplaySequence.SelectedItem.ToString(), false);
                this.setTransactionSortingControls();
                this.listBoxTransactionDisplaySequence.SelectedIndex = i;
            }
        }

        private void radioButtonTransactionDisplaySequenceDate_Click(object sender, EventArgs e)
        {
            if (this.listBoxTransactionDisplaySequence.SelectedItem != null)
            {
                DiversityCollection.LookupTable.TransactionDisplaySortingSetSorter(this.listBoxTransactionDisplaySequence.SelectedItem.ToString(), LookupTable.TransactionDisplaySorter.BeginDate);
            }
        }

        private void radioButtonTransactionDisplaySequenceTitle_Click(object sender, EventArgs e)
        {
            if (this.listBoxTransactionDisplaySequence.SelectedItem != null)
            {
                DiversityCollection.LookupTable.TransactionDisplaySortingSetSorter(this.listBoxTransactionDisplaySequence.SelectedItem.ToString(), LookupTable.TransactionDisplaySorter.TransactionTitle);
            }
        }

        private void radioButtonTransactionDisplaySequenceAccessionNumber_Click(object sender, EventArgs e)
        {
            if (this.listBoxTransactionDisplaySequence.SelectedItem != null)
            {
                DiversityCollection.LookupTable.TransactionDisplaySortingSetSorter(this.listBoxTransactionDisplaySequence.SelectedItem.ToString(), LookupTable.TransactionDisplaySorter.AccessionNumber);
            }
        }

        private void toolStripButtonTransactionDisplaySequenceReset_Click(object sender, EventArgs e)
        {
            DiversityCollection.LookupTable.TransactionDisplaySortingReset();
            this.setTransactionSortingControls();
        }

        private void toolStripButtonTransactionDisplaySequenceSave_Click(object sender, EventArgs e)
        {
            DiversityCollection.LookupTable.TransactionDisplaySortingSave();
        }

        private void listBoxTransactionDisplaySequence_SelectedIndexChanged(object sender, EventArgs e)
        {
            string s = this.listBoxTransactionDisplaySequence.SelectedItem.ToString();
            this.pictureBoxTransactionDisplaySequence.Image = Specimen.ImageListTransactionType().Images[Specimen.IndexImageTransactionType(s, false)];
            DiversityCollection.LookupTable.TransactionDisplaySorter TS = DiversityCollection.LookupTable.TransactionDisplaySorting()[s];
            switch (TS)
            {
                case LookupTable.TransactionDisplaySorter.AccessionNumber:
                    this.radioButtonTransactionDisplaySequenceAccessionNumber.Checked = true;
                    break;
                case LookupTable.TransactionDisplaySorter.BeginDate:
                    this.radioButtonTransactionDisplaySequenceDate.Checked = true;
                    break;
                case LookupTable.TransactionDisplaySorter.TransactionTitle:
                    this.radioButtonTransactionDisplaySequenceTitle.Checked = true;
                    break;
            }
        }

        private void checkBoxTransactionIncludeParentTitle_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeParentTitle = this.checkBoxTransactionIncludeParentTitle.Checked;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
        }

        private void checkBoxTransactionShowAllAddressFields_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionShowAllAddressFields = this.checkBoxTransactionShowAllAddressFields.Checked;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
        }
        
        private void checkBoxTransactionIncludeDateInTitle_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TransactionIncludeDateInTitle = this.checkBoxTransactionIncludeDateInTitle.Checked;
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
        }

        #endregion

        /// <summary>
        /// Changing the default currency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTransactionCurrency_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT COUNT(*) FROM TransactionPayment P WHERE NOT Amount IS NULL";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result != "0")
            {
                System.Windows.Forms.MessageBox.Show(Result + " payment(s) were found. Change of the default curreny is not allowed any more");
                return;
            }
            SQL ="SELECT [dbo].[TransactionCurrency] ()";
            string Currency = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Currency", "Enter the default currency for payments within the transactions", Currency);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                SQL = "ALTER FUNCTION [dbo].[TransactionCurrency] () RETURNS nvarchar(50) AS BEGIN RETURN '" + f.String.Replace("'", "''") + "' END";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
        }

        #endregion

        #region Template

        private void setTemplateControls()
        {
            switch (DiversityWorkbench.TemplateForData.OptionForFilling)
            {
                case DiversityWorkbench.TemplateForData.FillingOption.All:
                    this.radioButtonTemplateCopyAll.Checked = true;
                    break;
                case DiversityWorkbench.TemplateForData.FillingOption.OnlyEmpty:
                    this.radioButtonTemplateEmpty.Checked = true;
                    break;
                default:
                    this.radioButtonTemplateUserDecision.Checked = true;
                    break;
            }
        }

        private void radioButtonTemplateEmpty_Click(object sender, EventArgs e)
        {
            this.setTemplateFillingOption();
        }

        private void radioButtonTemplateUserDecision_Click(object sender, EventArgs e)
        {
            this.setTemplateFillingOption();
        }

        private void radioButtonTemplateCopyAll_Click(object sender, EventArgs e)
        {
            this.setTemplateFillingOption();
        }

        private void setTemplateFillingOption()
        {
            if (this.radioButtonTemplateCopyAll.Checked)
                DiversityWorkbench.TemplateForData.OptionForFilling = DiversityWorkbench.TemplateForData.FillingOption.All;
            else if (this.radioButtonTemplateEmpty.Checked)
                DiversityWorkbench.TemplateForData.OptionForFilling = DiversityWorkbench.TemplateForData.FillingOption.OnlyEmpty;
            else
                DiversityWorkbench.TemplateForData.OptionForFilling = DiversityWorkbench.TemplateForData.FillingOption.UserSelection;
            //DiversityWorkbench.Forms.FormTemplateEditor.SetTemplateFillingOption(DiversityWorkbench.TemplateForData.OptionForFilling);
        }

        #endregion

        #region SubcollectionDiplayText
        
        private void initSubcollectionDiplayText()
        {
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(SubcollectionContentDisplayText.AccessionNumber.ToString()))
                this.checkBoxSubcollectionContentAccessionNumber.Checked = true;
            else this.checkBoxSubcollectionContentAccessionNumber.Checked = false;
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(SubcollectionContentDisplayText.CollectionDate.ToString()))
                this.checkBoxSubcollectionContentCollectionDate.Checked = true;
            else this.checkBoxSubcollectionContentCollectionDate.Checked = false;

            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(SubcollectionContentDisplayText.Identification.ToString()))
                this.checkBoxSubcollectionContentIdentification.Checked = true;
            else this.checkBoxSubcollectionContentIdentification.Checked = false;

            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(SubcollectionContentDisplayText.Locality.ToString()))
                this.checkBoxSubcollectionContentLocality.Checked = true;
            else this.checkBoxSubcollectionContentLocality.Checked = false;

            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(SubcollectionContentDisplayText.PartNumber.ToString()))
                this.checkBoxSubcollectionContentPartNumber.Checked = true;
            else this.checkBoxSubcollectionContentPartNumber.Checked = false;

            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(SubcollectionContentDisplayText.PartSublabel.ToString()))
                this.checkBoxSubcollectionContentPartSublabel.Checked = true;
            else this.checkBoxSubcollectionContentPartSublabel.Checked = false;

            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Contains(SubcollectionContentDisplayText.StorageLocation.ToString()))
                this.checkBoxSubcollectionContentStorageLocation.Checked = true;
            else this.checkBoxSubcollectionContentStorageLocation.Checked = false;

            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubcollectionContentMaxCount > 0)
                this.maskedTextBoxSubcollectionContentCount.Text = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubcollectionContentMaxCount.ToString();
            else this.maskedTextBoxSubcollectionContentCount.Text = "";
        }

        private void checkBoxSubcollectionContentAccessionNumber_Click(object sender, EventArgs e)
        {
            this.SetSubcollectionDiplayText();
        }

        private void checkBoxSubcollectionContentPartNumber_Click(object sender, EventArgs e)
        {
            this.SetSubcollectionDiplayText();
        }

        private void checkBoxSubcollectionContentPartSublabel_Click(object sender, EventArgs e)
        {
            this.SetSubcollectionDiplayText();
        }

        private void checkBoxSubcollectionContentStorageLocation_Click(object sender, EventArgs e)
        {
            this.SetSubcollectionDiplayText();
        }

        private void checkBoxSubcollectionContentLocality_Click(object sender, EventArgs e)
        {
            this.SetSubcollectionDiplayText();
        }

        private void checkBoxSubcollectionContentCollectionDate_Click(object sender, EventArgs e)
        {
            this.SetSubcollectionDiplayText();
        }

        private void checkBoxSubcollectionContentIdentification_Click(object sender, EventArgs e)
        {
            this.SetSubcollectionDiplayText();
        }

        private void SetSubcollectionDiplayText()
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Clear();
            if (this.checkBoxSubcollectionContentAccessionNumber.Checked)
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Add(SubcollectionContentDisplayText.AccessionNumber.ToString());
            if (this.checkBoxSubcollectionContentCollectionDate.Checked)
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Add(SubcollectionContentDisplayText.CollectionDate.ToString());
            if (this.checkBoxSubcollectionContentIdentification.Checked)
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Add(SubcollectionContentDisplayText.Identification.ToString());
            if (this.checkBoxSubcollectionContentLocality.Checked)
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Add(SubcollectionContentDisplayText.Locality.ToString());
            if (this.checkBoxSubcollectionContentPartNumber.Checked)
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Add(SubcollectionContentDisplayText.PartNumber.ToString());
            if (this.checkBoxSubcollectionContentPartSublabel.Checked)
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Add(SubcollectionContentDisplayText.PartSublabel.ToString());
            if (this.checkBoxSubcollectionContentStorageLocation.Checked)
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubCollectionContentDisplayText.Add(SubcollectionContentDisplayText.StorageLocation.ToString());
        }
        
        private void maskedTextBoxSubcollectionContentCount_Leave(object sender, EventArgs e)
        {
            try
            {
                if (maskedTextBoxSubcollectionContentCount.Text.Length > 0)
                {
                    int Max;
                    if (int.TryParse(this.maskedTextBoxSubcollectionContentCount.Text, out Max))
                    {
                        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubcollectionContentMaxCount = Max;
                        DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                    }
                }
                else
                {
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.SubcollectionContentMaxCount = 0;
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                }
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region Default collection

        private void initDefaultCollection()
        {
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection > -1)
                this.comboBoxDefaultCollection.Text = DiversityCollection.LookupTable.CollectionName(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection);
        }

        private void comboBoxDefaultCollection_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxDefaultCollection.DataSource == null)
            {
                this.comboBoxDefaultCollection.DataSource = DiversityCollection.LookupTable.DtCollectionWithHierarchy;
                this.comboBoxDefaultCollection.DisplayMember = "DisplayText";
                this.comboBoxDefaultCollection.ValueMember = "CollectionID";
            }
        }
        
        private void comboBoxDefaultCollection_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                if (comboBoxDefaultCollection.SelectedValue.ToString().Length == 0)
                {
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection = -1;
                }
                else
                {
                    int CollectionID = int.Parse(comboBoxDefaultCollection.SelectedValue.ToString());
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.DefaultCollection = CollectionID;
                }
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Collection location

        private void initCollectionLocation()
        {
            this.checkBoxUseLocationForCollection.Checked = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
        }


        #endregion

        #region Resources directory

        private void initResourcesDirectory()
        {
            this.labelResourcesDirectory.Text = "Current resources directory: " + DiversityWorkbench.Settings.ResourcesDirectory;
        }

        private void buttonSetResourcesDirectory_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.setWorkbenchDirectory())
            {
                this.initResourcesDirectory();
                DiversityCollection.CollectionSpecimen.CopyWorkbenchDirectory();
            }
        }
        
        #endregion

        #region Distribution map
        
        private void buttonDistributionMapLoad_Click(object sender, EventArgs e)
        {

        }

        private void buttonDistributionMapAddSetting_Click(object sender, EventArgs e)
        {
            //DiversityCollection.DistributionMap.FormMapSettings f = new DistributionMap.FormMapSettings();
            //f.ShowDialog();
            //if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            //{
            //}
        }

        private void buttonDistributionMapSave_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Distribution map setting", "Please enter the title for the setting for the distribution map", "DistributionMap");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {

            }
        }



        #endregion

        #region ScientificTermsRemoteParameter

        //private bool _ScientificTermsRemoteParameterHasChanged = false;

        //public bool ScientificTermsRemoteParameterHasChanged
        //{
        //    get
        //    {
        //        return this._ScientificTermsRemoteParameterHasChanged;
        //    }
        //}

        private void initScientificTermsRemoteParameter()
        {
            //for(int i = 0; i < this.comboBoxScientificTermsRemoteParameter.Items.Count; i++)
            //{
            //    if (DiversityWorkbench.Settings.ScientificTermsRemoteParameter == this.comboBoxScientificTermsRemoteParameter.Items[i].ToString())
            //    {
            //        this.comboBoxScientificTermsRemoteParameter.SelectedIndex = i;
            //        break;
            //    }
            //}
        }
        private void comboBoxScientificTermsRemoteParameter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //DiversityWorkbench.Settings.ScientificTermsRemoteParameter = this.comboBoxScientificTermsRemoteParameter.SelectedItem.ToString();
            //this._ScientificTermsRemoteParameterHasChanged = true;
        }

        #endregion

        #region Tooltip

        private void initTooltipTime()
        {
            this.toolTip.AutoPopDelay = DiversityWorkbench.Settings.ToolTipAutoPopDelay;
            this.numericUpDownToolTipDelay.Value = this.toolTip.InitialDelay / 1000;
            this.numericUpDownToolTipTime.Value = this.toolTip.AutoPopDelay / 1000;
            this.numericUpDownToolTipTime.Minimum = 0;
            this.numericUpDownToolTipTime.Maximum = 6;
            this.setToolTipTimeExplanation();
        }

        private void numericUpDownToolTipDelay_Click(object sender, EventArgs e)
        {
            this.toolTip.InitialDelay = (int)this.numericUpDownToolTipDelay.Value * 1000;
        }

        private void numericUpDownToolTipTime_Click(object sender, EventArgs e)
        {
            switch (this.numericUpDownToolTipTime.Value)
            {
                case 0:
                    this.toolTip.AutoPopDelay = 1;
                    break;
                case 6:
                    this.toolTip.AutoPopDelay = 0;
                    break;
                default:
                    this.toolTip.AutoPopDelay = (int)this.numericUpDownToolTipTime.Value * 1000;
                    break;
            }
            this.setToolTipTimeExplanation();
        }

        private void setToolTipTimeExplanation()
        {
            switch (this.toolTip.AutoPopDelay)
            {
                case 0:
                    this.labelToolTipTimeExplained.Text = "no limit";
                    break;
                case 1:
                    this.labelToolTipTimeExplained.Text = "hide";
                    break;
                case 1000:
                    this.labelToolTipTimeExplained.Text = (this.toolTip.AutoPopDelay / 1000).ToString() + " second";
                    break;
                default:
                    this.labelToolTipTimeExplained.Text = (this.toolTip.AutoPopDelay/1000).ToString() + " seconds";
                    break;
            }
        }

        #endregion

        #region Query

        public bool QueryDefaultsChanged()
        {
            return _QueryDefaultOptimizedChanged != DiversityWorkbench.Settings.QueryOptimizedByDefault || _QueryDefaultRememberChanged != DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault;
        }

        private bool _QueryDefaultOptimizedChanged = false;
        private bool _QueryDefaultRememberChanged = false;

        private void initQuery()
        {
            this.setQueryRememberControl();
            this.setQueryOptimizedControl();
            _QueryDefaultOptimizedChanged = DiversityWorkbench.Settings.QueryOptimizedByDefault;
            _QueryDefaultRememberChanged = DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault;
        }

        private void setQueryOptimizedControl()
        {
            this.checkBoxQueryOptimized.Checked = DiversityWorkbench.Settings.QueryOptimizedByDefault;
            if (DiversityWorkbench.Settings.QueryOptimizedByDefault) this.pictureBoxQueryOptimized.BackColor = System.Drawing.Color.Yellow;
            else this.pictureBoxQueryOptimized.BackColor = System.Drawing.Color.Transparent;
        }

        private void setQueryRememberControl()
        {
            this.checkBoxQueryRemember.Checked = DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault;
            if (DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault) this.pictureBoxQueryRemember.BackColor = System.Drawing.Color.Yellow;
            else this.pictureBoxQueryRemember.BackColor = System.Drawing.Color.Transparent;
        }

        private void checkBoxQueryOptimized_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.QueryOptimizedByDefault = !DiversityWorkbench.Settings.QueryOptimizedByDefault;
            this.setQueryOptimizedControl();
        }

        private void checkBoxQueryRemember_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault = !DiversityWorkbench.Settings.QueryRememberQuerySettingsByDefault;
            this.setQueryRememberControl();
        }

        #endregion

    }
}
