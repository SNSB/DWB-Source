namespace DiversityCollection.Forms
{
    public struct GridModeQueryField
    {
        /// <summary>
        /// The name of the table in the database
        /// </summary>
        public string Table;
        /// <summary>
        /// the name of the table in the grid mode
        /// </summary>
        public string AliasForTable;
        /// <summary>
        ///  the name of the column in the database
        /// </summary>
        public string Column;
        /// <summary>
        /// the name of the column in the gridmode
        /// </summary>
        public string AliasForColumn;
        /// <summary>
        /// a restriction for selecting dataset from a table, e.g. LocalisationSystemID = 7
        /// </summary>
        public string Restriction;
        public string DatasourceTable;
        public bool IsVisible;
        public bool IsHidden;
        /// <summary>
        /// The column to delete in which the link to an external module is stored
        /// </summary>
        public string RemoveLinkColumn;
        public string Entity;
    }
}

