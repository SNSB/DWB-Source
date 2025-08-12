using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiversityWorkbench.Archive
{
    public interface iTableGUI
    {
        void setMaxRows(int Max);
        void setCurrentRow(int Row);
        void setCurrentRow();

        void setLogCount(int Found, int Inserted);
    }
}
