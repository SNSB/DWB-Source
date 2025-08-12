using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormImportImages : Form
    {
		#region Parameter
        //private DiversityWorkbench.ApplicationSettings applicationSettings;
        private Microsoft.Data.SqlClient.SqlConnection _SqlConnection;
		#endregion

        #region Construction
        public FormImportImages()
        {
            InitializeComponent();
            this.setEnumSource();
            this.setLogfilePath();
            //this._dsCollectionSpecimen = new DataSetCollectionSpecimen();
            this.initForm();
            this.initRemoteModules();
            //this.setUserControlDatabindings();
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }
        #endregion

        #region Form
        private void initForm()
        {
            if (DiversityCollection.Forms.FormImportImagesSettings.Default.FolderArchive.Length > 0)
                this.textBoxFolderArchive.Text = DiversityCollection.Forms.FormImportImagesSettings.Default.FolderArchive;
            if (DiversityCollection.Forms.FormImportImagesSettings.Default.FolderOriginal.Length > 0)
                this.textBoxFolderOriginal.Text = DiversityCollection.Forms.FormImportImagesSettings.Default.FolderOriginal;
            if (DiversityCollection.Forms.FormImportImagesSettings.Default.WebFolder.Length > 0)
                this.textBoxWebFolder.Text = DiversityCollection.Forms.FormImportImagesSettings.Default.WebFolder;
            this.checkBoxAppendDateToFilename.Checked = DiversityCollection.Forms.FormImportImagesSettings.Default.AppendDateToFilename;
            this.setMandatoryIndications();
        }

        private void setEnumSource()
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxMaterialCategory, "CollMaterialCategory_Enum", con);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTaxonomicGroup, "CollTaxonomicGroup_Enum", con);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxImageType, "CollSpecimenImageType_Enum", con);
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
                //if (this.radioButtonPreviewImage.Checked) Folder = this.textBoxFolderPreview.Text;
                //if (this.radioButtonWebImage.Checked) Folder = this.textBoxFolderWeb.Text;
                if (this.radioButtonOriginalImage.Checked) Folder = this.textBoxFolderOriginal.Text;
                if (Folder.IndexOf(":") > 0) Folder = Folder.Substring(Folder.IndexOf(":") + 1, Folder.Length - Folder.IndexOf(":") - 1);
                Folder = Folder.Replace(@"\", "/");
                this.textBoxWebFolder.Text = Folder;
            }
            catch { }
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

                // Exsiccatae
                DiversityWorkbench.Exsiccate E = new DiversityWorkbench.Exsiccate(DiversityWorkbench.Settings.ServerConnection);
                //if (E.DatabaseIsAvailable())
                this.userControlModuleRelatedEntryExsiccate.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)E;
                this.userControlModuleRelatedEntryExsiccate.setTableSource("CollectionSpecimen", "ExsiccataAbbreviation", "ExsiccataURI", "");

                // Gazetteer
                DiversityWorkbench.Gazetteer G = new DiversityWorkbench.Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                //if (G.DatabaseIsAvailable())
                this.userControlModuleRelatedEntryGazetteer.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)G;
                string[] ValueColumns = new string[4] {"Altitude", "Longitude", "Latitude", "Country" };
                this.userControlModuleRelatedEntryGazetteer.setTableSource("CollectionGeography", "Location1", "Location2", "LocalisationSystemID = 7", ValueColumns);

                // TaxonNames
                DiversityWorkbench.TaxonName T = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                //if (T.DatabaseIsAvailable())
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
        private void buttonFolderArchive_Click(object sender, EventArgs e)
        {
            this.SetFolder(this.textBoxFolderArchive);
        }

        private void buttonFolderWeb_Click(object sender, EventArgs e)
        {
            //this.SetFolder(this.textBoxFolderWeb);
        }

        private void buttonFolderPreview_Click(object sender, EventArgs e)
        {
            //this.SetFolder(this.textBoxFolderPreview);
        }

        private void SetFolder(System.Windows.Forms.TextBox textBox)
        {
            this.folderBrowserDialogImport = new FolderBrowserDialog();
            this.folderBrowserDialogImport.RootFolder = System.Environment.SpecialFolder.MyComputer;
            //if (textBox.Text.Length > 0) this.folderBrowserDialogImport.RootFolder = Environment.SpecialFolder.Recent
            this.folderBrowserDialogImport.ShowDialog();
            if (this.folderBrowserDialogImport.SelectedPath.Length > 0)
                textBox.Text = this.folderBrowserDialogImport.SelectedPath;
        }

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
            Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand("SELECT COUNT(*) FROM CollectionSpecimen WHERE AccessionNumber = '" + AccessionNumber + "'", this.SqlConnection);
            if (this.SqlConnection.State.ToString() == "Closed")
                this.SqlConnection.Open();
            string i = cmd.ExecuteScalar().ToString();
            if (i == "0") IsPresent = false;
            return IsPresent;
        }

        private string attachDateTimeToFileName(System.IO.FileInfo f)
        {
            string NewName = f.Name;
            if (this.checkBoxAppendDateToFilename.Checked)
            {
                char Pad = '0';
                string Year = f.LastWriteTime.Year.ToString();
                string Month = f.LastWriteTime.Month.ToString();
                Month = Month.PadLeft(2, Pad);
                string Day = f.LastWriteTime.Day.ToString();
                Day = Day.PadLeft(2, Pad);
                string Hour = f.LastWriteTime.Hour.ToString();
                Hour = Hour.PadLeft(2, Pad);
                string Min = f.LastWriteTime.Minute.ToString();
                Min = Min.PadLeft(2, Pad);
                string Sec = f.LastWriteTime.Second.ToString();
                Sec = Sec.PadLeft(2, Pad);
                NewName = f.Name.Substring(0, f.Name.Length - f.Extension.Length);
                NewName += "_" + Year + Month + Day + "_" + Hour + Min + Sec;
                NewName += f.Extension;
            }
            return NewName;
        }


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
                this.dataGridViewImport.Rows.Clear();
                this.buttonStartImport.Enabled = false;
                this.textBoxFolderOriginal.Text = "";
                foreach (string f in this.openFileDialogImport.FileNames)
                {
                    System.IO.FileInfo fi = new System.IO.FileInfo(f);
                    if (fi.Extension == ".jpg" ||
                        fi.Extension == ".png" ||
                        fi.Extension == ".bmp" ||
                        fi.Extension == ".tif" ||
                        fi.Extension == ".gif")
                    {
                        if (this.textBoxFolderOriginal.Text.Length == 0)
                        {
                            this.textBoxFolderOriginal.Text = fi.DirectoryName;
                            if (this.textBoxFolderArchive.Text.Length == 0)
                            {
                                this.textBoxFolderArchive.Text = fi.DirectoryName;
                            }
                       }
                        System.Object[] o = new object[8];

                        try
                        {
                            string AccNr = this.extractAccessionNumber(fi);
                            o[1] = AccNr;
                            o[2] = fi.Name;
                            if (this.checkBoxAppendDateToFilename.Checked)
                                o[3] = this.attachDateTimeToFileName(fi);
                            else o[3] = fi.Name;
                            System.IO.FileInfo fn = new System.IO.FileInfo(fi.DirectoryName + @"\" + o[3]);
                            string File = fn.Name.Substring(0, fn.Name.Length - fn.Extension.Length);
                            //if (this.checkBoxWebImages.Checked) o[4] = File + this.comboBoxImageFormatWeb.Text;
                            //if (this.checkBoxPreviewImages.Checked) o[5] = File + this.comboBoxImageFormatPreview.Text;
                            string Error = this.AccessionNumberIsOK(AccNr);
                            if (Error.Length == 0) o[0] = true;
                            else o[0] = false;
                            o[6] = Error;
                            o[7] = false;
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
                if (this.checkBoxArchiveImages.Checked)
                {
                    if (this.checkBoxAppendDateToFilename.Checked)
                    {
                        System.IO.FileInfo fa = new System.IO.FileInfo(AccNr + fi.Extension);
                        r.Cells[3].Value = this.attachDateTimeToFileName(fa);
                    }
                    else r.Cells[3].Value = AccNr + fi.Extension;
                }
                else
                    r.Cells[3].Value = "";
                System.IO.FileInfo fn;
                if (this.checkBoxArchiveImages.Checked)
                fn = new System.IO.FileInfo(r.Cells[3].Value.ToString());
                else
                fn = new System.IO.FileInfo(r.Cells[2].Value.ToString());
                string File = fn.Name.Substring(0, fn.Name.Length - fn.Extension.Length);
                //if (this.checkBoxWebImages.Checked) r.Cells[4].Value = File + this.comboBoxImageFormatWeb.Text;
                //else r.Cells[4].Value = "";
                //if (this.checkBoxPreviewImages.Checked) r.Cells[5].Value = File + this.comboBoxImageFormatPreview.Text;
                //else r.Cells[5].Value = "";
                string Error = this.AccessionNumberIsOK(AccNr);
                if (Error.Length == 0) r.Cells[0].Value = true;
                else r.Cells[0].Value = false;
                r.Cells[6].Value = Error;
                r.Cells[7].Value = false;
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

        private void dataGridViewImport_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //System.Windows.Forms.DataGridView v = (System.Windows.Forms.DataGridView)sender;
            //System.Windows.Forms.DataGridViewCell c = v.SelectedCells[0];
            //if (c.ColumnIndex == 8)
            //{
            //    this.fillDataGridRow(this.dataGridViewImport.Rows[c.RowIndex]);
            //    this.checkDataGridRow(this.dataGridViewImport.Rows[c.RowIndex]);
            //}
        }

        #endregion

        #region Test and prepare
        private void buttonTestImport_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (this.CheckDestinationURL() && this.CheckImageSource() && this.readyForImport())
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
                    if (this.checkBoxSubdirectory.Checked)
                    {
                        if (r.Cells[1].Value != null && r.Cells[1].Value.ToString().Length < this.numericUpDownSubdirectry.Value)
                        {
                            Error += "Accession number shorter than path for subdirectory. ";
                        }
                    }
                    URL = this.ImageUri(r);
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
                        r.Cells[6].Value = Error;
                        r.Cells[0].Value = false;
                    }
                    else
                    {
                        r.Cells[6].Value = "OK";
                        r.Cells[0].Value = true;
                        AnyValidLine = true;
                    }
                }
                catch (System.Exception ex)
                {
                    OK = false;
                    r.Cells[6].Value = Error + ex.Message;
                    r.Cells[0].Value = false;
                }
            }
            if (AnyValidLine) this.buttonStartImport.Enabled = true;
            else this.buttonStartImport.Enabled = false;
            return OK;
        }

        private void setSubfolderExample()
        {
            if (this.checkBoxSubdirectory.Checked && this.dataGridViewImport.Rows.Count > 0)
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
                if (AccNr.Length > this.numericUpDownSubdirectry.Value)
                {
                    string SubDir = "...\\" + AccNr.Substring(0, (int)this.numericUpDownSubdirectry.Value);
                    this.textBoxSubdirectory.Text = SubDir;
                }
                else
                    System.Windows.Forms.MessageBox.Show("The length of the accession number " + AccNr + " is lower than the length you specified for your subdirectories");
            }
        }

        private bool CheckRowsForConversion()
        {
            bool OK = true;
            foreach (System.Windows.Forms.DataGridViewRow r in this.dataGridViewImport.Rows)
            {
                string Error = "";
                try
                {
                    if (!this.checkBoxOverwriteImages.Checked)
                    {
                        //if (this.checkBoxWebImages.Checked)
                        //{
                        //    string Dir = this.textBoxFolderWeb.Text;
                        //    if (!Dir.EndsWith(@"\")) Dir += @"\";
                        //    System.IO.FileInfo f = new System.IO.FileInfo(Dir + r.Cells[4].Value.ToString());
                        //    if (f.Exists)
                        //        Error += "File for web image exists. ";
                        //}
                        //if (this.checkBoxWebImages.Checked)
                        //{
                        //    string Dir = this.textBoxFolderWeb.Text;
                        //    if (!Dir.EndsWith(@"\")) Dir += @"\";
                        //    System.IO.FileInfo f = new System.IO.FileInfo(Dir + r.Cells[5].Value.ToString());
                        //    if (f.Exists)
                        //        Error += "File for preview image exists. ";
                        //}
                    }
                    if (Error.Length > 0)
                    {
                        r.Cells[6].Value = Error;
                        r.Cells[0].Value = false;
                    }
                    else
                    {
                        r.Cells[6].Value = "OK";
                        r.Cells[0].Value = true;
                    }
                }
                catch (System.Exception ex)
                {
                    OK = false;
                    r.Cells[6].Value = Error + ex.Message;
                    r.Cells[0].Value = false;
                }
            }
            return OK;
        }

        private bool readyForImageConversion()
        {
            bool OK = true;
            if (!System.IO.Directory.Exists(this.textBoxFolderArchive.Text))
            {
                System.Windows.Forms.MessageBox.Show("The archive folder\r\n" + this.textBoxFolderArchive.Text + "\r\ndoes not exist");
                return false;
            }
            //if (this.checkBoxPreviewImages.Checked)
            //{
            //    if (!System.IO.Directory.Exists(this.textBoxFolderPreview.Text))
            //    {
            //        System.Windows.Forms.MessageBox.Show("The preview folder\r\n" + this.textBoxFolderPreview.Text + "\r\ndoes not exist");
            //        return false;
            //    }
            //}
            //if (this.checkBoxWebImages.Checked)
            //{
            //    if (!System.IO.Directory.Exists(this.textBoxFolderWeb.Text))
            //    {
            //        System.Windows.Forms.MessageBox.Show("The web folder\r\n" + this.textBoxFolderWeb.Text + "\r\ndoes not exist");
            //        return false;
            //    }
            //}
            return OK;
        }

        
        private bool readyForImport()
        {
            bool OK = true;
            if (!System.IO.Directory.Exists(this.textBoxFolderArchive.Text))
            {
                System.Windows.Forms.MessageBox.Show("The archive folder\r\n" + this.textBoxFolderArchive.Text + "\r\ndoes not exist");
                return false;
            }
            //if (this.checkBoxPreviewImages.Checked)
            //{
            //    if (!System.IO.Directory.Exists(this.textBoxFolderPreview.Text))
            //    {
            //        System.Windows.Forms.MessageBox.Show("The preview folder\r\n" + this.textBoxFolderPreview.Text + "\r\ndoes not exist");
            //        return false;
            //    }
            //}
            //if (this.checkBoxWebImages.Checked)
            //{
            //    if (!System.IO.Directory.Exists(this.textBoxFolderWeb.Text))
            //    {
            //        System.Windows.Forms.MessageBox.Show("The web folder\r\n" + this.textBoxFolderWeb.Text + "\r\ndoes not exist");
            //        return false;
            //    }
            //}
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
        
        #endregion   
     
        #region Convert images
        private void buttonConvertImages_Click(object sender, EventArgs e)
        {
            // only conversion of images, no import in database
            if (this.readyForImageConversion() && this.CheckRowsForConversion())
            {
                int i = 0;
                foreach (System.Windows.Forms.DataGridViewRow r in this.dataGridViewImport.Rows)
                {
                    string Error = "";
                    if (System.Boolean.Parse(r.Cells[0].Value.ToString()) && (r.Cells[6].Value.ToString().Length == 0 || r.Cells[6].Value.ToString() == "OK"))
                    {
                        try
                        {
                            Error += this.ConvertAndSaveImage(r);
                            if (Error.Length > 0)
                                r.Cells[6].Value = Error;
                            else
                            {
                                r.Cells[6].Value = "OK";
                                i++;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            System.Windows.Forms.MessageBox.Show(ex.Message);
                        }
                    }
                }
                System.Windows.Forms.MessageBox.Show(i.ToString() + " images converted");
            }
            else
                System.Windows.Forms.MessageBox.Show("Convesion failed");
        }
        
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
                        if (System.Boolean.Parse(r.Cells[0].Value.ToString()) && (r.Cells[6].Value.ToString().Length == 0 || r.Cells[6].Value.ToString() == "OK"))
                        {
                            try
                            {
                                Error += this.ConvertAndSaveImage(r);
                                URL = this.ImageUri(r);
                                System.Uri U = new Uri(URL);
                                Error += this.writeToDatabase(r.Cells[1].Value.ToString(), URL, System.Boolean.Parse(r.Cells[7].Value.ToString()));
                                if (Error.Length > 0)
                                    r.Cells[6].Value = Error;
                                else
                                {
                                    r.Cells[6].Value = "OK";
                                    i++;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                System.Windows.Forms.MessageBox.Show(ex.Message);
                            }
                        }
                        sw.WriteLine("Accession number:\t\t" + r.Cells[1].Value);
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

        private bool CheckImageSource()
        {
            if (this.radioButtonOriginalImage.Checked)
                if (System.IO.Directory.Exists(this.textBoxFolderOriginal.Text)) return true;
                else
                {
                    string Message = this.textBoxFolderOriginal.Text + " is not valid path";
                    if (this.textBoxFolderOriginal.Text.Length == 0) Message = "Missing path for directory of originals";
                    System.Windows.Forms.MessageBox.Show(Message);
                    return false;
                }
            else
            {
                if (this.radioButtonArchiveImage.Checked)
                    if (System.IO.Directory.Exists(this.textBoxFolderArchive.Text)) return true;
                    else
                    {
                        string Message = this.textBoxFolderArchive.Text + " is not valid path";
                        if (this.textBoxFolderArchive.Text.Length == 0) Message = "Missing path for archive directory";
                        System.Windows.Forms.MessageBox.Show(Message);
                        return false;
                    }
                //else
                //{
                //    if (this.radioButtonWebImage.Checked)
                //        if (System.IO.Directory.Exists(this.textBoxFolderWeb.Text)) return true;
                //        else
                //        {
                //            string Message = this.textBoxFolderWeb.Text + " is not valid path";
                //            if (this.textBoxFolderWeb.Text.Length == 0) Message = "Missing target folder for web images";
                //            System.Windows.Forms.MessageBox.Show(Message);
                //            return false;
                //        }
                //    else
                //        if (System.IO.Directory.Exists(this.textBoxFolderPreview.Text)) return true;
                //        else
                //        {
                //            string Message = this.textBoxFolderPreview.Text + " is not valid path";
                //            if (this.textBoxFolderPreview.Text.Length == 0) Message = "Missing target folder for preview images";
                //            System.Windows.Forms.MessageBox.Show(Message);
                //            return false;
                //        }
                //}
            }
            return false;
        }

        private string ImageUri(System.Windows.Forms.DataGridViewRow r)
        {
            string URL = "";
            // the folder
            if (this.checkBoxImageAsUri.Checked)
            {
                URL += this.textBoxWebFolder.Text;
            }
            else
            {
                if (this.radioButtonOriginalImage.Checked)
                    URL += this.textBoxFolderOriginal.Text;
                else
                {
                    if (this.radioButtonArchiveImage.Checked)
                        URL += this.textBoxFolderArchive.Text;
                    else
                    {
                        //if (this.radioButtonWebImage.Checked)
                        //    URL += this.textBoxFolderWeb.Text;
                        //else
                        //    URL += this.textBoxFolderPreview.Text;
                    }
                }
            }
            // the subdirectory
            if (this.checkBoxSubdirectory.Checked)
                URL += r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value) + "/";
            // the file
            if (this.radioButtonOriginalImage.Checked && r.Cells[2].Value != null)
                URL += r.Cells[2].Value.ToString();
            else
            {
                if (this.radioButtonArchiveImage.Checked && r.Cells[3].Value != null)
                    URL += r.Cells[3].Value.ToString();
                else
                {
                    if (this.radioButtonWebImage.Checked && r.Cells[4].Value != null)
                        URL += r.Cells[4].Value.ToString();
                    else
                    {
                        if (this.radioButtonPreviewImage.Checked && r.Cells[5].Value != null)
                            URL += r.Cells[5].Value.ToString();
                        else
                            URL = "";
                    }
                }
            }
            return URL;
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
                    r.Cells[6].Value = "Acc.Nr. allready in database";
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
                System.Uri Uri = new Uri(f.URL);
                if (Uri.IsFile || !f.URL.EndsWith("/"))
                {
                    System.Windows.Forms.MessageBox.Show(f.URL + "\r\nis a file. Please select a folder");
                    return;
                }
                this.textBoxWebFolder.Text = f.URL;
            }
        }

        private string ConvertAndSaveImage(System.Windows.Forms.DataGridViewRow r)
        {
            string Error = "";
            //if (this.checkBoxWebImages.Checked || this.checkBoxPreviewImages.Checked || this.checkBoxArchiveImages.Checked)
            //{
            //    string DirO = this.textBoxFolderOriginal.Text;
            //    if (!DirO.EndsWith(@"\")) DirO += @"\";
            //    string DirA = this.textBoxFolderArchive.Text;
            //    if (!DirA.EndsWith(@"\")) DirA += @"\";
            //    string DirW = this.textBoxFolderWeb.Text;
            //    if (!DirW.EndsWith(@"\")) DirW += @"\";
            //    string DirP = this.textBoxFolderPreview.Text;
            //    if (!DirP.EndsWith(@"\")) DirP += @"\";
            //    try
            //    {
            //        System.IO.FileInfo fO = new System.IO.FileInfo(DirO + r.Cells[2].Value.ToString());
            //        if (this.checkBoxSubdirectory.Checked)
            //        {
            //            if (!System.IO.Directory.Exists(DirA + r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value)))
            //            {
            //                System.IO.Directory.CreateDirectory(DirA + r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value));
            //            }
            //            if (this.textBoxFolderOriginal.Text == this.textBoxFolderArchive.Text
            //                && !this.checkBoxSubdirectory.Checked
            //                && !this.checkBoxAppendDateToFilename.Checked)
            //                fO.MoveTo(DirA + r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value) + @"\" + r.Cells[3].Value.ToString());
            //            else
            //                fO.CopyTo(DirA + r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value) + @"\" + r.Cells[3].Value.ToString(), this.checkBoxOverwriteImages.Checked);
            //        }
            //        else
            //        {
            //            if (this.textBoxFolderOriginal.Text == this.textBoxFolderArchive.Text
            //                && !this.checkBoxSubdirectory.Checked
            //                && !this.checkBoxAppendDateToFilename.Checked)
            //                fO.MoveTo(DirA + r.Cells[3].Value.ToString());
            //            else
            //                fO.CopyTo(DirA + r.Cells[3].Value.ToString(), this.checkBoxOverwriteImages.Checked);
            //        }
            //        System.Drawing.Bitmap Ori = new Bitmap(DirO + r.Cells[2].Value.ToString());
            //        float FaktWeb;
            //        float FaktPreview;
            //        //if (this.radioButtonWidth.Checked)
            //        //{
            //        //    FaktWeb = System.Single.Parse(this.comboBoxImageWidthWeb.Text) / (float)Ori.Width;
            //        //    FaktPreview = System.Single.Parse(this.comboBoxImageWidthPreview.Text) / (float)Ori.Width;
            //        //}
            //        //else
            //        //{
            //        //    FaktWeb = System.Single.Parse(this.textBoxScaleWeb.Text) / (float)100;
            //        //    FaktPreview = System.Single.Parse(this.textBoxScalePreview.Text) / (float)100;
            //        //}
            //        //if (this.checkBoxWebImages.Checked)
            //        //{
            //        //    string PathWeb = DirW;
            //        //    if (this.checkBoxSubdirectory.Checked)
            //        //    {
            //        //        if (!System.IO.Directory.Exists(DirW + r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value)))
            //        //            System.IO.Directory.CreateDirectory(DirW + r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value));
            //        //        PathWeb += r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value) + @"\";
            //        //    }
            //        //    PathWeb += r.Cells[4].Value.ToString();
            //        //    System.IO.FileInfo fWeb = new System.IO.FileInfo(PathWeb);
            //        //    this.saveImage(Ori, FaktWeb, fWeb);
            //        //}
            //        //if (this.checkBoxPreviewImages.Checked)
            //        //{
            //        //    string PathPreview = DirP;
            //        //    if (this.checkBoxSubdirectory.Checked)
            //        //    {
            //        //        if (!System.IO.Directory.Exists(DirP + r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value)))
            //        //            System.IO.Directory.CreateDirectory(DirP + r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value));
            //        //        PathPreview += r.Cells[1].Value.ToString().Substring(0, (int)this.numericUpDownSubdirectry.Value) + @"\";
            //        //    }
            //        //    PathPreview += r.Cells[5].Value.ToString();
            //        //    System.IO.FileInfo fPreview = new System.IO.FileInfo(PathPreview);
            //        //    this.saveImage(Ori, FaktPreview, fPreview);
            //        //}
            //    }
            //    catch (System.Exception ex)
            //    {
            //        Error += ex.Message;
            //    }
            //}
            return Error;
        }

        private void ConvertAndImportImages()
        {
            //System.IO.StreamWriter sw = System.IO.File.AppendText(this.textBoxLogfile.Text);
            //foreach (System.Windows.Forms.DataGridViewRow r in this.dataGridViewImport.Rows)
            //{
            //    I.FileInfo.MoveTo(I.OriginalFileNew);
            //    System.Drawing.Bitmap Ori = new Bitmap(I.OriginalFileNew);
            //    float FaktWeb;
            //    float FaktPreview;
            //    if (this.radioButtonWidth.Checked)
            //    {
            //        FaktWeb = System.Single.Parse(this.comboBoxImageWidthWeb.Text) / (float)Ori.Width;
            //        FaktPreview = System.Single.Parse(this.comboBoxImageWidthPreview.Text) / (float)Ori.Width;
            //    }
            //    else
            //    {
            //        FaktWeb = System.Single.Parse(this.textBoxScaleWeb.Text) / (float)100;
            //        FaktPreview = System.Single.Parse(this.textBoxScalePreview.Text) / (float)100;
            //    }
            //    if (this.checkBoxWebImages.Checked)
            //    {
            //        System.IO.FileInfo fWeb = new System.IO.FileInfo(I.WebFile);
            //        this.saveImage(Ori, FaktWeb, fWeb);
            //    }
            //    if (this.checkBoxPreviewImages.Checked)
            //    {
            //        System.IO.FileInfo fPreview = new System.IO.FileInfo(I.PreviewFile);
            //        this.saveImage(Ori, FaktPreview, fPreview);
            //    }
            //    string Error = this.writeToDatabase(I.AccessionNumber, I.URI, I.AccessionNumberPresent);
            //    sw.WriteLine("Accession number:\t" + I.AccessionNumber);
            //    System.IO.FileInfo f = new System.IO.FileInfo(I.OriginalFileNew);
            //    sw.WriteLine("URI:             \t" + I.URI);
            //    if (Error.Length > 0) sw.WriteLine("Error:       \t" + Error);
            //    sw.WriteLine();
            //}
            //sw.Close();
        }

        private string saveImage(System.Drawing.Bitmap Original, float ScaleFactor, System.IO.FileInfo File)
        {
            string Message = "";
            try
            {
                if (System.IO.File.Exists(File.FullName) && !this.checkBoxOverwriteImages.Checked)
                {
                    Message += "The image file " + File.FullName + " allready exists";
                    System.Windows.Forms.MessageBox.Show(Message);
                    return Message;
                }
                int Width = (int)((float)Original.Width * ScaleFactor);
                int Heigth = (int)((float)Original.Height * ScaleFactor);
                System.Drawing.Bitmap Copy = new Bitmap(Original, Width, Heigth);
                string Extension = File.Extension;
                switch (Extension)
                {
                    case ".tif":
                        Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                    case ".png":
                        Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".gif":
                        Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case ".bmp":
                        Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    default:
                        Copy.Save(File.FullName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Message += ex.Message;
            } 
            return Message;
        }
        #endregion

        #region Database
        private string checkIfImageIsInDB(string AccessionNumber, string URI)
        {
            string Message = "";
            try
            {
                string SQL = "SELECT COUNT(*) AS I " +
                    "FROM CollectionSpecimen " +
                    "INNER JOIN CollectionSpecimenImage " +
                    "ON CollectionSpecimen.CollectionSpecimenID = CollectionSpecimenImage.CollectionSpecimenID " +
                    "WHERE (CollectionSpecimen.AccessionNumber = N'" + AccessionNumber + "') " +
                    "AND (CollectionSpecimenImage.URI = N'" + URI + "') ";
                Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                if (this.SqlConnection.State.ToString() == "Closed")
                    this.SqlConnection.Open();
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
            try
            {
                if (AppendImage)
                {
                    string SQL = "SELECT CollectionSpecimenID FROM CollectionSpecimen WHERE AccessionNumber = '" + AccessionNumber + "'";
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                    if (this.SqlConnection.State.ToString() == "Closed")
                        this.SqlConnection.Open();
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
                    // CollectionSpecimen
                    FieldList = "AccessionNumber";
                    ValueList = "'" + AccessionNumber + "'";
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
                    if (this.comboBoxLabelType.SelectedValue != null && this.comboBoxLabelType.SelectedValue.ToString().Length > 0)
                    {
                        FieldList += ", LabelType";
                        ValueList += ", '" + this.comboBoxLabelType.SelectedValue.ToString() + "'";
                    }
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
                    string SQL = "INSERT INTO CollectionSpecimen (" + FieldList + " ) VALUES (" + ValueList + ")";
                    Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.SqlConnection);
                    if (this.SqlConnection.State.ToString() == "Closed")
                        this.SqlConnection.Open();
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "SELECT SCOPE_IDENTITY()";
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
                    cmd.CommandText = "INSERT INTO IdentificationUnit (CollectionSpecimenID, IdentificationUnitID, TaxonomicGroup, DisplayOrder)" +
                        " VALUES (" + CollectionSpecimenID.ToString() + ", 1, '" + this.comboBoxTaxonomicGroup.SelectedValue.ToString() + "', 1)";
                    cmd.ExecuteNonQuery();

                    // Identification
                    if (this.userControlModuleRelatedEntryIdentification.textBoxValue.Text.Length > 0)
                    {
                        cmd.CommandText = "INSERT INTO Identification (CollectionSpecimenID, IdentificationUnitID, IdentificationSequence, TaxonomicName, NameURI)" +
                            " VALUES (" + CollectionSpecimenID.ToString() + ", 1, 1, '" + this.userControlModuleRelatedEntryIdentification.textBoxValue.Text + "', ";
                        if (this.userControlModuleRelatedEntryIdentification.labelURI.Text.Length > 0)
                            cmd.CommandText += " '" + this.userControlModuleRelatedEntryIdentification.labelURI.Text + "' ";
                        else
                            cmd.CommandText += "NULL";
                        cmd.CommandText += ")";
                        cmd.ExecuteNonQuery();
                    }
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
                            ValueList += ", " + this.userControlDatePanelAccessionDate.maskedTextBoxDay.Text.ToString();
                        }
                        if (this.userControlDatePanelCollectionDate.maskedTextBoxMonth.Text.Length > 0)
                        {
                            FieldList += ", CollectionMonth";
                            ValueList += ", " + this.userControlDatePanelAccessionDate.maskedTextBoxMonth.Text.ToString();
                        }
                        if (this.userControlDatePanelCollectionDate.maskedTextBoxYear.Text.Length > 0)
                        {
                            FieldList += ", CollectionYear";
                            ValueList += ", " + this.userControlDatePanelAccessionDate.maskedTextBoxYear.Text.ToString();
                        }
                        if (this.userControlDatePanelCollectionDate.textBoxSupplement.Text.Length > 0)
                        {
                            FieldList += ", CollectionDateSupplement";
                            ValueList += ", '" + this.userControlDatePanelAccessionDate.textBoxSupplement.Text.ToString() + "'";
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
                        cmd.CommandText = "INSERT INTO CollectionEvent (" + FieldList + " ) VALUES (" + ValueList + ")";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "SELECT SCOPE_IDENTITY()";
                        int EventID = System.Int32.Parse(cmd.ExecuteScalar().ToString());
                        // Geography
                        if (this.userControlModuleRelatedEntryGazetteer.textBoxValue.Text.Length > 0)
                        {
                            cmd.CommandText = "INSERT INTO CollectionGeography (CollectionEventID, LocalisationSystemID, Location1, Location2, AverageAltitudeCache, AverageLatitudeCache, AverageLongitudeCache)" +
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
                    // CollectionStorage
                    cmd.CommandText = "INSERT INTO CollectionStorage ( CollectionSpecimenID, CollectionID, MaterialCategory)" +
                        " VALUES (" + CollectionSpecimenID.ToString() + ", " + CollectionID.ToString() + ", '" + this.comboBoxMaterialCategory.SelectedValue.ToString() + "')";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (System.Exception ex)
            {
                Message = ex.Message;
            }
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
                sw.WriteLine("Folder of original images:\t" + this.textBoxFolderArchive.Text);
                if (this.checkBoxAppendDateToFilename.Checked)
                    sw.WriteLine("Append date and time to filename");
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
                if (this.checkBoxSubdirectory.Checked)
                    sw.WriteLine("Import in subdirectries:\tlength. " + this.numericUpDownSubdirectry.Value.ToString() + " e.g.: " + this.textBoxSubdirectory.Text);
                sw.WriteLine("Collection:\t\t\t" + this.comboBoxCollection.Text);
                sw.WriteLine("Taxonomic group:\t\t" + this.comboBoxTaxonomicGroup.Text);
                sw.WriteLine("Image type:\t\t\t" + this.comboBoxImageType.Text);
                sw.WriteLine("Project:\t\t\t" + this.comboBoxProject.Text);
                sw.WriteLine("Material category:\t\t" + this.comboBoxMaterialCategory.Text);
                sw.Write("Images imported in Database:\t");
                if (this.radioButtonOriginalImage.Checked) sw.WriteLine("Original");
                if (this.radioButtonPreviewImage.Checked) sw.WriteLine("Preview");
                if (this.radioButtonWebImage.Checked) sw.WriteLine("Web");
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
            this.textBoxLogfile.Text = this.textBoxFolderArchive.Text + @"\LogImport_" + System.Environment.UserName + "_"
                + System.DateTime.Now.Year.ToString().PadLeft(4, '0')
                + System.DateTime.Now.Month.ToString().PadLeft(2, '0')
                + System.DateTime.Now.Day.ToString().PadLeft(2, '0')
                + "_" + System.DateTime.Now.Hour.ToString().PadLeft(2, '0')
                + System.DateTime.Now.Minute.ToString().PadLeft(2, '0')
                + System.DateTime.Now.Second.ToString().PadLeft(2, '0') + ".log";
        }

        #endregion

        #region Properties
        private Microsoft.Data.SqlClient.SqlConnection SqlConnection
        {
            get
            {
                if (this._SqlConnection == null)
                    this._SqlConnection = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                return this._SqlConnection;
            }
        }
        
        #endregion

        #region Mandatory indications

        private void setMandatoryIndications()
        {
            this.checkBoxArchiveImages_CheckedChanged(null, null);
            this.checkBoxBarcodeLength_CheckedChanged(null, null);
            this.checkBoxBarcodeStart_CheckedChanged(null, null);
            this.checkBoxCutAfterSeparator_CheckedChanged(null, null);
            this.checkBoxImageAsUri_CheckedChanged(null, null);
            this.checkBoxPreviewImages_CheckedChanged(null, null);
            this.checkBoxSubdirectory_CheckedChanged(null, null);
            this.checkBoxWebImages_CheckedChanged(null, null);
            this.setMandatoryIndicationsForScaling();
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

        private void checkBoxWebImages_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.checkBoxWebImages.Checked)
            //{
            //    this.textBoxFolderWeb.BackColor = System.Drawing.Color.Pink;
            //    this.comboBoxImageFormatWeb.BackColor = System.Drawing.Color.Pink;
            //}
            //else
            //{
            //    this.textBoxFolderWeb.BackColor = System.Drawing.SystemColors.Window;
            //    this.comboBoxImageFormatWeb.BackColor = System.Drawing.SystemColors.Window;
            //}
            this.setMandatoryIndicationsForScaling();
        }

        private void checkBoxPreviewImages_CheckedChanged(object sender, EventArgs e)
        {
            //if (this.checkBoxPreviewImages.Checked)
            //{
            //    this.textBoxFolderPreview.BackColor = System.Drawing.Color.Pink;
            //    this.comboBoxImageFormatPreview.BackColor = System.Drawing.Color.Pink;
            //}
            //else
            //{
            //    this.textBoxFolderPreview.BackColor = System.Drawing.SystemColors.Window;
            //    this.comboBoxImageFormatPreview.BackColor = System.Drawing.SystemColors.Window;
            //}
            this.setMandatoryIndicationsForScaling();
        }

        private void setMandatoryIndicationsForScaling()
        {
            //this.textBoxScaleWeb.BackColor = System.Drawing.SystemColors.Window;
            //this.textBoxScalePreview.BackColor = System.Drawing.SystemColors.Window;
            //this.comboBoxImageWidthWeb.BackColor = System.Drawing.SystemColors.Window;
            //this.comboBoxImageWidthPreview.BackColor = System.Drawing.SystemColors.Window;
            //if (this.radioButtonScale.Checked)
            //{
            //    if (this.checkBoxWebImages.Checked)
            //        this.textBoxScaleWeb.BackColor = System.Drawing.Color.Pink;
            //    if (this.checkBoxPreviewImages.Checked)
            //        this.textBoxScalePreview.BackColor = System.Drawing.Color.Pink;
            //}
            //else
            //{
            //    if (this.checkBoxWebImages.Checked)
            //        this.comboBoxImageWidthWeb.BackColor = System.Drawing.Color.Pink;
            //    if (this.checkBoxPreviewImages.Checked)
            //        this.comboBoxImageWidthPreview.BackColor = System.Drawing.Color.Pink;
            //}
        }

        private void radioButtonWidth_CheckedChanged(object sender, EventArgs e)
        {
            this.setMandatoryIndicationsForScaling();
        }

        private void checkBoxImageAsUri_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxImageAsUri.Checked)
                this.textBoxWebFolder.BackColor = System.Drawing.Color.Pink;
            else
                this.textBoxWebFolder.BackColor = System.Drawing.SystemColors.Window;
        }

        private void checkBoxSubdirectory_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxSubdirectory.Checked)
                this.numericUpDownSubdirectry.BackColor = System.Drawing.Color.Pink;
            else
                this.numericUpDownSubdirectry.BackColor = System.Drawing.SystemColors.Window;
        }

        private void checkBoxArchiveImages_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxArchiveImages.Checked)
                this.textBoxFolderArchive.BackColor = System.Drawing.Color.Pink;
            else
                this.textBoxFolderArchive.BackColor = System.Drawing.SystemColors.Window;
        }

       #endregion

    }
}