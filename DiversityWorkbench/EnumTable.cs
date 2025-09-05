using System;

namespace DiversityWorkbench
{
	/// <summary>
	/// Zusammenfassung für EnumTable.
	/// </summary>
	public class EnumTable
	{
		public EnumTable()
		{
		}

        #region Setting Enum as datasource
        /// <summary>
        /// Setting a lookup table as a datasource for a combo box
        /// </summary>
        /// <param name="CB">the combobox</param>
        /// <param name="EnumTableName">The name of the lookuplist, e.g.</param>
        /// <param name="conn">OleDB connection to the current database</param>
        public static void SetEnumTableAsDatasource(System.Windows.Forms.ComboBox CB, string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn)
		{
			CB.DataSource = EnumTable.SetEnumTable(EnumTableName, conn);
			CB.ValueMember = "Code";
			CB.DisplayMember = "DisplayText";
			CB.Leave += new System.EventHandler(DiversityWorkbench.EnumTable.comboBoxEnum_Leave);
            DiversityWorkbench.EnumTable.setAutoCompletion(CB, System.Windows.Forms.AutoCompleteMode.SuggestAppend);
        }

        public static void SetEnumTableAsDatasource(System.Windows.Forms.ComboBox CB, string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull)
        {
            CB.DataSource = EnumTable.SetEnumTable(EnumTableName, conn, IncludeNull);
            CB.ValueMember = "Code";
            CB.DisplayMember = "DisplayText";
            CB.Leave += new System.EventHandler(DiversityWorkbench.EnumTable.comboBoxEnum_Leave);
            DiversityWorkbench.EnumTable.setAutoCompletion(CB, System.Windows.Forms.AutoCompleteMode.SuggestAppend);
        }

        /// <summary>
        /// sets the datasource of a combobox to a standard enumeration table as used in some Diversity Workbench modules
        /// </summary>
        /// <param name="CB">the combobox for which the datasource should be set</param>
        /// <param name="EnumTableName">the name of the enumeration table</param>
        /// <param name="conn">the sql connection</param>
        /// <param name="IncludeNull">if a NULL value should be included at the beginning of the list</param>
        /// <param name="OrderByDisplay">if the values should be ordered in alphabetic sequence, otherwise according to the display order defined in the enumeration table</param>
        public static void SetEnumTableAsDatasource(System.Windows.Forms.ComboBox CB, string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull, bool OrderByDisplay)
        {
            CB.DataSource = EnumTable.SetEnumTable(EnumTableName, conn, IncludeNull, OrderByDisplay);
            CB.ValueMember = "Code";
            CB.DisplayMember = "DisplayText";
            CB.Leave += new System.EventHandler(DiversityWorkbench.EnumTable.comboBoxEnum_Leave);
            DiversityWorkbench.EnumTable.setAutoCompletion(CB, System.Windows.Forms.AutoCompleteMode.SuggestAppend);
        }
        
        /// <summary>
        /// sets the datasource of a combobox to a standard enumeration table as used in some Diversity Workbench modules
        /// </summary>
        /// <param name="CB">the combobox for which the datasource should be set</param>
        /// <param name="EnumTableName">the name of the enumeration table</param>
        /// <param name="conn">the sql connection</param>
        /// <param name="IncludeNull">if a NULL value should be included at the beginning of the list</param>
        /// <param name="OrderByDisplay">if the values should be ordered in alphabetic sequence, otherwise according to the display order defined in the enumeration table</param>
        /// <param name="IncludeDescription">if the display text should include the description</param>
        public static void SetEnumTableAsDatasource(System.Windows.Forms.ComboBox CB, string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull, bool OrderByDisplay, bool IncludeDescription)
        {
            try
            {
                System.Data.DataTable dt = EnumTable.SetEnumTable(EnumTableName, conn, IncludeNull, OrderByDisplay, IncludeDescription);
                CB.DataSource = dt;
                CB.ValueMember = "Code";
                CB.DisplayMember = "DisplayText";
                CB.Leave += new System.EventHandler(DiversityWorkbench.EnumTable.comboBoxEnum_Leave);
                int iLenMax = 0;
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    if (R["DisplayText"].ToString().Length > iLenMax) iLenMax = R["DisplayText"].ToString().Length;
                }
                iLenMax = (int)((float)iLenMax * (float)CB.Font.SizeInPoints / 1.75);
                if (CB.DropDownWidth < iLenMax)
                {
                    CB.DropDownWidth = iLenMax;
                }
                else if (CB.DropDownWidth < CB.Width)
                    CB.DropDownWidth = CB.Width;
                DiversityWorkbench.EnumTable.setAutoCompletion(CB, System.Windows.Forms.AutoCompleteMode.SuggestAppend);
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// sets the datasource of a combobox to a standard enumeration table as used in some Diversity Workbench modules
        /// </summary>
        /// <param name="CB">the combobox for which the datasource should be set</param>
        /// <param name="EnumTableName">the name of the enumeration table</param>
        /// <param name="Restriction">an SQL WHERE clause for the restriction of the values, e.g. "Enum.[Column] = 5" including Enum as table alias</param>
        /// <param name="conn">the sql connection</param>
        /// <param name="IncludeNull">if a NULL value should be included at the beginning of the list</param>
        /// <param name="OrderByDisplay">if the values should be ordered in alphabetic sequence, otherwise according to the display order defined in the enumeration table</param>
        /// <param name="IncludeDescription">if the display text should include the description</param>
        public static void SetEnumTableAsDatasource(System.Windows.Forms.ComboBox CB, string EnumTableName, string Restriction, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull, bool OrderByDisplay, bool IncludeDescription)
        {
            System.Data.DataTable dt = EnumTable.SetEnumTable(EnumTableName, conn, Restriction, IncludeNull, OrderByDisplay, IncludeDescription);
            CB.DataSource = dt;
            CB.ValueMember = "Code";
            CB.DisplayMember = "DisplayText";
            CB.Leave += new System.EventHandler(DiversityWorkbench.EnumTable.comboBoxEnum_Leave);
            int iLenMax = 0;
            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (R["DisplayText"].ToString().Length > iLenMax) iLenMax = R["DisplayText"].ToString().Length;
            }
            iLenMax = (int)((float)iLenMax * (float)CB.Font.SizeInPoints / 1.75);
            if (CB.DropDownWidth < iLenMax)
            {
                CB.DropDownWidth = iLenMax;
            }
            else if (CB.DropDownWidth < CB.Width)
                CB.DropDownWidth = CB.Width;
            DiversityWorkbench.EnumTable.setAutoCompletion(CB, System.Windows.Forms.AutoCompleteMode.SuggestAppend);
        }
        
