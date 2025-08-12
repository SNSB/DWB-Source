using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Import
{
    public partial class UserControlStepSelector : UserControl//, iDisposableControl
    {

        #region Parameter
        
        private DiversityWorkbench.Import.Step _Step;
        private DiversityWorkbench.Import.iWizardInterface _iWizard;
        
        #endregion

        #region Construction & Control
        
        public UserControlStepSelector(DiversityWorkbench.Import.Step Step, DiversityWorkbench.Import.iWizardInterface iWizard)
        {
            InitializeComponent();
            this.Step = Step;
            this._iWizard = iWizard;
        }
        
        private void initControl()
        {
            try
            {
                this.pictureBox.Image = this.Step.Image;
                this.checkBox.Text = this.Step.DisplayText;
                this.checkBox.Checked = this.Step.IsSelected;
                this.panelIndent.Width = this.Step.Indent;
                if (!this.Step.CanCreateCopiesOfItself)
                {
                    this.buttonAdd.Visible = false;
                    this.buttonRemove.Visible = false;
                }
                else
                {
                    if (DiversityWorkbench.Import.Import.Tables[this.Step.DataTableTemplate.TableAlias].TypeOfParallelity == DataTable.Parallelity.referencing)
                    {
                        if (DiversityWorkbench.Import.Import.Tables[this.Step.DataTableTemplate.TableAlias].ParallelPositionForReferencingTable == 1)
                        {
                            this.buttonRemove.Visible = false;
                            this.buttonAdd.Visible = true;
                        }
                        else
                        {
                            DiversityWorkbench.Import.Import.Tables[this.Step.DataTableTemplate.TableAlias].setParallelPositionForReferencingTable();
                            //bool IsLastPosition = false;
                            string ParentAlias = DiversityWorkbench.Import.Import.Tables[this.Step.DataTableTemplate.TableAlias].ParentTableAlias;
                            int LastPosition = 0;
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                            {
                                if (KV.Value.ParentTableAlias == ParentAlias && KV.Value.TableName == this._Step.DataTableTemplate.TableName)
                                {
                                    if (LastPosition < KV.Value.ParallelPosition)
                                        LastPosition = KV.Value.ParallelPosition;
                                }
                            }
                            if (DiversityWorkbench.Import.Import.Tables[this.Step.DataTableTemplate.TableAlias].ParallelPosition == LastPosition)
                            {
                                this.buttonRemove.Visible = true;
                                this.buttonAdd.Visible = false;
                            }
                            else
                            {
                                this.buttonRemove.Visible = false;
                                this.buttonAdd.Visible = false;
                            }
                        }
                    }
                    else
                    {
                        if (DiversityWorkbench.Import.Import.Tables[this.Step.DataTableTemplate.TableAlias].ParallelPosition == 1)
                        {
                            this.buttonRemove.Visible = false;
                            this.buttonAdd.Visible = true;
                        }
                        else
                        {
                            //bool IsLastPosition = false;
                            string ParentAlias = DiversityWorkbench.Import.Import.Tables[this.Step.DataTableTemplate.TableAlias].ParentTableAlias;
                            int LastPosition = 0;
                            foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                            {
                                if (KV.Value.ParentTableAlias == ParentAlias && KV.Value.TableName == this._Step.DataTableTemplate.TableName)
                                {
                                    if (LastPosition < KV.Value.ParallelPosition)
                                        LastPosition = KV.Value.ParallelPosition;
                                }
                            }
                            if (DiversityWorkbench.Import.Import.Tables[this.Step.DataTableTemplate.TableAlias].ParallelPosition == LastPosition)
                            {
                                this.buttonRemove.Visible = true;
                                this.buttonAdd.Visible = false;
                            }
                            else
                            {
                                this.buttonRemove.Visible = false;
                                this.buttonAdd.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            this.setInterface();
        }

        private void setInterface()
        {
        }

        //public void DisposeComponents()
        //{
        //    this.toolTip.Dispose();
        //}

        #endregion

        #region Interface
        
        public DiversityWorkbench.Import.Step Step
        {
            get
            {
                return this._Step;
            }
            set
            {
                this._Step = value;
                this.initControl();
            }
        }
        
        #endregion

        #region Events
        
        /// <summary>
        /// Adding a copy of the current step
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this._Step.IsSelected)
                {
                    System.Windows.Forms.MessageBox.Show("Only selected steps can be copied");
                    return;
                }
                this._Step.CopyStep();
                this._iWizard.RefreshStepSelectionPanel();

            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// removing this copy of the first step
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRemove_Click(object sender, EventArgs e)
        {
            try
            {
                bool IsSelected = this._Step.IsSelected;
                string StepKey = this._Step.PositionKey;
                string TableAlias = this._Step.TableAlias;
                this._Step.RemoveStepChilds();
                DiversityWorkbench.Import.Import.Steps.Remove(StepKey);
                DiversityWorkbench.Import.Import.TableListForImport.Remove(TableAlias);
                DiversityWorkbench.Import.Import.Tables.Remove(TableAlias);
                this._iWizard.RefreshStepSelectionPanel();
                if (IsSelected)
                {
                    this._iWizard.RefreshStepPanel();
                    this._iWizard.SetGridHeaders();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        /// <summary>
        /// changing the visiblity of a step
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Import.Import.Steps[this.Step.PositionKey].IsSelected = this.checkBox.Checked;
                if (DiversityWorkbench.Import.Import.Steps[this.Step.PositionKey].IsSelected)//.checkBox.Checked)
                {
                    if (DiversityWorkbench.Import.Import.Steps[this.Step.PositionKey].IsSelected && !this.checkBox.Checked)
                    {
                        System.Windows.Forms.MessageBox.Show("This step can not be deselected");
                        this.checkBox.Checked = true;
                    }
                    if ((bool)this._Step.DataTableTemplate.DependsOnParentTable)
                    {
                        try
                        {
                            DiversityWorkbench.Import.Import.Steps[DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.Tables[this._Step.TableAlias].ParentTableAlias].PositionKey].IsSelected = true;


                            string ParentTable = DiversityWorkbench.Import.Import.Tables[this._Step.TableAlias].ParentTableAlias;
                            if ((bool)DiversityWorkbench.Import.Import.Steps[DiversityWorkbench.Import.Import.Tables[ParentTable].PositionKey].DataTableTemplate.DependsOnParentTable)
                            {
                                ParentTable = DiversityWorkbench.Import.Import.Tables[ParentTable].ParentTableAlias;
                                DiversityWorkbench.Import.Import.Steps[DiversityWorkbench.Import.Import.Tables[ParentTable].PositionKey].IsSelected = true;
                                if ((bool)DiversityWorkbench.Import.Import.Steps[DiversityWorkbench.Import.Import.Tables[ParentTable].PositionKey].DataTableTemplate.DependsOnParentTable)
                                {
                                    ParentTable = DiversityWorkbench.Import.Import.Tables[ParentTable].ParentTableAlias;
                                    DiversityWorkbench.Import.Import.Steps[DiversityWorkbench.Import.Import.Tables[ParentTable].PositionKey].IsSelected = true;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                        }
                        this._iWizard.RefreshStepSelectionPanel();
                    }
                }
                else
                {
                    DiversityWorkbench.Import.Import.TableListForImport = null;
                    foreach (string T in DiversityWorkbench.Import.Import.TableListForImport)
                    {
                        if (DiversityWorkbench.Import.Import.Tables[T].ParentTableAlias == this._Step.TableAlias &&
                            (bool)DiversityWorkbench.Import.Import.Tables[T].DependsOnParentTable)
                        {
                            System.Windows.Forms.MessageBox.Show(this._Step.DisplayText + " can not be deselected because " + DiversityWorkbench.Import.Import.Tables[T].DisplayText + " depends upon it");
                            DiversityWorkbench.Import.Import.Steps[this.Step.PositionKey].IsSelected = true;
                            this.checkBox.Checked = true;
                            this._iWizard.RefreshStepSelectionPanel();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }

            //this.setInterface();
            DiversityWorkbench.Import.Import.TableListForImport = null;
            this._iWizard.RefreshStepPanel();
        }
        
        #endregion

    }
}
