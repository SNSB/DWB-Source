//#define CollectionLocactionIDAvailable

using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace DiversityCollection.Forms
{
    public partial class FormCollection : Form
    {

        #region Parameter

        private DiversityCollection.Collection _Collection;
        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private DiversityCollection.UserControls.iMainForm _iMainForm;
        private System.Windows.Forms.Integration.ElementHost elementHost;

        /// <summary>
        /// the WPF control for the GIS operations
        /// </summary>
        //private WpfSamplingPlotPage.WpfControl _wpfControl;

        private WpfControls.Geometry.UserControlGeometry _UserControlGeometry;

        #endregion

        #region Construction

        public FormCollection(DiversityCollection.UserControls.iMainForm iMainForm)
        {
            try
            {
                InitializeComponent();
                this.InitDesignerReplacements();

                this.splitContainerData.Panel2Collapsed = true;
                this.splitContainerMain.Panel2.Visible = false;
                this._iMainForm = iMainForm;
                this.initForm();
                // online manual
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                this.panelHeader.Visible = true;

                //this.tableLayoutPanelCollection.MinimumSize = new Size(0, 120);
                //this.textBoxDescription.MinimumSize = new Size(0, 22);
                //this.splitContainerCollection.Panel2MinSize = 89;
                this.splitContainerCollection.SplitterDistance = this.splitContainerCollection.Height - DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(150);

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void InitDesignerReplacements()
        {
            try
            {
                // inclusion of Plan - erster Versuch Absturz zu verhindern - kein Effekt
                this.elementHost = new System.Windows.Forms.Integration.ElementHost();
                this.tableLayoutPanelLocationPlan.Controls.Add(this.elementHost, 0, 1);
                this.tableLayoutPanelLocationPlan.SetColumnSpan(this.elementHost, 10);
                this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
                this.elementHost.Location = new System.Drawing.Point(3, 29);
                this.elementHost.Name = "elementHost";
                this.elementHost.Size = new System.Drawing.Size(728, 173);
                this.elementHost.TabIndex = 3;
                this.elementHost.Text = "elementHost1";
                this.elementHost.Child = null;

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public FormCollection(int? ItemID, DiversityCollection.UserControls.iMainForm iMainForm, bool ReadOnly = false) : this(iMainForm)
        {
            if (ItemID != null)
                this._Collection.setItem((int)ItemID);
            this.userControlDialogPanel.Visible = !ReadOnly;
            this.panelHeader.Visible = true;
            if (ReadOnly)
            {
                splitContainerMain.Panel1Collapsed = ReadOnly;
                this.splitContainerDataAndImages.Enabled = !ReadOnly;
                this.buttonCollectionManager.Enabled = !ReadOnly;
                this.buttonCollectionType.Enabled = !ReadOnly;
                this.buttonHeaderShowLabel.Enabled = !ReadOnly;
                this.buttonHistory.Enabled = !ReadOnly;
                this.buttonTableEditor.Enabled = !ReadOnly;
                this.buttonHistory.Enabled = !ReadOnly;
            }
        }

        public FormCollection()
        {
            try
            {
                InitializeComponent();
                this.InitDesignerReplacements();

                this.splitContainerData.Panel2Collapsed = true;
                this.splitContainerMain.Panel2.Visible = false;
                this.initForm();
                // online manual
                this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
                this.panelHeader.Visible = true;
                this.splitContainerCollection.SplitterDistance = this.splitContainerCollection.Height - DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(150);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public FormCollection(int CollectionParentID)
        {
            InitializeComponent();
            //if (this._Collection == null)
            //    this._Collection = new Collection()
            //this._Collection.setItem(CollectionParentID);
            this.initForm();
            this.userControlDialogPanel.Visible = true;
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                //#35
                this.KeyPreview = true;

                System.Data.DataSet Dataset = this.dataSetCollection;
                if (this._Collection == null)
                    this._Collection = new Collection(ref Dataset, this.dataSetCollection.Collection,
                        ref this.treeViewCollection, this, this.userControlQueryList, this.splitContainerMain,
                        this.splitContainerData, this.toolStripButtonSpecimenList, /*this.imageListSpecimenList,*/
                        this.userControlSpecimenList1, this.helpProvider, this.toolTip, ref this.collectionBindingSource);
                //TODO Ariane: for performance reasons we should avoid calling the whole hierarchy on initForm
                // see issue on github #21 and #23 
                this.SettingsInit(); // Auslesen der Ansicht der Hierarchie

                //this.userControlQueryList.RememberSettingIsAvailable(true);
                //this.userControlQueryList.RememberQuerySettingsIdentifier = "Collection";
                //this.userControlQueryList.RememberQueryConditionSettings_ReadFromFile();

                // Aufbau der Hierarchie erst nachdem Werte aus Settings eingelesen wurde
                HierarchicalEntity.HierarchyDisplayState hierarchyDisplayState = this._Collection.GetHierarchyDisplayState();
                // Ariane
                this._Collection.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Hide);
                // this.setHierarchyState(HierarchicalEntity.HierarchyDisplayState.Hide, toolStripMenuItemHierarchyNo, false);

                this._Collection.initForm();
                this._Collection.setToolStripButtonDeleteEvent(this.toolStripButtonDelete);
                this._Collection.setToolStripButtonNewEvent(this.toolStripButtonNew);
                this._Collection.setToolStripButtonCopyHierarchyEvent(this.toolStripButtonCopyHierarchy);
                this._Collection.setToolbarPermission(ref this.toolStripButtonDelete, "Collection", "Delete");
                this._Collection.setToolbarPermission(ref this.toolStripButtonNew, "Collection", "Insert");
                this._Collection.setToolStripButtonSetParentEvent(this.toolStripButtonSetParent);
                this._Collection.setToolbarPermission(ref this.toolStripButtonSetParent, "Collection", "Update");
                this._Collection.setToolStripButtonRemoveParentEvent(this.toolStripButtonRemoveParent);
                this._Collection.setToolbarPermission(ref this.toolStripButtonRemoveParent, "Collection", "Update");
                this._Collection.UserControlImageCollectionImage = this.userControlImageCollectionImage;
                this._Collection.ListBoxImages = this.listBoxCollectionImage;
                this._Collection.ImageListCollectionImages = this.imageListCollectionImages;
                this._Collection.ButtonHeaderShowCollectionImage = this.buttonHeaderShowCollectionImage;
                this._Collection.ButtonHeaderShowCollectionPlan = this.buttonPlan;
                this._Collection.SplitContainerDataAndImages = this.splitContainerDataAndImages;
                this._Collection.SplitContainerImagesAndLabel = this.splitContainerImageAndLabel;
                this._Collection.SplitContainerImagesAndPlan = this.splitContainerImageAndPlan;
                this._Collection.ControlMainData = this.tableLayoutPanelCollection;
                this._Collection.ControlDependentData = this.splitContainerImageAndPlan;
                this._Collection.LabelHeader = this.labelHeader;
                this._Collection.setFormCollection(this);
                //this._Collection.CollectionLocation = this.CollectionLocation;
                this.initRemoteModules();
                this.userControlSpecimenList1.toolStripButtonDelete.Visible = false;
                this.textBoxCollectionID.Visible = true;

                this.userControlSpecimenList1.AllowTransfer = true;
                this.userControlSpecimenList1.toolStripButtonTransfer.Click += this.transferSpecimen;

                this.userControlQueryList.RememberSettingIsAvailable(true);
                this.userControlQueryList.RememberQuerySettingsIdentifier = "Collection";
                this.userControlQueryList.RememberQueryConditionSettings_ReadFromFile();

                // Eintrag der zwischengespeichteren Ansicht der Hierarchie
                // Ariane raus
                //this.setHierarchyState(hierarchyDisplayState, toolStripMenuItemHierarchyWhole, true);
                this._Collection.SetHierarchyDisplayState(hierarchyDisplayState);

                bool OK = this.FormFunctions.getObjectPermissions("CollectionManager", "INSERT");
                this.buttonCollectionManager.Visible = OK;

                this.setLocationcontrols();

                //this.toolStripDropDownButtonHierarchy.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
                //this.toolStripButtonSetParentLocation.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
                //this.toolStripButtonRemoveParentLocation.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
                ////this.toolStripDropDownButtonHierarchy.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
                //this.buttonTransferToLocation.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;

                this.initQueryOptimizingResetEvents();
                this.initCollectionType();
                this.initCollectionImageType();

                //this.setVisibilityOfImagesAndLabel();
                this.setVisibilityOfHeader();

                this.initWPFcontrol();
                this.textBoxLocationPlanWidth.Enabled = false;

                //this.toolStripDropDownButtonHierarchy.Visible = true;
                // #205
                //this.setToolStripsParentLocation(this.toolStripMenuItemHierarchyCollection);

                if (this.userControlQueryList.listBoxQueryResult.Items.Count == 0)
                    this._Collection.setItem(-2);

                this.comboBoxCollectionType.SelectionChangeCommitted += this.comboBoxCollectionType_SelectionChangeCommitted;

                this.toolStripButtonLocationPlanEdit.Enabled = false;

                this.splitContainerCollection.Panel2MinSize = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(142);

                // #220
                this.splitContainerCollectionHierarchy.Panel2Collapsed = !DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
                // #222
                this.buttonTransferToLocation.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
#if CollectionLocactionIDAvailable
#endif

        }

        private void SettingsInit()
        {
            try
            {
                if (DiversityCollection.Forms.FormCollectionSettings.Default.HierarchyDisplayState != null)
                {
                    switch (DiversityCollection.Forms.FormCollectionSettings.Default.HierarchyDisplayState)
                    {
                        case "Show":
                            this._Collection.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Show);
                            this.toolStripDropDownButtonHierarchyType.Image = DiversityCollection.Resource.Hierarchy;
                            this.splitContainerCollection.Panel1Collapsed = false;
                            break;
                        case "Children":
                            this._Collection.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Children);
                            this.toolStripDropDownButtonHierarchyType.Image = DiversityCollection.Resource.HierarchyChildren;
                            this.splitContainerCollection.Panel1Collapsed = false;
                            break;
                        case "Parents":
                            this._Collection.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Parents);
                            this.toolStripDropDownButtonHierarchyType.Image = DiversityCollection.Resource.HierarchySuperior;
                            this.splitContainerCollection.Panel1Collapsed = false;
                            break;
                        case "Hide":
                            this._Collection.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Hide);
                            this.toolStripDropDownButtonHierarchyType.Image = DiversityCollection.Resource.HierarchyNo;
                            this.splitContainerCollection.Panel1Collapsed = true;
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SettingsSave()
        {
            try
            {
                DiversityCollection.Forms.FormCollectionSettings.Default.HierarchyDisplayState = this._Collection.HierarchyStateOfDisplay();
                DiversityCollection.Forms.FormCollectionSettings.Default.Save();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void FormCollection_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this._Collection.saveItem(false); // kein Aufbau der Hierarchie beim Schliessen des Formulars
                this.SettingsSave();
                if (this.userControlQueryList.RememberQuerySettings())
                    this.userControlQueryList.RememberQueryConditionSettings_SaveToFile();
                else
                    this.userControlQueryList.RememberQueryConditionSettings_RemoveFile();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void setHelp(string KeyWord)
        {
            try
            {
                DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void toolStripButtonCopyHierarchy_Click(object sender, EventArgs e)
        {

        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            //DiversityWorkbench.Feedback.SendFeedback(
            //    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
            //    System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
            //    this.userControlQueryList.QueryString(),
            //    this.ID.ToString());
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this.userControlQueryList.QueryString(),
                this.ID.ToString());

        }

        private void FormCollection_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollection.CollectionImage". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionImageTableAdapter.Fill(this.dataSetCollection.CollectionImage);

        }

        #endregion

        #region Interface

        public bool ShowDialogPanel { set { this.userControlDialogPanel.Visible = value; } }
        public int CurrentCollectionID
        {
            set
            {
                int id = value;
                if (this._Collection != null)
                {
                    this._Collection.setItem((int)id);
                }
            }
        }

        #endregion

        #region Query optimizing

        private void initQueryOptimizing()
        {
            DiversityWorkbench.UserControls.UserControlQueryList.QueryMainTable = "Collection";
            DiversityWorkbench.UserControls.UserControlQueryList.ModuleForm_QueryMainTableOptimizing(DiversityWorkbench.Settings.ModuleName, this.Name, "Collection");
        }

        private void initQueryOptimizingResetEvents()
        {
            try
            {
                this.userControlModuleRelatedEntryAdministrativeContactName.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
                this.userControlModuleRelatedEntryCollectionImageCreator.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
                this.userControlModuleRelatedEntryCollectionImageLicenseHolder.buttonOpenModule.Click += new System.EventHandler(this.ResetOptimizing_Click);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ResetOptimizing_Click(object sender, EventArgs e)
        {
            this.initQueryOptimizing();
        }

        #endregion

        #region Modules

        private void initRemoteModules()
        {
            try
            {
                // Agents
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);

                this.userControlModuleRelatedEntryAdministrativeContactName.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryAdministrativeContactName.bindToData("Collection", "AdministrativeContactName", "AdministrativeContactAgentURI", this.collectionBindingSource);

                this.userControlModuleRelatedEntryCollectionImageCreator.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryCollectionImageCreator.bindToData("CollectionImage", "CreatorAgent", "CreatorAgentURI", this.collectionImageBindingSource);

                this.userControlModuleRelatedEntryCollectionImageLicenseHolder.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryCollectionImageLicenseHolder.bindToData("CollectionImage", "LicenseHolder", "LicenseHolderAgentURI", this.collectionImageBindingSource);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Properties

        public int ID { get { if (this.collectionBindingSource.Position > -1) { return int.Parse(this.dataSetCollection.Collection.Rows[this.collectionBindingSource.Position][0].ToString()); } else return -1; } }
        public string DisplayText { get { if (this.collectionBindingSource.Position > -1) { return this.dataSetCollection.Collection.Rows[this.collectionBindingSource.Position][2].ToString(); } else return ""; } }
        public bool ChangeToSpecimen { get { return this.userControlSpecimenList1.ChangeToSpecimen; } }
        public int CollectionSpecimenID { get { return this.userControlSpecimenList1.CollectionSpecimenID; } }

        public System.Windows.Forms.Label LabelHeader { get { return this.labelHeader; } }

        #endregion

        #region Edit specimen list

        private void transferSpecimen(object sender, EventArgs e)
        {
            if (this.userControlSpecimenList1.listBoxSpecimenList.SelectedItems.Count < 1)
            {
                System.Windows.Forms.MessageBox.Show("Please select the specimen you want to transfer to another collection");
                //if (System.Windows.Forms.MessageBox.Show("Transfer all specimen to another collection", "Transfer all", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                //{
                //}
            }
            else
            {
                string SQL = "";
                if (System.Windows.Forms.MessageBox.Show("Do you want to transfer the parts of " + this.userControlSpecimenList1.SelectedSpecimenIDs.Count.ToString() + " specimen from the current collection to another collection", "Transfer", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
                foreach (int i in this.userControlSpecimenList1.SelectedSpecimenIDs)
                {
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += i.ToString();
                }
                DiversityWorkbench.Forms.FormGetItemFromHierarchy F = new DiversityWorkbench.Forms.FormGetItemFromHierarchy(DiversityCollection.LookupTable.DtCollectionWithHierarchy, "CollectionID", "CollectionParentID", "DisplayText", "CollectionID", "Please select the collection to which the specimen should be transfered", "New collection");
                F.ShowDialog();
                if (F.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    int CollectionID;
                    if (!int.TryParse(F.SelectedValue, out CollectionID))
                        return;
                    SQL = "UPDATE P SET CollectionID = " + CollectionID.ToString() +
                        " FROM CollectionSpecimenPart P WHERE P.CollectionID = " + this.ID.ToString() +
                        " AND P.CollectionSpecimenID IN (" + SQL + ")";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                        this._Collection.setItem(this.ID);
                }
            }
        }

        private void treeViewCollection_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void treeViewCollection_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("System.Data.DataRowView"))
            {
                e.Effect = System.Windows.Forms.DragDropEffects.Copy;
            }
        }

        private void treeViewCollection_DragOver(object sender, DragEventArgs e)
        {
            //e.Effect = System.Windows.Forms.DragDropEffects.Copy;
            //int i = this.userControlSpecimenList1.listBoxSpecimenList.IndexFromPoint(this.userControlSpecimenList1.listBoxSpecimenList.PointToClient(new Point(e.X, e.Y)));
            //if (i > -1)
            //{
            //    this.listBoxSynonymyOverview.SelectedIndex = i;
            //    System.Data.DataRow rName = this.dataSetTaxonName.SynonymyOverview.Rows[i];
            //    int Ord = System.Int32.Parse(rName["Ord"].ToString());
            //    string SynType = rName["SynType"].ToString();
            //    // lines and homonyms can not be selected
            //    if (SynType == "isonym" || SynType == "duplicate") return;
            //    //if (rName["DisplayText"].ToString().StartsWith(" ")) CanSelectLine = false;
            //    //bool CanSelectLine = true;
            //}
            //else
            //{
            //    this.listBoxSynonymyOverview.SelectedIndex = -1;
            //}
        }

        #endregion

        #region Images, plan and label

        #region Display

        //private bool _ShowImage = false;
        //private bool _ShowPlan = false;
        //private bool _ShowLabel = false;

        //private bool ShowImage
        //{
        //    get{return this._ShowImage;}
        //    set
        //    {
        //        this._ShowImage = value;
        //        this._ShowPlan = false;
        //        this._ShowLabel = false;
        //    }
        //    //this._ShowImage = DoShow;
        //    //this._ShowImage = false;
        //}

        //private bool ShowPlan
        //{
        //    get { return this._ShowPlan; }
        //    set
        //    {
        //        this._ShowPlan = value;
        //        this._ShowImage = false;
        //        this._ShowLabel = false;
        //    }
        //    //this._ShowImage = DoShow;
        //    //this._ShowImage = false;
        //}

        //private bool ShowLabel
        //{
        //    get{return this._ShowLabel;}
        //    set
        //    {
        //        this._ShowLabel = value;
        //        this._ShowPlan = false;
        //        this._ShowImage = false;
        //    }
        //}

        private void setVisibilityOfHeader()
        {
            this.buttonHeaderShowLabel.BackColor = System.Drawing.SystemColors.Control;
            // Image |       Label
            // Image | Plan
            switch (this._Collection.headerDisplay)
            {
                case Collection.HeaderDisplay.None:
                    this.splitContainerDataAndImages.Panel1Collapsed = true;
                    break;
                case Collection.HeaderDisplay.Image:
                    // hiding label
                    this.splitContainerImageAndLabel.Panel1Collapsed = false;
                    this.splitContainerImageAndLabel.Panel2Collapsed = true;
                    // showing image
                    this.splitContainerImageAndPlan.Panel1Collapsed = false;
                    this.splitContainerImageAndPlan.Panel2Collapsed = true;
                    goto default;
                case Collection.HeaderDisplay.Plan:
                    // hiding label
                    this.splitContainerImageAndLabel.Panel1Collapsed = false;
                    this.splitContainerImageAndLabel.Panel2Collapsed = true;
                    // showing plan
                    this.splitContainerImageAndPlan.Panel1Collapsed = true;
                    this.splitContainerImageAndPlan.Panel2Collapsed = false;
                    // setting minimal height
                    if (this.splitContainerDataAndImages.SplitterDistance < 280)
                    {
                        int Diff = this.splitContainerDataAndImages.SplitterDistance - 280;
                        this.splitContainerDataAndImages.SplitterDistance = 280;
                        this.splitContainerCollection.SplitterDistance = this.splitContainerCollection.Height - DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(152) + Diff;
                    }
                    else
                    {
                        this.splitContainerCollection.SplitterDistance = this.Height - this.splitContainerDataAndImages.SplitterDistance - DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(152);
                    }
                    goto default;
                case Collection.HeaderDisplay.Label:
                    // showing label
                    this.splitContainerImageAndLabel.Panel1Collapsed = true;
                    this.splitContainerImageAndLabel.Panel2Collapsed = false;
                    this.buttonHeaderShowLabel.BackColor = System.Drawing.Color.Red;
                    goto default;
                default:
                    this.splitContainerDataAndImages.Panel1Collapsed = false;
                    break;
            }

            this._Collection.setFormControls();
        }


        //private void setVisibilityOfImagesAndLabel(/*bool ShowImage, bool ShowLabel*/)
        //{
        //    //this.buttonHeaderShowCollectionImage.BackColor = System.Drawing.SystemColors.Control;

        //    // the formatting of the image button is handled in setFormControls() while the button for the label must be handeled here
        //    this.buttonHeaderShowLabel.BackColor = System.Drawing.SystemColors.Control;
        //    if (ShowImage || this.ShowLabel || this.ShowPlan)
        //    {
        //        this.splitContainerDataAndImages.Panel1Collapsed = false;
        //        if (ShowImage || ShowPlan) // either or - do not show both due do limited space
        //        {
        //            this.splitContainerImageAndLabel.Panel1Collapsed = false;
        //            this.splitContainerImageAndLabel.Panel2Collapsed = true;
        //            if (ShowImage)
        //            {
        //                this.splitContainerImageAndPlan.Panel1Collapsed = false;
        //                this.splitContainerImageAndPlan.Panel2Collapsed = true;
        //            }
        //            else
        //            {
        //                this.splitContainerImageAndPlan.Panel1Collapsed = true;
        //                this.splitContainerImageAndPlan.Panel2Collapsed = false;
        //            }
        //        }
        //        else
        //        {
        //            this.splitContainerImageAndLabel.Panel1Collapsed = true;
        //            this.splitContainerImageAndLabel.Panel2Collapsed = false;
        //            this.buttonHeaderShowLabel.BackColor = System.Drawing.Color.Red;
        //        }
        //    }
        //    else
        //    {
        //        this.splitContainerDataAndImages.Panel1Collapsed = true;
        //    }
        //    this._Collection.setFormControls();
        //}

        #endregion

        #region Images

        private void initCollectionImageType()
        {
            try
            {
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxCollectionImageType, "CollCollectionImageType_Enum", DiversityWorkbench.Settings.Connection);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonHeaderShowCollectionImage_Click(object sender, EventArgs e)
        {
            try
            {
                this._Collection.headerDisplay = Collection.HeaderDisplay.Image;
                this.setVisibilityOfHeader();

                //this.ShowImage = !this._ShowImage;
                //this.setVisibilityOfImagesAndLabel();

                //if (this.buttonHeaderShowCollectionImage.BackColor == System.Drawing.Color.Red)
                //{
                //    this.setVisibilityOfImagesAndLabel(false, false);
                //}
                //else
                //{
                //    this.setVisibilityOfImagesAndLabel(true, false);
                //}
            }
            catch { }
        }

        private void listBoxCollectionImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageListCollectionImages.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        private void listBoxCollectionImage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListCollectionImages.ImageSize.Height;
            e.ItemWidth = this.imageListCollectionImages.ImageSize.Width;
        }

        private void listBoxCollectionImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxCollectionImage.SelectedIndex;
                for (int p = 0; p <= i; p++)
                {
                    if (this.dataSetCollection.CollectionImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this.dataSetCollection.CollectionImage.Rows.Count > i)
                {
                    this.tableLayoutPanelImageTitle.Enabled = true;
                    this.toolStripButtonImageDelete.Enabled = true;
                    this.toolStripButtonImageDescription.Enabled = true;
                    System.Data.DataRow r = this.dataSetCollection.CollectionImage.Rows[i];
                    this.userControlImageCollectionImage.ImagePath = r["URI"].ToString();
                    this.collectionImageBindingSource.Position = i;
                    this.setImageDescription(this.collectionImageBindingSource);
                    this.setRecordingDateFormat();
                    this.setOldPlanControl(r);
                }
                else
                {
                    this.tableLayoutPanelImageTitle.Enabled = false;
                    this.toolStripButtonImageDelete.Enabled = false;
                    this.toolStripButtonImageDescription.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage();
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        DiversityCollection.Datasets.DataSetCollection.CollectionImageRow R = this.dataSetCollection.CollectionImage.NewCollectionImageRow();
                        R.CollectionID = this.ID;
                        R.URI = f.URIImage;
                        R.Description = f.Exif;
                        this.dataSetCollection.CollectionImage.Rows.Add(R);
                        this._Collection.setFormControls();
                        //this.FormFunctions.FillImageList(this.listBoxCollectionImage, this.imageListCollectionImages, this.imageListForm, this.dataSetCollection.CollectionImage, "URI", this.userControlImageCollectionImage);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonImageDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.collectionImageBindingSource.Current != null)
                {
                    System.Data.DataRowView dataRowView = (System.Data.DataRowView)this.collectionImageBindingSource.Current;
                    string SQL = "DELETE C FROM CollectionImage C WHERE C.CollectionID = " + this._Collection.ID.ToString() + " AND URI = '" + dataRowView["URI"].ToString() + "'";
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    {
                        //dataRowView.Delete();
                        // Remove the row from the DataTable without marking it as Deleted since this will lead to Concurrency errors if adapter.update is called afterwards
                        this.dataSetCollection.CollectionImage.Rows.Remove(dataRowView.Row);
                        this._Collection.setFormControls();
                        if (this.listBoxCollectionImage.Items.Count > 0) this.listBoxCollectionImage.SelectedIndex = 0;
                        else
                        {
                            this.listBoxCollectionImage.SelectedIndex = -1;
                            this.userControlImageCollectionImage.ImagePath = "";
                        }
                        return;
                    }
                    //this._Collection.setFormControls();
                }



                string URL = this.userControlImageCollectionImage.ImagePath;
                if (URL.Length > 0)
                {
                    System.Data.DataRow[] rr = this.dataSetCollection.CollectionImage.Select("URI = '" + URL + "'");
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow r = rr[0];
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            r.Delete();
                            this._Collection.setFormControls();
                            //this.FormFunctions.FillImageList(this.listBoxCollectionImage, this.imageListCollectionImages, this.imageListForm, this.dataSetCollection.CollectionImage, "URI", this.userControlImageCollectionImage);
                            if (this.listBoxCollectionImage.Items.Count > 0) this.listBoxCollectionImage.SelectedIndex = 0;
                            else
                            {
                                this.listBoxCollectionImage.SelectedIndex = -1;
                                this.userControlImageCollectionImage.ImagePath = "";
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

        private void comboBoxCollectionImageType_DropDown(object sender, EventArgs e)
        {

        }

        private void comboBoxWithholdingReason_DropDown(object sender, EventArgs e)
        {

        }

        private void toolStripButtonImageDescription_Click(object sender, EventArgs e)
        {
            this.setImageDescription(this.collectionImageBindingSource);
        }

        private string ImageDescriptionTemplate
        {
            get
            {
                string Template = "";
                string SQL = "SELECT P.ImageDescriptionTemplate " +
                    "FROM UserProxy U INNER JOIN " +
                    "ProjectProxy P ON U.CurrentProjectID = P.ProjectID " +
                    "WHERE (U.LoginName = User_name())";
                Template = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (this.userControlQueryList.ProjectID > 0 && Template.Length == 0)
                    System.Windows.Forms.MessageBox.Show("For the current project no description template is defined");
                return Template;
            }
        }

        private void setImageDescription(System.Windows.Forms.BindingSource BindingSource)
        {
            if (BindingSource.Current == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select an image");
                return;
            }
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)BindingSource.Current;
                string XML = R["Description"].ToString();

                //DiversityWorkbench.Forms.FormXml f = new DiversityWorkbench.Forms.FormXml("EXIF for image", XML, true);
                //f.ShowDialog();

                this.userControlXMLTreeCollectionImageExif.setToDisplayOnly();
                this.userControlXMLTreeCollectionImageExif.XML = XML;


                // Rotate if EXIF File contains info about orientiation of image
                try
                {
                    if (this.userControlImageCollectionImage.AutorotationEnabled && this.userControlImageCollectionImage.Autorotate)
                    {
                        System.Drawing.RotateFlipType Rotate = DiversityWorkbench.Forms.FormFunctions.ExifRotationInfo(XML);
                        if (Rotate != RotateFlipType.RotateNoneFlipNone)
                            this.userControlImageCollectionImage.RotateImage(Rotate);
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }

                //System.Data.DataRowView R = (System.Data.DataRowView)BindingSource.Current;
                //string XML = R["Description"].ToString();
                //if (XML.Length == 0)
                //{
                //    if (this.userControlQueryList.ProjectID == 0 || this.userControlQueryList.ProjectID == -1)
                //    {
                //        System.Windows.Forms.MessageBox.Show("Please select a project");
                //        return;
                //    }
                //    else
                //        XML = this.ImageDescriptionTemplate;
                //}
                //if (XML.Length > 0)
                //{
                //    DiversityWorkbench.Forms.FormEditXmlContent f = new DiversityWorkbench.Forms.FormEditXmlContent(XML, this.ImageDescriptionTemplate, "Edit the image description", "Image description");
                //    f.StartPosition = FormStartPosition.CenterParent;
                //    f.ShowDialog();
                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        R["Description"] = f.XmlWithoutEncoding;
                //    }
                //}
                //else
                //    System.Windows.Forms.MessageBox.Show("No template or description could be found");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void tableLayoutPanelImage_Enter(object sender, EventArgs e)
        {
            if (this.listBoxCollectionImage.Items.Count > 0)
            {
                this.tableLayoutPanelImageTitle.Enabled = true;
                this.toolStripButtonImageDescription.Enabled = true;
                this.toolStripButtonImageDelete.Enabled = true;
            }
            else
            {
                this.tableLayoutPanelImageTitle.Enabled = false;
                this.toolStripButtonImageDescription.Enabled = false;
                this.toolStripButtonImageDelete.Enabled = false;
            }
        }

        #region OldPlan

        private void setOldPlanControl(System.Data.DataRow dataRow)
        {
            string SQL = "SELECT TOP (1) ImageType FROM CollectionImage WHERE (CollectionID = " + dataRow["CollectionID"].ToString() + ") AND (URI = '" + dataRow["URI"].ToString() + "')";
            string Type = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);// dataRow["ImageType"].ToString().ToLower();
            this.buttonCollectionImageOldPlan.Enabled = Type == "plan";
            this.comboBoxCollectionImageType.Enabled = Type != "plan";
        }

        private void buttonCollectionImageOldPlan_Click(object sender, EventArgs e)
        {
            if (this.collectionBindingSource.Current != null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                try
                {
                    System.Windows.Forms.Form form = new Form();
                    form.Height = this.Height - 10;
                    form.Width = this.Width - 10;
                    form.StartPosition = FormStartPosition.CenterParent;
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionBindingSource.Current;
                    form.Text = R["CollectionName"].ToString();
                    IntPtr Hicon = DiversityCollection.Resource.Plan.GetHicon();
                    form.Icon = Icon.FromHandle(Hicon);
                    DiversityCollection.Tasks.UserControlPlan userControlPlan = new Tasks.UserControlPlan();
                    userControlPlan.setEditState(WpfControls.Geometry.UserControlGeometry.State.ReadOnly);
                    userControlPlan.SetCollectionID((int)this._Collection.ID);
                    form.Controls.Add(userControlPlan);
                    userControlPlan.Dock = DockStyle.Fill;
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    form.ShowDialog();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    System.Windows.Forms.MessageBox.Show("Available in upcoming version");
                }
                finally
                {
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        #endregion

        #endregion

        #region Plan

        private void buttonPlan_Click(object sender, EventArgs e)
        {
            try
            {
                this._Collection.headerDisplay = Collection.HeaderDisplay.Plan;
                this.setVisibilityOfHeader();
                //this.ShowPlan = !this._ShowPlan;
                //this.setVisibilityOfImagesAndLabel();
            }
            catch { }
        }

        private void initWPFcontrol()
        {
            try
            {
                _UserControlGeometry = new WpfControls.Geometry.UserControlGeometry();
                if (_UserControlGeometry != null && elementHost != null)
                {
                    elementHost.Child = _UserControlGeometry;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            //// Create WPF control instance
            //_wpfControl = new WpfSamplingPlotPage.WpfControl(DiversityWorkbench.Settings.Language);
            //this._wpfControl.WpfShowRefMapDelButton(false);
            //// hide the delete button of the backgroud map
            ////_wpfControl.WpfShowRefMapDelButton(false);
            //// Add to our Form
            //elementHost.Child = _wpfControl;

            //this._wpfControl.WpfShowRefMapDelButton(true);
            //elementHost.Child = null;
            //// Add canvas again to wpfControl
            //_wpfControl.WpfSetCanvas();
            //// Add wpfControl to ctrlHost
            //elementHost.Child = _wpfControl;

        }

        public void setPlanAccessibility(bool Accessible)
        {
            if (this._UserControlGeometry != null)
            {
                WpfControls.Geometry.UserControlGeometry.State state = Accessible ? WpfControls.Geometry.UserControlGeometry.State.Edit : WpfControls.Geometry.UserControlGeometry.State.ReadOnly;
                this._UserControlGeometry.setState(state);
                this.toolStripButtonLocationPlanEdit.Enabled = Accessible;
                this.toolStripButtonLocationPlanSave.Enabled = Accessible;
                this.toolStripButtonLocationPlanOpen.Enabled = Accessible;
                this.textBoxLocationPlanWidth.Enabled = Accessible;
                this.textBoxLocationHeight.Enabled = Accessible;
                this.dateTimePickerLocationPlanDate.Enabled = Accessible;
                this.buttonLocationPlanDate.Enabled = Accessible;
            }
        }

        private void toolStripButtonLocationPlanOpen_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                string LocationPlan = f.ImagePath;
                DiversityWorkbench.Forms.FormFunctions.Medium MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(LocationPlan);
                if (MediumType != DiversityWorkbench.Forms.FormFunctions.Medium.Image)
                {
                    LocationPlan = "";
                    System.Windows.Forms.MessageBox.Show("Please select an image file");
                }
                if (this.textBoxLocationPlan.Text != LocationPlan && LocationPlan.Length > 0 && this.textBoxLocationPlan.Text.Length > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to save the old plan as an image?", "Save old plan?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!this.SavePlanAsImage())
                            System.Windows.Forms.MessageBox.Show("Could not save plan as image");
                        //string SQL = "INSERT INTO CollectionImage (CollectionID, URI, ImageType, InternalNotes, RecordingDate, LocationGeometry) " +
                        //    "SELECT CollectionID, LocationPlan, 'plan', 'old plan from ' + CONVERT(varchar(20), GETDATE(), 120), GETDATE(), LocationGeometry " +
                        //    "FROM dbo.[CollectionHierarchySuperior](" + this._Collection.ID.ToString() + ") C WHERE C.CollectionID = " + this._Collection.ID.ToString();
                        //DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                }
                this.textBoxLocationPlan.Text = LocationPlan;
                if (LocationPlan.Length > 0)
                    this.SavePlan();
                this.toolStripButtonLocationPlanEdit.Enabled = false;
                this.toolStripButtonLocationPlanSave.BackColor = System.Drawing.Color.Red;
            }
        }

        private void toolStripButtonLocationPlanSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool PlanAvailable = this.textBoxLocationPlan.Text.Length > 0;
                bool GeometryAvailable = this._UserControlGeometry.GetRectangle().ToString().Length > 0;
                if (PlanAvailable)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionBindingSource.Current;
                    R.BeginEdit();
                    R["LocationPlan"] = this.textBoxLocationPlan.Text;
                    R.EndEdit();
                    this._UserControlGeometry.SetImage(this.textBoxLocationPlan.Text);
                    this.SavePlan();
                    this.SaveGeometry();
                    //#if DEBUG
                    //                    this.SavePlanPolygon();
                    //#else
                    //                    this.SavePlanRectangle();
                    //#endif
                    this.SavePlanScale();
                    //this.SetLocationPlan(this.textBoxLocationPlan.Text);
                    this.SetLocationGeometry((int)this._Collection.ID, this.textBoxLocationPlan.Text);
                    this.SetLocationScale((int)this._Collection.ID);
                }
                else if (GeometryAvailable)
                {
                    this.SaveGeometry();
                    //#if DEBUG
                    //                    this.SavePlanPolygon();
                    //#else
                    //                    this.SavePlanRectangle();
                    //#endif
                }
                else
                {
                    this.ResetLocationPlan();
                }
                this.toolStripButtonLocationPlanSave.BackColor = System.Drawing.SystemColors.Control;

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool SavePlan()
        {
            bool OK = true;
            if (this.textBoxLocationPlan.Text.Length > 0)
            {
                string SQL = "UPDATE C SET LocationPlan = '" + this.textBoxLocationPlan.Text + "' FROM Collection C WHERE C.CollectionID = " + this.ID.ToString();
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            return OK;
        }

        private bool SavePlanScale()
        {
            bool OK = true;
            if (this._UserControlGeometry.ScaleLineVisible())
            {
                System.Windows.Media.LineGeometry scaleLine = this._UserControlGeometry.GetScaleLine();
                if (scaleLine.EndPoint.X > scaleLine.StartPoint.X)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionBindingSource.Current;
                    if (!R["LocationPlanWidth"].Equals(System.DBNull.Value))
                    {
                        if (System.Windows.Forms.MessageBox.Show("Do you want to change the scale for the current plan?", "Change scale?", MessageBoxButtons.YesNo) == DialogResult.No)
                            return OK;
                    }
                    DiversityWorkbench.Forms.FormGetNumber f = new DiversityWorkbench.Forms.FormGetNumber(null, "Scale", "Please enter the value in meters that correspond to the width of the scale"); // e.g. the floor plan");
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.Number != null)
                    {
                        double Scale = (double)f.Number;
                        if (this._UserControlGeometry.ImageWidth() != null)
                        {
                            double widthScale = scaleLine.EndPoint.X - scaleLine.StartPoint.X;
                            double widthImage = (double)this._UserControlGeometry.ImageWidth();
                            double widthMeter = widthScale / (double)Scale;
                            double widthImageInMeter = widthImage / widthMeter;
                            string SQL = "UPDATE C SET LocationPlanWidth = " + widthImageInMeter + " FROM Collection C WHERE C.CollectionID = " + this.ID.ToString();
                            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                            R.BeginEdit();
                            R["LocationPlanWidth"] = widthImageInMeter;
                            R.EndEdit();
                        }
                    }
                }
            }
            return OK;
        }

        private void toolStripButtonLocationPlanRemove_Click(object sender, EventArgs e)
        {
            this.ResetLocationPlan();
        }

        private void ResetLocationPlan()
        {
            string SQL = "UPDATE C SET LocationPlanWidth = NULL, LocationPlan = NULL, LocationGeometry = NULL, LocationPlanDate = NULL, LocationHeight = NULL " +
                "FROM Collection C WHERE C.CollectionID = " + this.ID.ToString();
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            this.SetLocationPlan("");
            this.textBoxLocationPlan.Text = "";
        }

        private void textBoxLocationPlan_TextChanged(object sender, EventArgs e)
        {
            //this.textBoxLocationPlanWidth.Enabled = this.textBoxLocationPlan.Text.Length > 0;
            this.SetLocationGeometry((int)this.ID, this.textBoxLocationPlan.Text);
            //this.SetLocationPlan(this.textBoxLocationPlan.Text);
            try
            {
                if (this.textBoxLocationPlan.Text.Length > 0 && !this.textBoxLocationPlan.Text.StartsWith("http"))
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.textBoxLocationPlan.Text);
                    if (fileInfo.Exists)
                        this.toolTip.SetToolTip(this.textBoxLocationPlan, fileInfo.Name);
                    else
                        this.toolTip.SetToolTip(this.textBoxLocationPlan, "File inaccessible or does not exist");
                }
                else
                    this.toolTip.SetToolTip(this.textBoxLocationPlan, "");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.setLocationPlanDateFormat();
            this._UserControlGeometry.EditScaleLine = this.textBoxLocationPlan.Text.Length > 0;
            this._UserControlGeometry.ScaleCanEdit(this.textBoxLocationPlan.Text.Length > 0);
        }

        private void SetLocationPlan(string FilePath)
        {
            this._UserControlGeometry.SetImage(FilePath);
        }

        public void SetLocationGeometry(int CollectionID, string LocationPlan = "")
        {
            try
            {
                if (this._Collection.headerDisplay == Collection.HeaderDisplay.Plan)
                {
                    // Check type
                    DiversityWorkbench.Forms.FormFunctions.Medium MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(LocationPlan);
                    if (MediumType != DiversityWorkbench.Forms.FormFunctions.Medium.Image)
                        LocationPlan = "";

                    if (this._UserControlGeometry == null)
                        this.initWPFcontrol();

                    // Plan
                    if (LocationPlan.Length == 0)
                    {
                        string SQL = "SELECT LocationPlan FROM dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") WHERE CollectionID = " + CollectionID.ToString();
                        LocationPlan = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    }

                    MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(LocationPlan);
                    if (MediumType != DiversityWorkbench.Forms.FormFunctions.Medium.Image)
                        LocationPlan = "";
                    //if (Plan.Length > 0)
                    this.SetLocationPlan(LocationPlan);

                    // Geometry
                    string Sql = "SELECT [LocationGeometry].ToString()  FROM [dbo].[Collection]  WHERE CollectionID = " + CollectionID.ToString();
                    string Geometry = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Sql);
                    string ParentGeometry = "";
                    if (Geometry.Length == 0)
                    {
                        Sql = "SELECT [LocationGeometry].ToString() FROM dbo.CollectionLocationSuperior(" + CollectionID.ToString() + ") WHERE CollectionID = " + CollectionID.ToString();
                    }
                    else
                    {
                        Sql = "SELECT P.[LocationGeometry].ToString(), * FROM [dbo].[Collection] C " +
                            "INNER JOIN dbo.CollectionLocationSuperior(" + CollectionID.ToString() + ") P ON case when C.LocationParentID is null then C.CollectionParentID else C.LocationParentID end  = P.CollectionID AND C.CollectionID = " + CollectionID.ToString();
                        //Sql = "SELECT P.[LocationGeometry].ToString() FROM [dbo].[Collection] C INNER JOIN dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") P ON C.CollectionParentID = P.CollectionID AND C.CollectionID = " + CollectionID.ToString();
                    }
                    ParentGeometry = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(Sql, true);
                    this._UserControlGeometry.SetRectangleAndPolygonGeometry(Geometry, ParentGeometry);
                    this.toolStripButtonLocationPlanEdit.Enabled = Geometry.Length > 0;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void SetLocationScale(int CollectionID)
        {
            try
            {
                if (this._UserControlGeometry == null)
                    this.initWPFcontrol();

                // Scale
                string SQL = "SELECT LocationPlanWidth FROM dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") WHERE CollectionID = " + CollectionID.ToString();
                string Scale = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                if (Scale.Length == 0)
                {
                    SQL = "SELECT LocationPlanWidth FROM dbo.CollectionHierarchySuperior(" + CollectionID.ToString() + ") WHERE CollectionID = " + CollectionID.ToString();
                    Scale = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                }
                else { }
                this._UserControlGeometry.SetScaleLine(Scale);
                this._UserControlGeometry.EditScaleLine = false;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string GetPlanRectangle()
        {
            string Rect = "";
            System.Windows.Media.RectangleGeometry rectangle = this._UserControlGeometry.GetRectangle();
            return Rect;
        }

        private bool SaveGeometry()
        {
            bool OK = true;
            if (this._UserControlGeometry.HasPolygon())
                OK = this.SavePlanPolygon();
            else
                OK = this.SavePlanRectangle();
            this.toolStripButtonLocationPlanEdit.Enabled = OK;
            return OK;
        }

        private bool SavePlanRectangle()
        {
            bool OK = true;
            System.Windows.Media.RectangleGeometry rectangle = this._UserControlGeometry.GetRectangle();
            bool HasArea = false;
            if (rectangle.Bounds.Height > 0 || rectangle.Bounds.Width > 0)
                HasArea = true;
            string SQL = "";
            if (HasArea)
            {
                SQL = "UPDATE C SET LocationGeometry = geometry::STGeomFromText('POLYGON ((";
                SQL += rectangle.Bounds.BottomLeft.X.ToString() + " " + rectangle.Bounds.BottomLeft.Y.ToString() + ", " +
                    rectangle.Bounds.TopLeft.X.ToString() + " " + rectangle.Bounds.TopLeft.Y.ToString() + ", " +
                    rectangle.Bounds.TopRight.X.ToString() + " " + rectangle.Bounds.TopRight.Y.ToString() + ", " +
                    rectangle.Bounds.BottomRight.X.ToString() + " " + rectangle.Bounds.BottomRight.Y.ToString() + ", " +
                    rectangle.Bounds.BottomLeft.X.ToString() + " " + rectangle.Bounds.BottomLeft.Y.ToString();
                SQL += "))', 0) FROM Collection C WHERE C.CollectionID = " + this.ID.ToString();
            }
            else
            {
                SQL = "UPDATE C SET LocationGeometry = NULL FROM Collection C WHERE NOT C.LocationGeometry IS NULL AND C.CollectionID = " + this.ID.ToString();
            }
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            return OK;
            //0 0, 150 0, 150 150, 0 150, 0 0
        }


        private bool SavePlanPolygon()
        {
            bool OK = false;
            System.Windows.Shapes.Polygon polygon = this._UserControlGeometry.GetPolygon();
            bool HasArea = false;
            if (polygon.Points.Count > 3)
                HasArea = true;
            string SQL = "";
            if (HasArea)
            {
                foreach (System.Windows.Point P in polygon.Points)
                {
                    if (SQL.Length > 0)
                        SQL += ", ";
                    SQL += P.X.ToString() + " " + P.Y.ToString();
                }
                SQL = "UPDATE C SET LocationGeometry = geometry::STGeomFromText('POLYGON ((" + SQL + "))', 0) FROM Collection C WHERE C.CollectionID = " + this.ID.ToString();
            }
            else
            {
                SQL = "UPDATE C SET LocationGeometry = NULL FROM Collection C WHERE NOT C.LocationGeometry IS NULL AND C.CollectionID = " + this.ID.ToString();
            }
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            return OK;
            //0 0, 150 0, 150 150, 0 150, 0 0
        }

        private void toolStripButtonLocationPlanEdit_Click(object sender, EventArgs e)
        {
            this.SetLocationGeometry((int)this._Collection.ID);
        }

        private void toolStripButtonLocationPlanSaveAsImage_Click(object sender, EventArgs e)
        {
            if (!this.SavePlanAsImage())
                System.Windows.Forms.MessageBox.Show("Could not save plan as image");
        }

        private bool SavePlanAsImage()
        {
            string SQL = "INSERT INTO CollectionImage (CollectionID, URI, ImageType, InternalNotes, RecordingDate, LocationGeometry) " +
                "SELECT CollectionID, LocationPlan, 'plan', 'old plan from ' + CONVERT(varchar(20), GETDATE(), 120), GETDATE(), LocationGeometry " +
                "FROM dbo.[CollectionHierarchySuperior](" + this._Collection.ID.ToString() + ") C WHERE C.CollectionID = " + this._Collection.ID.ToString();
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
            {
                System.Windows.Forms.MessageBox.Show("Plan saved as image");
                this._Collection.setItem((int)this._Collection.ID);
                return true;
            }
            return false;
        }

        private void setLocationPlanDateFormat()
        {
            string CustomFormat = "-";
            if (this.collectionBindingSource != null && this.collectionBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionBindingSource.Current;
                if (!R["LocationPlanDate"].Equals(System.DBNull.Value))
                    CustomFormat = "yyyy-MM-dd";
            }
            this.dateTimePickerLocationPlanDate.CustomFormat = CustomFormat;
        }

        private void dateTimePickerLocationPlanDate_CloseUp(object sender, EventArgs e)
        {
            try
            {
                if (this.collectionBindingSource.Current != null)
                {
                    string SQL = "UPDATE C SET LocationPlanDate = CONVERT(DATETIME, '"
                        + this.dateTimePickerLocationPlanDate.Value.Year.ToString() + "-"
                        + this.dateTimePickerLocationPlanDate.Value.Month.ToString() + "-"
                        + this.dateTimePickerLocationPlanDate.Value.Day.ToString() + " 00:00:00', 102) FROM Collection C WHERE C.CollectionID = " + this._Collection.ID.ToString();
                    if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                        this._Collection.setItem((int)this._Collection.ID);
                    //System.Data.DataRowView R = (System.Data.DataRowView)this.collectionBindingSource.Current;
                    //R.BeginEdit();
                    //R["LocationPlanDate"] = this.dateTimePickerLocationPlanDate.Value;
                    //R.EndEdit();
                    //this._Collection.saveItem(false);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.setLocationPlanDateFormat();
        }

        private void buttonLocationPlanDate_Click(object sender, EventArgs e)
        {
            if (this.collectionBindingSource != null)
            {
                string SQL = "UPDATE C SET LocationPlanDate = NULL FROM Collection C WHERE C.CollectionID = " + this._Collection.ID.ToString();
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    this._Collection.setItem((int)this._Collection.ID);
                //System.Data.DataRowView R = (System.Data.DataRowView)this.collectionBindingSource.Current;
                //R["LocationPlanDate"] = System.DBNull.Value;
                this.setLocationPlanDateFormat();
            }
        }

        #endregion

        #region Label

        private void buttonHeaderShowLabel_Click(object sender, EventArgs e)
        {
            try
            {
                this._Collection.headerDisplay = Collection.HeaderDisplay.Label;
                this.setVisibilityOfHeader();
                //this.ShowLabel = !this._ShowLabel;
                //this.setVisibilityOfImagesAndLabel();
            }
            catch { }
        }

        #region Printing and DisplayOrder

        #region Toolstrips

        #region Preview

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserLabel.ShowPrintPreviewDialog();
        }

        private void toolStripButtonLabelExport_Click(object sender, EventArgs e)
        {
            //this.saveFileDialog.RestoreDirectory = true;
            //this.saveFileDialog.Filter = "html files (*.htm)|*.htm";
            //this.saveFileDialog.AddExtension = true;
            //this.saveFileDialog.DefaultExt = "htm";
            //this.saveFileDialog.FileName = "Label.htm";
            //if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    string Text = this.webxBrowserLabel.DocumentText;
            //    System.IO.StreamWriter w = new System.IO.StreamWriter(this.saveFileDialog.FileName, false, System.Text.Encoding.UTF8);
            //    w.Write(Text);
            //    w.Close();
            //}
        }

        private void toolStripButtonOpenSchemaFile_Click(object sender, EventArgs e)
        {
            string Path = Folder.LabelPrinting(Folder.LabelPrintingFolder.CollectionSchemas);
            if (this.textBoxSchemaFile.Text.Length > 0)
            {
                try
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxSchemaFile.Text);
                    if (FI.Exists)
                        Path = FI.DirectoryName;
                }
                catch { }
            }
            this.openFileDialogLabelSchema = new OpenFileDialog();
            this.openFileDialogLabelSchema.RestoreDirectory = true;
            this.openFileDialogLabelSchema.Multiselect = false;
            this.openFileDialogLabelSchema.InitialDirectory = Path;
            this.openFileDialogLabelSchema.Filter = "XSLT Files|*.xslt";
            try
            {
                this.openFileDialogLabelSchema.ShowDialog();
                if (this.openFileDialogLabelSchema.FileName.Length > 0)
                {
                    this.textBoxSchemaFile.Tag = this.openFileDialogLabelSchema.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogLabelSchema.FileName);
                    this.textBoxSchemaFile.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonLabelPreview_Click(object sender, EventArgs e)
        {
            this.CreateLabel();
            //int ID = (int)this._Collection.ID;
            //string SQL = "SELECT * FROM Collection WHERE CollectionID = " + ID.ToString();
            //DiversityCollection.Datasets.DataSetCollection dsColl = new Datasets.DataSetCollection();
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //ad.Fill(dsColl.Collection);
            //string File = this.CreateXmlForCollection(dsColl);
            //if (File.Length > 0)
            //{
            //    try
            //    {
            //        System.Uri URI = new Uri(File);
            //        this.web BrowserLabel.Url = URI;
            //    }
            //    catch (Exception ex)
            //    {
            //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //    }
            //}
        }

        private void toolStripButtonLabelMulti_Click(object sender, EventArgs e)
        {
            this.CreateLabel(true);
            //string SQL = "";
            //foreach (int i in this.userControlQueryList.ListOfIDs)
            //{
            //    if (SQL.Length > 0) SQL += ", ";
            //    SQL += i.ToString();
            //}
            //SQL = "SELECT * FROM Collection WHERE CollectionID IN ( " + SQL + ")";
            //DiversityCollection.Datasets.DataSetCollection dsColl = new Datasets.DataSetCollection();
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //ad.Fill(dsColl.Collection);
            //int ProjectID = -1;
            //if (this.comboBoxLabelProject.SelectedValue != null)
            //    ProjectID = int.Parse(this.comboBoxLabelProject.SelectedValue.ToString());

            //string File = this.CreateXmlForCollection(dsColl);
            //if (File.Length > 0)
            //{
            //    try
            //    {
            //        System.Uri URI = new Uri(File);
            //        this.webxBrowserLabel.Url = URI;
            //    }
            //    catch (Exception ex)
            //    {
            //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //    }
            //}
        }

        private void toolStripButtonLabelSplit_Click(object sender, EventArgs e)
        {
            this.CreateLabel(false, true);
        }

        #endregion

        #endregion

        #endregion

        #region QR code

        private DiversityCollection.XmlExport.QRcodeSourceCollection QRsource()
        {
            DiversityCollection.XmlExport.QRcodeSourceCollection _QRSource = XmlExport.QRcodeSourceCollection.CollectionName;
            if (this.comboBoxLabelQRcode.SelectedItem != null)
            {
                switch (this.comboBoxLabelQRcode.SelectedItem.ToString())
                {
                }
            }
            return _QRSource;
        }

        private bool _PrintQRcode = false;

        private void buttonLabelQRcode_Click(object sender, EventArgs e)
        {
            this.SetQrCodeControls();
        }

        private void checkBoxLabelQRcode_Click(object sender, EventArgs e)
        {
            this.SetQrCodeControls();
        }

        private void SetQrCodeControls()
        {
            this._PrintQRcode = !this._PrintQRcode;
            this.checkBoxLabelQRcode.Checked = this._PrintQRcode;
            this.comboBoxLabelQRcode.Enabled = this._PrintQRcode;
            this.comboBoxLabelQRcodeType.Enabled = this._PrintQRcode;
            this.pictureBoxLabelQRcodeSource.Enabled = this._PrintQRcode;

            this.buttonLabelQRcode.Image = DiversityCollection.Resource.QRcode;

        }

        private bool StableIdentifierSettingsComplete()
        {
            bool SpecificationsComplete = true;
            string SQL = "SELECT dbo.StableIdentifierBase() + 'Collection/'";
            string Base = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Base.Length == 0)
            {
                return false;
            }
            return true;

            if (this.comboBoxLabelProject.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                SpecificationsComplete = false;
            }
            else
            {
                SQL = "SELECT Project, StableIdentifierBase, StableIdentifierTypeID " +
                    "FROM ProjectProxy AS p " +
                    "WHERE ProjectID = " + this.comboBoxLabelProject.SelectedValue.ToString(); ;
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    if (dt.Rows[0]["StableIdentifierBase"].Equals(System.DBNull.Value)
                        || dt.Rows[0]["StableIdentifierBase"].ToString().Length == 0
                        || dt.Rows[0]["StableIdentifierTypeID"].Equals(System.DBNull.Value))
                    {
                        string Message = "Missing specifications:\r\n";
                        if (dt.Rows[0]["StableIdentifierBase"].Equals(System.DBNull.Value) || dt.Rows[0]["StableIdentifierBase"].ToString().Length == 0)
                            Message += "No base for the stable identifier specified\r\n";
                        if (dt.Rows[0]["StableIdentifierTypeID"].Equals(System.DBNull.Value))
                            Message += "No type for the stable identifier selected\r\n";
                        Message += "Do you want to insert the missing specifications?";
                        if (System.Windows.Forms.MessageBox.Show(Message, "Missing specifications", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                        {
                            DiversityWorkbench.Forms.FormStableIdentifier f = new DiversityWorkbench.Forms.FormStableIdentifier(this.userControlQueryList.ProjectID);//dtProject);
                            f.setHelp("Stable identifier");
                            f.ShowDialog();
                            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                dt.Clear();
                                ad.Fill(dt);
                                if (dt.Rows.Count == 0
                                    || dt.Rows[0]["StableIdentifierBase"].Equals(System.DBNull.Value)
                                    || dt.Rows[0]["StableIdentifierBase"].ToString().Length == 0
                                    || dt.Rows[0]["StableIdentifierTypeID"].Equals(System.DBNull.Value))
                                    SpecificationsComplete = false;
                            }
                            else
                                SpecificationsComplete = false;
                        }
                        else
                            SpecificationsComplete = false;
                    }
                }
                else
                {
                    SpecificationsComplete = false;
                }
            }
            if (!SpecificationsComplete)
            {
                this.buttonLabelQRcode.Image = DiversityCollection.Resource.QRcodeGray;
                this.comboBoxLabelQRcode.Items.Clear();
            }
            return SpecificationsComplete;
        }

        private void comboBoxLabelQRcode_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxLabelQRcode.Items.Count == 0)
            {
                this.comboBoxLabelQRcode.Items.Add("");
                //this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.AccessionNumber);
                //this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.CollectorsEventNumber);
                //this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.DepositorsAccessionNumber);
                //this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.ExternalIdentifier);
                //this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.PartAccessionNumber);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.StableIdentifier);
                this.comboBoxLabelQRcode.Items.Add(DiversityCollection.XmlExport.QRcodeSource.CollectionName);
            }
        }

        private void comboBoxLabelQRcode_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBoxLabelQRcodeType.Items.Clear();
            this.comboBoxLabelQRcodeType.Text = "";
            this.comboBoxLabelQRcodeType.Enabled = false;
            this.comboBoxLabelQRcodeType.Visible = false;
            // TODO: falls doch benoetigt hier wieder aktivieren
            this.comboBoxLabelProject.Visible = false;
            this.labelLabelProject.Visible = false;
            if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.CollectionName.ToString())
            {
                this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.Collection;
            }
            //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.CollectorsEventNumber.ToString())
            //{
            //    this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.Event;
            //}
            //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.DepositorsAccessionNumber.ToString())
            //{
            //    this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.CollectionSpecimen;
            //}
            //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.ExternalIdentifier.ToString())
            //{
            //    this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.Identifier;
            //    this.comboBoxLabelQRcodeType.Items.Add("");
            //    System.Data.DataTable dt = new DataTable();
            //    string SQL = "SELECT Type FROM ExternalIdentifierType ORDER BY Type";
            //    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //    ad.Fill(dt);
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (System.Data.DataRow R in dt.Rows)
            //            this.comboBoxLabelQRcodeType.Items.Add(R[0].ToString());
            //        this.comboBoxLabelQRcodeType.Enabled = true;
            //    }
            //}
            //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.PartAccessionNumber.ToString())
            //{
            //    this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.Specimen;
            //}
            else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.StableIdentifier.ToString())
            {
                this.comboBoxLabelProject.Visible = false;
                this.labelLabelProject.Visible = false;
                if (!this.StableIdentifierSettingsComplete())
                    this.SetQrCodeControls();

                //this.checkBoxLabelQRcode.Checked = false;
                //this.SetQrCodeControls();

                //this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.QRcode;
                //this.setProjectSourceForQR();
                //this.comboBoxLabelProject.Visible = true;
                //this.labelLabelProject.Visible = true;
                //if (this.StableIdentifierSettingsComplete())
                //{
                //    this.comboBoxLabelQRcodeType.Items.Add("");
                //    this.comboBoxLabelQRcodeType.Items.Add(DiversityCollection.XmlExport.QRcodeStableIdentifierType.Collection.ToString());
                //    this.comboBoxLabelQRcodeType.Enabled = true;
                //}
                //else
                //{
                //    //this.checkBoxLabelQRcode_Click(null, null);
                //    this.checkBoxLabelQRcode.Checked = false;
                //    this.SetQrCodeControls();
                //}
            }
            //else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == DiversityCollection.XmlExport.QRcodeSource.StorageLocation.ToString())
            //{
            //    this.pictureBoxLabelQRcodeSource.Image = DiversityCollection.Resource.Specimen;
            //}

        }

        private string StableIdentifierCollection(int CollectionID)
        {
            if (!this.userControlQueryList.ProjectIsSelected)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return "";
            }
            string SIS = "";
            //string SQL = "SELECT [dbo].[StableIdentifier] (" + this.userControlQueryList.ProjectID.ToString() +
            //    ", " + SpecimenID.ToString() + ", " + UnitID.ToString() + ", ";
            //if (PartID == null) SQL += "NULL)";
            //else SQL += PartID.ToString() + ")";
            //SIS = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            return SIS;
        }

        private void setProjectSourceForQR()
        {
            if (this.comboBoxLabelProject.DataSource == null)
            {
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT Project, ProjectID FROM ProjectList ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxLabelProject.DataSource = dt;
                this.comboBoxLabelProject.DisplayMember = "Project";
                this.comboBoxLabelProject.ValueMember = "ProjectID";
                this.comboBoxLabelProject.SelectedIndex = 0;
            }
        }

        #endregion

        #region XML Export

        private void CreateLabel(bool Multi = false, bool IncludeContent = false)
        {
            try
            {
                string SQL = "";
                string Columns = "CollectionID, CollectionParentID, CollectionName, CollectionAcronym, AdministrativeContactName, AdministrativeContactAgentURI, " +
                    "Description, Location, DisplayOrder, CollectionOwner, Type, LocationPlan, LocationPlanWidth, LocationHeight, LocationPlanDate, LocationParentID";
                if (Multi)
                {
                    foreach (int i in this.userControlQueryList.ListOfIDs)
                    {
                        if (SQL.Length > 0) SQL += ", ";
                        SQL += i.ToString();
                    }
                    SQL = "SELECT " + Columns + " FROM Collection WHERE CollectionID IN ( " + SQL + ")";
                }
                else
                {
                    SQL = "SELECT " + Columns + " FROM Collection WHERE CollectionID = " + ((int)this._Collection.ID).ToString();
                }
                DiversityCollection.Datasets.DataSetCollection dsColl = new Datasets.DataSetCollection();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dsColl.Collection);
                int ProjectID = -1;
                if (this.comboBoxLabelProject.SelectedValue != null)
                    ProjectID = int.Parse(this.comboBoxLabelProject.SelectedValue.ToString());

                string File = this.CreateXmlForCollection(dsColl, IncludeContent);
                if (File.Length > 0)
                {
                    try
                    {
                        System.Uri URI = new Uri(File);
                        this.webBrowserLabel.Url = URI;
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string CreateXmlForCollection(DiversityCollection.Datasets.DataSetCollection DsCollection, bool IncludeContent = false)
        {
            string XmlFile = "";
            try
            {
                XmlFile = Folder.LabelPrinting(Folder.LabelPrintingFolder.Collection) + "Label.XML";
                int ProjectID = -1;
                if (this.comboBoxLabelProject.SelectedValue != null)
                    ProjectID = int.Parse(this.comboBoxLabelProject.SelectedValue.ToString());
                //DiversityCollection.XmlExport.QRcodeSourceCollection _QRSource = XmlExport.QRcodeSourceCollection.CollectionName;

                XmlExport.QRcodeSourceCollection QRsource = XmlExport.QRcodeSourceCollection.None;
                if (this.comboBoxLabelQRcode.SelectedItem != null && this.checkBoxLabelQRcode.Checked)
                {
                    if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "CollectionName")
                        QRsource = XmlExport.QRcodeSourceCollection.CollectionName;
                    else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "StableIdentifier")
                        QRsource = XmlExport.QRcodeSourceCollection.StableIdentifier;
                    else if (this.comboBoxLabelQRcode.SelectedItem.ToString() == "")
                        QRsource = XmlExport.QRcodeSourceCollection.None;
                }
                string QRtype = "";
                if (this.comboBoxLabelQRcodeType.SelectedItem != null && this.checkBoxLabelQRcode.Checked)
                    QRtype = this.comboBoxLabelQRcodeType.SelectedItem.ToString();
                string Title = this.textBoxReportTitle.Text;

                DiversityCollection.XmlExport Export = new XmlExport(this.textBoxSchemaFile.Text, XmlFile, DsCollection, ProjectID);
                try
                {
                    int Duplicates = 1;
                    int.TryParse(this.toolStripTextBoxPrintDuplicates.Text, out Duplicates);
                    XmlExport.CollectionContentSorting sorting = XmlExport.CollectionContentSorting.StorageLocation;
                    if (this.toolStripDropDownButtonLabelSplitSorting.Tag != null)
                        sorting = (XmlExport.CollectionContentSorting)this.toolStripDropDownButtonLabelSplitSorting.Tag;
                    return Export.CreateXmlForCollection(DsCollection, ProjectID, Title, QRsource, QRtype, Duplicates, IncludeContent, sorting);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return XmlFile;
        }

        private void storageLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonLabelSplitSorting.Image = storageLocationToolStripMenuItem.Image;
            this.toolStripDropDownButtonLabelSplitSorting.Tag = XmlExport.CollectionContentSorting.StorageLocation;
        }

        private void accessionNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonLabelSplitSorting.Image = accessionNumberToolStripMenuItem.Image;
            this.toolStripDropDownButtonLabelSplitSorting.Tag = XmlExport.CollectionContentSorting.AccessionNumber;
        }

        private void taxonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonLabelSplitSorting.Image = taxonToolStripMenuItem.Image;
            this.toolStripDropDownButtonLabelSplitSorting.Tag = XmlExport.CollectionContentSorting.Taxon;
        }

        #endregion

        #endregion

        #region handling the recording date

        private void buttonCollectionImageRecordingDate_Click(object sender, EventArgs e)
        {
            if (this.collectionImageBindingSource != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionImageBindingSource.Current;
                R["RecordingDate"] = System.DBNull.Value;
                this.setRecordingDateFormat();
            }
        }

        private void setRecordingDateFormat()
        {
            string CustomFormat = "-";
            if (this.collectionImageBindingSource != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionImageBindingSource.Current;
                if (!R["RecordingDate"].Equals(System.DBNull.Value))
                    CustomFormat = "yyyy-MM-dd  HH:mm";
            }
            this.dateTimePickerCollectionImageRecordingDate.CustomFormat = CustomFormat;
        }

        private void dateTimePickerCollectionImageRecordingDate_ValueChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePickerCollectionImageRecordingDate_CloseUp(object sender, EventArgs e)
        {
            if (this.collectionImageBindingSource != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionImageBindingSource.Current;
                R.BeginEdit();
                R["RecordingDate"] = this.dateTimePickerCollectionImageRecordingDate.Value;
                R.EndEdit();
            }
            this.setRecordingDateFormat();
        }

        #endregion

        #endregion

        #region History

        private void buttonHistory_Click(object sender, EventArgs e)
        {
            string Title = "History of " + this.dataSetCollection.Collection.Rows[0]["CollectionName"].ToString() + " (CollectionID: " + this.dataSetCollection.Collection.Rows[0]["CollectionID"].ToString() + ")";
            try
            {
                bool HistoryPresent = false;
                System.Collections.Generic.List<System.Data.DataTable> LogTables = new List<DataTable>();

                if (this.dataSetCollection.Collection.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "CollectionID", this.dataSetCollection.Collection.TableName, ""));
                    HistoryPresent = true;
                }
                if (this.dataSetCollection.CollectionImage.Rows.Count > 0)
                {
                    LogTables.Add(DiversityWorkbench.Database.DtHistory(this.ID, "CollectionID", this.dataSetCollection.CollectionImage.TableName, ""));
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

        #endregion

        #region Button events for table editor and manager

        private void buttonTableEditor_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //    if (this.userControlQueryList.ListOfIDs.Count == 0)
            //    {
            //        System.Windows.Forms.MessageBox.Show("Nothing selected");
            //        this.Cursor = System.Windows.Forms.Cursors.Default;
            //        return;
            //    }
            //    DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(DiversityCollection.Resource.Collection, "Collection", "CollectionID");
            //    f.initTable(this.userControlQueryList.ListOfIDs);
            //    f.StartPosition = FormStartPosition.CenterParent;
            //    f.Width = this.Width - 10;
            //    f.Height = this.Height - 10;
            //    try
            //    {
            //        f.ShowDialog();
            //    }
            //    catch (System.Exception ex)
            //    {
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "TableEditing(string Table, System.Drawing.Image Icon)");
            //}
            //this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonCollectionManager_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormManagers f = new FormManagers();
            f.ShowDialog();
        }

        private void collectionTableEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditTable("Collection", DiversityCollection.Resource.Collection);

        }

        private void imageTableEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.EditTable("CollectionImage", DiversityCollection.Resource.Icones);
        }

        private void EditTable(string TableName, System.Drawing.Image image)
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
                DiversityWorkbench.Forms.FormTableEditor f = new DiversityWorkbench.Forms.FormTableEditor(image, TableName, "CollectionID");
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
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "EditTable(string TableName, System.Drawing.Image image)");
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "EditTable(string TableName, System.Drawing.Image image)");
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        #endregion

        #region Collection Type

        private void initCollectionType()
        {
#if !DEBUG
            //this.comboBoxCollectionType.Enabled = false;
            //return;
#endif
            try
            {
                DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxCollectionType, "CollCollectionType_Enum", DiversityWorkbench.Settings.Connection);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonCollectionType_Click(object sender, EventArgs e)
        {
#if !DEBUG
            //System.Windows.Forms.MessageBox.Show("available in upcoming version");
            //return;
#endif
            DiversityWorkbench.Forms.FormEnumAdministration f = new DiversityWorkbench.Forms.FormEnumAdministration(
                DiversityCollection.Resource.Cupboard,
                "CollCollectionType_Enum",
                "Administration of collection types",
                "",
                DiversityCollection.Specimen.CollectionType_Images);//, Directory);
            f.HierarchyChangesEnabled = true;
            f.setHelp("Collection type");
            //f.setOption("", DiversityCollection.Resource.DiversityWorkbench);
            f.ShowDialog();
            if (f.DataHaveBeenChanged)
            {
                DiversityCollection.Specimen.CollectionType_Images = f.Images;
            }
        }

        private void comboBoxCollectionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxCollectionType.SelectedItem;
            string CollectionType = R["Code"].ToString();
            //this.pictureBoxCollectionType.Image = Specimen.CollectionTypeImage(false, CollectionType);
            this.labelCollectionType.Image = Specimen.CollectionTypeImage(false, CollectionType);
            this.toolStripButtonLabelSplit.Visible = (CollectionType.ToLower() == "subdivided container");
            this.toolStripDropDownButtonLabelSplitSorting.Visible = (CollectionType.ToLower() == "subdivided container");
        }

        private void comboBoxCollectionType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxCollectionType.SelectedItem;
            //string CollectionType = R["Code"].ToString();
            ////this.pictureBoxCollectionType.Image = Specimen.CollectionTypeImage(false, CollectionType);
            //this.labelCollectionType.Image = Specimen.CollectionTypeImage(false, CollectionType);
            //this.toolStripButtonLabelSplit.Visible = (CollectionType.ToLower() == "subdivided container");
            //this.toolStripDropDownButtonLabelSplitSorting.Visible = (CollectionType.ToLower() == "subdivided container");
        }

        #endregion

        #region Hierarchy

        /// <summary>
        /// #205
        /// Initializes the hierarchies for the collection and location.
        /// Includes the location hierarchy if IncludeLocation is true.
        /// <paramref name="IncludeLocation"/> indicates whether to include the location hierarchy in the build process.
        /// </summary>
        private void buildHierarchies(bool IncludeLocation = true)
        {
            if (this._Collection.GetHierarchyDisplayState() != HierarchicalEntity.HierarchyDisplayState.Hide)
            {
                this._Collection.buildHierarchy();
                //if (IncludeLocation)
                //{
                //    if (this._CollectionLocation != null)
                //    {
                //        this._CollectionLocation.BuildHierarchy(this.treeViewLocationHierarchy);
                //    }
                //}
            }
        }

        private void toolStripMenuItemHierarchyWhole_Click(object sender, EventArgs e)
        {
            if (this._Collection.GetHierarchyDisplayState() != HierarchicalEntity.HierarchyDisplayState.Show)
            {
                this.setHierarchyState(HierarchicalEntity.HierarchyDisplayState.Show, toolStripMenuItemHierarchyWhole, true);
                this.CollectionLocation.setHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Show);
            }
        }

        private void toolStripMenuItemHierarchyChildren_Click(object sender, EventArgs e)
        {
            if (this._Collection.GetHierarchyDisplayState() != HierarchicalEntity.HierarchyDisplayState.Children)
            {
                this.setHierarchyState(HierarchicalEntity.HierarchyDisplayState.Children, toolStripMenuItemHierarchyChildren, true);
                this.CollectionLocation.setHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Children);
            }
        }

        private void toolStripMenuItemHierarchySuperior_Click(object sender, EventArgs e)
        {
            if (this._Collection.GetHierarchyDisplayState() != HierarchicalEntity.HierarchyDisplayState.Parents)
            {
                this.setHierarchyState(HierarchicalEntity.HierarchyDisplayState.Parents, toolStripMenuItemHierarchySuperior, true);
                this.CollectionLocation.setHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Parents);
            }
        }

        private void toolStripMenuItemHierarchyNo_Click(object sender, EventArgs e)
        {
            if (this._Collection.GetHierarchyDisplayState() != HierarchicalEntity.HierarchyDisplayState.Hide)
            {
                this.setHierarchyState(HierarchicalEntity.HierarchyDisplayState.Hide, toolStripMenuItemHierarchyNo, true);
            }
        }

        private void toolStripButtonShowPartNodes_Click(object sender, EventArgs e)
        {
            this._Collection.HierarchyIncludingParts = !this._Collection.HierarchyIncludingParts;
            if (_Collection.HierarchyIncludingParts)
                this.toolStripButtonShowPartNodes.Image = DiversityCollection.Resource.SpecimenHierarchy;
            else
                this.toolStripButtonShowPartNodes.Image = DiversityCollection.Resource.SpecimenHierarchyGrey;
        }

        private void setHierarchyState(HierarchicalEntity.HierarchyDisplayState state, System.Windows.Forms.ToolStripMenuItem menuItem, bool setNewItem)
        {
            this._Collection.SetHierarchyDisplayState(state);
            this.toolStripDropDownButtonHierarchyType.Image = menuItem.Image;
            this.toolStripDropDownButtonHierarchyType.ToolTipText = menuItem.Text;
            this.splitContainerCollection.Panel1Collapsed = state == HierarchicalEntity.HierarchyDisplayState.Hide;
            if (state != HierarchicalEntity.HierarchyDisplayState.Hide)
                this.buildHierarchies();
            else
            {
                if (setNewItem)
                    this._Collection.setItem(this.userControlQueryList.ID);
            }
        }


        private void buttonShowHierarchy_Click(object sender, EventArgs e)
        {
            if (this._Collection.GetHierarchyDisplayState() != HierarchicalEntity.HierarchyDisplayState.Hide)
            {
                this._Collection.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Hide);
                this.buttonShowHierarchy.Image = DiversityCollection.Resource.HierarchyNo;
            }
            else
            {
                this._Collection.SetHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Show);
                this.buttonShowHierarchy.Image = DiversityCollection.Resource.Hierarchy;
                this.buildHierarchies();
            }
            this.splitContainerCollection.Panel1Collapsed = this._Collection.GetHierarchyDisplayState() == HierarchicalEntity.HierarchyDisplayState.Hide;
        }

        private void buttonHierarchyCollapse_Click(object sender, EventArgs e)
        {
            if (this.treeViewCollection.Nodes.Count > 1)
            {
                foreach (System.Windows.Forms.TreeNode T in this.treeViewCollection.Nodes)
                    T.Collapse(false);
            }
            else
            {
                foreach (System.Windows.Forms.TreeNode P in this.treeViewCollection.Nodes)
                {
                    foreach (System.Windows.Forms.TreeNode T in P.Nodes)
                    {
                        T.Collapse(false);
                    }
                }
            }
            //this.treeViewCollection.CollapseAll();
        }

        // #205
        //private void toolStripMenuItemHierarchyCollection_Click(object sender, EventArgs e)
        //{
        //    this._Collection.HierarchyAccordingToLocation = false;
        //    //this._Collection.setHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Show);
        //    //this.toolStripDropDownButtonHierarchy.Image = this.toolStripMenuItemHierarchyCollection.Image;
        //    //this.toolStripDropDownButtonHierarchy.ToolTipText = "Hierarchy according to administration";
        //    //this.toolStripDropDownButtonHierarchy.BackColor = System.Drawing.SystemColors.Control;
        //    this.setToolStripsParentLocation(toolStripMenuItemHierarchyCollection);
        //    this.setLocationParentSettingControls();
        //    //this._Collection.buildHierarchy();
        //}


        //private void setToolStripsParentLocation(System.Windows.Forms.ToolStripMenuItem toolStripMenuItem)
        //{
        //    try
        //    {
        //        this.toolStripDropDownButtonHierarchy.Image = toolStripMenuItem.Image;
        //        string Message = "Hierarchy ";
        //        if (this._Collection.GetHierarchyDisplayState() == HierarchicalEntity.HierarchyDisplayState.Parents)
        //            Message = "Superior hierarchy ";
        //        Message += "according to ";
        //        if (this._Collection.HierarchyAccordingToLocation) Message += "location";
        //        else Message += "administration";
        //        this.toolStripDropDownButtonHierarchy.ToolTipText = Message;
        //        if (this._Collection.HierarchyAccordingToLocation)
        //            this.toolStripDropDownButtonHierarchy.BackColor = System.Drawing.Color.SandyBrown;
        //        else
        //            this.toolStripDropDownButtonHierarchy.BackColor = System.Drawing.SystemColors.Control;
        //        this.toolStripButtonSetParentLocation.Visible = this._Collection.HierarchyAccordingToLocation;
        //        this.toolStripButtonRemoveParentLocation.Visible = this._Collection.HierarchyAccordingToLocation;
        //        // Ariane deleted ?
        //        //if(this._Collection.ID != null)
        //        //    this._Collection.setItem((int)this._Collection.ID);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void toolStripButtonSetParentLocation_Click(object sender, EventArgs e)
        //{
        //    if (this.treeViewCollection.SelectedNode != null)
        //    {
        //        if (this.treeViewCollection.SelectedNode.Tag != null)
        //        {
        //            System.Data.DataRow RV = (System.Data.DataRow)this.treeViewCollection.SelectedNode.Tag;
        //            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList
        //                (DiversityCollection.LookupTable.DtCollectionLocationWithHierarchy, "DisplayText", "CollectionID", "Superior locality", "Please select the superior locality");
        //            f.ShowDialog();
        //            if (f.DialogResult == DialogResult.OK)
        //            {
        //                int ID;
        //                if (int.TryParse(f.SelectedValue, out ID))
        //                {
        //                    if (ID != this._Collection.ID && this._Collection.NoHierarchyLoop(int.Parse(RV["CollectionID"].ToString()), ID, true))
        //                    {
        //                        string SQL = "UPDATE C SET LocationParentID = " + ID.ToString() + " FROM Collection C WHERE CollectionID = " + RV["CollectionID"].ToString();
        //                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
        //                            DiversityCollection.LookupTable.DtCollectionLocationWithHierarchyReset();
        //                    }
        //                    else
        //                    {
        //                        return;
        //                        //System.Windows.Forms.MessageBox.Show("This would create a loop within the relations of " + this._MainTable);
        //                    }
        //                }
        //            }
        //            if (this._Collection.ID != null)
        //            {
        //                this._Collection.setItem((int)this._Collection.ID);
        //                this.buildHierarchies();
        //            }
        //        }
        //    }
        //    else { System.Windows.Forms.MessageBox.Show("Please select a node in the tree"); } // zu #205
        //}

        //private void toolStripButtonRemoveParentLocation_Click(object sender, EventArgs e)
        //{
        //    if (this.treeViewCollection.SelectedNode != null)
        //    {
        //        if (this.treeViewCollection.SelectedNode.Tag != null)
        //        {
        //            System.Data.DataRow RV = (System.Data.DataRow)this.treeViewCollection.SelectedNode.Tag;
        //            RV["LocationParentID"] = System.DBNull.Value;
        //            if (this._Collection.ID != null)
        //            {
        //                this._Collection.setItem((int)this._Collection.ID);
        //                this.buildHierarchies();
        //            }
        //        }
        //    }
        //    else { System.Windows.Forms.MessageBox.Show("Please select a node in the tree"); } // zu #205
        //}

        #endregion

        #region Location

        private CollectionLocation _CollectionLocation = null;
        public CollectionLocation CollectionLocation
        {
            get
            {
                if (this._CollectionLocation == null && this._Collection.ID.HasValue)
                {
                    this._CollectionLocation = new CollectionLocation(this._Collection.ID.Value, this.treeViewLocationHierarchy);
                }
                return this._CollectionLocation;
            }
        }

        private void toolStripButtonUseLocation_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation = !DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
            // #222
            this.buttonTransferToLocation.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation; 
            this.setLocationcontrols();
        }

        private void setLocationcontrols()
        {
            // #205
            // this.treeViewCollection.Nodes.Clear();
            if (this._Collection != null && this._Collection.ID.HasValue)
            {
                this._CollectionLocation = new CollectionLocation(this._Collection.ID.Value, this.treeViewLocationHierarchy);
            }
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation)
            {
                this.toolStripButtonUseLocation.Image = DiversityCollection.Resource.LocationHierarchy;
                this.toolStripButtonUseLocation.BackColor = System.Drawing.Color.SandyBrown;
            }
            else
            {
                this.toolStripButtonUseLocation.Image = DiversityCollection.Resource.LocationHierarchyGrey;
                this.toolStripButtonUseLocation.BackColor = System.Drawing.SystemColors.Control;
            }
            this.splitContainerCollectionHierarchy.Panel2Collapsed = !DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
            if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation && this._Collection.ID.HasValue)
            {
                this.CollectionLocation.BuildHierarchy((int)this._Collection.ID);
            }
            // #205
            //this.toolStripDropDownButtonHierarchy.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
            //this.toolStripButtonSetParentLocation.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
            //this.toolStripButtonRemoveParentLocation.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
            //this.buttonTransferToLocation.Visible = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation;
            //if (!DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.UseCollectionLocation)
            //{
            //    this._Collection.HierarchyAccordingToLocation = false;
            //    // Ariane uncomment: call is not needed here, because all is set here or after 'setLocationcontrols' is called
            //    // this.toolStripMenuItemHierarchyCollection_Click(null, null);
            //}
            //this.setLocationParentSettingControls();
        }

        private void toolStripButtonLocationHierarchyRemoveParent_Click(object sender, EventArgs e)
        {
            if (this.treeViewLocationHierarchy.SelectedNode != null
                && this.treeViewLocationHierarchy.SelectedNode.Tag != null
                && this.treeViewLocationHierarchy.SelectedNode.Tag.GetType() == typeof(LocationNode))
            {
                LocationNode locationNode = (LocationNode)this.treeViewLocationHierarchy.SelectedNode.Tag;
                int ChildID = locationNode.ID;
                if (this.CollectionLocation.SetParent(ChildID, null))
                    DiversityCollection.LookupTable.DtCollectionLocationWithHierarchyReset();
            }
            else { System.Windows.Forms.MessageBox.Show("Please select a node in the tree"); } // zu #205
        }

        private void toolStripButtonLocationHierarchySetParent_Click(object sender, EventArgs e)
        {
            if (this.treeViewLocationHierarchy.SelectedNode == null && this.treeViewLocationHierarchy.Nodes.Count == 1)
                this.treeViewLocationHierarchy.SelectedNode = this.treeViewLocationHierarchy.Nodes[0];
            if (this.treeViewLocationHierarchy.SelectedNode != null)
            {
                if (this.treeViewLocationHierarchy.SelectedNode.Tag != null
                && this.treeViewLocationHierarchy.SelectedNode.Tag.GetType() == typeof(LocationNode))
                {
                    LocationNode locationNode = (LocationNode)this.treeViewLocationHierarchy.SelectedNode.Tag;
                    int ChildID = locationNode.ID;
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList
                        (DiversityCollection.LookupTable.DtCollectionLocationWithHierarchy, "DisplayText", "CollectionID", "Superior locality", "Please select the superior locality");
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        int ID;
                        if (int.TryParse(f.SelectedValue, out ID))
                        {
                            if (ID != ChildID && this._Collection.NoHierarchyLoop(ChildID, ID, true))
                            {
                                if (this.CollectionLocation.SetParent(ChildID, ID))
                                    DiversityCollection.LookupTable.DtCollectionLocationWithHierarchyReset();
                            }
                            else
                            {
                                return;
                                //System.Windows.Forms.MessageBox.Show("This would create a loop within the relations of " + this._MainTable);
                            }
                        }
                    }
                }
            }
            else { System.Windows.Forms.MessageBox.Show("Please select a node in the tree"); } // zu #205
        }

        private void buttonLocationHierarchyCollapse_Click(object sender, EventArgs e)
        {
            if (this.buttonLocationHierarchyCollapse.Tag == null)
            {
                this.buttonLocationHierarchyCollapse.Tag = "Collapsed";
                this.buttonLocationHierarchyCollapse.Image = DiversityCollection.Resource.Expand;
                this.toolTip.SetToolTip(this.buttonLocationHierarchyCollapse, "Expand all locations");
                this.treeViewLocationHierarchy.CollapseAll();
            }
            else
            {
                this.buttonLocationHierarchyCollapse.Tag = null;
                this.buttonLocationHierarchyCollapse.Image = DiversityCollection.Resource.Collapse;
                this.toolTip.SetToolTip(this.buttonLocationHierarchyCollapse, "Collapse all locations");
                this.treeViewLocationHierarchy.ExpandAll();
            }
        }

        // 205
        //private void setLocationParentSettingControls()
        //{
        //    this.toolStripButtonRemoveParentLocation.Visible = this._Collection.HierarchyAccordingToLocation;
        //    this.toolStripButtonSetParentLocation.Visible = this._Collection.HierarchyAccordingToLocation;
        //}

        // #205
        //private void toolStripMenuItemHierarchyLocation_Click(object sender, EventArgs e)
        //{
        //    this.treeViewCollection.Nodes.Clear();

        //    this._Collection.HierarchyAccordingToLocation = true;
        //    //this._Collection.setHierarchyDisplayState(HierarchicalEntity.HierarchyDisplayState.Show);
        //    //this.toolStripDropDownButtonHierarchy.Image = this.toolStripMenuItemHierarchyLocation.Image;
        //    //this.toolStripDropDownButtonHierarchy.ToolTipText = "Hierarchy according to location";
        //    //this.toolStripDropDownButtonHierarchy.BackColor = System.Drawing.Color.SandyBrown;
        //    this.setToolStripsParentLocation(toolStripMenuItemHierarchyLocation);
        //    //this._Collection.buildHierarchy();
        //    this.setLocationParentSettingControls();
        //}

        private void treeViewLocationHierarchy_AfterSelect(object sender, TreeViewEventArgs e)
        {
            LocationNode locationNode = (LocationNode)this.treeViewLocationHierarchy.SelectedNode.Tag;
            this.toolStripButtonLocationHierarchySearch.Visible = 
                this.treeViewLocationHierarchy.SelectedNode != null 
                && this.treeViewLocationHierarchy.SelectedNode.Tag != null
                && locationNode.ID != this._Collection.ID
                && locationNode.HasAccess;

            // #221
            this.toolStripButtonLocationHierarchySetParent.Enabled = locationNode.HasAccess;
            this.toolStripButtonLocationHierarchyRemoveParent.Enabled = locationNode.HasAccess;
            //this.setPlanAccessibility(ManagerHasAccessToCollection((int)this.treeViewLocationHierarchy.SelectedNode.Tag));
        }

        private void toolStripButtonLocationHierarchySearch_Click(object sender, EventArgs e)
        {
            if (this.treeViewLocationHierarchy.SelectedNode != null
                && this.treeViewLocationHierarchy.SelectedNode.Tag != null
                && this.treeViewLocationHierarchy.SelectedNode.Tag.GetType() == typeof(LocationNode))
            {
                int ID = -1;
                LocationNode locationNode = (LocationNode)this.treeViewLocationHierarchy.SelectedNode.Tag;
                ID = locationNode.ID;
                if (locationNode.HasAccess)
                {
                    this._Collection.setItem(ID);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("You have no access to this location node", "No Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                //if (int.TryParse(this.treeViewLocationHierarchy.SelectedNode.Tag.ToString(), out ID))
                //{
                //    if (ManagerHasAccessToCollection(ID))
                //    {
                //        this._Collection.setItem((int)this.treeViewLocationHierarchy.SelectedNode.Tag);
                //    }
                //    else
                //    {
                //        System.Windows.Forms.MessageBox.Show("You have no access to this location node", "No Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    }
                //}
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select a valid location node", "Nothing selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool ManagerHasAccessToCollection(int CollectionID)
        {
            return DiversityCollection.Collection.ManagerHasAccessToCollection(CollectionID);
            //// Check if the user has access to the collection
            //// #221
            //string SQL = "SELECT COUNT(*) FROM dbo.ManagerCollectionList() WHERE CollectionID = " + CollectionID.ToString();
            //try
            //{
            //    int i = 0;
            //    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i))
            //        return i > 0; // Access granted if count is greater than 0
            //}
            //catch (Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "ManagerHasAccessToCollection");
            //}
            //return false; // Assume access not granted for now
        }

        #endregion


        #region Tasks

        private System.Data.DataTable _dtTask;

        public void listTasks(int ID)
        {
            this.groupBoxTask.Visible = false;
            return;

            string SQL = "SELECT HierarchyDisplayText, CollectionTaskID FROM [dbo].[CollectionTaskHierarchyAll] () WHERE CollectionID = " + ID.ToString() +
                " ORDER BY HierarchyDisplayText";
            if (this._dtTask == null)
                this._dtTask = new DataTable();
            else
                this._dtTask.Clear();
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtTask, ref Message);
            if (this._dtTask.Rows.Count > 0)
            {
                this.listBoxTask.DataSource = this._dtTask;
                this.listBoxTask.DisplayMember = "HierarchyDisplayText";
                this.listBoxTask.ValueMember = "CollectionTaskID";
                this.groupBoxTask.Visible = true;
            }
            else
                this.groupBoxTask.Visible = false;
        }

        private void buttonTask_Click(object sender, EventArgs e)
        {
            if (this.listBoxTask.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxTask.SelectedItem;
                int CollectionTaskID;
                if (int.TryParse(R["CollectionTaskID"].ToString(), out CollectionTaskID))
                {
                    DiversityCollection.Forms.FormCollectionTask f = new Forms.FormCollectionTask(CollectionTaskID, this._iMainForm);
                    f.ShowDialog();
                }
            }
        }

        private void toolStripButtonTask_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            bool showTask = true;
            if (this.toolStripButtonTask.BackColor == System.Drawing.Color.Yellow)
                showTask = false;
            showTask = !showTask;
            this._Collection.SetTaskVisibility(showTask);
            if (showTask)
            {
                this.toolStripButtonTask.BackColor = System.Drawing.SystemColors.Control;
                this.toolStripButtonTask.Image = DiversityCollection.Resource.Task;
                this.toolStripButtonTaskOpen.Visible = true;
            }
            else
            {
                this.toolStripButtonTask.BackColor = System.Drawing.Color.Yellow;
                this.toolStripButtonTask.Image = DiversityCollection.Resource.TaskGrey;
                this.toolStripButtonTaskOpen.Visible = false;
            }
            this.buildHierarchies(false);
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void toolStripButtonTaskOpen_Click(object sender, EventArgs e)
        {
            bool OK = false;
            if (this.treeViewCollection.SelectedNode != null)
            {
                int CollectionTaskID;
                if (int.TryParse(this.treeViewCollection.SelectedNode.Tag.ToString(), out CollectionTaskID))
                {
                    OK = true;
                    DiversityCollection.Forms.FormCollectionTask f = new Forms.FormCollectionTask(CollectionTaskID, this._iMainForm);
                    f.ShowDialog();
                }
            }
            if (!OK)
                System.Windows.Forms.MessageBox.Show("Please select the task you want to open");
        }

        #endregion

        #region DisplayOrder
        private void maskedTextBoxDisplayOrder_TextChanged(object sender, EventArgs e)
        {
            if (this.maskedTextBoxDisplayOrder.Text == "0" || this.maskedTextBoxDisplayOrder.Text.Length == 0)
                this.maskedTextBoxDisplayOrder.BackColor = System.Drawing.Color.Pink;
            else
                this.maskedTextBoxDisplayOrder.BackColor = System.Drawing.Color.White;
        }

        #endregion

        #region Transfer into hierarchy

        private void buttonTransferToParent_Click(object sender, EventArgs e)
        {
            this.TransferToParent();
        }

        private void buttonTransferToLocation_Click(object sender, EventArgs e)
        {
            this.TransferToParent(true);
        }

        private void TransferToParent(bool ForLocation = false)
        {
            if (this.userControlQueryList.ListOfSelectedIDs.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }

            string Target = "collection";
            if (ForLocation) Target = "location";

            System.Data.DataTable dt = null;
            if (ForLocation)
                dt = DiversityCollection.LookupTable.DtCollectionLocationWithHierarchy;
            else
                dt = DiversityCollection.LookupTable.DtCollectionWithHierarchy;
            int ParentCollectionID = 0;
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "DisplayText", "CollectionID",
                "Transfer to parent " + Target, "Please select the " + Target + " to which the collections in the list should be assigned as children", "", false, true, true, DiversityCollection.Resource.Arrow);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && int.TryParse(f.SelectedValue, out ParentCollectionID))
            {

                bool TakeAll = false;

                //if (this.userControlQueryList.ListOfSelectedIDs.Count == 1 && this.userControlQueryList.ListOfIDs.Count > 1)
                //{
                //    if (System.Windows.Forms.MessageBox.Show("Only one collection has been selected.\r\nShould all collections in the list be transferred as children of the " + Target + "\r\n" + f.SelectedString + "?", "Transfer all?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                //        TakeAll = true;
                //}

                System.Collections.Generic.List<int> IDs = new List<int>();
                if (TakeAll)
                    IDs = this.userControlQueryList.ListOfIDs;
                else
                    IDs = this.userControlQueryList.ListOfSelectedIDs;
                System.Collections.Generic.List<string> HierarchyLoops = new List<string>();
                foreach (int ID in IDs)
                {
                    if (!this._Collection.SetParentCollectionID(ID, ParentCollectionID, ForLocation))
                    {
                        HierarchyLoops.Add(DiversityCollection.LookupTable.CollectionNameHierarchy(ID));
                    }
                }
                if (HierarchyLoops.Count > 0)
                {
                    string Message = (IDs.Count - HierarchyLoops.Count).ToString() + " collections transferred.\r\n" +
                                     "The following collections were not transferred to the " + Target + "\r\n" + f.SelectedString + "\r\nto avoid loops in the hierarchy:\r\n";
                    foreach (string hl in HierarchyLoops)
                    {
                        Message += "\r\n" + hl;
                    }
                    System.Windows.Forms.MessageBox.Show(Message, "Non-transferred collections\r\n\r\n", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(IDs.Count.ToString() + " collections transferred to " + Target + " " + f.SelectedString, "Transfer completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this._Collection.setItem((int)this._Collection.ID);
                DiversityCollection.LookupTable.ResetCollection();
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