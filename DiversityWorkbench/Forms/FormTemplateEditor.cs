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
    public partial class FormTemplateEditor : Form
    {

        #region Parameter

        private DiversityWorkbench.TemplateForData _TemplateForData;
        private System.Collections.Generic.List<System.Windows.Forms.Control> _Controls;
        private System.Collections.Generic.List<System.Windows.Forms.CheckBox> _Checkboxlist;
        private System.Data.DataRow _Row;
        private System.Data.DataRow _SourceRow;

        #endregion

        #region Construction

        public FormTemplateEditor(string TableName, System.Data.DataRow R, System.Collections.Generic.List<string> SuppressedColumns, System.Collections.Generic.Dictionary<string, TemplateForDataSourceTable> SourceTables)
        {
            InitializeComponent();
            this._TemplateForData = new TemplateForData(TableName);
            this._TemplateForData.setSuppressedColumns(SuppressedColumns);
            this._TemplateForData.setSourceTables(SourceTables);
            this._SourceRow = R;
            this.initForm();
        }

        public FormTemplateEditor(string TableName, System.Data.DataRow R, System.Collections.Generic.List<string> SuppressedColumns)
        {
            InitializeComponent();
            this._TemplateForData = new TemplateForData(TableName);
            this._TemplateForData.setSuppressedColumns(SuppressedColumns);
            this._SourceRow = R;
            this.initForm();
        }

        public FormTemplateEditor(DiversityWorkbench.TemplateForData Template, System.Data.DataRow R)
        {
            InitializeComponent();
            Bitmap theBitmap = new Bitmap(this.imageListTemplate.Images[0], new Size(16, 16));
            this.Text = "Copy template to data";
            IntPtr Hicon = theBitmap.GetHicon();
            this.Icon = Icon.FromHandle(Hicon);
            this._TemplateForData = Template;
            this._Row = R;
            this.initForm();
        }


        //public FormTemplateEditor(string TableName)
        //{
        //    InitializeComponent();
        //    this._TemplateForData = new TemplateForData(TableName);
        //    this.initForm();
        //}

        #endregion

        #region Form

        private void initForm()
        {
            this.panelColumns.Controls.Clear();
            string NameOfTable = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation(this._TemplateForData.TableName));

            if (this._Row == null)
            {
                if (NameOfTable.Length > 0)
                    this.labelHeader.Text = "Template for " + NameOfTable;
                else
                    this.labelHeader.Text = "Template for table " + this._TemplateForData.TableName;

                this.buttonCheckAll.Visible = false;
                this.buttonCheckNone.Visible = false;
                this.toolStripCopy.Visible = true;

                switch (DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.FillingOption)
                {
                    case "All":
                        this.SetTemplateFillingOption(TemplateForData.FillingOption.All);
                        break;
                    case "OnlyEmpty":
                        this.SetTemplateFillingOption(TemplateForData.FillingOption.OnlyEmpty);
                        break;
                    case "UserSelection":
                        this.SetTemplateFillingOption(TemplateForData.FillingOption.UserSelection);
                        break;
                }
            }
            else
            {
                if (NameOfTable.Length > 0)
                    this.labelHeader.Text = "Copy data from template for " + NameOfTable;
                else
                    this.labelHeader.Text = "Copy data from template for table " + this._TemplateForData.TableName;

                this.buttonCheckAll.Visible = true;
                this.buttonCheckNone.Visible = true;
                this.toolStripCopy.Visible = false;

                System.Windows.Forms.UserControl U = new UserControl();

                System.Windows.Forms.Label LCol = new Label();
                LCol.Text = "Column in table";
                LCol.Width = 200;
                LCol.Dock = DockStyle.Left;
                LCol.TextAlign = ContentAlignment.MiddleCenter;
                U.Controls.Add(LCol);

                System.Windows.Forms.Label LTemplate = new Label();
                LTemplate.Text = "Template values";
                LTemplate.Dock = DockStyle.Fill;
                LTemplate.TextAlign = ContentAlignment.MiddleCenter;
                U.Controls.Add(LTemplate);

                System.Windows.Forms.Label LData = new Label();
                LData.Text = "Data in database";
                LData.Width = 200;
                LData.Dock = DockStyle.Right;
                LData.TextAlign = ContentAlignment.MiddleCenter;
                U.Controls.Add(LData);

                LTemplate.BringToFront();

                U.Height = 24;
                this.panelColumns.Controls.Add(U);
                U.Dock = DockStyle.Top;
                U.BringToFront();

            }
            this._Controls = new List<Control>();

            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._TemplateForData.TableColums())
            {
                string TemplateValue = "";
                string TemplateColumn = this._TemplateForData.TableName + "|" + KV.Key + "|";
                foreach (string s in DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template)
                {
                    if (s.StartsWith(TemplateColumn))
                        TemplateValue = s.Substring(TemplateColumn.Length);
                }
                if (this._Row != null && (!this._Row.Table.Columns.Contains(KV.Key) || TemplateValue.Trim() == this._Row[KV.Key].ToString().Trim() || TemplateValue.Length == 0))
                    continue;

                System.Windows.Forms.UserControl U = new UserControl();
                string NameOfColumn = DiversityWorkbench.Entity.EntityContent(DiversityWorkbench.Entity.EntityInformationField.DisplayText, DiversityWorkbench.Entity.EntityInformation(this._TemplateForData.TableName + "." + KV.Key));
                if (NameOfColumn.Length == 0)
                    NameOfColumn = KV.Key;
                U.Tag = KV.Key;
                if (this._Row == null)
                {
                    System.Windows.Forms.Label L = new Label();
                    L.Width = 200;
                    L.Text = NameOfColumn;// KV.Key;
                    L.Dock = DockStyle.Left;
                    L.TextAlign = ContentAlignment.MiddleRight;
                    U.Controls.Add(L);
                }
                else
                {
                    System.Windows.Forms.CheckBox C = new CheckBox();
                    C.Width = 200;
                    C.Text = NameOfColumn;// KV.Key;
                    C.Dock = DockStyle.Left;
                    C.Tag = KV.Key;
                    C.TextAlign = ContentAlignment.MiddleRight;
                    U.Controls.Add(C);
                    if (this._Checkboxlist == null)
                        this._Checkboxlist = new List<CheckBox>();
                    this._Checkboxlist.Add(C);
                }
                if (this._TemplateForData.ColumsWithForeignRelations.ContainsKey(KV.Key))
                {
                    if (this._TemplateForData.ColumsWithForeignRelations[KV.Key].EndsWith("_Enum"))
                    {
                        System.Windows.Forms.ComboBox CB = new ComboBox();
                        DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(CB, this._TemplateForData.ColumsWithForeignRelations[KV.Key], DiversityWorkbench.Settings.Connection, true, false, false);
                        CB.Tag = KV.Key;
                        CB.Dock = DockStyle.Fill;
                        CB.DropDownStyle = ComboBoxStyle.DropDownList;
                        if (TemplateValue.Length > 0)
                        {
                            System.Data.DataTable dt = (System.Data.DataTable)CB.DataSource;
                            CB.BindingContext = this.BindingContext;
                            int index = CB.FindString(TemplateValue);
                            CB.SelectedIndex = index;
                        }
                        U.Controls.Add(CB);
                        this._Controls.Add(CB);
                        CB.BringToFront();

                        if (this._Row != null)
                        {
                            System.Windows.Forms.ComboBox CbRow = new ComboBox();
                            DiversityWorkbench.EnumTable.SetEnumTableAsDatasource(CbRow, this._TemplateForData.ColumsWithForeignRelations[KV.Key], DiversityWorkbench.Settings.Connection, true, false, false);
                            CbRow.Width = 200;
                            CbRow.Dock = DockStyle.Right;
                            if (this._Row[KV.Key].ToString().Length > 0)
                            {
                                System.Data.DataTable dt = (System.Data.DataTable)CbRow.DataSource;
                                CbRow.BindingContext = this.BindingContext;
                                int index = CbRow.FindString(this._Row[KV.Key].ToString());
                                CbRow.SelectedIndex = index;
                            }
                            U.Controls.Add(CbRow);
                            CbRow.BringToFront();
                            CbRow.Enabled = false;
                            CB.Enabled = false;
                        }
                        CB.BringToFront();
                    }
                    else if (this._TemplateForData.SourceTables().ContainsKey(KV.Key))
                    {
                        System.Windows.Forms.ComboBox CB = new ComboBox();
                        CB.DataSource = this._TemplateForData.SourceTables()[KV.Key].SourceTable;
                        CB.DisplayMember = this._TemplateForData.SourceTables()[KV.Key].DisplayColumn;
                        CB.ValueMember = this._TemplateForData.SourceTables()[KV.Key].ValueColumn;
                        CB.Tag = KV.Key;
                        CB.DropDownStyle = ComboBoxStyle.DropDownList;
                        CB.Dock = DockStyle.Fill;
                        if (TemplateValue.Length > 0)
                        {
                            System.Data.DataTable dt = (System.Data.DataTable)CB.DataSource;
                            CB.BindingContext = this.BindingContext;
                            int i = 0;
                            foreach (System.Data.DataRow R in dt.Rows)
                            {
                                if (R[CB.ValueMember].ToString() == TemplateValue)
                                {
                                    CB.SelectedIndex = i;
                                    break;
                                }
                                i++;
                            }
                        }
                        U.Controls.Add(CB);
                        this._Controls.Add(CB);
                        CB.BringToFront();

                        if (this._Row != null)
                        {
                            System.Windows.Forms.ComboBox CbRow = new ComboBox();
                            CbRow.DataSource = this._TemplateForData.SourceTables()[KV.Key].SourceTable.Copy();
                            CbRow.DisplayMember = this._TemplateForData.SourceTables()[KV.Key].DisplayColumn;
                            CbRow.ValueMember = this._TemplateForData.SourceTables()[KV.Key].ValueColumn;
                            CbRow.Width = 200;
                            CbRow.Dock = DockStyle.Right;
                            if (this._Row[KV.Key].ToString().Length > 0)
                            {
                                System.Data.DataTable dt = (System.Data.DataTable)CbRow.DataSource;
                                CbRow.BindingContext = this.BindingContext;
                                int i = 0;
                                foreach (System.Data.DataRow R in dt.Rows)
                                {
                                    if (R[CbRow.ValueMember].ToString() == this._Row[KV.Key].ToString())
                                    {
                                        CbRow.SelectedIndex = i;
                                        break;
                                    }
                                    i++;
                                }
                            }
                            else CbRow.SelectedIndex = -1;

                            U.Controls.Add(CbRow);
                            CbRow.BringToFront();
                            CB.Enabled = false;
                            CbRow.Enabled = false;
                        }
                        CB.BringToFront();
                    }
                }
                else
                {
                    switch (KV.Value)
                    {
                        case "datetime2":
                        case "datetime":
                        case "smalldatetime":
                            System.Windows.Forms.Label Ldt = new Label();
                            Ldt.Dock = DockStyle.Fill;
                            Ldt.TextAlign = ContentAlignment.MiddleRight;
                            Ldt.Tag = KV.Key;
                            System.DateTime dt;
                            if (System.DateTime.TryParse(TemplateValue, out dt))
                            {
                                Ldt.Text = dt.Year.ToString() + "-";
                                if (dt.Month < 10)
                                    Ldt.Text += "0";
                                Ldt.Text += dt.Month.ToString() + "-";
                                if (dt.Day < 10)
                                    Ldt.Text += "0";
                                Ldt.Text += dt.Day.ToString();
                            }
                            U.Controls.Add(Ldt);

                            if (this._Row == null)
                            {
                                System.Windows.Forms.DateTimePicker DT = new DateTimePicker();
                                DT.Dock = DockStyle.Right;
                                DT.Width = 15;
                                DT.Format = DateTimePickerFormat.Custom;
                                DT.CustomFormat = "";
                                U.Controls.Add(DT);
                                DT.BringToFront();
                                DT.Tag = Ldt;
                                DT.CloseUp += new System.EventHandler(this.dateTimePicker_CloseUp);
                            }

                            this._Controls.Add(Ldt);

                            if (this._Row != null)
                            {
                                System.Windows.Forms.Label LdtRow = new Label();
                                LdtRow.Dock = DockStyle.Right;
                                LdtRow.TextAlign = ContentAlignment.MiddleRight;
                                LdtRow.Width = 200;
                                System.DateTime dtRow;
                                if (System.DateTime.TryParse(this._Row[KV.Key].ToString(), out dtRow))
                                {
                                    LdtRow.Text = dtRow.Year.ToString() + "-";
                                    if (dtRow.Month < 10)
                                        LdtRow.Text += "0";
                                    LdtRow.Text += dt.Month.ToString() + "-";
                                    if (dtRow.Day < 10)
                                        LdtRow.Text += "0";
                                    LdtRow.Text += dtRow.Day.ToString();
                                }
                                U.Controls.Add(LdtRow);
                            }

                            Ldt.BringToFront();

                            break;
                        case "xxx":
                            System.Windows.Forms.MaskedTextBox MTd = new MaskedTextBox();
                            MTd.Mask = "0000/00/00";// 90:00";
                            MTd.ValidatingType = typeof(System.DateTime);
                            //System.Windows.Forms.DateTimePicker DT = new DateTimePicker();
                            MTd.Dock = DockStyle.Fill;
                            U.Controls.Add(MTd);
                            MTd.Tag = KV.Key;
                            System.DateTime d;
                            if (System.DateTime.TryParse(TemplateValue, out d))
                            {
                                MTd.Text = d.Year.ToString() + "/";
                                if (d.Month < 10)
                                    MTd.Text += "0";
                                MTd.Text += d.Month.ToString() + "/";
                                if (d.Day < 10)
                                    MTd.Text += "0";
                                MTd.Text += d.Day.ToString();
                            }
                            this._Controls.Add(MTd);

                            if (this._Row != null)
                            {
                                System.Windows.Forms.MaskedTextBox MTdRow = new MaskedTextBox();
                                MTdRow.Mask = "0000/00/00";// 90:00";
                                MTdRow.ValidatingType = typeof(System.DateTime);
                                //System.Windows.Forms.DateTimePicker DT = new DateTimePicker();
                                MTdRow.Dock = DockStyle.Right;
                                U.Controls.Add(MTdRow);
                                MTdRow.BringToFront();
                                System.DateTime dRow;
                                if (System.DateTime.TryParse(this._Row[KV.Key].ToString(), out dRow))
                                {
                                    MTdRow.Text = dRow.Year.ToString() + "/";
                                    if (dRow.Month < 10)
                                        MTdRow.Text += "0";
                                    MTdRow.Text += dRow.Month.ToString() + "/";
                                    if (dRow.Day < 10)
                                        MTdRow.Text += "0";
                                    MTdRow.Text += dRow.Day.ToString();
                                }
                            }

                            MTd.BringToFront();

                            break;
                        case "tinyint":
                            System.Windows.Forms.MaskedTextBox MTt = new MaskedTextBox();
                            MTt.Mask = "000";
                            MTt.Dock = DockStyle.Fill;
                            U.Controls.Add(MTt);
                            MTt.Tag = KV.Key;
                            MTt.Text = TemplateValue;
                            this._Controls.Add(MTt);

                            if (this._Row != null)
                            {
                                System.Windows.Forms.MaskedTextBox MTtRow = new MaskedTextBox();
                                MTtRow.Mask = "000";
                                MTtRow.Width = 200;
                                MTtRow.Dock = DockStyle.Right;
                                U.Controls.Add(MTtRow);
                                MTtRow.BringToFront();
                                MTtRow.Text = this._Row[KV.Key].ToString();
                                MTt.ReadOnly = true;
                                MTtRow.ReadOnly = true;
                            }
                            MTt.BringToFront();

                            break;
                        case "smallint":
                            System.Windows.Forms.MaskedTextBox MTs = new MaskedTextBox();
                            MTs.Mask = "00000";
                            MTs.Dock = DockStyle.Fill;
                            U.Controls.Add(MTs);
                            MTs.Tag = KV.Key;
                            MTs.Text = TemplateValue;
                            this._Controls.Add(MTs);

                            if (this._Row != null)
                            {
                                System.Windows.Forms.MaskedTextBox MTsRow = new MaskedTextBox();
                                MTsRow.Mask = "00000";
                                MTsRow.Width = 200;
                                MTsRow.Dock = DockStyle.Right;
                                U.Controls.Add(MTsRow);
                                MTsRow.BringToFront();
                                MTsRow.Text = this._Row[KV.Key].ToString();
                                MTs.ReadOnly = true;
                                MTsRow.ReadOnly = true;
                            }

                            MTs.BringToFront();

                            break;
                        case "int":
                            System.Windows.Forms.MaskedTextBox MTi = new MaskedTextBox();
                            MTi.Mask = "00000000";
                            MTi.Dock = DockStyle.Fill;
                            U.Controls.Add(MTi);
                            MTi.BringToFront();
                            MTi.Tag = KV.Key;
                            MTi.Text = TemplateValue;
                            this._Controls.Add(MTi);

                            if (this._Row != null)
                            {
                                System.Windows.Forms.MaskedTextBox MTiRow = new MaskedTextBox();
                                MTiRow.Mask = "00000000";
                                MTiRow.Dock = DockStyle.Right;
                                MTiRow.Width = 200;
                                U.Controls.Add(MTiRow);
                                MTiRow.BringToFront();
                                MTiRow.Text = this._Row[KV.Key].ToString();
                                MTi.ReadOnly = true;
                                MTiRow.ReadOnly = true;
                            }

                            MTi.BringToFront();
                            break;
                        case "float":
                            System.Windows.Forms.MaskedTextBox MTf = new MaskedTextBox();
                            MTf.Mask = "00000.000";
                            MTf.Dock = DockStyle.Fill;
                            U.Controls.Add(MTf);
                            MTf.Tag = KV.Key;
                            MTf.Text = TemplateValue;
                            this._Controls.Add(MTf);

                            if (this._Row != null)
                            {
                                System.Windows.Forms.MaskedTextBox MTfRow = new MaskedTextBox();
                                MTfRow.Mask = "00000.000";
                                MTfRow.Dock = DockStyle.Right;
                                MTfRow.Width = 200;
                                U.Controls.Add(MTfRow);
                                MTfRow.BringToFront();
                                MTfRow.Text = this._Row[KV.Key].ToString();
                                MTf.ReadOnly = true;
                                MTfRow.ReadOnly = true;
                            }

                            MTf.BringToFront();

                            break;
                        case "varchar":
                        case "nvarchar":
                            System.Windows.Forms.TextBox T = new TextBox();
                            T.Dock = DockStyle.Fill;
                            U.Controls.Add(T);
                            T.BringToFront();
                            T.Tag = KV.Key;
                            T.Text = TemplateValue;
                            this._Controls.Add(T);
                            if (this._Row != null)
                            {
                                System.Windows.Forms.TextBox Trow = new TextBox();
                                Trow.Dock = DockStyle.Right;
                                Trow.Width = 200;
                                U.Controls.Add(Trow);
                                Trow.BringToFront();
                                Trow.Text = this._Row[KV.Key].ToString();
                                T.ReadOnly = true;
                                Trow.ReadOnly = true;
                            }
                            T.BringToFront();
                            break;
                        case "bit":
                            System.Windows.Forms.CheckBox CB = new CheckBox();
                            CB.Dock = DockStyle.Fill;
                            CB.Text = "";
                            CB.ThreeState = true;
                            U.Controls.Add(CB);
                            CB.Tag = KV.Key;
                            bool OK;
                            if (bool.TryParse(TemplateValue, out OK))
                                CB.Checked = OK;
                            else
                                CB.CheckState = CheckState.Indeterminate;
                            this._Controls.Add(CB);

                            if (this._Row != null)
                            {
                                System.Windows.Forms.CheckBox CBrow = new CheckBox();
                                CBrow.Dock = DockStyle.Right;
                                CBrow.Text = "";
                                CBrow.ThreeState = true;
                                U.Controls.Add(CBrow);
                                CBrow.BringToFront();
                                bool OKRow;
                                if (bool.TryParse(TemplateValue, out OKRow))
                                    CBrow.Checked = OKRow;
                                else
                                    CBrow.CheckState = CheckState.Indeterminate;
                                CB.Enabled = false;
                                CBrow.Enabled = false;
                            }

                            CB.BringToFront();

                            break;
                        default:
                            break;
                    }
                }
                U.Height = 24;
                this.panelColumns.Controls.Add(U);
                U.Dock = DockStyle.Top;
                U.BringToFront();
            }
        }

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        private void FormTemplateEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._Row != null)
                return;
            if (this.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                this.SaveTemplateFromForm();
                //string TemplateTable = this._TemplateForData.TableName + "|";
                //System.Collections.Generic.List<string> TemplatesToRemove = new List<string>();
                //for (int i = 0; i < DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template.Count; i++)
                //{
                //    if (DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template[i].StartsWith(TemplateTable))
                //        TemplatesToRemove.Add(DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template[i]);
                //}
                //foreach (string s in TemplatesToRemove)
                //{
                //    DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template.Remove(s);
                //}
                //string TemplateEntry = "";
                //foreach (System.Windows.Forms.Control C in this._Controls)
                //{
                //    TemplateEntry = "";
                //    if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                //    {
                //        System.Windows.Forms.ComboBox CB = (System.Windows.Forms.ComboBox)C;
                //        if (CB.SelectedIndex > 0)
                //        {
                //            System.Data.DataRowView R = (System.Data.DataRowView)CB.SelectedItem;
                //            if (R[0].ToString().Length > 0)
                //                TemplateEntry = TemplateTable + C.Tag.ToString() + "|" + R[0].ToString();
                //        }
                //        else if (CB.Text.Length > 0)
                //            TemplateEntry = TemplateTable + C.Tag.ToString() + "|" + CB.Text; ;
                //    }
                //    else if (C.GetType() == typeof(System.Windows.Forms.MaskedTextBox))
                //    {
                //        System.Windows.Forms.MaskedTextBox M = (System.Windows.Forms.MaskedTextBox)C;
                //        if (M.Text.Length > 0)
                //            TemplateEntry = TemplateTable + C.Tag.ToString() + "|" + M.Text;
                //    }
                //    else if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                //    {
                //        System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)C;
                //        if (T.Text.Length > 0)
                //            TemplateEntry = TemplateTable + C.Tag.ToString() + "|" + T.Text;
                //    }
                //    else if (C.GetType() == typeof(System.Windows.Forms.Label))
                //    {
                //        System.Windows.Forms.Label L = (System.Windows.Forms.Label)C;
                //        if (L.Text.Length > 0)
                //            TemplateEntry = TemplateTable + C.Tag.ToString() + "|" + L.Text;
                //    }
                //    if (TemplateEntry.Length > 0)
                //    {
                //        DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template.Add(TemplateEntry);
                //    }
                //}
                //DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Save();
            }
        }

        private void SaveTemplateFromForm()
        {
            string TemplateTable = this._TemplateForData.TableName + "|";
            System.Collections.Generic.List<string> TemplatesToRemove = new List<string>();
            for (int i = 0; i < DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template.Count; i++)
            {
                if (DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template[i].StartsWith(TemplateTable))
                    TemplatesToRemove.Add(DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template[i]);
            }
            foreach (string s in TemplatesToRemove)
            {
                DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template.Remove(s);
            }
            string TemplateEntry = "";
            string TemplateValue = "";
            foreach (System.Windows.Forms.Control C in this._Controls)
            {
                TemplateEntry = "";
                TemplateValue = "";
                if (C.GetType() == typeof(System.Windows.Forms.ComboBox))
                {
                    System.Windows.Forms.ComboBox CB = (System.Windows.Forms.ComboBox)C;
                    if (CB.SelectedIndex > 0)
                    {
                        System.Data.DataRowView R = (System.Data.DataRowView)CB.SelectedItem;
                        if (R[0].ToString().Length > 0)
                        {
                            TemplateValue = R[0].ToString();
                        }
                    }
                    else if (CB.Text.Length > 0)
                        TemplateValue = CB.Text; ;
                }
                else if (C.GetType() == typeof(System.Windows.Forms.MaskedTextBox))
                {
                    System.Windows.Forms.MaskedTextBox M = (System.Windows.Forms.MaskedTextBox)C;
                    if (M.Text.Length > 0)
                        TemplateValue = M.Text;
                }
                else if (C.GetType() == typeof(System.Windows.Forms.TextBox))
                {
                    System.Windows.Forms.TextBox T = (System.Windows.Forms.TextBox)C;
                    if (T.Text.Length > 0)
                        TemplateValue = T.Text;
                }
                else if (C.GetType() == typeof(System.Windows.Forms.Label))
                {
                    System.Windows.Forms.Label L = (System.Windows.Forms.Label)C;
                    if (L.Text.Length > 0)
                        TemplateValue = L.Text;
                }

                if (TemplateValue.Length > 0)
                    TemplateEntry = TemplateTable + C.Tag.ToString() + "|" + TemplateValue;

                if (TemplateEntry.Length > 0)
                {
                    DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template.Add(TemplateEntry);

                    if (!this._TemplateForData.ColumnTemplateValues().ContainsKey(C.Tag.ToString()))
                    {
                        this._TemplateForData.ColumnTemplateValues().Add(C.Tag.ToString(), TemplateValue);
                    }
                    else if (this._TemplateForData.ColumnTemplateValues()[C.Tag.ToString()] != TemplateValue
                        && TemplateValue.Length > 0)
                    {
                        this._TemplateForData.ColumnTemplateValues()[C.Tag.ToString()] = TemplateValue;
                    }
                }
                else if (this._TemplateForData.ColumnTemplateValues().ContainsKey(C.Tag.ToString()))
                    this._TemplateForData.ColumnTemplateValues().Remove(C.Tag.ToString());
            }
            DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Save();

        }

        private void SaveTemplate()
        {
            // getting the name of the table
            string TemplateTable = this._TemplateForData.TableName + "|";

            // removing present entries for this table
            System.Collections.Generic.List<string> TemplatesToRemove = new List<string>();
            for (int i = 0; i < DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template.Count; i++)
            {
                if (DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template[i].StartsWith(TemplateTable))
                    TemplatesToRemove.Add(DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template[i]);
            }
            foreach (string s in TemplatesToRemove)
            {
                DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template.Remove(s);
            }

            // writing the new entries
            string TemplateEntry = "";
            foreach (System.Collections.Generic.KeyValuePair<string, string> KV in this._TemplateForData.ColumnTemplateValues())
            {
                TemplateEntry = TemplateTable + KV.Key + "|" + KV.Value;
                DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Template.Add(TemplateEntry);
            }

            // saving the changes
            DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Save();
        }

        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            System.Windows.Forms.DateTimePicker DTP = (System.Windows.Forms.DateTimePicker)sender;
            System.Windows.Forms.Label L = (System.Windows.Forms.Label)DTP.Tag;
            L.Text = DTP.Value.Year.ToString() + "-";
            if (DTP.Value.Month < 10) L.Text += "0";
            L.Text += DTP.Value.Month.ToString() + "-";
            if (DTP.Value.Day < 10) L.Text += "0";
            L.Text += DTP.Value.Day.ToString();
        }

        private void toolStripButtonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Name,
                System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(),
                null,
                null);
        }

        #endregion

        #region Interface

        public System.Collections.Generic.List<string> SelectedColumns()
        {
            System.Collections.Generic.List<string> L = new List<string>();
            if (this._Checkboxlist != null)
            {
                foreach (System.Windows.Forms.CheckBox CB in this._Checkboxlist)
                {
                    if (CB.Checked)
                        L.Add(CB.Tag.ToString());
                }
            }
            return L;
        }

        #endregion

        #region Selection of User descisions

        private void buttonCheckNone_Click(object sender, EventArgs e)
        {
            if (this._Checkboxlist != null)
            {
                foreach (System.Windows.Forms.CheckBox C in this._Checkboxlist)
                    C.Checked = false;
            }
        }

        private void buttonCheckAll_Click(object sender, EventArgs e)
        {
            if (this._Checkboxlist != null)
            {
                foreach (System.Windows.Forms.CheckBox C in this._Checkboxlist)
                    C.Checked = true;
            }
        }

        #endregion

        #region Save, clear or copy values from the data

        private void copyAllValuesIntoTheTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._TemplateForData.ColumnTemplateValues().Clear();
            foreach (System.Data.DataColumn C in this._SourceRow.Table.Columns)
            {
                //if (this._TemplateForData.ColumnTemplateValues().ContainsKey(C.ColumnName))
                //    this._TemplateForData.ColumnTemplateValues().Remove(C.ColumnName);
                if (this._TemplateForData.TableColums().ContainsKey(C.ColumnName))
                {
                    if (this._SourceRow[C.ColumnName].Equals(System.DBNull.Value))
                        this._TemplateForData.ColumnTemplateValues().Add(C.ColumnName, "");
                    else
                        this._TemplateForData.ColumnTemplateValues().Add(C.ColumnName, this._SourceRow[C.ColumnName].ToString());
                }
            }
            this.SaveTemplate();
            this.initForm();
        }

        private void copyOnlyNotEmptyValuesIntoTheTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (System.Data.DataColumn C in this._SourceRow.Table.Columns)
            {
                if (!this._TemplateForData.ColumnTemplateValues().ContainsKey(C.ColumnName)
                    && this._TemplateForData.TableColums().ContainsKey(C.ColumnName)
                    && !this._SourceRow[C.ColumnName].Equals(System.DBNull.Value)
                    && this._SourceRow[C.ColumnName].ToString().Length > 0)
                {
                    this._TemplateForData.ColumnTemplateValues().Add(C.ColumnName, this._SourceRow[C.ColumnName].ToString());
                }
            }
            this.SaveTemplate();
            this.initForm();
        }

        private void copyOnlyMissingValuesIntoTheTemplateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (System.Data.DataColumn C in this._SourceRow.Table.Columns)
            {
                if (!this._TemplateForData.ColumnTemplateValues().ContainsKey(C.ColumnName)
                    && this._TemplateForData.TableColums().ContainsKey(C.ColumnName)
                    && !this._SourceRow[C.ColumnName].Equals(System.DBNull.Value)
                    && this._SourceRow[C.ColumnName].ToString().Length > 0)
                {
                    this._TemplateForData.ColumnTemplateValues().Add(C.ColumnName, this._SourceRow[C.ColumnName].ToString());
                }
            }
            this.SaveTemplate();
            this.initForm();
        }

        private void toolStripButtonClearTemplate_Click(object sender, EventArgs e)
        {
            this._TemplateForData.ColumnTemplateValues().Clear();
            //foreach(System.Collections.Generic.KeyValuePair<string, string> KV in this._TemplateForData.TableColums())
            //{
            //    this._TemplateForData.ColumnTemplateValues()[KV.Key] = "";
            //}
            this.SaveTemplate();
            this.initForm();
        }

        private void toolStripButtonSaveTemplate_Click(object sender, EventArgs e)
        {
            this.SaveTemplateFromForm();
            this.initForm();
        }

        #endregion

        #region Setting the copy option

        private void copyOnlyInEmptyFieldsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetTemplateFillingOption(TemplateForData.FillingOption.OnlyEmpty);
            this.SaveTemplateFillingOption(TemplateForData.FillingOption.OnlyEmpty);
        }

        private void askIfFieldsContainDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetTemplateFillingOption(TemplateForData.FillingOption.UserSelection);
            this.SaveTemplateFillingOption(TemplateForData.FillingOption.UserSelection);
        }

        private void copyWholeTemplateContentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SetTemplateFillingOption(TemplateForData.FillingOption.All);
            this.SaveTemplateFillingOption(TemplateForData.FillingOption.All);
        }

        private void SetTemplateFillingOption(DiversityWorkbench.TemplateForData.FillingOption F)
        {
            switch (F)
            {
                case TemplateForData.FillingOption.All:
                    this.copyOnlyInEmptyFieldsToolStripMenuItem.Image = null;
                    this.copyWholeTemplateContentToolStripMenuItem.Image = this.imageListTemplate.Images[2];
                    this.askIfFieldsContainDataToolStripMenuItem.Image = null;
                    break;
                case TemplateForData.FillingOption.OnlyEmpty:
                    this.copyOnlyInEmptyFieldsToolStripMenuItem.Image = this.imageListTemplate.Images[2];
                    this.copyWholeTemplateContentToolStripMenuItem.Image = null;
                    this.askIfFieldsContainDataToolStripMenuItem.Image = null;
                    break;
                case TemplateForData.FillingOption.UserSelection:
                    this.copyOnlyInEmptyFieldsToolStripMenuItem.Image = null;
                    this.copyWholeTemplateContentToolStripMenuItem.Image = null;
                    this.askIfFieldsContainDataToolStripMenuItem.Image = this.imageListTemplate.Images[2];
                    break;
            }
        }

        private void SaveTemplateFillingOption(DiversityWorkbench.TemplateForData.FillingOption F)
        {
            DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.FillingOption = F.ToString();
            DiversityWorkbench.Forms.FormTemplateEditorSettings.Default.Save();
        }

        #endregion

    }
}
