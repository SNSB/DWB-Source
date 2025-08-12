using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Maintenance
{
    public partial class UserControlSynchronizeTerm : UserControl
    {
        #region Parameter

        private Maintenance.SynchronizeTerm _SynchronizeTerm;
        private string _LabelImageSpacer = "      ";

        #endregion

        #region Construction
        public UserControlSynchronizeTerm(SynchronizeTerm synchronizeTerm)
        {
            InitializeComponent();
            _SynchronizeTerm = synchronizeTerm;
            this.InitControl();
            synchronizeTerm.setUserControl(this);
        }

        #endregion

        #region Init

        private void InitControl()
        {
            this.labelLanguage.Visible = !_SynchronizeTerm.Linked;
            this.comboBoxLanguage.Visible = !_SynchronizeTerm.Linked;
            this.checkBoxMaxResults.Visible = !_SynchronizeTerm.Linked;
            this.numericUpDownMaxResults.Visible = !_SynchronizeTerm.Linked;
            this.checkBoxTermRestriction.Visible = !_SynchronizeTerm.Linked;
            this.textBoxTermRestriction.Visible = !_SynchronizeTerm.Linked;
            if (_SynchronizeTerm.Linked)
            {
                this.checkBoxHierarchy.Text = "Compare hierarchy";
                this.buttonSearch.Text = "Search for differences";
            }

            this.setTableRestrictionLabel();
            this.setTableRestrictionSource();
            this.initDatabase();
            this.initProjects();
        }

        private void setTableRestrictionLabel()
        {
            switch(_SynchronizeTerm.Target)
            {
                case SynchronizeTerm.TargetTable.CollectionEventProperty:
                    this.labelTableRestriction.Image = Resource.EventProperty;
                    this.labelTableRestriction.Text = _LabelImageSpacer + "Site property";
                    break;
                case SynchronizeTerm.TargetTable.CollectionSpecimenPartDescription:
                    this.labelTableRestriction.Image = Resource.Specimen;
                    this.labelTableRestriction.Text = _LabelImageSpacer + "Material";
                    break;
                case SynchronizeTerm.TargetTable.Identification:
                    this.labelTableRestriction.Image = Resource.Kristall;
                    this.labelTableRestriction.Text = _LabelImageSpacer + "Group";
                    break;
            }
        }

        private void setTableRestrictionSource()
        {
            this.comboBoxTableRestriction.DataSource = _SynchronizeTerm.TableRestriction();
            this.comboBoxTableRestriction.DisplayMember = "Display";
            this.comboBoxTableRestriction.ValueMember = "Value";
        }

        private void initDatabase()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KVconn in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityScientificTerms"].ServerConnectionList())
            {
                this.comboBoxDatabase.Items.Add(KVconn.Key);
            }
        }

        private void initProjects()
        {
            this.comboBoxProject.DataSource = _SynchronizeTerm.ProjectList(_SynchronizeTerm.Linked);
            this.comboBoxProject.DisplayMember = "Project";
            this.comboBoxProject.ValueMember = "ProjectID";
        }

        #endregion

        #region Interface

        public System.Windows.Forms.ProgressBar ProgressBar { get { return this.progressBar; } }

        public System.Windows.Forms.DataGridView DataGridView { get { return this.dataGridView; } }

        public System.Windows.Forms.Label LabelResult { get { return this.labelResult; } }

        public System.Windows.Forms.Button UpdateButton { get => this.buttonUpdate; }

        public System.Windows.Forms.Label LabelRestrictionTable { get => this.labelTableRestriction; }

        public System.Windows.Forms.ComboBox ComboboxRestrictionTable { get => this.comboBoxTableRestriction; }


        #endregion

        #region Events

        #region Database
        private void comboBoxDatabase_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string Database = "";
                if (comboBoxDatabase.SelectedItem.GetType() == typeof(System.Data.DataRowView))
                {
                    System.Data.DataRowView r = (System.Data.DataRowView)comboBoxDatabase.SelectedValue;
                    Database = r[0].ToString();
                }
                else 
                    Database = comboBoxDatabase.SelectedItem.ToString();
                _SynchronizeTerm.setDatabase(Database);
                this.labelDatabase.Text = _LabelImageSpacer + _SynchronizeTerm.DatabaseNameDST();
                this.setTerminologySource();
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region Terminology

        private void setTerminologySource()
        {
            try
            {
                comboBoxTerminology.DataSource = _SynchronizeTerm.getTerminologies();
                comboBoxTerminology.DisplayMember = "DisplayText";
                comboBoxTerminology.ValueMember = "TerminologyID";
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void comboBoxTerminology_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ID = -1;
            if (int.TryParse(comboBoxTerminology.SelectedValue.ToString(), out ID))
            {
                _SynchronizeTerm.setTerminology(ID);
            }
            else if (comboBoxTerminology.SelectedValue.GetType() == typeof(System.Data.DataRowView))
            {
                System.Data.DataRowView R = (System.Data.DataRowView)comboBoxTerminology.SelectedValue;
                if (int.TryParse(R[0].ToString(), out ID))
                {
                    _SynchronizeTerm.setTerminology(ID);
                }
            }
            if (ID > -1 && !_SynchronizeTerm.Linked)
                this.setLanguageSource();
            this.SetTableRestriction();
        }

        #endregion

        #region Hierarchy

        private void checkBoxHierarchy_Click(object sender, EventArgs e)
        {
            setHierarchy();
        }

        private void radioButtonHierarchyTopDown_Click(object sender, EventArgs e)
        {
            setHierarchy();
        }

        private void radioButtonHierarchyButtomUp_Click(object sender, EventArgs e)
        {
            setHierarchy();
        }

        private void setHierarchy()
        {
            if (checkBoxHierarchy.Checked)
            {
                if (radioButtonHierarchyTopDown.Checked) _SynchronizeTerm.setHierarchy(SynchronizeTerm.Hierarchy.TopDetail);
                else if (radioButtonHierarchyButtomUp.Checked) _SynchronizeTerm.setHierarchy(SynchronizeTerm.Hierarchy.DetailTop);
                else _SynchronizeTerm.setHierarchy(SynchronizeTerm.Hierarchy.SelectionMissing);
            }
            else _SynchronizeTerm.setHierarchy(SynchronizeTerm.Hierarchy.None);
        }

        #endregion

        #region Term restriction

        private void checkBoxTermRestriction_Click(object sender, EventArgs e)
        {
            setTermRestriction();
        }

        private void textBoxTermRestriction_TextChanged(object sender, EventArgs e)
        {
            setTermRestriction();
        }

        private void setTermRestriction()
        {
            if (checkBoxTermRestriction.Checked) _SynchronizeTerm.setTermRestriction(this.textBoxTermRestriction.Text);
            else _SynchronizeTerm.setTermRestriction("");
        }

        #endregion

        #region Max results

        private void checkBoxMaxResults_Click(object sender, EventArgs e)
        {
            setMax();
        }

        private void numericUpDownMaxResults_ValueChanged(object sender, EventArgs e)
        {
            setMax();
        }

        private void setMax()
        {
            if (this.checkBoxMaxResults.Checked) _SynchronizeTerm.setMaxResults((int)this.numericUpDownMaxResults.Value);
            else _SynchronizeTerm.setMaxResults(null);
        }

        #endregion

        #region Include AccNr

        private void checkBoxIncludeAccNr_Click(object sender, EventArgs e)
        {
            this.buttonShowDetails.Visible = checkBoxIncludeAccNr.Checked;

            _SynchronizeTerm.IncludeAccessionNumbers(this.checkBoxIncludeAccNr.Checked);
        }

        private void buttonShowDetails_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedCells.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a dataset");
                return;
            }
            int CollectionSpecimenID = 0;
            int Position = this.dataGridView.SelectedCells[0].RowIndex;
            bool HasValidColumn = false;
            if (this.dataGridView.DataSource.GetType() == typeof(System.Data.DataTable))
            {
                System.Data.DataTable dt = (System.Data.DataTable)this.dataGridView.DataSource;
                HasValidColumn = dt.Columns.Contains("CollectionSpecimenID");
            }
            if (HasValidColumn && int.TryParse(this.dataGridView.Rows[Position].Cells["CollectionSpecimenID"].Value.ToString(), out CollectionSpecimenID))
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                DiversityCollection.Forms.FormCollectionSpecimen f = new Forms.FormCollectionSpecimen(CollectionSpecimenID, false, false);
                f.setViewMode(Forms.FormCollectionSpecimen.ViewMode.SingleInspectionMode);
                f.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
                f.Width = FormMaintenance().Width - 10;
                f.Height = FormMaintenance().Height - 10;
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Dataset in database can only be checked if you include the accession number before searching");
            }
        }

        private DiversityCollection.Forms.FormMaintenance _FormMaintenance;

        private System.Windows.Forms.Form FormMaintenance()
        {
            if (this._FormMaintenance == null)
            {
                System.Windows.Forms.Control c = this;
                while (_FormMaintenance == null)
                {
                    c = c.Parent;
                    if (c.Parent.GetType() == typeof(DiversityCollection.Forms.FormMaintenance))
                    {
                        _FormMaintenance = (DiversityCollection.Forms.FormMaintenance)c.Parent;
                        break;
                    }
                }
            }
            return _FormMaintenance;
        }

        #endregion

        #region Search

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            int i = _SynchronizeTerm.StartSearch();
            this.Cursor = System.Windows.Forms.Cursors.Default;
            if (i > 0)
            {
                this.labelResult.Text = i.ToString() + " matches found";
                this.buttonUpdate.Enabled = true;
            }
            else
            {
                this.labelResult.Text = "No match found";
                this.buttonUpdate.Enabled = false;
            }
        }

        #endregion

        #region Selection

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            _SynchronizeTerm.SelectAllRows();
        }

        private void buttonSelectNone_Click(object sender, EventArgs e)
        {
            _SynchronizeTerm.DeselectAllRows();
        }

        #endregion

        #region Update

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            _SynchronizeTerm.Update();
        }

        #endregion

        #region Table restriction

        private void SetTableRestriction()
        {
            switch(_SynchronizeTerm.Target)
            {
                case SynchronizeTerm.TargetTable.CollectionEventProperty:
                    this.SetTableRestrictionEventProperty();
                    break;
                case SynchronizeTerm.TargetTable.CollectionSpecimenPartDescription:
                    break;
                case SynchronizeTerm.TargetTable.Identification:
                    break;
            }
        }

        private void SetTableRestrictionEventProperty()
        {
            int ID;
            if (int.TryParse(this.comboBoxTerminology.SelectedValue.ToString(), out ID))
            {
                if (this.comboBoxTableRestriction.DataSource.GetType() == typeof(System.Data.DataTable))
                {
                    System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxTableRestriction.DataSource;
                    for(int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Value"].ToString() == ID.ToString())
                        {
                            this.comboBoxTableRestriction.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void comboBoxTableRestriction_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Restriction = "";
            if (comboBoxTableRestriction.SelectedValue.GetType() == typeof(System.Data.DataRowView))
            {
                System.Data.DataRowView R = (System.Data.DataRowView)comboBoxTableRestriction.SelectedValue;
                Restriction = R[0].ToString();
            }
            else Restriction = comboBoxTableRestriction.SelectedValue.ToString();
            _SynchronizeTerm.setTableRestriction(Restriction);

            this.setTableRestrictionImage(Restriction);

        }
        private void setTableRestrictionImage(string Restriction)
        {
            switch(_SynchronizeTerm.Target)
            {
                case SynchronizeTerm.TargetTable.CollectionEventProperty:
                    this.labelTableRestriction.Image = DiversityCollection.Specimen.ImageForCollectionEventProperty(Restriction);
                    break;
                case SynchronizeTerm.TargetTable.CollectionSpecimenPartDescription:
                    this.labelTableRestriction.Image = DiversityCollection.Specimen.MaterialCategoryImage(false, Restriction);
                    break;
                case SynchronizeTerm.TargetTable.Identification:
                    this.labelTableRestriction.Image = DiversityCollection.Specimen.TaxonImage(false, Restriction);
                    break;
            }
        }

        #endregion

        #region Project

        private void comboBoxProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ID;
            if (int.TryParse(comboBoxProject.SelectedValue.ToString(), out ID))
            {
                _SynchronizeTerm.setProjectID(ID);
            }
            else if (comboBoxProject.SelectedValue.GetType() == typeof(System.Data.DataRowView))
            {
                System.Data.DataRowView R = (System.Data.DataRowView)comboBoxProject.SelectedValue;
                if (int.TryParse(R[0].ToString(), out ID))
                    _SynchronizeTerm.setProjectID(ID);
            }
        }

        #endregion

        #region Language

        private void setLanguageSource()
        {
            comboBoxLanguage.DataSource = _SynchronizeTerm.DtLanguages();
            comboBoxLanguage.DisplayMember = "Display";
            comboBoxLanguage.ValueMember = "Value";
        }

        private void comboBoxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Language = "";
            if (comboBoxLanguage.SelectedValue.GetType() == typeof(System.Data.DataRowView))
            {
                System.Data.DataRowView R = (System.Data.DataRowView)comboBoxLanguage.SelectedValue;
                Language = R[0].ToString();
            }
            else
                Language = comboBoxLanguage.SelectedValue.ToString();
            _SynchronizeTerm.setLanguage(Language);
            setLanguageImage(Language);
        }

        private void setLanguageImage(string Language)
        {
            this.labelLanguage.Image = DiversityWorkbench.Language.Image(Language);
        }


        #endregion

        #endregion

    }
}
