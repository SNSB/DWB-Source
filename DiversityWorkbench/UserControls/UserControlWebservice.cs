using DWBServices.WebServices;
using DWBServices.WebServices.GeoServices;
using DWBServices.WebServices.TaxonomicServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text.Json;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlWebservice : UserControl
    {

        private IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> _api;
        private DwbServiceEnums.DwbService currentDwbService;
        #region Parameter

        private System.Data.DataTable _dtQuery;
        private int? _QueryMaxHeight = 0;
        private System.Collections.Generic.List<DiversityWorkbench.QueryCondition> _QueryConditions;
        // private System.Collections.Generic.List<DiversityWorkbench.WebserviceQueryOption> _QueryOptions;
        private Timer animationTimer;
        private int dotCount = 0;

        #endregion  

        #region Interface

        public UserControlWebservice()
        {
            InitializeComponent();
            
            // Create a timer for animation
            animationTimer = new Timer
            {
                Interval = 500
            };
            animationTimer.Tick += AnimationTimer_Tick;
            this._dtQuery = new DataTable();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            dotCount = (dotCount + 1) % 4;
            lblSearching.Text = "Loading" + new string('.', dotCount);
        }
        public void SetDwbWebservice(IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> dwbWebservice, DwbServiceEnums.DwbService currentService)
        {
            _api = dwbWebservice;
            if (_api != null)
            {
                this.linkLabelWebservice.Text = _api.GetServiceUri(currentService);
            }
            currentDwbService = currentService;
        }

        public IDwbWebservice<DwbSearchResult, DwbSearchResultItem, DwbEntity> GetDwbWebservice()
        {
            return _api;
        }

        public System.Data.DataTable DtQuery
        {
            get { return _dtQuery; }
            //set { _dtQuery = value; }
        }

        public void setVisibilityOfMaxResultsForQuery(bool IsVisible)
        {
            this.labelMaxResults.Visible = IsVisible;
            this.maskedTextBoxMaxResults.Visible = IsVisible;
        }


        public void resetQueryResults()
        {
            try
            {
                this.groupBoxQueryResults.Text = "Query results: No match";
                this._dtQuery.Clear();
            }
            catch { }
        }

        public void setQueryConditions(System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QueryConditions)
        {
            this.groupBoxQueryConditions.Controls.Clear();
            if (QueryConditions == null)
            {
                this.groupBoxQueryConditions.Visible = false;
            }
            else
            {
                this.groupBoxQueryConditions.Visible = true;
                this._QueryConditions = QueryConditions;
                int NeededHeight = 16 + (this._QueryConditions.Count * 21);
                NeededHeight = (int)(NeededHeight * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                if (this.groupBoxQueryConditions.Height < NeededHeight)
                {
                    this.splitContainerMain.SplitterDistance -= NeededHeight - this.groupBoxQueryConditions.Height;
                }
                for (int i = 0; i < this._QueryConditions.Count; i++)//System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.QueryCondition> KV in this._QueryConditions)
                {
                    DiversityWorkbench.UserControls.UserControlQueryCondition U = new UserControls.UserControlQueryCondition(this._QueryConditions[i], "");
                    this.groupBoxQueryConditions.Controls.Add(U);
                    U.Dock = DockStyle.Top;
                }
            }
        }

        #endregion

        #region Buttons

        private void buttonSetQueryConditionsUpDown_Click(object sender, EventArgs e)
        {
            if (this.buttonSetQueryConditionsUpDown.Tag == null)
                this.buttonSetQueryConditionsUpDown.Tag = true;
            if (bool.Parse(this.buttonSetQueryConditionsUpDown.Tag.ToString()))
            {
                this.buttonSetQueryConditionsUpDown.Tag = false;
                this.buttonSetQueryConditionsUpDown.Image = DiversityWorkbench.ResourceWorkbench.ArrowUp;
                this.toolTipQueryList.SetToolTip(this.buttonSetQueryConditionsUpDown, "Show the query conditions");
                this.splitContainerMain.Panel2Collapsed = true;
            }
            else
            {
                this.buttonSetQueryConditionsUpDown.Tag = true;
                this.buttonSetQueryConditionsUpDown.Image = DiversityWorkbench.ResourceWorkbench.ArrowDown;
                this.toolTipQueryList.SetToolTip(this.buttonSetQueryConditionsUpDown, "Hide the query conditions");
                this.splitContainerMain.Panel2Collapsed = false;
            }
            this.setQueryConditionsHeight();
        }

        private void setQueryConditionsHeight()
        {
            if (this.splitContainerMain.Orientation == Orientation.Horizontal)
            {
                if (!bool.Parse(this.buttonSetQueryConditionsUpDown.Tag.ToString()))
                {
                    this.splitContainerMain.SplitterDistance = this.splitContainerMain.Height - 20;
                }
                else
                {
                    if (this._QueryMaxHeight < (int)this.splitContainerMain.Size.Height / 1.5)
                        this.splitContainerMain.SplitterDistance = this.splitContainerMain.Size.Height - (int)this._QueryMaxHeight;
                    else
                    {
                        this.splitContainerMain.SplitterDistance = this.splitContainerMain.Size.Height - (int)(this.splitContainerMain.Size.Height / 1.5);
                    }
                }
            }
        }

        private async void buttonQuery_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    var searchUrl = CreateSearchUrl();
                    if (_api == null || string.IsNullOrEmpty(searchUrl))
                    {
                        if (_api == null)
                            MessageBox.Show("No webservice defined");
                        return;
                    }

                    lblSearching.Visible = true;
                    this.Enabled = false;
                    animationTimer.Start();
                    var tt = await _api.CallWebServiceAsync<object>(searchUrl,
                        DwbServiceEnums.HttpAction.GET);

                    listBoxQueryResult.SelectedIndex = -1;
                    listBoxQueryResult.DataSource = null;
                    _dtQuery.Clear();

                    if (tt != null)
                    {
                        var clientSearchModel = _api.GetDwbApiSearchResultModel(tt);
                        ReadDwbSearchModelInQueryTable(clientSearchModel, ref _dtQuery);
                    }

                    this.listBoxQueryResult.DataSource = this._dtQuery;
                    string D = "_DisplayText";
                    // if (this.comboBoxQueryColumn.Text.Length > 0) D = this.comboBoxQueryColumn.Text;
                    if (this.listBoxQueryResult.DisplayMember != D)
                        this.listBoxQueryResult.DisplayMember = D;
                    this.listBoxQueryResult.ValueMember = "_URI";

                    // Show no results info
                    if (!(this.listBoxQueryResult.Items.Count > 0))
                        this.groupBoxQueryResults.Text = "Query results: No match";
                    else
                    {
                        this.groupBoxQueryResults.Text = "Query results";
                    }

                }
                catch (ArgumentException aEx)
                {
                    MessageBox.Show(
                        "The web service call is incorrect.\r\n\r\n  " +
                        "For more details on the error, see the error log file.\r\n\r\n",
                        "Web service Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    ExceptionHandling.WriteToErrorLogFile(
                        "UserControlWebservice - buttonQuery_Click, ArgumentException exception: " +
                        aEx);
                }
                catch (InvalidOperationException ioEx)
                {
                    MessageBox.Show(
                        "The web service call is incorrect.\r\n\r\n  " +
                        "For more details on the error, see the error log file.\r\n\r\n",
                        "Web service Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    ExceptionHandling.WriteToErrorLogFile(
                        "UserControlWebservice - buttonQuery_Click, InvalidOperationException exception: " +
                        ioEx);
                }
                catch (JsonException jsonException)
                {
                    MessageBox.Show(
                        "The web service call is incorrect.\r\n\r\n  " +
                        "For more details on the error, see the error log file.\r\n\r\n",
                        "Web service Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    ExceptionHandling.WriteToErrorLogFile(
                        "UserControlWebservice - buttonQuery_Click, JsonException exception: " +
                        jsonException);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "The web service call is incorrect.\r\n\r\n  " +
                        "For more details on the error, see the error log file.\r\n\r\n",
                        "Web service Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    ExceptionHandling.WriteToErrorLogFile(
                        "UserControlWebservice - buttonQuery_Click, Exception exception: " +
                        ex);
                }
                finally
                {
                    lblSearching.Visible = false;
                    animationTimer.Stop();
                    this.Enabled = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string CreateSearchUrl()
        {
            const int offset = 0; // default TODO Ariane if we want to add paging, then we can get/set the offset here
            if (!int.TryParse(this.maskedTextBoxMaxResults.Text, out var MaxRecords))
            {
                MaxRecords = 10;
            }
            var queryRestriction = QueryRestriction();
            if (string.IsNullOrEmpty(queryRestriction) || queryRestriction.Length < 3)
            {
                MessageBox.Show("You must enter a valid name. It must be at least three characters long.");
                return "";
            }
            try
            {
                return _api?.DwbApiQueryUrlString(currentDwbService, queryRestriction, offset, MaxRecords) ?? string.Empty;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("You must enter a valid name. It must be at least three characters long.\r\n\r\n :" + ex.Message, "Argument Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ExceptionHandling.WriteToErrorLogFile("You must enter a valid name. It must be at least three characters long.\r\n\r\n :" + ex);
                return "";
            }
        }

        private string QueryRestriction()
        {
            string Restriction = "";
            if (this._QueryConditions == null || this._QueryConditions.Count == 0)
            {
                Restriction = this.textBoxQueryCondition1.Text;
            }
            else
            {
                System.Collections.Generic.List<DiversityWorkbench.QueryCondition> QQ = new List<QueryCondition>();
                foreach (System.Windows.Forms.Control C in this.groupBoxQueryConditions.Controls)
                {
                    DiversityWorkbench.UserControls.UserControlQueryCondition U = (DiversityWorkbench.UserControls.UserControlQueryCondition)C;
                    QQ.Add(U.Condition());
                }
            }
            return Restriction;
        }
        private void linkLabelWebservice_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Mark the clicked link as visited
            var clickedLink = linkLabelWebservice.Links[linkLabelWebservice.Links.IndexOf(e.Link)];
            clickedLink.Visited = true;
            // Determine the target URL
            string target = e.Link.LinkData as string ?? linkLabelWebservice.Text;
            // Check if the target is a valid URL and open it
            if (IsValidUrl(target))
            {
                OpenUrl(target);
            }
            else
            {
                MessageBox.Show("Item clicked: " + target);
            }
        }
        private bool IsValidUrl(string url)
        {
            return !string.IsNullOrEmpty(url) && (url.StartsWith("www") || url.StartsWith("http://") || url.StartsWith("https://"));
        }
        private void OpenUrl(string url)
        {
            try
            {
                var info = new System.Diagnostics.ProcessStartInfo(url)
                {
                    UseShellExecute = true
                };
                System.Diagnostics.Process.Start(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open URL: " + ex.Message);
            }
        }

        //private void comboBoxQueryColumn_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    this.listBoxQueryResult.DisplayMember = this.comboBoxQueryColumn.Text;
        //}

        private void ReadDwbSearchModelInQueryTable(DwbSearchResult dwbSearch, ref System.Data.DataTable dtQuery)
        {
            try
            {
                if (dwbSearch is null)
                    return;
                if (dwbSearch is TaxonomicSearchResult)
                {
                    TaxonomicSearchResult taxonomicSearch = (TaxonomicSearchResult)dwbSearch;
                    dtQuery.Columns.Clear();
                    var columns = new[]
                    {
                    new { Name = "_URI", Type = "System.String" },
                    new { Name = "_DisplayText", Type = "System.String" },
                    new { Name = "Taxon", Type = "System.String" },
                    new { Name = "URL", Type = "System.String" },
                    new { Name = "Kingdom", Type = "System.String" },
                    new { Name = "Rank", Type = "System.String" },
                    new { Name = "Status", Type = "System.String" },
                    new { Name = "External ID", Type = "System.String" },
                    new { Name = "Database", Type = "System.String" },
                    new { Name = "Common names", Type = "System.String" },
                    new { Name = "Distribution", Type = "System.String" },
                    new { Name = "Citation", Type = "System.String" }
                };
                    foreach (var column in columns)
                    {
                        dtQuery.Columns.Add(new DataColumn(column.Name, Type.GetType(column.Type)));
                    }

                    if (taxonomicSearch.DwbApiSearchResponse != null)
                        foreach (var item in taxonomicSearch.DwbApiSearchResponse)
                        {
                            var row = dtQuery.NewRow();
                            row["_URI"] = item._URL;
                            row["_DisplayText"] = item._DisplayText;
                            row["Taxon"] = item.Taxon;
                            row["URL"] = item._URL;
                            row["Kingdom"] = item.Kingdom;
                            row["Rank"] = item.Rank;
                            row["Status"] = item.Status;
                            row["Common names"] = item.CommonNames;
                            dtQuery.Rows.Add(row);
                        }
                }
                else
                {
                    if (dwbSearch is GeoSearchResult)
                    {
                        GeoSearchResult taxonomicSearch = (GeoSearchResult)dwbSearch;
                        dtQuery.Columns.Clear();
                        var columns = new[]
                        {
                            new { Name = "_URI", Type = "System.String" },
                            new { Name = "_DisplayText", Type = "System.String" },
                            new { Name = "Label", Type = "System.String" },
                            new { Name = "Synonyms", Type = "System.String" },
                        };
                        foreach (var column in columns)
                        {
                            dtQuery.Columns.Add(new DataColumn(column.Name, Type.GetType(column.Type)));
                        }

                        if (taxonomicSearch.DwbApiSearchResponse != null)
                            foreach (var item in taxonomicSearch.DwbApiSearchResponse)
                            {
                                var row = dtQuery.NewRow();
                                row["_URI"] = item._URL;
                                row["_DisplayText"] = item._DisplayText;
                                row["Label"] = item.Label;
                                row["Synonyms"] = string.Join(", ", item.Synonyms ?? Array.Empty<string>());
                                dtQuery.Rows.Add(row);
                            }
                    }
                    else
                    {
                        // TODO implement here other than above
                    }
                }

            }
            catch (Exception ex)
            {
#if DEBUG

                Console.WriteLine(ex.StackTrace);
#endif 
            }
        }
        #endregion

    }
}
