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
    public partial class UserControl_Relation : UserControl__Data
    {
        #region Parameter
        private bool _IsInvers;
        #endregion

        #region Construction

        public UserControl_Relation(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            bool IsInvers,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._IsInvers = IsInvers;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Interface

        public void setCollectionSource(bool Reset = false)
        {
            try
            {
                if (DiversityCollection.LookupTable.DtCollectionWithHierarchy != null && (this.comboBoxRelatedSpecimenCollectionID.DataSource == null || Reset))
                {
                    if (DiversityCollection.LookupTable.DtCollectionWithHierarchy != null)
                    {
                        int DropDownWithForCollection = 360;
                        System.Data.DataTable dtCollectionOfRelation = DiversityCollection.LookupTable.DtCollectionWithHierarchy.Copy();
                        this.comboBoxRelatedSpecimenCollectionID.DataSource = dtCollectionOfRelation;
                        this.comboBoxRelatedSpecimenCollectionID.DisplayMember = "DisplayText";
                        this.comboBoxRelatedSpecimenCollectionID.ValueMember = "CollectionID";
                        this.comboBoxRelatedSpecimenCollectionID.DropDownWidth = DropDownWithForCollection;
                    }

                    System.Data.DataTable DtCollectionForRelation = DiversityCollection.LookupTable.DtCollectionWithHierarchy.Copy();
                    this.userControlHierarchySelectorRelatedSpecimenCollectionID.initHierarchy(
                        DtCollectionForRelation,
                        "CollectionID",
                        "CollectionParentID",
                        "CollectionName",
                        "CollectionName",
                        "RelatedSpecimenCollectionID",
                        this._Source, true);
                    this.setRelatedSpecimenControls();
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.setRelatedSpecimenControls();
            if (this._iMainForm.SelectedUnitHierarchyNode() != null)
                this.pictureBoxSpecimenRelation.Image = DiversityCollection.Specimen.ImageList.Images[this._iMainForm.SelectedUnitHierarchyNode().ImageIndex];
            else if (this._iMainForm.SelectedPartHierarchyNode() != null)
                this.pictureBoxSpecimenRelation.Image = DiversityCollection.Specimen.ImageList.Images[this._iMainForm.SelectedPartHierarchyNode().ImageIndex];
            this.setCollectionSource();
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryRelatedSpecimen, "RelatedSpecimenURI");
        }

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelRelation
        {
            get { return this.tableLayoutPanelRelation; }
        }

        #endregion

        #region Control & Events

        private void initControl()
        {
            if (this._IsInvers)
                this.initControlInvers();
            else
            {
                this.splitContainerOverviewRelation.Panel1Collapsed = false;
                this.splitContainerOverviewRelation.Panel2Collapsed = true;
                this.textBoxRelatedSpecimenDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "RelatedSpecimenDescription", true));
                this.textBoxRelationNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
                this.textBoxRelatedSpecimenURL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "RelatedSpecimenURI", true));
                this.comboBoxRelatedSpecimenCollectionID.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "RelatedSpecimenCollectionID", true));
                this.comboBoxRelatedSpecimenRelationType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "RelationType", true));

                //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxRelatedSpecimenURL);

                //this.initEnumCombobox(this.comboBoxRelatedSpecimenRelationType, "CollSpecimenRelationType_Enum");
                this._EnumComboBoxes = new Dictionary<ComboBox, string>();
                this._EnumComboBoxes.Add(this.comboBoxRelatedSpecimenRelationType, "CollSpecimenRelationType_Enum");
                this.InitEnums();

                DiversityWorkbench.CollectionSpecimen S = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryRelatedSpecimen, S, "CollectionSpecimenRelation", "RelatedSpecimenDisplayText", "RelatedSpecimenURI", this._Source);

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

                this.setCollectionSource();
            }

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void initControlInvers()
        {
            this.splitContainerOverviewRelation.Panel1Collapsed = true;
            this.splitContainerOverviewRelation.Panel2Collapsed = false;
            this.textBoxRelatedSpecimenInversDescripition.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "RelatedSpecimenDescription", true));
            this.textBoxRelatedSpecimenInversNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxRelatedSpecimenInversDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "RelatedSpecimenDisplayText", true));
            this.textBoxRelatedSpecimenInversRelationType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "RelationType", true));
        }

        private void textBoxRelatedSpecimenURL_KeyUp(object sender, KeyEventArgs e)
        {
            if (this._Source.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                bool Internal;
                try
                {
                    if (bool.TryParse(R["IsInternalRelationCache"].ToString(), out Internal))
                    {
                        if (!Internal)
                        {
                            if (R["RelatedSpecimenURI"].ToString() != this.textBoxRelatedSpecimenURL.Text)
                            {
                                //this.buttonSaveUri.Visible = true;
                                //this.textBoxRelatedSpecimenURL.BackColor = System.Drawing.Color.Pink;

                                R.BeginEdit();
                                R["RelatedSpecimenURI"] = this.textBoxRelatedSpecimenURL.Text;
                                R["RelatedSpecimenDisplayText"] = R["RelatedSpecimenURI"].ToString();
                                R.EndEdit();
                                //this.textBoxRelatedSpecimenURL.SelectionStart = this.textBoxRelatedSpecimenURL.Text.Length; //.DeselectAll(); //.Select(this.textBoxRelatedSpecimenURL.Text.Length, 0);
                                //this.textBoxRelatedSpecimenURL.SelectionLength = 0;
                            }
                        }
                    }

                }
                catch (System.Data.ConstraintException cex)
                {
                    System.Windows.Forms.MessageBox.Show(cex.Message);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void buttonRelatedSpecimenURL_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(R["RelatedSpecimenURI"].ToString());
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                try
                {
                    this.textBoxRelatedSpecimenURL.Text = f.URL;
                    R.BeginEdit();
                    R["RelatedSpecimenDisplayText"] = f.URL;
                    R["RelatedSpecimenURI"] = f.URL;
                    R.EndEdit();

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void setRelatedSpecimenControls()
        {
            if (this._Source.Current != null && this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenRelation.Rows.Count > 0)
            {
                try
                {
                    this.tableLayoutPanelRelation.Visible = true;
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    bool IsInternal = false;
                    IsInternal = bool.Parse(R["IsInternalRelationCache"].ToString());
                    string DisplayText = R["RelationType"].ToString() + " ";
                    if (DisplayText.Length == 1) DisplayText = DisplayText.Trim();
                    DisplayText += R["RelatedSpecimenDisplayText"].ToString();
                    // Internal or not
                    this.userControlModuleRelatedEntryRelatedSpecimen.Visible = IsInternal;
                    this.comboBoxRelatedSpecimenCollectionID.Visible = !IsInternal;
                    this.labelRelatedSpecimenCollectionID.Visible = !IsInternal;
                    this.userControlHierarchySelectorRelatedSpecimenCollectionID.Visible = !IsInternal;
                    this.buttonRelatedSpecimenURL.Visible = !IsInternal;
                    this.textBoxRelatedSpecimenURL.Visible = !IsInternal;

                    if (IsInternal)
                    {
                        this.tableLayoutPanelRelation.SetColumnSpan(this.labelRelatedSpecimenRelationType, 3);
                        this.tableLayoutPanelRelation.SetColumnSpan(this.comboBoxRelatedSpecimenRelationType, 3);
                        this.groupBoxSpecimenRelation.Text = DiversityCollection.Forms.FormCollectionSpecimenText.Internal_relation_to + " " + DisplayText;// this.Message("Internal_relation_to") + " " + DisplayText;

                    }
                    else
                    {
                        this.tableLayoutPanelRelation.SetColumnSpan(this.labelRelatedSpecimenRelationType, 1);
                        this.tableLayoutPanelRelation.SetColumnSpan(this.comboBoxRelatedSpecimenRelationType, 1);
                        if (!R["RelatedSpecimenCollectionID"].Equals(System.DBNull.Value))
                            DisplayText += " in " + DiversityCollection.LookupTable.CollectionName(int.Parse(R["RelatedSpecimenCollectionID"].ToString()));
                        this.groupBoxSpecimenRelation.Text = DiversityCollection.Forms.FormCollectionSpecimenText.External_relation_to + " " + DisplayText;// this.Message("External_relation_to") + " " + DisplayText;
                    }
                    if (this.groupBoxSpecimenRelation.Visible && this.groupBoxSpecimenRelation.Text.Length > 40)
                    {
                        this.groupBoxSpecimenRelation.Padding = new Padding(3, 16, 3, 3);
                    }
                    else
                        this.groupBoxSpecimenRelation.Padding = new Padding(3, 3, 3, 3);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
                this.tableLayoutPanelRelation.Visible = false;
        }

        private void buttonChangeToRelatedSpecimen_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._iMainForm.SelectedUnitHierarchyNode() != null)
                {
                    System.Data.DataRow R = (System.Data.DataRow)this._iMainForm.SelectedUnitHierarchyNode().Tag;
                    int ID = int.Parse(R["CollectionSpecimenID"].ToString());
                    this._iMainForm.setSpecimen(ID);//.setSpecimen(ID);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSaveUri_Click(object sender, EventArgs e)
        {
            if (this._Source.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                bool Internal;
                try
                {
                    if (bool.TryParse(R["IsInternalRelationCache"].ToString(), out Internal))
                    {
                        if (!Internal)
                        {
                            if (R["RelatedSpecimenURI"].ToString() != this.textBoxRelatedSpecimenURL.Text)
                            {
                                R.BeginEdit();
                                R["RelatedSpecimenURI"] = this.textBoxRelatedSpecimenURL.Text;
                                R["RelatedSpecimenDisplayText"] = R["RelatedSpecimenURI"].ToString();
                                R.EndEdit();
                                //this.textBoxRelatedSpecimenURL.SelectionStart = this.textBoxRelatedSpecimenURL.Text.Length; //.DeselectAll(); //.Select(this.textBoxRelatedSpecimenURL.Text.Length, 0);
                                //this.textBoxRelatedSpecimenURL.SelectionLength = 0;
                            }
                        }
                    }

                }
                catch (System.Data.ConstraintException cex)
                {
                    System.Windows.Forms.MessageBox.Show(cex.Message);
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            this.buttonSaveUri.Visible = false;
            this.textBoxRelatedSpecimenURL.BackColor = System.Drawing.SystemColors.Window;
        }

        #endregion
    }
}
