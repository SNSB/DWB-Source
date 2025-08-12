using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DiversityWorkbench.Forms
{
    public partial class FormGetStringFromList : Form
    {

        #region Parameter

        private System.Data.DataTable _dtList;
        private bool _EnforceEntry = false;

        #endregion

        #region Construction

        public FormGetStringFromList(System.Collections.Generic.Dictionary<int, string> Dictionary, string FormTitle, string HeaderTitle)
        {
            InitializeComponent();
            this.textBox.Visible = false;
            this.labelTextBox.Visible = false;
            this.labelHeader.Text = HeaderTitle;
            this.Text = FormTitle;

            this._dtList = new DataTable();
            this._dtList.Columns.Add("Int", typeof(int));
            this._dtList.Columns.Add("String", typeof(string));
            foreach (System.Collections.Generic.KeyValuePair<int, string> KV in Dictionary)
            {
                System.Data.DataRow R = this._dtList.NewRow();
                R[0] = KV.Key;
                R[1] = KV.Value;
                this._dtList.Rows.Add(R);
            }
            this.setHeight(108);
            this.comboBox.DataSource = this._dtList;
            try
            {
                this.comboBox.DisplayMember = this._dtList.Columns[1].ToString();
                this.comboBox.ValueMember = this._dtList.Columns[0].ToString();
                this.comboBox.Focus();
            }
            catch { }
        }

        public FormGetStringFromList(System.Collections.Generic.Dictionary<string, string> Dictionary, string FormTitle, string HeaderTitle, bool RestrictToList, System.Drawing.Image image = null)
        {
            InitializeComponent();
            this.textBox.Visible = false;
            this.labelTextBox.Visible = false;
            this.labelHeader.Text = HeaderTitle;
            this.Text = FormTitle;

            if (image != null)
            {
                System.Drawing.Bitmap bitmap = new Bitmap(image, 16, 16);
                this.Icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
                this.ShowIcon = true;
            }

            if (RestrictToList)
            {
                this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                this.labelTextBox.Visible = false;
                this.textBox.Visible = false;
                this.Height = (int)(120 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            }

            this._dtList = new DataTable();
            this._dtList.Columns.Add("String", typeof(string));
            this._dtList.Columns.Add("Value", typeof(string));
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in Dictionary)
            {
                System.Data.DataRow R = this._dtList.NewRow();
                R[0] = KV.Key;
                R[1] = KV.Value;
                this._dtList.Rows.Add(R);
            }
            this.setHeight(108);
            this.comboBox.DataSource = this._dtList;
            try
            {
                this.comboBox.DisplayMember = this._dtList.Columns[1].ToString();
                this.comboBox.ValueMember = this._dtList.Columns[0].ToString();
                this.comboBox.Focus();
            }
            catch { }
        }

        public FormGetStringFromList(System.Collections.Generic.List<string> List, string FormTitle, string HeaderTitle, bool RestrictToList)
        {
            InitializeComponent();
            this.textBox.Visible = false;
            this.labelTextBox.Visible = false;
            this.labelHeader.Text = HeaderTitle;
            this.Text = FormTitle;

            if (RestrictToList)
            {
                this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                this.labelTextBox.Visible = false;
                this.textBox.Visible = false;
                this.Height = (int)(120 * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
            }

            this._dtList = new DataTable();
            this._dtList.Columns.Add("String", typeof(string));
            this._dtList.Columns.Add("Value", typeof(string));
            foreach (string s in List)
            {
                System.Data.DataRow R = this._dtList.NewRow();
                R[0] = s;
                R[1] = s;
                this._dtList.Rows.Add(R);
            }
            this.setHeight(108);
            this.comboBox.DataSource = this._dtList;
            try
            {
                this.comboBox.DisplayMember = this._dtList.Columns[1].ToString();
                this.comboBox.ValueMember = this._dtList.Columns[0].ToString();
            }
            catch { }
        }

        //public FormGetStringFromList(System.Collections.Generic.List<string> List, string FormTitle, string HeaderTitle, bool RestrictToList)
        //{
        //    InitializeComponent();
        //    this.textBox.Visible = false;
        //    this.labelTextBox.Visible = false;
        //    this.labelHeader.Text = HeaderTitle;
        //    this.Text = FormTitle;

        //    if (RestrictToList)
        //    {
        //        this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        //        this.labelTextBox.Visible = false;
        //        this.textBox.Visible = false;
        //        this.Height = 120;
        //    }

        //    this._dtList = new DataTable();
        //    this._dtList.Columns.Add("String", typeof(string));
        //    foreach (string s in List)
        //    {
        //        System.Data.DataRow R = this._dtList.NewRow();
        //        R[0] = s;
        //        this._dtList.Rows.Add(R);
        //    }
        //    this.setHeight(104);
        //    this.comboBox.DataSource = this._dtList;
        //    try
        //    {
        //        this.comboBox.DisplayMember = this._dtList.Columns[0].ToString();
        //        this.comboBox.ValueMember = this._dtList.Columns[0].ToString();
        //    }
        //    catch { }
        //}

        public FormGetStringFromList(System.Collections.Generic.Dictionary<int?, string> Dictionary, string FormTitle, string HeaderTitle)
        {
            InitializeComponent();
            this.textBox.Visible = false;
            this.labelTextBox.Visible = false;
            this.labelHeader.Text = HeaderTitle;
            this.Text = FormTitle;

            this._dtList = new DataTable();
            this._dtList.Columns.Add("Int", typeof(int?));
            this._dtList.Columns.Add("String", typeof(string));
            foreach (System.Collections.Generic.KeyValuePair<int?, string> KV in Dictionary)
            {
                System.Data.DataRow R = this._dtList.NewRow();
                R[0] = KV.Key;
                R[1] = KV.Value;
                this._dtList.Rows.Add(R);
            }
            this.setHeight(104);
            this.comboBox.DataSource = this._dtList;
            try
            {
                this.comboBox.DisplayMember = this._dtList.Columns[1].ToString();
                this.comboBox.ValueMember = this._dtList.Columns[0].ToString();
            }
            catch { }
        }

        public FormGetStringFromList(System.Data.DataTable dtList, string HeaderTitle)
        {
            InitializeComponent();
            this.labelHeader.Text = HeaderTitle;
            this._dtList = dtList;
            this.setHeight(108);
            this.comboBox.DataSource = this._dtList;
            try
            {
                this.comboBox.DisplayMember = this._dtList.Columns[0].ToString();
                if (dtList.Columns.Count > 1)
                    this.comboBox.ValueMember = this._dtList.Columns[1].ToString();
                else
                    this.comboBox.ValueMember = this._dtList.Columns[0].ToString();
            }
            catch { }
        }

        public FormGetStringFromList(System.Data.DataTable dtList, string HeaderTitle, bool RestrictToList, bool EnforceEntry = false)
        {
            InitializeComponent();
            this.labelHeader.Text = HeaderTitle;
            this._dtList = dtList;
            this.setHeight(108);
            this.comboBox.DataSource = this._dtList;
            try
            {
                this.comboBox.DisplayMember = this._dtList.Columns[0].ToString();
                if (dtList.Columns.Count > 1)
                    this.comboBox.ValueMember = this._dtList.Columns[1].ToString();
                else
                    this.comboBox.ValueMember = this._dtList.Columns[0].ToString();
                if (RestrictToList)
                    this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                if (EnforceEntry)
                {
                    this.comboBox.TextChanged += new EventHandler(this.EnableOKbutton);
                    if (this.comboBox.Text.Length == 0)
                        this.userControlDialogPanel1.buttonOK.Enabled = false;
                }
            }
            catch { }
        }

        public FormGetStringFromList(System.Data.DataTable dtList, string FormTitle, string HeaderTitle, string TextBoxTitle) :
            this(dtList, HeaderTitle)
        {
            this.Text = FormTitle;
            this.labelTextBox.Text = TextBoxTitle;
            this.labelTextBox.Visible = true;
            this.textBox.Visible = true;
            this.setHeight(146);
        }

        /// <summary>
        /// A form used to select an item from a list
        /// </summary>
        /// <param name="dtList">The table containing the items</param>
        /// <param name="DisplayColumn">The name of the display column</param>
        /// <param name="ValueColumn">The name of the value column</param>
        /// <param name="FormTitle">The title for the form</param>
        /// <param name="HeaderTitle">The text in the form above the combobox for the selection of the item</param>
        public FormGetStringFromList(System.Data.DataTable dtList, string DisplayColumn, string ValueColumn, string FormTitle, string HeaderTitle)
        {
            InitializeComponent();
            this.Text = FormTitle;
            this.labelHeader.Text = HeaderTitle;
            this._dtList = dtList;
            this.setHeight(108);
            this.comboBox.DataSource = this._dtList;
            try
            {
                this.comboBox.DisplayMember = DisplayColumn;
                this.comboBox.ValueMember = ValueColumn;
            }
            catch { }
        }

        /// <summary>
        /// A form used to select an item from a list
        /// </summary>
        /// <param name="dtList">The table containing the items</param>
        /// <param name="DisplayColumn">The name of the display column</param>
        /// <param name="ValueColumn">The name of the value column</param>
        /// <param name="FormTitle">The title for the form</param>
        /// <param name="HeaderTitle">The text in the form above the combobox for the selection of the item</param>
        /// <param name="PresetSelection">The value that should be preselected</param>
        public FormGetStringFromList(System.Data.DataTable dtList, string DisplayColumn, string ValueColumn, string FormTitle, string HeaderTitle, string PresetSelection)
            : this(dtList, DisplayColumn, ValueColumn, FormTitle, HeaderTitle)
        {
            try
            {
                int i = 0;
                foreach (System.Data.DataRow R in this._dtList.Rows)
                {
                    if (R[ValueColumn].ToString() == PresetSelection)
                        break;
                    i++;
                }
                this.comboBox.SelectedIndex = i;

            }
            catch { }
            this.textBox.Visible = false;
        }

        /// <summary>
        /// A form used to select an item from a list
        /// </summary>
        /// <param name="dtList">The table containing the items</param>
        /// <param name="DisplayColumn">The name of the display column</param>
        /// <param name="ValueColumn">The name of the value column</param>
        /// <param name="FormTitle">The title for the form</param>
        /// <param name="HeaderTitle">The text in the form above the combobox for the selection of the item</param>
        /// <param name="PresetSelection">The value that should be preselected</param>
        /// <param name="ShowTextBox">If the textbox for entering a string should be shown</param>
        public FormGetStringFromList(System.Data.DataTable dtList, string DisplayColumn, string ValueColumn, string FormTitle, string HeaderTitle, string PresetSelection, bool ShowTextBox)
            : this(dtList, FormTitle, HeaderTitle, "")
        {
            try
            {
                int i = 0;
                foreach (System.Data.DataRow R in this._dtList.Rows)
                {
                    if (R[ValueColumn].ToString() == PresetSelection)
                        break;
                    i++;
                }
                if (_dtList.Rows.Count > i)
                    this.comboBox.SelectedIndex = i;
                this.textBox.Visible = ShowTextBox;
                this.labelTextBox.Visible = ShowTextBox;
                if (ShowTextBox) this.textBox.Text = PresetSelection;
                if (!ShowTextBox)
                    this.Height = this.Height - 36;
                this.comboBox.DisplayMember = DisplayColumn;
                this.comboBox.ValueMember = ValueColumn;
                this.comboBox.Focus();
            }
            catch (System.Exception ex) { }
        }

        /// <summary>
        /// A form used to select an item from a list
        /// </summary>
        /// <param name="dtList">The table containing the items</param>
        /// <param name="DisplayColumn">The name of the display column</param>
        /// <param name="ValueColumn">The name of the value column</param>
        /// <param name="FormTitle">The title for the form</param>
        /// <param name="HeaderTitle">The text in the form above the combobox for the selection of the item</param>
        /// <param name="PresetSelection">The value that should be preselected</param>
        /// <param name="ShowTextBox">If the textbox for entering a string should be shown</param>
        /// <param name="RestrictToList">If the values should be taken exclusive from the list</param>
        /// <param name="EnforceEntry">If any value must be given</param>
        public FormGetStringFromList(System.Data.DataTable dtList, string DisplayColumn, string ValueColumn, string FormTitle, string HeaderTitle, string PresetSelection, bool ShowTextBox, bool RestrictToList, bool EnforceEntry = false, System.Drawing.Image image = null)
            : this(dtList, DisplayColumn, ValueColumn, FormTitle, HeaderTitle, PresetSelection, ShowTextBox)
        {
            try
            {
                if (RestrictToList)
                    this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                this.textBox.Visible = ShowTextBox;
                this._EnforceEntry = EnforceEntry;
                if (EnforceEntry)
                {
                    this.comboBox.TextChanged += new EventHandler(this.EnableOKbutton);
                    if (this.comboBox.Text.Length == 0)
                        this.userControlDialogPanel1.buttonOK.Enabled = false;
                    if (ShowTextBox)
                    {
                        this.textBox.TextChanged += new EventHandler(this.EnableOKbutton);
                    }
                }
                if (image != null)
                {
                    System.Drawing.Bitmap bitmap = new Bitmap(image, 16, 16);
                    this.Icon = System.Drawing.Icon.FromHandle(bitmap.GetHicon());
                    this.ShowIcon = true;
                }
            }
            catch (System.Exception ex) { }
        }

        private void setHeight(int minHeight)
        {
            int iLines = 1;
            string[] stringSeparators = new string[] { "\r\n" };
            string[] HeaderParts = this.labelHeader.Text.Split(stringSeparators, StringSplitOptions.None);
            iLines = HeaderParts.Length;
            float LineHeight = this.labelHeader.Font.GetHeight();
            int WrapChar = (3 * this.labelHeader.Width) / (2 * (int)LineHeight); // Approximation to insert additional lines for long strings
            for (int i = 0; i < HeaderParts.Length; i++)
            {
                if (HeaderParts[i].Length > WrapChar)
                    iLines += HeaderParts[i].Length / WrapChar;
            }
            this.Height = minHeight + 1 + ((int)LineHeight + 1) * iLines; // Toni 20200219: One added to show drop-down completely
            this.Height = (int)(this.Height * DiversityWorkbench.Forms.FormFunctions.DisplayZoomFactor);
        }

        #endregion

        #region Properties

        public DiversityWorkbench.UserControls.UserControlDialogPanel.DisplayOption DisplayOption
        {
            set { this.userControlDialogPanel1.Display = value; }
        }

        public void PresetSelection(string Key)
        {
            this.comboBox.Sorted = false;
            for (int i = 0; i < this._dtList.Rows.Count; i++)
            {
                if (this._dtList.Rows[i][0].ToString() == Key)
                {
                    this.comboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        public bool CanEditValuesInList
        {
            set
            {
                if (value)
                    this.comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                else this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }

        public string SelectedString { get { return this.comboBox.Text; } }

        public string SelectedValue
        {
            get
            {
                if (this._dtList.Columns.Count > 1)
                {
                    if (this.comboBox.SelectedValue == null)
                        return "NULL";
                    try
                    {
                        if (this.comboBox.Sorted)
                        {
                            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBox.SelectedItem;
                            string Value = R[this.comboBox.ValueMember].ToString();
                            return Value;
                        }
                    }
                    catch (System.Exception ex) { }
                    return this.comboBox.SelectedValue.ToString();
                }
                else
                    return this.comboBox.Text;
            }
        }

        public System.Data.DataRow SelectedRow
        {
            get
            {
                if (this._dtList.Columns.Count > 1)
                {
                    if (this.comboBox.SelectedItem == null)
                        return null;
                    try
                    {
                        if (this.comboBox.Sorted)
                        {
                            System.Data.DataRowView R = (System.Data.DataRowView)this.comboBox.SelectedItem;
                            return R.Row;
                        }
                    }
                    catch (System.Exception ex) { }
                    return null;
                }
                else
                    return null;
            }
        }

        public string TypedString { get { return this.textBox.Text; } }

        public string String
        {
            get
            {
                if (this.textBox.Text.Length > 0)
                    return this.textBox.Text;
                else return this.comboBox.Text;
            }
        }

        #endregion

        #region Public functions

        public void ReduceInterfaceToCombobox(string Header, bool RestrictToList)
        {
            this.labelHeader.Text = Header;
            this.textBox.Visible = false;
            this.labelTextBox.Visible = false;
            if (RestrictToList) this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Height = 122;
        }

        public void RestrictToListValues()
        {
            this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        #endregion

        #region Private functions
        private void EnableOKbutton(object sender, EventArgs e)
        {
            if (this.comboBox.Text.Length > 0 || this.textBox.Text.Length > 0)
                this.userControlDialogPanel1.buttonOK.Enabled = true;
            else
                this.userControlDialogPanel1.buttonOK.Enabled = false;
        }

        #endregion
    }
}