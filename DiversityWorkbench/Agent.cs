using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class Agent : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {

        #region Parameter
        private DiversityWorkbench.QueryCondition _QueryConditionProject;
        private System.Collections.Generic.Dictionary<string, string> _ServiceList;
        #endregion

        #region Construction

        public Agent(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }

        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityAgents"; }

        /// <summary>
        /// Additional services provided by webservices by external providers
        /// </summary>
        /// <returns></returns>
        public override System.Collections.Generic.Dictionary<string, string> AdditionalServicesOfModule()
        {
            if (this._ServiceList == null)
            {
                this._ServiceList = new Dictionary<string, string>();
                _ServiceList.Add("CacheDB", "Cache database");
            }
            return _ServiceList;
        }
#if DEBUG
#endif


        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int id)
        {
            System.Collections.Generic.Dictionary<string, string> values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                // Checking existence
                // SQL = "SELECT COUNT(*) FROM " + Prefix + "Agent AS A " +
                //    "WHERE A.AgentID = " + ID.ToString();
                //using (Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString))
                //{
                //    conn.Open();
                //    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, conn);
                //    string Count = C.ExecuteScalar()?.ToString();
                //    conn.Close();
                //    int i;
                //    if (Count.Length == 0 || (int.TryParse(Count, out i) && i < 1))
                //    {
                //        return null;
                //    }
                //}

                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
                string agentNameDisplayType = "";
                bool agentNameDisplayTypeOk = true;
                string message = "";
                string sql = "SELECT MIN(P.AgentNameDisplayType) AS DisplayType " +
                    "FROM ProjectProxy AS P INNER JOIN " +
                    "AgentProject AS A ON P.ProjectID = A.ProjectID " +
                    "GROUP BY A.AgentID " +
                    "HAVING A.AgentID = " + id.ToString();
                agentNameDisplayType = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql, ref message);

                if (agentNameDisplayType.Length == 0)
                {
                    sql = "SELECT AgentNameDisplayType FROM " + Prefix + "AgentNameDisplayType WHERE AgentID = " + id.ToString();
                    agentNameDisplayType = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sql, ref message);
                    sql = "SELECT DefaultAgentNameDisplayType FROM " + Prefix + "ViewDefaultAgentNameDisplayType";
                    try
                    {
                        con.Open();
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(sql, con);
                        agentNameDisplayType = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        if (agentNameDisplayType == string.Empty)
                            agentNameDisplayTypeOk = false;
                        con.Close();
                    }
                    catch (System.Exception ex) { agentNameDisplayTypeOk = false; }
                    if (!agentNameDisplayTypeOk && this._ServerConnection.LinkedServer.Length == 0)
                    {
                        try
                        {
                            sql = "SELECT " + Prefix + "DefaultAgentNameDisplayType()";
                            if (con.State == System.Data.ConnectionState.Closed)
                                con.Open();
                            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(sql, con);
                            agentNameDisplayType = C.ExecuteScalar()?.ToString() ?? string.Empty;
                            con.Close();
                        }
                        catch (System.Exception ex)
                        {
                            // ignored
                        }
                    }
                }

                sql = "SELECT B.BaseURL + cast(A.AgentID as varchar) AS [Link] " +
                    "FROM " + Prefix + "ViewBaseURL AS B, " +
                    Prefix + "Agent AS A " +
                    "WHERE A.AgentID = " + id.ToString();
                this.getDataFromTable(sql, ref values);

                sql = "SELECT UPPER(A.SynonymisationType) + ' ' + S.AgentName AS [Synonym To] " +
                    "FROM " + Prefix + "Agent AS A INNER JOIN " +
                    Prefix + "Agent AS S ON A.SynonymToAgentID = S.AgentID " +
                    "WHERE A.AgentID = " + id.ToString();
                this.getDataFromTable(sql, ref values);

                // getting the ID of the Agent if a replacement happend
                sql = "SELECT case when not a.SynonymToAgentID is null and a.SynonymisationType = 'replaced with' then [SynonymToAgentID] else [AgentID] end AS ID " +
                    "FROM [dbo].[Agent] a " +
                    "WHERE a.AgentID = " + id.ToString();
                try
                {
                    if (id > -1)
                    {
                        int i = 0;
                        con.Open();
                        Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(sql, con);
                        if (int.TryParse(C.ExecuteScalar()?.ToString(), out i))
                            id = i;
                        con.Close();
                    }
                }
                catch (System.Exception ex) { }

                // getting the synonym of the Agent if available
                sql = "SELECT case when A.SynonymisationType is null then '' else A.SynonymisationType + ': ' end + A.AgentName AS Synonym " +
                    "FROM [Agent] A " +
                    "inner join [Agent] S on S.AgentID = dbo.agentsynonymtopid(A.AgentID) " +
                    "and dbo.agentsynonymtopid(A.AgentID) <> A.AgentID and A.AgentID = " + id.ToString();
                try
                {
                    this.getDataFromTable(sql, ref values);
                }
                catch (System.Exception ex) { }


                if (this._ServerConnection.LinkedServer.Length == 0)
                {
                    sql = "SELECT dbo.BaseURL() + CAST(A.AgentID AS VARCHAR) AS _URI, N.AgentName AS _DisplayText, N.AgentName AS [Displayed Name], A.AgentName, " +
                        "CASE WHEN AgentType = 'person' THEN CASE WHEN AgentTitle IS NULL THEN '' ELSE AgentTitle + ' ' END + GivenName + " +
                        "CASE WHEN GivenNamePostfix IS NULL THEN '' ELSE ' ' + GivenNamePostfix END + ' ' + " +
                        "CASE WHEN InheritedNamePrefix IS NULL THEN '' ELSE ' ' + InheritedNamePrefix END + " +
                        "InheritedName + " +
                        "CASE WHEN InheritedNamePostfix IS NULL THEN '' ELSE ' ' + InheritedNamePostfix END " +
                        "ELSE '' END AS PersonName, " +
                        "GivenName, InheritedName, " +
                        "Abbreviation, AgentType AS [Agent type], AgentRole AS [Agent role], Description, Notes, " +
                        "CASE WHEN [ValidFromDay] IS NULL THEN '' ELSE cast([ValidFromDay] as varchar) + '.' END +  " +
                        "CASE WHEN [ValidFromMonth] IS NULL THEN '' ELSE cast([ValidFromMonth] as varchar) + '.' END +  " +
                        "CASE WHEN [ValidFromYear] IS NULL THEN '' ELSE cast([ValidFromYear] as varchar) END AS [From_Date], " +
                        "CASE WHEN [ValidUntilDay] IS NULL THEN '' ELSE cast([ValidUntilDay] as varchar) + '.'  END +  " +
                        "CASE WHEN [ValidUntilMonth] IS NULL THEN '' ELSE cast([ValidUntilMonth] as varchar) + '.'  END +  " +
                        "CASE WHEN [ValidUntilYear] IS NULL THEN '' ELSE cast([ValidUntilYear] as varchar)  END AS [Until_Date], " +
                        "CASE WHEN [RevisionLevel] IS NULL THEN '' ELSE [RevisionLevel] END AS [Level of Revision] " +
                        "FROM  Agent A, AgentNames('" + agentNameDisplayType + "') N " +
                        "WHERE A.AgentID = N.AgentID AND A.AgentID =  " + id.ToString();
                    this.getDataFromTable(sql, ref values);
                }
                else
                {
                    sql = "SELECT U.BaseURL + CAST(A.AgentID AS VARCHAR) AS _URI, N.AgentName AS _DisplayText, N.AgentName AS [Displayed Name], A.AgentName, " +
                        "CASE WHEN AgentType = 'person' THEN CASE WHEN AgentTitle IS NULL THEN '' ELSE AgentTitle + ' ' END + GivenName + " +
                        "CASE WHEN GivenNamePostfix IS NULL THEN '' ELSE ' ' + GivenNamePostfix END + ' ' + " +
                        "CASE WHEN InheritedNamePrefix IS NULL THEN '' ELSE ' ' + InheritedNamePrefix END + " +
                        "InheritedName + " +
                        "CASE WHEN InheritedNamePostfix IS NULL THEN '' ELSE ' ' + InheritedNamePostfix END " +
                        "ELSE '' END AS PersonName, " +
                        "Abbreviation, AgentType AS [Agent type], AgentRole AS [Agent role], Description, Notes, " +
                        "CASE WHEN [ValidFromDay] IS NULL THEN '' ELSE cast([ValidFromDay] as varchar) + '.' END +  " +
                        "CASE WHEN [ValidFromMonth] IS NULL THEN '' ELSE cast([ValidFromMonth] as varchar) + '.' END +  " +
                        "CASE WHEN [ValidFromYear] IS NULL THEN '' ELSE cast([ValidFromYear] as varchar) END AS [From_Date], " +
                        "CASE WHEN [ValidUntilDay] IS NULL THEN '' ELSE cast([ValidUntilDay] as varchar) + '.'  END +  " +
                        "CASE WHEN [ValidUntilMonth] IS NULL THEN '' ELSE cast([ValidUntilMonth] as varchar) + '.'  END +  " +
                        "CASE WHEN [ValidUntilYear] IS NULL THEN '' ELSE cast([ValidUntilYear] as varchar)  END AS [Until_Date], " +
                        "CASE WHEN [RevisionLevel] IS NULL THEN '' ELSE [RevisionLevel] END AS [Level of Revision] " +
                        "FROM  " + Prefix + "Agent A, " + Prefix + "ViewAgentNames N , " + Prefix + "ViewBaseURL U " +
                        "WHERE A.AgentID = N.AgentID AND A.AgentID =  " + id.ToString();
                    if (!this.getDataFromTable(sql, ref values) && this._ServerConnection.LinkedServer.Length == 0)
                    {
                        sql = "SELECT dbo.BaseURL() + CAST(A.AgentID AS VARCHAR) AS _URI, N.AgentName AS _DisplayText, N.AgentName AS [Displayed Name], A.AgentName, " +
                            "CASE WHEN AgentType = 'person' THEN CASE WHEN AgentTitle IS NULL THEN '' ELSE AgentTitle + ' ' END + GivenName + " +
                            "CASE WHEN GivenNamePostfix IS NULL THEN '' ELSE ' ' + GivenNamePostfix END + ' ' + " +
                            "CASE WHEN InheritedNamePrefix IS NULL THEN '' ELSE ' ' + InheritedNamePrefix END + " +
                            "InheritedName + " +
                            "CASE WHEN InheritedNamePostfix IS NULL THEN '' ELSE ' ' + InheritedNamePostfix END " +
                            "ELSE '' END AS PersonName, " +
                            "Abbreviation, AgentType AS [Agent type], AgentRole AS [Agent role], Description, Notes, " +
                            "CASE WHEN [ValidFromDay] IS NULL THEN '' ELSE cast([ValidFromDay] as varchar) + '.' END +  " +
                            "CASE WHEN [ValidFromMonth] IS NULL THEN '' ELSE cast([ValidFromMonth] as varchar) + '.' END +  " +
                            "CASE WHEN [ValidFromYear] IS NULL THEN '' ELSE cast([ValidFromYear] as varchar) END AS [From_Date], " +
                            "CASE WHEN [ValidUntilDay] IS NULL THEN '' ELSE cast([ValidUntilDay] as varchar) + '.'  END +  " +
                            "CASE WHEN [ValidUntilMonth] IS NULL THEN '' ELSE cast([ValidUntilMonth] as varchar) + '.'  END +  " +
                            "CASE WHEN [ValidUntilYear] IS NULL THEN '' ELSE cast([ValidUntilYear] as varchar)  END AS [Until_Date], " +
                            "CASE WHEN [RevisionLevel] IS NULL THEN '' ELSE [RevisionLevel] END AS [Level of Revision] " +
                            "FROM  Agent A, AgentNames('" + agentNameDisplayType + "') N " +
                            "WHERE A.AgentID = N.AgentID AND A.AgentID =  " + id.ToString();
                        this.getDataFromTable(sql, ref values);
                    }
                }

                if (id == -1)
                    sql = "SELECT NULL AS ParentName, NULL AS AddressType, NULL AS Country, NULL AS City, NULL AS PostalCode, " +
                        "NULL AS Streetaddress, NULL AS Address, NULL AS Telephone, NULL AS CellularPhone, NULL AS Telefax, NULL AS Email, NULL AS URI";
                //else if (this._ServerConnection.LinkedServer.Length == 0)
                //    SQL = "SELECT AgentID, DisplayOrder, ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, " +
                //        " Notes AS AddressNotes, ValidFrom, ValidUntil FROM dbo.AgentAddress(" + ID.ToString() + ")";
                else
                    sql = "SELECT AgentID, DisplayOrder, ParentName, AddressType, Country, City, PostalCode, Streetaddress, Address, Telephone, CellularPhone, Telefax, Email, URI, " +
                        " Notes AS AddressNotes, ValidFrom, ValidUntil FROM " + Prefix + "PublicContactInformation WHERE AgentID = " + id.ToString();
                this.getDataFromTable(sql, ref values);

                if (this._ServerConnection.LinkedServer.Length == 0)
                {
                    sql = "SELECT AgentName + '\r\n' AS SuperiorAgents FROM dbo.AgentSuperiorList (" + id.ToString() + ") WHERE AgentID <> " + id.ToString() + " ORDER BY DisplayOrder";
                    this.getDataFromTable(sql, ref values);
                }
                else
                {
                }

                sql = "SELECT A.AgentName AS [Parent AgentName]" +
                    //, "CASE WHEN A.AgentType = 'person' THEN CASE WHEN A.AgentTitle IS NULL THEN '' ELSE A.AgentTitle + ' ' END + A.GivenName + " +
                    //"CASE WHEN A.GivenNamePostfix IS NULL THEN '' ELSE ' ' + A.GivenNamePostfix END + ' ' + " +
                    //"CASE WHEN A.InheritedNamePrefix IS NULL THEN '' ELSE ' ' + A.InheritedNamePrefix END + " +
                    //"A.InheritedName + " +
                    //"CASE WHEN A.InheritedNamePostfix IS NULL THEN '' ELSE ' ' + A.InheritedNamePostfix END " +
                    //"ELSE '' END AS [Parent person name] " +
                    ", A.Abbreviation AS [Parent Abbreviation], A.AgentType AS [Parent type] " +
                    //", A.AgentRole AS [Parent role], A.Description AS [Parent description], A.Notes AS [Parent notes] " +
                    " FROM  " + Prefix + "Agent A " +
                    " INNER JOIN  " + Prefix + "Agent C ON C.AgentParentID = A.AgentID AND C.AgentID =  " + id.ToString() +
                    " CROSS JOIN " + Prefix + "ViewBaseURL B ";
                this.getDataFromTable(sql, ref values);

                // Keywords
                sql = "SELECT Keyword FROM " + Prefix + "AgentKeyword " +
                    "WHERE AgentID = " + id.ToString() +
                    "ORDER BY Keyword";
                this.getDataFromTable(sql, ref values);

                // External sources
                sql = "SELECT D.ExternalDatabaseName AS [External source] " +
                    "FROM  " + Prefix + "AgentExternalID AS A INNER JOIN " +
                    "" + Prefix + "AgentExternalDatabase AS D ON A.ExternalDatabaseID = D.ExternalDatabaseID " +
                    "WHERE A.AgentID = " + id.ToString() +
                    "ORDER BY D.ExternalDatabaseName";
                this.getDataFromTable(sql, ref values);

                // Projects
                sql = "SELECT P.Project FROM " + Prefix + "AgentProject A, " + Prefix + "ProjectProxy P " +
                    "WHERE P.ProjectID = A.ProjectID AND A.AgentID = " + id.ToString() +
                    "ORDER BY P.Project";
                this.getDataFromTable(sql, ref values);

                // Logo
                sql = "SELECT TOP 1 URI AS Logo FROM " + Prefix + "AgentImage " +
                    "WHERE( AgentID = " + id.ToString() + ") AND (Type = N'Logo') AND (DataWithholdingReason = '' OR DataWithholdingReason IS NULL) " +
                    "ORDER BY Sequence";
                this.getDataFromTable(sql, ref values);
            }
            if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
            this._UnitValues.Clear();
            foreach (System.Collections.Generic.KeyValuePair<string, string> P in values)
            {
                this._UnitValues.Add(P.Key, P.Value);
            }
            return values;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitResources(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT URI, [Description] FROM " + Prefix + "AgentImage WHERE AgentID = " + ID.ToString() + " AND (DataWithholdingReason = '' OR DataWithholdingReason IS NULL) ";
                this.getResources(SQL, ref Values);
            }
            return Values;
        }

        public string MainTable() { return "Agent"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[8];
            QueryDisplayColumns[0].DisplayText = "Agent Name";
            QueryDisplayColumns[0].DisplayColumn = "DisplayText"; // "AgentName";
            QueryDisplayColumns[0].OrderColumn = "AgentName";

            QueryDisplayColumns[1].DisplayColumn = "Abbreviation";

            QueryDisplayColumns[2].DisplayText = "Type";
            QueryDisplayColumns[2].DisplayColumn = "AgentType";

            QueryDisplayColumns[3].DisplayColumn = "Country";
            QueryDisplayColumns[3].TableName = "ViewAgentAddress";

            QueryDisplayColumns[4].DisplayColumn = "City";
            QueryDisplayColumns[4].TableName = "ViewAgentAddress";

            QueryDisplayColumns[5].DisplayColumn = "Identifier";
            QueryDisplayColumns[5].TableName = "AgentIdentifier";

            QueryDisplayColumns[6].DisplayText = "Valid from";
            QueryDisplayColumns[6].DisplayColumn = "ValidFrom";

            QueryDisplayColumns[7].DisplayText = "Valid until";
            QueryDisplayColumns[7].DisplayColumn = "ValidUntil";

            for (int i = 0; i < QueryDisplayColumns.Length; i++)
            {
                if (QueryDisplayColumns[i].DisplayText == null)
                    QueryDisplayColumns[i].DisplayText = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].OrderColumn == null)
                    QueryDisplayColumns[i].OrderColumn = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].IdentityColumn == null)
                    QueryDisplayColumns[i].IdentityColumn = "AgentID";
                if (QueryDisplayColumns[i].TableName == null)
                    QueryDisplayColumns[i].TableName = "Agent_Core";
                if (QueryDisplayColumns[i].TipText == null)
                    QueryDisplayColumns[i].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(QueryDisplayColumns[i].TableName, QueryDisplayColumns[i].DisplayColumn);
                QueryDisplayColumns[i].Module = "DiversityAgents";
            }

            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.UserControls.QueryDisplayColumn> QueryDisplayColumnList()
        {
            System.Collections.Generic.List<DiversityWorkbench.UserControls.QueryDisplayColumn> Q = new List<UserControls.QueryDisplayColumn>();

            DiversityWorkbench.UserControls.QueryDisplayColumn Q1 = new UserControls.QueryDisplayColumn();
            Q1.DisplayText = "Agent Name";
            Q1.DisplayColumn = "DisplayText"; // "AgentName";
            Q1.OrderColumn = "AgentName";
            Q1.IdentityColumn = "AgentID";
            Q1.TableName = "Agent_Core";
            Q.Add(Q1);

            return Q;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            //string Prefix = "";
            //if (this._ServerConnection.LinkedServer.Length > 0)
            //    Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            //else Prefix = "dbo.";

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            #region PROJECT

            System.Data.DataTable dtProject = new System.Data.DataTable();
            string SQL = "SELECT ProjectID AS [Value], Project AS Display " +
                "FROM " + Prefix + "ProjectList " +
                "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtProject); }
                catch { }
            }
            if (dtProject.Rows.Count > 1)
            {
                dtProject.Clear();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ProjectID AS [Value], Project AS Display " +
                    "FROM " + Prefix + "ProjectList " +
                    "ORDER BY Display";
                a.SelectCommand.CommandText = SQL;
                try { a.Fill(dtProject); }
                catch { }
            }
            if (dtProject.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtProject.Columns.Add(Value);
                dtProject.Columns.Add(Display);
            }
            string Description = DiversityWorkbench.Functions.ColumnDescription("ProjectProxy", "Project");
            this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "AgentProject", "AgentID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
            QueryConditions.Add(this._QueryConditionProject);

            #endregion

            #region Agent

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "AgentName");
            DiversityWorkbench.QueryCondition qAgentName = new DiversityWorkbench.QueryCondition(true, "Agent", "AgentID", "AgentName", "Agent", "Agent", "Agent Name", Description);
            QueryConditions.Add(qAgentName);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "AgentID");
            DiversityWorkbench.QueryCondition qAgentID = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "AgentID", "Agent", "ID", "ID of the Agent", Description);
            QueryConditions.Add(qAgentID);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "Abbreviation");
            DiversityWorkbench.QueryCondition qAbbreviation = new DiversityWorkbench.QueryCondition(true, "Agent", "AgentID", "Abbreviation", "Agent", "Abbr.", "Abbreviation", Description);
            QueryConditions.Add(qAbbreviation);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "AgentTitle");
            DiversityWorkbench.QueryCondition qAgentTitle = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "AgentTitle", "Agent", "Title", "Title", Description);
            QueryConditions.Add(qAgentTitle);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "GivenName");
            DiversityWorkbench.QueryCondition qGivenName = new DiversityWorkbench.QueryCondition(true, "Agent", "AgentID", "GivenName", "Agent", "First name", "First name", Description);
            QueryConditions.Add(qGivenName);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "GivenNamePostfix");
            DiversityWorkbench.QueryCondition qGivenNamePostfix = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "GivenNamePostfix", "Agent", "Postfix", "Postfix", Description);
            QueryConditions.Add(qGivenNamePostfix);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "InheritedNamePrefix");
            DiversityWorkbench.QueryCondition qInheritedNamePrefix = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "InheritedNamePrefix", "Agent", "Prefix", "Prefix", Description);
            QueryConditions.Add(qInheritedNamePrefix);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "InheritedName");
            DiversityWorkbench.QueryCondition qInheritedName = new DiversityWorkbench.QueryCondition(true, "Agent", "AgentID", "InheritedName", "Agent", "Last name", "Last name", Description);
            QueryConditions.Add(qInheritedName);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "InheritedNamePostfix");
            DiversityWorkbench.QueryCondition qInheritedNamePostfix = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "InheritedNamePostfix", "Agent", "L.n.Post.", "Last name Postfix", Description);
            QueryConditions.Add(qInheritedNamePostfix);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "AgentRole");
            DiversityWorkbench.QueryCondition qAgentRole = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "AgentRole", "Agent", "Role", "Role", Description);
            QueryConditions.Add(qAgentRole);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "Description");
            DiversityWorkbench.QueryCondition qDescription = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "Description", "Agent", "Descript.", "Description", Description);
            QueryConditions.Add(qDescription);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "OriginalSpelling");
            DiversityWorkbench.QueryCondition qOriginalSpelling = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "OriginalSpelling", "Agent", "Ori.spell.", "Original spelling", Description);
            QueryConditions.Add(qOriginalSpelling);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "AgentGender");
            DiversityWorkbench.QueryCondition qAgentGender = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "AgentGender", "Agent", "Sex", "Sex of agent", Description);
            QueryConditions.Add(qAgentGender);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "Notes");
            DiversityWorkbench.QueryCondition qNotes = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "Notes", "Agent", "Notes", "Notes", Description);
            QueryConditions.Add(qNotes);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "DataWithholdingReason");
            DiversityWorkbench.QueryCondition qDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "DataWithholdingReason", "Agent", "Withhold", "Data withholding reason", Description);
            QueryConditions.Add(qDataWithholdingReason);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "RevisionLevel");
            DiversityWorkbench.QueryCondition qRevisionLevel = new DiversityWorkbench.QueryCondition(true, "Agent", "AgentID", "RevisionLevel", "Agent", "Revision", "Revision level", Description);
            QueryConditions.Add(qRevisionLevel);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "AgentType");
            string DB = "DiversityAgents";
            if (this._ServerConnection.DatabaseName.Length > 0) DB = this._ServerConnection.DatabaseName;
            DiversityWorkbench.QueryCondition qAgentType = new DiversityWorkbench.QueryCondition(true, "Agent", "AgentID", "AgentType", "Agent", "Type", "Agent type", Description, "AgentType_Enum", DB);
            QueryConditions.Add(qAgentType);

            System.Data.DataTable dtUser = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT LoginName, CombinedNameCache " +
                "FROM         " + Prefix + "UserProxy " +
                "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter aUser = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { aUser.Fill(dtUser); }
                catch { }
            }
            //Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "LogCreatedBy");
            //DiversityWorkbench.QueryCondition qLogCreatedBy = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "LogCreatedBy", "Agent", "Creat. by", "The user that created the dataset", Description, dtUser, false);
            //QueryConditions.Add(qLogCreatedBy);

            //Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "LogCreatedWhen");
            //DiversityWorkbench.QueryCondition qLogCreatedWhen = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "LogCreatedWhen", "Agent", "Creat. date", "The date when the dataset was created", Description, true);
            //QueryConditions.Add(qLogCreatedWhen);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "ValidFromDate");
            DiversityWorkbench.QueryCondition qValidFromDate = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "ValidFromDate", "Agent", "Birth date", "Birth date", Description, "ValidFromDay", "ValidFromMonth", "ValidFromYear");
            QueryConditions.Add(qValidFromDate);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "ValidUntilDate");
            DiversityWorkbench.QueryCondition qValidUntilDate = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "ValidUntilDate", "Agent", "Death date", "Date of death", Description, "ValidUntilDay", "ValidUntilMonth", "ValidUntilYear");
            QueryConditions.Add(qValidUntilDate);


            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "LogUpdatedBy");
            DiversityWorkbench.QueryCondition qLogUpdatedBy = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "LogUpdatedBy", "Agent", "Changed by", "The last user that changed the dataset", Description, dtUser, false);
            QueryConditions.Add(qLogUpdatedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("Agent", "LogUpdatedWhen");
            DiversityWorkbench.QueryCondition qLogUpdatedWhen = new DiversityWorkbench.QueryCondition(false, "Agent", "AgentID", "LogUpdatedWhen", "Agent", "Changed at", "The last date when the dataset was changed", Description, true);
            QueryConditions.Add(qLogUpdatedWhen);

            #endregion

            #region Addresses

            Description = "If any contact information is present";
            DiversityWorkbench.QueryCondition qContact = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "Contact", "Presence", "Contact present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qContact);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "Country");
            DiversityWorkbench.QueryCondition qCountry = new DiversityWorkbench.QueryCondition(true, "AgentContactInformation", "AgentID", "Country", "Contact", "Country", "Country", Description);
            QueryConditions.Add(qCountry);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "City");
            DiversityWorkbench.QueryCondition qCity = new DiversityWorkbench.QueryCondition(true, "AgentContactInformation", "AgentID", "City", "Contact", "City", "City", Description);
            QueryConditions.Add(qCity);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "PostalCode");
            DiversityWorkbench.QueryCondition qPostalCode = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "PostalCode", "Contact", "Code", "Postal code", Description);
            QueryConditions.Add(qPostalCode);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "Streetaddress");
            DiversityWorkbench.QueryCondition qStreetaddress = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "Streetaddress", "Contact", "Street", "Street address", Description);
            QueryConditions.Add(qStreetaddress);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "Address");
            DiversityWorkbench.QueryCondition qAddress = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "Address", "Contact", "Address", "Address", Description);
            QueryConditions.Add(qAddress);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "Telephone");
            DiversityWorkbench.QueryCondition qTelephone = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "Telephone", "Contact", "Telephone", "Telephone", Description);
            QueryConditions.Add(qTelephone);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "CellularPhone");
            DiversityWorkbench.QueryCondition qCellularPhone = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "CellularPhone", "Contact", "Cell.phone", "Cellular phone", Description);
            QueryConditions.Add(qCellularPhone);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "Telefax");
            DiversityWorkbench.QueryCondition qTelefax = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "Telefax", "Contact", "Telefax", "Telefax", Description);
            QueryConditions.Add(qTelefax);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "Email");
            DiversityWorkbench.QueryCondition qEmail = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "Email", "Contact", "Email", "Email", Description);
            QueryConditions.Add(qEmail);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "URI");
            DiversityWorkbench.QueryCondition qURI = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "URI", "Contact", "URI", "URI", Description);
            QueryConditions.Add(qURI);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentContactInformation", "Notes");
            DiversityWorkbench.QueryCondition qContactNotes = new DiversityWorkbench.QueryCondition(false, "AgentContactInformation", "AgentID", "Notes", "Contact", "Notes", "Notes", Description);
            QueryConditions.Add(qContactNotes);

            #endregion

            #region Image

            Description = "If any image is present";
            DiversityWorkbench.QueryCondition qImagePresent = new DiversityWorkbench.QueryCondition(false, "AgentImage", "AgentID", "Image", "Presence", "Image present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qImagePresent);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentImage", "URI");
            DiversityWorkbench.QueryCondition qImageUri = new DiversityWorkbench.QueryCondition(false, "AgentImage", "AgentID", "URI", "Image", "Image", "Image", Description);
            QueryConditions.Add(qImageUri);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentImage", "Description");
            DiversityWorkbench.QueryCondition qImageDescription = new DiversityWorkbench.QueryCondition(false, "AgentImage", "AgentID", "Description", "Image", "Description", "Description", Description);
            QueryConditions.Add(qImageDescription);

            #endregion

            #region Keyword

            Description = "If any keyword is present";
            DiversityWorkbench.QueryCondition qKeywordPresent = new DiversityWorkbench.QueryCondition(false, "AgentKeyword", "AgentID", "Keyword", "Presence", "Keyword present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qKeywordPresent);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentKeyword", "Keyword");
            DiversityWorkbench.QueryCondition qKeyword = new DiversityWorkbench.QueryCondition(false, "AgentKeyword", "AgentID", "Keyword", "Keyword", "Keyword", "Keyword", Description);
            QueryConditions.Add(qKeyword);

            #endregion

            #region Reference

            Description = "If any reference is present";
            DiversityWorkbench.QueryCondition qReferencePresent = new DiversityWorkbench.QueryCondition(false, "AgentReference", "AgentID", "Reference", "Presence", "Reference present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qReferencePresent);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentReference", "ReferenceTitle");
            DiversityWorkbench.QueryCondition qReferenceTitle = new DiversityWorkbench.QueryCondition(false, "AgentReference", "AgentID", "ReferenceTitle", "Reference", "Title", "Reference title", Description);
            QueryConditions.Add(qReferenceTitle);

            #endregion

            #region Relation

            Description = "If any relation is present";
            DiversityWorkbench.QueryCondition qRelationPresent = new DiversityWorkbench.QueryCondition(false, "AgentRelation", "AgentID", "Relation", "Presence", "Relation present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qRelationPresent);

            #endregion

            #region Identifier

            Description = "If any identifier is present";
            DiversityWorkbench.QueryCondition qIdentifierPresent = new DiversityWorkbench.QueryCondition(false, "AgentIdentifier", "AgentID", "Identifier", "Presence", "Identifier present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qIdentifierPresent);

            Description = DiversityWorkbench.Functions.ColumnDescription("AgentIdentifier", "Identifier");
            DiversityWorkbench.QueryCondition qIdentifier = new DiversityWorkbench.QueryCondition(false, "AgentIdentifier", "AgentID", "Identifier", "Identifier", "Identifier", "Identifier", Description);
            QueryConditions.Add(qIdentifier);

            #endregion

            return QueryConditions;
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList = new List<QueryCondition>();
            if (this._QueryConditionProject.Column == null)
            {
                System.Data.DataTable dtProject = new System.Data.DataTable();
                string SQL = "SELECT ProjectID AS [Value], Project AS Display " +
                    "FROM ProjectList " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { a.Fill(dtProject); }
                    catch { }
                }
                if (dtProject.Rows.Count > 1)
                {
                    dtProject.Clear();
                    SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ProjectID AS [Value], Project AS Display " +
                        "FROM ProjectList " +
                        "ORDER BY Display";
                    a.SelectCommand.CommandText = SQL;
                    try { a.Fill(dtProject); }
                    catch { }
                }
                if (dtProject.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtProject.Columns.Add(Value);
                    dtProject.Columns.Add(Display);
                }
                string Description = DiversityWorkbench.Functions.ColumnDescription("ProjectProxy", "Project");
                this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "AgentProject", "AgentID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
            }
            PredefinedQueryPersistentConditionList.Add(this._QueryConditionProject);
            return PredefinedQueryPersistentConditionList;
        }

        #endregion

        #region Agent name according to display type

        //public System.Data.DataTable AgentNameDisplayTypes()
        //{
        //    string SQL = "SELECT Code, Description, DisplayText " +
        //        "FROM DiversityAgents.dbo.AgentNameDisplayType_Enum " +
        //        "WHERE DisplayEnable = 1 " +
        //        "ORDER BY DisplayText";
        //    System.Data.DataTable dtTypes = new System.Data.DataTable();
        //    try
        //    {
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
        //        con.Open();
        //        ad.Fill(dtTypes);
        //        con.Close();
        //    }
        //    catch { }
        //    return dtTypes;
        //}

        public System.Data.DataTable AgentNameDisplayTypes(bool IncludeNull)
        {
            string SQL = "";
            if (IncludeNull)
                SQL = "SELECT NULL AS Code, NULL AS Description, NULL AS DisplayText UNION ";
            SQL += "SELECT Code, Description, DisplayText " +
                "FROM AgentNameDisplayType_Enum " +
                "WHERE DisplayEnable = 1 " +
                "ORDER BY DisplayText";
            System.Data.DataTable dtTypes = new System.Data.DataTable();
            try
            {
                string ConStr = this._ServerConnection.ConnectionString;
                if (ConStr.Length > 0)
                {
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConStr);
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
                    con.Open();
                    ad.Fill(dtTypes);
                    con.Close();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return dtTypes;
        }

        //public System.Collections.Generic.Dictionary<string, string> AgentNameDisplayTypes()
        //{
        //    System.Collections.Generic.Dictionary<string, string> DisplayTypes = new Dictionary<string, string>();
        //    string SQL = "SELECT Code, Description " +
        //        "FROM DiversityAgents.dbo.AgentNameDisplayType_Enum " +
        //        "ORDER BY Description";
        //    System.Data.DataTable dtTypes = new System.Data.DataTable();
        //    try
        //    {
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
        //        con.Open();
        //        ad.Fill(dtTypes);
        //        con.Close();
        //        foreach (System.Data.DataRow R in dtTypes.Rows)
        //            DisplayTypes.Add(R["Code"].ToString(), R["Description"].ToString());
        //    }
        //    catch { }
        //    return DisplayTypes;
        //}

        public string AgentName(string uri, string displayType)
        {
            string agent = "";
            const string sql = "SELECT dbo.BaseURL()";
            string connectionString = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> kv in this.ServerConnectionList())
            {
                if (kv.Value != null)
                {
                    if (uri.StartsWith(kv.Value.BaseURL))
                    {
                        connectionString = kv.Value.ConnectionString;
                        break;
                    }
                }
            }
            if (connectionString.Length > 0)
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
                try
                {
                    con.Open();
                    Microsoft.Data.SqlClient.SqlCommand c = new Microsoft.Data.SqlClient.SqlCommand(sql, con);
                    string baseUrl = c.ExecuteScalar()?.ToString() ?? string.Empty;
                    if (int.TryParse(uri.Substring(baseUrl.Length), out var id)) {
                        c.CommandText = "SELECT AgentName FROM dbo.AgentNames('" + displayType + "') WHERE AgentID = " +
                                        id.ToString();
                        agent = c.ExecuteScalar()?.ToString() ?? string.Empty;
                    }
                    con.Close();
                }
                catch
                {
                    // ignored
                    con.Close();
                }
            }
            return agent;
        }

        public string AgentName(int ID, string DisplayType)
        {
            string Agent = "";
            string SQL = "SELECT AgentName FROM dbo.AgentNames('" + DisplayType + "') WHERE AgentID = " + ID.ToString();
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
            try
            {
                con.Open();
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                Agent = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
            }
            catch { }
            return Agent;
        }

        #endregion

        #region Properties

        //public override DiversityWorkbench.ServerConnection ServerConnection
        //{
        //    get { return _ServerConnection; }
        //    set 
        //    {
        //        if (value != null)
        //            this._ServerConnection = value;
        //        else
        //        {
        //            this._ServerConnection = new ServerConnection();
        //            this._ServerConnection.DatabaseServer = "127.0.0.1";
        //            this._ServerConnection.IsTrustedConnection = true;
        //            this._ServerConnection.DatabaseName = "DiversityAgents";
        //        }
        //        this._ServerConnection.DatabaseName = "DiversityAgents";
        //        this._ServerConnection.ModuleName = "DiversityAgents";
        //    }
        //}

        //public override System.Collections.Generic.List<string> DatabaseList()
        //{
        //    if (this._DatabaseList == null)
        //    {
        //        this._DatabaseList = new List<string>();
        //        this._DatabaseList.Add("DiversityAgents");
        //    }
        //    return this._DatabaseList;
        //}
        #endregion    

        #region static functions

        /// <summary>
        /// Getting the agents underneath a given agent including the given agent
        /// </summary>
        /// <param name="URL">The URL of the agent (= BaseURL + AgentID)</param>
        /// <returns>A dictionary containing the URLs and the names</returns>
        public static System.Collections.Generic.Dictionary<string, string> SubAgent(string URL)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartAgent(URL);
            getSubAgents(ref dt);
            getAgents(ref DD, dt);
            return DD;
        }

        /// <summary>
        /// Getting the synonyms of a given agent
        /// </summary>
        /// <param name="URL">The URL of the agent (= BaseURL + AgentID)</param>
        /// <returns>A dictionary containing the URLs and the names</returns>
        public static System.Collections.Generic.Dictionary<string, string> Synonyms(string URL)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartAgent(URL);
            getSynonyms(ref dt);
            getAgents(ref DD, dt);
            return DD;
        }

        /// <summary>
        /// Getting the agents underneath a given agent including the given agent and all synonyms
        /// </summary>
        /// <param name="URL">The URL of the agent (= BaseURL + AgentID)</param>
        /// <returns>A dictionary containing the URLs and the names</returns>
        public static System.Collections.Generic.Dictionary<string, string> SubAgentSynonyms(string URL)
        {
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartAgent(URL);
            getSubAgents(ref dt);
            getSynonyms(ref dt);
            getAgents(ref DD, dt);
            return DD;
        }

        public static string AcceptedName(string URL, ref string UrlAcceptedName)
        {
            string AcceptedName = "";
            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            System.Data.DataTable dt = getStartAgent(URL);
            AcceptedName = getAcceptedName(ref dt, ref UrlAcceptedName);
            return AcceptedName;
        }

        private static string getAcceptedName(ref System.Data.DataTable DT, ref string UrlAcceptedName)
        {
            string ValidAgentName = "";
            try
            {
                if (_SC != null)
                {
                    getSynonyms(ref DT, true);
                    string IDs = "";
                    foreach (System.Data.DataRow R in DT.Rows)
                    {
                        if (IDs.Length > 0) IDs += ", ";
                        IDs += R[0].ToString();
                    }
                    // getting the names that are accepted names in the table
                    string SQL = "SELECT [AgentName], A.AgentID, SynonymToAgentID FROM " + _SC.Prefix() + "Agent A  WHERE A.AgentID IN (" + IDs + ") OR SynonymToAgentID IN (" + IDs + ") ORDER BY SynonymToAgentID ASC";

                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                    System.Data.DataTable dt = new System.Data.DataTable();
                    ad.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        ValidAgentName = dt.Rows[0][0].ToString();
                        UrlAcceptedName = _SC.BaseURL + dt.Rows[0][1].ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return ValidAgentName;
        }



        #region Merge agent

        public static bool MergeAgent(
            ref string Uri,
            ref string AgentDisplayText,
            DiversityWorkbench.ServerConnection SC,
            System.Collections.Generic.Dictionary<string, string> Values,
            System.Collections.Generic.List<string> DecisiveValues,
            int ProjectID)
        {
            bool OK = true;

            System.Collections.Generic.Dictionary<string, string> DD = new Dictionary<string, string>();
            foreach (string D in DecisiveValues)
                DD.Add(D, Values[D]);
            int? AgentID = null;
            try
            {
                AgentID = DiversityWorkbench.Agent.AgentID(SC, DD);
            }
            catch (System.Exception ex) { }
            if (AgentID != null)
            {
                string Prefix = DiversityWorkbench.Agent.TablePrefix(SC);
                string SQL = "SELECT BaseURL + CAST(" + AgentID.ToString() + " AS varchar) AS URI FROM " + Prefix + "ViewBaseURL";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionString);
                con.Open();
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                Uri = C.ExecuteScalar()?.ToString() ?? string.Empty;
                C.CommandText = "SELECT AgentName FROM " + Prefix + "ViewAgentNames WHERE AgentID = " + AgentID.ToString();
                AgentDisplayText = C.ExecuteScalar()?.ToString() ?? string.Empty;
                con.Close();
                con.Dispose();
            }
            else
            {
                OK = InsertAgent(ref Uri, ref AgentDisplayText, SC, Values);
            }

            return OK;
        }

        private static string TablePrefix(DiversityWorkbench.ServerConnection SC)
        {
            string Prefix = "[dbo].";
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].[dbo].";
            return Prefix;
        }

        private static int? AgentID(DiversityWorkbench.ServerConnection SC,
            System.Collections.Generic.Dictionary<string, string> Values)
        {
            int? ID = null;
            // getting the involved tables
            System.Collections.Generic.List<string> PotentialTables = new List<string>();
            PotentialTables.Add("Agent");
            PotentialTables.Add("AgentContactInformation");
            PotentialTables.Add("AgentIdentifier");
            System.Collections.Generic.Dictionary<string, string> Tables = new Dictionary<string, string>();
            string SQL = "SELECT T.AgentID FROM ";
            string FromClause = "";
            string WhereClause = "";
            string Prefix = DiversityWorkbench.Agent.TablePrefix(SC);
            foreach (string T in PotentialTables) // testing every potential table
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Values) // Checking the values
                {
                    System.Collections.Generic.List<string> L = AgentTableColumns(SC, T); // getting the columns of the potential tables
                    foreach (string C in L)
                    {
                        if (Values.ContainsKey(C) && L.Contains(KV.Key))
                        {
                            string Alias = "T";
                            if (!Tables.ContainsKey(T))
                            {
                                if (Tables.Count > 0)
                                    Alias += Tables.Count.ToString();
                                Tables.Add(T, Alias);
                            }
                            string Restriction = Alias + "." + KV.Key + " = '" + KV.Value + "' ";
                            if (WhereClause.IndexOf(Restriction) == -1)
                            {
                                if (WhereClause.Length > 0)
                                    WhereClause += " AND ";
                                WhereClause += Restriction;
                            }
                        }
                    }
                }
            }

            int i = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Tables)
            {
                if (FromClause.Length > 0)
                    FromClause += ", ";
                FromClause += Prefix + KV.Key + " AS " + KV.Value;
                if (i > 0)
                {
                    if (WhereClause.Length > 0)
                        WhereClause += " AND ";
                    WhereClause += " T.AgentID = T" + i.ToString() + ".AgentID ";
                }
                i++;
            }
            SQL += FromClause + " WHERE " + WhereClause;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand Com = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            int AgentID;
            try
            {
                if (int.TryParse(Com.ExecuteScalar().ToString(), out AgentID))
                    ID = AgentID;
            }
            catch { }
            con.Close();
            con.Dispose();
            return ID;
        }

        private static bool InsertAgent(
            ref string Uri,
            ref string AgentDisplayText,
            DiversityWorkbench.ServerConnection SC,
            System.Collections.Generic.Dictionary<string, string> Values)
        {
            bool OK = true;

            try
            {
                System.Collections.Generic.List<string> AdditionalTables = new List<string>();
                string Prefix = DiversityWorkbench.Agent.TablePrefix(SC);

                int AgentID;
                string SQL = "DECLARE @T Table(ID int); " + DiversityWorkbench.Agent.SqlAgentInsert(SC, "Agent", Values, "OUTPUT INSERTED.AgentID INTO @T (ID)") + "; SELECT TOP 1 ID FROM @T";
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(SC.ConnectionString);
                con.Open();
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                if (int.TryParse(C.ExecuteScalar()?.ToString(), out AgentID))
                {
                    SQL = "SELECT SCOPE_IDENTITY()";
                    C.CommandText = SQL;
                    int ID = 0;
                    int.TryParse(C.ExecuteScalar()?.ToString(), out ID);
                    if (ID >= AgentID)
                        AgentID = ID;
                    SQL = "SELECT BaseURL + CAST(" + AgentID.ToString() + " AS varchar) AS URI FROM " + Prefix + "ViewBaseURL";
                    C.CommandText = SQL;
                    Uri = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    C.CommandText = "SELECT AgentName FROM " + Prefix + "ViewAgentNames WHERE AgentID = " + AgentID.ToString();
                    AgentDisplayText = C.ExecuteScalar()?.ToString() ?? string.Empty;

                    // getting the involved tables
                    System.Collections.Generic.List<string> PotentialTables = new List<string>();
                    PotentialTables.Add("AgentContactInformation");
                    PotentialTables.Add("AgentIdentifier");
                    PotentialTables.Add("AgentProject");
                    foreach (string T in PotentialTables) // testing every potential table
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Values) // Checking the values
                        {
                            System.Collections.Generic.List<string> L = AgentTableColumns(SC, T); // getting the columns of the potential tables
                            foreach (string Col in L)
                            {
                                if (Values.ContainsKey(Col))
                                {
                                    if (!AdditionalTables.Contains(T))
                                    {
                                        AdditionalTables.Add(T);
                                    }
                                }
                            }
                        }
                    }
                    Values.Add("AgentID", AgentID.ToString());
                    foreach (string T in AdditionalTables)
                    {
                        if (T == "AgentContactInformation")
                        {
                            Values.Add("DisplayOrder", "1");
                            Values.Add("AddressType", "office");
                        }
                        SQL = DiversityWorkbench.Agent.SqlAgentInsert(SC, T, Values, "");
                        C.CommandText = SQL;
                        int Result = C.ExecuteNonQuery();
                        //if (Result != 1)
                        //    OK = false;
                    }
                }
                else
                    OK = false;
                con.Close();
                con.Dispose();
            }
            catch (Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return OK;
        }

        private static string SqlAgentInsert(DiversityWorkbench.ServerConnection SC,
            string TableName,
            System.Collections.Generic.Dictionary<string, string> Values,
            string OutputClause)
        {
            string SQL = "";
            try
            {
                string SqlColumns = "";
                string SqlValues = "";
                System.Collections.Generic.List<string> L = AgentTableColumns(SC, TableName);
                foreach (string C in L)
                {
                    if (Values.ContainsKey(C) && Values[C] != null && Values[C].Length > 0)
                    {
                        if (SqlColumns.Length > 0)
                            SqlColumns += ", ";
                        SqlColumns += C;
                        if (SqlValues.Length > 0)
                            SqlValues += ", ";
                        SqlValues += "'" + Values[C].Replace("'", "''") + "'";
                    }
                    //else if (Values.ContainsKey(C) && Values[C] == null)
                    //{

                    //}
                    //else
                    //{

                    //}
                }
                SQL = "INSERT INTO " + DiversityWorkbench.Agent.TablePrefix(SC) + TableName + " (" + SqlColumns + ") ";
                if (OutputClause.Length > 0)
                    SQL += OutputClause;
                SQL += " VALUES (" + SqlValues + ")";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return SQL;
        }

        private static System.Collections.Generic.List<string> AgentTableColumns(DiversityWorkbench.ServerConnection SC,
            string TableName)
        {
            System.Collections.Generic.List<string> Columns = new List<string>();
            try
            {
                string Prefix = DiversityWorkbench.Agent.TablePrefix(SC).Replace("[dbo].", "");
                string SQL = "select c.COLUMN_NAME from " + Prefix + "INFORMATION_SCHEMA.COLUMNS C where c.TABLE_NAME = '" + TableName + "'";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                    Columns.Add(R[0].ToString());
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Columns;
        }

        #endregion

        #region Auxillary

        private static int? _ProjectID;
        private static int ProjectID()
        {
            if (_ProjectID == null)
            {
                string SQL = "SELECT ProjectID, Project FROM " + _SC.Prefix() + "ProjectList ORDER BY Project";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                System.Data.DataTable dt = new System.Data.DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count > 1)
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Project", "Please select a project");
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        int i;
                        if (int.TryParse(f.SelectedValue, out i))
                            _ProjectID = i;
                    }
                }
                else
                    _ProjectID = int.Parse(dt.Rows[0][0].ToString());
            }
            return (int)_ProjectID;
        }

        private static int ProjectID(string IDs)
        {
            if (_ProjectID == null)
            {
                string SQL = "SELECT DISTINCT ProjectID FROM" + _SC.Prefix() + "AgentProject P WHERE AgentID IN (" + IDs + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                System.Data.DataTable dt = new System.Data.DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                    _ProjectID = int.Parse(dt.Rows[0][0].ToString());
                else
                {
                    dt = new System.Data.DataTable();
                    SQL = "SELECT ProjectID, Project FROM " + _SC.Prefix() + "ProjectList ORDER BY Project";
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dt);
                    if (dt.Rows.Count > 1)
                    {
                        DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Project", "Please select a project");
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            int i;
                            if (int.TryParse(f.SelectedValue, out i))
                                _ProjectID = i;
                        }
                    }
                    else
                        _ProjectID = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return (int)_ProjectID;
        }

        private static DiversityWorkbench.ServerConnection _SC;

        private static System.Data.DataTable getStartAgent(string URL)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                // resetting the project
                _ProjectID = null;
                // getting the server connection for the URL
                setServerConnection(URL);
                // Inserting the ID to start the query
                string ID = DiversityWorkbench.WorkbenchUnit.getIDFromURI(URL);
                if (ID != null && ID.Length > 0 && _SC != null)
                {
                    string SQL = "SELECT AgentID FROM " + _SC.Prefix() + "Agent N WHERE AgentID = " + ID + " " +
                        "AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL)";
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                    ad.Fill(dt);
                }
            }
            catch (System.Exception ex)
            {
            }
            return dt;
        }

        public static void setServerConnection(string URL)
        {
            // getting the server connection for the URL
            _SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URL);
        }

        public static void getAgents(ref System.Collections.Generic.Dictionary<string, string> DD, System.Data.DataTable DT)
        {
            string SQL = "";
            try
            {
                foreach (System.Data.DataRow R in DT.Rows)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += R[0].ToString();
                }
                SQL = "SELECT U.BaseURL + cast(T.AgentID as varchar) AS URL, T.AgentName AS Agent FROM " + _SC.Prefix() + "Agent T, " + _SC.Prefix() + "ViewBaseURL U WHERE T.AgentID IN (" + SQL + ")";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                System.Data.DataTable dt = new System.Data.DataTable();
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    if (!DD.ContainsKey(R[0].ToString()))
                        DD.Add(R[0].ToString(), R[1].ToString());
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private static void getSubAgents(ref System.Data.DataTable DT)
        {
            string IDs = "";
            foreach (System.Data.DataRow R in DT.Rows)
            {
                if (IDs.Length > 0) IDs += ", ";
                IDs += R[0].ToString();
            }
            string SQL = "SELECT H.AgentID FROM " + _SC.Prefix() + "Agent H, " + _SC.Prefix() + "Agent N WHERE H.AgentID <> N.AgentID " +
                " AND H.AgentParentID IN ( " + IDs + ") " +
                " AND H.ProjectID = " + ProjectID(IDs) + " AND H.IgnoreButKeepForReference = 0 AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
                " AND H.AgentID NOT IN (" + IDs + ")";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
            System.Data.DataTable dt = new System.Data.DataTable();
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                ad.Fill(DT);
                getSubAgents(ref DT);
            }
        }

        private static void getSynonyms(ref System.Data.DataTable DT, bool RestrictProjects = false)
        {
            try
            {
                if (_SC == null)
                    return;

                string IDs = "";
                foreach (System.Data.DataRow R in DT.Rows)
                {
                    if (IDs.Length > 0) IDs += ", ";
                    IDs += R[0].ToString();
                }
                //string sProjectID = "";
                //sProjectID = ProjectID(IDs).ToString();
                string SQL = "SELECT S.AgentID FROM " + _SC.Prefix() + "Agent S, " + _SC.Prefix() + "Agent N WHERE S.AgentID = N.SynonymToAgentID " +
                    " AND N.AgentID IN ( " + IDs + ") " +
                    " OR S.AgentID IN ( " + IDs + ") " +
                    " GROUP BY S.AgentID";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, _SC.ConnectionString);
                ad.Fill(DT);
                //System.Data.DataTable dt = new System.Data.DataTable();
                //ad.Fill(dt);
                //if (dt.Rows.Count > 0)
                //{
                //    getSynonyms(ref DT);
                //}
                //// getting the synonyms of the names in the table
                //dt.Clear();
                //ad.SelectCommand.CommandText = "SELECT S.SynonymToAgentID AS AgentID FROM " + _SC.Prefix() + "Agent S, " + _SC.Prefix() + "Agent N WHERE S.AgentID <> N.AgentID " +
                //    " AND S.AgentID IN ( " + IDs + ") " +
                //    " AND S.IgnoreButKeepForReference = 0 AND (N.IgnoreButKeepForReference = 0 OR N.IgnoreButKeepForReference IS NULL) " +
                //    " AND S.SynonymToAgentID NOT IN (" + IDs + ")";
                //ad.Fill(dt);
                //if (dt.Rows.Count > 0)
                //{
                //    ad.Fill(DT);
                //    getSynonyms(ref DT, RestrictProjects);
                //}
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #endregion

        #region Backlinks

        public override System.Windows.Forms.ImageList BackLinkImages(ModuleType CallingModule)
        {
            if (this._BackLinkImages == null)
            {
                this._BackLinkImages = this.BackLinkImages();
            }
            switch (CallingModule)
            {
                case ModuleType.References:
                    this._BackLinkImages.Images.Add("AgentReference", DiversityWorkbench.Properties.Resources.Agent);
                    break;
                case ModuleType.Projects:
                    this._BackLinkImages.Images.Add("ProjectProxy", DiversityWorkbench.Properties.Resources.Project);
                    break;
            }
            return this._BackLinkImages;
        }


        public override System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BackLinkServerConnectionDomains(string URI, ModuleType CallingModule, bool IncludeEmpty = false, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BLD = new Dictionary<ServerConnection, List<BackLinkDomain>>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this.BackLinkConnections(ModuleType.Agents))
            {
                switch (CallingModule)
                {
                    case ModuleType.References:
                        System.Collections.Generic.List<BackLinkDomain> _L = this.BackLinkDomainReferences(KV.Value, URI);
                        if (_L.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _L);
                        break;
                    case ModuleType.Projects:
                        System.Collections.Generic.List<BackLinkDomain> _P = this.BackLinkDomainProjects(KV.Value, URI);
                        if (_P.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _P);
                        break;
                }
            }
            return BLD;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainReferences(ServerConnection SC, string URI, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.List<BackLinkDomain> Terms = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain Ident = this.BackLinkDomain(SC, URI, "Agent", "AgentReference", "ReferenceURI", 2);
            if (Ident.DtItems.Rows.Count > 0)
                Terms.Add(Ident);
            return Terms;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainProjects(ServerConnection SC, string URI, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.List<BackLinkDomain> Links = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain("Project", "ProjectProxy", "ProjectURI", 2);
            string Prefix = "dbo.";
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "]." + Prefix;
            string SQL = "SELECT 'First of ' + CAST(COUNT(*) as varchar) + ' agents' AS DisplayText, " +
                "MIN(S.AgentID) AS ID " +
                "FROM " + Prefix + "ProjectProxy AS T " +
                "INNER JOIN " + Prefix + "AgentProject AS S ON T.ProjectID = S.ProjectID " +
                "INNER JOIN " + Prefix + "AgentID_UserAvailable AS A ON S.AgentID = A.AgentID " +
                "WHERE(T.ProjectURI = '" + URI + "') " +
                "GROUP BY T.Project, T.ProjectURI ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
            try
            {
                ad.Fill(BackLink.DtItems);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (BackLink.DtItems.Rows.Count > 0)
                Links.Add(BackLink);
            return Links;

        }


        private DiversityWorkbench.BackLinkDomain BackLinkDomain(ServerConnection SC, string URI, string DisplayText, string Table, string LinkColumn, int ImageKey, System.Collections.Generic.List<string> Restrictions = null)
        {
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain(DisplayText, Table, LinkColumn, ImageKey);
            string testConn = this.Prefix;
            string Prefix = "[" + SC.DatabaseName + "].dbo."; // Toni 20210727 database name added
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "]." + Prefix;
            string SQL = "SELECT T.AgentID AS ID, A.AgentName AS DisplayText " +
                "FROM " + Prefix + Table + " AS T " +
                "INNER JOIN " + Prefix + "Agent AS A ON T.AgentID = A.AgentID " +
                "WHERE(T." + LinkColumn + " = '" + URI + "') " +
                "GROUP BY T.AgentID, A.AgentName ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
            try
            {
                ad.Fill(BackLink.DtItems);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return BackLink;
        }

        #endregion

        #region Descriptor images

        private static System.Collections.Generic.Dictionary<string, System.Drawing.Image> _DescriptorTypeImageDict;
        public static System.Collections.Generic.Dictionary<string, System.Drawing.Image> DescriptorTypeImageDict
        {
            get { if (_DescriptorTypeImageDict == null) _DescriptorTypeImageDict = new Dictionary<string, System.Drawing.Image>(); return _DescriptorTypeImageDict; }
        }

        private static System.Collections.Generic.Dictionary<string, int> _DescriptorTypeDict;
        public static System.Collections.Generic.Dictionary<string, int> DescriptorTypeDict
        {
            get { if (_DescriptorTypeDict == null) _DescriptorTypeDict = new Dictionary<string, int>(); return _DescriptorTypeDict; }
        }


        private static System.Windows.Forms.ImageList _imageList;
        public static System.Windows.Forms.ImageList imageList
        {
            get { if (_imageList == null) _imageList = new System.Windows.Forms.ImageList(); return _imageList; }
        }

        public static void DescriptorTypeImageAdd(int BaseLengthOfImageList, string Key, System.IO.FileInfo f)
        {
            System.Drawing.Image I = System.Drawing.Image.FromFile(f.FullName);
            System.Drawing.Bitmap B = new System.Drawing.Bitmap(I, 16, 16);
            System.Drawing.Bitmap BG = DiversityWorkbench.Forms.FormFunctions.MakeGrayscale3(B);
            B.MakeTransparent();
            if (DescriptorTypeImageDict.ContainsKey(Key))
            {
                DescriptorTypeImageDict[Key] = (System.Drawing.Image)B;
                imageList.Images.RemoveByKey(Key);
                imageList.Images.Add(Key, B);
            }
            else
            {
                DescriptorTypeDict.Add(Key, BaseLengthOfImageList + DescriptorTypeDict.Count);
                int dictCount = DescriptorTypeImageDict.Count;
                DescriptorTypeImageDict.Add(Key, B);
                imageList.Images.Add(Key, B);
            }

        }

        public static void DescriptorTypeImageReset() { _DescriptorTypeDict = null; _DescriptorTypeImageDict = null; imageList.Images.Clear(); }

        public static void DescriptorTypeImageAdd(int BaseLengthOfImageList, string Key, System.Drawing.Bitmap B)
        {
            if (DescriptorTypeImageDict.ContainsKey(Key))
            {
                DescriptorTypeImageDict[Key] = (System.Drawing.Image)B;
                imageList.Images.RemoveByKey(Key);
                imageList.Images.Add(Key, B);
            }
            else
            {
                DescriptorTypeDict.Add(Key, BaseLengthOfImageList + DescriptorTypeDict.Count);
                int dictCount = DescriptorTypeImageDict.Count;
                DescriptorTypeImageDict.Add(Key, B);
                imageList.Images.Add(Key, B);
            }

        }

        #endregion

    }
}
