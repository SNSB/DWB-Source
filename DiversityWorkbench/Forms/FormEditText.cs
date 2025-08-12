using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormEditText : Form
    {
        #region Construction

        public FormEditText(string Title, string TextToEdit)
        {
            InitializeComponent();
            this.Text = Title;
            this.textBox.Text = TextToEdit;
        }

        public FormEditText(string Title, string TextToEdit, bool ReadOnly) 
            : this(Title, TextToEdit)
        {
            this.textBox.ReadOnly = ReadOnly;
            if (ReadOnly)
            {
                this.userControlDialogPanel1.Visible = false;
                if (Title.StartsWith("Edit "))
                    this.Text = Title.Substring(5);
            }
        }

        public FormEditText(string Title, string TextToEdit, bool ReadOnly, int MaxLength)
            : this(Title, TextToEdit, ReadOnly)
        {
            this.textBox.MaxLength = MaxLength;
        }

        #endregion

        #region Help

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }
        
        #endregion

        #region Properties

        private int _MaxLength;

        public string EditedText { get { return this.textBox.Text; } }

        public int MaxLength
        {
            set { this._MaxLength = value; }
            get
            {
                if (this._MaxLength == 0)
                    this._MaxLength = this.textBox.MaxLength;
                return this._MaxLength;
            }
        }

        #endregion

        #region Text length
        
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            if (this.textBox.Text.Length > this.MaxLength)
            {
                System.Windows.Forms.MessageBox.Show("The length of the entered text is " + this.textBox.Text.Length.ToString() + "\r\n" +
                    "It will be shortened to the maximal allowed length of " + this.MaxLength.ToString());
                this.textBox.Text = this.textBox.Text.Substring(0, this.MaxLength);
            }
        }
        
        #endregion

    }
}