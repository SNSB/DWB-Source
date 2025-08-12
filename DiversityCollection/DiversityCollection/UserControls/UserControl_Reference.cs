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
    public partial class UserControl_Reference : UserControl__Data
    {
        #region Construction
        public UserControl_Reference(
            iMainForm MainForm,
            System.Windows.Forms.BindingSource Source,
            string HelpNamespace)
            : base(MainForm, Source, HelpNamespace)
        {
            InitializeComponent();
            this.initControl();
            this.FormFunctions.addEditOnDoubleClickToTextboxes();
            this.FormFunctions.setDescriptions(this);
        }

        #endregion

        #region Control

        private void initControl()
        {
            this.textBoxReferenceNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "Notes", true));
            this.textBoxReferenceDetails.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._Source, "ReferenceDetails", true));

            DiversityWorkbench.Reference R = new DiversityWorkbench.Reference(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryReference, R, "CollectionSpecimenReference", "ReferenceTitle", "ReferenceURI", this._Source);

            DiversityWorkbench.Agent A = new DiversityWorkbench.Agent(DiversityWorkbench.Settings.ServerConnection);
            this.inituserControlModuleRelatedEntry(ref this.userControlModuleRelatedEntryReferenceResponsible, A, "CollectionSpecimenReference", "ResponsibleName", "ResponsibleAgentURI", this._Source);

            DiversityWorkbench.Forms.FormFunctions.setAutoCompletion(this, true);

            DiversityWorkbench.Entity.setEntity(this, this.toolTip);

            this.CheckIfClientIsUpToDate();
        }

        #endregion

        #region Interface

        public override void SetPosition(int Position)
        {
            base.SetPosition(Position);
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryReference, "ReferenceURI");
            this.setUserControlSourceFixing(ref this.userControlModuleRelatedEntryReferenceResponsible, "ResponsibleAgentURI");
        }

        #endregion
    }
}
