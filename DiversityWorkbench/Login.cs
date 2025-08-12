using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    /// <summary>
    /// Informations about the Modules - Databases - Roles - Projects
    /// </summary>
    public class Login
    {
        #region Parameter

        private string _LoginName;
        private string _DisplayText;
        private string _LinkToDiversityAgents;
        
        #endregion

        #region Construction

        public Login(string LoginName)
        {
            this._LoginName = LoginName;
        }
        
        #endregion

        #region Public functions

        #region Names and URI

        public string LoginName() { return this._LoginName; }

        public void SetLinkToDiversityAgents(string DisplayText, string URI)
        {
            this._DisplayText = DisplayText;
            this._LinkToDiversityAgents = URI;
        }

        public string DisplayText() { return this._DisplayText; }
        public string LinkToDiversityAgent() { return this._LinkToDiversityAgents; }
        
        public bool SetLinkToDiversityAgents(DiversityWorkbench.ServerConnection SC)
        {
            bool OK = false;
            if (this.ProjectsAndUserProxyDoExist(SC) 
                && this._DisplayText != null && this._DisplayText.Length > 0 
                && this._LinkToDiversityAgents != null && this._LinkToDiversityAgents.Length > 0)
            {
                string SQL = "use " + SC.DatabaseName + "; UPDATE U SET CombinedNameCache = '" + this.DisplayText() + "', AgentURI = '" + this.LinkToDiversityAgent() +
                    "' from UserProxy U WHERE U.LoginName = '" + this.LoginName() +
                    "' AND (U.CombinedNameCache IS NULL OR RTRIM(U.CombinedNameCache) = '' OR U.CombinedNameCache = '" + this.LoginName() + 
                    "') AND (U.AgentURI IS NULL OR RTRIM(U.AgentURI) = '') ";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    OK = true;
            }
            return OK;
        }

        #endregion

        #region Database

        public bool DatabaseUserCreate(DiversityWorkbench.ServerConnection SC)
        {
            string SQL = "";
            SQL = "use " + SC.DatabaseName + " IF  EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + this._LoginName + "') " +
                "DROP USER [" + this._LoginName + "] " +
                "IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + this._LoginName + "') " +
                "CREATE USER [" + this._LoginName + "] FOR LOGIN [" + this._LoginName + "] WITH DEFAULT_SCHEMA=[dbo];";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                return true;
            return false;
        }

        public bool LoginHasAccessToDatabase(DiversityWorkbench.ServerConnection SC)
        {
            bool LoginHasAccess = false;
            try
            {
                if (SC != null)
                {
                    string HasAccess = "0";
                    string SQL = "use " + SC.DatabaseName + "; " +
                        "SELECT count(*) " +
                        "FROM sysusers u " +
                        "WHERE u.name = '" + this._LoginName + "'";
                    Microsoft.Data.SqlClient.SqlConnection Con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, Con);
                    try
                    {
                        Con.Open();
                        int i;
                        HasAccess = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        if (HasAccess == string.Empty)
                            LoginHasAccess = false;
                        else if (HasAccess != "0")
                        {
                            SQL = "use " + SC.DatabaseName + "; " +
                                "SELECT u.hasdbaccess " +
                                "FROM sysusers u " +
                                "WHERE u.name = '" + this._LoginName + "'";
                            C.CommandText = SQL;
                            HasAccess = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        }
                    }
                    catch (System.Exception ex) { }
                    finally
                    {
                        Con.Close();
                        Con.Dispose();
                    }
                    if (HasAccess == string.Empty || HasAccess == "0")
                    {
                        LoginHasAccess = false;
                    }
                    else
                    {
                        LoginHasAccess = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                LoginHasAccess = false;
            }
            return LoginHasAccess;
        }

        public bool LoginExistsInDatabase(DiversityWorkbench.ServerConnection SC)
        {
            string SQL = "";
            SQL = "use " + SC.DatabaseName + "; select case when (SELECT count(*) FROM sys.database_principals WHERE name = N'" 
                + this._LoginName + "') > 0 then 1 else 0 end  " ;
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result == "1")
                return true;
            else
                return false;
        }

        public bool CreateDatabaseUser(DiversityWorkbench.ServerConnection SC)
        {
            string SQL = "";
            SQL = "use " + SC.DatabaseName + " IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + this._LoginName + "') " +
                "DROP USER [" + this._LoginName + "] " +
                "IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + this._LoginName + "') " +
                "CREATE USER [" + this._LoginName + "] FOR LOGIN [" + this._LoginName + "] WITH DEFAULT_SCHEMA=[dbo];";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                return true;
            return false;
        }

        public bool DeleteDatabaseUser(DiversityWorkbench.ServerConnection SC)
        {
            string SQL = "";
            SQL = "use " + SC.DatabaseName + "; " +
                "DELETE FROM ProjectUser WHERE LoginName = '" + this._LoginName + "'; ";
            bool OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            SQL = "use " + SC.DatabaseName + "; " +
                "DELETE FROM UserProxy WHERE LoginName = '" + this._LoginName + "'; ";
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            SQL = "use " + SC.DatabaseName + "; " +
                "IF EXISTS (SELECT * FROM sys.database_principals WHERE name = N'" + this._LoginName + "') " +
                "DROP USER [" + this._LoginName + "]; ";
            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                return true;
            return false;
        }

        public bool CopyDatabaseSettings(DiversityWorkbench.ServerConnection SC, DiversityWorkbench.Login TargetLogin)
        {
            bool OK = true;
            if (!this.LoginExistsInDatabase(SC))
                TargetLogin.DeleteDatabaseUser(SC);
            else
            {
                TargetLogin.CreateDatabaseUser(SC);
                this.CopyUserRoles(SC, TargetLogin.LoginName());
                if (this.ProjectsAndUserProxyDoExist(SC))
                {
                    this.CopyProjects(SC, TargetLogin.LoginName());
                    TargetLogin.SetLinkToDiversityAgents(SC);
                }
            }
            return OK;
        }
        
        #endregion


        #region Roles

        public void DatabaseRoleAdd(DiversityWorkbench.ServerConnection SC, string Role)
        {
            string SQL = "use " + SC.DatabaseName + "; " +
               "EXEC sp_addrolemember N'" + Role + "', N'" + this._LoginName + "'";
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                c.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex) { }
        }

        public void DatabaseRoleRemove(DiversityWorkbench.ServerConnection SC, string Role)
        {
            string SQL = "use " + SC.DatabaseName + "; " +
               "EXEC sp_droprolemember N'" + Role + "', N'" + this._LoginName + "'";
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                c.ExecuteNonQuery();
                con.Close();
            }
            catch (System.Exception ex) { }
        }

        public void DatabaseRolesRemoveAll(DiversityWorkbench.ServerConnection SC)
        {
            foreach (System.Data.DataRow R in this.UserRoles(SC).Rows)
            {
                this.DatabaseRoleRemove(SC, R[0].ToString());
            }
        }

        public void DatabaseRolesCopyToLogin(DiversityWorkbench.ServerConnection SC, string TargetLogin)
        {
            DiversityWorkbench.Login Target = new Login(TargetLogin);
            foreach (System.Data.DataRow R in this.UserRoles(SC).Rows)
            {
                Target.DatabaseRoleAdd(SC, R[0].ToString());
            }
        }

        public System.Data.DataTable UserRoles(ServerConnection SC)
        {
            System.Data.DataTable dtUserRoles = new System.Data.DataTable();
            string SQL = "use " + SC.DatabaseName + "; " +
                "SELECT sysusers_1.name " +
                "FROM sysmembers INNER JOIN " +
                "sysusers ON sysmembers.memberuid = sysusers.uid INNER JOIN " +
                "sysusers sysusers_1 ON sysmembers.groupuid = sysusers_1.uid " +
                "WHERE (replace(sysusers.name, '\', '\\') = N'" + this._LoginName + "') " +
                "ORDER BY sysusers_1.name";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            a.Fill(dtUserRoles);
            return dtUserRoles;
        }

        public void CopyUserRoles(DiversityWorkbench.ServerConnection SC, string TargetLogin)
        {
            // removing roles of Target
            DiversityWorkbench.Login T = new Login(TargetLogin);
            T.DatabaseRolesRemoveAll(SC);

            // Add roles of current login
            this.DatabaseRolesCopyToLogin(SC, TargetLogin);
        }
        
        #endregion

        #region Projects

        public bool ProjectsAndUserProxyDoExist(DiversityWorkbench.ServerConnection SC)
        {
            bool ProjectsDoExist = false;
            try
            {
                int i;
                //Check if the table UserProxy and the column AgentURI exists
                string Message = "";
                string SQL = "use " + SC.DatabaseName + "; select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C where C.TABLE_NAME = 'UserProxy' and C.COLUMN_NAME = 'AgentURI'";
                if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message), out i) && i > 0)
                {
                    SQL = "use " + SC.DatabaseName + "; " +
                    "SELECT COUNT(*) AS Anzahl " +
                    "FROM UserProxy AS U INNER JOIN " +
                    "ProjectUser AS PU ON U.LoginName = PU.LoginName RIGHT OUTER JOIN " +
                    "ProjectProxy AS P ON PU.ProjectID = P.ProjectID";
                    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message), out i))
                    {
                        ProjectsDoExist = true;
                    }
                    else
                        ProjectsDoExist = false;
                }
                else
                    ProjectsDoExist = false;
            }
            catch (System.Exception ex)
            {
                ProjectsDoExist = false;
            }

            return ProjectsDoExist;
        }

        public bool ProjectProvideReadOnly(DiversityWorkbench.ServerConnection SC)
        {
            bool ProjectsProvideReadOnly = false;
            string SQL = "use " + SC.DatabaseName + "; SELECT COUNT(*) " +
                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                "WHERE (TABLE_NAME = 'ProjectUser') AND (COLUMN_NAME = 'ReadOnly')";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result == "1")
            {
                ProjectsProvideReadOnly = true;
            }
            return ProjectsProvideReadOnly;
        }

        public System.Data.DataTable AccessibleProjects(DiversityWorkbench.ServerConnection SC)
        {
            string SQL = "use " + SC.DatabaseName + "; " +
                "SELECT ProjectProxy.ProjectID, ProjectProxy.Project " +
                "FROM ProjectUser INNER JOIN " +
                "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID " +
                "WHERE (replace(ProjectUser.LoginName, '\', '\\') = N'" + this._LoginName + "') ";
            if (this.ProjectProvideReadOnly(SC))
                SQL += " AND ProjectUser.ReadOnly = 0 ";
            SQL += " ORDER BY ProjectProxy.Project";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dtProjectsAccess = new System.Data.DataTable();
            a.Fill(dtProjectsAccess);
            return dtProjectsAccess;
        }

        public System.Data.DataTable ReadOnlyProjects(DiversityWorkbench.ServerConnection SC)
        {
            System.Data.DataTable dtProjectsReadOnly = new System.Data.DataTable();
            if (this.ProjectProvideReadOnly(SC))
            {
                string SQL = "use " + SC.DatabaseName + "; " +
                    "SELECT ProjectProxy.ProjectID, ProjectProxy.Project " +
                    "FROM ProjectUser INNER JOIN " +
                    "ProjectProxy ON ProjectUser.ProjectID = ProjectProxy.ProjectID " +
                    "WHERE (replace(ProjectUser.LoginName, '\', '\\') = N'" + this._LoginName + "') " +
                    " AND ProjectUser.ReadOnly = 1 " +
                    " ORDER BY ProjectProxy.Project";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                a.Fill(dtProjectsReadOnly);
            }
            return dtProjectsReadOnly;
        }

        public bool ProjectHasListRestriction(DiversityWorkbench.ServerConnection SC)
        {
            bool ProjectsRestrictedToList = false;
            string SQL = "use " + SC.DatabaseName + "; SELECT COUNT(*) " +
                "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                "WHERE (TABLE_NAME = 'TaxonNameListUser') AND (COLUMN_NAME = 'LoginName')";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result == "1")
            {
                ProjectsRestrictedToList = true;
            }
            return ProjectsRestrictedToList;
        }

        public System.Data.DataTable ProjectListRestrictions(DiversityWorkbench.ServerConnection SC)
        {
            System.Data.DataTable dtLists = new System.Data.DataTable();
            if (this.ProjectHasListRestriction(SC))
            {
                string SQL = "use " + SC.DatabaseName + "; " +
                    " SELECT case when  P.DisplayText is null or P.DisplayText = '' then P.Project else P.DisplayText end AS List " +
                    " FROM TaxonNameListUser AS U INNER JOIN " +
                    " TaxonNameListProjectProxy AS P ON U.ProjectID = P.ProjectID " +
                    " WHERE (replace(U.LoginName, '\', '\\') = N'" + this._LoginName + "') " +
                    " ORDER BY List";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                try
                {
                    a.Fill(dtLists);
                }
                catch (System.Exception ex) { }
            }
            return dtLists;
        }

        public void CopyProjects(DiversityWorkbench.ServerConnection SC, string TargetLogin)
        {
            try
            {

                // Remove previous entries
                string SQL = "DELETE P FROM ProjectUser P WHERE P.LoginName = '" + TargetLogin + "'";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionString);
                con.Open();
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                C.ExecuteNonQuery();

                // Insert User in UserProxy if missing
                SQL = "use " + SC.DatabaseName + "; " +
                    "IF (SELECT COUNT(*) FROM UserProxy WHERE LoginName = '" + TargetLogin + "') = 0 " +
                    "BEGIN INSERT INTO UserProxy " +
                    "(LoginName, CombinedNameCache) " +
                    "VALUES     ('" + TargetLogin + "', '" + TargetLogin + "') END";
                C.CommandText = SQL;
                C.ExecuteNonQuery();

                // Enter entries according to current login
                SQL = "SELECT COLUMN_NAME FROM  INFORMATION_SCHEMA.COLUMNS AS C WHERE (TABLE_NAME = 'ProjectUser') AND C.COLUMN_NAME NOT LIKE 'Log%When' AND C.COLUMN_NAME NOT LIKE 'Log%By' AND C.COLUMN_NAME <> 'RowGUID'";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
                ad.Fill(dt);
                SQL = "INSERT INTO ProjectUser (";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0) SQL += ", ";
                    SQL += dt.Rows[i][0].ToString();
                }
                SQL += ") SELECT ";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i > 0) SQL += ", ";
                    if (dt.Rows[i][0].ToString() == "LoginName")
                        SQL += "'" + TargetLogin + "'";
                    else
                        SQL += dt.Rows[i][0].ToString();
                }
                SQL += " FROM ProjectUser P WHERE P.LoginName = '" + this._LoginName + "'";
                C.CommandText = SQL;
                C.ExecuteNonQuery();
                con.Close();
                con.Dispose();

            }
            catch (Exception ex)
            {
            }
        }
        
        #endregion  

        #region Settings and Agent info

        public System.Collections.Generic.Dictionary<string, string> AgentInfos(DiversityWorkbench.ServerConnection SC)
        {
            System.Collections.Generic.Dictionary<string, string> Dict = new Dictionary<string, string>();
            if (this.ProjectsAndUserProxyDoExist(SC))
            {
                string SQL = "use " + SC.DatabaseName + "; " +
                    "SELECT U.[AgentURI] " +
                    "FROM UserProxy U  " +
                    "WHERE U.LoginName = N'" + this._LoginName + "' ";
                string Message = "";
                string Uri = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message);
                if (Uri.Length > 0)
                {
                    DiversityWorkbench.Agent A = new Agent(SC);
                    Dict = A.UnitValues(Uri);
                }
            }
            return Dict;
        }

        public bool UserSettingsAvailable(DiversityWorkbench.ServerConnection SC)
        {
            bool SettingsAvailable = false;
            try
            {
                string SQL = "use " + SC.DatabaseName + "; " +
                    "select COUNT(*) from INFORMATION_SCHEMA.COLUMNS C " +
                    "where C.TABLE_NAME = 'UserProxy' " +
                    "and C.COLUMN_NAME = 'Settings' " +
                    "and C.DATA_TYPE = 'XML'";
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL) == "1")
                    SettingsAvailable = true;
            }
            catch (System.Exception ex) { }
            return SettingsAvailable;
        }

        public void CopySettings(DiversityWorkbench.ServerConnection SC, string TargetLogin)
        {
        }
        
        #endregion 
     
        #endregion
    }
}
