using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.XslEditor
{
    public partial class FormXslTableEditor : Form
    {

        #region Parameter

        private DiversityWorkbench.XslEditor.XslTable _Table;
       // private DiversityWorkbench.XslEditor.XslEditor _Editor;
        private System.Collections.Generic.List<DiversityWorkbench.XslEditor.UserControlXslTableColumn> _ColumnControls;
        private System.Collections.Generic.List<DiversityWorkbench.XslEditor.UserControlXslTableRow> _RowControls;
        private DiversityWorkbench.XslEditor.XslNode _XslNode;

        #endregion

        #region Construction
        
        public FormXslTableEditor(DiversityWorkbench.XslEditor.XslNode XslNode)
        {
            InitializeComponent();
            this._XslNode = XslNode;
            this.DrawTableColumnsAndRows();
        }
        
        #endregion

        #region Public properties

        public System.Collections.Generic.List<DiversityWorkbench.XslEditor.UserControlXslTableColumn> ColumnControls
        {
            get
            {
                if (this._ColumnControls == null)
                    this._ColumnControls = new List<DiversityWorkbench.XslEditor.UserControlXslTableColumn>();
                return _ColumnControls;
            }
        }

        public System.Collections.Generic.List<DiversityWorkbench.XslEditor.UserControlXslTableRow> RowControls
        {
            get
            {
                if (this._RowControls == null)
                    this._RowControls = new List<DiversityWorkbench.XslEditor.UserControlXslTableRow>();
                return _RowControls;
            }
        }

        public DiversityWorkbench.XslEditor.XslTable Table
        {
            get
            {
                if (this._Table == null)
                    this._Table = new DiversityWorkbench.XslEditor.XslTable(this._XslNode);
                return this._Table;
            }
        }

        #endregion

        #region Zoom

        public double ZoomFactor
        {
            get
            {
                double Zoom = 1;
                if (double.TryParse(this.numericUpDownZoom.Value.ToString(), out Zoom))
                    return Zoom / 100;
                return 1;
            }
        }

        private int Zoom(int Value)
        {
            int i = Value;
            i = (int)Math.Round((i * ZoomFactor), 0);
            return i;
        }
        
        private void numericUpDownZoom_ValueChanged(object sender, EventArgs e)
        {
            foreach (DiversityWorkbench.XslEditor.UserControlXslTableColumn C in this.ColumnControls)
                C.ZoomFactor = this.ZoomFactor;
            foreach (DiversityWorkbench.XslEditor.UserControlXslTableRow R in this.RowControls)
                R.ZoomFactor = this.ZoomFactor;
            this.DrawTableColumnsAndRows();
        }

        #endregion

        #region Rows & Columns

        private int TableWidth
        {
            get
            {
                return this.Zoom(this.Table.TableWidth);
            }
        }

        private int TableHeight
        {
            get
            {
                return this.Zoom(this.Table.TableHeight);
            }
        }

        private void toolStripButtonRowAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslTableRow LR = this._Table.GetTableRow();// new DiversityWorkbench.XslEditor.XslTableRow(N);
            this.DrawTableColumnsAndRows();
        }

        private void toolStripButtonColumnAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XslEditor.XslTableColumn LC = this._Table.GetTableColumn();// new DiversityWorkbench.XslEditor.XslTableColumn(N);
            this.DrawTableColumnsAndRows();
        }

        private void toolStripButtonColumnRemove_Click(object sender, EventArgs e)
        {
            this.panelColumns.Controls.Remove(this.ColumnControls[0]);
            this.ColumnControls.RemoveAt(0);
            this.Table.TableColums.RemoveAt(0);
            this.DrawTableColumnsAndRows();
        }

        private void toolStripButtonRowRemove_Click(object sender, EventArgs e)
        {
            this.panelRows.Controls.Remove(this.RowControls[0]);
            this.RowControls.RemoveAt(0);
            this.Table.TableRows.RemoveAt(0);
            this.DrawTableColumnsAndRows();
        }

        private void DrawTableColumnsAndRows()
        {
            this.tableLayoutPanelRowColumnEditor.Controls.Clear();

            // ColumnControls
            this.tableLayoutPanelRowColumnEditor.ColumnStyles.Clear();
            this.tableLayoutPanelRowColumnEditor.ColumnCount = this.Table.ColumnCount;
            this.tableLayoutPanelRowColumnEditor.Width = this.TableWidth;
            this.panelColumns.Controls.Clear();
            this.ColumnControls.Clear();
            for (int i = 0; i < this.Table.TableColums.Count; i++)
            {
                System.Single S = (System.Single)this.Zoom(this.Table.TableColums[i].ColumnWidth);
                this.tableLayoutPanelRowColumnEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, S));
            }
            for (int i = this.Table.TableColums.Count - 1; i > -1; i--)
            {
                DiversityWorkbench.XslEditor.XslTableColumn LC = this.Table.TableColums[i];
                DiversityWorkbench.XslEditor.UserControlXslTableColumn U = new DiversityWorkbench.XslEditor.UserControlXslTableColumn(ref LC);
                U.ZoomFactor = this.ZoomFactor;
                this.panelColumns.Controls.Add(U);
                this.ColumnControls.Add(U);
                U.Dock = DockStyle.Left;
            }

            // Row controls
            this.tableLayoutPanelRowColumnEditor.RowStyles.Clear();
            this.tableLayoutPanelRowColumnEditor.RowCount = this.Table.RowCount;
            this.tableLayoutPanelRowColumnEditor.Height = this.TableHeight;
            this.panelRows.Controls.Clear();
            this.RowControls.Clear();
            for (int i = 0; i < this.Table.TableRows.Count; i++)
            {
                System.Single S = (System.Single)this.Zoom(this.Table.TableRows[i].RowHeight);
                this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, S));
            }
            int Offset = 0;
            for (int i = this.Table.TableRows.Count - 1; i > -1; i--)
            {
                System.Single S = (System.Single)this.Table.TableRows[i].RowHeight;
                this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, S));
                DiversityWorkbench.XslEditor.XslTableRow LR = this.Table.TableRows[i];
                DiversityWorkbench.XslEditor.UserControlXslTableRow U = new DiversityWorkbench.XslEditor.UserControlXslTableRow(ref LR);
                U.ZoomFactor = this.ZoomFactor;
                U.buttonOptions.Click += new System.EventHandler(this.setRowOptions);
                this.panelRows.Controls.Add(U);
                this.RowControls.Add(U);
                U.Dock = DockStyle.Top;
                if (LR.RowHeight < U.MinimumSize.Height)
                    Offset += U.MinimumSize.Height - LR.RowHeight;
            }
            //if (Offset > 0)
            {
                System.Windows.Forms.Padding P = new Padding(0, Offset, 0, 0);
                this.tableLayoutPanelRowColumnEditor.Margin = P;
            }

            int Row = 0;
            foreach (DiversityWorkbench.XslEditor.XslNode TR in this._XslNode.XslNodes)
            {
                if (TR.Name == "tr" && TR.XslNodeType == "html")
                {
                    int Column = 0;
                    foreach (DiversityWorkbench.XslEditor.XslNode TC in TR.XslNodes)
                    {
                        if ((TC.Name == "td" || TC.Name == "th") && TC.XslNodeType == "html" && TC.XslNodes.Count > 0)
                        {
                            DiversityWorkbench.XslEditor.UserControlXslTableField Field = new UserControlXslTableField(TC);
                            this.tableLayoutPanelRowColumnEditor.Controls.Add(Field, Column, Row);
                            this.tableLayoutPanelRowColumnEditor.SetColumnSpan(Field, Field.SpanColumn);
                            this.tableLayoutPanelRowColumnEditor.SetRowSpan(Field, Field.SpanRow);
                            Field.Dock = DockStyle.Fill;
                            Field.Margin = new System.Windows.Forms.Padding(0);
                            Field.Visible = true;
                        }
                        Column++;
                    }
                }
                Row++;
            }

        }

        private void setRowOptions(object sender, EventArgs e)
        {
            System.Windows.Forms.Button B = (System.Windows.Forms.Button)sender;
            System.Windows.Forms.Panel P = (System.Windows.Forms.Panel)B.Parent;
            DiversityWorkbench.XslEditor.UserControlXslTableRow U = (DiversityWorkbench.XslEditor.UserControlXslTableRow)P.Parent;
            //this.tabControlDesigner.SelectedTab = this.tabPageRows;
            //this.labelRowOptionsAttributes.Text = U.RowHeight.ToString();
        }

        #endregion


    }
}
