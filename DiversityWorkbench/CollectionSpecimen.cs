using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench
{
    public class CollectionSpecimen : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {

        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryConditionProject;

        public DiversityWorkbench.QueryCondition QueryConditionProject
        {
            get
            {
                if (this._QueryConditionProject == null ||
                    this._QueryConditionProject.ServerConnection.ConnectionString != this.ServerConnection.ConnectionString)
                {
                    System.Data.DataTable dtProject = new System.Data.DataTable();
                    string SQL = "SELECT ProjectID AS [Value], Project AS Display " +
                        "FROM ProjectList " +
                        "ORDER BY Display";
                    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                    if (this.ServerConnection.ConnectionString.Length > 0 && this.ServerConnection.ConnectionString.IndexOf(";Initial Catalog=DiversityCollection") > -1)
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
                    this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "CollectionProject", "CollectionSpecimenID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
                    this._QueryConditionProject.IsSet = true;
                }


                return _QueryConditionProject;
            }
            //set { _QueryConditionProject = value; }
        }


        #endregion

        #region Construction

        public CollectionSpecimen(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }

        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityCollection"; }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            //string Prefix = "";
            //if (this._ServerConnection.LinkedServer.Length > 0)
            //    Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            //else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this.ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT U.BaseURL + CAST(CollectionSpecimenID AS varchar) AS _URI, U.BaseURL + CAST(CollectionSpecimenID AS varchar) AS Link, " +
                    "CASE WHEN AccessionNumber IS NULL OR LTRIM(AccessionNumber) = '' THEN 'ID: ' + CAST(CollectionSpecimenID as varchar) ELSE AccessionNumber END AS _DisplayText, " +
                    "CollectionSpecimenID AS ID, AccessionNumber AS [Accession number], DepositorsName AS Depositor, ExsiccataAbbreviation AS Exsiccata " +
                    "FROM " + Prefix + "CollectionSpecimen S, ViewBaseURL U " +
                    "WHERE CollectionSpecimenID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT RTRIM(CONVERT(nvarchar(500), " +
                      "CASE WHEN LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0 " +
                      "THEN '' ELSE ' ' + LocalityDescription + ' ' END + " +
                      "CASE WHEN HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0 THEN '' ELSE ' ' + HabitatDescription + ' ' END) + " +
                      "CASE WHEN (LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0) " +
                      "AND (HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0) " +
                      "THEN '(ID: ' + CONVERT(nvarchar, E.CollectionEventID) + ')  ' ELSE '' END) " +
                      "AS Locality, " +
                      " RTRIM(CONVERT(nvarchar(50), " +
                      "(CASE WHEN CollectionYear IS NULL THEN '' ELSE cast(CollectionYear AS varchar) END)  + '-' " +
                      " + (CASE WHEN CollectionMonth IS NULL THEN '' ELSE cast(CollectionMonth AS varchar) END) + '-' " +
                      " + (CASE WHEN CollectionDay IS NULL THEN '' ELSE cast(CollectionDay AS varchar) END)  )) " +
                      "AS [Collection date] " +
                      "FROM  " + Prefix + "CollectionEvent E INNER JOIN " +
                      Prefix + "CollectionSpecimen S ON E.CollectionEventID = S.CollectionEventID " +
                      "WHERE S.CollectionSpecimenID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT Location1 AS SamplingPlot, Location2 AS [Link to DiversitySamplingPlots] " +
                      "FROM  " + Prefix + "CollectionEventLocalisation L INNER JOIN " +
                      Prefix + "CollectionSpecimen S ON L.CollectionEventID = S.CollectionEventID AND L.LocalisationSystemID = 13 " +
                      " WHERE S.CollectionSpecimenID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT DisplayText AS CollectionSiteProperty, PropertyURI AS [Link to DiversityScientificTerms] " +
                      "FROM  " + Prefix + "CollectionEventProperty P INNER JOIN " +
                      Prefix + "CollectionSpecimen S ON P.CollectionEventID = S.CollectionEventID " +
                      " WHERE S.CollectionSpecimenID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT CollectorsName AS Collectors, CollectorsAgentURI AS [Link to DiversityAgents] " +
                    " FROM " + Prefix + "CollectionAgent " +
                    " WHERE CollectionSpecimenID = " + ID.ToString() +
                    " ORDER BY CollectorsSequence ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT LastIdentificationCache AS Organisms, NameURI AS [Link to DiversityTaxonNames] " +
                    " FROM " + Prefix + "Identification I, " + Prefix + "IdentificationUnit U " +
                    " WHERE U.CollectionSpecimenID = I.CollectionSpecimenID " +
                    " AND U.IdentificationUnitID = I.IdentificationUnitID " +
                    " AND I.TaxonomicName <> '' " +
                    " AND U.LastIdentificationCache = I.TaxonomicName " +
                    " AND I.CollectionSpecimenID = " + ID.ToString() +
                    " ORDER BY U.DisplayOrder ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT VernacularTerm AS Objects, TermUri AS [Link to DiversityScientificTerms] " +
                    " FROM " + Prefix + "Identification I, " + Prefix + "IdentificationUnit U " +
                    " WHERE U.CollectionSpecimenID = I.CollectionSpecimenID " +
                    " AND U.IdentificationUnitID = I.IdentificationUnitID " +
                    " AND I.VernacularTerm <> '' " +
                    " AND U.LastIdentificationCache = I.VernacularTerm " +
                    " AND I.CollectionSpecimenID = " + ID.ToString() +
                    " ORDER BY U.DisplayOrder ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT TypeStatus + ': ' + TaxonomicName AS [Type specimen] FROM " + Prefix + "Identification " +
                    " WHERE CollectionSpecimenID = " + ID.ToString() + " AND (TypeStatus <> N'')";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT DISTINCT MaterialCategory AS Material " +
                    " FROM " + Prefix + "CollectionSpecimenPart " +
                    " WHERE CollectionSpecimenID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT DISTINCT StorageLocation AS [Storage location] " +
                    " FROM " + Prefix + "CollectionSpecimenPart " +
                    " WHERE CollectionSpecimenID = " + ID.ToString();
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

        public string MainTable() { return "CollectionSpecimen"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[18];

            try
            {
                DiversityWorkbench.Entity E = new Entity(this.ServerConnection.ConnectionString);
                System.Collections.Generic.Dictionary<string, string> Entity = E.EntityInformation_O("CollectionSpecimen.AccessionNumber");
                QueryDisplayColumns[0].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[0].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[0].DisplayText = "Specimen Acc.no.";
                QueryDisplayColumns[0].DisplayColumn = "AccessionNumber";

                Entity = E.EntityInformation_O("CollectionSpecimen.DepositorsAccessionNumber");
                QueryDisplayColumns[1].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[1].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[1].DisplayText = "Deposit. Acc.no.";
                QueryDisplayColumns[1].DisplayColumn = "DepositorsAccessionNumber";

                Entity = E.EntityInformation_O("CollectionSpecimen.ExternalIdentifier");
                QueryDisplayColumns[2].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[2].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[2].DisplayText = "External ID";
                QueryDisplayColumns[2].DisplayColumn = "ExternalIdentifier";

                Entity = E.EntityInformation_O("IdentificationUnit.LastIdentificationCache");
                QueryDisplayColumns[3].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[3].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[3].DisplayText = "Last Ident.";
                QueryDisplayColumns[3].DisplayColumn = "LastIdentificationCache";
                QueryDisplayColumns[3].TableName = "IdentificationUnit_Core2";

                Entity = E.EntityInformation_O("Identification.TaxonomicName");
                QueryDisplayColumns[4].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[4].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[4].DisplayText = "Tax. name";
                QueryDisplayColumns[4].DisplayColumn = "TaxonomicName";
                QueryDisplayColumns[4].TableName = "Identification_Core2";

                Entity = E.EntityInformation_O("Identification.VernacularTerm");
                QueryDisplayColumns[5].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[5].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[5].DisplayText = "Vern. term";
                QueryDisplayColumns[5].DisplayColumn = "VernacularTerm";
                QueryDisplayColumns[5].TableName = "Identification_Core2";

                Entity = E.EntityInformation_O("IdentificationUnit.UnitIdentifier");
                QueryDisplayColumns[6].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[6].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[6].DisplayText = "Unit identifier";
                QueryDisplayColumns[6].DisplayColumn = "UnitIdentifier";
                QueryDisplayColumns[6].TableName = "IdentificationUnit_Core2";

                Entity = E.EntityInformation_O("IdentificationUnit.ExsiccataNumber");
                QueryDisplayColumns[7].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[7].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[7].DisplayText = "Exsiccata Nr.";
                QueryDisplayColumns[7].DisplayColumn = "ExsiccataNumber";
                QueryDisplayColumns[7].TableName = "IdentificationUnit_Core2";

                Entity = E.EntityInformation_O("CollectionSpecimenPart.StorageLocation");
                QueryDisplayColumns[8].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[8].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[8].DisplayText = "Storage location";
                QueryDisplayColumns[8].DisplayColumn = "StorageLocation";
                QueryDisplayColumns[8].TableName = "CollectionSpecimenPart_Core2";

                Entity = E.EntityInformation_O("CollectionSpecimenPart.AccessionNumber");
                QueryDisplayColumns[9].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[9].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[9].DisplayText = "Acc.Nr. Part";
                QueryDisplayColumns[9].DisplayColumn = "PartAccessionNumber";
                QueryDisplayColumns[9].TableName = "CollectionSpecimenPart_Core2";

                Entity = E.EntityInformation_O("CollectionAgent.CollectorsName");
                QueryDisplayColumns[10].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[10].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[10].DisplayText = "Collector";
                QueryDisplayColumns[10].DisplayColumn = "CollectorsName";
                QueryDisplayColumns[10].TableName = "CollectionAgent_Core";

                Entity = E.EntityInformation_O("CollectionAgent.CollectorsNumber");
                QueryDisplayColumns[11].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[11].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[11].DisplayText = "Collectors number";
                QueryDisplayColumns[11].DisplayColumn = "CollectorsNumber";
                QueryDisplayColumns[11].TableName = "CollectionAgent_Core";
                QueryDisplayColumns[11].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription("CollectionAgent", "CollectorsNumber");

                Entity = E.EntityInformation_O("CollectionEvent.CollectionDate");
                QueryDisplayColumns[12].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[12].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[12].DisplayText = "Collection date";
                QueryDisplayColumns[12].DisplayColumn = "CollectionDate";
                QueryDisplayColumns[12].TipText = "Date of the collection event";

                Entity = E.EntityInformation_O("CollectionEvent.LocalityDescription");
                QueryDisplayColumns[13].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[13].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[13].DisplayText = "Locality";
                QueryDisplayColumns[13].DisplayColumn = "Locality";
                QueryDisplayColumns[13].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription("CollectionEvent", "LocalityDescription");

                Entity = E.EntityInformation_O("CollectionEvent.HabitatDescription");
                QueryDisplayColumns[14].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[14].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[14].DisplayText = "Habitat";
                QueryDisplayColumns[14].DisplayColumn = "Habitat";
                QueryDisplayColumns[14].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription("CollectionEvent", "HabitatDescription");

                Entity = E.EntityInformation_O("CollectionSpecimen.CollectionSpecimenID");
                QueryDisplayColumns[15].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[15].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[15].DisplayText = "Specimen ID";
                QueryDisplayColumns[15].DisplayColumn = "CollectionSpecimenID";

                Entity = E.EntityInformation_O("CollectionSpecimen.LogUpdatedWhen");
                QueryDisplayColumns[16].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[16].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[16].DisplayText = "Last update";
                QueryDisplayColumns[16].DisplayColumn = "LogUpdatedWhen";
                QueryDisplayColumns[16].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription("CollectionSpecimen", "LogUpdatedWhen");

                Entity = E.EntityInformation_O("Collection");
                QueryDisplayColumns[17].DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, Entity);
                if (QueryDisplayColumns[17].DisplayText.Trim().Length == 0)
                    QueryDisplayColumns[17].DisplayText = "Collection";
                QueryDisplayColumns[17].DisplayColumn = "Collection";
                QueryDisplayColumns[17].TableName = "CollectionSpecimenPart_Core2";

                for (int i = 0; i < QueryDisplayColumns.Length; i++)
                {
                    if (QueryDisplayColumns[i].OrderColumn == null)
                        QueryDisplayColumns[i].OrderColumn = QueryDisplayColumns[i].DisplayColumn;
                    if (QueryDisplayColumns[i].IdentityColumn == null)
                        QueryDisplayColumns[i].IdentityColumn = "CollectionSpecimenID";
                    if (QueryDisplayColumns[i].TableName == null)
                        QueryDisplayColumns[i].TableName = "CollectionSpecimen_Core2";
                    if (QueryDisplayColumns[i].TipText == null)
                        QueryDisplayColumns[i].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(QueryDisplayColumns[i].TableName, QueryDisplayColumns[i].DisplayColumn);
                    QueryDisplayColumns[i].Module = "DiversityCollection";
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            string Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityCollection"].ServerConnection.DatabaseName;
            
            string SQL = "";
            string Description = "";
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            // if no connection has been established
            if (Database.Length == 0)
                return QueryConditions;
            if (this.ServerConnection.ConnectionString.Length == 0)
                return QueryConditions;

            try
            {
                #region PROJECT

                QueryConditions.Add(this.QueryConditionProject);

                Description = "If any project is present";
                DiversityWorkbench.QueryCondition qProjectPresence = new DiversityWorkbench.QueryCondition(false, "CollectionProject", "CollectionSpecimenID", "Project", "Presence", "Project present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qProjectPresence);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionProject", "ProjectID");
                DiversityWorkbench.QueryCondition qProjectID = new DiversityWorkbench.QueryCondition(false, "CollectionProject", "CollectionSpecimenID", "ProjectID", "Project", "ID", "Project ID", Description, false, false, true, false);
                //qProjectID.IsSet = true;
                QueryConditions.Add(qProjectID);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionProject", "LogCreatedWhen");
                DiversityWorkbench.QueryCondition qProjectLogCreatedWhen = new DiversityWorkbench.QueryCondition(false, "CollectionProject", "CollectionSpecimenID", "LogCreatedWhen", "Project", "Creat. date", "The date when the dataset was created", Description, QueryCondition.QueryTypes.DateTime);
                QueryConditions.Add(qProjectLogCreatedWhen);

                #endregion

                #region Specimen and Parts

                System.Collections.Generic.List<DiversityWorkbench.QueryField> Fields = new List<QueryField>();
                DiversityWorkbench.QueryField CSANr = new QueryField("CollectionSpecimen_Core2", "AccessionNumber", "CollectionSpecimenID");
                DiversityWorkbench.QueryField CPANr = new QueryField("CollectionSpecimenPart", "AccessionNumber", "CollectionSpecimenID");
                Fields.Add(CSANr);
                Fields.Add(CPANr);
                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen_Core2", "AccessionNumber");
                Description += " or " + DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core2", "AccessionNumber");
                DiversityWorkbench.QueryCondition qAccessionNumber = new DiversityWorkbench.QueryCondition(false, Fields, "SpecimenAndParts", "Acc.Nr.", "Accession number", Description, true);
                QueryConditions.Add(qAccessionNumber);

                #endregion

                #region Specimen

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "AccessionNumber");
                DiversityWorkbench.QueryCondition qAccessionNumberSpecimen = new DiversityWorkbench.QueryCondition(true, "CollectionSpecimen_Core2", "CollectionSpecimenID", "AccessionNumber", "Specimen", "Acc.no.spec.", "Accession number of specimen", Description);
                QueryConditions.Add(qAccessionNumberSpecimen);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "CollectionSpecimenID");
                DiversityWorkbench.QueryCondition qCollectionSpecimenID = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "CollectionSpecimenID", "Specimen", "ID", "CollectionSpecimenID", Description, false, false, true, false);
                QueryConditions.Add(qCollectionSpecimenID);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionEventID");
                DiversityWorkbench.QueryCondition qCollectionEventIDInSpecimen = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", true, SQL, "CollectionEventID", "Specimen", "Event ID", "CollectionEventID", Description, false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qCollectionEventIDInSpecimen);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "AccessionDate");
                DiversityWorkbench.QueryCondition qAccessionDate = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "AccessionDate", "Specimen", "Acc.Date", "Accession date", Description, "AccessionDay", "AccessionMonth", "AccessionYear");
                QueryConditions.Add(qAccessionDate);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "DepositorsName");
                DiversityWorkbench.QueryCondition qDepositorsName = new DiversityWorkbench.QueryCondition(true, "CollectionSpecimen_Core2", "CollectionSpecimenID", "DepositorsName", "Specimen", "Depositor", "Depositor", Description);
                QueryConditions.Add(qDepositorsName);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "DepositorsAccessionNumber");
                DiversityWorkbench.QueryCondition qDepositorsAccessionNumber = new DiversityWorkbench.QueryCondition(true, "CollectionSpecimen_Core2", "CollectionSpecimenID", "DepositorsAccessionNumber", "Specimen", "Depos.Acc.Nr.", "Depositors accession number", Description);
                QueryConditions.Add(qDepositorsAccessionNumber);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "ReferenceTitle");
                DiversityWorkbench.QueryCondition qReferenceTitle = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "ReferenceTitle", "Specimen", "Reference", "Reference", Description);
                QueryConditions.Add(qReferenceTitle);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "LabelTitle");
                DiversityWorkbench.QueryCondition qLabelTitle = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "LabelTitle", "Label", "Title", "Title", Description);
                qLabelTitle.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qLabelTitle);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "LabelType");
                DiversityWorkbench.QueryCondition qLabelType = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "LabelType", "Label", "Type", "Type", Description, "CollLabelType_Enum", Database);
                qLabelType.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qLabelType);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "LabelTranscriptionState");
                DiversityWorkbench.QueryCondition qLabelTranscriptionState = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "LabelTranscriptionState", "Label", "State", "Transcription state", Description, "CollLabelTranscriptionState_Enum", Database);
                QueryConditions.Add(qLabelTranscriptionState);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "OriginalNotes");
                DiversityWorkbench.QueryCondition qOriginalNotes = new DiversityWorkbench.QueryCondition(true, "CollectionSpecimen_Core2", "CollectionSpecimenID", "OriginalNotes", "Specimen", "Orig. notes", "Original notes", Description);
                QueryConditions.Add(qOriginalNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "AdditionalNotes");
                DiversityWorkbench.QueryCondition qAdditionalNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "AdditionalNotes", "Specimen", "Add. notes", "Additional notes", Description);
                QueryConditions.Add(qAdditionalNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "InternalNotes");
                DiversityWorkbench.QueryCondition qInternalNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "InternalNotes", "Specimen", "Int. notes", "Internal notes", Description);
                QueryConditions.Add(qInternalNotes);

                System.Data.DataTable dtExternalSource = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ExternalDatasourceID AS [Value], ExternalDatasourceName AS Display " +
                    "FROM " + Prefix + "CollectionExternalDatasource " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter aExt = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aExt.Fill(dtExternalSource); }
                    catch { }
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("ExternalDatasourceName", "CollectionExternalDatasource");
                DiversityWorkbench.QueryCondition qExternalDatasourceID = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "ExternalDatasourceID", "Specimen", "Ext. source", "External data source", Description, dtExternalSource, true);
                QueryConditions.Add(qExternalDatasourceID);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "ExternalIdentifier");
                DiversityWorkbench.QueryCondition qExternalIdentifier = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "ExternalIdentifier", "Specimen", "Ext. ID", "External identifier", Description);
                QueryConditions.Add(qExternalIdentifier);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "Problems");
                DiversityWorkbench.QueryCondition qProblems = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "Problems", "Specimen", "Problems", "Problems", Description);
                QueryConditions.Add(qProblems);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qSpecimenDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "DataWithholdingReason", "Specimen", "Withhold.", "Data withholding reason", Description);
                QueryConditions.Add(qSpecimenDataWithholdingReason);



                System.Data.DataTable dtUser = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT LoginName, CombinedNameCache " +
                    "FROM " + Prefix + "UserProxy " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter aUser = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aUser.Fill(dtUser); }
                    catch { }
                }
                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "LogCreatedBy");
                DiversityWorkbench.QueryCondition qLogCreatedBy = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "LogCreatedBy", "Specimen", "Creat. by", "The user that created the dataset", Description, dtUser, false);
                QueryConditions.Add(qLogCreatedBy);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "LogCreatedWhen");
                DiversityWorkbench.QueryCondition qLogCreatedWhen = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "LogCreatedWhen", "Specimen", "Creat. date", "The date when the dataset was created", Description, QueryCondition.QueryTypes.DateTime);
                QueryConditions.Add(qLogCreatedWhen);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "LogUpdatedBy");
                DiversityWorkbench.QueryCondition qLogUpdatedBy = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "LogUpdatedBy", "Specimen", "Changed by", "The last user that changed the dataset", Description, dtUser, false);
                QueryConditions.Add(qLogUpdatedBy);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "LogUpdatedWhen");
                DiversityWorkbench.QueryCondition qLogUpdatedWhen = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "LogUpdatedWhen", "Specimen", "Changed at", "The last date when the dataset was changed", Description, true);
                QueryConditions.Add(qLogUpdatedWhen);

                #endregion

                #region Specimen identifier

                //System.Data.DataTable dtIdentifierSpecimen = new System.Data.DataTable();
                //SQL = "SELECT NULL AS [Value] UNION SELECT Type " +
                //    "FROM " + Prefix + "ExternalIdentifierType " +
                //    "ORDER BY Value";
                //Microsoft.Data.SqlClient.SqlDataAdapter aIdentifierSpecimen = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                //if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                //{
                //    try { aIdentifierSpecimen.Fill(dtIdentifierSpecimen); }
                //    catch { }
                //}
                //Description = DiversityWorkbench.Functions.ColumnDescription("Identifier", "Type");
                //DiversityWorkbench.QueryCondition qIdentifierSpecimenType = new DiversityWorkbench.QueryCondition(false, "IdentifierSpecimen", "CollectionSpecimenID", "Type", "Specimen identifier", "Identifier type", "The type of the identifier", Description, dtIdentifierSpecimen, false);
                //qIdentifierSpecimenType.DisplayColumn = "Type";
                //QueryConditions.Add(qIdentifierSpecimenType);

                //Description = DiversityWorkbench.Functions.ColumnDescription("Identifier", "Identifier");
                //DiversityWorkbench.QueryCondition qIdentifierSpecimen = new DiversityWorkbench.QueryCondition(false, "IdentifierSpecimen", "CollectionSpecimenID", "Identifier", "Specimen identifier", "Identifier", "The identifier", Description);
                //QueryConditions.Add(qIdentifierSpecimen);

                #endregion

                #region EVENT

                Description = "If a collection event is present";
                string DisplayText = "Presence";
                string DisplayTextLong = "Collection event present";
                DiversityWorkbench.QueryCondition qEvent = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionEventID", "Event", DisplayText, DisplayTextLong, Description, QueryCondition.CheckDataExistence.ForeignKeyIsNull);
                QueryConditions.Add(qEvent);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEvent T ON S.CollectionEventID = T.CollectionEventID ";

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionEventID");
                DiversityWorkbench.QueryCondition qCollectionEventID = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectionEventID", "Event", "ID", "CollectionEventID", Description, false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qCollectionEventID);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionDate");
                DiversityWorkbench.QueryCondition qCollectionDate = new DiversityWorkbench.QueryCondition(true, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectionDate", "Event", "Coll. date", "Collection date", Description, "CollectionDay", "CollectionMonth", "CollectionYear", "CollectionEventID");
                qCollectionDate.IsNumeric = false;
                QueryConditions.Add(qCollectionDate);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionDate");
                DiversityWorkbench.QueryCondition qCollectionDateCache = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", "CollectionDate", "Event", "Coll. date cache", "Cached collection date", Description, false);
                qCollectionDateCache.SqlFromClause = SQL;
                qCollectionDateCache.ForeignKey = "CollectionEventID";
                QueryConditions.Add(qCollectionDateCache);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionYear");
                DiversityWorkbench.QueryCondition qCollectionYear = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectionYear", "Event", "Year", "Collection year", Description, false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qCollectionYear);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionDateSupplement");
                DiversityWorkbench.QueryCondition qCollectionDateSupplement = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectionDateSupplement", "Event", "Date supl.", "Collection date supplement", Description, "CollectionEventID");
                QueryConditions.Add(qCollectionDateSupplement);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionTime");
                DiversityWorkbench.QueryCondition qCollectionTime = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectionTime", "Event", "Time", "Collection time", Description, "CollectionEventID");
                QueryConditions.Add(qCollectionTime);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionTimeSpan");
                DiversityWorkbench.QueryCondition qCollectionTimeSpan = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectionTimeSpan", "Event", "Timespan", "Collection time span", Description, "CollectionEventID");
                QueryConditions.Add(qCollectionTimeSpan);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionEndDay");
                DiversityWorkbench.QueryCondition qCollectionEndDay = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectionEndDay", "Event", "End day", "Collection end day", Description, false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qCollectionEndDay);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionEndMonth");
                DiversityWorkbench.QueryCondition qCollectionEndMonth = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectionEndMonth", "Event", "End month", "Collection end month", Description, false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qCollectionEndMonth);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionEndYear");
                DiversityWorkbench.QueryCondition qCollectionEndYear = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectionEndYear", "Event", "End year", "Collection end year", Description, false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qCollectionEndYear);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "LocalityDescription");
                DiversityWorkbench.QueryCondition qLocalityDescription = new DiversityWorkbench.QueryCondition(true, "CollectionEvent", "CollectionSpecimenID", true, SQL, "LocalityDescription", "Event", "Locality", "Locality description", Description, "CollectionEventID");
                QueryConditions.Add(qLocalityDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "LocalityVerbatim");
                DiversityWorkbench.QueryCondition qLocalityVerbatim = new DiversityWorkbench.QueryCondition(true, "CollectionEvent", "CollectionSpecimenID", true, SQL, "LocalityVerbatim", "Event", "Loc. verb.", "Locality verbatim", Description, "CollectionEventID");
                QueryConditions.Add(qLocalityVerbatim);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "HabitatDescription");
                DiversityWorkbench.QueryCondition qHabitatDescription = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "HabitatDescription", "Event", "Habitat", "Habitat description", Description, "CollectionEventID");
                QueryConditions.Add(qHabitatDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectorsEventNumber");
                DiversityWorkbench.QueryCondition qCollectorsEventNumber = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectorsEventNumber", "Event", "Event Nr.", "Collectors event number", Description, "CollectionEventID");
                QueryConditions.Add(qCollectorsEventNumber);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "ReferenceTitle");
                DiversityWorkbench.QueryCondition qEventReferenceTitle = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "ReferenceTitle", "Event", "Reference", "Reference", Description, "CollectionEventID");
                QueryConditions.Add(qEventReferenceTitle);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectingMethod");
                DiversityWorkbench.QueryCondition qCollectingMethod = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CollectingMethod", "Event", "Coll. method", "Collecting method", Description, "CollectionEventID");
                QueryConditions.Add(qCollectingMethod);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "Notes");
                DiversityWorkbench.QueryCondition qEventNotes = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "Notes", "Event", "Notes", "Notes", Description, "CollectionEventID");
                QueryConditions.Add(qEventNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CountryCache");
                DiversityWorkbench.QueryCondition qCountryCache = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "CountryCache", "Event", "Country", "Country", Description, "CollectionEventID");
                QueryConditions.Add(qCountryCache);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qEventDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "DataWithholdingReason", "Event", "Withhold.", "Data withholding reason", Description, "CollectionEventID");
                QueryConditions.Add(qEventDataWithholdingReason);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "DataWithholdingReasonDate");
                DiversityWorkbench.QueryCondition qEventDataWithholdingReasonDate = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "DataWithholdingReasonDate", "Event", "Withhold. date", "Data withholding reason date", Description, "CollectionEventID");
                QueryConditions.Add(qEventDataWithholdingReasonDate);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "LogUpdatedBy");
                DiversityWorkbench.QueryCondition qEventLogUpdatedBy = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", true, SQL, "LogUpdatedBy", "Event", "Changed by", "The last user that changed the dataset", Description, dtUser, false);
                QueryConditions.Add(qEventLogUpdatedBy);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "LogUpdatedWhen");
                DiversityWorkbench.QueryCondition qEventLogUpdatedWhen = new DiversityWorkbench.QueryCondition(false, "CollectionEvent", "CollectionSpecimenID", "LogUpdatedWhen", "Event", "Changed at", "The last date when the dataset was changed", Description, true);
                qEventLogUpdatedWhen.SqlFromClause = SQL;
                QueryConditions.Add(qEventLogUpdatedWhen);

                #endregion

                #region EventLocalisation

                #region Common entries

                System.Data.DataTable dtLocalisationSystem = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT LocalisationSystemID, LocalisationSystemParentID, DisplayText " +
                    "FROM " + Prefix + "LocalisationSystem " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aLocalisationSystem = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aLocalisationSystem.Fill(dtLocalisationSystem); }
                    catch { }
                }
                if (dtLocalisationSystem.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtLocalisationSystem.Columns.Add(Value);
                    dtLocalisationSystem.Columns.Add(ParentValue);
                    dtLocalisationSystem.Columns.Add(Display);
                }
                Description = "The localisation system used";
                DiversityWorkbench.QueryCondition qLocalisationSystem = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", "LocalisationSystemID", "Display", "Value", "ParentValue", "Display", "Event localisation", "Localisation", "Localisation system", Description, dtLocalisationSystem, false);
                qLocalisationSystem.SqlFromClause = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID ";
                qLocalisationSystem.IsNumeric = true;
                qLocalisationSystem.ForeignKey = "CollectionEventID";
                QueryConditions.Add(qLocalisationSystem);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventLocalisation", "Location1");
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qLocation1 = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "Location1", "Event localisation", "Loc.1", "Location 1", Description, false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qLocation1);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventLocalisation", "Location2");
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qLocation2 = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "Location2", "Event localisation", "Loc.2", "Location 2", Description, false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qLocation2);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventLocalisation", "LocationNotes");
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qLocationNotes = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "LocationNotes", "Event localisation", "Notes", "Location notes", Description, false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qLocationNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventLocalisation", "ResponsibleName");
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qLocationResponsibleName = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "ResponsibleName", "Event localisation", "Responsible", "Responsible", Description, false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qLocationResponsibleName);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qAnyAvgLatitude = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "AverageLatitudeCache", "Event localisation", "Av. Lat.", "Average latitude", "Average latitude calculated from the given values", false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qAnyAvgLatitude);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qAnyAvgLongitude = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "AverageLongitudeCache", "Event localisation", "Av. Lon.", "Average longitude", "Average longitude calculated from the given values", false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qAnyAvgLongitude);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qAnyAvgAltitude = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "AverageAltitudeCache", "Event localisation", "Av. Lon.", "Average altitude", "Average altitude calculated from the given values", false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qAnyAvgAltitude);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventLocalisation", "Geography");
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (NOT T.Geography IS NULL)";
                DiversityWorkbench.QueryCondition _qAnyGeography = new QueryCondition();
                _qAnyGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qAnyGeography.Entity = "CollectionEventLocalisation.Geography";
                _qAnyGeography.SqlFromClause = SQL;
                _qAnyGeography.TextFixed = false;
                _qAnyGeography.Description = Description;
                _qAnyGeography.Table = "CollectionEventLocalisation";
                _qAnyGeography.IdentityColumn = "CollectionSpecimenID";
                _qAnyGeography.ForeignKey = "CollectionEventID";
                _qAnyGeography.Column = "Geography";
                _qAnyGeography.ForeignKeyTable = "CollectionSpecimen";
                _qAnyGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qAnyGeography.QueryGroup = "Event localisation";
                _qAnyGeography.QueryType = QueryCondition.QueryTypes.Geography;
                _qAnyGeography.useGroupAsEntityForGroups = true;
                DiversityWorkbench.QueryCondition qAnyGeography = new QueryCondition(_qAnyGeography);
                QueryConditions.Add(qAnyGeography);

                #endregion

                #region Exposition and slope

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.LocalisationSystemID = 10)";
                DiversityWorkbench.QueryCondition qExposition = new DiversityWorkbench.QueryCondition(false,
                    "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "Location1",
                    "Event localisation", "Exposition", "Exposition", "Exposition", false, false, true, false,
                    "CollectionEventID", "LocalisationSystem.LocalisationSystemID.10");
                qExposition.Restriction = "LocalisationSystemID = 10";
                QueryConditions.Add(qExposition);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.LocalisationSystemID = 11)";
                DiversityWorkbench.QueryCondition qSlope = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation",
                    "CollectionSpecimenID", true, SQL, "Location1", "Event localisation", "Slope", "Slope", "Slope", false, false, true,
                    false, "CollectionEventID", "LocalisationSystem.LocalisationSystemID.11");
                qSlope.Restriction = "LocalisationSystemID = 11";
                QueryConditions.Add(qSlope);

                #endregion

                #region Coordinates

                #region Presence

                DiversityWorkbench.QueryCondition _qWGS84present = new QueryCondition();
                _qWGS84present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qWGS84present.Entity = "LocalisationSystem.LocalisationSystemID.8";
                _qWGS84present.DisplayText = DiversityWorkbench.CollectionSpecimenText.WGS84_present;// "WGS84 present";
                _qWGS84present.Description = DiversityWorkbench.CollectionSpecimenText.If_any_WGS84_coordinates_are_present;// "If any WGS84 coordinates are present";
                _qWGS84present.TextFixed = true;
                _qWGS84present.Table = "CollectionEventLocalisation";
                _qWGS84present.IdentityColumn = "CollectionSpecimenID";
                _qWGS84present.ForeignKey = "CollectionEventID";
                _qWGS84present.Column = "CollectionEventID";
                _qWGS84present.ForeignKeyTable = "CollectionSpecimen";
                _qWGS84present.Restriction = "LocalisationSystemID = 8";
                _qWGS84present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qWGS84present.QueryGroup = "Coordinates WGS84";
                _qWGS84present.useGroupAsEntityForGroups = true;
                DiversityWorkbench.QueryCondition qWGS84present = new QueryCondition(_qWGS84present);
                QueryConditions.Add(qWGS84present);

                #endregion

                #region Values

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE T.LocalisationSystemID = 8";
                DiversityWorkbench.QueryCondition qLatitude = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "Location2", "Coordinates WGS84", "Lat.", "Latitude", "Latitude as entered by the user", false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qLatitude);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE T.LocalisationSystemID = 8";
                DiversityWorkbench.QueryCondition qLongitude = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "Location1", "Coordinates WGS84", "Lon.", "Longitude", "Longitude as entered by the user", false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qLongitude);

                #endregion

                #region Average Values

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE T.LocalisationSystemID = 8"; //" WHERE (NOT T.AverageLatitudeCache IS NULL)";
                DiversityWorkbench.QueryCondition qAvgLatitude = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "AverageLatitudeCache", "Coordinates WGS84", "Av. Lat.", "Average latitude", "Average latitude calculated from the given values", false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qAvgLatitude);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE T.LocalisationSystemID = 8"; //" WHERE (NOT T.AverageLongitudeCache IS NULL)";
                DiversityWorkbench.QueryCondition qAvgLongitude = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "AverageLongitudeCache", "Coordinates WGS84", "Av. Lon.", "Average longitude", "Average longitude calculated from the given values", false, false, true, false, "CollectionEventID");
                QueryConditions.Add(qAvgLongitude);

                #endregion

                #region Geography

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventLocalisation", "Geography");
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (NOT T.Geography IS NULL)";
                DiversityWorkbench.QueryCondition _qGeography = new QueryCondition();
                _qGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qGeography.Entity = "CollectionEventLocalisation.Geography";
                _qGeography.SqlFromClause = SQL;
                //_qGeography.DisplayText = DiversityWorkbench.CollectionSpecimenText.WGS84_present;// "WGS84 present";
                //_qGeography.Description = DiversityWorkbench.CollectionSpecimenText.If_any_WGS84_coordinates_are_present;// "If any WGS84 coordinates are present";
                _qGeography.TextFixed = false;
                _qGeography.Description = Description;
                _qGeography.Table = "CollectionEventLocalisation";
                _qGeography.IdentityColumn = "CollectionSpecimenID";
                _qGeography.ForeignKey = "CollectionEventID";
                _qGeography.Column = "Geography";
                _qGeography.ForeignKeyTable = "CollectionSpecimen";

                _qGeography.Restriction = "LocalisationSystemID = 8";

                _qGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                //_qGeography.QueryGroup = "Event localisation";
                _qGeography.QueryGroup = "Coordinates WGS84";
                //_qGeography.IsGeography = true;
                _qGeography.QueryType = QueryCondition.QueryTypes.Geography;
                _qGeography.useGroupAsEntityForGroups = true;
                DiversityWorkbench.QueryCondition qGeography = new QueryCondition(_qGeography);
                QueryConditions.Add(qGeography);
                //DiversityWorkbench.QueryCondition qGeography = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL, "Geography", "Event localisation", "Geography.", "Geography", "Geography", false, false, true, false, "CollectionEventID");
                //QueryConditions.Add(qGeography);

                #endregion

                #region UTM

                DiversityWorkbench.QueryCondition _qUTMpresent = new QueryCondition();
                _qUTMpresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qUTMpresent.Entity = "LocalisationSystem.LocalisationSystemID.22";
                _qUTMpresent.Description = "If a UTM entry is present";
                _qUTMpresent.DisplayText = "Present";
                _qUTMpresent.TextFixed = true;
                _qUTMpresent.Table = "CollectionEventLocalisation";
                _qUTMpresent.IdentityColumn = "CollectionSpecimenID";
                _qUTMpresent.ForeignKey = "CollectionEventID";
                _qUTMpresent.Column = "CollectionEventID";
                _qUTMpresent.ForeignKeyTable = "CollectionSpecimen";
                _qUTMpresent.Restriction = "LocalisationSystemID = 22";
                _qUTMpresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qUTMpresent.QueryGroup = "UTM";
                DiversityWorkbench.QueryCondition qUTMpresent = new QueryCondition(_qUTMpresent);
                qUTMpresent.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qUTMpresent);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.LocalisationSystemID = 22)";
                DiversityWorkbench.QueryCondition qUTMrecordingMethod = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                    "CollectionSpecimenID", true, SQL, "RecordingMethod", "UTM", "Method", "Recording method",
                    "Recording method for the UTM coordinates", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.22");
                qUTMrecordingMethod.useGroupAsEntityForGroups = true;
                qUTMrecordingMethod.DisplayText = "Recording method";
                qUTMrecordingMethod.Restriction = "LocalisationSystemID = 22";
                qUTMrecordingMethod.Description = "Recording method for the UTM coordinates";
                QueryConditions.Add(qUTMrecordingMethod);


                //SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                //    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 22)";
                DiversityWorkbench.QueryCondition qUTMeast = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                    "CollectionSpecimenID", true, SQL, "Location1", "UTM", "East", "East",
                    "The UTM grid based on WGS84", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.22.East");
                //qUTMeast.useGroupAsEntityForGroups = true;
                qUTMeast.DisplayText = "East";
                qUTMeast.Restriction = "LocalisationSystemID = 22";
                qUTMeast.Description = "UTM: grid zone and east value, e.g. 32U 289375";
                QueryConditions.Add(qUTMeast);

                //SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                //    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 22)";
                DiversityWorkbench.QueryCondition qUTMnorth = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                    "CollectionSpecimenID", true, SQL, "Location2", "UTM", "North", "North",
                    "", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.22.North");
                qUTMnorth.Description = "UTM: north value";
                qUTMnorth.Restriction = "LocalisationSystemID = 22";
                QueryConditions.Add(qUTMnorth);

                #endregion

                #endregion

                #region Altitude

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.LocalisationSystemID IN (4, 5))";
                DiversityWorkbench.QueryCondition qAltitude = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL,
                    "AverageAltitudeCache", "Altitude", "Av. Alt.", "Average altitude", "Average altitude calculated from the given values", false, false, true, false, "CollectionEventID");
                qAltitude.Restriction = "LocalisationSystemID IN (4, 5)";
                qAltitude.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qAltitude);

                DiversityWorkbench.QueryCondition _qAltitudepresent = new QueryCondition();
                _qAltitudepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qAltitudepresent.Entity = "LocalisationSystem.LocalisationSystemID.4";
                _qAltitudepresent.Description = DiversityWorkbench.CollectionSpecimenText.If_the_altitude_is_present;// "If the altitude is present";
                _qAltitudepresent.DisplayText = DiversityWorkbench.CollectionSpecimenText.Alt_present;// "Alt. present";
                _qAltitudepresent.TextFixed = true;
                _qAltitudepresent.Table = "CollectionEventLocalisation";
                _qAltitudepresent.IdentityColumn = "CollectionSpecimenID";
                _qAltitudepresent.ForeignKey = "CollectionEventID";
                _qAltitudepresent.Column = "CollectionEventID";
                _qAltitudepresent.ForeignKeyTable = "CollectionSpecimen";
                _qAltitudepresent.Restriction = "LocalisationSystemID = 4";
                _qAltitudepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qAltitidepresent.QueryGroup = "Event localisation";
                _qAltitudepresent.QueryGroup = "Altitude";
                DiversityWorkbench.QueryCondition qAltitudepresent = new QueryCondition(_qAltitudepresent);
                QueryConditions.Add(qAltitudepresent);

                #endregion

                #region Depth

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.LocalisationSystemID = 14)";
                DiversityWorkbench.QueryCondition qDepth = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation", "CollectionSpecimenID", true, SQL,
                    "Location1", "Depth", "Depth", "Depth", "Depth", false, false, true, false,
                    "CollectionEventID", "LocalisationSystem.LocalisationSystemID.14");
                qDepth.Restriction = "LocalisationSystemID = 14";
                qDepth.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qDepth);

                DiversityWorkbench.QueryCondition _qDepthPresent = new QueryCondition();
                _qDepthPresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qDepthPresent.Entity = "LocalisationSystem.LocalisationSystemID.14";
                _qDepthPresent.Description = "If the depth is present";
                _qDepthPresent.DisplayText = "Dep. present";
                _qDepthPresent.TextFixed = true;
                _qDepthPresent.Table = "CollectionEventLocalisation";
                _qDepthPresent.IdentityColumn = "CollectionSpecimenID";
                _qDepthPresent.ForeignKey = "CollectionEventID";
                _qDepthPresent.Column = "CollectionEventID";
                _qDepthPresent.ForeignKeyTable = "CollectionSpecimen";
                _qDepthPresent.Restriction = "LocalisationSystemID = 14";
                _qDepthPresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qDepthPresent.QueryGroup = "Depth";
                DiversityWorkbench.QueryCondition qDepthPresent = new QueryCondition(_qDepthPresent);
                QueryConditions.Add(qDepthPresent);

                #endregion

                #region TK25

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.LocalisationSystemID = 3)";
                DiversityWorkbench.QueryCondition qTK25 = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                    "CollectionSpecimenID", true, SQL, "Location1", "TK25", "TK25", "TK25",
                    "The UTM grid used in D, A and CH based on the maps 1:25000, former MTB", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.3");
                qTK25.Restriction = "LocalisationSystemID = 3";
                qTK25.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qTK25);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.LocalisationSystemID = 3)";
                DiversityWorkbench.QueryCondition qQuadrant = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                    "CollectionSpecimenID", true, SQL, "Location2", "TK25", "Quadrant", "Quadrant",
                    "", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.3.Quadrant");
                qQuadrant.Restriction = "LocalisationSystemID = 3";
                QueryConditions.Add(qQuadrant);

                DiversityWorkbench.QueryCondition _qTK25present = new QueryCondition();
                _qTK25present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qTK25present.Entity = "LocalisationSystem.LocalisationSystemID.3";
                _qTK25present.Description = DiversityWorkbench.CollectionSpecimenText.If_a_TK25_entry_is_present;// "If a TK25 entry is present";
                _qTK25present.DisplayText = DiversityWorkbench.CollectionSpecimenText.TK25_present;// "TK25 present";
                _qTK25present.TextFixed = true;
                _qTK25present.Table = "CollectionEventLocalisation";
                _qTK25present.IdentityColumn = "CollectionSpecimenID";
                _qTK25present.ForeignKey = "CollectionEventID";
                _qTK25present.Column = "CollectionEventID";
                _qTK25present.ForeignKeyTable = "CollectionSpecimen";
                _qTK25present.Restriction = "LocalisationSystemID = 3";
                _qTK25present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qTK25present.QueryGroup = "TK25";
                DiversityWorkbench.QueryCondition qTK25present = new QueryCondition(_qTK25present);
                QueryConditions.Add(qTK25present);

                #endregion

                #region Places (Gazetteer)

                System.Collections.Generic.Dictionary<int, string> DictGaz = new Dictionary<int, string>();
                DictGaz.Add(7, "Place");
                DictGaz.Add(18, "Place 2");
                DictGaz.Add(19, "Place 3");
                DictGaz.Add(20, "Place 4");
                DictGaz.Add(21, "Place 5");
                foreach (System.Collections.Generic.KeyValuePair<int, string> KV in DictGaz)
                {
                    System.Collections.Generic.Dictionary<string, string> EntityDictPlace = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID." + KV.Key.ToString());
                    string Place = KV.Value;
                    if (EntityDictPlace.ContainsKey("AbbreviationOK") &&
                        EntityDictPlace["AbbreviationOK"].ToLower() == "true")
                        Place = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictPlace);
                    SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                        " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                        " WHERE T.LocalisationSystemID = " + KV.Key.ToString();
                    string PlaceDescription = Place + " name of for datasets linked to the DiversityGazetteer";
                    if (EntityDictPlace.ContainsKey("DescriptionOK") &&
                        EntityDictPlace["DescriptionOK"].ToLower() == "true")
                        PlaceDescription = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, EntityDictPlace);
                    bool ShowPlace = false;
                    if (KV.Key == 7) ShowPlace = true;
                    DiversityWorkbench.QueryCondition qPlace = new DiversityWorkbench.QueryCondition(ShowPlace, "CollectionEventLocalisation",
                        "CollectionSpecimenID", true, SQL, "Location1", KV.Value, Place, "Name of a place",
                        PlaceDescription, "CollectionEventID", "LocalisationSystem.LocalisationSystemID." + KV.Key.ToString());
                    qPlace.Restriction = "LocalisationSystemID = " + KV.Key.ToString();
                    qPlace.useGroupAsEntityForGroups = true;
                    QueryConditions.Add(qPlace);

                    SQL = " FROM CollectionSpecimen INNER JOIN " +
                        " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                        " WHERE T.LocalisationSystemID = " + KV.Key.ToString();
                    DiversityWorkbench.QueryCondition qLinkToGazetteer = new DiversityWorkbench.QueryCondition(ShowPlace, "CollectionEventLocalisation",
                        "CollectionSpecimenID", true, SQL, "Location2", KV.Value, "Gazetteer URI", "Link to gazetteer",
                        "The link to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID." + KV.Key.ToString());
                    qLinkToGazetteer.Restriction = "LocalisationSystemID = " + KV.Key.ToString();
                    QueryConditions.Add(qLinkToGazetteer);

                    DiversityWorkbench.QueryCondition _qPlacepresent = new QueryCondition();
                    _qPlacepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                    _qPlacepresent.Entity = "LocalisationSystem.LocalisationSystemID." + KV.Key.ToString();
                    _qPlacepresent.Description = DiversityWorkbench.CollectionSpecimenText.If_a_named_place_is_present;// "If a named place is present";
                    _qPlacepresent.DisplayText = DiversityWorkbench.CollectionSpecimenText.Place_present;// "Place present";
                    _qPlacepresent.TextFixed = true;
                    _qPlacepresent.Table = "CollectionEventLocalisation";
                    _qPlacepresent.IdentityColumn = "CollectionSpecimenID";
                    _qPlacepresent.ForeignKey = "CollectionEventID";
                    _qPlacepresent.Column = "CollectionEventID";
                    _qPlacepresent.ForeignKeyTable = "CollectionSpecimen";
                    _qPlacepresent.Restriction = "LocalisationSystemID = " + KV.Key.ToString();
                    _qPlacepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                    _qPlacepresent.QueryGroup = KV.Value;
                    DiversityWorkbench.QueryCondition qPlacepresent = new QueryCondition(_qPlacepresent);
                    QueryConditions.Add(qPlacepresent);
                }

                #endregion

                #region Places single treatment

                //#region Place

                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 7)";
                //DiversityWorkbench.QueryCondition qPlace = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location1", "Place", "Place", "Name of a place",
                //    "The name of a place for datasets linked to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.7");
                //qPlace.Restriction = "LocalisationSystemID = 7";
                //qPlace.useGroupAsEntityForGroups = true;
                //QueryConditions.Add(qPlace);

                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 7)";
                //DiversityWorkbench.QueryCondition qLinkToGazetteer = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location2", "Place", "Gazetteer", "Link to gazetteer",
                //    "The link to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.7");
                //qLinkToGazetteer.Restriction = "LocalisationSystemID = 7";
                //QueryConditions.Add(qLinkToGazetteer);

                //DiversityWorkbench.QueryCondition _qPlacepresent = new QueryCondition();
                //_qPlacepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qPlacepresent.Entity = "LocalisationSystem.LocalisationSystemID.7";
                //_qPlacepresent.Description = DiversityWorkbench.CollectionSpecimenText.If_a_named_place_is_present;// "If a named place is present";
                //_qPlacepresent.DisplayText = DiversityWorkbench.CollectionSpecimenText.Place_present;// "Place present";
                //_qPlacepresent.TextFixed = true;
                //_qPlacepresent.Table = "CollectionEventLocalisation";
                //_qPlacepresent.IdentityColumn = "CollectionSpecimenID";
                //_qPlacepresent.ForeignKey = "CollectionEventID";
                //_qPlacepresent.Column = "CollectionEventID";
                //_qPlacepresent.ForeignKeyTable = "CollectionSpecimen";
                //_qPlacepresent.Restriction = "LocalisationSystemID = 7";
                //_qPlacepresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                ////_qPlacepresent.QueryGroup = "Event localisation";
                //_qPlacepresent.QueryGroup = "Place";
                //DiversityWorkbench.QueryCondition qPlacepresent = new QueryCondition(_qPlacepresent);
                //QueryConditions.Add(qPlacepresent);

                //#endregion

                //#region Place 2

                //System.Collections.Generic.Dictionary<string, string> EntityDictPlace2 = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.18");
                //string Place2 = "Place 2";
                //if (EntityDictPlace2.ContainsKey("AbbreviationOK") &&
                //    EntityDictPlace2["AbbreviationOK"].ToLower() == "true")
                //    Place2 = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictPlace2);
                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 18)";
                //DiversityWorkbench.QueryCondition qPlace2 = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location1", "Place 2", Place2, "Name of a place",
                //    Place2 + " name of for datasets linked to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.18");
                //qPlace2.Restriction = "LocalisationSystemID = 18";
                //qPlace2.useGroupAsEntityForGroups = true;
                //QueryConditions.Add(qPlace2);

                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 18)";
                //DiversityWorkbench.QueryCondition qLinkToGazetteer2 = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location2", "Place 2", "Gazetteer", "Link to gazetteer",
                //    "The link to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.18");
                //qLinkToGazetteer2.Restriction = "LocalisationSystemID = 18";
                //QueryConditions.Add(qLinkToGazetteer2);

                //DiversityWorkbench.QueryCondition _qPlacepresent2 = new QueryCondition();
                //_qPlacepresent2.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qPlacepresent2.Entity = "LocalisationSystem.LocalisationSystemID.18";
                //_qPlacepresent2.Description = DiversityWorkbench.CollectionSpecimenText.If_a_named_place_is_present;// "If a named place is present";
                //_qPlacepresent2.DisplayText = DiversityWorkbench.CollectionSpecimenText.Place_present;// "Place present";
                //_qPlacepresent2.TextFixed = true;
                //_qPlacepresent2.Table = "CollectionEventLocalisation";
                //_qPlacepresent2.IdentityColumn = "CollectionSpecimenID";
                //_qPlacepresent2.ForeignKey = "CollectionEventID";
                //_qPlacepresent2.Column = "CollectionEventID";
                //_qPlacepresent2.ForeignKeyTable = "CollectionSpecimen";
                //_qPlacepresent2.Restriction = "LocalisationSystemID = 18";
                //_qPlacepresent2.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qPlacepresent2.QueryGroup = "Place 2";
                //DiversityWorkbench.QueryCondition qPlacepresent2 = new QueryCondition(_qPlacepresent2);
                //QueryConditions.Add(qPlacepresent2);

                //#endregion

                //#region Place 3

                //System.Collections.Generic.Dictionary<string, string> EntityDictPlace3 = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.19");
                //string Place3 = "Place 3";
                //if (EntityDictPlace3.ContainsKey("AbbreviationOK") &&
                //    EntityDictPlace3["AbbreviationOK"].ToLower() == "true")
                //    Place3 = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictPlace3);
                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 19)";
                //DiversityWorkbench.QueryCondition qPlace3 = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location1", "Place 3", Place3, "Name of a place",
                //    Place3 + " name of for datasets linked to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.19");
                //qPlace3.Restriction = "LocalisationSystemID = 19";
                //qPlace3.useGroupAsEntityForGroups = true;
                //QueryConditions.Add(qPlace3);

                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 19)";
                //DiversityWorkbench.QueryCondition qLinkToGazetteer3 = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location3", "Place 3", "Gazetteer", "Link to gazetteer",
                //    "The link to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.19");
                //qLinkToGazetteer3.Restriction = "LocalisationSystemID = 19";
                //QueryConditions.Add(qLinkToGazetteer3);

                //DiversityWorkbench.QueryCondition _qPlacepresent3 = new QueryCondition();
                //_qPlacepresent3.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qPlacepresent3.Entity = "LocalisationSystem.LocalisationSystemID.19";
                //_qPlacepresent3.Description = DiversityWorkbench.CollectionSpecimenText.If_a_named_place_is_present;// "If a named place is present";
                //_qPlacepresent3.DisplayText = DiversityWorkbench.CollectionSpecimenText.Place_present;// "Place present";
                //_qPlacepresent3.TextFixed = true;
                //_qPlacepresent3.Table = "CollectionEventLocalisation";
                //_qPlacepresent3.IdentityColumn = "CollectionSpecimenID";
                //_qPlacepresent3.ForeignKey = "CollectionEventID";
                //_qPlacepresent3.Column = "CollectionEventID";
                //_qPlacepresent3.ForeignKeyTable = "CollectionSpecimen";
                //_qPlacepresent3.Restriction = "LocalisationSystemID = 19";
                //_qPlacepresent3.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qPlacepresent3.QueryGroup = "Place 3";
                //DiversityWorkbench.QueryCondition qPlacepresent3 = new QueryCondition(_qPlacepresent3);
                //QueryConditions.Add(qPlacepresent3);

                //#endregion

                //#region Place 4

                //System.Collections.Generic.Dictionary<string, string> EntityDictPlace4 = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.40");
                //string Place4 = "Place 4";
                //if (EntityDictPlace4.ContainsKey("AbbreviationOK") &&
                //    EntityDictPlace4["AbbreviationOK"].ToLower() == "true")
                //    Place4 = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictPlace4);
                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 40)";
                //DiversityWorkbench.QueryCondition qPlace4 = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location1", "Place 4", Place4, "Name of a place",
                //    Place4 + " name of for datasets linked to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.40");
                //qPlace4.Restriction = "LocalisationSystemID = 40";
                //qPlace4.useGroupAsEntityForGroups = true;
                //QueryConditions.Add(qPlace4);

                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 40)";
                //DiversityWorkbench.QueryCondition qLinkToGazetteer4 = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location4", "Place 4", "Gazetteer", "Link to gazetteer",
                //    "The link to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.40");
                //qLinkToGazetteer4.Restriction = "LocalisationSystemID = 40";
                //QueryConditions.Add(qLinkToGazetteer4);

                //DiversityWorkbench.QueryCondition _qPlacepresent4 = new QueryCondition();
                //_qPlacepresent4.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qPlacepresent4.Entity = "LocalisationSystem.LocalisationSystemID.40";
                //_qPlacepresent4.Description = DiversityWorkbench.CollectionSpecimenText.If_a_named_place_is_present;// "If a named place is present";
                //_qPlacepresent4.DisplayText = DiversityWorkbench.CollectionSpecimenText.Place_present;// "Place present";
                //_qPlacepresent4.TextFixed = true;
                //_qPlacepresent4.Table = "CollectionEventLocalisation";
                //_qPlacepresent4.IdentityColumn = "CollectionSpecimenID";
                //_qPlacepresent4.ForeignKey = "CollectionEventID";
                //_qPlacepresent4.Column = "CollectionEventID";
                //_qPlacepresent4.ForeignKeyTable = "CollectionSpecimen";
                //_qPlacepresent4.Restriction = "LocalisationSystemID = 40";
                //_qPlacepresent4.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qPlacepresent4.QueryGroup = "Place 4";
                //DiversityWorkbench.QueryCondition qPlacepresent4 = new QueryCondition(_qPlacepresent4);
                //QueryConditions.Add(qPlacepresent4);

                //#endregion

                //#region Place 5

                //System.Collections.Generic.Dictionary<string, string> EntityDictPlace5 = DiversityWorkbench.Entity.EntityInformation("LocalisationSystem.LocalisationSystemID.21");
                //string Place5 = "Place 5";
                //if (EntityDictPlace5.ContainsKey("AbbreviationOK") &&
                //    EntityDictPlace5["AbbreviationOK"].ToLower() == "true")
                //    Place5 = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictPlace5);
                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 21)";
                //DiversityWorkbench.QueryCondition qPlace5 = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location1", "Place 5", Place5, "Name of a place",
                //    Place5 + " name of for datasets linked to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.21");
                //qPlace5.Restriction = "LocalisationSystemID = 21";
                //qPlace5.useGroupAsEntityForGroups = true;
                //QueryConditions.Add(qPlace5);

                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " CollectionEventLocalisation T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID " +
                //    " WHERE (T.LocalisationSystemID = 21)";
                //DiversityWorkbench.QueryCondition qLinkToGazetteer5 = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation",
                //    "CollectionSpecimenID", true, SQL, "Location5", "Place 5", "Gazetteer", "Link to gazetteer",
                //    "The link to the DiversityGazetteer", "CollectionEventID", "LocalisationSystem.LocalisationSystemID.21");
                //qLinkToGazetteer5.Restriction = "LocalisationSystemID = 21";
                //QueryConditions.Add(qLinkToGazetteer5);

                //DiversityWorkbench.QueryCondition _qPlacepresent5 = new QueryCondition();
                //_qPlacepresent5.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qPlacepresent5.Entity = "LocalisationSystem.LocalisationSystemID.21";
                //_qPlacepresent5.Description = DiversityWorkbench.CollectionSpecimenText.If_a_named_place_is_present;// "If a named place is present";
                //_qPlacepresent5.DisplayText = DiversityWorkbench.CollectionSpecimenText.Place_present;// "Place present";
                //_qPlacepresent5.TextFixed = true;
                //_qPlacepresent5.Table = "CollectionEventLocalisation";
                //_qPlacepresent5.IdentityColumn = "CollectionSpecimenID";
                //_qPlacepresent5.ForeignKey = "CollectionEventID";
                //_qPlacepresent5.Column = "CollectionEventID";
                //_qPlacepresent5.ForeignKeyTable = "CollectionSpecimen";
                //_qPlacepresent5.Restriction = "LocalisationSystemID = 21";
                //_qPlacepresent5.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qPlacepresent5.QueryGroup = "Place 5";
                //DiversityWorkbench.QueryCondition qPlacepresent5 = new QueryCondition(_qPlacepresent5);
                //QueryConditions.Add(qPlacepresent5);

                //#endregion

                #endregion

                #endregion

                #region Sampling plot

                DiversityWorkbench.QueryCondition _qSamlingPlotpresent = new QueryCondition();
                _qSamlingPlotpresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qSamlingPlotpresent.Entity = "LocalisationSystem.LocalisationSystemID.13";
                _qSamlingPlotpresent.DisplayText = "Plot present";
                _qSamlingPlotpresent.Description = "If any sampling plot are present";
                _qSamlingPlotpresent.TextFixed = true;
                _qSamlingPlotpresent.Table = "CollectionEventLocalisation";
                _qSamlingPlotpresent.IdentityColumn = "CollectionSpecimenID";
                _qSamlingPlotpresent.ForeignKey = "CollectionEventID";
                _qSamlingPlotpresent.Column = "CollectionEventID";
                _qSamlingPlotpresent.ForeignKeyTable = "CollectionSpecimen";
                _qSamlingPlotpresent.Restriction = "LocalisationSystemID = 13";
                _qSamlingPlotpresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qSamlingPlotpresent.QueryGroup = "Sampling plot";
                _qSamlingPlotpresent.useGroupAsEntityForGroups = true;
                DiversityWorkbench.QueryCondition qSamlingPlotpresent = new QueryCondition(_qSamlingPlotpresent);
                QueryConditions.Add(qSamlingPlotpresent);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.LocalisationSystemID = 13)";
                DiversityWorkbench.QueryCondition qSamplingPlot = new DiversityWorkbench.QueryCondition(false, "CollectionEventLocalisation",
                    "CollectionSpecimenID", true, SQL, "Location1", "Sampling plot", "Sampl.plot", "Sampling plot", "Sampling plot",
                    false, false, false, false, "CollectionEventID", "LocalisationSystem.LocalisationSystemID.13");
                qSamplingPlot.useGroupAsEntityForGroups = true;
                qSamplingPlot.SupressSearchForPresence = true;
                qSamplingPlot.Restriction = "LocalisationSystemID = 13";
                QueryConditions.Add(qSamplingPlot);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventLocalisation", "Geography");
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventLocalisation T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (NOT T.Geography IS NULL) AND (T.LocalisationSystemID = 13)";
                DiversityWorkbench.QueryCondition _qPlotGeography = new QueryCondition();
                _qPlotGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qPlotGeography.Entity = "T.Geography";
                _qPlotGeography.SqlFromClause = SQL;
                _qPlotGeography.TextFixed = false;
                _qPlotGeography.Description = Description;
                _qPlotGeography.Table = "CollectionEventLocalisation";
                _qPlotGeography.IdentityColumn = "CollectionSpecimenID";
                _qPlotGeography.ForeignKey = "CollectionEventID";
                _qPlotGeography.Column = "Geography";
                _qPlotGeography.ForeignKeyTable = "CollectionSpecimen";
                _qPlotGeography.Restriction = "LocalisationSystemID = 13";
                _qPlotGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qPlotGeography.QueryGroup = "Sampling plot";
                _qPlotGeography.QueryType = QueryCondition.QueryTypes.Geography;
                _qPlotGeography.useGroupAsEntityForGroups = true;
                DiversityWorkbench.QueryCondition qPlotGeography = new QueryCondition(_qPlotGeography);
