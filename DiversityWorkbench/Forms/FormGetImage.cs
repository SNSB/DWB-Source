using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetImage : Form
    {

        #region Parameter

        private string _AccessionNumber = "";
        private string _AccessionNumberConverted = "";
        // Supported image formats
        private String[] _MediaServiceSupportedImageFormats = { ".jpg", ".jpeg", ".bmp", ".tif", ".tiff", ".png", ".gif" };
        // Supported audio formats
        private String[] _MediaServiceSupportedAudioFormats = { ".wav", ".mp4" };
        // Package size in bytes
        private const int m_PackageSize = 4193000; // < 4 MB !!
        private long _SizeOfFile;
        private int? _ProjectID;
        //private int? _AgentID;
        //private string _RowGUID = "";

        #endregion

        #region Construction

        public FormGetImage()
        {
            InitializeComponent();
            this.splitContainerResource.Panel2Collapsed = true;
            this.userControlXMLTreeEXIF.setToDisplayOnly();
            this.userControlXMLTreeEXIF.Visible = false;
        }

        //public FormGetImage(int ProjectID, int AgentID, string AccessionNumber, string RowGUID) : this()
        //{
        //    this._ProjectID = ProjectID;
        //    this._AgentID = AgentID;
        //    this._AccessionNumber = AccessionNumber;
        //    this._RowGUID = RowGUID;
        //    this.CreateFilesForMediaService();
        //    this.EnableMediaServiceControls();
        //    try
        //    {
        //        string UpleadDirectory = "Upload file via MediaService ";
        //        string SQL = "SELECT Project FROM ProjectProxy WHERE ProjectID = " + this._ProjectID.ToString();
        //        UpleadDirectory += "to " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).Replace(" ", "_") + "/" + this._AgentID.ToString();
        //        this.toolTip.SetToolTip(this.buttonUpload, UpleadDirectory);
        //    }
        //    catch { }
        //}

        public FormGetImage(int? ProjectID, string AccessionNumber)
            : this()
        {
            this._ProjectID = ProjectID;
            this._AccessionNumber = AccessionNumber;
            this.CreateFilesForMediaService();
            this.EnableMediaServiceControls();
            try
            {
                if (ProjectID != null)
                {
                    string UpleadDirectory = "Upload file via MediaService ";
                    string SQL = "SELECT Project FROM ProjectProxy WHERE ProjectID = " + this._ProjectID.ToString();
                    UpleadDirectory += "to " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).Replace(" ", "_") + "/" + this.UploadDirectory;
                    this.toolTip.SetToolTip(this.buttonUpload, UpleadDirectory);
                }
            }
            catch (System.Exception Exception) { }
        }

        public FormGetImage(string Folder, bool ShowMediaService)
            : this()
        {
            if (ShowMediaService)
            {
                this.CreateFilesForMediaService();
                this.EnableMediaServiceControls();
                try
                {
                    string UpleadDirectory = "Upload file via MediaService ";
                    string SQL = "SELECT Project FROM ProjectProxy WHERE ProjectID = " + this._ProjectID.ToString();
                    UpleadDirectory += "to " + DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL).Replace(" ", "_") + "/" + this.UploadDirectory;
                    this.toolTip.SetToolTip(this.buttonUpload, UpleadDirectory);
                }
                catch { }
            }
            this.MediaServiceControlsAreVisible = ShowMediaService;
            this.textBoxImageFilePath.Text = Folder;
        }

        #endregion

        #region Form

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Image

        private void buttonOpenFile_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog == null)
            {
                this.openFileDialog = new OpenFileDialog();
                string Filter = "|";
                for (int i = 0; i < this._MediaServiceSupportedImageFormats.Length; i++)
                {
                    if (i > 0) Filter = ", " + Filter;
                    Filter = this._MediaServiceSupportedImageFormats[i].Replace(".", "") + Filter;
                    if (i > 0) Filter = Filter + ", ";
                    Filter = Filter + this._MediaServiceSupportedImageFormats[i];
                }
                this.openFileDialog.Filter = Filter;
            }
            if (this.openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBoxImageFilePath.Text = this.openFileDialog.FileName;
            }
        }

        private void buttonOpenBrowser_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f;
            if (this.textBoxImageFilePath.Text.Length > 0 && this.textBoxImageFilePath.Text.StartsWith("http:"))
                f = new FormWebBrowser(this.textBoxImageFilePath.Text);
            else
                f = new FormWebBrowser();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxImageFilePath.Text = f.URL;
        }

        private void setSize()
        {
            this._SizeOfFile = DiversityWorkbench.Forms.FormFunctions.SizeOfFile(this.textBoxImageFilePath.Text);
            if (this._SizeOfFile == -1)
                this.labelSizeInMB.Text = "unknown";
            else
                this.labelSizeInMB.Text = (this._SizeOfFile / 1000).ToString() + " kb";
        }

        private void textBoxImageFilePath_TextChanged(object sender, System.EventArgs e)
        {
            this.ImagePath = this.textBoxImageFilePath.Text.ToString();
            this.EnableMediaServiceControls();
            this.setSize();
            if (!this.ImagePath.StartsWith("http://") && !this.ImagePath.StartsWith("https://"))
                this.CreateFilesForMediaService();
            else if (this.ImagePath.StartsWith("http://") || !this.ImagePath.StartsWith("https://"))
            {
            }
            string EXIF = this.Exif;
            if (EXIF.Length > 0)
            {
                this.userControlXMLTreeEXIF.XML = EXIF;
                this.userControlXMLTreeEXIF.Visible = true;
            }
            else
                this.userControlXMLTreeEXIF.Visible = false;
        }

        public string ImagePath
        {
            get
            {
                return this.textBoxImageFilePath.Text;
            }
            set
            {
                string Path = value;
                if (Path == "")
                {
                    this.pictureBox.Image = null;
                }
                else
                {
                    try
                    {
                        Path = Path.Replace("/", "\\");
                        if (Path.StartsWith("http:") || Path.StartsWith("https:"))
                        {
                            try
                            {
                                System.Net.WebRequest webrq = System.Net.WebRequest.Create(value);


                                DiversityWorkbench.Forms.FormFunctions.Medium MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(value);
                                switch (MediumType)
                                {
                                    case DiversityWorkbench.Forms.FormFunctions.Medium.Image:
                                        this.splitContainerResource.Panel1Collapsed = false;
                                        this.splitContainerResource.Panel2Collapsed = true;
                                        if (Path.StartsWith("http:") || Path.StartsWith("https:"))
                                        {
                                            System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webrq.GetResponse().GetResponseStream());
                                            this.pictureBox.Image = bmp;
                                        }
                                        else
                                        {
                                            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(value);
                                            this.pictureBox.Image = bmp;
                                        }
                                        break;
                                    case DiversityWorkbench.Forms.FormFunctions.Medium.Audio:
                                        this.splitContainerResource.Panel1Collapsed = true;
                                        this.splitContainerResource.Panel2Collapsed = false;
                                        this.splitContainerNonImage.Panel1Collapsed = true;
                                        this.splitContainerNonImage.Panel2Collapsed = false;
                                        this.userControlMediaPlayer.MediaImage = (System.Drawing.Bitmap)this.DefaultIconAudio;
                                        this.userControlMediaPlayer.File = value;
                                        break;
                                    case DiversityWorkbench.Forms.FormFunctions.Medium.Video:
                                        this.splitContainerResource.Panel1Collapsed = true;
                                        this.splitContainerResource.Panel2Collapsed = false;
                                        this.splitContainerNonImage.Panel1Collapsed = true;
                                        this.splitContainerNonImage.Panel2Collapsed = false;
                                        this.userControlMediaPlayer.MediaImage = (System.Drawing.Bitmap)this.DefaultIconVideo;
                                        this.userControlMediaPlayer.File = value;
                                        break;
                                    case DiversityWorkbench.Forms.FormFunctions.Medium.VectorGraphic:
                                    case DiversityWorkbench.Forms.FormFunctions.Medium.Unknown:
                                        this.splitContainerResource.Panel1Collapsed = true;
                                        this.splitContainerResource.Panel2Collapsed = false;
                                        this.splitContainerNonImage.Panel1Collapsed = false;
                                        this.splitContainerNonImage.Panel2Collapsed = true;
                                        System.Uri U = new Uri(value);
                                        this.webBrowser.Url = U;
                                        if (this.webBrowser.Url?.AbsoluteUri != value)
                                        {
                                            this.webBrowser.BeginInvoke(null);
                                            this.webBrowser.Focus();
                                            this.webBrowser.EndInvoke(null);
                                        }
                                        break;
                                    default:
                                        this.splitContainerResource.Panel1Collapsed = true;
                                        this.splitContainerResource.Panel2Collapsed = false;
                                        this.splitContainerNonImage.Panel1Collapsed = false;
                                        this.splitContainerNonImage.Panel2Collapsed = true;
                                        this.webBrowser.Url = null;

                                        break;
                                }

                                //System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webrq.GetResponse().GetResponseStream());
                                //this.pictureBox.Image = bmp;
                            }
                            catch (System.OutOfMemoryException ex)
                            {
                                DiversityWorkbench.UserControls.UserControlImage uci = new DiversityWorkbench.UserControls.UserControlImage();
                                this.pictureBox.Image = (Bitmap)uci.DefaultIconTooBig;
                            }
                            catch (System.Exception x)
                            {
                                this.pictureBox.Image = null;
                            }
                        }
                        else
                        {
                            System.IO.FileInfo File = new System.IO.FileInfo(Path);
                            if (File.Exists)
                            {
                                try
                                {
                                    DiversityWorkbench.Forms.FormFunctions.Medium MediumType = DiversityWorkbench.Forms.FormFunctions.MediaType(value);
                                    switch (MediumType)
                                    {
                                        case DiversityWorkbench.Forms.FormFunctions.Medium.Image:
                                            System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(value);
                                            this.pictureBox.Image = bmp;
                                            break;
                                        case DiversityWorkbench.Forms.FormFunctions.Medium.Audio:
                                            break;
                                        case DiversityWorkbench.Forms.FormFunctions.Medium.Video:
                                            break;
                                        case DiversityWorkbench.Forms.FormFunctions.Medium.VectorGraphic:
                                            this.splitContainerResource.Panel1Collapsed = true;
                                            this.splitContainerResource.Panel2Collapsed = false;
                                            this.splitContainerNonImage.Panel1Collapsed = false;
                                            this.splitContainerNonImage.Panel2Collapsed = true;
                                            System.Uri URI = new Uri(value);
                                            this.webBrowser.Url = URI;
                                            if (this.webBrowser.Url.AbsoluteUri != value)
                                            {
                                                this.webBrowser.BeginInvoke(null);
                                                this.webBrowser.Focus();
                                                this.webBrowser.EndInvoke(null);
                                            }
                                            break;
                                        case DiversityWorkbench.Forms.FormFunctions.Medium.Unknown:
                                            this.splitContainerResource.Panel1Collapsed = true;
                                            this.splitContainerResource.Panel2Collapsed = false;
                                            System.Uri U = new Uri(value);
                                            this.webBrowser.Url = U;
                                            if (this.webBrowser.Url.AbsoluteUri != value)
                                            {
                                                this.webBrowser.BeginInvoke(null);
                                                this.webBrowser.Focus();
                                                this.webBrowser.EndInvoke(null);
                                            }
                                            break;
                                        default:
                                            this.splitContainerResource.Panel1Collapsed = true;
                                            this.splitContainerResource.Panel2Collapsed = false;
                                            this.webBrowser.Url = null;
                                            break;
                                    }
                                    //System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(value);
                                    //this.pictureBox.Image = bmp;
                                }
                                catch (System.OutOfMemoryException ex)
                                {
                                    DiversityWorkbench.UserControls.UserControlImage uci = new DiversityWorkbench.UserControls.UserControlImage();
                                    this.pictureBox.Image = (Bitmap)uci.DefaultIconTooBig;
                                }
                                catch (System.Exception ex)
                                {
                                    this.pictureBox.Image = null;
                                }
                            }
                        }
                    }
                    catch (System.ArgumentException ex)
                    {
                        try
                        {
                            System.Net.WebRequest webrq = System.Net.WebRequest.Create(value);
                            System.Drawing.Bitmap bmp = (Bitmap)Bitmap.FromStream(webrq.GetResponse().GetResponseStream());
                            this.pictureBox.Image = bmp;
                        }
                        catch (System.OutOfMemoryException oomex)
                        {
                            DiversityWorkbench.UserControls.UserControlImage uci = new DiversityWorkbench.UserControls.UserControlImage();
                            this.pictureBox.Image = (Bitmap)uci.DefaultIconTooBig;
                        }
                        catch (System.Exception x)
                        {
                            this.pictureBox.Image = null;
                        }
                    }
                }
            }
        }

        public System.Drawing.Image DefaultIconAudio
        {
            get { return this.imageListMedia.Images[0]; }
        }

        public System.Drawing.Image DefaultIconVideo
        {
            get { return this.imageListMedia.Images[1]; }
        }

        public string Exif
        {
            get
            {
                string X = "";
                try
                {
                    string ExifToolFile = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ApplicationDirectory() + "\\exiftool.exe";
                    System.IO.FileInfo FI = new FileInfo(ExifToolFile);
                    if (FI.Exists)
                    {
                        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                        startInfo.CreateNoWindow = true;
                        startInfo.UseShellExecute = false;
                        startInfo.RedirectStandardOutput = true;
                        startInfo.FileName = ExifToolFile;
                        startInfo.WorkingDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ApplicationDirectory(); // ...Windows.Forms.Application.StartupPath;

                        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
                        startInfo.Arguments = "-X \"" + this.ImagePath + "\"";
                        using (System.Diagnostics.Process exeProcess = System.Diagnostics.Process.Start(startInfo))
                        {
                            //Read text from the process' standard output
                            using (StreamReader reader = exeProcess.StandardOutput)
                            {
                                X = reader.ReadToEnd();
                                X = X.Substring(X.IndexOf("<rdf:RDF"));
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    X = "";
                }
                return X;
            }
        }

        private string ReadFromClipboard()
        {
            //Gibt den Inhalt aus dem Clipboard zurück

            string strTextFromClipboard = "";
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                string strChar = (String)iData.GetData(DataFormats.Text);
                strTextFromClipboard = strChar;
            }
            else if (iData.GetDataPresent(DataFormats.UnicodeText))
            {
                string strChar = (String)iData.GetData(DataFormats.UnicodeText);
                strTextFromClipboard = strChar;
            }
            return strTextFromClipboard;
        }

        #endregion

        #region MediaService

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            try
            {
                //string Message = this.ReadAndTransferFile(this.textBoxImageFilePath.Text);
                //if (Message.StartsWith("http:"))
                //{
                //    this.textBoxImageFilePath.Text = Message;
                //}
                //else
                //    System.Windows.Forms.MessageBox.Show(Message);
                System.Windows.Forms.MessageBox.Show("Currently uploading is not possible");
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Creates file names according to the functions of the MediaService
        /// </summary>
        private void CreateFilesForMediaService()
        {
            if (this.textBoxImageFilePath.Text.Length == 0 && this._AccessionNumber.Trim().Length == 0)
                return;
            //else if (this.textBoxImageFilePath.Text.Length == 0 && this._AccessionNumber.Trim().Length > 0)
            //    this.radioButtonRenameWithAccNrForMediaService.Text = this.AccessionNumberConverted;
            try
            {
                System.IO.FileInfo FI = new FileInfo(this.textBoxImageFilePath.Text);
                if (FI.Exists)
                {
                    if (this._AccessionNumber.Length > 0)// && this.textBoxImageFilePath.Text.Length > 0)
                    {
                        if (!this.labelNameForMediaService.Text.EndsWith(FI.Extension))
                            this.labelNameForMediaService.Text = this.AccessionNumberConverted + FI.Extension;

                        //string NewFileName = this.AccessionNumberConverted;

                        //string TimeExtension = "";
                        //try
                        //{
                        //    DateTime DT = FI.CreationTime;
                        //    TimeExtension = "_" + DT.Year.ToString();
                        //    if (System.DateTime.Now.Month < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += DT.Month.ToString();
                        //    if (DT.Day < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += DT.Day.ToString();
                        //    TimeExtension += "_";
                        //    if (DT.Hour < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += DT.Hour.ToString();
                        //    if (DT.Minute < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += DT.Minute.ToString();
                        //    if (DT.Second < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += DT.Second.ToString();
                        //}
                        //catch
                        //{
                        //    TimeExtension = TimeExtension + "_" + System.DateTime.Now.Year.ToString();
                        //    if (System.DateTime.Now.Month < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += System.DateTime.Now.Month.ToString();
                        //    if (System.DateTime.Now.Day < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += System.DateTime.Now.Day.ToString();
                        //    TimeExtension += "_";
                        //    if (System.DateTime.Now.Hour < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += System.DateTime.Now.Hour.ToString();
                        //    if (System.DateTime.Now.Minute < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += System.DateTime.Now.Minute.ToString();
                        //    if (System.DateTime.Now.Second < 10)
                        //        TimeExtension += "0";
                        //    TimeExtension += System.DateTime.Now.Second.ToString();
                        //}

                        //NewFileName += TimeExtension + FI.Extension;

                        //this.labelNameForMediaService.Text = this.AccessionNumberConverted;
                        //this.radioButtonRenameWithAccNrForMediaService.Checked = true;
                    }
                    //else if (this._AccessionNumber.Trim().Length > 0)
                    //    this.labelNameForMediaService.Text = this.AccessionNumberConverted;
                    else
                        this.labelNameForMediaService.Text = "";

                    //if (this._RowGUID.Length > 0)
                    //{
                    //    string FileNamePart = FI.Name.Replace(FI.Extension, "");
                    //    string[] FileNameParts = FileNamePart.Split(new char[] { '-' });
                    //    if (FileNameParts.Length == 5
                    //        && FileNameParts[0].Length == 8
                    //        && FileNameParts[1].Length == 4
                    //        && FileNameParts[2].Length == 4
                    //        && FileNameParts[3].Length == 4
                    //        && FileNameParts[4].Length == 12)
                    //        this.radioButtonRenameWithGuidForMediaService.Text = FI.Name;
                    //    else
                    //    {
                    //        this.radioButtonRenameWithGuidForMediaService.Text = this._RowGUID + FI.Extension;
                    //        if (!this.radioButtonRenameWithAccNrForMediaService.Checked)
                    //            this.radioButtonRenameWithGuidForMediaService.Checked = true;
                    //    }
                    //}
                    //else
                    //    this.radioButtonRenameWithGuidForMediaService.Text = "";
                }
            }
            catch { }
        }

        /// <summary>
        /// If the image type is supported by the media service
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>If the type ist supported</returns>
        private bool isSupportedImageFile(String fileName)
        {
            try
            {
                // Check if filename extension is supported
                foreach (String str in _MediaServiceSupportedImageFormats)
                {
                    if ((fileName.ToLower()).EndsWith(str))
                        return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// If the audio type is supported by the media service
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>If the type ist supported</returns>
        private bool isSupportedAudioFile(String fileName)
        {
            try
            {
                // Check if filename extension is supported
                foreach (String str in _MediaServiceSupportedAudioFormats)
                {
                    if ((fileName.ToLower()).EndsWith(str))
                        return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Enabling the controls for uploading files using the webservice MediaService
        /// </summary>
        private void EnableMediaServiceControls()
        {
            try
            {
                if (this.labelNameForMediaService.Text.Length == 0 &&
                    this.AccessionNumberConverted.Length > 0)
                    this.labelNameForMediaService.Text = this.AccessionNumberConverted;

                if (this.labelDirectoryForMediaService.Text.Length == 0 &&
                    this.UploadDirectory.Length > 0)
                    this.labelDirectoryForMediaService.Text = this.UploadDirectory + "/";

                // nur zum Testen verwendet
                this.labelNameForMediaService.Visible = false;

                if (this._ProjectID == null || (this._ProjectID != null && !this.ProjectIsValidForUpload((int)this._ProjectID)) || this.AccessionNumberConverted.Length == 0 || this.UploadDirectory.Length == 0) // this._AgentID == 0 || this._ProjectID == 0)
                {
                    this.checkBoxRenameForMediaService.Enabled = false;
                    this.buttonUpload.Enabled = false;
                    string Message = "Upload can not be used:\r\n";
                    if (this._ProjectID == null)
                        Message += "You must select a project.";
                    if (this.AccessionNumberConverted.Length == 0)
                        Message += " You need a valid accession number.";
                    else if (this.UploadDirectory.Length == 0)
                    {
                        if (this.ProjectIsValidForUpload((int)this._ProjectID))
                            Message += " You need a valid accession number: At least one letter + at least 2 digits";
                        else
                            Message += " You need a valid project at the SNSB IT-Center.";
                    }
                    this.labelUploadNotPossible.Text = Message;
                    this.labelUploadNotPossible.Visible = true;
                    this.MediaServiceControlsAreVisible = false;
                    return;
                }
                else
                {
                    this.labelUploadNotPossible.Visible = false;
                    this.MediaServiceControlsAreVisible = true;
                }
                bool OK = true;
                if (this.textBoxImageFilePath.Text.Length == 0)
                    OK = false;
                else
                {
                    if (this.isSupportedImageFile(this.textBoxImageFilePath.Text)
                        || this.isSupportedAudioFile(this.textBoxImageFilePath.Text))
                        OK = true;
                    else
                        OK = false;
                }

                if (this._AccessionNumber.Length == 0)
                {
                    this.labelNameForMediaService.Text = "Accession number missing";
                }
                this.checkBoxRenameForMediaService.Enabled = OK;
                this.labelNameForMediaService.Enabled = OK;
                this.labelSize.Enabled = OK;
                this.labelSizeInMB.Enabled = OK;
                this.labelDirectoryForMediaService.Enabled = OK;
                if (OK && this.checkBoxRenameForMediaService.Checked)
                    this.buttonUpload.Enabled = true;
                else
                    this.buttonUpload.Enabled = false;
                if (OK)
                {
                    this.MediaServiceControlsAreVisible = true;
                }
            }
            catch (System.Exception ex) { }
        }

        private bool? _ProjectIsValidForUpload;
        private bool ProjectIsValidForUpload(int ProjectID)
        {
            if (this._ProjectIsValidForUpload == null)
            {
                this._ProjectIsValidForUpload = false;
                string SQL = "SELECT substring([ProjectURI], 1, LEN([ProjectURI]) - LEN(cast(" + ProjectID.ToString() + " as varchar))) " +
                    "FROM [ProjectProxy] p where p.ProjectID = " + ProjectID.ToString();
                string URI = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                string idProjectUri = global::DiversityWorkbench.Properties.Settings.Default.DiversityWokbenchIDUrl + "Projects/";
                if (URI == idProjectUri)
                    this._ProjectIsValidForUpload = true;
            }
            return (bool)this._ProjectIsValidForUpload;
        }

        // Ariane uncommented Medthods for .net8

        ///// <summary>
        ///// Transfer selected image file to Webserver
        ///// </summary>
        ///// <param name="path"></param>
        //private string ReadAndTransferFile(String path)
        //{
        //    this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
        //    String retString = String.Empty;
        //    FileStream fileStrm = null;
        //    BinaryReader rdr = null;
        //    byte[] data = null;

        //    // Create proxy instance
        //    MediaServiceProxy.PhoneMediaServiceClient proxy = new MediaServiceProxy.PhoneMediaServiceClient();

        //    FileInfo myFile = new FileInfo(path);
        //    string m_ImageName = myFile.Name;
        //    string lastWriteTime = myFile.LastWriteTime.ToString();

        //    try
        //    {
        //        // Create stream and reader for file data
        //        fileStrm = new FileStream(path, FileMode.Open, FileAccess.Read);
        //        rdr = new BinaryReader(fileStrm);

        //        // Number of bytes to be transferred
        //        long numBytes = fileStrm.Length;

        //        // Package counter
        //        int count = 0;

        //        if (this._SizeOfFile > 4193000)
        //        {
        //            try
        //            {
        //                // Webservice method call
        //                String retStr = String.Empty;
        //                retStr = proxy.BeginTransaction(m_ImageName, m_ImageName, "photograph", (float)0, (float)0, (float)0, this.UploadDirectory, lastWriteTime, (int)this._ProjectID);
        //                //Message = retStr;

        //                // Read complete packages
        //                while (numBytes > (long)m_PackageSize)
        //                {
        //                    // Read package of data from file
        //                    data = rdr.ReadBytes(m_PackageSize);

        //                    // Webservice method call
        //                    if ((retStr = proxy.EncodeFile(data)) != String.Empty)
        //                    {
        //                        //Message = retStr;
        //                        numBytes = 0;
        //                    }
        //                    else
        //                    {
        //                        count++;
        //                        numBytes -= (long)m_PackageSize;
        //                    }
        //                }

        //                // Read rest of file data
        //                if (numBytes > 0)
        //                {
        //                    data = rdr.ReadBytes((int)numBytes);

        //                    if ((retStr = proxy.EncodeFile(data)) != String.Empty)
        //                    {
        //                        //Message = retStr;
        //                    }
        //                    else
        //                    {
        //                        count++;
        //                    }
        //                    numBytes = 0;
        //                }

        //                // Webservice method call
        //                retString = proxy.Commit();

        //            }
        //            catch (Exception ex)
        //            {
        //                System.Windows.Forms.MessageBox.Show(ex.Message);
        //            }

        //        }

        //        //TimeSpan dif = DateTime.Now - start;
        //        else
        //        {
        //            if (numBytes > 0)
        //            {
        //                data = rdr.ReadBytes((int)numBytes);
        //                retString = proxy.Submit(m_ImageName, m_ImageName, "photograph", (float)0, (float)0, (float)0, this.UploadDirectory, lastWriteTime, (int)this._ProjectID, data); // IDs 372, 372, 374
        //            }
        //        }


        //        // Close reader and stream
        //        rdr.Close();
        //        fileStrm.Close();
        //    }
        //    catch (Exception ex)
        //    {

        //        // Close reader and stream if open
        //        retString = ex.Message;
        //        if (rdr != null)
        //            rdr.Close();
        //        if (fileStrm != null)
        //            fileStrm.Close();
        //    }
        //    finally
        //    {
        //        // Abort faulted proxy
        //        if (proxy.State == System.ServiceModel.CommunicationState.Faulted)
        //        {
        //            proxy.Abort();
        //        }
        //        // Close proxy
        //        else if (proxy.State == System.ServiceModel.CommunicationState.Opened)
        //        {
        //            proxy.Close();
        //        }
        //        this.Cursor = System.Windows.Forms.Cursors.Default;
        //    }
        //    return retString;
        //}

        //private string TransferFile(BinaryReader rdr,
        //    MediaServiceProxy.PhoneMediaServiceClient proxy,
        //    string m_ImageName,
        //    long numBytes,
        //    byte[] data,
        //    string lastWriteTime)
        //{
        //    string Message = "";
        //    try
        //    {
        //        String retString = String.Empty;
        //        if (numBytes > 0)
        //        {
        //            data = rdr.ReadBytes((int)numBytes);
        //            retString = proxy.Submit(m_ImageName, m_ImageName, "photograph", (float)11.2345, (float)48.9876, (float)333.33, "00000", lastWriteTime, 702, data); // IDs 372, 372, 374
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Message = ex.Message;
        //    }
        //    return Message;
        //}

        //private string TransferFile(BinaryReader rdr,
        //    MediaServiceProxy.PhoneMediaServiceClient proxy,
        //    string m_ImageName,
        //    long numBytes,
        //    int count,
        //    byte[] data,
        //    string lastWriteTime)
        //{
        //    string Message = "";
        //    try
        //    {
        //        // Webservice method call
        //        String retStr = String.Empty;
        //        retStr = proxy.BeginTransaction(m_ImageName, m_ImageName, "Image", (float)11.2345, (float)48.9876, (float)333.33, "Wolfgang Reichert", lastWriteTime, 3);
        //        Message = retStr;

        //        // Read complete packages
        //        while (numBytes > (long)m_PackageSize)
        //        {
        //            // Read package of data from file
        //            data = rdr.ReadBytes(m_PackageSize);

        //            // Webservice method call
        //            if ((retStr = proxy.EncodeFile(data)) != String.Empty)
        //            {
        //                Message = retStr;
        //                numBytes = 0;
        //            }
        //            else
        //            {
        //                count++;
        //                numBytes -= (long)m_PackageSize;
        //            }
        //        }

        //        // Read rest of file data
        //        if (numBytes > 0)
        //        {
        //            data = rdr.ReadBytes((int)numBytes);

        //            if ((retStr = proxy.EncodeFile(data)) != String.Empty)
        //            {
        //                Message = retStr;
        //            }
        //            else
        //            {
        //                count++;
        //            }
        //            numBytes = 0;
        //        }

        //        // Webservice method call
        //        String retString = proxy.Commit();

        //    }
        //    catch (Exception)
        //    {
        //    }
        //    return Message;
        //}

        /// <summary>
        /// Creates a copy of the select file according to the selected option
        /// and enables the upload button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxRenameForMediaService_CheckedChanged(object sender, EventArgs e)
        {
            this.setFileForMediaServiceUpload();
            //try
            //{
            //    if (this.checkBoxRenameForMediaService.Checked
            //        && this.textBoxImageFilePath.Text.Length > 0
            //        && (this.radioButtonRenameWithAccNrForMediaService.Text.Length > 0
            //        || this.radioButtonRenameWithGuidForMediaService.Text.Length > 0))
            //    {
            //        System.IO.FileInfo FI = new FileInfo(this.textBoxImageFilePath.Text);
            //        string newFileName = FI.DirectoryName + "\\";
            //        if (this.radioButtonRenameWithAccNrForMediaService.Checked)
            //            newFileName += this.radioButtonRenameWithAccNrForMediaService.Text;
            //        else if (this.radioButtonRenameWithGuidForMediaService.Checked)
            //            newFileName += this.radioButtonRenameWithGuidForMediaService.Text;
            //        else
            //            System.Windows.Forms.MessageBox.Show("Please select a converting option");
            //        System.IO.FileInfo FN = new FileInfo(newFileName);
            //        if (!FN.Exists)
            //            FI.CopyTo(newFileName);
            //        System.IO.FileInfo Fcheck = new FileInfo(newFileName);
            //        if (Fcheck.Exists)
            //        {
            //            this.textBoxImageFilePath.Text = FN.FullName;
            //            this.buttonUpload.Enabled = true;
            //        }
            //        else
            //            this.buttonUpload.Enabled = false;
            //        //FI.MoveTo(newFileName);
            //    }
            //    else
            //        this.buttonUpload.Enabled = false;
            //}
            //catch (System.Exception ex) 
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    this.checkBoxRenameForMediaService.Checked = false;
            //    this.buttonUpload.Enabled = false;
            //}
        }

        public bool MediaServiceControlsAreVisible
        {
            set
            {
                try
                {
                    this.buttonUpload.Visible = value;
                    //this.radioButtonRenameWithAccNrForMediaService.Visible = value;
                    //this.radioButtonRenameWithGuidForMediaService.Visible = value;
                    this.checkBoxRenameForMediaService.Visible = value;
                    this.labelSize.Visible = value;
                    this.labelSizeInMB.Visible = value;
                    this.labelDirectoryForMediaService.Visible = value;
                    this.labelNameForMediaService.Visible = value;
                }
                catch { }
            }
        }

        //private void radioButtonRenameWithAccNrForMediaService_Click(object sender, EventArgs e)
        //{
        //    this.setFileForMediaServiceUpload();
        //}

        //private void radioButtonRenameWithGuidForMediaService_Click(object sender, EventArgs e)
        //{
        //    this.setFileForMediaServiceUpload();
        //}

        private void setFileForMediaServiceUpload()
        {
            try
            {
                if (this.checkBoxRenameForMediaService.Checked
                    && this.textBoxImageFilePath.Text.Length > 0
                    && this.labelNameForMediaService.Text.Length > 0)
                {
                    System.IO.FileInfo FI = new FileInfo(this.textBoxImageFilePath.Text);
                    string newFileName = FI.DirectoryName + "\\";
                    //if (this.radioButtonRenameWithAccNrForMediaService.Checked)
                    newFileName += this.labelNameForMediaService.Text;
                    if (!newFileName.EndsWith(FI.Extension))
                        newFileName += FI.Extension;
                    //else if (this.radioButtonRenameWithGuidForMediaService.Checked)
                    //    newFileName += this.radioButtonRenameWithGuidForMediaService.Text;
                    //else
                    //    System.Windows.Forms.MessageBox.Show("Please select a converting option");
                    System.IO.FileInfo FN = new FileInfo(newFileName);
                    if (!FN.Exists)
                        FI.CopyTo(newFileName);
                    System.IO.FileInfo Fcheck = new FileInfo(newFileName);
                    if (Fcheck.Exists)
                    {
                        this.textBoxImageFilePath.Text = FN.FullName;
                        this.buttonUpload.Enabled = true;
                    }
                    else
                        this.buttonUpload.Enabled = false;
                    //FI.MoveTo(newFileName);
                }
                else
                    this.buttonUpload.Enabled = false;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                this.checkBoxRenameForMediaService.Checked = false;
                this.buttonUpload.Enabled = false;
            }
        }

        #endregion

        #region Properties

        private string AccessionNumber
        {
            //get { return _AccessionNumber; }
            set { _AccessionNumber = value; }
        }

        private string _UploadDirectory;
        private string UploadDirectory
        {
            get
            {
                if (this._UploadDirectory == null)
                {
                    _UploadDirectory = "";
                    if (this._ProjectID != null && this.ProjectIsValidForUpload((int)this._ProjectID))
                    {
                        int Numbers = 0;
                        string AccNr = this.AccessionNumberConverted.Substring(0, this.AccessionNumberConverted.Length - 16);
                        int iTest;
                        for (int i = 0; i < AccNr.Length; i++)
                        {
                            // At least one not numeric sign as Header
                            if (i == 0)
                            {
                                if (int.TryParse(AccNr.Substring(0, 1), out iTest))
                                    return "";
                                else
                                    _UploadDirectory += AccNr.Substring(i, 1);
                            }
                            else
                            {
                                if (Numbers < 4)
                                {
                                    if (this.IsANumber(AccNr.Substring(i, 1)))
                                    {
                                        _UploadDirectory += AccNr.Substring(i, 1);
                                        Numbers++;
                                    }
                                    else
                                        _UploadDirectory += AccNr.Substring(i, 1);
                                }
                                else
                                    break;
                            }
                        }
                        if (Numbers < 2)
                            this._UploadDirectory = "";
                    }
                }
                return this._UploadDirectory;
            }
        }

        private string AccessionNumberConverted
        {
            get
            {
                if (this._AccessionNumberConverted.Length > 0)
                    return this._AccessionNumberConverted;
                if (this._AccessionNumber.Trim().Length == 0)
                    return "";
                //string AccNr = this._AccessionNumber;
                this._AccessionNumberConverted = this.ConvertToAcceptedName(this._AccessionNumber);
                foreach (char C in this.NotAcceptedSigns)
                {
                    string Char = C.ToString();
                    this._AccessionNumberConverted = this._AccessionNumberConverted.Replace(Char, "_");
                }
                while (this._AccessionNumberConverted.IndexOf("__") > -1)
                    this._AccessionNumberConverted = this._AccessionNumberConverted.Replace("__", "_");

                this._AccessionNumberConverted = this._AccessionNumberConverted + "_" + System.DateTime.Now.Year.ToString();
                if (System.DateTime.Now.Month < 10)
                    this._AccessionNumberConverted += "0";
                this._AccessionNumberConverted += System.DateTime.Now.Month.ToString();
                if (System.DateTime.Now.Day < 10)
                    this._AccessionNumberConverted += "0";
                this._AccessionNumberConverted += System.DateTime.Now.Day.ToString();
                this._AccessionNumberConverted += "_";
                if (System.DateTime.Now.Hour < 10)
                    this._AccessionNumberConverted += "0";
                this._AccessionNumberConverted += System.DateTime.Now.Hour.ToString();
                if (System.DateTime.Now.Minute < 10)
                    this._AccessionNumberConverted += "0";
                this._AccessionNumberConverted += System.DateTime.Now.Minute.ToString();
                if (System.DateTime.Now.Second < 10)
                    this._AccessionNumberConverted += "0";
                this._AccessionNumberConverted += System.DateTime.Now.Second.ToString();
                return this._AccessionNumberConverted;
            }
        }

        private bool IsANumber(string C)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            Byte[] encodedBytes = ascii.GetBytes(C);
            int iPos;
            int.TryParse(encodedBytes[0].ToString(), out iPos);
            if (iPos > 47 && iPos < 58)
                return true;
            else return false;
        }

        private string ConvertToAcceptedName(string Input)
        {
            string Output = "";
            ASCIIEncoding ascii = new ASCIIEncoding();
            Byte[] encodedBytes = ascii.GetBytes(Input);
            for (int i = 0; i < encodedBytes.Length; i++)
            {
                int iPos;
                int.TryParse(encodedBytes[i].ToString(), out iPos);
                if ((iPos > 47 && iPos < 58)
                    || (iPos > 64 && iPos < 91)
                    || iPos == 45
                    || (iPos > 96 && iPos < 123))
                    Output += Input[i];
                else
                    Output += "_";
            }
            while (Output.IndexOf("__") > -1)
                Output = Output.Replace("__", "_");
            return Output;
        }

        private System.Collections.Generic.List<char> NotAcceptedSigns
        {
            get
            {
                System.Collections.Generic.List<char> Signs = new List<char>();
                Signs.Add(',');
                Signs.Add('.');
                Signs.Add(' ');
                Signs.Add('/');
                Signs.Add('\\');
                Signs.Add(':');
                Signs.Add('*');
                Signs.Add('?');
                Signs.Add('<');
                Signs.Add('>');
                Signs.Add(',');
                Signs.Add('|');
                Signs.Add('\'');
                Signs.Add('"');
                Signs.Add(',');
                Signs.Add('(');
                Signs.Add(')');
                Signs.Add('=');
                Signs.Add('{');
                Signs.Add('}');
                Signs.Add('[');
                Signs.Add(']');
                Signs.Add('+');
                Signs.Add(';');
                Signs.Add('#');
                Signs.Add('ß');
                return Signs;
            }
        }

        public string URIImage { get { return this.textBoxImageFilePath.Text; } }

        #endregion


    }
}