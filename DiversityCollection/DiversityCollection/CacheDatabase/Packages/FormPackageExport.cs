using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase.Packages
{
    public partial class FormPackageExport : Form
    {

        #region Parameter

        private DiversityCollection.CacheDatabase.Package _Package;
        private System.Data.DataTable _DtViews;
        private string _Schema;
        
        #endregion

        #region Construction

        public FormPackageExport(DiversityCollection.CacheDatabase.Package Package, string Schema)
        {
            InitializeComponent();
            this._Package = Package;
            this._Schema = Schema;
            this.initForm();
        }
        
        #endregion

        #region Form and events

        private void initForm()
        {
            this.labelHeader.Text = "Export the data of package " + this._Package.Name + " to " + ExportFolder();
            this.textBoxSQLiteDatabase.Text = this._Package.Name;
            this.comboBoxSchema.Items.Add(this._Schema);
            this.comboBoxSchema.Items.Add("public");
            this.comboBoxSchema.SelectedIndex = 0;
            this.GetPackageViews();
        }

        #region File and folder

        private string _ExportFolder = "";
        private string ExportFolder()
        {
            if (this._ExportFolder.Length == 0)
                this._ExportFolder = Folder.CacheDB(Folder.CacheDBFolder.Export) + this._Schema.Replace("Project_", "") + "\\" + this._Package.Name;
            return this._ExportFolder;
        }

        private void buttonOpenDirectory_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.InitialDirectory.Length > 0)
            {
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    DiversityWorkbench.Forms.FormWebBrowser f = new DiversityWorkbench.Forms.FormWebBrowser(this.openFileDialog.FileName);
                    f.ShowDialog();
                }
            }
        }

        #endregion

        private void comboBoxSchema_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this._Schema = this.comboBoxSchema.SelectedItem.ToString();
            this.GetPackageViews();
        }

        private void GetPackageViews()
        {
            try
            {
                string SQL = "select table_name from information_schema.tables P " +
                    " where (P.table_type = 'VIEW'  or P.table_type = 'BASE TABLE') " +
                    " and P.table_schema = '" + this._Schema + "'" + 
                    " and table_name LIKE '" + this._Package.Name + "_%' " +
                    " and table_name NOT LIKE '" + this._Package.Name + "\\_\\_%'" +
                    " order by table_name";
                Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString()); // .Postgres.PostgresConnection().ConnectionString);
                this._DtViews = new DataTable();
                ad.Fill(this._DtViews);
                this.listBoxViews.DataSource = this._DtViews;
                this.listBoxViews.DisplayMember = "table_name";
                this.listBoxViews.ValueMember = "table_name";
            }
            catch (System.Exception ex)
            { }

        }

        private void buttonExportXML_Click(object sender, EventArgs e)
        {
            string Message = "";
            string Errors = "";
            int i = 0;
            try
            {
                System.IO.DirectoryInfo D = new System.IO.DirectoryInfo(ExportFolder());
                if (!D.Exists) D.Create();
                Message += "Data exported to " + D.FullName;
                this.openFileDialog.InitialDirectory = D.FullName;
                foreach (System.Data.DataRow R in this._DtViews.Rows)
                {
                    try
                    {
                        System.Data.DataTable dtValues = new DataTable(R[0].ToString());
                        string SQL = "SELECT * FROM \"" + this._Schema + "\".\"" + R[0].ToString() + "\"";
                        Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                        ad.Fill(dtValues);
                        string FileName = D.FullName + "\\" + R[0].ToString() + ".xml";
                        if (dtValues.Rows.Count > 0)
                        {
                            dtValues.WriteXml(FileName, System.Data.XmlWriteMode.WriteSchema);
                            i++;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Errors += ex.Message + "\r\n";
                    }
                }
            }
            catch (System.Exception ex)
            {
                Errors += ex.Message + "\r\n";
            }
            if (Errors.Length > 0)
                Message += "\r\nErrors: " + Errors;
            else if (i == 0)
                Message = "No data for export detected" ;
            System.Windows.Forms.MessageBox.Show(Message);
        }
        
        private void listBoxViews_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxViews.SelectedItem;
                string SQL = "";
                System.Data.DataTable dt = new DataTable();
                SQL = "select * from \"" + this._Schema + "\".\"" + R[0].ToString() + "\"";
                Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());// .Postgres.PostgresConnection().ConnectionString);
                ad.Fill(dt);
                this.dataGridViewView.DataSource = dt;
            }
            catch (System.Exception ex)
            {
            }
        }

        #region SQLite

        private void buttonExportSqlite_Click(object sender, EventArgs e)
        {
            string Message = "";
            string Errors = "";
            string SQL = "";
            try
            {
                System.IO.DirectoryInfo DI = new System.IO.DirectoryInfo(Folder.Export());
                if (this.textBoxSQLiteDatabase.Text.Length == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please enter a name for the database");
                    return;
                }
                System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> Tables = new Dictionary<string, Dictionary<string, string>>();
                foreach (System.Data.DataRow R in this._DtViews.Rows)
                {
                    System.Collections.Generic.Dictionary<string, string> TableColumns = new Dictionary<string, string>();
                    SQL = "SELECT C.\"column_name\", " +
                        "case when \"character_maximum_length\" IS NULL THEN \"data_type\" ELSE concat(\"data_type\", ' (', \"character_maximum_length\", ')') end " +
                        "FROM information_schema.columns C " +
                        "WHERE C.\"table_schema\" = '" + this._Schema + "' " +
                        "AND C.\"table_name\" = '" + R[0].ToString() + "'";
                    Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString()); // .Postgres.PostgresConnection().ConnectionString);
                    System.Data.DataTable dtColumns = new DataTable();
                    ad.Fill(dtColumns);
                    foreach (System.Data.DataRow RC in dtColumns.Rows)
                        TableColumns.Add(RC[0].ToString(), RC[1].ToString());
                    Tables.Add(R[0].ToString(), TableColumns);
                }
                Message = "Data exported to\r\n" + DI.FullName + "\\" + this.SQLiteDB.DatabaseName();
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<string, string>> KV in Tables)
                {
                    if (!this.SQLiteDB.AddTable(KV.Key, KV.Value))
                        continue;
                    try
                    {
                        System.Data.DataTable dtValues = new DataTable();
                        SQL = "SELECT * FROM \"" + this._Schema + "\".\"" + KV.Key + "\"";
                        Npgsql.NpgsqlDataAdapter ad = new Npgsql.NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString()); // .Postgres.PostgresConnection().ConnectionString);
                        ad.Fill(dtValues);
                        this.progressBar.Value = 0;
                        this.progressBar.Maximum = dtValues.Rows.Count;
                        this.labelSQLiteTransferStep.Text = "Transfer " + KV.Key;
                        Application.DoEvents();
                        int i = 0;
                        foreach (System.Data.DataRow R in dtValues.Rows)
                        {
                            System.Collections.Generic.Dictionary<string, string> ColumnValues = new Dictionary<string, string>();
                            foreach (System.Data.DataColumn C in dtValues.Columns)
                            {
                                ColumnValues.Add(C.ColumnName, R[C.ColumnName].ToString());
                            }
                            this._SQLiteDB.InsertData(KV.Key, ColumnValues);
                            i++;
                            if (i <= this.progressBar.Maximum)
                                this.progressBar.Value = i;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Errors += ex.Message + "\r\n" + SQL + "\r\n";
                    }
                    this.progressBar.Value = this.progressBar.Maximum;
                }
                if (Errors.Length > 0)
                    Message += "\r\nErrors: " + Errors;
                System.Windows.Forms.MessageBox.Show(Message);
                this.labelSQLiteTransferStep.Text = "";

            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private DiversityWorkbench.SqlLite.Database _SQLiteDB;

        public DiversityWorkbench.SqlLite.Database SQLiteDB
        {
            get
            {
                if (this._SQLiteDB == null)
                {
                    string Path = Folder.Export() + this.textBoxSQLiteDatabase.Text + ".sqlite";
                    this._SQLiteDB = new DiversityWorkbench.SqlLite.Database(Path);
                }
                return _SQLiteDB;
            }
            //set { _SQLiteDB = value; }
        }

        #endregion

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