#if DEBUG
                //DiversityWorkbench.SamplingPlot samplingPlot = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                //qPlotGeography.iWorkbenchUnit = samplingPlot;
#endif
                DiversityWorkbench.SamplingPlot samplingPlot = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                qPlotGeography.iWorkbenchUnit = samplingPlot;


                QueryConditions.Add(qPlotGeography);


                DiversityWorkbench.QueryCondition _qPlotSelection = new QueryCondition();
                _qPlotSelection.QueryType = QueryCondition.QueryTypes.Module;
                DiversityWorkbench.SamplingPlot S = new DiversityWorkbench.SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                _qPlotSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)S;
                _qPlotSelection.Entity = "LocalisationSystem.LocalisationSystemID.13";
                _qPlotSelection.DisplayText = "Plots";
                _qPlotSelection.DisplayLongText = "Selection of plots";
                _qPlotSelection.Table = "CollectionEventLocalisation";
                _qPlotSelection.IdentityColumn = "CollectionSpecimenID";
                _qPlotSelection.ForeignKey = "CollectionEventID";
                _qPlotSelection.Column = "Location2";
                //_qPlotSelection.UpperValue = "Location1";
                _qPlotSelection.ForeignKeyTable = "CollectionSpecimen";
                _qPlotSelection.Restriction = "LocalisationSystemID = 13";
                _qPlotSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qPlotSelection.QueryGroup = "Sampling plot";
                _qPlotSelection.Description = "All plots from the list";
                _qPlotSelection.SqlFromClause = SQL;
                _qPlotSelection.QueryType = QueryCondition.QueryTypes.Module;
                QueryConditions.Add(_qPlotSelection);

