using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormDocumentation : Form
    {

        #region Parameter

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        /// <summary>
        /// the connection string may be set e.g. if not the main database should be documented but the cache database
        /// </summary>
        private string _ConnectionString = DiversityWorkbench.Settings.ConnectionString;
        private string _Schema = "dbo";
        private System.Data.DataTable _DtSchemata;
        private Microsoft.Data.SqlClient.SqlConnection _Conn;
        public SqlConnection Conn
        {
            get
            {
                if (_Conn == null)
                {
                    this._Conn = new SqlConnection(_ConnectionString);
                    this._Conn.Open();
                }
                return _Conn;
            }
        }

        public DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new FormFunctions(this, this._Conn.ToString(), ref this.toolTip);
                return _FormFunctions;
            }
            // set { _FormFunctions = value; }
        }
        private string _ServerVersion = "";
        Microsoft.SqlServer.Management.Smo.Database _Database;
        Microsoft.SqlServer.Management.Common.ServerConnection _Connection;
        System.Data.DataTable _dtCurrentTable;
        System.Data.DataTable _dtCurrentPK;
        System.Data.DataTable _dtCurrentRelation;
        System.Data.DataTable _dtCurrentConstraint;
        string _CurrentTable;
        string _CurrentPK;
        private string _Language;
        private bool? _EntityTablesExist;

        public bool EntityTablesExist
        {
            get
            {
                if (this._EntityTablesExist == null)
                    this._EntityTablesExist = DiversityWorkbench.Entity.EntityTablesExist;
                return (bool)_EntityTablesExist;
            }
            //set { _EntityTablesExist = value; }
        }

        #endregion

        #region Construction

        public FormDocumentation(string HelpNamespace, string ConnectionString = "")
        {
            try
            {
                InitializeComponent();
                if (ConnectionString.Length > 0)
                {
                    this._ConnectionString = ConnectionString;
                    this._Connection = new Microsoft.SqlServer.Management.Common.ServerConnection(this.Conn);
                    this._Connection.Connect();
                    Microsoft.SqlServer.Management.Smo.Server S = new Microsoft.SqlServer.Management.Smo.Server(this._Connection);
                    this._Database = S.Databases[DiversityWorkbench.Settings.DatabaseName];
                    string SQL = "select S.SCHEMA_NAME from INFORMATION_SCHEMA.SCHEMATA S " +
                        "where S.SCHEMA_NAME not like 'db[_]%' " +
                        "and S.SCHEMA_NAME <> 'guest' " +
                        "and S.SCHEMA_NAME <> 'sys' " +
                        "and S.SCHEMA_NAME <> 'INFORMATION_SCHEMA'";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.Conn);
                    this._DtSchemata = new DataTable();
                    ad.Fill(this._DtSchemata);
                    if (this._DtSchemata.Rows.Count > 1)
                    {
                        this.comboBoxSchema.DataSource = this._DtSchemata;
                        this.comboBoxSchema.DisplayMember = "SCHEMA_NAME";
                        this.comboBoxSchema.ValueMember = "SCHEMA_NAME";
                        this.labelSchema.Visible = true;
                        this.comboBoxSchema.Visible = true;
                    }
                }
                this.initForm();
                this.helpProvider.HelpNamespace = HelpNamespace;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public FormDocumentation()
        {
            InitializeComponent();
            this.initForm();
        }

        #endregion

        #region Form

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void initForm()
        {
            if (this.Parent != null)
            {
                this.StartPosition = FormStartPosition.CenterParent;
                this.Width = this.Parent.Width - 20;
                this.Height = this.Parent.Height - 20;
            }
            // TODO: falls fertig wieder einblenden
            this.buttonRequeryEntity.Visible = false;
#if !DEBUG
            //this.tabControlDocu.TabPages.Remove(this.tabPageMarkdown);
#endif

            this.checkBoxHtmlDarkmode.Visible = false;

            //this.checkBoxHtmlExcludeStandardTrigger.Visible = false;

            this.tabControlDocu.TabPages.Remove(this.tabPageContent);
            this.tabControlDocu.TabPages.Remove(this.tabPageXML);

            this.initContext();

            this.initHtml();

            if (!this.EntityTablesExist || DiversityWorkbench.Entity.DtContext.Rows.Count == 0)
            {
                this.setHtmlContextVisibility(false);
                this.groupBoxHtmlCss.Dock = DockStyle.Top;
                this.groupBoxHtmlCss.Height = 76;

                this.groupBoxMediaWikiContext.Visible = false;
            }
            else
            {
                this.setContext();
                this.setContextForMediaWiki();
                this.setLanguage();
                this.setLanguageForMediaWiki();
            }
            // Initialize Semantic media wiki
            this.comboBoxSMWikiTarget.SelectedIndex = 0;
            if (System.Windows.Forms.Application.ProductName.StartsWith("Diversity"))
            {
                string appName = System.Windows.Forms.Application.ProductName.Substring("Diversity".Length);
                if (appName.Length > 0)
                {
                    this.textBoxSMWikiScheme.Text = "dwb" + appName;
                    this.textBoxSMWikiPrefix.Text = "dwb" + appName[0];
                    for (int i = 1; i < appName.Length; i++)
                    {
                        if (char.IsUpper(appName[i]))
                            this.textBoxSMWikiPrefix.Text += appName[i];
                    }
                }
            }
            if (this.comboBoxSchema.Items.Count == 0)
            {
                this.labelSchema.Visible = false;
                this.comboBoxSchema.Visible = false;
            }
        }

        private void setHtmlContextVisibility(bool Visible)
        {
            if (Visible && this.tabControlHtmlOptions.TabPages.Contains(this.tabPageHtmlContext))
                this.tabControlHtmlOptions.TabPages.Remove(this.tabPageHtmlContext);
            else if (!Visible && !this.tabControlHtmlOptions.TabPages.Contains(this.tabPageHtmlContext))
                this.tabControlHtmlOptions.TabPages.Add(this.tabPageHtmlContext);
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                null,
                null);
        }

        private void FormDocumentation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_Conn != null)
            {
                if (_Conn.State == ConnectionState.Open)
                    _Conn.Close();
                _Conn.Dispose();
            }
            DiversityWorkbench.Forms.FormDocumentationSettings.Default.Save();
        }

        private void buttonHidePanel1_Click(object sender, EventArgs e)
        {
            if (this.splitContainerDocu.Panel1Collapsed == false)
            {
                this.splitContainerDocu.Panel1Collapsed = true;
                this.buttonHidePanel1.Image = DiversityWorkbench.Properties.Resources.ArrowRightSmall;
            }
            else
            {
                this.splitContainerDocu.Panel1Collapsed = false;
                this.buttonHidePanel1.Image = DiversityWorkbench.Properties.Resources.ArrowLeftSmall;
            }
        }

        #endregion

        #region Doku

        #region Server connection and Server Docu tables for columns, indices and constraints

        public Microsoft.SqlServer.Management.Smo.Database Database
        {
            get
            {
                if (this._Database == null)
                {
                    try
                    {
                        Microsoft.Data.SqlClient.SqlConnection SqlCon = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                        if (this._Connection == null)
                            this._Connection = new Microsoft.SqlServer.Management.Common.ServerConnection(SqlCon);
                        if (!this._Connection.IsOpen)
                            this._Connection.Connect();
                        Microsoft.SqlServer.Management.Smo.Server S = new Microsoft.SqlServer.Management.Smo.Server(this._Connection);
                        //if (S.Databases.Contains(DiversityWorkbench.Settings.DatabaseName))
                        this._Database = S.Databases[DiversityWorkbench.Settings.DatabaseName];
                    }
                    catch { }
                }
                if (!this._Connection.IsOpen)
                    this._Connection.Connect();
                return _Database;
            }
            //set { _Database = value; }
        }

        private System.Data.DataTable dtTableDocumentation(string Table, bool Index, bool Required)
        {
            // filling the local Docu table
            this._dtCurrentTable = new DataTable();
            this._CurrentTable = Table;
            string SQL = "";
            SQL = "SELECT COLUMN_NAME as ColumnName, DATA_TYPE as Datatype, CHARACTER_MAXIMUM_LENGTH as Length, " +
                "COLLATION_NAME  AS Collation, COLUMN_DEFAULT AS DefaultValue, '' AS Description ";
            if (Required) SQL += ", '-' AS Required ";
            if (Index) SQL += ", '-' AS Indices ";
            SQL += "FROM Information_Schema.COLUMNS  " +
                "WHERE Information_Schema.COLUMNS.TABLE_NAME = '" + Table + "'";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.Conn);
            try
            {
                a.Fill(this._dtCurrentTable);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            // getting the indices
            try
            {
                if (this.Database != null)
                {
                    Microsoft.SqlServer.Management.Smo.Table T = this.Database.Tables[Table];
                    if (T != null)
                    {
                        if (Index && T.HasIndex)
                        {
                            foreach (Microsoft.SqlServer.Management.Smo.Index I in T.Indexes)
                            {
                                foreach (Microsoft.SqlServer.Management.Smo.IndexedColumn IC in I.IndexedColumns)
                                {
                                    foreach (System.Data.DataRow R in this._dtCurrentTable.Rows)
                                    {
                                        if (R["ColumnName"].ToString() == IC.Name)
                                        {
                                            if (R["Indices"].ToString() == "-")
                                                R["Indices"] = "";
                                            if (I.IsUnique && !R["Indices"].ToString().Contains("U"))
                                                R["Indices"] += "U";
                                            else if (R["Indices"].ToString().Contains("I")) R["Indices"] += "I";
                                        }
                                    }
                                }
                            }
                        }

                        if (Required)
                        {
                            foreach (Microsoft.SqlServer.Management.Smo.Column C in T.Columns)
                            {
                                if (!C.Nullable)
                                {
                                    System.Data.DataRow[] RR = this._dtCurrentTable.Select("ColumnName = '" + C.Name + "'");
                                    RR[0]["Required"] = "R";
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return this._dtCurrentTable;
        }

        private System.Data.DataTable dtTableDocuConstraints(string Table)
        {
            // getting the constraints
            if (this._dtCurrentConstraint == null) this._dtCurrentConstraint = new DataTable();
            if (this._dtCurrentConstraint.Rows.Count > 0 && Table == this._CurrentTable)
                return this._dtCurrentConstraint;
            else
                this._dtCurrentConstraint.Clear();
            this._CurrentTable = Table;

            string SQL = "";
            SQL = "SELECT T.CONSTRAINT_NAME, U.TABLE_NAME, R.UNIQUE_CONSTRAINT_NAME, R.UPDATE_RULE, R.DELETE_RULE, " +
                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_TYPE, '' AS ColumnsConstraint, '' AS ColumnsUnique " +
                "FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS U RIGHT OUTER JOIN " +
                "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R RIGHT OUTER JOIN " +
                "INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS T INNER JOIN " +
                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS ON T.TABLE_NAME = INFORMATION_SCHEMA.TABLE_CONSTRAINTS.TABLE_NAME AND  " +
                "T.CONSTRAINT_NAME = INFORMATION_SCHEMA.TABLE_CONSTRAINTS.CONSTRAINT_NAME ON R.CONSTRAINT_NAME = T.CONSTRAINT_NAME ON " +
                "U.CONSTRAINT_NAME = R.UNIQUE_CONSTRAINT_NAME " +
                "WHERE (T.TABLE_NAME = '" + Table + "') ";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.Conn);
            try
            {
                a.Fill(this._dtCurrentConstraint);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            if (_dtCurrentConstraint.Rows.Count > 0)
            {
                try
                {
                    // getting the columns of the constraints
                    SQL = "SELECT COLUMN_NAME, CONSTRAINT_NAME " +
                        "FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE " +
                        "WHERE TABLE_NAME = '" + Table + "' " +
                        "ORDER BY CONSTRAINT_NAME ";
                    a.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dt = new DataTable();
                    a.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow RIndex in this._dtCurrentConstraint.Rows)
                        {
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                if (RIndex["CONSTRAINT_NAME"].ToString() == R["CONSTRAINT_NAME"].ToString())
                                {
                                    //if (RIndex["ColumnsConstraint"].ToString().Length == 0)
                                    //    RIndex["ColumnsConstraint"] = "ColumnsConstraint: ";
                                    RIndex["ColumnsConstraint"] = RIndex["ColumnsConstraint"].ToString() + R["COLUMN_NAME"].ToString() + ", ";
                                }
                            }
                        }
                        foreach (System.Data.DataRow RIndex in this._dtCurrentConstraint.Rows)
                        {
                            if (RIndex["ColumnsConstraint"].ToString().Length > 0
                                && (RIndex["ColumnsConstraint"].ToString().EndsWith(", ")))
                                RIndex["ColumnsConstraint"] = RIndex["ColumnsConstraint"].ToString().Substring(0, (RIndex["ColumnsConstraint"].ToString().Length - 2));
                        }
                    }

                    // getting the columns of the refered tables
                    SQL = "SELECT DISTINCT R.UNIQUE_CONSTRAINT_NAME, INFORMATION_SCHEMA.KEY_COLUMN_USAGE.COLUMN_NAME, " +
                        "INFORMATION_SCHEMA.KEY_COLUMN_USAGE.ORDINAL_POSITION " +
                        "FROM INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS T LEFT OUTER JOIN " +
                        "INFORMATION_SCHEMA.KEY_COLUMN_USAGE INNER JOIN " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON  " +
                        "INFORMATION_SCHEMA.KEY_COLUMN_USAGE.CONSTRAINT_NAME = R.UNIQUE_CONSTRAINT_NAME ON  " +
                        "T.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                        "WHERE (T.TABLE_NAME = '" + Table + "') AND (NOT (R.UNIQUE_CONSTRAINT_NAME IS NULL)) " +
                        "ORDER BY R.UNIQUE_CONSTRAINT_NAME, INFORMATION_SCHEMA.KEY_COLUMN_USAGE.ORDINAL_POSITION";
                    a.SelectCommand.CommandText = SQL;
                    System.Data.DataTable dtUnique = new DataTable();
                    a.Fill(dtUnique);
                    if (dtUnique.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow RIndex in this._dtCurrentConstraint.Rows)
                        {
                            foreach (System.Data.DataRow R in dtUnique.Rows)
                            {
                                if (RIndex["UNIQUE_CONSTRAINT_NAME"].ToString() == R["UNIQUE_CONSTRAINT_NAME"].ToString())
                                {
                                    //if (RIndex["ColumnsUnique"].ToString().Length == 0)
                                    //    RIndex["ColumnsUnique"] = "ColumnsUnique: ";
                                    RIndex["ColumnsUnique"] = RIndex["ColumnsUnique"].ToString() + R["COLUMN_NAME"].ToString() + ", ";
                                }
                            }
                        }
                        foreach (System.Data.DataRow RIndex in this._dtCurrentConstraint.Rows)
                        {
                            if (RIndex["ColumnsUnique"].ToString().Length > 0
                                && (RIndex["ColumnsUnique"].ToString().EndsWith(", ")))
                                RIndex["ColumnsUnique"] = RIndex["ColumnsUnique"].ToString().Substring(0, (RIndex["ColumnsUnique"].ToString().Length - 2));
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            return this._dtCurrentConstraint;
        }

        #endregion

        #region Table list

        private void fillTableListDocu()
        {
            System.Data.DataTable dt = this.TableList();
            try
            {
                if (dt != null && dt.Columns.Count > 0 && this.checkedListBoxDocuTables != null)
                {
                    if (this.checkedListBoxDocuTables.DataSource != null)
                        this.checkedListBoxDocuTables.DataSource = null;

                    this.checkedListBoxDocuTables.Items.Clear();
                    // Markus 26.7.23: Bugfix listing objects
                    //foreach (System.Data.DataRow R in dt.Rows)
                    //{
                    //    System.Collections.Generic.List<object> L = new List<object>();
                    //    L.Add(R["ObjectName"].ToString());
                    //    L.Add(R["ObjectType"].ToString());
                    //    L.Add(R);
                    //    this.checkedListBoxDocuTables.Items.Add(L);

                    //}
                    try
                    {
                        // https://stackoverflow.com/questions/8215933/how-come-checkedlistbox-does-not-have-datasource-how-to-bind-to-a-list-of-valu
                        this.checkedListBoxDocuTables.DataSource = null;
                        ((System.Windows.Forms.ListBox)this.checkedListBoxDocuTables).DataSource = dt;
                    }
                    catch { } // Markus 10.7.23: unklar was hier schief laeuft - danach klappt es
                    this.checkedListBoxDocuTables.DataSource = dt;
                    this.checkedListBoxDocuTables.DisplayMember = "ObjectName";
                    this.checkedListBoxDocuTables.ValueMember = "ObjectType";
                }
                else
                {
                    this.checkedListBoxDocuTables.DataSource = null;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #region Checking
        private void comboBoxSchema_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxSchema.SelectedItem;
            this._Schema = this.comboBoxSchema.SelectedValue.ToString();
        }

        private void checkBoxDocuTableListNone_Click(object sender, EventArgs e)
        {
            this.setDocuTableListChecked(false);
        }

        private void checkBoxDocuTableListAll_Click(object sender, EventArgs e)
        {
            this.setDocuTableListChecked(true);
        }

        private void setDocuTableListChecked(bool Checked)
        {
            this.checkBoxIncludeRoles.Checked = Checked;
            this.checkBoxIncludeTables.Checked = Checked;
            this.checkBoxIncludeTrigger.Checked = Checked;
            this.checkBoxIncludeViews.Checked = Checked;
            this.checkBoxIncludeFunctions.Checked = Checked;
        }

        private void checkBoxDocuListAll_Click(object sender, EventArgs e)
        {
            this.setDocuListChecked(true);
        }

        private void checkBoxDocuListNone_Click(object sender, EventArgs e)
        {
            this.setDocuListChecked(false);
        }


        private void setDocuListChecked(bool Checked)
        {
            for (int i = 0; i < this.checkedListBoxDocuTables.Items.Count; i++)
                this.checkedListBoxDocuTables.SetItemChecked(i, Checked);
        }

        #endregion

        private void buttonDocuFillTable_Click(object sender, EventArgs e)
        {
            this.fillTableListDocu();
        }

        private System.Data.DataTable dtTableDocu(string TableName, bool UseContext)
        {
            if (this._dtCurrentTable == null)
                this._dtCurrentTable = new DataTable();
            try
            {
                //System.Data.DataTable dt = new DataTable();
                if (this._dtCurrentTable.Rows.Count > 0 && TableName == this._CurrentTable)
                    return this._dtCurrentTable;
                else
                {
                    this._dtCurrentTable.Clear();
                    this._dtCurrentTable.Columns.Clear();
                }
                this._CurrentTable = TableName;
                string SQL = "";
                string Column = "ColumnName";
                string Datatype = "Datatype";
                string Length = "Length";
                string Default = "DefaultValue";
                string Description = "Description";
                if (UseContext && DiversityWorkbench.Settings.Language != "en-US")
                {
                    Column = "Spalte";
                    Datatype = "Datentyp";
                    Length = "Länge";
                    Default = "Defaultwert";
                    Description = "Beschreibung";
                }
                SQL = "SELECT C.COLUMN_NAME as ColumnName, C.DATA_TYPE as Datatype, C.CHARACTER_MAXIMUM_LENGTH as Length, " +
                    "C.COLLATION_NAME  AS Collation, C.COLUMN_DEFAULT AS DefaultValue, '' AS Description ";
                if (this.IncludeUsageNotes)
                    SQL += ", '' AS Notes ";
                if (this.IncludeNullable)
                    SQL += ", C.IS_NULLABLE As Nullable ";
                if (this.IncludeVisibility)
                {
                    SQL += ", '-' As Visibility";
                }
                if (this.IncludeRelations)
                {
                    SQL += ", CASE WHEN MIN(P.TABLE_NAME) IS NULL THEN '-' ELSE 'Refers to table ' + " +
                        " CASE WHEN MIN(P.TABLE_NAME) <> MAX(P.TABLE_NAME) THEN MIN(P.TABLE_NAME) + ' and table ' + MAX(P.TABLE_NAME) ELSE MIN(P.TABLE_NAME) END END AS Relation, C.ORDINAL_POSITION ";
                    SQL += "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R INNER JOIN " +
                        "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS F ON R.CONSTRAINT_NAME = F.CONSTRAINT_NAME INNER JOIN " +
                        "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME RIGHT OUTER JOIN " +
                        "INFORMATION_SCHEMA.COLUMNS AS C ON F.COLUMN_NAME = C.COLUMN_NAME AND F.TABLE_NAME = C.TABLE_NAME AND " +
                        "F.TABLE_SCHEMA = C.TABLE_SCHEMA AND F.TABLE_CATALOG = C.TABLE_CATALOG  ";
                }
                else
                    SQL += " FROM Information_Schema.COLUMNS C ";
                SQL += " WHERE C.TABLE_NAME = '" + TableName + "' AND C.TABLE_SCHEMA = '" + this._Schema + "' ";
                if (this.IncludeRelations)
                {
                    SQL += " GROUP BY C.COLUMN_NAME, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH, " +
                    "C.COLLATION_NAME, C.COLUMN_DEFAULT, C.ORDINAL_POSITION, C.IS_NULLABLE ";
                }
                SQL += " ORDER BY C.ORDINAL_POSITION";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.Conn);
                try
                {
                    a.Fill(this._dtCurrentTable);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
                if (this.checkBoxIncludeUsageNotes.Checked)
                {
                    foreach (System.Data.DataRow R in this._dtCurrentTable.Rows)
                    {
                        System.Collections.Generic.Dictionary<string, string> E = DiversityWorkbench.Entity.EntityInformation(TableName + "." + R["ColumnName"]);
                        if (E.Count > 0 && E.ContainsKey("UsageNotes"))
                            R["Notes"] = E["UsageNotes"];
                        string Visibility = "";
                        if (E.Count > 0 && E.ContainsKey("Visibility"))
                            Visibility = E["Visibility"];
                        if (Visibility.Length > 0)
                            R["Visibility"] = Visibility;
                        else if (E.Count > 0)
                        {
                            if (E["Accessibility"] == "inapplicable")
                                R["Visibility"] = "-";
                            else if (E["Accessibility"] == "no_restrictions")
                                R["Visibility"] = "visible";
                            else if (E["Accessibility"] == "read_only")
                                R["Visibility"] = "hidden";
                            else if (E["Accessibility"].Length == 0)
                                R["Visibility"] = "-";
                            else
                                R["Visibility"] = "-";
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return this._dtCurrentTable;
        }

        private void buttonAddToSelection_Click(object sender, EventArgs e)
        {
            if (this.textBoxAddToSelection.Text.Length > 0)
                this.setDocuTableListChecked(true, this.textBoxAddToSelection.Text);
        }

        private void buttonRemoveFromSelection_Click(object sender, EventArgs e)
        {
            if (this.textBoxRemoveFromSelection.Text.Length > 0)
                this.setDocuTableListChecked(false, this.textBoxRemoveFromSelection.Text);
        }

        private void buttonDefaultSelection_Click(object sender, EventArgs e)
        {
            ForEnums = false;

            this.setDocuTableListChecked(true);
            this.setDocuListChecked(true);

            this.HtmlExclusions.Clear();

            this.checkBoxListExcludeOld.Checked = true;
            this.checkBoxListExcludeSystem.Checked = true;
            this.checkBoxListExcludeLog.Checked = true;
            this.checkBoxListExcludeEnum.Checked = true;
            this.checkBoxListExcludeDeprecated.Checked = true;

            System.Collections.Generic.List<string> Remove = this.ListExclude();// new List<string>();

            foreach (string R in Remove)
                this.setDocuTableListChecked(false, R);
            this.HtmlExclusionSet(Exclusion.PreviousVersion);
            this.checkBoxHtmlExcludeStandardTrigger.Checked = true;
            this.checkBoxIncludeContext.Checked = false;
            this.setContextControls(false);
            this.checkBoxIncludeDefinition.Checked = false;
            this.checkBoxIncludeDescription.Checked = true;
            this.checkBoxIncludeLookupRelations.Checked = true;
            this.checkBoxIncludeNullable.Checked = true;
            this.checkBoxIndex.Checked = this._outputType != OutputType.HUGO;
            this.checkBoxExcludeSystemObjects.Checked = true;
            this.checkBoxSuppressSpecialColumns.Checked = true;
            this.checkBoxSuppressSpecialColumnsEnding.Checked = true;
            this.checkBoxHtmlExcludeColumnsObsolete.Checked = true;
            this.checkBoxHtmlIncludeNotesOnExclusions.Checked = true;
            this.checkBoxHtmlIncludeMetadata.Checked = this._outputType != OutputType.HUGO;
            this.checkBoxHtmlIncludeLogo.Checked = this._outputType != OutputType.HUGO;
            this.checkBoxListExcludeEnum.Checked = true;
            this.checkBoxHtmlExcludeLoggingColumns.Checked = true;
            this.checkBoxHtmlIncludeHomeButton.Checked = this._outputType != OutputType.HUGO;
            this.checkBoxHtmlNoColumns.Checked = false;

            this.checkBoxHtmlCitationInclude.Checked = this._outputType != OutputType.HUGO;
            this.checkBoxHtml_ER_Include.Checked = true; // this._outputType != OutputType.HUGO;
            this.checkBoxHtmlModel.Checked = this._outputType != OutputType.HUGO;
            this.checkBoxHugoMermaid.Checked = this._outputType == OutputType.HUGO;
            this.checkBoxHugoMenuIcon.Checked = this._outputType == OutputType.HUGO;
            this.checkBoxHugoMermaid.Checked = false;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation));
            this.setDefaultCSS(ref this._HtmlCssFiles, dir);
            this.initHtmlCssList();

        }

        #region Enums

        private bool _ForEnums = false;

        private bool ForEnums
        {
            get => _ForEnums;
            set
            {
                _ForEnums = value;
                if (_ForEnums)
                {
                    this.buttonEnumSelection.BackColor = System.Drawing.Color.Pink;
                    this.toolTip.SetToolTip(this.buttonEnumSelection, "Enumeration Option set. Click to return to default setting");
                    this.buttonEnumSelection.Image = DiversityWorkbench.Properties.Resources.InLines;
                    this.buttonEnumSelection.ImageAlign = ContentAlignment.MiddleLeft;
                }
                else
                {
                    this.buttonEnumSelection.BackColor = System.Drawing.SystemColors.Control;
                    this.toolTip.SetToolTip(this.buttonEnumSelection, "Set default selection for enumeration tables");
                    this.buttonEnumSelection.Image = null;
                }
            }
        }

        private void buttonEnumSelection_Click(object sender, EventArgs e)
        {
            if (ForEnums)
            {
                ForEnums = false;
                return;
            }

            this.ForEnums = true;
            this.setDocuTableListChecked(false);
            this.setDocuListChecked(false);

            this.HtmlExclusions.Clear();

            this.checkBoxIncludeTables.Checked = true;
            this.checkBoxIncludeTrigger.Checked = true;

            this.checkBoxListExcludeOld.Checked = true;
            this.checkBoxListExcludeSystem.Checked = true;
            this.checkBoxListExcludeLog.Checked = true;
            this.checkBoxListExcludeEnum.Checked = false;
            this.checkBoxListExcludeDeprecated.Checked = true;

            System.Collections.Generic.List<string> Include = this.ListInclude();// new List<string>();

            foreach (string R in Include)
                this.setDocuTableListChecked(true, R);

            System.Collections.Generic.List<string> Remove = this.ListExclude();// new List<string>();

            foreach (string R in Remove)
                this.setDocuTableListChecked(false, R);

            this.HtmlExclusionSet(Exclusion.PreviousVersion);
            this.checkBoxHtmlExcludeStandardTrigger.Checked = true;
            this.checkBoxIncludeContext.Checked = false;
            this.setContextControls(false);
            this.checkBoxIncludeDefinition.Checked = false;
            this.checkBoxIncludeDescription.Checked = true;
            this.checkBoxIncludeLookupRelations.Checked = true;
            this.checkBoxIncludeNullable.Checked = false;
            this.checkBoxIndex.Checked = _outputType != OutputType.HUGO;
            this.checkBoxExcludeSystemObjects.Checked = true;
            this.checkBoxSuppressSpecialColumns.Checked = true;
            this.checkBoxSuppressSpecialColumnsEnding.Checked = true;
            this.checkBoxHtmlExcludeColumnsObsolete.Checked = true;
            this.checkBoxHtmlIncludeNotesOnExclusions.Checked = true;
            this.checkBoxHtmlIncludeMetadata.Checked = true;
            this.checkBoxHtmlIncludeLogo.Checked = true;
            this.checkBoxListExcludeEnum.Checked = false;
            this.checkBoxHtmlExcludeLoggingColumns.Checked = true;
            this.checkBoxHtmlNoColumns.Checked = true;
            this.checkBoxHtmlIncludeHomeButton.Checked = _outputType != OutputType.HUGO;
            this.checkBoxHtmlIncludeDepending.Checked = true;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation));
            this.setDefaultCSS(ref this._HtmlCssFiles, dir);
            this.initHtmlCssList();

        }

        private string DefaultEnumTableName()
        {
            if (ForEnums)
            {
                string SQL = "select top 1 C.TABLE_NAME from INFORMATION_SCHEMA.COLUMNS C inner join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE P on C.TABLE_NAME = P.TABLE_NAME and P.COLUMN_NAME = 'ParentCode' where C.TABLE_NAME LIKE '%_enum' Group by C.TABLE_NAME order by COUNT(*) desc";
                string table = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (table.Length == 0)
                {
                    SQL = "select top 1 C.TABLE_NAME from INFORMATION_SCHEMA.COLUMNS C inner join INFORMATION_SCHEMA.KEY_COLUMN_USAGE K on C.TABLE_NAME = K.TABLE_NAME and K.CONSTRAINT_NAME like 'PK_%' and K.COLUMN_NAME = 'Code'where C.TABLE_NAME LIKE '%_enum' Group by C.TABLE_NAME order by COUNT(*) desc";
                    table = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
                return table;
            }
            return "";
        }

        #endregion

        private System.Collections.Generic.List<string> ListExclude()
        {
            System.Collections.Generic.List<string> Remove = new List<string>();
            // Log
            if (this.checkBoxListExcludeLog.Checked)
            {
                Remove.Add("*_log");
                this.HtmlExclusionSet(Exclusion.LogTab);
            }

            // Enum
            if (this.checkBoxListExcludeEnum.Checked)
            {
                Remove.Add("*_Enum");
                this.HtmlExclusionSet(Exclusion.EnumTab);
            }

            // System
            if (this.checkBoxListExcludeSystem.Checked)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.DatabaseSystemObjects)
                    Remove.Add(KV.Key);
                this.HtmlExclusionSet(Exclusion.System);
            }

            if (_outputType == OutputType.HUGO)
            {
                Remove.Add("Application*");
                Remove.Add("*Proxy*");

            }

            // Old & Deprecated
            System.Collections.Generic.List<string> All = new List<string>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> Versions = new Dictionary<string, System.Collections.Generic.List<int>>();
            for (int i = 0; i < this.checkedListBoxDocuTables.Items.Count; i++)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.checkedListBoxDocuTables.Items[i];
                string Item = R[0].ToString();
                string Type = R[1].ToString();
                // Deprecated
                if (this.checkBoxListExcludeDeprecated.Checked)
                {
                    this.HtmlExclusionSet(Exclusion.ObsoleteObj);
                    if (this.IsOutdated(Item, Type))
                        Remove.Add(Item);
                }
                // Old
                if (!All.Contains(Item))
                    All.Add(Item);
                int Version;
                if (int.TryParse(Item.Substring(Item.Length - 1), out Version))
                {
                    if (!Versions.ContainsKey(Item.Substring(0, Item.Length - 1)))
                    {
                        System.Collections.Generic.List<int> v = new List<int>();
                        v.Add(Version);
                        Versions.Add(Item.Substring(0, Item.Length - 1), v);
                    }
                    else if (Versions.ContainsKey(Item.Substring(0, Item.Length - 1)))
                    {
                        Versions[Item.Substring(0, Item.Length - 1)].Add(Version);
                    }
                }
            }
            if (Versions.Count > 0 && this.checkBoxListExcludeOld.Checked)
            {
                this.HtmlExclusionSet(Exclusion.PreviousVersion);
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in Versions)
                {
                    if (KV.Value.Count > 1)
                    {
                        int Max = KV.Value.Max();
                        foreach (int i in KV.Value)
                        {
                            if (i != Max && !Remove.Contains(KV.Key + i.ToString()))
                                Remove.Add(KV.Key + i.ToString());
                        }
                    }
                    if (All.Contains(KV.Key))
                    {
                        if (!Remove.Contains(KV.Key))
                            Remove.Add(KV.Key);
                    }
                    if (KV.Key.EndsWith("_") && All.Contains(KV.Key.Substring(0, KV.Key.Length - 1)))
                    {
                        if (!Remove.Contains(KV.Key.Substring(0, KV.Key.Length - 1)))
                            Remove.Add(KV.Key.Substring(0, KV.Key.Length - 1));
                    }
                }
            }
            return Remove;
        }

        private System.Collections.Generic.List<string> ListInclude()
        {
            System.Collections.Generic.List<string> Add = new List<string>();
            // Log
            if (!this.checkBoxListExcludeLog.Checked)
            {
                Add.Add("*_log");
                //this.HtmlExclusionSet(Exclusion.LogTab);
            }

            // Enum
            if (!this.checkBoxListExcludeEnum.Checked)
            {
                Add.Add("*_Enum");
                //this.HtmlExclusionSet(Exclusion.EnumTab);
            }

            // System
            if (!this.checkBoxListExcludeSystem.Checked)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this.DatabaseSystemObjects)
                    Add.Add(KV.Key);
                //this.HtmlExclusionSet(Exclusion.System);
            }

            // Old & Deprecated
            System.Collections.Generic.List<string> All = new List<string>();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> Versions = new Dictionary<string, System.Collections.Generic.List<int>>();
            for (int i = 0; i < this.checkedListBoxDocuTables.Items.Count; i++)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.checkedListBoxDocuTables.Items[i];
                string Item = R[0].ToString();
                string Type = R[1].ToString();
                // Deprecated
                if (!this.checkBoxListExcludeDeprecated.Checked)
                {
                    //this.HtmlExclusionSet(Exclusion.ObsoleteObj);
                    if (this.IsOutdated(Item, Type))
                        Add.Add(Item);
                }
                // Old
                if (!All.Contains(Item))
                    All.Add(Item);
                int Version;
                if (int.TryParse(Item.Substring(Item.Length - 1), out Version))
                {
                    if (!Versions.ContainsKey(Item.Substring(0, Item.Length - 1)))
                    {
                        System.Collections.Generic.List<int> v = new List<int>();
                        v.Add(Version);
                        Versions.Add(Item.Substring(0, Item.Length - 1), v);
                    }
                    else if (Versions.ContainsKey(Item.Substring(0, Item.Length - 1)))
                    {
                        Versions[Item.Substring(0, Item.Length - 1)].Add(Version);
                    }
                }
            }
            if (Versions.Count > 0 && !this.checkBoxListExcludeOld.Checked)
            {
                //this.HtmlExclusionSet(Exclusion.PreviousVersion);
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.List<int>> KV in Versions)
                {
                    if (KV.Value.Count > 1)
                    {
                        int Max = KV.Value.Max();
                        foreach (int i in KV.Value)
                        {
                            if (i != Max && !Add.Contains(KV.Key + i.ToString()))
                                Add.Add(KV.Key + i.ToString());
                        }
                    }
                    if (All.Contains(KV.Key))
                    {
                        if (!Add.Contains(KV.Key))
                            Add.Add(KV.Key);
                    }
                    if (KV.Key.EndsWith("_") && All.Contains(KV.Key.Substring(0, KV.Key.Length - 1)))
                    {
                        if (!Add.Contains(KV.Key.Substring(0, KV.Key.Length - 1)))
                            Add.Add(KV.Key.Substring(0, KV.Key.Length - 1));
                    }
                }
            }
            return Add;
        }

        private bool IsOutdated(string Item, string Type)
        {
            bool IsOutdated = false;
            string Description = "";
            // Markus 17.4.23: Special description retrieval for Roles
            if (Type == "Role")
                Description = DiversityWorkbench.Forms.FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.USER, Item, "");
            else
                Description = DiversityWorkbench.Forms.FormFunctions.getDescription(Item, Type, "", this.Conn, _Schema);
            IsOutdated = this.IsOutdated(Description);
            return IsOutdated;
        }

        private bool IsOutdated(string Description)
        {
            bool IsOutdated = false;
            if (Description != null)
            {
                if (Description.ToLower().StartsWith("deprecated") ||
                    Description.ToLower().StartsWith("obsolete") ||
                    Description.ToLower().StartsWith("outdated") ||
                    Description.ToLower().StartsWith("out-of-date") ||
                    Description.ToLower().StartsWith("out of use ") ||
                    Description.ToLower().StartsWith("out of date") ||
                    Description.ToLower().StartsWith("out-dated") ||
                    Description.ToLower().StartsWith("obsolescent"))
                    IsOutdated = true;
            }
            return IsOutdated;
        }


        private void setDocuTableListChecked(bool Add, string Pattern)
        {
            string PatternCore = Pattern.Replace("*", "");
            for (int i = 0; i < this.checkedListBoxDocuTables.Items.Count; i++)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.checkedListBoxDocuTables.Items[i];
                string Item = R[0].ToString();
                if (Item == Pattern
                    || (Pattern.StartsWith("*") && Item.EndsWith(PatternCore))
                    || (Pattern.EndsWith("*") && Item.StartsWith(PatternCore))
                    || (Pattern.StartsWith("*") && Pattern.EndsWith("*") && Item.IndexOf(PatternCore) > -1))
                    this.checkedListBoxDocuTables.SetItemChecked(i, Add);
            }
        }

        private void buttonRequeryEntity_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxIncludeTables_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxIncludeTrigger.Enabled = this.checkBoxIncludeTables.Checked;
            if (!this.checkBoxIncludeTables.Checked)
                this.checkBoxIncludeTrigger.Checked = this.checkBoxIncludeTables.Checked;
        }

        #endregion

        #region Context

        private void initContext()
        {
            this.checkBoxIncludeContext.Visible = DiversityWorkbench.Entity.EntityTablesExistInDatabase;
            this.groupBoxDokuIncludeObjectTypes.Height = 62;
            if (DiversityWorkbench.Entity.EntityTablesExistInDatabase)
            {
                this.groupBoxDokuIncludeObjectTypes.Height += 22;
                this.checkBoxIncludeContext.Checked = DiversityWorkbench.Settings.UseEntity;
            }
        }

        private void checkBoxIncludeContext_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.UseEntity = !DiversityWorkbench.Settings.UseEntity;
            this.checkBoxDocuContentIncludeIndex.Checked = DiversityWorkbench.Settings.UseEntity;
            this.setContextControls();
        }

        private void setContextControls(bool? UseContext = null)
        {
            if (UseContext != null)
                DiversityWorkbench.Settings.UseEntity = (bool)UseContext;
            this.setContext();
            this.setContextForMediaWiki();
        }

        #endregion

        #region HTML, markdown, HUGO

        #region Common
        private enum OutputType { html, md, HUGO }
        private OutputType _outputType = OutputType.html;

        private void initHtml()
        {
            this.initDocuHtml();
            this.initDocuHugo();
        }

        private void comboBoxDocuOutput_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (this.comboBoxDocuOutput.SelectedItem.ToString())
            {
                case "html":
                    _outputType = OutputType.html;
                    break;
                case "md":
                    _outputType = OutputType.md;
                    break;
                case "HUGO":
                    _outputType = OutputType.HUGO;
                    break;
            }
            this.textBoxHugoER_BasePath.Visible = _outputType == OutputType.HUGO;
            if (!this.textBoxHugoER_BasePath.Text.EndsWith(DiversityWorkbench.Settings.ModuleName.ToLower() + "/"))
                this.textBoxHugoER_BasePath.Text += DiversityWorkbench.Settings.ModuleName.ToLower() + "/";
        }

        private bool HtmlIncludeCSS { get { return this._HtmlCssFiles != null && this._HtmlCssFiles.Count > 0; } }
        private bool HtmlIncludeDWBcss { get { return HtmlIncludeCSS && this._HtmlCssFiles.ContainsKey("DWB.css"); } }

        #region Exclusions

        private enum Exclusion { EnumTab, LogTab, LogCol, ObsoleteCol, ObsoleteObj, PreviousVersion, System, StdTrigger, AnyColumn, Application }

        private System.Collections.Generic.Dictionary<Exclusion, string> HtmlExclusions = new Dictionary<Exclusion, string>();

        private void HtmlExclusionSet(Exclusion ex, bool Add = true, string Comment = "")
        {
            if (Comment.Length == 0 && HtmlExclusionContent.ContainsKey(ex))
                Comment = HtmlExclusionContent[ex];
            if (Add)
            {
                if (!HtmlExclusions.ContainsKey(ex))
                    HtmlExclusions.Add(ex, Comment);
            }
            else
            {
                if (HtmlExclusions.ContainsKey(ex))
                    HtmlExclusions.Remove(ex);
            }
        }


        private System.Collections.Generic.Dictionary<Exclusion, string> _HtmlExclusionContent;
        private System.Collections.Generic.Dictionary<Exclusion, string> HtmlExclusionContent
        {
            get
            {
                if (_HtmlExclusionContent == null)
                {
                    _HtmlExclusionContent = new Dictionary<Exclusion, string>();
                    _HtmlExclusionContent.Add(Exclusion.EnumTab, "Enumeration tables");
                    _HtmlExclusionContent.Add(Exclusion.LogTab, "Logging tables");
                    _HtmlExclusionContent.Add(Exclusion.LogCol, "Logging columns and column RowGUID");
                    _HtmlExclusionContent.Add(Exclusion.ObsoleteCol, "Columns marked as obsolete");
                    _HtmlExclusionContent.Add(Exclusion.ObsoleteObj, "Objects marked as obsolete");
                    _HtmlExclusionContent.Add(Exclusion.StdTrigger, "Standard trigger for logging");
                    _HtmlExclusionContent.Add(Exclusion.System, "System objects");
                    _HtmlExclusionContent.Add(Exclusion.PreviousVersion, "Previous versions of objects");
                    _HtmlExclusionContent.Add(Exclusion.AnyColumn, "All columns");
                }
                return _HtmlExclusionContent;
            }
        }

        private void checkBoxListExcludeEnum_CheckedChanged(object sender, EventArgs e)
        {
            this.HtmlExclusionSet(Exclusion.EnumTab, this.checkBoxListExcludeEnum.Checked);
        }

        private void checkBoxHtmlExcludeStandardTrigger_CheckedChanged(object sender, EventArgs e)
        {
            this.HtmlExclusionSet(Exclusion.StdTrigger, this.checkBoxHtmlExcludeStandardTrigger.Checked);
        }

        private void checkBoxHtmlExcludeLoggingColumns_CheckedChanged(object sender, EventArgs e)
        {
            this.HtmlExclusionSet(Exclusion.LogCol, this.checkBoxHtmlExcludeLoggingColumns.Checked);
        }

        private void checkBoxHtmlExcludeColumnsObsolete_CheckedChanged(object sender, EventArgs e)
        {
            this.HtmlExclusionSet(Exclusion.ObsoleteCol, this.checkBoxHtmlExcludeColumnsObsolete.Checked);
        }

        private void checkBoxHtmlNoColumns_CheckedChanged(object sender, EventArgs e)
        {
            this.HtmlExclusionSet(Exclusion.EnumTab, this.checkBoxListExcludeEnum.Checked);
        }

        private void checkBoxListExcludeDeprecated_CheckedChanged(object sender, EventArgs e)
        {
            this.HtmlExclusionSet(Exclusion.ObsoleteObj, this.checkBoxListExcludeDeprecated.Checked);
        }

        private void checkBoxListExcludeLog_CheckedChanged(object sender, EventArgs e)
        {
            this.HtmlExclusionSet(Exclusion.LogTab, this.checkBoxListExcludeLog.Checked);
        }

        private void checkBoxListExcludeOld_CheckedChanged(object sender, EventArgs e)
        {
            this.HtmlExclusionSet(Exclusion.PreviousVersion, this.checkBoxListExcludeOld.Checked);
        }

        private void checkBoxListExcludeSystem_CheckedChanged(object sender, EventArgs e)
        {
            this.HtmlExclusionSet(Exclusion.System, this.checkBoxListExcludeSystem.Checked);
        }

        #endregion

        #endregion

        #region Output

        private void buttonDocuHtmlCreate_Click(object sender, EventArgs e)
        {
            string Path = "";
            switch (_outputType)
            {
                case OutputType.HUGO:
                case OutputType.md:
                    Path = this.createDocuMarkdown("");
                    break;
                default:
                    Path = this.createDocuHtml("");
                    break;
            }
            if (Path.Length > 0)
            {
                System.Uri u = new Uri(Path);
                //this.webxBrowserDocuHtml.Url = u;
                this.userControlWebViewHtml.Url = null;
                this.userControlWebViewHtml.Navigate(u);

                //this.userControlWebViewHtml.Refresh();
                //this.userControlWebViewHtml.NavigateToDocument(Path);
                //this.userControlWebViewHtml.AllowScripting = true;
                System.Windows.Forms.MessageBox.Show("Documentation file created:\r\n" + Path);
                this.buttonOpenHtmlFolder.Tag = Path;
            }
        }

        #region html

        private void initDocuHtml()
        {
            this.comboBoxDocuOutput.Items.Add(OutputType.HUGO.ToString());
            this.comboBoxDocuOutput.Items.Add(OutputType.html.ToString());
            //this.comboBoxDocuOutput.Items.Add(OutputType.md.ToString());
            this.comboBoxDocuOutput.SelectedIndex = 0;
            this.comboBoxDocuOutput_SelectionChangeCommitted(null, null);
        }

        private string createDocuHtml(string FileName)
        {
            if (this.checkBoxHtmlContext.Checked && this.comboBoxContext.SelectedValue.ToString().Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a context");
                return "";
            }
            if (this.checkedListBoxDocuTables.CheckedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select at least one table");
                return "";
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //this._CheckedItems = null;

            if (FileName.Length == 0) FileName = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + DiversityWorkbench.Settings.ModuleName;
            if (this.ForEnums) FileName += "_Enum";
            FileName += ".htm";
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(FileName, settings);
            try
            {
                //W.WriteStartDocument();
                W.WriteStartElement("html");
                W.WriteString("\r\n");
                W.WriteStartElement("head");
                W.WriteString("\r\n");
                string Title = DiversityWorkbench.Settings.ModuleName;
                if (ForEnums) Title += " enumeration tables";
                W.WriteElementString("title", Title);
                W.WriteString("\r\n");

                this.createDocuHtmlCSS(W, FileName);

                this.createDocuHtmlFavicon(W);

                this.createDocuHtmlMetadata(W);

                W.WriteEndElement();//head
                W.WriteString("\r\n");

                W.WriteStartElement("body");
                W.WriteString("\r\n");

                this.createDocuHeader(W);

                this.createDocuHtmlLogo(W);


                this.createDocuHtmlContext(W);

                this.createDocuHtmlCitation(W);
                this.createDocuHtmlModel(W);
                this.createDocuHtmlER(W);

                this.createDocuHtmlNotes(W);

                this.createDocuHtmlIndex(W);

                if (ForEnums)
                    this.createDocuHtmlTable(W, DefaultEnumTableName(), "Table", true);


                string CurrentType = "";
                this.progressBarHtmlDocu.Value = 0;
                this.progressBarHtmlDocu.Maximum = this.checkedListBoxDocuTables.CheckedItems.Count;
                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    if (CurrentType != R[1].ToString() && this.HtmlDocuContainsMoreThenOneType())
                    {
                        CurrentType = R[1].ToString();
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                        W.WriteStartElement("h4");
                        W.WriteString(CurrentType.ToUpper() + "S");
                        W.WriteEndElement();//h4
                        W.WriteEndElement();//FONT
                        W.WriteString("\r\n");
                    }
                    string ObjectType = R["ObjectType"].ToString().ToLower();
                    if (ObjectType == "table" && this.checkBoxListExcludeEnum.Checked && R[0].ToString().ToLower().EndsWith("_enum"))
                        continue;
                    switch (ObjectType)
                    {
                        case "table":
                        case "view":
                            this.createDocuHtmlTable(W, R[0].ToString(), R[1].ToString());
                            break;
                        case "function":
                        case "procedure":
                            this.createDocuHtmlRoutine(W, R[0].ToString(), R[1].ToString());
                            break;
                        case "role":
                            this.createDocuHtmlRole(W, R[0].ToString());
                            break;
                    }
                    if (this.progressBarHtmlDocu.Value < this.progressBarHtmlDocu.Maximum)
                    {
                        this.progressBarHtmlDocu.Value++;
                        Application.DoEvents();
                    }
                }
                W.WriteEndElement();//body
                W.WriteEndElement();//html
                W.WriteEndDocument();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            return FileName;
        }

        #region Context

        //private void createDocuHtmlColorLegendTable(System.Xml.XmlWriter W)
        //{
        //    if (this.checkBoxHtmlIncludeAccessibility.Checked)
        //    {
        //        System.Data.DataTable dtAccessibility = new DataTable();
        //        string SQL = "SELECT Code, Description FROM dbo.EntityUsage_Enum ORDER BY DisplayOrder";
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.Conn);
        //        ad.Fill(dtAccessibility);

        //        if (dtAccessibility.Rows.Count > 0)
        //        {
        //            W.WriteStartElement("FONT");
        //            W.WriteAttributeString("face", "Verdana");
        //            W.WriteStartElement("h5");
        //            if (this.checkBoxHtmlContext.Checked && DiversityWorkbench.Settings.Language == "de-DE")
        //                W.WriteString("Farbcode:");
        //            else
        //                W.WriteString("Color code: ");
        //            W.WriteEndElement();//h5
        //            W.WriteEndElement();//Font

        //            W.WriteStartElement("table");
        //            W.WriteAttributeString("cellpadding", "3");
        //            W.WriteAttributeString("cellspacing", "0");
        //            W.WriteAttributeString("border", "1");

        //        }
        //    }

        //}

        private void createDocuHtmlColorLegendTable(System.Xml.XmlWriter W
            , string EnumTable
            , System.Collections.Generic.Dictionary<string, bool> ShowDataDict)
        {
            System.Data.DataTable dtEnumTable = new DataTable();
            string SQL = "SELECT Code, Description FROM dbo." + EnumTable + " ORDER BY DisplayOrder";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.Conn);
            ad.Fill(dtEnumTable);

            if (dtEnumTable.Rows.Count > 0)
            {
                string Title = EnumTable.Replace("Entity", "").Replace("_Enum", "");
                W.WriteStartElement("FONT");
                W.WriteAttributeString("face", "Verdana");
                W.WriteStartElement("h5");
                if (this.checkBoxHtmlContext.Checked && DiversityWorkbench.Settings.Language == "de-DE")
                    W.WriteString("Farbcode:");
                else
                    W.WriteString("Color code for " + Title + ": ");
                W.WriteEndElement();//h5
                W.WriteEndElement();//Font

                W.WriteStartElement("table");
                W.WriteAttributeString("cellpadding", "3");
                W.WriteAttributeString("cellspacing", "0");
                W.WriteAttributeString("border", "1");

                foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in ShowDataDict)
                {
                    System.Data.DataRow[] RR = dtEnumTable.Select("Code = '" + KV.Key + "'");
                    if (RR.Length > 0)
                    {
                        string ColorBack = this.userControlColorSettings.ColorCode(RR[0]["Code"].ToString());
                        string ColorText = this.userControlColorSettings.ColorCodeText(RR[0]["Code"].ToString());
                        string DisplayText = RR[0]["Code"].ToString();
                        string Description = RR[0]["Description"].ToString();

                        W.WriteStartElement("tr");

                        W.WriteStartElement("td");
                        W.WriteAttributeString("bgcolor", ColorBack);
                        W.WriteAttributeString("style", "color: " + ColorText + "; font-weight: bold");
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                        W.WriteAttributeString("size", "2");
                        W.WriteString(DisplayText);
                        W.WriteEndElement();//Font
                        W.WriteEndElement();//td

                        W.WriteStartElement("td");
                        W.WriteAttributeString("bgcolor", ColorBack);
                        W.WriteAttributeString("style", "color: " + ColorText);
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                        W.WriteAttributeString("size", "2");
                        W.WriteString(Description);
                        W.WriteEndElement();//Font
                        W.WriteEndElement();//td

                        W.WriteEndElement();//tr
                    }
                }
                W.WriteEndElement();//table
            }

        }


        private void createDocuHtmlContext(System.Xml.XmlWriter W)
        {
            try
            {
                if (this.checkBoxHtmlContext.Checked)
                {
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                    W.WriteStartElement("h1");
                    W.WriteString("Documentation for context: ");
                    W.WriteString(this.Context);
                    W.WriteEndElement();//h1
                    W.WriteEndElement();//Font

                    if (this.checkBoxHtmlIncludeAccessibility.Checked)
                    {
                        System.Collections.Generic.Dictionary<string, bool> DictAccess = new Dictionary<string, bool>();
                        DictAccess.Add(DiversityWorkbench.Entity.Accessibility.no_restrictions.ToString(), true);
                        DictAccess.Add(DiversityWorkbench.Entity.Accessibility.read_only.ToString(), !this.userControlColorSettings.HideReadOnly);
                        DictAccess.Add(DiversityWorkbench.Entity.Accessibility.inapplicable.ToString(), !this.userControlColorSettings.HideInapplicable);
                        this.createDocuHtmlColorLegendTable(W, "EntityAccessibility_Enum", DictAccess);
                    }

                    if (this.checkBoxHtmlIncludeDetermination.Checked)
                    {
                        System.Collections.Generic.Dictionary<string, bool> DictDet = new Dictionary<string, bool>();
                        DictDet.Add(DiversityWorkbench.Entity.Determination.user_defined.ToString(), true);
                        DictDet.Add(DiversityWorkbench.Entity.Determination.service_link.ToString(), !this.userControlColorSettings.HideServiceLink);
                        DictDet.Add(DiversityWorkbench.Entity.Determination.calculated.ToString(), !this.userControlColorSettings.HideCalculated);
                        this.createDocuHtmlColorLegendTable(W, "EntityDetermination_Enum", DictDet);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #endregion

        #region markdown
        private enum HugoMermaidType { Graph, Class, ER }
        private HugoMermaidType _hugoMermaidType = HugoMermaidType.Class;


        private void initDocuHugo()
        {
            this.comboBoxHugoMermaidType.Items.Add(HugoMermaidType.Class);
            this.comboBoxHugoMermaidType.Items.Add(HugoMermaidType.Graph);
            this.comboBoxHugoMermaidType.Items.Add(HugoMermaidType.ER);

            this.textBoxHugoRoot.Text = "/manual/dwb/" + DiversityWorkbench.Settings.ModuleName + "/database/database/";
            this.textBoxHugoMenuIconRoot.Text = "/manual/dwb/img/";
            this.initContentOfEnums();
        }

        private string HugoRoot { get { return this.textBoxHugoRoot.Text; } }

        private void comboBoxHugoMermaidType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (this.comboBoxHugoMermaidType.SelectedItem.ToString())
            {
                case "Graph":
                    _hugoMermaidType = HugoMermaidType.Graph;
                    break;
                case "Class":
                    _hugoMermaidType = HugoMermaidType.Class;
                    break;
                case "ER":
                    _hugoMermaidType = HugoMermaidType.ER;
                    break;
            }
        }

        private string createDocuMarkdown(string FileName)
        {
            //if (this.checkBoxHtmlContext.Checked && this.comboBoxContext.SelectedValue.ToString().Length == 0)
            //{
            //    System.Windows.Forms.MessageBox.Show("Please select a context");
            //    return "";
            //}

            if (this.checkedListBoxDocuTables.CheckedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select at least one table");
                return "";
            }

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //this._CheckedItems = null;

            if (FileName.Length == 0)
            {
                FileName = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation);
                if (this._outputType == OutputType.HUGO)
                    FileName += "Database";
                else
                    FileName += DiversityWorkbench.Settings.ModuleName;
                if (this.ForEnums) FileName += "_Enum";
                FileName += ".md";
            }
            System.IO.StreamWriter W = new System.IO.StreamWriter(FileName, false, System.Text.Encoding.UTF8);
            try
            {
                string Title = this.Title;
                if (ForEnums && _outputType != OutputType.HUGO) Title += " enumeration tables";
                if (_outputType == OutputType.HUGO)
                {
                    W.WriteLine("---");
                    Title = Title.Replace("Diversity", "Diversity ").Replace("Cache", " Cache");
                    W.WriteLine("title: " + Title);
                    if (ForEnums)
                    {
                        W.WriteLine("menutitle: Enumerations");
                        W.WriteLine("weight: 2");
                    }
                    else
                    {
                        W.WriteLine("menutitle: Tables, ...");
                        W.WriteLine("weight: 1");
                    }
                    if (this.checkBoxHugoMenuIcon.Checked && this.textBoxHugoMenuIcon.Text.Length > 0 && this.textBoxHugoMenuIconRoot.Text.Length > 0)
                    {
                        W.WriteLine("menuPre: <img src=\"" + this.textBoxHugoMenuIconRoot.Text.Trim() + this.textBoxHugoMenuIcon.Text.Trim() + "\" width=\"20\">&nbsp;");
                        W.WriteLine("---");
                        if (ForEnums)
                        {
                            W.WriteLine("");
                            W.WriteLine("# Enumeration tables");
                            W.WriteLine("");
                        }
                        else
                        {
                            string Objects = "";
                            string ObjectType = "";
                            this.progressBarHtmlDocu.Value = 0;
                            this.progressBarHtmlDocu.Maximum = this.checkedListBoxDocuTables.CheckedItems.Count;
                            int i = 0;
                            foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                            {
                                if (ObjectType != R[1].ToString()) // && this.HtmlDocuContainsMoreThenOneType())
                                {
                                    ObjectType = R[1].ToString();
                                    if (Objects.Length > 0) Objects += ", ";
                                    Objects += ObjectType.ToUpper() + "S";
                                    i++;
                                }
                            }
                            if (i > 1)
                            {
                                W.WriteLine("");
                                W.WriteLine("# " + Objects);
                                W.WriteLine("");
                            }
                        }
                        W.WriteLine("");
                        W.WriteLine("![](" + this.textBoxHugoMenuIconRoot.Text.Trim() + this.textBoxHugoMenuIcon.Text.Trim() + "?width=10vw)");
                        W.WriteLine("");
                    }
                    else
                        W.WriteLine("---");
                }
                else
                {
                    W.WriteLine("# ", Title);
                    this.createDocuMarkdownCSS(W, FileName);
                    this.createDocuMarkdownHeader(W);
                    this.createDocuMarkdownLogo(W);
                    this.createDocuMarkdownMetadata(W);
                }

                //this.createDocuHtmlFavicon(W);

                //this.createDocuHtmlContext(W);

                this.createDocuMarkdownCitation(W);

                this.createDocuMarkdownModel(W);

                if (_outputType != OutputType.HUGO)
                    this.createDocuMarkdownER(W);

                this.createDocuMarkdownNotes(W);

                this.createDocuMarkdownIndex(W);

                if (ForEnums)
                {
                    W.WriteLine("## Table");
                    W.WriteLine("");
                    this.createDocuMarkdownTable(W, DefaultEnumTableName(), "Table", true);
                }


                string CurrentType = "";
                this.progressBarHtmlDocu.Value = 0;
                this.progressBarHtmlDocu.Maximum = this.checkedListBoxDocuTables.CheckedItems.Count;
                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    if (CurrentType != R[1].ToString() && this.HtmlDocuContainsMoreThenOneType())
                    {
                        CurrentType = R[1].ToString();
                        W.WriteLine();
                        W.WriteLine("---");
                        W.WriteLine();
                        W.WriteLine("## " + CurrentType.ToUpper() + "S");
                        if (CurrentType == "TABLE" && _outputType == OutputType.HUGO && this.checkBoxHtml_ER_Include.Checked && this.textBoxHtmlER.Text.Length > 0 && this.textBoxHugoER_BasePath.Text.Length > 0)
                        {
                            W.WriteLine();
                            W.WriteLine("![ER-Diagram](" + this.textBoxHugoER_BasePath.Text + this.textBoxHtmlER.Text + ")");
                            W.WriteLine();
                        }
                    }
                    string ObjectType = R["ObjectType"].ToString().ToLower();
                    if (ObjectType == "table" && this.checkBoxListExcludeEnum.Checked && R[0].ToString().ToLower().EndsWith("_enum"))
                        continue;
                    W.WriteLine();
                    W.WriteLine("---");
                    W.WriteLine();
                    switch (ObjectType)
                    {
                        case "table":
                        case "view":
                            this.createDocuMarkdownTable(W, R[0].ToString(), R[1].ToString());
                            break;
                        case "function":
                        case "procedure":
                            this.createDocuMarkdownRoutine(W, R[0].ToString(), R[1].ToString());
                            break;
                        case "role":
                            this.createDocuMarkdownRole(W, R[0].ToString());
                            break;
                    }
                    if (this.progressBarHtmlDocu.Value < this.progressBarHtmlDocu.Maximum)
                    {
                        this.progressBarHtmlDocu.Value++;
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            return FileName;
        }

        private void createDocuMarkdownHeader(System.IO.TextWriter W)
        {
            // <a href="index.html"><img src="img/IcoHome.png" align="right"></a>
            try
            {
                string Title = DiversityWorkbench.Settings.ModuleName;
                if (ForEnums) Title += " enumeration tables";
                W.WriteLine("# " + Title);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            //this.createDocuHtmlLogo(W);
        }

        private void createDocuMarkdownLogo(System.IO.StreamWriter W)
        {
            if (this.checkBoxHtmlIncludeLogo.Checked)
            {
                try
                {
                    string LogoFile = "![](/img/Logo.svg)";
                    W.WriteLine(LogoFile);
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
        }

        private void createDocuMarkdownNotes(System.IO.StreamWriter W)
        {
            try
            {
                if (this.checkBoxHtmlIncludeNotesOnExclusions.Checked)
                {
                    if (this.HtmlExclusions.Count > 0)
                    {
                        W.WriteLine("> The following objects are not included:");

                        foreach (System.Collections.Generic.KeyValuePair<Exclusion, string> KV in this.HtmlExclusions)
                        {
                            W.WriteLine("> * " + KV.Value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownIndex(System.IO.StreamWriter W)
        {
            try
            {
                string CurrentType = "";
                if (this.checkBoxIndex.Checked)
                {
                    foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                    {
                        if (R[1].ToString().ToLower() == "table" && this.checkBoxListExcludeEnum.Checked && R[0].ToString().ToLower().EndsWith("_enum"))
                            continue;

                        if (CurrentType != R[1].ToString() && this.HtmlDocuContainsMoreThenOneType())
                        {
                            W.WriteLine();
                            W.WriteLine("#### " + R[1].ToString().ToUpper() + "S");
                            CurrentType = R[1].ToString();
                        }
                        this.createDocuMarkdownIndexEntry(W, R[0].ToString(), R[1].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownIndexEntry(System.IO.StreamWriter W, string Table, string Type)
        {
            try
            {
                W.WriteLine("- [" + Table + "](" + HugoRoot + "#" + Type.ToLower() + "-" + Table.ToLower() + ")");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownTrigger(System.IO.StreamWriter W, string Table)
        {
            System.Data.DataTable dtTrigger = this.dtTrigger(Table);

            foreach (System.Data.DataRow R in dtTrigger.Rows)
            {
                string Trigger = R[0].ToString();
                if (this.checkBoxHtmlExcludeStandardTrigger.Checked
                    && (Trigger.ToLower().StartsWith("trgupd")
                    || (Trigger.ToLower().StartsWith("trgdel"))))
                    continue;

                //if (!HtmlIncludeCSS)
                //{
                //    W.WriteStartElement("FONT");
                //    W.WriteAttributeString("face", "Verdana");
                //}

                //W.WriteStartElement("h4");
                W.WriteLine("#### " + Trigger);
                //W.WriteEndElement();//h4
                //W.WriteString("\r\n");
                if (this.checkBoxIncludeDescription.Checked)
                {
                    string DescriptionTrigger = DiversityWorkbench.Forms.FormFunctions.getDescription(Table, "TRIGGER", Trigger, this.Conn, _Schema);
                    W.WriteLine(DescriptionTrigger);
                }
                if (this.checkBoxIncludeDefinition.Checked)
                {
                    this.createDocuMarkdownDefinition(W, R[0].ToString());
                }
                //if (!HtmlIncludeCSS)
                //    W.WriteEndElement();//FONT
                //W.WriteString("\r\n");

            }
        }

        private void createDocuMarkdownTable(System.IO.StreamWriter W, string Object, string ObjectType, bool OnlyColumns = false)
        {
            if (Object.Length == 0)
                return;

            try
            {
                System.Data.DataTable dt = this.dtTableDocu(Object, this.checkBoxHtmlContext.Checked);

                string DisplayTextTable = Object;
                string DescriptionTable = "";

                if (!OnlyColumns)
                {
                    W.WriteLine("### " + ObjectType.Substring(0, 1).ToUpper() + ObjectType.Substring(1).ToLower() + " **" + DisplayTextTable + "**");
                    this.createDocuMarkdownHome(W, ObjectType);
                    //W.Write("   ");
                    if (this.checkBoxIncludeDescription.Checked)
                    {
                        if (!this.checkBoxHtmlContext.Checked)
                        {
                            DescriptionTable = DiversityWorkbench.Forms.FormFunctions.getDescription(Object, ObjectType, "", this.Conn, _Schema);
                            W.WriteLine(DescriptionTable);
                        }
                        else
                        {
                            W.WriteLine(this.FormFunctions.TableDescription(Object));
                        }
                    }
                }

                if (!this.checkBoxHtmlNoColumns.Checked || OnlyColumns)
                {
                    string ColumnNames = "";
                    string ColumnAlign = "";
                    foreach (System.Data.DataColumn CH in dt.Columns)
                    {
                        if (CH.ColumnName == "ORDINAL_POSITION")
                            continue;
                        if (CH.ColumnName == "Relation" && (!this.IncludeRelations || ObjectType.ToUpper() == "VIEW" || ForEnums))
                            continue;
                        if (CH.ColumnName == "Nullable" && !this.IncludeNullable)
                            continue;
                        if (!this.checkBoxIncludeDescription.Checked && CH.ColumnName == "Description")
                            continue;
                        if (CH.ColumnName != "Collation" && CH.ColumnName != "Length" && CH.ColumnName != "DefaultValue")
                        {
                            ColumnNames += "| ";
                            ColumnAlign += "| ";
                            string Text = CH.ColumnName.ToString();
                            if (Text == "ColumnName") Text = "Column";
                            if (Text == "Datatype") Text = "Data type";
                            ColumnNames += Text + " ";
                            switch (CH.ColumnName)
                            {
                                case "Datatype":
                                case "Nullable":
                                case "DefaultValue":
                                    ColumnAlign += ":-: ";
                                    break;
                                default:
                                    ColumnAlign += "--- ";
                                    break;
                            }
                        }
                    }
                    ColumnAlign += "|";
                    ColumnNames += "|";
                    W.WriteLine(ColumnNames);
                    W.WriteLine(ColumnAlign);
                    //W.WriteEndElement();//tr

                    // printing the informations for the columns
                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Description = "";
                        {
                            bool IsView = false;
                            if (ObjectType.ToLower() == "view")
                                IsView = true;
                            Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(Object, IsView, R["ColumnName"].ToString(), this.Conn, true, _Schema);
                        }

                        if (this.checkBoxHtmlExcludeColumnsObsolete.Checked && this.IsOutdated(Description))
                            continue;

                        if (Description.Length == 0) Description = "-";
                        R["Description"] = Description;
                        string Column = "";
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName == "ORDINAL_POSITION")
                                continue;
                            if (C.ColumnName == "Relation" && (!this.IncludeRelations || ObjectType.ToUpper() == "VIEW" || ForEnums))
                                continue;
                            if (C.ColumnName == "Nullable" && !this.IncludeNullable)
                                continue;
                            if (!this.checkBoxIncludeDescription.Checked && C.ColumnName == "Description")
                                continue;


                            bool IsPK = false;
                            string Default = "";
                            System.Data.DataTable dtPK = this.dtPrimaryKey(Object);
                            foreach (System.Data.DataRow PK in dtPK.Rows)
                            {
                                if (PK[0].ToString() == R[C.ColumnName].ToString())
                                {
                                    IsPK = true;
                                    break;
                                }
                            }
                            if (C.ColumnName != "DefaultValue")
                            {
                                {
                                    if (R["DefaultValue"].Equals(System.DBNull.Value)) Default = "";
                                    else
                                    {
                                        Default = R["DefaultValue"].ToString();
                                        if (Default.StartsWith("(")) Default = Default.Substring(1);
                                        if (Default.EndsWith(")")) Default = Default.Substring(0, Default.Length - 1);
                                        if (Default.Length > 0) Default = "Default value: " + Default;
                                    }
                                }
                            }
                            if (C.ColumnName != "Collation" && C.ColumnName != "Length" && C.ColumnName != "DefaultValue")
                            {
                                Column += "| ";
                                string T = R[C.ColumnName].ToString();
                                if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                                {
                                    //W.WriteAttributeString("nowrap", "nowrap");
                                    if (R["Length"].ToString() != "-1")
                                    {
                                        // Markus 18.4.23: no length for e.g. ntext (1073741823)
                                        if (R["Length"].ToString().Length < 6)
                                            T += " (" + R["Length"].ToString() + ")";
                                    }
                                    else if (T != "geography")
                                        T += " (MAX)";
                                }

                                if (IsPK) { Column += "__"; } // W.WriteStartElement("b"); W.WriteStartElement("u"); }
                                if (C.ColumnName != "Notes")
                                    Column += T;// W.WriteLine("__" + T + "__");
                                if (Default.Length > 0 && C.ColumnName == "Description")
                                {
                                    Column += Default; // W.WriteString(Default);
                                }
                                if (IsPK) { Column += "__"; } // { W.WriteEndElement(); W.WriteEndElement(); }//b
                            }
                        }
                        Column += "|";
                        W.WriteLine(Column);
                        i++;
                    }
                }

                if (!OnlyColumns)
                {
                    this.createDocuMarkdownDependencies(W, Object, ObjectType);

                    if (ObjectType.ToLower() == "table" && this.checkBoxIncludeTrigger.Checked)
                    {
                        this.createDocuMarkdownTrigger(W, Object);
                    }
                    else if (ObjectType.ToLower() == "view" && this.checkBoxIncludeDefinition.Checked) //(this.checkBoxIncludeTrigger.Checked || this.checkBoxIncludeDefinition.Checked))
                    {
                        this.createDocuMarkdownDefinition(W, Object);
                    }
                }

                if (this.checkBoxEnumContent.Checked)
                {
                    this.EnumContentAsMarkdown(W, Object);
                }

                if (this.listBoxTableContentColumns.Items.Count > 0)
                {
                    this.TableContentAsMarkdown(W, Object);
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownHome(System.IO.StreamWriter W, string ObjectType)
        {
            if (this.checkBoxIndex.Checked)
            {
                string Home = "[{{% icon icon=\"fas fa-chevron-circle-up\" %}}](" + HugoRoot + "#" + ObjectType.ToLower() + "s \"Index " + ObjectType[0].ToString().ToUpper() + ObjectType.Substring(1).ToLower() + "s\")    ";
                W.Write(Home);
            }
        }
        private void createDocuMarkdownRoutine(System.IO.StreamWriter W, string Routine, string RoutineType)
        {
            try
            {
                W.WriteLine("### " + RoutineType.Substring(0, 1).ToUpper() + RoutineType.Substring(1).ToLower() + " **" + Routine + "**");
                // Markus 9.1.2023 - Bugfix
                this.createDocuMarkdownHome(W, RoutineType);
                //W.Write("   ");
                string RoutineDescription = DiversityWorkbench.Forms.FormFunctions.getDescription(Routine, RoutineType, "", this.Conn, this._Schema);
                if (RoutineDescription.Length > 0)
                    W.WriteLine(RoutineDescription);
                string SQL = "SELECT DATA_TYPE + case when CHARACTER_MAXIMUM_LENGTH is null then '' else ' (' + cast(CHARACTER_MAXIMUM_LENGTH as varchar) + ')' end AS DataType " +
                    "FROM INFORMATION_SCHEMA.ROUTINES " +
                    "WHERE ROUTINE_NAME = '" + Routine + "' AND SPECIFIC_SCHEMA = '" + this._Schema + "' ";
                string DataType = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this.Conn);
                if (DataType.ToUpper() != "TABLE" && DataType.Length > 0)
                {
                    //W.WriteLine("");
                    W.WriteLine("");

                    W.WriteLine("DataType: " + DataType);
                }
                // PARAMETERS OF THE FUNCTION
                System.Data.DataTable dt = new DataTable();
                if (RoutineType.ToLower() == "function")
                    dt = DiversityWorkbench.Data.Routine.Parameters(Routine, DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION);// this.dtTableDocu(Routine, false);
                else
                    dt = DiversityWorkbench.Data.Routine.Parameters(Routine, DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.PROCEDURE);
                if (dt.Rows.Count > 0)
                {
                    //W.WriteElementString("br", "");
                    //W.WriteElementString("br", "");
                    //W.WriteStartElement("table");
                    //W.WriteAttributeString("cellpadding", "3");
                    //W.WriteAttributeString("cellspacing", "0");
                    //W.WriteAttributeString("border", "1");
                    //W.WriteAttributeString("width", "100%");

                    //W.WriteStartElement("tr");
                    string ColumnAlign = "";
                    string ColumnNames = "";
                    foreach (System.Data.DataColumn CH in dt.Columns)
                    {
                        if (CH.ColumnName.ToUpper() == "ORDINAL_POSITION")
                            continue;
                        if (CH.ColumnName != "Collation" && CH.ColumnName != "Length")
                        {
                            //W.WriteStartElement("th");
                            //W.WriteAttributeString("nowrap", "nowrap");
                            //W.WriteAttributeString("align", "left");
                            //if (!HtmlIncludeCSS)
                            //    W.WriteAttributeString("bgcolor", "lightgrey");
                            //if (!HtmlIncludeCSS)
                            //{
                            //    W.WriteStartElement("FONT");
                            //    W.WriteAttributeString("face", "Verdana");
                            //}
                            string Text = CH.ColumnName.ToString();
                            if (Text.ToUpper() == "NAME") Text = "Parameter";
                            if (Text == "Datatype") Text = "Type";
                            ColumnNames += "| " + Text + " ";
                            switch (CH.ColumnName.ToString())
                            {
                                case "DataType":
                                    ColumnAlign += " | :-: ";
                                    break;
                                default:
                                    ColumnAlign += " | --- ";
                                    break;
                            }
                            //if (!HtmlIncludeCSS)
                            //    W.WriteEndElement();//FONT
                            //W.WriteEndElement();//th
                            //W.WriteString("\r\n");
                        }
                    }
                    W.WriteLine(ColumnNames + "|");
                    W.WriteLine(ColumnAlign + "|");
                    //W.WriteEndElement();//tr

                    // printing the informations for the columns
                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Description = R["Description"].ToString();// DiversityWorkbench.Forms.FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION, Routine, R["ColumnName"].ToString());
                        //W.WriteStartElement("tr");
                        if (Description.Length == 0)
                        {
                            Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescriptionOfBase(Routine, R[0].ToString().Substring(1));
                            if (Description.Length == 0)
                                Description = "-";
                        }
                        R["Description"] = Description;
                        string Column = "";
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName.ToUpper() == "ORDINAL_POSITION")
                                continue;

                            if (C.ColumnName != "Collation" && C.ColumnName != "Length" && C.ColumnName != "DefaultValue")
                            {
                                //W.WriteStartElement("td");

                                string T = R[C.ColumnName].ToString();
                                if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                                {
                                    //W.WriteAttributeString("nowrap", "nowrap");
                                    if (R["Length"].ToString() != "-1")
                                        T += " (" + R["Length"].ToString() + ")";
                                    else if (T != "geography")
                                        T += " (MAX)";
                                }

                                //if (!HtmlIncludeCSS)
                                //{
                                //    W.WriteStartElement("FONT");
                                //    W.WriteAttributeString("face", "Verdana");
                                //}
                                //W.WriteAttributeString("size", "2");
                                if (C.ColumnName != "Notes")
                                    Column += "| " + T + " "; // W.WriteLine(T);
                                //if (!HtmlIncludeCSS)
                                //    W.WriteEndElement();//FONT
                                //W.WriteEndElement();//td
                                //W.WriteString("\r\n");
                            }
                        }
                        W.WriteLine(Column + "|");
                        //W.WriteEndElement();//tr
                        //W.WriteString("\r\n");
                        i++;
                    }
                    //W.WriteEndElement();//table
                }

                // COLUMNS OF THE FUNCTION
                dt = DiversityWorkbench.Data.Routine.Columns(Routine);// this.dtTableDocu(Routine, false);
                if (dt.Rows.Count > 0)
                {
                    W.WriteLine();

                    string ColumnAlign = "";
                    string ColumnNames = "";

                    //W.WriteElementString("br", "");
                    //W.WriteElementString("br", "");
                    //W.WriteStartElement("table");
                    //W.WriteAttributeString("cellpadding", "3");
                    //W.WriteAttributeString("cellspacing", "0");
                    //W.WriteAttributeString("border", "1");
                    //W.WriteAttributeString("width", "100%");

                    //W.WriteStartElement("tr");
                    foreach (System.Data.DataColumn CH in dt.Columns)
                    {
                        if (CH.ColumnName.ToUpper() == "ORDINAL_POSITION")
                            continue;
                        //W.WriteStartElement("th");
                        //W.WriteAttributeString("nowrap", "nowrap");
                        //W.WriteAttributeString("align", "left");
                        //if (!HtmlIncludeCSS)
                        //    W.WriteAttributeString("bgcolor", "lightgrey");
                        //if (!HtmlIncludeCSS)
                        //{
                        //    W.WriteStartElement("FONT");
                        //    W.WriteAttributeString("face", "Verdana");
                        //}
                        string Text = CH.ColumnName.ToString();
                        if (Text == "Name") Text = "Column";
                        if (Text == "Datatype") Text = "Type";
                        ColumnNames += "| " + Text + " ";// W.WriteLine(Text);
                        switch (CH.ColumnName.ToString())
                        {
                            case "DataType":
                                ColumnAlign += "| :-: ";
                                break;
                            default:
                                ColumnAlign += "| --- ";
                                break;
                        }
                        //if (!HtmlIncludeCSS)
                        //    W.WriteEndElement();//FONT
                        //W.WriteEndElement();//th
                        //W.WriteString("\r\n");
                    }
                    W.WriteLine(ColumnNames + "|");
                    W.WriteLine(ColumnAlign + "|");
                    //W.WriteEndElement();//tr

                    // printing the informations for the columns
                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Description = R["Description"].ToString();// DiversityWorkbench.Forms.FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION, Routine, R["ColumnName"].ToString());
                        //W.WriteStartElement("tr");
                        if (Description.Length == 0)
                        {
                            Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescriptionOfBase(Routine, R[0].ToString(), this.Conn, 1);
                            if (Description.Length == 0)
                                Description = "-";
                        }
                        R["Description"] = Description;
                        string Column = "";
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName.ToUpper() == "ORDINAL_POSITION")
                                continue;

                            //W.WriteStartElement("td");

                            string T = R[C.ColumnName].ToString();
                            if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                            {
                                //W.WriteAttributeString("nowrap", "nowrap");
                                if (R["Length"].ToString() != "-1")
                                    T += " (" + R["Length"].ToString() + ")";
                                else if (T != "geography")
                                    T += " (MAX)";
                            }

                            //if (!HtmlIncludeCSS)
                            //{
                            //    W.WriteStartElement("FONT");
                            //    W.WriteAttributeString("face", "Verdana");
                            //}
                            //W.WriteAttributeString("size", "2");
                            if (C.ColumnName != "Notes")
                                Column += "| " + T + " ";// W.WriteLine(T);
                            //if (!HtmlIncludeCSS)
                            //    W.WriteEndElement();//FONT
                            //W.WriteEndElement();//td
                            //W.WriteString("\r\n");
                        }
                        W.WriteLine(Column + "|");
                        //W.WriteEndElement();//tr
                        //W.WriteString("\r\n");
                        i++;
                    }
                    //W.WriteEndElement();//table
                }

                this.createDocuMarkdownDependencies(W, Routine, RoutineType);

                //W.WriteElementString("br", "");

                if (this.checkBoxIncludeDefinition.Checked)
                {
                    this.createDocuMarkdownDefinition(W, Routine);
                }

                //W.WriteElementString("br", "");
                //W.WriteString("\r\n");

                //W.WriteElementString("br", "");
                //W.WriteElementString("br", "");
                //W.WriteString("\r\n");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void createDocuMarkdownDefinition(System.IO.StreamWriter W, string Object)
        {
            string SQL = "EXEC sp_helptext '" + Object + "';";
            /// Markus 23.1.2023: Reducing number of connections
            System.Data.DataTable dtDef = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);// new DataTable();

            //W.WriteStartElement("FONT");
            //W.WriteAttributeString("face", "Verdana");
            //W.WriteAttributeString("size", "2");
            bool IsComment = false;
            foreach (System.Data.DataRow Rdef in dtDef.Rows)
            {
                string Text = Rdef[0].ToString().Trim();

                // Avoiding comment
                if (Text.StartsWith("/*"))
                {
                    IsComment = true;
                    if (Text.EndsWith("*/"))
                        IsComment = false;
                    continue;
                }
                if (IsComment && Text.EndsWith("*/"))
                {
                    IsComment = false;
                    continue;
                }
                if (IsComment)
                    continue;
                if (Text.StartsWith("--"))
                    continue;

                // Avoiding CREATE
                if (Text.StartsWith("CREATE "))
                {
                    //W.WriteStartElement("b");
                    W.WriteLine("**Definition**:");
                    //W.WriteElementString("br", "");
                    //W.WriteEndElement();//b
                    if (Text.IndexOf("/*") > -1 && Text.IndexOf("*/") > -1)
                    {
                        string t1 = Text.Substring(0, Text.IndexOf("/*"));
                        string t2 = Text.Substring(Text.IndexOf("*/") + 2);
                        Text = t1 + " " + t2;
                        Text = Text.Replace("  ", " ");
                    }
                    if (Text.StartsWith("()"))
                        Text = Text.Substring(2).Trim();
                    if (Text == "()")
                        continue;
                    else if (Text.IndexOf(" ()") > -1 && !Text.EndsWith("()"))
                    {
                        Text = Text.Substring(Text.IndexOf(" ()") + 3).Trim();
                        if (Text.Length == 0)
                            continue;
                        W.WriteLine(Text);
                        //W.WriteElementString("br", "");
                    }
                    else if (Text.IndexOf("()") > -1 && Text.EndsWith("()") && Text.IndexOf("()") == Text.LastIndexOf("()"))
                    {
                        continue;
                    }
                    else if (Text.IndexOf(" RETURNS ") > -1)
                    {
                        W.WriteLine(Text.Substring(Text.IndexOf(" RETURNS ")));
                        //W.WriteElementString("br", "");
                    }
                    else if (Text.IndexOf(" AS ") > -1)
                    {
                        W.WriteLine(Text.Substring(Text.IndexOf(" AS ")));
                        //W.WriteElementString("br", "");
                    }
                    else if (Text.IndexOf(" (") > -1)
                    {
                        W.WriteLine(Text.Substring(Text.IndexOf(" (")));
                        //W.WriteElementString("br", "");
                    }
                    continue;
                }
                if (Text.Length == 0)
                    continue;
                W.WriteLine(Text);
                //W.WriteElementString("br", "");
            }
        }

        private void createDocuMarkdownRole(System.IO.StreamWriter W, string Role)
        {
            try
            {
                //W.WriteStartElement("FONT");
                //W.WriteAttributeString("face", "Verdana");

                //W.WriteStartElement("h3");
                //W.WriteStartElement("a");
                //W.WriteAttributeString("name", Role);
                //W.WriteStartElement("u");
                W.WriteLine("### Role **" + Role + "**");
                this.createDocuMarkdownHome(W, "Role");
                //W.Write("   ");
                //W.WriteEndElement();//u
                //W.WriteEndElement();//a
                //W.WriteEndElement();//h3
                //W.WriteString("\r\n");
                W.WriteLine(FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.USER, Role, ""));
                //W.WriteElementString("br", "");
                //W.WriteElementString("br", "");

                this.createDocuMarkdownPermissions(W, Role);
                this.createDocuMarkdownIncludedRoles(W, Role);

                //W.WriteElementString("br", "");
                //W.WriteElementString("br", "");
                //W.WriteEndElement();//FONT
                //W.WriteString("\r\n");

                //W.WriteElementString("br", "");
                //W.WriteElementString("br", "");
                //W.WriteString("\r\n");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownPermissions(System.IO.StreamWriter W, string Role)
        {
            try
            {
                // Objects
                System.Collections.Generic.List<string> Objects = new List<string>();
                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    if (R["ObjectType"].ToString().ToLower() != "role")
                        Objects.Add(R[0].ToString());
                }
                //W.WriteStartElement("table");
                //W.WriteAttributeString("cellpadding", "3");
                //W.WriteAttributeString("cellspacing", "0");
                //W.WriteAttributeString("border", "1");
                //W.WriteAttributeString("width", "100%");

                // Permissions
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Data.Role.Permission, string>> PerDict = DiversityWorkbench.Data.Role.AllPermissions(Role);

                string HeaderNames = "";
                string HeaderAlign = "";
                // Table header
                System.Collections.Generic.List<Data.Role.Permission> PermissionColumns = new List<Data.Role.Permission>();
                PermissionColumns.Add(Data.Role.Permission.SELECT);
                PermissionColumns.Add(Data.Role.Permission.INSERT);
                PermissionColumns.Add(Data.Role.Permission.UPDATE);
                PermissionColumns.Add(Data.Role.Permission.DELETE);
                PermissionColumns.Add(Data.Role.Permission.EXECUTE);
                //W.WriteStartElement("tr");
                // Column for Securables
                //W.WriteStartElement("th");
                //W.WriteAttributeString("nowrap", "nowrap");
                //W.WriteAttributeString("align", "left");
                //if (!HtmlIncludeCSS)
                //    W.WriteAttributeString("bgcolor", "lightgrey");
                //W.WriteStartElement("FONT");
                //W.WriteAttributeString("face", "Verdana");
                HeaderNames += "| Permissions ";// W.WriteLine("Permissions");
                HeaderAlign += "| --- ";
                //W.WriteEndElement();//FONT
                //W.WriteEndElement();//th
                foreach (Data.Role.Permission C in PermissionColumns)
                {
                    //W.WriteStartElement("th");
                    //W.WriteAttributeString("nowrap", "nowrap");
                    //W.WriteAttributeString("align", "left");
                    //if (!HtmlIncludeCSS)
                    //    W.WriteAttributeString("bgcolor", "lightgrey");
                    //W.WriteStartElement("FONT");
                    //W.WriteAttributeString("face", "Verdana");
                    HeaderNames += "| " + C.ToString() + " ";
                    HeaderAlign += "| --- ";// W.WriteLine(C.ToString());
                    //W.WriteEndElement();//FONT
                    //W.WriteEndElement();//th
                    //W.WriteString("\r\n");
                }
                // Column for Type
                //W.WriteStartElement("th");
                //W.WriteAttributeString("nowrap", "nowrap");
                //W.WriteAttributeString("align", "left");
                //if (!HtmlIncludeCSS)
                //    W.WriteAttributeString("bgcolor", "lightgrey");
                //W.WriteStartElement("FONT");
                //W.WriteAttributeString("face", "Verdana");
                HeaderNames += "| Type |"; //W.WriteLine("Type");
                HeaderAlign += "| --- |";
                W.WriteLine(HeaderNames);
                W.WriteLine(HeaderAlign);
                //W.WriteEndElement();//FONT
                //W.WriteEndElement();//th
                //W.WriteEndElement();//tr

                // printing the informations for the columns
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<Data.Role.Permission, string>> KVobj in PerDict)
                {
                    if (this.checkBoxExcludeSystemObjects.Checked)
                    {
                        // exclude the system objects
                        if (this.DatabaseSystemObjects.ContainsKey(KVobj.Key))
                            continue;

                        if (!KVobj.Key.EndsWith("_Enum")) // enums are included in any case with this option
                        {
                            if (KVobj.Key.EndsWith("_log"))
                            {
                                // Include log tables for selected objects
                                string DataTable = KVobj.Key.Remove(KVobj.Key.Length - 4);
                                if (!Objects.Contains(DataTable))
                                    continue;
                            }
                            else if (!Objects.Contains(KVobj.Key))
                                continue;
                        }
                    }
                    else
                    {
                        // if no objects are selected, show all otherwise restrict to selected
                        if (Objects.Count > 0 && !Objects.Contains(KVobj.Key))
                            continue;
                    }

                    //W.WriteStartElement("tr");


                    // Column for ObjectName
                    //W.WriteStartElement("td");
                    //W.WriteStartElement("FONT");
                    //W.WriteAttributeString("face", "Verdana");
                    //W.WriteAttributeString("size", "2");
                    string Permissions = "| " + KVobj.Key + " ";
                    //W.WriteLine(KVobj.Key);
                    //W.WriteEndElement();//FONT
                    //W.WriteEndElement();//td

                    // Columns for Permissions
                    foreach (Data.Role.Permission C in PermissionColumns)
                    {
                        //W.WriteStartElement("td");
                        //W.WriteAttributeString("align", "center");
                        //W.WriteStartElement("FONT");
                        //W.WriteAttributeString("face", "Verdana");
                        if (KVobj.Value[C].Length == 0)
                        {
                            Permissions += "| ";
                            //W.Write(" ");
                        }
                        else if (KVobj.Value[C] == Role)
                        {
                            //W.WriteAttributeString("size", "4");
                            Permissions += "| &bull; ";
                            //W.Write("&bull;");
                        }
                        else
                        {
                            //if (!HtmlIncludeCSS)
                            //    W.WriteAttributeString("color", "grey");
                            //W.WriteAttributeString("size", "1");
                            string InheritedFrom = KVobj.Value[C];
                            string PrintedRole = InheritedFrom.Substring(0, 1);
                            for (int ii = 1; ii < InheritedFrom.Length; ii++)
                            {
                                if (InheritedFrom[ii].ToString() == InheritedFrom[ii].ToString().ToUpper())
                                {
                                    PrintedRole += " ";
                                }
                                PrintedRole += InheritedFrom[ii];
                            }
                            Permissions += "| " + PrintedRole + " ";
                            //W.Write(PrintedRole);
                        }
                        //W.WriteLine(Permissions + "|");
                        //W.WriteEndElement();//FONT
                        //W.WriteEndElement();//td
                        //W.WriteString("\r\n");
                    }

                    // Column for Type
                    //W.WriteStartElement("td");
                    //W.WriteStartElement("FONT");
                    //W.WriteAttributeString("face", "Verdana");
                    //W.WriteAttributeString("size", "2");
                    string Type = DiversityWorkbench.Data.Role.Securables()[KVobj.Key];
                    if (Type == "BASE TABLE")
                        Type = "TABLE";
                    //W.Write(Type);
                    Permissions += "| " + Type + " |";
                    W.WriteLine(Permissions);
                    //W.WriteEndElement();//FONT
                    //W.WriteEndElement();//td

                    //W.WriteEndElement();//tr
                    //W.WriteString("\r\n");
                    i++;
                }
                //W.WriteEndElement();//table
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Data.DataTable DtDependency(string Object, string ObjectType = "")
        {
            string SQL = "";
            if (ObjectType.Length == 0)
            {
                SQL = "SELECT DISTINCT B.TABLE_NAME from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS A " +
                "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE D on A.UNIQUE_CONSTRAINT_NAME = D.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B on A.CONSTRAINT_NAME = B.CONSTRAINT_NAME " +
                "WHERE D.TABLE_NAME = '" + Object + "' and B.TABLE_NAME <> '" + Object + "'; ";
            }
            else
            {
                SQL = "SELECT DISTINCT referenced_entity_name " +
                    "FROM sys.sql_expression_dependencies " +
                    "WHERE referencing_id = OBJECT_ID(N'dbo." + Object + "') and not referenced_id is null;";
                if (ObjectType.ToLower() == "table")
                {
                    SQL = "SELECT DISTINCT P.TABLE_NAME " +
                        "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R  " +
                        "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS F ON R.CONSTRAINT_NAME = F.CONSTRAINT_NAME  " +
                        "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME  " +
                        "RIGHT OUTER JOIN INFORMATION_SCHEMA.COLUMNS AS C ON F.COLUMN_NAME = C.COLUMN_NAME AND F.TABLE_NAME = C.TABLE_NAME AND F.TABLE_SCHEMA = C.TABLE_SCHEMA AND F.TABLE_CATALOG = C.TABLE_CATALOG    " +
                        "WHERE C.TABLE_NAME = '" + Object + "' and not P.TABLE_NAME is null and P.TABLE_NAME <> '" + Object + "' " +
                        "ORDER BY P.TABLE_NAME";
                }
                if (ObjectType == "USER")
                {

                }
            }
            /// Markus 23.1.2023: Reducing number of connections
            System.Data.DataTable dtDepend = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL); // new DataTable();
            return dtDepend;
        }

        private void createDocuMarkdownDependencies(System.IO.StreamWriter W, string Object, string ObjectType)
        {
            try
            {
                if (this.checkBoxIncludeLookupRelations.Checked)
                {
                    //string SQL = "SELECT DISTINCT referenced_entity_name " +
                    //    "FROM sys.sql_expression_dependencies " +
                    //    "WHERE referencing_id = OBJECT_ID(N'dbo." + Object + "') and not referenced_id is null;";
                    //if (ObjectType.ToLower() == "table")
                    //{
                    //    SQL = "SELECT DISTINCT P.TABLE_NAME " +
                    //        "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R  " +
                    //        "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS F ON R.CONSTRAINT_NAME = F.CONSTRAINT_NAME  " +
                    //        "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME  " +
                    //        "RIGHT OUTER JOIN INFORMATION_SCHEMA.COLUMNS AS C ON F.COLUMN_NAME = C.COLUMN_NAME AND F.TABLE_NAME = C.TABLE_NAME AND F.TABLE_SCHEMA = C.TABLE_SCHEMA AND F.TABLE_CATALOG = C.TABLE_CATALOG    " +
                    //        "WHERE C.TABLE_NAME = '" + Object + "' and not P.TABLE_NAME is null and P.TABLE_NAME <> '" + Object + "' " +
                    //        "ORDER BY P.TABLE_NAME";
                    //}
                    //if (ObjectType == "USER")
                    //{

                    //}
                    /// Markus 23.1.2023: Reducing number of connections
                    System.Data.DataTable dtDepend = this.DtDependency(Object, ObjectType); // DiversityWorkbench.Forms.FormFunctions.DataTable(SQL); // new DataTable();
                    //string Message = "";
                    //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtDepend, ref Message);
                    if (dtDepend.Rows.Count > 0)
                    {
                        //W.WriteString("\r\n");
                        ////W.WriteStartElement("p");
                        //if (!HtmlIncludeCSS)
                        //{
                        //    W.WriteStartElement("FONT");
                        //    W.WriteAttributeString("face", "Verdana");
                        //}
                        //W.WriteStartElement("h4");
                        W.WriteLine("#### Depending on:");
                        //W.WriteEndElement();//h6
                        //W.WriteString("\r\n");
                        //W.WriteStartElement("ul");
                        //W.WriteString("\r\n");
                        foreach (System.Data.DataRow rr in dtDepend.Rows)
                        {
                            //W.WriteStartElement("li");
                            //if (!HtmlIncludeCSS)
                            //{
                            //    W.WriteStartElement("FONT");
                            //    W.WriteAttributeString("face", "Verdana");
                            //}
                            W.WriteLine("- " + rr[0].ToString());
                            //W.WriteEndElement();//li
                            //if (!HtmlIncludeCSS)
                            //    W.WriteEndElement();//FONT
                            //W.WriteString("\r\n");
                        }
                        //if (!HtmlIncludeCSS)
                        //    W.WriteEndElement();//FONT
                        //W.WriteEndElement();//ul
                    }
                }

                if (this.checkBoxHtmlIncludeDepending.Checked && ObjectType.ToLower() == "table")
                {
                    //string SQL = "select distinct B.TABLE_NAME from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS A " +
                    //    "inner join INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE D on A.UNIQUE_CONSTRAINT_NAME = D.CONSTRAINT_NAME " +
                    //    "inner join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B on A.CONSTRAINT_NAME = B.CONSTRAINT_NAME " +
                    //    "where D.TABLE_NAME = '" + Object + "' and B.TABLE_NAME <> '" + Object + "'; ";
                    System.Data.DataTable dtDepend = this.DtDependency(Object); // DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                    if (dtDepend.Rows.Count > 0)
                    {
                        //W.WriteStartElement("p");
                        //W.WriteStartElement("FONT");
                        //W.WriteAttributeString("face", "Verdana");
                        W.WriteLine("#### Dependent tables:");
                        //W.WriteStartElement("ul");
                        foreach (System.Data.DataRow rr in dtDepend.Rows)
                        {
                            //W.WriteStartElement("li");
                            //W.WriteStartElement("FONT");
                            //W.WriteAttributeString("face", "Verdana");
                            W.WriteLine("- " + rr[0].ToString());
                            //W.WriteEndElement();//li
                            //W.WriteEndElement();//FONT
                        }
                        //W.WriteEndElement();//FONT
                        //W.WriteEndElement();//ul
                        //W.WriteEndElement();//p
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void createDocuMarkdownIncludedRoles(System.IO.StreamWriter W, string Object)
        {
            if (this.checkBoxIncludeLookupRelations.Checked)
            {
                System.Collections.Generic.List<string> Roles = DiversityWorkbench.Data.Role.InheritingFromRoles(Object);
                if (Roles.Count > 0)
                {
                    //W.WriteStartElement("p");
                    //W.WriteStartElement("FONT");
                    //W.WriteAttributeString("face", "Verdana");
                    W.WriteLine("Inheriting from roles:");
                    //W.WriteStartElement("ul");
                    foreach (string R in Roles)
                    {
                        //W.WriteStartElement("li");
                        //W.WriteStartElement("FONT");
                        //W.WriteAttributeString("face", "Verdana");
                        W.WriteLine(" - " + R);
                        //W.WriteEndElement();//li
                        //W.WriteEndElement();//FONT
                    }
                    //W.WriteEndElement();//FONT
                    //W.WriteEndElement();//ul
                    //W.WriteEndElement();//p
                }
            }
        }


        #endregion

        #region Content of enums

        private void initContentOfEnums()
        {
            foreach (string c in this.EnumColumnList())
            {
                this.checkedListBoxEnumContent.Items.Add(c);
            }
        }

        private void EnumContentAsMarkdown(System.IO.StreamWriter W, string TableName)
        {
            try
            {
                if (this.checkBoxEnumContent.Checked)
                {
                    System.Data.DataTable dt = this.EnumContent(TableName);
                    if (dt.Rows.Count > 0)
                    {
                        string Header = "";
                        string Subline = "";
                        this.EnumContentMarkdownHeader(ref Header, ref Subline);
                        if (Header.Length > 0)
                        {
                            W.WriteLine("#### Content");
                            W.WriteLine(Header);
                            W.WriteLine(Subline);
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                W.WriteLine(this.EnumContentMarkdownRowContent(R));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void EnumContentMarkdownHeader(ref string Header, ref string SubLine)
        {
            foreach (System.Object o in this.checkedListBoxEnumContent.CheckedItems)
            {
                if (Header.Length > 0)
                {
                    Header += " | ";
                    SubLine += " | ";
                }
                Header += o.ToString();
                SubLine += " --- ";
            }
            Header = "| " + Header + " |";
            SubLine = "| " + SubLine + " |";
        }

        private string EnumContentMarkdownRowContent(System.Data.DataRow R)
        {
            string Content = "";
            foreach (System.Object o in this.checkedListBoxEnumContent.CheckedItems)
            {
                if (Content.Length > 0) Content += " | ";
                string content = " ";
                if (R.Table.Columns.Contains(o.ToString()) && !R[o.ToString()].Equals(System.DBNull.Value))
                {
                    content = R[o.ToString()].ToString();
                }
                Content += content;
            }
            Content = "| " + Content + " |";
            return Content;
        }


        private System.Data.DataTable EnumContent(string TableName)
        {
            string SQL = "";
            if (this.checkedListBoxEnumContent.CheckedItems.Count > 0) // #262
            {
                foreach (System.Object o in this.checkedListBoxEnumContent.CheckedItems)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += o.ToString();
                }
                SQL = "SELECT " + SQL + " FROM " + TableName;
                System.Data.DataTable dataTable = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                return dataTable;
            }
            System.Data.DataTable table = new DataTable();
            return table;
        }

        System.Collections.Generic.List<string> _EnumColumnList;
        private System.Collections.Generic.List<string> EnumColumnList()
        {
            if (_EnumColumnList == null)
            {
                _EnumColumnList = new List<string>();
                string SQL = "SELECT distinct t.COLUMN_NAME " +
                    "FROM INFORMATION_SCHEMA.COLUMNS AS c " +
                    "inner join INFORMATION_SCHEMA.COLUMNS AS t on C.TABLE_NAME = t.TABLE_NAME " +
                    "WHERE(c.TABLE_NAME LIKE '%_enum') and c.COLUMN_NAME = 'code' and c.ORDINAL_POSITION = 1 and c.IS_NULLABLE = 'no'";
                System.Data.DataTable dt = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                if (dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow R in dt.Rows)
                        _EnumColumnList.Add(R[0].ToString());
                }
            }
            return _EnumColumnList;
        }

        #endregion

        #endregion

        private bool HtmlDocuContainsMoreThenOneType()
        {
            bool ManyTypes = true;
            int CountTypes = 0;
            if (this.checkBoxIncludeFunctions.Checked) CountTypes++;
            if (this.checkBoxIncludeTables.Checked) CountTypes++;
            if (this.checkBoxIncludeViews.Checked) CountTypes++;
            if (this.checkBoxIncludeRoles.Checked) CountTypes++;
            if (CountTypes < 2) ManyTypes = false;
            return ManyTypes;
        }


        private void checkBoxHtmlIncludeDepending_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBoxHtmlIncludeNotesOnExclusions_CheckedChanged(object sender, EventArgs e)
        {
        }

        #region Creation

        private void createDocuHeader(System.Xml.XmlWriter W)
        {
            // <a href="index.html"><img src="img/IcoHome.png" align="right"></a>
            try
            {
                W.WriteStartElement("h2");
                W.WriteString(Title);
                W.WriteStartElement("a");
                W.WriteAttributeString("href", "index.html");
                W.WriteStartElement("img");
                W.WriteAttributeString("src", "img/IcoHome.png");
                W.WriteAttributeString("align", "right");
                W.WriteEndElement();//img
                W.WriteEndElement();//a
                W.WriteEndElement();//h2
                W.WriteString("\r\n");

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            //this.createDocuHtmlLogo(W);
        }

        private string Title
        {
            get
            {
                string SQL = "SELECT [dbo].[DiversityWorkbenchModule] ()";
                string Title = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this.Conn);
                if (ForEnums) Title += " enumeration tables";
                return Title;
            }
        }


        private void createDocuHtmlLogo(System.Xml.XmlWriter W)
        {
            if (this.checkBoxHtmlIncludeLogo.Checked)
            {
                try
                {
                    string LogoFile = "img/Logo.svg";
                    // Image not present in output directory, but should be present in target directory. decision according to selection of homebutton
                    //System.IO.FileInfo file = new System.IO.FileInfo(LogoFile);
                    if (this.checkBoxHtmlIncludeHomeButton.Checked)// file.Exists)
                    {
                        W.WriteStartElement("img");
                        W.WriteAttributeString("src", LogoFile);
                        W.WriteAttributeString("align", "right");
                        W.WriteEndElement();//img
                        W.WriteString("\r\n");
                    }

                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
        }


        private void createDocuHtmlNotes(System.Xml.XmlWriter W)
        {
            try
            {
                if (this.checkBoxHtmlIncludeNotesOnExclusions.Checked)
                {
                    if (this.HtmlExclusions.Count > 0)
                    {
                        if (!this.HtmlIncludeCSS)
                        {
                            W.WriteStartElement("FONT");
                            W.WriteAttributeString("face", "Verdana");
                            W.WriteAttributeString("size", "small");
                        }
                        //else if(HtmlIncludeDWBcss)
                        //{
                        //    W.WriteStartElement("p");
                        //    W.WriteAttributeString("class", "info");
                        //    W.WriteString("\r\n");
                        //}
                        W.WriteStartElement("h4");
                        if (this.HtmlIncludeDWBcss)
                            W.WriteAttributeString("class", "infoheader");
                        W.WriteString("The following objects are not included:");
                        W.WriteEndElement();//h1
                        W.WriteString("\r\n");

                        W.WriteStartElement("ul");
                        if (this.HtmlIncludeDWBcss)
                            W.WriteAttributeString("class", "info");
                        W.WriteString("\r\n");

                        foreach (System.Collections.Generic.KeyValuePair<Exclusion, string> KV in this.HtmlExclusions)
                        {
                            W.WriteStartElement("li");
                            W.WriteString(KV.Value);
                            W.WriteEndElement();//li
                            W.WriteString("\r\n");

                        }

                        W.WriteEndElement();//ul
                        if (!this.HtmlIncludeCSS)
                            W.WriteEndElement();//Font                        
                        //else if (HtmlIncludeDWBcss)
                        //    W.WriteEndElement();//p                        
                        W.WriteString("\r\n");

                        W.WriteElementString("br", "");
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuHtmlIndex(System.Xml.XmlWriter W)
        {
            try
            {
                string CurrentType = "";
                if (this.checkBoxIndex.Checked)
                {
                    W.WriteString("\r\n");
                    if (!this.HtmlIncludeCSS)
                    {
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                    }
                    //W.WriteStartElement("h3");
                    //W.WriteString("Index");
                    //W.WriteEndElement();//h3
                    if (!this.HtmlIncludeCSS)
                        W.WriteEndElement();//FONT
                    //if (this.checkBoxHtmlIncludeHomeButton.Checked)
                    //    this.createDocuHtmlHome(W);
                    W.WriteString("\r\n");
                    //if (this.HtmlDocuContainsMoreThenOneType())
                    //{
                    //    W.WriteStartElement("ul");
                    //    W.WriteString("\r\n");
                    //}

                    W.WriteStartElement("ul");
                    W.WriteString("\r\n");


                    foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                    {
                        if (R[1].ToString().ToLower() == "table" && this.checkBoxListExcludeEnum.Checked && R[0].ToString().ToLower().EndsWith("_enum"))
                            continue;

                        if (CurrentType != R[1].ToString() && this.HtmlDocuContainsMoreThenOneType())
                        {
                            if (CurrentType.Length > 0)
                            {
                                W.WriteEndElement();//ul
                                W.WriteString("\r\n");
                            }
                            if (CurrentType.Length > 0)
                                W.WriteElementString("br", ""); // Markus 9.1.2023 - Bugfix
                            if (!this.HtmlIncludeCSS)
                            {
                                W.WriteStartElement("FONT");
                                W.WriteAttributeString("face", "Verdana");
                            }
                            W.WriteString("\r\n");
                            W.WriteStartElement("h4");
                            W.WriteString(R[1].ToString().ToUpper() + "S");
                            W.WriteEndElement();//h4
                            W.WriteString("\r\n");
                            if (!this.HtmlIncludeCSS)
                                W.WriteEndElement();//FONT
                            W.WriteStartElement("ul");
                            //if (CurrentType.Length > 0)
                            //{
                            //    W.WriteEndElement();//ul
                            //    W.WriteString("\r\n");
                            //    W.WriteStartElement("ul");
                            //    W.WriteString("\r\n");
                            //}
                            W.WriteString("\r\n");
                            CurrentType = R[1].ToString();
                        }
                        this.createDocuHtmlIndexEntry(W, R[0].ToString());
                        //W.WriteElementString("br", ""); // Markus 9.1.2023 - Bugfix
                        W.WriteString("\r\n");
                    }

                    // Bugfix - ul braucht man immer fuer liste
                    W.WriteEndElement();//ul
                    W.WriteString("\r\n");

                    //if (this.HtmlDocuContainsMoreThenOneType())
                    //{
                    //    W.WriteEndElement();//ul
                    //    W.WriteString("\r\n");
                    //}
                    W.WriteElementString("br", "");
                    W.WriteElementString("br", "");
                    W.WriteString("\r\n");
                    W.WriteString("\r\n");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuHtmlIndexEntry(System.Xml.XmlWriter W, string Table)
        {
            try
            {
                W.WriteStartElement("li");
                if (!this.HtmlIncludeCSS)
                {
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                }
                W.WriteStartElement("a");
                W.WriteAttributeString("href", "#" + Table);
                W.WriteString(Table);
                W.WriteEndElement();//a
                W.WriteEndElement();//li
                if (!this.HtmlIncludeCSS)
                    W.WriteEndElement();//FONT
                W.WriteString("\r\n");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuHtmlTable(System.Xml.XmlWriter W, string Object, string ObjectType, bool OnlyColumns = false)
        {
            if (Object.Length == 0)
                return;

            try
            {
                System.Data.DataTable dt = this.dtTableDocu(Object, this.checkBoxHtmlContext.Checked);

                string DisplayTextTable = Object;
                string DescriptionTable = "";

                string AccessibilityTable = "";
                string DeterminationTable = "";
                string VisibilityTable = "";

                string AccessibilityColumn = "";
                string DeterminationColumn = "";
                string VisibilityColumn = "";

                string UsageTableNotes = "";
                string PresetValue = "";
                string UsageNotes = "";
                bool TableHasUsageNotes = true;
                if (dt.Columns.Contains("Notes"))
                {
                    System.Data.DataRow[] rr = dt.Select("Notes <> ''");
                    if (rr.Length == 0)
                        TableHasUsageNotes = false;
                }
                else
                    TableHasUsageNotes = false;

                if (this.checkBoxHtmlContext.Checked)
                {
                    System.Collections.Generic.Dictionary<string, string> dict = DiversityWorkbench.Entity.EntityInformation(Object);
                    DescriptionTable = dict["Description"];

                    AccessibilityTable = dict["Accessibility"];
                    DeterminationTable = dict["Determination"];
                    VisibilityTable = dict["Visibility"];

                    UsageTableNotes = dict["UsageNotes"];
                    DisplayTextTable = dict["DisplayText"];
                }

                // if a table with a certain Accessibility should not be printed
                switch (AccessibilityTable.ToLower())
                {
                    case "inapplicable":
                        if (this.userControlColorSettings.HideInapplicable) return;
                        break;
                    case "read_only":
                        if (this.userControlColorSettings.HideReadOnly) return;
                        break;
                }

                string ColorText = this.userControlColorSettings.ColorCodeText(AccessibilityTable.ToString());
                if (!HtmlIncludeCSS)
                {
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                    if (AccessibilityTable == "inapplicable")
                        W.WriteAttributeString("style", "color: " + ColorText);
                }
                if (!OnlyColumns)
                {
                    W.WriteStartElement("h3");
                    W.WriteStartElement("a");
                    W.WriteAttributeString("name", DisplayTextTable);
                    W.WriteString(ObjectType.Substring(0, 1).ToUpper() + ObjectType.Substring(1).ToLower() + " ");
                    W.WriteStartElement("u");
                    W.WriteString(Object);
                    W.WriteEndElement();//u
                    W.WriteEndElement();//a
                    W.WriteEndElement();//h3
                    W.WriteString("\r\n");
                    if (this.checkBoxIncludeDescription.Checked)
                    {
                        if (!this.checkBoxHtmlContext.Checked)
                        {
                            DescriptionTable = DiversityWorkbench.Forms.FormFunctions.getDescription(Object, ObjectType, "", this.Conn, _Schema);
                            W.WriteString(DescriptionTable);
                            //DescriptionTable = this.FormFunctions.TableDescription(Object);
                        }
                        else
                        {
                            W.WriteString(this.FormFunctions.TableDescription(Object));
                        }
                        W.WriteElementString("br", "");
                    }

                    W.WriteElementString("br", "");
                    if (!HtmlIncludeCSS)
                        W.WriteEndElement();//FONT
                    W.WriteString("\r\n");

                    if (this.checkBoxIncludeUsageNotes.Checked && UsageTableNotes.Length > 0)
                    {
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                        W.WriteAttributeString("size", "2");
                        W.WriteString(UsageTableNotes);
                        W.WriteEndElement();//FONT
                        W.WriteElementString("br", "");
                        W.WriteElementString("br", "");
                    }
                }

                if (!this.checkBoxHtmlNoColumns.Checked || OnlyColumns)
                {
                    W.WriteStartElement("table");
                    W.WriteAttributeString("cellpadding", "3");
                    W.WriteAttributeString("cellspacing", "0");
                    W.WriteAttributeString("border", "1");
                    W.WriteAttributeString("width", "100%");

                    // the backcolor for the whole table
                    if (!HtmlIncludeCSS)
                    {
                        string ColorTable = this.userControlColorSettings.ColorCode(AccessibilityTable);
                        if (ColorTable.Length > 0)
                            W.WriteAttributeString("bgcolor", ColorTable);
                        if (AccessibilityTable == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                            W.WriteAttributeString("style", "color:" + this.userControlColorSettings.ColorCodeText(AccessibilityTable));
                    }
                    // printing the information about the table
                    W.WriteStartElement("tr");
                    foreach (System.Data.DataColumn CH in dt.Columns)
                    {
                        if (CH.ColumnName == "Notes" && (!this.IncludeUsageNotes || !TableHasUsageNotes))
                            continue;
                        if (CH.ColumnName == "ORDINAL_POSITION")
                            continue;
                        if (CH.ColumnName == "Relation" && (!this.IncludeRelations || ObjectType.ToUpper() == "VIEW" || ForEnums))
                            continue;
                        if (CH.ColumnName == "Nullable" && !this.IncludeNullable)
                            continue;
                        if (!this.checkBoxIncludeDescription.Checked && CH.ColumnName == "Description")
                            continue;
                        if (CH.ColumnName != "Collation" && CH.ColumnName != "Length" && CH.ColumnName != "DefaultValue")
                        {
                            W.WriteStartElement("th");
                            W.WriteAttributeString("nowrap", "nowrap");
                            W.WriteAttributeString("align", "left");
                            if (HtmlIncludeDWBcss)
                                W.WriteAttributeString("class", "columnheader");
                            if (!HtmlIncludeCSS)
                            {
                                string Color = this.userControlColorSettings.ColorCode(AccessibilityTable);
                                if (Color.Length > 0 && AccessibilityTable != "no_restrictions")
                                    W.WriteAttributeString("bgcolor", Color);
                                else if (AccessibilityTable == DiversityWorkbench.Entity.Accessibility.inapplicable.ToString())
                                    W.WriteAttributeString("style", "color:" + this.userControlColorSettings.ColorCodeText(AccessibilityTable));
                                else
                                    W.WriteAttributeString("bgcolor", "lightgrey");
                            }
                            if (!this.HtmlIncludeCSS)
                            {
                                W.WriteStartElement("FONT");
                                W.WriteAttributeString("face", "Verdana");
                            }
                            string Text = CH.ColumnName.ToString();
                            if (Text == "ColumnName") Text = "Column";
                            if (Text == "Datatype") Text = "Data type";
                            W.WriteString(Text);
                            if (!this.HtmlIncludeCSS)
                                W.WriteEndElement();//FONT
                            W.WriteEndElement();//th
                            W.WriteString("\r\n");
                        }
                    }
                    W.WriteEndElement();//tr

                    // printing the informations for the columns
                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Description = "";
                        if (this.checkBoxHtmlContext.Checked)
                        {
                            string Entity = Object + "." + R["ColumnName"].ToString();
                            System.Collections.Generic.Dictionary<string, string> dict = DiversityWorkbench.Entity.EntityInformation(Entity);
                            Description = dict["Description"];
                            AccessibilityColumn = dict["Accessibility"];
                            DeterminationColumn = dict["Determination"];
                            VisibilityColumn = dict["Visibility"];
                            PresetValue = dict["PresetValue"];
                            if (dict.ContainsKey("UsageNotes"))
                                UsageNotes = dict["UsageNotes"];
                        }
                        else
                        {
                            bool IsView = false;
                            if (ObjectType.ToLower() == "view")
                                IsView = true;
                            Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(Object, IsView, R["ColumnName"].ToString(), this.Conn, true, _Schema);
                        }

                        if (this.checkBoxHtmlExcludeColumnsObsolete.Checked && this.IsOutdated(Description))
                            continue;

                        if (this.DocuHtmlColumnIsExcluded(AccessibilityColumn, DeterminationColumn, ObjectType, R))
                            continue;

                        W.WriteStartElement("tr");
                        if (Description.Length == 0) Description = "-";
                        R["Description"] = Description;
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName == "Notes" && (!this.IncludeUsageNotes || !TableHasUsageNotes))
                                continue;
                            if (C.ColumnName == "ORDINAL_POSITION")
                                continue;
                            if (C.ColumnName == "Relation" && (!this.IncludeRelations || ObjectType.ToUpper() == "VIEW" || ForEnums))
                                continue;
                            if (C.ColumnName == "Nullable" && !this.IncludeNullable)
                                continue;
                            if (!this.checkBoxIncludeDescription.Checked && C.ColumnName == "Description")
                                continue;

                            // getting the backcolor of the fields
                            string ColorCode = "";
                            DiversityWorkbench.Entity.Accessibility Accessibility = DiversityWorkbench.Entity.Accessibility.no_restrictions;
                            DiversityWorkbench.Entity.Determination Determination = DiversityWorkbench.Entity.Determination.user_defined;
                            DiversityWorkbench.Entity.Visibility Visibility = DiversityWorkbench.Entity.Visibility.visible;

                            if (AccessibilityTable == "read_only")
                                AccessibilityColumn = "read_only";
                            else if (AccessibilityTable == "inapplicable")
                                AccessibilityColumn = "inapplicable";

                            switch (AccessibilityColumn)
                            {
                                case "inapplicable":
                                    Accessibility = DiversityWorkbench.Entity.Accessibility.inapplicable;
                                    break;
                                case "read_only":
                                    Accessibility = DiversityWorkbench.Entity.Accessibility.read_only;
                                    break;
                            }
                            switch (DeterminationColumn)
                            {
                                case "service_link":
                                    Determination = DiversityWorkbench.Entity.Determination.service_link;
                                    break;
                                case "calculated":
                                    Determination = DiversityWorkbench.Entity.Determination.calculated;
                                    break;
                            }
                            switch (VisibilityColumn)
                            {
                                case "optional":
                                    Visibility = DiversityWorkbench.Entity.Visibility.optional;
                                    break;
                                case "hidden":
                                    Visibility = DiversityWorkbench.Entity.Visibility.hidden;
                                    break;
                            }
                            ColorCode = this.userControlColorSettings.ColorCode(Accessibility, Determination, Visibility);
                            ColorText = this.userControlColorSettings.ColorCodeText(Accessibility.ToString());

                            bool IsPK = false;
                            string Default = "";
                            System.Data.DataTable dtPK = this.dtPrimaryKey(Object);
                            foreach (System.Data.DataRow PK in dtPK.Rows)
                            {
                                if (PK[0].ToString() == R[C.ColumnName].ToString())
                                {
                                    IsPK = true;
                                    break;
                                }
                            }
                            if (C.ColumnName != "DefaultValue")
                            {
                                if (PresetValue.Length > 0)
                                {
                                    Default = "Preset value: " + PresetValue;
                                }
                                else
                                {
                                    if (R["DefaultValue"].Equals(System.DBNull.Value)) Default = "";
                                    else
                                    {
                                        Default = R["DefaultValue"].ToString();
                                        if (Default.StartsWith("(")) Default = Default.Substring(1);
                                        if (Default.EndsWith(")")) Default = Default.Substring(0, Default.Length - 1);
                                        if (Default.Length > 0) Default = "Default value: " + Default;
                                    }
                                }
                            }
                            if (C.ColumnName != "Collation" && C.ColumnName != "Length" && C.ColumnName != "DefaultValue")
                            {
                                W.WriteStartElement("td");
                                if (!HtmlIncludeCSS)
                                {
                                    if (ColorCode.Length > 0)
                                    {
                                        W.WriteAttributeString("bgcolor", ColorCode);
                                    }
                                    if (Accessibility == DiversityWorkbench.Entity.Accessibility.inapplicable)
                                        W.WriteAttributeString("style", "color: " + ColorText);
                                }

                                string T = R[C.ColumnName].ToString();
                                if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                                {
                                    W.WriteAttributeString("nowrap", "nowrap");
                                    if (R["Length"].ToString() != "-1")
                                    {
                                        // Markus 18.4.23: no length for e.g. ntext (1073741823)
                                        if (R["Length"].ToString().Length < 6)
                                            T += " (" + R["Length"].ToString() + ")";
                                    }
                                    else if (T != "geography")
                                        T += " (MAX)";
                                }

                                if (IsPK) { W.WriteStartElement("b"); W.WriteStartElement("u"); }
                                if (!this.HtmlIncludeCSS)
                                {
                                    W.WriteStartElement("FONT");
                                    W.WriteAttributeString("face", "Verdana");
                                    W.WriteAttributeString("size", "2");
                                }
                                if (C.ColumnName != "Notes")
                                    W.WriteString(T);
                                if (Default.Length > 0 && C.ColumnName == "Description")
                                {
                                    W.WriteElementString("br", "");
                                    W.WriteStartElement("i");
                                    W.WriteString(Default);
                                    W.WriteEndElement();
                                }
                                if (C.ColumnName == "Notes" && this.IncludeUsageNotes)
                                {
                                    if (UsageNotes.Length > 0)
                                        W.WriteString(UsageNotes);
                                    else
                                        W.WriteString("-");
                                }
                                if (!this.HtmlIncludeCSS)
                                    W.WriteEndElement();//FONT
                                if (IsPK) { W.WriteEndElement(); W.WriteEndElement(); }//b
                                W.WriteEndElement();//td
                                W.WriteString("\r\n");
                            }
                        }
                        W.WriteEndElement();//tr
                        W.WriteString("\r\n");
                        i++;
                    }
                    W.WriteEndElement();//table
                }

                if (!OnlyColumns)
                {
                    this.createDocuHtmlDependencies(W, Object, ObjectType);

                    if (ObjectType.ToLower() == "table" && this.checkBoxIncludeTrigger.Checked)
                    {
                        this.createDocuHtmlTrigger(W, Object);
                        //W.WriteElementString("br", "");
                        //W.WriteElementString("br", "");
                    }
                    else if (ObjectType.ToLower() == "view" && this.checkBoxIncludeDefinition.Checked) //(this.checkBoxIncludeTrigger.Checked || this.checkBoxIncludeDefinition.Checked))
                    {
                        this.createDocuHtmlDefinition(W, Object);
                    }
                }
                W.WriteElementString("br", "");
                W.WriteElementString("br", "");
                W.WriteString("\r\n");

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private bool DocuHtmlColumnIsExcluded(string AccessibilityColumn, string DeterminationColumn, string ObjectType, System.Data.DataRow R)
        {
            // if a column with a certain usage should not be printed
            switch (AccessibilityColumn.ToLower())
            {
                case "inapplicable":
                    if (this.userControlColorSettings.HideInapplicable) return true;
                    break;
                case "read_only":
                    if (this.userControlColorSettings.HideReadOnly) return true;
                    break;
            }
            switch (DeterminationColumn.ToLower())
            {
                case "calculated":
                    if (this.userControlColorSettings.HideCalculated) return true;
                    break;
                case "service_link":
                    if (this.userControlColorSettings.HideServiceLink) return true;
                    break;
            }

            // if a column with a certain name pattern should not be printed
            if (this.checkBoxSuppressSpecialColumns.Checked && this.textBoxSuppressSpecialColumns.Text.Length > 0)
            {
                bool Suppress = false;
                string x = R["ColumnName"].ToString();
                string[] SuppressColumns = this.textBoxSuppressSpecialColumns.Text.Split((new Char[] { '|' }));
                for (int ii = 0; ii < SuppressColumns.Length; ii++)
                {
                    if (R["ColumnName"].ToString().StartsWith(SuppressColumns[ii]))
                        Suppress = true;
                }
                if (Suppress)
                    return true;
            }
            if (this.checkBoxSuppressSpecialColumnsEnding.Checked && this.textBoxSuppressSpecialColumnsEnding.Text.Length > 0)
            {
                bool Suppress = false;
                string x = R["ColumnName"].ToString();
                string[] SuppressEnding = this.textBoxSuppressSpecialColumnsEnding.Text.Split((new Char[] { '|' }));
                for (int ii = 0; ii < SuppressEnding.Length; ii++)
                {
                    if (R["ColumnName"].ToString().EndsWith(SuppressEnding[ii]))
                        Suppress = true;
                }
                if (Suppress)
                    return true;
            }
            if (this.checkBoxHtmlExcludeLoggingColumns.Checked && ObjectType.ToLower() != "view")
            {
                if (R["ColumnName"].ToString().ToLower() == "logupdatedby" ||
                    R["ColumnName"].ToString().ToLower() == "logupdatedwhen" ||
                    R["ColumnName"].ToString().ToLower() == "logcreatedby" ||
                    R["ColumnName"].ToString().ToLower() == "logcreatedwhen" ||
                    R["ColumnName"].ToString().ToLower() == "loginsertedby" ||
                    R["ColumnName"].ToString().ToLower() == "loginsertedwhen" ||
                    R["ColumnName"].ToString().ToLower() == "rowguid")
                    return true;
            }
            return false;
        }

        private void createDocuHtmlTrigger(System.Xml.XmlWriter W, string Table)
        {
            //string SQL = "select o.name from sysobjects o, sysobjects t " +
            //    "where o.type = 'tr' " +
            //    "and o.parent_obj = t.id " +
            //    "and t.name = '" + Table + "' " +
            //    "and o.name not in (SELECT name FROM sys.triggers where is_disabled = 1) " +
            //    "order by o.name";
            /// Markus 23.1.2023: Reducing number of connections
            System.Data.DataTable dtTrigger = this.dtTrigger(Table); // DiversityWorkbench.Forms.FormFunctions.DataTable(SQL); // new DataTable();
            //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTrigger);
            foreach (System.Data.DataRow R in dtTrigger.Rows)
            {
                string Trigger = R[0].ToString();
                if (this.checkBoxHtmlExcludeStandardTrigger.Checked
                    && (Trigger.ToLower().StartsWith("trgupd")
                    || (Trigger.ToLower().StartsWith("trgdel"))))
                    continue;

                if (!HtmlIncludeCSS)
                {
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                }

                W.WriteStartElement("h4");
                W.WriteString(Trigger);
                W.WriteEndElement();//h4
                W.WriteString("\r\n");
                if (this.checkBoxIncludeDescription.Checked)
                {
                    string DescriptionTrigger = DiversityWorkbench.Forms.FormFunctions.getDescription(Table, "TRIGGER", Trigger, this.Conn, _Schema);
                    W.WriteString(DescriptionTrigger);
                }
                if (this.checkBoxIncludeDefinition.Checked)
                {
                    this.createDocuHtmlDefinition(W, R[0].ToString());
                }
                if (!HtmlIncludeCSS)
                    W.WriteEndElement();//FONT
                W.WriteString("\r\n");

            }
        }

        private System.Data.DataTable dtTrigger(string Table)
        {
            string SQL = "select o.name from sysobjects o, sysobjects t " +
                "where o.type = 'tr' " +
                "and o.parent_obj = t.id " +
                "and t.name = '" + Table + "' " +
                "and o.name not in (SELECT name FROM sys.triggers where is_disabled = 1) " +
                "order by o.name";
            /// Markus 23.1.2023: Reducing number of connections
            System.Data.DataTable dtTrigger = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL); // new DataTable();
            return dtTrigger;
        }


        private void createDocuHtmlRoutine(System.Xml.XmlWriter W, string Routine, string RoutineType)
        {
            try
            {
                if (!HtmlIncludeCSS)
                {
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                }

                W.WriteStartElement("h3");
                W.WriteStartElement("a");
                W.WriteAttributeString("name", Routine);
                W.WriteString(RoutineType.Substring(0, 1).ToUpper() + RoutineType.Substring(1).ToLower() + " ");
                W.WriteStartElement("u");
                W.WriteString(Routine);
                W.WriteEndElement();//u
                W.WriteEndElement();//a
                W.WriteEndElement();//h3
                if (!HtmlIncludeCSS)
                    W.WriteEndElement();//FONT // Markus 9.1.2023 - Bugfix
                W.WriteString("\r\n");
                // Markus 9.1.2023 - Bugfix
                string RoutineDescription = DiversityWorkbench.Forms.FormFunctions.getDescription(Routine, RoutineType, "", this.Conn, this._Schema);
                if (RoutineDescription.Length > 0)
                    W.WriteString(RoutineDescription);
                string SQL = "SELECT DATA_TYPE + case when CHARACTER_MAXIMUM_LENGTH is null then '' else ' (' + cast(CHARACTER_MAXIMUM_LENGTH as varchar) + ')' end AS DataType " +
                    "FROM INFORMATION_SCHEMA.ROUTINES " +
                    "WHERE ROUTINE_NAME = '" + Routine + "' AND SPECIFIC_SCHEMA = '" + this._Schema + "' ";
                string DataType = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this.Conn);
                if (DataType.ToUpper() != "TABLE" && DataType.Length > 0)
                {
                    W.WriteElementString("br", "");
                    W.WriteElementString("br", "");

                    // Markus 9.1.2023 - Bugfix
                    W.WriteStartElement("p");
                    if (!HtmlIncludeCSS)
                    {
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                    }
                    W.WriteString("DataType: " + DataType);
                    if (!HtmlIncludeCSS)
                        W.WriteEndElement();//FONT 
                    W.WriteEndElement();//p

                    //W.WriteElementString("p", "DataType: " + DataType);
                }
                // PARAMETERS OF THE FUNCTION
                System.Data.DataTable dt = new DataTable();
                if (RoutineType.ToLower() == "function")
                    dt = DiversityWorkbench.Data.Routine.Parameters(Routine, DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION);// this.dtTableDocu(Routine, false);
                else
                    dt = DiversityWorkbench.Data.Routine.Parameters(Routine, DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.PROCEDURE);
                if (dt.Rows.Count > 0)
                {
                    W.WriteElementString("br", "");
                    W.WriteElementString("br", "");
                    W.WriteStartElement("table");
                    W.WriteAttributeString("cellpadding", "3");
                    W.WriteAttributeString("cellspacing", "0");
                    W.WriteAttributeString("border", "1");
                    W.WriteAttributeString("width", "100%");

                    W.WriteStartElement("tr");
                    foreach (System.Data.DataColumn CH in dt.Columns)
                    {
                        if (CH.ColumnName.ToUpper() == "ORDINAL_POSITION")
                            continue;
                        if (CH.ColumnName != "Collation" && CH.ColumnName != "Length")
                        {
                            W.WriteStartElement("th");
                            W.WriteAttributeString("nowrap", "nowrap");
                            W.WriteAttributeString("align", "left");
                            if (!HtmlIncludeCSS)
                                W.WriteAttributeString("bgcolor", "lightgrey");
                            if (!HtmlIncludeCSS)
                            {
                                W.WriteStartElement("FONT");
                                W.WriteAttributeString("face", "Verdana");
                            }
                            string Text = CH.ColumnName.ToString();
                            if (Text.ToUpper() == "NAME") Text = "Parameter";
                            if (Text == "Datatype") Text = "Type";
                            W.WriteString(Text);
                            if (!HtmlIncludeCSS)
                                W.WriteEndElement();//FONT
                            W.WriteEndElement();//th
                            W.WriteString("\r\n");
                        }
                    }
                    W.WriteEndElement();//tr

                    // printing the informations for the columns
                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Description = R["Description"].ToString();// DiversityWorkbench.Forms.FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION, Routine, R["ColumnName"].ToString());
                        W.WriteStartElement("tr");
                        if (Description.Length == 0)
                        {
                            Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescriptionOfBase(Routine, R[0].ToString().Substring(1));
                            if (Description.Length == 0)
                                Description = "-";
                        }
                        R["Description"] = Description;
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName.ToUpper() == "ORDINAL_POSITION")
                                continue;

                            if (C.ColumnName != "Collation" && C.ColumnName != "Length" && C.ColumnName != "DefaultValue")
                            {
                                W.WriteStartElement("td");

                                string T = R[C.ColumnName].ToString();
                                if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                                {
                                    W.WriteAttributeString("nowrap", "nowrap");
                                    if (R["Length"].ToString() != "-1")
                                        T += " (" + R["Length"].ToString() + ")";
                                    else if (T != "geography")
                                        T += " (MAX)";
                                }

                                if (!HtmlIncludeCSS)
                                {
                                    W.WriteStartElement("FONT");
                                    W.WriteAttributeString("face", "Verdana");
                                }
                                W.WriteAttributeString("size", "2");
                                if (C.ColumnName != "Notes")
                                    W.WriteString(T);
                                if (!HtmlIncludeCSS)
                                    W.WriteEndElement();//FONT
                                W.WriteEndElement();//td
                                W.WriteString("\r\n");
                            }
                        }
                        W.WriteEndElement();//tr
                        W.WriteString("\r\n");
                        i++;
                    }
                    W.WriteEndElement();//table
                }

                // COLUMNS OF THE FUNCTION
                dt = DiversityWorkbench.Data.Routine.Columns(Routine);// this.dtTableDocu(Routine, false);
                if (dt.Rows.Count > 0)
                {
                    W.WriteElementString("br", "");
                    W.WriteElementString("br", "");
                    W.WriteStartElement("table");
                    W.WriteAttributeString("cellpadding", "3");
                    W.WriteAttributeString("cellspacing", "0");
                    W.WriteAttributeString("border", "1");
                    W.WriteAttributeString("width", "100%");

                    W.WriteStartElement("tr");
                    foreach (System.Data.DataColumn CH in dt.Columns)
                    {
                        if (CH.ColumnName.ToUpper() == "ORDINAL_POSITION")
                            continue;
                        W.WriteStartElement("th");
                        W.WriteAttributeString("nowrap", "nowrap");
                        W.WriteAttributeString("align", "left");
                        if (!HtmlIncludeCSS)
                            W.WriteAttributeString("bgcolor", "lightgrey");
                        if (!HtmlIncludeCSS)
                        {
                            W.WriteStartElement("FONT");
                            W.WriteAttributeString("face", "Verdana");
                        }
                        string Text = CH.ColumnName.ToString();
                        if (Text == "Name") Text = "Column";
                        if (Text == "Datatype") Text = "Type";
                        W.WriteString(Text);
                        if (!HtmlIncludeCSS)
                            W.WriteEndElement();//FONT
                        W.WriteEndElement();//th
                        W.WriteString("\r\n");
                    }
                    W.WriteEndElement();//tr

                    // printing the informations for the columns
                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Description = R["Description"].ToString();// DiversityWorkbench.Forms.FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION, Routine, R["ColumnName"].ToString());
                        W.WriteStartElement("tr");
                        if (Description.Length == 0)
                        {
                            Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescriptionOfBase(Routine, R[0].ToString(), this.Conn, 1);
                            if (Description.Length == 0)
                                Description = "-";
                        }
                        R["Description"] = Description;
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName.ToUpper() == "ORDINAL_POSITION")
                                continue;

                            W.WriteStartElement("td");

                            string T = R[C.ColumnName].ToString();
                            if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                            {
                                W.WriteAttributeString("nowrap", "nowrap");
                                if (R["Length"].ToString() != "-1")
                                    T += " (" + R["Length"].ToString() + ")";
                                else if (T != "geography")
                                    T += " (MAX)";
                            }

                            if (!HtmlIncludeCSS)
                            {
                                W.WriteStartElement("FONT");
                                W.WriteAttributeString("face", "Verdana");
                            }
                            W.WriteAttributeString("size", "2");
                            if (C.ColumnName != "Notes")
                                W.WriteString(T);
                            if (!HtmlIncludeCSS)
                                W.WriteEndElement();//FONT
                            W.WriteEndElement();//td
                            W.WriteString("\r\n");
                        }
                        W.WriteEndElement();//tr
                        W.WriteString("\r\n");
                        i++;
                    }
                    W.WriteEndElement();//table
                }

                this.createDocuHtmlDependencies(W, Routine, RoutineType);

                W.WriteElementString("br", "");

                if (this.checkBoxIncludeDefinition.Checked)
                {
                    this.createDocuHtmlDefinition(W, Routine);
                }
                //W.WriteEndElement();//FONT // Markus 9.1.2023 - Bugfix

                W.WriteElementString("br", "");
                //W.WriteEndElement();//FONT // Markus 9.1.2023 - Bugfix
                W.WriteString("\r\n");

                W.WriteElementString("br", "");
                W.WriteElementString("br", "");
                W.WriteString("\r\n");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void createDocuHtmlDefinition(System.Xml.XmlWriter W, string Object)
        {
            string SQL = "EXEC sp_helptext '" + Object + "';";
            /// Markus 23.1.2023: Reducing number of connections
            System.Data.DataTable dtDef = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);// new DataTable();
            //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtDef);
            W.WriteStartElement("FONT");
            W.WriteAttributeString("face", "Verdana");
            W.WriteAttributeString("size", "2");
            bool IsComment = false;
            foreach (System.Data.DataRow Rdef in dtDef.Rows)
            {
                string Text = Rdef[0].ToString().Trim();

                // Avoiding comment
                if (Text.StartsWith("/*"))
                {
                    IsComment = true;
                    if (Text.EndsWith("*/"))
                        IsComment = false;
                    continue;
                }
                if (IsComment && Text.EndsWith("*/"))
                {
                    IsComment = false;
                    continue;
                }
                if (IsComment)
                    continue;
                if (Text.StartsWith("--"))
                    continue;

                // Avoiding CREATE
                if (Text.StartsWith("CREATE "))
                {
                    W.WriteStartElement("b");
                    W.WriteString("Definition:");
                    W.WriteElementString("br", "");
                    W.WriteEndElement();//b
                    if (Text.IndexOf("/*") > -1 && Text.IndexOf("*/") > -1)
                    {
                        string t1 = Text.Substring(0, Text.IndexOf("/*"));
                        string t2 = Text.Substring(Text.IndexOf("*/") + 2);
                        Text = t1 + " " + t2;
                        Text = Text.Replace("  ", " ");
                    }
                    if (Text.StartsWith("()"))
                        Text = Text.Substring(2).Trim();
                    if (Text == "()")
                        continue;
                    else if (Text.IndexOf(" ()") > -1 && !Text.EndsWith("()"))
                    {
                        Text = Text.Substring(Text.IndexOf(" ()") + 3).Trim();
                        if (Text.Length == 0)
                            continue;
                        W.WriteString(Text);
                        W.WriteElementString("br", "");
                    }
                    else if (Text.IndexOf("()") > -1 && Text.EndsWith("()") && Text.IndexOf("()") == Text.LastIndexOf("()"))
                    {
                        continue;
                    }
                    else if (Text.IndexOf(" RETURNS ") > -1)
                    {
                        W.WriteString(Text.Substring(Text.IndexOf(" RETURNS ")));
                        W.WriteElementString("br", "");
                    }
                    else if (Text.IndexOf(" AS ") > -1)
                    {
                        W.WriteString(Text.Substring(Text.IndexOf(" AS ")));
                        W.WriteElementString("br", "");
                    }
                    else if (Text.IndexOf(" (") > -1)
                    {
                        W.WriteString(Text.Substring(Text.IndexOf(" (")));
                        W.WriteElementString("br", "");
                    }
                    continue;
                }
                if (Text.Length == 0)
                    continue;
                W.WriteString(Text);
                W.WriteElementString("br", "");
            }
        }


        private void createDocuHtmlRole(System.Xml.XmlWriter W, string Role)
        {
            try
            {
                W.WriteStartElement("FONT");
                W.WriteAttributeString("face", "Verdana");

                W.WriteStartElement("h3");
                W.WriteStartElement("a");
                W.WriteAttributeString("name", Role);
                W.WriteStartElement("u");
                W.WriteString(Role);
                W.WriteEndElement();//u
                W.WriteEndElement();//a
                W.WriteEndElement();//h3
                W.WriteString("\r\n");
                W.WriteString(FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.USER, Role, ""));
                W.WriteElementString("br", "");
                W.WriteElementString("br", "");

                this.createDocuHtmlPermissions(W, Role);
                this.createDocuHtmlIncludedRoles(W, Role);

                W.WriteElementString("br", "");
                W.WriteElementString("br", "");
                W.WriteEndElement();//FONT
                W.WriteString("\r\n");

                W.WriteElementString("br", "");
                W.WriteElementString("br", "");
                W.WriteString("\r\n");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void createDocuHtmlPermissions(System.Xml.XmlWriter W, string Role)
        {
            try
            {
                // Objects
                System.Collections.Generic.List<string> Objects = new List<string>();
                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    if (R["ObjectType"].ToString().ToLower() != "role")
                        Objects.Add(R[0].ToString());
                }
                W.WriteStartElement("table");
                W.WriteAttributeString("cellpadding", "3");
                W.WriteAttributeString("cellspacing", "0");
                W.WriteAttributeString("border", "1");
                W.WriteAttributeString("width", "100%");

                // Permissions
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Data.Role.Permission, string>> PerDict = DiversityWorkbench.Data.Role.AllPermissions(Role);

                // Table header
                System.Collections.Generic.List<Data.Role.Permission> PermissionColumns = new List<Data.Role.Permission>();
                //Columns.Add("Permissions");
                PermissionColumns.Add(Data.Role.Permission.SELECT);
                PermissionColumns.Add(Data.Role.Permission.INSERT);
                PermissionColumns.Add(Data.Role.Permission.UPDATE);
                PermissionColumns.Add(Data.Role.Permission.DELETE);
                PermissionColumns.Add(Data.Role.Permission.EXECUTE);
                //Columns.Add("Type");
                W.WriteStartElement("tr");
                // Column for Securables
                W.WriteStartElement("th");
                W.WriteAttributeString("nowrap", "nowrap");
                W.WriteAttributeString("align", "left");
                if (!HtmlIncludeCSS)
                    W.WriteAttributeString("bgcolor", "lightgrey");
                W.WriteStartElement("FONT");
                W.WriteAttributeString("face", "Verdana");
                W.WriteString("Permissions");
                W.WriteEndElement();//FONT
                W.WriteEndElement();//th
                foreach (Data.Role.Permission C in PermissionColumns)
                {
                    W.WriteStartElement("th");
                    W.WriteAttributeString("nowrap", "nowrap");
                    W.WriteAttributeString("align", "left");
                    if (!HtmlIncludeCSS)
                        W.WriteAttributeString("bgcolor", "lightgrey");
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                    W.WriteString(C.ToString());
                    W.WriteEndElement();//FONT
                    W.WriteEndElement();//th
                    W.WriteString("\r\n");
                }
                // Column for Type
                W.WriteStartElement("th");
                W.WriteAttributeString("nowrap", "nowrap");
                W.WriteAttributeString("align", "left");
                if (!HtmlIncludeCSS)
                    W.WriteAttributeString("bgcolor", "lightgrey");
                W.WriteStartElement("FONT");
                W.WriteAttributeString("face", "Verdana");
                W.WriteString("Type");
                W.WriteEndElement();//FONT
                W.WriteEndElement();//th
                W.WriteEndElement();//tr

                // printing the informations for the columns
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<Data.Role.Permission, string>> KVobj in PerDict)
                {
                    if (this.checkBoxExcludeSystemObjects.Checked)
                    {
                        // exclude the system objects
                        if (this.DatabaseSystemObjects.ContainsKey(KVobj.Key))
                            continue;

                        if (!KVobj.Key.EndsWith("_Enum")) // enums are included in any case with this option
                        {
                            if (KVobj.Key.EndsWith("_log"))
                            {
                                // Include log tables for selected objects
                                string DataTable = KVobj.Key.Remove(KVobj.Key.Length - 4);
                                if (!Objects.Contains(DataTable))
                                    continue;
                            }
                            else if (!Objects.Contains(KVobj.Key))
                                continue;
                        }
                    }
                    else
                    {
                        // if no objects are selected, show all otherwise restrict to selected
                        if (Objects.Count > 0 && !Objects.Contains(KVobj.Key))
                            continue;
                    }

                    W.WriteStartElement("tr");

                    // Column for ObjectName
                    W.WriteStartElement("td");
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                    W.WriteAttributeString("size", "2");
                    W.WriteString(KVobj.Key);
                    W.WriteEndElement();//FONT
                    W.WriteEndElement();//td

                    // Columns for Permissions
                    foreach (Data.Role.Permission C in PermissionColumns)
                    {
                        W.WriteStartElement("td");
                        W.WriteAttributeString("align", "center");
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                        if (KVobj.Value[C].Length == 0)
                        {
                            W.WriteRaw("&nbsp;");
                        }
                        else if (KVobj.Value[C] == Role)
                        {
                            W.WriteAttributeString("size", "4");
                            W.WriteRaw("&bull;");
                        }
                        else
                        {
                            if (!HtmlIncludeCSS)
                                W.WriteAttributeString("color", "grey");
                            W.WriteAttributeString("size", "1");
                            string InheritedFrom = KVobj.Value[C];
                            string PrintedRole = InheritedFrom.Substring(0, 1);
                            for (int ii = 1; ii < InheritedFrom.Length; ii++)
                            {
                                if (InheritedFrom[ii].ToString() == InheritedFrom[ii].ToString().ToUpper())
                                {
                                    PrintedRole += "\r\n";
                                }
                                PrintedRole += InheritedFrom[ii];
                            }
                            W.WriteString(PrintedRole);
                            //W.WriteRaw("cir;");
                        }
                        W.WriteEndElement();//FONT
                        W.WriteEndElement();//td
                        W.WriteString("\r\n");
                    }

                    // Column for Type
                    W.WriteStartElement("td");
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                    W.WriteAttributeString("size", "2");
                    string Type = DiversityWorkbench.Data.Role.Securables()[KVobj.Key];
                    if (Type == "BASE TABLE")
                        Type = "TABLE";
                    W.WriteString(Type);
                    W.WriteEndElement();//FONT
                    W.WriteEndElement();//td

                    W.WriteEndElement();//tr
                    W.WriteString("\r\n");
                    i++;
                }
                W.WriteEndElement();//table
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private void createDocuHtmlPermissionsAll(System.Xml.XmlWriter W, string Role)
        //{
        //    try
        //    {
        //        W.WriteStartElement("table");
        //        W.WriteAttributeString("cellpadding", "3");
        //        W.WriteAttributeString("cellspacing", "0");
        //        W.WriteAttributeString("border", "1");
        //        W.WriteAttributeString("width", "100%");

        //        // Permissions
        //        System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Data.Role.Permission, string>> PerDict = DiversityWorkbench.Data.Role.AllPermissions(Role);

        //        // Table header
        //        System.Collections.Generic.List<Data.Role.Permission> PermissionColumns = new List<Data.Role.Permission>();
        //        //Columns.Add("Permissions");
        //        PermissionColumns.Add(Data.Role.Permission.SELECT);
        //        PermissionColumns.Add(Data.Role.Permission.INSERT);
        //        PermissionColumns.Add(Data.Role.Permission.UPDATE);
        //        PermissionColumns.Add(Data.Role.Permission.DELETE);
        //        PermissionColumns.Add(Data.Role.Permission.EXECUTE);
        //        //Columns.Add("Type");
        //        W.WriteStartElement("tr");
        //        // Column for Securables
        //        W.WriteStartElement("th");
        //        W.WriteAttributeString("nowrap", "nowrap");
        //        W.WriteAttributeString("align", "left");
        //        W.WriteAttributeString("bgcolor", "lightgrey");
        //        W.WriteStartElement("FONT");
        //        W.WriteAttributeString("face", "Verdana");
        //        W.WriteString("Permissions");
        //        W.WriteEndElement();//FONT
        //        W.WriteEndElement();//th
        //        foreach (Data.Role.Permission C in PermissionColumns)
        //        {
        //            W.WriteStartElement("th");
        //            W.WriteAttributeString("nowrap", "nowrap");
        //            W.WriteAttributeString("align", "left");
        //            W.WriteAttributeString("bgcolor", "lightgrey");
        //            W.WriteStartElement("FONT");
        //            W.WriteAttributeString("face", "Verdana");
        //            W.WriteString(C.ToString());
        //            W.WriteEndElement();//FONT
        //            W.WriteEndElement();//th
        //            W.WriteString("\r\n");
        //        }
        //        // Column for Type
        //        W.WriteStartElement("th");
        //        W.WriteAttributeString("nowrap", "nowrap");
        //        W.WriteAttributeString("align", "left");
        //        W.WriteAttributeString("bgcolor", "lightgrey");
        //        W.WriteStartElement("FONT");
        //        W.WriteAttributeString("face", "Verdana");
        //        W.WriteString("Type");
        //        W.WriteEndElement();//FONT
        //        W.WriteEndElement();//th
        //        W.WriteEndElement();//tr

        //        // printing the informations for the columns
        //        int i = 0;
        //        foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<Data.Role.Permission, string>> KVobj in PerDict)
        //        {
        //            W.WriteStartElement("tr");

        //            // Column for ObjectName
        //            W.WriteStartElement("td");
        //            W.WriteStartElement("FONT");
        //            W.WriteAttributeString("face", "Verdana");
        //            W.WriteAttributeString("size", "2");
        //            W.WriteString(KVobj.Key);
        //            W.WriteEndElement();//td

        //            // Columns for Permissions
        //            foreach (Data.Role.Permission C in PermissionColumns)
        //            {
        //                W.WriteStartElement("td");
        //                    W.WriteAttributeString("align", "center");
        //                W.WriteStartElement("FONT");
        //                W.WriteAttributeString("face", "Verdana");
        //                if (KVobj.Value[C].Length == 0)
        //                {
        //                    W.WriteRaw("&nbsp;");
        //                }
        //                else if (KVobj.Value[C] == Role)
        //                {
        //                    W.WriteAttributeString("size", "4");
        //                    W.WriteRaw("&bull;");
        //                }
        //                else
        //                {
        //                    W.WriteAttributeString("color", "grey");
        //                    W.WriteAttributeString("size", "1");
        //                    string InheritedFrom = KVobj.Value[C];
        //                    string PrintedRole = InheritedFrom.Substring(0,1);
        //                    for (int ii = 1; ii < InheritedFrom.Length; ii++ )
        //                    {
        //                        if (InheritedFrom[ii].ToString() == InheritedFrom[ii].ToString().ToUpper())
        //                        {
        //                            PrintedRole += "\r\n";
        //                        }
        //                        PrintedRole += InheritedFrom[ii];
        //                    }
        //                    W.WriteString(PrintedRole);
        //                    //W.WriteRaw("cir;");
        //                }
        //                W.WriteEndElement();//FONT
        //                W.WriteEndElement();//td
        //                W.WriteString("\r\n");
        //            }

        //            // Column for Type
        //            W.WriteStartElement("td");
        //            W.WriteStartElement("FONT");
        //            W.WriteAttributeString("face", "Verdana");
        //            W.WriteAttributeString("size", "2");
        //            string Type = DiversityWorkbench.Data.Role.Securables()[KVobj.Key];
        //            if (Type == "BASE TABLE")
        //                Type = "TABLE";
        //            W.WriteString(Type);
        //            W.WriteEndElement();//td

        //            W.WriteEndElement();//tr
        //            W.WriteString("\r\n");
        //            i++;
        //        }
        //        W.WriteEndElement();//table
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private void createDocuHtmlDependencies(System.Xml.XmlWriter W, string Object, string ObjectType)
        {
            try
            {
                if (this.checkBoxIncludeLookupRelations.Checked)
                {
                    string SQL = "SELECT DISTINCT referenced_entity_name " +
                        "FROM sys.sql_expression_dependencies " +
                        "WHERE referencing_id = OBJECT_ID(N'dbo." + Object + "') and not referenced_id is null;";
                    if (ObjectType.ToLower() == "table")
                    {
                        SQL = "SELECT DISTINCT P.TABLE_NAME " +
                            "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R  " +
                            "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS F ON R.CONSTRAINT_NAME = F.CONSTRAINT_NAME  " +
                            "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME  " +
                            "RIGHT OUTER JOIN INFORMATION_SCHEMA.COLUMNS AS C ON F.COLUMN_NAME = C.COLUMN_NAME AND F.TABLE_NAME = C.TABLE_NAME AND F.TABLE_SCHEMA = C.TABLE_SCHEMA AND F.TABLE_CATALOG = C.TABLE_CATALOG    " +
                            "WHERE C.TABLE_NAME = '" + Object + "' and not P.TABLE_NAME is null and P.TABLE_NAME <> '" + Object + "' " +
                            "ORDER BY P.TABLE_NAME";
                    }
                    if (ObjectType == "USER")
                    {

                    }
                    /// Markus 23.1.2023: Reducing number of connections
                    System.Data.DataTable dtDepend = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL); // new DataTable();
                    //string Message = "";
                    //DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtDepend, ref Message);
                    if (dtDepend.Rows.Count > 0)
                    {
                        W.WriteString("\r\n");
                        //W.WriteStartElement("p");
                        if (!HtmlIncludeCSS)
                        {
                            W.WriteStartElement("FONT");
                            W.WriteAttributeString("face", "Verdana");
                        }
                        W.WriteStartElement("h4");
                        W.WriteString("Depending on:");
                        W.WriteEndElement();//h6
                        W.WriteString("\r\n");
                        W.WriteStartElement("ul");
                        W.WriteString("\r\n");
                        foreach (System.Data.DataRow rr in dtDepend.Rows)
                        {
                            W.WriteStartElement("li");
                            if (!HtmlIncludeCSS)
                            {
                                W.WriteStartElement("FONT");
                                W.WriteAttributeString("face", "Verdana");
                            }
                            W.WriteString(rr[0].ToString());
                            W.WriteEndElement();//li
                            if (!HtmlIncludeCSS)
                                W.WriteEndElement();//FONT
                            W.WriteString("\r\n");
                        }
                        if (!HtmlIncludeCSS)
                            W.WriteEndElement();//FONT
                        W.WriteEndElement();//ul
                        //W.WriteEndElement();//p
                    }
                }

                if (this.checkBoxHtmlIncludeDepending.Checked && ObjectType.ToLower() == "table")
                {
                    string SQL = "select distinct B.TABLE_NAME from INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS A " +
                        "inner join INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE D on A.UNIQUE_CONSTRAINT_NAME = D.CONSTRAINT_NAME " +
                        "inner join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE B on A.CONSTRAINT_NAME = B.CONSTRAINT_NAME " +
                        "where D.TABLE_NAME = '" + Object + "' and B.TABLE_NAME <> '" + Object + "'; ";
                    System.Data.DataTable dtDepend = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                    if (dtDepend.Rows.Count > 0)
                    {
                        W.WriteStartElement("p");
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                        W.WriteString("Dependent tables:");
                        W.WriteStartElement("ul");
                        foreach (System.Data.DataRow rr in dtDepend.Rows)
                        {
                            W.WriteStartElement("li");
                            W.WriteStartElement("FONT");
                            W.WriteAttributeString("face", "Verdana");
                            W.WriteString(rr[0].ToString());
                            W.WriteEndElement();//li
                            W.WriteEndElement();//FONT
                        }
                        W.WriteEndElement();//FONT
                        W.WriteEndElement();//ul
                        W.WriteEndElement();//p
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void createDocuHtmlIncludedRoles(System.Xml.XmlWriter W, string Object)
        {
            if (this.checkBoxIncludeLookupRelations.Checked)
            {
                System.Collections.Generic.List<string> Roles = DiversityWorkbench.Data.Role.InheritingFromRoles(Object);
                if (Roles.Count > 0)
                {
                    W.WriteStartElement("p");
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                    W.WriteString("Inheriting from roles:");
                    W.WriteStartElement("ul");
                    foreach (string R in Roles)
                    {
                        W.WriteStartElement("li");
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                        W.WriteString(R);
                        W.WriteEndElement();//li
                        W.WriteEndElement();//FONT
                    }
                    W.WriteEndElement();//FONT
                    W.WriteEndElement();//ul
                    W.WriteEndElement();//p
                }
            }
        }

        #endregion

        private void checkBoxIndex_CheckedChanged(object sender, EventArgs e)
        {

        }

        #region Properties for creation



        #endregion

        private void checkBoxIncludeLookupRelations_CheckedChanged(object sender, EventArgs e)
        {
            this._CurrentTable = "";
        }

        private void checkBoxIncludeNullable_CheckedChanged(object sender, EventArgs e)
        {
            this._CurrentTable = "";
        }

        #region Context and Language

        private void setContext()
        {
            try
            {
                if (DiversityWorkbench.Entity.EntityTablesExistInDatabase && DiversityWorkbench.Settings.UseEntity)
                {
                    if (this._ConnectionString.Length > 0)
                    {
                        string SQL = "SELECT NULL AS Code, NULL AS DisplayText UNION " +
                            "SELECT Code, DisplayText FROM EntityContext_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText";
                        using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_ConnectionString))
                        {
                            con.Open();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
                            System.Data.DataTable dtContext = new DataTable();
                            try
                            {
                                ad.Fill(dtContext);
                                this.comboBoxContext.DataSource = dtContext;
                                this.comboBoxContext.DisplayMember = "DisplayText";
                                this.comboBoxContext.ValueMember = "Code";
                                //this.panelDocuHtml.Height = 146;
                                this.setHtmlContextVisibility(true);
                                this.groupBoxHtmlCss.Dock = DockStyle.Fill;
                                //this.tableLayoutPanelHtmlContext.Visible = true;
                            }
                            catch
                            {
                                this.setHtmlContextVisibility(false);
                                this.groupBoxHtmlCss.Dock = DockStyle.Top;
                                this.groupBoxHtmlCss.Height = 96;
                                //this.tableLayoutPanelHtmlContext.Visible = false;
                                //this.panelDocuHtml.Height = 66;
                                //this._useContext = false;
                            }
                            con.Close();
                        }
                    }
                }
                else
                {
                    this.setHtmlContextVisibility(false);
                    this.groupBoxHtmlCss.Dock = DockStyle.Top;
                    this.groupBoxHtmlCss.Height = 96;
                    //this.tableLayoutPanelHtmlContext.Visible = false;
                    //this.panelDocuHtml.Height = 66;
                    //this._useContext = false;
                }

            }
            catch { }
        }

        private void setLanguage()
        {
            System.Data.DataTable dtLanguage = new DataTable();
            try
            {
                if (dtLanguage.Rows.Count == 0 && DiversityWorkbench.Entity.EntityTablesExist)
                {
                    string SQL = "SELECT NULL AS Code, NULL AS DisplayText UNION SELECT Code, DisplayText FROM EntityLanguageCode_Enum ORDER BY DisplayText";
                    // Markus 23.1.2023: Reducing number of connections
                    try
                    {
                        using (Microsoft.Data.SqlClient.SqlConnection sqlConnection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                        {
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, sqlConnection);
                            ad.Fill(dtLanguage);
                        }
                    }
                    catch { }
                }
                if (dtLanguage.Rows.Count > 0)
                {
                    this.comboBoxHtmlLanguage.DataSource = dtLanguage;
                    try
                    {
                        this.comboBoxHtmlLanguage.DisplayMember = "DisplayText";
                        this.comboBoxHtmlLanguage.ValueMember = "Code";
                    }
                    catch { }
                    for (int i = 0; i < dtLanguage.Rows.Count; i++)
                    {
                        if (dtLanguage.Rows[i][0].ToString() == DiversityWorkbench.Settings.Language)
                        {
                            this.comboBoxHtmlLanguage.SelectedIndex = i;
                            break;
                        }
                    }
                }

            }
            catch { }
            this._Language = DiversityWorkbench.Settings.Language;

        }

        private void comboBoxHtmlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.comboBoxHtmlLanguage.SelectedValue.Equals(System.DBNull.Value) && this.comboBoxHtmlLanguage.SelectedValue.ToString().Length > 0)
            {
                this._Language = this.comboBoxHtmlLanguage.SelectedValue.ToString();
                //DiversityWorkbench.Settings.Language = this.comboBoxHtmlLanguage.SelectedValue.ToString();
            }
        }

        private void comboBoxContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxContext.SelectedValue.Equals(System.DBNull.Value)
                || this.comboBoxContext.SelectedValue.ToString().Trim().Length == 0)
            { }
            //this._useContext = false;
            else
            {
                //this._useContext = true;
                DiversityWorkbench.Settings.Context = this.comboBoxContext.SelectedValue.ToString();
            }
        }

        private string Context { get { return this.comboBoxContext.SelectedValue.ToString(); } }

        private bool IncludeUsageNotes { get { if (this.checkBoxIncludeUsageNotes.Checked) return true; else return false; } }
        private bool IncludeNullable { get { if (this.checkBoxIncludeNullable.Checked) return true; else return false; } }
        private bool IncludeRelations { get { if (this.checkBoxIncludeLookupRelations.Checked) return true; else return false; } }
        private bool IncludeVisibility { get { if (this.checkBoxIncludeUsageVisibility.Checked) return true; else return false; } }

        #endregion

        private System.Collections.Generic.Dictionary<string, string> _DatabaseSystemObjects;
        private System.Collections.Generic.Dictionary<string, string> DatabaseSystemObjects
        {
            get
            {
                if (this._DatabaseSystemObjects == null)
                {
                    this._DatabaseSystemObjects = new Dictionary<string, string>();
                    this._DatabaseSystemObjects.Add("dtproperties", "TABLE");
                    this._DatabaseSystemObjects.Add("sysdiagrams", "TABLE");
                    this._DatabaseSystemObjects.Add("fn_diagramobjects", "FUNCTION");
                    this._DatabaseSystemObjects.Add("dt_addtosourcecontrol", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_addtosourcecontrol_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_adduserobject", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_adduserobject_vcs", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_checkinobject", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_checkinobject_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_checkoutobject", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_checkoutobject_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_displayoaerror", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_displayoaerror_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_droppropertiesbyid", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_dropuserobjectbyid", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_generateansiname", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_getobjwithprop", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_getobjwithprop_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_getpropertiesbyid", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_getpropertiesbyid_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_getpropertiesbyid_vcs", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_getpropertiesbyid_vcs_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_isundersourcecontrol", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_isundersourcecontrol_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_removefromsourcecontrol", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_setpropertybyid", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_setpropertybyid_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_validateloginparams", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_validateloginparams_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_vcsenabled", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_verstamp006", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_verstamp007", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_whocheckedout", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("dt_whocheckedout_u", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("sp_alterdiagram", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("sp_creatediagram", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("sp_dropdiagram", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("sp_helpdiagramdefinition", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("sp_helpdiagrams", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("sp_renamediagram", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("sp_upgraddiagrams", "PROCEDURE");
                    this._DatabaseSystemObjects.Add("wbCurrentUserID", "FUNCTION");
                    this._DatabaseSystemObjects.Add("wb_functions", "ROLE");
                }
                return this._DatabaseSystemObjects;
            }
        }

        private void buttonOpenHtmlFolder_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.buttonOpenHtmlFolder.Tag != null && this.buttonOpenHtmlFolder.Tag.ToString().Length > 0)
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.buttonOpenHtmlFolder.Tag.ToString());
                    if (fileInfo.Exists)
                    {
                        string args = string.Format("/e, /select, \"{0}\"", fileInfo.FullName);

                        System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo();
                        info.FileName = "explorer";
                        info.Arguments = args;
                        System.Diagnostics.Process.Start(info);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #region Darkmode

        private void setDarkmode()
        {
            bool DarkMode = this.checkBoxHtmlDarkmode.Checked;
            if (DarkMode)
            {
                this.userControlWebViewHtml.AllowScripting = true;
            }
            else
            {

            }
        }
        private void checkBoxHtmlDarkmode_CheckedChanged(object sender, EventArgs e)
        {
            this.setDarkmode();
        }

        #endregion

        #region Home and favicon

        /// <summary>
        /// Include home button
        /// </summary>
        /// <param name="W"></param>
        //private void createDocuHtmlHome(System.Xml.XmlWriter W)
        //{
        //    //<a href="index.html"><img src="img/IcoHome.png" align="right"></a>
        //    try
        //    {
        //        if (this.checkBoxHtmlIncludeHomeButton.Checked)
        //        {
        //            string html = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + "index.html";
        //            System.IO.FileInfo fileHtml = new System.IO.FileInfo(html);
        //            string png = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + "img/IcoHome.png";
        //            System.IO.FileInfo filePng = new System.IO.FileInfo(png);
        //            if (/*fileHtml.Exists &&*/ filePng.Exists)
        //            {
        //                W.WriteStartElement("a");
        //                W.WriteAttributeString("href", "index.html");
        //                W.WriteStartElement("img");
        //                W.WriteAttributeString("src", "img/IcoHome.png");
        //                W.WriteAttributeString("align", "right");
        //                W.WriteEndElement(); // img
        //                W.WriteEndElement(); // a
        //                W.WriteString("\r\n");

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        /// <summary>
        /// Include favicon in html header
        /// </summary>
        /// <param name="W"></param>
        private void createDocuHtmlFavicon(System.Xml.XmlWriter W)
        {

            //<link rel="icon" type="image/svg+xml" href="favicon.svg" sizes="any">
            //<link rel="icon" href="/favicon.png" type="image/png">

            try
            {
                if (this.checkBoxHtmlIncludeHomeButton.Checked)
                {
                    string SVG = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + "favicon.svg";
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(SVG);
                    //if (fileInfo.Exists) // not available in target directory but final directory
                    {
                        W.WriteStartElement("link");
                        W.WriteAttributeString("rel", "icon");
                        W.WriteAttributeString("type", "image/svg+xml");
                        W.WriteAttributeString("href", "favicon.svg");
                        W.WriteAttributeString("sizes", "any");
                        W.WriteEndElement();
                        W.WriteString("\r\n");
                    }
                    string PNG = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + "favicon.png";
                    fileInfo = new System.IO.FileInfo(PNG);
                    //if (fileInfo.Exists) // not available in target directory but final directory
                    {
                        W.WriteStartElement("link");
                        W.WriteAttributeString("rel", "icon");
                        W.WriteAttributeString("type", "image/png");
                        W.WriteAttributeString("href", "favicon.png");
                        W.WriteEndElement();
                        W.WriteString("\r\n");
                    }

                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        /*
         * <meta name="database" content="DiversityAgents_Base">
<meta name="version" content="4.6.24">
<meta name="clientversion" content="4.3.44">
<meta name="creation" content="2023-04-04">
<meta name="creator" content="mweiss">

            */


        private void createDocuHtmlMetadata(System.Xml.XmlWriter W)
        {
            try
            {
                if (this.checkBoxHtmlIncludeMetadata.Checked)
                {
                    W.WriteStartElement("meta");
                    W.WriteAttributeString("name", "database");
                    W.WriteAttributeString("content", DiversityWorkbench.Settings.DatabaseName);
                    W.WriteEndElement(); // meta
                    W.WriteString("\r\n");

                    W.WriteStartElement("meta");
                    W.WriteAttributeString("name", "version");
                    W.WriteAttributeString("content", this.DatabaseVersion());
                    W.WriteEndElement(); // meta
                    W.WriteString("\r\n");

                    W.WriteStartElement("meta");
                    W.WriteAttributeString("name", "clientversion");
                    W.WriteAttributeString("content", System.Windows.Forms.Application.ProductVersion.ToString());
                    W.WriteEndElement(); // meta
                    W.WriteString("\r\n");

                    W.WriteStartElement("meta");
                    W.WriteAttributeString("name", "creation");
                    W.WriteAttributeString("content", System.DateTime.Now.ToString("yyyy-MM-dd"));
                    W.WriteEndElement(); // meta
                    W.WriteString("\r\n");

                    W.WriteStartElement("meta");
                    W.WriteAttributeString("name", "creator");
                    W.WriteAttributeString("content", System.Environment.UserName);
                    W.WriteEndElement(); // meta
                    W.WriteString("\r\n");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownMetadata(System.IO.StreamWriter W)
        {
            try
            {
                if (this.checkBoxHtmlIncludeMetadata.Checked)
                {
                    W.WriteLine("Database: " + DiversityWorkbench.Settings.DatabaseName);

                    W.WriteLine("Version: " + this.DatabaseVersion());

                    W.WriteLine("Client version: " + System.Windows.Forms.Application.ProductVersion.ToString());

                    W.WriteLine("Creation: " + System.DateTime.Now.ToString("yyyy-MM-dd"));

                    W.WriteLine("Creator: " + System.Environment.UserName);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private string DatabaseVersion()
        {
            string SQL = "SELECT dbo.Version()";
            string version = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            return version;
        }

        #endregion

        #region CSS

        private void setDefaultCSS(ref System.Collections.Generic.Dictionary<string, System.IO.FileInfo> CssDict, System.IO.DirectoryInfo directory = null)
        {
            try
            {
                if (CssDict == null) CssDict = new Dictionary<string, System.IO.FileInfo>();
                CssDict.Clear();

                //string DirectoryName = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation);
                //System.IO.FileInfo WaterCSS = new System.IO.FileInfo(DirectoryName + "water.css");
                //System.IO.FileInfo DwbCSS = new System.IO.FileInfo(DirectoryName + "DWB.css");

                System.IO.DirectoryInfo dir;
                if (directory != null) dir = directory;
                else if (_ChmDirectory != null) dir = _ChmDirectory;
                else dir = new System.IO.DirectoryInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation));


                System.IO.FileInfo WaterCSS = new System.IO.FileInfo(dir.FullName + "\\water.css");
                System.IO.FileInfo DwbCSS = new System.IO.FileInfo(dir.FullName + "\\DWB.css");

                if (!WaterCSS.Exists)
                {
                    try
                    {
                        string Content = DiversityWorkbench.Properties.Resources.water;
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(WaterCSS.FullName, false, System.Text.Encoding.UTF8);
                        sw.Write(Content);
                        sw.Close();
                        sw.Dispose();
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
                if (!DwbCSS.Exists)
                {
                    try
                    {
                        string Content = DiversityWorkbench.Properties.Resources.DWB;
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(DwbCSS.FullName, false, System.Text.Encoding.UTF8);
                        sw.Write(Content);
                        sw.Close();
                        sw.Dispose();
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }


                if (WaterCSS != null)
                    CssDict.Add(WaterCSS.Name, WaterCSS);
                if (WaterCSS != null)
                    CssDict.Add(DwbCSS.Name, DwbCSS);
                //this.initHtmlCssList();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        private void createDocuHtmlCSS(System.Xml.XmlWriter W, string FileName)
        {
            try
            {
                if (HtmlIncludeCSS)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> F in _HtmlCssFiles)
                    {
                        W.WriteStartElement("link");
                        W.WriteAttributeString("rel", "stylesheet");
                        W.WriteAttributeString("href", F.Value.Name);
                        W.WriteEndElement();
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(FileName);
                        System.IO.FileInfo css = new System.IO.FileInfo(fileInfo.DirectoryName + "\\" + F.Value.Name);
                        if (!css.Exists && F.Value.Exists)
                        {
                            F.Value.CopyTo(css.FullName);
                        }
                        W.WriteString("\r\n");
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownCSS(System.IO.StreamWriter W, string FileName)
        {
            try
            {
                if (HtmlIncludeCSS)
                {
                    string css = "";
                    foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> F in _HtmlCssFiles)
                    {
                        if (css.Length > 0) css += ", ";
                        css += F.Value.Name;
                    }
                    if (css.Length > 0)
                    {
                        W.WriteLine("output: ");
                        W.WriteLine("  html_document:");
                        W.WriteLine("   css: " + css);
                        W.WriteLine("     self_contained: no");
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private System.Collections.Generic.Dictionary<string, System.IO.FileInfo> _HtmlCssFiles;

        private void toolStripButtonHtmlCssAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Filter = "css files|*.css";
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    try
                    {
                        if (this._HtmlCssFiles == null)
                            this._HtmlCssFiles = new Dictionary<string, System.IO.FileInfo>();
                        System.IO.FileInfo css = new System.IO.FileInfo(this.openFileDialog.FileName);
                        this._HtmlCssFiles.Add(css.Name, css);
                        this.initHtmlCssList();
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void toolStripButtonHtmlCssRemove_Click(object sender, EventArgs e)
        {
            if (this._HtmlCssFiles.ContainsKey(this.listBoxHtmlCss.SelectedItem.ToString()))
            {
                this._HtmlCssFiles.Remove(this.listBoxHtmlCss.SelectedItem.ToString());
                this.initHtmlCssList();
            }
        }

        private void initHtmlCssList()
        {
            this.listBoxHtmlCss.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in this._HtmlCssFiles)
            {
                this.listBoxHtmlCss.Items.Add(KV.Key);
            }
        }

        #endregion

        #region ER diagam

        private void buttonHtmlER_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._outputType != OutputType.HUGO)
                {
                    DiversityWorkbench.Forms.FormWebBrowser f = new FormWebBrowser();
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.URL.Length > 0)
                        this.textBoxHtmlER.Text = f.URL;
                }
                else
                {
                    this.openFileDialog.ShowDialog();
                    if (this.openFileDialog.FileName.Length > 0)
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.openFileDialog.FileName);
                        if (fileInfo.Exists)
                            this.textBoxHtmlER.Text = fileInfo.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuHtmlER(System.Xml.XmlWriter W)
        {
            try
            {
                if (this.checkBoxHtml_ER_Include.Checked && this.textBoxHtmlER.Text.Length > 0)
                {
                    W.WriteStartElement("img");
                    W.WriteAttributeString("alt", "ER-diagram");
                    W.WriteAttributeString("src", "\"" + this.textBoxHtmlER.Text + "\"");
                    W.WriteAttributeString("width", "100%");
                    W.WriteEndElement();//img
                    W.WriteString("\r\n");
                    W.WriteElementString("br", "");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownER(System.IO.StreamWriter W)
        {
            try
            {
                if (this.checkBoxHtml_ER_Include.Checked && this.textBoxHtmlER.Text.Length > 0)
                {
                    W.WriteLine("![ER-diagram](" + this.textBoxHtmlER.Text + ")");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }



        #endregion

        #region Citation

        private void createDocuHtmlCitation(System.Xml.XmlWriter W)
        {
            try
            {
                if (this.checkBoxHtmlCitationInclude.Checked)
                {
                    string Header = DiversityWorkbench.Settings.ModuleName;
                    string SQL = "SELECT dbo.Version()";
                    string VersionDB = "";
                    VersionDB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    Header += " Information Model (version " + VersionDB + ")";
                    W.WriteStartElement("h2");
                    W.WriteString(this.textBoxHtmlModel.Text);
                    W.WriteEndElement();//h2
                    W.WriteString("\r\n");

                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownCitation(System.IO.StreamWriter W)
        {
            try
            {
                if (this.checkBoxHtmlCitationInclude.Checked)
                {
                    string Header = DiversityWorkbench.Settings.ModuleName;
                    string SQL = "SELECT dbo.Version()";
                    string VersionDB = "";
                    VersionDB = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    Header += " Information Model (version " + VersionDB + ")";
                    W.WriteLine("## " + this.textBoxHtmlModel.Text);

                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }



        #endregion

        #region Model

        private void createDocuHtmlModel(System.Xml.XmlWriter W)
        {
            try
            {
                if (this.checkBoxHtmlModel.Checked && this.textBoxHtmlModel.Text.Length > 0)
                {
                    W.WriteStartElement("h2");
                    W.WriteString(this.textBoxHtmlModel.Text);
                    W.WriteEndElement();//h2
                    W.WriteString("\r\n");

                    W.WriteString("This information model is available as ");
                    W.WriteStartElement("a");
                    W.WriteAttributeString("href", "\"" + this.textBoxHtmlModelLink.Text + "\"");
                    W.WriteString(this.textBoxHtmlModel.Text);
                    W.WriteEndElement();//a
                    W.WriteString(" with each single data table and data column referenced as term or concept by its own stable and persistent URL.");
                    W.WriteString("\r\n");
                    W.WriteElementString("br", "");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createDocuMarkdownModel(System.IO.StreamWriter W)
        {
            try
            {
                if (this.checkBoxHtmlModel.Checked && this.textBoxHtmlModel.Text.Length > 0)
                {
                    W.WriteLine("## " + this.textBoxHtmlModel.Text);

                    W.WriteLine("This information model is available as ");
                    W.WriteLine("[" + this.textBoxHtmlModel.Text + "](" + this.textBoxHtmlModelLink.Text + ")");
                    W.WriteLine(" with each single data table and data column referenced as term or concept by its own stable and persistent URL.");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region TableContents

        private string _TableContent = "";
        private System.Collections.Generic.List<string> _TableContentTables = new List<string>();
        private DiversityWorkbench.Data.Table _TableContentTable = null;

        //private System.Collections.Generic.List<string> _TableContentColumns = new List<string>();
        private void comboBoxTableContentTable_DropDown(object sender, EventArgs e)
        {
            _TableContentTables.Clear();
            this.comboBoxTableContentTable.DataSource = null;
            this.comboBoxTableContentTable.Items.Clear();
            foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
            {
                //System.Data.DataTable dt = this.dtTableDocu(R[0].ToString(), false);
                _TableContentTables.Add(R[0].ToString());
                this.comboBoxTableContentTable.Items.Add(R[0].ToString());
            }
            //this.comboBoxTableContentTable.DataSource = _TableContentTables;
        }
        private void comboBoxTableContentTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listBoxTableContentColumns.Items.Clear();
            this.comboBoxTableContentColumn.Items.Clear();
            _TableContentTable = new Data.Table(comboBoxTableContentTable.SelectedItem.ToString());
            foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Data.Column> C in _TableContentTable.Columns)
            {
                this.comboBoxTableContentColumn.Items.Add(C.Key);
            }
        }

        private void comboBoxTableContentColumn_SelectionChangeCommitted(object sender, EventArgs e)
        {

        }
        private void buttonTableContentAddColumn_Click(object sender, EventArgs e)
        {
            if (!this.listBoxTableContentColumns.Items.Contains(this.comboBoxTableContentColumn.SelectedItem.ToString()))
                this.listBoxTableContentColumns.Items.Add(this.comboBoxTableContentColumn.SelectedItem.ToString());
        }

        private void buttonTableContentRemoveColumn_Click(object sender, EventArgs e)
        {
            this.listBoxTableContentColumns.Items.Remove(this.listBoxTableContentColumns.SelectedItem);
        }

        private void TableContentAsMarkdown(System.IO.StreamWriter W, string TableName)
        {
            try
            {
                if (this.listBoxTableContentColumns.Items.Count > 0)
                {
                    System.Data.DataTable dt = this.TableContent(TableName);
                    if (dt.Rows.Count > 0)
                    {
                        string Header = "";
                        string Subline = "";
                        this.TableContentMarkdownHeader(ref Header, ref Subline);
                        if (Header.Length > 0)
                        {
                            W.WriteLine("#### Content");
                            W.WriteLine(Header);
                            W.WriteLine(Subline);
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                W.WriteLine(this.TableContentMarkdownRowContent(R));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void TableContentMarkdownHeader(ref string Header, ref string SubLine)
        {
            foreach (System.Object o in this.listBoxTableContentColumns.Items)
            {
                if (Header.Length > 0)
                {
                    Header += " | ";
                    SubLine += " | ";
                }
                Header += o.ToString();
                SubLine += " --- ";
            }
            Header = "| " + Header + " |";
            SubLine = "| " + SubLine + " |";
        }

        private string TableContentMarkdownRowContent(System.Data.DataRow R)
        {
            string Content = "";
            foreach (System.Object o in this.listBoxTableContentColumns.Items)
            {
                if (Content.Length > 0) Content += " | ";
                string content = " ";
                if (R.Table.Columns.Contains(o.ToString()) && !R[o.ToString()].Equals(System.DBNull.Value))
                {
                    content = R[o.ToString()].ToString();
                }
                Content += content;
            }
            Content = "| " + Content + " |";
            return Content;
        }


        private System.Data.DataTable TableContent(string TableName)
        {
            string SQL = "";
            foreach (System.Object o in this.listBoxTableContentColumns.Items)
            {
                if (SQL.Length > 0) SQL += ", ";
                SQL += o.ToString();
            }
            SQL = "SELECT " + SQL + " FROM " + TableName;
            System.Data.DataTable dataTable = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
            return dataTable;
        }

        //System.Collections.Generic.List<string> _TableContentColumnList;
        //private System.Collections.Generic.List<string> TableContentColumnList()
        //{
        //    if (_TableContentColumnList == null)
        //    {
        //        _TableContentColumnList = new List<string>();
        //        foreach (System.Object o in this.listBoxTableContentColumns.Items)
        //            _TableContentColumnList.Add(o.ToString());
        //    }
        //    return _TableContentColumnList;
        //}


        #endregion

        #endregion

        #region JSP-WIKI

        private void createDocuWiki()
        {
            string Wiki = "";
            try
            {
                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    System.Data.DataTable dt = this.dtTableDocu(R[0].ToString(), this.checkBoxMediaWikiUseContext.Checked);
                    this.createDocuWikiTable(ref Wiki, R[0].ToString(), dt);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            Wiki = Wiki.Replace("[", "~[");
            this.textBoxDocuWiki.Text = Wiki;
        }

        private void createDocuWikiTable(ref string Wiki, string Table, System.Data.DataTable dt)
        {
            try
            {
                Wiki += "!Table " + Table + "\r\n";
                Wiki += this.FormFunctions.TableDescription(Table) + "\r\n";
                if (this.textBoxDocuWikiCSSPrefix.Text.Length > 0) Wiki += "%%" + this.textBoxDocuWikiCSSPrefix.Text + "\r\n";
                foreach (System.Data.DataColumn CH in dt.Columns)
                {
                    if (CH.ColumnName != "Collation" && CH.ColumnName != "Length" && CH.ColumnName != "DefaultValue")
                    {
                        string Text = CH.ColumnName.ToString();
                        if (Text == "ColumnName") Text = "Column";
                        if (Text == "Datatype") Text = "Data type";
                        Wiki += "||" + Text;
                    }
                }
                Wiki += "\r\n";
                int i = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Description = this.FormFunctions.ColumnDescription(Table, R["ColumnName"].ToString());
                    if (!R["DefaultValue"].Equals(System.DBNull.Value))
                    {
                        string Default = R["DefaultValue"].ToString();
                        if (Default.StartsWith("(")) Default = Default.Substring(1);
                        if (Default.EndsWith(")")) Default = Default.Substring(0, Default.Length - 1);
                        if (Default == "NULL") Default = "";
                        if (Default == "''") Default = " \" ";
                        if (Default.Length > 0) Description += "\\\\ ''DefaultValue: " + Default + "''";
                    }
                    R["Description"] = Description;
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        bool IsPK = false;
                        System.Data.DataTable dtPK = this.dtPrimaryKey(Table);
                        foreach (System.Data.DataRow PK in dtPK.Rows)
                        {
                            if (PK[0].ToString() == R[C.ColumnName].ToString())
                            {
                                IsPK = true;
                                break;
                            }
                        }
                        if (C.ColumnName != "Collation" && C.ColumnName != "Length" && C.ColumnName != "DefaultValue")
                        {
                            string T = R[C.ColumnName].ToString();
                            if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                            {
                                T += " (" + R["Length"].ToString() + ")";
                            }
                            if (IsPK) Wiki += "|__" + T + "__";
                            else Wiki += "|" + T;
                        }
                    }
                    Wiki += "\r\n";
                    i++;
                }
                if (this.textBoxDocuWikiCSSPrefix.Text.Length > 0) Wiki += "%%\r\n\\\\\r\n";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonDocuWikiCreate_Click(object sender, EventArgs e)
        {
            this.createDocuWiki();
        }

        #endregion

        #region MediaWiki

        private void buttonDocuMediaWikiCreate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.createMediaWiki();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createMediaWiki()
        {
            string Wiki = "";
            try
            {
                if (this.checkBoxMediaWikiUseContext.Checked && this.checkBoxMediaWikiCreateColorCodeTable.Checked)
                {
                    if (this.checkBoxMediaWikiShowAccessibility.Checked)
                    {
                        System.Collections.Generic.Dictionary<string, bool> DictAccess = new Dictionary<string, bool>();
                        DictAccess.Add(DiversityWorkbench.Entity.Accessibility.no_restrictions.ToString(), true);
                        DictAccess.Add(DiversityWorkbench.Entity.Accessibility.read_only.ToString(), !this.userControlColorSettingsMediaWiki.HideReadOnly);
                        DictAccess.Add(DiversityWorkbench.Entity.Accessibility.inapplicable.ToString(), !this.userControlColorSettingsMediaWiki.HideInapplicable);
                        Wiki += "====Color code for accessibility====\r\n";
                        this.createMediaWikiColorCodeTable(ref Wiki, "EntityAccessibility_Enum", DictAccess);
                        Wiki += "\r\n\r\n";
                    }

                    if (this.checkBoxMediaWikiShowDetermination.Checked)
                    {
                        System.Collections.Generic.Dictionary<string, bool> DictDet = new Dictionary<string, bool>();
                        DictDet.Add(DiversityWorkbench.Entity.Determination.user_defined.ToString(), true);
                        DictDet.Add(DiversityWorkbench.Entity.Determination.service_link.ToString(), !this.userControlColorSettingsMediaWiki.HideServiceLink);
                        DictDet.Add(DiversityWorkbench.Entity.Determination.calculated.ToString(), !this.userControlColorSettingsMediaWiki.HideCalculated);
                        Wiki += "====Color code for determination====\r\n";
                        this.createMediaWikiColorCodeTable(ref Wiki, "EntityDetermination_Enum", DictDet);
                        Wiki += "\r\n\r\n";
                    }
                }
                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    System.Data.DataTable dt = this.dtTableDocumentation(R[0].ToString(), this.checkBoxMediaWikiIncludeIndex.Checked, this.checkBoxMediaWikiIncludeRequired.Checked);

                    string ObjectType = R["ObjectType"].ToString().ToLower();
                    switch (ObjectType)
                    {
                        case "table":
                            if (this.createMediaWikiTable(ref Wiki, R[0].ToString(), dt))
                            {
                                if (this.checkBoxMediaWikiIncludeRelation.Checked)
                                    this.addMediaWikiRelation(ref Wiki, R[0].ToString());
                                if (this.checkBoxMediaWikiIncludeContraints.Checked)
                                    this.addMediaWikiConstraint(ref Wiki, R[0].ToString());
                            }
                            break;
                        case "view":
                            if (this.createMediaWikiView(ref Wiki, R[0].ToString(), dt))
                            {
                                if (this.checkBoxMediaWikiIncludeRelation.Checked)
                                    this.addMediaWikiRelation(ref Wiki, R[0].ToString());
                                //if (this.checkBoxMediaWikiIncludeContraints.Checked)
                                //    this.addMediaWikiConstraint(ref Wiki, R[0].ToString());
                            }
                            break;
                        case "function":
                        case "procedure":
                            this.createMediaWikiProcedure(ref Wiki, R[0].ToString(), R[1].ToString());
                            break;
                        case "role":
                            this.createMediaWikiRole(ref Wiki, R[0].ToString());
                            break;
                    }
                    Wiki += "\r\n";



                }
                this._Connection.Disconnect();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            Wiki += "\r\n\r\n'''Footnotes''': The following conventions and abbreviations have been used in the tables:";
            if (this.checkBoxMediaWikiPKunderlined.Checked || this.checkBoxMediaWikiPKbold.Checked)
            {
                Wiki += " Columns of primary key:";
                if (this.checkBoxMediaWikiPKbold.Checked && this.checkBoxMediaWikiPKunderlined.Checked)
                    Wiki += "<u>'''bold and underlined'''</u>";
                else
                {
                    if (this.checkBoxMediaWikiPKbold.Checked)
                        Wiki += "'''bold''' ";
                    if (this.checkBoxMediaWikiPKunderlined.Checked)
                        Wiki += "<u>underlined</u>";
                }
            }
            if (this.checkBoxMediaWikiIncludeRequired.Checked)
                Wiki += " '''R''': It is required to enter data in this field.";
            if (this.checkBoxMediaWikiIncludeIndex.Checked)
                Wiki += " '''I''': The field is indexed to enable faster searching. '''U''': unique index";
            this.textBoxMediaWiki.Text = Wiki;
        }

        private bool createMediaWikiTable(ref string Wiki, string Table, System.Data.DataTable dt)
        {
            try
            {
                bool TableIsRestricted = false;
                string Entity = Table;
                System.Collections.Generic.Dictionary<string, string> dictEntity = DiversityWorkbench.Entity.EntityInformation(Entity);

                string AccessibilityOfTable = "";
                string DeterminationOfTable = "";
                string VisibilityOfTable = "";

                string AccessibilityOfColumn = "";
                string DeterminationOfColumn = "";
                string VisibilityOfColumn = "";

                if (this.checkBoxMediaWikiUseContext.Checked)
                {
                    AccessibilityOfTable = dictEntity["Accessibility"];
                    DeterminationOfTable = dictEntity["Determination"];
                    VisibilityOfTable = dictEntity["Visibility"];
                }
                if (AccessibilityOfTable.Length > 0)
                {
                    switch (AccessibilityOfTable)
                    {
                        case "read_only":
                            if (this.userControlColorSettingsMediaWiki.HideReadOnly) return false;
                            break;
                        case "inapplicable":
                            if (this.userControlColorSettingsMediaWiki.HideInapplicable) return false;
                            break;
                    }
                }
                if (VisibilityOfTable.Length > 0)
                {
                    switch (VisibilityOfTable)
                    {
                        case "hidden":
                            return false;
                            break;
                    }
                }

                string UsageTable = "";
                // the header for the table
                string HeaderWeight = "";
                for (int iHeader = 0; iHeader < this.numericUpDownMediaWikiTableHeaederWeight.Value; iHeader++)
                    HeaderWeight += "=";
                Wiki += HeaderWeight + "Table: " + Table + HeaderWeight + "\r\n";

                // the description
                Wiki += this.FormFunctions.TableDescription(Table) + "\r\n";

                // the table
                Wiki += "{|border=" + this.numericUpDownMediaWikiBorder.Value.ToString() +
                    " cellpadding=\"" + this.numericUpDownMediaWikiCellpadding.Value.ToString() +
                    "\" style=\"margin:1em 1em 1em 0; ";
                if (this.checkBoxMediaWikiUseContext.Checked)
                {

                    // the backcolor for the whole table
                    string ColorTable = this.userControlColorSettingsMediaWiki.ColorCode(AccessibilityOfTable);
                    if (AccessibilityOfTable.Length > 0 || VisibilityOfTable.Length > 0)
                        TableIsRestricted = true;
                    if (ColorTable.Length == 0)
                        ColorTable = "#FFFFFF";
                    Wiki += "background:" + ColorTable + ";";
                }
                else
                {
                    Wiki += "background:#" + this.textBoxMediaWikiColorTableBack.BackColor.R.ToString("X2") +
                        this.textBoxMediaWikiColorTableBack.BackColor.G.ToString("X2") +
                        this.textBoxMediaWikiColorTableBack.BackColor.B.ToString("X2") + ";";
                }
                Wiki += "border:1px #AAA solid; " +
                    "border-collapse:collapse; " +
                    "empty-cells:show;\"\r\n" +
                    "|-style=\"text-align:left; background:";
                if (this.checkBoxMediaWikiUseContext.Checked)
                {
                    string ColorTable = this.userControlColorSettingsMediaWiki.ColorCode(AccessibilityOfTable);
                    if (ColorTable.Length == 0)
                        ColorTable = "#FFFFFF";
                    Wiki += ColorTable;
                }
                else
                {
                    Wiki += "#" + this.textBoxMediaWikiColor.BackColor.R.ToString("X2") +
                                this.textBoxMediaWikiColor.BackColor.G.ToString("X2") +
                                this.textBoxMediaWikiColor.BackColor.B.ToString("X2");
                }
                Wiki += ";\"\r\n";
                foreach (System.Data.DataColumn CH in dt.Columns)
                {
                    if (CH.ColumnName != "Collation" && CH.ColumnName != "Length" && CH.ColumnName != "DefaultValue")
                    {
                        string Text = CH.ColumnName.ToString();
                        if (Text == "ColumnName") Text = "Column";
                        if (Text == "Datatype") Text = "Data type";
                        if (Text == "Required") Text = "Requ.";
                        if (Text == "Indices") Text = "Ind.";
                        Wiki += "!" + Text + "\r\n";
                    }
                }

                int i = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    // if a column with a certain name pattern should not be printed
                    bool Suppress = false;
                    Entity = Table + "." + R["ColumnName"].ToString();
                    string UsageColumn = "";
                    string PresetValue = "";
                    System.Collections.Generic.Dictionary<string, string> dict = DiversityWorkbench.Entity.EntityInformation(Entity);
                    if (this.checkBoxMediaWikiUseContext.Checked && dict.Count > 0 && dict.ContainsKey("Accessibility") && dict.ContainsKey("PresetValue"))
                    {
                        //Description = dict["Description"];
                        AccessibilityOfColumn = dict["Accessibility"];
                        DeterminationOfColumn = dict["Determination"];
                        VisibilityOfColumn = dict["Visibility"];
                        //UsageColumn = dict["EntityUsage"];
                        PresetValue = dict["PresetValue"];
                    }
                    if (this.checkBoxMediaWikiSuppressColumns.Checked && this.textBoxMediaWikiSuppressColumns.Text.Length > 0)
                    {
                        string x = R["ColumnName"].ToString();
                        string[] SuppressColumns = this.textBoxMediaWikiSuppressColumns.Text.Split((new Char[] { '|' }));
                        for (int ii = 0; ii < SuppressColumns.Length; ii++)
                        {
                            if (R["ColumnName"].ToString().StartsWith(SuppressColumns[ii]))
                            {
                                Suppress = true;
                                break;
                            }
                        }
                    }
                    if (this.checkBoxMediaWikiUseContext.Checked)
                    {
                        // if a column with a certain usage should not be printed
                        switch (AccessibilityOfColumn.ToLower())
                        {
                            case "inapplicable":
                                if (this.userControlColorSettingsMediaWiki.HideInapplicable) continue;
                                break;
                            case "read_only":
                                if (this.userControlColorSettingsMediaWiki.HideReadOnly) continue;
                                break;
                        }
                        switch (DeterminationOfColumn.ToLower())
                        {
                            case "calculated":
                                if (this.userControlColorSettingsMediaWiki.HideCalculated) continue;
                                break;
                            case "service_link":
                                if (this.userControlColorSettingsMediaWiki.HideServiceLink) continue;
                                break;
                        }
                    }
                    if (!Suppress)
                    {
                        string Description = this.FormFunctions.ColumnDescription(Table, R["ColumnName"].ToString());
                        if (!R["DefaultValue"].Equals(System.DBNull.Value))
                        {
                            string Default = R["DefaultValue"].ToString();
                            if (Default.StartsWith("(")) Default = Default.Substring(1);
                            if (Default.EndsWith(")")) Default = Default.Substring(0, Default.Length - 1);
                            if (Default == "NULL") Default = "";
                            if (Default == "''") Default = "";
                            if (Default.Length > 0) Description += "<br />''DefaultValue: " + Default + "''";
                        }
                        R["Description"] = Description;
                        string DisplayTextTable = Table;
                        string UsageNotes = "";
                        if (this.checkBoxMediaWikiUseContext.Checked)
                        {
                            Description = "";
                            Description = dict["Description"];
                            PresetValue = dict["PresetValue"];
                            if (dict.ContainsKey("UsageNotes"))
                                UsageNotes = dict["UsageNotes"];

                            // getting the backcolor of the fields
                            string ColorCode = "";
                            DiversityWorkbench.Entity.Accessibility Accessibility = DiversityWorkbench.Entity.Accessibility.no_restrictions;
                            DiversityWorkbench.Entity.Determination Determination = DiversityWorkbench.Entity.Determination.user_defined;
                            DiversityWorkbench.Entity.Visibility Visibility = DiversityWorkbench.Entity.Visibility.visible;

                            if (AccessibilityOfTable == "read_only")
                                AccessibilityOfColumn = "read_only";
                            else if (AccessibilityOfTable == "inapplicable")
                                AccessibilityOfColumn = "inapplicable";

                            switch (AccessibilityOfColumn)
                            {
                                case "inapplicable":
                                    Accessibility = DiversityWorkbench.Entity.Accessibility.inapplicable;
                                    break;
                                case "read_only":
                                    Accessibility = DiversityWorkbench.Entity.Accessibility.read_only;
                                    break;
                            }
                            switch (DeterminationOfColumn)
                            {
                                case "service_link":
                                    Determination = DiversityWorkbench.Entity.Determination.service_link;
                                    break;
                                case "calculated":
                                    Determination = DiversityWorkbench.Entity.Determination.calculated;
                                    break;
                            }
                            switch (VisibilityOfColumn)
                            {
                                case "optional":
                                    Visibility = DiversityWorkbench.Entity.Visibility.optional;
                                    break;
                                case "hidden":
                                    Visibility = DiversityWorkbench.Entity.Visibility.hidden;
                                    break;
                            }
                            ColorCode = this.userControlColorSettingsMediaWiki.ColorCode(Accessibility, Determination, Visibility);
                            string ColorText = this.userControlColorSettingsMediaWiki.ColorCodeText(Accessibility.ToString());

                            if (UsageColumn.Length > 0 && AccessibilityOfTable.Length == 0)
                            {
                                ColorCode = this.userControlColorSettingsMediaWiki.ColorCode(AccessibilityOfTable);
                            }
                            if (ColorCode.Length == 0)
                            {
                                if (PresetValue.Length > 0) ColorCode = this.ColorCodeMediaWiki("preset");
                                else if (AccessibilityOfTable == "" && !this.checkBoxMediaWikiUseContext.Checked)
                                {
                                    ColorCode = "#FFFFFF";
                                }
                            }
                            if (ColorCode.Length == 0)
                                ColorCode = "#FFFFFF";
                            if (!Suppress && !TableIsRestricted)
                            {
                                Wiki += "|-style=\"background:" + ColorCode + ";\"\r\n";
                            }
                            else if (!Suppress)
                                Wiki += "|-\r\n";
                        }
                        else
                        {
                            if (i % 2 == 1)
                            {
                                string ColorCode = this.textBoxMediaWikiColorTableLine.BackColor.ToKnownColor().ToString();
                                Wiki += "|-style=\"background:" + ColorCode + ";\"\r\n";
                            }
                            else
                            {
                                Wiki += "|-\r\n";
                            }
                        }
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            string Default = "";
                            bool IsPK = false;
                            System.Data.DataTable dtPK = this.dtPrimaryKey(Table);
                            foreach (System.Data.DataRow PK in dtPK.Rows)
                            {
                                if (PK[0].ToString() == R[C.ColumnName].ToString())
                                {
                                    IsPK = true;
                                    break;
                                }
                            }
                            if (C.ColumnName != "DefaultValue")
                            {
                                if (this.checkBoxMediaWikiUseContext.Checked && PresetValue.Length > 0)
                                {
                                    Default = "Preset value: " + PresetValue;
                                }
                            }
                            if (C.ColumnName != "Collation" && C.ColumnName != "Length" && C.ColumnName != "DefaultValue")
                            {
                                string T = R[C.ColumnName].ToString();
                                if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                                {
                                    if (R["Length"].ToString() != "-1")
                                        T += " (" + R["Length"].ToString() + ")";
                                    else if (T != "geography")
                                        T += " (MAX)";
                                }
                                Wiki += "|";
                                if (IsPK)
                                {
                                    if (this.checkBoxMediaWikiPKunderlined.Checked)
                                        Wiki += "<u>";
                                    if (this.checkBoxMediaWikiPKbold.Checked)
                                        Wiki += "'''";
                                    Wiki += T;
                                    if (this.checkBoxMediaWikiPKbold.Checked)
                                        Wiki += "'''";
                                    if (this.checkBoxMediaWikiPKunderlined.Checked)
                                        Wiki += "</u>";
                                }
                                else
                                {
                                    if (T.Length == 0)
                                        Wiki += "&nbsp;";
                                    else if (T == "-")
                                        Wiki += "&nbsp; - &nbsp;";
                                    else
                                        Wiki += T;
                                }
                                if (Default.Length > 0 && C.ColumnName == "Description")
                                {
                                    Wiki += "<br />";
                                    Wiki += "''" + Default + "''";
                                }
                                Wiki += "\r\n";
                            }
                        }
                        i++;
                    }
                }
                Wiki += "|}\r\n\r\n";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        private void addMediaWikiRelation(ref string Wiki, string Table)
        {
            try
            {
                if (this.dtTableDocuConstraints(Table).Rows.Count == 0) return;
                System.Data.DataRow[] RR = this._dtCurrentConstraint.Select("CONSTRAINT_TYPE = 'FOREIGN KEY'");
                if (RR.Length > 0)
                {
                    // the index table
                    Wiki += "\r\n\r\n<u>'''Relations to other tables'''</u>\r\n\r\n";
                    Wiki += "{|\r\n";
                    Wiki += "|'''Columns'''\r\n|'''Related table'''\r\n|'''Columns in related table'''\r\n|'''Update rule'''\r\n|'''Delete rule'''\r\n";
                    Wiki += "|-\r\n";
                    foreach (System.Data.DataRow R in RR)
                    {
                        Wiki += "|" + R["ColumnsConstraint"].ToString() + "\r\n";
                        Wiki += "|" + R["TABLE_NAME"].ToString() + "\r\n";
                        Wiki += "|" + R["ColumnsUnique"].ToString() + "\r\n";
                        Wiki += "|" + R["UPDATE_RULE"].ToString() + "\r\n";
                        Wiki += "|" + R["DELETE_RULE"].ToString() + "\r\n";
                        Wiki += "|-\r\n";
                    }
                    Wiki += "|}\r\n";
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void addMediaWikiConstraint(ref string Wiki, string Table)
        {
            try
            {
                if (this.dtTableDocuConstraints(Table).Rows.Count == 0) return;
                System.Data.DataRow[] RR = this._dtCurrentConstraint.Select("CONSTRAINT_TYPE = 'CHECK'");
                if (RR.Length > 0)
                {
                    // the index table
                    Wiki += "\r\n\r\n<u>'''Constraints of the table'''</u>\r\n\r\n";
                    Wiki += "{|\r\n";
                    Wiki += "|'''Constraint'''\r\n|'''Columns'''\r\n";
                    Wiki += "|-\r\n";
                    foreach (System.Data.DataRow R in RR)
                    {
                        Wiki += "|" + R["CONSTRAINT_NAME"].ToString() + "\r\n";
                        Wiki += "|" + R["ColumnsConstraint"].ToString() + "\r\n";
                        Wiki += "|-\r\n";
                    }
                    Wiki += "|}\r\n";
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool createMediaWikiView(ref string Wiki, string View, System.Data.DataTable dt)
        {
            try
            {
                bool TableIsRestricted = false;
                string Entity = View;
                System.Collections.Generic.Dictionary<string, string> dictEntity = DiversityWorkbench.Entity.EntityInformation(Entity);

                // the header for the table
                string HeaderWeight = "";
                for (int iHeader = 0; iHeader < this.numericUpDownMediaWikiTableHeaederWeight.Value; iHeader++)
                    HeaderWeight += "=";
                Wiki += HeaderWeight + "View: " + View + HeaderWeight + "\r\n";

                // the description
                Wiki += DiversityWorkbench.Forms.FormFunctions.getDescription(View, "View", "", this._ConnectionString) + "\r\n";

                // the table
                Wiki += "{|border=" + this.numericUpDownMediaWikiBorder.Value.ToString() +
                    " cellpadding=\"" + this.numericUpDownMediaWikiCellpadding.Value.ToString() +
                    "\" style=\"margin:1em 1em 1em 0; ";
                Wiki += "background:#" + this.textBoxMediaWikiColorTableBack.BackColor.R.ToString("X2") +
                    this.textBoxMediaWikiColorTableBack.BackColor.G.ToString("X2") +
                    this.textBoxMediaWikiColorTableBack.BackColor.B.ToString("X2") + ";";
                Wiki += "border:1px #AAA solid; " +
                    "border-collapse:collapse; " +
                    "empty-cells:show;\"\r\n" +
                    "|-style=\"text-align:left; background:";
                Wiki += "#" + this.textBoxMediaWikiColor.BackColor.R.ToString("X2") +
                            this.textBoxMediaWikiColor.BackColor.G.ToString("X2") +
                            this.textBoxMediaWikiColor.BackColor.B.ToString("X2");
                Wiki += ";\"\r\n";
                foreach (System.Data.DataColumn CH in dt.Columns)
                {
                    if (CH.ColumnName != "Collation" && CH.ColumnName != "Length" && CH.ColumnName != "DefaultValue" && CH.ColumnName != "Required" && CH.ColumnName != "Indices")
                    {
                        string Text = CH.ColumnName.ToString();
                        if (Text == "ColumnName") Text = "Column";
                        if (Text == "Datatype") Text = "Data type";
                        if (Text == "Required") Text = "Requ.";
                        if (Text == "Indices") Text = "Ind.";
                        Wiki += "!" + Text + "\r\n";
                    }
                }

                int i = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    // if a column with a certain name pattern should not be printed
                    bool Suppress = false;
                    Entity = View + "." + R["ColumnName"].ToString();
                    string PresetValue = "";
                    System.Collections.Generic.Dictionary<string, string> dict = DiversityWorkbench.Entity.EntityInformation(Entity);
                    if (this.checkBoxMediaWikiSuppressColumns.Checked && this.textBoxMediaWikiSuppressColumns.Text.Length > 0)
                    {
                        string x = R["ColumnName"].ToString();
                        string[] SuppressColumns = this.textBoxMediaWikiSuppressColumns.Text.Split((new Char[] { '|' }));
                        for (int ii = 0; ii < SuppressColumns.Length; ii++)
                        {
                            if (R["ColumnName"].ToString().StartsWith(SuppressColumns[ii]))
                            {
                                Suppress = true;
                                break;
                            }
                        }
                    }
                    if (!Suppress)
                    {
                        string Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(View, true, R["ColumnName"].ToString(), true, this._ConnectionString);
                        R["Description"] = Description;
                        string DisplayTextTable = View;
                        string UsageNotes = "";
                        if (i % 2 == 1)
                        {
                            string ColorCode = this.textBoxMediaWikiColorTableLine.BackColor.ToKnownColor().ToString();
                            Wiki += "|-style=\"background:" + ColorCode + ";\"\r\n";
                        }
                        else
                        {
                            Wiki += "|-\r\n";
                        }
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName == "Collation" || C.ColumnName == "DefaultValue" || C.ColumnName == "Required" || C.ColumnName == "Indices")
                                continue;
                            string Default = "";
                            bool IsPK = false;
                            System.Data.DataTable dtPK = this.dtPrimaryKey(View);
                            foreach (System.Data.DataRow PK in dtPK.Rows)
                            {
                                if (PK[0].ToString() == R[C.ColumnName].ToString())
                                {
                                    IsPK = true;
                                    break;
                                }
                            }
                            if (C.ColumnName != "DefaultValue")
                            {
                                if (this.checkBoxMediaWikiUseContext.Checked && PresetValue.Length > 0)
                                {
                                    Default = "Preset value: " + PresetValue;
                                }
                            }
                            if (C.ColumnName != "Collation" && C.ColumnName != "Length" && C.ColumnName != "DefaultValue")
                            {
                                string T = R[C.ColumnName].ToString();
                                if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                                {
                                    if (R["Length"].ToString() != "-1")
                                        T += " (" + R["Length"].ToString() + ")";
                                    else if (T != "geography")
                                        T += " (MAX)";
                                }
                                Wiki += "|";
                                if (IsPK)
                                {
                                    if (this.checkBoxMediaWikiPKunderlined.Checked)
                                        Wiki += "<u>";
                                    if (this.checkBoxMediaWikiPKbold.Checked)
                                        Wiki += "'''";
                                    Wiki += T;
                                    if (this.checkBoxMediaWikiPKbold.Checked)
                                        Wiki += "'''";
                                    if (this.checkBoxMediaWikiPKunderlined.Checked)
                                        Wiki += "</u>";
                                }
                                else
                                {
                                    if (T.Length == 0)
                                        Wiki += "&nbsp;";
                                    else if (T == "-")
                                        Wiki += "&nbsp; - &nbsp;";
                                    else
                                        Wiki += T;
                                }
                                if (Default.Length > 0 && C.ColumnName == "Description")
                                {
                                    Wiki += "<br />";
                                    Wiki += "''" + Default + "''";
                                }
                                Wiki += "\r\n";
                            }
                        }
                        i++;
                    }
                }
                Wiki += "|}\r\n\r\n";

                this.createMediaWikiDependencies(ref Wiki, View, "VIEW");

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        private void createMediaWikiDependencies(ref string Wiki, string Object, string ObjectType)
        {
            if (this.checkBoxMediaWikiIncludeRelation.Checked)
            {
                string SQL = "SELECT referenced_entity_name " +
                    "FROM sys.sql_expression_dependencies " +
                    "WHERE referencing_id = OBJECT_ID(N'dbo." + Object + "');";
                if (ObjectType.ToLower() == "table")
                {
                    SQL = "SELECT DISTINCT P.TABLE_NAME " +
                        "FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R  " +
                        "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS F ON R.CONSTRAINT_NAME = F.CONSTRAINT_NAME  " +
                        "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME  " +
                        "RIGHT OUTER JOIN INFORMATION_SCHEMA.COLUMNS AS C ON F.COLUMN_NAME = C.COLUMN_NAME AND F.TABLE_NAME = C.TABLE_NAME AND F.TABLE_SCHEMA = C.TABLE_SCHEMA AND F.TABLE_CATALOG = C.TABLE_CATALOG    " +
                        "WHERE C.TABLE_NAME = '" + Object + "' and not P.TABLE_NAME is null and P.TABLE_NAME <> '" + Object + "' " +
                        "ORDER BY P.TABLE_NAME";
                }
                if (ObjectType == "USER")
                {

                }
                System.Data.DataTable dtDepend = new DataTable();
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtDepend, ref Message);
                if (dtDepend.Rows.Count > 0)
                {
                    Wiki += "Depending on:\r\n";
                    foreach (System.Data.DataRow rr in dtDepend.Rows)
                    {
                        Wiki += "*" + rr[0].ToString() + "\r\n";
                    }
                }
            }
        }

        private bool createMediaWikiProcedure(ref string Wiki, string Routine, string RoutineType)
        {
            try
            {
                // The header
                string HeaderWeight = "";
                for (int iHeader = 0; iHeader < this.numericUpDownMediaWikiTableHeaederWeight.Value; iHeader++)
                    HeaderWeight += "=";
                Wiki += HeaderWeight + RoutineType + ": " + Routine + HeaderWeight + "\r\n";

                // the description
                Wiki += DiversityWorkbench.Forms.FormFunctions.getDescription(Routine, RoutineType, "", this._ConnectionString) + "\r\n";

                string SQL = "SELECT DATA_TYPE + case when CHARACTER_MAXIMUM_LENGTH is null then '' else ' (' + cast(CHARACTER_MAXIMUM_LENGTH as varchar) + ')' end AS DataType " +
                    "FROM INFORMATION_SCHEMA.ROUTINES " +
                    "WHERE ROUTINE_NAME = '" + Routine + "' ";
                string DataType = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (DataType.ToUpper() != "TABLE" && DataType.Length > 0)
                {
                    Wiki += "DataType: " + DataType + "\r\n";
                }

                // PARAMETERS OF THE FUNCTION
                System.Data.DataTable dt = new DataTable();
                if (RoutineType.ToLower() == "function")
                    dt = DiversityWorkbench.Data.Routine.Parameters(Routine, DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION);
                else
                    dt = DiversityWorkbench.Data.Routine.Parameters(Routine, DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.PROCEDURE);
                if (dt.Rows.Count > 0)
                {
                    // PARAMETERS OF THE ROUTINE
                    // the table
                    Wiki += "{|border=" + this.numericUpDownMediaWikiBorder.Value.ToString() +
                        " cellpadding=\"" + this.numericUpDownMediaWikiCellpadding.Value.ToString() +
                        "\" style=\"margin:1em 1em 1em 0; background:#FFFFFF;border:1px #AAA solid; border-collapse:collapse; empty-cells:show; \r\n";
                    Wiki += "|-style=\"text-align:left; background:#D3D3D3;\"\r\n";
                    foreach (System.Data.DataColumn CH in dt.Columns)
                    {
                        if (CH.ColumnName.ToUpper() == "ORDINAL_POSITION")
                            continue;
                        if (CH.ColumnName != "Collation" && CH.ColumnName != "Length")
                        {
                            string Text = CH.ColumnName.ToString();
                            if (Text.ToUpper() == "NAME")
                                Text = "Parameter";
                            if (Text == "Datatype")
                                Text = "Type";
                            Wiki += "!" + Text + "\r\n";
                        }
                    }

                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        Wiki += "|-";
                        if (i % 2 == 1)
                            Wiki += "style=\"background:WhiteSmoke;\"";
                        Wiki += "\r\n";
                        string Description = R["Description"].ToString();// 
                        if (Description.Length == 0)
                        {
                            Description = "-";
                        }
                        R["Description"] = Description;
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName.ToUpper() == "ORDINAL_POSITION")
                                continue;

                            if (C.ColumnName != "Collation" && C.ColumnName != "Length" && C.ColumnName != "DefaultValue")
                            {
                                string T = R[C.ColumnName].ToString();
                                if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                                {
                                    if (R["Length"].ToString() != "-1")
                                        T += " (" + R["Length"].ToString() + ")";
                                    else if (T != "geography")
                                        T += " (MAX)";
                                }
                                Wiki += "|" + T + "\r\n";
                            }
                        }
                        i++;
                    }

                    // End of table
                    Wiki += "|}\r\n\r\n";
                }

                // COLUMNS OF THE ROUTINE
                dt = DiversityWorkbench.Data.Routine.Columns(Routine);// this.dtTableDocu(Routine, false);

                if (dt.Rows.Count > 0)
                {
                    // table
                    Wiki += "{|border=" + this.numericUpDownMediaWikiBorder.Value.ToString() +
                        " cellpadding=\"" + this.numericUpDownMediaWikiCellpadding.Value.ToString() +
                        "\" style=\"margin:1em 1em 1em 0; background:#FFFFFF;border:1px #AAA solid; border-collapse:collapse; empty-cells:show; \r\n";

                    Wiki += "|-style=\"text-align:left; background:#D3D3D3;\"\r\n";
                    foreach (System.Data.DataColumn CH in dt.Columns)
                    {
                        if (CH.ColumnName.ToUpper() == "ORDINAL_POSITION")
                            continue;
                        string Text = CH.ColumnName.ToString();
                        if (Text == "Name") Text = "Column";
                        if (Text == "Datatype") Text = "Type";
                        Wiki += "!" + Text + "\r\n";
                    }

                    // printing the informations for the columns
                    int i = 0;
                    foreach (System.Data.DataRow R in dt.Rows)
                    {
                        string Description = R["Description"].ToString();// DiversityWorkbench.Forms.FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION, Routine, R["ColumnName"].ToString());
                        if (Description.Length == 0)
                        {
                            Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescriptionOfBase(Routine, R[0].ToString());
                            if (Description.Length == 0)
                                Description = "-";
                        }
                        R["Description"] = Description;
                        Wiki += "|-";
                        if (i % 2 == 1)
                            Wiki += "style=\"background:WhiteSmoke;\"";
                        Wiki += "\r\n";
                        foreach (System.Data.DataColumn C in R.Table.Columns)
                        {
                            if (C.ColumnName.ToUpper() == "ORDINAL_POSITION")
                                continue;
                            string T = R[C.ColumnName].ToString();
                            if (C.ColumnName == "Datatype" && !R["Length"].Equals(System.DBNull.Value))
                            {
                                if (R["Length"].ToString() != "-1")
                                    T += " (" + R["Length"].ToString() + ")";
                                else if (T != "geography")
                                    T += " (MAX)";
                            }

                            if (C.ColumnName != "Notes")
                                Wiki += "|" + T + "\r\n";
                        }
                        i++;
                    }
                    // End of table
                    Wiki += "|}\r\n\r\n";
                }

                this.createMediaWikiDependencies(ref Wiki, Routine, RoutineType);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }

        private bool createMediaWikiRole(ref string Wiki, string Role)
        {
            try
            {
                string HeaderWeight = "";
                for (int iHeader = 0; iHeader < this.numericUpDownMediaWikiTableHeaederWeight.Value; iHeader++)
                    HeaderWeight += "=";
                Wiki += HeaderWeight + "Role: " + Role + HeaderWeight + "\r\n";

                // the description
                Wiki += DiversityWorkbench.Forms.FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.USER, Role, "") + "\r\n";

                this.createMediaWikiPermissions(ref Wiki, Role);
                this.createMediaWikiIncludedRoles(ref Wiki, Role);

                Wiki += "\r\n";
            }
            catch (System.Exception ex)
            {
                return false;
            }
            return true;
        }

        private void createMediaWikiPermissions(ref string Wiki, string Role)
        {
            try
            {
                // Objects
                System.Collections.Generic.List<string> Objects = new List<string>();
                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    if (R["ObjectType"].ToString().ToLower() != "role")
                        Objects.Add(R[0].ToString());
                }

                // Permissions
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<Data.Role.Permission, string>> PerDict = DiversityWorkbench.Data.Role.AllPermissions(Role);

                // Table header
                System.Collections.Generic.List<Data.Role.Permission> PermissionColumns = new List<Data.Role.Permission>();
                //Columns.Add("Permissions");
                PermissionColumns.Add(Data.Role.Permission.SELECT);
                PermissionColumns.Add(Data.Role.Permission.INSERT);
                PermissionColumns.Add(Data.Role.Permission.UPDATE);
                PermissionColumns.Add(Data.Role.Permission.DELETE);
                PermissionColumns.Add(Data.Role.Permission.EXECUTE);

                // the table
                Wiki += "{|border=" + this.numericUpDownMediaWikiBorder.Value.ToString() +
                    " cellpadding=\"" + this.numericUpDownMediaWikiCellpadding.Value.ToString() +
                    "\" style=\"margin:1em 1em 1em 0; ";
                Wiki += "background:#" + this.textBoxMediaWikiColorTableBack.BackColor.R.ToString("X2") +
                    this.textBoxMediaWikiColorTableBack.BackColor.G.ToString("X2") +
                    this.textBoxMediaWikiColorTableBack.BackColor.B.ToString("X2") + ";";
                Wiki += "border:1px #AAA solid; " +
                    "border-collapse:collapse; " +
                    "empty-cells:show;\"\r\n" +
                    "|-style=\"text-align:left; background:";
                Wiki += "#" + this.textBoxMediaWikiColor.BackColor.R.ToString("X2") +
                            this.textBoxMediaWikiColor.BackColor.G.ToString("X2") +
                            this.textBoxMediaWikiColor.BackColor.B.ToString("X2");
                Wiki += ";\"\r\n";

                Wiki += "!Permissions\r\n";
                int i = 0;
                foreach (Data.Role.Permission C in PermissionColumns)
                {
                    Wiki += "!" + C.ToString() + "\r\n";
                }
                // Column for Type
                Wiki += "!TYPE\r\n";

                // printing the informations for the columns
                i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<Data.Role.Permission, string>> KVobj in PerDict)
                {
                    if (this.checkBoxExcludeSystemObjects.Checked)
                    {
                        // exclude the system objects
                        if (this.DatabaseSystemObjects.ContainsKey(KVobj.Key))
                            continue;

                        if (!KVobj.Key.EndsWith("_Enum")) // enums are included in any case with this option
                        {
                            if (KVobj.Key.EndsWith("_log"))
                            {
                                // Include log tables for selected objects
                                string DataTable = KVobj.Key.Remove(KVobj.Key.Length - 4);
                                if (!Objects.Contains(DataTable))
                                    continue;
                            }
                            else if (!Objects.Contains(KVobj.Key))
                                continue;
                        }
                    }
                    else
                    {
                        // if no objects are selected, show all otherwise restrict to selected
                        if (Objects.Count > 0 && !Objects.Contains(KVobj.Key))
                            continue;
                    }

                    // Column for ObjectName
                    Wiki += "|-style=\"text-align:center";
                    if (i % 2 == 1)
                        Wiki += "; background:WhiteSmoke;";
                    Wiki += "\"\r\n";

                    Wiki += "|" + KVobj.Key + "\r\n";

                    // Columns for Permissions
                    foreach (Data.Role.Permission C in PermissionColumns)
                    {
                        if (KVobj.Value[C].Length == 0)
                        {
                            Wiki += "|\r\n";
                        }
                        else if (KVobj.Value[C] == Role)
                        {
                            Wiki += "|<big>&bull;</big>\r\n";
                        }
                        else
                        {
                            string InheritedFrom = KVobj.Value[C];
                            Wiki += "|<small><span style=\"color: gray\">" + InheritedFrom + "</span></small>\r\n";
                        }
                    }

                    // Column for Type
                    string Type = DiversityWorkbench.Data.Role.Securables()[KVobj.Key];
                    if (Type == "BASE TABLE")
                        Type = "TABLE";
                    Wiki += "|" + Type + "\r\n";
                    i++;
                }

                Wiki += "|}\r\n\r\n";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createMediaWikiIncludedRoles(ref string Wiki, string Object)
        {
            if (this.checkBoxMediaWikiIncludeRelation.Checked)
            {
                System.Collections.Generic.List<string> Roles = DiversityWorkbench.Data.Role.InheritingFromRoles(Object);
                if (Roles.Count > 0)
                {
                    Wiki += "Inheriting from roles:" + "\r\n";
                    foreach (string R in Roles)
                    {
                        Wiki += R + "\r\n";
                    }
                }
            }
        }

        private void createMediaWikiIndex(ref string Wiki, string Table, System.Data.DataTable dt)
        {
            try
            {

                // the index table
                Wiki += "\r\n\r\n'''Indices and contraints'''\r\n\r\n";
                Wiki += "{|border=" + this.numericUpDownMediaWikiBorder.Value.ToString() +
                    " cellpadding=\"" + this.numericUpDownMediaWikiCellpadding.Value.ToString() +
                    "\" style=\"margin:1em 1em 1em 0; " +
                    "background:#" + this.textBoxMediaWikiColorTableBack.BackColor.R.ToString("X2") +
                    this.textBoxMediaWikiColorTableBack.BackColor.G.ToString("X2") +
                    this.textBoxMediaWikiColorTableBack.BackColor.B.ToString("X2") + ";" +
                    "border:1px #AAA solid; " +
                    "border-collapse:collapse; " +
                    "empty-cells:show;\"\r\n" +
                    "|-style=\"text-align:left; background:#" + this.textBoxMediaWikiColor.BackColor.R.ToString("X2") +
                            this.textBoxMediaWikiColor.BackColor.G.ToString("X2") +
                            this.textBoxMediaWikiColor.BackColor.B.ToString("X2") +
                            ";\"\r\n";
                Wiki += "!Type\r\n!Name\r\n!Attributes and properties\r\n";
                Wiki += "|-\r\n";
                int i = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        Wiki += "|" + R[C.ColumnName].ToString();
                        Wiki += "\r\n";
                    }
                    if (i < dt.Rows.Count - 1)
                    {
                        if (i % 2 == 0)
                        {
                            Wiki += "|-style=\"background:#" + this.textBoxMediaWikiColorTableLine.BackColor.R.ToString("X2") +
                                    this.textBoxMediaWikiColorTableLine.BackColor.G.ToString("X2") +
                                    this.textBoxMediaWikiColorTableLine.BackColor.B.ToString("X2") + ";\"\r\n";
                        }
                        else
                            Wiki += "|-\r\n";
                    }
                    else
                        Wiki += "|}\r\n";
                    i++;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonMediaWikiColor_Click(object sender, EventArgs e)
        {
            this.colorDialog = new ColorDialog();
            // Keeps the user from selecting a custom color.
            this.colorDialog.AllowFullOpen = true;
            // Allows the user to get help. (The default is false.)
            this.colorDialog.ShowHelp = true;
            // Sets the initial color select to the current text color.
            this.colorDialog.Color = this.textBoxMediaWikiColor.BackColor;

            // Update the text box color if the user clicks OK 
            if (this.colorDialog.ShowDialog() == DialogResult.OK)
                this.textBoxMediaWikiColor.BackColor = this.colorDialog.Color;
        }

        private void buttonMediaWikiColorTableBack_Click(object sender, EventArgs e)
        {
            this.colorDialog = new ColorDialog();
            this.colorDialog.AllowFullOpen = true;
            this.colorDialog.ShowHelp = true;
            this.colorDialog.Color = this.textBoxMediaWikiColorTableBack.BackColor;

            if (this.colorDialog.ShowDialog() == DialogResult.OK)
                this.textBoxMediaWikiColorTableBack.BackColor = this.colorDialog.Color;
        }

        private void buttonMediaWikiColorTableLine_Click(object sender, EventArgs e)
        {
            this.colorDialog = new ColorDialog();
            this.colorDialog.AllowFullOpen = true;
            this.colorDialog.ShowHelp = true;
            this.colorDialog.Color = this.textBoxMediaWikiColorTableLine.BackColor;

            if (this.colorDialog.ShowDialog() == DialogResult.OK)
                this.textBoxMediaWikiColorTableLine.BackColor = this.colorDialog.Color;
        }

        #region Context for MediaWiki

        private void createMediaWikiColorCodeTable(ref string Wiki, string TableName, System.Collections.Generic.Dictionary<string, bool> ShowDataDict)
        {
            string SQL = "SELECT Code, Description, DisplayText FROM " + TableName + " ORDER BY DisplayOrder";
            System.Data.DataTable dtData = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
            ad.Fill(dtData);

            Wiki += "{|border=" + this.numericUpDownMediaWikiBorder.Value.ToString() +
               " cellpadding=\"" + this.numericUpDownMediaWikiCellpadding.Value.ToString() +
               "\" style=\"margin:1em 1em 1em 0; border:1px #AAA solid; border-collapse:collapse; empty-cells:show;\r\n";

            foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in ShowDataDict)
            {
                if (!KV.Value)
                    continue;
                System.Data.DataRow[] RR = dtData.Select("Code = '" + KV.Key + "'");
                if (RR.Length > 0)
                {
                    string ColorBack = this.userControlColorSettingsMediaWiki.ColorCode(RR[0]["Code"].ToString());
                    string ColorText = this.userControlColorSettingsMediaWiki.ColorCodeText(RR[0]["Code"].ToString());
                    string DisplayText = RR[0]["Code"].ToString();
                    string Description = RR[0]["Description"].ToString();

                    Wiki += "|-style=\"background:" + ColorBack + ";\"\r\n";
                    Wiki += "|" + DisplayText + "\r\n";
                    Wiki += "|" + Description + "\r\n";
                }
            }


            //foreach (System.Data.DataRow R in dtData.Rows)
            //{
            //    string Code = R["Code"].ToString();
            //    switch (Code)
            //    {
            //        case "read_only":
            //            if (this.userControlColorSettingsMediaWiki.HideReadOnly) continue;
            //            break;
            //        case "calculated":
            //            if (this.userControlColorSettingsMediaWiki.HideCalculated) continue;
            //            break;
            //        case "service_link":
            //            if (this.userControlColorSettingsMediaWiki.HideServiceLink) continue;
            //            break;
            //        case "inapplicable":
            //            if (this.userControlColorSettingsMediaWiki.HideInapplicable) continue;
            //            break;
            //    }
            //    Wiki += "|-style=\"background:" + this.ColorCodeMediaWiki(R["Code"].ToString()) + ";\"\r\n";
            //    Wiki += "|" + R["DisplayText"].ToString() + "\r\n";
            //    Wiki += "|" + R["Description"].ToString() + "\r\n";
            //}

            Wiki += "|}\r\n";
        }
        private void createMediaWikiColorCodeTable(ref string Wiki)
        {
            string SQL = "SELECT Code, Description, DisplayText FROM dbo.EntityUsage_Enum ORDER BY DisplayOrder";
            System.Data.DataTable dtUsage = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
            ad.Fill(dtUsage);

            Wiki += "{|border=" + this.numericUpDownMediaWikiBorder.Value.ToString() +
               " cellpadding=\"" + this.numericUpDownMediaWikiCellpadding.Value.ToString() +
               "\" style=\"margin:1em 1em 1em 0; border:1px #AAA solid; border-collapse:collapse; empty-cells:show;\r\n";

            foreach (System.Data.DataRow R in dtUsage.Rows)
            {
                string Code = R["Code"].ToString();
                switch (Code)
                {
                    case "read_only":
                        if (this.userControlColorSettingsMediaWiki.HideReadOnly) continue;
                        break;
                    case "calculated":
                        if (this.userControlColorSettingsMediaWiki.HideCalculated) continue;
                        break;
                    case "service_link":
                        if (this.userControlColorSettingsMediaWiki.HideServiceLink) continue;
                        break;
                    case "inapplicable":
                        if (this.userControlColorSettingsMediaWiki.HideInapplicable) continue;
                        break;
                }
                Wiki += "|-style=\"background:" + this.ColorCodeMediaWiki(R["Code"].ToString()) + ";\"\r\n";
                Wiki += "|" + R["DisplayText"].ToString() + "\r\n";
                Wiki += "|" + R["Description"].ToString() + "\r\n";
            }
            Wiki += "|}\r\n";
        }

        private void comboBoxMediaWikiContext_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxMediaWikiContext.SelectedValue.Equals(System.DBNull.Value))
            {
                //this._useContext = false;
            }
            else
            {
                //this._useContext = true;
                DiversityWorkbench.Settings.Context = this.comboBoxMediaWikiContext.SelectedValue.ToString();
            }
            //this.groupBoxMediaWikiContext.Visible = this._useContext;
        }

        private void setContextForMediaWiki()
        {
            try
            {
                if (DiversityWorkbench.Entity.EntityTablesExistInDatabase && DiversityWorkbench.Settings.UseEntity)
                {
                    if (this._ConnectionString.Length > 0)
                    {
                        string SQL = "SELECT NULL AS Code, NULL AS DisplayText UNION " +
                            "SELECT Code, DisplayText FROM EntityContext_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText";
                        using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_ConnectionString))
                        {
                            con.Open();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
                            System.Data.DataTable dtContext = new DataTable();
                            try
                            {
                                ad.Fill(dtContext);
                                this.comboBoxMediaWikiContext.DataSource = dtContext;
                                this.comboBoxMediaWikiContext.DisplayMember = "DisplayText";
                                this.comboBoxMediaWikiContext.ValueMember = "Code";
                                this.groupBoxMediaWikiContext.Visible = true;
                            }
                            catch
                            {
                                //this.tableLayoutPanelHtmlContext.Visible = false;
                                //this.panelDocuHtml.Height = 46;
                                //this._useContext = false;
                            }
                            con.Close();
                        }
                    }
                }
                else
                {
                    this.groupBoxMediaWikiContext.Visible = false;
                    //this.tableLayoutPanelHtmlContext.Visible = false;
                    //this.panelDocuHtml.Height = 46;
                    //this._useContext = false;
                }

            }
            catch { }
        }

        private string ContextMediaWiki { get { return this.comboBoxMediaWikiContext.SelectedValue.ToString(); } }

        private string ColorCodeMediaWiki(string Usage)
        {
            string ColorCode = "";
            try
            {
                switch (Usage.ToLower())
                {
                    case "inapplicable":
                        ColorCode = "#" + this.userControlColorSettingsMediaWiki.ColorInapplicable.R.ToString("X2") +
                            this.userControlColorSettingsMediaWiki.ColorInapplicable.G.ToString("X2") +
                            this.userControlColorSettingsMediaWiki.ColorInapplicable.B.ToString("X2");
                        break;
                    case "read_only":
                        ColorCode = "#" + this.userControlColorSettingsMediaWiki.ColorReadOnly.R.ToString("X2") +
                            this.userControlColorSettingsMediaWiki.ColorReadOnly.G.ToString("X2") +
                            this.userControlColorSettingsMediaWiki.ColorReadOnly.B.ToString("X2");
                        break;
                    case "calculated":
                        ColorCode = "#" + this.userControlColorSettingsMediaWiki.ColorCalculated.R.ToString("X2") +
                            this.userControlColorSettingsMediaWiki.ColorCalculated.G.ToString("X2") +
                            this.userControlColorSettingsMediaWiki.ColorCalculated.B.ToString("X2");
                        break;
                    case "service_link":
                        ColorCode = "#" + this.userControlColorSettingsMediaWiki.ColorServiceLink.R.ToString("X2") +
                            this.userControlColorSettingsMediaWiki.ColorServiceLink.G.ToString("X2") +
                            this.userControlColorSettingsMediaWiki.ColorServiceLink.B.ToString("X2");
                        break;
                    default:
                        ColorCode = "#" + System.Drawing.Color.White.R.ToString("X2") +
                            System.Drawing.Color.White.G.ToString("X2") +
                            System.Drawing.Color.White.B.ToString("X2");
                        break;
                }

            }
            catch { }
            return ColorCode;
        }

        private void setLanguageForMediaWiki()
        {
            System.Data.DataTable dtLanguage = new DataTable();
            try
            {
                if (dtLanguage.Rows.Count == 0 && DiversityWorkbench.Entity.EntityTablesExist)
                {
                    string SQL = "SELECT NULL AS Code, NULL AS DisplayText UNION SELECT Code, DisplayText FROM EntityLanguageCode_Enum ORDER BY DisplayText";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
                    try
                    {
                        ad.Fill(dtLanguage);
                    }
                    catch { }
                }
                if (dtLanguage.Rows.Count > 0)
                {
                    this.comboBoxMediaWikiContextLanguage.DataSource = dtLanguage;
                    try
                    {
                        this.comboBoxMediaWikiContextLanguage.DisplayMember = "DisplayText";
                        this.comboBoxMediaWikiContextLanguage.ValueMember = "Code";
                    }
                    catch { }
                    for (int i = 0; i < dtLanguage.Rows.Count; i++)
                    {
                        if (dtLanguage.Rows[i][0].ToString() == DiversityWorkbench.Settings.Language)
                        {
                            this.comboBoxMediaWikiContextLanguage.SelectedIndex = i;
                            break;
                        }
                    }
                }

            }
            catch { }
            this._Language = DiversityWorkbench.Settings.Language;

        }

        #endregion

        #endregion

        #region Content

        private void buttonCreateDocuContent_Click(object sender, EventArgs e)
        {
            if (this.DocuContentColumnList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select columns that should be included in the documentation");
                return;
            }
            this.setLanguageContextSource();
            System.Uri u = new Uri(this.createDocuContent(""));
            this.userControlWebViewDocuContent.Url = null;
            this.userControlWebViewDocuContent.Navigate(u);
            //this.webxBrowserDocuContent.Url = u;
        }

        private void setLanguageContextSource()
        {
            if (this.comboBoxDocuContentContextLanguage.DataSource == null)
            {
                try
                {
                    System.Data.DataTable dt = new DataTable();
                    string SQL = "SELECT  NULL AS Code, NULL AS DisplayText " +
                        "UNION SELECT Code, DisplayText " +
                        "FROM  EntityLanguageCode_Enum " +
                        "WHERE (DisplayEnable = 1) " +
                        "ORDER BY DisplayText";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
                    a.Fill(dt);
                    this.comboBoxDocuContentContextLanguage.DataSource = dt;
                    comboBoxDocuContentContextLanguage.DisplayMember = "DisplayText";
                    comboBoxDocuContentContextLanguage.ValueMember = "Code";
                }
                catch { }
            }
        }

        private string createDocuContent(string FileName)
        {
            if (FileName.Length == 0) FileName = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + DiversityWorkbench.Settings.DatabaseName + "_Content.htm";
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(FileName, settings);
            try
            {
                //W.WriteStartDocument();
                W.WriteStartElement("html");
                W.WriteString("\r\n");
                W.WriteStartElement("head");
                W.WriteElementString("title", DiversityWorkbench.Settings.DatabaseName + "_Content");
                W.WriteEndElement();//head
                W.WriteString("\r\n");

                W.WriteStartElement("body");
                W.WriteString("\r\n");

                if (this.checkBoxDocuContentIncludeIndex.Checked)
                    this.createDocuContentIndex(W);
                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    this.createDocuContentTable(W, R[0].ToString());
                }

                W.WriteEndElement();//body
                W.WriteEndElement();//html
                W.WriteEndDocument();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
            return FileName;
        }

        private void createDocuContentIndex(System.Xml.XmlWriter W)
        {
            try
            {
                W.WriteStartElement("FONT");
                W.WriteAttributeString("face", "Verdana");
                W.WriteStartElement("h3");
                W.WriteString("Index");
                W.WriteEndElement();//h3
                W.WriteEndElement();//FONT
                W.WriteStartElement("ul");
                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    this.createDocuHtmlIndexEntry(W, R[0].ToString());
                    W.WriteElementString("br", "");
                    W.WriteString("\r\n");
                }
                W.WriteEndElement();//ul
                W.WriteElementString("br", "");
                W.WriteElementString("br", "");
                W.WriteString("\r\n");
                W.WriteString("\r\n");
            }
            catch { }
        }

        private System.Collections.Generic.List<System.Windows.Forms.ComboBox> DocuContentColumnComboBoxList
        {
            get
            {
                // Markus 23.12.24: fixing bug: ... = new List<System.Windows.Forms.ComboBox>() added missing "System.Windows.Forms."
                System.Collections.Generic.List<System.Windows.Forms.ComboBox> ColumnComboBoxList = new List<System.Windows.Forms.ComboBox>();
                ColumnComboBoxList.Add(this.comboBoxDocuContent1);
                ColumnComboBoxList.Add(this.comboBoxDocuContent2);
                ColumnComboBoxList.Add(this.comboBoxDocuContent3);
                ColumnComboBoxList.Add(this.comboBoxDocuContent4);
                ColumnComboBoxList.Add(this.comboBoxDocuContent5);

                return ColumnComboBoxList;
            }
        }

        private System.Collections.Generic.List<string> DocuContentColumnList
        {
            get
            {
                System.Collections.Generic.List<string> ColumnList = new List<string>();
                foreach (System.Windows.Forms.ComboBox C in this.DocuContentColumnComboBoxList)
                {
                    if (C.SelectedIndex > -1 && C.Text.Length > 0)
                        ColumnList.Add(C.Text);
                }
                return ColumnList;
            }
        }

        private System.Data.DataTable TableColumnsForContent
        {
            get
            {
                string Table = "";
                System.Data.DataTable dt = new DataTable();
                if (this.checkedListBoxDocuTables.CheckedItems.Count == 0)
                    return dt;
                if (this.checkedListBoxDocuTables.CheckedItems[0] != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.checkedListBoxDocuTables.CheckedItems[0];
                    Table = R[0].ToString();
                    string SQL = "SELECT NULL AS ColumnName UNION SELECT COLUMN_NAME as ColumnName FROM Information_Schema.COLUMNS  " +
                        "WHERE Information_Schema.COLUMNS.TABLE_NAME = '" + Table + "'";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
                    try
                    {
                        a.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                    return dt;
                }
                else
                    return dt;
            }
        }

        private void createDocuContentTable(System.Xml.XmlWriter W, string Table)
        {
            try
            {
                System.Data.DataTable dt = this.dtTableDocuContent(Table);

                W.WriteStartElement("FONT");
                W.WriteAttributeString("face", "Verdana");
                W.WriteStartElement("h3");
                W.WriteStartElement("a");
                //W.WriteAttributeString("name", DisplayTextTable);
                W.WriteAttributeString("name", Table);
                if (this.comboBoxDocuContentContextLanguage.SelectedIndex > -1
                    && this.comboBoxDocuContentContextLanguage.Text.StartsWith("D"))
                    W.WriteString("Tabelle ");
                else
                    W.WriteString("Table ");
                W.WriteStartElement("u");
                W.WriteString(Table);
                W.WriteEndElement();//u
                W.WriteEndElement();//a
                W.WriteEndElement();//h3
                W.WriteString("\r\n");
                string TableDescription = "";
                if (this.comboBoxDocuContentContextLanguage.SelectedIndex > -1
                    && this.comboBoxDocuContentContextLanguage.Text.Length > 0)
                    TableDescription = DiversityWorkbench.Entity.EntityInformation(Table, "General", this.comboBoxDocuContentContextLanguage.SelectedValue.ToString())["Description"];
                else
                    TableDescription = this.FormFunctions.TableDescription(Table);
                W.WriteString(TableDescription);
                W.WriteElementString("br", "");
                W.WriteElementString("br", "");
                W.WriteEndElement();//FONT
                W.WriteString("\r\n");

                W.WriteStartElement("table");
                W.WriteAttributeString("cellpadding", "3");
                W.WriteAttributeString("cellspacing", "0");
                W.WriteAttributeString("border", "1");

                // printing the information about the table
                W.WriteStartElement("tr");
                foreach (System.Data.DataColumn CH in dt.Columns)
                {
                    W.WriteStartElement("th");
                    W.WriteAttributeString("nowrap", "nowrap");
                    W.WriteAttributeString("align", "left");
                    W.WriteStartElement("FONT");
                    W.WriteAttributeString("face", "Verdana");
                    string Text = "";
                    if (this.comboBoxDocuContentContextLanguage.SelectedIndex > -1
                        && this.comboBoxDocuContentContextLanguage.Text.Length > 0)
                    {
                        string Context = Table + "." + CH.ColumnName.ToString();
                        Text = DiversityWorkbench.Entity.EntityInformation(Context, "General", this.comboBoxDocuContentContextLanguage.SelectedValue.ToString())["DisplayText"];
                    }
                    else
                        Text = this.FormFunctions.TableDescription(Table);
                    W.WriteString(Text);
                    W.WriteEndElement();//FONT
                    W.WriteEndElement();//th
                    W.WriteString("\r\n");
                }
                W.WriteEndElement();//tr

                // printing the informations for the columns
                int i = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {

                    W.WriteStartElement("tr");
                    foreach (System.Data.DataColumn C in R.Table.Columns)
                    {
                        W.WriteStartElement("td");
                        W.WriteStartElement("FONT");
                        W.WriteAttributeString("face", "Verdana");
                        W.WriteAttributeString("size", "2");
                        string Content = R[C.ColumnName].ToString();
                        if (Content.Length == 0)
                            W.WriteRaw("&nbsp;");
                        else
                            W.WriteString(Content);
                        W.WriteEndElement();//FONT
                        W.WriteEndElement();//td
                        W.WriteString("\r\n");
                    }
                    W.WriteEndElement();//tr
                }
                W.WriteString("\r\n");
                i++;
                //}
                W.WriteEndElement();//table
                W.WriteElementString("br", "");
                W.WriteElementString("br", "");
                W.WriteString("\r\n");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private System.Data.DataTable dtTableDocuContent(string TableName)
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT ";
            if (this.comboBoxDocuContentContextLanguage.SelectedIndex > -1
                && this.comboBoxDocuContentContextLanguage.Text.Length > 0)
            {
                string PK = "";
                System.Data.DataTable dtPK = this.dtPrimaryKey(TableName);
                if (dtPK.Rows.Count != 1)
                    return dt;
                PK = dtPK.Rows[0][0].ToString();
                string Context = TableName + "." + PK + ".";
                System.Collections.Generic.List<string> EntityColumns = this.TableColumns("EntityRepresentation");
                foreach (string s in this.DocuContentColumnList)
                {
                    if (EntityColumns.Contains(s))
                        SQL += "CASE WHEN E." + s + " IS NULL THEN T." + s + " ELSE E." + s + " END AS ";
                    else SQL += "T.";
                    SQL += s + ", ";
                }
                SQL = SQL.Substring(0, SQL.Length - 2);
                SQL += " FROM " + TableName + " AS T LEFT OUTER JOIN EntityRepresentation AS E " +
                    " ON E.Entity = '" + Context + "' + T." + PK + " WHERE (E.LanguageCode = '" + this.comboBoxDocuContentContextLanguage.SelectedValue.ToString() + "' " +
                    " OR E.LanguageCode IS NULL) ";
                if (this.comboBoxDocuContentRestrictionColumn.SelectedIndex > -1
                    && this.comboBoxDocuContentRestrictionColumn.Text.Length > 0
                    && this.textBoxDocuContentRestrictionValue.Text.Length > 0)
                    SQL += " AND T." + this.comboBoxDocuContentRestrictionColumn.Text + " = '" + this.textBoxDocuContentRestrictionValue.Text + "'";
                if (this.comboBoxDocuContentOrder.SelectedIndex > -1
                    && this.comboBoxDocuContentOrder.Text.Length > 0)
                {
                    SQL += " ORDER BY ";
                    if (EntityColumns.Contains(this.comboBoxDocuContentOrder.Text))
                        SQL += "E.";
                    else SQL += "T.";
                    SQL += this.comboBoxDocuContentOrder.Text;
                }
            }
            else
            {
                foreach (string s in this.DocuContentColumnList)
                {
                    SQL += "T." + s + ", ";
                }
                SQL = SQL.Substring(0, SQL.Length - 2);
                SQL += " FROM " + TableName + " AS T ";
                if (this.comboBoxDocuContentRestrictionColumn.SelectedIndex > -1
                    && this.comboBoxDocuContentRestrictionColumn.Text.Length > 0
                    && this.textBoxDocuContentRestrictionValue.Text.Length > 0)
                    SQL += " WHERE T." + this.comboBoxDocuContentRestrictionColumn.Text + " = '" + this.textBoxDocuContentRestrictionValue.Text + "'";
                if (this.comboBoxDocuContentOrder.SelectedIndex > -1
                    && this.comboBoxDocuContentOrder.Text.Length > 0)
                    SQL += " ORDER BY T." + this.comboBoxDocuContentOrder.Text;
            }
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
            try
            {
                a.Fill(dt);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return dt;
        }

        private void setContentAccordingToContextLanguage(ref System.Data.DataTable DT, string TableName)
        {
            string PK = "";
            System.Data.DataTable dtPK = this.dtPrimaryKey(TableName);
            if (dtPK.Rows.Count != 1)
                return;
            PK = dtPK.Rows[0][0].ToString();
            if (this.comboBoxDocuContentContextLanguage.SelectedIndex > -1
                && this.comboBoxDocuContentContextLanguage.Text.Length > 0)
            {
                try
                {
                    foreach (System.Data.DataRow R in DT.Rows)
                    {
                        foreach (System.Data.DataColumn C in DT.Columns)
                        {
                            string Context = TableName + "." + PK + ".";
                        }
                    }
                }
                catch { }
            }
        }

        private void buttonDocuContentGetColumns_Click(object sender, EventArgs e)
        {
            this._CurrentTable = null;
            if (this.checkedListBoxDocuTables.CheckedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select the tables that should be included in the documentation");
                return;
            }
            foreach (System.Windows.Forms.ComboBox C in this.DocuContentColumnComboBoxList)
            {
                System.Data.DataTable dt = this.TableColumnsForContent.Copy();
                C.DataSource = dt;
                C.DisplayMember = "ColumnName";
                C.ValueMember = "ColumnName";
            }
            System.Data.DataTable dtO = this.TableColumnsForContent.Copy();
            this.comboBoxDocuContentOrder.DataSource = dtO;
            this.comboBoxDocuContentOrder.DisplayMember = "ColumnName";
            this.comboBoxDocuContentOrder.ValueMember = "ColumnName";

            System.Data.DataTable dtR = this.TableColumnsForContent.Copy();
            comboBoxDocuContentRestrictionColumn.DataSource = dtR;
            comboBoxDocuContentRestrictionColumn.DisplayMember = "ColumnName";
            comboBoxDocuContentRestrictionColumn.ValueMember = "ColumnName";

        }

        #endregion

        #region Semantic Media Wiki

        private string _SMWikiCollection =
            "{{{{Concept collection\r\n" +
            "|concept scheme={0}\r\n" +
            "|description={1}\r\n" +
            "}}}}\r\n" +
            "[[Category:{0}]]\r\n";

        private string _SMWikiConcept =
            "{{{{Concept\r\n" +
            "|label={3}\r\n" +
            "|definition={4}\r\n" +
            "|concept type=class\r\n" +
            "}}}}\r\n" +
            "{{{{Concept scheme relation\r\n" +
            "|scheme={0}\r\n" +
            "}}}}\r\n" +
            "{{{{Concept relation\r\n" +
            "|relation=skos: collection\r\n" +
            "|internal page={1}:{2}\r\n" +
            "}}}}\r\n" +
            "{{{{dwb_concept\r\n" +
            "|type={5}\r\n" +
            "|required={6}\r\n" +
            "|indices={7}\r\n" +
            "}}}}\r\n";

        private string _SMWikiRelation =
            "{{{{dwb_relation\r\n" +
            "|referred={0}:{1}.{2}\r\n" +
            "|update={3}\r\n" +
            "|delete={4}\r\n" +
            "}}}}\r\n";

        private DateTime _SMWikiTimestamp;

        private void buttonSMWikiCreateDocu_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.comboBoxSMWikiTarget.SelectedIndex < 0)
                {
                    MessageBox.Show("Please select a target", "Select value");
                    return;
                }
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.createSMWiki();
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void createSMWiki()
        {
            try
            {
                int i = 0;
                // local parameter
                Dictionary<string, string> WikiPages = new Dictionary<string, string>();
                string Prefix = this.textBoxSMWikiPrefix.Text;
                string Scheme = this.textBoxSMWikiScheme.Text;
                List<string> SuppressColumns = null;
                if (this.checkBoxSMWikiSuppressColumns.Checked && this.textBoxSMWikiSuppressColumns.Text.Length > 0)
                    SuppressColumns = this.textBoxSMWikiSuppressColumns.Text.Split((new Char[] { '|' })).ToList();
                _SMWikiTimestamp = DateTime.Now;

                foreach (System.Data.DataRowView R in this.checkedListBoxDocuTables.CheckedItems)
                {
                    if (R["ObjectType"].ToString() == "TABLE")
                    {
                        this.createSMWikiPages(WikiPages, Scheme, Prefix, R[0].ToString(), SuppressColumns);
                        i++;
                    }
                }

                if (_Connection != null)
                    this._Connection.Disconnect();

                // build output text
                StringBuilder sb = new StringBuilder();
                switch (this.comboBoxSMWikiTarget.SelectedIndex)
                {
                    case 0: // Plain text
                        foreach (var item in WikiPages)
                        {
                            sb.AppendLine("<!--" + item.Key + "-->");
                            sb.AppendLine(item.Value);
                            //wiki.AppendLine();
                        }
                        this.textBoxSMWikiDocu.Text = sb.ToString();
                        break;
                    case 1: // XML
                        // prepare media wiki structures
                        MediaWiki_0_3.MediaWikiType mw = new MediaWiki_0_3.MediaWikiType();
                        mw.lang = "en";
                        mw.version = "0.3";

                        // convert pages dictionary to typed list
                        List<MediaWiki_0_3.PageType> pageList = new List<MediaWiki_0_3.PageType>();
                        foreach (var item in WikiPages)
                        {
                            MediaWiki_0_3.RevisionType[] conceptText = { new MediaWiki_0_3.RevisionType() };
                            conceptText[0].timestamp = _SMWikiTimestamp;
                            conceptText[0].text = new MediaWiki_0_3.TextType();
                            conceptText[0].text.Value = item.Value;

                            MediaWiki_0_3.PageType conceptPage = new MediaWiki_0_3.PageType();
                            conceptPage.title = item.Key;
                            conceptPage.Items = conceptText;
                            pageList.Add(conceptPage);
                        }
                        mw.page = pageList.ToArray();
                        using (System.IO.TextWriter writer = new System.IO.StringWriter(sb))
                        {
                            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(MediaWiki_0_3.MediaWikiType));
                            serializer.Serialize(writer, mw);
                        }
                        this.textBoxSMWikiDocu.Text = sb.ToString();
                        break;
                    default:
                        break;
                }
                if (i > 0)
                {
                    System.Windows.Forms.MessageBox.Show("Description for " + i.ToString() + " tables generated");
                }
                else
                    System.Windows.Forms.MessageBox.Show("No table has been selected");
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool createSMWikiPages(Dictionary<string, string> WikiPages, string Scheme, string Prefix, string Table, List<string> SuppressColumns)
        {
            try
            {
                string title = "";
                string wiki = "";

                // get relations and table data
                System.Data.DataTable dtc = this.dtTableDocuConstraints(Table);
                System.Data.DataTable dt = this.dtTableDocumentation(Table, true, true);

                foreach (System.Data.DataRow R in dt.Rows)
                {
                    // Save column name
                    string Column = R["ColumnName"].ToString();

                    // if a column with a certain name pattern should not be printed
                    bool Suppress = false;
                    if (SuppressColumns != null)
                    {
                        for (int i = 0; i < SuppressColumns.Count; i++)
                        {
                            if (Column.StartsWith(SuppressColumns[i]))
                            {
                                Suppress = true;
                                break;
                            }
                        }
                    }
                    if (!Suppress)
                    {
                        // the label
                        string Label = /*Table + "." +*/
        Column;
                        bool IsPK = false;
                        System.Data.DataTable dtPK = this.dtPrimaryKey(Table);
                        foreach (System.Data.DataRow PK in dtPK.Rows)
                        {
                            if (PK[0].ToString() == Column)
                            {
                                IsPK = true;
                                break;
                            }
                        }
                        if (IsPK)
                        {
                            if (this.radioButtonSMWikiUnderline.Checked)
                                Label = "<u>" + Label + "</u>";
                            else if (this.radioButtonSMWikiItalic.Checked)
                                Label = "<i>" + Label + "</i>";
                        }

                        // the description
                        string Description = this.FormFunctions.ColumnDescription(Table, Column);
                        if (!R["DefaultValue"].Equals(System.DBNull.Value))
                        {
                            string Default = R["DefaultValue"].ToString();
                            if (Default.StartsWith("(")) Default = Default.Substring(1);
                            if (Default.EndsWith(")")) Default = Default.Substring(0, Default.Length - 1);
                            if (Default == "NULL") Default = "";
                            if (Default == "''") Default = "";
                            if (Default.Length > 0) Description += "<br />''DefaultValue: " + Default + " ''";
                        }
                        R["Description"] = Description;

                        // the type
                        string DataType = R["DataType"].ToString();
                        if (!R["Length"].Equals(System.DBNull.Value))
                        {
                            if (R["Length"].ToString() != "-1")
                                DataType += " (" + R["Length"].ToString() + ")";
                            else if (DataType != "geography")
                                DataType += " (MAX)";
                        }

                        // Insert concept
                        title = Prefix + ":" + Table + "." + Column;

                        // the concept body
                        wiki = string.Format(_SMWikiConcept, Scheme, Prefix, Table, Label, Description, DataType, R["Required"], R["Indices"]);

                        foreach (System.Data.DataRow CR in dtc.Rows)
                        {
                            // skip rows that do not contain foreightn keys
                            if (CR["CONSTRAINT_TYPE"].ToString() != "FOREIGN KEY" || Column != CR["ColumnsConstraint"].ToString())
                                continue;

                            // if a column with a certain name pattern should not be printed
                            Suppress = false;
                            if (SuppressColumns != null)
                            {
                                string y = CR["ColumnsUnique"].ToString();
                                for (int i = 0; i < SuppressColumns.Count; i++)
                                {
                                    if (y.StartsWith(SuppressColumns[i]))
                                    {
                                        Suppress = true;
                                        break;
                                    }
                                }
                            }
                            if (!Suppress)
                            {
                                // the relation template
                                wiki += string.Format(_SMWikiRelation, Prefix, CR["TABLE_NAME"], CR["ColumnsUnique"], CR["UPDATE_RULE"], CR["DELETE_RULE"]);
                            }
                        }

                        // Append concept to pages
                        WikiPages.Add(title, wiki);
                    }
                }

                // Insert collection
                title = Prefix + ":" + Table;
                wiki = string.Format(_SMWikiCollection, Scheme, this.FormFunctions.TableDescription(Table));

                // Append collection to pages
                WikiPages.Add(title, wiki);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                return false;
            }
            return true;
        }
        #endregion

        #region XML
        private void buttonCreateXML_Click(object sender, EventArgs e)
        {

        }

        private string createDocuXML(string FileName)
        {
            if (this.checkedListBoxDocuTables.CheckedItems.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select at least one table");
                return "";
            }

            //this._CheckedItems = null;

            if (FileName.Length == 0) FileName = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Documentation) + DiversityWorkbench.Settings.DatabaseName + ".xml";
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(FileName, settings);
            try
            {
                W.WriteStartDocument();
                W.WriteStartElement("Database");
                string Module = "";
                W.WriteStartElement("Report");
                string Date = System.DateTime.Now.Year.ToString() + "/" + System.DateTime.Now.Month.ToString() + "/" + System.DateTime.Now.Day.ToString();
                W.WriteElementString("Date", Date);
                W.WriteEndElement(); // Report
                W.WriteFullEndElement();  // LabelPrint
                W.WriteEndDocument();
                W.Flush();
                W.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
            return FileName;
        }



        #endregion

        #region chm

        #region common
        private System.IO.DirectoryInfo _ChmDirectory;
        private System.IO.FileInfo _ChmHhkFile;
        private System.IO.FileInfo _ChmHhcFile;
        private System.IO.FileInfo _ChmIndexFile;
        private System.IO.FileInfo _ChmKeywordFile;
        private System.IO.FileInfo _ChmIndexMarkdown;
        private System.IO.FileInfo _ChmKeywordMarkdown;
        //private System.IO.FileInfo _CssFile;
        private System.IO.FileInfo _SvgLogoFile;


        private void buttonChmFile_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Filter = "hhc files|*.hhc";
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0) // && this.openFileDialog.FileName != _ChmHhcFile.FullName)
                {
                    try
                    {
                        //System.IO.FileInfo ChmFile = new System.IO.FileInfo(this.openFileDialog.FileName);
                        _ChmHhcFile = new System.IO.FileInfo(this.openFileDialog.FileName);
                        _ChmIndexFile = new System.IO.FileInfo(_ChmHhcFile.Directory + "\\index.html");
                        this.textBoxChmIndexFile.Text = _ChmIndexFile.Name;
                        this._ChmDirectory = new System.IO.DirectoryInfo(_ChmHhcFile.DirectoryName);
                        this.labelChmFolder.Text = this._ChmDirectory.FullName;
                        this.initChm();
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void initChm()
        {
            try
            {
                foreach (System.IO.FileInfo file in this._ChmDirectory.GetFiles())
                {
                    if (file.Extension == ".hhk")
                    {
                        _ChmHhkFile = file;
                        _ChmKeywordFile = new System.IO.FileInfo(file.Directory + "\\keywords.html");
                        _ChmIndexMarkdown = new System.IO.FileInfo(file.Directory + "\\index.md");
                        _ChmKeywordMarkdown = new System.IO.FileInfo(file.Directory + "\\keywords.md");
                        this.textBoxChmKeywordFile.Text = _ChmKeywordFile.Name;
                    }
                }
                if (DiversityWorkbench.Forms.FormDocumentationSettings.Default.DokuCSS != null && DiversityWorkbench.Forms.FormDocumentationSettings.Default.DokuCSS.Count > 0)
                {
                    foreach (string css in DiversityWorkbench.Forms.FormDocumentationSettings.Default.DokuCSS)
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(css);
                        if (file.Exists)
                        {
                            if (_ChmCssFiles == null)
                                _ChmCssFiles = new Dictionary<string, System.IO.FileInfo>();
                            if (!_ChmCssFiles.ContainsKey(file.Name))
                                this._ChmCssFiles.Add(file.Name, file);
                        }
                    }
                    this.setDefaultCSS(ref this._ChmCssFiles);
                }
                else
                {
                    System.Collections.Generic.Dictionary<string, System.IO.FileInfo> CssDict = new Dictionary<string, System.IO.FileInfo>();
                    this.setDefaultCSS(ref CssDict);
                    if (DiversityWorkbench.Forms.FormDocumentationSettings.Default.DokuCSS == null)
                        DiversityWorkbench.Forms.FormDocumentationSettings.Default.DokuCSS = new System.Collections.Specialized.StringCollection();
                    foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in CssDict)
                    {
                        DiversityWorkbench.Forms.FormDocumentationSettings.Default.DokuCSS.Add(KV.Value.FullName);
                        System.IO.FileInfo file = new System.IO.FileInfo(KV.Value.FullName);
                        if (file.Exists)
                        {
                            if (_ChmCssFiles == null)
                                _ChmCssFiles = new Dictionary<string, System.IO.FileInfo>();
                            if (!_ChmCssFiles.ContainsKey(file.Name))
                                this._ChmCssFiles.Add(file.Name, file);
                        }
                    }
                }
                this.initChmCssList();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #region html
        private void buttonChmIndexFile_Click(object sender, EventArgs e)
        {
            this.ChmGenerateFile(_ChmHhcFile, _ChmIndexFile, ".hhc", "Content", this.userControlWebViewChmIndexFile);
            return;

            try
            {
                if (_ChmHhcFile != null)
                {
                    string Directory = "";
                    string Header = _ChmHhcFile.Name.Replace(".hhc", "");
                    if (Header == "Content")
                    {
                        Header = _ChmHhcFile.Directory.Name;
                        if (Header.EndsWith("Help"))
                        {
                            Directory = Header;
                            Header = Header.Substring(0, Header.Length - 4);
                        }
                    }
                    System.IO.StreamReader sr = new System.IO.StreamReader(_ChmHhcFile.FullName, true);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(_ChmIndexFile.FullName, false, System.Text.Encoding.UTF8);
                    this.ChmGenerateFile(sr, sw, Header);
                    this.userControlWebViewChmIndexFile.Url = null;
                    this.userControlWebViewChmIndexFile.Navigate(_ChmIndexFile.FullName);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonChmKeywordFile_Click(object sender, EventArgs e)
        {
            this.ChmGenerateFile(_ChmHhkFile, _ChmKeywordFile, ".hhk", "Index", this.userControlWebViewChmKeywordFile);
            return;

            /*
            try
            {
                if (_ChmHhkFile != null)
                {
                    string Directory = "";
                    string Header = _ChmHhkFile.Name.Replace(".hhk", "");
                    if (Header == "Index")
                    {
                        Header = _ChmHhkFile.Directory.Name;
                        if (Header.EndsWith("Help"))
                        {
                            Directory = Header;
                            Header = Header.Substring(0, Header.Length - 4);
                        }
                    }
                    Header += ": Keywords";
                    System.IO.StreamReader sr = new System.IO.StreamReader(_ChmHhkFile.FullName, true);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(_ChmKeywordFile.FullName, false, System.Text.Encoding.UTF8);
                    this.ChmGenerateFile(sr, sw, Header, Directory);
                    this.userControlWebViewChmKeywordFile.Url = null;
                    this.userControlWebViewChmKeywordFile.Navigate(_ChmKeywordFile.FullName);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            */
        }

        private void ChmGenerateFile(System.IO.FileInfo SourceFile, System.IO.FileInfo TargetFile, string fileExtension, string Replacement, DiversityWorkbench.UserControls.UserControlWebView webView)
        {
            try
            {
                if (SourceFile != null)
                {
                    string Directory = "";
                    string Header = SourceFile.Name.Replace(fileExtension, "");
                    if (Header == Replacement)
                    {
                        Header = SourceFile.Directory.Name;
                        if (Header.EndsWith("Help"))
                        {
                            Directory = Header;
                            Header = Header.Substring(0, Header.Length - 4);
                        }
                    }
                    System.IO.StreamReader sr = new System.IO.StreamReader(SourceFile.FullName, true);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(TargetFile.FullName, false, System.Text.Encoding.UTF8);
                    this.ChmGenerateFile(sr, sw, Header, Directory);
                    webView.Url = null;
                    webView.Navigate(TargetFile.FullName);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void ChmGenerateFile(System.IO.StreamReader sr, System.IO.StreamWriter sw, string Header, string Directory = "")
        {
            try
            {
                using (sr)
                {
                    string line = "";
                    string Link = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line == "<OBJECT type=\"text/site properties\">" ||
                            line == "\t<param name=\"ImageType\" value=\"Folder\">" ||
                            line.IndexOf("</OBJECT>") > -1)
                            continue;

                        if (line.IndexOf("<OBJECT type=\"text/sitemap\">") > -1)
                            line = line.Replace("<OBJECT type=\"text/sitemap\">", "");
                        if (line.IndexOf("</HEAD><BODY>") > -1)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> CSS in this._ChmCssFiles)
                            {
                                sw.WriteLine("<link rel=\"stylesheet\" href=\"" + CSS.Key + "\">");
                            }
                            sw.WriteLine("<link rel=\"icon\" type=\"image/svg+xml\" href=\"favicon.svg\" sizes=\"any\">");
                            sw.WriteLine("<link rel=\"icon\" type=\"image/png\" href=\"favicon.png\">");
                            sw.WriteLine("<meta name=\"database\" content=\"" + DiversityWorkbench.Settings.DatabaseName + "\">");
                            sw.WriteLine("<meta name=\"version\" content=\"" + this.DatabaseVersion() + "\">");
                            sw.WriteLine("<meta name=\"clientversion\" content=\"" + System.Windows.Forms.Application.ProductVersion.ToString() + "\">");
                            sw.WriteLine("<meta name=\"creation\" content=\"" + System.DateTime.Now.ToString("yyyy-MM-dd") + "\">");
                            sw.WriteLine("<meta name=\"creator\" content=\"" + System.Environment.UserName + "\">");
                            line += "\r\n<H1>" + Header + "</H1>";
                            if (this.LogoFile().Length > 0)
                                line += "\r\n<img src=\"" + this.LogoFile() + "\" align=\"right\" >";
                        }
                        if (line.IndexOf("</BODY>") > -1)
                        {
                            //line = "</FONT>\r\n" + line;
                        }
                        if (line.IndexOf("<param name=\"Name\" value=\"") > -1)
                        {
                            if (Link.Length > 0)
                                continue;
                            line = line.Replace("<param name=\"Name\" value=\"", "");
                            if (line.IndexOf("\t") > -1)
                            {
                                line = line.Substring(0, line.LastIndexOf("\t") + 1) + "<a # >" + line.Substring(line.LastIndexOf("\t") + 1);
                                line = line.Replace("\">", "");
                                Link = line;
                                continue;
                            }
                        }
                        if (line.IndexOf("<param name=\"Local\" value=\"") > -1)
                        {
                            line = line.Replace("<param name=\"Local\" value=\"", "");
                            if (line.IndexOf("\t") > -1)
                            {
                                line = line.Substring(line.LastIndexOf("\t") + 1);
                                line = line.Replace("\">", "");
                                Link = Link.Replace("#", "href=\"" + line + "\"") + "</a></LI>";
                            }
                        }
                        if (line.IndexOf("<UL>") > -1 && Link.Length > 0)
                        {
                            // headers without links
                            Link = Link.Replace("<a # >", "");
                            sw.WriteLine(Link);
                            Link = "";
                        }
                        //sw.WriteLine();
                        if (Link.Length > 0)
                        {
                            if (Directory.Length > 0 && Link.IndexOf("\"" + Directory + "\\") > -1)
                                Link = Link.Replace("\"" + Directory + "\\", "\"");
                            sw.WriteLine(Link);
                            Link = "";
                        }
                        else
                            sw.WriteLine(line);
                    }
                    sr.Dispose();
                    sw.Close();
                    sw.Dispose();
                    this.userControlWebViewChmIndexFile.Url = null;
                    this.userControlWebViewChmIndexFile.Navigate(_ChmIndexFile.FullName);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region CSS

        private System.Collections.Generic.Dictionary<string, System.IO.FileInfo> _ChmCssFiles;


        private void initChmCssList()
        {
            try
            {
                this.listBoxChmCss.Items.Clear();
                if (_ChmCssFiles != null)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in this._ChmCssFiles)
                    {
                        this.listBoxChmCss.Items.Add(KV.Key);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        private void toolStripButtonChmCssAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Filter = "css files|*.css";
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    try
                    {
                        if (this._ChmCssFiles == null)
                            this._ChmCssFiles = new Dictionary<string, System.IO.FileInfo>();
                        System.IO.FileInfo css = new System.IO.FileInfo(this.openFileDialog.FileName);
                        this._ChmCssFiles.Add(css.Name, css);
                        this.setDefaultCSS(ref this._ChmCssFiles);
                        this.initChmCssList();
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        }

        private void toolStripButtonChmCssRemove_Click(object sender, EventArgs e)
        {
            if (this._ChmCssFiles.ContainsKey(this.listBoxHtmlCss.SelectedItem.ToString()))
            {
                this._ChmCssFiles.Remove(this.listBoxChmCss.SelectedItem.ToString());
                this.setDefaultCSS(ref this._ChmCssFiles);
                this.initChmCssList();
            }

        }

        #endregion

        #region Logo

        private string LogoFile()
        {
            string file = "";
            if (_ChmDirectory != null)
            {
                file = _ChmDirectory.FullName + "\\img\\Logo.svg";
                System.IO.FileInfo info = new System.IO.FileInfo(file);
                if (!info.Exists)
                    file = "";
                else
                    file = "img/" + info.Name;
            }
            return file;
        }

        #endregion

        #endregion

        #region markdown

        private void buttonChmIndexMarkdown_Click(object sender, EventArgs e)
        {
            try
            {
                if (_ChmHhcFile != null)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(_ChmHhcFile.FullName, true);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(_ChmIndexMarkdown.FullName, false, System.Text.Encoding.UTF8);
                    this.ChmGenerateMarkdown(sr, sw, _ChmHhcFile.Name.Replace(".hhc", "") + ": Content");
                    this.userControlWebViewChmIndexFile.Url = null;
                    this.userControlWebViewChmIndexFile.Navigate(_ChmIndexMarkdown.FullName);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonChmKeywordMarkdown_Click(object sender, EventArgs e)
        {
            try
            {
                if (_ChmHhkFile != null)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(_ChmHhkFile.FullName, true);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(_ChmKeywordMarkdown.FullName, false, System.Text.Encoding.UTF8);
                    this.ChmGenerateMarkdown(sr, sw, _ChmHhkFile.Name.Replace(".hhk", "") + ": Keywords");
                    this.userControlWebViewChmKeywordFile.Url = null;
                    this.userControlWebViewChmKeywordFile.Navigate(_ChmKeywordMarkdown.FullName);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void ChmGenerateMarkdown(System.IO.StreamReader sr, System.IO.StreamWriter sw, string Header)
        {
            try
            {
                using (sr)
                {
                    string line = "";
                    string Link = "";
                    string H = "";
                    string Prefix = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("<!DOCTYPE HTML PUBLIC \")") ||
                            line.StartsWith("<HTML>") ||
                            line.StartsWith("<HEAD>") ||
                            line.StartsWith("<meta name=") ||
                            line.StartsWith("<!--") ||
                            //line.StartsWith("</HEAD>") ||
                            line.StartsWith("</OBJECT>") ||
                            line.StartsWith("\t<LI> <OBJECT ") ||
                            line.StartsWith("<HEAD>") ||
                            line.StartsWith("<HEAD>") ||
                            line.StartsWith("<HEAD>") ||
                            line == "<OBJECT type=\"text/site properties\">" ||
                            line == "\t<param name=\"ImageType\" value=\"Folder\">" ||
                            line.IndexOf("</OBJECT>") > -1)
                            continue;

                        if (line.IndexOf("</HEAD><BODY>") > -1 && Header.Length > 0)
                        {
                            sw.WriteLine("# " + Header);
                        }

                        // handling of UL
                        if (line.IndexOf("<UL>") > -1)
                        {
                            // Header without site
                            if (Link.Length > 0)
                            {
                                sw.WriteLine(Prefix + Link);
                                Link = "";
                            }

                            if (Prefix.Length > 0) Prefix = "  " + Prefix;
                            else Prefix = "- ";
                            //if (Link.Length > 0) Prefix = "  " + Prefix;
                        }
                        if (line.IndexOf("</UL>") > -1)
                        {
                            if (Prefix.Length > 0) Prefix = Prefix.Substring(2);
                        }

                        if (line.IndexOf("<H1") > -1)
                        {
                            H = "# ";
                        }
                        if (line.IndexOf("<param name=\"Name\" value=\"") > -1)
                        {
                            if (Link.Length > 0)
                                continue;
                            line = line.Replace("<param name=\"Name\" value=\"", "");
                            if (line.IndexOf("\t") > -1)
                            {
                                line = line.Substring(line.LastIndexOf("\t") + 1);
                                line = line.Replace("\">", "");
                                Link = line;
                                continue;
                            }
                        }
                        if (line.IndexOf("<param name=\"Local\" value=\"") > -1)
                        {
                            line = line.Replace("<param name=\"Local\" value=\"", "");
                            if (line.IndexOf("\t") > -1)
                            {
                                line = line.Substring(line.LastIndexOf("\t") + 1);
                            }
                            line = line.Replace("\">", "");
                            Link = "[" + Link + "](" + line + ")";
                        }

                        if (Link.Length > 0)
                        {
                            sw.WriteLine(Prefix + Link);
                            Link = "";
                        }
                    }
                    sr.Dispose();
                    sw.Close();
                    sw.Dispose();
                    this.userControlWebViewChmIndexFile.Url = null;
                    this.userControlWebViewChmIndexFile.Navigate(_ChmIndexFile.FullName);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        // Markus 23.12.24: Generation of a textfile for the hugo manual
        #region HUGO - Keyword txt file

        private void initChmHugoKeywordPrefixes()
        {
            string Module = "";
            if (ChmHugoKeywordFile.Directory.Parent.Name.IndexOf("_") > -1)
                Module = ChmHugoKeywordFile.Directory.Parent.Name.Substring(0, ChmHugoKeywordFile.Directory.Parent.Name.IndexOf("_"));
            else
                switch (ChmHugoKeywordFile.Directory.Name)
                {
                    case "DiversityGazetteerHelp":
                        Module = "DiversityGazetteer";
                        break;
                    case "DiversitySamplingPlotsHelp":
                        Module = "DiversitySamplingPlots";
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("Please enter a valid module prefix");
                        break;
                }
            if (this.textBoxChmHugoKeywordPrefix.Text.Length == 0 || this.textBoxChmHugoKeywordPrefix.Text.IndexOf("*") > -1)
            {
                this.textBoxChmHugoKeywordPrefix.Text = this.HugoModuleAcronym(Module) + "_";
                this.textBoxChmHugoKeywordPostfix.Text = "_" + this.HugoModuleAcronym(Module);
                this.textBoxChmHugoKeywordFilePostfix.Text = "_" + this.HugoModuleAcronym(Module);
                //switch (Module)
                //{
                //    case "DiversityAgents":
                //        this.textBoxChmHugoKeywordPrefix.Text = "DA_";
                //        this.textBoxChmHugoKeywordPostfix.Text = "_DA";
                //        break;
                //    case "DiversityCollection":
                //        this.textBoxChmHugoKeywordPrefix.Text = "DC_";
                //        break;
                //    default:
                //        System.Windows.Forms.MessageBox.Show("Please enter a valid module prefix");
                //        break;
                //}
            }
            if (this.textBoxChmHugoKeywordFilePrefix.Text.Length == 0 || this.textBoxChmHugoKeywordFilePrefix.Text.IndexOf("*") > -1)
            {
                this.textBoxChmHugoKeywordFilePrefix.Text = "modules/" + Module.ToLower() + "/";
                //switch (Module)
                //{
                //    case "DiversityAgents":
                //        this.textBoxChmHugoKeywordFilePrefix.Text = "modules/diversityagents/";
                //        break;
                //    case "DiversityCollection":
                //        this.textBoxChmHugoKeywordFilePrefix.Text = "modules/diversitycollection/";
                //        break;
                //    default:
                //        System.Windows.Forms.MessageBox.Show("Please enter a valid prefix for the linked chapters");
                //        break;
                //}
            }
        }

        private string HugoModuleAcronym(string Module)
        {
            string Acronym = "D";
            string EditString = Module.Replace("Diversity", "");
            if (string.IsNullOrEmpty(EditString)) { Acronym = "D"; }
            else { Acronym += EditString.Substring(0, 1).ToUpper(); }
            switch (EditString.ToLower())
            {
                case "samplingplots":
                    Acronym = "DSP";
                    break;
                case "scientificterms":
                    Acronym = "DST";
                    break;
                case "taxonnames":
                    Acronym = "DTN";
                    break;
            }
            return Acronym;
        }

        private System.IO.FileInfo _ChmHugoKeywordFile = null;

        private System.IO.FileInfo ChmHugoKeywordFile
        {
            get
            {
                if (_ChmHugoKeywordFile == null && _ChmKeywordFile != null)
                {
                    _ChmHugoKeywordFile = new FileInfo(_ChmKeywordFile.FullName.Replace(".html", ".txt"));
                }
                return _ChmHugoKeywordFile;
            }
        }

        private bool readyToCreateHugoKeywordFile()
        {
            if (ChmHugoKeywordFile == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a source file");
                return false;
            }
            else
            {
                this.initChmHugoKeywordPrefixes();
            }
            if (this.textBoxChmHugoKeywordPrefix.Text.Length == 0 || this.textBoxChmHugoKeywordPrefix.Text.IndexOf("*") > -1)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid module prefix");
                return false;
            }
            if (this.textBoxChmHugoKeywordFilePrefix.Text.Length == 0 || this.textBoxChmHugoKeywordFilePrefix.Text.IndexOf("*") > -1)
            {
                System.Windows.Forms.MessageBox.Show("Please enter a valid prefix for the linked chapters");
                return false;
            }

            return true;
        }

        private void buttonChmHugoKeywordFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (readyToCreateHugoKeywordFile())
                    this.GenerateHugoKeywordFile();
                else { System.Windows.Forms.MessageBox.Show("Please select a source file"); }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); return; }
        }

        private void GenerateHugoKeywordFile()
        {
            try
            {
                if (_ChmHhkFile != null)
                {
                    string Directory = "";
                    string Header = _ChmHhkFile.Name.Replace(".hhk", "");
                    if (Header == "Index")
                    {
                        Header = _ChmHhkFile.Directory.Name;
                        if (Header.EndsWith("Help"))
                        {
                            Directory = Header;
                            Header = Header.Substring(0, Header.Length - 4);
                        }
                    }
                    System.IO.StreamReader sr = new System.IO.StreamReader(_ChmHhkFile.FullName, true);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ChmHugoKeywordFile.FullName, false, System.Text.Encoding.UTF8);

                    using (sr)
                    {
                        string line = "";
                        string Key = "";
                        string Link = "";

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line == "<OBJECT type=\"text/site properties\">" ||
                                line == "\t<param name=\"ImageType\" value=\"Folder\">" ||
                                line.IndexOf("</OBJECT>") > -1 ||
                                line.IndexOf("<OBJECT type=\"text/sitemap\">") > -1 ||
                                line.IndexOf("</HEAD><BODY>") > -1 ||
                                line.IndexOf("</BODY>") > -1)
                                continue;

                            if (line.IndexOf("<param name=\"Name\" value=\"") > -1)
                            {
                                //if (Link.Length > 0)
                                //    continue;
                                line = sr.ReadLine(); // Reading the next line containing the key
                                line = line.Replace("<param name=\"Name\" value=\"", "");
                                if (line.IndexOf("\t") > -1)
                                {
                                    line = line.Substring(0, line.LastIndexOf("\t") + 1) + line.Substring(line.LastIndexOf("\t") + 1);
                                    line = line.Replace("\">", "");
                                    line = line.Substring(line.LastIndexOf("\t") + 1);
                                    if (this.radioButtonChmHugoKeywordPrefix.Checked)
                                        Key = this.textBoxChmHugoKeywordPrefix.Text + line;// + this.textBoxChmHugoKeywordPostfix;
                                    else
                                        Key = line + this.textBoxChmHugoKeywordPostfix.Text;// + this.textBoxChmHugoKeywordPostfix;
                                    continue;
                                }
                            }
                            if (line.IndexOf("<param name=\"Local\" value=\"") > -1)
                            {
                                line = line.Replace("<param name=\"Local\" value=\"", "");
                                if (line.IndexOf("\t") > -1)
                                {
                                    line = line.Substring(line.LastIndexOf("\t") + 1);
                                    line = line.Substring(0, line.LastIndexOf(".htm"));
                                    Link = this.textBoxChmHugoKeywordFilePrefix.Text + line + this.textBoxChmHugoKeywordFilePostfix.Text; ; //.ToLower();
                                }
                            }
                            if (line.IndexOf("<UL>") > -1 && Link.Length > 0)
                            {
                                continue;
                                // headers without links
                                //Link = Link.Replace("<a # >", "");
                                //sw.WriteLine(Link);
                                //Link = "";
                            }
                            //sw.WriteLine();
                            if (Link.Length > 0 && Key.Length > 0)
                            {
                                sw.WriteLine(Key + " " + Link);
                                Link = "";
                                Key = "";
                            }
                            //else
                            //    sw.WriteLine(line);
                        }
                        sr.Dispose();
                        sw.Close();
                        sw.Dispose();
                        sw = null;

                        //this.userControlWebViewChmIndexFile.Url = null;
                        //this.userControlWebViewChmIndexFile.Navigate(_ChmIndexFile.FullName);
                    }

                    this.userControlWebViewChmKeywordFile.Url = null;
                    this.userControlWebViewChmKeywordFile.Navigate(ChmHugoKeywordFile.FullName);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonChmHugoKeywordFolder_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Filter = "txt files|*.txt";
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    if (true)
                    {
                        string Path = openFileDialog.FileName;
                        Process.Start(new ProcessStartInfo
                        {
                            FileName = Path,
                            UseShellExecute = true,
                        });
                    }

                }
                this.openFileDialog.OpenFile();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Hugo relref

        private string HugoModuleAcronym()
        {
            string Module = ChmHugoKeywordFile.Directory.Parent.Name.Substring(0, ChmHugoKeywordFile.Directory.Parent.Name.IndexOf("_"));
            string Acronym = "D";
            string EditString = Module.Replace("Diversity", "");
            if (string.IsNullOrEmpty(EditString)) { Acronym = "D"; }
            else { Acronym += EditString.Substring(0, 1).ToUpper(); }
            switch (EditString.ToLower())
            {
                case "samplingplots":
                    Acronym = "DSP";
                    break;
                case "scientificterms":
                    Acronym = "DST";
                    break;
                case "taxonnames":
                    Acronym = "DTN";
                    break;
            }
            return Acronym;
        }


        private System.IO.FileInfo _ChmHugoKeywordLinksFile = null;
        private System.IO.FileInfo ChmHugoKeywordLinksFile
        {
            get
            {
                if (_ChmHugoKeywordLinksFile == null && _ChmKeywordFile != null)
                {
                    _ChmHugoKeywordLinksFile = new FileInfo(_ChmKeywordFile.FullName.Replace(".html", "_" + this.HugoModuleAcronym() + ".txt"));
                }
                return _ChmHugoKeywordLinksFile;
            }
        }

        private void buttonChmHugoKeywordLinksFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (readyToCreateHugoKeywordFile())
                    this.GenerateHugoKeywordLinksFile();
                else { System.Windows.Forms.MessageBox.Show("Please select a source file"); }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); return; }
        }

        private void GenerateHugoKeywordLinksFile()
        {
            try
            {
                if (_ChmHhkFile != null)
                {
                    string Directory = "";
                    string Header = _ChmHhkFile.Name.Replace(".hhk", "");
                    if (Header == "Index")
                    {
                        Header = _ChmHhkFile.Directory.Name;
                        if (Header.EndsWith("Help"))
                        {
                            Directory = Header;
                            Header = Header.Substring(0, Header.Length - 4);
                        }
                    }
                    System.IO.StreamReader sr = new System.IO.StreamReader(_ChmHhkFile.FullName, true);
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(ChmHugoKeywordLinksFile.FullName, false, System.Text.Encoding.UTF8);

                    using (sr)
                    {
                        string line = "";
                        string Key = "";
                        string Link = "";

                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line == "<OBJECT type=\"text/site properties\">" ||
                                line == "\t<param name=\"ImageType\" value=\"Folder\">" ||
                                line.IndexOf("</OBJECT>") > -1 ||
                                line.IndexOf("<OBJECT type=\"text/sitemap\">") > -1 ||
                                line.IndexOf("</HEAD><BODY>") > -1 ||
                                line.IndexOf("</BODY>") > -1)
                                continue;

                            if (line.IndexOf("<param name=\"Name\" value=\"") > -1)
                            {
                                //if (Link.Length > 0)
                                //    continue;
                                line = sr.ReadLine(); // Reading the next line containing the key
                                line = line.Replace("<param name=\"Name\" value=\"", "");
                                if (line.IndexOf("\t") > -1)
                                {
                                    line = line.Substring(0, line.LastIndexOf("\t") + 1) + line.Substring(line.LastIndexOf("\t") + 1);
                                    line = line.Replace("\">", "");
                                    line = line.Substring(line.LastIndexOf("\t") + 1);
                                    Key = line + "_" + HugoModuleAcronym();
                                    //if (this.radioButtonChmHugoKeywordPrefix.Checked)
                                    //    Key = this.textBoxChmHugoKeywordPrefix.Text + line;// + this.textBoxChmHugoKeywordPostfix;
                                    //else
                                    //    Key = line + this.textBoxChmHugoKeywordPostfix.Text;// + this.textBoxChmHugoKeywordPostfix;
                                    continue;
                                }
                            }
                            if (line.IndexOf("<param name=\"Local\" value=\"") > -1)
                            {
                                line = line.Replace("<param name=\"Local\" value=\"", "");
                                if (line.IndexOf("\t") > -1)
                                {
                                    line = line.Substring(line.LastIndexOf("\t") + 1);
                                    line = line.Substring(0, line.LastIndexOf(".htm"));
                                    Link = this.textBoxChmHugoKeywordFilePrefix.Text + line + this.textBoxChmHugoKeywordFilePostfix.Text; ; //.ToLower();
                                }
                            }
                            if (line.IndexOf("<UL>") > -1 && Link.Length > 0)
                            {
                                continue;
                                // headers without links
                                //Link = Link.Replace("<a # >", "");
                                //sw.WriteLine(Link);
                                //Link = "";
                            }
                            //sw.WriteLine();
                            if (Link.Length > 0 && Key.Length > 0)
                            {
                                sw.WriteLine("- [" + Key.ToLower() + "]({{< relref \"" + Key.ToLower() + "\" >}})");
                                Link = "";
                                Key = "";
                            }
                            //else
                            //    sw.WriteLine(line);
                        }
                        sr.Dispose();
                        sw.Close();
                        sw.Dispose();
                        sw = null;

                        //this.userControlWebViewChmIndexFile.Url = null;
                        //this.userControlWebViewChmIndexFile.Navigate(_ChmIndexFile.FullName);
                    }

                    //this.userControlWebViewChmKeywordFile.Url = null;
                    //this.userControlWebViewChmKeywordFile.Navigate(ChmHugoKeywordFile.FullName);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }



        #endregion

        #endregion

        #region Markdown

        #region Html - md
        private void buttonMarkdownSourceFolder_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.ShowDialog();
            if (this.folderBrowserDialog.SelectedPath.Length > 0)
            {
                _MarkdownConvert.Clear();
                this.textBoxMarkdownSourceFolder.Text = this.folderBrowserDialog.SelectedPath;
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(this.folderBrowserDialog.SelectedPath);
                foreach (System.IO.FileInfo f in directory.GetFiles())
                {
                    // Markus 20.12.2023: Bugfix - missing .
                    if (f.Extension == ".htm" || f.Extension == ".html")
                    {
                        if (!_MarkdownConvert.ContainsKey(f.Name))
                            _MarkdownConvert.Add(f.Name, f);
                    }
                }
                this.initMarkdownSourceList();
            }
        }

        System.Collections.Generic.Dictionary<string, System.IO.FileInfo> _MarkdownConvert = new Dictionary<string, System.IO.FileInfo>();
        private void initMarkdownSourceList()
        {
            this.checkedListBoxMarkdownSouce.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in _MarkdownConvert)
            {
                this.checkedListBoxMarkdownSouce.Items.Add(KV.Value.Name);
                this.checkedListBoxMarkdownSouce.SetItemChecked(this.checkedListBoxMarkdownSouce.Items.Count - 1, true);
            }
        }

        private void buttonMarkdownTargetFolder_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.ShowDialog();
            if (this.folderBrowserDialog.SelectedPath.Length > 0)
            {
                this.textBoxMarkdownTargetFolder.Text = this.folderBrowserDialog.SelectedPath;
                this.initMarkdownTargetList();
            }
        }

        private void initMarkdownTargetList()
        {
            try
            {
                this.listBoxMarkdown.Items.Clear();
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(this.textBoxMarkdownTargetFolder.Text);
                foreach (System.IO.FileInfo f in directory.GetFiles())
                {
                    if (f.Extension == ".md")
                        this.listBoxMarkdown.Items.Add(f.Name);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        private void buttonMarkdownConvert_Click(object sender, EventArgs e)
        {
            if (this.textBoxMarkdownSourceFolder.Text.Length > 0 && this.textBoxMarkdownTargetFolder.Text.Length > 0)
            {
                foreach (System.Object item in this.checkedListBoxMarkdownSouce.CheckedItems)
                {
                    System.IO.FileInfo f = _MarkdownConvert[item.ToString()];
                    string Command = "--to=gfm --standalone " + f.FullName + " --output=" + this.textBoxMarkdownTargetFolder.Text + "\\" + f.Name.Replace(f.Extension, ".md");
                    System.Diagnostics.Process P = new System.Diagnostics.Process();
                    P.StartInfo.Arguments = Command;
                    P.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Pandoc\\pandoc.exe";
                    P.StartInfo.CreateNoWindow = true;
                    //P.StartInfo.RedirectStandardOutput = true;
                    //P.StartInfo.UseShellExecute = false;
                    P.Start();
                    P.WaitForExit(1000);
                    P.Close();
                    P.Dispose();
                    //P.WaitForExit();
                }
                this.initMarkdownTargetList();
            }
        }
        private void buttonMarkdownSourceCheck_Click(object sender, EventArgs e)
        {
            this.SetMarkdownSourceSelection(true);
        }

        private void buttonMarkdownSourceUncheck_Click(object sender, EventArgs e)
        {
            this.SetMarkdownSourceSelection(false);
        }


        private void SetMarkdownSourceSelection(bool IsSelected)
        {
            for (int i = 0; i < this.checkedListBoxMarkdownSouce.Items.Count; i++)
            {
                this.checkedListBoxMarkdownSouce.SetItemChecked(i, IsSelected);
            }
        }

        #endregion

        #region md to html

        System.Collections.Generic.Dictionary<string, System.IO.FileInfo> _MarkdownToHtmlConvert = new Dictionary<string, System.IO.FileInfo>();

        private void initMarkdownToHtmlSourceList()
        {
            this.checkedListBoxMarkdownToHtmlSource.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in _MarkdownToHtmlConvert)
            {
                this.checkedListBoxMarkdownToHtmlSource.Items.Add(KV.Value.Name);
                this.checkedListBoxMarkdownToHtmlSource.SetItemChecked(this.checkedListBoxMarkdownToHtmlSource.Items.Count - 1, true);
            }
        }

        private void buttonMarkdownToHtmlSource_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.ShowDialog();
            if (this.folderBrowserDialog.SelectedPath.Length > 0)
            {
                this.textBoxMarkdownToHtmlSource.Text = this.folderBrowserDialog.SelectedPath;
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(this.folderBrowserDialog.SelectedPath);
                foreach (System.IO.FileInfo f in directory.GetFiles())
                {
                    if (f.Extension == ".md")
                    {
                        if (!_MarkdownToHtmlConvert.ContainsKey(f.Name))
                            _MarkdownToHtmlConvert.Add(f.Name, f);
                    }
                }
                this.initMarkdownToHtmlSourceList();
            }
        }

        private void initMarkdownToHtmlTargetList()
        {
            this.listBoxMarkdownToHtmlTarget.Items.Clear();
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(this.textBoxMarkdownToHtmlTarget.Text);
            foreach (System.IO.FileInfo f in directory.GetFiles())
            {
                if (f.Extension == ".htm" || f.Extension == ".html")
                    this.listBoxMarkdownToHtmlTarget.Items.Add(f.Name);
            }
        }


        private void buttonMarkdownToHtmlTarget_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.ShowDialog();
            if (this.folderBrowserDialog.SelectedPath.Length > 0)
            {
                this.textBoxMarkdownToHtmlTarget.Text = this.folderBrowserDialog.SelectedPath;
                this.initMarkdownToHtmlTargetList();
            }
        }

        System.Collections.Generic.Dictionary<string, System.IO.FileInfo> _MarkdownToHtmlCss = new Dictionary<string, System.IO.FileInfo>();

        private void toolStripButtonMarkdownToHtmlCssAdd_Click(object sender, EventArgs e)
        {
            try
            {
                this.openFileDialog.Filter = "css files|*.css";
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    try
                    {
                        System.IO.FileInfo css = new System.IO.FileInfo(this.openFileDialog.FileName);
                        if (!this._MarkdownToHtmlCss.ContainsKey(css.Name))
                        {
                            this._MarkdownToHtmlCss.Add(css.Name, css);
                            this.initMarkdownToHtmlCssList();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initMarkdownToHtmlCssList()
        {
            this.listBoxMarkdownToHtmlCss.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in this._MarkdownToHtmlCss)
            {
                this.listBoxMarkdownToHtmlCss.Items.Add(KV.Key);
            }
        }

        private void toolStripButtonMarkdownToHtmlCssRemove_Click(object sender, EventArgs e)
        {
            if (this._MarkdownToHtmlCss.ContainsKey(this.listBoxMarkdownToHtmlCss.SelectedItem.ToString()))
            {
                this._MarkdownToHtmlCss.Remove(this.listBoxMarkdownToHtmlCss.SelectedItem.ToString());
                this.initMarkdownToHtmlCssList();
            }
        }

        private void buttonMarkdownToHtmlConvert_Click(object sender, EventArgs e)
        {
            if (this.textBoxMarkdownToHtmlSource.Text.Length > 0 && this.textBoxMarkdownToHtmlTarget.Text.Length > 0)
            {
                foreach (System.Object item in this.checkedListBoxMarkdownToHtmlSource.CheckedItems)
                {
                    System.IO.FileInfo f = _MarkdownToHtmlConvert[item.ToString()];
                    string CSS = "";
                    foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in this._MarkdownToHtmlCss)
                    {
                        CSS += " --css=" + KV.Value.FullName;
                    }
                    string Command = "--to=html5 " + CSS + " --standalone " + f.FullName + " --output=" + this.textBoxMarkdownToHtmlTarget.Text + "\\" + f.Name.Replace(f.Extension, ".htm"); //; --to=markdown --standalone " + f.FullName + " --output=" + this.textBoxMarkdownTargetFolder.Text + "\\" + f.Name.Replace(f.Extension, ".md");
                    System.Diagnostics.Process P = new System.Diagnostics.Process();
                    P.StartInfo.Arguments = Command;
                    P.StartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\AppData\\Local\\Pandoc\\pandoc.exe";
                    P.StartInfo.CreateNoWindow = true;
                    P.Start();
                    P.WaitForExit(1000);
                    P.Close();
                    P.Dispose();
                }
                this.initMarkdownToHtmlTargetList();
            }
        }

        private void buttonMarkdownToHtmlUncheck_Click(object sender, EventArgs e)
        {
            this.SetMarkdownToHtmlSelection(false);
        }

        private void buttonMarkdownToHtmlCheck_Click(object sender, EventArgs e)
        {
            this.SetMarkdownToHtmlSelection(true);
        }

        private void SetMarkdownToHtmlSelection(bool IsSelected)
        {
            for (int i = 0; i < this.checkedListBoxMarkdownToHtmlSource.Items.Count; i++)
            {
                this.checkedListBoxMarkdownToHtmlSource.SetItemChecked(i, IsSelected);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Auxillary functions

        private System.Data.DataTable TableList()
        {
            System.Data.DataTable dt = new DataTable();
            string SQL = "";
            if (this._ConnectionString.Length > 0)
            {
                if (this.ServerVersion == "2000")
                {
                    SQL = "SELECT TABLE_NAME as ObjectName, 'TABLE' AS ObjectType, cast(0 as tinyint) AS DisplayOrder " +
                        "FROM Information_Schema.Tables " +
                        "WHERE TABLE_NAME <> 'dtproperties' AND TABLE_TYPE = 'BASE TABLE' ORDER BY Table_Name";
                }
                else if (this.ServerVersion == "2005")
                {
                    SQL = "SELECT sys.tables.name as ObjectName, 'TABLE' AS ObjectType, cast(0 as tinyint) AS DisplayOrder " +
                        "FROM sys.tables " +
                        "WHERE sys.tables.name <> 'sysdiagrams' ORDER BY sys.tables.name";
                }
                else
                {
                    SQL = "";
                    if (this.checkBoxIncludeTables.Checked)
                    {
                        SQL += "SELECT TABLE_NAME as ObjectName, 'TABLE' AS ObjectType, cast(0 as tinyint) AS DisplayOrder " +
                        "FROM Information_Schema.Tables WHERE TABLE_TYPE = 'BASE TABLE' ";
                        if (this._DtSchemata != null
                            && this._DtSchemata.Rows.Count > 1)
                        {
                            SQL += " AND TABLE_SCHEMA = '" + this._Schema + "' ";
                        }
                    }
                    if (this.checkBoxIncludeViews.Checked)
                    {
                        if (SQL.Length > 0)
                            SQL += " UNION ";
                        SQL += "SELECT TABLE_NAME as ObjectName, TABLE_TYPE AS ObjectType, cast(1 as tinyint) AS DisplayOrder " +
                            "FROM Information_Schema.Tables WHERE TABLE_TYPE = 'VIEW' ";
                        if (this._DtSchemata != null
                            && this._DtSchemata.Rows.Count > 1)
                        {
                            SQL += " AND TABLE_SCHEMA = '" + this._Schema + "' ";
                        }
                    }
                    if (this.checkBoxIncludeFunctions.Checked)
                    {
                        if (SQL.Length > 0)
                            SQL += " UNION ";
                        SQL += "SELECT ROUTINE_NAME as ObjectName, ROUTINE_TYPE AS ObjectType, cast(2 as tinyint) AS DisplayOrder " +
                            "FROM Information_Schema.ROUTINES " +
                            " WHERE ((ROUTINE_TYPE = 'FUNCTION' AND ROUTINE_NAME NOT LIKE 'fn_%') OR (ROUTINE_TYPE = 'PROCEDURE' AND ROUTINE_NAME NOT LIKE 'dt_%' AND ROUTINE_NAME NOT LIKE 'sp_%')) ";
                        if (this._DtSchemata != null
                            && this._DtSchemata.Rows.Count > 1)
                        {
                            SQL += " AND SPECIFIC_SCHEMA = '" + this._Schema + "' ";
                        }
                    }
                    if (this.checkBoxIncludeRoles.Checked)
                    {
                        if (SQL.Length > 0)
                            SQL += " UNION ";
                        SQL += "SELECT name as ObjectName, 'Role' AS ObjectType, cast(3 as tinyint) AS DisplayOrder FROM sysusers WHERE (issqlrole = 1) AND (name <> N'public') AND (name NOT LIKE 'db_%') ";
                    }
                    if (SQL != "")
                        SQL += " ORDER BY DisplayOrder, ObjectType, ObjectName";
                }
                if (SQL == "")
                    return dt;
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.Conn);// this._ConnectionString);
                try
                {
                    a.Fill(dt);
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("Please connect to database");
            return dt;
        }

        private string ServerVersion
        {
            get
            {
                if (this._ServerVersion != "") return this._ServerVersion;
                string SQL = "SELECT @@VERSION AS 'SQL Server Version'";
                if (DiversityWorkbench.Settings.DatabaseName == "") DiversityWorkbench.Settings.DatabaseName = "master";
                if (this._ConnectionString != "")
                {
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        this._ServerVersion = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    }
                    finally { con.Close(); }
                }
                if (this._ServerVersion.Length > 0)
                {
                    if (this._ServerVersion.IndexOf("Microsoft SQL Server 2005") == 0) this._ServerVersion = "2005";
                    else if (this._ServerVersion.IndexOf("Microsoft SQL Server  2000") == 0) this._ServerVersion = "2000";
                }
                return this._ServerVersion;
            }
            set
            {
                this._ServerVersion = value;
            }
        }


        private System.Data.DataTable dtPrimaryKey(string Table)
        {
            if (this._dtCurrentPK == null) this._dtCurrentPK = new DataTable();
            if (this._dtCurrentPK.Rows.Count > 0 && this._CurrentPK == Table)
                return this._dtCurrentPK;
            else
            {
                this._dtCurrentPK.Clear();
                this._CurrentPK = Table;
            }
            //System.Data.DataTable dt = new DataTable();
            string SQL = "";
            if (this.ServerVersion == "2000")
            {
                SQL = "SELECT COLUMN_NAME AS ColumnName  FROM Information_Schema.KEY_COLUMN_USAGE " +
                    "WHERE Information_Schema.KEY_COLUMN_USAGE.TABLE_NAME = '" + Table + "' " +
                    "AND CONSTRAINT_NAME LIKE 'PK_%'";
            }
            else
            {
                SQL = "select c.Column_Name AS ColumnName from INFORMATION_SCHEMA.KEY_COLUMN_USAGE c " +
                ", INFORMATION_SCHEMA.TABLE_CONSTRAINTS t " +
                "where c.Table_name = '" + Table + "' " +
                "and c.Table_name = t.Table_name " +
                "and c.Constraint_name = t.Constraint_name " +
                "and T.Constraint_type = 'primary key'";
            }
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
            a.Fill(this._dtCurrentPK);
            return this._dtCurrentPK;
        }

        private System.Collections.Generic.List<string> TableColumns(string Table)
        {
            System.Collections.Generic.List<string> CC = new List<string>();
            foreach (System.Data.DataRow R in this.dtTable(Table).Rows)
                CC.Add(R[0].ToString());
            return CC;
        }


        private System.Data.DataTable dtTable(string TableName)
        {
            if (this._dtCurrentTable == null) this._dtCurrentTable = new DataTable();
            if (this._dtCurrentTable.Rows.Count > 0 && TableName == this._CurrentTable)
                return this._dtCurrentTable;
            else
            {
                this._dtCurrentTable.Clear();
                this._dtCurrentTable.Columns.Clear();
            }
            this._CurrentTable = TableName;
            //System.Data.DataTable dt = new DataTable();
            string SQL = "";
            //if (this.ServerVersion == "2000")
            //{
            SQL = "SELECT COLUMN_NAME as ColumnName, DATA_TYPE as Datatype, CHARACTER_MAXIMUM_LENGTH as Length, " +
                "COLLATION_NAME  AS Collation, COLUMN_DEFAULT AS DefaultValue, '' AS Description " +
                "FROM Information_Schema.COLUMNS  " +
                "WHERE Information_Schema.COLUMNS.TABLE_NAME = '" + TableName + "'";
            //}
            //else
            //{
            //    SQL = "SELECT /*sys.tables.name as TableName,*/
            //sys.columns.name as ColumnName, sys.types.name as Datatype, " +
            //    "case when sys.types.precision = 0 then " +
            //    "case when sys.types.name = 'nvarchar' or sys.types.name = 'nchar' then sys.columns.max_length / 2 else sys.columns.max_length end  " +
            //    "else null end as Length, sys.columns.collation_name AS Collation, '' AS Description " +
            //    "FROM sys.tables, sys.columns, sys.types " +
            //    "WHERE sys.tables.object_id = sys.columns.object_id " +
            //    "and sys.columns.user_type_id = sys.types.user_type_id " +
            //    "AND sys.tables.type_desc = 'USER_TABLE' " +
            //    "AND sys.tables.name <> 'sysdiagrams' " +
            //    "AND sys.tables.name = '" + TableName + "'";
            //}
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ConnectionString);
            try
            {
                a.Fill(this._dtCurrentTable);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return this._dtCurrentTable;
        }


        #endregion

        #region Hugo Replacements

        #region Parameter

        System.Collections.Generic.Dictionary<string, System.IO.FileInfo> _HugoReplacementFiles = new Dictionary<string, System.IO.FileInfo>();

        System.Collections.Generic.Dictionary<string, string> _HugoReplacements = new Dictionary<string, string>();
        System.Data.DataTable _HugoReplacementDataTable;

        private readonly string _HugoReplacementSeparator = "    ⇒    ";
        private readonly string _HugoReplacementWildcard = " :::: ";

        #endregion

        #region Getting and selecting the files

        private void buttonHugoReplacementsOpen_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog.ShowDialog();
            if (this.folderBrowserDialog.SelectedPath.Length > 0)
            {
                _HugoReplacementFiles.Clear();
                this.textBoxHugoReplacementsDirectory.Text = this.folderBrowserDialog.SelectedPath;
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(this.folderBrowserDialog.SelectedPath);
                foreach (System.IO.FileInfo f in directory.GetFiles())
                {
                    if (f.Extension == ".md")
                    {
                        if (!_HugoReplacementFiles.ContainsKey(f.Name))
                            _HugoReplacementFiles.Add(f.Name, f);
                    }
                }
                this.initHugoReplacementFiles();
            }
        }

        private void initHugoReplacementFiles()
        {
            this.checkedListBoxHugoReplacementFiles.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in _HugoReplacementFiles)
            {
                this.checkedListBoxHugoReplacementFiles.Items.Add(KV.Value.Name);
                this.checkedListBoxHugoReplacementFiles.SetItemChecked(this.checkedListBoxHugoReplacementFiles.Items.Count - 1, true);
            }
            this.initHugoReplacements();
        }

        private void buttonHugoReplacementsFilesAll_Click(object sender, EventArgs e)
        {
            this.SetHugoReplacementsSourceSelection(true, this.checkedListBoxHugoReplacementFiles);
        }

        private void buttonHugoReplacementsFilesNone_Click(object sender, EventArgs e)
        {
            this.SetHugoReplacementsSourceSelection(false, this.checkedListBoxHugoReplacementFiles);
        }

        private void checkedListBoxHugoReplacementFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.HugoReplacementReadFile();
        }

        private void HugoReplacementReadFile()
        {
            this.textBoxHugoReplacementFile.Text = "";
            if (this.checkedListBoxHugoReplacementFiles.SelectedItem == null)
                return;

            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.textBoxHugoReplacementsDirectory.Text + "\\" + this.checkedListBoxHugoReplacementFiles.SelectedItem.ToString());
            if (fileInfo.Exists)
            {
                this.textBoxHugoReplacementFile.Text = this.HugoReplacementFileContent(fileInfo.Name);
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private string HugoReplacementFileContent(string FileName)
        {
            string Content = "";
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.textBoxHugoReplacementsDirectory.Text + "\\" + FileName);
            if (fileInfo.Exists)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(fileInfo.FullName, System.Text.Encoding.UTF8);
                string Line;
                while ((Line = sr.ReadLine()) != null)
                    Content += Line + "\r\n";
                sr.Close();
                sr.Dispose();
            }
            return Content;
        }

        #endregion

        #region Handling the replacements

        private void initHugoReplacements()
        {
            this.checkedListBoxHugoReplacements.Items.Clear();
            _HugoReplacements.Clear();
            if (DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements == null)
                DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements = new System.Collections.Specialized.StringCollection();
            foreach (string s in DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements)
            {
                string[] ss = s.Split(new string[] { this._HugoReplacementSeparator }, StringSplitOptions.None);
                if (ss.Length > 1 && !_HugoReplacements.ContainsKey(ss[0]))
                {
                    _HugoReplacements.Add(ss[0], ss[1]);
                    //string Replacement = s.Replace(_HugoReplacementSeparator, "   ->   ");
                    this.checkedListBoxHugoReplacements.Items.Add(s);
                    this.checkedListBoxHugoReplacements.SetItemChecked(this.checkedListBoxHugoReplacements.Items.Count - 1, true);
                }
            }
        }

        private void buttonHugoReplacementsDelete_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        private void buttonHugoReplacementsAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormTableContent f = new FormTableContent("HUGO replacements", "Replacements for markdown files for publication via HUGO ", getHugoReplacements());
            f.AllowEdit(true, true, true);
            f.setIcon(DiversityWorkbench.Properties.Resources.Translation);
            f.RowHeaderVisible(true);
            f.Width = 600;
            f.Height = 600;
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
            this.setHugoReplacements(f.DataTable());
        }

        private void buttonHugoReplacementsOpenFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Filter = "txt files|*.txt";
            this.openFileDialog.ShowDialog();
            if (this.openFileDialog.FileName.Length > 0 && this.openFileDialog.FileName != "openFileDialog")
            {
                try
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.openFileDialog.FileName);
                    if (fileInfo.Exists)
                    {
                        System.Collections.Generic.Dictionary<int, string> Content = HugoReplacementsFileContent(fileInfo);
                        if (Content.Count > 0)
                        {
                            if (DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements == null)
                                DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements = new System.Collections.Specialized.StringCollection();
                            DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements.Clear();
                            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in Content)
                            {
                                string[] ss = KV.Value.Split(new string[] { "\t" }, StringSplitOptions.None);
                                if (ss.Length == 2 || (ss.Length == 3 && ss[2].Trim().Length == 0))
                                {
                                    string Replacement = ss[0] + this._HugoReplacementSeparator + ss[1];
                                    DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements.Add(Replacement);
                                }
                                else { }
                            }
                            DiversityWorkbench.Forms.FormDocumentationSettings.Default.Save();
                            this.initHugoReplacements();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private System.Collections.Generic.Dictionary<int, string> HugoReplacementsFileContent(System.IO.FileInfo fileInfo)
        {
            System.Collections.Generic.Dictionary<int, string> Content = new Dictionary<int, string>();
            try
            {
                if (fileInfo.Exists)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(fileInfo.FullName, System.Text.Encoding.UTF8);
                    string Line;
                    int i = 0;
                    while ((Line = sr.ReadLine()) != null)
                    {
                        Content.Add(i, Line);
                        i++;
                    }
                    sr.Close();
                    sr.Dispose();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Content;
        }

        private System.Data.DataTable getHugoReplacements()
        {
            _HugoReplacementDataTable = new DataTable();
            System.Data.DataColumn dcReplace = new DataColumn("Replace", typeof(string));
            _HugoReplacementDataTable.Columns.Add(dcReplace);
            System.Data.DataColumn dcWith = new DataColumn("With", typeof(string));
            _HugoReplacementDataTable.Columns.Add(dcWith);
            if (DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements == null)
                DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements = new System.Collections.Specialized.StringCollection();
            foreach (string s in DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements)
            {
                string[] ss = s.Split(new string[] { this._HugoReplacementSeparator }, StringSplitOptions.None);
                _HugoReplacementDataTable.Rows.Add(ss[0], ss[1]);
            }
            return _HugoReplacementDataTable;
        }

        private void setHugoReplacements(System.Data.DataTable dataTable)
        {
            DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements.Clear();
            foreach (System.Data.DataRow R in dataTable.Rows)
            {
                string Replacement = R[0].ToString() + _HugoReplacementSeparator + R[1].ToString();
                DiversityWorkbench.Forms.FormDocumentationSettings.Default.HugoReplacements.Add(Replacement);
            }
            DiversityWorkbench.Forms.FormDocumentationSettings.Default.Save();
            this.initHugoReplacements();
        }

        private void buttonHugoReplacementsAll_Click(object sender, EventArgs e)
        {
            this.SetHugoReplacementsSourceSelection(true, this.checkedListBoxHugoReplacements);
        }

        private void buttonHugoReplacementsNone_Click(object sender, EventArgs e)
        {
            this.SetHugoReplacementsSourceSelection(false, this.checkedListBoxHugoReplacements);
        }

        private void SetHugoReplacementsSourceSelection(bool IsSelected, System.Windows.Forms.CheckedListBox checkedListBox)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                checkedListBox.SetItemChecked(i, IsSelected);
            }
        }

        #endregion

        #region Editing the files
        private void buttonHugoReplacementsReplace_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            int i = 0;
            foreach (System.Object o in this.checkedListBoxHugoReplacementFiles.CheckedItems)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.textBoxHugoReplacementsDirectory.Text + "\\" + o.ToString());
                if (fileInfo.Exists)
                {
                    HugoReplacementsEditFile(fileInfo);
                    i++;
                }
            }
            this.HugoReplacementReadFile();
            this.Cursor = System.Windows.Forms.Cursors.Default;
            System.Windows.Forms.MessageBox.Show("Replacement performed for " + i.ToString() + " files");
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }

        private void HugoReplacementsEditFile(System.IO.FileInfo fileInfo)
        {
            if (!fileInfo.Exists)
                return;
            System.Collections.Generic.Dictionary<int, string> Content = new Dictionary<int, string>();
            System.IO.StreamReader sr = new System.IO.StreamReader(fileInfo.FullName, System.Text.Encoding.UTF8);
            string Line;
            int i = 0;
            while ((Line = sr.ReadLine()) != null)
            {
                Content.Add(i, Line);
                i++;
            }
            sr.Close();
            sr.Dispose();

            foreach (System.Object o in this.checkedListBoxHugoReplacements.CheckedItems)
            {
                string Rep = o.ToString();
                string[] rw = Rep.Split(new string[] { this._HugoReplacementSeparator }, StringSplitOptions.None);
                if (rw.Length == 2)
                {
                    for (int c = 0; c < Content.Count; c++)
                    {
                        Content[c] = this.HugoReplacementsReplaceText(Content[c], rw[0], rw[1]);
                        //if (Content[c].IndexOf(rw[0]) > -1)
                        //{
                        //    Content[c] = Content[c].Replace(rw[0], rw[1]);
                        //}
                        //if (rw[0].IndexOf(_HugoReplacementWildcard) > -1)
                        //{
                        //    string[] ss = rw[0].Split(new string[] { _HugoReplacementWildcard }, StringSplitOptions.None);
                        //    if (ss.Length == 2)
                        //    {
                        //        if (Content[c].IndexOf(ss[0]) > -1 && Content[c].IndexOf(ss[1]) > -1 && Content[c].IndexOf(ss[1]) > Content[c].IndexOf(ss[0]))
                        //        {
                        //            string RE = Content[c].Substring(0, Content[c].IndexOf(ss[0])) + rw[1] + Content[c].Substring(Content[c].IndexOf(ss[1]) + 1);
                        //            Content[c] = RE;
                        //        }
                        //    }
                        //}
                    }
                }
            }

            System.IO.StreamWriter sw = new System.IO.StreamWriter(fileInfo.FullName, false, System.Text.Encoding.UTF8);
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in Content)
            {
                sw.WriteLine(KV.Value);// + "\r\n");
            }
            sw.Close();
            sw.Dispose();

        }

        private string HugoReplacementsReplaceText(string Text, string Replace, string With)
        {
            string Result = Text;
            if (Result.IndexOf(Replace) > -1)
            {
                Result = Result.Replace(Replace, With);
            }
            if (Replace.IndexOf(_HugoReplacementWildcard) > -1)
            {
                string[] rr = Replace.Split(new string[] { _HugoReplacementWildcard }, StringSplitOptions.None);
                string[] ww = With.Split(new string[] { _HugoReplacementWildcard }, StringSplitOptions.None);
                if (rr.Length == 2)
                {
                    if (Result.IndexOf(rr[0]) > -1 && Result.IndexOf(rr[1]) > -1 && Result.IndexOf(rr[1]) > Result.IndexOf(rr[0]))
                    {
                        string RE = Result.Substring(0, Result.IndexOf(rr[0])) + With + Result.Substring(Result.IndexOf(rr[1]) + 1);
                        Result = RE;
                    }
                }
                else if (rr.Length > 2 && rr.Length == ww.Length)
                {
                    string result = "";
                    System.Collections.Generic.Dictionary<int, string> RR = new Dictionary<int, string>();
                    System.Collections.Generic.Dictionary<int, string> WW = new Dictionary<int, string>();
                    for (int i = 0; i < ww.Length; i++) WW.Add(i, ww[i]);
                    for (int i = 0; i < rr.Length; i++) RR.Add(i, ww[i]);
                    bool Match = true;
                    for (int i = 0; i < RR.Count; i++)
                    {
                        if (Text.IndexOf(RR[i]) == -1)
                        {
                            Match = false;
                            break;
                        }
                        if (i > 0)
                        {
                            if (Text.IndexOf(RR[i - 1]) > Text.IndexOf(RR[i]))
                            {
                                Match = false;
                                break;
                            }
                        }
                    }
                    if (Match)
                    {
                        string Rest = Text;
                        result = "";
                        for (int i = 0; i < RR.Count; i++)
                        {
                            result += Rest.Substring(0, Rest.IndexOf(RR[i])) + WW[i];
                            Rest = Rest.Substring(Rest.IndexOf(RR[i]) + RR[i].Length);
                        }
                        result += Rest;
                        Result = result;
                    }
                }
                else
                {

                }
            }
            return Result;
        }

        #endregion

        #region setting the header

        private void buttonHugoReplacementSetDefaultHeader_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string Message = "";
            int iOK = 0;
            foreach (System.Object o in this.checkedListBoxHugoReplacementFiles.CheckedItems)
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.textBoxHugoReplacementsDirectory.Text + "\\" + o.ToString());
                if (fileInfo.Exists)
                {
                    string NoStandardHeaderFor = HugoReplacementSetDefaultHeader(fileInfo.Name);
                    if (NoStandardHeaderFor.Length > 0)
                    {
                        if (Message.Length == 0) Message = "No standard header found for:\r\n";
                        Message += NoStandardHeaderFor;
                    }
                    else iOK++;
                }
            }
            if (iOK > 0)
                Message = "Header adapted for " + iOK.ToString() + " files\r\n\r\n" + Message;
            this.HugoReplacementReadFile();
            this.Cursor = System.Windows.Forms.Cursors.Default;
            if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
        }

        private string HugoReplacementSetDefaultHeader(string FileName)
        {
            string Message = "";
            string Content = this.HugoReplacementFileContent(FileName);
            string[] cc = Content.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            if (cc[0] == "---" && cc[2] == "---")
            {
                string Title = cc[1].Substring(cc[1].IndexOf(":") + 1).Trim();
                string title = "";
                int previousCapital = 0;
                for (int i = 0; i < Title.Length; i++)
                {
                    if (i > 0)
                    {
                        if (Title[i].ToString().ToUpper() == Title[i].ToString())
                        {
                            if (previousCapital < i - 1)
                                title += " ";
                            previousCapital = i;
                        }
                    }
                    title += Title[i].ToString().Replace("_", " ");
                }
                Title = title;
                string DefaultHeader = "---\r\n" +
                    "title: " + Title + "\r\n" +
                    "menutitle: " + Title + "\r\n" +
                    "menuPre: <img src=\"/manual/dwb/img/" + Title.Replace(" ", "") + ".svg\" height=\"20\"> &nbsp;\r\n" +
                    "weight: 1\r\n" +
                    "alwaysopen: false\r\n" +
                    "---\r\n" +
                    "\r\n" +
                    "![](/manual/dwb/img/" + Title.Replace(" ", "") + ".svg?height=10vw&lightbox=false)\r\n" +
                    "\r\n";

                string Replacement = DefaultHeader;
                for (int i = 3; i < cc.Length; i++)
                {
                    Replacement += cc[i] + "\r\n";
                }
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.textBoxHugoReplacementsDirectory.Text + "\\" + FileName);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fileInfo.FullName, false, System.Text.Encoding.UTF8);
                sw.Write(Replacement);
                sw.Close();
                sw.Dispose();
                Message = ""; // FileName + ": " + "Header adapted\r\n";
            }
            else
            {
                Message = FileName + "\r\n"; //: " + "No standard header found\r\n";
            }
            return Message;
        }

        #endregion

        #endregion


        #region Hugo Links

        private System.IO.DirectoryInfo _HugoLinkDir = null;
        System.Collections.Generic.Dictionary<string, System.IO.FileInfo> _HugoLinkFiles = new Dictionary<string, System.IO.FileInfo>();
        System.Collections.Generic.Dictionary<string, string> _HugoLinks = new Dictionary<string, string>();
        System.Collections.Generic.Dictionary<string, string> _HugoLinksMissing = new Dictionary<string, string>();
        //System.Collections.Generic.List<string> _HugoLinksMissing = new List<string>();

        #region getting the files
        private void buttonHugoLinksDirectory_Click(object sender, EventArgs e)
        {
            try
            {
                string Message = "";
                this.folderBrowserDialog.ShowDialog();
                if (this.folderBrowserDialog.SelectedPath.Length > 0)
                {
                    _HugoLinkFiles.Clear();
                    this.textBoxHugoLinksDirectory.Text = this.folderBrowserDialog.SelectedPath;
                    _HugoLinkDir = new System.IO.DirectoryInfo(this.folderBrowserDialog.SelectedPath);
                    this.AddHugoLinkFiles(_HugoLinkDir);
                    this.initHugoLinkFiles();
                    string Marker = "_D" + _HugoLinkDir.Name.Substring("diversity".Length, 1).ToUpper();
                    this.textBoxHugoLinksModuleMarker.Text = Marker;
                    Message = _HugoLinkFiles.Count.ToString() + " files in directory\r\n" +
                        "Module marker according to directory: " + Marker;
                    labelHugoLinksMessage.Text = Message;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void AddHugoLinkFiles(System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo f in directory.GetFiles())
            {
                if (f.Extension == ".md")
                {
                    string File = directory.Name + "/" + f.Name;
                    if (!_HugoLinkFiles.ContainsKey(File))
                        _HugoLinkFiles.Add(File, f);
                }
            }
            foreach (System.IO.DirectoryInfo dir in directory.GetDirectories())
            {
                AddHugoLinkFiles(dir);
            }
        }

        private void initHugoLinkFiles()
        {
            this.listBoxHugoLinks.Items.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in _HugoLinkFiles)
            {
                this.listBoxHugoLinks.Items.Add(KV.Key);
            }
        }

        private void buttonHugoLinksShowFileContent_Click(object sender, EventArgs e)
        {
            if (this.listBoxHugoLinks.SelectedIndex > -1)
            {
                System.IO.FileInfo fileInfo = this._HugoLinkFiles[this.listBoxHugoLinks.SelectedItem.ToString()];
                using (StreamReader sr = fileInfo.OpenText())
                {
                    string Output = "";
                    var s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Output += s + "\r\n";
                    }
                    DiversityWorkbench.Forms.FormEditText formEditText = new DiversityWorkbench.Forms.FormEditText(fileInfo.Name, Output, true);
                    formEditText.StartPosition = FormStartPosition.CenterParent;
                    formEditText.Width = this.Width - 10;
                    formEditText.Height = this.Height - 10;
                    formEditText.ShowDialog();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select a file");
            }
        }

        #endregion

        #region getting the links

        private void HugoLinksGetLinks_Click(object sender, EventArgs e)
        {
            try
            {
                _HugoLinks.Clear();
                _HugoLinksMissing.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> file in _HugoLinkFiles)
                {
                    HugoLinksGetLinksInFile(file.Value);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            this.HugoLinksInitTables();
        }

        private void HugoLinksGetLinksInFile(System.IO.FileInfo fileInfo)
        {
            using (StreamReader sr = fileInfo.OpenText())
            {
                var s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    HugoLinksGetLinksInLine(s, fileInfo);
                }
            }
        }

        private void HugoLinksInitTables()
        {
            try
            {
                this.HugoLinksInitTable(_HugoLinks, dataGridViewHugoLinks);
                this.HugoLinksInitTable(_HugoLinksMissing, dataGridViewHugoLinksMissing);
                string Message = _HugoLinks.Count.ToString() + " Links and their corresponding files in files detected\r\n" +
                    _HugoLinksMissing.Count.ToString() + " could not be linked to an existing file";
                this.labelHugoLinksMessage.Text = Message;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void HugoLinksInitTable(System.Collections.Generic.Dictionary<string, string> dict, DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            if (dict.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in dict)
                {
                    switch (dataGridView.Columns.Count)
                    {
                        case 1:
                            dataGridView.Rows.Add(kvp.Key);
                            break;
                        case 2:
                            dataGridView.Rows.Add(kvp.Key, kvp.Value);
                            break;
                    }
                }
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }

        private void HugoLinksGetLinksInLine(string Line, System.IO.FileInfo fileInfo)
        {
            string pattern = @"\[([^\]]+)\]\(([^)]+)\)";
            MatchCollection m = Regex.Matches(Line, pattern);
            if (m.Count > 0)
            {
                foreach (Match match in m)
                {
                    if (HugoLinksIsMatch(match.Value) && match.Success)
                    {
                        string Core = HugoLinkCore(match.Value);// + this.textBoxHugoLinksModuleMarker.Text;
                        string FileKey = HugoLinksCheckIfFileExists(Core);
                        if (_HugoLinkTranslations.Count > 0 && FileKey.Length == 0)
                        {
                            string OriLinkFromFile = HugoLinkCore(match.Value, false, true);
                            FileKey = HugoLinksCheckIfTranslationExists(OriLinkFromFile);
                        }
                        string LinkText = match.Value.Substring(0, match.Value.IndexOf("]") + 1);
                        string Marker = "";
                        if (match.Value.IndexOf("#") > -1) Marker = match.Value.Substring(match.Value.IndexOf("#"));
                        if (Marker.EndsWith(")")) Marker = Marker.Substring(0, Marker.Length - 1);
                        if (FileKey.Length > 0 && !_HugoLinks.ContainsKey(match.Value))
                        {
                            string Link = HugoLinkFromText(LinkText, FileKey, Marker);// HugoLinkFromText(Core);
                            _HugoLinks.Add(match.Value, Link);
                        }
                        else if (!_HugoLinksMissing.ContainsKey(match.Value))
                        {
                            //if (_HugoLinkTranslations.Count > 0 && FileKey.Length == 0)
                            //{
                            //    FileKey = HugoLinksCheckIfTranslationExists(Core);
                            //}
                            //if (FileKey.Length > 0)
                            //{

                            //}
                            //else
                            _HugoLinksMissing.Add(match.Value, fileInfo.FullName);
                        }
                    }
                }
            }
        }

        private string HugoLinkFromText(string LinkText, string FileKey, string Marker)
        {
            string HugoLink = "";
            try
            {
                if (FileKey.IndexOf("/_index") > -1)
                    FileKey = FileKey.Substring(0, FileKey.IndexOf("/_index"));
                if (Marker.Length > 0)
                {
                    if (!Marker.StartsWith("#")) Marker = "#" + Marker;
                    FileKey += Marker;
                }
                if (FileKey.IndexOf("/") > -1)
                {
                    FileKey = FileKey.Substring(FileKey.IndexOf("/") + 1);
                }
                if (FileKey.EndsWith(".md"))
                {
                    FileKey = FileKey.Substring(0, FileKey.Length - 3);
                }
                HugoLink = LinkText + "({{< relref \"" + FileKey + "\" >}})";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return HugoLink;
        }

        private string HugoLinkFromText(string Link)
        {
            string HugoLink = "";
            try
            {
                string Core = HugoLinkCore(Link);// + this.textBoxHugoLinksModuleMarker.Text;
                HugoLink = "{{< relref \"" + Core + "\" >}}";
                HugoLink = Link.Substring(Link.IndexOf('(') + 1) + Link + ")";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return HugoLink;
        }

        private string HugoLinkCore(string Link, bool AddModuleMarker = true, bool RemoveMarker = false)
        {
            string Core = "";
            try
            {
                if (Link.IndexOf("]") > -1)
                {
                    Core = Link.Substring(Link.IndexOf("]") + 1);
                    //Core = Core.Substring(0, Core.Length - 1);
                }
                else Core = Link;
                if (Core.IndexOf("(") > -1)
                {
                    Core = Core.Substring(Core.IndexOf("(") + 1);
                    if (Core.EndsWith(")"))
                        Core = Core.Substring(0, Core.Length - 1);
                }
                if (RemoveMarker && Core.IndexOf("#") > -1)
                {
                    Core = Core.Substring(0, Core.IndexOf("#"));
                }
                //else Core = Link;
                if (!Core.StartsWith("http"))
                {
                    if (Core.EndsWith("/"))
                        Core = Core.Substring(0, Core.Length - 1);
                    if (Core.IndexOf(".html") > -1)
                        Core = Core.Replace(".html", "");
                    if (Core.IndexOf(".htm") > -1)
                        Core = Core.Replace(".htm", "");
                    if (Core.IndexOf("/") > -1)
                    {
                        string[] Path = Core.Split(new char[] { '/' });
                        Core = Path[Path.Length - 1];
                        if (!AddModuleMarker && Core == "_index.md" && Path.Length == 2)
                            Core = Path[0];
                        if (Core.StartsWith("#") && Path.Length > 2)
                            Core = Path[Path.Length - 2];// + Core;
                    }
                    if (AddModuleMarker && !Core.ToLower().EndsWith(this.textBoxHugoLinksModuleMarker.Text.ToLower()))
                        Core += this.textBoxHugoLinksModuleMarker.Text;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Core;
        }

        private bool HugoLinksIsMatch(string Link)
        {
            bool IsMatch = true;
            if (Link.IndexOf("VideoDE.svg") > -1) { IsMatch = false; }
            if (Link.ToLower().IndexOf("videode.png") > -1) { IsMatch = false; }
            if (Link.IndexOf("{{") > -1 && Link.IndexOf("}}") > -1 && Link.IndexOf("}}") > Link.IndexOf("]")) { IsMatch = false; }
            if (Link.IndexOf("?") > -1) { IsMatch = false; }
            if (Link.IndexOf("](http") > -1) { IsMatch = false; }
            return IsMatch;
        }

        private string HugoLinksCheckIfFileExists(string Link)
        {
            string Key = "";
            if (Link.IndexOf("#") > -1) Link = Link.Substring(0, Link.IndexOf("#"));
            if (!Link.ToLower().EndsWith(this.textBoxHugoLinksModuleMarker.Text.ToLower()))
            {
                Link += this.textBoxHugoLinksModuleMarker.Text;
            }
            foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in _HugoLinkFiles)
            {
                if (KV.Value.Name.ToLower() == Link.ToLower()) { Key = KV.Key; break; }
            }
            if (Key.Length == 0)
            {
                System.Collections.Generic.List<string> LL = HugoLinkFileVariants(Link);
                foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> KV in _HugoLinkFiles)
                {
                    System.Collections.Generic.List<string> KK = HugoLinkKeyVariants(KV.Key);
                    foreach (string L in LL)
                    {
                        foreach (string K in KK)
                        {
                            if (L == K)
                            {
                                Key = KV.Key;
                                break;
                            }
                        }
                        if (Key.Length > 0) break;
                    }
                    if (Key.Length > 0) break;
                }
            }
            return Key;
        }

        /// <summary>
        /// Searching in the manual added translations for a match
        /// </summary>
        /// <param name="Link">Core if a link in the text derived from HugoLinkCore</param>
        /// <returns></returns>
        private string HugoLinksCheckIfTranslationExists(string Link)
        {
            string Key = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _HugoLinkTranslations)
            {
                string OriginalLinkedFile = HugoLinkCore(KV.Key, false, true);
                if (Link == OriginalLinkedFile)
                {
                    string File = HugoLinkCore(KV.Value, false, true);
                    Key = File;
                    break;
                }
            }
            //if (Link.IndexOf("#") > -1) Link = Link.Substring(0, Link.IndexOf("#"));
            //if (!Link.ToLower().EndsWith(this.textBoxHugoLinksModuleMarker.Text.ToLower()))
            //{
            //    //Link += this.textBoxHugoLinksModuleMarker.Text;
            //}
            //foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _HugoLinkTranslations)
            //{
            //    if (KV.Value.ToLower() == Link.ToLower()) 
            //    { Key = KV.Key; break; }
            //}
            //if (Key.Length == 0)
            //{
            //    System.Collections.Generic.List<string> LL = HugoLinkFileVariants(Link);
            //    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _HugoLinkTranslations)
            //    {
            //        System.Collections.Generic.List<string> KK = HugoLinkKeyVariants(KV.Key);
            //        foreach (string L in LL)
            //        {
            //            foreach (string K in KK)
            //            {
            //                if (L == K)
            //                {
            //                    Key = KV.Key;
            //                    break;
            //                }
            //            }
            //            if (Key.Length > 0) break;
            //        }
            //        if (Key.Length > 0) break;
            //    }
            //}
            return Key;
        }


        private System.Collections.Generic.List<string> HugoLinkFileVariants(string Link)
        {
            System.Collections.Generic.List<string> strings = new List<string>();
            Link = Link.ToLower();
            strings.Add(Link);
            string Parent = Link + "/_index.md";
            //if (!strings.Contains(Parent)) strings.Add(Parent);

            if (!strings.Contains(Link)) strings.Add(Link);
            Link = Link.Replace("_", "");
            if (!strings.Contains(Link)) strings.Add(Link);
            //Parent = Link + "/_index.md";
            //if (!strings.Contains(Parent)) strings.Add(Parent);

            Link = Link.Replace("-", "");
            if (!strings.Contains(Link)) strings.Add(Link);
            //Parent = Link + "/_index.md";
            //if (!strings.Contains(Parent)) strings.Add(Parent);

            return strings;
        }

        private System.Collections.Generic.List<string> HugoLinkKeyVariants(string Key)
        {
            System.Collections.Generic.List<string> strings = new List<string>();
            Key = Key.ToLower();
            if (Key.EndsWith(".md")) Key = Key.Substring(0, Key.Length - 3);

            if (Key.EndsWith("/_index")) Key = Key.Substring(0, Key.IndexOf("/_index"));

            if (Key.IndexOf("/") > -1) Key = Key.Substring(Key.LastIndexOf("/") + 1);

            strings.Add(Key);

            Key = Key.Replace("_", "");
            //if (Key.IndexOf("/index") > -1) { Key = Key.Replace("/index", "/_index"); }
            if (!strings.Contains(Key)) strings.Add(Key);

            Key = Key.Replace("-", "");
            if (!strings.Contains(Key)) strings.Add(Key);

            //if (Key.IndexOf("/") > -1 && Key.IndexOf("/_index.md") == -1)
            //{
            //    Key = Key.Substring(Key.IndexOf("/") + 1);
            //    if (!strings.Contains(Key)) strings.Add(Key);
            //}

            return strings;
        }

        #endregion

        #region broken links

        int iBrokenLinks = 0;
        private void buttonHugoLinksFixBrokenLinks_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                iBrokenLinks = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> file in _HugoLinkFiles)
                {
                    this.HugoLinksFixBrokenLinksInFile(file.Value);
                    i++;
                }
                this.labelHugoLinksFixBrokenLinks.Text = iBrokenLinks.ToString() + " Links in\r\n" + i.ToString() + " files fixed";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void HugoLinksFixBrokenLinksInFile(System.IO.FileInfo file)
        {
            try
            {
                System.Collections.Generic.Queue<string> Lines = HugoLinksFileContent(file);
                int iLines = Lines.Count;
                System.Collections.Generic.Queue<string> LinesFixed = new Queue<string>();
                if (Lines.Count > 0)
                {
                    string Line = Lines.Dequeue();
                    string NextLine = "";
                    if (Lines.Count > 0)
                        NextLine = Lines.Dequeue();
                    bool jumpToNextLine = false;
                    while (Lines.Count > 0)
                    {
                        if (Line.IndexOf(" [") > -1 && Line.IndexOf("](") == -1 && NextLine.IndexOf("](") > -1)
                        {
                            Line = Line + NextLine;
                            LinesFixed.Enqueue(Line);
                            Line = "";
                            NextLine = Lines.Dequeue();
                            jumpToNextLine = true;
                            iBrokenLinks++;
                            continue;
                        }
                        else
                        {
                            if (jumpToNextLine)
                                jumpToNextLine = false;
                            else
                                LinesFixed.Enqueue(Line);
                        }
                        Line = NextLine;
                        NextLine = Lines.Dequeue();
                    }
                    if (Line.IndexOf(" [") > -1 && Line.IndexOf("](") == -1 && NextLine.IndexOf("](") > -1)
                    {
                        LinesFixed.Enqueue(Line + NextLine);
                        iBrokenLinks++;
                    }
                    else
                    {
                        LinesFixed.Enqueue(Line);
                        LinesFixed.Enqueue(NextLine);
                    }
                    int iLinesFixed = LinesFixed.Count;
                    if (iLines > iLinesFixed)
                    {
                        using (StreamWriter sw = new StreamWriter(file.FullName, false))
                        {
                            while (LinesFixed.Count > 0)
                            {
                                sw.WriteLine(LinesFixed.Dequeue());
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        #endregion


        #region Searching missing links

        private string _MissingLink = "";
        private void dataGridViewHugoLinksMissing_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            _MissingLink = this.dataGridViewHugoLinksMissing.SelectedCells[0].Value.ToString();
        }
        private void buttonHugoLinksFindMissingFile_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;
            if (_MissingLink.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a link");
                return;
            }
            this.openFileDialog.Filter = "markdown files|*.md";
            this.openFileDialog.Title = "File corresponding to the link " + _MissingLink;
            this.openFileDialog.InitialDirectory = this._HugoLinkDir.FullName;
            this.openFileDialog.ShowDialog();
            if (this.openFileDialog.FileName.Length > 0 && this.openFileDialog.FileName != "openFileDialog")
            {
                try
                {
                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(this.openFileDialog.FileName);
                    if (fileInfo.Exists)
                    {
                        string Core = HugoLinkCore(_MissingLink);// + this.textBoxHugoLinksModuleMarker.Text;
                        string FileKey = fileInfo.Directory.Name + "/" + fileInfo.Name;
                        string LinkText = _MissingLink.Substring(0, _MissingLink.IndexOf("]") + 1);
                        string Marker = "";
                        if (_MissingLink.IndexOf("#") > -1) Marker = _MissingLink.Substring(_MissingLink.IndexOf("#"));
                        if (Marker.EndsWith(")")) Marker = Marker.Substring(0, Marker.Length - 1);
                        if (Marker.Length > 0)
                        {
                            DiversityWorkbench.Forms.FormDocumentationLinks formDocumentationLinks = new FormDocumentationLinks(fileInfo);
                            if (!formDocumentationLinks.Links.ContainsValue(Marker))
                            {
                                // Test Marker for tables
                                if (formDocumentationLinks.Links.ContainsValue(Marker.ToLower().Replace("#", "#table-")))
                                {
                                    Marker = Marker.ToLower().Replace("#", "#table-");
                                }
                                else
                                {
                                    formDocumentationLinks.ShowDialog();
                                    if (formDocumentationLinks.LinkMarker != null && formDocumentationLinks.LinkMarker.Length > 0)
                                    {
                                        Marker = formDocumentationLinks.LinkMarker;
                                    }
                                }
                            }
                        }
                        if (FileKey.Length > 0)
                        {
                            if (!_HugoLinks.ContainsKey(_MissingLink))
                            {
                                string Link = HugoLinkFromText(LinkText, FileKey, Marker);// HugoLinkFromText(Core);
                                _HugoLinks.Add(_MissingLink, Link);
                                _HugoLinksMissing.Remove(_MissingLink);
                                this.HugoLinksInitTables();
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("The link " + _MissingLink + " is already in the list.");
                            }
                        }

                        string HugoLink = HugoLinkFromText(fileInfo.Name);
                        _MissingLink = "";
                        this.dataGridViewHugoLinksMissing.Select();
                    }
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }
            }
        }

        private void buttonHugoLinksShowMissingFile_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewHugoLinksMissing.SelectedCells.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select an item in the table");
                return;
            }
            try
            {
                System.IO.FileInfo fileInfo = new FileInfo(this.dataGridViewHugoLinksMissing.Rows[this.dataGridViewHugoLinksMissing.SelectedCells[0].RowIndex].Cells[1].Value.ToString());
                if (fileInfo != null && fileInfo.Exists)
                {
                    Process.Start("notepad.exe", fileInfo.FullName);

                    //this.openFileDialog.InitialDirectory = fileInfo.DirectoryName;
                    //this.openFileDialog.Filter = "(" + fileInfo.Name + ")|*.md";
                    //this.openFileDialog.ShowDialog();
                    //this.openFileDialog.OpenFile();

                    //DiversityWorkbench.Forms.FormWebBrowser form = new FormWebBrowser(fileInfo.FullName);
                    //form.Text = "Link: " + this.dataGridViewHugoLinksMissing.Rows[this.dataGridViewHugoLinksMissing.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
                    //form.ShowDialog();
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }


        #endregion


        #region setting the new links
        private void buttonHugoLinksUpdateFiles_Click(object sender, EventArgs e)
        {
            try
            {
                int i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, System.IO.FileInfo> file in _HugoLinkFiles)
                {
                    this.HugoLinksReplaceLinksInFile(file.Value);
                    i++;
                }
                this.labelHugoLinksMessage.Text = i.ToString() + " files updated";
                // Requery of links
                this.HugoLinksGetLinks_Click(null, null);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void HugoLinksReplaceLinksInFile(System.IO.FileInfo file)
        {
            System.Collections.Generic.Queue<string> Lines = HugoLinksFileContent(file);
            System.Collections.Generic.Queue<string> LinesFixed = HugoLinksReplaceLinks(Lines);
            using (StreamWriter sw = new StreamWriter(file.FullName, false))
            {
                while (LinesFixed.Count > 0)
                {
                    sw.WriteLine(LinesFixed.Dequeue());
                }
            }
        }

        private System.Collections.Generic.Queue<string> HugoLinksFileContent(System.IO.FileInfo file)
        {
            System.Collections.Generic.Queue<string> Lines = new Queue<string>();
            try
            {
                using (StreamReader sr = file.OpenText())
                {
                    while (!sr.EndOfStream)
                    {
                        Lines.Enqueue(sr.ReadLine());
                    }
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Lines;
        }

        private System.Collections.Generic.Queue<string> HugoLinksReplaceLinks(System.Collections.Generic.Queue<string> strings)
        {
            System.Collections.Generic.Queue<string> queue = new Queue<string>();
            while (strings.Count > 0)
            {
                string line = strings.Dequeue();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _HugoLinks)
                {
                    if (line.Contains(KV.Key))
                        line = line.Replace(KV.Key, KV.Value);
                }
                queue.Enqueue(line);
            }
            return queue;
        }

        //private string HugoLinksReplaceLink(string Line, string SearchText, string Replacement)
        //{
        //    string fix = Line.Replace(SearchText, Replacement);
        //    return fix;
        //}

        #endregion

        #region Translations
        private void buttonHugoLinksAddTranslations_Click(object sender, EventArgs e)
        {
            if (_MissingLink.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a link");
                return;
            }

            if (this.listBoxHugoLinks.SelectedIndex > -1)
            {
                string file = this.listBoxHugoLinks.SelectedItem.ToString();
                if (!_HugoLinkTranslations.ContainsKey(_MissingLink))
                {
                    _HugoLinkTranslations.Add(_MissingLink, file);
                    dataGridViewHugoLinksTranslations_Init();
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select a file");
            }
        }

        private void dataGridViewHugoLinksTranslations_Init()
        {
            this.dataGridViewHugoLinksTranslations.Rows.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in _HugoLinkTranslations)
            {
                this.dataGridViewHugoLinksTranslations.Rows.Add(KV.Key, KV.Value);
            }
        }

        private System.Collections.Generic.Dictionary<string, string> _HugoLinkTranslations = new Dictionary<string, string>();

        private void dataGridViewHugoLinksTranslations_SizeChanged(object sender, EventArgs e)
        {
            this.dataGridViewHugoLinksTranslations.Columns[0].Width = this.dataGridViewHugoLinksTranslations.Width / 2;
            this.dataGridViewHugoLinksTranslations.Columns[1].Width = this.dataGridViewHugoLinksTranslations.Width / 2;
        }

        private void buttonHugoLinksClearTranslations_Click(object sender, EventArgs e)
        {
            _HugoLinkTranslations.Clear();
            dataGridViewHugoLinksTranslations_Init();
        }

        #endregion

        #endregion

    }
}
