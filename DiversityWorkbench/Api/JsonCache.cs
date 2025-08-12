using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Api
{
    public class JsonCache
    {
        public static bool UpdateContent(string TableName, string KeyColumnName, iUpdate iUpdate, int? ID = null, int? ProjectID = null, string ProjectTableName = "", string ProjectKeyColumnName = "")
        {
            bool OK = true;
            string SQL = "SELECT T.[" + KeyColumnName + "] AS ID FROM [" + TableName + "] AS T";
            if (ProjectID != null && ProjectTableName.Length > 0)
            {
                string ProjectIDcolumn = "ProjectID";
                if (ProjectKeyColumnName.Length > 0)
                    ProjectIDcolumn = ProjectKeyColumnName;
                SQL += " INNER JOIN  [" + ProjectTableName + "] AS P ON P.[" + ProjectIDcolumn + "] = " + ProjectID.ToString() + " AND P.[" + KeyColumnName + "] = T.[" + KeyColumnName + "]";
            }
            if (ID is null)
            {
                System.Data.DataTable dataTable = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL);
                iUpdate.setMax(dataTable.Rows.Count);
                int i = 0;
                foreach (System.Data.DataRow R in dataTable.Rows)
                {
                    SQL = "EXECUTE [dbo].[procFillJsonCache] " + R[0].ToString();
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    iUpdate.setCurrent(i++);
                }
            }
            else
            {
                SQL = "EXECUTE [dbo].[procFillJsonCache] " + ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            }
            return OK;
        }

        public static bool UpdateContent(string TableName, string KeyColumnName, int ID)// = null)
        {
            bool OK = true;
            string SQL = "EXECUTE [dbo].[procFillJsonCache] " + ID.ToString();
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            return OK;
        }

        public static System.Data.DataTable DtJson(string DatabasePrefix = "", string ConnectionString = "", string Restriction = "")
        {
            string Prefix = "";
            if (DatabasePrefix.Length > 0) Prefix = DatabasePrefix + ".dbo.";
            //string SQL = "SELECT * FROM " + Prefix + "JsonCache_FullAccess " + Restriction;
            string SQL = "SELECT * FROM " + Prefix + "JsonCache " + Restriction;
            System.Data.DataTable dataTable = DiversityWorkbench.Forms.FormFunctions.DataTable(SQL, ConnectionString);
            return dataTable;
        }
        /// <summary>
        /// The list containing the projects available for a user
        /// </summary>
        /// <param name="Source">The name of the source in the database</param>
        /// <returns>A Datatable containing the data</returns>
        public static System.Data.DataTable DtProject(string Source = "ProjectProxy")
        {
            System.Data.DataTable dataTable = DiversityWorkbench.Forms.FormFunctions.DataTable("SELECT NULL AS Project, NULL AS ProjectID UNION SELECT P.Project, P.ProjectID FROM " + Source + " P ORDER BY Project");
            return dataTable;
        }


        public static int TableContentCount(string TableName)
        {
            int i = 0;
            int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar("SELECT COUNT(*) FROM [" + TableName + "]"), out i);
            return i;
        }

        public static bool DoesExist()
        {
            string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.VIEWS V WHERE V.TABLE_NAME = 'JsonCache'"; // _FullAccess'"; // "select count(*) from INFORMATION_SCHEMA.TABLES t where t.TABLE_NAME = 'JsonCache'";
            int i = 0;
            int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i);
            return i == 1;
        }

        public static void ShowJson(string JSON)
        {
            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Content" + System.DateTime.Now.ToString("_yyyyMMddHHmmss") + ".json");
                //if (!file.Exists)
                //    file.Create();
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(file.FullName))
                {
                    sw.Write(JSON);
                    sw.Close();
                    sw.Dispose();
                }
                DiversityWorkbench.Forms.FormWebBrowser webBrowser = new DiversityWorkbench.Forms.FormWebBrowser(file.FullName);
                webBrowser.ShowDialog();
                file.Delete();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public static string Json(int ID)
        {
            string json = "";
            //string SQL = "SELECT Data FROM JsonCache_FullAccess T WHERE T.ID = " + ID.ToString();
            string SQL = "SELECT Data FROM JsonCache T WHERE T.ID = " + ID.ToString();
            json = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString();
            return json;
        }

        public static string JsonPublic(int ID)
        {
            string json = "";
            if (ContainsColumnPublic())
            {
                string SQL = "SELECT [Public] FROM JsonCache_Public T WHERE T.ID = " + ID.ToString();
                json = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).ToString();
            }
            return json;
        }

        public static bool ContainsColumnPublic()
        {
            string SQL = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = 'JsonCache' AND C.COLUMN_NAME = 'Public'";
            int i;
            if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(@SQL), out i) && i > 0) { return true; }
            else return false;
        }

    }
}
