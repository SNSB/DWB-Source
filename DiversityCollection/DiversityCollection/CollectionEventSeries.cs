using System;
using System.Collections.Generic;
using System.Text;

namespace DiversityCollection
{
    class CollectionEventSeries : HierarchicalEntity
    {
        #region Construction
        public CollectionEventSeries(
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
            HelpProvider, ToolTip, ref BindingSource, null, null)
        {
            this._sqlItemFieldList = " SeriesID, SeriesParentID, Description, SeriesCode, Notes, DateStart, DateEnd ";
            this._SpecimenTable = "CollectionSpecimen";
            this._MainTable = "CollectionEventSeries";
        }
        
        #endregion

        #region Functions and properties
        protected override string SqlSpecimenCount(int ID)
        {
            return "SELECT COUNT(*) FROM CollectionEventSeries INNER JOIN " +
                "CollectionEvent ON CollectionEventSeries.SeriesID = CollectionEvent.SeriesID INNER JOIN " +
                "CollectionSpecimen ON CollectionEvent.CollectionEventID = CollectionSpecimen.CollectionEventID " +
                "HAVING CollectionEventSeries.SeriesID = " + ID.ToString();
        }

        public override string ColumnDisplayText
        {
            get
            {
                return "Description";
            }
        }

        public override string ColumnDisplayOrder
        {
            get
            {
                return "Description";
            }
        }

        public override string ColumnID
        {
            get
            {
                return "SeriesID";
            }
        }

        public override string ColumnParentID
        {
            get
            {
                return "SeriesParentID";
            }
        }


        #endregion

        #region Interface

        public override System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions
        {
            get
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions = new List<DiversityWorkbench.QueryCondition>();

                string Description = this.FormFunctions.ColumnDescription("CollectionEventSeries", "Description");
                DiversityWorkbench.QueryCondition qDescription = new DiversityWorkbench.QueryCondition(true, "CollectionEventSeries", "SeriesID", "Description", "Series", "Description", "Description", Description);
                QueryConditions.Add(qDescription);

                Description = this.FormFunctions.ColumnDescription("CollectionEventSeries", "SeriesCode");
                DiversityWorkbench.QueryCondition qCode = new DiversityWorkbench.QueryCondition(true, "CollectionEventSeries", "SeriesID", "SeriesCode", "Series", "Code", "Code", Description);
                QueryConditions.Add(qCode);

                Description = this.FormFunctions.ColumnDescription("CollectionEventSeries", "Notes");
                DiversityWorkbench.QueryCondition qNotes = new DiversityWorkbench.QueryCondition(true, "CollectionEventSeries", "SeriesID", "Notes", "Series", "Notes", "Notes", Description);
                QueryConditions.Add(qNotes);

                return QueryConditions;
            }
        }

        public override DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns
        {
            get
            {
                DiversityWorkbench.UserControls.QueryDisplayColumn[] QueryDisplayColumns = new DiversityWorkbench.UserControls.QueryDisplayColumn[1];
                QueryDisplayColumns[0].DisplayText = "Description";
                QueryDisplayColumns[0].DisplayColumn = "Description";
                QueryDisplayColumns[0].OrderColumn = "Description";
                QueryDisplayColumns[0].IdentityColumn = "SeriesID";
                QueryDisplayColumns[0].TableName = "CollectionEventSeries";
                return QueryDisplayColumns;
            }
        }

        #endregion
    }
}
