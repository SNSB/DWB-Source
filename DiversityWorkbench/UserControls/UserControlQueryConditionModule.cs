using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.UserControls
{
    public partial class UserControlQueryConditionModule : UserControl, DiversityWorkbench.IUserControlQueryCondition
    {

        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryCondition;

        public DiversityWorkbench.QueryCondition QueryCondition
        {
            get { return _QueryCondition; }
        }
        private string _AutoCompletionStarter;

        private UserControlQueryList _UserControlQueryList;
        public void setUserControlQueryList(UserControlQueryList List)
        {
            this._UserControlQueryList = List;
        }

        /// <summary>
        ///  The dictionary containing the results of the query where the key korresponds to the URI and the value to the display text
        /// </summary>
        private System.Collections.Generic.Dictionary<string, string> _Values = new Dictionary<string, string>();

        /// <summary>
        ///  The dictionary containing the starting result of the query where the key korresponds to the URI and the value to the display text
        ///  used for queries including hierarchy and synonymy to fix the starting point
        /// </summary>
        private System.Collections.Generic.Dictionary<string, string> _StartValue = new Dictionary<string, string>();

        private string _Domain = "";

        public string Domain
        {
            get { return _Domain; }
            set { _Domain = value; }
        }

        private string _BaseURL;

        public string BaseURL
        {
            get { return _BaseURL; }
            set
            {
                _BaseURL = value;
                this.buttonBaseURL.Text = value;
                if (value.Length > 0)
                    this.buttonBaseURL.Image = DiversityWorkbench.ResourceWorkbench.Database;
                else
                    this.buttonBaseURL.Image = null;
            }
        }

        #endregion

        #region Construction

        public UserControlQueryConditionModule()
        {
            InitializeComponent();
        }

        public UserControlQueryConditionModule(DiversityWorkbench.QueryCondition Q, string ConnectionString)
            : this()
        {
            try
            {
                this.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(32); //#139 Originalwert: 32
                this._QueryCondition = Q;
                this.AccessibleName = this._QueryCondition.Table + "." + this._QueryCondition.Column;
                this._QueryCondition.QueryType = QueryCondition.QueryTypes.Module;

                this.setControls();
            }
            catch (System.Exception ex)
            {

            }
        }

        #endregion

        #region Interface

        public DiversityWorkbench.QueryCondition Condition()
        {
            return this._QueryCondition;
        }

        public DiversityWorkbench.QueryCondition getCondition()
        {
            DiversityWorkbench.QueryCondition QC = new QueryCondition();
            QC = this._QueryCondition;
            QC.QueryConditionOperator = this.buttonQueryConditionOperator.Text;
            if (this._Values != null && this._Values.Count > 0 && (QC.dtValues == null || QC.dtValues.Rows.Count != this._Values.Count))
            {
                System.Data.DataTable dt = new DataTable();
                System.Data.DataColumn dcURL = new DataColumn("URL", typeof(string));
                System.Data.DataColumn dcDisplay = new DataColumn("Display", typeof(string));
                dt.Columns.Add(dcURL);
                dt.Columns.Add(dcDisplay);
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Values)
                {
                    System.Data.DataRow R = dt.NewRow();
                    R["URL"] = KV.Key;
                    R["Display"] = KV.Value;
                    dt.Rows.Add(R);
                }
                QC.dtValues = dt;
            }
            return QC;
        }

        public void setConditionValues(DiversityWorkbench.QueryCondition Condition)
        {
            this.buttonQueryConditionOperator.Text = Condition.QueryConditionOperator;
            this.comboBoxQueryConditionOperator.Text = Condition.QueryConditionOperator;
            if (Condition.dtValues != null && Condition.dtValues.Rows.Count > 0)
            {
                this._Values = new Dictionary<string, string>();
                foreach (System.Data.DataRow R in Condition.dtValues.Rows)
                    this._Values.Add(R[0].ToString(), R[1].ToString());
                string sBaseURL = Condition.dtValues.Rows[0][0].ToString();
                sBaseURL = sBaseURL.Substring(0, sBaseURL.LastIndexOf('/'));
                this.BaseURL = sBaseURL;
                //this.buttonBaseURL.Text = BaseURL;
                this.labelItemCount.Text = this._Values.Count.ToString();
            }
            if (Condition.Value != null)
                this._QueryCondition.Value = Condition.Value;
            this._QueryCondition.IsBoolean = Condition.IsBoolean; // Used to decide if text of Uri should be selected
            this._QueryCondition.UpperValue = Condition.UpperValue; // Used to set the text column
        }

        public string ConditionValue()
        {
            string Value = "";
            return Value;
        }

        public string WhereClause()
        {
            if (this.buttonQueryConditionOperator.Text == " ")
            {
                return "";
            }
            if (UserControlQueryList.UseOptimizing)
                return this.WhereClauseOptimizing();
            else
            {
                string QueryString = "";
                try
                {
                    if (this._Values != null && this._Values.Count > 0)
                    {
                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Values)
                        {
                            if (QueryString.Length > 0)
                                QueryString += ", ";
                            if (this._QueryCondition.UpperValue != null && this._QueryCondition.UpperValue.Length > 0 && this._QueryCondition.IsBoolean)
                                QueryString += "'" + KV.Value.Replace("'", "''") + "'";
                            else
                                QueryString += "'" + KV.Key + "'";
                        }
                        QueryString = " IN(" + QueryString + ") ";
                        if (this.buttonQueryConditionOperator.Text == "∉")
                            QueryString = " NOT" + QueryString;
                    }
                    if (QueryString.Length > 0)
                    {
                        if (this._QueryCondition.UpperValue != null && this._QueryCondition.UpperValue.Length > 0 && this._QueryCondition.IsBoolean)
                            QueryString = "T." + this._QueryCondition.UpperValue + QueryString;
                        else
                            QueryString = "T." + this._QueryCondition.Column + QueryString;
                    }
                }
                catch (System.Exception ex)
                {
                }
                return QueryString;
            }
        }

        public string SQL()
        {
            string QueryString = "";
            if (this.WhereClause().Length > 0)
            {
                if (this._QueryCondition.SqlFromClause != null && this._QueryCondition.SqlFromClause.Length > 0)
                {
                    QueryString = "SELECT " + this._QueryCondition.IdentityColumn + " " + this._QueryCondition.SqlFromClause +
                        " AND " + this.WhereClause();
                }
                else
                {
                    QueryString = "SELECT " + this._QueryCondition.IdentityColumn + " FROM ";
                    if (this._UserControlQueryList != null &&
                        this._UserControlQueryList.LinkedServer.Length > 0 &&
                        this._UserControlQueryList.LinkedServerDatabase.Length > 0)
                        QueryString += "[" + this._UserControlQueryList.LinkedServer + "].[" + this._UserControlQueryList.LinkedServerDatabase + "].dbo.";
                    QueryString += this._QueryCondition.Table + " AS T " +
                        " WHERE " + this.WhereClause();
                }
            }
            return QueryString;
        }

        public string SqlByIndex(int FieldSequence)
        {
            string QueryString = "";
            if (this._QueryCondition.QueryFields != null && FieldSequence < this._QueryCondition.QueryFields.Count)
            {
                this._QueryCondition.Table = _QueryCondition.QueryFields[FieldSequence].TableName;
                this._QueryCondition.Column = _QueryCondition.QueryFields[FieldSequence].ColumnName;
                this._QueryCondition.IdentityColumn = _QueryCondition.QueryFields[FieldSequence].IdentityColumn;
            }
            else return "";
            if (this.WhereClause().Length > 0)
            {
                if (this._QueryCondition.SqlFromClause.Length > 0)
                {
                    QueryString = "SELECT " + this._QueryCondition.IdentityColumn + " " + this._QueryCondition.SqlFromClause +
                        " AND " + this.WhereClause();
                }
                else
                {
                    // MW 9.4.2015: Optimizing
                    if (UserControlQueryList.UseOptimizing)
                    {
                        if (this._QueryCondition.QueryFields[FieldSequence].TableName == UserControlQueryList.QueryMainTable)
                        {
                            string x = this.WhereClause();
                            QueryString = "T." + x.Substring(x.IndexOf('.') + 1);
                        }
                        else
                        {
                            //if (UserControlQueryList.TableAliases.ContainsKey(this._QueryCondition.Table))
                            {
                                QueryString = "T." + this._QueryCondition.IdentityColumn + " IN (SELECT " + this._QueryCondition.IdentityColumn +
                                    " FROM ";
                                if (this._UserControlQueryList != null &&
                                    this._UserControlQueryList.LinkedServer.Length > 0 &&
                                    this._UserControlQueryList.LinkedServerDatabase.Length > 0)
                                    QueryString += "[" + this._UserControlQueryList.LinkedServer + "].[" + this._UserControlQueryList.LinkedServerDatabase + "].dbo.";
                                QueryString += this._QueryCondition.Table +
                                    " WHERE " + this.WhereClause() + ")";
                                //QueryString = " = " + UserControlQueryList.TableAliases[this._QueryCondition.Table] + "." + this._QueryCondition.IdentityColumn + " AND " + this.WhereClause();
                            }
                        }
                    }
                    else
                    {
                        QueryString = "SELECT " + this._QueryCondition.IdentityColumn + " FROM ";
                        if (this._UserControlQueryList != null &&
                            this._UserControlQueryList.LinkedServer.Length > 0 &&
                            this._UserControlQueryList.LinkedServerDatabase.Length > 0)
                            QueryString += "[" + this._UserControlQueryList.LinkedServer + "].[" + this._UserControlQueryList.LinkedServerDatabase + "].dbo.";
                        QueryString += this._QueryCondition.Table + " AS T " +
                            " WHERE " + this.WhereClause();
                    }
                }
            }
            return QueryString;
        }

        public void Clear()
        {
            this.comboBoxQueryConditionOperator.SelectedIndex = 0;
            this._Values.Clear();
            if (this._QueryCondition.dtValues != null)
                this._QueryCondition.dtValues.Clear();
            this.BaseURL = "";
            this.setControls();
        }

        public void setEntity()
        {
            if (!DiversityWorkbench.Entity.EntityTablesExist) return;
            if (this._QueryCondition.TextFixed)
            {
                this.toolTipQueryCondition.SetToolTip(this.buttonCondition, this._QueryCondition.Description);
                return;
            }
            string Description = "";
            string Abbreviation = "";
            System.Collections.Generic.Dictionary<string, string> EntityDict = new Dictionary<string, string>();
            if (this._QueryCondition.Entity != null && this._QueryCondition.Entity.Length > 0 && !this._QueryCondition.useGroupAsEntityForGroups)
            {
                EntityDict = DiversityWorkbench.Entity.EntityInformation(this._QueryCondition.Entity);
                Description = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, EntityDict);
                Abbreviation = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDict);
            }
            if (Description.Length == 0 || Abbreviation.Length == 0)
            {
                string Table = this._QueryCondition.Table;
                if (Table.EndsWith("_Core")) Table = Table.Substring(0, Table.Length - 5);
                string Entity = Table + "." + this._QueryCondition.Column;
                EntityDict = DiversityWorkbench.Entity.EntityInformation(Entity);
                if (Description.Length == 0)
                    Description = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Description, EntityDict);
                if (Abbreviation.Length == 0)
                    Abbreviation = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.Abbreviation, EntityDict);
                if (Abbreviation.Length == 0
                    && this.AccessibleDescription != null
                    && this.AccessibleDescription.Length > 0)
                    Abbreviation = this.AccessibleDescription;
                if (this.AccessibleDescription == null)
                    this.AccessibleDescription = this.buttonCondition.Text;
            }
        }

        public string DisplayText { get { return this.buttonCondition.Text; } }

        public void RefreshProjectDependentHierarchy() { }

        #endregion

        private void setQueryConditionOperators()
        {
            this.comboBoxQueryConditionOperator.Items.Clear();

            this.comboBoxQueryConditionOperator.Items.Add(" ");
            this.comboBoxQueryConditionOperator.Items.Add("∈");
            this.comboBoxQueryConditionOperator.Items.Add("∉");
            switch (this._QueryCondition.iWorkbenchUnit.getServerConnection().ModuleName)
            {
                case "DiversityTaxonNames":
                    this.comboBoxQueryConditionOperator.Items.Add("+H");
                    this.comboBoxQueryConditionOperator.Items.Add("+S");
                    this.comboBoxQueryConditionOperator.Items.Add("+HS");
                    break;
                case "DiversityScientificTerms":
                    this.comboBoxQueryConditionOperator.Items.Add("∈+S");
                    this.comboBoxQueryConditionOperator.Items.Add("+S");
                    this.comboBoxQueryConditionOperator.Items.Add("+HS");
                    break;
                case "DiversitySamplingPlots":
                    this.comboBoxQueryConditionOperator.Items.Add("+H");
                    break;
                case "DiversityAgents":
                    this.comboBoxQueryConditionOperator.Items.Add("+H");
                    this.comboBoxQueryConditionOperator.Items.Add("+S");
                    this.comboBoxQueryConditionOperator.Items.Add("+HS");
                    break;
                case "DiversityDescriptions":
                    this.comboBoxQueryConditionOperator.Items.Clear();
                    this.comboBoxQueryConditionOperator.Items.Add("+T");
                    break;
                default:
                    break;
            }
        }


        private void setQueryConditionOperator()
        {
            this._QueryCondition.Operator = this.buttonQueryConditionOperator.Text;

        }

        private void setOperatorTooltip()
        {
            string Message = DiversityWorkbench.UserControls.UserControlQueryConditionText.search_for_entries.Substring(0, 1).ToUpper()
                        + DiversityWorkbench.UserControls.UserControlQueryConditionText.search_for_entries.Substring(1) + " ";
            switch (this.buttonQueryConditionOperator.Text)
            {
                case "∈":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.within_the_range_of_the_given_values;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                case "∉":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.not_within_the_range_of_the_given_values;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                case "+H":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.including_lower_hierarchy;
                    this.toolTipQueryCondition.SetToolTip(this.pictureBoxQueryOperator, Message);
                    break;
                case "+S":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.including_synonyms;
                    this.toolTipQueryCondition.SetToolTip(this.pictureBoxQueryOperator, Message);
                    break;
                case "+HS":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.including_lower_hierarchy_and_synonyms;
                    this.toolTipQueryCondition.SetToolTip(this.pictureBoxQueryOperator, Message);
                    break;
                case "∈+S":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.within_the_range_of_the_given_values + " + "
                        + DiversityWorkbench.UserControls.UserControlQueryConditionText.including_synonyms;
                    this.toolTipQueryCondition.SetToolTip(this.pictureBoxQueryOperator, Message);
                    break;
                default:
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.within_the_range_of_the_given_values;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
            }
        }

        private void setControls()
        {
            if (this._QueryCondition.DisplayText.Length > 0)
            {
                this.buttonCondition.Text = this._QueryCondition.DisplayText;
            }
            if (this._Values != null)
                this.labelItemCount.Text = this._Values.Count.ToString();

            if (this._QueryCondition.dtValues != null && this._QueryCondition.dtValues.Rows.Count > 0 && this._Values == null)
            {
                try
                {
                    this._Values = new Dictionary<string, string>();
                    foreach (System.Data.DataRow R in this._QueryCondition.dtValues.Rows)
                    {
                        this._Values.Add(R[0].ToString(), R[1].ToString());
                    }
                }
                catch (System.Exception ex)
                {

                }
            }

            string Module = this._QueryCondition.iWorkbenchUnit.getServerConnection().ModuleName.Replace("Diversity", "");
            this.labelModule.Text = Module;

            this.setQueryConditionOperators();

            this.setQueryConditionOperator();

            if (this._QueryCondition.Operator.Length > 0)
            {
                this.buttonModuleConnection.Enabled = true;
            }
            else
            {
                this.buttonModuleConnection.Enabled = false;
            }

            this.setOperatorTooltip();

            if (this._Values != null && this._Values.Count > 0)
            {
                this.labelItemCount.Text = this._Values.Count.ToString();
                string BaseURL = "";
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Values)
                {
                    BaseURL = KV.Key.Substring(0, KV.Key.LastIndexOf('/') + 1);
                    if (BaseURL.Length > 0)
                        break;
                }
                //this.buttonBaseURL.Text = BaseURL;
            }
            if (this._QueryCondition.UpperValue != null && this._QueryCondition.UpperValue.Length > 0)
            {
                this._QueryCondition.IsBoolean = !this._QueryCondition.IsBoolean;
                this.switchFilter();
            }
            else
            {
                //this.tableLayoutPanel.RowCount = 1;
                this.pictureBoxTextOrUri.Visible = false;
                this.switchFilter();
                //this.pictureBoxTextOrUri.Height = 1;
                //this.tableLayoutPanel.Controls.Remove(this.pictureBoxTextOrUri);
                //this.tableLayoutPanel.Controls.Remove(this.labelTextOrUri);
                //this.labelTextOrUri.Visible = false;
                //this.labelTextOrUri.Height = 1;
                //this.labelTextOrUri.Text = "";
                //this.labelTextOrUri.AutoSize = true;
                //// hier sollte 26 stehen. Das Ausblenden der obigen Controls klappt nicht
                //this.Height = 30;
            }
        }


        private string WhereClauseOptimizing()
        {
            string QueryString = "";
            string Column = this._QueryCondition.Column;
            if (this._QueryCondition.UpperValue != null && this._QueryCondition.UpperValue.Length > 0 && this._QueryCondition.IsBoolean)
                Column = this._QueryCondition.UpperValue;

            try
            {
                if (this._Values != null && this._Values.Count > 0)
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Values)
                    {
                        if (QueryString.Length > 0)
                            QueryString += ", ";
                        if (this._QueryCondition.UpperValue != null && this._QueryCondition.UpperValue.Length > 0 && this._QueryCondition.IsBoolean)
                        {
                            QueryString += "'" + KV.Value.Replace("'", "''") + "'";

                        }
                        else
                        {
                            QueryString += "'" + KV.Key + "'";

                        }
                    }
                    QueryString = " IN(" + QueryString + ") ";
                    if (this.buttonQueryConditionOperator.Text == "∉")
                        QueryString = " NOT" + QueryString;
                }
                if (QueryString.Length > 0)
                {
                    if (UserControlQueryList.UseOptimizing)
                    {
                        if (UserControlQueryList.TableAliases.ContainsKey(this._QueryCondition.Table))
                            QueryString = UserControlQueryList.TableAliases[this._QueryCondition.Table] + ".[" + Column + "]" + QueryString;
                        else
                        {
                            if (this._QueryCondition.Table != UserControlQueryList.QueryMainTable)
                            {
                                if (!UserControlQueryList.TableAliases.ContainsKey(this._QueryCondition.Table)) // Markus 14.02.2018 - vermutlich nur notwendig wenn es fehlt
                                    UserControlQueryList.TableAliases.Add(this._QueryCondition.Table, "T" + UserControlQueryList.TableAliases.Count.ToString());
                                string Alias = UserControlQueryList.TableAliases[this._QueryCondition.Table];
                                QueryString = Alias + ".[" + Column + "]" + QueryString;
                            }
                        }
                        if (this._QueryCondition.Restriction != null && this._QueryCondition.Restriction.Length > 0)
                        {
                            if (QueryString.Length > 0)
                                QueryString += " AND ";
                            QueryString += UserControlQueryList.TableAliases[this._QueryCondition.Table] + "." + this._QueryCondition.Restriction;
                        }
                    }
                    else
                        QueryString = "T." + Column + QueryString;
                }
            }
            catch (System.Exception ex)
            {
            }
            return QueryString;
        }

        private void buttonViewItems_Click(object sender, EventArgs e)
        {
            string Message = "";
            if (this._Values == null || this._Values.Count == 0)
            {
                Message = "Nothing selected";
                System.Windows.Forms.MessageBox.Show(Message, this.buttonBaseURL.Text);
            }
            else
            {
                string Title = "";
                switch (this._QueryCondition.iWorkbenchUnit.ServiceName())
                {
                    case "DiversityTaxonNames":
                        Title = "Selected taxa";
                        break;
                    case "DiversityScientificTerms":
                        Title = "Selected terms";
                        break;
                    case "DiversitySamplingPlots":
                        Title = "Selected plots";
                        break;
                    case "DiversityAgents":
                        Title = "Selected agents";
                        break;
                    default:
                        break;
                }
                string Header = "";
                if (this._QueryCondition.Value != null && this._QueryCondition.Value.Length > 0)
                {
                    Header = this._QueryCondition.Value + ": ";
                }
                else
                    Header = this.buttonQueryConditionOperator.Text + " = ";
                switch (this.buttonQueryConditionOperator.Text)
                {
                    case "∈":
                        Header += DiversityWorkbench.UserControls.UserControlQueryConditionText.within_the_range_of_the_given_values;
                        break;
                    case "∉":
                        Header += DiversityWorkbench.UserControls.UserControlQueryConditionText.not_within_the_range_of_the_given_values;
                        break;
                    case "+H":
                        Header += DiversityWorkbench.UserControls.UserControlQueryConditionText.including_lower_hierarchy;
                        break;
                    case "+S":
                        Header += DiversityWorkbench.UserControls.UserControlQueryConditionText.including_synonyms;
                        break;
                    case "+HS":
                        Header += DiversityWorkbench.UserControls.UserControlQueryConditionText.including_lower_hierarchy_and_synonyms;
                        break;
                    case "∈+S":
                        Header += DiversityWorkbench.UserControls.UserControlQueryConditionText.within_the_range_of_the_given_values + " + "
                            + DiversityWorkbench.UserControls.UserControlQueryConditionText.including_synonyms;
                        break;
                    default:
                        Header += DiversityWorkbench.UserControls.UserControlQueryConditionText.within_the_range_of_the_given_values;
                        break;
                }
                DiversityWorkbench.Forms.FormTableContent f = new Forms.FormTableContent(Title, Header, this.DtSelectedItems());
                if (this.buttonQueryConditionOperator.Text.Length > 1)
                    f.setIcon(this.pictureBoxQueryOperator.Image);
                f.RowHeaderVisible(false);
                f.setDataGridColumnForeColor(1, System.Drawing.Color.Blue);
                System.Drawing.Font F = new Font("Arial", (float)8.25, FontStyle.Underline);
                f.setDataGridColumnFont(1, F);
                if (this._Values.Count < 20)
                {
                    f.Height = DiversityWorkbench.Forms.FormFunctions.DiplayZoomCorrected(94 + this._Values.Count * 22);
                }
                f.ShowDialog();
            }
        }

        private System.Data.DataTable DtSelectedItems()
        {
            System.Data.DataTable DT = new DataTable();
            System.Data.DataColumn C = new DataColumn();
            switch (this._QueryCondition.iWorkbenchUnit.ServiceName())
            {
                case "DiversityTaxonNames":
                    C = new DataColumn("Taxon", typeof(string));
                    break;
                case "DiversityScientificTerms":
                    C = new DataColumn("Term", typeof(string));
                    break;
                case "DiversitySamplingPlots":
                    C = new DataColumn("Plot", typeof(string));
                    break;
                case "DiversityAgents":
                    C = new DataColumn("Agent", typeof(string));
                    break;
                default:
                    break;
            }
            DT.Columns.Add(C);
            string ColumnName = "URI";
            if (this._QueryCondition.iWorkbenchUnit.getServerConnection().ModuleName == DiversityWorkbench.Settings.ModuleName)
            {
                switch (this._QueryCondition.iWorkbenchUnit.ServiceName())
                {
                    case "DiversityTaxonNames":
                        ColumnName = "NameID";
                        break;
                    case "DiversityScientificTerms":
                        ColumnName = "ID";
                        break;
                    case "DiversitySamplingPlots":
                        ColumnName = "ID";
                        break;
                    case "DiversityAgents":
                        ColumnName = "AgentID";
                        break;
                    default:
                        ColumnName = "ID";
                        break;
                }
            }
            System.Data.DataColumn U = new DataColumn(ColumnName, typeof(string));
            DT.Columns.Add(U);
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Values)
            {
                System.Data.DataRow R = DT.NewRow();
                R[0] = KV.Value;
                R[1] = KV.Key;
                DT.Rows.Add(R);
            }
            return DT;
        }

        private void buttonModuleConnection_Click(object sender, EventArgs e)
        {
            if (this._QueryCondition.Operator.Trim().Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select the type of comparision");
                return;
            }
            DiversityWorkbench.Forms.FormRemoteQuery f;

            if (this.Domain.Length > 0)
            {
                f = new DiversityWorkbench.Forms.FormRemoteQuery(this._QueryCondition.iWorkbenchUnit, this.Domain, true);
                try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
                catch { }
            }
            else if (this._QueryCondition.iWorkbenchUnit.getServerConnection().ModuleName == DiversityWorkbench.Settings.ModuleName)
            {
                f = new DiversityWorkbench.Forms.FormRemoteQuery(this._QueryCondition.iWorkbenchUnit, true, true);
                try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
                catch { }
            }
            else
            {
                f = new DiversityWorkbench.Forms.FormRemoteQuery(this._QueryCondition.iWorkbenchUnit, false, true);
                try { f.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace; }
                catch { }
            }
            this.Cursor = System.Windows.Forms.Cursors.Default;
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                //this.buttonBaseURL.Text = f.ServerConnection.BaseURL;
                this.BaseURL = f.ServerConnection.BaseURL;
                if (this._QueryCondition.Operator.Length > 1)
                {
                    this._Values.Clear();
                    this._StartValue = f.SelectedItem();
                    if (this._StartValue.Count > 0)
                    {
                        string _StartUri = this._StartValue.First().Key;
                        this._QueryCondition.Value = this._StartValue.First().Value;
                        switch (this._QueryCondition.iWorkbenchUnit.ServiceName())
                        {
                            case "DiversityTaxonNames":
                                switch (this._QueryCondition.Operator)
                                {
                                    case "+H":
                                        this._Values = DiversityWorkbench.TaxonName.SubTaxa(this._StartValue.First().Key);
                                        break;
                                    case "+S":
                                        this._Values = DiversityWorkbench.TaxonName.Synonyms(this._StartValue.First().Key);
                                        break;
                                    case "+HS":
                                        this._Values = DiversityWorkbench.TaxonName.SubTaxaSynonyms(this._StartValue.First().Key);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "DiversityScientificTerms":
                                switch (this._QueryCondition.Operator)
                                {
                                    case "+S":
                                        this._Values = DiversityWorkbench.ScientificTerm.Synonyms(this._StartValue.First().Key);
                                        break;
                                    case "+HS":
                                        this._Values = DiversityWorkbench.ScientificTerm.SubTermSynonyms(this._StartValue.First().Key);
                                        break;
                                    case "∈+S":
                                        System.Collections.Generic.List<string> L = new List<string>();
                                        foreach (System.Collections.Generic.KeyValuePair<string, string> KV in f.SelectedItems())
                                            L.Add(KV.Key);
                                        this._Values = DiversityWorkbench.ScientificTerm.SynonymsOfList(L);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "DiversitySamplingPlots":
                                switch (this._QueryCondition.Operator)
                                {
                                    case "+H":
                                        this._Values = DiversityWorkbench.SamplingPlot.SubPlots(this._StartValue.First().Key);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "DiversityAgents":
                                switch (this._QueryCondition.Operator)
                                {
                                    case "+H":
                                        this._Values = DiversityWorkbench.Agent.SubAgent(this._StartValue.First().Key);
                                        break;
                                    case "+S":
                                        this._Values = DiversityWorkbench.Agent.Synonyms(this._StartValue.First().Key);
                                        break;
                                    case "+HS":
                                        this._Values = DiversityWorkbench.Agent.SubAgentSynonyms(this._StartValue.First().Key);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                        }
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Not supported so far");
                    }
                }
                else
                {
                    this._Values = f.SelectedItems();
                    this._StartValue.Clear();
                    this._QueryCondition.Value = "";
                }
                if (this._QueryCondition.iWorkbenchUnit.getServerConnection().ModuleName == DiversityWorkbench.Settings.ModuleName)
                {
                    System.Collections.Generic.Dictionary<string, string> IDs = new Dictionary<string, string>();
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._Values)
                    {
                        string ID = KV.Key.Substring(DiversityWorkbench.Settings.ServerConnection.BaseURL.Length);
                        if (!IDs.ContainsKey(ID))
                            IDs.Add(ID, KV.Value);
                    }
                    this._Values = IDs;
                }
                this.buttonModuleConnection.BackColor = System.Drawing.SystemColors.Control;
                this.setControls();
            }
        }

        private void comboBoxQueryConditionOperator_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (this.comboBoxQueryConditionOperator.SelectedItem.ToString().Length > 1)
            {
                this._StartValue.Clear();
                this._Values.Clear();
                this._QueryCondition.Value = "";
            }
            if (this.comboBoxQueryConditionOperator.SelectedItem.ToString().Length > 1 ||
                this._Values.Count == 0 &&
                this.comboBoxQueryConditionOperator.SelectedItem.ToString().Trim().Length > 0)
                this.buttonModuleConnection.BackColor = System.Drawing.Color.Red;
            else
                this.buttonModuleConnection.BackColor = System.Drawing.Color.Transparent;
        }

        private void comboBoxQueryConditionOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonQueryConditionOperator.Text = this.comboBoxQueryConditionOperator.SelectedItem.ToString();
            if (this.buttonQueryConditionOperator.Text.Length > 1)
            {
                this._QueryCondition.Operator = this.buttonQueryConditionOperator.Text;
                switch (this.buttonQueryConditionOperator.Text)
                {
                    case "+H":
                        this.pictureBoxQueryOperator.Image = this.imageListOperator.Images[0];
                        break;
                    case "+S":
                        this.pictureBoxQueryOperator.Image = this.imageListOperator.Images[1];
                        break;
                    case "+HS":
                        this.pictureBoxQueryOperator.Image = this.imageListOperator.Images[2];
                        break;
                    case "∈+S":
                        this.pictureBoxQueryOperator.Image = this.imageListOperator.Images[3];
                        break;
                    default:
                        break;
                }
                this.buttonQueryConditionOperator.Visible = false;
                this.pictureBoxQueryOperator.Visible = true;
                //this.buttonModuleConnection_Click(null, null);
            }
            else
            {
                this.buttonQueryConditionOperator.Visible = true;
                this.pictureBoxQueryOperator.Visible = false;
            }
            this.setControls();
        }

        private void buttonBaseURL_Click(object sender, EventArgs e)
        {
            string Message = "Link to " + this._QueryCondition.iWorkbenchUnit.getServerConnection().ModuleName + " database:\r\n" + this.buttonBaseURL.Text;
            System.Windows.Forms.MessageBox.Show(Message);
        }

        private void pictureBoxTextOrUri_Click(object sender, EventArgs e)
        {
            this.switchFilter();
        }

        private void labelTextOrUri_Click(object sender, EventArgs e)
        {
            this.switchFilter();
        }

        private void switchFilter()
        {
            this._QueryCondition.IsBoolean = !this._QueryCondition.IsBoolean;
            if (this._QueryCondition.IsBoolean && this._QueryCondition.UpperValue != null && this._QueryCondition.UpperValue.Length > 0)
            {
                switch (this._QueryCondition.iWorkbenchUnit.ServiceName())
                {
                    case "DiversityTaxonNames":
                        this.labelTextOrUri.Text = "Tax. names";
                        break;
                    case "DiversityScientificTerms":
                        this.labelTextOrUri.Text = "Sci. terms";
                        break;
                    case "DiversitySamplingPlots":
                        this.labelTextOrUri.Text = "Plot identifiers";
                        break;
                    case "DiversityAgents":
                        this.labelTextOrUri.Text = "Agent names";
                        break;
                    default:
                        break;
                }
                this.labelTextOrUri.ForeColor = System.Drawing.Color.Black;
                this.toolTipQueryCondition.SetToolTip(this.labelTextOrUri, "Query for text values. Click to change to query for URL");
                System.Drawing.Font F = new Font("Microsoft Sans Serif", 8, FontStyle.Regular);
                this.labelTextOrUri.Font = F;
            }
            else
            {
                string Test = this._QueryCondition.iWorkbenchUnit.ServiceName();
                if (this._QueryCondition.iWorkbenchUnit.getServerConnection().ModuleName == DiversityWorkbench.Settings.ModuleName)
                    this.labelTextOrUri.Text = "ID";
                else
                {
                    switch (this._QueryCondition.iWorkbenchUnit.ServiceName())
                    {
                        case "DiversityTaxonNames":
                            this.labelTextOrUri.Text = "Links to taxa";
                            break;
                        case "DiversityScientificTerms":
                            this.labelTextOrUri.Text = "Links to terms";
                            break;
                        case "DiversitySamplingPlots":
                            this.labelTextOrUri.Text = "Links to plots";
                            break;
                        case "DiversityAgents":
                            this.labelTextOrUri.Text = "Links to agents";
                            break;
                        default:
                            break;
                    }
                }
                if (this._QueryCondition.UpperValue == null || (this._QueryCondition.UpperValue != null && this._QueryCondition.UpperValue.Length == 0))
                    this.toolTipQueryCondition.SetToolTip(this.labelTextOrUri, this.labelTextOrUri.Text);
                else
                    this.toolTipQueryCondition.SetToolTip(this.labelTextOrUri, "Query for URL. Click to change to query for text values");
                this.labelTextOrUri.ForeColor = System.Drawing.Color.Blue;
                System.Drawing.Font F = new Font("Microsoft Sans Serif", 8, FontStyle.Underline);
                this.labelTextOrUri.Font = F;
            }
        }

        //#region Hierarchy

        //private bool _IncludeHierarchy = false;

        //public void setHierarchyInclusion(bool IncludeHierarchy)
        //{
        //    this._IncludeHierarchy = IncludeHierarchy;
        //    this.buttonIncludeHierarchy.Visible = true;
        //    if (this._IncludeHierarchy)
        //    {
        //        this.toolTipQueryCondition.SetToolTip(this.buttonIncludeHierarchy, "Include all depending items");
        //        this.buttonIncludeHierarchy.BackColor = System.Drawing.Color.White;
        //        this.buttonIncludeHierarchy.FlatAppearance.BorderColor = System.Drawing.Color.Red;
        //        this.buttonIncludeHierarchy.FlatAppearance.BorderSize = 1;
        //    }
        //    else
        //    {
        //        this.buttonIncludeHierarchy.BackColor = System.Drawing.SystemColors.Control;
        //        this.buttonIncludeHierarchy.FlatAppearance.BorderSize = 0;
        //        this.toolTipQueryCondition.SetToolTip(this.buttonIncludeHierarchy, "Include only selected items. Do not add depending items");
        //    }
        //}

        //private void buttonIncludeHierarchy_Click(object sender, EventArgs e)
        //{
        //    this.setHierarchyInclusion(!this._IncludeHierarchy);
        //}

        //#endregion

    }
}
