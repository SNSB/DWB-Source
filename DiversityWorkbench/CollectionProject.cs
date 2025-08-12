using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench
{
    public class CollectionProject : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Construction

        public CollectionProject(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }
        
        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityCollection"; }

        public System.Collections.Generic.Dictionary<string, string> UnitValues(int ID)
        {
            this._UnitValues = new Dictionary<string, string>();

            if (this._ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT dbo.BaseURL() + 'ProjectProxy/' + CAST(ProjectID AS varchar) AS _URI, Project AS _DisplayText, " +
                    "ProjectID, CreateArchive, ArchiveProtocol, StableIdentifierBase, convert(nvarchar(20), LastChanges, 120) as [Last changes] " +
                    "FROM   [ProjectProxy] AS T " +
                    "WHERE   ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT P.DisplayText AS Processing " +
                    " FROM ProjectProcessing AS PP INNER JOIN Processing AS P ON PP.ProcessingID = P.ProcessingID " +
                    " WHERE PP.ProjectID = " + ID.ToString() +
                    " ORDER BY Processing ";
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT A.DisplayText AS Analysis " +
                    "FROM ProjectAnalysis AS P INNER JOIN Analysis AS A ON P.AnalysisID = A.AnalysisID " +
                    "WHEREP.ProjectID = " + ID.ToString() +
                    "ORDER BY Analysis ";
                this.getDataFromTable(SQL, ref this._UnitValues);

                SQL = "SELECT COUNT(*) AS [Number of specimen] " +
                    "FROM  CollectionProject " +
                    "GROUP BY ProjectID " +
                    "HAVING ProjectID = " + ID.ToString();
                this.getDataFromTable(SQL, ref this._UnitValues);
            }
            return this._UnitValues;
        }

        public string MainTable() { return "ProjectProxy"; }

        public DiversityWorkbench.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.QueryDisplayColumn[1];
            QueryDisplayColumns[0].DisplayText = "Project";
            QueryDisplayColumns[0].DisplayColumn = "Project";
            QueryDisplayColumns[0].OrderColumn = "Project";
            QueryDisplayColumns[0].IdentityColumn = "ProjectID";
            QueryDisplayColumns[0].TableName = "ProjectProxy";
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            string Database = "DiversityCollection";
            try
            {
                Database = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityCollection"].ServerConnection.DatabaseName;
            }
            catch { }

            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            string Description = "Project";
            DiversityWorkbench.QueryCondition qProject = new DiversityWorkbench.QueryCondition(true, "ProjectProxy", "ProjectID", "Project", "Project", "Project", "Project", Description);
            QueryConditions.Add(qProject);

            return QueryConditions;
        }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(string Domain, int ID)
        {
            return this.UnitValues(ID);
        }

        public override DiversityWorkbench.QueryDisplayColumn[] QueryDisplayColumns(string Domain)
        {
            return this.QueryDisplayColumns();
        }

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions(string Domain)
        {
            return this.QueryConditions();
        }

        #endregion

    }
}
