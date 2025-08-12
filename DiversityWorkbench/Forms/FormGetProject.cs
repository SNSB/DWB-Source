using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetProject : Form
    {
        //private string _Section = "";
        public FormGetProject()
        {
            InitializeComponent();
        }

        public enum Subgroup {None, Section, Checklist }

        /// <summary>
        /// Getting a project from a database
        /// </summary>
        /// <param name="SC">The server connection to access the database</param>
        /// <param name="Header">An optional message if different from the standard message</param>
        /// <param name="Section">An optional string changing the source to e.g. Section in DSP or Checklist in DTN</param>
        public FormGetProject(DiversityWorkbench.ServerConnection SC, string Header, Subgroup Group = Subgroup.None, string Restriction = "", bool SilentMode = false)
        {
            InitializeComponent();
            try
            {
                if (Group != Subgroup.None)
                {
                    this.Text = Group.ToString();
                    if (Header.Length > 0)
                    {
                        this.label.Text = Header;
                        this.label.Text += "\r\nSource: " + SC.DatabaseName;
                        if (SC.Project != null && SC.Project.Length > 0)
                            this.label.Text += "\r\nProject: " + SC.Project;
                    }
                    else
                    {
                        this.label.Text = "Please select a ";
                        this.label.Text += Group.ToString().ToLower();
                        this.label.Text += " from " + SC.DatabaseName;
                        if (SC.Project != null && SC.Project.Length > 0)
                            this.label.Text += "\r\nProject: " + SC.Project;
                    }

                    switch (Group)
                    {
                        case Subgroup.Section:
                            this.Icon = DiversityWorkbench.Forms.FormFunctions.IconFromImage(DiversityWorkbench.Properties.Resources.Section);
                            break;
                        case Subgroup.Checklist:
                            this.Icon = DiversityWorkbench.Forms.FormFunctions.IconFromImage(DiversityWorkbench.Properties.Resources.Checklist);
                            break;
                    }
                }
                else if (Header.Length > 0)
                    this.label.Text = Header;
                else
                    this.label.Text = "Please select a project from " + SC.DatabaseName;
                string Prefix = "dbo.";
                if (SC.LinkedServer.Length > 0)
                    Prefix = "[" + SC.LinkedServer + "].[" + SC.DatabaseName + "].dbo.";
                string SQL = "SELECT ProjectID, Project FROM " + Prefix;
                if (Group != Subgroup.None)
                {
                    SQL = "SELECT ";
                    switch (SC.ModuleName)
                    {
                        case "DiversityTaxonNames":
                            SQL += "ProjectID, DisplayText AS Project " +
                                "FROM " + Prefix + "TaxonNameListProjectProxy ORDER BY Project";
                            break;
                        case "DiversityScientificTerms":
                            SQL += "SectionID AS ProjectID, DisplayText AS Project " +
                                "FROM " + Prefix + "Section ";
                            if (Restriction.Length > 0)
                                SQL += " WHERE " + Restriction;
                            SQL += "ORDER BY Project ";
                            break;
                        default:
                            SQL += "ProjectList";
                            break;
                    }
                }
                else
                {
                    switch (SC.ModuleName)
                    {
                        case "DiversityGazetteer":
                        case "DiversityScientificTerms":
                            SQL += "ProjectProxy";
                            break;
                        default:
                            SQL += "ProjectList";
                            break;
                    }
                    SQL += " ORDER BY Project";

                }
                System.Data.DataTable dt = new DataTable();
                Microsoft.Data.SqlClient.SqlDataAdapter ad = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, SC.ConnectionString);
                ad.Fill(dt);
                this._ProjectCount = dt.Rows.Count;
                if (dt.Rows.Count == 0 && !SilentMode)
                {
                    if (Group == Subgroup.None)
                        System.Windows.Forms.MessageBox.Show("No projects available");
                    else
                        System.Windows.Forms.MessageBox.Show("No " + Group.ToString().ToLower() + " available");
                    this.Close();
                }
                else
                {
                    this.comboBox.DataSource = dt;
                    this.comboBox.DisplayMember = "Project";
                    this.comboBox.ValueMember = "ProjectID";
                    this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                }
            }
            catch(System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region Properties

        public DiversityWorkbench.UserControls.UserControlDialogPanel.DisplayOption DisplayOption
        {
            set { this.userControlDialogPanel.Display = value; }
        }


        private int _ProjectCount = 0;
        public int ProjectCount { get { return this._ProjectCount; } }

        public int? ProjectID
        {
            get
            {
                if (this.comboBox.SelectedValue != null)
                    return int.Parse(this.comboBox.SelectedValue.ToString());
                else return null;
            }
        }
        public string Project 
        { 
            get 
            {
                string ProjectName = "";
                try
                {
                    if (ProjectName.Length == 0)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)this.comboBox.SelectedItem;
                        ProjectName = R["Project"].ToString();
                    }
                    if (ProjectName.Length == 0 && this.comboBox.SelectedText != null)
                        ProjectName = this.comboBox.SelectedText;
                    if (ProjectName.Length == 0)
                        ProjectName = this.comboBox.Text;
                }
                catch (System.Exception ex)
                {
                }
                return ProjectName; 
            } 
        }

        #endregion


    }
}
