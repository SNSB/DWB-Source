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
    public partial class UserControlImport_ColumnDisplay : UserControl
    {
        private Import_Column _Import_Column;
        private int _Position;
        private DiversityCollection.Import _Import;
        private System.Windows.Forms.DataGridViewColumn _DataGridViewColumn;

        public UserControlImport_ColumnDisplay(Import_Column IC, int Position, DiversityCollection.Import Import)
        {
            InitializeComponent();
            this._Import_Column = IC;
            this._Position = Position;
            this._Import = Import;
            this.initControl();
        }

        private void initControl()
        {
            this.textBoxMapTable.Text = this._Import_Column.Table;
            this.textBoxMapColumn.Text = this._Import_Column.Column;
            if (this._Import_Column.TableAlias != null &&
                this._Import_Column.TableAlias.Length > 0)
            {
                this.textBoxMapTable.Text = this._Import_Column.TableAlias;
            }
            if (this._Import_Column.MultiColumn
                && this._Import_Column.Sequence() > 1)
            {
                this.labelSequence.Visible = true;
                this.textBoxSequence.Text = this._Import_Column.Sequence().ToString();
            }
            else
            {
                this.labelSequence.Visible = false;
                this.textBoxSequence.Visible = false;
            }

            if (this._Import_Column.PresetValue != null && this._Import_Column.PresetValueColumn != null)
            {
                this.labelPreset.Text = this._Import_Column.PresetValueColumn + " = " + this._Import_Column.PresetValue;
                this.labelPreset.Visible = true;
            }
            else
                this.labelPreset.Visible = false;
            string Entity = this._Import_Column.Table + "." + this._Import_Column.Column;
            string DisplayText = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation(Entity));
            if (DisplayText.Length > 0 && DisplayText != this._Import_Column.Column)
            {
                this.textBoxEntity.Text = DisplayText;
                this.textBoxEntity.Visible = true;
                this.labelEntity.Visible = true;
            }
            else
            {
                this.textBoxEntity.Visible = false;
                this.labelEntity.Visible = false;
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (System.Windows.Forms.MessageBox.Show("Delete this setting?", "Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string Key = this._Import_Column.Key;
                try
                {
                    this._Import.RemoveColumn(Key);
                }
                catch (System.Exception ex) { }
            }
        }
    }
}
