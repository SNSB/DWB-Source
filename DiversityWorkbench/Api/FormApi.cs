using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;


namespace DiversityWorkbench.Api
{
    public partial class FormApi : Form, iUpdate
    {
        #region Parameter

        private string _TableName;
        private string _KeyColumnName;
        private string _ProjectTableName;
        private string _ProjectIDcolumn;
        private string _ProjectSource;
        private enum TaxonTest { Single, Top10, Group }

        private bool? _ContainsColumnPublic;

        #endregion

        #region Construction
        public FormApi(string TableName, string KeyColumnName)
        {
            InitializeComponent();
            this._KeyColumnName = KeyColumnName;
            this._TableName = TableName;
            this.initForm();
        }
        #endregion

        #region Form

        private void initForm()
        {
            if (!JsonCache.DoesExist())
            {
                System.Windows.Forms.MessageBox.Show("Table JsonCache does not exist in current database", "Missing JsonCache", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                this.Dispose();
                return;
            }

            this.labelSourceTable.Text = _TableName + ": " + JsonCache.TableContentCount(_TableName).ToString();
            this.labelJsonCacheTable.Text += ": " + JsonCache.TableContentCount("JsonCache").ToString();
            this.dataGridViewJsonCache.DataSource = JsonCache.DtJson();
            this.setPublic(ContainsColumnPublic);
#if DEBUG
            this.initTest();
#else
            this.buttonTest.Visible = false;
#endif
        }


        #endregion

        #region Update

        public void setMax(int Max)
        {
            this.progressBarUpdate.Maximum = Max;
        }
        public void setCurrent(int Current)
        {
            if (Current < this.progressBarUpdate.Maximum)
                this.progressBarUpdate.Value = Current;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            if (this.SelectedProjectID == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a project", "Project?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                return;
            }
            if (JsonCache.UpdateContent(this._TableName, this._KeyColumnName, this, null, this.SelectedProjectID, this._ProjectTableName, this._ProjectIDcolumn))
                this.dataGridViewJsonCache.DataSource = JsonCache.DtJson();
            this.progressBarUpdate.Value = 0;
        }

        private void buttonUpdateSingle_Click(object sender, EventArgs e)
        {
            int ID;
            if (int.TryParse(this.dataGridViewJsonCache.Rows[this.dataGridViewJsonCache.SelectedCells[0].RowIndex].Cells["ID"].Value.ToString(), out ID))
            {
                if (JsonCache.UpdateContent(this._TableName, this._KeyColumnName, this, ID))
                {
                    System.Data.DataRowView rowView = (System.Data.DataRowView)this.dataGridViewJsonCache.Rows[this.dataGridViewJsonCache.SelectedCells[0].RowIndex].DataBoundItem;
                    rowView.BeginEdit();
                    rowView.Row["Data"] = JsonCache.Json(ID);
                    rowView.EndEdit();
                    this.dataGridViewJsonCache_CellContentClick(null, null);
                    //this.dataGridViewJsonCache.DataSource = JsonCache.DtJson();
                }
            }
        }

        #endregion

        #region Content
        private void dataGridViewJsonCache_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string Json = this.dataGridViewJsonCache.Rows[this.dataGridViewJsonCache.SelectedCells[0].RowIndex].Cells["Data"].Value.ToString();
            this.setJsonText(Json);
            if (ContainsColumnPublic)
            {
                Json = this.dataGridViewJsonCache.Rows[this.dataGridViewJsonCache.SelectedCells[0].RowIndex].Cells["Public"].Value.ToString();
                this.setPublicText(Json);
            }
        }

        private void setJsonText(string Json)
        {
            Json = Json.Replace("}],", "}],\r\n").Replace("},", "},\r\n\t");
            this.textBoxJson.Text = Json;
        }

        private void setPublicText(string Json)
        {
            Json = Json.Replace("}],", "}],\r\n").Replace("},", "},\r\n\t");
            this.textBoxPublic.Text = Json;
        }

