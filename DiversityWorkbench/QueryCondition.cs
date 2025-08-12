using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DiversityWorkbench
{

    public interface IUserControlQueryCondition
    {
        DiversityWorkbench.QueryCondition Condition();
        DiversityWorkbench.QueryCondition getCondition();
        void setConditionValues(DiversityWorkbench.QueryCondition Condition);
        string ConditionValue();
        string WhereClause();
        string SQL();
        string SqlByIndex(int FieldSequence);
        void Clear();
        void setEntity();
        //void Refresh();
        void RefreshProjectDependentHierarchy();
        void setUserControlQueryList(UserControls.UserControlQueryList List);
    }

    public struct QueryField
    {
        #region Parameter

        public string TableName;
        public string ColumnName;
        public string IdentityColumn;

        #endregion

        public QueryField(string Table, string Column, string IdentityColumn)
        {
            this.TableName = Table;
            this.ColumnName = Column;
            this.IdentityColumn = IdentityColumn;
        }
    }

    [Serializable]
    public class QueryCondition
    {

        #region Parameter

        /// <summary>
        /// If the query condition should be shown by default. Default = false;
        /// </summary>
        public bool showCondition;
        /// <summary>
        /// The table name of the query. Must be set
        /// </summary>
        public string Table;
        public bool SourceIsFunction;
        public string Source;
        /// <summary>
        /// The identity column of the query table. Must be set
        /// </summary>
        public string IdentityColumn;
        public string ForeignKey;
        public string ForeignKeySecondColumn;
        public string ForeignKeySecondColumnInForeignTable;
        /// <summary>
        /// The name of the table linked via a foreign key
        /// </summary>
        public string ForeignKeyTable;
        public string IntermediateTable;
        public string SqlFromClause;
        /// <summary>
        /// If the SqlFromClause is followed by the value entered by the user and a postfix
        /// e.g. when a SQL-function is used to retrieve the data like Select * from dbo.MultiColumnQuery('Identifier', ' ... ')
        /// where ... is the entry of the user
        /// </summary>
        public string SqlFromClausePostfix;
        /// <summary>
        /// The column to which the query should be applied. Must be set
        /// </summary>
        public string Column;
        public string DisplayColumn;
        public string HierarchyDisplayColumn;
        public string HierarchyColumn;
        public string HierarchyParentColumn;
        public string OrderColumn;
        /// <summary>
        /// The group within which the query should be displayed in the user interface. Must be set.
        /// </summary>
        public string QueryGroup;
        /// <summary>
        /// The display text shown in the user interface. Length should be about 10 - 15 characters.
        /// otherwise it will be abbreviated
        /// </summary>
        public string DisplayText;
        public string DisplayLongText;
        public string Description;
        /// <summary>
        ///  If the queried column is a date
        /// </summary>
        public bool IsDate;
        public bool IsYear;
        public bool IsNumeric;
        public bool IsBoolean;
        public bool IsTextAsNumeric;
        /// <summary>
        ///  If the queried column is of the type datetime
        /// </summary>
        public bool IsDatetime;
        /// <summary>
        ///  If the queried column is of the type XML
        /// </summary>
        public bool IsXML;
        /// <summary>
        ///  If the queried column is of the type XML resp. EXIF extracted from an image not working with standard XML queries
        /// </summary>
        public bool IsEXIF = false;
        ///// <summary>
        ///// If the queried column is of the type geography (as WGS84)
        ///// </summary>
        //public bool IsGeography;
        /// <summary>
        /// If the values should be selected from a list
        /// </summary>
        public bool SelectFromList;
        /// <summary>
        ///  If the values should be selected from a hierarchy
        public bool SelectFromHierachy;
        /// <summary>
        /// If the control may be copied to allow several entries
        /// </summary>
        public bool IsSet;
        /// <summary>
        /// The operator to combine parts of the set
        /// </summary>
        public UserControls.UserControlQueryList.QueryOperator SetOperator = UserControls.UserControlQueryList.QueryOperator.AND;
        /// <summary>
        /// The number of controls within a set
        /// </summary>
        public int SetCount = 1;
        /// <summary>
        /// The position within a set
        /// </summary>
        public int SetPosition = 0;
        /// <summary>
        /// The datatable containing the values
        /// </summary>
        public System.Data.DataTable dtValues;
        public string Value;
        /// <summary>
        /// The upper value for queries searching for a range
        /// </summary>
        public string UpperValue;
        public int? Day;
        public string DayColumn;
        public int? Month;
        public string MonthColumn;
        public int? Year;
        public string YearColumn;
        public string Operator;
        public string Restriction;
        public bool CombineQueryFieldsWithAnd;
        public int SelectedIndex;
        /// <summary>
        /// The operator for the query as selected in the user interface. Default = "=".
        /// </summary>
        public string QueryConditionOperator;
        public System.Windows.Forms.CheckState CheckState;
        /// <summary>
        /// If the query should be restricted to the project e.g. where tables contain the project as part of the primary key
        /// </summary>
        public bool RestrictToProject;
        /// <summary>
        /// If the existence of datasets should be checked, either via the foreign key or the existence in the related table
        /// </summary>
        public enum CheckDataExistence { NoCheck, ForeignKeyIsNull, DatasetsInRelatedTable };
        /// <summary>
        /// If the existence of data should be checked. Default = NoCheck.
        /// </summary>
        public CheckDataExistence CheckIfDataExist;
        /// <summary>
        /// The entity related to the queried field. Refers to the ENTITY tables where context, description etc. in any language may be available
        /// </summary>
        public string Entity;
        /// <summary>
        /// If the text is fixed and should not be derived from the entity information
        /// </summary>
        public bool TextFixed;
        public bool useGroupAsEntityForGroups;
        /// <summary>
        /// A list of fields that should be checked in a query
        /// </summary>
        public System.Collections.Generic.List<DiversityWorkbench.QueryField> QueryFields;
        public bool ForMultiFieldQuery = false;

#if DEBUG // Markus 22.6.2021 - Ermöglichung von AND Suche

        /// <summary>
        /// Enable query where all given values must be present (Symbol = &)
        /// </summary>
        //public bool EnableQueryWithAnd = false;
#endif
        /// <summary>
        /// Selection types
        /// </summary>
        public enum QueryTypes { Text, DateTime, Date, Year, Numeric, Boolean, XML, ListString, ListNumeric, Hierarchy, Geography, Annotation, AnnotationReference, ReferencingTable, Module, Count };

        /// <summary>
        /// If the existence of data should be checked. Default = NoCheck.
        /// </summary>
        public QueryTypes QueryType;
        /// <summary>

        public DiversityWorkbench.Annotation Annotation;

        public DiversityWorkbench.ReferencingTable ReferencingTable;

        public System.Collections.Generic.Dictionary<string, System.Drawing.Image> Images;

        public DiversityWorkbench.ServerConnection ServerConnection;

        public bool DependsOnCurrentProjectID = false;

        public DiversityWorkbench.IUserControlQueryCondition iUserControlQueryCondition;

        /// <summary>
        /// if a column belongs to the primary key and a search for exsistence does not make sence, e.g. LocalisationSystemID in Table CollectionEventLocalisation
        /// </summary>
        public bool SupressSearchForPresence = false;

        /// <summary>
        /// Link columns used for optimized queries containing the tables and the columns where the key is the name of the column in the table and the value the name of the related column
        /// </summary>
        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> OptimizingLinkColumns;

        /// <summary>
        /// For queries linked to other modules, the interface to the specific module
        /// </summary>
        public DiversityWorkbench.IWorkbenchUnit iWorkbenchUnit = null;

        //public DiversityWorkbench.WorkbenchUnit.ModuleType Module = WorkbenchUnit.ModuleType.None;

        #endregion

        #region Construction

        public QueryCondition()
        {
        }

        public QueryCondition(DiversityWorkbench.QueryCondition QueryCondition)
        {
            this.QueryType = QueryCondition.QueryTypes.Text;
            //QuerySelectionType = UserControlQueryCondition.SelectionType.Text;
            System.Collections.Generic.Dictionary<string, string> EntityInformation = new Dictionary<string, string>();
            this.TextFixed = QueryCondition.TextFixed;
            if (QueryCondition.Entity != null && QueryCondition.Entity.Length > 0)
            {
                this.Entity = QueryCondition.Entity;
                EntityInformation = DiversityWorkbench.Entity.EntityInformation(QueryCondition.Entity);
                this.DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityInformation);
                this.DisplayLongText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityInformation);
                this.Description = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, EntityInformation);
            }
            else
            {
                this.DisplayText = "";
                this.DisplayLongText = "";
                this.Description = "";
            }

            if (QueryCondition.DisplayText != null && QueryCondition.DisplayText.Length > 0)
                this.DisplayText = QueryCondition.DisplayText;
            if (QueryCondition.DisplayLongText != null && QueryCondition.DisplayLongText.Length > 0)
                this.DisplayLongText = QueryCondition.DisplayLongText;
            if (this.DisplayLongText.Length == 0 && this.TextFixed && this.DisplayText.Length > 0)
                this.DisplayLongText = this.DisplayText;
            if (QueryCondition.Description != null && QueryCondition.Description.Length > 0)
                this.Description = QueryCondition.Description;


            this.showCondition = QueryCondition.showCondition;
            this.Table = QueryCondition.Table;
            this.SourceIsFunction = false;

            this.IdentityColumn = QueryCondition.IdentityColumn;

            if (QueryCondition.ForeignKey == null || QueryCondition.ForeignKey == "")
                this.ForeignKey = QueryCondition.IdentityColumn;
            else
                this.ForeignKey = QueryCondition.ForeignKey;

            if (QueryCondition.ForeignKeySecondColumn != null)
                this.ForeignKeySecondColumn = QueryCondition.ForeignKeySecondColumn;
            else this.ForeignKeySecondColumn = "";
            this.ForeignKeyTable = QueryCondition.ForeignKeyTable;
            if (this.ForeignKeyTable == null) this.ForeignKeyTable = "";

            if (QueryCondition.ForeignKeySecondColumnInForeignTable != null)
                this.ForeignKeySecondColumnInForeignTable = QueryCondition.ForeignKeySecondColumnInForeignTable;
            else this.ForeignKeySecondColumnInForeignTable = "";

            if (QueryCondition.IntermediateTable != null)
                this.IntermediateTable = QueryCondition.IntermediateTable;
            else this.IntermediateTable = "";

            this.SqlFromClause = QueryCondition.SqlFromClause;
            if (this.SqlFromClause == null) this.SqlFromClause = "";

            this.Column = QueryCondition.Column;
            if (this.Column == null) this.Column = this.IdentityColumn;

            if (QueryCondition.HierarchyDisplayColumn == null || QueryCondition.HierarchyDisplayColumn == "")
                this.HierarchyDisplayColumn = QueryCondition.Column;
            else
                this.HierarchyDisplayColumn = QueryCondition.HierarchyDisplayColumn;

            this.HierarchyParentColumn = QueryCondition.HierarchyParentColumn;
            if (this.HierarchyParentColumn == null) this.HierarchyParentColumn = "";

            if (QueryCondition.HierarchyColumn == null || QueryCondition.HierarchyColumn == "")
                this.HierarchyColumn = this.Column;
            else
                this.HierarchyColumn = QueryCondition.HierarchyColumn;

            if (QueryCondition.OrderColumn == null || QueryCondition.OrderColumn == "")
                this.OrderColumn = this.Column;
            else
                this.OrderColumn = QueryCondition.OrderColumn;

            if (QueryCondition.QueryGroup == null || QueryCondition.QueryGroup == "")
                this.QueryGroup = QueryCondition.Table;
            else
                this.QueryGroup = QueryCondition.QueryGroup;

            if (this.DisplayText.Length == 0)
            {
                if (QueryCondition.DisplayText == null || QueryCondition.DisplayText == "")
                    this.DisplayText = QueryCondition.Column;
                else
                    this.DisplayText = QueryCondition.DisplayText;
            }

            if (this.DisplayLongText.Length == 0)
                this.DisplayLongText = QueryCondition.DisplayLongText;

            if (this.Description.Length == 0)
                this.Description = QueryCondition.Description;

            this.IsDate = QueryCondition.IsDate;
            if (this.IsDate) this.QueryType = QueryCondition.QueryTypes.Date;
            this.IsYear = QueryCondition.IsYear;
            if (this.IsYear) this.QueryType = QueryCondition.QueryTypes.Year;
            this.IsNumeric = QueryCondition.IsNumeric;
            if (this.IsNumeric) this.QueryType = QueryCondition.QueryTypes.Numeric;
            this.IsTextAsNumeric = false;
            this.IsBoolean = QueryCondition.IsBoolean;
            if (this.IsBoolean) this.QueryType = QueryCondition.QueryTypes.Boolean;
            this.IsDatetime = QueryCondition.IsDatetime;
            if (this.IsDatetime) this.QueryType = QueryCondition.QueryTypes.DateTime;
            this.IsXML = QueryCondition.IsXML;
            if (this.IsXML) this.QueryType = QueryCondition.QueryTypes.XML;

            //this.IsGeography = QueryCondition.IsGeography;
            //if (this.IsGeography) this.QuerySelectionType = QueryCondition.QueryTypes.Geography;


            this.SelectFromList = QueryCondition.SelectFromList;
            if (this.SelectFromList && this.IsNumeric) this.QueryType = QueryCondition.QueryTypes.ListNumeric;
            if (this.SelectFromList && !this.IsNumeric) this.QueryType = QueryCondition.QueryTypes.ListString;
            this.SelectFromHierachy = QueryCondition.SelectFromHierachy;
            if (this.SelectFromHierachy) this.QueryType = QueryCondition.QueryTypes.Hierarchy;
            this.IsSet = QueryCondition.IsSet;
            if (!this.SelectFromHierachy && !this.SelectFromList) this.IsSet = false;
            this.Value = QueryCondition.Value;
            if (this.Value == null) this.Value = "";
            this.UpperValue = QueryCondition.UpperValue;
            this.Day = QueryCondition.Day;
            this.Month = QueryCondition.Month;
            this.Year = QueryCondition.Year;
            this.Operator = QueryCondition.Operator;
            this.dtValues = QueryCondition.dtValues;
            this.DayColumn = QueryCondition.DayColumn;
            this.MonthColumn = QueryCondition.MonthColumn;
            this.YearColumn = QueryCondition.YearColumn;
            this.Restriction = QueryCondition.Restriction;
            if (QueryCondition.QueryFields != null)
                this.QueryFields = QueryCondition.QueryFields;
            else
                this.QueryFields = new List<QueryField>();
            this.CombineQueryFieldsWithAnd = QueryCondition.CombineQueryFieldsWithAnd;
            this.SelectedIndex = QueryCondition.SelectedIndex;
            this.QueryConditionOperator = QueryCondition.QueryConditionOperator;
            this.CheckState = QueryCondition.CheckState;
            this.Entity = QueryCondition.Entity;
            this.useGroupAsEntityForGroups = QueryCondition.useGroupAsEntityForGroups;
            this.RestrictToProject = QueryCondition.RestrictToProject;
            this.CheckIfDataExist = QueryCondition.CheckIfDataExist;
            this.Annotation = new Annotation();
            this.Images = new Dictionary<string, Image>();
            this.QueryType = QueryCondition.QueryType;

            this.ServerConnection = new ServerConnection(DiversityWorkbench.Settings.ConnectionString);

        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description)
        {
            this.showCondition = showCondition;
            this.Table = Table;
            this.SourceIsFunction = false;
            this.IdentityColumn = IdentityColumn;
            this.ForeignKey = IdentityColumn;
            this.ForeignKeySecondColumn = "";
            this.ForeignKeySecondColumnInForeignTable = "";
            this.ForeignKeyTable = "";
            this.IntermediateTable = "";
            this.SqlFromClause = "";
            this.Column = Column;
            this.HierarchyDisplayColumn = Column;
            this.HierarchyParentColumn = "";
            this.HierarchyColumn = Column;
            this.OrderColumn = Column;
            this.QueryGroup = Group;
            this.DisplayText = Display;
            this.DisplayLongText = DisplayLong;
            this.Description = Description;
            this.IsDate = false;
            this.IsYear = false;
            this.IsNumeric = false;
            this.IsTextAsNumeric = false;
            this.IsBoolean = false;
            this.IsDatetime = false;
            this.IsXML = false;
            //this.IsGeography = false;
            this.SelectFromList = false;
            this.SelectFromHierachy = false;
            this.IsSet = false;
            this.Value = "";
            this.UpperValue = "";
            this.Day = null;
            this.Month = null;
            this.Year = null;
            this.Operator = "=";
            this.dtValues = new DataTable();
            this.DayColumn = "";
            this.MonthColumn = "";
            this.YearColumn = "";
            this.Restriction = "";
            this.QueryFields = new List<QueryField>();
            this.CombineQueryFieldsWithAnd = false;
            this.SelectedIndex = 0;
            this.QueryConditionOperator = "";
            this.CheckState = CheckState.Indeterminate;
            this.Entity = "";
            this.useGroupAsEntityForGroups = false;
            this.RestrictToProject = false;
            this.CheckIfDataExist = CheckDataExistence.NoCheck;
            this.TextFixed = false;
            //this.QuerySelectionType = QueryCondition.QueryTypes.Text;
            this.QueryType = QueryTypes.Text;
            this.Annotation = new Annotation();
            this.Images = new Dictionary<string, Image>();

            this.ServerConnection = new ServerConnection(DiversityWorkbench.Settings.ConnectionString);

        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, bool IsXML, string Group, string Display, string DisplayLong, string Description)
        {
            this.showCondition = showCondition;
            this.Table = Table;
            this.SourceIsFunction = false;
            this.IdentityColumn = IdentityColumn;
            this.ForeignKey = IdentityColumn;
            this.ForeignKeySecondColumn = "";
            this.ForeignKeySecondColumnInForeignTable = "";
            this.ForeignKeyTable = "";
            this.IntermediateTable = "";
            this.SqlFromClause = "";
            this.Column = Column;
            this.HierarchyDisplayColumn = Column;
            this.HierarchyParentColumn = "";
            this.HierarchyColumn = Column;
            this.OrderColumn = Column;
            this.QueryGroup = Group;
            this.DisplayText = Display;
            this.DisplayLongText = DisplayLong;
            this.Description = Description;
            this.IsDate = false;
            this.IsYear = false;
            this.IsNumeric = false;
            this.IsTextAsNumeric = false;
            this.IsBoolean = false;
            this.IsDatetime = false;
            this.IsXML = true;
            //this.IsGeography = false;
            this.SelectFromList = false;
            this.SelectFromHierachy = false;
            this.IsSet = false;
            this.Value = "";
            this.UpperValue = "";
            this.Day = null;
            this.Month = null;
            this.Year = null;
            this.Operator = "=";
            this.dtValues = new DataTable();
            this.DayColumn = "";
            this.MonthColumn = "";
            this.YearColumn = "";
            this.Restriction = "";
            this.QueryFields = new List<QueryField>();
            this.CombineQueryFieldsWithAnd = false;
            this.SelectedIndex = 0;
            this.QueryConditionOperator = "";
            this.CheckState = CheckState.Indeterminate;
            this.Entity = "";
            this.useGroupAsEntityForGroups = false;
            this.RestrictToProject = false;
            //this.CheckDatasetPresence = false;
            //this.CheckIfFieldIsNull = false;
            this.CheckIfDataExist = CheckDataExistence.NoCheck;
            this.TextFixed = false;
            this.QueryType = QueryCondition.QueryTypes.Text;
            this.Annotation = new Annotation();
            //this.QueryType = QueryTypes.Query;
            this.Images = new Dictionary<string, Image>();

            this.ServerConnection = new ServerConnection(DiversityWorkbench.Settings.ConnectionString);

        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description, bool isDate, bool isYear, bool isNumeric, bool isBoolean)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.IsDate = isDate;
            if (!isDate)
            {
                this.IsYear = isYear;
                if (!isYear)
                {
                    this.IsNumeric = isNumeric;
                    if (!isNumeric)
                    {
                        this.IsBoolean = isBoolean;
                    }
                }
            }
        }

        /// <summary>
        /// For DateTime values
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="isDateTime"></param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description, bool isDateTime)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.IsDatetime = true;
            this.IsDate = false;
            this.IsYear = false;
            this.IsNumeric = false;
            this.IsBoolean = false;
        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description, QueryCondition.QueryTypes QuerySelectionType)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.IsBoolean = false;
            this.IsDate = false;
            this.IsDatetime = false;
            //this.IsGeography = false;
            this.IsNumeric = false;
            this.IsXML = false;
            this.IsYear = false;
            this.SelectFromHierachy = false;
            this.SelectFromList = false;
            switch (QuerySelectionType)
            {
                case QueryCondition.QueryTypes.Boolean:
                    this.IsBoolean = true;
                    break;
                case QueryCondition.QueryTypes.Date:
                    this.IsDate = true;
                    break;
                case QueryCondition.QueryTypes.DateTime:
                    this.IsDatetime = true;
                    break;
                //case QueryCondition.QueryTypes.Geography:
                //    this.IsGeography = true;
                //    break;
                case QueryCondition.QueryTypes.Hierarchy:
                    this.SelectFromHierachy = true;
                    break;
                case QueryCondition.QueryTypes.ListNumeric:
                    this.SelectFromList = true;
                    this.IsNumeric = true;
                    break;
                case QueryCondition.QueryTypes.ListString:
                    this.SelectFromList = true;
                    break;
                case QueryCondition.QueryTypes.Numeric:
                    this.IsNumeric = true;
                    break;
                case QueryCondition.QueryTypes.XML:
                    this.IsXML = true;
                    break;
                case QueryCondition.QueryTypes.Year:
                    this.IsYear = true;
                    break;
                case QueryTypes.Count:
                    this.QueryType = QuerySelectionType;
                    this.IsEXIF = false;
                    this.IsNumeric = true;
                    this.IsSet = false;
                    this.IsTextAsNumeric = false;
                    break;
            }
        }

        /// <summary>
        /// For Datetime values with separated columns for Day, Month and Year
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="DayColumn"></param>
        /// <param name="MonthColumn"></param>
        /// <param name="YearColumn"></param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description, string DayColumn, string MonthColumn, string YearColumn)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.IsDate = true;
            this.IsYear = false;
            this.IsNumeric = true;
            this.IsBoolean = false;
            this.DayColumn = DayColumn;
            this.MonthColumn = MonthColumn;
            this.YearColumn = YearColumn;
        }

        /// <summary>
        /// For conditions selected from a table
        /// </summary>
        /// <param name="showCondition">If the condition should be shown by default</param>
        /// <param name="Table">The name of the table</param>
        /// <param name="IdentityColumn">The name of the identity Column</param>
        /// <param name="Column">The name of the column that should be tested</param>
        /// <param name="Group">The group in which the condition should be shown in the interface</param>
        /// <param name="Display">The display text in the interface</param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description">The description of the column</param>
        /// <param name="dtValues">The table listing the values</param>
        /// <param name="isNumeric">If the values in the list are numeric</param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description, System.Data.DataTable dtValues, bool isNumeric)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SelectFromList = true;
            this.dtValues = dtValues;
            this.IsNumeric = isNumeric;
        }

        /// <summary>
        /// For conditions selected from a table function
        /// </summary>
        /// <param name="showCondition">If the condition should be shown by default</param>
        /// <param name="Table">The name of the table</param>
        /// <param name="IdentityColumn">The name of the identity Column</param>
        /// <param name="Column">The name of the column that should be tested</param>
        /// <param name="Group">The group in which the condition should be shown in the interface</param>
        /// <param name="Display">The display text in the interface</param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description">The description of the column</param>
        /// <param name="dtValues">The table listing the values</param>
        /// <param name="isNumeric">If the values in the list are numeric</param>
        public QueryCondition(bool showCondition, string Table, bool SourceIsFunction, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description, System.Data.DataTable dtValues, bool isNumeric)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SelectFromList = true;
            this.SourceIsFunction = true;
            this.dtValues = dtValues;
            this.IsNumeric = isNumeric;
        }

        #endregion

        #region Checking the existence

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Group, string Display, string DisplayLong, string Description, CheckDataExistence CheckIfDataExist)
            : this(showCondition, Table, IdentityColumn, IdentityColumn, Group, Display, DisplayLong, Description)
        {
            this.CheckIfDataExist = CheckIfDataExist;
        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description, bool isDate, bool isYear, bool isNumeric, bool isBoolean, string ForeignKey, string Entity, CheckDataExistence CheckIfDataExist)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SqlFromClause = SqlFromClause;
            this.IsDate = isDate;
            this.Entity = Entity;
            if (!isDate)
            {
                this.IsYear = isYear;
                if (!isYear)
                {
                    this.IsNumeric = isNumeric;
                    if (!isNumeric)
                    {
                        this.IsBoolean = isBoolean;
                    }
                }
            }
            this.ForeignKey = ForeignKey;
        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Group, string Display, string DisplayLong, string Description, QueryTypes Type)
            : this(showCondition, Table, IdentityColumn, IdentityColumn, Group, Display, DisplayLong, Description)
        {
            this.CheckIfDataExist = CheckDataExistence.DatasetsInRelatedTable;
            this.QueryType = Type;
        }

        #endregion

        #region Enumeration tables
        /// <summary>
        /// For conditions selected from a standard enumeration table
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="SourceTable">The name of the enumeration table, e.g. CollMaterialCategory_Enum</param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description, string SourceTable)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            System.Data.DataTable dtEnum = new DataTable();
            dtEnum = DiversityWorkbench.EnumTable.EnumTableForQuery(SourceTable);
            this.SelectFromList = true;
            this.dtValues = dtEnum;
            this.IsNumeric = false;
        }

        /// <summary>
        /// For conditions selected from a standard enumeration table
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="SourceTable">The name of the enumeration table, e.g. CollMaterialCategory_Enum</param>
        /// <param name="ServerConnection">The Server connection</param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description, string SourceTable, DiversityWorkbench.ServerConnection ServerConnection)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            System.Data.DataTable dtEnum = new DataTable();
            dtEnum = DiversityWorkbench.EnumTable.EnumTableForQuery(SourceTable, ServerConnection.DatabaseName, ServerConnection.LinkedServer);
            this.SelectFromList = true;
            this.dtValues = dtEnum;
            this.IsNumeric = false;
        }


        /// <summary>
        /// For conditions selected from a standard enumeration table
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="SourceTable">The name of the enumeration table, e.g. CollMaterialCategory_Enum</param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description, string SourceTable, string Database)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            System.Data.DataTable dtEnum = new DataTable();
            dtEnum = DiversityWorkbench.EnumTable.EnumTableForQuery(SourceTable, Database);
            this.SelectFromList = true;
            this.dtValues = dtEnum;
            this.IsNumeric = false;
        }

        /// <summary>
        /// For conditions selected from a standard enumeration table
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="SourceTable">The name of the enumeration table, e.g. CollMaterialCategory_Enum</param>
        /// <param name="UseHierarchy">If the hierarchy should be used</param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong,
            string Description, string SourceTable, string Database, bool UseHierarchy)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            if (UseHierarchy)
            {
                this.SelectFromHierachy = true;
                this.OrderColumn = "DisplayText";
                this.HierarchyParentColumn = "ParentCode";
                this.HierarchyDisplayColumn = "DisplayText";
                this.HierarchyColumn = "Code";
                System.Data.DataTable dtEnum = new DataTable();
                string SQL = "SELECT NULL AS Code, NULL AS ParentCode, NULL AS DisplayText, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT Code, ParentCode, DisplayText, DisplayOrder FROM " + Database + ".dbo." + SourceTable +
                    " WHERE (DisplayEnable = 1) ORDER BY DisplayOrder ";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtEnum);
                this.dtValues = dtEnum;
                this.IsNumeric = false;
            }
            else
            {
                System.Data.DataTable dtEnum = new DataTable();
                dtEnum = DiversityWorkbench.EnumTable.EnumTableForQuery(SourceTable, Database);
                this.SelectFromList = true;
                this.dtValues = dtEnum;
                this.IsNumeric = false;
            }
        }

        /// <summary>
        /// For conditions selected from a standard enumeration table
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="SourceTable">The name of the enumeration table, e.g. CollMaterialCategory_Enum</param>
        /// <param name="UseHierarchy">If the hierarchy should be used</param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong,
            string Description, string SourceTable, string Database, bool UseHierarchy, bool UseDisplayOrder)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            if (UseHierarchy)
            {
                this.SelectFromHierachy = true;
                this.OrderColumn = "DisplayText";
                this.HierarchyParentColumn = "ParentCode";
                this.HierarchyDisplayColumn = "DisplayText";
                this.HierarchyColumn = "Code";
                System.Data.DataTable dtEnum = new DataTable();
                string SQL = "SELECT NULL AS Code, NULL AS ParentCode, NULL AS DisplayText, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT Code, ParentCode, DisplayText, DisplayOrder FROM " + Database + ".dbo." + SourceTable +
                    " WHERE (DisplayEnable = 1) ORDER BY ";
                if (UseDisplayOrder) SQL += "DisplayOrder";
                else SQL += "DisplayText";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtEnum);
                this.dtValues = dtEnum;
                this.IsNumeric = false;
            }
            else
            {
                System.Data.DataTable dtEnum = new DataTable();
                dtEnum = DiversityWorkbench.EnumTable.EnumTableForQuery(SourceTable, Database);
                this.SelectFromList = true;
                this.dtValues = dtEnum;
                this.IsNumeric = false;
            }
        }

        /// <summary>
        /// For conditions selected from a standard enumeration table
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="SourceTable">The name of the enumeration table, e.g. CollMaterialCategory_Enum</param>
        /// <param name="UseHierarchy">If the hierarchy should be used</param>
        /// <param name="ImageDictionary">A dictionary of images used in the hierarchy</param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string Group, string Display, string DisplayLong,
            string Description, string SourceTable, string Database, bool UseHierarchy, System.Collections.Generic.Dictionary<string, System.Drawing.Image> ImageDictionary)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            if (UseHierarchy)
            {
                this.SelectFromHierachy = true;
                this.OrderColumn = "DisplayText";
                this.HierarchyParentColumn = "ParentCode";
                this.HierarchyDisplayColumn = "DisplayText";
                this.HierarchyColumn = "Code";
                System.Data.DataTable dtEnum = new DataTable();
                string SQL = "SELECT NULL AS Code, NULL AS ParentCode, NULL AS DisplayText, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT Code, ParentCode, DisplayText, DisplayOrder FROM " + Database + ".dbo." + SourceTable +
                    " WHERE (DisplayEnable = 1) ORDER BY DisplayOrder ";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dtEnum);
                this.dtValues = dtEnum;
                this.IsNumeric = false;
            }
            else
            {
                System.Data.DataTable dtEnum = new DataTable();
                dtEnum = DiversityWorkbench.EnumTable.EnumTableForQuery(SourceTable, Database);
                this.SelectFromList = true;
                this.dtValues = dtEnum;
                this.IsNumeric = false;
            }
            this.Images = ImageDictionary;
        }
        #endregion

        #region Connected via foreign key
        // CONNECTED VIA FOREIGN KEY

        /// <summary>
        /// for data in tables connected via a foreign key
        /// </summary>
        /// <param name="showCondition">If the condition should be shown by default</param>
        /// <param name="Table">The name of the</param>
        /// <param name="IdentityColumn"></param>
        /// <param name="SqlForeignKey"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="SourceTable"></param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SqlFromClause = SqlFromClause;
            this.SelectFromList = false;
            this.IsNumeric = false;
        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description, string ForeignKey)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SqlFromClause = SqlFromClause;
            this.SelectFromList = false;
            this.IsNumeric = false;
            this.ForeignKey = ForeignKey;
        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description, string ForeignKey, string Entity)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SqlFromClause = SqlFromClause;
            this.SelectFromList = false;
            this.IsNumeric = false;
            this.ForeignKey = ForeignKey;
            this.Entity = Entity;
        }

        /// <summary>
        /// For dates connected via a foreign key
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="SqlForeignKey">the sql statement for the foreign key</param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="DayColumn"></param>
        /// <param name="MonthColumn"></param>
        /// <param name="YearColumn"></param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description, string DayColumn, string MonthColumn, string YearColumn)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.IsDate = true;
            this.IsYear = false;
            this.IsNumeric = true;
            this.IsBoolean = false;
            this.DayColumn = DayColumn;
            this.MonthColumn = MonthColumn;
            this.YearColumn = YearColumn;
            this.SqlFromClause = SqlFromClause;
        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description, string DayColumn, string MonthColumn, string YearColumn, string ForeignKey)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.IsDate = true;
            this.IsYear = false;
            this.IsNumeric = true;
            this.IsBoolean = false;
            this.DayColumn = DayColumn;
            this.MonthColumn = MonthColumn;
            this.YearColumn = YearColumn;
            this.SqlFromClause = SqlFromClause;
            this.ForeignKey = ForeignKey;
        }

        /// <summary>
        /// for numeric values in tables connected via a foreign key
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="SqlForeignKey"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="isDate"></param>
        /// <param name="isYear"></param>
        /// <param name="isNumeric"></param>
        /// <param name="isBoolean"></param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description, bool isDate, bool isYear, bool isNumeric, bool isBoolean)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SqlFromClause = SqlFromClause;
            this.IsDate = isDate;
            if (!isDate)
            {
                this.IsYear = isYear;
                if (!isYear)
                {
                    this.IsNumeric = isNumeric;
                    if (!isNumeric)
                    {
                        this.IsBoolean = isBoolean;
                    }
                }
            }
        }

        /// <summary>
        /// for numeric values in tables connected via a foreign key
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Table"></param>
        /// <param name="IdentityColumn"></param>
        /// <param name="IsForeignTable"></param>
        /// <param name="SqlFromClause"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="isDate"></param>
        /// <param name="isYear"></param>
        /// <param name="isNumeric"></param>
        /// <param name="isBoolean"></param>
        /// <param name="ForeignKey">The column name of the foreign key</param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description, bool isDate, bool isYear, bool isNumeric, bool isBoolean, string ForeignKey)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SqlFromClause = SqlFromClause;
            this.IsDate = isDate;
            if (!isDate)
            {
                this.IsYear = isYear;
                if (!isYear)
                {
                    this.IsNumeric = isNumeric;
                    if (!isNumeric)
                    {
                        this.IsBoolean = isBoolean;
                    }
                }
            }
            this.ForeignKey = ForeignKey;
        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description, bool isDate, bool isYear, bool isNumeric, bool isBoolean, string ForeignKey, string Entity)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SqlFromClause = SqlFromClause;
            this.IsDate = isDate;
            this.Entity = Entity;
            if (!isDate)
            {
                this.IsYear = isYear;
                if (!isYear)
                {
                    this.IsNumeric = isNumeric;
                    if (!isNumeric)
                    {
                        this.IsBoolean = isBoolean;
                    }
                }
            }
            this.ForeignKey = ForeignKey;
        }

        public QueryCondition(bool showCondition, string Table, string IdentityColumn, bool IsForeignTable, string SqlFromClause, string Column, string Group, string Display, string DisplayLong, string Description, System.Data.DataTable dtValues, bool isNumeric)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SqlFromClause = SqlFromClause;
            this.SelectFromList = true;
            this.dtValues = dtValues;
            this.IsNumeric = isNumeric;
        }

        #endregion

        #region Conditions in forms without connection to a dataset
        // FOR CUSTOM FORM
        /// <summary>
        /// For conditions in a custom for without connection to a dataset
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="Description"></param>
        public QueryCondition(bool showCondition, string Group, string Display, string Description)
            : this(showCondition, "", "", "", Group, Display, Display, Description)
        {
        }

        /// <summary>
        /// For conditions in a custom for without connection to a dataset
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="Description"></param>
        public QueryCondition(bool showCondition, string Table, string Column, string Group, string Display, string Description)
            : this(showCondition, Table, "", Column, Group, Display, Display, Description)
        {
        }

        #endregion

        #region Query conditions for combined queries where a field should be tested in several tables

        /// <summary>
        /// QueryCondition for a combined Query in several fields
        /// </summary>
        /// <param name="showCondition">If the condition should be shown by default</param>
        /// <param name="QueryFields">The list of the query fields</param>
        /// <param name="Group">The group in which the condition should be placed e.g. in a user interface</param>
        /// <param name="Display">The name of the condition e.g. in a user interface</param>
        /// <param name="DisplayLong">The explicit title of the condition</param>
        /// <param name="Description">The decription of the condition</param>
        public QueryCondition(bool showCondition, System.Collections.Generic.List<DiversityWorkbench.QueryField> QueryFields, string Group, string Display, string DisplayLong, string Description)
            : this(showCondition, QueryFields[0].TableName, QueryFields[0].IdentityColumn, QueryFields[0].ColumnName, Group, Display, DisplayLong, Description)
        {
            this.QueryFields = QueryFields;
        }

        /// <summary>
        /// QueryCondition for a combined Query in several fields with an entity to set a display text etc.
        /// </summary>
        /// <param name="showCondition">If the condition should be shown by default</param>
        /// <param name="QueryFields">The list of the query fields</param>
        /// <param name="Group">The group in which the condition should be placed e.g. in a user interface</param>
        /// <param name="Display">The name of the condition e.g. in a user interface</param>
        /// <param name="DisplayLong">The explicit title of the condition</param>
        /// <param name="Description">The decription of the condition</param>
        /// <param name="Entity">The entity assigned to the group in which the condition should appear</param>
        public QueryCondition(bool showCondition, System.Collections.Generic.List<DiversityWorkbench.QueryField> QueryFields, string Group, string Display, string DisplayLong, string Description, bool UseGroupAsEntityForGroup)
            : this(showCondition, QueryFields[0].TableName, QueryFields[0].IdentityColumn, QueryFields[0].ColumnName, Group, Display, DisplayLong, Description)
        {
            this.QueryFields = QueryFields;
            this.useGroupAsEntityForGroups = UseGroupAsEntityForGroup;
            if (UseGroupAsEntityForGroup)
                this.Entity = Group;
        }

        /// <summary>
        /// For conditions selected from a table
        /// </summary>
        /// <param name="showCondition">If the condition should be shown by default</param>
        /// <param name="QueryFields">The list of the query fields</param>
        /// <param name="Group">The group in which the condition should be shown in the interface</param>
        /// <param name="Display">The display text in the interface</param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description">The description of the column</param>
        /// <param name="dtValues">The table listing the values</param>
        /// <param name="isNumeric">If the values in the list are numeric</param>
        public QueryCondition(bool showCondition, System.Collections.Generic.List<DiversityWorkbench.QueryField> QueryFields, string Group, string Display, string DisplayLong, string Description, System.Data.DataTable dtValues, bool isNumeric)
            : this(showCondition, QueryFields[0].TableName, QueryFields[0].IdentityColumn, QueryFields[0].ColumnName, Group, Display, DisplayLong, Description)
        {
            this.SelectFromList = true;
            this.dtValues = dtValues;
            this.IsNumeric = isNumeric;
            this.QueryFields = QueryFields;
        }

        public QueryCondition(bool showCondition, System.Collections.Generic.List<DiversityWorkbench.QueryField> QueryFields, string Group, string Display, string DisplayLong, string Description, System.Data.DataTable dtValues, bool isNumeric, bool CombineQueryFieldsWithAnd)
            : this(showCondition, QueryFields, Group, Display, DisplayLong, Description, dtValues, isNumeric)
        {
            this.CombineQueryFieldsWithAnd = CombineQueryFieldsWithAnd;
        }

        #endregion

        #region Hierarchical conditions

        /// <summary>
        /// for conditions with a hierarchical organized table
        /// </summary>
        /// <param name="showCondition">If the condition should be shown by default</param>
        /// <param name="Table">The name of the table</param>
        /// <param name="IdentityColumn">The name of the identity Column</param>
        /// <param name="Column">The name of the column that should be tested</param>
        /// <param name="DisplayColumn">The name of the display column of the lookup table</param>
        /// <param name="HierarchyColumn">The name of the parent column of the lookup table</param>
        /// <param name="OrderColumn">The name of the order column of the lookup table</param>
        /// <param name="Group">The group in which the condition should be shown in the interface</param>
        /// <param name="Display">The display text in the interface</param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description">The description of the column</param>
        /// <param name="dtValues">The table listing the values</param>
        /// <param name="isNumeric">If the values in the list are numeric</param>
        public QueryCondition(bool showCondition, string Table, string IdentityColumn, string Column, string HierarchyDisplayColumn, string HierarchyColumn, string HierarchyParentColumn, string OrderColumn, string Group, string Display, string DisplayLong, string Description, System.Data.DataTable dtValues, bool isNumeric)
            : this(showCondition, Table, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.SelectFromHierachy = true;
            this.OrderColumn = OrderColumn;
            this.HierarchyParentColumn = HierarchyParentColumn;
            this.HierarchyDisplayColumn = HierarchyDisplayColumn;
            this.HierarchyColumn = HierarchyColumn;
            this.dtValues = dtValues;
            this.IsNumeric = isNumeric;
        }

        /// <summary>
        /// for conditions with a hierarchical organized table and a field list
        /// </summary>
        /// <param name="showCondition">If the hierarchy condition should be shown by default</param>
        /// <param name="QueryFields">The list of the query fields</param>
        /// <param name="Group">The group in which the condition should be shown in the interface</param>
        /// <param name="Display">The display text in the interface</param>
        /// <param name="DisplayLong">The more explicit text in the interface</param>
        /// <param name="Description">The description of the column</param>
        /// <param name="dtValues">The table listing the values</param>
        /// <param name="isNumeric">If the values in the list are numeric</param>
        /// <param name="OrderColumn">The name of the order column of the lookup table</param>
        /// <param name="HierarchyParentColumn">The column in the hierarchy table containing the link to the parent dataset</param>
        /// <param name="HierarchyDisplayColumn">The display column in the hierarchy table</param>
        /// <param name="HierarchyColumn">The name of the parent column of the lookup table</param>
        public QueryCondition(bool showCondition, System.Collections.Generic.List<DiversityWorkbench.QueryField> QueryFields, string Group, string Display, string DisplayLong, string Description, System.Data.DataTable dtValues, bool isNumeric, string OrderColumn, string HierarchyParentColumn, string HierarchyDisplayColumn, string HierarchyColumn)
            : this(showCondition, QueryFields[0].TableName, QueryFields[0].IdentityColumn, QueryFields[0].ColumnName, Group, Display, DisplayLong, Description)
        {
            this.SelectFromList = false;
            this.SelectFromHierachy = true;
            this.OrderColumn = OrderColumn;
            this.HierarchyParentColumn = HierarchyParentColumn;
            this.HierarchyDisplayColumn = HierarchyDisplayColumn;
            this.HierarchyColumn = HierarchyColumn;
            this.dtValues = dtValues;
            this.IsNumeric = isNumeric;

            this.QueryFields = QueryFields;
        }

        /// <summary>
        /// For Annotations
        /// </summary>
        /// <param name="showCondition"></param>
        /// <param name="Column"></param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="Annotation"></param>
        public QueryCondition(bool showCondition, string Column, string Group, string Display, string DisplayLong, string Description, DiversityWorkbench.Annotation Annotation)
        {
            this.showCondition = showCondition;
            this.Table = "Annotation";
            this.SourceIsFunction = false;
            this.IdentityColumn = "";
            this.ForeignKey = IdentityColumn;
            this.ForeignKeySecondColumn = "";
            this.ForeignKeySecondColumnInForeignTable = "";
            this.ForeignKeyTable = "";
            this.IntermediateTable = "";
            this.SqlFromClause = "";
            this.Column = Column;
            this.HierarchyDisplayColumn = Column;
            this.HierarchyParentColumn = "";
            this.HierarchyColumn = Column;
            this.OrderColumn = Column;
            this.QueryGroup = Group;
            this.DisplayText = Display;
            this.DisplayLongText = DisplayLong;
            this.Description = Description;
            this.IsDate = false;
            this.IsYear = false;
            this.IsNumeric = false;
            this.IsTextAsNumeric = false;
            this.IsBoolean = false;
            this.IsDatetime = false;
            this.IsXML = false;
            this.SelectFromList = false;
            this.SelectFromHierachy = false;
            this.IsSet = false;
            this.Value = "";
            this.UpperValue = "";
            this.Day = null;
            this.Month = null;
            this.Year = null;
            this.Operator = "=";
            this.dtValues = new DataTable();
            this.DayColumn = "";
            this.MonthColumn = "";
            this.YearColumn = "";
            this.Restriction = "";
            this.QueryFields = new List<QueryField>();
            this.CombineQueryFieldsWithAnd = false;
            this.SelectedIndex = 0;
            this.QueryConditionOperator = "";
            this.CheckState = CheckState.Indeterminate;
            this.Entity = "";
            this.useGroupAsEntityForGroups = false;
            this.RestrictToProject = false;
            this.CheckIfDataExist = CheckDataExistence.NoCheck;
            this.TextFixed = false;
            this.QueryType = QueryCondition.QueryTypes.Annotation;
            this.Annotation = Annotation;
            this.Images = new Dictionary<string, Image>();

            this.ServerConnection = new ServerConnection(DiversityWorkbench.Settings.ConnectionString);

        }

        public QueryCondition(bool showCondition, string Column, string Group, string Display, string DisplayLong, string Description, DiversityWorkbench.ReferencingTable ReferencingTable)
        {
            this.showCondition = showCondition;
            this.Table = ReferencingTable.TableName();
            this.SourceIsFunction = false;
            this.IdentityColumn = "";
            this.ForeignKey = IdentityColumn;
            this.ForeignKeySecondColumn = "";
            this.ForeignKeySecondColumnInForeignTable = "";
            this.ForeignKeyTable = "";
            this.IntermediateTable = "";
            this.SqlFromClause = "";
            this.Column = Column;
            this.HierarchyDisplayColumn = Column;
            this.HierarchyParentColumn = "";
            this.HierarchyColumn = Column;
            this.OrderColumn = Column;
            this.QueryGroup = Group;
            this.DisplayText = Display;
            this.DisplayLongText = DisplayLong;
            this.Description = Description;
            this.IsDate = false;
            this.IsYear = false;
            this.IsNumeric = false;
            this.IsTextAsNumeric = false;
            this.IsBoolean = false;
            this.IsDatetime = false;
            this.IsXML = false;
            this.SelectFromList = false;
            this.SelectFromHierachy = false;
            this.IsSet = false;
            this.Value = "";
            this.UpperValue = "";
            this.Day = null;
            this.Month = null;
            this.Year = null;
            this.Operator = "=";
            this.dtValues = new DataTable();
            this.DayColumn = "";
            this.MonthColumn = "";
            this.YearColumn = "";
            this.Restriction = "";
            this.QueryFields = new List<QueryField>();
            this.CombineQueryFieldsWithAnd = false;
            this.SelectedIndex = 0;
            this.QueryConditionOperator = "";
            this.CheckState = CheckState.Indeterminate;
            this.Entity = "";
            this.useGroupAsEntityForGroups = false;
            this.RestrictToProject = false;
            this.CheckIfDataExist = CheckDataExistence.NoCheck;
            this.TextFixed = false;
            this.QueryType = QueryCondition.QueryTypes.ReferencingTable;
            this.ReferencingTable = ReferencingTable;
            this.Images = new Dictionary<string, Image>();

            this.ServerConnection = new ServerConnection(DiversityWorkbench.Settings.ConnectionString);

        }

        /// <summary>
        /// for conditions with a hierarchical organized table that need a requery depending e.g. on the selected project
        /// </summary>
        /// <param name="showCondition">If the hierarchy condition should be shown by default</param>
        /// <param name="TableName">Name of the table</param>
        /// <param name="Source">Name of the source, e.g. a function</param>
        /// <param name="IdentityColumn">The IdentityColumn of the table</param>
        /// <param name="ColumnName">The Column name</param>
        /// <param name="Group"></param>
        /// <param name="Display"></param>
        /// <param name="DisplayLong"></param>
        /// <param name="Description"></param>
        /// <param name="isNumeric"></param>
        /// <param name="HierarchyColumn">the column containing the whole hierarchy</param>
        /// <param name="HierarchyParentColumn"></param>
        /// <param name="DisplayColumn">the column shown in the interface</param>
        /// <param name="HierarchyDisplayColumn"></param>
        public QueryCondition(bool showCondition, string TableName, string Source, string IdentityColumn, string Group, string Display, string DisplayLong, string Description, bool isNumeric, string HierarchyColumn, string HierarchyParentColumn, string DisplayColumn, string HierarchyDisplayColumn)
            : this(showCondition, TableName, IdentityColumn, HierarchyParentColumn, Group, Display, DisplayLong, Description)
        {
            this.SelectFromList = false;
            this.Column = HierarchyColumn;
            this.SelectFromHierachy = true;
            this.Source = Source;
            this.HierarchyParentColumn = HierarchyParentColumn;
            this.HierarchyDisplayColumn = HierarchyDisplayColumn;
            this.DisplayColumn = DisplayColumn;
            this.HierarchyColumn = HierarchyColumn;
            this.IsNumeric = isNumeric;
        }

        #endregion

        public QueryCondition(bool showCondition, DiversityWorkbench.IWorkbenchUnit iWorkbenchUnit, string TableName, string IdentityColumn, string Column, string Group, string Display, string DisplayLong, string Description)
            : this(showCondition, TableName, IdentityColumn, Column, Group, Display, DisplayLong, Description)
        {
            this.QueryType = QueryTypes.Module;
            this.iWorkbenchUnit = iWorkbenchUnit;
            this.QueryFields = new List<QueryField>();
            QueryField QF = new QueryField(TableName, Column, IdentityColumn);
            this.QueryFields.Add(QF);
        }

        #region public interface
        
        public string Display(System.Windows.Forms.Control C, DiversityWorkbench.Entity.EntityInformationField PreferredContent)
        {
            string Display = "";
            Graphics g = Graphics.FromHwnd(C.Handle);
            int BorderWidth = 8;
            int StringWidth = 0;
            if (this.Entity != null && this.Entity.Length > 0)
            {
                System.Collections.Generic.Dictionary<string, string> EntityInformation = DiversityWorkbench.Entity.EntityInformation(this.Entity);
                switch (PreferredContent)
                {
                    case DiversityWorkbench.Entity.EntityInformationField.Abbreviation:
                        Display = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityInformation);
                        if (Display.Length == 0)
                            Display = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityInformation);
                        if (Display.Length == 0)
                            Display = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, EntityInformation);
                        break;
                    case DiversityWorkbench.Entity.EntityInformationField.DisplayText:
                        Display = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityInformation);
                        if (Display.Length == 0)
                            Display = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityInformation);
                        if (Display.Length == 0)
                            Display = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, EntityInformation);
                        break;
                    case DiversityWorkbench.Entity.EntityInformationField.Description:
                        Display = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, EntityInformation);
                        if (Display.Length == 0)
                            Display = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, EntityInformation);
                        if (Display.Length == 0)
                            Display = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityInformation);
                        break;
                    default:
                        break;
                }
                if (Display == this.Entity)
                    Display = "";
            }
            if (Display.Length == 0)
            {
                switch (PreferredContent)
                {
                    case DiversityWorkbench.Entity.EntityInformationField.Abbreviation:
                        Display = this.DisplayText;
                        break;
                    case DiversityWorkbench.Entity.EntityInformationField.DisplayText:
                        if (this.DisplayLongText != null)
                            Display = this.DisplayLongText;
                        break;
                    case DiversityWorkbench.Entity.EntityInformationField.Description:
                        if (this.Description != null)
                            Display = this.Description;
                        break;
                    default:
                        Display = this.DisplayText;
                        break;
                }
                StringWidth = (int)g.MeasureString(this.Description, C.Font).Width;
                if (this.DisplayLongText != null && ((StringWidth > C.Width - BorderWidth) || Display.Length == 0))
                    Display = this.DisplayLongText;

                StringWidth = (int)g.MeasureString(this.DisplayLongText, C.Font).Width;
                if (Display != null && ((StringWidth > C.Width - BorderWidth) || Display.Length == 0))
                    Display = this.DisplayText;
                else Display = this.DisplayText;
            }
            if (Display == "" && this.Entity.Length > 0)
                Display = this.Entity;
            if (PreferredContent == DiversityWorkbench.Entity.EntityInformationField.Description)
                return Display;
            if (this.CheckIfDataExist != CheckDataExistence.NoCheck
                && !Display.EndsWith(" present"))
                Display += " present";
            StringWidth = (int)g.MeasureString(Display, C.Font).Width;
            if (StringWidth > C.Width * 2)
            {
                Display = this.Column;
                string NewDisplay = "";
                for (int i = 0; i < Display.Length; i++)
                {
                    if (i > 0 && Display[i].ToString().ToUpper() == Display[i].ToString())
                        NewDisplay += " ";
                    NewDisplay += Display[i];
                }
                Display = NewDisplay;
            }
            while (StringWidth > C.Width - BorderWidth)
            {
                if (Display.IndexOf(" ") > -1)
                {
                    string[] ss = Display.Split(new char[] { ' ' });
                    int MaxStringPartWidth = 0;
                    for (int i = 0; i < ss.Length; i++)
                    {
                        int iMax = (int)g.MeasureString(ss[i], C.Font).Width;
                        if (iMax > MaxStringPartWidth)
                            MaxStringPartWidth = iMax;
                    }
                    string DisplayShort = "";
                    for (int i = 0; i < ss.Length; i++)
                    {
                        int test;
                        bool IsNumber = false;
                        if (int.TryParse(ss[i].Substring(ss[i].Length - 1), out test))
                            IsNumber = true;
                        int iMax = (int)g.MeasureString(ss[i], C.Font).Width;
                        if (iMax >= MaxStringPartWidth
                            && !IsNumber)
                        {
                            if (ss[i].EndsWith("."))
                                ss[i] = ss[i].Substring(0, ss[i].Length - 1);
                            if (ss[i].IndexOf("(") > -1 && ss[i].IndexOf(")") > -1)
                                ss[i] = ss[i].Replace("(", "").Replace(")", "");
                            ss[i] = ss[i].Substring(0, ss[i].Length - 1) + ".";

                        }
                        DisplayShort += " " + ss[i];
                    }
                    Display = DisplayShort.Trim();
                }
                Display = Display.Substring(0, Display.Length - 2) + ".";
                if (Display.EndsWith(".."))
                    Display = Display.Substring(0, Display.Length - 1);
                if (Display.EndsWith(". ."))
                    Display = Display.Substring(0, Display.Length - 2);
                if (Display.EndsWith(" ."))
                    Display = Display.Substring(0, Display.Length - 1).Trim();
                StringWidth = (int)g.MeasureString(Display, C.Font).Width;
            }
            return Display;
        }

        public void RequeryHierarchySource()
        {
            try
            {
                if (this.iUserControlQueryCondition != null)
                {
                    if (this.iUserControlQueryCondition.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryConditionHierarchy))
                    {
                        DiversityWorkbench.UserControls.UserControlQueryConditionHierarchy U = (DiversityWorkbench.UserControls.UserControlQueryConditionHierarchy)this.iUserControlQueryCondition;
                        System.Data.DataTable dt = new DataTable();
                        string ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                        if (this.ServerConnection != null && this.ServerConnection.ConnectionString != null && this.ServerConnection.ConnectionString.Length > 0)
                            ConnectionString = this.ServerConnection.ConnectionString;
                        string SQL = "SELECT NULL AS " + this.HierarchyColumn + ", NULL AS " + this.HierarchyParentColumn + ", NULL AS " + this.DisplayColumn + ", NULL AS " + this.HierarchyDisplayColumn +
                            " UNION " +
                            " SELECT " + this.HierarchyColumn + ", " + this.HierarchyParentColumn + ", " + this.DisplayColumn + ", " + this.HierarchyDisplayColumn + " FROM " + this.Source +
                            " ORDER BY " + this.HierarchyDisplayColumn;
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionString);
                        ad.Fill(dt);
                        if (dt.Columns.Count == 0)
                        {
                            System.Data.DataColumn Value = new System.Data.DataColumn(this.HierarchyColumn);
                            System.Data.DataColumn ParentValue = new System.Data.DataColumn(this.HierarchyParentColumn);
                            System.Data.DataColumn Display = new System.Data.DataColumn(this.DisplayColumn);
                            System.Data.DataColumn HierarchyDisplay = new System.Data.DataColumn(this.HierarchyDisplayColumn);
                            dt.Columns.Add(Value);
                            dt.Columns.Add(ParentValue);
                            dt.Columns.Add(Display);
                            dt.Columns.Add(HierarchyDisplay);
                        }
                        this.dtValues = dt;
                        U.buildHierarchy();
                    }
                    else if (this.iUserControlQueryCondition.GetType() == typeof(DiversityWorkbench.UserControls.UserControlQueryCondition))
                    {
                        this.iUserControlQueryCondition.RefreshProjectDependentHierarchy();
                    }
                }
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

    }

}
