using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityWorkbench
{
    public class ApplicationEntity : DiversityWorkbench.WorkbenchUnit, DiversityWorkbench.IWorkbenchUnit
    {
        #region Construction
        public ApplicationEntity(DiversityWorkbench.ServerConnection ServerConnection)
            : base(ServerConnection)
        {
        }

        #endregion

        #region Interface

        public override string ServiceName() { return "DiversityWorkbench"; }

        public override System.Collections.Generic.Dictionary<string, string> UnitValues(string Entity)
        {
            System.Collections.Generic.Dictionary<string, string> Values = new Dictionary<string, string>();
            if (this.ServerConnection.ConnectionString.Length > 0)
            {
                string SQL = "SELECT Entity AS _URI, Entity AS _DisplayText, Entity " +
                    "FROM Entity " +
                    "WHERE Entity = " + Entity.ToString();
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

        public string MainTable() { return "Entity"; }

        public DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns()
        {
            DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
            QueryDisplayColumns[0].DisplayText = "Entity";
            QueryDisplayColumns[0].DisplayColumn = "Entity";

            for (int i = 0; i < QueryDisplayColumns.Length; i++)
            {
                if (QueryDisplayColumns[i].OrderColumn == null)
                    QueryDisplayColumns[i].OrderColumn = QueryDisplayColumns[i].DisplayColumn;
                if (QueryDisplayColumns[i].IdentityColumn == null)
                    QueryDisplayColumns[i].IdentityColumn = "Entity";
                if (QueryDisplayColumns[i].TableName == null)
                    QueryDisplayColumns[i].TableName = "Entity";
                if (QueryDisplayColumns[i].TipText == null)
                    QueryDisplayColumns[i].TipText = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(QueryDisplayColumns[i].TableName, QueryDisplayColumns[i].DisplayColumn);
            }
            return QueryDisplayColumns;
        }

        public System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions()
        {
            string Database = DiversityWorkbench.Settings.DatabaseName;
            string SQL = "";
            string Description = "";
            System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

            Description = DiversityWorkbench.Functions.ColumnDescription("Entity", "Entity");
            DiversityWorkbench.QueryCondition qEntity = new DiversityWorkbench.QueryCondition(true, "Entity", "Entity", "Entity", "Entity", "Entity", "Entity", Description, false, false, false, false);
            QueryConditions.Add(qEntity);

            Description = DiversityWorkbench.Functions.ColumnDescription("EntityRepresentation", "EntityContext");
            DiversityWorkbench.QueryCondition qEntityContext = new DiversityWorkbench.QueryCondition(true, "EntityRepresentation", "Entity", "EntityContext", "Entity", "Context", "Context", Description, "EntityContext_Enum", Database);
            QueryConditions.Add(qEntityContext);

            Description = DiversityWorkbench.Functions.ColumnDescription("EntityRepresentation", "LanguageCode");
            DiversityWorkbench.QueryCondition qLanguageCode = new DiversityWorkbench.QueryCondition(true, "EntityRepresentation", "Entity", "LanguageCode", "Entity", "Lang.", "Language code", Description, "EntityLanguageCode_Enum", Database);
            QueryConditions.Add(qLanguageCode);

            Description = DiversityWorkbench.Functions.ColumnDescription("EntityRepresentation", "DisplayText");
            DiversityWorkbench.QueryCondition qDisplayText = new DiversityWorkbench.QueryCondition(true, "EntityRepresentation", "Entity", "DisplayText", "Entity", "Disp.", "Display text", Description);
            QueryConditions.Add(qDisplayText);

            Description = DiversityWorkbench.Functions.ColumnDescription("EntityRepresentation", "Abbreviation");
            DiversityWorkbench.QueryCondition qAbbreviation = new DiversityWorkbench.QueryCondition(true, "EntityRepresentation", "Entity", "Abbreviation", "Entity", "Abbr.", "Abbreviation", Description);
            QueryConditions.Add(qAbbreviation);

            Description = DiversityWorkbench.Functions.ColumnDescription("EntityUsage", "EntityUsage");
            DiversityWorkbench.QueryCondition qEntityUsage = new DiversityWorkbench.QueryCondition(true, "EntityUsage", "Entity", "EntityUsage", "Entity", "Usage", "Usage", Description, "EntityUsage_Enum", Database);
            QueryConditions.Add(qEntityUsage);

            Description = DiversityWorkbench.Functions.ColumnDescription("EntityUsage", "PresetValue");
            DiversityWorkbench.QueryCondition qPresetValue = new DiversityWorkbench.QueryCondition(true, "EntityUsage", "Entity", "PresetValue", "Entity", "Preset", "Preset value", Description, false, false, false, false);
            QueryConditions.Add(qPresetValue);

            return QueryConditions;
        }

        #endregion


    }
}
