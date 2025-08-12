using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class Analysis : HierarchicalEntity//, IHierarchicalEntity
    {
        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryConditionProject;

        #endregion

        #region Construction

        public Analysis(
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
            HelpProvider, ToolTip, ref BindingSource, DiversityCollection.LookupTable.DtAnalysis, DiversityCollection.LookupTable.DtAnalysisHierarchy)
        {
            this._SpecimenTable = "IdentificationUnitAnalysis";
            this._sqlItemFieldList = " AnalysisID, AnalysisParentID, DisplayText, Description, MeasurementUnit, Notes, AnalysisURI, OnlyHierarchy ";
            this._MainTable = "Analysis";
        }
        
        #endregion

        #region Functions and properties

        protected override string SqlSpecimenCount(int ID)
        {
            return "SELECT COUNT(*) FROM IdentificationUnitAnalysis WHERE AnalysisID = " + ID.ToString();
        }

        public override bool deleteDependentData(int ID) 
        {
            bool OK = true;
            string SQL = "Delete M FROM MethodForAnalysis M WHERE M.AnalysisID = " + ID.ToString();
            OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
            if (OK)
            {
                SQL = "Delete M FROM AnalysisTaxonomicGroup M WHERE M.AnalysisID = " + ID.ToString();
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                if (OK)
                {
                    SQL = "Delete M FROM ProjectAnalysis M WHERE M.AnalysisID = " + ID.ToString();
                    OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    if (OK)
                    {
                        SQL = "Delete M FROM AnalysisResult M WHERE M.AnalysisID = " + ID.ToString();
                        OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    }
                }
            }
            return OK; 
        }



        #endregion

        #region Datahandling of dependent tables

        //protected virtual void setDependentTable(int ID) 
        //{
        //    this.saveDependentTables();
        //}

        public override void saveDependentTables() 
        {
            this.FormFunctions.updateTable(this._DataSet, "AnalysisTaxonomicGroup", this._SqlDataAdapterDepend, this._BindingSource);
            this.FormFunctions.updateTable(this._DataSet, "ProjectAnalysis", this._SqlDataAdapterDepend_2, this._BindingSource);
            this.FormFunctions.updateTable(this._DataSet, "AnalysisResult", this._SqlDataAdapterDepend_4, this._BindingSource);
            this.FormFunctions.updateTable(this._DataSet, "MethodForAnalysis", this._SqlDataAdapterDepend_5, this._BindingSource);
        }

        public override void fillDependentTables(int ID) 
        {
            string SQL = "SELECT AnalysisID, TaxonomicGroup FROM AnalysisTaxonomicGroup WHERE AnalysisID = " + ID.ToString();
            if (this._SqlDataAdapterDepend == null)
                this._SqlDataAdapterDepend = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend, SQL, this._DataSet.Tables["AnalysisTaxonomicGroup"]);
            this._DataSet.Tables["AnalysisTaxonomicGroup"].Clear();
            this._SqlDataAdapterDepend.Fill(this._DataSet.Tables["AnalysisTaxonomicGroup"]);

            SQL = "SELECT AnalysisID, ProjectID FROM ProjectAnalysis WHERE AnalysisID = " + ID.ToString();
            if (this._SqlDataAdapterDepend_2 == null)
                this._SqlDataAdapterDepend_2 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend_2.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_2, SQL, this._DataSet.Tables["ProjectAnalysis"]);
            this._DataSet.Tables["ProjectAnalysis"].Clear();
            this._SqlDataAdapterDepend_2.Fill(this._DataSet.Tables["ProjectAnalysis"]);

            SQL = "SELECT A.AnalysisID, A.ProjectID, P.Project FROM ProjectAnalysis A, ProjectProxy P WHERE A.ProjectID = P.ProjectID AND AnalysisID = " + ID.ToString();
            if (this._SqlDataAdapterDepend_3 == null)
                this._SqlDataAdapterDepend_3 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend_3.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_3, SQL, this._DataSet.Tables["ProjectAnalysisList"]);
            this._DataSet.Tables["ProjectAnalysisList"].Clear();
            this._SqlDataAdapterDepend_3.Fill(this._DataSet.Tables["ProjectAnalysisList"]);

            try
            {
                SQL = "SELECT AnalysisID, AnalysisResult, Description, DisplayText, DisplayOrder, Notes " +
                    "FROM AnalysisResult " +
                    "WHERE AnalysisID = " + ID.ToString();
                if (this._SqlDataAdapterDepend_4 == null)
                    this._SqlDataAdapterDepend_4 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                else
                    this._SqlDataAdapterDepend_4.SelectCommand.CommandText = SQL;
                FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_4, SQL, this._DataSet.Tables["AnalysisResult"]);
                this._DataSet.Tables["AnalysisResult"].Clear();
                this._SqlDataAdapterDepend_4.Fill(this._DataSet.Tables["AnalysisResult"]);

            }
            catch { }

            // Methods
            SQL = "SELECT AnalysisID, MethodID FROM MethodForAnalysis WHERE AnalysisID = " + ID.ToString();
            if (this._SqlDataAdapterDepend_5 == null)
                this._SqlDataAdapterDepend_5 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend_5.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_5, SQL, this._DataSet.Tables["MethodForAnalysis"]);
            this._DataSet.Tables["MethodForAnalysis"].Clear();
            this._SqlDataAdapterDepend_5.Fill(this._DataSet.Tables["MethodForAnalysis"]);

            SQL = "SELECT A.AnalysisID, A.MethodID, M.DisplayText FROM MethodForAnalysis A, Method M WHERE A.MethodID = M.MethodID AND AnalysisID = " + ID.ToString();
            if (this._SqlDataAdapterDepend_6 == null)
                this._SqlDataAdapterDepend_6 = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            else
                this._SqlDataAdapterDepend_6.SelectCommand.CommandText = SQL;
            FormFunctions.initSqlAdapter(ref this._SqlDataAdapterDepend_6, SQL, this._DataSet.Tables["MethodForAnalysisList"]);
            this._DataSet.Tables["MethodForAnalysisList"].Clear();
            this._SqlDataAdapterDepend_6.Fill(this._DataSet.Tables["MethodForAnalysisList"]);


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
                    this._QueryConditionProject = new DiversityWorkbench.QueryCondition(true, "ProjectAnalysis", "AnalysisID", "ProjectID", "Project", "Project", "Project", Description, dtProject, true);
                    QueryConditions.Add(this._QueryConditionProject);
                }

                #endregion

                #region Analysis

                Description = this.FormFunctions.ColumnDescription("Analysis", "DisplayText");
                DiversityWorkbench.QueryCondition q0 = new DiversityWorkbench.QueryCondition(true, "Analysis", "AnalysisID", "DisplayText", "Analysis", "Display", "Display text", Description);
                QueryConditions.Add(q0);

                Description = this.FormFunctions.ColumnDescription("Analysis", "AnalysisID");
                DiversityWorkbench.QueryCondition qAnalysisID = new DiversityWorkbench.QueryCondition(true, "Analysis", "AnalysisID", "AnalysisID", "Analysis", "ID", "Analysis ID", Description);
                QueryConditions.Add(qAnalysisID);

                Description = this.FormFunctions.ColumnDescription("Analysis", "Description");
                DiversityWorkbench.QueryCondition q1 = new DiversityWorkbench.QueryCondition(true, "Analysis", "AnalysisID", "Description", "Analysis", "Description", "Description", Description);
                QueryConditions.Add(q1);

                Description = this.FormFunctions.ColumnDescription("Analysis", "MeasurementUnit");
                DiversityWorkbench.QueryCondition qm = new DiversityWorkbench.QueryCondition(true, "Analysis", "AnalysisID", "MeasurementUnit", "Analysis", "Unit", "Measurement unit", Description);
                QueryConditions.Add(qm);

                Description = this.FormFunctions.ColumnDescription("Analysis", "Notes");
                DiversityWorkbench.QueryCondition q2 = new DiversityWorkbench.QueryCondition(true, "Analysis", "AnalysisID", "Notes", "Analysis", "Notes", "Notes", Description);
                QueryConditions.Add(q2);

                Description = this.FormFunctions.ColumnDescription("Analysis", "AnalysisURI");
                DiversityWorkbench.QueryCondition q3 = new DiversityWorkbench.QueryCondition(true, "Analysis", "AnalysisID", "AnalysisURI", "Analysis", "URI", "URI", Description);
                QueryConditions.Add(q3);

                Description = this.FormFunctions.ColumnDescription("AnalysisResult", "AnalysisResult");
                DiversityWorkbench.QueryCondition qAnalysisResult = new DiversityWorkbench.QueryCondition(true, "AnalysisResult", "AnalysisID", "AnalysisResult", "Result", "Result", "Analysis result", Description);
                QueryConditions.Add(qAnalysisResult);

                Description = this.FormFunctions.ColumnDescription("AnalysisResult", "DisplayText");
                DiversityWorkbench.QueryCondition qAnalysisResultDisplayText = new DiversityWorkbench.QueryCondition(true, "AnalysisResult", "AnalysisID", "DisplayText", "Result", "Disp. text", "Display text", Description);
                QueryConditions.Add(qAnalysisResultDisplayText);

                System.Data.DataTable dtTaxonomicGroup = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display " +
                    "UNION " +
                    "SELECT Code AS [Value], DisplayText AS Display " +
                    "FROM CollTaxonomicGroup_Enum " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter aTaxonomicGroup = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aTaxonomicGroup.Fill(dtTaxonomicGroup); }
                    catch { }
                }
                if (dtTaxonomicGroup.Columns.Count > 0)
                {
                    Description = DiversityWorkbench.Functions.TableDescription("AnalysisTaxonomicGroup");
                    DiversityWorkbench.QueryCondition qTaxonomicGroup = new DiversityWorkbench.QueryCondition(true, "AnalysisTaxonomicGroup", "AnalysisID", "TaxonomicGroup", "Taxonomic group", "Tax. group", "Taxonomic group", Description, dtTaxonomicGroup, false);
                    QueryConditions.Add(qTaxonomicGroup);
                }

                System.Data.DataTable dtMethods = new System.Data.DataTable();
                SQL = "SELECT NULL AS [Value], NULL AS Display " + 
                    "UNION " +
                    "SELECT MethodID AS [Value], DisplayText AS Display " +
                    "FROM Method " +
                    "ORDER BY Display";
                Microsoft.Data.SqlClient.SqlDataAdapter aMethod = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                {
                    try { aMethod.Fill(dtMethods); }
                    catch { }
                }
                if (dtMethods.Columns.Count > 0)
                {
                    Description = DiversityWorkbench.Functions.TableDescription("MethodForAnalysis");
                    DiversityWorkbench.QueryCondition qMethodForAnalysis = new DiversityWorkbench.QueryCondition(true, "MethodForAnalysis", "AnalysisID", "MethodID", "Method", "Method", "Method", Description, dtMethods, true);
                    QueryConditions.Add(qMethodForAnalysis);
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
                QueryDisplayColumns[0].DisplayText = "Analysis";
                QueryDisplayColumns[0].DisplayColumn = "DisplayText";
                QueryDisplayColumns[0].OrderColumn = "DisplayText";
                QueryDisplayColumns[0].IdentityColumn = "AnalysisID";
                QueryDisplayColumns[0].TableName = "Analysis";
                return QueryDisplayColumns;
            }
        }

        public static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> AnalysisForTaxonomicGroup(int ProjectID)
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<int>> Dict = new Dictionary<string, List<int>>();
            try
            {
                string SQL = "SELECT P.AnalysisID, T.TaxonomicGroup " +
                    "FROM dbo.AnalysisProjectList(" + ProjectID.ToString() + ") AS P INNER JOIN " +
                    "AnalysisTaxonomicGroup AS T ON P.AnalysisID = T.AnalysisID";
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string TaxonomicGroup = R[1].ToString();
                    int AnalysisID = int.Parse(R[0].ToString());
                    if (!Dict.ContainsKey(TaxonomicGroup))
                    {
                        System.Collections.Generic.List<int> L = new List<int>();
                        L.Add(AnalysisID);
                        Dict.Add(TaxonomicGroup, L);
                    }
                    else
                    {
                        Dict[TaxonomicGroup].Add(AnalysisID);
                    }
                }

            }
            catch (System.Exception ex)
            {
            }
            return Dict;
        }

        public enum SequenceType { None, Nucleotide, Protein }

        public static SequenceType TypeOfSequence(string MeasurementUnit)
        {
            Analysis.SequenceType sequence = Analysis.SequenceType.None;
            if (MeasurementUnit != null && MeasurementUnit == "DNA")
                sequence = Analysis.SequenceType.Nucleotide;
            return sequence;
        }


        #endregion
    }
}