#if DEBUG

#endif

                #endregion

                #region EventProperty

                //Description = "If a collection site property is present";
                //DisplayText = "Presence";
                //DisplayTextLong = "Collection site property present";
                //DiversityWorkbench.QueryCondition qCollectionSitePropertyPresent = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty", "CollectionEventID", "Site property", DisplayText, DisplayTextLong, Description, QueryCondition.CheckDataExistence.ForeignKeyIsNull);
                //QueryConditions.Add(qCollectionSitePropertyPresent);
                //qCollectionSitePropertyPresent.SqlFromClause = SQL;

                DiversityWorkbench.QueryCondition _qCollectionSitePropertyPresent = new QueryCondition();
                _qCollectionSitePropertyPresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                //_qCollectionSitePropertyPresent.Entity = "LocalisationSystem.LocalisationSystemID.8";
                _qCollectionSitePropertyPresent.DisplayText = "Presence";// "WGS84 present";
                _qCollectionSitePropertyPresent.Description = "If a collection site property is present";// "If any WGS84 coordinates are present";
                _qCollectionSitePropertyPresent.TextFixed = true;
                _qCollectionSitePropertyPresent.Table = "CollectionEventProperty";
                _qCollectionSitePropertyPresent.IdentityColumn = "CollectionSpecimenID";
                _qCollectionSitePropertyPresent.ForeignKey = "CollectionEventID";
                _qCollectionSitePropertyPresent.Column = "CollectionEventID";
                _qCollectionSitePropertyPresent.ForeignKeyTable = "CollectionSpecimen";
                //_qCollectionSitePropertyPresent.Restriction = "LocalisationSystemID = 8";
                _qCollectionSitePropertyPresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qCollectionSitePropertyPresent.QueryGroup = "Site property";
                _qCollectionSitePropertyPresent.useGroupAsEntityForGroups = true;
                DiversityWorkbench.QueryCondition qCollectionSitePropertyPresent = new QueryCondition(_qCollectionSitePropertyPresent);
                QueryConditions.Add(qCollectionSitePropertyPresent);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID ";

                DiversityWorkbench.QueryCondition qSitePropertyDisplayText = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "Site property", "DisplayText", "DisplayText", "DisplayText",
                    false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qSitePropertyDisplayText);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qSitePropertyID = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "PropertyID", "Site property", "ID", "PropertyID", "ID of the property",
                    false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qSitePropertyID);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qSitePropertyURI = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "Site property", "PropertyURI", "URI", "URI",
                    false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qSitePropertyURI);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qSitePropertyHierarchy = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "Site property", "PropertyHierarchyCache", "Hierarchy", "Hierarchy",
                    false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qSitePropertyHierarchy);

                SQL = " FROM " + Prefix + "CollectionSpecimen  S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qSitePropertyNotes = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "Site property", "Notes", "Notes", "Notes",
                    false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qSitePropertyNotes);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (10))";
                DiversityWorkbench.QueryCondition qGeographicRegions = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "Site property", "Geo.reg.", "Geographic regions", "Geographic regions",
                    false, false, false, false, "CollectionEventID", "Property.PropertyID.10");
                qGeographicRegions.Restriction = "PropertyID IN (10)";
                QueryConditions.Add(qGeographicRegions);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (40))";
                DiversityWorkbench.QueryCondition qLebensraumTyp = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "Site property", "Leb.r.typ.", "Lebensraumtyp", "Lebensraumtyp",
                    false, false, false, false, "CollectionEventID", "Property.PropertyID.40");
                qLebensraumTyp.Restriction = "PropertyID IN (40)";
                QueryConditions.Add(qLebensraumTyp);

                #region Chronostratigraphy

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (20))";
                DiversityWorkbench.QueryCondition qChronostratigraphy = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "Chronostratigraphy", "Chron.strat.", "Chronostratigraphy",
                    "Chronostratigraphy", false, false, false, false, "CollectionEventID", "Property.PropertyID.20");
                qChronostratigraphy.Restriction = "PropertyID IN (20)";
                qChronostratigraphy.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qChronostratigraphy);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (20))";
                DiversityWorkbench.QueryCondition qChronoStratigraphyHierarchy = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "PropertyHierarchyCache", "Chronostratigraphy", "Hierarchy", "Hierarchy of the chronostratigraphy",
                    "Hierarchy of the Chronostratigraphy", false, false, false, false, "CollectionEventID", "Property.PropertyID.20");
                qChronoStratigraphyHierarchy.Restriction = "PropertyID IN (20)";
                QueryConditions.Add(qChronoStratigraphyHierarchy);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (20))";
                DiversityWorkbench.QueryCondition qChronoStratigraphyPropertyURI = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "PropertyURI", "Chronostratigraphy", "URI", "URI of the chronostratigraphy",
                    "URI of the Chronostratigraphy", false, false, false, false, "CollectionEventID", "Property.PropertyID.20");
                qChronoStratigraphyPropertyURI.Restriction = "PropertyID IN (20)";
                QueryConditions.Add(qChronoStratigraphyPropertyURI);

                #endregion

                #region Lithostratigraphy

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (30))";
                DiversityWorkbench.QueryCondition qLithoStratigraphy = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "Lithostratigraphy", "Lith.strat.", "Lithostratigraphy",
                    "Lithostratigraphy", false, false, false, false, "CollectionEventID", "Property.PropertyID.30");
                qLithoStratigraphy.Restriction = "PropertyID IN (30)";
                qLithoStratigraphy.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qLithoStratigraphy);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (30))";
                DiversityWorkbench.QueryCondition qLithoStratigraphyHierarchy = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "PropertyHierarchyCache", "Lithostratigraphy", "Hierarchy", "Lithostratigraphy",
                    "Lithostratigraphy", false, false, false, false, "CollectionEventID", "Property.PropertyID.30");
                qLithoStratigraphyHierarchy.Restriction = "PropertyID IN (30)";
                QueryConditions.Add(qLithoStratigraphyHierarchy);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (30))";
                DiversityWorkbench.QueryCondition qLithoStratigraphyPropertyURI = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "PropertyURI", "Lithostratigraphy", "URI", "Lithostratigraphy",
                    "Lithostratigraphy", false, false, false, false, "CollectionEventID", "Property.PropertyID.30");
                qLithoStratigraphyPropertyURI.Restriction = "PropertyID IN (30)";
                QueryConditions.Add(qLithoStratigraphyPropertyURI);

                #endregion

                #region Biostratigraphy

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (60))";
                DiversityWorkbench.QueryCondition qBioStratigraphy = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "Biostratigraphy", "Bio.strat.", "Biostratigraphy",
                    "Biostratigraphy", false, false, false, false, "CollectionEventID", "Property.PropertyID.60");
                qBioStratigraphy.Restriction = "PropertyID IN (60)";
                qBioStratigraphy.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qBioStratigraphy);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (60))";
                DiversityWorkbench.QueryCondition qBioStratigraphyHierarchy = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "PropertyHierarchyCache", "Biostratigraphy", "Hierarchy", "Biostratigraphy",
                    "Biostratigraphy", false, false, false, false, "CollectionEventID", "Property.PropertyID.60");
                qBioStratigraphyHierarchy.Restriction = "PropertyID IN (60)";
                QueryConditions.Add(qBioStratigraphyHierarchy);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (60))";
                DiversityWorkbench.QueryCondition qBioStratigraphyPropertyURI = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "PropertyURI", "Biostratigraphy", "URI", "Biostratigraphy",
                    "Biostratigraphy", false, false, false, false, "CollectionEventID", "Property.PropertyID.60");
                qBioStratigraphyPropertyURI.Restriction = "PropertyID IN (60)";
                QueryConditions.Add(qBioStratigraphyPropertyURI);

                #endregion

                #region Eunis2012

                DiversityWorkbench.QueryCondition _qEunis2012present = new QueryCondition();
                _qEunis2012present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qEunis2012present.Entity = "Property.PropertyID.2";
                _qEunis2012present.DisplayText = "Presence";
                _qEunis2012present.Description = "If any EUNIS 2012 is present";
                _qEunis2012present.TextFixed = true;
                _qEunis2012present.Table = "CollectionEventProperty";
                _qEunis2012present.IdentityColumn = "CollectionSpecimenID";
                _qEunis2012present.ForeignKey = "CollectionEventID";
                _qEunis2012present.Column = "PropertyID";
                _qEunis2012present.ForeignKeyTable = "CollectionSpecimen";
                _qEunis2012present.Restriction = "PropertyID = 2";
                _qEunis2012present.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qEunis2012present.QueryGroup = "EUNIS 2012";
                _qEunis2012present.useGroupAsEntityForGroups = true;
                DiversityWorkbench.QueryCondition qEunis2012present = new QueryCondition(_qEunis2012present);
                QueryConditions.Add(qEunis2012present);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (2))";
                DiversityWorkbench.QueryCondition qEunis2012 = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "DisplayText", "EUNIS 2012", "EUNIS 2012", "EUNIS 2012",
                    "EUNIS 2012", false, false, false, false, "CollectionEventID", "Property.PropertyID.2");
                qEunis2012.Restriction = "PropertyID IN (2)";
                qEunis2012.useGroupAsEntityForGroups = true;
                QueryConditions.Add(qEunis2012);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (2))";
                DiversityWorkbench.QueryCondition qEunis2012Hierarchy = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "PropertyHierarchyCache", "EUNIS 2012", "Hierarchy", "EUNIS 2012",
                    "EUNIS 2012", false, false, false, false, "CollectionEventID", "Property.PropertyID.2");
                qEunis2012Hierarchy.Restriction = "PropertyID IN (2)";
                QueryConditions.Add(qEunis2012Hierarchy);

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventProperty T ON S.CollectionEventID = T.CollectionEventID " +
                    " WHERE (T.PropertyID IN (2))";
                DiversityWorkbench.QueryCondition qEunis2012PropertyURI = new DiversityWorkbench.QueryCondition(false, "CollectionEventProperty",
                    "CollectionSpecimenID", true, SQL, "PropertyURI", "EUNIS 2012", "URI", "EUNIS 2012",
                    "EUNIS 2012", false, false, false, false, "CollectionEventID", "Property.PropertyID.2");
                qEunis2012PropertyURI.Restriction = "PropertyID IN (2)";
                QueryConditions.Add(qEunis2012PropertyURI);

                #endregion

                #endregion

                #region EventImage

                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventImage T ON S.CollectionEventID = T.CollectionEventID ";

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "ImageType");
                DiversityWorkbench.QueryCondition qEventImageType = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "ImageType", "Type", "Image type",
                    false, false, false, false, "CollectionEventID");
                qEventImageType.Description = Description;
                QueryConditions.Add(qEventImageType);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "Notes");
                DiversityWorkbench.QueryCondition qEventImageNotes = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "Notes", "Notes", "Notes",
                    false, false, false, false, "CollectionEventID");
                qEventImageNotes.Description = Description;
                QueryConditions.Add(qEventImageNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qEventImageDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "DataWithholdingReason", "Withhold", "Data withholding reason",
                    false, false, false, false, "CollectionEventID");
                qEventImageDataWithholdingReason.Description = Description;
                QueryConditions.Add(qEventImageDataWithholdingReason);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "URI");
                DiversityWorkbench.QueryCondition qEventImageURI = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "URI", "URI", "URI",
                    false, false, false, false, "CollectionEventID");
                qEventImageURI.Description = Description;
                QueryConditions.Add(qEventImageURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "Description");
                DiversityWorkbench.QueryCondition qEventImageDescription = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "Description", "Description", "Description",
                    false, false, false, false, "CollectionEventID");
                qEventImageDescription.IsEXIF = true;
                qEventImageDescription.Description = Description;
                QueryConditions.Add(qEventImageDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "CreatorAgent");
                DiversityWorkbench.QueryCondition qEventImageCreatorAgent = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "CreatorAgent", "Creator", "Creator of the image",
                    false, false, false, false, "CollectionEventID");
                qEventImageCreatorAgent.Description = Description;
                QueryConditions.Add(qEventImageCreatorAgent);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "Title");
                DiversityWorkbench.QueryCondition qEventImageTitle = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "Title", "Title", "Title",
                    false, false, false, false, "CollectionEventID");
                qEventImageTitle.Description = Description;
                QueryConditions.Add(qEventImageTitle);


                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "IPR");
                DiversityWorkbench.QueryCondition qEventImageIPR = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "IPR", "IPR", "IPR",
                    false, false, false, false, "CollectionEventID");
                qEventImageIPR.Description = Description;
                QueryConditions.Add(qEventImageIPR);
                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "IPR");
                //DiversityWorkbench.QueryCondition qSpecimenImageIPR = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "IPR", "Image", "IPR", "IPR of the image", Description);
                //QueryConditions.Add(qSpecimenImageIPR);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "CopyrightStatement");
                DiversityWorkbench.QueryCondition qEventImageCopyrightStatement = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "CopyrightStatement", "Copyright", "Copyright statement",
                    false, false, false, false, "CollectionEventID");
                qEventImageCopyrightStatement.Description = Description;
                QueryConditions.Add(qEventImageCopyrightStatement);
                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "CopyrightStatement");
                //DiversityWorkbench.QueryCondition qSpecimenImageCopyrightStatement = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "CopyrightStatement", "Image", "Copyright", "Copyright of the image", Description);
                //QueryConditions.Add(qSpecimenImageCopyrightStatement);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "LicenseURI");
                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseURI");
                //DiversityWorkbench.QueryCondition qSpecimenImageLicenseURI = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LicenseURI", "Image", "License URI", "URI of the license", Description);
                //QueryConditions.Add(qSpecimenImageLicenseURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "LicenseHolder");
                DiversityWorkbench.QueryCondition qEventImageLicenseHolder = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "LicenseHolder", "Lic.holder", "License holder",
                    false, false, false, false, "CollectionEventID");
                qEventImageLicenseHolder.Description = Description;
                QueryConditions.Add(qEventImageLicenseHolder);
                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseHolder");
                //DiversityWorkbench.QueryCondition qSpecimenImageLicenseHolder = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LicenseHolder", "Image", "License", "License holder of the image", Description);
                //QueryConditions.Add(qSpecimenImageLicenseHolder);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "LicenseType");
                DiversityWorkbench.QueryCondition qEventImageLicenseType = new DiversityWorkbench.QueryCondition(false, "CollectionEventImage",
                    "CollectionSpecimenID", true, SQL, "URI", "Event image", "LicenseType", "Lic.type", "License type",
                    false, false, false, false, "CollectionEventID");
                qEventImageLicenseType.Description = Description;
                QueryConditions.Add(qEventImageLicenseType);
                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseType");
                //DiversityWorkbench.QueryCondition qSpecimenImageLicenseType = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LicenseType", "Image", "Lic. type", "Type of the license of the image", Description);
                //QueryConditions.Add(qSpecimenImageLicenseType);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "LicenseYear");
                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseYear");
                //DiversityWorkbench.QueryCondition qSpecimenImageLicenseYear = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LicenseYear", "Image", "Lic. year", "Year of the license of the image", Description);
                //QueryConditions.Add(qSpecimenImageLicenseYear);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "InternalNotes");
                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseNotes");
                //DiversityWorkbench.QueryCondition qSpecimenImageLicenseNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "InternalNotes", "Image", "Lic. notes", "Notes about the license of the image", Description);
                //QueryConditions.Add(qSpecimenImageLicenseNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "LogCreatedBy");
                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LogCreatedBy");
                //DiversityWorkbench.QueryCondition qSpecimenImageLogCreatedBy = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LogCreatedBy", "Image", "Creat. by", "The user that created the dataset", Description, dtUser, false);
                //QueryConditions.Add(qSpecimenImageLogCreatedBy);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventImage", "LogCreatedWhen");
                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LogCreatedWhen");
                //DiversityWorkbench.QueryCondition qSpecimenImageLogCreatedWhen = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LogCreatedWhen", "Image", "Creat. date", "The date when the dataset was created", Description, QueryCondition.QueryTypes.DateTime);
                //QueryConditions.Add(qSpecimenImageLogCreatedWhen);


                #endregion

                #region EventRegulation

                //Description = "If any regulation is present";
                //DiversityWorkbench.QueryCondition qEventRegulationExists = new DiversityWorkbench.QueryCondition(false, "CollectionEventRegulation",
                //    "CollectionEventID", "Regulation (event)", "Presence", "Regulation present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                //SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                //    " " + Prefix + "CollectionEventRegulation T ON S.CollectionEventID = T.CollectionEventID ";
                //qEventRegulationExists.SqlFromClause = SQL;
                //QueryConditions.Add(qEventRegulationExists);


                DiversityWorkbench.QueryCondition _qEventRegulationPresent = new QueryCondition();
                _qEventRegulationPresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qEventRegulationPresent.Entity = "CollectionEventRegulation";
                _qEventRegulationPresent.Description = "If any regulation is present";// "If a TK25 entry is present";
                _qEventRegulationPresent.DisplayText = "Presence";// Any regulation present";// "TK25 present";
                _qEventRegulationPresent.TextFixed = true;
                _qEventRegulationPresent.Table = "CollectionEventRegulation";
                _qEventRegulationPresent.IdentityColumn = "CollectionSpecimenID";
                _qEventRegulationPresent.ForeignKey = "CollectionEventID";
                _qEventRegulationPresent.Column = "CollectionEventID";
                _qEventRegulationPresent.ForeignKeyTable = "CollectionSpecimen";
                _qEventRegulationPresent.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qEventRegulationPresent.QueryGroup = "Regulation (event)";
                DiversityWorkbench.QueryCondition qEventRegulationPresent = new QueryCondition(_qEventRegulationPresent);
                QueryConditions.Add(qEventRegulationPresent);


                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventRegulation T ON S.CollectionEventID = T.CollectionEventID ";
                DiversityWorkbench.QueryCondition qEventRegulation = new DiversityWorkbench.QueryCondition(false, "CollectionEventRegulation",
                    "CollectionSpecimenID", true, SQL, "Regulation", "Regulation (event)", "Regulation", "Regulation", "Regulation",
                    false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qEventRegulation);


                //Description = "The number of Regulations";
                //DiversityWorkbench.QueryCondition qRegulationCount = new DiversityWorkbench.QueryCondition(false, "CollectionEventRegulation", "CollectionSpecimenID", "Regulation (event)", "Count", "Number of regulations", Description, QueryCondition.QueryTypes.Count);
                //qRegulationCount.SqlFromClause = SQL;
                //QueryConditions.Add(qRegulationCount);


                #endregion

                #region EventMethod

                System.Data.DataTable dtEventMethod = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT    DISTINCT    M.MethodID, M.MethodParentID, M.DisplayText " +
                    "FROM            " + Prefix + "Method AS M  " +
                    "WHERE        (M.ForCollectionEvent = 1) " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aEventMethod = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aEventMethod.Fill(dtEventMethod); }
                    catch { }
                }
                if (dtEventMethod.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtEventMethod.Columns.Add(Value);
                    dtEventMethod.Columns.Add(ParentValue);
                    dtEventMethod.Columns.Add(Display);
                }
                Description = "The method used in a collection event";
                DiversityWorkbench.QueryCondition qEventMethod = new DiversityWorkbench.QueryCondition(false, "CollectionEventMethod", "CollectionSpecimenID", "MethodID", "Display", "Value", "ParentValue", "Display", "Event method", "Method", "Method used for the collection event", Description, dtEventMethod, false);
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventMethod T ON S.CollectionEventID = T.CollectionEventID ";
                qEventMethod.SqlFromClause = SQL;
                qEventMethod.IsNumeric = true;
                qEventMethod.ForeignKey = "CollectionEventID";
                qEventMethod.ForeignKeyTable = "CollectionEvent";
                QueryConditions.Add(qEventMethod);

                ///ToDo: Missing Methods

                System.Data.DataTable dtEventMethodParameter = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT    DISTINCT    P.ParameterID, NULL AS Expr1, M.DisplayText + ' | ' + P.DisplayText AS Display " +
                    "FROM            " + Prefix + "Parameter AS P INNER JOIN " +
                    "Method AS M ON P.MethodID = M.MethodID WHERE M.ForCollectionEvent = 1 " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aEventMethodParameter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aEventMethodParameter.Fill(dtEventMethodParameter); }
                    catch { }
                }
                if (dtEventMethodParameter.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtEventMethodParameter.Columns.Add(Value);
                    dtEventMethodParameter.Columns.Add(ParentValue);
                    dtEventMethodParameter.Columns.Add(Display);
                }
                Description = "The parameter of a method applied during a collection event";
                DiversityWorkbench.QueryCondition qEventMethodParameter = new DiversityWorkbench.QueryCondition(false, "CollectionEventParameterValue", "CollectionSpecimenID", "ParameterID", "Display", "Value", "ParentValue", "Display", "Event method", "Parameter", "Parameter of the method", Description, dtEventMethodParameter, false);
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                    " " + Prefix + "CollectionEventParameterValue T ON S.CollectionEventID = T.CollectionEventID ";
                qEventMethodParameter.SqlFromClause = SQL;
                qEventMethodParameter.IsNumeric = true;
                QueryConditions.Add(qEventMethodParameter);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventParameterValue", "Value");
                DiversityWorkbench.QueryCondition qEventMethodParameterValue = new DiversityWorkbench.QueryCondition(false, "CollectionEventParameterValue", "CollectionSpecimenID", "Value", "Event method", "Par. value", "Parameter value", Description);
                SQL = " FROM " + Prefix + "CollectionSpecimen S INNER JOIN " +
                   " " + Prefix + "CollectionEventParameterValue T ON S.CollectionEventID = T.CollectionEventID ";
                qEventMethodParameterValue.SqlFromClause = SQL;
                QueryConditions.Add(qEventMethodParameterValue);

                #endregion

                #region EventSeries

                System.Collections.Generic.Dictionary<string, string> DictSeriesID = new Dictionary<string, string>();
                DictSeriesID.Add("SeriesID", "SeriesID");
                System.Collections.Generic.Dictionary<string, string> DictEventID = new Dictionary<string, string>();
                DictEventID.Add("CollectionEventID", "CollectionEventID");

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventSeries", "Description");
                SQL = " FROM " + Prefix + "CollectionEventSeries T INNER JOIN " +
                    " " + Prefix + "CollectionEvent E ON T.SeriesID = E.SeriesID INNER JOIN " +
                    " " + Prefix + "CollectionSpecimen S ON E.CollectionEventID = S.CollectionEventID";
                DiversityWorkbench.QueryCondition qSeriesDescription = new DiversityWorkbench.QueryCondition(false, "CollectionEventSeries", "CollectionSpecimenID", true, SQL, "Description", "Event series", "Descript.", "Series description", Description, false, false, false, false, "CollectionEventID");
                qSeriesDescription.OptimizingLinkColumns = new Dictionary<string, Dictionary<string, string>>();
                qSeriesDescription.OptimizingLinkColumns.Add("CollectionEvent", DictSeriesID);
                qSeriesDescription.OptimizingLinkColumns.Add("CollectionSpecimen_Core2", DictEventID);
                QueryConditions.Add(qSeriesDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventSeries", "SeriesCode");
                SQL = " FROM " + Prefix + "CollectionEventSeries T INNER JOIN " +
                    " " + Prefix + "CollectionEvent E ON T.SeriesID = E.SeriesID INNER JOIN " +
                    " " + Prefix + "CollectionSpecimen S ON E.CollectionEventID = S.CollectionEventID";
                DiversityWorkbench.QueryCondition qSeriesCode = new DiversityWorkbench.QueryCondition(false, "CollectionEventSeries", "CollectionSpecimenID", true, SQL, "SeriesCode", "Event series", "Ser.code.", "Series code", Description, false, false, false, false, "CollectionEventID");
                qSeriesCode.OptimizingLinkColumns = new Dictionary<string, Dictionary<string, string>>();
                qSeriesCode.OptimizingLinkColumns.Add("CollectionEvent", DictSeriesID);
                qSeriesCode.OptimizingLinkColumns.Add("CollectionSpecimen_Core2", DictEventID);
                QueryConditions.Add(qSeriesCode);

                #endregion

                #region EventSeriesImage

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventSeriesImage", "URI");
                SQL = " FROM " + Prefix + "CollectionEventSeriesImage T INNER JOIN " +
                    " " + Prefix + "CollectionEvent E ON T.SeriesID = E.SeriesID INNER JOIN " +
                    " " + Prefix + "CollectionSpecimen S ON E.CollectionEventID = S.CollectionEventID";
                DiversityWorkbench.QueryCondition qSeriesImageURI = new DiversityWorkbench.QueryCondition(false, "CollectionEventSeriesImage", "CollectionSpecimenID", true, SQL, "URI", "Event series image", "URI", "URI", Description, false, false, false, false, "CollectionEventID");
                qSeriesImageURI.OptimizingLinkColumns = new Dictionary<string, Dictionary<string, string>>();
                qSeriesImageURI.OptimizingLinkColumns.Add("CollectionEvent", DictSeriesID);
                qSeriesImageURI.OptimizingLinkColumns.Add("CollectionSpecimen_Core2", DictEventID);
                QueryConditions.Add(qSeriesImageURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventSeriesImage", "ImageType");
                SQL = " FROM " + Prefix + "CollectionEventSeriesImage T INNER JOIN " +
                    " " + Prefix + "CollectionEvent E ON T.SeriesID = E.SeriesID INNER JOIN " +
                    " " + Prefix + "CollectionSpecimen  S ON E.CollectionEventID = S.CollectionEventID";
                DiversityWorkbench.QueryCondition qSeriesImageType = new DiversityWorkbench.QueryCondition(false, "CollectionEventSeriesImage", "CollectionSpecimenID", true, SQL, "ImageType", "Event series image", "Type", "Image type", Description, false, false, false, false, "CollectionEventID");
                qSeriesImageType.OptimizingLinkColumns = new Dictionary<string, Dictionary<string, string>>();
                qSeriesImageType.OptimizingLinkColumns.Add("CollectionEvent", DictSeriesID);
                qSeriesImageType.OptimizingLinkColumns.Add("CollectionSpecimen_Core2", DictEventID);
                QueryConditions.Add(qSeriesImageType);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventSeriesImage", "Notes");
                SQL = " FROM " + Prefix + "CollectionEventSeriesImage T INNER JOIN " +
                    " " + Prefix + "CollectionEvent E ON T.SeriesID = E.SeriesID INNER JOIN " +
                    " " + Prefix + "CollectionSpecimen S ON E.CollectionEventID = S.CollectionEventID";
                DiversityWorkbench.QueryCondition qSeriesImageNotes = new DiversityWorkbench.QueryCondition(false, "CollectionEventSeriesImage", "CollectionSpecimenID", true, SQL, "Notes", "Event series image", "Notes", "Notes", Description, false, false, false, false, "CollectionEventID");
                qSeriesImageNotes.OptimizingLinkColumns = new Dictionary<string, Dictionary<string, string>>();
                qSeriesImageNotes.OptimizingLinkColumns.Add("CollectionEvent", DictSeriesID);
                qSeriesImageNotes.OptimizingLinkColumns.Add("CollectionSpecimen_Core2", DictEventID);
                QueryConditions.Add(qSeriesImageNotes);

                #endregion

                #region CollectionEventSeriesDescriptor

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventSeriesDescriptor", "Descriptor");
                SQL = " FROM " + Prefix + "CollectionEventSeriesDescriptor T INNER JOIN " +
                    " " + Prefix + "CollectionEvent E ON T.SeriesID = E.SeriesID INNER JOIN " +
                    " " + Prefix + "CollectionSpecimen S ON E.CollectionEventID = S.CollectionEventID";
                DiversityWorkbench.QueryCondition qCollectionEventSeriesDescriptor = new DiversityWorkbench.QueryCondition(false, "CollectionEventSeriesDescriptor", "CollectionSpecimenID", true, SQL, "Descriptor", "Event series descriptor", "Descriptor", "Descriptor", Description, false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qCollectionEventSeriesDescriptor);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEventSeriesDescriptor", "DescriptorType");
                SQL = " FROM " + Prefix + "CollectionEventSeriesDescriptor T INNER JOIN " +
                    " " + Prefix + "CollectionEvent E ON T.SeriesID = E.SeriesID INNER JOIN " +
                    " " + Prefix + "CollectionSpecimen S ON E.CollectionEventID = S.CollectionEventID";
                DiversityWorkbench.QueryCondition qCollectionEventSeriesDescriptorType = new DiversityWorkbench.QueryCondition(false, "CollectionEventSeriesDescriptor", "CollectionSpecimenID", true, SQL, "DescriptorType", "Event series descriptor", "Type", "Descriptor type", Description, false, false, false, false, "CollectionEventID");
                QueryConditions.Add(qCollectionEventSeriesDescriptorType);


                #endregion

                #region EXSICCATA

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "ExsiccataAbbreviation");
                DiversityWorkbench.QueryCondition qExsiccataAbbreviation = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimen_Core2", "CollectionSpecimenID", "ExsiccataAbbreviation", "Exsiccata", "Exsiccata", "Exsiccata series", Description);
                QueryConditions.Add(qExsiccataAbbreviation);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "ExsiccataNumber");
                DiversityWorkbench.QueryCondition qExsiccataNumber = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit", "CollectionSpecimenID", "ExsiccataNumber", "Exsiccata", "Number", "Exsiccata number", Description);
                QueryConditions.Add(qExsiccataNumber);

                #endregion

                #region ORGANISM

                //Description = "If any organism is present";
                //DiversityWorkbench.QueryCondition qUnit = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit", "CollectionSpecimenID", "Organism", "Presence", "Organism present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                //QueryConditions.Add(qUnit);

                Description = "The number of organisms";
                DiversityWorkbench.QueryCondition qUnitCount = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "Organism", "Count", "Number of organisms", Description, QueryCondition.QueryTypes.Count);
                QueryConditions.Add(qUnitCount);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "IdentificationUnitID");
                DiversityWorkbench.QueryCondition qIdentificationUnitID = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "IdentificationUnitID", "Organism", "Unit ID", "Identification unit ID", Description, false, false, true, false);
                QueryConditions.Add(qIdentificationUnitID);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "OnlyObserved");
                DiversityWorkbench.QueryCondition qOnlyObserved = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "OnlyObserved", "Organism", "Only obs.", "Only observed", Description, false, false, false, true);
                QueryConditions.Add(qOnlyObserved);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "DisplayOrder");
                DiversityWorkbench.QueryCondition qDisplayOrder = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "DisplayOrder", "Organism", "Displ.ord.", "Display order", Description, false, false, true, false);
                QueryConditions.Add(qDisplayOrder);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "RelationType");
                DiversityWorkbench.QueryCondition qUnitRelationType = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "RelationType", "Organism", "Rel. type", "Relation type", Description, "CollUnitRelationType_Enum", Database);
                QueryConditions.Add(qUnitRelationType);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "ColonisedSubstratePart");
                DiversityWorkbench.QueryCondition qColonisedSubstratePart = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "ColonisedSubstratePart", "Organism", "Col. subst. part", "Colonised substrate part", Description);
                QueryConditions.Add(qColonisedSubstratePart);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "LifeStage");
                DiversityWorkbench.QueryCondition qLifeStage = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "LifeStage", "Organism", "Life stage", "Life stage", Description);
                QueryConditions.Add(qLifeStage);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "Gender");
                DiversityWorkbench.QueryCondition qGender = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "Gender", "Organism", "Gender", "Gender", Description);
                QueryConditions.Add(qGender);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "UnitIdentifier");
                DiversityWorkbench.QueryCondition qUnitIdentifier = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "UnitIdentifier", "Organism", "Identifier", "Unit identifier", Description);
                QueryConditions.Add(qUnitIdentifier);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "UnitDescription");
                DiversityWorkbench.QueryCondition qUnitDescription = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "UnitDescription", "Organism", "Descript.", "Description of the unit", Description);
                QueryConditions.Add(qUnitDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "Circumstances");
                DiversityWorkbench.QueryCondition qCircumstances = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "Circumstances", "Organism", "Circumst.", "Circumstances", Description, "CollCircumstances_Enum", Database);
                QueryConditions.Add(qCircumstances);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "LastIdentificationCache");
                DiversityWorkbench.QueryCondition qLastIdentificationCache = new DiversityWorkbench.QueryCondition(true, "IdentificationUnit_Core2", "CollectionSpecimenID", "LastIdentificationCache", "Organism", this.QueryDisplayColumns()[3].DisplayText, "Last identification", Description);
                QueryConditions.Add(qLastIdentificationCache);

                //Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "TaxonomicGroup");
                //DiversityWorkbench.QueryCondition qTaxonomicGroup = new DiversityWorkbench.QueryCondition(true, "IdentificationUnit_Core2", "CollectionSpecimenID", "TaxonomicGroup", "Organism", "Tax.group", "Taxonomic group", Description, "CollTaxonomicGroup_Enum", Database);
                ////qTaxonomicGroup.IsSet = true;
                //QueryConditions.Add(qTaxonomicGroup);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "RetrievalType");
                DiversityWorkbench.QueryCondition qRetrievalType = new DiversityWorkbench.QueryCondition(true, "IdentificationUnit_Core2", "CollectionSpecimenID", "RetrievalType", "Organism", "Retrieval", "Retrieval type", Description, "CollRetrievalType_Enum", Database);
                QueryConditions.Add(qRetrievalType);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "FamilyCache");
                DiversityWorkbench.QueryCondition qFamily = new DiversityWorkbench.QueryCondition(true, "IdentificationUnit_Core2", "CollectionSpecimenID", "FamilyCache", "Organism", "Family", "Family", Description);
                QueryConditions.Add(qFamily);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "OrderCache");
                DiversityWorkbench.QueryCondition qOrder = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "OrderCache", "Organism", "Order", "Order", Description);
                QueryConditions.Add(qOrder);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "HierarchyCache");
                DiversityWorkbench.QueryCondition qHierarchyCache = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "HierarchyCache", "Organism", "Hierarchy", "Taxonomic hierarchy", Description);
                QueryConditions.Add(qHierarchyCache);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "Notes");
                DiversityWorkbench.QueryCondition qUnitNotes = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "Notes", "Organism", "Notes", "Notes about the unit", Description);
                QueryConditions.Add(qUnitNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qUnitDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "DataWithholdingReason", "Organism", "Withhold", "Data withholding reason of the unit", Description);
                QueryConditions.Add(qUnitDataWithholdingReason);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "TaxonomicGroup");
                DiversityWorkbench.QueryCondition qTaxonomicGroupSet = new DiversityWorkbench.QueryCondition(true, "IdentificationUnit_Core2", "CollectionSpecimenID", "TaxonomicGroup", "Taxonomic group", "Tax. group", "Taxonomic group", Description, "CollTaxonomicGroup_Enum", Database);
                qTaxonomicGroupSet.IsSet = true;
                QueryConditions.Add(qTaxonomicGroupSet);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "NumberOfUnits");
                DiversityWorkbench.QueryCondition qNumberOfUnits = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit_Core2", "CollectionSpecimenID", "NumberOfUnits", "Organism", "Number", "Number of organisms", Description, false, false, true, false);
                QueryConditions.Add(qNumberOfUnits);

                #endregion

                #region IDENTIFICATION

                //Description = "If any identification is present";
                //DiversityWorkbench.QueryCondition qIdentification = new DiversityWorkbench.QueryCondition(false, "Identification", "CollectionSpecimenID", "Identification", "Presence", "Identification present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                //QueryConditions.Add(qIdentification);

                Description = "The number of identifications";
                DiversityWorkbench.QueryCondition qIdentificationCount = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "Identification", "Count", "Number of identifications", Description, QueryCondition.QueryTypes.Count);
                qIdentificationCount.IntermediateTable = "IdentificationUnit";
                qIdentificationCount.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qIdentificationCount);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "TypeStatus");
                DiversityWorkbench.QueryCondition qTypeStatus = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "TypeStatus", "Identification", "Type status", "Type status", Description, "CollTypeStatus_Enum", Database);
                qTypeStatus.IntermediateTable = "IdentificationUnit";
                qTypeStatus.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qTypeStatus);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "TypeNotes");
                DiversityWorkbench.QueryCondition qTypeNotes = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "TypeNotes", "Identification", "Type notes", "Type notes", Description);
                qTypeNotes.IntermediateTable = "IdentificationUnit";
                qTypeNotes.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qTypeNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "IdentificationQualifier");
                DiversityWorkbench.QueryCondition qIdentificationQualifier = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "IdentificationQualifier", "Identification", "Qualifier", "Identification qualifier", Description, "CollIdentificationQualifier_Enum", Database);
                qIdentificationQualifier.IntermediateTable = "IdentificationUnit";
                qIdentificationQualifier.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qIdentificationQualifier);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "TaxonomicName");
                DiversityWorkbench.QueryCondition qTaxonomicName = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "TaxonomicName", "Identification", this.QueryDisplayColumns()[4].DisplayText, "Taxonomic name", Description);
                qTaxonomicName.IntermediateTable = "IdentificationUnit";
                qTaxonomicName.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qTaxonomicName);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "NameURI");
                DiversityWorkbench.QueryCondition qNameURI = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "NameURI", "Identification", "Name URI", "Name URI", Description);
                qNameURI.IntermediateTable = "IdentificationUnit";
                qNameURI.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qNameURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "VernacularTerm");
                DiversityWorkbench.QueryCondition qVernacularTerm = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "VernacularTerm", "Identification", "Vern. term", "Vernacular term or common name", Description);
                qVernacularTerm.IntermediateTable = "IdentificationUnit";
                qVernacularTerm.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qVernacularTerm);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "TermURI");
                DiversityWorkbench.QueryCondition qTermURI = new DiversityWorkbench.QueryCondition(false, "Identification", "CollectionSpecimenID", "TermURI", "Identification", "Term URI", "Term URI", Description);
                qTermURI.IntermediateTable = "IdentificationUnit";
                qTermURI.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qTermURI);


                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "ResponsibleName");
                DiversityWorkbench.QueryCondition qResponsibleName = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "ResponsibleName", "Identification", "Respons.", "Name of the person responsible for the identification", Description);
                qResponsibleName.IntermediateTable = "IdentificationUnit";
                qResponsibleName.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qResponsibleName);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "IdentificationCategory");
                DiversityWorkbench.QueryCondition qIdentificationCategory = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "IdentificationCategory", "Identification", "Category", "Identification category", Description, "CollIdentificationCategory_Enum", Database);
                qIdentificationCategory.IntermediateTable = "IdentificationUnit";
                qIdentificationCategory.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qIdentificationCategory);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "IdentificationSequence");
                DiversityWorkbench.QueryCondition qIdentificationSequence = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "IdentificationSequence", "Identification", "Sequence", "Sequence", Description, false, false, true, false);
                QueryConditions.Add(qIdentificationSequence);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "Notes");
                DiversityWorkbench.QueryCondition qIdentificationNotes = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "Notes", "Identification", "Notes", "Notes", Description);
                qIdentificationNotes.IntermediateTable = "IdentificationUnit";
                qIdentificationNotes.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qIdentificationNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "ReferenceTitle");
                DiversityWorkbench.QueryCondition qIdentificationReferenceTitle = new DiversityWorkbench.QueryCondition(false, "Identification_Core2", "CollectionSpecimenID", "ReferenceTitle", "Identification", "Reference", "Title of the reference", Description);
                qIdentificationNotes.IntermediateTable = "IdentificationUnit";
                qIdentificationNotes.ForeignKeySecondColumn = "IdentificationUnitID";
                QueryConditions.Add(qIdentificationReferenceTitle);


                DiversityWorkbench.QueryCondition _qTaxonSelection = new QueryCondition();
                _qTaxonSelection.QueryType = QueryCondition.QueryTypes.Module;
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                _qTaxonSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
                _qTaxonSelection.Entity = "Identification.NameURI";
                _qTaxonSelection.DisplayText = "Taxa";
                _qTaxonSelection.DisplayLongText = "Selection of taxa";
                _qTaxonSelection.Table = "Identification";
                _qTaxonSelection.IdentityColumn = "CollectionSpecimenID";
                _qTaxonSelection.Column = "NameURI";
                _qTaxonSelection.UpperValue = "TaxonomicName";
                _qTaxonSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qTaxonSelection.QueryGroup = "Identification";
                _qTaxonSelection.Description = "All taxa from the list";
                _qTaxonSelection.QueryType = QueryCondition.QueryTypes.Module;
                QueryConditions.Add(_qTaxonSelection);

                DiversityWorkbench.QueryCondition _qTermSelection = new QueryCondition();
                _qTermSelection.QueryType = QueryCondition.QueryTypes.Module;
                DiversityWorkbench.ScientificTerm ST = new DiversityWorkbench.ScientificTerm(DiversityWorkbench.Settings.ServerConnection);
                _qTermSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)ST;
                _qTermSelection.Entity = "Identification.TermURI";
                _qTermSelection.DisplayText = "Terms";
                _qTermSelection.DisplayLongText = "Selection of terms";
                _qTermSelection.Table = "Identification";
                _qTermSelection.IdentityColumn = "CollectionSpecimenID";
                _qTermSelection.Column = "TermURI";
                _qTermSelection.UpperValue = "VernacularTerm";
                _qTermSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qTermSelection.QueryGroup = "Identification";
                _qTermSelection.Description = "All terms from the list";
                _qTermSelection.QueryType = QueryCondition.QueryTypes.Module;
                QueryConditions.Add(_qTermSelection);

