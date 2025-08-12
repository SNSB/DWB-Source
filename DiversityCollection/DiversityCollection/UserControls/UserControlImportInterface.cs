using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiversityCollection.UserControls
{
    public interface iUserControlImportInterface
    {
        //void initUserControl(DiversityCollection.FormImportWizard f, string TableAlias, string StepKey, DiversityCollection.Import Import);
        //void initUserControl(DiversityCollection.iImportInterface I, string TableAlias, DiversityCollection.Import_Step SuperiorImportStep, DiversityCollection.Import Import);
        //void initUserControl(DiversityCollection.iImportInterface I, System.Windows.Forms.Control ParentControl, DiversityCollection.Import_Step SuperiorImportStep);
        void initUserControl(DiversityCollection.iImportInterface I, DiversityCollection.Import_Step SuperiorImportStep);
        void showStepControls(DiversityCollection.Import_Step ImportStep);
        //void AddImportStep(DiversityCollection.Import_Step ImportStep);
        //System.Collections.Generic.Dictionary<string, DiversityCollection.Import_Step> getImportSteps();
        void AddImportStep();
        void AddImportStep(string StepKey);
        void HideImportStep();
        void ShowHiddenImportSteps();
        System.Windows.Forms.Panel SelectionPanelForDependentSteps();
        void UpdateSelectionPanel();
        string StepKey();
        void Reset();
    }
}
