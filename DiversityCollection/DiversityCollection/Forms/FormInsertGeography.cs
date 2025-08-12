using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormInsertGeography : Form
    {
        #region Parameter
        private System.Data.DataTable DtLocalisationSystem;
        
        #endregion

        #region Construction
        public FormInsertGeography(System.Data.DataTable dtLocalisationSystem)
        {
            InitializeComponent();
            this.DtLocalisationSystem = dtLocalisationSystem;
            this.initForm();
        }
        
        #endregion


        #region Form
        private void initForm()
        {
            this.comboBoxLocalisationSystem.DataSource = this.DtLocalisationSystem;
            this.comboBoxLocalisationSystem.DisplayMember = "LocalisationSystemName";
            this.comboBoxLocalisationSystem.ValueMember = "LocalisationSystemID";
        }
        
        #endregion


        #region Properties
		public int LocalisationSystemID
        {
            get
            {
                int ID = -1;
                if (this.comboBoxLocalisationSystem.SelectedIndex != -1)
                    ID = int.Parse(this.comboBoxLocalisationSystem.SelectedValue.ToString());
                return ID;
            }
        }

        public string LocalisationSystemName
        {
            get
            {
                string Name = "";
                if (this.comboBoxLocalisationSystem.SelectedIndex != -1)
                    Name = this.comboBoxLocalisationSystem.Text;
                return Name;
            }
        }
        
        #endregion    
    }
}