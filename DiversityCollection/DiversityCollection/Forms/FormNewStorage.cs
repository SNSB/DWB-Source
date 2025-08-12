using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormNewStorage : Form
    {

        #region Parameter
        System.Data.DataTable dtCollection;
        
        #endregion

        #region Construction
        public FormNewStorage()
        {
            InitializeComponent();
            Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString);
            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(this.comboBoxMaterialCategory, "CollMaterialCategory_Enum", con);
            this.dtCollection = new System.Data.DataTable("Collection");
            string SQL = "SELECT CollectionID, CollectionName " +
                "FROM Collection " +
                "ORDER BY DisplayOrder";
            Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
            ad.Fill(this.dtCollection);
            this.comboBoxCollection.DataSource = this.dtCollection;
            this.comboBoxCollection.DisplayMember = "CollectionName";
            this.comboBoxCollection.ValueMember = "CollectionID";
            this.userControlDialogPanel.buttonOK.Click -= new System.EventHandler(this.userControlDialogPanel.buttonOK_Click);
            this.userControlDialogPanel.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
        }

        public FormNewStorage(int CollectionID, string StorageLocation) : this()
        {
            int i = 0;
            for (i = 0; i < this.dtCollection.Rows.Count; i++)
            {
                if (dtCollection.Rows[i]["CollectionID"].ToString() == CollectionID.ToString())
                    break;
            }
            this.comboBoxCollection.SelectedIndex = i;
            this.textBoxStorageLocation.Text = StorageLocation;
            this.userControlDialogPanel.buttonOK.Click -= new System.EventHandler(this.userControlDialogPanel.buttonOK_Click);
            this.userControlDialogPanel.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
        }



        #endregion

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (this.comboBoxCollection.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a collection");
                return;
            }
            if (this.comboBoxMaterialCategory.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please select a material category");
                return;
            }
            if (this.textBoxStorageLocation.Text.Length == 0)
            {
                System.Windows.Forms.MessageBox.Show("Please give a name for the storage location");
                return;
            }
            System.Windows.Forms.Form f = this.ParentForm;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #region Properties
        public int CollectionID { get { return int.Parse(this.comboBoxCollection.SelectedValue.ToString()); } }
        public string MaterialCategory { get { return this.comboBoxMaterialCategory.SelectedValue.ToString(); } }
        public string StorageLocation { get { return this.textBoxStorageLocation.Text; } }
        #endregion
    }
}