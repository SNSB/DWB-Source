using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Maintenance
{
    public class ReferencesTransfer
    {
        #region Parameter
        //private System.Data.DataTable _dtProject;
        private int? _ProjectID = null;
        public enum Target { Specimen, Identification }
        public enum Match { Existence, Title, URI, AllColumns }

        private Target _Target;
        private Match _Match = Match.Title;
        #endregion

        #region Construction

        public ReferencesTransfer(Target target) { _Target = target; }

        #endregion

        #region Interface

        public void initProjectCombobox(System.Windows.Forms.ComboBox comboBox)
        {
            comboBox.DataSource = dtProject;
            comboBox.DisplayMember = "Project";
            comboBox.ValueMember = "ProjectID";
        }

        public void SetProjectID(int? ID) { this._ProjectID = ID; _dtSearch = null; }

        public void setDatagridSource(System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                dataGridView.DataSource = this.SearchData();
                dataGridView.Columns[0].Visible = false;
                dataGridView.AutoResizeColumns(System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public int ResultCount { get { if (_dtSearch != null) return _dtSearch.Rows.Count; else return 0; } }

        public string TransferData(System.Windows.Forms.DataGridView dataGridView)
        {
            string Message = "";
            int i = ResultCount;
            if (i == 0) Message = "Nothing to transfer";
            else
            {
                if (this.Transfer(dataGridView)) Message = i.ToString() + " datasets transferred from table " + this.TargetTable + " into table CollectionSpecimenReference";
            }
            return Message;
        }

        public DiversityCollection.Forms.FormCollectionSpecimen ShowDetails(int ID)
        {
            DiversityCollection.Forms.FormCollectionSpecimen f = new Forms.FormCollectionSpecimen(ID, false, false, Forms.FormCollectionSpecimen.ViewMode.SingleInspectionMode, true);
            return f;
        }

        public void setMatch(Match match) { this._Match = match; this._dtSearch = null; }

        #endregion

        #region Data handling
        private System.Data.DataTable dtProject
        {
            get
            {
                string sql = "SELECT NULL AS ProjectID , NULL AS Project " +
                    "UNION " +
                    "SELECT ProjectID, Project " +
                    "FROM [ProjectList] ORDER BY Project";
                System.Data.DataTable dataTable = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(sql, ref dataTable);
                return dataTable;
            }
        }


        private System.Data.DataTable _dtSearch;

        private System.Data.DataTable SearchData()
        {
            if (_dtSearch == null)
            {
                string SQL = SqlSearch();
                _dtSearch = new System.Data.DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _dtSearch);
            }
            return this._dtSearch;
        }

        private string SqlSearch(bool ForInsert = false)
        {
            string SQL = "SELECT T.CollectionSpecimenID, ";
            if (!ForInsert)
            {
                if (_Target == Target.Identification)
                    SQL += "case when T.TaxonomicName <> '' then T.TaxonomicName else T.VernacularTerm end as Taxon, ";
                else
                    SQL += "T.AccessionNumber, ";
            }
            SQL += "T.ReferenceTitle, T.ReferenceURI, T.ReferenceDetails ";
            if (_Target == Target.Identification)
                SQL += ", T.IdentificationUnitID, T.IdentificationSequence, T.ResponsibleName, T.ResponsibleAgentURI ";
            SQL += "FROM " + TargetTable + " AS T ";
            if (_ProjectID != null)
                SQL += " INNER JOIN CollectionProject AS P ON T.CollectionSpecimenID = P.CollectionSpecimenID ";
            SQL += "LEFT OUTER JOIN CollectionSpecimenReference AS R ON T.CollectionSpecimenID = R.CollectionSpecimenID ";
            switch(_Match)
            {
                case Match.Existence:
                    break;
                case Match.Title:
                    SQL += " AND T.ReferenceTitle = R.ReferenceTitle ";
                    break;
                case Match.URI:
                    SQL += " T.ReferenceTitle = R.ReferenceTitle AND T.ReferenceURI = R.ReferenceURI AND (T.ReferenceURI <> N'') ";
                    break;
                case Match.AllColumns:
                    SQL += " AND T.ReferenceTitle = R.ReferenceTitle AND T.ReferenceURI = R.ReferenceURI AND T.ReferenceDetails = R.ReferenceDetails ";
                    break;
            }
            switch(_Target)
            {
                case Target.Identification:
                    SQL += " AND T.IdentificationUnitID = R.IdentificationUnitID AND T.IdentificationSequence = R.IdentificationSequence AND R.SpecimenPartID IS NULL ";
                    break;
                case Target.Specimen:
                    SQL += " AND R.IdentificationUnitID IS NULL AND R.IdentificationSequence IS NULL AND R.SpecimenPartID IS NULL ";
                    break;
            }
            SQL += "WHERE (R.CollectionSpecimenID IS NULL) AND (T.ReferenceTitle <> N'') ";
            if (_ProjectID != null)
                SQL += " AND P.ProjectID = " + _ProjectID.ToString();
            return SQL;
        }

        private string SqlInsert()
        {
            string SQL = "INSERT INTO CollectionSpecimenReference " +
                "(CollectionSpecimenID, ReferenceTitle, ReferenceURI, ReferenceDetails";
            if (_Target == Target.Identification)
                SQL += ", IdentificationUnitID, IdentificationSequence, ResponsibleName, ResponsibleAgentURI";
            SQL += ")";
            return SQL;
        }

        private string TargetTable
        {
            get
            {
                switch(_Target)
                {
                    case Target.Identification:
                        return "Identification";
                    default:
                        return "CollectionSpecimen";
                }
            }
        }

        private bool Transfer(System.Windows.Forms.DataGridView dataGridView)
        {
            bool OK = true;
            try
            {
                string SQL = SqlInsert() + " " + SqlSearch(true);
                OK = DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL, true);
                if (OK)
                {
                    this._dtSearch = null;
                    dataGridView.DataSource = this.SearchData();
                }
            }
            catch(System.Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        #endregion

    }
}