        private void setPublic(bool ShowPublic)
        {
            this.labelPublic.Visible = ShowPublic;
            this.buttonShowPublic.Visible = ShowPublic;
            this.textBoxPublic.Visible = ShowPublic;
        }

        private bool ContainsColumnPublic
        {
            get
            {
                if (_ContainsColumnPublic == null)
                {
                    _ContainsColumnPublic = JsonCache.ContainsColumnPublic();
                }
                return (bool)_ContainsColumnPublic;
            }
        }

        #endregion

        #region Project

        private int? SelectedProjectID
        {
            get
            {
                if (this.comboBoxProject.SelectedIndex > -1)
                {
                    int ProjectID;
                    if (int.TryParse(this.comboBoxProject.SelectedValue.ToString(), out ProjectID))
                        return ProjectID;
                }
                return null;
            }
        }

        /// <summary>
        /// Setting the parameters for the project selection
        /// </summary>
        /// <param name="ProjectTable">The name of the table that links the single dataset to a project</param>
        /// <param name="ProjectColumn">The name of the column containing the ID of the project</param>
        /// <param name="ProjectList">The source containing the projects avaiable for a user</param>
        public void setProject(string ProjectTable, string ProjectColumn = "ProjectID", string ProjectList = "ProjectProxy")
        {
            this._ProjectTableName = ProjectTable;
            this._ProjectIDcolumn = ProjectColumn;
            this._ProjectSource = ProjectList;
            if (_ProjectTableName == _TableName)
            {
                this.comboBoxProject.Visible = false;
                this.labelProject.Visible = false;
            }
            else if (_ProjectIDcolumn.Length > 0 && _ProjectTableName.Length > 0)
            {
                this.comboBoxProject.Visible = true;
                this.labelProject.Visible = true;
            }
        }

        private void comboBoxProject_DropDown(object sender, EventArgs e)
        {
            if (this.comboBoxProject.DataSource == null)
            {
                this.comboBoxProject.DataSource = JsonCache.DtProject(this._ProjectSource);
                this.comboBoxProject.DisplayMember = "Project";
                this.comboBoxProject.ValueMember = "ProjectID";
            }
        }

        #endregion

        #region Test

