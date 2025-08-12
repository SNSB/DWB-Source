using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class CollectionExpedition : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Construction
        public CollectionExpedition(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Interface

        public string[] DatabaseList()
        {
            string[] Databases = new string[1] { "DiversityCollection" };
            return Databases;
        }

        public System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT ExpeditionID AS _URI, " +
                    "Description AS _DisplayText, ExpeditionID AS ID, Description," +
                    "ExpeditionCode, " +
                    " DateStart AS [Date] " +
                    "FROM  dbo.CollectionExpedition WHERE ExpeditionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT RTRIM(CONVERT(nvarchar(500), " +
                    "CASE WHEN CollectionYear IS NULL THEN '' ELSE cast(CollectionYear AS varchar) END  + '-' " +
                    " + (CASE WHEN CollectionMonth IS NULL THEN '' ELSE cast(CollectionMonth AS varchar) END) + '-' " +
                    " + (CASE WHEN CollectionDay IS NULL THEN '' ELSE cast(CollectionDay AS varchar) END)   + " +
                    "CASE WHEN LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0 " +
                    "THEN '' ELSE ' ' + LocalityDescription + ' ' END + " +
                    "CASE WHEN HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0 THEN '' ELSE ' ' + HabitatDescription + ' ' END) + " +
                    "CASE WHEN (LocalityDescription IS NULL OR len(Rtrim(LocalityDescription)) = 0) " +
                    "AND (HabitatDescription IS NULL OR len(Rtrim(HabitatDescription)) = 0) " +
                    "THEN '(ID: ' + CONVERT(nvarchar, dbo.CollectionEvent.CollectionEventID) + ')  ' ELSE '' END) " +
                    "AS [Collection event] " +
                    "FROM CollectionEvent INNER JOIN " +
                    "CollectionExpedition ON CollectionExpedition.ExpeditionID = CollectionEvent.ExpeditionID " +
                    "WHERE CollectionExpedition.ExpeditionID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

            }
            return Values;
        }

        public string MainTable() { return "CollectionEvent"; }
        
        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[3];
            QueryDisplayColumns[0].DisplayText = "Expedition";
            QueryDisplayColumns[0].DisplayColumn = "Description";
            QueryDisplayColumns[0].OrderColumn = "Description";
            QueryDisplayColumns[0].IdentityColumn = "ExpeditionID";
            QueryDisplayColumns[0].TableName = "CollectionExpedition";
            QueryDisplayColumns[1].DisplayText = "Date";
            QueryDisplayColumns[1].DisplayColumn = "convert(varchar(50), DateStart , 120)";
            QueryDisplayColumns[1].OrderColumn = "DateStart";
            QueryDisplayColumns[1].IdentityColumn = "ExpeditionID";
            QueryDisplayColumns[1].TableName = "CollectionExpedition";
            QueryDisplayColumns[2].DisplayText = "Code";
            QueryDisplayColumns[2].DisplayColumn = "ExpeditionCode";
            QueryDisplayColumns[2].OrderColumn = "ExpeditionCode";
            QueryDisplayColumns[2].IdentityColumn = "ExpeditionID";
            QueryDisplayColumns[2].TableName = "CollectionExpedition";
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            // EXPEDITION
            string Description = DiversityWorkbench.Functions.ColumnDescription("CollectionExpedition", "Description");
            DiversityWorkbench.QueryCondition qDescription = new DiversityWorkbench.QueryCondition(true, "CollectionExpedition", "ExpeditionID", "Description", "Expedition", "Expedition", "Expedition description", Description);
            QueryConditions.Add(qDescription);

            Description = DiversityWorkbench.Functions.ColumnDescription("CollectionExpedition", "ExpeditionCode");
            DiversityWorkbench.QueryCondition qExpeditionCode = new DiversityWorkbench.QueryCondition(true, "CollectionExpedition", "ExpeditionID", "ExpeditionCode", "Expedition", "Code", "Expedition code", Description);
            QueryConditions.Add(qExpeditionCode);

            Description = DiversityWorkbench.Functions.ColumnDescription("CollectionExpedition", "DateStart");
            DiversityWorkbench.QueryCondition qDateStart = new DiversityWorkbench.QueryCondition(true, "CollectionExpedition", "ExpeditionID", "DateStart", "Expedition", "Date", "Expedition date", Description, true, false, false, false);
            QueryConditions.Add(qDateStart);

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
