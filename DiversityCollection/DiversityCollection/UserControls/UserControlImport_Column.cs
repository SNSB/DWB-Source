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
    /// <summary>
    /// Refers to a column in the database that should be filled either from data in a file or from values entered in a form.
    /// Several parts in the file may relate to this column
    /// </summary>
    public partial class UserControlImport_Column : UserControl, iImportColumnControl
    {

        #region Parameter

        public enum ActualDataSource { File, Interface, Database }

        private ActualDataSource _ActualDataSource;

        //public ActualDataSource ActualSourceOfData
        //{
        //    get { return _ActualDataSource; }
        //    //set { _ActualDataSource = value; }
        //}

        private readonly string _CheckBoxMessageBase = "Check if this column should be imported into the database";
        private DiversityCollection.Import_Column _Import_Column;
        private DiversityCollection.Import _Import;
        private string _StepPosition;
        private System.Collections.Generic.Dictionary<DiversityCollection.Import_Column, DiversityCollection.UserControls.UserControlImport_Column> _MultiColumnControls;

        #endregion
        
        #region Construction
        
        public UserControlImport_Column()
        {
            InitializeComponent();
            this.pictureBoxValueIsFixed.Enabled = false;
            //this.buttonSplit.Enabled = false;
            this.buttonTest.Enabled = false;
            this.buttonTranslation.Enabled = false;
            this.textBoxForAll.Enabled = false;
            this.comboBoxForAll.Enabled = false;
            this.comboBoxFormat.Enabled = false;
            this.dateTimePickerForAll.Enabled = false;
            this.radioButtonForAll.Enabled = false;
            this.radioButtonFromFile.Enabled = false;
        }

        #endregion

        #region Control

        public void initUserControl(DiversityCollection.Import_Column Import_Column,
            DiversityCollection.Import Import)
        {
            this.Import_Column = Import_Column;
            this._Import = Import;
            this.initControls();
        }

       
        /// <summary>
        /// all settings that should be set only when the user control is created
        /// and not be changed later
        /// </summary>
        private void initControls()
        {
            try
            {
                this.Height = (int)(24 * DiversityWorkbench.FormFunctions.DisplayZoomFactor);

                if (this._Import_Column.MustSelect)
                {
                    this._Import_Column.IsSelected = true;
                    /// wird anders geregelt (bei click-event) - sonst keine Aktivierung moeglich
                    //this.checkBoxColumn.Enabled = false;
                }

                // Decision column
                this.pictureBoxDecision.Visible = this._Import_Column.CanSetDecisionColumn;

                // Sequence
                if (this._Import_Column.MultiColumn)
                {
                    if (this._Import_Column.Sequence() == 1)
                    {
                        this.buttonAdd.Visible = true;
                        if (this._Import_Column.MultiColumn && this._Import_Column.Sequence() > 1)
                            this.textBoxSeparator.Text = this._Import_Column.Separator;
                        else if (this._Import_Column.Separator.Trim().Length == 0)
                            this.textBoxSeparator.Text = "";
                    }
                    else
                    {
                        this.buttonAdd.Visible = false;
                        this.textBoxSeparator.Text = this._Import_Column.Separator;
                        this._Import_Column.TypeOfSource = DiversityCollection.Import_Column.SourceType.File;
                    }
                    this.labelSeparator.Visible = true;
                    this.textBoxSeparator.Visible = true;
                }
                else
                {
                    this.buttonAdd.Visible = false;
                    //this.labelSeparator.Visible = false;
                    //this.textBoxSeparator.Visible = false;

                    // Separator
                    if ((this._Import_Column.TypeOfSource == Import_Column.SourceType.File ||
                        this._Import_Column.TypeOfSource == Import_Column.SourceType.Any) &&
                        this._Import_Column.TypeOfEntry == DiversityCollection.Import_Column.EntryType.Text &&
                        this._Import_Column.Sequence() > 0)
                    {
                        this.labelSeparator.Visible = true;
                        this.textBoxSeparator.Visible = true;
                        if (this._Import_Column.MultiColumn && this._Import_Column.Sequence() > 1)
                            this.textBoxSeparator.Text = this._Import_Column.Separator;
                        else if (this._Import_Column.Separator.Trim().Length == 0)
                            this.textBoxSeparator.Text = "";
                    }
                    else
                    {
                        this.labelSeparator.Visible = false;
                        this.textBoxSeparator.Visible = false;
                    }
                }

                // Checkbox: Set the title according to the DisplayTitle of the Import_Column
                this.checkBoxColumn.Text = this.Import_Column.getDisplayTitle();
                string ColumnInfo = this.Import_Column.Table + "\r\n";
                if (this.Import_Column.Table != this.Import_Column.TableAlias)
                    ColumnInfo += "(" + this.Import_Column.TableAlias + ")\r\n";
                ColumnInfo += this.Import_Column.Column;
                this.toolTip.SetToolTip(this.pictureBoxTableColumn, ColumnInfo);
                string Entity = this._Import_Column.Table + "." + this._Import_Column.Column;
                string Description = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, DiversityWorkbench.Entity.EntityInformation(Entity));
                if (Description.Length > 0)
                    this.toolTip.SetToolTip(this.checkBoxColumn, Description);
                else
                this.toolTip.SetToolTip(this.checkBoxColumn, DiversityWorkbench.FormFunctions.getColumnDescription(this._Import_Column.Table, this._Import_Column.Column));

                // pictureBoxValueIsFixed - only visible if value can be written in Schema
                if (this._Import_Column.TypeOfFixing == Import_Column.FixingType.Schema)
                    this.pictureBoxValueIsFixed.Visible = true;
                else this.pictureBoxValueIsFixed.Visible = false;

                // controls for the decision where the data come from (file or interface
                // radioButtonFromFile
                // radioButtonForAll
                if (this._Import_Column.TypeOfSource == Import_Column.SourceType.Interface)
                {
                    this.radioButtonFromFile.Visible = false;
                    this.radioButtonForAll.Visible = false;
                }
                else if (this._Import_Column.TypeOfSource != Import_Column.SourceType.File &&
                    this._Import_Column.TypeOfFixing != Import_Column.FixingType.None &&
                    (this._Import_Column.TypeOfEntry != DiversityCollection.Import_Column.EntryType.Database && 
                    this._Import_Column.TypeOfSource != DiversityCollection.Import_Column.SourceType.Database))
                {
                    this.radioButtonFromFile.Visible = true;
                    this.radioButtonForAll.Visible = true;
                }
                else
                {
                    this.radioButtonFromFile.Visible = false;
                    this.radioButtonForAll.Visible = false;
                }

                // buttonTranslation
                if (this._Import_Column.CanBeTransformed)
                    this.buttonTranslation.Visible = true;
                else this.buttonTranslation.Visible = false;

                // format
                if (this.Import_Column.Formats != null && this.Import_Column.Formats.Count > 0 &&
                    this._Import_Column.TypeOfEntry != DiversityCollection.Import_Column.EntryType.Boolean)
                {
                    this.comboBoxFormat.Visible = true;
                    foreach (string s in this.Import_Column.Formats)
                        this.comboBoxFormat.Items.Add(s);
                }

                // dateTimePickerForAll
                if (this._Import_Column.TypeOfSource == Import_Column.SourceType.Any &&
                    this._Import_Column.TypeOfFixing != Import_Column.FixingType.None &&
                    (this._Import_Column.TypeOfEntry == Import_Column.EntryType.Date ||
                    this._Import_Column.TypeOfEntry == Import_Column.EntryType.Time) &&
                    this._Import_Column.TypeOfEntry != DiversityCollection.Import_Column.EntryType.Boolean)
                {
                    this.dateTimePickerForAll.Visible = true;
                    if (this._Import_Column.TypeOfEntry == Import_Column.EntryType.Time)
                        this.dateTimePickerForAll.Format = DateTimePickerFormat.Time;
                    else this.dateTimePickerForAll.Format = DateTimePickerFormat.Short;
                }
                else
                    this.dateTimePickerForAll.Visible = false;

                // textBoxForAll
                if (this._Import_Column.TypeOfSource != Import_Column.SourceType.File &&
                    this._Import_Column.TypeOfFixing != Import_Column.FixingType.None &&
                    this._Import_Column.TypeOfEntry != DiversityCollection.Import_Column.EntryType.Boolean &&
                    (this._Import_Column.TypeOfEntry != DiversityCollection.Import_Column.EntryType.Database &&
                    this._Import_Column.TypeOfSource != DiversityCollection.Import_Column.SourceType.Database))
                {
                    if (this._Import_Column.TypeOfEntry == Import_Column.EntryType.MandatoryList)
                        this.textBoxForAll.Visible = false;
                    else
                        this.textBoxForAll.Visible = true;
                }
                else
                {
                    this.textBoxForAll.Visible = false;
                }

                // comboBoxForAll
                if (this._Import_Column.TypeOfSource != Import_Column.SourceType.File &&
                    this._Import_Column.TypeOfFixing != Import_Column.FixingType.None &&
                    this._Import_Column.TypeOfEntry != DiversityCollection.Import_Column.EntryType.Boolean)
                {
                    if (this._Import_Column.TypeOfEntry == Import_Column.EntryType.ListAndText
                        || this._Import_Column.TypeOfEntry == Import_Column.EntryType.MandatoryList)
                    {
                        try
                        {
                            if (this._Import_Column.getLookUpTable().Rows.Count > 0)
                            {
                                this.comboBoxForAll.Visible = true;
                                this.comboBoxForAll.DataSource = this._Import_Column.getLookUpTable();
                                this.comboBoxForAll.DisplayMember = this._Import_Column.DisplayColumn;
                                this.comboBoxForAll.ValueMember = this._Import_Column.ValueColumn;
                                if (this._Import_Column.TypeOfEntry == Import_Column.EntryType.MandatoryList)
                                    this.comboBoxForAll.Width = 150;
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                    else this.comboBoxForAll.Visible = false;
                }
                else
                {
                    this.comboBoxForAll.Visible = false;
                }

                //checkBoxForAll
                if (this._Import_Column.TypeOfEntry == DiversityCollection.Import_Column.EntryType.Boolean)
                {
                    this.checkBoxForAll.Visible = true;
                }
                else this.checkBoxForAll.Visible = false;

                this.buttonTest.Visible = false;

                if (this._Import_Column.Separator != null && this._Import_Column.Separator.Length > 0)
                    this.textBoxSeparator.Text = this._Import_Column.Separator;

                DiversityWorkbench.FormFunctions.addEditOnDoubleClickToTextboxes(this.textBoxSeparator);
                DiversityWorkbench.FormFunctions.addEditOnDoubleClickToTextboxes(this.textBoxForAll);
            }
            catch (System.Exception ex) { }
            this.setInterface();
        }

        #endregion

        #region Properties and Interface

        public void requerySourceTable()
        {
            this.comboBoxForAll.DataSource = this.Import_Column.getLookUpTable();
            this.comboBoxForAll.DisplayMember = this.Import_Column.DisplayColumn;
            this.comboBoxForAll.ValueMember = this.Import_Column.ValueColumn;
        }

        public Import getImport()
        { return this._Import; }

        public void setValue(string Value)
        {
            try
            {
                switch (this._Import_Column.TypeOfEntry)
                {
                    case Import_Column.EntryType.MandatoryList:
                        if (this._Import_Column.getLookUpTable() != null)
                        {
                            for (int i = 0; i < this._Import_Column.getLookUpTable().Rows.Count; i++)
                            {
                                if (this._Import_Column.getLookUpTable().Rows[i][this._Import_Column.ValueColumn].ToString() == Value)
                                {
                                    //this.comboBoxForAll.BeginUpdate();
                                    this.comboBoxForAll.SelectedIndex = i;
                                    //this.comboBoxForAll.Text = this._Import_Column.LookUpTable.Rows[i][1].ToString();
                                    //this.comboBoxForAll.SelectedItem = this._Import_Column.LookUpTable.Rows[i];
                                    //this.comboBoxForAll.SelectedText = this._Import_Column.LookUpTable.Rows[i][1].ToString();
                                    //this.comboBoxForAll.SelectedValue = this._Import_Column.LookUpTable.Rows[i][0].ToString();
                                    //this.comboBoxForAll.Update();
                                    //this.comboBoxForAll.EndUpdate();
                                    //this.comboBoxForAll.BackColor = System.Drawing.Color.Tomato;
                                    break;
                                }
                            }
                        }
                        //else if (this.comboBoxForAll.DataSource != null)
                        //{
                        //    //this.comboBoxForAll.SelectedIndex = -1;
                        //    System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxForAll.DataSource;
                        //    for (int i = 0; i < dt.Rows.Count; i++)
                        //    {
                        //        if (dt.Rows[i][0].ToString() == Value)
                        //        {
                        //            string DC = this.comboBoxForAll.DisplayMember;
                        //            this.comboBoxForAll.Text = dt.Rows[i][DC].ToString();
                        //            this.comboBoxForAll.SelectedIndex = i;
                        //            //this.comboBoxForAll_SelectedIndexChanged(null, null);
                        //            break;
                        //        }
                        //    }
                        //}
                        break;
                    case Import_Column.EntryType.ListAndText:
                        this.textBoxForAll.Text = Value;
                        if (this._Import_Column.getLookUpTable() != null)
                        {
                            for (int i = 0; i < this._Import_Column.getLookUpTable().Rows.Count; i++)
                            {
                                if (this._Import_Column.getLookUpTable().Rows[i][0].ToString() == Value)
                                {
                                    this.comboBoxForAll.BeginUpdate();
                                    if (this.comboBoxForAll.DataSource == null)
                                    {
                                        this.comboBoxForAll.DataSource = this._Import_Column.getLookUpTable();
                                        this.comboBoxForAll.DisplayMember = this._Import_Column.DisplayColumn;
                                        this.comboBoxForAll.ValueMember = this._Import_Column.ValueColumn;
                                    }
                                    //this.comboBoxForAll.MaxDropDownItems = i + 1;
                                    this.comboBoxForAll.SelectedIndex = i;
                                    this.comboBoxForAll.EndUpdate();
                                    break;
                                }
                            }
                        }
                        //this.comboBoxForAll.BackColor = System.Drawing.Color.Green;
                        //this.textBoxForAll.BackColor = System.Drawing.Color.Green;

                        //else if (this.comboBoxForAll.DataSource != null)
                        //{
                        //    //this.comboBoxForAll.SelectedIndex = -1;
                        //    System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxForAll.DataSource;
                        //    for (int i = 0; i < dt.Rows.Count; i++)
                        //    {
                        //        if (dt.Rows[i][0].ToString() == Value)
                        //        {
                        //            string DC = this.comboBoxForAll.DisplayMember;
                        //            this.comboBoxForAll.Text = dt.Rows[i][DC].ToString();
                        //            this.comboBoxForAll.SelectedIndex = i;
                        //            //this.comboBoxForAll_SelectedIndexChanged(null, null);
                        //            break;
                        //        }
                        //    }
                        //}
                        break;
                    case Import_Column.EntryType.Text:
                        this.textBoxForAll.Text = Value;
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }
        }
       
        public DiversityCollection.Import_Column ImportColumn()
        { return this._Import_Column; }

        public void setTitle(string Title)
        { this.checkBoxColumn.Text = Title; }

        /// <summary>
        /// setting all not permanent parts of the interface that may change during the handling
        /// e.g. setting the color of the background of the control
        /// </summary>
        public void setInterface()
        {
            try
            {
                if (this._Import_Column == null)
                    return;

                // setting all controls according to the selection of the column
                if (!this._Import_Column.IsSelected && this._Import_Column.MustSelect)
                {
                    this._Import_Column.IsSelected = true;
                    this.checkBoxColumn.Checked = true;
                }
                if (!this._Import_Column.IsSelected)
                {
                    this.checkBoxColumn.Checked = false;
                    this.radioButtonFromFile.Enabled = false;
                    this.radioButtonForAll.Enabled = false;
                    this.dateTimePickerForAll.Enabled = false;
                    this.textBoxForAll.Enabled = false;
                    this.comboBoxForAll.Enabled = false;
                    this.comboBoxFormat.Enabled = false;
                    this.buttonTest.Enabled = false;
                    this.buttonTranslation.Enabled = false;
                    this.pictureBoxValueIsFixed.Enabled = false;
                }
                else
                {
                    this.checkBoxColumn.Checked = true;
                    this.radioButtonFromFile.Enabled = true;
                    this.radioButtonForAll.Enabled = true;
                    this.buttonTest.Enabled = true;

                    // getting the actual source of the data
                    if (this.Import_Column.TypeOfSource == Import_Column.SourceType.File)
                    {
                        this._ActualDataSource = ActualDataSource.File;
                    }
                    else if (this.Import_Column.TypeOfSource == Import_Column.SourceType.Interface)
                    {
                        this._ActualDataSource = ActualDataSource.Interface;
                    }
                    else if (this.Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Database)
                    {
                        this._ActualDataSource = ActualDataSource.Database;
                    }
                    else if (this.Import_Column.TypeOfSource == Import_Column.SourceType.Any)
                    {
                        if (this._Import_Column.ColumnInSourceFile != null)
                        {
                            this._ActualDataSource = ActualDataSource.File;
                        }
                        else if (this._Import_Column.Value != null
                            && this._Import_Column.Value.Length > 0)
                        {
                            this._ActualDataSource = ActualDataSource.Interface;
                        }
                    }

                    // disable controls that change according to the data source

                    // enable controls according to the data source
                    switch (this._ActualDataSource)
                    {
                        case ActualDataSource.File:
                            this.comboBoxFormat.Enabled = true;
                            this.labelFormat.Enabled = true;
                            this.buttonTranslation.Enabled = true;

                            this.comboBoxForAll.Enabled = false;
                            this.textBoxForAll.Enabled = false;
                            this.dateTimePickerForAll.Enabled = false;
                            this.pictureBoxValueIsFixed.Enabled = false;
                            break;
                        case ActualDataSource.Interface:
                            this.comboBoxForAll.Enabled = true;
                            this.textBoxForAll.Enabled = true;
                            this.dateTimePickerForAll.Enabled = true;
                            this.pictureBoxValueIsFixed.Enabled = true;

                            this.comboBoxFormat.Enabled = false;
                            this.labelFormat.Enabled = false;
                            this.buttonTranslation.Enabled = false;
                            break;
                        default:
                            break;
                    }

                    // setting the checkboxes according to the data source
                    switch (this._ActualDataSource)
                    {
                        case ActualDataSource.File:
                            this.radioButtonFromFile.Checked = true;
                            this.radioButtonForAll.Checked = false;
                            break;
                        case ActualDataSource.Interface:
                            this.radioButtonFromFile.Checked = false;
                            this.radioButtonForAll.Checked = true;
                            break;
                        default:
                            this.radioButtonFromFile.Checked = false;
                            this.radioButtonForAll.Checked = false;
                            break;
                    }

                    if (this.Import_Column.Value != null && this.Import_Column.Value.Length > 0)
                        this.setValue(this.Import_Column.Value);

                    if (this.Import_Column.Value == null &&
                        this._ActualDataSource == ActualDataSource.Interface &&
                        this.textBoxForAll.Text.Length > 0)
                        this.Import_Column.Value = this.textBoxForAll.Text;

                    if (this.Import_Column.StepKey != null)
                    {
                        if (DiversityCollection.Import.ImportSteps.ContainsKey(this.Import_Column.StepKey))
                        {
                            DiversityCollection.Import.ImportSteps[this.Import_Column.StepKey].setStepError(this.Error());
                        }
                    }

                    if (this._ActualDataSource == ActualDataSource.File
                        && Import.CurrentImportColumn != null
                        && Import.CurrentImportColumn == this._Import_Column)
                    {
                        this._Import.ImportInterface.setCurrentImportColumn(this.Import_Column);
                    }
                    else
                    {
                        if (this._Import != null)
                            this._Import.ImportInterface.setCurrentImportColumn();
                    }

                    // setting the controls for the column in source file
                    if (this._Import_Column.ColumnInSourceFile != null 
                        && this._ActualDataSource == ActualDataSource.File 
                        && this._Import != null)
                    {
                        this._Import.ImportInterface.setColumnHeaeder((int)this._Import_Column.ColumnInSourceFile);
                        //if (this.radioButtonFromFile.Visible
                        //    && (this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Any
                        //    || this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.File))
                        //    this.radioButtonFromFile.Checked = true;
                        this.buttonColumnInSourceFile.Visible = true;
                        this.buttonColumnInSourceFile.Text = this._Import_Column.ColumnInSourceFile.ToString();
                    }
                    else
                    {
                        this.buttonColumnInSourceFile.Visible = false;
                        this.buttonColumnInSourceFile.Text = "";
                    }

                    // Setting the color for the Translation button
                    if (this._Import_Column.CanBeTransformed
                        && this._ActualDataSource == ActualDataSource.File
                        && (this._Import_Column.TranslationDictionary.Count > 0
                        || this._Import_Column.Splitters.Count > 0
                        || this._Import_Column.RegularExpressionPattern != null))
                        this.buttonTranslation.BackColor = System.Drawing.Color.Red;
                    else
                        this.buttonTranslation.BackColor = System.Drawing.SystemColors.ControlLightLight;
                }

                this.setInterfaceColor();

            }
            catch (System.Exception ex) { }
        }

        private void setInterfaceColor()
        {
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            try
            {
                if (this._Import_Column == null)
                {
                    this.BackColor = System.Drawing.SystemColors.ControlLightLight;
                }
                else
                {
                    if (!this.Import_Column.IsSelected)
                    {
                        this.BackColor = System.Drawing.SystemColors.ControlLightLight;
                    }
                    else
                    {
                        // A fixed value
                        if (this._Import_Column.ValueIsFixed)
                        {
                            if (this._Import_Column.Value != null
                                && this._Import_Column.Value.Length > 0
                                && this._Import_Column.TypeOfSource != DiversityCollection.Import_Column.SourceType.File)
                                this.BackColor = FormImportWizard.ColorForFixing;
                            else if (this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.File
                                && this._Import_Column.ColumnInSourceFile != null)
                            {
                                if (Import.AttachmentKeyImportColumn != null
                                  && this._Import_Column.Key == Import.AttachmentKeyImportColumn.Key)
                                    this.BackColor = FormImportWizard.ColorForAttachment;
                                else
                                    this.BackColor = FormImportWizard.ColorForFixing;
                            }
                            else if (this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Database &&
                                this._Import_Column.TypeOfEntry == DiversityCollection.Import_Column.EntryType.Database &&
                                this._Import_Column.TypeOfFixing == DiversityCollection.Import_Column.FixingType.Schema &&
                                this._Import_Column.IsInternalRelation &&
                                this._Import_Column.ValueIsFixed)
                                this.BackColor = FormImportWizard.ColorForFixing;
                            else
                                this.BackColor = FormImportWizard.ColorForError;
                        }
                        // A value from the file where the column has not been selected
                        else if (
                            this._Import_Column.ColumnInSourceFile == null &&
                            DiversityCollection.Import.CurrentImportColumn == this._Import_Column &&
                            this._Import_Column.TypeOfSource == Import_Column.SourceType.File)
                            this.BackColor = FormImportWizard.ColorForColumQuery;
                        // A value from the file where the column has not been selected
                        else if (
                            this._Import_Column.ColumnInSourceFile == null &&
                            this._Import_Column.TypeOfSource == Import_Column.SourceType.File)
                            this.BackColor = FormImportWizard.ColorForColumMissing;
                        // A value for all
                        else if (
                            (this._Import_Column.TypeOfSource == Import_Column.SourceType.Interface) &&
                            (this._Import_Column.Value == null ||
                            this._Import_Column.Value.Length == 0))
                            this.BackColor = FormImportWizard.ColorForError;
                        // A hidden value for all
                        //else if (
                        //    this._Import_Column.TypeOfSource != Import_Column.SourceType.File &&
                        //    !this.radioButtonForAll.Visible &&
                        //    (this._Import_Column.Value == null ||
                        //    this._Import_Column.Value.Length == 0))
                        //    this.BackColor = System.Drawing.Color.Pink;
                        //Attachment key
                        //else if (this._Import_Column.Value == null
                        //    && this._Import_Column.TypeOfSource == Import_Column.SourceType.Interface
                        //    && this.checkBoxColumn.Enabled == false)
                        //    this.BackColor = System.Drawing.Color.Pink;
                        else if ((this._Import_Column.Value == null || this._Import_Column.Value.Length == 0)
                            && this._Import_Column.TypeOfSource == Import_Column.SourceType.Interface)
                            this.BackColor = FormImportWizard.ColorForError;
                        else if (Import.AttachmentKeyImportColumn != null
                            && this._Import_Column.Key == Import.AttachmentKeyImportColumn.Key)
                            this.BackColor = FormImportWizard.ColorForAttachment;
                        else if (this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Any 
                            && this._Import_Column.MustSelect
                            && this._Import_Column.Value == null
                            && this._Import_Column.ColumnInSourceFile == null)
                            this.BackColor = FormImportWizard.ColorForError;
                        // not selected or value given
                        else
                            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
                    }

                    if (this._Import_Column.IsDecisionColumn &&
                        this._Import_Column.IsSelected)
                    {
                        this.pictureBoxDecision.Image = this.imageListDecision.Images[1];
                        this.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.pictureBoxDecision.Image = this.imageListDecision.Images[0];
                        this.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        public DiversityCollection.Import_Column Import_Column
        {
            get
            {
                if (this._Import_Column == null)
                    this._Import_Column = DiversityCollection.Import_Column.GetImportColumn(this._StepPosition, "", "", this);
                return this._Import_Column;
            }
            set
            {
                _Import_Column = value;
            }
        }

        public string Error()
        {
            try
            {
                if (!this.Import_Column.IsSelected)
                    return "";
                if (DiversityCollection.Import.ImportSteps[this.Import_Column.StepKey].IsVisible() == false)
                    return "";
                if (this.Import_Column.TypeOfSource == Import_Column.SourceType.File)
                {
                    if (this.Import_Column.ColumnInSourceFile == null)
                        return "No column for " + this.Import_Column.getDisplayTitle() + " is selected";
                    else return "";
                }

                if (this.Import_Column.TypeOfSource == Import_Column.SourceType.Interface)
                {
                    if (this._Import_Column.Value != null
                        && this._Import_Column.Value.Length > 0)
                        return "";
                    else
                        return "No " + this.Import_Column.getDisplayTitle() + " is given";
                }

                if (this.Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Any)
                {
                    if (this.Import_Column.MustSelect)
                    {
                        if (this.Import_Column.Value == null && this.Import_Column.ColumnInSourceFile == null)
                            return "No " + this.Import_Column.getDisplayTitle() + " is given";
                    }
                }
                if (this.Import_Column.MustSelect && this.Import_Column.ColumnInSourceFile == null && this.Import_Column.Value == null)
                {
                    return "No " + this.Import_Column.getDisplayTitle() + " is given";
                }
            }
            catch (System.Exception ex) { }
            return "";
        }

        /// <summary>
        /// setting the position of the column in the source file
        /// </summary>
        /// <param name="ColumnInSourceFile">the position of the column</param>
        public void setColumnInSourceFile(int? ColumnInSourceFile)
        {
            this._Import_Column.ColumnInSourceFile = ColumnInSourceFile;
            this.setInterface();
        }

        public int? getColumnInSourceFile()
        {
            return this._Import_Column.ColumnInSourceFile;
        }

        #endregion        

        #region Events
        
        private void pictureBoxValueIsFixed_Click(object sender, EventArgs e)
        {
            if (this._Import_Column.ValueIsFixed)
                this._Import_Column.ValueIsFixed = false;
            else
                this._Import_Column.ValueIsFixed = true;
            this.setInterface();
        }

        private void checkBoxColumn_Click(object sender, EventArgs e)
        {
            if (this._Import_Column.MustSelect && !this.checkBoxColumn.Checked)
            {
                System.Windows.Forms.MessageBox.Show("This entry can not be deselected.\r\nEither provide a value or remove the whole entry");
                this.checkBoxColumn.Checked = true;
                //this._Import_Column.IsSelected = true;
            }
            else
                this._Import_Column.IsSelected = this.checkBoxColumn.Checked;

            if (this.Import_Column.IsSelected
                && this.Import_Column.TypeOfSource == Import_Column.SourceType.Any && this.radioButtonForAll.Visible)
            {
                this.Import_Column.TypeOfSource = Import_Column.SourceType.File;
            }
            if (this.Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.File || 
                (this.Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Any && !this.radioButtonForAll.Visible))
                DiversityCollection.Import.CurrentImportColumn = this.Import_Column;

            if (this._Import_Column.TypeOfEntry == DiversityCollection.Import_Column.EntryType.Database &&
                this._Import_Column.TypeOfFixing == DiversityCollection.Import_Column.FixingType.Schema &&
                this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Database &&
                this._Import_Column.IsInternalRelation &&
                this.checkBoxColumn.Checked)
                this._Import_Column.ValueIsFixed = true;


            this.setInterface();
        }

        #region Radio buttons
        
        private void radioButtonFromFile_Click(object sender, EventArgs e)
        {
            if (this.Import_Column.IsSelected && this.radioButtonFromFile.Checked)
                DiversityCollection.Import.CurrentImportColumn = this.Import_Column;
            this.setTypeOfSource();
        }

        private void radioButtonForAll_Click(object sender, EventArgs e)
        {
            this.setTypeOfSource();
        }

        private void setTypeOfSource()
        {
            if (radioButtonFromFile.Checked && this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Any && this.radioButtonForAll.Visible)
                this.Import_Column.TypeOfSource = Import_Column.SourceType.File;
            else if (this.radioButtonForAll.Checked && this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Any && this.radioButtonForAll.Visible)
                this.Import_Column.TypeOfSource = Import_Column.SourceType.Interface;
            else
                this.Import_Column.TypeOfSource = Import_Column.SourceType.Any;
            if (this.radioButtonFromFile.Checked && this._Import_Column.ValueIsFixed)
                this._Import_Column.ValueIsFixed = false;
            this.setInterface();
        }
        
        #endregion

        private void buttonTranslation_Click(object sender, EventArgs e)
        {
            if (this._Import_Column.ColumnInSourceFile == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a column in the file");
                return;
            }
            DiversityCollection.Forms.FormImportWizardSplitSettings f = new DiversityCollection.Forms.FormImportWizardSplitSettings(this._Import_Column);
            f.ShowDialog();
            this.setInterface();
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormImportWizardTest f = new DiversityCollection.Forms.FormImportWizardTest(this._Import_Column, this._Import);
            f.ShowDialog();
        }

        private void textBoxForAll_Leave(object sender, EventArgs e)
        {
            this._Import_Column.Value = this.textBoxForAll.Text;
            this.setInterface();
        }

        private void comboBoxForAll_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Import_Column.Value = this.comboBoxForAll.SelectedValue.ToString();
            if (this._Import_Column.TypeOfEntry == DiversityCollection.Import_Column.EntryType.MandatoryList
                && this._Import_Column.TypeOfFixing == DiversityCollection.Import_Column.FixingType.Schema
                && !this._Import_Column.ValueIsFixed
                && this._Import_Column.MustSelect)
                this._Import_Column.ValueIsFixed = true;
            if (this.checkBoxColumn.Checked && !this._Import_Column.IsSelected)
                this._Import_Column.IsSelected = true;
            if (this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.Any ||
                this._Import_Column.TypeOfSource == DiversityCollection.Import_Column.SourceType.File)
                this._Import_Column.TypeOfSource = DiversityCollection.Import_Column.SourceType.Interface;
            if (this.checkBoxColumn.Visible == false && !this._Import_Column.IsSelected)
                this._Import_Column.IsSelected = true;
            this.setInterface();
        }

        private void dateTimePickerForAll_CloseUp(object sender, EventArgs e)
        {
            try
            {
                this._Import_Column.Value = this.dateTimePickerForAll.Value.Year.ToString() + "/" +
                    this.dateTimePickerForAll.Value.Month.ToString() + "/" +
                    this.dateTimePickerForAll.Value.Day.ToString();
                this.textBoxForAll.Text = this._Import_Column.Value;
                this.setInterface();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonTransform_Click(object sender, EventArgs e)
        {
            if (this._Import_Column.ColumnInSourceFile == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a column in the file");
                return;
            }
            DiversityCollection.Forms.FormImportWizardSplitSettings f = new DiversityCollection.Forms.FormImportWizardSplitSettings(this._Import_Column);
            f.ShowDialog();
        }

        private void comboBoxFormat_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBoxFormat.SelectedText.Length > 0)
                this._Import_Column.Format = this.comboBoxFormat.SelectedText;
        }

        private void textBoxSeparator_Leave(object sender, EventArgs e)
        {
            this._Import_Column.Separator = this.textBoxSeparator.Text;
        }

        private void checkBoxForAll_Click(object sender, EventArgs e)
        {
            this._Import_Column.Value = this.checkBoxForAll.Checked.ToString();
            this.setInterface();
        }

        private void buttonColumnInSourceFile_Click(object sender, EventArgs e)
        {
            DiversityCollection.FormImportWizard f = (DiversityCollection.FormImportWizard)this._Import.ImportInterface;
            System.Windows.Forms.DataGridView G = f.Grid();
            if ((int)this._Import_Column.ColumnInSourceFile > G.Columns.Count)
                System.Windows.Forms.MessageBox.Show("The file contains only " + G.Columns.Count + " columns. Columns " + this._Import_Column.ColumnInSourceFile.ToString() + " can not be found");
            else
                G.CurrentCell = G[(int)this._Import_Column.ColumnInSourceFile, 0];
        }
        
        private void pictureBoxDecision_Click(object sender, EventArgs e)
        {
            if (this._Import_Column.IsDecisionColumn)
                this._Import_Column.IsDecisionColumn = false;
            else this._Import_Column.IsDecisionColumn = true;
            this.setInterfaceColor();
        }

        #endregion

        #region Adding and removing columns
        
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            this.AddMultiColumn();
            //DiversityCollection.Import_Column C = DiversityCollection.Import_Column.GetImportColumn(
        }

        public void AddMultiColumn()
        {
            DiversityCollection.UserControls.UserControlImport_Column UCMulti = new UserControlImport_Column();
            if (this._MultiColumnControls == null)
                this._MultiColumnControls = new Dictionary<Import_Column,UserControlImport_Column>();
            int i = this._MultiColumnControls.Count + 2;
            DiversityCollection.Import_Column C = DiversityCollection.Import_Column.GetImportColumn(
                this._Import_Column.StepKey,
                this._Import_Column.Table,
                this._Import_Column.TableAlias,
                this._Import_Column.Column,
                i,
                UCMulti, this.Import_Column.TypeOfSource, this.Import_Column.TypeOfFixing, this.Import_Column.TypeOfEntry
                );
            C.CanBeTransformed = this._Import_Column.CanBeTransformed;
            C.Format = this._Import_Column.Format;
            C.TypeOfEntry = this._Import_Column.TypeOfEntry;
            C.TypeOfFixing = this._Import_Column.TypeOfFixing;
            C.TypeOfSource = this._Import_Column.TypeOfSource;
            UCMulti.initUserControl(C, this._Import);
            this._MultiColumnControls.Add(C, UCMulti);
            this.ShowMultiColumns();//C);
        }

        public void AddMultiColumn(DiversityCollection.Import_Column C)
        {
            if (this._MultiColumnControls == null)
                this._MultiColumnControls = new Dictionary<Import_Column,UserControlImport_Column>();
            if (!this._MultiColumnControls.ContainsKey(DiversityCollection.Import.ImportColumns[C.Key]))
            {
                DiversityCollection.UserControls.UserControlImport_Column UCMulti = new UserControlImport_Column();
                int i = this._MultiColumnControls.Count + 2;
                C.TypeOfEntry = DiversityCollection.Import_Column.EntryType.Text;
                UCMulti.initUserControl(DiversityCollection.Import.ImportColumns[C.Key], this._Import);
                this._MultiColumnControls.Add(DiversityCollection.Import.ImportColumns[C.Key], UCMulti);
                this.ShowMultiColumns();//DiversityCollection.Import.ImportColumns[C.Key]);
            }
        }

        private void ShowMultiColumns()//DiversityCollection.Import_Column C)
        {
            this.panelMultiColumn.Controls.Clear();
            if (this._MultiColumnControls.Count == 0)
            {
                this.Height = (int)(24 * DiversityWorkbench.FormFunctions.DisplayZoomFactor);
            }
            else
            {
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<DiversityCollection.Import_Column, DiversityCollection.UserControls.UserControlImport_Column> KV in this._MultiColumnControls)
                {
                    System.Windows.Forms.Panel P = new Panel();
                    P.Height = (int)(24 * DiversityWorkbench.FormFunctions.DisplayZoomFactor);
                    System.Windows.Forms.Label L = new Label();
                    L.Text = KV.Value._Import_Column.Sequence().ToString();
                    L.Width = 16;
                    L.TextAlign = ContentAlignment.MiddleLeft;
                    L.Dock = DockStyle.Left;
                    P.Controls.Add(L);
                    P.Controls.Add(KV.Value);
                    KV.Value.Dock = DockStyle.Fill;
                    KV.Value.BringToFront();
                    P.Dock = DockStyle.Top;
                    this.panelMultiColumn.Controls.Add(P);
                    P.BringToFront();
                    i++;
                    if (i == this._MultiColumnControls.Count)
                    {
                        KV.Value.buttonRemove.Visible = true;
                        KV.Value.buttonRemove.Tag = this;
                    }
                    else
                    {
                        KV.Value.buttonRemove.Visible = false;
                        KV.Value.buttonRemove.Tag = null;
                    }
                }
                this.Height = (this._MultiColumnControls.Count + 1) * (int)(24 * DiversityWorkbench.FormFunctions.DisplayZoomFactor);
            }
        }
        
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this.RemoveLastMultiColumn();
        }

        private void RemoveLastMultiColumn()
        {
            if (DiversityCollection.Import.ImportColumns.ContainsKey(this._Import_Column.Key))
            {
                DiversityCollection.UserControls.UserControlImport_Column UC = (DiversityCollection.UserControls.UserControlImport_Column)this._Import_Column.ImportColumnControl;
                if (UC.buttonRemove.Tag != null)
                {
                    DiversityCollection.UserControls.UserControlImport_Column ParentUC = (DiversityCollection.UserControls.UserControlImport_Column)UC.buttonRemove.Tag;
                    if (this._Import_Column.ColumnInSourceFile != null)
                    {
                        int i = (int)this._Import_Column.ColumnInSourceFile;
                        this._Import_Column.ColumnInSourceFile = null;
                        this._Import.ImportInterface.setColumnHeaeder(i);
                        //if (this._Import.ImportInterface.Grid
                        //this._Import.ImportInterface.setColumnDisplays(i);
                    }
                    DiversityCollection.Import.ImportColumns.Remove(this._Import_Column.Key);
                    ParentUC._MultiColumnControls.Remove(this._Import_Column);
                    ParentUC.ShowMultiColumns();
                }
                //DiversityCollection.UserControls.UserControlImport_Column UCMulti = new UserControlImport_Column();
                //int i = this._MultiColumnControls.Count + 2;
                //C.TypeOfEntry = DiversityCollection.Import_Column.EntryType.Text;
                //UCMulti.initUserControl(DiversityCollection.Import.ImportColumns[C.Key], this._Import);
                //this._MultiColumnControls.Add(DiversityCollection.Import.ImportColumns[C.Key], UCMulti);
                //this.ShowMultiColumns(DiversityCollection.Import.ImportColumns[C.Key]);
            }
            //this.ShowMultiColumns();//this._Import_Column);
        }

        #endregion

    }
}
