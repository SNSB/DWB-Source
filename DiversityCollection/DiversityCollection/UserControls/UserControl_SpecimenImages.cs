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
    public partial class UserControl_SpecimenImages : UserControl__Data
    {
        #region Parameter

        private System.Windows.Forms.BindingSource _SourceProperty;
        private System.Windows.Forms.BindingSource _SourcePart;
        private int? _RestrictTo_IdentificationUnitID = null;
        private int? _RestrictTo_SpecimenPartID = null;
        
        #endregion

        #region Construction

        public UserControl_SpecimenImages(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            System.Windows.Forms.BindingSource SourceProperty,
            System.Windows.Forms.BindingSource SourcePart,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._SourceProperty = SourceProperty;
            this._SourcePart = SourcePart;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Control

        private void initControl()
        {
            try
            {
                this.comboBoxSpecimenImageIdentificationUnitID.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "IdentificationUnitID", true));
                this.comboBoxSpecimenImageSpecimenPart.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "SpecimenPartID", true));

                this.comboBoxSpecimenImageType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "ImageType", true));

                //this.initEnumCombobox(this.comboBoxSpecimenImageType, "CollSpecimenImageType_Enum");
                this._EnumComboBoxes = new Dictionary<ComboBox, string>();
                this._EnumComboBoxes.Add(this.comboBoxSpecimenImageType, "CollSpecimenImageType_Enum");
                this.InitLookupSources();
                if (this.comboBoxSpecimenImageType.DataSource == null)
                {
                    this.initEnumCombobox(this.comboBoxSpecimenImageType, "CollSpecimenImageType_Enum");
                }

                this.comboBoxSpecimenImageWithholdingReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DataWithholdingReason", true));
                this.comboBoxSpecimenImageWithholdingReason_TextChanged(null, null);

                this.listBoxImageProperty.DataSource = this._SourceProperty;
                this.listBoxImageProperty.DisplayMember = "Property";

                this.textBoxSpecimenImageNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
                this.textBoxSpecimenImageTitle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Title", true));
                this.textBoxSpecimenImageInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "InternalNotes", true));
                this.textBoxSpecimenImageDisplayOrder.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DisplayOrder", true));
                this.textBoxSpecimenImageIPR.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "IPR", true));
                this.textBoxSpecimenImageCopyright.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "CopyrightStatement", true));
                this.textBoxSpecimenImageLicenseYear.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseYear", true));
                this.textBoxSpecimenImageLicense.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseType", true));
                this.textBoxImageLicenseURI.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseURI", true));
                this.textBoxImageLicenseNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "LicenseNotes", true));
                this.textBoxImagePropertyDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._SourceProperty, "Description", true));

                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntrySpecimenImageCreator, A, "CollectionSpecimenImage", "CreatorAgent", "CreatorAgentURI", this._Source);
                this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntrySpecimenImageLicenseHolder, A, "CollectionSpecimenImage", "LicenseHolder", "LicenseHolderAgentURI", this._Source);

                this.userControlXMLTreeExif.XML = "";

                this.userControlImageSpecimenImage.ImagePathLabel = "URI:";
                this.userControlImageSpecimenImage.AutorotationEnabled = true;

                this.FormFunctions.addRestrictLengthToTextboxes();
                this.FormFunctions.addEditOnDoubleClickToTextboxes();

                DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

                DiversityWorkbench.Entity.setEntity(this, this.toolTip);

                this.CheckIfClientIsUpToDate();

                // markus 20.1.2020 - vorlaeufig entfernt da gegenpart im UserControlImage noch nicht fertig und bislang keine Anforderung
                this.tabControlSpecimenImage.TabPages.Remove(this.tabPageSpecimenImageProperty);

                DiversityWorkbench.Settings.WebViewUsage(this.toolStripButtonUseWebView);
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public override void SetPosition(int Position = 0)
        {
            base.SetPosition(Position);
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntrySpecimenImageCreator, "CreatorAgentURI");
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntrySpecimenImageLicenseHolder, "LicenseHolderAgentURI");
        }


        public override void InitLookupSources() { this.InitEnums(); }
        
        #endregion   
     
        #region public interface

        public void ShowImages()
        {
            try
            {
                this.setSpecimenImagePartList();
                this.setSpecimenImageUnitList();
                if (this._RestrictTo_IdentificationUnitID != null)
                {
                    System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("IdentificationUnitID = " + this._RestrictTo_IdentificationUnitID.ToString());
                    this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, RR, "URI", this.userControlImageSpecimenImage);
                }
                else if (this._RestrictTo_SpecimenPartID != null)
                {
                    System.Data.DataRow[] RR = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Select("SpecimenPartID = " + this._RestrictTo_SpecimenPartID.ToString());
                    this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, RR, "URI", this.userControlImageSpecimenImage);
                }
                else
                    this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);

                if (this.listBoxSpecimenImage.SelectedIndex == -1)
                {
                    this.tabControlSpecimenImage.Enabled = false;
                    // Markus 12.4.2022: Muss separat gelöscht werden
                    this.userControlXMLTreeExif.XML = "";
                }
                else
                    this.tabControlSpecimenImage.Enabled = true;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public override void setAvailability()
        {
            base.setAvailability();
            if (!this._iMainForm.ReadOnly())
            {
                this.toolStripButtonSpecimenImageDelete.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.DELETE];
                this.toolStripButtonSpecimenImageNew.Enabled = this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.INSERT];
                this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelSpecimenImage, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
                this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelSpecimenImageIPR, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
                this.FormFunctions.setDataControlEnabled(this.userControlXMLTreeExif, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);

                this.FormFunctions.setDataControlEnabled(this.tableLayoutPanelImageProperty, this.TablePermissions()[DiversityWorkbench.Forms.FormFunctions.Permission.UPDATE]);
            }
        }


        public void ReleaseImageRestriction()
        {
            this._RestrictTo_SpecimenPartID = null;
            this._RestrictTo_IdentificationUnitID = null;
        }

        #endregion

        #region Units and Parts
        
        public void RestrictImagesToUnit(int IdentificationUnitID) 
        {
            if (this._RestrictTo_IdentificationUnitID == null || this._RestrictTo_IdentificationUnitID != IdentificationUnitID)
            {
                this._RestrictTo_IdentificationUnitID = IdentificationUnitID;
                this._RestrictTo_SpecimenPartID = null;
                this.toolStripButtonImagesSpecimenShowAll.Visible = true;
                this.ShowImages();
            }
        }
        public void RestrictImagesToPart(int SpecimenPartID) 
        {
            if (this._RestrictTo_SpecimenPartID == null || this._RestrictTo_SpecimenPartID != SpecimenPartID)
            {
                this._RestrictTo_SpecimenPartID = SpecimenPartID;
                this._RestrictTo_IdentificationUnitID = null;
                this.toolStripButtonImagesSpecimenShowAll.Visible = true;
                this.ShowImages();
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

        private void comboBoxSpecimenImageSpecimenPart_SelectionChangeCommitted(object sender, EventArgs e)
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
            catch (System.Exception ex) 
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

        }

        private void comboBoxSpecimenImageSpecimenPart_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void toolStripButtonImagesSpecimenShowAll_Click(object sender, EventArgs e)
        {
            this._RestrictTo_SpecimenPartID = null;
            this._RestrictTo_IdentificationUnitID = null;
            this._iMainForm.ReleaseImageRestriction();
            this.ShowImages();
        }

        private System.Data.DataTable _dtImageUnits;
        private System.Data.DataTable _dtImageParts;

        private void setSpecimenImageUnitList()
        {
            try
            {
                this.comboBoxSpecimenImageIdentificationUnitID.DataBindings.Clear();
                this.comboBoxSpecimenImageIdentificationUnitID.DataSource = null;
                this._dtImageUnits = new DataTable();
                string SQL = "SELECT NULL AS IdentificationUnitID, NULL AS LastIdentificationCache UNION " +
                    "SELECT IdentificationUnitID, LastIdentificationCache FROM IdentificationUnit U WHERE U.CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() +
                    " ORDER BY LastIdentificationCache";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtImageUnits, ref Message);
                if (this._dtImageUnits.Columns.Contains("IdentificationUnitID") && this.comboBoxSpecimenImageIdentificationUnitID.DataBindings.Count == 0)
                {
                    this.comboBoxSpecimenImageIdentificationUnitID.DataSource = this._dtImageUnits;
                    this.comboBoxSpecimenImageIdentificationUnitID.DisplayMember = "LastIdentificationCache";
                    this.comboBoxSpecimenImageIdentificationUnitID.ValueMember = "IdentificationUnitID";
                    this.comboBoxSpecimenImageIdentificationUnitID.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "IdentificationUnitID", true));
                }
                return;






                if (this._dtImageUnits == null)
                {
                    this._dtImageUnits = new DataTable();
                    System.Data.DataColumn C1 = new System.Data.DataColumn("IdentificationUnitID", System.Type.GetType("System.Int32"));
                    System.Data.DataColumn C2 = new System.Data.DataColumn("LastIdentificationCache", System.Type.GetType("System.String"));
                    _dtImageUnits.Columns.Add(C1);
                    _dtImageUnits.Columns.Add(C2);
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
                this.comboBoxSpecimenImageIdentificationUnitID.DataSource = this._dtImageUnits;
                this.comboBoxSpecimenImageIdentificationUnitID.DisplayMember = "LastIdentificationCache";
                this.comboBoxSpecimenImageIdentificationUnitID.ValueMember = "IdentificationUnitID";
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
                this.comboBoxSpecimenImageSpecimenPart.DataBindings.Clear();
                this.comboBoxSpecimenImageSpecimenPart.DataSource = null;
                this._dtImageParts = new DataTable();
                string SQL = "SELECT NULL AS SpecimenPartID, NULL AS StorageLocation UNION " +
                    "SELECT SpecimenPartID, " +
                    " CASE WHEN P.AccessionNumber IS NULL THEN '' ELSE AccessionNumber END + " +
                    " CASE WHEN P.AccessionNumber IS NULL AND P.PartSublabel IS NULL THEN '' ELSE ' ' END + " +
                    " CASE WHEN P.PartSublabel IS NULL THEN '' ELSE P.PartSublabel END + " +
                    " CASE WHEN (P.AccessionNumber IS NULL OR P.AccessionNumber = '') AND (P.PartSublabel IS NULL OR P.PartSublabel = '') THEN '' ELSE ' - ' END + " +
                    " CASE WHEN P.StorageLocation IS NULL THEN '' ELSE P.StorageLocation END AS StorageLocation " +
                    " FROM CollectionSpecimenPart P WHERE P.CollectionSpecimenID = " + this._iMainForm.ID_Specimen().ToString() +
                    " ORDER BY StorageLocation";
                string Message = "";
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._dtImageParts, ref Message);
                if (this._dtImageParts.Columns.Contains("SpecimenPartID") && this.comboBoxSpecimenImageSpecimenPart.DataBindings.Count == 0)
                {
                    this.comboBoxSpecimenImageSpecimenPart.DataSource = this._dtImageParts;
                    this.comboBoxSpecimenImageSpecimenPart.DisplayMember = "StorageLocation";
                    this.comboBoxSpecimenImageSpecimenPart.ValueMember = "SpecimenPartID";
                    this.comboBoxSpecimenImageSpecimenPart.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "SpecimenPartID", true));
                }

                return;






                if (this._dtImageParts == null)
                {
                    this._dtImageParts = new DataTable();
                    System.Data.DataColumn C1 = new System.Data.DataColumn("SpecimenPartID", System.Type.GetType("System.Int32"));
                    System.Data.DataColumn C2 = new System.Data.DataColumn("StorageLocation", System.Type.GetType("System.String"));
                    _dtImageParts.Columns.Add(C1);
                    _dtImageParts.Columns.Add(C2);
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
                this.comboBoxSpecimenImageSpecimenPart.DataSource = this._dtImageParts;
                this.comboBoxSpecimenImageSpecimenPart.DisplayMember = "StorageLocation";
                this.comboBoxSpecimenImageSpecimenPart.ValueMember = "SpecimenPartID";
                this.comboBoxSpecimenImageSpecimenPart.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "SpecimenPartID", true));
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Withholding
        
        private void comboBoxSpecimenImageWithholdingReason_DropDown(object sender, EventArgs e)
        {
            string SQL = "SELECT DISTINCT DataWithholdingReason FROM CollectionSpecimenImage ORDER BY DataWithholdingReason";
            System.Data.DataTable dt = new DataTable();
            try
            {
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxSpecimenImageWithholdingReason.DataSource = dt;
                this.comboBoxSpecimenImageWithholdingReason.DisplayMember = "DataWithholdingReason";
                this.comboBoxSpecimenImageWithholdingReason.ValueMember = "DataWithholdingReason";
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxSpecimenImageWithholdingReason_TextChanged(object sender, EventArgs e)
        {
            this.SetDataWithholdingControl(this.comboBoxSpecimenImageWithholdingReason, this.pictureBoxSpecimenImageWithholdingReason);
        }
        
        #endregion

        #region Property
        
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
                        this.userControlImageSpecimenImage.setMarkingWhereClause(" WHERE Collectionthis._iMainForm.ID_Specimen() = " + this._iMainForm.ID_Specimen().ToString() + " AND URI = '" + R["URI"].ToString() + "' AND Property = '" + R["Property"].ToString() + "'");
                        this.userControlImageSpecimenImage.LoadGeometry();
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

                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                string Header = "Please select the property from the list";
                string Message = "Please select the property from the list";
                if (AllowNewProperty)
                {
                    Message += " above\r\nor\r\nenter a new property in the text box below";
                    Header += " or enter a new one";
                }
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Property", "Property", "New image property", Message, "", AllowNewProperty);
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

        private enum ImagePropertyAreaDisplayStyle { All, Show, Hide };
        private ImagePropertyAreaDisplayStyle _ImagePropertyAreaDisplayStyle = ImagePropertyAreaDisplayStyle.Hide;

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
                    this.userControlImageSpecimenImage.setMarkingWhereClause(" WHERE Collectionthis._iMainForm.ID_Specimen() = " + this._iMainForm.ID_Specimen().ToString() + " AND URI = '" + RI["URI"].ToString() + "' AND Property = '" + RI["Property"].ToString() + "'");
                }
                else
                    this.userControlImageSpecimenImage.setMarkingWhereClause(" WHERE 1 = 0");
                this.userControlImageSpecimenImage.LoadImageForMarking();
            }
            else
            {
            }

            return;
        }

        private void toolStripMenuItemImagePropertyAreaHide_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonImagePropertyArea.ForeColor = System.Drawing.Color.Black;
            this.toolStripDropDownButtonImagePropertyArea.Text = "Hide";
            this.toolStripDropDownButtonImagePropertyArea.ToolTipText = this.toolStripMenuItemImagePropertyAreaHide.ToolTipText;
            this._ImagePropertyAreaDisplayStyle = ImagePropertyAreaDisplayStyle.Hide;
        }

        private void toolStripMenuItemImagePropertyAreaShow_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonImagePropertyArea.ForeColor = System.Drawing.Color.Red;
            this.toolStripDropDownButtonImagePropertyArea.Text = "Show";
            this.toolStripDropDownButtonImagePropertyArea.ToolTipText = this.toolStripMenuItemImagePropertyAreaShow.ToolTipText;
            this._ImagePropertyAreaDisplayStyle = ImagePropertyAreaDisplayStyle.Show;
        }

        private void toolStripMenuItemImagePropertyAreaAll_Click(object sender, EventArgs e)
        {
            this.toolStripDropDownButtonImagePropertyArea.ForeColor = System.Drawing.Color.Red;
            this.toolStripDropDownButtonImagePropertyArea.Text = "All";
            this.toolStripDropDownButtonImagePropertyArea.ToolTipText = this.toolStripMenuItemImagePropertyAreaAll.ToolTipText;
            this._ImagePropertyAreaDisplayStyle = ImagePropertyAreaDisplayStyle.All;
        }

        private void toolStripButtonImagePropertyDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxImageProperty.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxImageProperty.SelectedItem;
                R.Delete();
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
            this._iMainForm.saveSpecimen();
            this.toolStripButtonImagePropertySave.Visible = false;
            this.listBoxSpecimenImage.SelectedIndex = Pos;
            this.toolStripTextBoxImagePropertyFilter.AutoCompleteCustomSource = null;
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

        private void toolStripButtonImagePropertyFilter_Click(object sender, EventArgs e)
        {
            if (this.ImagePropertyFilter().Length == 0)
                System.Windows.Forms.MessageBox.Show("No filter defined");
            else
                this.RestrictImagesToPropertyFilter();
        }

        private void toolStripTextBoxImagePropertyFilter_Enter(object sender, EventArgs e)
        {
            if (this.toolStripTextBoxImagePropertyFilter.AutoCompleteCustomSource == null)
            {
                this.toolStripTextBoxImagePropertyFilter.AutoCompleteMode = AutoCompleteMode.Suggest;
                this.toolStripTextBoxImagePropertyFilter.AutoCompleteSource = AutoCompleteSource.CustomSource;
                System.Windows.Forms.AutoCompleteStringCollection Source = new AutoCompleteStringCollection();
                foreach (System.Data.DataRow RP in this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImageProperty.Rows)
                {
                    Source.Add(RP["Property"].ToString());
                }
                this.toolStripTextBoxImagePropertyFilter.AutoCompleteCustomSource = Source;
            }
        }

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

        private void RestrictImagesToPropertyFilter()
        {
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
            this._iMainForm.saveSpecimen();
            //this.FormFunctions.updateTable(this._iMainForm.DataSetCollectionSpecimen(), "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this._Source);

            this.listBoxSpecimenImage.Items.Clear();
            this.userControlImageSpecimenImage.ImagePath = "";
            this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Clear();
            System.Data.DataRowView R = (System.Data.DataRowView)this._SourcePart.Current;
            string PropertyFilter = this.ImagePropertyFilter();
            if (PropertyFilter.Length > 0)
            {
                string WhereClause = " WHERE Collectionthis._iMainForm.ID_Specimen() = " + this._iMainForm.ID_Specimen().ToString() +
                    " AND Collectionthis._iMainForm.ID_Specimen() IN (SELECT Collectionthis._iMainForm.ID_Specimen() FROM Collectionthis._iMainForm.ID_Specimen()_UserAvailable) " +
                    " AND URI IN (SELECT I.URI FROM CollectionSpecimenImage AS I " +
                    "INNER JOIN CollectionSpecimenImageProperty AS P ON I.Collectionthis._iMainForm.ID_Specimen() = P.Collectionthis._iMainForm.ID_Specimen() AND I.URI = P.URI " +
                    "WHERE (I.Collectionthis._iMainForm.ID_Specimen() = " + this._iMainForm.ID_Specimen().ToString() + ") AND (P.Property LIKE '" + PropertyFilter + "'))";
                //this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage);
                this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
                this.toolStripButtonImagesSpecimenShowAll.Visible = true;
            }
        }

        private void toolStripButtonImagePropertyFilterClear_Click(object sender, EventArgs e)
        {
            if (this.toolStripTextBoxImagePropertyFilter.Text.Length > 0)
                this.toolStripButtonImagesSpecimenShowAll_Click(null, null);
        }
        
        private System.Data.DataView _dvImageProperty;

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
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            System.Data.DataTable dt = new DataTable();
            try
            {
                ad.Fill(dt);
            }
            catch (System.Exception ex) { }
            return dt;
        }


        #endregion

        #region toolstrip for image handling

        private void toolStripButtonSpecimenImageNew_Click(object sender, EventArgs e)
        {
            try
            {
                string AccessionNumber = "";
                if (!this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].Equals(System.DBNull.Value))
                    AccessionNumber = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimen.Rows[0]["AccessionNumber"].ToString();
                int? ProjectID = this._iMainForm.ProjectID();// null;
                //if (this.userControlQueryList.ProjectIsSelected)
                //    ProjectID = this.userControlQueryList.ProjectID;
                string RowGUID = System.Guid.NewGuid().ToString();
                DiversityWorkbench.Forms.FormGetImage f = new DiversityWorkbench.Forms.FormGetImage(ProjectID, AccessionNumber);
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
        
        #endregion

        #region Description

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

        #region Display order
        
        private void textBoxSpecimenImageDisplayOrder_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.textBoxSpecimenImageDisplayOrder.Text.Length == 0)
                {
                    this.textBoxSpecimenImageDisplayOrder.BackColor = System.Drawing.Color.Pink;
                    if (this._Source.Current != null)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                        R.BeginEdit();
                        R["DisplayOrder"] = System.DBNull.Value;
                        R.EndEdit();
                    }
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
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

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
                this._iMainForm.saveSpecimen();
                //this.setSpecimen(this.this._iMainForm.ID_Specimen());
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
                this._iMainForm.saveSpecimen();
                //this.setSpecimen(this.this._iMainForm.ID_Specimen());
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
                this._iMainForm.saveSpecimen();
                //this.setSpecimen(this.this._iMainForm.ID_Specimen());
            }
            catch (System.Exception ex) { }
        }
        
        #endregion

        #region Image list
        
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
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
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
                    if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[p].RowState == System.Data.DataRowState.Deleted) 
                        i++;
                    if (this._RestrictTo_SpecimenPartID != null)
                    {
                        if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[p]["SpecimenPartID"].Equals(System.DBNull.Value))
                            i++;
                        else
                        {
                            if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[p]["SpecimenPartID"].ToString() != this._RestrictTo_SpecimenPartID.ToString())
                                i++;
                        }
                    }
                    else if (this._RestrictTo_IdentificationUnitID != null)
                    {
                        if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[p]["IdentificationUnitID"].Equals(System.DBNull.Value))
                            i++;
                        else
                        {
                            if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[p]["IdentificationUnitID"].ToString() != this._RestrictTo_IdentificationUnitID.ToString())
                                i++;
                        }
                    }
                }
                if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count > i)
                {
                    this.tabControlSpecimenImage.Enabled = true;
                    System.Data.DataRow r = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[i];
                    if (!r["URI"].Equals(System.DBNull.Value))
                    this.userControlImageSpecimenImage.ImagePath = r["URI"].ToString();
                    this._Source.Position = i;

                    //Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                    //string Restriction = "";//(Code LIKE 'audio%' OR Code LIKE 'video%' OR Code LIKE 'sound%')";
                    //if (this.userControlImageSpecimenImage.MediumType == DiversityWorkbench.Forms.FormFunctions.Medium.Image)
                    //    Restriction = "NOT " + Restriction;
                    this.setImagePropertyList();
                    //this.listBoxImageProperty_SelectedIndexChanged(null, null);
                    ///##2
                    string XML = r["Description"].ToString();
                    this.userControlXMLTreeExif.setToDisplayOnly();
                    this.userControlXMLTreeExif.XML = XML;

//#if DEBUG
//#endif
                    // Rotate if EXIF File contains info about orientiation of image
                    try
                    {
                        if (this.userControlImageSpecimenImage.AutorotationEnabled && this.userControlImageSpecimenImage.Autorotate)
                        {
                            System.Drawing.RotateFlipType Rotate = DiversityWorkbench.Forms.FormFunctions.ExifRotationInfo(XML); 
                            if (Rotate != RotateFlipType.RotateNoneFlipNone)
                                this.userControlImageSpecimenImage.RotateImage(Rotate);
                        }
                    }
                    catch(System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }

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

                return;




                if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count > 0)
                {
                    if (!this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[0]["SpecimenPartID"].Equals(System.DBNull.Value))
                    {
                        System.Data.DataRow[] rv = this._dtImageParts.Select("SpecimenPartID = " + this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[0]["SpecimenPartID"].ToString());
                        if (rv.Length > 0)
                            this.comboBoxSpecimenImageSpecimenPart.Text = rv[0]["StorageLocation"].ToString();
                    }
                    if (!this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[0]["IdentificationUnitID"].Equals(System.DBNull.Value))
                    {
                        System.Data.DataRow[] ru = this._dtImageUnits.Select("IdentificationUnitID = " + this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[0]["IdentificationUnitID"].ToString());
                        try
                        {
                            if (ru.Length > 0)
                                this.comboBoxSpecimenImageIdentificationUnitID.Text = ru[0]["LastIdentificationCache"].ToString();
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message, "Error setting the identification unit of the image", System.Windows.Forms.MessageBoxButtons.OK);
                        }
                    }
                    if (this.ImagePropertyFilter().Length > 0)
                        this.RestrictImagesToPropertyFilter();
                }

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void toolStripButtonUseWebView_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Settings.WebViewUsage(!DiversityWorkbench.Settings.UseWebView, this.toolStripButtonUseWebView);
            if (this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows.Count > 0)
            {
                System.Data.DataRow r = this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Rows[0];
                this.userControlImageSpecimenImage.ImagePath = r["URI"].ToString();
            }
        }

        #endregion

        }
}