#if DEBUG
                DiversityWorkbench.QueryCondition _qDDTaxonSelection = new QueryCondition();
                _qDDTaxonSelection.QueryType = QueryCondition.QueryTypes.Module;
                DiversityWorkbench.Description D = new DiversityWorkbench.Description(DiversityWorkbench.Settings.ServerConnection);
                _qDDTaxonSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)D;
                _qDDTaxonSelection.Entity = "Identification.NameURI";
                _qDDTaxonSelection.DisplayText = "Taxa";
                _qDDTaxonSelection.DisplayLongText = "Selection of taxa from DiversityDescriptions";
                _qDDTaxonSelection.Table = "Identification";
                _qDDTaxonSelection.IdentityColumn = "CollectionSpecimenID";
                _qDDTaxonSelection.Column = "NameURI";
                _qDDTaxonSelection.UpperValue = "TaxonomicName";
                _qDDTaxonSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qDDTaxonSelection.QueryGroup = "Identification";
                _qDDTaxonSelection.Description = "All taxa from the list";
                _qDDTaxonSelection.QueryType = QueryCondition.QueryTypes.Module;
                QueryConditions.Add(_qDDTaxonSelection);
#endif

                #endregion

                #region SUBSTRATE

                SQL = " FROM " + Prefix + "IdentificationUnit U INNER JOIN " +
                    "" + Prefix + "IdentificationUnit U_1 ON U.CollectionSpecimenID = U_1.CollectionSpecimenID AND " +
                    "U.IdentificationUnitID = U_1.RelatedUnitID ";
                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "LastIdentificationCache");
                DiversityWorkbench.QueryCondition qSubstrate = new DiversityWorkbench.QueryCondition(true, "IdentificationUnit", "CollectionSpecimenID", "LastIdentificationCache", "Substrate", "Taxon", "Substrate taxon", Description);
                qSubstrate.IntermediateTable = "IdentificationUnit";
                qSubstrate.ForeignKeySecondColumn = "IdentificationUnitID";
                qSubstrate.ForeignKeySecondColumnInForeignTable = "RelatedUnitID";
                QueryConditions.Add(qSubstrate);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "TaxonomicGroup");
                DiversityWorkbench.QueryCondition qSubstrateTaxonomicGroup = new DiversityWorkbench.QueryCondition(false, "IdentificationUnit", "CollectionSpecimenID", "TaxonomicGroup", "Substrate", "Tax.group", "Substrate taxonomic group", Description, "CollTaxonomicGroup_Enum", Database);
                qSubstrateTaxonomicGroup.IntermediateTable = "IdentificationUnit";
                qSubstrateTaxonomicGroup.ForeignKeySecondColumn = "IdentificationUnitID";
                qSubstrateTaxonomicGroup.ForeignKeySecondColumnInForeignTable = "RelatedUnitID";
                QueryConditions.Add(qSubstrateTaxonomicGroup);

                Description = DiversityWorkbench.Functions.ColumnDescription("Identification", "TaxonomicName");
                DiversityWorkbench.QueryCondition qSubstrateTaxonomicName = new DiversityWorkbench.QueryCondition(false, "Identification", "CollectionSpecimenID", "TaxonomicName", "Substrate", "Tax.name", "Substrate taxonomic name", Description);
                qSubstrateTaxonomicName.IntermediateTable = "IdentificationUnit";
                qSubstrateTaxonomicName.ForeignKeySecondColumn = "IdentificationUnitID";
                qSubstrateTaxonomicName.ForeignKeySecondColumnInForeignTable = "RelatedUnitID";
                QueryConditions.Add(qSubstrateTaxonomicName);

                #endregion

                #region ANALYSIS

                Description = "If any analysis is present";
                DiversityWorkbench.QueryCondition qUnitAnalysis = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysis", "CollectionSpecimenID", "Analysis", "Presence", "Analysis present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qUnitAnalysis);

                System.Data.DataTable dtAnalysis = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT AnalysisID, AnalysisParentID, DisplayText " +
                    "FROM " + Prefix + "Analysis " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aAnalysis = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aAnalysis.Fill(dtAnalysis); }
                    catch { }
                }
                if (dtAnalysis.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtAnalysis.Columns.Add(Value);
                    dtAnalysis.Columns.Add(ParentValue);
                    dtAnalysis.Columns.Add(Display);
                }
                Description = "The analysis applied on a specimen";
                DiversityWorkbench.QueryCondition qAnalysis = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysis", "CollectionSpecimenID", "AnalysisID", "Display", "Value", "ParentValue", "Display", "Analysis", "Type", "Type of the analysis", Description, dtAnalysis, false);
                qAnalysis.ForeignKeySecondColumn = "IdentificationUnitID";
                qAnalysis.IntermediateTable = "IdentificationUnit";
                qAnalysis.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                qAnalysis.IsNumeric = true;
                QueryConditions.Add(qAnalysis);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitAnalysis", "AnalysisNumber");
                DiversityWorkbench.QueryCondition qAnalysisNumber = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysis", "CollectionSpecimenID", "AnalysisNumber", "Analysis", "Number", "Analysis number", Description);
                qAnalysisNumber.ForeignKeySecondColumn = "IdentificationUnitID";
                qAnalysisNumber.IntermediateTable = "IdentificationUnit";
                qAnalysisNumber.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                QueryConditions.Add(qAnalysisNumber);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitAnalysis", "AnalysisResult");
                DiversityWorkbench.QueryCondition qAnalysisResult = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysis", "CollectionSpecimenID", "AnalysisResult", "Analysis", "Result", "Analysis result", Description);
                qAnalysisResult.ForeignKeySecondColumn = "IdentificationUnitID";
                qAnalysisResult.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                qAnalysisResult.IntermediateTable = "IdentificationUnit";
                QueryConditions.Add(qAnalysisResult);

                Description = "Results of the analysis as numeric values";
                DiversityWorkbench.QueryCondition qAnalysisResultNumeric = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysis", "CollectionSpecimenID", "AnalysisResult", "Analysis", "Result num.", "Analysis result numeric", Description);
                qAnalysisResultNumeric.ForeignKeySecondColumn = "IdentificationUnitID";
                qAnalysisResultNumeric.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                qAnalysisResultNumeric.IntermediateTable = "IdentificationUnit";
                qAnalysisResultNumeric.IsTextAsNumeric = true;
                QueryConditions.Add(qAnalysisResultNumeric);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitAnalysis", "AnalysisDate");
                DiversityWorkbench.QueryCondition qAnalysisDate = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysis", "CollectionSpecimenID", "AnalysisDate", "Analysis", "Date", "Analysis date", Description);
                qAnalysisDate.ForeignKeySecondColumn = "IdentificationUnitID";
                qAnalysisDate.IntermediateTable = "IdentificationUnit";
                qAnalysisDate.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                QueryConditions.Add(qAnalysisDate);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitAnalysis", "Notes");
                DiversityWorkbench.QueryCondition qAnalysisNotes = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysis", "CollectionSpecimenID", "Notes", "Analysis", "Notes", "Analysis notes", Description);
                qAnalysisNotes.ForeignKeySecondColumn = "IdentificationUnitID";
                qAnalysisNotes.IntermediateTable = "IdentificationUnit";
                qAnalysisNotes.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                QueryConditions.Add(qAnalysisNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitAnalysis", "ResponsibleName");
                DiversityWorkbench.QueryCondition qAnalysisResponsibleName = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysis", "CollectionSpecimenID", "ResponsibleName", "Analysis", "Responsible", "Analysis responsible", Description);
                qAnalysisNotes.ForeignKeySecondColumn = "IdentificationUnitID";
                qAnalysisNotes.IntermediateTable = "IdentificationUnit";
                qAnalysisNotes.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                QueryConditions.Add(qAnalysisResponsibleName);


                #region Method

                System.Data.DataTable dtAnalysisMethod = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT   DISTINCT     M.MethodID, M.MethodParentID, M.DisplayText " +
                    "FROM            " + Prefix + "Method AS M INNER JOIN " +
                    "" + Prefix + "MethodForAnalysis AS A ON M.MethodID = A.MethodID " +
                    "WHERE        (M.OnlyHierarchy = 0) " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aAnalysisMethod = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aAnalysisMethod.Fill(dtAnalysisMethod); }
                    catch { }
                }
                if (dtAnalysisMethod.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtAnalysisMethod.Columns.Add(Value);
                    dtAnalysisMethod.Columns.Add(ParentValue);
                    dtAnalysisMethod.Columns.Add(Display);
                }
                Description = "The analysis method applied on a specimen";
                DiversityWorkbench.QueryCondition qIdentificationUnitAnalysisMethod = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysisMethod", "CollectionSpecimenID", "MethodID", "Display", "Value", "ParentValue", "Display", "Analysis", "Method", "Method used for the analysis", Description, dtAnalysisMethod, false);
                qIdentificationUnitAnalysisMethod.ForeignKeySecondColumn = "IdentificationUnitID";
                qIdentificationUnitAnalysisMethod.IntermediateTable = "IdentificationUnit";
                qIdentificationUnitAnalysisMethod.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                qIdentificationUnitAnalysisMethod.IsNumeric = true;
                QueryConditions.Add(qIdentificationUnitAnalysisMethod);

                System.Data.DataTable dtAnalysisMethodParameter = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT   DISTINCT     P.ParameterID, NULL AS Expr1, M.DisplayText + ' | ' + P.DisplayText AS Display " +
                    "FROM            " + Prefix + "Parameter AS P INNER JOIN " +
                    "" + Prefix + "MethodForAnalysis AS A ON P.MethodID = A.MethodID INNER JOIN " +
                    "" + Prefix + "Method AS M ON P.MethodID = M.MethodID AND A.MethodID = M.MethodID " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aAnalysisMethodParameter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aAnalysisMethodParameter.Fill(dtAnalysisMethodParameter); }
                    catch { }
                }
                if (dtAnalysisMethod.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtAnalysisMethodParameter.Columns.Add(Value);
                    dtAnalysisMethodParameter.Columns.Add(ParentValue);
                    dtAnalysisMethodParameter.Columns.Add(Display);
                }
                Description = "The parameter of a method applied on a specimen";
                DiversityWorkbench.QueryCondition qIdentificationUnitAnalysisMethodParameter = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysisMethodParameter", "CollectionSpecimenID", "ParameterID", "Display", "Value", "ParentValue", "Display", "Analysis", "Parameter", "Parameter of the method", Description, dtAnalysisMethodParameter, false);
                qIdentificationUnitAnalysisMethodParameter.ForeignKeySecondColumn = "IdentificationUnitID";
                qIdentificationUnitAnalysisMethodParameter.IntermediateTable = "IdentificationUnit";
                qIdentificationUnitAnalysisMethodParameter.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                qIdentificationUnitAnalysisMethodParameter.IsNumeric = true;
                QueryConditions.Add(qIdentificationUnitAnalysisMethodParameter);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitAnalysisMethodParameter", "Value");
                DiversityWorkbench.QueryCondition qIdentificationUnitAnalysisMethodParameterValue = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitAnalysisMethodParameter", "CollectionSpecimenID", "Value", "Analysis", "Par. value", "Parameter value", Description);
                qIdentificationUnitAnalysisMethodParameterValue.ForeignKeySecondColumn = "IdentificationUnitID";
                qIdentificationUnitAnalysisMethodParameterValue.IntermediateTable = "IdentificationUnit";
                qIdentificationUnitAnalysisMethodParameterValue.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                QueryConditions.Add(qIdentificationUnitAnalysisMethodParameterValue);

                #endregion

                #endregion

                #region GEOANALYSIS

                Description = "If any geo analysis is present";
                DiversityWorkbench.QueryCondition qUnitGeoAnalysis = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitGeoAnalysis", "CollectionSpecimenID", "Geoanalysis", "Presence", "Geoanalysis present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qUnitGeoAnalysis);

                //Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitGeoAnalysis", "Geography");
                //SQL = " FROM CollectionSpecimen INNER JOIN " +
                //    " IdentificationUnitGeoAnalysis ON CollectionSpecimen.CollectionSpecimenID = IdentificationUnitGeoAnalysis.CollectionSpecimenID " +
                //    " WHERE (NOT IdentificationUnitGeoAnalysis.Geography IS NULL)";
                DiversityWorkbench.QueryCondition _qGeoAnalysisGeography = new QueryCondition();
                _qGeoAnalysisGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.DatasetsInRelatedTable;
                _qGeoAnalysisGeography.Entity = "IdentificationUnitGeoAnalysis.Geography";
                //_qGeoAnalysisGeography.SqlFromClause = SQL;
                _qGeoAnalysisGeography.TextFixed = false;
                _qGeoAnalysisGeography.Description = Description;
                _qGeoAnalysisGeography.Table = "IdentificationUnitGeoAnalysis";
                _qGeoAnalysisGeography.IdentityColumn = "CollectionSpecimenID";
                _qGeoAnalysisGeography.ForeignKeySecondColumn = "IdentificationUnitID";
                _qGeoAnalysisGeography.IntermediateTable = "IdentificationUnit";
                _qGeoAnalysisGeography.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                _qGeoAnalysisGeography.Column = "Geography";
                _qGeoAnalysisGeography.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qGeoAnalysisGeography.QueryGroup = "Geoanalysis";
                //_qGeoAnalysisGeography.IsGeography = true;
                _qGeoAnalysisGeography.QueryType = QueryCondition.QueryTypes.Geography;
                DiversityWorkbench.QueryCondition qGeoAnalysisGeography = new QueryCondition(_qGeoAnalysisGeography);
                QueryConditions.Add(qGeoAnalysisGeography);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitGeoAnalysis", "AnalysisDate");
                DiversityWorkbench.QueryCondition qGeoAnalysisDate = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitGeoAnalysis", "CollectionSpecimenID", "AnalysisDate", "Geoanalysis", "Date", "Analysis date", Description);
                qGeoAnalysisDate.ForeignKeySecondColumn = "IdentificationUnitID";
                qGeoAnalysisDate.IntermediateTable = "IdentificationUnit";
                qGeoAnalysisDate.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                QueryConditions.Add(qGeoAnalysisDate);

                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitGeoAnalysis", "Notes");
                DiversityWorkbench.QueryCondition qGeoAnalysisNotes = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitGeoAnalysis", "CollectionSpecimenID", "Notes", "Geoanalysis", "Notes", "Analysis notes", Description);
                qGeoAnalysisNotes.ForeignKeySecondColumn = "IdentificationUnitID";
                qGeoAnalysisNotes.IntermediateTable = "IdentificationUnit";
                qGeoAnalysisNotes.ForeignKeySecondColumnInForeignTable = "IdentificationUnitID";
                QueryConditions.Add(qGeoAnalysisNotes);

                #endregion

                #region AGENT

                //Description = "If any collector is present";
                //DiversityWorkbench.QueryCondition qAgent = new DiversityWorkbench.QueryCondition(false, "CollectionAgent", "CollectionSpecimenID", "Collector", "Presence", "Collector present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                //QueryConditions.Add(qAgent);
