using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormGetProject : Form
    {
        #region Construction

        public FormGetProject(System.Data.DataTable dtProjects)
        {
            InitializeComponent();
            this.comboBoxProjects.DataSource = dtProjects;
            this.comboBoxProjects.DisplayMember = "Project";
            this.comboBoxProjects.ValueMember = "ProjectID";
        }

        public FormGetProject(System.Data.DataTable dtProjects, string Title, bool ProjectsAreAccessible)
        {
            InitializeComponent();
            this.comboBoxProjects.DataSource = dtProjects;
            this.comboBoxProjects.DisplayMember = "Project";
            this.comboBoxProjects.ValueMember = "ProjectID";
            if (Title.Length > 0)
                this.Text = Title;
            if (!ProjectsAreAccessible)
                this.comboBoxProjects.BackColor = System.Drawing.Color.Pink;
        }

        #endregion

        #region Properties

        public int? ProjectID 
        { 
            get 
            {
                if (this.comboBoxProjects.SelectedValue != null)
                    return int.Parse(this.comboBoxProjects.SelectedValue.ToString());
                else return null;
            } 
        }
        public string Project 
        { 
            get 
            {
                try
                {
                    System.Data.DataRowView R = (System.Data.DataRowView)this.comboBoxProjects.SelectedItem;
                    return R["Project"].ToString();
                }
                catch(System.Exception ex) { }
                return this.comboBoxProjects.SelectedText; 
            } 
        }

        #endregion

    }
}