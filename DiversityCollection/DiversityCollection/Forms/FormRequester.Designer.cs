namespace DiversityCollection.Forms
{
    partial class FormRequester
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormRequester));
            this.tableLayoutPanelRequesters = new System.Windows.Forms.TableLayoutPanel();
            this.listBoxUser = new System.Windows.Forms.ListBox();
            this.listBoxAdministratingCollection = new System.Windows.Forms.ListBox();
            this.buttonCollectionAdd = new System.Windows.Forms.Button();
            this.buttonCollectionRemove = new System.Windows.Forms.Button();
            this.labelRequester = new System.Windows.Forms.Label();
            this.labelCollectionRequester = new System.Windows.Forms.Label();
            this.labelAdministratingCollections = new System.Windows.Forms.Label();
            this.treeViewCollectionHierarchy = new System.Windows.Forms.TreeView();
            this.listBoxCollectionRequester = new System.Windows.Forms.ListBox();
            this.dataSetTransaction = new DiversityCollection.Datasets.DataSetTransaction();
            this.checkBoxIncludeSubcollections = new System.Windows.Forms.CheckBox();
            this.collectionRequesterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.collectionRequesterTableAdapter = new DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionRequesterTableAdapter();
            this.tableLayoutPanelRequesters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionRequesterBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelRequesters
            // 
            this.tableLayoutPanelRequesters.ColumnCount = 5;
            this.tableLayoutPanelRequesters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33332F));
            this.tableLayoutPanelRequesters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRequesters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelRequesters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRequesters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanelRequesters.Controls.Add(this.listBoxUser, 0, 1);
            this.tableLayoutPanelRequesters.Controls.Add(this.listBoxAdministratingCollection, 4, 1);
            this.tableLayoutPanelRequesters.Controls.Add(this.buttonCollectionAdd, 3, 1);
            this.tableLayoutPanelRequesters.Controls.Add(this.buttonCollectionRemove, 3, 2);
            this.tableLayoutPanelRequesters.Controls.Add(this.labelRequester, 0, 0);
            this.tableLayoutPanelRequesters.Controls.Add(this.labelCollectionRequester, 2, 0);
            this.tableLayoutPanelRequesters.Controls.Add(this.labelAdministratingCollections, 4, 0);
            this.tableLayoutPanelRequesters.Controls.Add(this.treeViewCollectionHierarchy, 2, 4);
            this.tableLayoutPanelRequesters.Controls.Add(this.listBoxCollectionRequester, 2, 1);
            this.tableLayoutPanelRequesters.Controls.Add(this.checkBoxIncludeSubcollections, 2, 3);
            this.tableLayoutPanelRequesters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelRequesters.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRequesters.Name = "tableLayoutPanelRequesters";
            this.tableLayoutPanelRequesters.RowCount = 5;
            this.tableLayoutPanelRequesters.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRequesters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelRequesters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanelRequesters.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelRequesters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tableLayoutPanelRequesters.Size = new System.Drawing.Size(716, 472);
            this.tableLayoutPanelRequesters.TabIndex = 5;
            // 
            // listBoxUser
            // 
            this.listBoxUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxUser.FormattingEnabled = true;
            this.listBoxUser.IntegralHeight = false;
            this.listBoxUser.Location = new System.Drawing.Point(3, 16);
            this.listBoxUser.Name = "listBoxUser";
            this.tableLayoutPanelRequesters.SetRowSpan(this.listBoxUser, 4);
            this.listBoxUser.Size = new System.Drawing.Size(219, 453);
            this.listBoxUser.TabIndex = 0;
            this.listBoxUser.SelectedIndexChanged += new System.EventHandler(this.listBoxUser_SelectedIndexChanged);
            // 
            // listBoxAdministratingCollection
            // 
            this.listBoxAdministratingCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxAdministratingCollection.FormattingEnabled = true;
            this.listBoxAdministratingCollection.IntegralHeight = false;
            this.listBoxAdministratingCollection.Location = new System.Drawing.Point(493, 16);
            this.listBoxAdministratingCollection.Name = "listBoxAdministratingCollection";
            this.tableLayoutPanelRequesters.SetRowSpan(this.listBoxAdministratingCollection, 4);
            this.listBoxAdministratingCollection.Size = new System.Drawing.Size(220, 453);
            this.listBoxAdministratingCollection.TabIndex = 2;
            // 
            // buttonCollectionAdd
            // 
            this.buttonCollectionAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonCollectionAdd.Location = new System.Drawing.Point(470, 117);
            this.buttonCollectionAdd.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonCollectionAdd.Name = "buttonCollectionAdd";
            this.buttonCollectionAdd.Size = new System.Drawing.Size(20, 23);
            this.buttonCollectionAdd.TabIndex = 1;
            this.buttonCollectionAdd.Text = "<";
            this.buttonCollectionAdd.UseVisualStyleBackColor = true;
            this.buttonCollectionAdd.Click += new System.EventHandler(this.buttonCollectionAdd_Click);
            // 
            // buttonCollectionRemove
            // 
            this.buttonCollectionRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonCollectionRemove.Location = new System.Drawing.Point(470, 146);
            this.buttonCollectionRemove.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.buttonCollectionRemove.Name = "buttonCollectionRemove";
            this.buttonCollectionRemove.Size = new System.Drawing.Size(20, 23);
            this.buttonCollectionRemove.TabIndex = 2;
            this.buttonCollectionRemove.Text = ">";
            this.buttonCollectionRemove.UseVisualStyleBackColor = true;
            this.buttonCollectionRemove.Click += new System.EventHandler(this.buttonCollectionRemove_Click);
            // 
            // labelRequester
            // 
            this.labelRequester.AutoSize = true;
            this.labelRequester.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelRequester.Location = new System.Drawing.Point(3, 0);
            this.labelRequester.Name = "labelRequester";
            this.labelRequester.Size = new System.Drawing.Size(219, 13);
            this.labelRequester.TabIndex = 3;
            this.labelRequester.Text = "Collection requesters";
            this.labelRequester.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelCollectionRequester
            // 
            this.labelCollectionRequester.AutoSize = true;
            this.labelCollectionRequester.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCollectionRequester.Location = new System.Drawing.Point(248, 0);
            this.labelCollectionRequester.Name = "labelCollectionRequester";
            this.labelCollectionRequester.Size = new System.Drawing.Size(219, 13);
            this.labelCollectionRequester.TabIndex = 4;
            this.labelCollectionRequester.Text = "Collections open for requests";
            this.labelCollectionRequester.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelAdministratingCollections
            // 
            this.labelAdministratingCollections.AutoSize = true;
            this.labelAdministratingCollections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAdministratingCollections.Location = new System.Drawing.Point(493, 0);
            this.labelAdministratingCollections.Name = "labelAdministratingCollections";
            this.labelAdministratingCollections.Size = new System.Drawing.Size(220, 13);
            this.labelAdministratingCollections.TabIndex = 5;
            this.labelAdministratingCollections.Text = "Collections";
            this.labelAdministratingCollections.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // treeViewCollectionHierarchy
            // 
            this.treeViewCollectionHierarchy.BackColor = System.Drawing.SystemColors.Control;
            this.treeViewCollectionHierarchy.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeViewCollectionHierarchy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewCollectionHierarchy.Location = new System.Drawing.Point(248, 299);
            this.treeViewCollectionHierarchy.Name = "treeViewCollectionHierarchy";
            this.treeViewCollectionHierarchy.Size = new System.Drawing.Size(219, 170);
            this.treeViewCollectionHierarchy.TabIndex = 7;
            // 
            // listBoxCollectionRequester
            // 
            this.listBoxCollectionRequester.DataSource = this.dataSetTransaction;
            this.listBoxCollectionRequester.DisplayMember = "CollectionRequester.LoginName";
            this.listBoxCollectionRequester.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxCollectionRequester.FormattingEnabled = true;
            this.listBoxCollectionRequester.IntegralHeight = false;
            this.listBoxCollectionRequester.Location = new System.Drawing.Point(248, 16);
            this.listBoxCollectionRequester.Name = "listBoxCollectionRequester";
            this.tableLayoutPanelRequesters.SetRowSpan(this.listBoxCollectionRequester, 2);
            this.listBoxCollectionRequester.Size = new System.Drawing.Size(219, 254);
            this.listBoxCollectionRequester.TabIndex = 8;
            this.listBoxCollectionRequester.SelectedIndexChanged += new System.EventHandler(this.listBoxCollectionRequester_SelectedIndexChanged);
            // 
            // dataSetTransaction
            // 
            this.dataSetTransaction.DataSetName = "DataSetTransaction";
            this.dataSetTransaction.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // checkBoxIncludeSubcollections
            // 
            this.checkBoxIncludeSubcollections.AutoSize = true;
            this.checkBoxIncludeSubcollections.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.collectionRequesterBindingSource, "IncludeSubcollections", true));
            this.checkBoxIncludeSubcollections.Location = new System.Drawing.Point(248, 276);
            this.checkBoxIncludeSubcollections.Name = "checkBoxIncludeSubcollections";
            this.checkBoxIncludeSubcollections.Size = new System.Drawing.Size(131, 17);
            this.checkBoxIncludeSubcollections.TabIndex = 9;
            this.checkBoxIncludeSubcollections.Text = "Include subcollections";
            this.checkBoxIncludeSubcollections.UseVisualStyleBackColor = true;
            this.checkBoxIncludeSubcollections.CheckedChanged += new System.EventHandler(this.checkBoxIncludeSubcollections_CheckedChanged);
            // 
            // collectionRequesterBindingSource
            // 
            this.collectionRequesterBindingSource.DataMember = "CollectionRequester";
            this.collectionRequesterBindingSource.DataSource = this.dataSetTransaction;
            // 
            // collectionRequesterTableAdapter
            // 
            this.collectionRequesterTableAdapter.ClearBeforeFill = true;
            // 
            // FormRequester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 472);
            this.Controls.Add(this.tableLayoutPanelRequesters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormRequester";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administration of loan requester";
            this.Load += new System.EventHandler(this.FormRequester_Load);
            this.tableLayoutPanelRequesters.ResumeLayout(false);
            this.tableLayoutPanelRequesters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetTransaction)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.collectionRequesterBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRequesters;
        private System.Windows.Forms.ListBox listBoxUser;
        private System.Windows.Forms.ListBox listBoxAdministratingCollection;
        private System.Windows.Forms.Button buttonCollectionAdd;
        private System.Windows.Forms.Button buttonCollectionRemove;
        private System.Windows.Forms.Label labelRequester;
        private System.Windows.Forms.Label labelCollectionRequester;
        private System.Windows.Forms.Label labelAdministratingCollections;
        private System.Windows.Forms.TreeView treeViewCollectionHierarchy;
        private System.Windows.Forms.ToolTip toolTip;
        private Datasets.DataSetTransaction dataSetTransaction;
        private System.Windows.Forms.ListBox listBoxCollectionRequester;
        private System.Windows.Forms.CheckBox checkBoxIncludeSubcollections;
        private System.Windows.Forms.BindingSource collectionRequesterBindingSource;
        private DiversityCollection.Datasets.DataSetTransactionTableAdapters.CollectionRequesterTableAdapter collectionRequesterTableAdapter;
    }
}