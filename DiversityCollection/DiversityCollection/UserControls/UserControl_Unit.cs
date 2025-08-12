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
    public partial class UserControl_Unit : UserControl__Data, iUnit
    {

        #region Parameter

        private iIdentification _iIdentification;
        private System.Data.DataTable _dtGender;
        private System.Data.DataTable _dtLifeStage;
        private System.Data.DataView _dvUnitsDisplay;
        private System.Data.DataView _dvUnitsHide;

        #endregion 

        #region Construction

        public UserControl_Unit(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            iIdentification iIdentification,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._iIdentification = iIdentification;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Public, Base

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.setUnitDatawithholding();
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            this.groupBoxUnit.Text = R["LastIdentificationCache"].ToString();
            bool IsPartOf = false;
            if (R["RelationType"].ToString() == "Part of" && !R["RelatedUnitID"].Equals(System.DBNull.Value))
                IsPartOf = true;
            string Code = R["TaxonomicGroup"].ToString();
            bool IsTaxonomyRelatedTaxonomicGroup = false;
            if (DiversityWorkbench.CollectionSpecimen.TaxonomyRelatedTaxonomicGroups.Contains(Code))
            {
                IsTaxonomyRelatedTaxonomicGroup = true;
            }
            bool Hide = false;
            if (!IsTaxonomyRelatedTaxonomicGroup)
                Hide = true;
            if (IsPartOf)
                Hide = true;
            this.labelFamilyCache.Visible = !Hide;
            this.textBoxFamilyCache.Visible = !Hide;
            this.labelOrderCache.Visible = !Hide;
            this.textBoxOrderCache.Visible = !Hide;
            this.buttonEditOrderAndFamily.Visible = !Hide;
            this.labelGender.Visible = !Hide;
            this.comboBoxGender.Visible = !Hide;
            this.labelLifeStage.Visible = !Hide;
            this.comboBoxLifeStage.Visible = !Hide;
            bool CanUpdate = this.FormFunctions.getObjectPermissions("IdentificationUnit", "UPDATE");
            if (CanUpdate
                && !IsPartOf)//                && this._Availability == DiversityCollection.CollectionSpecimen.AvailabilityState.Available)
                this.comboBoxTaxonomicGroup.Enabled = true;
            else this.comboBoxTaxonomicGroup.Enabled = false;
        }

        public override void setAvailability()
        {
            base.setAvailability();
            if (!this._iMainForm.ReadOnly())
            {
                this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelIdentificationUnit, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
                this.FormFunctions.setDataControlEnabled(this.groupBoxUnit, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
                this.FormFunctions.setDataControlEnabled(this.comboBoxTaxonomicGroup, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
                this.toolStripDisplayOrder.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE];
                this.toolStripDisplay.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE];
                this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelExsiccataUnit, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
                this.EnableImageRestriction();
            }
        }


        public void ReleaseImageRestriction()
        {
            this.buttonRestrictImagesToCurrentUnit.BackColor = System.Drawing.SystemColors.Control;
        }

        private bool _EnableImageRestriction = true;
        public void EnableImageRestriction(bool Enable)
        {
            this._EnableImageRestriction = Enable;
            this.buttonRestrictImagesToCurrentUnit.Enabled = this._EnableImageRestriction;
        }

        private void EnableImageRestriction()
        {
            this.buttonRestrictImagesToCurrentUnit.Enabled = this._EnableImageRestriction;
        }

        #endregion

        private void initControl()
        {
            this.checkBoxOnlyObserved.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this._Source, "OnlyObserved", true));

            this.comboBoxTaxonomicGroup.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "TaxonomicGroup", true));
            this.comboBoxSubstrateRelationType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "RelationType", true));
            this.comboBoxGender.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Gender", true));
            this.comboBoxLifeStage.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LifeStage", true));
            this.comboBoxCircumstances.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "Circumstances", true));
            this.comboBoxUnitRetrievalType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "RetrievalType", true));
            this.comboBoxUnitDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "UnitDescription", true));
            this.comboBoxExsiccataIdentification.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "ExsiccataIdentification", true));

            this._EnumComboBoxes = new Dictionary<ComboBox, string>();
            this._EnumComboBoxes.Add(this.comboBoxSubstrateRelationType, "CollUnitRelationType_Enum");
            this._EnumComboBoxes.Add(this.comboBoxUnitRetrievalType, "CollRetrievalType_Enum");
            this._EnumComboBoxes.Add(this.comboBoxCircumstances, "CollCircumstances_Enum");
            this._EnumComboBoxes.Add(this.comboBoxTaxonomicGroup, "CollTaxonomicGroup_Enum");
            this.InitLookupSources();

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxGender);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxLifeStage);

            this.textBoxHierarchyCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "HierarchyCache", true));

            this.textBoxNumberOfUnits.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "NumberOfUnits", true));
            this.textBoxFamilyCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "FamilyCache", true));
            this.textBoxOrderCache.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "OrderCache", true));
            this.textBoxIdentificationUnitID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "IdentificationUnitID", true));
            this.textBoxUnitNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxColonizedSubstratePart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ColonisedSubstratePart", true));
            this.textBoxUnitIdentifier.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "UnitIdentifier", true));
            this.textBoxNumberOfUnitsModifier.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "NumberOfUnitsModifier", true));
            this.textBoxExsiccataNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ExsiccataNumber", true));

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxColonizedSubstratePart);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxFamilyCache);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxOrderCache);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxUnitIdentifier);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxUnitNotes);

            this.textBoxFamilyCache.ReadOnly = true;
            this.textBoxOrderCache.ReadOnly = true;

            this.userControlHierarchySelectorCircumstances.initHierarchyForEnum("CollCircumstances_Enum", "Circumstances", this.comboBoxCircumstances, this._Source);
            this.userControlHierarchySelectorUnitDescription.initHierarchy(
                this.dtUnitDescription,
                "HierarchyColumn",
                "ParentColumn",
                "HierarchyColumn",
                "OrderColumn",
                "UnitDescription",
                "ImageIndexColumn",
                DiversityCollection.Specimen.ImageListUnitDescription,
                this.comboBoxUnitDescription,
                this._Source);

            this.setDatabindingsDisplayOrder();

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        public override void InitLookupSources() { this.InitEnums(); }

        System.Data.DataTable dtUnitDescription
        {
            get
            {
                System.Data.DataTable dtUnitDescription = new DataTable();
                System.Data.DataColumn cHierarchy = new System.Data.DataColumn("HierarchyColumn", System.Type.GetType("System.String"));
                dtUnitDescription.Columns.Add(cHierarchy);
                System.Data.DataColumn cParent = new System.Data.DataColumn("ParentColumn", System.Type.GetType("System.String"));
                dtUnitDescription.Columns.Add(cParent);
                System.Data.DataColumn cOrder = new System.Data.DataColumn("OrderColumn", System.Type.GetType("System.Int32"));
                dtUnitDescription.Columns.Add(cOrder);
                System.Data.DataColumn cValue = new System.Data.DataColumn("ValueColumn", System.Type.GetType("System.String"));
                dtUnitDescription.Columns.Add(cValue);
                System.Data.DataColumn cImageIndex = new System.Data.DataColumn("ImageIndexColumn", System.Type.GetType("System.Int32"));
                dtUnitDescription.Columns.Add(cImageIndex);

                System.Data.DataRow R0 = dtUnitDescription.NewRow();
                R0["HierarchyColumn"] = "plant";
                R0["OrderColumn"] = 0;
                dtUnitDescription.Rows.Add(R0);

                System.Data.DataRow R1 = dtUnitDescription.NewRow();
                R1["HierarchyColumn"] = "tree";
                R1["ParentColumn"] = "plant";
                R1["OrderColumn"] = 1;
                R1["ValueColumn"] = "tree";
                R1["ImageIndexColumn"] = 0;
                dtUnitDescription.Rows.Add(R1);

                System.Data.DataRow R2 = dtUnitDescription.NewRow();
                R2["HierarchyColumn"] = "branch";
                R2["ParentColumn"] = "plant";
                R2["OrderColumn"] = 2;
                R2["ValueColumn"] = "branch";
                R2["ImageIndexColumn"] = 1;
                dtUnitDescription.Rows.Add(R2);

                System.Data.DataRow R3 = dtUnitDescription.NewRow();
                R3["HierarchyColumn"] = "leaf";
                R3["ParentColumn"] = "plant";
                R3["OrderColumn"] = 3;
                R3["ValueColumn"] = "leaf";
                R3["ImageIndexColumn"] = 2;
                dtUnitDescription.Rows.Add(R3);

                System.Data.DataRow R4 = dtUnitDescription.NewRow();
                R4["HierarchyColumn"] = "flower";
                R4["ParentColumn"] = "plant";
                R4["OrderColumn"] = 4;
                R4["ValueColumn"] = "flower";
                R4["ImageIndexColumn"] = 3;
                dtUnitDescription.Rows.Add(R4);

                System.Data.DataRow R5 = dtUnitDescription.NewRow();
                R5["HierarchyColumn"] = "gall";
                R5["ParentColumn"] = "plant";
                R5["OrderColumn"] = 5;
                R5["ValueColumn"] = "gall";
                R5["ImageIndexColumn"] = 4;
                dtUnitDescription.Rows.Add(R5);

                System.Data.DataRow R6 = dtUnitDescription.NewRow();
                R6["HierarchyColumn"] = "root";
                R6["ParentColumn"] = "plant";
                R6["OrderColumn"] = 4;
                R6["ValueColumn"] = "root";
                R6["ImageIndexColumn"] = 5;
                dtUnitDescription.Rows.Add(R6);

                return dtUnitDescription;
            }
        }

        private void comboBoxExsiccataIdentification_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxExsiccataIdentification.SelectedItem;
            System.Data.DataRowView I = (System.Data.DataRowView)this._Source.Current;
            I.BeginEdit();
            I["ExsiccataIdentification"] = R[0];
            I.EndEdit();
            this._iMainForm.saveSpecimen();// this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "IdentificationUnit", this.sqlDataAdapterUnit, this.BindingContext);
        }

        public void setExsiccateIdentificationSource(int IdentificationUnitID)
        {
            System.Data.DataRow[] rrU = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("IdentificationUnitID = " + IdentificationUnitID.ToString());
            this.comboBoxExsiccataIdentification.DataBindings.Clear();
            this.comboBoxExsiccataIdentification.DataSource = null;
            System.Data.DataTable dtIdent = new System.Data.DataTable("Identification");
            System.Data.DataColumn C1 = new System.Data.DataColumn("IdentificationSequence", System.Type.GetType("System.Int16"));
            System.Data.DataColumn C2 = new System.Data.DataColumn("TaxonomicName", System.Type.GetType("System.String"));
            dtIdent.Columns.Add(C1);
            dtIdent.Columns.Add(C2);
            try
            {
                if (rrU.Length > 0)
                {
                    System.Data.DataRow rU = rrU[0];
                    System.Object[] rowVals = new object[2];
                    rowVals[0] = System.DBNull.Value;
                    rowVals[1] = System.DBNull.Value;
                    dtIdent.Rows.Add(rowVals);
                    System.Data.DataRow[] rr2 = this._iMainForm.DataSetCollectionSpecimen().Identification.Select("IdentificationUnitID = " + IdentificationUnitID.ToString());
                    foreach (System.Data.DataRow r2 in rr2)
                    {
                        System.Object[] rowVals2 = new object[2];
                        rowVals2[0] = r2["IdentificationSequence"];
                        rowVals2[1] = r2["TaxonomicName"];
                        dtIdent.Rows.Add(rowVals2);
                    }
                    this.comboBoxExsiccataIdentification.DataSource = dtIdent;
                    this.comboBoxExsiccataIdentification.DisplayMember = "TaxonomicName";
                    this.comboBoxExsiccataIdentification.ValueMember = "IdentificationSequence";
                    this.comboBoxExsiccataIdentification.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._iMainForm.DataSetCollectionSpecimen(), "IdentificationUnit.ExsiccataIdentification"));
                    if (rU["ExsiccataIdentification"].Equals(System.DBNull.Value)) this.comboBoxExsiccataIdentification.SelectedIndex = 0;
                    else
                    {
                        int EI = int.Parse(rU["ExsiccataIdentification"].ToString());
                        for (int i = 1; i < this.comboBoxExsiccataIdentification.Items.Count; i++)
                        {
                            System.Data.DataRowView RV = (System.Data.DataRowView)this.comboBoxExsiccataIdentification.Items[i];
                            if (RV[0].ToString() == EI.ToString())
                            {
                                this.comboBoxExsiccataIdentification.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void textBoxExsiccataNumber_TextChanged(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            if (R != null)
            {
                R.BeginEdit();
                R.EndEdit();
            }
        }

        private void buttonSetParentUnitID_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                System.Data.DataTable dt = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Copy();
                System.Collections.Generic.Dictionary<int, string> ParentList = new Dictionary<int, string>();
                foreach (System.Data.DataRow R in dt.Rows)
                {
                    if (R["IdentificationUnitID"].ToString() != RV["IdentificationUnitID"].ToString() &&
                        R["IdentificationUnitID"].ToString() != RV["RelatedUnitID"].ToString())
                    {
                        DiversityCollection.HierarchyNode N = new HierarchyNode(R);
                        ParentList.Add(int.Parse(R["IdentificationUnitID"].ToString()), N.Text.Trim());
                    }
                }
                ParentList.Add(-1, "");
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(ParentList, "Parent", "Please select the parent from the list");
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.SelectedString.Length > 0 && f.SelectedValue.ToString() != "-1")
                {
                    RV["ParentUnitID"] = int.Parse(f.SelectedValue);
                    this._iMainForm.setSpecimen(); //.buildOverviewHierarchy();
                }
            }
            catch (System.Exception ex) { }
        }

        private void pictureBoxWithholdUnit_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("Datawithholding", "Enter the datawithholding reason", R["DatawithholdingReason"].ToString());
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                R["DatawithholdingReason"] = f.String;
                this.setUnitDatawithholding();
            }
        }

        private void setUnitDatawithholding()
        {
            try
            {
                if (this._Source.Current == null)
                    this.pictureBoxWithholdUnit.Visible = false;
                else
                {
                    this.pictureBoxWithholdUnit.Visible = true;
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    if (R["DataWithholdingReason"].Equals(System.DBNull.Value) || R["DataWithholdingReason"].ToString().Trim().Length == 0)
                    {
                        this.pictureBoxWithholdUnit.Image = this.imageListDataWithholding.Images[1];
                        this.toolTip.SetToolTip(this.pictureBoxWithholdUnit, "Set the withholding reason for this organism");
                    }
                    else
                    {
                        this.pictureBoxWithholdUnit.Image = this.imageListDataWithholding.Images[0];
                        this.toolTip.SetToolTip(this.pictureBoxWithholdUnit, "Withholding reason: " + R["DataWithholdingReason"].ToString());
                    }
                }
            }
            catch (System.Exception ex)
            { }
        }

        private void comboBoxUnitRetrievalType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.pictureBoxUnitRetrievalType.Image = DiversityCollection.Specimen.ImageForRetrievalType(this.comboBoxUnitRetrievalType.SelectedValue.ToString());
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonRestrictImagesToCurrentUnit_Click(object sender, EventArgs e)
        {
            int UnitID;
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            if (int.TryParse(R["IdentificationUnitID"].ToString(), out UnitID))
            {
                this._iMainForm.RestrictImagesToUnit(UnitID);
                this.buttonRestrictImagesToCurrentUnit.BackColor = System.Drawing.Color.Red;
            }

            //// Reset the property filter
            //this.toolStripTextBoxImagePropertyFilter.Text = "";

            //System.Data.DataRowView RImage = (System.Data.DataRowView)this.collectionSpecimenImageBindingSource.Current;
            //if (RImage != null)
            //{
            //    try
            //    {
            //        RImage.BeginEdit();
            //        RImage.EndEdit();
            //    }
            //    catch { }
            //}
            //this.FormFunctions.updateTable(this.dataSetCollectionSpecimen, "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this.collectionSpecimenImageBindingSource);

            //this.listBoxSpecimenImage.Items.Clear();
            //this.userControlImageSpecimenImage.ImagePath = "";
            //this.dataSetCollectionSpecimen.CollectionSpecimenImage.Clear();
            //System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            //int UnitID;
            //if (int.TryParse(R["IdentificationUnitID"].ToString(), out UnitID))
            //{
            //    string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString() +
            //        " AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable) " +
            //        " AND IdentificationUnitID = " + UnitID.ToString();
            //    this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this.dataSetCollectionSpecimen.CollectionSpecimenImage);
            //    this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this.dataSetCollectionSpecimen.CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
            //    this.toolStripButtonImagesSpecimenShowAll.Visible = true;
            //    this.buttonRestrictImagesToCurrentUnit.BackColor = System.Drawing.Color.Red;
            //}
        }

        private void buttonEditOrderAndFamily_Click(object sender, EventArgs e)
        {
            this.textBoxFamilyCache.ReadOnly = false;
            this.textBoxOrderCache.ReadOnly = false;
        }

        private void comboBoxUnitDescription_DropDown(object sender, EventArgs e)
        {
            if (this._Source != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (!R["TaxonomicGroup"].Equals(System.DBNull.Value))
                {
                    string TaxonomicGroup = R["TaxonomicGroup"].ToString();
                    string SQL = "SELECT DISTINCT UnitDescription FROM IdentificationUnit WHERE TaxonomicGroup = '" + TaxonomicGroup + "' AND UnitDescription <> '' AND NOT UnitDescription IS NULL";
                    if (this.comboBoxUnitDescription.Text.Length > 0) SQL += " AND UnitDescription Like '" + this.comboBoxUnitDescription.Text + "%'";
                    SQL += " ORDER BY UnitDescription";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Collections.Generic.KeyValuePair<string, int> KV in DiversityCollection.Specimen.UnitPartList(TaxonomicGroup))
                    {
                        if (this.comboBoxUnitDescription.Text.Length > 0 && !KV.Key.StartsWith(this.comboBoxUnitDescription.Text))
                            continue;
                        System.Data.DataRow[] rr = dt.Select("UnitDescription ='" + KV.Key + "'");
                        if (rr.Length == 0)
                        {
                            System.Data.DataRow RN = dt.NewRow();
                            RN["UnitDescription"] = KV.Key;
                            dt.Rows.Add(RN);
                        }
                    }
                    this.comboBoxUnitDescription.DataSource = dt;
                    this.comboBoxUnitDescription.DisplayMember = "UnitDescription";
                    this.comboBoxUnitDescription.ValueMember = "UnitDescription";
                }
            }
        }

        private void comboBoxTaxonomicGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //#134
            if (this.comboBoxTaxonomicGroup.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxTaxonomicGroup.SelectedItem;
                string Code = R["Code"].ToString();
                this.setIdentificationTermControls(Code);
                this._iIdentification.setTaxonomicGroup(Code);
            }
        }

        private void textBoxIdentificationUnitID_ReadOnlyChanged(object sender, EventArgs e)
        {
            if (!textBoxIdentificationUnitID.ReadOnly)
                textBoxIdentificationUnitID.ReadOnly = true;
        }

        private void comboBoxSubstrateRelationType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonSetParentUnitID.Enabled = false;
            if (this._Source.Current != null)
            {
                System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                if (RV["RelationType"].ToString() == "Child of" || comboBoxSubstrateRelationType.SelectedValue.ToString() == "Child of")
                    this.buttonSetParentUnitID.Enabled = true;
            }
        }

        private void comboBoxGender_DropDown(object sender, EventArgs e)
        {
            if (this._Source != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (!R["TaxonomicGroup"].Equals(System.DBNull.Value))
                {
                    string TaxonomicGroup = R["TaxonomicGroup"].ToString();
                    string SQL = "SELECT DISTINCT Gender FROM IdentificationUnit WHERE TaxonomicGroup = '" + TaxonomicGroup + "' AND Gender <> '' AND NOT Gender IS NULL ORDER BY Gender";
                    if (this._dtGender == null) this._dtGender = new DataTable();
                    this._dtGender.Clear();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._dtGender);
                    this.comboBoxGender.DataSource = this._dtGender;
                    this.comboBoxGender.DisplayMember = "Gender";
                    this.comboBoxGender.ValueMember = "Gender";
                }
            }
        }

        private void comboBoxLifeStage_DropDown(object sender, EventArgs e)
        {
            if (this._Source != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (!R["TaxonomicGroup"].Equals(System.DBNull.Value))
                {
                    string TaxonomicGroup = R["TaxonomicGroup"].ToString();
                    string SQL = "SELECT NULL AS LifeStage UNION SELECT DISTINCT LifeStage FROM IdentificationUnit WHERE TaxonomicGroup = '" + TaxonomicGroup + "' AND LifeStage <> '' AND NOT LifeStage IS NULL ORDER BY LifeStage";
                    if (this._dtLifeStage == null) this._dtLifeStage = new DataTable();
                    this._dtLifeStage.Clear();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(this._dtLifeStage);
                    this.comboBoxLifeStage.DataSource = this._dtLifeStage;
                    this.comboBoxLifeStage.DisplayMember = "LifeStage";
                    this.comboBoxLifeStage.ValueMember = "LifeStage";
                }
            }
        }

        private void textBoxNumberOfUnits_Validating(object sender, CancelEventArgs e)
        {
            int i = 0;
            if (this.textBoxNumberOfUnits.Text.Length > 0 && !int.TryParse(this.textBoxNumberOfUnits.Text, out i))
                System.Windows.Forms.MessageBox.Show("only numbers are allowed here");
            else if (this.textBoxNumberOfUnits.Text.Length == 0)
            {
                if (this._Source != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    R.BeginEdit();
                    R["NumberOfUnits"] = System.DBNull.Value;
                    R.EndEdit();
                }
            }
        }

        #region DisplayOrder

        public void setDisplayOrderTitle(string Title, bool AddToCurrentTitle = false)
        {
            if (AddToCurrentTitle)
                this.groupBoxDisplayOrder.Text += Title;
            else
                this.groupBoxDisplayOrder.Text = Title;
        }
        
        private void setDatabindingsDisplayOrder()
        {
            try
            {
                if (this._dvUnitsDisplay == null)
                    this._dvUnitsDisplay = new System.Data.DataView(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit, "DisplayOrder > 0", "DisplayOrder", System.Data.DataViewRowState.CurrentRows);
                this.listBoxDisplayOrderListShow.DataSource = this._dvUnitsDisplay;
                this.listBoxDisplayOrderListShow.DisplayMember = "LastIdentificationCache";
                if (this._dvUnitsHide == null)
                    this._dvUnitsHide = new System.Data.DataView(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit, "DisplayOrder = 0", "LastIdentificationCache", System.Data.DataViewRowState.CurrentRows);
                this.listBoxDisplayOrderListHide.DataSource = this._dvUnitsHide;
                this.listBoxDisplayOrderListHide.DisplayMember = "LastIdentificationCache";

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonDisplayOrderUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxDisplayOrderListShow.SelectedIndex > 0)
                {
                    System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxDisplayOrderListShow.SelectedItem;
                    int UnitID = System.Int32.Parse(rv["IdentificationUnitID"].ToString());
                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("IdentificationUnitID = " + UnitID.ToString());
                    System.Data.DataRow r = rr[0];
                    int DisplayOrderLower = System.Int32.Parse(r["DisplayOrder"].ToString());
                    System.Data.DataRow[] rrU = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("DisplayOrder < " + DisplayOrderLower.ToString(), "DisplayOrder DESC");
                    if (rrU.Length > 0)
                    {
                        System.Data.DataRow rU = rrU[0];
                        int DisplayOrderUpper = System.Int32.Parse(rU["DisplayOrder"].ToString());
                        System.Data.DataRow[] rrDU = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("DisplayOrder = " + DisplayOrderUpper.ToString());
                        System.Data.DataRow rDU = rrDU[0];
                        r["DisplayOrder"] = DisplayOrderUpper;
                        rDU["DisplayOrder"] = DisplayOrderLower;
                    }
                }
                else
                {
                    if (this.listBoxDisplayOrderListShow.SelectedIndex == 0)
                    {
                        System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxDisplayOrderListShow.SelectedItem;
                        int UnitID = System.Int32.Parse(rv["IdentificationUnitID"].ToString());
                        System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("IdentificationUnitID = " + UnitID.ToString());
                        System.Data.DataRow r = rr[0];
                        int DisplayOrder = System.Int32.Parse(r["DisplayOrder"].ToString());
                        if (DisplayOrder != 1)
                            r["DisplayOrder"] = 1;
                    }
                }
                this.corrDisplayOrder();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonDisplayOrderDown_Click(object sender, EventArgs e)
        {
            if (this.listBoxDisplayOrderListShow.SelectedIndex < this.listBoxDisplayOrderListShow.Items.Count - 1)
            {
                try
                {
                    // the selected item to move down resp. get a higher display order
                    System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxDisplayOrderListShow.SelectedItem;
                    int UnitID = System.Int32.Parse(rv["IdentificationUnitID"].ToString());
                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("IdentificationUnitID = " + UnitID.ToString());
                    System.Data.DataRow r = rr[0];
                    int DisplayOrderLower = System.Int32.Parse(r["DisplayOrder"].ToString());

                    System.Data.DataRow[] rrU = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("DisplayOrder > " + DisplayOrderLower.ToString(), "DisplayOrder");
                    System.Data.DataRow rU = rrU[0];
                    int DisplayOrderUpper = System.Int32.Parse(rU["DisplayOrder"].ToString());

                    //				int DisplayOrderUpper = DisplayOrderLower + 1;
                    // the item to change place with
                    System.Data.DataRow[] rrDU = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("DisplayOrder = " + DisplayOrderUpper.ToString());
                    System.Data.DataRow rDU = rrDU[0];
                    r["DisplayOrder"] = DisplayOrderUpper;
                    rDU["DisplayOrder"] = DisplayOrderLower;

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            this.corrDisplayOrder();
        }

        private void corrDisplayOrder()
        {
            try
            {
                System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder");
                System.Data.DataRow r = rr[0];
                int DisplayOrder = System.Int32.Parse(r["DisplayOrder"].ToString());
                if (DisplayOrder != 1)
                {
                    r["DisplayOrder"] = 1;
                    this.corrDisplayOrder(1);
                }
            }
            catch { }
            try
            {
                System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("DisplayOrder > 0", "DisplayOrder DESC");
                System.Data.DataRow r = rr[0];
                int DisplayOrder = System.Int32.Parse(r["DisplayOrder"].ToString());
                if (DisplayOrder != rr.Length)
                {
                    int DO = rr.Length;
                    foreach (System.Data.DataRow rCorr in rr)
                    {
                        rCorr["DisplayOrder"] = DO;
                        DO--;
                    }
                }
            }
            catch { }
        }

        private void corrDisplayOrder(int DisplayOrder)
        {
            try
            {
                System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("DisplayOrder > " + DisplayOrder.ToString(), "DisplayOrder");
                if (rr.Length > 0)
                {
                    System.Data.DataRow r = rr[0];
                    int DO = System.Int32.Parse(r["DisplayOrder"].ToString());
                    if (DO != DisplayOrder + 1)
                    {
                        DO = DisplayOrder + 1;
                        r["DisplayOrder"] = DO;
                    }
                    System.Data.DataRow[] rrRest = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("DisplayOrder > " + DO.ToString());
                    if (rrRest.Length > 0)
                    {
                        this.corrDisplayOrder(DO);
                    }
                }
            }
            catch { }
        }

        private void SortByDisplayOrder(string Target)
        {
            System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("DisplayOrder > 0", Target);
            for (int i = 0; i < RR.Length; i++)
            {
                RR[i]["DisplayOrder"] = i + 1;
            }
            string Restriction = "IdentificationUnitID = " + RR[0]["IdentificationUnitID"].ToString();
            this._iMainForm.setSpecimen();//.setSpecimen(this.ID);

            System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select(Restriction);
            this._iMainForm.SelectNode(rr[0], Forms.FormCollectionSpecimen.Tree.UnitTree);
        }

        private void toolStripMenuItemDisplayOrderSortByName_Click(object sender, EventArgs e)
        {
            this.SortByDisplayOrder("LastIdentificationCache");
        }

        private void toolStripMenuItemDisplayOrderSortByID_Click(object sender, EventArgs e)
        {
            this.SortByDisplayOrder("IdentificationUnitID");
        }

        private void toolStripMenuItemDisplayOrderSortByIdentifier_Click(object sender, EventArgs e)
        {
            this.SortByDisplayOrder("UnitIdentifier");
        }

        private void toolStripButtonDisplayShow_Click(object sender, EventArgs e)
        {
            if (this.listBoxDisplayOrderListHide.SelectedIndex > -1)
            {
                try
                {
                    System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxDisplayOrderListHide.SelectedItem;
                    int UnitID = System.Int32.Parse(rv["IdentificationUnitID"].ToString());
                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("IdentificationUnitID = " + UnitID.ToString());
                    System.Data.DataRow r = rr[0];
                    System.Data.DataRow[] rrDO = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("", "DisplayOrder DESC");
                    System.Data.DataRow rDO = rrDO[0];
                    int DisplayOrder = System.Int32.Parse(rDO["DisplayOrder"].ToString()) + 1;
                    r["DisplayOrder"] = DisplayOrder;
                    this.corrDisplayOrder(0);
                    System.Data.DataRow Row = (System.Data.DataRow)this._iMainForm.SelectedUnitHierarchyNode().Tag;
                    string Restriction = "IdentificationUnitID = " + Row["IdentificationUnitID"].ToString();
                    this._iMainForm.setSpecimen();
                    System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select(Restriction);
                    this._iMainForm.SelectNode(RR[0], Forms.FormCollectionSpecimen.Tree.UnitTree);
                    //System.Collections.Generic.List<System.Windows.Forms.TreeNode> UnitNodes = new List<TreeNode>();
                    //this.getOverviewHierarchyNodes(null, "IdentificationUnit", this.treeViewOverviewHierarchy, ref UnitNodes);
                    //foreach (System.Windows.Forms.TreeNode N in UnitNodes)
                    //{
                    //    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                    //    int IdentificationUnitID = int.Parse(R["IdentificationUnitID"].ToString());
                    //    if (UnitID == IdentificationUnitID)
                    //    {
                    //        int i = N.ImageIndex;
                    //        N.ImageIndex = i - 1;
                    //        N.SelectedImageIndex = i - 1;
                    //        break;
                    //    }
                    //}
                }
                catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            }
        }

        private void toolStripButtonDisplayHide_Click(object sender, EventArgs e)
        {
            if (this.listBoxDisplayOrderListShow.SelectedIndex > -1)
            {
                try
                {
                    System.Data.DataRowView rv = (System.Data.DataRowView)this.listBoxDisplayOrderListShow.SelectedItem;
                    if (/*rv["DisplayOrder"].ToString() == "1" ||*/ this.listBoxDisplayOrderListShow.SelectedIndex == 0)
                    {
                        System.Windows.Forms.MessageBox.Show("The main unit can not be hidden from the label");
                        return;
                    }
                    int UnitID = System.Int32.Parse(rv["IdentificationUnitID"].ToString());
                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("IdentificationUnitID = " + UnitID.ToString());
                    System.Data.DataRow r = rr[0];
                    r["DisplayOrder"] = 0;
                    this.corrDisplayOrder(0);
                    System.Data.DataRow Row = (System.Data.DataRow)this._iMainForm.SelectedUnitHierarchyNode().Tag;
                    string Restriction = "IdentificationUnitID = " + Row["IdentificationUnitID"].ToString();
                    this._iMainForm.setSpecimen();
                    System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select(Restriction);
                    this._iMainForm.SelectNode(RR[0], Forms.FormCollectionSpecimen.Tree.UnitTree);
                    //System.Collections.Generic.List<System.Windows.Forms.TreeNode> UnitNodes = new List<TreeNode>();
                    //this.getOverviewHierarchyNodes(null, "IdentificationUnit", this.treeViewOverviewHierarchy, ref UnitNodes);
                    //foreach (System.Windows.Forms.TreeNode N in UnitNodes)
                    //{
                    //    System.Data.DataRow R = (System.Data.DataRow)N.Tag;
                    //    int IdentificationUnitID = int.Parse(R["IdentificationUnitID"].ToString());
                    //    if (UnitID == IdentificationUnitID)
                    //    {
                    //        int i = N.ImageIndex;
                    //        N.ImageIndex = i + 1;
                    //        N.SelectedImageIndex = i + 1;
                    //        break;
                    //    }
                    //}
                }
                catch { }
            }
        }
        
        #endregion

        private void setIdentificationTermControls(string TaxonomicGroup)
        {
            //System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxTaxonomicGroup.SelectedItem;
            //string Code = R["Code"].ToString();
            bool IsTaxonomyRelatedTaxonomicGroup = false;
            if (DiversityWorkbench.CollectionSpecimen.TaxonomyRelatedTaxonomicGroups.Contains(TaxonomicGroup))
            {
                IsTaxonomyRelatedTaxonomicGroup = true;
            }

            this.setIdentificationTermControls(IsTaxonomyRelatedTaxonomicGroup);
            this._iIdentification.setIdentificationTermControls(IsTaxonomyRelatedTaxonomicGroup);
        }

        #region Interface

        public void setIdentificationTermControls(bool IsTaxonomyRelatedTaxonomicGroup)
        {
            this.labelCircumstances.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.comboBoxCircumstances.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.userControlHierarchySelectorCircumstances.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelFamilyCache.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.textBoxFamilyCache.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelOrderCache.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.textBoxOrderCache.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.buttonEditOrderAndFamily.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.buttonSetParentUnitID.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelIdentificationUnitID.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelGender.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.comboBoxGender.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelLifeStage.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.comboBoxLifeStage.Visible = IsTaxonomyRelatedTaxonomicGroup;

            this.labelSubstrateRelationType.Visible = IsTaxonomyRelatedTaxonomicGroup;
            this.comboBoxSubstrateRelationType.Visible = IsTaxonomyRelatedTaxonomicGroup;

            bool IsGrey = false;
            string TaxonomicGroup = "plant";
            if (this._Source.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                try
                {
                    TaxonomicGroup = R["TaxonomicGroup"].ToString();
                    bool.TryParse(R["OnlyObserved"].ToString(), out IsGrey);
                    if (!IsGrey)
                    {
                        int Order = 1;
                        if (int.TryParse(R["DisplayOrder"].ToString(), out Order) && Order == 0)
                            IsGrey = true;
                    }
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            this.pictureBoxUnit.Image = DiversityCollection.Specimen.TaxonImage(IsGrey, TaxonomicGroup);

        }
        
        #endregion

        #region Showing parts of the control

        public void ShowUnit(bool Show)
        {
            if (Show)
                this.splitContainerMain.Panel1Collapsed = false;
            else
                this.splitContainerMain.Panel1Collapsed = true;
        }
        
        public void ShowExsiccataAndDisplayOrder(bool Show)
        {
            if (Show)
            {
                this.splitContainerMain.Panel2Collapsed = false;
                this.tableLayoutPanelExsiccataUnit.Visible = true;
            }
            else
            {
                this.splitContainerMain.Panel2Collapsed = true;
                this.tableLayoutPanelExsiccataUnit.Visible = false;
            }
        }

        public void ShowExsiccata(bool Show)
        {
            if (Show)
                this.tableLayoutPanelExsiccataUnit.Visible = true;
            else
                this.tableLayoutPanelExsiccataUnit.Visible = false;
        }
        
        #endregion

        #region Template
        
        private void buttonTemplateUnitEdit_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = ((System.Data.DataRowView)this._Source.Current).Row;
            DiversityWorkbench.Forms.FormTemplateEditor f = new DiversityWorkbench.Forms.FormTemplateEditor("IdentificationUnit", R, this.TemplateUnitSuppressedColumns);
            f.setHelp("Template");
            f.ShowDialog();
        }

        private void buttonTemplateUnitSet_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.TemplateForData T = new DiversityWorkbench.TemplateForData("IdentificationUnit", TemplateUnitSuppressedColumns);
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            T.CopyTemplateToRow(R.Row);
        }

        private System.Collections.Generic.List<string> TemplateUnitSuppressedColumns
        {
            get
            {
                System.Collections.Generic.List<string> Suppress = new List<string>();
                Suppress.Add("CollectionSpecimenID");
                Suppress.Add("IdentificationUnitID");
                Suppress.Add("LastIdentificationCache");
                Suppress.Add("RelatedUnitID");
                Suppress.Add("DisplayOrder");
                Suppress.Add("ParentUnitID");
                Suppress.Add("LogCreatedWhen");
                Suppress.Add("LogCreatedBy");
                Suppress.Add("LogUpdatedWhen");
                Suppress.Add("LogUpdatedBy");
                Suppress.Add("RowGUID");
                return Suppress;
            }
        }
        
        #endregion
    }
}
