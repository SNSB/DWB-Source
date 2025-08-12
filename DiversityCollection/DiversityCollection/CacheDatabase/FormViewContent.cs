using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DiversityWorkbench.DwbManual;
using Npgsql;

namespace DiversityCollection.CacheDatabase
{
    public partial class FormViewContent : Form
    {
        #region Parameter

        private bool _ForPostgres;
        private string _Schema;
        private string _Package;
        private string _SourceTable = "";
        private bool _IncludeSchemaPublic = false;
        private string _DefaultFilterColumn = "";
        private string _DefaultFilterOperator = "";
        private string _DefaultFilterValue = "";
        
        #endregion

        #region Construction

        public FormViewContent(bool ForPostgres, string Schema)
        {
            InitializeComponent();
            this._ForPostgres = ForPostgres;
            this._Schema = Schema;
            this._Package = "";
            this.initForm();
        }

        public FormViewContent(bool ForPostgres, string Schema, string SourceTable, string WhereClauseColumn, string WhereClauseValue)
        {
            InitializeComponent();
            this._ForPostgres = ForPostgres;
            this._Schema = Schema;
            this._SourceTable = SourceTable;
            this._DefaultFilterColumn = WhereClauseColumn;
            this._DefaultFilterOperator = "=";
            this._DefaultFilterValue = WhereClauseValue;
            this.initForm();
        }



        public FormViewContent(bool ForPostgres, string Schema, string Package, string WhereClauseColumn = "", string WhereClauseOperator = "=", string WhereClauseValue = "")
        {
            InitializeComponent();
            this._ForPostgres = ForPostgres;
            this._Schema = Schema;
            this._Package = Package;
            this._DefaultFilterColumn = WhereClauseColumn;
            this._DefaultFilterOperator = WhereClauseOperator;
            this._DefaultFilterValue = WhereClauseValue;
            this.initForm();
        }

