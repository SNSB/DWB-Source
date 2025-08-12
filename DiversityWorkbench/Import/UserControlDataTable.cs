using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Import
{
    public partial class UserControlDataTable : UserControl//, iDisposableControl
    {

        #region Parameter

        private DiversityWorkbench.Import.DataTable _DataTable;
        
        #endregion

        #region Construction

        public UserControlDataTable(DiversityWorkbench.Import.DataTable DataTable)
        {
            InitializeComponent();
            this._DataTable = DataTable;
            this.labelTable.Text = this._DataTable.GetDisplayText();
            this.pictureBoxTable.Image = this._DataTable.Image;
            if (this._DataTable.MergeHandling != null)
            {
                switch (this._DataTable.MergeHandling)
                {
                    case DiversityWorkbench.Import.DataTable.Merging.Insert:
                        this.radioButtonInsert.Checked = true;
                        break;
                    case DiversityWorkbench.Import.DataTable.Merging.Merge:
                        this.radioButtonMerge.Checked = true;
                        break;
                    case DiversityWorkbench.Import.DataTable.Merging.Update:
                        this.radioButtonUpdate.Checked = true;
                        break;
                    case DiversityWorkbench.Import.DataTable.Merging.Attach:
                        this.radioButtonAttach.Checked = true;
                        break;
                }
                this.setInterface();
            }
        }

        //public void DisposeComponents()
        //{
        //    this.toolTip.Dispose();
        //    this.imageListMergeHandling.Dispose();
        //}

        #endregion

        #region Events
        
        private void radioButtonInsert_Click(object sender, EventArgs e)
        {
            if (this.radioButtonInsert.Checked)
                this._DataTable.MergeHandling = DataTable.Merging.Insert;
            this.setInterface();
        }

        private void radioButtonMerge_Click(object sender, EventArgs e)
        {
            if (this.radioButtonMerge.Checked)
                this._DataTable.MergeHandling = DataTable.Merging.Merge;
            this.setInterface();
        }

        private void radioButtonUpdate_Click(object sender, EventArgs e)
        {
            if (this.radioButtonUpdate.Checked)
                this._DataTable.MergeHandling = DataTable.Merging.Update;
            this.setInterface();
        }

        private void radioButtonAttach_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAttach.Checked)
                this._DataTable.MergeHandling = DataTable.Merging.Attach;
            this.setInterface();
        }
        private void setInterface()
        {
            if (this._DataTable.IsForAttachment)
            {
                this.pictureBoxMergeHandling.Image = this.imageListMergeHandling.Images[3];
                this.radioButtonInsert.Enabled = false;
                this.radioButtonMerge.Enabled = false;
                this.radioButtonUpdate.Enabled = false;
                this.radioButtonInsert.Checked = false;
                this.radioButtonMerge.Checked = false;
                this.radioButtonUpdate.Checked = false;
                this.radioButtonAttach.Checked = true;
                if (DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == this._DataTable.TableAlias)
                    this.BackColor = DiversityWorkbench.Import.Import.ColorAttachment;
            }
            else
            {
                if (DiversityWorkbench.Import.Import.AttachmentColumn != null &&
                    DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == this._DataTable.TableAlias)
                {
                    this.BackColor = DiversityWorkbench.Import.Import.ColorAttachment;
                    if (this._DataTable.MergeHandling == DataTable.Merging.Insert
                        && !this._DataTable.AttachViaParentChildRelation())
                    {
                        this._DataTable.MergeHandling = DataTable.Merging.Update;
                        this.radioButtonUpdate.Checked = true;
                    }
                    if (this._DataTable.AttachViaParentChildRelation())
                        this.radioButtonInsert.Enabled = true;
                    else
                        this.radioButtonInsert.Enabled = false;
                    this.radioButtonMerge.Enabled = true;
                    this.radioButtonUpdate.Enabled = true;
                    this.radioButtonAttach.Enabled = true;
                }
                else
                {
                    this.radioButtonInsert.Enabled = true;
                    this.radioButtonMerge.Enabled = true;
                    this.radioButtonUpdate.Enabled = true;
                    this.radioButtonAttach.Enabled = true;
                }

                switch (this._DataTable.MergeHandling)
                {
                    case DataTable.Merging.Insert:
                        this.pictureBoxMergeHandling.Image = this.imageListMergeHandling.Images[0];
                        break;
                    case DataTable.Merging.Merge:
                        this.pictureBoxMergeHandling.Image = this.imageListMergeHandling.Images[1];
                        break;
                    case DataTable.Merging.Update:
                        this.pictureBoxMergeHandling.Image = this.imageListMergeHandling.Images[2];
                        break;
                    case DataTable.Merging.Attach:
                        this.pictureBoxMergeHandling.Image = this.imageListMergeHandling.Images[3];
                        break;
                }
            }
        }
        
        #endregion

    }
}
