using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormConvertImages : Form
    {
        #region Construction
        public FormConvertImages()
        {
            InitializeComponent();
        }
        
        #endregion

        #region Form
        private void initForm()
        {
            if (DiversityCollection.Forms.FormImportImagesSettings.Default.FolderPreview.Length > 0)
                this.textBoxFolderPreview.Text = DiversityCollection.Forms.FormImportImagesSettings.Default.FolderPreview;
            if (DiversityCollection.Forms.FormImportImagesSettings.Default.FolderWeb.Length > 0)
                this.textBoxFolderWeb.Text = DiversityCollection.Forms.FormImportImagesSettings.Default.FolderWeb;
        }

        private void buttonSetUrl_Click(object sender, System.EventArgs e)
        {
            try
            {
                string Folder = "";
                if (Folder.IndexOf(":") > 0) Folder = Folder.Substring(Folder.IndexOf(":") + 1, Folder.Length - Folder.IndexOf(":") - 1);
                Folder = Folder.Replace(@"\", "/");
            }
            catch { }
        }

        private void FormImportImages_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DiversityCollection.Forms.FormImportImagesSettings.Default.FolderArchive = this.textBoxFolderArchive.Text;
            //DiversityCollection.Forms.FormImportImagesSettings.Default.FolderOriginal = this.textBoxFolderOriginal.Text;
            DiversityCollection.Forms.FormImportImagesSettings.Default.FolderPreview = this.textBoxFolderPreview.Text;
            DiversityCollection.Forms.FormImportImagesSettings.Default.FolderWeb = this.textBoxFolderWeb.Text;
            //DiversityCollection.Forms.FormImportImagesSettings.Default.WebFolder = this.textBoxWebFolder.Text;
            DiversityCollection.Forms.FormImportImagesSettings.Default.Save();
        }
        
        #endregion

        #region Set folders
        private void buttonFolderArchive_Click(object sender, EventArgs e)
        {
            //this.SetFolder(this.textBoxFolderArchive);
        }

        private void buttonFolderWeb_Click(object sender, EventArgs e)
        {
            this.SetFolder(this.textBoxFolderWeb);
        }

        private void buttonFolderPreview_Click(object sender, EventArgs e)
        {
            this.SetFolder(this.textBoxFolderPreview);
        }

        private void SetFolder(System.Windows.Forms.TextBox textBox)
        {
            //this.folderBrowserDialogImport = new FolderBrowserDialog();
            //this.folderBrowserDialogImport.RootFolder = System.Environment.SpecialFolder.MyComputer;
            ////if (textBox.Text.Length > 0) this.folderBrowserDialogImport.RootFolder = Environment.SpecialFolder.Recent
            //this.folderBrowserDialogImport.ShowDialog();
            //if (this.folderBrowserDialogImport.SelectedPath.Length > 0)
            //    textBox.Text = this.folderBrowserDialogImport.SelectedPath;
        }

        #endregion

    }
}