using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityCollection.Tasks
{
    public partial class FormIPM_ImageDetails : Form
    {

        #region Parameter

        private int _CollectionTaskParentID;
        private int _CollectionTaskID;
        private string _TrapURI;
        private DiversityWorkbench.Forms.FormFunctions FormFunctions;
        private Microsoft.Data.SqlClient.SqlDataAdapter _CollectionTaskTableAdapter;
        private Microsoft.Data.SqlClient.SqlDataAdapter _CollectionTaskImageTableAdapter;

        #endregion

        #region Construction and Form

        /// <summary>
        /// Form for detail images of a trap image
        /// </summary>
        /// <param name="CollectionTaskParentID">The ID of the Trap inspection containing the image of the trap</param>
        /// <param name="TrapURI">The image of the trap</param>
        public FormIPM_ImageDetails(int CollectionTaskParentID, string TrapURI, string Header)
        {
            InitializeComponent();
            this._CollectionTaskParentID = CollectionTaskParentID;
            this._TrapURI = TrapURI;
            this.labelHeader.Text = Header;
            this.labelHeader.Image = DiversityCollection.Specimen.getImage(Specimen.OverviewImageTableOrField.Trap);
            this.labelHeader.ImageAlign = ContentAlignment.MiddleLeft;
            this.FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
            this.initForm();
        }

        private void initForm()
        {
            try
            {
                this.FormFunctions.FillImageList(this.listBoxDetailImages, this.imageListDetailImages, this.dataSetCollectionTask.CollectionTaskImage, "URI", this.userControlImageDetailImage);

                this.userControlPlanMasterImage.setEditState(WpfControls.Geometry.UserControlGeometry.State.Positions);
                this.userControlPlanMasterImage.setCollectionTaskImage(this._CollectionTaskParentID, this._TrapURI);

                string SQL = "SELECT * FROM CollectionTask WHERE CollectionTaskParentID = " + this._CollectionTaskParentID.ToString();
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._CollectionTaskTableAdapter, this.dataSetCollectionTask.CollectionTask, SQL, DiversityWorkbench.Settings.ConnectionString);
                this.listBoxCollectionTasks.DataSource = this.dataSetCollectionTask.CollectionTask;
                this.listBoxCollectionTasks.DisplayMember = "Description";
                this.listBoxCollectionTasks.ValueMember = "CollectionTaskID";

                this.userControlImageDetailImage.ImagePath = "";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                this._TrapURI,
                this._CollectionTaskParentID.ToString());
        }

        private void FormIPM_ImageDetails_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCollectionTask.CollectionTask". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collectionTaskTableAdapter.Fill(this.dataSetCollectionTask.CollectionTask);

        }

        #endregion

        #region Tasks

        private void listBoxCollectionTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                this.collectionTaskBindingSource.Position = this.listBoxCollectionTasks.SelectedIndex;
                if (!int.TryParse(this.listBoxCollectionTasks.SelectedValue.ToString(), out this._CollectionTaskID))
                { 
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxCollectionTasks.SelectedValue;
                    this._CollectionTaskID = int.Parse(R["CollectionTaskID"].ToString());
                }
                this.initDetailImageList(this._CollectionTaskID);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Image list

        private void listBoxDetailImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxDetailImages.SelectedItem != null)
                {
                    System.Data.DataRow R = this.dataSetCollectionTask.Tables["CollectionTaskImage"].Rows[this.listBoxDetailImages.SelectedIndex];
                    this.userControlImageDetailImage.ImagePath = R["URI"].ToString();
                    if (!R["Notes"].Equals(System.DBNull.Value) && R["Notes"].ToString().Contains(this._TrapURI))
                    {
                        this.userControlPlanMasterImage.setCollectionTaskImageGeometry(int.Parse(R["CollectionTaskID"].ToString()), R["URI"].ToString());
                    }
                    else
                        this.userControlPlanMasterImage.setCollectionTaskImageGeometry();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void initDetailImageList(int ID)
        {
            try
            {
                this.listBoxDetailImages.Items.Clear();
                this.dataSetCollectionTask.CollectionTaskImage.Clear();
                string SQL = "SELECT CollectionTaskID, URI, ImageType, Notes " + 
                    //, Description, Title, IPR, CreatorAgent, CreatorAgentURI, CopyrightStatement, " +
                    // "LicenseType, InternalNotes, LicenseHolder, LicenseHolderAgentURI, LicenseYear, DisplayOrder, DataWithholdingReason " +
                    "FROM CollectionTaskImage WHERE CollectionTaskID = " + ID.ToString();
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._CollectionTaskImageTableAdapter, this.dataSetCollectionTask.CollectionTaskImage, SQL, DiversityWorkbench.Settings.ConnectionString);
                this.FormFunctions.FillImageList(this.listBoxDetailImages, this.imageListDetailImages,
                    this.dataSetCollectionTask.Tables["CollectionTaskImage"], "URI", this.userControlImageDetailImage);
                if (this.listBoxDetailImages.Items.Count > 0)
                    this.listBoxDetailImages.SelectedIndex = 0;

                //this._CollectionTaskImageTableAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                //this._CollectionTaskImageTableAdapter.Fill(this.dataSetCollectionTask.CollectionTaskImage);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonDetailImageAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage();
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        System.Data.DataRow R = this.dataSetCollectionTask.CollectionTaskImage.NewCollectionTaskImageRow();
                        R["CollectionTaskID"] = this._CollectionTaskID;
                        R["URI"] = f.URIImage;
                        R["Description"] = f.Exif;
                        R["Notes"] = "in " + this._TrapURI;
                        this.dataSetCollectionTask.CollectionTaskImage.Rows.Add(R);
                        this._CollectionTaskImageTableAdapter.Update(this.dataSetCollectionTask.CollectionTaskImage);
                        this.initDetailImageList(this._CollectionTaskID);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonDetailImageDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string URL = this.userControlImageDetailImage.ImagePath;
                if (URL.Length > 0)
                {
                    System.Data.DataRow[] rr = this.dataSetCollectionTask.CollectionTaskImage.Select("URI = '" + URL + "'");
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow r = rr[0];
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            r.Delete();
                            this._CollectionTaskImageTableAdapter.Update(this.dataSetCollectionTask.CollectionTaskImage);
                            this.initDetailImageList(this._CollectionTaskID);
                            //this._CollectionTask.setFormControls();
                            if (this.listBoxDetailImages.Items.Count > 0)
                                this.listBoxDetailImages.SelectedIndex = 0;
                            else
                            {
                                this.listBoxDetailImages.SelectedIndex = -1;
                                this.userControlImageDetailImage.ImagePath = "";
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

        private void toolStripButtonDetailImageSave_Click(object sender, EventArgs e)
        {
            string RectangleGeometry = this.userControlPlanMasterImage.getRectangleGeometry();
            string SQL = "";
            if (RectangleGeometry.Length > 0)
            {
                SQL = "UPDATE C SET ObjectGeometry = " + RectangleGeometry;
            }
            else
            {
                SQL = "UPDATE C SET ObjectGeometry = NULL";
            }
            SQL += " FROM CollectionTaskImage C WHERE C.CollectionTaskID = " + this._CollectionTaskID.ToString();
            SQL += " AND URI = '" + this.userControlImageDetailImage.ImagePath + "'";
            DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
        }

        private void listBoxDetailImages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListDetailImages.ImageSize.Height;
            e.ItemWidth = this.imageListDetailImages.ImageSize.Width;
        }

        private void listBoxDetailImages_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageListDetailImages.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        #endregion

    }
}
