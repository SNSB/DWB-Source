using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.XsltEditor
{
    public partial class FormEditor : Form
    {
        public FormEditor()
        {
            InitializeComponent();
            DiversityWorkbench.XsltEditor.ControlMoverOrResizer.Init(this.richTextBox3);
        }

        private System.Windows.Forms.RichTextBox _RCurrent;
        private bool _MoveControl = false;
        private int _iClickX = 0;
        private int _iClickY = 0;

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            this._RCurrent = this.richTextBox1;
        }

        private void richTextBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this._RCurrent = this.richTextBox1;
            if (e.Button == MouseButtons.Left)
            {
                Point p = ConvertFromChildToForm(e.X, e.Y, this.richTextBox1);
                //iOldX = p.X;
                //iOldY = p.Y;
                _iClickX = e.X;
                _iClickY = e.Y;
                this._MoveControl = true;
            }
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this._RCurrent == this.richTextBox1 && this._MoveControl)
            {
                //this.richTextBox1.SetBounds(e.X, e.Y, this.richTextBox1.Width, this.richTextBox1.Height);
                //this.richTextBox1.SetBounds(this.richTextBox1.Location.X + e.X, this.richTextBox1.Location.Y + e.Y, this.richTextBox1.Width, this.richTextBox1.Height);
                
                Point p = new Point(); // New Coordinate
                p.X = e.X + this.richTextBox1.Left;
                p.Y = e.Y + this.richTextBox1.Top;
                this.richTextBox1.Left = p.X - _iClickX;
                this.richTextBox1.Top = p.Y - _iClickY;
            }
        }

        private void richTextBox1_MouseUp(object sender, MouseEventArgs e)
        {
            this._MoveControl = false;
        }

        private void richTextBox2_Click(object sender, EventArgs e)
        {
            this._RCurrent = this.richTextBox2;
        }

        private Point ConvertFromChildToForm(int x, int y, Control control)
        {
            Point p = new Point(x, y);
            control.Location = p;
            return p;
        }

        private void toolStripButtonSetRegistryKey_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.XsltEditor.FormSetKeyForWebbrowser f = new FormSetKeyForWebbrowser();
            f.ShowDialog();
        }
    }
}
