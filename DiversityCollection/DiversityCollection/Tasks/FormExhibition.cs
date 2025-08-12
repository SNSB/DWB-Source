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
    public partial class FormExhibition : Form
    {

        #region Parameter

        private System.Data.DataTable _DtExhibition;
        private System.Data.DataTable _DtCollection;
        private System.Data.DataTable _DtPart;
        private System.Data.DataTable _DtPartInCollection;

        private Exhibition _Exhibition;

        private UserControls.iMainForm _iMainForm;

        private WpfControls.Geometry.UserControlGeometry _UserControlGeometry;


        #endregion

        #region Construction
        public FormExhibition(UserControls.iMainForm iMainForm, ref bool OK)
        {
            InitializeComponent();
            try
            {
                this.SuspendLayout();
                this._iMainForm = iMainForm;
                this.initForm();
                this.ResumeLayout();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                OK = false;
            }
        }

        #endregion

        #region Form

        private void initForm()
        {
            this.initExhibition();
            this.userControlPlan.setEditState(WpfControls.Geometry.UserControlGeometry.State.ReadOnly);
        }

        private void splitContainerPart_SizeChanged(object sender, EventArgs e)
        {
            splitContainerPart.SplitterDistance = splitContainerPart.Height - 133;
        }

        #endregion

        #region Exhibition

        private void initExhibition()
        {
            this._DtExhibition = Exhibition.ExhibitionList();
            this.listBoxExhibition.DataSource = this._DtExhibition;// this._DtExhibition;
            this.listBoxExhibition.DisplayMember = "DisplayText";
            this.listBoxExhibition.ValueMember = "CollectionTaskID";
            this.toolStripButtonExhibitionDelete.Enabled = this.listBoxExhibition.SelectedItem != null;
            this.toolStripButtonEditExhibition.Enabled = this.listBoxExhibition.SelectedItem != null;
            this.initParts();
            this.initCollections();
        }

        #region Toolstrip
        private void toolStripButtonExhibitionAdd_Click(object sender, EventArgs e)
        {
            int? ID = this.InsertExhibition();
            if (ID != null)
            {
                this.initExhibition();
                if (this._DtExhibition != null)
                {
                    for (int i = 0; i < this._DtExhibition.Rows.Count; i++)
                    {
                        if (this._DtExhibition.Rows[i]["CollectionTaskID"].ToString() == ID.ToString())
                        {
                            this.listBoxExhibition.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        private void toolStripButtonEditExhibition_Click(object sender, EventArgs e)
        {
            DiversityCollection.Tasks.FormCollectionTask f = new FormCollectionTask(this._Exhibition.ID, this._iMainForm);
            f.StartPosition = FormStartPosition.CenterScreen;
            f.Height = 300;
            f.Width = 800;
            f.ShowDialog();
            this.initExhibition();
        }

        private void toolStripButtonExhibitionDelete_Click(object sender, EventArgs e)
        {
            if (this.listBoxExhibition.SelectedItem != null)
            {
                int ID = 0;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxExhibition.SelectedItem;
                if (int.TryParse(R["CollectionTaskID"].ToString(), out ID))
                {
                    if (System.Windows.Forms.MessageBox.Show("Do you really want to delete the selected exbition?", "Delete exhibition", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        string Error = "";
                        if (Tasks.Exhibition.DeleteExhibition(ID, ref Error))
                        {
                            this.initExhibition();
                        }
                        else if (Error.Length > 0)
                        {
                            System.Windows.Forms.MessageBox.Show(Error);
                        }
                    }
                }
            }
        }

        private void toolStripButtonResponsible_Click(object sender, EventArgs e)
        {
            DiversityCollection.Forms.FormCustomizeDisplay form = new Forms.FormCustomizeDisplay(null, Forms.FormCustomizeDisplay.Customization.Responsible);
            form.ShowDialog();
        }

        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            string ID = "";
            if (this._Exhibition != null)
                ID = this._Exhibition.ID.ToString();
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                ID);
        }

        #endregion

        private int? ExhibitionID
        {
            get
            {
                if (this.listBoxExhibition.SelectedItem != null)
                {
                    int ID;
                    System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxExhibition.SelectedItem;
                    if (int.TryParse(R["CollectionTaskID"].ToString(), out ID))
                        return ID;
                }
                return null;
            }
        }

        private int? InsertExhibition()
        {
            int? CollectionTaskID = Exhibition.InsertExhibition();
            return CollectionTaskID;

        }

        //private bool GetTaskIDs(ref int ExhibitionTaskID, ref int PartTaskID)
        //{
        //    bool OK = false;
        //    string SQL = "SELECT  TOP 1  E.TaskID, P.TaskID AS PartTaskID " +
        //        "FROM Task AS E INNER JOIN " +
        //        "Task AS P ON E.TaskID = E.TaskParentID " +
        //        "AND (E.Type = N'exhibition') AND (P.Type = N'part') " +
        //        "ORDER BY E.TaskID DESC ";
        //    System.Data.DataTable dtTask = new DataTable();
        //    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtTask);
        //    if (dtTask.Rows.Count == 0)
        //    {
        //        SQL = "INSERT INTO Task (DisplayText, Type, DateType, DateBeginType, DateEndType) " +
        //            "VALUES('Exhibition', 'Exhibition', 'Date from to', 'from', 'until')";
        //        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
        //        {
        //            SQL = "SELECT  TOP 1  E.TaskID " +
        //                "FROM Task AS E " +
        //                "WHERE (E.Type = N'exhibition') " +
        //                "ORDER BY E.TaskID DESC ";
        //            if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ExhibitionTaskID))
        //            {
        //                SQL = "INSERT INTO Task (TaskParentID, DisplayText, Type, SpecimenPartType) " +
        //                    "VALUES(" + ExhibitionTaskID.ToString() + ", 'Part', 'Part', 'Part')";
        //                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
        //                {
        //                    SQL = "SELECT TOP 1  E.TaskID " +
        //                        "FROM Task AS E " +
        //                        "WHERE (E.Type = N'Part') AND TaskParentID = " + ExhibitionTaskID.ToString() +
        //                        " ORDER BY E.TaskID DESC ";
        //                    if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out PartTaskID))
        //                    {
        //                        OK = true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        if (!int.TryParse(dtTask.Rows[0][0].ToString(), out ExhibitionTaskID) || !int.TryParse(dtTask.Rows[0][1].ToString(), out PartTaskID))
        //            OK = false;
        //    }
        //    return OK;
        //}

        private void listBoxExhibition_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxExhibition.SelectedItem;
                int CollectionTaskID;
                if (int.TryParse(R["CollectionTaskID"].ToString(), out CollectionTaskID))
                {
                    if (this._Exhibition == null)
                        this._Exhibition = new Exhibition(CollectionTaskID);
                    else
                        this._Exhibition.ID = CollectionTaskID;
                    this.groupBoxExhibition.Text = this._Exhibition.DisplayText;
                    this.initCollections();// CollectionTaskID);
                    this.initParts();
                }
                this.toolStripButtonExhibitionDelete.Enabled = this.listBoxExhibition.SelectedItem != null;
                this.toolStripButtonEditExhibition.Enabled = this.listBoxExhibition.SelectedItem != null;
            }
            catch(System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #endregion

        #region Collection


        private void initCollections()//int CollectionTaskID)
        {
            if (this._Exhibition != null && this._Exhibition.Collections().Rows.Count > 1)
            {
                this._DtCollection = this._Exhibition.Collections();
                this.listBoxCollections.DataSource = this._DtCollection;
                this.listBoxCollections.DisplayMember = "DisplayText";
                this.listBoxCollections.ValueMember = "CollectionID";
                this.ShowCollectionControls();
            }
            else
            {
                this.ShowCollectionControls(false);
            }
        }

        private void ShowCollectionControls(bool Show = true)
        {
            this.splitContainerCollectionAndParts.Panel1Collapsed = !Show;
            if (!Show) this.groupBoxCollection.Text = "";
        }


        private void toolStripButtonCollectionAdd_Click(object sender, EventArgs e)
        {

        }

        private void listBoxCollections_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CollID = 0;
            if (this.listBoxCollections.SelectedItem != null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxCollections.SelectedItem;
                this.groupBoxCollection.Text = R["DisplayText"].ToString();
                if (int.TryParse(R["CollectionID"].ToString(), out CollID) && this.listBoxCollections.SelectedIndex > 0)
                {
                    this.initParts(CollID);
                    if (this.ShowCollectionPlan)
                    {
                        this.userControlPlan.SetCollectionID(CollID);
                        this.userControlPlan.Enabled = true;
                    }
                    this.toolStripLabelCollectionType.Image = Specimen.CollectionTypeImage(false, R["Type"].ToString());
                    this.initPartsInCollection(CollID);
                }
                else
                {
                    this.initParts();
                    if (this.ShowCollectionPlan)
                    {
                        this.userControlPlan.SetCollectionID(-1);
                        this.userControlPlan.Enabled = false;
                    }
                    this.initPartsInCollection();
                    this.userControlPlan.Enabled = false;
                    this.toolStripLabelCollectionType.Image = DiversityCollection.Resource.NULL;
                }
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private bool ShowCollectionPlan = false;

        private void toolStripButtonCollectionShowPlan_Click(object sender, EventArgs e)
        {
            this.ShowCollectionPlan = !this.ShowCollectionPlan;
            if (this.ShowCollectionPlan)
            {
                this.toolStripButtonCollectionShowPlan.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                this.toolStripButtonCollectionShowPlan.BackColor = System.Drawing.SystemColors.Control;
            }
            this.splitContainerPrintAndPlan.Panel1Collapsed = !this.ShowCollectionPlan;
            this.splitContainerPrintAndPlan.Panel2Collapsed = !this.ShowPrinting;
            this.splitContainerCollection.Panel2Collapsed = !this.ShowCollectionPlan && !this.ShowPrinting;
        }

        #region Printing

        private bool ShowPrinting = false;
        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            this.ShowPrinting = !this.ShowPrinting;
            if (this.ShowPrinting)
            {
                this.toolStripButtonPrint.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                this.toolStripButtonPrint.BackColor = System.Drawing.SystemColors.Control;
            }
            this.splitContainerPrintAndPlan.Panel1Collapsed = !this.ShowCollectionPlan;
            this.splitContainerPrintAndPlan.Panel2Collapsed = !this.ShowPrinting;
            this.splitContainerCollection.Panel2Collapsed = !this.ShowCollectionPlan && !this.ShowPrinting;
        }

        private void toolStripButtonReportOpenSchemaFile_Click(object sender, EventArgs e)
        {
            string Path = Folder.Report(Folder.ReportFolder.TaskSchemaCollection);
            if (this.textBoxReportSchemaFile.Text.Length > 0)
            {
                try
                {
                    System.IO.FileInfo FI = new System.IO.FileInfo(this.textBoxReportSchemaFile.Text);
                    if (FI.Exists)
                        Path = FI.DirectoryName;
                }
                catch { }
            }
            this.openFileDialogReportSchema = new OpenFileDialog();
            this.openFileDialogReportSchema.RestoreDirectory = true;
            this.openFileDialogReportSchema.Multiselect = false;
            this.openFileDialogReportSchema.InitialDirectory = Path;
            this.openFileDialogReportSchema.Filter = "XSLT Files|*.xslt";
            try
            {
                this.openFileDialogReportSchema.ShowDialog();
                if (this.openFileDialogReportSchema.FileName.Length > 0)
                {
                    this.textBoxReportSchemaFile.Tag = this.openFileDialogReportSchema.FileName;
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialogReportSchema.FileName);
                    this.textBoxReportSchemaFile.Text = f.FullName;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        private void toolStripButtonReportCreate_Click(object sender, EventArgs e)
        {
            if (this.listBoxCollections.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a collection in the list", "Nothing selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //System.Collections.Generic.List<int> CollIDs = new List<int>();
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxCollections.SelectedItem;
            int? CollID = null;// = Tasks.Settings.Default.TopCollectionID;
            int i;
            if (int.TryParse(R["CollectionID"].ToString(), out i))// && this.listBoxCollections.SelectedIndex > 0)
                CollID = i;
            System.IO.FileInfo XmlFile = new System.IO.FileInfo(Folder.Report() + "Collection.XML");
            DiversityCollection.XmlExport xmlExport = new XmlExport(this.textBoxReportSchemaFile.Text, XmlFile.FullName);
            System.Data.DataRowView row = (System.Data.DataRowView)this.listBoxExhibition.SelectedItem;
            string Title = row["DisplayText"].ToString();
            string File = xmlExport.CreateXmlForCollectionTask(this._Exhibition, CollID, Title, XmlExport.QRcodeSourceCollectionTask.None, this.checkBoxReportPlan.Checked);
            if (File.Length > 0)
            {
                try
                {
                    System.Uri URI = new Uri(File);
                    this.webBrowserReport.Url = URI;
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private void toolStripButtonReportPrint_Click(object sender, EventArgs e)
        {
            this.webBrowserReport.ShowPrintPreviewDialog();
        }

        #endregion

        #region PartsInCollection

        private void initPartsInCollection(int? CollectionID = null)
        {
            if (CollectionID != null)
            {
                this._DtPartInCollection = this._Exhibition.PartsInCollection((int)CollectionID);
                if (_DtPartInCollection.Rows.Count > 0)
                {
                    this.listBoxPartsInCollection.DataSource = this._DtPartInCollection;
                    this.listBoxPartsInCollection.DisplayMember = "DisplayText";
                    this.listBoxPartsInCollection.ValueMember = "CollectionSpecimenID";
                    this.splitContainerPartsInCollection.Panel2Collapsed = false;
                }
                else
                {
                    this.splitContainerPartsInCollection.Panel2Collapsed = true;
                }
            }
            else
            {
                this.splitContainerPartsInCollection.Panel2Collapsed = true;
            }
        }

        private void toolStripButtonPartsInCollectionDetails_Click(object sender, EventArgs e)
        {
            if (this.listBoxPartsInCollection.SelectedItem != null)
            {
                this.Cursor = Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPartsInCollection.SelectedItem;
                int ID = 0;
                if (int.TryParse(R["CollectionSpecimenID"].ToString(), out ID))
                {
                    DiversityCollection.Forms.FormCollectionSpecimen form = new DiversityCollection.Forms.FormCollectionSpecimen(ID,false, false, Forms.FormCollectionSpecimen.ViewMode.SingleInspectionMode, true);
                    form.ShowDialog();
                }
                this.Cursor=Cursors.Default;
            }
        }

        #endregion

        #endregion

        #region Parts

        private void initParts(int? CollectionID = null)
        {
            if (CollectionID !=null)
            {
                this.listBoxParts.DataSource = this._Exhibition.Parts((int)CollectionID);
            }
            else
            {
                if (this._Exhibition != null)
                    this.listBoxParts.DataSource = this._Exhibition.Parts();
            }
            bool HasItems = false;
            if (this._Exhibition != null && listBoxParts.DataSource != null)
            {
                if (listBoxParts.DataSource.GetType() == typeof(System.Data.DataTable))
                {
                    System.Data.DataTable dt = (System.Data.DataTable)listBoxParts.DataSource;
                    if (dt.Rows.Count > 0)
                        HasItems = true;
                }
                else if (listBoxParts.DataSource.GetType() == typeof(System.Data.DataView))
                {
                    System.Data.DataView view = (System.Data.DataView)listBoxParts.DataSource;
                    System.Data.DataTable dt = view.ToTable();
                    if (dt.Rows.Count > 0)
                        HasItems = true;
                }
            }
            if (HasItems)
            {
                this.listBoxParts.DisplayMember = "DisplayText";
                this.listBoxParts.ValueMember = "SpecimenPartID";
            }
            this.splitContainerPart.Panel2Collapsed = !HasItems;
            this.groupBoxExhibition.Enabled = this._Exhibition != null;
            this.toolStripButtonPartDelete.Enabled = this.listBoxParts.SelectedItem != null;
            this.toolStripButtonPartEdit.Enabled = this.listBoxParts.SelectedItem != null;
        }

        #region Toolstrip

        private void toolStripButtonGetParts_Click(object sender, EventArgs e)
        {
            if (this.ExhibitionID == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select an exhibition");
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (this._Exhibition.InsertParts())
                this.initParts();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        /// <summary>
        /// Removing a part from an exhibition
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonPartDelete_Click(object sender, EventArgs e)
        {
            if (this.ExhibitionID == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select an exhibition");
                return;
            }
            if (this.listBoxParts.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the part that should be removed from the exhibition");
                return;
            }
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxParts.SelectedItem;
            if (R != null)
            {
                int PartID;
                if (int.TryParse(R["CollectionTaskID"].ToString(), out PartID))
                {
                    string Error = "";
                    if (this._Exhibition.DeletePart(PartID, ref Error))
                        this.initParts();
                    else
                    {
                        string Message = "Failed to remove part from exhibition";
                        if (Error.Length > 0)
                            Message += ": " + Error;
                        System.Windows.Forms.MessageBox.Show(Message);
                    }
                }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void toolStripButtonPartEdit_Click(object sender, EventArgs e)
        {
            if (this.listBoxParts.SelectedItem != null)
            {
                int TaskID;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxParts.SelectedItem;
                if(int.TryParse(R["CollectionTaskID"].ToString(), out TaskID))
                {
                    DiversityCollection.Tasks.FormCollectionTask f = new FormCollectionTask(TaskID, this._iMainForm);
                    f.StartPosition = FormStartPosition.CenterScreen;
                    f.Height = 300;
                    f.Width = 800;
                    f.ShowDialog();
                    this.initExhibition();
                }
            }
        }

        #endregion

        private void initPart()
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxParts.SelectedItem;
            int PartID;
            if (this.listBoxParts.SelectedItem != null && int.TryParse(R["SpecimenPartID"].ToString(), out PartID))
            {
                this.listBoxUnits.DataSource = this._Exhibition.Units(PartID);
                this.listBoxUnits.DisplayMember = "LastIdentificationCache";
                this.textBoxAccessionNumber.Text = R["AccessionNumber"].ToString();
                this.textBoxStorageLocation.Text = R["StorageLocation"].ToString();
                this.textBoxOriginalCollection.Text = _Exhibition.OriginalCollection(PartID); 
                this.pictureBoxMaterialCategory.Image = Specimen.MaterialCategoryImage(false, R["MaterialCategory"].ToString());
            }
            else
            {
                this.listBoxUnits.DataSource = null;
                this.textBoxAccessionNumber.Text = "";
                this.textBoxStorageLocation.Text = "";
                this.pictureBoxMaterialCategory.Image = null;
                this.textBoxOriginalCollection.Text = "";
            }
            this.toolStripButtonPartDelete.Enabled = this.listBoxParts.SelectedItem != null;
            this.toolStripButtonPartEdit.Enabled = this.listBoxParts.SelectedItem != null;
        }

        private void listBoxParts_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.initPart();
        }

        private void toolStripButtonPartDetails_Click(object sender, EventArgs e)
        {
            if (this.listBoxParts.SelectedItem != null)
            {
                this.Cursor = Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxParts.SelectedItem;
                int ID = 0;
                if (int.TryParse(R["CollectionSpecimenID"].ToString(), out ID))
                {
                    DiversityCollection.Forms.FormCollectionSpecimen form = new DiversityCollection.Forms.FormCollectionSpecimen(ID, false, false, Forms.FormCollectionSpecimen.ViewMode.SingleInspectionMode, true);
                    form.ShowDialog();
                }
                this.Cursor = Cursors.Default;
            }
        }

        #endregion

    }
}
