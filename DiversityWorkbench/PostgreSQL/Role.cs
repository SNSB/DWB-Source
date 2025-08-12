using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace DiversityWorkbench.PostgreSQL
{
    public class Role
    {
        //private System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Database> _Databases;

        //public System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Database> Databases
        //{
        //    get
        //    {
        //        if (this._Databases == null)
        //            this._Databases = new Dictionary<string, Database>();
        //        if (this._Databases.Count == 0)
        //        {
        //            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
        //            {
        //                try
        //                {
        //                    string SQL = "SELECT datname FROM pg_database " +
        //                        "WHERE datname not like 'template%' " +
        //                        "AND datname <> 'postgres' " +
        //                        "ORDER BY datname";
        //                    System.Data.DataTable dt = new DataTable();
        //                    string Message = "";
        //                    DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL,ref dt, ref Message);
        //                    //NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
        //                    //ad.Fill(dt);
        //                    foreach (System.Data.DataRow R in dt.Rows)
        //                    {
        //                        string Database = R[0].ToString();
        //                        DiversityWorkbench.PostgreSQL.Database D = new PostgreSQL.Database(Database, this);
        //                        this._Databases.Add(D.Name, D);
        //                    }
        //                    //ad.Dispose();
        //                }
        //                catch (System.Exception ex)
        //                {
        //                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //                }
        //            }

        //        }
        //        return _Databases;
        //    }
        //}

        //public void RestrictToDiversityWorkbenchModule(string ModuleName)
        //{
        //    System.Collections.Generic.List<string> DbToRemove = new List<string>();
        //    foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Database> KV in this._Databases)
        //    {
        //        DbToRemove.Add(KV.Key);
        //    }
        //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Database> KV in this._Databases)
        //    {
        //        string SQL = "select public.diversityworkbenchmodule()";
        //        string Module = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, DiversityWorkbench.PostgreSQL.Connection.ConnectionString(KV.Key));
        //        if (Module == ModuleName)
        //        {
        //            SQL = "select public.version()";
        //            string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, DiversityWorkbench.PostgreSQL.Connection.ConnectionString(KV.Key));
        //            if (Result != "")
        //                DbToRemove.Remove(KV.Key);
        //        }
        //    }
        //    foreach (string DB in DbToRemove)
        //        this._Databases.Remove(DB);
        //}

        #region Construction

        //public Role(string Name, DiversityWorkbench.PostgreSQL.Server Server)
        //{
        //    this._Name = Name;
        //    //this._Server = Server;
        //}

        public Role(string Name)
        {
            this._Name = Name;
        }
        
        #endregion

        #region Schema usage
        
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _SchemaUsage;

        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> SchemaUsage()
        {
            if (this._SchemaUsage == null)
            {
                this._SchemaUsage = new Dictionary<string, List<string>>();
            }
            if (!this._SchemaUsage.ContainsKey(DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name))
            {
                System.Collections.Generic.List<string> L = new List<string>();
                string SQL = "select schema_name from information_schema.schemata " +
                    "where schema_name not in ('pg_toast','pg_temp_1','pg_toast_temp_1','pg_catalog','information_schema');";
                System.Data.DataTable dt = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    SQL = "SELECT has_schema_privilege('" + this.Name + "', '" + R[0].ToString() + "', 'Usage')";
                    string Privilege = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// .Postgres.PostgresExecuteSqlSkalar(SQL);
                    bool Usage = false;
                    bool.TryParse(Privilege, out Usage);
                    if (Usage)
                    {
                        L.Add(R[0].ToString());
                    }
                }
                this._SchemaUsage.Add(DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name, L);
            }
            return this._SchemaUsage;
        }

        public void ResetSchemaUsage()
        {
            this._SchemaUsage = null;
        }
        
        #endregion

        #region Membership
        
        public bool GrantMembership(string MemberOf, bool WithAdminOption = false)
        {
            string SQL = "grant \"" + MemberOf + "\" to \"" + this.Name + "\"";
            if (WithAdminOption)
                SQL += " WITH ADMIN OPTION";
            SQL += "; ";
            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))
            {
                this._MemberShipList = null;
                return true;
            }
            return false;
        }

        public bool RevokeMembership(string NoMemberOf)
        {
            string SQL = "REVOKE \"" + NoMemberOf + "\" FROM \"" + this.Name + "\";";
            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))
            {
                this._MemberShipList = null;
                return true;
            }
            else
                return false;

        }

        private System.Collections.Generic.List<string> _MemberShipList;

        public System.Collections.Generic.List<string> MemberShipList()
        {
            if (this._MemberShipList == null)
            {
                this._MemberShipList = new List<string>();
                string SQL = "select g.rolname " +
                    "from pg_auth_members m, pg_roles r, pg_roles g " +
                    "where m.member = r.oid " +
                    "and m.roleid = g.oid " +
                    "and r.rolname = '" + this.Name + "'";
                System.Data.DataTable dtMember = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtMember, ref Message);
                foreach (System.Data.DataRow R in dtMember.Rows)
                {
                    this._MemberShipList.Add(R[0].ToString());
                }
            }
            return this._MemberShipList;
        }
        
        #endregion

        #region Parameter and Properties
        
        private string _Name;

        public string Name
        {
            get { return _Name; }
        }

        //private DiversityWorkbench.PostgreSQL.Server _Server;

        //public DiversityWorkbench.PostgreSQL.Server Server
        //{
        //    get { return _Server; }
        //}

        private string _Description;

        public string Description
        {
            get
            {
                if (this._Description == null)
                {

                }
                return _Description;
            }
            set
            {
                string SQL = "COMMENT ON ROLE \"" + this._Name + "\" " +
                    "IS '" + value + "';";
                DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);
                _Description = value;
            }
        }

        public bool CanLogin;
        public bool CanCreateDatabase;
        public bool CanCreateRoles;
        public bool IsSuperuser;
        public bool Inherit;
        
        #endregion
    }
}
