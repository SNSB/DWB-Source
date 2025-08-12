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
    public partial class UserControl_Images : UserControl
    {

        #region Parameter and Properties

        private System.Windows.Forms.BindingSource _Source;
        private System.Windows.Forms.BindingSource _SourceProperty;
        private string _HelpNamespace;
        private iMainForm _iMainForm;
        private DiversityWorkbench.FormFunctions _FormFunctions;
        private DiversityWorkbench.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private System.Data.DataTable _dtImageUnits;
        private System.Data.DataTable _dtImageParts;
        private System.Data.DataView _dvImageProperty;
        private string _TableName;
        public string TableName
        {
            get
            {
                if (this._TableName == null)
                {
                    this._TableName = this._Source.DataMember;
                }
                return _TableName;
            }
        }


        #endregion

        #region Construction

        public UserControl_Images(System.Windows.Forms.BindingSource Source, string HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
        }

        public UserControl_Images(System.Windows.Forms.BindingSource Source, System.Windows.Forms.BindingSource SourceProperty, iMainForm MainForm, string HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            this._SourceProperty = SourceProperty;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this._iMainForm = MainForm;
        }
        
        #endregion

        private void initControl()
        {
            try
            {
                // common controls
                this.comboBoxSpecimenImageType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "ImageType", true));
                this.textBoxSpecimenImageNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
                this.comboBoxSpecimenImageWithholdingReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DataWithholdingReason", true));
                this.textBoxSpecimenImageTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Title", true));
                this.textBoxSpecimenImageInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "InternalNotes", true));
                this.textBoxSpecimenImageIPR.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "IPR", true));
                this.textBoxSpecimenImageCopyright.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "CopyrightStatement", true));
                this.textBoxSpecimenImageLicenseYear.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseYear", true));
                this.textBoxSpecimenImageLicense.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseType", true));
                this.textBoxImageLicenseURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseURI", true));
                this.textBoxImageLicenseNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseNotes", true));

                // controls for SpecimenImage
                switch (this.TableName)
                {
                    case "CollectionSpecimenImage":
                        this.textBoxSpecimenImageDisplayOrder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DisplayOrder", true));
                        this.comboBoxSpecimenImageIdentificationUnitID.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "IdentificationUnitID", true));
                        this.comboBoxSpecimenImageSpecimenPart.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "SpecimenPartID", true));

                        this._Source.DataMember = "CollectionSpecimenImage";
                        this._Source.DataSource = this._iMainForm.DataSetCollectionSpecimen();

                        this.listBoxImageProperty.DataSource = this._SourceProperty;
                        this._SourceProperty.DataMember = "CollectionSpecimenImageProperty";
                        this._SourceProperty.DataSource = this._iMainForm.DataSetCollectionSpecimen();

                        break;
                    case "CollectionEventSeriesImage":
                        this.tabControlSpecimenImage.TabPages.Remove(this.tabPageSpecimenImageProperty);
                        break;
                }


                /*
                this.comboBoxSpecimenImageIdentificationUnitID.SelectionChangeCommitted += new System.EventHandler(this.comboBoxSpecimenImageIdentificationUnitID_SelectionChangeCommitted);
                this.comboBoxSpecimenImageSpecimenPart.SelectedIndexChanged += new System.EventHandler(this.comboBoxSpecimenImageSpecimenPart_SelectedIndexChanged);
                this.listBoxImageProperty.SelectedIndexChanged += new System.EventHandler(this.listBoxImageProperty_SelectedIndexChanged);
                this.comboBoxSpecimenImageWithholdingReason.DropDown += new System.EventHandler(this.comboBoxSpecimenImageWithholdingReason_DropDown);
                this.comboBoxSpecimenImageWithholdingReason.TextChanged += new System.EventHandler(this.comboBoxSpecimenImageWithholdingReason_TextChanged);
                this.toolStripButtonImagePropertyAdd.Click += new System.EventHandler(this.toolStripButtonImagePropertyAdd_Click);
                this.toolStripButtonImagePropertyGeometry.Click += new System.EventHandler(this.toolStripButtonImagePropertyGeometry_Click);
                this.toolStripMenuItemImagePropertyAreaHide.Click += new System.EventHandler(this.toolStripMenuItemImagePropertyAreaHide_Click);
                this.toolStripMenuItemImagePropertyAreaShow.Click += new System.EventHandler(this.toolStripMenuItemImagePropertyAreaShow_Click);
                this.toolStripMenuItemImagePropertyAreaAll.Click += new System.EventHandler(this.toolStripMenuItemImagePropertyAreaAll_Click);
                this.toolStripButtonImagePropertyDelete.Click += new System.EventHandler(this.toolStripButtonImagePropertyDelete_Click);
                this.toolStripButtonImagePropertySave.Click += new System.EventHandler(this.toolStripButtonImagePropertySave_Click);
                this.textBoxImagePropertyDescription.Leave += new System.EventHandler(this.textBoxImagePropertyDescription_Leave);
                this.toolStripButtonImagePropertyFilter.Click += new System.EventHandler(this.toolStripButtonImagePropertyFilter_Click);
                this.toolStripTextBoxImagePropertyFilter.Enter += new System.EventHandler(this.toolStripTextBoxImagePropertyFilter_Enter);
                this.toolStripButtonImagePropertyFilterClear.Click += new System.EventHandler(this.toolStripButtonImagePropertyFilterClear_Click);
                this.toolStripButtonSpecimenImageNew.Click += new System.EventHandler(this.toolStripButtonSpecimenImageNew_Click);
                this.toolStripButtonSpecimenImageDelete.Click += new System.EventHandler(this.toolStripButtonSpecimenImageDelete_Click);
                this.toolStripButtonImagesSpecimenShowAll.Click += new System.EventHandler(this.toolStripButtonImagesSpecimenShowAll_Click);
                this.toolStripButtonImageDescription.Click += new System.EventHandler(this.toolStripButtonImageDescription_Click);
                this.toolStripButtonImageUp.Click += new System.EventHandler(this.toolStripButtonImageUp_Click);
                this.toolStripButtonImageDown.Click += new System.EventHandler(this.toolStripButtonImageDown_Click);
                this.toolStripMenuItemImageDisplayOrderUriUp.Click += new System.EventHandler(this.toolStripMenuItemImageDisplayOrderUriUp_Click);
                this.toolStripMenuItemImageDisplayOrderUriDown.Click += new System.EventHandler(this.toolStripMenuItemImageDisplayOrderUriDown_Click);
                this.toolStripMenuItemImageDisplayOrderDateUp.Click += new System.EventHandler(this.toolStripMenuItemImageDisplayOrderDateUp_Click);
                this.toolStripMenuItemImageDisplayOrderDateDown.Click += new System.EventHandler(this.toolStripMenuItemImageDisplayOrderDateDown_Click);
                this.listBoxSpecimenImage.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBoxSpecimenImage_DrawItem);
                this.listBoxSpecimenImage.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBoxSpecimenImage_MeasureItem);
                this.listBoxSpecimenImage.SelectedIndexChanged += new System.EventHandler(this.listBoxSpecimenImage_SelectedIndexChanged);
                 * */
            }
            catch (System.Exception ex)
            {
            }
        }

        #region Specimen images

        #region Common

        private void listBoxSpecimenImage_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index > -1)
                {
                    this.imageListSpecimenImages.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, 50, 50, e.Index);
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void listBoxSpecimenImage_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = this.imageListSpecimenImages.ImageSize.Height;
            e.ItemWidth = this.imageListSpecimenImages.ImageSize.Width;
        }

        private void listBoxSpecimenImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int i = this.listBoxSpecimenImage.SelectedIndex;
                for (int p = 0; p <= i; p++)
                {
                    if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[p].RowState == System.Data.DataRowState.Deleted) i++;
                }
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count > i)
                {
                    this.tabControlSpecimenImage.Enabled = true;
                    System.Data.DataRow r = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[i];
                    this.userControlImageSpecimenImage.ImagePath = r["URI"].ToString();
                    this._Source.Position = i;

                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    string Restriction = "";//(Code LIKE 'audio%' OR Code LIKE 'video%' OR Code LIKE 'sound%')";
                    if (this.userControlImageSpecimenImage.MediumType == DiversityWorkbench.FormFunctions.Medium.Image)
                        Restriction = "NOT " + Restriction;
                    this.setImagePropertyList();

                    string XML = r["Description"].ToString();
                    this.userControlXMLTreeExif.setToDisplayOnly();
                    this.userControlXMLTreeExif.XML = XML;
                    // Checking the display order
                    if (!r["DisplayOrder"].Equals(System.DBNull.Value))
                    {
                        int DisplayOrder;
                        if (int.TryParse(r["DisplayOrder"].ToString(), out DisplayOrder))
                        {
                            System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() + " AND DisplayOrder = " + DisplayOrder.ToString(), "");
                            if (rr.Length > 1)
                                this.textBoxSpecimenImageDisplayOrder.BackColor = System.Drawing.Color.Pink;
                            else
                                this.textBoxSpecimenImageDisplayOrder.BackColor = System.Drawing.SystemColors.Window;
                        }
                    }
                }
                else
                {
                    this.tabControlSpecimenImage.Enabled = false;
                    this.userControlXMLTreeExif.XML = "";
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonSpecimenImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                string AccessionNumber = "";
                if (!this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
                    AccessionNumber = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].ToString();
                int? ProjectID = this._iMainForm.ProjectID();
                string RowGUID = System.Guid.NewGuid().ToString();
                DiversityWorkbench.FormGetImage f = new DiversityWorkbench.FormGetImage(ProjectID, AccessionNumber);
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (f.ImagePath.Length > 0)
                    {
                        DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenImageRow R = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.NewCollectionSpecimenImageRow();
                        R.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                        R.URI = f.URIImage;
                        R.Description = f.Exif;
                        System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("", "DisplayOrder DESC");
                        int DisplayOrder = 1;
                        if (rr.Length > 0 && !rr[0]["DisplayOrder"].Equals(System.DBNull.Value))
                            DisplayOrder = int.Parse(rr[0]["DisplayOrder"].ToString()) + 1;
                        R.DisplayOrder = DisplayOrder;
                        this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Add(R);
                        this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonSpecimenImageDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string URL = this.userControlImageSpecimenImage.ImagePath;
                if (URL.Length > 0)
                {
                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("URI = '" + URL + "'");
                    if (rr.Length > 0)
                    {
                        System.Data.DataRow r = rr[0];
                        if (r.RowState != System.Data.DataRowState.Deleted)
                        {
                            r.Delete();
                            this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
                            if (this.listBoxSpecimenImage.Items.Count > 0) this.listBoxSpecimenImage.SelectedIndex = 0;
                            else
                            {
                                this.listBoxSpecimenImage.SelectedIndex = -1;
                                this.userControlImageSpecimenImage.ImagePath = "";
                            }
                        }
                    }
                }
                bool NoImageLeft = false;
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count == 0
                    || (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count == 1
                    && this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[0].RowState == DataRowState.Deleted))
                    NoImageLeft = true;
                if (NoImageLeft)
                {
                    this.tabControlSpecimenImage.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxSpecimenImageIdentificationUnitID_SelectionChangeCommitted(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView r = (System.Data.DataRowView)this.comboBoxSpecimenImageIdentificationUnitID.SelectedItem;
                System.Data.DataRowView rI = (System.Data.DataRowView)this._Source.Current;
                rI.BeginEdit();
                if (!r[0].Equals(System.DBNull.Value))
                {
                    int IdentificationUnitID = int.Parse(r[0].ToString());
                    rI["IdentificationUnitID"] = IdentificationUnitID;
                }
                else
                    rI["IdentificationUnitID"] = System.DBNull.Value;
                rI.EndEdit();
            }
            catch { }
        }

        private void setSpecimenImageUnitList()
        {
            try
            {
                if (this._dtImageUnits == null)
                {
                    this._dtImageUnits = new DataTable();
                    System.Data.DataColumn C1 = new System.Data.DataColumn("IdentificationUnitID", System.Type.GetType("System.Int32"));
                    System.Data.DataColumn C2 = new System.Data.DataColumn("LastIdentificationCache", System.Type.GetType("System.String"));
                    _dtImageUnits.Columns.Add(C1);
                    _dtImageUnits.Columns.Add(C2);
                    this.comboBoxSpecimenImageIdentificationUnitID.DataSource = this._dtImageUnits;
                    this.comboBoxSpecimenImageIdentificationUnitID.DisplayMember = "LastIdentificationCache";
                    this.comboBoxSpecimenImageIdentificationUnitID.ValueMember = "IdentificationUnitID";
                }
                this._dtImageUnits.Clear();
                System.Data.DataRow r = this._dtImageUnits.NewRow();
                r["LastIdentificationCache"] = System.DBNull.Value;
                r["IdentificationUnitID"] = System.DBNull.Value;
                this._dtImageUnits.Rows.Add(r);
                if (this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Rows.Count > 0)
                {
                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().IdentificationUnit.Select("", "LastIdentificationCache");
                    foreach (System.Data.DataRow R in rr)
                    {
                        if (!R["LastIdentificationCache"].Equals(System.DBNull.Value))
                        {
                            if (R["LastIdentificationCache"].ToString().Length > 0)
                            {
                                System.Data.DataRow ru = this._dtImageUnits.NewRow();
                                ru["LastIdentificationCache"] = R["LastIdentificationCache"].ToString();
                                ru["IdentificationUnitID"] = int.Parse(R["IdentificationUnitID"].ToString());
                                this._dtImageUnits.Rows.Add(ru);
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

        private void setSpecimenImagePartList()
        {
            try
            {
                if (this._dtImageParts == null)
                {
                    this._dtImageParts = new DataTable();
                    System.Data.DataColumn C1 = new System.Data.DataColumn("SpecimenPartID", System.Type.GetType("System.Int32"));
                    System.Data.DataColumn C2 = new System.Data.DataColumn("StorageLocation", System.Type.GetType("System.String"));
                    _dtImageParts.Columns.Add(C1);
                    _dtImageParts.Columns.Add(C2);
                    this.comboBoxSpecimenImageSpecimenPart.DataSource = this._dtImageParts;
                    this.comboBoxSpecimenImageSpecimenPart.DisplayMember = "StorageLocation";
                    this.comboBoxSpecimenImageSpecimenPart.ValueMember = "SpecimenPartID";
                }
                this._dtImageParts.Clear();
                System.Data.DataRow r = this._dtImageParts.NewRow();
                r["StorageLocation"] = System.DBNull.Value;
                r["SpecimenPartID"] = System.DBNull.Value;
                this._dtImageParts.Rows.Add(r);
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Rows.Count > 0)
                {
                    System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenPart.Select("", "StorageLocation");
                    foreach (System.Data.DataRow R in rr)
                    {
                        string DisplayText = "";
                        if (!R["AccessionNumber"].Equals(System.DBNull.Value) && R["AccessionNumber"].ToString().Length > 0)
                            DisplayText = R["AccessionNumber"].ToString() + " - ";
                        if (!R["PartSublabel"].Equals(System.DBNull.Value) && R["PartSublabel"].ToString().Length > 0)
                            DisplayText += R["PartSublabel"].ToString() + " - ";
                        if (!R["StorageLocation"].Equals(System.DBNull.Value) && R["StorageLocation"].ToString().Length > 0)
                            DisplayText += R["StorageLocation"].ToString() + " - ";
                        if (!R["MaterialCategory"].Equals(System.DBNull.Value) && R["MaterialCategory"].ToString().Length > 0)
                            DisplayText += R["MaterialCategory"].ToString() + " - ";
                        System.Data.DataRow ru = this._dtImageParts.NewRow();
                        ru["StorageLocation"] = DisplayText;
                        ru["SpecimenPartID"] = int.Parse(R["SpecimenPartID"].ToString());
                        this._dtImageParts.Rows.Add(ru);
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxSpecimenImageSpecimenPart_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView r = (System.Data.DataRowView)this.comboBoxSpecimenImageSpecimenPart.SelectedItem;
                System.Data.DataRowView rI = (System.Data.DataRowView)this._Source.Current;
                if (r == null || rI == null) return;
                rI.BeginEdit();
                if (!r[0].Equals(System.DBNull.Value))
                {
                    int SpecimenPartID = int.Parse(r[0].ToString());
                    rI["SpecimenPartID"] = SpecimenPartID;
                }
                else
                    rI["SpecimenPartID"] = System.DBNull.Value;
                rI.EndEdit();
            }
            catch { }
        }

        private void toolStripButtonImageDescription_Click(object sender, EventArgs e)
        {
            this.setImageDescription(this._Source);
        }

        private void setImageDescription(System.Windows.Forms.BindingSource BindingSource)
        {
            if (BindingSource.Current == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select an image");
                return;
            }
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)BindingSource.Current;
                string XML = R["Description"].ToString();
                DiversityWorkbench.Forms.FormXml f = new DiversityWorkbench.Forms.FormXml("EXIF for image", XML, true);
                f.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }
        
        #endregion

        #region Moving the image and display order

        private void toolStripButtonImageUp_Click(object sender, EventArgs e)
        {
            if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count == 0)
                return;
            this.ReplenishImageDisplayOrder();
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("", "DisplayOrder");
            if (rr[0]["DisplayOrder"].ToString() == R["DisplayOrder"].ToString())
            {
                System.Windows.Forms.MessageBox.Show("The image is already on top of the list");
                return;
            }
            else
            {
                int CurrentDisplayOrder = int.Parse(R["DisplayOrder"].ToString());
                int NewDisplayOrder = CurrentDisplayOrder - 1;
                System.Data.DataRow[] rrChange = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("DisplayOrder = " + NewDisplayOrder.ToString(), "DisplayOrder");
                if (rrChange.Length > 0)
                {
                    rrChange[0]["DisplayOrder"] = CurrentDisplayOrder;
                }
                R["DisplayOrder"] = NewDisplayOrder;
                try
                {
                    R.BeginEdit();
                    R.EndEdit();
                    rrChange[0].BeginEdit();
                    rrChange[0].EndEdit();
                }
                catch { }
                this._iMainForm.setSpecimen();
            }
        }

        private void toolStripButtonImageDown_Click(object sender, EventArgs e)
        {
            if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count == 0)
                return;
            this.ReplenishImageDisplayOrder();
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("", "DisplayOrder DESC");
            if (rr[0]["DisplayOrder"].ToString() == R["DisplayOrder"].ToString())
            {
                System.Windows.Forms.MessageBox.Show("The image is already at the end of the list");
                return;
            }
            else
            {
                if (R["DisplayOrder"].Equals(System.DBNull.Value))
                {
                    int DisplayOrder = 1;
                    if (rr.Length > 0 && !rr[0]["DisplayOrder"].Equals(System.DBNull.Value))
                        DisplayOrder = int.Parse(rr[0]["DisplayOrder"].ToString());
                    R.BeginEdit();
                    R["DisplayOrder"] = DisplayOrder;
                    R.EndEdit();
                    //return;
                }
                int CurrentDisplayOrder = int.Parse(R["DisplayOrder"].ToString());
                int NewDisplayOrder = CurrentDisplayOrder + 1;
                System.Data.DataRow[] rrChange = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("DisplayOrder = " + NewDisplayOrder.ToString(), "DisplayOrder DESC");
                if (rrChange.Length > 0)
                {
                    rrChange[0]["DisplayOrder"] = CurrentDisplayOrder;
                }
                R["DisplayOrder"] = NewDisplayOrder;
                try
                {
                    R.BeginEdit();
                    R.EndEdit();
                    rrChange[0].BeginEdit();
                    rrChange[0].EndEdit();
                }
                catch { }
                //this.SaveSpecimenImages();
                this._iMainForm.setSpecimen();
            }
        }

        private void ReplenishImageDisplayOrder()
        {
            if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count == 0)
                return;
            int Previous = 0;
            int Current = Previous + 1;
            bool OK = true;
            System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("", "DisplayOrder ASC");
            foreach (System.Data.DataRow R in rr)
            {
                if (R["DisplayOrder"].Equals(System.DBNull.Value))
                {
                    OK = false;
                    break;
                }
                int.TryParse(R["DisplayOrder"].ToString(), out Current);
                if (Current == Previous)
                {
                    OK = false;
                    break;
                }
                Previous = Current;
            }
            if (OK)
                return;
            Current = 1;
            foreach (System.Data.DataRow R in this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows)
            {
                R["DisplayOrder"] = Current;
                Current++;
            }
        }

        private void SaveSpecimenImages()
        {
            this._iMainForm.SaveImagesSpecimen();
        }

        private void toolStripMenuItemImageDisplayOrderUriUp_Click(object sender, EventArgs e)
        {
            this.setImageDisplayOrder("URI", false);
        }

        private void toolStripMenuItemImageDisplayOrderUriDown_Click(object sender, EventArgs e)
        {
            this.setImageDisplayOrder("URI", true);
        }

        private void toolStripMenuItemImageDisplayOrderDateUp_Click(object sender, EventArgs e)
        {
            this.setImageDisplayOrder("LogCreatedWhen", false);
        }

        private void toolStripMenuItemImageDisplayOrderDateDown_Click(object sender, EventArgs e)
        {
            this.setImageDisplayOrder("LogCreatedWhen", true);
        }

        private void setImageDisplayOrder(string OrderingColumn, bool Descending)
        {
            try
            {
                string SortString = OrderingColumn;
                if (Descending) SortString += " DESC";
                System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("", SortString);
                for (int i = 0; i < this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count; i++)
                {
                    rr[i].BeginEdit();
                    rr[i]["DisplayOrder"] = i;
                    rr[i].EndEdit();
                }
                this._iMainForm.setSpecimen();
            }
            catch (System.Exception ex) { }
        }

        private void textBoxSpecimenImageDisplayOrder_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxSpecimenImageDisplayOrder.Text.Length == 0)
            {
                this.textBoxSpecimenImageDisplayOrder.BackColor = System.Drawing.Color.Pink;
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                R.BeginEdit();
                R["DisplayOrder"] = System.DBNull.Value;
                R.EndEdit();
                this.textBoxSpecimenImageNotes.Focus();
            }
            else this.textBoxSpecimenImageDisplayOrder.BackColor = System.Drawing.Color.White;
            int i;
            if (this.textBoxSpecimenImageDisplayOrder.Text.Length > 0 && !int.TryParse(this.textBoxSpecimenImageDisplayOrder.Text, out i))
            {
                System.Windows.Forms.MessageBox.Show(this.textBoxSpecimenImageDisplayOrder.Text + " is not a valid sequence");
                this.textBoxSpecimenImageDisplayOrder.Text = "";
            }
        }

        #endregion

        #region Image property

        private void setImagePropertyList()
        {
            try
            {
                this.toolStripImagePropertyFilter.Enabled = false;
                this.toolStripButtonImagePropertyDelete.Enabled = false;
                this.toolStripButtonImagePropertyGeometry.Enabled = false;
                this.toolStripTextBoxImagePropertyFilter.AutoCompleteCustomSource = null;

                System.Data.DataRowView RI = (System.Data.DataRowView)this._Source.Current;
                string URI = RI["URI"].ToString();
                this._dvImageProperty = new DataView(this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty, "URI = '" + URI + "'", "Property", DataViewRowState.CurrentRows);
                this.listBoxImageProperty.DataSource = this._dvImageProperty;
                this.listBoxImageProperty.DisplayMember = "Property";
                this.listBoxImageProperty.ValueMember = "Property";
                if (this._dvImageProperty.Count == 0)
                {
                    this.textBoxImagePropertyDescription.DataBindings.Clear();
                    this.textBoxImagePropertyDescription.Text = "";
                    if (this.ImageMarkingActive)
                        this.toolStripButtonImagePropertyGeometry_Click(null, null);
                }
                else
                {
                    if (this.textBoxImagePropertyDescription.DataBindings.Count == 0)
                        this.textBoxImagePropertyDescription.DataBindings.Add("Text", this._SourceProperty, "Description");
                }
                if (this.toolStripDropDownButtonImagePropertyArea.Visible)
                    this.toolStripDropDownButtonImagePropertyArea.Visible = false;
            }
            catch (System.Exception ex)
            {

            }
        }

        private void toolStripButtonImagePropertyAdd_Click(object sender, EventArgs e)
        {
            try
            {
                bool AllowNewProperty = false;
                if (DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator")
                    || DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
                {
                    AllowNewProperty = true;
                }

                System.Data.DataTable dt = new DataTable();
                System.Data.DataRowView RI = (System.Data.DataRowView)this._Source.Current;
                string CurrentUri = RI["URI"].ToString();

                string SqlList = "";
                foreach (System.Object o in this.listBoxImageProperty.Items)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)o;
                    if (SqlList.Length > 0) SqlList += ", ";
                    SqlList += "'" + R["Property"].ToString() + "'";
                }
                string SQL = "SELECT DISTINCT Property " +
                    "FROM CollectionSpecimenImageProperty ";
                if (SqlList.Length > 0)
                    SQL += " WHERE Property NOT IN ( " + SqlList + ")";

                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                string Header = "Please select the property from the list";
                string Message = "Please select the property from the list";
                if (AllowNewProperty)
                {
                    Message += " above\r\nor\r\nenter a new property in the text box below";
                    Header += " or enter a new one";
                }
                DiversityWorkbench.FormGetStringFromList f = new DiversityWorkbench.FormGetStringFromList(dt, "Property", "Property", "New image property", Message, "", AllowNewProperty);
                f.ReduceInterfaceToCombobox(Header, !AllowNewProperty);
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    string Property = f.SelectedString;
                    if (AllowNewProperty && f.String.Length > 0)
                        Property = f.String;

                    if (Property.Length > 0)
                    {
                        DiversityCollection.Datasets.DataSetCollectionSpecimen.CollectionSpecimenImagePropertyRow R = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.NewCollectionSpecimenImagePropertyRow();
                        R.CollectionSpecimenID = this._iMainForm.ID_Specimen();
                        R.URI = CurrentUri;
                        R.Property = Property;
                        this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.Rows.Add(R);
                        this.toolStripButtonImagePropertySave.Visible = true;
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private bool _ImageMarkingActive = false;

        public bool ImageMarkingActive
        {
            get { return _ImageMarkingActive; }
            set
            {
                if (value)
                    this.toolStripButtonImagePropertyGeometry.BackColor = System.Drawing.Color.Red;
                else
                    this.toolStripButtonImagePropertyGeometry.BackColor = System.Drawing.SystemColors.Control;
                _ImageMarkingActive = value;
            }
        }

        private void toolStripButtonImagePropertyGeometry_Click(object sender, EventArgs e)
        {
            this.ImageMarkingActive = !this._ImageMarkingActive;

            this.userControlImageSpecimenImage.setMarkingArea(this._ImageMarkingActive);
            if (this._ImageMarkingActive)
            {
                if (this._SourceProperty.Current == null
                    || this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.Rows.Count == 0
                    || this.listBoxImageProperty.SelectedIndex == -1)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a property");
                    return;
                }
                this.userControlImageSpecimenImage.setMarkingTable("CollectionSpecimenImageProperty");
                this.userControlImageSpecimenImage.setMarkingGeometryColumn("ImageArea");
                this.userControlImageSpecimenImage.setMarkingDisplayColumn("Property");
                if (this._SourceProperty.Current != null)
                {
                    System.Data.DataRowView RI = (System.Data.DataRowView)this._SourceProperty.Current;
                    this.userControlImageSpecimenImage.setMarkingWhereClause(" WHERE CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() + " AND URI = '" + RI["URI"].ToString() + "' AND Property = '" + RI["Property"].ToString() + "'");
                }
                else
                    this.userControlImageSpecimenImage.setMarkingWhereClause(" WHERE 1 = 0");
                this.userControlImageSpecimenImage.LoadImageForMarking();
            }
            else
            {
            }

            return;




            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");

            System.Data.DataRowView R = (System.Data.DataRowView)this._SourceProperty.Current;
            string Property = R["Property"].ToString();
            string WhereClause = " WHERE CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() + " AND URI = '" + this.userControlImageSpecimenImage.ImagePath + "' AND Property = '" + Property + "'";
            //string SQL = "DECLARE @Points table(X int, Y int); " +
            //"DECLARE @g geometry; " +
            //    "SET @g = (select ImageArea from [dbo].[CollectionSpecimenImageProperty] " +
            //    WhereClause + ");" +
            //    "INSERT INTO @Points(X, Y) " +
            //    "SELECT @g.STPointN(1).STX, @g.STPointN(1).STY WHERE NOT @g IS NULL; " +
            //    "INSERT INTO @Points(X, Y)  " +
            //    "SELECT @g.STPointN(2).STX, @g.STPointN(2).STY WHERE NOT @g IS NULL; " +
            //    "INSERT INTO @Points(X, Y)  " +
            //    "SELECT @g.STPointN(3).STX, @g.STPointN(3).STY WHERE NOT @g IS NULL; " +
            //    "INSERT INTO @Points(X, Y)  " +
            //    "SELECT @g.STPointN(4).STX, @g.STPointN(4).STY WHERE NOT @g IS NULL; " +
            //    "SELECT * from @Points";
            //System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dt = this.getImagePropertyArea();// new DataTable();
            //try
            //{
            //    ad.Fill(dt);
            //}
            //catch (System.Exception ex) { }
            System.Drawing.Rectangle Area = new Rectangle();
            if (dt.Rows.Count > 0)
            {
                Area.X = int.Parse(dt.Rows[0]["X"].ToString());
                Area.Y = int.Parse(dt.Rows[0]["Y"].ToString());
                Area.Width = int.Parse(dt.Rows[1]["X"].ToString()) - int.Parse(dt.Rows[0]["X"].ToString());
                Area.Height = int.Parse(dt.Rows[3]["Y"].ToString()) - int.Parse(dt.Rows[0]["Y"].ToString());
            }
            this.userControlImageSpecimenImage.MarkArea("CollectionSpecimenImageProperty", "ImageArea", WhereClause, Area, Property);
            //this.toolStripButtonImagePropertySave.Visible = true;
            return;
        }

        private void toolStripButtonImagePropertyDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxImageProperty.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxImageProperty.SelectedItem;
                R.Delete();
            }
        }

        private void listBoxImageProperty_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxImageProperty.SelectedIndex == -1)
                {
                    if (this.userControlImageSpecimenImage.MarkingArea())
                    {
                        this.userControlImageSpecimenImage.setMarkingWhereClause(" WHERE 1 = 0");
                        this.userControlImageSpecimenImage.LoadGeometry();
                    }
                    this.toolStripImagePropertyFilter.Enabled = false;
                    this.toolStripButtonImagePropertyDelete.Enabled = false;
                    this.toolStripButtonImagePropertyGeometry.Enabled = false;
                }
                else
                {
                    this.toolStripImagePropertyFilter.Enabled = true;
                    this.toolStripButtonImagePropertyDelete.Enabled = true;
                    this.toolStripButtonImagePropertyGeometry.Enabled = true;

                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxImageProperty.SelectedItem;
                    string P = R["Property"].ToString();
                    string URI = R["URI"].ToString();
                    if (this.userControlImageSpecimenImage.MarkingArea())
                    {
                        this.userControlImageSpecimenImage.setMarkingWhereClause(" WHERE CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() + " AND URI = '" + R["URI"].ToString() + "' AND Property = '" + R["Property"].ToString() + "'");
                        this.userControlImageSpecimenImage.LoadGeometry();
                        /// ##1
                    }
                    int i = 0;
                    foreach (System.Data.DataRow RP in this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.Rows)
                    {
                        if (RP["Property"].ToString() == R["Property"].ToString() &&
                            RP["URI"].ToString() == R["URI"].ToString())
                        {
                            this._SourceProperty.Position = i;
                            break;
                        }
                        i++;
                    }
                }
                //this.toolStripDropDownButtonImagePropertyArea

                /*
                 * old version
                System.Drawing.Rectangle Area = new Rectangle();
                this.userControlImageSpecimenImage.MarkArea("", "", "", Area, "");
                //this.userControlImageSpecimenImage.MarkArea("", "", "", Area);

                if (this.listBoxImageProperty.SelectedIndex > -1)
                {
                    this.textBoxImagePropertyDescription.Enabled = true;
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxImageProperty.SelectedItem;
                    string P = R["Property"].ToString();
                    string URI = R["URI"].ToString();
                    int Position = 0;
                    for (int i = 0; i < this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.Rows.Count; i++)
                    {
                        if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.Rows[i]["Property"].ToString() == P &&
                            this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.Rows[i]["URI"].ToString() == URI)
                            break;
                        Position++;
                    }
                    this._SourceProperty.Position = Position;
                    System.Data.DataTable dt = this.getImagePropertyArea();// new DataTable();
                    if (dt.Rows.Count > 0)
                    {
                        this.toolStripButtonImagePropertyGeometry.BackColor = System.Drawing.Color.Red;
                    }
                    else
                        this.toolStripButtonImagePropertyGeometry.BackColor = System.Drawing.SystemColors.Control;
                    string Property = "";
                    System.Collections.Generic.Dictionary<string, System.Drawing.Rectangle> Areas = new Dictionary<string, Rectangle>();
                    switch (this._ImagePropertyAreaDisplayStyle)
                    {
                        case ImagePropertyAreaDisplayStyle.All:
                            foreach (System.Data.DataRow Rprop in this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.Rows)
                            {
                                if (Rprop["URI"].ToString() == URI)
                                {
                                    Property = Rprop["Property"].ToString();
                                    dt = this.getImagePropertyArea(Rprop);
                                    if (dt.Rows.Count > 0)
                                    {
                                        Area.X = int.Parse(dt.Rows[0]["X"].ToString());
                                        Area.Y = int.Parse(dt.Rows[0]["Y"].ToString());
                                        Area.Width = int.Parse(dt.Rows[1]["X"].ToString()) - int.Parse(dt.Rows[0]["X"].ToString());
                                        Area.Height = int.Parse(dt.Rows[3]["Y"].ToString()) - int.Parse(dt.Rows[0]["Y"].ToString());
                                        Areas.Add(Property, Area);
                                    }
                                }
                            }
                            this.userControlImageSpecimenImage.MarkAreas(Areas);
                            break;
                        case ImagePropertyAreaDisplayStyle.Show:
                            Property = R["Property"].ToString();
                            if (dt.Rows.Count > 0)
                            {
                                Area.X = int.Parse(dt.Rows[0]["X"].ToString());
                                Area.Y = int.Parse(dt.Rows[0]["Y"].ToString());
                                Area.Width = int.Parse(dt.Rows[1]["X"].ToString()) - int.Parse(dt.Rows[0]["X"].ToString());
                                Area.Height = int.Parse(dt.Rows[3]["Y"].ToString()) - int.Parse(dt.Rows[0]["Y"].ToString());
                            }
                            Areas.Add(Property, Area);
                            this.userControlImageSpecimenImage.MarkAreas(Areas);
                            break;
                        case ImagePropertyAreaDisplayStyle.Hide:
                            //this.userControlImageSpecimenImage.MarkAreas(Areas);
                            break;
                    }
                }
                else
                {
                    this.textBoxImagePropertyDescription.Enabled = false;
                }
                 * */
            }
            catch (System.Exception ex)
            {
            }
        }

        private System.Data.DataTable getImagePropertyArea()
        {
            System.Data.DataRowView RP = (System.Data.DataRowView)this._SourceProperty.Current;
            System.Data.DataTable dt = this.getImagePropertyArea(RP.Row);
            return dt;
        }

        private System.Data.DataTable getImagePropertyArea(System.Data.DataRow RP)
        {
            string Property = RP["Property"].ToString();
            string WhereClause = " WHERE CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() + " AND URI = '" + this.userControlImageSpecimenImage.ImagePath + "' AND Property = '" + Property + "'";
            string SQL = "DECLARE @Points table(X int, Y int); " +
            "DECLARE @g geometry; " +
                "SET @g = (select ImageArea from [dbo].[CollectionSpecimenImageProperty] " +
                WhereClause + ");" +
                "INSERT INTO @Points(X, Y) " +
                "SELECT @g.STPointN(1).STX, @g.STPointN(1).STY WHERE NOT @g.STPointN(1) IS NULL; " +
                "INSERT INTO @Points(X, Y)  " +
                "SELECT @g.STPointN(2).STX, @g.STPointN(2).STY WHERE NOT @g.STPointN(2) IS NULL; " +
                "INSERT INTO @Points(X, Y)  " +
                "SELECT @g.STPointN(3).STX, @g.STPointN(3).STY WHERE NOT @g.STPointN(3) IS NULL; " +
                "INSERT INTO @Points(X, Y)  " +
                "SELECT @g.STPointN(4).STX, @g.STPointN(4).STY WHERE NOT @g.STPointN(4) IS NULL; " +
                "SELECT * from @Points";
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dt = new DataTable();
            try
            {
                ad.Fill(dt);
            }
            catch (System.Exception ex) { }
            return dt;
        }

        private void textBoxImagePropertyDescription_Leave(object sender, EventArgs e)
        {
            System.Data.DataRowView RImageProperty = (System.Data.DataRowView)this._SourceProperty.Current;
            if (RImageProperty != null)
            {
                try
                {
                    RImageProperty.BeginEdit();
                    RImageProperty.EndEdit();
                }
                catch { }
            }
        }

        private void toolStripButtonImagePropertySave_Click(object sender, EventArgs e)
        {
            int Pos = this.listBoxSpecimenImage.SelectedIndex;// this._SourceProperty.Position;
            System.Data.DataRowView RImageProperty = (System.Data.DataRowView)this._SourceProperty.Current;
            if (RImageProperty != null)
            {
                try
                {
                    RImageProperty.BeginEdit();
                    RImageProperty.EndEdit();
                }
                catch { }
            }
            this._iMainForm.SaveImagesSpecimen();
            this.toolStripButtonImagePropertySave.Visible = false;

            this.listBoxSpecimenImage.SelectedIndex = Pos;
            this.toolStripTextBoxImagePropertyFilter.AutoCompleteCustomSource = null;
        }

        //private void toolStripButtonImagePropertyFilter_Click(object sender, EventArgs e)
        //{
        //    if (this.ImagePropertyFilter().Length == 0)
        //        System.Windows.Forms.MessageBox.Show("No filter defined");
        //    else
        //        this.RestrictImagesToPropertyFilter();
        //}

        //private void RestrictImagesToPropertyFilter()
        //{
        //    System.Data.DataRowView RImage = (System.Data.DataRowView)this._Source.Current;
        //    if (RImage != null)
        //    {
        //        try
        //        {
        //            RImage.BeginEdit();
        //            RImage.EndEdit();
        //        }
        //        catch { }
        //    }
        //    this.FormFunctions.updateTable(this._iMainForm.DataSetCollectionSpecimen(), "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this._Source);

        //    this.listBoxSpecimenImage.Items.Clear();
        //    this.userControlImageSpecimenImage.ImagePath = "";
        //    this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Clear();
        //    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionSpecimenPartBindingSource.Current;
        //    string PropertyFilter = this.ImagePropertyFilter();
        //    if (PropertyFilter.Length > 0)
        //    {
        //        string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString() +
        //            " AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable) " +
        //            " AND URI IN (SELECT I.URI FROM CollectionSpecimenImage AS I " +
        //            "INNER JOIN CollectionSpecimenImageProperty AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID AND I.URI = P.URI " +
        //            "WHERE (I.CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() + ") AND (P.Property LIKE '" + PropertyFilter + "'))";
        //        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage);
        //        this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
        //        this.toolStripButtonImagesSpecimenShowAll.Visible = true;
        //    }
        //}

        private string ImagePropertyFilter()
        {
            try
            {
                if (this.toolStripTextBoxImagePropertyFilter != null && this.toolStripTextBoxImagePropertyFilter.Text != null)
                    return this.toolStripTextBoxImagePropertyFilter.Text.Trim();
                else
                    return "";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return "";
        }

        private enum ImagePropertyAreaDisplayStyle { All, Show, Hide };
        private ImagePropertyAreaDisplayStyle _ImagePropertyAreaDisplayStyle = ImagePropertyAreaDisplayStyle.Hide;

        private void toolStripMenuItemImagePropertyAreaShow_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonImagePropertyArea.ForeColor = System.Drawing.Color.Red;
            this.toolStripDropDownButtonImagePropertyArea.Text = "Show";
            this.toolStripDropDownButtonImagePropertyArea.ToolTipText = this.toolStripMenuItemImagePropertyAreaShow.ToolTipText;
            this._ImagePropertyAreaDisplayStyle = ImagePropertyAreaDisplayStyle.Show;
        }

        private void toolStripMenuItemImagePropertyAreaHide_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonImagePropertyArea.ForeColor = System.Drawing.Color.Black;
            this.toolStripDropDownButtonImagePropertyArea.Text = "Hide";
            this.toolStripDropDownButtonImagePropertyArea.ToolTipText = this.toolStripMenuItemImagePropertyAreaHide.ToolTipText;
            this._ImagePropertyAreaDisplayStyle = ImagePropertyAreaDisplayStyle.Hide;
        }

        private void toolStripMenuItemImagePropertyAreaAll_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonImagePropertyArea.ForeColor = System.Drawing.Color.Red;
            this.toolStripDropDownButtonImagePropertyArea.Text = "All";
            this.toolStripDropDownButtonImagePropertyArea.ToolTipText = this.toolStripMenuItemImagePropertyAreaAll.ToolTipText;
            this._ImagePropertyAreaDisplayStyle = ImagePropertyAreaDisplayStyle.All;
        }

        private void toolStripButtonImagePropertyFilterClear_Click(object sender, EventArgs e)
        {
            if (this.toolStripTextBoxImagePropertyFilter.Text.Length > 0)
                this.toolStripButtonImagesSpecimenShowAll_Click(null, null);
        }

        private void toolStripTextBoxImagePropertyFilter_Enter(object sender, EventArgs e)
        {
            if (this.toolStripTextBoxImagePropertyFilter.AutoCompleteCustomSource == null)
            {
                this.toolStripTextBoxImagePropertyFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                this.toolStripTextBoxImagePropertyFilter.AutoCompleteSource = AutoCompleteSource.CustomSource;
                System.Windows.Forms.AutoCompleteStringCollection Source = new AutoCompleteStringCollection();
                foreach (System.Data.DataRow RP in this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.Rows)
                {
                    Source.Add(RP["Property"].ToString());
                }
                this.toolStripTextBoxImagePropertyFilter.AutoCompleteCustomSource = Source;
            }
        }

        #endregion

        #region Restriction of the displayed images

        private void toolStripButtonImagesSpecimenShowAll_Click(object sender, EventArgs e)
        {
            this.toolStripTextBoxImagePropertyFilter.Text = "";
            this.listBoxSpecimenImage.Items.Clear();
            this.userControlImageSpecimenImage.ImagePath = "";
            System.Data.DataRowView RImage = (System.Data.DataRowView)this._Source.Current;
            if (RImage != null)
            {
                try
                {
                    RImage.BeginEdit();
                    RImage.EndEdit();
                }
                catch { }
            }
            this._iMainForm.SaveImagesSpecimen();
            this._iMainForm.ShowAllSpecimenImages();
        }

        //private void EnableRestrictImageToUnitButton(System.Data.DataRow R)
        //{
        //    int UnitID;
        //    try
        //    {
        //        if (R.Table.Columns.Contains("IdentificationUnitID"))
        //        {
        //            if (int.TryParse(R["IdentificationUnitID"].ToString(), out UnitID))
        //            {
        //                System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("IdentificationUnitID = " + UnitID.ToString());
        //                if (rr.Length > 0)
        //                    this.buttonRestrictImagesToCurrentUnit.Enabled = true;
        //                else this.buttonRestrictImagesToCurrentUnit.Enabled = false;
        //            }
        //        }
        //        else this.buttonRestrictImagesToCurrentUnit.Enabled = false;
        //    }
        //    catch
        //    {
        //        this.buttonRestrictImagesToCurrentUnit.Enabled = false;
        //    }
        //}

        //private void buttonRestrictImagesToCurrrentPart_Click(object sender, EventArgs e)
        //{
        //    // Reset the property filter
        //    this.toolStripTextBoxImagePropertyFilter.Text = "";

        //    System.Data.DataRowView RImage = (System.Data.DataRowView)this._Source.Current;
        //    if (RImage != null)
        //    {
        //        try
        //        {
        //            RImage.BeginEdit();
        //            RImage.EndEdit();
        //        }
        //        catch { }
        //    }
        //    this.FormFunctions.updateTable(this._iMainForm.DataSetCollectionSpecimen(), "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this._Source);

        //    this.listBoxSpecimenImage.Items.Clear();
        //    this.userControlImageSpecimenImage.ImagePath = "";
        //    this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Clear();
        //    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionSpecimenPartBindingSource.Current;
        //    int SpecimenPartID;
        //    if (int.TryParse(R["SpecimenPartID"].ToString(), out SpecimenPartID))
        //    {
        //        string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString() +
        //            " AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable) " +
        //            " AND SpecimenPartID = " + SpecimenPartID.ToString();
        //        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage);
        //        this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
        //        this.toolStripButtonImagesSpecimenShowAll.Visible = true;
        //        this.buttonRestrictImagesToCurrrentPart.BackColor = System.Drawing.Color.Red;
        //    }
        //}

        //private void EnableRestrictImageToPartButton(System.Data.DataRow R)
        //{
        //    int SpecimenPartID;
        //    try
        //    {
        //        this.buttonRestrictImagesToCurrrentPart.FlatAppearance.BorderSize = 0;
        //        if (int.TryParse(R["SpecimenPartID"].ToString(), out SpecimenPartID))
        //        {
        //            System.Data.DataRow[] rr = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("SpecimenPartID = " + SpecimenPartID.ToString());
        //            if (rr.Length > 0)
        //                this.buttonRestrictImagesToCurrrentPart.Enabled = true;
        //            else this.buttonRestrictImagesToCurrrentPart.Enabled = false;
        //        }
        //    }
        //    catch
        //    {
        //        this.buttonRestrictImagesToCurrrentPart.Enabled = false;
        //    }
        //}

        //private void buttonRestrictImagesToCurrentUnit_Click(object sender, EventArgs e)
        //{
        //    // Reset the property filter
        //    this.toolStripTextBoxImagePropertyFilter.Text = "";

        //    System.Data.DataRowView RImage = (System.Data.DataRowView)this._Source.Current;
        //    if (RImage != null)
        //    {
        //        try
        //        {
        //            RImage.BeginEdit();
        //            RImage.EndEdit();
        //        }
        //        catch { }
        //    }
        //    this.FormFunctions.updateTable(this._iMainForm.DataSetCollectionSpecimen(), "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this._Source);

        //    this.listBoxSpecimenImage.Items.Clear();
        //    this.userControlImageSpecimenImage.ImagePath = "";
        //    this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Clear();
        //    System.Data.DataRowView R = (System.Data.DataRowView)this._iMainForm.ID_Specimen()entificationUnitBindingSource.Current;
        //    int UnitID;
        //    if (int.TryParse(R["IdentificationUnitID"].ToString(), out UnitID))
        //    {
        //        string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString() +
        //            " AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable) " +
        //            " AND IdentificationUnitID = " + UnitID.ToString();
        //        this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage);
        //        this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
        //        this.toolStripButtonImagesSpecimenShowAll.Visible = true;
        //        this.buttonRestrictImagesToCurrentUnit.BackColor = System.Drawing.Color.Red;
        //    }
        //}

        #endregion

        #endregion

    }
}