        /// <summary>
        /// sets the datasource of a combobox to a standard enumeration table as used in some Diversity Workbench modules
        /// </summary>
        /// <param name="CB">the combobox for which the datasource should be set</param>
        /// <param name="EnumTableName">the name of the enumeration table</param>
        /// <param name="conn">the sql connection</param>
        /// <param name="IncludeNull">if a NULL value should be included at the beginning of the list</param>
        /// <param name="OrderByDisplayText">if the values should be ordered in alphabetic sequence, otherwise according to the display order defined in the enumeration table</param>
        /// <param name="IncludeDescription">if the display text should include the description</param>
        /// <param name="Restriction">a SQL restriction for the values</param>
        public static void SetEnumTableAsDatasource(System.Windows.Forms.ComboBox CB, string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull, bool OrderByDisplayText, bool IncludeDescription, string Restriction)
        {
            System.Data.DataTable dt = EnumTable.SetEnumTable(EnumTableName, conn, IncludeNull, OrderByDisplayText, IncludeDescription, Restriction);
            CB.DataSource = dt;
            CB.ValueMember = "Code";
            CB.DisplayMember = "DisplayText";
            CB.Leave += new System.EventHandler(DiversityWorkbench.EnumTable.comboBoxEnum_Leave);
            int iLenMax = 0;
            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (R["DisplayText"].ToString().Length > iLenMax) iLenMax = R["DisplayText"].ToString().Length;
            }
            iLenMax = (int)((float)iLenMax * (float)CB.Font.SizeInPoints / 1.75);
            if (CB.DropDownWidth < iLenMax)
            {
                CB.DropDownWidth = iLenMax;
            }
            else if (CB.DropDownWidth < CB.Width)
                CB.DropDownWidth = CB.Width;
            DiversityWorkbench.EnumTable.setAutoCompletion(CB, System.Windows.Forms.AutoCompleteMode.SuggestAppend);
        }

        /// <summary>
        /// sets the datasource of a combobox to a standard enumeration table as used in some Diversity Workbench modules
        /// </summary>
        /// <param name="ValueColumn">the column of the table, the enum should refer to</param>
        /// <param name="CB">the combobox for which the datasource should be set</param>
        /// <param name="HierarchySelector">the hierarchy selector for which the datasource should be set</param>
        /// <param name="EnumTableName">the name of the enumeration table</param>
        /// <param name="conn">the sql connection</param>
        /// <param name="IncludeNull">if a NULL value should be included at the beginning of the list</param>
        /// <param name="OrderByDisplay">if the values should be ordered in alphabetic sequence, otherwise according to the display order defined in the enumeration table</param>
        /// <param name="IncludeDescription">if the display text should include the description</param>
        public static void SetEnumTableAsDatasource(string ValueColumn, System.Windows.Forms.ComboBox CB, DiversityWorkbench.UserControls.   UserControlHierarchySelector HierarchySelector, string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull, bool OrderByDisplay, bool IncludeDescription)
        {
            System.Data.DataTable dt = EnumTable.SetEnumTable(EnumTableName, conn, IncludeNull, OrderByDisplay, IncludeDescription, true);
            CB.DataSource = dt;
            CB.ValueMember = "Code";
            CB.DisplayMember = "DisplayText";
            CB.Leave += new System.EventHandler(DiversityWorkbench.EnumTable.comboBoxEnum_Leave);
            int iLenMax = 0;
            foreach (System.Data.DataRow R in dt.Rows)
            {
                if (R["DisplayText"].ToString().Length > iLenMax) iLenMax = R["DisplayText"].ToString().Length;
            }
            iLenMax = (int)((float)iLenMax * (float)CB.Font.SizeInPoints / 1.75);
            if (CB.DropDownWidth < iLenMax)
            {
                CB.DropDownWidth = iLenMax;
            }
            else if (CB.DropDownWidth < CB.Width)
                CB.DropDownWidth = CB.Width;
            string OrderColumn = "DisplayOrder";
            if (OrderByDisplay) OrderColumn = "DisplayText";
            HierarchySelector.initHierarchy(dt, "Code", "ParentCode", "DisplayText", OrderColumn, ValueColumn, CB);
            DiversityWorkbench.EnumTable.setAutoCompletion(CB, System.Windows.Forms.AutoCompleteMode.SuggestAppend);
        }

