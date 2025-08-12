using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Export
{
    public partial class UserControlFileColumn : UserControl
    {

        #region Parameter
        
        private Export.TableColumn _TableColumn;
        private Export.FileColumn _FileColumn;
        private Export.iExporter _iExporter;
        
        #endregion

        #region Construction & Controls

        public UserControlFileColumn(Export.FileColumn FC, DiversityWorkbench.Export.iExporter iExporter, string HelpNamespace)
        {
            InitializeComponent();
            this._FileColumn = FC;
            this._iExporter = iExporter;
            this.helpProvider.HelpNamespace = HelpNamespace;
            this.labelSource.Text = FC.TableColumn.Table.DisplayText;
            if (FC.TableColumnUnitValue != null)
            {
                if (FC.TableColumnUnitValue.DiversityWorkbenchModuleBaseUri != null)
                {
                    System.Drawing.Font F = new Font("Arial", 7);
                    this.labelUnitSource.Font = F;
                    this.labelUnitSource.ForeColor = System.Drawing.Color.Blue;
                    this.labelUnitSource.TextAlign = ContentAlignment.MiddleCenter;
                    this.textBoxHeader.Text = FC.Header;
                    if (FC.TableColumnUnitValue.LinkedDiversityWorkbenchModuleBaseUri != null)
                    {
                        this.labelUnitSource.Text = FC.TableColumnUnitValue.LinkedDiversityWorkbenchModuleBaseUri;
                        this.labelTableColumn.Text = FC.TableColumnUnitValue.LinkedUnitValue;
                    }
                    else
                    {
                        this.labelUnitSource.Text = FC.TableColumnUnitValue.DiversityWorkbenchModuleBaseUri;
                        this.labelTableColumn.Text = FC.TableColumnUnitValue.UnitValue;
                    }
                }
                else
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in FC.TableColumn.ForeignRelations)
                    {
                        if (this.labelUnitSource.Text.Length > 0)
                            this.labelUnitSource.Text += " and ";
                        this.labelUnitSource.Text += KV.Key;
                    }
                    this.labelUnitSource.TextAlign = ContentAlignment.MiddleCenter;
                    this.labelUnitSource.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                    this.labelTableColumn.Text = FC.TableColumnUnitValue.UnitValue;
                    this.textBoxHeader.Text = FC.Header;
                }
            }
            else
            {
                this.labelTableColumn.Text = FC.TableColumn.DisplayText;
                this.textBoxHeader.Text = FC.Header;
            }
            this.setControls();
        }

        public UserControlFileColumn(Export.FileColumn FC, string HelpNamespace)
        {
            InitializeComponent();
            this._FileColumn = FC;
            //this._TransformationFilter = TF;
            this.helpProvider.HelpNamespace = HelpNamespace;
            this.labelSource.Text = FC.TableColumn.Table.DisplayText;
            if (FC.TableColumnUnitValue != null)
            {
                if (FC.TableColumnUnitValue.DiversityWorkbenchModuleBaseUri != null)
                {
                    System.Drawing.Font F = new Font("Arial", 7);
                    this.labelUnitSource.Font = F;
                    this.labelUnitSource.ForeColor = System.Drawing.Color.Blue;
                    this.labelUnitSource.TextAlign = ContentAlignment.MiddleCenter;
                    this.labelUnitSource.Text = FC.TableColumnUnitValue.DiversityWorkbenchModuleBaseUri;
                    this.labelTableColumn.Text = FC.TableColumnUnitValue.UnitValue;
                    this.textBoxHeader.Text = FC.TableColumnUnitValue.UnitValue;
                }
                else
                {
                    foreach (System.Collections.Generic.KeyValuePair<string, string> KV in FC.TableColumn.ForeignRelations)
                    {
                        if (this.labelUnitSource.Text.Length > 0)
                            this.labelUnitSource.Text += " and ";
                        this.labelUnitSource.Text += KV.Key;
                    }
                    this.labelUnitSource.TextAlign = ContentAlignment.MiddleCenter;
                    this.labelUnitSource.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                    this.labelTableColumn.Text = FC.TableColumnUnitValue.UnitValue;
                    this.textBoxHeader.Text = FC.TableColumnUnitValue.UnitValue;
                }
            }
            else
            {
                this.labelTableColumn.Text = FC.TableColumn.DisplayText;
                this.textBoxHeader.Text = FC.Header;
            }
            this.setControls();
            this.buttonDelete.Enabled = false;
            this.buttonMoveLeft.Enabled = false;
            this.buttonMoveRight.Enabled = false;
            this.buttonTransform.Enabled = false;
            this.buttonFilter.Visible = true;
            this.textBoxHeader.Enabled = false;
            this.textBoxPostfix.Enabled = false;
            this.textBoxPrefix.Enabled = false;
            this.panelFileColumnSeparator.Enabled = false;
        }

        private void setControls()
        {
            if (this._FileColumn.IsSeparatedFromPreviousColumn)
            {
                this.panelFileColumnSeparator.BackColor = System.Drawing.Color.DarkGray;
                this.toolTip.SetToolTip(this.panelFileColumnSeparator, "Separate from previous column");
            }
            else
            {
                this.panelFileColumnSeparator.BackColor = System.Drawing.SystemColors.Control;
                this.toolTip.SetToolTip(this.panelFileColumnSeparator, "Combine with previous column");
            }
            if (this._FileColumn.Transformations.Count > 0)
            {
                this.buttonTransform.BackColor = System.Drawing.Color.Red;
            }
            else
            {
                this.buttonTransform.BackColor = System.Drawing.SystemColors.Control;
            }
            this.textBoxPostfix.Text = this._FileColumn.Postfix;
            this.textBoxPrefix.Text = this._FileColumn.Prefix;
            this.textBoxHeader.Text = this._FileColumn.Header;
            this.numericUpDownPosition.Value = this._FileColumn.Position + 1;
            int LastPosition = Exporter.FileColumnList.Last().Value.Position;
            this.numericUpDownPosition.Maximum = LastPosition + 1;
            this.numericUpDownPosition.Minimum = 1;
            DiversityWorkbench.Forms.FormFunctions.addEditOnDoubleClickToTextboxes(this.textBoxHeader);
            DiversityWorkbench.Forms.FormFunctions.addEditOnDoubleClickToTextboxes(this.textBoxPostfix);
            DiversityWorkbench.Forms.FormFunctions.addEditOnDoubleClickToTextboxes(this.textBoxPrefix);
        }
        
        #endregion

        #region Events

        #region Delete
        
        private void buttonDelete_Click(object sender, EventArgs e)
        {
            Exporter.FileColumns.Remove(this._FileColumn);
            Exporter.ResetFileColumnList();
            this._iExporter.ShowFileColumns();
        }
        
        #endregion

        private void panelFileColumnSeparator_Click(object sender, EventArgs e)
        {
            if (this._FileColumn.IsSeparatedFromPreviousColumn)
            {
                this._FileColumn.IsSeparatedFromPreviousColumn = false;
            }
            else
                this._FileColumn.IsSeparatedFromPreviousColumn = true;
            this.setControls();
        }

        #region Moving

        private void buttonMoveLeft_Click(object sender, EventArgs e)
        {
            int ValueCurrent = this._FileColumn.Position;
            int ValuePrevious = 0;
            string Message = this.CheckFilterUsage(ValueCurrent);
            foreach (System.Collections.Generic.KeyValuePair<int, Export.FileColumn> KV in Exporter.FileColumnList)
            {
                if (KV.Value.Position > this._FileColumn.Position)
                    continue;
                if (KV.Value.Position > ValuePrevious && KV.Value.Position < this._FileColumn.Position)
                    ValuePrevious = KV.Value.Position;
            }
            Exporter.FileColumnList[ValueCurrent].Position = ValuePrevious;
            Exporter.FileColumnList[ValuePrevious].Position = ValueCurrent;
            Exporter.ResetFileColumnList();
            this._iExporter.ShowFileColumns();
            if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
            Message = this.CheckFilterUsage(ValuePrevious);
            if (Message.Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
        }

        private void buttonMoveRight_Click(object sender, EventArgs e)
        {
            int ValueCurrent = this._FileColumn.Position;
            int ValueNext = ValueCurrent + 1;
            string Message = this.CheckFilterUsage(ValueCurrent);
            bool moveDone = false;
            foreach (System.Collections.Generic.KeyValuePair<int, Export.FileColumn> KV in Exporter.FileColumnList)
            {
                if (KV.Value.Position < this._FileColumn.Position)
                    continue;
                if (KV.Value.Position < ValueNext && KV.Value.Position > this._FileColumn.Position)
                {
                    ValueNext = KV.Value.Position;
                    moveDone = true;
                }
            }
            if (moveDone)
            {
                Exporter.FileColumnList[ValueCurrent].Position = ValueNext;
                Exporter.FileColumnList[ValueNext].Position = ValueCurrent;
                Exporter.ResetFileColumnList();
                this._iExporter.ShowFileColumns();
                if (Message.Length > 0)
                    System.Windows.Forms.MessageBox.Show(Message);
                Message = this.CheckFilterUsage(ValueNext);
                if (Message.Length > 0)
                    System.Windows.Forms.MessageBox.Show(Message);
            }
        }

        private void buttonMoveToPosition_Click(object sender, EventArgs e)
        {
            this.MoveColumn((int)this.numericUpDownPosition.Value - 1);
        }

        private void MoveColumn(int Position)
        {
            if (Position == this._FileColumn.Position)
                return;
            int CurrentPosition = this._FileColumn.Position;
            string Message = this.CheckFilterUsage(CurrentPosition);
            this._FileColumn.Position = Position;
            foreach (System.Collections.Generic.KeyValuePair<int, Export.FileColumn> KV in Exporter.FileColumnList)
            {
                if (Position > CurrentPosition) // Move to back
                {
                    if (KV.Key <= Position && KV.Key > CurrentPosition)
                    {
                        KV.Value.Position--;
                        Message += this.CheckFilterUsage(KV.Key) + "\r\n";
                    }
                }
                else // move to front
                {
                    if (KV.Key >= Position && KV.Key < CurrentPosition)
                    {
                        KV.Value.Position++;
                        Message += this.CheckFilterUsage(KV.Key) + "\r\n";
                    }
                }
            }

            Exporter.ResetFileColumnList();
            this._iExporter.ShowFileColumns();
            if (Message.Trim().Length > 0)
                System.Windows.Forms.MessageBox.Show(Message);
        }

        #endregion

        private string CheckFilterUsage(double ValueCurrent)
        {
            string Message = "";
            {
                foreach (Export.FileColumn FC in Export.Exporter.FileColumns)
                {
                    foreach (Export.Transformation T in FC.Transformations)
                    {
                        if (T.TypeOfTransformation == Transformation.TransformationType.Filter)
                        {
                            foreach (Export.TransformationFilter F in T.FilterConditions)
                            {
                                if (F.PositionOfFilterColumn == ValueCurrent)
                                {
                                    if (Message.Length == 0) Message = "Column was used for filter\r\nPlease check filters in column(s)\r\n";
                                    else Message += "\r\n";
                                    Message += FC.Position.ToString();
                                    if (FC.Header.Length > 0) Message += " (" + FC.Header + ")";
                                }
                            }
                        }
                    }
                }
            }
            return Message;
        }
        
        private void buttonTransform_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Export.FormTransformation f = new FormTransformation(this._FileColumn.Transformations, this._FileColumn.Prefix, this._FileColumn.Postfix, this._FileColumn, this._iExporter, this.helpProvider.HelpNamespace);
            f.ShowDialog();
            this.setControls();
        }

        private void textBoxHeader_TextChanged(object sender, EventArgs e)
        {
            this._FileColumn.Header = this.textBoxHeader.Text;
            string Header = "The header text of the column";
            if (this._FileColumn.Header.Length > 0)
                Header += ": " + this._FileColumn.Header;
            this.toolTip.SetToolTip(this.textBoxHeader, Header);
        }

        private void textBoxPrefix_TextChanged(object sender, EventArgs e)
        {
            this._FileColumn.Prefix = this.textBoxPrefix.Text;
        }

        private void textBoxPostfix_TextChanged(object sender, EventArgs e)
        {
            this._FileColumn.Postfix = this.textBoxPostfix.Text;
        }

        #endregion

        #region Interface

        public void SetAsSelected(bool IsSelected)
        {
            if (IsSelected)
            {
                this.BackColor = System.Drawing.Color.LightBlue;
            }
            else this.BackColor = System.Drawing.SystemColors.Control;
        }

        public void SetSeparatorVisibility(bool IsVisible)
        {
            this.panelFileColumnSeparator.Visible = IsVisible;
            if (!IsVisible) this.Width = 156;
            else this.Width = 164;
        }

        //public double FilterPosition() { return this._FileColumn.Position; }
        public int FilterPosition() { return this._FileColumn.Position; }
        
        #endregion



    }
}
