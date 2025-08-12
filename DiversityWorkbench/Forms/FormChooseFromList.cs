using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormChooseFromList : Form
    {
        public FormChooseFromList(ref System.Data.DataTable dtItems, string ItemColumnHeader, string Title, string Message)
        {
            InitializeComponent();

            this.AcceptButton = this.userControlDialogPanel.buttonOK;
            this.CancelButton = this.userControlDialogPanel.buttonCancel;

            this.dataGridViewItems.DataSource = dtItems;

            this.dataGridViewItems.Columns[0].Width = 250;
            this.dataGridViewItems.Columns[0].HeaderText = ItemColumnHeader;
            this.dataGridViewItems.Columns[0].ReadOnly = true;

            this.dataGridViewItems.Columns[1].Visible = false;

            if (Title != "") this.Text = Title;
            if (Message != "") this.labelMain.Text = Message;
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

    }
}