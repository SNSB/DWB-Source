using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormTaxonomicGroups : Form
    {
        public FormTaxonomicGroups(System.Windows.Forms.ImageList Images)
        {
            InitializeComponent();
            this.treeViewTaxonomicGroups.ImageList = Images;
            this.initForm();
        }

        private void initForm()
        {
            int i = 0;
            foreach (System.Data.DataRow R in DiversityCollection.LookupTable.DtTaxonomicGroups.Rows)
            {
                string Visibility = DiversityCollection.Forms.FormCollectionSpecimenSettings.Default.TaxonomicGroups;
                System.Windows.Forms.TreeNode N = new TreeNode(R["DisplayText"].ToString());
                N.ImageIndex = DiversityCollection.Specimen.TaxonImage(R["Code"].ToString(), false);
                N.SelectedImageIndex = DiversityCollection.Specimen.TaxonImage(R["Code"].ToString(), false);
                string Visible = "1";
                if (Visibility.Length > i)
                    Visible = Visibility.Substring(i, 1);
                if (Visible == "1") N.Checked = true;
                else N.Checked = false;
                N.ToolTipText = R["Description"].ToString();
                this.treeViewTaxonomicGroups.Nodes.Add(N);
                i++;
            }
            this.Height = 100 + i * 20;
        }

        public string VisibilityTaxonomicGroups
        {
            get
            {
                string V = "";
                foreach (System.Windows.Forms.TreeNode N in this.treeViewTaxonomicGroups.Nodes)
                {
                    if (N.Checked) V += "1";
                    else V += "0";
                }
                return V;
            }
        }
    }
}