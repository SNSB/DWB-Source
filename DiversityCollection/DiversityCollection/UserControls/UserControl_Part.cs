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
    public partial class UserControl_Part : UserControl__Data
    {

        #region Construction

        public UserControl_Part(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
                    //System.Windows.Forms.BindingSource SourcePartDescription,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this._Source = Source;
            //this._SourcePartDescription = SourcePartDescription;
            this._HelpNamespace = HelpNamespace;
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region public interface

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.pictureBoxSpecimenPart.Image = DiversityCollection.Specimen.ImageList.Images[this._iMainForm.SelectedPartHierarchyNode().ImageIndex];
            this.textBoxPartWithhold_TextChanged(null, null);
            this.groupBoxPart.Text = this._iMainForm.SelectedPartHierarchyNode().Text;
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryPreparationResponsible, "ResponsibleAgentURI");
            this.setCollectionSource();
            this.setDate();
        }

        public void ReleaseRestriction()
        {
            this.buttonRestrictImagesToCurrrentPart.BackColor = System.Drawing.SystemColors.Control;
        }

        public void EnableImageRestriction(bool Enable)
        {
            this.buttonRestrictImagesToCurrrentPart.Enabled = Enable;
        }

        #endregion

        #region Private control events

        private void initControl()
        {
            this.comboBoxStorageContainer.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "StorageContainer", true));
            this.comboBoxStockUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "StockUnit", true));
            this.comboBoxMaterialCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "MaterialCategory", true));
            this.comboBoxCollection.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this._Source, "CollectionID", true));

            //this.initEnumCombobox(this.comboBoxMaterialCategory, "CollMaterialCategory_Enum");
            this._EnumComboBoxes = new Dictionary<ComboBox, string>();
            this._EnumComboBoxes.Add(this.comboBoxMaterialCategory, "CollMaterialCategory_Enum");
            this.InitLookupSources();

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxPreparationMethod);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxStorageLocation);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxStorageNotes);

            this.textBoxSpecimenPartID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "SpecimenPartID", true));
            this.textBoxStorageNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxStock.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Stock", true));
            this.textBoxStorageLocation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "StorageLocation", true));
            this.textBoxAccessionNumberPart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "AccessionNumber", true));
            this.textBoxPartSublabel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "PartSublabel", true));
            this.textBoxPreparationMethod.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "PreparationMethod", true));
            this.textBoxPartWithhold.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "DataWithholdingReason", true));

            //this.textBoxPreparationDate.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "PreparationDate", true));
            this.dateTimePickerPreparationDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._Source, "PreparationDate", true));

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxPartSublabel);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxStorageLocation);
            //DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.textBoxPreparationMethod);

            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryPreparationResponsible, A, "CollectionSpecimenPart", "ResponsibleName", "ResponsibleAgentURI", this._Source);

            this.setCollectionSource();

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        public override void InitLookupSources() { this.InitEnums(); }

        public void setCollectionSource(bool Reset = false)
        {
            try
            {
                if (DiversityCollection.LookupTable.DtCollectionWithHierarchy != null && (this.comboBoxCollection.DataSource == null || Reset))
                {
                    int DropDownWithForCollection = 360;
                    System.Data.DataTable dtCollection = DiversityCollection.LookupTable.DtCollectionWithHierarchy.Copy(); // aufruf bei start als 2. aus setDatabase() Zeile 3696 und nochmal als 3. aus setDatabindings() Zeile 2433
                    this.comboBoxCollection.DataSource = dtCollection;
                    this.comboBoxCollection.DisplayMember = "DisplayText";
                    this.comboBoxCollection.ValueMember = "CollectionID";
                    this.comboBoxCollection.DropDownWidth = DropDownWithForCollection;

                    System.Data.DataTable DtCollectionForPart = DiversityCollection.LookupTable.DtCollectionWithHierarchy.Copy();
                    this.userControlHierarchySelectorCollection.initHierarchy(
                        DtCollectionForPart,
                        "CollectionID",
                        "CollectionParentID",
                        "CollectionName",
                        "CollectionName",
                        "CollectionID",
                        true,
                        this._Source, true);
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxStorageContainer_DropDown(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            string Container = "";
            if (!R["StorageContainer"].Equals(System.DBNull.Value) && R["StorageContainer"].ToString().Length > 0)
                Container = R["StorageContainer"].ToString();
            string SQL = "SELECT DISTINCT SP.StorageContainer " +
                "FROM CollectionSpecimenPart AS SP INNER JOIN " +
                "CollectionProject AS P ON SP.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN " +
                "ProjectList AS L ON P.ProjectID = L.ProjectID ";
            if (this._iMainForm.ProjectID() != null) //.userControlQueryList.ProjectID != -1)
                SQL += "WHERE L.ProjectID = " + this._iMainForm.ProjectID() /*.userControlQueryList.ProjectID.ToString()*/ + " AND SP.StorageContainer <> '' ";
            SQL += " ORDER BY SP.StorageContainer";
            System.Data.DataTable dt = new DataTable();
            if (Container.Length > 0)
            {
                System.Data.DataColumn C = new DataColumn("StorageContainer", System.Type.GetType("System.String"));
                dt.Columns.Add(C);
                System.Data.DataRow r = dt.NewRow();
                r[0] = Container;
                dt.Rows.Add(r);
                //SQL = "SELECT '" + Container + "' AS StorageContainer UNION " + SQL;
            }
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                this.comboBoxStorageContainer.DataSource = dt;
                this.comboBoxStorageContainer.ValueMember = "StorageContainer";
                this.comboBoxStorageContainer.DisplayMember = "StorageContainer";
            }
            else
                this.comboBoxStorageContainer.DataSource = null;
        }

        #region Stock

        private void textBoxStock_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxStock.Text.Length == 0)
            {
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    R.BeginEdit();
                    R["Stock"] = System.DBNull.Value;
                    R.EndEdit();
                }
                this.textBoxStorageNotes.Focus();
            }
            else
            {
                double Stock;
                if (!double.TryParse(this.textBoxStock.Text, out Stock))
                {
                    System.Windows.Forms.MessageBox.Show(this.textBoxStock.Text + " is not a numeric value");
                }
            }
        }

        private void comboBoxStockUnit_DropDown(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            string StockUnit = "";
            if (!R["StockUnit"].Equals(System.DBNull.Value) && R["StockUnit"].ToString().Length > 0)
                StockUnit = R["StockUnit"].ToString();
            string SQL = "SELECT DISTINCT SP.StockUnit " +
                "FROM CollectionSpecimenPart AS SP INNER JOIN " +
                "CollectionProject AS P ON SP.CollectionSpecimenID = P.CollectionSpecimenID INNER JOIN " +
                "ProjectList AS L ON P.ProjectID = L.ProjectID ";
            if (this._iMainForm.ProjectID() != null)// != -1)
                SQL += "WHERE L.ProjectID = " + this._iMainForm.ProjectID().ToString();
            SQL += " ORDER BY SP.StockUnit";
            System.Data.DataTable dt = new DataTable();
            if (StockUnit.Length > 0)
            {
                System.Data.DataColumn C = new DataColumn("StockUnit", System.Type.GetType("System.String"));
                dt.Columns.Add(C);
                System.Data.DataRow r = dt.NewRow();
                r[0] = StockUnit;
                dt.Rows.Add(r);
            }
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                this.comboBoxStockUnit.DataSource = dt;
                this.comboBoxStockUnit.ValueMember = "StockUnit";
                this.comboBoxStockUnit.DisplayMember = "StockUnit";
            }
            else
                this.comboBoxStockUnit.DataSource = null;
        }

        private void buttonStockHistoryInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                    string PartID = RV["SpecimenPartID"].ToString();
                    string SQL = "SELECT P.Stock, P.StockUnit, getdate() AS [Date], P.LogUpdatedBy AS [User] " +
                        "FROM CollectionSpecimenPart AS P " +
                        "WHERE (P.SpecimenPartID = " + PartID + ") " +
                        "UNION " +
                        "SELECT L.Stock, L.StockUnit, L.LogDate AS [Date], case when U.CombinedNameCache is null or L.LogUser = 'dbo' then L.LogUser else U.CombinedNameCache end AS [User] " +
                        "FROM CollectionSpecimenPart AS P INNER JOIN " +
                        "CollectionSpecimenPart_log AS L ON P.CollectionSpecimenID = L.CollectionSpecimenID AND P.SpecimenPartID = L.SpecimenPartID LEFT OUTER JOIN " +
                        "UserProxy AS U ON L.LogUser = U.LoginName " +
                        "WHERE (NOT (L.Stock IS NULL)) AND (L.SpecimenPartID = " + PartID + ") " +
                        "ORDER BY [Date]";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.Connection);
                    ad.Fill(dt);
                    System.Windows.Forms.Form F = new Form();
                    F.Text = "Volume history of the selected part";
                    F.Width = 500;
                    F.Height = 200;
                    System.Windows.Forms.DataGridView G = new DataGridView();
                    F.Controls.Add(G);
                    G.DataSource = dt;
                    G.Dock = DockStyle.Fill;
                    //G.Enabled = false;
                    G.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Forms.FormCollectionSpecimen));
                    F.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
                    G.AllowUserToAddRows = false;
                    G.RowHeadersVisible = false;
                    G.AllowUserToDeleteRows = false;
                    foreach (System.Windows.Forms.DataGridViewColumn C in G.Columns)
                    {
                        C.ReadOnly = true;
                        C.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    F.StartPosition = FormStartPosition.CenterParent;
                    F.ShowDialog();
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        
        #endregion

        private void buttonFindNextAccessionNumberPart_Click(object sender, EventArgs e)
        {
            try
            {
                string AccessionNumber = this.textBoxAccessionNumberPart.Text;
                //if (AccessionNumber.Length == 0)
                //    AccessionNumber = this.textBoxAccessionNumber.Text;
                DiversityCollection.Forms.FormAccessionNumber f = new Forms.FormAccessionNumber(AccessionNumber, false, true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (f.AccessionNumber.Length > 0)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                        R["AccessionNumber"] = f.AccessionNumber;
                        this.textBoxAccessionNumberPart.Text = f.AccessionNumber;
                    }
                }
            }
            catch (System.Exception ex) { }
        }

        private void buttonSetMaterialCategoryRange_Click(object sender, EventArgs e)
        {
            this._iMainForm.CustomizeDisplay(Forms.FormCustomizeDisplay.Customization.MaterialCategory);
        }

        private void textBoxSpecimenPartID_ReadOnlyChanged(object sender, EventArgs e)
        {
            if (!this.textBoxSpecimenPartID.ReadOnly)
                this.textBoxSpecimenPartID.ReadOnly = true;
        }

        private void buttonRestrictImagesToCurrrentPart_Click(object sender, EventArgs e)
        {
            int SpecimenPartID;
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            if (int.TryParse(R["SpecimenPartID"].ToString(), out SpecimenPartID))
            {
                this._iMainForm.RestrictImagesToPart(SpecimenPartID);
                this.buttonRestrictImagesToCurrrentPart.BackColor = System.Drawing.Color.Red;
            }

            // Reset the property filter
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
            //this.FormFunctions.updateTable(this._iMainForm.DataSetCollectionSpecimen(), "CollectionSpecimenImage", this.sqlDataAdapterSpecimenImage, this.collectionSpecimenImageBindingSource);

            //this.listBoxSpecimenImage.Items.Clear();
            //this.userControlImageSpecimenImage.ImagePath = "";
            //this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage.Clear();
            //System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            //int SpecimenPartID;
            //if (int.TryParse(R["SpecimenPartID"].ToString(), out SpecimenPartID))
            //{
            //    string WhereClause = " WHERE CollectionSpecimenID = " + SpecimenID.ToString() +
            //        " AND CollectionSpecimenID IN (SELECT CollectionSpecimenID FROM CollectionSpecimenID_UserAvailable) " +
            //        " AND SpecimenPartID = " + SpecimenPartID.ToString();
            //    this.FormFunctions.initSqlAdapter(ref this.sqlDataAdapterSpecimenImage, DiversityCollection.CollectionSpecimen.SqlSpecimenImage + WhereClause + "ORDER BY LogCreatedWhen DESC", this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage);
            //    this.FormFunctions.FillImageList(this.listBoxSpecimenImage, this.imageListSpecimenImages, this.imageListForm, this._iMainForm.DataSetCollectionSpecimen().CollectionSpecimenImage, "URI", this.userControlImageSpecimenImage);
            //    this.toolStripButtonImagesSpecimenShowAll.Visible = true;
            //    this.buttonRestrictImagesToCurrrentPart.BackColor = System.Drawing.Color.Red;
            //}
        }

        #region Storage location

        private void buttonSetStorageLocationSource_Click(object sender, EventArgs e)
        {
            this._iMainForm.CustomizeDisplay(Forms.FormCustomizeDisplay.Customization.StorageLocation);
        }

        private void comboBoxStorageLocation_DropDown(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> StorageList = new List<string>();
            StorageList.Add(this.textBoxStorageLocation.Text);
            try
            {
                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationSource == "Text")
                {
                    StorageList.Add(DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationText);
                }
                else if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationSource == "Taxa")
                {
                    foreach (System.Data.DataRow R in this._iMainForm.DataSetCollectionSpecimen().Identification.Rows)
                    {
                        if (!R["TaxonomicName"].Equals(System.DBNull.Value) && !StorageList.Contains(R["TaxonomicName"].ToString()))
                            StorageList.Add(R["TaxonomicName"].ToString());
                        if (!R["VernacularTerm"].Equals(System.DBNull.Value) && !StorageList.Contains(R["VernacularTerm"].ToString()))
                            StorageList.Add(R["VernacularTerm"].ToString());
                    }
                }
                else
                {
                    string CollectionID = "";
                    string SQL = "SELECT DISTINCT StorageLocation " +
                        "FROM CollectionSpecimenPart " +
                        "WHERE (StorageLocation <> '') ";
                    System.Data.DataRowView RV = (System.Data.DataRowView)this._Source.Current;
                    if (!RV["StorageLocation"].Equals(System.DBNull.Value) && RV["StorageLocation"].ToString().Length > 0)
                        SQL += " AND StorageLocation <> '" + RV["StorageLocation"].ToString() + "'";
                    if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.StorageLocationSource == "Collection")
                    {
                        if (!RV["CollectionID"].Equals(System.DBNull.Value))
                            CollectionID = RV["CollectionID"].ToString();
                        if (CollectionID.Length > 0)
                            SQL += " AND CollectionID = " + CollectionID;
                    }
                    SQL += " ORDER BY StorageLocation";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    foreach (System.Data.DataRow r in dt.Rows)
                        StorageList.Add(r[0].ToString());
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.comboBoxStorageLocation.DataSource = StorageList;
        }

        private void comboBoxStorageLocation_SelectionChangeCommitted(object sender, EventArgs e)
        {
            string StorageLocation = this.comboBoxStorageLocation.SelectedValue.ToString();
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R.BeginEdit();
            R["StorageLocation"] = StorageLocation;
            R.EndEdit();
            this.textBoxStorageLocation.Text = this.comboBoxStorageLocation.SelectedValue.ToString(); //StorageLocation;
            this._iMainForm.setSpecimen();
            //if (this.treeViewOverviewHierarchyStorage.SelectedNode != null)
            //{
            //    DiversityCollection.HierarchyNode N = (DiversityCollection.HierarchyNode)this.treeViewOverviewHierarchyStorage.SelectedNode;
            //    N.setText();
            //}
        }

        #endregion

        #region Storage notes

        private void comboBoxStorageNotes_SelectionChangeCommitted(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R.BeginEdit();
            R["Notes"] = this.comboBoxStorageNotes.SelectedValue.ToString(); ;
            this.textBoxStorageNotes.Text = this.comboBoxStorageNotes.SelectedValue.ToString();
            R.EndEdit();
        }

        private void comboBoxStorageNotes_DropDown(object sender, EventArgs e)
        {
            this.comboBoxStorageNotes.DropDownWidth = this.textBoxStorageNotes.Width;
            System.Data.DataTable dt = new DataTable();
            string SQL = "SELECT DISTINCT CollectionSpecimenPart.Notes AS Notes " +
                "FROM CollectionSpecimenPart INNER JOIN " +
                "CollectionSpecimenID_UserAvailable ON " +
                "CollectionSpecimenPart.CollectionSpecimenID = CollectionSpecimenID_UserAvailable.CollectionSpecimenID " +
                "WHERE (CollectionSpecimenPart.Notes LIKE N'" + this.textBoxStorageNotes.Text + "%') " +
                "ORDER BY CollectionSpecimenPart.Notes";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            this.comboBoxStorageNotes.DataSource = dt;
            this.comboBoxStorageNotes.DisplayMember = "Notes";
            this.comboBoxStorageNotes.ValueMember = "Notes";
        }

        #endregion

        private void dateTimePickerPreparationDate_CloseUp(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            System.DateTime DT;
            if (System.DateTime.TryParse(this.dateTimePickerPreparationDate.Value.ToShortDateString(), out DT))
            { 
                R["PreparationDate"] = DT;
            }
            this.setDate();
        }

        private void setDate()
        {
            bool DateIsSet = false;
            try
            {
                if (this._Source.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                    if (!R["PreparationDate"].Equals(System.DBNull.Value))
                        DateIsSet = true;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            if (DateIsSet)
                this.dateTimePickerPreparationDate.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            else
                this.dateTimePickerPreparationDate.CustomFormat = "-";
        }

        private void buttonPreparationDateDelete_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R["PreparationDate"] = System.DBNull.Value;
            this.setDate();
        }

        #region Method
        private void comboBoxPreparationMethod_DropDown(object sender, EventArgs e)
        {
            this.comboBoxPreparationMethod.DropDownWidth = this.textBoxPreparationMethod.Width;
            if (this._Source.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
                if (!R["MaterialCategory"].Equals(System.DBNull.Value))
                {
                    string MaterialCategory = R["MaterialCategory"].ToString();
                    string SQL = "SELECT DISTINCT PreparationMethod FROM CollectionSpecimenPart WHERE MaterialCategory = '" + MaterialCategory + "' ";
                    if (this.textBoxPreparationMethod.Text.Length > 0)
                        SQL += " AND PreparationMethod LIKE '" + this.textBoxPreparationMethod.Text + "%' ";
                    SQL += "ORDER BY PreparationMethod";
                    System.Data.DataTable dt = new DataTable();
                    Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    this.comboBoxPreparationMethod.DataSource = dt;
                    this.comboBoxPreparationMethod.DisplayMember = "PreparationMethod";
                    this.comboBoxPreparationMethod.ValueMember = "PreparationMethod";
                }
            }
        }

        private void comboBoxPreparationMethod_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBoxPreparationMethod.SelectedValue == null) return;
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            R["PreparationMethod"] = this.comboBoxPreparationMethod.SelectedValue.ToString();
            this.textBoxPreparationMethod.Text = this.comboBoxPreparationMethod.SelectedValue.ToString();
        }

        #endregion

        #region Withhold

        private void textBoxPartWithhold_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxPartWithhold.AutoCompleteCustomSource == null
                || this.textBoxPartWithhold.AutoCompleteCustomSource.Count == 0)
            {
                try
                {
                    string SQL = "SELECT DISTINCT S.DataWithholdingReason " +
                        "FROM CollectionSpecimenPart AS S ";
                    if (this._iMainForm.ProjectID() != null)
                    {
                        SQL += " INNER JOIN CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID WHERE P.ProjectID = " +
                            this._iMainForm.ProjectID().ToString() + " AND ";
                    }
                    else
                        SQL += " WHERE ";
                    SQL += "S.DataWithholdingReason <> N'' ORDER BY S.DataWithholdingReason";
                    System.Data.DataTable dt = new DataTable();
                    string Message = "";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, ref Message);
                    System.Windows.Forms.AutoCompleteStringCollection Source = new AutoCompleteStringCollection();
                    foreach (System.Data.DataRow RP in dt.Rows)
                    {
                        Source.Add(RP["DataWithholdingReason"].ToString());
                    }
                    this.textBoxPartWithhold.AutoCompleteMode = AutoCompleteMode.Suggest;
                    this.textBoxPartWithhold.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    this.textBoxPartWithhold.AutoCompleteCustomSource = Source;
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            this.SetDataWithholdingControl(this.textBoxPartWithhold, this.pictureBoxWithholdPart);
        }

        private void textBoxPartWithhold_Leave(object sender, EventArgs e)
        {
            if (this.textBoxPartWithhold.AutoCompleteCustomSource != null &&
                !this.textBoxPartWithhold.AutoCompleteCustomSource.Contains(this.textBoxPartWithhold.Text))
            {
                this.textBoxPartWithhold.AutoCompleteCustomSource.Add(this.textBoxPartWithhold.Text);
            }
        }
        
        #endregion

        #region Template

        private void buttonTemplatePartSet_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.TemplateForData T = new DiversityWorkbench.TemplateForData("CollectionSpecimenPart", TemplatePartSuppressedColumns, TemplatePartSourceTables);
            System.Data.DataRowView R = (System.Data.DataRowView)this._Source.Current;
            T.CopyTemplateToRow(R.Row);
        }

        private System.Collections.Generic.List<string> TemplatePartSuppressedColumns
        {
            get
            {
                System.Collections.Generic.List<string> Suppress = new List<string>();
                Suppress.Add("CollectionSpecimenID");
                Suppress.Add("SpecimenPartID");
                Suppress.Add("CollectionID");
                Suppress.Add("DerivedFromSpecimenPartID");
                Suppress.Add("LogCreatedWhen");
                Suppress.Add("LogCreatedBy");
                Suppress.Add("LogUpdatedWhen");
                Suppress.Add("LogUpdatedBy");
                Suppress.Add("RowGUID");
                return Suppress;
            }
        }

        private System.Collections.Generic.Dictionary<string, DiversityWorkbench.TemplateForDataSourceTable> TemplatePartSourceTables
        {
            get
            {
                DiversityWorkbench.TemplateForDataSourceTable ST = new DiversityWorkbench.TemplateForDataSourceTable(LookupTable.DtCollection, "CollectionName", "CollectionID");
                System.Collections.Generic.Dictionary<string, DiversityWorkbench.TemplateForDataSourceTable> SourceTables = new Dictionary<string, DiversityWorkbench.TemplateForDataSourceTable>();
                SourceTables.Add("CollectionID", ST);
                return SourceTables;
            }
        }

        private void buttonTemplatePartEdit_Click(object sender, EventArgs e)
        {
            System.Data.DataRow R = ((System.Data.DataRowView)this._Source.Current).Row;
            DiversityWorkbench.Forms.FormTemplateEditor f = new DiversityWorkbench.Forms.FormTemplateEditor("CollectionSpecimenPart", R, this.TemplatePartSuppressedColumns, this.TemplatePartSourceTables);
            f.setHelp("Template");
            f.ShowDialog();
        }

        #endregion

        #region Obsolete - not used any more, replaced by PartDescription

        private void comboBoxIdentificationUnitInPartDescription_DropDown(object sender, EventArgs e)
        {
            try
            {
                this.comboBoxIdentificationUnitInPartDescription.DataSource = null;
                System.Data.DataRow R = (System.Data.DataRow)this._iMainForm.SelectedPartHierarchyNode().Tag;
                int UnitID;
                int PartID;
                if (!int.TryParse(R["IdentificationUnitID"].ToString(), out UnitID)) return;
                if (!int.TryParse(R["SpecimenPartID"].ToString(), out PartID)) return;
                string SQL = "SELECT TaxonomicGroup " +
                    "FROM IdentificationUnit AS U " +
                    "WHERE IdentificationUnitID = " + UnitID.ToString();
                string TaxonomicGroup = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL); ;

                SQL = "SELECT DISTINCT I.Description " +
                    "FROM IdentificationUnitInPart AS I INNER JOIN " +
                    "IdentificationUnit AS U ON I.CollectionSpecimenID = U.CollectionSpecimenID AND I.IdentificationUnitID = U.IdentificationUnitID ";
                if (this._iMainForm.ProjectID() != 0)
                    SQL += " INNER JOIN CollectionProject AS P ON I.CollectionSpecimenID = P.CollectionSpecimenID";
                SQL += " WHERE U.TaxonomicGroup = N'" + TaxonomicGroup + "'";
                if (this._iMainForm.ProjectID() != 0)
                    SQL += " AND P.ProjectID = " + this._iMainForm.ProjectID().ToString();

                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                ad.Fill(dt);
                this.comboBoxIdentificationUnitInPartDescription.DataSource = dt;
                this.comboBoxIdentificationUnitInPartDescription.DisplayMember = "Description";
                this.comboBoxIdentificationUnitInPartDescription.ValueMember = "Description";
            }
            catch (System.Exception ex) { }
        }

        #endregion

        #endregion

        private void textBoxStorageNotes_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBoxStorageNotes_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void textBoxStorageNotes_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
