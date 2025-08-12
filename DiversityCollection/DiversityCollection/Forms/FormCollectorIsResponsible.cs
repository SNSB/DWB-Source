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
    public partial class FormCollectorIsResponsible : Form
    {
        public enum CollectorAsIdentifier { none, first, all }
        private CollectorAsIdentifier _Respons = CollectorAsIdentifier.none;

        public FormCollectorIsResponsible(string FirstCollector, string AllCollectors)
        {
            InitializeComponent();
            this.buttonAllCollectors.Text = AllCollectors;
            this.buttonFirstCollector.Text = FirstCollector;
        }

        private void buttonFirstCollector_Click(object sender, EventArgs e)
        {
            this._Respons = CollectorAsIdentifier.first;
            this.Close();
        }

        private void buttonAllCollectors_Click(object sender, EventArgs e)
        {
            this._Respons = CollectorAsIdentifier.all;
            this.Close();
        }

        public CollectorAsIdentifier Responsible { get { return this._Respons; } }

    }
}
