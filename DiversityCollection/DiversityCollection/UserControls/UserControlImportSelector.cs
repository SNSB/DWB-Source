using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControlImportSelector : UserControl
    {
        #region Parameter
       
        //private System.Collections.Generic.List<System.Windows.Forms.TabPage> _TabPages;
        private System.Collections.Generic.List<DiversityCollection.Import_Step> _ImportSteps;
        private readonly int _Indent = 10;

        #endregion

        #region Construction

        //public UserControlImportSelector()
        //{
        //    InitializeComponent();
        //}

        public UserControlImportSelector(DiversityCollection.Import_Step ImportStep, string Selector)
        {
            InitializeComponent();

            if (ImportStep.SuperiorImportStep != null
                && ImportStep.SuperiorImportStep.getUserControlImportInterface().SelectionPanelForDependentSteps() != null
                && ImportStep.SuperiorImportStep.getUserControlImportInterface().SelectionPanelForDependentSteps().Tag == null)
                ImportStep.SuperiorImportStep.getUserControlImportInterface().SelectionPanelForDependentSteps().Tag = ImportStep.Level;
            int IndentSetback = 0;
            if (ImportStep.SuperiorImportStep != null
                && ImportStep.SuperiorImportStep.getUserControlImportInterface() != null 
                && ImportStep.SuperiorImportStep.getUserControlImportInterface().SelectionPanelForDependentSteps() != null 
                && ImportStep.SuperiorImportStep.getUserControlImportInterface().SelectionPanelForDependentSteps().Tag != null)
                int.TryParse(ImportStep.SuperiorImportStep.getUserControlImportInterface().SelectionPanelForDependentSteps().Tag.ToString(), out IndentSetback);
            this.panelIndent.Width = 1 + (ImportStep.Level - IndentSetback) * this._Indent;

            if (ImportStep.SuperiorImportStep != null 
                && ImportStep.Level > 0
                && ImportStep.StepParallelNumber() != null)
            {
                this.checkBox.Text = ImportStep.StepParallelNumber().ToString();
            }
            else
            {
                if (Selector.Length > 0)
                    this.checkBox.Text = Selector;
                else
                    this.checkBox.Text = ImportStep.Title;
            }

            if (ImportStep.Image == null)
                this.pictureBox.Width = 1;
            else
            {
                this.pictureBox.Image = ImportStep.Image;
                this.pictureBox.Width = 16;
            }

            this._ImportSteps = new List<Import_Step>();
            this._ImportSteps.Add(ImportStep);
            this.checkBox.Checked = true;
        }
        
        #endregion

        #region Interface
        
        public void setSelection(bool StepsAreSelected)
        {
            this.checkBox.Checked = StepsAreSelected;
        }

        public bool isSelected { get { return this.checkBox.Checked; } }

        public void addImportStep(DiversityCollection.Import_Step Step)
        {
            this._ImportSteps.Add(Step);
        }

        public void setSelectionDependingOnVisibilityOfSteps()
        {
            int NumberOfVisibleSteps = 0;
            foreach (DiversityCollection.Import_Step IS in this._ImportSteps)
            {
                if (IS.IsVisible()) NumberOfVisibleSteps++;
            }
            if (NumberOfVisibleSteps == this._ImportSteps.Count)
                this.checkBox.Checked = true;
            else if (NumberOfVisibleSteps == 0)
                this.checkBox.Checked = false;
        }

        public System.Collections.Generic.List<DiversityCollection.Import_Step> ImportSteps() { return this._ImportSteps; }
        
        #endregion

        #region Control events and setting of the steps
        
        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            //foreach (DiversityCollection.Import_Step IS in this._ImportSteps)
            //{
            //    IS.setImportStepVisibility(this.checkBox.Checked);
            //    System.Collections.Generic.SortedList<string, DiversityCollection.Import_Step> DependentImportSteps = new SortedList<string, Import_Step>();
            //    foreach(System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
            //    {
            //        if (KV.Value.SuperiorImportStep == IS)
            //            DependentImportSteps.Add(KV.Value.StepKey(), KV.Value);
            //    }
            //    if (DependentImportSteps.Count > 0)
            //    {
            //        foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DependentImportSteps)
            //        {
            //            KV.Value.setImportStepVisibility(this.checkBox.Checked);
            //            //KV.Value.setTabPageVisibility(this.checkBox.Checked);
            //        }
            //    }
            //    foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> KV in DiversityCollection.Import.ImportSteps)
            //    {
            //        if (KV.Value.SuperiorImportStep == IS)
            //            KV.Value.setImportStepVisibility(this.checkBox.Checked);
            //    }
            //}
            //if (this.checkBox.Checked)
            //{
            //    if (!DiversityCollection.Import.ImportSteps.ContainsKey(this._ImportSteps[0].StepKey()))
            //        DiversityCollection.Import.ImportSteps.Add(this._ImportSteps[0].StepKey(), this._ImportSteps[0]);
            //}
        }

        public void setStepSelection()
        {
            this.setStepSelectionProject();

            System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)this.Parent;
            foreach (DiversityCollection.Import_Step IS in this._ImportSteps)
            {
                IS.setImportStepVisibility(this.checkBox.Checked);
                // if CollectionEventSeries is selected, deselect all other steps
                try
                {
                    if (IS.TableName() == "CollectionEventSeries")
                    {
                        //System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)this.Parent;
                        foreach (System.Windows.Forms.Control C in P.Controls)
                        {
                            if (C != this)
                            {
                                DiversityCollection.UserControls.UserControlImportSelector U = (DiversityCollection.UserControls.UserControlImportSelector)C;
                                if (this.checkBox.Checked)
                                {
                                    U.setSelection(false);
                                    U.setStepSelection();
                                    U.Enabled = false;
                                }
                                else
                                {
                                    if (U._ImportSteps[0].TableName() != "CollectionProject")
                                        U.Enabled = true;
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex) { }
            }
            if (this.checkBox.Checked)
            {
                if (!DiversityCollection.Import.ImportSteps.ContainsKey(this._ImportSteps[0].StepKey()))
                    DiversityCollection.Import.ImportSteps.Add(this._ImportSteps[0].StepKey(), this._ImportSteps[0]);
            }
        }

        public void setStepSelectionProject()
        {
            bool MustSelectProject = false;
            System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)this.Parent;
            if (DiversityCollection.Import.AttachmentKeyImportColumn == null ||
                DiversityCollection.Import.AttachmentKeyImportColumn.Table == "CollectionEventSeries" ||
                DiversityCollection.Import.AttachmentKeyImportColumn.Table == "CollectionEvent")
            {
                foreach (System.Windows.Forms.Control C in P.Controls)
                {
                    DiversityCollection.UserControls.UserControlImportSelector U = (DiversityCollection.UserControls.UserControlImportSelector)C;
                    string Table = U._ImportSteps[0].TableName();
                    if (U._ImportSteps[0].TableName() != "CollectionEvent"
                        && U._ImportSteps[0].TableName() != "CollectionEventSeries"
                        && U._ImportSteps[0].TableName() != "CollectionProject")
                    {
                        if (U.checkBox.Checked)
                        {
                            MustSelectProject = true;
                            break;
                        }
                    }
                    if (MustSelectProject)
                        break;
                }
            }

            bool MustCheck = false;
            if (!MustSelectProject)
            {
                if (DiversityCollection.Import.AttachmentKeyImportColumn == null)
                {
                    MustCheck = true;
                }
                else
                {
                    if (DiversityCollection.Import.AttachmentKeyImportColumn.Column == null)
                        MustCheck = true;
                    else
                    {
                        if (DiversityCollection.Import.AttachmentKeyImportColumn.Column.Length == 0)
                            MustCheck = true;
                    }
                }
            }

            if (MustCheck)
            {
                foreach (System.Windows.Forms.Control C in P.Controls)
                {
                    DiversityCollection.UserControls.UserControlImportSelector U = (DiversityCollection.UserControls.UserControlImportSelector)C;
                    string Table = U._ImportSteps[0].TableName();
                    if (U.checkBox.Checked &&
                        (Table.StartsWith("CollectionSpecimen") ||
                        Table.StartsWith("Identification")))
                    {
                        MustSelectProject = true;
                        break;
                    }
                }
            }

            foreach (System.Windows.Forms.Control C in P.Controls)
            {
                DiversityCollection.UserControls.UserControlImportSelector U = (DiversityCollection.UserControls.UserControlImportSelector)C;
                if (U._ImportSteps[0].TableName() == "CollectionProject")
                {
                    if (U.checkBox.Checked != MustSelectProject)
                    {
                        U.setSelection(MustSelectProject);
                        U.setStepSelection();
                    }
                    break;
                }
            }
        }


        private void checkBox_Click(object sender, EventArgs e)
        {
            if (DiversityCollection.Import.AttachmentKeyImportColumn != null
                && DiversityCollection.Import.AttachmentKeyImportColumn.Table == this.ImportSteps()[0].TableName()
                && this.ImportSteps()[0].StepParallelNumber() != null 
                && this.ImportSteps()[0].StepParallelNumber() > 1)
            {
                System.Windows.Forms.MessageBox.Show("Only one item for the table " + this.ImportSteps()[0].TableName() + " can be selected" +
                "\r\nif you attach data using a identifier in this table");
                return;
            }
            this.setStepSelection();
        }

        #endregion
    }
}
