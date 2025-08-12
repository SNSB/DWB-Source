using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityCollection.Tasks
{
    public class TaxonCategory
    {
        #region Parameter
        private static System.Data.DataTable _DtAnalysisCategory;
        private static System.Data.DataTable DtAnalysisCategory
        {
            get
            {
                if (_DtAnalysisCategory == null)
                {
                    string tntServer = global::DiversityCollection.Properties.Settings.Default.TNTServer;
                    string tntDB = global::DiversityCollection.Properties.Settings.Default.TNTTaxaVariaDB;
                    string tnt = "[" + tntServer + "].[" + tntDB + "]";
                    string SQL = "select A.AnalysisID AS ID, case when V.DisplayText is null then A.DisplayText else V.DisplayText end AS Analyse, P.DisplayText AS Gruppe, A.SortingID AS Sortierung, I.AnalysisID  AS PreviewImageID " +
                        "from " + tnt + ".dbo.TaxonNameListAnalysisCategory A " +
                        "inner join " + tnt + ".dbo.TaxonNameListAnalysisCategory P ON A.AnalysisParentID = P.AnalysisID and P.DisplayText in (N'Stage', N'Remains')  " +
                        "left outer join " + tnt + ".dbo.TaxonNameListAnalysisCategory I ON I.AnalysisParentID = A.AnalysisID and I.DisplayText = 'preview image' " +
                        "left outer join " + tnt + ".dbo.TaxonNameListAnalysisCategoryValue V ON A.AnalysisID = V.AnalysisID and V.[AnalysisValue] = '" + _LanguageCode + "' " +
                        "order by a.AnalysisParentID, a.SortingID";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref _DtAnalysisCategory);
                }
                return _DtAnalysisCategory;

            }
        }

        public enum Group { Stage, Remain }
        private Group _group;
        public Group group { get { return _group; } }
        private int _AnalysisID;
        public int AnalysisID { get { return _AnalysisID; } }
        private int? _PreviewImageAnalysisID;
        public int? PreviewImageAnalysisID { get { return _PreviewImageAnalysisID; } }

        private static System.Collections.Generic.Dictionary<int, TaxonCategory> _TaxonCategories;
        private static string _LanguageCode;

        private int _Sorting;
        public int Sorting { get { return _Sorting; } }

        private string _DisplayText;
        public string DisplayText { get { return _DisplayText; } }

        #endregion

        #region Interface
        public static System.Collections.Generic.Dictionary<int, TaxonCategory> TaxonCategories(string LanguageCode = "")
        {
            if (_TaxonCategories == null)
            {
                if (LanguageCode.Length == 0)
                    _LanguageCode = DiversityWorkbench.Settings.Language.Substring(0, 2).ToLower();
                else
                    _LanguageCode = LanguageCode.Substring(0, 1);
                _TaxonCategories = new Dictionary<int, TaxonCategory>();
                foreach(System.Data.DataRow R in DtAnalysisCategory.Rows)
                {
                    int ID;
                    if (int.TryParse(R["ID"].ToString(), out ID))
                    {
                        Group group = Group.Stage;
                        if (R["Gruppe"].ToString().ToLower() != "stage") group = Group.Remain;
                        int? Preview = null;
                        if (!R["PreviewImageID"].Equals(System.DBNull.Value))
                            Preview = int.Parse(R["PreviewImageID"].ToString());
                        TaxonCategory taxonCategory = new TaxonCategory(ID, R["Analyse"].ToString(), group, int.Parse(R["Sortierung"].ToString()), Preview);
                        _TaxonCategories.Add(ID, taxonCategory);
                    }
                }
            }
            return _TaxonCategories;
        }

        #endregion

        #region Construction

        public TaxonCategory(int ID, string DisplayText, Group group, int Sorting, int? IDpreview)
        {
            _AnalysisID = ID;
            _DisplayText = DisplayText;
            _group = group;
            _Sorting = Sorting;
            _PreviewImageAnalysisID = IDpreview;
        }

        #endregion
    }
}
