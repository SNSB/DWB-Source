using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormImportWizardTranslation : Form
    {
        #region Parameter

        private DiversityCollection.Import_Column _IC;
        private string _SqlForEnumerationTable;
        
        #endregion

        #region Construction und From

        public FormImportWizardTranslation(DiversityCollection.Import_Column IC)
        {
            InitializeComponent();
            this._IC = IC;
            this._SqlForEnumerationTable = this.SqlForEnumerationTable;
            this.initForm();
        }
        
        private void initForm()
        {
            System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(this._SqlForEnumerationTable, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this.dataSetImportWizard.DataTableEnumeration);
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._IC.TranslationDictionary)
            {
                System.Data.DataRow R = this.dataSetImportWizard.DataTableTranslation.NewDataTableTranslationRow();
                R[0] = KV.Key;
                this.dataSetImportWizard.DataTableTranslation.Rows.Add(R);
            }
        }

        private void FormImportWizardTranslation_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                foreach (System.Data.DataRow R in this.dataSetImportWizard.DataTableTranslation.Rows)
                {
                    this._IC.TranslationDictionary[R[0].ToString()] = R[1].ToString();
                }
            }
        }

        #endregion

        #region Properties
        
        private string SqlForEnumerationTable
        {
            get
            {
                string SQL = "";

                try
                {
                    SQL = "SELECT C.COLUMN_NAME, C.TABLE_NAME " +
                        "FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS K INNER JOIN " +
                        "INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON K.CONSTRAINT_NAME = R.CONSTRAINT_NAME INNER JOIN " +
                        "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE AS C ON R.UNIQUE_CONSTRAINT_NAME = C.CONSTRAINT_NAME " +
                        "WHERE (K.TABLE_NAME = '" + this._IC.Table + "') and K.COLUMN_NAME = '" + this._IC.Column + "'";
                    System.Data.DataTable dt = new DataTable();
                    System.Data.SqlClient.SqlDataAdapter ad = new System.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                    ad.Fill(dt);
                    string Table = dt.Rows[0][0].ToString();
                    string Column = dt.Rows[0][1].ToString();
                    SQL = "SELECT C.COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS C " +
                        "WHERE C.TABLE_NAME = '" + Table + "'" +
                        "AND C.COLUMN_NAME <> '" + Column + "'" +
                        "AND C.COLUMN_NAME NOT LIKE 'Log%' " +
                        "AND C.COLUMN_NAME <> 'RowGUID' " +
                        "AND C.COLUMN_NAME NOT LIKE '%ID' " +
                        "AND C.COLUMN_NAME NOT LIKE '%URI'";
                    System.Data.DataTable dtColumns = new DataTable();
                    ad.SelectCommand.CommandText = SQL;
                    ad.Fill(dtColumns);
                    string DisplayColumn = Column;
                    System.Collections.Generic.List<string> DisplayColumnList = new List<string>();
                    DisplayColumnList.Add("DisplayText");
                    DisplayColumnList.Add("Display");
                    foreach (System.Data.DataRow R in dtColumns.Rows)
                    {
                        string Col = R[0].ToString();
                        if (DisplayColumnList.Contains(Col))
                        {
                            DisplayColumn = Col;
                            break;
                        }
                        if (Col.IndexOf(Table) > -1 && (Col.IndexOf("Name") > -1 || Col.IndexOf("Title") > -1))
                        {
                            DisplayColumn = Col;
                            break;
                        }
                    }
                    SQL = "SELECT " + Column + " AS [Key], " + DisplayColumn + " AS DisplayText " +
                        "FROM  " + Table + "ORDER BY DisplayText";
                }
                catch (Exception)
                {
                }

                return SQL;
            }
        }
        
        #endregion
    }
}