        private void initTest()
        {
            this.buttonTest.Visible = false;
            if (this._TableName == "TaxonName")
            {
                this.comboBoxTest.Visible = true;
                this.comboBoxTest.Items.Add(TaxonTest.Single.ToString());
                this.comboBoxTest.Items.Add(TaxonTest.Top10.ToString());
                this.comboBoxTest.Items.Add(TaxonTest.Group.ToString());
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            if(this._TableName == "TaxonName" && this.comboBoxTest.SelectedItem == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a target");
                return;
            }
            try
            {
                switch (_TableName)
                {
                    case "TaxonName":
                        if (this.comboBoxTest.SelectedItem.ToString() == TaxonTest.Group.ToString())
                        {
                            if (this.dataGridViewJsonCache.Rows.Count > 0)
                            {
                                for (int i = 0; i < this.dataGridViewJsonCache.Rows.Count; i++)
                                {
                                    int ID;
                                    if (int.TryParse(this.dataGridViewJsonCache.Rows[i].Cells["ID"].Value.ToString(), out ID))
                                    {
                                        string JSON = this.dataGridViewJsonCache.Rows[i].Cells["Data"].Value.ToString();
                                        Taxon.Checklists.AddTaxon(JSON);
                                    }
                                }
                                //foreach(System.Collections.Generic.KeyValuePair<int, Taxon.TaxonList> KV in Taxon.Checklists.Taxa)
                                //{
                                //    (string, int) group = Taxon.Checklists.Taxa.First().Value.Taxa.First().Value.IpmGroup(KV.Key);
                                //}
                                //foreach (System.Collections.Generic.KeyValuePair<int, Taxon.TaxonList> KV in Taxon.Checklists.Taxa)
                                //{
                                //    (string, string) Gruppe = Taxon.Checklists.Taxa.First().Value.Taxa.First().Value.ChecklistAnalysis(1190, 47, true, "de");
                                //}

                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("List is empty");
                            }
                        }
                        else if (this.comboBoxTest.SelectedItem.ToString() == TaxonTest.Single.ToString())
                        {
                            if (this.textBoxJson.Text.Length > 0)
                            {
                                string JSON = this.textBoxJson.Text;
                                string json = JSON.Substring(1, JSON.Length - 2);
                                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                                try
                                {
                                    DiversityWorkbench.Api.Taxon.Taxon[] Taxa = JsonSerializer.Deserialize<DiversityWorkbench.Api.Taxon.Taxon[]>(JSON);
                                    if (Taxa != null)
                                    {
                                        System.Windows.Forms.MessageBox.Show("Test hat geklappt fuer\r\n" + Taxa[0].FullName.ToString());
                                    }
                                }
                                catch(System.Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }
                                try
                                {
                                    var taxa = JsonSerializer.Deserialize<List<DiversityWorkbench.Api.Taxon.Taxon>>(JSON, options);
                                    if (taxa != null)
                                    {
                                        System.Windows.Forms.MessageBox.Show("Test hat geklappt fuer\r\n" + taxa[0].FullName.ToString());
                                    }
                                }
                                catch { }
                                try
                                {
                                    var tax = JsonSerializer.Deserialize<DiversityWorkbench.Api.Taxon.Taxon>(JSON, options);
                                }
                                catch { }
                                try
                                {
                                    var taxon = JsonSerializer.Deserialize<DiversityWorkbench.Api.Taxon.Taxon>(json, options);
                                }
                                catch { }

                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("JSON is empty");
                            }
                        }
                        else if (this.comboBoxTest.SelectedItem.ToString() == TaxonTest.Top10.ToString())
                        {
                            if (this.dataGridViewJsonCache.Rows.Count > 0)
                            {
                                int Max = 10;
                                if (Max > this.dataGridViewJsonCache.Rows.Count) Max = this.dataGridViewJsonCache.Rows.Count;
                                for (int i = 0; i < Max; i++)
                                {
                                    int ID;
                                    if (int.TryParse(this.dataGridViewJsonCache.Rows[i].Cells["ID"].Value.ToString(), out ID))
                                    {
                                        string JSON = this.dataGridViewJsonCache.Rows[i].Cells["Data"].Value.ToString();
                                        Taxon.Checklists.AddTaxon(JSON);
                                    }
                                }
                            }
                            else
                            {
                                System.Windows.Forms.MessageBox.Show("List is empty");
                            }
                        }
                        break;
                    case "CollectionSpecimen":
                        int id;
                        if (int.TryParse(this.dataGridViewJsonCache.Rows[this.dataGridViewJsonCache.SelectedCells[0].RowIndex].Cells["ID"].Value.ToString(), out id))
                        {
                            DiversityWorkbench.Api.JsonCache.ShowJson(DiversityWorkbench.Api.JsonCache.Json(id));
                        }
                        break;
                    default:
                        System.Windows.Forms.MessageBox.Show("not implemented so far for table \r\n" + _TableName, "ToDo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            catch (System.Exception ex)
            { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void buttonShowInBrowser_Click(object sender, EventArgs e)
        {
            if (this.textBoxJson.Text.Length > 0) 
            {
                DiversityWorkbench.Api.JsonCache.ShowJson(this.textBoxJson.Text);
            }
            else { System.Windows.Forms.MessageBox.Show("Nothing selected"); }
        }

        private void buttonShowPublic_Click(object sender, EventArgs e)
        {
            if (this.textBoxPublic.Text.Length > 0)
            {
                DiversityWorkbench.Api.JsonCache.ShowJson(this.textBoxPublic.Text);
            }
            else { System.Windows.Forms.MessageBox.Show("Nothing selected"); }
        }
    }

    #endregion

}