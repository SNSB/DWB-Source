using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Windows.Forms;

namespace DiversityWorkbench.SqlLite
{
    public class Database
    {

        private string _Path;
        private System.Data.SQLite.SQLiteConnection _Connection;
        private string _DatabaseName;

        public Database(string Path)
        {
            System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            return;

            try
            {
                this._Path = Path;
                System.IO.FileInfo FI = new System.IO.FileInfo(Path);
                if (!FI.Exists)
                {
                    System.Data.SQLite.SQLiteConnection.CreateFile(FI.FullName);
                }
                this._Connection = new SQLiteConnection("Data Source=" + FI.FullName + ";Version=3;");
                this._DatabaseName = FI.Name;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            //{"Unable to load DLL 'SQLite.Interop.dll': Das angegebene Modul wurde nicht gefunden. (Exception from HRESULT: 0x8007007E)"}
        }

        public string DatabaseName()
        {
            return this._DatabaseName;
        }

        public int TableRowCount(string TableName)
        {
            int i = 0;
            if (!TableName.StartsWith("[") && !TableName.EndsWith("]"))
                TableName = "[" + TableName + "]";
            string SQL = "SELECT COUNT(*) FROM " + TableName;
            try
            {
                if (this._Connection.State == System.Data.ConnectionState.Closed)
                    this._Connection.Open();
                System.Data.SQLite.SQLiteCommand com = new SQLiteCommand(SQL, this._Connection);
                string Result = com.ExecuteScalar().ToString();
                int.TryParse(Result, out i);
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            return i;
        }

        public bool AddTable(string TableName, System.Collections.Generic.Dictionary<string, string> Columns)
        {
            bool OK = true;
            if (!TableName.EndsWith("]") && !TableName.StartsWith("["))
                TableName = "[" + TableName + "]";
            string SQL = "";
            try
            {
                if (this._Connection.State == System.Data.ConnectionState.Closed)
                    this._Connection.Open();
                SQL = "SELECT count(*) FROM sqlite_master where tbl_name ='" + TableName.Replace("[", "").Replace("]", "") + "'";
                 
                System.Data.SQLite.SQLiteCommand com = new SQLiteCommand(SQL, this._Connection);
                try
                {
                    string Result = com.ExecuteScalar().ToString();
                    if (Result == "1")
                    {
                        com.CommandText = "DROP TABLE " + TableName;
                        com.ExecuteNonQuery();
                    }
                }
                catch (System.Exception ex)
                {
                }
                SQL = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Columns)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += KV.Key + " " + KV.Value;
                }
                SQL = "CREATE TABLE " + TableName + "(" + SQL + ");";
                com.CommandText = SQL;
                com.ExecuteNonQuery();
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                OK = false;
            }
            finally
            {
                if (this._Connection.State == System.Data.ConnectionState.Open)
                    this._Connection.Close();
            }
            return OK;
        }

        private bool ColumNamesOK(System.Collections.Generic.Dictionary<string, string> Columns)
        {
            bool OK = true;
            System.Collections.Generic.List<string> NotAllowedColumnNames = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Columns)
            {
                if (!DiversityWorkbench.SqlLite.Database.IsValidName(KV.Key))
                {
                    NotAllowedColumnNames.Add(KV.Key);
                }
            }
            if (NotAllowedColumnNames.Count > 0)
            {
                string Message = "The following headers must be changed to a column compatible string:\r\n";
                foreach (string H in NotAllowedColumnNames)
                {
                    Message += H + "\r\n";
                }
                System.Windows.Forms.MessageBox.Show(Message);
            }
            return OK;
        }

