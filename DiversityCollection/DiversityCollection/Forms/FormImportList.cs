using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection
{
    public struct ImportColumn
    {
        public string TableName;
        public string TableAlias;
        public string ParentAlias;
        public string ColumnName;
        public string Content;
    }

    public struct ImportColumnGroup
    {
        public string TableName;
        public string TableAlias;
        public string ColumnName;
        //public string Content;
        public string Separator;
        public System.Drawing.Color GroupColor;
    }

    public partial class FormImportList : Form
    {
        #region Parameter

        //private System.Collections.Generic.Dictionary<string, DiversityCollection.ImportColumn> _ImportColumnDictionary = new Dictionary<string, ImportColumn>();
        private System.Collections.Generic.List<DiversityCollection.ImportColumn> _ImportColumns = new List<ImportColumn>();
        private System.Collections.Generic.List<DiversityCollection.ImportColumn> _ImportColumnsAddOn = new List<ImportColumn>();
        private System.Collections.Generic.List<DiversityCollection.ImportColumn> _ImportColumnsAll = new List<ImportColumn>();
        private System.Collections.Generic.Dictionary<string, System.Text.Encoding> _Encodings = new Dictionary<string, Encoding>();

        //private System.Collections.Generic.Dictionary<string, string> _ParentAlias = new Dictionary<string, string>();
        /// <summary>
        /// list of all line groups in the file
        ///     line groups contain the lines related to a dataset defined by the CollectionSpecimenID as first column
        ///     if the CollectionSpecimenID is missing, every line is one dataset
        ///         the lines contain the values
        /// </summary>
        private System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<string>>> _DatasetList;
        private int _DatasetPosition = 0;

        private string _ErrorMessage;
        private string _Transaction = "ListImport";
        private System.Collections.Generic.List<System.Windows.Forms.Control> _PresetControls;
        private System.Collections.Generic.List<System.Windows.Forms.Control> _PresetControlsForSecondPart;
        private DiversityCollection.Datasets.DataSetCollectionSpecimen _DsCollectionSpecimen;
        System.Data.SqlClient.SqlConnection _SqlConnection;
        System.Data.SqlClient.SqlCommand _SqlCommand;

        /// <summary>
        /// List of the panel generated to provide the user with the possibility to specify the localisation system of imported localisations
        /// </summary>
        private System.Collections.Generic.List<System.Windows.Forms.Panel> _LocalisationPanelList = new List<Panel>();
        private System.Collections.Generic.Dictionary<System.Windows.Forms.ComboBox, string> _LocalisationSystemIDComboBoxes = new Dictionary<ComboBox, string>();

        private bool _ImportEmptyData = false;
        private bool _UseColumnMapping = false;
        private bool _IsReimport = false;

        private enum _PartsInData { none, one, two, many};

        private int? _SeriesID = null;

        private System.Collections.Generic.List<System.Drawing.Color> _GroupColorList = new List<Color>();
        private System.Collections.Generic.List<DiversityCollection.ImportColumnGroup> _ImportColumnGroupList = new List<ImportColumnGroup>();

        private bool _AddList = false;


        #endregion

        #region Construction

        public FormImportList(bool IsReimport, bool AddList)
        {
            InitializeComponent();
            this._IsReimport = IsReimport;
            this._AddList = AddList;
            this.setEnumSource();
            this.setPresettingTags();
            this.initRemoteModules();
            this.treeViewAnalysis.ImageList = DiversityCollection.Specimen.ImageList;
            this.tabPageEvent.Controls.Remove(this.panel1);
            this.tabControlPresettings.TabPages.Remove(this.tabPageHidden);
            //this.tabControlPresettings.TabPages.Remove(this.tabPageColumnMapping);
            this.panel1.Dispose();
            this.setEncodingList();
            // online manual
            this.helpProvider.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm";
            this.initGroupColors();
            this.initEventTrees();
            if (!this._IsReimport)
            {
                this._UseColumnMapping = true;
                this.tabControlPresettings.TabPages.Remove(this.tabPageFile);
                if (!this.tabControlPresettings.TabPages.Contains(this.tabPageUnits))
                    this.tabControlPresettings.TabPages.Add(this.tabPageUnits);
                if (!this.tabControlPresettings.TabPages.Contains(this.tabPageSpecimen))
                    this.tabControlPresettings.TabPages.Add(this.tabPageSpecimen);
                if (!this.tabControlPresettings.TabPages.Contains(this.tabPageEvent))
                    this.tabControlPresettings.TabPages.Add(this.tabPageEvent);
                if (!this.tabControlPresettings.TabPages.Contains(this.tabPageColumnMapping))
                    this.tabControlPresettings.TabPages.Add(this.tabPageColumnMapping);
                if (this._AddList)
                {
                    if (!this.tabControlPresettings.TabPages.Contains(this.tabPageAdding))
                        this.tabControlPresettings.TabPages.Add(this.tabPageAdding);
                    this.tabControlPresettings.TabPages[0] = this.tabPageAdding;
                    this.tabControlPresettings.TabPages[1] = this.tabPageColumnMapping;
                    this.tabControlPresettings.TabPages[2] = this.tabPageEvent;
                    this.tabControlPresettings.TabPages[3] = this.tabPageSpecimen;
                    this.tabControlPresettings.TabPages[4] = this.tabPageUnits;
                    this.tabControlPresettings.TabPages[5] = this.tabPageStorage;
                    this.tabControlPresettings.SelectTab(this.tabPageColumnMapping);
                    this.tabControlPresettings.SelectedTab = this.tabPageAdding;
                    this.radioButtonOnePart.Checked = true;
                    this.labelPart1.Visible = false;
                    this.panelPartsContained.Visible = false;
                }
                else
                {
                    this.tabControlPresettings.TabPages.Remove(this.tabPageAdding);
                    this.tabControlPresettings.TabPages[0] = this.tabPageColumnMapping;
                    this.tabControlPresettings.TabPages[1] = this.tabPageEvent;
                    this.tabControlPresettings.TabPages[2] = this.tabPageSpecimen;
                    this.tabControlPresettings.TabPages[3] = this.tabPageUnits;
                    this.tabControlPresettings.TabPages[4] = this.tabPageStorage;
                    this.tabControlPresettings.SelectTab(this.tabPageColumnMapping);
                    this.tabControlPresettings.SelectedTab = this.tabPageColumnMapping;
                }
                this.buttonAnalyse.Enabled = false;
                this.toolStripNavigation.Enabled = false;
            }
            else
            {
                this._UseColumnMapping = false;
                this.tabControlPresettings.TabPages.Remove(this.tabPageColumnMapping);
                this.tabControlPresettings.TabPages.Remove(this.tabPageSpecimen);
                this.tabControlPresettings.TabPages.Remove(this.tabPageEvent);
                this.tabControlPresettings.TabPages.Remove(this.tabPageUnits);
            }
        }
       
        #endregion

        #region Form

        #region Button events

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            if (this.textBoxImportFile.Text.Length > 0)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxImportFile.Text);
                this.openFileDialog.InitialDirectory = FI.DirectoryName;
            }
            else
                this.openFileDialog.InitialDirectory = System.Windows.Forms.Application.StartupPath;
            this.openFileDialog.Filter = "Text Files|*.txt";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxImportFile.Text = f.FullName;
                    this.groupBoxPresettings.Enabled = true;
                    //this.buttonAnalyseStructure.Enabled = true;
                    this.treeViewAnalysis.Nodes.Clear();
                    this.buttonAnalyse.Enabled = false;
                    this.toolStripNavigation.Enabled = false;
                    this.buttonStartImport.Enabled = false;
                    this.ResetAll();
                    //this.checkBoxUseColumnMapping.Enabled = true;
                    if (!this._IsReimport) 
                        this.InitColumnMapping();
                    else
                    {
                        if (this.readFileForPreview())
                            this.buttonAnalyse.Enabled = true;
                    }
                }
                //else
                    //this.checkBoxUseColumnMapping.Enabled = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            if (!this._IsReimport) this.InitColumnMapping();
        }

        private void textBoxImportFile_TextChanged(object sender, EventArgs e)
        {
            this.buttonAnalyse.Enabled = false;
            this.toolStripNavigation.Enabled = false;
        }

        private void buttonAnalyse_Click(object sender, EventArgs e)
        {
            this.toolStripLabelCurrent.Text = "1";
            this._ErrorMessage = "";
            if (this.textBoxImportFile.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a file");
                return;
            }
            if (!this.RefillImportColumnsAndCheckForChanges())
                return;
            this.treeViewAnalysis.Nodes.Clear();
            int StartingLine = 6;
            if (!this._IsReimport)
                StartingLine = (int)this.numericUpDownDataStartInLine.Value;
            this.ReadDataFromFileToDatasetList((int?)this.numericUpDownUpTo.Value - 1);

            this.analyseData(0);
            if (this._DatasetList.Count > 1)
            {
                this.toolStripButtonNext.Enabled = true;
                this.toolStripButtonLast.Enabled = true;
                if (this._IsReimport)
                    this.toolStripNavigation.Enabled = true;
            }
            else
            {
                this.toolStripButtonNext.Enabled = false;
                this.toolStripButtonLast.Enabled = false;
                this.toolStripNavigation.Enabled = false;
            }
            this.toolStripButtonPrevious.Enabled = false;
            this.toolStripButtonFirst.Enabled = false;
            if (this._ErrorMessage.Length == 0) this.buttonStartImport.Enabled = true;
            else this.buttonStartImport.Enabled = false;
        }
        
        private void buttonStartImport_Click(object sender, EventArgs e)
        {
            int? MaxLines = null;
            if (this.radioButtonImportFirstLines.Checked) MaxLines = (int)this.numericUpDownImport.Value;
            string M = this.importData(MaxLines);
            if (M.Length > 0)
                System.Windows.Forms.MessageBox.Show(M);
        }

        //private void buttonAnalyseStructure_Click(object sender, EventArgs e)
        //{
        //    //this.treeViewAnalysis.Nodes.Clear();
        //    //if (this.analyseImportFile())
        //    //{
        //    //    this.buttonAnalyse.Enabled = true;
        //    //    this.buttonStartImport.Enabled = true;
        //    //}
        //    //else
        //    //{
        //    //    this.buttonAnalyse.Enabled = false;
        //    //    this.buttonStartImport.Enabled = false;
        //    //}
        //}

        //private void buttonTestData_Click(object sender, EventArgs e)
        //{
        //    //this._OK = true;
        //    //this.analyseImportFile();
        //    //this._DatasetList = this.getDatasets((int?)this.numericUpDownUpTo.Value);
        //    //if (this._OK) this.buttonStartImport.Enabled = true;
        //    //else System.Windows.Forms.MessageBox.Show(this._ErrorMessage);
        //}   
     
        private void buttonHeaderFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name, System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
            //string ErrorLog = DiversityWorkbench.ExceptionHandling.ErrorLogFile;
            //string User = "";
            //string SQL = "SELECT USER_NAME()";
            //try
            //{
            //    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //    System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            //    con.Open();
            //    User = C.ExecuteScalar().ToString();
            //    con.Close();
            //    con.Dispose();
            //}
            //catch { }
            //if (User.Length == 0) User = System.Environment.UserName;
            //string Database = DiversityWorkbench.Settings.DatabaseName;
            //string Module = System.Windows.Forms.Application.ProductName + " " + System.Windows.Forms.Application.ProductVersion;
            //DiversityWorkbench.FormFeedback f = new DiversityWorkbench.FormFeedback(Module, Database, User, ErrorLog);
            //f.ShowDialog();
        }

        #endregion

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.saveFileDialog = new SaveFileDialog();
            string Dir = System.Windows.Forms.Application.StartupPath + "\\ImportMappings";
            if (!System.IO.Directory.Exists(Dir))
                System.IO.Directory.CreateDirectory(Dir);
            this.saveFileDialog.InitialDirectory = Dir;
            this.saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            this.saveFileDialog.FileName = "ImportSchema_" + System.Environment.UserName + ".xml";
            this.saveFileDialog.ShowDialog();
            if (this.saveFileDialog.FileName.Length > 0)
                this.WriteColumnMappingToFile(this.saveFileDialog.FileName);
        }

        private void buttonLoadSettings_Click(object sender, EventArgs e)
        {
            string Dir = System.Windows.Forms.Application.StartupPath;
            if (System.IO.Directory.Exists(Dir + "\\ImportMappings")) Dir += "\\ImportMappings";
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            this.openFileDialog.InitialDirectory = Dir;
            this.openFileDialog.Filter = "XML Files|*.xml";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    this.ImportColumnMappingFromFile(this.openFileDialog.FileName);
                    if (this.ColumnMappingComplete)
                    {
                        this.buttonAnalyse.Enabled = true;
                        this.toolStripNavigation.Enabled = true;
                    }
                    else
                    {
                        this.buttonAnalyse.Enabled = false;
                        this.toolStripNavigation.Enabled = false;
                    }
                }
                //else
                //    this.checkBoxUseColumnMapping.Enabled = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //public bool AddList
        //{
        //    get { return _AddList; }
        //    set 
        //    { 
        //        _AddList = value;
        //        if (_AddList)
        //        {

        //        }
        //    }
        //}


        #region Toolstrip for navigation
        private void toolStripButtonPrevious_Click(object sender, EventArgs e)
        {
            this.moveDatasetPosition(int.Parse(this.toolStripLabelCurrent.Text)-1);
            //this.moveDatasetPosition(false);
        }

        private void toolStripButtonNext_Click(object sender, EventArgs e)
        {
            this.moveDatasetPosition(int.Parse(this.toolStripLabelCurrent.Text) + 1);
            //this.moveDatasetPosition(true);
        }

        private void toolStripButtonFirst_Click(object sender, EventArgs e)
        {
            this.moveDatasetPosition(1);
        }

        private void toolStripButtonLast_Click(object sender, EventArgs e)
        {
            this.moveDatasetPosition(this._DatasetList.Count);
        }

        private void moveDatasetPosition(int Position)
        {
            try
            {
                // Position out of range
                if (Position < 1)
                {
                    this.toolStripButtonPrevious.Enabled = false;
                    this.toolStripButtonFirst.Enabled = false;
                    this.toolStripLabelCurrent.Text = "1";
                    return;
                }
                else
                {
                    this.toolStripButtonPrevious.Enabled = true;
                    this.toolStripButtonFirst.Enabled = true;
                }
                if (Position > (this._DatasetList.Count))
                {
                    this.toolStripButtonNext.Enabled = false;
                    this.toolStripButtonLast.Enabled = false;
                    this.toolStripLabelCurrent.Text = this._DatasetList.Count.ToString();
                    return;
                }
                else
                {
                    this.toolStripButtonNext.Enabled = true;
                    this.toolStripButtonLast.Enabled = true;
                }

                // Position within range
                this.toolStripLabelCurrent.Text = Position.ToString();
                this.analyseData(Position - 1);
                if (Position <= 1)
                {
                    this.toolStripButtonPrevious.Enabled = false;
                    this.toolStripButtonFirst.Enabled = false;
                }
                else
                {
                    this.toolStripButtonPrevious.Enabled = true;
                    this.toolStripButtonFirst.Enabled = true;
                }
                if (Position == this._DatasetList.Count)
                {
                    this.toolStripButtonNext.Enabled = false;
                    this.toolStripButtonLast.Enabled = false;
                }
                else
                {
                    this.toolStripButtonNext.Enabled = true;
                    this.toolStripButtonLast.Enabled = true;
                }
            }
            catch { }
        }

        //private void moveDatasetPosition(bool Next)
        //{
        //    int Pos = 0;
        //    if (int.TryParse(this.toolStripLabelCurrent.Text, out Pos))
        //    {
        //        if (Next) Pos++;
        //        else Pos--;
        //        if (Pos < 0)
        //        {
        //            this.toolStripButtonPrevious.Enabled = false;
        //            this.toolStripLabelCurrent.Text = "0";
        //            return;
        //        }
        //        else
        //        {
        //            this.toolStripButtonPrevious.Enabled = true;
        //        }
        //        if (Pos > (this._DatasetList.Count - 1))
        //        {
        //            this.toolStripButtonNext.Enabled = false;
        //            this.toolStripLabelCurrent.Text = this._DatasetList.Count.ToString();
        //            return;
        //        }
        //        else
        //        {
        //            this.toolStripButtonNext.Enabled = true;
        //        }
        //        this.toolStripLabelCurrent.Text = Pos.ToString();
        //        this.analyseData(Pos);
        //        if (Pos == 0) this.toolStripButtonPrevious.Enabled = false;
        //        if (Pos == this._DatasetList.Count - 1) this.toolStripButtonNext.Enabled = false;
        //    }
        //}
        
        #endregion

        private void setEnumSource()
        {
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxMaterialCategory, "CollMaterialCategory_Enum", con, true, true);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxMaterialCategory2, "CollMaterialCategory_Enum", con, true, true);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTaxonomicGroup, "CollTaxonomicGroup_Enum", con, true, true);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTaxonomicGroupUnit2, "CollTaxonomicGroup_Enum", con, true, true);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxImageType, "CollSpecimenImageType_Enum", con, true, true);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxLabelType, "CollLabelType_Enum", con, true, true);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxRelationType, "CollUnitRelationType_Enum", con, true, true);
            // Collection
            System.Data.DataTable dtCollection = new System.Data.DataTable("Collection");
            string SQL = "SELECT NULL AS CollectionID, NULL AS CollectionName UNION SELECT CollectionID, CollectionName from dbo.Collection WHERE CollectionName IS NOT NULL ORDER BY CollectionName";
            System.Data.SqlClient.SqlDataAdapter a = new System.Data.SqlClient.SqlDataAdapter(SQL, con);
            try
            {
                a.Fill(dtCollection);
                this.comboBoxCollection.DataSource = dtCollection;
                this.comboBoxCollection.DisplayMember = "CollectionName";
                this.comboBoxCollection.ValueMember = "CollectionID";

                System.Data.DataTable dtCollection2 = dtCollection.Copy();
                this.comboBoxCollection2.DataSource = dtCollection2;
                this.comboBoxCollection2.DisplayMember = "CollectionName";
                this.comboBoxCollection2.ValueMember = "CollectionID";
            }
            catch  {}
            this.userControlHierarchySelectorCollection.initHierarchy(
                DiversityCollection.LookupTable.DtCollection,
                "CollectionID",
                "CollectionParentID",
                "CollectionName",
                "CollectionName",
                "CollectionID",
                this.comboBoxCollection);
            this.userControlHierarchySelectorCollection2.initHierarchy(
                DiversityCollection.LookupTable.DtCollection,
                "CollectionID",
                "CollectionParentID",
                "CollectionName",
                "CollectionName",
                "CollectionID",
                this.comboBoxCollection2);
            // Projects
            System.Data.DataTable dtProjects = new System.Data.DataTable("Projects");
            //a.SelectCommand.CommandText = "SELECT NULL AS ProjectID, NULL AS Project UNION SELECT ProjectID, Project FROM  ProjectProxy ORDER BY Project";
            a.SelectCommand.CommandText = "SELECT ProjectID, Project FROM ProjectList ORDER BY Project";
            try
            {
                a.Fill(dtProjects);
                this.comboBoxProject.DataSource = dtProjects;
                this.comboBoxProject.DisplayMember = "Project";
                this.comboBoxProject.ValueMember = "ProjectID";
            }
            catch { }
            // Country
            System.Data.DataTable dtCountry = new DataTable();
            a.SelectCommand.CommandText = "SELECT NULL AS NameEn UNION SELECT DISTINCT NameEn FROM DiversityGazetteer.dbo.GeoCountries WHERE (NameEn NOT LIKE '(%') ORDER BY NameEn";
            try
            {
                a.Fill(dtCountry);
                this.comboBoxCountry.DataSource = dtCountry;
                this.comboBoxCountry.DisplayMember = "NameEn";
                this.comboBoxCountry.ValueMember = "NameEn";
            }
            catch { }
        }

        private void initRemoteModules()
        {
            try
            {
                // Agents
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryCollector.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryIdentifiedBy.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryDepositor.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;

                // Exsiccatae
                DiversityWorkbench.Exsiccate E = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryExsiccate.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)E;

                // Gazetteer
                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryGazetteer.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;

                // TaxonNames
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryIdentification.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setEncodingList()
        {
            this._Encodings.Add("", null);
            this._Encodings.Add("ASCII", System.Text.Encoding.ASCII);
            //this._Encodings.Add("Unicode", System.Text.Encoding.Unicode);
            this._Encodings.Add("UTF8", System.Text.Encoding.UTF8);
            this._Encodings.Add("UTF32", System.Text.Encoding.UTF32);
            foreach (System.Collections.Generic.KeyValuePair<string, System.Text.Encoding> KV in this._Encodings)
            {
                this.comboBoxEncoding.Items.Add(KV.Key);
            }
            if (this._IsReimport)
                this.comboBoxEncoding.SelectedIndex = 2;
            else
                this.comboBoxEncoding.SelectedIndex = 0;
        }

        #region Event trees and event series

        private void initEventTrees()
        {
            this.treeViewEventsSeparate.ExpandAll();
            this.treeViewEventsInGroups.ExpandAll();
        }

        private void radioButtonEventsSeparate_CheckedChanged(object sender, EventArgs e)
        {
            this.treeViewEventsInGroups.Enabled = !this.radioButtonEventsSeparate.Checked;
            this.treeViewEventsSeparate.Enabled = this.radioButtonEventsSeparate.Checked;
            if (this.radioButtonEventsSeparate.Checked)
            {
                this.treeViewEventsInGroups.ImageList = this.imageListGrey;
                this.treeViewEventsSeparate.ImageList = this.imageList;
                this.treeViewEventsInGroups.BackColor = System.Drawing.SystemColors.Control;
                this.treeViewEventsSeparate.BackColor = System.Drawing.SystemColors.Window;
            }
            else
            {
                this.treeViewEventsInGroups.ImageList = this.imageList;
                this.treeViewEventsSeparate.ImageList = this.imageListGrey;
                this.treeViewEventsInGroups.BackColor = System.Drawing.SystemColors.Window;
                this.treeViewEventsSeparate.BackColor = System.Drawing.SystemColors.Control;
            }
        }

        private void checkBoxEventSeries_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxEventSeries.Checked)
            {
                this.textBoxEventSeriesCode.Enabled = true;
                this.textBoxEventSeriesDescription.Enabled = true;
                this.textBoxEventSeriesNotes.Enabled = true;
            }
            else
            {
                this.textBoxEventSeriesCode.Text = "";
                this.textBoxEventSeriesDescription.Text = "";
                this.textBoxEventSeriesNotes.Text = "";
                this.textBoxEventSeriesCode.Enabled = false;
                this.textBoxEventSeriesDescription.Enabled = false;
                this.textBoxEventSeriesNotes.Enabled = false;
            }
        }

        private void buttonGetEventSeries_Click(object sender, EventArgs e)
        {
            DiversityCollection.FormCollectionEventSeries f = new FormCollectionEventSeries();
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.ID != null)
            {
                try
                {
                    if (f.ID != null)
                    {
                        this._SeriesID = f.ID;
                        this.textBoxEventSeriesCode.Text = f.ColumnCode;
                        this.textBoxEventSeriesDescription.Text = f.DisplayText;
                        this.textBoxEventSeriesNotes.Text = f.ColumnNotes;
                        this.textBoxEventSeriesCode.Enabled = false;
                        this.textBoxEventSeriesDescription.Enabled = false;
                        this.textBoxEventSeriesNotes.Enabled = false;
                    }
                    else
                    {
                        this._SeriesID = null;
                        this.textBoxEventSeriesCode.Enabled = true;
                        this.textBoxEventSeriesDescription.Enabled = true;
                        this.textBoxEventSeriesNotes.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        #endregion

        #endregion

        #region Presetting

        #region Common

        private void setPresettingTags()
        {
            DiversityCollection.Datasets.DataSetCollectionSpecimen ds = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            // Storage
            this.comboBoxCollection.Tag = ds.CollectionSpecimenPart.CollectionIDColumn;
            //this.comboBoxCollection2.Tag = ds.CollectionSpecimenPart.CollectionIDColumn;
            this.comboBoxMaterialCategory.Tag = ds.CollectionSpecimenPart.MaterialCategoryColumn;
            //this.comboBoxMaterialCategory2.Tag = ds.CollectionSpecimenPart.MaterialCategoryColumn;
            this.comboBoxDerivedFrom.Tag = ds.CollectionSpecimenPart.DerivedFromSpecimenPartIDColumn;
            //this.comboBoxDerivedFrom2.Tag = ds.CollectionSpecimenPart.DerivedFromSpecimenPartIDColumn;

            this.comboBoxImageType.Tag = ds.CollectionSpecimenImage.ImageTypeColumn;
            this.comboBoxLabelType.Tag = ds.CollectionSpecimen.LabelTypeColumn;
            this.comboBoxProject.Tag = ds.CollectionProject.ProjectIDColumn;

            this.comboBoxTaxonomicGroup.Tag = ds.IdentificationUnit.TaxonomicGroupColumn;
            this.comboBoxTaxonomicGroupUnit2.Tag = ds.IdentificationUnit.TaxonomicGroupColumn;
            this.comboBoxRelationType.Tag = ds.IdentificationUnit.RelationTypeColumn;

            this.comboBoxCountry.Tag = ds.CollectionEvent.CountryCacheColumn;
            this.textBoxCollectionTime.Tag = ds.CollectionEvent.CollectionTimeColumn;
            this.textBoxCollectionTimeSpan.Tag = ds.CollectionEvent.CollectionTimeSpanColumn;
            //this.textBoxEventSeriesCode.Tag = ds.CollectionEventSeries.SeriesCodeColumn;
            //this.textBoxEventSeriesDescription.Tag = ds.CollectionEventSeries.DescriptionColumn;
            //this.textBoxEventSeriesNotes.Tag = ds.CollectionEventSeries.NotesColumn;

            this.userControlModuleRelatedEntryCollector.textBoxValue.Tag = ds.CollectionAgent.CollectorsNameColumn;
            this.userControlModuleRelatedEntryCollector.labelURI.Tag = ds.CollectionAgent.CollectorsAgentURIColumn;

            this.userControlDatePanelAccessionDate.maskedTextBoxDay.Tag = ds.CollectionSpecimen.AccessionDayColumn;
            this.userControlDatePanelAccessionDate.maskedTextBoxMonth.Tag = ds.CollectionSpecimen.AccessionMonthColumn;
            this.userControlDatePanelAccessionDate.maskedTextBoxYear.Tag = ds.CollectionSpecimen.AccessionYearColumn;
            this.userControlDatePanelAccessionDate.textBoxSupplement.Tag = ds.CollectionSpecimen.AccessionDateSupplementColumn;

            this.userControlDatePanelCollectionDate.maskedTextBoxDay.Tag = ds.CollectionEvent.CollectionDayColumn;
            this.userControlDatePanelCollectionDate.maskedTextBoxMonth.Tag = ds.CollectionEvent.CollectionMonthColumn;
            this.userControlDatePanelCollectionDate.maskedTextBoxYear.Tag = ds.CollectionEvent.CollectionYearColumn;
            this.userControlDatePanelCollectionDate.textBoxSupplement.Tag = ds.CollectionEvent.CollectionDateSupplementColumn;

            this.userControlModuleRelatedEntryExsiccate.textBoxValue.Tag = ds.CollectionSpecimen.ExsiccataAbbreviationColumn;
            this.userControlModuleRelatedEntryExsiccate.labelURI.Tag = ds.CollectionSpecimen.ExsiccataURIColumn;

            this.userControlModuleRelatedEntryGazetteer.textBoxValue.Tag = ds.CollectionEventLocalisation.Location1Column;
            this.userControlModuleRelatedEntryGazetteer.labelURI.Tag = ds.CollectionEventLocalisation.Location2Column;

            this.userControlModuleRelatedEntryIdentification.textBoxValue.Tag = ds.Identification.TaxonomicNameColumn;
            this.userControlModuleRelatedEntryIdentification.labelURI.Tag = ds.Identification.NameURIColumn;

            this.userControlModuleRelatedEntryIdentifiedBy.textBoxValue.Tag = ds.Identification.ResponsibleNameColumn;
            this.userControlModuleRelatedEntryIdentifiedBy.labelURI.Tag = ds.Identification.ResponsibleAgentURIColumn;

            this.userControlModuleRelatedEntryDepositor.textBoxValue.Tag = ds.CollectionSpecimen.DepositorsNameColumn;
            this.userControlModuleRelatedEntryDepositor.labelURI.Tag = ds.CollectionSpecimen.DepositorsAgentURIColumn;

            this._PresetControls = new List<Control>();
            this._PresetControls.Add(this.comboBoxCollection);
            //this._PresetControls.Add(this.comboBoxCollection2);
            this._PresetControls.Add(this.comboBoxImageType);
            this._PresetControls.Add(this.comboBoxLabelType);
            this._PresetControls.Add(this.comboBoxMaterialCategory);
            //this._PresetControls.Add(this.comboBoxMaterialCategory2);
            this._PresetControls.Add(this.comboBoxDerivedFrom);
            //this._PresetControls.Add(this.comboBoxDerivedFrom2);
            this._PresetControls.Add(this.comboBoxProject);
            this._PresetControls.Add(this.comboBoxTaxonomicGroup);
            this._PresetControls.Add(this.comboBoxTaxonomicGroupUnit2);
            this._PresetControls.Add(this.comboBoxCountry);
            this._PresetControls.Add(this.comboBoxRelationType);
            this._PresetControls.Add(this.textBoxCollectionTime);
            this._PresetControls.Add(this.textBoxCollectionTimeSpan);
            //this._PresetControls.Add(this.textBoxEventSeriesNotes);
            //this._PresetControls.Add(this.textBoxEventSeriesDescription);
            //this._PresetControls.Add(this.textBoxEventSeriesCode);

            this._PresetControls.Add(this.userControlModuleRelatedEntryCollector.textBoxValue);
            this._PresetControls.Add(this.userControlModuleRelatedEntryCollector.labelURI);

            this._PresetControls.Add(this.userControlDatePanelAccessionDate.maskedTextBoxDay);
            this._PresetControls.Add(this.userControlDatePanelAccessionDate.maskedTextBoxMonth);
            this._PresetControls.Add(this.userControlDatePanelAccessionDate.maskedTextBoxYear);
            this._PresetControls.Add(this.userControlDatePanelAccessionDate.textBoxSupplement);

            this._PresetControls.Add(this.userControlDatePanelCollectionDate.maskedTextBoxDay);
            this._PresetControls.Add(this.userControlDatePanelCollectionDate.maskedTextBoxMonth);
            this._PresetControls.Add(this.userControlDatePanelCollectionDate.maskedTextBoxYear);
            this._PresetControls.Add(this.userControlDatePanelCollectionDate.textBoxSupplement);

            this._PresetControls.Add(this.userControlModuleRelatedEntryExsiccate.textBoxValue);
            this._PresetControls.Add(this.userControlModuleRelatedEntryExsiccate.labelURI);

            this._PresetControls.Add(this.userControlModuleRelatedEntryGazetteer.textBoxValue);
            this._PresetControls.Add(this.userControlModuleRelatedEntryGazetteer.labelURI);

            this._PresetControls.Add(this.userControlModuleRelatedEntryIdentification.textBoxValue);
            this._PresetControls.Add(this.userControlModuleRelatedEntryIdentification.labelURI);

            this._PresetControls.Add(this.userControlModuleRelatedEntryIdentifiedBy.textBoxValue);
            this._PresetControls.Add(this.userControlModuleRelatedEntryIdentifiedBy.labelURI);

            this._PresetControls.Add(this.userControlModuleRelatedEntryDepositor.labelURI);
            this._PresetControls.Add(this.userControlModuleRelatedEntryDepositor.textBoxValue);

            //this._PresetControlsForSecondPart = new List<Control>();
            //this._PresetControlsForSecondPart.Add(this.comboBoxCollection2);
            //this._PresetControlsForSecondPart.Add(this.comboBoxMaterialCategory2);
            //this._PresetControlsForSecondPart.Add(this.comboBoxDerivedFrom2);
        }

        private void addMissingPkColumns()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
            {
                System.Collections.Generic.List<string> Columns = this.getTableColumns(KV.Key);
                System.Collections.Generic.List<string> PK = this.getPrimaryKey(KV.Value);
                foreach (string s in PK)
                {
                    if (!Columns.Contains(s))
                    {
                        DiversityCollection.ImportColumn I = new ImportColumn();
                        I.TableAlias = KV.Key;
                        I.TableName = KV.Value;
                        I.ColumnName = s;
                        //if (KV.Key == "CollectionSpecimenPart" && KV.Key == KV.Value && s == "SpecimenPartID")
                        //    I.Content = "1";
                        //else 
                            I.Content = "";
                        this._ImportColumnsAddOn.Add(I);
                        this._ImportColumnsAll = null;
                    }
                }
            }
        }

        private void addMissingTables()
        {
            this.addMissingSpecimenTable();
            this.addMissingUnitTables();
            this.addMissingEventTable();
            this.AddMissingPartTables();
        }

        private void addMissingSpecimenTable()
        {
            if (!this.AliasDictionary.ContainsValue("CollectionSpecimen"))
            {
                bool CollectionSpecimenIDInSpecimen = false;
                string AliasSpecimen = this.AliasCollectionSpecimen;
                if (AliasSpecimen.Length == 0)
                    AliasSpecimen = "CollectionSpecimen";
                foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                {
                    if (I.ColumnName == "CollectionSpecimen"
                        && I.TableAlias == AliasSpecimen)
                        CollectionSpecimenIDInSpecimen = true;
                }
                if (!CollectionSpecimenIDInSpecimen)
                {
                    DiversityCollection.ImportColumn ISpecimen = new ImportColumn();
                    ISpecimen.TableAlias = "CollectionSpecimen";
                    ISpecimen.TableName = "CollectionSpecimen";
                    ISpecimen.ColumnName = "CollectionSpecimenID";
                    ISpecimen.Content = "";
                    this._ImportColumnsAddOn.Add(ISpecimen);

                    DiversityCollection.ImportColumn IProblems = new ImportColumn();
                    IProblems.TableAlias = "CollectionSpecimen";
                    IProblems.TableName = "CollectionSpecimen";
                    IProblems.ColumnName = "InternalNotes";
                    IProblems.Content = "Import without Acc.Nr. by " + System.Environment.UserName + " at " + System.DateTime.Now.ToShortDateString();
                    this._ImportColumnsAddOn.Add(IProblems);

                    this._ImportColumnsAll = null;

                    if (this.AliasDictionary.ContainsValue("CollectionEvent"))
                    {
                        bool EventIdInSpecimen = false;
                        foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                        {
                            if (I.ColumnName == "CollectionEventID"
                                && I.TableAlias == AliasSpecimen)
                                EventIdInSpecimen = true;
                        }
                        if (!EventIdInSpecimen)
                        {
                            DiversityCollection.ImportColumn IE = new ImportColumn();
                            IE.TableAlias = AliasSpecimen;
                            IE.TableName = "CollectionSpecimen";
                            IE.ColumnName = "CollectionEventID";
                            IE.Content = "";
                            this._ImportColumnsAddOn.Add(IE);
                        }
                    }

                    this._ImportColumnsAll = null;
                }
            }
        }

        private void initPresettings()
        {
            // EventSeries
            if (this.checkBoxEventSeries.Checked)
            {
                DiversityCollection.ImportColumn I = new ImportColumn();
                I.TableAlias = "CollectionEvent";
                I.TableName = "CollectionEvent";
                I.ColumnName = "SeriesID";
                if (this._SeriesID == null)
                    I.Content = "";
                else
                    I.Content = this._SeriesID.ToString();
                this._ImportColumnsAddOn.Add(I);
                this._ImportColumnsAll = null;
            }

            string Table = "";
            string Column = "";
            string Value = "";
            string Alias = "";
            // check all preset controls
            foreach (System.Windows.Forms.Control C in this._PresetControls)
            {
                if (C.Tag == null) continue;
                //if (!C.Visible) continue;
                System.Data.DataColumn col = (System.Data.DataColumn)C.Tag;
                Table = col.Table.TableName;
                Column = col.ColumnName;
                Value = "";
                Alias = col.Table.TableName;
                if (Alias == "IdentificationUnit" && this.ManyUnitTables)
                {
                    if (C == this.comboBoxTaxonomicGroup)
                        Alias = this.textBoxUnitTable1.Text;
                    else if (C == this.comboBoxTaxonomicGroupUnit2)
                        Alias = this.textBoxUnitTable2.Text;
                    else if (C == this.comboBoxRelationType)
                    {
                        if (this.radioButtonUnit1isHost.Checked)
                            Alias = this.textBoxUnitTable2.Text;
                        else
                            Alias = this.textBoxUnitTable1.Text;
                    }
                }
                if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    System.Windows.Forms.ComboBox c = (System.Windows.Forms.ComboBox)C;
                    if (c.SelectedIndex > -1 && c.SelectedValue != null)
                    {
                        Value = c.SelectedValue.ToString();
                    }
                    if (this._LocalisationSystemIDComboBoxes.ContainsKey(c))
                    {
                        Alias = this._LocalisationSystemIDComboBoxes[c];
                    }
                }
                else if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    Value = C.Text;
                }
                else if (C.GetType() == typeof(System.Windows.Forms.MaskedTextBox))
                {
                    Value = C.Text;
                }
                if (Value.Length > 0)
                {
                    // only if there is a value, add it to the presettings
                    bool ValueIsSet = false;
                    if (this.TableListAll.Contains(Table))
                    {
                        // if the table is allready there, take its alias for the new field
                        int TableCount = 0;
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                        {
                            if (KV.Value == Table && KV.Key == Table)
                            {
                                TableCount++;
                                if (Alias.Length == 0)// || Table == Alias)
                                {
                                    Alias = KV.Key;
                                }
                            }
                        }
                        if (Column == "RelationType")
                        {
                            if (!this.ColumnList.Contains("RelationType"))
                            {
                                DiversityCollection.ImportColumn I = new ImportColumn();
                                I.TableAlias = this.TableOfParasite;
                                I.TableName = "IdentificationUnit";
                                I.ColumnName = "RelationType";
                                I.Content = Value;
                                this._ImportColumnsAddOn.Add(I);
                                this._ImportColumnsAll = null;
                                ValueIsSet = true;
                            }
                        }
                        else if (TableCount <= 1)
                        {
                            // only if the table is unique, that means that there is only one alias, this alias can be used for the presetting
                            System.Collections.Generic.List<string> TableColumnList = this.getTableColumns(Alias);
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                            {
                                // get the colomns of this 
                                if (KV.Key == Alias
                                    && KV.Value == Table
                                    && TableColumnList.Contains(Column))
                                {
                                    for (int i = 0; i < this._ImportColumnsAddOn.Count; i++)
                                    {
                                        if (this._ImportColumnsAddOn[i].TableAlias == Alias
                                            && this._ImportColumnsAddOn[i].TableName == Table
                                            && this._ImportColumnsAddOn[i].ColumnName == Column)
                                        {
                                            DiversityCollection.ImportColumn IC = this._ImportColumnsAddOn[i];
                                            IC.ColumnName = this._ImportColumnsAddOn[i].ColumnName;
                                            IC.Content = this._ImportColumnsAddOn[i].Content;
                                            IC.ParentAlias = this._ImportColumnsAddOn[i].ParentAlias;
                                            IC.TableAlias = this._ImportColumnsAddOn[i].TableAlias;
                                            IC.TableName = this._ImportColumnsAddOn[i].TableName;
                                            IC.Content = Value;
                                            this._ImportColumnsAddOn[i] = IC;
                                            ValueIsSet = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            // if the table IdentificationUnit ...
                            //if (Alias == "IdenitifcationUnit"
                        }
                    }
                    else
                    {
                        // the table is missing
                        DiversityCollection.ImportColumn I = new ImportColumn();
                        I.TableAlias = Table;
                        I.TableName = Table;
                        I.ColumnName = Column;
                        I.Content = Value;
                        this._ImportColumnsAddOn.Add(I);
                        this._ImportColumnsAll = null;
                        ValueIsSet = true;
                    }
                    if (!ValueIsSet)
                    {
                        foreach (DiversityCollection.ImportColumn I in this._ImportColumnsAddOn)
                        {
                            if (I.ColumnName == Column
                                && I.TableAlias == Alias
                                && I.TableName == Table
                                && I.Content != "")
                            {
                                ValueIsSet = true;
                                break;
                            }
                        }
                        if (!ValueIsSet)
                        {
                            // the value is still not set
                            DiversityCollection.ImportColumn I = new ImportColumn();
                            I.Content = Value;
                            I.ColumnName = Column;
                            I.TableAlias = Alias;
                            I.TableName = Table;
                            this._ImportColumnsAddOn.Add(I);
                            this._ImportColumnsAll = null;
                        }
                    }
                }
            }
            if (this.insertHostRelation)
            {
                DiversityCollection.ImportColumn I = new ImportColumn();
                I.Content = "";
                I.ColumnName = "RelatedUnitID";
                I.TableAlias = this.TableOfParasite;
                I.TableName = "IdentificationUnit";
                this._ImportColumnsAddOn.Add(I);
                this._ImportColumnsAll = null;
            }
            if (this.checkBoxTaxonAnywhere.Checked)
            {
                // only if the column StorageLocation is not allready present and the column TaxonomicName is present
                bool StorageLocationPresent = false;
                bool TaxonomicNamePresent = false;
                if (!this.ColumnList.Contains("StorageLocation")
                    && !this.ColumnListAddOn.Contains("StorageLocation")
                    && (this.ColumnList.Contains("TaxonomicName") 
                    || this.ColumnListAddOn.Contains("TaxonomicName")))
                {
                    Alias = "CollectionSpecimenPart";
                    Table = "CollectionSpecimenPart";
                    Column = "StorageLocation";
                    Value = "";
                    // find the alias
                    if (this.AliasDictionary.ContainsValue(Alias))
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                        {
                            if (KV.Value == Alias)
                            {
                                Alias = KV.Key;
                                break;
                            }
                        }
                    }
                    DiversityCollection.ImportColumn I = new ImportColumn();
                    I.TableAlias = Alias;
                    I.TableName = Table;
                    I.ColumnName = Column;
                    I.Content = Value;
                    this._ImportColumnsAddOn.Add(I);
                    this._ImportColumnsAll = null;
                }
            }
            if (this.ColumnListAddOn.Contains("IdentificationSequence") && !this.ColumnList.Contains("IdentificationSequence"))
            {
                if (this.listBoxIdentificationsUnit1.Items.Count > 1
                    || this.listBoxIdentificationsUnit2.Items.Count > 1)
                {
                }
                else
                {
                    for (int i = 0; i < this._ImportColumnsAddOn.Count; i++)
                    {
                        if (this._ImportColumnsAddOn[i].ColumnName == "IdentificationSequence")
                        {
                            DiversityCollection.ImportColumn I = this._ImportColumnsAddOn[i];
                            I.Content = "1";
                            this._ImportColumnsAddOn[i] = I;
                        }
                    }
                }
            }
            System.Collections.Generic.List<string> UnitAliasTables = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
            {
                if (KV.Value == "IdentificationUnit")
                {
                    if (!UnitAliasTables.Contains(KV.Key)) UnitAliasTables.Add(KV.Key);
                }
            }
            if (UnitAliasTables.Count > 0)
            {
                foreach (string s in UnitAliasTables)
                {
                    bool LastIdentificationUnitCacheIsMissing = true;
                    for (int i = 0; i < this.ColumnListAll.Count; i++)
                    {
                        if (this.ImportColumnsAll[i].ColumnName == "LastIdentificationCache"
                            && this.ImportColumnsAll[i].TableName == "IdentificationUnit"
                            && this.ImportColumnsAll[i].TableAlias == s)
                        {
                            LastIdentificationUnitCacheIsMissing = false;
                            break;
                        }
                    }
                    if (LastIdentificationUnitCacheIsMissing)
                    {
                        DiversityCollection.ImportColumn I = new ImportColumn();
                        I.TableAlias = s;
                        I.TableName = "IdentificationUnit";
                        I.ColumnName = "LastIdentificationCache";
                        this._ImportColumnsAddOn.Add(I);
                        this._ImportColumnsAll = null;
                    }
                }
            }
        }

        private void WriteTaxonInStorageLocation()
        {
            if (this.ColumnListAddOn.Contains("StorageLocation") && this.ColumnList.Contains("TaxonomicName"))
            {
                string AliasMainTableIdentification = this.AliasOfIdentificationTableOfMainUnit;
                int iTaxon = 0;
                bool TaxonPresent = false;
                for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                {
                    if (this.ImportColumnsAll[i].TableAlias == AliasMainTableIdentification
                        && this.ImportColumnsAll[i].TableName == "Identification"
                        && this.ImportColumnsAll[i].ColumnName == "TaxonomicName")
                    {
                        iTaxon = i;
                        TaxonPresent = true;
                        break;
                    }
                }
                if (TaxonPresent)
                {
                    string Taxon = this._DatasetList[this._DatasetPosition][0][iTaxon];
                    //string Taxon = this._Dataset[0][iTaxon];
                    if (Taxon.Length > 0)
                    {
                        int iStorageLocation = this.ColumnListAddOn.IndexOf("StorageLocation");
                        iStorageLocation = iStorageLocation + this._ImportColumns.Count;
                        foreach (System.Collections.Generic.List<string> L in this._DatasetList[this._DatasetPosition])
                        {
                            L[iStorageLocation] = Taxon;
                        }
                        //foreach (System.Collections.Generic.List<string> L in this._Dataset)
                        //{
                        //    L[iStorageLocation] = Taxon;
                        //}
                    }
                }
            }
        }

        private void WriteTaxonInLastIdentificationCache()
        {
            if (this.ColumnListAddOn.Contains("LastIdentificationCache") && this.ColumnList.Contains("TaxonomicName"))
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV_Parent in this.ParentUnitAlias)
                {
                    //foreach (System.Collections.Generic.List<string> L in this._Dataset)
                    foreach (System.Collections.Generic.List<string> L in this._DatasetList[this._DatasetPosition])
                    {
                        string TaxonomicName = "";
                        for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                        {
                            if (this.ImportColumnsAll[i].TableAlias == KV_Parent.Key
                                && this.ImportColumnsAll[i].TableName == "Identification"
                                && this.ImportColumnsAll[i].ColumnName == "TaxonomicName")
                            {
                                TaxonomicName = L[i];
                                break;
                            }
                        }
                        if (TaxonomicName.Length > 0)
                        {
                            for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                            {
                                if (this.ImportColumnsAll[i].TableAlias == KV_Parent.Value
                                    && this.ImportColumnsAll[i].TableName == "IdentificationUnit"
                                    && this.ImportColumnsAll[i].ColumnName == "LastIdentificationCache")
                                {
                                    L[i] = TaxonomicName;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void WriteIdentificationUnitIDInDependentTables()
        {
            if (this.ColumnList.Contains("IdentificationUnitID") && !this._IsReimport)
            {
                string TaxonomicName = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV_Parent in this.ParentUnitAlias)
                {
                    for (int iL = 0; iL < this._DatasetList[this._DatasetPosition].Count; iL++)
                    {
                        string IdentificationUnitID = this.getUnitValueFromData(KV_Parent.Key, "IdentificationUnitID", iL);
                        for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                        {
                            if (this.ImportColumnsAll[i].TableAlias == KV_Parent.Key
                                && this.ImportColumnsAll[i].ColumnName == "IdentificationUnitID"
                                && this._DatasetList[this._DatasetPosition][iL][i].Length == 0)
                            {
                                this._DatasetList[this._DatasetPosition][iL][i] = IdentificationUnitID;
                            }
                        }
                    }
                    //for (int iL = 0; iL < this._Dataset.Count; iL++)
                    //{
                    //    string IdentificationUnitID = this.getValueFromData(KV_Parent.Key, "IdentificationUnitID", iL);
                    //    for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                    //    {
                    //        if (this.ImportColumnsAll[i].TableAlias == KV_Parent.Key
                    //            && this.ImportColumnsAll[i].ColumnName == "IdentificationUnitID"
                    //            && this._Dataset[iL][i].Length == 0)
                    //        {
                    //            this._Dataset[iL][i] = IdentificationUnitID;
                    //        }
                    //    }
                    //}
                }
            }

        }
        
        private string getValueFromData(string Column, int Position)
        {
            string Value = "";
            for (int i = 0; i < this.ColumnList.Count; i++)
            {
                if (this.ColumnList[i] == Column
                    && this.Dataset()[Position][i].Length > 0)
                {
                    Value = this.Dataset()[Position][i];
                    break;
                }
            }
            return Value;
        }

        private string getUnitValueFromData(string AliasChild, string Column, int Position)
        {
            string Value = "";
            try
            {
                string AliasParent = this.ParentUnitAlias[AliasChild];
                for (int i = 0; i < this._ImportColumns.Count; i++)
                {
                    if (this._ImportColumns[i].TableAlias == AliasParent
                        && this._ImportColumns[i].ColumnName == Column
                        && this.Dataset()[Position][i].Length > 0)
                    {
                        Value = this.Dataset()[Position][i];
                        break;
                    }
                }
            }
            catch 
            {
            } 
            return Value;
        }

        private void setPresetControlVisibility()
        {
            if (!this._IsReimport)
            {
                bool Enabled = true;
                if (this.ColumnList.Contains("CountryCache"))
                {
                    Enabled = false;
                    this.comboBoxCountry.SelectedIndex = -1;
                    this.userControlModuleRelatedEntryGazetteer.textBoxValue.Text = "";
                    this.userControlModuleRelatedEntryGazetteer.labelURI.Text = "";
                }
                this.tableLayoutPanelPlaceAndCountry.Enabled = Enabled;

                if (this.TableList.Contains("CollectionAgent"))
                {
                    Enabled = false;
                    this.userControlModuleRelatedEntryCollector.textBoxValue.Text = "";
                    this.userControlModuleRelatedEntryCollector.labelURI.Text = "";
                }
                else Enabled = true;
                this.tableLayoutPanelCollector.Enabled = Enabled;

                if (this.ColumnList.Contains("CollectionDate")
                    || this.ColumnList.Contains("CollectionDay")
                    || this.ColumnList.Contains("CollectionMonth")
                    || this.ColumnList.Contains("CollectionYear")
                    || this.ColumnList.Contains("CollectionDateSupplement")
                    || this.ColumnList.Contains("CollectionTime")
                    || this.ColumnList.Contains("CollectionTimeSpan"))
                {
                    Enabled = false;
                    this.userControlDatePanelCollectionDate.maskedTextBoxDay.Text = "";
                    this.userControlDatePanelCollectionDate.maskedTextBoxMonth.Text = "";
                    this.userControlDatePanelCollectionDate.maskedTextBoxYear.Text = "";
                    this.userControlDatePanelCollectionDate.textBoxSupplement.Text = "";
                }
                else Enabled = true;
                this.tableLayoutPanelEventDate.Enabled = Enabled;

                if (!this.TableList.Contains("CollectionSpecimenImage")
                 || !this.ColumnList.Contains("URI"))
                {
                    Enabled = false;
                }
                else Enabled = true;
                this.labelImageType.Enabled = Enabled;
                this.comboBoxImageType.Enabled = Enabled;

                // Parts
                bool ContainsSecondPart = false;
                string PartTable = "";
                System.Collections.Generic.List<string> PartTables = new List<string>();
                foreach (DiversityCollection.ImportColumn IC in this._ImportColumns)
                {
                    if (IC.TableName == "CollectionSpecimenPart" && !PartTables.Contains(IC.TableAlias))
                    {
                        PartTables.Add(IC.TableAlias);
                    }
                }
                if (PartTables.Count > 1)
                {
                    ContainsSecondPart = true;
                    this.radioButtonTwoParts.Checked = true;
                    //this.PartsInData = _PartsInData.two;
                }

                //foreach (DiversityCollection.ImportColumn IC in this._ImportColumns)
                //{
                //    if (IC.TableName == "CollectionSpecimenPart" && PartTable.Length == 0)
                //    {
                //        PartTable = IC.TableAlias;
                //        this.labelPart1.Text = IC.TableAlias;
                //    }
                //    else if (IC.TableName == "" && PartTable.Length > 0 && IC.TableAlias != PartTable)
                //    {
                //        ContainsSecondPart = true;
                //        this.labelPart2.Text = IC.TableAlias;
                //        break;
                //    }
                //}
                if (PartTables.Count > 1)
                    this.labelPart2.Text = PartTables[1];
                if (PartTables.Count > 0)
                    this.labelPart1.Text = PartTables[0];
                //this.labelPart2.Visible = ContainsSecondPart;
                //this.labelCollection2.Visible = ContainsSecondPart;
                //this.comboBoxCollection2.Visible = ContainsSecondPart;
                //this.labelDerivedFrom2.Visible = ContainsSecondPart;
                //this.labelMaterialCategory2.Visible = ContainsSecondPart;
                //this.comboBoxMaterialCategory2.Visible = ContainsSecondPart;
                //this.comboBoxDerivedFrom2.Visible = ContainsSecondPart;
                //this.comboBoxDerivedFrom.Visible = ContainsSecondPart;
                //this.labelDerivedFrom.Visible = ContainsSecondPart;
                //this.userControlHierarchySelectorCollection2.Visible = ContainsSecondPart;
                //if (ContainsSecondPart)
                //{
                //    this.comboBoxDerivedFrom2.Items.Add(PartTables[0]);
                //    //this.comboBoxDerivedFrom.Items.Add(PartTables[1]);
                //}
                //else
                //{
                //    this.comboBoxDerivedFrom2.Items.Clear();
                //    //this.comboBoxDerivedFrom.Items.Clear();
                //}
            }
            else
            {
                //this.tabPageFile.BringToFront();
                //this.tabControlPresettings.Enabled = true;
            }
        }
        #endregion

        #region Event

        private void addMissingEventSeriesTable()
        {
            if (this.checkBoxEventSeries.Checked)
            {
                DiversityCollection.ImportColumn IEventSeriesID = new ImportColumn();
                IEventSeriesID.TableAlias = "CollectionEvent";
                IEventSeriesID.TableName = "CollectionEvent";
                IEventSeriesID.ColumnName = "SeriesID";
                IEventSeriesID.Content = "";
                this._ImportColumnsAddOn.Add(IEventSeriesID);
            }

            ////if (!this.AliasDictionary.ContainsValue("CollectionEventSeries")
            ////    && this.checkBoxEventSeries.Checked)
            //{
            //    DiversityCollection.ImportColumn IEventSeriesID = new ImportColumn();
            //    IEventSeriesID.TableAlias = "CollectionEventSeries";
            //    IEventSeriesID.TableName = "CollectionEventSeries";
            //    IEventSeriesID.ColumnName = "SeriesID";
            //    IEventSeriesID.Content = "";
            //    this._ImportColumnsAddOn.Add(IEventSeriesID);
            //    DiversityCollection.ImportColumn IEventSeriesCode = new ImportColumn();
            //    IEventSeriesCode.TableAlias = "CollectionEventSeries";
            //    IEventSeriesCode.TableName = "CollectionEventSeries";
            //    IEventSeriesCode.ColumnName = "SeriesCode";
            //    IEventSeriesCode.Content = this.textBoxEventSeriesCode.Text;
            //    this._ImportColumnsAddOn.Add(IEventSeriesCode);
            //    DiversityCollection.ImportColumn IEventSeriesDescription = new ImportColumn();
            //    IEventSeriesDescription.TableAlias = "CollectionEventSeries";
            //    IEventSeriesDescription.TableName = "CollectionEventSeries";
            //    IEventSeriesDescription.ColumnName = "Description";
            //    IEventSeriesDescription.Content = this.textBoxEventSeriesDescription.Text;
            //    this._ImportColumnsAddOn.Add(IEventSeriesDescription);
            //    DiversityCollection.ImportColumn IEventSeriesNotes = new ImportColumn();
            //    IEventSeriesNotes.TableAlias = "CollectionEventSeries";
            //    IEventSeriesNotes.TableName = "CollectionEventSeries";
            //    IEventSeriesNotes.ColumnName = "Notes";
            //    IEventSeriesNotes.Content = this.textBoxEventSeriesNotes.Text;
            //    this._ImportColumnsAddOn.Add(IEventSeriesNotes);
            //}
        }


        private bool EventDataInFile
        {
            get
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                {
                    if (KV.Value.StartsWith("CollectionEvent"))
                        return true;
                }
                return false;
            }
        }

        private void addMissingEventTable()
        {
            if ((this.AliasDictionary.ContainsValue("CollectionEventProperty")
                || this.AliasDictionary.ContainsValue("CollectionEventImage")
                || this.AliasDictionary.ContainsValue("CollectionEventLocalisation"))
                && !this.AliasDictionary.ContainsValue("CollectionEvent"))
            {
                DiversityCollection.ImportColumn IEvent = new ImportColumn();
                IEvent.TableAlias = "CollectionEvent";
                IEvent.TableName = "CollectionEvent";
                IEvent.ColumnName = "CollectionEventID";
                IEvent.Content = "";

                this.addMissingPkColumns();
            }
            if (this.AliasDictionary.ContainsValue("CollectionEvent"))
            {
                bool EventIdInSpecimen = false;
                string AliasSpecimen = this.AliasCollectionSpecimen;
                foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll )
                {
                    if (I.ColumnName == "CollectionEventID"
                        && I.TableAlias == AliasSpecimen)
                        EventIdInSpecimen = true;
                }
                if (!EventIdInSpecimen && AliasSpecimen.Length > 0)
                {
                    DiversityCollection.ImportColumn IE = new ImportColumn();
                    IE.TableAlias = AliasSpecimen;
                    IE.TableName = "CollectionSpecimen";
                    IE.ColumnName = "CollectionEventID";
                    IE.Content = "";
                    this._ImportColumnsAddOn.Add(IE);
                    this._ImportColumnsAll = null;
                }
            }

        }
        
        #endregion

        #region Localisations

        private void AnalyseLocalisation()
        {
            bool ResetLocSys = false;
            System.Collections.Generic.List<System.Collections.Generic.List<string>> LocSysList = new List<List<string>>();
            if (this.TableList.Contains("CollectionEventLocalisation"))
            {
                for (int i = 0; i < this._ImportColumns.Count; i++)
                {
                    // check for columns for a localisation (L1, L2 or L1 and L2)
                    if (this._ImportColumns[i].TableAlias == "CollectionEventLocalisation"
                        && (this._ImportColumns[i].ColumnName == "Location1"
                        || (this._ImportColumns.Count > i && this._ImportColumns[i].ColumnName == "Location1" && this._ImportColumns[i + 1].ColumnName == "Location2")
                        || (i > 0 && this._ImportColumns[i].ColumnName == "Location2" && this._ImportColumns[i - 1].ColumnName != "Location1"))
                        && this.LocalisationSystemIdIsMissing(this._ImportColumns[i].TableAlias))
                    {
                        System.Collections.Generic.List<string> L = new List<string>();
                        L.Add(this._ImportColumns[i].TableAlias);
                        L.Add(this._ImportColumns[i].ColumnName);
                        L.Add(this.Dataset()[0][i]);
                        LocSysList.Add(L);
                    }
                }
                // if the number of localisations does not correspond with the panels, reset the panels
                if (LocSysList.Count != this._LocalisationPanelList.Count) ResetLocSys = true;
                else
                {
                    // if the number of panels correspond to the localisations, check the aliases and the contents
                    for (int i = 0; i < LocSysList.Count; i++)
                    {
                        // Check if the alias of the panel is the same as the new one
                        if (LocSysList[i][0] != this._LocalisationPanelList[i].Name)
                        {
                            ResetLocSys = true;
                            break;
                        }
                        System.Collections.Generic.List<System.Windows.Forms.TextBox> TextBoxesInPanel = new List<TextBox>();
                        foreach(System.Windows.Forms.Control C in this._LocalisationPanelList[i].Controls)
                        {
                            if (C.GetType() == typeof(System.Windows.Forms.TextBox) && C.Name.StartsWith("Location"))
                                TextBoxesInPanel.Add((System.Windows.Forms.TextBox)C);
                        }
                        // check if the number of columns is the same as the number of textboxes
                        if (TextBoxesInPanel.Count != LocSysList[i].Count - 1)
                        {
                            ResetLocSys = true;
                            break;
                        }
                        // check if the names of the text boxes correspond to the columns
                        for (int ii = 1; ii < LocSysList.Count; ii++)
                        {
                            if (LocSysList[ii][1] != TextBoxesInPanel[ii - 1].Name)
                            {
                                ResetLocSys = true;
                                break;
                            }
                        }
                        if (ResetLocSys) break;
                    }
                }
            }
            if (ResetLocSys)
                this.ResetLocalisationSystemPanels();
        }

        private string LocalisationSystemIDforAlias(string Alias)
        {
            string LocalisationSystemID = "";
            if (this._LocalisationPanelList.Count > 0)
            {
                foreach (System.Windows.Forms.Panel P in this._LocalisationPanelList)
                {
                    if (P.Name == Alias)
                    {
                        foreach (System.Windows.Forms.Control C in P.Controls)
                        {
                            if (C.GetType() == typeof(System.Windows.Forms.ComboBox) && C.Name.StartsWith("comboBoxLocalisationSystem"))
                            {
                                System.Windows.Forms.ComboBox CB = (System.Windows.Forms.ComboBox)C;
                                int LocSysID = 0;
                                if (int.TryParse(CB.SelectedValue.ToString(), out LocSysID))
                                {
                                    LocalisationSystemID = LocSysID.ToString();
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
            return LocalisationSystemID;
        }

        private void ResetLocalisationSystemPanels()
        {
            foreach (System.Windows.Forms.Panel P in this._LocalisationPanelList)
            {
                this.tabPageEvent.Controls.Remove(P);
            }
            this._LocalisationPanelList.Clear();
            this._LocalisationSystemIDComboBoxes.Clear();
            if (this.TableList.Contains("CollectionEventLocalisation"))
            {
                for (int i = 0; i < this._ImportColumns.Count; i++)
                {
                    if (this._ImportColumns[i].TableName == "CollectionEventLocalisation"
                        && this._ImportColumns[i].ColumnName == "Location1"
                        && this.LocalisationSystemIdIsMissing(this._ImportColumns[i].TableAlias))
                    {
                        bool ImportColumnPresent = false;
                        foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                        {
                            if (I.TableName == "CollectionEventLocalisation"
                                && I.ColumnName == "LocalisationSystemID"
                                && I.TableAlias == this._ImportColumns[i].TableAlias)
                            {
                                ImportColumnPresent = true;
                                break;
                            }
                        }
                        if (!ImportColumnPresent)
                        {
                            DiversityCollection.ImportColumn ILocSys1 = new ImportColumn();
                            ILocSys1.TableAlias = this._ImportColumns[i].TableAlias;
                            ILocSys1.TableName = "CollectionEventLocalisation";
                            ILocSys1.ColumnName = "LocalisationSystemID";
                            this._ImportColumnsAddOn.Add(ILocSys1);
                            this._ImportColumnsAll = null;
                            System.Collections.Generic.Dictionary<string, string> ColumnContents = new Dictionary<string, string>();
                            ColumnContents.Add("Location1", this.Dataset()[0][i]);
                            if (this._ImportColumns.Count > i
                                && this._ImportColumns[i + 1].TableName == "CollectionEventLocalisation"
                                && this._ImportColumns[i + 1].ColumnName == "Location2")
                                ColumnContents.Add("Location2", this.Dataset()[0][i + 1]);
                            this.AddLocalisation(this._ImportColumns[i].TableAlias, ColumnContents);
                        }
                    }
                }
            }
        }

        private bool LocalisationPanelsMatchData
        {
            get
            {
                bool LocalisationPanelsOK = true;
                System.Collections.Generic.List<string> LocalisationTables = new List<string>();
                foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                {
                    if (I.TableName == "CollectionEventLocalisation")
                    {
                        if (!LocalisationTables.Contains(I.TableAlias)) LocalisationTables.Add(I.TableAlias);
                    }
                }
                if (this._LocalisationPanelList.Count > 0 && LocalisationTables.Count != this._LocalisationPanelList.Count)
                {
                    string Message = "The aliases for do not match the choosen localisation systems \r\n\r\n Localisation systems:\r\n";
                    foreach (System.Windows.Forms.Panel P in this._LocalisationPanelList)
                    {
                        Message += P.Name + ", ";
                    }
                    Message += "\r\n\r\nTables:\r\n";
                    foreach (string s in LocalisationTables)
                    {
                        Message += s + ", ";
                    }
                    System.Windows.Forms.MessageBox.Show(Message);
                    return false;
                }
                else
                {
                    System.Collections.Generic.List<string> PanelLocAliasList = new List<string>();
                    foreach (System.Windows.Forms.Panel P in this._LocalisationPanelList)
                    {
                        PanelLocAliasList.Add(P.Name);
                    }
                    foreach (string s in PanelLocAliasList)
                    {
                        if (!LocalisationTables.Contains(s))
                        {
                            return false;
                        }
                    }
                    if (LocalisationTables.Count != this._LocalisationPanelList.Count)
                        return false;
                }
                return LocalisationPanelsOK;
            }
        }

        private bool LocalisationSystemIdsInPanelsAreSet
        {
            get
            {
                bool IdsSet = true;
                if (this._LocalisationSystemIDComboBoxes.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<System.Windows.Forms.ComboBox, string> KV in this._LocalisationSystemIDComboBoxes)
                    {
                        if (KV.Key.SelectedIndex == -1 || KV.Key.SelectedValue == null)
                        {
                            return false;
                        }
                    }
                }
                return IdsSet;
            }
        }

        private bool LocalisationSystemIdIsSet
        {
            get
            {
                bool LocalisationPanelsOK = true;
                System.Collections.Generic.List<string> LocalisationTables = new List<string>();
                foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                {
                    if (I.TableName == "CollectionEventLocalisation")
                    {
                        if (!LocalisationTables.Contains(I.TableAlias)) LocalisationTables.Add(I.TableAlias);
                    }
                }
                if (LocalisationTables.Count != this._LocalisationPanelList.Count)
                    LocalisationPanelsOK = false;
                else
                {
                    System.Collections.Generic.List<string> PanelLocAliasList = new List<string>();
                    foreach (System.Windows.Forms.Panel P in this._LocalisationPanelList)
                    {
                        PanelLocAliasList.Add(P.Name);
                    }
                    foreach (string s in PanelLocAliasList)
                    {
                        if (!LocalisationTables.Contains(s))
                        {
                            LocalisationPanelsOK = false;
                            break;
                        }
                    }
                }
                if (!LocalisationPanelsOK)
                {
                    this.ResetLocalisationSystemPanels();
                    System.Windows.Forms.MessageBox.Show("Please set the localisation systems");
                    this.tabControlPresettings.SelectTab(this.tabPageEvent);
                    return false;
                }
                if (!LocalisationPanelsOK /* && UnsetLoc > 0*/)
                {
                    System.Windows.Forms.MessageBox.Show("Please set the localisation systems");
                    this.tabControlPresettings.SelectTab(this.tabPageEvent);
                }
                return LocalisationPanelsOK;
            }
        }

        private bool LocalisationSystemIdIsMissing(string TableAlias)
        {
            bool IsMissing = true;
            for (int i = 0; i < this._ImportColumns.Count; i++)
            {
                if (this._ImportColumns[i].TableAlias == TableAlias
                    && this._ImportColumns[i].ColumnName == "LocalisationSystemID"
                    && this.Dataset()[0][i] != "")
                {
                    IsMissing = false;
                    break;
                }
            }
            return IsMissing;
        }

        private void AddLocalisation(string TableAlias, System.Collections.Generic.Dictionary<string, string> ColumnContents)
        {
            DiversityCollection.Datasets.DataSetCollectionSpecimen ds = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            System.Windows.Forms.Panel PanelLoc = new Panel();
            PanelLoc.Name = TableAlias;
            PanelLoc.Width = this.tabPageEvent.Width;
            this._LocalisationPanelList.Add(PanelLoc);
            PanelLoc.SuspendLayout();

            int Position = 0;

            System.Windows.Forms.Label labelLocalisationSystem = new Label();
            PanelLoc.Controls.Add(labelLocalisationSystem);
            labelLocalisationSystem.Dock = System.Windows.Forms.DockStyle.Left;
            labelLocalisationSystem.Name = "labelLocalisationSystem";
            labelLocalisationSystem.Width = 140;
            labelLocalisationSystem.Location = new System.Drawing.Point(Position, 0);
            //labelLocalisationSystem.Size = new System.Drawing.Size(200, 21);
            labelLocalisationSystem.Text = "Localisation " + TableAlias + ":";
            labelLocalisationSystem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            Position += labelLocalisationSystem.Width;

            System.Windows.Forms.ComboBox comboBoxLocalisationSystem = new ComboBox();
            PanelLoc.Controls.Add(comboBoxLocalisationSystem);
            comboBoxLocalisationSystem.Location = new System.Drawing.Point(Position, 0);
            comboBoxLocalisationSystem.Dock = DockStyle.Right;
            comboBoxLocalisationSystem.Name = "comboBoxLocalisationSystem";
            comboBoxLocalisationSystem.Width = 140;
            comboBoxLocalisationSystem.MaxDropDownItems = 20;
            //comboBoxLocalisationSystem.Size = new System.Drawing.Size(300, 21);
            comboBoxLocalisationSystem.DataSource = DiversityCollection.LookupTable.DtLocalisationSystemNew;
            comboBoxLocalisationSystem.DisplayMember = "LocalisationSystemName";
            comboBoxLocalisationSystem.ValueMember = "LocalisationSystemID";
            comboBoxLocalisationSystem.SelectedIndexChanged += new System.EventHandler(this.comboBoxLoc_SelectedIndexChanged);
            comboBoxLocalisationSystem.SelectedIndex = -1;
            comboBoxLocalisationSystem.Tag = ds.CollectionEventLocalisation.LocalisationSystemIDColumn;
            this._PresetControls.Add(comboBoxLocalisationSystem);
            this._LocalisationSystemIDComboBoxes.Add(comboBoxLocalisationSystem, TableAlias);

            Position += comboBoxLocalisationSystem.Width;

            System.Windows.Forms.Label labelLocation1 = new Label();
            labelLocation1.Name = "labelLocation1";
            labelLocation1.Text = "Location1:";
            System.Windows.Forms.TextBox textBoxLocation1 = new TextBox();
            textBoxLocation1.ReadOnly = true;
            textBoxLocation1.Name = "textboxLocation1";
            if (ColumnContents.ContainsKey("Location1"))
            {
                PanelLoc.Controls.Add(labelLocation1);
                labelLocation1.Dock = DockStyle.Right;
                labelLocation1.Width = 140;
                //labelLocation1.Width = "Location1".Length * (int)labelLocation1.Font.SizeInPoints / 2;
                labelLocation1.Location = new System.Drawing.Point(Position, 0);
                labelLocation1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

                Position += labelLocation1.Width;

                PanelLoc.Controls.Add(textBoxLocation1);
                textBoxLocation1.Dock = DockStyle.Right;
                textBoxLocation1.Text = ColumnContents["Location1"];
                //textBoxLocation1.Width = ColumnContents["Location1"].Length * (int)textBoxLocation1.Font.SizeInPoints/2;
                textBoxLocation1.Width = 200;
                textBoxLocation1.Dock = DockStyle.Right;
                textBoxLocation1.Location = new System.Drawing.Point(Position, 0);

                Position += textBoxLocation1.Width;
            }

            System.Windows.Forms.Label labelLocation2 = new Label();
            labelLocation2.Name = "labelLocation2";
            labelLocation2.Text = "Location2:";
            System.Windows.Forms.TextBox textBoxLocation2 = new TextBox();
            textBoxLocation2.ReadOnly = true;
            textBoxLocation2.Name = "textboxLocation2";
            if (ColumnContents.ContainsKey("Location2"))
            {
                PanelLoc.Controls.Add(labelLocation2);
                labelLocation2.Dock = DockStyle.Right;
                labelLocation2.Width = 140;
                //labelLocation2.Width = "Location2".Length * (int)labelLocation2.Font.SizeInPoints / 2;
                labelLocation2.Location = new System.Drawing.Point(Position, 0);
                labelLocation2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

                Position += labelLocation2.Width;
                
                PanelLoc.Controls.Add(textBoxLocation2);
                textBoxLocation2.Dock = DockStyle.Right;
                textBoxLocation2.Text = ColumnContents["Location2"];
                //textBoxLocation2.Width = ColumnContents["Location2"].Length * (int)textBoxLocation2.Font.SizeInPoints / 2;
                textBoxLocation2.Width = 200;
                textBoxLocation2.Dock = DockStyle.Right;
                textBoxLocation2.Location = new System.Drawing.Point(Position, 0);
            }

            //comboBoxLocalisationSystem.Dock = DockStyle.Fill;
            comboBoxLocalisationSystem.BringToFront();
            this.tabPageEvent.Controls.Add(PanelLoc);
            PanelLoc.BringToFront();
            PanelLoc.Dock = System.Windows.Forms.DockStyle.Top;
            //PanelLoc.Location = new System.Drawing.Point(3, 103);
            //PanelLoc.Name = "PanelLoc";
            PanelLoc.Height = 23;
            PanelLoc.Padding = new Padding(0, 1, 0, 1);
            PanelLoc.Visible = true;
            comboBoxLocalisationSystem.SelectedIndex = -1;
            //comboBoxLocalisationSystem.SelectedValue = null;
            comboBoxLocalisationSystem.SelectedItem = null;
            comboBoxLocalisationSystem.SelectedText = null;
        }

        #region Combobox events
        private void comboBoxLoc_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ComboBox C = (System.Windows.Forms.ComboBox)sender;
                System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)C.Parent;
                System.Collections.Generic.List<System.Windows.Forms.TextBox> TextBoxList = new List<TextBox>();
                System.Collections.Generic.List<System.Windows.Forms.Label> LabelList = new List<Label>();
                foreach (System.Windows.Forms.Control L in P.Controls)
                {
                    if (L.GetType() == typeof(System.Windows.Forms.Label) && L.Name.StartsWith("labelLocation"))
                        LabelList.Add((System.Windows.Forms.Label)L);
                }
                int LocSysID = 0;
                if (C.SelectedValue != null)
                {
                    if (int.TryParse(C.SelectedValue.ToString(), out LocSysID))
                    {
                        System.Data.DataRow[] r1 = DiversityCollection.LookupTable.DtLocationColumns(LocSysID).Select("ColumnName = 'Location1'");
                        System.Data.DataRow[] r2 = DiversityCollection.LookupTable.DtLocationColumns(LocSysID).Select("ColumnName = 'Location2'");
                        foreach (System.Windows.Forms.Label L in LabelList)
                        {
                            if (L.Name == "labelLocation1") L.Text = r1[0]["DisplayText"].ToString() + ":";
                            if (L.Name == "labelLocation2") L.Text = r2[0]["DisplayText"].ToString() + ":";
                        }
                    }
                }
                string Alias = P.Name;
                for (int i = 0; i < this._ImportColumnsAddOn.Count; i++)
                {
                    if (this._ImportColumnsAddOn[i].TableAlias == Alias
                        && this._ImportColumnsAddOn[i].TableName == "CollectionEventLocalisation"
                        && this._ImportColumnsAddOn[i].ColumnName == "LocalisationSystemID")
                    {
                        DiversityCollection.ImportColumn I = this._ImportColumnsAddOn[i];
                        I.Content =  LocSysID.ToString();
                        this._ImportColumnsAddOn[i] = I;
                        this.treeViewAnalysis.Nodes.Clear();
                        //this.analyseData(0);
                    }
                }
                //for (int i = 0; i < this._ContentListAddOn.Count; i++)
                //{
                //    if (this._AliasListAddOn[i] == Alias
                //        && this._TableListAddOn[i] == "CollectionEventLocalisation"
                //        && this._ColumnListAddOn[i] == "LocalisationSystemID")
                //    {
                //        this._ContentListAddOn[i] = LocSysID.ToString();
                //        this.treeViewAnalysis.Nodes.Clear();
                //        //this.analyseData(0);
                //    }
                //}
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxLoc_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ComboBox C = (System.Windows.Forms.ComboBox)sender;
                System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)C.Parent;
                System.Collections.Generic.List<System.Windows.Forms.ComboBox> ComboBoxList = (System.Collections.Generic.List<System.Windows.Forms.ComboBox>)P.Tag;
                int LocSysID = 0;
                if (int.TryParse(C.SelectedValue.ToString(), out LocSysID))
                {
                    ComboBoxList[1].DataSource = DiversityCollection.LookupTable.DtLocationColumns(LocSysID);
                    ComboBoxList[1].DisplayMember = DiversityCollection.LookupTable.DtLocationColumns(LocSysID).Columns[1].ColumnName;
                    ComboBoxList[1].ValueMember = DiversityCollection.LookupTable.DtLocationColumns(LocSysID).Columns[0].ColumnName;
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxLocCol_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.ComboBox C = (System.Windows.Forms.ComboBox)sender;
                if (C.SelectedIndex > -1 && C.ValueMember != "")
                {
                    System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)C.Parent;
                    foreach (System.Windows.Forms.Control L in P.Controls)
                    {
                        if (L.GetType() == typeof(System.Windows.Forms.Label) && L.Name == "labelLocCol")
                            L.Text = C.SelectedValue.ToString() + ":";
                    }
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        
        #endregion        

        #endregion

        #region Units

        private void addMissingUnitTables()
        {
            if (this._ImportColumnsAddOn == null) this._ImportColumnsAddOn = new List<ImportColumn>();
            //Unit
            if ((this.AliasDictionary.ContainsValue("Identification")
                || this.AliasDictionary.ContainsValue("IdentificationUnitAnalysis")
                || this.AliasDictionary.ContainsValue("IdentificationUnitInPart"))
                && !this.AliasDictionary.ContainsValue("IdentificationUnit"))
            {
                if (this.ManyUnitTables)
                {
                    DiversityCollection.ImportColumn Iid = new ImportColumn();
                    Iid.TableAlias = "IdentificationUnit_1";
                    Iid.TableName = "IdentificationUnit";
                    Iid.ColumnName = "CollectionSpecimenID";
                    Iid.Content = "";
                    this._ImportColumnsAddOn.Add(Iid);

                    DiversityCollection.ImportColumn Iuid = new ImportColumn();
                    Iuid.TableAlias = "IdentificationUnit_1";
                    Iuid.TableName = "IdentificationUnit";
                    Iuid.ColumnName = "IdentificationUnitID";
                    Iuid.Content = "";
                    this._ImportColumnsAddOn.Add(Iuid);

                    DiversityCollection.ImportColumn Ilic = new ImportColumn();
                    Ilic.TableAlias = "IdentificationUnit_1";
                    Ilic.TableName = "IdentificationUnit";
                    Ilic.ColumnName = "LastIdentificationCache";
                    Ilic.Content = "";
                    this._ImportColumnsAddOn.Add(Ilic);

                    DiversityCollection.ImportColumn Ido = new ImportColumn();
                    Ido.TableAlias = "IdentificationUnit_1";
                    Ido.TableName = "IdentificationUnit";
                    Ido.ColumnName = "DisplayOrder";
                    if (this.radioButtonMain1.Checked) Ido.Content = "1";
                    else Ido.Content = "2";
                    this._ImportColumnsAddOn.Add(Ido);

                    DiversityCollection.ImportColumn Itg = new ImportColumn();
                    Itg.TableAlias = "IdentificationUnit_1";
                    Itg.TableName = "IdentificationUnit";
                    Itg.ColumnName = "TaxonomicGroup";
                    Itg.Content = this.comboBoxTaxonomicGroup.Text;
                    this._ImportColumnsAddOn.Add(Itg);

                    DiversityCollection.ImportColumn Iu2id = new ImportColumn();
                    Iu2id.TableAlias = "IdentificationUnit_2";
                    Iu2id.TableName = "IdentificationUnit";
                    Iu2id.ColumnName = "CollectionSpecimenID";
                    Iu2id.Content = "";
                    this._ImportColumnsAddOn.Add(Iu2id);

                    DiversityCollection.ImportColumn Iu2uid = new ImportColumn();
                    Iu2uid.TableAlias = "IdentificationUnit_2";
                    Iu2uid.TableName = "IdentificationUnit";
                    Iu2uid.ColumnName = "IdentificationUnitID";
                    Iu2uid.Content = "";
                    this._ImportColumnsAddOn.Add(Iu2uid);

                    DiversityCollection.ImportColumn Iu2lic = new ImportColumn();
                    Iu2lic.TableAlias = "IdentificationUnit_2";
                    Iu2lic.TableName = "IdentificationUnit";
                    Iu2lic.ColumnName = "LastIdentificationCache";
                    Iu2lic.Content = "";
                    this._ImportColumnsAddOn.Add(Iu2lic);

                    DiversityCollection.ImportColumn Iu2do = new ImportColumn();
                    Iu2do.TableAlias = "IdentificationUnit_2";
                    Iu2do.TableName = "IdentificationUnit";
                    Iu2do.ColumnName = "DisplayOrder";
                    if (this.radioButtonMain2.Checked) Iu2do.Content = "1";
                    else Iu2do.Content = "2";
                    this._ImportColumnsAddOn.Add(Iu2do);

                    DiversityCollection.ImportColumn Iu2tg = new ImportColumn();
                    Iu2tg.TableAlias = "IdentificationUnit_2";
                    Iu2tg.TableName = "IdentificationUnit";
                    Iu2tg.ColumnName = "TaxonomicGroup";
                    Iu2tg.Content = this.comboBoxTaxonomicGroupUnit2.Text;
                    this._ImportColumnsAddOn.Add(Iu2tg);
                    this._ImportColumnsAll = null;

                    this.textBoxUnitTable1.Text = "IdentificationUnit_1";
                    this.textBoxUnitTable2.Text = "IdentificationUnit_2";

                }
                else
                {
                    //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                    //{
                    //    if (KV.Value.StartsWith("Identification"))
                    //        this._ParentAlias.Add(KV.Key, "IdentificationUnit");
                    //}

                    DiversityCollection.ImportColumn Iid = new ImportColumn();
                    Iid.TableAlias = "IdentificationUnit";
                    Iid.TableName = "IdentificationUnit";
                    Iid.ColumnName = "CollectionSpecimenID";
                    Iid.Content = "";
                    this._ImportColumnsAddOn.Add(Iid);

                    DiversityCollection.ImportColumn Iuid = new ImportColumn();
                    Iuid.TableAlias = "IdentificationUnit";
                    Iuid.TableName = "IdentificationUnit";
                    Iuid.ColumnName = "IdentificationUnitID";
                    Iuid.Content = "";
                    this._ImportColumnsAddOn.Add(Iuid);

                    DiversityCollection.ImportColumn Ilic = new ImportColumn();
                    Ilic.TableAlias = "IdentificationUnit";
                    Ilic.TableName = "IdentificationUnit";
                    Ilic.ColumnName = "LastIdentificationCache";
                    Ilic.Content = "";
                    this._ImportColumnsAddOn.Add(Ilic);

                    DiversityCollection.ImportColumn Ido = new ImportColumn();
                    Ido.TableAlias = "IdentificationUnit";
                    Ido.TableName = "IdentificationUnit";
                    Ido.ColumnName = "DisplayOrder";
                    Ido.Content = "1";
                    this._ImportColumnsAddOn.Add(Ido);

                    DiversityCollection.ImportColumn Itg = new ImportColumn();
                    Itg.TableAlias = "IdentificationUnit";
                    Itg.TableName = "IdentificationUnit";
                    Itg.ColumnName = "TaxonomicGroup";
                    Itg.Content = this.comboBoxTaxonomicGroup.Text;
                    this._ImportColumnsAddOn.Add(Itg);
                    this._ImportColumnsAll = null;

                    this.textBoxUnitTable1.Text = "IdentificationUnit";
                }
                this.addMissingPkColumns();
            }
        }

        private bool UnitHierarchyMissing
        {
            get
            {
                bool OK = false;
                System.Collections.Generic.List<string> IdentificationTableAliasList = new List<string>();
                System.Collections.Generic.List<string> IdentificationUnitTableAliasList = new List<string>();
                foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                {
                    if (I.TableName == "Identification")
                    {
                        if (!IdentificationTableAliasList.Contains(I.TableAlias))
                            IdentificationTableAliasList.Add(I.TableAlias);
                    }
                    if (I.TableName == "IdentificationUnit")
                    {
                        if (!IdentificationUnitTableAliasList.Contains(I.TableAlias))
                            IdentificationUnitTableAliasList.Add(I.TableAlias);
                    }
                }
                if (IdentificationTableAliasList.Count > 1 || IdentificationUnitTableAliasList.Count > 1)
                {
                    if (this.ManyUnitTables)
                    {
                        if (this.textBoxUnitTable1.Text.Length == 0
                           || this.textBoxUnitTable2.Text.Length == 0)
                        {
                            OK = true;
                        }
                        if (!TaxonomicGroupsSet) OK = false;
                    }
                }
                else
                {
                    this.checkBox2Units.Checked = false;
                    return false;
                }
                return OK;
            }
        }

        private bool TaxonomicGroupsSet
        {
            get
            {
                bool OK = false;
                if (this.comboBoxTaxonomicGroup.Text.Length == 0 && this.textBoxUnitTable1.Text.Length > 0)//this.comboBoxTaxonomicGroup.Visible && 
                {
                    System.Windows.Forms.MessageBox.Show("Please specify the taxonomic group");
                    this.tabControlPresettings.SelectTab(this.tabPageUnits);
                    this.comboBoxTaxonomicGroup.Focus();
                    return false;
                }
                if (this.comboBoxTaxonomicGroupUnit2.Text.Length == 0 && this.textBoxUnitTable2.Text.Length > 0)//this.comboBoxTaxonomicGroupUnit2.Visible && 
                {
                    System.Windows.Forms.MessageBox.Show("Please specify the taxonomic group of the second organism");
                    this.tabControlPresettings.SelectTab(this.tabPageUnits);
                    this.comboBoxTaxonomicGroupUnit2.Focus();
                    return false;
                }
                if (this.checkBox2Units.Checked)
                {
                    if (this.comboBoxTaxonomicGroup.Text.Length > 0 
                        && this.comboBoxTaxonomicGroupUnit2.Text.Length > 0)
                    {
                        OK = true;
                    }
                    if (!OK)
                    {
                        System.Windows.Forms.MessageBox.Show("Please specify the taxonomic groups for both organisms resp. units");
                        this.comboBoxTaxonomicGroup.Focus();
                        this.tabControlPresettings.SelectTab(this.tabPageUnits);
                    }
                }
                else
                {
                    if (this.comboBoxTaxonomicGroup.Text.Length > 0)
                        OK = true;
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Please specify the taxonomic group");
                        this.comboBoxTaxonomicGroup.Focus();
                        this.tabControlPresettings.SelectTab(this.tabPageUnits);
                        return false;
                    }
                }
                return OK;
            }
        }

        /// <summary>
        /// for the decision if an additional column must be added for the relation
        /// to be used after analsis of the data resp. a filled _ColumnList
        /// </summary>
        private bool insertHostRelation
        {
            get
            {
                if (this.ManyUnitTables
                    && this.checkBoxInsertRelation.Checked
                    && this.textBoxUnitTable1.Text.Length > 0
                    && this.textBoxUnitTable2.Text.Length > 0)
                {
                    if (this.textBoxUnitTable1.Text == this.textBoxUnitTable2.Text) return false;
                    if (this._ImportColumns.Count == 0) return false;
                    else
                    {
                        if (this.ColumnListAll.Contains("RelatedUnitID")) return false;
                        else if (this.ColumnListAddOn.Contains("RelatedUnitID")) return false;
                        else return true;
                    }
                }
                else
                    return false;
            }
        }

        private bool IdentificationAndUnitPresettingsComplete()
        {
            bool OK = true;
            return OK;
        }

        private bool IdentificationAliasListChanged
        {
            get
            {
                bool AliasListChanged = false;
                if (this.AliasDictionary.ContainsValue("Identification"))
                {
                    System.Collections.Generic.List<string> ListAliasInForm = new List<string>();
                    foreach (System.Object o in this.listBoxIdentificationsUnit1.Items)
                    {
                        ListAliasInForm.Add(o.ToString());
                    }
                    foreach (System.Object o in this.listBoxIdentificationsUnit2.Items)
                    {
                        ListAliasInForm.Add(o.ToString());
                    }
                    System.Collections.Generic.List<string> ListAliasInData = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                    {
                        if (KV.Value == "Identification")
                            ListAliasInData.Add(KV.Key);
                    }
                    if (ListAliasInForm.Count != ListAliasInData.Count) AliasListChanged = true;
                    else
                    {
                        foreach (string s in ListAliasInData)
                        {
                            if (!ListAliasInForm.Contains(s))
                            {
                                AliasListChanged = true;
                                break;
                            }
                        }
                    }
                }
                return AliasListChanged;
            }
        }

        private void updateIdentificationLists()
        {
            this.listBoxIdentificationsUnit1.Items.Clear();
            this.listBoxIdentificationsUnit2.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
            {
                if (KV.Value == "Identification")
                    this.listBoxIdentificationsUnit1.Items.Add(KV.Key);
            }
        }

        private void fillIdentificationList()
        {
            bool AliasListChanged = false;
            if (this.AliasDictionary.ContainsValue("Identification"))
            {
                System.Collections.Generic.List<string> ListAliasInForm = new List<string>();
                foreach (System.Object o in this.listBoxIdentificationsUnit1.Items)
                {
                    ListAliasInForm.Add(o.ToString());
                }
                foreach (System.Object o in this.listBoxIdentificationsUnit2.Items)
                {
                    ListAliasInForm.Add(o.ToString());
                }
                System.Collections.Generic.List<string> ListAliasInData = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                {
                    if (KV.Value == "Identification")
                        ListAliasInData.Add(KV.Key);
                }
                if (ListAliasInForm.Count != ListAliasInData.Count) AliasListChanged = true;
                else
                {
                    foreach (string s in ListAliasInData)
                    {
                        if (!ListAliasInForm.Contains(s))
                        {
                            AliasListChanged = true;
                            break;
                        }
                    }
                }
                if (AliasListChanged)
                {
                    this.listBoxIdentificationsUnit1.Items.Clear();
                    this.listBoxIdentificationsUnit2.Items.Clear();
                    if (this.AliasDictionary.ContainsValue("Identification"))
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                        {
                            if (KV.Value == "Identification")
                                this.listBoxIdentificationsUnit1.Items.Add(KV.Key);
                        }
                    }
                }
            }
            else
            {
                this.listBoxIdentificationsUnit1.Items.Clear();
                this.listBoxIdentificationsUnit2.Items.Clear();
            }
        }

        private bool setUnitTables()
        {
            if (this.AliasDictionary.ContainsValue("IdentificationUnit"))
            {
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                {
                    if (KV.Value == "IdentificationUnit")
                    {
                        if (i == 0) this.textBoxUnitTable1.Text = KV.Key;
                        if (i == 1) this.textBoxUnitTable2.Text = KV.Key;
                        i++;
                    }
                    if (i > 1) break;
                }
                return true;
            }
            else
                return false;
        }

        private bool ManyUnitTables
        {
            get
            {
                return this.checkBox2Units.Checked;
            }
            set
            {
                this.buttonMoveToUnit1.Visible = value;
                this.buttonMoveToUnit2.Visible = value;
                this.buttonSwitchUnitTables.Visible = value;
                this.textBoxUnitTable1.Visible = value;
                this.textBoxUnitTable2.Visible = value;
                this.checkBoxInsertRelation.Visible = value;
                this.radioButtonUnit1isHost.Visible = value;
                this.radioButtonUnit2isHost.Visible = value;
                this.labelHost.Visible = value;
                this.tableLayoutPanelUnit2.Visible = value;
                this.radioButtonMain1.Visible = value;
                this.radioButtonMain2.Visible = value;
                this.labelMain.Visible = value;
                this.comboBoxRelationType.Visible = value;
                if (value)
                    this.checkBoxTaxonAnywhere.Text = "Taxon name is storage location.     Main organism:";
                else
                    this.checkBoxTaxonAnywhere.Text = "Taxon name is storage location.";
                this._ImportColumnsAddOn = null;
                if (value)
                    this.checkBox2Units.Text = "Datasets contain 2 organisms. Tables:";
                else
                    this.checkBox2Units.Text = "Datasets contain 2 organisms.";
            }
        }

        private void buttonSwitchUnitTables_Click(object sender, EventArgs e)
        {
            try
            {
                string U1 = this.textBoxUnitTable1.Text;
                string U2 = this.textBoxUnitTable2.Text;
                this.textBoxUnitTable2.Text = U1;
                this.textBoxUnitTable1.Text = U2;
            }
            catch  { }
        }

        private void buttonMoveToUnit2_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxIdentificationsUnit1.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a table from the list");
                    return;
                }
                string ItemToMove = this.listBoxIdentificationsUnit1.SelectedItem.ToString();
                this.listBoxIdentificationsUnit1.Items.Remove(this.listBoxIdentificationsUnit1.SelectedItem);
                this.listBoxIdentificationsUnit2.Items.Add(ItemToMove);
            }
            catch { }
        }

        private void buttonMoveToUnit1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxIdentificationsUnit2.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a table from the list");
                    return;
                }
                string ItemToMove = this.listBoxIdentificationsUnit2.SelectedItem.ToString();
                this.listBoxIdentificationsUnit2.Items.Remove(this.listBoxIdentificationsUnit2.SelectedItem);
                this.listBoxIdentificationsUnit1.Items.Add(ItemToMove);
            }
            catch { }
        }

        private void buttonIdentification1Down_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxIdentificationsUnit1.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a table from the list");
                    return;
                }
                if (this.listBoxIdentificationsUnit1.SelectedIndex + 1 >= this.listBoxIdentificationsUnit1.Items.Count)
                {
                    System.Windows.Forms.MessageBox.Show("The last item can not be moved further down");
                    return;
                }
                string ItemToMove = this.listBoxIdentificationsUnit1.SelectedItem.ToString();
                this.listBoxIdentificationsUnit1.Items.Remove(this.listBoxIdentificationsUnit1.SelectedItem);
                this.listBoxIdentificationsUnit1.Items.Add(ItemToMove);
            }
            catch { }
        }

        private void buttonIdentification2Down_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxIdentificationsUnit2.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a table from the list");
                    return;
                }
                if (this.listBoxIdentificationsUnit2.SelectedIndex + 1 >= this.listBoxIdentificationsUnit2.Items.Count)
                {
                    System.Windows.Forms.MessageBox.Show("The last item can not be moved further down");
                    return;
                }
                string ItemToMove = this.listBoxIdentificationsUnit2.SelectedItem.ToString();
                this.listBoxIdentificationsUnit2.Items.Remove(this.listBoxIdentificationsUnit2.SelectedItem);
                this.listBoxIdentificationsUnit2.Items.Add(ItemToMove);
            }
            catch { }
        }

        private string TableOfParasite
        { 
            get 
            {
                if (this.radioButtonUnit1isHost.Checked)
                    return this.textBoxUnitTable2.Text;
                else 
                    return this.textBoxUnitTable1.Text;
            }
        }

        private string TableOfHost
        {
            get
            {
                if (this.radioButtonUnit2isHost.Checked)
                    return this.textBoxUnitTable2.Text;
                else
                    return this.textBoxUnitTable1.Text;
            }
        }

        private void checkBox2Units_CheckedChanged(object sender, EventArgs e)
        {
            this.ManyUnitTables = this.checkBox2Units.Checked;
        }

        private string AliasOfIdentificationTableOfMainUnit
        {
            get
            {
                string Alias = "";
                if (this.radioButtonMain1.Checked)
                {
                    if (this.listBoxIdentificationsUnit1.Items.Count > 0)
                        Alias = this.listBoxIdentificationsUnit1.Items[0].ToString();
                }
                else if (this.radioButtonMain2.Checked && this.radioButtonMain2.Visible)
                {
                    if (this.listBoxIdentificationsUnit2.Items.Count > 0)
                        Alias = this.listBoxIdentificationsUnit2.Items[0].ToString();
                }
                return Alias;
            }
        }

        private void radioButtonUnit1isHost_Click(object sender, EventArgs e)
        {
            if (this.radioButtonUnit1isHost.Checked) this.radioButtonUnit2isHost.Checked = false;
            else this.radioButtonUnit2isHost.Checked = true;
        }

        private void radioButtonUnit2isHost_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonUnit2isHost.Checked) this.radioButtonUnit1isHost.Checked = false;
            else this.radioButtonUnit1isHost.Checked = true;
        }

        #endregion

        #region Part

        private void radioButtonNoPart_CheckedChanged(object sender, EventArgs e)
        {
            this.setPartsInData();
        }

        private void radioButtonOnePart_CheckedChanged(object sender, EventArgs e)
        {
            this.setPartsInData();
        }

        private void radioButtonTwoParts_CheckedChanged(object sender, EventArgs e)
        {
            this.setPartsInData();
        }

        private void setPartsInData()
        {
            if (this.radioButtonNoPart.Checked)
                this.PartsInData = _PartsInData.none;
            else if (this.radioButtonOnePart.Checked)
                this.PartsInData = _PartsInData.one;
            else if (this.radioButtonTwoParts.Checked)
                this.PartsInData = _PartsInData.two;
            else
                this.PartsInData = _PartsInData.many;
        }

        private _PartsInData PartsInData
        {
            get
            {
                if (this.radioButtonNoPart.Checked) return _PartsInData.none;
                else if (this.radioButtonOnePart.Checked) return _PartsInData.one;
                else if (this.radioButtonTwoParts.Checked) return _PartsInData.two;
                else return _PartsInData.many;
            }
            set
            {
                bool ShowPart1 = false;
                bool ShowPart2 = false;
                bool Many = false;
                switch (value)
                {
                    case _PartsInData.two:
                        ShowPart1 = true;
                        ShowPart2 = true;
                        break;
                    case _PartsInData.one:
                        ShowPart1 = true;
                        break;
                    case _PartsInData.many:
                        ShowPart1 = true;
                        Many = true;
                        break;
                }

                this.labelPart2.Visible = ShowPart2;
                this.labelCollection2.Visible = ShowPart2;
                this.comboBoxCollection2.Visible = ShowPart2;
                this.userControlHierarchySelectorCollection2.Visible = ShowPart2;
                this.labelMaterialCategory2.Visible = ShowPart2;
                this.comboBoxMaterialCategory2.Visible = ShowPart2;
                this.labelDerivedFrom2.Visible = ShowPart2;
                this.comboBoxDerivedFrom2.Visible = ShowPart2;

                string SourcePartTables = "";
                if (ShowPart2)
                {
                    foreach (DiversityCollection.ImportColumn IC in this._ImportColumns)
                    {
                        if (IC.TableName == "CollectionSpecimenPart")
                        {
                            SourcePartTables = IC.TableAlias;
                            break;
                        }
                    }

                    this.comboBoxDerivedFrom2.Items.Clear();
                    this.comboBoxDerivedFrom2.Items.Add(SourcePartTables);
                }

                this.labelPart1.Visible = ShowPart1;
                if (Many)
                    this.labelPart1.Visible = !Many;
                this.labelCollection.Visible = ShowPart1;
                this.comboBoxCollection.Visible = ShowPart1;
                this.userControlHierarchySelectorCollection.Visible = ShowPart1;
                this.labelMaterial.Visible = ShowPart1;
                this.comboBoxMaterialCategory.Visible = ShowPart1;
            }
        }

        private void WriteSpecimenPartIDInDependentTables()
        {
            if (this.ColumnList.Contains("SpecimenPartID") && !this._IsReimport)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV_Parent in this.ParentPartAlias)
                {
                    for (int iL = 0; iL < this._DatasetList[this._DatasetPosition].Count; iL++)
                    {
                        string SpecimenPartID = this.getPartValueFromData(KV_Parent.Key, "SpecimenPartID", iL);
                        for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                        {
                            if (this.ImportColumnsAll[i].TableAlias == KV_Parent.Key
                                && this.ImportColumnsAll[i].ColumnName == "SpecimenPartID"
                                && this._DatasetList[this._DatasetPosition][iL][i].Length == 0)
                            {
                                this._DatasetList[this._DatasetPosition][iL][i] = SpecimenPartID;
                            }
                        }
                    }
                }
            }

        }


        private string getPartValueFromData(string AliasChild, string Column, int Position)
        {
            string Value = "";
            try
            {
                //string AliasParent = this.ParentUnitAlias[AliasChild];
                //for (int i = 0; i < this._ImportColumns.Count; i++)
                //{
                //    if (this._ImportColumns[i].TableAlias == AliasParent
                //        && this._ImportColumns[i].ColumnName == Column
                //        && this.Dataset()[Position][i].Length > 0)
                //    {
                //        Value = this.Dataset()[Position][i];
                //        break;
                //    }
                //}
            }
            catch
            {
            }
            return Value;
        }



         
        //private bool ManyPartTables
        //{
        //    get
        //    {
        //        return this.checkBoxTwoParts.Checked;
        //    }
        //    set
        //    {
        //        this.labelPart2.Visible = value;
        //        this.labelCollection2.Visible = value;
        //        this.comboBoxCollection2.Visible = value;
        //        this.userControlHierarchySelectorCollection2.Visible = value;
        //        this.labelMaterialCategory2.Visible = value;
        //        this.comboBoxMaterialCategory2.Visible = value;
        //        this.labelDerivedFrom2.Visible = value;
        //        this.comboBoxDerivedFrom2.Visible = value;
        //        //if (value)
        //        //    this.checkBoxTaxonAnywhere.Text = "Taxon name is storage location.     Main organism:";
        //        //else
        //        //    this.checkBoxTaxonAnywhere.Text = "Taxon name is storage location.";
        //        //this._ImportColumnsAddOn = null;
        //        //if (value)
        //        //    this.checkBox2Units.Text = "Datasets contain 2 organisms. Tables:";
        //        //else
        //        //    this.checkBox2Units.Text = "Datasets contain 2 organisms.";
        //    }
        //}

        //private void checkBoxTwoParts_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.ManyPartTables = this.checkBoxTwoParts.Checked;
        //}

        private void AddMissingPartTables()
        {
        }

        private bool PartDataComplete
        {
            get
            {
                if (this.PartsInData == _PartsInData.none)
                    return true;

                bool Complete = true;

                if (this.PartsInData == _PartsInData.one || this.PartsInData == _PartsInData.two)
                {
                    // Collection
                    if (this.checkBoxTaxonAnywhere.Checked
                        && (!this.AliasDictionary.ContainsValue("CollectionSpecimenPart")
                        || !this.ColumnListAll.Contains("CollectionID")))
                    {
                        System.Windows.Forms.MessageBox.Show("Please specify the collection");
                        this.tabControlPresettings.SelectTab(this.tabPageStorage);
                        this.comboBoxCollection.Focus();
                        return false;
                    }
                    else if (this.checkBoxTaxonAnywhere.Checked
                        && (this.ColumnListAddOn.Contains("CollectionID") || this.comboBoxCollection.SelectedIndex == -1))
                    {
                        if (this.comboBoxCollection.SelectedIndex == -1 || this.comboBoxCollection.Text.Length == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Please specify the collection");
                            this.tabControlPresettings.SelectTab(this.tabPageStorage);
                            this.comboBoxCollection.Focus();
                            return false;
                        }
                    }

                    // Material
                    if (this.checkBoxTaxonAnywhere.Checked
                        && (!this.AliasDictionary.ContainsValue("CollectionSpecimenPart")
                        || !this.ColumnListAll.Contains("MaterialCategory")))
                    {
                        System.Windows.Forms.MessageBox.Show("Please specify the material");
                        this.tabControlPresettings.SelectTab(this.tabPageStorage);
                        this.comboBoxMaterialCategory.Focus();
                        return false;
                    }
                    else if (this.checkBoxTaxonAnywhere.Checked
                        && (this.ColumnListAddOn.Contains("MaterialCategory") || this.comboBoxMaterialCategory.SelectedIndex == -1))
                    {
                        if (this.comboBoxMaterialCategory.SelectedIndex == -1 || this.comboBoxMaterialCategory.Text.Length == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Please specify the material");
                            this.tabControlPresettings.SelectTab(this.tabPageStorage);
                            this.comboBoxMaterialCategory.Focus();
                            return false;
                        }
                    }
                }
                if (this.PartsInData == _PartsInData.two)
                {
                    // Collection
                    if (this.checkBoxTaxonAnywhere.Checked
                        && (!this.AliasDictionary.ContainsValue("CollectionSpecimenPart")
                        || !this.ColumnListAll.Contains("CollectionID")))
                    {
                        System.Windows.Forms.MessageBox.Show("Please specify the collection of the second part");
                        this.tabControlPresettings.SelectTab(this.tabPageStorage);
                        this.comboBoxCollection2.Focus();
                        return false;
                    }
                    else if (this.checkBoxTaxonAnywhere.Checked
                        && (this.ColumnListAddOn.Contains("CollectionID") || this.comboBoxCollection2.SelectedIndex == -1))
                    {
                        if (this.comboBoxCollection2.SelectedIndex == -1 || this.comboBoxCollection2.Text.Length == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Please specify the collection of the second part");
                            this.tabControlPresettings.SelectTab(this.tabPageStorage);
                            this.comboBoxCollection2.Focus();
                            return false;
                        }
                    }

                    // Material
                    if (this.checkBoxTaxonAnywhere.Checked
                        && (!this.AliasDictionary.ContainsValue("CollectionSpecimenPart")
                        || !this.ColumnListAll.Contains("MaterialCategory")))
                    {
                        System.Windows.Forms.MessageBox.Show("Please specify the material of the second part");
                        this.tabControlPresettings.SelectTab(this.tabPageStorage);
                        this.comboBoxMaterialCategory2.Focus();
                        return false;
                    }
                    else if (this.checkBoxTaxonAnywhere.Checked
                        && (this.ColumnListAddOn.Contains("MaterialCategory") || this.comboBoxMaterialCategory2.SelectedIndex == -1))
                    {
                        if (this.comboBoxMaterialCategory2.SelectedIndex == -1 || this.comboBoxMaterialCategory2.Text.Length == 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Please specify the material of the second part");
                            this.tabControlPresettings.SelectTab(this.tabPageStorage);
                            this.comboBoxMaterialCategory2.Focus();
                            return false;
                        }
                    }
                }
                return Complete;
            }
        }
        
        #endregion    

        #region Unit in Parts

        private void setUnitInParts()
        {
            try
            {
                if (this.AliasDictionary.ContainsValue("IdentificationUnit")
                    && this.AliasDictionary.ContainsValue("CollectionSpecimenPart")
                    && !this.AliasDictionary.ContainsValue("IdentificationUnitInPart"))
                {
                    System.Collections.Generic.List<string> UnitAliasList = new List<string>();
                    if (this.ManyUnitTables)
                    {
                        if (this.radioButtonMain1.Checked) UnitAliasList.Add(this.textBoxUnitTable1.Text);
                        else UnitAliasList.Add(this.textBoxUnitTable2.Text);
                    }
                    System.Collections.Generic.List<string> PartAliasList = new List<string>();
                    //if (this.ManyPartTables)
                    //{
                    //    if (this.radioButtonMain1.Checked) PartAliasList.Add(this.textBoxUnitTable1.Text);
                    //    else PartAliasList.Add(this.textBoxUnitTable2.Text);
                    //}
                    foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                    {
                        if (I.TableName == "IdentificationUnit"
                            && !UnitAliasList.Contains(I.TableAlias))
                            UnitAliasList.Add(I.TableAlias);
                    }

                    int iUnit = 1;
                    //System.Collections.Generic.Dictionary<string, string> dictPartTables = new Dictionary<string, string>();
                    //foreach(System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                    //{
                    //    if (KV.Value == "CollectionSpecimenPart")
                    //        dictPartTables.Add(KV.Key, KV.Value);
                    //}
                    //for (int i = 0; i < dictPartTables.Count; i++)
                    //{

                        //for (iUnit = 1; iUnit <= UnitAliasList.Count; iUnit++)
                        foreach (string Unit in UnitAliasList)                        
                        {
                            DiversityCollection.ImportColumn I = new ImportColumn();
                            //I.TableAlias = "IdentificationUnitInPart_" + (iUnit + i).ToString();
                            I.TableAlias = "IdentificationUnitInPart_" + iUnit.ToString();
                            I.TableName = "IdentificationUnitInPart";
                            I.ColumnName = "CollectionSpecimenID";
                            I.Content = "";
                            this._ImportColumnsAddOn.Add(I);

                            DiversityCollection.ImportColumn IU = new ImportColumn();
                            //I.TableAlias = "IdentificationUnitInPart_" + (iUnit + i).ToString();
                            IU.TableAlias = "IdentificationUnitInPart_" + iUnit.ToString();
                            IU.TableName = "IdentificationUnitInPart";
                            IU.ColumnName = "IdentificationUnitID";
                            IU.Content = "";
                            this._ImportColumnsAddOn.Add(IU);

                            //DiversityCollection.ImportColumn IP = new ImportColumn();
                            ////I.TableAlias = "IdentificationUnitInPart_" + (iUnit + i).ToString();
                            //IP.TableAlias = "IdentificationUnitInPart_" + iUnit.ToString();
                            //IP.TableName = "IdentificationUnitInPart";
                            //IP.ColumnName = "SpecimenPartID";
                            //IP.Content = "1";
                            ////IP.Content = i.ToString();
                            //this._ImportColumnsAddOn.Add(IP);

                            DiversityCollection.ImportColumn ID = new ImportColumn();
                            //I.TableAlias = "IdentificationUnitInPart_" + (iUnit + i).ToString();
                            ID.TableAlias = "IdentificationUnitInPart_" + iUnit.ToString();
                            ID.TableName = "IdentificationUnitInPart";
                            ID.ColumnName = "DisplayOrder";
                            ID.Content = iUnit.ToString();
                            this._ImportColumnsAddOn.Add(ID);
                            this._ImportColumnsAll = null;

                            iUnit++;
                        }
                    //}
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion    

        #region Adding

        private bool MatchingColumnForAddingAreUnique()
        {
            bool OK = false;

            try
            {
                string MatchingColumn = "AccessionNumber";
                string MatchingTable = "CollectionSpecimen";
                if (this.radioButtonAddPartsToPart.Checked)
                    MatchingTable += "Part";
                if (this.radioButtonAddPartsToPart.Checked)
                    MatchingColumn = this.comboBoxAddPartToPartMatchingColumn.SelectedValue.ToString();
                string SQL = "SELECT " + MatchingColumn + " FROM " + MatchingTable + " GROUP BY " + MatchingColumn + " HAVING COUNT(*) > 1";
                System.Data.DataTable dt = new DataTable();
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count == 0)
                    OK = true;
                else
                {
                    string Message = "";
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        if (Message.Length > 0) Message += ", ";
                        Message += R[0].ToString();
                    }
                    Message = "The following values for the column " + MatchingColumn + "\r\nin the table " + MatchingTable + " are not unique:\r\n" + Message 
                        + "\r\n\r\nImport not possible";
                }
            }
            catch (Exception ex)
            {
            }
            return OK;
        }

        #endregion

        #region Analysis

        //private void AnalyseLocalisation()
        //{
        //    bool ResetLocSys = false;
        //    System.Collections.Generic.List<System.Collections.Generic.List<string>> LocSysList = new List<List<string>>();
        //    if (this._TableList.Contains("CollectionEventLocalisation"))
        //    {
        //        for (int i = 0; i < this._AliasList.Count; i++)
        //        {
        //            // check for columns for a localisation (L1, L2 or L1 and L2)
        //            if (this._TableList[i] == "CollectionEventLocalisation"
        //                && (this._ColumnList[i] == "Location1"
        //                || (this._ColumnList.Count > i && this._ColumnList[i] == "Location1" && this._ColumnList[i + 1] == "Location2")
        //                || (i > 0 && this._ColumnList[i] == "Location2" && this._ColumnList[i - 1] != "Location1"))
        //                && this.LocalisationSystemIdIsMissing(this._AliasList[i]))
        //            {
        //                System.Collections.Generic.List<string> L = new List<string>();
        //                L.Add(this._AliasList[i]);
        //                L.Add(this._ColumnList[i]);
        //                L.Add(this.Dataset()[0][i]);
        //                LocSysList.Add(L);
        //            }
        //        }
        //        // if the number of localisations does not correspond with the panels, reset the panels
        //        if (LocSysList.Count != this._LocalisationPanelList.Count) ResetLocSys = true;
        //        else
        //        {
        //            // if the number of panels correspond to the localisations, check the aliases and the contents
        //            for (int i = 0; i < LocSysList.Count; i++)
        //            {
        //                // Check if the alias of the panel is the same as the new one
        //                if (LocSysList[i][0] != this._LocalisationPanelList[i].Name)
        //                {
        //                    ResetLocSys = true;
        //                    break;
        //                }
        //                System.Collections.Generic.List<System.Windows.Forms.TextBox> TextBoxesInPanel = new List<TextBox>();
        //                foreach (System.Windows.Forms.Control C in this._LocalisationPanelList[i].Controls)
        //                {
        //                    if (C.GetType() == typeof(System.Windows.Forms.TextBox) && C.Name.StartsWith("Location"))
        //                        TextBoxesInPanel.Add((System.Windows.Forms.TextBox)C);
        //                }
        //                // check if the number of columns is the same as the number of textboxes
        //                if (TextBoxesInPanel.Count != LocSysList[i].Count - 1)
        //                {
        //                    ResetLocSys = true;
        //                    break;
        //                }
        //                // check if the names of the text boxes correspond to the columns
        //                for (int ii = 1; ii < LocSysList.Count; ii++)
        //                {
        //                    if (LocSysList[ii][1] != TextBoxesInPanel[ii - 1].Name)
        //                    {
        //                        ResetLocSys = true;
        //                        break;
        //                    }
        //                }
        //                if (ResetLocSys) break;
        //            }
        //        }
        //    }
        //    if (ResetLocSys)
        //        this.ResetLocalisationSystemPanels();
        //}

        //private void ResetLocalisationSystemPanels()
        //{
        //    foreach (System.Windows.Forms.Panel P in this._LocalisationPanelList)
        //    {
        //        this.tabPageEvent.Controls.Remove(P);
        //    }
        //    this._LocalisationPanelList.Clear();
        //    if (this._TableList.Contains("CollectionEventLocalisation"))
        //    {
        //        for (int i = 0; i < this._AliasList.Count; i++)
        //        {
        //            if (this._TableList[i] == "CollectionEventLocalisation"
        //                && this._ColumnList[i] == "Location1"
        //                && this.LocalisationSystemIdIsMissing(this._AliasList[i]))
        //            {
        //                System.Collections.Generic.Dictionary<string, string> ColumnContents = new Dictionary<string, string>();
        //                ColumnContents.Add("Location1", this.Dataset()[0][i]);
        //                if (this._TableList.Count > i
        //                    && this._TableList[i + 1] == "CollectionEventLocalisation"
        //                    && this._ColumnList[i + 1] == "Location2")
        //                    ColumnContents.Add("Location2", this.Dataset()[0][i + 1]);
        //                this.AddLocalisation(this._AliasList[i], ColumnContents);
        //            }
        //        }
        //    }
        //}

        //private bool LocalisationSystemIdIsSet
        //{
        //    get
        //    {
        //        bool OK = true;
        //        int UnsetLoc = 0;
        //        try
        //        {
        //            for (int i = 0; i < this.ColumnList.Count; i++)
        //            {
        //                // check if there are empty localisation systems
        //                if (this.ColumnList[i] == "LocalisationSystemID" && this.Dataset()[0][i] == "")
        //                {
        //                    if (this._LocalisationPanelList.Count == 0)
        //                    {
        //                        OK = false;
        //                        break;
        //                    }
        //                    string Alias = this.AliasList[i];
        //                    foreach (System.Windows.Forms.Panel P in this._LocalisationPanelList)
        //                    {
        //                        System.Collections.Generic.List<System.Windows.Forms.ComboBox> ComboboxList = new List<ComboBox>();
        //                        foreach (System.Windows.Forms.Control C in P.Controls)
        //                        {
        //                            if (C.GetType() == typeof(System.Windows.Forms.ComboBox) && C.Name == "comboBoxLocalisationSystem")
        //                                ComboboxList.Add((System.Windows.Forms.ComboBox)C);
        //                        }
        //                        if (P.Name == Alias)
        //                        {
        //                            if (ComboboxList[0].SelectedValue == null)
        //                            {
        //                                UnsetLoc++;
        //                                OK = false;
        //                            }
        //                            else
        //                            {
        //                                for (int ii = 0; ii < this._ContentListAddOn.Count; ii++)
        //                                {
        //                                    if (this._AliasListAddOn[ii] == Alias
        //                                        && this._TableListAddOn[ii] == "CollectionEventLocalisation"
        //                                        && this._ColumnListAddOn[ii] == "LocalisationSystemID")
        //                                    {
        //                                        this._ContentListAddOn[ii] = ComboboxList[0].SelectedValue.ToString();
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            OK = false;
        //        }
        //        if (!OK /* && UnsetLoc > 0*/)
        //        {
        //            System.Windows.Forms.MessageBox.Show("Please set the localisation systems");
        //            this.tabControlPresettings.SelectTab(this.tabPageEvent);
        //        }
        //        if (UnsetLoc == 0 && this._LocalisationPanelList.Count > 0)
        //        {
        //            //this.ResetLocalisationSystemPanels();
        //            //foreach (System.Windows.Forms.Panel P in this._LocalisationPanelList)
        //            //{
        //            //    this.tabPageEvent.Controls.Remove(P);
        //            //}
        //            //this._LocalisationPanelList.Clear();
        //        }
        //        return OK;
        //    }
        //}

        //private bool LocalisationSystemIdIsMissing(string TableAlias)
        //{
        //    bool IsMissing = true;
        //    for (int i = 0; i < this._AliasList.Count; i++)
        //    {
        //        if (this._AliasList[i] == TableAlias
        //            && this._ColumnList[i] == "LocalisationSystemID"
        //            && this.Dataset()[0][i] != "")
        //        {
        //            IsMissing = false;
        //            break;
        //        }
        //    }
        //    return IsMissing;
        //}

        //private void AddLocalisation(string TableAlias, System.Collections.Generic.Dictionary<string, string> ColumnContents)
        //{
        //    System.Windows.Forms.Panel PanelLoc = new Panel();
        //    PanelLoc.Name = TableAlias;
        //    PanelLoc.Width = this.tabPageEvent.Width;
        //    this._LocalisationPanelList.Add(PanelLoc);
        //    PanelLoc.SuspendLayout();

        //    int Position = 0;

        //    System.Windows.Forms.Label labelLocalisationSystem = new Label();
        //    PanelLoc.Controls.Add(labelLocalisationSystem);
        //    labelLocalisationSystem.Dock = System.Windows.Forms.DockStyle.Left;
        //    labelLocalisationSystem.Name = "labelLocalisationSystem";
        //    labelLocalisationSystem.Width = 140;
        //    labelLocalisationSystem.Location = new System.Drawing.Point(Position, 0);
        //    //labelLocalisationSystem.Size = new System.Drawing.Size(200, 21);
        //    labelLocalisationSystem.Text = "Localisation " + TableAlias + ":";
        //    labelLocalisationSystem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

        //    Position += labelLocalisationSystem.Width;

        //    System.Windows.Forms.ComboBox comboBoxLocalisationSystem = new ComboBox();
        //    PanelLoc.Controls.Add(comboBoxLocalisationSystem);
        //    comboBoxLocalisationSystem.Location = new System.Drawing.Point(Position, 0);
        //    comboBoxLocalisationSystem.Dock = DockStyle.Right;
        //    comboBoxLocalisationSystem.Name = "comboBoxLocalisationSystem";
        //    comboBoxLocalisationSystem.Width = 140;
        //    //comboBoxLocalisationSystem.Size = new System.Drawing.Size(300, 21);
        //    comboBoxLocalisationSystem.DataSource = DiversityCollection.LookupTable.DtLocalisationSystemNew;
        //    comboBoxLocalisationSystem.DisplayMember = "LocalisationSystemName";
        //    comboBoxLocalisationSystem.ValueMember = "LocalisationSystemID";
        //    comboBoxLocalisationSystem.SelectedIndexChanged += new System.EventHandler(this.comboBoxLoc_SelectedIndexChanged);
        //    comboBoxLocalisationSystem.SelectedIndex = -1;

        //    Position += comboBoxLocalisationSystem.Width;

        //    System.Windows.Forms.Label labelLocation1 = new Label();
        //    labelLocation1.Name = "labelLocation1";
        //    labelLocation1.Text = "Location1:";
        //    System.Windows.Forms.TextBox textBoxLocation1 = new TextBox();
        //    textBoxLocation1.ReadOnly = true;
        //    textBoxLocation1.Name = "textboxLocation1";
        //    if (ColumnContents.ContainsKey("Location1"))
        //    {
        //        PanelLoc.Controls.Add(labelLocation1);
        //        labelLocation1.Dock = DockStyle.Right;
        //        labelLocation1.Width = 140;
        //        //labelLocation1.Width = "Location1".Length * (int)labelLocation1.Font.SizeInPoints / 2;
        //        labelLocation1.Location = new System.Drawing.Point(Position, 0);
        //        labelLocation1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

        //        Position += labelLocation1.Width;

        //        PanelLoc.Controls.Add(textBoxLocation1);
        //        textBoxLocation1.Dock = DockStyle.Right;
        //        textBoxLocation1.Text = ColumnContents["Location1"];
        //        //textBoxLocation1.Width = ColumnContents["Location1"].Length * (int)textBoxLocation1.Font.SizeInPoints/2;
        //        textBoxLocation1.Width = 200;
        //        textBoxLocation1.Dock = DockStyle.Right;
        //        textBoxLocation1.Location = new System.Drawing.Point(Position, 0);

        //        Position += textBoxLocation1.Width;
        //    }

        //    System.Windows.Forms.Label labelLocation2 = new Label();
        //    labelLocation2.Name = "labelLocation2";
        //    labelLocation2.Text = "Location2:";
        //    System.Windows.Forms.TextBox textBoxLocation2 = new TextBox();
        //    textBoxLocation2.ReadOnly = true;
        //    textBoxLocation2.Name = "textboxLocation2";
        //    if (ColumnContents.ContainsKey("Location2"))
        //    {
        //        PanelLoc.Controls.Add(labelLocation2);
        //        labelLocation2.Dock = DockStyle.Right;
        //        labelLocation2.Width = 140;
        //        //labelLocation2.Width = "Location2".Length * (int)labelLocation2.Font.SizeInPoints / 2;
        //        labelLocation2.Location = new System.Drawing.Point(Position, 0);
        //        labelLocation2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

        //        Position += labelLocation2.Width;

        //        PanelLoc.Controls.Add(textBoxLocation2);
        //        textBoxLocation2.Dock = DockStyle.Right;
        //        textBoxLocation2.Text = ColumnContents["Location2"];
        //        //textBoxLocation2.Width = ColumnContents["Location2"].Length * (int)textBoxLocation2.Font.SizeInPoints / 2;
        //        textBoxLocation2.Width = 200;
        //        textBoxLocation2.Dock = DockStyle.Right;
        //        textBoxLocation2.Location = new System.Drawing.Point(Position, 0);
        //    }

        //    //comboBoxLocalisationSystem.Dock = DockStyle.Fill;
        //    comboBoxLocalisationSystem.BringToFront();
        //    this.tabPageEvent.Controls.Add(PanelLoc);
        //    PanelLoc.BringToFront();
        //    PanelLoc.Dock = System.Windows.Forms.DockStyle.Top;
        //    //PanelLoc.Location = new System.Drawing.Point(3, 103);
        //    //PanelLoc.Name = "PanelLoc";
        //    PanelLoc.Height = 23;
        //    PanelLoc.Padding = new Padding(0, 1, 0, 1);
        //}

        //#region Combobox events
        //private void comboBoxLoc_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Windows.Forms.ComboBox C = (System.Windows.Forms.ComboBox)sender;
        //        System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)C.Parent;
        //        System.Collections.Generic.List<System.Windows.Forms.TextBox> TextBoxList = new List<TextBox>();
        //        System.Collections.Generic.List<System.Windows.Forms.Label> LabelList = new List<Label>();
        //        foreach (System.Windows.Forms.Control L in P.Controls)
        //        {
        //            if (L.GetType() == typeof(System.Windows.Forms.Label) && L.Name.StartsWith("labelLocation"))
        //                LabelList.Add((System.Windows.Forms.Label)L);
        //        }
        //        int LocSysID = 0;
        //        if (int.TryParse(C.SelectedValue.ToString(), out LocSysID))
        //        {
        //            System.Data.DataRow[] r1 = DiversityCollection.LookupTable.DtLocationColumns(LocSysID).Select("ColumnName = 'Location1'");
        //            System.Data.DataRow[] r2 = DiversityCollection.LookupTable.DtLocationColumns(LocSysID).Select("ColumnName = 'Location2'");
        //            foreach (System.Windows.Forms.Label L in LabelList)
        //            {
        //                if (L.Name == "labelLocation1") L.Text = r1[0]["DisplayText"].ToString() + ":";
        //                if (L.Name == "labelLocation2") L.Text = r2[0]["DisplayText"].ToString() + ":";
        //            }
        //        }
        //        string Alias = P.Name;
        //        for (int i = 0; i < this._ContentListAddOn.Count; i++)
        //        {
        //            if (this._AliasListAddOn[i] == Alias
        //                && this._TableListAddOn[i] == "CollectionEventLocalisation"
        //                && this._ColumnListAddOn[i] == "LocalisationSystemID")
        //            {
        //                this._ContentListAddOn[i] = LocSysID.ToString();
        //                this.analyseData(0);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void comboBoxLoc_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Windows.Forms.ComboBox C = (System.Windows.Forms.ComboBox)sender;
        //        System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)C.Parent;
        //        System.Collections.Generic.List<System.Windows.Forms.ComboBox> ComboBoxList = (System.Collections.Generic.List<System.Windows.Forms.ComboBox>)P.Tag;
        //        int LocSysID = 0;
        //        if (int.TryParse(C.SelectedValue.ToString(), out LocSysID))
        //        {
        //            ComboBoxList[1].DataSource = DiversityCollection.LookupTable.DtLocationColumns(LocSysID);
        //            ComboBoxList[1].DisplayMember = DiversityCollection.LookupTable.DtLocationColumns(LocSysID).Columns[1].ColumnName;
        //            ComboBoxList[1].ValueMember = DiversityCollection.LookupTable.DtLocationColumns(LocSysID).Columns[0].ColumnName;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void comboBoxLocCol_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        System.Windows.Forms.ComboBox C = (System.Windows.Forms.ComboBox)sender;
        //        if (C.SelectedIndex > -1 && C.ValueMember != "")
        //        {
        //            System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)C.Parent;
        //            foreach (System.Windows.Forms.Control L in P.Controls)
        //            {
        //                if (L.GetType() == typeof(System.Windows.Forms.Label) && L.Name == "labelLocCol")
        //                    L.Text = C.SelectedValue.ToString() + ":";
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        //private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        //{

        //}

        //#endregion

        #endregion

        #endregion

        #region Checks of definitions etc.

        private bool RefillImportColumnsAndCheckForChanges()
        {
            bool OK = true;
            try
            {
                this.ImportColumns = null;
                if (!this._IsReimport)
                {
                    if (this.ColumnMappingComplete)
                    {
                        this.WriteColumnMappingToImportColumns();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Please set all columns");
                        this.tabControlPresettings.SelectedTab = this.tabPageColumnMapping;
                        this.dataGridViewColumnMapping.Focus();
                        return false;
                    }
                }
                else
                {
                    if (!this.WriteHeaderLinesToImportColumnsAndCheck())
                        return false;
                }
                if (this.DuplicateColumnsInData)
                {
                    return false;
                }
                this.setPresetControlVisibility();
                if (!this._IsReimport)
                {
                    // Event and related tables
                    if (this.EventDataInFile && !this._IsReimport)
                    {
                        this.addMissingEventTable();
                        if (!this.LocalisationPanelsMatchData)
                        {
                            this.ResetLocalisationSystemPanels();
                            System.Windows.Forms.MessageBox.Show("Please set the localisation systems");
                            this.tabControlPresettings.SelectTab(this.tabPageEvent);
                            //this.AnalyseLocalisation();
                            return false;
                        }
                        else
                        {
                            if (!this.LocalisationSystemIdsInPanelsAreSet)
                            {
                                System.Windows.Forms.MessageBox.Show("Please set the localisation systems");
                                this.tabControlPresettings.SelectTab(this.tabPageEvent);
                                //this.AnalyseLocalisation();
                                return false;
                            }
                        }
                    }

                    // Add missing specimen
                    this.addMissingSpecimenTable();

                    // Identification and related tables
                    if (this.IdentificationAliasListChanged && !this._IsReimport)
                    {
                        this.updateIdentificationLists();
                        if (this.listBoxIdentificationsUnit1.Items.Count > 1 && this.ManyUnitTables)
                        {
                            System.Windows.Forms.MessageBox.Show("Please check the identification tables");
                            return false;
                        }
                    }
                    this.addMissingUnitTables();
                    if (!this.TaxonomicGroupsSet)
                        return false;
                    if (this.UnitHierarchyMissing)
                    {
                        this.ManyUnitTables = this.ManyUnitTables;
                        if (this.ManyUnitTables)
                        {
                            if (!this.setUnitTables()) return false;
                        }
                        return false;
                    }

                    this.AddMissingPartTables();

                    this.initPresettings();

                    //Part and related tables
                    if (!this.PartDataComplete)
                        return false;
                    else
                    {
                        this.setUnitInParts();
                        OK = true;
                    }

                    //this.initPresettings();

                    this.addMissingPkColumns();
                    //if (!this.TaxonomicGroupsSet) return false;
                }
            }
            catch { return false; }
            return OK;
        }

        private bool DuplicateColumnsInData
        {
            get
            {
                bool OK = false;
                for (int i = 0; i < this._ImportColumns.Count; i++)
                {
                    for (int d = i + 1; d < this._ImportColumns.Count; d++)
                    {
                        if (this._ImportColumns[i].TableAlias == this._ImportColumns[d].TableAlias
                            && this._ImportColumns[i].TableName == this._ImportColumns[d].TableName
                            && this._ImportColumns[i].ColumnName == this._ImportColumns[d].ColumnName)
                        {
                            string Message = "The colomn \r\n\t" + this._ImportColumns[i].ColumnName +
                                "\r\nin table\r\n\t" + this._ImportColumns[i].TableName +
                                "\r\nwith the alias\r\n\t" + this._ImportColumns[i].TableAlias +
                                "\r\nis more than one time in your data definitions.\r\nPlease correct this";
                            System.Windows.Forms.MessageBox.Show(Message);
                            this.dataGridViewColumnMapping.Rows[1].Cells[i].Selected = true;
                            return true;
                        }
                    }
                }
                return OK;
            }
        }

        private bool FillImportColumnsAddOn()
        {
            try
            {
                if (!this._IsReimport)
                {
                    this.fillIdentificationList();
                    this.addMissingTables();
                    this.initPresettings();
                    this.setPresetControlVisibility();
                    this.addMissingPkColumns();
                    if (this.UnitHierarchyMissing)
                    {
                        this.ManyUnitTables = this.ManyUnitTables;
                        if (this.ManyUnitTables)
                        {
                            if (!this.setUnitTables()) return false;
                        }
                        return false;
                    }
                    if (!this.TaxonomicGroupsSet) return false;
                    if (!this.LocalisationSystemIdIsSet)
                    {
                        this.AnalyseLocalisation();
                        return false;
                    }
                    if (!this.PartDataComplete)
                        return false;
                    else
                    {
                        this.setUnitInParts();
                        return true;
                    }
                }
                else
                    return true;
            }
            catch { return false; }
        }

        private bool ReadyForImportAndNoMissingEntries()
        {
            try
            {
                this.analyseData(0);
                return true;
            }
            catch { return false; }
        }
        
        #endregion

        #region Column mapping

        #region Save and import mapping

		private void buttonColumnMappingImportFromFile_Click(object sender, EventArgs e)
        {
            string Dir = System.Windows.Forms.Application.StartupPath;
            if (System.IO.Directory.Exists(Dir + "\\ImportMappings")) Dir += "\\ImportMappings";
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            this.openFileDialog.InitialDirectory =  Dir;
            this.openFileDialog.Filter = "XML Files|*.xml";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    this.ImportColumnMappingFromFile(this.openFileDialog.FileName);
                    if (this.ColumnMappingComplete)
                    {
                        this.buttonAnalyse.Enabled = true;
                        this.toolStripNavigation.Enabled = true;
                    }
                    else
                    {
                        this.buttonAnalyse.Enabled = false;
                        this.toolStripNavigation.Enabled = false;
                    }
                }
                //else
                //    this.checkBoxUseColumnMapping.Enabled = false;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ImportColumnMappingFromFile(string FileName)
        {
            System.Collections.Generic.List<DiversityCollection.ImportColumnGroup> IClist = new List<ImportColumnGroup>();
            string Node = "";
            //System.Xml.XmlReader R = System.Xml.XmlReader.Create(FileName);
            System.Xml.XmlTextReader R = new System.Xml.XmlTextReader(FileName);
            while (R.Read())
            {
                if (R.IsStartElement())
                {
                    Node = R.Name;
                    if (Node == "ImportColumnGroups")
                        break;
                    switch (Node)
                    {
                        case "ImportColumn":
                            DiversityCollection.ImportColumnGroup I = new ImportColumnGroup();
                            IClist.Add(I);
                            break;
                        case "TableName":
                            R.Read();
                            DiversityCollection.ImportColumnGroup IT = IClist[IClist.Count - 1];
                            IT.TableName = R.Value;
                            IClist[IClist.Count - 1] = IT;
                            break;
                        case "TableAlias":
                            R.Read();
                            DiversityCollection.ImportColumnGroup IA = IClist[IClist.Count - 1];
                            IA.TableAlias = R.Value;
                            IClist[IClist.Count - 1] = IA;
                            break;
                        case "ColumnName":
                            R.Read();
                            DiversityCollection.ImportColumnGroup IC = IClist[IClist.Count - 1];
                            IC.ColumnName = R.Value;
                            IClist[IClist.Count - 1] = IC;
                            break;
                        case "GroupColor":
                            R.Read();
                            DiversityCollection.ImportColumnGroup IG = IClist[IClist.Count - 1];
                            IG.GroupColor = this.GroupColor(R.Value);
                            IClist[IClist.Count - 1] = IG;
                            break;
                    }
                }
                else
                {
                    //Node = R.Name;
                    //string v = R.LocalName;
                    //string x = R.Value;
                    //if (Node == "GroupColor")
                    {
                        if (this.GroupColor(R.Value) == System.Drawing.Color.Black)
                        {
                            DiversityCollection.ImportColumnGroup IG = IClist[IClist.Count - 1];
                            IG.GroupColor = this.GroupColor(R.Value);
                            IClist[IClist.Count - 1] = IG;
                        }
                    }
                }
            }
            for (int i = 0; i < IClist.Count; i++ )
            {
                if (this.dataGridViewColumnMapping.Rows[0].Cells.Count <= i)
                    break;
                this.dataGridViewColumnMapping.Rows[0].Cells[i].Value = IClist[i].TableName;
                this.dataGridViewColumnMapping.Rows[1].Cells[i].Value = IClist[i].TableAlias;
                this.dataGridViewColumnMapping.Rows[2].Cells[i].Value = IClist[i].ColumnName;
                if (IClist[i].GroupColor != System.Drawing.Color.White)
                {
                    this.dataGridViewColumnMapping.Rows[0].Cells[i].Style.BackColor = IClist[i].GroupColor;
                    this.dataGridViewColumnMapping.Rows[1].Cells[i].Style.BackColor = IClist[i].GroupColor;
                    this.dataGridViewColumnMapping.Rows[2].Cells[i].Style.BackColor = IClist[i].GroupColor;
                }
            }
            this._ImportColumnGroupList.Clear();
            while (R.Read())
            {
                if (R.IsStartElement())
                {
                    Node = R.Name;
                    if (Node == "Presettings")
                        break;
                    switch (Node)
                    {
                        case "ImportColumnGroup":
                            DiversityCollection.ImportColumnGroup I = new ImportColumnGroup();
                            this._ImportColumnGroupList.Add(I);
                            break;
                        case "TableName":
                            R.Read();
                            DiversityCollection.ImportColumnGroup IT = this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1];
                            IT.TableName = R.Value;
                            this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1] = IT;
                            break;
                        case "TableAlias":
                            R.Read();
                            DiversityCollection.ImportColumnGroup IA = this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1];
                            IA.TableAlias = R.Value;
                            this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1] = IA;
                            break;
                        case "ColumnName":
                            R.Read();
                            DiversityCollection.ImportColumnGroup IC = this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1];
                            IC.ColumnName = R.Value;
                            this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1] = IC;
                            break;
                        case "Separator":
                            R.Read();
                            DiversityCollection.ImportColumnGroup IS = this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1];
                            IS.Separator = R.Value;
                            this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1] = IS;
                            break;
                        case "GroupColor":
                            R.Read();
                            DiversityCollection.ImportColumnGroup IG = this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1];
                            IG.GroupColor = this.GroupColor(R.Value);
                            this._ImportColumnGroupList[this._ImportColumnGroupList.Count - 1] = IG;
                            break;
                    }
                }
            }
            //if (System.Windows.Forms.MessageBox.Show("Do you want to import the presettings", "Import presettings?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                //if (this._ImportColumnsAddOn != null)
                //    this._ImportColumnsAddOn.Clear();
                //else
                //    this._ImportColumnsAddOn = new List<ImportColumn>();
                while (R.Read())
                {
                    int iCurrentControl;
                    //if (R.IsStartElement())
                    {
                        Node = R.Name;
                        //string ControlName = R.Name;
                        if (Node == "Control.Name")
                        {
                            R.Read();
                            string ControlName = R.Value;
                            string x = R.LocalName;
                            string TableName = "";
                            string ColumnName = "";
                            string Alias = "";
                            while (R.Read() && (TableName.Length == 0 || ColumnName.Length == 0 || Alias.Length == 0))
                            {
                                if (R.Name == "TableName" && R.NodeType == System.Xml.XmlNodeType.Element)
                                {   
                                    R.Read();
                                    if (R.HasValue && R.NodeType == System.Xml.XmlNodeType.Text)
                                        TableName = R.Value;
                                }
                                else if (R.Name == "ColumnName" && R.NodeType == System.Xml.XmlNodeType.Element)
                                {
                                    R.Read();
                                    if (R.HasValue && R.NodeType == System.Xml.XmlNodeType.Text)
                                        ColumnName = R.Value;
                                }
                                else if (R.Name == "Alias" && R.NodeType == System.Xml.XmlNodeType.Element)
                                {
                                    R.Read();
                                    if (R.HasValue && R.NodeType == System.Xml.XmlNodeType.Text)
                                        Alias = R.Value;
                                }
                            }
                            for (iCurrentControl = 0; iCurrentControl < this._PresetControls.Count; iCurrentControl++)
                            {
                                if (this._PresetControls[iCurrentControl].Name == ControlName)
                                {
                                    System.Data.DataColumn Col = (System.Data.DataColumn)this._PresetControls[iCurrentControl].Tag;
                                    if (Col.Table.TableName == TableName
                                        && Col.ColumnName == ColumnName)
                                        break;
                                }
                            }
                            if (this._PresetControls.Count <= iCurrentControl)
                            {
                                continue;
                            }
                            while (R.Read())
                            {
                                string Property = R.Name;
                                if (Property == "Control.SelectedIndex" || Property == "Control.Text")
                                {
                                    R.Read();
                                    string Value = R.Value;
                                    switch (Property)
                                    {
                                        case "Control.SelectedIndex":
                                            System.Windows.Forms.ComboBox c = (System.Windows.Forms.ComboBox)this._PresetControls[iCurrentControl];
                                            try
                                            {
                                                c.SelectedIndex = int.Parse(Value);
                                            }
                                            catch { }
                                            break;
                                        case "Control.Text":
                                            this._PresetControls[iCurrentControl].Text = Value;
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                        //switch (Node)
                        //{
                        //    case "Control.Name":
                        //        for (iCurrentControl = 0; iCurrentControl < this._PresetControls.Count; iCurrentControl++)
                        //        {
                        //            if (this._PresetControls[iCurrentControl].Name == Node)
                        //                break;
                        //        }
                        //        break;
                        //    case "ImportColumn":
                        //        DiversityCollection.ImportColumn I = new ImportColumn();
                        //        this._ImportColumnsAddOn.Add(I);
                        //        break;
                        //    case "TableName":
                        //        R.Read();
                        //        DiversityCollection.ImportColumn IT = this._ImportColumnsAddOn[this._ImportColumnsAddOn.Count - 1];
                        //        IT.TableName = R.Value;
                        //        this._ImportColumnsAddOn[this._ImportColumnsAddOn.Count - 1] = IT;
                        //        break;
                        //    case "TableAlias":
                        //        R.Read();
                        //        DiversityCollection.ImportColumn IA = this._ImportColumnsAddOn[this._ImportColumnsAddOn.Count - 1];
                        //        IA.TableAlias = R.Value;
                        //        this._ImportColumnsAddOn[this._ImportColumnsAddOn.Count - 1] = IA;
                        //        break;
                        //    case "ColumnName":
                        //        R.Read();
                        //        DiversityCollection.ImportColumn IC = this._ImportColumnsAddOn[this._ImportColumnsAddOn.Count - 1];
                        //        IC.ColumnName = R.Value;
                        //        this._ImportColumnsAddOn[this._ImportColumnsAddOn.Count - 1] = IC;
                        //        break;
                        //    case "Content":
                        //        R.Read();
                        //        DiversityCollection.ImportColumn IS = this._ImportColumnsAddOn[this._ImportColumnsAddOn.Count - 1];
                        //        IS.Content = R.Value;
                        //        this._ImportColumnsAddOn[this._ImportColumnsAddOn.Count - 1] = IS;
                        //        break;
                        //}
                    }
                }
                //foreach (System.Windows.Forms.Control C in this._PresetControls)
                //{
                //    foreach (DiversityCollection.ImportColumn IC in this._ImportColumnsAddOn)
                //    {
                //        if (IC.Content.Length > 0)
                //        {
                //            System.Data.DataColumn col = (System.Data.DataColumn)C.Tag;
                //            if (col.ColumnName == IC.ColumnName
                //                && col.Table.TableName == IC.TableName
                //                && col.Table.TableName == IC.TableAlias)
                //            {
                //                if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                //                {
                //                    System.Windows.Forms.ComboBox c = (System.Windows.Forms.ComboBox)C;
                //                    if (c.DataSource != null)
                //                    {
                //                        try
                //                        {
                //                            for (int i = 0; i < c.Items.Count; i++)
                //                            {
                //                                System.Data.DataRowView RV = (System.Data.DataRowView)c.Items[i];
                //                                if (RV[c.ValueMember].ToString() == IC.Content)
                //                                {
                //                                    c.SelectedIndex = i;
                //                                    break;
                //                                }
                //                            }
                //                        }
                //                        catch { }
                //                    }
                //                }
                //                else if (C.GetType() == typeof(System.Windows.Forms.TextBox)
                //                    || (C.GetType() == typeof(System.Windows.Forms.MaskedTextBox)))
                //                {
                //                    C.Text = IC.Content;
                //                }
                //                break;
                //            }
                //        }
                //    }
                //}
                //int test = this._ImportColumnsAddOn.Count;
            }
        }

        private void buttonColumnMappingSaveToFile_Click(object sender, EventArgs e)
        {
            this.saveFileDialog = new SaveFileDialog();
            string Dir = System.Windows.Forms.Application.StartupPath + "\\ImportMappings";
            if (!System.IO.Directory.Exists(Dir))
                System.IO.Directory.CreateDirectory(Dir);
            this.saveFileDialog.InitialDirectory = Dir;
            this.saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            this.saveFileDialog.FileName = "ImportSchema_" + System.Environment.UserName + ".xml";
            this.saveFileDialog.ShowDialog();
            if (this.saveFileDialog.FileName.Length > 0)
                this.WriteColumnMappingToFile(this.saveFileDialog.FileName);
        }

        private void WriteColumnMappingToFile(string FileName)
        {
            System.Xml.XmlWriter W;
            try
            {
                W = System.Xml.XmlWriter.Create(FileName);
                W.WriteStartDocument();
                W.WriteStartElement("ColumnMapping");
                W.WriteStartElement("ImportColumns");
                for (int i = 0; i < this.dataGridViewColumnMapping.Columns.Count; i++)
                {
                    W.WriteStartElement("ImportColumn");
                    if (this.dataGridViewColumnMapping.Rows[0].Cells[i].Value == null)
                        W.WriteElementString("TableName", "");
                    else
                        W.WriteElementString("TableName", this.dataGridViewColumnMapping.Rows[0].Cells[i].Value.ToString());
                    
                    if (this.dataGridViewColumnMapping.Rows[1].Cells[i].Value == null)
                        W.WriteElementString("TableAlias", "");
                    else 
                        W.WriteElementString("TableAlias", this.dataGridViewColumnMapping.Rows[1].Cells[i].Value.ToString());
                    
                    if (this.dataGridViewColumnMapping.Rows[2].Cells[i].Value == null)
                        W.WriteElementString("ColumnName", "");
                    else
                        W.WriteElementString("ColumnName", this.dataGridViewColumnMapping.Rows[2].Cells[i].Value.ToString());
                    
                    W.WriteElementString("GroupColor", this.dataGridViewColumnMapping.Rows[0].Cells[i].Style.BackColor.ToString());
                    W.WriteEndElement();        //ImportColumn
                }
                W.WriteEndElement();        //ImportColumns
                W.WriteStartElement("ImportColumnGroups");
                foreach (DiversityCollection.ImportColumnGroup ICG in this._ImportColumnGroupList)
                {
                    W.WriteStartElement("ImportColumnGroup");
                    W.WriteElementString("TableName", ICG.TableName);
                    W.WriteElementString("TableAlias", ICG.TableAlias);
                    W.WriteElementString("ColumnName", ICG.ColumnName);
                    W.WriteElementString("GroupColor", ICG.GroupColor.ToString());
                    W.WriteElementString("Separator", ICG.Separator);
                    W.WriteEndElement();        //ImportColumnGroups
                }
                if (this._PresetControls.Count > 0)
                {
                    W.WriteStartElement("Presettings");
                    foreach (System.Windows.Forms.Control C in this._PresetControls )
                    {
                        System.Data.DataColumn col = (System.Data.DataColumn)C.Tag;
                        if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                        {
                            System.Windows.Forms.ComboBox c = (System.Windows.Forms.ComboBox)C;
                            if (c.SelectedIndex > 0)
                            {
                                W.WriteStartElement("Presetting");
                                W.WriteElementString("Control.Name", C.Name);
                                //W.WriteStartAttribute("TableName", col.Table.TableName);
                                //W.WriteAttributeString("ColumnName", col.ColumnName);
                                //W.WriteAttributeString("Alias", col.Table.TableName);
                                W.WriteElementString("TableName", col.Table.TableName);
                                W.WriteElementString("ColumnName", col.ColumnName);
                                W.WriteElementString("Alias", col.Table.TableName);
                                W.WriteElementString("Control.SelectedIndex", c.SelectedIndex.ToString());
                                W.WriteEndElement();        //Presetting
                            }
                        }
                        else if (C.GetType() == typeof(System.Windows.Forms.TextBox)
                            || (C.GetType() == typeof(System.Windows.Forms.MaskedTextBox)))
                        {
                            if (C.Text.Length > 0)
                            {
                                W.WriteStartElement("Presetting");
                                W.WriteElementString("Control.Name", C.Name);
                                //W.WriteAttributeString("TableName", col.Table.TableName);
                                //W.WriteAttributeString("ColumnName", col.ColumnName);
                                //W.WriteAttributeString("Alias", col.Table.TableName);
                                W.WriteElementString("TableName", col.Table.TableName);
                                W.WriteElementString("ColumnName", col.ColumnName);
                                W.WriteElementString("Alias", col.Table.TableName);
                                W.WriteElementString("Control.Text", C.Text);
                                W.WriteEndElement();        //Presetting
                            }
                        }
                    }
                    W.WriteEndElement();        //Presettings
                }
                //if (this._ImportColumnsAddOn.Count > 0)
                //{
                //    W.WriteStartElement("ImportColumnsAddOn");
                //    foreach (DiversityCollection.ImportColumn IC in this._ImportColumnsAddOn)
                //    {
                //        W.WriteStartElement("ImportColumn");
                //        W.WriteElementString("TableName", IC.TableName);
                //        W.WriteElementString("TableAlias", IC.TableAlias);
                //        W.WriteElementString("ColumnName", IC.ColumnName);
                //        W.WriteElementString("Content", IC.Content);
                //        W.WriteEndElement();        //ImportColumnGroups
                //    }
                //    W.WriteEndElement();        //ImportColumnGroups
                //}
                W.WriteEndElement();        //ImportColumnGroups
                W.WriteFullEndElement();    //ColumnMapping
                W.WriteEndDocument();
                W.Flush();
                W.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            //finally
            //{
            //    if (W != null)
            //    {
            //        W.Flush();
            //        W.Close();
            //    }
            //}
        }
 
	    #endregion    
    
        private System.Drawing.Color GroupColor(string Color)
        {
            foreach (System.Drawing.Color C in this._GroupColorList)
            {
                string ListColor = C.ToString();
                if (Color == C.ToString()) return C;
            }
            return System.Drawing.Color.White;
        }

        private bool InitColumnMapping()
        {
            bool OK = true;
            if (this._ImportColumns == null) this._ImportColumns = new List<ImportColumn>();
            this.checkBox2Units.Checked = false;
            if (this.textBoxImportFile.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a file");
                return false;
            }
            DiversityCollection.Datasets.DataSetCollectionSpecimen ds = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            this.dataGridViewColumnMappingSource.Columns.Clear();
            this.dataGridViewColumnMapping.Columns.Clear();
            try
            {
                //using (System.IO.StreamReader sr = new System.IO.StreamReader(this.textBoxImportFile.Text))
                System.IO.StreamReader sr = this.StreamReader(this.textBoxImportFile.Text);
                using (sr)
                {
                    String line;
                    int iLine = 0;
                    int iColumn = 0;
                    // reading the first for lines into the datagrid
                    while ((line = sr.ReadLine()) != null && iLine + 1 < (int)this.numericUpDownDataStartInLine.Value + (int)this.numericUpDownUpTo.Value)
                    {
                        // reading the columns
                        if (iLine == 0)
                        {
                            string Columns = line;
                            while (Columns.Length > 0)
                            {
                                string Column = "";
                                if (Columns.IndexOf("\t") > -1)
                                    Column = Columns.Substring(0, Columns.IndexOf("\t")).ToString().Trim();
                                else
                                {
                                    Column = Columns.Trim();
                                    Columns = "";
                                }
                                iColumn++;
                                this.dataGridViewColumnMappingSource.Columns.Add("Column_" + iColumn.ToString(), "Column " + iColumn.ToString());
                                this.dataGridViewColumnMapping.Columns.Add("Column_" + iColumn.ToString(), "Column " + iColumn.ToString());
                                Columns = Columns.Substring(Columns.IndexOf("\t") + 1);
                            }
                        }
                        // reading the lines
                        this.dataGridViewColumnMappingSource.Rows.Add(1);
                        if (iLine + 1 < (int)this.numericUpDownDataStartInLine.Value) this.dataGridViewColumnMappingSource.Rows[iLine].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                        iColumn = 0;
                        while (line.Length > 0)// && iColumn < this.dataGridViewColumnMappingSource.Rows[iLine].Cells.Count)
                        {
                            if (iColumn >= this.dataGridViewColumnMappingSource.Rows[iLine].Cells.Count)
                            {
                                this.dataGridViewColumnMappingSource.Columns.Add("Column_" + iColumn.ToString(), "Column " + iColumn.ToString());
                                this.dataGridViewColumnMapping.Columns.Add("Column_" + iColumn.ToString(), "Column " + iColumn.ToString());
                            }
                            if (line.Replace("\t", "").Trim().Length == 0)
                                line = "";
                            else if (line.IndexOf("\t") > -1)
                                this.dataGridViewColumnMappingSource.Rows[iLine].Cells[iColumn].Value = line.Substring(0, line.IndexOf("\t")).ToString().Trim();
                            else
                            {
                                this.dataGridViewColumnMappingSource.Rows[iLine].Cells[iColumn].Value = line.Trim();
                                line = "";
                            }
                            line = line.Substring(line.IndexOf("\t") + 1);
                            iColumn++;
                        }
                        iLine++;
                    }
                }
                this.dataGridViewColumnMapping.Rows.Add(3);
                if (this.dataGridViewColumnMapping.Columns.Count != this._ImportColumns.Count)
                {
                    this._ImportColumns.Clear();
                    foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewColumnMapping.Columns)
                    {
                        DiversityCollection.ImportColumn I = new ImportColumn();
                        I.ColumnName = "";
                        I.TableAlias = "";
                        I.TableName = "";
                        this._ImportColumns.Add(I);
                    }
                }
                else
                {
                    for (int i = 0; i < this.dataGridViewColumnMapping.Columns.Count; i++ )
                    {
                        System.Windows.Forms.DataGridViewRow R_Table = this.dataGridViewColumnMapping.Rows[0];
                        System.Windows.Forms.DataGridViewRow R_Alias = this.dataGridViewColumnMapping.Rows[1];
                        System.Windows.Forms.DataGridViewRow R_Column = this.dataGridViewColumnMapping.Rows[2];
                        R_Table.Cells[i].Value = this._ImportColumns[i].TableName;
                        R_Column.Cells[i].Value = this._ImportColumns[i].TableAlias;
                        R_Alias.Cells[i].Value = this._ImportColumns[i].ColumnName;
                    }
                }
            }
            catch (System.IO.IOException IOex)
            {
                System.Windows.Forms.MessageBox.Show(IOex.Message);
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return this.ColumnMappingComplete;
        }

        private void checkBoxUseColumnMapping_CheckedChanged(object sender, EventArgs e)
        {
            if (!this._IsReimport)
            {
                this._UseColumnMapping = true;
                this.tabControlPresettings.TabPages.Add(this.tabPageColumnMapping);
                if (!this.tabControlPresettings.TabPages.Contains(this.tabPageUnits))
                    this.tabControlPresettings.TabPages.Add(this.tabPageUnits);
                if (!this.tabControlPresettings.TabPages.Contains(this.tabPageSpecimen))
                    this.tabControlPresettings.TabPages.Add(this.tabPageSpecimen);
                if (!this.tabControlPresettings.TabPages.Contains(this.tabPageEvent))
                    this.tabControlPresettings.TabPages.Add(this.tabPageEvent);
                this.tabControlPresettings.SelectTab(this.tabPageColumnMapping);
                this.tabControlPresettings.TabPages.Remove(this.tabPageFile);
                this.InitColumnMapping();
                //this.buttonAnalyseStructure.Enabled = false;
                this.buttonAnalyse.Enabled = true;
                this.toolStripNavigation.Enabled = true;
            }
            else
            {
                this._UseColumnMapping = false;
                this.tabControlPresettings.TabPages.Remove(this.tabPageColumnMapping);
                this.tabControlPresettings.TabPages.Remove(this.tabPageSpecimen);
                this.tabControlPresettings.TabPages.Remove(this.tabPageEvent);
                this.tabControlPresettings.TabPages.Remove(this.tabPageUnits);
                this.tabControlPresettings.TabPages.Add(this.tabPageFile);
                //this.buttonAnalyseStructure.Enabled = true;
            }
        }

        private void dataGridViewColumnMapping_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = this.dataGridViewColumnMapping.SelectedCells[0].ColumnIndex;
            if (this.ImportColumns.Count > i)
            {
                this.comboBoxColumnMappingTable.Text = this._ImportColumns[i].TableName;
                this.comboBoxColumnMappingColumn.Text = this._ImportColumns[i].ColumnName;
                this.comboBoxColumnMappingAlias.Text = this._ImportColumns[i].TableAlias;
            }
            else
            {
                if (this.dataGridViewColumnMapping.Rows[0].Cells[i].Value != null)
                    this.comboBoxColumnMappingTable.Text = this.dataGridViewColumnMapping.Rows[0].Cells[i].Value.ToString();
                else
                    this.comboBoxColumnMappingTable.Text = "";

                if (this.dataGridViewColumnMapping.Rows[1].Cells[i].Value != null)
                    this.comboBoxColumnMappingAlias.Text = this.dataGridViewColumnMapping.Rows[1].Cells[i].Value.ToString();
                else
                    this.comboBoxColumnMappingAlias.Text = "";

                if (this.dataGridViewColumnMapping.Rows[2].Cells[i].Value != null)
                    this.comboBoxColumnMappingColumn.Text = this.dataGridViewColumnMapping.Rows[2].Cells[i].Value.ToString();
                else
                    this.comboBoxColumnMappingColumn.Text = "";
            }
            System.Windows.Forms.DataGridViewCell C0 = this.dataGridViewColumnMappingSource.Rows[0].Cells[i];
            C0.Selected = true;
        }

        private void dataGridViewColumnMappingSource_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int i = this.dataGridViewColumnMappingSource.SelectedCells[0].ColumnIndex;
                if (i > this._ImportColumns.Count) return;
                this.comboBoxColumnMappingTable.Text = this._ImportColumns[i].TableName;
                this.comboBoxColumnMappingColumn.Text = this._ImportColumns[i].ColumnName;
                this.comboBoxColumnMappingAlias.Text = this._ImportColumns[i].TableAlias;
                System.Windows.Forms.DataGridViewCell C0 = this.dataGridViewColumnMapping.Rows[0].Cells[i];
                C0.Selected = true;
            }
            catch { }
        }

        private void dataGridViewColumnMappingSource_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //int i = this.dataGridViewColumnMappingSource.SelectedCells[0].ColumnIndex;
            //this.comboBoxColumnMappingTable.Text = this._TableList[i];
            //this.comboBoxColumnMappingColumn.Text = this._ColumnList[i];
            //this.textBoxColumnMappingAlias.Text = this._AliasList[i];
            //System.Windows.Forms.DataGridViewCell C0 = this.dataGridViewColumnMapping.Rows[0].Cells[i];
            ////System.Windows.Forms.DataGridViewCell C1 = this.dataGridViewColumnMapping.Rows[1].Cells[i];
            ////System.Windows.Forms.DataGridViewCell C2 = this.dataGridViewColumnMapping.Rows[2].Cells[i];
            //C0.Selected = true;
            ////C1.Selected = true;
            ////C2.Selected = true;
        }

        private void numericUpDownDataStartInLine_ValueChanged(object sender, EventArgs e)
        {
            for (int iLine = 0; iLine < this.dataGridViewColumnMappingSource.Rows.Count; iLine++)
            {
                if (iLine + 1 < (int)this.numericUpDownDataStartInLine.Value) this.dataGridViewColumnMappingSource.Rows[iLine].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                else this.dataGridViewColumnMappingSource.Rows[iLine].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            }
        }

        private void comboBoxColumnMappingTable_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBoxColumnMappingTable.SelectedItem != null)
            {
                string Table = this.comboBoxColumnMappingTable.SelectedItem.ToString();
                this.comboBoxColumnMappingTable.Text = Table;
                this.setColumnMappingColumnSource(Table);
                //if (this.textBoxColumnMappingAlias.Text.Length == 0) 
                this.comboBoxColumnMappingAlias.Text = Table;
            }
        }

        private void comboBoxColumnMappingColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {

            this.comboBoxColumnMappingColumn.Text = this.comboBoxColumnMappingColumn.SelectedValue.ToString();
            this.buttonColumnMappingSave_Click(null, null);
        }

        private void comboBoxColumnMappingTable_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxColumnGroup.BackColor == System.Drawing.Color.Black)
            {
                this.comboBoxColumnGroup.SelectedIndex = 0;
                this.comboBoxColumnGroup_SelectionChangeCommitted(null, null);
            }
            this.setColumnMappingTableSource();
        }

        private void comboBoxColumnMappingAlias_DropDown(object sender, EventArgs e)
        {
            this.comboBoxColumnMappingAlias.Items.Clear();
            if (this.CurrentImportColumnGroup.GroupColor != System.Drawing.Color.White
                && this.CurrentImportColumnGroup.TableAlias != null)
                this.comboBoxColumnMappingAlias.Items.Add(this.CurrentImportColumnGroup.TableAlias);
            else
            {
                System.Collections.Generic.List<string> AliasList = new List<string>();
                for (int i = 0; i < this.dataGridViewColumnMapping.Columns.Count; i++)
                {
                    if (this.dataGridViewColumnMapping.Rows[0].Cells[i].Value != null)
                    {
                        if (this.dataGridViewColumnMapping.Rows[0].Cells[i].Value.ToString()
                            == this.comboBoxColumnMappingTable.Text)
                        {
                            string Alias = this.dataGridViewColumnMapping.Rows[1].Cells[i].Value.ToString();
                            if (!AliasList.Contains(Alias))
                                AliasList.Add(Alias);
                        }
                    }
                }
                if (AliasList.Count > 0)
                    foreach (string s in AliasList) this.comboBoxColumnMappingAlias.Items.Add(s);
            }
        }

        private void buttonColumnMappingSave_Click(object sender, EventArgs e)
        {
            this.setColumnMappingListValues();
            if (this.ColumnMappingComplete)
            {
                this.buttonAnalyse.Enabled = true;
                this.toolStripNavigation.Enabled = true;
            }
            else
            {
                this.buttonAnalyse.Enabled = false;
                this.toolStripNavigation.Enabled = false;
            }
            //bool OK = this.ColumnMappingComplete;

            //if (OK && this.ReadyForImportAndNoMissingEntries())
            //    this.buttonStartImport.Enabled = true;
            ////this.buttonAnalyse.Enabled = true;
            //else
            //    this.buttonStartImport.Enabled = false;
            //    //this.buttonAnalyse.Enabled = false;
        }

        private bool ColumnMappingComplete
        {
            get
            {
                bool OK = true;
                for(int i = 0; i < this.dataGridViewColumnMapping.Columns.Count; i++)
                {
                    if (this.dataGridViewColumnMapping.Rows[0].Cells[i].Style.BackColor == System.Drawing.Color.Black) 
                        continue;
                    if (this.dataGridViewColumnMapping.Rows[0].Cells[i].Value == null
                        || this.dataGridViewColumnMapping.Rows[1].Cells[i].Value == null
                        || this.dataGridViewColumnMapping.Rows[2].Cells[i].Value == null)
                        return false;
                }
                return OK;
            }
        }

        private void setColumnMappingListValues()
        {
            System.Windows.Forms.DataGridViewCellStyle Style = new DataGridViewCellStyle();
            if (this.comboBoxColumnGroup.SelectedIndex > -1)
                Style.BackColor = this._GroupColorList[this.comboBoxColumnGroup.SelectedIndex];
            else
                Style.BackColor = System.Drawing.Color.White;
            int i = this.dataGridViewColumnMappingSource.SelectedCells[0].ColumnIndex;
            System.Windows.Forms.DataGridViewRow R_Table = this.dataGridViewColumnMapping.Rows[0];
            System.Windows.Forms.DataGridViewRow R_Alias = this.dataGridViewColumnMapping.Rows[1];
            System.Windows.Forms.DataGridViewRow R_Column = this.dataGridViewColumnMapping.Rows[2];
            R_Table.Cells[i].Value = this.comboBoxColumnMappingTable.Text;
            R_Column.Cells[i].Value = this.comboBoxColumnMappingColumn.Text;
            R_Alias.Cells[i].Value = this.comboBoxColumnMappingAlias.Text; // No group
            if (Style.BackColor == System.Drawing.Color.White
                || Style.BackColor == System.Drawing.Color.Black)
            {
                R_Table.Cells[i].Style = Style;
                R_Column.Cells[i].Style = Style;
                R_Alias.Cells[i].Style = Style;
            }
            else // A group
            {
                if (this.ImportColumnColorGroup(this.comboBoxColumnMappingTable.Text, this.comboBoxColumnMappingAlias.Text, this.comboBoxColumnMappingColumn.Text) 
                    == System.Drawing.Color.White) // this definition is missing so far
                {
                    if (!this.ImportColumnGroupExists(Style.BackColor))
                    {
                        DiversityWorkbench.FormGetString f = new DiversityWorkbench.FormGetString("Separator for new group", "Please type the separator that shoul be used for the contatenation of the column values", " ");
                        f.ShowDialog();
                        if (f.DialogResult == DialogResult.OK)
                        {
                            R_Table.Cells[i].Style = Style;
                            R_Column.Cells[i].Style = Style;
                            R_Alias.Cells[i].Style = Style;
                            DiversityCollection.ImportColumnGroup ICG = new ImportColumnGroup();
                            ICG.TableName = this.comboBoxColumnMappingTable.Text;
                            ICG.TableAlias = this.comboBoxColumnMappingAlias.Text;
                            ICG.ColumnName = this.comboBoxColumnMappingColumn.Text;
                            ICG.GroupColor = Style.BackColor;
                            ICG.Separator = f.String;
                            this._ImportColumnGroupList.Add(ICG);
                        }
                        else
                        {
                            this.comboBoxColumnGroup.SelectedIndex = 0;
                            this.comboBoxColumnGroup.BackColor = System.Drawing.Color.White;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("The definition for this column does not match the already existing definition");
                    }
                }
                else if (this.ImportColumnColorGroup(this.comboBoxColumnMappingTable.Text, this.comboBoxColumnMappingAlias.Text, this.comboBoxColumnMappingColumn.Text) 
                    != Style.BackColor)
                {
                    System.Windows.Forms.MessageBox.Show("The definition for this column does not match the already existing definition");
                }
                else
                {
                    R_Table.Cells[i].Style = Style;
                    R_Column.Cells[i].Style = Style;
                    R_Alias.Cells[i].Style = Style;
                }
            }
        }

        private void setColumnMappingTableSource()
        {
            this.comboBoxColumnMappingTable.Items.Clear();
            string SQL = "SELECT TABLE_NAME FROM Information_Schema.Tables " +
                "WHERE TABLE_TYPE = 'BASE TABLE' " +
                "AND TABLE_NAME NOT LIKE '%_Enum' " +
                "AND TABLE_NAME NOT LIKE '%_log' " +
                "AND TABLE_NAME NOT LIKE '%_log_%' " +
                "AND TABLE_NAME NOT LIKE 'xx_%' " +
                "AND TABLE_NAME NOT LIKE '%Proxy' " +
                "AND TABLE_NAME NOT IN ( " +
                "'Analysis', " +
                "'AnalysisTaxonomicGroup', " +
                "'ApplicationEntityDescription', " +
                "'ApplicationSearchSelectionStrings', " +
                "'Collection', " +
                "'CollectionManager', " +
                "'dtproperties', " +
                "'ExternalRequestCredentials ', " +
                "'LocalisationSystem', " +
                "'Processing', " +
                "'ProcessingMaterialCategory', " +
                "'ProjectUser', " +
                "'Property', " +
                "'sysdiagrams', " +
                "'Transaction', " +
                "'TransactionDocument') " +
                "ORDER BY Information_Schema.Tables.TABLE_NAME";
            System.Data.DataTable dt = new DataTable();
            System.Data.SqlClient.SqlDataAdapter a = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                a.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string Table = "";
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Table = R[0].ToString();
                        if (this.CurrentImportColumnGroup.GroupColor != System.Drawing.Color.White
                            && Table != this.CurrentImportColumnGroup.TableName
                            && this.CurrentImportColumnGroup.TableName != null)
                            continue;
                        SQL = "IF PERMISSIONS(OBJECT_ID('" + Table + "'))&8=8 SELECT 'True' ELSE SELECT 'False'";
                        System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                        System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                        con.Open();
                        if (C.ExecuteScalar().ToString() == "True")
                            this.comboBoxColumnMappingTable.Items.Add(Table);
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void setColumnMappingColumnSource(string TableName)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT COLUMN_NAME as ColumnName " +
                "FROM Information_Schema.COLUMNS  " +
                "WHERE Information_Schema.COLUMNS.TABLE_NAME = '" + TableName + "' AND Information_Schema.COLUMNS.COLUMN_NAME NOT LIKE 'LOG%'";
            if (this.CurrentImportColumnGroup.GroupColor != System.Drawing.Color.White
                && this.CurrentImportColumnGroup.GroupColor != System.Drawing.Color.Black
                && this.CurrentImportColumnGroup.GroupColor != null
                && this.CurrentImportColumnGroup.ColumnName != null)
                SQL += " AND Information_Schema.COLUMNS.COLUMN_NAME = '" + this.CurrentImportColumnGroup.ColumnName + "'";
            System.Data.SqlClient.SqlDataAdapter a = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            try
            {
                a.Fill(dt);
                this.comboBoxColumnMappingColumn.DataSource = dt;
                this.comboBoxColumnMappingColumn.DisplayMember = "ColumnName";
                this.comboBoxColumnMappingColumn.ValueMember = "ColumnName";
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void WriteColumnMappingToImportColumns()
        {
            //this._ImportColumnDictionary = new Dictionary<string,ImportColumn>();
            this._ImportColumns = new List<ImportColumn>();
            for (int i = 0; i < this.dataGridViewColumnMapping.Columns.Count; i++)
            {
                // No import of black columns
                if (this.dataGridViewColumnMapping.Rows[0].Cells[i].Style.BackColor == System.Drawing.Color.Black) 
                    continue;

                System.Windows.Forms.DataGridViewRow R_Table = this.dataGridViewColumnMapping.Rows[0];
                System.Windows.Forms.DataGridViewRow R_Alias = this.dataGridViewColumnMapping.Rows[1];
                System.Windows.Forms.DataGridViewRow R_Column = this.dataGridViewColumnMapping.Rows[2];
                DiversityCollection.ImportColumn I = new ImportColumn();
                if (R_Alias.Cells[i].Value == null || R_Table.Cells[i].Value == null || R_Column.Cells[i].Value == null)
                    return;
                I.TableAlias = R_Alias.Cells[i].Value.ToString();
                I.TableName = R_Table.Cells[i].Value.ToString();
                I.ColumnName = R_Column.Cells[i].Value.ToString();
                bool ImportColumnInList = false;
                {
                    foreach(DiversityCollection.ImportColumn IC in this._ImportColumns)
                    {
                        if (IC.ColumnName == I.ColumnName && IC.TableAlias == I.TableAlias && IC.TableName == I.TableName)
                        {
                            ImportColumnInList = true;
                            break;
                        }
                    }
                }
                if (ImportColumnInList
                    && this.ImportColumnColorGroup(I.TableName, I.TableAlias, I.ColumnName) != System.Drawing.Color.White)
                    continue;
                //this._ImportColumnDictionary.Add(I.TableName + "." + I.TableAlias + "." + I.ColumnName, I);
                this._ImportColumns.Add(I);
            }
        }

        #region Groups

        private void initGroupColors()
        {
            this._GroupColorList.Add(System.Drawing.Color.White);
            this._GroupColorList.Add(System.Drawing.Color.Yellow);
            this._GroupColorList.Add(System.Drawing.Color.Red);
            this._GroupColorList.Add(System.Drawing.Color.Lime);
            this._GroupColorList.Add(System.Drawing.Color.Aqua);
            this._GroupColorList.Add(System.Drawing.Color.Orange);
            this._GroupColorList.Add(System.Drawing.Color.Magenta);
            this._GroupColorList.Add(System.Drawing.Color.Black);
            foreach (System.Drawing.Color C in this._GroupColorList)
            {
                //this.GroupColorDict.Add(C.Name, C);
                this.comboBoxColumnGroup.Items.Add(C.Name);
            }
            DiversityCollection.ImportColumnGroup I = new ImportColumnGroup();
            I.GroupColor = System.Drawing.Color.Black;
            this._ImportColumnGroupList.Add(I);
        }

        private DiversityCollection.ImportColumnGroup ImportColumnGroup(System.Drawing.Color GroupColor)
        {
            if (this._ImportColumnGroupList.Count > 0)
            {
                foreach (DiversityCollection.ImportColumnGroup ICG in this._ImportColumnGroupList)
                {
                    if (ICG.GroupColor == GroupColor)
                        return ICG;
                }
            }
            DiversityCollection.ImportColumnGroup I = new ImportColumnGroup();
            return I;
        }

        private DiversityCollection.ImportColumnGroup CurrentImportColumnGroup
        {
            get
            {
                DiversityCollection.ImportColumnGroup I = new ImportColumnGroup();
                if (this.comboBoxColumnGroup.SelectedIndex > 0)
                {
                    return this.ImportColumnGroup(this._GroupColorList[this.comboBoxColumnGroup.SelectedIndex]);
                }
                I.GroupColor = System.Drawing.Color.White;
                return I;
            }
        }

        private bool ImportColumnGroupMatch(System.Drawing.Color GroupColor, string Table, string Alias, string Column)
        {
            if (this._ImportColumnGroupList.Count > 0)
            {
                foreach (DiversityCollection.ImportColumnGroup ICG in this._ImportColumnGroupList)
                {
                    if (ICG.GroupColor == GroupColor 
                        && ICG.TableName == Table 
                        && ICG.TableAlias == Alias 
                        && ICG.ColumnName == Column)
                        return true;
                }
            }
            return false;
        }

        private bool ImportColumnGroupExists(System.Drawing.Color GroupColor)
        {
            System.Collections.Generic.List<DiversityCollection.ImportColumnGroup> ICGtoDelete = new List<ImportColumnGroup>();
            if (this._ImportColumnGroupList.Count > 0)
            {
                foreach (DiversityCollection.ImportColumnGroup ICG in this._ImportColumnGroupList)
                {
                    if (ICG.GroupColor == GroupColor)
                    {
                        foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridViewColumnMapping.Rows[0].Cells)
                        {
                            if (C.Style.BackColor == GroupColor)
                                return true;
                        }
                        ICGtoDelete.Add(ICG);
                    }
                }
            }
            if (ICGtoDelete.Count > 0)
            {
                foreach (DiversityCollection.ImportColumnGroup I in ICGtoDelete)
                {
                    if (I.GroupColor != System.Drawing.Color.Black)
                        this._ImportColumnGroupList.Remove(I);
                }
                ICGtoDelete.Clear();
            }
            return false;
        }

        private System.Drawing.Color ImportColumnColorGroup(string Table, string Alias, string Column)
        {
            if (this._ImportColumnGroupList.Count > 0)
            {
                foreach (DiversityCollection.ImportColumnGroup ICG in this._ImportColumnGroupList)
                {
                    if (ICG.TableName == Table && ICG.TableAlias == Alias && ICG.ColumnName == Column)
                        return ICG.GroupColor;
                }
            }
            return System.Drawing.Color.White;
        }

        private void comboBoxColumnGroup_DrawItem(object sender, DrawItemEventArgs e)
        {

            Rectangle rectangle = new Rectangle(2, e.Bounds.Top + 2,
                    e.Bounds.Height, e.Bounds.Height - 4);
            string Color = this._GroupColorList[e.Index].Name;
            e.Graphics.FillRectangle(new SolidBrush(this._GroupColorList[e.Index]), rectangle);
        }

        private void comboBoxColumnGroup_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            //switch (e.Index)
            //{
            //    case 0:
            //        e.ItemHeight = 45;
            //        break;
            //    case 1:
            //        e.ItemHeight = 20;
            //        break;
            //    case 2:
            //        e.ItemHeight = 35;
            //        break;
            //}
            //e.ItemWidth = 260;

        }
        
        private void comboBoxColumnGroup_DropDown(object sender, EventArgs e)
        {
            this.comboBoxColumnGroup.BackColor = System.Drawing.Color.White;
        }

        private void comboBoxColumnGroup_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.comboBoxColumnGroup.BackColor = this._GroupColorList[this.comboBoxColumnGroup.SelectedIndex];
        }

        private void buttonIgnoreColumn_Click(object sender, EventArgs e)
        {
            this.comboBoxColumnGroup.SelectedIndex = this._GroupColorList.Count - 1;
            this.comboBoxColumnGroup_SelectionChangeCommitted(null, null);
            //this.comboBoxColumnGroup.BackColor = System.Drawing.Color.Black;
            this.buttonColumnMappingSave_Click(null, null);
        }

        #endregion

        #endregion

        #region Analyse data and build tree

        private void buttonResetAll_Click(object sender, EventArgs e)
        {
            this.ResetAll();
        }

        private void ResetAll()
        {
            this.resetAnalysis();
            this.ImportColumns = null;
            this.ImportColumnsAddOn = null;

            if (this._DatasetList != null)
                this._DatasetList.Clear();
            this.treeViewAnalysis.Nodes.Clear();
            this.dataGridViewColumnMapping.Columns.Clear();
            this.dataGridViewColumnMappingSource.Columns.Clear();
            this.dataGridViewFile.Columns.Clear();
        }

        private bool WriteHeaderLinesToImportColumnsAndCheck()
        {
            bool OK = true;
            if (this.textBoxImportFile.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a file");
                return false;
            }
            this._ImportColumns = new List<ImportColumn>();
            DiversityCollection.Datasets.DataSetCollectionSpecimen ds = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            try
            {
                System.Collections.Generic.List<string> Tables = new List<string>();
                System.Collections.Generic.List<string> Aliases = new List<string>();
                System.Collections.Generic.List<string> Columns = new List<string>();
                System.IO.StreamReader sr = this.StreamReader(this.textBoxImportFile.Text);
                using (sr)
                {
                    String line;
                    int i = 0;
                    while ((line = sr.ReadLine()) != null && i < 7)
                    {
                        if (i == 2) // tables
                        {
                            while (line.Length > 0)
                            {
                                if (line.IndexOf("\t") > -1)
                                    Tables.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim());
                                else
                                {
                                    Tables.Add(line.Trim());
                                    line = "";
                                }
                                line = line.Substring(line.IndexOf("\t") + 1);
                            }
                        }
                        if (i == 3) // columns
                        {
                            while (line.Length > 0)
                            {
                                if (line.IndexOf("\t") > -1)
                                    Columns.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim());
                                else
                                {
                                    Columns.Add(line.Trim());
                                    line = "";
                                }
                                line = line.Substring(line.IndexOf("\t") + 1);
                            }
                        }
                        if (i == 4) // alias
                        {
                            while (line.Length > 0)
                            {
                                if (line.IndexOf("\t") > -1)
                                    Aliases.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim());
                                else
                                {
                                    Aliases.Add(line.Trim());
                                    line = "";
                                }
                                line = line.Substring(line.IndexOf("\t") + 1);
                            }
                            for (int iA = 0; iA < Aliases.Count; iA++)
                                if (Aliases[iA].Length == 0)
                                    Aliases[iA] = Tables[iA];
                        }
                        i++;
                    }
                    int c = Tables.Count;
                    if (Aliases.Count != c || Columns.Count != c)// || this.ContentList.Count != c)
                    {
                        System.Windows.Forms.MessageBox.Show("Error in table definiton of file");
                        return false;
                    }
                    if (ds == null) ds = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
                    // check if the definitions in the header line are consistent with the tables in the database
                    for (int iC = 0; iC < this._ImportColumns.Count; iC++)
                    {
                        try
                        {
                            if (!ds.Tables[this._ImportColumns[iC].TableName].Columns.Contains(this._ImportColumns[iC].ColumnName))
                            {
                                System.Windows.Forms.MessageBox.Show("The table \r\n\t" + this._ImportColumns[iC].TableName
                                + "\r\ndoes contain no column\r\n\t" + this._ImportColumns[iC].ColumnName
                                + "\r\n\r\nPlease correct definitions in the import file");
                                return false;
                            }
                        }
                        catch
                        {
                            System.Windows.Forms.MessageBox.Show("Error in table definition");
                            return false;
                        }
                    }
                }
                if (Aliases.Count > 0)
                {
                    for (int i = 0; i < Aliases.Count; i++)
                    {
                        DiversityCollection.ImportColumn I = new ImportColumn();
                        I.TableAlias = Aliases[i];
                        string Table = Tables[i];
                        if (Table.EndsWith("_Core")) Table = Table.Replace("_Core", "");
                        I.TableName = Table;
                        I.ColumnName = Columns[i];
                        this._ImportColumns.Add(I);
                    }
                    return true;
                }
            }
            catch (System.IO.IOException IOex)
            {
                System.Windows.Forms.MessageBox.Show(IOex.Message);
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return OK;
        }
        /// <summary>
        /// analysis of the import file and filling of the list for tables etc.
        /// </summary>
        private bool analyseImportFile()
        {
            bool OK = true;
            if (this.textBoxImportFile.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a file");
                return false;
            }
            DiversityCollection.Datasets.DataSetCollectionSpecimen ds = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            this.resetAnalysis();
            if (!!this._IsReimport)
            {
                this._ImportColumns.Clear();
                if (this.dataGridViewFile.Columns.Count == 0)
                    this.readFileForPreview();
            }
            if (this._ImportColumnsAddOn != null)
                this._ImportColumnsAddOn.Clear();
            if (this._DatasetList != null)
                this._DatasetList.Clear();
            try
            {
                if (this._UseColumnMapping)
                {
                    OK = this.InitColumnMapping();
                    if (!this.ColumnMappingComplete)
                    {
                        System.Windows.Forms.MessageBox.Show("Please assign all import columns in the file");
                        this.tabControlPresettings.SelectTab(this.tabPageColumnMapping);
                    }
                }
                else
                {
                    System.IO.StreamReader sr = this.StreamReader(this.textBoxImportFile.Text);
                    using (sr)
                    {
                        String line;
                        int i = 0;
                        System.Collections.Generic.List<string> Tables = new List<string>();
                        System.Collections.Generic.List<string> Aliases = new List<string>();
                        System.Collections.Generic.List<string> Columns = new List<string>();
                        while ((line = sr.ReadLine()) != null && i < 7)
                        {
                            if (i == 2) // tables
                            {
                                while (line.Length > 0)
                                {
                                    if (line.IndexOf("\t") > -1)
                                        Tables.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim());
                                    else
                                    {
                                        Tables.Add(line.Trim());
                                        line = "";
                                    }
                                    line = line.Substring(line.IndexOf("\t") + 1);
                                }
                            }
                            if (i == 3) // columns
                            {
                                while (line.Length > 0)
                                {
                                    if (line.IndexOf("\t") > -1)
                                        Columns.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim());
                                    else
                                    {
                                        Columns.Add(line.Trim());
                                        line = "";
                                    }
                                    line = line.Substring(line.IndexOf("\t") + 1);
                                }
                            }
                            if (i == 4) // alias
                            {
                                while (line.Length > 0)
                                {
                                    if (line.IndexOf("\t") > -1)
                                        Aliases.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim());
                                        //this._AliasList.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim());
                                    else
                                    {
                                        Aliases.Add(line.Trim());
                                        //this._AliasList.Add(line.Trim());
                                        line = "";
                                    }
                                    line = line.Substring(line.IndexOf("\t") + 1);
                                }
                                for (int iA = 0; iA < Aliases.Count; iA++)
                                    if (Aliases[iA].Length == 0)
                                        Aliases[iA] = Tables[iA];
                            }
                            i++;
                        }
                        int c = Tables.Count;
                        if (Aliases.Count != c || Columns.Count != c)// || this.ContentList.Count != c)
                        {
                            System.Windows.Forms.MessageBox.Show("Error in table definiton of file");
                            return false;
                        }
                        if (ds == null) ds = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
                        for (int iC = 0; iC < this._ImportColumns.Count; iC++)
                        {
                            try
                            {
                                if (!ds.Tables[this._ImportColumns[iC].TableName].Columns.Contains(this._ImportColumns[iC].ColumnName))
                                {
                                    System.Windows.Forms.MessageBox.Show("The table \r\n\t" + this._ImportColumns[iC].TableName
                                    + "\r\ndoes contain no column\r\n\t" + this._ImportColumns[iC].ColumnName
                                    + "\r\n\r\nPlease correct definitions in the import file");
                                    return false;
                                }
                            }
                            catch
                            {
                                System.Windows.Forms.MessageBox.Show("Error in table definition");
                                return false;
                            }
                        }
                    }
                }
                if (OK)
                {
                    if (!this.ReadyForImportAndNoMissingEntries()) return false;
                    else
                    {
                        this.setUnitInParts();
                        this.analyseData(0);
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (System.IO.IOException IOex)
            {
                System.Windows.Forms.MessageBox.Show(IOex.Message);
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        private bool readFileForPreview()
        {
            if (this.textBoxImportFile.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a file");
                return false;
            }
            System.IO.FileInfo F = new System.IO.FileInfo(this.textBoxImportFile.Text);
            this.tabPageFile.Text = F.Name;
            this.dataGridViewFile.Columns.Clear();
            try
            {
                System.IO.StreamReader sr = this.StreamReader(this.textBoxImportFile.Text);
                using (sr)
                {
                    String line;
                    int iLine = 0;
                    int iColumn = 0;
                    bool HeaderPassed = false;
                    bool EmptyLinePassed = false;
                    int iHeader = 0;
                    // reading the first for lines into the datagrid
                    while ((line = sr.ReadLine()) != null && iLine < (int)this.numericUpDownUpTo.Value + iHeader)
                    {
                        // reading the columns
                        if (iLine == 0)
                        {
                            string Columns = line;
                            while (Columns.Length > 0)
                            {
                                string Column = "";
                                if (Columns.IndexOf("\t") > -1)
                                    Column = Columns.Substring(0, Columns.IndexOf("\t")).ToString().Trim();
                                else
                                {
                                    Column = Columns.Trim();
                                    Columns = "";
                                }
                                iColumn++;
                                this.dataGridViewFile.Columns.Add("Column_" + iColumn.ToString(), "Column " + iColumn.ToString());
                                Columns = Columns.Substring(Columns.IndexOf("\t") + 1);
                            }
                        }
                        // reading the lines
                        this.dataGridViewFile.Rows.Add(1);
                        iColumn = 0;
                        if (!HeaderPassed || line.Replace("\t", "").Trim().Length == 0)
                        {
                            // skip the header lines
                            if (line.Replace("\t", "").Trim().Length == 0 && iHeader > 1)
                                EmptyLinePassed = true;
                            iHeader++;
                            if (iHeader > 3 && EmptyLinePassed)
                                HeaderPassed = true;
                            this.dataGridViewFile.Rows[iLine].DefaultCellStyle.BackColor = System.Drawing.SystemColors.Control;
                        }
                        while (line.Length > 0)
                        {
                            if (line.Replace("\t", "").Trim().Length == 0)
                                line = "";
                            else if (line.IndexOf("\t") > -1)
                                this.dataGridViewFile.Rows[iLine].Cells[iColumn].Value = line.Substring(0, line.IndexOf("\t")).ToString().Trim();
                            else
                            {
                                this.dataGridViewFile.Rows[iLine].Cells[iColumn].Value = line.Trim();
                                line = "";
                            }
                            line = line.Substring(line.IndexOf("\t") + 1);
                            iColumn++;
                        }
                        iLine++;
                    }
                }
            }
            catch (System.IO.IOException IOex)
            {
                System.Windows.Forms.MessageBox.Show(IOex.Message);
                return false;
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }

        private System.Collections.Generic.List<string> Dataline(string line)
        {
            System.Collections.Generic.List<string> Line = new List<string>();
            if (line.Replace("\t", "").Trim().Length > 0)
            {
                while (line.Length > 0)
                {
                    if (line.IndexOf("\t") > -1)
                        Line.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim());
                    else
                    {
                        Line.Add(line.Trim());
                        line = "";
                    }
                    line = line.Substring(line.IndexOf("\t") + 1);
                }
            }
            return Line;
        }

        private bool analyseData(int Position, int UpToPosition)
        {
            //this.DatasetList(UpToPosition);
            if (this._DatasetList == null)
                this.ReadDataFromFileToDatasetList((int?)UpToPosition);
            return this.analyseData(Position);
        }

        /// <summary>
        /// Analysing the data of the file at a certain position
        /// first clear the tree view
        /// than analyse the leading line of the import file to get the structure of the data
        /// than read the presettings and create lists to attach to the data of the file
        /// than read the data and create the tree
        /// </summary>
        /// <param name="Position">the line within the file starting a the first line containing data</param>
        private bool analyseData(int Position)
        {
            this.treeViewAnalysis.Nodes.Clear();

            // init the dataset
            if (this._DatasetList == null)
            {
                this.ReadDataFromFileToDatasetList((int?)Position);
            }


            System.Collections.Generic.List<string> TablesAliasList = new List<string>();
            if (this._DatasetList.Count > 0)
            {
                string Alias = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                {
                    if (this.ParentUnitAlias.ContainsKey(KV.Key)) continue;
                    //if (this._ParentAlias.ContainsKey(KV.Key)) continue;
                    System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Bold);//  
                    string NodeTitle = KV.Value;
                    if (KV.Value != KV.Key) NodeTitle += " [" + KV.Key + "]";
                    string Spacer = "";
                    for (int iS = 0; iS < NodeTitle.Length; iS++) Spacer += " ";
                    Spacer = Spacer.Substring(0, (int)Spacer.Length / 2);
                    NodeTitle += Spacer;
                    System.Windows.Forms.TreeNode TableNode = new TreeNode(NodeTitle);
                    TableNode.NodeFont = F;
                    if (KV.Value == "IdentificationUnit")
                    {
                        string TaxonomicGroup = "";
                        for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                        {
                            if (this.ImportColumnsAll[i].TableAlias == KV.Key
                                && this.ImportColumnsAll[i].ColumnName == "TaxonomicGroup")
                            {
                                if (this.ImportColumnsAll[i].Content != null)
                                    TaxonomicGroup = this.ImportColumnsAll[i].Content;// this.Dataset()[0][i];
                                if (TaxonomicGroup.Length > 0 && this.Dataset()[0][i] != TaxonomicGroup)
                                    this.Dataset()[0][i] = TaxonomicGroup;
                                if (TaxonomicGroup.Length > 0) 
                                    break;
                            }
                        }
                        if (TaxonomicGroup.Length > 0)
                        {
                            TableNode.ImageIndex = DiversityCollection.Specimen.TaxonImage(TaxonomicGroup, false);
                            TableNode.SelectedImageIndex = DiversityCollection.Specimen.TaxonImage(TaxonomicGroup, false);
                        }
                        else
                        {
                            TableNode.ImageIndex = DiversityCollection.Specimen.TableImage(KV.Value, false);
                            TableNode.SelectedImageIndex = DiversityCollection.Specimen.TableImage(KV.Value, false);
                        }
                    }
                    else if (KV.Value == "CollectionSpecimenPart")
                    {
                        string MaterialCategory = "";
                        for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                        {
                            if (this.ImportColumnsAll[i].TableAlias == KV.Key
                                && this.ImportColumnsAll[i].ColumnName == "MaterialCategory")
                                MaterialCategory = this.Dataset()[0][i];
                        }
                        if (MaterialCategory.Length > 0)
                        {
                            TableNode.ImageIndex = DiversityCollection.Specimen.MaterialCategoryImage(MaterialCategory, false);
                            TableNode.SelectedImageIndex = DiversityCollection.Specimen.TaxonImage(MaterialCategory, false);
                        }
                        else
                        {
                            TableNode.ImageIndex = DiversityCollection.Specimen.TableImage(KV.Value, false);
                            TableNode.SelectedImageIndex = DiversityCollection.Specimen.TableImage(KV.Value, false);
                        }
                    }
                    else
                    {
                        TableNode.ImageIndex = DiversityCollection.Specimen.TableImage(KV.Value, false);
                        TableNode.SelectedImageIndex = DiversityCollection.Specimen.TableImage(KV.Value, false);
                    }
                    if (this.getNodeData(Position, TableNode, KV.Value, KV.Key))
                    {
                        this.treeViewAnalysis.Nodes.Add(TableNode);
                        if (this.ParentUnitAlias.ContainsValue(KV.Key))
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KvP in this.ParentUnitAlias)
                            {
                                if (KvP.Value == KV.Key)
                                {
                                    string Table = this.AliasDictionary[KvP.Key];
                                    NodeTitle = Table;
                                    if (NodeTitle != KvP.Key) NodeTitle += " [" + KvP.Key + "]";
                                    Spacer = "";
                                    for (int iS = 0; iS < NodeTitle.Length; iS++) Spacer += " ";
                                    Spacer = Spacer.Substring(0, (int)Spacer.Length / 2);
                                    NodeTitle += Spacer;
                                    System.Windows.Forms.TreeNode ChildNode = new TreeNode(NodeTitle);
                                    ChildNode.NodeFont = F;
                                    if (this.getNodeData(Position, ChildNode, Table, KvP.Key))
                                    {
                                        TableNode.Nodes.Add(ChildNode);
                                    }
                                }
                            }
                        }
                    }
                }
                this.treeViewAnalysis.ExpandAll();
                return true;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("No data found");
                return false;
            }
        }

        #endregion

        #region Query Data: Columns of a table, PK, content, ...
        /// <summary>
        /// adding the missing PK columns to the tables
        /// </summary>
        private bool DataContainPK(string TableAlias)
        {
            bool ContainsPK = true;
            string Table = this.TableOfAlias(TableAlias);
            if (Table.Length == 0) Table = TableAlias;
            System.Collections.Generic.List<string> PK = this.getPrimaryKey(Table);
            foreach (string s in PK)
            {
                //ContainsPK = true;
                bool OK = false;
                foreach (DiversityCollection.ImportColumn I in this._ImportColumns)
                {
                    if (I.TableAlias == TableAlias && I.ColumnName == s)
                    {
                        OK = true;
                        break;
                    }
                }
                //for (int P = 0; P < this._AliasList.Count; P++)
                //{
                //    if (TableAlias == this._AliasList[P] && this._ColumnList[P] == s)
                //    {
                //        OK = true;
                //        break;
                //    }
                //}
                if (!OK)
                {
                    foreach (DiversityCollection.ImportColumn I in this._ImportColumnsAddOn)
                    {
                        if (TableAlias == I.TableAlias && I.ColumnName == s)
                        {
                            OK = true;
                            break;
                        }
                    }
                }
                //if (!OK)
                //{
                //    for (int P = 0; P < this._AliasListAddOn.Count; P++)
                //    {
                //        if (TableAlias == this._AliasListAddOn[P] && this._ColumnListAddOn[P] == s)
                //        {
                //            OK = true;
                //            break;
                //        }
                //    }
                //}
                if (!OK) ContainsPK = false;
            }
            return ContainsPK;
        }

        private System.Collections.Generic.List<string> getTableColumns
            (string TableAlias)
        {
            System.Collections.Generic.List<string> CList = new List<string>();
            foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll )
            {
                if (I.TableAlias == TableAlias)
                { CList.Add(I.ColumnName); }
            }
            //for (int a = 0; a < this.AliasList.Count; a++)
            //{
            //    if (this.AliasList[a] == TableAlias)
            //    { CList.Add(this.ColumnList[a]); }
            //}
            return CList;
        }

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> TableContent(int DatasetPosition, string TableAlias)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _TableContent = new Dictionary<string, List<string>>();
            string TableName = "";
            System.Collections.Generic.List<string> PK = this.getPrimaryKey(TableName);
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
            {
                if (KV.Key == TableAlias)
                {
                    TableName = KV.Value;
                    break;
                }
            }
            //for (int i = 0; i < this._AliasList.Count; i++)
            //{
            //    if (this._AliasList[i] == TableAlias)
            //    {

            //    }
            //}
            return _TableContent;
        }

        private bool isPrimaryKey(string Table, string Column)
        {
            System.Collections.Generic.List<string> PK = getPrimaryKey(Table);
            if (PK.Contains(Column)) return true;
            else return false;
        }
        
         /// <summary>
        /// get the 
        /// </summary>
        /// <param name="Table"></param>
        /// <returns></returns>
        private System.Collections.Generic.List<string> getPrimaryKey(string Table)
        {
            System.Collections.Generic.List<string> PK = new List<string>();
            if (Table.Length > 0)
            {
                try
                {
                    DiversityCollection.Datasets.DataSetCollectionSpecimen ds = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
                    System.Data.DataTable dt = ds.Tables[Table].Copy();
                    System.Data.DataColumn[] dc = dt.PrimaryKey;
                    foreach (System.Data.DataColumn DC in dc)
                    {
                        PK.Add(DC.ColumnName);
                    }
                }
                catch { }
            }
            return PK;
        }

        private bool getNodeData(int DatasetPosition, System.Windows.Forms.TreeNode Node, string Table, string TableAlias)
        {
            bool OK = false;
            if (this._DatasetList.Count < DatasetPosition) return OK;
            try
            {
                System.Drawing.Font F = new Font(System.Drawing.FontFamily.GenericSansSerif, (float)8.25, System.Drawing.FontStyle.Underline);//  
                System.Collections.Generic.List<System.Collections.Generic.List<string>> DataSet =
                    this.getTableData(DatasetPosition, Table, TableAlias);
                if (DataSet.Count > 0)
                {
                    int i = 1;
                    foreach (System.Collections.Generic.List<string> List in DataSet)
                    {
                        bool NoData = false;
                        if (!this.ColumnsContainValues(TableAlias, DatasetPosition, i - 1)) NoData = true;
                        System.Windows.Forms.TreeNode N = new TreeNode("[" + i.ToString() + "]");
                        System.Collections.Generic.List<string> CC = this.getTableColumns(TableAlias);
                        for (int c = 0; c < CC.Count; c++)
                        {
                            if (List.Count > c)
                            {
                                System.Windows.Forms.TreeNode nData = new TreeNode(CC[c] + ": " + List[c]);
                                if (NoData) nData.ForeColor = System.Drawing.Color.Blue;
                                if (this.isPrimaryKey(Table, CC[c]))
                                {
                                    nData.NodeFont = F;
                                    if (List[c].Length == 0 && !NoData)
                                        nData.ForeColor = System.Drawing.Color.Red;
                                }
                                N.Nodes.Add(nData);
                            }
                        }
                        Node.Nodes.Add(N);
                        i++;
                    }
                    OK = true;
                }
            }
            catch { }
            return OK;
        }

        private System.Collections.Generic.List<System.Collections.Generic.List<string>> getTableData
            (int DatasetPosition,
            string Table,
            string TableAlias)
        {
            System.Collections.Generic.List<System.Collections.Generic.List<string>> TableDataSet = new List<List<string>>();
            if (this._DatasetList.Count < DatasetPosition) return TableDataSet;
            try
            {
                System.Collections.Generic.List<string> PKFields = this.getPrimaryKey(Table);
                System.Collections.Generic.List<string> PKContent = new List<string>();
                //for (int d = 0; d < this.Dataset(DatasetPosition, false).Count; d++) // the lines in the dataset
                for (int d = 0; d < this.Dataset(DatasetPosition).Count; d++) // the lines in the dataset
                {
                    string PK = "";
                    for (int a = 0; a < this.ImportColumnsAll.Count; a++) // the columns in the dataset
                    {
                        if (this.ImportColumnsAll[a].TableAlias == TableAlias)
                        {
                            if (PKFields.Contains(this.ImportColumnsAll[a].ColumnName))
                                PK += this.Dataset(DatasetPosition)[d][a] + ".";
                            //PK += this.Dataset(DatasetPosition, false)[d][a] + ".";
                        }
                    }
                    //for (int a = 0; a < this.AliasList.Count; a++) // the columns in the dataset
                    //{
                    //    if (this.AliasList[a] == TableAlias)
                    //    {
                    //        if (PKFields.Contains(this.ColumnList[a]))
                    //            PK += this.Dataset(DatasetPosition, false)[d][a] + ".";
                    //    }
                    //}
                    if (!PKContent.Contains(PK)) // check if an entry with identical PK is allready in the lists
                    {
                        PKContent.Add(PK);
                        System.Collections.Generic.List<string> DataLine = new List<string>();
                        bool DataPresent = false;
                        for (int a = 0; a < this.ImportColumnsAll.Count; a++) // the columns in the dataset
                        {
                            if (this.ImportColumnsAll[a].TableAlias == TableAlias && this.Dataset(DatasetPosition)[d].Count > a)
                            {
                                DataLine.Add(this.Dataset(DatasetPosition)[d][a]);
                                if (this._DatasetList[this._DatasetPosition][d][a] != null
                                    && this._DatasetList[this._DatasetPosition][d][a].Length > 0) 
                                    DataPresent = true;
                                //if (this.Dataset(DatasetPosition)[d][a].Length > 0) DataPresent = true;
                            }
                            //if (this.ImportColumnsAll[a].TableAlias == TableAlias && this.Dataset(DatasetPosition, false)[d].Count > a)
                            //{
                            //    DataLine.Add(this.Dataset(DatasetPosition, false)[d][a]);
                            //    if (this.Dataset(DatasetPosition, false)[d][a].Length > 0) DataPresent = true;
                            //}
                        }
                        //for (int a = 0; a < this.AliasList.Count; a++) // the columns in the dataset
                        //{
                        //    if (this.AliasList[a] == TableAlias && this.Dataset(DatasetPosition, false)[d].Count > a)
                        //    {
                        //        DataLine.Add(this.Dataset(DatasetPosition, false)[d][a]);
                        //        if (this.Dataset(DatasetPosition, false)[d][a].Length > 0) DataPresent = true;
                        //    }
                        //}
                        if (DataPresent)
                        {
                            TableDataSet.Add(DataLine);
                        }
                    }
                }
            }
            catch { }
            return TableDataSet;
        }

       #endregion

        #region Import
        /// <summary>
        /// Import or update the lines of the generated dataset
        /// </summary>
        /// <param name="UpToLine">If only there first lines should be imported</param>
        /// <returns>Message in case of errors</returns>
        private string importData(int? UpToLine)
        {
            int i = 0;
            try
            {
                if (UpToLine == null)
                {
                    try
                    {
                        this._DatasetList = null;
                        if (UpToLine == null)
                            this.ReadDataFromFileToDatasetList(UpToLine);
                        //this.DatasetList(null);
                    }
                    catch (System.Exception ex) { return ""; } 
                }

                this._ErrorMessage = "";
                int _UpTo = this._DatasetList.Count;
                if (UpToLine != null) _UpTo = (int)UpToLine;
                this.progressBarImport.Maximum = _UpTo;
                this.progressBarImport.Value = 0;
                this.progressBarImport.Visible = true;
                for (i = 0; i < _UpTo; i++)
                {
                    this.TransactionBegin("");
                    this._ErrorMessage += this.ImportEventSeries();
                    if (this._ErrorMessage.Length == 0)
                        this._ErrorMessage += this.importEventTables(i);
                    if (this._ErrorMessage.Length == 0)
                        this._ErrorMessage += this.importSpecimenTables(i);
                    if (this._ErrorMessage.Length == 0)
                        this._ErrorMessage += this.importPartTables(i);
                    if (this._ErrorMessage.Length == 0)
                        this._ErrorMessage += this.importIdentificationTables(i);
                    if (this._ErrorMessage.Length > 0)
                    {
                        this.TransactionRollback("");
                        break;
                    }
                    else
                    {
                        this.TransactionCommit("");
                    }
                    this.progressBarImport.Value = i;
                }

                if (this._ErrorMessage.Length == 0)
                    this._ErrorMessage = _UpTo.ToString() + " datasets were imported";
                else
                    this._ErrorMessage = "Import stopped at dataset " + (i + 1).ToString() + "\r\n\r\n" + this._ErrorMessage;
                this.progressBarImport.Visible = false;
                return this._ErrorMessage;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(i.ToString() + " datasets imported, " + ex.Message);
                return ex.Message;
            }
        }

        private string ImportEventSeries()
        {
            string Message = "";
            //if (!this.AliasDictionary.ContainsValue("CollectionEventSeries"))
            //    return "";
            if (!this.checkBoxEventSeries.Checked)
                return "";
            try
            {
                if (this._SeriesID == null)
                {
                    string SQL = "INSERT INTO CollectionEventSeries (SeriesCode, Description, Notes) " +
                        " VALUES ('" + this.textBoxEventSeriesCode.Text + "', '"
                        + this.textBoxEventSeriesDescription.Text + "', '"
                        + this.textBoxEventSeriesNotes.Text + "') " +
                        "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                    this._SeriesID = this.ExecuteQueryInt(SQL);
                }
                if (this._SeriesID != null)
                {
                    this.setValueForAllColumns("SeriesID", this._SeriesID.ToString());
                }
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        /// <summary>
        /// Import of the event tables and if there are any, setting of the CollectionEventID in the line
        /// </summary>
        /// <param name="DatasetLine">The number of the line in the dataset</param>
        /// <returns>The message in case of an error</returns>
        private string importEventTables(int DataSetPosition)
        {
            string Message = "";
            if (!this.AliasDictionary.ContainsValue("CollectionEvent")
                && !this.AliasDictionary.ContainsValue("CollectionEventImage")
                && !this.AliasDictionary.ContainsValue("CollectionEventLocalisation")
                && !this.AliasDictionary.ContainsValue("CollectionEventProperty"))
            {
                return "";
            }
            try
            {
                // try to find an existing CollectionEventID
                string CollectionEventID = this.getValueOfColumn("CollectionEventID", DataSetPosition);
                if (CollectionEventID.Length > 0)
                {
                    this.setValueForAllColumns("CollectionEventID", CollectionEventID, DataSetPosition, this.radioButtonEventsInGroups.Checked);
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                    {
                        if (KV.Value.StartsWith("CollectionEvent"))
                        {
                            for (int i = 0; i < this._DatasetList[DataSetPosition].Count; i++)
                            {
                                this.importOrUpdateTable(KV.Key, DataSetPosition, i);
                            }
                        }
                    }
                }
                else
                {
                    // insert a dataset in CollectionEvent if missing
                    if (!this.AliasDictionary.ContainsValue("CollectionEvent"))
                    {
                        string SQL = "INSERT INTO CollectionEvent (LocalityDescription) VALUES ('?') " +
                            "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                        int? ID = this.ExecuteQueryInt(SQL);
                        if (ID != null)
                        {
                            this.setValueForAllColumns("CollectionEventID", ID.ToString(), DataSetPosition, this.radioButtonEventsInGroups.Checked);
                            //if (this.radioButtonEventsSeparate.Checked)
                            //    this.setValueForAllColumns("CollectionEventID", ID.ToString(), DataSetPosition);
                            //else
                            //    this.setValueForAllColumns("CollectionEventID", ID.ToString(), DataSetPosition, true);
                        }
                    }
                    else
                    {
                        int? ID = this.importOrUpdateTable(this.AliasCollectionEvent, DataSetPosition, 0);
                        if (ID != null)
                        {
                            this.setValueForAllColumns("CollectionEventID", ID.ToString(), DataSetPosition, this.radioButtonEventsInGroups.Checked);

                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                            {
                                if (KV.Value.StartsWith("CollectionEvent") && KV.Value != "CollectionEvent")
                                {
                                    for (int i = 0; i < this._DatasetList[DataSetPosition].Count; i++)
                                    {
                                        this.importOrUpdateTable(KV.Key, DataSetPosition, i);
                                        if (this._ErrorMessage.Length > 0)
                                            return this._ErrorMessage;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        private string importSpecimenTables(int DatasetPosition)
        {
            string Message = "";
            try
            {
                // try to find an existing CollectionEventID
                string CollectionSpecimenID = this.getValueOfColumn("CollectionSpecimenID", DatasetPosition);
                if (CollectionSpecimenID.Length > 0)
                {
                    this.setValueForAllColumns("CollectionSpecimenID", CollectionSpecimenID, DatasetPosition);
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                    {
                        if (!KV.Value.StartsWith("CollectionEvent") && !KV.Value.StartsWith("Identification"))
                        {
                            for (int i = 0; i < this._DatasetList[DatasetPosition].Count; i++)
                            {
                                this.importOrUpdateTable(KV.Key, DatasetPosition, i);
                            }
                        }
                    }
                }
                else
                {
                    // insert a dataset in CollectionSpecimen if missing
                    if (!this.AliasDictionary.ContainsValue("CollectionSpecimen"))
                    {
                        string SQL = "INSERT INTO CollectionSpecimen (Problems) VALUES ('Import without Acc.Nr. by " + System.Environment.UserName + " at " + System.DateTime.Now.ToShortDateString() + "') " +
                            "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                        int? ID = this.ExecuteQueryInt(SQL);
                        if (ID != null)
                        {
                            this.setValueForAllColumns("CollectionSpecimenID", ID.ToString(), DatasetPosition);
                        }
                    }
                    else
                    {
                        int? ID = this.importOrUpdateTable(this.AliasCollectionSpecimen, DatasetPosition, 0);
                        if (ID != null)
                        {
                            this.setValueForAllColumns("CollectionSpecimenID", ID.ToString(), DatasetPosition);
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                            {
                                if (!KV.Value.StartsWith("CollectionEvent") && KV.Value != "CollectionSpecimen" && !KV.Value.StartsWith("Identification") && !KV.Value.StartsWith("CollectionSpecimenPart"))
                                {
                                    for (int i = 0; i < this._DatasetList[DatasetPosition].Count; i++)
                                    {
                                        this.importOrUpdateTable(KV.Key, DatasetPosition, i);
                                        if (this._ErrorMessage.Length > 0)
                                            return this._ErrorMessage;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        private string importIdentificationTables(int DataSetPosition)
        {
            string HostAlias = "";
            string Message = "";
            if (!this.AliasDictionary.ContainsValue("Identification")
                && !this.AliasDictionary.ContainsValue("IdentificationUnit")
                && !this.AliasDictionary.ContainsValue("IdentificationUnitAnalysis")
                && !this.AliasDictionary.ContainsValue("IdentificationUnitInPart"))
            {
                return "";
            }
            try
            {
                // try to find an existing IdentificationUnitID
                System.Collections.Generic.List <string> UnitAliasList = new List<string>();
                if (this.ManyUnitTables)
                {
                    HostAlias = this.textBoxUnitTable2.Text;
                    if (this.radioButtonUnit1isHost.Checked) HostAlias = this.textBoxUnitTable1.Text;
                    UnitAliasList.Add(HostAlias);
                }
                foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                {
                    if (I.TableName == "IdentificationUnit"
                        && !UnitAliasList.Contains(I.TableAlias))
                        UnitAliasList.Add(I.TableAlias);
                }
                string IdentificationUnitID = this.getValueOfColumn("IdentificationUnitID", DataSetPosition);
                if (IdentificationUnitID.Length > 0 && UnitAliasList.Count == 1)
                    this.setValueForAllColumns("IdentificationUnitID", IdentificationUnitID, DataSetPosition);
                else
                {
                    // import the units
                    foreach (string UnitAlias in UnitAliasList)
                    {
                        int? ID = this.importOrUpdateTable(UnitAlias, DataSetPosition, 0);
                        if (ID != null)
                        {
                            string UnitID = ID.ToString();
                            // for each child related to the unit tables like Identification and Anlaysis
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV_ParentChild in this.ParentUnitAlias)
                            {
                                // if there is a child-parent match
                                if (UnitAlias == KV_ParentChild.Value)
                                {
                                    // for each line in the dataset
                                    foreach (System.Collections.Generic.List<string> L in this.Dataset())
                                    {
                                        // writing the UnitID in the dependent tables
                                        for (int iUnitID = 0; iUnitID < this.ImportColumnsAll.Count; iUnitID++)
                                        {
                                            if ((this.ImportColumnsAll[iUnitID].TableAlias == KV_ParentChild.Key
                                                || this.ImportColumnsAll[iUnitID].TableAlias == KV_ParentChild.Value)
                                                && this.ImportColumnsAll[iUnitID].ColumnName == "IdentificationUnitID" )// && L[iUnitID] == "")
                                            {
                                                L[iUnitID] = UnitID;
                                            }
                                        }
                                    }
                                }
                            }
                            if (this.ColumnListAll.Contains("RelatedUnitID") && UnitAlias == HostAlias && UnitID.Length > 0)
                            {
                                int iRelation = this.ColumnListAll.IndexOf("RelatedUnitID");
                                string AliasParasite = this.ImportColumnsAll[iRelation].TableAlias;
                                for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                                {
                                    if (this.ImportColumnsAll[i].ColumnName == "RelatedUnitID"
                                        && this.ImportColumnsAll[i].TableAlias == AliasParasite)// && this.Dataset()[0][i] == "")
                                    {
                                        this.Dataset()[0][i] = UnitID;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                {
                    if (KV.Value.StartsWith("Identification") && KV.Value != "IdentificationUnit")
                    {
                        for (int i = 0; i < this._DatasetList[DataSetPosition].Count; i++)
                        {
                            this.importOrUpdateTable(KV.Key, DataSetPosition, i);
                            if (this._ErrorMessage.Length > 0) 
                                return this._ErrorMessage;
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        private string importPartTables(int DataSetPosition)
        {
            string SourceAlias = "";
            string Message = "";
            if (!this.AliasDictionary.ContainsValue("CollectionSpecimenPart")
                && !this.AliasDictionary.ContainsValue("CollectionSpecimenProcessing")
                && !this.AliasDictionary.ContainsValue("IdentificationUnitInPart"))
            {
                return "";
            }
            try
            {
                // try to find an existing SpecimenPartID
                System.Collections.Generic.List<string> PartAliasList = new List<string>();
                if (this.PartsInData == _PartsInData.two)
                {
                    SourceAlias = this.labelPart2.Text;
                    //if (this.radioButtonUnit1isHost.Checked) SourceAlias = this.textBoxUnitTable1.Text;
                    PartAliasList.Add(SourceAlias);
                }
                foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                {
                    if (I.TableName == "CollectionSpecimenPart"
                        && !PartAliasList.Contains(I.TableAlias))
                        PartAliasList.Add(I.TableAlias);
                }
                string SpecimenPartID = this.getValueOfColumn("SpecimenPartID", DataSetPosition);
                if (SpecimenPartID.Length > 0 && PartAliasList.Count == 1)
                    this.setValueForAllColumns("SpecimenPartID", SpecimenPartID, DataSetPosition);
                else
                {
                    // import the parts
                    foreach (string PartAlias in PartAliasList)
                    {
                        int? ID = this.importOrUpdateTable(PartAlias, DataSetPosition, 0);
                        if (ID != null)
                        {
                            string PartID = ID.ToString();
                            // for each child related to the unit tables like CollectionSpecimenProcessing and CollectionSpecimenTransaction
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KV_ParentChild in this.ParentPartAlias)
                            {
                                // if there is a child-parent match
                                if (PartAlias == KV_ParentChild.Value)
                                {
                                    // for each line in the dataset
                                    foreach (System.Collections.Generic.List<string> L in this.Dataset())
                                    {
                                        // writing the PartID in the dependent tables
                                        for (int iPartID = 0; iPartID < this.ImportColumnsAll.Count; iPartID++)
                                        {
                                            if ((this.ImportColumnsAll[iPartID].TableAlias == KV_ParentChild.Key
                                                || this.ImportColumnsAll[iPartID].TableAlias == KV_ParentChild.Value)
                                                && this.ImportColumnsAll[iPartID].ColumnName == "SpecimenPartID")// && L[iUnitID] == "")
                                            {
                                                L[iPartID] = PartID;
                                            }
                                        }
                                    }
                                }
                            }
                            if (this.ColumnListAll.Contains("DerivedFromSpecimenPartID") && PartAlias == SourceAlias && PartID.Length > 0)
                            {
                                int iRelation = this.ColumnListAll.IndexOf("DerivedFromSpecimenPartID");
                                string AliasParasite = this.ImportColumnsAll[iRelation].TableAlias;
                                for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                                {
                                    if (this.ImportColumnsAll[i].ColumnName == "DerivedFromSpecimenPartID"
                                        && this.ImportColumnsAll[i].TableAlias == AliasParasite)// && this.Dataset()[0][i] == "")
                                    {
                                        this.Dataset()[0][i] = PartID;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                {
                    if (KV.Value.StartsWith("CollectionSpecimenTransaction") && KV.Value != "CollectionSpecimenPart")
                    {
                        for (int i = 0; i < this._DatasetList[DataSetPosition].Count; i++)
                        {
                            this.importOrUpdateTable(KV.Key, DataSetPosition, i);
                            if (this._ErrorMessage.Length > 0)
                                return this._ErrorMessage;
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        /// <summary>
        /// Begin a transaction
        /// </summary>
        /// <param name="Transaction">The name of the transaction - if empty, the standard string is used</param>
        private void TransactionBegin(string Transaction)
        {
            try
            {
                if (Transaction.Length == 0) Transaction = this._Transaction;
                string SQL = "BEGIN TRANSACTION " + Transaction;
                this.ExecuteNonQuery(SQL);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Rollback a transaction
        /// </summary>
        /// <param name="Transaction">The name of the transaction - if empty, the standard string is used</param>
        private void TransactionRollback(string Transaction)
        {
            try
            {
                if (Transaction.Length == 0) Transaction = this._Transaction;
                string SQL = "ROLLBACK TRANSACTION " + Transaction;
                this.ExecuteNonQuery(SQL);
                this._SqlConnection.Close();
                this._SqlConnection.Dispose();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Commit a transaction
        /// </summary>
        /// <param name="Transaction">The name of the transaction - if empty, the standard string is used</param>
        private void TransactionCommit(string Transaction)
        {
            try
            {
                if (Transaction.Length == 0) Transaction = this._Transaction;
                string SQL = "COMMIT TRANSACTION " + Transaction;
                this.ExecuteNonQuery(SQL);
                this._SqlConnection.Close();
                this._SqlConnection.Dispose();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// setting the value for any column of a certain name
        /// </summary>
        /// <param name="TableColumn">the name of the column</param>
        /// <param name="Value">the value</param>
        /// <param name="PositionInDataset">the position in the dataset</param>
        private void setValueForAllColumns(string TableColumn, string Value, int PositionInDataset)
        {
            foreach (System.Collections.Generic.List<string> Line in this._DatasetList[PositionInDataset])
            {
                for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                {
                    if (this.ImportColumnsAll[i].ColumnName == TableColumn)
                        Line[i] = Value;
                }
            }
        }

        private void setValueForAllColumns(string TableColumn, string Value, int PositionInDataset, bool findSuccessiveEquality)
        {
            if (!findSuccessiveEquality)
                this.setValueForAllColumns(TableColumn, Value, PositionInDataset);
            else
            {
                int CurrentLine = PositionInDataset;
                // finding all tables that contain the column
                System.Collections.Generic.List<string> TableList = new List<string>();
                for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                {
                    if (this.ImportColumnsAll[i].ColumnName == TableColumn && !TableList.Contains(this.ImportColumnsAll[i].TableName))
                    {
                        System.Collections.Generic.List<string> PKColumnList = this.getPrimaryKey(this.ImportColumnsAll[i].TableName);
                        if (PKColumnList.Contains(TableColumn))
                            TableList.Add(this.ImportColumnsAll[i].TableName);
                    }
                }

                // finding all values corresponding to the table
                System.Collections.Generic.List<string> BaseValueList = new List<string>();
                for (int i = 0; i < this.ImportColumns.Count; i++)
                {
                    if (TableList.Contains(this.ImportColumns[i].TableName))
                        BaseValueList.Add(this._DatasetList[PositionInDataset][0][i]);
                }

                bool ValuesAreEqual = true;
                while (ValuesAreEqual && CurrentLine < this._DatasetList.Count)
                {
                    CurrentLine++;
                    // check for Equality
                    if (CurrentLine >= this._DatasetList.Count)
                    {
                        ValuesAreEqual = false;
                        break;
                    }
                    System.Collections.Generic.List<string> CompareValueList = new List<string>();
                    for (int i = 0; i < this.ImportColumns.Count; i++)
                    {
                        if (TableList.Contains(this.ImportColumns[i].TableName))
                            CompareValueList.Add(this._DatasetList[CurrentLine][0][i]);
                    }
                    for (int i = 0; i < CompareValueList.Count; i++)
                    {
                        if (CompareValueList[i] != BaseValueList[i])
                        {
                            ValuesAreEqual = false;
                            break;
                        }
                    }
                    if (ValuesAreEqual)
                        setValueForAllColumns(TableColumn, Value, CurrentLine);
                }

                foreach (System.Collections.Generic.List<string> Line in this._DatasetList[PositionInDataset])
                {
                    for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                    {
                        if (this.ImportColumnsAll[i].ColumnName == TableColumn)
                            Line[i] = Value;
                    }
                }
            }
        }

        private void setValueForAllColumns(string TableColumn, string Value)
        {
            foreach (System.Collections.Generic.List<System.Collections.Generic.List<string>> LineList in this._DatasetList)
            {
                foreach (System.Collections.Generic.List<string> Line in LineList)
                {
                    for (int i = 0; i < this.ImportColumnsAll.Count; i++)
                    {
                        if (this.ImportColumnsAll[i].ColumnName == TableColumn)
                            Line[i] = Value;
                    }
                }
            }
        }

        private string getValueOfColumn(string TableColumn, int PositionInDataset)
        {
            string Value = "";
            if (PositionInDataset >= this._DatasetList.Count) return Value;
            foreach (System.Collections.Generic.List<string> Line in this._DatasetList[PositionInDataset])
            {
                for (int i = 0; i < this._ImportColumns.Count; i++)
                {
                    if (this._ImportColumns[i].ColumnName == TableColumn && Line[i].Length > 0)
                    {
                        Value = Line[i];
                        break;
                    }
                }
                if (Value.Length > 0) break;
            }
            return Value;
        }

        private int? importOrUpdateTable(string TableAlias, int DatasetPosition, int DatasetLine)
        {
            int? PK = null;
            if (TableAlias.Length == 0) 
                return null;
            string Table = this.AliasDictionary[TableAlias];
            if (!this.ColumnsContainValues(TableAlias, DatasetPosition, DatasetLine))
                return null;
            System.Collections.Generic.List<string> PkList = this.getPrimaryKey(Table);
            System.Collections.Generic.Dictionary<string, string> ColumnValues = this.ColumnValues(TableAlias, DatasetPosition, DatasetLine);
            bool DatasetPresent = false;
            bool KeyPresent = true;
            int i = 0;
            foreach (string P in PkList)
            {
                if (!ColumnValues.ContainsKey(P)) KeyPresent = false;
                else
                {
                    if (ColumnValues[P].Length == 0) KeyPresent = false;
                }
            }
            // if the PK is complete
            if (KeyPresent)
            {
                string SQL = "SELECT COUNT(*) FROM " + Table + " WHERE 1 = 1 ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ColumnValues)
                {
                    if (PkList.Contains(KV.Key))
                    {
                        SQL += " AND " + KV.Key + " = '" + this.ReplaceApostrophe(KV.Value) + "'";
                    }
                }
                int? R = this.ExecuteQueryInt(SQL);
                if (R != null)
                    if (R > 0) DatasetPresent = true;
            }
            // if there is a dataset with the same PK - UPDATE, else INSERT
            if (DatasetPresent)
            {
                string SQL = "UPDATE " + Table + " SET ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ColumnValues)
                {
                    if (!PkList.Contains(KV.Key))
                    {
                        SQL += " " + KV.Key + " = '" + this.ReplaceApostrophe(KV.Value) + "',";
                    }
                }
                SQL = SQL.Substring(0, SQL.Length - 1) + " WHERE 1 = 1 ";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ColumnValues)
                {
                    if (PkList.Contains(KV.Key))
                    {
                        SQL += " AND " + KV.Key + " = '" + this.ReplaceApostrophe(KV.Value) + "'";
                    }
                }
                string Message = this.ExecuteNonQuery(SQL);
                int ii = 0;
                if (!int.TryParse(Message, out ii))
                    this._ErrorMessage += Message;
            }
            else
            {
                string SQL = "";
                string SqlFields = "";
                string SqlValues = "";
                bool ContainsIdentity = false;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ColumnValues)
                {
                    if (KV.Value == null) 
                        continue;
                    if (this.dataSetCollectionSpecimen.Tables[Table].Columns[KV.Key].AutoIncrement) 
                        ContainsIdentity = true;
                    else
                    {
                        if (KV.Key == "IdentificationSequence" && KV.Value.Length == 0 && Table == "Identification")
                        {
                            int Sequence = 1;
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVident in this.AliasDictionary)
                            {
                                if (KVident.Key == TableAlias)
                                    break;
                                if (KVident.Value == Table)
                                    Sequence++;
                            }
                            SqlFields += " " + KV.Key + ",";
                            SqlValues += " " + Sequence.ToString() + ",";
                        }
                        else if (KV.Value.Length == 0
                            && this.dataSetCollectionSpecimen.Tables[Table].Columns[KV.Key].DataType.Name != "String"
                            && this.dataSetCollectionSpecimen.Tables[Table].Columns[KV.Key].DataType.IsPrimitive)
                        {
                        }
                        else if (KV.Value.Length > 0)
                        {
                            //if (KV.Value.Length == 0
                            //    && this.dataSetCollectionSpecimen.Tables[Table].Columns[KV.Key].DataType.Name == "String")
                            //{
                            //}
                            //else
                            //{
                            SqlFields += " " + KV.Key + ",";
                            string Value = "";
                            if (KV.Value != null) Value = KV.Value;
                            Value = this.ReplaceApostrophe(Value);
                            SqlValues += " '" + Value + "',";
                            //}
                        }
                    }
                }
                SqlValues = SqlValues.Substring(0, SqlValues.Length - 1);
                SqlFields = SqlFields.Substring(0, SqlFields.Length - 1);
                SQL = "INSERT INTO " + Table + " (" + SqlFields + ") VALUES (" + SqlValues + ")";
                if (ContainsIdentity) SQL += "; SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
                PK = this.ExecuteQueryInt(SQL);
            }
            return PK;
        }

        private string ReplaceApostrophe(string SQL)
        {
            if (SQL.IndexOf("'") > -1)
            {
                SQL = SQL.Replace("'", "' + char(39) + '");
            }
            return SQL;
        }

        #endregion

        #region Auxillary: DatasetList, TableNames, AliasDict etc.

        private System.IO.StreamReader StreamReader(string File)
        {
            System.IO.StreamReader sr;
            if (this.comboBoxEncoding.SelectedIndex <= 0)
                sr = new System.IO.StreamReader(File);
            else
            {
                sr = new System.IO.StreamReader(File, this._Encodings[this.comboBoxEncoding.Text]);
            }
            return sr;
        }

        private void ReadDataFromFileToDatasetList(int? UpToPosition)
        {
            this._DatasetList = new List<List<List<string>>>();
            int iDataset = 0;
            int iContent = 0;

            System.IO.StreamReader sr = this.StreamReader(this.textBoxImportFile.Text);
            using (sr)
            {
                String line;
                int i = 0;
                int? CollectionSpecimenID = null;
                string CollectionEventID = null;
                while ((line = sr.ReadLine()) != null)
                {
                    // ignore header lines
                    if (i < this.StartingLineForDataInFile)
                    {
                        i++;
                        continue;
                    }

                    // ignore empty lines
                    if (line.Replace("\t", "").Trim().Length == 0) 
                        continue;
                    // if last line was reached
                    if (UpToPosition != null)
                    {
                        if (i > UpToPosition + this.StartingLineForDataInFile)
                        {
                            i++;
                            continue;
                        }
                    }
                    i++;
                    System.Collections.Generic.List<string> Content = this.ReadLineInStringList(line); 

                    // Test the CollectionSpecimenID
                    // if the CollectionSpecimenID is present and at position 0 than the data are present in the DB
                    if (Content.Count == 0) continue;
                    //if (Content[0].Length > 0)
                    if (Content.Count > 0)
                    {
                        int _ID;
                        if (this.ColumnList[0] == "CollectionSpecimenID")
                        {
                            if (!int.TryParse(Content[0], out _ID))
                            {
                                System.Windows.Forms.MessageBox.Show("Corrupt data - " + Content[0] + " is no integer");
                                //this._OK = false;
                                this._ErrorMessage += "\r\n" + "Corrupt data - CollectionSpecimenID " + Content[0] + " in line " + i.ToString() + " is no integer";
                            }
                            CollectionSpecimenID = _ID;
                        }
                        if (this.ColumnList.Contains("CollectionEventID"))
                        {
                            for (int iE = 0; iE < this.ColumnList.Count; iE++)
                            {
                                if (this.ColumnList[iE] == "CollectionEventID" && Content[iE].Length > 0)
                                {
                                    CollectionEventID = Content[iE];
                                    break;
                                }
                            }
                            if (CollectionEventID.Length > 0)
                            {
                                for (int iE = 0; iE < this.ColumnList.Count; iE++)
                                {
                                    if (this.ColumnList[iE] == "CollectionEventID" && Content[iE].Length == 0)
                                        Content[iE] = CollectionEventID;
                                }
                            }
                        }

                        // Add preset columns
                        this.AddPresetColumnsToContent(ref Content, CollectionSpecimenID, CollectionEventID);

                        if (this._DatasetList.Count == 0)
                        {
                            System.Collections.Generic.List<System.Collections.Generic.List<string>> Dataset = new List<List<string>>();
                            this._DatasetList.Add(Dataset);
                        }
                        else if (this._DatasetList[iDataset].Count > 0)
                        {
                            if (this._ImportColumns[0].ColumnName == "CollectionSpecimenID")
                            {
                                if (this._DatasetList[iDataset][0][0] != Content[0])
                                {
                                    System.Collections.Generic.List<System.Collections.Generic.List<string>> Dataset = new List<List<string>>();
                                    this._DatasetList.Add(Dataset);
                                    iDataset++;
                                }
                            }
                            else
                            {
                                System.Collections.Generic.List<System.Collections.Generic.List<string>> Dataset = new List<List<string>>();
                                this._DatasetList.Add(Dataset);
                                iDataset++;
                            }
                        }
                    }
                    else
                    {
                        this.AddPresetColumnsToContent(ref Content, CollectionSpecimenID, CollectionEventID);
                    }
                    this._DatasetList[iDataset].Add(Content);

                    this._DatasetPosition = this._DatasetList.Count - 1;
                    if (!this._IsReimport)
                    {
                        this.WriteTaxonInStorageLocation();
                        this.WriteTaxonInLastIdentificationCache();
                        this.WriteIdentificationUnitIDInDependentTables();
                        this.WriteSpecimenPartIDInDependentTables();
                    }
                }
                i = i - this.StartingLineForDataInFile;
                //i++;
                this.textBoxTotalLines.Text = i.ToString();
                this.numericUpDownUpTo.Maximum = (decimal)i;
            }
        }

        private System.Collections.Generic.List<string> ReadLineInStringList(string line)
        {
            System.Collections.Generic.List<string> Content = new List<string>();
            while (line.Length > 0)
            {
                if (line.IndexOf("\t") > -1)
                    Content.Add(line.Substring(0, line.IndexOf("\t")).ToString().Trim());
                else
                {
                    Content.Add(line.Trim());
                    line = "";
                }
                line = line.Substring(line.IndexOf("\t") + 1);
            }
            if (!this._IsReimport)
            {
                System.Collections.Generic.List<string> ValueList = new List<string>();
                for (int i = 0; i < Content.Count; i++)
                {
                    if (this.dataGridViewColumnMapping.Rows[0].Cells[i].Style.BackColor == System.Drawing.Color.Black)
                        continue;
                    if (this.dataGridViewColumnMapping.Rows[0].Cells[i].Style.BackColor == System.Drawing.Color.White
                        || this.dataGridViewColumnMapping.Rows[0].Cells[i].Style.BackColor.Name.ToString() == "0")
                        ValueList.Add(Content[i]);
                    else
                    {
                        DiversityCollection.ImportColumnGroup ICG = this.ImportColumnGroup(this.dataGridViewColumnMapping.Rows[0].Cells[i].Style.BackColor);
                        for (int ii = 0; ii < this._ImportColumns.Count; ii++)
                        {
                            if (this._ImportColumns[ii].TableName == ICG.TableName
                                && this._ImportColumns[ii].TableAlias == ICG.TableAlias
                                && this._ImportColumns[ii].ColumnName == ICG.ColumnName)
                            {
                                if (ValueList.Count <= ii)
                                    ValueList.Add(Content[i]);
                                else
                                {
                                    if (Content[i].Trim().Length > 0)
                                    {
                                        if (ValueList[ii].Length > 0)
                                            ValueList[ii] = ValueList[ii] + ICG.Separator + Content[i];
                                        else
                                            ValueList[ii] = Content[i];
                                    }
                                }
                                break;
                            }
                        }
                    }
                }
                while (ValueList.Count < this.ColumnList.Count)
                    ValueList.Add("");
                return ValueList;
            }
            else
            {
                while (Content.Count < this.ColumnList.Count)
                    Content.Add("");
                for (int i = 0; i < Content.Count; i++)
                {
                    System.Data.DataColumn C = this.dataSetCollectionSpecimen.Tables[this.ImportColumns[i].TableName].Columns[this.ImportColumns[i].ColumnName];
                    if (/*Content[i].Contains(",") &&*/ C.DataType.Name == "Int32" || C.DataType.Name == "Single")
                    {
                        Content[i] = Content[i].Replace(",", ".");
                    }
                }
                return Content;
            }
        }

        private int StartingLineForDataInFile
        {
            get
            {
                int StartingLine = 6;
                if (!this._IsReimport)
                    StartingLine = (int)this.numericUpDownDataStartInLine.Value - 1;
                return StartingLine;
            }
        }

        private void AddPresetColumnsToContent(ref System.Collections.Generic.List<string> Content, int? CollectionSpecimenID, string CollectionEventID)
        {
            if (this.ImportColumnsAddOn.Count > 0)
            {
                for (int ic = 0; ic < this._ImportColumnsAddOn.Count; ic++)
                {
                    if (this._ImportColumnsAddOn[ic].ColumnName == "CollectionSpecimenID"
                        && CollectionSpecimenID != null
                        && this._ImportColumnsAddOn[ic].Content == "")
                        Content.Add(CollectionSpecimenID.ToString());
                    else if (this._ImportColumnsAddOn[ic].ColumnName == "CollectionEventID"
                        && CollectionEventID != null
                        && this._ImportColumnsAddOn[ic].Content == "")
                        Content.Add(CollectionEventID.ToString());
                    else
                        Content.Add(this._ImportColumnsAddOn[ic].Content);
                }
            }
            if (this.checkBoxInsertRelation.Checked && !this._IsReimport)
            {
                string ParasiteTable = this.TableOfParasite;
                string HostTable = this.TableOfHost;
                string HostID = "";
                for (int iH = 0; iH < this._ImportColumns.Count; iH++)
                {
                    if (this._ImportColumns[iH].TableAlias == HostTable
                        && this._ImportColumns[iH].TableName == "IdentificationUnit"
                        && this._ImportColumns[iH].ColumnName == "IdentificationUnitID")
                    {
                        HostID = Content[iH];
                        break;
                    }
                }
                if (HostID != "")
                {
                    for (int iC = 0; iC < this._ImportColumns.Count; iC++)
                    {
                        if (this._ImportColumns[iC].TableAlias == ParasiteTable
                            && this._ImportColumns[iC].TableName == "IdentificationUnit"
                            && this._ImportColumns[iC].ColumnName == "RelatedUnitID")
                        {
                            Content[iC] = HostID;
                            break;
                        }
                    }
                }
            }
        }

        private System.Collections.Generic.List<System.Collections.Generic.List<string>> Dataset()
        {
            System.Collections.Generic.List<System.Collections.Generic.List<string>> _Dataset;
            _Dataset = new List<List<string>>();
            if (this._DatasetList == null || this._DatasetList.Count <= this._DatasetPosition || this._DatasetList.Count == 0)
                this.ReadDataFromFileToDatasetList((int?)this._DatasetPosition);
            if (this._DatasetList.Count > this._DatasetPosition)
                _Dataset = this._DatasetList[this._DatasetPosition];
            //_Dataset = this.DatasetList(null)[this._DatasetPosition];
            //_Dataset = this._DatasetList[this._DatasetPosition];
            return _Dataset;

        }

        private System.Collections.Generic.List<System.Collections.Generic.List<string>> Dataset(int Position)
        {
            if (this._DatasetPosition != Position)
                this._DatasetPosition = Position;
            return this.Dataset();
        }


        private System.Collections.Generic.Dictionary<string, string> ColumnValues(string TableAlias, int DatasetPosition, int DatasetLine)
        {
            System.Collections.Generic.Dictionary<string, string> ColumnValues = new Dictionary<string, string>();
            for (int i = 0; i < this.ImportColumnsAll.Count; i++)
            {
                if (this.ImportColumnsAll[i].TableAlias == TableAlias && !ColumnValues.ContainsKey(this.ImportColumnsAll[i].ColumnName))
                {
                    ColumnValues.Add(this.ImportColumnsAll[i].ColumnName, this.Dataset(DatasetPosition)[DatasetLine][i]); // this._DatasetList[DatasetPosition][DatasetLine][i]);
                    //ColumnValues.Add(this.ImportColumnsAll[i].ColumnName, this.Dataset(DatasetPosition, false)[DatasetLine][i]); // this._DatasetList[DatasetPosition][DatasetLine][i]);
                }
            }
            return ColumnValues;
        }

        private bool ColumnsContainValues(string TableAlias, int DatasetPosition, int DatasetLine)
        {
            bool OK = false;
            // if empty data should be imported
            if (this._ImportEmptyData) return true;
            // if the table is restricted to the preset values
            System.Collections.Generic.Dictionary<string, string> ColumnValues = this.ColumnValues(TableAlias, DatasetPosition, DatasetLine);
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ColumnValues)
            {
                if (KV.Key != "IdentificationUnitID"
                && KV.Key != "SpecimenPartID"
                && KV.Key != "CollectionSpecimenID"
                && KV.Key != "CollectionEventID"
                && KV.Key != "LocalisationSystemID"
                    && KV.Value.Length > 0)
                {
                    return true;
                }
            }

            // check if there are values in the presettings other than PK's etc.
            for (int i = 0; i < this._ImportColumnsAddOn.Count; i++)
            {
                if (this._ImportColumnsAddOn[i].TableAlias == TableAlias
                && this._ImportColumnsAddOn[i].ColumnName != "IdentificationUnitID"
                && this._ImportColumnsAddOn[i].ColumnName != "SpecimenParID"
                && this._ImportColumnsAddOn[i].ColumnName != "CollectionSpecimenID"
                && this._ImportColumnsAddOn[i].ColumnName != "CollectionEventID"
                && this._ImportColumnsAddOn[i].ColumnName != "LocalisationSystemID")
                {
                    if (this.Dataset()[DatasetLine][i + this._ImportColumns.Count].Length > 0)
                    {
                        return true;
                    }
                }
            }

            return OK;
        }

        private void checkBoxImportEmptyData_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxImportEmptyData.Checked) this._ImportEmptyData = true;
            else this._ImportEmptyData = false;
        }

        private System.Collections.Generic.Dictionary<string, string> AliasDictionary
        {
            get
            {
                this.resetAnalysis();
                System.Collections.Generic.Dictionary<string, string> _AliasTables = new Dictionary<string, string>();
                foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                {
                    if (I.TableAlias != null)
                    {
                        if (!_AliasTables.ContainsKey(I.TableAlias))
                        {
                            if (I.TableAlias.Length > 0)
                                _AliasTables.Add(I.TableAlias, I.TableName);
                            //else if (I.TableAlias.Length == 0 
                            //    && I.TableName.Length > 0
                            //    && !_AliasTables.ContainsKey(I.TableName))
                            //    _AliasTables.Add(I.TableName, I.TableName);
                        }
                    }
                }

                return _AliasTables;
            }
        }

        private string AliasCollectionEvent
        {
            get
            {
                string Alias = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                {
                    if (KV.Value == "CollectionEvent")
                    {
                        Alias = KV.Key;
                        break;
                    }
                }
                return Alias;
            }
        }

        private string AliasCollectionSpecimen
        {
            get
            {
                string Alias = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                {
                    if (KV.Value == "CollectionSpecimen")
                    {
                        Alias = KV.Key;
                        break;
                    }
                }
                //if (Alias.Length == 0)
                //    Alias = "CollectionSpecimen";
                return Alias;
            }
        }

        private string TableOfAlias(string TableAlias)
        {
            string Table = "";
            foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
            {
                if (I.TableAlias == TableAlias)
                {
                    Table = I.TableName;
                    break;
                }
            }
            return Table;
        }

        private void resetAnalysis()
        {
            //this._ImportColumns = new List<ImportColumn>();
            this._ImportColumnsAll = new List<ImportColumn>();
        }

        #region Properties: TableList, AliasList, ColumnList

        private System.Collections.Generic.List<DiversityCollection.ImportColumn> ImportColumnsAll
        {
            get
            {
                if (this._ImportColumnsAll == null || this._ImportColumnsAll.Count == 0)
                {
                    this._ImportColumnsAll = new List<ImportColumn>();
                    if (this._ImportColumns != null)
                    {
                        foreach (DiversityCollection.ImportColumn IC in this._ImportColumns)
                            this._ImportColumnsAll.Add(IC);
                    }
                    if (this._ImportColumnsAddOn != null)
                    {
                        foreach (DiversityCollection.ImportColumn IC in this._ImportColumnsAddOn)
                            this._ImportColumnsAll.Add(IC);
                    }
                }
                return this._ImportColumnsAll;
            }
            set
            {
                this._ImportColumnsAll = value;
            }
        }

        private System.Collections.Generic.List<DiversityCollection.ImportColumn> ImportColumns
        {
            get
            {
                if (this._ImportColumns == null)
                {
                    this._ImportColumns = new List<ImportColumn>();
                    if (!this._IsReimport)
                    {
                        if (this.dataGridViewColumnMapping.Columns.Count > 0 && this.dataGridViewColumnMapping.Rows.Count == 3)
                        {
                            for (int i = 0; i < this.dataGridViewColumnMapping.Columns.Count; i++)
                            {
                                DiversityCollection.ImportColumn I = new ImportColumn();
                                I.TableName = this.dataGridViewColumnMapping.Rows[0].Cells[i].Value.ToString();
                                I.TableAlias = this.dataGridViewColumnMapping.Rows[1].Cells[i].Value.ToString();
                                I.ColumnName = this.dataGridViewColumnMapping.Rows[2].Cells[i].Value.ToString();
                                this._ImportColumns.Add(I);
                            }
                        }
                    }
                }
                return this._ImportColumns;
            }
            set
            {
                this._ImportColumns = value;
                if (this._ImportColumns == null)
                    this._ImportColumns = new List<ImportColumn>();
                this._ImportColumnsAddOn = new List<ImportColumn>();
                this._ImportColumnsAll = new List<ImportColumn>();
            }
        }

        private System.Collections.Generic.List<DiversityCollection.ImportColumn> ImportColumnsAddOn
        {
            get
            {
                if (this._ImportColumnsAddOn == null)
                {
                    this._ImportColumnsAddOn = new List<ImportColumn>();
                }
                if (this._ImportColumnsAddOn.Count == 0)
                    this.FillImportColumnsAddOn();
                return this._ImportColumnsAddOn;
            }
            set
            {
                this._ImportColumnsAddOn = value;
                this._ImportColumnsAll = null;
            }
        }


        private System.Collections.Generic.List<string> TableList
        {
            get
            {
                System.Collections.Generic.List<string> List = new List<string>();
                //foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.ImportColumn> I in this._ImportColumnDictionary)
                //    List.Add(I.Value.TableName);
                foreach (DiversityCollection.ImportColumn I in this._ImportColumns)
                    List.Add(I.TableName);
                return List;
            }
        }

        private System.Collections.Generic.List<string> TableListAddOn
        {
            get
            {
                System.Collections.Generic.List<string> List = new List<string>();
                foreach (DiversityCollection.ImportColumn I in this._ImportColumnsAddOn)
                    List.Add(I.TableName);
                return List;
            }
        }

        private System.Collections.Generic.List<string> TableListAll
        {
            get
            {
                System.Collections.Generic.List<string> List = new List<string>();
                //foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.ImportColumn> I in this._ImportColumnDictionary)
                //    List.Add(I.Value.TableName);
                foreach (DiversityCollection.ImportColumn I in this._ImportColumns)
                    List.Add(I.TableName);
                foreach (DiversityCollection.ImportColumn I in this._ImportColumnsAddOn)
                    List.Add(I.TableName);
                return List;
            }
        }

        private System.Collections.Generic.List<string> ColumnList
        {
            get
            {
                System.Collections.Generic.List<string> CList = new List<string>();
                foreach (DiversityCollection.ImportColumn I in this.ImportColumns)
                    CList.Add(I.ColumnName);
                return CList;
            }
        }

        private System.Collections.Generic.List<string> ColumnListAddOn
        {
            get
            {
                System.Collections.Generic.List<string> CList = new List<string>();
                foreach (DiversityCollection.ImportColumn I in this._ImportColumnsAddOn)
                    CList.Add(I.ColumnName);
                return CList;
            }
        }

        private System.Collections.Generic.List<string> ColumnListAll
        {
            get
            {
                System.Collections.Generic.List<string> CList = new List<string>();
                foreach (DiversityCollection.ImportColumn I in this.ImportColumns)
                    CList.Add(I.ColumnName);
                foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAddOn)
                    CList.Add(I.ColumnName);
                return CList;
            }
        }

        private System.Collections.Generic.Dictionary<string, string> ParentUnitAlias
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
                if (this.ManyUnitTables)
                {
                    foreach (System.Object o in this.listBoxIdentificationsUnit1.Items)
                    {
                        if (!Dict.ContainsKey(o.ToString()))
                            Dict.Add(o.ToString(), this.textBoxUnitTable1.Text);
                    }
                    foreach (System.Object o in this.listBoxIdentificationsUnit2.Items)
                    {
                        if (!Dict.ContainsKey(o.ToString()))
                            Dict.Add(o.ToString(), this.textBoxUnitTable2.Text);
                    }
                }
                else
                {
                    string UnitAlias = ""; 
                    foreach (DiversityCollection.ImportColumn I in this._ImportColumnsAddOn)
                    {
                        if (I.TableName == "IdentificationUnit")
                        {
                            UnitAlias = I.TableAlias;
                            break;
                        }
                    }
                    foreach (System.Object o in this.listBoxIdentificationsUnit1.Items)
                    {
                        Dict.Add(o.ToString(), UnitAlias);
                    }
                }
                //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.AliasDictionary)
                //{
                //    if (KV.Value.StartsWith("Identification") && !Dict.ContainsKey(KV.Key) && KV.Value != "IdentificationUnit")
                //        Dict.Add(KV.Key, "IdentificationUnit");
                //}
                if (this.AliasDictionary.ContainsValue("IdentificationUnit")
                    && this.AliasDictionary.ContainsValue("CollectionSpecimenPart")
                    && this.AliasDictionary.ContainsValue("IdentificationUnitInPart"))
                {
                    System.Collections.Generic.List<string> UnitAliasList = new List<string>();
                    if (this.ManyUnitTables)
                    {
                        if (this.radioButtonMain1.Checked) UnitAliasList.Add(this.textBoxUnitTable1.Text);
                        else UnitAliasList.Add(this.textBoxUnitTable2.Text);
                    }
                    foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                    {
                        if (I.TableName == "IdentificationUnit"
                            && !UnitAliasList.Contains(I.TableAlias))
                            UnitAliasList.Add(I.TableAlias);
                    }
                    int iUnit = 1;
                    foreach (string Unit in UnitAliasList)
                    {
                        if (!Dict.ContainsKey("IdentificationUnitInPart_" + iUnit.ToString()))
                            Dict.Add("IdentificationUnitInPart_" + iUnit.ToString(), Unit);
                        iUnit++;
                    }
                    this.resetAnalysis();
                }
                return Dict;
            }
        }

        private System.Collections.Generic.Dictionary<string, string> ParentPartAlias
        {
            get
            {
                System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
                if (this.PartsInData == _PartsInData.two)
                {
                    //foreach (System.Object o in this.listBoxIdentificationsUnit1.Items)
                    //{
                    //    if (!Dict.ContainsKey(o.ToString()))
                    //        Dict.Add(o.ToString(), this.textBoxUnitTable1.Text);
                    //}
                    //foreach (System.Object o in this.listBoxIdentificationsUnit2.Items)
                    //{
                    //    if (!Dict.ContainsKey(o.ToString()))
                    //        Dict.Add(o.ToString(), this.textBoxUnitTable2.Text);
                    //}
                }
                else
                {
                    string PartAlias = "";
                    foreach (DiversityCollection.ImportColumn I in this._ImportColumnsAddOn)
                    {
                        if (I.TableName == "CollectionSpecimenPart")
                        {
                            PartAlias = I.TableAlias;
                            break;
                        }
                    }
                    foreach (System.Object o in this.listBoxIdentificationsUnit1.Items)
                    {
                        Dict.Add(o.ToString(), PartAlias);
                    }
                }
                if (this.AliasDictionary.ContainsValue("IdentificationUnit")
                    && this.AliasDictionary.ContainsValue("CollectionSpecimenPart")
                    && this.AliasDictionary.ContainsValue("IdentificationUnitInPart"))
                {
                    System.Collections.Generic.List<string> PartAliasList = new List<string>();
                    if (this.PartsInData == _PartsInData.two)
                    {
                        //if (this.radioButtonMain1.Checked) PartAliasList.Add(this.textBoxUnitTable1.Text);
                        //else PartAliasList.Add(this.textBoxUnitTable2.Text);
                    }
                    foreach (DiversityCollection.ImportColumn I in this.ImportColumnsAll)
                    {
                        if (I.TableName == "CollectionSpecimenPart"
                            && !PartAliasList.Contains(I.TableAlias))
                            PartAliasList.Add(I.TableAlias);
                    }
                    int iPart = 1;
                    foreach (string Part in PartAliasList)
                    {
                        if (!Dict.ContainsKey("IdentificationUnitInPart_" + iPart.ToString()))
                            Dict.Add("IdentificationUnitInPart_" + iPart.ToString(), Part);
                        iPart++;
                    }
                    this.resetAnalysis();
                }
                return Dict;
            }
        }

        #endregion

        #endregion

        #region Database, SQL

        private System.Data.SqlClient.SqlConnection SqlConnection
        {
            get
            {
                if (this._SqlConnection == null)
                    this._SqlConnection = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                if (this._SqlConnection.ConnectionString.Length == 0)
                    this._SqlConnection = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                if (this._SqlConnection.State.ToString() != "Open")
                    this._SqlConnection.Open();
                return this._SqlConnection;
            }
        }

        private System.Data.SqlClient.SqlCommand SqlCommand
        {
            get
            {
                if (this._SqlCommand == null 
                    || this._SqlCommand.Connection.State == ConnectionState.Closed 
                    || this._SqlCommand.Connection.ConnectionString.Length == 0)
                    this._SqlCommand = new System.Data.SqlClient.SqlCommand("", this.SqlConnection);
                return this._SqlCommand;
            }
        }

        private int? ExecuteQueryInt(string SQL)
        {
            int? Result = null;
            try
            {
                int i = 0;
                this.SqlCommand.CommandText = SQL;
                if (SQL.IndexOf("SELECT ") > -1)
                {
                    if (int.TryParse(this.SqlCommand.ExecuteScalar().ToString(), out i))
                        Result = i;
                }
                else
                    this.SqlCommand.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                this._ErrorMessage += ex.Message + " \r\n\r\nQuery: " + SQL + "\r\n"; ;
            }
            catch { Result = null; }
            return Result;
        }

        private string ExecuteNonQuery(string SQL)
        {
            string Result = "";
            try
            {
                this.SqlCommand.CommandText = SQL;
                if (this.SqlCommand.Connection.State == ConnectionState.Closed)
                    this.SqlCommand.Connection.Open();
                Result = this.SqlCommand.ExecuteNonQuery().ToString();
            }
            catch (System.Exception ex) { Result = ex.Message; }
            return Result;
        }

        #endregion


    }
}