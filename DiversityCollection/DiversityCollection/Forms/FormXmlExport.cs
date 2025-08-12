using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormXmlExport : Form
    {
        #region Parameter

        private DiversityCollection.Datasets.DataSetCollectionSpecimen _dsCollectionSpecimen;
        private DiversityCollection.Datasets.DataSetCollectionEventSeries _dsEventSeries;
        
        #endregion

        #region Construction

        public FormXmlExport(DiversityCollection.Datasets.DataSetCollectionSpecimen dataSetCollectionSpecimen
            , DiversityCollection.Datasets.DataSetCollectionEventSeries dataSetEventSeries)
        {
            InitializeComponent();
            this.initForm();
            this._dsCollectionSpecimen = dataSetCollectionSpecimen;
            this._dsEventSeries = dataSetEventSeries;
            // online manual
            this.helpProvider.HelpNamespace = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace();
        }
        
        #endregion

        #region Form

        private void initForm()
        {
            this.textBoxContentContact1.Text = DiversityCollection.Forms.FormXmlExportSettings.Default.ContentContact;
            this.textBoxTechnicalContact1.Text = DiversityCollection.Forms.FormXmlExportSettings.Default.TechnicalContact;
            this.textBoxIconURI.Text = DiversityCollection.Forms.FormXmlExportSettings.Default.IconURI;
            this.textBoxOtherProviders.Text = DiversityCollection.Forms.FormXmlExportSettings.Default.OtherProviders;
            this.textBoxScope.Text = DiversityCollection.Forms.FormXmlExportSettings.Default.Scope;
            //this.textBoxExportFile.Text = DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.HelpProviderNameSpace() + "\\ABCD_" + System.DateTime.Now.ToShortDateString() + ".XML";
            //this.textBoxOtherProviders.Text = DiversityCollection.Forms.FormXmlExportSettings.Default.Providers;

            string SQL = "SELECT [CollectionID], [CollectionParentID], [CollectionName] FROM [Collection] ORDER BY CollectionName";
            System.Data.DataTable dtCollection = new DataTable();
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtCollection);
            this.comboBoxCollection.DataSource = dtCollection;
            this.comboBoxCollection.DisplayMember = "CollectionName";
            this.comboBoxCollection.ValueMember = "CollectionID";
            this.userControlHierarchySelectorCollection.initHierarchy(dtCollection, "CollectionID", "CollectionParentID", "CollectionName", "CollectionName", "CollectionID", this.comboBoxCollection);
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (this.ParametersOK)
            {
                try
                {
                    //TechnicalContacts
                    System.Collections.Generic.List<string> tc = new List<string>();
                    if (this.textBoxTechnicalContact1.Text.Length > 0)
                        tc.Add(this.textBoxTechnicalContact1.Text);
                    if (this.textBoxTechnicalContact2.Text.Length > 0)
                        tc.Add(this.textBoxTechnicalContact2.Text);

                    //ContentContact
                    System.Collections.Generic.List<string> cc = new List<string>();
                    if (this.textBoxContentContact1.Text.Length > 0)
                        cc.Add(this.textBoxContentContact1.Text);
                    if (this.textBoxContentContact2.Text.Length > 0)
                        cc.Add(this.textBoxContentContact2.Text);

                    //OtherProviders
                    System.Collections.Generic.List<string> op = new List<string>();
                    if (this.textBoxOtherProviders.Text.Length > 0)
                        op.Add(this.textBoxOtherProviders.Text);

                    //Metadata
                    System.Collections.Generic.Dictionary<string, string> m = new Dictionary<string, string>();
                    if (this.textBoxIconURI.Text.Length > 0)
                        m.Add("IconURI", this.textBoxIconURI.Text);
                    if (this.textBoxScope.Text.Length > 0)
                        m.Add("Scope", this.textBoxScope.Text);
                    string Version = this.dateTimePickerDateIssued.Value.ToShortDateString();
                    if (this.textBoxVersion.Text.Length > 0)
                        Version = this.textBoxVersion.Text + ", " + Version;
                    m.Add("Version", Version);

                    DiversityCollection.XmlExportABCD X = new XmlExportABCD(this.textBoxExportFile.Text, int.Parse(this.comboBoxCollection.SelectedValue.ToString()), this._dsCollectionSpecimen, this._dsEventSeries);
                    this.textBoxExportFile.Text = X.createXmlFromDatasets(this.textBoxDatasetGUID.Text, tc, cc, op, m);
                    System.Windows.Forms.MessageBox.Show("ABCD-Export file: " + this.textBoxExportFile.Text);
                    System.Uri U = new Uri(this.textBoxExportFile.Text);
                    this.webBrowser.Url = U;

                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }
        
        private void FormXmlExport_FormClosing(object sender, FormClosingEventArgs e)
        {
            DiversityCollection.Forms.FormXmlExportSettings.Default.ContentContact = this.textBoxContentContact1.Text;
            DiversityCollection.Forms.FormXmlExportSettings.Default.TechnicalContact = this.textBoxTechnicalContact1.Text;
            DiversityCollection.Forms.FormXmlExportSettings.Default.IconURI = this.textBoxIconURI.Text;
            DiversityCollection.Forms.FormXmlExportSettings.Default.Scope = this.textBoxScope.Text;
            DiversityCollection.Forms.FormXmlExportSettings.Default.OtherProviders = this.textBoxOtherProviders.Text;
            //DiversityCollection.Forms.FormXmlExportSettings.Default.Providers = this.textBoxOtherProviders.Text;
            DiversityCollection.Forms.FormXmlExportSettings.Default.Save();
        }

        private void buttonCreateGuid_Click(object sender, EventArgs e)
        {
            this.textBoxDatasetGUID.Text = System.Guid.NewGuid().ToString();
        }
        
        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name, System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString());
        }

        #endregion

        #region Auxilliary
        private bool ParametersOK
        {
            get
            {
                bool OK = true;
                if (this.comboBoxCollection.SelectedValue == null)
                    OK = false;
                if (OK)
                {
                    if (this.comboBoxCollection.SelectedValue.Equals(System.DBNull.Value))
                        OK = false;
                }
                if (!OK)
                    System.Windows.Forms.MessageBox.Show("Please select a collection");
                return OK;
            }
        }
        
        #endregion

        #region Icon
        private void buttonOpenIconURI_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.textBoxIconURI.Text = f.URL;
                System.Uri U = new Uri(f.URL);
                this.webBrowserIconURI.Url = U;
            }
        }
        
        #endregion

        #region Export file
        private void buttonOpenExportFile_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.textBoxExportFile.Text);
            f.ShowDialog();
        }
        
        #endregion


    }
}