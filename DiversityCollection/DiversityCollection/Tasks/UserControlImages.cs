using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Tasks
{
    public partial class UserControlImages : UserControl
    {
        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _sqlDataAdapterCollectionTaskImage;
        private DiversityWorkbench.Forms.FormFunctions FormFunctions;

        #endregion

        #region Interface

        private int _CollectionTaskID;
        public int CollectionTaskID
        {
            set
            {
                _CollectionTaskID = value;
                string SQL = "SELECT I.CollectionTaskID, I.URI, I.Notes, CONVERT(varchar(19), I.LogInsertedWhen, 120) AS LogInsertedWhen " +
                    "FROM CollectionTaskImage AS I WHERE I.CollectionTaskID = " + value.ToString() + " ORDER BY LogInsertedWhen";
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._sqlDataAdapterCollectionTaskImage, this.dataSetCollectionTask.Tables["CollectionTaskImage"], SQL, DiversityWorkbench.Settings.ConnectionString);
                this._sqlDataAdapterCollectionTaskImage.Fill(this.dataSetCollectionTask.Tables["CollectionTaskImage"]);
                this.FormFunctions.FillImageList(this.listBoxCollectionImage, this.imageListCollectionTaskImages,
                    this.dataSetCollectionTask.Tables["CollectionTaskImage"], "URI", this.userControlImage);
                if (this.listBoxCollectionImage.Items.Count > 0)
                    this.listBoxCollectionImage.SelectedIndex = 0;
            }
            get { return _CollectionTaskID; }
        }

        #endregion

        #region Construction

        public UserControlImages()
        {
            InitializeComponent();
            this.FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
        }

        #endregion

        #region Image list

        private void listBoxCollectionImage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListCollectionTaskImages.ImageSize.Height;
            e.ItemWidth = this.imageListCollectionTaskImages.ImageSize.Width;
        }

        private void listBoxCollectionImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageListCollectionTaskImages.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        private void listBoxCollectionImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxCollectionImage.SelectedIndex;
                for (int p = 0; p <= i; p++)
                {
                    if (this.dataSetCollectionTask.CollectionTaskImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this.dataSetCollectionTask.CollectionTaskImage.Rows.Count > i && i >= 0)
                {
                    this.tableLayoutPanelImage.Enabled = true;
                    this.toolStripButtonImageDelete.Enabled = true;
                    this.toolStripButtonImageDescription.Enabled = true;
                    System.Data.DataRow r = this.dataSetCollectionTask.CollectionTaskImage.Rows[i];
                    this.userControlImage.ImagePath = r["URI"].ToString();
                    this.collectionTaskImageBindingSource.Position = i;
                    string SQL = "SELECT COUNT(*) FROM CollectionTaskImage WHERE Notes LIKE '%in " + r["URI"].ToString() + "%'";
                    string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (Result != "0")
                        this.toolStripButtonImageDetails.BackColor = System.Drawing.Color.Yellow;
                    else
                        this.toolStripButtonImageDetails.BackColor = System.Drawing.SystemColors.Control;
                }
                else
                {
                    this.tableLayoutPanelImage.Enabled = false;
                    this.toolStripButtonImageDelete.Enabled = false;
                    this.toolStripButtonImageDescription.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region toolstrip for handling images

        private void toolStripButtonImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage();
                f.Text = "Select image for " + this.CurrentTrap();
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        DiversityCollection.Tasks.DataSetCollectionTask.CollectionTaskImageRow R = this.dataSetCollectionTask.CollectionTaskImage.NewCollectionTaskImageRow();
                        R.CollectionTaskID = this.CollectionTaskID;
                        R.URI = f.URIImage;
                        R.Description = f.Exif;
                        this.dataSetCollectionTask.CollectionTaskImage.Rows.Add(R);
                        this._sqlDataAdapterCollectionTaskImage.Update(this.dataSetCollectionTask.CollectionTaskImage);
                        this.CollectionTaskID = this._CollectionTaskID;
                        //this._CollectionTask.setFormControls();
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonImageDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string URL = this.userControlImage.ImagePath;
                if (URL.Length > 0)
                {
                    System.Data.DataRow[] rr = this.dataSetCollectionTask.CollectionTaskImage.Select("URI = '" + URL + "'");
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow r = rr[0];
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            r.Delete();
                            this._sqlDataAdapterCollectionTaskImage.Update(this.dataSetCollectionTask.CollectionTaskImage);
                            //this._CollectionTask.setFormControls();
                            if (this.listBoxCollectionImage.Items.Count > 0) this.listBoxCollectionImage.SelectedIndex = 0;
                            else
                            {
                                this.listBoxCollectionImage.SelectedIndex = -1;
                                this.userControlImage.ImagePath = "";
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

        private void toolStripButtonImageDetails_Click(object sender, EventArgs e)
        {
            if (this.collectionTaskImageBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskImageBindingSource.Current;
                int ID;
                string URI;
                if (int.TryParse(R["CollectionTaskID"].ToString(), out ID) && !R["URI"].Equals(System.DBNull.Value))
                {
                    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                    DiversityCollection.Tasks.FormIPM_ImageDetails form = new FormIPM_ImageDetails(ID, R["URI"].ToString(), CurrentTrap());
                    form.StartPosition = FormStartPosition.CenterParent;
                    form.Width = this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Width - 10;
                    form.Height = this.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Parent.Height - 10;
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                    form.ShowDialog();
                }
            }
        }

        private string CurrentTrap()
        {
            string Trap = "";
            try
            {
                if (this.collectionTaskImageBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionTaskImageBindingSource.Current;
                    int ID;
                    if (int.TryParse(R["CollectionTaskID"].ToString(), out ID))
                    {
                        string SQL = "SELECT T.DisplayText + ' at ' + convert(varchar(10), T.TaskStart, 120) + ' in ' + C.DisplayText " +
                        "FROM CollectionTask AS T INNER JOIN[dbo].[CollectionHierarchyAll] () C ON T.CollectionID = C.CollectionID " +
                        "WHERE CollectionTaskID = " + ID.ToString();
                        Trap = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    }
                }
                else
                {
                        string SQL = "SELECT T.DisplayText + ' at ' + convert(varchar(10), T.TaskStart, 120) + ' in ' + C.DisplayText " +
                        "FROM CollectionTask AS T INNER JOIN[dbo].[CollectionHierarchyAll] () C ON T.CollectionID = C.CollectionID " +
                        "WHERE CollectionTaskID = " + this._CollectionTaskID.ToString();
                        Trap = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return Trap;
        }

        #endregion

    }
}
