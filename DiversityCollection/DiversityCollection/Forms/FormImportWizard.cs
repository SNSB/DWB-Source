using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection
{

    public partial class FormImportWizard : Form, iImportInterface, UserControls.iUserControlImportInterface
    {

        ///TODO:
        ///xslt file muss mit whitespace funktionieren
        ///Series einlesbar machen
        ///dann evtl. alle anderen abwaehlen
        ///Series anhängbar machen
        ///Project verpflichtend nur wenn Tabellen-liste CollectionSpecimen enthält
        
        /*
         * Schritte:
         * 
         * Auswahl der Datei, Encoding etc.
         * Überprüfung der Datei
         * Festlegen der Start- und Endzeile
         * Festlegen ob Datenangefuegt werden sollen
         *      Bei Anfügen Auswahl des Schluessels
         * 
         * Wählen des Project
         *      Bei Anfügen Testen des Schluessels
         * Festlegen u. evtl. Erstellen einer Quelle
         * Festlegen der Spalte der Quellinformationen
         * 
         * EventSeries - Angeben oder Auswählen
         * 
         * Event - Angeben oder Auswaehlen
         *      Country  - Ja/Nein, Angeben oder Auswaehlen
         * 
         * Localisation
         * 
         * Beleg
         * 
         * Sammler
         * 
         * Standort
         * 
         * Organismen
         * 
         *      Bestimmungen
         * 
         *      Analysen
         * 
         * Lagerung
         * 
         *      Processing
         * 
         *      Ausleihen, Inventar
         * 
         * Bilder
         * 
         * Anforderungen Flo: mehrere Bestimmungen und Sammler, Problem bei Altitude
         * 
         * */

        #region Parameter

        public static readonly System.Drawing.Color ColorForAttachment = System.Drawing.Color.Cyan;
        public static readonly System.Drawing.Color ColorForFixing = System.Drawing.Color.LightBlue;
        public static readonly System.Drawing.Color ColorForSuccess = System.Drawing.Color.LightGreen;
        public static readonly System.Drawing.Color ColorForError = System.Drawing.Color.Pink;
        public static readonly System.Drawing.Color ColorForColumQuery = System.Drawing.Color.Yellow;
        public static readonly System.Drawing.Color ColorForColumMissing = System.Drawing.Color.LightYellow;
        public static readonly System.Drawing.Color ColorForDatabaseAsSource = System.Drawing.Color.LightGray;

        private System.Collections.Generic.Dictionary<string, System.Text.Encoding> _Encodings = new Dictionary<string, Encoding>();

        private System.IO.FileInfo _TransformationSchema;

        int? _SeriesID;

        private DiversityCollection.Import _Import;

        public DiversityCollection.Import Import
        {
            get 
            {
                if (this._Import == null)
                {
                    this._Import = new Import(this);
                }
                return _Import; 
            }
            //set { _Import = value; }
        }

        private DiversityCollection.Import_Column _CurrentImportColumn;

        private System.Collections.Generic.Dictionary<string, string> _TableDictionary;

        private DiversityCollection.Import_Column _PreviousAttachmentImportColum;
        private string PreviousAttachmentKey
        {
            get
            {
                if (this._PreviousAttachmentImportColum == null)
                    return "";
                else
                    return this._PreviousAttachmentImportColum.Key;
            }
        }
        private string PreviousAttachmentCorrespondingColumKey
        {
            get
            {
                try
                {
                    if (this.PreviousAttachmentKey == null || this.PreviousAttachmentKey.Length == 0)
                        return "";
                    else
                    {
                        string _PreviousAttachmentCorrespondingColumKey = this.PreviousAttachmentKey.Substring(0, this.PreviousAttachmentKey.Length - 1) + "1";
                        return _PreviousAttachmentCorrespondingColumKey;
                    }
                }
                catch (System.Exception ex) { }
                return "";
            }
        }

        #endregion

        #region Construction
        
        public FormImportWizard()
        {
            InitializeComponent();
            try
            {
                DiversityCollection.Import.ImportColumns.Clear();
                DiversityCollection.Import.ImportColumnControls.Clear();
                DiversityCollection.Import.ImportSelectors.Clear();
                DiversityCollection.Import.ImportSteps.Clear();
                DiversityCollection.Import.ImportTables.Clear();
                try
                {
                    if (DiversityCollection.Import.AttachmentKeyImportColumn != null)
                        DiversityCollection.Import.AttachmentKeyImportColumn = null;
                    if (DiversityCollection.Import.CurrentImportColumn != null)
                        DiversityCollection.Import.CurrentImportColumn = null;
                    if (DiversityCollection.Import.SettingObjects != null)
                        DiversityCollection.Import.SettingObjects = null;
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                DiversityCollection.Import.TabPagesControl.Clear();

                this.helpProvider.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm";

                this.initForm();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Interface

        public void Reset() { this.ResetAll(); }

        public System.Windows.Forms.Button ButtonForward()
        {
            return this.buttonForward;
        }
        public System.Windows.Forms.Button ButtonBack()
        {
            return this.buttonBack;
        }
        public System.Windows.Forms.Panel PanelForImportSteps()
        {
            return this.panelImportSteps;
        }
        public System.Windows.Forms.DataGridView Grid()
        {
            return this.dataGridViewFile;
        }

        public void setColumnHeaeder(int Position)
        {
            try
            {
                this.dataGridViewFile.Columns[Position].HeaderText = this.ColumnHeaderForDataGrid(Position);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public string StepKey() { return ""; }

        public int FirstLineForImport()
        {
            return (int)this.numericUpDownStartLine.Value;
        }

        public int LastLineForImport()
        {
            return (int)this.numericUpDownEndLine.Value;
        }

        public string getCurrentImportKey()
        {
            if (this._CurrentImportColumn != null
                && this._CurrentImportColumn.Key != null
                && this._CurrentImportColumn.Key.Length > 1)
                return this._CurrentImportColumn.Key;
            else 
                return "";
        }

        public void setDataGridLineColor(int iLine, System.Drawing.Color Color)
        {
            //this.dataGridViewFile.Rows[iLine].DefaultCellStyle.BackColor = Color;

            int iAttachmentColumn = -1;
            if (DiversityCollection.Import.AttachmentKeyImportColumn != null && DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile != null)
            {
                iAttachmentColumn = (int)DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile;
            }
            for (int i = 0; i < this.dataGridViewFile.Columns.Count; i++)
            {
                if (iAttachmentColumn == i)
                    this.dataGridViewFile.Rows[iLine].Cells[i].Style.BackColor = FormImportWizard.ColorForAttachment;
                else
                    this.dataGridViewFile.Rows[iLine].Cells[i].Style.BackColor = Color;
            }

        }

        public void AddImportStep(string StepKey)
        {
            DiversityCollection.Import.ImportStep I = DiversityCollection.Import_Step.ImportStepKey(StepKey);
            switch (I)
            {
                case DiversityCollection.Import.ImportStep.Collector:
                    this.userControlImportCollector.AddImportStep();
                    break;
                case DiversityCollection.Import.ImportStep.Organism:
                    this.userControlImportUnit.AddImportStep(StepKey);
                    break;
                case DiversityCollection.Import.ImportStep.Storage:
                    this.userControlImportStorage.AddImportStep(StepKey);
                    break;
                case DiversityCollection.Import.ImportStep.Relation:
                    this.userControlImportSpecimenRelation.AddImportStep();
                    break;
                case DiversityCollection.Import.ImportStep.Event:
                    this.userControlImportMethod_Event.AddImportStepParameter();
                    break;
            }
            int i = this.tabPageOrganisms.Controls.Count;
        }

        public void AddImportColumn(DiversityCollection.Import_Column ImportColumn)
        {
            if (ImportColumn.Sequence() > 1)
            {
                string KeyBasicColumn = ImportColumn.Key.Substring(0, ImportColumn.Key.Length - ImportColumn.Sequence().ToString().Length) + "1";
                if (DiversityCollection.Import.ImportColumns.ContainsKey(KeyBasicColumn))
                {
                    DiversityCollection.Import.ImportColumns[KeyBasicColumn].ImportColumnControl.AddMultiColumn(ImportColumn);
                }
            }
            //if (ImportColumn.ImportStep != null && !DiversityCollection.Import.ImportSteps.ContainsKey(ImportColumn.ImportStep))
            //{
            //}
        }

        public DiversityCollection.Import getImport() { return this._Import; }

        public bool SaveErrorLinesInFile
        {
            get { return this.checkBoxSaveErrors.Checked; }
        }

        public string SourceFile { get { return this.textBoxImportFile.Text; } }

        public void SetImportErrorFile(string ErrorFile)
        {
            this.buttonOpenErrorFile.Tag = ErrorFile;
            if (ErrorFile.Length > 0)
            {
                this.buttonOpenErrorFile.Visible = true;
            }
            else
            {
                this.buttonOpenErrorFile.Visible = false;
            }
        }

        #endregion

        #region Interface iUserControlImportInterface

        public System.Windows.Forms.Panel SelectionPanelForDependentSteps() { return this.panelImportSeletion; }

        public void UpdateSelectionPanel() { }

        public void initUserControl(DiversityCollection.iImportInterface I, DiversityCollection.Import_Step SuperiorImportStep)
        {
            //this._iImportInterface = I;
            //this.AddItem();
        }

        public void showStepControls(DiversityCollection.Import_Step ImportStep)
        {
            this.tabControlMain.TabPages.Clear();
            string Key = ImportStep.StepKey();
            if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.File))
                this.tabControlMain.TabPages.Add(this.tabPageChooseFile);
            else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Series))
                this.tabControlMain.TabPages.Add(this.tabPageSeries);
            else if (Key.StartsWith(DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Event)))
            {
                this.tabControlMain.TabPages.Add(this.tabPageCollectionEvent);
                this.tabControlCollectionEvent.TabPages.Clear();
                if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Altitude))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventAltitude);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Date))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventDate);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Coordinates))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventCoordinates);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Place))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventPlace);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Depth))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventDepth);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Exposition))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventExposition);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.GaussKrueger))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventGK);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Height))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventHeight);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Locality))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventLocality);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.MTB))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventTK25);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Slope))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventSlope);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Plot))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventPlot);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Lithostratigraphy))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageLithostratigraphy);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Chronostratigraphy))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageChronostratigraphy);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepEvent.Method))
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageMethod);
                else
                    this.tabControlCollectionEvent.TabPages.Add(this.tabPageEventDate);
            }
            else if (Key.StartsWith(DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Collector)))
            {
                this.tabControlMain.TabPages.Add(this.tabPageCollector);
                if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Collector))
                    this.userControlImportCollector.showStepControls(ImportStep);
            }
            else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Images))
                this.tabControlMain.TabPages.Add(this.tabPageImages);
            else if (Key.StartsWith(DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Organism)))
            {
                this.tabControlMain.TabPages.Add(this.tabPageOrganisms);
                if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Organism))
                    this.userControlImportUnit.showStepControls(ImportStep);
            }
            else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Project))
                this.tabControlMain.TabPages.Add(this.tabPageProject);
            else if (Key.StartsWith(DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Specimen)))
            {
                this.tabControlMain.TabPages.Add(this.tabPageSpecimen);
                this.tabControlSpecimen.TabPages.Clear();
                if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepSpecimen.Accession))
                    this.tabControlSpecimen.TabPages.Add(this.tabPageSpecimenAccession);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepSpecimen.Depositor))
                    this.tabControlSpecimen.TabPages.Add(this.tabPageSpecimenDepositor);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepSpecimen.Label))
                    this.tabControlSpecimen.TabPages.Add(this.tabPageSpecimenLabel);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepSpecimen.Notes))
                    this.tabControlSpecimen.TabPages.Add(this.tabPageSpecimenNotes);
                else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStepSpecimen.Reference))
                    this.tabControlSpecimen.TabPages.Add(this.tabPageSpecimenReference);
                else
                    this.tabControlSpecimen.TabPages.Add(this.tabPageSpecimenAccession);
            }
            else if (Key.StartsWith(DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Relation)))
            {
                this.tabControlMain.TabPages.Add(this.tabPageRelation);
                if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Relation))
                    this.userControlImportSpecimenRelation.showStepControls(ImportStep);
            }
            else if (Key.StartsWith(DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Storage)))
            {
                this.tabControlMain.TabPages.Add(this.tabPageStorage);
                if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Storage))
                    this.userControlImportStorage.showStepControls(ImportStep);
            }
            else if (Key == DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Summary))
                this.tabControlMain.TabPages.Add(this.tabPageSummary);
        }

        public void AddImportStep() { }
        public void HideImportStep() { }
        public void ShowHiddenImportSteps()
        {
        }

        
        #endregion

        #region Form and common functions
        
        private void initForm()
        {
            try
            {
                this.initImport();
                //this.initTabPages();

                //this.buttonForward.Visible = false;
                //this.buttonBack.Visible = false;
                this.setTabControlMain();

                this.initLists();

                this.initFile();
                this.initSeries();
                this.initEvent();
                this.initSpecimen();
                this.initProject();
                this.initSpecimenRelations();
                this.initCollector();
                this.initUnit();
                //this.initUnitGroup();
                this.initStorage();
                this.initImages();
                this.initSummary();

                this.initSettingContols();
                this.initSettings();
                //this.Import.ImportSteps[((int)(DiversityCollection.FormImportWizard.ImportStep.File)).ToString()].UserControlImportStep.IsCurrent = true;
                this.Import.ImportStepsShow();
                this.Import.CurrentPosition = DiversityCollection.Import.getImportStepKey(Import.ImportStep.File);

                DiversityWorkbench.Entity.setEntity(this, this.toolTip);

                //this.treeViewAnalyseData.ImageList = DiversityCollection.Specimen.ImageList;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void dataGridViewFile_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string rowNumber =
                (e.RowIndex + 1).ToString()
                .PadLeft(this.dataGridViewFile.RowCount.ToString().Length);

            // Schriftgröße:
            SizeF size = e.Graphics.MeasureString(rowNumber, this.Font);

            // Breite des ZeilenHeaders anpassen:
            if (this.dataGridViewFile.RowHeadersWidth < (int)(size.Width + 20))
                this.dataGridViewFile.RowHeadersWidth = (int)(size.Width + 20);

            // ZeilenNr zeichnen:
            e.Graphics.DrawString(
                rowNumber,
                this.Font,
                SystemBrushes.ControlText,
                e.RowBounds.Location.X + 15,
                e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
        }

        private void initImport()
        {
            try
            {
                DiversityCollection.Import.ImportSteps.Clear();
                DiversityCollection.Import.ImportColumns.Clear();
                DiversityCollection.Import.CurrentImportColumn = null;
                DiversityCollection.Import.ImportSelectors.Clear();
                DiversityCollection.Import.initTabPagesControl(this.tabControlMain);
                DiversityCollection.Import.initTabPagesControl(this.tabControlCollectionEvent);
                DiversityCollection.Import.initTabPagesControl(this.tabControlSpecimen);
                DiversityCollection.Import.initTabPagesControl(this.userControlImportCollector.tabControlCollector);
                DiversityCollection.Import.initTabPagesControl(this.userControlImportStorage.tabControlStorage);
                DiversityCollection.Import.initTabPagesControl(this.userControlImportUnit.tabControlImportSteps);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ResetAll()
        {
            DiversityCollection.Import.AttachmentKeyImportColumn = null;
            this.setAttachmentInterface();

            this.Import.ImportReset();

            DiversityCollection.Import.resetTabPagesControl(this.tabControlMain);
            DiversityCollection.Import.resetTabPagesControl(this.tabControlCollectionEvent);
            DiversityCollection.Import.resetTabPagesControl(this.tabControlSpecimen);
            DiversityCollection.Import.resetTabPagesControl(this.userControlImportCollector.tabControlCollector);
            DiversityCollection.Import.resetTabPagesControl(this.userControlImportStorage.tabControlStorage);
            this.userControlImportStorage.Reset();
            DiversityCollection.Import.resetTabPagesControl(this.userControlImportUnit.tabControlImportSteps);
            this.userControlImportUnit.Reset();
            
            DiversityCollection.Import.ImportColumns.Clear();
            DiversityCollection.Import.ImportColumnControls.Clear();
            DiversityCollection.Import.ImportSelectors.Clear();
            DiversityCollection.Import.ImportSteps.Clear();
            DiversityCollection.Import.ImportTables.Clear();
            DiversityCollection.Import.AttachmentKeyImportColumn = null;
            DiversityCollection.Import.CurrentImportColumn = null;
            DiversityCollection.Import.SettingObjects = null;
            DiversityCollection.Import.TabPagesControl.Clear();

            this.initForm();
        }

        #region Lists

        private void initLists()
        {
            this.setEncodingList();
        }

        private void setEncodingList()
        {
            try
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
                this.comboBoxEncoding.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public System.Text.Encoding Encoding
        {
            get
            {
                System.Text.Encoding E = System.Text.Encoding.UTF8;
                if (this._Encodings.Count > 0)
                {
                    E = this._Encodings[this.comboBoxEncoding.SelectedItem.ToString()];
                    if (E == null)
                        System.Windows.Forms.MessageBox.Show("Please select an encoding");
                }
                return E;
            }
        }

        #endregion

        private void setTabControlMain()
        {
            try
            {
                this.tabControlMain.TabPages.Remove(this.tabPageChooseFile);
                this.tabControlMain.TabPages.Remove(this.tabPageCollectionEvent);
                this.tabControlMain.TabPages.Remove(this.tabPageOrganisms);
                this.tabControlMain.TabPages.Remove(this.tabPageStorage);
                this.tabControlMain.TabPages.Remove(this.tabPageCollector);
                this.tabControlMain.TabPages.Remove(this.tabPageSeries);
                this.tabControlMain.TabPages.Remove(this.tabPageProject);
                this.tabControlMain.TabPages.Remove(this.tabPageSpecimen);
                this.tabControlMain.TabPages.Remove(this.tabPageImages);
                this.tabControlMain.TabPages.Remove(this.tabPageSummary);

                // solange das nicht benutzt wird ausblenden
                this.tabControlCollectionEvent.TabPages.Remove(this.tabPageEventHierarchy);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            DiversityCollection.Import.MoveToPreviousStep();
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            DiversityCollection.Import.MoveToNextStep();
        }

        public string StepError(string StepPosition)
        {
            string Error = "";
            return Error;
        }

        #endregion

        #region Settings
        
        private void initSettingContols()
        {
            try
            {
                if (!DiversityCollection.Import.SettingObjects.ContainsKey(DiversityCollection.Import.Setting.Encoding.ToString()))
                    DiversityCollection.Import.SettingObjects.Add(Import.Setting.Encoding.ToString(), this.comboBoxEncoding);
                if (!DiversityCollection.Import.SettingObjects.ContainsKey(DiversityCollection.Import.Setting.StartLine.ToString()))
                    DiversityCollection.Import.SettingObjects.Add(Import.Setting.StartLine.ToString(), this.numericUpDownStartLine);
                if (!DiversityCollection.Import.SettingObjects.ContainsKey(DiversityCollection.Import.Setting.EndLine.ToString()))
                    DiversityCollection.Import.SettingObjects.Add(Import.Setting.EndLine.ToString(), this.numericUpDownEndLine);
                if (!DiversityCollection.Import.SettingObjects.ContainsKey(DiversityCollection.Import.Setting.TrimValues.ToString()))
                    DiversityCollection.Import.SettingObjects.Add(Import.Setting.TrimValues.ToString(), this.checkBoxTrimValues);
                if (!DiversityCollection.Import.SettingObjects.ContainsKey(DiversityCollection.Import.Setting.Attachment.ToString()))
                    DiversityCollection.Import.SettingObjects.Add(Import.Setting.Attachment.ToString(), DiversityCollection.Import.AttachmentKeyImportColumn);
                if (!DiversityCollection.Import.SettingObjects.ContainsKey(DiversityCollection.Import.Setting.AttachmentColumnInSourceFile.ToString()))
                    DiversityCollection.Import.SettingObjects.Add(Import.Setting.AttachmentColumnInSourceFile.ToString(), DiversityCollection.Import.AttachmentKeyImportColumn);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initSettings()
        {
            try
            {
                DiversityCollection.Import.AddSetting(DiversityCollection.Import.Setting.Attachment, "");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region File

        private void initFile()
        {
            try
            {
                DiversityCollection.Import_Step IF =
                    DiversityCollection.Import_Step.GetImportStep(
                    "File",
                    "The imported file",
                    DiversityCollection.Import.getImportStepKey(Import.ImportStep.File),
                    "",
                    null, null,
                    0,
                    (UserControls.iUserControlImportInterface)this,
                    this.StepImage(DiversityCollection.Import.ImportStep.File),
                    null);
                IF.CanHide(false);
                DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.File)].setStepError(this.FileOK);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private string FileOK
        {
            get
            {
                this.labelEncoding.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.labelImportFile.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.labelImportRange.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.labelImportRangeEnd.BackColor = System.Drawing.SystemColors.ControlLightLight;

                string Message = "";
                if (this.numericUpDownEndLine.Value < this.numericUpDownStartLine.Value)
                {
                    this.labelImportRange.BackColor = FormImportWizard.ColorForError;
                    this.labelImportRangeEnd.BackColor = FormImportWizard.ColorForError;
                    Message = "No lines selected\r\n";
                }
                if (this.textBoxImportFile.Text.Length == 0)
                {
                    this.labelImportFile.BackColor = FormImportWizard.ColorForError;
                    Message += "No file selected\r\n";
                }
                if (this.comboBoxEncoding.SelectedIndex == 0)
                {
                    this.labelEncoding.BackColor = FormImportWizard.ColorForError;
                    Message += "No encoding selected";
                }
                return Message;
            }
        }

        private void comboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DiversityCollection.Import.ImportSteps.ContainsKey(DiversityCollection.Import.getImportStepKey(Import.ImportStep.File)))
                DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.File)].setStepError(this.FileOK);
            //this.UpdateImportStep(this.FileOK);
            this.setDataGridColorRange();
            if (this.comboBoxEncoding.SelectedItem.ToString() != null)
            {
                DiversityCollection.Import.AddSetting(Import.Setting.Encoding, this.comboBoxEncoding.SelectedItem.ToString());
            }
            this.ResetFile();
        }

        #region Line range
        
        private void numericUpDownEndLine_ValueChanged(object sender, EventArgs e)
        {
            DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.File)].setStepError(this.FileOK);
            //this.UpdateImportStep(this.FileOK);
            this.numericUpDownAnalyseDataLine.Maximum = this.numericUpDownEndLine.Value;
            this.setDataGridColorRange();
            this.setProgressBarMaximum();
            DiversityCollection.Import.AddSetting(Import.Setting.EndLine, this.numericUpDownEndLine.Value.ToString());
        }

        private void numericUpDownEndLine_Leave(object sender, EventArgs e)
        {
            if (this.numericUpDownEndLine.Value < this.numericUpDownEndLine.Maximum)
                DiversityCollection.Import.AddSetting("EndLine", this.numericUpDownEndLine.Value.ToString());
        }

        private void numericUpDownStartLine_ValueChanged(object sender, EventArgs e)
        {
            DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.File)].setStepError(this.FileOK);
            //DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.File)].setStepError(this.FileOK);
            //this.UpdateImportStep(this.FileOK);
            try
            {
                DiversityCollection.Import.AddSetting(Import.Setting.StartLine, this.numericUpDownStartLine.Value.ToString());
                if (this.numericUpDownAnalyseDataLine.Maximum >= this.numericUpDownStartLine.Value)
                {
                    try
                    {
                        this.numericUpDownAnalyseDataLine.Value = this.numericUpDownStartLine.Value;
                    }
                    catch (System.Exception ex) { }
                }
                this.setDataGridColorRange();
                this.setProgressBarMaximum();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setProgressBarMaximum()
        {
            int i = (int)(this.numericUpDownEndLine.Value - this.numericUpDownStartLine.Value);
            if (i > 0)
                this.progressBar.Maximum = i;
        }

        public void setDataGridColorRange()
        {
            int iAttachmentColumn = -1;
            if (DiversityCollection.Import.AttachmentKeyImportColumn != null && DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile != null)
            {
                iAttachmentColumn = (int)DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile;
            }
            for (int iLine = 0; iLine < this.dataGridViewFile.Rows.Count; iLine++)
            {
                if (iLine + 1 < (int)this.numericUpDownStartLine.Value ||
                    iLine + 1 > (int)this.numericUpDownEndLine.Value)
                {
                    for (int i = 0; i < this.dataGridViewFile.Columns.Count; i++)
                    {
                        this.dataGridViewFile.Rows[iLine].Cells[i].Style.BackColor = System.Drawing.SystemColors.Control;
                    }
                    this.dataGridViewFile.Rows[iLine].DefaultCellStyle.ForeColor = System.Drawing.Color.Gray;
                }
                else
                {
                    for (int i = 0; i < this.dataGridViewFile.Columns.Count; i++)
                    {
                        if (iAttachmentColumn == i)
                            this.dataGridViewFile.Rows[iLine].Cells[i].Style.BackColor = FormImportWizard.ColorForAttachment;
                        else
                            this.dataGridViewFile.Rows[iLine].Cells[i].Style.BackColor = System.Drawing.SystemColors.Window;
                    }
                    this.dataGridViewFile.Rows[iLine].DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                }
            }
        }

        private void checkBoxFirstLineContainsColumns_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxFirstLineContainsColumns.Checked)
            {
                if (this.numericUpDownAnalyseDataLine.Value == 1)
                    this.numericUpDownAnalyseDataLine.Value = 2;
                if (this.numericUpDownStartLine.Value == 1)
                    this.numericUpDownStartLine.Value = 2;
            }
            else
            {
                if (this.numericUpDownAnalyseDataLine.Value == 2)
                    this.numericUpDownAnalyseDataLine.Value = 1;
                if (this.numericUpDownStartLine.Value == 2)
                    this.numericUpDownStartLine.Value = 1;
            }
        }

        public bool FirstLineContainsColumnDefinition { get { return this.checkBoxFirstLineContainsColumns.Checked; } }

        #endregion
        
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
            {
                System.IO.DirectoryInfo D = new System.IO.DirectoryInfo( System.Windows.Forms.Application.StartupPath + "\\Import");
                if (!D.Exists)
                    D.Create();
                this.openFileDialog.InitialDirectory = D.FullName;
            }
            this.openFileDialog.Filter = "Text Files|*.txt";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    //this.ResetAll();
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxImportFile.Text = f.FullName;
                    this.ResetFile();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void ResetFile()
        {
            try
            {
                if (this.comboBoxEncoding.SelectedIndex < 1 || this.textBoxImportFile.Text.Length == 0)
                    return;
                //this.ResetAll();
                //this.initImport();
                System.IO.FileInfo f = new System.IO.FileInfo(this.textBoxImportFile.Text);
                DiversityCollection.Import.readFileInDataGridView(f, this.dataGridViewFile, this.Encoding, null);
                this.numericUpDownStartLine.Maximum = this.dataGridViewFile.Rows.Count;
                this.numericUpDownEndLine.Maximum = this.dataGridViewFile.Rows.Count;
                this.numericUpDownEndLine.Value = this.dataGridViewFile.Rows.Count;
                //this.setTabControlMain();
                this.setDataGridColorRange();
                DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.File)].setStepError(this.FileOK);
                //this.UpdateImportStep(this.FileOK);
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewFile.Columns)
                    C.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonOpenImportSchema_Click(object sender, EventArgs e)
        {
            if (this.textBoxImportFile.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select the file for the import");
                return;
            }
            System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\Import\\ImportSchedules");
            if (!Dir.Exists)
                Dir.Create();
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            this.openFileDialog.InitialDirectory = Dir.FullName;
            this.openFileDialog.Filter = "XML Files|*.xml";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxImportSchedule.Text = f.FullName;
                    this.LoadSchema(f.FullName);
                    this.AnalyseTables();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonShowImportSchema_Click(object sender, EventArgs e)
        {
            if (this.textBoxImportSchedule.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("No schedule has been selected");
                return;
            }
            this.ShowConvertedFile(this.textBoxImportSchedule.Text);
        }

        private void buttonReload_Click(object sender, EventArgs e)
        {
            try
            {
                this.ResetAll();
                System.IO.FileInfo f;
                if (this.textBoxImportFile.Text.Length == 0)
                    f = new System.IO.FileInfo(this.openFileDialog.FileName);
                else
                    f = new System.IO.FileInfo(this.textBoxImportFile.Text);
                this.textBoxImportFile.Text = f.FullName;
                this.textBoxImportSchedule.Text = "";
                this.Import.ResetImportTables();
                this.ResetFile();
                DiversityCollection.Import.readFileInDataGridView(f, this.dataGridViewFile, this.Encoding, null);
                this.numericUpDownEndLine.Value = this.dataGridViewFile.Rows.Count;
                DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.File)].setStepError(this.FileOK);
                this.setDataGridColorRange();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string ConvertXmlToHtml(string XmlFile, string XsltFile)
        {
            string OutputFile = XmlFile;
            try
            {
                //string SchemaPath = System.Windows.Forms.Application.StartupPath + "\\Import\\ImportSchedules\\Transformation\\TransformationSchema.xlst";
                //SchemaPath = "C:\\Daten\\TransformationSchema.xls";
                System.IO.FileInfo FIschema = new System.IO.FileInfo(XsltFile);
                System.IO.FileInfo x = new System.IO.FileInfo(XmlFile);
                if (x.Exists) { }
                if (FIschema.Exists)
                {
                    System.Xml.Xsl.XslCompiledTransform XSLT = new System.Xml.Xsl.XslCompiledTransform();
                    XSLT.Load(XsltFile);

                    // Load the file to transform.
                    System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(XmlFile);

                    // The output file:
                    OutputFile = XmlFile.Substring(0, XmlFile.Length - 4) + ".htm";

                    // Create the writer.             
                    System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(OutputFile, XSLT.OutputSettings);

                    // Transform the file and send the output to the console.
                    XSLT.Transform(doc, writer);
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OutputFile = "";
            }
            finally
            {
            }
            return OutputFile;
        }

        #endregion

        #region Attachment

        private void radioButtonImportNewData_Click(object sender, EventArgs e)
        {
            if (this.radioButtonImportNewData.Checked)
            {
                this.setPreviousAttachmentColumn();
                DiversityCollection.Import.AttachmentKeyImportColumn = null;
                DiversityCollection.Import.AddSetting(Import.Setting.Attachment, "");
                this.setAttachmentInterface();
                this.setDataGridColorRange();
                this._PreviousAttachmentImportColum = null;
            }
        }

        private void radioButtonImportAdditionalData_Click(object sender, EventArgs e)
        {
            if (this.radioButtonImportAdditionalData.Checked)
            {
                this.radioButtonAttachAccessionNumber.Checked = true;
                this.radioButtonAttachAccessionNumber_Click(null, null);
            }
        }

        public void setAttachmentInterface()
        {
            this.setProjectSelection();
            if (this._PreviousAttachmentImportColum != null && this._PreviousAttachmentImportColum.Table != null)
            {
                if (DiversityCollection.Import.ImportColumns.ContainsKey(this.PreviousAttachmentKey))
                {
                    DiversityCollection.Import.ImportColumns[this.PreviousAttachmentKey].MustSelect = false;
                    DiversityCollection.Import.ImportColumns[this.PreviousAttachmentKey].ImportColumnControl.setInterface();
                    DiversityCollection.Import.ImportSteps[DiversityCollection.Import.ImportColumns[this.PreviousAttachmentKey].StepKey].setStepError();
                }
                switch (this._PreviousAttachmentImportColum.Table)
                {
                    case "CollectionEventSeries":
                        break;
                    case "CollectionEvent":
                        this.userControlImport_Column_CollectorsEventNumber.Enabled = true;
                        break;
                    case "CollectionSpecimen":
                        if (this._PreviousAttachmentImportColum.Column == "AccessionNumber")
                        {
                            this.userControlImport_Column_SpecimenAccessionNumber.Enabled = true;
                        }
                        else if (this._PreviousAttachmentImportColum.Column == "DepositorsAccessionNumber")
                        {
                            this.userControlImport_Column_Specimen_DepositorsAccessionNumber.Enabled = true;
                        }
                        break;
                    case "CollectionAgent":
                        //this.tabPageCollector.en
                        break;
                }
                if (this._PreviousAttachmentImportColum.Table != null &&
                    (this._PreviousAttachmentImportColum.Table.StartsWith("CollectionEvent") ||
                    this._PreviousAttachmentImportColum.Table == "CollectionSpecimen" ||
                    this._PreviousAttachmentImportColum.Table == "CollectionAgent"||
                    this._PreviousAttachmentImportColum.Table == "CollectionEventSeries"))
                {
                    DiversityCollection.Import.ImportColumns[this.PreviousAttachmentCorrespondingColumKey].MustSelect = false;
                    if (DiversityCollection.Import.ImportColumns[this.PreviousAttachmentCorrespondingColumKey].ImportColumnControl != null)
                        DiversityCollection.Import.ImportColumns[this.PreviousAttachmentCorrespondingColumKey].ImportColumnControl.setInterface();
                }
            }
            if (DiversityCollection.Import.AttachmentKeyImportColumn == null || DiversityCollection.Import.AttachmentKeyImportColumn.Table == null)
            {
                this.radioButtonImportAdditionalData.Checked = false;
                this.radioButtonImportNewData.Checked = true;
                this.tableLayoutPanelKeysForAttachment.Visible = false;

                this.userControlImport_ColumnProject.Import_Column.MustSelect = true;

                this.radioButtonAttachAccessionNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachCollectorsEventNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachCollectionSpecimenID.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachDepositorsAccessionNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachPartAccessionNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachSeriesCode.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachCollectorsNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachPartSublabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachStorageLocation.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachUnitIdentifier.BackColor = System.Drawing.SystemColors.ControlLightLight;
            }
            else
            {
                this.radioButtonImportAdditionalData.Checked = true;
                this.radioButtonImportNewData.Checked = false;
                this.tableLayoutPanelKeysForAttachment.Visible = true;

                this.userControlImport_ColumnProject.Import_Column.MustSelect = false;

                this.radioButtonAttachCollectorsNumber.Checked = false;
                this.radioButtonAttachAccessionNumber.Checked = false;
                this.radioButtonAttachCollectorsEventNumber.Checked = false;
                this.radioButtonAttachDepositorsAccessionNumber.Checked = false;
                this.radioButtonAttachPartAccessionNumber.Checked = false;
                this.radioButtonAttachPartSublabel.Checked = false;
                this.radioButtonAttachSeriesCode.Checked = false;
                this.radioButtonAttachStorageLocation.Checked = false;
                this.radioButtonAttachUnitIdentifier.Checked = false;

                this.radioButtonAttachAccessionNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachDepositorsAccessionNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachCollectionSpecimenID.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachCollectorsEventNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachPartAccessionNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachSeriesCode.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachCollectorsNumber.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachPartSublabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachStorageLocation.BackColor = System.Drawing.SystemColors.ControlLightLight;
                this.radioButtonAttachUnitIdentifier.BackColor = System.Drawing.SystemColors.ControlLightLight;

                if (DiversityCollection.Import.AttachmentKeyImportColumn.Table != null)
                {
                    switch (DiversityCollection.Import.AttachmentKeyImportColumn.Table)
                    {
                        case "CollectionEventSeries":
                            this.radioButtonAttachSeriesCode.Checked = true;
                            this.radioButtonAttachSeriesCode.BackColor = FormImportWizard.ColorForAttachment;
                            this.userControlImport_ColumnProject.Import_Column.MustSelect = true;
                            break;
                        case "CollectionEvent":
                            this.radioButtonAttachCollectorsEventNumber.Checked = true;
                            this.radioButtonAttachCollectorsEventNumber.BackColor = FormImportWizard.ColorForAttachment;
                            this.userControlImport_Column_CollectorsEventNumber.Enabled = false;
                            this.userControlImport_ColumnProject.Import_Column.MustSelect = true;
                            break;
                        case "CollectionAgent":
                            this.radioButtonAttachCollectorsNumber.Checked = true;
                            this.radioButtonAttachCollectorsNumber.BackColor = FormImportWizard.ColorForAttachment;
                            //this.userControlImport_ColumnProject.Import_Column.MustSelect = false;
                            //this.userControlImport_ColumnProject.Import_Column.IsSelected = false;
                            break;
                        case "CollectionSpecimen":
                            if (DiversityCollection.Import.AttachmentKeyImportColumn.Column == "AccessionNumber")
                            {
                                this.radioButtonAttachAccessionNumber.Checked = true;
                                this.radioButtonAttachAccessionNumber.BackColor = FormImportWizard.ColorForAttachment;
                                this.userControlImport_Column_SpecimenAccessionNumber.Enabled = false;
                            }
                            else if (DiversityCollection.Import.AttachmentKeyImportColumn.Column == "DepositorsAccessionNumber")
                            {
                                this.radioButtonAttachDepositorsAccessionNumber.Checked = true;
                                this.radioButtonAttachDepositorsAccessionNumber.BackColor = FormImportWizard.ColorForAttachment;
                                this.userControlImport_Column_Specimen_DepositorsAccessionNumber.Enabled = false;
                            }
                            else if (DiversityCollection.Import.AttachmentKeyImportColumn.Column == "CollectionSpecimenID")
                            {
                                this.radioButtonAttachCollectionSpecimenID.Checked = true;
                                this.radioButtonAttachCollectionSpecimenID.BackColor = FormImportWizard.ColorForAttachment;
                                //this.userControlImport_Column_Specimen_DepositorsAccessionNumber.Enabled = false;
                            }
                            //this.userControlImport_ColumnProject.Import_Column.MustSelect = false;
                            //this.userControlImport_ColumnProject.Import_Column.IsSelected = false;
                            break;
                        case "CollectionSpecimenPart":
                            if (DiversityCollection.Import.AttachmentKeyImportColumn.Column == "AccessionNumber")
                            {
                                this.radioButtonAttachPartAccessionNumber.Checked = true;
                                this.radioButtonAttachPartAccessionNumber.BackColor = FormImportWizard.ColorForAttachment;
                            }
                            else if (DiversityCollection.Import.AttachmentKeyImportColumn.Column == "PartSublabel")
                            {
                                this.radioButtonAttachPartSublabel.Checked = true;
                                this.radioButtonAttachPartSublabel.BackColor = FormImportWizard.ColorForAttachment;
                            }
                            else
                            {
                                this.radioButtonAttachStorageLocation.Checked = true;
                                this.radioButtonAttachStorageLocation.BackColor = FormImportWizard.ColorForAttachment;
                            }
                            //this.userControlImport_ColumnProject.Import_Column.MustSelect = false;
                            //this.userControlImport_ColumnProject.Import_Column.IsSelected = false;
                            break;
                        case "IdentifictionUnit":
                            this.radioButtonAttachUnitIdentifier.Checked = true;
                            this.radioButtonAttachUnitIdentifier.BackColor = FormImportWizard.ColorForAttachment;
                            //this.userControlImport_ColumnProject.Import_Column.MustSelect = false;
                            //this.userControlImport_ColumnProject.Import_Column.IsSelected = false;
                            break;
                        default:
                            System.Windows.Forms.MessageBox.Show("No valid table");
                            break;
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> IS in DiversityCollection.Import.ImportSteps)
                {
                    if (IS.Value.TableName() == DiversityCollection.Import.AttachmentKeyImportColumn.Table && IS.Value.StepParallelNumber() > 1)
                    {
                        IS.Value.setImportStepVisibility(false);
                    }
                }
            }
            this.userControlImport_ColumnProject.setInterface();
            this.userControlImport_Column_Attach.initUserControl(DiversityCollection.Import.AttachmentKeyImportColumn, this.Import);
            if (DiversityCollection.Import.AttachmentKeyImportColumn != null && DiversityCollection.Import.AttachmentKeyImportColumn.Table != null)
            {
                this.userControlImport_Column_Attach.Visible = true;
                this.setCurrentImportColumn(DiversityCollection.Import.AttachmentKeyImportColumn);
            }
            else this.userControlImport_Column_Attach.Visible = false;

            this.setDataGridColorRange();

        }
        
        /// <summary>
        /// returns the first Import_Column for the Table + Column combination if the Dictionary contains one, otherwise null
        /// </summary>
        /// <param name="Table">the name of the table</param>
        /// <param name="Column">the name of the column</param>
        /// <returns>The Import_Column</returns>
        private DiversityCollection.Import_Column getAttachmentImportColumn(string Table, string Column)
        {
            DiversityCollection.Import_Column AttachmentColumnTemplate = new Import_Column();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in DiversityCollection.Import.ImportColumns)
            {
                if (KV.Value.Table == Table && KV.Value.Column == Column)
                {
                    if (KV.Value.Sequence() == 0)
                        return KV.Value;
                    else
                        AttachmentColumnTemplate = KV.Value;
                }
            }
            if (AttachmentColumnTemplate.Table != null)
            {
                DiversityCollection.Import.AttachmentKeyImportColumn = DiversityCollection.Import_Column.GetImportColumn(AttachmentColumnTemplate.StepKey, 
                    AttachmentColumnTemplate.Table, AttachmentColumnTemplate.TableAlias, 
                    AttachmentColumnTemplate.Column, 0,  this.userControlImport_Column_Attach,AttachmentColumnTemplate.TypeOfSource, 
                    AttachmentColumnTemplate.TypeOfFixing, AttachmentColumnTemplate.TypeOfEntry);
                DiversityCollection.Import.AttachmentKeyImportColumn.TypeOfSource = Import_Column.SourceType.File;
                return DiversityCollection.Import.AttachmentKeyImportColumn;
            }
            return null;
        }

        #region radioButton events
        
        private void radioButtonAttachSeriesCode_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachSeriesCode.Checked)
                this.setAttachmentKey("CollectionEventSeries", "SeriesCode");
        }

        private void radioButtonAttachAccessionNumber_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachAccessionNumber.Checked)
                this.setAttachmentKey("CollectionSpecimen", "AccessionNumber");
        }

        private void radioButtonAttachCollectorsEventNumber_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachCollectorsEventNumber.Checked)
                this.setAttachmentKey("CollectionEvent", "CollectorsEventNumber");
        }

        private void radioButtonAttachDepositorsAccessionNumber_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachDepositorsAccessionNumber.Checked)
                this.setAttachmentKey("CollectionSpecimen", "DepositorsAccessionNumber");
        }

        private void radioButtonAttachCollectionSpecimenID_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachCollectionSpecimenID.Checked)
                this.setAttachmentKey("CollectionSpecimen", "CollectionSpecimenID");
        }

        private void radioButtonAttachPartAccessionNumber_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachPartAccessionNumber.Checked)
                this.setAttachmentKey("CollectionSpecimenPart", "AccessionNumber");
        }

        private void radioButtonAttachCollectorsNumber_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachCollectorsNumber.Checked)
                this.setAttachmentKey("CollectionAgent", "CollectorsNumber");
        }

        private void radioButtonAttachUnitIdentifier_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachUnitIdentifier.Checked)
                this.setAttachmentKey("IdentificationUnit", "UnitIdentifier");
        }

        private void radioButtonAttachPartSublabel_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachPartSublabel.Checked)
                this.setAttachmentKey("CollectionSpecimenPart", "PartSublabel");
        }

        private void radioButtonAttachStorageLocation_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttachStorageLocation.Checked)
                this.setAttachmentKey("CollectionSpecimenPart", "StorageLocation");
        }
        
        #endregion

        private void setPreviousAttachmentColumn()
        {
            if (DiversityCollection.Import.AttachmentKeyImportColumn != null)
            {
                this._PreviousAttachmentImportColum = new Import_Column();
                if (Import.AttachmentKeyImportColumn.Table != null)
                {
                    this._PreviousAttachmentImportColum.Table = Import.AttachmentKeyImportColumn.Table;
                    this._PreviousAttachmentImportColum.TableAlias = Import.AttachmentKeyImportColumn.TableAlias;
                    this._PreviousAttachmentImportColum.Column = Import.AttachmentKeyImportColumn.Column;
                    this._PreviousAttachmentImportColum.setSequence(0);
                }
            }
        }

        public void setAttachmentKey(string Table, string Column)
        {
            this.setPreviousAttachmentColumn();
            DiversityCollection.Import.AttachmentKeyImportColumn = this.getAttachmentImportColumn(Table, Column);
            if (DiversityCollection.Import.AttachmentKeyImportColumn != null)
            {
                DiversityCollection.Import.AttachmentKeyImportColumn.setSequence(0);
                DiversityCollection.Import.AddSetting(Import.Setting.Attachment, DiversityCollection.Import.AttachmentKeyImportColumn.Key);
                DiversityCollection.Import.ImportColumns[Import.AttachmentKeyImportColumn.Key].MustSelect = true;
                DiversityCollection.Import.ImportColumns[Import.AttachmentKeyImportColumn.Key].ImportColumnControl.setInterface();
                DiversityCollection.Import.ImportSteps[Import.AttachmentKeyImportColumn.StepKey].setStepError();
            }
            else if (DiversityCollection.Import.AttachmentKeyImportColumn == null &&
                Table == "CollectionSpecimen" &&
                Column == "CollectionSpecimenID")
            {
                DiversityCollection.Import_Column ICCollectionSpecimenID = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession), "CollectionSpecimen", "CollectionSpecimen", "CollectionSpecimenID", 1, null
                    , Import_Column.SourceType.Database, Import_Column.FixingType.None, Import_Column.EntryType.Database); // new Import_Column();
                ICCollectionSpecimenID.IsSelected = true;
                ICCollectionSpecimenID.CanBeTransformed = false;
                DiversityCollection.Import.AttachmentKeyImportColumn = this.getAttachmentImportColumn(Table, Column);
                DiversityCollection.Import.AddSetting(Import.Setting.Attachment, DiversityCollection.Import.AttachmentKeyImportColumn.Key);
            }
            else
            {
                DiversityCollection.Import.AddSetting(Import.Setting.Attachment, "");
            }
            if (this.PreviousAttachmentKey.Length > 0)
            {
                if (DiversityCollection.Import.ImportColumns.ContainsKey(this.PreviousAttachmentKey))
                {
                    DiversityCollection.Import.ImportColumns[this.PreviousAttachmentKey].ImportColumnControl.setInterface();
                    if (DiversityCollection.Import.ImportSteps.ContainsKey(DiversityCollection.Import.ImportColumns[this.PreviousAttachmentKey].StepKey))
                        DiversityCollection.Import.ImportSteps[DiversityCollection.Import.ImportColumns[this.PreviousAttachmentKey].StepKey].setStepError();
                }
                if (DiversityCollection.Import.ImportColumns.ContainsKey(this.PreviousAttachmentCorrespondingColumKey) &&
                    DiversityCollection.Import.ImportColumns[this.PreviousAttachmentCorrespondingColumKey].ImportColumnControl != null)
                {

                    DiversityCollection.Import.ImportColumns[this.PreviousAttachmentCorrespondingColumKey].ImportColumnControl.setInterface();
                }
            }
            this.setAttachmentInterface();
        }

        public DiversityCollection.UserControls.UserControlImport_Column UserControlImportAttachmentColumn()
        {
            return this.userControlImport_Column_Attach;
        }

        #endregion
        
        #region Project

        private void initProject()
        {
            try
            {
                string StepKey = DiversityCollection.Import.getImportStepKey(Import.ImportStep.Project);
                DiversityCollection.Import_Step IF =
                    DiversityCollection.Import_Step.GetImportStep(
                    "Project",
                    "The project of the imported data",
                    StepKey,
                    "CollectionProject",
                    null,
                    null,
                    0,
                    (UserControls.iUserControlImportInterface)this,
                    this.StepImage(DiversityCollection.Import.ImportStep.Project),
                    this.panelImportSeletion);//null);//this.tabPageSource);

                IF.MustImport = true;
                IF.UserControlImportStep.MustImportCanBeChanged = false;
                foreach (System.Windows.Forms.Control C in this.panelImportSeletion.Controls)
                {
                    DiversityCollection.UserControls.UserControlImportSelector U = (DiversityCollection.UserControls.UserControlImportSelector)C;
                    bool ControlFound = false;
                    foreach (DiversityCollection.Import_Step IS in U.ImportSteps())
                    {
                        if (IS.TableName() == "CollectionProject")
                        {
                            U.Enabled = false;
                            ControlFound = true;
                            break;
                        }
                    }
                    if (ControlFound) break;
                }
                this.initProject(StepKey);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void initProject(string StepKey)
        {
            try
            {
                DiversityCollection.Import_Column CProject = DiversityCollection.Import_Column.GetImportColumn(StepKey
                    , "CollectionProject", "ProjectID", (DiversityCollection.iImportColumnControl)this.userControlImport_ColumnProject);
                CProject.TypeOfEntry = DiversityCollection.Import_Column.EntryType.MandatoryList;
                CProject.TypeOfFixing = DiversityCollection.Import_Column.FixingType.Schema;
                CProject.TypeOfSource = Import_Column.SourceType.Any;
                CProject.CanBeTransformed = false;
                CProject.MustSelect = true;
                CProject.setLookupTable(this.DtProject, "Project", "ProjectID");
                CProject.setDisplayTitle("Project");
                CProject.StepKey = StepKey;
                this.userControlImport_ColumnProject.initUserControl(CProject, this.Import);
                this.userControlImport_ColumnProject.setInterface();
                DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.Project)].setStepError();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Data.DataTable _DtProject;
        private System.Data.DataTable DtProject
        {
            get
            {
                if (this._DtProject == null)
                {
                    this._DtProject = new System.Data.DataTable("Projects");
                    string SQL = "SELECT NULL AS ProjectID, NULL AS Project UNION SELECT ProjectID, Project FROM ProjectList ORDER BY Project";
                    System.Data.SqlClient.SqlDataAdapter a = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        a.Fill(this._DtProject);
                    }
                    catch { }
                }
                return this._DtProject;
            }
        }
        
        #region Project Selection

        private void setProjectSelection()
        {
            DiversityCollection.UserControls.UserControlImportSelector U = (DiversityCollection.UserControls.UserControlImportSelector)this.panelImportSeletion.Controls[0];
            U.setStepSelectionProject();
        }

        #endregion

        #endregion

        #region Series

        private void initSeries()
        {
            try
            {
                DiversityCollection.Import_Step IF = DiversityCollection.Import_Step.GetImportStep(
                    "Series",
                    "The collection event series of the imported data",
                    DiversityCollection.Import.getImportStepKey(Import.ImportStep.Series),
                    "CollectionEventSeries",
                    null,
                    null, 0,
                    this,
                    this.StepImage(DiversityCollection.Import.ImportStep.Series),
                    this.panelImportSeletion);

                foreach (System.Windows.Forms.Control C in this.panelImportSeletion.Controls)
                {
                    DiversityCollection.UserControls.UserControlImportSelector U = (DiversityCollection.UserControls.UserControlImportSelector)C;
                    bool ControlFound = false;
                    foreach (DiversityCollection.Import_Step IS in U.ImportSteps())
                    {
                        if (IS.TableName() == "CollectionEventSeries")
                        {
                            U.setSelection(false);
                            U.setStepSelection();
                            ControlFound = true;
                            break;
                        }
                    }
                    if (ControlFound) break;
                }

                DiversityCollection.Import_Column ICSeriesCode = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStep.Series)
                    , "CollectionEventSeries", "SeriesCode", this.userControlImport_Column_SeriesCode);
                ICSeriesCode.TypeOfEntry = Import_Column.EntryType.Text;
                ICSeriesCode.TypeOfFixing = Import_Column.FixingType.None;
                this.userControlImport_Column_SeriesCode.initUserControl(ICSeriesCode, this.Import);

                DiversityCollection.Import_Column ICSeriesDescription = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStep.Series)
                    , "CollectionEventSeries", "Description", this.userControlImport_Column_SeriesDescription);
                ICSeriesDescription.TypeOfEntry = Import_Column.EntryType.Text;
                ICSeriesDescription.TypeOfFixing = Import_Column.FixingType.None;
                this.userControlImport_Column_SeriesDescription.initUserControl(ICSeriesDescription, this.Import);

                DiversityCollection.Import_Column ICSeriesNotes = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStep.Series)
                    , "CollectionEventSeries", "Notes", this.userControlImport_Column_SeriesNotes);
                ICSeriesNotes.TypeOfEntry = Import_Column.EntryType.Text;
                ICSeriesNotes.TypeOfFixing = Import_Column.FixingType.None;
                this.userControlImport_Column_SeriesNotes.initUserControl(ICSeriesNotes, this.Import);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string SeriesOK
        {
            get
            {
                if (this.radioButtonEventSeriesNone.Checked) return "";
                if (this.radioButtonEventSeriesNew.Checked && this.textBoxEventSeriesDescription.Text.Length == 0) return "Please enter description for event series";
                if (this.radioButtonEventSeriesSelect.Checked && this._SeriesID == null) return "No series selected";
                return "";
            }
        }

        private void textBoxEventSeriesDescription_TextChanged(object sender, EventArgs e)
        {
            DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.Series)].setStepError(this.SeriesOK);
            //this.UpdateImportStep(this.SeriesOK);
        }

        private void radioButtonEventSeriesNew_CheckedChanged(object sender, EventArgs e)
        {
            this.setEventSeriesControls();
            DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.Series)].setStepError(this.SeriesOK);
            //this.UpdateImportStep(this.SeriesOK);
        }

        private void radioButtonEventSeriesSelect_CheckedChanged(object sender, EventArgs e)
        {
            this.setEventSeriesControls();
            DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.Series)].setStepError(this.SeriesOK);
            //this.UpdateImportStep(this.SeriesOK);
        }

        private void buttonEventSeriesSelect_Click(object sender, EventArgs e)
        {
            DiversityCollection.FormCollectionEventSeries f = new FormCollectionEventSeries(this._SeriesID);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this._SeriesID = f.ID;
            DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.Series)].setStepError(this.SeriesOK);
            //this.UpdateImportStep(this.SeriesOK);
        }

        private void setEventSeriesControls()
        {
            this.tableLayoutPanelEventSeriesNew.Visible = this.radioButtonEventSeriesNew.Checked;
            this.buttonEventSeriesSelect.Visible = this.radioButtonEventSeriesSelect.Checked;
        }

        #endregion

        #region Event

        #region Common functions

        private void initEvent()
        {
            try
            {
                this.treeViewEventsInGroups.ExpandAll();
                this.treeViewEventsSeparate.ExpandAll();

                DiversityCollection.Import_Step IE = DiversityCollection.Import_Step.GetImportStep(
                    "Collection event",
                    "The collection event of the imported data",
                    DiversityCollection.Import.getImportStepKey(Import.ImportStep.Event),
                    "CollectionEvent",
                    null,
                    null, 0,
                    (UserControls.iUserControlImportInterface)this,
                    this.StepImage(DiversityCollection.Import.ImportStep.Event),
                    this.panelImportSeletion);
                IE.IsGroupHaeder = true;
                //DiversityCollection.Import_Step IEHierarchy = DiversityCollection.Import_Step.GetImportStep("Hierarchy", "The hierarchy of the collection event", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Hierarchy), 1, this.tabControlCollectionEvent, this.tabPageEventHierarchy);
                //IE.DependentImportSteps.Add(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Hierarchy), IEHierarchy);

                this.initEventLocalisation();

                DiversityCollection.Import_Step IED = DiversityCollection.Import_Step.GetImportStep("Collection date", "The date and time of the collection event",
                    DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date),
                        "CollectionEvent",
                        null,
                    IE, 1,
                    (UserControls.iUserControlImportInterface)this,
                    this.StepImage(DiversityCollection.Import.ImportStepEvent.Date), this.panelEventSelection);

                DiversityCollection.Import_Step IEL = DiversityCollection.Import_Step.GetImportStep("Locality", "The locality",
                    DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Locality), "CollectionEvent",
                    null,
                    IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.Country), this.panelEventSelection);

                DiversityCollection.Import_Step IEA = DiversityCollection.Import_Step.GetImportStep("Altitude", "The altitude of the locality", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Altitude),
                    "CollectionEventLocalisation",
                    null,
                    IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.Altitide), this.panelEventSelection);
                IEA.setTableAlias(this.userControlImportLocalisationAltitude.TableAlias);

                DiversityCollection.Import_Step IEC = DiversityCollection.Import_Step.GetImportStep("Coordinates", "The coordinates of the locality", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Coordinates),
                    "CollectionEventLocalisation",
                    null,
                    IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.Localisation), this.panelEventSelection);
                IEC.setTableAlias(this.userControlImportLocalisationCoordinates.TableAlias);

                DiversityCollection.Import_Step IEP = DiversityCollection.Import_Step.GetImportStep("Place", "The named place of the locality", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Place),
                    "CollectionEventLocalisation",
                    null,
                    IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.Place), this.panelEventSelection);
                IEP.setTableAlias(this.userControlImportLocalisationPlace.TableAlias);

                DiversityCollection.Import_Step IETK25 = DiversityCollection.Import_Step.GetImportStep("TK25", "The MTB and quadrant", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.MTB),
                    "CollectionEventLocalisation",
                    null,
                    IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.MTB), this.panelEventSelection);
                IETK25.setTableAlias(this.userControlImportLocalisationMTB.TableAlias);

                DiversityCollection.Import_Step IEGK = DiversityCollection.Import_Step.GetImportStep("Gauss Krüger", "The Gauss Krüger coordinates", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.GaussKrueger),
                    "CollectionEventLocalisation",
                    null, IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.GaussKrueger), this.panelEventSelection);
                IEGK.setTableAlias(this.userControlImportLocalisationGK.TableAlias);

                DiversityCollection.Import_Step IEDepth = DiversityCollection.Import_Step.GetImportStep("Depth", "The depth underneath the surface", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Depth),
                    "CollectionEventLocalisation",
                    null, IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.Depth), this.panelEventSelection);
                IEDepth.setTableAlias(this.userControlImportLocalisationDepth.TableAlias);

                DiversityCollection.Import_Step IEH = DiversityCollection.Import_Step.GetImportStep("Height", "The height above ground level", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Height),
                    "CollectionEventLocalisation",
                    null, IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.Height), this.panelEventSelection);
                IEH.setTableAlias(this.userControlImportLocalisationHeight.TableAlias);

                DiversityCollection.Import_Step IEEx = DiversityCollection.Import_Step.GetImportStep("Exposition", "The exposition of the collection site", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Exposition),
                    "CollectionEventLocalisation",
                    null, IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.Exposition), this.panelEventSelection);
                IEEx.setTableAlias(this.userControlImportLocalisationExposition.TableAlias);

                DiversityCollection.Import_Step IES = DiversityCollection.Import_Step.GetImportStep("Slope", "The slope of the collection site", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Slope),
                    "CollectionEventLocalisation",
                    null, IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.Slope), this.panelEventSelection);
                IES.setTableAlias(this.userControlImportLocalisationSlope.TableAlias);

                DiversityCollection.Import_Step IESP = DiversityCollection.Import_Step.GetImportStep("Plot", "The sampling plot of the collection site", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Plot),
                    "CollectionEventLocalisation",
                    null, IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForLocalisationSystem(Specimen.OverviewImageLocalisationSystem.SamplingPlot), this.panelEventSelection);
                IESP.setTableAlias(this.userControlImportLocalisationPlot.TableAlias);



                this.initEventProperty();

                DiversityCollection.Import_Step IELitho = DiversityCollection.Import_Step.GetImportStep("Lithostratigraphy", "The lithostratigraphy of the collection site", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Lithostratigraphy),
                    "CollectionEventProperty",
                    null, IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForCollectionEventProperty(false, "Stratigraphy"), this.panelEventSelection);
                IELitho.setTableAlias(this.userControlImportEventPropertyLithostratigraphy.TableAlias);

                DiversityCollection.Import_Step IEChrono = DiversityCollection.Import_Step.GetImportStep("Chronostratigraphy", "The chronostratigraphy of the collection site", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Chronostratigraphy),
                    "CollectionEventProperty",
                    null, IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForCollectionEventProperty(false, "Stratigraphy"), this.panelEventSelection);
                IEChrono.setTableAlias(this.userControlImportEventPropertyChronostratigraphy.TableAlias);

                this.initEventDate();
                this.initEventLocality();

                ///TODO:
                DiversityCollection.Import_Step IEMethod = DiversityCollection.Import_Step.GetImportStep("Method", "The method used for collection of the specimen", DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Method),
                    "CollectionEventMethod",
                    null, IE, 1, (UserControls.iUserControlImportInterface)this,
                    DiversityCollection.Specimen.ImageForTable("CollectionEventMethod", false), this.panelEventSelection);

                //this.initEventMethod(IE);
                this.userControlImportMethod_Event.ImportStepMethod = IEMethod;

                this.userControlImportMethod_Event.initUserControl(this, IE);

                /*
                 * LocalisationSystemID	LocalisationSystemName
                    1	Top50 (deutsche Landesvermessung)
                    2	Gauss-Krüger coordinates
                    3	MTB (A, CH, D)
                    4	Altitude (mNN)
                    5	mNN (barometric)
                    6	Greenwich Coordinates
                    7	Named area (DiversityGazetteer)
                    8	Coordinates WGS84
                    9	Coordinates
                    10	Exposition
                    11	Slope
                    12	Coordinates PD
                    13	Sampling plot
                    14	Depth
                    15	Height
                    16	Dutch RD coordinates

                 * 
                 * * PropertyID	PropertyName
                    1	European Nature Information System (EUNIS)
                    10	Geographic regions
                    20	Chronostratigraphy
                    30	Lithostratigraphy
                    40	Lebensraumtypen (LfU)
                    50	Pflanzengesellschaften
                 */
            }
            catch (System.Exception ex) { }
        }


        #endregion

        #region Hierarchy
        
        private string EventHierachyOK
        {
            get
            {
                return "";
            }
        }
        
        private void radioButtonEventsSeparate_CheckedChanged(object sender, EventArgs e)
        {
            this.treeViewEventsInGroups.Enabled = !this.radioButtonEventsSeparate.Checked;
            this.treeViewEventsSeparate.Enabled = this.radioButtonEventsSeparate.Checked;
            if (this.radioButtonEventsSeparate.Checked)
            {
                this.treeViewEventsInGroups.ImageList = this.imageListEventTreeGray;
                this.treeViewEventsSeparate.ImageList = this.imageListEventTree;
                this.treeViewEventsInGroups.BackColor = System.Drawing.SystemColors.Control;
                this.treeViewEventsSeparate.BackColor = System.Drawing.SystemColors.Window;
                //this.Import.AddSetting(Import.Settings.EventHierarchySeparate.ToString(), true.ToString());
            }
            else
            {
                this.treeViewEventsInGroups.ImageList = this.imageListEventTree;
                this.treeViewEventsSeparate.ImageList = this.imageListEventTreeGray;
                this.treeViewEventsInGroups.BackColor = System.Drawing.SystemColors.Window;
                this.treeViewEventsSeparate.BackColor = System.Drawing.SystemColors.Control;
                //this.Import.AddSetting(Import.Settings.EventHierarchyGroups.ToString(), true.ToString());
            }
        }

        #endregion

        #region Date

        private void initEventDate()
        {
            DiversityCollection.Import_Column ICD = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date)
                ,"CollectionEvent", "CollectionDay", this.userControlImportDateCollectionDate.userControlImport_ColumnDay);
            ICD.CanBeTransformed = true;
            DiversityCollection.Import_Column ICM = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date)
                , "CollectionEvent",  "CollectionMonth", this.userControlImportDateCollectionDate.userControlImport_ColumnMonth);
            ICM.CanBeTransformed = true;
            DiversityCollection.Import_Column ICY = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date)
                ,"CollectionEvent","CollectionYear" , this.userControlImportDateCollectionDate.userControlImport_ColumnYear);
            ICY.CanBeTransformed = true;
            DiversityCollection.Import_Column ICDate = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date)
                , "CollectionEvent", "CollectionDateSupplement", this.userControlImportDateCollectionDate.userControlImport_ColumnSupplement);
            ICDate.CanBeTransformed = true;
            this.userControlImportDateCollectionDate.initUserControl(ICD, ICM, ICY, ICDate,
                DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date), this.Import, this);

            this.tableLayoutPanelEventDate.SendToBack();

            DiversityCollection.UserControls.UserControlImport_Column UC_CollectionTime = new UserControls.UserControlImport_Column();
            DiversityCollection.Import_Column ICCollectionTime = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date)
                , "CollectionEvent", "CollectionTime", this.userControlImportDateCollectionDate.userControlImport_ColumnSupplement);
            ICCollectionTime.CanBeTransformed = true;
            ICCollectionTime.TypeOfEntry = Import_Column.EntryType.Text;
            ICCollectionTime.TypeOfFixing = Import_Column.FixingType.None;
            ICCollectionTime.TypeOfSource = Import_Column.SourceType.File;
            UC_CollectionTime.initUserControl(ICCollectionTime, this._Import);
            UC_CollectionTime.Dock = DockStyle.Top;
            this.tabPageEventDate.Controls.Add(UC_CollectionTime);
            UC_CollectionTime.BringToFront();


            DiversityCollection.UserControls.UserControlImport_Column UC_CollectionTimeSpan = new UserControls.UserControlImport_Column();
            DiversityCollection.Import_Column ICCollectionTimeSpan = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date)
                , "CollectionEvent", "CollectionTimeSpan", this.userControlImportDateCollectionDate.userControlImport_ColumnSupplement);
            ICCollectionTimeSpan.CanBeTransformed = true;
            ICCollectionTimeSpan.TypeOfEntry = Import_Column.EntryType.Text;
            ICCollectionTimeSpan.TypeOfFixing = Import_Column.FixingType.None;
            ICCollectionTimeSpan.TypeOfSource = Import_Column.SourceType.File;
            UC_CollectionTimeSpan.initUserControl(ICCollectionTimeSpan, this._Import);
            UC_CollectionTimeSpan.Dock = DockStyle.Top;
            this.tabPageEventDate.Controls.Add(UC_CollectionTimeSpan);
            UC_CollectionTimeSpan.BringToFront();


        }

        //private string EventDateOK
        //{
        //    get
        //    {
        //        //if (this.radioButtonCollectionTime.Checked)
        //        //{
        //        //    if (this.Import.ImportContainsColumn("CollectionEvent", "CollectionTime"))
        //        //        return "";
        //        //    else
        //        //        return "No column for a collection time is selected";
        //        //}
        //        return "";
        //    }
        //}

        #endregion

        #region Locality

        private void initEventLocality()
        {
            try
            {
                System.Data.DataTable dtCountry = new DataTable();
                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey("DiversityGazetteer")
                    && DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].DatabaseList().Count > 0)
                {
                    string GeoDatabase = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityGazetteer"].DatabaseList()[0];
                    string SQL = "SELECT N.Name AS Country " +
                        "FROM  " + GeoDatabase + ".dbo.GeoName AS N INNER JOIN " +
                        GeoDatabase + ".dbo.GeoPlace AS P ON N.PlaceID = P.PlaceID AND N.NameID = P.PreferredNameID " +
                        "WHERE (P.PlaceType = 'nation') " +
                        "ORDER BY N.Name";
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(dtCountry);
                    }
                    catch (System.Exception ex) { }
                    if (dtCountry.Rows.Count == 0)
                    {
                        ad.SelectCommand.CommandText = "SELECT DISTINCT CountryCache AS Country FROM CollectionEvent WHERE CountryCache <> N'' ORDER BY CountryCache";
                        try
                        {
                            ad.Fill(dtCountry);
                        }
                        catch (System.Exception ex) { }
                    }
                }
                DiversityCollection.Import_Column ICCountry = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Locality)
                    , "CollectionEvent", "CountryCache", this.userControlImport_Column_Country);
                ICCountry.TypeOfEntry = Import_Column.EntryType.ListAndText;
                ICCountry.TypeOfFixing = Import_Column.FixingType.Schema;
                ICCountry.setLookupTable(dtCountry, "Country", "Country");
                this.userControlImport_Column_Country.initUserControl(ICCountry, this.Import);
                this.userControlImport_Column_Country.setInterface();

                DiversityCollection.Import_Column ICLocality = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Locality)
                    , "CollectionEvent", "LocalityDescription", this.userControlImport_Column_Locality);
                ICLocality.TypeOfEntry = Import_Column.EntryType.Text;
                ICLocality.TypeOfFixing = Import_Column.FixingType.Import;
                ICLocality.MultiColumn = true;
                this.userControlImport_Column_Locality.initUserControl(ICLocality, this.Import);

                DiversityCollection.Import_Column ICE = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Locality)
                    , "CollectionEvent", "CollectorsEventNumber", this.userControlImport_Column_CollectorsEventNumber);
                ICE.TypeOfEntry = Import_Column.EntryType.Text;
                ICE.TypeOfFixing = Import_Column.FixingType.None;
                this.userControlImport_Column_CollectorsEventNumber.initUserControl(ICE, this.Import);

                DiversityCollection.Import_Column ICH = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Locality)
                    , "CollectionEvent", "HabitatDescription", this.userControlImport_Column_Habitat);
                ICH.TypeOfEntry = Import_Column.EntryType.Text;
                ICH.TypeOfFixing = Import_Column.FixingType.Import;
                ICH.MultiColumn = true;
                this.userControlImport_Column_Habitat.initUserControl(ICH, this.Import);

                DiversityCollection.Import_Column ICCollectingMethod = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Locality)
                    , "CollectionEvent", "CollectingMethod", this.userControlImport_Column_CollectingMethod);
                ICCollectingMethod.TypeOfEntry = Import_Column.EntryType.Text;
                ICCollectingMethod.TypeOfFixing = Import_Column.FixingType.Schema;
                this.userControlImport_Column_CollectingMethod.initUserControl(ICCollectingMethod, this.Import);

                DiversityCollection.UserControls.UserControlImport_Column userControlImport_Column_CollectionEvent_Notes = new UserControls.UserControlImport_Column();
                DiversityCollection.Import_Column ICCollectionEventNotes = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Locality)
                    , "CollectionEvent", "Notes", userControlImport_Column_CollectionEvent_Notes);
                ICCollectionEventNotes.TypeOfEntry = Import_Column.EntryType.Text;
                ICCollectionEventNotes.TypeOfFixing = Import_Column.FixingType.None;
                userControlImport_Column_CollectionEvent_Notes.initUserControl(ICCollectionEventNotes, this.Import);
                userControlImport_Column_CollectionEvent_Notes.Dock = DockStyle.Top;
                this.panelEventLocality.Controls.Add(userControlImport_Column_CollectionEvent_Notes);
                userControlImport_Column_CollectionEvent_Notes.BringToFront();
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Event localisation

        private void initEventLocalisation()
        {
            this.userControlImportLocalisationAltitude.initControl(4, this, this.Import);
            this.userControlImportLocalisationCoordinates.initControl(8, this, this.Import);
            this.userControlImportLocalisationPlace.initControl(7, this, this.Import);
            this.userControlImportLocalisationMTB.initControl(3, this, this.Import);
            this.userControlImportLocalisationHeight.initControl(15, this, this.Import);
            this.userControlImportLocalisationGK.initControl(2, this, this.Import);
            this.userControlImportLocalisationDepth.initControl(14, this, this.Import);
            this.userControlImportLocalisationExposition.initControl(10, this, this.Import);
            this.userControlImportLocalisationSlope.initControl(11, this, this.Import);
            this.userControlImportLocalisationPlot.initControl(13, this, this.Import);
        }
       
        #endregion

        #region Site Property

        private void initEventProperty()
        {
            this.userControlImportEventPropertyChronostratigraphy.initControl(20, this, this.Import);
            this.userControlImportEventPropertyLithostratigraphy.initControl(30, this, this.Import);
        }

        #endregion

        #region Collection event method

        private void initEventMethod(DiversityCollection.Import_Step IE)
        {
            //string SQL = "SELECT MethodID, DisplayText " +
            //    "FROM Method " +
            //    "WHERE (OnlyHierarchy = 0 or OnlyHierarchy IS NULL) AND (ForCollectionEvent = 1) " +
            //    "ORDER BY DisplayText";
            //System.Data.DataTable dtMethod = new DataTable();
            //System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //ad.Fill(dtMethod);
            //DiversityCollection.Import_Column ICEventMethod = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Method)
            //    , "CollectionEventMethod", "MethodID", this.userControlImport_Column_CollectionEventMethod);
            //ICEventMethod.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            //ICEventMethod.TypeOfFixing = Import_Column.FixingType.Schema;
            //ICEventMethod.TypeOfSource = Import_Column.SourceType.Interface;
            //ICEventMethod.setLookupTable(dtMethod, "DisplayText", "MethodID");
            //this.userControlImport_Column_CollectionEventMethod.initUserControl(ICEventMethod, this.Import);
            //this.userControlImport_Column_CollectionEventMethod.setInterface();

            //this.userControlImport_Column_CollectionEventMethod.comboBoxForAll.SelectedIndexChanged += this.initEventMethodParameter;

        }

        private void initEventMethodParameter(object sender, EventArgs e)
        {
            //this.panelCollectionEventMethodParameterValues.Controls.Clear();
            //if (this.userControlImport_Column_CollectionEventMethod.comboBoxForAll.SelectedItem == null)
            //    return;

            //System.Data.DataRowView RM = (System.Data.DataRowView)this.userControlImport_Column_CollectionEventMethod.comboBoxForAll.SelectedItem;
            //int MethodID;
            //if (int.TryParse(RM["MethodID"].ToString(), out MethodID))
            //{
            //    string SQL = "SELECT MethodID, ParameterID, DisplayText, Description, DefaultValue " +
            //        "FROM Parameter " +
            //        "WHERE MethodID = " + MethodID.ToString();
            //    System.Data.DataTable dtMethod = new DataTable();
            //    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //    ad.Fill(dtMethod);
            //    foreach (System.Data.DataRow R in dtMethod.Rows)
            //    {
            //        DiversityCollection.UserControls.UserControlImport_Column UC = new UserControls.UserControlImport_Column();
            //        int ParameterID;
            //        if (int.TryParse(R["ParameterID"].ToString(), out ParameterID))
            //        {
            //            DiversityCollection.Import_Step _SuperiorImportStep = DiversityCollection.Import_Step.GetImportStep(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Method));

            //            int NextImportStepNumber = DiversityCollection.Import_Step.getNextImportStepNumber(_SuperiorImportStep, 1);//(int)Import.ImportStepEventMethod.Parameter);

            //            string StepKey = DiversityCollection.Import_Step.getNextImportStepKey((int)DiversityCollection.Import.ImportStepUnit.Identification, _SuperiorImportStep, NextImportStepNumber);

            //            DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
            //                R["DisplayText"].ToString(),
            //                R["Description"].ToString(),
            //                StepKey,
            //                "CollectionEventParameterValue",
            //                NextImportStepNumber,
            //                _SuperiorImportStep,
            //                2,
            //                (UserControls.iUserControlImportInterface)this,
            //                DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Tool), null
            //                );

            //            DiversityCollection.UserControls.UserControlImport_Column UC_Parameter = new UserControls.UserControlImport_Column();
            //            DiversityCollection.Import_Column ICEventParameter = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Method)
            //                , "CollectionEventParameterValue", "Value", UC_Parameter);

            //            ICEventParameter.TypeOfFixing = Import_Column.FixingType.Schema;
            //            ICEventParameter.MustSelect = false;
            //            ICEventParameter.TypeOfSource = Import_Column.SourceType.Any;

            //            SQL = "SELECT Value, DisplayText " +
            //                "FROM  ParameterValue_Enum " +
            //                "WHERE ParameterID = " + ParameterID.ToString();
            //            System.Data.DataTable dtParameter = new DataTable();
            //            ad.SelectCommand.CommandText = SQL;
            //            ad.Fill(dtParameter);
            //            if (dtParameter.Rows.Count > 0)
            //            {

            //                ICEventParameter.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            //                ICEventParameter.CanBeTransformed = false;
            //                ICEventParameter.setLookupTable(dtParameter, "DisplayText", "Value");
            //                ICEventParameter.DisplayColumn = "DisplayText";
            //                ICEventParameter.ValueColumn = "Value";
            //                ICEventParameter.CanBeTransformed = false;

            //            }
            //            else
            //            {
            //                ICEventParameter.CanBeTransformed = true;
            //                ICEventParameter.TypeOfEntry = Import_Column.EntryType.Text;
            //            }

            //            UC_Parameter.initUserControl(ICEventParameter, this._Import);
            //            UC_Parameter.Dock = DockStyle.Top;
            //            UC_Parameter.Height = 24;
            //            UC_Parameter.setTitle(R["DisplayText"].ToString());
            //            this.panelCollectionEventMethodParameterValues.Controls.Add(UC_Parameter);

            //        }
            //    }
            //}
        }

        #endregion

        #endregion

        #region Specimen

        #region Common functions

        private void initSpecimen()
        {
            try
            {
            DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep("Specimen", "The specimen of the imported data", DiversityCollection.Import.getImportStepKey(Import.ImportStep.Specimen),
                "CollectionSpecimen",
                null, null, 0, this, 
                DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Specimen), this.panelImportSeletion);
            IS.IsGroupHaeder = true;

            DiversityCollection.Import_Step ISAcc = DiversityCollection.Import_Step.GetImportStep("Accession", "The accession of the specimen", DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession),
                "CollectionSpecimen",
                null, IS, 1, this, 
                DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Specimen), this.panelSpecimenSelection);

            DiversityCollection.Import_Step ISDep = DiversityCollection.Import_Step.GetImportStep("Depositor", "The depositor of the specimen", DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Depositor),
                "CollectionSpecimen",
                null, IS, 1, this, 
                DiversityCollection.Specimen.ImageForAgent(Specimen.OverviewImageUser.Depositor), this.panelSpecimenSelection);

            DiversityCollection.Import_Step ISN = DiversityCollection.Import_Step.GetImportStep("Notes", "The Notes of the specimen", DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Notes),
                "CollectionSpecimen",
                null, IS, 1, this, 
                this.StepImage(DiversityCollection.Import.ImportStepSpecimen.Notes), this.panelSpecimenSelection);

            DiversityCollection.Import_Step ISRef = DiversityCollection.Import_Step.GetImportStep("Reference", "The reference of the specimen", DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Reference),
                "CollectionSpecimen",
                null, IS, 1, this, 
                this.StepImage(DiversityCollection.Import.ImportStepSpecimen.Reference), this.panelSpecimenSelection);

            DiversityCollection.Import_Step ISLab = DiversityCollection.Import_Step.GetImportStep("Label", "The label of the specimen", DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Label),
                "CollectionSpecimen",
                null, IS, 1, this, 
                this.StepImage(DiversityCollection.Import.ImportStepSpecimen.Label), this.panelSpecimenSelection);

            //DiversityCollection.Import_Step ISRel = DiversityCollection.Import_Step.GetImportStep("Relations", "The relations of the specimen", DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Relations),
            //    "CollectionSpecimen",
            //    null, IS, 1, this, 
            //    this.StepImage(DiversityCollection.Import.ImportStepSpecimen.Relations), this.panelSpecimenSelection);
                this.initSpecimenAccession();
                this.initSpecimenDepositor();
                this.initSpecimenNotes();
                this.initSpecimenLabel();
                this.initSpecimenReference();
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Accession

        private void initSpecimenAccession()
        { 
            DiversityCollection.Import_Column C = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession)
                , "CollectionSpecimen"
                , "AccessionNumber"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_SpecimenAccessionNumber);
            C.TypeOfEntry = Import_Column.EntryType.Text;
            C.TypeOfFixing = Import_Column.FixingType.None;
            C.TypeOfSource = Import_Column.SourceType.File;
            C.MultiColumn = true;
            this.initAccessionDate();
            this.userControlImport_Column_SpecimenAccessionNumber.initUserControl(C, this.Import);

            DiversityCollection.Import_Column CRev = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession), "CollectionSpecimen", "LabelTranscriptionState", (DiversityCollection.iImportColumnControl)this.userControlImport_Column_TranscriptionState);
            CRev.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            CRev.TypeOfFixing = Import_Column.FixingType.Schema;
            CRev.TypeOfSource = Import_Column.SourceType.File;
            CRev.MustSelect = false;
            CRev.IsSelected = false;
            this.userControlImport_Column_TranscriptionState.initUserControl(CRev, this.Import);
            this.userControlImport_Column_TranscriptionState.setInterface();
        }

        private void initAccessionDate()
        {
            DiversityCollection.Import_Column ICD = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession)
                , "CollectionSpecimen", "AccessionDay", this.userControlImportDate_AccessionDate.userControlImport_ColumnDay);
            ICD.CanBeTransformed = true;
            DiversityCollection.Import_Column ICM = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession)
                , "CollectionSpecimen", "AccessionMonth", this.userControlImportDate_AccessionDate.userControlImport_ColumnMonth);
            ICM.CanBeTransformed = true;
            DiversityCollection.Import_Column ICY = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession)
                , "CollectionSpecimen", "AccessionYear", this.userControlImportDate_AccessionDate.userControlImport_ColumnYear);
            ICY.CanBeTransformed = true;
            DiversityCollection.Import_Column ICDate = DiversityCollection.Import_Column.GetImportColumn(DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession)
                , "CollectionSpecimen", "AccessionDateSupplement", this.userControlImportDate_AccessionDate.userControlImport_ColumnSupplement);
            ICDate.AlternativeColumn = "AccessionDateSupplement";
            ICDate.CanBeTransformed = true;
            this.userControlImportDate_AccessionDate.initUserControl(ICD, ICM, ICY, ICDate, 
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Accession), this.Import, this);
        }
        
        #endregion

        #region Specimen details
        
        private void initSpecimenDepositor()
        {
            try
            {
                DiversityCollection.Import_Column CDepositor = DiversityCollection.Import_Column.GetImportColumn(
                    DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Depositor)
                    , "CollectionSpecimen"
                    , "DepositorsName"
                    , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_Depositor);
                CDepositor.TypeOfEntry = Import_Column.EntryType.Text;
                CDepositor.TypeOfFixing = Import_Column.FixingType.Schema;
                CDepositor.TypeOfSource = Import_Column.SourceType.Any;
                this.userControlImport_Column_Specimen_Depositor.initUserControl(CDepositor, this.Import);

                DiversityCollection.Import_Column CDepositorsNr = DiversityCollection.Import_Column.GetImportColumn(
                    DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Depositor)
                    , "CollectionSpecimen"
                    , "DepositorsAccessionNumber"
                    , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_DepositorsAccessionNumber);
                CDepositorsNr.TypeOfEntry = Import_Column.EntryType.Text;
                CDepositorsNr.TypeOfFixing = Import_Column.FixingType.None;
                CDepositorsNr.TypeOfSource = Import_Column.SourceType.File;
                this.userControlImport_Column_Specimen_DepositorsAccessionNumber.initUserControl(CDepositorsNr, this.Import);

                DiversityCollection.Import_Column CExternalIdentifier = DiversityCollection.Import_Column.GetImportColumn(
                    DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Depositor)
                    , "CollectionSpecimen"
                    , "ExternalIdentifier"
                    , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_ExternalIdentifier);
                CDepositorsNr.TypeOfEntry = Import_Column.EntryType.Text;
                CDepositorsNr.TypeOfFixing = Import_Column.FixingType.None;
                CDepositorsNr.TypeOfSource = Import_Column.SourceType.File;
                this.userControlImport_Column_Specimen_ExternalIdentifier.initUserControl(CExternalIdentifier, this.Import);

                DiversityCollection.Import_Column CSource = DiversityCollection.Import_Column.GetImportColumn(
                    DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Depositor)
                    , "CollectionSpecimen"
                    , "ExternalDatasourceID"
                    , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_ExternalDataSource);
                CSource.TypeOfEntry = DiversityCollection.Import_Column.EntryType.MandatoryList;
                CSource.TypeOfFixing = DiversityCollection.Import_Column.FixingType.Schema;
                CSource.TypeOfSource = Import_Column.SourceType.Interface;
                CSource.CanBeTransformed = false;
                CSource.IsSelected = false;
                CSource.setDisplayTitle("Source");
                CSource.setLookupTable(DiversityCollection.LookupTable.DtExternalDatasource, "ExternalDatasourceName", "ExternalDataSourceID");
                CSource.StepKey = DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Depositor);
                this.userControlImport_Column_ExternalDataSource.initUserControl(CSource, this.Import);
                this.userControlImport_Column_ExternalDataSource.setInterface();
                //DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStep.Project)].setStepError();
            }
            catch (System.Exception ex) { }
        }
        
        #region Source

        //private void initSource()
        //{
        //    string StepKey = DiversityCollection.Import.getImportStepKey(Import.ImportStep.Source);
        //    DiversityCollection.Import_Step IF =
        //        DiversityCollection.Import_Step.GetImportStep(
        //        "Source",
        //        "The source of the imported data",
        //        StepKey,
        //        "CollectionSpecimen",
        //        null,
        //        null,
        //        0,
        //        (UserControls.iUserControlImportInterface)this,
        //        this.StepImage(DiversityCollection.Import.ImportStep.Specimen),
        //        null);//this.tabPageSource);
        //    IF.CanHide(false);
        //    //this.Import.ImportStepAdd(StepKey, IF);
        //    this.initProject(StepKey);
        //    this.initSource(StepKey);
        //}

        #region Source

        //private void initSource(string StepKey)
        //{
        //}

        //private System.Data.DataTable _DtExternalDataSource;
        //private System.Data.DataTable DtExternalDataSource
        //{
        //    get
        //    {
        //        if (this._DtExternalDataSource == null)
        //        {
        //            this._DtExternalDataSource = new System.Data.DataTable("Source");
        //            string SQL = "SELECT NULL AS ExternalDatasourceID, NULL AS ExternalDatasourceName " +
        //                "UNION SELECT ExternalDatasourceID, ExternalDatasourceName FROM CollectionExternalDatasource " +
        //                "ORDER BY ExternalDatasourceName";
        //            System.Data.SqlClient.SqlDataAdapter a = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
        //            try
        //            {
        //                a.Fill(this._DtExternalDataSource);
        //            }
        //            catch { }
        //        }
        //        return this._DtExternalDataSource;
        //    }
        //}

        private void buttonExternalDataSource_Click(object sender, EventArgs e)
        {
            DiversityCollection.FormExternalDatasource f = new FormExternalDatasource();
            f.ShowDialog();
            //this._DtExternalDataSource = null;
            //this.userControlImport_Column_ExternalDataSource.Import_Column.setLookupTable(this._DtExternalDataSource, "ExternalDatasourceName", "ExternalDatasourceID");
            this.userControlImport_Column_ExternalDataSource.requerySourceTable();
            //this.userControlImport_Column_ExternalDataSource.setSourceTable(this.DtExternalDataSource, "ExternalDataSourceID", "ExternalDataSource");
        }

        #endregion

        #endregion

        private void initSpecimenNotes()
        {
            DiversityCollection.Import_Column C = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Notes)
                , "CollectionSpecimen"
                , "OriginalNotes"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_OriginalNotes);
            C.TypeOfEntry = Import_Column.EntryType.Text;
            C.TypeOfFixing = Import_Column.FixingType.None;
            C.TypeOfSource = Import_Column.SourceType.File;
            this.userControlImport_Column_Specimen_OriginalNotes.initUserControl(C, this.Import);

            DiversityCollection.Import_Column CAN = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Notes)
                , "CollectionSpecimen"
                , "AdditionalNotes"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_AdditionalNotes);
            CAN.TypeOfEntry = Import_Column.EntryType.Text;
            CAN.TypeOfFixing = Import_Column.FixingType.None;
            CAN.TypeOfSource = Import_Column.SourceType.File;
            this.userControlImport_Column_Specimen_AdditionalNotes.initUserControl(CAN, this.Import);

            DiversityCollection.Import_Column CDWR = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Notes)
                , "CollectionSpecimen"
                , "DatawithholdingReason"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_DatawithholdingReason);
            CDWR.TypeOfEntry = Import_Column.EntryType.Text;
            CDWR.TypeOfFixing = Import_Column.FixingType.Schema;
            CDWR.TypeOfSource = Import_Column.SourceType.Any;
            this.userControlImport_Column_Specimen_DatawithholdingReason.initUserControl(CDWR, this.Import);

            DiversityCollection.Import_Column CProblems = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Notes)
                , "CollectionSpecimen"
                , "Problems"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_Problems);
            CProblems.TypeOfEntry = Import_Column.EntryType.Text;
            CProblems.TypeOfFixing = Import_Column.FixingType.Schema;
            CProblems.TypeOfSource = Import_Column.SourceType.Any;
            this.userControlImport_Column_Specimen_Problems.initUserControl(CProblems, this.Import);

            DiversityCollection.Import_Column CInternal = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Notes)
                , "CollectionSpecimen"
                , "InternalNotes"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_InternalNotes);
            CInternal.TypeOfEntry = Import_Column.EntryType.Text;
            CInternal.TypeOfFixing = Import_Column.FixingType.Schema;
            CInternal.TypeOfSource = Import_Column.SourceType.Any;
            this.userControlImport_Column_Specimen_InternalNotes.initUserControl(CInternal, this.Import);

        }

        private void initSpecimenReference()
        {
            DiversityCollection.Import_Column CReference = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Reference)
                , "CollectionSpecimen"
                , "ReferenceTitle"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_Reference);
            CReference.TypeOfEntry = Import_Column.EntryType.Text;
            CReference.TypeOfFixing = Import_Column.FixingType.Schema;
            CReference.TypeOfSource = Import_Column.SourceType.Any;
            CReference.MultiColumn = true;
            this.userControlImport_Column_Specimen_Reference.initUserControl(CReference, this.Import);

            DiversityCollection.Import_Column CReferenceDetails = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Reference)
                , "CollectionSpecimen"
                , "ReferenceDetails"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_ReferenceDetails);
            CReferenceDetails.TypeOfEntry = Import_Column.EntryType.Text;
            CReferenceDetails.TypeOfFixing = Import_Column.FixingType.None;
            CReferenceDetails.TypeOfSource = Import_Column.SourceType.File;
            this.userControlImport_Column_Specimen_ReferenceDetails.initUserControl(CReferenceDetails, this.Import);
        }

        private void initSpecimenLabel()
        {
            DiversityCollection.Import_Column CLabelTitle = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Label)
                , "CollectionSpecimen"
                , "LabelTitle"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_LabelTitle);
            CLabelTitle.TypeOfEntry = Import_Column.EntryType.Text;
            CLabelTitle.TypeOfFixing = Import_Column.FixingType.None;
            CLabelTitle.TypeOfSource = Import_Column.SourceType.File;
            this.userControlImport_Column_Specimen_LabelTitle.Dock = DockStyle.Fill;
            this.userControlImport_Column_Specimen_LabelTitle.initUserControl(CLabelTitle, this.Import);

            DiversityCollection.Import_Column CLabelType = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Label)
                , "CollectionSpecimen"
                , "LabelType"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_LabelType);
            CLabelType.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            CLabelType.TypeOfFixing = Import_Column.FixingType.Schema;
            CLabelType.TypeOfSource = Import_Column.SourceType.Interface;
            CLabelType.IsSelected = false;
            CLabelType.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollLabelType_Enum", true, true, true), "DisplayText", "Code");
            this.userControlImport_Column_Specimen_LabelType.Dock = DockStyle.Fill;
            this.userControlImport_Column_Specimen_LabelType.initUserControl(CLabelType, this.Import);

            DiversityCollection.Import_Column CLabelTranscriptionState = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Label)
                , "CollectionSpecimen"
                , "LabelTranscriptionState"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_LabelTranscriptionState);
            CLabelTranscriptionState.TypeOfEntry = Import_Column.EntryType.MandatoryList;
            CLabelTranscriptionState.TypeOfFixing = Import_Column.FixingType.Schema;
            CLabelTranscriptionState.TypeOfSource = Import_Column.SourceType.Any;
            CLabelTranscriptionState.setLookupTable(DiversityWorkbench.EnumTable.EnumTableForQuery("CollLabelTranscriptionState_Enum", true, true, true), "DisplayText", "Code");
            this.userControlImport_Column_Specimen_LabelTranscriptionState.Dock = DockStyle.Fill;
            this.userControlImport_Column_Specimen_LabelTranscriptionState.initUserControl(CLabelTranscriptionState, this.Import);

            DiversityCollection.Import_Column CLabelTranscriptionNotes = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.getImportStepKey(Import.ImportStepSpecimen.Label)
                , "CollectionSpecimen"
                , "LabelTranscriptionNotes"
                , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_Specimen_LabelTranscriptionNotes);
            CLabelTranscriptionNotes.TypeOfEntry = Import_Column.EntryType.Text;
            CLabelTranscriptionNotes.TypeOfFixing = Import_Column.FixingType.Schema;
            CLabelTranscriptionNotes.TypeOfSource = Import_Column.SourceType.Any;
            this.userControlImport_Column_Specimen_LabelTranscriptionNotes.Dock = DockStyle.Fill;
            this.userControlImport_Column_Specimen_LabelTranscriptionNotes.initUserControl(CLabelTranscriptionNotes, this.Import);
        }

        #endregion

        #endregion

        #region Relations
        
        private void initSpecimenRelations()
        {
            DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                "Relations",
                "The relations of the imported data",
                DiversityCollection.Import.getImportStepKey(Import.ImportStep.Relation),
                "CollectionSpecimenRelation",
                null, null,
                0,
                this,
                DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Relation),
                this.panelImportSeletion);
            IS.IsGroupHaeder = true;
            this.userControlImportSpecimenRelation.initUserControl((DiversityCollection.iImportInterface)this, IS);
        }
        
        #endregion

        #region Collector

        private void initCollector()
        {
            DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                "Collectors", 
                "The collectors of the imported data", 
                DiversityCollection.Import.getImportStepKey(Import.ImportStep.Collector),
                "CollectionAgent",
                null, null, 
                0, 
                this, 
                DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Agent), 
                this.panelImportSeletion);
            IS.IsGroupHaeder = true;
            this.userControlImportCollector.initUserControl((DiversityCollection.iImportInterface)this, IS);
        }

        #endregion

        #region Unit

        private void initUnit()
        {
            try
            {
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Organisms",
                    "The organisms of the imported data",
                    DiversityCollection.Import.getImportStepKey(Import.ImportStep.Organism),
                    "IdentificationUnit",
                    null, null,
                    0,
                    this,
                    DiversityCollection.Specimen.getImage(Specimen.OverviewImageTaxon.Plant),
                    this.panelImportSeletion);
                IS.IsGroupHaeder = true;
                this.userControlImportUnit.initUserControl((DiversityCollection.iImportInterface)this, IS);
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Storage

        private void initStorage()
        {
            try
            {
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep(
                    "Parts",
                    "The stored parts",
                    DiversityCollection.Import.getImportStepKey(Import.ImportStep.Storage),
                    "CollectionSpecimenPart",
                    null, null,
                    0,
                    this,
                    DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Collection),
                    this.panelImportSeletion);
                IS.IsGroupHaeder = true;
                this.userControlImportStorage.initUserControl((DiversityCollection.iImportInterface)this, IS);
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region UnitInPart
       
        private void initIdentificationUnitInPart()
        {
        }
        
        #endregion

        #region Images

        private void initImages()
        {
            try
            {
                string StepImages = DiversityCollection.Import.getImportStepKey(DiversityCollection.Import.ImportStep.Images);
                DiversityCollection.Import_Step IS = DiversityCollection.Import_Step.GetImportStep("Images", "The images of the specimen", DiversityCollection.Import.getImportStepKey(Import.ImportStep.Images),
                    "CollectionSpecimenImage",
                    null, null, 0, this,
                    DiversityCollection.Specimen.getImage(Specimen.OverviewImageStorage.Icones), this.panelImportSeletion);

                DiversityCollection.UserControls.UserControlImport_Column UCImage = new DiversityCollection.UserControls.UserControlImport_Column();

                DiversityCollection.Import_Column C = DiversityCollection.Import_Column.GetImportColumn(
                    DiversityCollection.Import.getImportStepKey(Import.ImportStep.Images)
                    , "CollectionSpecimenImage"
                    , "URI"
                    , (DiversityCollection.iImportColumnControl)this.userControlImport_Column_SpecimenAccessionNumber);
                C.TypeOfEntry = Import_Column.EntryType.Text;
                C.TypeOfFixing = Import_Column.FixingType.None;
                C.TypeOfSource = Import_Column.SourceType.File;
                C.MustSelect = true;
                UCImage.Dock = DockStyle.Top;
                this.tabPageImages.Controls.Add(UCImage);
                UCImage.initUserControl(C, this.Import);
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #region Summary

        private void initSummary()
        {
            try
            {
                DiversityCollection.Import_Step IS =
                    DiversityCollection.Import_Step.GetImportStep(
                    "Import",
                    "Start import",
                    DiversityCollection.Import.getImportStepKey(Import.ImportStep.Summary),
                    "",
                    null, null,
                    0,
                    (UserControls.iUserControlImportInterface)this,
                    this.StepImage(DiversityCollection.Import.ImportStep.Summary),
                    null);
                IS.CanHide(false);
                this.tabControlAnalysis.TabPages.Remove(this.tabPageAnalysisTables);

                DiversityCollection.Import.AddSetting(DiversityCollection.Import.Setting.TrimValues, this.checkBoxTrimValues.Checked.ToString());
            }
            catch (System.Exception ex) { }
        }
        
        private void buttonSaveSchema_Click(object sender, EventArgs e)
        {
            this.saveFileDialog = new SaveFileDialog();
            string Dir = System.Windows.Forms.Application.StartupPath + "\\Import\\ImportSchedules";
            if (!System.IO.Directory.Exists(Dir))
                System.IO.Directory.CreateDirectory(Dir);
            this.saveFileDialog.InitialDirectory = Dir;
            this.saveFileDialog.Filter = "xml files (*.xml)|*.xml";
            this.saveFileDialog.FileName = "ImportSchedule_" + System.Environment.UserName + ".xml";
            this.saveFileDialog.ShowDialog();
            if (this.saveFileDialog.FileName.Length > 0)
            {
                this.SaveSchema(this.saveFileDialog.FileName);
            }
        }

        #endregion

        #region Datagrid
        
        private void dataGridViewFile_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int Position = this.dataGridViewFile.SelectedCells[0].ColumnIndex;
                if (DiversityCollection.Import.CurrentImportColumn != null
                    && DiversityCollection.Import.CurrentImportColumn.ColumnInSourceFile == null)// DiversityCollection.Import.ColumnSelectionPending)
                {
                    if (this.Import.getImportColumns(Position).Count > 0)
                    {
                        string Message = "This column has allready been assigned to\r\n" +
                            this.ColumnHeaderForDataGrid(Position) +
                            "\r\nDo you want to change this?";
                        if (System.Windows.Forms.MessageBox.Show(Message, "Change setting?", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }
                    this.Import.setImportColumnInSourceFile(Position, DiversityCollection.Import.CurrentImportColumn);
                    this.dataGridViewFile.Columns[this.dataGridViewFile.SelectedCells[0].ColumnIndex].HeaderText =
                        this.ColumnHeaderForDataGrid(this.dataGridViewFile.SelectedCells[0].ColumnIndex);

                    if (DiversityCollection.Import.CurrentImportColumn != null &&
                        DiversityCollection.Import.AttachmentKeyImportColumn != null &&
                        DiversityCollection.Import.CurrentImportColumn.Key == DiversityCollection.Import.AttachmentKeyImportColumn.Key)
                        this.setDataGridColorRange();

                    this.Import.setColumnInterface(DiversityCollection.Import.CurrentImportColumn);
                }

                this.setColumnDisplays(Position);
                this.labelMapMessage.Visible = false;
                this.labelMapMessage.Text = "";
                this.labelMapMessage.BackColor = System.Drawing.SystemColors.Control;
                if (DiversityCollection.Import.CurrentImportColumn != null && DiversityCollection.Import.CurrentImportColumn.StepKey != null)
                {
                    DiversityCollection.Import.ImportSteps[DiversityCollection.Import.CurrentImportColumn.StepKey].setStepError(DiversityCollection.Import.CurrentImportColumn.getError());
                }
            }
            catch (System.Exception ex) { }
        }

        private string ColumnHeaderForDataGrid(int Position)
        {
            string Header = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> C in DiversityCollection.Import.ImportColumns)
            {
                if (C.Value.ColumnInSourceFile == Position && C.Value.IsSelected)
                {
                    string Test = C.Key;
                    if (Header.Length > 0) Header += " ";
                    if (C.Value.TableAlias != null && C.Value.TableAlias.Length > 0) Header += C.Value.TableAlias;
                    else Header += C.Value.Table;
                    Header += " " + C.Value.Column;
                }
            }
            return Header;
        }

        private void setMapMessage()
        {
            string Key = this.Import.CurrentPosition;
        }

        private bool DataInColumMatchDefinition(int Position, Import_Column IC)
        {
            return true;
        }

        public void setColumnDisplays(int Position)
        {
            this.panelColumnDisplays.Controls.Clear();
            System.Collections.Generic.List<DiversityCollection.Import_Column> ImportColumns = new List<Import_Column>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> KV in DiversityCollection.Import.ImportColumns)
            {
                if (!KV.Value.IsSelected)
                    continue;
                if (KV.Value.ColumnInSourceFile == Position)
                    ImportColumns.Add(KV.Value);
            }
            //if (!this._Import.ImportColumns.ContainsKey(Position))
            if (ImportColumns.Count == 0)
            {
                this.panelColumnDisplays.Visible = false;
                return;
            }
            else
            {
                foreach (DiversityCollection.Import_Column IC in ImportColumns)
                {
                    this.panelColumnDisplays.Visible = true;
                    DiversityCollection.UserControls.UserControlImport_ColumnDisplay U = new DiversityCollection.UserControls.UserControlImport_ColumnDisplay(IC, Position, this.Import);
                    this.panelColumnDisplays.Controls.Add(U);
                    U.Dock = DockStyle.Bottom;
                    int H = (this.panelColumnDisplays.Controls.Count * U.Height) + 6;
                    if (this.labelMapMessage.Text.Length > 0) H += this.labelMapMessage.Height;
                    this.tableLayoutPanelImportColumnDisplay.Height = H;
                }
            }
        }

        private void setDataSource(System.Windows.Forms.Control Control)
        {
            string Source = Control.AccessibleName;
        }

        public void setCurrentImportColumn()
        {
            //if (DiversityCollection.Import.CurrentImportColumn != null 
            //    && DiversityCollection.Import.CurrentImportColumn.MultiColumn)
            //    this.buttonAddNextColumn.Visible = true;
            //else
            //    this.buttonAddNextColumn.Visible = false;
            //DiversityCollection.Import.ColumnSelectionPending = false;
            DiversityCollection.Import.CurrentImportColumn = null;
            this.labelMapMessage.Text = "";
            this.labelMapMessage.Visible = false;
            this.panelColumnDisplays.Visible = false;
        }

        public void setCurrentImportColumn(DiversityCollection.Import_Column ImportColumn)
        {
            //this._ColumnSelectionPending = true;
            //this._Current_Import_Column = ImportColumn;
            //string Message = "Please select a column in the table with data for the import into the database ";
            //if (this._Current_Import_Column.TableAlias != null &&
            //    this._Current_Import_Column.TableAlias.Length > 0 &&
            //    this._Current_Import_Column.TableAlias != this._Current_Import_Column.Table)
            //    Message += "for the " + this._Current_Import_Column.TableAlias + " ";
            //Message += "in the table " + this._Current_Import_Column.Table + " " +
            //    "and the column " + this._Current_Import_Column.Column;
            if (ImportColumn.TypeOfSource != Import_Column.SourceType.Interface)
            {
                this.labelMapMessage.Text = DiversityCollection.Import.setCurrentImportColumn(ImportColumn);
                this.labelMapMessage.BackColor = FormImportWizard.ColorForColumQuery;
                this.labelMapMessage.Visible = true;
                //if (DiversityCollection.Import.CurrentImportColumn.MultiColumn)
                //    this.buttonAddNextColumn.Visible = true;
                //else this.buttonAddNextColumn.Visible = false;
                this.panelColumnDisplays.Visible = false;
            }
            else
                this.panelColumnDisplays.Visible = false;
        }

        //public void setCurrentImportColumn(string StepKey, string ImportColumn)
        //{
        //    DiversityCollection.Import_Column C = new Import_Column(StepKey);
        //    //DiversityCollection.Import.ColumnSelectionPending = true;
        //    //this._ColumnSelectionPending = true;
        //    try
        //    {
        //        string[] IC = ImportColumn.Split(new char[] { '.' });
        //        C.Table = IC[0];
        //        C.Column = IC[1];
        //        this.setCurrentImportColumn(C);
        //    }
        //    catch   { }
        //}

        //public void setCurrentImportColumn(string StepKey, System.Windows.Forms.Control C)
        //{
        //    DiversityCollection.Import_Column IC = new Import_Column(StepKey);
        //    //DiversityCollection.Import.ColumnSelectionPending = true;
        //    //this._ColumnSelectionPending = true;
        //    try
        //    {
        //        string[] CC = C.AccessibleName.Split(new char[] { '.' });
        //        IC.Table = CC[0];
        //        IC.Column = CC[1];
        //        this.setCurrentImportColumn(IC);
        //    }
        //    catch { }
        //}

        private void buttonAddNextColumn_Click(object sender, EventArgs e)
        {
            //DiversityCollection.Import.ColumnSelectionPending = true;
            //this._ColumnSelectionPending = true;
            DiversityCollection.Import_Column C = DiversityCollection.Import_Column.GetImportColumn(
                DiversityCollection.Import.CurrentImportColumn.StepKey,
                DiversityCollection.Import.CurrentImportColumn.Table,
                DiversityCollection.Import.CurrentImportColumn.TableAlias,
                DiversityCollection.Import.CurrentImportColumn.Column,
                DiversityCollection.Import.CurrentImportColumn.Sequence() + 1,
                DiversityCollection.Import.CurrentImportColumn.ImportColumnControl,
                DiversityCollection.Import_Column.SourceType.Any , 
                DiversityCollection.Import_Column.FixingType.None, 
                DiversityCollection.Import_Column.EntryType.Database);
            DiversityCollection.Import.setCurrentImportColumn(C);

            //DiversityCollection.Import.setCurrentImportColumn(DiversityCollection.Import.CurrentImportColumn);
            //this.setCurrentImportColumn(this._Current_Import_Column);
        }

        #endregion

        #region Schema
        
        private void SaveSchema(string FileName)
        {
            if (DiversityCollection.Import.AttachmentKeyImportColumn != null && DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile != null)
                DiversityCollection.Import.AddSetting(DiversityCollection.Import.Setting.AttachmentColumnInSourceFile, DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile.ToString());
            this._Import.SaveSchemaFile(FileName, null);
        }

        private void LoadSchema(string FileName)
        {
            this._Import.LoadSchemaFile(FileName);
            this.setProjectSelection();
            return;
        }

        private void ShowConvertedFile(string FileName)
        {
            System.IO.FileInfo xmlFile = new System.IO.FileInfo(FileName);
            System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
            // load default style sheet from resources
            System.IO.StringReader xsltReader = new System.IO.StringReader(DiversityCollection.Properties.Resources.TransformationSchema);
            System.Xml.XmlReader xml = System.Xml.XmlReader.Create(xsltReader);
            xslt.Load(xml);

            System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(xmlFile.FullName);

            // The output file:
            string outputFile = xmlFile.FullName.Substring(0, xmlFile.FullName.Length
                - xmlFile.Extension.Length) + ".htm";

            // Create the writer.             
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outputFile, xslt.OutputSettings);

            // Transform the file
            xslt.Transform(doc, writer);
            writer.Close();
            DiversityWorkbench.FormWebBrowser f = new DiversityWorkbench.FormWebBrowser(outputFile);
            try
            {
                f.ShowInTaskbar = true;
                f.ShowDialog();
            }
            catch (System.Exception ex) { }


            //if (this._TransformationSchema == null)
            //{
            //    System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\Import\\ImportSchedules\\Transformation");
            //    if (!Dir.Exists)
            //        Dir.Create();
            //    this.openFileDialog = new OpenFileDialog();
            //    this.openFileDialog.RestoreDirectory = true;
            //    this.openFileDialog.Multiselect = false;
            //    this.openFileDialog.InitialDirectory = Dir.FullName;
            //    this.openFileDialog.Filter = "XSLT Files|*.xslt";
            //    try
            //    {
            //        this.openFileDialog.ShowDialog();
            //        if (this.openFileDialog.FileName.Length > 0)
            //        {
            //            this._TransformationSchema = new System.IO.FileInfo(this.openFileDialog.FileName);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //    }
            //}
            //string SchedulePath = this.ConvertXmlToHtml(FileName, this._TransformationSchema.FullName);
            //if (SchedulePath.Length > 0)
            //{
            //    DiversityWorkbench.FormWebBrowser f = new DiversityWorkbench.FormWebBrowser(SchedulePath);
            //    try
            //    {
            //        f.ShowDialog();
            //    }
            //    catch (System.Exception ex) { }
            //}
            //else System.Windows.Forms.MessageBox.Show("Conversion failed");
        }

        #endregion

        #region Analysis

        private void buttonAnalyseData_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.AnalyseFile();
            DiversityCollection.Import.ImportTablesDataTreatment = new Dictionary<string, Import_Table>();
            this.AnalyseTables();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void AnalyseFile()
        {
            if (!this._Import.ImportPrepared())
                return;
            if (!this._Import.AllStepsOK())
                return;
            this.imageListSteps.Images.Clear();
            this.StepImageIndex.Clear();
            string ErrorMessage = "";
            DiversityCollection.Import.DecisionStepsWithNoValues.Clear();
            this.treeViewAnalyseData.Nodes.Clear();
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
                {
                    // nur zur Kontrolle
                    string CurrentTable = KV.Value.TableAlias();

                    if (KV.Value.IsGroupHaeder || !KV.Value.IsVisible())
                        continue;
                    if (!DiversityCollection.Import.ImportTables.ContainsKey(KV.Value.TableAlias()))
                        continue;

                    this.AddStepImage(KV.Value);
                    System.Windows.Forms.TreeNode N = new TreeNode(KV.Value.StepTitle());
                    N.ImageIndex = this.StepImageIndex[KV.Key];
                    N.SelectedImageIndex = this.StepImageIndex[KV.Key];
                    DiversityCollection.Import.ImportTables[KV.Value.TableAlias()].FillTableWithDataFromFile((int)this.numericUpDownAnalyseDataLine.Value - 1);

                    bool HasValues = false;
                    bool SuperiorStepIsEmpty = false;
                    foreach (string s in DiversityCollection.Import.DecisionStepsWithNoValues)
                    {
                        if (KV.Key.StartsWith(s))
                            SuperiorStepIsEmpty = true;
                    }
                    if (SuperiorStepIsEmpty)
                        continue;

                    if (DiversityCollection.Import.ImportTables[KV.Value.TableAlias()].DecisionColumns.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> IC in DiversityCollection.Import.ImportTables[KV.Value.TableAlias()].DecisionColumns)
                        {
                            if (DiversityCollection.Import.ImportTables[KV.Value.TableAlias()].ColumnValueDictionary().ContainsKey(IC.Value.Column) &&
                                DiversityCollection.Import.ImportTables[KV.Value.TableAlias()].ColumnValueDictionary()[IC.Value.Column].ToString().Length > 0)
                            {
                                HasValues = true;
                                break;
                            }
                        }
                        if (!HasValues)
                        {
                            DiversityCollection.Import.DecisionStepsWithNoValues.Add(KV.Key);
                            continue;
                        }
                    }
                    foreach (System.Collections.Generic.KeyValuePair<string, string> CC in DiversityCollection.Import.ImportTables[KV.Value.TableAlias()].ColumnValueDictionary())
                    {
                        if (CC.Value.Length == 0)
                            continue;
                        string ColumnKey = KV.Value.TableAlias() + "." + CC.Key + ".1";
                        if (!DiversityCollection.Import.ImportColumns.ContainsKey(ColumnKey))
                            continue;
                        else if (DiversityCollection.Import.ImportColumns[ColumnKey].StepKey != KV.Key)
                            continue;
                        string DisplayText = CC.Key + ": " + CC.Value;
                        string Entity = KV.Value.TableName() + "." + CC.Key;
                        string Description = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation(Entity));
                        if (Description.Length > 0)
                            DisplayText = Description + ": " + CC.Value;
                        System.Windows.Forms.TreeNode NC = new TreeNode(DisplayText);
                        if (DiversityCollection.Import.AttachmentKeyImportColumn != null
                            && CC.Key == DiversityCollection.Import.AttachmentKeyImportColumn.Column
                            && KV.Value.TableAlias() == DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias)
                            NC.BackColor = FormImportWizard.ColorForAttachment;
                        else if (DiversityCollection.Import.ImportColumns.ContainsKey(ColumnKey)
                            && DiversityCollection.Import.ImportColumns[ColumnKey].ValueIsFixed)
                        {
                            if (DiversityCollection.Import.ImportColumns[ColumnKey].ColumnInSourceFile != null)
                                NC.BackColor = FormImportWizard.ColorForFixing;
                            else if (DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfEntry != Import_Column.EntryType.Database)
                                NC.BackColor = FormImportWizard.ColorForFixing;
                            else if (DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfEntry == Import_Column.EntryType.Database)
                                NC.BackColor = FormImportWizard.ColorForDatabaseAsSource;
                            else if (DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfSource == Import_Column.SourceType.Database)
                                NC.BackColor = FormImportWizard.ColorForDatabaseAsSource;
                            else
                            {
                                NC.BackColor = System.Drawing.Color.LightGray;
                            }
                        }
                        else if (DiversityCollection.Import.ImportColumns.ContainsKey(ColumnKey)
                            && DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfSource == Import_Column.SourceType.Database
                            && DiversityCollection.Import.ImportColumns[ColumnKey].ColumnInSourceFile == null)
                            NC.BackColor = FormImportWizard.ColorForDatabaseAsSource;
                        if (DiversityCollection.Import.ImportTables[KV.Value.TableAlias()].ColumnValueErros.ContainsKey(CC.Key))
                        {
                            N.ForeColor = System.Drawing.Color.Red;
                            NC.ForeColor = System.Drawing.Color.Red;
                            if (ErrorMessage.Length == 0)
                                ErrorMessage = "Errors detected:\r\n";
                            ErrorMessage += "Column " + CC.Key + ": " + DiversityCollection.Import.ImportTables[KV.Value.TableAlias()].ColumnValueErros[CC.Key];
                        }
                        N.Nodes.Add(NC);
                        //if (DiversityCollection.Import.ImportColumns[ColumnKey].TypeOfEntry != Import_Column.EntryType.Database)
                            HasValues = true;
                    }
                    if (HasValues)
                        this.treeViewAnalyseData.Nodes.Add(N);
                }
            }
            this.treeViewAnalyseData.ExpandAll();
            if (ErrorMessage.Length > 0)
                System.Windows.Forms.MessageBox.Show(ErrorMessage);
        }

        private void AnalyseTables()
        {
            try
            {
                this.tabPageAnalysisTables.Controls.Clear();
                if (DiversityCollection.Import.ImportTablesDataTreatment != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KVTreatment in DiversityCollection.Import.ImportTablesDataTreatment)
                    {
                        if (!DiversityCollection.Import.ImportTables.ContainsKey(KVTreatment.Key))
                            DiversityCollection.Import.ImportTables.Add(KVTreatment.Key, KVTreatment.Value);
                    }
                }
                if (DiversityCollection.Import.AttachmentKeyImportColumn != null)
                {
                    int i = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Table> KV in DiversityCollection.Import.ImportTables)
                    {
                        DiversityCollection.UserControls.UserControlImportTable UT = new UserControls.UserControlImportTable(KV.Value);
                        UT.Dock = DockStyle.Top;
                        UT.SendToBack();
                        if (i % 2 == 1 && UT.BackColor != FormImportWizard.ColorForAttachment)
                            UT.BackColor = System.Drawing.SystemColors.Control;
                        this.tabPageAnalysisTables.Controls.Add(UT);
                        if (!DiversityCollection.Import.ImportTablesDataTreatment.ContainsKey(KV.Key))
                        {
                            DiversityCollection.Import.ImportTablesDataTreatment.Add(KV.Key, KV.Value);
                        }
                        i++;
                    }
                    if (!this.tabControlAnalysis.TabPages.Contains(this.tabPageAnalysisTables))
                        this.tabControlAnalysis.TabPages.Add(this.tabPageAnalysisTables);
                }
                else
                {
                    if (this.tabControlAnalysis.TabPages.Contains(this.tabPageAnalysisTables))
                        this.tabControlAnalysis.TabPages.Remove(this.tabPageAnalysisTables);
                }
            }
            catch (System.Exception ex) { }
        }

        private System.Collections.Generic.Dictionary<string, int> _StepImages;
        private System.Collections.Generic.Dictionary<string, int> StepImages
        {
            get
            {
                if (this._StepImages == null)
                {
                    this._StepImages = new Dictionary<string, int>();

                }
                return this._StepImages;
            }
        }
        private void AddStepImage(Import_Step Step)
        {
            System.Drawing.Image Image = DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Null);
            if (Step.Image == null)
            {
                if (Step.SuperiorImportStep != null && Step.SuperiorImportStep.Image != null)
                    Image = Step.SuperiorImportStep.Image;
                // correction for buggy asignement of superior steps 
                ///TODO: Ursache finden
                if (Step.SuperiorImportStep != null && Step.Level == Step.SuperiorImportStep.Level)
                {
                    if (Step.SuperiorImportStep.SuperiorImportStep != null && Step.SuperiorImportStep.SuperiorImportStep.Image != null)
                        Image = Step.SuperiorImportStep.SuperiorImportStep.Image;
                }
            }
            else
                Image = Step.Image;
            if (this.imageListSteps.Images.Count == 0 || !this.imageListSteps.Images.ContainsKey("0"))
                this.imageListSteps.Images.Add("0", DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Null));
            if (!this.imageListSteps.Images.ContainsKey(Step.StepKey()))
            {
                this.imageListSteps.Images.Add(Step.StepKey(), Image);
                if (!this.StepImageIndex.ContainsKey(Step.StepKey()))
                    this.StepImageIndex.Add(Step.StepKey(), this.imageListSteps.Images.Count - 1);
            }
        }
        private System.Collections.Generic.Dictionary<string, int> _StepImageIndex;
        private System.Collections.Generic.Dictionary<string, int> StepImageIndex
        {
            get
            {
                if (this._StepImageIndex == null)
                {
                    this._StepImageIndex = new Dictionary<string, int>();
                }
                if (this._StepImageIndex.Count == 0)
                    this._StepImageIndex.Add("0", 0);
                return this._StepImageIndex;
            }
        }


        private System.Collections.Generic.List<DiversityCollection.Import_Column> TableColumns(string TableAlias)
        {
            System.Collections.Generic.List<DiversityCollection.Import_Column> L = new List<Import_Column>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Column> IC in DiversityCollection.Import.ImportColumns)
            {
                if (IC.Value.IsSelected && IC.Value.TableAlias == TableAlias && !L.Contains(IC.Value))
                {
                    L.Add(IC.Value);
                }
            }
            return L;
        }

        

        
        private void checkBoxTrimValues_CheckedChanged(object sender, EventArgs e)
        {
            DiversityCollection.Import.AddSetting(Import.Setting.TrimValues, this.checkBoxTrimValues.Checked.ToString());
        }

        #endregion    
    
        #region Import

        private void buttonStartImport_Click(object sender, EventArgs e)
        {
            if (!this._Import.AllStepsOK())
                return;

            // reset special lists
            DiversityCollection.Import.SeriesIDList = null;
            DiversityCollection.Import.EventIDList = null;

            System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxImportFile.Text);
            string File = FI.Name.Substring(0, FI.Name.Length - FI.Extension.Length);
            string DateString = System.DateTime.Now.Year.ToString();
            if (System.DateTime.Now.Month < 10) DateString += "0";
            DateString += System.DateTime.Now.Month.ToString();
            if (System.DateTime.Now.Day < 10) DateString += "0";
            DateString += System.DateTime.Now.Day.ToString() + "_";
            if (System.DateTime.Now.Hour < 10) DateString += "0";
            DateString += System.DateTime.Now.Hour.ToString();
            if (System.DateTime.Now.Minute < 10) DateString += "0";
            DateString += System.DateTime.Now.Minute.ToString();
            if (System.DateTime.Now.Second < 10) DateString += "0";
            DateString += System.DateTime.Now.Second.ToString();
            string FileProtocol = File + "_Protocol_" + DateString + ".xml";
            try
            {
                this.saveFileDialog = new SaveFileDialog();
                string DirProtcol = System.Windows.Forms.Application.StartupPath + "\\Import\\ImportProtocol";
                if (!System.IO.Directory.Exists(DirProtcol))
                    System.IO.Directory.CreateDirectory(DirProtcol);
                this.saveFileDialog.InitialDirectory = DirProtcol;
                FileProtocol = DirProtcol + "\\" + FileProtocol;
                this.saveFileDialog.Filter = "xml files (*.xml)|*.xml";
                string Message = this._Import.ImportData();
                this.progressBar.Visible = true;
                if (Message.Length > 0)
                {
                    this._Import.SaveSchemaFile(FileProtocol, Message);
                    this.buttonShowProtocoll.Tag = FileProtocol;
                    this.buttonShowProtocoll.Visible = true;
                    this.ShowConvertedFile(FileProtocol);
                }
                else
                    System.Windows.Forms.MessageBox.Show("Nothing imported");
            }
            catch (Exception ex)
            {
            }
            this.progressBar.Visible = false;
        }
        
        private System.Collections.Generic.List<string> _TableList;
        private System.Collections.Generic.List<string> TableList
        {
            get
            {
                if (this._TableList == null)
                {
                    _TableList = new List<string>();

                    _TableList.Add("CollectionEventSeries");
                    _TableList.Add("CollectionEventSeriesImage");

                    _TableList.Add("CollectionEvent");
                    _TableList.Add("CollectionEventImage");
                    _TableList.Add("CollectionEventLocalisation");
                    _TableList.Add("CollectionEventProperty");

                    _TableList.Add("CollectionSpecimen");
                    _TableList.Add("CollectionProject");
                    _TableList.Add("CollectionAgent");
                    _TableList.Add("CollectionSpecimenRelation");

                    _TableList.Add("CollectionSpecimenPart");
                    _TableList.Add("CollectionSpecimenProcessing");

                    _TableList.Add("IdentificationUnit");
                    _TableList.Add("Identification");
                    _TableList.Add("IdentificationUnitAnalysis");
                    _TableList.Add("IdentificationUnitGeoAnalysis");
                    _TableList.Add("IdentificationUnitInPart");
                    _TableList.Add("CollectionSpecimenImage");

                    _TableList.Add("CollectionSpecimenTransaction");

                    _TableList.Add("Annotation");
                }
                return _TableList;
            }
        }

        private void buttonShowProtocoll_Click(object sender, EventArgs e)
        {
            if (this.buttonShowProtocoll.Tag != null)
                this.ShowConvertedFile(this.buttonShowProtocoll.Tag.ToString());
            else
            {
                string Dir = System.Windows.Forms.Application.StartupPath + "\\Import\\ImportProtocol";
                if (!System.IO.Directory.Exists(Dir))
                    System.IO.Directory.CreateDirectory(Dir);
                this.openFileDialog = new OpenFileDialog();
                this.openFileDialog.RestoreDirectory = true;
                this.openFileDialog.Multiselect = false;
                this.openFileDialog.Filter = "XML Files|*.xml";
                this.openFileDialog.InitialDirectory = Dir;
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    this.ShowConvertedFile(this.openFileDialog.FileName);
                }
            }
        }

        private void buttonOpenErrorFile_Click(object sender, EventArgs e)
        {
            if (this.buttonOpenErrorFile.Tag != null)
            {
                System.Diagnostics.Process.Start(this.buttonOpenErrorFile.Tag.ToString());
            }
            else
            {
                string Dir = System.Windows.Forms.Application.StartupPath + "\\Import";
                if (!System.IO.Directory.Exists(Dir))
                    System.IO.Directory.CreateDirectory(Dir);
                this.openFileDialog = new OpenFileDialog();
                this.openFileDialog.RestoreDirectory = true;
                this.openFileDialog.Multiselect = false;
                this.openFileDialog.Filter = "Text Files|*.txt";
                this.openFileDialog.InitialDirectory = Dir;
                this.openFileDialog.ShowDialog();
            }
        }

        public void setProgressBarValue(int ValueOfProgressBar)
        {
            if (this.progressBar.Visible == false)
                this.progressBar.Visible = true;
            if (ValueOfProgressBar <= this.progressBar.Maximum)
                this.progressBar.Value = ValueOfProgressBar;
        }

        /// <summary>
        /// Display the imported series as there can not been seen as long as they are not connected to a specimen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonImportedSeries_Click(object sender, EventArgs e)
        {
            if (DiversityCollection.Import.SeriesIDList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No collection event series to show");
                return;
            }
        }

        //private System.Collections.Generic.List<int> _SeriesIDList;

        //public System.Collections.Generic.List<int> SeriesIDList
        //{
        //    get 
        //    {
        //        if (this._SeriesIDList == null)
        //            this._SeriesIDList = new List<int>();
        //        return _SeriesIDList; 
        //    }
        //    //set { _SeriesIDList = value; }
        //}

        /// <summary>
        /// Display the imported events as there can not been seen as long as they are not connected to a specimen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonViewImportedEvents_Click(object sender, EventArgs e)
        {
            if (DiversityCollection.Import.EventIDList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("No collection events to show");
                return;
            }
        }

        //private System.Collections.Generic.List<int> _EventIDList;

        //public System.Collections.Generic.List<int> EventIDList
        //{
        //    get 
        //    {
        //        if (this._EventIDList == null)
        //            this._EventIDList = new List<int>();
        //        return _EventIDList; 
        //    }
        //    //set { _EventIDList = value; }
        //}

        #endregion

        #region Feedback
        
        private void buttonHeaderFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }
        
        #endregion

        #region StepIcons
        
        private System.Drawing.Image StepImage(DiversityCollection.Import.ImportStep ImportStep)
        {
            System.Drawing.Image I = this.imageListTabControl.Images[0];
            switch (ImportStep)
            {
                case DiversityCollection.Import.ImportStep.Collector:
                    I = DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Agent);
                    break;
                case DiversityCollection.Import.ImportStep.Event:
                    I = DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Event);
                    break;
                case DiversityCollection.Import.ImportStep.Series:
                    I = DiversityCollection.Specimen.getImage(Specimen.OverviewImage.EventSeries);
                    break;
                case DiversityCollection.Import.ImportStep.Project:
                    I = DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Project);
                    break;
                case DiversityCollection.Import.ImportStep.File:
                    I = this.imageListTabControl.Images[0];
                    break;
                case DiversityCollection.Import.ImportStep.Summary:
                    I = this.imageListTabControl.Images[11];
                    break;
            }
            return I;
        }

        private System.Drawing.Image StepImage(DiversityCollection.Import.ImportStepEvent ImportStep)
        {
            System.Drawing.Image I = this.imageListTabControl.Images[0];
            switch (ImportStep)
            {
                case DiversityCollection.Import.ImportStepEvent.Altitude:
                    I = this.imageListTabEvent.Images[3];
                    break;
                case DiversityCollection.Import.ImportStepEvent.Coordinates:
                    I = DiversityCollection.Specimen.getImage(Specimen.OverviewImage.Event);
                    break;
                case DiversityCollection.Import.ImportStepEvent.Date:
                    I = this.imageListTabEvent.Images[1];
                    break;
            }
            return I;
        }

        private System.Drawing.Image StepImage(DiversityCollection.Import.ImportStepSpecimen ImportStep)
        {
            System.Drawing.Image I = this.imageListTabSpecimen.Images[0];
            switch (ImportStep)
            {
                case DiversityCollection.Import.ImportStepSpecimen.Notes:
                    I = this.imageListTabSpecimen.Images[3];
                    break;
                case DiversityCollection.Import.ImportStepSpecimen.Reference:
                    I = this.imageListTabSpecimen.Images[2];
                    break;
                case DiversityCollection.Import.ImportStepSpecimen.Label:
                    I = this.imageListTabSpecimen.Images[4];
                    break;
                case DiversityCollection.Import.ImportStepSpecimen.Relations:
                    I = this.imageListTabSpecimen.Images[5];
                    break;
            }
            return I;
        }
        
        #endregion

    }
}
