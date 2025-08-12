using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.PostgreSQL
{
    public class Table
    {
        #region Properties
        
        public enum Grants { Select, Insert, Update, Delete }

        protected DiversityWorkbench.PostgreSQL.Schema _Schema;
        protected string _Name;
        protected System.Data.DataTable _dt;

        public string Name
        {
            get { return _Name; }
            //set { _Name = value; }
        }
        
        #endregion

        #region Construction

        protected Table()
        { }

        public Table(string Name, DiversityWorkbench.PostgreSQL.Schema Schema)
        {
            this._Schema = Schema;
            this._Name = Name;
        }
        
        #endregion

        #region public functions

        public bool Fill(string SQL)
        {
            bool OK = true;
            this._dt = new System.Data.DataTable(this._Name);
            string Message = "";
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref this._dt, ref Message);
            return OK;
        }

        public bool ClearTable()
        {
            string SQL = "DELETE FROM \"" + this._Schema.Name + "\".\"" + this.Name + "\"";
            return DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
        }

        private string _ColumnsAsString = "";
        private System.Collections.Generic.Dictionary<string, Column> _Columns;
        public System.Collections.Generic.Dictionary<string, Column> Columns()
        {
            if (this._Columns == null)
            {
                string SQL = "SELECT c.column_name, c.ordinal_position, c.column_default, c.is_nullable, c.data_type, c.character_maximum_length, c.numeric_precision, c.datetime_precision " +
                    "FROM information_schema.columns c " +
                    "WHERE c.table_schema = '" + this._Schema + "' " +
                    "AND c.table_name   = '" + this._Name + "' " +
                    "ORDER BY c.ordinal_position";
                System.Data.DataTable dt = new System.Data.DataTable(this._Name);
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    bool IsNullable = true;
                    if (R["is_nullable"].ToString().ToUpper() == "NO")
                        IsNullable = false;
                    string DataType = R["data_type"].ToString();
                    int Length = 0;
                    if (DataType.ToLower() == "integer" || DataType.ToLower() == "smallint"  || DataType.ToLower() == "double precision")
                        Length = int.Parse(R["numeric_precision"].ToString());
                    else if (DataType.ToLower().StartsWith("timestamp"))
                        Length = int.Parse(R["datetime_precision"].ToString());
                    else
                        Length = int.Parse(R["character_maximum_length"].ToString());
                    DiversityWorkbench.PostgreSQL.Column C = new Column(R["column_name"].ToString(), int.Parse(R["ordinal_position"].ToString()), R["column_default"].ToString(), IsNullable, DataType, Length);
                    this._Columns.Add(C.Name(), C);
                    if (this._ColumnsAsString.Length > 0)
                        this._ColumnsAsString += ", ";
                    this._ColumnsAsString += "\"" + C.Name() + "\"";
                }
            }
            return _Columns;
        }

        public string ColumnsAsString() { return this._ColumnsAsString; }
        
        #endregion

        //#region Special functionallity for cache database
        
        //private bool ClearProject(int ProjectID)
        //{
        //    bool OK = true;
        //    string SQL = "DELETE FROM \"public\".\"" + this._Name + "\" WHERE ProjectID = " + ProjectID;
        //    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);
        //    return OK;
        //}
        
        //#endregion  

        public static System.Collections.Generic.Dictionary<string, Column> PrimaryKey(string TableName, string Schema)
        {
            System.Collections.Generic.Dictionary<string, Column> PK = new Dictionary<string, Column>();
            string SQL = "SELECT COLUMN_NAME, ordinal_position AS \"Position\", column_default AS \"Default\", data_type AS \"Type\", character_maximum_length as \"Length\" " +
                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                "WHERE (TABLE_NAME = '" + TableName + "') " +
                "AND TABLE_SCHEMA = '" + Schema + "' " +
                "AND (EXISTS " +
                "(SELECT * " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
            System.Data.DataTable dt = new System.Data.DataTable();
            string Message = "";
            try
            {
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    int Position = int.Parse(R["Position"].ToString());
                    int Length = 0;
                    int.TryParse(R["Length"].ToString(), out Length);
                    DiversityWorkbench.PostgreSQL.Column C = new Column(R[0].ToString(), Position, R["Default"].ToString(), false, "", Length);
                    PK.Add(R[0].ToString(), C);
                }
            }
            catch (System.Exception ex)
            {
            }
            return PK;
        }
       
    }
}
