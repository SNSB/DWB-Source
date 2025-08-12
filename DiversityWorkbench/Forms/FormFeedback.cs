using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormFeedback : Form
    {

        #region Parameter

        private string _Error;
        private System.IO.FileInfo _ErrorLog;
        private string _User;
        private string _DatabaseUser = "";
        private string _ReportingUser = "";

        private string _Module;
        private string _Database;
        private string _Version;
        private string _Server = "";
        private string _Query = "";
        private string _ID = "";
        private System.Byte[] _BImage;
        private Microsoft.Data.SqlClient.SqlConnection _Connection;
        private string _ConnectionString = "";
        private bool _History = false;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapter;
        private string _Priority = "";

        public Microsoft.Data.SqlClient.SqlDataAdapter SqlDataAdapter
        {
            get
            {
                if (this._SqlDataAdapter == null)
                {
                    string SQL = "SELECT ID, Image, Description, LogFile, Module, Version, DatabaseName, Server, ReportedBy, ReplyAddress, QueryString, CurrentID, Priority, ToDoUntil, Topic " +
                        "FROM DiversityWorkbenchFeedback.dbo.Feedback WHERE ID = -1";
                    this._SqlDataAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionStringForFeedback);
                    DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlDataAdapter, this.dataSetFeedback.Feedback, SQL, ConnectionStringForFeedback);
                }
                return _SqlDataAdapter;
            }
        }

        public enum FormState { Reporting, History, Editing, NotFixed }
        private FormState _FormState = FormState.NotFixed;

        private System.Collections.Generic.Dictionary<WhereClausePart, string> _WhereClauseParts;

        public static string HelpProviderHelpNamespace = "";

        #endregion

        #region Construction

        #region Old versions

        public FormFeedback()
        {
            InitializeComponent();
            this.initForm();
            this.dateTimePickerToDoUntil.MinDate = System.DateTime.Now;
        }

        public FormFeedback(string Module, string Version, Microsoft.Data.SqlClient.SqlConnection Connection)
            : this()
        {
            try
            {
                //this._Database = Module;
                this._ErrorLog = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ErrorLogFile(Module));// ...Windows.Forms.Application.StartupPath + "\\" + Module + "Error.log");
                if (this._ErrorLog.Exists)
                    this._Error = this._ErrorLog.FullName;
                this._Module = Module;
                this._Version = Version;
                //this._User = System.Environment.UserName;
                this._Connection = Connection;
            }
            catch { }
        }

        public FormFeedback(string Module, string Version, string Query, string ID)
            : this()
        {
            try
            {
                //this._Database = Module;
                this._ErrorLog = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ErrorLogFile(Module));// ...Windows.Forms.Application.StartupPath + "\\" + Module + "Error.log");
                if (this._ErrorLog.Exists)
                    this._Error = this._ErrorLog.FullName;
                this._Module = Module;
                this._Version = Version;
                //this._User = System.Environment.UserName;
                this._Query = Query;
                this._ID = ID;

            }
            catch { }
        }

        /// <summary>
        /// For history of the feedbacks
        /// </summary>
        /// <param name="Module">The module</param>
        public FormFeedback(string Module)
        {
            InitializeComponent();
            this.dateTimePickerToDoUntil.MinDate = System.DateTime.Now;
            try
            {
                //this._Database = Module;
                this._Module = Module;
                //this._User = System.Environment.UserName;
                this._History = true;
            }
            catch { }
            this.initForm();
        }

        /// <summary>
        /// For history of the feedbacks
        /// </summary>
        /// <param name="Module">The module</param>
        /// <param name="ForHistory">If the datasets</param>
        public FormFeedback(string Module, bool ForHistory)
        {
            InitializeComponent();
            this.dateTimePickerToDoUntil.MinDate = System.DateTime.Now;
            try
            {
                this._Module = Module;
                this._History = ForHistory;
            }
            catch { }
            this.initForm();
        }

        #endregion

        /// <summary>
        /// Reporting a feedback
        /// </summary>
        /// <param name="Version">If available the version of the client application</param>
        /// <param name="Query">If available the query for selecting the datasets</param>
        /// <param name="ID">If available the ID of the current dataset</param>
        /// <param name="State">The state of the form</param>
        public FormFeedback(string Version, string Query, string ID, FormState State)
        {
            InitializeComponent();
            this.dateTimePickerToDoUntil.MinDate = System.DateTime.Now;
            try
            {
                string Module = DiversityWorkbench.Settings.ModuleName;
                this._ErrorLog = new System.IO.FileInfo(DiversityWorkbench.WorkbenchResources.WorkbenchDirectory.ErrorLogFile(Module));// ...Windows.Forms.Application.StartupPath + "\\" + Module + "Error.log");
                if (this._ErrorLog.Exists)
                    this._Error = this._ErrorLog.FullName;
                this._Module = Module;
                this._Version = Version;
                this._Query = Query;
                this._ID = ID;
                this._FormState = State;
                if (DiversityWorkbench.Forms.FormFeedback.HelpProviderHelpNamespace.Length > 0)
                {
                    this.helpProvider.HelpNamespace = DiversityWorkbench.Forms.FormFeedback.HelpProviderHelpNamespace;
                    this.setHelp("Feedback");
                }
                this.initForm();
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        #endregion

        #region Form

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void initForm()
        {
            this.FillPicklists();
            if (this._FormState == FormState.NotFixed)
            {

                this.splitContainerMain.Visible = false;
                this.buttonCancel.Visible = false;
                this.buttonSendFeedback.Visible = false;
                this.splitContainerMain.Panel1Collapsed = true;
                this.bindingNavigator.Visible = false;

                this.buttonFilterForDatabase.Visible = false;
                this.buttonFilterForModule.Visible = false;
                this.buttonFilterForUser.Visible = false;
                this.buttonSave.Visible = false;

                try
                {
                    if (this._History)
                    {
                        this.SetFormForHistory();
                    }
                    else
                    {
                        if (this.DatabaseRoles().Contains("DiversityWorkbenchAdmin") || this.DatabaseUser == "dbo")
                            this.SetFormForAdmin();
                        else if (this.DatabaseRoles().Contains("DiversityWorkbenchEditor"))
                            this.SetFormForEditor();
                        else
                            this.SetFormForUser();
                    }
                    this.splitContainerMain.Visible = true;
                }
                catch (System.Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Please connect to a database\r\n" + ex.Message);
                    this.Close();
                }
            }
            else
            {
                switch (this._FormState)
                {
                    case FormState.Editing:
                        if (this.DatabaseRoles().Contains("DiversityWorkbenchAdmin") || this.DatabaseUser == "dbo")
                            this.SetFormForAdmin();
                        else if (this.DatabaseRoles().Contains("DiversityWorkbenchEditor"))
                            this.SetFormForEditor();
                        else
                            this.SetFormForUser();
                        break;
                    case FormState.History:
                        this.SetFormForHistory();
                        break;
                    case FormState.Reporting:
                        this.SetFormForUser();
                        break;
                }
            }
        }

        private void FillPicklists()
        {
            try
            {
                string SQL = "SELECT Code, DisplayText " +
                    "FROM DiversityWorkbenchFeedback.dbo.ProgressState_Enum " +
                    "WHERE (DisplayEnable = 1) " +
                    "ORDER BY DisplayText";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringForFeedback);
                ad.Fill(this.dataSetFeedback.ProgressState_Enum);


                if (this.DatabaseRoles().Contains("DiversityWorkbenchAdmin") || this.DatabaseUser == "dbo")
                    SQL = "SELECT NULL AS Code, NULL AS DisplayText, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT Code, DisplayText, DisplayOrder " +
                    "FROM DiversityWorkbenchFeedback.dbo.Priority_Enum " +
                    "WHERE (DisplayEnable = 1) " +
                    "ORDER BY DisplayOrder";
                else
                    SQL = "SELECT NULL AS Code, NULL AS DisplayText, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT Code, DisplayText, DisplayOrder " +
                    "FROM DiversityWorkbenchFeedback.dbo.Priority_Enum " +
                    "WHERE (DisplayEnable = 1) AND Code <> 'rejected' " +
                    "ORDER BY DisplayOrder";
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(this.dataSetFeedback.Priority_Enum);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Failed to load progress state. Please connect to a database\r\n" + ex.Message);
                this.Close();
            }
        }

        private void FormFeedback_Load(object sender, EventArgs e)
        {
            return;

            //// TODO: Diese Codezeile lädt Daten in die Tabelle "dataSetFeedback.Priority_Enum". Sie können sie bei Bedarf verschieben oder entfernen.
            //this.priority_EnumTableAdapter.Fill(this.dataSetFeedback.Priority_Enum);
            string SQL = "";
            try
            {
                SQL = "SELECT Code, DisplayText " +
                    "FROM DiversityWorkbenchFeedback.dbo.ProgressState_Enum " +
                    "WHERE (DisplayEnable = 1) " +
                    "ORDER BY DisplayText";
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringForFeedback);
                ad.Fill(this.dataSetFeedback.ProgressState_Enum);


                if (this.DatabaseRoles().Contains("DiversityWorkbenchAdmin") || this.DatabaseUser == "dbo")
                    SQL = "SELECT NULL AS Code, NULL AS DisplayText, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT Code, DisplayText, DisplayOrder " +
                    "FROM DiversityWorkbenchFeedback.dbo.Priority_Enum " +
                    "WHERE (DisplayEnable = 1) " +
                    "ORDER BY DisplayOrder";
                else
                    SQL = "SELECT NULL AS Code, NULL AS DisplayText, NULL AS DisplayOrder " +
                    "UNION " +
                    "SELECT Code, DisplayText, DisplayOrder " +
                    "FROM DiversityWorkbenchFeedback.dbo.Priority_Enum " +
                    "WHERE (DisplayEnable = 1) AND Code <> 'rejected' " +
                    "ORDER BY DisplayOrder";
                ad.SelectCommand.CommandText = SQL;
                ad.Fill(this.dataSetFeedback.Priority_Enum);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Failed to load progress state. Please connect to a database\r\n" + ex.Message);
                this.Close();
            }


            this.splitContainerMain.Visible = false;
            this.buttonCancel.Visible = false;
            this.buttonSendFeedback.Visible = false;
            this.splitContainerMain.Panel1Collapsed = true;
            this.bindingNavigator.Visible = false;

            this.buttonFilterForDatabase.Visible = false;
            this.buttonFilterForModule.Visible = false;
            this.buttonFilterForUser.Visible = false;
            this.buttonSave.Visible = false;

            try
            {
                if (this._History)
                {
                    this.SetFormForHistory();
                }
                else
                {
                    if (this.DatabaseRoles().Contains("DiversityWorkbenchAdmin") || this.DatabaseUser == "dbo")
                        this.SetFormForAdmin();
                    else if (this.DatabaseRoles().Contains("DiversityWorkbenchEditor"))
                        this.SetFormForEditor();
                    else
                        this.SetFormForUser();
                }
                this.splitContainerMain.Visible = true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Please connect to a database\r\n" + ex.Message);
                this.Close();
            }
        }

        private void SetFormForHistory()
        {
            // try to get the name of the database server
            string Host = System.Environment.MachineName;
            try
            {
                string Server = DiversityWorkbench.Settings.DatabaseServer;
                string[] Parts = Server.Split(new char[] { '.' });
                if (Parts.Length == 4)
                {
                    byte[] IpParts = new byte[4];
                    for (int i = 0; i < Parts.Length; i++)
                    {
                        byte.TryParse(Parts[i], out IpParts[i]);
                    }
                    System.Net.IPAddress IP = new System.Net.IPAddress(IpParts);
                    Host = System.Net.Dns.GetHostByAddress(IP.ToString()).HostName;
                }
                else
                    Host = Server;
            }
            catch { }
            string User = this.ReportingUser; // this.DatabaseUser;
            if (User == "Feedback")
                User = System.Environment.UserName;
            if (User.Contains("\\"))
                User = User.Substring(User.IndexOf('\\') + 1);
            this.WhereClauseParts[WhereClausePart.User] = " (ReportedBy LIKE '%" + User + "' OR ReportedBy = '" + User + "') ";
            this.WhereClauseParts[WhereClausePart.Module] = " (Module LIKE N'" + this._Module + "%') ";
            this.FilterDatasets();
            if (this.dataSetFeedback.Feedback.Rows.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("Nou fiedbag faund");
                this.Close();
            }
            this.splitContainerMain.Panel1Collapsed = false;
            this.splitContainerMain.SplitterDistance = 15;
            this.splitContainerLogData.Enabled = false;
            this.labelHeader.Visible = true;
            this.labelHeader.Text = "Feedback sent by: " + this.ReportingUser + ".   Module: " + this._Module + ".   Database: " + this.Database;
            this.labelDateForHistory.Visible = true;
            this.buttonCancel.Visible = false;
            this.buttonSendFeedback.Visible = false;
            this.textBoxDescription.Focus();
            this.comboBoxState.Enabled = false;
            this.buttonSendReply.Visible = false;
            this.bindingNavigator.BindingSource = this.feedbackBindingSource;
            this.bindingNavigator.Visible = true;
            this.textBoxProgress.BackColor = System.Drawing.Color.White;
            this.bindingNavigatorAddNewItem.Visible = false;
            this.labelImage.Visible = false;
            this.labelImage2.Visible = false;
            this.buttonInsertImage.Visible = false;
            this.splitContainerDescription.Panel2Collapsed = false;
            this.buttonSortByDate.Visible = true;
            this.buttonSortByPriority.Visible = true;

            this.comboBoxPriority.Enabled = false;
            this.comboBoxState.Enabled = false;
            this.comboBoxTopic.Enabled = false;
            this.textBoxDescription.Enabled = true;
            this.textBoxDescription.ReadOnly = true;
            this.textBoxProgress.ReadOnly = true;
            this.buttonSortByDate.Enabled = false;
            this.buttonSortByPriority.Enabled = false;
            this.dateTimePickerToDoUntil.Enabled = false;

            this.buttonSave.Visible = false;
            this.bindingNavigatorDeleteItem.Visible = false;

            this.tableLayoutPanelEmailReply.Visible = false;

        }

        private void SetFormForAdmin()
        {
            this.WhereClauseParts[WhereClausePart.ToDo] = " (ProgressState <> N'Finished') AND (ProgressState <> N'NoError')";
            this.WhereClauseParts[WhereClausePart.Module] = " (Module = N'" + this._Module + "')";
            this.buttonFilterForModule.Image = this.imageListFilter.Images[1];
            this.textBoxModule.BackColor = System.Drawing.Color.Pink;
            this.buttonFilterForModule.Tag = "Filtered";
            this.splitContainerMain.Panel1Collapsed = false;
            this.labelHeader.Visible = false;
            this.FilterDatasets();

            this.bindingNavigator.BindingSource = this.feedbackBindingSource;
            this.bindingNavigator.Visible = true;
            this.buttonSendReply.Visible = true;
            this.buttonNewEntry.Visible = true;
            this.buttonShowAll.Visible = true;
            this.buttonSortByDate.Visible = true;
            this.buttonSortByPriority.Visible = true;
            this.splitContainerDescription.Panel2Collapsed = false;

            this.buttonFilterForUser.Visible = true;
            this.buttonFilterForModule.Visible = true;
            this.buttonFilterForDatabase.Visible = true;
            this.buttonSave.Visible = true;
        }

        private void SetFormForEditor()
        {
            //this.OrderClause = " ORDER BY Priority, ToDoUntil, ID ";
            this.bindingNavigatorDeleteItem.Visible = false;
            this.WhereClauseParts[WhereClausePart.ToDo] = " (ProgressState <> N'Finished') AND (ProgressState <> N'NoError')";
            this.WhereClauseParts[WhereClausePart.Module] = " (Module = N'" + this._Module + "')";

            this.FilterDatasets();
            this.bindingNavigator.BindingSource = this.feedbackBindingSource;
            this.bindingNavigatorAddNewItem.Visible = false;
            this.bindingNavigator.Visible = true;

            this.labelImage.Visible = false;
            this.labelImage2.Visible = false;
            this.buttonInsertImage.Visible = false;
            this.labelHeader.Visible = true;
        }

        private void SetFormForUser()
        {
            this.splitContainerMain.Panel1Collapsed = false;
            this.splitContainerMain.SplitterDistance = 20;
            this.splitContainerLogData.Visible = false;
            this.labelHeader.Visible = true;
            this.labelHeader.Text = "Feedback sent by: " + this.ReportingUser + ".   Module: " + this._Module + ".   Database: " + this.Database;
            this.labelDateForHistory.Visible = true;
            this.buttonCancel.Visible = true;
            this.buttonSendFeedback.Visible = true;
            this.buttonFilterForTopic.Visible = false;
            this.textBoxDescription.Focus();
            this.buttonSendReply.Visible = false;
            this.labelState.Visible = false;
            this.labelProgress.Visible = false;
            this.comboBoxState.Visible = false;
            this.textBoxProgress.Visible = false;
            this.splitContainerDescription.Panel2Collapsed = true;
            this.textBoxDescription.Focus();
            this.bindingNavigatorDeleteItem.Visible = false;
            if (DiversityWorkbench.WorkbenchSettings.Default.FeedbackReplyAddress.Length > 0)
                this.textBoxReplyAddress.Text = DiversityWorkbench.WorkbenchSettings.Default.FeedbackReplyAddress;
        }

        private void FormFeedback_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((this.DialogResult != DialogResult.Abort &&
                this.DialogResult != DialogResult.Cancel)
                || this.DatabaseUser == "dbo")
            {
                try
                {
                    this.labelHeader.Focus();
                    System.Data.DataRowView R = (System.Data.DataRowView)this.feedbackBindingSource.Current;
                    R.BeginEdit();
                    R.EndEdit();
                    if (this.dataSetFeedback.HasChanges())
                        this.SqlDataAdapter.Update(this.dataSetFeedback.Feedback);
                    else if (this.dataSetFeedback.HasChanges() && this.feedbackTableAdapter != null)
                        this.feedbackTableAdapter.Update(this.dataSetFeedback.Feedback);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        public void setHelpProviderNameSpace(string HelpNameSpace, string Keyword)
        {
            try
            {
                this.helpProvider.HelpNamespace = HelpNameSpace;
                this.helpProvider.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
                this.helpProvider.SetHelpKeyword(this, Keyword);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Button events etc.

        private void comboBoxState_SelectedIndexChanged(object sender, EventArgs e)
        {
            string State = "";
            if (this.comboBoxState.SelectedValue != null)
            {
                State = this.comboBoxState.SelectedValue.ToString();
            }
            switch (State)
            {
                case "ToDo":
                    this.comboBoxState.BackColor = System.Drawing.Color.Pink;
                    break;
                case "Finished":
                    this.comboBoxState.BackColor = System.Drawing.Color.LightGreen;
                    break;
                case "InProgress":
                    this.comboBoxState.BackColor = System.Drawing.Color.Yellow;
                    break;
                case "Waiting":
                    this.comboBoxState.BackColor = System.Drawing.Color.Violet;
                    break;
                case "Unclear":
                    this.comboBoxState.BackColor = System.Drawing.Color.Violet;
                    break;
                case "NoError":
                    this.comboBoxState.BackColor = System.Drawing.Color.LightGreen;
                    break;
                default:
                    this.comboBoxState.BackColor = System.Drawing.Color.White;
                    break;
            }
            if (this.comboBoxState.Enabled == false)
                this.labelState.BackColor = this.comboBoxState.BackColor;
        }

        private void buttonInsertImage_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Clipboard.ContainsImage())
            {
                try
                {
                    System.Drawing.Image I = System.Windows.Forms.Clipboard.GetImage();
                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    I.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    this._BImage = (System.Byte[])ms.ToArray();
                    this.pictureBoxImage.Image = I;
                    this.pictureBoxImage.Width = I.Width;
                    this.pictureBoxImage.Height = I.Height;
                    if (this.feedbackBindingSource.Current != null)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.feedbackBindingSource.Current;
                        R["Image"] = this._BImage;
                    }
                }
                catch (Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
            else
                System.Windows.Forms.MessageBox.Show("No image in clipboard");
        }

        private void bindingNavigator_RefreshItems(object sender, EventArgs e)
        {
            try
            {
                if (this.feedbackBindingSource.Current == null)
                {
                    this.pictureBoxImage.Image = null;
                    return;
                }
                System.Data.DataRowView R = (System.Data.DataRowView)this.feedbackBindingSource.Current;
                if (R["Image"].Equals(System.DBNull.Value))
                {
                    this.pictureBoxImage.Image = null;
                    return;
                }
                System.Byte[] B = (System.Byte[])R["Image"];
                System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                this.pictureBoxImage.Width = I.Width;
                this.pictureBoxImage.Height = I.Height;
                //this.pictureBoxImage.Image = I;
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonSendFeedback_Click(object sender, EventArgs e)
        {
            if (this.textBoxReplyAddress.BackColor == System.Drawing.Color.Pink || this.textBoxReplyAddress.Text.Length == 0)
            {
                if (System.Windows.Forms.MessageBox.Show("You did not enter an email address for a reply.\r\nDo you want to enter an address to receive a response", "Enter reply address", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                    return;
            }
            Microsoft.Data.SqlClient.SqlConnection Connection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionStringForFeedback);
            try
            {
                string ErrorDescription = this.textBoxDescription.Text;
                string Date = this.textBoxToDoUntil.Text;
                string Priority = this.comboBoxPriority.SelectedValue.ToString();
                string Topic = this.comboBoxTopic.Text;
                this.dataSetFeedback.Feedback.Clear();
                DiversityWorkbench.Datasets.DataSetFeedback.FeedbackRow R = (DiversityWorkbench.Datasets.DataSetFeedback.FeedbackRow)this.dataSetFeedback.Feedback.NewRow();
                R.DatabaseName = this.Database;
                R.Server = this.Server;
                R.Module = this._Module;
                R.Version = this._Version;
                R.ReportedBy = this.ReportingUser;
                R.LogFile = this.ErrorLog;
                R.Image = this._BImage;
                R.QueryString = this._Query;
                R.CurrentID = this._ID;
                R.LogInsertedWhen = System.DateTime.Now;
                R.Description = ErrorDescription;
                R.Topic = Topic;
                R.Priority = Priority;
                if (DiversityWorkbench.WorkbenchSettings.Default.FeedbackReplyAddress != null)
                    R.ReplyAddress = DiversityWorkbench.WorkbenchSettings.Default.FeedbackReplyAddress;
                if (Date.Length > 0)
                {
                    DateTime Until;
                    if (System.DateTime.TryParse(Date, out Until))
                        R.ToDoUntil = Until;
                }
                this.dataSetFeedback.Feedback.Rows.Add(R);
                this.SqlDataAdapter.Update(this.dataSetFeedback.Feedback);
                System.Windows.Forms.MessageBox.Show("Feedback has been sent to administrator");
                this.SqlDataAdapter.SelectCommand.Connection.Close();
                this.SqlDataAdapter.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void buttonSendReply_Click(object sender, EventArgs e)
        {
            try
            {

                string Betreff = this.comboBoxTopic.Text;
                if (Betreff.Length == 0)
                    Betreff = this.textBoxDescription.Text;
                if (Betreff.Length > 80) Betreff = Betreff.Substring(0, 80) + "...";
                string mail = "mailto:" + this.textBoxReplyAddress.Text + "?subject=Feedback%20-%20"
                    + System.DateTime.Parse(this.textBoxDate.Text.ToString()).ToShortDateString()
                    + "%20-%20" + this.textBoxDatabase.Text + "%20-%20"
                    + Betreff; // + "&amp;body=Hallo " + this.textBoxUser.Text);
                System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(mail);
                info.UseShellExecute = true;
                System.Diagnostics.Process.Start(info);
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonNewEntry_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Datasets.DataSetFeedback.FeedbackRow R = (DiversityWorkbench.Datasets.DataSetFeedback.FeedbackRow)this.dataSetFeedback.Feedback.NewRow();
                R.DatabaseName = this.Database;
                R.Server = this.Server;
                R.Module = this._Module;
                R.Version = this._Version;
                R.ReportedBy = this.SystemUser;
                R.LogFile = this.ErrorLog;
                R.Image = this._BImage;
                R.QueryString = this._Query;
                R.CurrentID = this._ID;
                R.LogInsertedWhen = System.DateTime.Now;
                R.ProgressState = "ToDo";
                R.Description = "";
                this.dataSetFeedback.Feedback.Rows.Add(R);
                this.feedbackBindingSource.MoveLast();
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonShowAll_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                if (this.buttonShowAll.Tag == null || this.buttonShowAll.Tag.ToString().Length == 0)
                {
                    this.WhereClauseParts[WhereClausePart.ToDo] = "";
                    this.buttonShowAll.Tag = "NoFilter";
                    this.buttonShowAll.Image = this.imageListFilter.Images[0];
                    this.buttonShowAll.Text = "Show TODO";
                }
                else
                {
                    this.WhereClauseParts[WhereClausePart.ToDo] = " (ProgressState <> N'Finished') AND (ProgressState <> N'NoError')";
                    this.buttonShowAll.Tag = null;
                    this.buttonShowAll.Image = this.imageListFilter.Images[1];
                    this.buttonShowAll.Text = "Show all";
                }
                if (this.dataSetFeedback.Feedback.Rows.Count > 0)
                    this.SqlDataAdapter.Update(this.dataSetFeedback.Feedback);

                this.dataSetFeedback.Feedback.Clear();
                this.FilterDatasets();

                //this.buttonShowAll.Visible = false;
            }
            catch (System.Exception ex) { }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        #region Filtering the datasets

        private void buttonSearchForModule_Click(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT DISTINCT Module FROM DiversityWorkbenchFeedback.dbo.Feedback WHERE (Module <> N'') ORDER BY Module";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringForFeedback);
                ad.Fill(dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Module", "Module", "Module", "Select module for restriction");
                f.ShowDialog();
                if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    this.buttonFilterForModule.Image = this.imageListFilter.Images[1];
                    this.textBoxModule.BackColor = System.Drawing.Color.Pink;
                    this.buttonFilterForModule.Tag = "Filtered";
                    this.WhereClauseParts[WhereClausePart.Module] = "Module = '" + f.SelectedValue + "'";
                    this.FilterDatasets();
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonFilterForModule_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            if (this.textBoxModule.Text.Length > 0 || this.buttonFilterForModule.Tag != null)
            {
                this.FilterForModule(this.textBoxModule.Text);
            }
            else
                System.Windows.Forms.MessageBox.Show("Missing entry");
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void FilterForModule(string Module)
        {
            if (this.buttonFilterForModule.Tag != null)
            {
                this.buttonFilterForModule.Image = this.imageListFilter.Images[0];
                this.textBoxModule.BackColor = System.Drawing.Color.White;
                this.buttonFilterForModule.Tag = null;
                this.WhereClauseParts[WhereClausePart.Module] = "";
            }
            else
            {
                this.buttonFilterForModule.Image = this.imageListFilter.Images[1];
                this.textBoxModule.BackColor = System.Drawing.Color.Pink;
                this.buttonFilterForModule.Tag = "Filtered";
                this.WhereClauseParts[WhereClausePart.Module] = "Module = '" + Module + "'";
            }
            this.FilterDatasets();
        }

        private void buttonFilterForUser_Click(object sender, EventArgs e)
        {
            if (this.textBoxUser.Text.Length > 0)
            {
                if (this.buttonFilterForUser.Tag != null)
                {
                    this.buttonFilterForUser.Image = this.imageListFilter.Images[0];
                    this.textBoxUser.BackColor = System.Drawing.Color.White;
                    this.buttonFilterForUser.Tag = null;
                    this.WhereClauseParts[WhereClausePart.User] = "";
                }
                else
                {
                    this.buttonFilterForUser.Image = this.imageListFilter.Images[1];
                    this.textBoxUser.BackColor = System.Drawing.Color.Pink;
                    this.buttonFilterForUser.Tag = "Filtered";
                    this.WhereClauseParts[WhereClausePart.User] = "ReportedBy = '" + this.textBoxUser.Text + "'";
                }
                this.FilterDatasets();
                //string WhereClause = "ReportedBy = '" + this.textBoxUser.Text + "'";
                //this.FilterDatasets(WhereClause);
            }
            else
                System.Windows.Forms.MessageBox.Show("Missing entry");
        }

        private void buttonFilterForDatabase_Click(object sender, EventArgs e)
        {
            if (this.textBoxDatabase.Text.Length > 0)
            {
                if (this.buttonFilterForDatabase.Tag != null)
                {
                    this.buttonFilterForDatabase.Image = this.imageListFilter.Images[0];
                    this.textBoxDatabase.BackColor = System.Drawing.Color.White;
                    this.buttonFilterForDatabase.Tag = null;
                    this.WhereClauseParts[WhereClausePart.Database] = "";
                }
                else
                {
                    this.buttonFilterForDatabase.Image = this.imageListFilter.Images[1];
                    this.textBoxDatabase.BackColor = System.Drawing.Color.Pink;
                    this.buttonFilterForDatabase.Tag = "Filtered";
                    this.WhereClauseParts[WhereClausePart.Database] = "DatabaseName = '" + this.textBoxDatabase.Text + "'";
                }
                this.FilterDatasets();
                //string WhereClause = "DatabaseName = '" + this.textBoxDatabase.Text + "'";
                //this.FilterDatasets(WhereClause);
            }
            else
                System.Windows.Forms.MessageBox.Show("Missing entry");
        }

        private void buttonFilterForTopic_Click(object sender, EventArgs e)
        {
            if (this.comboBoxTopic.Text.Length > 0)
            {
                if (this.buttonFilterForTopic.Tag != null)
                {
                    this.buttonFilterForTopic.Image = this.imageListFilter.Images[0];
                    this.comboBoxTopic.BackColor = System.Drawing.Color.White;
                    this.buttonFilterForTopic.Tag = null;
                    this.WhereClauseParts[WhereClausePart.Topic] = "";
                }
                else
                {
                    this.buttonFilterForTopic.Image = this.imageListFilter.Images[1];
                    this.comboBoxTopic.BackColor = System.Drawing.Color.Pink;
                    this.buttonFilterForTopic.Tag = "Filtered";
                    this.WhereClauseParts[WhereClausePart.Topic] = "Topic = '" + this.comboBoxTopic.Text + "'";
                }
                this.FilterDatasets();
                //string WhereClause = "DatabaseName = '" + this.textBoxDatabase.Text + "'";
                //this.FilterDatasets(WhereClause);
            }
            else
                System.Windows.Forms.MessageBox.Show("Missing entry");
        }

        private void FilterDatasets()
        {
            try
            {
                this.labelHeader.Focus();
                if (this.feedbackBindingSource.Current != null)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.feedbackBindingSource.Current;
                    R.BeginEdit();
                    R.EndEdit();
                    if (this.dataSetFeedback.HasChanges())
                        this.SqlDataAdapter.Update(this.dataSetFeedback.Feedback);
                }
                this.dataSetFeedback.Feedback.Clear();
                string SQL = "SELECT ID, Image, Description, ProgressState, Progress, LogFile, Module, Version, " +
                    "DatabaseName, Server, ReportedBy, ReplyAddress, QueryString, CurrentID, LogInsertedWhen, Priority, ToDoUntil, Topic " +
                    "FROM DiversityWorkbenchFeedback.dbo.Feedback ";
                if (this.WhereClause.Length > 0)
                    SQL += "WHERE " + this.WhereClause;
                SQL += this.OrderClause;
                this.SqlDataAdapter.SelectCommand.CommandText = SQL;
                this.SqlDataAdapter.Fill(this.dataSetFeedback.Feedback);
            }
            catch (System.Exception ex) { }
        }

        #endregion

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                this.labelHeader.Focus();
                System.Data.DataRowView R = (System.Data.DataRowView)this.feedbackBindingSource.Current;
                R.BeginEdit();
                R.EndEdit();
                if (this.dataSetFeedback.HasChanges())
                    this.SqlDataAdapter.Update(this.dataSetFeedback.Feedback);
            }
            catch (System.Exception ex) { }
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void buttonRequery_Click(object sender, EventArgs e)
        {
            //this.dataSetFeedback.Feedback.Clear();
            //string SQL = "SELECT ID, Image, Description, ProgressState, Progress, LogFile, Module, Version, " +
            //    "DatabaseName, Server, ReportedBy, ReplyAddress, QueryString, CurrentID, LogInsertedWhen " +
            //    "FROM DiversityWorkbenchFeedback.dbo.Feedback " +
            //    "WHERE (ProgressState <> N'Finished') AND (ProgressState <> N'NoError') " +
            //    "ORDER BY LogInsertedWhen";
            //Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.ConnectionStringForFeedback);
            ////Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, this.feedbackTableAdapter.Connection.ConnectionString);
            //ad.Fill(this.dataSetFeedback.Feedback);
        }

        private void dateTimePickerToDoUntil_CloseUp(object sender, EventArgs e)
        {
            string Date = this.dateTimePickerToDoUntil.Value.Year.ToString() + ".";
            if (this.dateTimePickerToDoUntil.Value.Month < 10)
                Date += "0";
            Date += this.dateTimePickerToDoUntil.Value.Month.ToString() + ".";
            if (this.dateTimePickerToDoUntil.Value.Day < 10)
                Date += "0";
            Date += this.dateTimePickerToDoUntil.Value.Day.ToString();
            this.textBoxToDoUntil.Text = Date;
        }

        private void comboBoxPriority_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBoxPriority.SelectedItem == null)
            {
                this.textBoxDescription.ForeColor = System.Drawing.Color.Black;
                this.textBoxDescription.BackColor = System.Drawing.Color.White;
                return;
            }

            this._Priority = "";
            System.Data.DataRowView R = (System.Data.DataRowView)comboBoxPriority.SelectedItem;
            this._Priority = R["Code"].ToString();
            /*
            as soon as possible
            nice to have
            rejected
            urgent
            */
            switch (this._Priority)
            {
                case "as soon as possible":
                    this.textBoxDescription.ForeColor = System.Drawing.Color.Black;
                    this.textBoxDescription.BackColor = System.Drawing.Color.Yellow;
                    break;
                case "nice to have":
                    this.textBoxDescription.ForeColor = System.Drawing.Color.Black;
                    this.textBoxDescription.BackColor = System.Drawing.Color.LightGreen;
                    break;
                case "urgent":
                    this.textBoxDescription.ForeColor = System.Drawing.Color.Black;
                    this.textBoxDescription.BackColor = System.Drawing.Color.Pink;
                    break;
                case "rejected":
                    this.textBoxDescription.ForeColor = System.Drawing.Color.LightGray;
                    this.textBoxDescription.BackColor = System.Drawing.Color.White;
                    break;
                case "":
                    this.textBoxDescription.ForeColor = System.Drawing.Color.Black;
                    this.textBoxDescription.BackColor = System.Drawing.Color.White;
                    break;
            }
        }

        private void comboBoxTopic_DropDown(object sender, EventArgs e)
        {
            try
            {
                string SQL = "SELECT NULL AS Topic UNION SELECT Topic FROM DiversityWorkbenchFeedback.dbo.Feedback";
                if (this.WhereClauseParts[WhereClausePart.Module].Length > 0)
                    SQL += " WHERE " + this.WhereClauseParts[WhereClausePart.Module];
                SQL += " GROUP BY Topic ";
                if (this.comboBoxTopic.Text.Length > 0)
                    SQL += " HAVING (Topic LIKE N'" + this.comboBoxTopic.Text + "%') ";
                SQL += " ORDER BY Topic";
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, ConnectionStringForFeedback);
                ad.Fill(dt);
                this.comboBoxTopic.DataSource = dt;
                this.comboBoxTopic.DisplayMember = "Topic";
                this.comboBoxTopic.ValueMember = "Topic";
            }
            catch (System.Exception ex)
            {
            }
        }

        #endregion

        #region Properties

        public enum WhereClausePart { ToDo, Module, User, Database, ID, Topic };

        public System.Collections.Generic.Dictionary<WhereClausePart, string> WhereClauseParts
        {
            get
            {
                if (this._WhereClauseParts == null)
                {
                    this._WhereClauseParts = new Dictionary<WhereClausePart, string>();
                    this._WhereClauseParts.Add(WhereClausePart.ToDo, "");//(ProgressState <> N'Finished') AND (ProgressState <> N'NoError')");
                    this._WhereClauseParts.Add(WhereClausePart.Module, "");
                    this._WhereClauseParts.Add(WhereClausePart.User, "");
                    this._WhereClauseParts.Add(WhereClausePart.Database, "");
                    this._WhereClauseParts.Add(WhereClausePart.ID, "");
                    this._WhereClauseParts.Add(WhereClausePart.Topic, "");
                }
                return _WhereClauseParts;
            }
            //set { _WhereClauseParts = value; }
        }

        public string WhereClause
        {
            get
            {
                string WhereClause = "";
                foreach (System.Collections.Generic.KeyValuePair<WhereClausePart, string> KV in this.WhereClauseParts)
                {
                    if (KV.Value.Length > 0)
                    {
                        if (WhereClause.Length > 0)
                            WhereClause += " AND ";
                        WhereClause += KV.Value;
                    }
                }
                return WhereClause;
            }
        }

        private string _OrderClause;

        public string OrderClause
        {
            get
            {
                if (this._OrderClause == null)
                {
                    this._OrderClause = "";
                    if (this._PrioritySorting != Forms.FormFeedbackSorting.Sorting.Unsorted)
                    {
                        this._OrderClause += " Priority ";
                        if (this._PrioritySorting == Forms.FormFeedbackSorting.Sorting.Descending)
                            this._OrderClause += "DESC";
                    }
                    if (this._DateSorting != Forms.FormFeedbackSorting.Sorting.Unsorted)
                    {
                        if (this._OrderClause.Length > 0)
                            this._OrderClause += ", ";
                        this._OrderClause += " ToDoUntil ";
                        if (this._DateSorting == Forms.FormFeedbackSorting.Sorting.Descending)
                            this._OrderClause += "DESC";
                    }
                    if (this._OrderClause.Length > 0)
                        this._OrderClause += ", ";
                    this._OrderClause = " ORDER BY " + this._OrderClause + "ID";
                }
                return _OrderClause;
            }
            //set { _OrderClause = value; }
        }

        private string ErrorLog
        {
            get
            {
                string Log = "";
                //string CurrentDate = System.DateTime.Now.ToString();
                if (this._ErrorLog.Exists)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(this._ErrorLog.FullName); ;
                    using (sr)
                    {
                        string line = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("Date:"))
                            {
                                string DateInLine = line.Substring(6).Trim();
                                System.DateTime D;
                                if (System.DateTime.TryParse(DateInLine, out D))
                                {
                                    if (D.ToShortDateString() == System.DateTime.Now.ToShortDateString())
                                    {
                                        Log += line + "\r\n";
                                        break;
                                    }
                                }
                            }
                        }
                        while ((line = sr.ReadLine()) != null)
                        {
                            Log += line + "\r\n";
                        }
                    }
                }
                return Log;
            }
        }

        private string ConnectionStringForFeedback
        {
            get
            {
                if (_FormState == FormState.Editing)
                    return Feedback.ConnectionStringFeedbackEditing;

                if (this._ConnectionString.Length > 0)
                    return this._ConnectionString;

                // try the current connection
                if (this.CanGetConnectionToFeedback(DiversityWorkbench.Settings.ConnectionString))
                {
                    this._ConnectionString = DiversityWorkbench.Settings.ConnectionString;
                    return this._ConnectionString;
                }

                // try to replace the datasource
                DiversityWorkbench.ServerConnection S = new ServerConnection(DiversityWorkbench.Settings.ConnectionString);
                string feedbackServer = global::DiversityWorkbench.Properties.Settings.Default.FeedbackDatabaseServer;
                string[] serverValues = feedbackServer.Split(',');
                if (serverValues?.Length == 2)
                {
                    S.DatabaseServer = serverValues[0];
                    S.DatabaseServerPort = int.Parse(serverValues[1]);
                }

                if (this.CanGetConnectionToFeedback(S.ConnectionString))
                {
                    this._ConnectionString = S.ConnectionString;
                    return this._ConnectionString;
                }

                // try FeedBack account
                string ConnectionString = global::DiversityWorkbench.Properties.Settings.Default.DiversityWorkbenchFeedbackConnectionString;
                if (this.CanGetConnectionToFeedback(ConnectionString))
                {
                    this._ConnectionString = ConnectionString;
                    return this._ConnectionString;
                }

                return this._ConnectionString;
            }
        }

        private bool CanGetConnectionToFeedback(string ConnectionString)
        {
            bool OK = false;
            //string SQL = "SELECT COUNT(*) FROM Feedback";
            string SQL = "SELECT COUNT(*) FROM DiversityWorkbenchFeedback.dbo.Feedback";

            Microsoft.Data.SqlClient.SqlConnection Connection = new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, Connection);
            try
            {
                Connection.Open();
                C.ExecuteNonQuery();
                Connection.Close();
                OK = true;
            }
            catch (System.Exception ex)
            {
                OK = false;
            }
            return OK;
        }

        private string UserName
        {
            get
            {
                if (this._User != "")
                    return this._User;
                try
                {
                    this._User = System.Environment.UserName;

                    string SQL = "SELECT USER_NAME()";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringForFeedback);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con); // = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    string User = "";
                    try
                    {
                        User = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    }
                    catch { }
                    if (User.Length > 0)
                    {
                        if (User.ToLower() == "feedback")
                            User = System.Environment.UserName;
                        this._User = User;
                        if (this._DatabaseUser.Length == 0)
                            this._DatabaseUser = User;
                    }
                    con.Close();
                }
                catch (System.Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show("Please connect to a database\r\n" + ex.Message);
                    //this.Close();
                }
                return this._User;
            }
        }

        public string ReportingUser
        {
            get
            {
                if (_ReportingUser.Length == 0)
                {
                    string SQL = "select USER_NAME()";
                    _ReportingUser = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    if (_ReportingUser == "dbo")
                    {
                        SQL = "SELECT SUSER_SNAME()";
                        _ReportingUser = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                    }
                    if (_ReportingUser.Length == 0 || _ReportingUser == "dbo")
                        _ReportingUser = System.Environment.UserName;
                }
                return _ReportingUser;
            }
            set { _ReportingUser = value; }
        }

        public string DatabaseUser
        {
            get
            {
                if (this._DatabaseUser.Length > 0)
                    return _DatabaseUser;
                try
                {
                    this._User = System.Environment.UserName;

                    string SQL = "SELECT USER_NAME()";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringForFeedback);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con); // = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    string User = "";
                    try
                    {
                        User = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    }
                    catch { }
                    if (User.Length > 0)
                    {
                        this._User = User;
                        if (this._DatabaseUser.Length == 0)
                            this._DatabaseUser = User;
                    }
                    con.Close();
                }
                catch (System.Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show("Please connect to a database\r\n" + ex.Message);
                    //this.Close();
                }
                return this._DatabaseUser;
            }
        }

        public string SystemUser
        {
            get
            {
                if (this._DatabaseUser.Length > 0)
                    return _DatabaseUser;
                try
                {
                    this._User = System.Environment.UserName;

                    string SQL = "SELECT SUSER_SNAME()";
                    Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(this.ConnectionStringForFeedback);
                    Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con); // = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
                    con.Open();
                    string User = "";
                    try
                    {
                        User = C.ExecuteScalar()?.ToString() ?? string.Empty;
                    }
                    catch { }
                    if (User.Length > 0)
                    {
                        this._User = User;
                        if (this._DatabaseUser.Length == 0)
                            this._DatabaseUser = User;
                    }
                    con.Close();
                }
                catch (System.Exception ex)
                {
                    //System.Windows.Forms.MessageBox.Show("Please connect to a database\r\n" + ex.Message);
                    //this.Close();
                }
                return this._DatabaseUser;
            }
        }

        private System.Collections.Generic.List<string> _DatabaseRoles;
        public System.Collections.Generic.List<string> DatabaseRoles()
        {
            if (this._DatabaseRoles == null)
            {
                this._DatabaseRoles = DiversityWorkbench.Database.DatabaseRoles(this.ConnectionStringForFeedback);
            }
            return this._DatabaseRoles;
        }

        public string Database
        {
            get
            {
                if (this._Database != null && this._Database.Length > 0)
                    return this._Database;
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                this._Database = con.Database;
                return _Database;
            }
        }

        public string Server
        {
            get
            {
                if (this._Server.Length > 0)
                    return this._Server;
                Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
                this._Server = con.DataSource;
                return _Server;
            }
        }

        #endregion

        #region Sorting

        private DiversityWorkbench.Forms.FormFeedbackSorting.Sorting _DateSorting = Forms.FormFeedbackSorting.Sorting.Unsorted;
        private DiversityWorkbench.Forms.FormFeedbackSorting.Sorting _PrioritySorting = Forms.FormFeedbackSorting.Sorting.Unsorted;

        private void buttonSortByDate_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFeedbackSorting f = new Forms.FormFeedbackSorting(Forms.FormFeedbackSorting.SortTarget.Date, this._DateSorting);
            f.ShowDialog();
            this._DateSorting = f.SelectedSorting();
            switch (this._DateSorting)
            {
                case Forms.FormFeedbackSorting.Sorting.Unsorted:
                    this.buttonSortByDate.Image = null;// 
                    break;
                case Forms.FormFeedbackSorting.Sorting.Ascending:
                    this.buttonSortByDate.Image = this.imageListSortBy.Images[1];
                    break;
                case Forms.FormFeedbackSorting.Sorting.Descending:
                    this.buttonSortByDate.Image = this.imageListSortBy.Images[0];
                    break;

            }
            this._OrderClause = null;
            this.FilterDatasets();
        }

        private void buttonSortByPriority_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormFeedbackSorting f = new Forms.FormFeedbackSorting(Forms.FormFeedbackSorting.SortTarget.Priority, this._PrioritySorting);
            f.ShowDialog();
            this._PrioritySorting = f.SelectedSorting();
            switch (this._PrioritySorting)
            {
                case Forms.FormFeedbackSorting.Sorting.Unsorted:
                    this.buttonSortByPriority.Image = null;// 
                    break;
                case Forms.FormFeedbackSorting.Sorting.Ascending:
                    this.buttonSortByPriority.Image = this.imageListSortBy.Images[1];
                    break;
                case Forms.FormFeedbackSorting.Sorting.Descending:
                    this.buttonSortByPriority.Image = this.imageListSortBy.Images[0];
                    break;

            }
            this._OrderClause = null;
            this.FilterDatasets();
        }

        #endregion

        #region Email

        private void textBoxReplyAddress_TextChanged(object sender, EventArgs e)
        {
            if (this.IsValidEmail(this.textBoxReplyAddress.Text))
            {
                this.textBoxReplyAddress.BackColor = System.Drawing.Color.White;
                DiversityWorkbench.WorkbenchSettings.Default.FeedbackReplyAddress = this.textBoxReplyAddress.Text;
            }
            else
                this.textBoxReplyAddress.BackColor = System.Drawing.Color.Pink;
        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}