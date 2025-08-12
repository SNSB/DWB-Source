using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public class ProjectTable : DiversityWorkbench.PostgreSQL.Table
    {
        public ProjectTable(string Name, DiversityWorkbench.PostgreSQL.Schema Schema)
        { }

        public bool ClearProject(int ProjectID)
        {
            bool OK = true;
            string SQL = "DELETE FROM \"public\".\"" + this._Name + "\" WHERE ProjectID = " + ProjectID;
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);
            return OK;
        }
    }
}
