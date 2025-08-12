using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.Forms
{
    public partial class FormImportWizardTest : Form
    {
        private DiversityCollection.Import_Column _IC;
        private DiversityCollection.Import _Import;
        public FormImportWizardTest(DiversityCollection.Import_Column IC, DiversityCollection.Import Import)
        {
            InitializeComponent();
            this._IC = IC;
            this._Import = Import;
        }
    }
}
