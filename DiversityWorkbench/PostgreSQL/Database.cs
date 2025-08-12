using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Npgsql;
using System.Data;

namespace DiversityWorkbench.PostgreSQL
{
    public class Database
    {
        private string _Name;

        public string Name
        {
            get { return _Name; }
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Schema> _Schemas;

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Schema> Schemas
        {
            get
            {
                if (this._Schemas == null || (this._Schemas != null && this._Schemas.Count == 1))
                {
                    this._Schemas = new Dictionary<string, Schema>();
                    string SQL = "select schema_name from information_schema.schemata";
                    System.Data.DataTable dt = new DataTable();
                    Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this.Name));// DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    ad.Fill(dt);
                    foreach(System.Data.DataRow R in dt.Rows)
                    {
                        string Schema = R[0].ToString();
                        if (Schema == "public" || Schema.StartsWith("Project"))
                        {
                            DiversityWorkbench.PostgreSQL.Schema S = new Schema(Schema, this);
                            this._Schemas.Add(S.Name, S);
                        }
                        //if (Schema == "pg_toast" || Schema == "pg_temp_1" || Schema == "pg_toast_temp_1" || Schema == "pg_catalog" || Schema == "information_schema")
                        //    continue;
                    }
                }
                return _Schemas;
            }
        }

