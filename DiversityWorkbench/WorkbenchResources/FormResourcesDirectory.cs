using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.WorkbenchResources
{
    public partial class FormResourcesDirectory : Form
    {

        #region Parameter

        private DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryType _Type;

        #endregion

        #region Construction
        public FormResourcesDirectory()
        {
            InitializeComponent();
            this.initForm();
        }

        #endregion

        #region Interface

        public DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryType Type()
        {
            return this._Type;
        }

        #endregion

        #region Form
        private void initForm()
        {
            System.Drawing.Font F = new Font(this.radioButtonHome.Font, FontStyle.Italic);
            switch (DiversityWorkbench.Settings.ResourcesDirectory)
            {
                case "Home":
                    this._Type = WorkbenchDirectory.WorkbenchDirectoryType.Home;
                    this.radioButtonHome.Font = F;
                    break;
                case "MyDocuments":
                    this._Type = WorkbenchDirectory.WorkbenchDirectoryType.MyDocuments;
                    this.radioButtonMyDocuments.Font = F;
                    break;
                default:
                    this._Type = WorkbenchDirectory.WorkbenchDirectoryType.UserDefined;
                    this.radioButtonUserDefined.Font = F;
                    this.textBoxFolder.Text = DiversityWorkbench.Settings.ResourcesDirectory;
                    break;
            }
            this.setControls();
        }

        private void setControls()
        {
            this.buttonSetFolder.Enabled = false;
            this.userControlDialogPanel.buttonOK.Enabled = true;
            switch (this._Type)
            {
                case WorkbenchDirectory.WorkbenchDirectoryType.Home:
                    this.radioButtonHome.Checked = true;
                    break;
                case WorkbenchDirectory.WorkbenchDirectoryType.MyDocuments:
                    this.radioButtonMyDocuments.Checked = true;
                    break;
                default:
                    this.radioButtonUserDefined.Checked = true;
                    this.buttonSetFolder.Enabled = true;
                    if (this.textBoxFolder.Text.Length == 0)
                        this.userControlDialogPanel.buttonOK.Enabled = false;
                    else
                        this.userControlDialogPanel.buttonOK.Enabled = true;
                    break;
            }
            this.initCopyOptions();
        }

        private void buttonSetFolder_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog = new FolderBrowserDialog();
            this.folderBrowserDialog.ShowDialog();
            if (this.folderBrowserDialog.SelectedPath.Length > 0)
                this.textBoxFolder.Text = this.folderBrowserDialog.SelectedPath;
        }

        public string UserDefinedDirectory() { return this.textBoxFolder.Text; }

        private void radioButtonHome_Click(object sender, EventArgs e)
        {
            if (this._Type != WorkbenchDirectory.WorkbenchDirectoryType.Home)
            {
                this._Type = WorkbenchDirectory.WorkbenchDirectoryType.Home;
                this.setControls();
            }
        }

        private void radioButtonMyDocuments_Click(object sender, EventArgs e)
        {
            if (this._Type != WorkbenchDirectory.WorkbenchDirectoryType.MyDocuments)
            {
                this._Type = WorkbenchDirectory.WorkbenchDirectoryType.MyDocuments;
                this.setControls();
            }
        }

        private void radioButtonUserDefined_Click(object sender, EventArgs e)
        {
            if (this._Type != WorkbenchDirectory.WorkbenchDirectoryType.UserDefined)
            {
                this._Type = WorkbenchDirectory.WorkbenchDirectoryType.UserDefined;
                this.setControls();
            }
        }

        private void textBoxFolder_TextChanged(object sender, EventArgs e)
        {
            if (this.textBoxFolder.Text.Length > 0)
                this.userControlDialogPanel.buttonOK.Enabled = true;
            else
                this.userControlDialogPanel.buttonOK.Enabled = false;
        }

        private void ButtonOpenDirectory_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.WorkbenchDirectoryModule());
        }

        #region Options

        private void initCopyOptions()
        {
            int i = 0;
            int p = 0;
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HowToCopyAppToUserDirectoryOptions)
            {
                this.comboBoxCopyOption.Items.Add(KV.Key);
                if(KV.Value != null && KV.Value == DiversityWorkbench.WorkbenchSettings.Default.HowToCopyAppToUserDirectory ) { p = i; }
                i++;
            }
            this.comboBoxCopyOption.SelectedIndex = p;
            this.setCopyOptionImage();
        }

        private void comboBoxCopyOption_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HowToCopyAppToUserDirectoryOptions.ContainsKey(this.comboBoxCopyOption.SelectedItem.ToString()))
            {
                DiversityWorkbench.WorkbenchSettings.Default.HowToCopyAppToUserDirectory = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HowToCopyAppToUserDirectoryOptions[this.comboBoxCopyOption.SelectedItem.ToString()];
                DiversityWorkbench.WorkbenchSettings.Default.Save();
            }
            this.setCopyOptionImage();
        }

        private void setCopyOptionImage()
        {
            switch (DiversityWorkbench.WorkbenchSettings.Default.HowToCopyAppToUserDirectory)
            {
                case "Copy":
                    this.buttonCopyOption.Image = DiversityWorkbench.Properties.Resources.Copy2;
                    break;
                case "Missing":
                    this.buttonCopyOption.Image = DiversityWorkbench.Properties.Resources.Lupe;
                    break;
                case "None":
                    this.buttonCopyOption.Image = DiversityWorkbench.Properties.Resources.Minus;
                    break;
            }

        }

        #endregion

        #endregion

    }
}
