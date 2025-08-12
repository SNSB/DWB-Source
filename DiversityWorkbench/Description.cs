using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiversityWorkbench;

namespace DiversityWorkbench
{
    public class Description : WorkbenchUnit, IWorkbenchUnit
    {
        #region Fields
        private DiversityWorkbench.QueryCondition _QueryConditionProject;
        #endregion

        #region Construction
        public Description(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
            this._FeatureList = new List<ClientFeature>();
            this._FeatureList.Add(ClientFeature.CreateItem);
            this._FeatureList.Add(ClientFeature.SingleItem);
            this._FeatureList.Add(ClientFeature.AdditionalUnitValues);
            //this._FeatureList.Add(ClientFeature.HtmlUnitValues);
        }
        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityDescriptions"; }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this.ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT U.BaseURL + CAST(D.id AS varchar) AS _URI, U.BaseURL + CAST(D.id AS varchar) AS Link, D.label AS _DisplayText, D.id AS ID, D.label AS [Description name], D.detail AS [Details] " +
                    "FROM " + Prefix + "Description D, " + Prefix + "ViewBaseURL AS U " +
                    "WHERE id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);
                if (Values["_DisplayText"] == "")
                    Values["_DisplayText"] = "ID: " + Values["ID"];

                SQL = "SELECT P.label AS [Project], P.detail AS [Project details], P.rights_text AS [Rights], P.licence_uri AS [License] " +
                      "FROM " + Prefix + "Description D INNER JOIN " + Prefix + "Project P ON P.id = D.project_id " +
                      "WHERE D.id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT dbo.project.detail AS [Project details] " +
                //      "FROM  dbo.description INNER JOIN " +
                //      "Project ON Project.id = Description.project_id " +
                //      "WHERE Description.id = " + ID.ToString();
                //this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT dbo.project.rights_text AS [Rights] " +
                //      "FROM  dbo.description INNER JOIN " +
                //      "Project ON Project.id = Description.project_id " +
                //      "WHERE Description.id = " + ID.ToString();
                //this.getDataFromTable(SQL, ref Values);

