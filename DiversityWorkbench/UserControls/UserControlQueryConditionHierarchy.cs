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
    public partial class UserControlQueryConditionHierarchy : UserControl, DiversityWorkbench.IUserControlQueryCondition
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

        public UserControlQueryConditionHierarchy()
        {
            InitializeComponent();
        }

        public UserControlQueryConditionHierarchy(DiversityWorkbench.QueryCondition Q, string ConnectionString)
            : this()
        {
            this._QueryCondition = Q;
            this.initControls();

            this.setQueryConditionOperators();

            this.setQueryConditionOperator();

            this.setQueryConditionControlsVisibility();

            this.setOperatorTooltip();

            this.AccessibleName = this._QueryCondition.Table + "." + this._QueryCondition.Column;
        }

        #endregion

        #region Formatting and setting of the controls

        private void setQueryConditionControlsVisibility()
        {
            // textBoxQueryCondition
            // comboBoxQueryCondition
            if (this.buttonQueryConditionOperator.Text == "∆")
            {
                this.toolStrip.Visible = true;
                //this.comboBoxQueryCondition.Visible = true;
            }
            else
            {
                this.toolStrip.Visible = false;
                this.comboBoxQueryCondition.Visible = false;
                //this.toolTipQueryCondition.SetToolTip(this.textBoxQueryCondition, this._QueryCondition.Description);
            }



            // labelNull
            if (this.buttonQueryConditionOperator.Text == "Ø"
                || this.buttonQueryConditionOperator.Text == "•")
            {
                this.labelNull.Visible = true;
                this.labelNull.AutoSize = true;
                this.labelNull.Dock = DockStyle.Right;
            }
            else
            {
                this.labelNull.Dock = DockStyle.Left;
                this.labelNull.Visible = false;
            }

        }

        private void setQueryConditionOperators()
        {
            this.comboBoxQueryConditionOperator.Items.Clear();

            this.comboBoxQueryConditionOperator.Items.Add(" ");
            this.comboBoxQueryConditionOperator.Items.Add("∆");
            this.comboBoxQueryConditionOperator.Items.Add("Ø");
            this.comboBoxQueryConditionOperator.Items.Add("•");
        }

        private void setQueryConditionOperator()
        {
            this._QueryCondition.Operator = this.buttonQueryConditionOperator.Text;

        }

        private void initControls()
        {
            if (this._QueryCondition.DisplayText.Length > 0)
            {
                this.setButtonConditionText(this._QueryCondition.DisplayText);
                //this.buttonCondition.Text = this._QueryCondition.Display(this.buttonCondition, Entity.EntityInformationField.Abbreviation);
            }
            //this.toolTipQueryCondition.SetToolTip(this.comboBoxQueryCondition, DiversityWorkbench.UserControls.UserControlQueryConditionText.maximal_distance_to);
        }

        private void setOperatorTooltip()
        {
            string Message = DiversityWorkbench.UserControls.UserControlQueryConditionText.search_for_entries.Substring(0, 1).ToUpper()
                        + DiversityWorkbench.UserControls.UserControlQueryConditionText.search_for_entries.Substring(1) + " ";
            switch (this.buttonQueryConditionOperator.Text)
            {
                case "Ø":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.where_a_value_is_missing;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                case "•":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.where_a_value_is_present;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                case "∆":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.containing_the_given_value_and_all_subsidiary_values;
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

            this.setQueryConditionControlsVisibility();

            if (this.buttonQueryConditionOperator.Text == "•")
            {
                if (this._QueryCondition.IsDate)
                    this.labelNull.Text = DiversityWorkbench.UserControls.UserControlQueryConditionText.complete;// this.Message("complete");
                else
                    this.labelNull.Text = DiversityWorkbench.UserControls.UserControlQueryConditionText.present;// this.Message("present");
            }
            else if (this.buttonQueryConditionOperator.Text == "Ø")
                this.labelNull.Text = DiversityWorkbench.UserControls.UserControlQueryConditionText.missing;// this.Message("missing");
            else if (this.buttonQueryConditionOperator.Text == " ")
                this.labelNull.Text = "";
        }

        #endregion

        #region public Properties and functions

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
            System.Windows.Forms.MessageBox.Show("Please check restriction for " + Condition.DisplayText + " in " + Condition.QueryGroup);
            //if (Condition.SelectFromList || Condition.SelectFromHierachy)
            //    this.comboBoxQueryCondition.SelectedIndex = Condition.SelectedIndex;
        }

        public string ConditionValue()
        {
            string Value = "";
            Value = this.comboBoxQueryCondition.Text;
            if (this.toolStrip.Tag != null)
            {
                if (!this._QueryCondition.IsNumeric && !this._QueryCondition.IsBoolean && !this._QueryCondition.IsDate && !this._QueryCondition.IsYear)
                    Value += "'";
                System.Data.DataRow R = (System.Data.DataRow)this.toolStrip.Tag;
                Value += R[this._QueryCondition.HierarchyColumn].ToString();
                if (!this._QueryCondition.IsNumeric && !this._QueryCondition.IsBoolean && !this._QueryCondition.IsDate && !this._QueryCondition.IsYear)
                    Value += "'";
            }
            return Value;
        }

        public string WhereClause()
        {

            ///TODO: Putzen - hier ist noch einiges drin, was fuer die Hierarchie nicht zutrifft


            if (this.buttonQueryConditionOperator.Text == " ")
            {
                return "";
            }
            string QueryString = "";
            if (this.buttonQueryConditionOperator.Text == "Ø")
            {
                QueryString += " IS NULL ";
            }
            else if (this.buttonQueryConditionOperator.Text == "•")
            {
                QueryString += " IS NOT NULL ";
            }

            if (QueryString.Length > 0)
            {
                if (this.buttonQueryConditionOperator.Text == "∆")
                    QueryString = "T.[" + this._QueryCondition.Column + "]" + QueryString;
                else
                    QueryString = "T.[" + this._QueryCondition.HierarchyParentColumn + "]" + QueryString;
                //if (this.buttonQueryConditionOperator.Text == "∆")
                //    QueryString = "[" + this._QueryCondition.Table + "].[" + this._QueryCondition.Column + "]" + QueryString;
                //else
                //    QueryString = "[" + this._QueryCondition.Table + "].[" + this._QueryCondition.HierarchyParentColumn + "]" + QueryString;
            }

            if (this.toolStrip.Tag != null && this.buttonQueryConditionOperator.Text == "∆")
            {
                if (this._QueryCondition.IsNumeric && !this._QueryCondition.IsBoolean && !this._QueryCondition.IsDate && !this._QueryCondition.IsYear)
                {
                    if (this.buttonQueryConditionOperator.Text == "Ø")
                    {
                        QueryString += " IS NULL ";
                    }
                    else if (this.buttonQueryConditionOperator.Text == "•")
                        QueryString += " <> '' ";
                    //else if (this.buttonQueryConditionOperator.Text == "≠")
                    //{
                    //    QueryString += " <> ";
                    //}
                    else if (this.buttonQueryConditionOperator.Text == "∆")
                    {
                        QueryString += " IN ( ";
                    }
                    //else if (this.buttonQueryConditionOperator.Text == "~")
                    //{
                    //    QueryString += " LIKE ";
                    //}
                    else
                        QueryString += " " + this.buttonQueryConditionOperator.Text + " ";
                }
                else
                    QueryString += " = ";
                if (!this._QueryCondition.IsNumeric
                    && !this._QueryCondition.IsBoolean
                    && !this._QueryCondition.IsDate
                    && !this._QueryCondition.IsYear
                    && this.buttonQueryConditionOperator.Text != "Ø"
                    && this.buttonQueryConditionOperator.Text != "•")
                    QueryString += "'";
                if ((this.buttonQueryConditionOperator.Text != "Ø"
                    && this.buttonQueryConditionOperator.Text != "•")
                    || !this.buttonQueryConditionOperator.Visible)
                {
                    System.Data.DataRow R = (System.Data.DataRow)this.toolStrip.Tag;
                    if (R[this._QueryCondition.HierarchyColumn].ToString().Length == 0) QueryString = "";
                    else
                    {
                        QueryString += R[this._QueryCondition.HierarchyColumn].ToString();
                        if (this.buttonQueryConditionOperator.Text == "∆")
                        {
                            if (!this._QueryCondition.IsNumeric
                                && !this._QueryCondition.IsBoolean
                                && !this._QueryCondition.IsDate
                                && !this._QueryCondition.IsYear
                                && this.buttonQueryConditionOperator.Text != "Ø"
                                && this.buttonQueryConditionOperator.Text != "•")
                                QueryString += "'";
                            QueryString += this.getHierarchyChildValueList(R[this._QueryCondition.HierarchyColumn].ToString()) + ")";
                        }
                    }
                }
                if (!this._QueryCondition.IsNumeric
                    && !this._QueryCondition.IsBoolean
                    && !this._QueryCondition.IsDate
                    && !this._QueryCondition.IsYear
                    && this.buttonQueryConditionOperator.Text != "Ø"
                    && this.buttonQueryConditionOperator.Text != "∆"
                    && this.buttonQueryConditionOperator.Text != "•")
                {
                    if (QueryString.Length > 0)
                    {
                        if (this.buttonQueryConditionOperator.Text == "~")
                            QueryString += "%";
                        QueryString += "'";
                    }
                }
            }
            if (QueryString.Length > 0 && this.buttonQueryConditionOperator.Text == "∆")
            {
                string TableOfColumn = "";
                if (this._QueryCondition.Table.Contains(".dbo."))
                {
                    string[] TT = this._QueryCondition.Table.Split(new char[] { '.' });
                    for (int i = 0; i < TT.Length; i++)
                        TableOfColumn += "[" + TT[i] + "].";
                }
                else
                {
                    TableOfColumn = "T.";
                    //TableOfColumn = "[" + this._QueryCondition.Table + "].";
                }

                if (!this._QueryCondition.IsDate)
                {
                    if (this._QueryCondition.IntermediateTable.Length == 0)
                    {
                        if (this._QueryCondition.Column.StartsWith(this._QueryCondition.Table + "."))
                        {
                            if (QueryString == " IS NULL ")
                                QueryString = "(" + this._QueryCondition.Column + QueryString + " OR " + this._QueryCondition.Column + " = '') ";
                            else
                                QueryString = this._QueryCondition.Column + QueryString;
                        }
                        else
                        {
                            if (QueryString == " IS NULL ")
                            {
                                QueryString = "(" + TableOfColumn + this._QueryCondition.Column + QueryString + " OR " + " T." + this._QueryCondition.Column + " = '') ";
                                //QueryString = "(" + TableOfColumn + this._QueryCondition.Column + QueryString + " OR " + " [" + this._QueryCondition.Table + "]." + this._QueryCondition.Column + " = '') ";
                            }
                            else
                            {
                                if (this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.NoCheck
                                    && this.buttonQueryConditionOperator.Text == "∆")
                                    QueryString = TableOfColumn + "[" + this._QueryCondition.Column + "]" + QueryString;
                                //else if (this._QueryCondition.CheckIfDataExist != QueryCondition.CheckDataExistence.NoCheck
                                //    && this._QueryCondition.ForeingKeyTable.Length > 0)
                                //    QueryString = TableOfColumn + "[" + this._QueryCondition.Column + "]" + QueryString;
                            }
                        }
                    }
                    else
                    {
                        if (this._QueryCondition.Column.StartsWith(this._QueryCondition.Table + "."))
                        {
                            if (QueryString == " IS NULL ")
                                QueryString = "(" + this._QueryCondition.Column + QueryString + " OR " + this._QueryCondition.Column + " = '') ";
                            else
                                QueryString = this._QueryCondition.Column + QueryString;
                        }
                        else
                        {
                            if (QueryString == " IS NULL ")
                            {
                                QueryString = "(" + TableOfColumn + this._QueryCondition.Column + QueryString + " OR " + "T." + this._QueryCondition.Column + " = '') ";
                                //QueryString = "(" + TableOfColumn + this._QueryCondition.Column + QueryString + " OR " + "[" + this._QueryCondition.Table + "]." + this._QueryCondition.Column + " = '') ";
                            }
                            else
                            {
                                QueryString = TableOfColumn + this._QueryCondition.Column + QueryString;
                            }
                        }
                    }
                }
            }
            return QueryString;
        }

        public string SQL()
        {
            string QueryString = "";
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
            this.comboBoxQueryCondition.Text = "";
            //this.toolStrip.Tag = null;
            //if (this.comboBoxQueryCondition.DataSource != null)
            this.comboBoxQueryCondition.SelectedIndex = 0;
        }

        public void setEntity()
        {
            if (!DiversityWorkbench.Entity.EntityTablesExist) return;
            if (this._QueryCondition.TextFixed)
            {
                this.toolTipQueryCondition.SetToolTip(this.comboBoxQueryCondition, this._QueryCondition.Description);
                //this.toolTipQueryCondition.SetToolTip(this.linkLabelValue, this._QueryCondition.Description);
                //this.toolTipQueryCondition.SetToolTip(this.checkBoxQueryCondition, this._QueryCondition.Description);
                //this.toolTipQueryCondition.SetToolTip(this.comboBoxQueryCondition, this._QueryCondition.Description);
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
                this.toolTipQueryCondition.SetToolTip(this.comboBoxQueryCondition, Description);
                //this.toolTipQueryCondition.SetToolTip(this.checkBoxQueryCondition, Description);
                //this.toolTipQueryCondition.SetToolTip(this.comboBoxQueryCondition, Description);
                this.toolTipQueryCondition.SetToolTip(this.buttonCondition, Description);
            }
            if (Abbreviation.Length > 0)
                this.setButtonConditionText(Abbreviation);
            //this.buttonCondition.Text = this._QueryCondition.Display(this.buttonCondition);
            //if (Abbreviation.Length > 0 && this.buttonCondition.Text.Length == 0)
            //    this.setButtonConditionText(Abbreviation);
        }

        #endregion

        #region Hierarchy

        public void buildHierarchy()
        {
            try
            {
                this.toolStripDropDownButton.DropDownItems.Clear();
                this.toolStripDropDownButton.Text = "";
                string Restriction = "";
                // Markus 22.7.2022: nullstellen werden bei Abhängigheit von Projekt nur dadurch gefunden, dass die ParentID nicht in der Liste der Ids auftaucht
                if (this._QueryCondition.DependsOnCurrentProjectID && this._QueryCondition.HierarchyColumn != null && this._QueryCondition.HierarchyColumn.Length > 0)
                {
                    System.Collections.Generic.List<string> IDs = new List<string>();
                    System.Collections.Generic.List<string> ParentIDs = new List<string>();
                    foreach (System.Data.DataRow R in this._QueryCondition.dtValues.Rows)
                    {
                        IDs.Add(R[this._QueryCondition.HierarchyColumn].ToString());
                        ParentIDs.Add(R[this._QueryCondition.HierarchyParentColumn].ToString());
                    }
                    System.Collections.Generic.List<string> TopIDs = new List<string>();
                    foreach (string P in ParentIDs)
                    {
                        if (!IDs.Contains(P))
                            TopIDs.Add(P);
                    }
                    foreach (string T in TopIDs)
                    {
                        if (Restriction.Length > 0) Restriction += ", ";
                        Restriction += T;
                    }
                    if (Restriction.Length > 0)
                        Restriction = this._QueryCondition.HierarchyParentColumn + " IN (" + Restriction + ") OR " + this._QueryCondition.HierarchyParentColumn + " IS NULL";
                    else
                        Restriction = this._QueryCondition.HierarchyParentColumn + " IS NULL";
                }
                else
                {
                    Restriction = this._QueryCondition.HierarchyParentColumn + " IS NULL";
                }
                System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Restriction, this._QueryCondition.OrderColumn);
                if (rr.Length > 1000) //Markus 18.1.2017 dauert fuer zu grosse Datenbestände zu lange. Beschränken auf die Datensaetze die abhaengige Daten haben
                {
                    System.Collections.Generic.List<System.Data.DataRow> RR = new List<DataRow>();
                    for (int i = 0; i < rr.Length; i++)
                    {
                        System.Data.DataRow[] rrCheck = this._QueryCondition.dtValues.Select(this._QueryCondition.HierarchyParentColumn + " = '" + rr[i][this._QueryCondition.HierarchyParentColumn] + "'", this._QueryCondition.OrderColumn);
                        if (rrCheck.Length > 0)
                            RR.Add(rr[i]);
                    }
                    foreach (System.Data.DataRow R in RR)
                    {
                        string Display = R[_QueryCondition.HierarchyDisplayColumn].ToString();
                        System.Windows.Forms.ToolStripMenuItem M = new ToolStripMenuItem(Display, null, this.ToolStripMenuItem_Click);
                        M.Tag = R;
                        this.appendHierarchyChilds(M);
                        this.toolStripDropDownButton.DropDownItems.Add(M);
                    }
                }
                else
                {
                    for (int i = 0; i < rr.Length; i++)
                    {
                        string Display = rr[i][_QueryCondition.HierarchyDisplayColumn].ToString();
                        System.Windows.Forms.ToolStripMenuItem M = new ToolStripMenuItem(Display, null, this.ToolStripMenuItem_Click);
                        M.Tag = rr[i];
                        this.appendHierarchyChilds(M);
                        this.toolStripDropDownButton.DropDownItems.Add(M);
                    }
                }
                this.comboBoxQueryCondition.DataSource = this._QueryCondition.dtValues;
                this.comboBoxQueryCondition.DisplayMember = this._QueryCondition.dtValues.Columns[2].ColumnName;
                this.comboBoxQueryCondition.ValueMember = this._QueryCondition.dtValues.Columns[0].ColumnName;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void appendHierarchyChilds(System.Windows.Forms.ToolStripMenuItem M)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)M.Tag;
                if (R[this._QueryCondition.HierarchyColumn].Equals(System.DBNull.Value)) return;
                string Select = this._QueryCondition.HierarchyParentColumn + " = '" + R[this._QueryCondition.HierarchyColumn].ToString() + "'";
                System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Select, this._QueryCondition.OrderColumn);
                for (int i = 0; i < rr.Length; i++)
                {
                    string Display = rr[i][_QueryCondition.HierarchyDisplayColumn].ToString();
                    System.Windows.Forms.ToolStripMenuItem MChild = new ToolStripMenuItem(Display, null, this.ToolStripMenuItem_Click);
                    MChild.Tag = rr[i];
                    this.appendHierarchyChilds(MChild);
                    M.DropDownItems.Add(MChild);
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.ToolStripMenuItem M = (System.Windows.Forms.ToolStripMenuItem)sender;
            if (M.Tag != null)
            {
                System.Data.DataRow R = (System.Data.DataRow)M.Tag;
                this.toolStripDropDownButton.Text = R[this._QueryCondition.DisplayColumn].ToString();
                this.toolStripDropDownButton.ToolTipText = R[this._QueryCondition.HierarchyDisplayColumn].ToString();
                this.toolStrip.Tag = R;
                string ToolTip = this.HierarchyString(R[this._QueryCondition.HierarchyColumn].ToString());
                if (ToolTip.Length == 0)
                {
                    ToolTip = "Select an item from the hierarchy";
                    this.comboBoxQueryCondition.SelectedIndex = -1;
                }
                else
                {
                    string ValueColumn = this.comboBoxQueryCondition.ValueMember;
                    int i = 0;
                    for (i = 0; i < this._QueryCondition.dtValues.Rows.Count; i++)
                    {
                        if (this._QueryCondition.dtValues.Rows[i][ValueColumn].ToString() == R[this._QueryCondition.HierarchyColumn].ToString())
                            break;
                    }
                    this.comboBoxQueryCondition.SelectedIndex = i;
                }
            }
        }

        private string HierarchyString(string Key)
        {
            if (Key.Length == 0) return "";
            string Hierarchy = "";
            try
            {
                string Select = this._QueryCondition.HierarchyColumn + " = '" + Key + "'";
                System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Select, this._QueryCondition.OrderColumn);
                if (rr.Length > 0)
                {
                    Hierarchy = this.HierarchyString(rr[0][this._QueryCondition.HierarchyParentColumn].ToString());
                    if (Hierarchy.Length > 0) Hierarchy += " - ";
                    Hierarchy += rr[0][this._QueryCondition.HierarchyDisplayColumn].ToString();
                }
            }
            catch (System.Exception ex) { }
            return Hierarchy;
        }

        private string getHierarchyChildValueList(string ParentID)
        {
            string Childs = "";
            try
            {
                System.Collections.Generic.List<string> ChildIDList = new List<string>();
                string Restriction = this._QueryCondition.HierarchyParentColumn + " = ";
                int i;
                if (!int.TryParse(ParentID, out i))
                    Restriction += "'";
                Restriction += ParentID;
                if (!int.TryParse(ParentID, out i))
                    Restriction += "'";
                System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Restriction);
                foreach (System.Data.DataRow R in rr)
                {
                    ChildIDList.Add(R[this._QueryCondition.HierarchyColumn].ToString());
                    this.getHierarchyChildValueList(R[this._QueryCondition.HierarchyColumn].ToString(), ref ChildIDList);
                }
                foreach (string s in ChildIDList)
                {
                    //if (Childs.Length > 0)
                    Childs += ", ";
                    if (!this._QueryCondition.IsNumeric) Childs += "'";
                    Childs += s;
                    if (!this._QueryCondition.IsNumeric) Childs += "'";
                }
            }
            catch (System.Exception ex) { }
            return Childs;
        }

        private void getHierarchyChildValueList(string ParentID, ref System.Collections.Generic.List<string> ChildIDList)
        {
            try
            {
                string Restriction = this._QueryCondition.HierarchyParentColumn + " = ";
                int i;
                if (!int.TryParse(ParentID, out i)) Restriction += "'";
                Restriction += ParentID;
                if (!int.TryParse(ParentID, out i)) Restriction += "'";

                System.Data.DataRow[] rr = this._QueryCondition.dtValues.Select(Restriction);
                foreach (System.Data.DataRow R in rr)
                {
                    ChildIDList.Add(R[this._QueryCondition.HierarchyColumn].ToString());
                    this.getHierarchyChildValueList(R[this._QueryCondition.HierarchyColumn].ToString(), ref ChildIDList);
                }
            }
            catch (System.Exception ex) { }
        }

        public void RefreshProjectDependentHierarchy() { }

        #endregion

    }
}
