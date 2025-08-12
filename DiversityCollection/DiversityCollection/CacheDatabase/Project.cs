using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.CacheDatabase
{
    public class Project : DiversityWorkbench.PostgreSQL.Schema
    {

        #region Parameter

        private string _TransferDirectory;

        #endregion

        #region Construction

        public Project(string Name, DiversityWorkbench.PostgreSQL.Database Database)
        {
            this._Database = Database;
            this._Name = Name;
        }

        #endregion

        #region Interface
        public string SchemaName
        {
            get
            {
                if (this._Name.StartsWith("Project_"))
                    return this._Name;
                return "Project_" + this._Name;
            }
        }

        public void SetSchemaName(string ProjectName)
        {
            if (this._Name == "" || this._Name == null)
            {
                this._Name = ProjectName;
            }
        }

        private int? _ProjectID;

        public int? ProjectID
        {
            get
            {
                if (_ProjectID != null)
                    return _ProjectID;
                string SQL = "select \"" + this.SchemaName + "\".ProjectID()";
                int ID;
                string Message = "";
                string Result = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString(), ref Message);
                if (int.TryParse(Result, out ID))
                {
                    _ProjectID = ID;
                    return _ProjectID;
                }
                else return null;
            }
            set { _ProjectID = value; }
        }

        public void ClearProject()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Table> KV in this.Tables)
            {
                KV.Value.ClearTable();
            }
        }


        #endregion

        #region Packages

        private System.Data.DataTable _dtPackage;
        public System.Data.DataTable dtPackage()
        {
            if (this._dtPackage == null)
            {
                this._dtPackage = new System.Data.DataTable();
                string SQL = "SELECT \"Package\", \"Version\", \"Description\" FROM \"" + this.SchemaName + "\".\"Package\" ORDER BY \"Package\"";
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref this._dtPackage, ref Message);
            }
            return this._dtPackage;
        }
        public void ResetDtPackages()
        {
            this._dtPackage = null;
        }

        public DiversityCollection.CacheDatabase.Package GetPackage(string PackageName)
        {
            if (this.Packages.ContainsKey(PackageName))
                return this.Packages[PackageName];
            else return null;
        }

        public void EstablishPackage(Package.Pack Package, string Description)
        {
            string SQL = "INSERT INTO \"" + this.SchemaName + "\".\"Package\"(\"Package\", \"Version\", \"Description\") " + 
                "VALUES ('" + Package.ToString() + "', 1, '" + Description + "'); ";
            if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))
            {
                this.ResetDtPackages();
            }
            else
            {
                if (System.Windows.Forms.MessageBox.Show("Establishing of package " + Package + " failed", "Remove old entry?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    SQL = "DELETE FROM \"" + this.SchemaName + "\".\"Package\" WHERE \"Package\" = '" + Package.ToString() + "'; ";
                    if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))
                        this.ResetDtPackages();
                }
            }
        }

        private bool InsertPackageTarget(Package.Pack Package)
        {
            bool OK = true;
            string SQL = "SELECT TargetID FROM Target WHERE ";
            SQL = "INSERT INTO [dbo].[ProjectTargetPackage] ([ProjectID],[TargetID],[Package]) " +
                "VALUES (" + this._ProjectID.ToString() + ",<TargetID, int,>, '" + Package + "') ";
            return OK;
        }

        private System.Collections.Generic.Dictionary<string, DiversityCollection.CacheDatabase.Package> _Packages;

        public System.Collections.Generic.Dictionary<string, DiversityCollection.CacheDatabase.Package> Packages
        {
            get 
            {
                if (_Packages == null)
                {
                    _Packages = new Dictionary<string, Package>();
                    foreach (System.Data.DataRow R in this.dtPackage().Rows)
                    {
                        DiversityCollection.CacheDatabase.Package P = new Package(R[0].ToString(), this);
                        P.setDescription(R[2].ToString());
                        _Packages.Add(R[0].ToString(), P);
                    }
                }
                return _Packages; 
            }
            set { _Packages = value; }
        }
        
        #endregion

        #region static
        
        private static System.Collections.Generic.Dictionary<int, DiversityCollection.CacheDatabase.Project> _Projects;

        public static System.Collections.Generic.Dictionary<int, DiversityCollection.CacheDatabase.Project> Projects
        {
            get
            {
                if (DiversityCollection.CacheDatabase.Project._Projects == null)
                {
                    DiversityCollection.CacheDatabase.Project._Projects = new Dictionary<int, Project>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Schema> KV in DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Schemas)
                    {
                        string Project = KV.Value.Name;
                        if (Project.StartsWith("Project_"))
                            Project = Project.Substring("Project_".Length);
                        DiversityCollection.CacheDatabase.Project P = new Project(Project, KV.Value.Database);
                        if (P.ProjectID != null)
                            DiversityCollection.CacheDatabase.Project._Projects.Add((int)P.ProjectID, P);
                    }
                }
                else if (DiversityCollection.CacheDatabase.Project._Projects.Count == 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Schema> KV in DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Schemas)
                    {
                        DiversityCollection.CacheDatabase.Project P = new Project(KV.Value.Name, KV.Value.Database);
                        if (P.ProjectID != null)
                            DiversityCollection.CacheDatabase.Project._Projects.Add((int)P.ProjectID, P);
                    }
                }
                return DiversityCollection.CacheDatabase.Project._Projects;
            }
            //set { Project._Projects = value; }
        }

        public static System.Data.DataTable DtProjects()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ProjectID", typeof(int));
            dt.Columns.Add("Project", typeof(string));
            foreach (System.Collections.Generic.KeyValuePair<int, DiversityCollection.CacheDatabase.Project> KV in DiversityCollection.CacheDatabase.Project.Projects)
            {
                System.Data.DataRow R = dt.NewRow();
                R[0] = KV.Key;
                R[1] = KV.Value._Name;
                dt.Rows.Add(R);
            }
            return dt;
        }

        public static void ResetProjects()
        {
            DiversityCollection.CacheDatabase.Project._Projects = null;
        }

        public static DiversityWorkbench.PostgreSQL.Schema GetProject(int ProjectID)
        {
            if (DiversityCollection.CacheDatabase.Project.Projects.ContainsKey(ProjectID))
                return DiversityCollection.CacheDatabase.Project.Projects[ProjectID];
            else return null;
            //DiversityWorkbench.PostgreSQL.Schema S = new DiversityWorkbench.PostgreSQL.Schema("", DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase());
            //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Schema> KV in DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Schemas)
            //{
            //    DiversityCollection.CacheDatabase.Project P = (DiversityCollection.CacheDatabase.Project)KV.Value;
            //    if (P.ProjectID == ProjectID)
            //        return P;
            //}
            //return S;
        }

        public static DiversityCollection.CacheDatabase.Project GetProject(string ProjectName)
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<int, DiversityCollection.CacheDatabase.Project> P in DiversityCollection.CacheDatabase.Project.Projects)
                {
                    if (P.Value._Name == ProjectName)
                        return P.Value;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return null;
        }

        #endregion

    }
}
