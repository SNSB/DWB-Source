using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class Resource : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Construction
        public Resource(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityResources"; }

        public System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT ExsAbbreviation AS Abbreviation, ExsTitle AS Title, EditingInstitution, EditingLocationOri AS EditingLocation, " +
                    "case when ExsNumberFirst  is null or len(ExsNumberFirst) = 0 then '' else ExsNumberFirst end + " +
                    "case when ExsNumberFirst  is null or len(ExsNumberFirst) = 0 or ExsNumberLast  is null or len(ExsNumberLast) = 0 then '' else ' - ' end + " +
                    "case when ExsNumberLast  is null or len(ExsNumberLast) = 0 then '' else ExsNumberLast end as Numbers, " +
                    "case when ExsPublYearFirst  is null or len(ExsPublYearFirst) = 0 then '' else ExsPublYearFirst end + " +
                    "case when ExsPublYearFirst  is null or len(ExsPublYearFirst) = 0 or ExsPublYearLast  is null or len(ExsPublYearLast) = 0 then '' else ' - ' end + " +
                    "case when ExsPublYearLast  is null or len(ExsPublYearLast) = 0 then '' else ExsPublYearLast end as Years " +
                    "FROM Exsiccata " +
                    "WHERE ExsiccataID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT Name AS Editors " +
                    "FROM ExsiccataEditor " +
                    "WHERE ExsiccataID = " + ID.ToString() +
                    "ORDER BY Sequence";
                this.getDataFromTable(SQL, ref Values);
            }
            return Values;
        }

        public string MainTable() { return "Exsiccata"; }
        
        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[3];
            QueryDisplayColumns[0].DisplayText = "Abbreviation";
            QueryDisplayColumns[0].DisplayColumn = "ExsAbbreviation";
            QueryDisplayColumns[0].OrderColumn = "ExsAbbreviation";
            QueryDisplayColumns[0].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[0].TableName = "Exsiccata";
            QueryDisplayColumns[1].DisplayText = "Title";
            QueryDisplayColumns[1].DisplayColumn = "ExsTitle";
            QueryDisplayColumns[1].OrderColumn = "ExsTitle";
            QueryDisplayColumns[1].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[1].TableName = "Exsiccata";
            QueryDisplayColumns[2].DisplayText = "Editor";
            QueryDisplayColumns[2].DisplayColumn = "Name";
            QueryDisplayColumns[2].OrderColumn = "Name";
            QueryDisplayColumns[2].IdentityColumn = "ExsiccataID";
            QueryDisplayColumns[2].TableName = "ExsiccataEditor";
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            string Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "ExsAbbreviation");
            DiversityWorkbench.QueryCondition qExsAbbreviation = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "ExsAbbreviation", "Exsiccata", "Abbrev.", "Exsiccata abbreviation", Description);
            QueryConditions.Add(qExsAbbreviation);

            Description = DiversityWorkbench.Functions.ColumnDescription("Exsiccata", "ExsTitle");
            DiversityWorkbench.QueryCondition qExsTitle = new DiversityWorkbench.QueryCondition(true, "Exsiccata", "ExsiccataID", "ExsTitle", "Exsiccata", "Title.", "Exsiccata title", Description);
            QueryConditions.Add(qExsTitle);

            Description = DiversityWorkbench.Functions.ColumnDescription("ExsiccataEditor", "Name");
            DiversityWorkbench.QueryCondition qName = new DiversityWorkbench.QueryCondition(true, "ExsiccataEditor", "ExsiccataID", "Name", "Exsiccata", "Editor", "Editor", Description, "AccessionDay", "AccessionMonth", "AccessionYear");
            QueryConditions.Add(qName);

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
        //        this._ServerConnection.ModuleName = "DiversityResources";
        //        this._ServerConnection.DatabaseName = "DiversityResources";
        //    }
        //}

        #endregion    


    }
}
