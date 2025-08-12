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
    public partial class FormXslNodeEditor : Form
    {
        private DiversityWorkbench.XslEditor.XslNode _XslNode;

        public FormXslNodeEditor(DiversityWorkbench.XslEditor.XslNode XslNode)
        {
            InitializeComponent();
            this._XslNode = XslNode;
        }

    }
}
