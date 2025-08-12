using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Import
{
    /// <summary>
    /// If a Column in the database corresponds to several columns in the file
    /// </summary>
    public class ColumnMulti : DiversityWorkbench.Import.iDataColumn
    {

        #region Interface

        public void setCopyPrevious(bool Copy)
        {
            this._CopyPrevious = Copy;
        }

        public void setIsDecisive(bool IsDecisive)
        {
            this.IsDecisive = IsDecisive;
        }

        public void setColumnInFile(int ColumnInFile)
        {
            this.ColumnInFile = ColumnInFile;
        }

        public void setPrefix(string Prefix)
        {
            this.Prefix = Prefix;
        }

        public void setPostfix(string Postfix)
        {
            this.Postfix = Postfix;
        }

        public System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> TransformationList()
        {
            return this.Transformations;
        }

        public System.Data.DataTable getMandatoryList()
        {
            return this.DataColumn.MandatoryList;
        }

        public string getMandatoryListDisplayColumn()
        {
            return this.DataColumn.MandatoryListDisplayColumn;
        }

        public string getMandatoryListValueColumn()
        {
            return this.DataColumn.MandatoryListValueColumn;
        }

        public int? PositionInFile() { return (int?)this._ColumnInFile; }

        #endregion        
        


        private DiversityWorkbench.Import.DataColumn _DataColumn;

        public DiversityWorkbench.Import.DataColumn DataColumn
        {
            get { return _DataColumn; }
            //set { _DataColumn = value; }
        }

        private DiversityWorkbench.Import.iDataColumnInterface _iDataColumn;

        //private int _Sequence = 2;
        //private System.Collections.Generic.SortedDictionary<int, DiversityWorkbench.Import.Transformation> _Transformations;
        
        private System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> _Transformations;

        public System.Collections.Generic.List<DiversityWorkbench.Import.Transformation> Transformations
        {
            get { if (this._Transformations == null) this._Transformations = new List<Transformation>(); return _Transformations; }
            set { _Transformations = value; }
        }

        private string _Prefix;

        public string Prefix
        {
            get
            {
                if (this._Prefix == null)
                    this._Prefix = "";
                return _Prefix;
            }
            set { _Prefix = value; }
        }

        private string _Postfix;

        public string Postfix
        {
            get
            {
                if (this._Postfix ==  null)
                    this._Postfix = "";
                return _Postfix;
            }
            set { _Postfix = value; }
        }

        public string TransformedValue()
        {
            string Result = DiversityWorkbench.Import.Import.LineValuesFromFile()[this._ColumnInFile];
            if (this._CopyPrevious)
            {
                if (Result.Length > 0)
                    this.PreviousValue = Result;
                Result = this.PreviousValue;
            }
            if (this.Transformations.Count > 0)
                Result = DiversityWorkbench.Import.DataColumn.TransformedValue(Result, this.Transformations, DiversityWorkbench.Import.Import.LineValuesFromFile());
            if (Result.Length > 0)
                Result = this._Prefix + Result + this.Postfix;
            return Result;
        }

        //public string TransformedValue(string Value)
        //{
        //    string Result = DiversityWorkbench.Import.DataColumn.TransformedValue(Value, this.Transformations);
        //    if (this.Prefix.Length > 0)
        //        Result = this.Prefix + Result;
        //    return Result;
        //}

        private int _ColumnInFile;

        public int ColumnInFile
        {
            get { return _ColumnInFile; }
            set { _ColumnInFile = value; }
        }

        //private string _Value;

        //public string Value
        //{
        //    get { return _Value; }
        //    set { _Value = value; }
        //}

        private string _PreviousValue;

        public string PreviousValue
        {
            get { return _PreviousValue; }
            set { _PreviousValue = value; }
        }

        private bool _CopyPrevious;

        public bool CopyPrevious
        {
            get { return _CopyPrevious; }
            set { _CopyPrevious = value; }
        }

        private bool _IsDecisive = false;
        /// <summary>
        /// if in the file any column of this type contains values, the data will be imported resp. updated
        /// </summary>
        public bool IsDecisive
        {
            get { if (this._IsDecisive == null) this._IsDecisive = false; return _IsDecisive; }
            set { _IsDecisive = value; }
        }

        //public ColumnMulti(DiversityWorkbench.Import.DataColumn DataColumn, int Sequence, int ColumnInFile)
        //{
        //    this._DataColumn = DataColumn;
        //    this._Sequence = Sequence;
        //    this._ColumnInFile = ColumnInFile;
        //}

        public ColumnMulti(DiversityWorkbench.Import.DataColumn DataColumn, DiversityWorkbench.Import.iDataColumnInterface iDataColumn, int ColumnInFile)
        {
            this._DataColumn = DataColumn;
            DiversityWorkbench.Import.Import.Tables[this.DataColumn.DataTable.TableAlias].DataColumns[this.DataColumn.ColumnName].MultiColumns.Add(this);
            //this._Sequence = Sequence;
            this._ColumnInFile = ColumnInFile;
        }

    }
}
