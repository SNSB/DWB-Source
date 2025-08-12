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
    public partial class UserControlDataColumnMulti : UserControl//, iDisposableControl
    {

        #region Parameter

        private DiversityWorkbench.Import.ColumnMulti _ColumnMulti;
        private DiversityWorkbench.Import.iDataColumnInterface _iDataColumn;
        private DiversityWorkbench.Import.iWizardInterface _iWizardInterface;

        #endregion

        #region Construction & control

        public UserControlDataColumnMulti(DiversityWorkbench.Import.ColumnMulti ColumnMulti,
            DiversityWorkbench.Import.iDataColumnInterface iDataColumn,
            DiversityWorkbench.Import.iWizardInterface iWizardInterface,
            string HelpNameSpace)
        {
            InitializeComponent();
            this._ColumnMulti = ColumnMulti;
            this._iDataColumn = iDataColumn;
            this._iWizardInterface = iWizardInterface;
            this.initControl();
            this.helpProvider.HelpNamespace = HelpNameSpace;
        }

        private void initControl()
        {
            this.SuspendLayout();
            this.textBoxPrefix.Text = this._ColumnMulti.Prefix;
            this.textBoxPostfix.Text = this._ColumnMulti.Postfix;
            this.buttonColumnInSourceFile.Text = this._ColumnMulti.ColumnInFile.ToString();
            // Transformation
            if (this._ColumnMulti.Transformations.Count > 0)
                this.buttonTranslation.BackColor = System.Drawing.Color.Red;
            else this.buttonTranslation.BackColor = System.Drawing.SystemColors.Control;
            // decision
            if (this._ColumnMulti.IsDecisive)
            {
                this.ForeColor = System.Drawing.Color.Green;
                this.pictureBoxDecision.Image = this.imageListDecision.Images[0];
            }
            else
            {
                this.ForeColor = System.Drawing.Color.Black;
                this.pictureBoxDecision.Image = this.imageListDecision.Images[1];
            }
            // Copy previous
            if (this._ColumnMulti.CopyPrevious)
            {
                this.pictureBoxCopyPrevious.Image = this.imageListCopyLine.Images[0];
                this.pictureBoxCopyPrevious.BackColor = System.Drawing.Color.White;
            }
            else
            {
                this.pictureBoxCopyPrevious.Image = this.imageListCopyLine.Images[1];
                this.pictureBoxCopyPrevious.BackColor = System.Drawing.SystemColors.Control;
            }
            this.ResumeLayout();
        }

        //public void DisposeComponents()
        //{
        //    this.toolTip.Dispose();
        //    this.imageListCopyLine.Dispose();
        //    this.imageListDecision.Dispose();
        //    this.helpProvider.Dispose();
        //}

        #endregion

        #region Events

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            this._ColumnMulti.DataColumn.MultiColumns.Remove(this._ColumnMulti);
            this._iDataColumn.initControl();
        }

        private void buttonTranslation_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Import.FormTransformation f = new FormTransformation(this._ColumnMulti.Transformations, this._ColumnMulti.Prefix, this._ColumnMulti.Postfix, this._ColumnMulti, this._ColumnMulti.ColumnInFile, this._iWizardInterface, this.helpProvider.HelpNamespace);
            f.ShowDialog();
            this.initControl();
        }

        private void pictureBoxCopyPrevious_Click(object sender, EventArgs e)
        {
            this._ColumnMulti.CopyPrevious = !this._ColumnMulti.CopyPrevious;
            this.initControl();
        }

        private void pictureBoxDecision_Click(object sender, EventArgs e)
        {
            this._ColumnMulti.IsDecisive = !this._ColumnMulti.IsDecisive;
            this.initControl();
        }

        private void buttonColumnInSourceFile_Click(object sender, EventArgs e)
        {
            try
            {
                DiversityWorkbench.Import.FormFileDataGrid f = new FormFileDataGrid(this.DataGridCopy(50), this._ColumnMulti.DataColumn, (int?)this._ColumnMulti.ColumnInFile);
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && f.SelectedFileColumn() != null)
                {
                    int i = (int)f.SelectedFileColumn();
                    this._ColumnMulti.ColumnInFile = i;
                    this.initControl();
                    this._iWizardInterface.SetGridHeaders();
                }
            }
            catch (System.Exception ex) { }
        }

        private void buttonColumnInSourceFile_MouseEnter(object sender, EventArgs e)
        {
            try
            {
                int Position = this._ColumnMulti.ColumnInFile;
                this._iWizardInterface.DataGridView().FirstDisplayedScrollingColumnIndex = Position;
            }
            catch (System.Exception ex) { }
        }

        private void textBoxPrefix_TextChanged(object sender, EventArgs e)
        {
            this._ColumnMulti.Prefix = this.textBoxPrefix.Text;
        }

        private void textBoxPostfix_TextChanged(object sender, EventArgs e)
        {
            this._ColumnMulti.Postfix = this.textBoxPostfix.Text;
        }

        private void textBoxPrefix_DoubleClick(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Prefix", this.textBoxPrefix.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxPrefix.Text = f.EditedText;
        }

        private void textBoxPostfix_DoubleClick(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormEditText f = new DiversityWorkbench.Forms.FormEditText("Prefix", this.textBoxPostfix.Text);
            f.ShowDialog();
            if (f.DialogResult == DialogResult.OK)
                this.textBoxPostfix.Text = f.EditedText;
        }

        #endregion

        #region Grid

        /// <summary>
        /// Returns a copy of the datagrid containing the first lines of the original
        /// </summary>
        /// <param name="FristLines">Number of lines starting at the top</param>
        /// <returns></returns>
        private System.Windows.Forms.DataGridView DataGridCopy(int FristLines)
        {
            System.Windows.Forms.DataGridView G = new DataGridView();
            G.ReadOnly = true;
            foreach (System.Windows.Forms.DataGridViewColumn C in this._iWizardInterface.DataGridView().Columns)
            {
                System.Windows.Forms.DataGridViewColumn Col = new DataGridViewColumn(C.CellTemplate);
                Col.HeaderText = C.HeaderText;
                G.Columns.Add(Col);
            }
            for (int i = 0; i < this._iWizardInterface.DataGridView().Rows.Count && i < FristLines; i++)
            {
                System.Windows.Forms.DataGridViewRow Row = new DataGridViewRow();
                G.Rows.Add(Row);
                for (int ii = 0; ii < this._iWizardInterface.DataGridView().Columns.Count; ii++)
                {
                    G.Rows[i].Cells[ii].Value = this._iWizardInterface.DataGridView().Rows[i].Cells[ii].Value;
                    G.Rows[i].Cells[ii].Style = this._iWizardInterface.DataGridView().Rows[i].Cells[ii].Style;
                }
            }
            return G;
        }

        #endregion

    }
}
