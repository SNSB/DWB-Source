using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Import
{
    public partial class UserControlDataColumn : UserControl, iDataColumnInterface//, iDisposableControl
    {
        #region Parameter

        private DiversityWorkbench.Import.DataTable _DataTable;
        private DiversityWorkbench.Import.DataColumn _DataColumn;
        private DiversityWorkbench.Import.iWizardInterface _WizardInterface;

        #endregion

        #region Construction

        public UserControlDataColumn(DiversityWorkbench.Import.DataTable Table, string DataColumn, DiversityWorkbench.Import.iWizardInterface WizardInterface, string HelpNameSpace)
        {
            InitializeComponent();
            this._DataTable = Table;
            this._DataColumn = Table.DataColumns[DataColumn];
            this._DataColumn.iDataColumnInterface = this;
            this._WizardInterface = WizardInterface;
            this.initControl();
            this.helpProvider.HelpNamespace = HelpNameSpace;
        }

        #endregion

        #region Control

        public void initControl()
        {
            this.SuspendLayout();
            try
            {
                // If the current control is for Attachment, not necessarily the AttachmentColumn
                bool IsAttachmentControl = false;
                switch (DiversityWorkbench.Import.Import.AttachmentType)
                {
                    case Import.TypeOfAttachment.ChildParentInSameTable:
                        if (DiversityWorkbench.Import.Import.AttachmentColumn != null &&
                            DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == this._DataColumn.DataTable.TableAlias &&
                            !this._DataTable.IsForAttachment &&
                            this._DataTable.AttachViaParentChildRelation() &&
                            this._DataColumn.ForeignRelationTableAlias == DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias &&
                            this._DataColumn.ForeignRelationColumn == this._DataColumn.ParentColumn)
                        {
                            IsAttachmentControl = true;
                        }
                        break;
                    case Import.TypeOfAttachment.AttachToChild:
                    case Import.TypeOfAttachment.AttachToTable:
                        if (DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == this._DataColumn.DataTable.TableAlias &&
                            DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName == this._DataColumn.ColumnName)
                            IsAttachmentControl = true;
                        break;
                    case Import.TypeOfAttachment.TableOnlyForAttachment:
                        if (DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == this._DataColumn.DataTable.TableAlias &&
                            DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName == this._DataColumn.ColumnName)
                            IsAttachmentControl = true;
                        break;
                }

                bool IsLocalParentAttachmentColumn = false;
                if (!IsAttachmentControl)
                {
                    if (this._DataColumn.IsParentAttachmentColumn)
                        IsLocalParentAttachmentColumn = true;
                }

                bool IsLocalChildParentColumn;
                if (this._DataColumn.ForeignRelationTable == this._DataColumn.DataTable.TableName &&
                    !IsAttachmentControl)
                    IsLocalChildParentColumn = true;
                else IsLocalChildParentColumn = false;

                // Height
                if (this._DataColumn.IsMultiColumn
                    && this._DataColumn.MultiColumns != null
                    && this._DataColumn.MultiColumns.Count > 0)
                {
                    foreach (DiversityWorkbench.Import.ColumnMulti CM in this._DataColumn.MultiColumns)
                    {
                        DiversityWorkbench.Import.UserControlDataColumnMulti UM = new UserControlDataColumnMulti(CM, this, this._WizardInterface, this.helpProvider.HelpNamespace);
                        UM.Dock = DockStyle.Bottom;
                        this.panelMultiColumn.Controls.Add(UM);
                    }
                    this.Height = (int)(24 * (this._DataColumn.MultiColumns.Count + 1) * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                }
                else
                {
                    foreach (System.Windows.Forms.Control C in this.panelMultiColumn.Controls)
                        C.Dispose();
                    this.panelMultiColumn.Controls.Clear();
                    this.Height = (int)(24 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                }

                // Backcolor
                if (IsAttachmentControl)
                    this.BackColor = DiversityWorkbench.Import.Import.ColorAttachment;
                else if (this._DataColumn.CompareKey)
                    this.BackColor = DiversityWorkbench.Import.Import.ColorKeyColumn;
                //else if (!this._DataColumn.AllowInsert)
                //    this.BackColor = System.Drawing.SystemColors.ControlDark;
                else
                    this.BackColor = System.Drawing.SystemColors.Control;

                // Forecolor
                if (!this._DataColumn.AllowInsert)
                    this.ForeColor = System.Drawing.SystemColors.ControlDark;

                // pictureBoxDecision
                if ((IsAttachmentControl && this._DataColumn.IsDecisive && this._DataColumn.CompareKey) || !this._DataColumn.AllowInsert)
                    this.pictureBoxDecision.Enabled = false;
                else
                    this.pictureBoxDecision.Enabled = this._DataColumn.IsSelected;
                if (this._DataColumn.IsDecisive)
                {
                    this.pictureBoxDecision.Image = this.imageListDecision.Images[0];
                    this.toolTip.SetToolTip(this.pictureBoxDecision, "Decisive column. If any decisive column contains content, the data will be imported. Click to make this column not decisive");
                    this.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    this.pictureBoxDecision.Image = this.imageListDecision.Images[1];
                    this.toolTip.SetToolTip(this.pictureBoxDecision, "Not decisive. Click to make this column a decisive column");
                    this.ForeColor = System.Drawing.Color.Black;
                }
                if (IsAttachmentControl)
                    this.pictureBoxDecision.BackColor = DiversityWorkbench.Import.Import.ColorAttachment;
                else if (this._DataColumn.CompareKey)
                    this.pictureBoxDecision.BackColor = DiversityWorkbench.Import.Import.ColorKeyColumn;
                else
                    this.pictureBoxDecision.BackColor = System.Drawing.SystemColors.Control;

                // pictureBoxCompareKey
                if (IsAttachmentControl && this._DataColumn.CompareKey)
                    this.pictureBoxCompareKey.Enabled = false;
                else
                    this.pictureBoxCompareKey.Enabled = this._DataColumn.IsSelected;
                if (this._DataColumn.CompareKey)
                {
                    if (this._DataTable.AttachViaParentChildRelation() &&
                        this._DataColumn.IsParentAttachmentColumn)
                    {
                        this.pictureBoxCompareKey.Image = this.imageListKey.Images[2];
                        this.pictureBoxCompareKey.BackColor = DiversityWorkbench.Import.Import.ColorAttachment;
                        this.toolTip.SetToolTip(this.pictureBoxCompareKey, "This column is used as key for comparision to attach data to existing data in the database");
                    }
                    else
                    {
                        this.pictureBoxCompareKey.Image = this.imageListKey.Images[0];
                        this.pictureBoxCompareKey.BackColor = DiversityWorkbench.Import.Import.ColorKeyColumn;
                        this.toolTip.SetToolTip(this.pictureBoxCompareKey, "This column is used as a key for comparision with data in the database. Click if it should not be used");
                    }
                }
                else
                {
                    if (IsAttachmentControl)
                    {
                        this.pictureBoxCompareKey.Image = this.imageListKey.Images[0];
                        this.pictureBoxCompareKey.BackColor = DiversityWorkbench.Import.Import.ColorAttachment;
                        this.toolTip.SetToolTip(this.pictureBoxCompareKey, "This column is used as a key for comparision with data in the database. Click if it should not be used");
                    }
                    else
                    {
                        this.pictureBoxCompareKey.Image = this.imageListKey.Images[1];
                        this.pictureBoxCompareKey.BackColor = System.Drawing.SystemColors.Control;
                        this.toolTip.SetToolTip(this.pictureBoxCompareKey, "This column is not used as key. Click to use it as a key for comparision with data in the database");
                    }
                }

                // checkBoxColumn
                if (IsAttachmentControl || !this._DataColumn.AllowInsert)
                {
                    this.checkBoxColumn.Enabled = false;
                    if (DiversityWorkbench.Import.Import.AttachmentType == Import.TypeOfAttachment.ChildParentInSameTable)
                        this.checkBoxColumn.Text = "Att.to " + DiversityWorkbench.Import.Import.AttachmentColumn.DisplayText;
                    else
                        this.checkBoxColumn.Text = this._DataColumn.DisplayText;
                }
                else
                {
                    this.checkBoxColumn.Enabled = true;
                    this.checkBoxColumn.Text = this._DataColumn.DisplayText;
                }
                if (IsAttachmentControl)
                    this._DataColumn.IsSelected = true;
                this.checkBoxColumn.Checked = this._DataColumn.IsSelected;

                // comboBoxInternalRelation
                if (IsLocalParentAttachmentColumn || IsLocalChildParentColumn || this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.comboBoxInternalRelation.Visible = true;
                    this.comboBoxInternalRelation.Enabled = this._DataColumn.IsSelected;
                    if ((this._DataColumn.SelectParallelForeignRelationTableAlias &&
                        this._DataColumn.ForeignRelationTableAlias != null)
                        ||
                        (this._DataColumn.ForeignRelationTableAlias != null
                        && this._DataColumn.ForeignRelationTable != null
                        && this._DataColumn.ForeignRelationColumn != null))
                    {
                        System.Data.DataTable dtSource = this.dtSourceInternalRelation;
                        this.comboBoxInternalRelation.DataSource = dtSource;
                        this.comboBoxInternalRelation.DisplayMember = "Display";
                        this.comboBoxInternalRelation.ValueMember = "TableAlias";
                        for (int i = 0; i < dtSource.Rows.Count; i++)
                        {
                            if (dtSource.Rows[i]["TableAlias"].ToString() == this._DataColumn.ForeignRelationTableAlias)
                            {
                                this.comboBoxInternalRelation.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }

                // radioButtonFromFile
                if (IsAttachmentControl || IsLocalChildParentColumn || this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.radioButtonFromFile.Visible = false;
                    this.radioButtonFromFile.Enabled = false;
                }
                else
                {
                    this.radioButtonFromFile.Enabled = this._DataColumn.IsSelected;
                    this.radioButtonFromFile.Visible = true;
                }
                if (this._DataColumn.TypeOfSource == DataColumn.SourceType.File)
                    this.radioButtonFromFile.Checked = true;
                else this.radioButtonFromFile.Checked = false;

                // buttonTranslation
                if (IsLocalChildParentColumn || this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.buttonTranslation.Visible = false;
                }
                else
                {
                    if (this._DataColumn.Transformations != null
                        && this._DataColumn.Transformations.Count > 0)
                    {
                        this.buttonTranslation.BackColor = System.Drawing.Color.Red;
                    }
                    else
                        this.buttonTranslation.BackColor = System.Drawing.SystemColors.Control;
                    if (this._DataColumn.TypeOfSource == DataColumn.SourceType.File &&
                        this._DataColumn.IsSelected)
                        this.buttonTranslation.Enabled = true;
                    else
                    {
                        if (IsAttachmentControl &&
                            DiversityWorkbench.Import.Import.AttachmentColumn.CanEditColumnContent)
                            this.buttonTranslation.Enabled = true;
                        else
                            this.buttonTranslation.Enabled = false;
                    }
                }

                // pictureBoxCopyPrevious
                if (IsLocalChildParentColumn || this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.pictureBoxCopyPrevious.Visible = false;
                }
                else
                {
                    this.pictureBoxCopyPrevious.Visible = true;
                    if (this._DataColumn.CopyPrevious)
                    {
                        this.pictureBoxCopyPrevious.Image = this.imageListCopyLine.Images[0];
                        this.pictureBoxCopyPrevious.BackColor = System.Drawing.Color.White;
                        this.toolTip.SetToolTip(this.pictureBoxCopyPrevious, "The values found in the file will be copied into following empty lines. Click if the lines should NOT be copied. Do not use for merging!");
                    }
                    else
                    {
                        this.pictureBoxCopyPrevious.Image = this.imageListCopyLine.Images[1];
                        this.pictureBoxCopyPrevious.BackColor = System.Drawing.SystemColors.Control;
                        this.toolTip.SetToolTip(this.pictureBoxCopyPrevious, "The values found in the file will NOT be copied into following empty lines. Click if the lines should be copied. Do not use for merging!");
                    }
                    if (this._DataColumn.TypeOfSource == DataColumn.SourceType.File &&
                        this._DataColumn.IsSelected)
                        this.pictureBoxCopyPrevious.Enabled = true;
                    else
                    {
                        if (IsAttachmentControl)
                            this.pictureBoxCopyPrevious.Enabled = true;
                        else
                            this.pictureBoxCopyPrevious.Enabled = false;
                    }
                }

                // Prefix & Postfix
                if (this._DataColumn.CanEditColumnContent
                    && !(IsLocalParentAttachmentColumn || IsLocalChildParentColumn)
                    && !this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.labelPrefix.Visible = true;
                    this.textBoxPrefix.Visible = true;
                    this.textBoxPrefix.Text = this._DataColumn.Prefix;
                    this.labelPostfix.Visible = true;
                    this.textBoxPostfix.Visible = true;
                    this.textBoxPostfix.Text = this._DataColumn.Postfix;
                }
                else
                {
                    if (IsAttachmentControl &&
                        DiversityWorkbench.Import.Import.AttachmentColumn.CanEditColumnContent &&
                        !IsLocalChildParentColumn &&
                        !this._DataColumn.SelectParallelForeignRelationTableAlias)
                    {
                        this.labelPrefix.Visible = true;
                        this.textBoxPrefix.Visible = true;
                        this.labelPostfix.Visible = true;
                        this.textBoxPostfix.Visible = true;
                    }
                    else
                    {
                        this.labelPrefix.Visible = false;
                        this.textBoxPrefix.Visible = false;
                        this.labelPostfix.Visible = false;
                        this.textBoxPostfix.Visible = false;
                    }
                }
                if (this._DataColumn.TypeOfSource == DataColumn.SourceType.File &&
                    this._DataColumn.IsSelected)
                {
                    this.labelPrefix.Enabled = true;
                    this.textBoxPrefix.Enabled = true;
                    this.labelPostfix.Enabled = true;
                    this.textBoxPostfix.Enabled = true;
                }
                else
                {
                    if (IsAttachmentControl &&
                        DiversityWorkbench.Import.Import.AttachmentColumn.CanEditColumnContent)
                    {
                        this.labelPrefix.Enabled = true;
                        this.textBoxPrefix.Enabled = true;
                        this.labelPostfix.Enabled = true;
                        this.textBoxPostfix.Enabled = true;
                    }
                    else
                    {
                        this.labelPrefix.Enabled = false;
                        this.textBoxPrefix.Enabled = false;
                        this.labelPostfix.Enabled = false;
                        this.textBoxPostfix.Enabled = false;
                    }
                }
                // Geography
                if (this._DataColumn.DataType == "geography" &&
                    this._DataColumn.IsSelected &&
                    this._DataColumn.TypeOfSource == DataColumn.SourceType.File)
                {
                    if (this._DataColumn.Prefix == null)
                        this.textBoxPrefix.Text = "geography::STPointFromText('POINT(";
                    if (this._DataColumn.Postfix == null)
                        this.textBoxPostfix.Text = ")', 4326)";
                }

                // buttonAdd
                if (this._DataColumn.IsMultiColumn)
                {
                    this.buttonAdd.Visible = true;
                }
                else if (IsAttachmentControl &&
                    DiversityWorkbench.Import.Import.AttachmentColumn.IsMultiColumn &&
                    !IsLocalChildParentColumn)
                    this.buttonAdd.Visible = true;
                else
                    this.buttonAdd.Visible = false;

                if (this._DataColumn.TypeOfSource == DataColumn.SourceType.File &&
                    this._DataColumn.IsSelected)
                    this.buttonAdd.Enabled = true;
                else if (IsAttachmentControl &&
                        DiversityWorkbench.Import.Import.AttachmentColumn.IsMultiColumn)
                    this.buttonAdd.Enabled = true;
                else
                    this.buttonAdd.Enabled = false;

                // buttonColumnInSourceFile
                if (IsLocalChildParentColumn || this._DataColumn.SelectParallelForeignRelationTableAlias)
                    this.buttonColumnInSourceFile.Visible = false;
                else
                {
                    this.buttonColumnInSourceFile.Visible = true;
                    if (this._DataColumn.FileColumn != null)
                    {
                        this.buttonColumnInSourceFile.Text = this._DataColumn.FileColumn.ToString();
                        this.buttonColumnInSourceFile.BackColor = System.Drawing.SystemColors.Control;
                        this.buttonColumnInSourceFile.Image = null;
                    }
                    else
                    {
                        this.buttonColumnInSourceFile.Text = "";
                        this.buttonColumnInSourceFile.Image = this.imageListFileColumn.Images[0];
                        //if (this._DataColumn.TypeOfSource == DataColumn.SourceType.File ||
                        //    IsAttachmentControl)
                        //    this.buttonColumnInSourceFile.BackColor = System.Drawing.Color.Red;
                        //else 
                        //    this.buttonColumnInSourceFile.BackColor = System.Drawing.SystemColors.Control;
                    }
                    if (this._DataColumn.TypeOfSource == DataColumn.SourceType.File &&
                        this._DataColumn.IsSelected)
                        this.buttonColumnInSourceFile.Enabled = this._DataColumn.IsSelected;
                    else if (IsAttachmentControl)
                        this.buttonColumnInSourceFile.Enabled = true;
                    else
                        this.buttonColumnInSourceFile.Enabled = false;
                }

                // radioButtonForAll
                if (this._DataColumn.MultiColumns.Count > 0 ||
                    IsAttachmentControl ||
                    IsLocalChildParentColumn ||
                    this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.radioButtonForAll.Visible = false;
                }
                else
                {
                    this.radioButtonForAll.Visible = true;
                    // Markus: Adaption for delegates and if value can be taken via function
                    if (this._DataColumn.PrepareInsertDefined && this._DataColumn.SourceFunctionDisplayText.Length > 0)
                    {
                        this.radioButtonForAll.Text = this._DataColumn.SourceFunctionDisplayText;
                        if (this._DataColumn.DataRetrievalType == DataColumn.RetrievalType.IDorIDviaTextFromFile)
                            this.radioButtonForAll.Visible = false;
                    }
                }
                if (this._DataColumn.DataRetrievalType == DataColumn.RetrievalType.Default)
                    this.radioButtonForAll.Enabled = this._DataColumn.IsSelected;
                if (this._DataColumn.TypeOfSource == DataColumn.SourceType.Interface)
                    this.radioButtonForAll.Checked = true;
                else this.radioButtonForAll.Checked = false;

                // comboBoxForAll
                if (this._DataColumn.MultiColumns.Count > 0 ||
                    IsAttachmentControl ||
                    IsLocalChildParentColumn ||
                    this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.comboBoxForAll.Visible = false;
                }
                else
                {
                    // Markus: Adaption for delegates and if value can be taken via function
                    if (this._DataColumn.PrepareInsertDefined && this._DataColumn.SourceFunctionDisplayText.Length > 0)
                    {
                        this.comboBoxForAll.Visible = false;
                    }
                    else if (this._DataColumn.SqlDataSource != null
                        && this._DataColumn.SqlDataSource.Length > 0)
                    {
                        this.comboBoxForAll.Visible = true;
                        this.comboBoxForAll.Width = 120;
                        this.textBoxForAll.Visible = false;
                        this.comboBoxForAll.DataSource = this._DataColumn.MandatoryList;
                        // Markus 16.5.23: Einschränkung auf Quelle
                        if (this._DataColumn.RestrictToDatasourceValues)
                            this.comboBoxForAll.DropDownStyle = ComboBoxStyle.DropDownList;

                        //if (this._DataColumn.MandatoryList != null && this._DataColumn.SqlDataSource.Length > 0)
                        //    this.comboBoxForAll.DropDownStyle = ComboBoxStyle.DropDownList;
                        //else
                        //    this.comboBoxForAll.DropDownStyle = ComboBoxStyle.DropDown;
                        if (this._DataColumn.MandatoryListDisplayColumn == null || this._DataColumn.MandatoryListDisplayColumn.Length == 0)
                            this.comboBoxForAll.DisplayMember = "DisplayText";
                        else
                            this.comboBoxForAll.DisplayMember = this._DataColumn.MandatoryListDisplayColumn;
                        if (this._DataColumn.MandatoryListValueColumn == null)
                        {
                            this.comboBoxForAll.ValueMember = "Value";
                        }
                        else
                            this.comboBoxForAll.ValueMember = this._DataColumn.MandatoryListValueColumn;
                        if (this._DataColumn.Value != null && this._DataColumn.Value.Length > 0 && this._DataColumn.Value != "System.Data.DataRowView")
                        {
                            for (int i = 0; i < this._DataColumn.MandatoryList.Rows.Count; i++)
                            {
                                if (this._DataColumn.MandatoryList.Rows[i][this.comboBoxForAll.ValueMember].ToString() == this._DataColumn.Value) // this._DataColumn.Value = 931
                                {
                                    this.comboBoxForAll.SelectedIndex = i; // i = 7
                                    break;
                                }
                                //if (this._DataColumn.MandatoryList.Rows[i][this.comboBoxForAll.ValueMember].ToString() == this._DataColumn.TransformedValue())
                                //{
                                //    this.comboBoxForAll.SelectedIndex = i; // i = 7
                                //    break;
                                //}
                            }
                        }
                        //// Toni: Preselect single possible value
                        //if (this.comboBoxForAll.Items.Count == 1 && this.comboBoxForAll.SelectedItem != null)
                        //{
                        //    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxForAll.SelectedItem;
                        //    this._DataColumn.Value = R.Row["Value"].ToString();
                        //    this._WizardInterface.setTableMessage(this._DataTable);
                        //}
                    }
                    else
                        this.comboBoxForAll.Visible = false;
                }
                if (this._DataColumn.TypeOfSource == DataColumn.SourceType.Interface &&
                    this._DataColumn.IsSelected)
                    this.comboBoxForAll.Enabled = true;
                else
                    this.comboBoxForAll.Enabled = false;

                // textBoxForAll
                if (this._DataColumn.MultiColumns.Count > 0 ||
                    IsAttachmentControl ||
                    IsLocalChildParentColumn ||
                    this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.textBoxForAll.Visible = false;
                }
                else
                {
                    if ((this._DataColumn.SqlDataSource != null
                        && this._DataColumn.SqlDataSource.Length > 0) ||
                        this._DataColumn.DataType == "bit")
                        this.textBoxForAll.Visible = false;
                    // Markus: Adaption for delegates and if value can be taken via function
                    else if (this._DataColumn.PrepareInsertDefined && this._DataColumn.SourceFunctionDisplayText.Length > 0)
                        this.textBoxForAll.Visible = false;
                    // Markus: Only for values taken from the interface
                    else if (this._DataColumn.TypeOfSource == DataColumn.SourceType.Interface)
                    {
                        this.textBoxForAll.Visible = true;
                        this.textBoxForAll.Text = this._DataColumn.OriginalValue; // Toni this._DataColumn.Value;
                    }
                    else
                    {
                        this.textBoxForAll.Visible = true;
                        //this.textBoxForAll.Text = this._DataColumn.OriginalValue; // Toni this._DataColumn.Value;
                    }
                }
                if (this._DataColumn.TypeOfSource == DataColumn.SourceType.Interface &&
                    this._DataColumn.IsSelected)
                    this.textBoxForAll.Enabled = true;
                else
                    this.textBoxForAll.Enabled = false;

                // checkBoxForAll
                if (this._DataColumn.MultiColumns.Count > 0 ||
                    IsAttachmentControl ||
                    IsLocalChildParentColumn ||
                    this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.checkBoxForAll.Visible = false;
                }
                else
                {
                    // Markus: Adaption for delegates and if value can be taken via function
                    if (this._DataColumn.PrepareInsertDefined && this._DataColumn.SourceFunctionDisplayText.Length > 0)
                        this.checkBoxForAll.Visible = false;
                    else if (this._DataColumn.DataType == "bit")
                    {
                        this.checkBoxForAll.Visible = true;
                        bool IsTrue;
                        if ((bool.TryParse(this._DataColumn.Value, out IsTrue) &&
                            IsTrue) || this._DataColumn.Value == "1")
                            this.checkBoxForAll.Checked = true;
                        else
                            this.checkBoxForAll.Checked = false;
                    }
                    else this.checkBoxForAll.Visible = false;
                }
                if (this._DataColumn.TypeOfSource == DataColumn.SourceType.Interface &&
                    this._DataColumn.IsSelected)
                    this.checkBoxForAll.Enabled = true;
                else
                    this.checkBoxForAll.Enabled = false;

                // dateTimePickerForAll
                if (this._DataColumn.MultiColumns.Count > 0 ||
                    IsAttachmentControl ||
                    IsLocalChildParentColumn ||
                    this._DataColumn.SelectParallelForeignRelationTableAlias)
                {
                    this.dateTimePickerForAll.Visible = false;
                }
                else
                {
                    // Markus: Adaption for delegates and if value can be taken via function
                    if (this._DataColumn.PrepareInsertDefined && this._DataColumn.SourceFunctionDisplayText.Length > 0)
                        this.dateTimePickerForAll.Visible = false;
                    else if (this._DataColumn.DataType == "datetime"
                        || this._DataColumn.DataType == "smalldatetime")
                        this.dateTimePickerForAll.Visible = true;
                    else this.dateTimePickerForAll.Visible = false;
                }
                if (this._DataColumn.TypeOfSource == DataColumn.SourceType.Interface &&
                    this._DataColumn.IsSelected)
                    this.dateTimePickerForAll.Enabled = true;
                else
                    this.dateTimePickerForAll.Enabled = false;

            }
            catch (System.Exception ex)
            {
            }
            this.ResumeLayout();
            this._WizardInterface.setTableMessage(this._DataTable);
        }

        private void ShowInterfaceControls(bool Show)
        {
            this.radioButtonFromFile.Visible = Show;
            this.radioButtonForAll.Visible = Show;
            this.textBoxForAll.Visible = Show;
            this.textBoxPrefix.Visible = Show;
            this.textBoxPostfix.Visible = Show;
            this.labelPrefix.Visible = Show;
            this.labelPostfix.Visible = Show;
            this.buttonAdd.Visible = Show;
            this.buttonTranslation.Visible = Show;
            this.checkBoxForAll.Visible = Show;
            this.dateTimePickerForAll.Visible = Show;
            this.comboBoxForAll.Visible = Show;
        }

        public void setParentAttachmentColumn()
        {
            this._DataColumn.IsParentAttachmentColumn = true;
        }

        //public void DisposeComponents()
        //{
        //    this.toolTip.Dispose();
        //    this.helpProvider.Dispose();
        //    this.imageListCopyLine.Dispose();
        //    this.imageListDecision.Dispose();
        //    this.imageListFileColumn.Dispose();
        //    this.imageListKey.Dispose();
        //}

        #endregion

        #region Events

        private void buttonTranslation_Click(object sender, EventArgs e)
        {
            if (this._DataColumn.FileColumn == null)
                return;
            DiversityWorkbench.Import.FormTransformation f = new FormTransformation(this._DataColumn.Transformations, this._DataColumn.Prefix, this._DataColumn.Postfix, this._DataColumn, (int)this._DataColumn.FileColumn, this._WizardInterface, this.helpProvider.HelpNamespace);
            f.ShowDialog();
            this.initControl();
        }

        private void checkBoxColumn_Click(object sender, EventArgs e)
        {
            this._DataColumn.IsSelected = this.checkBoxColumn.Checked;
            if (this._DataColumn.IsSelected && !this.checkBoxColumn.Checked)
            {
                string Message = "";
                if (!this._DataColumn.IsNullable && (this._DataColumn.ColumnDefault == null || this._DataColumn.ColumnDefault.Length == 0))
                    Message = "This column can not be deselected as it is necessary for the import of data and no default has been defined";
                if (Message.Length > 0)
                    System.Windows.Forms.MessageBox.Show(Message);
            }
            this.checkBoxColumn.Checked = this._DataColumn.IsSelected;
            this.initControl();
            this._WizardInterface.SetGridHeaders();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            int? iScroll = null;
            if (this._WizardInterface.DataGridView().SelectedCells.Count > 0)
                iScroll = this._WizardInterface.DataGridView().SelectedCells[0].ColumnIndex;
            DiversityWorkbench.Import.FormFileDataGrid f = new FormFileDataGrid(this.DataGridCopy(50), this._DataColumn, iScroll);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.SelectedFileColumn() != null)
            {
                int i = (int)f.SelectedFileColumn();
                DiversityWorkbench.Import.ColumnMulti M = new ColumnMulti(this._DataColumn, this, i);
                this.initControl();
                this._WizardInterface.SetGridHeaders();
            }
        }

        private void pictureBoxCopyPrevious_Click(object sender, EventArgs e)
        {
            this._DataColumn.CopyPrevious = !this._DataColumn.CopyPrevious;
            this.initControl();
        }

        private void pictureBoxCompareKey_Click(object sender, EventArgs e)
        {
            this._DataColumn.CompareKey = !this._DataColumn.CompareKey;
            this.initControl();
        }

        private void pictureBoxDecision_Click(object sender, EventArgs e)
        {
            this._DataColumn.IsDecisive = !this._DataColumn.IsDecisive;
            this.initControl();
        }

        private void radioButtonFromFile_Click(object sender, EventArgs e)
        {
            if (this.radioButtonFromFile.Checked)
            {
                this._DataColumn.TypeOfSource = DataColumn.SourceType.File;
                int? ScrollPosition = null;
                if (this._WizardInterface.DataGridView().SelectedCells.Count > 0)
                    ScrollPosition = this._WizardInterface.DataGridView().SelectedCells[0].ColumnIndex;
                DiversityWorkbench.Import.FormFileDataGrid f = new FormFileDataGrid(this.DataGridCopy(50), this._DataColumn, ScrollPosition);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    this._DataColumn.FileColumn = f.SelectedFileColumn();
                    this._WizardInterface.SetGridHeaders();
                }
                this.initControl();
            }
        }

        private void radioButtonForAll_Click(object sender, EventArgs e)
        {
            if (this.radioButtonForAll.Checked)
            {
                this._DataColumn.TypeOfSource = DataColumn.SourceType.Interface;
                this.initControl();

                // Toni: Preselect first item
                if (this.comboBoxForAll.SelectedItem == null && this.comboBoxForAll.Items.Count > 0)
                    this.comboBoxForAll.SelectedItem = this.comboBoxForAll.Items[0];
                if (this.comboBoxForAll.SelectedItem != null)
                    comboBoxForAll_SelectionChangeCommitted(sender, e);
            }
        }

        private void checkBoxForAll_Click(object sender, EventArgs e)
        {
            if (this.checkBoxForAll.Checked)
                this._DataColumn.Value = "1";
            else
                this._DataColumn.Value = "0";
            this.initControl();
        }

        private void buttonColumnInSourceFile_Click(object sender, EventArgs e)
        {
            int? iScroll = null;
            if (this._WizardInterface.DataGridView().SelectedCells.Count > 0)
                iScroll = this._WizardInterface.DataGridView().SelectedCells[0].ColumnIndex;
            DiversityWorkbench.Import.FormFileDataGrid f = new FormFileDataGrid(this.DataGridCopy(50), this._DataColumn, iScroll);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK && f.SelectedFileColumn() != null)
            {
                int i = (int)f.SelectedFileColumn();
                this._DataColumn.FileColumn = i;
                this._WizardInterface.SetGridHeaders();
                this.initControl();
            }
        }

        private void buttonColumnInSourceFile_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                if (this._DataColumn.IsSelected &&
                    this._DataColumn.TypeOfSource == DataColumn.SourceType.File &&
                    this._DataColumn.FileColumn != null)
                {
                    int Position = (int)this._DataColumn.FileColumn;
                    this._WizardInterface.DataGridView().FirstDisplayedScrollingColumnIndex = Position;
                }
            }
            catch (System.Exception ex) { }
        }

        private void comboBoxForAll_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxForAll.DataSource == null)
            {
                this.comboBoxForAll.DataSource = this._DataColumn.MandatoryList;// dt;
                this.comboBoxForAll.DisplayMember = this._DataColumn.MandatoryListDisplayColumn;
                this.comboBoxForAll.ValueMember = this._DataColumn.MandatoryListValueColumn;
            }
        }

        private void comboBoxInternalRelation_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._DataColumn.ForeignRelationTableAlias = this.comboBoxInternalRelation.SelectedValue.ToString();
            if (this._DataColumn.ForeignRelationTableAlias.Length > 0)
            {
                this._DataColumn.IsSelected = true;
                this._DataColumn.TypeOfSource = DataColumn.SourceType.Database;
            }
            else
            {
                this._DataColumn.IsSelected = false;
                this._DataColumn.TypeOfSource = DataColumn.SourceType.NotDecided;
            }
            this.initControl();
        }

        private void comboBoxInternalRelation_DropDown(object sender, EventArgs e)
        {
            System.Data.DataTable dtSource = this.dtSourceInternalRelation;

            this.comboBoxInternalRelation.DataSource = dtSource;
            this.comboBoxInternalRelation.DisplayMember = "Display";
            this.comboBoxInternalRelation.ValueMember = "TableAlias";
        }

        //private System.Data.DataTable _dtSourceInternalRelation;
        private System.Data.DataTable dtSourceInternalRelation
        {
            get
            {
                System.Data.DataTable dtSource = new System.Data.DataTable();
                System.Data.DataColumn CDisplay = new System.Data.DataColumn("Display", typeof(string));
                System.Data.DataColumn CTableAlias = new System.Data.DataColumn("TableAlias", typeof(string));
                dtSource.Columns.Add(CDisplay);
                dtSource.Columns.Add(CTableAlias);
                System.Data.DataRow R = dtSource.NewRow();
                R[0] = "";
                R[1] = "";
                dtSource.Rows.Add(R);

                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                {
                    if (this._DataColumn.SelectParallelForeignRelationTableAlias)
                    {
                        if (KV.Value.TableName == this._DataColumn.ForeignRelationTable &&
                            DiversityWorkbench.Import.Import.TableListForImport.Contains(KV.Key))
                        {
                            if (DiversityWorkbench.Import.Import.Tables[KV.Key].TableName == this._DataTable.TableName &&
                            DiversityWorkbench.Import.Import.Tables[KV.Key].ParallelPosition >= this._DataTable.ParallelPosition)
                            {
                                continue;
                            }
                            else
                            {
                                System.Data.DataRow RTA = dtSource.NewRow();
                                RTA[0] = KV.Value.GetDisplayText();
                                RTA[1] = KV.Value.TableAlias;
                                dtSource.Rows.Add(RTA);
                            }
                        }
                    }
                    else
                    {
                        if (KV.Value.TableName == this._DataColumn.DataTable.TableName
                            && KV.Value.TableAlias != this._DataColumn.DataTable.TableAlias)
                        {
                            System.Data.DataRow RTA = dtSource.NewRow();
                            RTA[0] = KV.Value.GetDisplayText();
                            RTA[1] = KV.Value.TableAlias;
                            dtSource.Rows.Add(RTA);
                        }
                    }
                }
                return dtSource;
            }
        }

        private void textBoxPrefix_TextChanged(object sender, EventArgs e)
        {
            this._DataColumn.Prefix = this.textBoxPrefix.Text;
        }

        private void textBoxPostfix_TextChanged(object sender, EventArgs e)
        {
            this._DataColumn.Postfix = this.textBoxPostfix.Text;
        }

        private void textBoxForAll_TextChanged(object sender, EventArgs e)
        {
            this._DataColumn.Value = this.textBoxForAll.Text;
            this._WizardInterface.setTableMessage(this._DataTable);
        }

        private void comboBoxForAll_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxForAll.SelectedItem;
            this._DataColumn.Value = R.Row["Value"].ToString();
            this._WizardInterface.setTableMessage(this._DataTable);
        }

        private void dateTimePickerForAll_CloseUp(object sender, EventArgs e)
        {
            //this._DataColumn.Value = "CONVERT(DATETIME, '" + this.dateTimePickerForAll.Value.Year.ToString() + "-" + this.dateTimePickerForAll.Value.Month.ToString() + "-" + this.dateTimePickerForAll.Value.Day.ToString() + " 00:00:00', 102)";
            this._DataColumn.Value = this.dateTimePickerForAll.Value.Year.ToString() + "-"
                + this.dateTimePickerForAll.Value.Month.ToString() + "-"
                + this.dateTimePickerForAll.Value.Day.ToString();
            this.textBoxForAll.Text = this._DataColumn.Value;
        }

        private void textBoxPrefix_DoubleClick(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Prefix", this.textBoxPrefix.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxPrefix.Text = f.EditedText;
        }

        private void textBoxPostfix_DoubleClick(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Postfix", this.textBoxPostfix.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxPostfix.Text = f.EditedText;
        }

        private void textBoxForAll_DoubleClick(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("For all", this.textBoxForAll.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxForAll.Text = f.EditedText;
        }

        private void pictureBoxTableColumn_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.FormColumnInfo f = new FormColumnInfo(this._DataColumn);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                if (this._DataColumn.DataType.ToLower() == "geography")
                {
                    this.textBoxPrefix.Text = f.Prefix();
                    if (this._DataColumn.IsMultiColumn &&
                        this._DataColumn.MultiColumns.Count > 0)
                    {
                        this._DataColumn.MultiColumns.Last().Postfix = f.Postfix();
                        this.initControl();
                    }
                    else
                        this.textBoxPostfix.Text = f.Postfix();
                }
            }
        }

        #endregion

        #region Grid

        /// <summary>
        /// Returns a copy of the datagrid containing the first lines of the original
        /// </summary>
        /// <param name="FristLines">Number of lines starting at the top</param>
        /// <returns></returns>
        private System.Windows.Forms.DataGridView DataGridCopy(int FristLines)
        {
            System.Windows.Forms.DataGridView G = new DataGridView();
            G.ReadOnly = true;
            foreach (System.Windows.Forms.DataGridViewColumn C in this._WizardInterface.DataGridView().Columns)
            {
                System.Windows.Forms.DataGridViewColumn Col = new DataGridViewColumn(C.CellTemplate);
                Col.HeaderText = C.HeaderText;
                G.Columns.Add(Col);
            }
            for (int i = 0; i < this._WizardInterface.DataGridView().Rows.Count && i < FristLines; i++)
            {
                System.Windows.Forms.DataGridViewRow Row = new DataGridViewRow();
                G.Rows.Add(Row);
                for (int ii = 0; ii < this._WizardInterface.DataGridView().Columns.Count; ii++)
                {
                    G.Rows[i].Cells[ii].Value = this._WizardInterface.DataGridView().Rows[i].Cells[ii].Value;
                    //G.Rows[i].Cells[ii].Style = this._WizardInterface.DataGridView().Rows[i].Cells[ii].Style;
                }
            }
            return G;
        }

        #endregion

    }
}
