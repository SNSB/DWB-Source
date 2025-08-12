using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Spreadsheet
{
    public partial class FormTableSettingsSingle : Form, iTableSettings
    {

        #region Parameter

        private Sheet _Sheet;
        private string _TableAlias;
        private string _HelpNameSpace;
        private bool _OnlyEditing;
        private readonly int _DefaultWidth = 865;
        private readonly int _DefaultHeight = 500;

        #endregion

        #region Construction

        public FormTableSettingsSingle(ref Sheet Sheet, string TableAlias, string HelpNameSpace, bool ForAdding, bool OnlyEditing)
        {
            InitializeComponent();
            try
            {
                this._Sheet = Sheet;
                this._TableAlias = TableAlias;
                this._HelpNameSpace = HelpNameSpace;
                this._OnlyEditing = OnlyEditing;
                this.initForm(ForAdding);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Form

        private void initForm(bool ForAdding)
        {
            try
            {
                if (ForAdding)
                {
                    this.splitContainerMain.Panel2Collapsed = true;
                    this.InitTableSelectors();
                }
                else if (this._TableAlias != null)
                {
                    // Check if parent table is available
                    this.Width = (int)(this._DefaultWidth * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    this.Height = (int)(this._DefaultHeight * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    this.Text = "Edit Table";
                    this.splitContainerMain.Panel1Collapsed = true;
                    if (this._Sheet.DataTables()[this._TableAlias].TableImage() != null)
                    {
                        this.pictureBoxTableImage.Image = this._Sheet.DataTables()[this._TableAlias].TableImage();
                    }
                    UserControlTableSetting UT = new UserControlTableSetting(this._Sheet.DataTables()[this._TableAlias], this, !this._OnlyEditing);
                    this.panelTable.Controls.Add(UT);
                    UT.Dock = DockStyle.Fill;
                    this._TableAlias = UT.TableAlias();
                    if (this._OnlyEditing)
                        this.Text = "Settings for table " + this._Sheet.DataTables()[this._TableAlias].DisplayText;
                    else
                        this.Text = "Settings for new table for " + this._Sheet.DataTables()[this._TableAlias].Description();
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        private void InitTableSelectors()
        {
            try
            {
                this.Text = "Selection of Table";
                int iWidth = this.splitContainerMain.Panel1.Width / 3;
                //System.Windows.Forms.Padding Pa = new Padding(iWidth, 10, iWidth, 10);
                //this.splitContainerMain.Panel1.Padding = Pa;
                System.Collections.Generic.Dictionary<string, string> TablesToAdd = _Sheet.TablesToAdd();
                //this.splitContainerMain.Panel1.Controls.Clear();
                this.panelSelection.SuspendLayout();
                foreach (System.Collections.Generic.KeyValuePair<string, string> KV in TablesToAdd)
                {
                    System.Windows.Forms.Panel P = new Panel();
                    System.Windows.Forms.Button B = new Button();
                    if (this._Sheet.DataTables()[KV.Key].Type() == DataTable.TableType.Lookup)
                        B.BackColor = this._Sheet.DataTables()[KV.Key].paleColor();
                    else
                        B.BackColor = this._Sheet.DataTables()[KV.Key].ColorBack();
                    B.ForeColor = this._Sheet.DataTables()[KV.Key].ColorFont();
                    B.Text = KV.Value; // this._Sheet.DataTables()[KV.Key].DisplayText;
                    B.TextAlign = ContentAlignment.MiddleLeft;
                    this.toolTip.SetToolTip(B, KV.Value); // this._Sheet.DataTables()[KV.Key].DisplayText);
                    B.FlatStyle = FlatStyle.Flat;
                    B.FlatAppearance.BorderSize = 0;
                    toolTip.SetToolTip(B, "Add this table");
                    B.Tag = KV.Key;
                    P.Controls.Add(B);
                    P.BorderStyle = BorderStyle.FixedSingle;
                    B.Dock = DockStyle.Fill;
                    this.panelSelection.Controls.Add(P);//B);
                    P.Dock = DockStyle.Top;
                    P.Height = (int)(24 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    P.BringToFront();
                    if (this._Sheet.DataTables()[KV.Key].TableImage() != null)
                    {
                        B.Image = this._Sheet.DataTables()[KV.Key].TableImage();
                        B.ImageAlign = ContentAlignment.MiddleLeft;
                        B.TextAlign = ContentAlignment.MiddleLeft;
                        B.Text = "      " + KV.Value; // this._Sheet.DataTables()[KV.Key].DisplayText;
                    }
                    else
                    {
                        B.TextAlign = ContentAlignment.MiddleLeft;
                    }
                    SizeF size = B.CreateGraphics().MeasureString(B.Text, B.Font);
                    if (this.panelSelection.Width < (int)size.Width + 10)
                    {
                        this.panelSelection.Width = (int)size.Width + (int)(10 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                        if (this.panelSelection.Width + (int)(40 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor) > this.Width)
                            this.panelSelection.Width = this.Width - (int)(40 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                    }
                    B.Click += new System.EventHandler(this.buttonTableSelector_Click);
                }
                this.Width = this.panelSelection.Width + (int)(40 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
                int H = (TablesToAdd.Count * 24) + 110;
                this.Height = (int)(H * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);// (TablesToAdd.Count * 24) + 110;
                this.panelSelection.ResumeLayout();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonTableSelector_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
                this._TableAlias = B.Tag.ToString();
                this.initForm(false);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        public void AddTable(string TableAlias)
        {
            //try
            //{
            //    DiversityWorkbench.Spreadsheet.FormTableSettingsSingle f = new FormTableSettingsSingle(ref this._Sheet, TableAlias, this._HelpNameSpace, true);
            //    f.ShowDialog();
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
        }

        public void RemoveTable(string TableAlias)
        {
            //try
            //{
            //    foreach (System.Windows.Forms.TabPage TP in this.tabControlTables.TabPages)
            //    {
            //        if (TP.Tag.ToString() == TableAlias)
            //        {
            //            this.tabControlTables.TabPages.Remove(TP);
            //            break;
            //        }
            //    }
            //    this._Sheet.RemoveDataTable(TableAlias);
            //}
            //catch (System.Exception ex)
            //{
            //    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            //}
        }

        public void SetTabName(string TableAlias, string DisplayText)
        {
            //foreach (System.Windows.Forms.TabPage T in this.tabControlTables.TabPages)
            //{
            //    if (T.Tag.ToString() == TableAlias)
            //    {
            //        T.Text = DisplayText;
            //        break;
            //    }
            //}
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        private void FormTableSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if any column was selected
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                bool AnyColumnWasSelected = false;
                foreach (System.Collections.Generic.KeyValuePair<string, DataColumn> DC in this._Sheet.DataTables()[this._TableAlias].DataColumns())
                {
                    if (DC.Value.IsVisible && !DC.Value.IsHidden)
                    {
                        AnyColumnWasSelected = true;
                        break;
                    }
                }
                if (!AnyColumnWasSelected && System.Windows.Forms.MessageBox.Show("No column has been selected. Do you want to select any columns?", "No columns selected", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
            // Disposing controls
            foreach (System.Windows.Forms.Control U in this.panelTable.Controls)
            {
                if (U.GetType() == typeof(UserControlTableSetting))
                {
                    UserControlTableSetting UT = (UserControlTableSetting)U;
                    UT.Dispose();
                }
            }
            // Removing parallel tables that are not selected
            System.Collections.Generic.List<string> ParallelTablesToRemove = new List<string>();
            foreach (System.Collections.Generic.KeyValuePair<string, DataTable> KV in this._Sheet.DataTables())
            {
                if (KV.Value.Type() == DataTable.TableType.Parallel && KV.Value.TemplateAlias != KV.Key)
                {
                    if (KV.Value.DisplayedColumns().Count == 0)
                        ParallelTablesToRemove.Add(KV.Key);
                }
            }
            if (ParallelTablesToRemove.Count > 0)
            {
                foreach (string P in ParallelTablesToRemove)
                    this._Sheet.DataTables().Remove(P);
            }
        }

    }

}
