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
    public partial class UserControl_DisplayOrderUnit : UserControl__Data
    {

        #region Parameter

        private System.Data.DataView dvUnitsDisplay;
        private System.Data.DataView dvUnitsHide;
        
        #endregion

        #region construction

        public UserControl_DisplayOrderUnit(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
        }
        
        #endregion

        #region Control

        private void initControl()
        {
            this.setDatabindings();

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        private void setDatabindings()
        {
            try
            {
                if (this.dvUnitsDisplay == null)
                    this.dvUnitsDisplay = new System.Data.DataView(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit, "DisplayOrder > 0", "DisplayOrder", System.Data.DataViewRowState.CurrentRows);
                this.listBoxDisplayOrderListShow.DataSource = this.dvUnitsDisplay;
                this.listBoxDisplayOrderListShow.DisplayMember = "LastIdentificationCache";
                if (this.dvUnitsHide == null)
                    this.dvUnitsHide = new System.Data.DataView(this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit, "DisplayOrder = 0", "LastIdentificationCache", System.Data.DataViewRowState.CurrentRows);
                this.listBoxDisplayOrderListHide.DataSource = this.dvUnitsHide;
                this.listBoxDisplayOrderListHide.DisplayMember = "LastIdentificationCache";

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Toolstrip

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
            this._iMainForm.setSpecimen();//.setSpecimen(this.ID);
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
                    this._iMainForm.setSpecimen();
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
                catch { }
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
                    this._iMainForm.setSpecimen();
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

        #region Interface

        public void setTitle(string Title)
        {
            this.groupBoxDisplayOrder.Text = Title;
        }


        public System.Windows.Forms.TableLayoutPanel TableLayoutPanelDisplayOrder
        {
            get { return this.tableLayoutPanelDisplayOrder; }
        }

        #endregion

    }
}
