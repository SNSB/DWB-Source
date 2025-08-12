using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class CollectionEvent : HierarchicalEntity
    {

        #region Construction

        public CollectionEvent(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable,
            ref System.Windows.Forms.TreeView TreeView,
            System.Windows.Forms.Form Form,
            DiversityWorkbench.UserControls.UserControlQueryList UserControlQueryList,
            System.Windows.Forms.SplitContainer SplitContainerMain,
            System.Windows.Forms.SplitContainer SplitContainerData,
            System.Windows.Forms.ToolStripButton ToolStripButtonSpecimenList,
            //System.Windows.Forms.ImageList ImageListSpecimenList,
            DiversityCollection.UserControls.UserControlSpecimenList UserControlSpecimenList,
            System.Windows.Forms.HelpProvider HelpProvider,
            System.Windows.Forms.ToolTip ToolTip,
            ref System.Windows.Forms.BindingSource BindingSource)
            : base(ref Dataset, DataTable, ref TreeView, Form, UserControlQueryList, SplitContainerMain,
            SplitContainerData, ToolStripButtonSpecimenList, /*ImageListSpecimenList,*/ UserControlSpecimenList,
            HelpProvider, ToolTip, ref BindingSource, null, null)
        {
            this._sqlItemFieldList = " CollectionEventID, Version, SeriesID, CollectorsEventNumber, CollectionDate, CollectionDay, CollectionMonth, CollectionYear, " +
                "CollectionDateSupplement, CollectionDateCategory, CollectionTime, CollectionTimeSpan, LocalityDescription, HabitatDescription, ReferenceTitle, " +
                "ReferenceURI, CollectingMethod, Notes, CountryCache, DataWithholdingReason ";
            this._SpecimenTable = "CollectionSpecimen";
            this._MainTable = "CollectionEvent_Core2";
            this._MainTableContainsHierarchy = false;
        }
        
        #endregion

        #region Functions and properties

        protected override string SqlSpecimenCount(int ID)
        {
            return "SELECT COUNT(*) FROM CollectionEvent_Core2 T INNER JOIN " +
                "CollectionSpecimen S ON T.CollectionEventID = S.CollectionEventID " +
                "WHERE T.CollectionEventID = " + ID.ToString();
        }

        public override string ColumnDisplayText
        {
            get
            {
                return "LocalityDescription";
            }
        }

        public override string ColumnDisplayOrder
        {
            get
            {
                return "LocalityDescription";
            }
        }

        public override string ColumnID
        {
            get
            {
                return "CollectionEventID";
            }
        }

        public override string ColumnParentID
        {
            get
            {
                return "SeriesID";
            }
        }


        #endregion

        #region Interface

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            get
            {
                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[5];

                QueryDisplayColumns[0].DisplayText = "Locality";
                QueryDisplayColumns[0].DisplayColumn = "LocalityDescription";
                QueryDisplayColumns[0].OrderColumn = "LocalityDescription";
                QueryDisplayColumns[0].IdentityColumn = "CollectionEventID";
                QueryDisplayColumns[0].TableName = "CollectionEvent_Core2";

                QueryDisplayColumns[1].DisplayText = "Date";
                QueryDisplayColumns[1].DisplayColumn = "CollectionDate_YMD";
                QueryDisplayColumns[1].OrderColumn = "CollectionDate_YMD";
                QueryDisplayColumns[1].IdentityColumn = "CollectionEventID";
                QueryDisplayColumns[1].TableName = "CollectionEvent_Core2";

                QueryDisplayColumns[2].DisplayText = "Habitat";
                QueryDisplayColumns[2].DisplayColumn = "HabitatDescription";
                QueryDisplayColumns[2].OrderColumn = "HabitatDescription";
                QueryDisplayColumns[2].IdentityColumn = "CollectionEventID";
                QueryDisplayColumns[2].TableName = "CollectionEvent_Core2";

                QueryDisplayColumns[3].DisplayText = "Event number";
                QueryDisplayColumns[3].DisplayColumn = "CollectorsEventNumber";
                QueryDisplayColumns[3].OrderColumn = "CollectorsEventNumber";
                QueryDisplayColumns[3].IdentityColumn = "CollectionEventID";
                QueryDisplayColumns[3].TableName = "CollectionEvent_Core2";

                QueryDisplayColumns[4].DisplayText = "Country";
                QueryDisplayColumns[4].DisplayColumn = "CountryCache";
                QueryDisplayColumns[4].OrderColumn = "CountryCache";
                QueryDisplayColumns[4].IdentityColumn = "CollectionEventID";
                QueryDisplayColumns[4].TableName = "CollectionEvent_Core2";

                return QueryDisplayColumns;
            }
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
        {
            get
            {
                string SQL = "";
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

                // PROJECT
                System.Data.DataTable dtProject = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display UNION SELECT ProjectID AS [Value], Project AS Display " +
                                "FROM ProjectListNotReadOnly " +
                                "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { a.Fill(dtProject); }
                    catch (System.Exception ex) { }
                }
                if (dtProject.Columns.Count == 0)
                {
                    System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                    System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                    dtProject.Columns.Add(Value);
                    dtProject.Columns.Add(Display);
                }
                string Description = DiversityWorkbench.Functions.ColumnDescription("ProjectProxy", "Project");
                SQL = " FROM CollectionSpecimen INNER JOIN " +
                      "CollectionProject T ON CollectionSpecimen.CollectionSpecimenID = T.CollectionSpecimenID ";
                //SQL = " FROM CollectionEvent E " +
                //    " INNER JOIN CollectionSpecimen S ON S.CollectionEventID = E.CollectionEventID " +
                //    " INNER JOIN CollectionProject P ON S.CollectionSpecimenID = P.CollectionSpecimenID ";

                DiversityWorkbench.QueryCondition qProject = new DiversityWorkbench.QueryCondition(true, "CollectionProject", "CollectionEventID", true, SQL, "ProjectID", "Project", "Project", "Project", Description, dtProject, false);
                qProject.IntermediateTable = "CollectionSpecimen";
                qProject.ForeignKeyTable = "CollectionSpecimen";
                qProject.ForeignKey = "CollectionSpecimenID";
                QueryConditions.Add(qProject);

                // GEOGRAPHY
                SQL = " FROM CollectionEventLocalisation T " +
                    " WHERE (T.LocalisationSystemID IN (4, 5))";
                DiversityWorkbench.QueryCondition qAltitude = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation", "CollectionEventID", true, SQL, "AverageAltitudeCache", "Geography", "Av. Alt.", "Average altitude", "Average altitude calculated from the given values", false, false, true, false);
                QueryConditions.Add(qAltitude);

                SQL = " FROM CollectionEventLocalisation T " +
                    " WHERE (T.LocalisationSystemID = 7)";
                DiversityWorkbench.QueryCondition qPlace = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation", "CollectionEventID", true, SQL, "Location1", "Geography", "Place", "Name of a place", "The name of a place");
                QueryConditions.Add(qPlace);

                DiversityWorkbench.QueryCondition qLatitude = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation", "CollectionEventID", "AverageLatitudeCache", "Geography", "Av. Lat.", "Average latitude", "Average latitude calculated from the given values", false, false, true, false);
                QueryConditions.Add(qLatitude);

                DiversityWorkbench.QueryCondition qLongitude = new DiversityWorkbench.QueryCondition(true, "CollectionEventLocalisation", "CollectionEventID", "AverageLongitudeCache", "Geography", "Av. Lat.", "Average longitude", "Average longitude calculated from the given values", false, false, true, false);
                QueryConditions.Add(qLongitude);

                // EVENT
                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectorsEventNumber");
                DiversityWorkbench.QueryCondition qCollectorsEventNumber = new DiversityWorkbench.QueryCondition(true, "CollectionEvent_Core2", "CollectionEventID", "CollectorsEventNumber", "Event", "Number", "Collectors event number", Description);
                QueryConditions.Add(qCollectorsEventNumber);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionEventID");
                DiversityWorkbench.QueryCondition qCollectionEventID = new DiversityWorkbench.QueryCondition(true, "CollectionEvent_Core2", "CollectionEventID", "CollectionEventID", "Event", "ID", "ID of the collection event", Description);
                QueryConditions.Add(qCollectionEventID);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectionDate");
                DiversityWorkbench.QueryCondition qCollectionDate = new DiversityWorkbench.QueryCondition(true, "CollectionEvent_Core2", "CollectionEventID", "CollectionDate", "Event", "Coll.Date", "Collection date", Description, "CollectionDay", "CollectionMonth", "CollectionYear");
                QueryConditions.Add(qCollectionDate);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "LocalityDescription");
                DiversityWorkbench.QueryCondition qLocalityDescription = new DiversityWorkbench.QueryCondition(true, "CollectionEvent_Core2", "CollectionEventID", "LocalityDescription", "Event", "Locality", "Locality description", Description);
                QueryConditions.Add(qLocalityDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "HabitatDescription");
                DiversityWorkbench.QueryCondition qHabitatDescription = new DiversityWorkbench.QueryCondition(true, "CollectionEvent_Core2", "CollectionEventID", "HabitatDescription", "Event", "Habitat", "Habitat description", Description);
                QueryConditions.Add(qHabitatDescription);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "ReferenceTitle");
                DiversityWorkbench.QueryCondition qEventReferenceTitle = new DiversityWorkbench.QueryCondition(false, "CollectionEvent_Core2", "CollectionEventID", "ReferenceTitle", "Event", "Reference", "Reference", Description);
                QueryConditions.Add(qEventReferenceTitle);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CollectingMethod");
                DiversityWorkbench.QueryCondition qCollectingMethod = new DiversityWorkbench.QueryCondition(false, "CollectionEvent_Core2", "CollectionEventID", "CollectingMethod", "Event", "Coll. method", "Collecting method", Description);
                QueryConditions.Add(qCollectingMethod);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "Notes");
                DiversityWorkbench.QueryCondition qEventNotes = new DiversityWorkbench.QueryCondition(false, "CollectionEvent_Core2", "CollectionEventID", "Notes", "Event", "Notes", "Notes", Description);
                QueryConditions.Add(qEventNotes);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "CountryCache");
                DiversityWorkbench.QueryCondition qCountryCache = new DiversityWorkbench.QueryCondition(true, "CollectionEvent_Core2", "CollectionEventID", "CountryCache", "Event", "Country", "Country", Description);
                QueryConditions.Add(qCountryCache);

                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionEvent", "DataWithholdingReason");
                DiversityWorkbench.QueryCondition qDataWithholdingReason = new DiversityWorkbench.QueryCondition(true, "CollectionEvent_Core2", "CollectionEventID", "DataWithholdingReason", "Event", "Withholding", "Data withholding reason", Description);
                QueryConditions.Add(qDataWithholdingReason);

                // SPECIMEN
                SQL = " FROM CollectionSpecimen INNER JOIN " +
                    " CollectionEvent T ON CollectionSpecimen.CollectionEventID = T.CollectionEventID ";
                Description = DiversityWorkbench.Functions.ColumnDescription("CollectionSpecimen", "AccessionNumber");
                DiversityWorkbench.QueryCondition qAccessionNumber = new DiversityWorkbench.QueryCondition(true, "CollectionSpecimen", "CollectionEventID", true, SQL, "AccessionNumber", "Specimen", "Acc. Nr", "Accession number", Description);
                //qAccessionNumber.IntermediateTable = "CollectionSpecimen";
                QueryConditions.Add(qAccessionNumber);

                // UNIT
                SQL = " FROM CollectionEvent T INNER JOIN " +
                      "CollectionSpecimen ON T.CollectionEventID = CollectionSpecimen.CollectionEventID INNER JOIN " +
                      "IdentificationUnit ON CollectionSpecimen.CollectionSpecimenID = IdentificationUnit.CollectionSpecimenID ";
                Description = DiversityWorkbench.Functions.ColumnDescription("IdentificationUnit", "LastIdentificationCache");
                DiversityWorkbench.QueryCondition qUnit = new DiversityWorkbench.QueryCondition(true, "IdentificationUnit", "CollectionEventID", true, SQL, "LastIdentificationCache", "Specimen", "Ident.", "Identification", Description);
                QueryConditions.Add(qUnit);

                return QueryConditions;
            }
        }

        #endregion

        #region Interface Workbench unit (alt)

        //public string[] DatabaseList()
        //{
        //    string[] Databases = new string[1] { "DiversityCollection" };
        //    return Databases;
        //}

        //public System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        //{
        //    System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
        //    if (this._ServerConnection.ConnectionString.Length > 0)
        //    {
        //        string SQL = "SELECT CollectionEventID AS _URI, " +
        //            "RTRIM(CONVERT(nvarchar(500), " +
        //            "CASE WHEN CollectionYear IS NULL THEN '' ELSE cast(CollectionYear AS varchar) END  + '-' " +
        //            " + (CASE WHEN CollectionMonth IS NULL THEN '' ELSE cast(CollectionMonth AS varchar) END) + '-' " +
        //            " + (CASE WHEN CollectionDay IS NULL THEN '' ELSE cast(CollectionDay AS varchar) END)   + " +
        //            "CASE WHEN LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0 " +
        //            "THEN '' ELSE ' ' + LocalityDescription + ' ' END + " +
        //            "CASE WHEN HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0 THEN '' ELSE ' ' + HabitatDescription + ' ' END) + " +
        //            "CASE WHEN (LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0) " +
        //            "AND (HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0) " +
        //            "THEN '(ID: ' + CONVERT(nvarchar, dbo.CollectionEvent.CollectionEventID) + ')  ' ELSE '' END) " +
        //            "AS _DisplayText, CollectionEventID AS ID," +
        //            "RTRIM(CONVERT(nvarchar(500), " +
        //            "CASE WHEN LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0 " +
        //            "THEN '' ELSE ' ' + LocalityDescription + ' ' END + " +
        //            "CASE WHEN HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0 THEN '' ELSE ' ' + HabitatDescription + ' ' END) + " +
        //            "CASE WHEN (LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0) " +
        //            "AND (HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0) " +
        //            "THEN '(ID: ' + CONVERT(nvarchar, dbo.CollectionEvent.CollectionEventID) + ')  ' ELSE '' END) " +
        //            "AS Locality, " +
        //            " RTRIM(CONVERT(nvarchar(50), " +
        //            "(CASE WHEN CollectionYear IS NULL THEN '' ELSE cast(CollectionYear AS varchar) END)  + '-' " +
        //            " + (CASE WHEN CollectionMonth IS NULL THEN '' ELSE cast(CollectionMonth AS varchar) END) + '-' " +
        //            " + (CASE WHEN CollectionDay IS NULL THEN '' ELSE cast(CollectionDay AS varchar) END)  )) " +
        //            "AS [Collection date] " +
        //            "FROM  dbo.CollectionEvent WHERE CollectionEventID = " + ID.ToString();
        //        this.getDataFromTable(SQL, ref Values);

        //        SQL = "SELECT CollectionExpedition.Description AS Expedition " +
        //            "FROM CollectionExpedition INNER JOIN " +
        //            "CollectionEvent ON CollectionExpedition.ExpeditionID = CollectionEvent.ExpeditionID " +
        //            "WHERE CollectionEvent.CollectionEventID = " + ID.ToString();
        //        this.getDataFromTable(SQL, ref Values);

        //        SQL = "SELECT LocalisationSystem.LocalisationSystemName + ': ' + " +
        //            "case when CollectionGeography.Location1 is null then '' else + LocalisationSystem.DisplayTextLocation1 + ' = ' + CollectionGeography.Location1 + '  ' end +  " +
        //            "case when CollectionGeography.Location2 is null then '' else LocalisationSystem.DisplayTextLocation2 + ' = ' + CollectionGeography.Location2 end + '\r\n' AS [Localisation system] " +
        //              "FROM CollectionGeography INNER JOIN " +
        //              "LocalisationSystem ON CollectionGeography.LocalisationSystemID = LocalisationSystem.LocalisationSystemID " +
        //              "WHERE CollectionGeography.CollectionEventID = " + ID.ToString();
        //        this.getDataFromTable(SQL, ref Values);

        //        SQL = "SELECT HabitatName AS Habitat " +
        //            "FROM CollectionHabitat " +
        //            "WHERE CollectionEventID = " + ID.ToString();
        //        this.getDataFromTable(SQL, ref Values);

        //        SQL = "SELECT AccessionNumber AS [Accession number] " +
        //            "FROM  dbo.CollectionEvent INNER JOIN " +
        //            "CollectionSpecimen ON CollectionEvent.CollectionEventID = CollectionSpecimen.CollectionEventID " +
        //            "WHERE CollectionEvent.CollectionEventID = " + ID.ToString();
        //        this.getDataFromTable(SQL, ref Values);

        //        SQL = "SELECT DISTINCT IdentificationUnit.LastIdentificationCache AS Organisms " +
        //            "FROM CollectionEvent INNER JOIN " +
        //            "CollectionSpecimen ON CollectionEvent.CollectionEventID = CollectionSpecimen.CollectionEventID INNER JOIN " +
        //            "IdentificationUnit ON CollectionSpecimen.CollectionSpecimenID = IdentificationUnit.CollectionSpecimenID " +
        //            "WHERE CollectionEvent.CollectionEventID = " + ID.ToString() +
        //            " ORDER BY IdentificationUnit.LastIdentificationCache ";
        //        this.getDataFromTable(SQL, ref Values);

        //    }
        //    return Values;
        //}

        public string MainTable() { return "CollectionEvent"; }
        


        #endregion

        #region Properties Workbench unit (alt)

        //public override DiversityWorkbench.ServerConnection ServerConnection
        //{
        //    get { return _ServerConnection; }
        //    set 
        //    {
        //        if (value != null)
        //            this._ServerConnection = value;
        //        else
        //        {
        //            this._ServerConnection = new DiversityWorkbench.ServerConnection();
        //            this._ServerConnection.DatabaseServer = "127.0.0.1";
        //            this._ServerConnection.IsTrustedConnection = true;
        //        }
        //        this._ServerConnection.DatabaseName = "DiversityCollection";
        //        this._ServerConnection.ModuleName = "DiversityCollection";
        //    }
        //}

        #endregion    

    }
}
