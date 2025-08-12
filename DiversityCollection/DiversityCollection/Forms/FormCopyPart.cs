using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormCopyPart : Form
    {

        #region Parameter

        private DiversityCollection.Datasets.DataSetCollectionSpecimen _DsCollectionSpecimen;

        #endregion


        #region Construction

        public FormCopyPart(DiversityCollection.Datasets.DataSetCollectionSpecimen DsCollectionSpecimen)
        {
            InitializeComponent();
            this._DsCollectionSpecimen = DsCollectionSpecimen;
            this.initForm();
        }
        
        #endregion

        #region Form

        private void initForm()
        {
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
            this.dataGridViewCopies.DataSource = this._DsCollectionSpecimen.CollectionSpecimenPart;
            this.setCopyTable();
            for (int i = 0; i < this.dataGridViewCopies.Columns.Count; i++)
            {
                if (i == 5 || i == 6 || i == 9)
                    this.dataGridViewCopies.Columns[i].Visible = true;
                else
                    this.dataGridViewCopies.Columns[i].Visible = false;
            }
            this.checkBoxIncludeRegulation.Checked = Forms.FormCopyPart._IncludeRegulation;
            this.checkBoxIncludeAnnotations.Checked = Forms.FormCopyPart._IncludeAnnotation;
            this.checkBoxIncludeExternalIdentifier.Checked = Forms.FormCopyPart._IncludeExternalIdentifier;
            this.checkBoxIncludeProcessing.Checked = Forms.FormCopyPart._IncludeProcessing;
            this.checkBoxIncludeProcessingMethods.Checked = Forms.FormCopyPart._IncludeProcessingMethod;
            this.checkBoxIncludeReferences.Checked = Forms.FormCopyPart._IncludeReference;
            this.checkBoxIncludeRegulation.Checked = Forms.FormCopyPart._IncludeRegulation;
            this.checkBoxIncludeRelations.Checked = Forms.FormCopyPart._IncludeRelation;
            this.checkBoxIncludeSpecimenPartDescription.Checked = Forms.FormCopyPart._IncludeDescription;
            this.checkBoxIncludeTransactions.Checked = Forms.FormCopyPart._IncludeTransaction;
            this.checkBoxIncludeUnitInPart.Checked = Forms.FormCopyPart._IncludeIdentificationUnitInPart;
        }
        
        #endregion

        #region Multi Copy

        public System.Data.DataTable DataTableParts()
        {
            return this._DsCollectionSpecimen.CollectionSpecimenPart;
        }

        //public int NumberOfCopies
        //{
        //    get
        //    {
        //        try
        //        {
        //            return (int)this.numericUpDownNumberOfCopies.Value;
        //        }
        //        catch
        //        {
        //            return 1;
        //        }
        //    }
        //}

        //public bool AllowMultiCopy
        //{
        //    set
        //    {
        //        if (value)
        //        {
        //            if (!this.tabControlMain.Contains(this.tabPageMultiCopy))
        //                tabControlMain.TabPages.Add(this.tabPageMultiCopy);
        //        }
        //        else
        //        {
        //            if (this.tabControlMain.Contains(this.tabPageMultiCopy))
        //                tabControlMain.TabPages.Remove(this.tabPageMultiCopy);
        //        }
        //    }
        //}

        private void numericUpDownNumberOfCopies_Leave(object sender, EventArgs e)
        {
            if (this.numericUpDownNumberOfCopies.Value > 1)
            {
                this.userControlDialogPanel.buttonOK.Enabled = false;
            }
        }

        private void setCopyTable()
        {
            if (this._DsCollectionSpecimen.CollectionSpecimenPart.Rows.Count == 0)
                return;
            System.Data.DataRow Rori = this._DsCollectionSpecimen.CollectionSpecimenPart.Rows[0];
            int CurrentRowNumber = this._DsCollectionSpecimen.CollectionSpecimenPart.Rows.Count;
            int PlannedRowNumber = (int)this.numericUpDownNumberOfCopies.Value;
            if (CurrentRowNumber > PlannedRowNumber)
            {
                for (int i = PlannedRowNumber; i < CurrentRowNumber; i++)
                {
                    this._DsCollectionSpecimen.CollectionSpecimenPart.Rows[i].Delete();
                }
                this._DsCollectionSpecimen.CollectionSpecimenPart.AcceptChanges();
            }
            else if (CurrentRowNumber < PlannedRowNumber)
            {
                for (int i = 0; i < PlannedRowNumber - CurrentRowNumber; i++)
                {
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenPartRow RS = this._DsCollectionSpecimen.CollectionSpecimenPart.NewCollectionSpecimenPartRow();
                    foreach (System.Data.DataColumn C in this._DsCollectionSpecimen.CollectionSpecimenPart.Columns)
                    {
                        if (C.ColumnName == "SpecimenPartID")
                            continue;
                        RS[C.ColumnName] = Rori[C.ColumnName];
                    }
                    this._DsCollectionSpecimen.CollectionSpecimenPart.Rows.Add(RS);
                }
            }
            this.userControlDialogPanel.buttonOK.Enabled = true;
        }

        private void numericUpDownNumberOfCopies_ValueChanged(object sender, EventArgs e)
        {
            this.buttonRequeryMultiCopyList.Enabled = true;
            this.buttonRequeryMultiCopyList.FlatAppearance.BorderSize = 2;
        }

        private void buttonRequeryMultiCopyList_Click(object sender, EventArgs e)
        {
            this.buttonRequeryMultiCopyList.Enabled = false;
            this.buttonRequeryMultiCopyList.FlatAppearance.BorderSize = 0;
            this.setCopyTable();

        }

        #endregion

        #region Included tables

        private static bool _IncludeIdentificationUnitInPart = true;
        private static bool _IncludeDescription = false;
        private static bool _IncludeProcessing = false;
        private static bool _IncludeProcessingMethod = false;
        private static bool _IncludeRelation = false;
        private static bool _IncludeReference = false;
        private static bool _IncludeRegulation = false;
        private static bool _IncludeTransaction = false;
        private static bool _IncludeExternalIdentifier = false;
        private static bool _IncludeAnnotation = false;

        public System.Collections.Generic.List<string> IncludedTableList()
        {
            System.Collections.Generic.List<string> L = new List<string>();
            Forms.FormCopyPart._IncludeIdentificationUnitInPart = this.checkBoxIncludeUnitInPart.Checked;
            Forms.FormCopyPart._IncludeAnnotation = this.checkBoxIncludeAnnotations.Checked;
            Forms.FormCopyPart._IncludeProcessing = this.checkBoxIncludeProcessing.Checked;
            Forms.FormCopyPart._IncludeProcessingMethod = this.checkBoxIncludeProcessingMethods.Checked;
            Forms.FormCopyPart._IncludeReference = this.checkBoxIncludeReferences.Checked;
            Forms.FormCopyPart._IncludeRelation = this.checkBoxIncludeRelations.Checked;
            Forms.FormCopyPart._IncludeTransaction = this.checkBoxIncludeTransactions.Checked;
            Forms.FormCopyPart._IncludeExternalIdentifier = this.checkBoxIncludeExternalIdentifier.Checked;
            Forms.FormCopyPart._IncludeRegulation = this.checkBoxIncludeRegulation.Checked;
            Forms.FormCopyPart._IncludeDescription = this.checkBoxIncludeSpecimenPartDescription.Checked;

            if (this.checkBoxIncludeUnitInPart.Checked)
            {
                L.Add("IdentificationUnitInPart");
            }
            if (this.checkBoxIncludeProcessing.Checked)
            {
                L.Add("CollectionSpecimenProcessing");
            }
            if (this.checkBoxIncludeProcessingMethods.Checked)
            {
                L.Add("CollectionSpecimenProcessingMethod");
                L.Add("CollectionSpecimenProcessingMethodParameter");
            }
            if (this.checkBoxIncludeRelations.Checked)
            {
                L.Add("CollectionSpecimenRelation");
            }
            if (this.checkBoxIncludeReferences.Checked)
            {
                L.Add("CollectionSpecimenReference");
            }
            if (this.checkBoxIncludeTransactions.Checked)
            {
                L.Add("CollectionSpecimenTransaction");
            }
            if (this.checkBoxIncludeExternalIdentifier.Checked)
            {
                L.Add("ExternalIdentifier");
            }
            if (this.checkBoxIncludeAnnotations.Checked)
            {
                L.Add("Annotation");
            }
            return L;
        }

        public string IncludedTables()
        {
            string Tables = "|";
            foreach (string T in this.IncludedTableList())
            {
                Tables += T + "|";
            }
            return Tables;
        }

        private void checkBoxIncludeProcessingMethods_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxIncludeProcessingMethods.Checked)
                this.checkBoxIncludeProcessing.Checked = true;
        }

        private void checkBoxIncludeProcessing_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.checkBoxIncludeProcessing.Checked)
                this.checkBoxIncludeProcessingMethods.Checked = false;
        }

        #endregion

    }
}
