using System;

namespace DiversityWorkbench
{
    /// <summary>
    /// Zusammenfassung für EntityDescription.
    /// </summary>
    public class EntityDescription
    {
        private System.Windows.Forms.Form p_Form;
        private System.Data.OleDb.OleDbConnection p_Conn;
        private System.Windows.Forms.ToolTip p_ToolTip;
        public EntityDescription(System.Windows.Forms.Form Form, System.Windows.Forms.ToolTip ToolTip, System.Data.OleDb.OleDbConnection Conn)
        {
            this.p_Form = Form;
            this.p_Conn = Conn;
            this.p_ToolTip = ToolTip;
        }
        #region Description

        public void setDescriptions()
        {
            foreach (System.Windows.Forms.Control C in this.p_Form.Controls)
            {
                if (C.GetType() == typeof(System.Windows.Forms.GroupBox) || C.GetType() == typeof(System.Windows.Forms.Panel))
                    this.setDescriptions(C);
            }
        }

        private void setDescriptions(System.Windows.Forms.Control Cont)
        {
            foreach (System.Windows.Forms.Control C in Cont.Controls)
            {
                object[] ar = new Object[9] { "", "", "", "", "", "", "", "", "" };
                string Table = "";
                string Column = "";
                string Database = "";
                if (C.GetType() == typeof(System.Windows.Forms.CheckBox) || C.GetType() == typeof(System.Windows.Forms.TextBox) || C.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    try
                    {
                        C.DataBindings.CopyTo(ar, 0);
                        if (ar[0].ToString() != "")
                        {
                            System.Windows.Forms.Binding B = (System.Windows.Forms.Binding)ar[0];
                            Column = B.BindingMemberInfo.BindingField;
                            Table = B.BindingMemberInfo.BindingPath;
                            Database = this.p_Conn.Database.ToString();
                            string ToolTipText = this.ColumnDescription(Table, Column, Database);
                            this.p_ToolTip.SetToolTip(C, ToolTipText);
                        }
                    }
                    catch { }
                }
                else if (C.GetType() == typeof(System.Windows.Forms.GroupBox) ||
                    C.GetType() == typeof(System.Windows.Forms.Panel) ||
                    C.GetType() == typeof(System.Windows.Forms.TabControl) ||
                    C.GetType() == typeof(System.Windows.Forms.TabPage))
                    this.setDescriptions(C);
            }
        }
        /// <summary>
        /// the description of a table column
        /// </summary>
        /// <param name="Table">the database table</param>
        /// <param name="Column">the column</param>
        /// <param name="Database">the database</param>
        /// <returns>the description of the column</returns>
        public string ColumnDescription(string Table, string Column, string Database)
        {
            string Description = "";
            if (Table.IndexOf("_Main20") > 0)
                Table = Table.Substring(0, Table.IndexOf("_Main20"));
            /*switch (Database)
			{
				case "DiversityCollection":
					Description = DB.ColumnDescriptionCollection(Table, Column);
					break;
			}*/
            Description = DiversityWorkbench.Forms.FormFunctions.getDescriptionCache(Table, Column);
            if (Description == null || Description.Length == 0)
            {
                System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(this.p_Conn.ConnectionString);
                string SQL = "SELECT     max(CONVERT(varchar(300), [value])) " +
                    " FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table +
                    "', 'column', '" + Column + "') [::fn_listextendedproperty_1]";
                System.Data.OleDb.OleDbCommand Com = new System.Data.OleDb.OleDbCommand(SQL, conn);
                conn.Open();
                System.Data.OleDb.OleDbDataReader r;
                r = Com.ExecuteReader();
                try
                {
                    while (r.Read())
                    {
                        if (!r.IsDBNull(0))
                            Description = r.GetString(0).ToString();
                    }
                }
                catch
                {
                    r.Close();
                }
                conn.Close();
            }
            return Description;
        }

        /// <summary>
        /// Despriction of a Database table of column
        /// </summary>
        /// <param name="TableOrColumn">Table and optionally column separated by a "."</param>
        /// <returns>The description</returns>
        public static string TableColumnDescription(string TableOrColumn)
        {
            string Table = TableOrColumn;
            string Column = "";
            if (Table.IndexOf(".") > -1)
            {
                Column = Table.Substring(Table.IndexOf(".") + 1);
                Table = Table.Substring(0, Table.IndexOf("."));
            }
            string Description = DiversityWorkbench.Forms.FormFunctions.getDescriptionCache(Table, Column);
            if (Description == null || Description.Length == 0)
            {
                string SQL = "select value FROM ::fn_listextendedproperty(NULL, 'user', 'dbo', 'table', '" + Table + "'";
                if (Column.Length > 0) SQL += ", 'column', '" + Column + "'";
                SQL += ") WHERE name = 'MS_Description'";
                string Message = "";
                Description = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
            }
            return Description;
        }

        #endregion
    }
}
