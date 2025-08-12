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
    public partial class FormIdentificationUnitGridNewEntry : Form
    {

        #region Parameter
        private DiversityCollection.Datasets.DataSetCollectionSpecimen _DsCollectionSpecimen;
        private string _AccNrInitials = "";
        public enum UnitCopyMode { PartOfLastSpecimen, NewSpecimen };
        private UnitCopyMode _UnitCopyMode;
        private string _TaxonomicGroup;
        private int? _UnitID;
        private System.Data.DataRow _RoriginalUnit;

        public UnitCopyMode IdentificationUnitCopyMode
        {
            get 
            { 
                return _UnitCopyMode; 
            }
            //set { _UnitCopyMode = value; }
        }

        public string AccessionNumber { get { return this.textBoxAccessionNumber.Text; } }

        #endregion

        #region Construction

        public FormIdentificationUnitGridNewEntry(
            DiversityCollection.Datasets.DataSetCollectionSpecimen DsCollectionSpecimen
            , string TaxonomicGroup)
        {
            InitializeComponent();
            this._TaxonomicGroup = TaxonomicGroup;
            this.treeViewOriginal.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewCopy.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewNewEvent.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewSameEvent.ImageList = DiversityCollection.Specimen.ImageList;
            this._DsCollectionSpecimen = DsCollectionSpecimen;
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
            this.initForm();
        }

        public FormIdentificationUnitGridNewEntry(
            DiversityCollection.Datasets.DataSetCollectionSpecimen DsCollectionSpecimen
            , int UnitID)
        {
            InitializeComponent();
            this._UnitID = UnitID;
            this.treeViewOriginal.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewCopy.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewNewEvent.ImageList = DiversityCollection.Specimen.ImageList;
            this.treeViewSameEvent.ImageList = DiversityCollection.Specimen.ImageList;
            this._DsCollectionSpecimen = DsCollectionSpecimen;
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
            this.initForm();
        }

        #endregion

        private void initForm()
        {
            if (this._UnitID != null)
            {
                System.Data.DataRow[] rr = this._DsCollectionSpecimen.IdentificationUnit.Select("IdentificationUnitID = " + this._UnitID.ToString());
                if (rr.Length > 0)
                {
                    this._TaxonomicGroup = rr[0]["TaxonomicGroup"].ToString();
                    this._RoriginalUnit = rr[0];
                }
                else this._RoriginalUnit = this._DsCollectionSpecimen.IdentificationUnit.Rows[0];

            }
            //throw new NotImplementedException();
            this.initTrees();
            this.userControlDialogPanel.buttonOK.Enabled = false;
            this.buttonNewSpecimen.Enabled = true;
            this.buttonSameSpecimen.Enabled = true;
            this.enableAccesionNumberControls(false);
            if (!this._DsCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
                this._AccNrInitials = this._DsCollectionSpecimen.CollectionSpecimen.Rows[0]["AccessionNumber"].ToString();
        }

        private void initTrees()
        {
            if (this._DsCollectionSpecimen.IdentificationUnit.Rows.Count > 0 && this._DsCollectionSpecimen.CollectionSpecimen.Rows.Count > 0)
            {
                this.treeViewCopy.Nodes.Clear();
                this.treeViewOriginal.Nodes.Clear();
                this.treeViewNewEvent.Nodes.Clear();
                this.treeViewSameEvent.Nodes.Clear();

                System.Data.DataRow Rnew = this._DsCollectionSpecimen.IdentificationUnit.NewIdentificationUnitRow();
                foreach (System.Data.DataColumn C in this._DsCollectionSpecimen.IdentificationUnit.Columns)
                {
                    Rnew[C.ColumnName] = this._DsCollectionSpecimen.IdentificationUnit.Rows[0][C.ColumnName];
                }
                if (this._TaxonomicGroup.Length > 0)
                    Rnew["TaxonomicGroup"] = this._TaxonomicGroup;

                // OldData
                DiversityCollection.HierarchyNode HNOldDataSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], true);
                this.treeViewOriginal.Nodes.Add(HNOldDataSpecimen);
                DiversityCollection.HierarchyNode HNOldDataUnit = new HierarchyNode(this._RoriginalUnit, true);
                HNOldDataSpecimen.Nodes.Add(HNOldDataUnit);
                this.treeViewOriginal.ExpandAll();

                // New data
                DiversityCollection.HierarchyNode HNNewDataSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], false);
                if (this.textBoxAccessionNumber.Text.Length > 0)
                    HNNewDataSpecimen.Text = this.textBoxAccessionNumber.Text;
                else
                    HNNewDataSpecimen.Text = "New specimen";
                this.treeViewCopy.Nodes.Add(HNNewDataSpecimen);
                DiversityCollection.HierarchyNode HNNewDataUnit = new HierarchyNode(Rnew, false);
                HNNewDataUnit.Text = "New organism";
                HNNewDataSpecimen.Nodes.Add(HNNewDataUnit);
                this.treeViewCopy.ExpandAll();



                // Same specimen
                DiversityCollection.HierarchyNode HNSameSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], true);
                this.treeViewSameEvent.Nodes.Add(HNSameSpecimen);
                DiversityCollection.HierarchyNode HNSameSpecimenUnit = new HierarchyNode(this._RoriginalUnit, true);
                HNSameSpecimen.Nodes.Add(HNSameSpecimenUnit);
                DiversityCollection.HierarchyNode HNSameSpecimenNewUnit = new HierarchyNode(Rnew, false);
                HNSameSpecimenNewUnit.Text = "New organism";
                HNSameSpecimen.Nodes.Add(HNSameSpecimenNewUnit);
                this.treeViewSameEvent.ExpandAll();

                // New specimen
                DiversityCollection.HierarchyNode HNOldSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], true);
                this.treeViewNewEvent.Nodes.Add(HNOldSpecimen);
                DiversityCollection.HierarchyNode HNOldSpecimenUnit = new HierarchyNode(this._RoriginalUnit, true);
                HNOldSpecimen.Nodes.Add(HNOldSpecimenUnit);
                DiversityCollection.HierarchyNode HNNewSpecimen = new HierarchyNode(this._DsCollectionSpecimen.CollectionSpecimen.Rows[0], false);
                if (this.textBoxAccessionNumber.Text.Length > 0)
                    HNNewSpecimen.Text = this.textBoxAccessionNumber.Text;
                else
                    HNNewSpecimen.Text = "New specimen";
                this.treeViewNewEvent.Nodes.Add(HNNewSpecimen);
                DiversityCollection.HierarchyNode HNNewSpecimenUnit = new HierarchyNode(Rnew, false);
                HNNewSpecimenUnit.Text = "New organism";
                HNNewSpecimen.Nodes.Add(HNNewSpecimenUnit);
                this.treeViewNewEvent.ExpandAll();

            }
        }

        private void buttonSameSpecimen_Click(object sender, EventArgs e)
        {
            this.treeViewSameEvent.BackColor = System.Drawing.Color.White;
            this.treeViewNewEvent.BackColor = System.Drawing.SystemColors.Control;
            this._UnitCopyMode = UnitCopyMode.PartOfLastSpecimen;
            this.userControlDialogPanel.buttonOK.Enabled = true;
            this.enableAccesionNumberControls(false);
        }

        private void buttonNewSpecimen_Click(object sender, EventArgs e)
        {
            this.treeViewSameEvent.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewNewEvent.BackColor = System.Drawing.Color.White;
            this._UnitCopyMode = UnitCopyMode.NewSpecimen;
            this.userControlDialogPanel.buttonOK.Enabled = true;
            this.enableAccesionNumberControls(true);
        }

        #region AccessionNumber

        private void buttonFindNextAccessionNumber_Click(object sender, EventArgs e)
        {
            string NextAccNr = this.getNextFreeAccessionNumber(this._AccNrInitials, 1000);
            if (NextAccNr.Length > 0)
            {
                this.textBoxAccessionNumber.Text = NextAccNr;
                //this._DsCollectionSpecimen.CollectionSpecimen.Rows[1]["AccessionNumber"] = NextAccNr;
            }
            else
                System.Windows.Forms.MessageBox.Show("No free accession number could be found");
        }

        private string getNextFreeAccessionNumber(string AccNr, int Timeout)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            string NextAccNr = "";
            string SQL = "SELECT [dbo].[NextFreeAccNr] ('" + AccNr + "')";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionStringWithTimeout(Timeout));
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            con.Open();
            try
            {
                NextAccNr = C.ExecuteScalar()?.ToString();
            }
            catch
            {

            }
            finally { this.Cursor = System.Windows.Forms.Cursors.Default; }
            con.Close();
            return NextAccNr;
        }

        private void textBoxAccessionNumber_TextChanged(object sender, EventArgs e)
        {
            this._AccNrInitials = this.textBoxAccessionNumber.Text;
            this.initTrees();
        }

        private void enableAccesionNumberControls(bool Enabled)
        {
            this.textBoxAccessionNumber.Enabled = Enabled;
            this.buttonFindNextAccessionNumber.Enabled = Enabled;
            this.labelAccessionNumber.Enabled = Enabled;
        }

        #endregion


    }
}
