namespace DiversityCollection.Forms
{
    partial class FormExportGPI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormExportGPI));
            helpProvider = new System.Windows.Forms.HelpProvider();
            textBoxErrors = new System.Windows.Forms.TextBox();
            textBoxExportFile = new System.Windows.Forms.TextBox();
            buttonCreateExport = new System.Windows.Forms.Button();
            webBrowser = new System.Windows.Forms.WebBrowser();
            toolTip = new System.Windows.Forms.ToolTip(components);
            textBoxInstitutionCode = new System.Windows.Forms.TextBox();
            textBoxInstitutionName = new System.Windows.Forms.TextBox();
            textBoxTypeNoteRestrictions = new System.Windows.Forms.TextBox();
            checkBoxExportTypeNotes = new System.Windows.Forms.CheckBox();
            checkBoxRestrictToLinkedNames = new System.Windows.Forms.CheckBox();
            buttonSaveAs = new System.Windows.Forms.Button();
            textBoxMissingCollector = new System.Windows.Forms.TextBox();
            tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            labelXmlFile = new System.Windows.Forms.Label();
            labelInstitution = new System.Windows.Forms.Label();
            comboBoxInstitution = new System.Windows.Forms.ComboBox();
            labelInstitutionCode = new System.Windows.Forms.Label();
            labelInstitutionName = new System.Windows.Forms.Label();
            checkBoxTypeNoteRestrictions = new System.Windows.Forms.CheckBox();
            progressBar = new System.Windows.Forms.ProgressBar();
            labelNames = new System.Windows.Forms.Label();
            labelCity = new System.Windows.Forms.Label();
            textBoxCity = new System.Windows.Forms.TextBox();
            radioButtonExportAll = new System.Windows.Forms.RadioButton();
            radioButtonRestrictToTypeAndLast = new System.Windows.Forms.RadioButton();
            labelTypeNotes = new System.Windows.Forms.Label();
            checkBoxLastNameAsStorage = new System.Windows.Forms.CheckBox();
            textBoxMessages = new System.Windows.Forms.TextBox();
            labelMessages = new System.Windows.Forms.Label();
            labelMissingCollector = new System.Windows.Forms.Label();
            checkBoxIncludeAdditionalNotes = new System.Windows.Forms.CheckBox();
            checkBoxMissingFamily = new System.Windows.Forms.CheckBox();
            textBoxMissingFamily = new System.Windows.Forms.TextBox();
            labelGazetteer = new System.Windows.Forms.Label();
            comboBoxGazetteer = new System.Windows.Forms.ComboBox();
            dataSetCollectionSpecimen = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionSpecimen).BeginInit();
            SuspendLayout();
            // 
            // textBoxErrors
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxErrors, 8);
            textBoxErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxErrors.Location = new System.Drawing.Point(110, 144);
            textBoxErrors.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxErrors.Multiline = true;
            textBoxErrors.Name = "textBoxErrors";
            tableLayoutPanelMain.SetRowSpan(textBoxErrors, 2);
            textBoxErrors.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            helpProvider.SetShowHelp(textBoxErrors, true);
            textBoxErrors.Size = new System.Drawing.Size(963, 103);
            textBoxErrors.TabIndex = 17;
            textBoxErrors.Visible = false;
            // 
            // textBoxExportFile
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxExportFile, 9);
            textBoxExportFile.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxExportFile.Location = new System.Drawing.Point(87, 609);
            textBoxExportFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxExportFile.Name = "textBoxExportFile";
            textBoxExportFile.ReadOnly = true;
            textBoxExportFile.Size = new System.Drawing.Size(986, 23);
            textBoxExportFile.TabIndex = 0;
            // 
            // buttonCreateExport
            // 
            tableLayoutPanelMain.SetColumnSpan(buttonCreateExport, 2);
            buttonCreateExport.Dock = System.Windows.Forms.DockStyle.Left;
            buttonCreateExport.Location = new System.Drawing.Point(4, 144);
            buttonCreateExport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonCreateExport.Name = "buttonCreateExport";
            buttonCreateExport.Size = new System.Drawing.Size(88, 27);
            buttonCreateExport.TabIndex = 1;
            buttonCreateExport.Text = "Start export";
            buttonCreateExport.UseVisualStyleBackColor = true;
            buttonCreateExport.Click += buttonCreateExport_Click;
            // 
            // webBrowser
            // 
            tableLayoutPanelMain.SetColumnSpan(webBrowser, 10);
            webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            webBrowser.Location = new System.Drawing.Point(4, 316);
            webBrowser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            webBrowser.MinimumSize = new System.Drawing.Size(23, 23);
            webBrowser.Name = "webBrowser";
            webBrowser.Size = new System.Drawing.Size(1069, 264);
            webBrowser.TabIndex = 2;
            // 
            // textBoxInstitutionCode
            // 
            textBoxInstitutionCode.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxInstitutionCode.Location = new System.Drawing.Point(452, 3);
            textBoxInstitutionCode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxInstitutionCode.Name = "textBoxInstitutionCode";
            textBoxInstitutionCode.ReadOnly = true;
            textBoxInstitutionCode.Size = new System.Drawing.Size(55, 23);
            textBoxInstitutionCode.TabIndex = 7;
            toolTip.SetToolTip(textBoxInstitutionCode, "The code of the institution");
            // 
            // textBoxInstitutionName
            // 
            textBoxInstitutionName.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxInstitutionName.Location = new System.Drawing.Point(675, 3);
            textBoxInstitutionName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxInstitutionName.Name = "textBoxInstitutionName";
            textBoxInstitutionName.ReadOnly = true;
            textBoxInstitutionName.Size = new System.Drawing.Size(212, 23);
            textBoxInstitutionName.TabIndex = 9;
            toolTip.SetToolTip(textBoxInstitutionName, "The name of the institution");
            // 
            // textBoxTypeNoteRestrictions
            // 
            textBoxTypeNoteRestrictions.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTypeNoteRestrictions.Location = new System.Drawing.Point(675, 57);
            textBoxTypeNoteRestrictions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxTypeNoteRestrictions.Name = "textBoxTypeNoteRestrictions";
            textBoxTypeNoteRestrictions.Size = new System.Drawing.Size(212, 23);
            textBoxTypeNoteRestrictions.TabIndex = 11;
            textBoxTypeNoteRestrictions.Text = "?";
            toolTip.SetToolTip(textBoxTypeNoteRestrictions, "Enter the list of notes that shoud be exported together with the type (use | as a separator)");
            // 
            // checkBoxExportTypeNotes
            // 
            checkBoxExportTypeNotes.AutoSize = true;
            checkBoxExportTypeNotes.Checked = true;
            checkBoxExportTypeNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            tableLayoutPanelMain.SetColumnSpan(checkBoxExportTypeNotes, 3);
            checkBoxExportTypeNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxExportTypeNotes.Location = new System.Drawing.Point(110, 57);
            checkBoxExportTypeNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxExportTypeNotes.Name = "checkBoxExportTypeNotes";
            checkBoxExportTypeNotes.Size = new System.Drawing.Size(334, 23);
            checkBoxExportTypeNotes.TabIndex = 12;
            checkBoxExportTypeNotes.Text = "Export type notes (attach to type status)";
            toolTip.SetToolTip(checkBoxExportTypeNotes, "Export the entries in the field type notes together with the type");
            checkBoxExportTypeNotes.UseVisualStyleBackColor = true;
            checkBoxExportTypeNotes.CheckedChanged += checkBoxExportTypeNotes_CheckedChanged;
            // 
            // checkBoxRestrictToLinkedNames
            // 
            checkBoxRestrictToLinkedNames.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(checkBoxRestrictToLinkedNames, 2);
            checkBoxRestrictToLinkedNames.Location = new System.Drawing.Point(515, 32);
            checkBoxRestrictToLinkedNames.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxRestrictToLinkedNames.Name = "checkBoxRestrictToLinkedNames";
            checkBoxRestrictToLinkedNames.Size = new System.Drawing.Size(229, 19);
            checkBoxRestrictToLinkedNames.TabIndex = 13;
            checkBoxRestrictToLinkedNames.Text = "Restrict to names linked to a thesaurus";
            toolTip.SetToolTip(checkBoxRestrictToLinkedNames, "Restrict to names that are linke to a taxonomic thesaurus");
            checkBoxRestrictToLinkedNames.UseVisualStyleBackColor = true;
            // 
            // buttonSaveAs
            // 
            tableLayoutPanelMain.SetColumnSpan(buttonSaveAs, 2);
            buttonSaveAs.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonSaveAs.Location = new System.Drawing.Point(4, 220);
            buttonSaveAs.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            buttonSaveAs.Name = "buttonSaveAs";
            buttonSaveAs.Size = new System.Drawing.Size(98, 27);
            buttonSaveAs.TabIndex = 25;
            buttonSaveAs.Text = "Save errors";
            toolTip.SetToolTip(buttonSaveAs, "Save error report as text file");
            buttonSaveAs.UseVisualStyleBackColor = true;
            buttonSaveAs.Click += buttonSaveAs_Click;
            // 
            // textBoxMissingCollector
            // 
            textBoxMissingCollector.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxMissingCollector.Location = new System.Drawing.Point(675, 86);
            textBoxMissingCollector.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxMissingCollector.Name = "textBoxMissingCollector";
            textBoxMissingCollector.Size = new System.Drawing.Size(212, 23);
            textBoxMissingCollector.TabIndex = 30;
            textBoxMissingCollector.Text = "unknown";
            toolTip.SetToolTip(textBoxMissingCollector, "Enter all values that should be replaced with \"Not on sheet\" in the export. Use | to separate entries");
            // 
            // tableLayoutPanelMain
            // 
            tableLayoutPanelMain.ColumnCount = 10;
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            tableLayoutPanelMain.Controls.Add(buttonCreateExport, 0, 5);
            tableLayoutPanelMain.Controls.Add(webBrowser, 0, 8);
            tableLayoutPanelMain.Controls.Add(textBoxExportFile, 1, 10);
            tableLayoutPanelMain.Controls.Add(labelXmlFile, 0, 10);
            tableLayoutPanelMain.Controls.Add(labelInstitution, 0, 0);
            tableLayoutPanelMain.Controls.Add(comboBoxInstitution, 1, 0);
            tableLayoutPanelMain.Controls.Add(labelInstitutionCode, 4, 0);
            tableLayoutPanelMain.Controls.Add(textBoxInstitutionCode, 5, 0);
            tableLayoutPanelMain.Controls.Add(labelInstitutionName, 6, 0);
            tableLayoutPanelMain.Controls.Add(textBoxInstitutionName, 7, 0);
            tableLayoutPanelMain.Controls.Add(checkBoxTypeNoteRestrictions, 5, 2);
            tableLayoutPanelMain.Controls.Add(textBoxTypeNoteRestrictions, 7, 2);
            tableLayoutPanelMain.Controls.Add(checkBoxExportTypeNotes, 2, 2);
            tableLayoutPanelMain.Controls.Add(checkBoxRestrictToLinkedNames, 6, 1);
            tableLayoutPanelMain.Controls.Add(progressBar, 0, 9);
            tableLayoutPanelMain.Controls.Add(labelNames, 0, 1);
            tableLayoutPanelMain.Controls.Add(textBoxErrors, 2, 5);
            tableLayoutPanelMain.Controls.Add(labelCity, 8, 0);
            tableLayoutPanelMain.Controls.Add(textBoxCity, 9, 0);
            tableLayoutPanelMain.Controls.Add(radioButtonExportAll, 1, 1);
            tableLayoutPanelMain.Controls.Add(radioButtonRestrictToTypeAndLast, 3, 1);
            tableLayoutPanelMain.Controls.Add(labelTypeNotes, 0, 2);
            tableLayoutPanelMain.Controls.Add(checkBoxLastNameAsStorage, 8, 1);
            tableLayoutPanelMain.Controls.Add(buttonSaveAs, 0, 6);
            tableLayoutPanelMain.Controls.Add(textBoxMessages, 2, 7);
            tableLayoutPanelMain.Controls.Add(labelMessages, 0, 7);
            tableLayoutPanelMain.Controls.Add(labelMissingCollector, 5, 3);
            tableLayoutPanelMain.Controls.Add(checkBoxIncludeAdditionalNotes, 8, 2);
            tableLayoutPanelMain.Controls.Add(textBoxMissingCollector, 7, 3);
            tableLayoutPanelMain.Controls.Add(checkBoxMissingFamily, 0, 3);
            tableLayoutPanelMain.Controls.Add(textBoxMissingFamily, 3, 3);
            tableLayoutPanelMain.Controls.Add(labelGazetteer, 4, 4);
            tableLayoutPanelMain.Controls.Add(comboBoxGazetteer, 7, 4);
            tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            tableLayoutPanelMain.RowCount = 11;
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelMain.Size = new System.Drawing.Size(1077, 635);
            tableLayoutPanelMain.TabIndex = 4;
            // 
            // labelXmlFile
            // 
            labelXmlFile.AutoSize = true;
            labelXmlFile.Dock = System.Windows.Forms.DockStyle.Fill;
            labelXmlFile.Location = new System.Drawing.Point(4, 606);
            labelXmlFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelXmlFile.Name = "labelXmlFile";
            labelXmlFile.Size = new System.Drawing.Size(75, 29);
            labelXmlFile.TabIndex = 3;
            labelXmlFile.Text = "Export file:";
            labelXmlFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelInstitution
            // 
            labelInstitution.AutoSize = true;
            labelInstitution.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInstitution.Location = new System.Drawing.Point(4, 0);
            labelInstitution.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelInstitution.Name = "labelInstitution";
            labelInstitution.Size = new System.Drawing.Size(75, 29);
            labelInstitution.TabIndex = 4;
            labelInstitution.Text = "Institution:";
            labelInstitution.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxInstitution
            // 
            tableLayoutPanelMain.SetColumnSpan(comboBoxInstitution, 3);
            comboBoxInstitution.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxInstitution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxInstitution.FormattingEnabled = true;
            comboBoxInstitution.Location = new System.Drawing.Point(87, 3);
            comboBoxInstitution.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxInstitution.Name = "comboBoxInstitution";
            comboBoxInstitution.Size = new System.Drawing.Size(311, 23);
            comboBoxInstitution.TabIndex = 5;
            comboBoxInstitution.SelectedIndexChanged += comboBoxInstitution_SelectedIndexChanged;
            // 
            // labelInstitutionCode
            // 
            labelInstitutionCode.AutoSize = true;
            labelInstitutionCode.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInstitutionCode.Location = new System.Drawing.Point(406, 0);
            labelInstitutionCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelInstitutionCode.Name = "labelInstitutionCode";
            labelInstitutionCode.Size = new System.Drawing.Size(38, 29);
            labelInstitutionCode.TabIndex = 6;
            labelInstitutionCode.Text = "Code:";
            labelInstitutionCode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelInstitutionName
            // 
            labelInstitutionName.AutoSize = true;
            labelInstitutionName.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInstitutionName.Location = new System.Drawing.Point(515, 0);
            labelInstitutionName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelInstitutionName.Name = "labelInstitutionName";
            labelInstitutionName.Size = new System.Drawing.Size(152, 29);
            labelInstitutionName.TabIndex = 8;
            labelInstitutionName.Text = "Name:";
            labelInstitutionName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxTypeNoteRestrictions
            // 
            checkBoxTypeNoteRestrictions.AutoSize = true;
            checkBoxTypeNoteRestrictions.Checked = true;
            checkBoxTypeNoteRestrictions.CheckState = System.Windows.Forms.CheckState.Checked;
            tableLayoutPanelMain.SetColumnSpan(checkBoxTypeNoteRestrictions, 2);
            checkBoxTypeNoteRestrictions.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxTypeNoteRestrictions.Location = new System.Drawing.Point(452, 57);
            checkBoxTypeNoteRestrictions.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxTypeNoteRestrictions.Name = "checkBoxTypeNoteRestrictions";
            checkBoxTypeNoteRestrictions.Size = new System.Drawing.Size(215, 23);
            checkBoxTypeNoteRestrictions.TabIndex = 10;
            checkBoxTypeNoteRestrictions.Text = "Restriction for notes (separator = | ):";
            checkBoxTypeNoteRestrictions.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            tableLayoutPanelMain.SetColumnSpan(progressBar, 10);
            progressBar.Dock = System.Windows.Forms.DockStyle.Fill;
            progressBar.Location = new System.Drawing.Point(4, 586);
            progressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(1069, 17);
            progressBar.TabIndex = 14;
            // 
            // labelNames
            // 
            labelNames.AutoSize = true;
            labelNames.Dock = System.Windows.Forms.DockStyle.Fill;
            labelNames.Location = new System.Drawing.Point(4, 29);
            labelNames.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelNames.Name = "labelNames";
            labelNames.Size = new System.Drawing.Size(75, 25);
            labelNames.TabIndex = 16;
            labelNames.Text = "Names:";
            labelNames.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCity
            // 
            labelCity.AutoSize = true;
            labelCity.Dock = System.Windows.Forms.DockStyle.Fill;
            labelCity.Location = new System.Drawing.Point(895, 0);
            labelCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelCity.Name = "labelCity";
            labelCity.Size = new System.Drawing.Size(31, 29);
            labelCity.TabIndex = 19;
            labelCity.Text = "City:";
            labelCity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxCity
            // 
            textBoxCity.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxCity.Location = new System.Drawing.Point(934, 3);
            textBoxCity.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxCity.Name = "textBoxCity";
            textBoxCity.ReadOnly = true;
            textBoxCity.Size = new System.Drawing.Size(139, 23);
            textBoxCity.TabIndex = 20;
            // 
            // radioButtonExportAll
            // 
            radioButtonExportAll.AutoSize = true;
            radioButtonExportAll.Checked = true;
            tableLayoutPanelMain.SetColumnSpan(radioButtonExportAll, 2);
            radioButtonExportAll.Location = new System.Drawing.Point(87, 32);
            radioButtonExportAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonExportAll.Name = "radioButtonExportAll";
            radioButtonExportAll.Size = new System.Drawing.Size(152, 19);
            radioButtonExportAll.TabIndex = 21;
            radioButtonExportAll.TabStop = true;
            radioButtonExportAll.Text = "Export all identifications";
            radioButtonExportAll.UseVisualStyleBackColor = true;
            // 
            // radioButtonRestrictToTypeAndLast
            // 
            radioButtonRestrictToTypeAndLast.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(radioButtonRestrictToTypeAndLast, 3);
            radioButtonRestrictToTypeAndLast.Location = new System.Drawing.Point(247, 32);
            radioButtonRestrictToTypeAndLast.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            radioButtonRestrictToTypeAndLast.Name = "radioButtonRestrictToTypeAndLast";
            radioButtonRestrictToTypeAndLast.Size = new System.Drawing.Size(207, 19);
            radioButtonRestrictToTypeAndLast.TabIndex = 22;
            radioButtonRestrictToTypeAndLast.Text = "Export types and last identification";
            radioButtonRestrictToTypeAndLast.UseVisualStyleBackColor = true;
            // 
            // labelTypeNotes
            // 
            labelTypeNotes.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelTypeNotes, 2);
            labelTypeNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTypeNotes.Location = new System.Drawing.Point(4, 54);
            labelTypeNotes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelTypeNotes.Name = "labelTypeNotes";
            labelTypeNotes.Size = new System.Drawing.Size(98, 29);
            labelTypeNotes.TabIndex = 23;
            labelTypeNotes.Text = "Type notes:";
            labelTypeNotes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // checkBoxLastNameAsStorage
            // 
            checkBoxLastNameAsStorage.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(checkBoxLastNameAsStorage, 2);
            checkBoxLastNameAsStorage.Location = new System.Drawing.Point(895, 32);
            checkBoxLastNameAsStorage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxLastNameAsStorage.Name = "checkBoxLastNameAsStorage";
            checkBoxLastNameAsStorage.Size = new System.Drawing.Size(173, 19);
            checkBoxLastNameAsStorage.TabIndex = 24;
            checkBoxLastNameAsStorage.Text = "Last identification = storage";
            checkBoxLastNameAsStorage.UseVisualStyleBackColor = true;
            // 
            // textBoxMessages
            // 
            tableLayoutPanelMain.SetColumnSpan(textBoxMessages, 8);
            textBoxMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxMessages.Location = new System.Drawing.Point(110, 253);
            textBoxMessages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxMessages.Multiline = true;
            textBoxMessages.Name = "textBoxMessages";
            textBoxMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxMessages.Size = new System.Drawing.Size(963, 57);
            textBoxMessages.TabIndex = 26;
            textBoxMessages.Visible = false;
            // 
            // labelMessages
            // 
            labelMessages.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelMessages, 2);
            labelMessages.Dock = System.Windows.Forms.DockStyle.Top;
            labelMessages.Location = new System.Drawing.Point(4, 253);
            labelMessages.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            labelMessages.Name = "labelMessages";
            labelMessages.Size = new System.Drawing.Size(98, 15);
            labelMessages.TabIndex = 27;
            labelMessages.Text = "Messages:";
            labelMessages.TextAlign = System.Drawing.ContentAlignment.TopRight;
            labelMessages.Visible = false;
            // 
            // labelMissingCollector
            // 
            labelMissingCollector.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelMissingCollector, 2);
            labelMissingCollector.Dock = System.Windows.Forms.DockStyle.Right;
            labelMissingCollector.Location = new System.Drawing.Point(512, 83);
            labelMissingCollector.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelMissingCollector.Name = "labelMissingCollector";
            labelMissingCollector.Size = new System.Drawing.Size(155, 29);
            labelMissingCollector.TabIndex = 29;
            labelMissingCollector.Text = "No collector (separator = | ):";
            labelMissingCollector.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // checkBoxIncludeAdditionalNotes
            // 
            checkBoxIncludeAdditionalNotes.AutoSize = true;
            checkBoxIncludeAdditionalNotes.Checked = true;
            checkBoxIncludeAdditionalNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            tableLayoutPanelMain.SetColumnSpan(checkBoxIncludeAdditionalNotes, 2);
            checkBoxIncludeAdditionalNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            checkBoxIncludeAdditionalNotes.Location = new System.Drawing.Point(895, 57);
            checkBoxIncludeAdditionalNotes.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxIncludeAdditionalNotes.Name = "checkBoxIncludeAdditionalNotes";
            checkBoxIncludeAdditionalNotes.Size = new System.Drawing.Size(178, 23);
            checkBoxIncludeAdditionalNotes.TabIndex = 28;
            checkBoxIncludeAdditionalNotes.Text = "Include additional notes";
            checkBoxIncludeAdditionalNotes.UseVisualStyleBackColor = true;
            // 
            // checkBoxMissingFamily
            // 
            checkBoxMissingFamily.AutoSize = true;
            checkBoxMissingFamily.Checked = true;
            checkBoxMissingFamily.CheckState = System.Windows.Forms.CheckState.Checked;
            tableLayoutPanelMain.SetColumnSpan(checkBoxMissingFamily, 3);
            checkBoxMissingFamily.Dock = System.Windows.Forms.DockStyle.Right;
            checkBoxMissingFamily.Location = new System.Drawing.Point(63, 86);
            checkBoxMissingFamily.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxMissingFamily.Name = "checkBoxMissingFamily";
            checkBoxMissingFamily.Size = new System.Drawing.Size(176, 23);
            checkBoxMissingFamily.TabIndex = 31;
            checkBoxMissingFamily.Text = "Replace missing family with:";
            checkBoxMissingFamily.UseVisualStyleBackColor = true;
            // 
            // textBoxMissingFamily
            // 
            textBoxMissingFamily.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxMissingFamily.Location = new System.Drawing.Point(247, 86);
            textBoxMissingFamily.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxMissingFamily.Name = "textBoxMissingFamily";
            textBoxMissingFamily.Size = new System.Drawing.Size(151, 23);
            textBoxMissingFamily.TabIndex = 32;
            textBoxMissingFamily.Text = "incertae sedis";
            // 
            // labelGazetteer
            // 
            labelGazetteer.AutoSize = true;
            tableLayoutPanelMain.SetColumnSpan(labelGazetteer, 3);
            labelGazetteer.Dock = System.Windows.Forms.DockStyle.Right;
            labelGazetteer.Location = new System.Drawing.Point(458, 112);
            labelGazetteer.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelGazetteer.Name = "labelGazetteer";
            labelGazetteer.Size = new System.Drawing.Size(209, 29);
            labelGazetteer.TabIndex = 33;
            labelGazetteer.Text = "Gazetteer for retrieval of country code:";
            labelGazetteer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxGazetteer
            // 
            tableLayoutPanelMain.SetColumnSpan(comboBoxGazetteer, 3);
            comboBoxGazetteer.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxGazetteer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBoxGazetteer.FormattingEnabled = true;
            comboBoxGazetteer.Location = new System.Drawing.Point(675, 115);
            comboBoxGazetteer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxGazetteer.Name = "comboBoxGazetteer";
            comboBoxGazetteer.Size = new System.Drawing.Size(398, 23);
            comboBoxGazetteer.TabIndex = 34;
            // 
            // dataSetCollectionSpecimen
            // 
            dataSetCollectionSpecimen.DataSetName = "DataSetCollectionSpecimen";
            dataSetCollectionSpecimen.Namespace = "http://tempuri.org/DataSetCollectionSpecimen.xsd";
            dataSetCollectionSpecimen.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FormExportGPI
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1077, 635);
            Controls.Add(tableLayoutPanelMain);
            helpProvider.SetHelpKeyword(this, "exportgpi_dc");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormExportGPI";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Export for the Global Plant Initiative / JSTOR";
            KeyDown += Form_KeyDown;
            tableLayoutPanelMain.ResumeLayout(false);
            tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataSetCollectionSpecimen).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.TextBox textBoxExportFile;
        private System.Windows.Forms.Button buttonCreateExport;
        private System.Windows.Forms.WebBrowser webBrowser;
        private DiversityCollection.Datasets.DataSetCollectionSpecimen dataSetCollectionSpecimen;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelXmlFile;
        private System.Windows.Forms.Label labelInstitution;
        private System.Windows.Forms.ComboBox comboBoxInstitution;
        private System.Windows.Forms.Label labelInstitutionCode;
        private System.Windows.Forms.TextBox textBoxInstitutionCode;
        private System.Windows.Forms.Label labelInstitutionName;
        private System.Windows.Forms.TextBox textBoxInstitutionName;
        private System.Windows.Forms.CheckBox checkBoxTypeNoteRestrictions;
        private System.Windows.Forms.TextBox textBoxTypeNoteRestrictions;
        private System.Windows.Forms.CheckBox checkBoxExportTypeNotes;
        private System.Windows.Forms.CheckBox checkBoxRestrictToLinkedNames;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelNames;
        private System.Windows.Forms.TextBox textBoxErrors;
        private System.Windows.Forms.Label labelCity;
        private System.Windows.Forms.TextBox textBoxCity;
        private System.Windows.Forms.RadioButton radioButtonExportAll;
        private System.Windows.Forms.RadioButton radioButtonRestrictToTypeAndLast;
        private System.Windows.Forms.Label labelTypeNotes;
        private System.Windows.Forms.CheckBox checkBoxLastNameAsStorage;
        private System.Windows.Forms.Button buttonSaveAs;
        private System.Windows.Forms.TextBox textBoxMessages;
        private System.Windows.Forms.Label labelMessages;
        private System.Windows.Forms.CheckBox checkBoxIncludeAdditionalNotes;
        private System.Windows.Forms.Label labelMissingCollector;
        private System.Windows.Forms.TextBox textBoxMissingCollector;
        private System.Windows.Forms.CheckBox checkBoxMissingFamily;
        private System.Windows.Forms.TextBox textBoxMissingFamily;
        private System.Windows.Forms.Label labelGazetteer;
        private System.Windows.Forms.ComboBox comboBoxGazetteer;
    }
}