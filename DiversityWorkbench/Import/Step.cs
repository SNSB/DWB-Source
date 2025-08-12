using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Import
{

    public interface iWizardStep
    {
        //void setCurrentStep();
    }

    /// <summary>
    /// Keeps the informations connected to the step of data transfers like an image, the table, key etc.
    /// </summary>
    public class Step
    {
        #region Parameter & Properties

        #region Type
        
        public enum StepType { File, Attachment, Merging, Table, Testing, Import, Export }

        private StepType _StepType;

        public StepType TypeOfStep
        {
            get { return _StepType; }
            //set { _StepType = value; }
        }
        
        #endregion

        #region Column groups

        /// <summary>
        /// Groups of columns as shown in the interface with a separate display text and image positioned as child of the man step
        /// </summary>
        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> _StepColumnGroups;

        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> StepColumnGroups
        {
            get { return _StepColumnGroups; }
            set { _StepColumnGroups = value; }
        }

        /// <summary>
        /// List of ignored colums, e.g. LogUpdatedWhen, RowGUID etc.
        /// </summary>
        private System.Collections.Generic.List<string> _IgnoredColumns;
        
        #endregion

        #region Title and image

        /// <summary>
        /// If the step contains no table (e.g. step for selection of the file), the title of the step will be shown in the interface
        /// </summary>
        private string _Title;
        //public string Title
        //{
        //    get { return _Title; }
        //    set { _Title = value; }
        //}

        private System.Drawing.Image _Image;
        /// <summary>
        /// The image representing the step
        /// </summary>
        public System.Drawing.Image Image
        {
            get { return _Image; }
            set { _Image = value; }
        }
        
        public string DisplayText
        {
            get 
            {
                if (this.DataTableTemplate != null &&
                    DiversityWorkbench.Import.Import.Tables.ContainsKey(this.DataTableTemplate.TableAlias) &&
                    DiversityWorkbench.Import.Import.Tables[this.DataTableTemplate.TableAlias].GetDisplayText().Length > 0)
                    return DiversityWorkbench.Import.Import.Tables[this.DataTableTemplate.TableAlias].GetDisplayText();
                //if (this._DataTable != null &&
                //    this._DataTable.GetDisplayText().Length > 0)
                //    return this._DataTable.GetDisplayText();
                if (this._Title != null &&
                    this._Title.Length > 0)
                    return this._Title;
                if (this.TypeOfStep != StepType.Table)
                    return this.TypeOfStep.ToString();
                return ""; 
            }
        }

        #endregion

        #region Indent
        
        public readonly int IndentSize = 15;
        private int _IndentPosition = 0;
        /// <summary>
        /// the steps of indents from the front
        /// </summary>
        public int IndentPosition
        {
            get { return _IndentPosition; }
            set { _IndentPosition = value; }
        }

        public int Indent
        {
            get { return this._IndentPosition * this.IndentSize; }
        }
        
        #endregion

        //private System.Collections.Generic.List<int> _PositionKeyList;

        //public System.Collections.Generic.List<int> PositionKeyList
        //{
        //    get
        //    {
        //        if (this._PositionKeyList == null)
        //            this._PositionKeyList = new List<int>();
        //        return _PositionKeyList;
        //    }
        //    set { _PositionKeyList = value; }
        //}

        private string _PositionKey;
        /// <summary>
        /// A string representing the position of the step within the list - necessary for sorting
        /// Format: _[StepType][ n x _[Position of ParentStep]:[Parallel of ParentStep]]_[Position of Step]:[Parallel of Step]
        /// e.g.: 
        /// _04
        /// _01:00_05:02
        /// </summary>
        public string PositionKey
        {
            get 
            {
                if (this.TypeOfStep != StepType.Table)
                {
                    string Position = "_";
                    if ((int)this.TypeOfStep < 10)
                        Position += "0";
                    Position += ((int)this.TypeOfStep).ToString();
                    return Position;
                }
                if (DiversityWorkbench.Import.Import.Tables.ContainsKey(this.TableAlias))
                    return DiversityWorkbench.Import.Import.Tables[this.TableAlias].PositionKey;
                else return "";
            }
            set { _PositionKey = value; } // TODO: ReadOnly
        }

        private bool _IsSelected = false;
        /// <summary>
        /// If a step is selected
        /// </summary>
        public bool IsSelected
        {
            get 
            {
                if (this.MustSelect)
                    return true;
                return _IsSelected; 
            }
            set { _IsSelected = value; }
        }

        private bool _MustSelect = false;
        /// <summary>
        /// if this step must be selected
        /// </summary>
        public bool MustSelect
        {
            get { return _MustSelect; }
            set { _MustSelect = value; }
        }


        //private DiversityWorkbench.Import.Step _ParentStep;

        //public DiversityWorkbench.Import.Step ParentStep
        //{
        //    get { return _ParentStep; }
        //    set { _ParentStep = value; }
        //}

        //private int _ParallelPosition;

        //public int ParallelPosition
        //{
        //    get 
        //    {
        //        if (this._ParallelPosition == 0
        //            && this._ParentStep != null)
        //        {

        //        }
        //        return _ParallelPosition; 
        //    }
        //    set { _ParallelPosition = value; }
        //}

        private string _TableAlias;
        public string TableAlias
        {
            get { return _TableAlias; }
            set { _TableAlias = value; }
        }

        private DiversityWorkbench.Import.DataTable _DataTableTemplate;

        public DiversityWorkbench.Import.DataTable DataTableTemplate
        {
            get { return _DataTableTemplate; }
            set { _DataTableTemplate = value; }
        }

        /// <summary>
        /// The columns of the table that are not ignored and not inside a group
        /// these a shown with the main step
        /// </summary>
        public System.Collections.Generic.List<string> _ColumnsOutsideGroups;
        public System.Collections.Generic.List<string> ColumnsOutsideGroups
        {
            get
            {
                if (this._ColumnsOutsideGroups == null)
                {
                    this._ColumnsOutsideGroups = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in DiversityWorkbench.Import.Import.Tables[this.DataTableTemplate.TableAlias].DataColumns)
                    {
                        if (DiversityWorkbench.Import.Import.Tables[this.DataTableTemplate.TableAlias].IgnoredColumns != null && DiversityWorkbench.Import.Import.Tables[this.DataTableTemplate.TableAlias].IgnoredColumns.Contains(KV.Key))
                            continue;
                        if (DiversityWorkbench.Import.Import.Tables[this.DataTableTemplate.TableAlias].LoggingColumns.Contains(KV.Key))
                            continue;
                        bool ColumnInGroup = false;
                        if (this._StepColumnGroups != null && this._StepColumnGroups.Count > 0)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.StepColumnGroup> KVgroup in this._StepColumnGroups)
                            {
                                if (KVgroup.Value.Columns.Contains(KV.Key))
                                {
                                    ColumnInGroup = true;
                                    continue;
                                }
                            }
                        }
                        if (ColumnInGroup)
                            continue;
                        this._ColumnsOutsideGroups.Add(KV.Key);
                    }
                }
                return this._ColumnsOutsideGroups;
            }
        }

        //System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step>> _ChildSteps;

        //public System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step>> ChildSteps
        //{
        //    get 
        //    {
        //        if (this._ChildSteps == null) this._ChildSteps = new Dictionary<string, Dictionary<string, Step>>();
        //        return _ChildSteps; 
        //    }
        //    set { _ChildSteps = value; }
        //}

        //public void AddChildStep(DiversityWorkbench.Import.Step ChildStep)
        //{
        //    if (!this.ChildSteps.ContainsKey(DiversityWorkbench.Import.Import.Tables[ChildStep.DataTableTemplate.TableAlias].TableName))
        //    {
        //        System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.Step> Steps = new Dictionary<string, Step>();
        //        this._ChildSteps.Add(DiversityWorkbench.Import.Import.Tables[ChildStep.DataTableTemplate.TableAlias].TableName, Steps);
        //    }
        //    this.ChildSteps[DiversityWorkbench.Import.Import.Tables[ChildStep.DataTableTemplate.TableAlias].TableName].Add(ChildStep.PositionKey, ChildStep);
        //}

        /// <summary>
        /// If a step can create copies of itself depends upon the table
        /// </summary>
        public bool CanCreateCopiesOfItself
        {
            get
            {
                if (this.DataTableTemplate.TableAlias == null || this.DataTableTemplate.TableAlias.Length == 0 || !DiversityWorkbench.Import.Import.Tables.ContainsKey(this.DataTableTemplate.TableAlias))
                    return false;
                // Markus 22.12.2016: Test if possible
                if (DiversityWorkbench.Import.Import.Tables[this.DataTableTemplate.TableAlias].TypeOfParallelity == DataTable.Parallelity.parallel)
                    return true;
                // MW 2017-05-04 Test
                //else if (DiversityWorkbench.Import.Import.Tables[this.DataTableTemplate.TableAlias].TypeOfParallelity == DataTable.Parallelity.referencing)
                //    return true;
                return false;
            }
        }

        /// <summary>
        /// Copy the current step together with all child steps an their childs
        /// </summary>
        public void CopyStep()
        {
            DiversityWorkbench.Import.DataTable DT = DiversityWorkbench.Import.DataTable.GetTableParallel(DiversityWorkbench.Import.Import.Tables[this.TableAlias], true, "");
            DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Step.GetStep(DT, this.IndentPosition);
            if (this.StepColumnGroups != null && this.StepColumnGroups.Count > 0)
            {
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups = new Dictionary<string, DiversityWorkbench.Import.StepColumnGroup>();
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.StepColumnGroup> KV in this.StepColumnGroups)
                {
                    ColumnGroups.Add(KV.Key, KV.Value);
                }
                S.StepColumnGroups = ColumnGroups;
            }
            this.CopyChildSteps(DiversityWorkbench.Import.Import.Tables[this.TableAlias], DT.TableAlias);
        }

        /// <summary>
        /// Copy the child steps according to the hierarchy in the template table
        /// </summary>
        /// <param name="DTtemplate">The table used as a template for the hierarchy</param>
        /// <param name="ParentTableAlias">The name of the table where the new child steps should be placed</param>
        public void CopyChildSteps(DiversityWorkbench.Import.DataTable DTtemplate, string ParentTableAlias)
        {

            // find all table templates that should be copied
            //System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTable> TemplatesToCopy = new Dictionary<string, DataTable>();
            System.Collections.Generic.List<DiversityWorkbench.Import.DataTable> ChildTemplatesToCopy = new List<DataTable>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
            {
                if (KV.Value.ParentTableAlias == DTtemplate.TableAlias)
                {
                    ChildTemplatesToCopy.Add(KV.Value);
                }
            }

            if (ChildTemplatesToCopy.Count == 0)
                return;

            // for all templates that where found, create the copies of the tables and the steps 
            // and add the template tables to the list that must be checked for childs
            System.Collections.Generic.Dictionary<DiversityWorkbench.Import.DataTable, string> TemplateTablesToCheckForChilds = new Dictionary<DiversityWorkbench.Import.DataTable, string>();
            foreach (DiversityWorkbench.Import.DataTable TT in ChildTemplatesToCopy)
            {
                DiversityWorkbench.Import.DataTable DT = DiversityWorkbench.Import.DataTable.GetTableParallel(TT, true, ParentTableAlias);
                DT.ParentTableAlias = ParentTableAlias;
                if (DT.ParallelPosition != 1)
                { }
                //DT.ParallelPosition = 1;

                DiversityWorkbench.Import.Step S = DiversityWorkbench.Import.Step.GetStep(DT, DiversityWorkbench.Import.Import.Steps[TT.PositionKey].IndentPosition);
                if (DiversityWorkbench.Import.Import.Steps[TT.PositionKey].StepColumnGroups != null && DiversityWorkbench.Import.Import.Steps[TT.PositionKey].StepColumnGroups.Count > 0)
                {
                    System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups = new Dictionary<string, DiversityWorkbench.Import.StepColumnGroup>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.StepColumnGroup> KV in DiversityWorkbench.Import.Import.Steps[TT.PositionKey].StepColumnGroups)
                    {
                        ColumnGroups.Add(KV.Key, KV.Value);
                    }
                    S.StepColumnGroups = ColumnGroups;
                }
                TemplateTablesToCheckForChilds.Add(TT, DT.TableAlias);
            }

            // Check for further childs for the Template tables that had been found
            foreach (System.Collections.Generic.KeyValuePair<DiversityWorkbench.Import.DataTable, string> KV in TemplateTablesToCheckForChilds)
            {
                this.CopyChildSteps(KV.Key, KV.Value);
            }
        }

        public void RemoveStepChilds()
        {
            System.Collections.Generic.List<string> ChildsToRemove = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
            {
                if (KV.Value.ParentTableAlias == this.TableAlias)
                {
                    ChildsToRemove.Add(KV.Value.TableAlias);
                    this.RemoveStepChilds(KV.Value.TableAlias, ref ChildsToRemove);
                }
            }
            foreach (string s in ChildsToRemove)
            {
                DiversityWorkbench.Import.Import.Steps.Remove(DiversityWorkbench.Import.Import.Tables[s].PositionKey);
                DiversityWorkbench.Import.Import.Tables.Remove(s);
            }
        }

        private void RemoveStepChilds(string TableAlias, ref System.Collections.Generic.List<string> ChildsToRemove)
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
            {
                if (KV.Value.ParentTableAlias == TableAlias)
                {
                    ChildsToRemove.Add(KV.Value.TableAlias);
                    this.RemoveStepChilds(KV.Value.TableAlias, ref ChildsToRemove);
                }
            }
        }

        #endregion

        #region Construction

        private Step(string PositionKey, DiversityWorkbench.Import.DataTable DataTableTemplate, System.Drawing.Image Image, int Indent)
        {
            this.PositionKey = PositionKey;
            this._DataTableTemplate = DataTableTemplate;
            this.TableAlias = DataTableTemplate.TableAlias;
            //this.DataTableTemplate = DataTableTemplate;
            this._Image = Image;
            this._IndentPosition = Indent;
            this._StepType = StepType.Table;
        }

        private Step(DiversityWorkbench.Import.DataTable DataTableTemplate, System.Drawing.Image Image, int Indent)
        {
            this._DataTableTemplate = DataTableTemplate;
            this._TableAlias = DataTableTemplate.TableAlias;
            this.DataTableTemplate = DataTableTemplate;
            this._Image = Image;
            this._IndentPosition = Indent;
            this._StepType = StepType.Table;
        }

        private Step(string PositionKey, DiversityWorkbench.Import.Step.StepType Type, System.Drawing.Image Image, int Indent)
        {
            this.PositionKey = PositionKey;
            this._StepType = Type;
            this._Image = Image;
            this._IndentPosition = Indent;
        }

        #endregion

        #region Getting templates for the steps
        

        /// <summary>
        /// Getting a step for the import
        /// </summary>
        /// <param name="PositionKey">The position key</param>
        /// <param name="DataTable">The datatable</param>
        /// <param name="Image">The image of the step</param>
        /// <param name="Indent">The indent in the interface showing the hierarchical position of the step</param>
        /// <param name="IsSelected">If a step is selected by default</param>
        /// <param name="ColumnGroups">The column groups within the step</param>
        /// <returns></returns>
        public static DiversityWorkbench.Import.Step GetStepTemplate(
            string PositionKey,
            DiversityWorkbench.Import.DataTable DataTable,
            System.Drawing.Image Image,
            int Indent,
            bool IsSelected,
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups)
        {
            try
            {
                if (!DiversityWorkbench.Import.Import.TemplateSteps.ContainsKey(PositionKey))
                {
                    DiversityWorkbench.Import.Step S = new Step(PositionKey, DataTable, Image, Indent);
                    S._IsSelected = IsSelected;
                    S._StepColumnGroups = ColumnGroups;
                    DiversityWorkbench.Import.Import.TemplateSteps.Add(PositionKey, S);
                    if (DiversityWorkbench.Import.Import.TemplateTables[S.DataTableTemplate.TableAlias].TypeOfParallelity == DiversityWorkbench.Import.DataTable.Parallelity.parallel)
                    {

                    }
                }
                return DiversityWorkbench.Import.Import.TemplateSteps[PositionKey];
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }

        /// <summary>
        /// Getting a step for the import
        /// </summary>
        /// <param name="DataTable">The template for the datatable</param>
        /// <param name="Image">The image of the step</param>
        /// <param name="Indent">The indent in the interface showing the hierarchical position of the step</param>
        /// <param name="IsSelected">If a step is selected by default</param>
        /// <param name="ColumnGroups">The column groups within the step</param>
        /// <returns></returns>
        public static DiversityWorkbench.Import.Step GetStepTemplate(
            DiversityWorkbench.Import.DataTable DataTable,
            System.Drawing.Image Image,
            int Indent,
            bool IsSelected,
            System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.StepColumnGroup> ColumnGroups)
        {//3
            try
            {
                if (!DiversityWorkbench.Import.Import.TemplateSteps.ContainsKey(DataTable.PositionKey))
                {
                    DiversityWorkbench.Import.Step S = new Step(DataTable, Image, Indent);
                    S._IsSelected = IsSelected;
                    S._StepColumnGroups = ColumnGroups;
                    DiversityWorkbench.Import.Import.TemplateSteps.Add(DataTable.PositionKey, S);
                    //if (DiversityWorkbench.Import.Import.TemplateTables[S.DataTableTemplate.TableAlias].TypeOfParallelity == DiversityWorkbench.Import.DataTable.Parallelity.parallel)
                    //{

                    //}
                }
                return DiversityWorkbench.Import.Import.TemplateSteps[DataTable.PositionKey];
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }

        #endregion

        internal static DiversityWorkbench.Import.Step GetStep(DiversityWorkbench.Import.DataTable DataTable, int Indent)
        {
            try
            {
                if (!DiversityWorkbench.Import.Import.Steps.ContainsKey(DataTable.PositionKey))
                {
                    DiversityWorkbench.Import.Step S = new Step(DataTable.PositionKey, DataTable, DataTable.Image, Indent);
                    S.TableAlias = DataTable.TableAlias;
                    DiversityWorkbench.Import.Import.Steps.Add(DataTable.PositionKey, S);
                }
                return DiversityWorkbench.Import.Import.Steps[DataTable.PositionKey];
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }


        /// <summary>
        /// Getting a step for the import
        /// </summary>
        /// <param name="PositionKey">The position key</param>
        /// <param name="DataTable">The datatable</param>
        /// <param name="Image">The image of the step</param>
        /// <param name="Indent">The indent in the interface showing the hierarchical position of the step</param>
        /// <returns></returns>
        internal static DiversityWorkbench.Import.Step GetStep(string PositionKey, DiversityWorkbench.Import.DataTable DataTable, System.Drawing.Image Image, int Indent)
        {
            try
            {
                if (!DiversityWorkbench.Import.Import.Steps.ContainsKey(PositionKey))
                {
                    DiversityWorkbench.Import.Step S = new Step(PositionKey, DataTable, Image, Indent);
                    S.TableAlias = DataTable.TableAlias;
                    DiversityWorkbench.Import.Import.Steps.Add(PositionKey, S);
                    //if (DiversityWorkbench.Import.Import.Tables[S.TableAlias].TypeOfParallelity == DiversityWorkbench.Import.DataTable.Parallelity.parallel)
                    //{

                    //}
                }
                return DiversityWorkbench.Import.Import.Steps[PositionKey];
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }

        internal static DiversityWorkbench.Import.Step GetStep(DiversityWorkbench.Import.Step.StepType StepType, System.Drawing.Image Image)
        {
            try
            {
                string PositionKey = "_";
                if (((int)StepType) < 10)
                    PositionKey += "0";
                PositionKey += ((int)StepType).ToString();
                if (!DiversityWorkbench.Import.Import.Steps.ContainsKey(PositionKey))
                {
                    DiversityWorkbench.Import.Step S = new Step(PositionKey, StepType, Image, 0);
                    DiversityWorkbench.Import.Import.Steps.Add(PositionKey, S);
                }
                return DiversityWorkbench.Import.Import.Steps[PositionKey];
            }
            catch (System.Exception ex)
            {
            }
            return null;
        }
    
    }

}
