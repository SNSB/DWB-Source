using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormImageGrid : Form
    {

        #region Parameter and private properties

        private string _sIDs;
        private System.Collections.Generic.List<int> _IDs;
        private string _DataTable;
        private int _SpecimenID = 0;
        private int _ProjectID;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCollectionSpecimenImage;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdaptercollSpecimenImageType_Enum;

        private System.Collections.Generic.Dictionary<string, string> _BlockedColumns;
        private System.Collections.Generic.Dictionary<string, string> _RemoveColumns;
        private System.Collections.Generic.List<string> _ReadOnlyColumns;

        private DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState _ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace;

        private DiversityWorkbench.Forms.FormFunctions _FormFunctions;
        private DiversityWorkbench.Forms.FormFunctions FormFunctions
        {
            get
            {
                if (this._FormFunctions == null)
                    this._FormFunctions = new DiversityWorkbench.Forms.FormFunctions(this, DiversityWorkbench.Settings.ConnectionString, ref this.toolTip);
                return this._FormFunctions;
            }
        }

        private System.Collections.Generic.List<DiversityCollection.Forms.GridModeQueryField> _GridModeQueryFields;

        #endregion

        #region Interface
        
        public int SpecimenID { get { return this._SpecimenID; } }

        public int? CollectionSpecimenID
        {
            get
            {
                return this._SpecimenID;
            }
        }
        
        #endregion

        #region Construction

        public FormImageGrid(System.Collections.Generic.List<int> IDs, int ProjectID)
        {
            if (DiversityWorkbench.Settings.ConnectionString.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please connect to a database");
                this.Close();
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            
            InitializeComponent();
            this._ProjectID = ProjectID;
            this._IDs = IDs;
            for (int i = 0; i < _IDs.Count; i++)
            {
                if (i > 0) _sIDs += ", ";
                this._sIDs += _IDs[i].ToString();
            }
            this.initForm();
            //this.userControlXMLTree.initControl(true, true, false, false, false);
            this.Cursor = System.Windows.Forms.Cursors.Default;

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiversityCollection.Forms.FormCollectionSpecimenGridModeText));
            DiversityWorkbench.Entity.setEntity(this, resources);

        }
        
        #endregion

        #region Form

        private void initForm()
        {
            string TableName = "CollectionSpecimenImage";
            bool OK = this.FormFunctions.getObjectPermissions(TableName, "UPDATE");
            if (!OK)
            {
                this.dataGridView.ReadOnly = true;
                this.dataGridView.AllowUserToAddRows = false;
                this.dataGridView.AllowUserToDeleteRows = false;
                this.tableLayoutPanelGridModeParameter.Visible = false;
            }

            string SQL = "SELECT CollectionSpecimenID, URI, ResourceURI, SpecimenPartID, IdentificationUnitID, ImageType, Notes, DataWithholdingReason, Description, Title, IPR, CreatorAgent, " +
                "CreatorAgentURI, CopyrightStatement, LicenseType, LicenseHolder, LicenseHolderAgentURI, LicenseYear, InternalNotes, DisplayOrder, LogCreatedWhen, " +
                "LogCreatedBy, LogUpdatedWhen, LogUpdatedBy " +
                "FROM CollectionSpecimenImage WHERE CollectionSpecimenID IN (" + this._sIDs + ") " +
                "ORDER BY URI, CollectionSpecimenID";
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapterCollectionSpecimenImage, this.dataSetImageGrid.CollectionSpecimenImage, SQL, DiversityWorkbench.Settings.ConnectionString);

            this.tabControlDescription.TabPages.Remove(this.tabPageDescription);
            this.tabControlDescription.TabPages.Remove(this.tabPageTemplate);
            this.tabControlDescription.TabPages.Remove(this.tabPageProperties);
            this.userControlXMLTreeExif.setToDisplayOnly();

            SQL = "SELECT Code, DisplayText FROM CollSpecimenImageType_Enum WHERE (DisplayEnable = 1) ORDER BY DisplayText";
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdaptercollSpecimenImageType_Enum, this.dataSetImageGrid.CollSpecimenImageType_Enum, SQL, DiversityWorkbench.Settings.ConnectionString);

            
            //this.collSpecimenImageType_EnumTableAdapter.Adapter.SelectCommand.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            //this.collSpecimenImageType_EnumTableAdapter.Fill(this.dataSetImageGrid.CollSpecimenImageType_Enum);


            //SQL = "SELECT COUNT(*) " +
            //    "FROM CollectionSpecimenImage WHERE CollectionSpecimenID IN (" + this._sIDs + ") AND Description IS NULL OR CAST(Description AS nvarchar(max)) = '' ";
            //int MissingDescriptions = int.Parse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL));
            //if (MissingDescriptions == 0)
            //{
            //    this.tabControlDescription.TabPages.Remove(this.tabPageTemplate);
            //}
            //else
            //{
            //    SQL = "SELECT     CAST(ImageDescriptionTemplate AS nvarchar(MAX)) AS ImageDescriptionTemplate " +
            //        "FROM         ProjectProxy AS P " +
            //        "WHERE     ProjectID = " + this._ProjectID.ToString();
            //    string DescriptionTemplate = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //    this.userControlXMLTreeProjectTemplate.setToDisplayOnly();
            //    this.userControlXMLTreeProjectTemplate.XML = DescriptionTemplate;
            //}
        }

        private void FormImageGrid_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetImageGrid.CollSpecimenImageType_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.collSpecimenImageType_EnumTableAdapter.Fill(this.dataSetImageGrid.CollSpecimenImageType_Enum);
            // TODO: This line of code loads data into the 'dataSetImageGrid.CollectionSpecimenImage' table. You can move, or remove it, as needed.
            //this.collectionSpecimenImageTableAdapter.Fill(this.dataSetImageGrid.CollectionSpecimenImage);
        }

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void updateSpecimen()
        {
            try
            {
                if (this.collectionSpecimenImageBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.collectionSpecimenImageBindingSource.Current;
                    R.BeginEdit();
                    R.EndEdit();
                }
                if (this._SqlDataAdapterCollectionSpecimenImage != null)
                {
                    this.FormFunctions.updateTable(this.dataSetImageGrid, "CollectionSpecimenImage", this._SqlDataAdapterCollectionSpecimenImage, this.BindingContext);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void FormImageGrid_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.updateSpecimen();
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                null,
                null);
        }

        #endregion

        #region Grid and common functions
        
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.collectionSpecimenImageBindingSource.Current;
            string URI = R["URI"].ToString();
            this.userControlImage.ImagePath = URI;
            if (!R["Description"].Equals(System.DBNull.Value))
            {
                string Description = R["Description"].ToString();
                this.userControlXMLTreeExif.XML = Description;
                //this.buttonAddDescription.Visible = false;
                //this.tabControlDescription.SelectTab(this.tabPageDescription);
            }
            else
            {
                this.userControlXMLTreeExif.XML = "";
                //this.buttonAddDescription.Visible = true;
                //this.tabControlDescription.SelectTab(this.tabPageTemplate);
            }
            this.enableReplaceButtons();
            this.labelGridCounter.Text = (this.dataGridView.SelectedCells[0].RowIndex + 1).ToString();

            string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
            if (e.ColumnIndex == this.dataGridView.SelectedCells[0].ColumnIndex)
            {
                if (ColumnName.Length > 0)
                {
                    switch (ColumnName)
                    {
                        case "CreatorAgentURI":
                        case "LicenseHolderAgentURI":
                            this.GetRemoteValues(this.dataGridView.SelectedCells[0]);
                            break;
                    }
                }
                else
                {
                    ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].Name;
                    string ColumnToSetToNull = "";
                    switch (ColumnName)
                    {
                        case "CreatorRemoveLinkToAgents":
                            ColumnToSetToNull = "CreatorAgentURI";
                            break;
                        case "HolderRemoveLink":
                            ColumnToSetToNull = "LicenseHolderAgentURI";
                            break;
                    }
                    if (this.collectionSpecimenImageBindingSource != null)
                    {
                        System.Data.DataRowView RU = (System.Data.DataRowView)this.collectionSpecimenImageBindingSource.Current;
                        RU[ColumnToSetToNull] = System.DBNull.Value;
                    }

                }
            }


        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
        
        private string Message(string Resource)
        {
            string Message = "";
            try
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiversityCollection.Forms.FormCollectionSpecimenGridModeText));
                Message = resources.GetString(Resource);

            }
            catch { }
            return Message;
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.DrawRowNumber(this.dataGridView, this.dataGridView.Font, e);
        }

        private void dataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.enableReplaceButtons();
        }

        private void dataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.setCellBlockings();
            this.setRemoveCellStyle();
        }

        #endregion

        #region Button events for Finding, Copy and Saving and related functions

        private void buttonGridModeFind_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataSetImageGrid.CollectionSpecimenImage.Rows.Count == 0
                    || this.dataGridView.SelectedCells.Count == 0)
                {
                    System.Windows.Forms.MessageBox.Show(this.Message("Nothing_selected"));
                    return;
                }
                if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.SaveAll();
                int ID = 0;
                System.Data.DataRowView R = (System.Data.DataRowView)this.collectionSpecimenImageBindingSource.Current;
                if (int.TryParse(R["CollectionSpecimenID"].ToString(), out ID))
                {
                    this.DialogResult = DialogResult.OK;
                    this._SpecimenID = ID;
                    this.Close();
                }
                //if (int.TryParse(this.dataSetImageGrid.CollectionSpecimenImage.Rows[DatasetIndexOfCurrentLine]["CollectionSpecimenID"].ToString(), out ID))
                //{
                //    this.DialogResult = DialogResult.OK;
                //    this._SpecimenID = ID;
                //    this.Close();
                //}

            }
            catch { }
        }

        private void buttonGridModeSave_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.collectionSpecimenImageBindingSource.Current;
            R.BeginEdit();
            R.EndEdit();
            //this.dataSetImageGrid.CollectionSpecimenImage.Rows[DatasetIndexOfCurrentLine].BeginEdit();
            //this.dataSetImageGrid.CollectionSpecimenImage.Rows[DatasetIndexOfCurrentLine].EndEdit();
            this._SqlDataAdapterCollectionSpecimenImage.Update(this.dataSetImageGrid.CollectionSpecimenImage);
            //this.GridModeUpdate(DatasetIndexOfCurrentLine);
        }

        private void buttonSaveAll_Click(object sender, EventArgs e)
        {
            this.updateSpecimen();
            //this._SaveMode = SaveModes.All;
            //this.SaveAll();
            //this._SaveMode = SaveModes.Single;
        }

        private void CheckForChangesAndAskForSaving(object sender, EventArgs e)
        {
            try
            {
                //this.textBoxHeaderAccessionNumber.Focus();
                if (this.dataSetImageGrid.HasChanges())
                {
                    if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        this.SaveAll();
                }
            }
            catch { }
        }

        private void SaveAll()
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.progressBarSaveAll.Visible = true;
                this.progressBarSaveAll.Maximum = this.dataSetImageGrid.CollectionSpecimenImage.Rows.Count;
                this.progressBarSaveAll.Value = 0;
                for (int i = 0; i < this.dataSetImageGrid.CollectionSpecimenImage.Rows.Count; i++)
                {
                    if (this.dataSetImageGrid.CollectionSpecimenImage.Rows[i].RowState == DataRowState.Deleted
                        || this.dataSetImageGrid.CollectionSpecimenImage.Rows[i].RowState == DataRowState.Detached
                        || this.dataSetImageGrid.CollectionSpecimenImage.Rows[i].RowState == DataRowState.Unchanged)
                        continue;
                    this.dataGridView.Rows[i].Cells[0].Selected = true;
                    this.dataSetImageGrid.CollectionSpecimenImage.Rows[i].BeginEdit();
                    this.dataSetImageGrid.CollectionSpecimenImage.Rows[i].EndEdit();
                    this._SqlDataAdapterCollectionSpecimenImage.Update(this.dataSetImageGrid.CollectionSpecimenImage);
                    //this.GridModeUpdate(i);
                    this.progressBarSaveAll.Value = i;
                }
            }
            catch { }
            finally { this.progressBarSaveAll.Visible = false; }
            this.Cursor = Cursors.Default;
        }

        private void buttonGridModeCopy_Click(object sender, EventArgs e)
        {
            bool MoreThanOneRow = false;
            if (this.dataGridView.SelectedRows.Count > 1) MoreThanOneRow = true;
            try
            {
                if (this.dataGridView.SelectedCells.Count > 1)
                {
                    System.Collections.Generic.List<int> RowIndex = new List<int>();
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                        if (!RowIndex.Contains(C.RowIndex)) RowIndex.Add(C.RowIndex);
                    if (RowIndex.Count > 1) MoreThanOneRow = true;
                }
                if (MoreThanOneRow)
                {
                    System.Windows.Forms.MessageBox.Show(this.Message("Only_single_datasets_can_be_copied"));
                    return;
                }
                else
                {
                    // Save the current dataset
                    this.buttonGridModeSave_Click(null, null);
                }

                if (System.Windows.Forms.MessageBox.Show("Do you want to create a copy of the current dataset", "Copy dataset", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    System.Data.DataRow Rori = this.dataSetImageGrid.CollectionSpecimenImage.Rows[this.dataGridView.SelectedCells[0].RowIndex];
                    //System.Data.DataRow Rnew = this.dataSetImageGrid.CollectionSpecimenImage.NewFirstLinesPartRow();
                    //Rnew[0] = this.InsertNewPart(this._SpecimenID);
                    //for (int i = 1; i < this.dataSetImageGrid.CollectionSpecimenImage.Columns.Count; i++)
                    //{
                    //    string ColumnName = this.dataSetImageGrid.CollectionSpecimenImage.Columns[i].ColumnName;
                    //    Rnew[ColumnName] = Rori[ColumnName];
                    //}
                    //this.dataSetImageGrid.CollectionSpecimenImage.Rows.Add(Rnew);
                }
            }
            catch { }
        }

        private void buttonGridModeDelete_Click(object sender, EventArgs e)
        {
            //bool DeleteSelection = false;
            //this.buttonGridModeDelete.Enabled = false;
            //System.Collections.Generic.List<int> IDsToDelete = new List<int>();
            //try
            //{
            //    foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.SelectedRows)
            //    {
            //        System.Data.DataRowView RV = (System.Data.DataRowView)R.DataBoundItem;
            //        int ID = 0;
            //        if (int.TryParse(RV["CollectionSpecimenID"].ToString(), out ID))
            //        {
            //            string AccessionNumber = RV["Accession_Number"].ToString();
            //            if (!DeleteSelection)
            //            {
            //                string Message = DiversityWorkbench.Entity.Message("Do_you_want_to_delete_the_dataset");
            //                if (AccessionNumber.Length > 0) Message += "\r\n" + DiversityWorkbench.Entity.EntityInformation("CollectionSpecimen.AccessionNumber")["DisplayText"] + ": " + AccessionNumber;
            //                else Message += " ID " + ID.ToString();
            //                if (this.dataGridView.SelectedRows.Count > 1)
            //                    Message += " " + DiversityWorkbench.Entity.Message("and") + " " + (this.dataGridView.SelectedRows.Count - 1).ToString() + " " + this.Message("further_datasets") + "?";
            //                if (System.Windows.Forms.MessageBox.Show(Message, DiversityWorkbench.Entity.Message("delete_entry"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            //                    return;
            //                else
            //                    DeleteSelection = true;
            //            }
            //        }
            //        if (DeleteSelection)
            //        {
            //            IDsToDelete.Add(ID);
            //        }
            //    }
            //    if (DeleteSelection)
            //    {
            //        foreach (int ID in IDsToDelete)
            //        {
            //            this.deleteSpecimen(ID);
            //        }
            //        //this.GridModeFillTable();
            //    }
            //}
            //catch { }
        }

        /// <summary>
        /// delete a specimen from the database
        /// </summary>
        /// <param name="ID">the Primary key of table CollectionSpecimen corresponding to the item that should be deleted</param>
        //private void deleteSpecimen(int ID)
        //{
        //    try
        //    {
        //        string SQL = "DELETE FROM CollectionSpecimen WHERE CollectionSpecimenID = " + ID.ToString();
        //        Microsoft.Data.SqlClient.SqlConnection Conn = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
        //        Microsoft.Data.SqlClient.SqlCommand com = new Microsoft.Data.SqlClient.SqlCommand(SQL, Conn);
        //        com.CommandType = System.Data.CommandType.Text;
        //        Conn.Open();
        //        com.ExecuteNonQuery();
        //        Conn.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private void dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (this.dataGridView.SelectedRows.Count > 0)
            //    this.buttonGridModeDelete.Enabled = true;
            //else this.buttonGridModeDelete.Enabled = false;
        }

        #endregion

        #region Filter, Column height etc.

        private void buttonOptRowHeight_Click(object sender, EventArgs e)
        {
            this.dataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
        }

        private void buttonOptColumnWidth_Click(object sender, EventArgs e)
        {
            this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void buttonFilterGrid_Click(object sender, EventArgs e)
        {
            //if (this.dataSetImageGrid.HasChanges())
            //{
            //    if (System.Windows.Forms.MessageBox.Show(this.Message("Save_changes") + "?", this.Message("Save") + "?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            //    {
            //        this.textBoxGridModeReplace.Focus();
            //        this.SaveAll();
            //        System.Windows.Forms.MessageBox.Show("Please try to filter again");
            //        this.dataGridView.CurrentCell = null;
            //        return;
            //    }
            //}
            if (this.dataGridView.SelectedCells != null)
            {
                if (this.dataGridView.SelectedCells[0].Value != null)
                {
                    string Filter = this.dataGridView.SelectedCells[0].Value.ToString();
                    int iFilterColumn = this.dataGridView.SelectedCells[0].ColumnIndex;
                    this.SaveAll();
                    System.Collections.Generic.List<DataRow> RowsToRemove = new List<DataRow>();
                    foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
                    {
                        if (R.Cells[iFilterColumn].Value.ToString() != Filter)
                        {
                            System.Data.DataRowView Rdata = (System.Data.DataRowView)R.DataBoundItem;
                            RowsToRemove.Add(Rdata.Row);
                        }
                    }
                    foreach (System.Data.DataRow R in RowsToRemove)
                        R.Delete();
                    this.dataSetImageGrid.AcceptChanges();
                }
            }
        }

        private void buttonRemoveRowFromGrid_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedRows.Count > 0)
            {
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.SelectedRows)
                {
                    try
                    {
                        System.Data.DataRowView Rdata = (System.Data.DataRowView)R.DataBoundItem;
                        Rdata.Delete();
                        Rdata.Row.AcceptChanges();
                    }
                    catch (System.Exception ex) { }
                }
                this.dataSetImageGrid.AcceptChanges();
            }
            else
                System.Windows.Forms.MessageBox.Show("No rows selected");
        }
        
        #endregion

        #region Replace, Insert, Append, Clear Function for a single Column

        private void buttonMarkWholeColumn_Click(object sender, EventArgs e)
        {
            try
            {
                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                if (ColumnName == "CollectionSpecimenID") return;
                System.Windows.Forms.DataGridViewColumn C = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex];
                int iLines = this.dataGridView.Rows.Count;
                if (this.dataGridView.AllowUserToAddRows)
                    iLines--;
                for (int i = 0; i < iLines; i++)
                {
                    this.dataGridView.Rows[i].Cells[this.dataGridView.SelectedCells[0].ColumnIndex].Selected = true;
                }
                string DisplayTextForColumn = ColumnName;
                if (DisplayTextForColumn.Length > 20)
                {
                    string[] Parts = DisplayTextForColumn.Split(new char[] { '_' });
                    int L = 20;
                    while (L > 2 && DisplayTextForColumn.Length > 20)
                    {
                        for (int i = 0; i < Parts.Length; i++)
                        {
                            if (Parts[i].Length > L)
                                Parts[i] = Parts[i].Substring(0, L) + ".";
                        }
                        DisplayTextForColumn = Parts[0];
                        for (int i = 1; i < Parts.Length; i++)
                        {
                            DisplayTextForColumn += " " + Parts[i];
                            if (!DisplayTextForColumn.EndsWith(".") && i < Parts.Length)
                                DisplayTextForColumn += " ";
                        }
                        L--;
                    }
                }
                this.labelGridViewReplaceColumnName.Text = DisplayTextForColumn;
            }
            catch { }
            this.enableReplaceButtons();
        }

        private void contextMenuStripDataGrid_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if ((System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.CommaSeparatedValue)
                || System.Windows.Forms.Clipboard.ContainsData(System.Windows.Forms.DataFormats.Text))
                && e.ClickedItem.Name == "toolStripMenuItemCopyFromClipboard")
            {
                // finding the top row
                int IndexTopRow = this.dataGridView.Rows.Count;
                if (this.dataGridView.SelectedCells.Count > 0)
                {
                    foreach (System.Windows.Forms.DataGridViewCell C in this.dataGridView.SelectedCells)
                        if (IndexTopRow > C.RowIndex) IndexTopRow = C.RowIndex;
                }

                // parsing the content of the clipboard
                System.Collections.Generic.List<System.Collections.Generic.List<string>> ClipBoardValues = new List<List<string>>();
                string[] stringSeparators = new string[] { "\r\n" };
                string[] lineSeparators = new string[] { "\t" };
                string ClipBoardText = System.Windows.Forms.Clipboard.GetText();
                string[] ClipBoardList = ClipBoardText.Split(stringSeparators, StringSplitOptions.None);
                for (int i = 0; i < ClipBoardList.Length - 1; i++)
                {
                    System.Collections.Generic.List<string> ClipBoardValueStrings = new List<string>();
                    string[] ClipBoardListStrings = ClipBoardList[i].Split(lineSeparators, StringSplitOptions.None);
                    for (int ii = 0; ii < ClipBoardListStrings.Length; ii++)
                        ClipBoardValueStrings.Add(ClipBoardListStrings[ii]);
                    ClipBoardValues.Add(ClipBoardValueStrings);
                }

                // transfering the values
                try
                {
                    // the colums for the transfer
                    System.Collections.Generic.List<System.Windows.Forms.DataGridViewColumn> GridColums = new List<DataGridViewColumn>();
                    int CurrentDisplayIndex = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DisplayIndex;
                    for (int i = 0; i < ClipBoardValues[0].Count; i++)
                    {
                        foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridView.Columns)
                        {
                            if (C.Visible && C.DisplayIndex == CurrentDisplayIndex + i)
                            {
                                GridColums.Add(C);
                                break;
                            }
                        }
                    }
                    for (int ii = 0; ii < GridColums.Count; ii++) // the columns
                    {
                        for (int i = 0; i < ClipBoardValues.Count; i++) // the rows
                        {
                            if (DiversityCollection.Forms.FormGridFunctions.ValueIsValid(this.dataGridView, GridColums[ii].Index, ClipBoardValues[i][ii]))
                            {
                                this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index].Value = ClipBoardValues[i][ii];
                                //this.checkForMissingAndDefaultValues(this.dataGridView.Rows[IndexTopRow + i].Cells[GridColums[ii].Index], true);
                            }
                            else
                            {
                                string Message = ClipBoardValues[i][ii] + " is not a valid value for "
                                    + this.dataGridView.Columns[GridColums[ii].Index].DataPropertyName
                                    + "\r\n\r\nDo you want to try to insert the other values?";
                                if (System.Windows.Forms.MessageBox.Show(Message, "Invalid value", MessageBoxButtons.YesNo) == DialogResult.No)
                                    break;
                            }
                            if (i + IndexTopRow + 3 > this.dataGridView.Rows.Count)
                                continue;
                        }
                    }
                }
                catch { }
            }
            else if (e.ClickedItem.Name == "toolStripMenuItemCopyFromClipboard")
            {
                System.Windows.Forms.MessageBox.Show("Only text and spreadsheet values can be copied");
                return;
            }
        }
        
        private void enableReplaceButtons()
        {
            bool IsEnabled = true;
            if (this.dataGridView.SelectedCells.Count < 2)
                IsEnabled = false;
            this.enableReplaceButtons(IsEnabled);
        }

        private void enableReplaceButtons(bool IsEnabled)
        {
            System.Collections.Generic.List<int> ColList = new List<int>();
            for (int i = 0; i < this.dataGridView.SelectedCells.Count; i++)
                if (!ColList.Contains(this.dataGridView.SelectedCells[i].ColumnIndex)) ColList.Add(this.dataGridView.SelectedCells[i].ColumnIndex);
            if (IsEnabled && ColList.Count > 1)
                IsEnabled = false;
            this.buttonGridModeReplace.Enabled = IsEnabled;
            this.buttonGridModeInsert.Enabled = IsEnabled;
            this.buttonGridModeRemove.Enabled = IsEnabled;
            this.buttonGridModeAppend.Enabled = IsEnabled;
            this.radioButtonGridModeAppend.Enabled = IsEnabled;
            this.radioButtonGridModeInsert.Enabled = IsEnabled;
            this.radioButtonGridModeRemove.Enabled = IsEnabled;
            this.radioButtonGridModeReplace.Enabled = IsEnabled;
            this.textBoxGridModeReplace.Enabled = IsEnabled;
            this.textBoxGridModeReplaceWith.Enabled = IsEnabled;
            this.comboBoxReplace.Enabled = IsEnabled;
            this.comboBoxReplaceWith.Enabled = IsEnabled;

            this.labelGridModeReplaceWith.Enabled = IsEnabled;
            this.labelGridViewReplaceColumn.Enabled = IsEnabled;
            this.labelGridViewReplaceColumnName.Enabled = IsEnabled;
            if (!IsEnabled) this.labelGridViewReplaceColumnName.Text = "?";

            if (this.IsLinkColumn)
            {
                this.linkLabelLink.Enabled = IsEnabled;
                this.buttonGetLink.Enabled = IsEnabled;
            }
            else
            {
                this.buttonGetLink.Enabled = false;
                this.linkLabelLink.Enabled = false;
            }

            if (this.dataGridView.SelectedCells.Count > 0 && ColList.Count == 1)
            {
                if (typeof(System.Windows.Forms.DataGridViewComboBoxCell)
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    System.Windows.Forms.DataGridViewComboBoxColumn Combo
                        = (System.Windows.Forms.DataGridViewComboBoxColumn)this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex];
                    this.comboBoxReplace.Width = 100;
                    this.comboBoxReplaceWith.Width = 100;
                    System.Object O = Combo.DataSource;
                    System.Windows.Forms.BindingSource BS = (System.Windows.Forms.BindingSource)O;
                    DiversityCollection.Datasets.DataSetImageGrid DS = (DiversityCollection.Datasets.DataSetImageGrid)BS.DataSource;
                    System.Data.DataTable dtReplace = DS.Tables[BS.DataMember.ToString()].Copy();
                    System.Data.DataTable dtReplaceWith = DS.Tables[BS.DataMember.ToString()].Copy();

                    this.comboBoxReplace.DataSource = dtReplace;
                    this.comboBoxReplace.DisplayMember = Combo.DisplayMember;
                    this.comboBoxReplace.ValueMember = Combo.ValueMember;
                    DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReplace, AutoCompleteMode.Suggest);

                    this.comboBoxReplaceWith.DataSource = dtReplaceWith;
                    this.comboBoxReplaceWith.DisplayMember = Combo.DisplayMember;
                    this.comboBoxReplaceWith.ValueMember = Combo.ValueMember;
                    DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxReplaceWith, AutoCompleteMode.Suggest);
                }
                else if (this.IsLinkColumn)
                {
                    this.buttonGridModeReplace.Enabled = false;
                    this.buttonGridModeAppend.Enabled = false;

                    this.radioButtonGridModeReplace.Enabled = false;
                    this.radioButtonGridModeAppend.Enabled = false;

                    this.radioButtonGridModeRemove.Enabled = true;
                    this.radioButtonGridModeInsert.Enabled = true;
                    this.buttonGridModeInsert.Enabled = true;
                    this.buttonGridModeRemove.Enabled = true;

                    this.radioButtonGridModeReplace.Checked = false;
                    this.radioButtonGridModeInsert.Checked = false;
                    this.radioButtonGridModeAppend.Checked = false;

                    this.radioButtonGridModeRemove.Checked = true;
                }
            }
            this.setReplaceOptions();
        }

        private bool IsLinkColumn
        {
            get
            {
                if (this.dataGridView.SelectedCells == null || this.dataGridView.SelectedCells.Count == 0)
                    return false;

                string ColumnName = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();

                if (ColumnName == "CreatorAgentURI" ||
                    ColumnName == "LicenseHolderAgentURI")
                {
                    return true;
                }
                else return false;
            }
        }



        private System.Collections.Generic.List<int> _PkColumns;
        private System.Collections.Generic.List<int> PkColumns
        {
            get
            {
                if (this._PkColumns == null)
                {
                    this._PkColumns = new List<int>();
                    this._PkColumns.Add(0);
                    this._PkColumns.Add(1);
                }
                return this._PkColumns;
            }
        }

        private void buttonGridModeReplace_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetImageGrid.CollectionSpecimenImage, this.PkColumns, this.ReplaceOptionStatus);
        }

        private void buttonGridModeInsert_Click(object sender, EventArgs e)
        {
            if (this.IsLinkColumn)
            {
                int iDisplay = this.dataGridView.SelectedCells[0].ColumnIndex - 1;
                DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                    this.dataGridView, this.linkLabelLink, iDisplay, this.LinkDisplayText(),
                    this.dataSetImageGrid.CollectionSpecimenImage, this.PkColumns, this.ReplaceOptionStatus);
            }
            else
            {
                DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                    this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                    this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                    this.dataSetImageGrid.CollectionSpecimenImage, this.PkColumns, this.ReplaceOptionStatus);
            }
        }

        private void buttonGridModeAppend_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetImageGrid.CollectionSpecimenImage, this.PkColumns, this.ReplaceOptionStatus);
        }

        private void buttonGridModeRemove_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormGridFunctions.ColumnValues_ReplaceInsertClear(
                this.dataGridView, this.comboBoxReplace, this.comboBoxReplaceWith,
                this.textBoxGridModeReplace, this.textBoxGridModeReplaceWith,
                this.dataSetImageGrid.CollectionSpecimenImage, this.PkColumns, this.ReplaceOptionStatus);
        }

        private void buttonGridModeUndo_Click(object sender, EventArgs e)
        {
            try
            {
                this.dataSetImageGrid.Clear();
                this.initForm();
                //this._GridModeQueryFields = null;
                //this._GridModeColumnList = null;
                //this._GridModeTableList = null;
                //this.GridModeSaveFieldVisibility();
                //this.GridModeFillTable();
            }
            catch { }
            //foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridView.Rows)
            //{
            //    this.UndoChanges(R.Index);
            //}
        }

        private void buttonGridModeUndoSingleLine_Click(object sender, EventArgs e)
        {
            if (this.dataGridView.SelectedCells.Count > 0)
            {
                this.UndoChanges(this.dataGridView.SelectedCells[0].RowIndex);
            }
        }

        private void UndoChanges(int RowIndex)
        {
            try
            {
                //int PartID;
                //if (int.TryParse(this.dataGridView.Rows[RowIndex].Cells[0].Value.ToString(), out PartID))
                //{
                //    System.Data.DataRow Rori = this.dataSetImageGrid.CollectionSpecimenImage.Rows[this.DatasetIndexOfLine(PartID)];
                //    if (Rori.RowState != DataRowState.Unchanged)
                //    {
                //        Rori.RejectChanges();
                //    }
                //}
            }
            catch { }
        }

        private void radioButtonGridModeReplace_CheckedChanged(object sender, EventArgs e)
        {
            this.setReplaceOptions();
        }

        private void radioButtonGridModeInsert_CheckedChanged(object sender, EventArgs e)
        {
            this.setReplaceOptions();
        }

        private void radioButtonGridModeRemove_CheckedChanged(object sender, EventArgs e)
        {
            this.setReplaceOptions();
        }

        private void radioButtonGridModeAppend_CheckedChanged(object sender, EventArgs e)
        {
            this.setReplaceOptions();
        }

        private DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState ReplaceOptionStatus
        {
            get
            {
                if (this.radioButtonGridModeInsert.Checked) this._ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Insert;
                if (this.radioButtonGridModeRemove.Checked) this._ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Clear;
                if (this.radioButtonGridModeReplace.Checked) this._ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace;
                if (this.radioButtonGridModeAppend.Checked) this._ReplaceOptionState = DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Append;
                return this._ReplaceOptionState;
            }
        }

        private void setReplaceOptions()
        {
            bool ShowComboBoxes = false;

            this.buttonGetLink.Visible = false;
            this.buttonRemoveLink.Visible = false;
            this.linkLabelLink.Visible = false;

            if (this.dataGridView.SelectedCells.Count > 0)
            {
                if (typeof(System.Windows.Forms.DataGridViewComboBoxCell)
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                {
                    System.Windows.Forms.DataGridViewComboBoxColumn Combo
                        = (System.Windows.Forms.DataGridViewComboBoxColumn)this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex];
                    ShowComboBoxes = true;
                }
                this.labelGridViewReplaceColumnName.Text = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].HeaderText;
            }
            switch (this.ReplaceOptionStatus)
            {
                case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Replace:
                    this.buttonGridModeReplace.Visible = true;
                    this.buttonGridModeRemove.Visible = false;
                    this.buttonGridModeInsert.Visible = false;
                    this.buttonGridModeAppend.Visible = false;
                    this.buttonGetLink.Visible = false;
                    this.textBoxGridModeReplace.Visible = !ShowComboBoxes;
                    this.textBoxGridModeReplaceWith.Visible = !ShowComboBoxes;
                    this.comboBoxReplace.Visible = ShowComboBoxes;
                    this.comboBoxReplaceWith.Visible = ShowComboBoxes;
                    this.labelGridModeReplaceWith.Visible = true;
                    if (typeof(System.Windows.Forms.DataGridViewTextBoxCell)
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                    {
                        if (this.textBoxGridModeReplaceWith.Text.Length > 0 && this.textBoxGridModeReplace.Text.Length > 0 && this.dataGridView.SelectedCells.Count > 1)
                            this.buttonGridModeReplace.Enabled = true;
                        else this.buttonGridModeReplace.Enabled = false;
                    }
                    else
                    {
                        if (this.dataGridView.SelectedCells.Count > 1)
                            this.buttonGridModeReplace.Enabled = true;
                        else
                            this.buttonGridModeReplace.Enabled = false;
                    }
                    break;
                case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Insert:
                    this.buttonGridModeReplace.Visible = false;
                    this.buttonGridModeRemove.Visible = false;
                    this.buttonGridModeInsert.Visible = true;
                    this.buttonGridModeAppend.Visible = false;
                    this.textBoxGridModeReplace.Visible = false;
                    this.textBoxGridModeReplaceWith.Visible = !ShowComboBoxes;
                    this.comboBoxReplace.Visible = false;
                    this.comboBoxReplaceWith.Visible = ShowComboBoxes;
                    this.labelGridModeReplaceWith.Visible = false;
                    if (typeof(System.Windows.Forms.DataGridViewTextBoxCell)
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                    {
                        if (this.textBoxGridModeReplaceWith.Text.Length > 0 && this.dataGridView.SelectedCells.Count > 1)
                            this.buttonGridModeInsert.Enabled = true;
                        else this.buttonGridModeInsert.Enabled = false;
                    }
                    else
                    {
                        if (this.dataGridView.SelectedCells.Count > 1)
                            this.buttonGridModeInsert.Enabled = true;
                        else
                            this.buttonGridModeInsert.Enabled = false;
                    }

                    if (this.IsLinkColumn)
                    {
                        this.buttonGetLink.Visible = true;
                        this.buttonGetLink.Enabled = true;
                        this.linkLabelLink.Text = "";
                        this.linkLabelLink.Visible = true;
                        this.linkLabelLink.Enabled = true;
                        this.buttonGridModeInsert.Enabled = false;
                        this.textBoxGridModeReplaceWith.Visible = false;
                        this.comboBoxReplaceWith.Visible = false;
                    }

                    break;
                case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Append:
                    this.buttonGridModeReplace.Visible = false;
                    this.buttonGridModeRemove.Visible = false;
                    this.buttonGridModeInsert.Visible = false;
                    this.buttonGridModeAppend.Visible = true;
                    this.textBoxGridModeReplace.Visible = false;
                    this.textBoxGridModeReplaceWith.Visible = !ShowComboBoxes;
                    this.comboBoxReplace.Visible = false;
                    this.comboBoxReplaceWith.Visible = ShowComboBoxes;
                    this.labelGridModeReplaceWith.Visible = false;
                    if (typeof(System.Windows.Forms.DataGridViewTextBoxCell)
                    == this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].CellType)
                    {
                        if (this.textBoxGridModeReplaceWith.Text.Length > 0 && this.dataGridView.SelectedCells.Count > 1)
                            this.buttonGridModeAppend.Enabled = true;
                        else this.buttonGridModeAppend.Enabled = false;
                    }
                    else
                    {
                        if (this.dataGridView.SelectedCells.Count > 1)
                            this.buttonGridModeAppend.Enabled = true;
                        else this.buttonGridModeAppend.Enabled = false;
                    }
                    break;
                case DiversityCollection.Forms.FormGridFunctions.ReplaceOptionState.Clear:
                    this.buttonGridModeReplace.Visible = false;
                    this.buttonGridModeRemove.Visible = true;
                    this.buttonGridModeInsert.Visible = false;
                    this.buttonGridModeAppend.Visible = false;
                    this.textBoxGridModeReplace.Visible = false;
                    this.textBoxGridModeReplaceWith.Visible = false;
                    this.comboBoxReplace.Visible = false;
                    this.comboBoxReplaceWith.Visible = false;
                    this.labelGridModeReplaceWith.Visible = false;
                    break;
            }
        }

        private void textBoxGridModeReplace_TextChanged(object sender, EventArgs e)
        {
            this.enableReplaceButtons();
        }

        private void textBoxGridModeReplaceWith_TextChanged(object sender, EventArgs e)
        {
            this.enableReplaceButtons();
        }

        #endregion

        #region Remote services

        private void GetRemoteValues(System.Windows.Forms.DataGridViewCell Cell)
        {
            DiversityWorkbench.Forms.FormRemoteQuery f;
            try
            {
                string ValueColumn = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
                string DisplayColumn = ValueColumn;
                bool IsListInDatabase = false;
                string ListInDatabase = "";
                DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit;
                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RemoteValueBindings = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();
                switch (ValueColumn)
                {
                    case "CreatorAgentURI":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "CreatorAgentURI";
                        DisplayColumn = "CreatorAgent";
                        DiversityWorkbench.Agent Creator = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Creator;
                        break;

                    case "LicenseHolderAgentURI":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "LicenseHolderAgentURI";
                        DisplayColumn = "LicenseHolder";
                        DiversityWorkbench.Agent LicenseHolder = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)LicenseHolder;
                        break;

                    default:
                        DiversityWorkbench.TaxonName Default = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Default;
                        break;
                }

                if (this.collectionSpecimenImageBindingSource != null && IWorkbenchUnit != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.collectionSpecimenImageBindingSource.Current;
                    string URI = "";
                    if (RU != null)
                        if (!RU[ValueColumn].Equals(System.DBNull.Value)) URI = RU[ValueColumn].ToString();
                    if (URI.Length == 0)
                    {
                        if (IsListInDatabase)
                        {
                            f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit, ListInDatabase);
                        }
                        else
                        {
                            f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit);
                        }
                    }
                    else
                    {
                        f = new DiversityWorkbench.Forms.FormRemoteQuery(URI, IWorkbenchUnit);
                    }
                    try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
                    catch { }
                    f.TopMost = true;
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.DisplayText != null)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.collectionSpecimenImageBindingSource.Current;
                        R.BeginEdit();
                        R[ValueColumn] = f.URI;
                        R[DisplayColumn] = f.DisplayText;
                        R.EndEdit();
                        //this.labelURI.Text = f.URI;
                        if (RemoteValueBindings != null && RemoteValueBindings.Count > 0)
                        {
                            foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in RemoteValueBindings)
                            {
                                foreach (System.Collections.Generic.KeyValuePair<string, string> P in IWorkbenchUnit.UnitValues())
                                {
                                    if (RVB.RemoteParameter == P.Key)
                                    {
                                        System.Data.DataRowView RV = (System.Data.DataRowView)RVB.BindingSource.Current;
                                        RV.BeginEdit();
                                        RV[RVB.Column] = P.Value;
                                        RV.EndEdit();
                                    }
                                }
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

        #region Removing links and Columns for removing links to external services

        private void RemoveLink(System.Windows.Forms.DataGridViewCell Cell)
        {
            string DisplayColumn = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
            string ValueColumn = "";
            string Table = "";
            string LinkColumn = "";
            try
            {
                foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
                {
                    if (Q.AliasForColumn == DisplayColumn)
                    {
                        ValueColumn = Q.Column;
                        Table = Q.AliasForTable;
                        break;
                    }
                }

                foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
                {
                    if (Q.AliasForTable == Table && Q.Column == ValueColumn)
                    {
                        LinkColumn = Q.AliasForColumn;
                        break;
                    }
                }

                if (this.collectionSpecimenImageBindingSource != null)
                {
                    System.Data.DataRowView RU = (System.Data.DataRowView)this.collectionSpecimenImageBindingSource.Current;
                    RU[LinkColumn] = System.DBNull.Value;
                }

            }
            catch { }
        }

        /// <summary>
        /// Dictionary of columns that remove links of related modules etc.
        /// </summary>
        private System.Collections.Generic.Dictionary<string, string> RemoveColumns
        {
            get
            {
                if (this._RemoveColumns == null)
                {
                    this._RemoveColumns = new Dictionary<string, string>();
                    this._RemoveColumns.Add("CreatorRemoveLinkToAgents", "");
                    this._RemoveColumns.Add("HolderRemoveLink", "");
                }
                return this._RemoveColumns;
            }
        }

        private void setRemoveCellStyle()
        {
            for (int i = 0; i < this.dataGridView.Rows.Count; i++)
                this.setRemoveCellStyle(i);
        }

        private void setRemoveCellStyle(int RowIndex)
        {
            try
            {
                foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
                {
                    if (this.dataGridView.Columns[Cell.ColumnIndex].HeaderText == "X")
                    {
                        Cell.Style = DiversityCollection.Forms.FormGridFunctions.StyleRemove;
                    }
                    //if (this.RemoveColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
                    //{
                    //    foreach (System.Windows.Forms.DataGridViewCell RemoveCell in this.dataGridView.Rows[RowIndex].Cells)
                    //    {
                    //        if (this.dataGridView.Columns[RemoveCell.ColumnIndex].DataPropertyName ==
                    //            this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName)
                    //        {
                    //            RemoveCell.Style = DiversityCollection.Forms.FormGridFunctions.StyleRemove;// this._StyleRemove;
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            catch { }
        }

        #endregion


        #endregion

        #region Blocking of Cells that are linked to external services

        private void setCellBlockings()
        {
            for (int i = 0; i < this.dataGridView.Rows.Count; i++)
            {
                this.setCellBlockings(i);
                //this.setCellBlockingsForAnalysis(i);
            }
        }

        private void setCellBlockings(int RowIndex)
        {
            try
            {
                foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
                {
                    if (this.BlockedColumns.ContainsKey(this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName))
                    {
                        foreach (System.Windows.Forms.DataGridViewCell CellToBlock in this.dataGridView.Rows[RowIndex].Cells)
                        {
                            if (this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName ==
                                this.BlockedColumns[this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName])
                            {
                                if (Cell.EditedFormattedValue.ToString().Length > 0)
                                {
                                    CellToBlock.Style = DiversityCollection.Forms.FormGridFunctions.StyleBlocked;// DiversityCollection.Forms.FormGridFunctions.StyleBlocked;
                                    CellToBlock.ReadOnly = true;
                                }
                                else
                                {
                                    CellToBlock.Style = DiversityCollection.Forms.FormGridFunctions.StyleUnblocked;// this._StyleUnblocked;
                                    CellToBlock.ReadOnly = false;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (System.Windows.Forms.DataGridViewCell CellToBlock in this.dataGridView.Rows[RowIndex].Cells)
                        {
                            if (this.ReadOnlyColumns.Contains(this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName))
                            {
                                CellToBlock.Style = DiversityCollection.Forms.FormGridFunctions.StyleReadOnly;// this._StyleReadOnly;
                                CellToBlock.ReadOnly = true;
                            }
                        }
                    }
                }

            }
            catch (System.Exception ex) { }
        }

        //private void setCellBlockingsForAnalysis(int RowIndex)
        //{
        //    try
        //    {
        //        string TaxonomicGroup = this.dataSetIdentifictionUnitGridMode.FirstLinesUnit.Rows[RowIndex]["Taxonomic_group"].ToString();
        //        foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
        //        {
        //            if (this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName == "Taxonomic_group")
        //            {
        //                TaxonomicGroup = Cell.Value.ToString();
        //                break;
        //            }
        //        }
        //        //foreach (System.Windows.Forms.DataGridViewCell Cell in this.dataGridView.Rows[RowIndex].Cells)
        //        //{
        //        //    string DataProperty = this.dataGridView.Columns[Cell.ColumnIndex].DataPropertyName;
        //        //    if (DataProperty.StartsWith("Analysis_result"))
        //        //    {
        //        //        foreach (System.Windows.Forms.DataGridViewCell CellToBlock in this.dataGridView.Rows[RowIndex].Cells)
        //        //        {
        //        //            if (this.AnalysisReadOnlyColumns.Contains(this.dataGridView.Columns[CellToBlock.ColumnIndex].DataPropertyName))
        //        //            {
        //        //                CellToBlock.Style = DiversityCollection.Forms.FormGridFunctions.StyleReadOnly;// this._StyleReadOnly;
        //        //                CellToBlock.ReadOnly = true;
        //        //            }
        //        //            else
        //        //            {
        //        //                int i;
        //        //                if (int.TryParse(DataProperty.Substring(DataProperty.Length - 1), out i))
        //        //                {
        //        //                    if (this.TaxonAnalysisDict[TaxonomicGroup].Contains(this.AnalysisList[i].AnalysisID))
        //        //                    {
        //        //                        Cell.Style = DiversityCollection.Forms.FormGridFunctions.StyleUnblocked;// this._StyleUnblocked;
        //        //                        Cell.ReadOnly = false;
        //        //                    }
        //        //                    else
        //        //                    {
        //        //                        Cell.Style = DiversityCollection.Forms.FormGridFunctions.StyleReadOnly; //this._StyleReadOnly;
        //        //                        Cell.ReadOnly = true;
        //        //                    }
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //}
        //    }
        //    catch (System.Exception ex) { }
        //}

        private System.Collections.Generic.Dictionary<string, string> BlockedColumns
        {
            get
            {
                if (this._BlockedColumns == null)
                {
                    this._BlockedColumns = new Dictionary<string, string>();
                    this._BlockedColumns.Add("CreatorAgentURI", "CreatorAgent");
                    this._BlockedColumns.Add("LicenseHolderAgentURI", "LicenseHolder");
                }
                return this._BlockedColumns;
            }
        }

        private System.Collections.Generic.List<string> ReadOnlyColumns
        {
            get
            {
                if (this._ReadOnlyColumns == null)
                {
                    this._ReadOnlyColumns = new List<string>();
                    //foreach (DiversityCollection.Forms.GridModeQueryField Q in this._GridModeQueryFields)
                    //{
                    //    if (Q.Table != "IdentificationUnit" &&
                    //        Q.Table != "Identification" &&
                    //        Q.Table != "IdentificationUnitAnalysis")
                    //        if (!this._ReadOnlyColumns.Contains(Q.AliasForColumn))
                    //            this._ReadOnlyColumns.Add(Q.AliasForColumn);
                    //}
                    //this._ReadOnlyColumns.Add("Related_organism");
                    //this._ReadOnlyColumns.Add("Analysis");
                    //this._ReadOnlyColumns.Add("AnalysisID");
                }
                return this._ReadOnlyColumns;
            }
        }

        #endregion

        private System.Collections.Generic.Dictionary<string, string> _LinkLabelLinkColumns;
        private void linkLabelLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (this._LinkLabelLinkColumns != null && this._LinkLabelLinkColumns.Count > 0)
            {
                string Message = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._LinkLabelLinkColumns)
                {
                    Message += KV.Key + ": " + KV.Value + "\r\n";
                }
                System.Windows.Forms.MessageBox.Show(Message);
            }
        }

        private string LinkDisplayText()
        {
            string DisplayText = "";
            if (this._LinkLabelLinkColumns != null && this._LinkLabelLinkColumns.Count > 0)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._LinkLabelLinkColumns)
                {
                    DisplayText = KV.Value;
                    break;
                }
            }
            return DisplayText;
        }

        private void buttonGetLink_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormRemoteQuery f;
            try
            {
                string ValueColumn = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName;
                string DisplayColumn = ValueColumn;
                bool IsListInDatabase = false;
                string ListInDatabase = "";
                DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit;
                System.Collections.Generic.List<DiversityWorkbench.UserControls.RemoteValueBinding> RemoteValueBindings = new List<DiversityWorkbench.UserControls.RemoteValueBinding>();
                if (this._LinkLabelLinkColumns == null)
                    this._LinkLabelLinkColumns = new Dictionary<string, string>();
                switch (ValueColumn)
                {

                    case "CreatorAgentURI":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "CreatorAgentURI";
                        DisplayColumn = "CreatorAgent";
                        DiversityWorkbench.Agent Creator = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Creator;
                        break;

                    case "LicenseHolderAgentURI":
                        if (!DiversityWorkbench.WorkbenchUnit.IsAnyServiceAvailable("DiversityAgents")) return;
                        ValueColumn = "LicenseHolderAgentURI";
                        DisplayColumn = "LicenseHolder";
                        DiversityWorkbench.Agent LicenseHolder = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)LicenseHolder;
                        break;

                    default:
                        DiversityWorkbench.TaxonName Default = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                        IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)Default;
                        break;
                }

                if (IWorkbenchUnit != null)
                {
                    string URI = this.linkLabelLink.Text;
                    if (URI.Length == 0)
                    {
                        if (IsListInDatabase)
                        {
                            f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit, ListInDatabase);
                        }
                        else
                        {
                            f = new DiversityWorkbench.Forms.FormRemoteQuery(IWorkbenchUnit);
                        }
                    }
                    else
                    {
                        f = new DiversityWorkbench.Forms.FormRemoteQuery(URI, IWorkbenchUnit);
                    }
                    try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
                    catch { }
                    f.TopMost = true;
                    f.ShowDialog();
                    if (f.DialogResult == DialogResult.OK && f.DisplayText != null)
                    {
                        this.linkLabelLink.Text = f.URI;
                        this.toolTip.SetToolTip(this.linkLabelLink, f.DisplayText);
                        if (RemoteValueBindings != null)
                        {
                            this._LinkLabelLinkColumns = new Dictionary<string, string>();
                        }
                        if (this.linkLabelLink.Text.Length > 0)
                        {
                            this.buttonGridModeInsert.Enabled = true;
                            this.buttonGetLink.Visible = false;
                            this.buttonRemoveLink.Visible = true;
                            this._LinkLabelLinkColumns.Add(DisplayColumn, f.DisplayText);
                            this._LinkLabelLinkColumns.Add(ValueColumn, f.URI);
                            if (RemoteValueBindings != null && RemoteValueBindings.Count > 0)
                            {
                                foreach (DiversityWorkbench.UserControls.RemoteValueBinding RVB in RemoteValueBindings)
                                {
                                    foreach (System.Collections.Generic.KeyValuePair<string, string> P in IWorkbenchUnit.UnitValues())
                                    {
                                        if (RVB.RemoteParameter == P.Key)
                                        {
                                            System.Data.DataRowView RV = (System.Data.DataRowView)RVB.BindingSource.Current;
                                            this._LinkLabelLinkColumns.Add(RVB.Column, P.Value);
                                        }
                                    }
                                }
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

        private void buttonRemoveLink_Click(object sender, EventArgs e)
        {
            this.linkLabelLink.Text = "";
            this.toolTip.SetToolTip(this.linkLabelLink, "");
            this.buttonGetLink.Visible = true;
            this.buttonRemoveLink.Visible = false;
            this.buttonGridModeInsert.Enabled = false;
        }



        #region Indices of Grid

        //private int DatasetIndexOfCurrentLine
        //{
        //    get
        //    {
        //        int i = 0;
        //        try
        //        {
        //            if (this.dataGridView.SelectedCells.Count > 0)
        //            {
        //                int SpecimenPartID = 0;
        //                if (int.TryParse(this.dataGridView.Rows[this.dataGridView.SelectedCells[0].RowIndex].Cells[0].Value.ToString(), out SpecimenPartID))
        //                {
        //                    for (i = 0; i < this.dataSetImageGrid.CollectionSpecimenImage.Rows.Count; i++)
        //                    {
        //                        if (this.dataSetImageGrid.CollectionSpecimenImage.Rows[i].RowState == DataRowState.Deleted
        //                            || this.dataSetImageGrid.CollectionSpecimenImage.Rows[i].RowState == DataRowState.Detached)
        //                            continue;
        //                        if (this.dataSetImageGrid.CollectionSpecimenImage.Rows[i]["SpecimenPartID"].ToString() == SpecimenPartID.ToString())
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //        catch { }
        //        return i;
        //    }
        //}

        //private int DatasetIndexOfLine(int SpecimenPartID)
        //{
        //    int i = 0;
        //    try
        //    {
        //        for (i = 0; i < this.dataSetImageGrid.CollectionSpecimenImage.Rows.Count; i++)
        //        {
        //            if (this.dataSetImageGrid.CollectionSpecimenImage.Rows[i]["SpecimenPartID"].ToString() == SpecimenPartID.ToString())
        //                break;
        //        }

        //    }
        //    catch { }
        //    return i;
        //}

        //private int GridIndexOfDataline(int SpecimenPartID)
        //{
        //    int i = 0;
        //    try
        //    {
        //        if (this.dataGridView.Rows.Count > 0)
        //        {
        //            for (i = 0; i < this.dataGridView.Rows.Count; i++)
        //            {
        //                if (this.dataGridView.Rows[i].Cells[0].Value.ToString() == SpecimenPartID.ToString())
        //                    break;
        //            }
        //        }

        //    }
        //    catch { }
        //    return i;
        //}

        #endregion

        #region Autocomplete

        private void dataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                TextBox textBox = e.Control as TextBox;
                if (textBox != null)
                {
                    string Column = this.dataGridView.Columns[this.dataGridView.SelectedCells[0].ColumnIndex].DataPropertyName.ToString();
                    // getting Table and ColumnName
                    System.Windows.Forms.BindingSource binding = (System.Windows.Forms.BindingSource)this.dataGridView.DataSource;
                    string Table = binding.DataMember;
                    System.Windows.Forms.AutoCompleteStringCollection autoCompleteStringCollection = DiversityWorkbench.Forms.FormFunctions.AutoCompleteStringCollectionOnDemand(Table, Column);
                    textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                    textBox.AutoCompleteCustomSource = autoCompleteStringCollection;
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Manual

        /// <summary>
        /// Adding event deletates to form and controls
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task InitManual()
        {
            try
            {

                DiversityWorkbench.DwbManual.Hugo manual = new Hugo(this.helpProvider, this);
                if (manual != null)
                {
                    await manual.addKeyDownF1ToForm();
                }
            }
            catch (Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        /// <summary>
        /// ensure that init is only done once
        /// </summary>
        private bool _InitManualDone = false;


        /// <summary>
        /// KeyDown of the form adding event deletates to form and controls within the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!_InitManualDone)
                {
                    await this.InitManual();
                    _InitManualDone = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        #endregion

    }
}