                //SQL = "SELECT dbo.project.licence_uri AS [License] " +
                //      "FROM  dbo.description INNER JOIN " +
                //      "Project ON Project.id = Description.project_id " +
                //      "WHERE Description.id = " + ID.ToString();
                //this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT label AS [Taxon name] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE type = 'TaxonName' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT label AS [Observation] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE type = 'Observation' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT label AS [Specimen] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE type = 'Specimen' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT label AS [Reference] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE type = 'Citation' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT label AS [Geographic area] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE type = 'GeographicArea' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT label AS [Sampling plot] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE type = 'SamplingPlot' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT dbo.OtherScope.label AS [Sex] " +
                      "FROM " + Prefix + "DescriptionScope INNER JOIN " +
                      "dbo.OtherScope ON OtherScope.id = DescriptionScope.other_scope_id " +
                      "WHERE dbo.DescriptionScope.type = 'Sex' AND dbo.DescriptionScope.description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT dbo.OtherScope.label AS [Stage] " +
                      "FROM " + Prefix + "DescriptionScope INNER JOIN " +
                      "dbo.OtherScope ON OtherScope.id = DescriptionScope.other_scope_id " +
                      "WHERE dbo.DescriptionScope.type = 'Stage' AND dbo.DescriptionScope.description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT dbo.OtherScope.label AS [Part] " +
                      "FROM " + Prefix + "DescriptionScope INNER JOIN " +
                      "dbo.OtherScope ON OtherScope.id = DescriptionScope.other_scope_id " +
                      "WHERE dbo.DescriptionScope.type = 'Part' AND dbo.DescriptionScope.description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT dbo.OtherScope.label AS [Other scope] " +
                      "FROM " + Prefix + "DescriptionScope INNER JOIN " +
                      "dbo.OtherScope ON OtherScope.id = DescriptionScope.other_scope_id " +
                      "WHERE dbo.DescriptionScope.type = 'OtherConcept' AND dbo.DescriptionScope.description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT dwbURI AS [Link to DiversityTaxonNames] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE (type = 'TaxonName') AND NOT dwbURI IS NULL AND dwbURI LIKE '%/TaxonName%' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT dwbURI AS [Link to DiversityCollection] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE (type = 'Observation' OR type = 'Specimen') AND NOT dwbURI IS NULL AND dwbURI LIKE '%/Collection%' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT dwbURI AS [Link to DiversityReferences] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE (type = 'Citation') AND NOT dwbURI IS NULL AND dwbURI LIKE '%/Reference%' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT dwbURI AS [Link to DiversityGazetteers] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE (type = 'GeographicArea') AND NOT dwbURI IS NULL AND dwbURI LIKE '%/Gazetteer%' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT dwbURI AS [Link to DiversitySamplingPlots] " +
                      "FROM " + Prefix + "DescriptionScope " +
                      "WHERE (type = 'SamplingPlot') AND NOT dwbURI IS NULL AND dwbURI LIKE '%/SamplingPlot%' AND description_id = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                if (this._UnitValues == null) this._UnitValues = new Dictionary<string, string>();
                this._UnitValues.Clear();
                foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
                {
                    this._UnitValues.Add(P.Key, P.Value);
                }
            }
            return Values;
        }

        public override void GetAdditionalUnitValues(Dictionary<string, string> UnitValues)
        {
            // Allocate summary data field
            StringBuilder summary = new StringBuilder();

            // Get ID from unit values
            int ID = 0;
            if (UnitValues != null && UnitValues.ContainsKey("ID"))
                int.TryParse(UnitValues["ID"], out ID);
            else if (this._UnitValues != null && this._UnitValues.ContainsKey("ID"))
                int.TryParse(this._UnitValues["ID"], out ID);

            if (ID > 0)
            {
                // Read additional unit values (short text) and build string
                Dictionary<string, string> summaryData = AdditionalUnitValues(ID, false);
                if (summaryData != null && summaryData.Count > 0)
                {
                    foreach (KeyValuePair<string, string> item in summaryData)
                    {
                        if (item.Key != "" && item.Key.IndexOf(". ") > 0)
                        {
                            // Descriptor label with sequence number is given as key
                            string dscrLabel = "• " + item.Key.Substring(item.Key.IndexOf(". ") + 2) + ": ";
                            summary.Append((summary.Length == 0 ? "" : "; ") + dscrLabel);
                            summary.Append(item.Value);
                        }
                        else
                        {
                            // Irregular response
                            summary.Append(item.Value);
                        }
                    }
                }
            }

            // Append summary data in unit values
            if (UnitValues.ContainsKey("Summary data"))
                UnitValues["Summary data"] = summary.ToString();
            else
                UnitValues.Add("Summary data", summary.ToString());
        }

        public System.Collections.Generic.Dictionary<string, string> AdditionalUnitValues(Dictionary<string, string> UnitValues)
        {
            int ID = 0;
            if (UnitValues != null && UnitValues.ContainsKey("ID"))
                int.TryParse(UnitValues["ID"], out ID);
            else if (this._UnitValues != null && this._UnitValues.ContainsKey("ID"))
                int.TryParse(this._UnitValues["ID"], out ID);
            if (ID > 0)
                return AdditionalUnitValues(ID, false);
            else
                return new Dictionary<string, string>();
        }

        public System.Collections.Generic.Dictionary<string, string> AdditionalUnitValues(int ID, bool longText)
        {
            // Allocate result dictionary
            Dictionary<string, string> result = new Dictionary<string, string>();
            try
            {
                // Check for data withholding
                string SQL = string.Format("SELECT TOP 1 E.label " +
                                           "FROM " + Prefix + "[DescriptorStatusData] S " +
                                           "INNER JOIN " + Prefix + "[DataStatus_Enum] E ON S.datastatus_id=E.id " +
                                           "WHERE S.description_id={0} AND E.abbreviation='§'", ID);
                string withhold = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this._ServerConnection.ConnectionString);
                if (withhold != "")
                {
                    // Return irregular response
                    result.Add("", "§§§ " + withhold + " §§§");
                    return result;
                }

                // Read description project
                SQL = "SELECT project_id FROM " + Prefix + "Description WHERE id=" + ID.ToString();
                string projectId = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this._ServerConnection.ConnectionString);

                // Read descriptors
                SQL = string.Format("SELECT D.id, D.label, D.wording, D.wording_before, D.wording_after, D.subclass, D.state_collection_model, D.measurement_unit " +
                                    "FROM " + Prefix + "Descriptor D WHERE D.id IN (SELECT descriptor_id FROM " + Prefix + "View_DescriptorProject WHERE project_id IN ({0})) ORDER BY D.display_order", projectList(ID, projectId));
                System.Data.DataTable dtDscr = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                ad.Fill(dtDscr);

                // Process descriptors according sequence number
                int dscrNum = 0;
                foreach (System.Data.DataRow dscrRow in dtDscr.Rows)
                {
                    // Read descriptor label
                    dscrNum++;
                    string dscrLabel = dscrNum.ToString() + ". " + dscrRow["label"].ToString().Trim();
                    if (dscrRow["wording"] != DBNull.Value && dscrRow["wording"].ToString().Trim() != "")
                        dscrLabel = dscrNum.ToString() + ". " + dscrRow["wording"].ToString().Trim();

                    // Read descriptor unit
                    if (dscrRow["subclass"].ToString() == "quantitative" && dscrRow["measurement_unit"] != DBNull.Value)
                    {
                        if (dscrRow["measurement_unit"].ToString().Trim() != "")
                            dscrLabel += " [" + dscrRow["measurement_unit"].ToString().Trim() + "]";
                    }

                    // Read descriptor status data
                    string column = longText ? "label" : "abbreviation";
                    SQL = string.Format("SELECT E.{2} as status, E.label " +
                                        "FROM " + Prefix + "[DescriptorStatusData] S " +
                                        "INNER JOIN " + Prefix + "[DataStatus_Enum] E ON S.datastatus_id=E.id " +
                                        "WHERE S.description_id={0} AND S.descriptor_id={1} ORDER BY E.abbreviation", ID, dscrRow["id"].ToString(), column);
                    System.Data.DataTable dtStatus = new System.Data.DataTable();
                    ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                    ad.Fill(dtStatus);
                    string status = "";
                    string statusLabel = "";
                    foreach (System.Data.DataRow rowStatus in dtStatus.Rows)
                    {
                        status += (status == "" ? "" : ", ") + rowStatus["label"].ToString().Trim();
                        statusLabel += (statusLabel == "" ? "" : ", ") + rowStatus["status"].ToString().Trim();
                    }
                    if (statusLabel != "")
                        dscrLabel += " (" + statusLabel + ")";
                    //dscrLabel += ": ";

                    string wordingBefore = dscrRow["wording_before"].ToString().Trim();
                    if (wordingBefore != "")
                        wordingBefore += " ";
                    string wordingAfter = dscrRow["wording_after"].ToString().Trim();
                    if (wordingAfter != "")
                        wordingAfter = " " + wordingAfter;

                    StringBuilder summary = new StringBuilder();
                    switch (dscrRow["subclass"].ToString())
                    {
                        case "categorical":
                            string stateConn = longText ? " <i>OR</i> " : " / ";
                            switch (dscrRow["state_collection_model"].ToString())
                            {
                                case "WithSeq":
                                    stateConn = longText ? " <i>WITH</i> " : " with ";
                                    break;
                                case "Between":
                                    stateConn = longText ? " <i>TO</i> " : " - ";
                                    break;
                                default:
                                    if (dscrRow["state_collection_model"].ToString().StartsWith("And"))
                                        stateConn = longText ? " <i>AND</i> " : " & ";
                                    break;
                            }

                            // Read recommended values
                            System.Data.DataTable dtFreq = new System.Data.DataTable();
                            SQL = string.Format("SELECT M.id, M.label FROM " + Prefix + "[Modifier] AS M " +
                                                "INNER JOIN " + Prefix + "[DescriptorTreeNodeRecModifier] AS RM ON M.id=RM.modifier_id " +
                                                "INNER JOIN " + Prefix + "[DescriptorTreeNode] AS DN ON DN.id=RM.node_id " +
                                                "INNER JOIN " + Prefix + "[DescriptorTree] AS DT ON DT.id=DN.descriptortree_id " +
                                                "WHERE DN.descriptor_id={1} AND DT.project_id=(SELECT project_id FROM " + Prefix + "[Description] WHERE id={0}) " +
                                                "UNION SELECT F.id, F.label FROM " + Prefix + "[Frequency] AS F " +
                                                "INNER JOIN " + Prefix + "[DescriptorTreeNodeRecFrequency] AS RF ON F.id=RF.frequency_id " +
                                                "INNER JOIN " + Prefix + "[DescriptorTreeNode] AS DN ON DN.id=RF.node_id " +
                                                "INNER JOIN " + Prefix + "[DescriptorTree] AS DT ON DT.id=DN.descriptortree_id " +
                                                "WHERE DN.descriptor_id={1} AND DT.project_id=(SELECT project_id FROM " + Prefix + "[Description] WHERE id={0})", ID, dscrRow["id"]);
                            ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                            ad.Fill(dtFreq);

                            // Read states
                            //SQL = string.Format("SELECT ST.label, ST.wording, SD.modifier_id, SD.frequency_id FROM " + Prefix + "[CategoricalSummaryData] AS SD INNER JOIN " + Prefix + "[CategoricalState] AS ST ON SD.state_id = ST.id WHERE SD.description_id={0} AND ST.descriptor_id={1} ORDER BY ST.display_order", ID, dscrRow["id"].ToString());
                            SQL = string.Format("SELECT ST.label, ST.wording, SD.modifier_id, SD.frequency_id FROM " + Prefix + "[CategoricalState] AS ST INNER JOIN " + Prefix + "[CategoricalSummaryData] AS SD ON SD.state_id = ST.id WHERE SD.description_id={0} AND ST.descriptor_id={1} ORDER BY ST.display_order", ID, dscrRow["id"].ToString());
                            System.Data.DataTable dtStates = new System.Data.DataTable();
                            ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                            ad.Fill(dtStates);
                            foreach (System.Data.DataRow stateRow in dtStates.Rows)
                            {
                                string stateLabel = stateRow["label"].ToString().Trim();
                                if (stateRow["wording"] != DBNull.Value && stateRow["wording"].ToString().Trim() != "")
                                    stateLabel = stateRow["wording"].ToString().Trim();
                                string modString = "";
                                if (stateRow["modifier_id"] != DBNull.Value)
                                {
                                    System.Data.DataRow[] dr = dtFreq.Select("id=" + stateRow["modifier_id"].ToString());
                                    if (dr.Length > 0)
                                        modString += (modString == "" ? "" : ", ") + dr[0]["label"].ToString().Trim();
                                }
                                if (stateRow["frequency_id"] != DBNull.Value)
                                {
                                    System.Data.DataRow[] dr = dtFreq.Select("id=" + stateRow["frequency_id"].ToString());
                                    if (dr.Length > 0)
                                        modString += (modString == "" ? "" : ", ") + dr[0]["label"].ToString().Trim();
                                }
                                summary.Append((summary.Length == 0 ? "" : stateConn) + stateLabel + (modString == "" ? "" : string.Format(" ({0})", modString)));
                            }
                            break;
                        case "quantitative":
                            // Read recommended values
                            System.Data.DataTable dtMod = new System.Data.DataTable();
                            SQL = string.Format("SELECT DISTINCT M.id, M.label FROM " + Prefix + "[Modifier] AS M " +
                                                "INNER JOIN " + Prefix + "[DescriptorTreeNodeRecModifier] AS RM ON M.id=RM.modifier_id " +
                                                "INNER JOIN " + Prefix + "[DescriptorTreeNode] AS DN ON DN.id=RM.node_id " +
                                                "INNER JOIN " + Prefix + "[DescriptorTree] AS DT ON DT.id=DN.descriptortree_id " +
                                                "WHERE DN.descriptor_id={1} AND DT.project_id=(SELECT project_id FROM " + Prefix + "[Description] WHERE id={0})", ID, dscrRow["id"]);
                            ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                            ad.Fill(dtMod);

                            System.Data.DataTable dtRMeas = new System.Data.DataTable();
                            SQL = string.Format("SELECT DISTINCT RM.measure_id FROM " + Prefix + "[DescriptorTreeNodeRecStatMeasure] AS RM " +
                                                "INNER JOIN " + Prefix + "[DescriptorTreeNode] AS DN ON DN.id=RM.node_id " +
                                                "INNER JOIN " + Prefix + "[DescriptorTree] AS DT ON DT.id=DN.descriptortree_id " +
                                                "WHERE DN.descriptor_id={1} AND DT.project_id=(SELECT project_id FROM " + Prefix + "[Description] WHERE id={0})", ID, dscrRow["id"]);
                            ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                            ad.Fill(dtRMeas);

                            // Read measures
                            column = longText ? "label" : "abbreviation";
                            SQL = string.Format("SELECT SM.{2} as measure, SD.modifier_id, SD.value, SD.measure_id " +
                                                "FROM " + Prefix + "[QuantitativeSummaryData] AS SD " +
                                                "INNER JOIN " + Prefix + "[StatisticalMeasure_Enum] AS SM ON SD.measure_id = SM.id " +
                                                "WHERE SD.description_id={0} AND SD.descriptor_id={1} ORDER BY SM.display_order", ID, dscrRow["id"].ToString(), column);
                            System.Data.DataTable dtMeas = new System.Data.DataTable();
                            ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                            ad.Fill(dtMeas);
                            foreach (System.Data.DataRow measRow in dtMeas.Rows)
                            {
                                if (dtRMeas.Select("measure_id=" + measRow["measure_id"].ToString()).Length > 0)
                                {
                                    string modString = "";
                                    if (measRow["modifier_id"] != DBNull.Value)
                                    {
                                        System.Data.DataRow[] dr = dtMod.Select("id=" + measRow["modifier_id"].ToString());
                                        if (dr.Length > 0)
                                            modString += (modString == "" ? "" : ", ") + dr[0]["label"].ToString().Trim();
                                    }
                                    summary.Append((summary.Length == 0 ? "" : longText ? "\r\n" : ", ") + measRow["measure"].ToString().Trim() + " = " + measRow["value"].ToString() + (modString == "" ? "" : string.Format(" ({0})", modString)));
                                }
                            }
                            break;
                        case "text":
                            SQL = string.Format("SELECT T.content FROM " + Prefix + "[TextDescriptorData] T " +
                                                "WHERE T.description_id={0} AND T.descriptor_id={1}", ID, dscrRow["id"].ToString());
                            summary.Append(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this._ServerConnection.ConnectionString));
                            break;
                        case "sequence":
                            SQL = string.Format("SELECT M.sequence FROM " + Prefix + "[MolecularSequenceData] M " +
                                                "WHERE M.description_id={0} AND M.descriptor_id={1}", ID, dscrRow["id"].ToString());
                            string sequence = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, this._ServerConnection.ConnectionString);
                            for (int i = 0; i < sequence.Length; i++)
                            {
                                if (i > 0 && i % 3 == 0)
                                    summary.Append(" ");
                                summary.Append(sequence[i]);
                            }
                            break;
                        default:
                            break;
                    }
                    // Add summary value to result
                    if (summary.Length > 0)
                    {
                        summary.Insert(0, wordingBefore);
                        summary.Append(wordingAfter);
                        result.Add(dscrLabel, summary.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return result;
        }

        private string projectList(int descriptionId, string projectId)
        {
            try
            {
                // Read description project
                string result = projectId;
                string SQL = string.Format("SELECT id FROM Project WHERE parent_project_id={0} AND ID IN (SELECT id from View_Project_UserAvailable)", projectId);
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    string id = row["id"].ToString();
                    result += ", " + projectList(descriptionId, id);
                }
                return result;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return "";
        }

        public override string HtmlUnitValues(Dictionary<string, string> UnitValues)
        {
            // Read possibly missing summary data
            if (!UnitValues.ContainsKey("Summary data"))
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
                    // Get images
                    string SQL = "SELECT DISTINCT R.[display_order], R.[rights_text], (SELECT TOP 1 [url] FROM " +
                                 this.Prefix + "[ResourceVariant] IV, " + this.Prefix + "[ResourceVariant_Enum] IE " +
                                 "WHERE IV.[variant_id] = IE.[id] AND IV.[resource_id]= R.[id] ORDER BY IE.[quality_order] desc) AS [url] FROM " +
                                 this.Prefix + "[Resource] R INNER JOIN " + this.Prefix + "[ResourceVariant] V ON V.[resource_id] = R.[id] " +
                                 "WHERE V.[mime_type] LIKE 'image%' AND R.[description_id]=" + ID.ToString() + " ORDER BY R.[display_order]";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, this._ServerConnection.ConnectionString);

                    // Start XML writer
                    using (System.Xml.XmlWriter W = System.Xml.XmlWriter.Create(sb))
                    {
                        // Start HTML document
                        W.WriteStartElement("html");
                        W.WriteString("\r\n");
                        W.WriteStartElement("head");
                        W.WriteElementString("title", UnitValues["_DisplayText"] != "" ? UnitValues["_DisplayText"] : "ID: " + ID.ToString());
                        W.WriteEndElement();//head
                        W.WriteString("\r\n");
                        W.WriteElementString("style",
                                             "\r\n" +
                                             ".zoomDesc {\r\n" +
                                             "    transition: transform .2s; /* Animation */\r\n" +
                                             "    margin: 0 auto;\r\n" +
                                             "}\r\n" +
                                             ".zoomDesc:hover {\r\n" +
                                             "    transform: scale(1.5); /* 150% zoom */\r\n" +
                                             "}");
                        W.WriteString("\r\n");
                        W.WriteStartElement("body");
                        W.WriteString("\r\n");

                        // Write description heading
                        W.WriteStartElement("h2");
                        W.WriteStartElement("font");
                        W.WriteAttributeString("face", "Verdana");
                        WriteXmlString(W, UnitValues["Description name"]);
                        W.WriteEndElement();//font
                        W.WriteEndElement();//h2

                        // Write images
                        if (dt.Rows.Count > 0)
                        {
                            foreach (System.Data.DataRow row in dt.Rows)
                            {
                                W.WriteStartElement("a");
                                W.WriteAttributeString("href", (string)row["url"]);
                                W.WriteStartElement("img");
                                W.WriteAttributeString("class", "zoomDesc");
                                W.WriteAttributeString("alt", "");
                                W.WriteAttributeString("height", "80");
                                if (row["rights_text"] != DBNull.Value)
                                    W.WriteAttributeString("title", (string)row["rights_text"]);
                                W.WriteAttributeString("src", (string)row["url"]);
                                W.WriteEndElement();//img 
                                W.WriteEndElement();//a 
                                W.WriteString("\r\n");
                            }
                            W.WriteElementString("br", "");
                            W.WriteElementString("br", "");
                            W.WriteString("\r\n");
                        }

                        // Start table
                        W.WriteStartElement("table");
                        //W.WriteAttributeString("width", "900");
                        W.WriteAttributeString("border", "0");
                        W.WriteAttributeString("cellpadding", "1");
                        W.WriteAttributeString("cellspacing", "0");
                        W.WriteAttributeString("class", "small");
                        W.WriteAttributeString("style", "margin-left:10px");

                        foreach (KeyValuePair<string, string> item in UnitValues)
                        {
                            // Skip irrelevan entries
                            if (item.Key.StartsWith("_"))
                                continue;
                            if (item.Key == "Description name")
                                continue;

                            // End condition
                            if (item.Key.StartsWith("Link to"))
                            {
                                W.WriteEndElement();//table
                                break;
                            }

                            // Insert unit value
                            if (item.Value.Trim() != "")
                            {
                                W.WriteStartElement("tr");
                                //W.WriteAttributeString("style", "padding-top:10px; padding-bottom:20px");

                                // Write first column <td width=_ColumnWidth align="right">
                                W.WriteStartElement("td");
                                W.WriteAttributeString("width", "120");
                                W.WriteAttributeString("align", "right");
                                W.WriteAttributeString("valign", "top");
                                W.WriteAttributeString("style", "padding-right:5px");
                                W.WriteStartElement("font");
                                W.WriteAttributeString("face", "Verdana");
                                W.WriteAttributeString("size", "2");
                                WriteXmlString(W, string.Format("<b>{0}</b>", item.Key));
                                W.WriteEndElement();//font
                                W.WriteEndElement();//td

                                // Write second column <td style="padding-left:5px">
                                W.WriteStartElement("td");
                                W.WriteAttributeString("style", "padding-left:5px");
                                W.WriteStartElement("font");
                                W.WriteAttributeString("face", "Verdana");
                                W.WriteAttributeString("size", "2");
                                WriteXmlString(W, item.Value);
                                W.WriteEndElement();//font
                                W.WriteEndElement();//td
                                W.WriteEndElement();//tr
                            }

                            if (item.Key == "License")
                            {
                                // Start of scope section
                                W.WriteEndElement();//table

                                // Write Scope heading
                                W.WriteStartElement("h3");
                                W.WriteStartElement("font");
                                W.WriteAttributeString("face", "Verdana");
                                W.WriteString("Scopes");
                                W.WriteEndElement();//font
                                W.WriteEndElement();//h3

                                // Start table
                                W.WriteStartElement("table");
                                //W.WriteAttributeString("width", "900");
                                W.WriteAttributeString("border", "0");
                                W.WriteAttributeString("cellpadding", "1");
                                W.WriteAttributeString("cellspacing", "0");
                                W.WriteAttributeString("class", "small");
                                W.WriteAttributeString("style", "margin-left:10px");
                            }
                        }

                        // Get additional values
                        Dictionary<string, string> addtitionalValues = AdditionalUnitValues(ID, true);

                        // Write Summary heading
                        W.WriteStartElement("h3");
                        W.WriteStartElement("font");
                        W.WriteAttributeString("face", "Verdana");
                        W.WriteString("Summary data");
                        W.WriteEndElement();//font
                        W.WriteEndElement();//h3

                        foreach (KeyValuePair<string, string> item in addtitionalValues)
                        {
                            // Insert summary value
                            if (item.Value.Trim() != "")
                            {
                                // Write descriptor
                                W.WriteStartElement("h4");
                                W.WriteAttributeString("style", "padding-left: 45px");
                                W.WriteStartElement("font");
                                W.WriteAttributeString("face", "Verdana");
                                WriteXmlString(W, string.Format("<b>{0}</b>", item.Key));
                                W.WriteEndElement();//font
                                W.WriteEndElement();//h4

                                // Write value
                                W.WriteStartElement("p");
                                W.WriteAttributeString("style", "padding-left: 140px");
                                W.WriteStartElement("font");
                                W.WriteAttributeString("face", "Verdana");
                                W.WriteAttributeString("size", "2");
                                WriteXmlString(W, item.Value);
                                W.WriteEndElement();//font
                                W.WriteEndElement();//p
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
                    string Prefix = "";
                    if (this._ServerConnection.LinkedServer.Length > 0)
                        Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
                    else
                        Prefix = "dbo.";

                    string SQL = "SELECT COUNT(*) FROM " + Prefix + MainTable() +
                        " AS T WHERE (T.id = " + ID.ToString() + ")";
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

        public string MainTable() { return "Description"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[3];
            QueryDisplayColumns[0].DisplayText = "Description";
            QueryDisplayColumns[0].DisplayColumn = "label";
            QueryDisplayColumns[0].OrderColumn = "label";
            QueryDisplayColumns[0].IdentityColumn = "id";
            QueryDisplayColumns[0].TableName = "Description_QueryExt";

            QueryDisplayColumns[1].DisplayText = "Description ID";
            QueryDisplayColumns[1].DisplayColumn = "(SELECT CAST(id AS NVARCHAR) + ' - ' + label)";
            QueryDisplayColumns[1].OrderColumn = "id";
            QueryDisplayColumns[1].IdentityColumn = "id";
            QueryDisplayColumns[1].TableName = "Description_QueryExt";

            QueryDisplayColumns[2].DisplayText = "Accepted name";
            QueryDisplayColumns[2].DisplayColumn = "(SELECT CASE WHEN accepted_name IS NULL THEN '' ELSE accepted_name END + ' - ' + label)";
            QueryDisplayColumns[2].OrderColumn = "accepted_name";
            QueryDisplayColumns[2].IdentityColumn = "id";
            QueryDisplayColumns[2].TableName = "Description_AcceptedName";
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            // User table for log queries
            System.Data.DataTable dtUser = new System.Data.DataTable();
            string SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT CAST(ID AS NVARCHAR), CombinedNameCache " +
                         "FROM " + Prefix + "UserProxy " +
                         "ORDER BY Display";
            Microsoft.Data.SqlClient.SqlDataAdapter aUser = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { aUser.Fill(dtUser); }
                catch { }
            }

            #region Description
            #region ID
            string Description = DiversityWorkbench.Functions.ColumnDescription("Description", "id");
            DiversityWorkbench.QueryCondition qDescriptionId = new DiversityWorkbench.QueryCondition(false, "Description", "id", "id", "Description", "ID", "id", Description, false, false, true, false);
            QueryConditions.Add(qDescriptionId);

            Description = DiversityWorkbench.Functions.ColumnDescription("Description", "alternate_id");
            DiversityWorkbench.QueryCondition qAlternateId = new DiversityWorkbench.QueryCondition(false, "Description", "id", "alternate_id", "Description", "AID", "alternate_id", Description, false, false, false, false);
            QueryConditions.Add(qAlternateId);
            #endregion

            #region Title
            Description = DiversityWorkbench.Functions.ColumnDescription("Description", "label");
            DiversityWorkbench.QueryCondition qDescriptionLabel = new DiversityWorkbench.QueryCondition(true, "Description", "id", "label", "Description", "Title", "Title of the description", Description);
            QueryConditions.Add(qDescriptionLabel);
            #endregion

            #region Detail
            Description = DiversityWorkbench.Functions.ColumnDescription("Description", "detail");
            DiversityWorkbench.QueryCondition qDescriptionDetail = new DiversityWorkbench.QueryCondition(true, "Description", "id", "detail", "Description", "Details", "Description details", Description);
            QueryConditions.Add(qDescriptionDetail);
            #endregion

            #region Project
            System.Data.DataTable dtProject = new System.Data.DataTable();
            SQL = "SELECT id AS [Value], parent_project_id AS [CRef], label AS [Display] " +
                  "FROM Project_Query " +
                  "ORDER BY [Display]";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtProject); }
                catch { }
            }
            if (dtProject.Rows.Count > 1)
            {
                dtProject.Clear();
                SQL = "SELECT NULL AS [Value], NULL AS [CRef], NULL AS [Display] UNION SELECT id AS [Value], parent_project_id AS [CRef], label AS [Display] " +
                      "FROM Project_Query " +
                      "ORDER BY [Display]";
                a.SelectCommand.CommandText = SQL;
                try { a.Fill(dtProject); }
                catch { }
            }
            HashSet<int> prIds = new HashSet<int>();
            foreach (System.Data.DataRow item in dtProject.Rows)
                if (item["Value"] != DBNull.Value)
                    prIds.Add((int)item["Value"]);
            foreach (System.Data.DataRow item in dtProject.Rows)
                if (item["CRef"] != DBNull.Value && !prIds.Contains((int)item["CRef"]))
                    item["CRef"] = DBNull.Value;
            if (dtProject.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn CRef = new System.Data.DataColumn("CRef");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtProject.Columns.Add(Value);
                dtProject.Columns.Add(CRef);
                dtProject.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("Project", "label");
            DiversityWorkbench.QueryCondition qDescriptionProject = new DiversityWorkbench.QueryCondition(true, "Description_QueryExt", "id", "ProjectID", "Display", "Value", "CRef", "Display", "Description", "Project", "Project", Description, dtProject, true);
            QueryConditions.Add(qDescriptionProject);
            #endregion

            #region Additional project
            System.Data.DataTable dtAddProject = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [CRef], NULL AS [Display] UNION SELECT id AS [Value], parent_project_id AS [CRef], label AS [Display] " +
                  "FROM Project " +
                  "ORDER BY [Display]";
            a.SelectCommand.CommandText = SQL;
            try { a.Fill(dtAddProject); }
            catch { }
            if (dtAddProject.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn CRef = new System.Data.DataColumn("CRef");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtAddProject.Columns.Add(Value);
                dtAddProject.Columns.Add(CRef);
                dtAddProject.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("Project", "label");
            DiversityWorkbench.QueryCondition qDescriptionAddProject = new DiversityWorkbench.QueryCondition(false, "Description_AdditionalProject", "id", "add_project_id", "Display", "Value", "CRef", "Display", "Description", "Add. project", "Additional project", Description, dtAddProject, true);
            //DiversityWorkbench.QueryCondition qDescriptionAddProject = new DiversityWorkbench.QueryCondition(false, "Description_AdditionalProject", "id", "add_project_id", "Description", "Add. project", "Additional project", Description, false, false, true, false);
            QueryConditions.Add(qDescriptionAddProject);

            //Description = "If any additional project is present";
            //DiversityWorkbench.QueryCondition qAddProjectPresence = new DiversityWorkbench.QueryCondition(false, "DescriptionProject", "description_id", "Description", "Add. present", "Additional project present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            //QueryConditions.Add(qAddProjectPresence);
            #endregion

            #region Parent project
            System.Data.DataTable dtParProject = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [CRef], NULL AS [Display] UNION SELECT id AS [Value], parent_project_id AS [CRef], label AS [Display] " +
                  "FROM Project WHERE [id] IN (SELECT DISTINCT [parent_project_id] FROM [Project] WHERE NOT [parent_project_id] IS NULL) " +
                  "ORDER BY [Display]";
            a.SelectCommand.CommandText = SQL;
            try { a.Fill(dtParProject); }
            catch { }
            if (dtParProject.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn CRef = new System.Data.DataColumn("CRef");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtParProject.Columns.Add(Value);
                dtParProject.Columns.Add(CRef);
                dtParProject.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("Project", "label");
            DiversityWorkbench.QueryCondition qDescriptionParProject = new DiversityWorkbench.QueryCondition(false, "Description_ParentProject", "id", "parent_project_id", "Display", "Value", "CRef", "Display", "Description", "Parent project", "Parent project", Description, dtParProject, true);
            QueryConditions.Add(qDescriptionParProject);
            #endregion

            #region Log
            Description = DiversityWorkbench.Functions.ColumnDescription("Description", "LogInsertedBy");
            DiversityWorkbench.QueryCondition qDescInsertedBy = new DiversityWorkbench.QueryCondition(false, "Description", "id", "LogInsertedBy", "Description", "Creat. by", "The user that created the dataset", Description, dtUser, false);
            QueryConditions.Add(qDescInsertedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("Description", "LogInsertedWhen");
            DiversityWorkbench.QueryCondition qDescInsertedWhen = new DiversityWorkbench.QueryCondition(false, "Description", "id", "LogInsertedWhen", "Description", "Creat. date", "The date when the dataset was created", Description, QueryCondition.QueryTypes.DateTime);
            QueryConditions.Add(qDescInsertedWhen);

            Description = DiversityWorkbench.Functions.ColumnDescription("Description", "LogUpdatedBy");
            DiversityWorkbench.QueryCondition qDescUpdatedBy = new DiversityWorkbench.QueryCondition(false, "DescriptionUpdate_Query", "id", "LogUpdatedBy", "Description", "Chg. by", "The last user that changed the dataset", Description, dtUser, false);
            QueryConditions.Add(qDescUpdatedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("Description", "LogUpdatedWhen");
            DiversityWorkbench.QueryCondition qDescUpdatedWhen = new DiversityWorkbench.QueryCondition(false, "DescriptionUpdate_Query", "id", "LogUpdatedWhen", "Description", "Chg. date", "The last date when the dataset was changed", Description, QueryCondition.QueryTypes.DateTime);
            QueryConditions.Add(qDescUpdatedWhen);
            #endregion
            #endregion

            #region Description resource
            #region ID
            Description = DiversityWorkbench.Functions.ColumnDescription("Resource", "id");
            DiversityWorkbench.QueryCondition qDescriptionResourceId = new DiversityWorkbench.QueryCondition(false, "Resource", "description_id", "id", "Description resource", "ID", "ID of description resource", Description, false, false, true, true);
            QueryConditions.Add(qDescriptionResourceId);
            #endregion

            #region Title
            Description = DiversityWorkbench.Functions.ColumnDescription("Resource", "label");
            DiversityWorkbench.QueryCondition qDescriptorResourceLabel = new DiversityWorkbench.QueryCondition(true, "Resource", "description_id", "label", "Description resource", "Title", "Title of the description resource", Description);
            QueryConditions.Add(qDescriptorResourceLabel);
            #endregion

            #region Details
            Description = DiversityWorkbench.Functions.ColumnDescription("Resource", "detail");
            DiversityWorkbench.QueryCondition qDescriptorResourceDetail = new DiversityWorkbench.QueryCondition(false, "Resource", "description_id", "detail", "Description resource", "Details", "Description resource details", Description);
            QueryConditions.Add(qDescriptorResourceDetail);
            #endregion

            #region Rights
            Description = DiversityWorkbench.Functions.ColumnDescription("Resource", "rights_text");
            DiversityWorkbench.QueryCondition qDescriptorResourceRights = new DiversityWorkbench.QueryCondition(false, "Resource", "description_id", "rights_text", "Description resource", "Rights", "Description resource rights", Description);
            QueryConditions.Add(qDescriptorResourceRights);
            #endregion

            #region Log
            Description = DiversityWorkbench.Functions.ColumnDescription("Resource", "LogInsertedBy");
            DiversityWorkbench.QueryCondition qDescRscInsertedBy = new DiversityWorkbench.QueryCondition(false, "DescriptionResourceInsert_Query", "id", "LogInsertedBy", "Description resource", "Creat. by", "The user that created the dataset", Description, dtUser, false);
            QueryConditions.Add(qDescRscInsertedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("Resource", "LogInsertedWhen");
            DiversityWorkbench.QueryCondition qDescRscInsertedWhen = new DiversityWorkbench.QueryCondition(false, "DescriptionResourceInsert_Query", "id", "LogInsertedWhen", "Description resource", "Creat. date", "The date when the dataset was created", Description, QueryCondition.QueryTypes.DateTime);
            QueryConditions.Add(qDescRscInsertedWhen);

            Description = DiversityWorkbench.Functions.ColumnDescription("Resource", "LogUpdatedBy");
            DiversityWorkbench.QueryCondition qDescRscUpdatedBy = new DiversityWorkbench.QueryCondition(false, "DescriptionResourceUpdate_Query", "id", "LogUpdatedBy", "Description resource", "Chg. by", "The last user that changed the dataset", Description, dtUser, false);
            QueryConditions.Add(qDescRscUpdatedBy);

            Description = DiversityWorkbench.Functions.ColumnDescription("Resource", "LogUpdatedWhen");
            DiversityWorkbench.QueryCondition qDescRscUpdatedWhen = new DiversityWorkbench.QueryCondition(false, "DescriptionResourceUpdate_Query", "id", "LogUpdatedWhen", "Description resource", "Chg. date", "The last date when the dataset was changed", Description, QueryCondition.QueryTypes.DateTime);
            QueryConditions.Add(qDescRscUpdatedWhen);
            #endregion
            #endregion

            #region Descriptor
            #region Categorical descriptor
            System.Data.DataTable dtCategoricalDescriptor = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [CRef], NULL AS [Display] UNION SELECT id AS [Value], parent_project_id AS [CRef], label AS [Display] FROM Project_Query " +
                  "UNION SELECT Descriptor.id AS [Value], DescriptorTree.project_id AS [CRef], Descriptor.label AS [Display] FROM Descriptor " +
                  "INNER JOIN DescriptorTreeNode ON DescriptorTreeNode.descriptor_id=Descriptor.id " +
                  "INNER JOIN DescriptorTree ON DescriptorTree.id=DescriptorTreeNode.descriptortree_id " +
                  "WHERE DescriptorTree.project_id IN (SELECT id FROM Project_Query) AND Descriptor.subclass='categorical'" +
                  "ORDER BY Display";
            a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtCategoricalDescriptor); }
                catch { }
            }
            if (dtCategoricalDescriptor.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn CRef = new System.Data.DataColumn("CRef");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtCategoricalDescriptor.Columns.Add(Value);
                dtCategoricalDescriptor.Columns.Add(CRef);
                dtCategoricalDescriptor.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("Descriptor", "label");
            DiversityWorkbench.QueryCondition qCategoricalDescriptor = new DiversityWorkbench.QueryCondition(true, "View_DescriptorState", "description_id", "descriptor_id", "Display", "Value", "CRef", "Display", "Descriptor", "Categorical", "Categorical summary data", Description, dtCategoricalDescriptor, true);
            QueryConditions.Add(qCategoricalDescriptor);
            #endregion

            #region Quantitative descriptor
            System.Data.DataTable dtQuantitativeDescriptor = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [CRef], NULL AS [Display] UNION SELECT id AS [Value], parent_project_id AS [CRef], label AS [Display] FROM Project_Query " +
                  "UNION SELECT Descriptor.id AS [Value], DescriptorTree.project_id AS [CRef], Descriptor.label AS [Display] FROM Descriptor " +
                  "INNER JOIN DescriptorTreeNode ON DescriptorTreeNode.descriptor_id=Descriptor.id " +
                  "INNER JOIN DescriptorTree ON DescriptorTree.id=DescriptorTreeNode.descriptortree_id " +
                  "WHERE DescriptorTree.project_id IN (SELECT id FROM Project_Query) AND Descriptor.subclass='quantitative'" +
                  "ORDER BY Display";
            a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtQuantitativeDescriptor); }
                catch { }
            }
            if (dtQuantitativeDescriptor.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn CRef = new System.Data.DataColumn("CRef");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtQuantitativeDescriptor.Columns.Add(Value);
                dtQuantitativeDescriptor.Columns.Add(CRef);
                dtQuantitativeDescriptor.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("Descriptor", "label");
            DiversityWorkbench.QueryCondition qQuantitativeDescriptor = new DiversityWorkbench.QueryCondition(true, "QuantitativeSummaryData", "description_id", "descriptor_id", "Display", "Value", "CRef", "Display", "Descriptor", "Quantitative", "Quantitative summary data", Description, dtQuantitativeDescriptor, true);
            QueryConditions.Add(qQuantitativeDescriptor);
            #endregion

            #region Text descriptor
            System.Data.DataTable dtTextDescriptor = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [CRef], NULL AS [Display] UNION SELECT id AS [Value], parent_project_id AS [CRef], label AS [Display] FROM Project_Query " +
                  "UNION SELECT Descriptor.id AS [Value], DescriptorTree.project_id AS [CRef], Descriptor.label AS [Display] FROM Descriptor " +
                  "INNER JOIN DescriptorTreeNode ON DescriptorTreeNode.descriptor_id=Descriptor.id " +
                  "INNER JOIN DescriptorTree ON DescriptorTree.id=DescriptorTreeNode.descriptortree_id " +
                  "WHERE DescriptorTree.project_id IN (SELECT id FROM Project_Query) AND Descriptor.subclass='text'" +
                  "ORDER BY Display";
            a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtTextDescriptor); }
                catch { }
            }
            if (dtTextDescriptor.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn CRef = new System.Data.DataColumn("CRef");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtTextDescriptor.Columns.Add(Value);
                dtTextDescriptor.Columns.Add(CRef);
                dtTextDescriptor.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("Descriptor", "label");
            DiversityWorkbench.QueryCondition qTextDescriptor = new DiversityWorkbench.QueryCondition(true, "TextDescriptorData", "description_id", "descriptor_id", "Display", "Value", "CRef", "Display", "Descriptor", "Text", "Text descriptor data", Description, dtTextDescriptor, true);
            QueryConditions.Add(qTextDescriptor);
            #endregion

            #region Sequence descriptor
            System.Data.DataTable dtSequenceDescriptor = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [CRef], NULL AS [Display] UNION SELECT id AS [Value], parent_project_id AS [CRef], label AS [Display] FROM Project_Query " +
                  "UNION SELECT Descriptor.id AS [Value], DescriptorTree.project_id AS [CRef], Descriptor.label AS [Display] FROM Descriptor " +
                  "INNER JOIN DescriptorTreeNode ON DescriptorTreeNode.descriptor_id=Descriptor.id " +
                  "INNER JOIN DescriptorTree ON DescriptorTree.id=DescriptorTreeNode.descriptortree_id " +
                  "WHERE DescriptorTree.project_id IN (SELECT id FROM Project_Query) AND Descriptor.subclass='sequence'" +
                  "ORDER BY Display";
            a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtSequenceDescriptor); }
                catch { }
            }
            if (dtSequenceDescriptor.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn CRef = new System.Data.DataColumn("CRef");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtSequenceDescriptor.Columns.Add(Value);
                dtSequenceDescriptor.Columns.Add(CRef);
                dtSequenceDescriptor.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("Descriptor", "label");
            DiversityWorkbench.QueryCondition qSequenceDescriptor = new DiversityWorkbench.QueryCondition(true, "MolecularSequenceData", "description_id", "descriptor_id", "Display", "Value", "CRef", "Display", "Descriptor", "Sequence", "Molecular sequence description", Description, dtSequenceDescriptor, true);
            QueryConditions.Add(qSequenceDescriptor);
            #endregion
            #endregion

            #region Categorical state
            // Categorical state queery causes a very slow processing due to hierachy search if many descriptor/states are present!
            //System.Data.DataTable dtCategoricalState = new System.Data.DataTable();
            //SQL = "SELECT id AS [Value], descriptor_id AS [CRef], label AS Display, NULL AS [Order] " +
            //      "FROM CategoricalState " +
            //      "UNION SELECT id AS [Value], NULL AS [CRef], label AS Display, display_order AS [Order] " +
            //      "FROM [Descriptor] WHERE subclass = 'categorical' " +
            //      "ORDER BY Display";
            //a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            ////if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            ////{
            ////    try { a.Fill(dtCategoricalState); }
            ////    catch { }
            ////}
            ////if (dtCategoricalState.Rows.Count > 1)
            //{
            //    dtCategoricalState.Clear();
            //    SQL = "SELECT NULL AS [Value], NULL AS CRef, NULL AS Display, NULL AS [Order] " +
            //          "UNION SELECT id AS [Value], descriptor_id AS [CRef], label AS Display, NULL AS [Order] " +
            //          "FROM CategoricalState " +
            //          "UNION SELECT id AS [Value], NULL AS [CRef], label AS Display, display_order AS [Order] " +
            //          "FROM [Descriptor] WHERE subclass = 'categorical' " +
            //          "ORDER BY Display";
            //    a.SelectCommand.CommandText = SQL;
            //    try { a.Fill(dtCategoricalState); }
            //    catch { }
            //}
            //if (dtCategoricalState.Columns.Count == 0)
            //{
            //    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
            //    System.Data.DataColumn CRef = new System.Data.DataColumn("CRef");
            //    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
            //    dtCategoricalState.Columns.Add(Value);
            //    dtCategoricalState.Columns.Add(CRef);
            //    dtCategoricalState.Columns.Add(Display);
            //}

            //Description = DiversityWorkbench.Functions.ColumnDescription("CategoricalState", "label");
            //DiversityWorkbench.QueryCondition qCategoricalState = new DiversityWorkbench.QueryCondition(true, "CategoricalSummaryData", "description_id", "state_id", "Display", "Value", "CRef", "Order", "Categorical States", "", "Categorical Descriptor States", Description, dtCategoricalState, true);
            //qCategoricalState.IsSet = true;
            //QueryConditions.Add(qCategoricalState);
            #endregion

            #region Data status
            System.Data.DataTable dtDataStatus = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [Display] UNION SELECT id AS [Value], label AS [Display] " +
                  "FROM DataStatus_Enum " +
                  "ORDER BY [Display]";
            a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtDataStatus); }
                catch { }
            }
            if (dtDataStatus.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtDataStatus.Columns.Add(Value);
                dtDataStatus.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("DataStatus_Enum", "label");
            DiversityWorkbench.QueryCondition qDataStatus = new DiversityWorkbench.QueryCondition(true, "DescriptorStatusData", "description_id", "datastatus_id", "Status data", "Status", "Data status", Description, dtDataStatus, true);
            //qDataStatus.IsSet = true;
            QueryConditions.Add(qDataStatus);
            #endregion

            #region Scope
            #region Taxon
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "label");
            DiversityWorkbench.QueryCondition qScopeTaxonAccepted = new DiversityWorkbench.QueryCondition(false, "Description_AcceptedName", "id", "accepted_name", "Scope", "Accepted name", "Taxon accepted name", Description);
            QueryConditions.Add(qScopeTaxonAccepted);

            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "label");
            DiversityWorkbench.QueryCondition qScopeTaxon = new DiversityWorkbench.QueryCondition(true, "DescriptionTaxon_Query", "description_id", "label", "Scope", "Taxon", "Taxon", Description);
            QueryConditions.Add(qScopeTaxon);

            DiversityWorkbench.QueryCondition _qTaxonSelection = new QueryCondition();
            _qTaxonSelection.showCondition = true;
            _qTaxonSelection.QueryType = QueryCondition.QueryTypes.Module;
            DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
            _qTaxonSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
            _qTaxonSelection.Entity = "DescriptionTaxon_Query.dwbURI";
            _qTaxonSelection.DisplayText = "Taxa";
            _qTaxonSelection.DisplayLongText = "Selection of taxa";
            _qTaxonSelection.Table = "DescriptionTaxon_Query";
            _qTaxonSelection.IdentityColumn = "description_id";
            _qTaxonSelection.Column = "dwbURI";
            _qTaxonSelection.UpperValue = "label";
            _qTaxonSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            _qTaxonSelection.QueryGroup = "Scope";
            _qTaxonSelection.Description = "All taxa from the list";
            _qTaxonSelection.QueryType = QueryCondition.QueryTypes.Module;
            QueryConditions.Add(_qTaxonSelection);
            #endregion

            #region Geography
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "label");
            DiversityWorkbench.QueryCondition qScopeGeography = new DiversityWorkbench.QueryCondition(true, "DescriptionGeography_Query", "description_id", "label", "Scope", "Geography", "Geographic area", Description);
            QueryConditions.Add(qScopeGeography);

            DiversityWorkbench.QueryCondition _qGeographySelection = new QueryCondition();
            _qGeographySelection.showCondition = true;
            _qGeographySelection.QueryType = QueryCondition.QueryTypes.Module;
            DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
            _qGeographySelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;
            _qGeographySelection.Entity = "DescriptionGeography_Query.dwbURI";
            _qGeographySelection.DisplayText = "Geo. areas";
            _qGeographySelection.DisplayLongText = "Selection of geographic areas";
            _qGeographySelection.Table = "DescriptionGeography_Query";
            _qGeographySelection.IdentityColumn = "description_id";
            _qGeographySelection.Column = "dwbURI";
            _qGeographySelection.UpperValue = "label";
            _qGeographySelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            _qGeographySelection.QueryGroup = "Scope";
            _qGeographySelection.Description = "All geographic areas from the list";
            _qGeographySelection.QueryType = QueryCondition.QueryTypes.Module;
            QueryConditions.Add(_qGeographySelection);
            #endregion

            #region Sampling plot
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "label");
            DiversityWorkbench.QueryCondition qScopePlot = new DiversityWorkbench.QueryCondition(false, "DescriptionSamplingPlot_Query", "description_id", "label", "Scope", "Plot", "Sampling plot", Description);
            QueryConditions.Add(qScopePlot);

            DiversityWorkbench.QueryCondition _qPlotSelection = new QueryCondition();
            _qPlotSelection.showCondition = false;
            _qPlotSelection.QueryType = QueryCondition.QueryTypes.Module;
            DiversityWorkbench.SamplingPlot SP = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
            _qPlotSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)SP;
            _qPlotSelection.Entity = "DescriptionSamplingPlot_Query.dwbURI";
            _qPlotSelection.DisplayText = "Plots";
            _qPlotSelection.DisplayLongText = "Selection of plots";
            _qPlotSelection.Table = "DescriptionSamplingPlot_Query";
            _qPlotSelection.IdentityColumn = "description_id";
            _qPlotSelection.Column = "dwbURI";
            _qPlotSelection.UpperValue = "label";
            _qPlotSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            _qPlotSelection.QueryGroup = "Scope";
            _qPlotSelection.Description = "All plots from the list";
            _qPlotSelection.QueryType = QueryCondition.QueryTypes.Module;
            QueryConditions.Add(_qPlotSelection);
            #endregion

            #region Specimen
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "label");
            DiversityWorkbench.QueryCondition qScopeSpecimen = new DiversityWorkbench.QueryCondition(true, "DescriptionSpecimen_Query", "description_id", "label", "Scope", "Specimen", "Specimen", Description);
            QueryConditions.Add(qScopeSpecimen);

            DiversityWorkbench.QueryCondition _qSpecimenSelection = new QueryCondition();
            _qSpecimenSelection.showCondition = true;
            _qSpecimenSelection.QueryType = QueryCondition.QueryTypes.Module;
            DiversityWorkbench.CollectionSpecimen CS = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
            _qSpecimenSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)CS;
            _qSpecimenSelection.Entity = "DescriptionSpecimen_Query.dwbURI";
            _qSpecimenSelection.DisplayText = "Specimens";
            _qSpecimenSelection.DisplayLongText = "Selection of specimens";
            _qSpecimenSelection.Table = "DescriptionSpecimen_Query";
            _qSpecimenSelection.IdentityColumn = "description_id";
            _qSpecimenSelection.Column = "dwbURI";
            _qSpecimenSelection.UpperValue = "label";
            _qSpecimenSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            _qSpecimenSelection.QueryGroup = "Scope";
            _qSpecimenSelection.Description = "All specimens from the list";
            _qSpecimenSelection.QueryType = QueryCondition.QueryTypes.Module;
            QueryConditions.Add(_qSpecimenSelection);
            #endregion

            #region Observation
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "label");
            DiversityWorkbench.QueryCondition qScopeObservation = new DiversityWorkbench.QueryCondition(false, "DescriptionObservation_Query", "description_id", "label", "Scope", "Observation", "Observation", Description);
            QueryConditions.Add(qScopeObservation);

            DiversityWorkbench.QueryCondition _qObservationSelection = new QueryCondition();
            _qObservationSelection.showCondition = false;
            _qObservationSelection.QueryType = QueryCondition.QueryTypes.Module;
            DiversityWorkbench.CollectionSpecimen CO = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
            _qObservationSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)CO;
            _qObservationSelection.Entity = "DescriptionObservation_Query.dwbURI";
            _qObservationSelection.DisplayText = "Observations";
            _qObservationSelection.DisplayLongText = "Selection of observations";
            _qObservationSelection.Table = "DescriptionObservation_Query";
            _qObservationSelection.IdentityColumn = "description_id";
            _qObservationSelection.Column = "dwbURI";
            _qObservationSelection.UpperValue = "label";
            _qObservationSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            _qObservationSelection.QueryGroup = "Scope";
            _qObservationSelection.Description = "All observations from the list";
            _qObservationSelection.QueryType = QueryCondition.QueryTypes.Module;
            QueryConditions.Add(_qObservationSelection);
            #endregion

            #region Reference
            System.Data.DataTable dtScopeReference = new System.Data.DataTable();
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "label");
            DiversityWorkbench.QueryCondition qScopeReference = new DiversityWorkbench.QueryCondition(true, "DescriptionCitation_Query", "description_id", "label", "Scope", "Reference", "Reference", Description);
            QueryConditions.Add(qScopeReference);

            DiversityWorkbench.QueryCondition _qReferenceSelection = new QueryCondition();
            _qReferenceSelection.showCondition = true;
            _qReferenceSelection.QueryType = QueryCondition.QueryTypes.Module;
            DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
            _qReferenceSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)R;
            _qReferenceSelection.Entity = "DescriptionCitation_Query.dwbURI";
            _qReferenceSelection.DisplayText = "References";
            _qReferenceSelection.DisplayLongText = "Selection of references";
            _qReferenceSelection.Table = "DescriptionCitation_Query";
            _qReferenceSelection.IdentityColumn = "description_id";
            _qReferenceSelection.Column = "dwbURI";
            _qReferenceSelection.UpperValue = "label";
            _qReferenceSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
            _qReferenceSelection.QueryGroup = "Scope";
            _qReferenceSelection.Description = "All references from the list";
            _qReferenceSelection.QueryType = QueryCondition.QueryTypes.Module;
            QueryConditions.Add(_qReferenceSelection);
            #endregion

            #region OtherScope
            #region Sex
            System.Data.DataTable dtScopeSex = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [Display] UNION SELECT id AS [Value], label AS [Display] " +
                  "FROM OtherScope " +
                  "WHERE id IN (SELECT DISTINCT other_scope_id FROM DescriptionScope) " + // only values associated with DescriptionScope
                  "AND type = 'sex' " +
                  "ORDER BY [Display]";
            a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtScopeSex); }
                catch { }
            }
            if (dtScopeSex.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtScopeSex.Columns.Add(Value);
                dtScopeSex.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "other_scope_id");
            DiversityWorkbench.QueryCondition qScopeSex = new DiversityWorkbench.QueryCondition(false, "DescriptionSex_Query", "description_id", "other_scope_id", "Scope", "Sex", "Sex", Description, dtScopeSex, true);
            QueryConditions.Add(qScopeSex);
            #endregion

            #region Stage
            System.Data.DataTable dtScopeStage = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [Display] UNION SELECT id AS [Value], label AS [Display] " +
                  "FROM OtherScope " +
                  "WHERE id IN (SELECT DISTINCT other_scope_id FROM DescriptionScope) " + // only values associated with DescriptionScope
                  "AND type = 'stage' " +
                  "ORDER BY [Display]";
            a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtScopeStage); }
                catch { }
            }
            if (dtScopeStage.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtScopeStage.Columns.Add(Value);
                dtScopeStage.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "other_scope_id");
            DiversityWorkbench.QueryCondition qScopeStage = new DiversityWorkbench.QueryCondition(false, "DescriptionStage_Query", "description_id", "other_scope_id", "Scope", "Stage", "Stage", Description, dtScopeStage, true);
            QueryConditions.Add(qScopeStage);
            #endregion

            #region Part
            System.Data.DataTable dtScopePart = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [Display] UNION SELECT id AS [Value], label AS [Display] " +
                  "FROM OtherScope " +
                  "WHERE id IN (SELECT DISTINCT other_scope_id FROM DescriptionScope) " + // only values associated with DescriptionScope
                  "AND type = 'part' " +
                  "ORDER BY [Display]";
            a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtScopePart); }
                catch { }
            }
            if (dtScopePart.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtScopePart.Columns.Add(Value);
                dtScopePart.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "other_scope_id");
            DiversityWorkbench.QueryCondition qScopePart = new DiversityWorkbench.QueryCondition(false, "DescriptionPart_Query", "description_id", "other_scope_id", "Scope", "Part", "Part", Description, dtScopePart, true);
            QueryConditions.Add(qScopePart);
            #endregion

            #region Other
            System.Data.DataTable dtScopeOther = new System.Data.DataTable();
            SQL = "SELECT NULL AS [Value], NULL AS [Display] UNION SELECT id AS [Value], label AS [Display] " +
                  "FROM OtherScope " +
                  "WHERE id IN (SELECT DISTINCT other_scope_id FROM DescriptionScope) " + // only values associated with DescriptionScope
                  "AND type = 'other' " +
                  "ORDER BY [Display]";
            a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
            if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            {
                try { a.Fill(dtScopeOther); }
                catch { }
            }
            if (dtScopeOther.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtScopeOther.Columns.Add(Value);
                dtScopeOther.Columns.Add(Display);
            }
            Description = DiversityWorkbench.Functions.ColumnDescription("DescriptionScope", "other_scope_id");
            DiversityWorkbench.QueryCondition qScopeOther = new DiversityWorkbench.QueryCondition(false, "DescriptionOther_Query", "description_id", "other_scope_id", "Scope", "Other", "Other Scope", Description, dtScopeOther, true);
            QueryConditions.Add(qScopeOther);
            #endregion
            #endregion
            #endregion

            return QueryConditions;
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
                    break;
                case ModuleType.Collection:
                    this._BackLinkImages.Images.Add("DescriptionScope", DiversityWorkbench.Properties.Resources.Scope);
                    this._BackLinkImages.Images.Add("SamplingUnit", DiversityWorkbench.Properties.Resources.Sample);
                    break;
                case ModuleType.Gazetteer:
                    this._BackLinkImages.Images.Add("DescriptionScope", DiversityWorkbench.Properties.Resources.Scope);
                    this._BackLinkImages.Images.Add("SamplingEvent", DiversityWorkbench.Properties.Resources.Event);
                    break;
                case ModuleType.References:
                    this._BackLinkImages.Images.Add("DescriptionScope", DiversityWorkbench.Properties.Resources.Scope);
                    break;
                case ModuleType.SamplingPlots:
                    this._BackLinkImages.Images.Add("DescriptionScope", DiversityWorkbench.Properties.Resources.Scope);
                    break;
                case ModuleType.TaxonNames:
                    this._BackLinkImages.Images.Add("DescriptionScope", DiversityWorkbench.Properties.Resources.Scope);
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
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this.BackLinkConnections(ModuleType.Descriptions))
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
                case ModuleType.Collection:
                    DiversityWorkbench.BackLinkDomain collDomain = this.BackLinkDomain(SC, URI, "Description scope", "DescriptionScope", "dwbURI", 2, Restrictions, "([type]='Specimen' OR [type]='Observation')");
                    if (collDomain.DtItems.Rows.Count > 0)
                        Terms.Add(collDomain);
                    collDomain = this.BackLinkDomain(SC, URI, "Sampling unit", "SamplingUnit", "collection_specimen_uri", 1, Restrictions);
                    if (collDomain.DtItems.Rows.Count > 0)
                        Terms.Add(collDomain);
                    break;
                case ModuleType.Gazetteer:
                    DiversityWorkbench.BackLinkDomain gazDomain = this.BackLinkDomain(SC, URI, "Description scope", "DescriptionScope", "dwbURI", 2, Restrictions, "[type]='GeographicArea'");
                    if (gazDomain.DtItems.Rows.Count > 0)
                        Terms.Add(gazDomain);
                    gazDomain = this.BackLinkDomain(SC, URI, "Sampling event", "SamplingEvent", "geographic_area_uri", 3, Restrictions);
                    if (gazDomain.DtItems.Rows.Count > 0)
                        Terms.Add(gazDomain);
                    break;
                case ModuleType.References:
                    DiversityWorkbench.BackLinkDomain refDomain = this.BackLinkDomain(SC, URI, "Description scope", "DescriptionScope", "dwbURI", 2, Restrictions, "[type]='Citation'");
                    if (refDomain.DtItems.Rows.Count > 0)
                        Terms.Add(refDomain);
                    break;
                case ModuleType.SamplingPlots:
                    DiversityWorkbench.BackLinkDomain plotDomain = this.BackLinkDomain(SC, URI, "Description scope", "DescriptionScope", "dwbURI", 2, Restrictions, "[type]='SamplingPlot'");
                    if (plotDomain.DtItems.Rows.Count > 0)
                        Terms.Add(plotDomain);
                    break;
                case ModuleType.TaxonNames:
                    DiversityWorkbench.BackLinkDomain taxDomain = this.BackLinkDomain(SC, URI, "Description scope", "DescriptionScope", "dwbURI", 3, Restrictions, "[type]='TaxonName'");
                    if (taxDomain.DtItems.Rows.Count > 0)
                        Terms.Add(taxDomain);
                    break;
                case ModuleType.Projects:
                    DiversityWorkbench.BackLinkDomain prjDomain = this.BackLinkDomain(SC, URI, "Workbench project", "ProjectProxy", "ProjectURI", 2, Restrictions);
                    if (prjDomain.DtItems.Rows.Count > 0)
                        Terms.Add(prjDomain);
                    break;
                default:
                    break;
            }
            return Terms;
        }

        private DiversityWorkbench.BackLinkDomain BackLinkDomain(ServerConnection SC, string URI, string DisplayText, string Table, string LinkColumn, int ImageKey, System.Collections.Generic.List<string> Restrictions = null, string ScopeRestriction = "")
        {
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain(DisplayText, Table, LinkColumn, ImageKey);
            string Prefix = "[" + SC.DatabaseName + "].dbo.";
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "]." + Prefix;
            string SQL = "";
            if (Table == "ProjectProxy")
            {
                // Provide only first description id of the project
                SQL = "SELECT 'First of ' + CAST(COUNT(*) as varchar) + ' descriptions' AS DisplayText, MIN(D.id) AS ID , " +
                       "(SELECT COUNT(*) FROM " + Prefix + "[ProjectUser] U WHERE U.LoginName = USER_NAME() AND U.ProjectID = X.ProjectID) AS AccessCount " +
                       "FROM " + Prefix + "ProjectProxy AS X ";
                SQL += "INNER JOIN " + Prefix + "Project AS S ON S.ProjectProxyID = X.ProjectID " +
                       "INNER JOIN " + Prefix + "[Description] AS D ON D.project_id = S.id " +
                       "WHERE(X.ProjectURI = '" + URI + "') " +
                       "GROUP BY X.Project, X.ProjectURI, X.ProjectID ";
            }
            else
            {
                // Provide description labels or ids
                SQL = "SELECT CASE WHEN D.label <> '' THEN D.label ELSE '[ID: ' + CAST(D.id AS varchar) + ']' END AS DisplayText, D.id AS ID, " +
                      "(SELECT COUNT(*) FROM " + Prefix + "[ProjectUser] U WHERE P.ProjectProxyID = U.ProjectID AND U.LoginName = USER_NAME()) AS AccessCount " +
                      "FROM " + Prefix + Table + " AS S ";
                if (Table == "SamplingUnit")
                {
                    // Two joins to description table
                    SQL += "INNER JOIN " + Prefix + "[SamplingEvent] AS E ON S.sampling_event_id = E.id " +
                           "INNER JOIN " + Prefix + "[Description] AS D ON E.description_id = D.id " +
                           "INNER JOIN " + Prefix + "[Project] AS P ON D.project_id = P.id ";
                }
                else
                {
                    // One join to description table
                    SQL += "INNER JOIN " + Prefix + "[Description] AS D ON S.description_id = D.id " +
                           "INNER JOIN " + Prefix + "[Project] AS P ON D.project_id = P.id ";
                }
                // Append filter
                SQL += "WHERE(S." + LinkColumn + " = '" + URI + "') ";
                if (ScopeRestriction != "")
                {
                    SQL += " AND " + ScopeRestriction;
                }
                if (Restrictions != null)
                {
                    foreach (string R in Restrictions)
                    {
                        SQL += " AND " + R;
                    }
                }
                SQL += " GROUP BY D.id, D.label, P.ProjectProxyID ";
            }
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

        #region Static
        /// <summary>
        /// Write an XML string with an XML writer and preserve selected number of HTML format tags
        /// for bold, italic, underline, superior, subordinated text, paragraph and break. Newline
        /// (Windows \r\n or Unix \n) will be replaced by break.
        /// </summary>
        /// <param name="W">Open XML writer (target)</param>
        /// <param name="text">Output text</param>
        public static void WriteXmlString(System.Xml.XmlWriter W, string text)
        {
            try
            {
                text = text.Replace("\r\n", "<br />").Replace("\n", "<br />").Replace("\r", "");
                string buffered = "";
                int startIdx = 0;
                int endIdx = -1;
                int lastIdx = 0;

                while (startIdx < text.Length && (startIdx = text.IndexOf('<', startIdx)) > -1)
                {
                    if (lastIdx < startIdx)
                        buffered += text.Substring(lastIdx, startIdx - lastIdx);
                    endIdx = text.IndexOf('>', startIdx);
                    if (endIdx > -1)
                    {
                        string tag = text.Substring(startIdx, endIdx + 1 - startIdx);
                        switch (tag)
                        {
                            case "<b>":
                            case "<i>":
                            case "<u>":
                            case "<s>":
                            case "<p>":
                            case "<sub>":
                            case "<sup>":
                                if (buffered != "")
                                {
                                    W.WriteString(buffered);
                                    buffered = "";
                                }
                                W.WriteStartElement(tag.Substring(1, tag.Length - 2));
                                startIdx += tag.Length;
                                break;
                            case "</b>":
                            case "</i>":
                            case "</u>":
                            case "</s>":
                            case "</p>":
                            case "</sub>":
                            case "</sup>":
                                if (buffered != "")
                                {
                                    W.WriteString(buffered);
                                    buffered = "";
                                }
                                W.WriteEndElement();
                                startIdx += tag.Length;
                                break;
                            case "<br />":
                                if (buffered != "")
                                {
                                    W.WriteString(buffered);
                                    buffered = "";
                                }
                                W.WriteElementString("br", "");
                                startIdx += tag.Length;
                                break;
                            default:
                                buffered += tag.Substring(0, 1);
                                startIdx++;
                                break;
                        }
                    }
                    else
                    {
                        buffered += text.Substring(startIdx);
                        startIdx = text.Length;
                        lastIdx = startIdx;
                        break;
                    }
                    lastIdx = startIdx;
                }
                if (lastIdx < text.Length)
                    buffered += text.Substring(lastIdx);
                if (buffered != "")
                    W.WriteString(buffered);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// Get text with correct HTML format tags
        /// Replaces line breaks by tag, corrects start/end tags and processes RTF-like tags if required.
        /// </summary>
        /// <param name="text">Original text</param>
        /// <param name="ignoreCascading">true if cascaded tags shall be ignored</param>
        /// <param name="acceptRtf">true is RTF-like tags shall be processed</param>
        /// <returns>Text with correct HTML format tags</returns>
        public static string TaggedText(string text, bool ignoreCascading, bool acceptRtf)
        {
            StringBuilder sb = new StringBuilder(text);
            try
            {
                // Process line breaks
                sb.Replace("<br/>", "<br />");
                sb.Replace("<br>", "<br />");
                sb.Replace("\r\n", "<br />"); // Windows
                sb.Replace("\n", "<br />");   // Unix
                sb.Replace("\r", "");
                sb.Replace("\t", " ");
                if (sb.ToString().EndsWith("<br />"))
                    sb.Remove(sb.ToString().LastIndexOf("<br />"), "<br />".Length);

                // Substitute redundant format tags
                sb.Replace("<em>", "<i>");
                sb.Replace("</em>", "</i>");
                sb.Replace("<strong>", "<b>");
                sb.Replace("</strong>", "</b>");

                // Process RTF tags
                if (acceptRtf)
                {
                    sb.Replace("\\b0{}", "</b>");
                    sb.Replace("\\b{}", "<b>");
                    sb.Replace("\\i0{}", "</i>");
                    sb.Replace("\\i{}", "<i>");
                    sb.Replace("\\ulnone{}", "</u>");
                    sb.Replace("\\ul0{}", "</u>");
                    sb.Replace("\\ul{}", "<u>");
                    sb.Replace("\\sub{}", "<sub>");
                    sb.Replace("\\super{}", "<sup>");
                    int idx = 0;
                    while ((idx = sb.ToString().IndexOf("\\nosupersub{}", idx)) > -1)
                    {
                        sb.Remove(idx, "\\nosupersub{}".Length);
                        if (sb.ToString().LastIndexOf("<sub>", idx) > sb.ToString().LastIndexOf("sup", idx))
                            sb.Insert(idx, "</sub>");
                        else
                            sb.Insert(idx, "</sup>");
                    }
                }

                int startIdx = 0;
                int endIdx = -1;
                Stack<string> startStack = new Stack<string>();
                Stack<string> repStack = new Stack<string>();
                while (startIdx < sb.Length && (startIdx = sb.ToString().IndexOf('<', startIdx)) > -1)
                {
                    endIdx = sb.ToString().IndexOf('>', startIdx);
                    if (endIdx > -1)
                    {
                        string tag = sb.ToString().Substring(startIdx, endIdx + 1 - startIdx);
                        string repTag = "";
                        switch (tag)
                        {
                            case "<b>":
                            case "<i>":
                            case "<u>":
                            case "<s>":
                            case "<p>":
                            case "<sub>":
                            case "<sup>":
                                // Ignore duplicate start tag
                                if (ignoreCascading && startStack.Contains(tag))
                                {
                                    sb.Remove(startIdx, tag.Length);
                                    break;
                                }

                                // Store start tag
                                startStack.Push(tag);
                                startIdx += tag.Length;
                                break;
                            case "</b>":
                            case "</i>":
                            case "</u>":
                            case "</s>":
                            case "</p>":
                            case "</sub>":
                            case "</sup>":
                                // Flag for detection of start tag
                                bool startFound = false;

                                // Insert end tags of nested items
                                while (startStack.Count > 0)
                                {
                                    // Get last start tag
                                    repTag = startStack.Pop();

                                    // continue processing if start tag was found
                                    if (repTag == tag.Replace("</", "<"))
                                    {
                                        startFound = true;
                                        break;
                                    }

                                    // Store tag for restart
                                    repStack.Push(repTag);

                                    // Check if start tag is directly before reading position
                                    if (startIdx >= repTag.Length && sb.ToString().Substring(startIdx - repTag.Length, repTag.Length) == repTag)
                                    {
                                        // Remove preceeding start tag
                                        sb.Remove(startIdx - repTag.Length, repTag.Length);
                                        startIdx -= repTag.Length;
                                    }
                                    else
                                    {
                                        // Insert end tag
                                        sb.Insert(startIdx, repTag.Replace("<", "</"));
                                        startIdx += repTag.Replace("<", "</").Length;
                                    }
                                }

                                if (startFound)
                                {
                                    // Check if start tag is directly before reading position
                                    string startTag = tag.Replace("</", "<");
                                    if (startIdx >= startTag.Length && sb.ToString().Substring(startIdx - startTag.Length, startTag.Length) == startTag)
                                    {
                                        // Remove end tag
                                        sb.Remove(startIdx, tag.Length);

                                        // Remove preceeding start tag
                                        sb.Remove(startIdx - startTag.Length, startTag.Length);
                                        startIdx -= startTag.Length;
                                    }
                                    else
                                    {
                                        // Skip actual tag
                                        startIdx += tag.Length;
                                    }
                                }
                                else
                                {
                                    // Remove end tag with missing start
                                    sb.Remove(startIdx, tag.Length);
                                }

                                // Re-open closed tags
                                while (repStack.Count > 0)
                                {
                                    repTag = repStack.Pop();
                                    startStack.Push(repTag);
                                    sb.Insert(startIdx, repTag);
                                    startIdx += repTag.Length;
                                }
                                break;
                            case "<br />":
                                startIdx += tag.Length;
                                break;
                            default:
                                startIdx++;
                                break;
                        }
                    }
                    else
                    {
                        //Nothing else to do
                        break;
                    }
                }
                while (startStack.Count > 0)
                    sb.Append(startStack.Pop().Replace("<", "</"));
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get text with correct HTML format tags
        /// Replaces line breaks by tag, corrects start/end tags.
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>Text with correct HTML format tags</returns>
        public static string TaggedText(string text)
        {
            return TaggedText(text, false, false);
        }

        /// <summary>
        /// Replace square brackets by angle brackets for selected HTML tags
        /// </summary>
        /// <param name="text">Text with square brackets</param>
        /// <returns>Text with angle brackets</returns>
        public static string Square2Angle(string text)
        {
            StringBuilder sb = new StringBuilder(text);
            sb.Replace("[i]", "<i>");
            sb.Replace("[/i]", "</i>");
            sb.Replace("[em]", "<em>");
            sb.Replace("[/em]", "</em>");
            sb.Replace("[b]", "<b>");
            sb.Replace("[/b]", "</b>");
            sb.Replace("[strong]", "<strong>");
            sb.Replace("[/strong]", "</strong>");
            sb.Replace("[u]", "<u>");
            sb.Replace("[/u]", "</u>");
            sb.Replace("[tt]", "<tt>");
            sb.Replace("[/tt]", "</tt>");
            sb.Replace("[sup]", "<sup>");
            sb.Replace("[/sup]", "</sup>");
            sb.Replace("[sub]", "<sub>");
            sb.Replace("[/sub]", "</sub>");
            sb.Replace("[small]", "<small>");
            sb.Replace("[/small]", "</small>");
            sb.Replace("[br]", "<br />");
            sb.Replace("[br/]", "<br />");
            sb.Replace("[br /]", "<br />");
            sb.Replace("[[", "<");
            sb.Replace("]]", ">");
            return sb.ToString();
        }

        /// <summary>
        /// Replace angle brackets by square brackets for selected HTML tags
        /// </summary>
        /// <param name="text">Text with angle brackets</param>
        /// <returns>Text with square brackets</returns>
        public static string Angle2Square(string text)
        {
            StringBuilder sb = new StringBuilder(TaggedText(text));
            sb.Replace("<i>", "[i]");
            sb.Replace("</i>", "[/i]");
            sb.Replace("<em>", "[em]");
            sb.Replace("</em>", "[/em]");
            sb.Replace("<b>", "[b]");
            sb.Replace("</b>", "[/b]");
            sb.Replace("<strong>", "[strong]");
            sb.Replace("</strong>", "[/strong]");
            sb.Replace("<u>", "[u]");
            sb.Replace("</u>", "[/u]");
            sb.Replace("<tt>", "[tt]");
            sb.Replace("</tt>", "[/tt]");
            sb.Replace("<sup>", "[sup]");
            sb.Replace("</sup>", "[/sup]");
            sb.Replace("<sub>", "[sub]");
            sb.Replace("</sub>", "[/sub]");
            sb.Replace("<small>", "[small]");
            sb.Replace("</small>", "[/small]");
            sb.Replace("<br>", "[br /]");
            sb.Replace("<br/>", "[br /]");
            sb.Replace("<br />", "[br /]");
            sb.Replace("<", "[[");
            sb.Replace(">", "]]");
            return sb.ToString();
        }

        /// <summary>
        /// Replaces illegal characters below 0x20 in a string by a double exclamation mark
        /// </summary>
        /// <param name="input">Input string</param>
        /// <returns>Input string without illegal characters</returns>
        public static string ToXMLstring(string input)
        {
            StringBuilder sb = new StringBuilder(input);

            for (int i = 0; i < input.Length; i++)
            {
                char chk = input[i];
                if (chk < 0x20 && chk != 0x9 && chk != 0xA && chk != 0xD)
                {
                    switch (chk)
                    {
                        case (char)0x9:
                        case (char)0xA:
                        case (char)0xD:
                            break;
                        default:
                            sb.Replace(chk, '‼');
                            break;
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Format sequence string to blocked representation and wrap to line count
        /// </summary>
        /// <param name="sequence">Original sequence</param>
        /// <param name="symbolLength">Sequence length (1 or 3)</param>
        /// <param name="maxLineCount">Number of characters per wrapped line</param>
        /// <returns>Wrapped text</returns>
        public static string WrapSequence(string sequence, int symbolLength, int maxLineCount = 130)
        {
            // Insert blanks in seqeunce string
            int block = symbolLength == 3 ? 3 : 5;
            int count = 0;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < sequence.Length; i++)
            {
                if (i > 0 && i % block == 0)
                {
                    if (maxLineCount > 0 && count >= maxLineCount)
                    {
                        sb.AppendLine();
                        count = 0;
                    }
                    else
                    {
                        sb.Append(" ");
                        count++;
                    }
                }
                sb.Append(sequence[i]);
                count++;
            }
            return sb.ToString();
        }

        /// <summary>
        /// Strips off all text formatting tags and wraps it to specified character cout
        /// </summary>
        /// <param name="text">Original text</param>
        /// <param name="maxLineCount">Number of characters per wrapped line</param>
        /// <returns>Wrapped text</returns>
        public static string WrapText(string text, int maxLineCount = 130)
        {
            StringBuilder wrappedLines = new StringBuilder();
            bool appendLineAtEnd = text.EndsWith("\r\n");
            if (text.StartsWith("\r\n"))
                wrappedLines.AppendLine();

            // Split test in lines
            string[] lines = StripText(text).Split(new string[] { "\r\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                string[] words = line.Split(new string[] { " " }, StringSplitOptions.None);
                string actualLine = "";
                int actualWidth = 0;

                foreach (string word in words)
                {
                    if (word.Length > maxLineCount)
                    {
                        // Split up abnormal long words 
                        if (actualLine.Length > 0)
                        {
                            wrappedLines.AppendLine(actualLine.ToString());
                            actualLine = "";
                            actualWidth = 0;
                        }
                        string splitWord = word;
                        while (splitWord.Length > maxLineCount)
                        {
                            wrappedLines.AppendLine(splitWord.Remove(maxLineCount - 1));
                            splitWord = splitWord.Substring(maxLineCount);
                        }
                        if (splitWord.Length > 0)
                            wrappedLines.AppendLine(splitWord);
                    }
                    else
                    {
                        // Accumulate words
                        actualLine += word + " ";
                        actualWidth += word.Length + 1;

                        if (actualWidth > maxLineCount)
                        {
                            wrappedLines.AppendLine(actualLine.ToString());
                            actualLine = "";
                            actualWidth = 0;
                        }
                    }
                }

                if (actualLine.Length > 0)
                    wrappedLines.AppendLine(actualLine.ToString());
                //wrappedLines.AppendLine();
            }

            if (appendLineAtEnd)
                wrappedLines.AppendLine();

            return wrappedLines.ToString().TrimEnd();
        }

        /// <summary>
        /// Strips off all text formatting tags
        /// </summary>
        /// <param name="text">Original text</param>
        /// <param name="removeRtfTags">Remove RTF tags</param>
        /// <returns>Stripped text</returns>
        public static string StripText(string text, bool removeRtfTags = true)
        {
            string result = text.Replace("<p>", "\r\n").Replace("</p>", "\r\n").Replace("<br>", "\r\n").Replace("<br/>", "\r\n").Replace("<br />", "\r\n");
            result = result.Replace("<b>", "").Replace("</b>", "").Replace("<i>", "").Replace("</i>", "").Replace("<u>", "").Replace("</u>", "").Replace("<s>", "").Replace("</s>", "");
            result = result.Replace("<strong>", "").Replace("</strong>", "").Replace("<em>", "").Replace("</em>", "").Replace("<sub>", "").Replace("</sub>", "").Replace("<sup>", "").Replace("</sup>", "");

            // Process RTF tags
            if (removeRtfTags)
            {
                result = result.Replace("\\b0{}", "").Replace("\\b{}", "").Replace("\\i0{}", "").Replace("\\i{}", "").Replace("\\ulnone{}", "").Replace("\\ul0{}", "").Replace("\\ul{}", "");
                result = result.Replace("\\sub{}", "").Replace("\\super{}", "").Replace("\\nosupersub{}", "");
            }
            return result.Trim();
        }

        /// <summary>
        /// Format text for label
        /// </summary>
        /// <param name="text">Original text</param>
        /// <returns>Label text</returns>
        public static string LabelText(string text)
        {
            return text.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\t", " ");
        }
        #endregion
    }
}
