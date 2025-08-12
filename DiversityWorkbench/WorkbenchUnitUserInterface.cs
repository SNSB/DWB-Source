using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityWorkbench
{
    public interface WorkbenchUnitUserInterface
    {
        void SetHeight(int Height);
        DiversityWorkbench.WorkbenchUnit.ModuleType GetModuleType();
    }
}
