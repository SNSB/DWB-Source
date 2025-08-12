using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormPresetStorageUnit : Form
    {
        #region Parameter
        System.Data.DataTable dtCollection;
        #endregion

        #region Construction
        public FormPresetStorageUnit()
        {
            InitializeComponent();
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxMaterialCategory, "CollMaterialCategory_Enum", con);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxTaxonomicGroup, "CollTaxonomicGroup_Enum", con);
            this.dtCollection = new System.Data.DataTable("Collection");
            string SQL = "SELECT CollectionID, CollectionName " +
                "FROM Collection " +
                "ORDER BY DisplayOrder";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this.dtCollection);
            this.comboBoxCollection.DataSource = this.dtCollection;
            this.comboBoxCollection.DisplayMember = "CollectionName";
            this.comboBoxCollection.ValueMember = "CollectionID";
        }

        public FormPresetStorageUnit(int CollectionID, string MaterialCategory, string TaxonomicGroup, string StorageLocation)
            : this()
        {
            int i = 0;
            if (CollectionID > 0)
            {
                for (i = 0; i < this.dtCollection.Rows.Count; i++)
                {
                    if (dtCollection.Rows[i]["CollectionID"].ToString() == CollectionID.ToString())
                        break;
                }
                this.comboBoxCollection.SelectedIndex = i;
            }

            if (MaterialCategory != null && MaterialCategory.Length > 0)
            {
                for (i = 0; i < this.comboBoxMaterialCategory.Items.Count; i++)
                {
                    if (this.comboBoxMaterialCategory.Items[i].ToString() == MaterialCategory)
                        break;
                }
                this.comboBoxMaterialCategory.SelectedIndex = i - 1;
            }

            if (TaxonomicGroup != null && TaxonomicGroup.Length > 0)
            {
                for (i = 0; i < this.comboBoxTaxonomicGroup.Items.Count; i++)
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxTaxonomicGroup.Items[i];
                    string TaxGr = R[0].ToString();
                    if (TaxGr == TaxonomicGroup)
                        break;
                }
                this.comboBoxTaxonomicGroup.SelectedIndex = i;
            }

            if (StorageLocation.Length > 0)
                this.textBoxStorageLocation.Text = StorageLocation;
        }
        
        #endregion

        #region Properties
        public int CollectionID { get { return int.Parse(this.comboBoxCollection.SelectedValue.ToString()); } }
        public string Collection { get { return this.comboBoxCollection.Text.ToString(); } }
        public string MaterialCategory { get { return this.comboBoxMaterialCategory.SelectedValue.ToString(); } }
        public string TaxonomicGroup { get { return this.comboBoxTaxonomicGroup.SelectedValue.ToString(); } }
        public string StorageLocation { get { return this.textBoxStorageLocation.Text.ToString(); } }
        #endregion


    }
}