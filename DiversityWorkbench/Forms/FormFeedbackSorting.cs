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
    public partial class FormFeedbackSorting : Form
    {
        public enum SortTarget { Priority, Date }
        public enum Sorting { Ascending, Unsorted, Descending }

        private Sorting _Sorting;
        private SortTarget _SortTarget;

        public FormFeedbackSorting(SortTarget Target, Sorting Sorting)
        {
            InitializeComponent();
            this.setSorting(Sorting);
            this._SortTarget = Target;
            this.label.Text = "Please select the type of sorting for the " + this._SortTarget.ToString().ToLower();
        }

        private void radioButtonAscending_Click(object sender, EventArgs e)
        {
            if (this.radioButtonAscending.Checked)
            {
                //this.radioButtonAscending.Checked = true;
                this.setSorting(Sorting.Ascending);
            }
        }

        private void radioButtonUnsorted_Click(object sender, EventArgs e)
        {
            if (this.radioButtonUnsorted.Checked)
            {
                //this.radioButtonUnsorted.Checked = true;
                this.setSorting(Sorting.Unsorted);
            }
        }

        private void radioButtonDescending_Click(object sender, EventArgs e)
        {
            if (this.radioButtonDescending.Checked)
            {
                //this.radioButtonDescending.Checked = true;
                this.setSorting(Sorting.Descending);
            }
        }

        private void setSorting(Sorting SelectedSorting)
        {
            this.radioButtonAscending.Checked = false;
            this.radioButtonDescending.Checked = false;
            this.radioButtonUnsorted.Checked = false;
            this._Sorting = SelectedSorting;
            switch(this._Sorting)
            {
                case Sorting.Ascending:
                    this.radioButtonAscending.Checked = true;
                    break;
                case Sorting.Descending:
                    this.radioButtonDescending.Checked = true;
                    break;
                case Sorting.Unsorted:
                    this.radioButtonUnsorted.Checked = true;
                    break;
            }
        }

        public Sorting SelectedSorting()
        {
            return this._Sorting;
        }
    }
}
