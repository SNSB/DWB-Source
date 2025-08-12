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
    public partial class UserControlStep : UserControl, iWizardStep//, iDisposableControl
    {
        #region Parameter

        private DiversityWorkbench.Import.iWizardInterface _iWizardInterface;
        private DiversityWorkbench.Import.Step _Step;
        private DiversityWorkbench.Import.StepColumnGroup _ColumnGroup;

        #endregion

        #region Construction & Control

        public UserControlStep(DiversityWorkbench.Import.Step Step, DiversityWorkbench.Import.iWizardInterface iInterface, string HelpNameSpace)
        {
            InitializeComponent();
            this._iWizardInterface = iInterface;
            this._Step = Step;
            if (this._Step.TypeOfStep != DiversityWorkbench.Import.Step.StepType.Table)
            {
                this.labelStep.ForeColor = System.Drawing.Color.Blue;
            }
            this.pictureBoxStep.Image = Step.Image;
            this.labelStep.Text = Step.DisplayText;
            this.labelMessage.Text = "";
            this.panelIndent.Width = Step.Indent;
            this.AddColumnGroups();
            this.helpProvider.HelpNamespace = HelpNameSpace;
        }

        public UserControlStep(DiversityWorkbench.Import.Step Step, DiversityWorkbench.Import.StepColumnGroup ColumnGroup, DiversityWorkbench.Import.iWizardInterface iInterface, string HelpNameSpace)
        {
            InitializeComponent();
            this._iWizardInterface = iInterface;
            this._Step = Step;
            this._ColumnGroup = ColumnGroup;
            this.labelMessage.Text = "";
            this.panelIndent.Width = Step.Indent + Step.IndentSize * 2;
            this.pictureBoxStep.Image = ColumnGroup.Image;
            this.labelStep.Text = ColumnGroup.DisplayText;
            this.panelSpacer.Height = 0;
            this.Height = 18;
            this.helpProvider.HelpNamespace = HelpNameSpace;
        }

        private void initUserControl()
        {
        }

        //public void DisposeComponents()
        //{
        //    this.helpProvider.Dispose();
        //}

        #endregion

        #region Events
        
        private void AddColumnGroups()
        {
            if (this._Step.StepColumnGroups != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.StepColumnGroup> KV in this._Step.StepColumnGroups)
                {
                    DiversityWorkbench.Import.UserControlStep ucS = new UserControlStep(this._Step, KV.Value, this._iWizardInterface, this.helpProvider.HelpNamespace);
                }
            }
        }

        private void pictureBoxStep_Click(object sender, EventArgs e)
        {
            this.SetCurrentStep();
        }

        private void labelStep_Click(object sender, EventArgs e)
        {
            this.SetCurrentStep();
        }

        private void labelMessage_Click(object sender, EventArgs e)
        {

        }

        private void SetCurrentStep() //bool IsCurrent
        {
            DiversityWorkbench.Import.Import.CurrentStepKey = this._Step.PositionKey;
            foreach (System.Windows.Forms.Control C in this._iWizardInterface.DataColumnPanel().Controls)
            {
                C.Dispose();
            }

            this._iWizardInterface.DataColumnPanel().Controls.Clear();
            try
            {
                switch (this._Step.TypeOfStep)
                {
                    case Step.StepType.File:
                        this._iWizardInterface.setDataHeader(this._Step);
                        DiversityWorkbench.Import.UserControlFile Ufile = new UserControlFile(this._iWizardInterface);
                        Ufile.Dock = DockStyle.Fill;
                        this._iWizardInterface.DataColumnPanel().Controls.Add(Ufile);
                        break;
                    case Step.StepType.Attachment:
                        this._iWizardInterface.setDataHeader(this._Step);
                        if (!this._iWizardInterface.FileIsOK())
                        {
                            System.Windows.Forms.MessageBox.Show("Please select a file");
                            return;
                        }
                        System.Collections.Generic.Stack<DiversityWorkbench.Import.DataTable> AttachTableStack = new Stack<DiversityWorkbench.Import.DataTable>();

                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                        {
                            if (DiversityWorkbench.Import.Import.TableListForImport.Contains(KV.Value.TableAlias))
                                AttachTableStack.Push(KV.Value);
                        }
                        bool ShowAttachmentUseTransformation = false;
                        while (AttachTableStack.Count > 0)
                        {
                            DiversityWorkbench.Import.DataTable DT = AttachTableStack.Pop();
                            if (DT.AttachmentColumns != null
                                && DT.AttachmentColumns.Count > 0)
                            {
                                bool AttachmentColumnPresent = false;
                                foreach (string A in DT.AttachmentColumns)
                                {
                                    if (A.Length == 0)
                                        continue;
                                    System.Windows.Forms.RadioButton RB = new RadioButton();
                                    RB.Text = DT.DataColumns[A].DisplayText;
                                    RB.Dock = DockStyle.Top;
                                    RB.Tag = DT.DataColumns[A];
                                    if (DiversityWorkbench.Import.Import.AttachmentColumn == DT.DataColumns[A])
                                    {
                                        RB.BackColor = DiversityWorkbench.Import.Import.ColorAttachment;
                                        RB.Checked = true;
                                    }
                                    RB.Click += this.setAttachmentColumn;
                                    this._iWizardInterface.DataColumnPanel().Controls.Add(RB);
                                    AttachmentColumnPresent = true;
                                }
                                if (AttachmentColumnPresent)
                                {
                                    System.Windows.Forms.Panel P = new Panel();
                                    P.Height = 20;
                                    P.Dock = DockStyle.Top;
                                    P.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
                                    System.Windows.Forms.Label L = new Label();
                                    L.Text = DT.GetDisplayText();
                                    L.Dock = DockStyle.Left;
                                    L.TextAlign = ContentAlignment.MiddleLeft;
                                    P.Controls.Add(L);
                                    System.Windows.Forms.PictureBox Pi = new PictureBox();
                                    Pi.Width = 16;
                                    Pi.Height = 16;
                                    Pi.Dock = DockStyle.Left;
                                    Pi.Image = DT.Image;
                                    P.Controls.Add(Pi);
                                    this._iWizardInterface.DataColumnPanel().Controls.Add(P);
                                    ShowAttachmentUseTransformation = true;
                                }
                            }
                        }
                        System.Windows.Forms.Panel PSpacer = new Panel();
                        PSpacer.Height = 10;
                        PSpacer.Dock = DockStyle.Top;
                        this._iWizardInterface.DataColumnPanel().Controls.Add(PSpacer);
                        System.Windows.Forms.RadioButton RBasNew = new RadioButton();
                        if (DiversityWorkbench.Import.Import.AttachmentColumn == null)
                            RBasNew.Checked = true;
                        RBasNew.Text = "Import as new data";
                        this.toolTip.SetToolTip(RBasNew, "Import data without a relation to existing data in the database");
                        RBasNew.Dock = DockStyle.Top;
                        RBasNew.Margin = new Padding(4);
                        RBasNew.Click += this.setAttachmentColumn;
                        this._iWizardInterface.DataColumnPanel().Controls.Add(RBasNew);
                        if (ShowAttachmentUseTransformation)
                        {
                            // Spacer
                            System.Windows.Forms.Panel PSp = new Panel();
                            PSp.Height = 10;
                            PSp.Dock = DockStyle.Top;
                            this._iWizardInterface.DataColumnPanel().Controls.Add(PSp);
                            //Checkbox
                            System.Windows.Forms.CheckBox CBUseTrans = new CheckBox();
                            CBUseTrans.Checked = DiversityWorkbench.Import.Import.AttachmentUseTransformation;
                            CBUseTrans.Text = "Use transformation for attachment value";
                            this.toolTip.SetToolTip(CBUseTrans, "By default the transformation of the attachment value is not used for the comparision with the content in the database.\r\nSelect this option to use the transformed value");
                            CBUseTrans.Dock = DockStyle.Top;
                            CBUseTrans.Margin = new Padding(4);
                            CBUseTrans.Click += this.setAttachmentUseTransformation;
                            this._iWizardInterface.DataColumnPanel().Controls.Add(CBUseTrans);
                        }
                        break;
                    case Step.StepType.Merging:
                        this._iWizardInterface.setDataHeader(this._Step);
                        System.Collections.Generic.Stack<DiversityWorkbench.Import.DataTable> TableStack = new Stack<DiversityWorkbench.Import.DataTable>();
                        foreach (System.Collections.Generic.KeyValuePair<string, DiversityWorkbench.Import.DataTable> KV in DiversityWorkbench.Import.Import.Tables)
                        {
                            //if (DiversityWorkbench.Import.Import.SelectedTableAliases.Contains(KV.Value.TableAlias))
                            //    TableStack.Push(KV.Value);
                            if (DiversityWorkbench.Import.Import.TableListForImport.Contains(KV.Value.TableAlias))
                                TableStack.Push(KV.Value);
                        }
                        while (TableStack.Count > 0)
                        {
                            DiversityWorkbench.Import.UserControlDataTable U = new UserControlDataTable(TableStack.Pop());
                            //string TooltipMessage = "Merging options for the table: Import = Data will be imported as new data." +
                            //    "\r\nUpdate = Update if corresponding data are detected in the database." +
                            //    "\r\nMerge = If corresponding data are detected in the database these will be updated, otherwise the data will be imported as new data." +
                            //    "\r\nAttach = Use this table as a anchor to import data in relation to existing data in the database";
                            //this.toolTip.SetToolTip(U, TooltipMessage);
                            U.Dock = DockStyle.Top;
                            this._iWizardInterface.DataColumnPanel().Controls.Add(U);
                        }
                        break;
                    case Step.StepType.Table:
                        if (!this._iWizardInterface.FileIsOK())
                        {
                            System.Windows.Forms.MessageBox.Show("Please select a file");
                            return;
                        }
                        System.Collections.Generic.Stack<string> ColumnStack = new Stack<string>();
                        if (this._Step.DataTableTemplate.IsForAttachment)
                        {
                            foreach (string s in this._Step.DataTableTemplate.AttachmentColumns)
                            {
                                if (DiversityWorkbench.Import.Import.AttachmentColumn != null &&
                                    DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName == s)
                                    ColumnStack.Push(s);
                            }
                            if (ColumnStack.Count == 0)
                                System.Windows.Forms.MessageBox.Show(this._Step.DisplayText + " is only used for attachment");
                            this._iWizardInterface.setDataHeader(this._Step);
                        }
                        else
                        {
                            if (this._ColumnGroup != null && this._ColumnGroup.Columns.Count > 0)
                            {
                                foreach (string s in this._ColumnGroup.Columns)
                                {
                                    ColumnStack.Push(s);
                                }
                                this._iWizardInterface.setDataHeader(this._Step, this._ColumnGroup);
                            }
                            else
                            {
                                foreach (string s in this._Step.ColumnsOutsideGroups)
                                {
                                    ColumnStack.Push(s);
                                }
                                this._iWizardInterface.setDataHeader(this._Step);
                            }
                        }
                        while (ColumnStack.Count > 0)
                        {
                            string Column = ColumnStack.Pop();
                            // if a column is not contained in the table
                            if (!DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns.ContainsKey(Column))
                                continue;

                            // if a value is preset
                            if (DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].PresetValues.ContainsKey(Column) && DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].PresetValues[Column].Length > 0)
                                continue;

                            // if a column is an identity column and not used for attachment
                            if (DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].IsIdentity &&
                                !(DiversityWorkbench.Import.Import.AttachmentColumn != null &&
                                DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].TableAlias &&
                                DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName == Column))
                                continue;

                            // if a column is an identity column in the parent table and not used for attachment
                            if (DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].IsIdentityInParent &&
                                !(DiversityWorkbench.Import.Import.AttachmentColumn != null &&
                                DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].TableAlias &&
                                DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName == Column))
                                continue;

                            // if data are not retieved via a function, a foreign relation is given and there is no need to choose between the source of the relation
                            if (DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].ForeignRelationTable != null
                                && DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].ForeignRelationTableAlias != null
                                && DiversityWorkbench.Import.Import.Tables.ContainsKey(DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].ForeignRelationTableAlias)
                                && !DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].SelectParallelForeignRelationTableAlias
                                && !DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].PrepareInsertDefined
                                && DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].DataRetrievalType == DataColumn.RetrievalType.Default)
                                continue;

                            // if it is the first parallel table and a Parent column is given and the content should not be retrieved via ID
                            if (DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].ParentColumn != null
                                && DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].ParentColumn.Length > 0
                                && DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].ParallelPosition == 1
                                && DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[Column].DataRetrievalType != DataColumn.RetrievalType.IDorIDviaTextFromFile)
                                continue;

                            DiversityWorkbench.Import.UserControlDataColumn U = new UserControlDataColumn(DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias], Column, this._iWizardInterface, this.helpProvider.HelpNamespace);
                            U.Dock = DockStyle.Top;
                            this._iWizardInterface.DataColumnPanel().Controls.Add(U);
                        }
                        if (!this._Step.DataTableTemplate.IsForAttachment &&
                            DiversityWorkbench.Import.Import.AttachmentColumn != null &&
                            DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias == DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].TableAlias &&
                            DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].AttachViaParentChildRelation())
                        {
                            DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].DataColumns[DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].AttachmentParentColumn()].IsSelected = true;
                            DiversityWorkbench.Import.UserControlDataColumn UAttach = new UserControlDataColumn(DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].TableAlias], DiversityWorkbench.Import.Import.Tables[this._Step.DataTableTemplate.TableAlias].AttachmentParentColumn(), this._iWizardInterface, this.helpProvider.HelpNamespace);
                            UAttach.setParentAttachmentColumn();
                            UAttach.Dock = DockStyle.Top;
                            this._iWizardInterface.DataColumnPanel().Controls.Add(UAttach);
                        }
                        break;
                    case Step.StepType.Testing:
                        this._iWizardInterface.setDataHeader(this._Step);
                        DiversityWorkbench.Import.UserControlTesting UAna = new UserControlTesting(this._iWizardInterface);
                        UAna.Dock = DockStyle.Fill;
                        this._iWizardInterface.DataColumnPanel().Controls.Add(UAna);
                        break;
                    case Step.StepType.Import:
                        this._iWizardInterface.setDataHeader(this._Step);
                        DiversityWorkbench.Import.UserControlImport Uimp = new UserControlImport(this._iWizardInterface);
                        Uimp.Dock = DockStyle.Fill;
                        this._iWizardInterface.DataColumnPanel().Controls.Add(Uimp);
                        break;
                }
                if (this._Step.TypeOfStep != Step.StepType.Table)
                    this._iWizardInterface.setTableMessage(null);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setAttachmentColumn(object sender, EventArgs e)
        {
            System.Windows.Forms.RadioButton RB = (System.Windows.Forms.RadioButton)sender;
            if (DiversityWorkbench.Import.Import.AttachmentColumn != null)
            {
                DiversityWorkbench.Import.DataColumn DCurrent = (DiversityWorkbench.Import.DataColumn)RB.Tag;
                if (RB.Tag == null ||
                    DCurrent.ColumnName != DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName ||
                    DCurrent.DataTable.TableAlias != DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias)
                {
                    if (DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias].DataColumns[DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName].FileColumn == null)
                    {
                        DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias].DataColumns[DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName].IsDecisive = false;
                        DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias].DataColumns[DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName].IsSelected = false;
                        DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias].DataColumns[DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName].CompareKey = false;
                        DiversityWorkbench.Import.Import.Tables[DiversityWorkbench.Import.Import.AttachmentColumn.DataTable.TableAlias].DataColumns[DiversityWorkbench.Import.Import.AttachmentColumn.ColumnName].TypeOfSource = DataColumn.SourceType.NotDecided;
                    }
                }
            }
            if (RB.Tag == null)
            {
                DiversityWorkbench.Import.Import.AttachmentColumn = null;
            }
            else
            {
                DiversityWorkbench.Import.DataColumn DC = (DiversityWorkbench.Import.DataColumn)RB.Tag;
                DiversityWorkbench.Import.Import.AttachmentColumn = DC;
            }
        }

        private void setAttachmentUseTransformation(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.CheckBox CB = (System.Windows.Forms.CheckBox)sender;
                DiversityWorkbench.Import.Import.AttachmentUseTransformation = CB.Checked;
            }
            catch(System.Exception ex) { ExceptionHandling.WriteToErrorLogFile(ex); }
        }


        #endregion

    }
}
