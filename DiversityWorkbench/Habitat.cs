using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class Habitat : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Construction
        public Habitat(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Interface

        //public override System.Collections.Generic.List<string> DatabaseList()
        //{
        //    if (this._DatabaseList == null)
        //    {
        //        this._DatabaseList = new List<string>();
        //        this._DatabaseList.Add("DiversityHabitats");
        //    }
        //    return this._DatabaseList;
        //}

        //public string[] DatabaseList()
        //{
        //    string[] Databases = new string[1] { "DiversityHabitats" };
        //    return Databases;
        //}

        public System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT dbo.BaseURL() + CAST(HabitatID AS VARCHAR) as _URI, HabitatName as _DisplayText, HabitatName, HierarchyListCache " +
                    "FROM Habitat " +
                    "WHERE HabitatID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);

                SQL = "SELECT HabitatExternalDatabase.ExternalDatabaseName + " +
                    "case when HabitatExternalDatabase.ExternalDatabaseVersion is null then '' " +
                    "else ' Vers.: ' + HabitatExternalDatabase.ExternalDatabaseVersion end AS [Habitat database] " +
                    "FROM HabitatExternalDatabase INNER JOIN " +
                    "Habitat ON HabitatExternalDatabase.ExternalDatabaseID = Habitat.ExternalDatabaseID " +
                    "WHERE Habitat.HabitatID = " + ID.ToString();
                this.getDataFromTable(SQL, ref Values);
            }
            return Values;
        }

        public string MainTable() { return "Habitat"; }
        
        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
            QueryDisplayColumns[0].DisplayText = "Habitat";
            QueryDisplayColumns[0].DisplayColumn = "HabitatName";
            QueryDisplayColumns[0].OrderColumn = "HabitatName";
            QueryDisplayColumns[0].IdentityColumn = "HabitatID";
            QueryDisplayColumns[0].TableName = "Habitat";
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            string Description = DiversityWorkbench.Functions.ColumnDescription("Habitat", "HabitatName");
            DiversityWorkbench.QueryCondition qHabitat = new DiversityWorkbench.QueryCondition(true, "Habitat", "HabitatID", "HabitatName", "Habitat", "Habitat", "Habitat", Description);
            QueryConditions.Add(qHabitat);

            // DATABASE
            System.Data.DataTable dtHabitatList = new System.Data.DataTable();
            string SQL = "SELECT ExternalDatabaseID AS [Value], HabitatExternalDatabase.ExternalDatabaseName + " +
                    "case when HabitatExternalDatabase.ExternalDatabaseVersion is null then '' " +
                    "else ' Vers.: ' + HabitatExternalDatabase.ExternalDatabaseVersion end AS Display " +
                    "FROM HabitatExternalDatabase " +
                    "ORDER BY HabitatExternalDatabase.ExternalDatabaseName";
            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this._ServerConnection.ConnectionString);
                try { a.Fill(dtHabitatList); }
                catch { }
            }
            if (dtHabitatList.Columns.Count == 0)
            {
                System.Data.DataColumn Value = new System.Data.DataColumn("Value");
                System.Data.DataColumn Display = new System.Data.DataColumn("Display");
                dtHabitatList.Columns.Add(Value);
                dtHabitatList.Columns.Add(Display);
            }
            SQL = "FROM HabitatExternalDatabase INNER JOIN " +
                "Habitat ON HabitatExternalDatabase.ExternalDatabaseID = Habitat.ExternalDatabaseID ";
            Description = DiversityWorkbench.Functions.ColumnDescription("HabitatExternalDatabase", "ExternalDatabaseName");
            DiversityWorkbench.QueryCondition qHabitatList = new DiversityWorkbench.QueryCondition(true, "HabitatExternalDatabase", "HabitatID", true, SQL, "HabitatExternalDatabase.ExternalDatabaseID", "Habitat", "Database", "Habitat database", Description, dtHabitatList, false);
            QueryConditions.Add(qHabitatList);

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
        //        this._ServerConnection.ModuleName = "DiversityHabitats";
        //        this._ServerConnection.DatabaseName = "DiversityHabitats";
        //    }
        //}

        #endregion    


    }
}
