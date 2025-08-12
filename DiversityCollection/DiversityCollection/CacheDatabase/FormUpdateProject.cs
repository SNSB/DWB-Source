using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityCollection.CacheDatabase
{
    public partial class FormUpdateProject : Form
    {
        private string _Project;
        private int _ProjectID;
        private bool _AutoUpdate;

        public FormUpdateProject(string Project, int ProjectID, bool AutoUpdate)
        {
            InitializeComponent();
            this.Text = "Update " + Project;
            this._AutoUpdate = AutoUpdate;
            this._Project = Project;
            this._ProjectID = ProjectID;
        }
    }
}
