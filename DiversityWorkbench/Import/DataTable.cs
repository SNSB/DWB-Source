using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Linq.Expressions;

namespace DiversityWorkbench.Import
{
    public struct DataTableInfo
    {
        public string TableName;
        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataColumn> DataColumns;
        public System.Collections.Generic.List<string> PrimaryKeyColumns;
        public string IdentityColumn;
        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTableIndex> IndexList;
    }

    public class DataTable
    {

        #region Properties and parameter

        #region Ignored columns

        private System.Collections.Generic.List<string> _IgnoredColumns;
        /// <summary>
        /// Columns that will not be included in the import
        /// </summary>
        public System.Collections.Generic.List<string> IgnoredColumns
        {
            get { return _IgnoredColumns; }
            set { _IgnoredColumns = value; }
        }

        #endregion

        #region Attachement columns and attachmentstate

        private System.Collections.Generic.List<string> _AttachmentColumns;
        /// <summary>
        /// Columns that can be used as anchors for an import
        /// </summary>
        public System.Collections.Generic.List<string> AttachmentColumns
        {
            get { return _AttachmentColumns; }
            set { _AttachmentColumns = value; }
        }

        private bool _IsForAttachment = false;
        /// <summary>
        /// If the whole table is only used to attach data, 
        /// e.g. Import of CollectionEvents where these can be attached to a series via a SeriesCode, but no Series should be imported
        /// only possible for unique tables
        /// </summary>
        public bool IsForAttachment
        {
            get { return _IsForAttachment; }
            set
            {
                if (this.TypeOfParallelity == Parallelity.unique && value)
                    _IsForAttachment = value;
                else _IsForAttachment = false;
            }
        }

        #endregion

        #region Logging columns
        /// <summary>
        /// The columns of the table linked to the logging of a dataset
        /// </summary>
        public System.Collections.Generic.List<string> _LoggingColumns;
        public System.Collections.Generic.List<string> LoggingColumns
        {
            get
            {
                if (this._LoggingColumns == null)
                {
                    this._LoggingColumns = new List<string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                    {
                        if (KV.Key.ToLower() == "logcreatedwhen"
                            || KV.Key.ToLower() == "logcreatedby"
                            || KV.Key.ToLower() == "loginsertedwhen"
                            || KV.Key.ToLower() == "loginsertedby"
                            || KV.Key.ToLower() == "logupdatedwhen"
                            || KV.Key.ToLower() == "logupdatedby"
                            || KV.Key.ToLower() == "rowguid")
                            this._LoggingColumns.Add(KV.Key);
                    }
                }
                return this._LoggingColumns;
            }
        }

        #endregion

        #region Datasources

        private System.Collections.Generic.Dictionary<string, string> _DataSources;

        internal System.Collections.Generic.Dictionary<string, string> DataSources
        {
            get
            {
                if (this._DataSources == null)
                    this._DataSources = new Dictionary<string, string>();
                return _DataSources;
            }
            //set { _DataSources = value; }
        }

        /// <summary>
        /// Setting the SQL string for the datasource for a column where the default generation will not be appropriate 
        /// </summary>
        /// <param name="ColumnName">The name of the column</param>
        /// <param name="SQL">The complete SQL statement containing 2 columns: DisplayText and Value as a result</param>
        public void AddDatasource(string ColumnName, string SQL)
        {
            if (this.DataSources.ContainsKey(ColumnName))
                this.DataSources[ColumnName] = SQL;
            else this.DataSources.Add(ColumnName, SQL);
            if (this.DataColumns.ContainsKey(ColumnName))
                this.DataColumns[ColumnName].SqlDataSource = SQL;
        }

        #endregion

        #region Preset values

        private System.Collections.Generic.Dictionary<string, string> _PresetValues;

        internal System.Collections.Generic.Dictionary<string, string> PresetValues
        {
            get { if (this._PresetValues == null) this._PresetValues = new Dictionary<string, string>(); return _PresetValues; }
            set { _PresetValues = value; }
        }
        /// <summary>
        /// Adding a preset value that will not be changed later, e.g. LocalisationSystemID
        /// </summary>
        /// <param name="ColumnName">The name of the column</param>
        /// <param name="Value">The value for this column</param>
        public void AddPresetValue(string ColumnName, string Value)
        {
            if (this.PresetValues.ContainsKey(ColumnName))
                this.PresetValues[ColumnName] = Value;
            else this.PresetValues.Add(ColumnName, Value);
        }

        #endregion

        #region Display texts

        private System.Collections.Generic.Dictionary<string, string> _ColumnDisplayTexts;

        internal System.Collections.Generic.Dictionary<string, string> ColumnDisplayTexts
        {
            get { if (this._ColumnDisplayTexts == null) this._ColumnDisplayTexts = new Dictionary<string, string>(); return _ColumnDisplayTexts; }
            set { _ColumnDisplayTexts = value; }
        }
        /// <summary>
        ///  setting the display texts for columns that differ from the standard procedure
        ///  e.g. for the localisation where the displaytext depends upon the selected localiastion system
        /// </summary>
        /// <param name="ColumnName">The name of the column</param>
        /// <param name="DisplayText">The display text for the column</param>
        public void AddColumnDisplayText(string ColumnName, string DisplayText)
        {
            if (this.ColumnDisplayTexts.ContainsKey(ColumnName))
                this.ColumnDisplayTexts[ColumnName] = DisplayText;
            else this.ColumnDisplayTexts.Add(ColumnName, DisplayText);
            //this.DataColumns[ColumnName].DisplayText = DisplayText;
        }

        #endregion

        #region Parallelity

        private int _SequencePosition;
        /// <summary>
        /// The position of the table in the sequence of several tables on the same level
        /// </summary>
        public int SequencePosition
        {
            get { return _SequencePosition; }
            //set { _SequencePosition = value; }
        }

        private int _ParallelPosition = 1;
        /// <summary>
        /// The position within the copies of a table. Important for dependent tables and for relations within the same table, e.g. RelatedUnitID in IdentificationUnit
        /// </summary>
        public int ParallelPosition
        {
            get { return _ParallelPosition; }
            set { _ParallelPosition = value; }
        }

        private int _ParallelPositionForReferencingTable = 0;
        /// <summary>
        /// The position within the copies of a table if the parallelity type is referencing, e.g. Annotation
        /// </summary>
        public int ParallelPositionForReferencingTable
        {
            get
            {
                // MW 2017-05-04 Test
                // stack overflow - klappt so nicht
                //if (_ParallelPositionForReferencingTable == 0)
                //{
                //    foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                //    {
                //        if (KV.Value.TypeOfParallelity == Parallelity.referencing
                //            && KV.Value.TableName == this._TableName
                //            && KV.Value.ParentTableAlias == this.ParentTableAlias)
                //        {
                //            if (this._ParallelPositionForReferencingTable < KV.Value.ParallelPositionForReferencingTable)
                //                this._ParallelPositionForReferencingTable = KV.Value.ParallelPositionForReferencingTable + 1;
                //        }
                //    }
                //}

                return _ParallelPositionForReferencingTable;
            }
            set { _ParallelPositionForReferencingTable = value; }
        }

