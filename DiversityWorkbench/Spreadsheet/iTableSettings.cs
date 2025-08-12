using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public interface iTableSettings
    {
        void AddTable(string TableAlias);
        void RemoveTable(string TableAlias);
        void SetTabName(string TableAlias, string DisplayText);
    }
}
