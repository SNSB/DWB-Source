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
    public partial class UserControlTransformation : UserControl, iTransformationInterface
    {

        #region Parameter

        private DiversityWorkbench.Import.Transformation _Transformation;

        public DiversityWorkbench.Import.Transformation Transformation
        {
            get { return _Transformation; }
            //set { _Transformation = value; }
        }

        private int _PositionInFile;

        private DiversityWorkbench.Import.iWizardInterface _iWizardInterface;

        #endregion

        #region Construction and control

        public UserControlTransformation(DiversityWorkbench.Import.Transformation Transformation, int PositionInFile, DiversityWorkbench.Import.iWizardInterface iWizardInterface, string HelpNameSpace)
        {
            InitializeComponent();
            this._Transformation = Transformation;
            this._PositionInFile = PositionInFile;
            this._iWizardInterface = iWizardInterface;
            this.initControl();
            this.helpProvider.HelpNamespace = HelpNameSpace;
        }

        public void initControl()
        {
            try
            {
                switch (this._Transformation.TypeOfTransformation)
                {
                    case Transformation.TransformationType.Split:
                        this.splitContainerSpTr_RxRe.Panel2Collapsed = true;
                        this.splitContainerSpTr.Panel2Collapsed = true;
                        this.splitContainerSpCo.Panel1Collapsed = false;
                        this.splitContainerSpCo.Panel2Collapsed = true;
                        this.numericUpDownPosition.Value = this._Transformation.SplitterPosition;
                        if (this._Transformation.ReverseSequence)
                            this.buttonReverseSequence.Image = this.imageListReverseSequence.Images[1];
                        else
                            this.buttonReverseSequence.Image = this.imageListReverseSequence.Images[0];
                        this.listBoxSplitters.Items.Clear();
                        if (this._Transformation.SplitterList != null)
                        {
                            foreach (string s in this._Transformation.SplitterList)
                                this.listBoxSplitters.Items.Add(s);
                        }
                        break;
                    case Transformation.TransformationType.Translation:
                        this.splitContainerSpTr_RxRe.Panel2Collapsed = true;
                        this.splitContainerSpTr.Panel1Collapsed = true;
                        this.InitTranslations();
                        break;
                    case Transformation.TransformationType.RegularExpression:
                        this.splitContainerSpTr_RxRe.Panel1Collapsed = true;
                        this.splitContainerRxRe.Panel2Collapsed = true;
                        this.splitContainerReFi.Panel1Collapsed = false;
                        this.splitContainerReFi.Panel2Collapsed = true;
                        if (this._Transformation.RegularExpression == null)
                            this._Transformation.RegularExpression = "";
                        this.textBoxRegex.Text = this._Transformation.RegularExpression;
                        if (this._Transformation.RegularExpressionReplacement == null)
                            this._Transformation.RegularExpressionReplacement = "";
                        this.textBoxRegExReplace.Text = this._Transformation.RegularExpressionReplacement;
                        break;
                    case Transformation.TransformationType.Replacement:
                        this.splitContainerSpTr_RxRe.Panel1Collapsed = true;
                        this.splitContainerRxRe.Panel1Collapsed = true;
                        this.splitContainerReCal.Panel2Collapsed = true;
                        if (this._Transformation.Replace == null)
                            this._Transformation.Replace = "";
                        if (this._Transformation.ReplaceWith == null)
                            this._Transformation.ReplaceWith = "";
                        this.textBoxReplace.Text = this._Transformation.Replace;
                        this.textBoxReplaceWith.Text = this._Transformation.ReplaceWith;
                        break;
                    case DiversityWorkbench.Import.Transformation.TransformationType.Calculation:
                        this.splitContainerSpTr_RxRe.Panel1Collapsed = true;
                        this.splitContainerRxRe.Panel1Collapsed = true;
                        this.splitContainerReCal.Panel1Collapsed = true;

                        this.textBoxCalculationFactor.Text = this._Transformation.CalculationFactor;

                        if (this._Transformation.CalulationOperator != null)
                        {
                            if (this.comboBoxCalculationOperator.Items.Count == 0)
                            {
                                foreach (string s in this._Transformation.CalculationOperators)
                                    this.comboBoxCalculationOperator.Items.Add(s);
                            }
                            this.comboBoxCalculationOperator.Text = this._Transformation.CalulationOperator;
                        }

                        // Condition
                        if (this._Transformation.CalculationConditionValue != null &&
                            this._Transformation.CalculationConditionOperator != null &&
                            this._Transformation.CalculationConditionValue.Length > 0 &&
                            this._Transformation.CalculationConditionOperator.Length > 0)
                        {
                            this.textBoxCalculationCondition.Text = this._Transformation.CalculationConditionValue;
                            if (this.comboBoxCalculationCondition.Items.Count == 0)
                            {
                                foreach (string s in this._Transformation.CalculationConditionOperators)
                                    this.comboBoxCalculationCondition.Items.Add(s);
                            }
                            this.comboBoxCalculationCondition.Text = this._Transformation.CalculationConditionOperator;
                        }
                        // Apply on data
                        if (this._Transformation.CalculationApplyOnData && this._Transformation.CalculationApplyOnDataOperator != null && this._Transformation.CalculationApplyOnDataOperator.Length > 0)
                        {
                            this.checkBoxCalculationApplyOnData.Checked = this._Transformation.CalculationApplyOnData;
                            if (this.comboBoxCalculationApplyOnDataOperator.Items.Count == 0)
                            {
                                foreach (string s in this._Transformation.CalculationOperators)
                                    this.comboBoxCalculationApplyOnDataOperator.Items.Add(s);
                            }
                            this.comboBoxCalculationApplyOnDataOperator.Text = this._Transformation.CalculationApplyOnDataOperator;
                        }
                        break;
                    case DiversityWorkbench.Import.Transformation.TransformationType.Filter:
                        this.splitContainerSpTr_RxRe.Panel1Collapsed = true;
                        this.splitContainerRxRe.Panel2Collapsed = true;
                        this.splitContainerReFi.Panel1Collapsed = true;
                        this.splitContainerReFi.Panel2Collapsed = false;
                        if (this.comboBoxFilterOperator.Items.Count == 0)
                        {
                            this.comboBoxFilterOperator.Items.Add("And");
                            this.comboBoxFilterOperator.Items.Add("Or");
                            if (this._Transformation.FilterConditionsOperator == null)
                            {
                                this._Transformation.FilterConditionsOperator = DiversityWorkbench.Import.Transformation.FilterConditionsOperators.And;
                                this.comboBoxFilterOperator.Text = "And";
                            }
                            else
                                this.comboBoxFilterOperator.Text = this._Transformation.FilterConditionsOperator.ToString();
                        }
                        this.radioButtonFilterFixedValue.Checked = this._Transformation.FilterUseFixedValue;
                        if (this._Transformation.FilterUseFixedValue)
                        {
                            this.labelFilterHeader.Text = "Use fixed value only";
                            this.textBoxFilterFixedValue.Visible = true;
                            this.textBoxFilterFixedValue.Text = this._Transformation.FilterFixedValue;
                        }
                        else
                        {
                            this.labelFilterHeader.Text = "Use content of file only";
                            this.textBoxFilterFixedValue.Visible = false;
                        }
                        if (this._Transformation.FilterConditions.Count == 0)
                        {
                            DiversityWorkbench.Import.TransformationFilter TF = new TransformationFilter(this._Transformation);
                            this._Transformation.FilterConditions.Add(TF);
                        }
                        foreach (System.Windows.Forms.Control C in this.panelFilterConditions.Controls)
                            C.Dispose();
                        this.panelFilterConditions.Controls.Clear();
                        foreach (DiversityWorkbench.Import.TransformationFilter TF in this._Transformation.FilterConditions)
                        {
                            DiversityWorkbench.Import.UserControlTransformationFilter U = new UserControlTransformationFilter(TF, this);
                            U.Dock = DockStyle.Top;
                            this.panelFilterConditions.Controls.Add(U);
                            U.BringToFront();
                        }
                        break;
                    case Transformation.TransformationType.Color:
                        this.splitContainerSpTr_RxRe.Panel2Collapsed = true;
                        this.splitContainerSpTr.Panel2Collapsed = true;
                        this.splitContainerSpCo.Panel1Collapsed = true;
                        this.splitContainerSpCo.Panel2Collapsed = false;
                        this.initColorComboBox(this.comboBoxColorFrom);
                        for (int i = 0; i < this.comboBoxColorFrom.Items.Count; i++)
                        {
                            if (this.ColorFormats[this.comboBoxColorFrom.Items[i].ToString()] == this._Transformation.ColorFrom)
                            {
                                this.comboBoxColorFrom.SelectedIndex = i;
                                break;
                            }
                        }
                        this.initColorComboBox(this.comboBoxColorTo);
                        for (int i = 0; i < this.comboBoxColorTo.Items.Count; i++)
                        {
                            if (this.ColorFormats[this.comboBoxColorTo.Items[i].ToString()] == this._Transformation.ColorInto)
                            {
                                this.comboBoxColorTo.SelectedIndex = i;
                                break;
                            }
                        }
                        break;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Splitting

        private void toolStripButtonAddSplitter_Click(object sender, EventArgs e)
        {
            string Splitter = "";
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Please enter the splitter", "", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                Splitter = f.String;
            else return;
            this._Transformation.SplitterList.Add(Splitter);
            this.initControl();
        }

        private void toolStripButtonRemoveSplitter_Click(object sender, EventArgs e)
        {
            if (this.listBoxSplitters.SelectedIndex == -1 || this.listBoxSplitters.Items.Count == 0)
                return;
            string Splitter = this.listBoxSplitters.SelectedItem.ToString();
            this._Transformation.SplitterList.Remove(Splitter);
            this.initControl();
        }

        private void numericUpDownPosition_ValueChanged(object sender, EventArgs e)
        {
            this._Transformation.SplitterPosition = (int)this.numericUpDownPosition.Value;
        }

        private void buttonReverseSequence_Click(object sender, EventArgs e)
        {
            this._Transformation.ReverseSequence = !this._Transformation.ReverseSequence;
            this.initControl();
        }

        #endregion

        #region Regular expression

        private void textBoxRegex_TextChanged(object sender, EventArgs e)
        {
            this._Transformation.RegularExpression = this.textBoxRegex.Text;
        }

        private void textBoxRegExReplace_TextChanged(object sender, EventArgs e)
        {
            this._Transformation.RegularExpressionReplacement = this.textBoxRegExReplace.Text;
        }

        #endregion

        #region Replace

        private void textBoxReplace_TextChanged(object sender, EventArgs e)
        {
            this._Transformation.Replace = this.textBoxReplace.Text;
        }

        private void textBoxReplaceWith_TextChanged(object sender, EventArgs e)
        {
            this._Transformation.ReplaceWith = this.textBoxReplaceWith.Text;
        }

        #endregion

        #region Translation

        private void toolStripButtonTranslationRequery_Click(object sender, EventArgs e)
        {
            this.GetTranslationList();
        }

        private void toolStripButtonTranslationAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New translation", "Please enter the new text that should be translated", "");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK
                && f.String.Length > 0
                && !this._Transformation.TranslationDictionary.ContainsKey(f.String))
            {
                this._Transformation.TranslationDictionary.Add(f.String, "");
            }
            this.ShowTranslations();
        }

        private void toolStripButtonTranslationRemove_Click(object sender, EventArgs e)
        {
            string ToRemove = this.dataGridViewTranslation.Rows[this.dataGridViewTranslation.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
            this._Transformation.TranslationDictionary.Remove(ToRemove);
            this.ShowTranslations();
        }

        private void dataGridViewTranslation_Leave(object sender, EventArgs e)
        {
            this.SaveTranslations();
        }

        private void toolStripButtonTranslationClear_Click(object sender, EventArgs e)
        {
            this._Transformation.TranslationDictionary.Clear();
            this.ShowTranslations();
        }

        private void toolStripButtonTranslationSave_Click(object sender, EventArgs e)
        {
            this.SaveTranslations();
            if (this._Transformation.TranslationDictionary.Count > 0)
            {
                if (MessageBox.Show("Save translation table to file?", "Save to file", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        string path = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule() + "\\Import\\TranslationTable_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                        System.IO.FileInfo fi = new System.IO.FileInfo(path + ".txt");
                        if (fi.Exists)
                        {
                            int i = 1;
                            do
                            {
                                fi = new System.IO.FileInfo(path + "(" + (++i).ToString() + ").txt");
                            } while (fi.Exists);
                        }
                        // Save file
                        string csv = string.Join(Environment.NewLine, this._Transformation.TranslationDictionary.Select(d => $"{d.Key}\t{d.Value}"));
                        System.IO.File.WriteAllText(fi.FullName, csv);
                    }
                    catch (Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
            }
        }

        private void SaveTranslations()
        {
            this.dataGridViewTranslation.EndEdit();
            foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewTranslation.Rows)
            {
                if (R.Cells[1].Value != null)
                    this._Transformation.TranslationDictionary[R.Cells[0].Value.ToString()] = R.Cells[1].Value.ToString();
                else
                {
                    string Position = R.Cells[0].Value.ToString();
                    this._Transformation.TranslationDictionary.Remove(Position);
                }
            }
        }

        /// <summary>
        /// Building the structure of the gridview to show and edit the translations
        /// and show them if any are present
        /// </summary>
        private void InitTranslations()
        {
            if (this.dataGridViewTranslation.ColumnCount == 0)
            {
                System.Windows.Forms.DataGridViewTextBoxColumn Tsource = new System.Windows.Forms.DataGridViewTextBoxColumn();
                Tsource.HeaderText = "Source";
                if (this.Transformation.iDataColumn.getMandatoryList() != null)
                {
                    System.Windows.Forms.DataGridViewComboBoxColumn Ttranslation = new System.Windows.Forms.DataGridViewComboBoxColumn();
                    Ttranslation.HeaderText = "Translation";
                    this.dataGridViewTranslation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    Tsource,
                    Ttranslation});
                    Ttranslation.DataSource = this._Transformation.iDataColumn.getMandatoryList();
                    Ttranslation.DisplayMember = this._Transformation.iDataColumn.getMandatoryListDisplayColumn();
                    Ttranslation.ValueMember = this._Transformation.iDataColumn.getMandatoryListValueColumn();
                }
                else
                {
                    System.Windows.Forms.DataGridViewTextBoxColumn Ttranslation = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    Ttranslation.HeaderText = "Translation";
                    this.dataGridViewTranslation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    Tsource,
                    Ttranslation});
                }
                this.dataGridViewTranslation.Columns[0].ReadOnly = true;
                this.dataGridViewTranslation.AllowUserToAddRows = false;
                this.dataGridViewTranslation.AllowUserToDeleteRows = false;
                this.dataGridViewTranslation.AllowUserToOrderColumns = false;
                this.dataGridViewTranslation.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridViewTranslation.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridViewTranslation.RowHeadersVisible = false;
                this.dataGridViewTranslation.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            if (this.dataGridViewTranslationSourceTable.ColumnCount == 0)
            {
                System.Windows.Forms.DataGridViewTextBoxColumn Tsource = new System.Windows.Forms.DataGridViewTextBoxColumn();
                Tsource.HeaderText = "Source";
                //if (this.Transformation.iDataColumn.getMandatoryList() != null)
                //{
                //    System.Windows.Forms.DataGridViewComboBoxColumn Ttranslation = new System.Windows.Forms.DataGridViewComboBoxColumn();
                //    Ttranslation.HeaderText = "Translation";
                //    this.dataGridViewTranslation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                //    Tsource,
                //    Ttranslation});
                //    Ttranslation.DataSource = this._Transformation.iDataColumn.getMandatoryList();
                //    Ttranslation.DisplayMember = this._Transformation.iDataColumn.getMandatoryListDisplayColumn();
                //    Ttranslation.ValueMember = this._Transformation.iDataColumn.getMandatoryListValueColumn();
                //}
                //else
                //{
                System.Windows.Forms.DataGridViewTextBoxColumn Ttranslation = new System.Windows.Forms.DataGridViewTextBoxColumn();
                Ttranslation.HeaderText = "Translation";
                this.dataGridViewTranslationSourceTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    Tsource,
                    Ttranslation});
                //}
                this.dataGridViewTranslationSourceTable.ReadOnly = true;
                this.dataGridViewTranslationSourceTable.AllowUserToAddRows = false;
                this.dataGridViewTranslationSourceTable.AllowUserToDeleteRows = false;
                this.dataGridViewTranslationSourceTable.AllowUserToOrderColumns = false;
                this.dataGridViewTranslationSourceTable.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridViewTranslationSourceTable.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridViewTranslationSourceTable.RowHeadersVisible = false;
                this.dataGridViewTranslationSourceTable.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            this.ShowTranslations();
        }

        /// <summary>
        /// Get a list of all texts in the data that can be translated and are so far missing in the translation dictionary
        /// </summary>
        private void GetTranslationList()
        {
            for (int i = DiversityWorkbench.Import.Import.StartLine - 1; i < DiversityWorkbench.Import.Import.EndLine; i++)
            {
                if (this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value != null
                                    && this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString().Length > 0)
                {
                    string Result = this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString();
                    System.Collections.Generic.Dictionary<int, string> DD = new Dictionary<int, string>();
                    Result = DiversityWorkbench.Import.DataColumn.TransformedValue(Result, this._Transformation.iDataColumn.TransformationList(), DD);
                    if (!this._Transformation.TranslationDictionary.ContainsKey(Result))
                        this._Transformation.TranslationDictionary.Add(Result, "");

                    //string NewText = this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString();
                    //if (!this._Transformation.TranslationDictionary.ContainsKey(NewText))
                    //    this._Transformation.TranslationDictionary.Add(NewText, "");
                }
            }
            this.ShowTranslations();
        }

        /// <summary>
        /// Show the current translations
        /// </summary>
        private void ShowTranslations()
        {
            this.dataGridViewTranslation.Rows.Clear();
            if (this._Transformation.TranslationDictionary != null &&
                this._Transformation.TranslationDictionary.Count > 0)
            {
                this.dataGridViewTranslation.Rows.Add(this._Transformation.TranslationDictionary.Count);
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Transformation.TranslationDictionary)
                {
                    this.dataGridViewTranslation.Rows[i].Cells[0].Value = KV.Key;
                    this.dataGridViewTranslation.Rows[i].Cells[1].Value = KV.Value;
                    i++;
                }
            }
            this.dataGridViewTranslationSourceTable.Rows.Clear();
            if (this._Transformation.TranslationFromColumn != null &&
                this._Transformation.TranslationFromColumn.Length > 0 &&
                this._Transformation.TranslationIntoColumn.Length > 0 &&
                this._Transformation.TranslationSourceTable.Length > 0)
            {
                this.tableLayoutPanelTranslationSourceTable.Visible = true;
                this.labelTranslationSourceTable.Text = "Table  [" + this._Transformation.TranslationSourceTable + "]:   translate  [" + this._Transformation.TranslationFromColumn + "]   into  [" + this._Transformation.TranslationIntoColumn + "]";
                this.dataGridViewTranslationSourceTable.Rows.Add(this._Transformation.TranslationDictionarySourceTable.Count);
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Transformation.TranslationDictionarySourceTable)
                {
                    this.dataGridViewTranslationSourceTable.Rows[i].Cells[0].Value = KV.Key;
                    this.dataGridViewTranslationSourceTable.Rows[i].Cells[1].Value = KV.Value;
                    i++;
                }
            }
            else
                this.tableLayoutPanelTranslationSourceTable.Visible = false;
        }

        /// <summary>
        /// Open a source for the translation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonOpenTranslationSource_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.FormTranslationSource f = new FormTranslationSource();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this._Transformation.TranslationDictionary.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in f.TranslationDictionary)
                {
                    this._Transformation.TranslationDictionary.Add(KV.Key, KV.Value);
                }
                this.ShowTranslations();
            }
        }

        private void toolStripButtonOpenTranslationSourceTable_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Import.FormTranslationSource f = new FormTranslationSource(false, this._Transformation.TranslationSourceTable, this._Transformation.TranslationFromColumn, this._Transformation.TranslationIntoColumn);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && f.TranslationSourceTable.Length > 0 && f.TranslationFromColumn.Length > 0 && f.TranslationIntoColumn.Length > 0 && f.TranslationIntoColumn != f.TranslationFromColumn)
                {
                    if (this._Transformation.TranslationSourceTable != f.TranslationSourceTable ||
                        this._Transformation.TranslationIntoColumn != f.TranslationIntoColumn ||
                        this._Transformation.TranslationFromColumn != f.TranslationFromColumn)
                    {
                        if (this._Transformation.TranslationSourceTable.Length > 0 ||
                        this._Transformation.TranslationIntoColumn.Length > 0 ||
                        this._Transformation.TranslationFromColumn.Length > 0)
                        {
                            this._Transformation.TranslationDictionarySourceTable.Clear();
                        }
                        this._Transformation.TranslationSourceTable = f.TranslationSourceTable;
                        this._Transformation.TranslationIntoColumn = f.TranslationIntoColumn;
                        this._Transformation.TranslationFromColumn = f.TranslationFromColumn;
                        this._Transformation.TranslationDictionarySourceTableReadData();
                        this.ShowTranslations();
                    }
                }
                else if (f.DialogResult == DialogResult.OK && f.TranslationSourceTable.Length > 0 && f.TranslationFromColumn.Length > 0 && f.TranslationIntoColumn.Length > 0 && f.TranslationIntoColumn == f.TranslationFromColumn)
                {
                    System.Windows.Forms.MessageBox.Show("Translation of the same column + " + f.TranslationFromColumn + " into itself does not make sense");
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonTranslationSourceTableRemove_Click(object sender, EventArgs e)
        {
            this._Transformation.TranslationIntoColumn = "";
            this._Transformation.TranslationFromColumn = "";
            this._Transformation.TranslationSourceTable = "";
            this._Transformation.TranslationDictionarySourceTable.Clear();
            this.ShowTranslations();
        }

        private void dataGridViewTranslation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        #endregion

        #region Calculation

        private void comboBoxCalculationOperator_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxCalculationOperator.Items.Count == 0)
            {
                foreach (string s in this._Transformation.CalculationOperators)
                    this.comboBoxCalculationOperator.Items.Add(s);
            }
        }

        private void comboBoxCalculationOperator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Transformation.CalulationOperator = this.comboBoxCalculationOperator.SelectedItem.ToString();
            this.setCalculationExplanation();
        }

        private void comboBoxCalculationCondition_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxCalculationCondition.Items.Count == 0)
            {
                foreach (string s in this._Transformation.CalculationConditionOperators)
                    this.comboBoxCalculationCondition.Items.Add(s);
            }
        }

        private void comboBoxCalculationCondition_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Transformation.CalculationConditionOperator = this.comboBoxCalculationCondition.SelectedItem.ToString();
            this.setCalculationExplanation();
        }

        //private void maskedTextBoxCalculationCondition_TextChanged(object sender, EventArgs e)
        //{
        //    this._Transformation.CalculationConditionValue = this.maskedTextBoxCalculationCondition.Text;
        //}

        private void textBoxCalculationFactor_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBoxCalculationFactor.Text.Length > 0)
                {
                    System.Double D;
                    if (System.Double.TryParse(this.textBoxCalculationFactor.Text, out D))
                    {
                        this._Transformation.CalculationFactor = D.ToString();
                        //this.textBoxCalculationFactor.Text = D.ToString();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(this.textBoxCalculationFactor.Text + " is not a numeric value");
                        this.textBoxCalculationFactor.Text = "";
                    }
                }
                else
                    this._Transformation.CalculationFactor = null;
            }
            catch (System.Exception ex) { }
            this.setCalculationExplanation();
        }

        private void textBoxCalculationCondition_TextChanged(object sender, EventArgs e)
        {
            //this._Transformation.CalculationConditionValue = this.textBoxCalculationCondition.Text;

            try
            {
                if (this.textBoxCalculationCondition.Text.Length > 0)
                {
                    System.Double D;
                    if (System.Double.TryParse(this.textBoxCalculationCondition.Text, out D))
                    {
                        this._Transformation.CalculationConditionValue = D.ToString();
                        //this.textBoxCalculationCondition.Text = D.ToString();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show(this.textBoxCalculationCondition.Text + " is not a numeric value");
                        this.textBoxCalculationCondition.Text = "";
                    }
                }
                else
                    this._Transformation.CalculationConditionValue = null;
            }
            catch (System.Exception ex) { }
            this.setCalculationExplanation();
        }

        private void checkBoxCalculationApplyOnData_CheckedChanged(object sender, EventArgs e)
        {
            this._Transformation.CalculationApplyOnData = this.checkBoxCalculationApplyOnData.Checked;
            this.setCalculationExplanation();
        }

        private void comboBoxCalculationApplyOnDataOperator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Transformation.CalculationApplyOnDataOperator = this.comboBoxCalculationApplyOnDataOperator.SelectedItem.ToString();
            this.setCalculationExplanation();
        }

        private void comboBoxCalculationApplyOnDataOperator_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxCalculationApplyOnDataOperator.Items.Count == 0)
            {
                foreach (string s in this._Transformation.CalculationOperators)
                    this.comboBoxCalculationApplyOnDataOperator.Items.Add(s);
            }
        }

        private void setCalculationExplanation()
        {
            try
            {
                this.labelCalculationExplanation.Text = "[DB-value] = ";
                if (this._Transformation.CalculationApplyOnData && this._Transformation.CalculationApplyOnDataOperator != null && this._Transformation.CalculationApplyOnDataOperator.Length > 0)
                {
                    this.labelCalculationExplanation.Text += "[DB-value] " + this._Transformation.CalculationApplyOnDataOperator + " ";
                }
                if (this._Transformation.CalculationConditionOperator != null && this._Transformation.CalculationConditionValue != null && this._Transformation.CalculationConditionOperator.Length > 0 && this._Transformation.CalculationConditionValue.Length > 0)
                {
                    this.labelCalculationExplanation.Text += " if [FILE-value] " + this._Transformation.CalculationConditionOperator + " " + this._Transformation.CalculationConditionValue + " then ";
                }
                this.labelCalculationExplanation.Text += "[FILE-value] ";
                if (this._Transformation.CalulationOperator != null && this._Transformation.CalculationFactor != null && this._Transformation.CalulationOperator.Length > 0 && this._Transformation.CalculationFactor.Length > 0)
                {
                    this.labelCalculationExplanation.Text += this._Transformation.CalulationOperator + " " + this._Transformation.CalculationFactor + " ";
                }
                if (this._Transformation.CalculationConditionOperator != null && this._Transformation.CalculationConditionValue != null && this._Transformation.CalculationConditionOperator.Length > 0 && this._Transformation.CalculationConditionValue.Length > 0)
                {
                    this.labelCalculationExplanation.Text += " else [FILE-value]";
                }
            }
            catch (System.Exception ex)
            { }
        }

        #endregion

        #region Filter

        public void RemoveFilterConditin(DiversityWorkbench.Import.TransformationFilter Filter)
        {
            try
            {
                this._Transformation.FilterConditions.Remove(Filter);
                this.initControl();
            }
            catch (System.Exception ex) { }
        }

        private void buttonFilterColumn_Click(object sender, EventArgs e)
        {
            //DiversityWorkbench.Import.FormFileDataGrid f = new FormFileDataGrid(this.DataGridCopy(50), this._Transformation.FilterColumn);
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK && f.SelectedFileColumn() != null)
            //{
            //    int i = (int)f.SelectedFileColumn();
            //    this._Transformation.FilterColumn = i;
            //    this.initControl();
            //}
        }

        public System.Windows.Forms.DataGridView DataGridCopy(int FristLines)
        {
            System.Windows.Forms.DataGridView G = new DataGridView();
            G.ReadOnly = true;
            foreach (System.Windows.Forms.DataGridViewColumn C in this._iWizardInterface.DataGridView().Columns)
            {
                System.Windows.Forms.DataGridViewColumn Col = new DataGridViewColumn(C.CellTemplate);
                Col.HeaderText = C.HeaderText;
                G.Columns.Add(Col);
            }
            for (int i = 0; i < this._iWizardInterface.DataGridView().Rows.Count && i < FristLines; i++)
            {
                System.Windows.Forms.DataGridViewRow Row = new DataGridViewRow();
                G.Rows.Add(Row);
                for (int ii = 0; ii < this._iWizardInterface.DataGridView().Columns.Count; ii++)
                {
                    G.Rows[i].Cells[ii].Value = this._iWizardInterface.DataGridView().Rows[i].Cells[ii].Value;
                    G.Rows[i].Cells[ii].Style = this._iWizardInterface.DataGridView().Rows[i].Cells[ii].Style;
                }
            }
            return G;
        }

        private void radioButtonFilterColumnValue_Click(object sender, EventArgs e)
        {
            this._Transformation.FilterUseFixedValue = !this.radioButtonFilterColumnValue.Checked;
            this.initControl();
        }

        private void radioButtonFilterFixedValue_Click(object sender, EventArgs e)
        {
            this._Transformation.FilterUseFixedValue = this.radioButtonFilterFixedValue.Checked;
            this.initControl();
        }

        private void textBoxFilterFixedValue_TextChanged(object sender, EventArgs e)
        {
            this._Transformation.FilterFixedValue = this.textBoxFilterFixedValue.Text;
        }

        private void buttonFilterConditionAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.TransformationFilter TF = new TransformationFilter(this._Transformation);
            this._Transformation.FilterConditions.Add(TF);
            this.initControl();
            //DiversityWorkbench.Import.UserControlTransformationFilter U = new UserControlTransformationFilter(TF, this);
            //U.Dock = DockStyle.Top;
            //this.panelFilterConditions.Controls.Add(U);
            //U.BringToFront();
        }

        private void comboBoxFilterOperator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBoxFilterOperator.SelectedItem.ToString() == DiversityWorkbench.Import.Transformation.FilterConditionsOperators.And.ToString())
                this._Transformation.FilterConditionsOperator = DiversityWorkbench.Import.Transformation.FilterConditionsOperators.And;
            else this._Transformation.FilterConditionsOperator = DiversityWorkbench.Import.Transformation.FilterConditionsOperators.Or;
        }

        #endregion

        #region Color

        private System.Collections.Generic.Dictionary<string, Transformation.ColorFormat> _ColorFormats;
        private System.Collections.Generic.Dictionary<string, Transformation.ColorFormat> ColorFormats
        {
            get
            {
                if (_ColorFormats == null)
                {
                    _ColorFormats = new Dictionary<string, Transformation.ColorFormat>();
                    _ColorFormats.Add("Negative integer representing the color e.g. -8311918", Transformation.ColorFormat.ARGBint);
                    _ColorFormats.Add("3 decimals representing the color e.g. 255,239,175", Transformation.ColorFormat.RGBdec);
                    _ColorFormats.Add("hex value representing the color e.g. #FFEFAF", Transformation.ColorFormat.RGBhex);
                }
                return _ColorFormats;
            }
        }

        private void initColorComboBox(System.Windows.Forms.ComboBox comboBox)
        {
            if (comboBox.Items.Count == 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, Transformation.ColorFormat> KV in this.ColorFormats)
                    comboBox.Items.Add(KV.Key);
            }
        }

        private void comboBoxColorFrom_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Transformation.ColorFrom = ColorFormats[this.comboBoxColorFrom.SelectedItem.ToString()];
        }

        private void comboBoxColorTo_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Transformation.ColorInto = ColorFormats[this.comboBoxColorTo.SelectedItem.ToString()];
        }

        #endregion

    }
}
