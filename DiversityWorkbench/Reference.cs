using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class Reference : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {

        #region Parameter
        private DiversityWorkbench.QueryCondition _QueryConditionProject;
        private System.Collections.Generic.Dictionary<string, string> _ServiceList;
        #endregion

        #region Construction

        public Reference(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
            this._FeatureList = new List<ClientFeature>();
            this._FeatureList.Add(ClientFeature.SingleItem);
            //this._FeatureList.Add(ClientFeature.HtmlUnitValues);
        }

        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityReferences"; }

        public override System.Collections.Generic.Dictionary<string, string> AdditionalServicesOfModule()
        {
            if (this._ServiceList == null)
            {
                this._ServiceList = new Dictionary<string, string>();
                _ServiceList.Add("CacheDB", "Cache database");
                //_ServiceList.Add("RLL", "Recent Literature on Lichens");
            }
            return _ServiceList;
        }
        // Ariane exclude Webserice.. for .NET 8
        //protected override System.Collections.Generic.Dictionary<string, string> AdditionalServiceURIsOfModule()
        //{
        //    Ariane exclude Webserice.. for .NET 8

        //   System.Collections.Generic.Dictionary<string, string> _Add = new Dictionary<string, string>();
        //   _Add.Add("RLL", DiversityWorkbench.WebserviceRLL.UriBaseWeb);
        //    return _Add;
        //}

        public override System.Collections.Generic.List<DiversityWorkbench.DatabaseService> DatabaseServices()
        {
            System.Collections.Generic.List<DiversityWorkbench.DatabaseService> ds = new List<DatabaseService>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()].ServerConnectionList())
            {
                if (KV.Value.ConnectionIsValid)
                {
                    DiversityWorkbench.DatabaseService d = new DatabaseService(KV.Key);
                    d.IsWebservice = false;
                    ds.Add(d);
                }
            }
            // Ariane exclude Webserice.. for .NET 8
            //DiversityWorkbench.DatabaseService RLL = new DatabaseService("RLL");
            //RLL.IsWebservice = true;
            //RLL.DisplayUnitdataInTreeview = false;
            //RLL.WebService = Webservice.WebServices.RLL;
            //ds.Add(RLL);

            return ds;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            //string Prefix = "";
            //if (this._ServerConnection.LinkedServer.Length > 0)
            //    Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            //else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT U.BaseURL + CAST(R.RefID AS varchar) AS _URI, U.BaseURL + CAST(R.RefID AS varchar) AS Link, R.RefDescription_Cache AS _DisplayText, " +
                    " RefId AS ID, R.RefType, R.RefDescription_Cache AS [Description], R.Title, " +
                    " (CASE WHEN R.SourceRefID IS NULL THEN R.SourceTitle ELSE (SELECT Title FROM " + Prefix + "ReferenceTitle WHERE RefID = R.SourceRefID) END) AS [Source title], " +
                    " R.SeriesTitle AS [Series title], " +
                    " (CASE WHEN P.FullName IS NULL THEN R.Periodical ELSE P.FullName END) AS [Periodical], " +
                    " R.DateYear AS [Publication year], R.Volume, R.Issue, R.Pages, R.Edition, " +
                    " R.Publisher + (CASE WHEN R.PublPlace <> '' THEN ', ' + R.PublPlace ELSE '' END) AS [Publisher], R.ISSN_ISBN AS [ISSN/ISBN] " +
                    "FROM " + Prefix + "ReferenceTitle AS R " +
                    "LEFT JOIN " + Prefix + "ReferencePeriodical AS P ON P.Abbreviation = R.Periodical " +
                    "INNER JOIN " + Prefix + "ViewBaseURL AS U ON 1 = 1 " +
                    "WHERE R.RefID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT Name AS Author " +
                    "FROM " + Prefix + "ReferenceRelator " +
                    "WHERE RefID = " + ID.ToString() +
                    "AND Role = 'Aut' ORDER BY Sequence";
                this.getDataFromTable(SQL, ref Values, true); // Use semicolon if commas are present in data

                SQL = "SELECT Name AS Editor " +
                    "FROM " + Prefix + "ReferenceRelator " +
                    "WHERE RefID = " + ID.ToString() +
                    "AND Role = 'Edt' ORDER BY Sequence";
                this.getDataFromTable(SQL, ref Values, true); // Use semicolon if commas are present in data

                SQL = "SELECT Name AS Serieseditor " +
                    "FROM " + Prefix + "ReferenceRelator " +
                    "WHERE RefID = " + ID.ToString() +
                    "AND Role = 'Sed' ORDER BY Sequence";
                this.getDataFromTable(SQL, ref Values, true); // Use semicolon if commas are present in data

                SQL = "SELECT Content AS Keyword " +
                    "FROM " + Prefix + "ReferenceDescriptor " +
                    "WHERE RefID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values, true); // Use semicolon if commas are present in data

                if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
                this._UnitValues.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
                {
                    this._UnitValues.Add(P.Key, P.Value);
                }
            }
            return Values;
        }

        public override void GetAdditionalUnitValues(System.Collections.Generic.Dictionary<string, string> UnitValues)
        {
            try
            {
                // Get ID from unit values
                int ID = 0;
                if (UnitValues != null && UnitValues.ContainsKey("ID"))
                    int.TryParse(UnitValues["ID"], out ID);
                else if (this._UnitValues != null && this._UnitValues.ContainsKey("ID"))
                    int.TryParse(this._UnitValues["ID"], out ID);

                if (ID > 0)
                {
                    string SQL = $"SELECT [WebLinks] AS [Web Link]" +
                                 $", [LinkToPDF] AS [PDF Link]" +
                                 $", [LinkToFullText] AS [Text Link]" +
                                 $", [RelatedLinks] AS [Related Link]" +
                                 $", [LinkToImages] AS [Image Link]" +
                                 $" FROM {Prefix}ReferenceTitle WHERE RefID = {ID}";
                    this.getDataFromTable(SQL, ref UnitValues);

                    //SQL = $"SELECT [LinkToPDF] AS [PDF Link] FROM {Prefix}ReferenceTitle WHERE RefID = {ID}";
                    //this.getDataFromTable(SQL, ref UnitValues);

                    //SQL = $"SELECT [LinkToFullText] AS [Text Link] FROM {Prefix}ReferenceTitle WHERE RefID = {ID}";
                    //this.getDataFromTable(SQL, ref UnitValues);

                    //SQL = $"SELECT [RelatedLink] AS [Related Link] FROM {Prefix}ReferenceTitle WHERE RefID = {ID}";
                    //this.getDataFromTable(SQL, ref UnitValues);

                    //SQL = $"SELECT [ImageLink] AS [Image Link] FROM {Prefix}ReferenceTitle WHERE RefID = {ID}";
                    //this.getDataFromTable(SQL, ref UnitValues);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public override string HtmlUnitValues(Dictionary<string, string> UnitValues)
        {
            // Read possibly missing additional data
            this.GetAdditionalUnitValues(UnitValues);

            // String builder for result
            StringBuilder sb = new StringBuilder();

            try
            {
                // Get ID
                int ID = 0;
                if (UnitValues != null && UnitValues.ContainsKey("ID"))
                    int.TryParse(UnitValues["ID"], out ID);

                if (ID > 0)
                {
                    // Start XML writer
                    using (System.Xml.XmlWriter W = System.Xml.XmlWriter.Create(sb))
                    {
                        // Save title
                        string title = UnitValues.ContainsKey("Title") && UnitValues["Title"].Length > 0 ? UnitValues["Title"] : UnitValues["_DisplayText"];

                        // Start HTML document
                        W.WriteStartElement("html");
                        W.WriteString("\r\n");
                        W.WriteStartElement("head");
                        W.WriteElementString("title", title);
                        W.WriteEndElement();//head
                        W.WriteString("\r\n");
                        W.WriteStartElement("body");
                        W.WriteString("\r\n");

                        // Write reference title
                        W.WriteStartElement("h2");
                        W.WriteStartElement("font");
                        W.WriteAttributeString("face", "Verdana");
                        DiversityWorkbench.Description.WriteXmlString(W, title);
                        W.WriteEndElement();//font
                        W.WriteEndElement();//h2

                        // Start table
                        W.WriteStartElement("table");
                        //W.WriteAttributeString("width", "900");
                        W.WriteAttributeString("border", "0");
                        W.WriteAttributeString("cellpadding", "1");
                        W.WriteAttributeString("cellspacing", "0");
                        W.WriteAttributeString("class", "small");
                        W.WriteAttributeString("style", "margin-left:0px");

                        foreach (KeyValuePair<string, string> item in UnitValues)
                        {
                            // Skip irrelevant entries
                            if (item.Key.StartsWith("_"))
                                continue;

                            // Insert unit value
                            if (item.Value.Trim() != "")
                            {
                                if (item.Key != "Link" && item.Key.EndsWith("Link"))
                                {
                                    string titleText = item.Key;
                                    string compText = item.Value.ToLower().Trim();
                                    string[] links = item.Value.Split(';');

                                    // Write all links
                                    foreach (string link in links)
                                    {
                                        // New table row
                                        W.WriteStartElement("tr");
                                        //W.WriteAttributeString("style", "padding-top:10px; padding-bottom:20px");

                                        // Write first column <td width=_ColumnWidth align="right">
                                        W.WriteStartElement("td");
                                        W.WriteAttributeString("width", "150");
                                        W.WriteAttributeString("align", "right");
                                        W.WriteAttributeString("valign", "top");
                                        W.WriteAttributeString("style", "padding-right:5px");
                                        W.WriteStartElement("font");
                                        W.WriteAttributeString("face", "Verdana");
                                        W.WriteAttributeString("size", "2");
                                        DiversityWorkbench.Description.WriteXmlString(W, string.Format("<b>{0}</b>", titleText));
                                        W.WriteEndElement();//font
                                        W.WriteEndElement();//td
                                        titleText = "";

                                        // Write second column <td style="padding-left:5px">
                                        W.WriteStartElement("td");
                                        W.WriteAttributeString("style", "padding-left:5px");
                                        W.WriteStartElement("font");
                                        W.WriteAttributeString("face", "Verdana");
                                        W.WriteAttributeString("size", "2");
                                        if (compText.StartsWith("https://") || compText.StartsWith("http://") || compText.StartsWith("file:///"))
                                        {
                                            W.WriteStartElement("a");
                                            W.WriteAttributeString("href", link);
                                            DiversityWorkbench.Description.WriteXmlString(W, link);
                                            W.WriteEndElement();//a
                                        }
                                        else
                                            DiversityWorkbench.Description.WriteXmlString(W, link);
                                        W.WriteEndElement();//font
                                        W.WriteEndElement();//td
                                        W.WriteEndElement();//tr
                                    }
                                }
                                else
                                {
                                    // New table row
                                    W.WriteStartElement("tr");
                                    //W.WriteAttributeString("style", "padding-top:10px; padding-bottom:20px");

                                    // Write first column <td width=_ColumnWidth align="right">
                                    W.WriteStartElement("td");
                                    W.WriteAttributeString("width", "150");
                                    W.WriteAttributeString("align", "right");
                                    W.WriteAttributeString("valign", "top");
                                    W.WriteAttributeString("style", "padding-right:5px");
                                    W.WriteStartElement("font");
                                    W.WriteAttributeString("face", "Verdana");
                                    W.WriteAttributeString("size", "2");
                                    DiversityWorkbench.Description.WriteXmlString(W, string.Format("<b>{0}</b>", item.Key));
                                    W.WriteEndElement();//font
                                    W.WriteEndElement();//td

                                    // Write second column <td style="padding-left:5px">
                                    W.WriteStartElement("td");
                                    W.WriteAttributeString("style", "padding-left:5px");
                                    W.WriteStartElement("font");
                                    W.WriteAttributeString("face", "Verdana");
                                    W.WriteAttributeString("size", "2");
                                    DiversityWorkbench.Description.WriteXmlString(W, item.Value);
                                    W.WriteEndElement();//font
                                    W.WriteEndElement();//td
                                    W.WriteEndElement();//tr
                                }
                            }
                        }

                        W.WriteEndElement();//body
                        W.WriteEndElement();//html
                        W.WriteEndDocument();
                        W.Flush();
                        W.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return sb.ToString();
        }

        public override bool DoesExist(int ID)
        {
            bool Exists = false;
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                try
                {
                    //string Prefix = "";
                    //if (this._ServerConnection.LinkedServer.Length > 0)
                    //    Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
                    //else
                    //    Prefix = "dbo.";

                    string SQL = "SELECT COUNT(*) FROM " + Prefix + MainTable() +
                        " AS T WHERE (T.RefID = " + ID.ToString() + ")";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    if (int.Parse(C.ExecuteScalar()?.ToString()) > 0)
                        Exists = true;
                    con.Close();
                    con.Dispose();
                }
                catch (Exception ex)
                {
                    Exists = base.DoesExist(ID);
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return Exists;
        }

        public string MainTable() { return "ReferenceTitle"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[2];
            QueryDisplayColumns[0].DisplayText = "Description";
            QueryDisplayColumns[0].DisplayColumn = "RefDescription_Cache";
            QueryDisplayColumns[0].OrderColumn = "RefDescription_Cache";
            QueryDisplayColumns[0].IdentityColumn = "RefID";
            QueryDisplayColumns[0].TableName = "ReferenceTitle_Core";

            QueryDisplayColumns[1].DisplayText = "Title";
            QueryDisplayColumns[1].DisplayColumn = "Title";
            QueryDisplayColumns[1].OrderColumn = "Title";
            QueryDisplayColumns[1].IdentityColumn = "RefID";
            QueryDisplayColumns[1].TableName = "ReferenceTitle_Core";
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            //string Prefix = "";
            //if (this._ServerConnection.LinkedServer.Length > 0)
            //    Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            //else Prefix = "dbo.";

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            string ConnectionString = this._ServerConnection.ConnectionString;
            if (ConnectionString.Length == 0)
                ConnectionString = DiversityWorkbench.Settings.ConnectionString;

            if (ConnectionString.Length == 0)
            {
                if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList().ContainsKey("DiversityReferences"))
                {
                    if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnectionList().Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityReferences"].ServerConnectionList())
                        {
                            ConnectionString = KV.Value.ConnectionString;
                            if (ConnectionString.Length > 0) break;
                        }
                    }
                }
            }
            if (this._ServerConnection.ModuleName == DiversityWorkbench.Settings.ServerConnection.ModuleName &&
                this._ServerConnection.DatabaseName != DiversityWorkbench.Settings.ServerConnection.DatabaseName)
                ConnectionString = DiversityWorkbench.Settings.ConnectionString;


            #region PROJECT

            System.Data.DataTable dtProject = new System.Data.DataTable();
            string SQL = "SELECT ProjectID AS [Value], Project AS Display " +
                "FROM " + Prefix + "ProjectList " +
                "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
            if (ConnectionString.Length > 0)
            {
                try { ad.Fill(dtProject); }
                catch { }
            }
            if (dtProject.Rows.Count > 1)
            {
                dtProject.Clear();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ProjectID AS [Value], Project AS Display " +
                    "FROM " + Prefix + "ProjectList " +
                    "ORDER BY Display";
                ad.SelectCommand.CommandText = SQL;
                try { ad.Fill(dtProject); }
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
            this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "ReferenceProject", "RefID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
            QueryConditions.Add(this._QueryConditionProject);

            #endregion

            #region Title

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "Title");
            DiversityWorkbench.QueryCondition qTitle = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "Title", "Reference", "Title", "Reference title", Description);
            QueryConditions.Add(qTitle);

            System.Data.DataTable dtRefType = new System.Data.DataTable();
            SQL = "SELECT NULL  AS [Value], NULL AS Display UNION " +
                "SELECT DISTINCT RefType AS [Value], Label AS Display " +
                "FROM " + Prefix + "Ref_Type_Enum " +
                "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter adType = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
            if (ConnectionString.Length > 0)
            {
                try { adType.Fill(dtRefType); }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "RefType");
            DiversityWorkbench.QueryCondition qRefType = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "RefType", "Reference", "Type", "Reference type", Description, dtRefType, false);
            QueryConditions.Add(qRefType);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "RefID");
            DiversityWorkbench.QueryCondition qRefID = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "RefID", "Reference", "ID", "Reference ID", Description, false, false, true, false);
            QueryConditions.Add(qRefID);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "RefDescription_Cache");
            DiversityWorkbench.QueryCondition qDescription = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "RefDescription_Cache", "Reference", "Description", "Description", Description);
            QueryConditions.Add(qDescription);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "SourceTitle");
            DiversityWorkbench.QueryCondition qSourceTitle = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "SourceTitle", "Reference", "Source tit.", "Source title", Description);
            QueryConditions.Add(qSourceTitle);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "SourceRefID");
            DiversityWorkbench.QueryCondition qSourceRefID = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "SourceRefID", "SourceRefID", "Reference", "Source ID", "Source ID", Description, false, false, true, false);
            QueryConditions.Add(qSourceRefID);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "ReplaceWithRefID");
            DiversityWorkbench.QueryCondition qReplaceID = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "ReplaceWithRefID", "ReplaceWithRefID", "Reference", "Replace ID", "Replace ID", Description, false, false, true, false);
            QueryConditions.Add(qReplaceID);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "SeriesTitle");
            DiversityWorkbench.QueryCondition qSeriesTitle = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "SeriesTitle", "Reference", "Series", "SeriesTitle", Description);
            QueryConditions.Add(qSeriesTitle);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "Periodical");
            DiversityWorkbench.QueryCondition qPeriodical = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "Periodical", "Reference", "Periodical", "Periodical", Description);
            QueryConditions.Add(qPeriodical);

            System.Data.DataTable dtSource = new System.Data.DataTable();
            SQL = "SELECT NULL  AS [Value], NULL AS Display UNION " +
                "SELECT DISTINCT ImportedFrom AS [Value], ImportedFrom AS Display " +
                "FROM " + Prefix + "ReferenceTitle " +
                "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);//DiversityWorkbench.Settings.ConnectionString);
            if (ConnectionString.Length > 0)
            {
                try { a.Fill(dtSource); }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "ImportedFrom");
            DiversityWorkbench.QueryCondition qImportedFrom = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "ImportedFrom", "Source", "Import", "Source", Description, dtSource, false);
            QueryConditions.Add(qImportedFrom);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "ImportedID");
            DiversityWorkbench.QueryCondition qImportedID = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "ImportedID", "Source", "Import ID", "Source ID", Description);
            QueryConditions.Add(qImportedID);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "DateYear");
            DiversityWorkbench.QueryCondition qDate = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "DateYear", "Reference", "Date", "Date", Description, "DateDay", "DateMonth", "DateYear");
            QueryConditions.Add(qDate);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "DateYear");
            DiversityWorkbench.QueryCondition qYear = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "DateYear", "Reference", "Year", "Year", Description);
            QueryConditions.Add(qYear);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "Publisher");
            DiversityWorkbench.QueryCondition qPublisher = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "Publisher", "Reference", "Publisher", "Publisher", Description);
            QueryConditions.Add(qPublisher);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "Problem");
            DiversityWorkbench.QueryCondition qProblem = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "Problem", "Reference", "Problem", "Problem", Description);
            QueryConditions.Add(qProblem);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "Miscellaneous1");
            DiversityWorkbench.QueryCondition qMiscellaneous1 = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "Miscellaneous1", "Reference", "Misc.1", "Miscellaneous 1", Description);
            QueryConditions.Add(qMiscellaneous1);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "Miscellaneous2");
            DiversityWorkbench.QueryCondition qMiscellaneous2 = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "Miscellaneous2", "Reference", "Misc.2", "Miscellaneous 2", Description);
            QueryConditions.Add(qMiscellaneous2);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "Miscellaneous3");
            DiversityWorkbench.QueryCondition qMiscellaneous3 = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "Miscellaneous3", "Reference", "Misc.3", "Miscellaneous 3", Description);
            QueryConditions.Add(qMiscellaneous3);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "UserDef1");
            DiversityWorkbench.QueryCondition qUserDef1 = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "UserDef1", "Reference", "User def.1", "User defined 1", Description);
            QueryConditions.Add(qUserDef1);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "UserDef2");
            DiversityWorkbench.QueryCondition qUserDef2 = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "UserDef2", "Reference", "User def.2", "User defined 2", Description);
            QueryConditions.Add(qUserDef2);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "UserDef3");
            DiversityWorkbench.QueryCondition qUserDef3 = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "UserDef3", "Reference", "User def.3", "User defined 3", Description);
            QueryConditions.Add(qUserDef3);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "UserDef4");
            DiversityWorkbench.QueryCondition qUserDef4 = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "UserDef5", "Reference", "User def.4", "User defined 4", Description);
            QueryConditions.Add(qUserDef4);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "UserDef5");
            DiversityWorkbench.QueryCondition qUserDef5 = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "UserDef5", "Reference", "User def.5", "User defined 5", Description);
            QueryConditions.Add(qUserDef5);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "Language");
            DiversityWorkbench.QueryCondition qLanguage = new DiversityWorkbench.QueryCondition(true, "ReferenceTitle_Core", "RefID", "Language", "Reference", "Language", "Language", Description);
            QueryConditions.Add(qLanguage);

            #region Log
            // User table for log queries
            System.Data.DataTable dtUser = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT CAST(ID AS NVARCHAR), CombinedNameCache " +
                  "FROM " + Prefix + "UserProxy " +
                  "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter aUser = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
            if (ConnectionString.Length > 0)
            {
                try { aUser.Fill(dtUser); }
                catch { }
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "LogInsertedBy");
            DiversityWorkbench.QueryCondition qDescInsertedBy = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "LogInsertedBy", "Reference", "Creat. by", "The user that created the dataset", Description, dtUser, false);
            QueryConditions.Add(qDescInsertedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "LogInsertedWhen");
            DiversityWorkbench.QueryCondition qDescInsertedWhen = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "LogInsertedWhen", "Reference", "Creat. date", "The date when the dataset was created", Description, QueryCondition.QueryTypes.DateTime);
            QueryConditions.Add(qDescInsertedWhen);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "LogUpdatedBy");
            DiversityWorkbench.QueryCondition qDescUpdatedBy = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "LogUpdatedBy", "Reference", "Chg. by", "The last user that changed the dataset", Description, dtUser, false);
            QueryConditions.Add(qDescUpdatedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceTitle", "LogUpdatedWhen");
            DiversityWorkbench.QueryCondition qDescUpdatedWhen = new DiversityWorkbench.QueryCondition(false, "ReferenceTitle_Core", "RefID", "LogUpdatedWhen", "Reference", "Chg. date", "The last date when the dataset was changed", Description, QueryCondition.QueryTypes.DateTime);
            QueryConditions.Add(qDescUpdatedWhen);
            #endregion

            #endregion

            #region Authors

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceRelator", "Name");
            DiversityWorkbench.QueryCondition qAuthor = new DiversityWorkbench.QueryCondition(true, "ReferenceRelator", "RefID", "Name", "Authors", "Author", "Author", Description);
            QueryConditions.Add(qAuthor);

            #endregion

            #region Keywords

            System.Data.DataTable dtReferenceCollection = new System.Data.DataTable();
            SQL = "SELECT NULL  AS [Value], NULL AS Display UNION " +
                  "SELECT DISTINCT [Content] AS [Value], [Content] AS Display " +
                  "FROM " + Prefix + "ReferenceDescriptor_CollectionWithElement " +
                  "WHERE (ElementLabel = 'ReferenceCollection') " +
                  "ORDER BY Display";
            a.SelectCommand.CommandText = SQL;
            if (ConnectionString.Length > 0)
            {
                try { a.Fill(dtReferenceCollection); }
                catch { }
            }
            Description = "Collection of reference titels";
            DiversityWorkbench.QueryCondition qReferenceCollection = new DiversityWorkbench.QueryCondition(true, "ReferenceDescriptor_CollectionWithElement", "RefID", "[Content]", "Keywords and marker", "Collection", "Reference collection", Description, dtReferenceCollection, false);
            QueryConditions.Add(qReferenceCollection);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceDescriptor", "Content");
            DiversityWorkbench.QueryCondition qKeyword = new DiversityWorkbench.QueryCondition(true, "ReferenceDescriptor", "RefID", "Content", "Keywords and marker", "Keyword", "Keyword", Description);
            QueryConditions.Add(qKeyword);
            //DiversityWorkbench.QueryCondition qKeyword2 = new DiversityWorkbench.QueryCondition(true, "ReferenceDescriptor", "RefID", "Content", "Keywords and marker", "Keyword", "Keyword", Description);
            //QueryConditions.Add(qKeyword2);
            //DiversityWorkbench.QueryCondition qKeyword3 = new DiversityWorkbench.QueryCondition(false, "ReferenceDescriptor", "RefID", "Content", "Keywords and marker", "Keyword", "Keyword", Description);
            //QueryConditions.Add(qKeyword3);
            //DiversityWorkbench.QueryCondition qKeyword4 = new DiversityWorkbench.QueryCondition(false, "ReferenceDescriptor", "RefID", "Content", "Keywords and marker", "Keyword", "Keyword", Description);
            //QueryConditions.Add(qKeyword4); More than one keyword does not work

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferencePrivateDescriptor", "Content");
            DiversityWorkbench.QueryCondition qMarker = new DiversityWorkbench.QueryCondition(true, "ReferencePrivateDescriptor", "RefID", "Content", "Keywords and marker", "Marker", "Marker", Description);
            QueryConditions.Add(qMarker);

            #endregion

            #region Abstract and notes

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceNote", "Content");
            DiversityWorkbench.QueryCondition qAbstract = new DiversityWorkbench.QueryCondition(true, "ReferenceNote", "RefID", "Content", "Abstract and notes", "Abstract", "Abstract", Description);
            QueryConditions.Add(qAbstract);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferencePrivateNote", "Content");
            DiversityWorkbench.QueryCondition qNotes = new DiversityWorkbench.QueryCondition(true, "ReferencePrivateNote", "RefID", "Content", "Abstract and notes", "Notes", "Notes", Description);
            QueryConditions.Add(qNotes);

            #endregion

            #region Availability

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceAvailability", "FilingCode");
            DiversityWorkbench.QueryCondition qFilingCode = new DiversityWorkbench.QueryCondition(true, "ReferenceAvailability", "RefID", "FilingCode", "Availability", "Code", "Filing code", Description);
            QueryConditions.Add(qFilingCode);

            Description = DiversityWorkbench.Functions.ColumnDescription("ReferenceAvailability", "RequestDate");
            DiversityWorkbench.QueryCondition qRequestDate = new DiversityWorkbench.QueryCondition(false, "ReferenceAvailability", "RefID", "RequestDate", "Availability", "Date", "Request date", Description);
            QueryConditions.Add(qRequestDate);

            #endregion

            return QueryConditions;
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList()
        {
            //string Prefix = "";
            //if (this._ServerConnection.LinkedServer.Length > 0)
            //    Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            //else Prefix = "dbo.";

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList = new List<QueryCondition>();
            //TODO: _QueryConditionProject ist leer - unklar warum - wenn gelöst kann folgendes if(..){..} entfallen
            if (this._QueryConditionProject.Column == null)
            {
                System.Data.DataTable dtProject = new System.Data.DataTable();
                string SQL = "SELECT ProjectID AS [Value], Project AS Display " +
                    "FROM " + Prefix + "ProjectList " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
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
                this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "CollectionProject", "CollectionSpecimenID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
            }
            PredefinedQueryPersistentConditionList.Add(this._QueryConditionProject);
            return PredefinedQueryPersistentConditionList;
        }

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
                case ModuleType.Agents:
                    this._BackLinkImages.Images.Add("ReferenceRelator", DiversityWorkbench.Properties.Resources.Agent);
                    break;
                case ModuleType.Collection:
                    break;
                case ModuleType.Descriptions:
                    break;
                case ModuleType.Gazetteer:
                    break;
                case ModuleType.SamplingPlots:
                    break;
                case ModuleType.ScientificTerms:
                    break;
                case ModuleType.TaxonNames:
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
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this.BackLinkConnections(ModuleType.References))
            {
                System.Collections.Generic.List<BackLinkDomain> _L = this.BackLinkDomainList(KV.Value, URI, CallingModule, Restrictions);
                if (_L.Count > 0 || IncludeEmpty)
                    BLD.Add(KV.Value, _L);
            }
            return BLD;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainList(ServerConnection SC, string URI, ModuleType CallingModule, System.Collections.Generic.List<string> Restrictions)
        {
            System.Collections.Generic.List<BackLinkDomain> Terms = new List<BackLinkDomain>();
            switch (CallingModule)
            {
                case ModuleType.Agents:
                    DiversityWorkbench.BackLinkDomain refDomain = this.BackLinkDomainAgent(SC, URI, "Reference relator", "ReferenceRelator", "AgentURI", 2, Restrictions);
                    if (refDomain.DtItems.Rows.Count > 0)
                        Terms.Add(refDomain);
                    break;
                case ModuleType.Collection:
                    break;
                case ModuleType.Descriptions:
                    break;
                case ModuleType.Exsiccatae:
                    break;
                case ModuleType.Gazetteer:
                    break;
                case ModuleType.Projects:
                    DiversityWorkbench.BackLinkDomain prjDomain = this.BackLinkDomainProject(SC, URI, "Reference project", "ProjectProxy", "ProjectURI", 2, Restrictions);
                    if (prjDomain.DtItems.Rows.Count > 0)
                        Terms.Add(prjDomain);
                    break;
                case ModuleType.References:
                    break;
                case ModuleType.SamplingPlots:
                    break;
                case ModuleType.ScientificTerms:
                    break;
                case ModuleType.TaxonNames:
                    break;
                default:
                    break;
            }
            return Terms;
        }

        private DiversityWorkbench.BackLinkDomain BackLinkDomainAgent(ServerConnection SC, string URI, string DisplayText, string Table, string LinkColumn, int ImageKey, System.Collections.Generic.List<string> Restrictions = null)
        {
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain(DisplayText, Table, LinkColumn, ImageKey);
            string Prefix = "[" + SC.DatabaseName + "].dbo.";
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "]." + Prefix;
            string SQL = "SELECT CASE WHEN T.RefDescription_Cache <> '' THEN T.RefDescription_Cache ELSE T.Title END AS DisplayText, T.RefID AS ID " +
                         "(SELECT COUNT(*) FROM " + Prefix + "[ProjectUser] U WHERE U.LoginName = USER_NAME() AND U.ProjectID = P.ProjectID) AS AccessCount " +
                         "FROM " + Prefix + Table + " AS R " +
                         "INNER JOIN " + Prefix + "[ReferenceTitle] AS T ON R.RefID = T.RefID " +
                         "INNER JOIN " + Prefix + "[ReferenceProject] AS P ON T.RefID = P.RefID " +
                         "WHERE(R." + LinkColumn + " = '" + URI + "') ";
            if (Restrictions != null)
            {
                foreach (string R in Restrictions)
                {
                    SQL += " AND " + R;
                }
            }
            SQL += " GROUP BY D.id ";
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

        private DiversityWorkbench.BackLinkDomain BackLinkDomainProject(ServerConnection SC, string URI, string DisplayText, string Table, string LinkColumn, int ImageKey, System.Collections.Generic.List<string> Restrictions = null)
        {
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain(DisplayText, Table, LinkColumn, ImageKey);
            string Prefix = "dbo.";
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "]." + Prefix;
            string SQL = "SELECT 'First of ' + CAST(COUNT(*) as varchar) + ' references' AS DisplayText, MIN(T.RefId) AS ID , " +
                         "(SELECT COUNT(*) FROM " + Prefix + "[ProjectUser] U WHERE U.LoginName = USER_NAME() AND U.ProjectID = X.ProjectID) AS AccessCount " +
                         "FROM " + Prefix + "ProjectProxy AS X " +
                         "INNER JOIN " + Prefix + "[ReferenceProject] AS P ON P.ProjectID = X.ProjectID " +
                         "INNER JOIN " + Prefix + "[ReferenceTitle] AS T ON T.RefID = P.RefID " +
                         "WHERE(X.ProjectURI = '" + URI + "') " +
                         "GROUP BY X.Project, X.ProjectURI, X.ProjectID ";
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

    }
}
