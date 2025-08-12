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
    public partial class UserControlQueryConditionReferencingTable : UserControl, DiversityWorkbench.IUserControlQueryCondition
    {

        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryCondition;

        private UserControlQueryList _UserControlQueryList;
        public void setUserControlQueryList(UserControlQueryList List)
        {
            this._UserControlQueryList = List;
        }

        #endregion

        #region Construction

        public UserControlQueryConditionReferencingTable()
        {
            InitializeComponent();
            DiversityWorkbench.Forms.FormFunctions.addEditOnDoubleClickToTextboxes(this.textBoxQueryCondition);
        }

        public UserControlQueryConditionReferencingTable(DiversityWorkbench.QueryCondition Q, string ConnectionString)
            : this()
        {
            this._QueryCondition = Q;
            this.initControls();

            this.setOperatorTooltip();

            this.AccessibleName = this._QueryCondition.Table + "." + this._QueryCondition.Column;
            this._QueryCondition.QueryType = QueryCondition.QueryTypes.ReferencingTable;
        }

        #endregion

        #region Control

        private void initControls()
        {
            this.comboBoxQueryConditionOperator.Items.Clear();
            this.comboBoxQueryConditionOperator.Items.Add("~");
            this.comboBoxQueryConditionOperator.Items.Add("=");
            this.comboBoxQueryConditionOperator.Items.Add("•");
            this.comboBoxQueryConditionOperator.Items.Add("Ø");

            //this.comboBoxType.SelectedIndex = 1; // bringt so nix und macht auch keinen Sinn
            if (this._QueryCondition.DisplayText.Length > 0)
            {
                this.setButtonConditionText(this._QueryCondition.DisplayText);
            }
            //if (this._QueryCondition.QueryType == QueryCondition.QueryTypes.ReferencingTable)
            //{
            //    this.comboBoxReferencedTable.DataSource = this._QueryCondition.ReferencingTable.DtReferencedTables;
            //}
            //else

            // In case that there is only one table, this can be selected by default
            if (this._QueryCondition.ReferencingTable.DtReferencedTables.Rows.Count == 2 &&
                this._QueryCondition.ReferencingTable.DtReferencedTables.Rows[0][0].ToString().Trim().Length == 0)
            {
                this._QueryCondition.ReferencingTable.DtReferencedTables.Rows[0].Delete();
                this._QueryCondition.ReferencingTable.DtReferencedTables.AcceptChanges();
            }
            this.comboBoxReferencedTable.DataSource = this._QueryCondition.ReferencingTable.DtReferencedTables;
            this.comboBoxReferencedTable.ValueMember = "Table";
            this.comboBoxReferencedTable.DisplayMember = "DisplayText";
            //if (this._QueryCondition.ReferencingTable.DtReferencedTables.Rows.Count == 2 && 
            //    this._QueryCondition.ReferencingTable.DtReferencedTables.Rows[0][0].ToString().Trim().Length == 0)
            //    this.comboBoxReferencedTable.SelectedIndex = 1;

            this.comboBoxType.Items.Clear();
            foreach (string T in this._QueryCondition.ReferencingTable.TypeList())
                this.comboBoxType.Items.Add(T);
            this.comboBoxType.SelectedIndex = 0;
        }

        #endregion

        #region Formatting and setting of the controls

        private void setOperatorTooltip()
        {
            string Message = DiversityWorkbench.UserControls.UserControlQueryConditionText.search_for_entries.Substring(0, 1).ToUpper()
                        + DiversityWorkbench.UserControls.UserControlQueryConditionText.search_for_entries.Substring(1) + " ";
            switch (this.buttonQueryConditionOperator.Text)
            {
                case "=":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.exactly_like_the_given_value;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                case "~":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.similar_to_the_given_value + "; "
                        + DiversityWorkbench.UserControls.UserControlQueryConditionText.use_wildcard;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                case "Ø":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.where_a_value_is_missing;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                case "•":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.where_a_value_is_present;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                default:
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.Operator_for_comparision_between_the_given_value_and_the_values_in_the_database;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
            }
        }

        private void setButtonConditionText(string Text)
        {
            int ButtonWidth = this.buttonCondition.Width;
            string DisplayText = Text;
            Graphics g = Graphics.FromHwnd(this.Handle);
            int StringWidth = (int)g.MeasureString(Text, this.buttonCondition.Font).Width;
            while (StringWidth > ButtonWidth - 8)
            {
                DisplayText = DisplayText.Trim();
                DisplayText = DisplayText.Substring(0, DisplayText.Length - 2) + ".";
                if (DisplayText.EndsWith(".."))
                    DisplayText = DisplayText.Substring(0, DisplayText.Length - 1);
                if (DisplayText.EndsWith(". ."))
                    DisplayText = DisplayText.Substring(0, DisplayText.Length - 2);
                StringWidth = (int)g.MeasureString(DisplayText, this.buttonCondition.Font).Width;
            }
            this.buttonCondition.Text = DisplayText;
        }

        private void comboBoxQueryConditionOperator_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.buttonQueryConditionOperator.Text = this.comboBoxQueryConditionOperator.SelectedItem.ToString();
        }

        private void buttonQueryConditionOperator_TextChanged(object sender, EventArgs e)
        {
            this._QueryCondition.Operator = this.buttonQueryConditionOperator.Text;

            this.setOperatorTooltip();

            if (this.buttonQueryConditionOperator.Text == "•" || this.buttonQueryConditionOperator.Text == "Ø")
            {
                if (this.buttonQueryConditionOperator.Text == "•")
                    this.labelNull.Text = DiversityWorkbench.UserControls.UserControlQueryConditionText.present;// this.Message("present");
                else if (this.buttonQueryConditionOperator.Text == "Ø")
                    this.labelNull.Text = DiversityWorkbench.UserControls.UserControlQueryConditionText.missing;
                this.textBoxQueryCondition.Dock = DockStyle.Left;
                this.textBoxQueryCondition.Visible = false;

                this.labelNull.Visible = true;
                this.labelNull.Dock = DockStyle.Fill;
            }
            else
            {
                this.labelNull.Dock = DockStyle.Right;
                this.labelNull.Visible = false;
                this.textBoxQueryCondition.Dock = DockStyle.Fill;
                this.textBoxQueryCondition.Visible = true;
            }
        }

        #endregion

        #region public Properties and functions

        public string ReferencedTable
        {
            get { return this.comboBoxReferencedTable.SelectedValue.ToString(); }
        }

        public string ReferencedColumn
        {
            get { return this._QueryCondition.Column; }
        }

        public DiversityWorkbench.QueryCondition Condition()
        {
            return this._QueryCondition;
        }

        public DiversityWorkbench.QueryCondition getCondition()
        {
            DiversityWorkbench.QueryCondition QC = new QueryCondition();
            QC = this._QueryCondition;
            QC.QueryConditionOperator = this.buttonQueryConditionOperator.Text;
            return QC;
        }

        public void setConditionValues(DiversityWorkbench.QueryCondition Condition)
        {
            this.buttonQueryConditionOperator.Text = Condition.QueryConditionOperator;
            if (Condition.DisplayText != null && Condition.DisplayText.Length > 0)
                System.Windows.Forms.MessageBox.Show("Please check restriction for " + Condition.DisplayText);
        }

        public string ConditionValue()
        {
            string Value = "";
            Value = this.textBoxQueryCondition.Text;
            return Value;
        }

        public string WhereClause()
        {
            string QueryString = "";
            if (UserControlQueryList.UseOptimizing)
            {
                try
                {
                    if (this.textBoxQueryCondition.Text.Length == 0 && this.buttonQueryConditionOperator.Text != "•")
                        return "";
                    string Operator = this.buttonQueryConditionOperator.Text;
                    string ReferencedTable = "";
                    if (this.comboBoxReferencedTable.SelectedValue != null)
                        ReferencedTable = this.comboBoxReferencedTable.SelectedValue.ToString();
                    string Type = "";
                    if (this.comboBoxType.SelectedItem != null)
                        Type = this.comboBoxType.SelectedItem.ToString();
                    QueryString = this._QueryCondition.ReferencingTable.WhereClause(this._QueryCondition.ReferencingTable.TableName(), ReferencedTable, this._QueryCondition.Column, Operator, this.textBoxQueryCondition.Text, Type);
                }
                catch { }
                return QueryString;
            }
            else
                return "";

            if (this.buttonQueryConditionOperator.Text == " ")
            {
                return "";
            }
            QueryString = "";
            if (this.buttonQueryConditionOperator.Text == "~" && this.textBoxQueryCondition.Text.Length > 0)
            {
                QueryString += " LIKE '" + this.textBoxQueryCondition.Text + "%'";
            }
            if (this.buttonQueryConditionOperator.Text == "Ø")
            {
                QueryString += " IS NULL ";
            }
            else if (this.buttonQueryConditionOperator.Text == "•")
            {
                QueryString += " IS NOT NULL ";
            }
            if (QueryString.Length > 0)
                QueryString = " A." + this._QueryCondition.Column + QueryString;

            return QueryString;
        }

        public string SQL()
        {
            string QueryString = "";
            try
            {
                if (this.textBoxQueryCondition.Text.Length == 0 && this.buttonQueryConditionOperator.Text != "•" && this.buttonQueryConditionOperator.Text != "Ø")
                    return "";
                string Operator = this.buttonQueryConditionOperator.Text;
                string ReferencedTable = "";
                if (this.comboBoxReferencedTable.SelectedValue != null)
                    ReferencedTable = this.comboBoxReferencedTable.SelectedValue.ToString();
                string Type = "";
                if (this.comboBoxType.SelectedItem != null)
                    Type = this.comboBoxType.SelectedItem.ToString();
                QueryString = this._QueryCondition.ReferencingTable.WhereClause(this._QueryCondition.ReferencingTable.TableName(), ReferencedTable, this._QueryCondition.Column, Operator, this.textBoxQueryCondition.Text, Type);
                if (this.buttonQueryConditionOperator.Text == "Ø" && DiversityWorkbench.Settings.ModuleName == "DiversityCollection")
                {
                    QueryString = " select N.CollectionSpecimenID from CollectionSpecimen_Core2 N where N.CollectionSpecimenID not in(" + QueryString + ") ";
                }
            }
            catch { }
            return QueryString;
        }

        public string SqlByIndex(int FieldSequence)
        {
            string QueryString = "";
            if (FieldSequence < this._QueryCondition.QueryFields.Count)
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

        public void Clear()
        {
            this.textBoxQueryCondition.Text = "";
            this.comboBoxReferencedTable.SelectedIndex = 0;
            this.comboBoxType.SelectedIndex = 0;
            this.comboBoxQueryConditionOperator.SelectedIndex = 0;
        }

        public void setEntity()
        {
            if (!DiversityWorkbench.Entity.EntityTablesExist) return;
            if (this._QueryCondition.TextFixed)
            {
                this.toolTipQueryCondition.SetToolTip(this.textBoxQueryCondition, this._QueryCondition.Description);
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
            if (Description.Length > 0)
            {
                this.toolTipQueryCondition.SetToolTip(this.textBoxQueryCondition, Description);
                this.toolTipQueryCondition.SetToolTip(this.buttonCondition, Description);
            }
        }

        public void RefreshProjectDependentHierarchy() { }

        #endregion

    }
}
