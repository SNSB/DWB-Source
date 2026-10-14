using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityProjects.GFBio
{

    public struct AgentValue
    {
        public string GivenName;
        public string InheritedName;
        public string Email;
        public string Address;
        public string AgentName;
        public string AgentURI;
        public string AgentType;
        public System.Collections.Generic.List<string> AgentRoles;
        //public System.Collections.Generic.List<string> Roles;
    }


    public class Project
    {

        #region Parameter

        private System.Collections.Generic.List<GFBioValue> _GFBioValues;
        private int _ProjectID;
        private DiversityWorkbench.ServerConnection _SCAgents = null;
        private int? _AgentsProjectID = null;

        #endregion

        #region Construction

        public Project(int ProjectID, System.Collections.Generic.List<GFBioValue> Values, DiversityWorkbench.ServerConnection AgentsDatabase = null, int? AgentsProjectID = null)
        {
            this._GFBioValues = Values;
            this._ProjectID = ProjectID;
            if (AgentsDatabase != null)
                this._SCAgents = AgentsDatabase;
            if (AgentsProjectID != null)
                this._AgentsProjectID = AgentsProjectID;
        }

        #endregion

        #region Import

        public bool ImportData(ref string Error)
        {
            bool OK = true;
            if (!IdentifierTypeAvailable(this._GFBioValues))
            {
                Error = "Identifier type is missing";
                return false;
            }
            string CurrentPosition = "";
            try
            {
                // import Agents
                foreach (GFBioValue G in this._GFBioValues)
                {
                    if (G.Table == "Agent" || G.Table == "AgentContactInformation" || G.Table == "ProjectAgentRole")
                    {
                        this.AddAgentInfo(G);
                    }
                    else
                        continue;
                }
                OK = this.ImportAgents(ref Error);

                // import the rest
                foreach (GFBioValue G in this._GFBioValues)
                {
                    if (G.Table == "Agent" || G.Table == "AgentContactInformation" || G.Table == "ProjectAgentRole")
                    {
                        continue;
                    }
                    if (G.Value == null)
                        continue;
                    if (G.Value.Trim().ToLower() == "null" || G.Value.Trim().Length == 0)
                        continue;
                    CurrentPosition = "Table: " + G.Table + "; ";
                    if (G.Column != null)
                        CurrentPosition += "Column: " + G.Column + "; ";
                    if (G.Setting != null)
                        CurrentPosition += "Setting: " + G.Setting + "; ";
                    if (G.Identifier != null)
                        CurrentPosition += "Identifier: " + G.Identifier + "; ";
                    CurrentPosition += "Value: " + G.Value;
                    if (G.Setting != null && G.Setting.Length > 0 && G.Table == "ProjectAgentRole")
                    {
                        this.InsertValue(G.Table, G.Column, G.Setting, "AgentName", G.Value, ref Error);
                    }
                    else if (G.Table == "ProjectLicense" && G.Column == "LicenseHolder")
                    {
                        this.InsertValue(G.Table, G.Column, G.Value, ref Error);
                        if (GFBio.SettingsGFBio.Default.LicenseHolderURI != null && GFBio.SettingsGFBio.Default.LicenseHolderURI.Length > 0)
                            this.InsertValue(G.Table, "LicenseHolderAgentURI", GFBio.SettingsGFBio.Default.LicenseHolderURI, ref Error);
                    }
                    else if (G.Table == "ProjectLicense" && G.Column == "IPR")
                    {
                        this.InsertValue(G.Table, "IPRHolder", G.Value, ref Error);
                        if (GFBio.SettingsGFBio.Default.IPRURI != null && GFBio.SettingsGFBio.Default.IPRURI.Length > 0)
                            this.InsertValue(G.Table, "IPRHolderAgentURI", GFBio.SettingsGFBio.Default.IPRURI, ref Error);
                    }
                    else if (G.Setting != null && G.Setting.Length > 0)
                    {
                        this.InsertSetting(G.Setting, G.Value, ref Error);
                    }
                    else if (G.Identifier != null && G.Identifier.Length > 0)
                    {
                        this.InsertIdentifier(G.Identifier, G.Column, G.Value, ref Error);
                    }
                    else
                    {
                        this.InsertValue(G.Table, G.Column, G.Value, ref Error);
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error += ex.Message + "\r\nCurrent position: " + CurrentPosition;
            }
            if (Error.Length > 0)
            {
                //System.Windows.Forms.MessageBox.Show("Import procuded errors:\r\n" + Error);
                OK = false;
            }
            return OK;
        }

        private System.Collections.Generic.Dictionary<int, AgentValue> _AgentValues;

        private void AddAgentInfo(GFBioValue Value)
        {
            try
            {
                if (this._AgentValues == null)
                    this._AgentValues = new Dictionary<int, AgentValue>();
                int Identifier;
                if (int.TryParse(Value.Identifier, out Identifier))
                {
                    if (!this._AgentValues.ContainsKey(Identifier))
                    {
                        AgentValue A = new AgentValue();
                        this._AgentValues.Add(Identifier, A);
                    }
                    switch (Value.Column)
                    {
                        case "GivenName":
                            AgentValue A = this._AgentValues[Identifier];
                            A.GivenName = Value.Value.Trim();
                            this._AgentValues[Identifier] = A;
                            break;
                        case "InheritedName":
                            AgentValue AI = this._AgentValues[Identifier];
                            AI.InheritedName = Value.Value.Trim();
                            this._AgentValues[Identifier] = AI;
                            break;
                        case "Email":
                            AgentValue AE = this._AgentValues[Identifier];
                            AE.Email = Value.Value.Trim();
                            this._AgentValues[Identifier] = AE;
                            break;
                        case "Institution":
                            AgentValue ACN = this._AgentValues[Identifier];
                            ACN.Address = Value.Value.Trim();
                            this._AgentValues[Identifier] = ACN;
                            break;
                        case "AgentRole":
                            AgentValue AR = this._AgentValues[Identifier];
                            if (AR.AgentRoles == null)
                                AR.AgentRoles = new List<string>();
                            AR.AgentRoles.Add(Value.Value.Trim());
                            this._AgentValues[Identifier] = AR;
                            break;
                        case "AgentType":
                            AgentValue AT = this._AgentValues[Identifier];
                            AT.AgentType = Value.Value.Trim();
                            this._AgentValues[Identifier] = AT;
                            break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private bool ImportAgents(ref string Error)
        {
            if (this._AgentValues == null)
                return true;

            bool OK = true;
            string SQL = "";
            System.Data.DataTable dtRoles = new System.Data.DataTable();
            SQL = "SELECT Code, DisplayText FROM ProjectAgentRole_Enum ORDER BY Code ";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtRoles, ref Message);
            foreach (System.Collections.Generic.KeyValuePair<int, AgentValue> AV in this._AgentValues)
            {
                try
                {
                    System.Collections.Generic.Dictionary<string, string> AgentDict = new Dictionary<string, string>();
                    AgentDict.Add("ProjectID", this._AgentsProjectID.ToString());
                    AgentDict.Add("GivenName", AV.Value.GivenName);
                    AgentDict.Add("InheritedName", AV.Value.InheritedName);
                    AgentDict.Add("Address", AV.Value.Address);
                    AgentDict.Add("Email", AV.Value.Email);
                    AgentDict.Add("AgentType", AV.Value.AgentType);

                    System.Collections.Generic.List<string> Decisives = new List<string>();
                    Decisives.Add("GivenName");
                    Decisives.Add("AgentType");
                    if (AV.Value.AgentType == "Person")
                    {
                        Decisives.Add("InheritedName");
                        Decisives.Add("Email");
                    }

                    string URI = "";
                    string AgentDisplayText = "";
                    OK = DiversityWorkbench.Agent.MergeAgent(ref URI, ref AgentDisplayText, this._SCAgents, AgentDict, Decisives, (int)this._AgentsProjectID);
                    if (OK)
                    {
                        // testing if Agent already present
                        SQL = "SELECT COUNT(*) FROM ProjectAgent " +
                            " WHERE ProjectID = " + this._ProjectID.ToString() + " AND AgentName = '" + AgentDisplayText + "' AND AgentURI = '" + URI + "'";
                        string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (Result == "0")
                        {
                            SQL = "INSERT INTO ProjectAgent " +
                                "(ProjectID, AgentName, AgentURI, AgentType, AgentSequence) " +
                                "VALUES (" + this._ProjectID.ToString() + ", '" + AgentDisplayText + "', '" + URI + "', '" + AV.Value.AgentType + "', " + AV.Key.ToString() + ") ";
                            if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
                                OK = false;
                        }
                        if (OK)
                        {
                            // Getting the ProjectAgentID
                            SQL = "SELECT TOP 1 ProjectAgentID FROM ProjectAgent WHERE ProjectID = " + this._ProjectID.ToString() + " AND AgentName = '" + AgentDisplayText + "' AND AgentURI = '" + URI + "'";
                            string ProjectAgentID = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                            if (AV.Value.AgentRoles != null)
                            {
                                foreach (string R in AV.Value.AgentRoles)
                                {
                                    string Role = R;
                                    // testing if the role is valid
                                    System.Data.DataRow[] rr = dtRoles.Select("Code = '" + Role + "'");
                                    if (rr.Length == 0)
                                    {
                                        rr = dtRoles.Select("DisplayText = '" + Role + "'");
                                        if (rr.Length == 1)
                                        {
                                            Role = rr[0]["Code"].ToString();
                                        }
                                        else
                                        {
                                            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtRoles, "Code", "Code", "Wrong role", "The role " + Role + " does not exist. Please select a valid role from the list", "", false, true, true);
                                            f.ShowDialog();
                                            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                                                Role = f.SelectedString;
                                            else
                                            {
                                                System.Windows.Forms.MessageBox.Show("Import stoped");
                                                return false;
                                            }
                                        }
                                    }
                                    // testing if role already present
                                    SQL = "SELECT COUNT(*) FROM ProjectAgentRole " +
                                        " WHERE ProjectID = " + this._ProjectID.ToString() + " AND AgentName = '" + AgentDisplayText + "' AND AgentURI = '" + URI + "' AND AgentRole = '" + Role + "'";
                                    Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
                                    if (Result == "0")
                                    {
                                        SQL = "INSERT INTO ProjectAgentRole " +
                                            "(ProjectID, AgentName, AgentURI, AgentRole, ProjectAgentID) " +
                                            "VALUES (" + this._ProjectID.ToString() + ", '" + AgentDisplayText + "', '" + URI + "', '" + Role + "', " + ProjectAgentID + ")";
                                        if (!DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error))
                                            OK = false;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                    Error += "\r\nError importing agents: " + ex.Message + "\r\nSQL: " + SQL + "\r\n";
                }
            }
            return OK;
        }

        #endregion

        #region Setting

        private bool InsertSetting(string Setting, string Value, ref string Error)
        {
            bool OK = true;
            string SQL = "INSERT INTO ProjectSetting (ProjectID, SettingID, Value) " +
                "VALUES (" + _ProjectID.ToString() + ", " + this.SettingID(Setting, ref Error).ToString() + ", '" + Value.Replace("'", "''") + "')";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
            return OK;
        }

        private int SettingID(string SettingDisplayText, ref string Error)
        {
            int ID = 0;
            string SQL = "SELECT COUNT(*) FROM Setting S WHERE S.DisplayText = '" + SettingDisplayText + "' AND S.ParentSettingID = " + this.SettingIdForGFBio(ref Error);
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
            if (Result == "0")
            {
                SQL = "INSERT INTO Setting (DisplayText, ParentSettingID) VALUES ('" + SettingDisplayText + "', " + this.SettingIdForGFBio(ref Error) + ")";
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
            }
            SQL = "SELECT MIN(S.SettingID) FROM Setting S WHERE S.DisplayText = '" + SettingDisplayText + "' AND S.ParentSettingID = " + this.SettingIdForGFBio(ref Error).ToString();
            Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
            int.TryParse(Result, out ID);
            return ID;
        }

        private int? _SettingIdForGFBio = null;
        private int SettingIdForGFBio(ref string Error)
        {
            if (this._SettingIdForGFBio == null)
            {
                int ID = 0;
                string SQL = "SELECT COUNT(*) FROM Setting S WHERE S.DisplayText = 'GFBio'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
                if (Result == "0")
                {
                    SQL = "INSERT INTO Setting (DisplayText) VALUES ('GFBio')";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
                }
                SQL = "SELECT MIN(S.SettingID) FROM Setting S WHERE S.DisplayText = 'GFBio'";
                Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
                if (int.TryParse(Result, out ID))
                    this._SettingIdForGFBio = ID;
            }
            return (int)this._SettingIdForGFBio;
        }

        #endregion

        #region Identifier

        public static bool IdentifierTypeAvailable(System.Collections.Generic.List<GFBioValue> Values)
        {
            bool OK = true;
            string Error = "";
            string Message = "";
            try
            {
                foreach (GFBioValue G in Values)
                {
                    bool IdentiferAvailable = IdentifierTypeAvailable(G, ref Error, ref Message);
                    if (!IdentiferAvailable)
                        OK = IdentiferAvailable;
                }
                if (Message.Length > 0)
                    System.Windows.Forms.MessageBox.Show(Message, "Missing identifier types", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Stop);
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error += ex.Message;
            }
            if (Error.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Import procuded errors:\r\n" + Error);
                OK = false;
            }
            return OK;
        }

        public static bool IdentifierTypeAvailable(GFBioValue Value, ref string Error, ref string Message)
        {
            bool OK = true;
            try
            {
                if (Value.Identifier != null && Value.Identifier.Length > 0 && Value.Column == "Type")
                {
                    string SQL = "SELECT Type FROM ProjectIdentifierType WHERE Type = '" + Value.Value.Replace("'", "''") + "'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
                    if (Result.ToLower() != Value.Value.ToLower())
                    {
                        OK = false;
                        if (Message.Length > 0)
                            Message += "\r\n\r\n";
                        Message += "An identifier type is missing:\r\n" + Value.Value + "\r\nPlease insert this type to enable the import";
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                Error += ex.Message;
            }
            if (Error.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Import procuded errors:\r\n" + Error);
                OK = false;
            }
            return OK;
        }

        private bool InsertIdentifier(string Identifier, string Column, string Value, ref string Error)
        {
            bool OK = true;
            string SQL = "SELECT COUNT(*) FROM ProjectIdentifier P WHERE ProjectID = " + _ProjectID.ToString() + " AND Identifier = '" + Identifier.Replace("'", "''").Trim() + "'";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            if (Result == "0")
            {
                SQL = "INSERT INTO ProjectIdentifier (ProjectID, Identifier, " + Column + ") " +
                    "VALUES (" + _ProjectID.ToString() + ", '" + Identifier.Replace("'", "''").Trim() + "', '" + Value.Replace("'", "''").Trim() + "')";
            }
            else
            {
                SQL = "UPDATE P SET " + Column + " = '" + Value.Replace("'", "''").Trim() + "' " +
                    "FROM ProjectIdentifier P WHERE ProjectID = " + _ProjectID.ToString() + " AND Identifier = '" + Identifier.Replace("'", "''").Trim() + "'";
            }
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
            return OK;
        }

        #endregion

        #region Values

        private bool InsertValue(string Table, string Column, string Value, ref string Error)
        {
            bool OK = true;
            string SQL = "";
            try
            {
                if (Table == "ProjectLabel" || Table == "ProjectKeyword")
                {
                    if (Value != null && Value.Length > 0)
                    {
                        SQL = "SELECT COUNT(*) FROM ProjectDescriptor WHERE ProjectID = " + this._ProjectID.ToString() + " AND Content = '" + Value.Replace("'", "''") + "'";
                        string Count = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (Count == "0")
                        {
                            string Type = "1";
                            if (Table == "ProjectLabel")
                                Type = "2";
                            SQL = "INSERT INTO ProjectDescriptor (ProjectID, Language, ElementID, [Content], ContentURI) VALUES (" + this._ProjectID.ToString() + ", 'en', " + Type + ", '" + Value.Replace("'", "''") + "', '')";
                        }
                    }
                }
                else
                {
                    DiversityWorkbench.Data.Table T = new DiversityWorkbench.Data.Table(Table, DiversityWorkbench.Settings.ConnectionString);
                    SQL = "SELECT COUNT(*) FROM " + Table + " WHERE ProjectID = " + this._ProjectID.ToString();
                    if (T.PrimaryKeyColumnList.Contains(Column))
                        SQL += " AND " + Column + " = '" + Value + "'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
                    if (Result == "0")
                    {
                        SQL = "INSERT INTO " + Table + " (ProjectID, " + Column + ") VALUES (" + this._ProjectID.ToString() + ", '" + Value.Replace("'", "''") + "')";
                    }
                    else if (Result == "1")
                    {
                        SQL = "UPDATE " + Table + " SET " + Column + " = '" + Value.Replace("'", "''") + "' WHERE ProjectID = " + this._ProjectID.ToString();
                    }
                }
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                Error += "\r\n" + ex.Message + "\r\nSQL: " + SQL;
            }
            return OK;
        }

        private bool InsertValue(string Table, string Column, string KeyValue, string KeyColumn, string Value, ref string Error)
        {
            bool OK = true;
            string SQL = "";
            try
            {
                DiversityWorkbench.Data.Table T = new DiversityWorkbench.Data.Table(Table, DiversityWorkbench.Settings.ConnectionString);
                SQL = "SELECT COUNT(*) FROM " + Table + " WHERE ProjectID = " + this._ProjectID.ToString();
                if (T.PrimaryKeyColumnList.Contains(Column))
                    SQL += " AND " + Column + " = '" + Value + "'";
                if (T.PrimaryKeyColumnList.Contains(KeyColumn))
                    SQL += " AND " + KeyColumn + " = '" + KeyValue + "'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Error);
                if (Result == "0")
                {
                    System.Collections.Generic.Dictionary<string, string> MissingPKcolumns = new Dictionary<string, string>();
                    foreach (string PK in T.PrimaryKeyColumnList)
                    {
                        if (PK != "ProjectID" && PK != Column && PK != KeyColumn && T.Columns[PK].ColumnDefault == null)
                            MissingPKcolumns.Add(PK, "");
                    }
                    SQL = "INSERT INTO " + Table + "(ProjectID, " + KeyColumn + ", " + Column;
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in MissingPKcolumns)
                    {
                        SQL += ", " + KV.Key;
                    }
                    SQL += ") VALUES (" + this._ProjectID.ToString() + ", '" + KeyValue.Replace("'", "''") + "', '" + Value.Replace("'", "''") + "'";
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in MissingPKcolumns)
                    {
                        SQL += ", '" + KV.Value + "'";
                    }
                    SQL += ")";
                }
                else if (Result == "1")
                {
                    SQL = "UPDATE " + Table + " SET " + Column + " = '" + Value.Replace("'", "''") + "' WHERE ProjectID = " + this._ProjectID.ToString() + " AND " + KeyColumn + " = '" + KeyValue + "'";
                }
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, ref Error);

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return OK;
        }

        //private string InsertAgent(string GivenName, string InheritedName, string Email, string Identifier, string IdentifierType = "ORCID")
        //{
        //    string URI = "";
        //    if (this._SCAgents != null)
        //    {
        //        // Testing if Agent exists
        //        string Prefix = this._SCAgents.Prefix();
        //        string SelectClause = "SELECT B.BaseURL + CAST(A.AgentID AS varchar) ";
        //        string FromClause = "FROM " + Prefix + "ViewBaseURL B, " + Prefix + "Agent A ";
        //        string WhereClause = "WHERE A.GivenName = '" + GivenName + "' AND A.InheritedName = '" + InheritedName + "' ";
        //        if (Email.Length > 0)
        //        {
        //            SelectClause += ", C.Email ";
        //            FromClause += " LEFT OUTER JOIN " + Prefix + "AgentContactInformation C ON A.AgentID = C.AgentID AND C.Email = '" + Email + "'";
        //        }
        //        if (Identifier.Length > 0 && IdentifierType.Length > 0)
        //        {
        //            SelectClause += ", I.Identifier ";
        //            FromClause += " LEFT OUTER JOIN " + Prefix + "AgentIdentifier I ON A.AgentID = I.AgentID AND I.Identifier = '" + Identifier + "'";
        //        }
        //        string SQL = SelectClause + FromClause + WhereClause;
        //        Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._SCAgents.ConnectionString);
        //        con.Open();
        //        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
        //        System.Data.DataTable dtAgent = new System.Data.DataTable();
        //        ad.Fill(dtAgent);
        //        bool CreateAgent = false;
        //        if (dtAgent.Rows.Count == 0)
        //        {
        //            // No fit - create agent
        //            CreateAgent = true;
        //        }
        //        else if (dtAgent.Rows.Count == 1)
        //        {
        //            URI = dtAgent.Rows[0][0].ToString();
        //        }
        //        else
        //        {
        //            foreach(System.Data.DataRow R in dtAgent.Rows)
        //            {
        //                if (Email.Length > 0 && R.Table.Columns.Contains("Email") && !R["Email"].Equals(System.DBNull.Value) && R["Email"].ToString() == Email)
        //                {
        //                    URI = R[0].ToString();
        //                    break;
        //                }
        //                if (Identifier.Length > 0 && R.Table.Columns.Contains("Identifier") && !R["Identifier"].Equals(System.DBNull.Value) && R["Identifier"].ToString() == Identifier)
        //                {
        //                    URI = R[0].ToString();
        //                    break;
        //                }
        //            }
        //            if (URI.Length == 0)
        //                CreateAgent = true;
        //        }
        //        if (CreateAgent)
        //        {
        //            try
        //            {
        //                SQL = "DECLARE @T Table(AgentID int); " +
        //                    "INSERT INTO " + Prefix + "Agent " +
        //                    "(GivenName, InheritedName) " +
        //                    "OUTPUT INSERTED.AgentID INTO @T " +
        //                    "VALUES ('" + GivenName + "', '" + InheritedName + "')";
        //                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
        //                string AgentID = C.ExecuteScalar().ToString();
        //                SQL = "SELECT B.BaseURL FROM " + Prefix + "ViewBaseURL B";
        //                string BaseURL = C.ExecuteScalar().ToString();
        //                URI = BaseURL + AgentID;
        //                if (Email.Length > 0)
        //                {
        //                    SQL = "DECLARE @I int = 1; " +
        //                        " IF (SELECT COUNT(*) FROM " + Prefix + "AgentContactInformation WHERE AgentID = " + AgentID + ") > 0 " +
        //                        " BEGIN SET @I = (SELECT MAX(DisplayOrder) + 1 FROM " + Prefix + "AgentContactInformation WHERE AgentID = " + AgentID + ") END ;" +
        //                        " INSERT INTO " + Prefix + "AgentContactInformation " +
        //                        " (AgentID, DisplayOrder, Email) " +
        //                        " VALUES (" + AgentID + ", @I, '" + Email + "')";
        //                    C.CommandText = SQL;
        //                    try
        //                    {
        //                        C.ExecuteNonQuery();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //                    }
        //                }
        //                if (Identifier.Length > 0)
        //                {
        //                    SQL = "INSERT INTO " + Prefix + "AgentIdentifier " +
        //                    "(AgentID, Identifier, Type) " +
        //                    "VALUES (" + AgentID + ", '" + Identifier + "', '" + IdentifierType + "')";
        //                    C.CommandText = SQL;
        //                    try
        //                    {
        //                        C.ExecuteNonQuery();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //            }
        //        }
        //    }
        //    return URI;
        //}

        #endregion

    }
}