        public void setParallelPositionForReferencingTable()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KV in DiversityWorkbench.Import.Import.Tables)
            {
                if (KV.Value.TypeOfParallelity == Parallelity.referencing
                    && KV.Value.TableName == this._TableName
                    && KV.Value.ParentTableAlias == this.ParentTableAlias)
                {
                    if (this._ParallelPositionForReferencingTable < KV.Value.ParallelPositionForReferencingTable)
                        this._ParallelPositionForReferencingTable = KV.Value.ParallelPositionForReferencingTable + 1;
                }
            }
        }

        private string _ParentPositionKey;
        /// <summary>
        /// The key of the parent table
        /// </summary>
        public string ParentPositionKey
        {
            get
            {
                if (this._ParentPositionKey == null)
                {
                    if (this.ParentTableAlias != null && this.ParentTableAlias.Length > 0 && DiversityWorkbench.Import.Import.Tables.ContainsKey(this.ParentTableAlias))
                        this._ParentPositionKey = DiversityWorkbench.Import.Import.Tables[this.ParentTableAlias].PositionKey;
                    else if (this.ParentTableAlias != null && this.ParentTableAlias.Length > 0 && DiversityWorkbench.Import.Import.TemplateTables.ContainsKey(this.ParentTableAlias))
                        this._ParentPositionKey = DiversityWorkbench.Import.Import.TemplateTables[this.ParentTableAlias].PositionKey;
                    else
                        this._ParentPositionKey = "";
                }
                return _ParentPositionKey;
            }
            //set { _ParentKey = value; }
        }

        private string _PositionKey;
        // The key of the table within the whole list of tables, including the type
        public string PositionKey
        {
            get
            {
                if (this.TableAlias.IndexOf('_') > -1 && this._PositionKey != null && !this.TableAlias.EndsWith(this._ParallelPosition.ToString()))
                {
                    ///TODO - geflickt - Ursache noch zu ermitteln
                    string[] PP = this.TableAlias.Split(new char[] { '_' });
                    string P = PP.Last();
                    this._ParallelPosition = int.Parse(P);
                    this._PositionKey = null;
                    this._TableAliasKey = null;
                }
                if (this._PositionKey == null)
                {
                    if (this.TypeOfParallelity != Parallelity.unique)
                        this._PositionKey = this.ParentPositionKey;
                    else
                        this._PositionKey = "";
                    if (this._PositionKey.Length == 0)
                    {
                        this._PositionKey = "_";
                        if (((int)DiversityWorkbench.Import.Step.StepType.Table) < 10)
                            this._PositionKey += "0";
                        this._PositionKey += ((int)DiversityWorkbench.Import.Step.StepType.Table).ToString() + "_";
                    }
                    else
                        this._PositionKey += "_";
                    if (this.SequencePosition < 10)
                        this._PositionKey += "0";
                    this._PositionKey += this.SequencePosition.ToString();
                    this._PositionKey += ":";
                    if (this.ParallelPosition < 10)
                        this._PositionKey += "0";
                    this._PositionKey += this.ParallelPosition.ToString();
                }
                return _PositionKey;
            }
            //set { _PositionKey = value; }
        }

        /// <summary>
        /// If a table is unique, e.g. CollectionSpecimen, restricted e.g. CollectionEventLocalisation or parallel e.g. CollectionAgent
        /// </summary>
        public enum Parallelity { unique, restricted, parallel, referencing };
        private Parallelity _Parallelity;
        public Parallelity TypeOfParallelity
        { get { return this._Parallelity; } }

        private string _ParentTableAlias;
        /// <summary>
        /// The Alias of the parent table as defined during the creation of the steps
        /// </summary>
        public string ParentTableAlias
        {
            get { return _ParentTableAlias; }
            set
            {
                _ParentTableAlias = value;
            }
        }

        private bool? _DependsOnParentTable;
        /// <summary>
        /// If the table depends upon the import of the parent table, e.g. if the parent table contains an identity that is created during the import
        /// </summary>
        public bool? DependsOnParentTable
        {
            get
            {
                if (_DependsOnParentTable == null)
                {
                    _DependsOnParentTable = false;
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                    {
                        if (KV.Value.ForeignRelationTableAlias == this.ParentTableAlias &&
                            this.PrimaryKeyColumnList.Contains(KV.Key))
                        {
                            _DependsOnParentTable = true;
                            break;
                        }
                        else if (KV.Value.ForeignRelationTableAlias == this.ParentTableAlias &&
                            this.TypeOfParallelity == Parallelity.referencing)
                        {
                            _DependsOnParentTable = true;
                            break;
                        }
                    }
                }
                return _DependsOnParentTable;
            }
            set { _DependsOnParentTable = value; }
        }

        private string _SecondParentTableAlias;
        /// <summary>
        /// the Alias of a second parent of the table, e.g. for IdentificationUnitInPart 
        /// where data of a second parent table must be copied into the current table
        /// in the Panel the step will be placed with the primary table and with as many steps as there are available for the second table
        /// </summary>
        public string SecondParentTableAlias
        {
            get { return _SecondParentTableAlias; }
            set { _SecondParentTableAlias = value; }
        }

        private string _ThirdParentTableAlias;
        /// <summary>
        /// the Alias of a 3. parent of the table, e.g. for CollectionSpecimenImage 
        /// where data of a 3. parent table must be copied into the current table
        /// in the Panel the step will be placed with the primary table and with as many steps as there are available for the 3. table
        /// </summary>
        public string ThirdParentTableAlias
        {
            get { return _ThirdParentTableAlias; }
            set { _ThirdParentTableAlias = value; }
        }

        #endregion

        #region Names and display text

        private string _TableName;
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set { _TableName = value; }
        }

        private string _TableAlias;
        public string TableAlias
        {
            get
            {
                if (this._TableAlias == null)
                {
                    if (this._TableName != null)
                        this._TableAlias = this.TableName;
                    else
                        this._TableAlias = ""; // this should never happen
                    if (this._ParentTableAlias != null && this._ParentTableAlias.Length > 0 && DiversityWorkbench.Import.Import.Tables.ContainsKey(this._ParentTableAlias))
                    {
                        this._TableAlias += DiversityWorkbench.Import.Import.Tables[this._ParentTableAlias].TableAliasKey;
                    }
                    else if (this._ParentTableAlias != null && this._ParentTableAlias.Length > 0 && DiversityWorkbench.Import.Import.TemplateTables.ContainsKey(this._ParentTableAlias))
                        this._TableAlias += DiversityWorkbench.Import.Import.TemplateTables[this._ParentTableAlias].TableAliasKey;
                    if (this.TypeOfParallelity != Parallelity.unique)
                        this._TableAlias += "_" + this._ParallelPosition.ToString();
                }
                return this._TableAlias;
            }
        }

        private string _TableAliasKey;

        public string TableAliasKey
        {
            get
            {
                if (this._TableAliasKey == null)
                {
                    this._TableAliasKey = "";
                    if (this._ParentTableAlias != null && this._ParentTableAlias.Length > 0 && DiversityWorkbench.Import.Import.Tables.ContainsKey(this._ParentTableAlias))
                    {
                        this._TableAliasKey = DiversityWorkbench.Import.Import.Tables[this._ParentTableAlias].TableAliasKey;
                    }
                    if (this.TypeOfParallelity != Parallelity.unique)
                        this._TableAliasKey += "_" + this.ParallelPosition.ToString();
                }
                return _TableAliasKey;
            }
            //set { _TableAliasKey = value; }
        }

        public void TableAliasReset()
        {
            this._TableAliasKey = null;
            this._TableAlias = null;
        }

        private string _DisplayText;

        internal string DisplayText
        {
            get { return _DisplayText; }
            set { _DisplayText = value; }
        }

        //private string _DisplayTextParallel;
        public string GetDisplayText()
        {
            string DisplayText = this._DisplayText;
            if (this._Parallelity == Parallelity.parallel)
            {
                string Position = this.TableAliasKey.Substring(1);
                Position = Position.Replace('_', '.');
                DisplayText += " " + Position;
                //if (this.ParentTableAlias != null)
                //{
                //    if (DiversityWorkbench.Import.Import.Tables.ContainsKey(this.ParentTableAlias))
                //    {
                //        if (DiversityWorkbench.Import.Import.Tables[this.ParentTableAlias].TypeOfParallelity == Parallelity.parallel)
                //        {
                //            if (DiversityWorkbench.Import.Import.Tables[this.ParentTableAlias].ParentTableAlias != null)
                //            {
                //            }
                //            else
                //                DisplayText = this._DisplayText + " " + DiversityWorkbench.Import.Import.Tables[this.ParentTableAlias].ParallelPosition.ToString() + " " + this.ParallelPosition.ToString();
                //        }
                //        else if (DiversityWorkbench.Import.Import.Tables[this.ParentTableAlias].TypeOfParallelity == Parallelity.unique)
                //        {
                //            DisplayText = this._DisplayText + " " + this.ParallelPosition.ToString();
                //        }
                //    }
                //}
                //else
                //{
                //    DisplayText += " " + this.ParallelPosition.ToString();
                //}
            }

            //if (this._Parallelity == Parallelity.parallel)
            //{
            //    if (this._DisplayTextParallel == null)
            //    {
            //        //if (this.Step != null && this.Step.ParentStep != null)
            //        //    this._DisplayTextParallel = this._DisplayText + " " + this.Step.
            //    }
            //}

            //if (this._Parallelity == Parallelity.parallel && this.PrimaryKeyColumnList.Count > 1)
            //{
            //    string ParentTable = "";
            //    foreach (string s in this.PrimaryKeyColumnList)
            //    {
            //        DiversityWorkbench.Import.DataColumn DC = this.DataColumns[s];
            //        if (DC.ForeignRelationTableAlias != null 
            //            && DiversityWorkbench.Import.Import.Tables.ContainsKey(DC.ForeignRelationTableAlias))
            //        {
            //            DisplayText = this._DisplayText + " " + this._ParallelPosition.ToString();
            //            break;
            //        }
            //    }
            //    DisplayText = this._DisplayText + " " + this._ParallelPosition.ToString();
            //}
            return DisplayText;
        }

        #endregion

        #region Image

        private System.Drawing.Image _Image;

        public System.Drawing.Image Image
        {
            get { return _Image; }
            set { _Image = value; }
        }

        #endregion

        #region Needed action

        public enum NeededAction { Undefined, NoData, NoDifferences, ReadAttachment, Attach, Insert, Update, Correction, Error, Duplicate /*, SelectTarget*/ };
        private NeededAction _NeededAction = NeededAction.Undefined;

        public NeededAction ActionNeeded()
        {
            // testing
            //if (this._TableName == "TaxonSynonymy")
            //{ }
            // testing

            string Message = "";
            if (this._NeededAction == NeededAction.Undefined)
            {
                try
                {
                    if (this.IsForAttachment)
                        this._NeededAction = NeededAction.ReadAttachment;
                    else if (!this.DecisiveColumnsContainData())
                        this._NeededAction = NeededAction.NoData;
                    else if (DiversityWorkbench.Import.Import.AttachmentColumn != null
                        && this.TableAlias == DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias
                        && this._MergeHandling == Merging.Attach)
                        this._NeededAction = NeededAction.ReadAttachment;
                    else if (DiversityWorkbench.Import.Import.Tables.ContainsKey(this._ParentTableAlias) &&
                        DiversityWorkbench.Import.Import.Tables[this._ParentTableAlias]._NeededAction == NeededAction.NoData)
                    {
                        // Markus 21.7.2016: If there are no data in the parent table, a depending table fails to import
                        this._NeededAction = NeededAction.NoData;
                    }
                    else
                    {
                        bool ParentIsEmpty = false;
                        switch (this.MergeHandling)
                        {
                            case Merging.Insert:
                                // the data should be imported. This is only possible if the PK is missing in the database
                                // and the Not-Identity PK is complete
                                Message = this.NonIdentityPKisComplete(ref ParentIsEmpty);
                                if (!ParentIsEmpty)
                                {
                                    if (Message.Length > 0)
                                        this.AddMessage(Message);
                                    else
                                    {
                                        // Toni: Check if foreign key relations are o.k.
                                        if (!ForeignRelationsOk(ref Message))
                                        {
                                            if (Message.Length > 0)
                                            {
                                                this._NeededAction = NeededAction.Correction;
                                                this.AddMessage(Message);
                                            }
                                            else
                                                this._NeededAction = NeededAction.NoData;
                                            break;
                                        }

                                        if (this.DataAreMissingInDatabase(ref Message))
                                        {
                                            if (Message.Length == 0)
                                            {
                                                //// Markus 16.8.2016: special treatment for function derived PK values were the function runs before the insert (e.g. DiversityReferences
                                                //if (this._DataColumns[this._PrimaryKeyColumns[0]].DataRetrievalType == DataColumn.RetrievalType.RunBeforeFunctionInDatabase)
                                                //    this._NeededAction = NeededAction.Update;
                                                //else
                                                this._NeededAction = NeededAction.Insert;
                                            }
                                            else
                                            {
                                                this._NeededAction = NeededAction.Correction;
                                                this.AddMessage(Message);
                                            }
                                        }
                                        else
                                        {
                                            this._NeededAction = NeededAction.Correction;
                                            foreach (string s in this.PrimaryKeyColumnList)
                                            {
                                                Message += "\r\n" + s + ": " + this.DataColumns[s].Value;
                                            }
                                            this.AddMessage("The data are allready present in the database" + Message);
                                        }
                                    }
                                }
                                else
                                    this._NeededAction = NeededAction.NoData;
                                break;
                            case Merging.Update:
                                // the data should be updated. This is only possible if the PK is complete
                                if (!this.PKisComplete)
                                {
                                    if (!this.GetKeyDataForUpdate())
                                    {
                                        this._NeededAction = NeededAction.Correction;
                                        Message = "The primary key is not complete:";
                                        foreach (string s in this.PrimaryKeyColumnList)
                                        {
                                            if (this.DataColumns[s].Value == null || this.DataColumns[s].Value.Length == 0)
                                                Message += "\r\n" + s + " is missing";
                                        }
                                        this.AddMessage(Message);
                                        break;
                                    }
                                }
                                if (this.PKisComplete)
                                {
                                    if (this.DataAreMissingInDatabase(ref Message))
                                    {
                                        this._NeededAction = NeededAction.Correction;
                                        Message = "No dataset with the primary key:";
                                        foreach (string s in this.PrimaryKeyColumnList)
                                        {
                                            Message += "\r\n" + s + ": " + this.DataColumns[s].Value;
                                        }
                                        Message += "\r\ncould be found.";
                                        this.AddMessage(Message);
                                    }
                                    else
                                    {
                                        if (Message.Length == 0)
                                            this._NeededAction = NeededAction.Update;
                                        else
                                        {
                                            this._NeededAction = NeededAction.Correction;
                                            this.AddMessage(Message);
                                        }
                                    }
                                }
                                else
                                {
                                    this._NeededAction = NeededAction.Correction;
                                    foreach (string s in this.PrimaryKeyColumnList)
                                    {
                                        if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
                                            continue;
                                        else
                                            Message += "The column " + s + " contains no value\r\n";
                                    }
                                    this.AddMessage("The primary key is not complete: " + Message);
                                }
                                break;
                            case Merging.Merge:
                                // decide if the data should be imported or updated
                                if (this.PKisComplete)
                                {
                                    if (this.DataAreMissingInDatabase(ref Message))
                                        this._NeededAction = NeededAction.Insert;
                                    else
                                        this._NeededAction = NeededAction.Update;
                                }
                                else
                                {
                                    Message = this.NonIdentityPKisComplete(ref ParentIsEmpty);
                                    if (!ParentIsEmpty)
                                    {
                                        if (Message.Length > 0)
                                        {
                                            this._NeededAction = NeededAction.Correction;
                                            this.AddMessage(Message);
                                        }
                                        else
                                        {
                                            if (!this.GetKeyDataForUpdate())
                                            {
                                                this._NeededAction = NeededAction.Correction;
                                                break;
                                            }
                                            else
                                            {
                                                if (this.PKisComplete)
                                                    this._NeededAction = NeededAction.Update;
                                                else
                                                {
                                                    //// Markus 16.8.2016: special treatment for function derived PK values were the function runs before the insert (e.g. DiversityReferences
                                                    //if (this._DataColumns[this._PrimaryKeyColumns[0]].DataRetrievalType == DataColumn.RetrievalType.RunBeforeFunctionInDatabase)
                                                    //    this._NeededAction = NeededAction.Update;
                                                    //else
                                                    this._NeededAction = NeededAction.Insert;
                                                }
                                            }
                                        }
                                    }
                                    else
                                        this._NeededAction = NeededAction.NoData;
                                }
                                break;
                            case Merging.Attach:
                                // the data should be attached. This is only possible if the PK is complete
                                if (!this.PKisComplete)
                                {
                                    if (!this.GetKeyDataForUpdate())
                                    {
                                        // Markus 13.6.2016: Message inserted
                                        Message = "Attachment not possible to missing data or key incomplete";
                                        this._NeededAction = NeededAction.Correction;
                                        break;
                                    }
                                }
                                if (this.PKisComplete)
                                {
                                    if (this.DataAreMissingInDatabase(ref Message))
                                    {
                                        // Markus 13.6.2016: Message inserted
                                        Message = "Attachment not possible to missing data";
                                        this._NeededAction = NeededAction.Correction;
                                    }
                                    else
                                    {
                                        if (Message.Length == 0)
                                            this._NeededAction = NeededAction.Attach;
                                        else
                                        {
                                            this._NeededAction = NeededAction.Correction;
                                            this.AddMessage(Message);
                                        }
                                    }
                                }
                                else
                                {
                                    this._NeededAction = NeededAction.Correction;
                                    foreach (string s in this.PrimaryKeyColumnList)
                                    {
                                        if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
                                            continue;
                                        else
                                            Message += "The column " + s + " contains no value\r\n";
                                    }
                                    this.AddMessage("The primary key is not complete: " + Message);
                                }
                                break;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            return this._NeededAction;
        }

        public void SetActionNeeded(NeededAction Action)
        {
            this._NeededAction = Action;
        }

        //public NeededAction ActionNeeded
        //{
        //    get 
        //    {
        //        string Message = "";
        //        if (this._NeededAction == NeededAction.Undefined)
        //        {
        //            try
        //            {
        //                if (this.IsForAttachment)
        //                    this._NeededAction = NeededAction.ReadAttachment;
        //                else if (!this.DecisiveColumnsContainData())
        //                    this._NeededAction = NeededAction.NoData;
        //                else if (DiversityWorkbench.Import.Import.AttachmentColumn != null
        //                    && this.TableAlias == DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias
        //                    && this._MergeHandling == Merging.Attach)
        //                    this._NeededAction = NeededAction.ReadAttachment;
        //                else
        //                {
        //                    //if (DiversityWorkbench.Import.Import.AttachmentColumn != null)
        //                    //{
        //                    //    if (this.AttachViaParentChildRelation())
        //                    //    {
        //                    //        if (this.AttachViaParentChildRelation())
        //                    //        {
        //                    //            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
        //                    //            {
        //                    //                if (KV.Value.IsParentAttachmentColumn && (KV.Value.Value == null || KV.Value.Value.Length == 0))
        //                    //                {
        //                    //                    this.GetAttachmentData();
        //                    //                    break;
        //                    //                }
        //                    //            }
        //                    //        }
        //                    //    }
        //                    //    else
        //                    //    {
        //                    //        if (DiversityWorkbench.Import.Import.AttachmentColumn.Value == null
        //                    //            || DiversityWorkbench.Import.Import.AttachmentColumn.Value.Length == 0)
        //                    //        {
        //                    //            //this.GetAttachmentData();
        //                    //        }
        //                    //    }
        //                    //}
        //                    switch (this.MergeHandling)
        //                    {
        //                        case Merging.Insert:
        //                            // the data should be imported. This is only possible if the PK is missing in the database
        //                            // and the Not-Identity PK is complete
        //                            Message = this.NonIdentityPKisComplete;
        //                            if (Message.Length > 0)
        //                                this.AddMessage(Message);
        //                            else
        //                            {
        //                                if (this.DataAreMissingInDatabase(ref Message))
        //                                {
        //                                    if (Message.Length == 0)
        //                                        this._NeededAction = NeededAction.Insert;
        //                                    else
        //                                    {
        //                                        this._NeededAction = NeededAction.Correction;
        //                                        this.AddMessage(Message);
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    this._NeededAction = NeededAction.Correction;
        //                                    foreach (string s in this.PrimaryKeyColumnList)
        //                                    {
        //                                        Message += "\r\n" + s + ": " + this.DataColumns[s].Value;
        //                                    }
        //                                    this.AddMessage("The data are allready present in the database" + Message);
        //                                }
        //                            }
        //                            break;
        //                        case Merging.Update:
        //                            // the data should be updated. This is only possible if the PK is complete
        //                            if (!this.PKisComplete)
        //                            {
        //                                if (!this.GetKeyDataForUpdate())
        //                                {
        //                                    this._NeededAction = NeededAction.Correction;
        //                                    Message = "The primary key is not complete:";
        //                                    foreach (string s in this.PrimaryKeyColumnList)
        //                                    {
        //                                        if (this.DataColumns[s].Value == null || this.DataColumns[s].Value.Length == 0)
        //                                            Message += "\r\n" + s + " is missing";
        //                                    }
        //                                    this.AddMessage(Message);
        //                                    break;
        //                                }
        //                            }
        //                            if (this.PKisComplete)
        //                            {
        //                                if (this.DataAreMissingInDatabase(ref Message))
        //                                {
        //                                    this._NeededAction = NeededAction.Correction;
        //                                    Message = "No dataset with the primary key:";
        //                                    foreach (string s in this.PrimaryKeyColumnList)
        //                                    {
        //                                        Message += "\r\n" + s + ": " + this.DataColumns[s].Value;
        //                                    }
        //                                    Message += "\r\ncould be found.";
        //                                    this.AddMessage(Message);
        //                                }
        //                                else
        //                                {
        //                                    if (Message.Length == 0)
        //                                        this._NeededAction = NeededAction.Update;
        //                                    else
        //                                    {
        //                                        this._NeededAction = NeededAction.Correction;
        //                                        this.AddMessage(Message);
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                this._NeededAction = NeededAction.Correction;
        //                                foreach (string s in this.PrimaryKeyColumnList)
        //                                {
        //                                    if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
        //                                        continue;
        //                                    else
        //                                        Message += "The column " + s + " contains no value\r\n";
        //                                }
        //                                this.AddMessage("The primary key is not complete: " + Message);
        //                            }
        //                            break;
        //                        case Merging.Merge:
        //                            // decide if the data should be imported or updated
        //                            if (this.PKisComplete)
        //                            {
        //                                if (this.DataAreMissingInDatabase(ref Message))
        //                                    this._NeededAction = NeededAction.Insert;
        //                                else
        //                                    this._NeededAction = NeededAction.Update;
        //                            }
        //                            else
        //                            {
        //                                Message = this.NonIdentityPKisComplete;
        //                                if (Message.Length > 0)
        //                                {
        //                                    this._NeededAction = NeededAction.Correction;
        //                                    this.AddMessage(Message);
        //                                }
        //                                else
        //                                {
        //                                    if (!this.GetKeyDataForUpdate())
        //                                    {
        //                                        this._NeededAction = NeededAction.Correction;
        //                                        break;
        //                                    }
        //                                    else
        //                                    {
        //                                        if (this.PKisComplete)
        //                                            this._NeededAction = NeededAction.Update;
        //                                        else
        //                                            this._NeededAction = NeededAction.Insert;
        //                                    }
        //                                }
        //                            }
        //                            break;
        //                        case Merging.Attach:
        //                            // the data should be attached. This is only possible if the PK is complete
        //                            if (!this.PKisComplete)
        //                            {
        //                                if (!this.GetKeyDataForUpdate())
        //                                {
        //                                    this._NeededAction = NeededAction.Correction;
        //                                    break;
        //                                }
        //                            }
        //                            if (this.PKisComplete)
        //                            {
        //                                if (this.DataAreMissingInDatabase(ref Message))
        //                                {
        //                                    this._NeededAction = NeededAction.Correction;
        //                                }
        //                                else
        //                                {
        //                                    if (Message.Length == 0)
        //                                        this._NeededAction = NeededAction.Attach;
        //                                    else
        //                                    {
        //                                        this._NeededAction = NeededAction.Correction;
        //                                        this.AddMessage(Message);
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                this._NeededAction = NeededAction.Correction;
        //                                foreach (string s in this.PrimaryKeyColumnList)
        //                                {
        //                                    if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
        //                                        continue;
        //                                    else
        //                                        Message += "The column " + s + " contains no value\r\n";
        //                                }
        //                                this.AddMessage("The primary key is not complete: " + Message);
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //            catch (System.Exception ex)
        //            {
        //            }
        //        }
        //        return this._NeededAction; 
        //    }
        //    set { this._NeededAction = value; }
        //}

        //public NeededAction ActionNeeded
        //{
        ////    get 
        ////    {
        ////        string Message = "";
        ////        if (this._NeededAction == NeededAction.Undefined)
        ////        {
        ////            try
        ////            {
        ////                if (this.IsForAttachment)
        ////                    this._NeededAction = NeededAction.ReadAttachment;
        ////                else if (!this.DecisiveColumnsContainData())
        ////                    this._NeededAction = NeededAction.NoData;
        ////                else if (DiversityWorkbench.Import.Import.AttachmentColumn != null
        ////                    && this.TableAlias == DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias
        ////                    && this._MergeHandling == Merging.Attach)
        ////                    this._NeededAction = NeededAction.ReadAttachment;
        ////                else
        ////                {
        ////                    //if (DiversityWorkbench.Import.Import.AttachmentColumn != null)
        ////                    //{
        ////                    //    if (this.AttachViaParentChildRelation())
        ////                    //    {
        ////                    //        if (this.AttachViaParentChildRelation())
        ////                    //        {
        ////                    //            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
        ////                    //            {
        ////                    //                if (KV.Value.IsParentAttachmentColumn && (KV.Value.Value == null || KV.Value.Value.Length == 0))
        ////                    //                {
        ////                    //                    this.GetAttachmentData();
        ////                    //                    break;
        ////                    //                }
        ////                    //            }
        ////                    //        }
        ////                    //    }
        ////                    //    else
        ////                    //    {
        ////                    //        if (DiversityWorkbench.Import.Import.AttachmentColumn.Value == null
        ////                    //            || DiversityWorkbench.Import.Import.AttachmentColumn.Value.Length == 0)
        ////                    //        {
        ////                    //            //this.GetAttachmentData();
        ////                    //        }
        ////                    //    }
        ////                    //}
        ////                    switch (this.MergeHandling)
        ////                    {
        ////                        case Merging.Insert:
        ////                            // the data should be imported. This is only possible if the PK is missing in the database
        ////                            // and the Not-Identity PK is complete
        ////                            Message = this.NonIdentityPKisComplete;
        ////                            if (Message.Length > 0)
        ////                                this.AddMessage(Message);
        ////                            else
        ////                            {
        ////                                if (this.DataAreMissingInDatabase(ref Message))
        ////                                {
        ////                                    if (Message.Length == 0)
        ////                                        this._NeededAction = NeededAction.Insert;
        ////                                    else
        ////                                    {
        ////                                        this._NeededAction = NeededAction.Correction;
        ////                                        this.AddMessage(Message);
        ////                                    }
        ////                                }
        ////                                else
        ////                                {
        ////                                    this._NeededAction = NeededAction.Correction;
        ////                                    foreach (string s in this.PrimaryKeyColumnList)
        ////                                    {
        ////                                        Message += "\r\n" + s + ": " + this.DataColumns[s].Value;
        ////                                    }
        ////                                    this.AddMessage("The data are allready present in the database" + Message);
        ////                                }
        ////                            }
        ////                            break;
        ////                        case Merging.Update:
        ////                            // the data should be updated. This is only possible if the PK is complete
        ////                            if (!this.PKisComplete)
        ////                            {
        ////                                if (!this.GetKeyDataForUpdate())
        ////                                {
        ////                                    this._NeededAction = NeededAction.Correction;
        ////                                    Message = "The primary key is not complete:";
        ////                                    foreach (string s in this.PrimaryKeyColumnList)
        ////                                    {
        ////                                        if (this.DataColumns[s].Value == null || this.DataColumns[s].Value.Length == 0)
        ////                                            Message += "\r\n" + s + " is missing";
        ////                                    }
        ////                                    this.AddMessage(Message);
        ////                                    break;
        ////                                }
        ////                            }
        ////                            if (this.PKisComplete)
        ////                            {
        ////                                if (this.DataAreMissingInDatabase(ref Message))
        ////                                {
        ////                                    this._NeededAction = NeededAction.Correction;
        ////                                    Message = "No dataset with the primary key:";
        ////                                    foreach (string s in this.PrimaryKeyColumnList)
        ////                                    {
        ////                                        Message += "\r\n" + s + ": " + this.DataColumns[s].Value;
        ////                                    }
        ////                                    Message += "\r\ncould be found.";
        ////                                    this.AddMessage(Message);
        ////                                }
        ////                                else
        ////                                {
        ////                                    if (Message.Length == 0)
        ////                                        this._NeededAction = NeededAction.Update;
        ////                                    else
        ////                                    {
        ////                                        this._NeededAction = NeededAction.Correction;
        ////                                        this.AddMessage(Message);
        ////                                    }
        ////                                }
        ////                            }
        ////                            else
        ////                            {
        ////                                this._NeededAction = NeededAction.Correction;
        ////                                foreach (string s in this.PrimaryKeyColumnList)
        ////                                {
        ////                                    if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
        ////                                        continue;
        ////                                    else
        ////                                        Message += "The column " + s + " contains no value\r\n";
        ////                                }
        ////                                this.AddMessage("The primary key is not complete: " + Message);
        ////                            }
        ////                            break;
        ////                        case Merging.Merge:
        ////                            // decide if the data should be imported or updated
        ////                            if (this.PKisComplete)
        ////                            {
        ////                                if (this.DataAreMissingInDatabase(ref Message))
        ////                                    this._NeededAction = NeededAction.Insert;
        ////                                else
        ////                                    this._NeededAction = NeededAction.Update;
        ////                            }
        ////                            else
        ////                            {
        ////                                Message = this.NonIdentityPKisComplete;
        ////                                if (Message.Length > 0)
        ////                                {
        ////                                    this._NeededAction = NeededAction.Correction;
        ////                                    this.AddMessage(Message);
        ////                                }
        ////                                else
        ////                                {
        ////                                    if (!this.GetKeyDataForUpdate())
        ////                                    {
        ////                                        this._NeededAction = NeededAction.Correction;
        ////                                        break;
        ////                                    }
        ////                                    else
        ////                                    {
        ////                                        if (this.PKisComplete)
        ////                                            this._NeededAction = NeededAction.Update;
        ////                                        else
        ////                                            this._NeededAction = NeededAction.Insert;
        ////                                    }
        ////                                }
        ////                            }
        ////                            break;
        ////                        case Merging.Attach:
        ////                            // the data should be attached. This is only possible if the PK is complete
        ////                            if (!this.PKisComplete)
        ////                            {
        ////                                if (!this.GetKeyDataForUpdate())
        ////                                {
        ////                                    this._NeededAction = NeededAction.Correction;
        ////                                    break;
        ////                                }
        ////                            }
        ////                            if (this.PKisComplete)
        ////                            {
        ////                                if (this.DataAreMissingInDatabase(ref Message))
        ////                                {
        ////                                    this._NeededAction = NeededAction.Correction;
        ////                                }
        ////                                else
        ////                                {
        ////                                    if (Message.Length == 0)
        ////                                        this._NeededAction = NeededAction.Attach;
        ////                                    else
        ////                                    {
        ////                                        this._NeededAction = NeededAction.Correction;
        ////                                        this.AddMessage(Message);
        ////                                    }
        ////                                }
        ////                            }
        ////                            else
        ////                            {
        ////                                this._NeededAction = NeededAction.Correction;
        ////                                foreach (string s in this.PrimaryKeyColumnList)
        ////                                {
        ////                                    if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
        ////                                        continue;
        ////                                    else
        ////                                        Message += "The column " + s + " contains no value\r\n";
        ////                                }
        ////                                this.AddMessage("The primary key is not complete: " + Message);
        ////                            }
        ////                            break;
        ////                    }
        ////                }
        ////            }
        ////            catch (System.Exception ex)
        ////            {
        ////            }
        ////        }
        ////        return this._NeededAction; 
        ////    }
        //    set { this._NeededAction = value; }
        //}

        /// <summary>
        /// The color related to the needed action resp. result of the test of an import
        /// </summary>
        /// <param name="Action"></param>
        /// <returns></returns>
        public static System.Drawing.Color ActionColor(DiversityWorkbench.Import.DataTable.NeededAction Action)
        {
            switch (Action)
            {
                case NeededAction.Correction:
                case NeededAction.Error:
                    return DiversityWorkbench.Import.Import.ColorError;
                case NeededAction.Insert:
                    return DiversityWorkbench.Import.Import.ColorImport;
                case NeededAction.Update:
                    return DiversityWorkbench.Import.Import.ColorUpdate;
                case NeededAction.NoDifferences:
                    return DiversityWorkbench.Import.Import.ColorNoDifference;
                case NeededAction.Attach:
                case NeededAction.ReadAttachment:
                    return DiversityWorkbench.Import.Import.ColorAttachment;
                case NeededAction.NoData:
                    return DiversityWorkbench.Import.Import.ColorNoData;
                case NeededAction.Duplicate:
                    return DiversityWorkbench.Import.Import.ColorError;
                default:
                    return System.Drawing.Color.White;
            }
        }

        /// <summary>
        /// if values columns should be compared with values in the database
        /// </summary>
        /// <returns>The SQL restriction added to the where clause</returns>
        private string CompareKeyColumnRestriction()
        {
            string SQL = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
            {
                if (SQL.Length > 0) SQL += " AND ";
                if (KV.Value.CompareKey && KV.Value.IsSelected && KV.Value.Value != null && KV.Value.Value.Length > 0)
                    SQL += KV.Key + " = '" + KV.Value.ValueFormatedForSQL() + "'";
            }
            return SQL;
        }

        private bool _ForTesting = true;

        public bool ForTesting
        {
            get { return _ForTesting; }
            set { _ForTesting = value; }
        }

        private bool _AllowEmptyImport = false;
        /// <summary>
        /// If values of a table can be imported despite there are no values in the file of interface
        /// only the PK is available resp. can be created
        /// </summary>
        public bool AllowEmptyImport
        {
            get { return _AllowEmptyImport; }
            set { _AllowEmptyImport = value; }
        }

        #endregion

        #region Merging

        public enum Merging { Insert, Merge, Update, Attach }
        private Merging _MergeHandling = Merging.Insert;

        public Merging MergeHandling
        {
            get { return _MergeHandling; }
            set { _MergeHandling = value; }
        }

        #endregion

        #region Step

        //private DiversityWorkbench.Import.Step _Step;

        //public DiversityWorkbench.Import.Step Step
        //{
        //    get { return _Step; }
        //    set { _Step = value; }
        //}

        #endregion

        #endregion

        #region Creation of new tables

        #region Templates

        public static DiversityWorkbench.Import.DataTable GetTableTemplate(
            string TableName,
            string ParentTableAlias,
            string DisplayText,
            System.Drawing.Image Image,
            DiversityWorkbench.Import.DataTable.Parallelity Parallelity,
            int SequencePosition,
            System.Collections.Generic.List<string> IgnoredColumns,
            System.Collections.Generic.List<string> AttachentColumns)
        {//2
            int Position = 1;
            DiversityWorkbench.Import.DataTable DT;
            if (Parallelity != DataTable.Parallelity.unique)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.TemplateTables)
                {
                    if (TableName == KV.Value._TableName)
                    {
                        if (KV.Value._ParallelPosition >= Position)
                            Position++;
                    }
                }
            }
            if (Parallelity == DataTable.Parallelity.referencing)
            {
                if (Position < 10)
                    Position = Position * 10;
            }
            string TableAliasKey = "";
            if (ParentTableAlias.Length > 0)
                TableAliasKey = DiversityWorkbench.Import.Import.TemplateTables[ParentTableAlias].TableAliasKey;
            if (Parallelity != DataTable.Parallelity.unique)
                TableAliasKey += "_" + Position.ToString();
            if (!DiversityWorkbench.Import.Import.TemplateTables.ContainsKey(TableName + TableAliasKey))
            {
                DT = new DataTable(TableName, DisplayText, Image, Position, Parallelity);
                DT.IgnoredColumns = IgnoredColumns;
                DT.ParentTableAlias = ParentTableAlias;
                DT._SequencePosition = SequencePosition;
                DT._TableAliasKey = TableAliasKey;
                if (AttachentColumns != null && AttachentColumns.Count > 0)
                {
                    DT.AttachmentColumns = AttachentColumns;
                }
                DiversityWorkbench.Import.Import.TemplateTables.Add(DT.TableAlias, DT);
            }

            return DiversityWorkbench.Import.Import.TemplateTables[TableName + TableAliasKey];
        }

        #endregion

        #region Tables

        internal static DiversityWorkbench.Import.DataTable GetTable(
            string TableName,
            string DisplayText,
            System.Drawing.Image Image,
            DiversityWorkbench.Import.DataTable.Parallelity Parallelity,
            int SequencePosition)
        {
            int Position = 1;
            if (Parallelity != DataTable.Parallelity.unique)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                {
                    if (TableName == KV.Value._TableName)
                    {
                        if (KV.Value._ParallelPosition >= Position)
                            Position++;
                    }
                }
            }
            if (!DiversityWorkbench.Import.Import.Tables.ContainsKey(TableName + Position.ToString()))
            {
                DiversityWorkbench.Import.DataTable DT = new DataTable(TableName, DisplayText, Image, Position, Parallelity);
                DiversityWorkbench.Import.Import.Tables.Add(DT.TableAlias, DT);
            }
            return DiversityWorkbench.Import.Import.Tables[TableName + Position.ToString()];
        }


        internal static DiversityWorkbench.Import.DataTable GetTable(
            string TableName,
            string ParentTableAlias,
            string DisplayText,
            System.Drawing.Image Image,
            DiversityWorkbench.Import.DataTable.Parallelity Parallelity,
            int SequencePosition)
        {
            int Position = 1;
            if (Parallelity != DataTable.Parallelity.unique)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                {
                    if (TableName == KV.Value._TableName)
                    {
                        if (KV.Value._ParallelPosition >= Position)
                            Position++;
                    }
                }
            }
            if (!DiversityWorkbench.Import.Import.Tables.ContainsKey(TableName + Position.ToString()))
            {
                DiversityWorkbench.Import.DataTable DT = new DataTable(TableName, ParentTableAlias, DisplayText, Image, Position, Parallelity);
                DiversityWorkbench.Import.Import.Tables.Add(DT.TableAlias, DT);
            }
            return DiversityWorkbench.Import.Import.Tables[TableName + Position.ToString()];
        }

        #endregion

        /// <summary>
        /// Copies for child tables, dependent of a superior table
        /// </summary>
        /// <param name="DTtemplate">The template of the table that should be copied</param>
        /// <param name="IsSecondaryCopy">If the copy is done within the client window, not in the initial creation within the template</param>
        /// <param name="ParentTableAlias">The alias of the parent table, only provided for copies in the client window</param>
        /// <returns></returns>
        internal static DiversityWorkbench.Import.DataTable GetTableParallel(DiversityWorkbench.Import.DataTable DTtemplate, bool IsSecondaryCopy, string ParentTableAlias)
        {
            int Position = 0;// DTtemplate._ParallelPosition;
            if (ParentTableAlias.Length == 0)
                ParentTableAlias = DTtemplate.ParentTableAlias;
            if ((DTtemplate.TypeOfParallelity == Parallelity.parallel)
                && IsSecondaryCopy && DTtemplate.ParentTableAlias != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                {
                    if (KV.Value.ParentTableAlias == ParentTableAlias &&
                        KV.Value.TableName == DTtemplate.TableName &&
                        KV.Value.ParallelPosition > Position)
                        Position = KV.Value.ParallelPosition;
                }
                Position++;
            }
            else if (DTtemplate.TypeOfParallelity == Parallelity.restricted)
            {
                Position = DTtemplate.ParallelPosition;
            }
            else if (DTtemplate.TypeOfParallelity == Parallelity.referencing)
            {
                Position = DTtemplate.ParallelPosition;
                // MW 2017-05-04 Test
                //Position = DTtemplate.ParallelPositionForReferencingTable;
            }
            else
                Position++;
            DiversityWorkbench.Import.DataTable DT = new DataTable(DTtemplate.TableName, DTtemplate.DisplayText, DTtemplate.Image, Position, DTtemplate.TypeOfParallelity);
            DT.AttachmentColumns = DTtemplate.AttachmentColumns;
            DT.IgnoredColumns = DTtemplate.IgnoredColumns;
            DT._SequencePosition = DTtemplate.SequencePosition;
            DT._IsForAttachment = DTtemplate.IsForAttachment;
            DT.ParentTableAlias = ParentTableAlias;
            DT.AllowEmptyImport = DTtemplate.AllowEmptyImport;
            if (DTtemplate.TypeOfParallelity == Parallelity.referencing)
            {
                DT.ParallelPositionForReferencingTable = 0;
                int i = DT.ParallelPositionForReferencingTable;
            }

            // the datasources for the columns
            if (DTtemplate.DataSources.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KVsource in DTtemplate.DataSources)
                {
                    if (DT.DataColumns.ContainsKey(KVsource.Key))
                    {
                        DT.DataSources.Add(KVsource.Key, KVsource.Value);
                        DT.DataColumns[KVsource.Key].SqlDataSource = KVsource.Value;
                    }
                }
            }

            // Foreign relation alias
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in DTtemplate.DataColumns)
            {
                if (KV.Value.ForeignRelationTableAlias != null && DT.DataColumns[KV.Key].ForeignRelationTableAlias == null)
                    DT.DataColumns[KV.Key].ForeignRelationTableAlias = KV.Value.ForeignRelationTableAlias;
                if (KV.Value.ForeignRelationTable != null && DT.DataColumns[KV.Key].ForeignRelationTable == null)
                    DT.DataColumns[KV.Key].ForeignRelationTable = KV.Value.ForeignRelationTable;
                if (KV.Value.ForeignRelationColumn != null && KV.Value.ForeignRelationColumn.Length > 0 && DT.DataColumns[KV.Key].ForeignRelationColumn.Length == 0)
                    DT.DataColumns[KV.Key].ForeignRelationColumn = KV.Value.ForeignRelationColumn;

                if (KV.Value.SelectParallelForeignRelationTableAlias)
                {
                    DT.DataColumns[KV.Key].SelectParallelForeignRelationTableAlias = KV.Value.SelectParallelForeignRelationTableAlias;
                    if (KV.Value.ForeignRelationTable != null && KV.Value.ForeignRelationTable.Length > 0)
                        DT.DataColumns[KV.Key].ForeignRelationTable = KV.Value.ForeignRelationTable;
                }

                // Toni: Copy prepare processing delegates to column
                KV.Value.CopyPrepareProcessingDelegates(DT.DataColumns[KV.Key]);

                // Markus: Text fuer Display
                DT.DataColumns[KV.Key].SourceFunctionDisplayText = KV.Value.SourceFunctionDisplayText;
                DT.DataColumns[KV.Key].DataRetrievalType = KV.Value.DataRetrievalType;
                DT.DataColumns[KV.Key].DisplayText = KV.Value.DisplayText;
                if (KV.Value.DataRetrievalType == DataColumn.RetrievalType.IDorIDviaTextFromFile)
                {
                    DT.DataColumns[KV.Key].ForeignRelationTableAlias = KV.Value.ForeignRelationTableAlias;
                    DT.DataColumns[KV.Key].ForeignRelationTable = KV.Value.ForeignRelationTable;
                    DT.DataColumns[KV.Key].ForeignRelationColumn = KV.Value.ForeignRelationColumn;
                    DT.DataColumns[KV.Key].IsDecisive = KV.Value.IsDecisive;
                }

                // Markus 21.10.2016: Referencing tables
                if (DT.TypeOfParallelity == Parallelity.referencing)
                {
                    if ((KV.Value.TypeOfSource == DataColumn.SourceType.ParentTable || KV.Value.TypeOfSource == DataColumn.SourceType.Database) && KV.Key == "ReferencedID")
                    {
                        DT.DataColumns[KV.Key].ForeignRelationTableAlias = DT.ParentTableAlias;
                    }
                }

                // Markus: for columns that had been prechoosed for decision etc.
                //if (KV.Key == "IgnoreButKeepForReference") 
                //{ }
                if (KV.Value.IsDecisive &&
                    KV.Value.IsSelected &&
                    KV.Value.ValueIsPreset &&
                    KV.Value.Value != null &&
                    KV.Value.TypeOfSource == DataColumn.SourceType.Preset)
                {
                    DT.DataColumns[KV.Key].IsDecisive = KV.Value.IsDecisive;
                    DT.DataColumns[KV.Key].IsSelected = KV.Value.IsSelected;
                    DT.DataColumns[KV.Key].Value = KV.Value.Value;
                    DT.DataColumns[KV.Key].ValueIsPreset = KV.Value.ValueIsPreset;
                    DT.DataColumns[KV.Key].TypeOfSource = KV.Value.TypeOfSource;
                }
            }

            // the preset values
            if (DTtemplate.PresetValues.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KVsource in DTtemplate.PresetValues)
                {
                    DT.PresetValues.Add(KVsource.Key, KVsource.Value);
                    if (DT.DataColumns.ContainsKey(KVsource.Key))
                    {
                        DT.DataColumns[KVsource.Key].ValueIsPreset = true;
                        DT.DataColumns[KVsource.Key].TypeOfSource = DataColumn.SourceType.Preset;
                        DT.DataColumns[KVsource.Key].Value = KVsource.Value;
                    }
                }
            }

            // Markus 16.5.13: restrict to source values
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KVcolumn in DTtemplate.DataColumns)
            {
                if (DT.DataColumns.ContainsKey(KVcolumn.Key) && KVcolumn.Value.RestrictToDatasourceValues)
                {
                    DT.DataColumns[KVcolumn.Key].RestrictToDatasourceValues = KVcolumn.Value.RestrictToDatasourceValues;
                }
            }

            // the display texts
            if (DTtemplate.ColumnDisplayTexts.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KVsource in DTtemplate.ColumnDisplayTexts)
                {
                    DT.ColumnDisplayTexts.Add(KVsource.Key, KVsource.Value);
                    if (DT.DataColumns.ContainsKey(KVsource.Key))
                        DT.DataColumns[KVsource.Key].DisplayText = KVsource.Value;
                }
            }

            // the links between the tables according to the steps
            if (DTtemplate.ParentTableAlias != null && ParentTableAlias.Length == 0)
                DT.ParentTableAlias = DTtemplate.ParentTableAlias;

            //DT.TableAliasReset();

            if (!DiversityWorkbench.Import.Import.Tables.ContainsKey(DT.TableAlias))
            {
                DiversityWorkbench.Import.Import.Tables.Add(DT.TableAlias, DT);
            }
            return DT;
        }

        #region Construction

        private DataTable(
            string TableName,
            string DisplayText,
            System.Drawing.Image Image,
            int Position,
            DiversityWorkbench.Import.DataTable.Parallelity Parallelity)
        {
            this._TableName = TableName;
            this._DisplayText = DisplayText;
            this._Image = Image;
            this._ParallelPosition = Position;
            this._Parallelity = Parallelity;
        }

        private DataTable(
            string TableName,
            string ParentTableAlias,
            string DisplayText,
            System.Drawing.Image Image,
            int Position,
            DiversityWorkbench.Import.DataTable.Parallelity Parallelity)
        {
            this._TableName = TableName;
            this._DisplayText = DisplayText;
            this._Image = Image;
            this._ParallelPosition = Position;
            this._Parallelity = Parallelity;
        }

        #endregion

        #endregion

        #region Database Infos

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataColumn> _DataColumns;
        /// <summary>
        /// The Columns of the table
        /// </summary>
        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataColumn> DataColumns
        {
            get
            {
                if (this._DataColumns == null || this._DataColumns.Count == 0)
                {
                    this._DataColumns = new Dictionary<string, DataColumn>();

                    try
                    {
                        // #255: Using cached data columns if available
                        if (Table_DataColumns != null && Table_DataColumns.ContainsKey(this._TableName))
                        {
                            foreach (KeyValuePair<string, Dictionary<DataColumnInfo, string>> kv in Table_DataColumns[this._TableName])
                            {
                                DiversityWorkbench.Import.DataColumn C = new DataColumn(this, kv.Key);
                                if (kv.Value.ContainsKey(DataColumnInfo.IsNullable))
                                    C.IsNullable = kv.Value[DataColumnInfo.IsNullable] == "YES";
                                if (kv.Value.ContainsKey(DataColumnInfo.DataType))
                                    C.DataType = kv.Value[DataColumnInfo.DataType];
                                if (kv.Value.ContainsKey(DataColumnInfo.ColumnDefault) && kv.Value[DataColumnInfo.ColumnDefault] != null && kv.Value[DataColumnInfo.ColumnDefault].Length > 0)
                                    C.ColumnDefault = kv.Value[DataColumnInfo.ColumnDefault];
                                if (Table_BaseTables != null && Table_BaseTables.ContainsKey(this._TableName))
                                {
                                    if (Table_DataColumns.ContainsKey(Table_BaseTables[this._TableName])
                                        && Table_DataColumns[Table_BaseTables[this._TableName]].ContainsKey(kv.Key))
                                    {
                                        var baseCol = Table_DataColumns[Table_BaseTables[this._TableName]][kv.Key];
                                        if (!C.IsNullable && baseCol.ContainsKey(DataColumnInfo.ColumnDefault))
                                            if (baseCol[DataColumnInfo.ColumnDefault] != null && baseCol[DataColumnInfo.ColumnDefault].Length > 0)
                                                C.ColumnDefault = baseCol[DataColumnInfo.ColumnDefault];
                                    }
                                }
                                if (kv.Value.ContainsKey(DataColumnInfo.CharacterMaximumLength) && kv.Value[DataColumnInfo.CharacterMaximumLength] != null && kv.Value[DataColumnInfo.CharacterMaximumLength].Length > 0)
                                    C.MaximumLength = int.Parse(kv.Value[DataColumnInfo.CharacterMaximumLength]);
                                if (Table_IdentityColumns.ContainsKey(this._TableName) 
                                    && Table_IdentityColumns[this._TableName] == kv.Key)
                                    C.IsIdentity = true;
                                if (Table_PrimaryKeyColumns.ContainsKey(this._TableName)
                                    && Table_PrimaryKeyColumns[this._TableName].Contains(kv.Key))
                                    C.IsPartOfPrimaryKey = true;
                                C.DisplayText = kv.Key;
                                this._DataColumns.Add(kv.Key, C);
                            }
                            this.FindColumnsWithForeignRelations();
                            this.FindChildParentColumns();
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVvalue in this.PresetValues)
                            {
                                if (this._DataColumns.ContainsKey(KVvalue.Key))
                                {
                                    this._DataColumns[KVvalue.Key].Value = KVvalue.Value;
                                    this._DataColumns[KVvalue.Key].ValueIsPreset = true;
                                }
                            }
                            foreach (System.Collections.Generic.KeyValuePair<string, string> KVdisplay in this.ColumnDisplayTexts)
                            {
                                if (this._DataColumns.ContainsKey(KVdisplay.Key))
                                    this._DataColumns[KVdisplay.Key].DisplayText = KVdisplay.Value;
                            }
                            foreach (string P in this.PrimaryKeyColumnList)
                            {
                                if (this._DataColumns.ContainsKey(P))
                                    this._DataColumns[P].IsPartOfPrimaryKey = true;
                            }
                            return _DataColumns;
                        }

                        // #255 if cache not available, read from database
                        System.Data.DataTable dt = new System.Data.DataTable();
                        string SQL = "SELECT C.COLUMN_NAME, C.COLUMN_DEFAULT, C.IS_NULLABLE, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH " +
                            "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                            "WHERE (TABLE_NAME = '" + this._TableName + "') AND C.COLUMN_NAME NOT LIKE 'xx%'  AND C.DATA_TYPE NOT IN ('image', 'binary', 'varbinary', 'cursor', 'table', 'hierarchyid', 'timestamp') ";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            DiversityWorkbench.Import.DataColumn C = new DataColumn(this, R[0].ToString());
                            if (R["IS_NULLABLE"].ToString().ToLower() == "yes")
                                C.IsNullable = true;
                            else C.IsNullable = false;
                            C.DataType = R["DATA_TYPE"].ToString();
                            if (!R["COLUMN_DEFAULT"].Equals(System.DBNull.Value))
                                C.ColumnDefault = R["COLUMN_DEFAULT"].ToString();
                            if (!R["CHARACTER_MAXIMUM_LENGTH"].Equals(System.DBNull.Value))
                                C.MaximumLength = int.Parse(R["CHARACTER_MAXIMUM_LENGTH"].ToString());
                            if (this.ColumnDisplayTexts.ContainsKey(R[0].ToString()))
                                C.DisplayText = this.ColumnDisplayTexts[R[0].ToString()];
                            this._DataColumns.Add(R[0].ToString(), C);
                        }
                        SQL = "select case when min(c.name) is null then '' else min(c.name) end from sys.columns c, sys.tables t where c.is_identity = 1 " +
                            "and c.object_id = t.object_id and t.name = '" + this._TableName + "'";
                        string IdentityColumn = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (IdentityColumn.Length > 0)
                            this._DataColumns[IdentityColumn].IsIdentity = true;
                        this.FindColumnsWithForeignRelations();
                        this.FindChildParentColumns();
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KVvalue in this.PresetValues)
                        {
                            this._DataColumns[KVvalue.Key].Value = KVvalue.Value;
                            this._DataColumns[KVvalue.Key].ValueIsPreset = true;
                        }
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KVdisplay in this.ColumnDisplayTexts)
                        {
                            this._DataColumns[KVdisplay.Key].DisplayText = KVdisplay.Value;
                        }
                        foreach (string P in this.PrimaryKeyColumnList)
                            this._DataColumns[P].IsPartOfPrimaryKey = true;

                        // Markus 16.5.23: Searching base object for defaults
                        SQL = "SELECT DISTINCT d.referenced_entity_name " +
                            "FROM sys.sql_expression_dependencies d, INFORMATION_SCHEMA.TABLES T " +
                            "WHERE referencing_id = OBJECT_ID(N'" + this._TableName + "') " +
                            "AND d.referenced_entity_name = T.TABLE_NAME; ";
                        string baseTable = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                        if (baseTable.Length > 0)
                        {
                            SQL = "SELECT C.COLUMN_NAME, C.COLUMN_DEFAULT, C.IS_NULLABLE, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH " +
                            "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                            "WHERE (TABLE_NAME = '" + baseTable + "') AND C.COLUMN_NAME NOT LIKE 'xx%'  AND C.DATA_TYPE NOT IN ('image', 'binary', 'varbinary', 'cursor', 'table', 'hierarchyid', 'timestamp') ";
                            dt.Clear();
                            ad.SelectCommand.CommandText = SQL;
                            ad.Fill(dt);
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                if (this._DataColumns.ContainsKey(R[0].ToString()))
                                {
                                    if (!this._DataColumns[R[0].ToString()].IsNullable && R["COLUMN_DEFAULT"].ToString().Length > 0)
                                        this._DataColumns[R[0].ToString()].ColumnDefault = R["COLUMN_DEFAULT"].ToString();
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return _DataColumns;
            }
            //set { _DataColumns = value; }
        }

        private System.Collections.Generic.List<string> _PrimaryKeyColumns;
        /// <summary>
        /// the list of the primary key columns
        /// </summary>
        public System.Collections.Generic.List<string> PrimaryKeyColumnList
        {
            get
            {
                // #255: Using cached primary key columns if available
                if (Table_PrimaryKeyColumns.ContainsKey(this.TableName))
                {
                    this._PrimaryKeyColumns = Table_PrimaryKeyColumns[this.TableName];
                    return this._PrimaryKeyColumns;
                }

                if (this._PrimaryKeyColumns == null)
                {
                    this._PrimaryKeyColumns = new List<string>();
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        string SQL = "SELECT COLUMN_NAME " +
                            "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                            "WHERE (TABLE_NAME = '" + this._TableName + "') AND (EXISTS " +
                            "(SELECT * " +
                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                            "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                            this._PrimaryKeyColumns.Add(R[0].ToString());
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return this._PrimaryKeyColumns;
            }
        }

        /// <summary>
        /// Finding all colums that depend on another table via a foreign relation
        /// </summary>
        public void FindColumnsWithForeignRelations()
        {
            // #255: Using cached foreign key columns if available
            if (Table_ForeignKeyColumns.ContainsKey(this.TableName))
            {
                foreach (KeyValuePair<string, Tuple<string, string>> fk in Table_ForeignKeyColumns[this.TableName])
                {
                    if (this._DataColumns.ContainsKey(fk.Key))
                    {
                        if (this._DataColumns[fk.Key].ForeignRelationTable != null
                            && this._DataColumns[fk.Key].ForeignRelationTable == this.ParentTableAlias)
                        {
                            /// Werte bereits vergeben, e.g. bei IdentificationUnit wo es 2 Schluessel fuer die Spalte CollectionSpecimenID gibt. 
                            /// Hier soll nur dann was eingetragen werden, wenn die Tabelle dem Parent entspricht und dieses nicht mehr ueberschrieben werden
                        }
                        else if (this._DataColumns[fk.Key].ForeignRelationColumn != null && this._DataColumns[fk.Key].ForeignRelationTable != null)
                        {
                            string CurrentForeignTable = this._DataColumns[fk.Key].ForeignRelationTable;
                            string NewForeignTable = fk.Value.Item1;
                            string NewForeignColumn = fk.Value.Item2;
                            if (CurrentForeignTable != null && NewForeignTable != null
                                && CurrentForeignTable.Length > 0 && NewForeignTable.Length > 0
                                && CurrentForeignTable == this._TableName && NewForeignTable != this._TableName)
                            {
                                /// eine interne Relation sollte zugunsten einer externen überschrieben werden, 
                                /// e.g. Identification hat eine interne und eine externe auf IdentificationUnit
                                /// wobei letztere die wichtige ist
                                this._DataColumns[fk.Key].ForeignRelationTable = NewForeignTable;
                                this._DataColumns[fk.Key].ForeignRelationColumn = NewForeignColumn;
                            }
                        }
                        else
                        {
                            this._DataColumns[fk.Key].ForeignRelationColumn = fk.Value.Item2;
                            this._DataColumns[fk.Key].ForeignRelationTable = fk.Value.Item1;
                        }
                    }
                }
                return;
            }

            //string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn " +
            //"FROM  INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP INNER JOIN " +
            //"INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF INNER JOIN " +
            //"INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME INNER JOIN " +
            //"INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK INNER JOIN " +
            //"INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON " +
            //"FK.COLUMN_NAME = PK.COLUMN_NAME INNER JOIN " +
            //"INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
            //"INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND  " +
            //"PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
            //"WHERE (TF.CONSTRAINT_TYPE = 'FOREIGN KEY') AND " +
            //"(TF.TABLE_NAME = '" + this._TableName + "') AND (TPK.TABLE_NAME = '" + this._TableName + "')";

            // Markus 31.5.23: Optimized according to ChatGPT
            string SQL = "SELECT DISTINCT FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK " +
                "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP ON PP.TABLE_NAME = P.TABLE_NAME AND PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                "WHERE TF.CONSTRAINT_TYPE = 'FOREIGN KEY' AND (TF.TABLE_NAME = '" + this._TableName + "')";
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    string Column = R[0].ToString();
                    string ForRelTab = this._DataColumns[R[0].ToString()].ForeignRelationTable;
                    if (this._DataColumns[R[0].ToString()].ForeignRelationTable != null
                        && this._DataColumns[R[0].ToString()].ForeignRelationTable == this.ParentTableAlias)
                    {
                        /// Werte bereits vergeben, e.g. bei IdentificationUnit wo es 2 Schluessel fuer die Spalte CollectionSpecimenID gibt. 
                        /// Hier soll nur dann was eingetragen werden, wenn die Tabelle dem Parent entspricht und dieses nicht mehr ueberschrieben werden
                    }
                    else if (this._DataColumns[R[0].ToString()].ForeignRelationColumn != null && this._DataColumns[R[0].ToString()].ForeignRelationTable != null)
                    {
                        string CurrentForeignTable = this._DataColumns[R[0].ToString()].ForeignRelationTable;
                        string NewForeignTable = R["ForeignTable"].ToString();
                        ///TODO: Entscheiden war hier passieren soll. 2 Schluessel verweisen auf 2 Tabellen, welche nehmen?
                        if (CurrentForeignTable != null && NewForeignTable != null
                            && CurrentForeignTable.Length > 0 && NewForeignTable.Length > 0
                            && CurrentForeignTable == this._TableName && NewForeignTable != this._TableName)
                        {
                            /// Markus 3.12.2020
                            /// eine interne Relation sollte zugunsten einer externen überschrieben werden, 
                            /// e.g. Identification hat eine interne und eine externe auf IdentificationUnit
                            /// wobei letztere die wichtige ist
                            this._DataColumns[R[0].ToString()].ForeignRelationTable = NewForeignTable;
                        }
                    }
                    else
                    {
                        this._DataColumns[R[0].ToString()].ForeignRelationColumn = R["ForeignColumn"].ToString();
                        this._DataColumns[R[0].ToString()].ForeignRelationTable = R["ForeignTable"].ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex, SQL);
            }
        }

        /// <summary>
        /// Finding all columns that have a table internal parent child relation
        /// </summary>
        public void FindChildParentColumns()
        {
            // #255: Using cached parent key columns if available
            if (Table_ChildParentColumns.ContainsKey(this.TableName))
            {
                foreach (KeyValuePair<string, string> fk in Table_ChildParentColumns[this.TableName])
                {
                    if (this._DataColumns.ContainsKey(fk.Key))
                    {
                        this._DataColumns[fk.Key].ParentColumn = fk.Value;
                    }
                }
                return;
            }

            string SQL = "SELECT Kc.COLUMN_NAME AS ChildColumn, Kp.COLUMN_NAME AS ParentColumn " +
                "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kc INNER JOIN " +
                "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON Kc.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kp ON R.UNIQUE_CONSTRAINT_NAME = Kp.CONSTRAINT_NAME " +
                "WHERE (Kc.TABLE_NAME = '" + this._TableName + "') AND (Kp.TABLE_NAME = '" + this._TableName + "')" +
                "AND  Kc.COLUMN_NAME <> Kp.COLUMN_NAME AND Kc.ORDINAL_POSITION = Kp.ORDINAL_POSITION";
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    this._DataColumns[R[0].ToString()].ParentColumn = R["ParentColumn"].ToString();
                }
            }
            catch (System.Exception ex) { }
        }

        private string _IdentityColumn;
        /// <summary>
        /// the name of the identity column if present
        /// </summary>
        public string IdentityColumn
        {
            get
            {
                if (this._IdentityColumn == null)
                {
                    this._IdentityColumn = "";
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                    {
                        if (KV.Value.IsIdentity)
                        {
                            this._IdentityColumn = KV.Key;
                            break;
                        }
                    }
                }
                return _IdentityColumn;
            }
            //set { _IdentityColumn = value; }
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTableIndex> _IndexList;
        /// <summary>
        /// The dictionary of indices of a table
        /// </summary>
        public System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTableIndex> IndexList
        {
            get
            {
                // #255: Using cached index list if available
                if(Table_IndexList.ContainsKey(this.TableName))
                {
                    this._IndexList = Table_IndexList[this.TableName];
                    return this._IndexList;
                }

                if (this._IndexList == null || this._IndexList.Count == 0)
                {
                    this._IndexList = new Dictionary<string, DataTableIndex>();
                    try
                    {
                        string SQL = "SELECT IndexName = ind.name, ind.is_unique, ind.is_primary_key, ColumnName = col.name " +
                            "FROM  sys.indexes ind  " +
                            "INNER JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id  " +
                            "INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id  " +
                            "INNER JOIN sys.tables t ON ind.object_id = t.object_id " +
                            "WHERE t.is_ms_shipped = 0 AND t.name = '" + this.TableName + "'";
                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!this._IndexList.ContainsKey(R["IndexName"].ToString()))
                            {
                                DiversityWorkbench.Import.DataTableIndex I = new DataTableIndex();
                                I.IndexName = R["IndexName"].ToString();
                                I.IsUnique = bool.Parse(R["is_unique"].ToString());
                                I.IsPK = bool.Parse(R["is_primary_key"].ToString());
                                I.Columns = new List<string>();
                                I.Columns.Add(R["ColumnName"].ToString());
                                this._IndexList.Add(I.IndexName, I);
                            }
                            else
                            {
                                this._IndexList[R["IndexName"].ToString()].Columns.Add(R["ColumnName"].ToString());
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
                return _IndexList;
            }
            //set { _IndexList = value; }
        }

        #endregion

        #region static Methods for Database infos
        // #255

        internal static System.Collections.Generic.Dictionary<string, DiversityWorkbench.Import.DataTableInfo> _DataTableInfos = new Dictionary<string, DataTableInfo>();

        #region Primary Key Columns
        internal static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> _Table_PrimaryKeyColumns;
        internal static System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> Table_PrimaryKeyColumns
        {
            get     
            {
                if (_Table_PrimaryKeyColumns == null)
                {
                    try
                    {
                        _Table_PrimaryKeyColumns = new Dictionary<string, List<string>>();
                        System.Data.DataTable dt = new System.Data.DataTable();
                        string SQL = "SELECT COLUMN_NAME, TABLE_NAME " +
                            "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                            "WHERE (EXISTS " +
                            "(SELECT * " +
                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                            "INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS T ON K.CONSTRAINT_NAME = T.CONSTRAINT_NAME " +
                            "WHERE (T.CONSTRAINT_TYPE = 'PRIMARY KEY') AND (K.COLUMN_NAME = C.COLUMN_NAME) AND (K.TABLE_NAME = C.TABLE_NAME)))";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            string TableName = R["TABLE_NAME"].ToString();
                            if (!_Table_PrimaryKeyColumns.ContainsKey(TableName))
                            {
                                System.Collections.Generic.List<string> l = new List<string>();
                                l.Add(R["COLUMN_NAME"].ToString());
                                _Table_PrimaryKeyColumns.Add(TableName, l);
                            }
                            else
                            {
                                _Table_PrimaryKeyColumns[TableName].Add(R["COLUMN_NAME"].ToString());
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return _Table_PrimaryKeyColumns;
            }
        }
        #endregion

        #region Data Columns
        internal enum DataColumnInfo
        {
            IsNullable,
            DataType,
            ColumnDefault,
            CharacterMaximumLength
        }

        internal static System.Collections.Generic.Dictionary<string, Dictionary<string, Dictionary<DataColumnInfo, string>>> _Table_DataColumns;

        internal static System.Collections.Generic.Dictionary<string, Dictionary<string, Dictionary<DataColumnInfo, string>>> Table_DataColumns
        {
            get
            {
                if (_Table_DataColumns == null)
                {
                    _Table_DataColumns = new Dictionary<string, Dictionary<string, Dictionary<DataColumnInfo, string>>>();
                    try
                    {
                        string SQL = "SELECT C.TABLE_NAME, C.COLUMN_NAME, C.COLUMN_DEFAULT, C.IS_NULLABLE, C.DATA_TYPE, C.CHARACTER_MAXIMUM_LENGTH " +
                            "FROM INFORMATION_SCHEMA.COLUMNS AS C " +
                            "WHERE C.COLUMN_NAME NOT LIKE 'xx%' AND C.DATA_TYPE NOT IN ('image', 'binary', 'varbinary', 'cursor', 'table', 'hierarchyid', 'timestamp') ";
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        System.Data.DataTable dt = new System.Data.DataTable();
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!_Table_DataColumns.ContainsKey(R["TABLE_NAME"].ToString()))
                            {
                                Dictionary<string, Dictionary<DataColumnInfo, string>> t = new Dictionary<string, Dictionary<DataColumnInfo, string>>();
                                Dictionary<DataColumnInfo, string> d = new Dictionary<DataColumnInfo, string>();
                                d.Add(DataColumnInfo.IsNullable, R["IS_NULLABLE"].ToString());
                                d.Add(DataColumnInfo.DataType, R["DATA_TYPE"].ToString());
                                if (!R["COLUMN_DEFAULT"].Equals(System.DBNull.Value))
                                    d.Add(DataColumnInfo.ColumnDefault, R["COLUMN_DEFAULT"].ToString());
                                if (!R["CHARACTER_MAXIMUM_LENGTH"].Equals(System.DBNull.Value))
                                    d.Add(DataColumnInfo.CharacterMaximumLength, R["CHARACTER_MAXIMUM_LENGTH"].ToString());
                                t.Add(R["COLUMN_NAME"].ToString(), d);
                                _Table_DataColumns.Add(R["TABLE_NAME"].ToString(), t);
                            }
                            else if (!_Table_DataColumns[R["TABLE_NAME"].ToString()].ContainsKey(R["COLUMN_NAME"].ToString()))
                            {
                                Dictionary<DataColumnInfo, string> d = new Dictionary<DataColumnInfo, string>();
                                d.Add(DataColumnInfo.IsNullable, R["IS_NULLABLE"].ToString());
                                d.Add(DataColumnInfo.DataType, R["DATA_TYPE"].ToString());
                                if (!R["COLUMN_DEFAULT"].Equals(System.DBNull.Value))
                                    d.Add(DataColumnInfo.ColumnDefault, R["COLUMN_DEFAULT"].ToString());
                                if (!R["CHARACTER_MAXIMUM_LENGTH"].Equals(System.DBNull.Value))
                                    d.Add(DataColumnInfo.CharacterMaximumLength, R["CHARACTER_MAXIMUM_LENGTH"].ToString());
                                _Table_DataColumns[R["TABLE_NAME"].ToString()].Add(R["COLUMN_NAME"].ToString(), d);
                            }
                            else
                            {
                            }
                            //DiversityWorkbench.Import.DataColumn C = new DataColumn(this, R[0].ToString());
                            //if (R["IS_NULLABLE"].ToString().ToLower() == "yes")
                            //    C.IsNullable = true;
                            //else C.IsNullable = false;
                            //C.DataType = R["DATA_TYPE"].ToString();
                            //if (!R["COLUMN_DEFAULT"].Equals(System.DBNull.Value))
                            //    C.ColumnDefault = R["COLUMN_DEFAULT"].ToString();
                            //if (!R["CHARACTER_MAXIMUM_LENGTH"].Equals(System.DBNull.Value))
                            //    C.MaximumLength = int.Parse(R["CHARACTER_MAXIMUM_LENGTH"].ToString());
                            //if (this.ColumnDisplayTexts.ContainsKey(R[0].ToString()))
                            //    C.DisplayText = this.ColumnDisplayTexts[R[0].ToString()];
                            //this._DataColumns.Add(R[0].ToString(), C);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);

                    }
                }
                return _Table_DataColumns;
            }
        }

        #endregion

        #region Base Tables
        internal static Dictionary<string, string> _Table_BaseTables;
        internal static Dictionary<string, string> Table_BaseTables
        {
            get
            {
                if (_Table_BaseTables == null)
                {
                    _Table_BaseTables = new Dictionary<string, string>();
                    try
                    {
                        string SQL = "SELECT DISTINCT v.name AS ViewName, t.name AS BaseTableName " +
                            "FROM sys.views v " +
                            "INNER JOIN sys.sql_expression_dependencies d ON v.object_id = d.referencing_id " +
                            "INNER JOIN sys.tables t ON d.referenced_id = t.object_id " +
                            "WHERE t.is_ms_shipped = 0 ";
                        //SQL = "SELECT DISTINCT d.referenced_entity_name, T.TABLE_NAME " +
                        //    "FROM sys.sql_expression_dependencies d, INFORMATION_SCHEMA.TABLES T " +
                        //    "WHERE d.referenced_entity_name = T.TABLE_NAME; ";
                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!_Table_BaseTables.ContainsKey(R["ViewName"].ToString()))
                                _Table_BaseTables.Add(R["ViewName"].ToString(), R["BaseTableName"].ToString());
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return _Table_BaseTables;
            }
        }

        #endregion

        #region Identity Columns
        internal static Dictionary<string, string> _Table_IdentityColumns;
        internal static Dictionary<string, string> Table_IdentityColumns
        {
            get
            {
                if (_Table_IdentityColumns == null)
                {
                    try
                    {
                        _Table_IdentityColumns = new Dictionary<string, string>();
                        string SQL = "SELECT t.name AS TableName, c.name AS IdentityColumn " +
                            "FROM  sys.indexes ind  " +
                            "INNER JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id  " +
                            "INNER JOIN sys.columns c ON ic.object_id = c.object_id and ic.column_id = c.column_id  " +
                            "INNER JOIN sys.tables t ON ind.object_id = t.object_id " +
                            "WHERE t.is_ms_shipped = 0 AND c.is_identity = 1 ";
                        SQL = "select t.name AS TableName, case when min(c.name) is null then '' else min(c.name) end AS IdentityColumn from sys.columns c, sys.tables t where c.is_identity = 1 " +
                            "and c.object_id = t.object_id group by t.name";

                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            string TableName = R["TableName"].ToString();
                            if (!_Table_IdentityColumns.ContainsKey(TableName))
                            {
                                _Table_IdentityColumns.Add(TableName, R["IdentityColumn"].ToString());
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return _Table_IdentityColumns;
            }
        }

        #endregion

        #region Base Tables
        internal static System.Collections.Generic.Dictionary<string, string> _TableBaseTables;

        internal static System.Collections.Generic.Dictionary<string, string> TableBaseTables
        {
            get
            {
                if (_TableBaseTables == null)
                {
                    string SQL = "SELECT DISTINCT T.TABLE_NAME, d.referenced_entity_name " +
                        "FROM sys.sql_expression_dependencies d, INFORMATION_SCHEMA.TABLES T " +
                        "WHERE d.referenced_entity_name = T.TABLE_NAME; ";
                    _TableBaseTables = new Dictionary<string, string>();
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            string TableName = R["TABLE_NAME"].ToString();
                            if (!_TableBaseTables.ContainsKey(TableName))
                            {
                                _TableBaseTables.Add(TableName, R["referenced_entity_name"].ToString());
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }

                }
                return _TableBaseTables;
            }
        }
        #endregion

        #region Foreign Key Columns
        internal static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, Tuple<string, string>>> _Table_ForeignKeyColumns;

        internal static Dictionary<string, Dictionary<string, Tuple<string, string>>> Table_ForeignKeyColumns
        {
            get
            {
                if (_Table_ForeignKeyColumns == null)
                {
                    if (true)
                    {
                        string SQL = "SELECT DISTINCT FK.TABLE_NAME, FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn " +
                            ", CASE WHEN FK.TABLE_NAME = P.TABLE_NAME THEN 2 ELSE CASE WHEN P.TABLE_NAME LIKE '%_Enum' THEN 3 ELSE 1 END END AS RelationOrder " +
                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK " +
                            "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                            "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                            "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME " +
                            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP ON PP.TABLE_NAME = P.TABLE_NAME AND PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                            "WHERE TF.CONSTRAINT_TYPE = 'FOREIGN KEY' " +
                            "ORDER BY FK.TABLE_NAME, RelationOrder";
                        _Table_ForeignKeyColumns = new Dictionary<string, Dictionary<string, Tuple<string, string>>>();
                        try
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            ad.Fill(dt);
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                if (!_Table_ForeignKeyColumns.ContainsKey(R["TABLE_NAME"].ToString()))
                                {
                                    Dictionary<string, Tuple<string, string>> d = new Dictionary<string, Tuple<string, string>>();
                                    d.Add(R["ColumnName"].ToString(), new Tuple<string, string>(R["ForeignTable"].ToString(), R["ForeignColumn"].ToString()));
                                    _Table_ForeignKeyColumns.Add(R["TABLE_NAME"].ToString(), d);
                                }
                                else
                                {
                                    if (!_Table_ForeignKeyColumns[R["TABLE_NAME"].ToString()].ContainsKey(R["ColumnName"].ToString()))
                                    {
                                        _Table_ForeignKeyColumns[R["TABLE_NAME"].ToString()].Add(R["ColumnName"].ToString(), new Tuple<string, string>(R["ForeignTable"].ToString(), R["ForeignColumn"].ToString()));
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    else
                    {
                        System.Collections.Generic.List<string> TableNameRestrictions = new List<string>();
                        TableNameRestrictions.Add(" AND FK.TABLE_NAME <> P.TABLE_NAME");// #288
                        TableNameRestrictions.Add(" AND FK.TABLE_NAME = P.TABLE_NAME");

                        string SQL = "SELECT DISTINCT FK.TABLE_NAME, FK.COLUMN_NAME AS ColumnName, P.TABLE_NAME AS ForeignTable, PP.COLUMN_NAME AS ForeignColumn " +
                            "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK " +
                            "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                            "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                            "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME " +
                            "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP ON PP.TABLE_NAME = P.TABLE_NAME AND PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                            "WHERE TF.CONSTRAINT_TYPE = 'FOREIGN KEY'";
                        _Table_ForeignKeyColumns = new Dictionary<string, Dictionary<string, Tuple<string, string>>>();
                        try
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                            foreach (string restr in TableNameRestrictions)
                            {
                                string SQLr = SQL + restr;
                                ad.SelectCommand.CommandText = SQLr;
                                ad.Fill(dt);
                                foreach (System.Data.DataRow R in dt.Rows)
                                {
                                    if (!_Table_ForeignKeyColumns.ContainsKey(R["TABLE_NAME"].ToString()))
                                    {
                                        Dictionary<string, Tuple<string, string>> d = new Dictionary<string, Tuple<string, string>>();
                                        d.Add(R["ColumnName"].ToString(), new Tuple<string, string>(R["ForeignTable"].ToString(), R["ForeignColumn"].ToString()));
                                        _Table_ForeignKeyColumns.Add(R["TABLE_NAME"].ToString(), d);
                                    }
                                    else
                                    {
                                        if (!_Table_ForeignKeyColumns[R["TABLE_NAME"].ToString()].ContainsKey(R["ColumnName"].ToString()))
                                        {
                                            _Table_ForeignKeyColumns[R["TABLE_NAME"].ToString()].Add(R["ColumnName"].ToString(), new Tuple<string, string>(R["ForeignTable"].ToString(), R["ForeignColumn"].ToString()));
                                        }
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }

                }
                return _Table_ForeignKeyColumns;
            }
        }

        #endregion

        #region Child Parent Columns
        internal static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> _Table_ChildParentColumns;

        internal static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> Table_ChildParentColumns
        {
            get
            {
                if (_Table_ChildParentColumns == null)
                {
                    _Table_ChildParentColumns = new Dictionary<string, Dictionary<string, string>>();
                    string SQL = "SELECT Kc.TABLE_NAME, Kc.COLUMN_NAME AS ChildColumn, Kp.COLUMN_NAME AS ParentColumn " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kc INNER JOIN " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON Kc.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                        "INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS Kp ON R.UNIQUE_CONSTRAINT_NAME = Kp.CONSTRAINT_NAME " +
                        "WHERE (Kc.TABLE_NAME = Kp.TABLE_NAME) " +
                        "AND  Kc.COLUMN_NAME <> Kp.COLUMN_NAME AND Kc.ORDINAL_POSITION = Kp.ORDINAL_POSITION";
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!_Table_ChildParentColumns.ContainsKey(R["TABLE_NAME"].ToString()))
                            {
                                Dictionary<string, string> d = new Dictionary<string, string>();
                                d.Add(R["ChildColumn"].ToString(), R["ParentColumn"].ToString());
                                _Table_ChildParentColumns.Add(R["TABLE_NAME"].ToString(), d);
                            }
                            else
                            {
                                if (!_Table_ChildParentColumns[R["TABLE_NAME"].ToString()].ContainsKey(R["ChildColumn"].ToString()))
                                {
                                    _Table_ChildParentColumns[R["TABLE_NAME"].ToString()].Add(R["ChildColumn"].ToString(), R["ParentColumn"].ToString());
                                }
                            }
                        }
                    }
                    catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
                }
                return _Table_ChildParentColumns;
            }
        }
        #endregion

        #region Index Columns
        internal static System.Collections.Generic.Dictionary<string, Dictionary<string, DiversityWorkbench.Import.DataTableIndex>> _Table_IndexList;
        internal static System.Collections.Generic.Dictionary<string, Dictionary<string, DiversityWorkbench.Import.DataTableIndex>> Table_IndexList
        {
            get 
            {
                if( _Table_IndexList == null)
                {
                    _Table_IndexList = new Dictionary<string, Dictionary<string, DiversityWorkbench.Import.DataTableIndex>>();
                    string SQL = "SELECT t.name AS TableName, IndexName = ind.name, ind.is_unique, ind.is_primary_key, ColumnName = col.name " +
                        "FROM  sys.indexes ind  " +
                        "INNER JOIN sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id  " +
                        "INNER JOIN sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id  " +
                        "INNER JOIN sys.tables t ON ind.object_id = t.object_id " +
                        "WHERE t.is_ms_shipped = 0 ";
                    try
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                        ad.Fill(dt);
                        foreach (System.Data.DataRow R in dt.Rows)
                        {
                            if (!_Table_IndexList.ContainsKey(R["TableName"].ToString()))
                            {
                                Dictionary<string, DiversityWorkbench.Import.DataTableIndex> d = new Dictionary<string, DataTableIndex>();
                                DiversityWorkbench.Import.DataTableIndex I = new DataTableIndex();
                                I.IndexName = R["IndexName"].ToString();
                                I.IsUnique = bool.Parse(R["is_unique"].ToString());
                                I.IsPK = bool.Parse(R["is_primary_key"].ToString());
                                I.Columns = new List<string>();
                                I.Columns.Add(R["ColumnName"].ToString());
                                d.Add(R["IndexName"].ToString(), I);
                                _Table_IndexList.Add(R["TableName"].ToString(), d);
                            }
                            else if (!_Table_IndexList[R["TableName"].ToString()].ContainsKey(R["IndexName"].ToString()))
                            {
                                DiversityWorkbench.Import.DataTableIndex I = new DataTableIndex();
                                I.IndexName = R["IndexName"].ToString();
                                I.IsUnique = bool.Parse(R["is_unique"].ToString());
                                I.IsPK = bool.Parse(R["is_primary_key"].ToString());
                                I.Columns = new List<string>();
                                I.Columns.Add(R["ColumnName"].ToString());
                                _Table_IndexList[R["TableName"].ToString()].Add(R["IndexName"].ToString(), I);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                }
                return _Table_IndexList; 
            }
        }
        #endregion

        internal static void ClearDataTableInfos()
        {
            _DataTableInfos.Clear();
        }

        #endregion


        #region Table message

        public enum TableMessageType { ColumnNotSeleced, DecisiveColumnNotSelected, CompareKeyNotSelected, TypeOfSouceNotSelected, FileColumnNotSelected, ValueMissing, ValueNotSelected, OK }

        /// <summary>
        /// The message about the ToDo's before an import can start
        /// </summary>
        /// <param name="Message">The returned message</param>
        /// <returns>The type of the state of the table</returns>
        public TableMessageType GetTableMessage(ref string Message)
        {
            DiversityWorkbench.Import.DataTable.TableMessageType MessageType = TableMessageType.OK;
            try
            {
                if (this.IsForAttachment)
                {
                    if (DiversityWorkbench.Import.Import.AttachmentColumn.FileColumn == null)
                    {
                        Message = "Attachment by " + DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName + ":\r\nPosition in the file?";
                        return TableMessageType.FileColumnNotSelected;
                    }
                    else
                        return TableMessageType.OK;
                }

                /// Check if any column is selected
                bool AnyColumnIsSelected = false;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.IsSelected &&
                        (KV.Value.TypeOfSource == DataColumn.SourceType.File ||
                        KV.Value.TypeOfSource == DataColumn.SourceType.Interface ||
                        KV.Value.TypeOfSource == DataColumn.SourceType.NotDecided 
                        /* 
                        || (KV.Value.ForeignRelationTableAlias != null && KV.Value.ForeignRelationTableAlias.Length > 0 && KV.Value.TypeOfSource == DataColumn.SourceType.Database) 
                        */ //#53 - Nebeneffekte für andere Optionen unklar, daher wieder deaktiviert
                        )) 
                    {
                        AnyColumnIsSelected = true;
                        break;
                    }
                }
                if (!AnyColumnIsSelected && this.MergeHandling != Merging.Attach && !this.AllowEmptyImport)
                {
                    Message = "Please select at least one column\r\n other than the primary key column";
                    return TableMessageType.ColumnNotSeleced;
                }


                /// Check if any decisive column is selected
                bool DecisiveColumnIsSelected = false;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (this.MergeHandling == Merging.Attach
                        && DiversityWorkbench.Import.Import.AttachmentColumn != null
                        && DiversityWorkbench.Import.Import.AttachmentColumn == KV.Value)
                    {
                    }
                    if (KV.Value.IsDecisive)
                    {
                        //if (DiversityWorkbench.Import.Import.AttachmentColumn == null ||
                        //    DiversityWorkbench.Import.Import.AttachmentColumn != KV.Value)
                        //{
                        DecisiveColumnIsSelected = true;
                        break;
                        //}
                    }
                    if (KV.Value.IsMultiColumn)
                    {
                        foreach (DiversityWorkbench.Import.ColumnMulti C in KV.Value.MultiColumns)
                        {
                            if (C.IsDecisive)
                            {
                                DecisiveColumnIsSelected = true;
                                break;
                            }
                        }
                    }
                }
                if (!DecisiveColumnIsSelected && this.MergeHandling != Merging.Attach && !this.AllowEmptyImport)
                {
                    Message = "Decision if line should be imported\r\nPlease select at least one decisive column";
                    return TableMessageType.DecisiveColumnNotSelected;
                }

                /// Check if the compare key columns are selected
                if (this.MergeHandling == Merging.Merge || this.MergeHandling == Merging.Update || this.MergeHandling == Merging.Attach)
                {
                    bool CompareColumnIsSelected = false;
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                    {
                        if (KV.Value.CompareKey)
                        {
                            //if (DiversityWorkbench.Import.Import.AttachmentColumn == null ||
                            //    DiversityWorkbench.Import.Import.AttachmentColumn != KV.Value)
                            //{
                            CompareColumnIsSelected = true;
                            break;
                            //}
                        }
                    }
                    if (!CompareColumnIsSelected && !this.AllowEmptyImport)
                    {
                        //if (DiversityWorkbench.Import.Import.AttachmentColumn != null
                        //    && DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == this.TableAlias)
                        //{
                        Message = "Comparision with data in the database:\r\nSelect at least one column for comparision";
                        return TableMessageType.CompareKeyNotSelected;
                        //}
                        //else if (this.
                    }
                }

                /// Check if the type of source is selected
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.IsSelected &&
                        KV.Value.TypeOfSource == DataColumn.SourceType.NotDecided)
                    {
                        if (this.IgnoredColumns.Contains(KV.Key))
                        {
                            continue;
                        }
                        if (!string.IsNullOrEmpty(KV.Value.ForeignRelationTableAlias) &&
                            !string.IsNullOrEmpty(KV.Value.ForeignRelationColumn) &&
                            DiversityWorkbench.Import.Import.Tables.ContainsKey(KV.Value.ForeignRelationTableAlias) &&
                            DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns.ContainsKey(KV.Value.ForeignRelationColumn) &&
                            DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].TypeOfSource == DataColumn.SourceType.Database)
                        {
                            continue;
                        }
                        if (KV.Value.ValueIsPreset &&
                            KV.Value.Value != null &&
                            KV.Value.Value.Length > 0)
                            continue;
                        if (KV.Value.SelectParallelForeignRelationTableAlias)
                        {
                            if (KV.Value.ForeignRelationTableAlias != null && KV.Value.ForeignRelationTableAlias.Length > 0)
                                continue;
                            else
                            {
                                Message = KV.Key + ":\r\nPlease select the related source";
                                return TableMessageType.ValueNotSelected;
                            }
                        }
                        if (DiversityWorkbench.Import.Import.AttachmentColumn != null
                            && KV.Value == DiversityWorkbench.Import.Import.AttachmentColumn)
                        {
                            if (KV.Value.TypeOfSource == DataColumn.SourceType.NotDecided)
                                KV.Value.TypeOfSource = DataColumn.SourceType.Database;
                            if (KV.Value.FileColumn == null)
                            {
                                Message = KV.Key + ":\r\nPlease select the position in the file";
                                return TableMessageType.FileColumnNotSelected;
                            }
                            else continue;
                        }
                        if (KV.Value.IsPartOfPrimaryKey &&
                            KV.Value.ForeignRelationColumn != null &&
                            KV.Value.ForeignRelationColumn.Length > 0 &&
                            KV.Value.ForeignRelationTableAlias != null &&
                            KV.Value.ForeignRelationTableAlias.Length > 0 &&
                            KV.Value.TypeOfSource == DataColumn.SourceType.NotDecided)
                            continue;
                        if (KV.Value.ForeignRelationColumn.Length > 0 &&
                            ParentTableAlias.Length > 0 &&
                            DiversityWorkbench.Import.Import.Tables[ParentTableAlias].DataColumns.ContainsKey(KV.Value.ForeignRelationColumn) &&
                            DiversityWorkbench.Import.Import.Tables[ParentTableAlias].DataColumns[KV.Value.ForeignRelationColumn].IsPartOfPrimaryKey &&
                            KV.Value.ForeignRelationColumn != null &&
                            KV.Value.ForeignRelationColumn.Length > 0 &&
                            KV.Value.ForeignRelationTableAlias != null &&
                            KV.Value.ForeignRelationTableAlias.Length > 0 &&
                            KV.Value.TypeOfSource == DataColumn.SourceType.NotDecided)
                            continue;
                        // Markus 27.03.2020: RowGUID should not be included in any case
                        if (KV.Value.ColumnDefault == "(newsequentialid())") // || KV.Value.ColumnName == "RowGUID")
                            continue;
                        Message = KV.Key + ":\r\nFrom file or For all?";
                        return TableMessageType.TypeOfSouceNotSelected;
                    }
                }

                /// Check if the file column is selected
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.IsSelected &&
                        KV.Value.TypeOfSource == DataColumn.SourceType.File &&
                        KV.Value.FileColumn == null)
                    {
                        Message = KV.Key + ":\r\nPlease select the position in the file";
                        return TableMessageType.FileColumnNotSelected;
                    }
                }
                if (DiversityWorkbench.Import.Import.AttachmentColumn != null
                    && DiversityWorkbench.Import.Import.AttachmentColumn.FileColumn == null
                    && DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == this.TableAlias)
                {
                    Message = "Attachment via " + DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName + ":\r\nPlease select the position in the file";
                    return TableMessageType.FileColumnNotSelected;
                }

                /// Check if the value is given
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.IsSelected &&
                        KV.Value.TypeOfSource == DataColumn.SourceType.Interface &&
                        (KV.Value.Value == null ||
                        KV.Value.Value.Length == 0) &&
                        KV.Value.SourceFunctionDisplayText.Length == 0 &&
                        !KV.Value.PrepareInsertDefined)
                    {
                        if (KV.Value.MandatoryList != null)
                        {
                            Message = KV.Key + ":\r\nPlease select a value from the list";
                            return TableMessageType.ValueNotSelected;
                        }
                        else
                        {
                            Message = KV.Key + ":\r\nPlease enter a value";
                            return TableMessageType.ValueMissing;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
            return MessageType;
        }

        #endregion

        #region Messages

        private System.Collections.Generic.Dictionary<int, string> _Messages;

        public System.Collections.Generic.Dictionary<int, string> Messages
        {
            get
            {
                if (this._Messages == null)
                    this._Messages = new Dictionary<int, string>();
                return _Messages;
            }
            set { _Messages = value; }
        }

        public void AddMessage(string Message)
        {
            if (this.Messages.ContainsKey(DiversityWorkbench.Import.Import.CurrentLine))
            {
                if (this.Messages[DiversityWorkbench.Import.Import.CurrentLine].Length > 0)
                    this.Messages[DiversityWorkbench.Import.Import.CurrentLine] += "\r\n";
                this.Messages[DiversityWorkbench.Import.Import.CurrentLine] += Message;
            }
            else this.Messages.Add(DiversityWorkbench.Import.Import.CurrentLine, Message);
        }

        #endregion

        #region Import

        public void ImportData(bool ForTesting, Microsoft.Data.SqlClient.SqlConnection ImportConnection, Microsoft.Data.SqlClient.SqlTransaction ImportTransaction)
        {
            try
            {
                this.ForTesting = ForTesting;
                this._NeededAction = NeededAction.Undefined;
                string Message = "";
                switch (this.ActionNeeded())
                {
                    case NeededAction.Insert:
                        this.InsertData(ImportConnection, ImportTransaction, ref Message);
                        break;
                    case NeededAction.Update:
                        this.UpdateData(ImportConnection, ImportTransaction, ref Message);
                        break;
                    // 6.10.2014 MW - eingebaut da sonst Spalten aus uebergeordenten Tabellen nicht korrekt eingetragen werden
                    case NeededAction.Attach:
                        if (this.ParentTableAlias != null && this.ParentTableAlias.Length > 0 && DiversityWorkbench.Import.Import.Tables[this.ParentTableAlias]._NeededAction == NeededAction.Insert)
                            this.UpdateData(ImportConnection, ImportTransaction, ref Message);
                        break;
                    //case NeededAction.ReadAttachment:
                    //    //this.GetAttachmentData();
                    //    break;
                    default:
                        break;
                }
                if (Message.Length > 0)
                    this._NeededAction = NeededAction.Error;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// If the merge handling of the table is Update try to get the PK according to the given data
        /// </summary>
        /// <returns>If retrieval of data was successful</returns>
        public bool GetKeyDataForUpdate()
        {
            try
            {
                string SQL = "";
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += s;
                }
                SQL = "SELECT " + SQL + " FROM [" + this._TableName + "]  WITH (NOLOCK) WHERE 1 = 1 ";
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    // Toni: Compare key if related key is marked for check
                    bool compareParent = false;
                    if (!KV.Value.CompareKey && KV.Value.ForeignRelationTableAlias != null && KV.Value.ForeignRelationTableAlias.Length > 0)
                    {
                        // Check if related table key must be compared
                        compareParent = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].CompareKey;
                    }
                    if (KV.Value.CompareKey || compareParent)
                    {
                        if (KV.Value.CheckGetValueDefined) // Toni: Get value if check/get processing is defined
                        {
                            string Message = "";
                            if (KV.Value.CheckGetProcessedValue(ref Message))
                            {
                                if (Message != "")
                                    this.AddMessage(Message);
                                SQL += "AND " + KV.Key + " = '" + KV.Value.ProcessedValue + "' ";
                            }
                        }
                        else if (KV.Value.TransformedValue() != null && KV.Value.TransformedValue().Length > 0)
                        {
                            string Value = KV.Value.TransformedValue();
                            if (Value.IndexOf("'") > -1)
                                Value = Value.Replace("'", "''");
                            SQL += "AND " + KV.Key + " = '" + Value + "' ";
                        }
                        else if (compareParent // Toni 20181219: Read value from related table
                                 && DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value != null
                                 && DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value.Length > 0)
                        {
                            string Value = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value;
                            if (Value.IndexOf("'") > -1)
                                Value = Value.Replace("'", "''");
                            SQL += "AND " + KV.Key + " = '" + Value + "' ";
                        }
                        else
                            SQL += "AND (" + KV.Key + " IS NULL OR RTRIM(" + KV.Key + ") = '') ";
                    }
                }
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    if (this.DataColumns[s].TransformedValue() != null && this.DataColumns[s].TransformedValue().Length > 0)
                    {
                        SQL += "AND " + s + " = '" + this.DataColumns[s].TransformedValue() + "' ";
                    }
                }
                System.Data.DataTable dt = new System.Data.DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    foreach (string s in this.PrimaryKeyColumnList)
                    {
                        if (dt.Rows[0][s].Equals(System.DBNull.Value))
                            return false;
                        this.DataColumns[s].Value = dt.Rows[0][s].ToString();
                    }
                    if (this.MergeHandling == Merging.Merge)
                    {
                        this._NeededAction = NeededAction.Update;
                    }
                    return true;
                }
                else if (dt.Rows.Count == 0)
                {
                    string Message = "No datasets where found for these values: ";
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                    {
                        if (KV.Value.CompareKey && KV.Value.TransformedValue() != null && KV.Value.TransformedValue().Length > 0)
                        {
                            Message += "\r\nColumn " + KV.Key + ": " + KV.Value.TransformedValue();
                        }
                    }
                    if (this.MergeHandling == Merging.Merge)
                    {
                        this._NeededAction = NeededAction.Insert;
                        return true;
                    }
                    else
                    {
                        this._NeededAction = NeededAction.Correction;
                        this.AddMessage(Message);
                        return false;
                    }
                }
                else
                {
                    this._NeededAction = NeededAction.Correction;
                    string Message = "Values for ";
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                    {
                        if (KV.Value.CompareKey)
                        {
                            Message += " Column " + KV.Key + ": ";
                            if (KV.Value.Value != null)
                            {
                                if (KV.Value.TransformedValue().Length > 0)
                                    Message += KV.Value.TransformedValue();
                                else Message += "''";
                            }
                            else Message += "[No value found]";
                        }
                    }
                    Message += " are not unique";
                    this.AddMessage(Message);
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                this.AddMessage(ex.Message);
                return false;
            }
            return false;
        }

        #region Attachment

        // Markus 21.01.2022 - Make attachemnt value available for later usage
        private string _AttachmentValue;
        public string AttachmentValue { get { return _AttachmentValue; } }

        /// <summary>
        /// Getting the data as defined in the attachement columns if present
        /// </summary>
        /// <returns>true: either no attachment column was defined or the retrieval of data was successful</returns>
        public bool GetAttachmentData()
        {
            if (DiversityWorkbench.Import.Import.AttachmentColumn.FileColumn == null)
                return true;
            _AttachmentValue = "";
            if (this.AttachViaParentChildRelation())
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.IsParentAttachmentColumn)
                    {
                        _AttachmentValue = DiversityWorkbench.Import.Import.LineValuesFromFile()[(int)KV.Value.FileColumn];

                        // Toni: Convert attachment value
                        if (DiversityWorkbench.Import.Import.AttachmentColumn.CheckGetValueDefined)
                        {
                            string message = "";
                            _AttachmentValue = DiversityWorkbench.Import.Import.AttachmentColumn.CheckGetProcessedValue(AttachmentValue, ref message);
                            if (message != "")
                                this.AddMessage(message);
                        }

                        KV.Value.ValueAttachmentParent = _AttachmentValue;
                        break;
                    }
                    else
                    {
#if DEBUG
                        //// Markus 20.01.2022 - enable Transformation for attachment
                        //if (KV.Value.Transformations.Count > 0)
                        //{
                        //    _AttachmentValue = DiversityWorkbench.Import.DataColumn.TransformedValue(AttachmentValue, KV.Value.Transformations, DiversityWorkbench.Import.Import.LineValuesFromFile());
                        //}
                        //// writing attachment value in colum
                        //if (this._MergeHandling == Merging.Attach)
                        //{
                        //    _AttachmentValue = DiversityWorkbench.Import.Import.LineValuesFromFile()[(int)KV.Value.FileColumn];
                        //    KV.Value.Value = AttachmentValue;
                        //}
#endif
                    }
                }
            }
            else
            {
                // Markus 18.7.22: Take converted value
                if (DiversityWorkbench.Import.Import.AttachmentUseTransformation)
                    _AttachmentValue = DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias].DataColumns[DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName].TransformedValue();
                else
                    _AttachmentValue = DiversityWorkbench.Import.Import.LineValuesFromFile()[(int)DiversityWorkbench.Import.Import.AttachmentColumn.FileColumn];

                // Toni: Convert attachment value
                if (DiversityWorkbench.Import.Import.AttachmentColumn.CheckGetValueDefined)
                {
                    // Toni 20200625: If attachment value is empty, try to read value from attachment column
                    if (AttachmentValue == null || AttachmentValue.Length == 0)
                        _AttachmentValue = DiversityWorkbench.Import.Import.AttachmentColumn.PreviousValue;
                    else if (DiversityWorkbench.Import.Import.AttachmentColumn.CopyPrevious)
                        DiversityWorkbench.Import.Import.AttachmentColumn.PreviousValue = AttachmentValue;
                    string message = "";
                    _AttachmentValue = DiversityWorkbench.Import.Import.AttachmentColumn.CheckGetProcessedValue(AttachmentValue, ref message);
                    if (message != "")
                        this.AddMessage(message);
                }
            }
            string SQL = "";
            foreach (string s in this.PrimaryKeyColumnList)
            {
                if (SQL.Length > 0) SQL += ", ";
                SQL += s;
            }
            SQL = "SELECT " + SQL + " FROM [" + this._TableName +
                "] WHERE " + DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName + " = '" + AttachmentValue + "'";
            System.Data.DataTable dt = new System.Data.DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count == 1)
            {
                if (this.AttachViaParentChildRelation() && !this.IsForAttachment)
                {
                    string ParentColumn = this.AttachmentParentColumn();
                    string Column = this.DataColumns[ParentColumn].ForeignRelationColumn;
                    SQL = "SELECT " + Column + " FROM [" + this._TableName +
                        "] WHERE " + DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName + " = '" + AttachmentValue + "'";
                    //SQL = "SELECT " + this.AttachmentParentColumn() + " FROM [" + this._TableName +
                    //    "] WHERE " + DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName + " = '" + AttachmentValue + "'";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    try
                    {
                        con.Open();
                        string ParentValue = C.ExecuteScalar()?.ToString() ?? string.Empty;
                        if (ParentValue.Length > 0)
                        {
                            this.DataColumns[this.AttachmentParentColumn()].Value = ParentValue;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    finally
                    {
                        con.Close();
                        con.Dispose();
                    }
                }
                else
                {
                    foreach (System.Data.DataColumn C in dt.Columns)
                    {
                        if (this.DataColumns[C.ColumnName].PrepareInsertDefined || this.DataColumns[C.ColumnName].PrepareUpdateDefined)
                            this.DataColumns[C.ColumnName].ProcessedValue = dt.Rows[0][C.ColumnName].ToString();
                        else
                            this.DataColumns[C.ColumnName].Value = dt.Rows[0][C.ColumnName].ToString();

                        if (this.DataColumns[C.ColumnName].ForeignRelationColumn != null
                            && this.DataColumns[C.ColumnName].ForeignRelationTableAlias != null)
                        {
                            if (DiversityWorkbench.Import.Import.Tables[this.DataColumns[C.ColumnName].ForeignRelationTableAlias].DataColumns[this.DataColumns[C.ColumnName].ForeignRelationColumn].Value == null
                                || DiversityWorkbench.Import.Import.Tables[this.DataColumns[C.ColumnName].ForeignRelationTableAlias].DataColumns[this.DataColumns[C.ColumnName].ForeignRelationColumn].Value.Length == 0)
                                DiversityWorkbench.Import.Import.Tables[this.DataColumns[C.ColumnName].ForeignRelationTableAlias].DataColumns[this.DataColumns[C.ColumnName].ForeignRelationColumn].Value = dt.Rows[0][C.ColumnName].ToString();
                        }
                        //DiversityWorkbench.Import.Import.Tables[this.TableAlias].DataColumns[C.ColumnName].Value = dt.Rows[0][C.ColumnName].ToString();
                    }
                }
            }
            else if (dt.Rows.Count == 0)
            {
                if (DiversityWorkbench.Import.Import.AttachmentColumn.IsDecisive && AttachmentValue == "")
                {
                    this._NeededAction = NeededAction.NoData;
                }
                else
                {
                    string Message = "No datasets where found for the value " + AttachmentValue +
                        " for the column " + DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName;
                    if (this.MergeHandling == Merging.Merge)
                    {
                        this._NeededAction = NeededAction.Insert;
                    }
                    else
                    {
                        this._NeededAction = NeededAction.Correction;
                        this.AddMessage(AttachmentValue + " in colomn " + DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName + " is missing");
                        return false;
                    }
                }
            }
            else
            {
                this._NeededAction = NeededAction.Correction;
                this.AddMessage(AttachmentValue + " in colomn " + DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName + " is not unique");
                return false;
            }
            return true;
        }

        private bool? _AttachViaParentChildRelation;
        /// <summary>
        /// if the attachment should be performed via a parent child relation in the database
        /// e.g. for table Analysis in DiversityCollection
        /// where the AnalysisID is the only PK
        /// </summary>
        /// <returns>If the attachment is within the same table via a child parent relation</returns>
        public bool AttachViaParentChildRelation()
        {
            if (this._AttachViaParentChildRelation == null)
            {
                if (this.PrimaryKeyColumnList.Count > 1)
                    _AttachViaParentChildRelation = false;
                else if (this.IsForAttachment)
                    _AttachViaParentChildRelation = false;
                else
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                    {
                        if (KV.Value.ParentColumn != null &&
                            KV.Value.ParentColumn.Length > 0 &&
                            this.PrimaryKeyColumnList.Contains(KV.Value.ParentColumn) &&
                            !this.IgnoredColumns.Contains(KV.Key)) // Markus 22.11.2019 - if ignored colums contain this column, do not take it
                            _AttachViaParentChildRelation = true;
                    }
                }
                if (_AttachViaParentChildRelation == null)
                    _AttachViaParentChildRelation = false;
            }
            return (bool)_AttachViaParentChildRelation;
        }

        /// <summary>
        /// For a child parent relation the column in the PK to which the data should be attached, e.g. SeriesID for CollectionEventSeries
        /// </summary>
        /// <returns>The name of the column</returns>
        public string AttachmentParentColumn()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
            {
                if (KV.Value.ParentColumn != null &&
                    KV.Value.ParentColumn.Length > 0 &&
                    this.PrimaryKeyColumnList.Contains(KV.Value.ParentColumn))
                    return KV.Value.ColumnName;
            }
            return "";
        }

        #endregion

        /// <summary>
        /// Reading the data that were read from the grid into the LineValuesFromFile in the columns of the table
        /// </summary>
        public bool ReadDataFromFile()
        {
            bool OK = true;
            try
            {
                if (this.IsForAttachment)// The data for attaching had allready been retrieved
                    return true;
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.TypeOfSource == DataColumn.SourceType.File && KV.Value.FileColumn != null)
                    {
                        if (!DiversityWorkbench.Import.Import.LineValuesFromFile().ContainsKey((int)KV.Value.FileColumn))
                        {
                            this.Messages.Add(DiversityWorkbench.Import.Import.CurrentLine, "Column " + KV.Value.FileColumn.ToString() + " was missing in data");
                            OK = false;
                        }
                        else
                            KV.Value.Value = DiversityWorkbench.Import.Import.LineValuesFromFile()[(int)KV.Value.FileColumn];
                    }
                    else if ((KV.Value.TypeOfSource == DataColumn.SourceType.Database || KV.Value.TypeOfSource == DataColumn.SourceType.ParentTable) && KV.Value.Value != null)
                        KV.Value.Value = "";
                    else if (KV.Value.TypeOfSource == DataColumn.SourceType.NotDecided && KV.Value.Value != null)
                        KV.Value.Value = "";
                }

            }
            catch (Exception ex)
            {
                OK = false;
            }
            if (!OK)
            {
                this.AddMessage("The transfer of the data from the file failed");
                this._NeededAction = NeededAction.Error;
            }
            return OK;
        }

        #region Checking data

        /// <summary>
        /// Check if all values of the PK columns are filled
        /// </summary>
        private bool PKisComplete
        {
            get
            {
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
                        continue;
                    else
                    {
                        // 26.2.2015 MW: Query for values in parent tables if so far these were not copied into dependent tables
                        if (this._DataColumns[s].ForeignRelationTableAlias != null &&
                            this._DataColumns[s].ForeignRelationTableAlias.Length > 0 &&
                            this._DataColumns[s].ForeignRelationColumn != null &&
                            this._DataColumns[s].ForeignRelationColumn.Length > 0 &&
                            DiversityWorkbench.Import.Import.TableListForImport.Contains(this._DataColumns[s].ForeignRelationTableAlias) &&
                            DiversityWorkbench.Import.Import.Tables[this._DataColumns[s].ForeignRelationTableAlias].DataColumns[this._DataColumns[s].ForeignRelationColumn].Value != null)
                            this._DataColumns[s].Value = DiversityWorkbench.Import.Import.Tables[this._DataColumns[s].ForeignRelationTableAlias].DataColumns[this._DataColumns[s].ForeignRelationColumn].Value;
                        if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
                            continue;
                        // 7.4.2015 MW: Preset values should be checked as wel
                        if (this.PresetValues.ContainsKey(s) && this.PresetValues[s].Length > 0)
                            continue;
                        else
                            return false;
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Check all values of the PK columns excluding the identity columns
        /// </summary>
        private string NonIdentityPKisComplete(ref bool ParentTableDecisionColumnsAreEmpty)
        {
            //get
            //{
            string Message = "";
            foreach (string s in this.PrimaryKeyColumnList)
            {
                try // #288
                {
                    if ((this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
                        || this.DataColumns[s].IsIdentity
                        || (this.DataColumns[s].ColumnDefault != null && this.DataColumns[s].ColumnDefault.Length > 0)
                        || (this.DataColumns[s].PrepareInsertDefined && this.DataColumns[s].DataRetrievalType != DataColumn.RetrievalType.Default))
                        continue;
                    // MW 15.10.2015:  Multicolumn war hier nicht beruecksichtigt
                    else if (this.DataColumns[s].Value != null && this.DataColumns[s].IsMultiColumn && this.DataColumns[s].TransformedValue().Length > 0)
                        continue;
                    else
                    {
                        if (this.DataColumns[s].ForeignRelationColumn != null && this.DataColumns[s].ForeignRelationColumn.Length > 0 &&
                            this.DataColumns[s].ForeignRelationTable != null && this.DataColumns[s].ForeignRelationTable.Length > 0)
                        {
                            this.DataColumns[s].Value = this.GetNonIdentityPK(this.DataColumns[s].ForeignRelationTableAlias, this.DataColumns[s].ForeignRelationTable, this.DataColumns[s].ForeignRelationColumn);
                            if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
                                continue;
                        }


                        if (this.DataColumns[s].ForeignRelationColumn != null && this.DataColumns[s].ForeignRelationColumn.Length > 0 &&
                            this.DataColumns[s].ForeignRelationTable != null && this.DataColumns[s].ForeignRelationTable.Length > 0)
                        {
                            string TableAlias = "";
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                            {
                                if (KV.Value.TableName == this.DataColumns[s].ForeignRelationTable)
                                {
                                    TableAlias = KV.Key;
                                    break;
                                }
                            }
                            if (TableAlias.Length > 0)
                            {
                                if (DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns[this.DataColumns[s].ForeignRelationColumn].Value != null &&
                                    DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns[this.DataColumns[s].ForeignRelationColumn].Value.Length > 0 &&
                                    DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns[this.DataColumns[s].ForeignRelationColumn].ForeignRelationTableAlias != null &&
                                    DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns[this.DataColumns[s].ForeignRelationColumn].ForeignRelationTableAlias].TypeOfParallelity == Parallelity.unique)
                                {
                                    this.DataColumns[s].Value = DiversityWorkbench.Import.Import.Tables[TableAlias].DataColumns[this.DataColumns[s].ForeignRelationColumn].Value;
                                }
                                else
                                {
                                    //string ForeignRelationTableAlias = this.DataColumns[s].ForeignRelationTableAlias;
                                    if (this.DataColumns[s].ForeignRelationTableAlias != null &&
                                        this.DataColumns[s].ForeignRelationTableAlias.Length > 0 &&
                                        this.DataColumns[s].ForeignRelationColumn != null && this.DataColumns[s].ForeignRelationColumn.Length > 0 &&
                                        this.DataColumns[s].ForeignRelationTable != null && this.DataColumns[s].ForeignRelationTable.Length > 0 &&
                                        DiversityWorkbench.Import.Import.Tables[this.DataColumns[s].ForeignRelationTableAlias].DecisiveColumnsContainData())
                                    {
                                        Message += "The value for the column " + s + " is missing\r\n";
                                    }
                                    else if (this.DataColumns[s].ForeignRelationTableAlias != null && // #288
                                        !DiversityWorkbench.Import.Import.Tables[this.DataColumns[s].ForeignRelationTableAlias].DecisiveColumnsContainData())
                                    {
                                        ParentTableDecisionColumnsAreEmpty = true;
                                    }
                                    else // #288
                                    {
                                        if (this.DataColumns[s].ForeignRelationTableAlias == null && this.DataColumns[s].ForeignRelationColumn != null && this.DataColumns[s].ForeignRelationTable == this.DataColumns[s].DataTable.TableName)
                                        {
                                            // the table contains an internal relation that has been used instead of the relation to the outer parent table
                                            // therefore the alias had not been set
                                        }
                                    }
                                }
                            }
                        }
                        else
                            Message += "The value for the column " + s + " is missing\r\n";
                    }
                }
                catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
            return Message;
            //}
        }

        /// <summary>
        /// Walk through the hierarchy of the tables upwards until either the hierarchy ends or the value is found
        /// </summary>
        /// <param name="ForeignRelationTableAlias">The name of the foreign table alias</param>
        /// <param name="ForeignRelationTable">The name of the foreign table</param>
        /// <param name="ForeignRelationColumn">The name of the foreign column</param>
        /// <returns></returns>
        private string GetNonIdentityPK(string ForeignRelationTableAlias, string ForeignRelationTable, string ForeignRelationColumn)
        {
            string Value = "";
            try
            {
                if (ForeignRelationTableAlias != null && DiversityWorkbench.Import.Import.Tables.ContainsKey(ForeignRelationTableAlias))
                {
                    if (DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].Value != null
                        && DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].Value.Length > 0)
                        Value = DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].Value;
                    else if (DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationColumn != null
                        && DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationColumn.Length > 0)
                    {
                        if (DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTableAlias != null
                            && DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTableAlias.Length > 0)
                            Value = this.GetNonIdentityPK(
                                DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTableAlias,
                                DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTable,
                                DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationColumn);
                        else if (DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTable != null
                            && DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTable.Length > 0
                            && DiversityWorkbench.Import.Import.Tables.ContainsKey(DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTable)
                            && DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTable].TypeOfParallelity == Parallelity.unique)
                            Value = this.GetNonIdentityPK(
                                DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTableAlias,
                                DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationTable,
                                DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias].DataColumns[ForeignRelationColumn].ForeignRelationColumn);
                    }
#if DEBUG
                    else
                    {
                        DiversityWorkbench.Import.DataTable dataTable = DiversityWorkbench.Import.Import.Tables[ForeignRelationTableAlias];
                        if (dataTable.AttachmentColumns.Count > 0 && dataTable._MergeHandling == Merging.Attach)
                        {
                            if (DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName == ForeignRelationColumn &&
                                DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == ForeignRelationTableAlias && 
                                dataTable.AttachmentColumns.Contains(ForeignRelationColumn))
                                {
                                    Value = dataTable.AttachmentValue;//.DataColumns[ForeignRelationColumn].Value;
                                }
                        }
                    }
#endif
                }
                else
                {
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Value;
        }

        // Toni: Check foreign key relations
        private bool ForeignRelationsOk(ref string Message)
        {
            bool ParentPresent = true;
            try
            {
                foreach (KeyValuePair<string, DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.IsSelected && KV.Value.ForeignRelationColumn != null && KV.Value.ForeignRelationColumn != "" && KV.Value.TransformedValue() == "")
                    {
                        if (KV.Value.ForeignRelationTableAlias != null && Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].IsDecisive)
                        {
                            NeededAction action = Import.Tables[KV.Value.ForeignRelationTableAlias].ActionNeeded();
                            if (action == NeededAction.ReadAttachment)
                            {
                                if (Import.Tables[KV.Value.ForeignRelationTableAlias].GetAttachmentData() && Import.Tables[KV.Value.ForeignRelationTableAlias].ActionNeeded() == NeededAction.NoData)
                                {
                                    ParentPresent = false;
                                    break;
                                }
                            }
                            else if (action == NeededAction.NoData)
                            {
                                ParentPresent = false;
                                break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                Message = ex.Message;
            }
            return ParentPresent;
        }

        /// <summary>
        /// If the PK is complete, check if the data are missing in the database
        /// </summary>
        /// <param name="Message">a failure information</param>
        /// <returns>if the data are missing</returns>
        private bool DataAreMissingInDatabase(ref string Message)
        {
            string WhereClause = "";
            bool DataMissing = true;
            bool IdentityOnly = true;
            try
            {
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    if (!this.DataColumns[s].IsIdentity)
                    {
                        IdentityOnly = false;
                    }

                    if ((this.DataColumns[s].Value == null || this.DataColumns[s].Value == "") && this.DataColumns[s].IsIdentity)
                    {
                        WhereClause = "";
                        DataMissing = true;
                        break;
                    }
                    else if ((this.DataColumns[s].Value == null || this.DataColumns[s].Value.Length == 0) &&
                        (this.DataColumns[s].ColumnDefault != null && this.DataColumns[s].ColumnDefault.Length > 0))
                    {
                        if (WhereClause.Length == 0)
                            WhereClause = " WHERE ";
                        else WhereClause += " AND ";
                        WhereClause += s + " = " + this.DataColumns[s].ColumnDefault;
                    }
                    else if (this.DataColumns[s].CheckGetValueDefined)
                    {
                        // Toni: Get key value from database
                        if (WhereClause.Length == 0)
                            WhereClause = " WHERE ";
                        else WhereClause += " AND ";
                        if (this.DataColumns[s].CheckGetProcessedValue(ref Message))
                            WhereClause += s + " = " + this.DataColumns[s].ValueFormatedForSQL();
                        else
                        {
                            WhereClause = "";
                            DataMissing = true;
                            break;
                        }
                    }
                    else if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
                    {
                        if (WhereClause.Length == 0)
                            WhereClause = " WHERE ";
                        else WhereClause += " AND ";
                        if (this.DataColumns[s].DataRetrievalType == DataColumn.RetrievalType.IDorIDviaTextFromFile && this.DataColumns[s].PrepareInsertDefined)
                        {
                            this.DataColumns[s].PrepareInsert(DiversityWorkbench.Settings.Connection, null, ref Message);
                            if (Message.Length > 0)
                            {
                                WhereClause = "";
                                break;
                            }
                            WhereClause += s + " = " + this.DataColumns[s].ValueFormatedForSQL();
                        }
                        else
                            WhereClause += s + " = " + this.DataColumns[s].ValueFormatedForSQL();
                    }
                    else if (this.DataColumns[s].DataRetrievalType == DataColumn.RetrievalType.FunctionInDatabase && this.DataColumns[s].PrepareInsertDefined)
                    {
                        WhereClause = "";
                        DataMissing = true;
                        break;
                    }
                    else if (this.DataColumns[s].DataRetrievalType == DataColumn.RetrievalType.IDorIDviaTextFromFile && this.DataColumns[s].PrepareInsertDefined)
                    {
                        WhereClause = "";
                        DataMissing = true;
                        break;
                    }
                    // MW 31.3.2015: getting preset values
                    else if (this.DataColumns[s].ValueIsPreset && this.PresetValues.ContainsKey(s) && this.PresetValues[s].Length > 0)
                    {
                        if (WhereClause.Length == 0)
                            WhereClause = " WHERE ";
                        else WhereClause += " AND ";
                        WhereClause += s + " = '" + this.PresetValues[s] + "'";
                    }
                    // MW 15.10.2015 - Transformed value for multicolums
                    else if (this.DataColumns[s].Value != null && this.DataColumns[s].IsMultiColumn && this.DataColumns[s].TransformedValue().Length > 0)
                    {
                        if (WhereClause.Length == 0)
                            WhereClause = " WHERE ";
                        else WhereClause += " AND ";
                        WhereClause += s + " = '" + this.DataColumns[s].TransformedValue() + "'";
                    }
                    else
                    {
                        Message = "The value for the column " + s + " is missing";
                        break;
                    }
                }
                if (!IdentityOnly && WhereClause.Length > 0)
                {
                    string SQL = "SELECT COUNT(*) FROM " + this.TableName + WhereClause;
                    Message = "";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message, IsolationLevel.ReadUncommitted);
                    if (Result != "0")
                        DataMissing = false;
                    if (Message.Length > 0)
                    {
                    }
                }
                else if (IdentityOnly && WhereClause.Length > 0)
                {
                    string SQL = "SELECT COUNT(*) FROM " + this.TableName + WhereClause;
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message, IsolationLevel.ReadUncommitted);
                    if (Result != "0")
                        DataMissing = false;
                }
                // Markus 3.6.24: for tables that are represented as views esp. related tables like ExternalIdentifier in DC
                else if (PrimaryKeyColumnList.Count == 0 && WhereClause.Length == 0) 
                {
                    // Test if is view
                    string SqlView = "select cast(case when COUNT(*) > 0 then 1 else 0 end as bit) AS IsView from INFORMATION_SCHEMA.VIEWS v where v.TABLE_NAME = '" + this.TableName + "'";
                    string IsView = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SqlView, ref Message, IsolationLevel.ReadUncommitted);
                    bool isView = false;
                    bool.TryParse(IsView, out isView);
                    if (isView)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> column in this.DataColumns)
                        {
                            if (column.Value.IsDecisive || (column.Value.IsIdentityInParent))
                            {
                                if (WhereClause.Length == 0)
                                    WhereClause = " WHERE ";
                                else WhereClause += " AND ";
                                WhereClause += column.Key + " = " + column.Value.ValueFormatedForSQL();
                            }
                        }

                        string SQL = "SELECT COUNT(*) FROM " + this.TableName + WhereClause;
                        string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL, ref Message, IsolationLevel.ReadUncommitted);
                        if (Result != "0")
                            DataMissing = false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return DataMissing;
        }

        private bool DecisiveColumnsContainData()
        {
            bool insert = false;
            try
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.IsDecisive /*&& KV.Value.Value != null && KV.Value.Value.Length > 0*/ && KV.Value.TransformedValue() != null && KV.Value.TransformedValue().Length > 0)
                    {
                        string Test = KV.Value.TransformedValue();
                        insert = true;
                        break;
                        //return true;
                    }
                    else if (DiversityWorkbench.Import.Import.AttachmentColumn != null
                        && KV.Value == DiversityWorkbench.Import.Import.AttachmentColumn)
                    {
                        insert = true;
                        break;
                        //return true;
                    }
                    else if (KV.Value.IsDecisive && (KV.Value.Value == null || KV.Value.Value.Length == 0))
                    {
                        if (KV.Value.ForeignRelationColumn != null && KV.Value.ForeignRelationColumn.Length > 0 &&
                            KV.Value.ForeignRelationTableAlias != null && KV.Value.ForeignRelationTableAlias.Length > 0 &&
                            DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value != null &&
                            DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value.Length > 0)
                        {
                            KV.Value.Value = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value;
                            insert = true;
                            break;
                            //return true;
                        }
                        // Markus: Adaption for delegates and if value can be taken via function
                        else if (KV.Value.IsDecisive && KV.Value.PrepareInsertDefined && KV.Value.SourceFunctionDisplayText.Length > 0)
                        {
                            // Markus 10.06.2020: Only when a value is given where the source function can relate to
                            if (KV.Value.TypeOfSource == DataColumn.SourceType.File)
                            {
                                if (KV.Value.TransformedValue() != null
                                && KV.Value.TransformedValue().Length > 0)
                                {
                                    insert = true;
                                    break;
                                }
                            }
                            else
                            {
                                // TODO: unklar was bei TypeOfSource != DataColumn.SourceType.File passieren soll
                                insert = true;
                                break;
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            // Toni: Additional check for column value if check insert delegate exists   
            if (insert)
            {
                try
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                    {
                        if (KV.Value.TransformedValue() != null && KV.Value.TransformedValue().Length > 0)
                        {
                            if (!KV.Value.InsertPossible())
                                return false;
                        }
                    }
                    return true;
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            // Markus: If empty import is allowed
            if (this.AllowEmptyImport)
            { return true; }
            return false;
        }

        private bool GetCurrentDataFromDatabase()
        {
            try
            {
                if (this.PKisComplete)
                {
                    string SQL = "";
                    foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                    {
                        if (!KV.Value.IsSelected)
                            continue;
                        if (SQL.Length > 0) SQL += ", ";
                        SQL += KV.Key;
                    }
                    if (SQL.Length == 0)
                        return true;
                    SQL = "SELECT " + SQL + " FROM [" + this._TableName + "] WHERE 1 = 1 ";
                    foreach (string s in this.PrimaryKeyColumnList)
                    {
                        // 7.4.2015 MW: Get value generated by function if defined
                        if (this.DataColumns[s].PrepareInsertDefined)
                        {
                            if (this.DataColumns[s].ProcessedValue != null)
                                SQL += " AND " + s + " = '" + this.DataColumns[s].ProcessedValue + "'";
                            else
                                SQL += " AND " + s + " = '" + this.DataColumns[s].TransformedValue() + "'";
                        }
                        else if (this._PresetValues.ContainsKey(s))
                            SQL += " AND " + s + " = '" + this._PresetValues[s] + "'";
                        else
                            SQL += " AND " + s + " = '" + this.DataColumns[s].Value + "'";
                    }
                    System.Data.DataTable dt = new System.Data.DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        foreach (System.Data.DataColumn C in dt.Columns)
                        {
                            // Toni 20190725: Convert floating point data by using xmlconvert to avoid NLS-dependent decimal separator!
                            if (dt.Rows[0][C.ColumnName] is double || dt.Rows[0][C.ColumnName] is Single)
                                this.DataColumns[C.ColumnName].ValueInDatabase = System.Xml.XmlConvert.ToString((double)dt.Rows[0][C.ColumnName]);
                            else
                                this.DataColumns[C.ColumnName].ValueInDatabase = dt.Rows[0][C.ColumnName].ToString();
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return true;
        }

        #endregion

        #region Writing data into the database

        /// <summary>
        /// inserting the data of the current row
        /// </summary>
        /// <param name="ImportConnection">The connection to the database used for the insert</param>
        /// <param name="ImportTransaction">The transaction in which the insert is included</param>
        /// <param name="Message">The message containig possible errors as returned after the insert</param>
        /// <returns>the error message if the insert was not successful</returns>
        private bool InsertData(Microsoft.Data.SqlClient.SqlConnection ImportConnection, Microsoft.Data.SqlClient.SqlTransaction ImportTransaction, ref string Message)
        {
            try
            {
                string SQL = "";

                string SqlColumns = "";
                string SqlValues = "";
                bool DoImport = false;
                // Toni: Check if insert is required before further processing
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.IsSelected && KV.Value.IsDecisive && KV.Value.TransformedValue().Length > 0)
                    {
                        DoImport = true;
                        break;
                    }
                }

                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.IsSelected && /*KV.Value.Value != null && KV.Value.Value.Length > 0 &&*/ KV.Value.TransformedValue().Length > 0)
                    {
                        if (SqlColumns.Length > 0) SqlColumns += ", ";
                        SqlColumns += KV.Key;
                        if (SqlValues.Length > 0) SqlValues += ", ";
                        if (DiversityWorkbench.Import.Import.AttachmentColumn != null &&
                            this.TableAlias == DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias &&
                            this.AttachViaParentChildRelation() &&
                            this.AttachmentParentColumn() == KV.Value.ColumnName)
                        {
                            SqlValues += "(SELECT " + KV.Value.ForeignRelationColumn + " FROM [" + KV.Value.ForeignRelationTable + "] WHERE " + DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName + " = '" + KV.Value.ValueAttachmentParent + "' )";
                        }
                        else
                        {
                            // Toni: Call prepare insert delegate if import is required
                            if (DoImport)
                            {
                                KV.Value.PrepareInsert(ImportConnection, ImportTransaction, ref Message);
                                if (Message != "")
                                {
                                    DoImport = false;
                                    break;
                                }
                            }
                            // Markus 2015-12-08: Transformed value or parent colum needed here
                            if (KV.Value.TypeOfSource == DataColumn.SourceType.ParentTable &&
                                KV.Value.ForeignRelationTableAlias != null &&
                                KV.Value.ForeignRelationColumn != null &&
                                DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Transformations.Count > 0)
                            {
                                string x = "";
                                // Toni 2018-12-18: If insert preprocessing is defined for parent, read the processed value
                                if (DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].PrepareInsertDefined)
                                    x = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value;
                                if (x == null || x.Length == 0)
                                    x = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].TransformedValue();
                                if (x.Length > 0)
                                {
                                    if (KV.Value.DataType.IndexOf("char") > -1)
                                        x = "'" + x + "'";
                                    SqlValues += x;
                                }
                                //string Test = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value;
                            }
                            else
                            {
                                SqlValues += KV.Value.ValueFormatedForSQL();
                            }
                        }
                        //if (KV.Value.IsDecisive)
                        //    DoImport = true;
                    }
                    else if (KV.Value.IsSelected && KV.Value.PrepareInsertDefined && KV.Value.DataRetrievalType != DataColumn.RetrievalType.Default)
                    {
                        KV.Value.PrepareInsert(ImportConnection, ImportTransaction, ref Message);
                        if (SqlColumns.Length > 0) SqlColumns += ", ";
                        SqlColumns += KV.Key;
                        if (SqlValues.Length > 0) SqlValues += ", ";
                        SqlValues += KV.Value.ValueFormatedForSQL();
                        DoImport = true;
                    }
                    else if (KV.Value.IsSelected &&
                        KV.Value.ForeignRelationColumn != null && KV.Value.ForeignRelationColumn.Length > 0 &&
                        KV.Value.ForeignRelationTableAlias != null && KV.Value.ForeignRelationTableAlias.Length > 0)
                    {
                        if (DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value != null
                            && DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value.Length > 0)
                        {
                            KV.Value.TypeOfSource = DataColumn.SourceType.ParentTable;
                            if (SqlColumns.Length > 0) SqlColumns += ", ";
                            SqlColumns += KV.Key;

                            // Toni 7.12.2015: Wenn ein prozessierter Wert vorliegt dann ist dieser zu nehmen!
                            if (DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].ProcessedValue != null &&
                                DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].ProcessedValue.Length > 0)
                                KV.Value.Value = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].ProcessedValue;
                            // Markus 18.9.2015: hier wurde der nicht transformierte Wert in die abhaengige Tabelle geschrieben
                            else if (DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Transformations.Count > 0)
                                KV.Value.Value = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].TransformedValue();
                            else
                                KV.Value.Value = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value;

                            if (SqlValues.Length > 0) SqlValues += ", ";
                            SqlValues += KV.Value.ValueFormatedForSQL();
                        }
                    }
                    else if (this.AllowEmptyImport && this.PrimaryKeyColumnList.Contains(KV.Key))
                    {
                        if (KV.Value.Value == null &&
                            KV.Value.ForeignRelationColumn != null && KV.Value.ForeignRelationColumn.Length > 0 &&
                            KV.Value.ForeignRelationTableAlias != null && KV.Value.ForeignRelationTableAlias.Length > 0)
                        {
                            KV.Value.Value = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value;
                            KV.Value.TypeOfSource = DataColumn.SourceType.ParentTable;
                        }
                        // 27.11.2015 Toni: Ignore identity column
                        if (!KV.Value.IsIdentity)
                        {
                            if (SqlColumns.Length > 0) SqlColumns += ", ";
                            SqlColumns += KV.Key;
                            if (SqlValues.Length > 0) SqlValues += ", ";
                            SqlValues += KV.Value.ValueFormatedForSQL();
                        }
                        DoImport = true;
                    }
                    //MW 13.11.2015 - Columns from parent table where missing
                    else if (this.PrimaryKeyColumnList.Contains(KV.Key) && (KV.Value.Value == null || KV.Value.Value.Length == 0) && !KV.Value.IsIdentity && !this.AllowEmptyImport)
                    {
                        if ((KV.Value.Value == null || KV.Value.Value.Length == 0) &&
                            KV.Value.ForeignRelationColumn != null && KV.Value.ForeignRelationColumn.Length > 0 &&
                            KV.Value.ForeignRelationTableAlias != null && KV.Value.ForeignRelationTableAlias.Length > 0)
                        {
                            // Markus 15.05.2016: Test if value is null
                            if (DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value != null)
                            {
                                KV.Value.Value = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value;
                                KV.Value.TypeOfSource = DataColumn.SourceType.ParentTable;
                            }
                        }
                        if (KV.Value.Value != null)
                        {
                            if (KV.Value.ForeignRelationTableAlias == null && KV.Value.ColumnDefault != null && KV.Value.ForeignRelationTable != null && KV.Value.ForeignRelationColumn != null)
                            {
                                // Markus 13.6.2016: Key column with optional relation to other table replaced by a default
                            }
                            else if (KV.Value.ForeignRelationTableAlias != null
                                && DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias]._NeededAction == NeededAction.NoData)
                            {
                                DoImport = false;
                                this._NeededAction = NeededAction.NoData;
                            }
                            else
                            {
                                // Markus 26.05.2017 - Tried to import emtpy value into a PK with numeric value
                                if (KV.Value.ColumnDefault != null &&
                                    KV.Value.ColumnDefault.Length > 0 &&
                                    KV.Value.DataType.IndexOf("int") > -1 &&
                                    KV.Value.Value.Length == 0)
                                    DoImport = true;
                                else
                                {
                                    if (SqlColumns.Length > 0) SqlColumns += ", ";
                                    SqlColumns += KV.Key;
                                    if (SqlValues.Length > 0) SqlValues += ", ";
                                    SqlValues += KV.Value.ValueFormatedForSQL();
                                    DoImport = true;
                                }
                            }
                        }
                    }
                }
                if (DoImport)
                {
                    SQL = "INSERT INTO [" + this._TableName + "] (" + SqlColumns + ") VALUES (" + SqlValues + ")";
                    if (DiversityWorkbench.Import.Import.TranslateReturn)
                    {
                        SqlValues = SqlValues.Replace("\\r\\n", "' + CHAR(13) + CHAR(10) + N'");
                        SQL = "INSERT INTO [" + this._TableName + "] (" + SqlColumns + ") VALUES (" + SqlValues + ")";
                    }
                    if (IdentityColumn.Length > 0)
                    {
                        SQL = "SELECT MAX(" + IdentityColumn + ") + 1 FROM [" + this._TableName + "]";
                        int MaxID = 0;
                        int.TryParse(this.SqlExecuteScalar(SQL, ImportConnection, ImportTransaction, ref Message), out MaxID);
                        int Identity = 0;
                        /// Markus 3.12.2019 - Umgestellt auf Output
                        //SQL = "DECLARE @i int " + SQL + " SET @i = (SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY]) SELECT @i";
                        SQL = "DECLARE @T Table(ID int); INSERT INTO [" + this._TableName + "] (" + SqlColumns + ") OUTPUT INSERTED." + IdentityColumn + " INTO @T VALUES (" + SqlValues + "); SELECT TOP 1 ID FROM @T"; // Using OUTPUT Clause
                        if (int.TryParse(this.SqlExecuteScalar(SQL, ImportConnection, ImportTransaction, ref Message), out Identity))
                        {
                            //// obiges statement liefert mitunter Werte wie 1 oder 2 - evtl. wegen Insert-Trigger. Um dies abzufangen erfolgt der Vergleich unten
                            bool EmptyTable = false;
                            // Markus 10.2.2020 - falls Tabelle leer ist
                            string SqlCheckEmpty = "SELECT COUNT(*) FROM [" + this._TableName + "]";
                            int Count;
                            string M = "";
                            if (int.TryParse(this.SqlExecuteScalar(SqlCheckEmpty, ImportConnection, ImportTransaction, ref M), out Count))
                            {
                                if (Count <= 1) // one dataset means the table has been empty before insert of the current row
                                    EmptyTable = true;
                            }
                            if (Identity < MaxID || EmptyTable)
                            {
                                SQL = "SELECT SCOPE_IDENTITY()";
                                int ID = 0;
                                int.TryParse(this.SqlExecuteScalar(SQL, ImportConnection, ImportTransaction, ref Message), out ID);
                                if (ID >= MaxID)
                                    Identity = ID;
                            }
                            this.DataColumns[IdentityColumn].Value = Identity.ToString();
                            //SQL = "SELECT @@IDENTITY";
                            ////SQL = "SELECT IDENT_CURRENT( '" + this._TableName + "' )"; //IDENT_CURRENT ist nicht durch einen Gültigkeitsbereich oder eine Sitzung begrenzt, sondern auf eine angegebene Tabelle
                            //int ID_Current;
                            //if (int.TryParse(this.SqlExecuteScalar(SQL, ImportConnection, ImportTransaction, ref Message), out ID_Current))
                            //{
                            //    if (Identity == ID_Current)
                            //        this.DataColumns[IdentityColumn].Value = ID_Current.ToString();
                            //    //else if (ID_Current > Identity)
                            //    //{
                            //    //    int Identity 
                            //    //    this.DataColumns[IdentityColumn].Value = ID_Current.ToString();
                            //    //}
                            //    else
                            //        this.AddMessage("The retrieval of the identity failed: Identity via OUTPUT = " + Identity.ToString() + "; Identity via @@Identity = " + ID_Current.ToString());
                            //    //this.DataColumns[IdentityColumn].Value = this.SqlExecuteScalar(SQL, ImportConnection, ImportTransaction, ref Message);
                            //}
                        }
                    }
                    else
                    {
                        this.SqlExecuteNonQuery(SQL, ImportConnection, ImportTransaction, ref Message);
                        bool ContainsColumnWithRetrievalFromFunction = false;
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                        {
                            if (KV.Value.DataRetrievalType == DataColumn.RetrievalType.FunctionInDatabase)
                            {
                                ContainsColumnWithRetrievalFromFunction = true;
                                break;
                            }
                        }
                        if (ContainsColumnWithRetrievalFromFunction)
                        {
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                            {
                                if (KV.Value.DataRetrievalType == DataColumn.RetrievalType.FunctionInDatabase)
                                {
                                    KV.Value.Value = KV.Value.Value;
                                }
                            }
                        }
                        if (Message.Length > 0)
                        {
                            bool ReasonMayBeTimeStampDefault = false;
                            string Datatype = "";
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTableIndex> KV in this.IndexList)
                            {
                                if (KV.Value.IsUnique)
                                {
                                    foreach (string C in KV.Value.Columns)
                                    {
                                        if (this.DataColumns[C].ColumnDefault != null
                                            && this.DataColumns[C].DataType.StartsWith("date")
                                            && this.DataColumns[C].FileColumn == null
                                            && Message.IndexOf(KV.Key) > -1)
                                        {
                                            ReasonMayBeTimeStampDefault = true;
                                            Datatype = this.DataColumns[C].DataType;
                                            break;
                                        }
                                    }
                                }
                                if (ReasonMayBeTimeStampDefault)
                                    break;
                            }
                            if (ReasonMayBeTimeStampDefault)
                            {
                                Message = "";
                                switch (Datatype)
                                {
                                    case "datetime":
                                        System.Threading.Thread.Sleep(1500);
                                        break;
                                    case "datetime2":
                                        System.Threading.Thread.Sleep(15);
                                        break;
                                    default:
                                        break;
                                }
                                this.SqlExecuteNonQuery(SQL, ImportConnection, ImportTransaction, ref Message);
                            }
                            /*
                             * "Eine Zeile mit doppeltem Schlüssel kann in das dbo.CollectionAgent-Objekt mit dem eindeutigen IX_CollectionAgentSequence-Index nicht eingefügt werden. Der doppelte Schlüsselwert ist (526267, Jun 10 2014  2:43PM).\r\nDie Anweisung wurde beendet."
                             * */
                        }
                    }
                }
                if (RecordSQL.Record)
                {
                    RecordSQL.AddSQLStatement(SQL, Message);
                }
                if (Message.Length > 0)
                {
                    this.AddMessage(Message);
                    return false;
                }
                else return true;
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
                this.AddMessage(Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Update the data in the database
        /// </summary>
        /// <param name="ImportConnection">The connection to the database used for the update</param>
        /// <param name="ImportTransaction">The transaction in which the update is included</param>
        /// <param name="Message">The message containig possible errors as returned after the update</param>
        private void UpdateData(Microsoft.Data.SqlClient.SqlConnection ImportConnection, Microsoft.Data.SqlClient.SqlTransaction ImportTransaction, ref string Message)
        {
            try
            {
                if (!this.GetCurrentDataFromDatabase())
                {
                    Message = "Failed to retrieve current data from database";
                    return;
                }
                string SQL = "";
                string SqlColumnValues = "";
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    try
                    {
                        if (KV.Key == this.IdentityColumn)
                            continue;
                        // Markus: if value is taken from a function
                        if (KV.Value.DataRetrievalType == DataColumn.RetrievalType.FunctionInDatabase)
                        { }//continue;

                        // Toni: Perform prepare update processing
                        KV.Value.PrepareUpdate(ImportConnection, ImportTransaction, ref Message);
                        if (Message != "")
                            this.AddMessage(Message);
                        if (KV.Value.Value == null || (KV.Value.Value != null && KV.Value.Value.Length == 0))
                        {
                            // Markus 24.8.2015: If value may be recieved from a related table
                            if (KV.Value.SelectParallelForeignRelationTableAlias
                                && KV.Value.ForeignRelationColumn.Length > 0
                                && KV.Value.ForeignRelationTableAlias != null
                                && KV.Value.ForeignRelationTableAlias.Length > 0
                                && DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value != null
                                && this._MergeHandling == Merging.Update)
                                KV.Value.Value = DiversityWorkbench.Import.Import.Tables[KV.Value.ForeignRelationTableAlias].DataColumns[KV.Value.ForeignRelationColumn].Value;
                            else
                                continue;
                        }
                        if (this.PrimaryKeyColumnList.Contains(KV.Key))
                            continue;
                        if (!KV.Value.IsSelected)
                            continue;
                        // Markus: in case of a multicolumn the contents of all columns must be compared with the content in the database
                        if (!KV.Value.IsMultiColumn && KV.Value.Value == KV.Value.ValueInDatabase)
                            continue;
                        if (KV.Value.IsMultiColumn)
                        {
                            string Val = KV.Value.TransformedValue();
                            if (Val == KV.Value.ValueInDatabase)
                                continue;
                        }
                        if (SqlColumnValues.Length > 0)
                            SqlColumnValues += ", ";
                        // Markus: in case of Transformations, use Transformed value
                        //if (false && KV.Value.Transformations.Count > 0)
                        //{
                        //    //SqlColumnValues += KV.Key + " = " + DiversityWorkbench.Import.DataColumn.TransformedValue(KV.Value, KV.Value.Transformations,;
                        //}
                        //else
                        SqlColumnValues += KV.Key + " = " + KV.Value.ValueFormatedForSQL(); // Toni: use formatted value instead original
                    }
                    catch (System.Exception ex)
                    {
                    }
                }
                if (SqlColumnValues.Length == 0)
                {
                    this._NeededAction = NeededAction.NoDifferences;
                    return;
                }

                SQL = "UPDATE " + this._TableName + " SET " + SqlColumnValues + " WHERE 1 = 1 ";
                foreach (string s in this.PrimaryKeyColumnList)
                {
                    if (this.MergeHandling == Merging.Update)
                    {
                        if (this.DataColumns[s].Value != null && this.DataColumns[s].Value.Length > 0)
                            SQL += " AND " + s + " = '" + this.DataColumns[s].Value + "'";
                        else if (this.DataColumns[s].Value.Length == 0
                            && this.DataColumns[s].ValueIsPreset
                            && this.DataColumns[s].TypeOfSource == DataColumn.SourceType.Preset
                            && this.PresetValues.ContainsKey(s)
                            && this.PresetValues[s].Length > 0)
                        {
                            SQL += " AND " + s + " = '" + this.PresetValues[s] + "'";
                        }
                    }
                    else if (this.MergeHandling == Merging.Merge)
                    {
                        if (this.DataColumns[s].Value.Length == 0
                            && this.DataColumns[s].ValueIsPreset
                            && this.DataColumns[s].TypeOfSource == DataColumn.SourceType.Preset
                            && this.PresetValues.ContainsKey(s)
                            && this.PresetValues[s].Length > 0)
                        {
                            SQL += " AND " + s + " = '" + this.PresetValues[s] + "'";
                        }
                        else
                            SQL += " AND " + s + " = '" + this.DataColumns[s].Value + "'";
                    }
                }
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataColumn> KV in this.DataColumns)
                {
                    if (KV.Value.CompareKey)
                    {
                        if (KV.Value.ValueInDatabase == null)
                            SQL += " AND " + KV.Key + " IS NULL ";
                        else
                        {
                            if (KV.Value.ValueInDatabase.Length == 0)
                            {
                                SQL += " AND (" + KV.Key + " IS NULL OR " + KV.Key + " = '')";
                            }
                            else
                            {
                                if (KV.Value.DataType.ToLower() == "geography")
                                    SQL += " AND " + KV.Key + ".ToString() = '" + KV.Value.ValueInDatabase + "'";
                                else
                                    SQL += " AND " + KV.Key + " = '" + KV.Value.ValueInDatabase + "'";
                            }
                        }
                    }
                }
                try
                {
                    this.SqlExecuteNonQuery(SQL, ImportConnection, ImportTransaction, ref Message);
                    if (Message.Length > 0)
                        this.AddMessage("Update failed: " + Message);
                    if (RecordSQL.Record)
                    {
                        RecordSQL.AddSQLStatement(SQL, Message);
                    }
                }
                catch (System.Exception ex)
                {
                    Message = ex.Message;
                }
            }
            catch (System.Exception ex) { }
        }

        /// <summary>
        /// Reading the data according to the attachment column
        /// </summary>
        /// <param name="ImportConnection">The connection to the database used for the query</param>
        /// <param name="ImportTransaction">The transaction in which the update is query</param>
        /// <param name="Message">The message containig possible errors as returned after the query</param>
        private void ReadData(Microsoft.Data.SqlClient.SqlConnection ImportConnection, Microsoft.Data.SqlClient.SqlTransaction ImportTransaction, ref string Message)
        {
            try
            {
                if (!this.GetCurrentDataFromDatabase())
                {
                    Message = "Failed to retrieve current data from database";
                    return;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private string SqlExecuteScalar(string SqlCommand, Microsoft.Data.SqlClient.SqlConnection ImportConnection, Microsoft.Data.SqlClient.SqlTransaction ImportTransaction, ref string Message)
        {
            string Result = "";
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, ImportConnection);
            try
            {
                if (ImportConnection.State == System.Data.ConnectionState.Closed)
                    ImportConnection.Open();
                C.Transaction = ImportTransaction;
                Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
            }
            catch (System.Exception ex) { Message = ex.Message; }
            finally
            {
            }
            return Result;
        }

        private void SqlExecuteNonQuery(string SqlCommand, Microsoft.Data.SqlClient.SqlConnection ImportConnection, Microsoft.Data.SqlClient.SqlTransaction ImportTransaction, ref string Message)
        {
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SqlCommand, ImportConnection);
            try
            {
                if (ImportConnection.State == System.Data.ConnectionState.Closed)
                    ImportConnection.Open();
                C.Transaction = ImportTransaction;
                C.ExecuteNonQuery();
            }
            catch (System.Exception ex)
            { Message = ex.Message + ": " + SqlCommand; }
            finally
            {
            }
            return;
        }


        #endregion

        #endregion

    }
}
