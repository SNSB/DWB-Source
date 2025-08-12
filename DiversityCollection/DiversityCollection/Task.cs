using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection
{
    class Task : HierarchicalEntity
    {

        #region Construction

        public Task(
            ref System.Data.DataSet Dataset,
            System.Data.DataTable DataTable,
            ref System.Windows.Forms.TreeView TreeView,
            System.Windows.Forms.Form Form,
            DiversityWorkbench.UserControls.UserControlQueryList UserControlQueryList,
            System.Windows.Forms.SplitContainer SplitContainerMain,
            System.Windows.Forms.SplitContainer SplitContainerData,
            System.Windows.Forms.HelpProvider HelpProvider,
            System.Windows.Forms.ToolTip ToolTip,
            ref System.Windows.Forms.BindingSource BindingSource)
            : base(ref Dataset, DataTable, ref TreeView, Form, UserControlQueryList, SplitContainerMain,
            SplitContainerData, null, null, 
            HelpProvider, ToolTip, ref BindingSource, DiversityCollection.LookupTable.DtTaskHierarchy, DiversityCollection.LookupTable.DtTaskHierarchy)
        {
            this._sqlItemFieldList = " TaskID, TaskParentID, DisplayText, Description, Notes, TaskURI, Type, ModuleTitle, ModuleType, SpecimenPartType, ResultType, DateType, DateBeginType, DateEndType, NumberType, BoolType, DescriptionType, NotesType, UriType ";
            this._MainTable = "Task";
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

                #region Task

                Description = this.FormFunctions.ColumnDescription("Task", "DisplayText");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "Task", "TaskID", "DisplayText", "Task", "DisplayText", "DisplayText", Description);
                QueryConditions.Add(q0);

                Description = this.FormFunctions.ColumnDescription("Task", "Description");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "Task", "TaskID", "Description", "Task", "Description", "Description", Description);
                QueryConditions.Add(q1);

                Description = this.FormFunctions.ColumnDescription("Task", "Notes");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "Task", "TaskID", "Notes", "Task", "Notes", "Notes", Description);
                QueryConditions.Add(q2);

                Description = this.FormFunctions.ColumnDescription("Task", "TaskURI");
                DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "Task", "TaskID", "TaskURI", "Task", "URI", "URI", Description);
                QueryConditions.Add(q3);

                Description = this.FormFunctions.ColumnDescription("Task", "TaskID");
                DiversityWorkbench.QueryCondition q4 = new DiversityWorkbench.QueryCondition(true, "Task", "TaskID", "TaskID", "Task", "ID", "ID", Description);
                QueryConditions.Add(q4);

                #endregion

                return QueryConditions;
            }
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            get
            {
                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[5];
                QueryDisplayColumns[0].DisplayText = "Task";
                QueryDisplayColumns[0].DisplayColumn = "DisplayText";
                QueryDisplayColumns[0].OrderColumn = "DisplayText";
                QueryDisplayColumns[0].IdentityColumn = "TaskID";
                QueryDisplayColumns[0].TableName = "TaskHierarchyAll()";

                QueryDisplayColumns[1].DisplayText = "Hierarchy";
                QueryDisplayColumns[1].DisplayColumn = "HierarchyDisplayText";
                QueryDisplayColumns[1].OrderColumn = "HierarchyDisplayText";
                QueryDisplayColumns[1].IdentityColumn = "TaskID";
                QueryDisplayColumns[1].TableName = "TaskHierarchyAll()";

                QueryDisplayColumns[2].DisplayText = "Type";
                QueryDisplayColumns[2].DisplayColumn = "Type";
                QueryDisplayColumns[2].OrderColumn = "Type";
                QueryDisplayColumns[2].IdentityColumn = "TaskID";
                QueryDisplayColumns[2].TableName = "TaskHierarchyAll()";

                QueryDisplayColumns[3].DisplayText = "Description";
                QueryDisplayColumns[3].DisplayColumn = "Description";
                QueryDisplayColumns[3].OrderColumn = "Description";
                QueryDisplayColumns[3].IdentityColumn = "TaskID";
                QueryDisplayColumns[3].TableName = "TaskHierarchyAll()";

                QueryDisplayColumns[4].DisplayText = "Module";
                QueryDisplayColumns[4].DisplayColumn = "ModuleType";
                QueryDisplayColumns[4].OrderColumn = "ModuleType";
                QueryDisplayColumns[4].IdentityColumn = "TaskID";
                QueryDisplayColumns[4].TableName = "TaskHierarchyAll()";

                return QueryDisplayColumns;
            }
        }

        public override void ItemChanged()
        {
            if(this._formTask != null)
            {
                this._formTask.SetFormAccordingToItem();
                //if (this._DataSet.Tables["TaskResult"].Rows.Count == 0)
                //{
                //}
            }
        }

        private DiversityCollection.Forms.FormTask _formTask;
        public void setFormTask(DiversityCollection.Forms.FormTask f)
        {
            this._formTask = f;
        }

        public override void markHierarchyNodes()
        {
            //base.markHierarchyNodes();
            if (this._TreeView.ImageList == null)
                this._TreeView.ImageList = DiversityCollection.Specimen.ImageList;//.Task.ImageList();

            foreach (System.Windows.Forms.TreeNode N in this._TreeView.Nodes)
            {
                this.markHierarchyNode(N);
                this.markHierachyChildNodes(N);
            }
        }

        private void markHierachyChildNodes(System.Windows.Forms.TreeNode N)
        {
            this.markHierarchyNode(N);
            foreach (System.Windows.Forms.TreeNode NC in N.Nodes)
                this.markHierachyChildNodes(NC);
        }

        private void markHierarchyNode(System.Windows.Forms.TreeNode N)
        {
            if (N.Tag != null && (N.Tag.GetType().BaseType == typeof(System.Data.DataRow) || N.Tag.GetType() == typeof(System.Data.DataRow)))
            {
                System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                N.ImageIndex = DiversityCollection.Specimen.ImageIndex(R, false);
                //int TaskID = 0;
                //string sTaskID = R["TaskID"].ToString();
                //string Type = "Task";
                //if (int.TryParse(sTaskID, out TaskID))
                //{
                //    Type = DiversityCollection.LookupTable.TaskModuleType(TaskID);
                //    if (Type.Length == 0 || Type == "-")
                //    {
                //        Type = DiversityCollection.LookupTable.TaskDateType(TaskID);
                //        if (Type.Length == 0 || Type == "-")
                //            Type = DiversityCollection.LookupTable.TaskResultType(TaskID);
                //    }
                //}
                //else { }
                //N.ImageIndex = DiversityCollection.Specimen.TaskTypeImage(Type, false);//.Task.ImageIndex(Type);
                N.SelectedImageIndex = N.ImageIndex;
            }
            else { }
        }

        #region Dependet data

        public override void saveDependentTables()
        {
            try
            {
                string Message = "";
                bool OK = true;
                foreach (System.Data.DataRow R in this._DataSet.Tables["TaskResult"].Rows)
                {
                    if (R.RowState != System.Data.DataRowState.Deleted)
                    {
                        R.BeginEdit();
                        R.EndEdit();
                    }
                }
                foreach (System.Data.DataRow R in this._DataSet.Tables["TaskModule"].Rows)
                {
                    if (R.RowState != System.Data.DataRowState.Deleted)
                    {
                        R.BeginEdit();
                        R.EndEdit();
                    }
                }
                this.FormFunctions.updateTable(this._DataSet, "TaskResult", this._SqlDataAdapterDepend, this._BindingSource);
                this.FormFunctions.updateTable(this._DataSet, "TaskModule", this._SqlDataAdapterDepend_2, this._BindingSource);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public override void fillDependentTables(int ID)
        {
            try
            {
                // TaskResult
                string SQL = "SELECT TaskID, Result, URI, Description, Notes " +
                    "FROM TaskResult " +
                    "WHERE TaskID = " + ID.ToString() +
                    " ORDER BY Result";
                if (this._SqlDataAdapterDepend == null)
                    this._SqlDataAdapterDepend = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                else
                    this._SqlDataAdapterDepend.SelectCommand.CommandText = SQL;
                FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend, SQL, this._DataSet.Tables["TaskResult"]);
                this._DataSet.Tables["TaskResult"].Clear();
                this._SqlDataAdapterDepend.Fill(this._DataSet.Tables["TaskResult"]);

                SQL = "SELECT TaskID, DisplayText, URI, Description, Notes " +
                    "FROM TaskModule " +
                    "WHERE TaskID = " + ID.ToString() +
                    " ORDER BY DisplayText";
                if (this._SqlDataAdapterDepend_2 == null)
                    this._SqlDataAdapterDepend_2 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                else
                    this._SqlDataAdapterDepend_2.SelectCommand.CommandText = SQL;
                FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_2, SQL, this._DataSet.Tables["TaskModule"]);
                this._DataSet.Tables["TaskModule"].Clear();
                this._SqlDataAdapterDepend_2.Fill(this._DataSet.Tables["TaskModule"]);

                //this.setXmlTree();
                this.ItemChanged();
            }
            catch (System.Exception ex) { }
        }

        public override bool deleteDependentData(int ID)
        {
            try
            {
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                con.Open();
                Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand("", con);

                // Testing existence of links
                int i = 0;
                C.CommandText = "SELECT COUNT(*) FROM CollectionTask M WHERE TaskID = " + ID.ToString();
                string Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
                if (Result != "0")
                {
                    if (System.Windows.Forms.MessageBox.Show("This task is still used for " + Result + " collection task(s).\r\nDo you really want to delete it?", "Delete?", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error) == System.Windows.Forms.DialogResult.No)
                        return false;
                }

                // CollectionTask
                C.CommandText = "DELETE M FROM CollectionTask M WHERE TaskID = " + ID.ToString();
                C.ExecuteNonQuery();
                // TaskResult
                C.CommandText = "DELETE M FROM TaskResult M WHERE TaskID = " + ID.ToString();
                C.ExecuteNonQuery();

                con.Close();
                con.Dispose();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Deleting dependent data failed: " + ex.Message);
                return false;
            }
            return true;
        }

        #endregion

        #endregion

        #region static 

        public enum TaskModuleType { None, Agent, Collection, Gazetteer, Project, Reference, SamplingPlot, ScientificTerm, TaxonName }
        public enum TaskResultType { None, List, Number, Text }
        public enum TaskDateType { None, DateAndTime, DateAndTimeFromTo, Time, TimeFromTo, Date, DateFromTo }

        public static TaskModuleType TypeOfTaskModule(string Type)
        {
            TaskModuleType T = TaskModuleType.None;
            switch(Type)
            {
                case "DiversityAgents":
                    T = TaskModuleType.Agent;
                    break;
                case "DiversityCollection":
                    T = TaskModuleType.Collection;
                    break;
                case "DiversityGazetteer":
                    T = TaskModuleType.Gazetteer;
                    break;
                case "DiversityProjects":
                    T = TaskModuleType.Project;
                    break;
                case "DiversityReferences":
                    T = TaskModuleType.Reference;
                    break;
                case "DiversitySamplingPlots":
                    T = TaskModuleType.SamplingPlot;
                    break;
                case "DiversityScientificTerms":
                    T = TaskModuleType.ScientificTerm;
                    break;
                case "DiversityTaxonNames":
                    T = TaskModuleType.TaxonName;
                    break;
            }
            return T;
        }

        public static TaskResultType TypeOfTaskResult(string Type)
        {
            TaskResultType T = TaskResultType.None;
            switch (Type)
            {
                case "Number":
                    T = TaskResultType.Number;
                    break;
                case "Text":
                    T = TaskResultType.Text;
                    break;
                case "List":
                    T = TaskResultType.List;
                    break;
            }
            return T;
        }

        public static TaskDateType TypeOfTaskDate(string Type)
        {
            TaskDateType T = TaskDateType.None;
            switch (Type)
            {
                case "Date & Time":
                    T = TaskDateType.DateAndTime;
                    break;
                case "Date & Time from to":
                    T = TaskDateType.DateAndTimeFromTo;
                    break;
                case "Time":
                    T = TaskDateType.Time;
                    break;
                case "Time from to":
                    T = TaskDateType.TimeFromTo;
                    break;
                case "Date":
                    T = TaskDateType.Date;
                    break;
                case "Date from to":
                    T = TaskDateType.DateFromTo;
                    break;
            }
            return T;
        }


        public static bool IsModuleRelated(string Type)
        {
            //bool IsModule = false;
            TaskModuleType taskType = TypeOfTaskModule(Type);
            if (taskType != TaskModuleType.None)
                return true;
            else
                return false;

        }

        public static System.Collections.Generic.Dictionary<string, string> DefaultColumnsForNewTasks(string Type)
        {
            System.Collections.Generic.Dictionary<string, string> DefaultColums = new Dictionary<string, string>();
            switch(Type)
            {
                case "Agent":
                    DefaultColums.Add("DisplayText", "Responsible");
                    DefaultColums.Add("ModuleType", "DiversityAgents");
                    break;
                case "Collection":
                    DefaultColums.Add("DisplayText", "Remote specimen");
                    DefaultColums.Add("ModuleType", "DiversityCollection");
                    break;
                case "Gazetteer":
                    DefaultColums.Add("DisplayText", "Location");
                    DefaultColums.Add("ModuleType", "DiversityGazetteer");
                    break;
                case "Part":
                    DefaultColums.Add("DisplayText", "Part");
                    DefaultColums.Add("SpecimenPartType", "Part");
                    break;
                case "Sampling plot":
                    DefaultColums.Add("DisplayText", "Plot");
                    DefaultColums.Add("ModuleType", "DiversitySamplingPlots");
                    break;
                case "Scientific term":
                    DefaultColums.Add("DisplayText", "Term");
                    DefaultColums.Add("ModuleType", "DiversityScientificTerms");
                    break;
                case "Taxon name":
                    DefaultColums.Add("DisplayText", "Taxon");
                    DefaultColums.Add("ModuleType", "DiversityTaxonNames");
                    break;
                case "Analysis":
                case "Cleaning":
                case "Damage":
                case "Description":
                case "DiversityWorkbench":
                case "Document":
                case "Evaluation":
                case "Freezing":
                case "Gas":
                case "Inspection":
                case "Search":
                case "Payment":
                case "Poison":
                case "Problem":
                case "Processing":
                case "Project":
                case "Query":
                case "Radiation":
                case "Repair":
                case "Legislation":
                case "Monitoring":
                case "Task":
                case "Trap":
                default:
                    break;
            }
            return DefaultColums;
        }

        #endregion

    }
}
