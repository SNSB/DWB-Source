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
    public partial class UserControl_EventImages : UserControl__Data
    {

        #region Construction

        public UserControl_EventImages(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Public

        public void ShowImages()
        {
            this.FormFunctions.FillImageList(this.listBoxEventImages, this.imageListEventImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage, "URI", this.userControlImageEventImage);
        }

        public override void setAvailability()
        {
            base.setAvailability();
            if (!this._iMainForm.ReadOnly())
            {
                bool AnyEvent = true;
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionEvent.Rows.Count == 0)
                    AnyEvent = false;
                if (AnyEvent)
                {
                    this.splitContainerImagesEventDetails.Enabled = true;
                    bool UpdateAnyImage = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE];
                    if (this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows.Count == 0)
                        UpdateAnyImage = false;
                    this.comboBoxEventImageDataWithholdingReason.Enabled = UpdateAnyImage;
                    this.toolStripButtonEventImageDescription.Enabled = UpdateAnyImage;
                    this.tabControlEventImageDetails.Enabled = UpdateAnyImage;
                    if (this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows.Count > 0)
                        this.toolStripButtonEventImageDelete.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.DELETE];
                    else
                        this.toolStripButtonEventImageDelete.Enabled = false;
                    this.toolStripButtonEventImageNew.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.INSERT];
                }
                else
                {
                    this.splitContainerImagesEventDetails.Enabled = false;
                }
            }
        }

        public void setAvailability(bool IsAvailable)
        {
            this.comboBoxEventImageType.Enabled = IsAvailable;
            this.textBoxEventImageNotes.Enabled = IsAvailable;
            this.textBoxEventImageCopyrightStatement.Enabled = IsAvailable;
            this.textBoxEventImageInternalNotes.Enabled = IsAvailable;
            this.textBoxEventImageIPR.Enabled = IsAvailable;
            this.textBoxEventImageLicenseType.Enabled = IsAvailable;
            this.textBoxEventImageLicenseYear.Enabled = IsAvailable;
            this.textBoxEventImageTitle.Enabled = IsAvailable;
            this.comboBoxEventImageDataWithholdingReason.Enabled = IsAvailable;
            this.userControlModuleRelatedEntryEventImageCreator.Enabled = IsAvailable;
            this.userControlModuleRelatedEntryEventImageLicenseHolder.Enabled = IsAvailable;
            this.toolStripButtonEventImageDescription.Enabled = IsAvailable;
            this.tableLayoutPanelEventImages.Enabled = IsAvailable;
            this.toolStripButtonEventImageDelete.Enabled = IsAvailable;
            this.toolStripButtonEventImageNew.Enabled = IsAvailable;

        }

        #endregion

        #region private
        
        private void initControl()
        {
            this.comboBoxEventImageType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "ImageType", true));
            
            //this.initEnumCombobox(this.comboBoxEventImageType, "CollEventImageType_Enum");
            this._EnumComboBoxes = new Dictionary<ComboBox, string>();
            this._EnumComboBoxes.Add(this.comboBoxEventImageType, "CollEventImageType_Enum");
            this.InitLookupSources();

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxEventImageDataWithholdingReason);
            this.comboBoxEventImageDataWithholdingReason_TextChanged(null, null);

            this.textBoxEventImageTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Title", true));
            this.textBoxEventImageNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxEventImageInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "InternalNotes", true));
            this.textBoxEventImageIPR.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "IPR", true));
            this.textBoxEventImageCopyrightStatement.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "CopyrightStatement", true));
            this.textBoxEventImageLicenseType.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseType", true));
            this.textBoxEventImageLicenseYear.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseYear", true));

            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventImageCreator, A, "CollectionEventImage", "CreatorAgent", "CreatorAgentURI", this._Source);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryEventImageLicenseHolder, A, "CollectionEventImage", "LicenseHolder", "LicenseHolderAgentURI", this._Source);

            //System.Collections.Generic.List<string> Settings = new List<string>();
            //Settings.Add("ModuleSource");
            //Settings.Add("CollectionEventImage");
            //Settings.Add("CreatorAgentURI");
            //this.setUserControlModuleRelatedEntrySources(Settings, ref this.userControlModuleRelatedEntryEventImageCreator);

            //Settings = new List<string>();
            //Settings.Add("ModuleSource");
            //Settings.Add("CollectionEventImage");
            //Settings.Add("LicenseHolderAgentURI");
            //this.setUserControlModuleRelatedEntrySources(Settings, ref this.userControlModuleRelatedEntryEventImageLicenseHolder);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();

            this.userControlImageEventImage.AutorotationEnabled = true;

            DiversityWorkbench.Settings.WebViewUsage(this.toolStripButtonSwitchBrowser);
        }

        public override void SetPosition(int Position = 0)
        {
            base.SetPosition(Position);
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryEventImageCreator, "CreatorAgentURI");
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryEventImageLicenseHolder, "LicenseHolderAgentURI");
        }


        public override void InitLookupSources() { this.InitEnums(); }

        private void listBoxEventImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxEventImages.SelectedIndex;
                for (int p = 0; p <= i; p++)
                {
                    if (this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows.Count > i)
                {
                    //this.setAvailability(true);
                    System.Data.DataRow r = this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows[i];
                    this.userControlImageEventImage.ImagePath = r["URI"].ToString();
                    this._Source.Position = i;
                    string ImageType = "";
                    if (!r["ImageType"].Equals(System.DBNull.Value))
                        ImageType = r["ImageType"].ToString();

                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    string Restriction = "(Code LIKE 'audio%' OR Code LIKE 'video%')";
                    if (this.userControlImageEventImage.MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Image)
                        Restriction = "NOT " + Restriction;
                    DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxEventImageType, "CollEventImageType_Enum", con, true, true, true, Restriction);
                    if (ImageType.Length > 0)
                    {
                        try
                        {
                            System.Data.DataTable dt = (System.Data.DataTable)this.comboBoxEventImageType.DataSource;
                            int t = 0;
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                if (R[0].ToString() == ImageType)
                                {
                                    this.comboBoxEventImageType.SelectedIndex = t;
                                    break;
                                }
                                t++;
                            }
                        }
                        catch (System.Exception ex)
                        {
                        }
                    }
                    string XML = r["Description"].ToString();
                    this.userControlXMLTreeEventImageExif.setToDisplayOnly();
                    this.userControlXMLTreeEventImageExif.XML = XML;


                    // Rotate if EXIF File contains info about orientiation of image
                    try
                    {
                        if (this.userControlImageEventImage.AutorotationEnabled && this.userControlImageEventImage.Autorotate)
                        {
                            System.Drawing.RotateFlipType Rotate = DiversityWorkbench.Forms.FormFunctions.ExifRotationInfo(XML);
                            if (Rotate != RotateFlipType.RotateNoneFlipNone)
                                this.userControlImageEventImage.RotateImage(Rotate);
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }

                }
                //else

                this.setAvailability();

            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void comboBoxEventImageDataWithholdingReason_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT DataWithholdingReason FROM CollectionEventImage ORDER BY DataWithholdingReason";
            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxEventImageDataWithholdingReason.DataSource = dt;
                this.comboBoxEventImageDataWithholdingReason.DisplayMember = "DataWithholdingReason";
                this.comboBoxEventImageDataWithholdingReason.ValueMember = "DataWithholdingReason";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonEventImageDelete_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.BindingManagerBase BMB = this.BindingContext[this._iMainForm.DataSetCollectionSpecimen(), "CollectionEventImage"];
                int p = BMB.Position;
                System.Data.DataRow r = this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows[p];
                if (r.RowState != System.Data.DataRowState.Deleted)
                {
                    this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows[p].Delete();
                    this.FormFunctions.FillImageList(this.listBoxEventImages, this.imageListEventImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage, "URI", this.userControlImageEventImage);
                    if (this.listBoxEventImages.Items.Count > 0) this.listBoxEventImages.SelectedIndex = 0;
                    else
                    {
                        this.listBoxEventImages.SelectedIndex = -1;
                        this.userControlImageEventImage.ImagePath = "";
                    }
                }
                this.setAvailability();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButtonEventImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                string AccessionNumber = "";
                if (!this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
                    AccessionNumber = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].ToString();
                int? ProjectID = null;
                if (this._iMainForm.ProjectID() != null)
                    ProjectID = this._iMainForm.ProjectID();
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage(ProjectID, AccessionNumber);
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionEventImageRow R = this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.NewCollectionEventImageRow();
                        int EventID = int.Parse(this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["CollectionEventID"].ToString());
                        R.CollectionEventID = EventID;
                        R.URI = f.URIImage;
                        R.Description = f.Exif;
                        this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows.Add(R);
                        this.FormFunctions.FillImageList(this.listBoxEventImages, this.imageListEventImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage, "URI", this.userControlImageEventImage);
                    }
                }

            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void listBoxEventImages_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListEventImages.ImageSize.Height;
            e.ItemWidth = this.imageListEventImages.ImageSize.Width;
        }

        private void toolStripButtonSwitchBrowser_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.WebViewUsage(!DiversityWorkbench.Settings.UseWebView, this.toolStripButtonSwitchBrowser);
            //DiversityWorkbench.Settings.UseWebView = !DiversityWorkbench.Settings.UseWebView;
            //if (DiversityWorkbench.Settings.UseWebView)
            //{
            //    this.toolStripButtonSwitchBrowser.Image = DiversityCollection.Resource.ExternerBrowserSmall;
            //    this.toolStripButtonSwitchBrowser.ToolTipText = "Change to image display";
            //}
            //else
            //{
            //    this.toolStripButtonSwitchBrowser.Image = DiversityCollection.Resource.Icones;
            //    this.toolStripButtonSwitchBrowser.ToolTipText = "Change to browser";
            //}
            try
            {
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows.Count > 0)
                {
                    System.Data.DataRow r = this._iMainForm.DataSetCollectionSpecimen().CollectionEventImage.Rows[0];
                    this.userControlImageEventImage.ImagePath = r["URI"].ToString();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }

        }

        private void listBoxEventImages_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                    this.imageListEventImages.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
            }
            catch { }
        }

        private void comboBoxEventImageDataWithholdingReason_TextChanged(object sender, EventArgs e)
        {
            this.SetDataWithholdingControl(this.comboBoxEventImageDataWithholdingReason, this.pictureBoxWithholdEventImage);
        }

        #endregion

    }
}
