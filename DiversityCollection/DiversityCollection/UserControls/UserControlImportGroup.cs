using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.UserControls
{
    public partial class UserControlImportGroup : UserControl
    {
        #region Parameter
        
        private DiversityCollection.Import_Step _ImportStep;
        private bool _CanAddSteps;

        private DiversityCollection.FormImportWizard _FormImportWizard;
        private System.DateTime _CollectorsSequence;
        private string _TableAlias;
        private DiversityCollection.Import _Import;

        #endregion

        #region Construction

        public UserControlImportGroup()
        {
            InitializeComponent();
        }

        public UserControlImportGroup(DiversityCollection.Import_Step Import_Step, bool CanAddSteps)
        {
            InitializeComponent();
            this._ImportStep = Import_Step;
            this._CanAddSteps = CanAddSteps;
            this.initUserControl();
        }
        
        #endregion

        #region UserControl

        public DiversityCollection.Import_Step ImportStep
        {
            set
            {
                this._ImportStep = value;
            }
        }

        public bool CanAddSteps
        {
            set
            {
                this._CanAddSteps = value;
            }
        }

        public void initUserControl(DiversityCollection.Import_Step Import_Step, bool CanAddSteps)
        {
            this._CanAddSteps = CanAddSteps;
            this._ImportStep = Import_Step;
            this.initUserControl();
        }

        private void initUserControl()
        {
            try
            {
                this.imageList.Images.Add(this._ImportStep.Image);
                this.tabControlImportSteps.TabPages[0].ImageIndex = 0;
                this.tabControlImportSteps.TabPages[0].Text = this._ImportStep.Title + " 1";
                this.buttonAdd.Visible = this._CanAddSteps;
                this.buttonRemove.Visible = this._CanAddSteps;

                DiversityCollection.UserControls.UserControlImportSelector IS = new UserControlImportSelector(this._ImportStep, "");
                this.panelSelection.Controls.Add(IS);
                IS.Dock = DockStyle.Top;
                IS.BringToFront();
                //foreach (System.Collections.Generic.KeyValuePair<string, DiversityCollection.Import_Step> S in this._ImportStep.DependentImportSteps)
                //{
                //    DiversityCollection.UserControls.UserControlImportSelector UCIS = new UserControlImportSelector(S.Value, "");
                //    this.panelSelection.Controls.Add(UCIS);
                //    UCIS.Dock = DockStyle.Top;
                //    UCIS.BringToFront();
                //}

                //// neither can Items on the same level be added nor are there additional steps, e.g. CollectionSpecimenTransaction
                //if (this._ImportStep.DependentImportSteps.Count == 0 && !_CanAddSteps)
                //    this.splitContainer.Panel1Collapsed = true;

                //// Building the tabs

                //// No item on the same level can be added and there are Dependent steps + steps on the same level, e.g. CollectionEvent
                //if (this._ImportStep.DependentImportSteps.Count > 0 &&
                //    this._ImportStep.DependentImportSteps.Count > 0 &&
                //    !_CanAddSteps)
                //{
                //    // place steps and depenent steps in 1 Tabcontrol
                //}

                //if (this._ImportStep.DependentImportSteps.Count > 0)
                //{
                //}
            }
            catch (System.Exception ex)
            {
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.TabPage TP = new TabPage(this._ImportStep.Title + " " + (this.tabControlImportSteps.TabPages.Count + 1).ToString());
            TP.ImageIndex = 0;
            this.tabControlImportSteps.TabPages.Add(TP);
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if (this.tabControlImportSteps.TabPages.Count == 1 ||
                this.tabControlImportSteps.SelectedTab == this.tabControlImportSteps.TabPages[0])
            {
                System.Windows.Forms.MessageBox.Show("Can not remove this page");
                return;
            }
            System.Windows.Forms.TabPage TP = this.tabControlImportSteps.SelectedTab;
            this.tabControlImportSteps.TabPages.Remove(TP);
        }

        #endregion
    }
}
