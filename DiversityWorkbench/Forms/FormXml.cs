using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormXml : Form
    {
        public FormXml(string HeaderText, string XML, bool IsReadOnly)
        {
            InitializeComponent();
            this.userControlXMLTree.XML = XML;
            this.Text = HeaderText;
            if (IsReadOnly)
                this.userControlXMLTree.setToDisplayOnly();
        }
    }
}
