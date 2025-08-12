namespace DiversityCollection.UserControls
{
    partial class UserControlImportMethod
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlImportMethod));
            DiversityCollection.Import_Column import_Column1 = new DiversityCollection.Import_Column();
            this.imageListMethod = new System.Windows.Forms.ImageList(this.components);
            this.tabControlImportSteps = new System.Windows.Forms.TabControl();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.panelSelectionParameter = new System.Windows.Forms.Panel();
            this.buttonRecover = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.userControlImport_Column_Method = new DiversityCollection.UserControls.UserControlImport_Column();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListMethod
            // 
            this.imageListMethod.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListMethod.ImageStream")));
            this.imageListMethod.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListMethod.Images.SetKeyName(0, "Parameter.ico");
            this.imageListMethod.Images.SetKeyName(1, "Tools.ico");
            this.imageListMethod.Images.SetKeyName(2, "Agent.ico");
            this.imageListMethod.Images.SetKeyName(3, "Analysis.ico");
            this.imageListMethod.Images.SetKeyName(4, "Identification.ico");
            this.imageListMethod.Images.SetKeyName(5, "Note.ico");
            this.imageListMethod.Images.SetKeyName(6, "Plant.ico");
            this.imageListMethod.Images.SetKeyName(7, "References.ico");
            // 
            // tabControlImportSteps
            // 
            this.tabControlImportSteps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlImportSteps.ImageList = this.imageListMethod;
            this.tabControlImportSteps.Location = new System.Drawing.Point(0, 0);
            this.tabControlImportSteps.Name = "tabControlImportSteps";
            this.tabControlImportSteps.SelectedIndex = 0;
            this.tabControlImportSteps.Size = new System.Drawing.Size(414, 195);
            this.tabControlImportSteps.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 26);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.panelSelectionParameter);
            this.splitContainer.Panel1.Controls.Add(this.buttonRecover);
            this.splitContainer.Panel1.Controls.Add(this.buttonRemove);
            this.splitContainer.Panel1.Controls.Add(this.buttonAdd);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tabControlImportSteps);
            this.splitContainer.Size = new System.Drawing.Size(467, 195);
            this.splitContainer.SplitterDistance = 49;
            this.splitContainer.TabIndex = 2;
            // 
            // panelSelectionParameter
            // 
            this.panelSelectionParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSelectionParameter.Location = new System.Drawing.Point(0, 72);
            this.panelSelectionParameter.Name = "panelSelectionParameter";
            this.panelSelectionParameter.Size = new System.Drawing.Size(49, 123);
            this.panelSelectionParameter.TabIndex = 2;
            // 
            // buttonRecover
            // 
            this.buttonRecover.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRecover.Image = global::DiversityCollection.Resource.Transfrom;
            this.buttonRecover.Location = new System.Drawing.Point(0, 48);
            this.buttonRecover.Name = "buttonRecover";
            this.buttonRecover.Size = new System.Drawing.Size(49, 24);
            this.buttonRecover.TabIndex = 3;
            this.buttonRecover.UseVisualStyleBackColor = true;
            this.buttonRecover.Visible = false;
            this.buttonRecover.Click += new System.EventHandler(this.buttonRecover_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonRemove.Image = global::DiversityCollection.Resource.Remove;
            this.buttonRemove.Location = new System.Drawing.Point(0, 24);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(49, 24);
            this.buttonRemove.TabIndex = 1;
            this.buttonRemove.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Visible = false;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonAdd.Image = global::DiversityCollection.Resource.Add1;
            this.buttonAdd.Location = new System.Drawing.Point(0, 0);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(49, 24);
            this.buttonAdd.TabIndex = 0;
            this.buttonAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // userControlImport_Column_Method
            // 
            this.userControlImport_Column_Method.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.userControlImport_Column_Method.Dock = System.Windows.Forms.DockStyle.Top;
            import_Column1.ColumnInSourceFile = null;
            import_Column1.DisplayColumn = null;
            import_Column1.IsSelected = false;
            import_Column1.MustSelect = false;
            import_Column1.TranslationDictionary = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("import_Column1.TranslationDictionary")));
            import_Column1.ValueColumn = null;
            this.userControlImport_Column_Method.Import_Column = import_Column1;
            this.userControlImport_Column_Method.Location = new System.Drawing.Point(0, 0);
            this.userControlImport_Column_Method.Name = "userControlImport_Column_Method";
            this.userControlImport_Column_Method.Size = new System.Drawing.Size(467, 26);
            this.userControlImport_Column_Method.TabIndex = 3;
            // 
            // UserControlImportMethod
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.userControlImport_Column_Method);
            this.Name = "UserControlImportMethod";
            this.Size = new System.Drawing.Size(467, 221);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListMethod;
        public System.Windows.Forms.TabControl tabControlImportSteps;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Panel panelSelectionParameter;
        private System.Windows.Forms.Button buttonRecover;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.ToolTip toolTip;
        private UserControlImport_Column userControlImport_Column_Method;
    }
}
