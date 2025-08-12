 using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class Exsiccate : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {

        #region Construction
        public Exsiccate(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityExsiccatae"; }

        public System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            string Prefix = "";
            if (this._ServerConnection.LinkedServer.Length > 0)
                Prefix = "[" + this._ServerConnection.LinkedServer + "]." + this._ServerConnection.DatabaseName + ".dbo.";
            else Prefix = "dbo.";

            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT V.BaseURL + CAST(E.ExsiccataID as varchar) AS _URI, E.ExsAbbreviation AS _DisplayText, E.ExsAbbreviation AS Abbreviation, " +
                    "E.ExsTitle AS Title, E.EditingInstitution AS [E.Editing institution], E.EditingLocationOri AS [E.Editing location], " +
                    "case when E.ExsNumberFirst  is null or len(E.ExsNumberFirst) = 0 then '' else E.ExsNumberFirst end + " +
                    "case when E.ExsNumberFirst  is null or len(E.ExsNumberFirst) = 0 or E.ExsNumberLast  is null or len(E.ExsNumberLast) = 0 then '' else ' - ' end + " +
                    "case when E.ExsNumberLast  is null or len(E.ExsNumberLast) = 0 then '' else E.ExsNumberLast end as Numbers, " +
                    "case when E.ExsPublYearFirst  is null or len(E.ExsPublYearFirst) = 0 then '' else E.ExsPublYearFirst end + " +
                    "case when E.ExsPublYearFirst  is null or len(E.ExsPublYearFirst) = 0 or E.ExsPublYearLast  is null or len(E.ExsPublYearLast) = 0 then '' else ' - ' end + " +
                    "case when E.ExsPublYearLast  is null or len(E.ExsPublYearLast) = 0 then '' else E.ExsPublYearLast end as Years " +
                    "FROM " + Prefix + "Exsiccata AS E, " + Prefix + "ViewBaseURL AS V " +
                    "WHERE E.ExsiccataID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT Name AS Editors " +
                    "FROM " + Prefix + "ExsiccataEditor " +
                    "WHERE ExsiccataID = " + ID.ToString() +
                    "ORDER BY Sequence";
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

        public string MainTable() { return "Exsiccata"; }
        
        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[10];
            QueryDisplayColumns[0].DisplayText = "Abbreviation";
            QueryDisplayColumns[0].DisplayColumn = "ExsAbbreviation";
            QueryDisplayColumns[0].OrderColumn = "ExsAbbreviation";
            QueryDisplayColumns[0].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[0].TableName = "Exsiccata";
            QueryDisplayColumns[0].TipText = "Abbreviation of the exsiccata";

            QueryDisplayColumns[1].DisplayText = "Title";
            QueryDisplayColumns[1].DisplayColumn = "ExsTitle";
            QueryDisplayColumns[1].OrderColumn = "ExsTitle";
            QueryDisplayColumns[1].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[1].TableName = "Exsiccata";
            QueryDisplayColumns[1].TipText = "Title of the exsiccata";

            QueryDisplayColumns[2].DisplayText = "Last year";
            QueryDisplayColumns[2].DisplayColumn = "ExsPublYearLast";
            QueryDisplayColumns[2].OrderColumn = "ExsPublYearLast";
            QueryDisplayColumns[2].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[2].TableName = "Exsiccata";
            QueryDisplayColumns[2].TipText = "Last publication year of the exsiccata";

            QueryDisplayColumns[3].DisplayText = "Coverage";
            QueryDisplayColumns[3].DisplayColumn = "Coverage";
            QueryDisplayColumns[3].OrderColumn = "Coverage";
            QueryDisplayColumns[3].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[3].TableName = "View_Coverage";
            QueryDisplayColumns[3].TipText = "Combined column for the coverage of the exsiccata";

            QueryDisplayColumns[4].DisplayText = "Purpose";
            QueryDisplayColumns[4].DisplayColumn = "Purpose";
            QueryDisplayColumns[4].OrderColumn = "Purpose";
            QueryDisplayColumns[4].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[4].TableName = "View_Coverage";
            QueryDisplayColumns[4].TipText = "Combined column for the purpose of the exsiccata";

            QueryDisplayColumns[5].DisplayText = "Editor";
            QueryDisplayColumns[5].DisplayColumn = "Name";
            QueryDisplayColumns[5].OrderColumn = "Name";
            QueryDisplayColumns[5].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[5].TableName = "ExsiccataEditor";
            QueryDisplayColumns[5].TipText = "Editor of the exsiccata";

            QueryDisplayColumns[6].DisplayText = "Taxon";
            QueryDisplayColumns[6].DisplayColumn = "PublishedName";
            QueryDisplayColumns[6].OrderColumn = "PublishedName";
            QueryDisplayColumns[6].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[6].TableName = "ExsiccataExamples";
            QueryDisplayColumns[6].TipText = "Published taxon name of the example for the exsiccata";

            QueryDisplayColumns[7].DisplayText = "Collection";
            QueryDisplayColumns[7].DisplayColumn = "CollectionName";
            QueryDisplayColumns[7].OrderColumn = "CollectionName";
            QueryDisplayColumns[7].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[7].TipText = "Acronym of the collection of the exsiccata";
            QueryDisplayColumns[7].TableName = "ExsiccataExamples";

            QueryDisplayColumns[8].DisplayText = "Region";
            QueryDisplayColumns[8].DisplayColumn = "Region";
            QueryDisplayColumns[8].OrderColumn = "Region";
            QueryDisplayColumns[8].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[8].TableName = "ExsiccataRegion";
            QueryDisplayColumns[8].TipText = "Region of the exsiccata";

            QueryDisplayColumns[9].DisplayText = "Reference";
            QueryDisplayColumns[9].DisplayColumn = "ReferenceTitle";
            QueryDisplayColumns[9].OrderColumn = "ReferenceTitle";
            QueryDisplayColumns[9].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[9].TableName = "ExsiccataReference";
            QueryDisplayColumns[9].TipText = "Title of the Reference";

            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            #region Exsiccata

            string Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "ExsAbbreviation");
            DiversityWorkbench.QueryCondition qExsAbbreviation = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "ExsAbbreviation", "Exsiccata", "Abbrev.", "Exsiccata abbreviation", Description);
            QueryConditions.Add(qExsAbbreviation);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "ExsiccataID");
            DiversityWorkbench.QueryCondition qExsiccataID = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "ExsiccataID", "Exsiccata", "ID", "Exsiccata ID", Description);
            QueryConditions.Add(qExsiccataID);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "ExsTitle");
            DiversityWorkbench.QueryCondition qExsTitle = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "ExsTitle", "Exsiccata", "Title", "Exsiccata title", Description);
            QueryConditions.Add(qExsTitle);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "EditingInstitution");
            DiversityWorkbench.QueryCondition qEdInstitution = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "EditingInstitution", "Exsiccata", "Institution", "Editing institution", Description);
            QueryConditions.Add(qEdInstitution);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "EditingLocationOri");
            DiversityWorkbench.QueryCondition qEditingLocation = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "EditingLocationOri", "Exsiccata", "Location", "Editing location", Description);
            QueryConditions.Add(qEditingLocation);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "EditingLocationCurrent");
            DiversityWorkbench.QueryCondition qEditingLocationCurrent = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "EditingLocationCurrent", "Exsiccata", "Curr.loc.", "Current editing location", Description);
            QueryConditions.Add(qEditingLocationCurrent);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "ExsNumberFirst");
            DiversityWorkbench.QueryCondition qExsNumberFirst = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "ExsNumberFirst", "Exsiccata", "First Nr.", "First number", Description);
            QueryConditions.Add(qExsNumberFirst);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "ExsNumberLast");
            DiversityWorkbench.QueryCondition qExsNumberLast = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "ExsNumberLast", "Exsiccata", "Last Nr.", "Last number", Description);
            QueryConditions.Add(qExsNumberLast);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "ExsPublYearFirst");
            DiversityWorkbench.QueryCondition qExsPublYearFirst = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "ExsPublYearFirst", "Exsiccata", "First year", "First year of publication", Description);
            QueryConditions.Add(qExsPublYearFirst);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "ExsPublYearLast");
            DiversityWorkbench.QueryCondition qExsPublYearLast = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "ExsPublYearLast", "Exsiccata", "Last year", "Last year of publication", Description);
            QueryConditions.Add(qExsPublYearLast);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Problems");
            DiversityWorkbench.QueryCondition qProblems = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "Problems", "Exsiccata", "Problems", "Problems", Description);
            QueryConditions.Add(qProblems);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Notes");
            DiversityWorkbench.QueryCondition qAdditionalNotes = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "Notes", "Exsiccata", "Notes", "Notes", Description);
            QueryConditions.Add(qAdditionalNotes);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Fungi");
            DiversityWorkbench.QueryCondition qFungi = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "Fungi", "Coverage", "Fungi", "Fungi", Description, false, false, false, true);
            QueryConditions.Add(qFungi);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Algae");
            DiversityWorkbench.QueryCondition qAlgae = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "Algae", "Coverage", "Algae", "Algae", Description, false, false, false, true);
            QueryConditions.Add(qAlgae);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Bacteria");
            DiversityWorkbench.QueryCondition qBacteria = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "Bacteria", "Coverage", "Bacteria", "Bacteria", Description, false, false, false, true);
            QueryConditions.Add(qBacteria);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Bryophytes");
            DiversityWorkbench.QueryCondition qBryophytes = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "Bryophytes", "Coverage", "Bryoph.", "Bryophytes", Description, false, false, false, true);
            QueryConditions.Add(qBryophytes);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Pteridophytes");
            DiversityWorkbench.QueryCondition qPteridophytes = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "Pteridophytes", "Coverage", "Pteridoph.", "Pteridophytes", Description, false, false, false, true);
            QueryConditions.Add(qPteridophytes);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "HigherPlants");
            DiversityWorkbench.QueryCondition qHigherPlants = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "HigherPlants", "Coverage", "Plants", "Higher plants", Description, false, false, false, true);
            QueryConditions.Add(qHigherPlants);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Zoocecidia");
            DiversityWorkbench.QueryCondition qZoocecidia = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "Zoocecidia", "Coverage", "Zoocecidia", "Zoocecidia", Description, false, false, false, true);
            QueryConditions.Add(qZoocecidia);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Educational");
            DiversityWorkbench.QueryCondition qEducational = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "Educational", "Coverage", "Educat.", "Educational", Description, false, false, false, true);
            QueryConditions.Add(qEducational);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "Scientific");
            DiversityWorkbench.QueryCondition qScientific = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "Scientific", "Coverage", "Scient.", "Scientific", Description, false, false, false, true);
            QueryConditions.Add(qScientific);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "GroupSpecific");
            DiversityWorkbench.QueryCondition qGroupSpecific = new DiversityWorkbench.QueryCondition(false, "Exsiccata", "ExsiccataID", "GroupSpecific", "Coverage", "Group spec.", "Group specific", Description, false, false, false, true);
            QueryConditions.Add(qGroupSpecific);
            
            #endregion

            #region EDITOR

            Description = "If any editor is present";
            DiversityWorkbench.QueryCondition qEditorPresent = new DiversityWorkbench.QueryCondition(false, "ExsiccataEditor", "ExsiccataID", "Editor", "Presence", "Editor present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qEditorPresent);

            Description = DiversityWorkbench.Functions.ColumnDescription("ExsiccataEditor", "Name");
            DiversityWorkbench.QueryCondition qEditor = new DiversityWorkbench.QueryCondition(true, "ExsiccataEditor", "ExsiccataID", "Name", "Editor", "Editor", "Editor", Description);
            QueryConditions.Add(qEditor);
            
            #endregion

            #region REFERENCE

            Description = "If any reference is present";
            DiversityWorkbench.QueryCondition qReferencePresent = new DiversityWorkbench.QueryCondition(false, "ExsiccataReference", "ExsiccataID", "Reference", "Presence", "Reference present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qReferencePresent);

            Description = DiversityWorkbench.Functions.ColumnDescription("ExsiccataReference", "ReferenceTitle");
            DiversityWorkbench.QueryCondition qReference = new DiversityWorkbench.QueryCondition(true, "ExsiccataReference", "ExsiccataID", "ReferenceTitle", "Reference", "Reference", "Reference title", Description);
            QueryConditions.Add(qReference);
            
            #endregion

            #region REGION

            Description = "If any region is present";
            DiversityWorkbench.QueryCondition qRegionPresent = new DiversityWorkbench.QueryCondition(false, "ExsiccataRegion", "ExsiccataID", "Region", "Presence", "Region present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qRegionPresent);

            Description = DiversityWorkbench.Functions.ColumnDescription("ExsiccataRegion", "Region");
            DiversityWorkbench.QueryCondition qRegion = new DiversityWorkbench.QueryCondition(true, "ExsiccataRegion", "ExsiccataID", "Region", "Region", "Region", "Region", Description);
            QueryConditions.Add(qRegion);
            
            #endregion

            #region Expample

            Description = "If any example is present";
            DiversityWorkbench.QueryCondition qExamplePresent = new DiversityWorkbench.QueryCondition(false, "ExsiccataExamples", "ExsiccataID", "Example", "Presence", "Example present", Description, QueryCondition.CheckDataExistence.DatasetsInRelatedTable);
            QueryConditions.Add(qExamplePresent);

            Description = DiversityWorkbench.Functions.ColumnDescription("ExsiccataExamples", "CollectionName");
            DiversityWorkbench.QueryCondition qCollectionName = new DiversityWorkbench.QueryCondition(true, "ExsiccataExamples", "ExsiccataID", "CollectionName", "Example", "Collection", "Collection name", Description);
            QueryConditions.Add(qCollectionName);

            Description = DiversityWorkbench.Functions.ColumnDescription("ExsiccataExamples", "PublishedName");
            DiversityWorkbench.QueryCondition qPublishedName = new DiversityWorkbench.QueryCondition(true, "ExsiccataExamples", "ExsiccataID", "PublishedName", "Example", "Publ.name", "Published name", Description);
            QueryConditions.Add(qPublishedName);

            Description = DiversityWorkbench.Functions.ColumnDescription("ExsiccataExamples", "StorageLocation");
            DiversityWorkbench.QueryCondition qStorageLocation = new DiversityWorkbench.QueryCondition(true, "ExsiccataExamples", "ExsiccataID", "StorageLocation", "Example", "Stor.location", "Storage location", Description);
            QueryConditions.Add(qStorageLocation);

            Description = DiversityWorkbench.Functions.ColumnDescription("ExsiccataExamples", "Number");
            DiversityWorkbench.QueryCondition qNumber = new DiversityWorkbench.QueryCondition(true, "ExsiccataExamples", "ExsiccataID", "Number", "Example", "Number", "Number", Description);
            QueryConditions.Add(qNumber);

            Description = DiversityWorkbench.Functions.ColumnDescription("ExsiccataExamples", "ImageFile");
            DiversityWorkbench.QueryCondition qImageFile = new DiversityWorkbench.QueryCondition(false, "ExsiccataExamples", "ExsiccataID", "ImageFile", "Example", "Image", "Image file", Description);
            QueryConditions.Add(qImageFile);
            
            #endregion

            return QueryConditions;
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
        //        }
        //        this._ServerConnection.DatabaseName = "DiversityExsiccatae";
        //        //bool OK = false;
        //        //foreach (string s in this.DatabaseList())
        //        //{
        //        //    if (s == this._ServerConnection.DatabaseName)
        //        //    {
        //        //        OK = true;
        //        //        break;
        //        //    }
        //        //}
        //        //if (!OK) this._ServerConnection.DatabaseName = this.DatabaseList()[0];
        //        this._ServerConnection.ModuleName = "DiversityExsiccatae";
        //    }
        //}

        #endregion    

    }
}
