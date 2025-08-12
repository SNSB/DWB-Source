using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace DiversityWorkbench
{
    public class Project : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {

        #region Construction
        public Project(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }

        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityProjects"; }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                try
                {
                    string Prefix = this.ServerConnection.DatabaseName + ".dbo.";
                    if (this.ServerConnection.LinkedServer.Length > 0)
                        Prefix = "[" + this.ServerConnection.LinkedServer + "]." + Prefix;
                    string SQL = "";

                    // Check if the StableIdentifierBase has been defined
                    SQL = "SELECT Project.StableIdentifier AS Project " +
                        "FROM " + Prefix + "ViewProject AS Project LEFT OUTER JOIN " +
                        Prefix + "ViewProject AS Project_1 ON Project.ProjectParentID = Project_1.ProjectID " +
                        "WHERE (Project.ProjectID = " + ID.ToString() + ")";
                    if (!this.getDataFromTable(SQL, ref Values) && Values.Count == 0)
                        Values.Add("Project", "Please define the STABLE IDENTIFIER BASE in the Projects database to get the results");
                    else
                        Values.Clear();

                    // Markus 8.8.23: Bugfix - using ViewBaseURL instead of function or linked server
                    string tntServer = global::DiversityWorkbench.Properties.Settings.Default.TNTServer;
                    SQL = "SELECT Base.BaseURL + CAST(Project.ProjectID AS varchar) AS _URI, Project.Project AS _DisplayText, " +
                        "Project.Project, Project_1.Project AS [Superior project], " +
                        "Project.ProjectTitle AS [Title], Project_1.ProjectTitle AS [Title of superior project], " +
                        "Project.ProjectDescription AS Description, Project.ProjectNotes AS Notes, " +
                        "Project.ProjectVersion AS Version, " +
                        "Project.StableIdentifier " +
                        "FROM " + Prefix + "ViewProject AS Project LEFT OUTER JOIN " +
                        Prefix + "ViewProject AS Project_1 ON Project.ProjectParentID = Project_1.ProjectID " +
                        "CROSS JOIN [" + tntServer +"].DiversityProjects_TNT.dbo.ViewBaseURL AS Base " +
                        "WHERE (Project.ProjectID = " + ID.ToString() + ")";
                    this.getDataFromTable(SQL, ref Values);

                    SQL = "SELECT AgentName AS Agent " +
                        "FROM " + Prefix + "ProjectAgent " +
                        "WHERE ProjectID = " + ID.ToString() +
                        " ORDER BY AgentName";
                    this.getDataFromTable(SQL, ref Values);

                    SQL = "SELECT ReferenceTitle AS Reference " +
                        "FROM " + Prefix + "ProjectReference " +
                        "WHERE ProjectID = " + ID.ToString() +
                        " ORDER BY ReferenceTitle";
                    this.getDataFromTable(SQL, ref Values);

                    SQL = "SELECT D.Content AS Descriptor, E.DisplayText AS Type " +
                        "FROM " + Prefix + "ProjectDescriptor D, " + Prefix + "ProjectDescriptorElement E " +
                        "WHERE D.ProjectID = " + ID.ToString() + " AND D.ElementID = E.ElementID " +
                        "ORDER BY D.Content";
                    this.getDataFromTable(SQL, ref Values);

                    //Markus, 23.7.2018: Only direct settings are displayed here. Inherited are retrieved via a function not available via a linked server
                    SQL = "SELECT S.DisplayText AS Setting , P.Value " +
                        "FROM " + Prefix + "ProjectSetting P INNER JOIN " +
                        "" + Prefix + "Setting S ON P.SettingID = S.SettingID " +
                        "WHERE P.ProjectID = " + ID.ToString();
                    this.getDataFromTable(SQL, ref Values);

                    if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
                    this._UnitValues.Clear();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
                    {
                        this._UnitValues.Add(P.Key, P.Value);
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return Values;
        }

        public string MainTable() { return "Project"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[9];
            QueryDisplayColumns[0].DisplayColumn = "Project";

            QueryDisplayColumns[1].DisplayText = "Title";
            QueryDisplayColumns[1].DisplayColumn = "ProjectTitle";

            QueryDisplayColumns[2].DisplayText = "Type";
            QueryDisplayColumns[2].DisplayColumn = "ProjectType";

            QueryDisplayColumns[3].DisplayText = "Agent";
            QueryDisplayColumns[3].DisplayColumn = "AgentName";
            QueryDisplayColumns[3].TableName = "ProjectAgent";

            QueryDisplayColumns[4].DisplayText = "IPR";
            QueryDisplayColumns[4].DisplayColumn = "IPRHolder";
            QueryDisplayColumns[4].TableName = "ProjectLicense";

            QueryDisplayColumns[5].DisplayText = "Reference";
            QueryDisplayColumns[5].DisplayColumn = "ReferenceTitle";
            QueryDisplayColumns[5].TableName = "ProjectReference";

            QueryDisplayColumns[6].DisplayText = "Descriptor";
            QueryDisplayColumns[6].DisplayColumn = "Descriptor";
            QueryDisplayColumns[6].TableName = "View_ProjectDescriptor";

            QueryDisplayColumns[7].DisplayText = "Setting";
            QueryDisplayColumns[7].DisplayColumn = "Setting";
            QueryDisplayColumns[7].TableName = "View_ProjectSetting";

            QueryDisplayColumns[8].DisplayText = "Hierarchy";
            QueryDisplayColumns[8].DisplayColumn = "DisplayText";
            QueryDisplayColumns[8].TableName = "View_ProjectHierarchyAll";

            for (int i = 0; i < QueryDisplayColumns.Length; i++)
            {
                if (QueryDisplayColumns[i].DisplayText == null)
                    QueryDisplayColumns[i].DisplayText = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].OrderColumn == null)
                    QueryDisplayColumns[i].OrderColumn = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].IdentityColumn == null)
                    QueryDisplayColumns[i].IdentityColumn = "ProjectID";
                if (QueryDisplayColumns[i].TableName == null)
                    QueryDisplayColumns[i].TableName = "Project_Core";
                if (QueryDisplayColumns[i].TipText == null)
                    QueryDisplayColumns[i].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(QueryDisplayColumns[i].TableName, QueryDisplayColumns[i].DisplayColumn);
                QueryDisplayColumns[i].Module = "DiversityProjects";
            }
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();
            string Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityProjects"].ServerConnection.DatabaseName;

            try
            {
                #region Project

                string Description = DiversityWorkbench.Functions.ColumnDescription("Project", "Project");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "Project_Core", "ProjectID", "Project", "Project", "Project", "Project", Description);
                QueryConditions.Add(q0);

                Description = DiversityWorkbench.Functions.ColumnDescription("Project", "ProjectID");
                DiversityWorkbench.QueryCondition qProjectID = new DiversityWorkbench.QueryCondition(false, "Project_Core", "ProjectID", "ProjectID", "Project", "ID", "ProjectID", Description, false, false, true, false);
                QueryConditions.Add(qProjectID);

                Description = DiversityWorkbench.Functions.ColumnDescription("Project", "ProjectTitle");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "Project_Core", "ProjectID", "ProjectTitle", "Project", "Title", "Project title", Description);
                QueryConditions.Add(q1);

                Description = DiversityWorkbench.Functions.ColumnDescription("Project", "ProjectDescription");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "Project_Core", "ProjectID", "ProjectDescription", "Project", "Description", "Description", Description);
                QueryConditions.Add(q2);

                Description = DiversityWorkbench.Functions.ColumnDescription("Project", "ProjectURL");
                DiversityWorkbench.QueryCondition qProjectURL = new DiversityWorkbench.QueryCondition(true, "Project_Core", "ProjectID", "ProjectURL", "Project", "URL", "Project URL", Description);
                QueryConditions.Add(qProjectURL);

                Description = DiversityWorkbench.Functions.ColumnDescription("Project", "ProjectNotes");
                DiversityWorkbench.QueryCondition qProjectNotes = new DiversityWorkbench.QueryCondition(true, "Project_Core", "ProjectID", "ProjectNotes", "Project", "Notes", "Project notes", Description);
                QueryConditions.Add(qProjectNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("Project", "ProjectType");
                DiversityWorkbench.QueryCondition qProjectType = new DiversityWorkbench.QueryCondition(false, "Project_Core", "ProjectID", "ProjectType", "Project", "Project Type", "Project Type", Description, "ProjectType_Enum", Database);
                QueryConditions.Add(qProjectType);

                //Description = DiversityWorkbench.Functions.ColumnDescription("Project", "ProjectRights");
                //DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "Project_Core", "ProjectID", "ProjectRights", "Project", "Rights", "Rights", Description);
                //QueryConditions.Add(q3);

                //Description = DiversityWorkbench.Functions.ColumnDescription("Project", "ProjectSettings");
                //DiversityWorkbench.QueryCondition qSettings = new DiversityWorkbench.QueryCondition(true, "Project_Core", "ProjectID", "ProjectSettings", true, "Project", "Settings", "Settings", Description);
                //QueryConditions.Add(qSettings);

                #endregion

                #region Setting

                Description = "If any setting is present";
                DiversityWorkbench.QueryCondition qSettingPresent = new DiversityWorkbench.QueryCondition(false, "ProjectSetting", "ProjectID", "Settings", "Presence", "Settings present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qSettingPresent);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectSetting", "Value");
                DiversityWorkbench.QueryCondition ProjectSettingValue = new DiversityWorkbench.QueryCondition(true, "ProjectSetting", "ProjectID", "Value", "Settings", "Value", "Value", Description);
                QueryConditions.Add(ProjectSettingValue);

                System.Data.DataTable dtSetting = new System.Data.DataTable();
                string SQL = "SELECT NULL AS SettingID, NULL AS ParentSettingID, NULL AS DisplayText UNION " +
                    "SELECT SettingID, ParentSettingID, DisplayText  " +
                    "FROM [Setting] " +
                    "ORDER BY DisplayText ";
                Microsoft.Data.SqlClient.SqlDataAdapter aSetting = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aSetting.Fill(dtSetting); }
                    catch { }
                }
                if (dtSetting.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("SettingID");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentSettingID");
                    System.Data.DataColumn Display = new System.Data.DataColumn("DisplayText");
                    dtSetting.Columns.Add(Value);
                    dtSetting.Columns.Add(ParentValue);
                    dtSetting.Columns.Add(Display);
                }
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FFSetting = new List<QueryField>();
                DiversityWorkbench.QueryField CSCSetting = new QueryField("ProjectSetting", "SettingID", "ProjectID");
                FFSetting.Add(CSCSetting);

                Description = DiversityWorkbench.Functions.ColumnDescription("Setting", "DisplayText");
                DiversityWorkbench.QueryCondition qSetting = new DiversityWorkbench.QueryCondition(false, FFSetting, "Settings", "Setting", "Setting", Description, dtSetting, true, "DisplayText", "ParentSettingID", "DisplayText", "SettingID");
                QueryConditions.Add(qSetting);

                #endregion

                #region Agent

                Description = "If any agent is present";
                DiversityWorkbench.QueryCondition qAgentPresence = new DiversityWorkbench.QueryCondition(true, "ProjectAgent_Core", "ProjectID", "Agents", "Presence", "Agent present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qAgentPresence);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectAgent", "AgentName");
                DiversityWorkbench.QueryCondition qAgentName = new DiversityWorkbench.QueryCondition(true, "ProjectAgent_Core", "ProjectID", "AgentName", "Agents", "Agent", "Agent", Description);
                QueryConditions.Add(qAgentName);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectAgent", "AgentType");
                DiversityWorkbench.QueryCondition qAgentType = new DiversityWorkbench.QueryCondition(false, "ProjectAgent_Core", "ProjectID", "AgentType", "Agents", "Type", "Type", Description, "ProjectAgentType_Enum", Database);
                QueryConditions.Add(qAgentType);


                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectAgent", "Notes");
                DiversityWorkbench.QueryCondition qAgentNotes = new DiversityWorkbench.QueryCondition(true, "ProjectAgent_Core", "ProjectID", "Notes", "Agents", "Notes", "Notes", Description);
                QueryConditions.Add(qAgentNotes);

                Description = "If any agent role is present";
                DiversityWorkbench.QueryCondition qAgentRolePresence = new DiversityWorkbench.QueryCondition(true, "ProjectAgentRole", "ProjectID", "Agents", "Role present", "Role present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qAgentRolePresence);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectAgentRole", "AgentRole");
                DiversityWorkbench.QueryCondition qAgentRole = new DiversityWorkbench.QueryCondition(false, "ProjectAgentRole", "ProjectID", "AgentRole", "Agents", "Role", "Role", Description, "ProjectAgentRole_Enum", Database);
                QueryConditions.Add(qAgentRole);

                #endregion

                #region Licenses

                Description = "If any license is present";
                DiversityWorkbench.QueryCondition qLicensePresence = new DiversityWorkbench.QueryCondition(true, "ProjectLicense", "ProjectID", "Licenses", "Presence", "License present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qLicensePresence);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectLicense", "DisplayText");
                DiversityWorkbench.QueryCondition qProjectLicenseDisplayText = new DiversityWorkbench.QueryCondition(true, "ProjectLicense", "ProjectID", "DisplayText", "Licenses", "License", "License", Description);
                QueryConditions.Add(qProjectLicenseDisplayText);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectLicense", "LicenseURI");
                DiversityWorkbench.QueryCondition qProjectLicenseURI = new DiversityWorkbench.QueryCondition(true, "ProjectLicense", "ProjectID", "LicenseURI", "Licenses", "URI", "License URI", Description);
                QueryConditions.Add(qProjectLicenseURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectLicense", "LicenseType");
                DiversityWorkbench.QueryCondition qProjectLicenseType = new DiversityWorkbench.QueryCondition(false, "ProjectLicense", "ProjectID", "LicenseType", "Licenses", "Type", "License Type", Description);
                QueryConditions.Add(qProjectLicenseType);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectLicense", "LicenseHolder");
                DiversityWorkbench.QueryCondition qProjectLicenseHolder = new DiversityWorkbench.QueryCondition(false, "ProjectLicense", "ProjectID", "LicenseHolder", "Licenses", "Holder", "License Holder", Description);
                QueryConditions.Add(qProjectLicenseHolder);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectLicense", "LicenseYear");
                DiversityWorkbench.QueryCondition qProjectLicenseYear = new DiversityWorkbench.QueryCondition(false, "ProjectLicense", "ProjectID", "LicenseYear", "Licenses", "Year", "License Year", Description);
                QueryConditions.Add(qProjectLicenseYear);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectLicense", "IPR");
                DiversityWorkbench.QueryCondition qProjectLicenseIPR = new DiversityWorkbench.QueryCondition(false, "ProjectLicense", "ProjectID", "IPR", "Licenses", "IPR", "IPR", Description);
                QueryConditions.Add(qProjectLicenseIPR);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectLicense", "CopyrightStatement");
                DiversityWorkbench.QueryCondition qProjectLicenseCopyrightStatement = new DiversityWorkbench.QueryCondition(true, "ProjectLicense", "ProjectID", "CopyrightStatement", "Licenses", "Copyright", "Copyright statement", Description);
                QueryConditions.Add(qProjectLicenseCopyrightStatement);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectLicense", "Notes");
                DiversityWorkbench.QueryCondition qProjectLicenseNotes = new DiversityWorkbench.QueryCondition(false, "ProjectLicense", "ProjectID", "Notes", "Licenses", "Notes", "License Notes", Description);
                QueryConditions.Add(qProjectLicenseNotes);

                #endregion

                #region Resources

                Description = "If any resource is present";
                DiversityWorkbench.QueryCondition qResourcePresence = new DiversityWorkbench.QueryCondition(true, "ProjectResource_Core", "ProjectID", "Resources", "Presence", "Resource present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qResourcePresence);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectResource", "URI");
                DiversityWorkbench.QueryCondition qProjectResource = new DiversityWorkbench.QueryCondition(true, "ProjectResource_Core", "ProjectID", "URI", "Resources", "Resource", "Resource", Description);
                QueryConditions.Add(qProjectResource);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectResource", "SpecificRights");
                DiversityWorkbench.QueryCondition qSpecificRights = new DiversityWorkbench.QueryCondition(true, "ProjectResource_Core", "ProjectID", "SpecificRights", "Resources", "Rights", "Rights", Description);
                QueryConditions.Add(qSpecificRights);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectResource", "Notes");
                DiversityWorkbench.QueryCondition qResourceNotes = new DiversityWorkbench.QueryCondition(true, "ProjectResource_Core", "ProjectID", "Notes", "Resources", "Notes", "Notes", Description);
                QueryConditions.Add(qResourceNotes);

                #endregion

                #region References

                Description = "If any reference is present";
                DiversityWorkbench.QueryCondition qReferencePresence = new DiversityWorkbench.QueryCondition(true, "ProjectReference", "ProjectID", "References", "Presence", "Reference present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qReferencePresence);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectReference", "ReferenceTitle");
                DiversityWorkbench.QueryCondition qProjectReference = new DiversityWorkbench.QueryCondition(true, "ProjectReference", "ProjectID", "ReferenceTitle", "References", "Reference", "Reference title", Description);
                QueryConditions.Add(qProjectReference);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectReference", "ReferenceURI");
                DiversityWorkbench.QueryCondition qProjectReferenceURI = new DiversityWorkbench.QueryCondition(true, "ProjectReference", "ProjectID", "ReferenceURI", "References", "URI", "Reference URI", Description);
                QueryConditions.Add(qProjectReferenceURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectReference", "ReferenceDetails");
                DiversityWorkbench.QueryCondition qProjectReferenceDetails = new DiversityWorkbench.QueryCondition(false, "ProjectReference", "ProjectID", "ReferenceDetails", "References", "Details", "Reference Details", Description);
                QueryConditions.Add(qProjectReferenceDetails);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectReference", "Notes");
                DiversityWorkbench.QueryCondition qProjectReferenceNotes = new DiversityWorkbench.QueryCondition(true, "ProjectReference", "ProjectID", "Notes", "References", "Notes", "Reference notes", Description);
                QueryConditions.Add(qProjectReferenceNotes);

                #endregion

                #region ProjectDescriptor

                Description = "If any descriptor is present";
                DiversityWorkbench.QueryCondition qDescriptorPresence = new DiversityWorkbench.QueryCondition(true, "ProjectDescriptor", "ProjectID", "Descriptors", "Presence", "Descriptor present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qDescriptorPresence);

                System.Data.DataTable dtProjectDescriptorElement = new System.Data.DataTable();
                SQL = "SELECT NULL AS ElementID, NULL AS ParentElementID, NULL AS DisplayText UNION " +
                    "SELECT ElementID, ParentElementID, DisplayText  " +
                    "FROM [ProjectDescriptorElement] " +
                    "ORDER BY DisplayText ";
                Microsoft.Data.SqlClient.SqlDataAdapter aProjectDescriptorElement = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aProjectDescriptorElement.Fill(dtProjectDescriptorElement); }
                    catch { }
                }
                if (dtProjectDescriptorElement.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("ElementID");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentElementID");
                    System.Data.DataColumn Display = new System.Data.DataColumn("DisplayText");
                    dtProjectDescriptorElement.Columns.Add(Value);
                    dtProjectDescriptorElement.Columns.Add(ParentValue);
                    dtProjectDescriptorElement.Columns.Add(Display);
                }
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FFProjectDescriptorElement = new List<QueryField>();
                DiversityWorkbench.QueryField CSCProjectDescriptorElement = new QueryField("ProjectDescriptor", "ElementID", "ProjectID");
                FFProjectDescriptorElement.Add(CSCProjectDescriptorElement);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectDescriptorElement", "DisplayText");
                DiversityWorkbench.QueryCondition qProjectDescriptorElement = new DiversityWorkbench.QueryCondition(false, FFProjectDescriptorElement, "Descriptors", "Type", "Type", Description, dtProjectDescriptorElement, true, "DisplayText", "ParentElementID", "DisplayText", "ElementID");
                QueryConditions.Add(qProjectDescriptorElement);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectDescriptor", "Content");
                DiversityWorkbench.QueryCondition qProjectDescriptor = new DiversityWorkbench.QueryCondition(true, "ProjectDescriptor", "ProjectID", "Content", "Descriptors", "Content", "Content", Description);
                QueryConditions.Add(qProjectDescriptor);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectDescriptor", "ContentURI");
                DiversityWorkbench.QueryCondition qProjectDescriptorContentURI = new DiversityWorkbench.QueryCondition(false, "ProjectDescriptor", "ProjectID", "ContentURI", "Descriptors", "URI", "Content URI", Description);
                QueryConditions.Add(qProjectDescriptorContentURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("ProjectDescriptor", "Language");
                DiversityWorkbench.QueryCondition qProjectDescriptorLanguage = new DiversityWorkbench.QueryCondition(false, "ProjectDescriptor", "ProjectID", "Language", "Descriptors", "Lang.", "Language", Description);
                QueryConditions.Add(qProjectDescriptorLanguage);

                #endregion

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return QueryConditions;
        }

        #endregion

        #region DOI
        public static string getDOI(string ConnectionString, int ProjectID)
        {
            string Doi = "";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            string Json = Project.getProjectInfo(con, ProjectID);
            Doi = "#TestDoi/" + System.DateTime.Now.Second.ToString();
            bool OK = Project.SaveDOI(con, ProjectID, Doi);
            if (OK)
                return Doi;
            else
                return "";
        }

        public static string getProjectInfo(string ConnectionString, int ProjectID)
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            string Json = Project.getProjectInfo(con, ProjectID);
            return Json;
        }

        private static string getProjectInfo(Microsoft.Data.SqlClient.SqlConnection con, int ProjectID)
        {
            string Json = "";
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();
            // Dictionary for all infos
            System.Collections.Generic.Dictionary<string, object> D_Doi = new Dictionary<string, object>();

            // Dictionary for data
            System.Collections.Generic.Dictionary<string, object> D_Data = new Dictionary<string, object>();

            D_Data.Add("type", "dois");

            // Dictionary for attributes
            System.Collections.Generic.Dictionary<string, object> D_Attributes = new Dictionary<string, object>();

            // List for creators
            System.Collections.Generic.List<object> L_creators = new List<object>();
            string Publisher = Project.DoiAgents(con, ProjectID, ref L_creators);
            D_Attributes.Add("creators", L_creators);

            // List for titles
            System.Collections.Generic.Dictionary<string, string> D_Title = new Dictionary<string, string>();
            string SQL = "SELECT ProjectTitle, PublicDescription FROM Project WHERE ProjectID = " + ProjectID.ToString();
            System.Data.DataTable dtProject = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter adProject = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
            adProject.Fill(dtProject);
            string Title = dtProject.Rows[0]["ProjectTitle"].ToString();
            string Description = dtProject.Rows[0]["PublicDescription"].ToString();
            D_Title.Add("title", Title);
            D_Attributes.Add("titles", D_Title);

            D_Attributes.Add("publisher", Publisher);
            D_Data.Add("attributes", D_Attributes);

            // Dictionary for included
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<object>> D_Included = new Dictionary<string, List<object>>();

            D_Doi.Add("data", D_Data);

            con.Close();
            Json = JsonSerializer.Serialize(D_Doi);
            return Json;
        }


        private static bool SaveDOI(Microsoft.Data.SqlClient.SqlConnection con, int ProjectID, string DOI)
        {
            bool OK = false;
            string SQL = "INSERT INTO ProjectIdentifier " +
                "(ProjectID, Identifier, Type) " +
                "VALUES(" + ProjectID.ToString() + ", '" + DOI + "', 'DOI')";
            Microsoft.Data.SqlClient.SqlTransaction T = con.BeginTransaction();
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con, T);
            if (C.ExecuteNonQuery() == 1)
            {
                SQL = "INSERT INTO ProjectArchive " +
                    "(ProjectID, Identifier, TransferDate) " +
                    "VALUES(" + ProjectID.ToString() + ", '" + DOI + "', getdate())";
            }
            else
                T.Rollback();
            return OK;
        }


        private static string DoiAgents(Microsoft.Data.SqlClient.SqlConnection con, int ProjectID, ref System.Collections.Generic.List<object> Creators)
        {
            System.Collections.Generic.List<object> Agents = new List<object>();
            System.Data.DataTable dtAgents = new System.Data.DataTable();
            string Publisher = "";
            string SQL = "SELECT A.AgentName, A.AgentURI, A.AgentType, R.AgentRole " +
                " FROM ProjectAgent AS A INNER JOIN ProjectAgentRole AS R ON A.ProjectID = R.ProjectID AND A.AgentName = R.AgentName AND A.AgentURI = R.AgentURI " +
                " WHERE A.ProjectID = " + ProjectID.ToString() +
                " ORDER BY A.AgentSequence ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
            ad.Fill(dtAgents);
            foreach (System.Data.DataRow R in dtAgents.Rows)
            {
                if (R["AgentRole"].ToString().ToLower() == "author")
                {
                    string URI = R["AgentURI"].ToString();
                    if (URI.Length > 0)
                    {
                        System.Collections.Generic.Dictionary<string, string> Values = Project.AgentValues(URI);
                        System.Collections.Generic.Dictionary<string, object> Agent = new Dictionary<string, object>();
                        Agent.Add("name", Values["AgentName"]);
                        switch (R["AgentType"].ToString().ToLower())
                        {
                            case "person":
                                Agent.Add("nameType", "Personal");
                                break;
                        }
                        Agent.Add("givenName", Values["GivenName"]);
                        Agent.Add("familyName", Values["InheritedName"]);
                    }
                }
                else if (R["AgentRole"].ToString().ToLower() == "publisher" && Publisher.Length == 0)
                {
                    Publisher = R["AgentName"].ToString();
                }
            }
            return Publisher;
        }

        private static System.Collections.Generic.Dictionary<string, string> AgentValues(string URI)
        {
            DiversityWorkbench.Agent A = new Agent(DiversityWorkbench.Settings.ServerConnection);
            System.Collections.Generic.Dictionary<string, string> AgentVals = A.UnitValues(URI);
            return AgentVals;
        }

        #endregion

        #region Properties

        //public override DiversityWorkbench.ServerConnection ServerConnection
        //{
        //    get { return _ServerConnection; }
        //    set 
        //    {
        //        try
        //        {
        //            if (value != null)
        //                this._ServerConnection = value;
        //            else
        //            {
        //                this._ServerConnection = new ServerConnection();
        //                this._ServerConnection.DatabaseServer = "127.0.0.1";
        //                this._ServerConnection.IsTrustedConnection = true;
        //                this._ServerConnection.DatabaseName = "DiversityProjects";
        //            }
        //            this._ServerConnection.ModuleName = "DiversityProjects";
        //            if (this._ServerConnection.DatabaseName.IndexOf("Projects") == -1)
        //                this._ServerConnection.DatabaseName = "DiversityProjects";
        //        }
        //        catch (Exception ex)
        //        {
        //            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //        }
        //    }
        //}

        #endregion

        #region Backlinks

        public override System.Windows.Forms.ImageList BackLinkImages(ModuleType CallingModule)
        {
            if (this._BackLinkImages == null)
            {
                this._BackLinkImages = this.BackLinkImages();
            }
            this._BackLinkImages.Images.Add("Project", DiversityWorkbench.Properties.Resources.Project);
            switch (CallingModule)
            {
                case ModuleType.Agents:
                    this._BackLinkImages.Images.Add("ProjectAgent", DiversityWorkbench.Properties.Resources.Agent); // 2
                    this._BackLinkImages.Images.Add("ProjectLicense", DiversityWorkbench.Properties.Resources.Document);       // 3
                    break;
                case ModuleType.References:
                    this._BackLinkImages.Images.Add("ProjectReference", DiversityWorkbench.Properties.Resources.Project); // 2
                    break;
            }
            return this._BackLinkImages;
        }

        public override System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BackLinkServerConnectionDomains(string URI, ModuleType CallingModule, bool IncludeEmpty = false, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BLD = new Dictionary<ServerConnection, List<BackLinkDomain>>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this.BackLinkConnections(ModuleType.Projects))
            {
                switch (CallingModule)
                {
                    case ModuleType.Agents:
                        System.Collections.Generic.List<BackLinkDomain> _A = this.BackLinkDomainAgent(KV.Value, URI);
                        if (_A.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _A);
                        break;
                    default:
                        System.Collections.Generic.List<BackLinkDomain> _L = this.BackLinkDomain(KV.Value, URI);
                        if (_L.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _L);
                        break;
                }
            }
            return BLD;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomain(ServerConnection SC, string URI, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.List<BackLinkDomain> BackLink = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain Link = this.BackLinkDomain(SC, URI, "Project", "ProjectReference", "ReferenceURI", 3); // Toni 20210728 ReferenceURI instead of ProjectURI, ProjectReference instead of Project
            if (Link.DtItems.Rows.Count > 0)
                BackLink.Add(Link);
            return BackLink;
        }

        private DiversityWorkbench.BackLinkDomain BackLinkDomain(ServerConnection SC, string URI, string DisplayText, string Table, string LinkColumn, int ImageKey, System.Collections.Generic.List<string> Restrictions = null)
        {
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain(DisplayText, Table, LinkColumn, ImageKey);
            string Prefix = "[" + SC.DatabaseName + "].dbo."; // Toni 20210727 database name added
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "]." + Prefix;
            string SQL = "SELECT T.ProjectID AS ID, P.Project AS DisplayText " +
                "FROM " + Prefix + Table + " AS T, " + Prefix + "Project_Core AS P " + // Toni 20210727 Project_Core instead of Project because Project include aan XML column that does not work with linked server
                "WHERE(T." + LinkColumn + " = '" + URI + "') AND T.ProjectID = P.ProjectID " +
                "GROUP BY T.ProjectID, P.Project ";
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

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainAgent(ServerConnection SC, string URI)
        {
            System.Collections.Generic.List<BackLinkDomain> Links = new List<BackLinkDomain>();
            // Agent
            DiversityWorkbench.BackLinkDomain A = this.BackLinkDomain(SC, URI, "Agent", "ProjectAgent", "AgentURI", 3);
            A.AddBacklinkColumn("ProjectAgent", "AgentName", "Displayed Name");
            if (A.DtItems.Rows.Count > 0)
                Links.Add(A);

            // License
            DiversityWorkbench.BackLinkDomain LH = this.BackLinkDomain(SC, URI, "License holder", "ProjectLicense", "LicenseHolderAgentURI", 4);
            LH.AddBacklinkColumn("ProjectLicense", "LicenseHolder", "Displayed Name");
            if (A.DtItems.Rows.Count > 0)
                Links.Add(LH);

            DiversityWorkbench.BackLinkDomain IH = this.BackLinkDomain(SC, URI, "IPR holder", "ProjectLicense", "IPRHolderAgentURI", 4);
            IH.AddBacklinkColumn("ProjectLicense", "IPRHolder", "Displayed Name");
            if (A.DtItems.Rows.Count > 0)
                Links.Add(IH);

            DiversityWorkbench.BackLinkDomain CH = this.BackLinkDomain(SC, URI, "Copyright holder", "ProjectLicense", "CopyrightHolderAgentURI", 4);
            CH.AddBacklinkColumn("ProjectLicense", "CopyrightHolder", "Displayed Name");
            if (A.DtItems.Rows.Count > 0)
                Links.Add(CH);

            return Links;
        }



        #endregion

        #region Metadata

        /// <summary>
        /// Metadata of the project
        /// </summary>
        /// <param name="ProjectID">The ID of the project</param>
        /// <returns>Dictionary containing the metadata of the project</returns>
        public System.Collections.Generic.Dictionary<string, string> Metadata(int ProjectID)
        {
            System.Collections.Generic.Dictionary<string, string> Data = new Dictionary<string, string>();
            Data.Add("ProjectID", ProjectID.ToString());
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                try
                {
                    string Prefix = this.ServerConnection.DatabaseName + ".dbo.";
                    if (this.ServerConnection.LinkedServer.Length > 0)
                        Prefix = "[" + this.ServerConnection.LinkedServer + "]." + Prefix;
                    string SQL = "";

                    // Project data
                    System.Data.DataTable dtProject = new System.Data.DataTable();
                    SQL = "SELECT Project, ProjectTitle, ProjectDescription, ProjectNotes, ProjectVersion, ProjectURL, ProjectType, PublicDescription, ProjectDescriptionType, EmbargoDate " +
                        "FROM Project WHERE ProjectID = " + ProjectID.ToString();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtProject, this.ServerConnection.ConnectionString);
                    if (dtProject.Rows.Count > 0)
                    {
                        Data.Add("Project", dtProject.Rows[0]["Project"].ToString());
                        Data.Add("ProjectTitle", dtProject.Rows[0]["ProjectTitle"].ToString());
                        Data.Add("ProjectDescription", dtProject.Rows[0]["ProjectDescription"].ToString());
                        Data.Add("PublicDescription", dtProject.Rows[0]["PublicDescription"].ToString());
                        Data.Add("ProjectURL", dtProject.Rows[0]["ProjectURL"].ToString());
                    }
                    else
                    {
                        Data.Add("Project", "");
                        Data.Add("ProjectTitle", "");
                        Data.Add("ProjectDescription", "");
                        Data.Add("PublicDescription", "");
                        Data.Add("ProjectURL", "");
                    }

                    // StableIdentifier
                    SQL = "SELECT CASE WHEN P.StableIdentifier IS NULL THEN PP.StableIdentifier ELSE P.StableIdentifier END AS StableIdentifier " +
                        "FROM " + Prefix + "ViewProject AS P LEFT OUTER JOIN " +
                        Prefix + "ViewProject AS PP ON P.ProjectParentID = PP.ProjectID " +
                        "WHERE (P.ProjectID = " + ProjectID.ToString() + ")";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this.ServerConnection.ConnectionString);
                    Data.Add("StableIdentifier", Result);

                    // ProjectLicense
                    System.Data.DataTable dtLicense = new System.Data.DataTable();
                    SQL = "SELECT DisplayText AS License, LicenseType, LicenseURI, LicenseDetails " +
                        "FROM " + Prefix + "ProjectLicense L " +
                        "WHERE EXISTS(SELECT MIN(F.LicenseID) FROM " + Prefix + "ProjectLicense F GROUP BY F.ProjectID HAVING  L.LicenseID = MIN(F.LicenseID))" +
                        "AND (ProjectID = " + ProjectID.ToString() + ")";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtLicense, this.ServerConnection.ConnectionString);
                    if (dtLicense.Rows.Count > 0)
                    {
                        Data.Add("License", dtLicense.Rows[0]["License"].ToString());
                        Data.Add("LicenseType", dtLicense.Rows[0]["LicenseType"].ToString());
                        Data.Add("LicenseURI", dtLicense.Rows[0]["LicenseURI"].ToString());
                        Data.Add("LicenseDetails", dtLicense.Rows[0]["LicenseDetails"].ToString());
                    }
                    else
                    {
                        Data.Add("License", "");
                        Data.Add("LicenseType", "");
                        Data.Add("LicenseURI", "");
                        Data.Add("LicenseDetails", "");
                    }

                    // Agents
                    this.AgentData(ref Data, "Content contact", ProjectID, this.ServerConnection.ConnectionString);
                    this.AgentData(ref Data, "Technical contact", ProjectID, this.ServerConnection.ConnectionString);
                    this.AgentData(ref Data, "Source institution", ProjectID, this.ServerConnection.ConnectionString);
                    this.AgentData(ref Data, "Data Owner", ProjectID, this.ServerConnection.ConnectionString);
                    this.AgentData(ref Data, "Data Owner Contact", ProjectID, this.ServerConnection.ConnectionString);

                    // Settings
                    SQL = "SELECT ProjectSetting AS Setting, " +
                        "case when isdate(value) = 1 and ProjectSetting like '%Date%' then convert(varchar(20), cast(value as datetime), 126) else Value end as Value " +
                        "FROM " + Prefix + "SettingsForProject(" + ProjectID.ToString() + ", 'ABCD | %', '.', 2)";
                    System.Data.DataTable dtSetting = new System.Data.DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtSetting, this.ServerConnection.ConnectionString);
                    foreach (System.Data.DataRow R in dtSetting.Rows)
                    {
                        Data.Add(R[0].ToString(), R[1].ToString());
                    }

                    // Descriptor
                    SQL = "SELECT E.DisplayText AS DescriptorType, D.[Content] AS DescriptorContent " +
                        "FROM ProjectDescriptor AS D INNER JOIN " +
                        "ProjectDescriptorElement AS E ON D.ElementID = E.ElementID WHERE D.ProjectID = " + ProjectID.ToString();
                    SQL += " ORDER BY E.DisplayText, D.[Content]";
                    System.Data.DataTable dtDescriptor = new System.Data.DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtDescriptor, this.ServerConnection.ConnectionString);
                    string Type = "";
                    int i = 1;
                    foreach (System.Data.DataRow R in dtDescriptor.Rows)
                    {
                        if (Type.Length == 0)
                        {
                            Type = R[0].ToString();
                        }
                        else
                        {
                            if (Type == R[0].ToString())
                                i++;
                            else
                            {
                                Type = R[0].ToString();
                                i = 1;
                            }
                        }
                        Data.Add(R[0].ToString() + " " + i.ToString(), R[1].ToString());
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }

            return Data;
        }

        private void AgentData(ref System.Collections.Generic.Dictionary<string, string> Data, string Role, int ProjectID, string connectionString)
        {
            try
            {
                string SQL = "SELECT A.AgentName, A.AgentURI " +
                    "FROM ProjectAgent AS A INNER JOIN " +
                    "ProjectAgentRole AS R ON A.ProjectAgentID = R.ProjectAgentID " +
                    "WHERE(R.AgentRole = N'" + Role + "') AND (A.ProjectID = " + ProjectID.ToString() + ") " +
                    "ORDER BY A.AgentSequence";
                System.Data.DataTable dtAgent = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtAgent, connectionString);
                if (dtAgent.Rows.Count > 0)
                {
                    for (int i = 1; i <= dtAgent.Rows.Count; i++)
                    {
                        Data.Add(Role + " " + i.ToString() + " Name", dtAgent.Rows[0][0].ToString());
                        Data.Add(Role + " " + i.ToString() + " URI", dtAgent.Rows[0][1].ToString());
                        this.AgentDetails(ref Data, dtAgent.Rows[0][1].ToString(), Role, i);
                    }
                }
                else
                {
                    Data.Add(Role + " 1 Name", "");
                    Data.Add(Role + " 1 URI", "");
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void AgentDetails(ref System.Collections.Generic.Dictionary<string, string> Data, string URI, string Role, int AgentSequence = 1)
        {
            try
            {
                DiversityWorkbench.ServerConnection SC = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);
                DiversityWorkbench.Agent A = new Agent(SC);
                System.Collections.Generic.Dictionary<string, string> D = A.UnitValues(URI);
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in D)
                {
                    if (!Data.ContainsKey(Role + " " + AgentSequence.ToString() + " " + KV.Key))
                        Data.Add(Role + " " + AgentSequence.ToString() + " " + KV.Key, KV.Value);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

    }
}
