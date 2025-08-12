using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public class Package
    {

        #region Properties

        public enum Pack { ABCD, /*ABCD_BayernFlora,*/ FloraRaster, /*GUC,*/ Observation, LIDO }
        private Pack _Pack;
        private string _Name;
        private DiversityCollection.CacheDatabase.Project _Project;
        public DiversityCollection.CacheDatabase.Project Project(){return this._Project;}
        private string _Description;
        private int _Version = 1;

        private System.Collections.Generic.Dictionary<string, object> _TransferHistory = new Dictionary<string, object>();
        
        #endregion

        #region Construction

        public Package(string Name, DiversityCollection.CacheDatabase.Project Project)
        {
            this._Name = Name;
            switch (Name.ToLower())
            {
                case "abcd":
                    this._Pack = Pack.ABCD;
                    break;
                //case "abcd_bayernflora":
                //    this._Pack = Pack.ABCD_BayernFlora;
                //    break;
                case "floraraster":
                    this._Pack = Pack.FloraRaster;
                    break;
                //case "guc":
                //    this._Pack = Pack.GUC;
                //    break;
                case "observation":
                    this._Pack = Pack.Observation;
                    break;
                case "lido":
                    this._Pack = Pack.LIDO;
                    break;
                default:
                    System.Windows.Forms.MessageBox.Show("Package " + Name + " is not implemented so far");
                    break;
            }
            this._Project = Project;
        }
        
        #endregion

        #region Description

        public string GetDescription()
        {
            if (this._Description == null)
            {
                string SQL = "SELECT \"Description\" FROM \"" + this._Project.SchemaName + "\".\"Package\" WHERE \"Package\" = '" + _Name + "';";
                this._Description = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                // getting the objects
                SQL = "SELECT c.relname, d.description " +
                    "FROM pg_class As c " +
                    "LEFT JOIN pg_namespace n ON n.oid = c.relnamespace " +
                    "LEFT JOIN pg_tablespace t ON t.oid = c.reltablespace " +
                    "LEFT JOIN pg_description As d ON (d.objoid = c.oid AND d.objsubid = 0) " +
                    "WHERE n.nspname = '" + this._Project.SchemaName + "' AND d.description > '' AND c.relname like '" + this._Name + "_%' " +
                    "ORDER BY c.relname ;";
                System.Data.DataTable dtTables = new System.Data.DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtTables, ref Message);
                foreach (System.Data.DataRow R in dtTables.Rows)
                {
                    this._Description += "\r\n\r\n" + R[0].ToString() + ": " + R[1].ToString();
                    System.Data.DataTable dtColumns = new System.Data.DataTable();
                    SQL = "SELECT cols.column_name, " +
                        "(SELECT pg_catalog.col_description(c.oid, cols.ordinal_position::int) FROM pg_catalog.pg_class c WHERE c.oid = (SELECT('\"' || cols.table_name || '\"')::regclass::oid) AND c.relname = cols.table_name) AS column_comment " +
                        "FROM information_schema.columns cols " +
                        "WHERE cols.table_catalog = '" + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + "' " +
                        "AND cols.table_name = '" + R[0].ToString() + "' " +
                        "AND cols.table_schema = '" + this._Project.SchemaName + "'; ";
                    DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtColumns, ref Message);
                    //System.Data.DataTable dtColumDescription = new System.Data.DataTable();
                    //SQL = "SELECT cols.column_name, " +
                    //    "(SELECT pg_catalog.col_description(c.oid, cols.ordinal_position::int) " +
                    //    "FROM pg_catalog.pg_class c " +
                    //    "WHERE c.relname = cols.table_name) " +
                    //    "as column_comment " +
                    //    "FROM information_schema.columns cols " +
                    //    "WHERE cols.table_name    = '" + R[0].ToString() + "' " +
                    //    "AND cols.table_schema  = '" + this._Project.SchemaName + "';";
                    //DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtColumns, ref Message);
                    foreach (System.Data.DataRow RC in dtColumns.Rows)
                    {
                        this._Description += "\r\n\t" + RC[0].ToString() + ": " + RC[1].ToString();
                    }
                }
            }
            return this._Description;
        }

        public void setDescription(string Description) { this._Description = Description; }

        public bool IsUsingSchemaPublic()
        {
            return DiversityCollection.CacheDatabase.Package.IsUsingSchemaPublic(this._Pack);
        }
        
        #endregion

        #region Removing

        public bool RemovePackage()
        {
            bool OK = this.RemovePackage("public", this.PackageObjects("public").Count);
            if (OK)
                OK = this.RemovePackage(this._Project.SchemaName, this.PackageObjects(this._Project.SchemaName).Count);
            return OK;
        }

        //private bool RemovePackage(string Schema, int NumberOfObjects)
        //{
        //    bool OK = true;
        //    try
        //    {
        //        // Removing grants from tables and views
        //        string SQL = "select G.table_name, G.grantee from information_schema.role_table_grants G " +
        //            "where G.table_schema = '" + Schema + "' " +
        //            "and G.table_name like '" + _Name + "%' " +
        //            "and G.grantee like '%_" + Schema + "' " +
        //            "and G.grantee <> 'CacheAdmin_" + Schema + "' ";
        //        System.Data.DataTable dtGrants = new System.Data.DataTable();
        //        string Message = "";
        //        DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtGrants, ref Message);
        //        foreach (System.Data.DataRow R in dtGrants.Rows)
        //        {
        //            if (Schema == "public") SQL = "";
        //            else SQL = "SET ROLE \"CacheAdmin_" + Schema + "\"; ";
        //            SQL += "REVOKE ALL PRIVILEGES ON \"" + Schema + "\".\"" + R[0].ToString() + "\" FROM \"" + R[1].ToString() + "\";";
        //            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
        //            if (Message.Length > 0)
        //                OK = false;
        //        }

        //        // Removing the tables
        //        SQL = "SELECT table_name FROM information_schema.tables t " +
        //            "where t.table_name like '" + _Name + "_%'  and t.table_schema = '" + Schema + "' and table_type = 'BASE TABLE' " +
        //            "order by table_name desc";
        //        System.Data.DataTable dtTables = new System.Data.DataTable();
        //        DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtTables, ref Message);
        //        foreach (System.Data.DataRow R in dtTables.Rows)
        //        {
        //            if (Schema == "public") SQL = "";
        //            else SQL = "SET ROLE \"CacheAdmin_" + Schema + "\"; ";
        //            SQL += "DROP TABLE \"" + Schema + "\".\"" + R[0].ToString() + "\" CASCADE;";
        //            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
        //            if (Message.Length > 0)
        //                OK = false;
        //        }

        //        // Removing the views
        //        SQL = "SELECT table_name FROM information_schema.tables t " +
        //            "where t.table_name like '" + _Name + "_%'  and t.table_schema = '" + Schema + "' and table_type = 'VIEW' " +
        //            "order by table_name desc";
        //        System.Data.DataTable dtViews = new System.Data.DataTable();
        //        DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtViews, ref Message);
        //        foreach (System.Data.DataRow R in dtViews.Rows)
        //        {
        //            if (Schema == "public") SQL = "";
        //            else SQL = "SET ROLE \"CacheAdmin_" + Schema + "\"; ";
        //            SQL += "DROP VIEW \"" + Schema + "\".\"" + R[0].ToString() + "\" CASCADE;";
        //            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
        //            if (Message.Length > 0)
        //                OK = false;
        //        }

        //        // Removing grants from materialized views
        //        SQL = "select relname, " +
        //            "coalesce(nullif(s[1], ''), 'public') as grantee " +
        //            "from  " +
        //            "pg_class c " +
        //            "join pg_namespace n on n.oid = relnamespace " +
        //            "join pg_roles r on r.oid = relowner, " +
        //            "unnest(coalesce(relacl::text[], format('{%s=arwdDxt/%s}', rolname, rolname)::text[])) acl,  " +
        //            "regexp_split_to_array(acl, '=|/') s " +
        //            "where relname like '" + _Name + "_%' " +
        //            "and c.oid::regclass::text like '\"" + Schema + "\".\"%' " +
        //            "and c.relkind = 'm'";
        //        System.Data.DataTable dtGrantMatViews = new System.Data.DataTable();
        //        Message = "";
        //        DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtGrantMatViews, ref Message);
        //        foreach (System.Data.DataRow R in dtGrantMatViews.Rows)
        //        {
        //            if (Schema == "public") SQL = "";
        //            else SQL = "SET ROLE \"CacheAdmin_" + Schema + "\"; ";
        //            SQL += "REVOKE ALL PRIVILEGES ON \"" + Schema + "\".\"" + R[0].ToString() + "\" FROM \"" + R[1].ToString() + "\";";
        //            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
        //            if (Message.Length > 0)
        //                OK = false;
        //        }

        //        // Removing the materialized views
        //        SQL = "SELECT relname " +
        //            "FROM   pg_class " +
        //            "WHERE  relkind = 'm' " +
        //            "AND relname like '" + _Name + "_%' " +
        //            "AND oid::regclass::text like '_" + Schema + "_.%';";
        //        System.Data.DataTable dtMatViews = new System.Data.DataTable();
        //        DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtMatViews, ref Message);
        //        foreach (System.Data.DataRow R in dtMatViews.Rows)
        //        {
        //            if (Schema == "public") SQL = "";
        //            else SQL = "SET ROLE \"CacheAdmin_" + Schema + "\" ;";
        //            SQL += "DROP MATERIALIZED VIEW \"" + Schema + "\".\"" + R[0].ToString() + "\" CASCADE;";
        //            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
        //            if (Message.Length > 0)
        //                OK = false;
        //        }

        //        // Removing grants from functions
        //        SQL = "select G.routine_name, G.grantee from information_schema.role_routine_grants G " +
        //            "where G.routine_schema = '" + Schema + "' " +
        //            "and G.routine_name like '" + _Name + "%' " +
        //            "and G.grantee like '%_" + Schema + "'";
        //        dtGrants.Clear();
        //        DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtGrants, ref Message);
        //        foreach (System.Data.DataRow R in dtGrants.Rows)
        //        {
        //            if (Schema == "public") SQL = "";
        //            else SQL = "SET ROLE \"CacheAdmin_" + Schema + "\"; ";
        //            SQL += "REVOKE ALL PRIVILEGES ON \"" + Schema + "\".\"" + R[0].ToString() + "\" FROM \"" + R[1].ToString() + "\";";
        //            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
        //            if (Message.Length > 0)
        //                OK = false;
        //        }

        //        // Removing the functions
        //        SQL = "select concat(r.routine_name, '(',  pg_get_function_identity_arguments(oid), ')') " +
        //            "from pg_proc p, information_schema.routines r  " +
        //            "where r.routine_name like '" + _Name.ToLower() + "_%'  " +
        //            "and p.proname like '" + _Name.ToLower() + "_%' " +
        //            "and p.proname = r.routine_name " +
        //            "and r.specific_schema = '" + Schema + "';";
        //        System.Data.DataTable dtRoutines = new System.Data.DataTable();
        //        DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtRoutines, ref Message);
        //        foreach (System.Data.DataRow R in dtRoutines.Rows)
        //        {
        //            if (Schema == "public") SQL = "";
        //            else SQL = "SET ROLE \"CacheAdmin_" + Schema + "\"; ";
        //            SQL += "DROP FUNCTION \"" + Schema + "\"." + R[0].ToString() + " CASCADE;";
        //            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
        //            if (Message.Length > 0)
        //                OK = false;
        //        }

        //        // Removing entry in table Package
        //        if (Schema != "public" && OK && Message.Length == 0)
        //        {
        //            SQL = "SET ROLE \"CacheAdmin_" + Schema + "\"; DELETE FROM \"" + Schema + "\".\"Package\"" +
        //                "WHERE \"Package\" = '" + this._Name + "';";
        //            DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
        //            if (Message.Length > 0)
        //            {
        //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("RemovePackage", this._Name, Message);
        //                //System.Windows.Forms.MessageBox.Show(Message);
        //                OK = false;
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        OK = false;
        //    }
        //    if (!OK)
        //    {
        //        int i = this.PackageObjects(Schema).Count;
        //        if (i > 0 && i < NumberOfObjects)
        //        {
        //            OK = this.RemovePackage(Schema, i);
        //        }
        //    }
        //    return OK;
        //}

        private bool RemovePackage(string Schema, int NumberOfObjects)
        {
            bool OK = true;
            try
            {
                string Role = "";
                string Message = "";
                string SQL = "";
                if (Schema != "public")
                    Role = "CacheAdmin"; //" + //_" + Schema;

                // Obsolet - Rollen entfernt
                // Removing grants from tables and views
                // SQL = "select G.table_name, G.grantee from information_schema.role_table_grants G " +
                //    "where G.table_schema = '" + Schema + "' " +
                //    "and G.table_name like '" + _Name + "%' " +
                //    "and G.grantee like '%_" + Schema + "' " +
                //    "and G.grantee <> 'CacheAdmin' "; // _" + Schema + "' ";
                //System.Data.DataTable dtGrants = new System.Data.DataTable();
                //DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtGrants, ref Message);
                //foreach (System.Data.DataRow R in dtGrants.Rows)
                //{
                //    SQL = "REVOKE ALL PRIVILEGES ON \"" + Schema + "\".\"" + R[0].ToString() + "\" FROM \"" + R[1].ToString() + "\";";
                //    this.RemoveObject(SQL, Role, ref Message);
                //    if (Message.Length > 0)
                //        OK = false;
                //}

                // Removing the tables
                SQL = "SELECT table_name FROM information_schema.tables t " +
                    "where t.table_name like '" + _Name + "_%'  and t.table_schema = '" + Schema + "' and table_type = 'BASE TABLE' " +
                    "order by table_name desc";
                System.Data.DataTable dtTables = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtTables, ref Message);
                foreach (System.Data.DataRow R in dtTables.Rows)
                {
                    SQL = "DROP TABLE \"" + Schema + "\".\"" + R[0].ToString() + "\" CASCADE;";
                    this.RemoveObject(SQL, Role, ref Message);
                    if (Message.Length > 0)
                        OK = false;
                }

                // Removing the views
                SQL = "SELECT table_name FROM information_schema.tables t " +
                    "where t.table_name like '" + _Name + "_%'  and t.table_schema = '" + Schema + "' and table_type = 'VIEW' " +
                    "order by table_name desc";
                System.Data.DataTable dtViews = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtViews, ref Message);
                foreach (System.Data.DataRow R in dtViews.Rows)
                {
                    // getting the owner of the view
                    if (Role == null || Role.Length == 0)
                        Role = DiversityWorkbench.PostgreSQL.Connection.ObjectOwner(R[0].ToString(), Schema, DiversityWorkbench.PostgreSQL.Connection.ObjectType.View);
                    SQL = "DROP VIEW IF EXISTS \"" + Schema + "\".\"" + R[0].ToString() + "\" CASCADE;";
                    string Error = "";
                    this.RemoveObject(SQL, Role, ref Error);
                    if (Error.Length > 0)
                    {
                        SQL = "select count(*) from information_schema.views where table_schema = '" + Schema + "' and table_name = '" + R[0].ToString() + "'";
                        string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                        if (Result == "0")
                        {
                            OK = true;
                        }
                        else
                        {
                            OK = false;
                            Message += Error;
                        }
                    }
                }

                // Removing grants from materialized views
                SQL = "select relname, " +
                    "coalesce(nullif(s[1], ''), 'public') as grantee " +
                    "from  " +
                    "pg_class c " +
                    "join pg_namespace n on n.oid = relnamespace " +
                    "join pg_roles r on r.oid = relowner, " +
                    "unnest(coalesce(relacl::text[], format('{%s=arwdDxt/%s}', rolname, rolname)::text[])) acl,  " +
                    "regexp_split_to_array(acl, '=|/') s " +
                    "where relname like '" + _Name + "_%' " +
                    "and c.oid::regclass::text like '\"" + Schema + "\".\"%' " +
                    "and c.relkind = 'm'";
                System.Data.DataTable dtGrantMatViews = new System.Data.DataTable();
                Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtGrantMatViews, ref Message);
                foreach (System.Data.DataRow R in dtGrantMatViews.Rows)
                {
                    SQL = "REVOKE ALL PRIVILEGES ON \"" + Schema + "\".\"" + R[0].ToString() + "\" FROM \"" + R[1].ToString() + "\";";
                    this.RemoveObject(SQL, Role, ref Message);
                    if (Message.Length > 0)
                        OK = false;
                }

                // Removing the materialized views
                SQL = "SELECT relname " +
                    "FROM   pg_class " +
                    "WHERE  relkind = 'm' " +
                    "AND relname like '" + _Name + "_%' " +
                    "AND oid::regclass::text like '_" + Schema + "_.%';";
                System.Data.DataTable dtMatViews = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtMatViews, ref Message);
                foreach (System.Data.DataRow R in dtMatViews.Rows)
                {
                    SQL = "DROP MATERIALIZED VIEW \"" + Schema + "\".\"" + R[0].ToString() + "\" CASCADE;";
                    this.RemoveObject(SQL, Role, ref Message);
                    if (Message.Length > 0)
                        OK = false;
                }

                // Removing grants from functions
                // Obsolet - Rollen gelöscht
                //SQL = "select G.routine_name, G.grantee from information_schema.role_routine_grants G " +
                //    "where G.routine_schema = '" + Schema + "' " +
                //    "and G.routine_name like '" + _Name + "%' " +
                //    "and G.grantee like '%_" + Schema + "'";
                //dtGrants.Clear();
                //DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtGrants, ref Message);
                //foreach (System.Data.DataRow R in dtGrants.Rows)
                //{
                //    SQL = "REVOKE ALL PRIVILEGES ON \"" + Schema + "\".\"" + R[0].ToString() + "\" FROM \"" + R[1].ToString() + "\";";
                //    this.RemoveObject(SQL, Role, ref Message);
                //    if (Message.Length > 0)
                //        OK = false;
                //}

                // Removing the functions
                SQL = "select concat(r.routine_name, '(',  pg_get_function_identity_arguments(oid), ')') " +
                    "from pg_proc p, information_schema.routines r  " +
                    "where r.routine_name like '" + _Name.ToLower() + "_%'  " +
                    "and p.proname like '" + _Name.ToLower() + "_%' " +
                    "and p.proname = r.routine_name " +
                    "and r.specific_schema = '" + Schema + "';";
                System.Data.DataTable dtRoutines = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtRoutines, ref Message);
                foreach (System.Data.DataRow R in dtRoutines.Rows)
                {
                    SQL = "DROP FUNCTION \"" + Schema + "\"." + R[0].ToString() + " CASCADE;";
                    this.RemoveObject(SQL, Role, ref Message);
                    if (Message.Length > 0)
                        OK = false;
                }

                // Removing entry in table Package
                if (Schema != "public" && OK && Message.Length == 0)
                {
                    SQL = "DELETE FROM \"" + Schema + "\".\"Package\"" +
                        "WHERE \"Package\" = '" + this._Name + "';";
                    this.RemoveObject(SQL, Role, ref Message);
                    if (Message.Length > 0)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("RemovePackage", this._Name, Message);
                        //System.Windows.Forms.MessageBox.Show(Message);
                        OK = false;
                    }
                }

                // Removing entry in table PackageAddOn
                if (Schema != "public" && OK && Message.Length == 0)
                {
                    SQL = "DELETE FROM \"" + Schema + "\".\"PackageAddOn\"" +
                        "WHERE \"Package\" = '" + this._Name + "';";
                    OK = this.RemoveObject(SQL, Role, ref Message);
                    if (!OK && Message.Length > 0)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("RemovePackage", this._Name, Message);
                        //System.Windows.Forms.MessageBox.Show(Message);
                        OK = false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            if (!OK)
            {
                int i = this.PackageObjects(Schema).Count;
                if (i > 0 && i < NumberOfObjects)
                {
                    OK = this.RemovePackage(Schema, i);
                }
            }
            return OK;
        }

        private bool RemoveObject(string SQL, string Role, ref string Message)
        {
            bool OK = true;
            try
            {
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message);
                if (!OK && Message.Length > 0 && Role.Length > 0 && Message.ToLower().IndexOf("owner") > -1)
                {
                    string Error = "";
                    SQL = "SET ROLE \"" + Role + "\"; " + SQL;
                    OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                    if (!OK && Error.Length > 0)
                        Message += "\r\n" + Error;
                }
                else if (!OK)
                {

                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
                Message = ex.Message;
            }
            return OK;
        }

        private System.Collections.Generic.List<string> PackageObjects(string Schema)
        {
            System.Collections.Generic.List<string> PO = new List<string>();
            try
            {
                string Message = "";
                // Getting the tables
                string SQL = "SELECT table_name FROM information_schema.tables t " +
                    "where t.table_name like '" + _Name + "_%'  and t.table_schema = '" + Schema + "' and table_type = 'BASE TABLE' " +
                    "order by table_name desc";
                System.Data.DataTable dtTables = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtTables, ref Message);
                foreach (System.Data.DataRow R in dtTables.Rows)
                {
                    PO.Add(R[0].ToString());
                }

                // Getting the views
                SQL = "SELECT table_name FROM information_schema.tables t " +
                    "where t.table_name like '" + _Name + "_%'  and t.table_schema = '" + Schema + "' and table_type = 'VIEW' " +
                    "order by table_name desc";
                System.Data.DataTable dtViews = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtViews, ref Message);
                foreach (System.Data.DataRow R in dtViews.Rows)
                {
                    PO.Add(R[0].ToString());
                }

                // Getting the materialized views
                SQL = "SELECT relname " +
                    "FROM   pg_class " +
                    "WHERE  relkind = 'm' " +
                    "AND relname like '" + _Name + "_%' " +
                    "AND oid::regclass::text like '_" + Schema + "_.%';";
                System.Data.DataTable dtMatViews = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtMatViews, ref Message);
                foreach (System.Data.DataRow R in dtMatViews.Rows)
                {
                    PO.Add(R[0].ToString());
                }

                // Getting the functions
                SQL = "select concat(r.routine_name, '(',  pg_get_function_identity_arguments(oid), ')') " +
                    "from pg_proc p, information_schema.routines r  " +
                    "where r.routine_name like '" + _Name.ToLower() + "_%'  " +
                    "and p.proname like '" + _Name.ToLower() + "_%' " +
                    "and p.proname = r.routine_name " +
                    "and r.specific_schema = '" + Schema + "';";
                System.Data.DataTable dtRoutines = new System.Data.DataTable();
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtRoutines, ref Message);
                foreach (System.Data.DataRow R in dtRoutines.Rows)
                {
                    PO.Add(R[0].ToString());
                }

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return PO;
        }
        
        #endregion

        #region Transfer & Update

        public bool TransferData(ref string Error, DiversityCollection.CacheDatabase.InterfacePackage iPackage, ref System.Collections.Generic.Dictionary<string, object> TransferHistory)
        {
            bool OK = true;
            if (Package.TransferSteps(this._Pack, this._Project.SchemaName).Count > 0)//.FunctionsInProjectSchema(this._Pack).Count > 0)
            {
                int iCount = Package.TransferSteps(this._Pack, this._Project.SchemaName).Count; // Package.FunctionsInProjectSchema(this._Pack).Count;
                int i = 1;
                foreach (TransferStep Step in Package.TransferSteps(this._Pack, this._Project.SchemaName))
                {
                    try
                    {
                        if (iPackage != null)
                        {
                            string State = Step.TransferProcedure.Substring(this.Name.Length + 1).Replace("_", " ").Replace("()", "");
                            iPackage.ShowTransferState("(" + i.ToString() + " of " + iCount.ToString() + ") Transfer " + State);
                        }

                        string SQL = "SELECT \"" + this._Project.SchemaName + "\"." + Step.TransferProcedure;
                        if (!SQL.EndsWith("()"))
                            SQL += "()";
                        SQL += "; ";
                        OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
                        if (!OK)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile("Package.TransferData", SQL, Error);
                            //break;
                        }
                        else
                        {
                            try
                            {
                                int iCountStep = 0;
                                SQL = "SELECT COUNT(*) FROM \"" + this._Project.SchemaName + "\".\"" + Step.TableName() + "\"";
                                string CountTableResult = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Error);
                                if (int.TryParse(CountTableResult, out iCountStep) && !TransferHistory.ContainsKey(Step.TableName()))
                                {
                                    TransferHistory.Add(Step.TableName(), iCountStep);
                                    CacheDB.LogEvent("", "",  iCountStep.ToString() + " " + Step.TableName() + " (procedure : " + Step.TransferProcedure + ")");
                                }
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                    i++;
                }
                if (iPackage != null)
                {
                    OK = this.setLastTransferDate();
                    if (OK)
                    {
                        iPackage.ShowTransferState("Transfer finished");
                    }
                    else
                        iPackage.ShowTransferState("Transfer failed");
                }
                else if (OK)
                {
                    OK = this.setLastTransferDate();
                }
            }
            return OK;
        }

        public bool setLastTransferDate()
        {
            bool OK = false;
            try 
            { 
                string Error = "";
                string SQL = "UPDATE \"" + this._Project.SchemaName + "\".\"Package\" " +
                    " SET \"LogLastTransfer\" = CURRENT_TIMESTAMP " +
                    " WHERE \"Package\" = '" + this._Pack.ToString() + "'; ";
                OK = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Error);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return OK;
        }

        public string LastTransferDate()
        {
            string Date = "";
            try
            {
                string SQL = "SELECT to_char(\"LogLastTransfer\", 'YYYY-MM-DD HH24:MI:SS') FROM \"" + this._Project.SchemaName + "\".\"Package\" " +
                            " WHERE \"Package\" = '" + this._Pack.ToString() + "'; "; ;
                Date = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
            }
            catch (System.Exception ex)
            {
            }
            return Date;
        }

        public bool NeedsUpdate()
        {
            bool UpdateNeeded = false;
            string SQL = "SELECT \"Version\" FROM \"" + this._Project.SchemaName + "\".\"Package\" WHERE \"Package\" = '" + this.Name + "';";
            int Version;
            if (int.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out Version))
            {
                if (Version < DiversityCollection.CacheDatabase.Package.Version(this.PackagePack))
                {
                    UpdateNeeded = true;
                }
            }
            return UpdateNeeded;
        }
        
        #endregion

        #region static

        public string Name { get { return this._Name; } }
        public Pack PackagePack { get { return this._Pack; } }
        
        private static System.Collections.Generic.Dictionary<Pack, string> _Packages;
        public static System.Collections.Generic.Dictionary<Pack, string> Packages()
        {
            if (DiversityCollection.CacheDatabase.Package._Packages == null)
            {
                DiversityCollection.CacheDatabase.Package._Packages = new Dictionary<Pack, string>();
                DiversityCollection.CacheDatabase.Package._Packages.Add(Pack.ABCD, "Views on the data according to the ABCD schema (see http://wiki.tdwg.org/ABCD)");
                DiversityCollection.CacheDatabase.Package._Packages.Add(Pack.FloraRaster, "Darstellung floristischer Daten, die als Koordinaten- oder Rasterangaben vorliegen können, in einem TK-, Quadranten- oder feinerem Raster mit Angaben zum floristischen Status und zum Erhebungszeitraum.");
                DiversityCollection.CacheDatabase.Package._Packages.Add(Pack.Observation, "Views for an overview of observation data");
                DiversityCollection.CacheDatabase.Package._Packages.Add(Pack.LIDO, "Views on the data according to the LIDO schema (see http://www.lido-schema.org/schema/v1.0/lido-v1.0-schema-listing.html)");
            }
            return DiversityCollection.CacheDatabase.Package._Packages;
        }

        public static bool IsUsingSchemaPublic(Pack PackageType)
        {
            switch (PackageType)
            {
                case Pack.LIDO:
                case Pack.ABCD:
                    return true;
                default:
                    return false;
            }
        }

        public static System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> TransferSteps(Pack PackageType, string PostGresSchema)
        {
            System.Collections.Generic.List<DiversityCollection.CacheDatabase.TransferStep> Steps = new List<TransferStep>();
            switch (PackageType)
            {
                case Pack.ABCD:
                    TransferStep TC = new TransferStep("Citation", PostGresSchema, "abcd___ProjectCitation", "ABCD__ProjectCitation", DiversityCollection.Resource.References);
                    Steps.Add(TC);
                    TransferStep TM = new TransferStep("Multimedia", PostGresSchema, "abcd__multimediaobject", "ABCD__MultiMediaObject", DiversityCollection.Resource.Icones);
                    Steps.Add(TM);
                    //TransferStep TM2 = new TransferStep("MultimediaNonPart", PostGresSchema, "abcd__2_multimediaobject_nonpart", "ABCD__2_MultiMediaObject_NonPart", DiversityCollection.Resource.Icones);
                    //Steps.Add(TM2);
                    //TransferStep TM3 = new TransferStep("MultimediaPart", PostGresSchema, "abcd__3_multimediaobject_part", "ABCD__3_MultiMediaObject_Part", DiversityCollection.Resource.Icones);
                    //Steps.Add(TM3);
                    TransferStep TMF = new TransferStep("Measurement", PostGresSchema, "abcd__measurementorfact", "ABCD_MeasurementOrFact", DiversityCollection.Resource.Analysis);
                    Steps.Add(TMF);

                    TransferStep TUP = new TransferStep("UnitPart", PostGresSchema, "abcd__unitpart", "ABCD__UnitPart", DiversityCollection.Resource.UnitInPart);
                    Steps.Add(TUP);

                    TransferStep TORI = new TransferStep("Observation - remove indices", PostGresSchema, "abcd__UnitNoPart_RemoveIndices", "ABCD__UnitNoPart", DiversityCollection.Resource.Observation);
                    Steps.Add(TORI);
                    TransferStep TO = new TransferStep("Observation", PostGresSchema, "abcd__unitnopart", "ABCD__UnitNoPart", DiversityCollection.Resource.Observation);
                    Steps.Add(TO);
                    TransferStep TOM = new TransferStep("Observation - Metadata", PostGresSchema, "abcd__UnitNoPartMetadata", "ABCD__UnitNoPart", DiversityCollection.Resource.Observation);
                    Steps.Add(TOM);
                    TransferStep TOHT = new TransferStep("Observation - HigherTaxon", PostGresSchema, "abcd__UnitNoPartKingdom", "ABCD__UnitNoPart", DiversityCollection.Resource.Observation);
                    Steps.Add(TOHT);
                    TransferStep TOQ = new TransferStep("Observation - Qualifier", PostGresSchema, "abcd__UnitNoPartQualifier", "ABCD__UnitNoPart", DiversityCollection.Resource.Observation);
                    Steps.Add(TOQ);
                    TransferStep TOAI = new TransferStep("Observation - add indices", PostGresSchema, "abcd__UnitNoPart_AddingIndices", "ABCD__UnitNoPart", DiversityCollection.Resource.Observation);
                    Steps.Add(TOAI);
                    
                    TransferStep TU = new TransferStep("Unit: Part + Observation", PostGresSchema, "abcd__unit", "ABCD_Unit", DiversityCollection.Resource.Plant);
                    Steps.Add(TU);

                    TransferStep TUA = new TransferStep("Association", PostGresSchema, "abcd__unit_associations_unitassociation", "ABCD_Unit_Associations_UnitAssociation", DiversityCollection.Resource.Hierarchy);
                    Steps.Add(TUA);
                    TransferStep TG = new TransferStep("Gathering", PostGresSchema, "abcd__unit_gathering", "ABCD_Unit_Gathering", DiversityCollection.Resource.Event);
                    Steps.Add(TG);
                    TransferStep TA = new TransferStep("Agent", PostGresSchema, "abcd__unit_gathering_agents", "ABCD_Unit_Gathering_Agents", DiversityCollection.Resource.Agent);
                    Steps.Add(TA);
                    TransferStep TCS = new TransferStep("Chronostratigraphy", PostGresSchema, "abcd__unit_gathering_stratigraphy_chronostratigraphicterm", "ABCD_Unit_Gathering_Stratigraphy_ChronostratigraphicTerm", Specimen.ImageForCollectionEventProperty(false, "stratigraphy"));
                    Steps.Add(TCS);
                    TransferStep TLS = new TransferStep("Lithostratigraphy", PostGresSchema, "abcd__unit_gathering_stratigraphy_lithostratigraphicterm", "ABCD_Unit_Gathering_Stratigraphy_LithostratigraphicTerm", Specimen.ImageForCollectionEventProperty(false, "stratigraphy"));
                    Steps.Add(TLS);
                    TransferStep TS = new TransferStep("Specimen", PostGresSchema, "abcd__unit_specimenunit", "ABCD_Unit_SpecimenUnit", DiversityCollection.Resource.CollectionSpecimen);
                    Steps.Add(TS);
                    break;
                case Pack.FloraRaster:
                    // Markus 16.8.2021 - wird von Stefan direkt erzeugt
                    //TransferStep TAVG = new TransferStep("Position", PostGresSchema, "floraraster__CacheCollectionEventLocalisationAVGPos", "FloraRaster_CacheCollectionEventLocalisationAVGPos", DiversityCollection.Resource.Localisation);
                    //Steps.Add(TAVG);
                    TransferStep TR = new TransferStep("Raster", PostGresSchema, "floraraster__KartenRasterPunkte_tbl", "FloraRaster_KartenRasterPunkte_tbl", DiversityCollection.Resource.MTB);
                    Steps.Add(TR);
                    TransferStep TSB = new TransferStep("Raster", PostGresSchema, "floraraster__sippen_basis", "FloraRaster_sippen_basis", DiversityCollection.Resource.Plant);
                    Steps.Add(TSB);
                    break;
            }
            return Steps;
        }

        #region Versions of the packages

        public static int Version(Pack Pack)
        {
            switch (Pack)
            {
                case Package.Pack.ABCD:
                    return 11; // ToDo: Bei Umstellung auf Version 10 Einbau der Anpassungen in BioCASE beachten
                case Package.Pack.FloraRaster:
                    return 2;
                case Package.Pack.Observation:
                    return 2;
                case Package.Pack.LIDO:
                    return 2;
                default:
                    return 1;
            }
        }
        
        #endregion 
  
        #endregion

     
    }
}
