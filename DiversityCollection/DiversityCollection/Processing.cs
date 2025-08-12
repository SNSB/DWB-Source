using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class Processing : HierarchicalEntity//, IHierarchicalEntity
    {
        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryConditionProject;

        #endregion

        #region Construction

        public Processing(
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
            HelpProvider, ToolTip, ref BindingSource, DiversityCollection.LookupTable.DtProcessing, DiversityCollection.LookupTable.DtProcessingHierarchy)
        {
            this._sqlItemFieldList = " ProcessingID, ProcessingParentID, DisplayText, Description, Notes, ProcessingURI, OnlyHierarchy ";
            this._SpecimenTable = "CollectionSpecimenProcessing";
            this._MainTable = "Processing";
        }
        
        #endregion

        #region Functions and properties

        protected override string SqlSpecimenCount(int ID)
        {
            return "SELECT COUNT(*) FROM CollectionSpecimenProcessing WHERE ProcessingID = " + ID.ToString();
        }
        
        #endregion

        #region Datahandling of dependent tables

        public override void saveDependentTables()
        {
            string Message = "";
            bool OK = true;
            this.FormFunctions.updateTable(this._DataSet, "ProcessingMaterialCategory", this._SqlDataAdapterDepend, this._BindingSource);
            this.FormFunctions.updateTable(this._DataSet, "ProjectProcessing", this._SqlDataAdapterDepend_2, this._BindingSource);
            OK = this.FormFunctions.updateTable(this._DataSet, "MethodForProcessing", this._SqlDataAdapterDepend_4, this._BindingSource, ref Message);
            if (!OK && Message.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Removing of data in table MethodForProcessing failed: " + Message);
            }
        }

        public override void fillDependentTables(int ID)
        {
            string SQL = "SELECT ProcessingID, MaterialCategory FROM ProcessingMaterialCategory WHERE ProcessingID = " + ID.ToString();
            if (this._SqlDataAdapterDepend == null)
                this._SqlDataAdapterDepend = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend, SQL, this._DataSet.Tables["ProcessingMaterialCategory"]);
            this._DataSet.Tables["ProcessingMaterialCategory"].Clear();
            this._SqlDataAdapterDepend.Fill(this._DataSet.Tables["ProcessingMaterialCategory"]);

            SQL = "SELECT ProcessingID, ProjectID FROM ProjectProcessing WHERE ProcessingID = " + ID.ToString();
            if (this._SqlDataAdapterDepend_2 == null)
                this._SqlDataAdapterDepend_2 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend_2.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_2, SQL, this._DataSet.Tables["ProjectProcessing"]);
            this._DataSet.Tables["ProjectProcessing"].Clear();
            this._SqlDataAdapterDepend_2.Fill(this._DataSet.Tables["ProjectProcessing"]);

            SQL = "SELECT A.ProcessingID, A.ProjectID, P.Project FROM ProjectProcessing A, ProjectProxy P WHERE A.ProjectID = P.ProjectID AND ProcessingID = " + ID.ToString();
            if (this._SqlDataAdapterDepend_3 == null)
                this._SqlDataAdapterDepend_3 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend_3.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_3, SQL, this._DataSet.Tables["ProjectProcessingList"]);
            this._DataSet.Tables["ProjectProcessingList"].Clear();
            this._SqlDataAdapterDepend_3.Fill(this._DataSet.Tables["ProjectProcessingList"]);

            // Methods
            SQL = "SELECT ProcessingID, MethodID FROM MethodForProcessing WHERE ProcessingID = " + ID.ToString();
            if (this._SqlDataAdapterDepend_4 == null)
                this._SqlDataAdapterDepend_4 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend_4.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_4, SQL, this._DataSet.Tables["MethodForProcessing"]);
            this._DataSet.Tables["MethodForProcessing"].Clear();
            this._SqlDataAdapterDepend_4.Fill(this._DataSet.Tables["MethodForProcessing"]);

            SQL = "SELECT A.ProcessingID, A.MethodID, M.DisplayText FROM MethodForProcessing A, Method M WHERE A.MethodID = M.MethodID AND ProcessingID = " + ID.ToString();
            if (this._SqlDataAdapterDepend_5 == null)
                this._SqlDataAdapterDepend_5 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend_5.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_5, SQL, this._DataSet.Tables["MethodForProcessingList"]);
            this._DataSet.Tables["MethodForProcessingList"].Clear();
            this._SqlDataAdapterDepend_5.Fill(this._DataSet.Tables["MethodForProcessingList"]);

        }

        #endregion

        #region Interface

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
        {
            get
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();
                string Description = "";
                string SQL = "";

                #region PROJECT

                System.Data.DataTable dtProject = new System.Data.DataTable();
                SQL = "SELECT ProjectID AS [Value], Project AS Display " +
                    "FROM ProjectListNotReadOnly " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { a.Fill(dtProject); }
                    catch { }
                }
                if (dtProject.Columns.Count > 0)
                {
                    Description = DiversityWorkbench.Functions.ColumnDescription("ProjectProxy", "Project");
                    this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "ProjectProcessing", "ProcessingID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
                    QueryConditions.Add(this._QueryConditionProject);
                }

                #endregion

                #region Processing

                Description = this.FormFunctions.ColumnDescription("Processing", "DisplayText");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "Processing", "ProcessingID", "DisplayText", "Processing", "Name", "Name", Description);
                QueryConditions.Add(q0);

                Description = this.FormFunctions.ColumnDescription("Processing", "Description");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "Processing", "ProcessingID", "Description", "Processing", "Description", "Description", Description);
                QueryConditions.Add(q1);

                Description = this.FormFunctions.ColumnDescription("Processing", "Notes");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "Processing", "ProcessingID", "Notes", "Processing", "Notes", "Notes", Description);
                QueryConditions.Add(q2);

                Description = this.FormFunctions.ColumnDescription("Processing", "ProcessingURI");
                DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "Processing", "ProcessingID", "ProcessingURI", "Processing", "URI", "URI", Description);
                QueryConditions.Add(q3);
                
                #endregion  
              
                #region Method

                System.Data.DataTable dtMethod = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display " +
                    "UNION " +
                    "SELECT MethodID AS [Value], DisplayText AS Display " +
                    "FROM Method " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter aMethod = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aMethod.Fill(dtMethod); }
                    catch (System.Exception ex) { }
                }
                if (dtMethod.Columns.Count > 0)
                {
                    Description = DiversityWorkbench.Functions.TableDescription("MethodForProcessing");
                    DiversityWorkbench.QueryCondition QueryConditionMethod = new DiversityWorkbench.QueryCondition(true, "MethodForProcessing", "ProcessingID", "MethodID", "Method", "Method", "Method", Description, dtMethod, true);
                    QueryConditions.Add(QueryConditionMethod);
                }

                #endregion

                return QueryConditions;
            }
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            get
            {
                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
                QueryDisplayColumns[0].DisplayText = "Processing";
                QueryDisplayColumns[0].DisplayColumn = "DisplayText";
                QueryDisplayColumns[0].OrderColumn = "DisplayText";
                QueryDisplayColumns[0].IdentityColumn = "ProcessingID";
                QueryDisplayColumns[0].TableName = "Processing";
                return QueryDisplayColumns;
            }
        }

        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> ProcessingForMaterialCategory(int ProjectID)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> Dict = new Dictionary<string, List<int>>();
            try
            {
                string SQL = "SELECT L.ProcessingID, M.MaterialCategory " +
                    "FROM dbo.ProcessingProjectList(" + ProjectID.ToString() + ") AS L INNER JOIN " +
                    "ProcessingMaterialCategory AS M ON L.ProcessingID = M.ProcessingID";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string MaterialCategory = R[1].ToString();
                    int ProcessingID = int.Parse(R[0].ToString());
                    if (!Dict.ContainsKey(MaterialCategory))
                    {
                        System.Collections.Generic.List<int> L = new List<int>();
                        L.Add(ProcessingID);
                        Dict.Add(MaterialCategory, L);
                    }
                    else
                    {
                        Dict[MaterialCategory].Add(ProcessingID);
                    }
                }

            }
            catch (System.Exception ex)
            {
            }
            return Dict;
        }



        #endregion
    }
}
