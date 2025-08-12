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
    public partial class FormSetColumnWidth : Form
    {

        private static ColumnWidthMode _ColumnWithMode = ColumnWidthMode.HeaderAndContent;

        #region Construction

        public FormSetColumnWidth(DiversityWorkbench.Spreadsheet.Sheet Sheet)
        {
            InitializeComponent();
            this.numericUpDownMaxWidth.Value = Sheet.MaxColumnWidth();
            switch (FormSetColumnWidth._ColumnWithMode)
            {
                case ColumnWidthMode.HeaderAndContent:
                    this.radioButtonContentAndHeader.Checked = true;
                    break;
                case ColumnWidthMode.Header:
                    this.radioButtonHeader.Checked = true;
                    break;
                case ColumnWidthMode.Content:
                    this.radioButtonContent.Checked = true;
                    break;
            }
        }
        
        #endregion

        #region Interface
        
        public int MaxColumnWidth() { return (int)this.numericUpDownMaxWidth.Value; }
        public enum ColumnWidthMode { Header, Content, HeaderAndContent }
        public ColumnWidthMode ModeForColumnWidth()
        {
            if (this.radioButtonHeader.Checked)
            {
                FormSetColumnWidth._ColumnWithMode = ColumnWidthMode.Header;
                return ColumnWidthMode.Header;
            }
            else if (this.radioButtonContent.Checked)
            {
                FormSetColumnWidth._ColumnWithMode = ColumnWidthMode.Content;
                return ColumnWidthMode.Content;
            }
            else
            {
                FormSetColumnWidth._ColumnWithMode = ColumnWidthMode.HeaderAndContent;
                return ColumnWidthMode.HeaderAndContent;
            }
        }
        
        #endregion

    }
}
