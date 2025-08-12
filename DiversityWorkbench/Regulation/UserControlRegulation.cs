using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Regulation
{
    public partial class UserControlRegulation : UserControl
    {

        #region Parameter

        private Microsoft.Data.SqlClient.SqlDataAdapter _AdapterRegulation;
        //private string _Regulation = "";
        private readonly string _SqlSelect = "SELECT Regulation, ParentRegulation, Type, ProjectURI, Status, Notes, HierarchyOnly FROM Regulation";
        public enum DesignVersion { Default, ObtainedFromProjects, RegulationInProjects }
        private DesignVersion _DesignVersion = DesignVersion.Default;
        private bool _ReadOnly = false;
        private Regulation _Regulation;

        #endregion

        #region Construction

        public UserControlRegulation(DesignVersion Design = DesignVersion.Default, bool ReadOnly = false)
        {
            InitializeComponent();
            this._DesignVersion = Design;
            this._ReadOnly = ReadOnly;
            this.initControl();
        }

        public UserControlRegulation(Regulation Regulation, DesignVersion Design = DesignVersion.Default, bool ReadOnly = false)
        {
            InitializeComponent();
            this._Regulation = Regulation;
            this._DesignVersion = Design;
            this.setReadOnly(ReadOnly);
            this.initControl();
        }

        #endregion

        public void SetRegulation(string Regulation)
        {
            if (this._Regulation == null)
                this._Regulation = new Regulation(Regulation);
            this.LoadData();
        }

        public System.Data.DataSet DataSet()
        {
            return this.dataSetRegulation;
        }

        private void initControl()
        {
            switch(this._DesignVersion)
            {
                case DesignVersion.Default:
                    this.splitContainerMain.Panel1Collapsed = true;
                    this.initProject();
                    break;
                case DesignVersion.ObtainedFromProjects:
                    this.splitContainerMain.Panel2Collapsed = true;
                    break;
                case DesignVersion.RegulationInProjects:
                    //this.userControlModuleRelatedEntryProject.Visible = false;
                    this.splitContainerMain.Panel1Collapsed = true;
                    this._ReadOnly = true;
                    break;
            }
            DiversityWorkbench.Agent A = new Agent(DiversityWorkbench.Settings.ServerConnection);

            this.userControlModuleRelatedEntryContractGiver.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
            this.userControlModuleRelatedEntryContractGiver.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
            this.userControlModuleRelatedEntryContractGiver.bindToData("Regulation", "ContractGiver", "ContractGiverAgentURI", this.regulationBindingSource);

            this.userControlModuleRelatedEntryContractAcceptor.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
            this.userControlModuleRelatedEntryContractAcceptor.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
            this.userControlModuleRelatedEntryContractAcceptor.bindToData("Regulation", "ContractAcceptor", "ContractAcceptorAgentURI", this.regulationBindingSource);

            this.userControlModuleRelatedEntryResponsiblePrincipleInvestigator.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)A;
            this.userControlModuleRelatedEntryResponsiblePrincipleInvestigator.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
            this.userControlModuleRelatedEntryResponsiblePrincipleInvestigator.bindToData("Regulation", "ResponsiblePrincipleInvestigator", "ResponsiblePrincipleInvestigatorAgentURI", this.regulationBindingSource);
        }

        public void SetProjectURI(string URI)
        {
            this.linkLabelProjectURI.Text = URI;
            this.linkLabelProjectURI.Visible = true;
        }

        private void initProject()
        {
            DiversityWorkbench.Project P = new DiversityWorkbench.Project(DiversityWorkbench.Settings.ServerConnection);
            //this.userControlModuleRelatedEntryProject.IWorkbenchUnit = (DiversityWorkbench.IWorkbenchUnit)P;
            //this.userControlModuleRelatedEntryProject.HelpProvider.HelpNamespace = this.helpProvider.HelpNamespace;
            //this.userControlModuleRelatedEntryProject.bindToData("Regulation", "Regulation", "ProjectURI", this.regulationBindingSource);
            //this.userControlModuleRelatedEntryProject.labelURI.TextChanged += new System.EventHandler(this.labelProjectURI_TextChanged);
        }

        private void setReadOnly(bool ReadOnly)
        {
            this._ReadOnly = ReadOnly;
            if (this._ReadOnly)
            {
                foreach (System.Windows.Forms.Control C in this.tableLayoutPanelRegulation.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                    {
                        System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)C;
                        tb.ReadOnly = true;
                    }
                    else if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                    {
                        System.Windows.Forms.ComboBox tb = (System.Windows.Forms.ComboBox)C;
                        tb.Enabled = false;
                    }
                    else if (C.GetType() == typeof(System.Windows.Forms.DateTimePicker))
                    {
                        System.Windows.Forms.DateTimePicker tb = (System.Windows.Forms.DateTimePicker)C;
                        tb.Enabled = false;
                    }
                    else if (C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry))
                    {
                        DiversityWorkbench.UserControls.UserControlModuleRelatedEntry tb = (DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)C;
                        tb.Enabled = false;
                    }
                }
                this.userControlModuleRelatedEntryContractAcceptor.Enabled = false;
                this.userControlModuleRelatedEntryContractGiver.Enabled = false;
                this.toolStripButtonRegulationIdentifierAdd.Enabled = false;
                this.toolStripButtonRegulationIdentifierDelete.Enabled = false;
                this.toolStripButtonRegulationResourceAdd.Enabled = false;
                this.toolStripButtonRegulationResourceDelete.Enabled = false;
                foreach (System.Windows.Forms.Control C in this.tableLayoutPanelRegulationResource.Controls)
                {
                    if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                    {
                        System.Windows.Forms.TextBox tb = (System.Windows.Forms.TextBox)C;
                        tb.ReadOnly = true;
                    }
                    else if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                    {
                        System.Windows.Forms.ComboBox tb = (System.Windows.Forms.ComboBox)C;
                        tb.Enabled = false;
                    }
                    else if (C.GetType() == typeof(System.Windows.Forms.DateTimePicker))
                    {
                        System.Windows.Forms.DateTimePicker tb = (System.Windows.Forms.DateTimePicker)C;
                        tb.Enabled = false;
                    }
                    else if (C.GetType() == typeof(DiversityWorkbench.UserControls.UserControlModuleRelatedEntry))
                    {
                        DiversityWorkbench.UserControls.UserControlModuleRelatedEntry tb = (DiversityWorkbench.UserControls.UserControlModuleRelatedEntry)C;
                        tb.Enabled = false;
                    }
                }
            }
        }

        private bool SaveData()
        {
            bool OK = true;
            try
            {
                this._AdapterRegulation.Update(this.dataSetRegulation.Regulation);
                this.LoadData();
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                OK = false;
            }
            return OK;
        }

        private UserControlRegulation _URC_ProjectRegulation;

        //private void labelProjectURI_TextChanged(object sender, EventArgs e)
        //{
        //    if (this.userControlModuleRelatedEntryProject.labelURI.Text.Length == 0)
        //    {
        //        this.splitContainerMain.Panel1Collapsed = true;
        //        this.splitContainerMain.Panel2Collapsed = false;
        //    }
        //    else
        //    {
        //        this.splitContainerMain.Panel2Collapsed = false;
        //        this.splitContainerMain.Panel1Collapsed = true;
        //        if (this.splitContainerProject.Panel2.Controls.Count == 0)
        //        {
        //            this._URC_ProjectRegulation = new UserControlRegulation();
        //            _URC_ProjectRegulation.Dock = DockStyle.Fill;
        //            this.splitContainerProject.Panel2.Controls.Add(_URC_ProjectRegulation);
        //        }
        //    }
        //}

        private void LoadData()
        {
            string SQL = this._SqlSelect + " WHERE Regulation = '" + this._Regulation.RegulationList()[0] + "'";
            this.dataSetRegulation.Regulation.Clear();
            DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._AdapterRegulation, this.dataSetRegulation.Regulation, SQL, DiversityWorkbench.Settings.ConnectionString);
        }

        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void comboBoxStatus_DropDown(object sender, EventArgs e)
        {
            System.Data.DataTable dtStatus = new DataTable();
            string SQL = "SELECT DISTINCT [Status] " +
                "FROM Regulation " +
                "UNION " +
                "SELECT NULL AS [Status] " +
                "ORDER BY [Status]";
            string Message = "";
            if (DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dtStatus, ref Message))
            {
                this.comboBoxStatus.DataSource = dtStatus;
                this.comboBoxStatus.DisplayMember = "Status";
                this.comboBoxStatus.ValueMember = "Status";
            }
        }

        private void listBoxProjectRegulations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this._URC_ProjectRegulation != null)
            {

            }
        }

        #region Resource

        private void toolStripButtonRegulationResourceAdd_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonRegulationResourceDelete_Click(object sender, EventArgs e)
        {

        }
        
        #endregion

        #region Identifier
        
        private void toolStripButtonRegulationIdentifierDelete_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButtonRegulationIdentifierAdd_Click(object sender, EventArgs e)
        {

        }
        
        #endregion


    }
}