        #region private Auxillary
        private static void comboBoxEnum_Leave(object sender, System.EventArgs e)
		{
            try
            {
                System.Windows.Forms.ComboBox CB = (System.Windows.Forms.ComboBox)sender;
                if (CB.SelectedIndex == -1 && CB.Text.Length == 0)
                {
                    CB.SelectedIndex = 0;
                }
            }
            catch (System.Exception ex) { }
		}

        //private static void setContext(System.Data.DataTable dtEnum, string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeDescription)
        //{
        //    string Entity = "";
        //    foreach (System.Data.DataRow R in dtEnum.Rows)
        //    {
        //        Entity = EnumTableName + ".Code." + R["Code"].ToString();
        //        System.Collections.Generic.Dictionary<string, string> Dict = DiversityWorkbench.Entity.EntityInformation(Entity);
        //        if (Dict["DisplayTextOK"] == "True")
        //        {
        //        }
        //    }
        //}

        //		private static void comboBoxEnum_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        //		{
        //			System.Windows.Forms.ComboBox CB = (System.Windows.Forms.ComboBox)sender;
        //			if (CB.SelectedIndex == -1 && CB.Text.Length == 0)
        //			{
        //				CB.SelectedIndex = 0;
        //			}
        //		}
        //

		private static System.Data.DataTable SetEnumTable(string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn)
		{
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            System.Data.DataColumn dcCode = new System.Data.DataColumn("Code", typeof(string));
            System.Data.DataColumn dcDisplayText = new System.Data.DataColumn("DisplayText", typeof(string));
            System.Data.DataColumn dcDescription = new System.Data.DataColumn("Description", typeof(string));
            DtEnumTable.Columns.Add(dcCode);
            DtEnumTable.Columns.Add(dcDisplayText);
            DtEnumTable.Columns.Add(dcDescription);
            string SQL = "SELECT NULL AS Code, NULL AS DisplayText, NULL AS Description, NULL as DisplayOrder " +
                " UNION " +
                " SELECT Code, DisplayText, Description, DisplayOrder FROM dbo." + DtEnumTable +
                " WHERE (DisplayEnable = 1) ORDER BY DisplayOrder, DisplayText";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, conn);
                if (conn.ConnectionString.Length > 0)
                    a.Fill(DtEnumTable);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return DtEnumTable;
		}

        private static System.Data.DataTable SetEnumTable(string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            System.Data.DataColumn dcCode = new System.Data.DataColumn("Code", typeof(string));
            System.Data.DataColumn dcDisplayText = new System.Data.DataColumn("DisplayText", typeof(string));
            System.Data.DataColumn dcDescription = new System.Data.DataColumn("Description", typeof(string));
            DtEnumTable.Columns.Add(dcCode);
            DtEnumTable.Columns.Add(dcDisplayText);
            DtEnumTable.Columns.Add(dcDescription);
            string SQL = "";
            if (IncludeNull) SQL += "SELECT NULL AS Code, NULL AS DisplayText, NULL AS Description, NULL as DisplayOrder  UNION ";
            SQL += " SELECT Code, DisplayText, Description, DisplayOrder FROM dbo." + DtEnumTable +
                " WHERE (DisplayEnable = 1) ORDER BY DisplayOrder";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, conn);
                a.Fill(DtEnumTable);
            }
            catch { }
            return DtEnumTable;
        }

        private static System.Data.DataTable SetEnumTable(string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull, bool OrderByDisplay)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            System.Data.DataColumn dcCode = new System.Data.DataColumn("Code", typeof(string));
            System.Data.DataColumn dcDisplayText = new System.Data.DataColumn("DisplayText", typeof(string));
            System.Data.DataColumn dcDescription = new System.Data.DataColumn("Description", typeof(string));
            DtEnumTable.Columns.Add(dcCode);
            DtEnumTable.Columns.Add(dcDisplayText);
            DtEnumTable.Columns.Add(dcDescription);
            string SQL = "";
            if (IncludeNull) SQL += "SELECT NULL AS Code, NULL AS DisplayText, NULL AS Description, NULL as DisplayOrder  UNION ";
            SQL += " SELECT Code, DisplayText, Description, DisplayOrder FROM dbo." + DtEnumTable +
                " WHERE (DisplayEnable = 1)";
            if (OrderByDisplay) SQL += " ORDER BY DisplayText";
            else SQL += " ORDER BY DisplayOrder";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, conn);
                a.Fill(DtEnumTable);
            }
            catch { }
            return DtEnumTable;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EnumTableName">The Enum table</param>
        /// <param name="conn">the SQL connection</param>
        /// <param name="IncludeNull">if the NULL value should be included</param>
        /// <param name="OrderByDisplay">If the item should be ordered by the display text</param>
        /// <param name="IncludeDescription">If the description text should be included in the display text</param>
        /// <returns></returns>
        private static System.Data.DataTable SetEnumTable(string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull, bool OrderByDisplay, bool IncludeDescription)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            System.Data.DataColumn dcCode = new System.Data.DataColumn("Code", typeof(string));
            System.Data.DataColumn dcDisplayText = new System.Data.DataColumn("DisplayText", typeof(string));
            System.Data.DataColumn dcDescription = new System.Data.DataColumn("Description", typeof(string));
            DtEnumTable.Columns.Add(dcCode);
            DtEnumTable.Columns.Add(dcDisplayText);
            DtEnumTable.Columns.Add(dcDescription);
            string SQL = "";

            if (DiversityWorkbench.Entity.EntityTablesExist)
            {
                SQL += "declare @Enum Table (Code nvarchar(50), " +
                    "DisplayText nvarchar(1000) NULL, " +
                    "[Description]nvarchar(1000) NULL, " +
                    "DisplayOrder smallint) ";
                if (IncludeNull) SQL += "insert @Enum (Code, DisplayText, Description, DisplayOrder) Values (NULL, NULL, NULL, NULL) ";
                SQL += "insert @Enum (Code, DisplayText, Description, DisplayOrder) " +
                    "SELECT  Enum.Code, cast(Enum.DisplayText + CASE WHEN Enum.Description IS NULL " +
                    "THEN '' ELSE CASE WHEN Enum.DisplayText = Enum.Description THEN '' " +
                    "ELSE '       = ' + Enum.Description END END as nvarchar(1000)) AS DisplayText, " +
                    "cast(Enum.Description as nvarchar(1000)) AS Description, Enum.DisplayOrder " +
                    "FROM " + EnumTableName + " AS Enum " +
                    "update E set E.DisplayText = R.DisplayText + CASE WHEN R.Description IS NULL OR rtrim(R.Description) = '' " +
                    "THEN '' ELSE CASE WHEN R.DisplayText = R.Description THEN '' " +
                    "ELSE '       = ' + R.Description END END " +
                    ", E.Description = CASE when R.Description Is null then E.Description else R.Description end " +
                    "From  @Enum AS E, EntityRepresentation AS R " +
                    "where '" + EnumTableName + ".Code.' + E.Code = R.Entity " +
                    "and R.LanguageCode = N'" + DiversityWorkbench.Settings.Language + "' AND not R.DisplayText is null " +
                    "Select * from @Enum";
            }
            else
            {
                if (IncludeNull) SQL += "SELECT NULL AS Code, NULL AS DisplayText, NULL AS Description, NULL as DisplayOrder  UNION ";
                SQL += " SELECT Code, DisplayText + CASE WHEN Description IS NULL THEN '' " +
                    " ELSE CASE WHEN DisplayText = Description THEN '' " +
                    " ELSE replicate(' ', cast(((select max(len(DisplayText)) from " + EnumTableName + " where DisplayEnable = 1)- len(DisplayText)) * 1.5 as int)) + '   =   ' + Description " +
                    " END END AS DisplayText, Description, DisplayOrder FROM dbo." + EnumTableName +
                    " WHERE (DisplayEnable = 1)";
            }
            if (OrderByDisplay) SQL += " ORDER BY DisplayText";
            else SQL += " ORDER BY DisplayOrder";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, conn);
                a.Fill(DtEnumTable);
            }
            catch { }
            return DtEnumTable;
        }

        private static System.Data.DataTable SetEnumTable(string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, string Restriction, bool IncludeNull, bool OrderByDisplay, bool IncludeDescription)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            System.Data.DataColumn dcCode = new System.Data.DataColumn("Code", typeof(string));
            System.Data.DataColumn dcDisplayText = new System.Data.DataColumn("DisplayText", typeof(string));
            System.Data.DataColumn dcDescription = new System.Data.DataColumn("Description", typeof(string));
            DtEnumTable.Columns.Add(dcCode);
            DtEnumTable.Columns.Add(dcDisplayText);
            DtEnumTable.Columns.Add(dcDescription);
            string SQL = "";

            if (DiversityWorkbench.Entity.EntityTablesExist)
            {
                SQL += "declare @Enum Table (Code nvarchar(50), " +
                    "DisplayText nvarchar(1000) NULL, " +
                    "[Description]nvarchar(1000) NULL, " +
                    "DisplayOrder smallint) ";
                if (IncludeNull) SQL += "insert @Enum (Code, DisplayText, Description, DisplayOrder) Values (NULL, NULL, NULL, NULL) ";
                SQL += "insert @Enum (Code, DisplayText, Description, DisplayOrder) " +
                    "SELECT  Enum.Code, cast(Enum.DisplayText + CASE WHEN Enum.Description IS NULL " +
                    "THEN '' ELSE CASE WHEN Enum.DisplayText = Enum.Description THEN '' " +
                    "ELSE '       = ' + Enum.Description END END as nvarchar(1000)) AS DisplayText, " +
                    "cast(Enum.Description as nvarchar(1000)) AS Description, Enum.DisplayOrder " +
                    "FROM " + EnumTableName + " AS Enum ";
                if (Restriction.Length > 0)
                    SQL += " WHERE " + Restriction + " ";
                SQL += "update E set E.DisplayText = R.DisplayText + CASE WHEN R.Description IS NULL OR rtrim(R.Description) = '' " +
                    "THEN '' ELSE CASE WHEN R.DisplayText = R.Description THEN '' " +
                    "ELSE '       = ' + R.Description END END " +
                    ", E.Description = CASE when R.Description Is null then E.Description else R.Description end " +
                    "From  @Enum AS E, EntityRepresentation AS R " +
                    "where '" + EnumTableName + ".Code.' + E.Code = R.Entity " +
                    "and R.LanguageCode = N'" + DiversityWorkbench.Settings.Language + "' AND not R.DisplayText is null " +
                    "Select * from @Enum";
            }
            else
            {
                if (IncludeNull) SQL += "SELECT NULL AS Code, NULL AS DisplayText, NULL AS Description, NULL as DisplayOrder  UNION ";
                SQL += " SELECT Code, DisplayText + CASE WHEN Description IS NULL THEN '' " +
                    " ELSE CASE WHEN DisplayText = Description THEN '' " +
                    " ELSE replicate(' ', cast(((select max(len(DisplayText)) from " + EnumTableName + " where DisplayEnable = 1)- len(DisplayText)) * 1.5 as int)) + '   =   ' + Description " +
                    " END END AS DisplayText, Description, DisplayOrder FROM dbo." + EnumTableName + " AS Enum " +
                    " WHERE (DisplayEnable = 1)";
                if (Restriction.Length > 0)
                    SQL += " AND " + Restriction + " ";
            }
            if (OrderByDisplay) SQL += " ORDER BY DisplayText";
            else SQL += " ORDER BY DisplayOrder";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, conn);
                a.Fill(DtEnumTable);
            }
            catch { }
            return DtEnumTable;
        }


        
        private static System.Data.DataTable SetEnumTable(string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull, bool OrderByDisplayText, bool IncludeDescription, string Restriction)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            System.Data.DataColumn dcCode = new System.Data.DataColumn("Code", typeof(string));
            System.Data.DataColumn dcDisplayText = new System.Data.DataColumn("DisplayText", typeof(string));
            System.Data.DataColumn dcDescription = new System.Data.DataColumn("Description", typeof(string));
            DtEnumTable.Columns.Add(dcCode);
            DtEnumTable.Columns.Add(dcDisplayText);
            DtEnumTable.Columns.Add(dcDescription);
            string SQL = "";
            if (IncludeNull) SQL += "SELECT NULL AS Code, NULL AS DisplayText, NULL AS Description, NULL as DisplayOrder  UNION ";
            if (IncludeDescription)
            {
                SQL += " SELECT Code, DisplayText + CASE WHEN Description IS NULL THEN '' " +
                    " ELSE CASE WHEN DisplayText = Description THEN '' " +
                    " ELSE replicate(' ', cast(((select max(len(DisplayText)) from " + EnumTableName + " where DisplayEnable = 1)- len(DisplayText)) * 1.5 as int)) + '   =   ' + Description " +
                    " END END AS DisplayText, Description, DisplayOrder FROM dbo." + DtEnumTable +
                    " WHERE (DisplayEnable = 1)";
            }
            else
            {
                SQL += " SELECT Code, DisplayText, Description, DisplayOrder FROM dbo." + DtEnumTable +
                    " WHERE (DisplayEnable = 1)";
            }
            if (Restriction.Length > 0)
                SQL += " AND " + Restriction + " ";
            if (OrderByDisplayText) SQL += " ORDER BY DisplayText";
            else SQL += " ORDER BY DisplayOrder";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, conn);
                a.Fill(DtEnumTable);
            }
            catch { }
            return DtEnumTable;
        }

        /// <summary>
        /// Returns the Enum table
        /// </summary>
        /// <param name="EnumTableName">The Enum table</param>
        /// <param name="conn">the SQL connection</param>
        /// <param name="IncludeNull">if the NULL value should be included</param>
        /// <param name="OrderByDisplay">If the item should be ordered by the display text</param>
        /// <param name="IncludeDescription">If the description text should be included in the display text</param>
        /// <param name="IncludeHierarchy">If the hierarchy should be included</param>
        /// <returns></returns>
        private static System.Data.DataTable SetEnumTable(string EnumTableName, Microsoft.Data.SqlClient.SqlConnection conn, bool IncludeNull, bool OrderByDisplay, bool IncludeDescription, bool IncludeHierarchy)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            System.Data.DataColumn dcCode = new System.Data.DataColumn("Code", typeof(string));
            System.Data.DataColumn dcDisplayText = new System.Data.DataColumn("DisplayText", typeof(string));
            System.Data.DataColumn dcDescription = new System.Data.DataColumn("Description", typeof(string));
            DtEnumTable.Columns.Add(dcCode);
            DtEnumTable.Columns.Add(dcDisplayText);
            DtEnumTable.Columns.Add(dcDescription);
            string SQL = "";
            if (IncludeNull)
            {
                SQL += "SELECT NULL AS Code, NULL AS DisplayText, NULL AS Description, NULL as DisplayOrder ";
                if (IncludeHierarchy) SQL += ", ParentCode AS NULL";
                SQL += " UNION ";
            }
            SQL += " SELECT Code, DisplayText + CASE WHEN Description IS NULL THEN '' ELSE ' - ' + Description END AS DisplayText, Description, DisplayOrder";
            if (IncludeHierarchy) SQL += ", ParentCode";
            SQL += "FROM dbo." + DtEnumTable + " WHERE (DisplayEnable = 1)";
            if (OrderByDisplay) SQL += " ORDER BY DisplayText";
            else SQL += " ORDER BY DisplayOrder";
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, conn);
                a.Fill(DtEnumTable);
            }
            catch { }
            return DtEnumTable;
        }

        #endregion

        #endregion

        #region Enum for Query

        public static System.Data.DataTable EnumTableForQuery(string EnumTableName)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "SELECT NULL AS [Value], NULL AS [Display], NULL AS Description, NULL as DisplayOrder  UNION " +
                        " SELECT Code, DisplayText, Description, DisplayOrder FROM dbo." + DtEnumTable +
                        " WHERE (DisplayEnable = 1) ORDER BY DisplayOrder";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(DtEnumTable);
                }
            }
            catch 
            { 
            }
            return DtEnumTable;
        }

        //public static System.Data.DataTable EnumTableForQuery(string EnumTableName, string ConnectionString)
        //{
        //    System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
        //    try
        //    {
        //        if (ConnectionString.Length > 0)
        //        {
        //            string SQL = "SELECT NULL AS [Value], NULL AS [Display], NULL AS Description, NULL as DisplayOrder  UNION " +
        //                " SELECT Code, DisplayText, Description, DisplayOrder FROM dbo." + DtEnumTable +
        //                " WHERE (DisplayEnable = 1) ORDER BY DisplayOrder";
        //            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
        //            a.Fill(DtEnumTable);
        //        }
        //    }
        //    catch { }
        //    return DtEnumTable;
        //}

        /// <summary>
        /// Returns a standard enum table with the columns Code as value and DisplayText
        /// </summary>
        /// <param name="EnumTableName">The name of the table</param>
        /// <param name="IncludeDescription">If the description should be included into the display text</param>
        /// <param name="IncludeNull">If a null value should be included as first dataset</param>
        /// <param name="OrderByDisplayText">If the content should be sorted by the displaytext and not the displayorder</param>
        /// <returns></returns>
        public static System.Data.DataTable EnumTableForQuery(string EnumTableName, bool IncludeDescription, bool IncludeNull, bool OrderByDisplayText)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    string SQL = "";
                    if (IncludeNull)
                    {
                        SQL += "SELECT NULL AS Code, NULL AS DisplayText, NULL AS Description, NULL AS ParentCode, NULL as DisplayOrder ";
                        SQL += " UNION ";
                    }
                    SQL += " SELECT Code, ";
                    if (IncludeDescription)
                        SQL += "DisplayText + CASE WHEN Description IS NULL THEN '' " +
                            " ELSE CASE WHEN DisplayText = Description THEN '' " +
                            " ELSE replicate(' ', cast(((select max(len(DisplayText)) from " + EnumTableName + " where DisplayEnable = 1)- len(DisplayText)) * 1.5 as int)) + '   =   ' + Description " +
                            " END END AS DisplayText ";
                    else
                        SQL += "DisplayText ";
                    SQL += ", Description, ParentCode, DisplayOrder FROM dbo." + DtEnumTable + " WHERE (DisplayEnable = 1) ";
                    if (OrderByDisplayText)
                        SQL += " ORDER BY DisplayText";
                    else SQL += " ORDER BY DisplayOrder";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(DtEnumTable);
                }
            }
            catch { }
            return DtEnumTable;
        }

        public static System.Data.DataTable EnumTableForQuery(string EnumTableName, string Database)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    DiversityWorkbench.ServerConnection S = new ServerConnection();
                    S.DatabaseName = Database;
                    S.DatabasePassword = DiversityWorkbench.Settings.Password;
                    S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                    S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                    S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                    S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                    S.IsLocalExpressDatabase = DiversityWorkbench.Settings.IsLocalExpressDatabase;
                    S.SqlExpressDbFileName = DiversityWorkbench.Settings.SqlExpressDbFileName;
                    string ConnectionString = S.ConnectionString;
                    string SQL = "SELECT NULL AS [Value], NULL AS [Display], NULL AS Description, NULL as DisplayOrder  UNION " +
                        " SELECT Code, DisplayText, Description, DisplayOrder FROM dbo." + DtEnumTable +
                        " WHERE (DisplayEnable = 1) ORDER BY Display, DisplayOrder ";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                    a.Fill(DtEnumTable);
                }
            }
            catch { }
            return DtEnumTable;
        }

        public static System.Data.DataTable EnumTableForQuery(string EnumTableName, string Database, string LinkedServer)
        {
            System.Data.DataTable DtEnumTable = new System.Data.DataTable(EnumTableName);
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    DiversityWorkbench.ServerConnection S = new ServerConnection();
                    S.DatabaseName = Database;
                    S.DatabasePassword = DiversityWorkbench.Settings.Password;
                    S.DatabaseServer = DiversityWorkbench.Settings.DatabaseServer;
                    S.DatabaseServerPort = DiversityWorkbench.Settings.DatabasePort;
                    S.DatabaseUser = DiversityWorkbench.Settings.DatabaseUser;
                    S.IsTrustedConnection = DiversityWorkbench.Settings.IsTrustedConnection;
                    S.IsLocalExpressDatabase = DiversityWorkbench.Settings.IsLocalExpressDatabase;
                    S.SqlExpressDbFileName = DiversityWorkbench.Settings.SqlExpressDbFileName;
                    S.LinkedServer = LinkedServer;
                    string ConnectionString = S.ConnectionString;
                    if (ConnectionString == "")
                        return DtEnumTable;
                    string Prefix = Database + ".dbo.";
                    if (LinkedServer.Length > 0)
                        Prefix = "[" + LinkedServer + "]." + Prefix;
                    string SQL = "SELECT NULL AS [Value], NULL AS [Display], NULL AS Description, NULL as DisplayOrder  UNION " +
                        " SELECT Code, DisplayText, Description, DisplayOrder FROM " + Prefix + EnumTableName +
                        " WHERE (DisplayEnable = 1) ORDER BY DisplayOrder, Display";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                    a.Fill(DtEnumTable);
                }
            }
            catch (System.Exception ex) { }
            return DtEnumTable;
        }

        #endregion

        #region Filling Enum table

        public static void FillEnumTable(System.Data.DataTable EnumTable, bool OrderByDisplayOrder)
        {
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    System.Collections.Generic.List<string> Columns = new System.Collections.Generic.List<string>();
                    foreach (System.Data.DataColumn C in EnumTable.Columns)
                        Columns.Add(C.ColumnName);
                    string SQL = "SELECT ";
                    foreach (string C in Columns)
                        SQL += " NULL AS [" + C + "],"; 
                    SQL = SQL.Substring(0, SQL.Length - 1) + " UNION SELECT ";
                    foreach (string C in Columns)
                        SQL += " [" + C + "],"; 
                    SQL = SQL.Substring(0, SQL.Length - 1) + " FROM dbo." + EnumTable.TableName +
                        " WHERE (DisplayEnable = 1)" ;
                    if (OrderByDisplayOrder) SQL += " ORDER BY DisplayOrder";
                    else SQL += " ORDER BY DisplayText";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(EnumTable);
                }
            }
            catch { }
        }

        public static void FillEnumTable(System.Data.DataTable EnumTable, bool includeNULL, bool OrderByDisplayOrder)
        {
            try
            {
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    System.Collections.Generic.List<string> Columns = new System.Collections.Generic.List<string>();
                    foreach (System.Data.DataColumn C in EnumTable.Columns)
                        Columns.Add(C.ColumnName);
                    string SQL = "SELECT ";
                    if (includeNULL)
                    {
                        foreach (string C in Columns)
                            SQL += " NULL AS [" + C + "],";
                        SQL = SQL.Substring(0, SQL.Length - 1) + " UNION SELECT ";
                    }
                    foreach (string C in Columns)
                        SQL += " [" + C + "],";
                    SQL = SQL.Substring(0, SQL.Length - 1) + " FROM dbo." + EnumTable.TableName +
                        " WHERE (DisplayEnable = 1)";
                    if (OrderByDisplayOrder) SQL += " ORDER BY DisplayOrder";
                    else SQL += " ORDER BY DisplayText";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    a.Fill(EnumTable);
                }
            }
            catch { }
        }

        #endregion

        #region LinkToProject

        public static bool ProjectSelectionAvailable(string EnumTable)
        {
            bool Available = false;
            string SQL = "SELECT COUNT(*) " + ProjectFromClause(EnumTable, "C.COLUMN_NAME = 'ProjectID'");
            int i = 0;
            if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i) && i > 0)
                Available = true;
            return Available;
        }

        public static bool ProjectRestrictionForItems(string EnumTable, int ProjectID)
        {
            string SQL = "SELECT COUNT(*) FROM " + ProjectLinkTable(EnumTable) + " WHERE ProjectID = " + ProjectID.ToString();
            int i = 0;
            return (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i) && i > 0);
        }

        public static bool ProjectRestrictionForItem(string EnumTable, string Code)
        {
            string SQL = "SELECT COUNT(*) FROM " + ProjectLinkTable(EnumTable) + " WHERE "  + ProjectLinkColumn(EnumTable) + " = '" + Code + "'";
            int i = 0;
            return (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i) && i > 0);
        }

        public static System.Collections.Generic.List<string> ProjectAvailableCodes(string EnumTable, string ProjectTable, string ProjectTableIdentityColumn, int ID)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            try
            {
                if (ID > -1)
                {
                    string projectLinkTable = ProjectLinkTable(EnumTable);
                    if (projectLinkTable.Length > 0)
                    {
                        // Check if any not restricted entries do exist
                        string SQL = "SELECT COUNT(*) FROM CollectionProject P " +
                            "WHERE  P.CollectionSpecimenID = " + ID.ToString() +
                            " AND P.ProjectID NOT IN (SELECT P.ProjectID FROM " + projectLinkTable + " E INNER JOIN " + ProjectTable + " P ON E.ProjectID = P.ProjectID AND P." + ProjectTableIdentityColumn + " = " + ID.ToString() + ")";
                        int i = 0;
                        if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true), out i) && i > 0)
                            return list;

                        // Entries in ProjectLinkTable
                        SQL = "SELECT " + ProjectLinkColumn(EnumTable) + " FROM " + projectLinkTable + " E INNER JOIN " + ProjectTable + " P ON E.ProjectID = P.ProjectID AND P." + ProjectTableIdentityColumn + " = " + ID.ToString();
                        System.Data.DataTable dt = new System.Data.DataTable();
                        DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                            list.Add(R[0].ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return list;
        }

        public static System.Collections.Generic.List<string> ProjectAvailableCodes(string EnumTable, int ProjectID)
        {
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
            string SQL = "SELECT " + ProjectLinkColumn(EnumTable) + " FROM " + ProjectLinkTable(EnumTable) + " WHERE ProjectID = " + ProjectID.ToString();
            System.Data.DataTable dt = new System.Data.DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            foreach (System.Data.DataRow R in dt.Rows)
                list.Add(R[0].ToString());
            return list;
        }

        private static string ProjectFromClause(string EnumTable, string Restriction = "")
        {
            string SQL = " FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP " +
                "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF " +
                "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK " +
                "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON FK.COLUMN_NAME = PK.COLUMN_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                "INNER JOIN INFORMATION_SCHEMA.COLUMNS C ON TF.TABLE_NAME = C.TABLE_NAME " +
                "WHERE(TF.CONSTRAINT_TYPE = 'FOREIGN KEY') " +
                "AND TF.TABLE_NAME = TPK.TABLE_NAME " +
                "AND P.TABLE_NAME = '" + EnumTable + "'";
            if (Restriction.Length > 0)
                SQL += " AND " + Restriction;
            return SQL;
        }

        public static string ProjectLinkTable(string EnumTable)
        {
            // #236
            if (_ProjectLinkTables.ContainsKey(EnumTable))
                return _ProjectLinkTables[EnumTable];

            string Table = "";
            try
            {
                string SQL = "SELECT TOP 1 C.TABLE_NAME " + ProjectFromClause(EnumTable, "C.COLUMN_NAME = 'ProjectID'");
                Table = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, true);
                if (Table == null) Table = "";
                if (Table.Length > 0) _ProjectLinkTables.Add(EnumTable, Table);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return Table;
        }

        // #236
        private static System.Collections.Generic.Dictionary<string, string> _ProjectLinkTables = new System.Collections.Generic.Dictionary<string, string>();
        public static void ClearProjectLinkTableCache()
        {
            _ProjectLinkTables.Clear();
        }

        public static string ProjectLinkColumn(string EnumTable)
        {
            string COLUMN = "";
            try
            {
                string SQL = "SELECT TOP 1 PK.COLUMN_NAME " + ProjectFromClause(EnumTable, "C.TABLE_NAME = '" + ProjectLinkTable(EnumTable) + "'");
                COLUMN = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return COLUMN;
        }




        #endregion

        #region Autocompletion

        private static void setAutoCompletion(System.Windows.Forms.ComboBox ComboBox, System.Windows.Forms.AutoCompleteMode Mode)
        {
            try
            {
                if (ComboBox.DataSource != null)
                {
                    System.Data.DataTable DT = (System.Data.DataTable)ComboBox.DataSource;
                    System.Windows.Forms.AutoCompleteStringCollection StringCollection = new System.Windows.Forms.AutoCompleteStringCollection();
                    foreach (System.Data.DataRow R in DT.Rows)
                        StringCollection.Add(R["DisplayText"].ToString());

                    ComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
                    ComboBox.AutoCompleteCustomSource = StringCollection;
                    ComboBox.AutoCompleteMode = Mode;
                }
            }
            catch { }
        }

        #endregion

        public static string DisplayTextFromCode(string EnumTableName, string Code)
        {
            string DisplayText = "";
            string SQL = "SELECT DisplayText FROM " + EnumTableName + " WHERE Code = '" + Code + "'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                DisplayText = C.ExecuteScalar().ToString();
                con.Close();
            }
            catch { }
            return DisplayText;
        }
        
    }

}
