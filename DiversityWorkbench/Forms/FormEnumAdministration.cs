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
    public partial class FormEnumAdministration : Form
    {

        #region Parameter and Properties

        private string _TableName = "";
        private string _Restriction = "";
        private System.Data.DataTable _DtEnum;
        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlAdapter;
        private System.Windows.Forms.BindingSource _BindingSource;

        private System.Drawing.Image _Image;

        private System.Collections.Generic.List<string> _LockedList = new List<string>();
        private string _SqlKeyRetrieval = "";

        private bool _DataHaveBeenChanged = false;

        public bool DataHaveBeenChanged
        {
            get { return _DataHaveBeenChanged; }
            //set { _DataHaveBeenChanged = value; }
        }

        private string _KeyColumn = "Code";
        private string _ParentKeyColumn = "ParentCode";
        private string _DisplayColumn = "DisplayText";

        private bool _EnableEntityDescriptions = false;

        private bool _HierarchyChangesEnabled = true;
        /// <summary>
        /// If changes in the hierarchy are possible. If a fixed parent code is provided this is not the case
        /// </summary>
        public bool HierarchyChangesEnabled
        {
            set
            {
                this._HierarchyChangesEnabled = value;
                this.toolStripButtonSetParent.Visible = this._HierarchyChangesEnabled;
            }
        }

        private string _FixedParentCode = "";
        /// <summary>
        /// If there is a fixed code for the parent, e.g. External identifier where all added entries are a child of this code
        /// </summary>
        public string FixedParentCode
        {
            //get { return _FixedParentCode; }
            set
            {
                _FixedParentCode = value;
                if (_FixedParentCode.Length > 0)
                {
                    this._HierarchyChangesEnabled = false;
                    this.buildTree();
                }
            }
        }

        private System.Collections.Generic.List<string> _ParentCodes;

        //private System.Collections.Generic.Dictionary<string, System.Drawing.Image> _Images;

        public System.Collections.Generic.Dictionary<string, System.Drawing.Image> Images
        {
            get
            {
                System.Collections.Generic.Dictionary<string, System.Drawing.Image> ii = new Dictionary<string, Image>();
                foreach (System.Collections.Generic.KeyValuePair<string, System.Collections.Generic.Dictionary<int, System.Drawing.Image>> KV in this.ImageDict)
                {
                    ii.Add(KV.Key, KV.Value.First().Value);
                }
                return ii;
                //return _Images;
            }
            set
            {
                this.imageList = new ImageList();
                this._ImageDict = new Dictionary<string, Dictionary<int, Image>>();
                foreach (System.Collections.Generic.KeyValuePair<string, System.Drawing.Image> KV in value)
                {
                    if (KV.Value != null)
                    {
                        int Count = this.ImageDict.Count;
                        System.Collections.Generic.Dictionary<int, System.Drawing.Image> dd = new Dictionary<int, Image>();
                        dd.Add(Count, KV.Value);
                        this.ImageDict.Add(KV.Key, dd);
                        this.imageList.Images.Add(KV.Key, KV.Value);
                    }
                    else
                    {
                        System.Data.DataTable dt = new DataTable();
                        string SQL = "SELECT [Icon]  FROM [dbo].[" + this._TableName + "] WHERE [" + this._KeyColumn + "] = '" + KV.Key + "' AND NOT Icon IS NULL ";
                        using (Microsoft.Data.SqlClient.SqlConnection con = new Microsoft.Data.SqlClient.SqlConnection(DiversityWorkbench.Settings.ConnectionString))
                        {
                            con.Open();
                            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt, con);
                            con.Close();
                        }
                        if (dt.Rows.Count > 0)
                        {
                            System.Byte[] B = (System.Byte[])dt.Rows[0]["Icon"];
                            System.IO.MemoryStream ms = new System.IO.MemoryStream(B);
                            System.Drawing.Image I = System.Drawing.Image.FromStream(ms);
                            System.Drawing.Bitmap BM = new Bitmap(I, 16, 16);
                            System.Drawing.Bitmap BG = DiversityWorkbench.Forms.FormFunctions.MakeGrayscale3(BM);
                            BM.MakeTransparent();

                            int Count = this.ImageDict.Count;
                            System.Collections.Generic.Dictionary<int, System.Drawing.Image> dd = new Dictionary<int, Image>();
                            dd.Add(Count, (System.Drawing.Image)BM);
                            this.ImageDict.Add(KV.Key, dd);
                            this.imageList.Images.Add(KV.Key, (System.Drawing.Image)BM);
                        }
                    }
                }
                //_Images = value;
            }
        }

        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, System.Drawing.Image>> _ImageDict;
        public System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, System.Drawing.Image>> ImageDict
        {
            get { if (_ImageDict == null) _ImageDict = new Dictionary<string, Dictionary<int, Image>>(); return _ImageDict; }
            set { _ImageDict = value; }
        }

        //private System.Collections.Generic.Dictionary<string, int> _ImageIndex;
        //private System.Collections.Generic.Dictionary<string, int> _ImagesAllIndex;

        private bool _ShowImage = false;
        /// <summary>
        /// If the images should be shown including
        /// </summary>
        public bool ShowImage
        {
            get { return _ShowImage; }
            set
            {
                _ShowImage = value;
                this.pictureBoxImage.Visible = this._ShowImage;
                this.buttonGetImage.Visible = this._ShowImage;
            }
        }

        System.IO.DirectoryInfo _ImageDirectory;

        private System.Collections.Generic.List<string> _BlockedModuleTypes;
        private System.Collections.Generic.List<string> _DoNotDelete;

        #endregion

        #region Construction

        /// <summary>
        /// Administration of standard enumeration tables
        /// </summary>
        /// <param name="Image">The icon for the window if available</param>
        /// <param name="TableName">The name of the table</param>
        /// <param name="Title">The title of the window</param>
        /// <param name="Restriction">The restriction of the entries that should be included</param>
        public FormEnumAdministration(System.Drawing.Image Image, string TableName, string Title, string Restriction)
        {
            InitializeComponent();
            try
            {
                if (Image != null)
                {
                    this._Image = Image;
                    Bitmap bmp = (Bitmap)Image;
                    Bitmap newBmp = new Bitmap(bmp);
                    Bitmap targetBmp = newBmp.Clone(new Rectangle(0, 0, newBmp.Width, newBmp.Height), System.Drawing.Imaging.PixelFormat.Format64bppArgb);
                    IntPtr Hicon = targetBmp.GetHicon();
                    Icon myIcon = Icon.FromHandle(Hicon);
                    this.Icon = myIcon;
                }
                else
                    this.ShowIcon = false;
                this._TableName = TableName;
                if (Title.Length > 0)
                    this.Text = Title;
                else
                    this.Text = "Administration of " + this._TableName;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            this._Restriction = Restriction;
            this.initForm();
        }

        public FormEnumAdministration(System.Drawing.Image Image, string TableName, string Title, string Restriction, System.Collections.Generic.Dictionary<string, System.Drawing.Image> Images)//, System.Collections.Generic.Dictionary<string, System.Drawing.Image> ImagesAll = null, System.IO.DirectoryInfo ImageDirectory)
            : this(Image, TableName, Title, Restriction)
        {
            try
            {
                string SQL = "SELECT COUNT(*) FROM Information_Schema.Columns T WHERE T.Table_Name = '" + this._TableName + "' AND T.Column_Name = 'Icon'";
                string Result = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
                if (Result == "1")
                    this.ShowImage = true;
                else
                    this.ShowImage = false;

                this.Images = Images;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            this.treeViewEnum.ImageList = this.imageList;
            this.buildTree();
        }


        public FormEnumAdministration(System.Drawing.Image Image, string TableName, string KeyColumn, string ParentColumn, string DisplayColumn, string Title, bool EnableEntityDescriptions, string FixedParentCode, System.Collections.Generic.List<string> ParentCodes)
        {
            InitializeComponent();
            try
            {
                if (Image != null)
                {
                    this._Image = Image;
                    Bitmap bmp = (Bitmap)Image;
                    Bitmap newBmp = new Bitmap(bmp);
                    Bitmap targetBmp = newBmp.Clone(new Rectangle(0, 0, newBmp.Width, newBmp.Height), System.Drawing.Imaging.PixelFormat.Format64bppArgb);
                    IntPtr Hicon = targetBmp.GetHicon();
                    Icon myIcon = Icon.FromHandle(Hicon);
                    this.Icon = myIcon;
                }
                else
                    this.ShowIcon = false;
                this._TableName = TableName;
                this._KeyColumn = KeyColumn;
                this._ParentKeyColumn = ParentColumn;
                this._DisplayColumn = DisplayColumn;
                this._FixedParentCode = FixedParentCode;
                this._ParentCodes = ParentCodes;
                if (Title.Length > 0)
                    this.Text = Title;
                else
                    this.Text = "Administration of " + this._TableName;
                this._Restriction = "";
                this._EnableEntityDescriptions = EnableEntityDescriptions;
                this.tableLayoutPanelRepresentation.Visible = EnableEntityDescriptions;
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.initForm();
        }

        #endregion

        #region Form

        private void initForm()
        {

            this.getData();

            try
            {
                this.toolTip.AutoPopDelay = 30000;

                if (this.components == null)
                    this.components = new System.ComponentModel.Container();

                this._BindingSource = new System.Windows.Forms.BindingSource(this.components);
                ((System.ComponentModel.ISupportInitialize)(this._BindingSource)).BeginInit();
                this._BindingSource.DataSource = this._DtEnum;

                // Key column
                this.labelCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, this._KeyColumn, true));
                if (this._KeyColumn == this._DisplayColumn)
                {
                    this.labelDisplayText.Visible = false;
                    this.textBoxDisplayText.Visible = false;
                }

                // Description
                this.textBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, "Description", true));

                // Display Column
                this.textBoxDisplayText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, this._DisplayColumn, true));

                // Abbreviation
                if (this._DtEnum.Columns.Contains("Abbreviation"))
                {
                    this.textBoxAbbreviation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, "Abbreviation", true));
                    this.labelAbbreviation.Visible = true;
                    this.textBoxAbbreviation.Visible = true;
                }

                // Notes
                if (this._DtEnum.Columns.Contains("InternalNotes"))
                    this.textBoxInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, "InternalNotes", true));
                else if (this._DtEnum.Columns.Contains("Notes"))
                    this.textBoxInternalNotes.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, "Notes", true));
                else
                {
                    this.labelInternalNotes.Visible = false;
                    this.textBoxInternalNotes.Visible = false;
                }

                // DisplayEnable
                if (this._DtEnum.Columns.Contains("DisplayEnable"))
                    this.checkBoxDisplayEnable.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this._BindingSource, "DisplayEnable", true));
                else if (this._DtEnum.Columns.Contains("DisplayEnabled"))
                    this.checkBoxDisplayEnable.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this._BindingSource, "DisplayEnabled", true));
                else
                {
                    this.checkBoxDisplayEnable.Visible = false;
                }

                // DisplayOrder
                if (this._DtEnum.Columns.Contains("DisplayOrder"))
                    this.numericUpDownDisplayOrder.DataBindings.Add(new System.Windows.Forms.Binding("Value", this._BindingSource, "DisplayOrder", true));
                else
                {
                    this.numericUpDownDisplayOrder.Visible = false;
                    this.labelDisplayOrder.Visible = false;
                }

                // URL
                if (this._DtEnum.Columns.Contains("URL"))
                {
                    this.textBoxURL.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, "URL", true));
                    this.textBoxURL.Visible = true;
                    this.labelURL.Visible = true;
                }
                else
                {
                    this.textBoxURL.Visible = false;
                    this.labelURL.Visible = false;
                }

                // ParentRelation
                if (this._DtEnum.Columns.Contains("ParentRelation"))
                {
                    this.comboBoxParentRelation.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, "ParentRelation", true));
                    this.comboBoxParentRelation.Visible = true;
                    this.labelParentRelation.Visible = true;
                }
                else
                {
                    this.comboBoxParentRelation.Visible = false;
                    this.labelParentRelation.Visible = false;
                }

                // ModuleName
                if (this._DtEnum.Columns.Contains("ModuleName"))
                {
                    this.comboBoxModuleName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this._BindingSource, "ModuleName", true));
                    this.comboBoxModuleName.Items.Add("");
                    this.comboBoxModuleName.Items.Add("DiversityAgents");
                    this.comboBoxModuleName.Items.Add("DiversityCollection");
                    this.comboBoxModuleName.Items.Add("DiversityGazetteer");
                    this.comboBoxModuleName.Items.Add("DiversityProjects");
                    this.comboBoxModuleName.Items.Add("DiversityReferences");
                    this.comboBoxModuleName.Items.Add("DiversitySamplingPlots");
                    this.comboBoxModuleName.Items.Add("DiversityScientificTerms");
                    this.comboBoxModuleName.Items.Add("DiversityTaxonNames");
                    this.comboBoxModuleName.Visible = true;
                    this.labelModuleName.Visible = true;
                }
                else
                {
                    this.comboBoxModuleName.Visible = false;
                    this.labelModuleName.Visible = false;
                }

                // TableEditor
                // get permissions
                this.toolStripButtonTableEditor.Enabled = DiversityWorkbench.Forms.FormFunctions.Permissions(this._TableName, DiversityWorkbench.Forms.FormFunctions.Permission.DELETE);

                ((System.ComponentModel.ISupportInitialize)(this._BindingSource)).EndInit();

                // Toolstrip
                if (this._FixedParentCode != null && this._FixedParentCode.Length > 0)
                    this.toolStripButtonRemoveParent.Visible = false;

                this.setSupplementControls();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
            this.buildTree();
        }

        private void getData()
        {
            try
            {
                string SQL = "SELECT * FROM " + this._TableName;
                if (this._Restriction.Length > 0)
                    SQL += " WHERE " + this._Restriction;
                this._DtEnum = new DataTable();
                this._SqlAdapter = new Microsoft.Data.SqlClient.SqlDataAdapter(SQL, DiversityWorkbench.Settings.ConnectionString);
                DiversityWorkbench.Forms.FormFunctions.initSqlAdapter(ref this._SqlAdapter, this._DtEnum, SQL, DiversityWorkbench.Settings.ConnectionString);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void FormEnumAdministration_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //System.Data.DataTable dtModified = this._DtEnum.GetChanges();
                //if (dtModified != null)
                //    this._DataHaveBeenChanged = true;
                this._BindingSource.EndEdit();
                this._SqlAdapter.Update(this._DtEnum);
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void buttonFeedback_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Feedback.SendFeedback(System.Reflection.Assembly.GetAssembly(this.GetType()).GetName().Version.ToString(), "", "");
        }

        //public void PreventEditingData()
        //{
        //    this.toolStripList.Visible = false;
        //    this.splitContainerMain.Panel2.Enabled = false;
        //}

        public void SetLockedList(System.Collections.Generic.List<string> LockedList)
        {
            this._LockedList = LockedList;
        }

        public void SetSqlKeyRetrieval(string SQL)
        {
            this._SqlKeyRetrieval = SQL;
        }

        #endregion

        #region Public functions

        /// <summary>
        /// Setting the helpprovider
        /// </summary>
        /// <param name="KeyWord">The keyword as defined in the manual</param>
        public void setHelp(string KeyWord)
        {
            DiversityWorkbench.Forms.FormFunctions.SetHelp(this.helpProvider, this, KeyWord);
        }

        /// <summary>
        /// setting an optional information, e.g. for Taxonomic groups the source of data retrieval - either DiversityTaxonNames or DiversityScientificTerms
        /// </summary>
        /// <param name="Text">Explanation text of the option</param>
        /// <param name="Image">The image corresponding to the option</param>
        public void setOption(string Text, System.Drawing.Image Image)
        {
            this.labelOption.Visible = true;
            this.labelOption.Text = Text;
            this.pictureBoxOption.Visible = true;
            this.pictureBoxOption.Image = Image;
        }

        public void setWarning(string Text)
        {
            this.labelOption.Visible = true;
            this.labelOption.Text = Text;
            this.labelOption.ForeColor = System.Drawing.Color.Red;
            this.labelOption.BackColor = System.Drawing.Color.Pink;
            this.labelOption.Margin = new Padding(3, 3, 0, 3);
            System.Drawing.Font font = new Font(this.labelOption.Font, FontStyle.Bold);
            this.labelOption.Font = font;
            this.pictureBoxOption.Visible = true;
            this.pictureBoxOption.Image = DiversityWorkbench.Properties.Resources.error;
            this.pictureBoxOption.BackColor = System.Drawing.Color.Pink;
            this.pictureBoxOption.Margin = new Padding(0, 3, 3, 3);
        }

        /// <summary>
        /// Set the entries where the module types can not be edited because they are already used
        /// </summary>
        /// <param name="BlockedEntries">The codes of the entries where the module types should be blocked</param>
        public void setBlockedModuleTypes(System.Collections.Generic.List<string> BlockedEntries)
        {
            this._BlockedModuleTypes = BlockedEntries;
        }

        public void setDoNotDelete(System.Collections.Generic.List<string> DontDelete)
        {
            this._DoNotDelete = DontDelete;
        }

        #endregion

        #region Tree

        private string _NodeColumn;
        private string NodeColumn
        {
            get
            {
                if (this._NodeColumn == null)
                {
                    if (this._KeyColumn != this._DisplayColumn && this._DtEnum.Columns[this._KeyColumn].DataType.Name == "Int32")
                        this._NodeColumn = this._DisplayColumn;
                    else
                        this._NodeColumn = this._KeyColumn;
                }
                return this._NodeColumn;
            }
        }

        private void buildTree()
        {
            this.treeViewEnum.Nodes.Clear();
            try
            {
                System.Data.DataRow[] RR = this._DtEnum.Select(this._ParentKeyColumn + " IS NULL", this._KeyColumn);
                if (RR.Length > 0 && this._FixedParentCode.Length == 0)
                {
                    foreach (System.Data.DataRow R in RR)
                    {
                        System.Windows.Forms.TreeNode N = new TreeNode(R[NodeColumn].ToString());
                        N.Tag = R;
                        this.setImageIndex(N, R);
                        //if (this._ImageIndex != null && this._ImageIndex.ContainsKey(R[this._KeyColumn].ToString().ToLower()))
                        //{
                        //    N.ImageIndex = this._ImageIndex[R[this._KeyColumn].ToString().ToLower()];
                        //    N.SelectedImageIndex = N.ImageIndex;
                        //}
                        //else if (this._ImageIndex != null)
                        //{
                        //    N.ForeColor = System.Drawing.Color.Gray;
                        //}
                        this.treeViewEnum.Nodes.Add(N);
                        this.AddNodeChildren(N);
                    }
                }
                else if (this._FixedParentCode.Length > 0)
                {
                    RR = this._DtEnum.Select(this._ParentKeyColumn + " = '" + this._FixedParentCode + "'", this._KeyColumn);
                    foreach (System.Data.DataRow R in RR)
                    {
                        System.Windows.Forms.TreeNode N = new TreeNode(R[NodeColumn].ToString());
                        N.Tag = R;
                        this.setImageIndex(N, R);
                        //if (this._ImageIndex != null && this._ImageIndex.ContainsKey(R[this._KeyColumn].ToString().ToLower()))
                        //{
                        //    N.ImageIndex = this._ImageIndex[R[this._KeyColumn].ToString().ToLower()];
                        //    N.SelectedImageIndex = N.ImageIndex;
                        //}
                        //else if (this._ImageIndex != null)
                        //{
                        //    N.ForeColor = System.Drawing.Color.Gray;
                        //}
                        this.treeViewEnum.Nodes.Add(N);
                        this.AddNodeChildren(N);
                    }
                    this.treeViewEnum.ShowRootLines = false;
                }
                this.treeViewEnum.ExpandAll();
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void setImageIndex(System.Windows.Forms.TreeNode N, System.Data.DataRow R)
        {
            try
            {
                bool? IsEnabled = null;
                if (R.Table.Columns.Contains("DisplayEnable"))
                {
                    bool Enable;
                    if (bool.TryParse(R["DisplayEnable"].ToString(), out Enable))
                        IsEnabled = Enable;
                }
                string Key = R[this._KeyColumn].ToString().ToLower();
                if (this._ImageDict != null)
                {
                    if (this._ImageDict.ContainsKey(R[this._KeyColumn].ToString().ToLower()))
                    {
                        N.ImageKey = Key;
                        N.SelectedImageKey = N.ImageKey;
                    }
                    else if (this._ImageDict.ContainsKey(R[this._KeyColumn].ToString()))
                    {
                        N.ImageKey = R[this._KeyColumn].ToString();
                        N.SelectedImageKey = N.ImageKey;
                    }
                    else
                    {
                        // Markus 28.7.23: Try to get the image corresponding to the module
                        if (R.Table.Columns.Contains("ModuleName") && !R["ModuleName"].Equals(System.DBNull.Value) && R["ModuleName"].ToString().Length > 0 && this._ImageDict.ContainsKey(R["ModuleName"].ToString()))
                        {
                            N.ImageKey = R["ModuleName"].ToString();
                            N.SelectedImageKey = N.ImageKey;
                        }
                        else if (IsEnabled != null && !(bool)IsEnabled)
                            N.ForeColor = System.Drawing.Color.Gray;
                    }
                }
                else
                {
                    if (IsEnabled != null && !(bool)IsEnabled)
                        N.ForeColor = System.Drawing.Color.Gray;
                }
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            //if (this._ImageIndex != null && this._ImageIndex.ContainsKey(R[this._KeyColumn].ToString().ToLower()))
            //{
            //    N.ImageIndex = this._ImageIndex[R[this._KeyColumn].ToString().ToLower()];
            //    N.SelectedImageIndex = N.ImageIndex;
            //}
            //else if (this._ImageIndex != null)
            //{
            //    N.ForeColor = System.Drawing.Color.Gray;
            //}
        }

        private void AddNodeChildren(System.Windows.Forms.TreeNode ParentNode)
        {
            try
            {
                System.Data.DataRow RP = (System.Data.DataRow)ParentNode.Tag;
                string ParentCode = RP[this._KeyColumn].ToString().ToLower();
                System.Data.DataRow[] RR = this._DtEnum.Select(this._ParentKeyColumn + " = '" + ParentCode.Replace("'", "''") + "'", this._KeyColumn);
                foreach (System.Data.DataRow R in RR)
                {
                    System.Windows.Forms.TreeNode N = new TreeNode(R[NodeColumn].ToString());
                    N.Tag = R;
                    this.setImageIndex(N, R);
                    //if (this._ImageIndex != null && this._ImageIndex.ContainsKey(R[this._KeyColumn].ToString().ToLower()))
                    //{
                    //    N.ImageIndex = this._ImageIndex[R[this._KeyColumn].ToString().ToLower()];
                    //    N.SelectedImageIndex = N.ImageIndex;
                    //}
                    //else if (this._ImageIndex != null)
                    //{
                    //    N.ForeColor = System.Drawing.Color.Gray;
                    //}
                    ParentNode.Nodes.Add(N);
                    this.AddNodeChildren(N);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        private void treeViewEnum_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (this.treeViewEnum.SelectedNode.Parent == null)
                    this.comboBoxParentRelation.Enabled = false;
                else
                    this.comboBoxParentRelation.Enabled = true;
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewEnum.SelectedNode.Tag;
                bool EnableEditing = true;
                if (this._LockedList.Contains(R[0].ToString()))
                    EnableEditing = false;
                this.toolStripButtonDelete.Enabled = EnableEditing;
                this.toolStripButtonRemoveParent.Enabled = EnableEditing;
                this.toolStripButtonSetParent.Enabled = EnableEditing;
                this.splitContainerMain.Panel2.Enabled = EnableEditing;

                this.tableLayoutPanelEnum.Enabled = true;
                for (int i = 0; i < this._DtEnum.Rows.Count; i++)
                {
                    if (R.RowState != DataRowState.Deleted &&
                        this._DtEnum.Rows[i].RowState != DataRowState.Deleted &&
                        R[this._KeyColumn].ToString().ToLower() == this._DtEnum.Rows[i][this._KeyColumn].ToString().ToLower())
                    {
                        this._BindingSource.Position = i;
                        break;
                    }
                }

                if (R.Table.Columns.Contains("DisplayOrder"))
                {
                    this.numericUpDownDisplayOrder.Visible = true;
                    if (R["DisplayOrder"].Equals(System.DBNull.Value))
                    {
                        this.numericUpDownDisplayOrder.Text = "";
                    }
                    else
                        this.numericUpDownDisplayOrder.Text = R["DisplayOrder"].ToString();
                }
                else
                {
                    this.numericUpDownDisplayOrder.Visible = false;
                }

                if (this._ImageDict != null
                    && this._ImageDict.ContainsKey(R[this._KeyColumn].ToString().ToLower()))
                {
                    this.pictureBoxImage.Image = this._ImageDict[R[this._KeyColumn].ToString().ToLower()].First().Value;
                }
                else if (this._ImageDict != null
                    && this._ImageDict.ContainsKey(R[this._KeyColumn].ToString()))
                    this.pictureBoxImage.Image = this._ImageDict[R[this._KeyColumn].ToString()].First().Value;
                else
                    this.pictureBoxImage.Image = null;


                //if (this._Images != null
                //    && this._Images.ContainsKey(R[this._KeyColumn].ToString().ToLower()))
                //{
                //    this.pictureBoxImage.Image = this._Images[R[this._KeyColumn].ToString().ToLower()];
                //}
                //else if (this._Images != null
                //    && this._Images.ContainsKey(R[this._KeyColumn].ToString()))
                //    this.pictureBoxImage.Image = this._Images[R[this._KeyColumn].ToString()];
                //else
                //    this.pictureBoxImage.Image = null;
                if (this.EntityTabelsDoExist)
                {
                    try
                    {
                        this.SqlDataAdapterEntityRepresentation.Update(this.DtEntityRepresentation);
                    }
                    catch (System.Exception ex)
                    {
                    }
                    this.SqlDataAdapterEntityRepresentation.SelectCommand.CommandText = this.SqlEntityRepresentation();
                    this.DtEntityRepresentation.Clear();
                    this.SqlDataAdapterEntityRepresentation.Fill(this.DtEntityRepresentation);
                    Microsoft.Data.SqlClient.SqlCommandBuilder cb = new Microsoft.Data.SqlClient.SqlCommandBuilder(this.SqlDataAdapterEntityRepresentation);

                    this.dataGridViewRepresentation.DataSource = this.DtEntityRepresentation;
                    this.dataGridViewRepresentation.Columns[0].ReadOnly = true;
                    this.dataGridViewRepresentation.Columns[0].Visible = false;
                    this.dataGridViewRepresentation.Columns[1].ReadOnly = true;
                    this.dataGridViewRepresentation.Columns[1].HeaderText = "Language";
                    this.dataGridViewRepresentation.Columns[2].ReadOnly = true;
                    this.dataGridViewRepresentation.Columns[2].HeaderText = "Context";
                    this.dataGridViewRepresentation.Columns[3].HeaderText = "Display";
                    this.dataGridViewRepresentation.AllowUserToAddRows = false;
                    this.dataGridViewRepresentation.AllowUserToDeleteRows = false;
                    this.dataGridViewRepresentation.AutoResizeColumns();
                    this.dataGridViewRepresentation.RowHeadersVisible = false;
                }

                bool BlockModule = false;
                if (this._BlockedModuleTypes != null && this._BlockedModuleTypes.Contains(R[this._KeyColumn].ToString()))
                {
                    BlockModule = true;
                }
                this.comboBoxModuleName.Enabled = !BlockModule;

                bool CanDelete = true;
                if (this._DoNotDelete != null && this._DoNotDelete.Contains(R[this._KeyColumn].ToString()))
                    CanDelete = false;
                this.toolStripButtonDelete.Enabled = CanDelete;
                if (ProjectSelectionAvailable())
                {
                    string Code = R[this._KeyColumn].ToString();
                    this.initProjectList(Code);
                }
            }
            catch (System.Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #region Toolstrip

        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormGetString f = new FormGetString("New entry", "Please enter the code for the new entry in table " + this._TableName, "");
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK && f.String.Length > 0)
            {
                try
                {
                    System.Data.DataRow R = this._DtEnum.NewRow();
                    if (this._SqlKeyRetrieval.Length > 0)
                    {
                        string Key = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(this._SqlKeyRetrieval);
                        R[this._KeyColumn] = Key;
                    }
                    else
                        R[this._KeyColumn] = f.String;
                    R["Description"] = f.String;
                    R[this._DisplayColumn] = f.String;
                    if (this._DtEnum.Columns.Contains("DisplayEnable"))
                        R["DisplayEnable"] = 1;
                    if (this._DtEnum.Columns.Contains("DisplayEnabled"))
                        R["DisplayEnabled"] = 1;
                    if (this._FixedParentCode.Length > 0)
                        R[this._ParentKeyColumn] = this._FixedParentCode;
                    this._DtEnum.Rows.Add(R);
                    this.buildTree();
                    this._DataHaveBeenChanged = true;
                    System.Windows.Forms.TreeNode treeNode = this.getNode(R[this._KeyColumn].ToString());
                    treeNode.EnsureVisible();
                }
                catch (System.Exception ex)
                {
                    DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
                }
            }
        }

        private System.Windows.Forms.TreeNode getNode(string Key/*, bool ChangeTree = false, string Parent = ""*/, System.Windows.Forms.TreeNode parent = null)
        {
            try
            {
                if (Key != null)
                {
                    if (parent == null)
                    {
                        foreach (System.Windows.Forms.TreeNode node in this.treeViewEnum.Nodes)
                        {
                            if (node.Text == Key || node.Text.ToLower() == Key.ToLower())
                                return node;
                        }
                        foreach (System.Windows.Forms.TreeNode node in this.treeViewEnum.Nodes)
                        {
                            System.Windows.Forms.TreeNode child = this.getNode(Key/*, ChangeTree, Parent*/, node);
                            if (child != null) return child;
                        }
                    }
                    else
                    {
                        foreach (System.Windows.Forms.TreeNode nn in parent.Nodes)
                        {
                            if (nn.Text.ToLower() == Key.ToLower())
                                return nn;
                            else
                            {
                                System.Windows.Forms.TreeNode cc = this.getNode(Key/*, ChangeTree, Parent*/, nn);
                                if (cc != null) return cc;
                            }
                        }
                        //System.Windows.Forms.TreeNode child = this.getNode(Key, ChangeTree, Parent, parent);
                        //if (child != null)
                        //    return child;
                        //else if (ChangeTree && Parent.Length > 0)
                        //{
                        //    System.Windows.Forms.TreeNode p = this.getNode(Parent);
                        //    if (p != null) return p;
                        //}
                        //else return null;
                    }
                }
                else return null;
            }
            catch (System.Exception ex) { DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex); }
            return null;
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            bool DeletingFailed = false;
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewEnum.SelectedNode.Tag;
                string Key = R[this._KeyColumn].ToString();
                R.Delete();
                if (this._ImageDict != null && (this._ImageDict.ContainsKey(Key) || this._ImageDict.ContainsKey(Key.ToLower())))
                {
                    if (this.imageList.Images.ContainsKey(Key))
                        this.imageList.Images.RemoveByKey(Key);
                    else if (this.imageList.Images.ContainsKey(Key.ToLower()))
                        this.imageList.Images.RemoveByKey(Key.ToLower());
                    if (this._ImageDict.ContainsKey(Key))
                        this._ImageDict.Remove(Key);
                    else if (this._ImageDict.ContainsKey(Key.ToLower()))
                        this._ImageDict.Remove(Key.ToLower());
                }
                this.buildTree();
                this._SqlAdapter.Update(this._DtEnum);
                this._DataHaveBeenChanged = true;
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                DeletingFailed = true;
            }
            if (DeletingFailed)
            {
                this.getData();
                this.buildTree();
            }
        }

        private void toolStripButtonSetParent_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewEnum.SelectedNode.Tag;
                string Key = R[this._KeyColumn].ToString().ToLower();
                string Parent = "";
                if (this._ParentCodes != null && this._ParentCodes.Count > 0)
                {
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(this._ParentCodes, "Parent entry", "Please select the superior entry", true);
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        R.BeginEdit();
                        if (f.SelectedString.Length > 0)
                        {
                            R[this._ParentKeyColumn] = f.SelectedString;
                            Parent = f.SelectedString;
                        }
                        else
                        {
                            R[this._ParentKeyColumn] = System.DBNull.Value;
                        }
                        R.EndEdit();
                        this.buildTree();
                        this._DataHaveBeenChanged = true;
                    }
                }
                else
                {
                    System.Data.DataTable dt = this._DtEnum.Copy();
                    System.Data.DataRow Rnull = dt.NewRow();
                    Rnull[0] = "";
                    dt.Rows.Add(Rnull);
                    System.Data.DataRow[] RR = dt.Select(this._ParentKeyColumn + " = '" + R[this._KeyColumn].ToString() + "'", "");
                    if (RR.Length > 0)
                    {
                        for (int i = 0; i < RR.Length; i++)
                        {
                            this.RemoveChildEntriesFromTable(ref dt, RR[i]);
                            RR[i].Delete();
                        }
                    }
                    System.Data.DataRow[] Rself = dt.Select(this._KeyColumn + " = '" + R[this._KeyColumn].ToString() + "'", "");
                    Rself[0].Delete();
                    DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, this._KeyColumn, this._KeyColumn, "Parent entry", "Please select the superior entry for " + R[this._KeyColumn].ToString() + " from the list");
                    f.ShowDialog();
                    if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                    {
                        R.BeginEdit();
                        if (f.SelectedString.Length > 0)
                        {
                            R[this._ParentKeyColumn] = f.SelectedString;
                            Parent = f.SelectedString;
                        }
                        else
                        {
                            R[this._ParentKeyColumn] = System.DBNull.Value;
                        }
                        R.EndEdit();
                        this.buildTree();
                        this._DataHaveBeenChanged = true;
                    }
                }
                System.Windows.Forms.TreeNode node = this.getNode(Key);//, true, Parent);
                if (node != null) node.EnsureVisible();
            }
            catch (System.Exception ex)
            {
            }
        }

        private void toolStripButtonRemoveParent_Click(object sender, EventArgs e)
        {
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewEnum.SelectedNode.Tag;
                R.BeginEdit();
                R[this._ParentKeyColumn] = System.DBNull.Value;
                R.EndEdit();
                this.buildTree();
                this._DataHaveBeenChanged = true;
                string Key = R[this._KeyColumn].ToString().ToLower();

            }
            catch (System.Exception ex)
            {
            }
        }

        private void RemoveChildEntriesFromTable(ref System.Data.DataTable dt, System.Data.DataRow R)
        {
            System.Data.DataRow[] RR = dt.Select(this._ParentKeyColumn + " = '" + R[this._KeyColumn].ToString() + "'", "");
            if (RR.Length > 0)
            {
                for (int i = 0; i < RR.Length; i++)
                {
                    this.RemoveChildEntriesFromTable(ref dt, RR[i]);
                    RR[i].Delete();
                }
            }
        }

        #endregion

        private void buttonGetURL_Click(object sender, EventArgs e)
        {
            DiversityWorkbench.Forms.FormWebBrowser f = new FormWebBrowser(this.textBoxURL.Text);
            f.ShowDialog();
            if (f.DialogResult == System.Windows.Forms.DialogResult.OK)
                this.textBoxURL.Text = f.URL;
        }

        #endregion

        #region Images

        /// <summary>
        /// Open an image from a file to add it to the entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonGetImage_Click(object sender, EventArgs e)
        {
            if (this.treeViewEnum.SelectedNode == null || this.treeViewEnum.SelectedNode.Tag == null)
            {
                System.Windows.Forms.MessageBox.Show("Please select a node in the tree");
                return;
            }
            try
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewEnum.SelectedNode.Tag;
                this.openFileDialog = new OpenFileDialog();
                this.openFileDialog.RestoreDirectory = true;
                this.openFileDialog.Multiselect = false;
                this.openFileDialog.InitialDirectory = System.Environment.SpecialFolder.MyPictures.ToString();
                this.openFileDialog.Filter = "Image Files (*.PNG;*.BMP;*.JPG;*.GIF)|*.PNG;*.BMP;*.JPG;*.GIF";
                this.openFileDialog.ShowDialog();
                if (this.openFileDialog.FileName.Length > 0)
                {
                    System.IO.FileInfo f = new System.IO.FileInfo(this.openFileDialog.FileName);
                    System.Drawing.Image I = System.Drawing.Image.FromFile(f.FullName);
                    System.Drawing.Bitmap B = new Bitmap(I, 16, 16);
                    System.Drawing.Bitmap BG = DiversityWorkbench.Forms.FormFunctions.MakeGrayscale3(B);
                    B.MakeTransparent();

                    string Key = R[this._KeyColumn].ToString().ToLower();

                    if (this.ImageDict.ContainsKey(Key))
                    {
                        int index = this.ImageDict[Key].First().Key;
                        this.ImageDict[Key][index] = (System.Drawing.Image)B;
                        this.imageList.Images.RemoveByKey(Key);
                        this.imageList.Images.Add(Key, B);
                    }
                    else
                    {
                        int dictCount = this.ImageDict.Count;
                        System.Collections.Generic.Dictionary<int, System.Drawing.Image> dd = new Dictionary<int, Image>();
                        dd.Add(dictCount, (System.Drawing.Image)B);
                        this.ImageDict.Add(Key, dd);
                        this.imageList.Images.Add(Key, B);
                    }



                    //if (this._Images.ContainsKey(this.labelCode.Text.ToLower()))
                    //    this._Images[this.labelCode.Text.ToLower()] = (System.Drawing.Image)B;
                    //else
                    //    this._Images.Add(this.labelCode.Text.ToLower(), (System.Drawing.Image)B);

                    //if (this._ImageIndex.ContainsKey(this.labelCode.Text.ToLower()))
                    //    this._ImageIndex[this.labelCode.Text.ToLower()] = this._Images.Count;
                    //else
                    //    this._ImageIndex.Add(this.labelCode.Text.ToLower(), this._Images.Count);

                    //this.treeViewEnum.SelectedNode.ImageIndex = this._ImageDict[Key].First().Key;
                    //this.treeViewEnum.SelectedNode.SelectedImageIndex = this._ImageDict[Key].First().Key;

                    this.treeViewEnum.SelectedNode.ImageKey = Key;
                    this.treeViewEnum.SelectedNode.SelectedImageKey = Key;

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    B.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    System.Byte[] BImage = (System.Byte[])ms.ToArray();
                    R["Icon"] = BImage;
                    //try
                    //{
                    //    R.BeginEdit();
                    //    R.EndEdit();
                    //    this._SqlAdapter.Update(this._DtEnum);
                    //}
                    //catch(System.Exception ex) { /*DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);*/ }

                    //this.treeViewEnum.SelectedNode.ImageIndex = this._ImageIndex[this.labelCode.Text.ToLower()];
                    //this.treeViewEnum.SelectedNode.SelectedImageIndex = this._ImageIndex[this.labelCode.Text.ToLower()];

                    //this._BImage = (System.Byte[])ms.ToArray();
                    this.pictureBoxImage.Image = B;
                    this.treeViewEnum.ImageList = this.imageList;

                    this.buildTree();
                    this._DataHaveBeenChanged = true;

                    System.Windows.Forms.TreeNode node = this.getNode(R[this._KeyColumn].ToString());
                    if (node != null) node.EnsureVisible();
                }
            }
            catch (Exception ex)
            {
                DiversityWorkbench.ExceptionHandling.WriteToErrorLogFile(ex);
            }
        }

        #endregion

        #region Supplements

        private void setSupplementControls()
        {
            bool ShowSupplements = false;
            if (this.EntityTabelsDoExist) ShowSupplements = true;
            else this.tabControlSupplements.TabPages.Remove(this.tabPageEntity);
            if (ProjectSelectionAvailable()) ShowSupplements = true;
            else this.tabControlSupplements.TabPages.Remove(this.tabPageProject);
            this.splitContainerSupplements.Panel2Collapsed = !ShowSupplements;
        }

        #region Entity

        bool? _EntityTabelsDoExist;

        public bool EntityTabelsDoExist
        {
            get
            {
                if (this._EnableEntityDescriptions)
                {
                    if (this._EntityTabelsDoExist == null)
                        this._EntityTabelsDoExist = DiversityWorkbench.Entity.EntityTablesExist;
                    return (bool)_EntityTabelsDoExist;
                }
                else return false;
            }
            //set { _EntityTabelsDoExist = value; }
        }

        private DiversityWorkbench.Datasets.DataSetEntity.EntityRepresentationDataTable _DtEntityRepresentation;

        public DiversityWorkbench.Datasets.DataSetEntity.EntityRepresentationDataTable DtEntityRepresentation
        {
            get
            {
                if (this._DtEntityRepresentation == null)
                    this._DtEntityRepresentation = new DiversityWorkbench.Datasets.DataSetEntity.EntityRepresentationDataTable();
                return _DtEntityRepresentation;
            }
            //set { _DtEntityRepresentation = value; }
        }

        private Microsoft.Data.SqlClient.SqlDataAdapter _SqlDataAdapterEntityRepresentation;

        public Microsoft.Data.SqlClient.SqlDataAdapter SqlDataAdapterEntityRepresentation
        {
            get
            {
                if (this._SqlDataAdapterEntityRepresentation == null)
                {
                    this._SqlDataAdapterEntityRepresentation = new Microsoft.Data.SqlClient.SqlDataAdapter(this.SqlEntityRepresentation(), DiversityWorkbench.Settings.ConnectionString);
                }
                return _SqlDataAdapterEntityRepresentation;
            }
            //set { _SqlDataAdapterEntityRepresentation = value; }
        }

        private string SqlEntityRepresentation()
        {
            string SQL = "SELECT Entity, LanguageCode, EntityContext, DisplayText, Abbreviation, Description, Notes " +
                "FROM EntityRepresentation WHERE Entity = '" + this._TableName + ".Code." + this.labelCode.Text + "'";
            return SQL;
        }

        private void buttonRepresentationNew_Click(object sender, EventArgs e)
        {
            // getting the language for the new representation
            System.Data.DataTable dtLanguage = DiversityWorkbench.Entity.DtLanguage;
            DiversityWorkbench.Forms.FormGetStringFromList fL = new DiversityWorkbench.Forms.FormGetStringFromList(dtLanguage, this._DisplayColumn, this._KeyColumn, "Language", "Please select the language for the new representation");
            fL.ShowDialog();
            if (fL.DialogResult == DialogResult.OK)
            {
                string Language = fL.SelectedValue;
                // getting the context for the new representation
                System.Data.DataTable dtContext = DiversityWorkbench.Entity.DtContext;
                foreach (System.Data.DataRow RR in this.DtEntityRepresentation.Rows)
                {
                    for (int i = 0; i < dtContext.Rows.Count; i++)
                    {
                        if (dtContext.Rows[i][this._KeyColumn].ToString() == RR["EntityContext"].ToString()
                            && RR["LanguageCode"].ToString() == Language)
                            dtContext.Rows[i].Delete();
                    }
                    dtContext.AcceptChanges();
                }
                if (dtContext.Rows.Count == 0) // all PK's are already used
                {
                    System.Windows.Forms.MessageBox.Show("All contexts for the selected language are already defined");
                    return;
                }
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dtContext, this._DisplayColumn, this._KeyColumn, "Context", "Select a context from the list");
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                    DiversityWorkbench.Datasets.DataSetEntity.EntityRepresentationRow RRep = this.DtEntityRepresentation.NewEntityRepresentationRow();
                    RRep.Entity = this._TableName + ".Code." + this.labelCode.Text;// this.dataSetEntity.Entity.Rows[0]["Entity"].ToString();
                    RRep.EntityContext = f.SelectedValue;
                    RRep.LanguageCode = Language;
                    this.DtEntityRepresentation.Rows.Add(RRep);

                    string Entity = this._TableName + ".Code." + this.labelCode.Text;
                    string SQL = "IF (SELECT COUNT(*) FROM Entity WHERE Entity = '" + Entity + "') = 0 BEGIN INSERT INTO Entity(Entity) Values ('" + Entity + "') END";
                    DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL);
                    this._DataHaveBeenChanged = true;
                }
            }
        }

        private void dataGridViewRepresentation_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        #endregion

        #region Projects

        private bool ProjectSelectionAvailable()
        {
            return DiversityWorkbench.EnumTable.ProjectSelectionAvailable(this._TableName);
            //bool Available = false;
            //string SQL = "SELECT COUNT(*) " + ProjectFromClause();
            //int i = 0;
            //if (int.TryParse(DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL), out i) && i > 0)
            //    Available = true;
            //return Available;
        }

        private void toolStripButtonProjectAdd_Click(object sender, EventArgs e)
        {
            string Code = "";
            int ProjectID = 0;
            if (this.treeViewEnum.SelectedNode != null && this.treeViewEnum.SelectedNode.Tag != null && this.treeViewEnum.SelectedNode.Tag.GetType() == typeof(System.Data.DataRow))
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewEnum.SelectedNode.Tag;
                Code = R[this._KeyColumn].ToString();
                string SQL = "SELECT Project, ProjectID FROM Project ORDER BY Project";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                DiversityWorkbench.Forms.FormGetStringFromList f = new DiversityWorkbench.Forms.FormGetStringFromList(dt, "Project", "ProjectID", "New project", "Please select a project", "", false, true, false, DiversityWorkbench.Properties.Resources.Project);
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK && f.SelectedValue != null)
                {
                    if (int.TryParse(f.SelectedValue, out ProjectID))
                    {
                        SQL = "INSERT INTO " + this.ProjectLinkTable() + " (ProjectID, " + ProjectLinkColumn() + ") VALUES (" + ProjectID.ToString() + ", '" + Code + "' )";
                        if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                            this.initProjectList(Code);

                    }

                }
            }
        }

        private void toolStripButtonProjectDelete_Click(object sender, EventArgs e)
        {
            string Code = "";
            int ProjectID;
            if (this.treeViewEnum.SelectedNode != null && this.treeViewEnum.SelectedNode.Tag != null && this.treeViewEnum.SelectedNode.Tag.GetType() == typeof(System.Data.DataRow) &&
                this.listBoxProjects.SelectedValue != null && int.TryParse(this.listBoxProjects.SelectedValue.ToString(), out ProjectID))
            {
                System.Data.DataRow R = (System.Data.DataRow)this.treeViewEnum.SelectedNode.Tag;
                Code = R[this._KeyColumn].ToString();
                string SQL = "DELETE P FROM " + this.ProjectLinkTable() + " P WHERE P." + ProjectLinkColumn() + " = '" + Code + "' AND P.ProjectID = " + ProjectID.ToString();
                if (DiversityWorkbench.Forms.FormFunctions.SqlExecuteNonQuery(SQL))
                    this.initProjectList(Code);
            }
        }

        private void toolStripButtonProjectList_Click(object sender, EventArgs e)
        {
            int ProjectID;
            if (this.treeViewEnum.SelectedNode != null && this.treeViewEnum.SelectedNode.Tag != null && this.treeViewEnum.SelectedNode.Tag.GetType() == typeof(System.Data.DataRow) &&
                this.listBoxProjects.SelectedValue != null && int.TryParse(this.listBoxProjects.SelectedValue.ToString(), out ProjectID))
            {
                string SQL = "SELECT M." + this.ProjectLinkColumn() + " " +
                             "FROM " + this.ProjectLinkTable() + " AS M INNER JOIN " +
                             "Project AS P ON M.ProjectID = P.ProjectID " +
                             "WHERE (P.ProjectID = " + ProjectID.ToString() + ")";
                System.Data.DataTable dt = new DataTable();
                DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dt);
                System.Data.DataRowView r = (System.Data.DataRowView)this.listBoxProjects.SelectedItem;
                string Project = r[1].ToString();
                DiversityWorkbench.Forms.FormTableContent f = new FormTableContent(Project, "List of current selection for the project", dt);
                f.ShowDialog();
            }
        }


        private void initProjectList(string Code)
        {
            // Markus 10.6.24: Bugfix - Replace Project with ProjectProxy in SQL
            string SQL = "SELECT DISTINCT M.ProjectID, P.Project " +
                         "FROM " + this.ProjectLinkTable() + " AS M INNER JOIN " +
                         "ProjectProxy AS P ON M.ProjectID = P.ProjectID " +
                         "WHERE (M." + ProjectLinkColumn();
            if (int.TryParse(Code, out int i))
                SQL += " = '" + Code + "')";
            else
                SQL += " = N'" + Code + "')";
            System.Data.DataTable dataTable = new DataTable();
            DiversityWorkbench.Forms.FormFunctions.SqlFillTable(SQL, ref dataTable);
            this.listBoxProjects.DataSource = dataTable;
            this.listBoxProjects.DisplayMember = "Project";
            this.listBoxProjects.ValueMember = "ProjectID";
        }

        private string ProjectLinkTable()
        {
            return DiversityWorkbench.EnumTable.ProjectLinkTable(this._TableName);
            //string SQL = "SELECT TOP 1 C.TABLE_NAME " + ProjectFromClause("C.COLUMN_NAME = 'ProjectID'");
            //string Table = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //return Table;
        }

        private string ProjectLinkColumn()
        {
            return DiversityWorkbench.EnumTable.ProjectLinkColumn(this._TableName);
            //string SQL = "SELECT TOP 1 PK.COLUMN_NAME " + ProjectFromClause("C.TABLE_NAME = '" + ProjectLinkTable() + "'");
            //string Table = DiversityWorkbench.Forms.FormFunctions.SqlExecuteScalar(SQL);
            //return Table;
        }


        private string ProjectFromClause(string Restriction = "")
        {
            string SQL = " FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PP " +
                "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TF " +
                "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS FK ON TF.CONSTRAINT_NAME = FK.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS PK " +
                "INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TPK ON PK.CONSTRAINT_NAME = TPK.CONSTRAINT_NAME ON FK.COLUMN_NAME = PK.COLUMN_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS R ON FK.CONSTRAINT_NAME = R.CONSTRAINT_NAME " +
                "INNER JOIN INFORMATION_SCHEMA.CONSTRAINT_TABLE_USAGE AS P ON R.UNIQUE_CONSTRAINT_NAME = P.CONSTRAINT_NAME ON PP.TABLE_NAME = P.TABLE_NAME AND PP.CONSTRAINT_NAME = P.CONSTRAINT_NAME AND PP.ORDINAL_POSITION = FK.ORDINAL_POSITION " +
                "INNER JOIN INFORMATION_SCHEMA.COLUMNS C ON TF.TABLE_NAME = C.TABLE_NAME " +
                "WHERE(TF.CONSTRAINT_TYPE = 'FOREIGN KEY') " +
                "AND TF.TABLE_NAME = TPK.TABLE_NAME " +
                "AND P.TABLE_NAME = '" + this._TableName + "'";
            if (Restriction.Length > 0)
                SQL += " AND " + Restriction;
            return SQL;
        }

        #endregion

        #endregion

        #region Enabling
        private void checkBoxDisplayEnable_Click(object sender, EventArgs e)
        {
            _DataHaveBeenChanged = true;
        }

        #endregion

        private void toolStripButtonTableEditor_Click(object sender, EventArgs e)
        {
            System.Collections.Generic.List<string> RO = new List<string>();
            RO.Add(this._KeyColumn);
            DiversityWorkbench.Forms.FormTableEditor f = new FormTableEditor(this._Image, this._SqlAdapter, this._TableName, "Enum", RO);
            f.ShowDialog();
        }
    }
}
