using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Export
{
    public interface iExporter
    {
        void InitSourceTables();
        void MarkCurrentSourceTable(Export.Table Table);
        void ShowCurrentSourceTableColumns(Export.Table Table);
        void AddSourceTable(Export.Table Table, Export.Table TargetTable);
        void AddMultiSourceTables(Export.Table Table, Export.Table TargetTable);
        void RemoveSourceTable(Export.Table Table);
        void ShowFileColumns();
        void ShowExportProgress(int Position, int Max, string Message);
        //void EnableTemplateTable(Export.Table Table, bool IsEnabled);
    }

    public partial class Exporter : Component
    {
        #region Construction

        public Exporter(DiversityWorkbench.Export.iExporter iExporter)
        {
            InitializeComponent();
            _iExporter = iExporter;
        }

        public Exporter(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        #endregion

        #region Reset

        public static void ResetAll()
        {
            Exporter._TemplateTables = null;
            Exporter._TemplateTableDictionary = null;
            Exporter._StartTableIdColumn = null;
            Exporter._StartTable = null;
            Exporter._iExporter = null;
            Exporter._ListOfIDs = null;
            Exporter.ResetUserActions();
        }

        public static void ResetUserActions()
        {
            Exporter._SourceTables = null;
            Exporter._SourceTableList = null;
            Exporter._FileColumnList = null;
            Exporter._FileColumns = null;
            Exporter._ExportTables = null;
            Exporter._ExportResults = null;
        }

        #endregion

        #region File columns

        private static System.Collections.Generic.List<Export.FileColumn> _FileColumns;
        /// <summary>
        /// the file columns as defined by the user
        /// </summary>
        public static System.Collections.Generic.List<Export.FileColumn> FileColumns
        {
            get
            {
                if (Exporter._FileColumns == null)
                    Exporter._FileColumns = new List<FileColumn>();
                return Exporter._FileColumns;
            }
            set { Exporter._FileColumns = value; }
        }

        private static System.Collections.Generic.SortedList<int, Export.FileColumn> _FileColumnList;
        /// <summary>
        /// The file columns sorted by the position
        /// </summary>
        public static System.Collections.Generic.SortedList<int, Export.FileColumn> FileColumnList
        {
            get
            {
                if (_FileColumnList == null)
                {
                    _FileColumnList = new SortedList<int, FileColumn>();
                    foreach (Export.FileColumn C in Exporter.FileColumns)
                    {
                        if (!_FileColumnList.ContainsKey(C.Position))
                            _FileColumnList.Add(C.Position, C);
                        else
                        {
                            int Offset = 1;
                            while (_FileColumnList.ContainsKey(C.Position + Offset))
                                Offset = Offset + 1;
                            _FileColumnList.Add(C.Position + Offset, C);
                        }
                    }
                }
                return _FileColumnList;
            }
        }

        //private static System.Collections.Generic.SortedList<double, Export.FileColumn> _FileColumnList;
        ///// <summary>
        ///// The file columns sorted by the position
        ///// </summary>
        //public static System.Collections.Generic.SortedList<double, Export.FileColumn> FileColumnList
        //{
        //    get
        //    {
        //        if (_FileColumnList == null)
        //        {
        //            _FileColumnList = new SortedList<double, FileColumn>();
        //            foreach (Export.FileColumn C in Exporter.FileColumns)
        //            {
        //                if (!_FileColumnList.ContainsKey(C.Position))
        //                    _FileColumnList.Add(C.Position, C);
        //                else
        //                {
        //                    double Offset = 0.1;
        //                    while (_FileColumnList.ContainsKey(C.Position + Offset))
        //                        Offset = Offset / 10;
        //                    _FileColumnList.Add(C.Position + Offset, C);
        //                }
        //            }
        //        }
        //        return _FileColumnList;
        //    }
        //}

        public static void ResetFileColumnList()
        {
            Exporter._FileColumnList = null;
        }

        #endregion

        #region Interface

        private static DiversityWorkbench.Export.iExporter _iExporter;

        public static DiversityWorkbench.Export.iExporter IExporter
        {
            get { return Exporter._iExporter; }
            set { Exporter._iExporter = value; }
        }

        #endregion

        #region Templates for tables

        private static System.Collections.Generic.List<Export.Table> _TemplateTables;
        /// <summary>
        /// The potential tables for the export as available in the database
        /// </summary>
        public static System.Collections.Generic.List<Export.Table> TemplateTables
        {
            get
            {
                if (_TemplateTables == null)
                    _TemplateTables = new List<Table>();
                return _TemplateTables;
            }
            set { _TemplateTables = value; }
        }

        private static System.Collections.Generic.Dictionary<string, Export.Table> _TemplateTableDictionary;
        /// <summary>
        /// A dictionary for all template tables listed with the names as found in the database
        /// </summary>
        public static System.Collections.Generic.Dictionary<string, Export.Table> TemplateTableDictionary
        {
            get
            {
                if (Exporter._TemplateTableDictionary == null)
                {
                    Exporter._TemplateTableDictionary = new Dictionary<string, Table>();
                    foreach (Export.Table T in Exporter.TemplateTables)
                    {
                        if (!Exporter.TemplateTableDictionary.ContainsKey(T.TableName))
                            Exporter.TemplateTableDictionary.Add(T.TableName, T);
                    }
                }
                return Exporter._TemplateTableDictionary;
            }
            //set { Exporter._TemplateTableDictionary = value; }
        }

        public static Export.Table AddTemplateTable(string TableName,
            string DisplayText,
            Table.Parallelity TypeOfParallelity,
            System.Drawing.Image Image,
            Export.Table ParentTable,
            System.Collections.Generic.List<string> SortingColumns,
            System.Collections.Generic.Dictionary<string, string> ModuleConnections,
            string Restriction)
        {
            // for testing only
            if (TableName == "CollectionEventMethod")
            { }


            // Creating the template
            Export.Table T = new Table(TableName, DisplayText, TypeOfParallelity, SortingColumns, ModuleConnections, Restriction);
            if (ParentTable != null)
                T.ParentTable = ParentTable;
            int ParallelPosition = 1;
            if (TypeOfParallelity == Table.Parallelity.unique)
                T.setParallelPosition(1);
            else
            {
                foreach (Export.Table TT in Exporter.TemplateTables)
                {
                    if (T.TableName == TT.TableName && ParallelPosition <= TT.ParallelPosition)
                        ParallelPosition = TT.ParallelPosition + 1;
                }
                T.setParallelPosition(ParallelPosition);
            }
            T.Image = Image;
            Exporter.TemplateTables.Add(T);

            int ParallelPositionOfParent = 1;
            if (T.ParentTable != null)
                ParallelPositionOfParent = T.ParentTable.ParallelPosition;

            // Creating the source as a copy of the template
            Export.Table S = new Table(TableName, DisplayText, TypeOfParallelity, SortingColumns, ModuleConnections, Restriction);
            if (ParentTable != null)
            {
                Export.Table SP = Exporter.getSourceTable(ParentTable.TableName, ParallelPositionOfParent);
                S.ParentTable = SP;
                S.Template = T.TableAlias;
            }
            S.SqlRestriction = Restriction;
            S.setParallelPosition(ParallelPosition);
            S.Image = Image;
            try
            {
                Exporter.SourceTables.Add(S.TableAlias, S);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return T;
        }

        public static Export.Table AddTemplateTable(string TableName,
            string DisplayText,
            Table.Parallelity TypeOfParallelity,
            System.Drawing.Image Image,
            Export.Table ParentTable,
            System.Collections.Generic.List<string> SortingColumns,
            System.Collections.Generic.Dictionary<string, string> ModuleConnections,
            string Restriction,
            System.Collections.Generic.Dictionary<string, string> JoinColumns,
            System.Collections.Generic.Dictionary<string, string> ForeignRelationTables)
        {
            // for testing only
            if (TableName == "IdentificationUnit")
            { }


            // Creating the template
            Export.Table T = new Table(TableName, DisplayText, TypeOfParallelity, SortingColumns, ModuleConnections, Restriction);
            if (ParentTable != null)
                T.ParentTable = ParentTable;
            if (JoinColumns.Count > 0)
            {
                T.JoinColumns = JoinColumns;
            }
            if (ForeignRelationTables == null)
                ForeignRelationTables = new Dictionary<string, string>();
            if (ForeignRelationTables.Count > 0)
            {
                T.ColumnForeignRelationTables = ForeignRelationTables;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ForeignRelationTables)
                {
                    T.TableColumns[KV.Key].ForeignRelationTable = KV.Value;
                }
            }
            int ParallelPosition = 1;
            if (TypeOfParallelity == Table.Parallelity.unique)
                T.setParallelPosition(1);
            else
            {
                foreach (Export.Table TT in Exporter.TemplateTables)
                {
                    if (T.TableName == TT.TableName && ParallelPosition <= TT.ParallelPosition)
                        ParallelPosition = TT.ParallelPosition + 1;
                }
                T.setParallelPosition(ParallelPosition);
            }
            T.Image = Image;
            Exporter.TemplateTables.Add(T);

            int ParallelPositionOfParent = 1;
            if (T.ParentTable != null)
                ParallelPositionOfParent = T.ParentTable.ParallelPosition;

            // Creating the source as a copy of the template
            Export.Table S = new Table(TableName, DisplayText, TypeOfParallelity, SortingColumns, ModuleConnections, Restriction);
            if (ParentTable != null)
            {
                Export.Table SP = Exporter.getSourceTable(ParentTable.TableName, ParallelPositionOfParent);
                S.ParentTable = SP;
                S.Template = T.TableAlias;
            }
            S.SqlRestriction = Restriction;
            S.setParallelPosition(ParallelPosition);
            S.Image = Image;
            if (T.JoinColumns.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in T.JoinColumns)
                    S.JoinColumns.Add(KV.Key, KV.Value);
            }
            if (ForeignRelationTables.Count > 0)
            {
                S.ColumnForeignRelationTables = ForeignRelationTables;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in ForeignRelationTables)
                {
                    S.TableColumns[KV.Key].ForeignRelationTable = KV.Value;
                }
            }
            try
            {
                Exporter.SourceTables.Add(S.TableAlias, S);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return T;
        }

        public static Export.Table AddTemplateTable(string TableName,
            string DisplayText,
            Table.Parallelity TypeOfParallelity,
            System.Drawing.Image Image,
            Export.Table ParentTable,
            System.Collections.Generic.List<string> SortingColumns,
            System.Collections.Generic.Dictionary<string, string> ModuleConnections,
            string Restriction,
            System.Collections.Generic.List<string> SupressedColumns,
            System.Collections.Generic.Dictionary<string, string> AddedColumns = null)
        {

            // Creating the template
            Export.Table T = new Table(TableName, DisplayText, TypeOfParallelity, SortingColumns, ModuleConnections, Restriction);
            if (ParentTable != null)
                T.ParentTable = ParentTable;
            int ParallelPosition = 1;
            if (TypeOfParallelity == Table.Parallelity.unique)
                T.setParallelPosition(1);
            else
            {
                foreach (Export.Table TT in Exporter.TemplateTables)
                {
                    if (T.TableName == TT.TableName && ParallelPosition <= TT.ParallelPosition)
                        ParallelPosition = TT.ParallelPosition + 1;
                }
                T.setParallelPosition(ParallelPosition);
            }
            T.setSuppressedColumns(SupressedColumns);
            T.setAddedColumns(AddedColumns);
            T.Image = Image;
            Exporter.TemplateTables.Add(T);

            int ParallelPositionOfParent = 1;
            if (T.ParentTable != null)
                ParallelPositionOfParent = T.ParentTable.ParallelPosition;

            // Creating the source as a copy of the template
            Export.Table S = new Table(TableName, DisplayText, TypeOfParallelity, SortingColumns, ModuleConnections, Restriction);
            S.setSuppressedColumns(SupressedColumns);
            if (ParentTable != null)
            {
                Export.Table SP = Exporter.getSourceTable(ParentTable.TableName, ParallelPositionOfParent);
                S.ParentTable = SP;
                S.Template = T.TableAlias;
            }
            S.SqlRestriction = Restriction;
            S.setParallelPosition(ParallelPosition);
            S.setAddedColumns(AddedColumns);
            S.Image = Image;
            try
            {
                Exporter.SourceTables.Add(S.TableAlias, S);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            return T;
        }


        public static void InitSourceTables()
        {
            Exporter.SourceTables.Clear();
            foreach (DiversityWorkbench.Export.Table T in Export.Exporter.TemplateTables)
            {
                System.Collections.Generic.List<string> SortingColumns = new List<string>();
                if (T.SortingColumnList().Count > 0)
                {
                    foreach (string s in T.SortingColumnList())
                        SortingColumns.Add(s);
                }
                DiversityWorkbench.Export.Table S = new Table(T.TableName, T.DisplayText, T.TypeOfParallelity, SortingColumns, T.ModuleConnections(), T.SqlRestriction);
                if (T.ParentTable != null)
                {
                    Export.Table SP = Exporter.getSourceTable(T.ParentTable.TableName, T.ParentTable.ParallelPosition);
                    S.ParentTable = SP;
                    S.Template = T.TableAlias;
                }
                S.SqlRestriction = T.SqlRestriction;
                S.setParallelPosition(T.ParallelPosition);
                S.Image = T.Image;
                if (T.JoinColumns.Count > 0)
                {
                    if (T.TypeOfParallelity == Table.Parallelity.referencing)
                    {
                        bool JoinColumnsFound = false;
                        // hier fehlen die Tabellen fuer Annotation noch
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.Table> KV in DiversityWorkbench.Export.Exporter.SourceTableDictionary())
                        {
                            if (KV.Value.TableName == S.TableName && KV.Value.ParentTable != null && KV.Value.ParentTable.TableAlias == S.ParentTable.TableAlias)
                            {
                                S.SqlRestriction = KV.Value.SqlRestriction;
                                foreach (System.Collections.Generic.KeyValuePair<string, string> KVJoin in KV.Value.JoinColumns)
                                    S.JoinColumns.Add(KVJoin.Key, KVJoin.Value);
                                JoinColumnsFound = true;
                            }
                        }
                        if (!JoinColumnsFound) // fuer annotation braucht man mehrere Templates oder eine andere Loesung - in den Templates ist nur das fuer den Event, hilft hier nix
                        {
                            if (DiversityWorkbench.Export.Exporter.TemplateTableDictionary.ContainsKey(S.TableName))
                            {
                                if (DiversityWorkbench.Export.Exporter.TemplateTableDictionary[S.TableName].ParentTable.TableAlias == S.ParentTable.TableAlias)
                                {
                                    T.JoinColumns = DiversityWorkbench.Export.Exporter.TemplateTableDictionary[T.TableName].JoinColumns;
                                    JoinColumnsFound = true;
                                }
                            }
                        }
                        if (!JoinColumnsFound)
                        {
                            foreach (DiversityWorkbench.Export.Table TT in DiversityWorkbench.Export.Exporter.TemplateTables)
                            {
                                if (TT.TableName == S.TableName && TT.ParentTable != null && TT.ParentTable.TableAlias == S.ParentTable.TableAlias)
                                {
                                    S.JoinColumns = TT.JoinColumns;
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in T.JoinColumns)
                            S.JoinColumns.Add(KV.Key, KV.Value);
                    }
                }
                if (T.ColumnForeignRelationTables.Count > 0)
                {
                    S.ColumnForeignRelationTables = T.ColumnForeignRelationTables;
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in T.ColumnForeignRelationTables)
                    {
                        S.TableColumns[KV.Key].ForeignRelationTable = KV.Value;
                    }
                }
                Exporter.SourceTables.Add(S.TableAlias, S);
            }
        }

        #endregion

        #region IDs

        private static System.Collections.Generic.List<int> _ListOfIDs;
        /// <summary>
        /// List of IDs restricting the PK of the StartTable
        /// </summary>
        public static System.Collections.Generic.List<int> ListOfIDs
        {
            get { return _ListOfIDs; }
            set { _ListOfIDs = value; }
        }

        private static string IDs()
        {
            string sID = "";
            foreach (int i in Exporter.ListOfIDs)
            {
                if (sID.Length > 0) sID += ", ";
                sID += i.ToString();
            }
            return sID;
        }

        private static string _StartTableIdColumn;
        public static string StartTableIdColumn()
        {
            if (Export.Exporter._StartTableIdColumn == null)
            {
                if (Export.Exporter._StartTable.IdentityColumn.Length > 0)
                    Export.Exporter._StartTableIdColumn = Export.Exporter._StartTable.IdentityColumn;
                else if (Export.Exporter._StartTable.PrimaryKeyColumnList.Count == 1)
                    Export.Exporter._StartTableIdColumn = Export.Exporter._StartTable.PrimaryKeyColumnList[0];
                else
                {
                    System.Windows.Forms.MessageBox.Show("The start table has no valid ID column");
                }
            }
            return Export.Exporter._StartTableIdColumn;
        }

        #endregion

        #region Tables

        private static Export.Table _StartTable;
        /// <summary>
        /// The center table of the export
        /// </summary>
        public static Export.Table StartTable
        {
            get { return _StartTable; }
            set
            {
                _StartTable = value;
                _Target = _StartTable.TableName;
            }
        }

        private static Export.Table _ProjectTable;
        /// <summary>
        /// The project table as defined in the database
        /// </summary>
        public static Export.Table ProjectTable
        {
            get { return _ProjectTable; }
            set
            {
                _ProjectTable = value;
            }
        }

        //private System.Collections.Generic.SortedDictionary<string, Export.Table> _SourceTables;
        ///// <summary>
        ///// The tables for export as selected by the user
        ///// </summary>
        //public System.Collections.Generic.SortedDictionary<string, Export.Table> SourceTables
        //{
        //    get 
        //    {
        //        if (this._SourceTables == null)
        //        {
        //            this._SourceTables = new SortedDictionary<string, Table>();
        //            if (this.StartTable != null)
        //            {
        //                this._SourceTables.Add(this.StartTable.PositionKey, this.StartTable);
        //            }
        //        }
        //        return _SourceTables; 
        //    }
        //    set { _SourceTables = value; }
        //}

        private static System.Collections.Generic.Dictionary<string, Export.Table> _SourceTables;
        /// <summary>
        /// The tables for export as selected by the user
        /// </summary>
        private static System.Collections.Generic.Dictionary<string, Export.Table> SourceTables
        {
            get
            {
                if (Exporter._SourceTables == null)
                {
                    Exporter._SourceTables = new Dictionary<string, Table>();
                }
                return _SourceTables;
            }
            //set { _SourceTables = value; }
        }


        //private static System.Collections.Generic.List<Export.Table> _SourceTables;
        ///// <summary>
        ///// The tables for export as selected by the user
        ///// </summary>
        //private static System.Collections.Generic.List<Export.Table> SourceTables
        //{
        //    get
        //    {
        //        if (Exporter._SourceTables == null)
        //        {
        //            Exporter._SourceTables = new List<Table>();
        //        }
        //        return _SourceTables;
        //    }
        //    //set { _SourceTables = value; }
        //}

        public static void AddMultiSourceTables(Export.Table Template, Export.Table Parent)
        {
            int ParallelData = Template.NumberOfParallelData();
            if (ParallelData > 9)
            {
                if (System.Windows.Forms.MessageBox.Show("Do you want to add " + ParallelData.ToString() + " parallels?", "Add " + ParallelData.ToString() + " parallels?", System.Windows.Forms.MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
                    return;
            }
            for (int i = Template.NumberOfParallelSourceTables(); i < ParallelData; i++)
            {
                Export.Exporter.AddSourceTable(Template, Parent);
            }
        }

        public static void AddSourceTable(Export.Table Template, Export.Table Parent)
        {
            int ParallelPosition = 1;
            string SqlRestriction = "";
            foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> KV in Exporter.SourceTables)
            {
                if (KV.Value.TypeOfParallelity == Table.Parallelity.referencing && Template.TypeOfParallelity == Table.Parallelity.referencing)
                {
                    if (ParallelPosition <= KV.Value.ParallelPosition) // only if ParallelPosition must be increased
                    {
                        if (KV.Value.ParentTable != null) // if a parent table is present, allways true for referenced table
                        {
                            if (KV.Value.ParentTable == Parent) // if the parent is the same 
                            {
                                SqlRestriction = KV.Value.SqlRestriction;
                            }
                            ParallelPosition = KV.Value.ParallelPosition + 1;
                        }
                    }
                }
                else if (KV.Value.TableName == Template.TableName) // only for parallels
                {
                    if (ParallelPosition <= KV.Value.ParallelPosition) // only if ParallelPosition must be increased
                    {
                        if (KV.Value.ParentTable != null) // if a parent table is present
                        {
                            if (KV.Value.ParentTable == Parent) // if the parent is the same 
                                ParallelPosition = KV.Value.ParallelPosition + 1;
                        }
                        else if (KV.Value.HasInternalRelations) // the current table has no parent but an interal relation is possible (e.g. top EventSeries)
                        {
                            ParallelPosition = KV.Value.ParallelPosition + 1;
                        }
                    }
                }
            }

            Export.Table S;
            if (Template.TypeOfParallelity == Table.Parallelity.referencing)
            {
                string DisplayText = "";
                if (Template.TableName.StartsWith("Annotation"))
                    DisplayText = "Annotation for " + Parent.DisplayText.ToLower();
                else
                    DisplayText = Template.TableName + " for " + Parent.DisplayText.ToLower();
                S = new Table(Template.TableName, DisplayText, Template.TypeOfParallelity, Template.SortingColumnList(), Template.ModuleConnections(), SqlRestriction);
            }
            else
                S = new Table(Template.TableName, Template.DisplayText, Template.TypeOfParallelity, Template.SortingColumnList(), Template.ModuleConnections(), Template.SqlRestriction);

            if (Template.TypeOfParallelity != Table.Parallelity.unique)
            {
                S.ParentTable = Parent;
                S.TypeOfJoin = Table.JoinType.Left;
            }
            //else if (Template.HasInternalRelations && Template.TypeOfParallelity == Table.Parallelity.unique)
            //{
            //    S.ParentTable = Parent;
            //    S.TypeOfJoin = Table.JoinType.Left;
            //}
            else
            {
                S.ParentTable = Parent.ParentTable;
                Parent.ParentTable = S;
                Parent.TypeOfJoin = Table.JoinType.Left;
            }
            if (Template.SuppressedColumns() != null && Template.SuppressedColumns().Count > 0)
                S.setSuppressedColumns(Template.SuppressedColumns());
            if (Template.AddedColumns() != null && Template.AddedColumns().Count > 0)
            {
                S.setAddedColumns(Template.AddedColumns());
            }
            S.setParallelPosition(ParallelPosition);
            S.Image = Template.Image;
            if (Template.JoinColumns.Count > 0)
            {
                if (Template.TypeOfParallelity == Table.Parallelity.referencing)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> KV in Exporter.SourceTables)
                    {
                        if (Template.TableName == KV.Value.TableName && KV.Value.ParentTable.TableAlias == Parent.TableAlias)
                        {
                            S.JoinColumns = KV.Value.JoinColumns;
                            break;
                        }
                    }
                }
                else
                    S.JoinColumns = Template.JoinColumns;
            }
            if (Template.ColumnForeignRelationTables.Count > 0)
            {
                S.ColumnForeignRelationTables = Template.ColumnForeignRelationTables;
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Template.ColumnForeignRelationTables)
                {
                    S.TableColumns[KV.Key].ForeignRelationTable = KV.Value;
                }
            }

            System.Collections.Generic.Dictionary<string, string> PotenialParentColumn = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> KV in Template.TableColumns)
            {
                if (KV.Value.ParentColumn != null)
                {
                    PotenialParentColumn.Add(KV.Key, KV.Key);
                }
            }
            if (PotenialParentColumn.Count > 1 && Template.TableName == Parent.TableName)
            {
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(PotenialParentColumn, "Relation to parent table", "Please select the column for the relation to the parent column", true);
                f.ShowDialog();
                if (f.DialogResult != System.Windows.Forms.DialogResult.OK)
                    return;
                S.InternalRelationJoinColumn = f.SelectedString;
            }
            try
            {
                Exporter.SourceTables.Add(S.TableAlias, S);
                if (Template.TypeOfParallelity == Table.Parallelity.parallel)
                    Exporter.AddSourceTableChildren(S);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public static void AddSourceTable(Export.Table Template, Export.Table Parent, string InternalJoinColumn)
        {
            int ParallelPosition = 1;

            foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> KV in Exporter.SourceTables)
            {
                if (KV.Value.TableName == Template.TableName && ParallelPosition <= KV.Value.ParallelPosition && KV.Value.ParentTable == Parent)
                    ParallelPosition = KV.Value.ParallelPosition + 1;
            }

            Export.Table S = new Table(Template.TableName, Template.DisplayText, Template.TypeOfParallelity, Template.SortingColumnList(), Template.ModuleConnections(), Template.SqlRestriction);
            if (Template.TypeOfParallelity != Table.Parallelity.unique)
            {
                S.ParentTable = Parent;
                S.TypeOfJoin = Table.JoinType.Left;
            }
            else
            {
                S.ParentTable = Parent.ParentTable;
                Parent.ParentTable = S;
                Parent.TypeOfJoin = Table.JoinType.Left;
            }
            if (Template.SuppressedColumns() != null && Template.SuppressedColumns().Count > 0)
                S.setSuppressedColumns(Template.SuppressedColumns());
            if (Template.AddedColumns() != null && Template.AddedColumns().Count > 0)
                S.setAddedColumns(Template.AddedColumns());
            S.setParallelPosition(ParallelPosition);
            S.Image = Template.Image;
            System.Collections.Generic.Dictionary<string, string> PotenialParentColumn = new Dictionary<string, string>();
            foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> KV in Template.TableColumns)
            {
                if (KV.Value.ParentColumn != null)
                {
                    PotenialParentColumn.Add(KV.Key, KV.Key);
                }
            }
            if (InternalJoinColumn.Length > 0)
            {
                S.InternalRelationJoinColumn = InternalJoinColumn;
            }
            if (Template.TypeOfParallelity == Table.Parallelity.parallel)
                Exporter.AddSourceTableChildren(S);
            Exporter.SourceTables.Add(S.TableAlias, S);
        }

        private static void AddSourceTableChildren(Export.Table Parent)
        {
            foreach (Export.Table T in Exporter.TemplateTables)
            {
                if (T.ParentTable != null
                    && T.ParentTable.TableName == Parent.TableName
                    && Parent.TypeOfParallelity == Table.Parallelity.parallel
                    && (T.TypeOfParallelity == Table.Parallelity.parallel
                    || T.TypeOfParallelity == Table.Parallelity.restricted
                    || T.TypeOfParallelity == Table.Parallelity.referencing))
                {
                    Export.Table C = new Table(T.TableName, T.DisplayText, T.TypeOfParallelity, T.SortingColumnList(), T.ModuleConnections(), T.SqlRestriction);
                    C.ParentTable = Parent;
                    int ParallelPosition = 1;
                    if (T.TypeOfParallelity == Table.Parallelity.parallel ||
                        ((T.TypeOfParallelity == Table.Parallelity.restricted || T.TypeOfParallelity == Table.Parallelity.referencing)
                        && T.ParentTable != null && T.ParentTable.TypeOfParallelity == Table.Parallelity.parallel))
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> TT in Exporter.SourceTables)
                        {
                            if (TT.Value.TableName == C.TableName && ParallelPosition <= TT.Value.ParallelPosition && TT.Value.ParentTable == Parent)
                                ParallelPosition = TT.Value.ParallelPosition + 1;
                        }
                    }
                    C.setParallelPosition(ParallelPosition);
                    C.Image = T.Image;
                    C.TypeOfJoin = Table.JoinType.Left;
                    if (!Exporter.SourceTables.ContainsKey(C.TableAlias))
                    {
                        Exporter.SourceTables.Add(C.TableAlias, C);
                        if (T.TypeOfParallelity == Table.Parallelity.parallel)
                            Exporter.AddSourceTableChildren(C);
                    }
                    else
                    {
                        // should not happen
                    }
                }
                else if (T.ParentTable != null
                    && T.ParentTable.TableName == Parent.TableName
                    && Parent.TypeOfParallelity == Table.Parallelity.parallel
                    && T.TypeOfParallelity == Table.Parallelity.referencing)
                {
                    Export.Table C = new Table(T.TableName, T.DisplayText, T.TypeOfParallelity, T.SortingColumnList(), T.ModuleConnections(), T.SqlRestriction);
                    C.ParentTable = Parent;
                    int ParallelPosition = Parent.ParallelPosition;
                    C.setParallelPosition(ParallelPosition);
                    C.Image = T.Image;
                    C.TypeOfJoin = Table.JoinType.Left;
                    Exporter.SourceTables.Add(C.TableAlias, C);
                    if (T.TypeOfParallelity == Table.Parallelity.parallel)
                        Exporter.AddSourceTableChildren(C);
                }
            }
        }

        public static Export.Table getSourceTable(string TableName, int Position)
        {
            Export.Table T = new Table("", "", Table.Parallelity.unique, null, null, "");
            foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> tt in Exporter.SourceTables)
            {
                if (tt.Value.TableName == TableName
                    && (tt.Value.ParallelPosition == Position))//                    || (tt.Value.ParallelPosition == 1 && tt.Value.TypeOfParallelity == Table.Parallelity.unique && tt.Value.HasInternalRelations == false)))
                {
                    T = tt.Value;
                    break;
                }
            }
            return T;
        }

        public static void RemoveSourceTable(Export.Table Table)
        {
            try
            {
                if (Table.ParentTable != null
                    && Table.TypeOfParallelity == Export.Table.Parallelity.unique
                    && Table.TableName == Table.ParentTable.TableName)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> T in Exporter.SourceTables)
                    {
                        if (T.Value.ParentTable == Table)
                            T.Value.ParentTable = Table.ParentTable;
                    }
                }
                if (Table.TypeOfParallelity == Export.Table.Parallelity.parallel)
                {
                    Exporter.RemoveSourceTableChildren(Table);
                }
                Exporter.SourceTables.Remove(Table.TableAlias);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void RemoveSourceTableChildren(Export.Table ParentTable)
        {
            try
            {
                System.Collections.Generic.List<Export.Table> TablesToDelete = new List<Table>();
                foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> T in Exporter.SourceTables)
                {
                    if (T.Value.ParentTable == ParentTable)
                    {
                        //Exporter.RemoveSourceTableChildren(T.Value);
                        TablesToDelete.Add(T.Value);
                    }
                }
                foreach (Export.Table T in TablesToDelete)
                {
                    Exporter.RemoveSourceTableChildren(T);
                    Exporter.SourceTables.Remove(T.TableAlias);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public static void ResetSourceTableList()
        {
            Export.Exporter._SourceTableList = null;
        }
        private static System.Collections.Generic.List<Export.Table> _SourceTableList;
        public static System.Collections.Generic.List<Export.Table> SourceTableList()
        {
            if (Export.Exporter._SourceTableList == null)
            {
                Export.Exporter._SourceTableList = new List<Table>();
                try
                {
                    // finding the top table
                    foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> T in Exporter.SourceTables)
                    {
                        if (T.Value.ParentTable == null && !Export.Exporter._SourceTableList.Contains(T.Value))
                        {
                            Export.Exporter._SourceTableList.Add(T.Value);
                            Export.Exporter.AddSourceTableChildren(T.Value, ref Export.Exporter._SourceTableList);
                            Export.Exporter.AddSourceTableParallels(T.Value, ref Export.Exporter._SourceTableList);
                            break;
                        }
                    }

                    //foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> T in Exporter.SourceTables)
                    //{
                    //    if (!Export.Exporter._SourceTableList.Contains(T.Value))
                    //    {
                    //        Export.Exporter._SourceTableList.Add(T.Value);
                    //        Export.Exporter.AddSourceTableChildren(T.Value, ref Export.Exporter._SourceTableList);
                    //        Export.Exporter.AddSourceTableParallels(T.Value, ref Export.Exporter._SourceTableList);
                    //    }
                    //    if (Exporter.SourceTables.Count <= Export.Exporter._SourceTableList.Count)
                    //        break;
                    //}
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return Export.Exporter._SourceTableList;
        }

        private static void AddSourceTableChildren(Export.Table ParentTable, ref System.Collections.Generic.List<Export.Table> TableList)
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> T in Exporter.SourceTables)
                {
                    if (T.Value.ParentTable != null
                        && T.Value.ParentTable.TableAlias == ParentTable.TableAlias
                        && !TableList.Contains(T.Value))
                    {
                        TableList.Add(T.Value);
                        Export.Exporter.AddSourceTableChildren(T.Value, ref TableList);
                        Export.Exporter.AddSourceTableParallels(T.Value, ref TableList);
                    }
                }

                //// add internal relations first
                //foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> T in Exporter.SourceTables)
                //{
                //    if (ParentTable.HasInternalRelations
                //        && T.Value.ParentTable != null
                //        && T.Value.TableName == ParentTable.TableName
                //        && T.Value.ParentTable.TableAlias == ParentTable.TableAlias
                //        && !TableList.Contains(T.Value))
                //    {
                //        TableList.Add(T.Value);
                //        Export.Exporter.AddSourceTableChildren(T.Value, ref TableList);
                //        Export.Exporter.AddSourceTableParallels(T.Value, ref TableList);
                //    }
                //}
                //// add rest of children
                //foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> T in Exporter.SourceTables)
                //{
                //    if (T.Value.ParentTable != null
                //        && T.Value.TableName != ParentTable.TableName
                //        && T.Value.ParentTable.TableAlias == ParentTable.TableAlias
                //        && !TableList.Contains(T.Value))
                //    {
                //        TableList.Add(T.Value);
                //        Export.Exporter.AddSourceTableChildren(T.Value, ref TableList);
                //        Export.Exporter.AddSourceTableParallels(T.Value, ref TableList);
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void AddSourceTableParallels(Export.Table ParallelTable, ref System.Collections.Generic.List<Export.Table> TableList)
        {
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> T in Exporter.SourceTables)
                {
                    if (T.Value.ParentTable != null
                        && ParallelTable.ParentTable != null
                        && T.Value.ParentTable.TableAlias == ParallelTable.ParentTable.TableAlias
                        && T.Value.TableName == ParallelTable.TableName
                        && !TableList.Contains(T.Value))
                    {
                        TableList.Add(T.Value);
                        Export.Exporter.AddSourceTableChildren(T.Value, ref TableList);
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public static System.Collections.Generic.Dictionary<string, Export.Table> SourceTableDictionary()
        {
            return Exporter.SourceTables;
            //System.Collections.Generic.Dictionary<string, Export.Table> D = new Dictionary<string, Table>();
            //foreach (Export.Table T in Exporter.SourceTables)
            //    D.Add(T.TableAlias, T);
            //return D;
        }

        //private static System.Collections.Generic.SortedDictionary<string, Export.Table> _SourceTableList;
        ///// <summary>
        ///// The tables for export as selected by the user
        ///// </summary>
        //public static System.Collections.Generic.SortedDictionary<string, Export.Table> SourceTableList
        //{
        //    get
        //    {
        //        if (Exporter._SourceTableList == null)
        //        {
        //            Exporter._SourceTableList = new SortedDictionary<string, Table>();
        //            foreach Export.Table T in Exporter.SourceTables)
        //                Exporter._SourceTableList.Add(T.PositionKey, KV.Value);
        //        }
        //        return _SourceTableList;
        //    }
        //    //set { _SourceTables = value; }
        //}


        //private static System.Collections.Generic.Dictionary<string, Export.Table> _SourceTables;
        ///// <summary>
        ///// The tables for export as selected by the user
        ///// </summary>
        //public static System.Collections.Generic.Dictionary<string, Export.Table> SourceTables
        //{
        //    get
        //    {
        //        if (Exporter._SourceTables == null)
        //        {
        //            Exporter._SourceTables = new Dictionary<string, Table>();
        //            if (Exporter.StartTable != null)
        //            {
        //                Exporter._SourceTables.Add(Exporter.StartTable.TableAlias, Exporter.StartTable);
        //            }
        //        }
        //        return _SourceTables;
        //    }
        //    set { _SourceTables = value; }
        //}

        //private static System.Collections.Generic.SortedDictionary<string, Export.Table> _SourceTableList;
        ///// <summary>
        ///// The tables for export as selected by the user
        ///// </summary>
        //public static System.Collections.Generic.SortedDictionary<string, Export.Table> SourceTableList
        //{
        //    get
        //    {
        //        if (Exporter._SourceTableList == null)
        //        {
        //            Exporter._SourceTableList = new SortedDictionary<string, Table>();
        //            foreach (System.Collections.Generic.KeyValuePair<string, Export.Table> KV in Exporter.SourceTables)
        //                Exporter._SourceTableList.Add(KV.Value.PositionKey, KV.Value);
        //        }
        //        return _SourceTableList;
        //    }
        //    //set { _SourceTables = value; }
        //}

        //public static void ResetSourceTableList() { Exporter._SourceTableList = null; }


        private static DiversityWorkbench.Export.Table _CurrentSourceTable;

        public static DiversityWorkbench.Export.Table CurrentSourceTable
        {
            get { return _CurrentSourceTable; }
            set
            {
                _CurrentSourceTable = value;
                if (Exporter.IExporter != null)
                {
                    Exporter.IExporter.MarkCurrentSourceTable(_CurrentSourceTable);
                    Exporter.IExporter.ShowCurrentSourceTableColumns(_CurrentSourceTable);
                }
            }
        }

        #endregion

        #region Schema

        #region Common

        private static string _SchemaName;
        /// <summary>
        /// The XML schema containing the settings for the import
        /// </summary>
        public static string SchemaName
        {
            get
            {
                if (_SchemaName == null)
                    _SchemaName = "";
                return _SchemaName;
            }
            set { _SchemaName = value; }
        }

        private static string _Target;
        /// <summary>
        /// The target of the export e.g. the main table
        /// </summary>
        public static string Target
        {
            get
            {
                if (Exporter._Target == null)
                    Exporter._Target = Exporter.StartTable.TableName;
                return Exporter._Target;
            }
            set { Exporter._Target = value; }
        }

        private static string _SchemaDescription;

        public static string SchemaDescription
        {
            get { return Exporter._SchemaDescription; }
            set { Exporter._SchemaDescription = value; }
        }

        private static int _SchemaVersion = 1;
        /// <summary>
        /// The version of the schema file, necessary to handle different versions of the schema 
        /// e.g. later versions containing additional columns that are missing in previous versions
        /// </summary>
        private static int SchemaVersion
        {
            get { return _SchemaVersion; }
            //set
            //{
            //    _SchemaVersion = value;
            //    switch (_SchemaVersion)
            //    {
            //        case 0:
            //            goto case 1;
            //        case 1:
            //            goto case 2;
            //        case 2:
            //            break;
            //    }
            //}
        }

        public static string ShowConvertedFile(string FileName)
        {
            System.IO.FileInfo xmlFile = new System.IO.FileInfo(FileName);
            System.Xml.Xsl.XslCompiledTransform xslt = new System.Xml.Xsl.XslCompiledTransform();
            // load default style sheet from resources
            System.IO.StringReader xsltReader = new System.IO.StringReader(DiversityWorkbench.Properties.Resources.ExportSchema);
            System.Xml.XmlReader xml = System.Xml.XmlReader.Create(xsltReader);
            xslt.Load(xml);

            System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(xmlFile.FullName);

            // The output file:
            string outputFile = xmlFile.FullName.Substring(0, xmlFile.FullName.Length - xmlFile.Extension.Length) + ".htm";

            // Create the writer.             
            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(outputFile, xslt.OutputSettings);

            // Transform the file
            xslt.Transform(doc, writer);
            writer.Close();

            return outputFile;
        }

        private static string _SchemaModule;

        public static string SchemaModule
        {
            get { return Exporter._SchemaModule; }
            //set { Import._SchemaModule = value; }
        }
        private static string _SchemaTarget;

        public static string SchemaTarget
        {
            get { return Exporter._SchemaTarget; }
            //set { Import._SchemaTarget = value; }
        }

        private static string _DBversion;

        public static string DBversion
        {
            get
            {
                if (Exporter._DBversion == null)
                {
                    string SQL = "select dbo.Version()";
                    Exporter._DBversion = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
                return Exporter._DBversion;
            }
            set { Exporter._DBversion = value; }
        }

        #endregion

        #region Save

        public static string SaveSchemaFile(bool IncludeProtocol, string FileName)
        {
            System.Xml.XmlWriter W;
            System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();

            // creating the file name
            System.IO.FileInfo FI = new System.IO.FileInfo(FileName);
            string SchemaFileName = "";
            if (FileName.Length == 0)
            {
                SchemaFileName = FI.FullName.Substring(0, FI.FullName.Length - FI.Extension.Length);
                SchemaFileName += "_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss");//.Year.ToString();
                //if (System.DateTime.Now.Month < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Month.ToString();
                //if (System.DateTime.Now.Day < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Day.ToString() + "_";
                //if (System.DateTime.Now.Hour < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Hour.ToString();
                //if (System.DateTime.Now.Minute < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Minute.ToString();
                //if (System.DateTime.Now.Second < 10) SchemaFileName += "0";
                //SchemaFileName += System.DateTime.Now.Second.ToString();
                SchemaFileName += ".xml";
            }
            else
                SchemaFileName = FileName;

            settings.Encoding = System.Text.Encoding.UTF8;
            W = System.Xml.XmlWriter.Create(SchemaFileName, settings);
            try
            {
                W.WriteStartDocument();
                W.WriteStartElement("ExportSchedule");
                W.WriteAttributeString("DBversion", DiversityWorkbench.Export.Exporter.DBversion);
                W.WriteAttributeString("version", "1");
                W.WriteAttributeString("Target", DiversityWorkbench.Export.Exporter.Target);
                W.WriteAttributeString("Module", DiversityWorkbench.Settings.ModuleName);

                if (IncludeProtocol)
                {
                    W.WriteStartElement("Header");
                    W.WriteElementString("Responsible", System.Environment.UserName);
                    W.WriteElementString("Date", System.DateTime.Now.ToLongDateString());
                    W.WriteElementString("Time", System.DateTime.Now.ToLongTimeString());
                    W.WriteElementString("Server", DiversityWorkbench.Settings.DatabaseServer);
                    W.WriteElementString("Database", DiversityWorkbench.Settings.DatabaseName);
                    if (!DiversityWorkbench.Settings.IsTrustedConnection)
                        W.WriteElementString("DatabaseUser", DiversityWorkbench.Settings.DatabaseUser);
                    W.WriteEndElement();//Header
                }

                W.WriteStartElement("Tables");
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.Table> KV in DiversityWorkbench.Export.Exporter.SourceTables)//.TableListForImport)
                {
                    W.WriteStartElement("Table");
                    W.WriteAttributeString("Position", KV.Value.ParallelPosition.ToString());
                    W.WriteAttributeString("Name", KV.Value.TableName);
                    W.WriteAttributeString("Alias", KV.Value.TableAlias);
                    W.WriteAttributeString("Template", KV.Value.Template);
                    W.WriteAttributeString("Display", KV.Value.DisplayText);
                    if (KV.Value.ParentTable != null)
                    {
                        W.WriteAttributeString("Parent", KV.Value.ParentTable.TableAlias);
                        if (KV.Value.InternalRelationJoinColumn != null && KV.Value.InternalRelationJoinColumn.Length > 0)
                            W.WriteAttributeString("JoinColumn", KV.Value.InternalRelationJoinColumn);
                    }
                    bool IsSorted = false;
                    foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in KV.Value.TableColumns)
                    {
                        if (TC.Value.SortingType != TableColumn.Sorting.notsorted)
                        {
                            IsSorted = true;
                            break;
                        }
                    }
                    W.WriteAttributeString("IsSorted", IsSorted.ToString());
                    DiversityWorkbench.Export.Exporter.SaveSourceTableColumns(ref W, KV.Value);
                    W.WriteEndElement();//Table
                }
                W.WriteEndElement();//Tables
                W.WriteStartElement("FileColumns");
                foreach (DiversityWorkbench.Export.FileColumn FC in DiversityWorkbench.Export.Exporter.FileColumns)
                    Exporter.SaveFileColum(ref W, FC);
                W.WriteEndElement();//FileColumns

                W.WriteEndElement();//ImportSchema
                W.WriteEndDocument();
                W.Flush();
                W.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                if (W != null)
                {
                    W.Flush();
                    W.Close();
                }
            }
            return SchemaFileName;
        }

        private static void SaveSourceTableColumns(ref System.Xml.XmlWriter W, Export.Table Table)
        {
            try
            {
                bool ColumnsContainInfos = false;
                foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in Table.TableColumns)
                {
                    if (TC.Value.UnitValues.Count > 0 || TC.Value.Filter.Length > 0 || TC.Value.RowFilter.Length > 0)
                    {
                        ColumnsContainInfos = true;
                        break;
                    }
                }


                System.Collections.Generic.SortedDictionary<int, string> SortingColumns = new SortedDictionary<int, string>();
                foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in Table.TableColumns)
                {
                    if (TC.Value.SortingType != TableColumn.Sorting.notsorted)
                    {
                        SortingColumns.Add(TC.Value.SortingSequence, TC.Key);
                        //ColumnsContainInfos = true;
                        //break;
                    }
                }
                if (SortingColumns.Count > 0 || ColumnsContainInfos)
                {
                    W.WriteStartElement("Columns");
                    foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in Table.TableColumns)
                    {
                        if (SortingColumns.ContainsValue(TC.Key) || TC.Value.UnitValues.Count > 0 || TC.Value.Filter.Length > 0 || TC.Value.RowFilter.Length > 0)
                            Exporter.SaveTableColumn(ref W, Table.TableColumns[TC.Key]);
                    }
                    //foreach (System.Collections.Generic.KeyValuePair<int, string> KV in SortingColumns)
                    //{
                    //    Exporter.SaveTableColumn(ref W, Table.TableColumns[KV.Value]);
                    //}
                    W.WriteEndElement();//Columns
                }
                //if (ColumnsContainInfos)
                //{
                //    W.WriteStartElement("Columns");
                //    foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in Table.TableColumns)
                //    {
                //        Exporter.SaveTableColumn(ref W, TC.Value);
                //    }
                //    W.WriteEndElement();//Columns
                //}
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void SaveSourceTable(ref System.Xml.XmlWriter W, Export.Table Table)
        {
            if (Table.ParentTable != null)
            {
                //W.WriteElementString("ParentTable", Table.ParentTable.TableAlias);
                if (Table.InternalRelationJoinColumn != null)
                    W.WriteElementString("InternalJoinColumn", Table.InternalRelationJoinColumn);
            }
            //W.WriteElementString("Template", Table.Template);
            bool ColumnsContainInfos = false;
            foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in Table.TableColumns)
            {
                if (TC.Value.SortingType != TableColumn.Sorting.notsorted)
                {
                    ColumnsContainInfos = true;
                    break;
                }
            }
            if (ColumnsContainInfos)
            {
                W.WriteStartElement("Columns");
                foreach (System.Collections.Generic.KeyValuePair<string, Export.TableColumn> TC in Table.TableColumns)
                {
                    Exporter.SaveTableColumn(ref W, TC.Value);
                }
                W.WriteEndElement();//Columns
            }
        }

        private static void SaveTableColumn(ref System.Xml.XmlWriter W, Export.TableColumn Column)
        {
            if ((Column.SortingType != TableColumn.Sorting.notsorted && Column.SortingSequence > 0)
                || Column.UnitValues.Count > 0
                || Column.Filter.Length > 0
                || Column.RowFilter.Length > 0)
            {
                W.WriteStartElement("Column");
                W.WriteAttributeString("Name", Column.ColumnName);
                W.WriteAttributeString("Sequence", Column.SortingSequence.ToString());
                W.WriteAttributeString("Sorting", Column.SortingType.ToString());
                if (Column.Filter.Length > 0)
                    W.WriteAttributeString("Filter", Column.Filter);
                if (Column.RowFilter.Length > 0)
                    W.WriteAttributeString("RowFilter", Column.RowFilter);
                if (Column.UnitValues.Count > 0)
                {
                    W.WriteStartElement("UnitValues");
                    foreach (Export.TableColumnUnitValue UV in Column.UnitValues)
                    {
                        W.WriteStartElement("UnitValue");
                        W.WriteAttributeString("Value", UV.UnitValue);
                        W.WriteAttributeString("Source", UV.SourceDisplayText);
                        W.WriteAttributeString("LinkedUnitValue", UV.LinkedUnitValue);
                        W.WriteAttributeString("LinkedDiversityWorkbenchModuleBaseUri", UV.LinkedDiversityWorkbenchModuleBaseUri);
                        if (UV.DiversityWorkbenchModuleBaseUri != null && UV.DiversityWorkbenchModuleBaseUri.Length > 0)
                        {
                            W.WriteElementString("BaseUri", UV.DiversityWorkbenchModuleBaseUri);
                        }
                        W.WriteEndElement();//UnitValue
                    }
                    W.WriteEndElement();//UnitValues
                }
                W.WriteEndElement();//Column
            }
        }

        private static void SaveFileColum(ref System.Xml.XmlWriter W, Export.FileColumn FileColumn)
        {
            W.WriteStartElement("FileColumn");
            W.WriteAttributeString("Position", FileColumn.Position.ToString());
            W.WriteAttributeString("Header", FileColumn.Header);
            W.WriteAttributeString("ColumnName", FileColumn.TableColumn.ColumnName);
            W.WriteAttributeString("TableAlias", FileColumn.TableColumn.Table.TableAlias);
            W.WriteAttributeString("IsSeparated", FileColumn.IsSeparatedFromPreviousColumn.ToString());
            if (FileColumn.TableColumnUnitValue != null)
            {
                W.WriteStartElement("UnitValue");
                W.WriteAttributeString("Value", FileColumn.TableColumnUnitValue.UnitValue);
                W.WriteAttributeString("DiversityWorkbenchModuleBaseUri", FileColumn.TableColumnUnitValue.DiversityWorkbenchModuleBaseUri);
                W.WriteAttributeString("LinkedDiversityWorkbenchModuleBaseUri", FileColumn.TableColumnUnitValue.LinkedDiversityWorkbenchModuleBaseUri);
                W.WriteAttributeString("LinkedUnitValue", FileColumn.TableColumnUnitValue.LinkedUnitValue);
                if (FileColumn.TableColumnUnitValue.SourceDisplayText != null)
                    W.WriteAttributeString("Source", FileColumn.TableColumnUnitValue.SourceDisplayText);
                if (FileColumn.TableColumnUnitValue.DiversityWorkbenchModuleBaseUri != null)
                    W.WriteElementString("BaseUri", FileColumn.TableColumnUnitValue.DiversityWorkbenchModuleBaseUri);
                W.WriteEndElement();//UnitValue
            }
            if (FileColumn.Prefix.Length > 0)
                W.WriteElementString("Prefix", FileColumn.Prefix);
            if (FileColumn.Postfix.Length > 0)
                W.WriteElementString("Postfix", FileColumn.Postfix);
            if (FileColumn.Transformations.Count > 0)
            {
                W.WriteStartElement("Transformations");
                foreach (DiversityWorkbench.Export.Transformation T in FileColumn.Transformations)
                    Export.Exporter.SaveFileColumnTransformations(ref W, T);
                W.WriteEndElement();//Transformations
            }
            W.WriteEndElement();//FileColumn
        }

        //private static void SaveTableSettings(ref System.Xml.XmlWriter W, string TableAlias)
        //{
        //    //W.WriteElementString("TableName", DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableName);
        //    //if (DiversityWorkbench.Export.Exporter.Tables[TableAlias].ParentTableAlias != null &&
        //    //    DiversityWorkbench.Export.Exporter.Tables[TableAlias].ParentTableAlias.Length > 0)
        //    //    W.WriteElementString("ParentTableAlias", DiversityWorkbench.Export.Exporter.Tables[TableAlias].ParentTableAlias);
        //    //W.WriteElementString("MergeHandling", DiversityWorkbench.Export.Exporter.Tables[TableAlias].MergeHandling.ToString());
        //    //W.WriteStartElement("Columns");
        //    //if (DiversityWorkbench.Export.Exporter.Tables[TableAlias].IsForAttachment)
        //    //{
        //    //    foreach (string AC in DiversityWorkbench.Export.Exporter.Tables[TableAlias].AttachmentColumns)
        //    //    {
        //    //        if (DiversityWorkbench.Export.Exporter.Tables[TableAlias].DataColumns[AC].IsSelected)
        //    //        {
        //    //            W.WriteStartElement("Column");
        //    //            W.WriteAttributeString("ColumnName", DiversityWorkbench.Export.Exporter.Tables[TableAlias].DataColumns[AC].ColumnName);
        //    //            DiversityWorkbench.Export.Exporter.SaveColumnSettings(ref W, DiversityWorkbench.Export.Exporter.Tables[TableAlias].DataColumns[AC]);
        //    //            W.WriteEndElement();//Column
        //    //        }
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.DataColumn> DC in DiversityWorkbench.Export.Exporter.Tables[TableAlias].DataColumns)
        //    //    {
        //    //        if (DC.Value.IsSelected)
        //    //        {
        //    //            W.WriteStartElement("Column");
        //    //            W.WriteAttributeString("ColumnName", DC.Key);
        //    //            DiversityWorkbench.Export.Exporter.SaveColumnSettings(ref W, DC.Value);
        //    //            W.WriteEndElement();//Column
        //    //        }
        //    //    }
        //    //}
        //    //W.WriteEndElement();//Columns
        //}

        //private static void SaveColumnSettings(ref System.Xml.XmlWriter W, DiversityWorkbench.Export.FileColumn FileColumn)
        //{
        //    try
        //    {
        //        //W.WriteElementString("CompareKey", FileColumn.CompareKey.ToString());
        //        //W.WriteElementString("CopyPrevious", FileColumn.CopyPrevious.ToString());
        //        //if (FileColumn.FileColumn != null)
        //        //    W.WriteElementString("FileColumn", FileColumn.FileColumn.ToString());
        //        //W.WriteElementString("IsDecisive", FileColumn.IsDecisive.ToString());
        //        //if (FileColumn.Prefix != null)
        //        //    W.WriteElementString("Prefix", FileColumn.Prefix.ToString());
        //        //if (FileColumn.Postfix != null)
        //        //    W.WriteElementString("Postfix", FileColumn.Postfix.ToString());
        //        //W.WriteElementString("TypeOfSource", FileColumn.TypeOfSource.ToString());
        //        //if (FileColumn.TypeOfSource == DiversityWorkbench.Export.DataColumn.SourceType.Interface
        //        //    && FileColumn.OriginalValue != null && FileColumn.OriginalValue.Length > 0) // Toni: Save original value supplied in form - adaption to preprocessing
        //        //    W.WriteElementString("Value", FileColumn.OriginalValue.ToString());         // Toni: Save original value supplied in form - adaption to preprocessing 
        //        //if (FileColumn.SelectParallelForeignRelationTableAlias)
        //        //    W.WriteElementString("ForeignRelationTableAlias", FileColumn.ForeignRelationTableAlias);

        //        //if (DataColumn.IsMultiColumn && DataColumn.MultiColumns.Count > 0)
        //        //{
        //        //    W.WriteStartElement("MultiColumns");
        //        //    foreach (DiversityWorkbench.Export.ColumnMulti MC in DataColumn.MultiColumns)
        //        //    {
        //        //        W.WriteStartElement("MultiColumn");
        //        //        DiversityWorkbench.Export.Exporter.SaveColumnMultiColumnSettings(ref W, MC);
        //        //        W.WriteEndElement();//MultiColumn
        //        //    }
        //        //    W.WriteEndElement();//MultiColumns
        //        //}

        //        if (FileColumn.Transformations.Count > 0)
        //        {
        //            W.WriteStartElement("Transformations");
        //            foreach (DiversityWorkbench.Export.Transformation T in FileColumn.Transformations)
        //            {
        //                W.WriteStartElement("Transformation");
        //                DiversityWorkbench.Export.Exporter.SaveColumnTransformationSettings(ref W, T);
        //                W.WriteEndElement();//Transformation
        //            }
        //            W.WriteEndElement();//Transformations
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //    }
        //}

        //private static void SaveColumnMultiColumnSettings(ref System.Xml.XmlWriter W, DiversityWorkbench.Export.ColumnMulti MultiColumn)
        //{
        //    W.WriteElementString("CopyPrevious", MultiColumn.CopyPrevious.ToString());
        //    W.WriteElementString("ColumnInFile", MultiColumn.ColumnInFile.ToString());
        //    W.WriteElementString("IsDecisive", MultiColumn.IsDecisive.ToString());
        //    W.WriteElementString("Prefix", MultiColumn.Prefix.ToString());
        //    W.WriteElementString("Postfix", MultiColumn.Postfix.ToString());

        //    if (MultiColumn.Transformations.Count > 0)
        //    {
        //        W.WriteStartElement("Transformations");
        //        foreach (DiversityWorkbench.Export.Transformation T in MultiColumn.Transformations)
        //        {
        //            W.WriteStartElement("Transformation");
        //            DiversityWorkbench.Export.Exporter.SaveColumnTransformationSettings(ref W, T);
        //            W.WriteEndElement();//Transformation
        //        }
        //        W.WriteEndElement();//Transformations
        //    }
        //}

        private static void SaveFileColumnTransformations(ref System.Xml.XmlWriter W, DiversityWorkbench.Export.Transformation T)
        {
            try
            {
                W.WriteStartElement("Transformation");
                W.WriteAttributeString("Type", T.TypeOfTransformation.ToString());

                switch (T.TypeOfTransformation)
                {
                    case Transformation.TransformationType.Calculation:
                        W.WriteElementString("CalulationOperator", T.CalulationOperator.ToString());
                        double CalculationFactor = double.Parse(T.CalculationFactor.Replace(" ", "").ToString());
                        W.WriteElementString("CalculationFactor", CalculationFactor.ToString());
                        if (T.CalculationConditionOperator != null && T.CalculationConditionValue != null &&
                            T.CalculationConditionOperator.Length > 0 && T.CalculationConditionValue.Length > 0)
                        {
                            W.WriteElementString("CalulationConditionOperator", T.CalculationConditionOperator);
                            double CalculationConditionValue = double.Parse(T.CalculationConditionValue.Replace(" ", ""));
                            W.WriteElementString("CalculationConditionValue", CalculationConditionValue.ToString());
                        }
                        break;
                    case Transformation.TransformationType.RegularExpression:
                        W.WriteElementString("RegularExpression", T.RegularExpression.ToString());
                        W.WriteElementString("RegularExpressionReplacement", T.RegularExpressionReplacement.ToString());
                        break;
                    case Transformation.TransformationType.Replacement:
                        W.WriteElementString("Replace", T.Replace.ToString());
                        string ReplaceWith = "";
                        if (T.ReplaceWith != null)
                            ReplaceWith = T.ReplaceWith.ToString();
                        W.WriteElementString("ReplaceWith", ReplaceWith);
                        break;
                    case Transformation.TransformationType.Split:
                        W.WriteElementString("SplitterPosition", T.SplitterPosition.ToString());
                        W.WriteElementString("ReverseSequence", T.ReverseSequence.ToString());
                        W.WriteElementString("SplitterIsStartPosition", T.SplitterIsStartPosition.ToString());
                        W.WriteStartElement("Splitters");
                        foreach (string S in T.SplitterList)
                            W.WriteElementString("Splitter", S);
                        W.WriteEndElement();//Splitters
                        break;
                    case Transformation.TransformationType.Translation:
                        W.WriteStartElement("Translations");
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in T.TranslationDictionary)
                        {
                            W.WriteStartElement("Translation");
                            W.WriteAttributeString("From", KV.Key);
                            W.WriteAttributeString("To", KV.Value);
                            W.WriteEndElement();
                            //W.WriteElementString("Translation", KV.Key);
                        }
                        W.WriteEndElement();//Translations
                        break;
                    case Transformation.TransformationType.Filter:
                        W.WriteElementString("FilterUseFixedValue", T.FilterUseFixedValue.ToString());
                        W.WriteElementString("FilterFixedValue", T.FilterFixedValue.ToString());
                        W.WriteElementString("FilterConditionsOperator", T.FilterConditionsOperator.ToString());
                        W.WriteStartElement("FilterConditions");
                        foreach (DiversityWorkbench.Export.TransformationFilter F in T.FilterConditions)
                        {
                            W.WriteStartElement("FilterCondition");
                            W.WriteElementString("Filter", F.Filter);
                            W.WriteElementString("FilterColumn", F.PositionOfFilterColumn.ToString());
                            W.WriteElementString("FilterOperator", F.FilterOperator.ToString());
                            W.WriteEndElement();//FiterCondition
                        }
                        W.WriteEndElement();//FiterConditions
                        break;
                    case Transformation.TransformationType.Color:
                        break;
                }
                W.WriteEndElement();//Transformation
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        //private static void SaveImportErrors(ref System.Xml.XmlWriter W)
        //{
        //    // Settings
        //    W.WriteStartElement("ImportErrors");
        //    //for (int i = DiversityWorkbench.Export.Exporter.StartLine - 1 /* this._ImportInterface.FirstLineForImport() - 1*/; i < DiversityWorkbench.Export.Exporter.EndLine /* this._ImportInterface.LastLineForImport()*/; i++)
        //    //{
        //    //    bool ErrorFound = false;
        //    //    //foreach (string s in DiversityWorkbench.Export.Exporter.TableListForImport)
        //    //    //{
        //    //    //    if (DiversityWorkbench.Export.Exporter.Tables[s].Messages.ContainsKey(i))
        //    //    //    {
        //    //    //        ErrorFound = true;
        //    //    //        break;
        //    //    //    }
        //    //    //}
        //    //    //if (ErrorFound)
        //    //    //{
        //    //    //    W.WriteStartElement("ImportError");
        //    //    //    W.WriteElementString("Line", (i + 1).ToString());
        //    //    //    foreach (string T in DiversityWorkbench.Export.Exporter.TableListForImport)
        //    //    //    {
        //    //    //        if (DiversityWorkbench.Export.Exporter.Tables[T].Messages.ContainsKey(i))
        //    //    //        {
        //    //    //            W.WriteStartElement("Table");
        //    //    //            W.WriteElementString("TableAlias", T);
        //    //    //            W.WriteElementString("TableDisplayText", DiversityWorkbench.Export.Exporter.Tables[T].DisplayText);
        //    //    //            W.WriteElementString("Error", DiversityWorkbench.Export.Exporter.Tables[T].Messages[i]);
        //    //    //            W.WriteEndElement(); //Table
        //    //    //        }
        //    //    //    }
        //    //    //    W.WriteEndElement(); //ImportError
        //    //    //}
        //    //}
        //    W.WriteEndElement(); //ImportErrors
        //}

        //private void SaveImportSteps(ref System.Xml.XmlWriter W)
        //{
        //    // Steps
        //    W.WriteStartElement("Steps");
        //    //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.Step> IS in DiversityWorkbench.Export.Exporter.Steps)
        //    //{
        //    //    if (IS.Value.IsSelected)
        //    //    {
        //    //        W.WriteStartElement("ImportStep");
        //    //        W.WriteElementString("Key", IS.Key);
        //    //        W.WriteElementString("Title", IS.Value.DisplayText.ToString());
        //    //        W.WriteEndElement(); //Step
        //    //    }
        //    //}
        //    W.WriteEndElement(); //Steps
        //}

        #endregion

        #region Load

        /// <summary>
        /// Loading a Schema for the export
        /// </summary>
        /// <param name="FileName">The name of the XML file</param>
        /// <returns></returns>
        public static bool LoadSchemaFile(string FileName)
        {
            Export.Exporter.ResetUserActions();
            Export.Exporter.InitSourceTables();

            System.Xml.XmlTextReader R = new System.Xml.XmlTextReader(FileName);
            try
            {
                int iVersion = 1;
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "xml")
                        continue;
                    if (R.Name == "ExportSchedule")
                    {
                        try
                        {
                            if (R.NodeType == System.Xml.XmlNodeType.Element)
                            {
                                string Vers = R.GetAttribute("version");
                                int.TryParse(Vers, out iVersion);
                                DiversityWorkbench.Export.Exporter._SchemaModule = R.GetAttribute("Module");
                                DiversityWorkbench.Export.Exporter._SchemaTarget = R.GetAttribute("Target");
                                DiversityWorkbench.Export.Exporter._DBversion = R.GetAttribute("DBversion");
                                if (DiversityWorkbench.Export.Exporter._SchemaModule != DiversityWorkbench.Settings.ModuleName ||
                                    DiversityWorkbench.Export.Exporter._SchemaTarget != DiversityWorkbench.Export.Exporter.Target)
                                {
                                    System.Windows.Forms.MessageBox.Show("The selected schema does not fit the current export");
                                    return false;
                                }
                                else
                                {
                                    if (iVersion != Export.Exporter.SchemaVersion)
                                    {
                                        System.Windows.Forms.MessageBox.Show("The loaded schema is version " + iVersion.ToString()
                                            + "\r\nThe expected version is " + Export.Exporter.SchemaVersion.ToString()
                                            + "\r\nPlease check for possible errors and save loaded schema as new version");
                                    }
                                    //SchemaVersion = iVersion;
                                    DiversityWorkbench.Export.Exporter.SchemaName = FileName;
                                    break;
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            //SchemaVersion = 0;
                            return false;
                        }
                        continue;
                    }
                }
                Export.Exporter.LoadTables(ref R);
                Export.Exporter.LoadFileColumns(ref R);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                R.ResetState();
                R.Close();
            }
            return true;
        }

        #region Tables

        private static void LoadTables(ref System.Xml.XmlTextReader R)
        {
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "Tables")
                        return;

                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "Table" && R.IsStartElement())
                        LoadTable(ref R);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private static void LoadTable(ref System.Xml.XmlTextReader R)
        {
            try
            {
                string TableAlias = "";
                string TableName = "";
                string Position = "";
                string Template = "";
                string Parent = "";
                string JoinColumn = "";
                if (R.AttributeCount > 0)
                {
                    TableAlias = R.GetAttribute("Alias");
                    TableName = R.GetAttribute("Name");
                    Position = R.GetAttribute("Position");
                    Template = R.GetAttribute("Template");
                    Parent = R.GetAttribute("Parent");
                    JoinColumn = R.GetAttribute("JoinColumn");
                }
                if (!DiversityWorkbench.Export.Exporter.SourceTables.ContainsKey(TableAlias))
                {
                    DiversityWorkbench.Export.Table T = new Table(DiversityWorkbench.Export.Exporter.TemplateTableDictionary[Template], DiversityWorkbench.Export.Exporter.SourceTables[Parent]);
                    T.setParallelPosition(int.Parse(Position));
                    if (JoinColumn != null)
                        T.InternalRelationJoinColumn = JoinColumn;
                    if (DiversityWorkbench.Export.Exporter.TemplateTableDictionary[Template].JoinColumns.Count > 0)
                    {
                        if (T.TypeOfParallelity == Table.Parallelity.referencing)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Export.Table> KV in DiversityWorkbench.Export.Exporter.TemplateTableDictionary)
                            {
                                if (KV.Value.TableName == T.TableName && KV.Value.ParentTable != null && KV.Value.ParentTable.TableAlias == T.ParentTable.TableAlias)
                                {
                                    T.JoinColumns = KV.Value.JoinColumns;
                                }
                            }
                        }
                        else
                            T.JoinColumns = DiversityWorkbench.Export.Exporter.TemplateTableDictionary[Template].JoinColumns;
                    }
                    DiversityWorkbench.Export.Exporter.SourceTables.Add(T.TableAlias, T);
                }
                if (!R.IsEmptyElement)
                {
                    while (R.Read())
                    {
                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                            continue;
                        if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "Table")
                            return;
                        if (R.IsStartElement() && R.Name == "Columns" && !R.IsEmptyElement)
                        {
                            System.Collections.Generic.List<string> Columns = LoadTableColumns(ref R, TableAlias);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Columns

        private static System.Collections.Generic.List<string> LoadTableColumns(ref System.Xml.XmlTextReader R, string TableAlias)
        {
            System.Collections.Generic.List<string> ColumnList = new List<string>();
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "Columns" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return ColumnList;
                    if (R.IsStartElement() && R.Name == "Column")
                    {
                        try
                        {
                            string ColumnName = R.GetAttribute("Name");
                            string Sorting = R.GetAttribute("Sorting");
                            string Sequence = R.GetAttribute("Sequence");
                            DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].SortingSequence = int.Parse(Sequence);
                            if (Sorting == "ascending")
                                DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].SortingType = TableColumn.Sorting.ascending;
                            else if (Sorting == "descending")
                                DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].SortingType = TableColumn.Sorting.descending;
                            else
                                DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].SortingType = TableColumn.Sorting.notsorted;
                            string Filter = R.GetAttribute("Filter");
                            if (Filter != null && Filter.Length > 0)
                                DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].Filter = Filter;
                            string RowFilter = R.GetAttribute("RowFilter");
                            if (RowFilter != null && RowFilter.Length > 0)
                                DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].RowFilter = RowFilter;
                            if (!R.IsEmptyElement)
                            {
                                R.Read();
                                if (R.Name == "UnitValues")
                                {
                                    while (R.NodeType != System.Xml.XmlNodeType.EndElement && R.Name == "UnitValues")
                                    {
                                        R.Read();
                                        if (R.Name == "UnitValue")
                                        {
                                            string Value = R.GetAttribute("Value");
                                            string Source = R.GetAttribute("Source");
                                            string LinkedDiversityWorkbenchModuleBaseUri = R.GetAttribute("LinkedDiversityWorkbenchModuleBaseUri");
                                            string LinkedUnitValue = R.GetAttribute("LinkedUnitValue");
                                            string BaseUri = "";
                                            if (!R.IsEmptyElement)
                                            {
                                                R.Read();
                                                if (R.Name == "BaseUri")
                                                {
                                                    R.Read();
                                                    BaseUri = R.Value;
                                                }
                                            }
                                            Export.TableColumnUnitValue UV;
                                            if (BaseUri.Length == 0)
                                            {
                                                UV = new TableColumnUnitValue(DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName], Value);
                                            }
                                            else
                                            {
                                                UV = new TableColumnUnitValue(DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName], BaseUri, Value, Source);
                                            }
                                            UV.LinkedDiversityWorkbenchModuleBaseUri = LinkedDiversityWorkbenchModuleBaseUri;
                                            UV.LinkedUnitValue = LinkedUnitValue;
                                            DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].UnitValues.Add(UV);
                                        }
                                    }
                                }
                            }

                            //R.Read();
                            //if (R.Name == "Sorting")
                            //{
                            //    string Sequence = R.GetAttribute("Sequence");
                            //    string Type = R.GetAttribute("Type");
                            //    DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].SortingSequence = int.Parse(Sequence);
                            //    if (Type == "ascending")
                            //        DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].SortingType = TableColumn.Sorting.ascending;
                            //    else if (Type == "descending")
                            //        DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName].SortingType = TableColumn.Sorting.descending;
                            //}
                            ColumnList.Add(ColumnName);
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return ColumnList;
        }

        private static System.Collections.Generic.List<string> LoadTableColumns(ref System.Xml.XmlTextReader R, DiversityWorkbench.Export.Table DT)
        {
            System.Collections.Generic.List<string> ColumnList = new List<string>();
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "Columns" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return ColumnList;
                    if (R.IsStartElement() && R.Name == "Column")
                    {
                        try
                        {
                            string ColumnName = R.GetAttribute("Name");
                            R.Read();
                            if (R.Name == "Sorting")
                            {
                                string Sequence = R.GetAttribute("Sequence");
                                string Type = R.GetAttribute("Type");
                                DT.TableColumns[ColumnName].SortingSequence = int.Parse(Sequence);
                                if (Type == "ascending")
                                    DT.TableColumns[ColumnName].SortingType = TableColumn.Sorting.ascending;
                                else if (Type == "descending")
                                    DT.TableColumns[ColumnName].SortingType = TableColumn.Sorting.descending;
                            }
                            ColumnList.Add(ColumnName);
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return ColumnList;
        }

        #endregion

        #region FileColumns

        private static System.Collections.Generic.List<string> LoadFileColumns(ref System.Xml.XmlTextReader R)//, DiversityWorkbench.Export.Table DT)
        {
            System.Collections.Generic.List<string> ColumnList = new List<string>();
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "FileColumns" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return ColumnList;
                    if (R.IsStartElement() && R.Name == "FileColumn")
                    {
                        try
                        {
                            string ColumnName = R.GetAttribute("ColumnName");
                            string TableAlias = R.GetAttribute("TableAlias");
                            string Position = R.GetAttribute("Position");
                            string Header = R.GetAttribute("Header");
                            string IsSeparated = R.GetAttribute("IsSeparated");
                            DiversityWorkbench.Export.FileColumn FC = new FileColumn(DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName]);
                            FC.Header = Header;
                            FC.Position = int.Parse(Position);
                            FC.IsSeparatedFromPreviousColumn = bool.Parse(IsSeparated);
                            if (R.IsEmptyElement)
                            {
                                //if (DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns == null)
                                //{
                                //}
                                //int i = DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns.Count;
                                DiversityWorkbench.Export.Exporter.FileColumns.Add(FC);
                            }
                            else
                            {
                                //DiversityWorkbench.Export.FileColumn FC = new FileColumn(DiversityWorkbench.Export.Exporter.SourceTables[TableAlias].TableColumns[ColumnName]);
                                //FC.Header = Header;
                                //FC.Position = int.Parse(Position);
                                //FC.IsSeparatedFromPreviousColumn = bool.Parse(IsSeparated);
                                bool FileColumnFinished = false;
                                while (!FileColumnFinished && R.Read())
                                {
                                    switch (R.Name)
                                    {
                                        case "UnitValue":
                                            if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                                                continue;
                                            string Value = R.GetAttribute("Value");
                                            FC.TableColumnUnitValue = new TableColumnUnitValue(FC.TableColumn, Value);
                                            FC.TableColumnUnitValue.LinkedDiversityWorkbenchModuleBaseUri = R.GetAttribute("LinkedDiversityWorkbenchModuleBaseUri");
                                            FC.TableColumnUnitValue.LinkedUnitValue = R.GetAttribute("LinkedUnitValue");
                                            FC.TableColumnUnitValue.SourceDisplayText = R.GetAttribute("Source");
                                            if (!R.IsEmptyElement)
                                            {
                                                R.Read();
                                                if (R.Name == "BaseUri")
                                                {
                                                    R.Read();
                                                    FC.TableColumnUnitValue.DiversityWorkbenchModuleBaseUri = R.Value;
                                                }
                                            }
                                            break;
                                        case "Prefix":
                                            if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                                                continue;
                                            R.Read();
                                            FC.Prefix = R.Value;
                                            break;
                                        case "Postfix":
                                            if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                                                continue;
                                            R.Read();
                                            FC.Postfix = R.Value;
                                            break;
                                        case "Transformations":
                                            Exporter.LoadFileColumnTransformations(ref R, FC);
                                            break;
                                        case "FileColumn":
                                            FileColumnFinished = true;
                                            continue;
                                            break;
                                    }
                                }
                                DiversityWorkbench.Export.Exporter.FileColumns.Add(FC);
                            }



                            //if (DT.TableColumns.ContainsKey(ColumnName))
                            //{
                            //    LoadColumn(ref R, DT.TableColumns[ColumnName]);
                            //    ColumnList.Add(ColumnName);
                            //}
                            //else
                            //{
                            //    string Message = "In the table " + DT.TableAlias + " (= " + DT.TableName + ") the column " + ColumnName + " could not be found.\r\n"
                            //        + "The schema may have been created depending on a different version of the database";
                            //    System.Windows.Forms.MessageBox.Show(Message);
                            //}
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return ColumnList;
        }

        private static void LoadColumn(ref System.Xml.XmlTextReader R, DiversityWorkbench.Export.TableColumn DC)
        {
            string _CurrentNode = "";
            //DC.IsSelected = true;
            try
            {
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "Column" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.IsStartElement() && R.Name.Length > 0)
                    {
                        string Node = R.Name;
                        if (!R.IsEmptyElement)
                            R.Read();
                        //switch (Node)
                        //{
                        //    case "CompareKey":
                        //        DC.CompareKey = bool.Parse(R.Value);
                        //        break;
                        //    case "CopyPrevious":
                        //        DC.CopyPrevious = bool.Parse(R.Value);
                        //        break;
                        //    case "IsDecisive":
                        //        DC.IsDecisive = bool.Parse(R.Value);
                        //        break;
                        //    case "TypeOfSource":
                        //        switch (R.Value)
                        //        {
                        //            case "Database":
                        //                DC.TypeOfSource = DataColumn.SourceType.Database;
                        //                break;
                        //            case "File":
                        //                DC.TypeOfSource = DataColumn.SourceType.File;
                        //                break;
                        //            case "NotDecided":
                        //                DC.TypeOfSource = DataColumn.SourceType.NotDecided;
                        //                break;
                        //            case "Interface":
                        //                DC.TypeOfSource = DataColumn.SourceType.Interface;
                        //                break;
                        //            case "Preset":
                        //                DC.TypeOfSource = DataColumn.SourceType.Preset;
                        //                break;
                        //            case "ParentTable":
                        //                DC.TypeOfSource = DataColumn.SourceType.ParentTable;
                        //                break;
                        //            default:
                        //                break;
                        //        }
                        //        break;
                        //    case "FileColumn":
                        //        DC.FileColumn = int.Parse(R.Value);
                        //        DC.TypeOfSource = DataColumn.SourceType.File;
                        //        break;
                        //    case "Prefix":
                        //        if (R.NodeType != System.Xml.XmlNodeType.Whitespace)
                        //            DC.Prefix = R.Value;
                        //        else
                        //        {
                        //            string P = R.Value;
                        //            if (P.Replace(" ", "").Length == 0)
                        //                DC.Prefix = P;
                        //        }
                        //        break;
                        //    case "Postfix":
                        //        if (R.NodeType != System.Xml.XmlNodeType.Whitespace)
                        //            DC.Postfix = R.Value;
                        //        else
                        //        {
                        //            string P = R.Value;
                        //            if (P.Replace(" ", "").Length == 0)
                        //                DC.Postfix = P;
                        //        }
                        //        break;
                        //    case "ForeignRelationTableAlias":
                        //        DC.ForeignRelationTableAlias = R.Value;
                        //        break;
                        //    case "MultiColumns":
                        //        DiversityWorkbench.Export.Exporter.LoadMultiColumns(ref R, ref DC);
                        //        break;
                        //    case "Transformations":
                        //        DiversityWorkbench.Export.Exporter.LoadColumnTransformations(ref R, DC);
                        //        break;
                        //    case "Value":
                        //        DC.Value = R.Value;
                        //        break;
                        //}
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return;
        }

        private static void LoadFileColumnTransformations(ref System.Xml.XmlTextReader R, DiversityWorkbench.Export.FileColumn FC)
        {
            try
            {
                if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                    R.Read();
                if (R.Name == "Transformations")
                    R.Read();
                string TypeOfTransformation = R.GetAttribute("Type");
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "Transformations")
                        return;
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                        continue;
                    if (R.IsStartElement())
                    {
                        if (R.Name == "Transformation")
                        {
                            //R.Read();
                            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                R.Read();
                            TypeOfTransformation = R.GetAttribute(0);
                        }
                        switch (TypeOfTransformation)
                        {
                            case "Replacement":
                                if (R.Name == "Transformation")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "Replace")
                                    R.Read();
                                string Replace = R.Value;
                                while (!R.IsStartElement() && R.Name != "ReplaceWith")
                                    R.Read();
                                R.Read();
                                string ReplaceWith = R.Value;
                                DiversityWorkbench.Export.Transformation Trepl = new Transformation(FC, Transformation.TransformationType.Replacement);
                                Trepl.Replace = Replace;
                                Trepl.ReplaceWith = ReplaceWith;
                                break;
                            case "Calculation":
                                if (R.Name == "Transformation")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "Calculation")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "CalulationOperator")
                                    R.Read();
                                string CalulationOperator = R.Value;
                                string CalculationFactor = "";
                                string CalulationConditionOperator = "";
                                string CalculationConditionValue = "";
                                while (!R.IsStartElement() && R.Name != "CalculationFactor")
                                    R.Read();
                                R.Read();
                                CalculationFactor = R.Value;
                                R.Read();
                                R.Read();
                                if (R.Name == "CalulationConditionOperator")
                                {
                                    R.Read();
                                    CalulationConditionOperator = R.Value;
                                    R.Read();
                                    R.Read();
                                    R.Read();
                                    CalculationConditionValue = R.Value;
                                }
                                //if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "Transformation")
                                //{
                                DiversityWorkbench.Export.Transformation Tcalc = new Transformation(FC, Transformation.TransformationType.Calculation);
                                Tcalc.CalulationOperator = CalulationOperator;
                                Tcalc.CalculationFactor = CalculationFactor;
                                if (CalulationConditionOperator.Length > 0)
                                {
                                    Tcalc.CalculationConditionOperator = CalulationConditionOperator;
                                    Tcalc.CalculationConditionValue = CalculationConditionValue;
                                }
                                //}
                                break;
                            case "Translation":
                                DiversityWorkbench.Export.Transformation Ttrans = new Transformation(FC, Transformation.TransformationType.Translation);
                                //R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "Translations" || R.Name == "Translation" || R.Name == "Transformation")
                                {
                                    R.Read();
                                    if (R.Name == "Transformation" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                        break;
                                    if (R.Name == "Translations" && R.NodeType == System.Xml.XmlNodeType.Element)
                                        R.Read();
                                    while (R.NodeType != System.Xml.XmlNodeType.EndElement && R.Name != "Translations")
                                    {
                                        if (R.IsStartElement() && R.Name == "Translations")
                                            R.Read();
                                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                            R.Read();
                                        if (R.IsStartElement() && R.Name == "Translation")
                                        {
                                            string Translate = R.GetAttribute(0);
                                            string Into = R.GetAttribute(1);
                                            Ttrans.TranslationDictionary.Add(Translate, Into);
                                        }
                                        if (R.Name == "Translations" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                            break;
                                        R.Read();
                                        if (R.Name == "Translations" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                            break;
                                    }

                                    // Original version - caused empty duplicate
                                    //R.Read();
                                    //if (R.Name == "Transformation" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                    //    break;
                                    //while (R.Name != "Translations")
                                    //{
                                    //    if (R.IsStartElement() && R.Name == "Translations")
                                    //        R.Read();
                                    //    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                    //        R.Read();
                                    //    if (R.IsStartElement() && R.Name == "Translation")
                                    //    {
                                    //        string Translate = R.GetAttribute(0);
                                    //        string Into = R.GetAttribute(1);
                                    //        Ttrans.TranslationDictionary.Add(Translate, Into);
                                    //    }
                                    //    if (R.Name == "Translations" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                    //        break;
                                    //    R.Read();
                                    //}
                                }
                                break;
                            case "Split":
                                DiversityWorkbench.Export.Transformation Tsplit = new Transformation(FC, Transformation.TransformationType.Split);
                                int Position;
                                if (R.Name == "SplitterPosition" && R.NodeType == System.Xml.XmlNodeType.Element)
                                {
                                    R.Read();
                                    if (R.NodeType == System.Xml.XmlNodeType.Text && R.Value.ToString().Length > 0)
                                    {
                                        if (int.TryParse(R.Value, out Position))
                                            Tsplit.SplitterPosition = Position;
                                    }
                                }
                                R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.EndElement)
                                    R.Read();
                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "SplitterPosition")
                                {
                                    R.Read();
                                    if (int.TryParse(R.Value, out Position))
                                        Tsplit.SplitterPosition = Position;
                                    R.Read();
                                }
                                if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "SplitterPosition")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "ReverseSequence")
                                {
                                    bool ReverseSequence;
                                    R.Read();
                                    if (bool.TryParse(R.Value, out ReverseSequence))
                                        Tsplit.ReverseSequence = ReverseSequence;
                                    R.Read();
                                }
                                if (R.NodeType == System.Xml.XmlNodeType.EndElement && R.Name == "ReverseSequence")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "SplitterIsStartPosition")
                                {
                                    bool SplitterIsStartPosition;
                                    R.Read();
                                    if (bool.TryParse(R.Value, out SplitterIsStartPosition))
                                        Tsplit.SplitterIsStartPosition = SplitterIsStartPosition;
                                    R.Read();
                                }
                                while (R.Name != "Transformation")
                                {
                                    R.Read();
                                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                        R.Read();
                                    if (R.IsEmptyElement)
                                    {
                                        System.Windows.Forms.MessageBox.Show("Split transformation missing splitters detected");
                                        break;
                                    }
                                    if (R.IsStartElement() && R.Name == "Splitters")
                                    {
                                        R.Read();
                                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                            R.Read();
                                        while (R.Name != "Splitters")
                                        {
                                            if (R.Name == "Splitter" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                                R.Read();
                                            if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                                R.Read();
                                            if (R.Name == "Splitter" && R.IsStartElement())
                                            {
                                                R.Read();
                                                Tsplit.SplitterList.Add(R.Value);
                                            }
                                            if (R.Name != "Splitters")
                                                R.Read();
                                        }
                                    }
                                }
                                break;
                            case "RegularExpression":
                                if (R.Name == "Transformation")
                                    R.Read();
                                if (R.IsStartElement() && R.Name == "RegularExpression")
                                    R.Read();
                                string RegularExpression = R.Value;
                                while (!R.IsStartElement() && R.Name != "RegularExpressionReplacement")
                                    R.Read();
                                R.Read();
                                string RegularExpressionReplacement = R.Value;
                                DiversityWorkbench.Export.Transformation Treg = new Transformation(FC, Transformation.TransformationType.RegularExpression);
                                Treg.RegularExpression = RegularExpression;
                                Treg.RegularExpressionReplacement = RegularExpressionReplacement;
                                break;
                            case "Filter":
                                // Alte version - fuehrt zu endlos schleife
                                if (R.Name == "Filter")
                                {
                                    return;
                                }

                                string FilterUseFixedValue = "";
                                if (R.Name == "Transformation")
                                    R.Read();

                                while (!R.IsStartElement() && R.Name != "FilterUseFixedValue")
                                    R.Read();
                                R.Read();
                                FilterUseFixedValue = R.Value;

                                while (R.NodeType != System.Xml.XmlNodeType.Element && R.Name != "FilterFixedValue")
                                    R.Read();
                                if (!R.IsEmptyElement)
                                    R.Read();
                                string FilterFixedValue = R.Value;

                                if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                    R.Read();

                                while (!R.IsStartElement() || R.Name != "FilterConditionsOperator")
                                    R.Read();

                                R.Read();
                                string FilterOperator = R.Value;
                                Transformation.FilterConditionsOperators FCO = Transformation.FilterConditionsOperators.And;
                                if (FilterOperator != FCO.ToString())
                                    FCO = Transformation.FilterConditionsOperators.Or;

                                DiversityWorkbench.Export.Transformation Tfilter = new Transformation(FC, Transformation.TransformationType.Filter);
                                Tfilter.FilterUseFixedValue = bool.Parse(FilterUseFixedValue);
                                Tfilter.FilterFixedValue = FilterFixedValue;
                                Tfilter.FilterConditionsOperator = FCO;

                                while (R.Name != "FilterConditions")
                                    R.Read();
                                R.Read();
                                while (R.Name != "FilterConditions") // looking for the end
                                {
                                    while (R.Name != "Filter")
                                    {
                                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                                            R.Read();
                                        if (R.Name == "FilterConditions")// && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                            break;
                                        R.Read();
                                    }
                                    if (R.Name == "FilterConditions")// && R.NodeType == System.Xml.XmlNodeType.EndElement)
                                        break;

                                    if (!R.IsEmptyElement)
                                        R.Read();
                                    DiversityWorkbench.Export.TransformationFilter TF = new TransformationFilter(Tfilter);
                                    Tfilter.FilterConditions.Add(TF);
                                    TF.Filter = R.Value;

                                    while (R.Name != "FilterColumn")
                                        R.Read();
                                    R.Read();
                                    TF.PositionOfFilterColumn = int.Parse(R.Value);

                                    while (R.Name != "FilterOperator")
                                        R.Read();
                                    R.Read();
                                    TF.FilterOperator = R.Value;
                                }
                                break;
                            case "Color":
                                break;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return;
        }

        private static void LoadFileColumnTranslation(ref System.Xml.XmlTextReader R, ref DiversityWorkbench.Export.FileColumn IC)
        {
            try
            {
                //string Column = "";
                string Property = "";
                string Source = "";
                string Translation = "";
                while (R.Read())
                {
                    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                        continue;
                    if (R.Name == "Translation" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                        return;
                    if (R.IsStartElement() && R.Name != "Translation")
                    {
                        if (R.Name.Length > 0)
                            Property = R.Name;
                        R.Read();
                        if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                            R.Read();
                        if (R.Name == "Translation" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                            return;
                        if (R.NodeType != System.Xml.XmlNodeType.Whitespace)
                        {
                            switch (Property)
                            {
                                case "Key":
                                    Source = R.Value;
                                    break;
                                case "Value":
                                    Translation = R.Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    //if (Source.Length > 0 && Translation.Length > 0 && !IC.Transformations.ContainsKey(Source))
                    //{
                    //    IC.TranslationDictionary.Add(Source, Translation);
                    //    Source = "";
                    //    Translation = "";
                    //    return;
                    //}
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return;
        }

        private static void LoadFileColumnSplitters(ref System.Xml.XmlTextReader R, ref DiversityWorkbench.Export.FileColumn IC)
        {
            try
            {
                //if ((R.NodeType == System.Xml.XmlNodeType.Text
                //    || R.NodeType == System.Xml.XmlNodeType.Whitespace)
                //    && R.Name == ""
                //    && R.Value.Length > 0
                //    && !IC.Splitters.Contains(R.Value))
                //{
                //    IC.Splitters.Add(R.Value);
                //}
                //while (R.Read())
                //{
                //    if (R.NodeType == System.Xml.XmlNodeType.Whitespace)
                //    {
                //        if ((R.NodeType == System.Xml.XmlNodeType.Text
                //            || R.NodeType == System.Xml.XmlNodeType.Whitespace)
                //            && R.Name == ""
                //            && R.Value.Length > 0
                //            && !IC.Splitters.Contains(R.Value))
                //        {
                //            IC.Splitters.Add(R.Value);
                //        }
                //        continue;
                //    }
                //    if (R.Name == "Splitters" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                //        return;
                //    if (!R.IsStartElement()
                //        && R.Name != "Splitter"
                //        && !IC.Splitters.Contains(R.Value)
                //        && R.NodeType == System.Xml.XmlNodeType.Text)
                //    {
                //        IC.Splitters.Add(R.Value);
                //    }
                //    else if (R.Name == "Splitter" && R.NodeType == System.Xml.XmlNodeType.Element && !R.IsEmptyElement)
                //    {
                //        R.MoveToContent();
                //        string x = R.ReadElementContentAsString();
                //        if (!IC.Splitters.Contains(x))
                //            IC.Splitters.Add(x);
                //        R.MoveToElement();
                //        if (R.Name == "Splitters" && R.NodeType == System.Xml.XmlNodeType.EndElement)
                //            return;
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return;
        }

        #endregion

        #endregion

        #endregion

        #region Export

        private static System.Collections.Generic.List<string> _ExportTableList;
        public static System.Collections.Generic.List<string> ExportTableList
        {
            get
            {
                if (Export.Exporter._ExportTableList == null)
                {
                    Export.Exporter._ExportTableList = new List<string>();
                    try
                    {
                        Export.Exporter._ExportTableList.Add(Export.Exporter.StartTable.TableAlias);
                        foreach (Export.FileColumn FC in Export.Exporter.FileColumns)
                        {
                            if (!Export.Exporter._ExportTableList.Contains(FC.TableColumn.Table.TableAlias))
                                Export.Exporter._ExportTableList.Add(FC.TableColumn.Table.TableAlias);
                        }
                        Export.Exporter.FindMissingExportTables();
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                return Export.Exporter._ExportTableList;
            }
        }

        private static void FindMissingExportTables()
        {
            //bool OK = false;
            try
            {
                string Parent = "";
                System.Collections.Generic.List<string> TablesToTest = new List<string>();
                System.Collections.Generic.List<string> ResolvedTables = new List<string>();
                for (int i = 0; i < 9; i++) // only 9 rounds as maximum
                {
                    TablesToTest.Clear();
                    // reading all current tables with a missing parent into the temporary list
                    foreach (string T in Export.Exporter._ExportTableList)
                    {
                        if (Export.Exporter.SourceTableDictionary()[T].ParentTable != null &&
                            !Export.Exporter._ExportTableList.Contains(Export.Exporter.SourceTableDictionary()[T].ParentTable.TableAlias) &&
                            Export.Exporter.SourceTableDictionary()[T].TableAlias != Export.Exporter.StartTable.TableAlias)
                        {
                            TablesToTest.Add(T);
                        }
                    }
                    if (TablesToTest.Count == 0)
                        break;

                    bool IsChildOfStartTable = false;
                    // get intermediate tables for childs of starttable
                    foreach (string T in TablesToTest)
                    {
                        IsChildOfStartTable = false;
                        Parent = Export.Exporter.SourceTableDictionary()[T].ParentTable.TableAlias;
                        for (int ii = 0; ii < 9; ii++) // only 9 rounds as maximum
                        {
                            if (Parent == Export.Exporter.StartTable.TableAlias)
                            {
                                IsChildOfStartTable = true;
                                break;
                            }
                            if (Export.Exporter.SourceTableDictionary()[Parent].ParentTable != null)
                                Parent = Export.Exporter.SourceTableDictionary()[Parent].ParentTable.TableAlias;
                            else break;
                        }
                        if (IsChildOfStartTable)
                        {
                            Parent = Export.Exporter.SourceTableDictionary()[T].ParentTable.TableAlias;
                            bool StartReached = false;
                            while (!StartReached)
                            {
                                if (Export.Exporter._ExportTableList.Contains(Parent))
                                    break;
                                Export.Exporter._ExportTableList.Add(Parent);
                                if (Export.Exporter.SourceTableDictionary()[Parent].ParentTable != null)
                                    Parent = Export.Exporter.SourceTableDictionary()[Parent].ParentTable.TableAlias;
                                else break;
                                if (Parent == Export.Exporter.StartTable.TableAlias)
                                    StartReached = true;
                            }
                            ResolvedTables.Add(T);
                        }
                    }

                    /// get intermediate tables for non child tables for start table
                    /// removing the resolved relations
                    foreach (string T in ResolvedTables)
                    {
                        if (TablesToTest.Contains(T))
                            TablesToTest.Remove(T);
                    }
                    /// getting the parents of the start table
                    System.Collections.Generic.List<string> StartTableParents = new List<string>();
                    Parent = Export.Exporter.StartTable.ParentTable.TableAlias;
                    StartTableParents.Add(Parent);
                    for (int p = 0; p < 9; p++)
                    {
                        if (Export.Exporter.SourceTableDictionary()[Parent].ParentTable != null)
                        {
                            Parent = Export.Exporter.SourceTableDictionary()[Parent].ParentTable.TableAlias;
                            StartTableParents.Add(Parent);
                        }
                        else
                            break;
                    }
                    /// for every table that is still to resolve, walk upwards until a table in the start table parent list if found
                    foreach (string T in TablesToTest)
                    {
                        if (Export.Exporter.SourceTableDictionary()[T].ParentTable != null &&
                            !Export.Exporter._ExportTableList.Contains(Export.Exporter.SourceTableDictionary()[T].ParentTable.TableAlias) &&
                            Export.Exporter.SourceTableDictionary()[T].ParentTable.TableAlias != Export.Exporter.StartTable.TableAlias)
                        {
                            System.Collections.Generic.List<string> ParentTablesOfCurrent = new List<string>();
                            Parent = Export.Exporter.SourceTableDictionary()[T].ParentTable.TableAlias;
                            ParentTablesOfCurrent.Add(Parent);
                            for (int p = 0; p < 9; p++)
                            {
                                if (Export.Exporter.SourceTableDictionary()[Parent].ParentTable != null)
                                {
                                    Parent = Export.Exporter.SourceTableDictionary()[Parent].ParentTable.TableAlias;
                                    if (StartTableParents.Contains(Parent))
                                    {
                                        foreach (string s in ParentTablesOfCurrent)
                                        {
                                            if (!Export.Exporter._ExportTableList.Contains(s))
                                                Export.Exporter._ExportTableList.Add(s);
                                        }
                                        bool StartPointReached = false;
                                        for (int s = StartTableParents.Count - 1; s >= 0; s--)
                                        {
                                            if (StartTableParents[s] == Parent)
                                                StartPointReached = true;
                                            if (StartPointReached)
                                            {
                                                if (!Export.Exporter._ExportTableList.Contains(StartTableParents[s]))
                                                    Export.Exporter._ExportTableList.Add(StartTableParents[s]);
                                            }
                                        }
                                        break;
                                    }
                                    else
                                        ParentTablesOfCurrent.Add(Parent);
                                }
                            }
                        }
                    }
                }
                //if (OK) return;
            }
            catch (System.Exception ex)
            {
            }
        }

        public static async System.Threading.Tasks.Task<System.Collections.Generic.List<System.Collections.Generic.List<string>>> TestExport(decimal MaxLines, DiversityWorkbench.Export.iExporter iExporter)
        {
            System.Collections.Generic.List<System.Collections.Generic.List<string>> LL = new List<List<string>>();
            try
            {
                System.Collections.Generic.SortedList<int, Export.FileColumn> SortedListOfFileColums = Exporter.FileColumnList;
                System.Collections.Generic.List<Export.FileColumn> ListOfFileColums = new List<FileColumn>();
                foreach (System.Collections.Generic.KeyValuePair<int, Export.FileColumn> KV in SortedListOfFileColums)
                {
                    ListOfFileColums.Add(KV.Value);
                }
                System.Collections.Generic.List<System.Collections.Generic.List<string>> exportList = await Exporter.ExportResults(MaxLines, iExporter);
                foreach (System.Collections.Generic.List<string> LSource in exportList)
                {
                    System.Collections.Generic.List<string> L = new List<string>();
                    foreach (string s in LSource)
                        L.Add(s);
                    LL.Add(L);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return LL;
        }

        /// <summary>
        /// Exports the data into the specified file
        /// </summary>
        /// <param name="ExportFile">The name of the file</param>
        /// <returns>If an exception occurred, the message created by the exception</returns>
        public static async System.Threading.Tasks.Task<string> ExportToFile(string ExportFile, System.Windows.Forms.TextBox TextBox, DiversityWorkbench.Export.iExporter iExporter)
        {
            string Message = "";
            try
            {
                System.IO.StreamWriter sw;
                sw = new System.IO.StreamWriter(ExportFile, false, System.Text.Encoding.UTF8);
                TextBox.Text = "";
                try
                {
                    int iLines = 0;
                    System.Collections.Generic.List<System.Collections.Generic.List<string>> LL = await Exporter.ExportResults(null, iExporter);
                    int MaxI = LL.Count;
                    foreach (System.Collections.Generic.List<string> L in LL)
                    {
                        string Line = "";
                        for (int i = 0; i < L.Count; i++)
                        {
                            if (i > 0) Line += "\t";
                            Line += L[i].Replace("\r\n", " ").Replace("\t", " ");
                        }
                        sw.WriteLine(Line);
                        iExporter.ShowExportProgress(iLines, MaxI, "Writing to file");
                        iLines++;
                        if (iLines < 20)
                            TextBox.Text += Line + "\r\n";
                        else TextBox.Text += "...";
                    }
                    iExporter.ShowExportProgress(MaxI, Exporter.ListOfIDs.Count, "Export finished");
                }
                catch (System.Exception ex)
                {
                    Message = ex.Message;
                    iExporter.ShowExportProgress(0, Exporter.ListOfIDs.Count, "Export error");
                }
                finally
                {
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                iExporter.ShowExportProgress(0, Exporter.ListOfIDs.Count, "Export error");
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Message;
        }

        /// <summary>
        /// Exports the data into the specified file
        /// </summary>
        /// <param name="ExportFile">The name of the file</param>
        /// <returns>If an exception occurred, the message created by the exception</returns>
        public static async System.Threading.Tasks.Task<string> ExportToXML(string ExportFile, System.Windows.Forms.TextBox TextBox, DiversityWorkbench.Export.iExporter iExporter, int MaxLines)
        {
            string Message = "";
            System.Collections.Generic.List<string> WrongColumns = await ExportToXmlValidNames(iExporter);
            if (WrongColumns.Count > 0)
            {
                Message = "The following column heaeders are not valid for a xml tag:";
                foreach (string s in WrongColumns)
                    Message += "\r\n" + s;
                //System.Windows.Forms.MessageBox.Show(Message);
                return Message;
            }
            try
            {
                System.Xml.XmlWriter W = null;
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                //System.IO.FileInfo FI = new System.IO.FileInfo(ExportFile);
                TextBox.Text = "";
                try
                {
                    settings.Encoding = System.Text.Encoding.UTF8;
                    W = System.Xml.XmlWriter.Create(ExportFile, settings);
                    W.WriteStartDocument();
                    int t = 0;
                    TextBox.Text = "<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>\r\n";
                    t++;
                    W.WriteStartElement("Export");
                    W.WriteAttributeString("Database", DiversityWorkbench.Settings.DatabaseName);
                    W.WriteAttributeString("Date", System.DateTime.Now.Year.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Day.ToString());
                    TextBox.Text += "<Export>\r\n";
                    t++;
                    System.Collections.Generic.List<System.Collections.Generic.List<string>> LL = await Exporter.ExportResults(null, iExporter);
                    int MaxI = LL.Count;
                    System.Collections.Generic.List<string> Columns = null;
                    for (int l = 0; l < LL.Count; l++)
                    {
                        if (l == 0)
                        {
                            Columns = LL[l];
                        }
                        else
                        {
                            W.WriteStartElement(Export.Exporter.StartTable.TableName); // Dataline
                            if (l < 5)
                                TextBox.Text += "<" + Export.Exporter.StartTable.TableName + ">\r\n";
                            System.Collections.Generic.List<string> Values = LL[l];
                            for (int i = 0; i < Values.Count; i++)
                            {
                                W.WriteElementString(Columns[i].Replace(" ", "_"), Values[i]);
                                if (l < 5)
                                {
                                    if (Values[i].Length > 0)
                                        TextBox.Text += "\t<" + Columns[i].Replace(" ", "_") + ">" + Values[i] + "</" + Columns[i].Replace(" ", "_") + ">\r\n";
                                    else
                                        TextBox.Text += "\t<" + Columns[i].Replace(" ", "_") + "/>\r\n";
                                }
                            }
                            if (l == 6)
                                TextBox.Text += "\t...";
                            W.WriteEndElement();//Dataline

                        }
                        iExporter.ShowExportProgress(l, MaxI, "Writing to file");
                    }

                    //foreach (System.Collections.Generic.List<string> L in LL)
                    //{
                    //    string Line = "";
                    //    for (int i = 0; i < L.Count; i++)
                    //    {
                    //        if (i > 0) Line += "\t";
                    //        Line += L[i].Replace("\r\n", " ").Replace("\t", " ");
                    //    }


                    //    iExporter.ShowExportProgress(iLines, MaxI, "Writing to file");
                    //    iLines++;
                    //    if (iLines < 20)
                    //        TextBox.Text += Line + "\r\n";
                    //    else TextBox.Text += "...";
                    //}

                    iExporter.ShowExportProgress(MaxI, Exporter.ListOfIDs.Count, "Export finished");
                    W.WriteEndElement();//Export
                    W.WriteEndDocument();
                    //W.Flush();
                    //W.Close();
                }
                catch (System.Exception ex)
                {
                    Message = ex.Message;
                    iExporter.ShowExportProgress(0, Exporter.ListOfIDs.Count, "Export error");
                }
                finally
                {
                    if (W != null)
                    {
                        W.Flush();
                        W.Close();
                    }
                }


                //System.IO.StreamWriter sw;
                //sw = new System.IO.StreamWriter(ExportFile, false, System.Text.Encoding.UTF8);
                //TextBox.Text = "";
                //try
                //{
                //    int iLines = 0;
                //    System.Collections.Generic.List<System.Collections.Generic.List<string>> LL = Exporter.ExportResults(null, iExporter);
                //    int MaxI = LL.Count;
                //    foreach (System.Collections.Generic.List<string> L in LL)
                //    {
                //        string Line = "";
                //        for (int i = 0; i < L.Count; i++)
                //        {
                //            if (i > 0) Line += "\t";
                //            Line += L[i].Replace("\r\n", " ").Replace("\t", " ");
                //        }
                //        sw.WriteLine(Line);
                //        iExporter.ShowExportProgress(iLines, MaxI, "Writing to file");
                //        iLines++;
                //        if (iLines < 20)
                //            TextBox.Text += Line + "\r\n";
                //        else TextBox.Text += "...";
                //    }
                //    iExporter.ShowExportProgress(MaxI, Exporter.ListOfIDs.Count, "Export finished");
                //}
                //catch (System.Exception ex)
                //{
                //    Message = ex.Message;
                //    iExporter.ShowExportProgress(0, Exporter.ListOfIDs.Count, "Export error");
                //}
                //finally
                //{
                //    sw.Close();
                //    sw.Dispose();
                //}
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                iExporter.ShowExportProgress(0, Exporter.ListOfIDs.Count, "Export error");
                //DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Message;
        }

        private async static System.Threading.Tasks.Task<System.Collections.Generic.List<string>> ExportToXmlValidNames(DiversityWorkbench.Export.iExporter iExporter)
        {
            System.Collections.Generic.List<System.Collections.Generic.List<string>> LL = await Exporter.ExportResults(null, iExporter);
            System.Collections.Generic.List<string> Columns = LL[0];
            System.Collections.Generic.List<string> WrongColumns = new List<string>();
            foreach (string C in Columns)
            {
                string Result = "";
                try { Result = System.Xml.XmlConvert.VerifyName(C); }
                catch { };
                if (Result.Length == 0)
                {
                    WrongColumns.Add(C);
                }
            }
            return WrongColumns;
        }

        private static void ShowXmlStarterInTextbox(System.Windows.Forms.TextBox TextBox, string Text)
        {
        }

        private static System.Collections.Generic.List<System.Collections.Generic.List<string>> _ExportResults;
        public static async System.Threading.Tasks.Task<System.Collections.Generic.List<System.Collections.Generic.List<string>>> ExportResults(decimal? MaxLines, DiversityWorkbench.Export.iExporter iExporter)
        {
            Export.Exporter._ExportTableList = null;

            Export.Exporter._CurrentFileColumnsContent = new Dictionary<int, string>();
            Export.Exporter._CurrentFileColumnsTransformedContent = new Dictionary<int, string>();
            if (Exporter._ExportResults == null)
                Exporter._ExportResults = new List<List<string>>();
            else Exporter._ExportResults.Clear();
            int i = 0;
            try
            {
                // Writing the header
                System.Collections.Generic.List<string> LHeader = new List<string>();
                System.Collections.Generic.SortedList<int, Export.FileColumn> ListOfFileColums = Exporter.FileColumnList;

                bool isFirstHeader = true;
                int PosHeader = -1;
                foreach (System.Collections.Generic.KeyValuePair<int, Export.FileColumn> KV in ListOfFileColums)
                {
                    string Header = KV.Value.Header;

                    if (isFirstHeader)
                    {
                        LHeader.Add(Header);
                        PosHeader++;
                    }
                    else
                    {
                        if (KV.Value.IsSeparatedFromPreviousColumn)
                        {
                            LHeader.Add(Header);
                            PosHeader++;
                        }
                        else
                        {
                            //LHeader[PosHeader] += Header;
                        }
                    }
                    isFirstHeader = false;
                }

                Exporter._ExportResults.Add(LHeader);

                foreach (int ID in Exporter.ListOfIDs)
                {
                    i++;
                    Export.Exporter._CurrentPosition = i;
                    iExporter.ShowExportProgress(i, Exporter.ListOfIDs.Count, "Reading data");
                    if (MaxLines != null && i > MaxLines)
                        break;

                    // reading the data from the database into the tables
                    Exporter.ReadDataFromSource(ID);

                    // writing the contents into the current file content
                    Export.Exporter._CurrentFileColumnsContent.Clear();
                    Export.Exporter._CurrentFileColumnsTransformedContent.Clear();
                    foreach (System.Collections.Generic.KeyValuePair<int, Export.FileColumn> KV in ListOfFileColums)
                    {
                        Export.Exporter.CurrentFileColumnsContent.Add(KV.Key, KV.Value.CurrentValue);
                        string TransVal = "";
                        try { TransVal = await KV.Value.TransformedValue(KV.Value.CurrentValue); }
                        catch (System.Exception ex) { }
                        Export.Exporter.CurrentFileColumnsTransformedContent.Add(KV.Key, TransVal);
                    }

                    // writing the list for the final export with transformations and fusion of file columns
                    System.Collections.Generic.List<string> L = new List<string>();
                    string CurrentValue = "";
                    int Pos = -1;
                    bool isFirstColumn = true;
                    foreach (System.Collections.Generic.KeyValuePair<int, Export.FileColumn> KV in ListOfFileColums)
                    {
                        CurrentValue = await KV.Value.TransformedValue(KV.Value.CurrentValue);
                        if (isFirstColumn)
                        {
                            L.Add(CurrentValue);
                            Pos++;
                        }
                        else
                        {
                            if (KV.Value.IsSeparatedFromPreviousColumn)
                            {
                                L.Add(CurrentValue);
                                Pos++;
                            }
                            else
                                L[Pos] += CurrentValue;
                        }
                        isFirstColumn = false;
                    }
                    Exporter._ExportResults.Add(L);
                }
                iExporter.ShowExportProgress(0, Exporter.ListOfIDs.Count, "Ready");
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                iExporter.ShowExportProgress(0, Exporter.ListOfIDs.Count, "Read error");
            }
            return Exporter._ExportResults;
        }

        private static int _CurrentPosition;
        public static int CurrentPosition { get { return Export.Exporter._CurrentPosition; } }

        private static System.Collections.Generic.Dictionary<int, string> _CurrentFileColumnsContent;
        /// <summary>
        /// Content of the file columns before transformation and still separated
        /// </summary>
        public static System.Collections.Generic.Dictionary<int, string> CurrentFileColumnsContent { get { return Export.Exporter._CurrentFileColumnsContent; } }

        private static System.Collections.Generic.Dictionary<int, string> _CurrentFileColumnsTransformedContent;
        /// <summary>
        /// Content of the file columns after transformation and still separated
        /// </summary>
        public static System.Collections.Generic.Dictionary<int, string> CurrentFileColumnsTransformedContent { get { return Export.Exporter._CurrentFileColumnsTransformedContent; } }

        public static void ResetExportResults()
        {
            Exporter._ExportResults = null;
            Exporter.initSql();
        }

        private static void ReadDataFromSource(int ID)
        {
            try
            {
                try
                {
                    foreach (DiversityWorkbench.Export.Table T in Exporter.SourceTableList())
                        T.ResetData();
                }
                catch (System.Exception ex)
                {
                }
                foreach (DiversityWorkbench.Export.Table T in Exporter.SourceTableList())
                {
                    try
                    {
                        if (Export.Exporter.ExportTableList != null)
                        {
                            try
                            {
                                if (Export.Exporter.ExportTableList.Contains(T.TableAlias))
                                    T.GetData(ID);
                            }
                            catch (System.Exception ex)
                            {
                            }
                        }
                        else
                        {
                        }
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        //private static System.Data.DataSet _DataSet;

        //private static System.Data.DataSet GetResult(int Key)
        //{
        //    if (Exporter._DataSet == null)
        //        Exporter._DataSet = new System.Data.DataSet();
        //    Exporter._DataSet.Clear();
        //    string SQL = "";
        //    foreach (Export.FileColumn FC in Exporter.FileColumns)
        //    {
        //        if (FC.TableColumn.Table.TableAlias == Exporter.StartTable.TableAlias 
        //            && FC.TableColumnUnitValue == null)
        //        {
        //            if (SQL.Length > 0) SQL += ", ";
        //            SQL += FC.TableColumn.ColumnName;
        //        }
        //    }
        //    SQL = "SELECT " + SQL + " FROM " + Exporter.StartTable.TableName +
        //        " WHERE " + Exporter.StartTable.PrimaryKeyColumnList[0] + " = " + Key;
        //    return Exporter._DataSet;
        //}

        //private static System.Collections.Generic.List<DiversityWorkbench.Export.Table> _ExportTableList;
        //private static System.Collections.Generic.List<DiversityWorkbench.Export.Table> ExportTableList()
        //{
        //    if (Exporter._ExportTableList == null)
        //    {
        //        Exporter._ExportTableList = new List<Table>();
        //        Exporter._ExportTableList.Add(Exporter.StartTable);
        //    }
        //    return Exporter._ExportTableList;
        //}

        //private static void GetExportTableListParents(DiversityWorkbench.Export.Table ChildTable)
        //{

        //}

        //private static void ResetExportTableList()
        //{
        //    Exporter._ExportTableList = null;
        //}

        private static System.Collections.Generic.Dictionary<string, string> _ExportTables;

        public static System.Collections.Generic.Dictionary<string, string> ExportTables
        {
            get
            {
                if (Exporter._ExportTables == null)
                {
                    Exporter._ExportTables = new Dictionary<string, string>();
                    foreach (Export.FileColumn FC in Exporter.FileColumns)
                    {
                        if (!Exporter._ExportTables.ContainsKey(FC.TableColumn.Table.TableAlias))
                        {
                            Exporter._ExportTables.Add(FC.TableColumn.Table.TableAlias, FC.TableColumn.Table.TableName);
                        }
                    }
                }
                return Exporter._ExportTables;
            }
            //set { Exporter._ExportTables = value; }
        }

        private static System.Collections.Generic.Dictionary<string, string> _ExportTableWhereClause;

        private static string ExportTableWhereClause(string TableName)
        {
            if (Exporter._ExportTableWhereClause == null)
                Exporter._ExportTableWhereClause = new Dictionary<string, string>();
            if (!Exporter._ExportTableWhereClause.ContainsKey(TableName))
            {
            }
            return Exporter._ExportTableWhereClause[TableName];
        }

        private static System.Collections.Generic.Dictionary<string, Export.Table> PathToStartTable(Export.Table Table, ref bool GoDown)
        {
            System.Collections.Generic.Dictionary<string, Export.Table> Path = new Dictionary<string, Export.Table>();

            // Try upwards
            GoDown = false;
            bool StarttableFound = false;
            bool TopReached = false;
            string ParentTable = Table.ParentTable.TableAlias;
            while (!StarttableFound && !TopReached)
            {
                foreach (Export.Table TT in Exporter.SourceTableList())
                {
                    if (TT.TableAlias == ParentTable)
                    {
                        Path.Add(TT.TableAlias, TT);
                        if (TT.ParentTable == null)
                            TopReached = true;
                        else
                            ParentTable = TT.ParentTable.TableAlias;
                        if (TT.TableAlias == Exporter.StartTable.TableAlias)
                            StarttableFound = true;
                        break;
                    }
                }
            }

            // Try downwards
            GoDown = true;
            if (!StarttableFound)
            {
                Path.Clear();
                bool ButtomReached = false;
                ParentTable = Table.TableAlias;
                while (!StarttableFound && !ButtomReached)
                {
                    ButtomReached = true;
                    foreach (Export.Table TT in Exporter.SourceTableList())
                    {
                        if (TT.ParentTable.TableAlias == ParentTable)
                        {
                            ButtomReached = false;
                            Path.Add(TT.TableAlias, TT);
                            if (TT.TableAlias == Exporter.StartTable.TableAlias)
                                StarttableFound = true;
                            break;
                        }
                    }
                }
            }

            return Path;
        }

        #endregion

        #region SQL

        public static bool SqlDocumentationActive = false;

        private static System.Collections.Generic.Dictionary<string, string> _SqlCommands;

        public static void initSql() { _SqlCommands = new Dictionary<string, string>(); }
        public static void addSQL(string TableAlias, string SQL)
        {
            if (_SqlCommands == null) { _SqlCommands = new Dictionary<string, string>(); }
            if (_SqlCommands.ContainsKey(TableAlias)) { _SqlCommands[TableAlias] += "\r\n" + SQL; }
            else
            {
                _SqlCommands[TableAlias] = SQL;
            }
        }

        public static string getSQL(string TableAlias) { return _SqlCommands[TableAlias]; }
        public static System.Collections.Generic.Dictionary<string, string> SqlCommands()
        {
            if (_SqlCommands == null) { _SqlCommands = new Dictionary<string, string>(); }
            return _SqlCommands;
        }
        public static string SqlCommandsToString()
        {
            if (_SqlCommands == null) { _SqlCommands = new Dictionary<string, string>(); }
            string Result = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in _SqlCommands)
            {
                Result += "\r\n-- " + kvp.Key + ":\r\n" + kvp.Value + "\r\n";
            }
            return Result;
        }
        #endregion
    }
}
