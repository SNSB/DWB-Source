using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.PostgreSQL
{
    public class View
    {
        private enum Grants { Select, Insert, Update, Delete }

        private DiversityWorkbench.PostgreSQL.Schema _Schema;
        private string _Name;

        public string Name
        {
            get { return _Name; }
            //set { _Name = value; }
        }

        public View(string Name, DiversityWorkbench.PostgreSQL.Schema Schema)
        {
            this._Schema = Schema;
            this._Name = Name;
        }

        public bool ClearView()
        {
            string SQL = "DELETE FROM \"" + this._Schema.Name + "\".\"" + this.Name + "\"";
            return DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
        }

    }
}
