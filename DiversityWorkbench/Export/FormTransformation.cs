using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Export
{
    public partial class FormTransformation : Form
    {
        #region Parameter

        private System.Collections.Generic.List<DiversityWorkbench.Export.Transformation> _Transformations;
        //private int _PositionInFile;
        private Export.FileColumn _FileColumn;
        //private DiversityWorkbench.Export.iTableColumn _iTableColumn;
        private string _Prefix;
        private string _Postfix;
        private DiversityWorkbench.Export.iExporter _iExporter;

        #endregion

        #region Construction

        public FormTransformation(System.Collections.Generic.List<DiversityWorkbench.Export.Transformation> Transformations,
            string Prefix,
            string Postfix,
            Export.FileColumn FileColumn,
            DiversityWorkbench.Export.iExporter iExporter,
            string HelpNameSpace)
        {
            InitializeComponent();
            this._Transformations = Transformations;
            this._Prefix = Prefix;
            this._Postfix = Postfix;
            //this._iTableColumn = iTableColumn;
            this._FileColumn = FileColumn;
            //this._PositionInFile = PositionInFile;
            this._iExporter = iExporter;
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
                this.Text = "Transformation for column " + this._FileColumn.TableColumn.DisplayText;
                if (this._FileColumn.TableColumnUnitValue != null)
                {
                    this.Text = "Transformation for column " + this._FileColumn.TableColumn.DisplayText + " (" + this._FileColumn.TableColumnUnitValue.SourceDisplayText + ")";
                }
                this.tabControlTransformations.TabPages.Clear();
                int i = 1;
                foreach (DiversityWorkbench.Export.Transformation T in this._Transformations)
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
                    DiversityWorkbench.Export.UserControlTransformation UT = new UserControlTransformation(T, this._FileColumn, this._iExporter, this.helpProvider.HelpNamespace);
                    UT.Dock = DockStyle.Fill;
                    TP.Controls.Add(UT);
                    this.tabControlTransformations.TabPages.Add(TP);
                    i++;
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
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

        private void toolStripButtonColor_Click(object sender, EventArgs e)
        {
            this.AddTransformation(Transformation.TransformationType.Color);
        }

        private void toolStripButtonFilter_Click(object sender, EventArgs e)
        {
            this.AddTransformation(Transformation.TransformationType.Filter);
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.tabControlTransformations.SelectedTab != null)
                {
                    System.Windows.Forms.TabPage TP = this.tabControlTransformations.SelectedTab;
                    DiversityWorkbench.Export.UserControlTransformation U = (DiversityWorkbench.Export.UserControlTransformation)TP.Controls[0];
                    this._Transformations.Remove(U.Transformation);
                    this.initForm();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void AddTransformation(DiversityWorkbench.Export.Transformation.TransformationType Type)
        {
            DiversityWorkbench.Export.Transformation T = new Transformation(this._FileColumn, Type);
            this.initForm();
        }

        #endregion

        #region Testing

        private async void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                // Test if testing is possible
                bool OnlyFilter = true;
                bool ContainsFilter = false;
                foreach (Export.Transformation T in this._FileColumn.Transformations)
                {
                    if (T.TypeOfTransformation == Export.Transformation.TransformationType.Filter)
                        ContainsFilter = true;
                    else OnlyFilter = false;
                }
                if (!ContainsFilter && OnlyFilter) OnlyFilter = false;
                if (OnlyFilter)
                {
                    System.Windows.Forms.MessageBox.Show("A test for filter transformation is not possible");
                    return;
                }
                else if (ContainsFilter)
                {
                    System.Windows.Forms.MessageBox.Show("Test will be performed without the filter transformation");
                }

                // Test
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
                string SQL = "SELECT DISTINCT [" + this._FileColumn.TableColumn.ColumnName + "] FROM [" + this._FileColumn.TableColumn.Table.TableName + "] T " +
                    this._FileColumn.TableColumn.Table.SqlWhereClause(DiversityWorkbench.Export.Exporter.ListOfIDs);
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                    SourceValues.Add(R[0].ToString());
                
                //for (int i = DiversityWorkbench.Import.Import.StartLine - 1; i < DiversityWorkbench.Import.Import.EndLine; i++)
                //{
                //    //if (this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value != null
                //    //    && this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString().Length > 0)
                //    //{
                //    //    string NewText = this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString();
                //    //    if (!SourceValues.Contains(NewText))
                //    //    {
                //    //        SourceValues.Add(NewText);
                //    //        Dictionary<int, string> dd = new Dictionary<int, string>();
                //    //        dd.Add((int)this._PositionInFile, this._iWizardInterface.DataGridView().Rows[i].Cells[this._PositionInFile].Value.ToString());
                //    //        DD.Add(NewText, dd);
                //    //    }
                //    //}
                //}
                //bool ContainsFilter = false;
                foreach (string S in SourceValues)
                {
                    //foreach (DiversityWorkbench.Export.Transformation T in this._Transformations)
                    //{
                    //    if (T.TypeOfTransformation == DiversityWorkbench.Export.Transformation.TransformationType.Filter)
                    //    {
                    //        foreach (DiversityWorkbench.Export.TransformationFilter TF in T.FilterConditions)
                    //            if (TF.FilterColumn != this._PositionInFile)
                    //                ContainsFilter = true;
                    //    }
                    //}
                    //System.Collections.Generic.Dictionary<int, string> dd = new Dictionary<int, string>();
                    //if (DD.ContainsKey(S))
                    //    dd = DD[S];
                    Transformation.Add(S, await this._FileColumn.TransformedValue(S));// DiversityWorkbench.Export.FileColumn.TransformedValue(S));//, this._Transformations, dd));
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
                    //if (ContainsFilter)
                    //    System.Windows.Forms.MessageBox.Show("Filter only valid for single lines");
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

        private void dataGridViewTest_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        #endregion

    }
}
