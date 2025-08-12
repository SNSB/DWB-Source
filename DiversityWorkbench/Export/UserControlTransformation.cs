using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Export
{
    public partial class UserControlTransformation : UserControl
    {
        #region Parameter

        private DiversityWorkbench.Export.Transformation _Transformation;

        public DiversityWorkbench.Export.Transformation Transformation
        {
            get { return _Transformation; }
        }

        //private int _PositionInFile;

        //private DiversityWorkbench.Import.iWizardInterface _iWizardInterface;

        #endregion

        #region Construction and control

        public UserControlTransformation(DiversityWorkbench.Export.Transformation Transformation, DiversityWorkbench.Export.FileColumn FileColumn, DiversityWorkbench.Export.iExporter iExporter, string HelpNameSpace)
        {
            InitializeComponent();
            this._Transformation = Transformation;
            //this._PositionInFile = PositionInFile;
            //this._iWizardInterface = iWizardInterface;
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
                        this.numericUpDownPosition.Value = this._Transformation.SplitterPosition;
                        if (this._Transformation.ReverseSequence)
                            this.buttonReverseSequence.Image = this.imageListReverseSequence.Images[1];
                        else
                            this.buttonReverseSequence.Image = this.imageListReverseSequence.Images[0];
                        if (this._Transformation.SplitterIsStartPosition)
                            this.radioButtonPositionStart.Checked = true;
                        else this.radioButtonPosition.Checked = true;
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
                        this.textBoxRegex.Text = this._Transformation.RegularExpression;
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
                    case DiversityWorkbench.Export.Transformation.TransformationType.Calculation:
                        this.splitContainerSpTr_RxRe.Panel1Collapsed = true;
                        this.splitContainerRxRe.Panel1Collapsed = true;
                        this.splitContainerReCal.Panel1Collapsed = true;
                        // Operator & Factor
                        string Mask = this.maskedTextBoxCalculationFactor.Mask;
                        int iMask = Mask.IndexOf('.');
                        int iText = -1;
                        if (this._Transformation.CalculationFactor != null)
                            this._Transformation.CalculationFactor.IndexOf('.');
                        string Spacer = "";
                        if (iText > 0)
                        {
                            for (int i = iText; i < iMask; i++)
                                Spacer += " ";
                        }
                        else if (iText == -1 && this._Transformation.CalculationFactor != null)
                        {
                            for (int i = this._Transformation.CalculationFactor.Length; i < iMask; i++)
                                Spacer += " ";
                        }
                        if (this._Transformation.CalculationFactor != null)
                            this.maskedTextBoxCalculationFactor.Text = Spacer + this._Transformation.CalculationFactor;
                        if (this._Transformation.CalulationOperator != null)
                            this.comboBoxCalculationOperator.Text = this._Transformation.CalulationOperator;
                        // Condition
                        if (this._Transformation.CalculationConditionValue != null &&
                            this._Transformation.CalculationConditionOperator != null &&
                            this._Transformation.CalculationConditionValue.Length > 0 &&
                            this._Transformation.CalculationConditionOperator.Length > 0)
                        {
                            Mask = this.maskedTextBoxCalculationCondition.Mask;
                            iMask = Mask.IndexOf('.');
                            iText = this._Transformation.CalculationConditionValue.IndexOf('.');
                            Spacer = "";
                            if (iText > 0)
                            {
                                for (int i = iText; i < iMask; i++)
                                    Spacer += " ";
                            }
                            else if (iText == -1)
                            {
                                for (int i = this._Transformation.CalculationConditionValue.Length; i < iMask; i++)
                                    Spacer += " ";
                            }
                            this.maskedTextBoxCalculationCondition.Text = Spacer + this._Transformation.CalculationConditionValue;
                            this.comboBoxCalculationCondition.Text = this._Transformation.CalculationConditionOperator;
                        }
                        break;
                    case DiversityWorkbench.Export.Transformation.TransformationType.Filter:
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
                                this._Transformation.FilterConditionsOperator = DiversityWorkbench.Export.Transformation.FilterConditionsOperators.And;
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
                            DiversityWorkbench.Export.TransformationFilter TF = new TransformationFilter(this._Transformation);
                            this._Transformation.FilterConditions.Add(TF);
                        }
                        this.panelFilterConditions.Controls.Clear();
                        foreach (DiversityWorkbench.Export.TransformationFilter TF in this._Transformation.FilterConditions)
                        {
                            DiversityWorkbench.Export.UserControlTransformationFilter U = new UserControlTransformationFilter(TF, this);
                            U.Dock = DockStyle.Top;
                            this.panelFilterConditions.Controls.Add(U);
                            U.BringToFront();
                        }
                        break;
                    case Transformation.TransformationType.Color:
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

        private void radioButtonPosition_CheckedChanged(object sender, EventArgs e)
        {
            this._Transformation.SplitterIsStartPosition = this.radioButtonPositionStart.Checked;
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
                System.Windows.Forms.DataGridViewTextBoxColumn Ttranslation = new System.Windows.Forms.DataGridViewTextBoxColumn();
                Ttranslation.HeaderText = "Translation";
                if (this.Transformation.FileColumn.TableColumn.DtSource() != null)
                {
                    System.Windows.Forms.DataGridViewComboBoxColumn Tsource = new System.Windows.Forms.DataGridViewComboBoxColumn();
                    Tsource.HeaderText = "Source";
                    this.dataGridViewTranslation.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                    Tsource,
                    Ttranslation});
                    Tsource.DataSource = this._Transformation.FileColumn.TableColumn.DtSource();
                    Tsource.DisplayMember = this._Transformation.FileColumn.TableColumn.ForeignRelationColumn;
                    Tsource.ValueMember = this._Transformation.FileColumn.TableColumn.ForeignRelationColumn;
                }
                else
                {
                    System.Windows.Forms.DataGridViewTextBoxColumn Tsource = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    Tsource.HeaderText = "Source";
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
            this.ShowTranslations();
        }

        /// <summary>
        /// Get a list of all texts in the data that can be translated and are so far missing in the translation dictionary
        /// </summary>
        private void GetTranslationList()
        {
            try
            {
                if (this._Transformation.FileColumn.TableColumn.DtSource() != null)
                {
                    foreach (System.Data.DataRow R in this._Transformation.FileColumn.TableColumn.DtSource().Rows)
                        this._Transformation.TranslationDictionary.Add(R[0].ToString(), "");
                    //for (int i = DiversityWorkbench.Import.Import.StartLine - 1; i < DiversityWorkbench.Import.Import.EndLine; i++)
                    //{
                    //    //if (this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value != null
                    //    //    && this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString().Length > 0)
                    //    //{
                    //    //    string Result = this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString();
                    //    //    System.Collections.Generic.Dictionary<int, string> DD = new Dictionary<int, string>();
                    //    //    Result = DiversityWorkbench.Import.DataColumn.TransformedValue(Result, this._Transformation.iDataColumn.TransformationList(), DD);
                    //    //    if (!this._Transformation.TranslationDictionary.ContainsKey(Result))
                    //    //        this._Transformation.TranslationDictionary.Add(Result, "");

                    //    //    //string NewText = this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString();
                    //    //    //if (!this._Transformation.TranslationDictionary.ContainsKey(NewText))
                    //    //    //    this._Transformation.TranslationDictionary.Add(NewText, "");
                    //    //}
                    //}
                    this.ShowTranslations();
                }
                else
                {
                    if (this._Transformation.FileColumn.TableColumnUnitValue == null)
                    {
                        this._Transformation.TranslationDictionary.Clear();
                        System.Data.DataTable dt = new DataTable();
                        string SQL = "SELECT DISTINCT [" + this._Transformation.FileColumn.TableColumn.ColumnName + "] " +
                            "FROM [" + this._Transformation.FileColumn.TableColumn.Table.TableName + "] " +
                            "WHERE [" + this._Transformation.FileColumn.TableColumn.ColumnName + "] <> '' " +
                            "ORDER BY [" + this._Transformation.FileColumn.TableColumn.ColumnName + "]";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                            this._Transformation.TranslationDictionary.Add(R[0].ToString(), "");
                        this.ShowTranslations();
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Only available for columns linked to enumeration tables");
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        /// Show the current translations
        /// </summary>
        private void ShowTranslations()
        {
            try
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
            }
            catch (System.Exception ex)
            {
            }
        }

        /// <summary>
        /// Open a source for the translation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonOpenTranslationSource_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.FormTranslationSource f = new DiversityWorkbench.Import.FormTranslationSource();
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

        private void dataGridViewTranslation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        #endregion

        #region Calculation

        private void comboBoxCalculationOperator_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxCalculationOperator.Items.Count == 0)
            {
                foreach (string s in this._Transformation.CalulationOperators)
                    this.comboBoxCalculationOperator.Items.Add(s);
            }
        }

        private void maskedTextBoxCalculationFactor_TextChanged(object sender, EventArgs e)
        {
            this._Transformation.CalculationFactor = this.maskedTextBoxCalculationFactor.Text;
        }

        private void comboBoxCalculationOperator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Transformation.CalulationOperator = this.comboBoxCalculationOperator.SelectedItem.ToString();
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
        }

        private void maskedTextBoxCalculationCondition_TextChanged(object sender, EventArgs e)
        {
            this._Transformation.CalculationConditionValue = this.maskedTextBoxCalculationCondition.Text;
        }

        #endregion

        #region Filter

        public void RemoveFilterCondition(DiversityWorkbench.Export.TransformationFilter Filter)
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
            //System.Windows.Forms.Form f = new Form();
            //f.Text = "Select a file column for filtering";
            //f.ShowIcon = false;
            //f.StartPosition = FormStartPosition.CenterParent;
            //System.Windows.Forms.Panel p = new Panel();
            //p.Dock = DockStyle.Fill;
            //p.AutoScroll = true;
            //f.Controls.Add(p);
            //this._FileColumnsForFilter = new List<UserControlFileColumn>();
            //foreach (System.Collections.Generic.KeyValuePair<double, Export.FileColumn> KV in Export.Exporter.FileColumnList)
            //{
            //    Export.UserControlFileColumn U = new UserControlFileColumn(KV.Value, "");
            //    U.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            //    this._FileColumnsForFilter.Add(U);
            //    U.Dock = DockStyle.Left;
            //    U.BringToFront();
            //    p.Controls.Add(U);
            //}


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
            //foreach (System.Windows.Forms.DataGridViewColumn C in this._iWizardInterface.DataGridView().Columns)
            //{
            //    System.Windows.Forms.DataGridViewColumn Col = new DataGridViewColumn(C.CellTemplate);
            //    Col.HeaderText = C.HeaderText;
            //    G.Columns.Add(Col);
            //}
            //for (int i = 0; i < this._iWizardInterface.DataGridView().Rows.Count && i < FristLines; i++)
            //{
            //    System.Windows.Forms.DataGridViewRow Row = new DataGridViewRow();
            //    G.Rows.Add(Row);
            //    for (int ii = 0; ii < this._iWizardInterface.DataGridView().Columns.Count; ii++)
            //    {
            //        G.Rows[i].Cells[ii].Value = this._iWizardInterface.DataGridView().Rows[i].Cells[ii].Value;
            //        G.Rows[i].Cells[ii].Style = this._iWizardInterface.DataGridView().Rows[i].Cells[ii].Style;
            //    }
            //}
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
            DiversityWorkbench.Export.TransformationFilter TF = new TransformationFilter(this._Transformation);
            this._Transformation.FilterConditions.Add(TF);
            this.initControl();
            //DiversityWorkbench.Export.UserControlTransformationFilter U = new UserControlTransformationFilter(TF, this);
            //U.Dock = DockStyle.Top;
            //this.panelFilterConditions.Controls.Add(U);
            //U.BringToFront();
        }

        private void comboBoxFilterOperator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBoxFilterOperator.SelectedItem.ToString() == DiversityWorkbench.Import.Transformation.FilterConditionsOperators.And.ToString())
                this._Transformation.FilterConditionsOperator = DiversityWorkbench.Export.Transformation.FilterConditionsOperators.And;
            else this._Transformation.FilterConditionsOperator = DiversityWorkbench.Export.Transformation.FilterConditionsOperators.Or;
        }

        #endregion


    }
}
