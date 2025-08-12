using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Import
{
    public partial class FormColumnInfo : Form
    {
        private DiversityWorkbench.Import.DataColumn _DataColumn;

        public FormColumnInfo(DiversityWorkbench.Import.DataColumn DataColumn)
        {
            InitializeComponent();
            this._DataColumn = DataColumn;
            this.Text = this._DataColumn.DisplayText;
            string Message = this.Text;
            if (this._DataColumn.DisplayText != this._DataColumn.ColumnName)
                Message += " (= column " + this._DataColumn.ColumnName + ")";
            Message += " in " + this._DataColumn.DataTable.DisplayText + " (= table " + this._DataColumn.DataTable.TableName + ")";
            Message += "\r\nDatatype = " + this._DataColumn.DataType;
            if (this._DataColumn.MaximumLength > 0)
                Message += "\r\nMax. length: " + this._DataColumn.MaximumLength.ToString();
            System.Collections.Generic.Dictionary<string, string> E = DiversityWorkbench.Entity.EntityInformation(this._DataColumn.DataTable.TableName + "." + this._DataColumn.ColumnName);
            if (E.ContainsKey("Description"))
                Message = E["Description"] + "\r\nDatatype: " + this._DataColumn.DataType;
            else if (E.Count == 0)
            {
                string Description = DiversityWorkbench.Forms.FormFunctions.getColumnDescription(this._DataColumn.DataTable.TableName, this._DataColumn.ColumnName);
                if (Description.Length > 0)
                {
                    Message += "\r\nDescription:\r\n" + Description;

                }
                if (!this._DataColumn.AllowInsert)
                    Message += "\r\n" + "Insert is not allowed.";
            }
            if (this._DataColumn.SourceFunctionDisplayText != null && this._DataColumn.SourceFunctionDisplayText.Length > 0)
            {
                Message += "\r\n";
                if (this._DataColumn.DataRetrievalType == DiversityWorkbench.Import.DataColumn.RetrievalType.FunctionInDatabase)
                {
                    Message += "\r\nEither provide " + this._DataColumn.DisplayText + "\r\nor";
                }
                Message += "\r\n" + this._DataColumn.SourceFunctionDisplayText;
            }
            this.labelColumnDescription.Text = Message;
            this.InitForm();
            if (this._DataColumn.SourceFunctionDisplayText != null && this._DataColumn.SourceFunctionDisplayText.Length > 0)
            {
                string[] stringSeparators = new string[] { "\r\n" };
                string[] TestHeight = Message.Split(stringSeparators, StringSplitOptions.None);
                this.Height += 10 * TestHeight.Length;
                //this.Height += 15;
                //if (this._DataColumn.DataRetrievalType == DiversityWorkbench.Import.DataColumn.RetrievalType.FunctionInDatabase)
                //    this.Height += 15;
            }
            else
            {
                string[] stringSeparators = new string[] { "\r\n" };
                string[] TestHeight = Message.Split(stringSeparators, StringSplitOptions.None);
                this.Height += 12 * TestHeight.Length;
            }
        }

        private void InitForm()
        {
            if (this._DataColumn.DataType.ToLower() == "geography")
            {
                this.splitContainerGeography.Visible = true;
            }
            else
            {
                this.splitContainerGeography.Visible = false;
                this.userControlDialogPanel.Visible = false;
                this.Height = 90;
            }
        }

        public string Prefix()
        {
            if (this.radioButtonGeographyPoint.Checked) return this.labelGeoraphyPrefixPOINT.Text;
            else if (this.radioButtonGeographyLine.Checked) return this.labelGeoraphyPrefixLINE.Text;
            else if (this.radioButtonGeographyArea.Checked) return this.labelGeoraphyPrefixAREA.Text;
            return "";
        }

        public string Postfix()
        {
            if (this.radioButtonGeographyPoint.Checked) return this.labelGeoraphyPostfixPOINT.Text;
            else if (this.radioButtonGeographyLine.Checked) return this.labelGeoraphyPostfixLINE.Text;
            else if (this.radioButtonGeographyArea.Checked) return this.labelGeoraphyPostfixAREA.Text;
            return "";
        }
    }
}
