using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormExportList : Form
    {
        #region Parameter
        private DiversityWorkbench.UserControls.UserControlQueryList _QueryList;
        private System.Collections.Generic.List<DiversityWorkbench.QueryCondition> _QueryConditions;
        
        #endregion

        #region Construction
        public FormExportList(ref DiversityWorkbench.UserControls.UserControlQueryList QueryList)
        {
            InitializeComponent();
            this._QueryList = QueryList;
            this.initForm();
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }
        
        #endregion

        #region Form and button events

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void initForm()
        {
            //string Logfile = ...Windows.Forms.Application.StartupPath + "\\" + System.Windows.Forms.Application.ProductName.ToString() + "Export_";
            //Logfile += System.DateTime.Now.Year.ToString();
            //if (System.DateTime.Now.Month.ToString().Length < 2) Logfile += "0";
            //Logfile += System.DateTime.Now.Month.ToString();
            //if (System.DateTime.Now.Day.ToString().Length < 2) Logfile += "0";
            //Logfile += System.DateTime.Now.Day.ToString() + "_";
            //if (System.DateTime.Now.Hour.ToString().Length < 2) Logfile += "0";
            //Logfile += System.DateTime.Now.Hour.ToString();
            //if (System.DateTime.Now.Minute.ToString().Length < 2) Logfile += "0";
            //Logfile += System.DateTime.Now.Minute.ToString();
            //if (System.DateTime.Now.Second.ToString().Length < 2) Logfile += "0";
            //Logfile += System.DateTime.Now.Second.ToString() + ".txt";
            //this.textBoxFilePath.Text = Logfile;

            this.checkBoxForReimport.Checked = true;
        }
        
        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog = new OpenFileDialog();
            this.openFileDialog.RestoreDirectory = true;
            this.openFileDialog.Multiselect = false;
            this.openFileDialog.InitialDirectory = Folder.Export();
            this.openFileDialog.Filter = "Text Files|*.txt";
            try
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    this.textBoxFilePath.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSettings_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> L = this.CollectionSpecimen.QueryConditions();
            DiversityWorkbench.Forms.FormQueryOptions f = new DiversityWorkbench.Forms.FormQueryOptions(L, DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ExportFields, "Choose the fields for the export");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ExportFields = f.QueryConditionVisibility;
            }
        }

        //private void initOrderByColumn()
        //{
        //    string TableAlias = "";
        //    string MainTable = "CollectionSpecimen";
        //    string MainTableAlias = "";
        //    this.comboBoxOrderBy.Items.Clear();
        //    System.Collections.Generic.List<DiversityWorkbench.QueryCondition> L = this.CollectionSpecimen.QueryConditions();
        //    DiversityWorkbench.Forms.FormQueryOptions f = new DiversityWorkbench.Forms.FormQueryOptions(L, DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ExportFields, "Choose the fields for the export");
        //    System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QC = f.QueryConditionList;
        //    foreach (DiversityWorkbench.QueryCondition Q in QC)
        //    {
        //        if (Q.showCondition)
        //        {
        //            if (Q.Table == MainTable)
        //            {
        //                if (MainTableAlias.Length == 0) MainTableAlias = "T0";
        //                TableAlias = MainTableAlias;
        //            }
        //            if (Q.Column.IndexOf(".") > -1)
        //            {
        //                this.comboBoxOrderBy.Items.Add(TableAlias + Q.Column.Substring(Q.Column.IndexOf(".")));
        //            }
        //            else
        //            {
        //                this.comboBoxOrderBy.Items.Add(TableAlias + "." + Q.Column);
        //            }
        //        }
        //    }
        //}

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name, System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
        }

        #endregion

        #region Export

        private string ExportFile
        {
            get
            {
                string Logfile = Folder.Export() + System.Windows.Forms.Application.ProductName.ToString() + "Export_";
                Logfile += System.DateTime.Now.Year.ToString();
                if (System.DateTime.Now.Month.ToString().Length < 2) Logfile += "0";
                Logfile += System.DateTime.Now.Month.ToString();
                if (System.DateTime.Now.Day.ToString().Length < 2) Logfile += "0";
                Logfile += System.DateTime.Now.Day.ToString() + "_";
                if (System.DateTime.Now.Hour.ToString().Length < 2) Logfile += "0";
                Logfile += System.DateTime.Now.Hour.ToString();
                if (System.DateTime.Now.Minute.ToString().Length < 2) Logfile += "0";
                Logfile += System.DateTime.Now.Minute.ToString();
                if (System.DateTime.Now.Second.ToString().Length < 2) Logfile += "0";
                Logfile += System.DateTime.Now.Second.ToString() + ".txt";
                return Logfile;
            }
        }

        private void buttonStartExport_Click(object sender, EventArgs e)
        {
            this.textBoxFilePath.Text = this.ExportFile;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string OrderColumn = this.comboBoxOrderBy.Text;
            this.comboBoxOrderBy.Items.Clear();
            System.IO.StreamWriter sw;
            sw = new System.IO.StreamWriter(this.textBoxFilePath.Text, true, System.Text.Encoding.UTF8);
            if (this._QueryList.ListOfIDs.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select at least one item for the export");
                return;
            }
            this.labelCurrentAction.Text = "Getting selected IDs";
            if (this._QueryList.ListOfIDs.Count > 100)
            {
                this.labelCurrentAction.Text = "";
                if (System.Windows.Forms.MessageBox.Show("You selected more than 100 item for the export! OK?", "Export more than 100 items?", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return;
            }
            try
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> L = this.CollectionSpecimen.QueryConditions();
                DiversityWorkbench.Forms.FormQueryOptions f = new DiversityWorkbench.Forms.FormQueryOptions(L, DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ExportFields, "Choose the fields for the export");
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QC = f.QueryConditionList;
                bool OK = false;
                foreach (DiversityWorkbench.QueryCondition Q in QC)
                {
                    if (Q.showCondition)
                    {
                        OK = true;
                        break;
                    }
                }
                if (!OK)
                {
                    f.ShowDialog();
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.ExportFields = f.QueryConditionVisibility;
                    DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.Save();
                    if (f.DialogResult != DialogResult.OK) return;
                    else QC = f.QueryConditionList;
                    foreach (DiversityWorkbench.QueryCondition Q in QC)
                    {
                        if (Q.showCondition)
                        {
                            OK = true;
                            break;
                        }
                    }
                    if (!OK)
                    {
                        System.Windows.Forms.MessageBox.Show("You must at least choose one field for the export");
                        return;
                    }
                }
                string SQL = "";
                string Fields = "";
                System.Collections.Generic.List<string> FieldList = new List<string>();
                System.Collections.Generic.List<string> TableList = new List<string>();
                System.Collections.Generic.List<string> TableAliasList = new List<string>();
                System.Collections.Generic.List<string> FieldDescriptionList = new List<string>();
                string MainTable = "CollectionSpecimen";
                string MainTableAlias = "";
                string FromClause = "";
                string WhereClause = "";
                System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();
                System.Collections.Generic.Dictionary<string, string> TableLinks = new Dictionary<string, string>();

                // FromClause and Fieldlists
                int iT = 1;
                string TableAlias = "";
                string JoinDuplicateCheck = "";
                string Join = "";
                MainTableAlias = "T0";
                string JoinClause = " LEFT OUTER JOIN ";
                if (this.radioButtonInnerJoin.Checked)
                    JoinClause = " INNER JOIN ";
                foreach (DiversityWorkbench.QueryCondition Q in QC)
                {
                    bool Add = false;
                    if (Q.showCondition)
                    {
                        if (Q.Table == MainTable)
                        {
                            if (!FromClause.StartsWith("FROM")) FromClause = "FROM " + MainTable + " T0 " + FromClause;
                            if (MainTableAlias.Length == 0) MainTableAlias = "T0";
                            TableAlias = MainTableAlias;
                        }
                        else
                        {
                            if (Q.IntermediateTable.Length > 0 && Q.ForeignKeySecondColumn.Length > 0)
                            {
                                string ForeignKeySecondColumnInForeignTable = Q.ForeignKeySecondColumnInForeignTable;
                                if (ForeignKeySecondColumnInForeignTable.Length == 0)
                                    ForeignKeySecondColumnInForeignTable = Q.ForeignKeySecondColumn;
                                JoinDuplicateCheck = JoinClause + Q.IntermediateTable + " ITx ON " +
                                    MainTableAlias + "." + Q.ForeignKey + " = ITx." + Q.ForeignKey +
                                    JoinClause + "[" + Q.Table + "] Tx ON " +
                                    "ITx." + Q.ForeignKey + " = Tx." + Q.ForeignKey +
                                    " AND ITx." + ForeignKeySecondColumnInForeignTable + " = Tx." + Q.ForeignKeySecondColumn;
                                if (Q.Restriction.Length > 0)
                                    JoinDuplicateCheck += " AND Tx." + Q.Restriction;
                                if (!Tables.TryGetValue(JoinDuplicateCheck, out TableAlias))
                                {
                                    iT++;
                                    if (Q.Table == MainTable) TableAlias = MainTableAlias;
                                    else TableAlias = "T" + iT.ToString();
                                    Tables.Add(JoinDuplicateCheck, TableAlias);
                                    TableLinks.Add("I" + TableAlias + "." + Q.ForeignKeySecondColumn, Q.IntermediateTable);
                                    Join = JoinClause + Q.IntermediateTable + " I" + TableAlias + " ON " +
                                        MainTableAlias + "." + Q.ForeignKey + " = I" + TableAlias + "." + Q.ForeignKey +
                                        JoinClause + "[" + Q.Table + "] " + TableAlias + " ON " +
                                        "I" + TableAlias + "." + Q.ForeignKey + " = " + TableAlias + "." + Q.ForeignKey +
                                        " AND I" + TableAlias + "." + ForeignKeySecondColumnInForeignTable + " = " + TableAlias + "." + Q.ForeignKeySecondColumn;
                                    if (Q.Restriction.Length > 0)
                                        Join += " AND " + TableAlias + "." + Q.Restriction;
                                    Add = true;
                                }
                            }
                            else if (Q.IntermediateTable.Length > 0 && Q.ForeignKey.Length > 0)
                            {
                                string ForeignKeySecondColumnInForeignTable = Q.ForeignKeySecondColumnInForeignTable;
                                if (ForeignKeySecondColumnInForeignTable.Length == 0)
                                    ForeignKeySecondColumnInForeignTable = Q.ForeignKeySecondColumn;
                                JoinDuplicateCheck = JoinClause + Q.IntermediateTable + " ITy ON " +
                                    MainTableAlias + "." + Q.IdentityColumn + " = ITy." + Q.IdentityColumn +
                                    JoinClause + "[" + Q.Table + "] Ty ON " +
                                    "ITy." + Q.ForeignKey + " = Ty." + Q.ForeignKey;
                                if (Q.Restriction.Length > 0)
                                    JoinDuplicateCheck += " AND Ty." + Q.Restriction;
                                if (!Tables.TryGetValue(JoinDuplicateCheck, out TableAlias))
                                {
                                    iT++;
                                    if (Q.Table == MainTable) TableAlias = MainTableAlias;
                                    else TableAlias = "T" + iT.ToString();
                                    Tables.Add(JoinDuplicateCheck, TableAlias);
                                    TableLinks.Add("I" + TableAlias + "." + Q.ForeignKeySecondColumn, Q.IntermediateTable);
                                    Join = JoinClause + Q.IntermediateTable + " I" + TableAlias + " ON " +
                                        MainTableAlias + "." + Q.IdentityColumn + " = I" + TableAlias + "." + Q.IdentityColumn +
                                        JoinClause + " [" + Q.Table + "] " + TableAlias + " ON " +
                                        "I" + TableAlias + "." + Q.ForeignKey + " = " + TableAlias + "." + Q.ForeignKey;
                                    if (Q.Restriction.Length > 0)
                                        Join += " AND " + TableAlias + "." + Q.Restriction;
                                    Add = true;
                                }
                            }
                            else
                            {
                                JoinDuplicateCheck = JoinClause + " [" + Q.Table + "] Tx ON " + MainTableAlias + "." + Q.ForeignKey + " = Tx." + Q.ForeignKey;
                                if (Q.Restriction.Length > 0)
                                    JoinDuplicateCheck += " AND Tx." + Q.Restriction;
                                if (!Tables.TryGetValue(JoinDuplicateCheck, out TableAlias))
                                {
                                    if (TableList.Contains(Q.Table))
                                    {
                                        TableAlias = TableAliasList[TableList.IndexOf(Q.Table)];
                                    }
                                    else
                                    {
                                        iT++;
                                        if (Q.Table == MainTable) TableAlias = MainTableAlias;
                                        else TableAlias = "T" + iT.ToString();
                                        Tables.Add(JoinDuplicateCheck, TableAlias);
                                        //if (Q.ForeignKeySecondColumn.Length > 0)
                                        TableLinks.Add(TableAlias, Q.Table);
                                        Join = JoinClause + " [" + Q.Table + "] " + TableAlias + " ON " + MainTableAlias + "." + Q.ForeignKey + " = " + TableAlias + "." + Q.ForeignKey;
                                        if (Q.Restriction.Length > 0)
                                            Join += " AND " + TableAlias + "." + Q.Restriction;
                                        Add = true;
                                    }
                                }
                            }
                            //TableAlias = "T" + iT.ToString();
                            if (Add) FromClause += Join;
                        }

                        //Fields for reimport
                        if (this.checkBoxForReimport.Checked)
                        {
                            if (!TableAliasList.Contains(MainTableAlias) && !TableList.Contains(MainTable) && !FieldList.Contains("CollectionSpecimenID"))
                            {
                                TableList.Add(MainTable);
                                TableAliasList.Add(MainTableAlias);
                                FieldList.Add("CollectionSpecimenID");
                                FieldDescriptionList.Add("CollectionSpecimenID");
                                if (Fields.Length > 0) Fields += ", ";
                                Fields += MainTableAlias + "." + "CollectionSpecimenID";
                            }
                        }
                        bool ColumnMissing = true;
                        if (FieldList.Contains(Q.Column) && TableList.Contains(Q.Table))
                        {
                            for (int i = 0; i < FieldList.Count; i++)
                                if (FieldList[i] == Q.Column && TableList[i] == Q.Table && TableAliasList[i] == TableAlias) ColumnMissing = false;
                        }
                        if (ColumnMissing)
                        {
                            if (Fields.Length > 0) Fields += ", ";
                            if (Q.Column.IndexOf(".") > -1)
                            {
                                Fields += TableAlias + Q.Column.Substring(Q.Column.IndexOf("."));
                                this.comboBoxOrderBy.Items.Add(TableAlias + Q.Column.Substring(Q.Column.IndexOf(".")));
                            }
                            else
                            {
                                Fields += TableAlias + "." + Q.Column;
                                this.comboBoxOrderBy.Items.Add(TableAlias + "." + Q.Column);
                            }
                            FieldList.Add(Q.Column);

                            TableList.Add(Q.Table);
                            TableAliasList.Add(TableAlias);//"T" + iT.ToString());
                            FieldDescriptionList.Add(Q.DisplayLongText);
                            if (Q.IsDate && !Q.IsYear)
                            {
                                // Day
                                if (Fields.Length > 0) Fields += ", ";
                                Fields += TableAlias + "." + Q.DayColumn;
                                FieldList.Add(Q.DayColumn);
                                TableList.Add(Q.Table);
                                TableAliasList.Add(TableAlias);//"T" + iT.ToString());
                                FieldDescriptionList.Add(Q.DisplayLongText + ": Day");
                                // Month
                                if (Fields.Length > 0) Fields += ", ";
                                Fields += TableAlias + "." + Q.MonthColumn;
                                FieldList.Add(Q.MonthColumn);
                                TableList.Add(Q.Table);
                                TableAliasList.Add(TableAlias);//"T" + iT.ToString());
                                FieldDescriptionList.Add(Q.DisplayLongText + ": Month");
                                // Year
                                if (Fields.Length > 0) Fields += ", ";
                                Fields += TableAlias + "." + Q.YearColumn;
                                FieldList.Add(Q.YearColumn);
                                TableList.Add(Q.Table);
                                TableAliasList.Add(TableAlias);//"T" + iT.ToString());
                                FieldDescriptionList.Add(Q.DisplayLongText + ": Year");
                            }
                            if (Q.Table == "CollectionEventLocalisation" && Q.Column == "Location1" && Q.DisplayText != "Place")
                            {
                                if (Fields.Length > 0) Fields += ", ";
                                Fields += TableAlias + ".Location2";
                                FieldList.Add("Location2");
                                TableList.Add(Q.Table);
                                TableAliasList.Add(TableAlias);//"T" + iT.ToString());
                                FieldDescriptionList.Add(Q.DisplayLongText + " upper value");
                            }
                        }

                        // Fields for reimport
                        if (this.checkBoxForReimport.Checked)
                        {
                            if (Q.Table == MainTable)
                            {
                                if (!TableAliasList.Contains(MainTableAlias) || !TableList.Contains(MainTable))
                                    this.getPrimaryKey(Q.Table, MainTableAlias, ref Fields, ref FieldList, ref TableList, ref TableAliasList, ref FieldDescriptionList);
                            }
                            else //if (!TableAliasList.Contains(TableAlias))//"T" + iT.ToString()))
                            {
                                this.getPrimaryKey(Q.Table, TableAlias, ref Fields, ref FieldList, ref TableList, ref TableAliasList, ref FieldDescriptionList);
                            }
                        }

                    }
                }

                // IDs
                this.labelCurrentAction.Text = "Getting selected IDs for final query";
                Application.DoEvents();
                this.progressBarCurrentAction.Value = 0;
                this.progressBarCurrentAction.Maximum = this._QueryList.ListOfIDs.Count;
                this.progressBarCurrentAction.Visible = true;
                foreach (int i in this._QueryList.ListOfIDs)
                {
                    if (WhereClause.Length > 0) WhereClause += ", ";
                    WhereClause += i.ToString();
                    if (this.progressBarCurrentAction.Value < this.progressBarCurrentAction.Maximum)
                        this.progressBarCurrentAction.Value++;
                }
                this.labelCurrentAction.Text = "";
                this.progressBarCurrentAction.Visible = false;
                Application.DoEvents();

                WhereClause = " WHERE " + MainTableAlias + ".CollectionSpecimenID IN (" + WhereClause + ")";

                foreach (System.Collections.Generic.KeyValuePair<string, string> k in TableLinks)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> i in TableLinks)
                    {
                        if (k.Value == i.Value)
                        {
                            string KeyColumn = "";
                            string iTable = i.Key;
                            string kTable = k.Key;
                            if (k.Key.IndexOf(".") > -1)
                            {
                                KeyColumn = k.Key.Substring(k.Key.IndexOf(".") + 1);
                                kTable = k.Key.Substring(0, k.Key.IndexOf("."));
                            }
                            else if (i.Key.IndexOf(".") > -1)
                            {
                                KeyColumn = i.Key.Substring(i.Key.IndexOf(".") + 1);
                                iTable = i.Key.Substring(0, i.Key.IndexOf("."));
                            }
                            if (KeyColumn.Length > 0)
                            {
                                if (iTable.IndexOf(".") > -1) iTable = iTable.Substring(0, iTable.IndexOf("."));
                                if (iTable != kTable)
                                    WhereClause += " AND " + iTable + "." + KeyColumn + " = " + kTable + "." + KeyColumn;
                                break;
                            }
                        }
                    }
                }

                if (!TableList.Contains(MainTable) || !FromClause.StartsWith("FROM"))
                {
                    FromClause = "FROM " + MainTable + " " + MainTableAlias + " " + FromClause;
                }

                if (this.checkBoxForReimport.Checked && !FieldList.Contains("CollectionSpecimenID"))
                {
                    TableList.Add(MainTable);
                    TableAliasList.Add(MainTableAlias);
                    FieldList.Add("CollectionSpecimenID");
                    FieldDescriptionList.Add("CollectionSpecimenID");
                    if (Fields.Length > 0) Fields += ", ";
                    Fields += MainTableAlias + "." + "CollectionSpecimenID";
                }


                SQL = "SELECT DISTINCT " + Fields + " " + FromClause + WhereClause;
                if (OrderColumn.Length > 0) SQL += " ORDER BY " + OrderColumn;

                this.labelCurrentAction.Text = "Retrieving Data from database";
                Application.DoEvents();

                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionStringWithTimeout(DiversityWorkbench.Settings.TimeoutDatabase));
                ad.SelectCommand.ResetCommandTimeout();
                ad.SelectCommand.CommandTimeout = DiversityWorkbench.Settings.TimeoutDatabase;
                ad.Fill(dt);

                this.labelCurrentAction.Text = "";
                Application.DoEvents();

                if (System.IO.File.Exists(this.textBoxFilePath.Text))
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(this.textBoxFilePath.Text);
                    try
                    {
                        //fi.Delete();
                    }
                    catch { }
                }
                string line = "";
                foreach (string s in FieldDescriptionList)
                {
                    line += s + "\t";
                }
                sw.WriteLine(line);
                sw.WriteLine();
                line = "";
                foreach (string s in TableList)
                {
                    line += s + "\t";
                }
                sw.WriteLine(line);
                line = "";
                foreach (string s in FieldList)
                {
                    line += s + "\t";
                }
                sw.WriteLine(line);
                if (this.checkBoxForReimport.Checked)
                {
                    line = "";
                    foreach (string s in TableAliasList)
                    {
                        line += s + "\t";
                    }
                    sw.WriteLine(line);
                }
                sw.WriteLine();
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    line = "";
                    for (int i = 0; i < R.ItemArray.Length; i++)
                    {
                        if (R[i].Equals(System.DBNull.Value))
                            line += "\t";
                        else
                            line += R[i].ToString() + "\t";
                    }
                    sw.WriteLine(line);
                }
                //if (this.checkBoxIncludeSQL.Checked)
                //{
                //    sw.WriteLine();
                //    sw.WriteLine();
                //    sw.WriteLine("SQL-Query");
                //    sw.WriteLine();
                //    sw.WriteLine(SQL);
                //}
                System.Windows.Forms.MessageBox.Show(dt.Rows.Count.ToString() + " datasets \r\nexported to \r\n" + this.textBoxFilePath.Text);
                sw.Close();
                sw.Dispose();
                this.GetResult(SQL);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                sw.Close();
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void getPrimaryKey(
            string Table,
            string TableAlias,
            ref string Fields,
            ref System.Collections.Generic.List<string> FieldList,
            ref System.Collections.Generic.List<string> TableList,
            ref System.Collections.Generic.List<string> TableAliasList,
            ref System.Collections.Generic.List<string> FieldDescriptionList
        )
        {
            DiversityCollection.Datasets.DataSetCollectionSpecimen ds = new DiversityCollection.Datasets.DataSetCollectionSpecimen();
            string TableInDataset = Table;
            if (!ds.Tables.Contains(TableInDataset) && TableInDataset.Contains("_"))
                TableInDataset = TableInDataset.Substring(0, TableInDataset.IndexOf("_"));
            if (ds.Tables.Contains(TableInDataset))
                Table = TableInDataset;
            if (ds.Tables.Contains(Table))
            {
                System.Data.DataTable dt = ds.Tables[Table].Copy();
                System.Data.DataColumn[] dc = dt.PrimaryKey;
                foreach (System.Data.DataColumn DC in dc)
                {
                    if (!FieldList.Contains(DC.ColumnName))
                    {
                        TableList.Add(DC.Table.TableName);
                        TableAliasList.Add(TableAlias);
                        FieldList.Add(DC.ColumnName);
                        FieldDescriptionList.Add(DC.ColumnName);
                        if (Fields.Length > 0) Fields += ", ";
                        Fields += TableAlias + "." + DC.ColumnName;
                    }
                    else
                    {
                        bool OK = false;
                        for (int i = 0; i < FieldList.Count; i++)
                        {
                            if (DC.ColumnName == FieldList[i] && TableAlias == TableAliasList[i])
                            {
                                OK = true;
                                break;
                            }
                        }
                        if (!OK)
                        {
                            TableList.Add(DC.Table.TableName);
                            TableAliasList.Add(TableAlias);
                            FieldList.Add(DC.ColumnName);
                            FieldDescriptionList.Add(DC.ColumnName);
                            if (Fields.Length > 0) Fields += ", ";
                            Fields += TableAlias + "." + DC.ColumnName;
                        }
                    }
                }
            }
            else
            {

            }
        }

        private void GetResult(string SQL)
        {
            try
            {
                using (System.IO.StreamReader sr = new System.IO.StreamReader(this.textBoxFilePath.Text))
                {
                    String line;
                    this.textBoxResult.Text = "";
                    decimal d = 0;
                    while ((line = sr.ReadLine()) != null && d < this.numericUpDownPreview.Value)
                    {
                        this.textBoxResult.Text += line + "\r\n";
                        d++;
                        if (d == this.numericUpDownPreview.Value)
                            this.textBoxResult.Text += "...\r\n";
                    }
                    if (this.checkBoxIncludeSQL.Checked)
                    {
                        this.textBoxResult.Text += "\r\n";
                        this.textBoxResult.Text += "\r\n";
                        this.textBoxResult.Text += "SQL-Query\r\n";
                        this.textBoxResult.Text += "\r\n";
                        this.textBoxResult.Text += SQL + "\r\n";
                    }
                }
            }
            catch { }
        }
        
        private void buttonTimeout_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetInteger f = new DiversityWorkbench.Forms.FormGetInteger(DiversityWorkbench.Settings.TimeoutDatabase, "Timeout", "Please enter the timeout in seconds");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                DiversityWorkbench.Settings.TimeoutDatabase = (int)f.Integer;
        }

        #endregion

        #region Properties
        internal DiversityCollection.CollectionSpecimen CollectionSpecimen
        {
            get
            {
                DiversityCollection.CollectionSpecimen C = new CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                return C;
            }
        }
        
        #endregion

    }
}