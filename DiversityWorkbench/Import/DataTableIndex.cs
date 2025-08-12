using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Import
{
    public class DataTableIndex
    {
        private string _IndexName;
        /// <summary>
        /// the name of the index
        /// </summary>
        public string IndexName
        {
            get { return _IndexName; }
            set { _IndexName = value; }
        }

        private bool _IsUnique;
        /// <summary>
        /// if the index is unique
        /// </summary>
        public bool IsUnique
        {
            get { return _IsUnique; }
            set { _IsUnique = value; }
        }

        private bool _IsPK;
        /// <summary>
        /// If the index is the primary key
        /// </summary>
        public bool IsPK
        {
            get { return _IsPK; }
            set { _IsPK = value; }
        }

        private System.Collections.Generic.List<string> _Columns;
        /// <summary>
        /// The list of columns included in the index
        /// </summary>
        public System.Collections.Generic.List<string> Columns
        {
            get { return _Columns; }
            set { _Columns = value; }
        }

    }
}