        public FormViewContent(bool ForPostgres, string Schema, string Package, bool IncludePublic)
        {
            InitializeComponent();
            this._ForPostgres = ForPostgres;
            this._Schema = Schema;
            this._Package = Package;
            this._IncludeSchemaPublic = IncludePublic;
            this.initForm();
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Form

        private void initForm()
        {
            try
            {
                this.labelMain.Text = "Data in schema " + this._Schema;
                if (this._Package != null && this._Package.Length > 0)
                {
                    // Markus 23.2.2021: Anzeige fuer CacheDB Sources
                    if (this._DefaultFilterColumn != null && this._DefaultFilterColumn.Length > 0 && this._DefaultFilterColumn == "SourceView"
                        && this._DefaultFilterValue != null && this._DefaultFilterValue.Length > 0)
                    {
                        this.labelMain.Text += " in source " + this._Package + " - " + this._DefaultFilterValue;
                    }
                    else
                    {
                        this.labelMain.Text += " in package " + this._Package;
                    }
                }
                if (this._ForPostgres)
                {
                    this.pictureBoxMain.Image = this.imageListDatabase.Images[1];
                    this.Text = "Content of postgres database " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                }
                else
                {
                    this.pictureBoxMain.Image = this.imageListDatabase.Images[0];
                    this.Text = "Content of cache database " + DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                }
                if (this._ForPostgres)
                {
                    try
                    {
                        bool ForWebservice = false;
                        // Tables
                        this.dataGridViewTables.DataSource = null;
                        string SQL = "select table_name from information_schema.tables P " +
                            "where P.table_type = 'BASE TABLE' " +
                            "and (P.table_schema = '" + this._Schema + "'";
                        if (this._IncludeSchemaPublic)
                            SQL += " or P.table_schema = 'public'";
                        SQL += ") ";
                        if (this._Package != null && this._Package.Length > 0)
                            SQL += " and table_name LIKE '" + this._Package + "%'";
                        if (this._SourceTable.Length > 0)
                        {
                            SQL += " and table_name = '" + this._SourceTable + "'";
                            if (this._DefaultFilterColumn.Length > 0 &&
                                this._DefaultFilterOperator.Length > 0 &&
                                this._DefaultFilterValue.Length > 0)
                            {
                                ForWebservice = true;
                                this.WhereClauses.Add(this._SourceTable, " WHERE \"" + this._DefaultFilterColumn + "\" " + this._DefaultFilterOperator + " '" + this._DefaultFilterValue + "'");
                            }
                        }
                        SQL += " order by table_name";
                        Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString()); // .Postgres.PostgresConnection().ConnectionString);
                        System.Data.DataTable dt = new DataTable();
                        ad.Fill(dt);
                        if (dt.Rows.Count == 0)
                        {
                            this.tabControlMain.TabPages.Remove(this.tabPageTables);
                        }
                        else
                        {
                            this.listBoxTables.DataSource = dt;
                            this.listBoxTables.DisplayMember = "table_name";
                            this.listBoxTables.ValueMember = "table_name";
                        }
                        if (ForWebservice)
                        {
                            this.tabControlMain.TabPages.Remove(this.tabPageMaterializedViews);
                            this.tabControlMain.TabPages.Remove(this.tabPageViewsPublic);
                            this.tabControlMain.TabPages.Remove(this.tabPageViews);
                            return;
                        }

                        // Views
                        SQL = "select table_name from information_schema.tables P " +
                            "where P.table_type = 'VIEW' " +
                            "and P.table_schema = '" + this._Schema + "'";
                        if (this._Package.Length > 0)
                            SQL += " and table_name LIKE '" + this._Package + "%'";
                        SQL += " order by table_name";
                        ad.SelectCommand.CommandText = SQL;
                        System.Data.DataTable dtViews = new DataTable();
                        ad.Fill(dtViews);
                        if (dtViews.Rows.Count == 0)
                        {
                            this.tabControlMain.TabPages.Remove(this.tabPageViews);
                        }
                        else
                        {
                            this.listBoxViews.DataSource = dtViews;
                            this.listBoxViews.DisplayMember = "table_name";
                            this.listBoxViews.ValueMember = "table_name";
                        }

                        // Views in public
                        if (this._IncludeSchemaPublic)
                        {
                            SQL = "select table_name from information_schema.tables P " +
                                "where P.table_type = 'VIEW' " +
                                "and (P.table_schema = 'public') ";
                            if (this._Package.Length > 0)
                                SQL += " and table_name LIKE '" + this._Package + "%'";
                            SQL += " order by table_name";
                            ad.SelectCommand.CommandText = SQL;
                            System.Data.DataTable dtPublicViews = new DataTable();
                            ad.Fill(dtPublicViews);
                            if (dtPublicViews.Rows.Count == 0)
                            {
                                this.tabControlMain.TabPages.Remove(this.tabPageViewsPublic);
                            }
                            else
                            {
                                this.listBoxPublicViews.DataSource = dtPublicViews;
                                this.listBoxPublicViews.DisplayMember = "table_name";
                                this.listBoxPublicViews.ValueMember = "table_name";
                            }
                        }
                        else
                        {
                            this.tabControlMain.TabPages.Remove(this.tabPageViewsPublic);
                        }

                        // Materialized views
                        SQL = "SELECT relname AS MaterializedView " +
                            "FROM   pg_class " +
                            "WHERE  relkind = 'm' " +
                            "and relname like '" + this._Package + "_%' " +
                            "and (oid::regclass::text like '\"" + this._Schema + "\".\"%'";
                        if (this._IncludeSchemaPublic)
                            SQL += " or oid::regclass::text like '\"public\".\"%'";
                        SQL += ") order by relname";
                        ad.SelectCommand.CommandText = SQL;
                        System.Data.DataTable dtMatViews = new DataTable();
                        ad.Fill(dtMatViews);
                        if (dtMatViews.Rows.Count == 0)
                        {
                            this.tabControlMain.TabPages.Remove(this.tabPageMaterializedViews);
                        }
                        else
                        {
                            this.listBoxMaterializedViews.DataSource = dtMatViews;
                            this.listBoxMaterializedViews.DisplayMember = "MaterializedView";
                            this.listBoxMaterializedViews.ValueMember = "MaterializedView";
                        }
                    }
                    catch (System.Exception ex)
                    { }
                }
                else
                {
                    this.tabControlMain.TabPages.Remove(this.tabPageMaterializedViews);
                    this.tabControlMain.TabPages.Remove(this.tabPageViewsPublic);
                    string Message = "";
                    string SQL = "select T.TABLE_NAME from INFORMATION_SCHEMA.TABLES T " +
                        "where T.TABLE_SCHEMA = '" + this._Schema + "' " +
                        "and T.TABLE_TYPE = 'BASE TABLE'";
                    if (this._Package.Length > 0)
                        SQL += " and table_name LIKE '" + this._Package + "%'";
                    SQL += " order by T.table_name";
                    System.Data.DataTable dt = new DataTable();
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message))
                    {
                        if (dt.Rows.Count == 0)
                        {
                            this.tabControlMain.TabPages.Remove(this.tabPageTables);
                        }
                        else
                        {
                            this.listBoxTables.DataSource = dt;
                            this.listBoxTables.DisplayMember = "table_name";
                            this.listBoxTables.ValueMember = "table_name";
                        }
                    }
                    SQL = "select T.TABLE_NAME from INFORMATION_SCHEMA.TABLES T " +
                        "where T.TABLE_SCHEMA = '" + this._Schema + "' " +
                        "and T.TABLE_TYPE = 'VIEW'";
                    if (this._Package.Length > 0)
                        SQL += " and table_name LIKE '" + this._Package + "%'";
                    SQL += " order by T.table_name";
                    System.Data.DataTable dtViews = new DataTable();
                    if (DiversityWorkbench.Settings.LoadConnections) // Markus 31.10.22: greift auf Linked Server zu - dauert ewig bei langsamer Leitung
                    {
                        if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtViews, ref Message))
                        {
                            if (dtViews.Rows.Count == 0)
                            {
                                this.tabControlMain.TabPages.Remove(this.tabPageViews);
                            }
                            else
                            {
                                this.listBoxViews.DataSource = dtViews;
                                this.listBoxViews.DisplayMember = "table_name";
                                this.listBoxViews.ValueMember = "table_name";
                            }
                        }
                    }
                    else
                    {
                        // ToDo ...
                        this.tabControlMain.TabPages.Remove(this.tabPageViews);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        this.listBoxTables_SelectedIndexChanged(null, null);
                    }
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                this._DtColumnDescription = null;

                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxViews.SelectedItem;
                this.FilteredObject = R[0].ToString();
                this._ObjectType = FormSetFilter.ObjectType.View;
                string SQL = "";
                System.Data.DataTable dt = new DataTable();
                if (this._ForPostgres)
                {
                    SQL = "select * from \"" + this._Schema + "\".\"" + R[0].ToString() + "\" ";
                    if (this._WhereClauses.ContainsKey(R[0].ToString()))
                        SQL += this._WhereClauses[R[0].ToString()];
                    SQL += " LIMIT " + this.numericUpDownTopLimit.Value.ToString();
                    Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());// .Postgres.PostgresConnection().ConnectionString);
                    ad.Fill(dt);
                    SQL = "select count(*) from \"" + this._Schema + "\".\"" + R[0].ToString() + "\"";
                    this.textBoxTotal.Text = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, 3).ToString();// some views need to long, Timeout set to 3 seconds
                }
                else
                {
                    SQL = "select TOP " + this.numericUpDownTopLimit.Value.ToString() + " * from " + this._Schema + "." + R[0].ToString();
                    if (this._WhereClauses.ContainsKey(R[0].ToString()))
                        SQL += " " + this._WhereClauses[R[0].ToString()];
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    SQL = "select count(*) from " + this._Schema + "." + R[0].ToString();
                    this.textBoxTotal.Text = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                }
                this.dataGridViewViews.DataSource = dt;
                this.textBoxFiltered.Text = dt.Rows.Count.ToString();
                this.SetDescription(this.splitContainerViewContent, this.textBoxViewDescription, this._Schema, R[0].ToString(), "VIEW");
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                this.dataGridViewViews.DataSource = null;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        private void listBoxTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                this._DtColumnDescription = null;

                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxTables.SelectedItem;
                this.FilteredObject = R[0].ToString();
                this._ObjectType = FormSetFilter.ObjectType.Table;
                System.Data.DataTable dt = new DataTable();
                string SQL = "";
                if (this._ForPostgres)
                {
                    SQL = "select * from \"" + this._Schema + "\".\"" + R[0].ToString() + "\" ";
                    if (this._WhereClauses.ContainsKey(R[0].ToString()))
                        SQL += this._WhereClauses[R[0].ToString()];
                    SQL += " LIMIT " + this.numericUpDownTopLimit.Value.ToString();
                    Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());// .Postgres.PostgresConnection().ConnectionString);
                    ad.Fill(dt);
                    SQL = "select count(*) from \"" + this._Schema + "\".\"" + R[0].ToString() + "\"";
                    this.textBoxTotal.Text = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL).ToString();
                }
                else
                {
                    SQL = "select TOP " + this.numericUpDownTopLimit.Value.ToString() + " * from [" + this._Schema + "]." + R[0].ToString();
                    if (this._WhereClauses.ContainsKey(R[0].ToString()))
                        SQL += " " + this._WhereClauses[R[0].ToString()];
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    SQL = "select count(*) from " + this._Schema + "." + R[0].ToString();
                    this.textBoxTotal.Text = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                }
                this.dataGridViewTables.DataSource = dt;
                this.textBoxFiltered.Text = dt.Rows.Count.ToString();
                this.SetDescription(this.splitContainerTableContent, this.textBoxTableDescription, this._Schema, R[0].ToString(), "TABLE");
            }
            catch (System.Exception ex)
            {
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        private void listBoxMaterializedViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                this._DtColumnDescription = null;

                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxMaterializedViews.SelectedItem;
                this.FilteredObject = R[0].ToString();
                this._ObjectType = FormSetFilter.ObjectType.Table;
                System.Data.DataTable dt = new DataTable();
                string SQL = "";
                if (this._ForPostgres)
                {
                    SQL = "select * from \"" + this._Schema + "\".\"" + R[0].ToString() + "\" ";
                    if (this._WhereClauses.ContainsKey(R[0].ToString()))
                        SQL += this._WhereClauses[R[0].ToString()];
                    SQL += " LIMIT " + this.numericUpDownTopLimit.Value.ToString();
                    Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());// .Postgres.PostgresConnection().ConnectionString);
                    ad.Fill(dt);
                    SQL = "select count(*) from \"" + this._Schema + "\".\"" + R[0].ToString() + "\"";
                    this.textBoxTotal.Text = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL).ToString();
                }
                else
                {
                    SQL = "select TOP " + this.numericUpDownTopLimit.Value.ToString() + " * from [" + this._Schema + "]." + R[0].ToString();
                    if (this._WhereClauses.ContainsKey(R[0].ToString()))
                        SQL += " " + this._WhereClauses[R[0].ToString()];
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    SQL = "select count(*) from " + this._Schema + "." + R[0].ToString();
                    this.textBoxTotal.Text = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                }
                this.dataGridViewMaterializedViews.DataSource = dt;
                this.textBoxFiltered.Text = dt.Rows.Count.ToString();
                this.SetDescription(this.splitContainerTableContent, this.textBoxMaterializedViews, this._Schema, R[0].ToString(), "MATERIALIZED VIEW");
            }
            catch (System.Exception ex)
            {
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;

        }

        private void listBoxPublicViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                // Reset descriptions
                this._DtColumnDescription = null;
                // get view
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPublicViews.SelectedItem;
                this.FilteredObject = R[0].ToString();
                this._ObjectType = FormSetFilter.ObjectType.View;
                string SQL = "";
                System.Data.DataTable dt = new DataTable();
                if (this._ForPostgres)
                {
                    SQL = "select * from \"public\".\"" + R[0].ToString() + "\" ";
                    if (this._WhereClauses.ContainsKey(R[0].ToString()))
                        SQL += this._WhereClauses[R[0].ToString()];
                    SQL += " LIMIT " + this.numericUpDownTopLimit.Value.ToString();
                    Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());// .Postgres.PostgresConnection().ConnectionString);
                    ad.Fill(dt);
                    SQL = "select count(*) from \"public\".\"" + R[0].ToString() + "\"";
                    this.textBoxTotal.Text = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, 3).ToString();// some views need to long, Timeout set to 3 seconds
                }
                else
                {
                    SQL = "select TOP " + this.numericUpDownTopLimit.Value.ToString() + " * from public." + R[0].ToString();
                    if (this._WhereClauses.ContainsKey(R[0].ToString()))
                        SQL += " " + this._WhereClauses[R[0].ToString()];
                    string Message = "";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
                    SQL = "select count(*) from public." + R[0].ToString();
                    this.textBoxTotal.Text = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                }
                this.dataGridViewPublicViews.DataSource = dt;
                this.textBoxFiltered.Text = dt.Rows.Count.ToString();
                this.SetDescription(this.splitContainerPublicViewsData, this.textBoxPublicViews, "public", R[0].ToString(), "VIEW");
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                this.dataGridViewPublicViews.DataSource = null;
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void SetDescription(System.Windows.Forms.SplitContainer Split, System.Windows.Forms.TextBox TextBox, string Schema, string Object, string ObjectType)
        {
            string SQL = "";
            string Description = "";
            if (this._ForPostgres)
            {
                string Message = "";
                SQL = "SELECT d.description " +
                    "FROM pg_class As c " +
                    "LEFT JOIN pg_namespace n ON n.oid = c.relnamespace " +
                    "LEFT JOIN pg_tablespace t ON t.oid = c.reltablespace " +
                    "LEFT JOIN pg_description As d ON (d.objoid = c.oid AND d.objsubid = 0) " +
                    "WHERE n.nspname = '" + Schema + "' AND d.description > '' AND c.relname = '" + Object + "';";
                Description = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message, false, true);
            }
            else
            {
                SQL = "SELECT max(CONVERT(nvarchar(MAX), [value])) " +
                " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', '" + ObjectType + "', '" + Object +
                "', default, NULL) WHERE name =  'MS_Description'--[::fn_listextendedproperty_1]";

            }
            if (Description.Length > 0)
            {
                TextBox.Text = Description;
                Split.Panel2Collapsed = false;
                this.setDescriptionForColumn(Split, null, null,null, null, null, null);
            }
            else Split.Panel2Collapsed = true;
        }

        private void setDescriptionForColumn(System.Windows.Forms.SplitContainer SplitContainer, System.Windows.Forms.Label Label, System.Windows.Forms.TextBox TextBox, string Schema, string Object, string ObjectType, string ColumnName)
        {
            if (this._ForPostgres)
            {
                string Description = this.ColumnDescription(Schema, Object, ColumnName);
                if (Description.Length > 0)
                {
                    Label.Text = ColumnName;
                    TextBox.Text = Description;
                    //SplitContainer.Panel2Collapsed = false;
                }
                //else
                //{
                //    SplitContainer.Panel2Collapsed = true;
                //}
                if(Object != null) // called by change of main list
                {
                    if (Description.Length > 0)
                        SplitContainer.Panel2Collapsed = false;
                    else
                        SplitContainer.Panel2Collapsed = true;
                }
                else
                {
                    foreach(System.Windows.Forms.Control C in SplitContainer.Panel2.Controls)
                    {
                        if (C.GetType() == typeof(System.Windows.Forms.SplitContainer))
                        {
                            System.Windows.Forms.SplitContainer S = (System.Windows.Forms.SplitContainer)C;
                            S.Panel2Collapsed = true;
                            //foreach (System.Windows.Forms.Control cc in S.Panel2.Controls)
                            //{
                            //    if (cc.GetType() == typeof(System.Windows.Forms.SplitContainer))
                            //    {
                            //        System.Windows.Forms.SplitContainer ss = (System.Windows.Forms.SplitContainer)cc;
                            //        ss.Panel2Collapsed = true;
                            //    }
                            //}
                        }
                    }
                }
            }
        }
        
        private System.Data.DataTable _DtColumnDescription;
        private System.Data.DataTable DtColumnDescription(string Schema, string Object)
        {
            if (this._DtColumnDescription == null)
            {
                this._DtColumnDescription = new DataTable();
                if (this._ForPostgres)
                {
                    string SQL = "SELECT " +
                        "cols.column_name, " +
                        "(" +
                        "SELECT " +
                        "pg_catalog.col_description(c.oid, cols.ordinal_position::int) " +
                        "FROM " +
                        "pg_catalog.pg_class c " +
                        "WHERE " +
                        "c.oid = (SELECT ('\"' || cols.table_name || '\"')::regclass::oid) " +
                        "AND c.relname = cols.table_name " +
                        "AND c.relname = cols.table_name " +
                        ") AS column_comment " +
                        "FROM " +
                        "information_schema.columns cols " +
                        "WHERE cols.table_name   = '" + Object + "' " +
                        "AND cols.table_schema = '" + Schema + "';";
                    string Message = "";
                    DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref this._DtColumnDescription, ref Message);
                }
            }
            return this._DtColumnDescription;
        }

        public string ColumnDescription(string Schema, string Object, string ColumnName)
        {
            string Description = "";
            if (Object != null && this.DtColumnDescription(Schema, Object).Rows.Count > 0)
            {
                System.Data.DataRow[] rr = this.DtColumnDescription(Schema, Object).Select("column_name = '" + ColumnName + "'");
                if (rr.Length == 1)
                    Description = rr[0]["column_comment"].ToString();
            }
            return Description;
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }

        private string _FilteredObject = "";

        public string FilteredObject
        {
            get 
            {
                this.setDefaultFilter();
                return _FilteredObject; 
            }
            set 
            { 
                _FilteredObject = value;
                this.setDefaultFilter();
                if (this.WhereClauses.ContainsKey(value))
                    this.labelFilter.Text = this.WhereClauses[value];
                else this.labelFilter.Text = "";
            }
        }

        private void setDefaultFilter()
        {
            if (this._FilteredObject.Length > 0
                && !this.WhereClauses.ContainsKey(this._FilteredObject)
                && !_ForPostgres
                && this._DefaultFilterColumn.Length > 0
                && this._DefaultFilterOperator.Length > 0
                && this._DefaultFilterValue.Length > 0)
            {
                string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.COLUMN_NAME = '" + this._DefaultFilterColumn + "' AND C.TABLE_NAME = '" + _FilteredObject + "'";
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result == "1")
                {
                    this.WhereClauses.Add(this._FilteredObject, " WHERE " + this._DefaultFilterColumn + " " + this._DefaultFilterOperator + " '" + this._DefaultFilterValue + "'");
                }
            }
        }

        private System.Collections.Generic.Dictionary<string, string> _WhereClauses;

        public System.Collections.Generic.Dictionary<string, string> WhereClauses
        {
            get 
            { 
                if (this._WhereClauses == null)
                    this._WhereClauses = new Dictionary<string, string>();
                return _WhereClauses; 
            }
            //set { _WhereClauses = value; }
        }

        private FormSetFilter.ObjectType _ObjectType = FormSetFilter.ObjectType.Table;

        private void buttonFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._FilteredObject.Length > 0)
                {
                    if (!this.WhereClauses.ContainsKey(this._FilteredObject))
                        this.WhereClauses.Add(this._FilteredObject, "");
                }
                DiversityCollection.CacheDatabase.FormSetFilter f = new FormSetFilter(this._FilteredObject, this._ObjectType, this._Schema, this._WhereClauses[this._FilteredObject], this._ForPostgres);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    this._WhereClauses[this._FilteredObject] = f.WhereClause();
                    switch (this._ObjectType)
                    {
                        case FormSetFilter.ObjectType.Table:
                            this.listBoxTables_SelectedIndexChanged(null, null);
                            break;
                        case FormSetFilter.ObjectType.View:
                            this.listBoxViews_SelectedIndexChanged(null, null);
                            break;
                    }
                }
                this.labelFilter.Text = this._WhereClauses[this._FilteredObject];
            }
            catch (System.Exception ex)
            { }
        }

        private void dataGridViewMaterializedViews_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridViewMaterializedViews.SelectedCells.Count == 0)
                    return;
                System.Data.DataTable dt = (System.Data.DataTable)this.dataGridViewMaterializedViews.DataSource;
                string Column = dt.Columns[this.dataGridViewMaterializedViews.SelectedCells[0].ColumnIndex].ColumnName;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxMaterializedViews.SelectedItem;
                string Table = R[0].ToString();
                this.setDescriptionForColumn(this.splitContainerMaterializedViewDescription, this.labelMaterializedViewColumnDescription, this.textBoxMaterializedViewColumnDescription, "public", Table, "VIEW", Column);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void dataGridViewViews_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridViewViews.SelectedCells.Count == 0)
                    return;
                System.Data.DataTable dt = (System.Data.DataTable)this.dataGridViewViews.DataSource;
                string Column = dt.Columns[this.dataGridViewViews.SelectedCells[0].ColumnIndex].ColumnName;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxViews.SelectedItem;
                string Table = R[0].ToString();
                this.setDescriptionForColumn(this.splitContainerViewDescription, this.labelViewColumnDescription, this.textBoxViewColumnDescription, this._Schema, Table, "VIEW", Column);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void dataGridViewTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridViewTables.SelectedCells.Count == 0)
                    return;
                System.Data.DataTable dt = (System.Data.DataTable)this.dataGridViewTables.DataSource;
                string Column = dt.Columns[this.dataGridViewTables.SelectedCells[0].ColumnIndex].ColumnName;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxTables.SelectedItem;
                string Table = R[0].ToString();
                this.setDescriptionForColumn(this.splitContainerTableDescription, this.labelTableColumnDescription, this.textBoxTableColumnDescription, this._Schema, Table, "TABLE", Column);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void dataGridViewPublicViews_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this.dataGridViewPublicViews.SelectedCells.Count == 0)
                    return;
                System.Data.DataTable dt = (System.Data.DataTable)this.dataGridViewPublicViews.DataSource;
                string Column = dt.Columns[this.dataGridViewPublicViews.SelectedCells[0].ColumnIndex].ColumnName;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPublicViews.SelectedItem;
                string Table = R[0].ToString();
                this.setDescriptionForColumn(this.splitContainerPublicViewDescription, this.labelPublicViewColumnDescription, this.textBoxPublicViewColumnDescription, "public", Table, "VIEW", Column);
            }
            catch (System.Exception ex)
            {
            }
        }

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.tabControlMain.SelectedTab.Name)
            {
                case "tabPageViews":
                    this.listBoxViews_SelectedIndexChanged(null, null);
                    break;
                case "tabPageTables":
                    this.listBoxTables_SelectedIndexChanged(null, null);
                    break;
                case "tabPageMaterializedViews":
                    this.listBoxMaterializedViews_SelectedIndexChanged(null, null);
                    break;
                case "tabPageViewsPublic":
                    this.listBoxPublicViews_SelectedIndexChanged(null, null);
                    break;
            }
        }

        #endregion

        #region Logging
        private void radioButtonTableUnlogged_Click(object sender, EventArgs e)
        {

        }

        private void radioButtonTableLogged_Click(object sender, EventArgs e)
        {

        }

        private void setLogging(bool SetLogged)
        {
            string SQL = "SELECT relname FROM pg_class " +
                "WHERE relpersistence ";
            if (SetLogged) SQL += "=";
            else SQL += "<>";
            SQL += " 'u' " +
                "and relkind in ('r', 'i') " +
                "and relname not like 'pg_toast_%' " +
                "order by relname; ";
        }

        #endregion

        #region Description

        private void buttonPublicViewGenerateDescription_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> DD = new Dictionary<string, Dictionary<string, string>>();
                System.Collections.Generic.Dictionary<string, string> Dtab = new Dictionary<string, string>();
                foreach (System.Object O in this.listBoxPublicViews.Items)
                {
                    System.Data.DataRowView Rtab = (System.Data.DataRowView)O;
                    string Object = Rtab[0].ToString();
                    string SQL = "SELECT d.description " +
                        "FROM pg_class As c " +
                        "LEFT JOIN pg_namespace n ON n.oid = c.relnamespace " +
                        "LEFT JOIN pg_tablespace t ON t.oid = c.reltablespace " +
                        "LEFT JOIN pg_description As d ON (d.objoid = c.oid AND d.objsubid = 0) " +
                        "WHERE n.nspname = 'public' AND d.description > '' AND c.relname = '" + Object + "';";
                    string Description = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, false, true);
                    Dtab.Add(Object, Description);
                    this._DtColumnDescription = null;
                    System.Data.DataTable dt = this.DtColumnDescription("public", Object);
                    System.Collections.Generic.Dictionary<string, string> Dcol = new Dictionary<string, string>();
                    foreach (System.Data.DataRow Rcol in dt.Rows)
                    {
                        Dcol.Add(Rcol[0].ToString(), Rcol[1].ToString());
                    }
                    DD.Add(Object, Dcol);
                }
                string Result = "Descriptions for package " + this._Package + "\r\n";
                foreach(System.Collections.Generic.KeyValuePair<string, string> KVtab in Dtab)
                {
                    Result += "\r\n" + KVtab.Key + ": " + KVtab.Value + "\r\n";
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KVcol in DD[KVtab.Key])
                    {
                        if (KVcol.Value.IndexOf("Retrieved from") > -1)
                        {
                            Result += "\t" + KVcol.Key + ": " + KVcol.Value.Replace("Retrieved from", "\r\n\t\tRetrieved from") + "\r\n";//.Replace(" ABCD: ", "\r\n\t\t ABCD: ");
                        }
                        //if (KVcol.Value.IndexOf("Retrieved from") > -1)
                        //{
                        //    Result += "\t" + KVcol.Key + ": " + KVcol.Value.Substring(0, KVcol.Value.IndexOf("Retrieved from")) + "\r\n";
                        //    Result += "\t\t" + KVcol.Value.Substring(KVcol.Value.IndexOf("Retrieved from")) + "\r\n";
                        //}
                        //else
                        //{
                        //    //Result += "\t" + KVcol.Key + ": " + KVcol.Value + "\r\n";
                        //}
                    }
                }
                DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Description", Result, true);
                f.ShowDialog();
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}
