using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormExternalDatasource : Form
    {
        #region Parameter
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapter;
        
        #endregion

        #region Construction

        public FormExternalDatasource()
        {
            InitializeComponent();
            this.dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            string SQL = "SELECT ExternalDatasourceID, ExternalDatasourceName, ExternalDatasourceVersion, Rights, ExternalDatasourceAuthors, ExternalDatasourceURI, " +
                "ExternalDatasourceInstitution, InternalNotes, ExternalAttribute_NameID, PreferredSequence, Disabled " +
                "FROM CollectionExternalDatasource";
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapter, this.dataSetExternalDatasource.CollectionExternalDatasource, SQL, DiversityWorkbench.Settings.ConnectionString);
        }
        
        #endregion

        #region Form
        private void FormExternalDatasource_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (this.dataSetExternalDatasource.HasChanges())
                {
                    this._SqlDataAdapter.Update(this.dataSetExternalDatasource.CollectionExternalDatasource);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Help
        public void setHelpProviderNameSpace(string HelpNameSpace, string Keyword)
        {
            try
            {
                this.helpProvider.HelpNamespace = HelpNameSpace;
                this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
                this.helpProvider.SetHelpKeyword(this, Keyword);
            }
            catch { }
        }

        #endregion


    }
}
