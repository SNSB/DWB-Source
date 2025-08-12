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
    public partial class UserControlTableColumnUnitValue : UserControl
    {

        private Export.TableColumn _TableColumn;
        private Export.TableColumnUnitValue _UnitValue;
        private Export.iExporter _iExporter;

        public UserControlTableColumnUnitValue(Export.TableColumn TC, Export.TableColumnUnitValue UV, DiversityWorkbench.Export.iExporter iExporter)
        {
            InitializeComponent();
            this._TableColumn = TC;
            this._UnitValue = UV;
            if (UV.UnitValue.StartsWith("Link to "))
            {
                this.labelUnitValue.ForeColor = System.Drawing.Color.Blue;
                this.labelUnitValue.Text = UV.UnitValue.Substring(UV.UnitValue.IndexOf("Diversity")) + " » " + UV.LinkedUnitValue;
                string LinkedSource = UV.LinkedDiversityWorkbenchModuleBaseUri;
                if (LinkedSource.EndsWith("/")) LinkedSource = LinkedSource.Substring(0, LinkedSource.Length - 1);
                LinkedSource = LinkedSource.Substring(LinkedSource.LastIndexOf("/") + 1);
                this.labelSource.Text = "(Source: " + UV.SourceDisplayText + " » " + LinkedSource + ")";// .DiversityWorkbenchModuleBaseUri + ")";
            }
            else
            {
                this.labelUnitValue.Text = UV.UnitValue;
                this.labelSource.Text = "(Source: " + UV.SourceDisplayText + ")";// .DiversityWorkbenchModuleBaseUri + ")";
            }
            this._iExporter = iExporter;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Export.FileColumn F = new FileColumn(this._UnitValue);
            if (Exporter.FileColumns.Count == 0)
                F.Position = 0;
            else
                F.Position = Exporter.FileColumnList.Last().Value.Position + 1;
            Exporter.FileColumns.Add(F);
            Exporter.ResetFileColumnList();
            this._iExporter.ShowFileColumns();
        }

    }
}
