using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.CacheDatabase.Diagnostics
{
    public class ABCD : Target
    {
        public ABCD(string Project, int ProjectID)
        {
            this._Project = Project;
            this._ProjectID = ProjectID;
        }

        public override System.Collections.Generic.List<Group> TestResults(DiversityWorkbench.Forms.FormStarting formStarting = null)
        {
#if DEBUG
            //DiversityWorkbench.Forms.FormStarting formStarting = new DiversityWorkbench.Forms.FormStarting(DiversityWorkbench.Forms.FormStarting.Unit.Collection, 9, "Check existance of schema");
            //formStarting.Show();
            //Application.DoEvents();
#endif

            this._DiagnosticGroups = null;


            // Postgres connection
            if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null &&
                DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name).Length > 0)
            {
                this._PostgresDatabase = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
            }
            else
                this._PostgresDatabase = "";

            if (formStarting != null) formStarting.ShowCurrentStep("Check if schema exits");
            // check if schema exits
            string sql = "SELECT COUNT(*) FROM Information_Schema.Schemata WHERE schema_name = 'Project_" + this._Project + "'";
            string result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(sql);
            if (result == "0")
            {
                Group P = new Group(this._Project);
                Result Missing = new Result(this._Project + " is not established");
                Missing.ToolTip = "If the project " + this._Project + " has not been established in the Postgres database";
                Missing.ForPostgres = true;
                Missing.OK = false;
                Missing.Description = "The project " + this._Project + " has not been established in the Postgres database";
                P.Content.Add(Missing);
                this.DiagnosticGroups().Add(P);
                return this.DiagnosticGroups();
            }

            if (formStarting != null) formStarting.ShowCurrentStep("check existance of any tables in the postgres database");
            // check existance of any tables in the postgres database
            sql = "SELECT COUNT(*) FROM Information_Schema.Tables WHERE table_schema = 'Project_" + this._Project + "'";
            result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(sql);
            if (result == "0")
            {
                Group P = new Group(this._Project);
                Result Missing = new Result(this._Project + " is missing tables");
                Missing.ToolTip = "If the project " + this._Project + " needs tables";
                Missing.ForPostgres = true;
                Missing.OK = false;
                Missing.Description = "The project " + this._Project + " has no tables and is missing the basic update";
                P.Content.Add(Missing);
                this.DiagnosticGroups().Add(P);
                return this.DiagnosticGroups();
            }

            if (formStarting != null) formStarting.ShowCurrentStep("check existance of any tables in the postgres database");
            // check existance of ABCD package
            sql = "SELECT COUNT(*) FROM \"Project_" + this._Project + "\".\"Package\" WHERE \"Package\" = 'ABCD';";
            result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(sql);
            if (result == "0")
            {
                Group P = new Group(this._Project);
                Result Missing = new Result("If package ABCD is present");
                Missing.ToolTip = "If the package " + this._Project + " is missing";
                Missing.ForPostgres = true;
                Missing.OK = false;
                Missing.Description = "The package ABCD is missing";
                P.Content.Add(Missing);
                this.DiagnosticGroups().Add(P);
                return this.DiagnosticGroups();
            }

            if (formStarting != null) formStarting.ShowCurrentStep("check project database");
            // DiversityProjects
            string SQL = "SELECT [dbo].[ProjectsDatabase] ();";
            string Db = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            Group DP = new Group(Db);

            this.DiagnosticGroups().Add(DP);

            if (formStarting != null) formStarting.ShowCurrentStep("check technical contacts etc.");
            // Technical contact
            DP.Content.Add(this.CheckAgent("Technical Contact"));
            // Content contact
            DP.Content.Add(this.CheckAgent("Content Contact"));

            DP.Content.Add(this.CheckOwner());

            DP.Content.Add(this.CheckOwnerContact());

            DP.Content.Add(this.CheckInstitution());

            DP.Content.Add(this.CheckAuthor());


            // Database
            string DB = DiversityWorkbench.Settings.DatabaseName;
            Group Database = new Group(DB);
            this.DiagnosticGroups().Add(Database);

            if (formStarting != null) formStarting.ShowCurrentStep("check collections");
            // Collection
            Database.Content.Add(this.CheckCollection());

            if (formStarting != null) formStarting.ShowCurrentStep("check NameURIs");
            // NameURI
            Database.Content.Add(this.CheckNameURI());

            //Database.Content.Add(this.CheckSource(Source.Taxa));

            // CacheDB
            // DB = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
            Group CacheDB = new Group("Cache Database");
            this.DiagnosticGroups().Add(CacheDB);

            if (formStarting != null) formStarting.ShowCurrentStep("check IsoCode");
            // IsoCode
            CacheDB.Content.Add(this.CheckGazetteer());

            if (formStarting != null) formStarting.ShowCurrentStep("check Kingdom");
            CacheDB.Content.Add(this.CheckKingdom());

            if (formStarting != null) formStarting.ShowCurrentStep("check RecordBasis");
            CacheDB.Content.Add(this.CheckRecordBasis());
#if DEBUG
            //formStarting.Close();
#endif

            return this.DiagnosticGroups();
        }

        #region Agents

        private Group CheckAgent(string Role)
        {
            Group Agent = new Group(Role);
            string SQL = "";
            string Result = "";

            // Role present
            Result RolePresent = this.AgentRolePresent(Role);
            Agent.Content.Add(RolePresent);
            if (!RolePresent.OK)
            {
                return Agent;
            }

            // Link
            string Uri = "";
            Result AgentLink = this.AgentLinkPresent(Role, ref Uri);
            Agent.Content.Add(AgentLink);

            Group DA = new Group("DiversityAgents");
            Group CacheDB = new Group("Cache database");

            if (AgentLink.OK)
            {
                Result AgentLinkInAgent = this.AgentLinkInAgent(Uri);
                Agent.Content.Add(AgentLinkInAgent);

                if (this.PostgresDatabase().Length > 0)
                {
                    Result PG_AgentLinkInAgent = this.PostgresAgentLinkInAgent(Uri);
                    Agent.Content.Add(PG_AgentLinkInAgent);
                }

                if (AgentLinkInAgent.OK)
                {
                    DA = this.AgentContactInformation(Role, Uri, "Email");
                    Agent.Content.Add(DA);
                }
                CacheDB = this.AgentTransferredToCacheDB(Role, Uri);
                Agent.Content.Add(CacheDB);

            }
            return Agent;
        }

        private Result AgentRolePresent(string Role)
        {
            // Role present
            Result RolePresent = new Result(Role + " present");
            RolePresent.ToolTip = "If an agent with the role " + Role + " is present in the database " + this.ProjectDB();
            string SQL = "SELECT COUNT(*) " +
                "FROM " + this.ProjectDB() + ".dbo.ProjectAgent AS A INNER JOIN " +
                this.ProjectDB() + ".dbo.ProjectAgentRole AS R ON A.ProjectAgentID = R.ProjectAgentID " +
                " WHERE(R.AgentRole = N'" + Role + "') " +
                " AND A.ProjectID = " + this._ProjectID.ToString();
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result == "0")
            {
                RolePresent.OK = false;
                RolePresent.Description = "An agent with the role " + Role + " is missing";
            }
            else
            {
                RolePresent.OK = true;
                SQL = "SELECT TOP 1 A.AgentName " +
                    "FROM " + this.ProjectDB() + ".dbo.ProjectAgent AS A INNER JOIN " +
                    this.ProjectDB() + ".dbo.ProjectAgentRole AS R ON A.ProjectAgentID = R.ProjectAgentID " +
                    " WHERE(R.AgentRole = N'" + Role + "') " +
                    " AND A.ProjectID = " + this._ProjectID.ToString();
                Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                RolePresent.Description = Result; // Role + " is present";
            }
            return RolePresent;
        }

        private Result AgentLinkPresent(string Role, ref string Uri)
        {
            Result AgentLink = new Result("Link to DiversityAgents");
            AgentLink.ToolTip = "If the agent with the role " + Role + " is linked to the module DiversityAgents";
            // Check
            string SQL = "SELECT TOP (1) A.AgentURI " +
                "FROM " + this.ProjectDB() + ".dbo.ProjectAgent AS A INNER JOIN " +
                this.ProjectDB() + ".dbo.ProjectAgentRole AS R ON A.ProjectAgentID = R.ProjectAgentID " +
                " WHERE(R.AgentRole = N'" + Role + "') " +
                " AND A.ProjectID = " + this._ProjectID.ToString() +
                " ORDER BY A.AgentSequence";
            Uri = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Uri.Length > 0)
            {
                AgentLink.OK = true;
                AgentLink.Description = Uri;
            }
            else
            {
                AgentLink.OK = false;
                AgentLink.Description = "Link to DiversityAgents is missing";
            }
            return AgentLink;
        }

        private System.Collections.Generic.Dictionary<string, Result> AgentLinksPresent(string Role)
        {
            System.Collections.Generic.Dictionary<string, Result> AgentLinks = new Dictionary<string, Result>();

            // Check
            string SQL = "SELECT A.AgentURI, A.AgentName " +
                "FROM " + this.ProjectDB() + ".dbo.ProjectAgent AS A INNER JOIN " +
                this.ProjectDB() + ".dbo.ProjectAgentRole AS R ON A.ProjectAgentID = R.ProjectAgentID " +
                " WHERE(R.AgentRole = N'" + Role + "') " +
                " AND A.ProjectID = " + this._ProjectID.ToString() +
                " ORDER BY A.AgentSequence";
            System.Data.DataTable dt = new System.Data.DataTable();
            string Message = "";
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dt, ref Message);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                string Agent = R["AgentName"].ToString();
                Result AgentLink = new Result(Agent + ": Link to DiversityAgents");
                AgentLink.ToolTip = "If the agent with the role " + Role + " is linked to the module DiversityAgents";
                string Uri = R["AgentURI"].ToString();
                if (Uri.Length > 0)
                {
                    AgentLink.OK = true;
                    AgentLink.Description = Uri;
                }
                else
                {
                    AgentLink.OK = false;
                    AgentLink.Description = "Link to DiversityAgents is missing";
                }
                AgentLinks.Add(Uri, AgentLink);
            }
            return AgentLinks;
        }

        private Result AgentLinkInAgent(string Uri)
        {
            Result AgentLinkInAgent = new Result("Link in table Agent");
            AgentLinkInAgent.ToolTip = "If the information about the agent has been transferred into the sources within the cache database (Sources - Agents)";
            string SQL = "SELECT SourceView FROM Agent A WHERE A.AgentURI = '" + Uri + "'";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result.Length > 0)
            {
                AgentLinkInAgent.OK = true;
                AgentLinkInAgent.Description = "Link provided by " + Result;
            }
            else
            {
                AgentLinkInAgent.OK = false;
                AgentLinkInAgent.Description = "Link not found in table Agent";
            }
            return AgentLinkInAgent;
        }

        private Group AgentContactInformation(string Role, string Uri, string ContactField)
        {
            Result AgentContactField = new Result(ContactField + " of the " + Role);
            AgentContactField.ToolTip = "If the " + ContactField + " of the agent with the role " + Role + " is available as a public information";
            DiversityWorkbench.ServerConnection SC = null;
            string SQL = "";
            try
            {
                SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(Uri);
                string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(Uri);
                SQL = "SELECT TOP (1) " + ContactField + " " +
                    "FROM PublicContactInformation " +
                    "WHERE AgentID = " + ID +
                    " ORDER BY DisplayOrder";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionString);
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                string ContactInfo = "";
                bool ContactInfoPresent = false;
                try
                {
                    ContactInfo = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    ContactInfoPresent = ContactInfo != string.Empty;
                }
                catch { }
                con.Close();
                if (ContactInfo.Length > 0)
                {
                    AgentContactField.OK = true;
                    AgentContactField.Description = ContactInfo;
                }
                else
                {
                    if (ContactInfoPresent)
                    {
                        SQL = "SELECT COUNT(*) " + ContactField + " " +
                        "FROM PublicContactInformation " +
                        "WHERE AgentID = " + ID;
                        C.CommandText = SQL;
                        con.Open();
                        string result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        con.Close();
                        if (result == "1")
                        {
                            AgentContactField.OK = false;
                            AgentContactField.Description = "Contact information present, but E-mail missing";
                        }
                        else
                        {
                            this.AgentTableInfo(ref AgentContactField, "AgentContactInformation", ContactField, SC, ID);
                        }
                    }
                    else
                    {
                        AgentContactField.OK = false;
                        AgentContactField.Description = "Contact information missing";
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            Group DA = new Group("DiversityAgents");
            if (SC != null)
                DA.Name = SC.DatabaseName;
            DA.Content.Add(AgentContactField);
            return DA;
        }

        private void AgentTableInfo(ref Result AgentResult, string Table, string Column, DiversityWorkbench.ServerConnection SC, string ID)
        {
            try
            {
                string OrderColumn = "DisplayOrder";
                if (Table == "AgentImage")
                    OrderColumn = "Sequence";
                else
                    OrderColumn = "AgentID";
                string SQL = "SELECT C.AgentID, C." + OrderColumn + ", C." + Column + ", C.DataWithholdingReason, A.DataWithholdingReason AS AgentWithholdingReason ";
                if (Table == "AgentContactInformation")
                    SQL += ", C.ValidFrom, C.ValidUntil ";
                SQL += " FROM " + Table + " AS C INNER JOIN  " +
                      " Agent AS A ON C.AgentID = A.AgentID " +
                      " WHERE A.AgentID = " + ID +
                      " AND C." + Column + " <> '' ";
                if (Table == "AgentImage")
                    SQL += " AND C.Type = 'Logo'";
                else
                    SQL += " ORDER BY C." + OrderColumn + "";
                System.Data.DataTable dtContactField = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
                ad.Fill(dtContactField);
                if (dtContactField.Rows.Count == 0)
                {
                    AgentResult.OK = false;
                    AgentResult.Description = Column + " missing";
                }
                else
                {
                    if (dtContactField.Rows.Count == 1)
                    {
                        if (!dtContactField.Rows[0]["AgentWithholdingReason"].Equals(System.DBNull.Value) && dtContactField.Rows[0]["AgentWithholdingReason"].ToString().Length > 0)
                        {
                            AgentResult.OK = false;
                            AgentResult.Description = "Agent withheld";
                        }
                        else if (!dtContactField.Rows[0]["DataWithholdingReason"].Equals(System.DBNull.Value) && dtContactField.Rows[0]["DataWithholdingReason"].ToString().Length > 0)
                        {
                            AgentResult.OK = false;
                            AgentResult.Description = Column + " withheld";
                        }
                        else
                        {
                            AgentResult.OK = true;
                            AgentResult.Description = Column + ": " + dtContactField.Rows[0][Column].ToString();
                        }

                    }
                    else
                    {
                        System.Data.DataRow[] RR = dtContactField.Select("AgentWithholdingReason IS NULL OR AgentWithholdingReason = ''");
                        if (RR.Length == 1)
                        {
                            AgentResult.OK = true;
                            AgentResult.Description = Column + ": " + RR[0][Column].ToString();
                        }
                        else if (RR.Length > 1)
                        {
                            System.Data.DataRow[] rr = dtContactField.Select("DataWithholdingReason IS NULL OR DataWithholdingReason = ''");
                            if (rr.Length == 1)
                            {
                                AgentResult.OK = true;
                                AgentResult.Description = Column + ": " + rr[0][Column].ToString();
                            }
                            else if (rr.Length == 0)
                            {
                                AgentResult.OK = false;
                                AgentResult.Description = Column + " withheld: " + rr[0]["DataWithholdingReason"].ToString();
                            }
                            else
                            {
                                AgentResult.OK = false;
                                AgentResult.Description = Column + " contains more than 1 entry. First entry: " + rr[0][Column].ToString();
                            }
                        }
                        else
                        {
                            AgentResult.OK = false;
                            AgentResult.Description = Column + "Agent withheld: " + RR[0]["AgentWithholdingReason"].ToString();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private Group AgentTransferredToCacheDB(string Role, string Uri)
        {
            Result TransferredToCacheDB = new Result("Transferred to cache database");
            string SQL = "SELECT TOP 1 R.AgentURI " +
                " FROM [Project_" + this._Project + "].[CacheProjectAgentRole]   AS R INNER JOIN " +
                " [Project_" + this._Project + "].CacheProjectAgent AS A ON R.ProjectID = A.ProjectID AND R.AgentName = A.AgentName AND R.AgentURI = A.AgentURI " +
                " WHERE(R.AgentRole = N'" + Role + "') " +
                " AND(R.ProjectID = " + this._ProjectID.ToString() + ") " +
                " AND(R.AgentURI = '" + Uri + "') " +
                " ORDER BY A.AgentSequence";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result.Length > 0 && Result == Uri)
            {
                TransferredToCacheDB.OK = true;
                TransferredToCacheDB.Description = Result;
            }
            else
            {
                TransferredToCacheDB.OK = false;
                TransferredToCacheDB.Description = "Not transferred into cache database";
            }
            Group CacheDB = new Group("Cache Database");
            CacheDB.Content.Add(TransferredToCacheDB);

            if (this.PostgresDatabase().Length > 0)
            {
                Result TransferredToPostgresDB = new Result(this.PostgresDatabase());
                TransferredToPostgresDB.ForPostgres = true;
                SQL = "SELECT R.\"AgentURI\" " +
                    " FROM \"Project_" + this._Project + "\".\"CacheProjectAgentRole\"   AS R INNER JOIN " +
                    " \"Project_" + this._Project + "\".\"CacheProjectAgent\" AS A ON R.\"ProjectID\" = A.\"ProjectID\" AND R.\"AgentName\" = A.\"AgentName\" AND R.\"AgentURI\" = A.\"AgentURI\" " +
                    " WHERE(R.\"AgentRole\" = N'" + Role + "') " +
                    " AND (R.\"ProjectID\" = " + this._ProjectID.ToString() + ") " +
                    " AND (R.\"AgentURI\" = '" + Uri + "') " +
                    " ORDER BY A.\"AgentSequence\" LIMIT 1";
                string Message = "";
                Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message);
                if (Result.Length > 0 && Result == Uri)
                {
                    TransferredToPostgresDB.OK = true;
                    TransferredToPostgresDB.Description = "";
                }
                else
                {
                    TransferredToPostgresDB.OK = false;
                    TransferredToPostgresDB.Description = "Not transferred into postgres database";
                }
                CacheDB.Content.Add(TransferredToPostgresDB);
            }

            return CacheDB;
        }

        private Result PostgresAgentLinkInAgent(string Uri)
        {
            Result PG_AgentLinkInAgent = new Result(this.PostgresDatabase());
            PG_AgentLinkInAgent.ForPostgres = true;
            string SQL = "SELECT \"SourceView\" FROM \"Agent\" A WHERE A.\"AgentURI\" = '" + Uri + "'";
            string Message = "";
            string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, ref Message);
            if (Result.Length > 0)
            {
                PG_AgentLinkInAgent.OK = true;
                PG_AgentLinkInAgent.Description = "link provided by " + Result;
            }
            else
            {
                PG_AgentLinkInAgent.OK = false;
                PG_AgentLinkInAgent.Description = "Link not found in table Agent";
            }
            return PG_AgentLinkInAgent;
        }

        private Group CheckOwner()
        {
            Group Agent = new Group("Data Owner");

            string Role = "Data Owner";

            // Role present
            Result RolePresent = this.AgentRolePresent(Role);
            Agent.Content.Add(RolePresent);
            if (!RolePresent.OK)
                return Agent;

            // Link
            string Uri = "";
            Result AgentLink = this.AgentLinkPresent(Role, ref Uri);
            Agent.Content.Add(AgentLink);

            if (AgentLink.OK)
            {
                Result AgentLinkInAgent = this.AgentLinkInAgent(Uri);
                Agent.Content.Add(AgentLinkInAgent);

                if (this.PostgresDatabase().Length > 0)
                {
                    Result PG_AgentLinkInAgent = this.PostgresAgentLinkInAgent(Uri);
                    Agent.Content.Add(PG_AgentLinkInAgent);
                }

                if (AgentLinkInAgent.OK)
                {
                    Group DA = this.AgentContactInformation(Role, Uri, "URI");
                    Agent.Content.Add(DA);

                    DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(Uri);
                    string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(Uri);

                    Result AgentAbbreviation = new Result("Abbreviation of the " + Role);
                    this.AgentTableInfo(ref AgentAbbreviation, "Agent", "Abbreviation", SC, ID);
                    DA.Content.Add(AgentAbbreviation);

                    Result AgentLogo = new Result("Logo of the " + Role);
                    this.AgentTableInfo(ref AgentLogo, "AgentImage", "URI", SC, ID);
                    DA.Content.Add(AgentLogo);
                }


                Group CacheDB = this.AgentTransferredToCacheDB(Role, Uri);
                Agent.Content.Add(CacheDB);
            }

            return Agent;
        }

        private Group CheckOwnerContact()
        {
            string Role = "Data Owner Contact";
            Group Agent = new Group(Role);

            // Role present
            Result RolePresent = this.AgentRolePresent(Role);
            Agent.Content.Add(RolePresent);
            if (!RolePresent.OK)
                return Agent;

            // Link
            string Uri = "";
            Result AgentLink = this.AgentLinkPresent(Role, ref Uri);
            Agent.Content.Add(AgentLink);

            if (AgentLink.OK)
            {
                Result AgentLinkInAgent = this.AgentLinkInAgent(Uri);
                Agent.Content.Add(AgentLinkInAgent);

                if (this.PostgresDatabase().Length > 0)
                {
                    Result PG_AgentLinkInAgent = this.PostgresAgentLinkInAgent(Uri);
                    Agent.Content.Add(PG_AgentLinkInAgent);
                }

                if (AgentLinkInAgent.OK)
                {
                    Group DA = this.AgentContactInformation(Role, Uri, "Email");
                    Agent.Content.Add(DA);
                }

                Group CacheDB = this.AgentTransferredToCacheDB(Role, Uri);
                Agent.Content.Add(CacheDB);
            }

            return Agent;
        }

        private Group CheckInstitution()
        {
            string Role = "Source Institution";
            Group Agent = new Group(Role);

            // Role present
            Result RolePresent = this.AgentRolePresent(Role);
            RolePresent.Description += ". Only needed if data contain observations (units without parts) ";
            RolePresent.ToolTip += ". This is only needed if there are data of e.g. organisms that do not contain parts stored within a collection";
            Agent.Content.Add(RolePresent);
            if (!RolePresent.OK)
            {
                return Agent;
            }

            // Link
            string Uri = "";
            Result AgentLink = this.AgentLinkPresent(Role, ref Uri);
            Agent.Content.Add(AgentLink);

            if (AgentLink.OK)
            {
                Result AgentLinkInAgent = this.AgentLinkInAgent(Uri);
                Agent.Content.Add(AgentLinkInAgent);

                if (this.PostgresDatabase().Length > 0)
                {
                    Result PG_AgentLinkInAgent = this.PostgresAgentLinkInAgent(Uri);
                    Agent.Content.Add(PG_AgentLinkInAgent);
                }

                Group CacheDB = this.AgentTransferredToCacheDB(Role, Uri);
                Agent.Content.Add(CacheDB);
            }

            return Agent;
        }

        private Group CheckAuthor()
        {
            Group Agents = new Group("Author(s)");

            string Role = "Author";

            // Role present
            Result RolePresent = this.AgentRolePresent(Role);
            RolePresent.Description += ". Author for citation ";
            RolePresent.ToolTip += ". Will be included as author of citation of the project";
            Agents.Content.Add(RolePresent);
            if (!RolePresent.OK)
                return Agents;

            // Link
            System.Collections.Generic.Dictionary<string, Result> AgentLinks = this.AgentLinksPresent(Role);
            foreach (System.Collections.Generic.KeyValuePair<string, Result> KV in AgentLinks)
            {
                Group Agent = new Group("Author");
                Agents.Content.Add(Agent);
                Agent.Content.Add(KV.Value);

                if (KV.Value.OK)
                {
                    Result AgentLinkInAgent = this.AgentLinkInAgent(KV.Key);
                    Agent.Content.Add(AgentLinkInAgent);

                    if (this.PostgresDatabase().Length > 0)
                    {
                        Result PG_AgentLinkInAgent = this.PostgresAgentLinkInAgent(KV.Key);
                        Agent.Content.Add(PG_AgentLinkInAgent);
                    }

                    //if (AgentLinkInAgent.OK)
                    //{
                    //    Group DA = this.AgentContactInformation(Role, KV.Key, "URI");
                    //    Agent.Content.Add(DA);

                    //    DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(KV.Key);
                    //    string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(KV.Key);

                    //    Result AgentAbbreviation = new Result("Abbreviation of the " + Role);
                    //    this.AgentTableInfo(ref AgentAbbreviation, "Agent", "Abbreviation", SC, ID);
                    //    DA.Content.Add(AgentAbbreviation);

                    //    Result AgentLogo = new Result("Logo of the " + Role);
                    //    this.AgentTableInfo(ref AgentLogo, "AgentImage", "URI", SC, ID);
                    //    DA.Content.Add(AgentLogo);
                    //}


                    Group CacheDB = this.AgentTransferredToCacheDB(Role, KV.Key);
                    Agent.Content.Add(CacheDB);
                }
            }

            return Agents;
        }

        #endregion

        private Group CheckGazetteer()
        {
            Group Gazetteer = new Group("Gazetteer");
            string SQL = "SELECT count(*) " +
                "FROM Gazetteer C " +
                "JOIN Gazetteer G ON C.PlaceID = G.PlaceID " +
                "WHERE G.LanguageCode = 'ISO 3166 ALPHA-3' " +
                "AND(C.LanguageCode LIKE 'ISO %' OR C.LanguageCode IS NULL) " +
                "AND C.NameID<> G.NameID; ";
            Result IsoCode = new Result("List for ISO code");
            IsoCode.ToolTip = "If the source retrieved from the module DivesityGazetteer contains the ISO codes for the countries (Sources - Gazetteer)";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result == "0")
            {
                IsoCode.OK = false;
                IsoCode.Description = "Missing";
            }
            else
            {
                IsoCode.OK = true;
                IsoCode.Description = Result + " entries";
            }
            Gazetteer.Content.Add(IsoCode);

            if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null &&
                DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name).Length > 0)
            {
                SQL = "SELECT count(*) " +
                "FROM \"Gazetteer\" \"C\" " +
                "JOIN \"Gazetteer\" \"G\" ON \"C\".\"PlaceID\" = \"G\".\"PlaceID\" " +
                "WHERE \"G\".\"LanguageCode\"::text = 'ISO 3166 ALPHA-3'::text " + "" +
                "AND(\"C\".\"LanguageCode\"::text!~~'ISO %'::text OR \"C\".\"LanguageCode\" IS NULL) " +
                "AND \"C\".\"NameID\" <> \"G\".\"NameID\"; ";
                Result TransferredToPostgresDB = new Result(DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name);
                TransferredToPostgresDB.ForPostgres = true;
                Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);
                if (Result == "0")
                {
                    TransferredToPostgresDB.OK = false;
                    TransferredToPostgresDB.Description = "Missing";
                }
                else
                {
                    TransferredToPostgresDB.OK = true;
                    TransferredToPostgresDB.Description = Result + " entries";
                }
                Gazetteer.Content.Add(TransferredToPostgresDB);
            }
            return Gazetteer;
        }

        private Group CheckCollection()
        {
            string DB = DiversityWorkbench.Settings.DatabaseName;
            Group Collection = new Group("Collection");
            string SQL = "SELECT CollectionName, CollectionID " +
                "FROM Project_" + this._Project + ".CacheCollection C " +
                "WHERE C.CollectionAcronym = '' OR C.CollectionAcronym IS NULL; ";
            Result Acronyms = new Result("Collection Acronyms");
            Acronyms.ToolTip = "If all collections contained in the data or parents of these collections have an acronym";
            System.Data.DataTable dtAcronyms = new System.Data.DataTable();
            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtAcronyms, ref Message) && Message.Length == 0)
            {
                if (dtAcronyms.Rows.Count == 0)
                {
                    Acronyms.OK = true;
                    Acronyms.Description = "All collections or their superior collections have acronyms";
                }
                else
                {
                    Acronyms.OK = false;
                    Acronyms.Description = dtAcronyms.Rows.Count.ToString() + " Collections or their superior collections with missing acronym, e.g.:";
                    int i = 0;
                    foreach(System.Data.DataRow R in dtAcronyms.Rows)
                    {
                        Acronyms.Description += R[0].ToString() + " (ID = " + R[1].ToString() + "); ";
                        i++;
                        if (i > 7)
                            break;
                    }
                }
            }
            Collection.Content.Add(Acronyms);

            if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null &&
                DiversityWorkbench.PostgreSQL.Connection.ConnectionString(DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name).Length > 0)
            {
                int iMissing = dtAcronyms.Rows.Count;
                dtAcronyms.Clear();
                SQL = "SELECT \"CollectionName\", \"CollectionID\" " +
                "FROM \"Project_" + this._Project + "\".\"CacheCollection\" AS C " +
                "WHERE C.\"CollectionAcronym\" = '' OR C.\"CollectionAcronym\" IS NULL; ";
                Result TransferredToPostgresDB = new Result(DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name);
                TransferredToPostgresDB.ForPostgres = true;
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtAcronyms, ref Message);
                if (dtAcronyms.Rows.Count == 0 && iMissing == 0)
                {
                    TransferredToPostgresDB.OK = true;
                    TransferredToPostgresDB.Description = dtAcronyms.Rows.Count.ToString() + " Collections with missing acronym of superior collections";
                }
                else
                {
                    TransferredToPostgresDB.OK = false;
                    if (iMissing == dtAcronyms.Rows.Count)
                    {
                        TransferredToPostgresDB.Description = dtAcronyms.Rows.Count.ToString() + " Collections with missing acronym of superior collection, e.g.:";
                        int i = 0;
                        foreach (System.Data.DataRow R in dtAcronyms.Rows)
                        {
                            TransferredToPostgresDB.Description += R[0].ToString() + " (" + R[1].ToString() + "); ";
                            i++;
                            if (i > 7)
                                break;
                        }
                    }
                    else
                    {
                        TransferredToPostgresDB.Description = "Number of collections in " + DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name + " (" + dtAcronyms.Rows.Count.ToString() + ") do not match number in cache database (" + iMissing.ToString() + ")";
                    }
                }
                Collection.Content.Add(TransferredToPostgresDB);
            }
            return Collection;
        }

        private Group CheckNameURI()
        {
            string DB = DiversityWorkbench.Settings.DatabaseName;
            Group Taxa = new Group("Taxa");
            string SQL = "select i.NameURI, i.IdentificationUnitID FROM [dbo].[Identification] i inner join CollectionProject P on i.CollectionSpecimenID = P.CollectionSpecimenID and P.ProjectID = " + this._ProjectID + " where NameURI <> '' and (isnumeric(SUBSTRING(NameURI,len(NameURI),1) ) = 0 or NameURI not like 'http://%') ; ";
            Result NameUris = new Result("NameURI");
            NameUris.ToolTip = "If all entries for the NameURI of the taxa are correct";
            System.Data.DataTable dtUnitIDs = new System.Data.DataTable();
            string Message = "";
            if (DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtUnitIDs, ref Message) && Message.Length == 0)
            {
                if (dtUnitIDs.Rows.Count == 0)
                {
                    NameUris.OK = true;
                    NameUris.Description = "All entries for the NameURI of the taxa are correct";
                }
                else
                {
                    NameUris.OK = false;
                    NameUris.Description = dtUnitIDs.Rows.Count.ToString() + " entries for the NameURI of the taxa are incorrect:  First 5 examples:   ";
                    int i = 0;
                    foreach (System.Data.DataRow R in dtUnitIDs.Rows)
                    {
                        NameUris.Description += R[0].ToString() + "  (IdentificationUnitID = " + R[1].ToString() + ");   ";
                        i++;
                        if (i > 5)
                            break;
                    }
                    //NameUris.Description += "  All IdentificationUnitIDs for query:   ";
                    NameUris.DescriptionForSelection += "All IdentificationUnitIDs for query in " + DB;
                    foreach (System.Data.DataRow R in dtUnitIDs.Rows)
                    {
                        //NameUris.Description += R[1].ToString() + " | ";
                        if (NameUris.Selection.Length > 0) NameUris.Selection += " | ";
                        NameUris.Selection += R[1].ToString();
                    }
                }
            }
            Taxa.Content.Add(NameUris);

            return Taxa;
        }

        private enum Source { Agents, Taxa }

        private Group CheckSource(Source source)
        {
            string DB = DiversityWorkbench.Settings.DatabaseName;
            Group Source = new Group("Source for " + source.ToString());
            string SQL = "";
            switch(source)
            {
                case ABCD.Source.Taxa:
                    SQL = "SELECT distinct I.NameURI, I.TaxonomicName " +
                        " FROM[Project_BFLportal01coll].[CacheIdentification] I left outer join[dbo].[TaxonSynonymy] T on I.NameURI = T.NameURI and I.NameURI<> '' and i.TaxonomicName<> '' WHERE T.NameURI is null and I.NameURI<> '' and i.TaxonomicName<> '' " +
                        " order by i.TaxonomicName";
                    break;
            }
            Result Uris = new Result("URI");
            Uris.ToolTip = "If every URI in the data exist in the sources for " + source.ToString().ToLower();
            System.Data.DataTable dtUnitIDs = new System.Data.DataTable();
            string Message = "";
            if (DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtUnitIDs, ref Message) && Message.Length == 0)
            {
                if (dtUnitIDs.Rows.Count == 0)
                {
                    Uris.OK = true;
                    Uris.Description = "All entries for the NameURI of the taxa are correct";
                }
                else
                {
                    Uris.OK = false;
                    Uris.Description = dtUnitIDs.Rows.Count.ToString() + " entries for the NameURI of the taxa are incorrect:  First 5 examples:   ";
                    int i = 0;
                    foreach (System.Data.DataRow R in dtUnitIDs.Rows)
                    {
                        Uris.Description += R[0].ToString() + "  (IdentificationUnitID = " + R[1].ToString() + ");   ";
                        i++;
                        if (i > 5)
                            break;
                    }
                    Uris.Description += "  All NameURI for query:   ";
                    foreach (System.Data.DataRow R in dtUnitIDs.Rows)
                    {
                        Uris.Description += R[1].ToString() + " | ";
                    }
                }
            }
            Source.Content.Add(Uris);

            return Source;
        }


        private Group CheckKingdom()
        {
            Group Kingdom = new Group("Kingdom");
            string SQL = "SELECT DISTINCT TaxonomicGroup " +
                "FROM Project_" + this._Project + ".CacheIdentificationUnit C " +
                "WHERE C.TaxonomicGroup NOT IN ('alga', 'amphibian', 'animal', 'artefact', 'arthropod', 'bacterium', 'bird', 'bryophyte', " +
                "'cnidaria', 'Coleoptera', 'Diptera', 'echinoderm', 'evertebrate', 'fish', 'fungus', 'gall', 'Heteroptera', 'Hymenoptera', " +
                "'insect', 'Lepidoptera', 'lichen', 'mammal', 'mineral', 'mollusc', 'myxomycete', 'other', 'plant', 'reptile', 'rock', " +
                "'soil', 'soil horizon', 'spider', 'unknown', 'vertebrate', 'virus') ";
            System.Data.DataTable dtTax = new System.Data.DataTable();
            string Message = "";
            Result TaxGroups = new Result("Taxonomic Groups");
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtTax, ref Message) && Message.Length == 0)
            {
                if (dtTax.Rows.Count == 0)
                {
                    TaxGroups.OK = true;
                    TaxGroups.Description = "Kingdom available for all taxonomic groups";
                }
                else
                {
                    TaxGroups.OK = false;
                    TaxGroups.Description = dtTax.Rows.Count.ToString() + " taxonomic groups are not available in table ABCD__Kingdom_TaxonomicGroups , e.g.:";
                    int i = 0;
                    foreach (System.Data.DataRow R in dtTax.Rows)
                    {
                        TaxGroups.Description += R[0].ToString() + "; ";
                        i++;
                        if (i > 7)
                            break;
                    }
                }
            }
            Kingdom.Content.Add(TaxGroups);

            return Kingdom;
        }


        private Group CheckRecordBasis()
        {
            Group RecordBasis = new Group("RecordBasis");
            string SQL = "SELECT DISTINCT MaterialCategory " +
                "FROM Project_" + this._Project + ".CacheCollectionSpecimenPart C " +
                "WHERE C.MaterialCategory NOT IN ('bones', 'complete skeleton', 'cones', 'cultures', 'DNA lyophilised', 'DNA sample', 'drawing', 'drawing or photograph', " +
                "'dried specimen', 'earth science specimen', 'egg', 'fossil bones', 'fossil complete skeleton', 'fossil incomplete skeleton', 'fossil otolith', " +
                "'fossil postcranial skeleton', 'fossil scales', 'fossil shell', 'fossil single bones', 'fossil skull', 'fossil specimen', 'fossil tooth', " +
                "'herbarium sheets', 'human observation', 'icones', 'incomplete skeleton', 'living specimen', 'machine observation', 'material sample', " +
                "'medium', 'micr. slide', 'mineral specimen', 'mould', 'nest', 'observation', 'other specimen', 'otolith', 'pelt', 'photogr. print', 'photogr. slide', " +
                "'postcranial skeleton', 'preserved specimen', 'SEM table', 'shell', 'single bones', 'skull', 'sound', 'specimen', 'TEM specimen', 'thin section', " +
                "'tissue sample', 'tooth', 'trace', 'trace fossil', 'vial') ";
            System.Data.DataTable dtMat = new System.Data.DataTable();
            string Message = "";
            Result MatCat = new Result("Material Category");
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlFillTableInCacheDB(SQL, ref dtMat, ref Message) && Message.Length == 0)
            {
                if (dtMat.Rows.Count == 0)
                {
                    MatCat.OK = true;
                    MatCat.Description = "RecordBasis available for all material categories ";
                }
                else
                {
                    MatCat.OK = false;
                    MatCat.Description = dtMat.Rows.Count.ToString() + " material categories are not available in table ABCD__RecordBasis_MaterialCategories , e.g.:";
                    int i = 0;
                    foreach (System.Data.DataRow R in dtMat.Rows)
                    {
                        MatCat.Description += R[0].ToString() + "; ";
                        i++;
                        if (i > 7)
                            break;
                    }
                }
            }
            RecordBasis.Content.Add(MatCat);

            return RecordBasis;
        }

    }
}
