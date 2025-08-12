using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Import
{
    public partial class FormTransformation : Form
    {

        #region Parameter

        private System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> _Transformations;
        private int _PositionInFile;
        private DiversityWorkbench.Import.iDataColumn _iDataColumn;
        private string _Prefix;
        private string _Postfix;
        private DiversityWorkbench.Import.iWizardInterface _iWizardInterface;
        
        #endregion

        #region Construction
        
        public FormTransformation(System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> Transformations, 
            string Prefix, 
            string Postfix,
            DiversityWorkbench.Import.iDataColumn iDataColumn, 
            int PositionInFile,
            DiversityWorkbench.Import.iWizardInterface iWizardInterface,
            string HelpNameSpace)
        {
            InitializeComponent();
            this._Transformations = Transformations;
            this._Prefix = Prefix;
            this._Postfix = Postfix;
            this._iDataColumn = iDataColumn;
            this._PositionInFile = PositionInFile;
            this._iWizardInterface = iWizardInterface;
            this.toolStripTextBoxPrefix.Text = this._Prefix;
            this.toolStripTextBoxPostfix.Text = this._Postfix;
            this.initForm();
            this.helpProvider.HelpNamespace = HelpNameSpace;
        }
        
        #endregion

        #region Form
        
        private void initForm()
        {
            try
            {
                if (this.Text.IndexOf(" for column ") == -1)
                {
                    this.Text += " for column ";
                    if (this._iDataColumn.GetType() == typeof(DiversityWorkbench.Import.DataColumn))
                    {
                        DiversityWorkbench.Import.DataColumn D = (DiversityWorkbench.Import.DataColumn)this._iDataColumn;
                        this.Text += D.DisplayText;
                    }
                    else if (this._iDataColumn.GetType() == typeof(DiversityWorkbench.Import.ColumnMulti))
                    {
                        DiversityWorkbench.Import.ColumnMulti MC = (DiversityWorkbench.Import.ColumnMulti)this._iDataColumn;
                        this.Text += MC.DataColumn.DisplayText;
                    }
                }

                this.tabControlTransformations.TabPages.Clear();
                int i = 1;
                foreach (DiversityWorkbench.Import.Transformation T in this._Transformations)
                {
                    string Title = i.ToString();
                    if (T.TypeOfTransformation == Transformation.TransformationType.RegularExpression)
                        Title = "RegEx " + Title;
                    if (T.TypeOfTransformation == Transformation.TransformationType.Calculation)
                        Title = "∑ " + Title;
                    System.Windows.Forms.TabPage TP = new TabPage(Title);
                    switch (T.TypeOfTransformation)
                    {
                        case Transformation.TransformationType.Split:
                            TP.ImageIndex = 0;
                            break;
                        case Transformation.TransformationType.Translation:
                            TP.ImageIndex = 1;
                            if (T.TranslationSourceTable != null && T.TranslationSourceTable.Length > 0 && 
                                T.TranslationIntoColumn != null && T.TranslationIntoColumn.Length > 0 && 
                                T.TranslationFromColumn != null && T.TranslationFromColumn.Length > 0)
                            {
                                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(700);
                            }
                            else
                            {
                                // adapt size
                                int Height = this.Height;
                                int split = this.splitContainerMain.SplitterDistance;
                                if (split < 300)
                                {
                                    split += (Height - split) / 3;
                                    this.splitContainerMain.SplitterDistance = split;
                                }
                            }
                            break;
                        case Transformation.TransformationType.Replacement:
                            TP.ImageIndex = 2;
                            break;
                        case Transformation.TransformationType.Filter:
                            TP.ImageIndex = 3;
                            break;
                        case Transformation.TransformationType.Color:
                            TP.ImageIndex = 4;
                            break;
                    }
                    DiversityWorkbench.Import.UserControlTransformation UT = new UserControlTransformation(T, this._PositionInFile, this._iWizardInterface, this.helpProvider.HelpNamespace);
                    UT.Dock = DockStyle.Fill;
                    TP.Controls.Add(UT);
                    this.tabControlTransformations.TabPages.Add(TP);
                    i++;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                null,
                null);
        }

        #endregion

        #region Adding and removing transformations
        
        private void toolStripButtonSplit_Click(object sender, EventArgs e)
        {
            this.AddTransformation(Transformation.TransformationType.Split);
        }

        private void toolStripButtonTranslation_Click(object sender, EventArgs e)
        {
            this.AddTransformation(Transformation.TransformationType.Translation);
        }

        private void toolStripButtonRegularExpression_Click(object sender, EventArgs e)
        {
            this.AddTransformation(Transformation.TransformationType.RegularExpression);
        }

        private void toolStripButtonReplacement_Click(object sender, EventArgs e)
        {
            this.AddTransformation(Transformation.TransformationType.Replacement);
        }

        private void toolStripButtonCalculation_Click(object sender, EventArgs e)
        {
            this.AddTransformation(Transformation.TransformationType.Calculation);
        }

        private void toolStripButtonFilter_Click(object sender, EventArgs e)
        {
            this.AddTransformation(Transformation.TransformationType.Filter);
        }

        private void toolStripButtonColor_Click(object sender, EventArgs e)
        {
            this.AddTransformation(Transformation.TransformationType.Color);
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.TabPage TP = this.tabControlTransformations.SelectedTab;
            DiversityWorkbench.Import.UserControlTransformation U = (DiversityWorkbench.Import.UserControlTransformation)TP.Controls[0];
            this._Transformations.Remove(U.Transformation);
            this.initForm();
        }

        private void AddTransformation(DiversityWorkbench.Import.Transformation.TransformationType Type)
        {
            try
            {
                DiversityWorkbench.Import.Transformation T = new Transformation(this._iDataColumn, Type);
                //this._Transformations.Add(T);
                this.initForm();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Testing
        
        private void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                this.dataGridViewTest.Columns.Clear();
                System.Windows.Forms.DataGridViewTextBoxColumn Tsource = new System.Windows.Forms.DataGridViewTextBoxColumn();
                Tsource.HeaderText = "Source";
                System.Windows.Forms.DataGridViewTextBoxColumn Ttransform = new System.Windows.Forms.DataGridViewTextBoxColumn();
                Ttransform.HeaderText = "Transformation";
                this.dataGridViewTest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                Tsource,
                Ttransform});
                System.Collections.Generic.List<string> SourceValues = new List<string>();
                System.Collections.Generic.SortedDictionary<string, string> Transformation = new SortedDictionary<string, string>();
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, string>> DD = new Dictionary<string, Dictionary<int, string>>();
                for (int i = DiversityWorkbench.Import.Import.StartLine - 1; i < DiversityWorkbench.Import.Import.EndLine; i++)
                {
                    if (this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value != null
                        && this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString().Length > 0)
                    {
                        string NewText = this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString();
                        if (!SourceValues.Contains(NewText))
                        {
                            SourceValues.Add(NewText);
                            Dictionary<int, string> dd = new Dictionary<int, string>();
                            dd.Add((int)this._PositionInFile, this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString());
                            DD.Add(NewText, dd);
                        }
                    }
                }
                bool ContainsFilter = false;
                foreach (string S in SourceValues)
                {
                    foreach (DiversityWorkbench.Import.Transformation T in this._Transformations)
                    {
                        if (T.TypeOfTransformation == DiversityWorkbench.Import.Transformation.TransformationType.Filter )
                        {
                            foreach(DiversityWorkbench.Import.TransformationFilter TF in T.FilterConditions)
                                if (TF.FilterColumn != this._PositionInFile)
                                    ContainsFilter = true;
                        }
                    }
                    System.Collections.Generic.Dictionary<int, string> dd = new Dictionary<int, string>();
                    if (DD.ContainsKey(S))
                        dd = DD[S];
                    Transformation.Add(S, DiversityWorkbench.Import.DataColumn.TransformedValue(S, this._Transformations, dd));
                }
                if (Transformation.Count > 0)
                {
                    this.dataGridViewTest.Rows.Add(Transformation.Count);
                    int tt = 0;
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Transformation)
                    {
                        this.dataGridViewTest.Rows[tt].Cells[0].Value = KV.Key;
                        this.dataGridViewTest.Rows[tt].Cells[1].Value = this.toolStripTextBoxPrefix.Text + KV.Value + this.toolStripTextBoxPostfix.Text;
                        tt++;
                    }
                    if (ContainsFilter)
                        System.Windows.Forms.MessageBox.Show("Filter only valid for single lines");
                }
                this.dataGridViewTest.ReadOnly = true;
                this.dataGridViewTest.AllowUserToAddRows = false;
                this.dataGridViewTest.AllowUserToDeleteRows = false;
                this.dataGridViewTest.AllowUserToOrderColumns = false;
                this.dataGridViewTest.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridViewTest.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
                this.dataGridViewTest.RowHeadersVisible = false;
                this.dataGridViewTest.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

    }
}