        public static bool IsValidName(string Name)
        {
            if (Name.Length == 0)
                return false;

            bool OK = true;
            // Checking keywords
            if (Name.ToUpper() == "ABORT" ||
                Name.ToUpper() == "ACTION" ||
                Name.ToUpper() == "ADD" ||
                Name.ToUpper() == "AFTER" ||
                Name.ToUpper() == "ALL" ||
                Name.ToUpper() == "ALTER" ||
                Name.ToUpper() == "ANALYZE" ||
                Name.ToUpper() == "AND" ||
                Name.ToUpper() == "AS" ||
                Name.ToUpper() == "ASC" ||
                Name.ToUpper() == "ATTACH" ||
                Name.ToUpper() == "AUTOINCREMENT" ||
                Name.ToUpper() == "BEFORE" ||
                Name.ToUpper() == "BEGIN" ||
                Name.ToUpper() == "BETWEEN" ||
                Name.ToUpper() == "BY" ||
                Name.ToUpper() == "CASCADE" ||
                Name.ToUpper() == "CASE" ||
                Name.ToUpper() == "CAST" ||
                Name.ToUpper() == "CHECK" ||
                Name.ToUpper() == "COLLATE" ||
                Name.ToUpper() == "COLUMN" ||
                Name.ToUpper() == "COMMIT" ||
                Name.ToUpper() == "CONFLICT" ||
                Name.ToUpper() == "CONSTRAINT" ||
                Name.ToUpper() == "CREATE" ||
                Name.ToUpper() == "CROSS" ||
                Name.ToUpper() == "CURRENT_DATE" ||
                Name.ToUpper() == "CURRENT_TIME" ||
                Name.ToUpper() == "CURRENT_TIMESTAMP" ||
                Name.ToUpper() == "DATABASE" ||
                Name.ToUpper() == "DEFAULT" ||
                Name.ToUpper() == "DEFERRABLE" ||
                Name.ToUpper() == "DEFERRED" ||
                Name.ToUpper() == "DELETE" ||
                Name.ToUpper() == "DESC" ||
                Name.ToUpper() == "DETACH" ||
                Name.ToUpper() == "DISTINCT" ||
                Name.ToUpper() == "DROP" ||
                Name.ToUpper() == "EACH" ||
                Name.ToUpper() == "ELSE" ||
                Name.ToUpper() == "END" ||
                Name.ToUpper() == "ESCAPE" ||
                Name.ToUpper() == "EXCEPT" ||
                Name.ToUpper() == "EXCLUSIVE" ||
                Name.ToUpper() == "EXISTS" ||
                Name.ToUpper() == "EXPLAIN" ||
                Name.ToUpper() == "FAIL" ||
                Name.ToUpper() == "FOR" ||
                Name.ToUpper() == "FOREIGN" ||
                Name.ToUpper() == "FROM" ||
                Name.ToUpper() == "FULL" ||
                Name.ToUpper() == "GLOB" ||
                Name.ToUpper() == "GROUP" ||
                Name.ToUpper() == "HAVING" ||
                Name.ToUpper() == "IF" ||
                Name.ToUpper() == "IGNORE" ||
                Name.ToUpper() == "IMMEDIATE" ||
                Name.ToUpper() == "IN" ||
                Name.ToUpper() == "INDEX" ||
                Name.ToUpper() == "INDEXED" ||
                Name.ToUpper() == "INITIALLY" ||
                Name.ToUpper() == "INNER" ||
                Name.ToUpper() == "INSERT" ||
                Name.ToUpper() == "INSTEAD" ||
                Name.ToUpper() == "INTERSECT" ||
                Name.ToUpper() == "INTO" ||
                Name.ToUpper() == "IS" ||
                Name.ToUpper() == "ISNULL" ||
                Name.ToUpper() == "JOIN" ||
                Name.ToUpper() == "KEY" ||
                Name.ToUpper() == "LEFT" ||
                Name.ToUpper() == "LIKE" ||
                Name.ToUpper() == "LIMIT" ||
                Name.ToUpper() == "MATCH" ||
                Name.ToUpper() == "NATURAL" ||
                Name.ToUpper() == "NO" ||
                Name.ToUpper() == "NOT" ||
                Name.ToUpper() == "NOTNULL" ||
                Name.ToUpper() == "NULL" ||
                Name.ToUpper() == "OF" ||
                Name.ToUpper() == "OFFSET" ||
                Name.ToUpper() == "ON" ||
                Name.ToUpper() == "OR" ||
                Name.ToUpper() == "ORDER" ||
                Name.ToUpper() == "OUTER" ||
                Name.ToUpper() == "PLAN" ||
                Name.ToUpper() == "PRAGMA" ||
                Name.ToUpper() == "PRIMARY" ||
                Name.ToUpper() == "QUERY" ||
                Name.ToUpper() == "RAISE" ||
                Name.ToUpper() == "RECURSIVE" ||
                Name.ToUpper() == "REFERENCES" ||
                Name.ToUpper() == "REGEXP" ||
                Name.ToUpper() == "REINDEX" ||
                Name.ToUpper() == "RELEASE" ||
                Name.ToUpper() == "RENAME" ||
                Name.ToUpper() == "REPLACE" ||
                Name.ToUpper() == "RESTRICT" ||
                Name.ToUpper() == "RIGHT" ||
                Name.ToUpper() == "ROLLBACK" ||
                Name.ToUpper() == "ROW" ||
                Name.ToUpper() == "SAVEPOINT" ||
                Name.ToUpper() == "SELECT" ||
                Name.ToUpper() == "SET" ||
                Name.ToUpper() == "TABLE" ||
                Name.ToUpper() == "TEMP" ||
                Name.ToUpper() == "TEMPORARY" ||
                Name.ToUpper() == "THEN" ||
                Name.ToUpper() == "TO" ||
                Name.ToUpper() == "TRANSACTION" ||
                Name.ToUpper() == "TRIGGER" ||
                Name.ToUpper() == "UNION" ||
                Name.ToUpper() == "UNIQUE" ||
                Name.ToUpper() == "UPDATE" ||
                Name.ToUpper() == "USING" ||
                Name.ToUpper() == "VACUUM" ||
                Name.ToUpper() == "VALUES" ||
                Name.ToUpper() == "VIEW" ||
                Name.ToUpper() == "VIRTUAL" ||
                Name.ToUpper() == "WHEN" ||
                Name.ToUpper() == "WHERE" ||
                Name.ToUpper() == "WITH" ||
                Name.ToUpper() == "WITHOUT")
            {
                OK = false;
            }
            // checking signs
            if (!Name.StartsWith("[") && !Name.EndsWith("]"))
            {
                foreach (char C in Name)
                {
                    if (!char.IsLetterOrDigit(C))
                    {
                        OK = false;
                        break;
                    }
                }
            }
            // checking first character
            if (char.IsDigit(Name[0]))
                OK = false;

            return OK;
        }

