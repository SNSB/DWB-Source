using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormApplicationEntity : Form
    {
        #region Parameter
        //private System.Data.DataTable _dtEntityList;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEntity;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEntityRepresentation;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEntityUsage;
        private System.Collections.Generic.List<string> _RemoveTablesFromList;
        private System.Collections.Generic.List<string> _RemoveColumnsFromList;
        private int _MaxAbbreviationLength = 20;

        //private System.Data.DataTable _dtLanguageCode;

        private string _ConnectionString;

        //private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEntityContext_Enum;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterWorkbenchEntityUsage_Enum;
        //private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterWorkbenchISO_Language_Enum;

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;

        #endregion

        #region Construction
        public FormApplicationEntity()
        {
            InitializeComponent();
            this.initForm();
            this.fillPickLists();
            //this.fillEntityList();
            this.userControlQueryList.IDisNumeric = false;
            this.initQuery();
            this.setDatabase();
            this._ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(); // ...Windows.Forms.Application.StartupPath + "\\" + DiversityWorkbench.Settings.ModuleName + ".chm";
        }

        #endregion

        #region Menu

        #region Entity

        private void insertAllMissingTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dtMissingTables = new DataTable();
            string SQL = "select T.TABLE_NAME from INFORMATION_SCHEMA.TABLES T " +
                "where T.TABLE_NAME not in " +
                "(" +
                "select Entity " +
                "from Entity " +
                "where Entity not like '%.%' " +
                ") " +
                "and T.TABLE_CATALOG = '" + DiversityWorkbench.Settings.DatabaseName + "' " +
                "and T.TABLE_TYPE = 'BASE TABLE' " +
                "and T.TABLE_SCHEMA = 'dbo' " +
                "order by T.TABLE_NAME";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtMissingTables);
            DiversityWorkbench.Forms.FormGetSelectionFromCheckedList f = new DiversityWorkbench.Forms.FormGetSelectionFromCheckedList(dtMissingTables,
                "TABLE_NAME", "Select tables",
                "Please select the tables you want to insert. This will insert the table together with the english description",
                this.RemoveTablesFromList);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataTable dtSelected = f.SelectedItems;
                foreach (System.Data.DataRow R in dtSelected.Rows)
                {
                    SQL = "INSERT INTO [Entity]([Entity]) " +
                        "VALUES ('" + R["TABLE_NAME"].ToString() + "')";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        C.ExecuteNonQuery();
                        string Description = DiversityWorkbench.Forms.FormFunctions.getTableDescription(R["TABLE_NAME"].ToString());
                        //C.CommandText = "INSERT INTO [EntityRepresentation] " +
                        //    "([Entity],[LanguageCode],[EntityContext],[DisplayText],[Abbreviation],[Description]) " +
                        //    "VALUES('" + R["TABLE_NAME"].ToString() + "', 'en', 'General', '"
                        //    + R["TABLE_NAME"].ToString() + "', "
                        //    + "SUBSTRING('" + R["TABLE_NAME"].ToString() + "', 1, 20), '" + Description + "')";
                        C.CommandText = "INSERT INTO [EntityRepresentation] " +
                            "([Entity],[LanguageCode],[EntityContext],[Description]) " +
                            "VALUES('" + R["TABLE_NAME"].ToString() + "', 'en-US', 'General', '" + Description + "')";
                        C.ExecuteNonQuery();
                        con.Close();
                    }
                    catch { }
                }
            }

            /*
            System.Data.DataTable dtMissingTables = new DataTable();
            string SQL = "select T.TABLE_NAME from INFORMATION_SCHEMA.TABLES T " +
                "where T.TABLE_NAME not in " +
                "(" +
                "select SUBSTRING(Entity, LEN('" + DiversityWorkbench.Settings.ModuleName + ".') + 1, 255) " +
                "from DiversityWorkbench.dbo.Entity " +
                "where SUBSTRING(Entity, LEN('" + DiversityWorkbench.Settings.ModuleName + ".') + 1, 255) not like '%.%' " +
                "and Entity like '" + DiversityWorkbench.Settings.ModuleName + ".%' " +
                ") " +
                "and T.TABLE_CATALOG = '" + DiversityWorkbench.Settings.DatabaseName + "' " +
                "and T.TABLE_TYPE = 'BASE TABLE' " +
                "and T.TABLE_SCHEMA = 'dbo' " +
                "order by T.TABLE_NAME";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtMissingTables);
            DiversityWorkbench.FormGetSelectionFromCheckedList f = new FormGetSelectionFromCheckedList(dtMissingTables, 
                "TABLE_NAME", "Select tables", 
                "Please select the tables you want to insert. This will insert the table together with the english description");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataTable dtSelected = f.SelectedItems;
                foreach (System.Data.DataRow R in dtSelected.Rows)
                {
                    SQL = "INSERT INTO [DiversityWorkbench].[dbo].[Entity]([Entity]) " +
                        "VALUES ('" + DiversityWorkbench.Settings.ModuleName + "." + R["TABLE_NAME"].ToString() + "')";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        C.ExecuteNonQuery();
                        string Description = DiversityWorkbench.Forms.FormFunctions.getTableDescription(R["TABLE_NAME"].ToString());
                        C.CommandText = "INSERT INTO [DiversityWorkbench].[dbo].[EntityRepresentation] " +
                            "([Entity],[LanguageCode],[EntityContext],[DisplayText],[Abbreviation],[Description]) " +
                            "VALUES('" + DiversityWorkbench.Settings.ModuleName + "." + R["TABLE_NAME"].ToString() + "', 'en', 'General', '" 
                            + R["TABLE_NAME"].ToString() + "', " 
                            + "SUBSTRING('" + R["TABLE_NAME"].ToString() + "', 1, 20), '" + Description + "')";
                        C.ExecuteNonQuery();
                        con.Close();
                    }
                    catch { }
                }
            }
            */
        }

        private void insertMissingColumnsForAllTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dtMissingEntities = new DataTable();
            string SQL = "select T.TABLE_NAME + '.' + C.COLUMN_NAME AS TABLE_COLUMN_NAME, C.COLUMN_NAME, T.TABLE_NAME, " +
                "C.COLUMN_NAME + replicate(' ', 2 * (20 - case when len(C.COLUMN_NAME) > 19 then 19 else len(C.COLUMN_NAME) end )) + '- ' + T.TABLE_NAME AS DisplayText " +
                "from INFORMATION_SCHEMA.TABLES T, INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = T.TABLE_NAME " +
                "and C.TABLE_CATALOG = T.TABLE_CATALOG " +
                "and C.TABLE_SCHEMA = T.TABLE_SCHEMA " +
                "AND T.TABLE_NAME + '.' + C.COLUMN_NAME " +
                "not in (select Entity from Entity) " +
                "and T.TABLE_CATALOG = '" + DiversityWorkbench.Settings.DatabaseName + "' " +
                "and T.TABLE_TYPE = 'BASE TABLE' " +
                "and T.TABLE_SCHEMA = 'dbo' " +
                "and T.TABLE_NAME " +
                "in (select Entity from Entity) " +
                "order by C.COLUMN_NAME";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtMissingEntities);
            DiversityWorkbench.Forms.FormGetSelectionFromCheckedList f = new DiversityWorkbench.Forms.FormGetSelectionFromCheckedList(dtMissingEntities,
                "DisplayText", "Select columns",
                "Please select the columns you want to insert. This will insert the column together with the english description",
                this.RemoveColumnsFromList);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataTable dtSelected = f.SelectedItems;
                foreach (System.Data.DataRow R in dtSelected.Rows)
                {
                    SQL = "INSERT INTO [Entity]([Entity]) " +
                        "SELECT ('" + R["TABLE_COLUMN_NAME"].ToString() + "') " +
                        "WHERE '" + R["TABLE_COLUMN_NAME"].ToString() + "' NOT IN (SELECT [Entity] FROM [Entity])";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        C.ExecuteNonQuery();
                        string Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(R["TABLE_NAME"].ToString(), R["COLUMN_NAME"].ToString(), true);
                        C.CommandText = "INSERT INTO [EntityRepresentation] " +
                            "([Entity],[LanguageCode],[EntityContext],[Description]) " +
                            "SELECT '" + R["TABLE_COLUMN_NAME"].ToString() + "', 'en-US', 'General', '" + Description + "' " +
                            "WHERE NOT EXISTS (SELECT * FROM [EntityRepresentation] " +
                            "WHERE [Entity] = '" + R["TABLE_COLUMN_NAME"].ToString() + "' AND [LanguageCode] = 'en-US')";
                        C.ExecuteNonQuery();
                        con.Close();
                    }
                    catch { }
                }
            }
        }

        private void insertMissingColumnsForSelectedTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.userControlQueryList.listBoxQueryResult.SelectedValue == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a table");
                return;
            }
            //string ModuleTable = this.userControlQueryList.listBoxQueryResult.SelectedValue.ToString();
            string Table = this.userControlQueryList.listBoxQueryResult.SelectedValue.ToString();// ModuleTable.Substring(DiversityWorkbench.Settings.ModuleName.Length + 1);
            if (Table.IndexOf('.') > -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a table");
                return;
            }
            System.Data.DataTable dtMissingEntities = new DataTable();
            string SQL = "select T.TABLE_NAME + '.' + C.COLUMN_NAME AS TABLE_COLUMN_NAME, C.COLUMN_NAME, T.TABLE_NAME " +
                "from INFORMATION_SCHEMA.TABLES T, INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = T.TABLE_NAME " +
                "and C.TABLE_CATALOG = T.TABLE_CATALOG " +
                "and C.TABLE_SCHEMA = T.TABLE_SCHEMA " +
                "and T.TABLE_NAME = '" + Table + "' " +
                "AND T.TABLE_NAME + '.' + C.COLUMN_NAME " +
                "not in (select Entity from Entity) " +
                "and T.TABLE_CATALOG = '" + DiversityWorkbench.Settings.DatabaseName + "' " +
                "and T.TABLE_TYPE = 'BASE TABLE' " +
                "and T.TABLE_SCHEMA = 'dbo' " +
                "and T.TABLE_NAME " +
                "in (select Entity from Entity) " +
                "order by T.TABLE_NAME + '.' + C.COLUMN_NAME";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtMissingEntities);
            DiversityWorkbench.Forms.FormGetSelectionFromCheckedList f = new DiversityWorkbench.Forms.FormGetSelectionFromCheckedList(dtMissingEntities,
                "COLUMN_NAME", "Select columns",
                "Please select the columns you want to insert. This will insert the column together with the english description",
                this.RemoveColumnsFromList);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataTable dtSelected = f.SelectedItems;
                foreach (System.Data.DataRow R in dtSelected.Rows)
                {
                    SQL = "INSERT INTO [Entity]([Entity]) " +
                        "VALUES ('" + R["TABLE_COLUMN_NAME"].ToString() + "')";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        C.ExecuteNonQuery();
                        string Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(R["TABLE_NAME"].ToString(), R["COLUMN_NAME"].ToString());
                        Description = Description.Replace("'", "' + char(39) + '");
                        //C.CommandText = "INSERT INTO [EntityRepresentation] " +
                        //    "([Entity],[LanguageCode],[EntityContext],[DisplayText],[Abbreviation],[Description]) " +
                        //    "VALUES('" + R["TABLE_COLUMN_NAME"].ToString() + "', 'en', 'General', '"
                        //    + R["COLUMN_NAME"].ToString() + "', '"
                        //    + R["COLUMN_NAME"].ToString() + "', '" + Description + "')";
                        C.CommandText = "INSERT INTO [EntityRepresentation] " +
                            "([Entity],[LanguageCode],[EntityContext],[Description]) " +
                            "VALUES('" + R["TABLE_COLUMN_NAME"].ToString() + "', 'en-US', 'General', '" + Description + "')";
                        C.ExecuteNonQuery();
                        con.Close();
                    }
                    catch { }
                }
            }

            /*
            if (this.userControlQueryList.listBoxQueryResult.SelectedValue == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a table");
                return;
            }
            string ModuleTable = this.userControlQueryList.listBoxQueryResult.SelectedValue.ToString();
            string Table = ModuleTable.Substring(DiversityWorkbench.Settings.ModuleName.Length + 1);
            if (!ModuleTable.StartsWith(DiversityWorkbench.Settings.ModuleName)
                || Table.IndexOf('.') > -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a table");
                return;
            }
            System.Data.DataTable dtMissingEntities = new DataTable();
            string SQL = "select T.TABLE_NAME + '.' + C.COLUMN_NAME AS TABLE_COLUMN_NAME, C.COLUMN_NAME, T.TABLE_NAME " +
                "from INFORMATION_SCHEMA.TABLES T, INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = T.TABLE_NAME " +
                "and C.TABLE_CATALOG = T.TABLE_CATALOG " +
                "and C.TABLE_SCHEMA = T.TABLE_SCHEMA " +
                "and T.TABLE_NAME = '" + Table + "' " +
                "AND '" + DiversityWorkbench.Settings.ModuleName + ".' + T.TABLE_NAME + '.' + C.COLUMN_NAME " +
                "not in (select Entity from DiversityWorkbench.dbo.Entity) " +
                "and T.TABLE_CATALOG = '" + DiversityWorkbench.Settings.DatabaseName + "' " +
                "and T.TABLE_TYPE = 'BASE TABLE' " +
                "and T.TABLE_SCHEMA = 'dbo' " +
                "and '" + DiversityWorkbench.Settings.ModuleName + ".' + T.TABLE_NAME " +
                "in (select Entity from DiversityWorkbench.dbo.Entity) " +
                "order by T.TABLE_NAME + '.' + C.COLUMN_NAME";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtMissingEntities);
            DiversityWorkbench.FormGetSelectionFromCheckedList f = new FormGetSelectionFromCheckedList(dtMissingEntities,
                "COLUMN_NAME", "Select columns",
                "Please select the columns you want to insert. This will insert the column together with the english description");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                System.Data.DataTable dtSelected = f.SelectedItems;
                foreach (System.Data.DataRow R in dtSelected.Rows)
                {
                    SQL = "INSERT INTO [DiversityWorkbench].[dbo].[Entity]([Entity]) " +
                        "VALUES ('" + DiversityWorkbench.Settings.ModuleName + "." + R["TABLE_COLUMN_NAME"].ToString() + "')";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        C.ExecuteNonQuery();
                        string Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(R["TABLE_NAME"].ToString(), R["COLUMN_NAME"].ToString());
                        Description = Description.Replace("'", "' + char(39) + '");
                        C.CommandText = "INSERT INTO [DiversityWorkbench].[dbo].[EntityRepresentation] " +
                            "([Entity],[LanguageCode],[EntityContext],[DisplayText],[Abbreviation],[Description]) " +
                            "VALUES('" + DiversityWorkbench.Settings.ModuleName + "." + R["TABLE_COLUMN_NAME"].ToString() + "', 'en', 'General', '"
                            + R["COLUMN_NAME"].ToString() + "', '"
                            + R["COLUMN_NAME"].ToString() + "', '" + Description + "')";
                        C.ExecuteNonQuery();
                        con.Close();
                    }
                    catch { }
                }
            }
             * */
        }

        private void insertPKForSelectedTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.userControlQueryList.listBoxQueryResult.SelectedValue == null)
            {
                System.Windows.Forms.MessageBox.Show(this.Message("Please_select_a_table")); //"Please select a table");
                return;
            }
            //string ModuleTable = this.userControlQueryList.listBoxQueryResult.SelectedValue.ToString();
            string Table = this.userControlQueryList.listBoxQueryResult.SelectedValue.ToString();// ModuleTable.Substring(DiversityWorkbench.Settings.ModuleName.Length + 1);
            if (Table.IndexOf('.') > -1)
            {
                System.Windows.Forms.MessageBox.Show(this.Message("Please_select_a_table")); //"Please select a table");
                return;
            }
            DiversityWorkbench.Forms.FormEntityInsertPK f = new FormEntityInsertPK(Table);
            f.ShowDialog();
            if (f.DialogResult != DialogResult.OK) return;
            string ColumnPK = f.ColumnForPrimaryKey;
            string ColumnForDisplayText = f.ColumnForDisplayText;
            string ColumnForAbbreviation = f.ColumnForAbbreviation;
            string ColumnForDescription = f.ColumnForDescription;
            string SQL = "INSERT INTO [Entity]([Entity]) " +
            "SELECT '" + Table + "." + ColumnPK + ".' + CAST([" + ColumnPK + "] AS VARCHAR) " +
            "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[" + Table + "] " +
            " WHERE '" + Table + "." + ColumnPK + ".' + CAST([" + ColumnPK + "] AS VARCHAR) NOT IN (SELECT [Entity]FROM [Entity])";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                C.ExecuteNonQuery();
                C.CommandText = "INSERT INTO [EntityRepresentation] " +
                    "([Entity],[LanguageCode],[EntityContext],[DisplayText],[Abbreviation],[Description]) " +
                    "SELECT '" + Table + "." + ColumnPK + ".' + CAST([" + ColumnPK + "] AS VARCHAR) " +
                    ",'en-US', CAST([Code] AS VARCHAR),[" + ColumnForDisplayText + "], SUBSTRING([" + ColumnForAbbreviation + "], 1, 20), [" + ColumnForDescription + "] " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[" + Table + "]  C " +
                    "WHERE NOT EXISTS (select * from [EntityRepresentation] r " +
                    "where r.[Entity] = '" + Table + "." + ColumnPK + ".' + CAST([" + ColumnPK + "] AS VARCHAR) " +
                    "and r.[LanguageCode] = 'en-US' " +
                    "and  r.[EntityContext] = CAST([Code] AS VARCHAR))";
                C.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex) { }
        }

        //private string Abbreviation(string Original)
        //{
        //    string Abbrev = "";
        //    string[] AA = this.DisplayText(Original).Split(new char[]{' '});
        //    if (AA.Length == 1)
        //    {
        //        Abbrev = AA[0];
        //        if (Abbrev.Length > this._MaxAbbreviationLength)
        //        {
        //        }
        //    }
        //    else
        //    {
        //        for (int i = AA.Length - 1; i > 0; i--)
        //        {

        //        }
        //    }
        //    return Abbrev;
        //}

        //private string DisplayText(string Original)
        //{
        //    string Display = "";
        //    for (int i = 0; i < Original.Length; i++)
        //    {
        //        if (Original[i].ToString() == Original[i].ToString().ToUpper()
        //            && i > 0)
        //        {
        //            if (Original.Length > i + 1 
        //                && 
        //                Original[i+1].ToString().ToUpper() == Original[i+1].ToString())
        //            Display += " " + Original[i].ToString().ToLower();
        //        }
        //        else
        //        {
        //            Display += Original[i].ToString();
        //        }
        //    }
        //    return Display;
        //}

        #endregion

        #region Representation

        private void insertMissingLanguageEntriesForAllEntitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dtLang = this.dataSetEntity.EntityLanguageCode_Enum.Copy();
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtLang, "DisplayText", "Code", "Language", "Please select a language");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                string SQL = "INSERT INTO [EntityRepresentation] " +
                    "([Entity],[LanguageCode],[EntityContext],[DisplayText]) " +
                    "SELECT Present.[Entity],'" + f.SelectedValue + "',Present.[EntityContext],Present.DisplayText " +
                    "FROM [EntityRepresentation] Present " +
                    "WHERE Present.LanguageCode = 'en-US' " +
                    "AND NOT EXISTS (SELECT * FROM [EntityRepresentation] Missing " +
                    "WHERE Present.Entity = Missing.Entity AND Present.EntityContext = Missing.EntityContext " +
                    "AND Missing.LanguageCode = '" + f.SelectedValue + "')";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                }
                catch { }
            }
            /*
            System.Data.DataTable dtLang = this.dataSetEntity.WorkbenchISO_Language_Enum.Copy();
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtLang, "Description", "Code", "Language", "Please select a language");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                string SQL = "INSERT INTO [DiversityWorkbench].[dbo].[EntityRepresentation] " +
                    "([Entity],[LanguageCode],[EntityContext],[DisplayText]) " +
                    "SELECT Present.[Entity],'" + f.SelectedValue + "',Present.[EntityContext],Present.DisplayText " +
                    "FROM [DiversityWorkbench].[dbo].[EntityRepresentation] Present " +
                    "WHERE Present.LanguageCode = 'en' " +
                    "AND NOT EXISTS (SELECT * FROM [DiversityWorkbench].[dbo].[EntityRepresentation] Missing " +
                    "WHERE Present.Entity = Missing.Entity AND Present.EntityContext = Missing.EntityContext " +
                    "AND Missing.LanguageCode = '" + f.SelectedValue + "')";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    C.ExecuteNonQuery();
                    con.Close();
                }
                catch { }
            }
             * */
        }

        private void updateDescriptionsAccordingToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // reduce to the update of one table. There may be changes in the entity that should be kept
            if (this.userControlQueryList.listBoxQueryResult.SelectedValue == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a table");
                return;
            }
            string Table = this.userControlQueryList.listBoxQueryResult.SelectedValue.ToString();// ModuleTable.Substring(DiversityWorkbench.Settings.ModuleName.Length + 1);
            if (Table.IndexOf('.') > -1)
            {
                System.Windows.Forms.MessageBox.Show("Please select a table");
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Data.DataTable dtEntity = new DataTable();
            string SQL = "SELECT [Entity] FROM [Entity] WHERE Entity LIKE '%" + Table + "%'";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtEntity);
            foreach (System.Data.DataRow R in dtEntity.Rows)
            {
                string Description = "";
                string Entity = R[0].ToString();
                string[] EntityParts = Entity.Split(new char[] { '.' });
                if (EntityParts.Length == 1)
                {
                    Description = DiversityWorkbench.Forms.FormFunctions.getTableDescription(EntityParts[0]);
                }
                else if (EntityParts.Length == 2)
                {
                    Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(EntityParts[0], EntityParts[1]);
                }
                if (Description.Length > 0)
                {
                    Description = Description.Replace("'", "' + CHAR(39) + '");
                    SQL = "UPDATE R SET Description = '" + Description + "' " +
                        "FROM [EntityRepresentation] R " +
                        "WHERE R.EntityContext = 'General' AND R.LanguageCode = 'en-US' AND Entity = '" + Entity + "' " +
                        "AND (R.Description <> '" + Description + "' OR R.Description IS NULL)";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        C.ExecuteNonQuery();
                        con.Close();
                    }
                    catch { }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;







            //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //System.Data.DataTable dtEntity = new DataTable();
            //string SQL = "SELECT [Entity] FROM [Entity]";
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //ad.Fill(dtEntity);
            //foreach (System.Data.DataRow R in dtEntity.Rows)
            //{
            //    string Description = "";
            //    string Entity = R[0].ToString();
            //    //if (Entity.StartsWith(DiversityWorkbench.Settings.ModuleName))
            //    //{
            //        string[] EntityParts = Entity.Split(new char[] { '.' });
            //        if (EntityParts.Length == 1)
            //        {
            //            Description = DiversityWorkbench.Forms.FormFunctions.getTableDescription(EntityParts[0]);
            //        }
            //        else if (EntityParts.Length == 2)
            //        {
            //            Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(EntityParts[0], EntityParts[1]);
            //        }
            //        if (Description.Length > 0)
            //        {
            //            Description = Description.Replace("'", "' + CHAR(39) + '");
            //            SQL = "UPDATE R SET Description = '" + Description + "' " +
            //                "FROM [EntityRepresentation] R " +
            //                "WHERE R.EntityContext = 'General' AND R.LanguageCode = 'en-US' AND Entity = '" + Entity + "' " +
            //                "AND (R.Description <> '" + Description + "' OR R.Description IS NULL)";
            //            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            //            try
            //            {
            //                con.Open();
            //                C.ExecuteNonQuery();
            //                con.Close();
            //            }
            //            catch { }
            //        }
            //    //}
            //}
            //this.Cursor = System.Windows.Forms.Cursors.Default;
            /*
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Data.DataTable dtEntity = new DataTable();
            string SQL = "SELECT [Entity] FROM [DiversityWorkbench].[dbo].[Entity]";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtEntity);
            foreach (System.Data.DataRow R in dtEntity.Rows)
            {
                string Description = "";
                string Entity = R[0].ToString();
                if (Entity.StartsWith(DiversityWorkbench.Settings.ModuleName))
                {
                    string[] EntityParts = Entity.Split(new char[] { '.' });
                    if (EntityParts.Length == 2)
                    {
                        Description = DiversityWorkbench.Forms.FormFunctions.getTableDescription(EntityParts[1]);
                    }
                    else if (EntityParts.Length == 3)
                    {
                        Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(EntityParts[1], EntityParts[2]);
                    }
                    if (Description.Length > 0)
                    {
                        SQL = "UPDATE R SET Description = '" + Description + "' " +
                            "FROM [DiversityWorkbench].[dbo].[EntityRepresentation] R " +
                            "WHERE R.EntityContext = 'General' AND R.LanguageCode = 'en' AND Entity = '" + Entity + "' " +
                            "AND R.Description <> '" + Description + "'";
                        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                        try
                        {
                            con.Open();
                            C.ExecuteNonQuery();
                            con.Close();
                        }
                        catch { }
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
             * */
        }

        private void updateDatabaseAccordingToDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //System.Windows.Forms.MessageBox.Show("ToDo");
            try
            {
                System.Data.DataRow[] RR = this.dataSetEntity.EntityRepresentation.Select("EntityContext = 'General' AND LanguageCode = 'en-US'");
                if (RR.Length == 0) return;
                if (RR[0]["Description"].Equals(System.DBNull.Value)) return;
                if (RR[0]["Description"].ToString().Length == 0) return;
                string Description = RR[0]["Description"].ToString();

                if (this.userControlQueryList.listBoxQueryResult.SelectedValue == null) return;
                string Entity = this.userControlQueryList.listBoxQueryResult.SelectedValue.ToString();// ModuleTable.Substring(DiversityWorkbench.Settings.ModuleName.Length + 1);
                string[] EntityParts = Entity.Split(new char[] { '.' });
                if (EntityParts.Length > 2) return;
                string Table = EntityParts[0];
                string Column = "";
                if (EntityParts.Length > 1) Column = EntityParts[1];

                string SQL = "EXEC updateextendedproperty  " +
                    "@name=N'MS_Description', " +
                    "@value = '" + Description + "', " +
                    "@level0type = N'Schema', " +
                    "@level0name = N'dbo', " +
                    "@level1type = N'Table',  " +
                    "@level1name = N'" + Table + "'";
                if (Column.Length > 0)
                    SQL += ", @level2type = N'Column', @level2name = N'" + Column + "'";

                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                C.ExecuteNonQuery();
                con.Close();
                System.Windows.Forms.MessageBox.Show("Description updated");
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Update failed");
            }
            return;
        }

        #endregion

        #region Usage
        private void insertUsageForTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Context = "";
            System.Data.DataTable dtContext = new DataTable();
            string SQL = "SELECT Code FROM EntityContext_Enum WHERE DisplayEnable = 1 ORDER BY Code";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtContext);
            DiversityWorkbench.Forms.FormGetStringFromList fContext = new DiversityWorkbench.Forms.FormGetStringFromList(dtContext, "Code", "Code", "Context for new usage", "Select the context for which you want to insert the usage");
            fContext.ShowDialog();
            if (fContext.DialogResult == DialogResult.OK)
            {
                Context = fContext.SelectedValue.ToString();
                string Usage = "";
                System.Data.DataTable dtUsage = new DataTable();
                SQL = "SELECT Code FROM  EntityUsage_Enum WHERE DisplayEnable = 1 ORDER BY Code";
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dtUsage);
                DiversityWorkbench.Forms.FormGetStringFromList fUsage = new DiversityWorkbench.Forms.FormGetStringFromList(dtUsage, "Code", "Code", "New usage", "Please select the new usage you want to insert");
                fUsage.ShowDialog();
                if (fUsage.DialogResult == DialogResult.OK)
                {
                    Usage = fUsage.SelectedValue.ToString();
                    System.Data.DataTable dtTables = new DataTable();
                    SQL = "SELECT DISTINCT Entity " +
                        "FROM Entity AS E " +
                        "WHERE (Entity NOT LIKE '%.%') AND (Entity NOT IN " +
                        "(SELECT Entity FROM EntityUsage AS U WHERE (EntityContext = '" + Context + "'))) " +
                        "ORDER BY Entity";
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtTables);
                    System.Collections.Generic.List<string> UsageForTables = new List<string>();
                    DiversityWorkbench.Forms.FormGetSelectionFromCheckedList f = new DiversityWorkbench.Forms.FormGetSelectionFromCheckedList(
                        dtTables,
                        "Entity",
                        "Select tables",
                        "Please select the tables for which you want to insert the usage " + Usage + " within the context " + Context);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        System.Data.DataTable dtSelected = f.SelectedItems;
                        foreach (System.Data.DataRow R in dtSelected.Rows)
                        {
                            SQL = "INSERT INTO [EntityUsage]([Entity],[EntityContext],[EntityUsage]) " +
                                "VALUES ('" + R[0].ToString() + "' " +
                                ", '" + Context + "' " +
                                ", '" + Usage + "')";
                            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                            try
                            {
                                con.Open();
                                C.ExecuteNonQuery();
                                con.Close();
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private void insertUsageForColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string Context = "";
            System.Data.DataTable dtContext = new DataTable();
            string SQL = "SELECT Code FROM EntityContext_Enum WHERE DisplayEnable = 1 ORDER BY Code";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtContext);
            DiversityWorkbench.Forms.FormGetStringFromList fContext = new DiversityWorkbench.Forms.FormGetStringFromList(dtContext, "Code", "Code", "Context for new usage", "Select the context for which you want to insert the usage");
            fContext.ShowDialog();
            if (fContext.DialogResult == DialogResult.OK)
            {
                Context = fContext.SelectedValue.ToString();
                string Usage = "";
                System.Data.DataTable dtUsage = new DataTable();
                SQL = "SELECT Code FROM  EntityUsage_Enum WHERE DisplayEnable = 1 ORDER BY Code";
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dtUsage);
                DiversityWorkbench.Forms.FormGetStringFromList fUsage = new DiversityWorkbench.Forms.FormGetStringFromList(dtUsage, "Code", "Code", "New usage", "Please select the new usage you want to insert");
                fUsage.ShowDialog();
                if (fUsage.DialogResult == DialogResult.OK)
                {
                    Usage = fUsage.SelectedValue.ToString();
                    System.Data.DataTable dtTables = new DataTable();
                    SQL = "SELECT DISTINCT Entity " +
                        "FROM Entity AS E " +
                        "WHERE (Entity LIKE '%.%') AND (Entity NOT LIKE '%.%.%') AND (Entity NOT IN " +
                        "(SELECT Entity FROM EntityUsage AS U WHERE (EntityContext = '" + Context + "'))) " +
                        "ORDER BY Entity";
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtTables);
                    System.Collections.Generic.List<string> UsageForTables = new List<string>();
                    DiversityWorkbench.Forms.FormGetSelectionFromCheckedList f = new DiversityWorkbench.Forms.FormGetSelectionFromCheckedList(
                        dtTables,
                        "Entity",
                        "Select columns",
                        "Please select the columns for which you want to insert the usage " + Usage + " within the context " + Context);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        string Preset = "";
                        DiversityWorkbench.Forms.FormGetString fPreset = new FormGetString("Preset value", "If you want to enter a preset value for the columns, enter it here", "");
                        fPreset.ShowDialog();
                        if (fPreset.DialogResult == DialogResult.OK)
                            Preset = fPreset.String.Trim();
                        System.Data.DataTable dtSelected = f.SelectedItems;
                        foreach (System.Data.DataRow R in dtSelected.Rows)
                        {
                            SQL = "INSERT INTO [EntityUsage]([Entity],[EntityContext],[EntityUsage],PresetValue) " +
                                "VALUES ('" + R[0].ToString() + "' " +
                                ", '" + Context + "' " +
                                ", '" + Usage + "' " +
                                ", ";
                            if (Preset.Length > 0)
                                SQL += "'" + Preset + "')";
                            else SQL += " NULL)";
                            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                            try
                            {
                                con.Open();
                                C.ExecuteNonQuery();
                                con.Close();
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        #endregion

        #region Administration

        private void contextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT [Code],[Description],[DisplayText],[DisplayOrder],[DisplayEnable],[InternalNotes], " +
            "[ParentCode] FROM [EntityContext_Enum]";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.Forms.FormEditTable f = new FormEditTable(ad, "Edit context", "Please edit the contexts and click OK to save your changes and close the window");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.dataSetEntity.EntityContext_Enum.Clear();
                ad.Fill(this.dataSetEntity.EntityContext_Enum);
                //this.entityContext_EnumTableAdapter.Fill(this.dataSetEntity.EntityContext_Enum);
            }
            /*
            string SQL = "SELECT [Code],[Description],[DisplayText],[DisplayOrder],[DisplayEnable],[InternalNotes], " +
            "[ParentCode] FROM [DiversityWorkbench].[dbo].[EntityContext_Enum]";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.FormEditTable f = new FormEditTable(ad, "Edit context", "Please edit the contexts and click OK to save your changes and close the window");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.dataSetEntity.EntityContext_Enum.Clear();
                this.entityContext_EnumTableAdapter.Fill(this.dataSetEntity.EntityContext_Enum);
            }
             * */
        }

        private void usageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string SQL = "SELECT [Code],[Description],[DisplayText],[DisplayOrder],[DisplayEnable],[InternalNotes], " +
            //"[ParentCode] FROM [EntityUsage_Enum]";
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //DiversityWorkbench.FormEditTable f = new FormEditTable(ad, "Edit usage", "Please edit the usages and click OK to save your changes and close the window");
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK)
            //{
            //    //this.dataSetEntity.EntityUsage_Enum.Clear();
            //    //this.entityUsage_EnumTableAdapter.Fill(this.dataSetEntity.EntityUsage_Enum);
            //}


            /*
            string SQL = "SELECT [Code],[Description],[DisplayText],[DisplayOrder],[DisplayEnable],[InternalNotes], " +
            "[ParentCode] FROM [DiversityWorkbench].[dbo].[EntityUsage_Enum]";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.FormEditTable f = new FormEditTable(ad, "Edit usage", "Please edit the usages and click OK to save your changes and close the window");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.dataSetEntity.EntityUsage_Enum.Clear();
                this.entityUsage_EnumTableAdapter.Fill(this.dataSetEntity.EntityUsage_Enum);
            }
             * */
        }

        private void accessibilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT [Code],[Description],[DisplayText],[DisplayOrder],[DisplayEnable],[InternalNotes], " +
            "[ParentCode] FROM [EntityAccessibility_Enum]";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.Forms.FormEditTable f = new FormEditTable(ad, "Edit usage", "Please edit the accessibility and click OK to save your changes and close the window");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.dataSetEntity.EntityAccessibility_Enum.Clear();
                this.entityAccessibility_EnumTableAdapter.Fill(this.dataSetEntity.EntityAccessibility_Enum);
            }
        }

        private void determinationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT [Code],[Description],[DisplayText],[DisplayOrder],[DisplayEnable],[InternalNotes], " +
            "[ParentCode] FROM [EntityDetermination_Enum]";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.Forms.FormEditTable f = new DiversityWorkbench.Forms.FormEditTable(ad, "Edit usage", "Please edit the determination and click OK to save your changes and close the window");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.dataSetEntity.EntityDetermination_Enum.Clear();
                this.entityDetermination_EnumTableAdapter.Fill(this.dataSetEntity.EntityDetermination_Enum);
            }
        }

        private void visibilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT [Code],[Description],[DisplayText],[DisplayOrder],[DisplayEnable],[InternalNotes], " +
            "[ParentCode] FROM [EntityVisibility_Enum]";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.Forms.FormEditTable f = new DiversityWorkbench.Forms.FormEditTable(ad, "Edit usage", "Please edit the visibility and click OK to save your changes and close the window");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.dataSetEntity.EntityVisibility_Enum.Clear();
                this.entityVisibility_EnumTableAdapter.Fill(this.dataSetEntity.EntityVisibility_Enum);
            }
        }



        #endregion        

        #endregion

        #region Form etc.

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
            this.dataGridViewUsage.Columns[0].Width = 0;
            this.dataGridViewUsage.Columns[0].Visible = false;
            this.dataGridViewUsage.Columns[1].Width = (int)((this.dataGridViewUsage.Width - this.dataGridViewUsage.RowHeadersWidth) * 0.15);
            this.dataGridViewUsage.Columns[2].Width = (int)((this.dataGridViewUsage.Width - this.dataGridViewUsage.RowHeadersWidth) * 0.15);
            this.dataGridViewUsage.Columns[3].Width = (int)((this.dataGridViewUsage.Width - this.dataGridViewUsage.RowHeadersWidth) * 0.15);
            this.dataGridViewUsage.Columns[4].Width = (int)((this.dataGridViewUsage.Width - this.dataGridViewUsage.RowHeadersWidth) * 0.15);
            this.dataGridViewUsage.Columns[5].Width = (int)((this.dataGridViewUsage.Width - this.dataGridViewUsage.RowHeadersWidth) * 0.15);
            this.dataGridViewUsage.Columns[6].Width = (int)((this.dataGridViewUsage.Width - this.dataGridViewUsage.RowHeadersWidth) * 0.25);
        }

        public void setHelpProviderPath(string Path)
        {
            this.helpProvider.HelpNamespace = Path;
        }

        private void setAutoCompletion()
        {
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxDisplayGroup);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxEntityNotes);
        }

        private void fillPickLists()
        {
            //string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_NAME = 'WorkbenchISO_Language_Enum'";
            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            //int i;
            //con.Open();
            //int.TryParse(C.ExecuteScalar()?.ToString(), out i);
            //con.Close();
            //SQL = "SELECT cast(RTRIM(LTRIM([Code])) as varchar(5)) AS [Code] ,MAX([Description]) AS Description " +
            //    "FROM [WorkbenchISO_Language_Enum] " +
            //    "WHERE Code NOT LIKE '%?%' AND Code NOT LIKE '%-%' " +
            //    "GROUP BY RTRIM(LTRIM([Code]))";
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            //if (i == 1)
            //{
            //    try
            //    {
            //        this.dataSetEntity.EntityLanguageCode_Enum.Clear();
            //        ad.Fill(this.dataSetEntity.EntityLanguageCode_Enum);
            //    }
            //    catch { }
            //}
            //else
            //{
            //try
            //{
            //    this.dataSetEntity.EntityLanguageCode_Enum.Clear();
            //    if (this.dataSetEntity.EntityLanguageCode_Enum.Rows.Count == 0)
            //    {
            //        ad.SelectCommand.CommandText = "SELECT cast(substring(Code, 1, 5) as varchar(5)) AS [Code], DisplayText AS [Description] FROM LanguageCode_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText";
            //        ad.Fill(this.dataSetEntity.EntityLanguageCode_Enum);
            //    }
            //}
            //catch { }
            //}
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                this.entityLanguageCode_EnumTableAdapter.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                this.entityLanguageCode_EnumTableAdapter.Fill(this.dataSetEntity.EntityLanguageCode_Enum);

                //Microsoft.Data.SqlClient.SqlDataAdapter adLang = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, InternalNotes, ParentCode " +
                //    "FROM EntityLanguageCode_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText", DiversityWorkbench.Settings.ConnectionString);
                //adLang.Fill(this.dataSetEntity.EntityLanguageCode_Enum);

                //Microsoft.Data.SqlClient.SqlDataAdapter adUsage = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, InternalNotes, ParentCode " +
                //    "FROM EntityUsage_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText", DiversityWorkbench.Settings.ConnectionString);
                //adUsage.Fill(this.dataSetEntity.EntityUsage_Enum);

                Microsoft.Data.SqlClient.SqlDataAdapter adAccessibility = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, InternalNotes, ParentCode " +
                    "FROM EntityAccessibility_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText", DiversityWorkbench.Settings.ConnectionString);
                adAccessibility.Fill(this.dataSetEntity.EntityAccessibility_Enum);

                Microsoft.Data.SqlClient.SqlDataAdapter adDetermination = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, InternalNotes, ParentCode " +
                    "FROM EntityDetermination_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText", DiversityWorkbench.Settings.ConnectionString);
                adDetermination.Fill(this.dataSetEntity.EntityDetermination_Enum);

                Microsoft.Data.SqlClient.SqlDataAdapter adVisibility = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, InternalNotes, ParentCode " +
                    "FROM EntityVisibility_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText", DiversityWorkbench.Settings.ConnectionString);
                adVisibility.Fill(this.dataSetEntity.EntityVisibility_Enum);

                Microsoft.Data.SqlClient.SqlDataAdapter adContext = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, InternalNotes, ParentCode " +
                    "FROM EntityContext_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText", DiversityWorkbench.Settings.ConnectionString);
                adContext.Fill(this.dataSetEntity.EntityContext_Enum);

                this.setEntityList();
            }

            //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //this.entityUsage_EnumTableAdapter.Adapter.SelectCommand = new Microsoft.Data.SqlClient.SqlCommand("SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, InternalNotes, ParentCode " +
            //    "FROM EntityUsage_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText", con);
            //this.entityContext_EnumTableAdapter.Adapter.SelectCommand = new Microsoft.Data.SqlClient.SqlCommand("SELECT Code, Description, DisplayText, DisplayOrder, DisplayEnable, InternalNotes, ParentCode " +
            //    "FROM EntityContext_Enum WHERE DisplayEnable = 1 ORDER BY DisplayText", con);
            ////this.entityContext_EnumTableAdapter.Adapter.SelectCommand.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            ////this.entityUsage_EnumTableAdapter.Adapter.SelectCommand.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            //this.entityUsage_EnumTableAdapter.Fill(this.dataSetEntity.EntityUsage_Enum);
            //this.entityContext_EnumTableAdapter.Fill(this.dataSetEntity.EntityContext_Enum);
        }

        private void setEntityList()
        {
            this.dataSetEntity.EntityList.Clear();
            Microsoft.Data.SqlClient.SqlDataAdapter adEntity = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT NULL AS Entity " +
                "UNION SELECT Entity FROM Entity ORDER BY Entity", DiversityWorkbench.Settings.ConnectionString);
            adEntity.Fill(this.dataSetEntity.EntityList);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxDisplayGroup);
        }

        private void FormApplicationEntity_Load(object sender, EventArgs e)
        {
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetEntity.EntityVisibility_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.entityVisibility_EnumTableAdapter.Fill(this.dataSetEntity.EntityVisibility_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetEntity.EntityDetermination_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.entityDetermination_EnumTableAdapter.Fill(this.dataSetEntity.EntityDetermination_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetEntity.EntityAccessibility_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.entityAccessibility_EnumTableAdapter.Fill(this.dataSetEntity.EntityAccessibility_Enum);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetEntity.EntityRepresentation". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.entityRepresentationTableAdapter.Fill(this.dataSetEntity.EntityRepresentation);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetEntity.EntityUsage". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.entityUsageTableAdapter.Fill(this.dataSetEntity.EntityUsage);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetEntity.EntityList". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.entityListTableAdapter.Fill(this.dataSetEntity.EntityList);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetEntity.EntityLanguageCode_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.entityLanguageCode_EnumTableAdapter.Fill(this.dataSetEntity.EntityLanguageCode_Enum);
            ////this.workbenchISO_Language_EnumTableAdapter.Fill(this.dataSetEntity.WorkbenchISO_Language_Enum);
        }

        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private void dataGridViewRepresentation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridViewUsage_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void FormApplicationEntity_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.updateEntity();
        }

        private string Message(string Resource)
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Forms.FormApplicationEntityMessages));
            string Message = resources.GetString(Resource);
            return Message;
        }

        private void feedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        #endregion

        #region Properties

        private System.Collections.Generic.List<string> RemoveTablesFromList
        {
            get
            {
                if (this._RemoveTablesFromList == null)
                {
                    this._RemoveTablesFromList = new List<string>();
                    //this._RemoveTablesFromList.Add("*_Enum");
                    this._RemoveTablesFromList.Add("*_log");
                    //this._RemoveTablesFromList.Add("Application*");
                    //this._RemoveTablesFromList.Add("Workbench*");
                    //this._RemoveTablesFromList.Add("Entity*");
                    this._RemoveTablesFromList.Add("*_log_*");
                    this._RemoveTablesFromList.Add("sysdiagrams");
                    this._RemoveTablesFromList.Add("dtproperties");
                    this._RemoveTablesFromList.Add("xx_*");
                }
                return _RemoveTablesFromList;
            }
        }

        private System.Collections.Generic.List<string> RemoveColumnsFromList
        {
            get
            {
                if (this._RemoveColumnsFromList == null)
                {
                    this._RemoveColumnsFromList = new List<string>();
                    //this._RemoveColumnsFromList.Add("LogCreated*");
                    //this._RemoveColumnsFromList.Add("LogUpdated*");
                    //this._RemoveColumnsFromList.Add("LogInserted*");
                    //this._RemoveColumnsFromList.Add("RowGUID*");
                    this._RemoveColumnsFromList.Add("xx_*");
                }
                return _RemoveColumnsFromList;
            }
        }

        internal DiversityWorkbench.ApplicationEntity ApplicationEntity
        {
            get
            {
                DiversityWorkbench.ApplicationEntity A = new ApplicationEntity(this.ServerConnection);
                return A;
            }
        }

        public DiversityWorkbench.ServerConnection ServerConnection
        {
            get
            {
                DiversityWorkbench.ServerConnection S = DiversityWorkbench.Settings.ServerConnection;
                return S;
            }
        }

        public string Entity
        {
            get
            {
                string E = "";
                if (this.dataSetEntity.Entity.Rows.Count > 0)
                    E = this.dataSetEntity.Entity.Rows[0]["Entity"].ToString();
                return E;
            }
        }

        #endregion

        #region Connection

        private void databaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.userControlQueryList.QueryConditionVisiblity.Length > 0)
                    DiversityWorkbench.Forms.FormApplicationEntitySettings.Default.QueryConditionVisibility = this.userControlQueryList.QueryConditionVisiblity;
                DiversityWorkbench.Forms.FormApplicationEntitySettings.Default.Save();

                //System.Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                DiversityWorkbench.Forms.FormConnectToDatabase f = new Forms.FormConnectToDatabase();
                //DiversityWorkbench.FormDatabaseConnection f = new DiversityWorkbench.FormDatabaseConnection(v);
                f.setHelpProviderNameSpace(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace(), "Login");
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                    this.setDatabase();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool setDatabase()
        {
            bool OK = true;
            if (DiversityWorkbench.Settings.ConnectionString != "" && this.FormFunctions.DatabaseAccessible)// && LocalDatabaseNotMissing)
            {
                try
                {
                    //this.resetSqlAdapters();
                    DiversityWorkbench.WorkbenchUnit.ResetWorkbenchUnitConnections();
                    //DiversityCollection.LookupTable.ResetLookupTables();

                    //this.setToolbarPermissions();
                    this.setSearchOptions();
                    //this.setEnumDataSource();
                    //this.setPicklists();
                    //this.initRemoteModules();
                    this.userControlQueryList.setConnection(DiversityWorkbench.Settings.ConnectionString, "Entity");
                    //this.buildSearchMenu();
                    this.FormFunctions.setDescriptions();
                    //this.prepareToolStripDropDownButtonsEventLocalisation();
                    //this.prepareToolStripDropDownButtonsEventProperty();
                    //this.setToolStripDropDownButtonOverviewHierarchyNewUnit();
                    //this.setToolStripDropDownButtonOverviewHierarchyNewPartWithAdaptiveHierarchy();
                    //this.setCollectionSource();
                    //this.setUserControlHierarchySelectors();
                    this.setAutoCompletion();
                    //if (DiversityCollection.FormCollectionSpecimenSettings.Default.DefaultCollection > -1)// && this.DefaultCollection != DiversityCollection.FormCollectionSpecimenSettings.Default.DefaultCollection)
                    //    this.DefaultCollection = DiversityCollection.FormCollectionSpecimenSettings.Default.DefaultCollection;
                }
                catch (Exception ex)
                {
                    OK = false;
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
                OK = false;
            this.splitContainerData.Visible = false;
            this.FormFunctions.setTitle();
            this.userControlQueryList.toolStripButtonConnection.ToolTipText = this.FormFunctions.ConnectionInfo();
            //this.setMenu();
            return OK;
        }

        #endregion

        #region Query

        private void initQuery()
        {
            this.userControlQueryList.IDisNumeric = false;
            if (DiversityWorkbench.Forms.FormApplicationEntitySettings.Default.QueryConditionVisibility.Length > 0)
                this.userControlQueryList.QueryConditionVisiblity = DiversityWorkbench.Forms.FormApplicationEntitySettings.Default.QueryConditionVisibility;
            this.setQueryControlEvents();
            this.setSearchOptions();
            this.setQueryDisplayColumns();
        }

        private void setQueryDisplayColumns()
        {
            this.userControlQueryList.QueryDisplayColumns = this.ApplicationEntity.QueryDisplayColumns();
        }

        private void setSearchOptions()
        {
            this.userControlQueryList.setQueryConditions(this.ApplicationEntity.QueryConditions(), DiversityWorkbench.Forms.FormApplicationEntitySettings.Default.QueryConditionVisibility.ToString());//.FormCollectionSpecimenSettings.Default.QueryConditionVisibility.ToString());
        }

        private void setQueryControlEvents()
        {
            try
            {
                // QueryList
                this.userControlQueryList.toolStripButtonConnection.Click += new System.EventHandler(this.databaseToolStripMenuItem_Click);
                this.userControlQueryList.toolStripButtonCopy.Click += new System.EventHandler(this.copyEntity);
                this.userControlQueryList.toolStripButtonDelete.Click += new System.EventHandler(this.deleteEntity);
                this.userControlQueryList.toolStripButtonNew.Click += new System.EventHandler(this.createNewEntity);
                this.userControlQueryList.toolStripButtonSave.Click += new System.EventHandler(this.saveEntity);
                //this.buttonSave.Click += new System.EventHandler(this.saveSpecimen);
                //this.toolStripButtonSave.Click += new System.EventHandler(this.saveSpecimen);
                //this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveSpecimen);
                this.userControlQueryList.toolStripButtonUndo.Click += new System.EventHandler(this.undoChangesInEntity);
                this.userControlQueryList.toolStripButtonSwitchOrientation.Click += new System.EventHandler(this.switchQueryOrientation);
                this.userControlQueryList.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResult_SelectedIndexChanged);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void switchQueryOrientation(object sender, System.EventArgs e)
        {
            try
            {
                if (this.userControlQueryList.splitContainerMain.Orientation == Orientation.Vertical)
                    this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance / 2;
                else
                    this.splitContainerMain.SplitterDistance = (int)this.splitContainerMain.SplitterDistance * 2;
                this.userControlQueryList.switchOrientation();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxQueryResult_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                if (this.Entity.ToString() != this.userControlQueryList.PK)
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    this.SuspendLayout();
                    this.setEntity(this.userControlQueryList.PK);
                    this.ResumeLayout();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region toolStripQuery

        #region Copy
        private void copyEntity(object sender, System.EventArgs e)
        {
            //Creating the DataSet used as as template for the Copy form
            //DiversityWorkbench.DataSetEntity ds = this.dataSetEntity.Clone();
            //DiversityCollection.DataSetCollectionSpecimen ds = (DiversityCollection.DataSetCollectionSpecimen)this.dataSetCollectionSpecimen.Clone();
            //if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
            //{
            //    DiversityCollection.DataSetCollectionSpecimen.CollectionEventRow RE = ds.CollectionEvent.NewCollectionEventRow();
            //    foreach (System.Data.DataColumn C in this.dataSetCollectionSpecimen.CollectionEvent.Columns)
            //    {
            //        RE[C.ColumnName] = this.dataSetCollectionSpecimen.CollectionEvent.Rows[0][C.ColumnName];
            //    }
            //    ds.CollectionEvent.Rows.Add(RE);
            //}
            //DiversityWorkbench.DataSetEntity.EntityRow RS = ds.Entity.NewEntityRow();
            ////DiversityCollection.DataSetCollectionSpecimen.CollectionSpecimenRow RS = ds.CollectionSpecimen.NewCollectionSpecimenRow();
            //foreach (System.Data.DataColumn C in this.dataSetEntity.Entity.Columns)
            //{
            //    RS[C.ColumnName] = this.dataSetEntity.Entity.Rows[0][C.ColumnName];
            //}
            //ds.Entity.Rows.Add(RS);

            //DiversityCollection.FormCopyDataset f = new FormCopyDataset(ds);
            //f.ShowDialog();
            //if (f.DialogResult == DialogResult.OK)
            //{
            //    string AccessionNumber = f.AccessionNumber;
            //    DiversityCollection.FormCopyDataset.EventCopyMode EventCopyMode = f.ModeForEventCopy;
            //    bool CopyIdentification = f.CopyUnits;
            //    int SpecimenID = -1;
            //    int OriginalID = (int)this.SpecimenID;
            //    try
            //    {
            //        SpecimenID = this.CopySpecimen(OriginalID, AccessionNumber, f.ModeForEventCopy, f.CopyUnits);
            //        this.userControlQueryList.AddListItem(SpecimenID, AccessionNumber);
            //    }
            //    catch (Exception ex)
            //    {
            //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //    }
            //}
        }

        /// <summary>
        /// create a copy of a collection specimen
        /// </summary>
        /// <param name="OriginalID">The CollectionSpecimenID of the original dataset</param>
        /// <param name="AccessionNumber">The new AccessionNumber</param>
        /// <param name="EventCopyMode">The mode for the copy of the collection event</param>
        /// <param name="CopyUnits">If the identification units should be copied</param>
        /// <returns></returns>
        //private int CopySpecimen(int OriginalID, string AccessionNumber, DiversityCollection.FormCopyDataset.EventCopyMode EventCopyMode, bool CopyUnits)
        //{
        //    string SQL = "execute dbo.procCopyCollectionSpecimen NULL , " + OriginalID.ToString() + ", '" + AccessionNumber + "'";
        //    switch (EventCopyMode)
        //    {
        //        case FormCopyDataset.EventCopyMode.NewEvent:
        //            SQL += ", 1";
        //            break;
        //        case FormCopyDataset.EventCopyMode.SameEvent:
        //            SQL += ", 0";
        //            break;
        //        case FormCopyDataset.EventCopyMode.NoEvent:
        //            SQL += ", -1";
        //            break;
        //    }
        //    if (CopyUnits) SQL += ", 1";
        //    else SQL += ", 0";
        //    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //    con.Open();
        //    int ID = 0;
        //    try
        //    {
        //        ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    con.Close();
        //    return ID;
        //}


        #endregion

        #region Delete
        private void deleteEntity(object sender, System.EventArgs e)
        {
            if (this.dataSetEntity.Entity.Rows.Count > 0)
            {
                //string ID = (int)this.ID;
                //string SQL = "NameID = " + ID.ToString();
                //string DisplayTextDel = this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString();
                try
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you want to delete the entity\r\n\r\n" + this.Entity, "Delete entity?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        this.deleteEntity(Entity);
                        this.dataSetEntity.Entity.Clear();
                        this.dataSetEntity.EntityRepresentation.Clear();
                        this.dataSetEntity.EntityUsage.Clear();
                        this.splitContainerData.Visible = false;
                        this.userControlQueryList.RemoveSelectedListItem();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
            {
                if (this.userControlQueryList.listBoxQueryResult.Items.Count > 0)
                {
                    if (this.userControlQueryList.listBoxQueryResult.SelectedIndex > -1)
                        this.userControlQueryList.listBoxQueryResult.SelectedIndex = this.userControlQueryList.listBoxQueryResult.SelectedIndex;
                }
            }
        }

        /// <summary>
        /// delete a name from the database
        /// </summary>
        /// <param name="NameID">the Primary key of table TaxonName corresponding to the name that should be deleted</param>
        private void deleteEntity(string Entity)
        {
            try
            {
                //System.Windows.Forms.MessageBox.Show(DiversityWorkbench.Entity.Message("Delete_entry"));
                string SQL = "DELETE FROM Entity WHERE Entity = '" + Entity + "'";
                Microsoft.Data.SqlClient.SqlConnection Conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Conn);
                com.CommandType = System.Data.CommandType.Text;
                Conn.Open();
                com.ExecuteNonQuery();
                Conn.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        #endregion

        #region New
        private void createNewEntity(object sender, System.EventArgs e)
        {
            try
            {
                if (System.Windows.Forms.MessageBox.Show(this.Message("Do_you_want_to_create_a_new_entity"), this.Message("New_entity") + "?", System.Windows.Forms.MessageBoxButtons.OKCancel, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    string Original = "";
                    if (this.dataSetEntity.Entity.Rows.Count > 0)
                        Original = this.dataSetEntity.Entity.Rows[0][0].ToString();
                    DiversityWorkbench.Forms.FormGetString f = new FormGetString(this.Message("New_entity"), this.Message("Please_enter_the_name_of_the_new_entity"), Original);
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK)
                    {
                        System.Data.DataRow[] rr = this.dataSetEntity.EntityList.Select("Entity = '" + f.String + "'");
                        if (rr.Length > 0)
                        {
                            System.Windows.Forms.MessageBox.Show(this.Message("New_entity") + " " + f.String + " " + this.Message("does_already_exist"));
                            return;
                        }
                        string SQL = "INSERT INTO Entity (Entity) VALUES ('" + f.String + "')";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                        SQL = "INSERT INTO [EntityRepresentation] " +
                            "([Entity],[LanguageCode],[EntityContext],[DisplayText]) VALUES ('" + f.String + "', 'en-US', 'General', '" + f.String + "')";
                        DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                        this.setEntityList();
                        this.setEntity(f.String);
                        this.userControlQueryList.AddListItem(f.String, f.String);
                    }
                    //string AccessionNumber = "";
                    //int ID = this.InsertNewSpecimen(ref AccessionNumber);
                    //if (ID > -1)
                    //{
                    //    this.setSpecimen(ID);
                    //    if (AccessionNumber.Length > 0)
                    //        this.userControlQueryList.AddListItem(ID, AccessionNumber);
                    //    else
                    //}
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// inserting a new specimen into the table CollectionSpecimen
        /// </summary>
        /// <returns>the CollectionSpecimenID of the new specimen</returns>
        //private string InsertNewEntity(ref string Entity)
        //{
        //    try
        //    {
        //        Entity = "";
        //        //if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
        //        //{
        //        //    if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
        //        //    {
        //        //        if (System.Windows.Forms.MessageBox.Show("Do you want the database to search for the next free accession number after " + this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString(), "Accession number", MessageBoxButtons.YesNo) == DialogResult.Yes)
        //        //            Entity = this.NextAccessionNumber(this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString(), this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString().Length);
        //        //    }
        //        //}
        //        //int ID = -1;
        //        string SQL = "";
        //        if (Entity.Length == 0)
        //            SQL = "INSERT INTO CollectionSpecimen (Version) VALUES (1) SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
        //        else
        //            SQL = "INSERT INTO CollectionSpecimen (Version, AccessionNumber) VALUES (1, '" + Entity + "') SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]";
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //        con.Open();
        //        ID = System.Convert.ToInt32(cmd.ExecuteScalar().ToString());
        //        SQL = "INSERT INTO CollectionProject (CollectionSpecimenID, ProjectID) VALUES (" + ID.ToString() + ", " + this.userControlQueryList.ProjectID.ToString() + ")";
        //        cmd.CommandText = SQL;
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //        //if (AccessionNumber.Length == 0)
        //        //    AccessionNumber = "Copy of specimen " + ID.ToString();
        //        return ID;
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        return -1;
        //    }
        //}

        #endregion

        #region Save
        private void saveEntity(object sender, System.EventArgs e)
        {
            // move to a neutral control to ensure that the last changes are stored
            this.textBoxEntity.Focus();

            // save the specimen
            this.updateEntity();
            this.setEntity(this.Entity);
            this.userControlQueryList.RefreshItemDisplayText(this.dataSetEntity);
        }

        #endregion

        #region Undo
        private void undoChangesInEntity(object sender, System.EventArgs e)
        {
            try
            {
                string E = this.Entity;
                this.dataSetEntity.Entity.Clear();
                this.dataSetEntity.EntityRepresentation.Clear();
                this.dataSetEntity.EntityUsage.Clear();
                this.setEntity(E);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Entity

        private void buttonUsageNew_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Datasets.DataSetEntity.EntityUsageRow R = this.dataSetEntity.EntityUsage.NewEntityUsageRow();
            System.Data.DataTable dtContext = this.dataSetEntity.EntityContext_Enum.Copy();
            foreach (System.Data.DataRow RR in this.dataSetEntity.EntityUsage.Rows)
            {
                for (int i = 0; i < dtContext.Rows.Count; i++)
                {
                    if (dtContext.Rows[i]["Code"].ToString() == RR["EntityContext"].ToString())
                        dtContext.Rows[i].Delete();
                }
                dtContext.AcceptChanges();
            }
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtContext, "DisplayText", "Code", "Context", "Select a context from the list");
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                R.Entity = this.dataSetEntity.Entity.Rows[0]["Entity"].ToString();
                R.EntityContext = f.SelectedValue;
                this.dataSetEntity.EntityUsage.Rows.Add(R);
            }
        }

        private void buttonRepresentationNew_Click(object sender, EventArgs e)
        {
            // getting the language for the new representation
            System.Data.DataTable dtLanguage = this.dataSetEntity.EntityLanguageCode_Enum.Copy();
            DiversityWorkbench.Forms.FormGetStringFromList fL = new DiversityWorkbench.Forms.FormGetStringFromList(dtLanguage, "DisplayText", "Code", "Language", "Please select the language for the new representation");
            fL.ShowDialog();
            if (fL.DialogResult == DialogResult.OK)
            {
                string Language = fL.SelectedValue;
                // getting the context for the new representation
                System.Data.DataTable dtContext = this.dataSetEntity.EntityContext_Enum.Copy();
                foreach (System.Data.DataRow RR in this.dataSetEntity.EntityRepresentation.Rows)
                {
                    for (int i = 0; i < dtContext.Rows.Count; i++)
                    {
                        if (dtContext.Rows[i]["Code"].ToString() == RR["EntityContext"].ToString()
                            && RR["LanguageCode"].ToString() == Language)
                            dtContext.Rows[i].Delete();
                    }
                    dtContext.AcceptChanges();
                }
                if (dtContext.Rows.Count == 0) // all PK's are already used
                {
                    System.Windows.Forms.MessageBox.Show("All contexts for the selected language are already defined");
                    return;
                }
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtContext, "DisplayText", "Code", "Context", "Select a context from the list");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversityWorkbench.Datasets.DataSetEntity.EntityRepresentationRow RRep = this.dataSetEntity.EntityRepresentation.NewEntityRepresentationRow();
                    RRep.Entity = this.dataSetEntity.Entity.Rows[0]["Entity"].ToString();
                    RRep.EntityContext = f.SelectedValue;
                    RRep.LanguageCode = Language;
                    this.dataSetEntity.EntityRepresentation.Rows.Add(RRep);
                }
            }
        }

        private bool setEntity(string Entity)
        {
            bool OK = true;
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0) return false;


            try
            {
                if (this.dataSetEntity.Entity.Rows.Count > 0)
                {
                    this.updateEntity();
                }
                //this.userControlImageSpecimenImage.ImagePath = "";
                //this.userControlImageEventImage.ImagePath = "";
                //this.userControlImageCollectionEventSeries.ImagePath = "";
                //this.webBrowserEventMap.Url = new Uri("about:blank ");
                //this.webBrowserLabel.Url = new Uri("about:blank ");
                this.fillEntity(Entity);
                if (this.dataSetEntity.Entity.Rows.Count > 0)
                {
                    //this.tableLayoutPanelBackground.Visible = false;
                    //this.tableLayoutPanelBackground.Dock = DockStyle.Right;
                    this.setHeader();
                    this.splitContainerData.Visible = true;
                    //this.tableLayoutPanelHeader.Visible = true;
                    //this.setTabPageCustomEntryToDefault();
                }
                else
                {
                    //this.tableLayoutPanelBackground.Dock = DockStyle.Fill;
                    //this.tableLayoutPanelBackground.Visible = true;
                    this.splitContainerData.Visible = false;
                    //if (!this.scanModeToolStripMenuItem.Checked)
                    //    this.tableLayoutPanelHeader.Visible = false;
                    if (Entity != "")
                        System.Windows.Forms.MessageBox.Show("You have no access to this dataset");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        //private bool setEntity(string Entity)
        //{
        //    bool OK = false;
        //    string SQL = "SELECT CollectionSpecimenID_UserAvailable.CollectionSpecimenID " +
        //        "FROM CollectionSpecimen INNER JOIN " +
        //        "CollectionSpecimenID_UserAvailable ON  " +
        //        "CollectionSpecimen.CollectionSpecimenID = CollectionSpecimenID_UserAvailable.CollectionSpecimenID " +
        //        "WHERE (CollectionSpecimen.AccessionNumber = N'" + AccessionNumber + "')";
        //    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //    Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //    try
        //    {
        //        con.Open();
        //        int ID;
        //        OK = int.TryParse(C.ExecuteScalar()?.ToString(), out ID);
        //        if (!OK)
        //        {
        //            System.Windows.Forms.MessageBox.Show("The " + AccessionNumber + " could not be found in the database");
        //            return OK;
        //        }
        //        //int IDv = int.Parse(C.ExecuteScalar()?.ToString());
        //        con.Close();
        //        OK = this.setSpecimen(ID);
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //    return OK;
        //}

        private void setHeader()
        {
            //try
            //{
            //    if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
            //    {
            //        string ID = this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionSpecimenID"].ToString();
            //        if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].Equals(System.DBNull.Value))
            //        {
            //            ID += " / " + this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["CollectionEventID"].ToString();
            //            this.textBoxHeaderID.Width = 100;
            //        }
            //        else this.textBoxHeaderID.Width = 50;
            //        this.textBoxHeaderID.Text = ID;
            //        if (!this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
            //            this.textBoxHeaderAccessionNumber.Text = this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString();
            //        else
            //            this.textBoxHeaderAccessionNumber.Text = "";
            //        if (this.dataSetCollectionSpecimen.IdentificationUnit.Rows.Count > 0)
            //        {
            //            System.Data.DataRow[] rrType = this.dataSetCollectionSpecimen.Identification.Select("TypeStatus <> '' AND TypeStatus NOT LIKE 'not %'", "IdentificationSequence DESC");
            //            if (rrType.Length > 0)
            //            {
            //                this.textBoxHeaderIdentification.BackColor = System.Drawing.Color.Red;
            //                System.Data.DataRow rType = rrType[0];
            //                if (!rType["TaxonomicName"].Equals(System.DBNull.Value))
            //                    this.textBoxHeaderIdentification.Text = rType["TaxonomicName"].ToString() + "\r\n" + rType["TypeStatus"].ToString();
            //                else
            //                    this.textBoxHeaderIdentification.Text = rType["TypeStatus"].ToString();
            //            }
            //            else
            //            {
            //                this.textBoxHeaderIdentification.BackColor = System.Drawing.SystemColors.Control;
            //                System.Data.DataRow[] rr = this.dataSetCollectionSpecimen.IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder");
            //                System.Data.DataRow r = rr[0];
            //                if (!r["LastIdentificationCache"].Equals(System.DBNull.Value))
            //                    this.textBoxHeaderIdentification.Text = r["LastIdentificationCache"].ToString();
            //                else
            //                    this.textBoxHeaderIdentification.Text = "";
            //            }
            //        }
            //        else
            //        {
            //            this.textBoxHeaderIdentification.Text = "";
            //            this.textBoxHeaderIdentification.BackColor = System.Drawing.SystemColors.Control;
            //        }
            //        string Version = this.dataSetCollectionSpecimen.CollectionSpecimen.Rows[0]["Version"].ToString();
            //        if (this.dataSetCollectionSpecimen.CollectionEvent.Rows.Count > 0)
            //            Version += " / " + this.dataSetCollectionSpecimen.CollectionEvent.Rows[0]["Version"].ToString();
            //        this.textBoxHeaderVersion.Text = Version;
            //    }
            //    else
            //    {
            //        this.textBoxHeaderID.Text = "";
            //        this.textBoxHeaderAccessionNumber.Text = "";
            //        this.textBoxHeaderIdentification.Text = "";
            //        this.textBoxHeaderVersion.Text = "";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
        }

        private void fillEntity(string Entity)
        {
            try
            {
                this.dataSetEntity.Entity.Clear();
                this.dataSetEntity.EntityRepresentation.Clear();
                this.dataSetEntity.EntityUsage.Clear();
                this.textBoxEntity.Text = Entity;
                string SQL = "SELECT Entity, DisplayGroup, Notes " +
                    "FROM Entity WHERE Entity = '" + Entity + "'";
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref _SqlDataAdapterEntity, this.dataSetEntity.Entity, SQL, DiversityWorkbench.Settings.ConnectionString);

                SQL = "SELECT Entity, LanguageCode, EntityContext, DisplayText, Abbreviation, Description, Notes " +
                    "FROM EntityRepresentation WHERE Entity = '" + Entity + "'";
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref _SqlDataAdapterEntityRepresentation, this.dataSetEntity.EntityRepresentation, SQL, DiversityWorkbench.Settings.ConnectionString);

                SQL = "SELECT Entity, EntityContext, Accessibility, Determination, Visibility, PresetValue, Notes " +
                    "FROM EntityUsage WHERE Entity = '" + Entity + "'";
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref _SqlDataAdapterEntityUsage, this.dataSetEntity.EntityUsage, SQL, DiversityWorkbench.Settings.ConnectionString);



                //this.dataSetEntity.Clear();
                ////this.dataSetCollectionEventSeries.Clear();
                //////this.treeViewExpedition.Nodes.Clear();
                //string WhereClause = " WHERE Entity = " + Entity;

                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimen, DiversityCollection.CollectionSpecimen.SqlSpecimen + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimen);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAgent, DiversityCollection.CollectionSpecimen.SqlAgent + WhereClause + " ORDER BY CollectorsSequence", this.dataSetCollectionSpecimen.CollectionAgent);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProject, DiversityCollection.CollectionSpecimen.SqlProject + WhereClause + " ORDER BY ProjectID", this.dataSetCollectionSpecimen.CollectionProject);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this.dataSetCollectionSpecimen.CollectionSpecimenImage);

                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterRelation, DiversityCollection.CollectionSpecimen.SqlRelation + WhereClause + " ORDER BY RelatedSpecimenDisplayText", this.dataSetCollectionSpecimen.CollectionSpecimenRelation);
                //string SQL = "SELECT R.CollectionSpecimenID, R.RelatedSpecimenURI, S.AccessionNumber AS RelatedSpecimenDisplayText, R.RelationType, R.RelatedSpecimenCollectionID, " +
                //    "R.RelatedSpecimenDescription, R.Notes, R.IsInternalRelationCache  " +
                //    "FROM CollectionSpecimenRelation R, CollectionSpecimen S  " +
                //    "WHERE (R.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable))  " +
                //    "AND (S.CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable))  " +
                //    "AND (R.IsInternalRelationCache = 1)  " +
                //    "AND rtrim(substring(R.RelatedSpecimenURI, len(dbo.BaseURL()) + 1, 255)) = '" + SpecimenID.ToString() + "'  " +
                //    "AND S.CollectionSpecimenID = R.CollectionSpecimenID " +
                //    "ORDER BY RelatedSpecimenDisplayText ";
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterRelationInvers, SQL, this.dataSetCollectionSpecimen.CollectionSpecimenRelationInvers);

                ////this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterStorage, DiversityCollection.CollectionSpecimen.SqlStorage + WhereClause, this.dataSetCollectionSpecimen.CollectionStorage);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterPart, DiversityCollection.CollectionSpecimen.SqlPart + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenPart);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterProcessing, DiversityCollection.CollectionSpecimen.SqlProcessing + WhereClause + " ORDER BY ProcessingDate, ProcessingID", this.dataSetCollectionSpecimen.CollectionSpecimenProcessing);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterTransaction, DiversityCollection.CollectionSpecimen.SqlTransaction + WhereClause, this.dataSetCollectionSpecimen.CollectionSpecimenTransaction);

                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnit, DiversityCollection.CollectionSpecimen.SqlUnit + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnit);

                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterUnitInPart, DiversityCollection.CollectionSpecimen.SqlUnitInPart + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnitInPart);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterIdentification, DiversityCollection.CollectionSpecimen.SqlIdentification + WhereClause + " ORDER BY IdentificationSequence", this.dataSetCollectionSpecimen.Identification);
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterAnalysis, DiversityCollection.CollectionSpecimen.SqlAnalysis + WhereClause, this.dataSetCollectionSpecimen.IdentificationUnitAnalysis);
                ////this.setAnalysisPartList();

                //this.listBoxSpecimenImage.Items.Clear();
                //this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this.dataSetCollectionSpecimen.CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
                //if (this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count == 0) this.tableLayoutPanelSpecimenImage.Enabled = false;
                //if (!this.ShowImagesSpecimen && this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count > 0)
                //    this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.Color.Yellow;
                //else if (!this.ShowImagesSpecimen && this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count == 0)
                //    this.buttonHeaderShowSpecimenImage.BackColor = System.Drawing.SystemColors.Control;

                //this.setSpecimenImageUnitList();
                //this.setSpecimenImagePartList();

                ////TODO: Notlösung fuer Anzeige - Ursache nicht gefunden
                //if (this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows.Count > 0)
                //{
                //    if (!this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows[0]["SpecimenPartID"].Equals(System.DBNull.Value))
                //    {
                //        System.Data.DataRow[] rv = this._dtImageParts.Select("SpecimenPartID = " + this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows[0]["SpecimenPartID"].ToString());
                //        this.comboBoxSpecimenImageSpecimenPart.Text = rv[0]["StorageLocation"].ToString();
                //    }
                //    if (!this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows[0]["IdentificationUnitID"].Equals(System.DBNull.Value))
                //    {
                //        System.Data.DataRow[] ru = this._dtImageUnits.Select("IdentificationUnitID = " + this.dataSetCollectionSpecimen.CollectionSpecimenImage.Rows[0]["IdentificationUnitID"].ToString());
                //        this.comboBoxSpecimenImageIdentificationUnitID.Text = ru[0]["LastIdentificationCache"].ToString();
                //    }
                //}

                ////this.buildUnitHierarchy();
                //this.fillProjectList();
                ////this.buildStorageHierarchy();
                //this.treeViewStorageHierarchy_AfterSelect(null, null);

                //if (this.listBoxSpecimenImage.Items.Count > 0)
                //{
                //    //this.listBoxSpecimenImage_SelectedIndexChanged(null, null);
                //    //this.listBoxSpecimenImage.SelectedIndex = 0;
                //    //this.comboBoxSpecimenImageSpecimenPart.DataSource;
                //    //this.comboBoxSpecimenImageSpecimenPart.DisplayMember;
                //    //this.comboBoxSpecimenImageSpecimenPart.ValueMember;
                //    //this.comboBoxSpecimenImageSpecimenPart.SelectedIndex;
                //    //this.comboBoxSpecimenImageSpecimenPart.Text;
                //    //this.comboBoxSpecimenImageSpecimenPart.SelectedText;
                //    //this.comboBoxSpecimenImageSpecimenPart.SelectedValue;
                //    //this.comboBoxSpecimenImageSpecimenPart.SelectedItem;
                //}

                //this.setRelatedSpecimenControls();

                //// Event
                //if (this.dataSetCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
                //{
                //    this.setEvent();
                //}
                //this.fillExternalRelatedSpecimenAndUnits();
                //this.setEventControls();
                //this.buildOverviewHierarchy();
                //this.setToolStripDropDownButtonsEventPropertyVisibilty();
                //this.setToolStripDropDownButtonsEventLocalisationVisibilty();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the specimen", System.Windows.Forms.MessageBoxButtons.OK);
            }
        }

        private void updateEntity()
        {
            if (this._SqlDataAdapterEntity != null)
            {
                this.FormFunctions.updateTable(this.dataSetEntity, "Entity", this._SqlDataAdapterEntity, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetEntity, "EntityRepresentation", this._SqlDataAdapterEntityRepresentation, this.BindingContext);
                this.FormFunctions.updateTable(this.dataSetEntity, "EntityUsage", this._SqlDataAdapterEntityUsage, this.BindingContext);
            }


            //if (this.sqlDataAdapterSpecimen != null)
            //{
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimen", this.sqlDataAdapterSpecimen, this.BindingContext);
            //    //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this.BindingContext);
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this.collectionSpecimenImageBindingSource);
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionAgent", this.sqlDataAdapterAgent, this.collectionAgentBindingSource);

            //    // special treatment for Relations
            //    System.Data.DataRowView RRel = (System.Data.DataRowView)this.collectionSpecimenRelationBindingSource.Current;
            //    if (RRel != null)
            //    {
            //        try
            //        {
            //            RRel.BeginEdit();
            //            RRel.EndEdit();
            //        }
            //        catch (System.Data.ConstraintException cex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(cex.Message);
            //        }
            //    }
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenRelation", this.sqlDataAdapterRelation, this.BindingContext);

            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionProject", this.sqlDataAdapterProject, this.BindingContext);

            //    // if datasets of this table were deleted, this must happen before deleting the parent tables
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

            //    System.Data.DataRowView RP = (System.Data.DataRowView)this.collectionSpecimenPartBindingSource.Current;
            //    if (RP != null)
            //    {
            //        try
            //        {
            //            RP.BeginEdit();
            //            RP.EndEdit();
            //        }
            //        catch (System.Data.ConstraintException cex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(cex.Message);
            //        }
            //        catch (System.Exception ex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(ex.Message);
            //        }
            //    }
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenPart", this.sqlDataAdapterPart, this.BindingContext);
            //    if (this._ViewMode != ViewMode.Gridmode)
            //        this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenProcessing", this.sqlDataAdapterProcessing, this.BindingContext);
            //    if (this._ViewMode != ViewMode.Gridmode)
            //        this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenTransaction", this.sqlDataAdapterTransaction, this.BindingContext);

            //    // special treatment for Identifications
            //    System.Data.DataRowView RU = (System.Data.DataRowView)this.identificationUnitBindingSource.Current;
            //    if (RU != null)
            //    {
            //        try
            //        {
            //            RU.BeginEdit();
            //            RU.EndEdit();
            //        }
            //        catch (System.Data.ConstraintException cex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(cex.Message);
            //        }
            //        catch (System.Exception ex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(ex.Message);
            //        }
            //    }
            //    System.Data.DataRowView RI = (System.Data.DataRowView)this.identificationBindingSource.Current;
            //    if (RI != null)
            //    {
            //        try
            //        {
            //            RI.BeginEdit();
            //            RI.EndEdit();
            //        }
            //        catch (System.Data.ConstraintException cex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(cex.Message);
            //        }
            //        catch (System.Exception ex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(ex.Message);
            //        }
            //    }
            //    System.Data.DataRowView RA = (System.Data.DataRowView)this.identificationUnitAnalysisBindingSource.Current;
            //    if (RA != null)
            //    {
            //        try
            //        {
            //            RA.BeginEdit();
            //            RA.EndEdit();
            //        }
            //        catch (System.Data.ConstraintException cex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(cex.Message);
            //        }
            //        catch (System.Exception ex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(ex.Message);
            //        }
            //    }

            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnit", this.sqlDataAdapterUnit, this.BindingContext);
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "Identification", this.sqlDataAdapterIdentification, this.BindingContext);
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitAnalysis", this.sqlDataAdapterAnalysis, this.BindingContext);
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnitInPart", this.sqlDataAdapterUnitInPart, this.BindingContext);

            //    System.Data.DataRowView RE = (System.Data.DataRowView)this.collectionEventBindingSource.Current;
            //    if (RE != null)
            //    {
            //        try
            //        {
            //            RE.BeginEdit();
            //            RE.EndEdit();
            //        }
            //        catch (System.Data.ConstraintException cex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(cex.Message);
            //        }
            //        catch (System.Exception ex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(ex.Message);
            //        }
            //    }
            //    System.Data.DataRowView RL = (System.Data.DataRowView)this.collectionEventLocalisationBindingSource.Current;
            //    if (RL != null)
            //    {
            //        try
            //        {
            //            RL.BeginEdit();
            //            RL.EndEdit();
            //        }
            //        catch (System.Data.ConstraintException cex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(cex.Message);
            //        }
            //        catch (System.Exception ex)
            //        {
            //            System.Windows.Forms.MessageBox.Show(ex.Message);
            //        }
            //    }
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEvent", this.sqlDataAdapterEvent, this.BindingContext);
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventImage", this.sqlDataAdapterEventImage, this.BindingContext);
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventLocalisation", this.sqlDataAdapterLocalisation, this.BindingContext);
            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionEventProperty", this.sqlDataAdapterProperty, this.BindingContext);

            //    this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "Collection", this.sqlDataAdapterCollection, this.BindingContext);

            //    this.FormFunctions.updateTable(this.dataSetCollectionEventSeries, "CollectionEventSeries", this.sqlDataAdapterEventSeries, this.BindingContext);
            //    this.FormFunctions.updateTable(this.dataSetCollectionEventSeries, "CollectionEventSeriesImage", this.sqlDataAdapterEventSeriesImage, this.BindingContext);
            //}
        }

        #endregion

    }
}
