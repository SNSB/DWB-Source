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

                // Add USAGE grant for CacheUser (required for PostgreSQL 15+)
                SQL = "GRANT USAGE ON SCHEMA \"" + ProjectName + "\" TO \"CacheUser\";";
                DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);
                // Add USAGE grant for CachePublicUser if it exists
                SQL = "DO $$ BEGIN " +
                    "IF EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'CachePublicUser') THEN " +
                    "EXECUTE 'GRANT USAGE ON SCHEMA \"" + ProjectName + "\" TO \"CachePublicUser\"'; " +
                    "END IF; END $$;";
                DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);

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


        public Database(string Name)
        {
            this._Name = Name;
        }

        #region Create and Copy of database

        public bool CreateCopy(string NameOfCopy, string DatabaseOwner, bool IncludeData, ref string Message)
        {
            if (IncludeData)
                return CopyDatabaseIncludingData(NameOfCopy, DatabaseOwner, ref Message);
            else
                return CreateDatabaseFromTemplate(NameOfCopy, ref Message);  // Use template for empty copy
        }

        /// <summary>
        /// Create database from pre-configured template (replaces all inline role creation by using template in postgres db)
        /// </summary>
        public bool CreateDatabaseFromTemplate(string DatabaseName, ref string Message)
        {
            try
            {
                // Connect to dwb_maintenance_db to execute CREATE DATABASE
                string maintenanceConnectionString = DiversityWorkbench.PostgreSQL.Connection.GetMaintenanceDbConnectionString();
                string templateName = DiversityWorkbench.PostgreSQL.Settings.Default.TemplateDB; 
                using (var con = new NpgsqlConnection(maintenanceConnectionString))
                {
                    con.Open();

                    // Step 1: Validate the operation
                    using (var cmd = new NpgsqlCommand(
                        "SELECT public.validate_database_operation('CREATE', @dbname)", con))
                    {
                        cmd.Parameters.AddWithValue("@dbname", DatabaseName);
                        cmd.ExecuteScalar();
                    }

                    string SQL = $@"
                        CREATE DATABASE ""{DatabaseName}"" 
                        WITH TEMPLATE ""{templateName}"" 
                        OWNER ""CacheAdmin"" 
                        ENCODING 'UTF8' 
                        CONNECTION LIMIT=-1;";

                    using (var cmd = new NpgsqlCommand(SQL, con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (System.Exception ex)
            {
                Message = "Error creating database: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Copy database with content - terminates sessions via dwb_maintenance_db, then creates copy
        /// </summary>
        private bool CopyDatabaseIncludingData(string NameOfCopy, string DatabaseOwner, ref string Message)
        {
            try
            {
                string maintenanceConnectionString = DiversityWorkbench.PostgreSQL.Connection.GetMaintenanceDbConnectionString();

                using (var con = new NpgsqlConnection(maintenanceConnectionString))
                {
                    con.Open();

                    // Step 1: Validate
                    using (var cmd = new NpgsqlCommand(
                        "SELECT public.validate_database_operation('COPY', @source, @target)", con))
                    {
                        cmd.Parameters.AddWithValue("@source", this.Name);
                        cmd.Parameters.AddWithValue("@target", NameOfCopy);
                        cmd.ExecuteScalar();
                    }

                    using (var cmd = new NpgsqlCommand(
                        "SELECT public.terminate_database_sessions(@dbname)", con))
                    {
                        cmd.Parameters.AddWithValue("@dbname", this.Name);
                        cmd.ExecuteScalar();
                    }

                    // Small delay to allow connections to close
                    System.Threading.Thread.Sleep(500);

                    // Step 2: Create the copy using source as template
                    string SQL = $@"
                        CREATE DATABASE ""{NameOfCopy}"" 
                        WITH TEMPLATE ""{this.Name}"" 
                        OWNER ""CacheAdmin"";";

                    using (var cmd = new NpgsqlCommand(SQL, con))
                    {
                        cmd.CommandTimeout = 0; // No timeout for large databases
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Message = "Error: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Drop a database
        /// </summary>
        public bool DropDatabase(string databaseName, ref string message)
        {
            try
            {
                string maintenanceConnectionString = DiversityWorkbench.PostgreSQL.Connection.GetMaintenanceDbConnectionString();

                using (var con = new NpgsqlConnection(maintenanceConnectionString))
                {
                    con.Open();

                    // Validate
                    using (var cmd = new NpgsqlCommand(
                        "SELECT public.validate_database_operation('DROP', @dbname)", con))
                    {
                        cmd.Parameters.AddWithValue("@dbname", databaseName);
                        cmd.ExecuteScalar();
                    }

                    // Terminate sessions
                    using (var cmd = new NpgsqlCommand(
                        "SELECT public.terminate_database_sessions(@dbname)", con))
                    {
                        cmd.Parameters.AddWithValue("@dbname", databaseName);
                        cmd.ExecuteScalar();
                    }

                    System.Threading.Thread.Sleep(500);

                    // DROP DATABASE (DDL - direct)
                    using (var cmd = new NpgsqlCommand($@"DROP DATABASE ""{databaseName}""", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Rename a database using dwb_maintenance_db function
        /// </summary>
        public bool RenameDatabase(string oldName, string newName, ref string message)
        {
            try
            {
                string maintenanceConnectionString = DiversityWorkbench.PostgreSQL.Connection.GetMaintenanceDbConnectionString();

                using (var con = new NpgsqlConnection(maintenanceConnectionString))
                {
                    con.Open();

                    // Validate
                    using (var cmd = new NpgsqlCommand(
                        "SELECT public.validate_database_operation('RENAME', @oldname, @newname)", con))
                    {
                        cmd.Parameters.AddWithValue("@oldname", oldName);
                        cmd.Parameters.AddWithValue("@newname", newName);
                        cmd.ExecuteScalar();
                    }

                    // Terminate sessions
                    using (var cmd = new NpgsqlCommand(
                        "SELECT public.terminate_database_sessions(@dbname)", con))
                    {
                        cmd.Parameters.AddWithValue("@dbname", oldName);
                        cmd.ExecuteScalar();
                    }

                    System.Threading.Thread.Sleep(500);
                    using (var cmd = new NpgsqlCommand(
                        $@"ALTER DATABASE ""{oldName}"" RENAME TO ""{newName}""", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Replace a database with another by renaming
        /// ReplaceDatabase(database_1, database_1_prod)
        ///   -> database_1_prod wird zu database_1_prod_backup (falls existent)
        ///   -> database_1 wird zu database_1_prod
        /// </summary>
        public static bool ReplaceDatabase(string targetDatabase, string sourceDatabase, ref string message,
            bool dropBackup = true, bool saveCopyOfSource = true, string backupSuffix = "_OLD", string sourceCopySuffix = "_temp"
            )
        {
            message = null;
            try
            {
                string maintenanceConnectionString = DiversityWorkbench.PostgreSQL.Connection.GetMaintenanceDbConnectionString();
                string backupName = targetDatabase + backupSuffix;
                string sourceCopyName = sourceDatabase + sourceCopySuffix;

                // Step 1: Validate and prepare
                using (var con = new NpgsqlConnection(maintenanceConnectionString))
                {
                    con.Open();

                    // Check if source exists
                    using (var cmd = new NpgsqlCommand(
                        "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @source)", con))
                    {
                        cmd.Parameters.AddWithValue("@source", sourceDatabase);
                        if (!(bool)cmd.ExecuteScalar())
                        {
                            message = $"Source database '{sourceDatabase}' does not exist";
                            return false;
                        }
                    }
                    // Check if target exists
                    using (var cmd = new NpgsqlCommand(
                        "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @target)", con))
                    {
                        cmd.Parameters.AddWithValue("@target", targetDatabase);
                        if (!(bool)cmd.ExecuteScalar())
                        {
                            message = $"Target database '{targetDatabase}' does not exist";
                            return false;
                        }
                    }

                    // Check if backup name already exists
                    using (var cmd = new NpgsqlCommand(
                        "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @backup)", con))
                    {
                        cmd.Parameters.AddWithValue("@backup", backupName);
                        if ((bool)cmd.ExecuteScalar())
                        {
                            message = $"Backup database '{backupName}' already exists. Please remove it first.";
                            return false;
                        }
                    }
                }

                // Step 2: If saveCopyOfSource, create copy first
                if (saveCopyOfSource)
                {
                    using (var con = new NpgsqlConnection(maintenanceConnectionString))
                    {
                        con.Open();

                        // Check if copy already exists
                        using (var cmd = new NpgsqlCommand(
                            "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @copy)", con))
                        {
                            cmd.Parameters.AddWithValue("@copy", sourceCopyName);
                            if ((bool)cmd.ExecuteScalar())
                            {
                                message = $"Source copy '{sourceCopyName}' already exists.";
                                return false;
                            }
                        }

                        // Terminate sessions on source
                        using (var cmd = new NpgsqlCommand(
                            "SELECT public.terminate_database_sessions(@dbname)", con))
                        {
                            cmd.Parameters.AddWithValue("@dbname", sourceDatabase);
                            cmd.ExecuteScalar();
                        }

                        System.Threading.Thread.Sleep(500);

                        // Create copy
                        using (var cmd = new NpgsqlCommand(
                            $@"CREATE DATABASE ""{sourceCopyName}"" WITH TEMPLATE ""{sourceDatabase}"" OWNER ""CacheAdmin""", con))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // Step 3: Rename target to backup (if exists)
                using (var con = new NpgsqlConnection(maintenanceConnectionString))
                {
                    con.Open();

                    bool targetExists = false;
                    using (var cmd = new NpgsqlCommand(
                        "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @target)", con))
                    {
                        cmd.Parameters.AddWithValue("@target", targetDatabase);
                        targetExists = (bool)cmd.ExecuteScalar();
                    }

                    if (targetExists)
                    {
                        // Terminate sessions on target
                        using (var cmd = new NpgsqlCommand(
                            "SELECT public.terminate_database_sessions(@dbname)", con))
                        {
                            cmd.Parameters.AddWithValue("@dbname", targetDatabase);
                            cmd.ExecuteScalar();
                        }

                        System.Threading.Thread.Sleep(500);

                        // Rename target to backup
                        using (var cmd = new NpgsqlCommand(
                            $@"ALTER DATABASE ""{targetDatabase}"" RENAME TO ""{backupName}""", con))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                // Step 4: Rename source to target (MUST use new connection)
                using (var con = new NpgsqlConnection(maintenanceConnectionString))
                {
                    con.Open();

                    // Terminate sessions on source
                    using (var cmd = new NpgsqlCommand(
                        "SELECT public.terminate_database_sessions(@dbname)", con))
                    {
                        cmd.Parameters.AddWithValue("@dbname", sourceDatabase);
                        cmd.ExecuteScalar();
                    }

                    System.Threading.Thread.Sleep(500);

                    // Rename source to target
                    using (var cmd = new NpgsqlCommand(
                        $@"ALTER DATABASE ""{sourceDatabase}"" RENAME TO ""{targetDatabase}""", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                // Step 5: Optionally drop backup
                if (dropBackup)
                {
                    using (var con = new NpgsqlConnection(maintenanceConnectionString))
                    {
                        con.Open();

                        bool backupExists = false;
                        using (var cmd = new NpgsqlCommand(
                            "SELECT EXISTS (SELECT 1 FROM pg_database WHERE datname = @backup)", con))
                        {
                            cmd.Parameters.AddWithValue("@backup", backupName);
                            backupExists = (bool)cmd.ExecuteScalar();
                        }

                        if (backupExists)
                        {
                            using (var cmd = new NpgsqlCommand(
                                "SELECT public.terminate_database_sessions(@dbname)", con))
                            {
                                cmd.Parameters.AddWithValue("@dbname", backupName);
                                cmd.ExecuteScalar();
                            }

                            System.Threading.Thread.Sleep(500);

                            using (var cmd = new NpgsqlCommand(
                                $@"DROP DATABASE ""{backupName}""", con))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                // rename _temp zu newDatabase
                // Step 5: Rename temp to source (MUST use new connection)
                using (var con = new NpgsqlConnection(maintenanceConnectionString))
                {
                    con.Open();

                    // Rename source to target
                    using (var cmd = new NpgsqlCommand(
                        $@"ALTER DATABASE ""{sourceCopyName}"" RENAME TO ""{sourceDatabase}""", con))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        #endregion
    }
}
