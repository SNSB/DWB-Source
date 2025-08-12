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
    public partial class FormGetMultiFromList : Form
    {

        System.Collections.Generic.Dictionary<string, bool> _Items;

        public System.Collections.Generic.Dictionary<string, bool> Items
        {
            get
            {
                this._Items.Clear();
                foreach (System.Object o in this.checkedListBox.CheckedItems)
                {
                    this._Items[o.ToString()] = true;
                }
                return _Items;
            }
            set { _Items = value; }
        }

        /// <summary>
        /// Form for getting several items from a list.
        /// </summary>
        /// <param name="Title">Title of the form</param>
        /// <param name="Header">Header text with explanation</param>
        /// <param name="Items">The list of the item - initially all must be FALSE</param>
        public FormGetMultiFromList(string Title, string Header, System.Collections.Generic.Dictionary<string, bool> Items)
        {
            InitializeComponent();
            this._Items = Items;
            if (Title != null && Title != "")
                this.Text = Title;
            if (Header != null && Header != "")
                this.labelHeader.Text = Header;
            //else
            //    this.labelHeader.Visible = false;
            this.initForm();
        }

        private void initForm()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, bool> KV in this._Items)
            {
                this.checkedListBox.Items.Add(KV.Key, KV.Value);
            }
        }

        public System.Collections.Generic.List<string> SelectedItems()
        {
            System.Collections.Generic.List<string> L = new List<string>();
            foreach (System.Object o in this.checkedListBox.CheckedItems)
                L.Add(o.ToString());
            return L;
        }

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Checking items

        private void buttonAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBox.Items.Count; i++)
                this.checkedListBox.SetItemChecked(i, true);
        }

        private void buttonNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBox.Items.Count; i++)
                this.checkedListBox.SetItemChecked(i, false);
        }

        #endregion

    }
}
