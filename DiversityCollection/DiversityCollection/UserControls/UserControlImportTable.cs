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
    /// Form to decide what is to do if a conflict occurs during the import
    /// Possible scenarios:
    /// INSERT 
    ///     Problem: a dataset where the PK allready exists: 
    ///         CheckError:
    ///             Ignore
    ///             Update
    ///         if NeededAction = NoAction: disable Update - just inform and no entry in the Report
    ///         Automatic:
    ///             Error message, No action
    /// UPDATE 
    ///     Problem: several matching datasets exist:
    ///         CheckError:
    ///             Ignore
    ///             Insert as new dataset (for tables with identity column)
    ///             Update of selected dataset (return Where Clause)
    ///         Automatic:
    ///             Choose first
    ///             No action
    /// MERGE
    ///     Problem: there are more than one dataset that could be updated
    ///         CheckError:
    ///             Ignore
    ///             Update: select the dataset that should be updated (see UPDATE - Update)
    ///             Insert: if possible, e.g. for tables with identity column import as new dataset
    ///         Automatic:
    ///             Choose first
    ///             No action
    /// </summary>
    public partial class UserControlImportTable : UserControl
    {
        private DiversityCollection.Import_Table _ImportTable;
        public UserControlImportTable(DiversityCollection.Import_Table ImportTable)
        {
            InitializeComponent();
            this.helpProvider.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm";
            this._ImportTable = ImportTable;
            this.labelTableName.Text = this._ImportTable.TableAlias;
            if (this._ImportTable.TableAlias != this._ImportTable.TableName &&
                !this._ImportTable.TableAlias.StartsWith(this._ImportTable.TableName))
                this.labelTableName.Text += " (" + this._ImportTable.TableName + ")";
            //this.radioButtonInsert.Checked = true;
            if (DiversityCollection.Import.AttachmentKeyImportColumn != null &&
                DiversityCollection.Import.AttachmentKeyImportColumn.TableAlias == this._ImportTable.TableAlias)
            {
                this.BackColor = FormImportWizard.ColorForAttachment;
                if (!DiversityCollection.Import.ImportTablesDataTreatment.ContainsKey(this._ImportTable.TableAlias))
                    DiversityCollection.Import.ImportTablesDataTreatment.Add(this._ImportTable.TableAlias, this._ImportTable);
                if (this._ImportTable.ChildParentColumn.Length > 0)
                {
                    string KeyForInsert = DiversityCollection.Import.AttachmentKeyImportColumn.Key.Substring(0, DiversityCollection.Import.AttachmentKeyImportColumn.Key.Length - 1) + "1";
                    if (DiversityCollection.Import.ImportColumns.ContainsKey(KeyForInsert) && DiversityCollection.Import.ImportColumns[KeyForInsert].ColumnInSourceFile == DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile)
                    {
                        this.radioButtonInsert.Checked = false;
                        this.radioButtonUpdate.Checked = true;
                        this.radioButtonUpdate.Enabled = false;
                        this.radioButtonInsert.Enabled = false;
                    }
                    else if (DiversityCollection.Import.ImportColumns.ContainsKey(KeyForInsert) && DiversityCollection.Import.ImportColumns[KeyForInsert].ColumnInSourceFile != DiversityCollection.Import.AttachmentKeyImportColumn.ColumnInSourceFile)
                    {
                        this.radioButtonInsert.Checked = true;
                        this.radioButtonUpdate.Checked = false;
                        this.radioButtonUpdate.Enabled = false;
                        this.radioButtonInsert.Enabled = false;
                        DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Insert;
                        DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Insert;
                    }
                    else
                    {
                        this.radioButtonInsert.Checked = false;
                        this.radioButtonUpdate.Checked = true;
                        this.radioButtonUpdate.Enabled = true;
                        this.radioButtonInsert.Enabled = true;
                        DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Update;
                        DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Update;
                    }
                }
                else
                {
                    this.radioButtonInsert.Checked = false;
                    this.radioButtonUpdate.Checked = true;
                    this.radioButtonUpdate.Enabled = false;
                    this.radioButtonInsert.Enabled = false;
                    DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Update;
                    DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Update;
                }
                this.radioButtonMerge.Enabled = false;
                //DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Update;
                //DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Update;
            }
            //else
            //{
            //    if (DiversityCollection.Import.ImportTablesDataTreatment.Count % 2 == 1)
            //        this.BackColor = System.Drawing.SystemColors.Control;
            //}
            switch (this._ImportTable.TreatmentOfData)
            {
                case Import_Table.DataTreatment.Update:
                    this.radioButtonUpdate.Checked = true;
                    break;
                case Import_Table.DataTreatment.Merge:
                    this.radioButtonMerge.Checked = true;
                    break;
                case Import_Table.DataTreatment.Insert:
                    this.radioButtonInsert.Checked = true;
                    break;
            }
            this.setErrorHandlingAutomaticOptions();
            switch(this._ImportTable.TreatmentOfErrors)
            {
                case Import_Table.ErrorTreatment.Manual:
                    this.radioButtonErrorHandlingManual.Checked = true;
                    break;
                case Import_Table.ErrorTreatment.AutoNone:
                    this.radioButtonErrorHandlingAutomaticNone.Checked = true;
                    break;
                case Import_Table.ErrorTreatment.AutoLast:
                    if (this.radioButtonErrorHandlingAutomaticLast.Enabled)
                        this.radioButtonErrorHandlingAutomaticLast.Checked = true;
                    else 
                        this.radioButtonErrorHandlingAutomaticFirst.Checked = true;
                    break;
                case Import_Table.ErrorTreatment.AutoFirst:
                    this.radioButtonErrorHandlingAutomaticFirst.Checked = true;
                    break;
            }
        }

        private void radioButtonMerge_Click(object sender, EventArgs e)
        {
            this.setValue();
            this.setErrorHandlingAutomaticOptions();
        }

        private void radioButtonInsert_Click(object sender, EventArgs e)
        {
            this.setValue();
            this.setErrorHandlingAutomaticOptions();
        }

        private void radioButtonUpdate_Click(object sender, EventArgs e)
        {
            this.setValue();
            this.setErrorHandlingAutomaticOptions();
        }

        private void setValue()
        {
            try
            {
                if (this.radioButtonInsert.Checked)
                    DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Insert;
                else if (this.radioButtonUpdate.Checked)
                    DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Update;
                else
                    DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfData = Import_Table.DataTreatment.Merge;
                //if (DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfData != this._ImportTable.TreatmentOfData)
                //{
                //    DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfData = this._ImportTable.TreatmentOfData;
                //}
            }
            catch (System.Exception ex) { }
        }

        private void radioButtonErrorHandlingManual_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.Manual;
                if (DiversityCollection.Import.ImportTables.ContainsKey(this._ImportTable.TableAlias))
                    DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.Manual;
            }
            catch (System.Exception ex)
            {
            }
        }

        private void radioButtonErrorHandlingAutomaticNone_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoNone;
                if (DiversityCollection.Import.ImportTables.ContainsKey(this._ImportTable.TableAlias))
                    DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoNone;
            }
            catch (System.Exception ex)
            {
            }
        }

        private void radioButtonErrorHandlingAutomaticLast_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoLast;
                if (DiversityCollection.Import.ImportTables.ContainsKey(this._ImportTable.TableAlias))
                    DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoLast;
            }
            catch (System.Exception ex)
            {
            }
        }

        private void radioButtonErrorHandlingAutomaticFirst_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoFirst;
                if (DiversityCollection.Import.ImportTables.ContainsKey(this._ImportTable.TableAlias))
                    DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoFirst;
            }
            catch (System.Exception ex)
            {
            }
        }

        private void setErrorHandlingAutomaticOptions()
        {
            if (this.radioButtonInsert.Checked)
            {
                if (this.radioButtonErrorHandlingAutomaticLast.Checked ||
                    this.radioButtonErrorHandlingAutomaticFirst.Checked)
                {
                    this.radioButtonErrorHandlingAutomaticNone.Checked = true;
                    DiversityCollection.Import.ImportTablesDataTreatment[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoNone;
                    if (DiversityCollection.Import.ImportTables.ContainsKey(this._ImportTable.TableAlias))
                        DiversityCollection.Import.ImportTables[this._ImportTable.TableAlias].TreatmentOfErrors = Import_Table.ErrorTreatment.AutoNone  ;
                }
                this.radioButtonErrorHandlingAutomaticFirst.Checked = false;
                this.radioButtonErrorHandlingAutomaticFirst.Enabled = false;
                this.radioButtonErrorHandlingAutomaticLast.Checked = false;
                this.radioButtonErrorHandlingAutomaticLast.Enabled = false;
                this.labelErrorHandlingAutomaticFirst.Enabled = false;
                this.labelErrorHandlingAutomaticLast.Enabled = false;
            }
            else
            {
                bool CanChooseLastEntry = false;
                try
                {
                    System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Column> DD = this._ImportTable.FindPKColumnsWithMissingForeignRelations();
                    foreach (string s in this._ImportTable.PrimaryKeyColumnList)
                    {
                        if (!DD.ContainsKey(s) && 
                            this._ImportTable.ColumnValueDictionary().ContainsKey(s) &&
                            this._ImportTable.ColumnValueDictionary()[s].Length == 0)
                            CanChooseLastEntry = true;
                        if (this._ImportTable.IdentityColumn() == s)
                            CanChooseLastEntry = true;
                    }
                }
                catch (System.Exception ex) { }
                this.radioButtonErrorHandlingAutomaticLast.Enabled = CanChooseLastEntry;
                this.labelErrorHandlingAutomaticLast.Enabled = CanChooseLastEntry;
                if (this.radioButtonErrorHandlingAutomaticLast.Checked && !CanChooseLastEntry)
                    this.radioButtonErrorHandlingAutomaticFirst.Checked = true;
                this.radioButtonErrorHandlingAutomaticFirst.Enabled = true;
                this.labelErrorHandlingAutomaticFirst.Enabled = true;
            }
        }
    }
}
