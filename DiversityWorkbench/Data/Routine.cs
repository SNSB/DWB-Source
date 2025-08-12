using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Data
{
    public class Routine
    {
        public static System.Data.DataTable List()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "SELECT ROUTINE_NAME AS [Name], ROUTINE_TYPE AS [Type] " +
                ",DATA_TYPE + case when CHARACTER_MAXIMUM_LENGTH is null then '' else ' (' + cast(CHARACTER_MAXIMUM_LENGTH as varchar) + ')' end AS DataType " +
                ",'' AS Description " +
                ",case when DATA_TYPE IS NULL then 0 else case when DATA_TYPE = 'table' then 2 else 1 end end AS DisplayOrder " +
                "FROM INFORMATION_SCHEMA.ROUTINES " +
                "WHERE ROUTINE_NAME NOT LIKE 'dt_%'  " +
                "AND ROUTINE_NAME NOT LIKE 'sp_%' " +
                "order by DisplayOrder, ROUTINE_NAME";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            foreach(System.Data.DataRow R in dt.Rows)
            {
                string Description = DiversityWorkbench.Forms.FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION, R[0].ToString(), "");
                if (Description.Length > 0)
                    R["Description"] = Description;
            }
            return dt;
        }

        public static System.Data.DataTable Parameters(string Name, DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType Type)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "SELECT P.PARAMETER_NAME AS [NAME] " +
            ", P.DATA_TYPE + CASE WHEN P.CHARACTER_MAXIMUM_LENGTH IS NULL THEN '' ELSE ' (' + CAST(P.CHARACTER_MAXIMUM_LENGTH as varchar) + ')' END AS DataType " +
            ", '' AS Description, Ordinal_Position " +
            "FROM INFORMATION_SCHEMA.PARAMETERS P " +
            "WHERE P.SPECIFIC_NAME = '" + Name + "' and P.PARAMETER_NAME <> '' " +
            "ORDER BY Ordinal_Position ";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                string Description = DiversityWorkbench.Forms.FormFunctions.getDescription(Type, Name, R[0].ToString(), 1);
                if (Description.Length > 0)
                    R["Description"] = Description;
            }
            return dt;
        }

        public static System.Data.DataTable Columns(string Name)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "SELECT C.COLUMN_NAME AS [Name], " +
                "DATA_TYPE + CASE WHEN C.CHARACTER_MAXIMUM_LENGTH IS NULL THEN '' ELSE ' (' + case when C.CHARACTER_MAXIMUM_LENGTH = -1 then 'MAX' else CAST(C.CHARACTER_MAXIMUM_LENGTH as varchar) end + ')' END AS DataType, " +
                "'' AS Description, Ordinal_Position " +
                "FROM INFORMATION_SCHEMA.ROUTINE_COLUMNS C " +
                "WHERE C.TABLE_NAME = '" + Name + "' " +
            "ORDER BY Ordinal_Position ";
            string Message = "";
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
            foreach (System.Data.DataRow R in dt.Rows)
            {
                string Description = DiversityWorkbench.Forms.FormFunctions.getDescription(DiversityWorkbench.Forms.FormFunctions.DatabaseObjectType.FUNCTION, Name, R[0].ToString());
                if (Description.Length > 0)
                    R["Description"] = Description;
            }
            return dt;
        }

        public static System.Collections.Generic.Dictionary<string, string> DependentOn(string Name)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string SQL = "SELECT DISTINCT d.referenced_entity_name, o.type_desc " +
                "FROM sys.sql_expression_dependencies d inner join sys.objects o on d.referenced_id = o.object_id " +
                "WHERE referencing_id = OBJECT_ID(N'dbo." + Name + "');";
            string Message = "";
            System.Collections.Generic.Dictionary<string, string> list = new Dictionary<string, string>();
            using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
            {
                con.Open();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, con);
                con.Close();
            }
            foreach (System.Data.DataRow R in dt.Rows)
            {
                list.Add(R[0].ToString(), R[1].ToString());
            }
            return list;
        }

    }
}
