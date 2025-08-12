using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench.Spreadsheet
{
    public interface iMainForm
    {
        void ShowInMainForm(int ID, bool ShowDialogPanel, bool ShowMenu);
        void Close();
        void Dispose();
    }
}
