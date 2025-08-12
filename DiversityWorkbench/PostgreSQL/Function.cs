using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.PostgreSQL
{
    public class Function
    {
        private DiversityWorkbench.PostgreSQL.Schema _Schema;
        private string _Name;

        public string Name
        {
            get { return _Name; }
        }

        public Function(string Name, DiversityWorkbench.PostgreSQL.Schema Schema)
        {
            this._Schema = Schema;
            this._Name = Name;
        }

        public bool CanExecute(string Role)
        {
            bool CanDo = false;
            string SQL = "SELECT has_function_privilege('" + Role + "', '\"" + this._Schema + "\".\"" + this._Name + "\"', 'execute');";
            string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
            bool.TryParse(Result, out CanDo);
            return CanDo;
        }

        public bool AllowExecute(string Role, bool CanExecute)
        {
            bool OK = true;
            string SQL = " EXECUTE ON " + this._Name + " TO " + Role + ";";
            if (CanExecute)
                SQL = "GRANT" + SQL;
            else SQL = "REVOKE" + SQL;
            OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
            return OK;
        }

    }
}
