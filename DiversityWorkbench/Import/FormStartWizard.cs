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
    public partial class FormStartWizard : Form, iStartingMessage
    {
        public enum Direction { Import, Export }

        public FormStartWizard()
        {
            InitializeComponent();
        }

        public FormStartWizard(string Title, Direction _Direction)
        {
            InitializeComponent();
            this.label.Text = Title;
            this.Text = _Direction.ToString() + " wizard";
            if (_Direction == Direction.Export)
                this.pictureBox.Image = this.imageList.Images[0];
        }

        public FormStartWizard(string Title, string Text, System.Drawing.Image Image)
        {
            InitializeComponent();
            this.label.Text = Title;
            this.Text = Text;
            this.pictureBox.Image = Image;
        }

        /// <summary>
        /// Actualize the text in the start window
        /// CAUTION: Calls DoEvents! Block possible concurring events!
        /// </summary>
        /// <param name="CurrentStep"></param>
        public void ShowCurrentStep(string CurrentStep)
        {
            this.labelCurrentStep.Text = CurrentStep;
            Application.DoEvents();
        }

    }
}