#if DEBUG
                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionAgent", "CollectorsName");
                DiversityWorkbench.QueryCondition qCollectors = new DiversityWorkbench.QueryCondition(true, "CollectionAgent_Core", "CollectionSpecimenID", "CollectorsName", "Collectors", "Collector", "Collector", Description);
                qCollectors.IsSet = true;
                QueryConditions.Add(qCollectors);
#endif

                Description = "The number of collectors";
                DiversityWorkbench.QueryCondition qAgentCount = new DiversityWorkbench.QueryCondition(false, "CollectionAgent", "CollectionSpecimenID", "Collector", "Count", "Number of collectors", Description, QueryCondition.QueryTypes.Count);
                QueryConditions.Add(qAgentCount);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionAgent", "CollectorsName");
                DiversityWorkbench.QueryCondition qCollectorsName = new DiversityWorkbench.QueryCondition(true, "CollectionAgent_Core", "CollectionSpecimenID", "CollectorsName", "Collector", "Collector", "Collector", Description);
                QueryConditions.Add(qCollectorsName);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionAgent", "CollectorsAgentURI");
                DiversityWorkbench.QueryCondition qCollectorsAgentURI = new DiversityWorkbench.QueryCondition(false, "CollectionAgent_Core", "CollectionSpecimenID", "CollectorsAgentURI", "Collector", "Name URI", "Name URI", Description);
                QueryConditions.Add(qCollectorsAgentURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionAgent", "CollectorsNumber");
                DiversityWorkbench.QueryCondition qCollectorsNumber = new DiversityWorkbench.QueryCondition(false, "CollectionAgent_Core", "CollectionSpecimenID", "CollectorsNumber", "Collector", "Nr.", "Number", Description);
                QueryConditions.Add(qCollectorsNumber);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionAgent", "Notes");
                DiversityWorkbench.QueryCondition qAgentNotes = new DiversityWorkbench.QueryCondition(false, "CollectionAgent_Core", "CollectionSpecimenID", "Notes", "Collector", "Notes", "Notes", Description);
                QueryConditions.Add(qAgentNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionAgent", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qAgentDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "CollectionAgent_Core", "CollectionSpecimenID", "DataWithholdingReason", "Collector", "Withhold.", "Data withholding reason", Description);
                QueryConditions.Add(qAgentDataWithholdingReason);

#if DEBUG

                DiversityWorkbench.QueryCondition _qAgentSelection = new QueryCondition();
                _qAgentSelection.QueryType = QueryCondition.QueryTypes.Module;
                DiversityWorkbench.Agent DA = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                _qAgentSelection.iWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)DA;
                _qAgentSelection.Entity = "CollectionAgent.CollectorsAgentURI";
                _qAgentSelection.DisplayText = "Agents";
                _qAgentSelection.DisplayLongText = "Selection of agents";
                _qAgentSelection.Table = "CollectionAgent";
                _qAgentSelection.IdentityColumn = "CollectionSpecimenID";
                _qAgentSelection.Column = "CollectorsAgentURI";
                _qAgentSelection.CheckIfDataExist = QueryCondition.CheckDataExistence.NoCheck;
                _qAgentSelection.QueryGroup = "Collector";
                _qAgentSelection.Description = "All agents from the list";
                QueryConditions.Add(_qAgentSelection);

#endif
                #endregion

                #region RELATION

                Description = "If any relation is present";
                DiversityWorkbench.QueryCondition qRelatedSpecimenPresence = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelation", "CollectionSpecimenID", "Relation", "Presence", "Relation present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qRelatedSpecimenPresence);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelation", "RelatedSpecimenDisplayText");
                DiversityWorkbench.QueryCondition qRelatedSpecimenDisplayText = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelation", "CollectionSpecimenID", "RelatedSpecimenDisplayText", "Relation", "Rel.specimen", "Related specimen", Description);
                QueryConditions.Add(qRelatedSpecimenDisplayText);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelation", "RelatedSpecimenDescription");
                DiversityWorkbench.QueryCondition qRelatedSpecimenDescription = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelation", "CollectionSpecimenID", "RelatedSpecimenDescription", "Relation", "Description", "Description", Description);
                QueryConditions.Add(qRelatedSpecimenDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelation", "Notes");
                DiversityWorkbench.QueryCondition qRelatedSpecimenNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelation", "CollectionSpecimenID", "Notes", "Relation", "Notes", "Notes", Description);
                QueryConditions.Add(qRelatedSpecimenNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelation", "RelationType");
                DiversityWorkbench.QueryCondition qRelationType = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelation", "CollectionSpecimenID", "RelationType", "Relation", "Relation type", "Relation type", Description, "CollSpecimenRelationType_Enum", Database);
                QueryConditions.Add(qRelationType);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelation", "IsInternalRelationCache");
                DiversityWorkbench.QueryCondition qIsInternalRelationCache = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelation", "CollectionSpecimenID", "IsInternalRelationCache", "Relation", "Internal", "Is internal relation", Description, QueryCondition.QueryTypes.Boolean);
                QueryConditions.Add(qIsInternalRelationCache);

                #endregion

                #region RELATION internal

                Description = "If any internal relation is present";
                DiversityWorkbench.QueryCondition qRelatedSpecimenInternalPresence = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInternal", "CollectionSpecimenID", "Internal relation", "Presence", "Relation present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qRelatedSpecimenInternalPresence);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationInternal", "RelatedSpecimenDisplayText");
                DiversityWorkbench.QueryCondition qRelatedSpecimenDisplayTextInternal = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInternal", "CollectionSpecimenID", "RelatedSpecimenDisplayText", "Internal relation", "Rel.specimen", "Internal related specimen", Description);
                QueryConditions.Add(qRelatedSpecimenDisplayTextInternal);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationInternal", "RelatedSpecimenDescription");
                DiversityWorkbench.QueryCondition qRelatedSpecimenDescriptionInternal = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInternal", "CollectionSpecimenID", "RelatedSpecimenDescription", "Internal relation", "Description", "Description", Description);
                QueryConditions.Add(qRelatedSpecimenDescriptionInternal);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationInternal", "Notes");
                DiversityWorkbench.QueryCondition qRelatedSpecimenNotesInternal = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInternal", "CollectionSpecimenID", "Notes", "Internal relation", "Notes", "Notes", Description);
                QueryConditions.Add(qRelatedSpecimenNotesInternal);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationInternal", "RelationType");
                DiversityWorkbench.QueryCondition qRelationTypeInternal = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInternal", "CollectionSpecimenID", "RelationType", "Internal relation", "Rel. type", "Relation type", Description, "CollSpecimenRelationType_Enum", Database);
                QueryConditions.Add(qRelationTypeInternal);

                #endregion

                #region RELATION external

                //Description = "If any External relation is present";
                //DiversityWorkbench.QueryCondition qRelatedSpecimenExternalPresence = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationExternal", "CollectionSpecimenID", "External relation", "Presence", "Relation present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                //QueryConditions.Add(qRelatedSpecimenExternalPresence);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationExternal", "RelatedSpecimenDisplayText");
                //DiversityWorkbench.QueryCondition qRelatedSpecimenDisplayTextExternal = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationExternal", "CollectionSpecimenID", "RelatedSpecimenDisplayText", "External relation", "Rel.specimen", "External related specimen", Description);
                //QueryConditions.Add(qRelatedSpecimenDisplayTextExternal);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationExternal", "RelatedSpecimenDescription");
                //DiversityWorkbench.QueryCondition qRelatedSpecimenDescriptionExternal = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationExternal", "CollectionSpecimenID", "RelatedSpecimenDescription", "External relation", "Description", "Description", Description);
                //QueryConditions.Add(qRelatedSpecimenDescriptionExternal);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationExternal", "Notes");
                //DiversityWorkbench.QueryCondition qRelatedSpecimenNotesExternal = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationExternal", "CollectionSpecimenID", "Notes", "External relation", "Notes", "Notes", Description);
                //QueryConditions.Add(qRelatedSpecimenNotesExternal);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationExternal", "RelationType");
                //DiversityWorkbench.QueryCondition qRelationTypeExternal = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationExternal", "CollectionSpecimenID", "RelationType", "External relation", "Rel. type", "Relation type", Description, "CollSpecimenRelationType_Enum", Database);
                //QueryConditions.Add(qRelationTypeExternal);

                #endregion



                #region RELATION invers

                //Description = "If any invers relation is present";
                //DiversityWorkbench.QueryCondition qRelatedSpecimenInversPresence = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInvers", "CollectionSpecimenID", "Invers relation", "Presence", "Relation present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                //QueryConditions.Add(qRelatedSpecimenInversPresence);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationInvers", "RelatedSpecimenAccessionNumber");
                //DiversityWorkbench.QueryCondition qRelatedSpecimenDisplayTextInvers = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInvers", "CollectionSpecimenID", "RelatedSpecimenAccessionNumber", "Invers relation", "Rel.specimen", "Invers related specimen", Description);
                //QueryConditions.Add(qRelatedSpecimenDisplayTextInvers);

                ////Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationInvers", "RelatedSpecimenDescription");
                ////DiversityWorkbench.QueryCondition qRelatedSpecimenDescriptionInvers = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInvers", "CollectionSpecimenID", "RelatedSpecimenDescription", "Relation", "Description", "Description", Description);
                ////QueryConditions.Add(qRelatedSpecimenDescriptionInvers);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationInvers", "Notes");
                //DiversityWorkbench.QueryCondition qRelatedSpecimenNotesInvers = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInvers", "CollectionSpecimenID", "Notes", "Invers relation", "Notes", "Notes", Description);
                //QueryConditions.Add(qRelatedSpecimenNotesInvers);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenRelationInvers", "RelationType");
                //DiversityWorkbench.QueryCondition qRelationTypeInvers = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenRelationInvers", "CollectionSpecimenID", "RelationType", "Invers relation", "Rel. type", "Relation type", Description, "CollSpecimenRelationType_Enum", Database);
                //QueryConditions.Add(qRelationTypeInvers);

                #endregion

                #region PART

                Description = "If any part is present";
                DiversityWorkbench.QueryCondition qPart = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "Storage", "Presence", "Part present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qPart);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "AccessionNumber");
                DiversityWorkbench.QueryCondition qAccessionNumberPart = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "AccessionNumber", "Storage", "Acc.Nr.part", "Accession number of part", Description);
                QueryConditions.Add(qAccessionNumberPart);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "SpecimenPartID");
                DiversityWorkbench.QueryCondition qSpecimenPartID = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart_Core", "CollectionSpecimenID", "SpecimenPartID", "Storage", "Part ID", "Specimen part ID", Description, false, false, true, false);
                QueryConditions.Add(qSpecimenPartID);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "PartSublabel");
                DiversityWorkbench.QueryCondition qPartSublabel = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "PartSublabel", "Storage", "Part label", "Label of part", Description);
                QueryConditions.Add(qPartSublabel);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "StorageLocation");
                DiversityWorkbench.QueryCondition qStorageLocation = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "StorageLocation", "Storage", "Stor. location", "Storage location", Description);
                QueryConditions.Add(qStorageLocation);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "MaterialCategory");
                DiversityWorkbench.QueryCondition qMaterialCategory = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "MaterialCategory", "Storage", "Material", "Material category",
                    Description, "CollMaterialCategory_Enum", Database, true, false);
                QueryConditions.Add(qMaterialCategory);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "Notes");
                DiversityWorkbench.QueryCondition qStorageNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "Notes", "Storage", "Notes", "Notes", Description);
                QueryConditions.Add(qStorageNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qStorageDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "DataWithholdingReason", "Storage", "Withhold.", "Data withholding reason", Description);
                QueryConditions.Add(qStorageDataWithholdingReason);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "StorageContainer");
                DiversityWorkbench.QueryCondition qStorageContainer = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "StorageContainer", "Storage", "Container", "Storage container", Description);
                QueryConditions.Add(qStorageContainer);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "Stock");
                DiversityWorkbench.QueryCondition qStock = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart_Core", "CollectionSpecimenID", "Stock", "Storage", "Stock", "Stock", Description, false, false, true, false);
                QueryConditions.Add(qStock);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "StockUnit");
                DiversityWorkbench.QueryCondition qStockUnit = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart_Core", "CollectionSpecimenID", "StockUnit", "Storage", "Unit", "Stock unit", Description);
                QueryConditions.Add(qStockUnit);

                System.Data.DataTable dtCollection = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display, NULL AS DisplayOrder " +
                    "UNION ";
                // Markus 20.8.24: Anzeige der Hierarchie sofern nicht via Linked Server
                if (IsLinkedServer)
                    SQL += "SELECT CollectionID AS [Value], CollectionParentID, CollectionName, DisplayOrder " +
                    "FROM " + Prefix + "Collection ";
                else
                    SQL += "SELECT CollectionID AS [Value], CollectionParentID, DisplayText, DisplayOrder " +
                    "FROM " + Prefix + "CollectionHierarchyAll() ";
                SQL+= "ORDER BY Display ";

                Microsoft.Data.SqlClient.SqlDataAdapter aColl = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aColl.Fill(dtCollection); }
                    catch { }
                }
                if (dtCollection.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    System.Data.DataColumn DisplayOrder = new System.Data.DataColumn("DisplayOrder");
                    dtCollection.Columns.Add(Value);
                    dtCollection.Columns.Add(ParentValue);
                    dtCollection.Columns.Add(Display);
                    dtCollection.Columns.Add(DisplayOrder);
                }
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FF = new List<QueryField>();
                //DiversityWorkbench.QueryField CPC = new QueryField("CollectionSpecimenPart_Core", "CollectionID", "CollectionSpecimenID");
                //FF.Add(CPC);
                //DiversityWorkbench.QueryField CS_C = new QueryField("CollectionSpecimen_Core", "CollectionID", "CollectionSpecimenID");
                //FF.Add(CS_C);
                //Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "CollectionName");
                //DiversityWorkbench.QueryCondition qCollection = new DiversityWorkbench.QueryCondition(true, FF, "Specimen", "Collection", "Collection", Description, dtCollection, true, "DisplayOrder", "ParentValue", "Display", "Value");
                //QueryConditions.Add(qCollection);

                System.Collections.Generic.List<DiversityWorkbench.QueryField> FFPart = new List<QueryField>();
                DiversityWorkbench.QueryField CPC_Part = new QueryField("CollectionSpecimenPart_Core", "CollectionID", "CollectionSpecimenID");
                FFPart.Add(CPC_Part);
                //DiversityWorkbench.QueryField CS_C_Part = new QueryField("CollectionSpecimen_Core", "CollectionID", "CollectionSpecimenID");
                //FFPart.Add(CS_C);
                Description = DiversityWorkbench.Functions.ColumnDescription("Collection", "CollectionName");
                DiversityWorkbench.QueryCondition qCollection_Part = new DiversityWorkbench.QueryCondition(true, FFPart, "Storage", "Collection", "Collection", Description, dtCollection, true, "DisplayOrder", "ParentValue", "Display", "Value");
                QueryConditions.Add(qCollection_Part);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "PreparationMethod");
                DiversityWorkbench.QueryCondition qPreparationMethod = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "PreparationMethod", "Storage", "Pre. meth.", "Preparation method", Description);
                QueryConditions.Add(qPreparationMethod);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPart_Core", "PreparationDate");
                DiversityWorkbench.QueryCondition qPreparationDate = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPart", "CollectionSpecimenID", "PreparationDate", "Storage", "Prep. date", "Preparation date", Description);
                QueryConditions.Add(qPreparationDate);

                #endregion

                #region PartRegulation

                //Description = "If any regulation is present";
                //DiversityWorkbench.QueryCondition qPartRegulationExists = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPartRegulation",
                //    "CollectionSpecimenID", "Regulation (part)", "Presence", "Regulation present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                //QueryConditions.Add(qPartRegulationExists);

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPartRegulation", "Regulation");
                //DiversityWorkbench.QueryCondition qPartRegulation = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPartRegulation", "CollectionSpecimenID", "Regulation", "Regulation (part)", "Regulation", "Regulation", Description);
                //QueryConditions.Add(qPartRegulation);

                #endregion

                #region Unit in part

                Description = "If any unit in part is present";
                DiversityWorkbench.QueryCondition qUnitInPart = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitInPart", "CollectionSpecimenID", "Unit in part", "Presence", "Unit in part present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qUnitInPart);

                //Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnitInPart", "Description");
                //DiversityWorkbench.QueryCondition qDescription = new DiversityWorkbench.QueryCondition(false, "IdentificationUnitInPart", "CollectionSpecimenID", "Description", "Unit in part", "Description", "Description", Description);
                //QueryConditions.Add(qDescription);

                #endregion

                #region CollectionTask

                Description = "If any task is present";
                DiversityWorkbench.QueryCondition qCollectionTaskPresent = new DiversityWorkbench.QueryCondition(false, "CollectionTask", "CollectionSpecimenID", "Task", "Presence", "Collection task present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qCollectionTaskPresent);

                // Task
                System.Data.DataTable dtTask = new System.Data.DataTable();
                if (this._ServerConnection.LinkedServer.Length > 0)
                {
                    SQL = "SELECT NULL AS TaskID, NULL AS DisplayText UNION " +
                        "SELECT TaskID, DisplayText " +
                        "FROM " + Prefix + "[Task] " +
                        "ORDER BY DisplayText ";
                }
                else
                {
                    SQL = "SELECT NULL AS TaskID, NULL AS DisplayText UNION " +
                        "SELECT TaskID, HierarchyDisplayText AS DisplayText  " +
                        "FROM " + Prefix + "[TaskHierarchyAll] () T " +
                        "ORDER BY DisplayText ";
                }

                Microsoft.Data.SqlClient.SqlDataAdapter aTask = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aTask.Fill(dtTask); }
                    catch { }
                }
                if (dtTask.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("TaskID");
                    System.Data.DataColumn Display = new System.Data.DataColumn("DisplayText");
                    dtTask.Columns.Add(Value);
                    dtTask.Columns.Add(Display);
                }
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FFTask = new List<QueryField>();
                DiversityWorkbench.QueryField CSCTask = new QueryField("CollectionTask", "TaskID", "CollectionSpecimenID");
                FFTask.Add(CSCTask);
                Description = DiversityWorkbench.Functions.ColumnDescription("Task", "TransactionTitle");
                DiversityWorkbench.QueryCondition qTask = new DiversityWorkbench.QueryCondition(false, FFTask, "Task", "Type", "Task type", Description, dtTask, true);
                QueryConditions.Add(qTask);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionTask", "DisplayText");
                DiversityWorkbench.QueryCondition qCollectionTask = new DiversityWorkbench.QueryCondition(false, "CollectionTask", "CollectionSpecimenID", "DisplayText", "Task", "Task", "Task", Description);
                QueryConditions.Add(qCollectionTask);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionTask", "Result");
                DiversityWorkbench.QueryCondition qCollectionTaskResult = new DiversityWorkbench.QueryCondition(false, "CollectionTask", "CollectionSpecimenID", "Result", "Task", "Result", "Result", Description);
                QueryConditions.Add(qCollectionTaskResult);


                #endregion

                #region TRANSACTION
                Description = "If any transaction is present";
                DiversityWorkbench.QueryCondition qTransactionPresent = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenTransaction", "CollectionSpecimenID", "Transaction", "Presence", "Transaction present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qTransactionPresent);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenTransaction", "AccessionNumber");
                DiversityWorkbench.QueryCondition qTransactionAccessionNumber = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenTransaction", "CollectionSpecimenID", "AccessionNumber", "Transaction", "Acc.Nr.", "Accession number", Description, false, false, false, false);
                QueryConditions.Add(qTransactionAccessionNumber);
#if DEBUG // Markus 17.8.23: besser hierarchie. mit der ausgabe passt irgendwas nicht. statt id wird Transaction angezeigt
                Description = DiversityWorkbench.Functions.ColumnDescription("Transaction", "TransactionID");
                DiversityWorkbench.QueryCondition qTransactionID = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenTransaction", "CollectionSpecimenID", "TransactionID", "Transaction", "Tr.ID", "Trans.ID", Description, false, false, true, false);
                QueryConditions.Add(qTransactionID);
