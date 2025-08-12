using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormInsertUnit : Form
    {
        #region Construction
        public FormInsertUnit()
        {
            InitializeComponent();
            this.initForm();
        }
        #endregion

        #region Form
        private void initForm()
        {
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTaxonomicGroup, "CollTaxonomicGroup_Enum", con);
        }
        #endregion

        #region Properties
		public string TaxonomicGroup
        {
            get
            {
                string TaxGroup = "";
                if (this.comboBoxTaxonomicGroup.SelectedIndex > -1)
                    TaxGroup = this.comboBoxTaxonomicGroup.SelectedValue.ToString();
                return TaxGroup;
            }
        }
	    #endregion   
    }
}