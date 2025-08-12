using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormEntityInsertPK : Form
    {
        private string _Table;
        private string _PkColumn = "";

        public FormEntityInsertPK(string TableName)
        {
            InitializeComponent();
            this._Table = TableName;
            this.initForm();
        }

        private void initForm()
        {
            string SQL = "select count(*) from INFORMATION_SCHEMA.KEY_COLUMN_USAGE K " +
                "where K.CONSTRAINT_NAME Like 'PK_%' " +
                "and K.TABLE_NAME = '" + _Table + "'";
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            Microsoft.Data.SqlClient.SqlCommand C = new Microsoft.Data.SqlClient.SqlCommand(SQL, con);
            try
            {
                con.Open();
                int i = 0;
                if (!int.TryParse(C.ExecuteScalar()?.ToString(), out i))
                    return;
                if (i != 1)
                {
                    System.Windows.Forms.MessageBox.Show("This function is only available for tables with a PK comprising one column");
                    this.DialogResult = DialogResult.No;
                    this.Close();
                }
                else
                {
                    C.CommandText = "select K.COLUMN_NAME from INFORMATION_SCHEMA.KEY_COLUMN_USAGE K " +
                        "where K.CONSTRAINT_NAME Like 'PK_%' " +
                        "and K.TABLE_NAME = '" + _Table + "'";
                    _PkColumn = C.ExecuteScalar()?.ToString();
                }
                con.Close();
            }
            catch 
            {
                this.DialogResult = DialogResult.No;
                this.Close(); 
            }

            System.Data.DataTable dtColumns = new DataTable();
            SQL = "select C.COLUMN_NAME from INFORMATION_SCHEMA.COLUMNS C " +
                "where C.TABLE_NAME = '" + this._Table + "' " +
                "order by C.ORDINAL_POSITION";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(dtColumns);
            System.Data.DataTable dtDisplayText = dtColumns.Copy();
            this.comboBoxDisplayText.DataSource = dtDisplayText;
            this.comboBoxDisplayText.DisplayMember = "COLUMN_NAME";
            this.comboBoxDisplayText.ValueMember = "COLUMN_NAME";

            System.Data.DataTable dtAbbreviation = dtColumns.Copy();
            this.comboBoxAbbreviation.DataSource = dtAbbreviation;
            this.comboBoxAbbreviation.DisplayMember = "COLUMN_NAME";
            this.comboBoxAbbreviation.ValueMember = "COLUMN_NAME";

            System.Data.DataTable dtDescription = dtColumns.Copy();
            this.comboBoxDescription.DataSource = dtDescription;
            this.comboBoxDescription.DisplayMember = "COLUMN_NAME";
            this.comboBoxDescription.ValueMember = "COLUMN_NAME";
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        public string ColumnForDisplayText { get { return this.comboBoxDisplayText.Text; } }
        public string ColumnForAbbreviation { get { return this.comboBoxAbbreviation.Text; } }
        public string ColumnForDescription { get { return this.comboBoxDescription.Text; } }
        public string ColumnForPrimaryKey { get { return this._PkColumn; } }
    }
}
