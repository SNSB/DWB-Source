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
    public partial class UserControl_EventSeriesImages : UserControl__Data
    {

        #region Construction

        public UserControl_EventSeriesImages(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Public

        public void ShowImages(int? SeriesID)
        {
            System.Data.DataRow[] RR = null;
            if (SeriesID != null)
            {
                RR = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Select("SeriesID = " + SeriesID.ToString());
            }
            this.FormFunctions.FillImageList(this.listBoxCollectionEventSeriesImages, this.imageListCollectionEventSeries, this.imageListForm, RR, "URI", this.userControlImageCollectionEventSeries);
        }

        public override void setAvailability()
        {
            bool AnySeries = true;
            if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeries.Rows.Count == 0)
                AnySeries = false;
            if (AnySeries)
            {
                base.setAvailability();
                if (!this._iMainForm.ReadOnly())
                {
                    this.tableLayoutPanelCollectionEventSeriesImages.Enabled = true;
                    bool PermitUpdate = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE];
                    if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows.Count == 0)
                        PermitUpdate = false;
                    this.tableLayoutPanelEventSeriesImageIPR.Enabled = PermitUpdate;
                    this.tableLayoutPanelEventSeriesImageLicense.Enabled = PermitUpdate;
                    this.tableLayoutPanelEventSeriesImageType.Enabled = PermitUpdate;

                    this.toolStripButtonCollectionEventSeriesImageDelete.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.DELETE];
                    this.toolStripButtonCollectionEventSeriesImageNew.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.INSERT];
                }
            }
            else
            {
                this.tableLayoutPanelCollectionEventSeriesImages.Enabled = false;
            }
        }

        public void setAvailability(bool IsAvailable)
        {
            this.tableLayoutPanelEventSeriesImageIPR.Enabled = IsAvailable;
            this.tableLayoutPanelEventSeriesImageLicense.Enabled = IsAvailable;
            this.tableLayoutPanelEventSeriesImageType.Enabled = IsAvailable;
        }

        public override void SetPosition(int Position = 0)
        {
            base.SetPosition(Position);
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryEventSeriesImageCreator, "CreatorAgentURI");
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryEventSeriesImageLicenseHolder, "LicenseHolderAgentURI");
        }

        #endregion

        #region Control

        private void initControl()
        {
            this.comboBoxCollectionEventSeriesImageType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "ImageType", true));
            this.comboBoxEventSeriesImageWithholdingReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DataWithholdingReason", true));
            
            //this.initEnumCombobox(this.comboBoxCollectionEventSeriesImageType, "CollEventSeriesImageType_Enum");
            this._EnumComboBoxes = new Dictionary<ComboBox, string>();
            this._EnumComboBoxes.Add(this.comboBoxCollectionEventSeriesImageType, "CollEventSeriesImageType_Enum");
            this.InitLookupSources();

            this.textBoxCollectionEventSeriesImageNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxCollectionEventSeriesImageInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "InternalNotes", true));
            this.textBoxCollectionEventSeriesImageTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Title", true));
            this.textBoxEventSeriesImageIPR.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "IPR", true));
            this.textBoxEventSeriesImageCopyrightStatement.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "CopyrightStatement", true));
            this.textBoxEventSeriesImageLicenseType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseType", true));
            this.textBoxEventSeriesImageLicenseYear.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseYear", true));

            // 
            // collectionEventSeriesImageBindingSource
            // 
            this._Source.DataMember = "CollectionEventSeriesImage";
            this._Source.DataSource = this._iMainForm.DataSetCollectionEventSeries();

            this.initRemoteConnections();

            //DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            //this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventSeriesImageCreator, (DiversityWorkbench.IWorkbenchUnit)A, "CollectionEventSeriesImage", "CreatorAgent", "CreatorAgentURI", this._Source);
            //this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventSeriesImageLicenseHolder, (DiversityWorkbench.IWorkbenchUnit)A, "CollectionEventSeriesImage", "LicenseHolder", "LicenseHolderAgentURI", this._Source);
            //this.userControlModuleRelatedEntryEventSeriesImageCreator.setForeColor(System.Drawing.Color.Blue);
            //this.userControlModuleRelatedEntryEventSeriesImageLicenseHolder.setForeColor(System.Drawing.Color.Blue);

            //this.ShowImages();

            this.SetDataWithholdingControl(this.comboBoxEventSeriesImageWithholdingReason, this.pictureBoxEventSeriesImageWithholdingReason);

            //System.Collections.Generic.List<string> Settings = new List<string>();
            //Settings.Add("ModuleSource");
            //Settings.Add("CollectionEventSeriesImage");
            //Settings.Add("CreatorAgentURI");
            //this.setUserControlModuleRelatedEntrySources(Settings, ref this.userControlModuleRelatedEntryEventSeriesImageCreator);

            //Settings = new List<string>();
            //Settings.Add("ModuleSource");
            //Settings.Add("CollectionEventSeriesImage");
            //Settings.Add("LicenseHolderAgentURI");
            //this.setUserControlModuleRelatedEntrySources(Settings, ref this.userControlModuleRelatedEntryEventSeriesImageLicenseHolder);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();

            DiversityWorkbench.Settings.WebViewUsage(this.toolStripButtonWebView);
        }

        private void initRemoteConnections()
        {
            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventSeriesImageCreator, (DiversityWorkbench.IWorkbenchUnit)A, "CollectionEventSeriesImage", "CreatorAgent", "CreatorAgentURI", this._Source);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventSeriesImageLicenseHolder, (DiversityWorkbench.IWorkbenchUnit)A, "CollectionEventSeriesImage", "LicenseHolder", "LicenseHolderAgentURI", this._Source);
            this.userControlModuleRelatedEntryEventSeriesImageCreator.setForeColor(System.Drawing.Color.Blue);
            this.userControlModuleRelatedEntryEventSeriesImageLicenseHolder.setForeColor(System.Drawing.Color.Blue);

        }

        public override void InitLookupSources() { this.InitEnums(); }

        #region Toolstrip

        private void toolStripButtonCollectionEventSeriesImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._iMainForm.EventSeriesID() == null) return;

                int? ProjectID = this._iMainForm.ProjectID();
                string AccessionNumber = "";
                if (!this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
                    AccessionNumber = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].ToString();

                string RowGUID = System.Guid.NewGuid().ToString();
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage(ProjectID, AccessionNumber);
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        DiversityCollection.Datasets.DataSetCollectionEventSeries.CollectionEventSeriesImageRow R = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.NewCollectionEventSeriesImageRow();
                        R.SeriesID = (int)this._iMainForm.EventSeriesID();
                        R.URI = f.URIImage;
                        R.Description = f.Exif;
                        this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows.Add(R);
                        this.FormFunctions.FillImageList(this.listBoxCollectionEventSeriesImages, this.imageListCollectionEventSeries, this.imageListForm, this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage, "URI", this.userControlImageCollectionEventSeries);
                        this.setAvailability();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonCollectionEventSeriesImageDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this._iMainForm.DataSetCollectionEventSeries(), "CollectionEventSeriesImage"];
                int p = BMB.Position;
                System.Data.DataRow r = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows[p];
                if (r.RowState != System.Data.DataRowState.Deleted)
                {
                    this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows[p].Delete();
                    this.FormFunctions.FillImageList(this.listBoxCollectionEventSeriesImages, this.imageListCollectionEventSeries, this.imageListForm, this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage, "URI", this.userControlImageCollectionEventSeries);
                    if (this.listBoxCollectionEventSeriesImages.Items.Count > 0) this.listBoxCollectionEventSeriesImages.SelectedIndex = 0;
                    else
                    {
                        this.listBoxCollectionEventSeriesImages.SelectedIndex = -1;
                        this.userControlImageCollectionEventSeries.ImagePath = "";
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonCollectionEventSeriesImageDescription_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFunctions.setImageDescription((System.Windows.Forms.BindingSource)this._Source);
        }

        #endregion

        #region Image List

        private void listBoxCollectionEventSeriesImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxCollectionEventSeriesImages.SelectedIndex;
                if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows.Count == 0) return;
                for (int p = 0; p <= i; p++)
                {
                    if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows.Count > i)
                {
                    this.setAvailability(true);
                    System.Data.DataRow r = this._iMainForm.DataSetCollectionEventSeries().CollectionEventSeriesImage.Rows[i];
                    this.userControlImageCollectionEventSeries.ImagePath = r["URI"].ToString();

                    string XML = r["Description"].ToString();
                    this.userControlXMLTreeExif.setToDisplayOnly();
                    this.userControlXMLTreeExif.XML = XML;
                    
                    // Rotate if EXIF File contains info about orientiation of image
                    try
                    {
                        if (this.userControlImageCollectionEventSeries.AutorotationEnabled && this.userControlImageCollectionEventSeries.Autorotate)
                        {
                            System.Drawing.RotateFlipType Rotate = DiversityWorkbench.Forms.FormFunctions.ExifRotationInfo(XML);
                            if (Rotate != RotateFlipType.RotateNoneFlipNone)
                                this.userControlImageCollectionEventSeries.RotateImage(Rotate);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }

                    this._Source.Position = i;

                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    string Restriction = "(Code LIKE 'audio%' OR Code LIKE 'video%')";
                    if (this.userControlImageCollectionEventSeries.MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Image)
                        Restriction = "NOT " + Restriction;
                    DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxCollectionEventSeriesImageType, "CollEventSeriesImageType_Enum", con, true, true, true, Restriction);
                }
                else
                    this.setAvailability(false);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void listBoxCollectionEventSeriesImages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListCollectionEventSeries.ImageSize.Height;
            e.ItemWidth = this.imageListCollectionEventSeries.ImageSize.Width;
        }

        private void listBoxCollectionEventSeriesImages_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageListCollectionEventSeries.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        private void toolStripButtonWebView_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.WebViewUsage(!DiversityWorkbench.Settings.UseWebView, this.toolStripButtonWebView);
        }

        #endregion

        #region Withholding

        private void comboBoxEventSeriesImageWithholdingReason_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT DataWithholdingReason FROM CollectionEventSeriesImage ORDER BY DataWithholdingReason";
            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxEventSeriesImageWithholdingReason.DataSource = dt;
                this.comboBoxEventSeriesImageWithholdingReason.DisplayMember = "DataWithholdingReason";
                this.comboBoxEventSeriesImageWithholdingReason.ValueMember = "DataWithholdingReason";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxEventSeriesImageWithholdingReason_TextChanged(object sender, EventArgs e)
        {
            this.SetDataWithholdingControl(this.comboBoxEventSeriesImageWithholdingReason, this.pictureBoxEventSeriesImageWithholdingReason);
        }

        #endregion

        #endregion

    }
}
