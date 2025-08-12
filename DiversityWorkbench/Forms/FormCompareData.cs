using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormCompareData : Form
    {

        #region Parameter
        
        System.Collections.Generic.Dictionary<string, Microsoft.Data.SqlClient.SqlDataAdapter> _TableAdapter;
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _DataDifferences;
        System.Collections.Generic.Dictionary<string, System.Windows.Forms.TabPage> _TabPages;
        System.Collections.Generic.Dictionary<string, System.Windows.Forms.DataGridView> _DataGrids;
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _KeyColumns;
        System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _KeyTextColumns;
        
        #endregion

        #region Construction
        
        public FormCompareData(string Title, System.Collections.Generic.Dictionary<string, Microsoft.Data.SqlClient.SqlDataAdapter> TableAdapter)
        {
            InitializeComponent();
            this.labelHeader.Text = Title;
            this._TableAdapter = TableAdapter;
            this.initForm();
        }
        
        #endregion

        #region Form

        private void initForm()
        {
            this.ReadData();
        }
        
        public void setHelpProvider(string HelpNamespace, string Keyword)
        {
            this.helpProvider.HelpNamespace = HelpNamespace;
            this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
            this.helpProvider.SetHelpKeyword(this, Keyword);
        }

        private void buttonRequery_Click(object sender, EventArgs e)
        {
            this.ReadData();
            this.FormatTabControl();
        }

        //private bool RestrictToDifferences = false;

        private System.Collections.Generic.List<string> HiddenPages = new List<string>();

        private void ReadData()
        {
            try
            {
                this.tabControlTables.TabPages.Clear();
                this._DataDifferences = new Dictionary<string, List<string>>();
                this._DataGrids = new Dictionary<string, DataGridView>();
                this._TabPages = new Dictionary<string, TabPage>();
                this._KeyColumns = new Dictionary<string, List<string>>();
                this._KeyTextColumns = new Dictionary<string, List<string>>();
                foreach (System.Collections.Generic.KeyValuePair<string, Microsoft.Data.SqlClient.SqlDataAdapter> KV in this._TableAdapter)
                {
                    try
                    {
                        try
                        {
                            string SQL = "SELECT COLUMN_NAME, DATA_TYPE " +
                                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                                "WHERE (TABLE_NAME = '" + KV.Key + "') AND (EXISTS " +
                                "(SELECT * " +
                                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                                "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                            System.Data.DataTable dtKey = new DataTable();
                            Microsoft.Data.SqlClient.SqlDataAdapter adKeys = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, KV.Value.SelectCommand.Connection.ConnectionString);
                            adKeys.Fill(dtKey);
                            System.Collections.Generic.List<string> KeyColumns = new List<string>();
                            System.Collections.Generic.List<string> KeyTextColumns = new List<string>();
                            foreach (System.Data.DataRow R in dtKey.Rows)
                            {
                                KeyColumns.Add(R[0].ToString());
                                if (R[1].ToString().IndexOf("char") > -1 || R[1].ToString().IndexOf("date") > -1)
                                    KeyTextColumns.Add(R[0].ToString());

                            }
                            this._KeyColumns.Add(KV.Key, KeyColumns);
                            this._KeyTextColumns.Add(KV.Key, KeyTextColumns);

                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Getting Key columns for  " + KV.Key);
                        }

                        System.Data.DataTable dtColumns = new DataTable();
                        string SqlColums = "select C.COLUMN_NAME, C.DATA_TYPE from INFORMATION_SCHEMA.COLUMNS C " +
                            "where c.TABLE_NAME = '" + KV.Key + "' ";
                        Microsoft.Data.SqlClient.SqlDataAdapter adColumns = new Microsoft.Data.SqlClient.SqlDataAdapter(SqlColums, KV.Value.SelectCommand.Connection.ConnectionString);
                        adColumns.Fill(dtColumns);
                        string ColumnNames = "";
                        foreach (System.Data.DataRow R in dtColumns.Rows)
                        {
                            if (ColumnNames.Length > 0) ColumnNames += ", ";
                            ColumnNames += R[0].ToString();
                            if (R[1].ToString() == "geography" || R[1].ToString() == "geometry")
                                ColumnNames += ".ToString() AS " + R[0].ToString();
                        }
                        string SelectCommand = KV.Value.SelectCommand.CommandText;
                        SelectCommand = SelectCommand.Replace("*", ColumnNames);
                        KV.Value.SelectCommand.CommandText = SelectCommand;

                        System.Data.DataTable dt = new DataTable();
                        try
                        {
                            KV.Value.Fill(dt);
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Filling table  " + KV.Key);
                        }

                        if (dt.Rows.Count == 0)
                            continue;
                        try
                        {
                            System.Windows.Forms.TabPage T = new TabPage(KV.Key);
                            this.tabControlTables.TabPages.Add(T);
                            this._TabPages.Add(KV.Key, T);
                            System.Windows.Forms.DataGridView D = new DataGridView();
                            T.Controls.Add(D);
                            this._DataGrids.Add(KV.Key, D);
                            D.Dock = DockStyle.Fill;
                            D.DataSource = dt;
                            D.ReadOnly = true;
                            D.AllowUserToAddRows = false;
                            D.AllowUserToDeleteRows = false;
                            D.RowHeadersVisible = false;
                            System.Collections.Generic.List<string> Columns = new List<string>();
                            for (int c = 0; c < dt.Columns.Count; c++)
                            {
                                try
                                {
                                    if (dt.Columns[c].ColumnName == "RowGUID" || dt.Columns[c].ColumnName.StartsWith("Log") && (dt.Columns[c].ColumnName.EndsWith("When") || dt.Columns[c].ColumnName.EndsWith("By")))
                                    {
                                        D.Columns[c].ReadOnly = true;
                                        D.Columns[c].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                                    }
                                    else if (this._KeyColumns[KV.Key].Contains(dt.Columns[c].ColumnName) 
                                        && 
                                        (!this.checkBoxCompareKey.Checked
                                        ||
                                        (this.checkBoxCompareKey.Checked && !this._KeyTextColumns[KV.Key].Contains(dt.Columns[c].ColumnName))))
                                    {
                                        D.Columns[c].ReadOnly = true;
                                        D.Columns[c].DefaultCellStyle.BackColor = System.Drawing.Color.Yellow;
                                    }
                                    else
                                    {
                                        bool DifferenceFound = false;
                                        string Content = dt.Rows[0][c].ToString();
                                        for (int r = 1; r < dt.Rows.Count; r++)
                                        {
                                            if (dt.Rows[r][c].ToString() != Content)
                                            {
                                                DifferenceFound = true;
                                                Columns.Add(dt.Columns[c].ColumnName);
                                                break;
                                            }
                                        }
                                        if (DifferenceFound)
                                        {
                                            if (this._KeyTextColumns[KV.Key].Contains(dt.Columns[c].ColumnName))
                                                D.Columns[c].DefaultCellStyle.BackColor = System.Drawing.Color.PeachPuff;
                                            else
                                                D.Columns[c].DefaultCellStyle.BackColor = System.Drawing.Color.Pink;
                                        }
                                    }
                                }
                                catch (System.Exception ex)
                                {
                                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Getting column " + dt.Columns[c].ColumnName);
                                }
                            }
                            this._DataDifferences.Add(KV.Key, Columns);

                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Adding controls for " + KV.Key);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, "Creating controls for " + KV.Key);
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.DataGridView> KV in this._DataGrids)
                {
                    this.setHeaderText(KV.Value);
                    KV.Value.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void setHeaderText(System.Windows.Forms.DataGridView Grid)
        {
            try
            {
                foreach (System.Windows.Forms.DataGridViewColumn C in Grid.Columns)
                {
                    string Header = C.HeaderText;
                    string NewHeader = "";
                    for (int i = 0; i < Header.Length; i++)
                    {
                        if (i > 0)
                        {
                            if (Header.Length > i + 1)
                            {
                                if (Header[i].ToString() != Header[i].ToString().ToLower())
                                {
                                    if (Header.Length > i && Header[i - 1].ToString() == Header[i - 1].ToString().ToLower())
                                        NewHeader += " ";
                                }
                            }
                        }
                        NewHeader += Header[i];
                    }
                    C.HeaderText = NewHeader;
                }
            }
            catch(System.Exception ex)
            {

            }
        }

        private void FormatTabControl()
        {
            this.SuspendLayout();
            if (this.checkBoxRestrictToDifferences.Checked) //.RestrictToDifferences)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<string>> KV in this._DataDifferences)
                {
                    if (KV.Value.Count == 0)
                    {
                        if (!HiddenPages.Contains(KV.Key))
                            HiddenPages.Add(KV.Key);
                        this.tabControlTables.TabPages.Remove(this._TabPages[KV.Key]);
                    }
                    else
                    {
                        foreach (System.Windows.Forms.DataGridViewColumn C in this._DataGrids[KV.Key].Columns)
                        {
                            if (KV.Value.Contains(C.DataPropertyName) || this._KeyColumns[KV.Key].Contains(C.DataPropertyName))
                                C.Visible = true;
                            else C.Visible = false;
                        }
                    }
                }
            }
            else
            {
                foreach (string T in this.HiddenPages)
                {
                    this.tabControlTables.TabPages.Add(this._TabPages[T]);
                }
                this.HiddenPages.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.DataGridView> KV in this._DataGrids)
                {
                    foreach (System.Windows.Forms.DataGridViewColumn C in KV.Value.Columns)
                    {
                        C.Visible = true;
                    }
                }
            }
            this.ResumeLayout();
        }

        private void checkBoxCompareKey_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxCompareKey.Checked)
                this.checkBoxCompareKey.BackColor = System.Drawing.Color.PeachPuff;
            else
                this.checkBoxCompareKey.BackColor = System.Drawing.Color.Transparent;
            this.ReadData();
            this.FormatTabControl();
        }

        private void checkBoxRestrictToDifferences_CheckedChanged(object sender, EventArgs e)
        {
            //this.RestrictToDifferences = !this.RestrictToDifferences;
            try
            {
                if (this.checkBoxRestrictToDifferences.Checked)
                {
                    this.checkBoxRestrictToDifferences.BackColor = System.Drawing.Color.Pink;
                    //this.buttonRestrictToDifferences.Text = "Reset";
                    //this.buttonRestrictToDifferences.BackColor = System.Drawing.Color.Pink;
                }
                else
                {
                    this.checkBoxRestrictToDifferences.BackColor = System.Drawing.Color.Transparent;
                    //this.buttonRestrictToDifferences.Text = "Restrict to differences";
                    //this.buttonRestrictToDifferences.BackColor = System.Drawing.Color.Transparent;
                    //this.buttonRestrictToDifferences.Tag = null;
                }
                this.FormatTabControl();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void buttonSetColumnWith_Click(object sender, EventArgs e)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.DataGridView> KV in this._DataGrids)
            {
                //KV.Value.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                KV.Value.AutoResizeColumnHeadersHeight();
                //KV.Value.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
                KV.Value.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            //foreach (System.Collections.Generic.KeyValuePair<string, System.Windows.Forms.DataGridView> KV in this._DataGrids)
            //{
            //    KV.Value.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //}
        }

        #endregion

    }
}
