using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormRemoteQueryDependent : Form
    {
        #region Parameter

        private string _URI;
        private string _DisplayText;
        private string _BaseURI;
        private DiversityWorkbench.ServerConnection _ServerConnection;
        private DiversityWorkbench.IWorkbenchUnit _IWorkbenchUnit;
        private string _DependsOnUri;
        private Microsoft.Data.SqlClient.SqlConnection _Connection;

        #endregion

        #region Construction

        public FormRemoteQueryDependent(string URI)
        {
            InitializeComponent();
            try
            {
                this._DependsOnUri = URI;
                this._ServerConnection = DiversityWorkbench.WorkbenchUnit.getServerConnectionFromURI(URI);
                if (this._ServerConnection != null)
                {
                    switch (this._ServerConnection.ModuleName)
                    {
                        case "DiversityAgents":
                            DiversityWorkbench.Agent A = new Agent(this._ServerConnection);
                            this._IWorkbenchUnit = A;
                            break;
                        default:
                            DiversityWorkbench.ScientificTerm S = new ScientificTerm(this._ServerConnection);
                            this._IWorkbenchUnit = S;
                            break;
                    }
                    this.userControlQueryList.setConnection(this._ServerConnection.ConnectionString, this._IWorkbenchUnit.MainTable());
                    if (this._ServerConnection.LinkedServer.Length > 0)
                    {
                        this.userControlQueryList.LinkedServer = this._ServerConnection.LinkedServer;
                        this.userControlQueryList.LinkedServerDatabase = this._ServerConnection.DatabaseName;
                    }
                    this.Text = this._ServerConnection.DatabaseName + " on " + this._ServerConnection.DatabaseServer;
                    this.userControlQueryList.setQueryConditions(this._IWorkbenchUnit.DependentQueryConditions(URI));
                    this.userControlQueryList.QueryDisplayColumns = this._IWorkbenchUnit.DependentQueryDisplayColumns(URI);
                    this.userControlQueryList.setModeOfControl(UserControls.UserControlQueryList.Mode.Simple);
                    this.userControlQueryList.listBoxQueryResult.SelectedIndexChanged += new System.EventHandler(this.listBoxQueryResult_SelectedIndexChanged);

                }
            }
            catch (System.Exception ex) { }
        }

        #endregion

        private string SqlScalar(string SQL)
        {
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, this.Connection());
            string Result = C.ExecuteScalar()?.ToString() ?? string.Empty;
            return Result;
        }

        private Microsoft.Data.SqlClient.SqlConnection Connection()
        {
            if (this._Connection == null)
                this._Connection = new Microsoft.Data.SqlClient.SqlConnection(this._ServerConnection.ConnectionString);
            this._Connection.Open();
            return this._Connection;
        }

        #region Form

        private void setTitle()
        {
            try
            {
                string Title = " " + this._ServerConnection.ModuleName;
                if (this._ServerConnection.DatabaseName != this._ServerConnection.ModuleName)
                    Title += "    (" + this._ServerConnection.DatabaseName + ")";
                if (this._ServerConnection.LinkedServer.Length > 0)
                    Title += "    Server: " + this._ServerConnection.LinkedServer;
                else
                    Title += "    Server: " + this._ServerConnection.DatabaseServer;
                if (this._ServerConnection.IsTrustedConnection)
                    Title += "    User: " + System.Environment.UserName.ToString();
                else
                {
                    if (this._ServerConnection.DatabaseUser.Length > 0) Title += "    User: " + this._ServerConnection.DatabaseUser;
                }
                this.Text = Title;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        public string URI
        {
            get
            {
                System.Data.DataRowView R = (System.Data.DataRowView)this.userControlQueryList.listBoxQueryResult.SelectedItem;
                return this._ServerConnection.BaseURL + R[0].ToString();
            }
        }

        public string DisplayText
        {
            get
            {
                if (this._DisplayText == null)
                    this._DisplayText = "";
                if (this._DisplayText.Length == 0) // Toni 20180821: Read query list only if database was called
                {
                    try
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.userControlQueryList.listBoxQueryResult.SelectedItem;
                        this._DisplayText = R["Display"].ToString();
                    }
                    catch (System.Exception ex) { }
                }
                return this._DisplayText.Trim();
            }
        }

        private void listBoxQueryResult_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                this.setUnit(this.userControlQueryList.ID);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }



        private void setUnit(int ID)
        {
            try
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (this.panelUnit.Controls.Count == 0)
                    this.setUnitPanel(this._IWorkbenchUnit);
                foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                        C.Text = "";
                }
                System.Collections.Generic.Dictionary<string, string> Values;
                {
                    {
                        Values = this._IWorkbenchUnit.UnitValues(ID);
                    }
                    try
                    {
                    }
                    catch (System.Exception ex)
                    {
                    }

                }

                foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
                {
                    foreach (System.Windows.Forms.Control C in this.panelUnit.Controls)
                    {
                        if (C.GetType() == typeof(System.Windows.Forms.TextBox) &&
                            C.Name == "textBox" + P.Key)
                        {
                            if (C.Text.Length > 0) C.Text += ", ";
                            C.Text += P.Value;
                        }
                    }
                    if (P.Key == "_URI")
                        this._URI = P.Value;
                    if (P.Key == "_DisplayText" && P.Value.Length > 0)
                        this._DisplayText = P.Value;
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            finally
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
            }
            //this.setTextHeights();
        }

        private void setUnitPanel(DiversityWorkbench.IWorkbenchUnit IWorkbenchUnit)
        {
            //this.tableLayoutPanelUnit.ColumnStyles.Clear();
            //foreach (System.Windows.Forms.Control C in this.tableLayoutPanelUnit.Controls)
            //    C.Dispose();
            //this.tableLayoutPanelUnit.Controls.Clear();

            //this.tableLayoutPanelUnit.Height = (int)(22 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);

            //int r = 0;
            //int i = this.tableLayoutPanelUnit.Height;

            //System.Windows.Forms.Label labelUnit = new Label();
            //labelUnit.Text = IWorkbenchUnit.MainTable();
            //labelUnit.Dock = DockStyle.Fill;
            //labelUnit.TextAlign = ContentAlignment.BottomLeft;
            //this.tableLayoutPanelUnit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            //this.tableLayoutPanelUnit.Controls.Add(labelUnit);
            //this.tableLayoutPanelUnit.SetColumn(labelUnit, 0);
            //this.tableLayoutPanelUnit.SetRow(labelUnit, r);
            //r++;

            string Key = "";
            System.Collections.Generic.Dictionary<string, string> Values = IWorkbenchUnit.UnitValues(-1);
            //if (_ShowAdditional)
            //    (this._IWorkbenchUnit as WorkbenchUnit).GetAdditionalUnitValues(-1, Values);
            //if (this._Domain != null && this._Domain.Length > 0)
            //    Values = IWorkbenchUnit.UnitValues(this._Domain, -1);
            foreach (System.Collections.Generic.KeyValuePair<string, string> P in Values)
            {
                if (!P.Key.StartsWith("_"))
                {
                    if (Key != P.Key)
                    {
                        try
                        {
                            System.Windows.Forms.Label L = new Label();
                            L.Text = P.Key;
                            L.Font = new Font(FontFamily.GenericSansSerif, 8.0F, FontStyle.Bold);
                            L.ForeColor = System.Drawing.Color.Gray;
                            L.Dock = DockStyle.Top;
                            L.TextAlign = ContentAlignment.BottomLeft;
                            this.panelUnit.Controls.Add(L);
                            L.BringToFront();

                            System.Windows.Forms.TextBox T = new TextBox();
                            T.Name = "textBox" + P.Key;
                            T.Dock = DockStyle.Top;
                            T.Height = (int)(39 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                            T.ReadOnly = true;
                            T.TextAlign = HorizontalAlignment.Center;
                            T.BorderStyle = BorderStyle.None;
                            T.Multiline = true;
                            T.ScrollBars = ScrollBars.Vertical;
                            this.panelUnit.Controls.Add(T);
                            T.BringToFront();
                            //T.ScrollBars = ScrollBars.None;
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                    }
                    Key = P.Key;
                }
            }
        }


    }
}