#endif

                // Transaction
                System.Data.DataTable dtTransaction = new System.Data.DataTable();
                if (this._ServerConnection.LinkedServer.Length > 0)
                {
                    SQL = "SELECT NULL AS TransactionID, NULL AS TransactionTitle UNION " +
                        "SELECT TransactionID, TransactionTitle  " +
                        "FROM " + Prefix + "[Transaction] " +
                        "ORDER BY TransactionTitle ";
                }
                else
                {
                    SQL = "SELECT NULL AS TransactionID, NULL AS TransactionTitle UNION " +
                        "SELECT T.TransactionID, T.HierarchyDisplayText AS TransactionTitle " +
                        "FROM " + Prefix + "[TransactionHierarchyAll] () T " +
                        "ORDER BY TransactionTitle ";
                }
                Microsoft.Data.SqlClient.SqlDataAdapter aTransaction = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aTransaction.Fill(dtTransaction); }
                    catch { }
                }
                if (dtTransaction.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("TransactionID");
                    System.Data.DataColumn Display = new System.Data.DataColumn("TransactionTitle");
                    dtTransaction.Columns.Add(Value);
                    dtTransaction.Columns.Add(Display);
                }
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FFTransaction = new List<QueryField>();
                DiversityWorkbench.QueryField CSCTransaction = new QueryField("CollectionSpecimenTransaction", "TransactionID", "CollectionSpecimenID");
                FFTransaction.Add(CSCTransaction);

                Description = DiversityWorkbench.Functions.ColumnDescription("Transaction", "TransactionTitle");
                //DiversityWorkbench.QueryCondition qTransaction = new DiversityWorkbench.QueryCondition(false, FFTransaction, "Transaction", "Transact.", "Transaction", Description, dtTransaction, true, "TransactionTitle", "ParentTransactionID", "TransactionTitle", "TransactionID");
                DiversityWorkbench.QueryCondition qTransaction = new DiversityWorkbench.QueryCondition(false, FFTransaction, "Transaction", "Transact.", "Transaction", Description, dtTransaction, true);
                QueryConditions.Add(qTransaction);

                //Microsoft.Data.SqlClient.SqlDataAdapter aTransaction = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                //if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                //{
                //    try { aTransaction.Fill(dtTransaction); }
                //    catch { }
                //}
                //if (dtTransaction.Columns.Count == 0)
                //{
                //    System.Data.DataColumn Value = new System.Data.DataColumn("TransactionID");
                //    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentTransactionID");
                //    System.Data.DataColumn Display = new System.Data.DataColumn("TransactionTitle");
                //    dtTransaction.Columns.Add(Value);
                //    dtTransaction.Columns.Add(ParentValue);
                //    dtTransaction.Columns.Add(Display);
                //}
                //System.Collections.Generic.List<DiversityWorkbench.QueryField> FFTransaction = new List<QueryField>();
                //DiversityWorkbench.QueryField CSCTransaction = new QueryField("CollectionSpecimenTransaction", "TransactionID", "CollectionSpecimenID");
                //FFTransaction.Add(CSCTransaction);

                //Description = DiversityWorkbench.Functions.ColumnDescription("Transaction", "TransactionTitle");
                //DiversityWorkbench.QueryCondition qTransaction = new DiversityWorkbench.QueryCondition(false, FFTransaction, "Transaction", "Transact.", "Transaction", Description, dtTransaction, true, "TransactionTitle", "ParentTransactionID", "TransactionTitle", "TransactionID");
                //QueryConditions.Add(qTransaction);


                // Return
                System.Data.DataTable dtReturnTransaction = new System.Data.DataTable();
                SQL = "SELECT NULL AS TransactionID, NULL AS ParentTransactionID, NULL AS TransactionTitle UNION " +
                    "SELECT TransactionID, ParentTransactionID, TransactionTitle  " +
                    "FROM " + Prefix + "[Transaction] " +
                    "WHERE TransactionType IN ('return') " +
                    "ORDER BY TransactionTitle ";
                Microsoft.Data.SqlClient.SqlDataAdapter aReturnTransaction = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aReturnTransaction.Fill(dtReturnTransaction); }
                    catch { }
                }
                if (dtReturnTransaction.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("TransactionID");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentTransactionID");
                    System.Data.DataColumn Display = new System.Data.DataColumn("TransactionTitle");
                    dtReturnTransaction.Columns.Add(Value);
                    dtReturnTransaction.Columns.Add(ParentValue);
                    dtReturnTransaction.Columns.Add(Display);
                }
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FFReturnTransaction = new List<QueryField>();
                DiversityWorkbench.QueryField CSCReturnTransaction = new QueryField("CollectionSpecimenTransaction", "TransactionReturnID", "CollectionSpecimenID");
                FFReturnTransaction.Add(CSCReturnTransaction);

                Description = DiversityWorkbench.Functions.ColumnDescription("Transaction", "TransactionTitle");
                DiversityWorkbench.QueryCondition qReturnTransaction = new DiversityWorkbench.QueryCondition(false, FFReturnTransaction, "Transaction", "Return", "Return", Description, dtReturnTransaction, true, "TransactionTitle", "ParentTransactionID", "TransactionTitle", "TransactionID");
                QueryConditions.Add(qReturnTransaction);


                Description = DiversityWorkbench.Functions.ColumnDescription("Transaction", "ToTransactionNumber");
                SQL = " FROM " + Prefix + "CollectionSpecimenTransaction S INNER JOIN " +
                    " " + Prefix + "[Transaction] T ON T.TransactionID = S.TransactionID ";
                DiversityWorkbench.QueryCondition qToTransactionNumber = new DiversityWorkbench.QueryCondition(false, "[Transaction]",
                    "CollectionSpecimenID", true, SQL, "ToTransactionNumber", "Transaction", "->Inv.Nr.", "Inventary number of receiver", Description,
                    false, false, false, false, "TransactionID", "ToTransactionNumber");
                qToTransactionNumber.IntermediateTable = "CollectionSpecimenTransaction";
                qToTransactionNumber.ForeignKey = "TransactionID";
                QueryConditions.Add(qToTransactionNumber);

                Description = DiversityWorkbench.Functions.ColumnDescription("Transaction", "FromTransactionNumber");
                SQL = " FROM " + Prefix + "CollectionSpecimenTransaction S INNER JOIN " +
                    " " + Prefix + "[Transaction] T ON T.TransactionID = S.TransactionID ";
                DiversityWorkbench.QueryCondition qFromTransactionNumber = new DiversityWorkbench.QueryCondition(false, "[Transaction]",
                    "CollectionSpecimenID", true, SQL, "FromTransactionNumber", "Transaction", "Inv.Nr.->", "Inventary number of sender", Description,
                    false, false, false, false, "TransactionID", "FromTransactionNumber");
                qFromTransactionNumber.IntermediateTable = "CollectionSpecimenTransaction";
                qFromTransactionNumber.ForeignKey = "TransactionID";
                QueryConditions.Add(qFromTransactionNumber);

                Description = DiversityWorkbench.Functions.ColumnDescription("Transaction", "TransactionType");
                SQL = " FROM " + Prefix + "CollectionSpecimenTransaction S INNER JOIN " +
                    " " + Prefix + "[Transaction] T ON T.TransactionID = S.TransactionID ";
                DiversityWorkbench.QueryCondition qTransactionType = new DiversityWorkbench.QueryCondition(false, "[Transaction]",
                    "CollectionSpecimenID", true, SQL, "TransactionType", "Transaction", "Type", "Transaction type", Description,
                    false, false, false, false, "TransactionID", "TransactionType");
                qTransactionType.IntermediateTable = "CollectionSpecimenTransaction";
                qTransactionType.ForeignKey = "TransactionID";
                qTransactionType.SelectFromList = true;
                System.Data.DataTable dtEnum = DiversityWorkbench.EnumTable.EnumTableForQuery("CollTransactionType_Enum");
                qTransactionType.dtValues = dtEnum;
                QueryConditions.Add(qTransactionType);

                #endregion

                #region PROCESSING

                Description = "If any transaction is present";
                DiversityWorkbench.QueryCondition qProcessingPresent = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenProcessing", "CollectionSpecimenID", "Processing", "Presence", "Processing present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qProcessingPresent);

                System.Data.DataTable dtProcessing = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT ProcessingID, ProcessingParentID, DisplayText " +
                    "FROM " + Prefix + "Processing " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aProcessing = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aProcessing.Fill(dtProcessing); }
                    catch { }
                }
                if (dtProcessing.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtProcessing.Columns.Add(Value);
                    dtProcessing.Columns.Add(ParentValue);
                    dtProcessing.Columns.Add(Display);
                }
                Description = "The Processing applied on a specimen";
                DiversityWorkbench.QueryCondition qProcessing = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenProcessing", "CollectionSpecimenID", "ProcessingID", "Display", "Value", "ParentValue", "Display", "Processing", "Type", "Type of the Processing", Description, dtProcessing, false);
                QueryConditions.Add(qProcessing);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenProcessing", "Protocoll");
                DiversityWorkbench.QueryCondition qProtocoll = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenProcessing", "CollectionSpecimenID", "Protocoll", "Processing", "Protocoll", "Protocoll", Description);
                QueryConditions.Add(qProtocoll);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenProcessing", "ResponsibleName");
                DiversityWorkbench.QueryCondition qProcessingResponsible = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenProcessing", "CollectionSpecimenID", "ResponsibleName", "Processing", "Responsible", "Responsible", Description);
                QueryConditions.Add(qProcessingResponsible);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenProcessing", "Notes");
                DiversityWorkbench.QueryCondition qProcessingNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenProcessing", "CollectionSpecimenID", "Notes", "Processing", "Notes", "Processing Notes", Description);
                QueryConditions.Add(qProcessingNotes);

                #endregion

                #region ProcessingMethod

                System.Data.DataTable dtProcessingMethod = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT DISTINCT M.MethodID, M.MethodParentID, M.DisplayText " +
                    "FROM " + Prefix + "Method AS M,  " +
                    " " + Prefix + "MethodForProcessing P WHERE " +
                    "P.MethodID = M.MethodID " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aProcessingMethod = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aProcessingMethod.Fill(dtProcessingMethod); }
                    catch { }
                }
                if (dtProcessingMethod.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtProcessingMethod.Columns.Add(Value);
                    dtProcessingMethod.Columns.Add(ParentValue);
                    dtProcessingMethod.Columns.Add(Display);
                }
                Description = "The method applied during a processing";
                DiversityWorkbench.QueryCondition qProcessingMethod = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenProcessingMethod", "CollectionSpecimenID", "MethodID", "Display", "Value", "ParentValue", "Display", "Processing method", "Method", "Method used for the processing", Description, dtProcessingMethod, false);
                qProcessingMethod.IsNumeric = true;
                QueryConditions.Add(qProcessingMethod);

                System.Data.DataTable dtProcessingMethodParameter = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS [ParentValue], NULL AS Display UNION " +
                    "SELECT DISTINCT P.ParameterID, NULL AS ParentValue, M.DisplayText + ' | ' + P.DisplayText AS Display " +
                    "FROM " + Prefix + "Parameter AS P, " +
                    Prefix + "Method AS M, " +
                    Prefix + "MethodForProcessing AS MP " +
                    "WHERE P.MethodID = MP.MethodID " +
                    "AND MP.MethodID = M.MethodID " +
                    "ORDER BY Display ";
                Microsoft.Data.SqlClient.SqlDataAdapter aProcessingMethodParameter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aProcessingMethodParameter.Fill(dtProcessingMethodParameter); }
                    catch { }
                }
                if (dtProcessingMethodParameter.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn ParentValue = new System.Data.DataColumn("ParentValue");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtProcessingMethodParameter.Columns.Add(Value);
                    dtProcessingMethodParameter.Columns.Add(ParentValue);
                    dtProcessingMethodParameter.Columns.Add(Display);
                }
                Description = "The parameter of a method applied during a collection Processing";
                DiversityWorkbench.QueryCondition qProcessingMethodParameter = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenProcessingMethodParameter", "CollectionSpecimenID", "ParameterID", "Display", "Value", "ParentValue", "Display", "Processing method", "Parameter", "Parameter of the method", Description, dtProcessingMethodParameter, false);
                qProcessingMethodParameter.IsNumeric = true;
                QueryConditions.Add(qProcessingMethodParameter);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenProcessingMethodParameter", "Value");
                DiversityWorkbench.QueryCondition qProcessingMethodParameterValue = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenProcessingMethodParameter", "CollectionSpecimenID", "Value", "Processing method", "Par. value", "Parameter value", Description);
                QueryConditions.Add(qProcessingMethodParameterValue);

                #endregion

                #region PartDescription

                Description = "If any part description is present";
                DiversityWorkbench.QueryCondition qSpecimenPartDescriptionPresence = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPartDescription", "CollectionSpecimenID", "Part description", "Presence", "Part present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qSpecimenPartDescriptionPresence);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPartDescription", "Description");
                DiversityWorkbench.QueryCondition qSpecimenPartDescription = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPartDescription", "CollectionSpecimenID", "Description", "Part description", "Description", "Description", Description);
                QueryConditions.Add(qSpecimenPartDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPartDescription", "DescriptionTermURI");
                DiversityWorkbench.QueryCondition qSpecimenPartDescriptionLink = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPartDescription", "CollectionSpecimenID", "DescriptionTermURI", "Part description", "Term URI", "URI of the term in DiveristyScientificTerms", Description);
                QueryConditions.Add(qSpecimenPartDescriptionLink);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenPartDescription", "Notes");
                DiversityWorkbench.QueryCondition qSpecimenPartDescriptionNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenPartDescription", "CollectionSpecimenID", "Notes", "Part description", "Notes", "Notes", Description);
                QueryConditions.Add(qSpecimenPartDescriptionNotes);

                #endregion

                #region Image

                Description = "If any image is present";
                DiversityWorkbench.QueryCondition qImage = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "Image", "Presence", "Image present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
                QueryConditions.Add(qImage);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "Title");
                DiversityWorkbench.QueryCondition qSpecimenImageTitle = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "Title", "Image", "Title", "Title of the image", Description);
                QueryConditions.Add(qSpecimenImageTitle);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "ImageType");
                DiversityWorkbench.QueryCondition qImageType = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "ImageType", "Image", "Type", "Type", Description, "CollSpecimenImageType_Enum", Database);
                QueryConditions.Add(qImageType);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "Notes");
                DiversityWorkbench.QueryCondition qImageNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "Notes", "Image", "Notes", "Notes", Description);
                QueryConditions.Add(qImageNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "InternalNotes");
                DiversityWorkbench.QueryCondition qSpecimenImageInternalNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "InternalNotes", "Image", "Int.Notes", "Internal notes", Description);
                QueryConditions.Add(qSpecimenImageInternalNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qSpecimenImageDataWithholdingReason = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "DataWithholdingReason", "Image", "Withhold", "DataWithholdingReason of the image", Description);
                QueryConditions.Add(qSpecimenImageDataWithholdingReason);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "DisplayOrder");
                DiversityWorkbench.QueryCondition qImageDisplayOrder = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "DisplayOrder", "Image", "Dis.Order", "DisplayOrder", Description, false, false, true, false);
                QueryConditions.Add(qImageDisplayOrder);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "URI");
                DiversityWorkbench.QueryCondition qImageURI = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "URI", "Image", "File", "Path and file name", Description);
                QueryConditions.Add(qImageURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "Description");
                DiversityWorkbench.QueryCondition qSpecimenImageDescription = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "Description", true, "Image", "Description", "Description", Description);
                qSpecimenImageDescription.IsEXIF = true;
                QueryConditions.Add(qSpecimenImageDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "CreatorAgent");
                DiversityWorkbench.QueryCondition qSpecimenImageCreatorAgent = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "CreatorAgent", "Image", "Creator", "Creator of the image", Description);
                QueryConditions.Add(qSpecimenImageCreatorAgent);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "IPR");
                DiversityWorkbench.QueryCondition qSpecimenImageIPR = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "IPR", "Image", "IPR", "IPR of the image", Description);
                QueryConditions.Add(qSpecimenImageIPR);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "CopyrightStatement");
                DiversityWorkbench.QueryCondition qSpecimenImageCopyrightStatement = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "CopyrightStatement", "Image", "Copyright", "Copyright of the image", Description);
                QueryConditions.Add(qSpecimenImageCopyrightStatement);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseURI");
                DiversityWorkbench.QueryCondition qSpecimenImageLicenseURI = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LicenseURI", "Image", "License URI", "URI of the license", Description);
                QueryConditions.Add(qSpecimenImageLicenseURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseHolder");
                DiversityWorkbench.QueryCondition qSpecimenImageLicenseHolder = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LicenseHolder", "Image", "Lic. holder", "License holder of the image", Description);
                QueryConditions.Add(qSpecimenImageLicenseHolder);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseType");
                DiversityWorkbench.QueryCondition qSpecimenImageLicenseType = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LicenseType", "Image", "Lic. type", "Type of the license of the image", Description);
                QueryConditions.Add(qSpecimenImageLicenseType);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseYear");
                DiversityWorkbench.QueryCondition qSpecimenImageLicenseYear = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LicenseYear", "Image", "Lic. year", "Year of the license of the image", Description);
                QueryConditions.Add(qSpecimenImageLicenseYear);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LicenseNotes");
                DiversityWorkbench.QueryCondition qSpecimenImageLicenseNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LicenseNotes", "Image", "Lic. notes", "Notes about the license of the image", Description);
                QueryConditions.Add(qSpecimenImageLicenseNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LogCreatedBy");
                DiversityWorkbench.QueryCondition qSpecimenImageLogCreatedBy = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LogCreatedBy", "Image", "Creat. by", "The user that created the dataset", Description, dtUser, false);
                QueryConditions.Add(qSpecimenImageLogCreatedBy);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImage", "LogCreatedWhen");
                DiversityWorkbench.QueryCondition qSpecimenImageLogCreatedWhen = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImage", "CollectionSpecimenID", "LogCreatedWhen", "Image", "Creat. date", "The date when the dataset was created", Description, QueryCondition.QueryTypes.DateTime);
                QueryConditions.Add(qSpecimenImageLogCreatedWhen);


                #region Image properties

                //Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenImageProperty", "Property");
                //DiversityWorkbench.QueryCondition qSpecimenImageProperty = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenImageProperty", "CollectionSpecimenID", "Property", "Image", "Property", "Property", Description);
                //qTypeNotes.IntermediateTable = "CollectionSpecimenImage";
                //qTypeNotes.ForeignKeySecondColumn = "URI";
                //QueryConditions.Add(qSpecimenImageProperty);

                #endregion

                #endregion

                #region Reference

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenReference", "ReferenceTitle");
                DiversityWorkbench.QueryCondition qSpecimenReferenceTitle = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenReference", "CollectionSpecimenID", "ReferenceTitle", "Reference", "Reference", "Reference", Description);
                QueryConditions.Add(qSpecimenReferenceTitle);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenReference", "ReferenceURI");
                DiversityWorkbench.QueryCondition qSpecimenReferenceURI = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenReference", "CollectionSpecimenID", "ReferenceURI", "Reference", "URI", "URI", Description);
                QueryConditions.Add(qSpecimenReferenceURI);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenReference", "ReferenceDetails");
                DiversityWorkbench.QueryCondition qSpecimenReferenceDetails = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenReference", "CollectionSpecimenID", "ReferenceDetails", "Reference", "Details", "Details", Description);
                QueryConditions.Add(qSpecimenReferenceDetails);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimenReference", "Notes");
                DiversityWorkbench.QueryCondition qSpecimenReferenceNotes = new DiversityWorkbench.QueryCondition(false, "CollectionSpecimenReference", "CollectionSpecimenID", "Notes", "Reference", "Notes", "Notes", Description);
                QueryConditions.Add(qSpecimenReferenceNotes);

                #endregion

                #region External Identifier

                System.Collections.Generic.Dictionary<string, DiversityWorkbench.ReferencingTableLink> IdentifierLinks = new Dictionary<string, ReferencingTableLink>();
                DiversityWorkbench.ReferencingTableLink IDevent = new ReferencingTableLink();
                IDevent.ReferencedTable = "CollectionEvent";
                IDevent.ReferencedColumn = "CollectionEventID";
                IDevent.LinkTable = "CollectionSpecimen";
                IDevent.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionEventID = DiversityWorkbench.Entity.EntityInformation("CollectionEvent");
                IDevent.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictCollectionEventID);
                if (IDevent.DisplayText.Length == 0)
                    IDevent.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionEventID);
                IdentifierLinks.Add("CollectionEvent", IDevent);

                DiversityWorkbench.ReferencingTableLink IDspecimen = new ReferencingTableLink();
                IDspecimen.ReferencedTable = "CollectionSpecimen";
                IDspecimen.ReferencedColumn = "CollectionSpecimenID";
                IDspecimen.LinkTable = "CollectionSpecimen";
                IDspecimen.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionSpecimenID = DiversityWorkbench.Entity.EntityInformation("CollectionSpecimen");
                IDspecimen.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictCollectionSpecimenID);
                if (IDspecimen.DisplayText.Length == 0)
                    IDspecimen.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionSpecimenID);
                IdentifierLinks.Add("CollectionSpecimen", IDspecimen);

                DiversityWorkbench.ReferencingTableLink IDspecimenpart = new ReferencingTableLink();
                IDspecimenpart.ReferencedTable = "CollectionSpecimenPart";
                IDspecimenpart.ReferencedColumn = "SpecimenPartID";
                IDspecimenpart.LinkTable = "CollectionSpecimenPart";
                IDspecimenpart.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionSpecimenPartID = DiversityWorkbench.Entity.EntityInformation("CollectionSpecimenPart");
                IDspecimenpart.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictCollectionSpecimenPartID);
                if (IDspecimenpart.DisplayText.Length == 0)
                    IDspecimenpart.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionSpecimenPartID);
                IdentifierLinks.Add("CollectionSpecimenPart", IDspecimenpart);

                DiversityWorkbench.ReferencingTableLink IDunit = new ReferencingTableLink();
                IDunit.ReferencedTable = "IdentificationUnit";
                IDunit.ReferencedColumn = "IdentificationUnitID";
                IDunit.LinkTable = "IdentificationUnit";
                IDunit.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictIdentificationUnitID = DiversityWorkbench.Entity.EntityInformation("IdentificationUnit");
                IDunit.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictIdentificationUnitID);
                if (IDunit.DisplayText.Length == 0)
                    IDunit.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictIdentificationUnitID);
                IdentifierLinks.Add("IdentificationUnit", IDunit);

                DiversityWorkbench.ReferencingTableLink IDReference = new ReferencingTableLink();
                IDReference.ReferencedTable = "CollectionSpecimenReference";
                IDReference.ReferencedColumn = "ReferenceID";
                IDReference.LinkTable = "CollectionSpecimenReference";
                IDReference.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionSpecimenReferenceID = DiversityWorkbench.Entity.EntityInformation("CollectionSpecimenReference");
                IDReference.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDictCollectionSpecimenReferenceID);
                if (IDReference.DisplayText.Length == 0)
                    IDReference.DisplayText = "Reference";
                if (IDReference.DisplayText.Length == 0)
                    IDReference.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionSpecimenReferenceID);
                IdentifierLinks.Add("CollectionSpecimenReference", IDReference);

                DiversityWorkbench.ReferencingTable ID = new ReferencingTable("ExternalIdentifier", IdentifierLinks);

                Description = DiversityWorkbench.Functions.ColumnDescription("ExternalIdentifier", "Identifier");
                DiversityWorkbench.QueryCondition qIdentifier = new DiversityWorkbench.QueryCondition(false, "Identifier", "External identifier", "Identifier", "Identifier", Description, ID);
                QueryConditions.Add(qIdentifier);



                //System.Data.DataTable dtIdentifierType = new System.Data.DataTable();
                //SQL = "SELECT NULL AS [Value] " +
                //    "UNION " +
                //    "SELECT Type AS [Value] " +
                //    "FROM " + Prefix + "ExternalIdentifierType " +
                //    "ORDER BY Value ";
                //Microsoft.Data.SqlClient.SqlDataAdapter aIdentifierType = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
                //if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                //{
                //    try { aColl.Fill(dtIdentifierType); }
                //    catch { }
                //}
                //if (dtIdentifierType.Columns.Count == 0)
                //{
                //    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                //    dtIdentifierType.Columns.Add(Value);
                //}
                //System.Collections.Generic.List<DiversityWorkbench.QueryField> FFIdentifierType = new List<QueryField>();
                //DiversityWorkbench.QueryField CPC_IdentifierType = new QueryField("Identifier", "Type", "CollectionSpecimenID");
                //FFIdentifierType.Add(CPC_IdentifierType);
                //Description = DiversityWorkbench.Functions.ColumnDescription("Identifier", "Type");
                ////DiversityWorkbench.QueryCondition qIdentifierType = new DiversityWorkbench.QueryCondition(true, FFIdentifierType, "External identifier", "Type", "Type", Description, dtIdentifierType, false);
                //DiversityWorkbench.QueryCondition qIdentifierType = new DiversityWorkbench.QueryCondition(true, "Identifier", "CollectionSpecimenID", "Type", "External identifier", "Type", "Type", Description, dtIdentifierType, false);
                //QueryConditions.Add(qIdentifierType);


                //Description = DiversityWorkbench.Functions.ColumnDescription("ExternalIdentifier", "Type");
                //DiversityWorkbench.QueryCondition qIdentifierType = new DiversityWorkbench.QueryCondition(false, "Identifier", "CollectionSpecimenID", "Type",
                //    "External identifier", "Type", "Type", Description, "ExternalIdentifierType", Database, true, false);
                //QueryConditions.Add(qIdentifierType);

                Description = DiversityWorkbench.Functions.ColumnDescription("ExternalIdentifier", "Type");
                DiversityWorkbench.QueryCondition qIdentifierType = new DiversityWorkbench.QueryCondition(false, "Type", "External identifier", "Type", "Type of the identifier", Description, ID);
                QueryConditions.Add(qIdentifierType);

                Description = DiversityWorkbench.Functions.ColumnDescription("ExternalIdentifier", "Notes");
                DiversityWorkbench.QueryCondition qIdentifierNotes = new DiversityWorkbench.QueryCondition(false, "Notes", "External identifier", "Notes", "Notes about the identifier", Description, ID);
                QueryConditions.Add(qIdentifierNotes);

                #endregion

                #region Annotation

                System.Collections.Generic.Dictionary<string, DiversityWorkbench.AnnotationLink> AnnotationLinks = new Dictionary<string, AnnotationLink>();
                DiversityWorkbench.AnnotationLink Aevent = new AnnotationLink();
                Aevent.ReferencedTable = "CollectionEvent";
                Aevent.ReferencedColumn = "CollectionEventID";
                Aevent.LinkTable = "CollectionSpecimen";
                Aevent.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionEvent = DiversityWorkbench.Entity.EntityInformation("CollectionEvent");
                Aevent.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionEvent);
                AnnotationLinks.Add("CollectionEvent", Aevent);

                DiversityWorkbench.AnnotationLink Aspecimen = new AnnotationLink();
                Aspecimen.ReferencedTable = "CollectionSpecimen";
                Aspecimen.ReferencedColumn = "CollectionSpecimenID";
                Aspecimen.LinkTable = "CollectionSpecimen";
                Aspecimen.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionSpecimen = DiversityWorkbench.Entity.EntityInformation("CollectionSpecimen");
                Aspecimen.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionSpecimen);
                AnnotationLinks.Add("CollectionSpecimen", Aspecimen);

                DiversityWorkbench.AnnotationLink Aspecimenpart = new AnnotationLink();
                Aspecimenpart.ReferencedTable = "CollectionSpecimenPart";
                Aspecimenpart.ReferencedColumn = "SpecimenPartID";
                Aspecimenpart.LinkTable = "CollectionSpecimenPart";
                Aspecimenpart.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionSpecimenPart = DiversityWorkbench.Entity.EntityInformation("CollectionSpecimenPart");
                Aspecimenpart.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionSpecimenPart);
                AnnotationLinks.Add("CollectionSpecimenPart", Aspecimenpart);

                DiversityWorkbench.AnnotationLink Aunit = new AnnotationLink();
                Aunit.ReferencedTable = "IdentificationUnit";
                Aunit.ReferencedColumn = "IdentificationUnitID";
                Aunit.LinkTable = "IdentificationUnit";
                Aunit.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictIdentificationUnit = DiversityWorkbench.Entity.EntityInformation("IdentificationUnit");
                Aunit.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictIdentificationUnit);
                AnnotationLinks.Add("IdentificationUnit", Aunit);

                DiversityWorkbench.Annotation A = new Annotation(AnnotationLinks);

                /*
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.ReferencingTableLink> AnnotationLinks = new Dictionary<string, ReferencingTableLink>();
                DiversityWorkbench.ReferencingTableLink Aevent = new ReferencingTableLink();
                Aevent.ReferencedTable = "CollectionEvent";
                Aevent.ReferencedColumn = "CollectionEventID";
                Aevent.LinkTable = "CollectionSpecimen";
                Aevent.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionEvent = DiversityWorkbench.Entity.EntityInformation("CollectionEvent");
                Aevent.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionEvent);
                AnnotationLinks.Add("CollectionEvent", Aevent);

                DiversityWorkbench.ReferencingTableLink Aspecimen = new ReferencingTableLink();
                Aspecimen.ReferencedTable = "CollectionSpecimen";
                Aspecimen.ReferencedColumn = "CollectionSpecimenID";
                Aspecimen.LinkTable = "CollectionSpecimen";
                Aspecimen.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionSpecimen = DiversityWorkbench.Entity.EntityInformation("CollectionSpecimen");
                Aspecimen.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionSpecimen);
                AnnotationLinks.Add("CollectionSpecimen", Aspecimen);

                DiversityWorkbench.ReferencingTableLink Aspecimenpart = new ReferencingTableLink();
                Aspecimenpart.ReferencedTable = "CollectionSpecimenPart";
                Aspecimenpart.ReferencedColumn = "SpecimenPartID";
                Aspecimenpart.LinkTable = "CollectionSpecimenPart";
                Aspecimenpart.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictCollectionSpecimenPart = DiversityWorkbench.Entity.EntityInformation("CollectionSpecimenPart");
                Aspecimenpart.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictCollectionSpecimenPart);
                AnnotationLinks.Add("CollectionSpecimenPart", Aspecimenpart);

                DiversityWorkbench.ReferencingTableLink Aunit = new ReferencingTableLink();
                Aunit.ReferencedTable = "IdentificationUnit";
                Aunit.ReferencedColumn = "IdentificationUnitID";
                Aunit.LinkTable = "IdentificationUnit";
                Aunit.LinkedColumn = "CollectionSpecimenID";
                System.Collections.Generic.Dictionary<string, string> EntityDictIdentificationUnit = DiversityWorkbench.Entity.EntityInformation("IdentificationUnit");
                Aunit.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityDictIdentificationUnit);
                AnnotationLinks.Add("IdentificationUnit", Aunit);

                DiversityWorkbench.ReferencingTable A = new ReferencingTable("Annotation", AnnotationLinks);
                */

                //Description = "If any annotation is present";
                //DiversityWorkbench.QueryCondition qAnnotation = new DiversityWorkbench.QueryCondition(false, "Annotation", "Annotation", "Any Annot.", "If any annotation is available", Description, A);
                //QueryConditions.Add(qAnnotation);

                Description = DiversityWorkbench.Functions.ColumnDescription("Annotation", "Annotation");
                DiversityWorkbench.QueryCondition qAnnotation = new DiversityWorkbench.QueryCondition(false, "Annotation", "Annotation", "Annotation", "Annotation", Description, A);
                QueryConditions.Add(qAnnotation);

                Description = DiversityWorkbench.Functions.ColumnDescription("Annotation", "Title");
                DiversityWorkbench.QueryCondition qAnnotationTitle = new DiversityWorkbench.QueryCondition(false, "Title", "Annotation", "Title", "Title or topic of the annotation", Description, A);
                QueryConditions.Add(qAnnotationTitle);

                Description = DiversityWorkbench.Functions.ColumnDescription("Annotation", "ReferenceDisplayText");
                DiversityWorkbench.QueryCondition qAnnotationReference = new DiversityWorkbench.QueryCondition(false, "ReferenceDisplayText", "Annotation", "Reference", "The title of the reference", Description, A);
                qAnnotationReference.QueryType = QueryCondition.QueryTypes.AnnotationReference;
                QueryConditions.Add(qAnnotationReference);

                Description = DiversityWorkbench.Functions.ColumnDescription("Annotation", "SourceDisplayText");
                DiversityWorkbench.QueryCondition qAnnotationSource = new DiversityWorkbench.QueryCondition(false, "SourceDisplayText", "Annotation", "Source", "The name of the source of the annotation", Description, A);
                QueryConditions.Add(qAnnotationSource);

                Description = DiversityWorkbench.Functions.ColumnDescription("Annotation", "LogCreatedBy");
                DiversityWorkbench.QueryCondition qAnnotationResponsible = new DiversityWorkbench.QueryCondition(false, "LogCreatedBy", "Annotation", "Responsible", "Who created the annotation", Description, A);
                QueryConditions.Add(qAnnotationResponsible);

                #endregion

                #region Any text fields

                //Identifier

                System.Collections.Generic.List<DiversityWorkbench.QueryField> FreeIdentifierFields = new List<QueryField>();

                // Specimen: AccessionNumber,  DepositorsAccessionNumber, ExternalIdentifier
                DiversityWorkbench.QueryField SpecimenAccessionNumber = new QueryField("CollectionSpecimen_Core2", "AccessionNumber", "CollectionSpecimenID");
                FreeIdentifierFields.Add(SpecimenAccessionNumber);
                DiversityWorkbench.QueryField SpecimenDepositorsAccessionNumber = new QueryField("CollectionSpecimen_Core2", "DepositorsAccessionNumber", "CollectionSpecimenID");
                FreeIdentifierFields.Add(SpecimenDepositorsAccessionNumber);
                DiversityWorkbench.QueryField SpecimenExternalIdentifier = new QueryField("CollectionSpecimen_Core2", "ExternalIdentifier", "CollectionSpecimenID");
                FreeIdentifierFields.Add(SpecimenExternalIdentifier);

                // Agent:  Notes, DataWithholdingReason, CollectorsNumber, CollectorsName
                DiversityWorkbench.QueryField AgentCollectorsNumber = new QueryField("CollectionAgent", "CollectorsNumber", "CollectionSpecimenID");
                FreeIdentifierFields.Add(AgentCollectorsNumber);

                // CollectionSpecimenPart: StorageLocation, AccessionNumber, PartSublabel
                DiversityWorkbench.QueryField PartStorageLocation = new QueryField("CollectionSpecimenPart", "StorageLocation", "CollectionSpecimenID");
                FreeIdentifierFields.Add(PartStorageLocation);
                DiversityWorkbench.QueryField PartANr = new QueryField("CollectionSpecimenPart", "AccessionNumber", "CollectionSpecimenID");
                FreeIdentifierFields.Add(PartANr);
                DiversityWorkbench.QueryField PartPartSublabel = new QueryField("CollectionSpecimenPart", "PartSublabel", "CollectionSpecimenID");
                FreeIdentifierFields.Add(PartPartSublabel);

                // CollectionSpecimenTransaction:    AccessionNumber
                DiversityWorkbench.QueryField TransactionAccessionNumber = new QueryField("CollectionSpecimenTransaction", "AccessionNumber", "CollectionSpecimenID");
                FreeIdentifierFields.Add(TransactionAccessionNumber);

                // CollectionSpecimenRelation:     RelatedSpecimenURI, RelatedSpecimenDisplayText
                DiversityWorkbench.QueryField RelationSpecimenURI = new QueryField("CollectionSpecimenRelation", "RelatedSpecimenURI", "CollectionSpecimenID");
                FreeIdentifierFields.Add(RelationSpecimenURI);
                DiversityWorkbench.QueryField RelatedSpecimenDisplayText = new QueryField("CollectionSpecimenRelation", "RelatedSpecimenDisplayText", "CollectionSpecimenID");
                FreeIdentifierFields.Add(RelatedSpecimenDisplayText);

                // Identification:      VernacularTerm, TaxonomicName
                DiversityWorkbench.QueryField IdentificationVernacularTerm = new QueryField("Identification", "VernacularTerm", "CollectionSpecimenID");
                FreeIdentifierFields.Add(IdentificationVernacularTerm);
                DiversityWorkbench.QueryField IdentificationTaxonomicName = new QueryField("Identification", "TaxonomicName", "CollectionSpecimenID");
                FreeIdentifierFields.Add(IdentificationTaxonomicName);

                // IdentificationUnit:    ExsiccataNumber, UnitIdentifier
                DiversityWorkbench.QueryField UnitExsiccataNumber = new QueryField("IdentificationUnit", "ExsiccataNumber", "CollectionSpecimenID");
                FreeIdentifierFields.Add(UnitExsiccataNumber);
                DiversityWorkbench.QueryField UnitIdentifier = new QueryField("IdentificationUnit", "UnitIdentifier", "CollectionSpecimenID");
                FreeIdentifierFields.Add(UnitIdentifier);

                // IdentificationUnitAnalysis:     AnalysisNumber
                DiversityWorkbench.QueryField AnalysisNumber = new QueryField("IdentificationUnitAnalysis", "AnalysisNumber", "CollectionSpecimenID");
                FreeIdentifierFields.Add(AnalysisNumber);

                Description = "Search for an expression in any identifier field like e.g. accession number (not collection event)";
                DiversityWorkbench.QueryCondition qFreeIdentifier = new DiversityWorkbench.QueryCondition(false, FreeIdentifierFields, "Any fields", "Identfier", "Any identifier field", Description, true);
                qFreeIdentifier.ForMultiFieldQuery = true;
                QueryConditions.Add(qFreeIdentifier);


                //DataWithholding

                System.Collections.Generic.List<DiversityWorkbench.QueryField> FreeDataWithholdingFields = new List<QueryField>();
                // Agent
                DiversityWorkbench.QueryField AgentDataWithholdingReason = new QueryField("CollectionAgent", "DataWithholdingReason", "CollectionSpecimenID");
                FreeDataWithholdingFields.Add(AgentDataWithholdingReason);

                // Specimen: DataWithholdingReason, InternalNotes, AccessionNumber,  DepositorsAccessionNumber, ReferenceTitle, ExternalIdentifier
                DiversityWorkbench.QueryField SpecimenDataWithholdingReason = new QueryField("CollectionSpecimen_Core2", "DataWithholdingReason", "CollectionSpecimenID");
                FreeDataWithholdingFields.Add(SpecimenDataWithholdingReason);

                // CollectionSpecimenPart: DataWithholdingReason
                DiversityWorkbench.QueryField PartDataWithholdingReason = new QueryField("CollectionSpecimenPart", "DataWithholdingReason", "CollectionSpecimenID");
                FreeDataWithholdingFields.Add(PartDataWithholdingReason);

                // CollectionSpecimenImage: DataWithholdingReason
                DiversityWorkbench.QueryField ImageDataWithholdingReason = new QueryField("CollectionSpecimenImage", "DataWithholdingReason", "CollectionSpecimenID");
                FreeDataWithholdingFields.Add(ImageDataWithholdingReason);

                Description = "Search for an expression in data withholding field (not collection event)";
                DiversityWorkbench.QueryCondition qFreeWithholding = new DiversityWorkbench.QueryCondition(false, FreeDataWithholdingFields, "Any fields", "Withhold", "Any data withholding field", Description, true);
                qFreeWithholding.ForMultiFieldQuery = true;
                QueryConditions.Add(qFreeWithholding);



                // Notes
                System.Collections.Generic.List<DiversityWorkbench.QueryField> FreeNotesFields = new List<QueryField>();

                // Agent:  Notes, DataWithholdingReason, CollectorsNumber, CollectorsName
                DiversityWorkbench.QueryField AgentNotes = new QueryField("CollectionAgent", "Notes", "CollectionSpecimenID");
                FreeNotesFields.Add(AgentNotes);

                // Specimen: LabelTitle, LabelTranscriptionNotes, OriginalNotes, AdditionalNotes, Problems, DataWithholdingReason, InternalNotes, AccessionNumber,  DepositorsAccessionNumber, ReferenceTitle, ExternalIdentifier
                DiversityWorkbench.QueryField SpecimenLabelTitle = new QueryField("CollectionSpecimen_Core2", "LabelTitle", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenLabelTitle);
                DiversityWorkbench.QueryField SpecimenLabelTranscriptionNotes = new QueryField("CollectionSpecimen_Core2", "LabelTranscriptionNotes", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenLabelTranscriptionNotes);
                DiversityWorkbench.QueryField SpecimenOriNotes = new QueryField("CollectionSpecimen_Core2", "OriginalNotes", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenOriNotes);
                DiversityWorkbench.QueryField SpecimenAdditionalNotes = new QueryField("CollectionSpecimen_Core2", "AdditionalNotes", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenAdditionalNotes);
                DiversityWorkbench.QueryField SpecimenProblems = new QueryField("CollectionSpecimen_Core2", "Problems", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenProblems);
                DiversityWorkbench.QueryField SpecimenInternalNotes = new QueryField("CollectionSpecimen_Core2", "InternalNotes", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenInternalNotes);

                // CollectionSpecimenImage: Notes, DataWithholdingReason, Title, CopyrightStatement, InternalNotes
                DiversityWorkbench.QueryField SpecimenImageNotes = new QueryField("CollectionSpecimenImage", "Notes", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenImageNotes);
                DiversityWorkbench.QueryField SpecimenImageInternalNotes = new QueryField("CollectionSpecimenImage", "InternalNotes", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenImageInternalNotes);
                DiversityWorkbench.QueryField SpecimenImageTitle = new QueryField("CollectionSpecimenImage", "Title", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenImageTitle);
                DiversityWorkbench.QueryField SpecimenImageCopyrightStatement = new QueryField("CollectionSpecimenImage", "CopyrightStatement", "CollectionSpecimenID");
                FreeNotesFields.Add(SpecimenImageCopyrightStatement);

                // CollectionSpecimenPart: PreparationMethod, Notes
                DiversityWorkbench.QueryField PartNotes = new QueryField("CollectionSpecimenPart", "Notes", "CollectionSpecimenID");
                FreeNotesFields.Add(PartNotes);
                DiversityWorkbench.QueryField PartPreparationMethod = new QueryField("CollectionSpecimenPart", "PreparationMethod", "CollectionSpecimenID");
                FreeNotesFields.Add(PartPreparationMethod);

                // CollectionSpecimenRelation:     RelatedSpecimenDescription, Notes
                DiversityWorkbench.QueryField RelatedSpecimenDescription = new QueryField("CollectionSpecimenRelation", "RelatedSpecimenDescription", "CollectionSpecimenID");
                FreeNotesFields.Add(RelatedSpecimenDescription);
                DiversityWorkbench.QueryField RelatedSpecimenNotes = new QueryField("CollectionSpecimenRelation", "Notes", "CollectionSpecimenID");
                FreeNotesFields.Add(RelatedSpecimenNotes);

                // Identification:      VernacularTerm, TaxonomicName, TypeNotes, ReferenceTitle, Notes
                DiversityWorkbench.QueryField IdentificationNotes = new QueryField("Identification", "Notes", "CollectionSpecimenID");
                FreeNotesFields.Add(IdentificationNotes);
                DiversityWorkbench.QueryField IdentificationTypeNotes = new QueryField("Identification", "TypeNotes", "CollectionSpecimenID");
                FreeNotesFields.Add(IdentificationTypeNotes);

                // IdentificationUnit:    ExsiccataNumber, UnitIdentifier, UnitDescription, Circumstances, Notes
                DiversityWorkbench.QueryField UnitDescription = new QueryField("IdentificationUnit", "UnitDescription", "CollectionSpecimenID");
                FreeNotesFields.Add(UnitDescription);
                DiversityWorkbench.QueryField UnitCircumstances = new QueryField("IdentificationUnit", "Circumstances", "CollectionSpecimenID");
                FreeNotesFields.Add(UnitCircumstances);
                DiversityWorkbench.QueryField UnitNotes = new QueryField("IdentificationUnit", "Notes", "CollectionSpecimenID");
                FreeNotesFields.Add(UnitNotes);

                // IdentificationUnitAnalysis:     AnalysisNumber, Notes
                DiversityWorkbench.QueryField UnitAnalysisNotes = new QueryField("IdentificationUnitAnalysis", "Notes", "CollectionSpecimenID");
                FreeNotesFields.Add(UnitAnalysisNotes);

                // IdentificationUnitGeoAnalysis:     Notes
                DiversityWorkbench.QueryField UnitGeoAnalysisNotes = new QueryField("IdentificationUnitGeoAnalysis", "Notes", "CollectionSpecimenID");
                FreeNotesFields.Add(UnitGeoAnalysisNotes);

                // IdentificationUnitInPart:     Description
                DiversityWorkbench.QueryField UnitInPartDescription = new QueryField("IdentificationUnitInPart", "Description", "CollectionSpecimenID");
                FreeNotesFields.Add(UnitInPartDescription);

                Description = "Search for an expression in any notes, desprition or the like field (not collection event)";
                DiversityWorkbench.QueryCondition qFreeNotes = new DiversityWorkbench.QueryCondition(false, FreeNotesFields, "Any fields", "Notes", "Any notes field", Description, true);
                qFreeNotes.ForMultiFieldQuery = true;
                QueryConditions.Add(qFreeNotes);

                if (this._ServerConnection.LinkedServer.Length == 0)
                {
                    DiversityWorkbench.QueryCondition qManyColumnQueryIdentifier = new DiversityWorkbench.QueryCondition(false, "Any fields", "Any identifier", "Any identifier field");
                    qManyColumnQueryIdentifier.SourceIsFunction = true;
                    qManyColumnQueryIdentifier.SqlFromClause = " FROM dbo.MultiColumnQuery('Identifier', '";
                    qManyColumnQueryIdentifier.SqlFromClausePostfix = ")";
                    qManyColumnQueryIdentifier.IdentityColumn = "CollectionSpecimenID";
                    qManyColumnQueryIdentifier.Table = "AnyIdentifierField";
                    QueryConditions.Add(qManyColumnQueryIdentifier);

                    DiversityWorkbench.QueryCondition qManyColumnQueryDataWithholding = new DiversityWorkbench.QueryCondition(false, "Any fields", "Any withhold", "Any data withholding field");
                    qManyColumnQueryDataWithholding.SourceIsFunction = true;
                    qManyColumnQueryDataWithholding.SqlFromClause = " FROM dbo.MultiColumnQuery('Withhold', '";
                    qManyColumnQueryDataWithholding.SqlFromClausePostfix = ")";
                    qManyColumnQueryDataWithholding.IdentityColumn = "CollectionSpecimenID";
                    qManyColumnQueryDataWithholding.Table = "AnyWithholdingField";
                    QueryConditions.Add(qManyColumnQueryDataWithholding);

                    DiversityWorkbench.QueryCondition qManyColumnQueryNotes = new DiversityWorkbench.QueryCondition(false, "Any fields", "Any notes", "Any notes field");
                    qManyColumnQueryNotes.SourceIsFunction = true;
                    qManyColumnQueryNotes.SqlFromClause = " FROM dbo.MultiColumnQuery('Notes', '";
                    qManyColumnQueryNotes.SqlFromClausePostfix = ")";
                    qManyColumnQueryNotes.IdentityColumn = "CollectionSpecimenID";
                    qManyColumnQueryNotes.Table = "AnyNotesField";
                    QueryConditions.Add(qManyColumnQueryNotes);
                }

                #endregion

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return QueryConditions;
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> PredefinedQueryPersistentConditionList = new List<QueryCondition>();
            //TODO: _QueryConditionProject ist leer - unklar warum - wenn gelöst kann folgendes if(..){..} entfallen
            //if (this._QueryConditionProject == null)
            //{
            //    System.Data.DataTable dtProject = new System.Data.DataTable();
            //    string SQL = "SELECT ProjectID AS [Value], Project AS Display " +
            //        "FROM ProjectList " +
            //        "ORDER BY Display";
            //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            //    if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            //    {
            //        try { a.Fill(dtProject); }
            //        catch { }
            //    }
            //    if (dtProject.Rows.Count > 1)
            //    {
            //        dtProject.Clear();
            //        SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ProjectID AS [Value], Project AS Display " +
            //            "FROM ProjectList " +
            //            "ORDER BY Display";
            //        a.SelectCommand.CommandText = SQL;
            //        try { a.Fill(dtProject); }
            //        catch { }
            //    }
            //    if (dtProject.Columns.Count == 0)
            //    {
            //        System.Data.DataColumn Value = new System.Data.DataColumn("Value");
            //        System.Data.DataColumn Display = new System.Data.DataColumn("Display");
            //        dtProject.Columns.Add(Value);
            //        dtProject.Columns.Add(Display);
            //    }
            //    string Description = DiversityWorkbench.Functions.ColumnDescription("ProjectProxy", "Project");
            //    this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "CollectionProject", "CollectionSpecimenID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
            //    //QueryConditions.Add(this._QueryConditionProject);
            //}
            //if (this._QueryConditionProject.Column == null)
            //{
            //    System.Data.DataTable dtProject = new System.Data.DataTable();
            //    string SQL = "SELECT ProjectID AS [Value], Project AS Display " +
            //        "FROM ProjectList " +
            //        "ORDER BY Display";
            //    Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ServerConnection.ConnectionString);
            //    if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
            //    {
            //        try { a.Fill(dtProject); }
            //        catch { }
            //    }
            //    if (dtProject.Rows.Count > 1)
            //    {
            //        dtProject.Clear();
            //        SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ProjectID AS [Value], Project AS Display " +
            //            "FROM ProjectList " +
            //            "ORDER BY Display";
            //        a.SelectCommand.CommandText = SQL;
            //        try { a.Fill(dtProject); }
            //        catch { }
            //    }
            //    if (dtProject.Columns.Count == 0)
            //    {
            //        System.Data.DataColumn Value = new System.Data.DataColumn("Value");
            //        System.Data.DataColumn Display = new System.Data.DataColumn("Display");
            //        dtProject.Columns.Add(Value);
            //        dtProject.Columns.Add(Display);
            //    }
            //    string Description = DiversityWorkbench.Functions.ColumnDescription("ProjectProxy", "Project");
            //    this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "CollectionProject", "CollectionSpecimenID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
            //}
            PredefinedQueryPersistentConditionList.Add(this.QueryConditionProject);
            return PredefinedQueryPersistentConditionList;
        }

        #endregion

        #region Properties

        private static System.Collections.Generic.List<string> _TaxonomyRelatedTaxonomicGroups;

        /// <summary>
        /// A list of those taxonomic groups where the identifications are related to the module DiversityTaxonNames. Any other group is linked to DiversityScientificTerms
        /// </summary>
        public static System.Collections.Generic.List<string> TaxonomyRelatedTaxonomicGroups
        {
            get
            {
                if (_TaxonomyRelatedTaxonomicGroups == null)
                {
                    string SQL = "declare @Taxa table (Code nvarchar(50), ParentCode nvarchar(50)) " +
                        "insert into @Taxa " +
                        "SELECT DISTINCT [Code], ParentCode " +
                        "FROM [dbo].[CollTaxonomicGroup_Enum] E " +
                        "where E.ParentCode in ('animal', 'alga', 'bacterium', 'bryophyte', 'fungus', 'gall', 'lichen', 'myxomycete', 'plant', 'virus', 'organism') " + // #115
                        "or E.Code in  ('animal', 'alga', 'bacterium', 'bryophyte', 'fungus', 'gall', 'lichen', 'myxomycete', 'plant', 'virus', 'organism') " + // #115
                        "while (SELECT count(*) " +
                        "FROM [dbo].[CollTaxonomicGroup_Enum] E, @Taxa T " +
                        "where T.Code = E.ParentCode " +
                        "and E.Code not in (Select Code from @Taxa)) > 0 " +
                        "begin " +
                        "insert into @Taxa " +
                        "SELECT DISTINCT E.[Code], E.ParentCode " +
                        "FROM [dbo].[CollTaxonomicGroup_Enum] E, @Taxa T " +
                        "where T.Code = E.ParentCode " +
                        "and E.Code not in (Select Code from @Taxa) " +
                        "end " +
                        "select Code from @Taxa";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(dt);
                        _TaxonomyRelatedTaxonomicGroups = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            _TaxonomyRelatedTaxonomicGroups.Add(R[0].ToString());
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return _TaxonomyRelatedTaxonomicGroups;
            }
            set { _TaxonomyRelatedTaxonomicGroups = value; }
        }

        private static System.Collections.Generic.List<string> _TermRelatedTaxonomicGroups;

        /// <summary>
        /// A list of those taxonomic groups where the identifications are related to the module DiversityTaxonNames. Any other group is linked to DiversityScientificTerms
        /// </summary>
        public static System.Collections.Generic.List<string> TermRelatedTaxonomicGroups
        {
            get
            {
                if (_TermRelatedTaxonomicGroups == null)
                {
                    string SQL = "SELECT DISTINCT [Code] " +
                        "FROM [dbo].[CollTaxonomicGroup_Enum] E " +
                        "ORDER BY Code";
                    System.Data.DataTable dt = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    try
                    {
                        ad.Fill(dt);
                        _TermRelatedTaxonomicGroups = new List<string>();
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!TaxonomyRelatedTaxonomicGroups.Contains(R[0].ToString()))
                                _TermRelatedTaxonomicGroups.Add(R[0].ToString());
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return _TermRelatedTaxonomicGroups;
            }
            set { _TermRelatedTaxonomicGroups = value; }
        }
        //private string Message(string Resource)
        //{
        //    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollectionSpecimenText));
        //    string Message = resources.GetString(Resource);
        //    return Message;
        //}

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
                    this._BackLinkImages.Images.Add("EventSeriesImage", DiversityWorkbench.Properties.Resources.EventSeriesImages); // 2
                    this._BackLinkImages.Images.Add("EventImage", DiversityWorkbench.Properties.Resources.EventImage);       // 3
                    this._BackLinkImages.Images.Add("EventLocalisation", DiversityWorkbench.Properties.Resources.Event);// 4
                    this._BackLinkImages.Images.Add("EventProperty", DiversityWorkbench.Properties.Resources.Habitat);    // 5
                    this._BackLinkImages.Images.Add("Collection", DiversityWorkbench.Properties.Resources.Collection);       // 6
                    this._BackLinkImages.Images.Add("CollectionImage", DiversityWorkbench.Properties.Resources.CollectionImage1);  // 7
                    this._BackLinkImages.Images.Add("Transaction", DiversityWorkbench.Properties.Resources.Transaction);      // 8
                    this._BackLinkImages.Images.Add("TransactionAgent", DiversityWorkbench.Properties.Resources.TransactionAgent); // 9
                    this._BackLinkImages.Images.Add("TransactionPayment", DiversityWorkbench.Properties.Resources.Payment); // 10
                    this._BackLinkImages.Images.Add("Identification", DiversityWorkbench.Properties.Resources.Identification); // 11
                    this._BackLinkImages.Images.Add("Part", DiversityWorkbench.Properties.Resources.Specimen);          // 12
                    this._BackLinkImages.Images.Add("Depositor", DiversityWorkbench.Properties.Resources.CollectionSpecimen); // 13
                    this._BackLinkImages.Images.Add("SpecimenImage", DiversityWorkbench.Properties.Resources.SpecimenImages); // 14
                    this._BackLinkImages.Images.Add("Collector", DiversityWorkbench.Properties.Resources.Agent); // 15
                    this._BackLinkImages.Images.Add("Geoanalysis", DiversityWorkbench.Properties.Resources.GeoAnalysis);   // 16
                    this._BackLinkImages.Images.Add("Analysis", DiversityWorkbench.Properties.Resources.Analysis);// 17
                    this._BackLinkImages.Images.Add("Processing", DiversityWorkbench.Properties.Resources.Processing);    // 18
                    this._BackLinkImages.Images.Add("Relation", DiversityWorkbench.Properties.Resources.Relation);    // 19
                    this._BackLinkImages.Images.Add("Reference", DiversityWorkbench.Properties.Resources.References);    // 20
                    break;
                case ModuleType.Gazetteer:
                    this._BackLinkImages.Images.Add("EventLocalisation", DiversityWorkbench.Properties.Resources.Event);
                    break;
                case ModuleType.Projects:
                    this._BackLinkImages.Images.Add("Project", DiversityWorkbench.Properties.Resources.Project);
                    break;
                case ModuleType.References:
                    this._BackLinkImages.Images.Add("Event", DiversityWorkbench.Properties.Resources.Event);
                    this._BackLinkImages.Images.Add("Specimen", DiversityWorkbench.Properties.Resources.Specimen);
                    break;
                case ModuleType.SamplingPlots:
                    this._BackLinkImages.Images.Add("EventLocalisation", DiversityWorkbench.Properties.Resources.Event);
                    break;
                case ModuleType.ScientificTerms:
                    this._BackLinkImages.Images.Add("Event", DiversityWorkbench.Properties.Resources.Event);
                    this._BackLinkImages.Images.Add("Identification", DiversityWorkbench.Properties.Resources.Identification);
                    this._BackLinkImages.Images.Add("Part", DiversityWorkbench.Properties.Resources.Specimen);
                    break;
                case ModuleType.TaxonNames:
                    this._BackLinkImages.Images.Add("Identification", DiversityWorkbench.Properties.Resources.Identification);
                    break;
            }
            return this._BackLinkImages;
        }

        protected override string BacklinkUpdateRestrictionView() { return "CollectionSpecimenID_CanEdit"; }

        protected override string BacklinkUpdateRestriction(string TableAlias, string RestrictionView)
        {
            string IdentityColumn = "";
            if (this.QueryDisplayColumns()[0].IdentityColumn != null)
                IdentityColumn = this.QueryDisplayColumns()[0].IdentityColumn;
            string SQL = " INNER JOIN " + RestrictionView + " AS RV ON RV." + IdentityColumn + " = " + TableAlias + "." + IdentityColumn;
            return SQL;
        }

        protected override string BacklinkUpdateRestriction(string TableAlias, string RestrictionView, string JoinTable, string ConnectionString)
        {
            string IdentityColumn = "";
            if (this.QueryDisplayColumns()[0].IdentityColumn != null)
                IdentityColumn = this.QueryDisplayColumns()[0].IdentityColumn;
            string SQL = "";
            // Check existence of column in table
            SQL = "select count(*) from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = '" + JoinTable + "' and c.COLUMN_NAME = '" + IdentityColumn + "'";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ConnectionString);
            if (Result == "1")
                SQL = "INNER JOIN " + RestrictionView + " AS RV ON RV." + IdentityColumn + " = " + TableAlias + "." + IdentityColumn;
            else
                SQL = "";
            return SQL;
        }


        public override System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BackLinkServerConnectionDomains(string URI, ModuleType CallingModule, bool IncludeEmpty = false, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.Dictionary<ServerConnection, System.Collections.Generic.List<BackLinkDomain>> BLD = new Dictionary<ServerConnection, List<BackLinkDomain>>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in this.BackLinkConnections(ModuleType.Collection))
            {
                switch (CallingModule)
                {
                    case ModuleType.Agents:
                        System.Collections.Generic.List<BackLinkDomain> _A = this.BackLinkDomainAgent(KV.Value, URI);
                        if (_A.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _A);
                        break;
                    case ModuleType.Gazetteer:
                        System.Collections.Generic.List<BackLinkDomain> _G = this.BackLinkDomainProject(KV.Value, URI);
                        if (_G.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _G);
                        break;
                    case ModuleType.Projects:
                        System.Collections.Generic.List<BackLinkDomain> _P = this.BackLinkDomainProject(KV.Value, URI);
                        if (_P.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _P);
                        break;
                    case ModuleType.References:
                        System.Collections.Generic.List<BackLinkDomain> _R = this.BackLinkDomainReferences(KV.Value, URI);
                        if (_R.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _R);
                        break;
                    case ModuleType.SamplingPlots:
                        System.Collections.Generic.List<BackLinkDomain> _SP = this.BackLinkDomainPlot(KV.Value, URI);
                        if (_SP.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _SP);
                        break;
                    case ModuleType.ScientificTerms:
                        System.Collections.Generic.List<BackLinkDomain> _ST = this.BackLinkDomainTerm(KV.Value, URI);
                        if (_ST.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _ST);
                        break;
                    case ModuleType.TaxonNames:
                        System.Collections.Generic.List<BackLinkDomain> _T = this.BackLinkDomainTaxa(KV.Value, URI, Restrictions);
                        if (_T.Count > 0 || IncludeEmpty)
                            BLD.Add(KV.Value, _T);
                        break;
                }
            }
            return BLD;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainAgent(ServerConnection SC, string URI)
        {
            /*
             * "Eventseries image creator"
             * "Eventseries image license holder"
             * "Event image creator"
             * "Event image license holder"
             * "Event localisation responsible"
             * "Event site property responsible"
             * "Collection administrator"
             * "Collection image creator"
             * "Collection image license holder"
             * "Transaction responsible"
             * "Transaction sender"
             * "Transaction receiver"
             * "Transaction agent"
             * "Transaction payment payer"
             * "Transaction payment receiver"
             * "Depositor"
             * "Collector"
             * "Specimen image creator"
             * "Specimen image license holder"
             * "Specimen reference responsible"
             * "Identification responsible"
             * "Part responsible"
             * "Processing responsible"
             * "Analysis responsible"
             * "Geoanalysis responsible"
             * */
            System.Collections.Generic.List<BackLinkDomain> Links = new List<BackLinkDomain>();


            // Series image
            DiversityWorkbench.BackLinkDomain ESIC = this.BackLinkDomain(SC, URI, "Collection event series image creator", "CollectionEventSeriesImage", "CreatorAgentURI", 2);
            ESIC.AddBacklinkColumn("CollectionEventSeriesImage", "CreatorAgent", "Displayed Name");
            if (ESIC.DtItems.Rows.Count > 0)
                Links.Add(ESIC);

            DiversityWorkbench.BackLinkDomain ESIL = this.BackLinkDomain(SC, URI, "Collection event series image license holder", "CollectionEventSeriesImage", "LicenseHolderAgentURI", 2);
            ESIL.AddBacklinkColumn("CollectionEventSeriesImage", "LicenseHolder", "Displayed Name");
            if (ESIL.DtItems.Rows.Count > 0)
                Links.Add(ESIL);

            // Event image
            DiversityWorkbench.BackLinkDomain EIC = this.BackLinkDomain(SC, URI, "Collection event image creator", "CollectionEventImage", "CreatorAgentURI", 3);
            EIC.AddBacklinkColumn("CollectionEventImage", "CreatorAgent", "Displayed Name");
            if (EIC.DtItems.Rows.Count > 0)
                Links.Add(EIC);
            DiversityWorkbench.BackLinkDomain EIL = this.BackLinkDomain(SC, URI, "Collection event image license holder", "CollectionEventImage", "LicenseHolderAgentURI", 3);
            EIL.AddBacklinkColumn("CollectionEventImage", "LicenseHolder", "Displayed Name");
            if (EIL.DtItems.Rows.Count > 0)
                Links.Add(EIL);

            // Localisation responsible
            DiversityWorkbench.BackLinkDomain EL = this.BackLinkDomain(SC, URI, "Collection event localisation responsible", "CollectionEventLocalisation", "ResponsibleAgentURI", 4);
            EL.AddBacklinkColumn("CollectionEventLocalisation", "ResponsibleName", "Displayed Name");
            if (EL.DtItems.Rows.Count > 0)
                Links.Add(EL);

            // Collection site property responsible
            DiversityWorkbench.BackLinkDomain EP = this.BackLinkDomain(SC, URI, "Collection event site property responsible", "CollectionEventProperty", "ResponsibleAgentURI", 5);
            EP.AddBacklinkColumn("CollectionEventProperty", "ResponsibleName", "Displayed Name");
            if (EP.DtItems.Rows.Count > 0)
                Links.Add(EP);

            // Collection administrator
            DiversityWorkbench.BackLinkDomain C = this.BackLinkDomain(SC, URI, "Collection administrator", "Collection", "AdministrativeContactAgentURI", 6);
            C.AddBacklinkColumn("Collection", "AdministrativeContactName", "Displayed Name");
            if (C.DtItems.Rows.Count > 0)
                Links.Add(C);

            // Collection image
            DiversityWorkbench.BackLinkDomain CIC = this.BackLinkDomain(SC, URI, "Collection image creator", "CollectionImage", "CreatorAgentURI", 7);
            CIC.AddBacklinkColumn("CollectionImage", "CreatorAgent", "Displayed Name");
            if (CIC.DtItems.Rows.Count > 0)
                Links.Add(CIC);
            DiversityWorkbench.BackLinkDomain CIL = this.BackLinkDomain(SC, URI, "Collection image license holder", "CollectionImage", "LicenseHolderAgentURI", 7);
            CIL.AddBacklinkColumn("CollectionImage", "LicenseHolder", "Displayed Name");
            if (CIL.DtItems.Rows.Count > 0)
                Links.Add(CIL);

            // Transaction
            DiversityWorkbench.BackLinkDomain TR = this.BackLinkDomain(SC, URI, "Transaction responsible", "Transaction", "ResponsibleAgentURI", 8);
            TR.AddBacklinkColumn("Transaction", "ResponsibleName", "Displayed Name");
            if (TR.DtItems.Rows.Count > 0)
                Links.Add(TR);
            DiversityWorkbench.BackLinkDomain TS = this.BackLinkDomain(SC, URI, "Transaction sender", "Transaction", "FromTransactionPartnerAgentURI", 9);
            TS.AddBacklinkColumn("Transaction", "FromTransactionPartnerName", "Displayed Name");
            if (TS.DtItems.Rows.Count > 0)
                Links.Add(TS);
            DiversityWorkbench.BackLinkDomain TT = this.BackLinkDomain(SC, URI, "Transaction receiver", "Transaction", "ToTransactionPartnerAgentURI", 9);
            TT.AddBacklinkColumn("Transaction", "ToTransactionPartnerName", "Displayed Name");
            if (TT.DtItems.Rows.Count > 0)
                Links.Add(TT);
            DiversityWorkbench.BackLinkDomain TA = this.BackLinkDomain(SC, URI, "Transaction agent", "TransactionAgent", "AgentURI", 9);
            TA.AddBacklinkColumn("TransactionAgent", "AgentName", "Displayed Name");
            if (TA.DtItems.Rows.Count > 0)
                Links.Add(TA);
            DiversityWorkbench.BackLinkDomain TPP = this.BackLinkDomain(SC, URI, "Transaction responsible", "TransactionPayment", "PayerAgentURI", 10);
            TPP.AddBacklinkColumn("TransactionPayment", "PayerName", "Displayed Name");
            if (TPP.DtItems.Rows.Count > 0)
                Links.Add(TPP);
            DiversityWorkbench.BackLinkDomain TPR = this.BackLinkDomain(SC, URI, "Transaction responsible", "TransactionPayment", "RecipientAgentURI", 10);
            TPR.AddBacklinkColumn("TransactionPayment", "RecipientName", "Displayed Name");
            if (TPR.DtItems.Rows.Count > 0)
                Links.Add(TPR);

            //Depositor
            DiversityWorkbench.BackLinkDomain Depositor = this.BackLinkDomain(SC, URI, "Depositor", "CollectionSpecimen", "DepositorsAgentURI", 13);
            Depositor.AddBacklinkColumn("CollectionSpecimen", "DepositorsName", "Displayed Name");
            if (Depositor.DtItems.Rows.Count > 0)
                Links.Add(Depositor);

            //Collector
            DiversityWorkbench.BackLinkDomain Collector = this.BackLinkDomain(SC, URI, "Collector", "CollectionAgent", "CollectorsAgentURI", 15);
            Collector.AddBacklinkColumn("CollectionAgent", "CollectorsName", "Displayed Name");
            if (Collector.DtItems.Rows.Count > 0)
                Links.Add(Collector);

            // Collection specimen image
            DiversityWorkbench.BackLinkDomain CSIC = this.BackLinkDomain(SC, URI, "Collection specimen image creator", "CollectionSpecimenImage", "CreatorAgentURI", 14);
            CSIC.AddBacklinkColumn("CollectionSpecimenImage", "CreatorAgent", "Displayed Name");
            if (CSIC.DtItems.Rows.Count > 0)
                Links.Add(CSIC);
            DiversityWorkbench.BackLinkDomain CSIL = this.BackLinkDomain(SC, URI, "Collection specimen image license holder", "CollectionSpecimenImage", "LicenseHolderAgentURI", 14);
            CSIL.AddBacklinkColumn("CollectionSpecimenImage", "LicenseHolder", "Displayed Name");
            if (CSIL.DtItems.Rows.Count > 0)
                Links.Add(CSIL);

            // Specimen reference responsible
            DiversityWorkbench.BackLinkDomain Reference = this.BackLinkDomain(SC, URI, "Specimen reference responsible", "CollectionSpecimenReference", "ResponsibleAgentURI", 20);
            Reference.AddBacklinkColumn("CollectionSpecimenReference", "ResponsibleName", "Displayed Name");
            if (Reference.DtItems.Rows.Count > 0)
                Links.Add(Reference);

            // Identification
            DiversityWorkbench.BackLinkDomain Ident = this.BackLinkDomain(SC, URI, "Identifier", "Identification", "ResponsibleAgentURI", 11);
            Ident.AddBacklinkColumn("Identification", "ResponsibleName", "Displayed Name");
            if (Ident.DtItems.Rows.Count > 0)
                Links.Add(Ident);

            // Part
            DiversityWorkbench.BackLinkDomain Part = this.BackLinkDomain(SC, URI, "Part responsible", "CollectionSpecimenPart", "ResponsibleAgentURI", 12);
            Part.AddBacklinkColumn("CollectionSpecimenPart", "ResponsibleName", "Displayed Name");
            if (Part.DtItems.Rows.Count > 0)
                Links.Add(Part);

            // Processing responsible
            DiversityWorkbench.BackLinkDomain Processing = this.BackLinkDomain(SC, URI, "Processing responsible", "CollectionSpecimenProcessing", "ResponsibleAgentURI", 18);
            Processing.AddBacklinkColumn("CollectionSpecimenProcessing", "ResponsibleName", "Displayed Name");
            if (Processing.DtItems.Rows.Count > 0)
                Links.Add(Processing);

            // Analysis responsible
            DiversityWorkbench.BackLinkDomain Analysis = this.BackLinkDomain(SC, URI, "Analysis responsible", "IdentificationUnitAnalysis", "ResponsibleAgentURI", 17);
            Analysis.AddBacklinkColumn("IdentificationUnitAnalysis", "ResponsibleName", "Displayed Name");
            if (Analysis.DtItems.Rows.Count > 0)
                Links.Add(Analysis);

            // Geoanalysis responsible
            DiversityWorkbench.BackLinkDomain Geoanalysis = this.BackLinkDomain(SC, URI, "Geoanalysis responsible", "IdentificationUnitGeoAnalysis", "ResponsibleAgentURI", 16);
            Geoanalysis.AddBacklinkColumn("IdentificationUnitGeoAnalysis", "ResponsibleName", "Displayed Name");
            if (Geoanalysis.DtItems.Rows.Count > 0)
                Links.Add(Geoanalysis);

            return Links;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainGazetteer(ServerConnection SC, string URI, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.List<BackLinkDomain> Links = new List<BackLinkDomain>();

            // Localisation 
            DiversityWorkbench.BackLinkDomain EL = this.BackLinkDomain(SC, URI, "Collection event localisation", "CollectionEventLocalisation", "Location1", 4);
            EL.AddBacklinkColumn("CollectionEvent", "CountryCache", "Country");
            EL.AddBacklinkColumn("CollectionEventLocalisation", "OrderCache", "Order");
            EL.AddBacklinkColumn("CollectionEventLocalisation", "HierarchyCache", "Hierarchy");
            if (EL.DtItems.Rows.Count > 0)
                Links.Add(EL);

            return Links;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainProject(ServerConnection SC, string URI, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.List<BackLinkDomain> Links = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain("Project", "ProjectProxy", "ProjectURI", 2);
            string Prefix = "dbo.";
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "]." + Prefix;
            string SQL = "SELECT 'First of ' + CAST(COUNT(*) as varchar) + ' specimen' AS DisplayText, " +
                "MIN(S.CollectionSpecimenID) AS ID ";
            if (SC.LinkedServer.Length > 0)
                SQL += "FROM " + Prefix + "ProjectList AS T ";
            else
                SQL += "FROM " + Prefix + "ProjectProxy AS T ";
            SQL += "INNER JOIN " + Prefix + "CollectionProject AS S ON T.ProjectID = S.ProjectID " +
                "INNER JOIN " + Prefix + "CollectionSpecimenID_Available AS A ON S.CollectionSpecimenID = A.CollectionSpecimenID " +
                "WHERE(T.ProjectURI = '" + URI + "') " +
                "GROUP BY T.Project, T.ProjectURI ";
            // Check existence of column ProjectURI
            bool ProjectURIexists = false;
            string sqlCheck = "SELECT TOP 1 ProjectURI FROM " + Prefix + "CollectionProject";
            string Message = "";
            string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(sqlCheck, SC.ConnectionString, ref Message);
            if (Message.Length == 0)
                ProjectURIexists = true;
            if (ProjectURIexists)
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
                try
                {
                    ad.Fill(BackLink.DtItems);
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
                }
            }

            if (BackLink.DtItems.Rows.Count > 0)
                Links.Add(BackLink);
            return Links;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainReferences(ServerConnection SC, string URI)
        {
            System.Collections.Generic.List<BackLinkDomain> Terms = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain Event = this.BackLinkDomain(SC, URI, "Event", "CollectionEvent", "ReferenceURI", 2);
            if (Event.DtItems.Rows.Count > 0)
                Terms.Add(Event);
            DiversityWorkbench.BackLinkDomain Specimen = this.BackLinkDomain(SC, URI, "Specimen", "CollectionSpecimenReference", "ReferenceURI", 3);
            if (Specimen.DtItems.Rows.Count > 0)
                Terms.Add(Specimen);
            return Terms;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainTaxa(ServerConnection SC, string URI, System.Collections.Generic.List<string> Restrictions = null)
        {
            System.Collections.Generic.Dictionary<string, BackLinkDomain> Taxa = BackLinkDomains(ModuleType.TaxonNames);
            Taxa["Identification.NameURI"].ImageKey = 2;

            System.Collections.Generic.List<BackLinkDomain> Terms = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain Ident = this.BackLinkDomain(SC, URI, "Identification", "Identification", "NameURI", 2, Restrictions);
            Ident.CacheColumnName = "TaxonomicName";
            Ident.AddBacklinkColumn("Identification", "TaxonomicName", "Taxonomic name");
            Ident.AddBacklinkColumn("IdentificationUnit", "FamilyCache", "Family");
            Ident.AddBacklinkColumn("IdentificationUnit", "OrderCache", "Order");
            Ident.AddBacklinkColumn("IdentificationUnit", "HierarchyCache", "Hierarchy");
            if (Ident.DtItems.Rows.Count > 0)
                Terms.Add(Ident);
            return Terms;

        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainTerm(ServerConnection SC, string URI)
        {
            System.Collections.Generic.List<BackLinkDomain> Terms = new List<BackLinkDomain>();
            DiversityWorkbench.BackLinkDomain Prop = this.BackLinkDomain(SC, URI, "Site property", "CollectionEventProperty", "PropertyURI", 2);
            if (Prop.DtItems.Rows.Count > 0)
                Terms.Add(Prop);
            DiversityWorkbench.BackLinkDomain Ident = this.BackLinkDomain(SC, URI, "Identification", "Identification", "TermURI", 3);
            if (Ident.DtItems.Rows.Count > 0)
                Terms.Add(Ident);
            DiversityWorkbench.BackLinkDomain Part = this.BackLinkDomain(SC, URI, "Part", "CollectionSpecimenPartDescription", "DescriptionTermURI", 4);
            if (Part.DtItems.Rows.Count > 0)
                Terms.Add(Part);
            return Terms;
        }

        private System.Collections.Generic.List<BackLinkDomain> BackLinkDomainPlot(ServerConnection SC, string URI)
        {
            System.Collections.Generic.List<BackLinkDomain> Plots = new List<BackLinkDomain>();

            // Localisation 
            DiversityWorkbench.BackLinkDomain EL = this.BackLinkDomain(SC, URI, "Event localisation", "CollectionEventLocalisation", "Location2", 13);
            //EL.AddBacklinkColumn("CollectionEvent", "CountryCache", "Country");
            //EL.AddBacklinkColumn("CollectionEventLocalisation", "OrderCache", "Order");
            //EL.AddBacklinkColumn("CollectionEventLocalisation", "HierarchyCache", "Hierarchy");
            if (EL.DtItems.Rows.Count > 0)
                Plots.Add(EL);

            return Plots;

            //System.Collections.Generic.List<BackLinkDomain> Terms = new List<BackLinkDomain>();
            //DiversityWorkbench.BackLinkDomain Prop = this.BackLinkDomain(SC, URI, "Site property", "CollectionEventProperty", "PropertyURI", 2);
            //if (Prop.DtItems.Rows.Count > 0)
            //    Terms.Add(Prop);
            //DiversityWorkbench.BackLinkDomain Ident = this.BackLinkDomain(SC, URI, "Identification", "Identification", "TermURI", 3);
            //if (Ident.DtItems.Rows.Count > 0)
            //    Terms.Add(Ident);
            //DiversityWorkbench.BackLinkDomain Part = this.BackLinkDomain(SC, URI, "Part", "CollectionSpecimenPartDescription", "DescriptionTermURI", 4);
            //if (Part.DtItems.Rows.Count > 0)
            //    Terms.Add(Part);
            //return Terms;
        }


        private DiversityWorkbench.BackLinkDomain BackLinkDomain(ServerConnection SC, string URI, string DisplayText, string Table, string LinkColumn, int ImageKey, System.Collections.Generic.List<string> Restrictions = null)
        {
            DiversityWorkbench.BackLinkDomain BackLink = new BackLinkDomain(DisplayText, Table, LinkColumn, ImageKey);

            // Check if new view exists in Target database
            if (Table.ToLower() == "collectioneventlocalisation" && SC.LinkedServer.Length > 0)
            {
                if (this.View_CollectionEventLocalisation_Core_Exists(SC))
                    Table += "_Core";
                else
                    return BackLink;
            }

            string Prefix = "dbo.";
            if (SC.LinkedServer.Length > 0)
                Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "]." + Prefix;
            if ((Table.IndexOf("Image") > -1 || Table == "IdentificationUnitGeoAnalysis") && SC.LinkedServer.Length > 0)
                Table = "View" + Table;
            string SQL = "SELECT CASE WHEN S.AccessionNumber <> '' THEN S.AccessionNumber ELSE '[ID: ' + CAST(S.CollectionSpecimenID AS varchar) + ']' END AS DisplayText, " +
                "S.CollectionSpecimenID AS ID " +
                "FROM " + Prefix + "[" + Table + "] AS T ";
            if (Table.StartsWith("CollectionEventSeries"))
                SQL += "INNER JOIN " + Prefix + "CollectionEvent AS E ON T.SeriesID = E.SeriesID " +
                    "INNER JOIN " + Prefix + "CollectionSpecimen AS S ON E.CollectionEventID = S.CollectionEventID ";
            else if (Table.StartsWith("CollectionEventLocalisation"))
                SQL += "INNER JOIN " + Prefix + "CollectionSpecimen AS S ON T.CollectionEventID = S.CollectionEventID ";
            else if (Table == "Collection" || Table == "CollectionImage")
            {
                SQL += "INNER JOIN " + Prefix + "CollectionSpecimenPart AS P ON T.CollectionID = P.CollectionID " +
                    "INNER JOIN " + Prefix + "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID ";
            }
            else if (Table.StartsWith("Transaction"))
                SQL += "INNER JOIN " + Prefix + "CollectionSpecimenTransaction AS P ON T.TransactionID = P.TransactionID " +
                    "INNER JOIN " + Prefix + "CollectionSpecimen AS S ON P.CollectionSpecimenID = S.CollectionSpecimenID ";
            else
            {
                SQL += "INNER JOIN " + Prefix + "CollectionSpecimen AS S ON ";
                if (Table.StartsWith("CollectionEvent"))
                    SQL += "T.CollectionEventID = S.CollectionEventID ";
                else
                    SQL += "T.CollectionSpecimenID = S.CollectionSpecimenID ";
            }
            SQL += "INNER JOIN " + Prefix + "CollectionSpecimenID_Available AS A ON S.CollectionSpecimenID = A.CollectionSpecimenID " +
                "WHERE(T." + LinkColumn + " = '" + URI + "') ";
            if (Restrictions != null)
            {
                foreach (string R in Restrictions)
                {
                    SQL += " AND " + R;
                }
            }
            SQL += " GROUP BY S.CollectionSpecimenID, S.AccessionNumber ";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
            try
            {
                ad.Fill(BackLink.DtItems);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
            return BackLink;
        }

        private bool View_CollectionEventLocalisation_Core_Exists(ServerConnection SC)
        {
            bool OK = false;
            string SQL = "select count(*) from INFORMATION_SCHEMA.VIEWS v where v.TABLE_NAME = 'CollectionEventLocalisation_Core'";
            int i;
            OK = (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, SC.ConnectionString), out i) && i > 0);
            return OK;
        }

        public override System.Collections.Generic.Dictionary<string, BackLinkDomain> BackLinkDomains(ModuleType CallingModule)
        {
            if (this._backLinkDomains == null)
            {
                this._backLinkDomains = new Dictionary<ModuleType, System.Collections.Generic.Dictionary<string, BackLinkDomain>>();
                // Agents
                System.Collections.Generic.Dictionary<string, BackLinkDomain> AgentDomains = new Dictionary<string, BackLinkDomain>();
                System.Collections.Generic.List<BackLinkColumn> AgentColumns = new List<BackLinkColumn>();
                DiversityWorkbench.BackLinkDomain Collector = new BackLinkDomain("CollectionAgent", "CollectionAgent", "CollectorsAgentURI", "CollectorsName", AgentColumns);
                AgentDomains.Add(Collector.Key, Collector);
                DiversityWorkbench.BackLinkDomain Identifier = new BackLinkDomain("Identification", "Identification", "ResponsibleAgentURI", "ResponsibleName", AgentColumns);
                AgentDomains.Add(Identifier.Key, Identifier);
#if DEBUG
                System.Windows.Forms.MessageBox.Show("da fehlen noch ein paar");
#endif
                _backLinkDomains.Add(ModuleType.Agents, AgentDomains);


                // TaxonNames
                System.Collections.Generic.Dictionary<string, BackLinkDomain> NameDomains = new Dictionary<string, BackLinkDomain>();
                System.Collections.Generic.List<BackLinkColumn> NameColumns = new List<BackLinkColumn>();
                NameColumns.Add(new BackLinkColumn("IdentificationUnit", "FamilyCache", "Family"));
                NameColumns.Add(new BackLinkColumn("IdentificationUnit", "OrderCache", "Order"));
                NameColumns.Add(new BackLinkColumn("IdentificationUnit", "HierarchyCache", "Hierarchy"));
                DiversityWorkbench.BackLinkDomain Ident = new BackLinkDomain("Identification", "Identification", "NameURI", "TaxonName", NameColumns);
                NameDomains.Add(Ident.Key, Ident);
                _backLinkDomains.Add(ModuleType.TaxonNames, NameDomains);
            }
            return this._backLinkDomains[CallingModule];
        }

        //private DiversityWorkbench.BackLinkDomain BackLinkDomain(string DisplayText, string Table, string LinkColumn, string CacheColumn, System.Collections.Generic.List<BackLinkColumn> backLinkColumns)
        //{
        //    DiversityWorkbench.BackLinkDomain backLinkDomain = new BackLinkDomain(DisplayText, Table, LinkColumn, CacheColumn, backLinkColumns);
        //    return backLinkDomain;
        //}

        #endregion

        #region Domains

        public override System.Collections.Generic.Dictionary<string, string> BackLinkValues(string Domain, string ConnectionString, string URI)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            this._ServerConnection.setConnection(ConnectionString);
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
            string SQL = "SELECT ProjectID FROM ProjectProxy WHERE ProjectURI = '" + URI + "'";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
            con.Open();
            System.Data.DataTable dt = new System.Data.DataTable();
            ad.Fill(dt);
            con.Close();
            con.Dispose();
            ad.Dispose();
            if (dt.Rows.Count > 0)
            {
                Values = ProjectProxyValues(int.Parse(dt.Rows[0][0].ToString()));
                return Values;
            }


            int ID;
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV
                in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()].ServerConnectionList())
            {
                this._ServerConnection.DatabaseName = KV.Value.DatabaseName;
                this._ServerConnection.BaseURL = KV.Value.BaseURL;
                this._ServerConnection.DatabaseServer = KV.Value.DatabaseServer;
                this._ServerConnection.DatabaseServerPort = KV.Value.DatabaseServerPort;
                this._ServerConnection.IsTrustedConnection = KV.Value.IsTrustedConnection;
                this._ServerConnection.LinkedServer = KV.Value.LinkedServer;
                if (!KV.Value.IsTrustedConnection)
                {
                    this._ServerConnection.DatabaseUser = KV.Value.DatabaseUser;
                    this._ServerConnection.DatabasePassword = KV.Value.DatabasePassword;
                }
                //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
                //string SQL = "SELECT ProjectID FROM ProjectProxy WHERE ProjectURI = '" + URI + "'";
                //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
                //con.Open();
                //System.Data.DataTable dt = new System.Data.DataTable();
                //ad.Fill(dt);
                //con.Close();
                //con.Dispose();
                //ad.Dispose();
                //if (dt.Rows.Count > 0)
                //{
                //    Values = ProjectProxyValues(int.Parse(dt.Rows[0][0].ToString()));
                //    return Values;
                //}


                //foreach(System.Data.DataRow R in dt.Rows)
                //{

                //}
                //if (URI.ToLower().StartsWith(KV.Value.BaseURL.ToLower()))
                //{
                //    string IdInUri = URI.Substring(KV.Value.BaseURL.Length);
                //    if (int.TryParse(IdInUri, out ID))
                //    {
                //        switch (Domain)
                //        {
                //            case "ProjectProxy":
                //                this._UnitValues = this.ProjectProxyValues(ID);
                //                break;
                //        }
                //    }
                //    //else
                //    //{
                //    //    if (IdInUri.IndexOf("/") > 1)
                //    //    {
                //    //        string Domain = IdInUri.Substring(0, IdInUri.IndexOf("/"));
                //    //        if (int.TryParse(IdInUri.Substring(IdInUri.IndexOf("/")), out ID))
                //    //            this._UnitValues = this.UnitValues(Domain, ID);
                //    //    }
                //    //}
                //}
            }

            //Values = DomainValues(Domain, URI);
            //switch (Domain)
            //{
            //    //case "Transaction":
            //    //    Values = TransactionValues(ID);
            //    //    break;
            //    case "ProjectProxy":
            //        break;
            //    //default:
            //    //    Values = UnitValues(ID);
            //    //    break;
            //}
            return Values;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(string Domain, int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            switch (Domain)
            {
                case "Transaction":
                    Values = TransactionValues(ID);
                    break;
                case "ProjectProxy":
                    Values = ProjectProxyValues(ID);
                    break;
                default:
                    Values = UnitValues(ID);
                    break;
            }
            return Values;
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns(string Domain)
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns;
            switch (Domain)
            {
                case "Transaction":
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
                    QueryDisplayColumns[0].DisplayText = "TransactionTitle";
                    QueryDisplayColumns[0].DisplayColumn = "TransactionTitle";
                    QueryDisplayColumns[0].OrderColumn = "TransactionTitle";
                    QueryDisplayColumns[0].IdentityColumn = "TransactionID";
                    QueryDisplayColumns[0].TableName = "Transaction";
                    break;
                case "ProjectProxy":
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
                    QueryDisplayColumns[0].DisplayText = "Project";
                    QueryDisplayColumns[0].DisplayColumn = "Project";
                    QueryDisplayColumns[0].OrderColumn = "Project";
                    QueryDisplayColumns[0].IdentityColumn = "ProjectID";
                    QueryDisplayColumns[0].TableName = "ProjectProxy";
                    break;
                default:
                    QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[0];
                    break;
            }
            return QueryDisplayColumns;
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions(string Domain)
        {
            string Database = "DiversityCollection";
            try
            {
                Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityCollection"].ServerConnection.DatabaseName;
            }
            catch { }

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();
            switch (Domain)
            {
                case "Transaction":
                    #region Representation

                    string Description = "Transaction";
                    DiversityWorkbench.QueryCondition qTransaction = new DiversityWorkbench.QueryCondition(true, "Transaction", "TransactionID", "TransactionTitle", "Transaction", "Transaction", "Transaction", Description);
                    QueryConditions.Add(qTransaction);

                    //Description = DiversityWorkbench.Functions.ColumnDescription("Transaction", "ExternalDatabase");
                    //DiversityWorkbench.QueryCondition qExternalDatabase = new DiversityWorkbench.QueryCondition(false, "Transaction", "TransactionID", "ExternalDatabase", "Transaction", "Ext. database", "External database", Description);
                    //QueryConditions.Add(qExternalDatabase);

                    //Description = DiversityWorkbench.Functions.ColumnDescription("Transaction", "ExternalDatabaseAuthors");
                    //DiversityWorkbench.QueryCondition qExternalDatabaseAuthors = new DiversityWorkbench.QueryCondition(false, "Transaction", "TransactionID", "ExternalDatabaseAuthors", "Transaction", "Ext. DB aut.", "External database authors", Description);
                    //QueryConditions.Add(qExternalDatabaseAuthors);

                    #endregion

                    break;
                case "ProjectProxy":
                    string DescriptionProject = "ProjectProxy";
                    DiversityWorkbench.QueryCondition qProject = new DiversityWorkbench.QueryCondition(true, "ProjectProxy", "ProjectID", "Project", "Project", "Project", "Project", DescriptionProject);
                    QueryConditions.Add(qProject);
                    break;
                default:
                    break;
            }
            return QueryConditions;
        }

        private System.Collections.Generic.Dictionary<string, string> TransactionValues(int ID)
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT dbo.BaseURL() + 'Transaction/' + CAST(TransactionID AS varchar) AS _URI, TransactionTitle AS _DisplayText, " +
                    "TransactionID, TransactionTitle, TransactionType, ReportingCategory, AdministratingCollectionID, MaterialDescription, MaterialCategory, MaterialCollectors, FromCollectionID, FromTransactionPartnerName, " +
                    "FromTransactionPartnerAgentURI, FromTransactionNumber, ToCollectionID, ToTransactionPartnerName, ToTransactionPartnerAgentURI, ToTransactionNumber, NumberOfUnits, Investigator, TransactionComment, " +
                    "BeginDate, AgreedEndDate, ActualEndDate, DateSupplement, InternalNotes, ToRecipient, ResponsibleName, ResponsibleAgentURI, MaterialSource " +
                    "FROM   [Transaction] AS T " +
                    "WHERE   TransactionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT AgentName, AgentURI, AgentRole, Notes " +
                    "FROM TransactionAgent AS A " +
                    "WHERE TransactionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT Date, TransactionText, InternalNotes, DisplayText, DocumentURI, DocumentType " +
                    "FROM  TransactionDocument AS D " +
                    "WHERE TransactionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT Amount, ForeignAmount, ForeignCurrency, Identifier, PaymentURI, PayerName, PayerAgentURI, RecipientName, RecipientAgentURI, PaymentDate, PaymentDateSupplement, Notes " +
                    "FROM TransactionPayment AS P " +
                    "WHERE TransactionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

            }
            return Values;
        }

        private System.Collections.Generic.Dictionary<string, string> ProjectProxyValues(int ID)
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT dbo.BaseURL() + 'ProjectProxy/' + CAST(ProjectID AS varchar) AS _URI, Project AS _DisplayText, " +
                    "ProjectID, CreateArchive, ArchiveProtocol, StableIdentifierBase, convert(nvarchar(20), LastChanges, 120) as [Last changes] " +
                    "FROM   [ProjectProxy] AS T " +
                    "WHERE   ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT TOP (1) substring([BaseURL], 1, len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500))) AS Server, " +
                    "replace(substring([BaseURL],  len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500)) + 1, 500), '/', '') AS [Database] " +
                    "FROM   [ViewBaseURL] ";
                this.getDataFromTable(SQL, ref Values);


                SQL = "SELECT P.DisplayText AS Processing " +
                    " FROM ProjectProcessing AS PP INNER JOIN Processing AS P ON PP.ProcessingID = P.ProcessingID " +
                    " WHERE PP.ProjectID = " + ID.ToString() +
                    " ORDER BY Processing ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT A.DisplayText AS Analysis " +
                    "FROM ProjectAnalysis AS P INNER JOIN Analysis AS A ON P.AnalysisID = A.AnalysisID " +
                    "WHERE P.ProjectID = " + ID.ToString() +
                    "ORDER BY Analysis ";
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT COUNT(*) AS [Number of specimen] " +
                    "FROM  CollectionProject " +
                    "GROUP BY ProjectID " +
                    "HAVING ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT case when U.CombinedNameCache <> '' then U.CombinedNameCache else P.LoginName end + CASE WHEN P.ReadOnly = 1 then ' (Read only)' else '' end AS [User] " +
                    "FROM ProjectUser AS P INNER JOIN UserProxy AS U ON P.LoginName = U.LoginName " +
                    "WHERE P.ProjectID = " + ID.ToString() +
                    "ORDER BY [User] ";
                this.getDataFromTable(SQL, ref Values);

            }
            return Values;
        }

        //private System.Collections.Generic.Dictionary<string, string> DomainValues(string Domain, string URI)
        //{
        //    int ID = 0;
        //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV
        //        in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()[this.ServiceName()].ServerConnectionList())
        //    {
        //        if (URI.ToLower().StartsWith(KV.Value.BaseURL.ToLower()))
        //        {
        //            string IdInUri = URI.Substring(KV.Value.BaseURL.Length);
        //            this._ServerConnection.DatabaseName = KV.Value.DatabaseName;
        //            this._ServerConnection.BaseURL = KV.Value.BaseURL;
        //            this._ServerConnection.DatabaseServer = KV.Value.DatabaseServer;
        //            this._ServerConnection.DatabaseServerPort = KV.Value.DatabaseServerPort;
        //            this._ServerConnection.IsTrustedConnection = KV.Value.IsTrustedConnection;
        //            this._ServerConnection.LinkedServer = KV.Value.LinkedServer;
        //            if (!KV.Value.IsTrustedConnection)
        //            {
        //                this._ServerConnection.DatabaseUser = KV.Value.DatabaseUser;
        //                this._ServerConnection.DatabasePassword = KV.Value.DatabasePassword;
        //            }
        //            if (int.TryParse(IdInUri, out ID))
        //            {
        //                switch (Domain)
        //                {
        //                    case "ProjectProxy":
        //                        this._UnitValues = this.ProjectProxyValues(ID);
        //                        break;
        //                }
        //            }
        //            //else
        //            //{
        //            //    if (IdInUri.IndexOf("/") > 1)
        //            //    {
        //            //        string Domain = IdInUri.Substring(0, IdInUri.IndexOf("/"));
        //            //        if (int.TryParse(IdInUri.Substring(IdInUri.IndexOf("/")), out ID))
        //            //            this._UnitValues = this.UnitValues(Domain, ID);
        //            //    }
        //            //}
        //        }
        //    }

        //    string Prefix = "";
        //    if (this._ServerConnection.LinkedServer.Length > 0)
        //        Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
        //    else Prefix = "dbo.";

        //    System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
        //    if (this._ServerConnection.ConnectionString.Length > 0)
        //    {
        //        string SQL = "SELECT dbo.BaseURL() + 'ProjectProxy/' + CAST(ProjectID AS varchar) AS _URI, Project AS _DisplayText, " +
        //            "ProjectID, CreateArchive, ArchiveProtocol, StableIdentifierBase, convert(nvarchar(20), LastChanges, 120) as [Last changes] " +
        //            "FROM   [ProjectProxy] AS T " +
        //            "WHERE   ProjectID = " + ID.ToString();
        //        this.getDataFromTable(SQL, ref Values);

        //        SQL = "SELECT TOP (1) substring([BaseURL], 1, len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500))) AS Server, " +
        //            "replace(substring([BaseURL],  len([BaseURL]) - CHARINDEX('/', substring(REVERSE([BaseURL]), 2, 500)) + 1, 500), '/', '') AS [Database] " +
        //            "FROM   [ViewBaseURL] ";
        //        this.getDataFromTable(SQL, ref Values);


        //        SQL = "SELECT P.DisplayText AS Processing " +
        //            " FROM ProjectProcessing AS PP INNER JOIN Processing AS P ON PP.ProcessingID = P.ProcessingID " +
        //            " WHERE PP.ProjectID = " + ID.ToString() +
        //            " ORDER BY Processing ";
        //        this.getDataFromTable(SQL, ref Values);

        //        SQL = "SELECT A.DisplayText AS Analysis " +
        //            "FROM ProjectAnalysis AS P INNER JOIN Analysis AS A ON P.AnalysisID = A.AnalysisID " +
        //            "WHERE P.ProjectID = " + ID.ToString() +
        //            "ORDER BY Analysis ";
        //        this.getDataFromTable(SQL, ref Values);

        //        SQL = "SELECT COUNT(*) AS [Number of specimen] " +
        //            "FROM  CollectionProject " +
        //            "GROUP BY ProjectID " +
        //            "HAVING ProjectID = " + ID.ToString();
        //        this.getDataFromTable(SQL, ref Values);

        //        SQL = "SELECT case when U.CombinedNameCache <> '' then U.CombinedNameCache else P.LoginName end + CASE WHEN P.ReadOnly = 1 then ' (Read only)' else '' end AS [User] " +
        //            "FROM ProjectUser AS P INNER JOIN UserProxy AS U ON P.LoginName = U.LoginName " +
        //            "WHERE P.ProjectID = " + ID.ToString() +
        //            "ORDER BY [User] ";
        //        this.getDataFromTable(SQL, ref Values);

        //    }
        //    return Values;
        //}

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
