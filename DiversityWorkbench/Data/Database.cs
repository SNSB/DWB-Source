using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Data
{
    public class Database
    {

        #region Parameter and Properties

        private string _Name;

        public string Name
        {
            get { return _Name; }
        }

        private string _Schema;

        public string Schema
        {
            get
            {
                if (this._Schema == null)
                    this._Schema = "dbo";
                return _Schema;
            }
        }

        private string _ConnectionString = DiversityWorkbench.Settings.ConnectionString;

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Data.Table> _Tables;

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.Data.Table> Tables
        {
            get
            {
                if (this._Tables == null || this._Tables.Count == 0)
                {
                    this._Tables = new Dictionary<string, Table>();
                }
                return _Tables;
            }
        }

        #endregion

        #region Construction

        public Database(System.Collections.Generic.List<string> Exclusions, string Schema = "dbo", bool IncludeViews = false, string ConnectionString = "")
        {
            string SQL = "SELECT TABLE_NAME " +
                "FROM INFORMATION_SCHEMA.TABLES AS T " +
                "WHERE(TABLE_SCHEMA = '" + Schema + "') ";
            if (!IncludeViews)
                SQL += "AND (TABLE_TYPE = 'BASE TABLE') ";
            if (ConnectionString.Length > 0)
                this._ConnectionString = ConnectionString;
            foreach (string E in Exclusions)
            {
                SQL += "AND (TABLE_NAME NOT LIKE '" + E + "')";
            }
            System.Data.DataTable dt = new System.Data.DataTable(); Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(_ConnectionString);
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, con);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                Table table = new Table(R[0].ToString(), Schema, _ConnectionString);
                this.Tables.Add(R[0].ToString(), table);
            }
        }


        #endregion

    }
}
