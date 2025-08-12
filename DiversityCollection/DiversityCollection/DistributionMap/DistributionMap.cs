using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DiversityCollection.DistributionMap
{
    public partial class DistributionMap : Component
    {
        public DistributionMap()
        {
            InitializeComponent();
        }

        public DistributionMap(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private static DiversityWorkbench.Spreadsheet.FormSpreadsheet _Spreadsheet;

        public static DiversityWorkbench.Spreadsheet.FormSpreadsheet Spreadsheet
        {
            get 
            {
                if (_Spreadsheet == null)
                {
                    DiversityWorkbench.Spreadsheet.Sheet Sheet = DiversityCollection.Spreadsheet.Target.TargetSheet(DiversityCollection.Spreadsheet.Target.SheetTarget.Organisms);
                    _Spreadsheet = new DiversityWorkbench.Spreadsheet.FormSpreadsheet(Sheet, "");
                    _Spreadsheet.IsStarter = false;
                    _Spreadsheet.IsForGettingFromWhereClause = true;
                }
                return _Spreadsheet; 
            }
            set { _Spreadsheet = value; }
        }

        public static string FromWhereClause()
        {
            string SQL = "";
            Spreadsheet.ShowDialog();
            System.Collections.Generic.List<string> IncludedTableAliases = new List<string>();
            IncludedTableAliases.Add(DiversityCollection.Spreadsheet.Target.TableAlias("CollectionEventLocalisation"));
            IncludedTableAliases.Add(DiversityCollection.Spreadsheet.Target.TableAlias("IdentificationUnit"));
            //string ResultTableAlias = DiversityCollection.Spreadsheet.Target.TableAlias("CollectionEventLocalisation");
            SQL = Spreadsheet.FromWhereClause(IncludedTableAliases);
            Spreadsheet.Hide();
            SQL += " AND NOT " + DiversityCollection.Spreadsheet.Target.TableAlias("CollectionEventLocalisation") + ".Geography IS NULL ";
            return SQL;
        }

    }
}
