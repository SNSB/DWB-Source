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
    public partial class UserControlImportDate : UserControl
    {
        #region Parameter

        private DiversityCollection.FormImportWizard _FormImportWizard;
        private DiversityCollection.Import_Column _Import_Column_Day;
        private DiversityCollection.Import_Column _Import_Column_Month;
        private DiversityCollection.Import_Column _Import_Column_Year;
        private DiversityCollection.Import_Column _Import_Column_Supplement;
        private DiversityCollection.Import _Import;
        private string _StepPosition;
        //private bool _ColumnSelectionPending;

        #endregion

        #region construction
        
        public UserControlImportDate()
        {
            InitializeComponent();
        }
        
        #endregion
        
        public void initUserControl(DiversityCollection.Import_Column ImportColumnDay
            , DiversityCollection.Import_Column ImportColumnMonth
            , DiversityCollection.Import_Column ImportColumnYear
            , DiversityCollection.Import_Column ImportColumnSupplement
            , string Position
            , DiversityCollection.Import Import
            , DiversityCollection.FormImportWizard FormImportWizard)
        {
            this._Import_Column_Day = ImportColumnDay;
            this._Import_Column_Day.TypeOfEntry = Import_Column.EntryType.Text;
            this._Import_Column_Day.TypeOfFixing = Import_Column.FixingType.Import;
            this._Import_Column_Day.StepKey = Position;

            this._Import_Column_Month = ImportColumnMonth;
            this._Import_Column_Month.TypeOfEntry = Import_Column.EntryType.Text;
            this._Import_Column_Month.TypeOfFixing = Import_Column.FixingType.Import;
            this._Import_Column_Month.StepKey = Position;

            this._Import_Column_Year = ImportColumnYear;
            this._Import_Column_Year.TypeOfEntry = Import_Column.EntryType.Text;
            this._Import_Column_Year.TypeOfFixing = Import_Column.FixingType.Import;
            this._Import_Column_Year.StepKey = Position;

            this._Import_Column_Supplement = ImportColumnSupplement;
            this._Import_Column_Supplement.TypeOfEntry = Import_Column.EntryType.Text;
            this._Import_Column_Supplement.TypeOfFixing = Import_Column.FixingType.Import;
            this._Import_Column_Supplement.StepKey = Position;


            //System.Collections.Generic.List<string> Formats = new List<string>();
            //Formats.Add("");
            //Formats.Add("Year.Month.Day");
            //Formats.Add("Day.Month.Year");
            //Formats.Add("Month.Day.Year");
            //this._Import_Column_Date.Formats = Formats;

            this._FormImportWizard = FormImportWizard;
            this._Import = Import;
            this._StepPosition = Position;

            this.userControlImport_ColumnDay.initUserControl(this._Import_Column_Day, this._Import);
            this.userControlImport_ColumnMonth.initUserControl(this._Import_Column_Month, this._Import);
            this.userControlImport_ColumnYear.initUserControl(this._Import_Column_Year, this._Import);
            this.userControlImport_ColumnSupplement.initUserControl(this._Import_Column_Supplement, this._Import);


            //this.setControls();
        }

        //private void checkBoxNoDate_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.checkBoxNoDate.Checked)
        //    {
        //        this.checkBoxCollectionDate.Checked = false;
        //        this.checkBoxSameDate.Checked = false;
        //    }
        //    this.setControls();
        //}

        //private void checkBoxSameDate_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.checkBoxSameDate.Checked)
        //    {
        //        this.checkBoxCollectionDate.Checked = false;
        //        this.checkBoxNoDate.Checked = false;
        //    }
        //    this.setControls();
        //}

        //private void checkBoxCollectionDate_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (this.checkBoxCollectionDate.Checked)
        //    {
        //        this.checkBoxNoDate.Checked = false;
        //        this.checkBoxSameDate.Checked = false;
        //    }
        //    this.setControls();
        //}

        //private void setControls()
        //{
        //    this.radioButtonCollectionDay.Visible = this.checkBoxCollectionDate.Checked;
        //    this.radioButtonCollectionMonth.Visible = this.checkBoxCollectionDate.Checked;
        //    this.radioButtonCollectionYear.Visible = this.checkBoxCollectionDate.Checked;
        //    this.radioButtonDate.Visible = this.checkBoxCollectionDate.Checked;
        //    this.comboBoxCollectionDateAnalysisType.Visible = this.checkBoxCollectionDate.Checked;
        //    this.labelDateFormat.Visible = this.checkBoxCollectionDate.Checked;
        //    this.dateTimePicker.Visible = this.checkBoxSameDate.Checked;
        //    this.textBoxCollectionDate.Visible = this.checkBoxSameDate.Checked;
        //}

        //private void radioButtonCollectionDay_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.setColumnInSourceFile(null);
        //    if (this.radioButtonCollectionDay.Checked)
        //    {
        //        this._FormImportWizard.setCurrentImportColumn(this._Import_Column_Day);
        //    }
        //    else
        //    {
        //        this._FormImportWizard.setCurrentImportColumn();
        //        this._Import.RemoveColumn(this._Import_Column_Day.Table, this._Import_Column_Day.TableAlias, this._Import_Column_Day.Column);
        //    }
        //    DiversityCollection.Import.ImportSteps[this._StepPosition].setStepError(this.Error());
        //}

        //private void radioButtonCollectionMonth_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.setColumnInSourceFile(null);
        //    if (this.radioButtonCollectionMonth.Checked)
        //    {
        //        this._FormImportWizard.setCurrentImportColumn(this._Import_Column_Day);
        //    }
        //    else
        //    {
        //        this._FormImportWizard.setCurrentImportColumn();
        //        this._Import.RemoveColumn(this._Import_Column_Day.Table, this._Import_Column_Day.TableAlias, this._Import_Column_Day.Column);
        //    }
        //    DiversityCollection.Import.ImportSteps[this._StepPosition].setStepError(this.Error());
        //}

        //private void radioButtonCollectionYear_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.setColumnInSourceFile(null);
        //    if (this.radioButtonCollectionYear.Checked)
        //    {
        //        this._FormImportWizard.setCurrentImportColumn(this._Import_Column_Day);
        //    }
        //    else
        //    {
        //        this._FormImportWizard.setCurrentImportColumn();
        //        this._Import.RemoveColumn(this._Import_Column_Day.Table, this._Import_Column_Day.TableAlias, this._Import_Column_Day.Column);
        //    }
        //    DiversityCollection.Import.ImportSteps[this._StepPosition].setStepError(this.Error());
        //}

        //private void radioButtonDate_CheckedChanged(object sender, EventArgs e)
        //{
        //    this.setColumnInSourceFile(null);
        //    if (this.radioButtonDate.Checked)
        //    {
        //        this._FormImportWizard.setCurrentImportColumn(this._Import_Column_Day);
        //    }
        //    else
        //    {
        //        this._FormImportWizard.setCurrentImportColumn();
        //        this._Import.RemoveColumn(this._Import_Column_Day.Table, this._Import_Column_Day.TableAlias, this._Import_Column_Day.Column);
        //    }
        //    DiversityCollection.Import.ImportSteps[this._StepPosition].setStepError(this.Error());
        //}

        //public void setColumnInSourceFile(int? ColumnInSourceFile)
        //{
        //    this.radioButtonCollectionDay.BackColor = System.Drawing.SystemColors.ControlLightLight;
        //    this.radioButtonCollectionMonth.BackColor = System.Drawing.SystemColors.ControlLightLight;
        //    this.radioButtonCollectionYear.BackColor = System.Drawing.SystemColors.ControlLightLight;
        //    this.radioButtonDate.BackColor = System.Drawing.SystemColors.ControlLightLight;
        //    if (this.radioButtonCollectionDay.Checked)
        //    {
        //        if (this._Import_Column_Day.ColumnInSourceFile == null)
        //            this.radioButtonCollectionDay.BackColor = System.Drawing.Color.Yellow;
        //    }
        //    else if (this.radioButtonCollectionMonth.Checked)
        //    {
        //        if (this._Import_Column_Month.ColumnInSourceFile == null)
        //            this.radioButtonCollectionMonth.BackColor = System.Drawing.Color.Yellow;
        //    }
        //    else if (this.radioButtonCollectionYear.Checked)
        //    {
        //        if (this._Import_Column_Year.ColumnInSourceFile == null)
        //            this.radioButtonCollectionYear.BackColor = System.Drawing.Color.Yellow;
        //    }
        //    else if (this.radioButtonDate.Checked)
        //    {
        //        if (this._Import_Column_Date.ColumnInSourceFile == null)
        //            this.radioButtonDate.BackColor = System.Drawing.Color.Yellow;
        //    }
        //}

        //public int? getColumnInSourceFile()
        //{
        //    if (this.radioButtonCollectionDay.Checked)
        //    {
        //        return this._Import_Column_Day.ColumnInSourceFile;
        //    }
        //    else if (this.radioButtonCollectionMonth.Checked)
        //    {
        //        return this._Import_Column_Month.ColumnInSourceFile;
        //    }
        //    else if (this.radioButtonCollectionYear.Checked)
        //    {
        //        return this._Import_Column_Year.ColumnInSourceFile;
        //    }
        //    else if (this.radioButtonDate.Checked)
        //    {
        //        return this._Import_Column_Date.ColumnInSourceFile;
        //    }
        //    else return null;
        //}

        //public string Error()
        //{
        //    //get
        //    //{
        //    string E = "";
        //    string Column = "";
        //    if (this.radioButtonCollectionDay.Checked)
        //    {
        //        Column = this._Import_Column_Day.Column;
        //    }
        //    else if (this.radioButtonCollectionMonth.Checked)
        //    {
        //        Column = this._Import_Column_Month.Column;
        //    }
        //    else if (this.radioButtonCollectionYear.Checked)
        //    {
        //        Column = this._Import_Column_Year.Column;
        //    }
        //    else if (this.radioButtonDate.Checked)
        //    {
        //        Column = this._Import_Column_Date.Column;
        //    }
        //    string Result = "";
        //    foreach (System.Char C in Column)
        //    {
        //        if (C.ToString().ToLower() != C.ToString() && Result.Length > 0)
        //            Result += " ";
        //        if (Result.Length == 0) Result += C.ToString().ToUpper();
        //        else Result += C.ToString().ToLower();
        //    }
        //    Result = Result.ToLower();
        //    if (this.checkBoxCollectionDate.Checked)
        //    {
        //        if (this.radioButtonCollectionDay.Checked)
        //        {
        //            if (!this._Import.ImportContainsColumn(this._Import_Column_Day.Table, this._Import_Column_Day.Column))
        //                E = "No " + Result + " is given";
        //        }
        //        else if (this.radioButtonCollectionMonth.Checked)
        //        {
        //            if (!this._Import.ImportContainsColumn(this._Import_Column_Month.Table, this._Import_Column_Month.Column))
        //                E = "No " + Result + " is given";
        //        }
        //        else if (this.radioButtonCollectionYear.Checked)
        //        {
        //            if (!this._Import.ImportContainsColumn(this._Import_Column_Year.Table, this._Import_Column_Year.Column))
        //                E = "No " + Result + " is given";
        //        }
        //        else if (this.radioButtonDate.Checked)
        //        {
        //            if (!this._Import.ImportContainsColumn(this._Import_Column_Date.Table, this._Import_Column_Date.Column))
        //                E = "No " + Result + " is given";
        //        }
        //    }
        //    return E;
        //    //}
        //}

        //private void dateTimePicker_CloseUp(object sender, EventArgs e)
        //{
        //    this.textBoxCollectionDate.Text = this.dateTimePicker.Value.ToString();
        //    if (DiversityCollection.Import.ImportSteps.ContainsKey(DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date)))
        //        DiversityCollection.Import.ImportSteps[DiversityCollection.Import.getImportStepKey(Import.ImportStepEvent.Date)].setStepError(this.Error());

        //}

        public void setInterface()
        {
        }

        private void UserControlImportDate_Load(object sender, EventArgs e)
        {

        }



    }
}
