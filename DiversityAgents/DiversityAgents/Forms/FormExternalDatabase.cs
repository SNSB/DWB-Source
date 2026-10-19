using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityAgents.Forms
{
    public partial class FormExternalDatabase : Form
    {

        Microsoft.Data.SqlClient.SqlDataAdapter _SqlDAagentExternalDatabase;

        public FormExternalDatabase()
        {
            InitializeComponent();
            this.dataSetExternalDatasource.AgentExternalDatabase.Clear();
            //this._SqlDAagentExternalDatabase = new Microsoft.Data.SqlClient.SqlDataAdapter("SELECT ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, ExternalDatabaseURI, ExternalDatabaseInstitution, " +
            //        "InternalNotes, ExternalAttribute_AgentID, PreferredSequence, Disabled " +
            //        "FROM AgentExternalDatabase", DiversityWorkbench.Settings.ConnectionString);
            string SQL = "SELECT ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, ExternalDatabaseURI, ExternalDatabaseInstitution, " +
                    "InternalNotes, ExternalAttribute_AgentID, PreferredSequence, Disabled FROM AgentExternalDatabase";
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDAagentExternalDatabase, this.dataSetExternalDatasource.AgentExternalDatabase, SQL, DiversityWorkbench.Settings.ConnectionString);
            //this._SqlDAagentExternalDatabase.Fill(this.dataSetExternalDatasource.AgentExternalDatabase);

            ////if (this.agentExternalDatabaseTableAdapter.Adapter.SelectCommand == null)
            ////{
            //    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            //    this.agentExternalDatabaseTableAdapter.Adapter.SelectCommand = new Microsoft.Data.SqlClient.SqlCommand("SELECT ExternalDatabaseID, ExternalDatabaseName, ExternalDatabaseVersion, Rights, ExternalDatabaseAuthors, ExternalDatabaseURI, ExternalDatabaseInstitution, " +
            //        "InternalNotes, ExternalAttribute_AgentID, PreferredSequence, Disabled " +
            //        "FROM AgentExternalDatabase", con);
            //    this.agentExternalDatabaseTableAdapter.Adapter.SelectCommand.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            ////}
            ////this.agentExternalDatabaseTableAdapter.Adapter.SelectCommand.Connection.ConnectionString = DiversityWorkbench.Settings.ConnectionString;
            //this.agentExternalDatabaseTableAdapter.Fill(this.dataSetExternalDatasource.AgentExternalDatabase);
        }

        private void FormExternalDatabase_Load(object sender, EventArgs e)
        {
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetExternalDatasource.AgentExternalDatabase". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.agentExternalDatabaseTableAdapter.Fill(this.dataSetExternalDatasource.AgentExternalDatabase);
        }

        private void toolStripButtonSourceAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new DiversityWorkbench.Forms.FormGetString("New datasource", "Please enter the name of the new datasource", "");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityAgents.Datasets.DataSetExternalDatasource.AgentExternalDatabaseRow R = this.dataSetExternalDatasource.AgentExternalDatabase.NewAgentExternalDatabaseRow();
                R.ExternalDatabaseName = f.String;
                int ID;
                string SQL = "SELECT MAX(ExternalDatabaseID) + 1 AS ID FROM AgentExternalDatabase";
                if (!int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out ID))
                    ID = 1;
                R.ExternalDatabaseID = ID;
                this.dataSetExternalDatasource.AgentExternalDatabase.Rows.Add(R);
            }
        }

        private void toolStripButtonSourceDelete_Click(object sender, EventArgs e)
        {
            this.agentExternalDatabaseBindingSource.RemoveCurrent();
        }

        private void FormExternalDatabase_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.agentExternalDatabaseBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
                this._SqlDAagentExternalDatabase.Update(this.dataSetExternalDatasource.AgentExternalDatabase);
            }
            catch (System.Exception ex) { }
            //this.agentExternalDatabaseTableAdapter.Update(this.dataSetExternalDatasource.AgentExternalDatabase);
        }

        private void FormExternalDatabase_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            Help.ShowHelp(this, "https://www.diversityworkbench.de/manual/dwb/diversityagents/editingdata/externaldata/index.html");
        }
    }
}
