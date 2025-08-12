using DiversityCollection.BayernFlora;
using DiversityCollection.Tasks.Taxa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace DiversityCollection.Tasks
{
    public partial class FormSettings : Form
    {

        #region Parameter
        //private System.Collections.Generic.List<IPM.TaxonSource> _taxonSources;

        private System.Collections.Generic.List<IPM.RecordingTarget> _recordingTargets;
        private System.Collections.Generic.Dictionary<IPM.RecordingTarget, bool> _SelectionHasChanged;

        #endregion

        #region Construction
        public FormSettings(string Message = "")
        {
            InitializeComponent();
            this.initForm(Message);
        }

        public FormSettings(System.Collections.Generic.List<IPM.RecordingTarget> recordingTargets)
        {
            InitializeComponent();
            this._recordingTargets = recordingTargets;
            string Message = "Select ";
            switch (recordingTargets[0])
            {
                case IPM.RecordingTarget.TrapPest:
                    Message += "pests";
                    break;
                case IPM.RecordingTarget.TrapBycatch:
                    Message += "bycatch";
                    break;
                case IPM.RecordingTarget.Beneficial:
                    Message += "beneficials";
                    break;
                default:
                    Message += "taxa";
                    break;
            }
            Message += " from the list";
            this.initForm(Message);
        }

        #endregion

        #region SelectionHasChanged

        public bool SelectionHasChanged(IPM.RecordingTarget target)
        {
            if (this._SelectionHasChanged == null)
                return false;
            else
            {
                if (this._SelectionHasChanged.ContainsKey(target))
                    return this._SelectionHasChanged[target];
                else
                    return false;
            }
            return false;
        }

        private void SetSelectionHasChanged(IPM.RecordingTarget target, bool Changed = true)
        {
            if (this._SelectionHasChanged == null)
                this._SelectionHasChanged = new Dictionary<IPM.RecordingTarget, bool>();
            if (!_SelectionHasChanged.ContainsKey(target))
                _SelectionHasChanged.Add(target, Changed);
            else if (_SelectionHasChanged[target] != Changed)
                _SelectionHasChanged[target] = Changed;
        }

        private void dataGridViewPests_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                SetSelectionHasChanged(IPM.RecordingTarget.TrapPest);
            }
        }

        private void dataGridViewBycatch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                SetSelectionHasChanged(IPM.RecordingTarget.TrapBycatch);
            }
        }

        private void dataGridViewBeneficials_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                SetSelectionHasChanged(IPM.RecordingTarget.Beneficial);
            }
        }

        #endregion

        #region Form

        private void initForm(string Message = "")
        {
            this.SuspendLayout();
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                this.RemoveNotUsedTabs();
                if (Message.Length > 0)
                {
                    this.labelMessage.Text = Message;
                }
                this.labelMessage.Visible = Message.Length > 0;
                this.buttonOK.Visible = Message.Length > 0;

                this.initTaxonDefaultNameUriLists();

                this.initDatabaseControls();

                this.initTaxonTabs();

                this.initTaxaGroups();

                this.initDisplayOptions();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.ResumeLayout();
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void RemoveNotUsedTabs()
        {
            this.tabControlSettings.TabPages.Remove(this.tabPageSensors);
            this.tabControlSettings.TabPages.Remove(this.tabPageGroups);
            this.tabControlSettings.TabPages.Remove(this.tabPageSorting);

        }

        private void initTaxonDefaultNameUriLists()
        {
            if (Settings.Default.PestNameUris == null)
                Settings.Default.PestNameUris = new System.Collections.Specialized.StringCollection();
            if (Settings.Default.BeneficialNameUris == null)
                Settings.Default.BeneficialNameUris = new System.Collections.Specialized.StringCollection();
            if (Settings.Default.BycatchNameUris == null)
                Settings.Default.BycatchNameUris = new System.Collections.Specialized.StringCollection();

        }

        private void initTaxonTabs()
        {
            if (this._recordingTargets == null || 
                this._recordingTargets.Contains(IPM.RecordingTarget.TrapPest) ||
                this._recordingTargets.Contains(IPM.RecordingTarget.TransactionPest) ||
                this._recordingTargets.Contains(IPM.RecordingTarget.CollectionPest) ||
                this._recordingTargets.Contains(IPM.RecordingTarget.SpecimenPest))
            {
                this.labelPestList.Text = this.GetProject();
                this.labelTaxaProject.Text = labelPestList.Text;
                this.initPests();
            }
            else
                this.tabControlTaxa.TabPages.Remove(this.tabPagePests);

            if (this._recordingTargets == null || this._recordingTargets.Contains(IPM.RecordingTarget.Beneficial))
            {
                this.labelBeneficialsList.Text = this.GetProject(true, false);
                this.initBeneficials();
            }
            else
                this.tabControlTaxa.TabPages.Remove(this.tabPageBeneficials);

            if (this._recordingTargets == null || this._recordingTargets.Contains(IPM.RecordingTarget.TrapBycatch))
            {
                this.labelTaxaProject.Text = this.GetProject(false);
                this.initBycatch();
            }
            else
                this.tabControlTaxa.TabPages.Remove(this.tabPageBycatch);


            //if (this._taxonSources == null || this._taxonSources.Contains(IPM.TaxonSource.Pest))
            //{
            //    this.labelPestList.Text = this.GetProject();
            //    this.labelTaxaProject.Text = labelPestList.Text;
            //    this.initPests();
            //}
            //else
            //    this.tabControlTaxa.TabPages.Remove(this.tabPagePests);

            //if (this._taxonSources == null || this._taxonSources.Contains(IPM.TaxonSource.Beneficial))
            //{
            //    this.labelBeneficialsList.Text = this.GetProject(true, false);
            //    this.initBeneficials();
            //}
            //else
            //    this.tabControlTaxa.TabPages.Remove(this.tabPageBeneficials);

            //if (this._taxonSources == null || this._taxonSources.Contains(IPM.TaxonSource.Bycatch))
            //{
            //    this.labelTaxaProject.Text = this.GetProject(false);
            //    this.initBycatch();
            //}
            //else
            //    this.tabControlTaxa.TabPages.Remove(this.tabPageBycatch);

        }

        private void initDatabaseControls()
        {
            if (DiversityWorkbench.Database.DatabaseRoles().Contains("Administrator")
                || DiversityWorkbench.Database.DatabaseRoles().Contains("db_owner"))
                this.buttonEditDatabase.Enabled = true;
            if (Tasks.Settings.Default.DiversityTaxonNamesDatabase.IndexOf("].") > -1)
            {
                this.labelTaxaDatabase.Text = Tasks.Settings.Default.DiversityTaxonNamesDatabase.Substring(Tasks.Settings.Default.DiversityTaxonNamesDatabase.IndexOf("].") + 2);
                this.toolTip.SetToolTip(this.labelTaxaDatabase, Tasks.Settings.Default.DiversityTaxonNamesDatabase);
            }
            else
                this.labelTaxaDatabase.Text = Tasks.Settings.Default.DiversityTaxonNamesDatabase;

        }

        private void FormSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.buttonEditDatabase.Focus();
                // Pests
                if ((this._recordingTargets != null
                    && this._recordingTargets.Contains(IPM.RecordingTarget.TrapPest))
                    || this._recordingTargets == null)
                {
                    Settings.Default.PestNameUris = this.ReadSelection(this.dataGridViewPests);
                }

                //this.ReadSelection(this.dataGridViewPests, Settings.Default.PestNameUris);
                //Settings.Default.PestNameUris.Clear();
                //foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewPests.Rows)
                //{
                //    if (R.Cells[3].Value != null && R.Cells[3].Value.ToString().ToLower() == "true")
                //        Settings.Default.PestNameUris.Add(R.Tag.ToString());
                //}

                // Beneficials
                if ((this._recordingTargets != null
                    && this._recordingTargets.Contains(IPM.RecordingTarget.Beneficial))
                    || this._recordingTargets == null)
                    Settings.Default.BeneficialNameUris = this.ReadSelection(this.dataGridViewBeneficials);

                //this.ReadSelection(this.dataGridViewBeneficials, Settings.Default.BeneficialNameUris);
                //Settings.Default.BeneficialNameUris.Clear();
                //foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewBeneficials.Rows)
                //{
                //    if (R.Cells[3].Value != null && R.Cells[3].Value.ToString().ToLower() == "true")
                //        Settings.Default.BeneficialNameUris.Add(R.Tag.ToString());
                //}

                // Bycatch
                if ((this._recordingTargets != null
                   && this._recordingTargets.Contains(IPM.RecordingTarget.TrapBycatch))
                   || this._recordingTargets == null)
                    Settings.Default.BycatchNameUris = this.ReadSelection(this.dataGridViewBycatch);

                //this.ReadSelection(this.dataGridViewBycatch, Settings.Default.BycatchNameUris);
                //if (Settings.Default.BycatchNameUris == null)
                //    Settings.Default.BycatchNameUris = new System.Collections.Specialized.StringCollection();
                //Settings.Default.BycatchNameUris.Clear();
                //foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewBycatch.Rows)
                //{
                //    if (R.Cells[3].Value != null && R.Cells[3].Value.ToString().ToLower() == "true")
                //        Settings.Default.BycatchNameUris.Add(R.Tag.ToString());
                //}

                Settings.Default.Save();
                this.DialogResult = DialogResult.OK;
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void ReadSelection(System.Windows.Forms.DataGridView dataGridView, System.Collections.Specialized.StringCollection strings)
        {
            if (strings == null)
                strings = new System.Collections.Specialized.StringCollection();
            strings.Clear();
            foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
            {
                if (R.Cells[3].Value != null && R.Cells[3].Value.ToString().ToLower() == "true")
                    strings.Add(R.Tag.ToString());
            }
        }

        private System.Collections.Specialized.StringCollection ReadSelection(System.Windows.Forms.DataGridView dataGridView)
        {
            System.Collections.Specialized.StringCollection strings = new System.Collections.Specialized.StringCollection();
            foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
            {
                if (R.Cells[3].Value != null && R.Cells[3].Value.ToString().ToLower() == "true")
                    strings.Add(R.Tag.ToString());
            }
            return strings;
        }


        private bool AnyTaxaSelected()
        {
            bool OK = false;
            if (this._recordingTargets == null)
            {
                foreach (System.Windows.Forms.DataGridViewRow R in this.dataGridViewPests.Rows)
                {
                    if (R.Cells[3].Value != null && R.Cells[3].Value.ToString().ToLower() == "true")
                    {
                        OK = true;
                        break;
                    }
                }
            }
            else
            {
                System.Windows.Forms.DataGridView dataGridView = null;
                switch (this._recordingTargets[0])
                {
                    case IPM.RecordingTarget.TrapPest:
                        dataGridView = this.dataGridViewPests;
                        break;
                    case IPM.RecordingTarget.TrapBycatch:
                        dataGridView = this.dataGridViewBycatch;
                        break;
                    case IPM.RecordingTarget.Beneficial:
                        dataGridView = this.dataGridViewBeneficials;
                        break;
                }
                if (dataGridView != null)
                {
                    foreach (System.Windows.Forms.DataGridViewRow R in dataGridView.Rows)
                    {
                        if (R.Cells[3].Value != null && R.Cells[3].Value.ToString().ToLower() == "true")
                        {
                            OK = true;
                            break;
                        }
                    }
                }

            }
            return OK;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (AnyTaxaSelected())
            {
                this.Close();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Please select any pest taxa");
            }
        }

#endregion

        #region Taxa

        private void buttonEditDatabase_Click(object sender, EventArgs e)
        {
            try
            {
                System.Collections.Generic.Dictionary<string, string> DbUris = new Dictionary<string, string>();
                System.Collections.Generic.List<string> list = new List<string>();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].DataBaseURIs())
                {
                    DbUris.Add(KV.Key, KV.Value);
                    list.Add(KV.Key);
                }
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(list, "Taxa - source", "Please select the source for the taxa", true);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    if (DbUris.ContainsKey(f.SelectedString) &&
                        f.SelectedString != Tasks.Settings.Default.DiversityTaxonNamesDatabase &&
                        DbUris.ContainsKey(Tasks.Settings.Default.DiversityTaxonNamesDatabase))
                    {
                        string BaseUriOld = DbUris[Tasks.Settings.Default.DiversityTaxonNamesDatabase]; // DbUris[Tasks.Settings.Default.DiversityTaxonNamesDatabase];
                        string BaseUriNew = DbUris[f.SelectedString];
                        if (System.Windows.Forms.MessageBox.Show("Check single taxa?", "Check all", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            string SQL = "SELECT ModuleUri " +
                                "FROM CollectionTask C " +
                                "INNER JOIN Task AS T ON C.TaskID = T.TaskID " +
                                "WHERE (C.ModuleUri LIKE '" + BaseUriOld + "%' AND (T.Type = N'Pest') AND (T.ModuleType = N'DiversityTaxonNames')) " +
                                "GROUP BY ModuleUri " +
                                "HAVING COUNT(DISTINCT C.DisplayText) > 1 OR COUNT(DISTINCT C.Description) > 1";
                            System.Data.DataTable dtCheckOld = new DataTable();
                            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtCheckOld);
                            if (dtCheckOld.Rows.Count > 0)
                            {

                            }
                            else
                            {
                                SQL = "SELECT DISTINCT C.DisplayText, C.ModuleUri, REPLACE(ModuleUri, '" + BaseUriOld + "', '" + BaseUriNew + "') AS ModuleUriNew, SUBSTRING(C.ModuleUri, LEN('" + BaseUriOld + "') + 1, 255) AS NameID, C.Description " +
                                    "FROM CollectionTask C " +
                                    "INNER JOIN Task AS T ON C.TaskID = T.TaskID " +
                                    "WHERE (C.ModuleUri LIKE '" + BaseUriOld + "%' AND (T.Type = N'Pest') AND (T.ModuleType = N'DiversityTaxonNames')) ";
                                System.Data.DataTable dtOld = new DataTable();
                                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtOld);
                                System.Collections.Generic.Dictionary<string, Taxon> Pests = new Dictionary<string, Taxon>();
                                DiversityWorkbench.TaxonName taxon = new DiversityWorkbench.TaxonName(DiversityWorkbench.Settings.ServerConnection);
                                foreach (System.Data.DataRow R in dtOld.Rows)
                                {
                                    Taxon pestOld = new Taxon(int.Parse(R["NameID"].ToString()), IPM.TaxonSource.Pest);

                                    //Pest pestOld = new Pest();
                                    //pestOld.ScientificName = R[0].ToString();
                                    //pestOld.VernacularName = R["Description"].ToString();
                                    //pestOld.BaseURL = BaseUriNew;
                                    //pestOld.NameID = int.Parse(R["NameID"].ToString());

                                    //System.Collections.Generic.Dictionary<string, string> dictNew = taxon.UnitValues(R["ModuleUriNew"].ToString());
                                    //Pest pestNew = new Pest();
                                    //pestNew.ScientificName = dictNew[""].ToString();
                                    //pestNew.VernacularName = dictNew[""].ToString();
                                    //pestNew.BaseURL = dictNew[""].ToString();
                                    //pestNew.NameID = int.Parse(dictNew[""].ToString());
                                    //DiversityWorkbench.IWorkbenchUnit taxonOld = new 
                                }
                                Tasks.Settings.Default.DiversityTaxonNamesDatabase = f.SelectedString;
                            }
                        }
                        else
                        {
                            string SQL = "UPDATE C SET ModuleUri = REPLACE(ModuleUri, '" + BaseUriOld + "', '" + BaseUriNew + "')" +
                            "FROM CollectionTask C " +
                            "INNER JOIN Task AS T ON C.TaskID = T.TaskID " +
                            "WHERE (C.ModuleUri LIKE '" + BaseUriOld + "%' AND (T.Type = N'Pest') AND (T.ModuleType = N'DiversityTaxonNames'))";
                            if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                            {
                                Tasks.Settings.Default.DiversityTaxonNamesDatabase = f.SelectedString;
                                this.initPests();
                                this.initBeneficials();
                                this.initBycatch();
                            }
                        }
                    }
                    else
                    {
                        Tasks.Settings.Default.DiversityTaxonNamesDatabase = f.SelectedString;
                        this.initPests();
                        this.initBeneficials();
                        this.initBycatch();
                    }
                    //if (services.ContainsKey(f.SelectedString) && f.SelectedString != Tasks.Settings.Default.DiversityTaxonNamesDatabase)
                    //{
                    //    Tasks.Settings.Default.DiversityTaxonNamesDatabase = f.SelectedString;
                    //    if (services.ContainsKey(f.SelectedString) && services.ContainsKey(Tasks.Settings.Default.DiversityTaxonNamesDatabase))
                    //    {
                    //        DiversityWorkbench.DatabaseService DSold = services[Tasks.Settings.Default.DiversityTaxonNamesDatabase];
                    //        DiversityWorkbench.DatabaseService DSnew = services[f.SelectedString];
                    //        if (DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].DataBaseURIs().ContainsKey(f.SelectedString))
                    //        {
                    //            string BaseUriNew = DiversityWorkbench.WorkbenchUnit.GlobalWorkbenchUnitList()["DiversityTaxonNames"].DataBaseURIs()[f.SelectedString];
                    //        }
                    //        string SQL = "";

                    //    }

                    //}
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonTaxaProject_Click(object sender, EventArgs e)
        {
            string Project = "";
            DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesProjectID = this.GetProjectID(ref Project, false);
            this.labelTaxaProject.Text = Project;
        }

        #region Pests

        private void initPests()
        {
            this.initTaxonDefaultNameUriLists();

            this.initChecklistTaxa(IPM.RecordingTarget.TrapPest);// this.dataGridViewPests, IPM.TaxonSource.Pest);
            return;

            //try
            //{
            //    this.dataGridViewPests.RowHeadersVisible = false;
            //    this.dataGridViewPests.AllowUserToAddRows = false;
            //    this.dataGridViewPests.AllowUserToDeleteRows = false;

            //    this.dataGridViewPests.Rows.Clear();
            //    IPM iPM = new IPM();
            //    int i = iPM.GetPestTaxa().Count; //this.dataGridViewTaxa.Rows.Count;
            //    this.dataGridViewPests.Rows.Add(i);
            //    DataGridViewCellStyle s = this.dataGridViewPests.DefaultCellStyle;
            //    s.WrapMode = DataGridViewTriState.True;
            //    this.dataGridViewPests.Columns[0].DefaultCellStyle = s;
            //    this.dataGridViewPests.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //    i = 0;
            //    foreach (System.Collections.Generic.KeyValuePair<string, Taxon> keyValue in iPM.GetPestTaxa())
            //    {
            //        this.dataGridViewPests.Rows[i].Height = 50;
            //        this.dataGridViewPests.Rows[i].Cells[0].Value = keyValue.Value.Group;
            //        this.dataGridViewPests.Rows[i].Cells[1].Value = keyValue.Value.DisplayText();
            //        if (keyValue.Value.Icones != null && keyValue.Value.Icones.Count > 0)
            //        {
            //            this.dataGridViewPests.Rows[i].Cells[2].Value = keyValue.Value.Icones[0].Icon;
            //        }
            //        this.dataGridViewPests.Rows[i].Tag = keyValue.Key;
            //        if (Settings.Default.PestNameUris.Contains(keyValue.Key))
            //            this.dataGridViewPests.Rows[i].Cells[3].Value = true;
            //        else
            //            this.dataGridViewPests.Rows[i].Cells[3].Value = false;
            //        i++;
            //    }

            //    this.dataGridViewPests.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
        }

        private void buttonPestList_Click(object sender, EventArgs e)
        {
            string Project = "";
            DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesPestListID = this.GetProjectID(ref Project);
            this.labelPestList.Text = Project;
        }

        #endregion

        #region Beneficials

        private void buttonBeneficialsSetList_Click(object sender, EventArgs e)
        {
            string Project = "";
            DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesPestListID = this.GetProjectID(ref Project);
            this.labelBeneficialsList.Text = Project;
        }

        private void initBeneficials()
        {
            this.initTaxonDefaultNameUriLists();

            this.initChecklistTaxa(IPM.RecordingTarget.Beneficial);// this.dataGridViewBeneficials, IPM.TaxonSource.Beneficial);
        }

        #endregion

        #region Bycatch

        private void buttonBycatch_Click(object sender, EventArgs e)
        {
            string Project = "";
            DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesBycatchListID = this.GetProjectID(ref Project);
            this.labelBycatch.Text = Project;
        }

        private void initBycatch()
        {
            this.initTaxonDefaultNameUriLists();

            this.initChecklistTaxa(IPM.RecordingTarget.TrapBycatch);// this.dataGridViewBycatch, IPM.TaxonSource.Bycatch);
        }

        #endregion


        //private void initChecklistTaxa(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source)
        //{
        //    try
        //    {
        //        dataGridView.RowHeadersVisible = false;
        //        dataGridView.AllowUserToAddRows = false;
        //        dataGridView.AllowUserToDeleteRows = false;

        //        dataGridView.Rows.Clear();
        //        int i = RecordDicts.ChecklistRecords(source).Count;

        //        dataGridView.Rows.Add(i);
        //        DataGridViewCellStyle s = dataGridView.DefaultCellStyle;
        //        s.WrapMode = DataGridViewTriState.True;
        //        dataGridView.Columns[0].DefaultCellStyle = s;
        //        dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        //        System.Collections.Generic.Dictionary<string, Record> records = RecordDicts.ChecklistRecords(source); // Tasks.Taxa.List.TaxonStageDict(source);// new Dictionary<string, Taxon>();
        //        var recordsSorted = records.OrderBy(r => r.Value.Sorting);
        //        i = 0;
        //        foreach (System.Collections.Generic.KeyValuePair<string, Record> keyValue in recordsSorted)
        //        {
        //            if (dataGridView.Rows.Count < i)
        //                break;
        //            try
        //            {
        //                dataGridView.Rows[i].Height = 50;
        //                dataGridView.Rows[i].Cells[0].Value = keyValue.Value.Group;
        //                dataGridView.Rows[i].Cells[1].Value = keyValue.Value.DisplayText;
        //                if (keyValue.Value.PreviewImage.Icon != null && dataGridView.Rows[i].Cells.Count > 1)
        //                {
        //                    dataGridView.Rows[i].Cells[2].Value = keyValue.Value.PreviewImage.Icon;
        //                }
        //                dataGridView.Rows[i].Tag = keyValue.Key;
        //                if (dataGridView.Rows[i].Cells.Count > 2)
        //                {
        //                    switch (source)
        //                    {
        //                        case IPM.TaxonSource.Pest:
        //                            dataGridView.Rows[i].Cells[3].Value = Settings.Default.PestNameUris.Contains(keyValue.Key);
        //                            break;
        //                        case IPM.TaxonSource.Bycatch:
        //                            dataGridView.Rows[i].Cells[3].Value = Settings.Default.BycatchNameUris.Contains(keyValue.Key);
        //                            break;
        //                        case IPM.TaxonSource.Beneficial:
        //                            dataGridView.Rows[i].Cells[3].Value = Settings.Default.BeneficialNameUris.Contains(keyValue.Key);
        //                            break;
        //                    }
        //                }
        //            }
        //            catch (System.Exception ex)
        //            {
        //                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //            }
        //            i++;
        //        }

        //        dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
        //    }
        //}

        private System.Windows.Forms.DataGridView DataGridView(IPM.RecordingTarget recordingTarget)
        {
            switch (recordingTarget)
            {
                case IPM.RecordingTarget.Beneficial:
                    return this.dataGridViewBeneficials;
                case IPM.RecordingTarget.TrapBycatch:
                    return this.dataGridViewBycatch;
                default:
                    return this.dataGridViewPests;
            }
        }



        private void initChecklistTaxa(IPM.RecordingTarget recordingTarget)
        {
            IPM.initTaxaGridViewRows(recordingTarget, this.DataGridView(recordingTarget), true);
        }


        private void initTaxa(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source)
        {
            try
            {
                dataGridView.RowHeadersVisible = false;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToDeleteRows = false;

                dataGridView.Rows.Clear();
                //IPM iPM = new IPM();
                int i = Tasks.Taxa.List.TaxonStageDict(source).Count;
                //switch(source)
                //{
                //    case IPM.TaxonSource.Pest:
                //        i = iPM.GetPestTaxa("").Count;
                //        break;
                //    case IPM.TaxonSource.Bycatch:
                //        i = iPM.GetBycatchTaxa("").Count; //this.dataGridViewTaxa.Rows.Count;
                //        break;
                //    case IPM.TaxonSource.Beneficial:
                //        i = iPM.GetBeneficialTaxa("").Count; //this.dataGridViewTaxa.Rows.Count;
                //        break;
                //}

                dataGridView.Rows.Add(i);
                DataGridViewCellStyle s = dataGridView.DefaultCellStyle;
                s.WrapMode = DataGridViewTriState.True;
                dataGridView.Columns[0].DefaultCellStyle = s;
                dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                System.Collections.Generic.Dictionary<string, Taxa.TaxonStage> Taxa = Tasks.Taxa.List.TaxonStageDict(source);// new Dictionary<string, Taxon>();
                //switch(source)
                //{
                //    case IPM.TaxonSource.Beneficial:
                //        Taxa = iPM.GetBeneficialTaxa("");
                //        break;
                //    case IPM.TaxonSource.Bycatch:
                //        Taxa = iPM.GetBycatchTaxa("");
                //        break;
                //    case IPM.TaxonSource.Pest:
                //        Taxa = iPM.GetPestTaxa("");
                //        break;
                //}

                i = 0;
                foreach (System.Collections.Generic.KeyValuePair<string, Taxa.TaxonStage> keyValue in Taxa)
                {
                    if (dataGridView.Rows.Count < i)
                        break;
                    try
                    {
                        dataGridView.Rows[i].Height = 50;
                        dataGridView.Rows[i].Cells[0].Value = keyValue.Value.Group;
                        dataGridView.Rows[i].Cells[1].Value = keyValue.Value.DisplayText();
                        if (keyValue.Value.PreviewImage.Icon != null && dataGridView.Rows[i].Cells.Count > 1)
                        {
                            dataGridView.Rows[i].Cells[2].Value = keyValue.Value.PreviewImage.Icon;
                        }
                        dataGridView.Rows[i].Tag = keyValue.Key;
                        if (dataGridView.Rows[i].Cells.Count > 2)
                        {
                            switch (source)
                            {
                                case IPM.TaxonSource.Pest:
                                    dataGridView.Rows[i].Cells[3].Value = Settings.Default.PestNameUris.Contains(keyValue.Key);
                                    break;
                                case IPM.TaxonSource.Bycatch:
                                    dataGridView.Rows[i].Cells[3].Value = Settings.Default.BycatchNameUris.Contains(keyValue.Key);
                                    break;
                                case IPM.TaxonSource.Beneficial:
                                    dataGridView.Rows[i].Cells[3].Value = Settings.Default.BeneficialNameUris.Contains(keyValue.Key);
                                    break;
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                    }
                    i++;
                }

                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private int GetProjectID(ref string Project, bool ForList = true)
        {
            int ProjectID = -1;
            string SQL = "SELECT ProjectID, Project FROM " + DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesDatabase + ".dbo.";
            if (ForList)
                SQL += "TaxonNameListProjectProxy";
            else
                SQL += "ProjectList";
            SQL += " ORDER BY Project";
            System.Data.DataTable dt = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
            DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "Please select an item from the list", "Project", "", false, true, true, DiversityCollection.Resource.Project1);
            f.ShowDialog();
            System.Data.DataRow R = f.SelectedRow;
            ProjectID = int.Parse(R["ProjectID"].ToString());
            Project = R["Project"].ToString();
            return ProjectID;
        }

        private string GetProject(bool ForList = true, bool Pests = true)
        {
            string Project = "";
            string SQL = "SELECT Project FROM " + DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesDatabase + ".dbo.";
            if (ForList)
                SQL += "TaxonNameListProjectProxy";
            else
                SQL += "ProjectList";
            SQL += " WHERE ProjectID = ";
            if (!ForList)
                SQL += DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesProjectID.ToString();
            else
            {
                if (Pests)
                    SQL += DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesPestListID.ToString();
                else
                    SQL += DiversityCollection.Tasks.Settings.Default.DiversityTaxonNamesBeneficialListID.ToString();
            }
            Project = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            return Project;
        }


        #endregion

        #region Sorting - deprecated

        private void initTaxaSorting()
        {
            this.initTaxaSorting(this.dataGridViewSortingBeneficials, IPM.TaxonSource.Beneficial);
            this.initTaxaSorting(this.dataGridViewSortingBycatch, IPM.TaxonSource.Bycatch);
            this.initTaxaSorting(this.dataGridViewSortingPests, IPM.TaxonSource.Pest);
        }

        private void initTaxaSorting(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source)
        {
            try
            {
                dataGridView.Columns.Clear();
                dataGridView.RowHeadersVisible = false;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToDeleteRows = false;
                int ListID = Tasks.Taxa.List.ListID(source);
                dataGridView.DataSource = Tasks.Taxa.Database.DtSorting(ListID);
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSortingReset_Click(object sender, EventArgs e)
        {
            Tasks.Taxa.Database.DtSortingReset();
            this.initTaxaSorting();
        }

        #endregion

        #region Groups

        private void initTaxaGroups()
        {
            this.initTaxaGroups(this.dataGridViewGroupsBeneficials, IPM.TaxonSource.Beneficial);
            this.initTaxaGroups(this.dataGridViewGroupsBycatch, IPM.TaxonSource.Bycatch);
            this.initTaxaGroups(this.dataGridViewGroupsPests, IPM.TaxonSource.Pest);
        }

        private void initTaxaGroups(System.Windows.Forms.DataGridView dataGridView, IPM.TaxonSource source)
        {
            try
            {
                dataGridView.Columns.Clear();
                dataGridView.RowHeadersVisible = false;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToDeleteRows = false;
                int ListID = Tasks.Taxa.List.ListID(source);
                dataGridView.DataSource = Tasks.Taxa.Database.DtGroups(ListID);
                dataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }


        private void buttonGroupsReset_Click(object sender, EventArgs e)
        {
            Tasks.Taxa.Database.DtGroupsReset();
            this.initTaxaGroups();
        }

        #endregion

        #region Display options

        private void initDisplayOptions()
        {
            if (this._recordingTargets != null && this._recordingTargets.Count == 1)
            {
                if (this.tabControlSettings.TabPages.Contains(this.tabPageDisplay))
                    this.tabControlSettings.TabPages.Remove(this.tabPageDisplay);
            }
            else
            {
                if (!this.tabControlSettings.TabPages.Contains(this.tabPageDisplay))
                    this.tabControlSettings.TabPages.Add(this.tabPageDisplay);
                this.checkBoxShowScientificName.Checked = Settings.Default.ShowScientificName;
                this.checkBoxIncludeCollections.Checked = Settings.Default.IncludeCollections;
                this.checkBoxIncludeSpecimen.Checked = Settings.Default.IncludeSpecimen;
                this.checkBoxIncludeTransaction.Checked = Settings.Default.IncludeTransactions;
            }
        }
        private void checkBoxShowScientificName_Click(object sender, EventArgs e)
        {
            Settings.Default.ShowScientificName = this.checkBoxShowScientificName.Checked;
            Settings.Default.Save();
        }

        private void checkBoxIncludeCollections_Click(object sender, EventArgs e)
        {
            Settings.Default.IncludeCollections = this.checkBoxIncludeCollections.Checked;
            Settings.Default.Save();
        }

        private void checkBoxIncludeSpecimen_Click(object sender, EventArgs e)
        {
            Settings.Default.IncludeSpecimen = this.checkBoxIncludeSpecimen.Checked;
            Settings.Default.Save();
        }

        private void checkBoxIncludeTransaction_Click(object sender, EventArgs e)
        {
            Settings.Default.IncludeTransactions = this.checkBoxIncludeTransaction.Checked;
            Settings.Default.Save();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Api.Taxon.Taxa.ResetChecklistTaxa();
            Tasks.Taxa.RecordDicts.Reset();
            this.initChecklistTaxa(IPM.RecordingTarget.Beneficial);// this.dataGridViewBeneficials, IPM.TaxonSource.Beneficial);
            this.initChecklistTaxa(IPM.RecordingTarget.TrapBycatch);// this.dataGridViewBycatch, IPM.TaxonSource.Bycatch);
            this.initChecklistTaxa(IPM.RecordingTarget.TrapPest);// this.dataGridViewPests, IPM.TaxonSource.Pest);
        }

        #endregion

    }
}