        public bool InsertData(string TableName, System.Collections.Generic.Dictionary<string, string> ColumnValues)
        {
            bool OK = true;
            if (!TableName.StartsWith("[") && !TableName.EndsWith("]"))
                TableName = "[" + TableName + "]";
            if (!DiversityWorkbench.SqlLite.Database.IsValidName(TableName))
            {
                System.Windows.Forms.MessageBox.Show(TableName + " is not a valid table name");
                return false;
            }
            string SQL = "";
            string SqlColumns = "";
            string SqlValues = "";
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ColumnValues)
                {
                    if (SqlColumns.Length > 0) SqlColumns += ", ";
                    SqlColumns += KV.Key;
                    if (SqlValues.Length > 0) SqlValues += ", ";
                    if (KV.Value.Length == 0)
                        SqlValues += "NULL";
                    else
                        SqlValues += "'" + KV.Value.Replace("'", "''") + "'";
                }
                SQL = "INSERT INTO " + TableName + "(" + SqlColumns + ") values (" + SqlValues + ");";
                if (this._Connection.State == System.Data.ConnectionState.Closed)
                    this._Connection.Open();
                System.Data.SQLite.SQLiteCommand com = new SQLiteCommand(SQL, this._Connection);
                com.ExecuteNonQuery();
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                OK = false;
            }
            return OK;
        }

        public System.Data.DataTable ViewTableContent(string TableName)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "";
            try
            {
                SQL = "SELECT * FROM " + TableName;
                if (this._Connection.State == System.Data.ConnectionState.Closed)
                    this._Connection.Open();
                System.Data.SQLite.SQLiteDataAdapter ad = new SQLiteDataAdapter(SQL, this._Connection);
                ad.Fill(dt);
            }
            catch (SQLiteException ex)
            {
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Sorry - something went wrong:\r\n" + SQL + "\r\nPlease try to open the database with another tool\r\nError message: " + ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// The version of SQLite
        /// </summary>
        /// <returns>the version</returns>
        public string Version()
        {
            string Version = "";
            string cs = "Data Source=:memory:";

            System.Data.SQLite.SQLiteConnection con = null;
            System.Data.SQLite.SQLiteCommand cmd = null;

            try
            {
                con = new System.Data.SQLite.SQLiteConnection(cs);
                con.Open();

                string stm = "SELECT SQLITE_VERSION()";
                cmd = new System.Data.SQLite.SQLiteCommand(stm, con);

                Version = Convert.ToString(cmd.ExecuteScalar());
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
            finally
            {
                if (cmd != null)
                {
                    cmd.Dispose();
                }

                if (con != null)
                {
                    try
                    {
                        con.Close();

                    }
                    catch (System.Data.SQLite.SQLiteException ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        con.Dispose();
                    }
                }
            }
            return Version;
        }

    }

}
