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
    /// <summary>
    /// Privides search options for XML fields where a template is defined
    /// </summary>
    public partial class UserControlQueryConditionXML : UserControl, IUserControlQueryCondition
    {
        #region Parameter

        private DiversityWorkbench.QueryCondition _QueryCondition;
        private string _TemplateXmlColumn;
        private string _TemplateXmlTable;
        private string _TemplateXmlProjectIDColumn;
        public enum SourceOfTemplate { Database };

        private UserControlQueryList _UserControlQueryList;
        public void setUserControlQueryList(UserControlQueryList List)
        {
            this._UserControlQueryList = List;
        }

        #endregion

        #region Construction

        public UserControlQueryConditionXML()
        {
            InitializeComponent();
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
        }

        public string ConditionValue()
        {
            string Value = "";
            Value = this.textBoxQueryCondition.Text;
            return Value;
        }

        public string WhereClause()
        {
            return "";


            if (this.buttonQueryConditionOperator.Text == " ")
            {
                return "";
            }
            string QueryString = "";
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
                //if (this.textBoxQueryCondition.Text.Length == 0 && this.buttonQueryConditionOperator.Text != "•")
                //    return "";
                //string Operator = this.buttonQueryConditionOperator.Text;
                //string ReferencedTable = "";
                //if (this.comboBoxReferencedTable.SelectedValue != null)
                //    ReferencedTable = this.comboBoxReferencedTable.SelectedValue.ToString();
                //string AnnotationType = "";
                //if (this.comboBoxAnnotationType.SelectedValue != null)
                //    AnnotationType = this.comboBoxAnnotationType.SelectedValue.ToString();
                //QueryString = this._QueryCondition.Annotation.WhereClause(ReferencedTable, this._QueryCondition.Column, Operator, this.textBoxQueryCondition.Text, AnnotationType);


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
            //this.comboBoxReferencedTable.SelectedIndex = 0;
            //this.comboBoxAnnotationType.SelectedIndex = 0;
            //this.comboBoxAnnotationType_SelectionChangeCommitted(null, null);
        }

        public void setEntity()
        {
            if (!DiversityWorkbench.Entity.EntityTablesExist) return;
            if (this._QueryCondition.TextFixed)
            {
                //this.toolTipQueryCondition.SetToolTip(this.textBoxQueryCondition, this._QueryCondition.Description);
                //this.toolTipQueryCondition.SetToolTip(this.buttonCondition, this._QueryCondition.Description);
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
                //this.toolTipQueryCondition.SetToolTip(this.textBoxQueryCondition, Description);
                //this.toolTipQueryCondition.SetToolTip(this.buttonCondition, Description);
            }
        }

        public void RefreshProjectDependentHierarchy() { }

        #endregion


    }
}
