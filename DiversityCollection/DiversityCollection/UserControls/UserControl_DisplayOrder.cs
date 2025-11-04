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
    public partial class UserControl_DisplayOrder : UserControl__Data
    {

        #region Parameter

        private System.Windows.Forms.BindingSource _NotInPartListBindingSource;
        private System.Windows.Forms.BindingSource _InPartHideListBindingSource;
        private System.Windows.Forms.BindingSource _InPartDisplayListBindingSource;
        
        #endregion

        #region Construction

        public UserControl_DisplayOrder(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource IdentificationUnitInPartSource,
            System.Windows.Forms.BindingSource NotInPartListBindingSource,
            System.Windows.Forms.BindingSource InPartHideListBindingSource,
            System.Windows.Forms.BindingSource InPartDisplayListBindingSource,
            string HelpNamespace)
            : base(MainForm, IdentificationUnitInPartSource, HelpNamespace)
        {
            InitializeComponent();
            this._Source = IdentificationUnitInPartSource;
            this._NotInPartListBindingSource = NotInPartListBindingSource;
            this._InPartDisplayListBindingSource = InPartDisplayListBindingSource;
            this._InPartHideListBindingSource = InPartHideListBindingSource;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
        }
        
        #endregion

        #region Control

        private void initControl()
        {
            this.listBoxUnitsNotInPart.DataSource = this._NotInPartListBindingSource;
            this.listBoxUnitsNotInPart.DisplayMember = "DisplayText";

            this.listBoxPartHide.DataSource = this._InPartHideListBindingSource;
            this.listBoxPartHide.DisplayMember = "DisplayText";

            this.listBoxPartShowInLabel.DataSource = this._InPartDisplayListBindingSource;
            this.listBoxPartShowInLabel.DisplayMember = "DisplayText";

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        public void SetTitle(string Title)
        {
            this.groupBoxDisplayOrderPart.Text = Title;
        }

        #region Tool strip

        private void toolStripButtonShowUnitInPartLabel_Click(object sender, EventArgs e)
        {
            if (this.listBoxPartHide.SelectedIndex == -1)
            {
                string Message = DiversityCollection.Forms.FormCollectionSpecimenText.Please_select;
                Message += ": " + DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation("IdentificationUnit"));
                Message = "Nothing selected";
                System.Windows.Forms.MessageBox.Show(Message);
                return;
            }
            try
            {
                // getting the row that should be shown
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPartHide.SelectedItem;

                string Restriction = "SpecimenPartID = " + R["SpecimenPartID"].ToString();

                // getting the next display order
                int DisplayOrder = 0;
                System.Data.DataRow[] rr1 = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select("DisplayOrder = 1", "DisplayOrder DESC");
                if (rr1.Length == 0)
                    DisplayOrder = 1;
                else
                {
                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select("NOT DisplayOrder IS NULL AND DisplayOrder <> 0", "DisplayOrder DESC");
                    if (rr.Length > 0)
                        DisplayOrder = int.Parse(rr[0]["DisplayOrder"].ToString()) + 1;
                    else
                        DisplayOrder = 1;
                }

                // getting the dataset and the tree node
                System.Data.DataRow[] RU = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select(
                    "CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() +
                    " AND IdentificationUnitID = " + R["IdentificationUnitID"].ToString() +
                    " AND SpecimenPartID = " + R["SpecimenPartID"].ToString());
                if (RU.Length == 0) return;

                // setting the display order
                RU[0].BeginEdit();
                RU[0]["DisplayOrder"] = DisplayOrder;
                RU[0].EndEdit();

                // moving the list items and update the node image
                DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartDisplayListRow Rshow = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.NewIdentificationUnitInPartDisplayListRow();
                Rshow["CollectionSpecimenID"] = R["CollectionSpecimenID"];
                Rshow["IdentificationUnitID"] = R["IdentificationUnitID"];
                Rshow["SpecimenPartID"] = R["SpecimenPartID"];
                Rshow["DisplayOrder"] = DisplayOrder;
                Rshow[4] = R[4];
                this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Rows.Add(Rshow);
                R.Row.Delete();
                //System.Windows.Forms.TreeNode Thide = this.getTreeNodeOfDataRow(this.treeViewOverviewHierarchyStorage, null, RU[0]);
                //Thide.ImageIndex--;

                this._iMainForm.setSpecimen();
                System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Select(Restriction);
                this._iMainForm.SelectNode(RR[0], Forms.FormCollectionSpecimen.Tree.PartTree);

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void toolStripButtonHideUnitFromPartLabel_Click(object sender, EventArgs e)
        {
            if (this.listBoxPartShowInLabel.SelectedIndex == -1)
            {
                string Message = DiversityCollection.Forms.FormCollectionSpecimenText.Please_select;
                Message += ": " + DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation("IdentificationUnit"));
                Message = "Nothing selected";
                System.Windows.Forms.MessageBox.Show(Message);
                return;
            }
            try
            {
                // getting the row that should be hidden
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPartShowInLabel.SelectedItem;
                if (R["DisplayOrder"].ToString() == "1" || this.listBoxPartShowInLabel.SelectedIndex == 0)
                {
                    System.Windows.Forms.MessageBox.Show("The main unit can not be hidden from the label");
                    return;
                }

                string Restriction = "SpecimenPartID = " + R["SpecimenPartID"].ToString();

                // getting the row and the tree node
                System.Data.DataRow[] RU = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select(
                    "CollectionSpecimenID = " + R["CollectionSpecimenID"].ToString() +
                    " AND IdentificationUnitID = " + R["IdentificationUnitID"].ToString() +
                    " AND SpecimenPartID = " + R["SpecimenPartID"].ToString());
                if (RU.Length == 0) return;

                // setting the display order to 0
                RU[0]["DisplayOrder"] = 0;

                // moving the list items and update the node image
                DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartHideListRow Rhide = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartHideList.NewIdentificationUnitInPartHideListRow();
                Rhide["CollectionSpecimenID"] = R["CollectionSpecimenID"];
                Rhide["IdentificationUnitID"] = R["IdentificationUnitID"];
                Rhide["SpecimenPartID"] = R["SpecimenPartID"];
                Rhide["DisplayOrder"] = 0;
                Rhide[4] = R[4];
                this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartHideList.Rows.Add(Rhide);
                R.Row.Delete();
                //System.Windows.Forms.TreeNode Thide = this.getTreeNodeOfDataRow(this.treeViewOverviewHierarchyStorage, null, RU[0]);
                //Thide.ImageIndex++;

                this._iMainForm.setSpecimen();
                System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Select(Restriction);
                this._iMainForm.SelectNode(RR[0], Forms.FormCollectionSpecimen.Tree.PartTree);

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        public void fillPartDisplayLists(System.Windows.Forms.TreeNode Node)
        {
            //if (Node == null) return;
            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Clear();
            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartHideList.Clear();
            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.Clear();
            System.Collections.Generic.List<System.Data.DataRow> RowsInPart = new List<DataRow>();
            try
            {
                // filling the table IdentificationUnitNotInPartList with all units that are part of the specimen
                foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitRow RN
                    in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Rows)
                {
                    if (RN.RowState == DataRowState.Deleted)
                        continue;
                    if (RN.CollectionSpecimenID != this._iMainForm.ID_Specimen())//.SpecimenID)
                        continue;
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitNotInPartListRow Rnew
                        = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.NewIdentificationUnitNotInPartListRow();
                    Rnew.CollectionSpecimenID = RN.CollectionSpecimenID;
                    Rnew.IdentificationUnitID = RN.IdentificationUnitID;
                    if (!RN.DisplayOrder.Equals(System.DBNull.Value))
                        Rnew.DisplayOrder = RN.DisplayOrder;
                    try
                    {
                        Rnew.DisplayText = DiversityCollection.HierarchyNode.DisplayText(RN);
                    }
                    catch
                    {
                        Rnew.DisplayText = RN.TaxonomicGroup;
                    }
                    this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.Rows.Add(Rnew);
                }
                if (Node != null)
                {
                    System.Data.DataRow DataRow = (System.Data.DataRow)Node.Tag;
                    int SpecimenPartID = 0;
                    // transfer the units to the other tables
                    if (DataRow.RowState != DataRowState.Deleted && DataRow.RowState != DataRowState.Detached && int.TryParse(DataRow["SpecimenPartID"].ToString(), out SpecimenPartID))
                    {
                        foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitNotInPartListRow RU
                            in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.Rows)
                        {
                            foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow RUIP
                                in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Rows)
                            {
                                if (RUIP.RowState != DataRowState.Deleted && RU.RowState != DataRowState.Deleted)
                                {
                                    // if a row in IdentificationUnitNotInPartList belongs to a row in IdentificationUnitInPart transfer it
                                    if (RUIP.CollectionSpecimenID.ToString() == RU.CollectionSpecimenID.ToString() &&
                                        RUIP.IdentificationUnitID.ToString() == RU.IdentificationUnitID.ToString() &&
                                        RUIP.SpecimenPartID.ToString() == SpecimenPartID.ToString())
                                    {
                                        // if the display order = 0 transfer it to the hidden units
                                        if (RUIP.DisplayOrder.Equals(System.DBNull.Value) ||
                                            RUIP.DisplayOrder.ToString() == "0")
                                        {
                                            DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartHideListRow RH
                                                = (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartHideListRow)this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartHideList.NewRow();
                                            RH.CollectionSpecimenID = RUIP.CollectionSpecimenID;
                                            RH.IdentificationUnitID = RUIP.IdentificationUnitID;
                                            RH.SpecimenPartID = RUIP.SpecimenPartID;
                                            RH.DisplayOrder = RUIP.DisplayOrder;
                                            RH.DisplayText = RU.DisplayText;
                                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartHideList.Rows.Add(RH);
                                        }
                                        else // transfer it to the shown units
                                        {
                                            DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartDisplayListRow RD
                                                = (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartDisplayListRow)this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.NewRow();
                                            RD.CollectionSpecimenID = RUIP.CollectionSpecimenID;
                                            RD.IdentificationUnitID = RUIP.IdentificationUnitID;
                                            RD.SpecimenPartID = RUIP.SpecimenPartID;
                                            RD.DisplayOrder = RUIP.DisplayOrder;
                                            RD.DisplayText = RU.DisplayText;
                                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Rows.Add(RD);
                                        }
                                        RowsInPart.Add(RU);
                                    }
                                }
                            }
                        }
                        foreach (System.Data.DataRow R in RowsInPart)
                        {
                            R.Delete();
                        }
                    }
                    // sorting the rows in IdentificationUnitInPartDisplayList
                    System.Data.DataTable dt = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Copy();
                    this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Clear();
                    System.Data.DataRow[] RR = dt.Select("", "DisplayOrder");
                    short i = 1;
                    foreach (System.Data.DataRow R in RR)
                    {
                        DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartDisplayListRow RD
                            = (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartDisplayListRow)this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.NewRow();
                        RD.CollectionSpecimenID = int.Parse(R["CollectionSpecimenID"].ToString());
                        RD.IdentificationUnitID = int.Parse(R["IdentificationUnitID"].ToString());
                        RD.SpecimenPartID = int.Parse(R["SpecimenPartID"].ToString());
                        R["DisplayOrder"] = i;
                        RD.DisplayOrder = i; // short.Parse(R["DisplayOrder"].ToString());
                        RD.DisplayText = R["DisplayText"].ToString();
                        this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Rows.Add(RD);
                        string Restriction = "CollectionSpecimenID = " + RD.CollectionSpecimenID.ToString() +
                            " AND SpecimenPartID = " + RD.SpecimenPartID.ToString() +
                            " AND IdentificationUnitID = " + RD.IdentificationUnitID.ToString();
                        System.Data.DataRow[] RIUIP =
                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select(Restriction);
                        if (RIUIP[0]["DisplayOrder"].ToString() != i.ToString())
                            RIUIP[0]["DisplayOrder"] = i;
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void fillPartDisplayLists(System.Data.DataRowView RowView)
        {
            if (RowView == null) 
                return;
            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Clear();
            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartHideList.Clear();
            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.Clear();
            System.Collections.Generic.List<System.Data.DataRow> RowsInPart = new List<DataRow>();
            try
            {
                // filling the table IdentificationUnitNotInPartList with all units that are part of the specimen
                foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitRow RN
                    in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Rows)
                {
                    if (RN.CollectionSpecimenID != this._iMainForm.ID_Specimen())//.SpecimenID)
                        continue;
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitNotInPartListRow Rnew
                        = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.NewIdentificationUnitNotInPartListRow();
                    Rnew.CollectionSpecimenID = RN.CollectionSpecimenID;
                    Rnew.IdentificationUnitID = RN.IdentificationUnitID;
                    if (!RN.DisplayOrder.Equals(System.DBNull.Value))
                        Rnew.DisplayOrder = RN.DisplayOrder;
                    try
                    {
                        Rnew.DisplayText = DiversityCollection.HierarchyNode.DisplayText(RN);
                    }
                    catch
                    {
                        Rnew.DisplayText = RN.TaxonomicGroup;
                    }
                    this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.Rows.Add(Rnew);
                }
                System.Data.DataRow DataRow = RowView.Row;
                int SpecimenPartID = 0;
                // transfer the units to the other tables
                if (DataRow.RowState != DataRowState.Deleted && DataRow.RowState != DataRowState.Detached && int.TryParse(DataRow["SpecimenPartID"].ToString(), out SpecimenPartID))
                {
                    foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitNotInPartListRow RU
                        in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.Rows)
                    {
                        foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow RUIP
                            in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Rows)
                        {
                            if (RUIP.RowState != DataRowState.Deleted && RU.RowState != DataRowState.Deleted)
                            {
                                // if a row in IdentificationUnitNotInPartList belongs to a row in IdentificationUnitInPart transfer it
                                if (RUIP.CollectionSpecimenID.ToString() == RU.CollectionSpecimenID.ToString() &&
                                    RUIP.IdentificationUnitID.ToString() == RU.IdentificationUnitID.ToString() &&
                                    RUIP.SpecimenPartID.ToString() == SpecimenPartID.ToString())
                                {
                                    // if the display order = 0 transfer it to the hidden units
                                    if (RUIP.DisplayOrder.Equals(System.DBNull.Value) ||
                                        RUIP.DisplayOrder.ToString() == "0")
                                    {
                                        DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartHideListRow RH
                                            = (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartHideListRow)this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartHideList.NewRow();
                                        RH.CollectionSpecimenID = RUIP.CollectionSpecimenID;
                                        RH.IdentificationUnitID = RUIP.IdentificationUnitID;
                                        RH.SpecimenPartID = RUIP.SpecimenPartID;
                                        RH.DisplayOrder = RUIP.DisplayOrder;
                                        RH.DisplayText = RU.DisplayText;
                                        this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartHideList.Rows.Add(RH);
                                    }
                                    else // transfer it to the shown units
                                    {
                                        DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartDisplayListRow RD
                                            = (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartDisplayListRow)this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.NewRow();
                                        RD.CollectionSpecimenID = RUIP.CollectionSpecimenID;
                                        RD.IdentificationUnitID = RUIP.IdentificationUnitID;
                                        RD.SpecimenPartID = RUIP.SpecimenPartID;
                                        RD.DisplayOrder = RUIP.DisplayOrder;
                                        RD.DisplayText = RU.DisplayText;
                                        this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Rows.Add(RD);
                                    }
                                    RowsInPart.Add(RU);
                                }
                            }
                        }
                    }
                    foreach (System.Data.DataRow R in RowsInPart)
                    {
                        R.Delete();
                    }
                    // sorting the rows in IdentificationUnitInPartDisplayList
                    System.Data.DataTable dt = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Copy();
                    this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Clear();
                    System.Data.DataRow[] RR = dt.Select("", "DisplayOrder");
                    short i = 1;
                    foreach (System.Data.DataRow R in RR)
                    {
                        DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartDisplayListRow RD
                            = (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartDisplayListRow)this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.NewRow();
                        RD.CollectionSpecimenID = int.Parse(R["CollectionSpecimenID"].ToString());
                        RD.IdentificationUnitID = int.Parse(R["IdentificationUnitID"].ToString());
                        RD.SpecimenPartID = int.Parse(R["SpecimenPartID"].ToString());
                        R["DisplayOrder"] = i;
                        RD.DisplayOrder = i; // short.Parse(R["DisplayOrder"].ToString());
                        RD.DisplayText = R["DisplayText"].ToString();
                        this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Rows.Add(RD);
                        string Restriction = "CollectionSpecimenID = " + RD.CollectionSpecimenID.ToString() +
                            " AND SpecimenPartID = " + RD.SpecimenPartID.ToString() +
                            " AND IdentificationUnitID = " + RD.IdentificationUnitID.ToString();
                        System.Data.DataRow[] RIUIP =
                            this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select(Restriction);
                        if (RIUIP[0]["DisplayOrder"].ToString() != i.ToString())
                            RIUIP[0]["DisplayOrder"] = i;
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonUnitRemoveFromPart_Click(object sender, EventArgs e)
        {
            if (this.listBoxPartShowInLabel.SelectedIndex == -1)
            {
                string Message = DiversityCollection.Forms.FormCollectionSpecimenText.Please_select;
                Message += ": " + DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation("IdentificationUnit"));
                System.Windows.Forms.MessageBox.Show(Message);
                return;
            }
            try
            {
                // getting the selected item
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPartShowInLabel.SelectedItem;
                string Restriction = "SpecimenPartID = " + R["SpecimenPartID"].ToString();
                string Table = "CollectionSpecimenPart";

                // getting the row that should be deleted
                System.Data.DataRowView RVdel = (System.Data.DataRowView)this.listBoxPartShowInLabel.SelectedItem;
                System.Data.DataRow[] RUdel = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select(
                    "CollectionSpecimenID = " + RVdel["CollectionSpecimenID"].ToString() +
                    " AND IdentificationUnitID = " + RVdel["IdentificationUnitID"].ToString() +
                    " AND SpecimenPartID = " + RVdel["SpecimenPartID"].ToString());
                System.Data.DataRow[] RUlistDel = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPartDisplayList.Select(
                    "CollectionSpecimenID = " + RVdel["CollectionSpecimenID"].ToString() +
                    " AND IdentificationUnitID = " + RVdel["IdentificationUnitID"].ToString() +
                    " AND SpecimenPartID = " + RVdel["SpecimenPartID"].ToString());
                System.Windows.Forms.TreeNode TNdel = this._iMainForm.SelectedPartHierarchyNode();// this.getTreeNodeOfDataRow(this.treeViewOverviewHierarchyStorage, null, RUdel[0]);
                if (RUdel.Length > 0 && RUlistDel.Length > 0 && TNdel != null)
                {
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitNotInPartListRow RUnot = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.NewIdentificationUnitNotInPartListRow();
                    RUnot["CollectionSpecimenID"] = RVdel["CollectionSpecimenID"];
                    RUnot["IdentificationUnitID"] = RVdel["IdentificationUnitID"];
                    RUnot["CollectionSpecimenID"] = RVdel["CollectionSpecimenID"];
                    int DisplayOrder = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.Rows.Count + 1;
                    string WhereClause = "CollectionSpecimenID = " + RVdel["CollectionSpecimenID"].ToString() + " AND IdentificationUnitID = " + RVdel["IdentificationUnitID"].ToString();
                    if (int.TryParse(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select(WhereClause)[0]["DisplayOrder"].ToString(), out DisplayOrder))
                        RUnot["DisplayOrder"] = DisplayOrder;
                    RUnot["DisplayText"] = RVdel[4].ToString();
                    this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitNotInPartList.Rows.Add(RUnot);
                    RUdel[0].Delete();
                    RUlistDel[0].Delete();
                    TNdel.Remove();
                }

                this._iMainForm.setSpecimen();
                System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().Tables[Table].Select(Restriction);
                this._iMainForm.SelectNode(RR[0], Forms.FormCollectionSpecimen.Tree.PartTree);

            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// Moving a unit into a part
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonUnitMoveInPart_Click(object sender, EventArgs e)
        {
            if (this.listBoxUnitsNotInPart.SelectedIndex == -1)
            {
                string Message = DiversityCollection.Forms.FormCollectionSpecimenText.Please_select;
                Message += ": " + DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation("IdentificationUnit"));
                System.Windows.Forms.MessageBox.Show(Message);
                return;
            }
            try
            {
                // getting the SpecimenPartID
                int SpecimenPartID = 0;
                if (this._iMainForm.SelectedPartHierarchyNode() != null)
                {
                    System.Data.DataRow Rselected = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                    if (Rselected.RowState != DataRowState.Deleted) SpecimenPartID = int.Parse(Rselected["SpecimenPartID"].ToString());
                    else
                    {
                        System.Data.DataRow RH = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                        SpecimenPartID = int.Parse(RH["SpecimenPartID"].ToString());
                    }
                }
                else
                {
                    // getting the selected row
                    if (this.listBoxPartShowInLabel.Items.Count > 0)
                    {
                        System.Data.DataRowView RVmainItem = (System.Data.DataRowView)this.listBoxPartShowInLabel.Items[0];
                        SpecimenPartID = int.Parse(RVmainItem["SpecimenPartID"].ToString());
                    }
                    else return;
                }

                // getting the next display order
                int DisplayOrder = 1;
                System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select("NOT DisplayOrder IS NULL", "DisplayOrder DESC");
                if (rr.Length > 0)
                    DisplayOrder = int.Parse(rr[0]["DisplayOrder"].ToString()) + 1;
                else
                    DisplayOrder = 1;

                // insert a new row in table IdentificationUnitInPart
                DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow RU =
                    (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow)this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.NewRow();
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxUnitsNotInPart.SelectedItem;
                RU.CollectionSpecimenID = int.Parse(R["CollectionSpecimenID"].ToString());
                RU.IdentificationUnitID = int.Parse(R["IdentificationUnitID"].ToString());
                RU.SpecimenPartID = SpecimenPartID;
                RU.DisplayOrder = (System.Int16)DisplayOrder;
                this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Rows.Add(RU);

                // requery the lists
                System.Data.DataRow Row = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                string Table = Row.Table.TableName;
                string Restriction = "SpecimenPartID = " + Row["SpecimenPartID"].ToString();
                if (Row.Table.Columns.Contains("IdentificationUnitID"))
                    Restriction += " AND IdentificationUnitID = " + Row["IdentificationUnitID"].ToString();
                this._iMainForm.setSpecimen();
                System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().Tables[Table].Select(Restriction);
                this._iMainForm.SelectNode(RR[0], Forms.FormCollectionSpecimen.Tree.PartTree);

                //this.fillPartDisplayLists(this._iMainForm.SelectedPartHierarchyNode());
                //try
                //{
                //    System.Windows.Forms.TreeNode Tsel = new TreeNode();
                //    if (this._iMainForm.SelectedPartHierarchyNode() != null)
                //    {
                //        Tsel = this._iMainForm.SelectedPartHierarchyNode();
                //    }
                //    else
                //    {
                //        System.Data.DataRow[] Rpart = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Select("SpecimenPartID = " + SpecimenPartID.ToString());
                //        if (Rpart.Length > 0)
                //        {
                //            Tsel = this._iMainForm.g this.getTreeNodeOfDataRow(this.treeViewOverviewHierarchyStorage, null, Rpart[0]);
                //        }
                //    }
                //    if (Tsel != null)
                //    {
                //        Tsel.Nodes.Clear();
                //        this.addOverviewHierarchyPartDependentData(Tsel);
                //        System.Collections.Generic.List<System.Windows.Forms.TreeNode> ChildNodes = new List<TreeNode>();
                //        this.getOverviewHierarchyNodes(Tsel, "IdentificationUnitInPart", this.treeViewOverviewHierarchyStorage, ref ChildNodes);
                //    }
                //}
                //catch { }
                //finally
                //{
                //}
            }
            catch { }
        }

        private void toolStripButtonUnitMoveInPartAll_Click(object sender, EventArgs e)
        {
            try
            {
                // getting the SpecimenPartID
                int SpecimenPartID = 0;
                if (this._iMainForm.SelectedPartHierarchyNode() != null)
                {
                    System.Data.DataRow Rselected = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                    if (Rselected.RowState != DataRowState.Deleted) SpecimenPartID = int.Parse(Rselected["SpecimenPartID"].ToString());
                    else
                    {
                        System.Data.DataRow RH = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                        SpecimenPartID = int.Parse(RH["SpecimenPartID"].ToString());
                    }
                }
                else
                {
                    // getting the selected row
                    if (this.listBoxPartShowInLabel.Items.Count > 0)
                    {
                        System.Data.DataRowView RVmainItem = (System.Data.DataRowView)this.listBoxPartShowInLabel.Items[0];
                        SpecimenPartID = int.Parse(RVmainItem["SpecimenPartID"].ToString());
                    }
                    else return;
                }

                // getting the next display order
                int DisplayOrder = 1;
                System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select("NOT DisplayOrder IS NULL", "DisplayOrder DESC");
                if (rr.Length > 0)
                    DisplayOrder = int.Parse(rr[0]["DisplayOrder"].ToString()) + 1;
                else
                    DisplayOrder = 1;

                // insert new rows in table IdentificationUnitInPart
                System.Collections.Generic.List<System.Data.DataRowView> RowsToInsert = new List<DataRowView>();
                foreach (System.Object O in this.listBoxUnitsNotInPart.Items)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)O;
                    RowsToInsert.Add(R);
                }
                System.Collections.Generic.List<int> MissingUnitIDs = new List<int>();
                {
                    int UnitID;
                    foreach (System.Data.DataRowView R in RowsToInsert)
                    {
                        if (int.TryParse(R["IdentificationUnitID"].ToString(), out UnitID))
                        {
                            if (!MissingUnitIDs.Contains(UnitID))
                                MissingUnitIDs.Add(UnitID);
                        }
                    }
                }
                foreach (int UnitID in MissingUnitIDs)
                {
                    DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow RU =
                        (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow)this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.NewRow();
                    RU.CollectionSpecimenID = this._iMainForm.ID_Specimen() ;
                    RU.IdentificationUnitID = UnitID;
                    RU.SpecimenPartID = SpecimenPartID;
                    RU.DisplayOrder = (System.Int16)DisplayOrder;
                    try
                    {
                        this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Rows.Add(RU);
                    }
                    catch (System.Exception ex)
                    {

                    }
                    DisplayOrder++;
                }
                string Table = "CollectionSpecimenPart";
                string Restriction = "SpecimenPartID = " + SpecimenPartID.ToString();
                this._iMainForm.setSpecimen();
                System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().Tables[Table].Select(Restriction);
                this._iMainForm.SelectNode(RR[0], Forms.FormCollectionSpecimen.Tree.PartTree);

                // requery the lists
                //this.fillPartDisplayLists(this._iMainForm.SelectedPartHierarchyNode());
                //try
                //{
                //    System.Windows.Forms.TreeNode Tsel = new TreeNode();
                //    if (this._iMainForm.SelectedPartHierarchyNode() != null)
                //    {
                //        Tsel = this._iMainForm.SelectedPartHierarchyNode();
                //    }
                //    else
                //    {
                //        System.Data.DataRow[] Rpart = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Select("SpecimenPartID = " + SpecimenPartID.ToString());
                //        if (Rpart.Length > 0)
                //        {
                //            Tsel = this.getTreeNodeOfDataRow(this.treeViewOverviewHierarchyStorage, null, Rpart[0]);
                //        }
                //    }
                //    if (Tsel != null)
                //    {
                //        Tsel.Nodes.Clear();
                //        this.addOverviewHierarchyPartDependentData(Tsel);
                //        System.Collections.Generic.List<System.Windows.Forms.TreeNode> ChildNodes = new List<TreeNode>();
                //        this.getOverviewHierarchyNodes(Tsel, "IdentificationUnitInPart", this.treeViewOverviewHierarchyStorage, ref ChildNodes);
                //    }
                //}
                //catch { }
                //finally
                //{
                //}
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #region Sorting

        private void toolStripButtonPartLabelMoveUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxPartShowInLabel.SelectedIndex == -1)
                {
                    string Message = DiversityCollection.Forms.FormCollectionSpecimenText.Please_select;
                    Message += ": " + DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation("IdentificationUnit"));
                    System.Windows.Forms.MessageBox.Show(Message);
                    return;
                }
                if (this.listBoxPartShowInLabel.SelectedIndex == 0)
                {
                    System.Windows.Forms.MessageBox.Show("The selected unit is already at the top");
                    return;
                }
                System.Data.DataRowView RMoveUp = (System.Data.DataRowView)this.listBoxPartShowInLabel.SelectedItem;
                System.Data.DataRowView RMoveDown = (System.Data.DataRowView)this.listBoxPartShowInLabel.Items[this.listBoxPartShowInLabel.SelectedIndex - 1];
                if (this.listBoxPartShowInLabel.SelectedIndex == 0 || RMoveUp["DisplayOrder"].ToString() == "1")
                {
                    System.Windows.Forms.MessageBox.Show("The selected unit is already at the top");
                    return;
                }
                int DisplayOrderLower = int.Parse(RMoveUp["DisplayOrder"].ToString()); // the DisplayOrder of the lower dataset
                int DisplayOrderUpper = DisplayOrderLower - 1;// the DisplayOrder of the upper dataset
                // changing the positions
                RMoveDown["DisplayOrder"] = DisplayOrderLower;
                RMoveUp["DisplayOrder"] = DisplayOrderUpper;
                // writing the result in the source table
                foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow R
                    in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Rows)
                {
                    if (R.RowState != DataRowState.Detached && R.RowState != DataRowState.Deleted)
                    {
                        if (R.CollectionSpecimenID.ToString() == RMoveDown["CollectionSpecimenID"].ToString()
                            && R.SpecimenPartID.ToString() == RMoveDown["SpecimenPartID"].ToString()
                            && R.IdentificationUnitID.ToString() == RMoveDown["IdentificationUnitID"].ToString())
                            R.DisplayOrder = short.Parse(RMoveDown["DisplayOrder"].ToString());

                        if (R.CollectionSpecimenID.ToString() == RMoveUp["CollectionSpecimenID"].ToString()
                            && R.SpecimenPartID.ToString() == RMoveUp["SpecimenPartID"].ToString()
                            && R.IdentificationUnitID.ToString() == RMoveUp["IdentificationUnitID"].ToString())
                            R.DisplayOrder = short.Parse(RMoveUp["DisplayOrder"].ToString());
                    }
                }
                //this.fillPartDisplayLists(this.treeViewOverviewHierarchyStorage.SelectedNode);
                this.fillPartDisplayLists((System.Data.DataRowView)this._Source.Current);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void toolStripButtonPartLabelMoveDown_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxPartShowInLabel.SelectedIndex == -1)
                {
                    string Message = DiversityCollection.Forms.FormCollectionSpecimenText.Please_select;
                    Message += ": " + DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation("IdentificationUnit"));
                    System.Windows.Forms.MessageBox.Show(Message);
                    return;
                }
                if (this.listBoxPartShowInLabel.SelectedIndex == this.listBoxPartShowInLabel.Items.Count - 1)
                {
                    System.Windows.Forms.MessageBox.Show("The selected unit is already at the base");
                    return;
                }
                System.Data.DataRowView RMoveDown = (System.Data.DataRowView)this.listBoxPartShowInLabel.SelectedItem;
                System.Data.DataRowView RMoveUp = (System.Data.DataRowView)this.listBoxPartShowInLabel.Items[this.listBoxPartShowInLabel.SelectedIndex + 1];
                int DisplayOrderUpper = int.Parse(RMoveDown["DisplayOrder"].ToString()); // the DisplayOrder of the higher dataset
                int DisplayOrderLower = DisplayOrderUpper + 1; // the DisplayOrder of the lower dataset
                // changing the positions
                RMoveDown["DisplayOrder"] = DisplayOrderLower;
                RMoveUp["DisplayOrder"] = DisplayOrderUpper;
                // writing the result in the source table
                foreach (DiversityCollection.Datasets.DataSetCollectionSpecimen.IdentificationUnitInPartRow R
                    in this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Rows)
                {
                    if (R.RowState != DataRowState.Detached && R.RowState != DataRowState.Deleted)
                    {
                        if (R.CollectionSpecimenID.ToString() == RMoveDown["CollectionSpecimenID"].ToString()
                            && R.SpecimenPartID.ToString() == RMoveDown["SpecimenPartID"].ToString()
                            && R.IdentificationUnitID.ToString() == RMoveDown["IdentificationUnitID"].ToString())
                            R.DisplayOrder = short.Parse(RMoveDown["DisplayOrder"].ToString());

                        if (R.CollectionSpecimenID.ToString() == RMoveUp["CollectionSpecimenID"].ToString()
                            && R.SpecimenPartID.ToString() == RMoveUp["SpecimenPartID"].ToString()
                            && R.IdentificationUnitID.ToString() == RMoveUp["IdentificationUnitID"].ToString())
                            R.DisplayOrder = short.Parse(RMoveUp["DisplayOrder"].ToString());
                    }
                }
                //this.fillPartDisplayLists(this.treeViewOverviewHierarchyStorage.SelectedNode);
                this.fillPartDisplayLists((System.Data.DataRowView)this._Source.Current);
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void toolStripMenuItemPartLabelSortByID_Click(object sender, EventArgs e)
        {
            this.SortByDisplayOrderPart("IdentificationUnitID");
        }

        private void toolStripMenuItemPartLabelSortByName_Click(object sender, EventArgs e)
        {
            this.SortByDisplayOrderPart("LastIdentificationCache");
        }

        private void toolStripMenuItemPartLabelSortByIdentifier_Click(object sender, EventArgs e)
        {
            this.SortByDisplayOrderPart("UnitIdentifier");
        }

        private void SortByDisplayOrderPart(string Target)
        {
            int SpecimenPartID = 0;
            if (this._iMainForm.SelectedPartHierarchyNode() != null)
            {
                System.Data.DataRow Rselected = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                if (Rselected.RowState != DataRowState.Deleted) SpecimenPartID = int.Parse(Rselected["SpecimenPartID"].ToString());
                else
                {
                    System.Data.DataRow RH = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                    SpecimenPartID = int.Parse(RH["SpecimenPartID"].ToString());
                }
            }
            else
            {
                // getting the selected row
                if (this.listBoxPartShowInLabel.Items.Count > 0)
                {
                    System.Data.DataRowView RVmainItem = (System.Data.DataRowView)this.listBoxPartShowInLabel.Items[0];
                    SpecimenPartID = int.Parse(RVmainItem["SpecimenPartID"].ToString());
                }
                else return;
            }
            string Restriction = "SpecimenPartID = " + SpecimenPartID.ToString();

            string SQL = "SELECT P.CollectionSpecimenID, P.IdentificationUnitID, P.SpecimenPartID, P.DisplayOrder, U.LastIdentificationCache " +
                "FROM  IdentificationUnitInPart AS P INNER JOIN " +
                "IdentificationUnit AS U ON P.CollectionSpecimenID = U.CollectionSpecimenID AND P.IdentificationUnitID = U.IdentificationUnitID " +
                "WHERE (P.SpecimenPartID = " + SpecimenPartID + ") AND (P.DisplayOrder > 0) " +
                "ORDER BY U." + Target;
            System.Data.DataTable dt = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                System.Data.DataRow[] RR =
                    this._iMainForm.DataSetCollectionSpecimen().IdentificationUnitInPart.Select(
                    " CollectionSpecimenID = " + dt.Rows[i]["CollectionSpecimenID"].ToString() +
                    " AND IdentificationUnitID = " + dt.Rows[i]["IdentificationUnitID"].ToString() +
                    " AND SpecimenPartID = " + SpecimenPartID, "");
                if (RR.Length > 0)
                {
                    RR[0]["DisplayOrder"] = i + 1;
                }
            }
            this._iMainForm.setSpecimen(this._iMainForm.ID_Specimen());//.setSpecimen(this.ID);
            System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Select(Restriction);
            this._iMainForm.SelectNode(rr[0], Forms.FormCollectionSpecimen.Tree.PartTree);
        }

        #endregion

        #endregion

        #endregion

        #region Help

        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelDisplayOrderPart
        {
            get { return this.tableLayoutPanelDisplayOrderPart; }
        }

        #endregion

    }
}
