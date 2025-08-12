using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetItemFromHierarchy : Form
    {
        #region Parameter

        private System.Data.DataTable _DtHierachy;
        private bool _RemoveMissingParentsRelations = false;

        #endregion

        #region Construction
        /// <summary>
        /// Get an item based on a hierarchy
        /// </summary>
        /// <param name="DtHierachy">The table containing the hierachy</param>
        /// <param name="HierarchyColumn">The PK column</param>
        /// <param name="ParentColumn">The parent column related to the PK</param>
        /// <param name="DisplayColumn">The display column for the combobox in the form</param>
        /// <param name="ValueColumn">The value column for the combobox in the form - this value will be returned</param>
        /// <param name="HeaderText">The text of the header label in the form</param>
        /// <param name="FormText">The text of the form</param>
        public FormGetItemFromHierarchy(System.Data.DataTable DtHierachy, string HierarchyColumn, string ParentColumn, string DisplayColumn, string ValueColumn, string HeaderText, string FormText, bool IgnoreQueryLimitHierarchy = false)
        {
            InitializeComponent();
            this.userControlHierarchySelector.initHierarchy(DtHierachy, HierarchyColumn, ParentColumn, DisplayColumn, DisplayColumn, ValueColumn, this.comboBox);
            if (HeaderText.Length > 0) this.labelHeader.Text = HeaderText;
            if (FormText.Length > 0) this.Text = FormText;
            try
            {
                this._DtHierachy = DtHierachy;
                this.comboBox.DataSource = this._DtHierachy;
                if (DisplayColumn.Length > 0)
                    this.comboBox.DisplayMember = DisplayColumn;
                else
                    this.comboBox.DisplayMember = this._DtHierachy.Columns[0].ToString();
                if (ValueColumn.Length > 0)
                    this.comboBox.ValueMember = ValueColumn;
                else
                {
                    if (this._DtHierachy.Columns.Count > 1)
                        this.comboBox.ValueMember = this._DtHierachy.Columns[1].ToString();
                    else
                        this.comboBox.ValueMember = this._DtHierachy.Columns[0].ToString();
                }
            }
            catch { }
        }

        public FormGetItemFromHierarchy(System.Data.DataTable DtHierachy, string HierarchyColumn, string ParentColumn, string DisplayColumnForCombobox, string DisplayColumnForHierarchy, string ValueColumn, string HeaderText, string FormText, bool RemoveMissingHierarchyRelations, 
            bool RestrictToList = false)
        {
            InitializeComponent();
            this.userControlHierarchySelector.initHierarchy(DtHierachy, HierarchyColumn, ParentColumn, DisplayColumnForHierarchy, DisplayColumnForHierarchy, ValueColumn, true, this.comboBox);
            if (HeaderText.Length > 0) this.labelHeader.Text = HeaderText;
            if (FormText.Length > 0) this.Text = FormText;
            if (RestrictToList) this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            try
            {
                this._DtHierachy = DtHierachy;
                this.comboBox.DataSource = this._DtHierachy;
                if (DisplayColumnForCombobox.Length > 0)
                    this.comboBox.DisplayMember = DisplayColumnForCombobox;
                else
                    this.comboBox.DisplayMember = this._DtHierachy.Columns[0].ToString();
                if (ValueColumn.Length > 0)
                    this.comboBox.ValueMember = ValueColumn;
                else
                {
                    if (this._DtHierachy.Columns.Count > 1)
                        this.comboBox.ValueMember = this._DtHierachy.Columns[1].ToString();
                    else
                        this.comboBox.ValueMember = this._DtHierachy.Columns[0].ToString();
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        private void initForm()
        {
        }
        
        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }
        
        #endregion

        #region Properties

        public string SelectedString { get { return this.comboBox.Text; } }

        public string SelectedValue
        {
            get
            {
                if (this._DtHierachy.Columns.Count > 1)
                {
                    if (this.comboBox.Sorted)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.comboBox.SelectedItem;
                        return R[this.comboBox.ValueMember.ToString()].ToString();
                    }
                    if (this.comboBox.SelectedValue == null)
                        return "NULL";
                    return this.comboBox.SelectedValue.ToString();
                }
                else
                    return this.comboBox.Text;
            }
        }

        #endregion

    }
}