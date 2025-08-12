using DiversityWorkbench.DwbManual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class FormTransferFilter : Form
    {
        #region Parameter

        private string _Schema;
        private int _ProjectID;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterLocalisation;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterTaxonomicGroup;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterMaterialCategory;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterAnalysis;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEventProperty;
        private DiversityWorkbench.CollectionSpecimen _CollectionSpecimen;
        
        #endregion

        #region Construction and form

        public FormTransferFilter(string Schema, int ProjectID)
        {
            InitializeComponent();
            this._Schema = Schema;
            this._ProjectID = ProjectID;
            this.initForm();
        }

        private void initForm()
        {
#if !DEBUG
            //this.tabControlMain.TabPages.Remove(this.tabPageSpecimen);
#else
#endif
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                this.initSpecimenQuery();
                this.Text = "Filter settings for " + this._Schema.Replace("_", " ");
                string SQL = "SELECT CoordinatePrecision FROM ProjectPublished WHERE ProjectID = " + this._ProjectID.ToString();
                string Result = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Result.Length > 0)
                {
                    this.checkBoxCoordinatePrecision.Checked = true;
                    this.numericUpDownCoordinatePrecision.Value = decimal.Parse(Result);
                }
                else
                {
                    this.checkBoxCoordinatePrecision.Checked = false;
                    this.numericUpDownCoordinatePrecision.Value = 0;
                }
                this.SetLocalisationList();
                this.SetEventProperty();
                this.SetTaxonomicGroups();
                this.SetMaterialCategories();
                this.SetAnalysis();
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void initSpecimenQuery()
        {
            try
            {
                this.userControlQueryListSpecimen.setModeOfControl(DiversityWorkbench.UserControls.UserControlQueryList.Mode.Embedded);

                this.userControlQueryListSpecimen.TableColors = DiversityCollection.HierarchyNode.TableColors;
                this.userControlQueryListSpecimen.TableImageIndex = DiversityCollection.HierarchyNode.TableAndGroupImageIndex;
                this.userControlQueryListSpecimen.ImageList = DiversityCollection.Specimen.ImageList;

                if (DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.QueryConditionVisibility.Length > 0)
                    this.userControlQueryListSpecimen.QueryConditionVisiblity = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.QueryConditionVisibility;
                if (this._CollectionSpecimen == null)
                    this._CollectionSpecimen = new DiversityWorkbench.CollectionSpecimen(DiversityWorkbench.Settings.ServerConnection);
                if (DiversityWorkbench.Settings.ConnectionString.Length > 0)
                    this.userControlQueryListSpecimen.setQueryConditions(this._CollectionSpecimen.QueryConditions(), DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.QueryConditionVisibility.ToString());
                //this.userControlQueryListSpecimen.AllowOptimizing(true);
                DiversityWorkbench.UserControls.QueryDisplayColumn[] CC = this._CollectionSpecimen.QueryDisplayColumns();
                this.userControlQueryListSpecimen.SetQueryDisplayColumns(CC, "AccessionNumber", "CollectionSpecimen_Core2");
                this.userControlQueryListSpecimen.setQueryConditions("", "CollectionSpecimen_Core2", "");
                this.userControlQueryListSpecimen.ResetListOfBlockedIDs();
                this.userControlQueryListSpecimen.SetSqlForBlockedIDs("SELECT CollectionSpecimenID FROM CollectionSpecimenID_AvailableReadOnly");
                this.userControlQueryListSpecimen.initEmbedment("UPDATE P SET P.Restriction = '###' FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString(), "###", DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                string Filter = this.SpecimenFilter;
                if (Filter.Length > 0)
                {
                    this.userControlQueryListSpecimen.IsPredefinedQuery = true;
                    this.userControlQueryListSpecimen.setQueryConditions("", "CollectionSpecimen_Core2", Filter, "");
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private string SpecimenFilter
        {
            get
            {
                string SQL = "SELECT Restriction FROM ProjectPublished WHERE ProjectID = " + this._ProjectID.ToString();
                string Restriction = DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlSkalarInCacheDB(SQL);
                if (Restriction.Trim().Length > 0 && !Restriction.ToUpper().Trim().StartsWith("WHERE"))
                    Restriction = " WHERE " + Restriction;
                return Restriction;
            }
            //set
            //{
            //    string SQL = "UPDATE P SET P.Restriction = '" + value + "' FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();
            //    DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, true);
            //}
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }
        
#endregion

#region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

#endregion

#region Coordinate precision

        private void checkBoxCoordinatePrecision_Click(object sender, EventArgs e)
        {
            //System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            string Value = this.numericUpDownCoordinatePrecision.Value.ToString();
            if (!this.checkBoxCoordinatePrecision.Checked)
            {
                Value = "NULL";
                //RV["CoordinatePrecision"] = System.DBNull.Value;
                this.numericUpDownCoordinatePrecision.Enabled = false;
            }
            else
            {
                //RV["CoordinatePrecision"] = this.numericUpDownCoordinatePrecision.Value;
                this.numericUpDownCoordinatePrecision.Enabled = true;
            }

            string SQL = "UPDATE P SET P.CoordinatePrecision = " + Value + " FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();// + RV["ProjectID"].ToString();
            DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
        }

        private void numericUpDownCoordinatePrecision_Click(object sender, EventArgs e)
        {
            //System.Data.DataRowView RV = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
            //RV["CoordinatePrecision"] = this.numericUpDownCoordinatePrecision.Value;
            if (this.checkBoxCoordinatePrecision.Checked)
            {
                string SQL = "UPDATE P SET P.CoordinatePrecision = " + this.numericUpDownCoordinatePrecision.Value.ToString() + " FROM ProjectPublished P WHERE P.ProjectID = " + this._ProjectID.ToString();// +RV["ProjectID"].ToString();
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Coordinate precision was not selected for restriction");
            }
        }

#endregion

#region Localisation

        private void buttonLocalisationPublished_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectLocalisationNotPublished.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectLocalisationNotPublished.SelectedItem;
                string SQL = "INSERT INTO [" + this._Schema + "].CacheLocalisationSystem (LocalisationSystemID, DisplayText, Sequence, ParsingMethodName) " +
                    "VALUES (" + R["LocalisationSystemID"].ToString() + ", '" + R["DisplayText"].ToString() + "', CASE WHEN (SELECT MAX(Sequence) + 1 FROM [" + this._Schema + "].CacheLocalisationSystem) IS NULL THEN 1 ELSE (SELECT MAX(Sequence) + 1 FROM [" + this._Schema + "].CacheLocalisationSystem) END, '" + R["ParsingMethodName"].ToString() + "')";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetLocalisationList();
            }
            else
                System.Windows.Forms.MessageBox.Show("Nothing selected");
        }

        private void buttonLocalisationNotPublished_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectLocalisationPubished.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectLocalisationPubished.SelectedItem;
                string SQL = "DELETE L FROM [" + this._Schema + "].CacheLocalisationSystem L WHERE LocalisationSystemID = " + R["LocalisationSystemID"].ToString();
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetLocalisationList();
            }
            else
                System.Windows.Forms.MessageBox.Show("Nothing selected");
        }

        private void buttonLocalisationPublishedUp_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectLocalisationPubished.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectLocalisationPubished.SelectedItem;
                string SQL = "UPDATE L SET [Sequence] = (SELECT MIN(Sequence) - 1 FROM [" + this._Schema + "].CacheLocalisationSystem) FROM [" + this._Schema + "].CacheLocalisationSystem L WHERE LocalisationSystemID = " + R["LocalisationSystemID"].ToString();
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetLocalisationList();
            }
            else
                System.Windows.Forms.MessageBox.Show("Nothing selected");
        }

        private void buttonLocalisationPublishedDown_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectLocalisationPubished.SelectedItem != null)
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectLocalisationPubished.SelectedItem;
                string SQL = "UPDATE L SET [Sequence] = (SELECT MAX(Sequence) + 1 FROM [" + this._Schema + "].CacheLocalisationSystem) FROM [" + this._Schema + "].CacheLocalisationSystem L WHERE LocalisationSystemID = " + R["LocalisationSystemID"].ToString();
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetLocalisationList();
            }
            else
                System.Windows.Forms.MessageBox.Show("Nothing selected");
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
                    SQL = "SELECT LocalisationSystemID, DisplayText, Sequence, ParsingMethodName FROM [" + this._Schema + "].CacheLocalisationSystem ORDER BY Sequence ";
                    this._SqlDataAdapterLocalisation = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
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
                    Microsoft.Data.SqlClient.SqlDataAdapter adUn = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    adUn.Fill(this._DtLocalisationUnPublished);
                }
                this.listBoxProjectLocalisationPubished.DataSource = this._DtLocalisationPublished;
                this.listBoxProjectLocalisationPubished.DisplayMember = "DisplayText";
                this.listBoxProjectLocalisationPubished.ValueMember = "LocalisationSystemID";

                this.listBoxProjectLocalisationNotPublished.DataSource = this._DtLocalisationUnPublished;
                this.listBoxProjectLocalisationNotPublished.DisplayMember = "DisplayText";
                this.listBoxProjectLocalisationNotPublished.ValueMember = "LocalisationSystemID";
            }
            catch (System.Exception ex)
            {
                OK = false;
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            return OK;
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
            string SQL = "INSERT INTO [" + this._Schema + "].ProjectTaxonomicGroup (TaxonomicGroup) " +
                "VALUES ('" + Code + "')";
            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                this.SetTaxonomicGroups();
            else
                OK = false;
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
            string SQL = "DELETE T FROM [" + this._Schema + "].ProjectTaxonomicGroup T WHERE TaxonomicGroup = '" + R["TaxonomicGroup"].ToString() + "'";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetTaxonomicGroups();
        }

        private System.Data.DataTable _DtTaxonomicGroupPublished;
        private System.Data.DataTable _DtTaxonomicGroupNotPublished;

        private void SetTaxonomicGroups()
        {
            try
            {
                this.SetTaxonomicGroupsPublished();
                this._DtTaxonomicGroupNotPublished = new DataTable();
                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "";
                    string TaxonomicGroups = "";
                    foreach (System.Data.DataRow R in this._DtTaxonomicGroupPublished.Rows)
                    {
                        if (TaxonomicGroups.Length > 0) TaxonomicGroups += ", ";
                        TaxonomicGroups += "'" + R["TaxonomicGroup"].ToString() + "'";
                    }
                    SQL = "select distinct U.TaxonomicGroup " +
                        "from IdentificationUnit U, CollectionProject P " +
                        "where U.CollectionSpecimenID = P.CollectionSpecimenID " +
                        "and P.ProjectID = " + _ProjectID.ToString();
                    if (TaxonomicGroups.Length > 0)
                        SQL += " and U.TaxonomicGroup not in (" + TaxonomicGroups + ") ";
                    SQL += " ORDER BY U.TaxonomicGroup";
                    DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref this._DtTaxonomicGroupNotPublished, "");

                //SQL = "select distinct U.TaxonomicGroup " +
                //                        "from [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.IdentificationUnit U, [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionProject P " +
                //                        "where U.CollectionSpecimenID = P.CollectionSpecimenID " +
                //                        "and P.ProjectID = " + _ProjectID.ToString() + " " +
                //                        "and U.TaxonomicGroup COLLATE DATABASE_DEFAULT not in (SELECT TaxonomicGroup COLLATE DATABASE_DEFAULT " +
                //                        "FROM [" + this._Schema + "].ProjectTaxonomicGroup) " +
                //                        "ORDER BY U.TaxonomicGroup ";
                //                    Microsoft.Data.SqlClient.SqlDataAdapter adUn = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                //                    adUn.Fill(this._DtTaxonomicGroupNotPublished);

                }
                this.listBoxProjectTaxonomicGroupNotPublished.DataSource = this._DtTaxonomicGroupNotPublished;
                this.listBoxProjectTaxonomicGroupNotPublished.DisplayMember = "TaxonomicGroup";
                this.listBoxProjectTaxonomicGroupNotPublished.ValueMember = "TaxonomicGroup";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void SetTaxonomicGroupsPublished()
        {
            try
            {
                this._DtTaxonomicGroupPublished = new DataTable();
                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT P.TaxonomicGroup, RestrictToLinkedIdentifications FROM [" + this._Schema + "].ProjectTaxonomicGroup P ORDER BY P.TaxonomicGroup ";
                    this._SqlDataAdapterTaxonomicGroup = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterTaxonomicGroup.Fill(this._DtTaxonomicGroupPublished);
                }
                this.listBoxProjectTaxonomicGroupPublished.DataSource = this._DtTaxonomicGroupPublished;
                this.listBoxProjectTaxonomicGroupPublished.DisplayMember = "TaxonomicGroup";
                this.listBoxProjectTaxonomicGroupPublished.ValueMember = "TaxonomicGroup";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonProjectTaxonomicGroupTransferExisting_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataTable dt = new DataTable();
                string SQL = "SELECT U.TaxonomicGroup FROM IdentificationUnit AS U INNER JOIN " +
                    "CollectionProject AS P ON U.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "WHERE (P.ProjectID = " + this._ProjectID.ToString() + ") GROUP BY U.TaxonomicGroup";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);// DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
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

        private void checkBoxProjectTaxonomicGroupPublishedRestricted_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectTaxonomicGroupPublished.SelectedItem;
                R.BeginEdit();
                bool OK = false;
                string Value = R[1].ToString();
                if (!Boolean.TryParse(Value, out OK))
                    OK = true;
                else
                    OK = !OK;
                this.checkBoxProjectTaxonomicGroupPublishedRestricted.Checked = OK;
                string SQL = "UPDATE T SET RestrictToLinkedIdentifications = ";
                if (OK) SQL += "1"; else SQL += "0";
                SQL += " FROM [" + this._Schema + "].ProjectTaxonomicGroup T WHERE TaxonomicGroup = '" + R[0].ToString() + "'";
                DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL);
                R.EndEdit();
                int i = this.listBoxProjectTaxonomicGroupPublished.SelectedIndex;
                this.SetTaxonomicGroupsPublished();
                this.listBoxProjectTaxonomicGroupPublished.SelectedIndex = i;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void listBoxProjectTaxonomicGroupPublished_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectTaxonomicGroupPublished.SelectedItem;
                bool OK = false;
                if (Boolean.TryParse(R[1].ToString(), out OK))
                    this.checkBoxProjectTaxonomicGroupPublishedRestricted.Checked = OK;
                else
                    this.checkBoxProjectTaxonomicGroupPublishedRestricted.Checked = OK;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
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
                string SQL = "INSERT INTO [" + this._Schema + "].ProjectMaterialCategory " +
                    "(MaterialCategory) " +
                    "select distinct S.MaterialCategory " +
                    "from " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionSpecimenPart S, " + DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionProject P " +
                    "where S.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "and P.ProjectID = " + _ProjectID.ToString() + " " +
                    "and S.MaterialCategory not in (SELECT MaterialCategory " +
                    "FROM [" + this._Schema + "].ProjectMaterialCategory) " +
                    "ORDER BY S.MaterialCategory ";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetMaterialCategories();
            }
            catch (System.Exception ex)
            {
            }

        }

        private bool InsertMaterialCategoryForProject(string Code)
        {
            bool OK = true;
            string SQL = "INSERT INTO [" + this._Schema + "].ProjectMaterialCategory (MaterialCategory) " +
                "VALUES ('" + Code + "')";
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
                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT P.MaterialCategory FROM [" + this._Schema + "].ProjectMaterialCategory P ORDER BY P.MaterialCategory ";
                    this._SqlDataAdapterMaterialCategory = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterMaterialCategory.Fill(this._DtMaterialPublished);
                    SQL = "";

                    // Markus 14.4.25: COLLATE Database_Default included #87
                    SQL = "select distinct S.MaterialCategory " +
                          "from [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionSpecimenPart S, [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionProject P " +
                          "where S.CollectionSpecimenID = P.CollectionSpecimenID " +
                          "and P.ProjectID = " + _ProjectID.ToString() + " " +
                          "and S.MaterialCategory COLLATE Database_Default not in (SELECT MaterialCategory COLLATE Database_Default " +
                          "FROM [" + this._Schema + "].ProjectMaterialCategory) " +
                          "ORDER BY S.MaterialCategory ";

                    Microsoft.Data.SqlClient.SqlDataAdapter adUn = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
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
            try
            {
                if (this.listBoxProjectMaterialCategoryNotPublished.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the material category that should be published");
                    return;
                }
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectMaterialCategoryNotPublished.SelectedItem;
                this.InsertMaterialCategoryForProject(R["MaterialCategory"].ToString());
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonProjectMaterialCategoryWithhold_Click(object sender, EventArgs e)
        {
            if (this.listBoxProjectMaterialCategoryPublished.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the material category that should not be published");
                return;
            }
            System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxProjectMaterialCategoryPublished.SelectedItem;
            string SQL = "DELETE T FROM [" + this._Schema + "].ProjectMaterialCategory T WHERE MaterialCategory = '" + R["MaterialCategory"].ToString() + "'";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                this.SetMaterialCategories();
        }

#endregion

#region Analysis

        private System.Data.DataTable _DtAnalysisPublished;
        private System.Data.DataTable _DtAnalysisNotPublished;

        private bool InsertAnalysisForProject(int AnalysisID)
        {
            bool OK = true;
            string SQL = "INSERT INTO [" + this._Schema + "].ProjectAnalysis (AnalysisID) " +
                "VALUES (" + AnalysisID.ToString() + ")";
            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                this.SetAnalysis();
            else
                OK = false;
            return OK;
        }

        private void SetAnalysis()
        {
            try
            {
                this._DtAnalysisNotPublished = new DataTable();
                this._DtAnalysisPublished = new DataTable();
                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT A.AnalysisID, A.DisplayText FROM [" + this._Schema + "].ProjectAnalysis P, [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.Analysis A " +
                        "WHERE A.AnalysisID = P.AnalysisID ORDER BY A.DisplayText ";
                    this._SqlDataAdapterAnalysis = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterAnalysis.Fill(this._DtAnalysisPublished);
                    SQL = "";

                    SQL = "select distinct A.AnalysisID, A.DisplayText " +
                        "from [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.IdentificationUnitAnalysis U" +
                        ", [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionProject P " +
                        ", [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.Analysis A " +
                        "where U.CollectionSpecimenID = P.CollectionSpecimenID " +
                        "and P.ProjectID = " + _ProjectID.ToString() + " " +
                        "and A.AnalysisID = U.AnalysisID " +
                        "and A.AnalysisID not in (SELECT AnalysisID " +
                        "FROM [" + this._Schema + "].ProjectAnalysis) " +
                        "ORDER BY A.DisplayText ";

                    // Markus 31.1.2022 - Bayernflora DB braucht 2 min fuer Abfrage
                    Microsoft.Data.SqlClient.SqlDataAdapter adUn = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    adUn.SelectCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
                    adUn.Fill(this._DtAnalysisNotPublished);
                }
                this.listBoxAnalysisPublished.DataSource = this._DtAnalysisPublished;
                this.listBoxAnalysisPublished.DisplayMember = "DisplayText";
                this.listBoxAnalysisPublished.ValueMember = "AnalysisID";

                this.listBoxAnalysisNotPublished.DataSource = this._DtAnalysisNotPublished;
                this.listBoxAnalysisNotPublished.DisplayMember = "DisplayText";
                this.listBoxAnalysisNotPublished.ValueMember = "AnalysisID";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonAnalysisPublish_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxAnalysisNotPublished.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the analysis that should be published");
                    return;
                }
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxAnalysisNotPublished.SelectedItem;
                this.InsertAnalysisForProject(int.Parse(R["AnalysisID"].ToString()));
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void buttonAnalysisBlock_Click(object sender, EventArgs e)
        {
            if (this.listBoxAnalysisPublished.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the analysis that should not be published");
                return;
            }
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxAnalysisPublished.SelectedItem;
                string SQL = "DELETE T FROM [" + this._Schema + "].ProjectAnalysis T WHERE AnalysisID = " + R["AnalysisID"].ToString();
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetAnalysis();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void buttonAnalysisTransferExisting_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string SQL = "INSERT INTO [" + this._Schema + "].ProjectAnalysis " +
                    "(AnalysisID) " +
                    "select distinct U.AnalysisID " +
                    "from " + DiversityWorkbench.Settings.DatabaseName + ".dbo.IdentificationUnitAnalysis U, " + 
                    DiversityWorkbench.Settings.DatabaseName + ".dbo.CollectionProject P " +
                    "where U.CollectionSpecimenID = P.CollectionSpecimenID " +
                    "and P.ProjectID = " + _ProjectID.ToString() + " " +
                    "and U.AnalysisID not in (SELECT AnalysisID " +
                    "FROM [" + this._Schema + "].ProjectAnalysis) " +
                    "ORDER BY U.AnalysisID ";
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetAnalysis();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        #endregion

        #region EventProperty

        private System.Data.DataTable _DtEventPropertyPublished;
        private System.Data.DataTable _DtEventPropertyNotPublished;

        private bool InsertEventPropertyForProject(int PropertyID)
        {
            bool OK = true;
            string SQL = "INSERT INTO [" + this._Schema + "].ProjectEventProperty (PropertyID) " +
                "VALUES (" + PropertyID.ToString() + ")";
            string Message = "";
            if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL, ref Message))
                this.SetEventProperty();
            else
                OK = false;
            return OK;
        }

        private void SetEventProperty()
        {
            try
            {
                this._DtEventPropertyNotPublished = new DataTable();
                this._DtEventPropertyPublished = new DataTable();
                string SQL = "";
                if (DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB.Length > 0)
                {
                    SQL = "SELECT P.PropertyID, P.DisplayText FROM [" + this._Schema + "].ProjectEventProperty E, [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.Property P " +
                        "WHERE E.PropertyID = P.PropertyID ORDER BY P.DisplayText ";
                    this._SqlDataAdapterEventProperty = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    this._SqlDataAdapterEventProperty.Fill(this._DtEventPropertyPublished);
                    SQL = "";

                    SQL = "select distinct P.PropertyID, P.DisplayText " +
                        "from [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionEventProperty E " +
                        ", [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionSpecimen S " +
                        ", [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionProject CP " +
                        ", [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.Property P " +
                        "where S.CollectionSpecimenID = CP.CollectionSpecimenID " +
                        "and E.CollectionEventID = S.CollectionEventID " +
                        "and CP.ProjectID = " + _ProjectID.ToString() + " " +
                        "and E.PropertyID = P.PropertyID " +
                        "and P.PropertyID not in (SELECT PropertyID " +
                        "FROM [" + this._Schema + "].ProjectEventProperty) " +
                        "ORDER BY P.DisplayText ";

                    Microsoft.Data.SqlClient.SqlDataAdapter adUn = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityCollection.CacheDatabase.CacheDB.ConnectionStringCacheDB);
                    adUn.SelectCommand.CommandTimeout = DiversityCollection.CacheDatabase.CacheDBsettings.Default.TimeoutCacheDB;
                    adUn.Fill(this._DtEventPropertyNotPublished);
                }
                this.listBoxEventPropertyPublished.DataSource = this._DtEventPropertyPublished;
                this.listBoxEventPropertyPublished.DisplayMember = "DisplayText";
                this.listBoxEventPropertyPublished.ValueMember = "PropertyID";

                this.listBoxEventPropertyNotPublished.DataSource = this._DtEventPropertyNotPublished;
                this.listBoxEventPropertyNotPublished.DisplayMember = "DisplayText";
                this.listBoxEventPropertyNotPublished.ValueMember = "PropertyID";
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void buttonEventPropertyTransferExisiting_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string SQL = "INSERT INTO [" + this._Schema + "].ProjectEventProperty " +
                    "(PropertyID) " +
                    "select E.PropertyID " +
                        "from [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionEventProperty E " +
                        ", [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionSpecimen S " +
                        ", [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.CollectionProject CP " +
                        ", [" + DiversityWorkbench.Settings.DatabaseName + "].dbo.Property P " +
                        "where S.CollectionSpecimenID = CP.CollectionSpecimenID " +
                        "and E.CollectionEventID = S.CollectionEventID " +
                        "and CP.ProjectID = " + _ProjectID.ToString() + " " +
                        "and E.PropertyID = P.PropertyID " +
                        "and P.PropertyID not in (SELECT PropertyID " +
                        "FROM [" + this._Schema + "].ProjectEventProperty) " +
                        "GROUP BY E.PropertyID ";

                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetEventProperty();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void buttonEventPropertyAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.listBoxEventPropertyNotPublished.SelectedItem == null)
                {
                    System.Windows.Forms.MessageBox.Show("Please select the event property that should be published");
                    return;
                }
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxEventPropertyNotPublished.SelectedItem;
                this.InsertEventPropertyForProject(int.Parse(R["PropertyID"].ToString()));
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private void buttonEventPropertyRemove_Click(object sender, EventArgs e)
        {
            if (this.listBoxEventPropertyPublished.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select the event property that should not be published");
                return;
            }
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                System.Data.DataRowView R = (System.Data.DataRowView)this.listBoxEventPropertyPublished.SelectedItem;
                string SQL = "DELETE T FROM [" + this._Schema + "].ProjectEventProperty T WHERE PropertyID = " + R["PropertyID"].ToString();
                if (DiversityCollection.CacheDatabase.CacheDB.ExecuteSqlNonQueryInCacheDB(SQL))
                    this.SetEventProperty();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

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