        public string Version()
        {
            string SQL = "select \"public\".Version();";
            string Version = "";
            try
            {
                Npgsql.NpgsqlConnection con = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.ConnectionString(this.Name));
                NpgsqlCommand C = new NpgsqlCommand(SQL, con);
                con.Open();
                Version = C.ExecuteScalar()?.ToString() ?? string.Empty;
                C.Dispose();
                con.Close();
                con.Dispose();

            }
            catch (Exception ex)
            {
            } 
            return Version;
        }

        //public DiversityWorkbench.PostgreSQL.Schema GetProject(int ProjectID)
        //{
        //    DiversityWorkbench.PostgreSQL.Schema S = new Schema("", DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase());
        //    foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Schema> KV in this.Schemas)
        //    {
        //        if (KV.Value.ProjectID == ProjectID)
        //            return KV.Value;
        //    }
        //    return S;
        //}

        //private System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Project> _Projects;

        //public System.Collections.Generic.Dictionary<string, DiversityWorkbench.PostgreSQL.Project> Projects
        //{
        //    get
        //    {
        //        if (this._Projects == null)
        //        {
        //            this._Projects = new Dictionary<string, Project>();
        //            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Schema> KV in this.Schemas)
        //            {
        //                string SQL = "select \"" + KV.Key + "\".ProjectID()";
        //                int ProjectID;
        //                if (int.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out ProjectID))
        //                {
        //                    SQL = "select \"" + KV.Key + "\".version()";
        //                    string Version = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
        //                    if (Version.Length > 0)
        //                    {
        //                        DiversityWorkbench.PostgreSQL.Project P = new Project();
        //                        _Projects.Add(KV.Key, P);
        //                    }
        //                }
        //            }
        //        }
        //        return _Projects;
        //    }
        //}

        //public void RestrictSchemataToProjects()
        //{
        //    System.Collections.Generic.List<string> NoProjects = new List<string>();
        //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Schema> KV in this.Schemas)
        //    {
        //        NoProjects.Add(KV.Key);
        //    }
        //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Schema> KV in this.Schemas)
        //    {
        //        string SQL = "select \"" + KV.Key + "\".ProjectID()";
        //        int ProjectID;
        //        if (int.TryParse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL), out ProjectID))
        //        {
        //            SQL = "select \"" + KV.Key + "\".version()";
        //            string Version = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
        //            if (Version.Length > 0)
        //                NoProjects.Remove(KV.Key);
        //        }
        //    }
        //    foreach (string S in NoProjects)
        //        this.Schemas.Remove(S);
        //}

        /// <summary>
        /// Creates a schema used as a project containing a function for the project ID and a function the version
        /// </summary>
        /// <param name="ProjectName">Name of the schema resp. the project</param>
        /// <param name="ProjectID">ID of the project</param>
        public bool CreateSchema(string ProjectName, int ProjectID)
        {
            bool OK = true;
            try
            {
                string SQL = "CREATE SCHEMA  \"" + ProjectName + "\"" +
                    "AUTHORIZATION \"CacheAdmin\";";
                DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);

                SQL = "ALTER DEFAULT PRIVILEGES IN SCHEMA \"" + ProjectName + "\"" +
                    "GRANT EXECUTE ON FUNCTIONS " +
                    "TO \"CacheUser\";";
                DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);

                SQL = "ALTER DEFAULT PRIVILEGES IN SCHEMA \"" + ProjectName + "\"" +
                    "GRANT SELECT ON TABLES " +
                    "TO \"CacheUser\";";
                DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);

                SQL = "CREATE OR REPLACE FUNCTION \"" + ProjectName + "\".version() " +
                    "RETURNS integer AS " +
                    "$BODY$ " +
                    "declare " +
                    "v integer; " +
                    "BEGIN " +
                    "SELECT 0 into v; " +
                    "RETURN v; " +
                    "END; " +
                    "$BODY$ " +
                    "LANGUAGE plpgsql STABLE " +
                    "COST 100; " +
                    "ALTER FUNCTION \"" + ProjectName + "\".version() " +
                    "OWNER TO \"CacheAdmin\"; " +
                    "GRANT EXECUTE ON FUNCTION \"" + ProjectName + "\".version() TO \"CacheAdmin\"; " +
                    "GRANT EXECUTE ON FUNCTION \"" + ProjectName + "\".version() TO \"CacheUser\"";
                DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);

                SQL = "CREATE OR REPLACE FUNCTION \"" + ProjectName + "\".projectid() " +
                    "RETURNS integer AS " +
                    "$BODY$ " +
                    "declare " +
                    "v integer; " +
                    "BEGIN " +
                    "SELECT " + ProjectID.ToString() + " into v; " +
                    "RETURN v; " +
                    "END; " +
                    "$BODY$ " +
                    "LANGUAGE plpgsql STABLE " +
                    "COST 100; " +
                    "ALTER FUNCTION \"" + ProjectName + "\".projectid() " +
                    "OWNER TO \"CacheAdmin\"; " +
                    "GRANT EXECUTE ON FUNCTION \"" + ProjectName + "\".projectid() TO \"CacheAdmin\"; " +
                    "GRANT EXECUTE ON FUNCTION \"" + ProjectName + "\".projectid() TO \"CacheUser\"";
                DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);
                this._Schemas = null;
            }
            catch(System.Exception ex)
            { OK = false; }
            return OK;
        }
        
        public bool DeleteSchema(string Name)
        {
            bool OK = true;
            string SQL = "DROP SCHEMA \"" + Name + "\" CASCADE;";
            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL)) // .Postgres.PostgresExecuteSqlNonQuery(SQL))
            {
                OK = true;
                this._Schemas = null;
            }
            else OK = false;
            return OK;
        }

        public bool ClearSchema(string Name)
        {
            bool OK = true;
            string Message = "";
            string SQL = "SELECT table_name FROM information_schema.tables WHERE table_schema='" + Name + "' ORDER BY table_name;";
            System.Data.DataTable dtClear = new DataTable();
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtClear, ref Message);
            if (Message.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Schema " + Name + " could not be cleared: " + Message);
                return false;
            }
            foreach(System.Data.DataRow R in dtClear.Rows)
            {
                SQL = "DELETE FROM \"" + Name + "\".\"" + R[0].ToString() + "\"";
                Message = "";
                if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                {
                    System.Windows.Forms.MessageBox.Show("Table or view " + R[0].ToString() + " could not be cleared: " + Message);
                    //return false;
                }
            }
            return OK;
        }

        //private DiversityWorkbench.PostgreSQL.Role _Role;

        //public DiversityWorkbench.PostgreSQL.Role Role
        //{
        //    get { return _Role; }
        //}

        public Database(string Name)
        {
            this._Name = Name;
        }

        #region Copy of database

        public bool CreateCopy(string NameOfCopy, string DatabaseOwner, bool IncludeData, string PostgresApplicationgDirectory, ref string Message)
        {
            if (IncludeData)
                return CopyDatabaseIncludingData(NameOfCopy, DatabaseOwner, ref Message);
            else
                return CopyDatabaseStructure(NameOfCopy, DatabaseOwner, PostgresApplicationgDirectory, ref Message);
        }

        private bool CopyDatabaseStructure(string NameOfCopy, string DatabaseOwner, string PostgresApplicationgDirectory, ref string Message)
        {
            bool OK = false;
            try
            {
                // Create the new database
                string SQL = "CREATE DATABASE \"" + NameOfCopy + "\" " +
                    "WITH ENCODING='UTF8' TABLESPACE = pg_default CONNECTION LIMIT=-1;";
                if (PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                {
                    // Copy the structure into the new database
                    string DumpFile = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.Folder(WorkbenchResources.WorkbenchDirectory.FolderType.Backup) + NameOfCopy + ".dmp";
                    string CommandPgDump = "pg_dump -s" +
                        " -h " + PostgreSQL.Connection.CurrentServer().Name +
                        " -p " + PostgreSQL.Connection.CurrentServer().Port.ToString() +
                        " -U postgres" +
                        " -d \"" + PostgreSQL.Connection.CurrentDatabase().Name + "\"" +
                        " -f \"" + DumpFile + "\"";
                    //string Command2 = "pg_dump -s -v" +
                    //    " -h " + PostgreSQL.Connection.CurrentServer().Name +
                    //    " -p " + PostgreSQL.Connection.CurrentServer().Port.ToString() +
                    //    " -U postgres" +
                    //    " -d " + PostgreSQL.Connection.CurrentDatabase().Name +
                    //    " | psql -h " + PostgreSQL.Connection.CurrentServer().Name +
                    //    " -p " + PostgreSQL.Connection.CurrentServer().Port.ToString() +
                    //    " -U postgres " + NameOfCopy;
                    //string Command3 = "pg_dump -s" +
                    //    " -h " + PostgreSQL.Connection.CurrentServer().Name +
                    //    " -p " + PostgreSQL.Connection.CurrentServer().Port.ToString() +
                    //    " -U postgres" +
                    //    " -d \"" + PostgreSQL.Connection.CurrentDatabase().Name + "\"" +
                    //    " | psql -h " + PostgreSQL.Connection.CurrentServer().Name +
                    //    " -p " + PostgreSQL.Connection.CurrentServer().Port.ToString() +
                    //    " -U postgres \"" + NameOfCopy + "\"";
                    //string Command4 = "psql -h " + PostgreSQL.Connection.CurrentServer().Name +
                    //    " -p " + PostgreSQL.Connection.CurrentServer().Port.ToString() +
                    //    " -U postgres \"" + NameOfCopy + "\"";
                    string CommandPsql = "psql " +
                        " -h " + PostgreSQL.Connection.CurrentServer().Name +
                        " -p " + PostgreSQL.Connection.CurrentServer().Port.ToString() +
                        " -U postgres" +
                        " \"" + NameOfCopy + "\" <  \"" + DumpFile + "\"";
                    try
                    {
                        //System.Diagnostics.Process pg_dump = new System.Diagnostics.Process();
                        //pg_dump.StartInfo.FileName = "pg_dump.exe";
                        //pg_dump.StartInfo.WorkingDirectory = @"C:\Program Files\PostgreSQL\9.4\bin";
                        //pg_dump.StartInfo.Arguments = "pg_dump /c  -s -v" +
                        //" -h " + PostgreSQL.Connection.CurrentServer().Name +
                        //" -p " + PostgreSQL.Connection.CurrentServer().Port.ToString() +
                        //" -U postgres" +
                        //" -d database \"" + PostgreSQL.Connection.CurrentDatabase().Name + "\"" +
                        //" -f \"" + ...Windows.Forms.Application.StartupPath + "\\" + NameOfCopy + ".dmp\""; 
                        //pg_dump.Start();

                        System.Diagnostics.Process pg_dump = new System.Diagnostics.Process();
                        pg_dump.StartInfo.FileName = "cmd.exe";

                        //cmd.StartInfo.RedirectStandardInput = true;
                        //cmd.StartInfo.RedirectStandardOutput = true;
                        //cmd.StartInfo.CreateNoWindow = true;
                        //cmd.StartInfo.UseShellExecute = false;

                        pg_dump.StartInfo.WorkingDirectory = PostgresApplicationgDirectory;
                        pg_dump.StartInfo.Arguments = "/c " + CommandPgDump;
                        pg_dump.Start();

                        //cmd.StandardInput.WriteLine("echo Oscar");
                        //cmd.StandardInput.Flush();
                        //cmd.StandardInput.Close();

                        pg_dump.WaitForExit();

                        System.Diagnostics.Process psql = new System.Diagnostics.Process();
                        psql.StartInfo.FileName = "cmd.exe";
                        psql.StartInfo.WorkingDirectory = PostgresApplicationgDirectory;
                        psql.StartInfo.Arguments = "/c " + CommandPsql;
                        psql.Start();

                        //cmd.StandardInput.WriteLine("echo Oscar");
                        //cmd.StandardInput.Flush();
                        //cmd.StandardInput.Close();

                        psql.WaitForExit();


                        //Console.WriteLine(cmd.StandardOutput.ReadToEnd());
                        //System.Diagnostics.Process.Start("cmd.exe", Command);
                        System.IO.FileInfo DF = new System.IO.FileInfo(DumpFile);
                        DF.Delete();
                        return true;
                    }
                    catch (System.Exception ex)
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private bool CopyDatabaseIncludingData(string NameOfCopy, string DatabaseOwner, ref string Message)
        {
            // KILL ALL EXISTING CONNECTIONS
            string SQL = "SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity " +
                "WHERE pg_stat_activity.datname = '" + this.Name + "' AND pid <> pg_backend_pid(); ";
            string ConnectionString = DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString();
            if (ConnectionString.Length > 0)// .Postgres.PostgresConnection() != null)
            {
                try
                {
                    NpgsqlConnection con = new NpgsqlConnection(ConnectionString);
                    Npgsql.NpgsqlCommand C = new NpgsqlCommand(SQL, con);// .Postgres.PostgresConnection());
                    C.CommandTimeout = 0;
                    if (con.State == ConnectionState.Closed)
                        con.Open();
                    C.ExecuteNonQuery();
                    C.CommandText = "CREATE DATABASE \"" + NameOfCopy + "\" WITH TEMPLATE \"" + this.Name + "\" OWNER \"CacheAdmin\"; ";
                    //if (DatabaseOwner == "postgres")
                    //    C.CommandText += DatabaseOwner + " ;";
                    //else
                    //    C.CommandText += "\"" + DatabaseOwner + "\";";
                    C.ExecuteNonQuery();
                    C.Dispose();
                    con.Close();
                    con.Dispose();
                }
                catch (System.Exception ex)
                {
                    Message = ex.Message;
                    return false;
                }
            }


            //if (PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
            //{
            //    //DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name);
            //    // Create the copy
            //    SQL = "CREATE DATABASE \"" + NameOfCopy + "\" WITH TEMPLATE \"" + this.Name + "\" OWNER " + DatabaseOwner + " ;";
            //    if (PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
            //        return true;
            //}
            return true;
        }
        
        #endregion

    }
}
