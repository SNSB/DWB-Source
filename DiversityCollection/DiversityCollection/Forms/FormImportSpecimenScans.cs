using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormImportSpecimenScans : Form
    {
        #region Construction
        public FormImportSpecimenScans()
        {
            InitializeComponent();
            this.setEnumSource();
            this.setLogfilePath();
            this.initForm();
            this.initRemoteModules();
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }
        #endregion

        #region Form
        private void initForm()
        {
            if (DiversityCollection.Forms.FormImportImagesSettings.Default.FolderOriginal.Length > 0)
                this.textBoxFolderOriginal.Text = DiversityCollection.Forms.FormImportImagesSettings.Default.FolderOriginal;
            if (DiversityCollection.Forms.FormImportImagesSettings.Default.WebFolder.Length > 0)
                this.textBoxWebFolder.Text = DiversityCollection.Forms.FormImportImagesSettings.Default.WebFolder;
            this.setMandatoryIndications();
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxCollection);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxImageType);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxLabelType);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxMaterialCategory);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxProject);
            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this.comboBoxTaxonomicGroup);
        }

        private void setEnumSource()
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxMaterialCategory, "CollMaterialCategory_Enum", con);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTaxonomicGroup, "CollTaxonomicGroup_Enum", con);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxImageType, "CollSpecimenImageType_Enum", con);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxLabelType, "CollLabelType_Enum", con);
            // Collection
            System.Data.DataTable dtCollection = new System.Data.DataTable("Collection");
            string SQL = "SELECT CollectionID, CollectionName from dbo.Collection WHERE CollectionName IS NOT NULL ORDER BY Collectionname";
            Microsoft.Data.SqlClient.SqlDataAdapter a = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, con);
            a.Fill(dtCollection);
            this.comboBoxCollection.DataSource = dtCollection;
            this.comboBoxCollection.DisplayMember = "CollectionName";
            this.comboBoxCollection.ValueMember = "CollectionID";
            // Projects
            System.Data.DataTable dtProjects = new System.Data.DataTable("Projects");
            a.SelectCommand.CommandText = "SELECT ProjectID, Project FROM  ProjectProxy ORDER BY Project";
            a.Fill(dtProjects);
            this.comboBoxProject.DataSource = dtProjects;
            this.comboBoxProject.DisplayMember = "Project";
            this.comboBoxProject.ValueMember = "ProjectID";
        }

        private void buttonSetUrl_Click(object sender, System.EventArgs e)
        {
            try
            {
                string Folder = "";
                if (Folder.IndexOf(":") > 0) Folder = Folder.Substring(Folder.IndexOf(":") + 1, Folder.Length - Folder.IndexOf(":") - 1);
                Folder = Folder.Replace(@"\", "/");
                this.textBoxWebFolder.Text = Folder;
            }
            catch { }
        }

        private void FormImportSpecimenScans_FormClosing(object sender, FormClosingEventArgs e)
        {
            DiversityCollection.Forms.FormImportImagesSettings.Default.FolderOriginal = this.textBoxFolderOriginal.Text;
            DiversityCollection.Forms.FormImportImagesSettings.Default.WebFolder = this.textBoxWebFolder.Text;
            DiversityCollection.Forms.FormImportImagesSettings.Default.Save();
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name, System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
        }

        #endregion

        #region Remote modules
        private void initRemoteModules()
        {
            try
            {
                // Agents
                DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryCollector.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryCollector.setTableSource("CollectionAgent", "CollectorsName", "CollectorsAgentURI", "");

                this.userControlModuleRelatedEntryIdentifiedBy.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryIdentifiedBy.setTableSource("Identification", "ResponsibleName", "ResponsibleAgentURI", "");

                this.userControlModuleRelatedEntryDepositor.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
                this.userControlModuleRelatedEntryDepositor.setTableSource("CollectionSpecimen", "DepositorsName", "DepositorsAgentURI", "");

                // Exsiccatae
                DiversityWorkbench.Exsiccate E = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryExsiccate.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)E;
                this.userControlModuleRelatedEntryExsiccate.setTableSource("CollectionSpecimen", "ExsiccataAbbreviation", "ExsiccataURI", "");

                // Gazetteer
                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryGazetteer.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;
                string[] ValueColumns = new string[4] {"Altitude", "Longitude", "Latitude", "Country" };
                this.userControlModuleRelatedEntryGazetteer.setTableSource("CollectionEventLocalisation", "Location1", "Location2", "LocalisationSystemID = 7", ValueColumns);

                // TaxonNames
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                this.userControlModuleRelatedEntryIdentification.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)T;
                this.setIdentificationSource();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void comboBoxTaxonomicGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.setIdentificationSource();
        }

        private void setIdentificationSource()
        {
            if (this.comboBoxTaxonomicGroup.Text.Length > 0)
            {
                string WhereClause = " EXISTS (SELECT * FROM IdentificationUnit U " +
                      "WHERE U.CollectionSpecimenID = Identification.CollectionSpecimenID " +
                      "AND U.IdentificationUnitID = U.IdentificationUnitID " +
                      "AND U.TaxonomicGroup = '" + this.comboBoxTaxonomicGroup.SelectedValue.ToString() + "')";
                this.userControlModuleRelatedEntryIdentification.setTableSource("Identification", "TaxonomicName", "NameURI", WhereClause);
            }
            else
                this.userControlModuleRelatedEntryIdentification.setTableSource("Identification", "TaxonomicName", "NameURI", "");
        }

        #endregion

        #region Set folders
        //private void buttonFolderArchive_Click(object sender, EventArgs e)
        //{
        //    //this.SetFolder(this.textBoxFolderArchive);
        //}

        //private void buttonFolderWeb_Click(object sender, EventArgs e)
        //{
        //    //this.SetFolder(this.textBoxFolderWeb);
        //}

        //private void buttonFolderPreview_Click(object sender, EventArgs e)
        //{
        //    //this.SetFolder(this.textBoxFolderPreview);
        //}

        //private void SetFolder(System.Windows.Forms.TextBox textBox)
        //{
        //    this.folderBrowserDialogImport = new FolderBrowserDialog();
        //    this.folderBrowserDialogImport.RootFolder = System.Environment.SpecialFolder.Recent;
        //    //if (textBox.Text.Length > 0) this.folderBrowserDialogImport.RootFolder = Environment.SpecialFolder.Recent
        //    this.folderBrowserDialogImport.ShowDialog();
        //    if (this.folderBrowserDialogImport.SelectedPath.Length > 0)
        //        textBox.Text = this.folderBrowserDialogImport.SelectedPath;
        //}

        #endregion

        #region Import Filenames
        private string extractAccessionNumber(System.IO.FileInfo f)
        {
            string AccNr = "";
            if (this.checkBoxCutAfterSeparator.Checked && f.Name.IndexOfAny(this.textBoxAccessionSeparator.Text.ToCharArray()) > -1)
                AccNr = f.Name.Substring(0, f.Name.IndexOfAny(this.textBoxAccessionSeparator.Text.ToCharArray()));
            else
                AccNr = f.Name.Substring(0, (f.Name.Length - f.Extension.Length));
            return AccNr;
        }

        private bool AccessionNumberIsPresent(string AccessionNumber)
        {
            bool IsPresent = true;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand("SELECT COUNT(*) FROM CollectionSpecimen WHERE AccessionNumber = '" + AccessionNumber + "'", con);
            con.Open();
            string i = cmd.ExecuteScalar().ToString();
            con.Close();
            if (i == "0") IsPresent = false;
            return IsPresent;
        }

        //private string attachDateTimeToFileName(System.IO.FileInfo f)
        //{
        //    string NewName = f.Name;
        //    //if (this.checkBoxAppendDateToFilename.Checked)
        //    //{
        //    //    char Pad = '0';
        //    //    string Year = f.LastWriteTime.Year.ToString();
        //    //    string Month = f.LastWriteTime.Month.ToString();
        //    //    Month = Month.PadLeft(2, Pad);
        //    //    string Day = f.LastWriteTime.Day.ToString();
        //    //    Day = Day.PadLeft(2, Pad);
        //    //    string Hour = f.LastWriteTime.Hour.ToString();
        //    //    Hour = Hour.PadLeft(2, Pad);
        //    //    string Min = f.LastWriteTime.Minute.ToString();
        //    //    Min = Min.PadLeft(2, Pad);
        //    //    string Sec = f.LastWriteTime.Second.ToString();
        //    //    Sec = Sec.PadLeft(2, Pad);
        //    //    NewName = f.Name.Substring(0, f.Name.Length - f.Extension.Length);
        //    //    NewName += "_" + Year + Month + Day + "_" + Hour + Min + Sec;
        //    //    NewName += f.Extension;
        //    //}
        //    return NewName;
        //}

        private void buttonFolderOriginal_Click(object sender, EventArgs e)
        {
            this.openFileDialogImport = new OpenFileDialog();
            if (this.textBoxFolderOriginal.Text.Length > 0)
            {
                if (System.IO.Directory.Exists(this.textBoxFolderOriginal.Text))
                {
                    this.openFileDialogImport.InitialDirectory = this.textBoxFolderOriginal.Text;
                }
            }
            this.openFileDialogImport.Multiselect = true;
            this.openFileDialogImport.ShowDialog();
            if (this.openFileDialogImport.FileNames.GetLength(0) > 0)
            {
                System.IO.FileInfo FI = new System.IO.FileInfo(this.openFileDialogImport.FileName);
                this.textBoxFolderOriginal.Text = FI.FullName.Substring(0, FI.FullName.Length - FI.Name.Length);
                this.dataGridViewImport.Rows.Clear();
                this.buttonStartImport.Enabled = false;
                foreach (string f in this.openFileDialogImport.FileNames)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(f);
                    if (fi.Extension == ".jpg" ||
                        fi.Extension == ".png" ||
                        fi.Extension == ".bmp" ||
                        fi.Extension == ".tif" ||
                        fi.Extension == ".gif")
                    {
                        System.Object[] o = new object[8];

                        try
                        {
                            string AccNr = this.extractAccessionNumber(fi);
                            o[1] = AccNr;
                            o[2] = fi.Name;
                            if (this.checkBoxSubfolder.Checked)
                            {
                                if (this.checkBoxImageAsUri.Checked)
                                    o[3] = this.textBoxWebFolder.Text + fi.Name.Substring(0, (int)this.numericUpDownSubfolder.Value) + "/" + fi.Name;
                                else
                                    o[3] = this.textBoxFolderOriginal.Text + fi.Name.Substring(0, (int)this.numericUpDownSubfolder.Value) + "/" + fi.Name;
                            }
                            else
                            {
                                if (this.checkBoxImageAsUri.Checked)
                                    o[3] = this.textBoxWebFolder.Text + fi.Name;
                                else
                                    o[3] = this.textBoxFolderOriginal.Text + "/" + fi.Name;
                            }
                            string Error = this.AccessionNumberIsOK(AccNr);
                            if (Error.Length == 0) o[0] = true;
                            else o[0] = false;
                            o[4] = Error;
                            o[5] = false;
                            this.dataGridViewImport.Rows.Add(o);
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                    }
                }
                if (this.dataGridViewImport.RowCount > 0)
                {
                    this.groupBoxDatabase.Enabled = true;
                }
            }
        }

        private void fillDataGridRow(System.Windows.Forms.DataGridViewRow r)
        {
            if (r.Cells[2].Value.ToString().Length > 0)
            {
                string AccNr = r.Cells[1].Value.ToString();
                System.IO.FileInfo fi = new System.IO.FileInfo(r.Cells[2].Value.ToString());
                string Error = this.AccessionNumberIsOK(AccNr);
                if (Error.Length == 0) r.Cells[0].Value = true;
                else r.Cells[0].Value = false;
                r.Cells["ColumnError"].Value = Error;
                r.Cells["ColumnAppend"].Value = false;
            }
        }

        private string AccessionNumberIsOK(string AccessionNumber)
        {
            string Error = "";
            if (this.checkBoxBarcodeStart.Checked)
            {
                if (AccessionNumber.Length < this.textBoxBarcodeStart.Text.Length) Error += "Acc.Nr. <> " + this.textBoxBarcodeStart.Text + "... . ";
                else
                    if (!(AccessionNumber.Substring(0, this.textBoxBarcodeStart.Text.Length) == this.textBoxBarcodeStart.Text)) Error += "Acc.Nr. <> " + this.textBoxBarcodeStart.Text + "... . ";
            }
            if (this.checkBoxBarcodeLength.Checked)
                if (!(AccessionNumber.Length == System.Int32.Parse(this.textBoxBarcodeLength.Text))) Error += "Acc.Nr. length <> " + this.textBoxBarcodeLength.Text + ". ";
            return Error;
        }

        //private void dataGridViewImport_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    //System.Windows.Forms.DataGridView v = (System.Windows.Forms.DataGridView)sender;
        //    //System.Windows.Forms.DataGridViewCell c = v.SelectedCells[0];
        //    //if (c.ColumnIndex == 8)
        //    //{
        //    //    this.fillDataGridRow(this.dataGridViewImport.Rows[c.RowIndex]);
        //    //    this.checkDataGridRow(this.dataGridViewImport.Rows[c.RowIndex]);
        //    //}
        //}

        #endregion

        #region Test and prepare

        private void buttonTestImport_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (this.CheckDestinationURL() && this.readyForImport()) //&& this.CheckImageSource()
            {
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewImport.Rows)
                {
                    this.fillDataGridRow(R);
                }
                this.CheckRows();
                this.setSubfolderExample();
            }
            else
                this.buttonStartImport.Enabled = false;
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private bool CheckRows()
        {
            bool OK = true;
            bool AnyValidLine = false;
            foreach (System.Windows.Forms.DataGridViewRow r in this.dataGridViewImport.Rows)
            {
                string Error = "";
                string URL = "";
                try
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(this.textBoxFolderOriginal.Text + "\\" + r.Cells[2].Value.ToString());
                    r.Cells[1].Value = this.extractAccessionNumber(fi);
                    Error += this.AccessionNumberIsOK(r.Cells[1].Value.ToString());
                    if (!this.checkBoxAppendImages.Checked)
                    {
                        if (this.AccessionNumberIsPresent(r.Cells[1].Value.ToString()))
                            Error += "Acc.Nr. allready in DB";
                    }
                    else
                    {
                        if (this.AccessionNumberIsPresent(r.Cells[1].Value.ToString()))
                            r.Cells["ColumnAppend"].Value = true;
                        else
                            r.Cells["ColumnAppend"].Value = false;
                    }
                    if (this.checkBoxMoveTogether.Checked)
                    {
                        for (int i = 0; i < r.Cells[0].RowIndex; i++)
                        {
                            if (this.dataGridViewImport.Rows[i].Cells[1].Value.ToString() == r.Cells[1].Value.ToString())
                            {
                                r.Cells["ColumnAppend"].Value = true;
                                break;
                            }
                        }
                    }
                    if (!this.checkBoxOverwriteImages.Checked)
                    {
                        //if (this.checkBoxWebImages.Checked)
                        //{
                        //    string Dir = this.textBoxFolderWeb.Text;
                        //    if (!Dir.EndsWith(@"\")) Dir += @"\";
                        //    if (this.checkBoxSubdirectory.Checked)
                        //    {
                        //        if (r.Cells[1].Value.ToString().Length < (int)this.numericUpDownSubdirectry.Value)
                        //            Error += "Acc.Nr. shorter than subdirectory.  ";
                        //        else
                        //            Dir += r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value) + @"\";
                        //    }
                        //    if (r.Cells[4].Value != null)
                        //    {
                        //        System.IO.FileInfo f = new System.IO.FileInfo(Dir + r.Cells[4].Value.ToString());
                        //        if (f.Exists)
                        //            Error += "File for web image exists. ";
                        //    }
                        //}
                        //if (this.checkBoxPreviewImages.Checked)
                        //{
                        //    string Dir = this.textBoxFolderPreview.Text;
                        //    if (!Dir.EndsWith(@"\")) Dir += @"\";
                        //    if (this.checkBoxSubdirectory.Checked)
                        //    {
                        //        if (r.Cells[1].Value.ToString().Length < (int)this.numericUpDownSubdirectry.Value)
                        //            Error += "Acc.Nr. shorter than subdirectory.  ";
                        //        else
                        //            Dir += r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value) + @"\";
                        //    }
                        //    if (r.Cells[5].Value != null)
                        //    {
                        //        System.IO.FileInfo f = new System.IO.FileInfo(Dir + r.Cells[5].Value.ToString());
                        //        if (f.Exists)
                        //            Error += "File for preview image exists. ";
                        //    }
                        //}
                    }
                    if (this.checkBoxSubfolder.Checked)
                    {
                        if (r.Cells[1].Value != null && r.Cells[1].Value.ToString().Length < this.numericUpDownSubfolder.Value)
                        {
                            Error += "Accession number shorter than path for subdirectory. ";
                        }
                        if (this.checkBoxImageAsUri.Checked)
                            r.Cells[3].Value = this.textBoxWebFolder.Text + fi.Name.Substring(0, (int)this.numericUpDownSubfolder.Value) + "/" + fi.Name;
                        else
                            r.Cells[3].Value = this.textBoxFolderOriginal.Text + fi.Name.Substring(0, (int)this.numericUpDownSubfolder.Value) + "/" + fi.Name;
                    }
                    else
                    {
                        if (this.checkBoxImageAsUri.Checked)
                            r.Cells[3].Value = this.textBoxWebFolder.Text + fi.Name;
                        else
                            r.Cells[3].Value = this.textBoxFolderOriginal.Text + "/" + fi.Name;
                    }
                    URL = r.Cells["ColumnPathInDB"].Value.ToString();
                    if (URL.Length > 0)
                    {
                        System.Uri U = new Uri(URL);
                        if (!U.IsWellFormedOriginalString() && this.checkBoxImageAsUri.Checked)
                            Error = "corrupt URL. ";
                        Error += this.checkIfImageIsInDB(r.Cells[1].Value.ToString(), URL);
                    }
                    else
                    {
                        Error += "Missing URL. ";
                    }
                    if (Error.Length > 0)
                    {
                        r.Cells["ColumnError"].Value = Error;
                        r.Cells[0].Value = false;
                    }
                    else
                    {
                        r.Cells["ColumnError"].Value = "OK";
                        r.Cells[0].Value = true;
                        AnyValidLine = true;
                    }
                    if (this.checkBoxCheckURIs.Checked && this.checkBoxImageAsUri.Checked)
                    {
                        Error += "  " + this.CheckURI(r.Cells[3].Value.ToString());
                        Error = Error.Trim();
                        if (Error.Length == 0)
                            r.Cells["ColumnError"].Value = "OK";
                        else
                        {
                            r.Cells["ColumnError"].Value = Error; 
                            r.Cells[0].Value = false;
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    OK = false;
                    r.Cells["ColumnError"].Value = Error + ex.Message;
                    r.Cells[0].Value = false;
                }
            }
            if (AnyValidLine) this.buttonStartImport.Enabled = true;
            else this.buttonStartImport.Enabled = false;
            return OK;
        }

        private void setSubfolderExample()
        {
            if (this.dataGridViewImport.Rows.Count > 0)
            {
                string AccNr = "";
                foreach (System.Windows.Forms.DataGridViewRow r in this.dataGridViewImport.Rows)
                {
                    if (bool.Parse(r.Cells[0].Value.ToString()) == true)
                    {
                        AccNr = r.Cells[1].Value.ToString();
                        break;
                    }
                }
                //if (AccNr.Length > this.numericUpDownSubdirectry.Value)
                //{
                //    string SubDir = "...\\" + AccNr.Substring(0, (int)this.numericUpDownSubdirectry.Value);
                //    this.textBoxSubdirectory.Text = SubDir;
                //}
                //else
                //    System.Windows.Forms.MessageBox.Show("The length of the accession number " + AccNr + " is lower than the length you specified for your subdirectories");
            }
        }

        private bool readyForImport()
        {
            bool OK = true;
            if (this.checkBoxImageAsUri.Checked)
            {
                if (this.textBoxWebFolder.Text.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("The folder for the URL\r\nis missing");
                    return false;
                }
                System.Uri URI = new Uri(this.textBoxWebFolder.Text);
                if (!URI.IsWellFormedOriginalString())
                {
                    System.Windows.Forms.MessageBox.Show("The folder for the URL\r\n" + this.textBoxWebFolder.Text + "\r\nis not valid");
                    return false;
                }
            }
            if (this.comboBoxProject.SelectedIndex == -1 || this.comboBoxProject.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project");
                return false;
            }
            if (this.comboBoxTaxonomicGroup.SelectedIndex == -1 || this.comboBoxTaxonomicGroup.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a taxonomic group");
                return false;
            }
            if (this.comboBoxMaterialCategory.SelectedIndex == -1 || this.comboBoxMaterialCategory.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a material category");
                return false;
            }
            if (this.comboBoxImageType.SelectedIndex == -1 || this.comboBoxImageType.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select an image type");
                return false;
            }
            if (this.comboBoxCollection.SelectedIndex == -1 || this.comboBoxCollection.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a collection");
                return false;
            }
            return OK;
        }

        private string CheckURI(string URI)
        {
            string Error = "";
            try
            {
                System.Net.WebRequest webrq = System.Net.WebRequest.Create(URI);
                System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webrq.GetResponse().GetResponseStream());
            }
            catch 
            {
                Error = "Wrong URI";//: " + URI;
            }
            return Error;
        }
        
        #endregion   
     
        #region Convert images
        //private void buttonConvertImages_Click(object sender, EventArgs e)
        //{
        //    // only conversion of images, no import in database
        //    //if (this.readyForImageConversion() && this.CheckRowsForConversion())
        //    //{
        //    //    int i = 0;
        //    //    foreach (System.Windows.Forms.DataGridViewRow r in this.dataGridViewImport.Rows)
        //    //    {
        //    //        string Error = "";
        //    //        if (System.Boolean.Parse(r.Cells[0].Value.ToString()) && (r.Cells[6].Value.ToString().Length == 0 || r.Cells[6].Value.ToString() == "OK"))
        //    //        {
        //    //            try
        //    //            {
        //    //                //Error += this.ConvertAndSaveImage(r);
        //    //                if (Error.Length > 0)
        //    //                    r.Cells[6].Value = Error;
        //    //                else
        //    //                {
        //    //                    r.Cells[6].Value = "OK";
        //    //                    i++;
        //    //                }
        //    //            }
        //    //            catch (System.Exception ex)
        //    //            {
        //    //                System.Windows.Forms.MessageBox.Show(ex.Message);
        //    //            }
        //    //        }
        //    //    }
        //    //    System.Windows.Forms.MessageBox.Show(i.ToString() + " images converted");
        //    //}
        //    //else
        //    //    System.Windows.Forms.MessageBox.Show("Convesion failed");
        //}
        
        #endregion

        #region Import
        private void buttonStartImport_Click(object sender, EventArgs e)
        {
            if (this.readyForImport() && this.CheckRows() && this.CheckDestinationURL())
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                int i = 0;
                this.initLogfile();
                System.IO.StreamWriter sw = System.IO.File.AppendText(this.textBoxLogfile.Text);
                try
                {
                    foreach (System.Windows.Forms.DataGridViewRow r in this.dataGridViewImport.Rows)
                    {
                        string Error = "";
                        string URL = "";
                        if (System.Boolean.Parse(r.Cells[0].Value.ToString()) && (r.Cells["ColumnError"].Value.ToString().Length == 0 || r.Cells["ColumnError"].Value.ToString() == "OK"))
                        {
                            try
                            {
                                URL = r.Cells["ColumnPathInDB"].Value.ToString();//this.ImageUri(r);
                                System.Uri U = new Uri(URL);
                                Error += this.writeToDatabase(r.Cells[1].Value.ToString(), URL, System.Boolean.Parse(r.Cells["ColumnAppend"].Value.ToString()));
                                if (Error.Length > 0)
                                    r.Cells["ColumnError"].Value = Error;
                                else
                                {
                                    r.Cells["ColumnError"].Value = "OK";
                                    i++;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.Message);
                            }
                        }
                        string AccNr = "Accession number:\t\t" + r.Cells[1].Value;
                        if (r.Cells[5].Value.ToString() == "True") 
                            AccNr += "\talready present, image added";
                        sw.WriteLine(AccNr);
                        sw.WriteLine("\tURI:             \t" + URL);
                        if (Error.Length > 0) sw.WriteLine("\tError:       \t" + Error);
                        sw.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
                finally
                {
                    sw.Close();
                    this.Cursor = System.Windows.Forms.Cursors.Default;
                }
                System.Windows.Forms.MessageBox.Show(i.ToString() + " datasets imported");
            }
            else
                System.Windows.Forms.MessageBox.Show("Import failed");
        }

        private bool CheckDestinationURL()
        {
            bool OK = false;
            if (this.checkBoxImageAsUri.Checked)
            {
                string URL = this.textBoxWebFolder.Text;
                if (URL.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a valid Base URL");
                    return false;
                }
                try
                {
                    System.Uri U = new Uri(URL);
                    if (!U.IsWellFormedOriginalString() || !U.IsAbsoluteUri)
                    {
                        System.Windows.Forms.MessageBox.Show(this.textBoxWebFolder.Text + " is not valid URL");
                        return false;
                    }
                    else
                        return true;
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(this.textBoxWebFolder.Text + " is not valid URL");
                    return false;
                }
            }
            else
                return true;
            return OK;
        }

        private bool checkDataGridRow(System.Windows.Forms.DataGridViewRow r)
        {
            bool OK = true;
            string Error = this.AccessionNumberIsOK(r.Cells[1].Value.ToString());
            r.Cells[6].Value = Error;
            if (Error.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show("Error for Accession Nr. " + r.Cells[1].Value.ToString() + "\r\n" + Error);
                r.Cells[0].Value = false;
                return false;
            }
            if (!this.checkBoxAppendImages.Checked)
            {
                if (this.AccessionNumberIsPresent(r.Cells[1].Value.ToString()))
                {
                    System.Windows.Forms.MessageBox.Show("Accession Nr. " + r.Cells[1].Value.ToString() + "\r\nallready in database");
                    r.Cells["ColumnError"].Value = "Acc.Nr. allready in database";
                    r.Cells[0].Value = false;
                    return false;
                }
            }
            else
            {
                r.Cells[0].Value = true;
                if (this.AccessionNumberIsPresent(r.Cells[1].Value.ToString()))
                    r.Cells[7].Value = true;
                else
                    r.Cells[7].Value = false;
            }
            return OK;
        }
 
	    #endregion        

        #region Import Images
        private void buttonBrowseUrlFolder_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxWebFolder.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                try
                {
                    System.Uri Uri = new Uri(f.URL);
                    if (Uri.IsFile || !f.URL.EndsWith("/"))
                    {
                        System.Windows.Forms.MessageBox.Show(f.URL + "\r\nis a file. Please select a folder");
                        return;
                    }
                    this.textBoxWebFolder.Text = f.URL;
                }
                catch { }
            }
        }

        //private string saveImage(System.Drawing.Bitmap Original, float ScaleFactor, System.IO.FileInfo File)
        //{
        //    string Message = "";
        //    try
        //    {
        //        if (System.IO.File.Exists(File.FullName) && !this.checkBoxOverwriteImages.Checked)
        //        {
        //            Message += "The image file " + File.FullName + " allready exists";
        //            System.Windows.Forms.MessageBox.Show(Message);
        //            return Message;
        //        }
        //        int Width = (int)((float)Original.Width * ScaleFactor);
        //        int Heigth = (int)((float)Original.Height * ScaleFactor);
        //        System.Drawing.Bitmap Copy = new Bitmap(Original, Width, Heigth);
        //        string Extension = File.Extension;
        //        switch (Extension)
        //        {
        //            case ".tif":
        //                Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Tiff);
        //                break;
        //            case ".png":
        //                Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Png);
        //                break;
        //            case ".gif":
        //                Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Gif);
        //                break;
        //            case ".bmp":
        //                Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Bmp);
        //                break;
        //            default:
        //                Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                break;
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Message += ex.Message;
        //    } 
        //    return Message;
        //}
        
        #endregion

        #region Database

        private string checkIfImageIsInDB(string AccessionNumber, string URI)
        {
            string Message = "";
            if (this.checkBoxOverwriteImages.Checked) return Message;
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            try
            {
                string SQL = "SELECT COUNT(*) AS I " +
                    "FROM CollectionSpecimen " +
                    "INNER JOIN CollectionSpecimenImage " +
                    "ON CollectionSpecimen.CollectionSpecimenID = CollectionSpecimenImage.CollectionSpecimenID " +
                    "WHERE (CollectionSpecimen.AccessionNumber = N'" + AccessionNumber + "') " +
                    "AND (CollectionSpecimenImage.URI = N'" + URI + "') ";
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                if (con.State.ToString() == "Closed")
                    con.Open();
                int iCount = System.Int32.Parse(cmd.ExecuteScalar().ToString());
                if (iCount > 0) Message = "Image allready in database";
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }
            return Message;
        }

        private string writeToDatabase(string AccessionNumber, string URI, bool AppendImage)
        {
            string Message = "";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            try
            {
                con.Open();
                if (AppendImage)
                {
                    string SQL = "SELECT CollectionSpecimenID FROM CollectionSpecimen WHERE AccessionNumber = '" + AccessionNumber + "'";
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    if (con.State.ToString() == "Closed")
                        con.Open();
                    int CollectionSpecimenID = System.Int32.Parse(cmd.ExecuteScalar().ToString());

                    // CollectionSpecimenImage
                    cmd.CommandText = "INSERT INTO CollectionSpecimenImage (CollectionSpecimenID, URI, ImageType)" +
                        " VALUES (" + CollectionSpecimenID.ToString() + ", '" + URI + "', '" + this.comboBoxImageType.SelectedValue.ToString() + "')";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    string FieldList = "";
                    string ValueList = "";
                    System.Data.DataRowView rv = (System.Data.DataRowView)this.comboBoxProject.SelectedItem;
                    int ProjectID = System.Int32.Parse(rv.Row["ProjectID", System.Data.DataRowVersion.Current].ToString());
                    string Project = rv.Row["Project", System.Data.DataRowVersion.Current].ToString();
                    int CollectionID = System.Int32.Parse(this.comboBoxCollection.SelectedValue.ToString());
                    bool HasEvent = false;
                    int EventID = 0;
                    string SQL = "";
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);

                    // Event
                    if (this.userControlModuleRelatedEntryGazetteer.textBoxValue.Text.Length > 0
                        || this.userControlDatePanelCollectionDate.maskedTextBoxDay.Text.Length > 0
                        || this.userControlDatePanelCollectionDate.maskedTextBoxMonth.Text.Length > 0
                        || this.userControlDatePanelCollectionDate.maskedTextBoxYear.Text.Length > 0
                        || this.userControlDatePanelCollectionDate.textBoxSupplement.Text.Length > 0)
                    {
                        FieldList = "";
                        ValueList = "";
                        if (this.userControlDatePanelCollectionDate.maskedTextBoxDay.Text.Length > 0)
                        {
                            FieldList += ", CollectionDay";
                            ValueList += ", " + this.userControlDatePanelCollectionDate.maskedTextBoxDay.Text.ToString();
                        }
                        if (this.userControlDatePanelCollectionDate.maskedTextBoxMonth.Text.Length > 0)
                        {
                            FieldList += ", CollectionMonth";
                            ValueList += ", " + this.userControlDatePanelCollectionDate.maskedTextBoxMonth.Text.ToString();
                        }
                        if (this.userControlDatePanelCollectionDate.maskedTextBoxYear.Text.Length > 0)
                        {
                            FieldList += ", CollectionYear";
                            ValueList += ", " + this.userControlDatePanelCollectionDate.maskedTextBoxYear.Text.ToString();
                        }
                        if (this.userControlDatePanelCollectionDate.textBoxSupplement.Text.Length > 0)
                        {
                            FieldList += ", CollectionDateSupplement";
                            ValueList += ", '" + this.userControlDatePanelCollectionDate.textBoxSupplement.Text.ToString() + "'";
                        }
                        if (this.userControlModuleRelatedEntryGazetteer.textBoxValue.Text.ToString().Length > 0)
                        {
                            FieldList += ", LocalityDescription";
                            ValueList += ", '" + this.userControlModuleRelatedEntryGazetteer.textBoxValue.Text + "'";
                            if (this.userControlModuleRelatedEntryGazetteer.RemoteValues != null)
                            {
                                DiversityWorkbench.GazetteerValues G = (DiversityWorkbench.GazetteerValues)this.userControlModuleRelatedEntryGazetteer.RemoteValues;
                                if (G.Country != null)
                                {
                                    FieldList += ", CountryCache";
                                    ValueList += ", '" + G.Country + "'";
                                }
                            }
                        }
                        if (FieldList.StartsWith(",")) FieldList = FieldList.Substring(1).Trim();
                        if (ValueList.StartsWith(",")) ValueList = ValueList.Substring(1).Trim();
                        cmd.CommandText = "INSERT INTO CollectionEvent (" + FieldList + " ) VALUES (" + ValueList + ")";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT SCOPE_IDENTITY()";
                        EventID = System.Int32.Parse(cmd.ExecuteScalar().ToString());

                        // EventLocalisation
                        if (this.userControlModuleRelatedEntryGazetteer.textBoxValue.Text.Length > 0)
                        {
                            cmd.CommandText = "INSERT INTO CollectionEventLocalisation (CollectionEventID, LocalisationSystemID, Location1, Location2, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache)" +
                                " VALUES (" + EventID.ToString() + ", 7, '" + this.userControlModuleRelatedEntryGazetteer.textBoxValue.Text + "', '" + this.userControlModuleRelatedEntryGazetteer.labelURI.Text + "', ";
                            if (this.userControlModuleRelatedEntryGazetteer.RemoteValues != null)
                            {
                                DiversityWorkbench.GazetteerValues G = (DiversityWorkbench.GazetteerValues)this.userControlModuleRelatedEntryGazetteer.RemoteValues;
                                if (G.Altitude != null)
                                {
                                    cmd.CommandText += G.Altitude.ToString() + ", ";
                                }
                                else
                                    cmd.CommandText += "NULL, ";
                                if (G.Latitude != null)
                                {
                                    cmd.CommandText += G.Latitude.ToString() + ", ";
                                }
                                else
                                    cmd.CommandText += "NULL, ";
                                if (G.Longitude != null)
                                {
                                    cmd.CommandText += G.Longitude.ToString();
                                }
                                else
                                    cmd.CommandText += "NULL";
                            }
                            else
                                cmd.CommandText += "NULL, NULL, NULL";
                            cmd.CommandText += ")";
                            cmd.ExecuteNonQuery();
                        }
                        HasEvent = true;
                    }

                    // CollectionSpecimen
                    FieldList = "AccessionNumber, LabelTranscriptionState";
                    ValueList = "'" + AccessionNumber + "', 'not started'";
                    if (HasEvent)
                    {
                        FieldList += ", CollectionEventID";
                        ValueList += ", " + EventID.ToString();
                    }
                    // AccessionDate
                    if (this.userControlDatePanelAccessionDate.maskedTextBoxDay.Text.Length > 0)
                    {
                        FieldList += ", AccessionDay";
                        ValueList += ", " + this.userControlDatePanelAccessionDate.maskedTextBoxDay.Text.ToString();
                    }
                    if (this.userControlDatePanelAccessionDate.maskedTextBoxMonth.Text.Length > 0)
                    {
                        FieldList += ", AccessionMonth";
                        ValueList += ", " + this.userControlDatePanelAccessionDate.maskedTextBoxMonth.Text.ToString();
                    }
                    if (this.userControlDatePanelAccessionDate.maskedTextBoxYear.Text.Length > 0)
                    {
                        FieldList += ", AccessionYear";
                        ValueList += ", " + this.userControlDatePanelAccessionDate.maskedTextBoxYear.Text.ToString();
                    }
                    if (this.userControlDatePanelAccessionDate.textBoxSupplement.Text.Length > 0)
                    {
                        FieldList += ", AccessionDateSupplement";
                        ValueList += ", '" + this.userControlDatePanelAccessionDate.textBoxSupplement.Text.ToString() + "'";
                    }
                    // Label
                    if (this.comboBoxLabelType.SelectedValue != null && this.comboBoxLabelType.SelectedValue.ToString().Length > 0)
                    {
                        FieldList += ", LabelType";
                        ValueList += ", '" + this.comboBoxLabelType.SelectedValue.ToString() + "'";
                    }
                    // Exsiccate
                    if (this.userControlModuleRelatedEntryExsiccate.textBoxValue.Text.Length > 0)
                    {
                        FieldList += ", ExsiccataAbbreviation";
                        ValueList += ", '" + this.userControlModuleRelatedEntryExsiccate.textBoxValue.Text + "'";
                    }
                    if (this.userControlModuleRelatedEntryExsiccate.labelURI.Text.Length > 0)
                    {
                        FieldList += ", ExsiccataURI";
                        ValueList += ", '" + this.userControlModuleRelatedEntryExsiccate.labelURI.Text + "'";
                    }
                    // Depositor
                    if (this.userControlModuleRelatedEntryDepositor.textBoxValue.Text.Length > 0)
                    {
                        FieldList += ", DepositorsName";
                        ValueList += ", '" + this.userControlModuleRelatedEntryDepositor.textBoxValue.Text + "'";
                    }
                    if (this.userControlModuleRelatedEntryDepositor.labelURI.Text.Length > 0)
                    {
                        FieldList += ", DepositorsAgentURI";
                        ValueList += ", '" + this.userControlModuleRelatedEntryDepositor.labelURI.Text + "'";
                    }
                    SQL = "INSERT INTO CollectionSpecimen (" + FieldList + " ) VALUES (" + ValueList + "); SELECT SCOPE_IDENTITY()";
                    cmd.CommandText = SQL;
                    int CollectionSpecimenID = System.Int32.Parse(cmd.ExecuteScalar().ToString());

                    // CollectionSpecimenImage
                    cmd.CommandText = "INSERT INTO CollectionSpecimenImage (CollectionSpecimenID, URI, ImageType)" +
                        " VALUES (" + CollectionSpecimenID.ToString() + ", '" + URI + "', '" + this.comboBoxImageType.SelectedValue.ToString() + "')";
                    cmd.ExecuteNonQuery();

                    // CollectionProject
                    cmd.CommandText = "INSERT INTO CollectionProject (CollectionSpecimenID, ProjectID)" +
                        " VALUES (" + CollectionSpecimenID.ToString() + ", " + ProjectID.ToString() + ")";
                    cmd.ExecuteNonQuery();

                    // IdentificationUnit
                    int UnitID = 0;
                    SQL = "INSERT INTO IdentificationUnit (CollectionSpecimenID, LastIdentificationCache, TaxonomicGroup, DisplayOrder) ";
                    SQL += "VALUES (" + CollectionSpecimenID.ToString() + ", '";
                    if (this.userControlModuleRelatedEntryIdentification.textBoxValue.Text.Length > 0)
                        SQL += this.userControlModuleRelatedEntryIdentification.textBoxValue.Text;
                    else
                        SQL += this.comboBoxTaxonomicGroup.SelectedValue.ToString();
                    SQL += "', '" + this.comboBoxTaxonomicGroup.SelectedValue.ToString() + "', 1) ";
                    SQL += "(SELECT SCOPE_IDENTITY() AS [SCOPE_IDENTITY])";
                    cmd.CommandText = SQL;
                    UnitID = System.Int32.Parse(cmd.ExecuteScalar().ToString());

                    // Identification
                    if (this.userControlModuleRelatedEntryIdentification.textBoxValue.Text.Length > 0)
                    {
                        cmd.CommandText = "INSERT INTO Identification (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, TaxonomicName, NameURI, ResponsibleName)" +
                            " VALUES (" + CollectionSpecimenID.ToString() + ", " + UnitID + ", 1, '" + this.userControlModuleRelatedEntryIdentification.textBoxValue.Text + "', ";
                        if (this.userControlModuleRelatedEntryIdentification.labelURI.Text.Length > 0)
                            cmd.CommandText += " '" + this.userControlModuleRelatedEntryIdentification.labelURI.Text + "' ";
                        else
                            cmd.CommandText += "NULL";
                        cmd.CommandText += ", ";
                        if (this.userControlModuleRelatedEntryIdentifiedBy.textBoxValue.Text.Length > 0)
                            cmd.CommandText += " '" + this.userControlModuleRelatedEntryIdentifiedBy.textBoxValue.Text + "' ";
                        else
                            cmd.CommandText += "NULL";
                        cmd.CommandText += ")";
                        cmd.ExecuteNonQuery();
                    }

                    // Collector
                    if (this.userControlModuleRelatedEntryCollector.textBoxValue.Text.Length > 0)
                    {
                        cmd.CommandText = "INSERT INTO CollectionAgent (CollectionSpecimenID, CollectorsName, CollectorsAgentURI)" +
                            " VALUES (" + CollectionSpecimenID.ToString() + ", '" + this.userControlModuleRelatedEntryCollector.textBoxValue.Text + "', ";
                        if (this.userControlModuleRelatedEntryCollector.labelURI.Text.Length > 0)
                            cmd.CommandText += "'" + this.userControlModuleRelatedEntryCollector.labelURI.Text + "'";
                        else
                            cmd.CommandText += "NULL";
                        cmd.CommandText += ")";
                        cmd.ExecuteNonQuery();
                    }

                    // CollectionSpecimenPart
                    cmd.CommandText = "INSERT INTO CollectionSpecimenPart ( CollectionSpecimenID, SpecimenPartID, CollectionID, MaterialCategory, StorageLocation)" +
                        " VALUES (" + CollectionSpecimenID.ToString() + ", 1, " + CollectionID.ToString() + ", '"
                        + this.comboBoxMaterialCategory.SelectedValue.ToString() + "', '";
                    if (this.userControlModuleRelatedEntryIdentification.textBoxValue.Text.Length > 0)
                        cmd.CommandText += this.userControlModuleRelatedEntryIdentification.textBoxValue.Text + "')";
                    else cmd.CommandText += this.comboBoxTaxonomicGroup.SelectedValue.ToString() + "')";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }
            finally { con.Close(); }
            return Message;
        }

        #endregion

        #region Logfile

        private bool initLogfile()
        {
            bool OK = true;
            System.IO.StreamWriter sw = new System.IO.StreamWriter(this.textBoxLogfile.Text);
            try
            {
                sw.WriteLine("Logfile for import of images in " + DiversityWorkbench.Settings.DatabaseName + ".\r\n");
                sw.WriteLine("Images imported by:\t\t" + System.Environment.UserName);
                sw.WriteLine();
                sw.Write("Date:\t\t\t\t");
                sw.WriteLine(DateTime.Now);
                sw.WriteLine();
                sw.WriteLine("Import options:\r\n");
                sw.WriteLine("Folder of original images:\t" + this.textBoxFolderOriginal.Text);
                if (this.checkBoxAppendImages.Checked)
                    sw.WriteLine("Append images if accession number is present");
                if (this.checkBoxOverwriteImages.Checked)
                    sw.WriteLine("Overwrite existing images");
                if (this.checkBoxBarcodeStart.Checked)
                    sw.WriteLine("Check start of file name.\tMust be " + this.textBoxBarcodeStart.Text);
                if (this.checkBoxBarcodeLength.Checked)
                    sw.WriteLine("Check length of file name.\tMust be " + this.textBoxBarcodeLength.Text);
                if (this.checkBoxImageAsUri.Checked)
                    sw.WriteLine("Import images as URL in:\t" + this.textBoxWebFolder.Text);
                if (this.checkBoxSubfolder.Checked)
                    sw.WriteLine("Import in subdirectries:\tlength. " + this.numericUpDownSubfolder.Value.ToString() 
                        + " e.g.: " + this.dataGridViewImport.Rows[0].Cells["ColumnPathInDB"].Value.ToString());
                sw.WriteLine("Collection:\t\t\t" + this.comboBoxCollection.Text);
                sw.WriteLine("Taxonomic group:\t\t" + this.comboBoxTaxonomicGroup.Text);
                sw.WriteLine("Image type:\t\t\t" + this.comboBoxImageType.Text);
                sw.WriteLine("Project:\t\t\t" + this.comboBoxProject.Text);
                sw.WriteLine("Material category:\t\t" + this.comboBoxMaterialCategory.Text);
                sw.Write("Images imported in Database:\t");
                sw.WriteLine();
            }
            catch
            {
                OK = false;
            }
            finally
            {
                sw.Close();
            }
            return OK;
        }

        private void textBoxFolderArchive_TextChanged(object sender, System.EventArgs e)
        {
            this.setLogfilePath();
        }

        private void setLogfilePath()
        {
            this.textBoxLogfile.Text = Folder.Import() + @"LogImport_" + System.Environment.UserName + "_"
                + System.DateTime.Now.Year.ToString().PadLeft(4, '0')
                + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                + System.DateTime.Now.Day.ToString().PadLeft(2, '0')
                + "_" + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                + System.DateTime.Now.Minute.ToString().PadLeft(2, '0')
                + System.DateTime.Now.Second.ToString().PadLeft(2, '0') + ".log";
        }

        #endregion

        #region Mandatory indications

        private void setMandatoryIndications()
        {
            this.checkBoxBarcodeLength_CheckedChanged(null, null);
            this.checkBoxBarcodeStart_CheckedChanged(null, null);
            this.checkBoxCutAfterSeparator_CheckedChanged(null, null);
            this.checkBoxImageAsUri_CheckedChanged(null, null);
        }

        private void checkBoxCutAfterSeparator_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxCutAfterSeparator.Checked)
                this.textBoxAccessionSeparator.BackColor = System.Drawing.Color.Pink;
            else
                this.textBoxAccessionSeparator.BackColor = System.Drawing.SystemColors.Window;
        }

        private void checkBoxBarcodeStart_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxBarcodeStart.Checked)
                this.textBoxBarcodeStart.BackColor = System.Drawing.Color.Pink;
            else
                this.textBoxBarcodeStart.BackColor = System.Drawing.SystemColors.Window;
        }

        private void checkBoxBarcodeLength_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxBarcodeLength.Checked)
                this.textBoxBarcodeLength.BackColor = System.Drawing.Color.Pink;
            else
                this.textBoxBarcodeLength.BackColor = System.Drawing.SystemColors.Window;
        }

        //private void radioButtonWidth_CheckedChanged(object sender, EventArgs e)
        //{
        //    //this.setMandatoryIndicationsForScaling();
        //}

        private void checkBoxImageAsUri_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxImageAsUri.Checked)
                this.textBoxWebFolder.BackColor = System.Drawing.Color.Pink;
            else
                this.textBoxWebFolder.BackColor = System.Drawing.SystemColors.Window;
        }

       #endregion

    }
}