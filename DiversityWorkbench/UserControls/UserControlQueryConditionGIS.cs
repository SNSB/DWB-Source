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
    public partial class UserControlQueryConditionGIS : UserControl, DiversityWorkbench.IUserControlQueryCondition
    {
        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryCondition;
        private enum _Unit { m, km, miles, feet };
        private System.Collections.Generic.Dictionary<_Unit, double> _UnitFactors;
        private string _Geography = "";

        private UserControlQueryList _UserControlQueryList;
        public void setUserControlQueryList(UserControlQueryList List)
        {
            this._UserControlQueryList = List;
        }
        #endregion

        #region Construction

        public UserControlQueryConditionGIS()
        {
            InitializeComponent();
        }

        public UserControlQueryConditionGIS(DiversityWorkbench.QueryCondition Q, string ConnectionString)
            : this()
        {
            this._QueryCondition = Q;
            this.initControls();

            this.setQueryConditionOperators();

            this.setQueryConditionOperator();

            this.setQueryConditionControlsVisibility();

            this.setOperatorTooltip();

            //this.setAutoCompletion(ConnectionString);

            this.AccessibleName = this._QueryCondition.Table + "." + this._QueryCondition.Column;
            this._QueryCondition.QueryType = QueryCondition.QueryTypes.Geography;
        }

        #endregion

        #region Formatting and setting of the controls

        private void setQueryConditionControlsVisibility()
        {
            // textBoxQueryCondition
            // comboBoxQueryCondition
            if (this.buttonQueryConditionOperator.Text == "‡")
            {
                this.textBoxQueryCondition.Visible = true;
                this.comboBoxQueryCondition.Visible = true;
            }
            else
            {
                this.textBoxQueryCondition.Visible = false;
                this.comboBoxQueryCondition.Visible = false;
                //this.toolTipQueryCondition.SetToolTip(this.textBoxQueryCondition, this._QueryCondition.Description);
            }

            // comboBoxQueryConditionOperator
            // buttonQueryConditionOperator
            //if (this._QueryCondition.IsBoolean
            //    || (this._QueryCondition.IsNumeric
            //    && (this._QueryCondition.SelectFromHierachy
            //    || this._QueryCondition.SelectFromList)))
            //{
            //    this.comboBoxQueryConditionOperator.Visible = false;
            //    this.buttonQueryConditionOperator.Visible = false;
            //}
            //else
            //{
            //    this.comboBoxQueryConditionOperator.Visible = true;
            //    this.buttonQueryConditionOperator.Visible = true;
            //}


            //if (this.buttonQueryConditionOperator.Text != "‡")
            //{
            //    this.comboBoxQueryCondition.Visible = true;
            //    //this.comboBoxQueryCondition.Dock = DockStyle.Fill;
            //}
            //else
            //    this.comboBoxQueryCondition.Visible = false;


            // buttonCondition
            //if (this._QueryCondition.DisplayText.Length > 0)
            //{
            //    this.buttonCondition.Visible = true;
            //}
            //else
            //    this.buttonCondition.Visible = false;


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

            // buttonGetGeography
            // linkLabelValue
            if (this.buttonQueryConditionOperator.Text == "Ø"
                || this.buttonQueryConditionOperator.Text == " "
                || this.buttonQueryConditionOperator.Text == "•")
            {
                this.linkLabelValue.Visible = false;
                this.buttonGetGeography.Visible = false;
            }
            else
            {
                this.buttonGetGeography.Visible = true;
                this.linkLabelValue.Visible = true;
            }
        }

        private void setQueryConditionOperators()
        {
            this.comboBoxQueryConditionOperator.Items.Clear();

            this.comboBoxQueryConditionOperator.Items.Add(" ");
            this.comboBoxQueryConditionOperator.Items.Add("‡");
            this.comboBoxQueryConditionOperator.Items.Add("O");
            this.comboBoxQueryConditionOperator.Items.Add("¤");
            // war auskommentiert - unklar wieso
            this.comboBoxQueryConditionOperator.Items.Add("Ø");
            this.comboBoxQueryConditionOperator.Items.Add("•");

            //if (!this._QueryCondition.IsXML
            //    && !this._QueryCondition.IsDatetime
            //    && !this._QueryCondition.IsDate
            //    && !this._QueryCondition.SelectFromHierachy
            //    && !this._QueryCondition.SelectFromList
            //    && !this._QueryCondition.IsBoolean
            //    && this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.NoCheck)
            //{
            //    this.comboBoxQueryConditionOperator.Items.Add("~");
            //}

            //if (!this._QueryCondition.IsXML
            //    && this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.NoCheck)
            //{
            //    this.comboBoxQueryConditionOperator.Items.Add("=");
            //}

            //if (!this._QueryCondition.IsDate
            //    && !this._QueryCondition.IsDatetime
            //    && !this._QueryCondition.IsXML
            //    && this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.NoCheck)
            //{
            //    this.comboBoxQueryConditionOperator.Items.Add("≠");
            //}

            //if (!this._QueryCondition.SelectFromHierachy
            //    && !this._QueryCondition.IsXML
            //    && this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.NoCheck)
            //{
            //    this.comboBoxQueryConditionOperator.Items.Add(">");
            //    this.comboBoxQueryConditionOperator.Items.Add("<");
            //}

            //if (!this._QueryCondition.IsDate
            //    && !this._QueryCondition.SelectFromHierachy
            //    && !this._QueryCondition.SelectFromList
            //    && !this._QueryCondition.IsXML
            //    && this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.NoCheck)
            //{
            //    this.comboBoxQueryConditionOperator.Items.Add("—");
            //}

            //if (this._QueryCondition.IsXML)
            //{
            //    this.comboBoxQueryConditionOperator.Items.Add("/");
            //    this.comboBoxQueryConditionOperator.Items.Add("¬");
            //}

            //if (this._QueryCondition.SelectFromHierachy)
            //{
            //    this.comboBoxQueryConditionOperator.Items.Add("∆");
            //}

            //if (this._QueryCondition.ForeignKey.Length == 0
            //    || this._QueryCondition.IdentityColumn == this._QueryCondition.ForeignKey
            //    || this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.ForeingKeyIsNull
            //    || this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable
            //    || (!this._QueryCondition.IsXML
            //    && !this._QueryCondition.IsDatetime
            //    && !this._QueryCondition.IsDate
            //    && !this._QueryCondition.SelectFromHierachy
            //    && !this._QueryCondition.SelectFromList
            //    && !this._QueryCondition.IsBoolean
            //    && this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.NoCheck))
            //{
            //    this.comboBoxQueryConditionOperator.Items.Add("Ø");
            //    this.comboBoxQueryConditionOperator.Items.Add("•");
            //}

            //if (this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.ForeingKeyIsNull
            //        || this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
            //{
            //    this.comboBoxQueryConditionOperator.Items.Add(" ");
            //}
        }

        private void setQueryConditionOperator()
        {
            //if (this._QueryCondition.IsXML)
            //{
            //    this.buttonQueryConditionOperator.Text = "/";
            //}
            //else if (this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.ForeingKeyIsNull
            //        || this._QueryCondition.CheckIfDataExist == QueryCondition.CheckDataExistence.DatasetsInRelatedTable)
            //{
            //    this.buttonQueryConditionOperator.Text = " ";
            //}
            //else if (this._QueryCondition.IsDate
            //    || this._QueryCondition.IsBoolean
            //    || this._QueryCondition.IsDatetime)
            //{
            //    this.buttonQueryConditionOperator.Text = "=";
            //}
            //else
            //    this.buttonQueryConditionOperator.Text = "~";
            this._QueryCondition.Operator = this.buttonQueryConditionOperator.Text;

        }

        private void initControls()
        {
            this._UnitFactors = new Dictionary<_Unit, double>();
            this._UnitFactors.Add(_Unit.m, 1.0);
            this._UnitFactors.Add(_Unit.km, 1000.0);
            this._UnitFactors.Add(_Unit.feet, 0.3048);
            this._UnitFactors.Add(_Unit.miles, 1609.344);
            this.Geography = "";

            foreach (System.Collections.Generic.KeyValuePair<_Unit, double> KV in this._UnitFactors)
                this.comboBoxQueryCondition.Items.Add(KV.Key);

            if (this._QueryCondition.DisplayText.Length > 0)
            {
                this.setButtonConditionText(this._QueryCondition.DisplayText);
                //this.buttonCondition.Text = this._QueryCondition.Display(this.buttonCondition, Entity.EntityInformationField.Abbreviation);
            }
            this.toolTipQueryCondition.SetToolTip(this.textBoxQueryCondition, DiversityWorkbench.UserControls.UserControlQueryConditionText.maximal_distance_to);
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
                case "‡":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.maximal_distance_to;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                case "O":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.within_an_area;
                    this.toolTipQueryCondition.SetToolTip(this.buttonQueryConditionOperator, Message);
                    break;
                case "¤":
                    Message += DiversityWorkbench.UserControls.UserControlQueryConditionText.outside_an_area;
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

        private void buttonGetGeography_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGeography f = new DiversityWorkbench.Forms.FormGeography();
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
            {
                this.Geography = f.Geography;
                if (f.Geography.Length == 0)
                    System.Windows.Forms.MessageBox.Show("No geography has been selected");
                //else
                //{
                //    this.linkLabelValue.Text = f.Geography;
                //}
            }
        }

        private void linkLabelValue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Geography", this._Geography);
            f.ShowDialog();
        }

        private double MaxDistance
        {
            get
            {
                double max = 0.0;
                if (!double.TryParse(this.textBoxQueryCondition.Text, out max))
                    System.Windows.MessageBox.Show(textBoxQueryCondition.Text + " is not a numeric value");
                else
                {
                    if (max < 0)
                    {
                        //System.Windows.Forms.MessageBox.Show(max.ToString() + " will be converted to a positive value");
                        max = max * -1;
                        this.textBoxQueryCondition.Text = max.ToString();
                    }
                    DiversityWorkbench.UserControls.UserControlQueryConditionGIS._Unit Unit = _Unit.m;
                    if (this.comboBoxQueryCondition.Text == _Unit.km.ToString()) Unit = _Unit.km;
                    else if (this.comboBoxQueryCondition.Text == _Unit.feet.ToString()) Unit = _Unit.feet;
                    else if (this.comboBoxQueryCondition.Text == _Unit.miles.ToString()) Unit = _Unit.miles;
                    max = max * this._UnitFactors[Unit];
                }
                return max;
            }
        }

        #endregion

        #region public Properties and functions

        public string Geography
        {
            get { return _Geography; }
            set
            {
                _Geography = value;
                this.linkLabelValue.Text = value;
                if (this.linkLabelValue.Text.Length > 200)
                    this.linkLabelValue.Text = this.linkLabelValue.Text.Substring(0, 200) + " ...";
                if (_Geography.Length > 0 && this.linkLabelValue.Text.Length == 0)
                {
                    string[] Geo = _Geography.Split(new char[] { ' ' });
                    for (int i = 0; i < 3; i++)
                    {
                        this.linkLabelValue.Text += Geo[i] + " ";
                    }
                    this.linkLabelValue.Text += "...";
                }
            }
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
            if (Condition.QueryConditionOperator != "Ø"
                && Condition.QueryConditionOperator != "•"
                && Condition.DisplayText != null
                && Condition.QueryGroup != null)
                System.Windows.Forms.MessageBox.Show("Please check restriction for " + Condition.DisplayText + " in " + Condition.QueryGroup);
        }

        public string ConditionValue()
        {
            string Value = "";
            Value = this.textBoxQueryCondition.Text;
            return Value;
        }

        public string WhereClause()
        {
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
            else if (this.Geography.Length > 0)
            {
                if (this.buttonQueryConditionOperator.Text == "‡")
                {
                    QueryString += ".STDistance(geography::STGeomFromText('";
                }
                else if (this.buttonQueryConditionOperator.Text == "O")
                {
                    QueryString += ".STIntersects(geography::STGeomFromText('";
                }
                else if (this.buttonQueryConditionOperator.Text == "¤")
                {
                    QueryString += ".STDisjoint(geography::STGeomFromText('";
                }
                QueryString += this._Geography + "', 4326)) ";
                if (this.buttonQueryConditionOperator.Text == "‡")
                {
                    QueryString += " <= " + this.MaxDistance.ToString();
                }
                else if (this.buttonQueryConditionOperator.Text == "O")
                {
                    QueryString += " = 1";
                }
                else if (this.buttonQueryConditionOperator.Text == "¤")
                {
                    QueryString += " = 1";
                }
            }
            if (QueryString.Length > 0)
            {
                // MW 11.6.2015: Optimizing
                if (UserControlQueryList.UseOptimizing)
                {
                    if (UserControlQueryList.TableAliases.ContainsKey(this._QueryCondition.Table))
                        QueryString = UserControlQueryList.TableAliases[this._QueryCondition.Table] + ".[" + this._QueryCondition.Column + "]" + QueryString;
                    else
                    {
                        if (this._QueryCondition.Table != UserControlQueryList.QueryMainTable)
                        {
                            if (!UserControlQueryList.TableAliases.ContainsKey(this._QueryCondition.Table)) // Markus 14.02.2018 - vermutlich nur notwendig wenn es fehlt
                                UserControlQueryList.TableAliases.Add(this._QueryCondition.Table, "T" + UserControlQueryList.TableAliases.Count.ToString());
                            string Alias = UserControlQueryList.TableAliases[this._QueryCondition.Table];
                            QueryString = Alias + ".[" + this._QueryCondition.Column + "]" + QueryString;
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
                    QueryString = "T." + this._QueryCondition.Column + QueryString;
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
            this.textBoxQueryCondition.Text = "";
            this.Geography = "";
            this.comboBoxQueryCondition.SelectedIndex = 0;
            // Markus 11.6.24: Bugfix - Operation to reset had been missing
            this.comboBoxQueryConditionOperator.SelectedIndex = 0;
        }

        public void setEntity()
        {
            if (!DiversityWorkbench.Entity.EntityTablesExist) return;
            if (this._QueryCondition.TextFixed)
            {
                this.toolTipQueryCondition.SetToolTip(this.textBoxQueryCondition, this._QueryCondition.Description);
                this.toolTipQueryCondition.SetToolTip(this.linkLabelValue, this._QueryCondition.Description);
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
            if (Abbreviation.Length > 0)
                this.setButtonConditionText(Abbreviation);
        }

        public void RefreshProjectDependentHierarchy() { }

        #endregion

        #region Gazetteer
        private void buttonGetGazetteer_Click(object sender, EventArgs e)
        {
#if !DEBUG
            //System.Windows.Forms.MessageBox.Show("Available in upcoming version");
            //return;
#endif
            if (this._QueryCondition.iWorkbenchUnit == null)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                DiversityWorkbench.Gazetteer G = new Gazetteer(DiversityWorkbench.Settings.ServerConnection);
                DiversityWorkbench.Forms.FormRemoteQuery f = new DiversityWorkbench.Forms.FormRemoteQuery(G, DiversityWorkbench.Settings.ServerConnection);
                this.Cursor = System.Windows.Forms.Cursors.Default;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    System.Collections.Generic.Dictionary<string, string> Result = f.SelectedItem();
                    string URI = Result.First().Key;
                    System.Collections.Generic.Dictionary<string, string> Results = G.UnitValues(URI);
                    if (Results.ContainsKey("Geography"))
                    {
                        this.Geography = Results["Geography"];
                    }
                    else
                        System.Windows.Forms.MessageBox.Show("No geography has been selected");
                    //this.Geography = f.Geography;
                    //if (f.Geography.Length == 0)
                    //    System.Windows.Forms.MessageBox.Show("No geography has been selected");
                }
            }
            else
            {
                switch (this._QueryCondition.iWorkbenchUnit.getServerConnection().ModuleName)
                {
                    case "DiversitySamplingPlots":
                        this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                        DiversityWorkbench.SamplingPlot samplingPlot = (DiversityWorkbench.SamplingPlot)this._QueryCondition.iWorkbenchUnit;// new SamplingPlot(DiversityWorkbench.Settings.ServerConnection);
                        DiversityWorkbench.Forms.FormRemoteQuery f = new DiversityWorkbench.Forms.FormRemoteQuery(samplingPlot, DiversityWorkbench.Settings.ServerConnection);
                        //DiversityWorkbench.Forms.FormRemoteQuery f = new FormRemoteQuery(samplingPlot, false, true);//, DiversityWorkbench.Settings.ServerConnection);
                        this.Cursor = System.Windows.Forms.Cursors.Default;
                        f.ShowDialog();
                        {
                            System.Collections.Generic.Dictionary<string, string> Result = f.SelectedItem();
                            if (Result.Count > 0)
                            {
                                string URI = Result.First().Key;
                                System.Collections.Generic.Dictionary<string, string> Results = samplingPlot.UnitValues(URI);
                                if (Results.ContainsKey("Geography"))
                                {
                                    this.Geography = Results["Geography"];
                                }
                            }
                            else
                                System.Windows.Forms.MessageBox.Show("No geography has been selected");
                        }
                        break;
                }
            }
        }

        #endregion

    }
}
