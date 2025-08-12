using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace DiversityCollection.CacheDatabase
{
    public partial class FormCacheDB : Form, InterfaceCacheDB, InterfacePackage
    {

        #region Parameter

        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterCacheDatabase;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterProjectProxy;
        private System.Data.DataTable _DtMaterialNotTransferred;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterLocalisation;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTaxonomicGroup;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterKingdoms;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTaxonSynonymySource;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterMaterialCategory;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEnumKingdom;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEnumPreparationType;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEnumRecordBasis;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEnumTaxonomicGroup;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTaxonomicGroupInProject;
        private System.Data.SqlClient.SqlDataAdapter _SqlDataAdapterLocalisationSystem;
        
        #endregion

        #region Construction
        
        public FormCacheDB()
        {
            try
            {
                InitializeComponent();
                ///TODO: nach Einbau der Quelle Seite wieder zugaenglich machen
                this.tabControlDatabase.TabPages.Remove(this.tabPageSettings);
                /// Obsolete objects
                this.tabControlDatabase.TabPages.Remove(this.tabPageKingdom);
                this.tabControlDatabase.TabPages.Remove(this.tabPageMaterialCategory);
                ///TODO: nach Einbau der Quelle
                this.tabControlDatabase.TabPages.Remove(this.tabPageAnonymCollectors);


                if (this.initDatabase())
                {
                    this.initBioCASE();
                    this.initPostgres();
                    this.initPostgresAdministration();
                    this.helpProvider.HelpNamespace = System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm";
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Failed to initialize cache database");
                    this.Close();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }
        
        #endregion

        #region Database
        
        private bool initDatabase()
        {
            bool OK = true;
            try
            {
                string SQL = "SELECT DatabaseName, Server, Port, Version /*, ProjectsDatabaseName*/ FROM CacheDatabase";
                this._SqlDataAdapterCacheDatabase = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                this._SqlDataAdapterCacheDatabase.Fill(this.dataSetCacheDB_1.CacheDatabase);
                if (this.dataSetCacheDB_1.CacheDatabase.Rows.Count == 0)
                {
                    if (System.Windows.Forms.MessageBox.Show("So far no cache database has been defined.\r\nDo you want to create a cache database?", "Create cache database?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    {
                        string DBname = DiversityWorkbench.Settings.DatabaseName;
                        if (DBname.IndexOf("_") > -1)
                            DBname = DBname.Replace("_", "Cache_");
                        else DBname += "Cache";
                        DiversityWorkbench.FormGetString f = new DiversityWorkbench.FormGetString("Please enter the name of the new cache database", "Name of cache database", DBname);
                        f.ShowDialog();
                        if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            System.Data.DataTable dtFiles = new DataTable();
                            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter("exec sp_helpfile", DiversityWorkbench.Settings.ConnectionString);
                            ad.Fill(dtFiles);
                            string DataFile = dtFiles.Rows[0]["filename"].ToString();
                            if (DataFile.IndexOf("\\" + DiversityWorkbench.Settings.DatabaseName + ".") > -1)
                                DataFile = DataFile.Replace("\\" + DiversityWorkbench.Settings.DatabaseName + ".", "\\" + DBname + ".");
                            else
                            {
                                DataFile = DataFile.Substring(0, DataFile.LastIndexOf("\\")) + "\\" + DBname + ".mdf";
                            }
                            string LogFile = DataFile.Substring(0, DataFile.IndexOf(".")) + "_log.ldf";
                            SQL = "CREATE DATABASE [" + DBname + "] " +
                                "CONTAINMENT = NONE " +
                                "ON  PRIMARY  " +
                                "( NAME = N'" + DBname + "', FILENAME = N'" + DataFile + "' , SIZE = 5032KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%) " +
                                "LOG ON  " +
                                "( NAME = N'" + DBname + "_Log', FILENAME = N'" + LogFile + "' , SIZE = 504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%) ";
                            try
                            {
                                string Message = "";
                                if (DiversityWorkbench.FormFunctions.SqlExecuteNonQuery(SQL, ref Message))
                                {
                                    SQL = "INSERT INTO CacheDatabase (Server, DatabaseName, Port, Version) "
                                    + "VALUES ('" + DiversityWorkbench.Settings.DatabaseServer + "', '" + DBname + "', " + DiversityWorkbench.Settings.DatabasePort.ToString() + ", '00.00.00')";
                                    DiversityWorkbench.FormFunctions.SqlExecuteNonQuery(SQL);
                                    this._SqlDataAdapterCacheDatabase.Fill(this.dataSetCacheDB_1.CacheDatabase);
                                    this.setDatabase();
                                }
                                else
                                {
                                    System.Windows.Forms.MessageBox.Show("Creation of cache database failed: " + Message);
                                    OK = false;
                                }
                            }
                            catch (System.Exception ex)
                            {
                                OK = false;
                            }
                        }
                        else
                            this.Close();
                    }
                    else
                        this.Close();
                }
                else if (this.dataSetCacheDB_1.CacheDatabase.Rows.Count == 1)
                {
                    this.setDatabase();
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("No cache databases available");
                OK = false;
                this.Close();
            }
            return OK;
        }

        private void setDatabase()
        {
            try
            {
                if (this.dataSetCacheDB_1.CacheDatabase.Rows.Count == 1)
                {
                    System.Data.DataRow R = this.dataSetCacheDB_1.CacheDatabase.Rows[0];
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseServer = R["Server"].ToString();
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseName = R["DatabaseName"].ToString();
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort = int.Parse(R["Port"].ToString());
                    DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion = R["Version"].ToString();

                    string SQL = "SELECT dbo.Version()";
                    this.textBoxCurrentDatabaseVersion.Text = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    if (this.textBoxCurrentDatabaseVersion.Text.Length == 0)
                    {
                        this.textBoxCurrentDatabaseVersion.Text = "00.00.00";
                        SQL = "CREATE FUNCTION [dbo].[Version] () RETURNS nvarchar(8) AS BEGIN RETURN '00.00.00' END";
                        DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                    }
                    this.textBoxDatabaseName.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                    this.textBoxPort.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseServerPort.ToString();
                    this.textBoxServer.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseServer;

                    SQL = "SELECT dbo.ProjectsDatabase()";
                    string ProjectsDB = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                    this.textBoxProjectsDatabase.Text = ProjectsDB;
                    if (this.textBoxProjectsDatabase.Text.Length == 0)
                        this.textBoxProjectsDatabase.Text = "Not defined";
                }

                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    this.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName + " on " + DiversityCollection.CacheDatabase.CacheDB.DatabaseServer;

                    if (this._FormState == FormState.Administration || this._FormState == FormState.Both)
                        this.tabPageAdminCacheDB.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;

                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    try
                    {
                        this._SqlDataAdapterEnumPreparationType = new System.Data.SqlClient.SqlDataAdapter("SELECT PreparationType FROM EnumPreparationType ORDER BY PreparationType", con);
                        this._SqlDataAdapterEnumPreparationType.Fill(this.dataSetCacheDB_1.EnumPreparationType);

                        this._SqlDataAdapterEnumRecordBasis = new System.Data.SqlClient.SqlDataAdapter("SELECT RecordBasis FROM EnumRecordBasis ORDER BY RecordBasis", con);
                        this._SqlDataAdapterEnumRecordBasis.Fill(this.dataSetCacheDB_1.EnumRecordBasis);
                        this._SqlDataAdapterMaterialCategory = new System.Data.SqlClient.SqlDataAdapter("SELECT Code, DisplayText, RecordBasis, PreparationType, CategoryOrder FROM EnumMaterialCategory ORDER BY DisplayText", con);
                        this._SqlDataAdapterMaterialCategory.Fill(this.dataSetCacheDB_1.EnumMaterialCategory);

                        this.dataGridViewMaterialCategory.Columns[0].Visible = false;
                        foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewMaterialCategory.Columns)
                            C.SortMode = DataGridViewColumnSortMode.NotSortable;
                        this.setMaterialCategoryNotTransferredList();

                        this._SqlDataAdapterEnumKingdom = new System.Data.SqlClient.SqlDataAdapter("SELECT Kingdom FROM EnumKingdom ORDER BY Kingdom", con);
                        this._SqlDataAdapterEnumKingdom.Fill(this.dataSetCacheDB_1.EnumKingdom);

                        this._SqlDataAdapterEnumTaxonomicGroup = new System.Data.SqlClient.SqlDataAdapter("SELECT Code, DisplayText, Kingdom FROM EnumTaxonomicGroup ORDER BY DisplayText", con);
                        this._SqlDataAdapterEnumTaxonomicGroup.Fill(this.dataSetCacheDB_1.EnumTaxonomicGroup);
                        System.Data.SqlClient.SqlCommandBuilder builderEnumTaxonomicGroup = new System.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterEnumTaxonomicGroup);

                        this._SqlDataAdapterTaxonomicGroupInProject = new System.Data.SqlClient.SqlDataAdapter("SELECT TaxonomicGroup, ProjectID FROM TaxonomicGroupInProject ORDER BY TaxonomicGroup", con);
                        this._SqlDataAdapterTaxonomicGroupInProject.Fill(this.dataSetCacheDB_1.TaxonomicGroupInProject);
                        System.Data.SqlClient.SqlCommandBuilder builderEnumTaxonomicGroupInProject = new System.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterTaxonomicGroupInProject);

                        this._SqlDataAdapterLocalisationSystem = new System.Data.SqlClient.SqlDataAdapter("SELECT LocalisationSystemID, DisplayText, Sequence FROM EnumLocalisationSystem ORDER BY Sequence", con);
                        this._SqlDataAdapterLocalisationSystem.Fill(this.dataSetCacheDB_1.EnumLocalisationSystem);
                        System.Data.SqlClient.SqlCommandBuilder builderEnumLocalisationSystem = new System.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterLocalisationSystem);

                    }
                    catch (System.Exception ex)
                    {
                    }

                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.setMessages();
            if (!this.setV1ProjectLists())
                this.tabControlDatabase.TabPages.Remove(this.tabPageProjects);
            else
            {
                if (!this.tabControlDatabase.TabPages.Contains(this.tabPageProjects))
                    this.tabControlDatabase.TabPages.Add(this.tabPageProjects);
            }
            if (!this.SetLocalisationList())
                this.tabControlDatabase.TabPages.Remove(this.tabPageLocalisation);
            else
            {
                if (!this.tabControlDatabase.TabPages.Contains(this.tabPageLocalisation))
                    this.tabControlDatabase.TabPages.Add(this.tabPageLocalisation);
            }
            if (!this.initTaxonSources())
                this.tabControlDatabase.TabPages.Remove(this.tabPageTaxonSynonymy);
            else
            {
                if (!this.tabControlDatabase.TabPages.Contains(this.tabPageTaxonSynonymy))
                    this.tabControlDatabase.TabPages.Add(this.tabPageTaxonSynonymy);
            }
            this.setUpdateControls();
            this.initOverview();
            //this.SetKingdoms();
            //this.SetTaxonomicGroups();
        }

        #endregion

        #region Form

        private void setMessages()
        {
            string SQL = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
            {
                this.UpdateCheckForDatabase();
                try
                {
                    SQL = "SELECT COUNT(*) AS Anzahl FROM CollectionSpecimenCache";
                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    con.Open();
                    System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                    this.labelV1CurrentSpecimenNumber.Text = "Current specimens: " + C.ExecuteScalar().ToString();
                    C.CommandText = "SELECT COUNT(*) AS Anzahl FROM TaxonSynonymy";
                    this.labelV1CurrentTaxa.Text = "Current taxa: " + C.ExecuteScalar().ToString();
                    C.CommandText = "SELECT convert(varchar(50), max([LogInsertedWhen]), 102) AS LastTransfer FROM [dbo].[CollectionSpecimenCache]";
                    this.labelV1LastSpecimenTransfer.Text = "Last transfer: " + C.ExecuteScalar().ToString();
                    C.CommandText = "SELECT convert(varchar(50), max([LogInsertedWhen]), 102) AS LastTransfer FROM [dbo].[TaxonSynonymy]";
                    this.labelV1LastTaxonTransfer.Text = "Last transfer: " + C.ExecuteScalar().ToString();
                    con.Close();
                    con.Dispose();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }
        
        private void FormCacheDB_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.projectPublishedBindingSource.Current != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.projectPublishedBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
            }
            if (this._SqlDataAdapterProjectProxy != null)
            {
                this._SqlDataAdapterProjectProxy.Update(this.dataSetCacheDB_1.ProjectPublished);
            }
        }

        private void FormCacheDB_Load(object sender, EventArgs e)
        {
            this.buttonTransferCountryIcoCode.Visible = false;
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionGazetteer() == null)
                this.labelWarningNoGazetteer.Visible = true;

            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB_1.EnumLocalisationSystem". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.enumLocalisationSystemTableAdapter.Fill(this.dataSetCacheDB_1.EnumLocalisationSystem);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB_1.TaxonomicGroupInProject". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.taxonomicGroupInProjectTableAdapter.Fill(this.dataSetCacheDB_1.TaxonomicGroupInProject);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB_1.EnumKingdom". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.enumKingdomTableAdapter.Fill(this.dataSetCacheDB_1.EnumKingdom);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB_1.EnumTaxonomicGroup". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.enumTaxonomicGroupTableAdapter.Fill(this.dataSetCacheDB_1.EnumTaxonomicGroup);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB_1.EnumPreparationType". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.enumPreparationTypeTableAdapter.Fill(this.dataSetCacheDB_1.EnumPreparationType);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB_1.EnumRecordBasis". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.enumRecordBasisTableAdapter.Fill(this.dataSetCacheDB_1.EnumRecordBasis);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB_1.EnumMaterialCategory". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.enumMaterialCategoryTableAdapter.Fill(this.dataSetCacheDB_1.EnumMaterialCategory);
            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB_1.ProjectPublished". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectPublishedTableAdapter.Fill(this.dataSetCacheDB_1.ProjectPublished);
            // TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetCacheDB.ProjectProxy". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.projectProxyTableAdapter.Fill(this.dataSetCacheDB_1.ProjectProxy);

        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                "",
                "");
        }

        public enum FormState {Transfer, Administration, Both}

        private FormState _FormState = FormState.Both;

        public void SetFormState(FormState State)
        {
            switch (State)
            {
                case FormState.Administration:
                    this.tabControlMain.TabPages.Remove(this.tabPageTransfer);
                    this._FormState = FormState.Administration;
                    break;
                case FormState.Transfer:
                    this.tabControlMain.TabPages.Remove(this.tabPageAdminCacheDB);
                    this.tabControlMain.TabPages.Remove(this.tabPageAdminPostgres);
                    this.CheckLastTransfer();
                    this.initTransferSteps();
                    this._FormState = FormState.Transfer;
                    break;
            }
        }

        #endregion

        #region Transfer

        #region Transfer SQL Server

        private System.Collections.Generic.List<DiversityCollection.CacheDatabase.InterfaceTransferStep> _TransferSteps;

        private void CheckLastTransfer()
        {
            string SQL = "SELECT MAX(CONVERT(nvarchar(19),[LogInsertedWhen],  120)) FROM [dbo].[CollectionSpecimenCache]";
            string LastTrans = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            this.labelLastTransfer.Text = "Last transfer:\r\n" + LastTrans; ;
        }

        private void initTransferSteps()
        {
            this._TransferSteps = new List<InterfaceTransferStep>();

            DiversityCollection.CacheDatabase.UserControlTransferStep UM = new UserControlTransferStep("Metadata", "exec procTransferMetadata", "SELECT * FROM [dbo].[ABCD_Metadata]", this.imageListTransferSteps.Images[10]);
            this.panelTransferSteps.Controls.Add(UM);
            UM.Dock = DockStyle.Top;
            UM.BringToFront();
            this._TransferSteps.Add(UM);

            DiversityCollection.CacheDatabase.UserControlTransferStep US = new UserControlTransferStep("Collection specimen", "exec procTransferCollectionSpecimenCache", "SELECT * FROM [dbo].[CollectionSpecimenCache]", this.imageListTransferSteps.Images[12]);
            this.panelTransferSteps.Controls.Add(US);
            US.Dock = DockStyle.Top;
            US.BringToFront();
            this._TransferSteps.Add(US);

            DiversityCollection.CacheDatabase.UserControlTransferStep UPr = new UserControlTransferStep("Project", "exec procTransferCollectionProject", "SELECT * FROM [dbo].[CollectionProjectCache]", this.imageListTransferSteps.Images[6]);
            this.panelTransferSteps.Controls.Add(UPr);
            UPr.Dock = DockStyle.Top;
            UPr.BringToFront();
            this._TransferSteps.Add(UPr);

            DiversityCollection.CacheDatabase.UserControlTransferStep UC = new UserControlTransferStep("Coordinates", "exec procTransferCoordinates", "SELECT * FROM [dbo].[CollectionSpecimenCache] where not Latitude is null", this.imageListTransferSteps.Images[1]);
            this.panelTransferSteps.Controls.Add(UC);
            UC.Dock = DockStyle.Top;
            UC.BringToFront();
            this._TransferSteps.Add(UC);

            DiversityCollection.CacheDatabase.UserControlTransferStep UI = new UserControlTransferStep("Country ISO codes", this, "SELECT * FROM [dbo].[CollectionSpecimenCache] where not CountryIsoCode is null", this.imageListTransferSteps.Images[2]);
            this.panelTransferSteps.Controls.Add(UI);
            UI.Dock = DockStyle.Top;
            UI.BringToFront();
            this._TransferSteps.Add(UI);

            DiversityCollection.CacheDatabase.UserControlTransferStep UR = new UserControlTransferStep("Record URIs", "exec procTransferSpecimenRecordURI", "SELECT * FROM [dbo].[CollectionSpecimenCache] where not RecordURI is null", this.imageListTransferSteps.Images[5]);
            this.panelTransferSteps.Controls.Add(UR);
            UR.Dock = DockStyle.Top;
            UR.BringToFront();
            this._TransferSteps.Add(UR);

            DiversityCollection.CacheDatabase.UserControlTransferStep UU = new UserControlTransferStep("Organisms", "exec procTransferIdentificationUnitPartCache", "SELECT * FROM [dbo].[IdentificationUnitPartCache]", this.imageListTransferSteps.Images[14]);
            this.panelTransferSteps.Controls.Add(UU);
            UU.Dock = DockStyle.Top;
            UU.BringToFront();
            this._TransferSteps.Add(UU);

            DiversityCollection.CacheDatabase.UserControlTransferStep Uana = new UserControlTransferStep("Analysis", "exec procTransferAnalysis", "SELECT * FROM [dbo].[AnalysisCache]", this.imageListTransferSteps.Images[11]);
            this.panelTransferSteps.Controls.Add(Uana);
            Uana.Dock = DockStyle.Top;
            Uana.BringToFront();
            this._TransferSteps.Add(Uana);

            DiversityCollection.CacheDatabase.UserControlTransferStep UUana = new UserControlTransferStep("Analysis of organisms", "exec procTransferIdentificationUnitAnalysisCache", "SELECT * FROM [dbo].[IdentificationUnitAnalysisCache]", this.imageListTransferSteps.Images[11]);
            this.panelTransferSteps.Controls.Add(UUana);
            UUana.Dock = DockStyle.Top;
            UUana.BringToFront();
            this._TransferSteps.Add(UUana);

            DiversityCollection.CacheDatabase.UserControlTransferStep UA = new UserControlTransferStep("Collectors", "exec procTransferCollectionAgentCache", "SELECT * FROM [dbo].[CollectionAgentCache]", this.imageListTransferSteps.Images[7]);
            this.panelTransferSteps.Controls.Add(UA);
            UA.Dock = DockStyle.Top;
            UA.BringToFront();
            this._TransferSteps.Add(UA);

            DiversityCollection.CacheDatabase.UserControlTransferStep UP = new UserControlTransferStep("Parts", "exec procTransferCollectionSpecimenPartCache", "SELECT * FROM [dbo].[CollectionSpecimenPartCache]", this.imageListTransferSteps.Images[15]);
            this.panelTransferSteps.Controls.Add(UP);
            UP.Dock = DockStyle.Top;
            UP.BringToFront();
            this._TransferSteps.Add(UP);

            DiversityCollection.CacheDatabase.UserControlTransferStep UIM = new UserControlTransferStep("Images and media", "exec procTransferCollectionSpecimenImageCache", "SELECT * FROM [dbo].[CollectionSpecimenImageCache]", this.imageListTransferSteps.Images[13]);
            this.panelTransferSteps.Controls.Add(UIM);
            UIM.Dock = DockStyle.Top;
            UIM.BringToFront();
            this._TransferSteps.Add(UIM);

            foreach (System.Data.DataRow R in this.dataSetCacheDB_1.TaxonSynonymySource.Rows)
            {
                DiversityCollection.CacheDatabase.UserControlTransferStep UT = new UserControlTransferStep("Taxonomomy from " + R["DatabaseName"].ToString(), "EXEC [dbo].[procTransferTaxonSynonymy] " + R["DatabaseName"].ToString() + ", " + R["SourceName"].ToString(), "SELECT * FROM [dbo].[TaxonSynonymy]  WHERE Source = '" + R["DatabaseName"].ToString() + "'", this.imageListTransferSteps.Images[4]);
                this.panelTransferSteps.Controls.Add(UT);
                UT.Dock = DockStyle.Top;
                UT.BringToFront();
                this._TransferSteps.Add(UT);
            }

        }

        private void buttonStartTransfer_Click(object sender, EventArgs e)
        {
            this.TransferDataToCacheDB();
        }

        public bool TransferDataToCacheDB()
        {
            bool OK = true;
            if (this._TransferSteps == null)
                this.initTransferSteps();
            foreach (InterfaceTransferStep I in this._TransferSteps)
            {
                if (!I.ReadyForAction())
                {
                    System.Windows.Forms.MessageBox.Show("The step " + I.Title() + " is not ready for transfer");
                    return false;
                }
            }

            if (this.ClearCacheDB())
            {
                foreach (InterfaceTransferStep I in this._TransferSteps)
                {
                    I.StartTransfer();
                }
                this.CheckLastTransfer();
            }
            return OK;
        }

        private bool ClearCacheDB()
        {
            bool OK = true;
            string SQL = "TRUNCATE TABLE CollectionSpecimenCache";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
            {
                SQL = "TRUNCATE TABLE IdentificationUnitPartCache";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                {
                    SQL = "TRUNCATE TABLE TaxonSynonymy";
                    OK = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                }
                else
                    OK = false;
            }
            else
                OK = false;
            return OK;
        }

        private void buttonV1StartTaxonTransfer_Click(object sender, EventArgs e)
        {
            string Result = "";
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            try
            {
                string SQL = "DECLARE @RC int; EXECUTE @RC = [dbo].[procRefreshTaxonSynonyms] ";
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                Result = C.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (Result.Length == 0)
                Result = "Transfer failed";
            this.Cursor = System.Windows.Forms.Cursors.Default;
            System.Windows.Forms.MessageBox.Show(Result);
        }

        private void buttonV1StartCollectionTransfer_Click(object sender, EventArgs e)
        {
            string Result = "";
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            try
            {
                string SQL = "DECLARE @RC int; EXECUTE @RC = [dbo].[procRefresh] ";
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                C.CommandTimeout = 99999;
                con.Open();
                Result = C.ExecuteScalar().ToString();
                Result += "\r\n" + this.TransferCountryIsoCode();
            }
            catch (Exception ex)
            {
                Result = ex.Message;
            }
            finally
            {
                con.Close();
                con.Dispose();
            }
            if (Result.Length == 0)
                Result = "Transfer failed";
            this.Cursor = System.Windows.Forms.Cursors.Default;
            System.Windows.Forms.MessageBox.Show(Result);
            if (Result.StartsWith("Problems with the taxon syn"))
            {
                System.Windows.Forms.Form f = new Form();
                f.Text = "Problems with the taxon synonymy";
                System.Windows.Forms.DataGridView DGV = new DataGridView();
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT CollectionSpecimenID, IdentificationUnitID, AcceptedName AS [Accepted name 1], _AcceptedName AS [Accepted name 2], AcceptedNameURI AS [Link to name 1], " +
                    "_AcceptedNameURI AS [Link to name 2] " +
                    "FROM Test_IdentificationUnit_AcceptedName_Duplicates";
                System.Data.SqlClient.SqlDataAdapter a = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                a.Fill(dt);
                DGV.DataSource = dt;
                f.Controls.Add(DGV);
                f.StartPosition = FormStartPosition.CenterParent;
                f.Height = this.Height - 20;
                f.Width = this.Width - 20;
                DGV.Dock = DockStyle.Fill;
                f.ShowDialog();
            }
            else
            {
                DiversityWorkbench.FormEditText f = new DiversityWorkbench.FormEditText("Transfer to CacheDB", Result, true);
                f.ShowDialog();
            }
        }

        private void buttonTransferCountryIcoCode_Click(object sender, EventArgs e)
        {
            string Message = this.TransferCountryIsoCode();
            DiversityWorkbench.FormEditText f = new DiversityWorkbench.FormEditText("Transfer to CacheDB", Message, true);
            f.ShowDialog();
        }

        public string TransferCountryIsoCode()
        {
            System.Collections.Generic.List<string> NoCountryIsoCodeFound = new List<string>();
            System.Data.DataTable DtCountryCode = new DataTable();
            string SQL = "SELECT DISTINCT CountryCache FROM CollectionSpecimenCache WHERE NOT CountryCache IS NULL AND RTRIM(CountryCache) <> '' ORDER BY CountryCache";
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            ad.Fill(DtCountryCode);
            System.Data.SqlClient.SqlConnection con = DiversityCollection.CacheDatabase.CacheDB.ConnectionGazetteer();
            if (con == null)
            {
                return "";
            }
            con.Open();
            foreach (System.Data.DataRow R in DtCountryCode.Rows)
            {
                string IsoCode = this.CountryIsoCode(R["CountryCache"].ToString(), con);
                if (IsoCode.Length > 0)
                {
                    SQL = "UPDATE C SET C.CountryIsoCode = '" + IsoCode + "' FROM CollectionSpecimenCache C WHERE C.CountryCache = '" + R["CountryCache"].ToString() + "'";
                    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                }
                else
                {
                    NoCountryIsoCodeFound.Add(R["CountryCache"].ToString());
                }
            }
            con.Close();
            string Message = "";
            if (NoCountryIsoCodeFound.Count > 0)
            {
                foreach (string s in NoCountryIsoCodeFound)
                    Message += s + "\r\n";
                Message = "No Iso Code was found for the following countries:\r\n\r\n" + Message;
            }
            return Message;
        }

        private string CountryIsoCode(string Country, System.Data.SqlClient.SqlConnection con)
        {
            string SQL = "SELECT MIN(I.Name) AS IsoCode " +
                "FROM GeoName AS I INNER JOIN " +
                "GeoPlace AS P ON I.PlaceID = P.PlaceID INNER JOIN " +
                "GeoName AS C ON P.PlaceID = C.PlaceID " +
                "WHERE (P.PlaceType = N'nation') AND (I.LanguageCode = 'ISO 3166 ALPHA-3') AND (C.LanguageCode NOT LIKE N'ISO 3166%' OR " +
                "C.LanguageCode IS NULL) " +
                "and C.Name = '" + Country.Trim() + "'";
            string CountryIsoCode = "";
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                CountryIsoCode = C.ExecuteScalar().ToString();
            }
            catch (System.Exception ex) { }
            return CountryIsoCode;
        }

        //private System.Data.SqlClient.SqlConnection _ConnGazetteer;
        //private System.Data.SqlClient.SqlConnection ConnectionGazetteer()
        //{
        //    if (this._ConnGazetteer == null)
        //    {
        //        DiversityWorkbench.WorkbenchUnit G = DiversityWorkbench.WorkbenchUnit.getWorkbenchUnit("DiversityGazetteer");
        //        string ConnectionString = "";
        //        if (G.ServerConnectionList().Count > 0)
        //        {
        //            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.ServerConnection> KV in G.ServerConnectionList())
        //            {
        //                ConnectionString = KV.Value.ConnectionString;
        //                break;
        //            }
        //        }
        //        if (ConnectionString.Length > 0)
        //        {
        //            this._ConnGazetteer = new System.Data.SqlClient.SqlConnection(ConnectionString);
        //            string SQL = "SELECT COUNT(*) FROM GeoName WHERE PlaceID IS NULL";
        //            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, this._ConnGazetteer);
        //            this._ConnGazetteer.Open();
        //            try
        //            {
        //                string x = C.ExecuteScalar().ToString();
        //                this._ConnGazetteer.Close();
        //            }
        //            catch (System.Exception ex)
        //            {
        //                this._ConnGazetteer = null;
        //                System.Windows.Forms.MessageBox.Show("No connection to Gazetteer established");
        //            }
        //        }
        //    }
        //    return this._ConnGazetteer;
        //}

        #endregion

        #region Postgres transfer

        private System.Data.DataTable _DtPostgresTableList;
        private System.Data.DataTable _DtPostgresTable;

        private void initPostgres()
        {
            this.tabControlPostgresAdmin.TabPages.Remove(this.tabPagePostgresGroups);
            this.tabControlPostgresAdmin.TabPages.Remove(this.tabPagePostgresRoles);
            this.tabControlPostgresAdmin.TabPages.Remove(this.tabPagePostgresTables);

            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                this.comboBoxPostgresProject.DataSource = DiversityCollection.CacheDatabase.Project.DtProjects();// this.DtPostgresEstablishedProjects;
                this.comboBoxPostgresProject.DisplayMember = "Project";
                this.comboBoxPostgresProject.ValueMember = "ProjectID";

            }
            this.PostgresSetConnectionControls();

            this.initPgAdminDatabase();

        }

        private void PostgresSetConnectionControls()
        {
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0)
            {
                this.buttonPostgresConnect.BackColor = System.Drawing.Color.Red;
                this.buttonPostgresConnect.Image = DiversityCollection.Resource.NoPostgres;
                this.labelPostgresConnection.Text = "Not connected";
                this.labelPostgresConnection.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                this.buttonPostgresConnect.BackColor = System.Drawing.Color.Transparent;
                this.buttonPostgresConnect.Image = DiversityCollection.Resource.Postgres;
                this.labelPostgresConnection.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentConnectionDisplayText(); // DiversityWorkbench.PostgreSQL.Connection.;
                this.labelPostgresConnection.ForeColor = System.Drawing.Color.SteelBlue;
            }
        }

        private void buttonPostgresConnect_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(DiversityWorkbench.Forms.FormConnectToDatabase.DatabaseManagementSystem.Postgres, DiversityWorkbench.PostgreSQL.Connection.PreviousConnections());
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentServer(f.Server);
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentPort(f.Port);
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(f.Database);
                if (!f.IsTrusted)
                    DiversityWorkbench.PostgreSQL.Connection.SetCurrentRole(f.User);
                    DiversityWorkbench.PostgreSQL.Connection.Password = f.Password;
            }
            this.PostgresSetConnectionControls();
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
                {
                    DiversityCollection.CacheDatabase.Project.ResetProjects();
                    this.comboBoxPostgresProject.DataSource = DiversityCollection.CacheDatabase.Project.DtProjects();// this.DtPostgresEstablishedProjects;
                    this.comboBoxPostgresProject.DisplayMember = "Project";
                    this.comboBoxPostgresProject.ValueMember = "ProjectID";
                    this.listPostgresViews();
                }
                else
                    System.Windows.Forms.MessageBox.Show("Connection parameters missing or not valid");
            }
            catch (System.Exception ex)
            { }
        }

        private void listBoxPostgresTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            string View = this.listBoxPostgresTables.SelectedItem.ToString();

            this.groupBoxPostgresTable.Text = View;// R[0].ToString();
            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxPostgresProject.SelectedItem;
            string Schema = R[1].ToString();
            char x = '"';
            string SQL = "SELECT * FROM " + x + Schema + x + "." + x + View + x;// R[0].ToString() + x;
            this._DtPostgresTable = new DataTable();
            try
            {
                this._DtPostgresTable = new DataTable();
                NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());// DiversityWorkbench.Postgres.PostgresConnection());
                ad.Fill(this._DtPostgresTable);
                this.dataGridViewPostgresTable.DataSource = this._DtPostgresTable;
            }
            catch (System.Exception ex)
            {
            }
        }

        private void comboBoxPostgresProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.listPostgresViews();
        }

        private void listPostgresViews()
        {
            try
            {
                this.listBoxPostgresTables.Items.Clear();
                DiversityWorkbench.PostgreSQL.Database DB = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase();
                System.Data.DataRowView Rschema = (System.Data.DataRowView)this.comboBoxPostgresProject.SelectedItem;
                string Schema = Rschema[1].ToString();
                if (DB.Schemas.ContainsKey(Schema))
                {
                    foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Table> KV in DB.Schemas[Schema].Tables)//.Views)
                    {
                        this.listBoxPostgresTables.Items.Add(KV.Key);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonPostgresTransferProject_Click(object sender, EventArgs e)
        {
            if (this.comboBoxPostgresProject.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxPostgresProject.SelectedItem;
                string ProjectID = R["ProjectID"].ToString();
                string Project = R["Project"].ToString();
                this.TransferProjectDataToPostgresDB(Project, int.Parse(ProjectID));
                //try
                //{
                //    this.PostgresRemoveProjectData(int.Parse(ProjectID));
                //    this.PostgresTransferProjectData(Project, int.Parse(ProjectID));
                //}
                //catch (System.Exception ex)
                //{
                //}
            }
        }

        public bool TransferProjectDataToPostgresDB(string Project, int? ProjectID)
        {
            bool OK = true;
            if (ProjectID == null)
            {
                System.Data.DataRow[] RR = DiversityCollection.CacheDatabase.Project.DtProjects().Select("Project = '" + Project + "'");
                ProjectID = int.Parse(RR[0]["ProjectID"].ToString());
            }
            try
            {
                this.PostgresRemoveProjectData((int)ProjectID);
                this.PostgresTransferProjectData(Project, (int)ProjectID);
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private void PostgresRemoveProjectData(int ProjectID)
        {
            if (DiversityCollection.CacheDatabase.Project.Projects.ContainsKey(ProjectID))
                DiversityCollection.CacheDatabase.Project.Projects[ProjectID].ClearProject();
            //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Schema> KVschema in DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Schemas)
            //{
            //    DiversityWorkbench.PostgreSQL.Schema S = KVschema.Value;
            //    DiversityCollection.CacheDatabase.Project P = new Project(S.Name, S.Database);// (DiversityCollection.CacheDatabase.Project)KVschema.Value;
            //    if (P.ProjectID == ProjectID)
            //    {

            //        //foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Table> KVtable in DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().GetProject(ProjectID).Tables)
            //        //{
            //        //    DiversityCollection.CacheDatabase.ProjectTable T = (DiversityCollection.CacheDatabase.ProjectTable)KVtable.Value;
            //        //    T.ClearProject(ProjectID);
            //        //}
            //    }
            //}
            //foreach (System.Data.DataRow Rdel in this._DtPostgresTableList.Rows)
            //{
            //    string SQL = "DELETE FROM \"public\".\"" + Rdel[0].ToString() + "\" WHERE ProjectID = " + ProjectID;
            //    DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL);// .Postgres.PostgresExecuteSqlNonQuery(SQL);
            //}
        }

        private void PostgresTransferProjectData(string Project, int ProjectID)
        {
            int i = 0;
            this.progressBarPostgresTransfer.Value = 0;
            //this.progressBarPostgresTransfer.Maximum = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Schemas["public"].Tables.Count;
            this.progressBarPostgresTransfer.Maximum = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Schemas[Project].Tables.Count;
            string Message = "";
            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Table> KV in DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Schemas[Project].Tables)
            {
                this.labelPostgresTransferMessage.Text = "Transfer " + KV.Key;
                this.progressBarPostgresTransfer.Value++;
                Application.DoEvents();
                if(this.PostgresTransferProjectData(Project, ProjectID, KV.Key, ref Message))
                    i++;
            }
            if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
            this.labelPostgresTransferMessage.Text = i.ToString() + " tables transferred";
        }

        private bool PostgresTransferProjectData(string Project, int ProjectID, string TableName, ref string Error)
        {
            bool OK = true;
            try
            {
                // Geting all columns that exist in the Postgres and the SqlServer Cache database
                System.Data.DataTable DtColPG = new DataTable();
                string SQL = "SELECT column_name FROM information_schema.columns " +
                    "WHERE table_schema='" + Project + "' AND table_name = '" + TableName + "'  " +
                    "ORDER BY column_name";
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref DtColPG, ref Message);

                // Getting the columns from SQL Server
                SQL = "select C.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS C " +
                    "where C.TABLE_NAME = '" + TableName + "' " +
                    "order by C.COLUMN_NAME";
                System.Data.DataTable DtColCache = new DataTable();
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(DtColCache);
                //DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref DtColCache, ref Message);

                // Comparing the two tables and getting the intersection
                System.Collections.Generic.List<string> TransferColumns = new List<string>();
                foreach (System.Data.DataRow Rcol in DtColPG.Rows)
                {
                    System.Data.DataRow[] rr = DtColCache.Select("COLUMN_NAME = '" + Rcol[0].ToString() + "'");
                    if (rr.Length > 0)
                        TransferColumns.Add(Rcol[0].ToString());
                }

                if (TransferColumns.Count == 0)
                    return false;

                // filling the intermediate table
                //SQL = "SELECT DISTINCT P.ProjectID";
                //if (!TransferColumns.Contains("CollectionSpecimenID"))
                //    SQL = "SELECT T.ProjectID";
                //string SqlPG = "SELECT \"ProjectID\"";
                //foreach (string C in TransferColumns)
                //{
                //    if (C == "ProjectID")
                //        continue;
                //    SQL += ", T." + C;
                //    SqlPG += ", \"" + C + "\"";
                //}
                SQL = "";
                string SqlPG = "";
                foreach (string C in TransferColumns)
                {
                    if (SQL.Length > 0) SQL += ", ";
                    SQL += "T." + C;
                    if (SqlPG.Length > 0) SqlPG += ", ";
                    SqlPG += "\"" + C + "\"";
                }
                SQL = "SELECT DISTINCT " + SQL;
                SqlPG = "SELECT " + SqlPG;
                SQL += " FROM " + TableName + " AS T";
                if (TransferColumns.Contains("CollectionSpecimenID"))
                {
                    SQL += ", CollectionProject AS P WHERE P.ProjectID = " + ProjectID.ToString();
                    SQL += " AND T.CollectionSpecimenID = P.CollectionSpecimenID";
                }
                else if (TransferColumns.Contains("ProjectID"))
                    SQL += " WHERE T.ProjectID = " + ProjectID.ToString();
                else if (TransferColumns.Contains("CollectionEventID"))
                {
                    SQL += ", CollectionProject AS P, CollectionSpecimen AS S " +
                        "WHERE P.ProjectID = " + ProjectID.ToString() +
                        "AND S.CollectionSpecimenID = P.CollectionSpecimenID " +
                        "AND S.CollectionEventID = T.CollectionEventID";
                }

                SqlPG += " FROM \"" + Project + "\".\"" + TableName + "\"";
                System.Data.DataTable dtSource = new DataTable();
                System.Data.SqlClient.SqlDataAdapter adTrans = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                adTrans.Fill(dtSource);

                // creating the target table
                Npgsql.NpgsqlDataAdapter adPG = new NpgsqlDataAdapter(SqlPG, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());//.Postgres.PostgresConnection().ConnectionString);
                System.Data.DataTable dtTarget = new DataTable();
                adPG.Fill(dtTarget);

                // transfer the data
                foreach (System.Data.DataRow R in dtSource.Rows)
                {
                    System.Data.DataRow Rnew = dtTarget.NewRow();
                    foreach (string s in TransferColumns)
                        Rnew[s] = R[s];
                    //if (!TransferColumns.Contains("ProjectID"))
                    //    Rnew["ProjectID"] = R["ProjectID"];
                    dtTarget.Rows.Add(Rnew);
                }

                Npgsql.NpgsqlCommandBuilder cb = new NpgsqlCommandBuilder(adPG);
                adPG.Update(dtTarget);
            }
            catch (System.Exception ex)
            {
                OK = false;
                if (Error.Length > 0)
                    Error += "\r\n\r\n";
                Error += ex.Message;
            }
            return OK;
        }

        private void userControlConnect_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region BioCASE

        private System.Collections.Generic.Stack<System.Uri> _BioCASEsites;

        private void initBioCASE()
        {
            if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources.Length > 0)
                this.textBoxBioCASEsources.Text = DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources;
        }

        private void buttonBioCASErefresh_Click(object sender, EventArgs e)
        {
            if (this.textBoxBioCASEsources.Text.Length > 0)
            {
                if (DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources != this.textBoxBioCASEsources.Text)
                {
                    DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources = this.textBoxBioCASEsources.Text;
                    DiversityCollection.CacheDatabase.CacheDBsettings.Default.Save();
                }
                try
                {
                    System.Uri U = new Uri(this.textBoxBioCASEsources.Text);
                    this.webBrowserBioCASE.Url = U;
                    //if (this._BioCASEsites == null)
                    //{
                    //    this._BioCASEsites = new Stack<Uri>();
                    //    this._BioCASEsites.Push(U);
                    //}
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        private void buttonBioCASEback_Click(object sender, EventArgs e)
        {
            if (this._BioCASEsites != null && this._BioCASEsites.Count > 1)
            {
                System.Uri U = this._BioCASEsites.Pop();
                this.webBrowserBioCASE.Url = this._BioCASEsites.Pop();
            }
        }

        private void webBrowserBioCASE_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (this._BioCASEsites == null)
                this._BioCASEsites = new Stack<Uri>();
            this._BioCASEsites.Push(this.webBrowserBioCASE.Url);
            if (this.webBrowserBioCASE.Url.ToString() != DiversityCollection.CacheDatabase.CacheDBsettings.Default.BioCASEsources)
                this.buttonBioCASEback.Enabled = true;
        }

        #endregion

        #region Project lists

        private System.Data.DataTable _DtProjectPublished;
        private System.Data.DataTable _DtProjectUnPublished;

        private bool setV1ProjectLists()
        {
            bool OK = true;
            try
            {
                this._DtProjectPublished = new DataTable();

                this._DtProjectUnPublished = new DataTable();

                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT ProjectID, Project, ProjectURI, CoordinatePrecision " +
                    "FROM ProjectPublished " +
                    "ORDER BY Project ";
                    this._SqlDataAdapterProjectProxy = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterProjectProxy.Fill(this._DtProjectPublished);
                    SQL = "";
                    string Projects = "";
                    foreach (System.Data.DataRow R in this._DtProjectPublished.Rows)
                    {
                        if (Projects.Length > 0) Projects += ", ";
                        Projects += R["ProjectID"].ToString();
                    }

                    SQL = "SELECT ProjectID, Project " +
                    "FROM ProjectProxy ";
                    if (Projects.Length > 0)
                        SQL += "WHERE ProjectID NOT IN (" + Projects + ") ";
                    SQL += "ORDER BY Project ";
                    System.Data.SqlClient.SqlDataAdapter adUn = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    adUn.Fill(this._DtProjectUnPublished);
                }
                this.listBoxV1ProjectPublished.DataSource = this._DtProjectPublished;
                this.listBoxV1ProjectPublished.DisplayMember = "Project";
                this.listBoxV1ProjectPublished.ValueMember = "ProjectID";

                this.listBoxProjects.DataSource = this._DtProjectPublished;
                this.listBoxProjects.DisplayMember = "Project";
                this.listBoxProjects.ValueMember = "ProjectID";
                this.splitContainerProjects.Panel2.Enabled = false;
                //this.tableLayoutPanelProjectSettings.Enabled = false;

                this.listBoxV1ProjectsUnpublished.DataSource = this._DtProjectUnPublished;
                this.listBoxV1ProjectsUnpublished.DisplayMember = "Project";
                this.listBoxV1ProjectsUnpublished.ValueMember = "ProjectID";
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private void buttonV1PublishProject_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxV1ProjectsUnpublished.SelectedItem;
            string SQL = "INSERT INTO ProjectPublished (ProjectID, Project) VALUES (" + R["ProjectID"].ToString() + ", '" + R["Project"].ToString() + "')";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            con.Open();
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            C.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            this.setV1ProjectLists();
            this.setMessages();
        }

        private void buttonV1UnpublishProject_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxV1ProjectPublished.SelectedItem;
                string SQL = "DELETE P FROM TaxonomicGroupInProject P WHERE ProjectID = " + R["ProjectID"].ToString();
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                con.Open();
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                C.ExecuteNonQuery();
                C.CommandText = "DELETE P FROM ProjectPublished P WHERE ProjectID = " + R["ProjectID"].ToString();
                C.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                this.setV1ProjectLists();
                this.setMessages();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void listBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxProjects.SelectedItem == null)
            {
                this.splitContainerProjects.Panel2.Enabled = false;
            }
            else
            {
                this.splitContainerProjects.Panel2.Enabled = true;
                this.SetTaxonomicGroups();
                this.SetMaterialCategories();
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                if (!R["CoordinatePrecision"].Equals(System.DBNull.Value))
                {
                    int Precision;
                    if (int.TryParse(R["CoordinatePrecision"].ToString(), out Precision))
                    {
                        this.checkBoxCoordinatePrecision.Checked = true;
                        this.numericUpDownCoordinatePrecision.Enabled = true;
                        this.numericUpDownCoordinatePrecision.Value = Precision;
                    }
                }
                else
                {
                    this.checkBoxCoordinatePrecision.Checked = false;
                    this.numericUpDownCoordinatePrecision.Enabled = false;
                }
                //if (!R["HtmlAvailable"].Equals(System.DBNull.Value))
                //{
                //    this.checkBoxHighResolutionImage.Checked = true;
                //}
                //else
                //    this.checkBoxHighResolutionImage.Checked = false;
            }
        }

        #endregion

        #region Inspection

        private void buttonV1CheckSpecimen_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "";
                string ProjectID;
                string Project;
                if (this.listBoxV1ProjectPublished.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select a published project");
                    return;
                }
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxV1ProjectPublished.SelectedItem;
                ProjectID = R["ProjectID"].ToString();
                Project = R["Project"].ToString();
                SQL = "SELECT S.AccessionNumber, S.CollectionDate, S.CollectionDay, S.CollectionMonth, S.CollectionYear, S.CollectionDateSupplement, S.LocalityDescription, S.CountryCache, " +
                    "S.LabelTranscriptionNotes, S.ExsiccataAbbreviation, S.OriginalNotes, S.Latitude, S.Longitude, S.LocationAccuracy, S.DistanceToLocation, S.DirectionToLocation, " +
                    "S.CollectorsEventNumber, S.Chronostratigraphy, S.Lithostratigraphy " +
                    "FROM CollectionSpecimenCache AS S INNER JOIN " +
                    "CollectionProject AS P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID =  " + ProjectID +
                    " ORDER BY S.AccessionNumber";
                System.Data.DataTable dt = new DataTable();
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormTableContent f = new DiversityWorkbench.Forms.FormTableContent("Specimen - " + Project + " (" + dt.Rows.Count.ToString() + ")", dt.Rows.Count.ToString() + " specimen within the project " + Project + " in the cache database", dt);
                f.ShowDialog();

            }
            catch (Exception ex)
            {
            }
        }

        private void buttonV1CheckTaxa_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT distinct reverse(substring(reverse([NameURI]), charindex('/', reverse([NameURI])) + 1, 255)) AS Source FROM [dbo].[TaxonSynonymy]";
                System.Data.DataTable dtSource = new DataTable();
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(dtSource);
                string WhereClause = "";
                string Source = "";
                if (dtSource.Rows.Count > 1)
                {
                    DiversityWorkbench.FormGetStringFromList f = new DiversityWorkbench.FormGetStringFromList(dtSource, "Please select a source");
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        WhereClause = " WHERE NameURI LIKE '" + f.SelectedString + "%'";
                        Source = f.SelectedString;
                    }
                    else
                        return;
                }
                SQL = "SELECT AcceptedName AS [Accepted name], CASE WHEN SynonymName = AcceptedName then '' else SynonymName end as Synonym FROM TaxonSynonymy " + WhereClause + " ORDER BY AcceptedName";
                System.Data.DataTable dtTaxa = new DataTable();
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(dtTaxa);
                string Title = "Taxa (" + dtTaxa.Rows.Count.ToString() + ")";
                string Header = dtTaxa.Rows.Count.ToString() + " taxa in cache database";
                if (Source.Length > 0)
                {
                    Title += ": " + Source;
                    Header += ". Source = " + Source;
                }
                DiversityWorkbench.Forms.FormTableContent fTable = new DiversityWorkbench.Forms.FormTableContent(Title, Header, dtTaxa);
                fTable.ShowDialog();
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion
        
        #endregion

        #region Administration
        
        #region Administration of database

        #region Update

        private void buttonUpdateDatabase_Click(object sender, EventArgs e)
        {
            this.UpdateDatabase();
        }

        private void setUpdateControls()
        {
            string SQL = "select user_name()";
            string User = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                try
                {
                    con.Open();
                    User = C.ExecuteScalar().ToString();
                    con.Close();
                    if (DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("01") || DiversityCollection.CacheDatabase.CacheDB.DatabaseVersion.StartsWith("00"))
                    {
                        if (DiversityCollection.CacheDatabase.CacheDB.DatabaseRoles().Contains("CacheAdministrator") || User == "dbo")
                        {
                            bool Update = this.UpdateCheckForDatabase();
                            this.buttonUpdateDatabase.Visible = Update;
                            this.textBoxCurrentDatabaseVersion.Visible = true;// this.buttonUpdateDatabase.Visible;
                            this.labelCurrentDatabaseVersion.Visible = true;// this.buttonUpdateDatabase.Visible;
                            this.textBoxAvailableDatabaseVersion.Visible = Update;
                            this.textBoxAvailableDatabaseVersion.Text = DiversityCollection.Properties.Settings.Default.CacheDBV1;

                            System.Data.DataTable dtCheck = new DataTable();
                            string Message = "";
                            int Fit = 0;
                            int NoFit = 0;
                            this.FunctionProjectsDatabaseIsOK(ref dtCheck, ref Message, ref Fit, ref NoFit);
                        }
                    }
                    else
                    {
                        bool IsAdmin = false;
                        if (DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator") || DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
                            IsAdmin = true;
                        if (User == "dbo" || IsAdmin)
                        {
                            this.buttonUpdateDatabase.Visible = this.UpdateCheckForDatabase();
                            this.textBoxCurrentDatabaseVersion.Visible = this.buttonUpdateDatabase.Visible;
                            this.labelCurrentDatabaseVersion.Visible = this.buttonUpdateDatabase.Visible;
                            this.textBoxAvailableDatabaseVersion.Visible = this.buttonUpdateDatabase.Visible;
                        }
                    }
                }
                catch (System.Exception ex)
                { }
            }
        }

        private bool UpdateCheckForDatabase()
        {
            //return false;

            bool Update = false;
            string SQL = "if (select count(*) from INFORMATION_SCHEMA.ROUTINES R " +
                "where R.ROUTINE_TYPE = 'FUNCTION' " +
                "and R.SPECIFIC_NAME = 'Version') = 1 " +
                "select dbo.version() " +
                "else " +
                "select '00.00.00'";
            string Version = "";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                Version = C.ExecuteScalar().ToString();
                con.Close();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("The connection " + DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB + " is not valid.");
                return false;
            }
            if (Version != DiversityCollection.Properties.Settings.Default.CacheDBV1)
            {
                int DBversionMajor;
                int DBversionMinor;
                int DBversionRevision;
                int C_versionMajor;
                int C_versionMinor;
                int C_versionRevision;
                string[] VersionDBParts;
                if (Version.IndexOf('.') > -1) VersionDBParts = Version.Split(new Char[] { '.' });
                else if (Version.IndexOf('/') > -1) VersionDBParts = Version.Split(new Char[] { '/' });
                else VersionDBParts = Version.Split(new Char[] { ' ' });
                if (!int.TryParse(VersionDBParts[0], out DBversionMajor)) return false;
                if (!int.TryParse(VersionDBParts[1], out DBversionMinor)) return false;
                if (!int.TryParse(VersionDBParts[2], out DBversionRevision)) return false;

                string[] VersionC_Parts = DiversityCollection.Properties.Settings.Default.CacheDBV1.Split(new Char[] { '.' });
                if (!int.TryParse(VersionC_Parts[0], out C_versionMajor)) return false;
                if (!int.TryParse(VersionC_Parts[1], out C_versionMinor)) return false;
                if (!int.TryParse(VersionC_Parts[2], out C_versionRevision)) return false;

                if (C_versionMajor > DBversionMajor ||
                    (C_versionMajor == DBversionMajor && C_versionMinor > DBversionMinor) ||
                    (C_versionMajor == DBversionMajor && C_versionMinor == DBversionMinor && C_versionRevision > DBversionRevision))
                    Update = true;
                if (DBversionMajor == 2)
                {
                    Version = "00.00.00";
                    Update = true;
                }
            }
            //if (Update) this.labelV1Update.Text = "Your version of the database is " + Version + ". Please update the database to version " + DiversityCollection.Properties.Settings.Default.CacheDBV1;
            //else this.labelV1Update.Text = "No updates for database";
            //this.panelV1Update.Visible = false;// Update;
            return Update;
        }

        private void UpdateDatabase()
        {
            string DatabaseCurrentVersion = "";
            string SQL = "select dbo.Version()";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                DatabaseCurrentVersion = C.ExecuteScalar().ToString();
                con.Close();
            }
            catch { }
            DatabaseCurrentVersion = DatabaseCurrentVersion.Replace(".", "").Replace("/", "");
            string DatabaseFinalVersion = DiversityCollection.Properties.Settings.Default.CacheDBV1;
            DatabaseFinalVersion = DatabaseFinalVersion.Replace(".", "").Replace("/", "");

            // Ignore former version
            if (DatabaseCurrentVersion.StartsWith("02"))
            {
                SQL = "ALTER FUNCTION [dbo].[Version] () RETURNS nvarchar(8) AS BEGIN RETURN '00.00.00' END";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                DatabaseCurrentVersion = "000000";
            }

            // check resouces for update scripts
            System.Collections.Generic.Dictionary<string, string> Versions = new Dictionary<string, string>();
            System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
            System.Resources.ResourceSet rs = rm.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
            if (rs != null)
            {
                System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                while (de.MoveNext() == true)
                {
                    if (de.Entry.Value is string)
                    {
                        if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdate_"))
                        {
                            Versions.Add(de.Key.ToString(), de.Value.ToString());
                        }
                    }
                }
            }

            if (Versions.Count > 0)
            {
                DiversityWorkbench.FormUpdateDatabase f = new DiversityWorkbench.FormUpdateDatabase(DiversityCollection.CacheDatabase.CacheDB.DatabaseName, DiversityCollection.Properties.Settings.Default.CacheDBV1, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB, Versions, System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm");
                f.ShowDialog();
                if (f.Reconnect) this.setDatabase();
                this.UpdateCheckForDatabase();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
            }
        }

        private void buttonV1Update_Click(object sender, EventArgs e)
        {
            this.UpdateDatabase();
        }

        private bool FunctionProjectsDatabaseIsOK(ref System.Data.DataTable dtNoFit, ref string Message, ref int Fit, ref int NoFit)
        {
            bool OK = true;
            try
            {
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                string SQL = "SELECT [dbo].[ProjectsDatabase] ()";
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                con.Open();
                string ProjectDB = C.ExecuteScalar().ToString();
                C.CommandText = "SELECT count(*) " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P " +
                    "where P.ProjectURI LIKE " + ProjectDB + ".dbo.BaseURL() + '%' ";
                string FittingProject = C.ExecuteScalar().ToString();
                C.CommandText = "SELECT count(*) " +
                    "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P " +
                    "where P.ProjectURI <> '' AND P.ProjectURI not LIKE " + ProjectDB + ".dbo.BaseURL() + '%'";
                string NotFitting = C.ExecuteScalar().ToString();
                if (int.TryParse(FittingProject, out Fit) && int.TryParse(NotFitting, out NoFit))
                {
                    if (NoFit > 0 || Fit == 0)
                    {
                        this.buttonCheckProjectsDatabase.Visible = true;
                        OK = false;
                    }
                    if (NoFit > 0)
                    {
                        SQL = "SELECT P.Project, P.ProjectURI " +
                            "FROM [" + DiversityWorkbench.Settings.DatabaseName + "].[dbo].[ProjectProxy] P " +
                            "where P.ProjectURI <> '' AND P.ProjectURI not LIKE " + ProjectDB + ".dbo.BaseURL() + '%'";
                        System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                        ad.Fill(dtNoFit);
                    }
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
                Message = ex.Message;
                this.buttonCheckProjectsDatabase.Visible = true;
            }
            return OK;
        }

        private void buttonCheckProjectsDatabase_Click(object sender, EventArgs e)
        {
            System.Data.DataTable dtCheck = new DataTable();
            string Message = "";
            int Fit = 0;
            int NoFit = 0;
            this.FunctionProjectsDatabaseIsOK(ref dtCheck, ref Message, ref Fit, ref NoFit);
            if (dtCheck.Rows.Count > 0)
            {
                DiversityWorkbench.Forms.FormTableContent f = new DiversityWorkbench.Forms.FormTableContent("Projects database", "These projects do not match the link to the projects database", dtCheck);
                f.ShowDialog();
            }
            else if (Message.Length > 0)
            {
                System.Windows.Forms.MessageBox.Show(Message);
            }
            else if (Fit == 0)
            {
                System.Windows.Forms.MessageBox.Show("No project with a fitting link to the projects database were found");
            }
        }

        #endregion

        #region User

        private void buttonLoginAdministration_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT dbo.DiversityWorkbenchModule()";
            string Module = "";
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            C.CommandTimeout = 999;
            try
            {
                con.Open();
                Module = C.ExecuteScalar().ToString();
            }
            catch (System.Exception ex) { }
            con.Close();
            if (Module != "DiversityCollectionCache")
            {
                System.Windows.Forms.MessageBox.Show("Please run updates first");
                return;
            }

            DiversityWorkbench.Forms.FormLoginAdministration f = new DiversityWorkbench.Forms.FormLoginAdministration(con);
            f.setHelpProvider(this.helpProvider.HelpNamespace, "Logins");
            f.SqlConnection = con;
            f.ShowDialog();
        }

        #endregion

        #region MaterialCategory

        private void setMaterialCategoryNotTransferredList()
        {
            this.SaveMaterial();
            this._DtMaterialNotTransferred = new DataTable();
            string SQL = "SELECT Code, DisplayText FROM CollMaterialCategory_Enum WHERE (DisplayEnable = 1) ORDER BY DisplayText";
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this._DtMaterialNotTransferred);
            System.Collections.Generic.List<System.Data.DataRow> RowsToDelete = new List<DataRow>();
            foreach (System.Data.DataRow R in this._DtMaterialNotTransferred.Rows)
            {
                System.Data.DataRow[] rr = this.dataSetCacheDB_1.EnumMaterialCategory.Select("Code = '" + R["Code"].ToString() + "'");
                if (rr.Length > 0)
                    RowsToDelete.Add(R);
            }
            foreach (System.Data.DataRow R in RowsToDelete)
                R.Delete();
            this._DtMaterialNotTransferred.AcceptChanges();
            this.listBoxMaterialNotTransferred.DataSource = this._DtMaterialNotTransferred;
            this.listBoxMaterialNotTransferred.DisplayMember = "DisplayText";
            this.listBoxMaterialNotTransferred.ValueMember = "Code";
        }

        private void buttonMaterialAdd_Click(object sender, EventArgs e)
        {
            if (this.listBoxMaterialNotTransferred.SelectedItem == null)
                return;
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxMaterialNotTransferred.SelectedItem;
            DiversityCollection.CacheDatabase.DataSetCacheDB_1.EnumMaterialCategoryRow R = this.dataSetCacheDB_1.EnumMaterialCategory.NewEnumMaterialCategoryRow();
            R.Code = RV["Code"].ToString();
            R.DisplayText = RV["DisplayText"].ToString();
            this.dataSetCacheDB_1.EnumMaterialCategory.Rows.Add(R);
            this.setMaterialCategoryNotTransferredList();
        }

        private void buttonMaterialRemove_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewMaterialCategory.SelectedCells == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the material that should be removed");
                return;
            }
            int i = this.dataGridViewMaterialCategory.SelectedCells[0].RowIndex;
            this.dataSetCacheDB_1.EnumMaterialCategory.Rows[i].Delete();
            this.setMaterialCategoryNotTransferredList();
        }

        private void buttonSaveMaterial_Click(object sender, EventArgs e)
        {
            this.SaveMaterial();
        }

        private void SaveMaterial()
        {
            if (this._SqlDataAdapterMaterialCategory != null && this.dataSetCacheDB_1.EnumMaterialCategory != null && this.dataSetCacheDB_1.EnumMaterialCategory.Rows.Count > 0)
            {
                if (this._SqlDataAdapterMaterialCategory.DeleteCommand == null)
                {
                    System.Data.SqlClient.SqlCommandBuilder builder = new System.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterMaterialCategory);
                }
                try
                {
                    this._SqlDataAdapterMaterialCategory.Update(this.dataSetCacheDB_1.EnumMaterialCategory);
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        #endregion

        #region Localisation

        private void buttonLocalisationPublished_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxLocalisationNotPublished.SelectedItem;
            string SQL = "INSERT INTO EnumLocalisationSystem (LocalisationSystemID, DisplayText, Sequence) " +
                "VALUES (" + R["LocalisationSystemID"].ToString() + ", '" + R["DisplayText"].ToString() + "', CASE WHEN (SELECT MAX(Sequence) + 1 FROM EnumLocalisationSystem) IS NULL THEN 1 ELSE (SELECT MAX(Sequence) + 1 FROM EnumLocalisationSystem) END)";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetLocalisationList();
        }

        private void buttonLocalisationNotPublished_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxLocalisationPublished.SelectedItem;
            string SQL = "DELETE L FROM EnumLocalisationSystem L WHERE LocalisationSystemID = " + R["LocalisationSystemID"].ToString();
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetLocalisationList();
        }

        private void buttonLocalisationPublishedUp_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxLocalisationPublished.SelectedItem;
            string SQL = "UPDATE L SET [Sequence] = (SELECT MIN(Sequence) - 1 FROM EnumLocalisationSystem) FROM EnumLocalisationSystem L WHERE LocalisationSystemID = " + R["LocalisationSystemID"].ToString();
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetLocalisationList();
        }

        private void buttonLocalisationPublishedDown_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxLocalisationPublished.SelectedItem;
            string SQL = "UPDATE L SET [Sequence] = (SELECT MAX(Sequence) + 1 FROM EnumLocalisationSystem) FROM EnumLocalisationSystem L WHERE LocalisationSystemID = " + R["LocalisationSystemID"].ToString();
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetLocalisationList();
        }

        private System.Data.DataTable _DtLocalisationPublished;
        private System.Data.DataTable _DtLocalisationUnPublished;

        private bool SetLocalisationList()
        {
            bool OK = true;
            try
            {
                this._DtLocalisationPublished = new DataTable();

                this._DtLocalisationUnPublished = new DataTable();

                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT LocalisationSystemID, DisplayText, Sequence FROM EnumLocalisationSystem ORDER BY Sequence ";
                    this._SqlDataAdapterLocalisation = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterLocalisation.Fill(this._DtLocalisationPublished);
                    SQL = "";
                    string LocalisationSystems = "";
                    foreach (System.Data.DataRow R in this._DtLocalisationPublished.Rows)
                    {
                        if (LocalisationSystems.Length > 0) LocalisationSystems += ", ";
                        LocalisationSystems += R["LocalisationSystemID"].ToString();
                    }

                    SQL = "SELECT LocalisationSystemID, DisplayText, ParsingMethodName " +
                    "FROM LocalisationSystem WHERE ParsingMethodName NOT IN ('Altitude', 'Exposition', 'Slope', 'Height')";
                    if (LocalisationSystems.Length > 0)
                        SQL += " AND LocalisationSystemID NOT IN (" + LocalisationSystems + ") ";
                    SQL += "ORDER BY ParsingMethodName, LocalisationSystemID, DisplayText ";
                    System.Data.SqlClient.SqlDataAdapter adUn = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    adUn.Fill(this._DtLocalisationUnPublished);
                }
                this.listBoxLocalisationPublished.DataSource = this._DtLocalisationPublished;
                this.listBoxLocalisationPublished.DisplayMember = "DisplayText";
                this.listBoxLocalisationPublished.ValueMember = "LocalisationSystemID";

                this.listBoxLocalisationNotPublished.DataSource = this._DtLocalisationUnPublished;
                this.listBoxLocalisationNotPublished.DisplayMember = "DisplayText";
                this.listBoxLocalisationNotPublished.ValueMember = "LocalisationSystemID";
            }
            catch (System.Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
        }

        #endregion

        #region Kingdom

        private void buttonKingdomPublished_Click(object sender, EventArgs e)
        {
            if (this.listBoxKingdomNotPublished.SelectedItem == null)
                return;

            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxKingdomNotPublished.SelectedItem;
            string SQL = "INSERT INTO EnumTaxonomicGroup (Code, DisplayText) " +
                "VALUES ('" + R["Code"].ToString() + "', '" + R["DisplayText"].ToString() + "')";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetKingdoms();
        }

        private void buttonKingdomNotPublished_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewKingdoms.SelectedCells.Count == 0)
                return;
            string TaxonomicGroup = this.dataGridViewKingdoms.Rows[this.dataGridViewKingdoms.SelectedCells[0].RowIndex].Cells[0].Value.ToString();
            string SQL = "SELECT COUNT(*) FROM TaxonomicGroupInProject WHERE TaxonomicGroup = '" + TaxonomicGroup + "'";
            string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (Result != "0")
            {
                string Message = "The taxonomic group " + TaxonomicGroup + " is still used in " + Result + " project(s).";
                System.Windows.Forms.MessageBox.Show(Message);
                return;
            }
            SQL = "DELETE T FROM EnumTaxonomicGroup T WHERE Code = '" + TaxonomicGroup + "'";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetKingdoms();
        }

        private System.Data.DataTable _DtKingdomsPublished;
        private System.Data.DataTable _DtKingdomsNotPublished;

        private void SetKingdoms()
        {
            try
            {
                if (this._SqlDataAdapterKingdoms != null && this._DtKingdomsPublished != null && this._DtKingdomsPublished.Rows.Count > 0)
                {
                    System.Data.SqlClient.SqlCommandBuilder cb = new System.Data.SqlClient.SqlCommandBuilder(this._SqlDataAdapterKingdoms);
                    this._SqlDataAdapterKingdoms.Update(this._DtKingdomsPublished);
                }
                this._DtKingdomsPublished = new DataTable();

                this._DtKingdomsNotPublished = new DataTable();

                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT Code, DisplayText, Kingdom FROM EnumTaxonomicGroup ORDER BY DisplayText ";
                    this._SqlDataAdapterKingdoms = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterKingdoms.Fill(this._DtKingdomsPublished);
                    SQL = "";
                    string TaxonomicGroups = "";
                    foreach (System.Data.DataRow R in this._DtKingdomsPublished.Rows)
                    {
                        if (TaxonomicGroups.Length > 0) TaxonomicGroups += ", ";
                        TaxonomicGroups += "'" + R["Code"].ToString() + "'";
                    }

                    SQL = "SELECT Code, DisplayText, DisplayOrder FROM CollTaxonomicGroup_Enum WHERE (DisplayEnable = 1) ";
                    if (TaxonomicGroups.Length > 0)
                        SQL += " AND Code NOT IN (" + TaxonomicGroups + ") ";
                    SQL += " ORDER BY DisplayOrder ";
                    System.Data.SqlClient.SqlDataAdapter adUn = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    adUn.Fill(this._DtKingdomsNotPublished);
                }
                this.dataGridViewKingdoms.DataSource = this._DtKingdomsPublished;
                this.dataGridViewKingdoms.Columns[0].Visible = false;
                this.dataGridViewKingdoms.Columns[0].ReadOnly = true;
                this.dataGridViewKingdoms.Columns[1].ReadOnly = true;
                this.dataGridViewKingdoms.AllowUserToAddRows = false;
                this.dataGridViewKingdoms.AllowUserToDeleteRows = false;
                foreach (System.Windows.Forms.DataGridViewColumn C in this.dataGridViewKingdoms.Columns)
                    C.SortMode = DataGridViewColumnSortMode.NotSortable;

                this.listBoxKingdomNotPublished.DataSource = this._DtKingdomsNotPublished;
                this.listBoxKingdomNotPublished.DisplayMember = "DisplayText";
                this.listBoxKingdomNotPublished.ValueMember = "Code";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSaveKingdoms_Click(object sender, EventArgs e)
        {
            this.SetKingdoms();
        }


        private void dataGridViewKingdoms_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        #endregion

        #region Sources for Taxonomy

        private void toolStripButtonTaxonSourceAdd_Click(object sender, EventArgs e)
        {
            string SQL = "select name from sys.databases where name like 'DiversityTaxonNames%'";
            if (this.dataSetCacheDB_1.TaxonSynonymySource.Rows.Count > 0)
            {
                foreach (System.Data.DataRow R in this.dataSetCacheDB_1.TaxonSynonymySource.Rows)
                    SQL += " AND name <> '" + R["DatabaseName"].ToString() + "'";
            }
            System.Data.DataTable dt = new DataTable();
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dt);
            if (dt.Rows.Count == 0)
                System.Windows.Forms.MessageBox.Show("No databases available");
            else if (dt.Rows.Count > 0)
            {
                DiversityWorkbench.FormGetStringFromList f = new DiversityWorkbench.FormGetStringFromList(dt, "Please select a database");
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    string DB = f.SelectedString;
                    SQL = "INSERT INTO TaxonSynonymySource (DatabaseName) VALUES ('" + DB + "')";
                    if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                        this.initTaxonSources();
                }
            }
        }

        public bool initTaxonSources()
        {
            bool OK = true;
            try
            {
                this.dataSetCacheDB_1.TaxonSynonymySource.Clear();
                string SQL = "SELECT DatabaseName, SourceName " +
                    "FROM TaxonSynonymySource " +
                    "ORDER BY DatabaseName ";
                if (this._SqlDataAdapterTaxonSynonymySource == null)
                    this._SqlDataAdapterTaxonSynonymySource = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                else
                    this._SqlDataAdapterTaxonSynonymySource.SelectCommand.CommandText = SQL;
                //this._SqlDataAdapterTaxonSynonymySource.SelectCommand.Connection.ConnectionString = DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB;
                this._SqlDataAdapterTaxonSynonymySource.Fill(this.dataSetCacheDB_1.TaxonSynonymySource);
                System.Collections.Generic.List<System.Windows.Forms.Control> CCtoDelete = new List<Control>();
                foreach (System.Windows.Forms.Control C in this.panelTaxonSources.Controls)
                {
                    CCtoDelete.Add(C);
                }
                this.panelTaxonSources.Controls.Clear();
                foreach (System.Windows.Forms.Control C in CCtoDelete)
                {
                    C.Dispose();
                }
                foreach (System.Data.DataRow R in this.dataSetCacheDB_1.TaxonSynonymySource.Rows)
                {
                    DiversityCollection.CacheDatabase.UserControlTaxonSource U = new UserControlTaxonSource(R, this);
                    U.Dock = DockStyle.Top;
                    this.panelTaxonSources.Controls.Add(U);
                    U.BringToFront();
                }
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private string CreateTaxonSource(string Database)
        {
            string TaxonomicRange = Database.Substring(Database.IndexOf('_'));
            string View = "TaxonSynonymy_" + TaxonomicRange;
            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
                "AS " +
                "SELECT TOP 100 PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .NameID AS varchar) AS NameURI, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .NameID AS varchar) AS SynNameURI, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .SpeciesGenusNameID AS varchar) AS SpeciesGenusNameURI,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName T INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName A ON T .NameID = A.NameID " +
                "WHERE (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP 100 PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .NameID AS varchar), T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .NameID AS varchar) AS SynNameID, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + cast(T .SpeciesGenusNameID AS varchar),  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, NULL " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName T " +
                "WHERE NameID NOT IN " +
                "(SELECT NameID " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy) AND NameID NOT IN " +
                "(SELECT NameID " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName) AND NameID NOT IN " +
                "(SELECT SynNameID " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy) " +
                "UNION " +
                "SELECT TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON A.NameID = T .NameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 ON T .NameID = S1.SynNameID AND A.ProjectID = S1.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S ON S1.NameID = S.SynNameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S2.SynNameID = T .NameID AND S2.ProjectID = A.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID " +
                "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S3 INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S3.SynNameID = T .NameID AND S3.ProjectID = A.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID ON  " +
                "S3.NameID = S2.SynNameID AND S3.ProjectID = S2.ProjectID " +
                "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) AND (S3.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP (100) PERCENT DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, DiversityTaxonNames_" + TaxonomicRange + ".dbo.BaseURL() + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S1.NameID = T1.NameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID ON S1.SynNameID = S.NameID AND  " +
                "S1.ProjectID = S.ProjectID " +
                "WHERE (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'duplicate') OR " +
                "(T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'isonym') " +
                "ORDER BY AcceptedName, SynonymName; GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CollectionCacheUser";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                return View;
            else return "";
        }

        private string CreateTaxonSource(string Server, string Database, string BaseURL)
        {
            string TaxonomicRange = Database.Substring(Database.IndexOf('_'));
            string View = "TaxonSynonymy_" + TaxonomicRange;
            string SQL = "CREATE VIEW [dbo].[" + View + "] " +
                "AS " +
                "SELECT TOP 100 PERCENT '" + BaseURL + "' + cast(T .NameID AS varchar) AS NameURI, T .TaxonNameCache AS AcceptedName,  " +
                "'" + BaseURL + " + cast(T .NameID AS varchar) AS SynNameURI, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + BaseURL + " + cast(T .SpeciesGenusNameID AS varchar) AS SpeciesGenusNameURI,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName T INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName A ON T .NameID = A.NameID " +
                "WHERE (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP (100) PERCENT '" + BaseURL + " + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + BaseURL + " + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + BaseURL + " + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP 100 PERCENT '" + BaseURL + " + cast(T .NameID AS varchar), T .TaxonNameCache AS AcceptedName,  " +
                "'" + BaseURL + " + cast(T .NameID AS varchar) AS SynNameID, T .TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + BaseURL + " + cast(T .SpeciesGenusNameID AS varchar),  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, NULL " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName T " +
                "WHERE NameID NOT IN " +
                "(SELECT NameID " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy) AND NameID NOT IN " +
                "(SELECT NameID " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName) AND NameID NOT IN " +
                "(SELECT SynNameID " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy) " +
                "UNION " +
                "SELECT TOP (100) PERCENT '" + BaseURL + " + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + BaseURL + " + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + BaseURL + " + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON A.NameID = T .NameID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 ON T .NameID = S1.SynNameID AND A.ProjectID = S1.ProjectID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S ON S1.NameID = S.SynNameID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID " +
                "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP (100) PERCENT '" + BaseURL + " + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + BaseURL + " + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + BaseURL + " + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S2.SynNameID = T .NameID AND S2.ProjectID = A.ProjectID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID " +
                "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP (100) PERCENT '" + BaseURL + " + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + BaseURL + " + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + BaseURL + " + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S3 INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON A.NameID = T .NameID ON S3.SynNameID = T .NameID AND S3.ProjectID = A.ProjectID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S2 INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 ON S.SynNameID = S1.NameID AND S.ProjectID = S1.ProjectID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S.NameID = T1.NameID ON S2.NameID = S1.SynNameID AND S2.ProjectID = S1.ProjectID ON  " +
                "S3.NameID = S2.SynNameID AND S3.ProjectID = S2.ProjectID " +
                "WHERE (S.IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (T .IgnoreButKeepForReference = 0) AND (S1.IgnoreButKeepForReference = 0) AND  " +
                "(S2.IgnoreButKeepForReference = 0) AND (S3.IgnoreButKeepForReference = 0) " +
                "UNION " +
                "SELECT TOP (100) PERCENT '" + BaseURL + " + CAST(T .NameID AS varchar) AS Expr1, T .TaxonNameCache AS AcceptedName,  " +
                "'" + BaseURL + " + CAST(T1.NameID AS varchar) AS SynNameID, T1.TaxonNameCache AS SynonymName, T .TaxonomicRank,  " +
                "T .GenusOrSupragenericName, '" + BaseURL + " + CAST(T .SpeciesGenusNameID AS varchar) AS Expr2,  " +
                "T .GenusOrSupragenericName + CASE WHEN T .InfragenericEpithet IS NULL OR " +
                "T .InfragenericEpithet = '' THEN '' ELSE ' ' + T .TaxonomicRank + ' ' + T .InfragenericEpithet END + CASE WHEN T .SpeciesEpithet IS NULL OR " +
                "T .SpeciesEpithet = '' THEN '' ELSE ' ' + T .SpeciesEpithet END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet <> T .InfraspecificEpithet THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .InfraspecificEpithet IS NULL OR " +
                "T .InfraspecificEpithet = '' THEN '' ELSE CASE WHEN T .SpeciesEpithet = T .InfraspecificEpithet AND NOT T .InfraspecificEpithet IS NULL AND  " +
                "T .InfraspecificEpithet <> '' THEN ' ' + CASE WHEN T .TaxonomicRank IS NULL OR " +
                "T .TaxonomicRank = '' THEN '' ELSE T .TaxonomicRank + ' ' END + T .InfraspecificEpithet ELSE '' END END + CASE WHEN T .NonNomenclaturalNameSuffix IS NULL  " +
                "THEN '' ELSE ' ' + T .NonNomenclaturalNameSuffix END AS TaxonNameSinAuthor, A.ProjectID " +
                "FROM [" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S1 INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T1 ON S1.NameID = T1.NameID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonSynonymy AS S INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonName AS T ON S.SynNameID = T .NameID INNER JOIN " +
                "[" + Server + "].DiversityTaxonNames_" + TaxonomicRange + ".dbo.TaxonAcceptedName AS A ON T .NameID = A.NameID AND S.ProjectID = A.ProjectID ON S1.SynNameID = S.NameID AND  " +
                "S1.ProjectID = S.ProjectID " +
                "WHERE (T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'duplicate') OR " +
                "(T .IgnoreButKeepForReference = 0) AND (A.IgnoreButKeepForReference = 0) AND (S.IgnoreButKeepForReference = 0) AND (S1.SynType = N'isonym') " +
                "ORDER BY AcceptedName, SynonymName; GRANT SELECT ON TaxonSynonymy_" + TaxonomicRange + " TO CollectionCacheUser";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                return View;
            else return "";
        }

        #endregion

        #region Project and their contents

        #region Adding and removing projects
        
        private void toolStripButtonProjectAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.FormGetStringFromList f = new DiversityWorkbench.FormGetStringFromList(this._DtProjectUnPublished, "Project", "ProjectID", "New project", "Please select the project that should be published");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                string SQL = "INSERT INTO ProjectPublished (ProjectID, Project) VALUES (" + f.SelectedValue + ", '" + f.SelectedString + "')";
                System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                con.Open();
                System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
                C.ExecuteNonQuery();
                con.Close();
                con.Dispose();
                this.setV1ProjectLists();
            }
        }
        
        private void toolStripButtonProjectRemove_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjects.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Nothing selected");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            string SQL = "DELETE P FROM TaxonomicGroupInProject P WHERE ProjectID = " + R["ProjectID"].ToString();
            System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            con.Open();
            System.Data.SqlClient.SqlCommand C = new System.Data.SqlClient.SqlCommand(SQL, con);
            C.ExecuteNonQuery();
            C.CommandText = "DELETE P FROM ProjectPublished P WHERE ProjectID = " + R["ProjectID"].ToString();
            C.ExecuteNonQuery();
            con.Close();
            con.Dispose();
            this.setV1ProjectLists();
        }

        #endregion

        #region Coordinate precision

        private void checkBoxCoordinatePrecision_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            string Value = this.numericUpDownCoordinatePrecision.Value.ToString();
            if (!this.checkBoxCoordinatePrecision.Checked)
            {
                Value = "NULL";
                RV["CoordinatePrecision"] = System.DBNull.Value;
                this.numericUpDownCoordinatePrecision.Enabled = false;
            }
            else
            {
                RV["CoordinatePrecision"] = this.numericUpDownCoordinatePrecision.Value;
                this.numericUpDownCoordinatePrecision.Enabled = true;
            }

            string SQL = "UPDATE P SET P.CoordinatePrecision = " + Value + " FROM ProjectPublished P WHERE P.ProjectID = " + RV["ProjectID"].ToString();
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);

            //if (this.projectPublishedBindingSource.Current != null)
            //{
            //    System.Data.DataRowView R = (System.Data.DataRowView)this.projectPublishedBindingSource.Current;
            //    if (this.checkBoxCoordinatePrecision.Checked)
            //    {
            //        this.numericUpDownCoordinatePrecision.Visible = true;
            //        this.numericUpDownCoordinatePrecision.Value = 0;
            //        R["CoordinatePrecision"] = 0;
            //    }
            //    else
            //    {
            //        this.numericUpDownCoordinatePrecision.Visible = false;
            //        R["CoordinatePrecision"] = System.DBNull.Value;
            //    }
            //}
            //else
            //    System.Windows.Forms.MessageBox.Show("No project selected");
        }

        private void numericUpDownCoordinatePrecision_Click(object sender, EventArgs e)
        {
            System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            RV["CoordinatePrecision"] = this.numericUpDownCoordinatePrecision.Value;
            string SQL = "UPDATE P SET P.CoordinatePrecision = " + this.numericUpDownCoordinatePrecision.Value.ToString() + " FROM ProjectPublished P WHERE P.ProjectID = " + RV["ProjectID"].ToString();
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);


            //if (this.projectPublishedBindingSource.Current != null)
            //{
            //    System.Data.DataRowView R = (System.Data.DataRowView)this.projectPublishedBindingSource.Current;
            //    R["CoordinatePrecision"] = this.numericUpDownCoordinatePrecision.Value;
            //}
            //else if (this.dataSetCacheDB_1.ProjectPublished.Rows.Count == 1)
            //{
            //}
        }

        #endregion

        #region Images

        private void buttonHighResolutionImages_Click(object sender, EventArgs e)
        {
            string SQL = "SELECT ROUTINE_DEFINITION AS Definition FROM INFORMATION_SCHEMA.ROUTINES WHERE ROUTINE_NAME = 'HighResolutionImagePath'";
            string Definition = "Definition of the Function for retrieval of high resolution images.\r\n(Please ask your admininstrator for changes)\r\n";
            Definition += DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            DiversityWorkbench.FormEditText f = new DiversityWorkbench.FormEditText("High resolution images", Definition, true);
            f.ShowDialog();
        }

        private void checkBoxHighResolutionImage_Click(object sender, EventArgs e)
        {
            //System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            //string Value = "1";
            //if (!this.checkBoxHighResolutionImage.Checked)
            //{
            //    Value = "NULL";
            //    RV["HtmlAvailable"] = System.DBNull.Value;
            //}
            //else
            //{
            //    RV["HtmlAvailable"] = Value;
            //}

            //string SQL = "UPDATE P SET P.HtmlAvailable = " + Value + " FROM ProjectPublished P WHERE P.ProjectID = " + RV["ProjectID"].ToString();
            //DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        }

        #endregion

        #region Taxonomic groups

        private void buttonProjectTaxonomicGroupPublished_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectTaxonomicGroupNotPublished.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the taxonomic group that should be published");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectTaxonomicGroupNotPublished.SelectedItem;
            this.InsertTaxonomicGroupForProject(R["TaxonomicGroup"].ToString());
        }

        private bool InsertTaxonomicGroupForProject(string Code)
        {
            bool OK = true;
            System.Data.DataRowView Rproject = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            string SQL = "INSERT INTO ProjectTaxonomicGroup (TaxonomicGroup, ProjectID) " +
                "VALUES ('" + Code + "', " + Rproject["ProjectID"].ToString() + ")";
            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                this.SetTaxonomicGroups();
            else
                OK = false;
                //System.Windows.Forms.MessageBox.Show("Insert failed: " + Message);
            return OK;
        }

        private void buttonProjectTaxonomicGroupNotPublished_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectTaxonomicGroupPublished.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the taxonomic group that should not be published");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectTaxonomicGroupPublished.SelectedItem;
            string SQL = "DELETE T FROM ProjectTaxonomicGroup T WHERE ProjectID = " + R["ProjectID"].ToString() + " AND TaxonomicGroup = '" + R["TaxonomicGroup"].ToString() + "'";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetTaxonomicGroups();
        }

        private System.Data.DataTable _DtTaxonomicGroupPublished;
        private System.Data.DataTable _DtTaxonomicGroupNotPublished;

        private void SetTaxonomicGroups()
        {
            try
            {
                this._DtTaxonomicGroupPublished = new DataTable();
                this._DtTaxonomicGroupNotPublished = new DataTable();

                int ProjectID;
                if (this.listBoxProjects.SelectedItem == null)
                    return;
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                if (!int.TryParse(RV["ProjectID"].ToString(), out ProjectID))
                    return;

                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT P.TaxonomicGroup, P.ProjectID FROM ProjectTaxonomicGroup P WHERE P.ProjectID = " + ProjectID.ToString() + " ORDER BY P.TaxonomicGroup ";
                    this._SqlDataAdapterTaxonomicGroup = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterTaxonomicGroup.Fill(this._DtTaxonomicGroupPublished);
                    SQL = "";
                    string TaxonomicGroups = "";
                    foreach (System.Data.DataRow R in this._DtTaxonomicGroupPublished.Rows)
                    {
                        if (TaxonomicGroups.Length > 0) TaxonomicGroups += ", ";
                        TaxonomicGroups += "'" + R["TaxonomicGroup"].ToString() + "'";
                    }

                    SQL = "select distinct U.TaxonomicGroup " +
                        "from " + DiversityWorkbench.Settings.DatabaseName + ".dbo.IdentificationUnit U, " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionProject P " +
                        "where U.CollectionSpecimenID = P.CollectionSpecimenID " +
                        "and P.ProjectID = " + ProjectID.ToString() + " " +
                        "and U.TaxonomicGroup not in (SELECT TaxonomicGroup " +
                        "FROM ProjectTaxonomicGroup T " +
                        "WHERE T.ProjectID = " + ProjectID.ToString() + ") " +
                        "ORDER BY U.TaxonomicGroup ";

                    //SQL = "SELECT Code, DisplayText FROM EnumTaxonomicGroup"; // CollTaxonomicGroup_Enum WHERE (DisplayEnable = 1) ";
                    //if (TaxonomicGroups.Length > 0)
                    //    SQL += " WHERE Code NOT IN (" + TaxonomicGroups + ") ";
                    //SQL += " ORDER BY DisplayText ";
                    System.Data.SqlClient.SqlDataAdapter adUn = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    adUn.Fill(this._DtTaxonomicGroupNotPublished);
                }
                this.listBoxProjectTaxonomicGroupPublished.DataSource = this._DtTaxonomicGroupPublished;
                this.listBoxProjectTaxonomicGroupPublished.DisplayMember = "TaxonomicGroup";
                this.listBoxProjectTaxonomicGroupPublished.ValueMember = "TaxonomicGroup";

                this.listBoxProjectTaxonomicGroupNotPublished.DataSource = this._DtTaxonomicGroupNotPublished;
                this.listBoxProjectTaxonomicGroupNotPublished.DisplayMember = "TaxonomicGroup";
                this.listBoxProjectTaxonomicGroupNotPublished.ValueMember = "TaxonomicGroup";
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonProjectTaxonomicGroupTransferExisting_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView Rproject = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT U.TaxonomicGroup FROM IdentificationUnit AS U INNER JOIN " +
                    "CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE (P.ProjectID = " + Rproject["ProjectID"].ToString() + ") GROUP BY U.TaxonomicGroup";
                System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);// DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                ad.Fill(dt);
                string NotTransferred = "";
                foreach (System.Data.DataRow r in dt.Rows)
                {
                    if (!this.InsertTaxonomicGroupForProject(r["TaxonomicGroup"].ToString()))
                    {
                        if (NotTransferred.Length > 0) NotTransferred += ", ";
                        NotTransferred += r["TaxonomicGroup"].ToString();
                    }
                }
                if (NotTransferred.Length > 0)
                    System.Windows.Forms.MessageBox.Show("The following taxonomic groups could not be transferred: " + NotTransferred);
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Material category

        private System.Data.DataTable _DtMaterialPublished;
        private System.Data.DataTable _DtMaterialNotPublished;

        private void buttonProjectMaterialCategoryTransferExisting_Click(object sender, EventArgs e)
        {
            try
            {
                int ProjectID;
                if (this.listBoxProjects.SelectedItem == null)
                    return;
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                if (!int.TryParse(RV["ProjectID"].ToString(), out ProjectID))
                    return;
                string SQL = "INSERT INTO ProjectMaterialCategory " +
                    "(MaterialCategory, ProjectID) " +
                    "select distinct S.MaterialCategory, P.ProjectID " +
                    "from " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionSpecimenPart S, " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionProject P " +
                    "where S.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "and P.ProjectID = " + ProjectID.ToString() + " " +
                    "and S.MaterialCategory not in (SELECT MaterialCategory " +
                    "FROM ProjectMaterialCategory M " +
                    "WHERE M.ProjectID = " + ProjectID.ToString() + ") " +
                    "ORDER BY S.MaterialCategory ";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetMaterialCategories();

                //System.Data.DataRowView Rproject = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                //System.Data.DataTable dt = new DataTable();
                //string SQL = "SELECT U.TaxonomicGroup FROM IdentificationUnit AS U INNER JOIN " +
                //    "CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                //    "WHERE (P.ProjectID = " + Rproject["ProjectID"].ToString() + ") GROUP BY U.TaxonomicGroup";
                //System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);// DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                //ad.Fill(dt);
                //string NotTransferred = "";
                //foreach (System.Data.DataRow r in dt.Rows)
                //{
                //    if (!this.InsertTaxonomicGroupForProject(r["TaxonomicGroup"].ToString()))
                //    {
                //        if (NotTransferred.Length > 0) NotTransferred += ", ";
                //        NotTransferred += r["TaxonomicGroup"].ToString();
                //    }
                //}
                //if (NotTransferred.Length > 0)
                //    System.Windows.Forms.MessageBox.Show("The following taxonomic groups could not be transferred: " + NotTransferred);
            }
            catch (System.Exception ex)
            {
            }

        }

        private bool InsertMaterialCategoryForProject(string Code)
        {
            bool OK = true;
            System.Data.DataRowView Rproject = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            string SQL = "INSERT INTO ProjectTaxonomicGroup (TaxonomicGroup, ProjectID) " +
                "VALUES ('" + Code + "', " + Rproject["ProjectID"].ToString() + ")";
            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                this.SetMaterialCategories();
            else
                OK = false;
            return OK;
        }

        private void SetMaterialCategories()
        {
            try
            {
                this._DtMaterialNotPublished = new DataTable();
                this._DtMaterialPublished = new DataTable();

                int ProjectID;
                if (this.listBoxProjects.SelectedItem == null)
                    return;
                System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                if (!int.TryParse(RV["ProjectID"].ToString(), out ProjectID))
                    return;

                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT P.MaterialCategory FROM ProjectMaterialCategory P WHERE P.ProjectID = " + ProjectID.ToString() + " ORDER BY P.MaterialCategory ";
                    this._SqlDataAdapterTaxonomicGroup = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterTaxonomicGroup.Fill(this._DtMaterialPublished);
                    SQL = "";
                    //string MaterialCategory = "";
                    //foreach (System.Data.DataRow R in this._DtMaterialPublished.Rows)
                    //{
                    //    if (MaterialCategory.Length > 0) MaterialCategory += ", ";
                    //    MaterialCategory += "'" + R["MaterialCategory"].ToString() + "'";
                    //}

                    SQL = "select distinct S.MaterialCategory " +
                        "from " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionSpecimenPart S, " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionProject P " +
                        "where S.CollectionSpecimenID = P.CollectionSpecimenID " +
                        "and P.ProjectID = " + ProjectID.ToString() + " " +
                        "and S.MaterialCategory not in (SELECT MaterialCategory " +
                        "FROM ProjectMaterialCategory M " +
                        "WHERE M.ProjectID = " + ProjectID.ToString() + ") " +
                        "ORDER BY S.MaterialCategory ";

                    //SQL = "SELECT Code, DisplayText FROM EnumTaxonomicGroup"; // CollTaxonomicGroup_Enum WHERE (DisplayEnable = 1) ";
                    //if (MaterialCategory.Length > 0)
                    //    SQL += " WHERE Code NOT IN (" + MaterialCategory + ") ";
                    //SQL += " ORDER BY DisplayText ";
                    System.Data.SqlClient.SqlDataAdapter adUn = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    adUn.Fill(this._DtMaterialNotPublished);
                }
                this.listBoxProjectMaterialCategoryPublished.DataSource = this._DtMaterialPublished;
                this.listBoxProjectMaterialCategoryPublished.DisplayMember = "MaterialCategory";
                this.listBoxProjectMaterialCategoryPublished.ValueMember = "MaterialCategory";

                this.listBoxProjectMaterialCategoryNotPublished.DataSource = this._DtMaterialNotPublished;
                this.listBoxProjectMaterialCategoryNotPublished.DisplayMember = "MaterialCategory";
                this.listBoxProjectMaterialCategoryNotPublished.ValueMember = "MaterialCategory";
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonProjectMaterialCategoryPublishe_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectMaterialCategoryNotPublished.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the material category that should be published");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectMaterialCategoryNotPublished.SelectedItem;
            this.InsertMaterialCategoryForProject(R["Code"].ToString());
        }

        private void buttonProjectMaterialCategoryWithhold_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectMaterialCategoryPublished.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the material category that should not be published");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectMaterialCategoryPublished.SelectedItem;
            string SQL = "DELETE T FROM ProjectMaterialCategory T WHERE ProjectID = " + R["ProjectID"].ToString() + " AND MaterialCategory = '" + R["MaterialCategory"].ToString() + "'";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetMaterialCategories();
        }

 
	    #endregion        
        
        #region Settings

        private void buttonProjectRecordURI_Click(object sender, EventArgs e)
        {
            if (this.textBoxProjectsDatabase.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Project database is not defined");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            string ProjectID = R["ProjectID"].ToString();
            string SQL = "SELECT Value FROM [" + this.textBoxProjectsDatabase.Text + "].DBO.SettingsForProject(" + ProjectID + ", '%ABCD | %', '', 2) S WHERE S.ProjectSetting = 'RecordURI'";
            string RecordURI = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            if (RecordURI.Length > 0)
                System.Windows.Forms.MessageBox.Show(RecordURI, "Record URI");
            else System.Windows.Forms.MessageBox.Show("Record URI has not been defined");
        }

        private void buttonProjectSettings_Click(object sender, EventArgs e)
        {
            if (this.textBoxProjectsDatabase.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Project database is not defined");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            string ProjectID = R["ProjectID"].ToString();
            string Project = R["Project"].ToString();
            string SQL = "SELECT S.DisplayText, S.Description, S.ProjectSetting, S.Value FROM [" + this.textBoxProjectsDatabase.Text + "].DBO.SettingsForProject(" + ProjectID + ", '%ABCD | %', '', 2) S";
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
            DiversityWorkbench.FormEditTable f = new DiversityWorkbench.FormEditTable(ad, "Settings", "Settings for the project " + Project, false);
            f.ShowDialog();
        }

        #endregion

        #endregion

        #endregion

        #region Administration of the postgres DB

        #region init and settings

        private void initPostgresAdministration()
        {
            try
            {
                string Version = "";
                this.tableLayoutPanelPostgresAdminProjects.Enabled = !this.PostgresDatabaseNeedsUpdate(ref Version);
                //this.toolStripButtonPostgresConnect.BackColor = System.Drawing.Color.Red;
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Databases

        private void toolStripButtonPostgresConnect_Click(object sender, EventArgs e)
        {
            this.ConnectToPostgresDatabase();
        }

        private void ConnectToPostgresDatabase()
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            //this.listBoxPostgresDBs.Items.Clear();

            DiversityWorkbench.Forms.FormConnectToDatabase f = new DiversityWorkbench.Forms.FormConnectToDatabase(DiversityWorkbench.Forms.FormConnectToDatabase.DatabaseManagementSystem.Postgres, DiversityWorkbench.PostgreSQL.Connection.PreviousConnections());
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentServer(f.Server);
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentPort(f.Port);
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(f.Database);
                if (!f.IsTrusted)
                {
                    DiversityWorkbench.PostgreSQL.Connection.SetCurrentRole(f.User);
                    DiversityWorkbench.PostgreSQL.Connection.Password = f.Password;
                }
                this.tabPageAdminPostgres.Text = f.Database;
            }

            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                if (f.IsTrusted)
                    DiversityWorkbench.PostgreSQL.Connection.AddPreviousConnection(f.Server, f.Port);
                else
                    DiversityWorkbench.PostgreSQL.Connection.AddPreviousConnection(f.Server, f.Port, f.User);
            }

            this.initPgAdminDatabase();

            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void initPgAdminDatabase()
        {
            string Database = "";
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                Database = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
            }
            if (Database != "postgres" && DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(Database))
            {
                tableLayoutPanelPostgresAdmin.Enabled = true;
                string Version = "";
                this.tableLayoutPanelPostgresAdminProjects.Enabled = !this.PostgresDatabaseNeedsUpdate(ref Version);
                this.initPostgresAdminProjectLists();
                this.initPgAdminRoles();
                this.initPGtables();
                this.initOverview();
            }
            this.setPgAdminConnectionControls();
        }

        private void setPgAdminConnectionControls()
        {
            string Database = "";
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)
            {
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() != null)
                    Database = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
            }
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0 
                && Database.Length > 0)
            {
                if (Database.ToLower() == "postgres")
                {
                    this.toolStripButtonPostgresDeleteDB.Enabled = false;
                    this.toolStripButtonPostgresNewDB.Enabled = true;
                    this.toolStripButtonPgAdminRoles.Enabled = true;
                    this.labelPgAdminConnection.Text = "No cache database selected or available";
                    this.labelPgAdminConnection.ForeColor = System.Drawing.Color.Red;
                    this.toolStripButtonPostgresConnect.BackColor = System.Drawing.Color.Transparent;
                    this.toolStripButtonPostgresConnect.Image = DiversityCollection.Resource.Postgres;

                    this.tabPageAdminPostgres.Text = "No cache database";
                }
                else
                {
                    this.labelPgAdminConnection.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentConnectionDisplayText();
                    this.labelPgAdminConnection.ForeColor = System.Drawing.Color.SteelBlue;
                    this.toolStripButtonPostgresConnect.BackColor = System.Drawing.Color.Transparent;
                    this.toolStripButtonPostgresConnect.Image = DiversityCollection.Resource.Postgres;

                    this.tabPageAdminPostgres.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                    //this.initPgAdminDatabaseList();

                    ///TODO: klappt noch nicht
                    if (DiversityWorkbench.PostgreSQL.Connection.Roles()[DiversityWorkbench.PostgreSQL.Connection.CurrentRole()].IsSuperuser)
                    {
                        this.toolStripButtonPostgresDeleteDB.Enabled = true;
                        this.toolStripButtonPostgresNewDB.Enabled = true;
                        this.toolStripButtonPgAdminRoles.Enabled = true;
                    }
                    else
                    {
                        this.toolStripButtonPostgresDeleteDB.Enabled = false;// true;
                        this.toolStripButtonPostgresNewDB.Enabled = false;
                        this.toolStripButtonPgAdminRoles.Enabled = false;
                    }
                }
            }
            else
            {
                this.labelPgAdminConnection.Text = "Not connected";
                this.labelPgAdminConnection.ForeColor = System.Drawing.Color.Red;
                this.toolStripButtonPostgresConnect.Image = DiversityCollection.Resource.NoPostgres;
            }
        }

        private void initPgAdminDatabaseList()
        {
            // wird nicht mehr verwendet
            return;

            this.listBoxPostgresDBs.Items.Clear();
            DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
            foreach(System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.PostgreSQL.Database> KV in DiversityWorkbench.PostgreSQL.Connection.Databases())
            {
                this.listBoxPostgresDBs.Items.Add(KV.Key);
            }
            if (this.listBoxPostgresDBs.Items.Count > 0)
                this.listBoxPostgresDBs.SelectedIndex = 0;
        }

        private void toolStripButtonPostgresNewDB_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0)// .Postgres.PostgresConnection() == null)
                this.toolStripButtonPostgresConnect_Click(null, null);
            string CacheDatabase = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
            DiversityWorkbench.FormGetString f = new DiversityWorkbench.FormGetString("New Database", "Please enter the name of the new database", CacheDatabase);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                char x = '"';
                string Database = f.String.Replace("\"", "").Replace("'", "");
                string SQL = "CREATE DATABASE " + x + f.String + x +
                    "WITH ENCODING='UTF8' OWNER=postgres TABLESPACE = pg_default CONNECTION LIMIT=-1;";
                string Message = "";
                if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))// .Postgres.PostgresExecuteSqlNonQuery(SQL, ref Message))
                    System.Windows.Forms.MessageBox.Show(Message);
                else
                {
                    DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(Database);// .Postgres.DatabaseName = Database;
                    //DiversityWorkbench.Postgres.PostgresConnectionReset();
                    SQL = "CREATE OR REPLACE FUNCTION diversityworkbenchmodule() " +
                        "RETURNS text AS " +
                        "$BODY$ " +
                        "declare " +
                        "v text; " +
                        "BEGIN " +
                        "SELECT 'DiversityCollectionCache' into v; " +
                        "RETURN v; " +
                        "END; " +
                        "$BODY$ " +
                        "LANGUAGE plpgsql STABLE " +
                        "COST 100;";
                    if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))// .Postgres.PostgresExecuteSqlNonQuery(SQL, ref Message))
                        System.Windows.Forms.MessageBox.Show(Message);
                    else
                    {
                        SQL = "CREATE OR REPLACE FUNCTION version() " +
                            "RETURNS text AS " +
                            "$BODY$ " +
                            "declare " +
                            "v text; " +
                            "BEGIN " +
                            "SELECT '00.00.00' into v; " +
                            "RETURN v; " +
                            "END; " +
                            "$BODY$ " +
                            "LANGUAGE plpgsql STABLE " +
                            "COST 100;";
                        if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) // .Postgres.PostgresExecuteSqlNonQuery(SQL, ref Message))
                            System.Windows.Forms.MessageBox.Show(Message);
                        else
                        {
                            DiversityWorkbench.FormGetString fDes = new DiversityWorkbench.FormGetString("Description for database", "Please enter a short description for the new database", "");
                            fDes.ShowDialog();
                            if (fDes.String.Length > 0 && fDes.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                string Description = fDes.String.Replace("\"", "").Replace("'", "");
                                SQL = "COMMENT ON DATABASE \"" + Database + "\" IS '" + Description + "';";
                                if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message)) // .Postgres.PostgresExecuteSqlNonQuery(SQL, ref Message))
                                    System.Windows.Forms.MessageBox.Show(Message);
                            }
                        }
                    }
                    DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                    DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(Database);

                    this.initPgAdminDatabase();

                    //this.initPgAdminDatabaseList();
                    //this.initPostgresDatabaseList();
                }
            }
        }

        private void toolStripButtonPostgresDeleteDB_Click(object sender, EventArgs e)
        {
            string Database = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name; // this.listBoxPostgresDBs.SelectedItem.ToString();
            string SQL = "SELECT pg_terminate_backend(pg_stat_activity.pid) " +
                "FROM pg_stat_activity " +
                "WHERE pg_stat_activity.datname = '" + Database + "' " +
                "AND pid <> pg_backend_pid();";// DROP DATABASE \"" + Database + "\";";
            if (System.Windows.Forms.MessageBox.Show("Do you really want to delete the database " + Database + "?", "Delete database", MessageBoxButtons.YesNo ) == System.Windows.Forms.DialogResult.Yes)
            {
                DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase("postgres");
                string Message = "";
                if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                {
                    SQL = "DROP DATABASE \"" + Database + "\";";
                    if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))
                    {
                        System.Windows.Forms.MessageBox.Show("Database " + Database + " deleted");
                        DiversityWorkbench.PostgreSQL.Connection.ResetDatabases();
                        DiversityWorkbench.PostgreSQL.Connection.ResetDefaultConnectionString();

                        this.initPgAdminDatabase();
                        //this.PostgresSetConnectionControls();
                        //this.setPgAdminConnectionControls();
                        //this.initOverview();
                        //this.initPgAdminDatabaseList();
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("Deleting database " + Database + " failed:\r\n" + Message);
                }
            }
        }

        private void listBoxPostgresDBs_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string Version = "";
                string Database = this.listBoxPostgresDBs.SelectedItem.ToString();
                if (Database != "postgres" && DiversityWorkbench.PostgreSQL.Connection.SetCurrentDatabase(Database))
                {
                    tableLayoutPanelPostgresAdmin.Enabled = true;
                    //DiversityWorkbench.Postgres.DatabaseName = this.listBoxPostgresDBs.SelectedItem.ToString();
                    //DiversityWorkbench.Postgres.PostgresConnectionReset();
                    this.tableLayoutPanelPostgresAdminProjects.Enabled = !this.PostgresDatabaseNeedsUpdate(ref Version);
                    this.initPostgresAdminProjectLists();
                    this.initPgAdminRoles();
                    this.initPGtables();
                    this.initOverview();
                }
                else
                {
                    string Message = "Database " + Database + " does not exist";
                    if (Database == "postgres")
                        Message = "Database " + Database + " is the administrative database and is not accessible as a cache database";
                    System.Windows.Forms.MessageBox.Show(Message);
                    tableLayoutPanelPostgresAdmin.Enabled = false;
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButtonPgAdminRoles_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.PostgreSQL.FormRoleAdministration f = new DiversityWorkbench.PostgreSQL.FormRoleAdministration();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Update

        private void buttonPostgresUpdate_Click(object sender, EventArgs e)
        {
            this.UpdatePostgresDatabase();
            this.initPgAdminDatabase();
        }

        private bool PostgresDatabaseNeedsUpdate(ref string Version)
        {
            string SQL = "select public.version()";
            Version = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// DiversityWorkbench.Postgres.PostgresExecuteSqlSkalar(SQL);
            bool NeedsUpdate = false;
            if (Version != DiversityCollection.Properties.Settings.Default.PostgresCacheDBVersion)
            {
                int DBversionMajor;
                int DBversionMinor;
                int DBversionRevision;
                int C_versionMajor;
                int C_versionMinor;
                int C_versionRevision;
                string[] VersionDBParts;
                if (Version.IndexOf('.') > -1) VersionDBParts = Version.Split(new Char[] { '.' });
                else if (Version.IndexOf('/') > -1) VersionDBParts = Version.Split(new Char[] { '/' });
                else VersionDBParts = Version.Split(new Char[] { ' ' });
                if (!int.TryParse(VersionDBParts[0], out DBversionMajor)) return false;
                if (!int.TryParse(VersionDBParts[1], out DBversionMinor)) return false;
                if (!int.TryParse(VersionDBParts[2], out DBversionRevision)) return false;

                string[] VersionC_Parts = DiversityCollection.Properties.Settings.Default.CacheDBV1.Split(new Char[] { '.' });
                if (!int.TryParse(VersionC_Parts[0], out C_versionMajor)) return false;
                if (!int.TryParse(VersionC_Parts[1], out C_versionMinor)) return false;
                if (!int.TryParse(VersionC_Parts[2], out C_versionRevision)) return false;

                if (C_versionMajor > DBversionMajor ||
                    (C_versionMajor == DBversionMajor && C_versionMinor > DBversionMinor) ||
                    (C_versionMajor == DBversionMajor && C_versionMinor == DBversionMinor && C_versionRevision > DBversionRevision))
                    NeedsUpdate = true;
            }

            if (NeedsUpdate)
            {
                this.buttonPostgresUpdate.Visible = true;
                this.buttonPostgresUpdate.Text = "Update to version " + DiversityCollection.Properties.Settings.Default.PostgresCacheDBVersion;
            }
            else
            {
                this.buttonPostgresUpdate.Visible = false;
            }
            this.labelPostgresVersion.Text = "Version: " + Version;

            return NeedsUpdate;
        }

        private void UpdatePostgresDatabase()
        {
            DiversityWorkbench.PostgreSQL.Database DB = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase();
            string DatabaseCurrentVersion = DB.Version();
            string DatabaseFinalVersion = DiversityCollection.Properties.Settings.Default.CacheDBV1;
            DatabaseFinalVersion = DatabaseFinalVersion.Replace(".", "").Replace("/", "");

            try
            {
                // check resouces for update scripts
                System.Collections.Generic.Dictionary<string, string> Versions = new Dictionary<string, string>();
                System.Resources.ResourceManager rm = Properties.Resources.ResourceManager;
                System.Resources.ResourceSet rs = rm.GetResourceSet(new System.Globalization.CultureInfo("en-US"), true, true);
                if (rs != null)
                {
                    System.Collections.IDictionaryEnumerator de = rs.GetEnumerator();
                    while (de.MoveNext() == true)
                    {
                        if (de.Entry.Value is string)
                        {
                            if (de.Key.ToString().StartsWith("DiversityCollectionCacheUpdatePG_"))
                            {
                                Versions.Add(de.Key.ToString(), de.Value.ToString());
                            }
                        }
                    }
                }

                if (Versions.Count > 0)
                {
                    Npgsql.NpgsqlConnection con = new NpgsqlConnection(DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());
                    DiversityWorkbench.FormUpdateDatabase f =
                        new DiversityWorkbench.FormUpdateDatabase(
                            DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name,
                            DiversityCollection.Properties.Settings.Default.PostgresCacheDBVersion,
                            con,
                            Versions,
                            System.Windows.Forms.Application.StartupPath + "\\DiversityCollection.chm");
                    f.ForPostgres = true;
                    f.ShowInTaskbar = true;
                    f.Width = this.Width + 10;
                    f.ShowDialog();
                    if (f.Reconnect)
                        this.setDatabase();
                    string Version = "";
                    this.tableLayoutPanelPostgresAdminProjects.Enabled = !this.PostgresDatabaseNeedsUpdate(ref Version);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Upgrade resources are missing");
                }
            }
            catch (System.Exception ex)
            { }
        }

        #endregion

        #region Roles

        private void initPgAdminRoles()
        {
            try
            {
                string SQL = "SELECT * FROM pg_roles order by rolname;";
                System.Data.DataTable dtRoles = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtRoles, ref Message);
                //Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());// .Postgres.PostgresConnection().ConnectionString);
                //ad.Fill(dtRoles);
                this.listBoxPgAdminRoles.DataSource = dtRoles;
                this.listBoxPgAdminRoles.DisplayMember = "rolname";
                this.listBoxPgAdminRoles.ValueMember = "rolname";
            }
            catch (System.Exception ex)
            { }
        }

        private void listBoxPgAdminRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            string Role = "";
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPgAdminRoles.SelectedItem;
            Role = R["rolname"].ToString();
            string SQL = "SELECT table_schema, table_name, privilege_type " +
                "FROM information_schema.role_table_grants  " +
                "WHERE grantee='" + Role + "' " +
                "order by table_schema, table_name";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
            //Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());// .Postgres.PostgresConnection().ConnectionString);
            //ad.Fill(dt);
            this.dataGridViewPgAdminRoleGrants.DataSource = dt;
        }
        
        private void toolStripButtonPgAdminRoleAdministration_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.PostgreSQL.FormRoleAdministration f = new DiversityWorkbench.PostgreSQL.FormRoleAdministration();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region User

        private void initPostgresUser()
        {
            try
            {
                string SQL = "SELECT * FROM pg_roles order by rolname;";
                System.Data.DataTable dtRoles = new DataTable();
                string Message = "";
                DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dtRoles, ref Message);
                //Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.Postgres.PostgresConnection().ConnectionString);
                //ad.Fill(dtRoles);
                this.listBoxPostgresUser.DataSource = dtRoles;
                this.listBoxPostgresUser.DisplayMember = "rolname";
                this.listBoxPostgresUser.ValueMember = "rolname";
            }
            catch (System.Exception ex)
            { }
        }

        private void listBoxPostgresUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string User = "";
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPostgresUser.SelectedItem;
            User = R["rolname"].ToString();
            string SQL = "SELECT table_schema, table_name, privilege_type " +
                "FROM information_schema.role_table_grants  " +
                "WHERE grantee='" + User + "' " +
                "order by table_schema, table_name";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
            //Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.Postgres.PostgresConnection().ConnectionString);
            //ad.Fill(dt);
            this.dataGridViewPostgresUser.DataSource = dt;
        }

        private void toolStripButtonPostgresUserAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.FormGetString f = new DiversityWorkbench.FormGetString("New role", "Please enter the name of the new role", "");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
            {
                char x = '"';
                string Database = f.String.Replace("\"", "").Replace("'", "");
                string SQL = "CREATE ROLE " + x + f.String + x +
                    "NOSUPERUSER INHERIT NOCREATEDB NOCREATEROLE NOREPLICATION;";
                string Message = "";
                if (!DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL, ref Message))// .Postgres.PostgresExecuteSqlNonQuery(SQL, ref Message))
                    System.Windows.Forms.MessageBox.Show(Message);
            }
        }

        private void toolStripButtonPostgresUserDelete_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonPostgresUserPassword_Click(object sender, EventArgs e)
        {
            string User = "";
            bool OK = true;
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPostgresUser.SelectedItem;
            User = R["rolname"].ToString();
            DiversityWorkbench.Forms.FormGetPassword f = new DiversityWorkbench.Forms.FormGetPassword(User);
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.Password().Length > 0)
            {
                try
                {
                    string SQL = "ALTER ROLE " + User + " WITH PASSWORD '" + f.Password() + "';";
                    if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))// .Postgres.PostgresExecuteSqlNonQuery(SQL);
                        System.Windows.Forms.MessageBox.Show("Password set");
                    else OK = false;
                }
                catch (System.Exception ex)
                {
                    OK = false;
                }
                if (!OK)
                    System.Windows.Forms.MessageBox.Show("Setting password failed");
            }
        }

        private void toolStripButtonPostgresRoleAdministration_Click(object sender, EventArgs e)
        {
            try
            {
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length == 0)// .Postgres.PostgresConnection() == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please connect to database server");
                    return;
                }
                if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)// .Postgres.PostgresConnection().ConnectionString.Length > 0)
                {
                    DiversityWorkbench.PostgreSQL.FormRoleAdministration f = new DiversityWorkbench.PostgreSQL.FormRoleAdministration();
                    f.ShowDialog();
                }
                else System.Windows.Forms.MessageBox.Show("Please connect to a postgres database server");
            }
            catch(System.Exception ex)
            { }
        }

        #endregion

        #region Tables

        private void initPGtables()
        {
            try
            {
                this.dataGridViewPGtable.DataSource = null;
                string SQL = "select table_name from information_schema.tables P " +
                    "where P.table_type = 'BASE TABLE' " +
                    "and P.table_schema = 'public'";
                Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString()); // .Postgres.PostgresConnection().ConnectionString);
                System.Data.DataTable dt = new DataTable();
                ad.Fill(dt);
                this.listBoxPGtables.DataSource = dt;
                this.listBoxPGtables.DisplayMember = "table_name";
                this.listBoxPGtables.ValueMember = "table_name";
            }
            catch(System.Exception ex)
            { }
        }

        private void listBoxPGtables_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPGtables.SelectedItem;
                string SQL = "select * from public.\"" + R[0].ToString() + "\"";
                Npgsql.NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString());// .Postgres.PostgresConnection().ConnectionString);
                System.Data.DataTable dt = new DataTable();
                ad.Fill(dt);
                this.dataGridViewPGtable.DataSource = dt;
            }
            catch(System.Exception ex)
            { }
        }
        
        #endregion

        #region Projects

        private string _CurrentProject;

        private System.Data.DataTable _DtPostgresProjectsAvailable;

        public void initPostgresAdminProjectLists()
        {
            try
            {
                DiversityCollection.CacheDatabase.Project.ResetProjects();
                int i;
                try
                {
                    if (DiversityCollection.CacheDatabase.Project.Projects != null)// this.DtPostgresEstablishedProjects.Rows.Count;
                        i = DiversityCollection.CacheDatabase.Project.Projects.Count;
                }
                catch (System.Exception ex) { }
                this._DtPostgresProjectsAvailable = new DataTable();
                this._DtPostgresProjectsAvailable.Columns.Add("Project", typeof(string));
                this._DtPostgresProjectsAvailable.Columns.Add("ProjectID", typeof(int));
                this.panelPostgresEstablishedProjects.Controls.Clear();
                this.ResetPackages();
                foreach (System.Data.DataRow R in this._DtProjectPublished.Rows)
                {
                    int ProjectID;
                    bool Available = false;
                    if (int.TryParse(R["ProjectID"].ToString(), out ProjectID))
                    {
                        if (DiversityCollection.CacheDatabase.Project.Projects.ContainsKey(ProjectID))
                        {
                            DiversityCollection.CacheDatabase.UserControlPostgresProject PP = new UserControlPostgresProject(ProjectID, DiversityCollection.CacheDatabase.Project.Projects[ProjectID].Name, this);
                            this.panelPostgresEstablishedProjects.Controls.Add(PP);
                            PP.Dock = DockStyle.Top;
                            PP.BringToFront();
                            Available = true;
                        }
                    }
                    if (!Available)
                    {
                        System.Data.DataRow Rn = this._DtPostgresProjectsAvailable.NewRow();
                        Rn["Project"] = R["Project"];
                        Rn["ProjectID"] = R["ProjectID"];
                        this._DtPostgresProjectsAvailable.Rows.Add(Rn);
                    }
                }
                this.listBoxPostgresProjectsAvailable.DataSource = this._DtPostgresProjectsAvailable;
                this.listBoxPostgresProjectsAvailable.DisplayMember = "Project";
                this.listBoxPostgresProjectsAvailable.ValueMember = "ProjectID";
            }
            catch (System.Exception ex)
            { }
        }

        //private void PostgresEstablishProject(string ProjectName, int ProjectID)
        //{
        //    string SQL = "CREATE SCHEMA  \"" + ProjectName + "\"" +
        //        "AUTHORIZATION \"CacheAdmin\";";
        //    DiversityWorkbench.Postgres.PostgresExecuteSqlNonQuery(SQL);

        //    SQL = "ALTER DEFAULT PRIVILEGES IN SCHEMA \"" + ProjectName + "\"" +
        //        "GRANT EXECUTE ON FUNCTIONS " +
        //        "TO \"CacheUser\";";
        //    DiversityWorkbench.Postgres.PostgresExecuteSqlNonQuery(SQL);

        //    SQL = "ALTER DEFAULT PRIVILEGES IN SCHEMA \"" + ProjectName + "\"" +
        //        "GRANT SELECT ON TABLES " +
        //        "TO \"CacheUser\";";
        //    DiversityWorkbench.Postgres.PostgresExecuteSqlNonQuery(SQL);

        //    SQL = "CREATE OR REPLACE FUNCTION \"" + ProjectName + "\".version() " +
        //        "RETURNS integer AS " +
        //        "$BODY$ " +
        //        "declare " +
        //        "v integer; " +
        //        "BEGIN " +
        //        "SELECT 0 into v; " +
        //        "RETURN v; " +
        //        "END; " +
        //        "$BODY$ " +
        //        "LANGUAGE plpgsql STABLE " +
        //        "COST 100; " +
        //        "ALTER FUNCTION \"" + ProjectName + "\".version() " +
        //        "OWNER TO \"CacheAdmin\"; " +
        //        "GRANT EXECUTE ON FUNCTION \"" + ProjectName + "\".version() TO \"CacheAdmin\"; " +
        //        "GRANT EXECUTE ON FUNCTION \"" + ProjectName + "\".version() TO \"CacheUser\"";
        //    DiversityWorkbench.Postgres.PostgresExecuteSqlNonQuery(SQL);

        //    SQL = "CREATE OR REPLACE FUNCTION \"" + ProjectName + "\".projectid() " +
        //        "RETURNS integer AS " +
        //        "$BODY$ " +
        //        "declare " +
        //        "v integer; " +
        //        "BEGIN " +
        //        "SELECT " + ProjectID.ToString() + " into v; " +
        //        "RETURN v; " +
        //        "END; " +
        //        "$BODY$ " +
        //        "LANGUAGE plpgsql STABLE " +
        //        "COST 100; " +
        //        "ALTER FUNCTION \"" + ProjectName + "\".projectid() " +
        //        "OWNER TO \"CacheAdmin\"; " +
        //        "GRANT EXECUTE ON FUNCTION \"" + ProjectName + "\".projectid() TO \"CacheAdmin\"; " +
        //        "GRANT EXECUTE ON FUNCTION \"" + ProjectName + "\".projectid() TO \"CacheUser\"";
        //    DiversityWorkbench.Postgres.PostgresExecuteSqlNonQuery(SQL);
        //}

        //private void UpdateCheckPostgresProject(string Project)
        //{
        //    string SQL = "select \"" + Project + "\".version()";
        //    int version = int.Parse(DiversityWorkbench.Postgres.PostgresExecuteSqlSkalar(SQL));
        //    if (version < DiversityCollection.Properties.Settings.Default.PostgresProjectVersion)
        //    {

        //    }
        //}

        private void buttonPostgresProjectsEstablish_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxPostgresProjectsAvailable.SelectedItem;
            if (this.PostgresEstablishProject(R))
                this.initPostgresAdminProjectLists();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private bool PostgresEstablishProject(System.Data.DataRowView R)
        {
            bool OK = true;
            System.Data.DataRow[] rr = DiversityCollection.CacheDatabase.Project.DtProjects().Select("Project = '" + R["Project"].ToString() + "'"); // this.DtPostgresEstablishedProjects.Select("Project = '" + R["Project"].ToString() + "'");
            if (rr.Length == 0)
            {
                if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().CreateSchema(R["Project"].ToString(), int.Parse(R["ProjectID"].ToString())))
                {
                    DiversityCollection.CacheDatabase.UserControlPostgresProject PP = new UserControlPostgresProject(int.Parse(R["ProjectID"].ToString()), R["Project"].ToString(), this);
                    this.panelPostgresEstablishedProjects.Controls.Add(PP);
                    PP.Dock = DockStyle.Top;
                    PP.BringToFront();
                    PP.PostgresEstablishProject();
                    PP.UpdateCheckPostgresProject();
                }
            }
            else
            {
                OK = false;
            }
            return OK;
        }

        //private void buttonPostgresProjectsRemove_Click(object sender, EventArgs e)
        //{
        //    string Project = "";
        //    string SQL = "DROP SCHEMA \"" + Project + "\" CASCADE;";
        //    if (DiversityWorkbench.PostgreSQL.Connection.SqlExecuteNonQuery(SQL))// .Postgres.PostgresExecuteSqlNonQuery(SQL))
        //        this.initPostgresAdminProjectLists();
        //}

        //private void listBoxPostgresProjectsEstablished_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //string Project = this.listBoxPostgresProjectsEstablished.SelectedItem.ToString();
        //    string SQL = "SELECT grantee, table_name, privilege_type " +
        //        "FROM information_schema.role_table_grants  " +
        //        "WHERE table_schema='" + PostgresSelectedProject + "' " +
        //        "order by grantee, table_name";
        //    System.Data.DataTable dt = new DataTable();
        //    NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.Postgres.PostgresConnection().ConnectionString);
        //    ad.Fill(dt);
        //    this.dataGridViewPostgresProjectPermissions.DataSource = dt;
        //}

        //public static string PostgresSelectedProject = "";

        public void setPostgresProject(string Project, bool NeedsUpdate)
        {
            this._CurrentProject = Project;
            string SQL = "SELECT grantee, table_name, privilege_type " +
                "FROM information_schema.role_table_grants  " +
                "WHERE table_schema='" + Project + "' " +
                "order by grantee, table_name";
            System.Data.DataTable dt = new DataTable();
            string Message = "";
            DiversityWorkbench.PostgreSQL.Connection.SqlFillTable(SQL, ref dt, ref Message);
            //NpgsqlDataAdapter ad = new NpgsqlDataAdapter(SQL, DiversityWorkbench.Postgres.PostgresConnection().ConnectionString);
            //ad.Fill(dt);
            this.dataGridViewPostgresProjectPermissions.DataSource = dt;
            //this.labelPostgresProjectPermissions.Text = "Permissions for project " + Project;
            if (NeedsUpdate)
                this.ResetPackages();
            else
                this.setPackages(Project);
        }

        #region Packages

        private void ResetPackages()
        {
            this.tabControlPgProjects.Enabled = false;
            this.listBoxPostgresPackagesAvailable.Items.Clear();
            this.panelPostgresProjectPackages.Controls.Clear();
        }

        private void setPackages(string Project)
        {
            this.tabControlPgProjects.Enabled = true;
            this.labelProjectPackages.Visible = true;
            this.labelProjectPackages.Text = Project;
            this.panelPostgresProjectPackages.Controls.Clear();
            this.listBoxPostgresPackagesAvailable.Items.Clear();

            System.Collections.Generic.Dictionary<Package.Pack, string> Packages = DiversityCollection.CacheDatabase.Package.Packages();
            System.Collections.Generic.List<string> AvailablePackages = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<Package.Pack, string> KV in Packages)
                AvailablePackages.Add(KV.Key.ToString());
            System.Data.DataTable dtPackages = DiversityCollection.CacheDatabase.Project.GetProject(Project).dtPackage();
            foreach (System.Data.DataRow R in dtPackages.Rows)
            {
                DiversityCollection.CacheDatabase.Packages.UserControlPackage U = new Packages.UserControlPackage(DiversityCollection.CacheDatabase.Project.GetProject(Project), R[0].ToString(), this);
                this.panelPostgresProjectPackages.Controls.Add(U);
                U.Dock = DockStyle.Top;
                U.BringToFront();
                if (AvailablePackages.Contains(R[0].ToString()))
                    AvailablePackages.Remove(R[0].ToString());
            }
            foreach (string s in AvailablePackages)
                this.listBoxPostgresPackagesAvailable.Items.Add(s);
        }

        private void buttonPostgresPackageEstablish_Click(object sender, EventArgs e)
        {
            if (this.listBoxPostgresPackagesAvailable.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a package");
                return;
            }
            try
            {
                string Package = this.listBoxPostgresPackagesAvailable.SelectedItem.ToString();
                Package.Pack Pack = CacheDatabase.Package.Pack.ABCD;
                switch (Package)
                {
                    case "FloraRaster":
                        Pack = CacheDatabase.Package.Pack.FloraRaster;
                        break;
                }
                DiversityCollection.CacheDatabase.Project.GetProject(this._CurrentProject).EstablishPackage(Pack, DiversityCollection.CacheDatabase.Package.Packages()[Pack]);
                this.setPackages(this._CurrentProject);
            }
            catch (System.Exception ex)
            {
            }
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
        }
        
        #endregion  
     
        #endregion

        #endregion
        
        #endregion

        #region Overview
        
        private void initOverview()
        {
            try
            {
                this.labelOverviewSourceDB.Text = DiversityWorkbench.Settings.DatabaseName;
                this.labelOverviewCacheDB.Text = DiversityCollection.CacheDatabase.CacheDB.DatabaseName;
                if (DiversityWorkbench.PostgreSQL.Connection.Databases().Count > 0)
                {
                    this.labelOverviewPostgresDB.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                    this.buttonOverviewPostgresDBConnect.Visible = false;
                }
                else
                    this.buttonOverviewPostgresDBConnect.Visible = true;
                this.panelOverviewCache.Controls.Clear();
                this.panelOverviewPostgres.Controls.Clear();
                this.panelOverviewSource.Controls.Clear();
                foreach (System.Data.DataRow R in this._DtProjectPublished.Rows)
                {
                    this.OverviewAddProject(R);
                    this.OverviewAddProjectCacheDB(R);
                    this.OverviewAddProjectPostgres(R);
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
            }
        }

        public void initOverviewCacheDB()
        {
            try
            {
                this.panelOverviewCache.Controls.Clear();
                foreach (System.Data.DataRow R in this._DtProjectPublished.Rows)
                {
                    this.OverviewAddProjectCacheDB(R);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void initOverviewPostgres()
        {
            try
            {
                this.panelOverviewPostgres.Controls.Clear();
                foreach (System.Data.DataRow R in this._DtProjectPublished.Rows)
                {
                    this.OverviewAddProjectPostgres(R);
                    Application.DoEvents();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void OverviewAddProject(System.Data.DataRow R)
        {
            try
            {
                string SQL = "SELECT COUNT(*) FROM CollectionProject WHERE ProjectID = " + int.Parse(R["ProjectID"].ToString());
                int Nr = int.Parse(DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL));
                SQL = "SELECT    CONVERT(varchar(20), S.LogUpdatedWhen, 120)  AS LastUpdate " +
                    "FROM CollectionSpecimen AS S INNER JOIN " +
                    "CollectionProject P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE P.ProjectID = " + int.Parse(R["ProjectID"].ToString());
                string LastUpdate = DiversityWorkbench.FormFunctions.SqlExecuteScalar(SQL);
                System.DateTime DT = System.DateTime.Parse(LastUpdate);
                string Project = R["Project"].ToString();
                DiversityCollection.CacheDatabase.UserControlOverviewProject U = new UserControlOverviewProject(Project, Nr, DT);
                this.panelOverviewSource.Controls.Add(U);
                U.Dock = DockStyle.Top;
                U.BringToFront();
            }
            catch(System.Exception ex)
            {

            }
        }

        private void OverviewAddProjectCacheDB(System.Data.DataRow R)
        {
            //string SQL = "SELECT COUNT(*) FROM CollectionProject P, CollectionSpecimenCache AS S " +
            //    "WHERE S.CollectionSpecimenID = P.CollectionSpecimenID  AND ProjectID = " + int.Parse(R["ProjectID"].ToString());
            //int Nr = int.Parse(DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL));
            //SQL = "SELECT    CONVERT(varchar(20), S.LogInsertedWhen, 126)  AS LastUpdate " +
            //    "FROM CollectionSpecimenCache AS S INNER JOIN " +
            //    "CollectionProject P ON S.CollectionSpecimenID = P.CollectionSpecimenID " +
            //    "WHERE P.ProjectID = " + int.Parse(R["ProjectID"].ToString());
            //string LastUpdate = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
            //System.DateTime DT = System.DateTime.Parse(LastUpdate);
            string Project = R["Project"].ToString();
            DiversityCollection.CacheDatabase.UserControlOverviewProject U = new UserControlOverviewProject(Project, int.Parse(R["ProjectID"].ToString()), UserControlOverviewProject.DatabaseManagementSystem.MSSQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB, this);
            this.panelOverviewCache.Controls.Add(U);
            //U.TransferAll(true);
            //if (this.panelOverviewCache.Controls.Count > 1)
                U.TransferButtonIsVisible(false);
            U.Dock = DockStyle.Top;
            U.BringToFront();
        }

        private void OverviewAddProjectPostgres(System.Data.DataRow R)
        {
            //int Nr = 0;
            //string LastUpdate = "";
            //System.DateTime DT = System.DateTime.Now;
            //try
            //{
            //    if (DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString().Length > 0)// .Postgres.PostgresConnection().ConnectionString.Length > 0)
            //    {
            //        string SQL = "SELECT COUNT(*) FROM \"CollectionSpecimenCache\" AS S " +
            //            "WHERE S.\"ProjectID\" = " + int.Parse(R["ProjectID"].ToString());
            //        //Nr = int.Parse(DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL));// .Postgres.PostgresExecuteSqlSkalar(SQL));
            //        //SQL = "SELECT S.\"LogInsertedWhen\" AS LastUpdate " +
            //        //    "FROM \"CollectionSpecimenCache\" " +
            //        //    "WHERE S.\"ProjectID\" = " + int.Parse(R["ProjectID"].ToString());
            //        //LastUpdate = DiversityWorkbench.PostgreSQL.Connection.SqlExecuteSkalar(SQL);// .Postgres.PostgresExecuteSqlSkalar(SQL);
            //        //System.DateTime.TryParse(LastUpdate, out DT);
            //        Project = ;
            //    }
            //}
            //catch(System.Exception ex)
            //{ }
            string Project = R["Project"].ToString();
            DiversityCollection.CacheDatabase.UserControlOverviewProject U = new UserControlOverviewProject(Project, int.Parse(R["ProjectID"].ToString()), UserControlOverviewProject.DatabaseManagementSystem.Postgres, DiversityWorkbench.PostgreSQL.Connection.DefaultConnectionString(), this);
            this.panelOverviewPostgres.Controls.Add(U);
            U.Dock = DockStyle.Top;
            U.BringToFront();
        }

        private void buttonOverviewPostgresDBConnect_Click(object sender, EventArgs e)
        {
            this.ConnectToPostgresDatabase();
            if (DiversityWorkbench.PostgreSQL.Connection.CurrentConnectionDisplayText().Length > 0)
            {
                this.labelOverviewPostgresDB.Text = DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name;
                this.buttonOverviewPostgresDBConnect.Visible = false;
            }
            else
                this.buttonOverviewPostgresDBConnect.Visible = true;
        }

        private void buttonOverviewTransferToCacheDB_Click(object sender, EventArgs e)
        {
            this.TransferDataToCacheDB();
            this.initOverviewCacheDB();
        }

        private void buttonOverviewTransferToPostgresDB_Click(object sender, EventArgs e)
        {
            if (DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase() == null || DiversityWorkbench.PostgreSQL.Connection.CurrentDatabase().Name.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Not connected to database");
                return;
            }

            foreach (System.Windows.Forms.Control C in this.panelOverviewPostgres.Controls)
            {
                DiversityCollection.CacheDatabase.UserControlOverviewProject U = (DiversityCollection.CacheDatabase.UserControlOverviewProject)C;
                U.StartTransfer();
            }
        }

        #endregion

        private void buttonCollectorIsAnonym_Click(object sender, EventArgs e)
        {

        }

        private void buttonCollectorNotAnonym_Click(object sender, EventArgs e)
        {

        }

        public void setPackages() { }

    }
}
