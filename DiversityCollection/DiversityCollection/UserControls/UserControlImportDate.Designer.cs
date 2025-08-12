namespace DiversityCollection.UserControls
{
    partial class UserControlImportDate
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
            DiversityCollection.Import_Column import_Column1 = new DiversityCollection.Import_Column();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlImportDate));
            DiversityCollection.Import_Column import_Column2 = new DiversityCollection.Import_Column();
            DiversityCollection.Import_Column import_Column3 = new DiversityCollection.Import_Column();
            DiversityCollection.Import_Column import_Column4 = new DiversityCollection.Import_Column();
            this.userControlImport_ColumnDay = new DiversityCollection.UserControls.UserControlImport_Column();
            this.userControlImport_ColumnMonth = new DiversityCollection.UserControls.UserControlImport_Column();
            this.userControlImport_ColumnYear = new DiversityCollection.UserControls.UserControlImport_Column();
            this.userControlImport_ColumnSupplement = new DiversityCollection.UserControls.UserControlImport_Column();
            this.SuspendLayout();
            // 
            // userControlImport_ColumnDay
            // 
            this.userControlImport_ColumnDay.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.userControlImport_ColumnDay.Dock = System.Windows.Forms.DockStyle.Top;
            import_Column1.ColumnInSourceFile = null;
            import_Column1.DisplayColumn = null;
            import_Column1.IsSelected = false;
            import_Column1.MustSelect = false;
            import_Column1.TranslationDictionary = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("import_Column1.TranslationDictionary")));
            import_Column1.ValueColumn = null;
            this.userControlImport_ColumnDay.Import_Column = import_Column1;
            this.userControlImport_ColumnDay.Location = new System.Drawing.Point(0, 0);
            this.userControlImport_ColumnDay.Name = "userControlImport_ColumnDay";
            this.userControlImport_ColumnDay.Size = new System.Drawing.Size(1028, 24);
            this.userControlImport_ColumnDay.TabIndex = 19;
            // 
            // userControlImport_ColumnMonth
            // 
            this.userControlImport_ColumnMonth.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.userControlImport_ColumnMonth.Dock = System.Windows.Forms.DockStyle.Top;
            import_Column2.ColumnInSourceFile = null;
            import_Column2.DisplayColumn = null;
            import_Column2.IsSelected = false;
            import_Column2.MustSelect = false;
            import_Column2.TranslationDictionary = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("import_Column2.TranslationDictionary")));
            import_Column2.ValueColumn = null;
            this.userControlImport_ColumnMonth.Import_Column = import_Column2;
            this.userControlImport_ColumnMonth.Location = new System.Drawing.Point(0, 24);
            this.userControlImport_ColumnMonth.Name = "userControlImport_ColumnMonth";
            this.userControlImport_ColumnMonth.Size = new System.Drawing.Size(1028, 24);
            this.userControlImport_ColumnMonth.TabIndex = 20;
            // 
            // userControlImport_ColumnYear
            // 
            this.userControlImport_ColumnYear.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.userControlImport_ColumnYear.Dock = System.Windows.Forms.DockStyle.Top;
            import_Column3.ColumnInSourceFile = null;
            import_Column3.DisplayColumn = null;
            import_Column3.IsSelected = false;
            import_Column3.MustSelect = false;
            import_Column3.TranslationDictionary = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("import_Column3.TranslationDictionary")));
            import_Column3.ValueColumn = null;
            this.userControlImport_ColumnYear.Import_Column = import_Column3;
            this.userControlImport_ColumnYear.Location = new System.Drawing.Point(0, 48);
            this.userControlImport_ColumnYear.Name = "userControlImport_ColumnYear";
            this.userControlImport_ColumnYear.Size = new System.Drawing.Size(1028, 24);
            this.userControlImport_ColumnYear.TabIndex = 21;
            // 
            // userControlImport_ColumnSupplement
            // 
            this.userControlImport_ColumnSupplement.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.userControlImport_ColumnSupplement.Dock = System.Windows.Forms.DockStyle.Top;
            import_Column4.ColumnInSourceFile = null;
            import_Column4.DisplayColumn = null;
            import_Column4.IsSelected = false;
            import_Column4.MustSelect = false;
            import_Column4.TranslationDictionary = ((System.Collections.Generic.Dictionary<string, string>)(resources.GetObject("import_Column4.TranslationDictionary")));
            import_Column4.ValueColumn = null;
            this.userControlImport_ColumnSupplement.Import_Column = import_Column4;
            this.userControlImport_ColumnSupplement.Location = new System.Drawing.Point(0, 72);
            this.userControlImport_ColumnSupplement.Name = "userControlImport_ColumnSupplement";
            this.userControlImport_ColumnSupplement.Size = new System.Drawing.Size(1028, 24);
            this.userControlImport_ColumnSupplement.TabIndex = 22;
            // 
            // UserControlImportDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.userControlImport_ColumnSupplement);
            this.Controls.Add(this.userControlImport_ColumnYear);
            this.Controls.Add(this.userControlImport_ColumnMonth);
            this.Controls.Add(this.userControlImport_ColumnDay);
            this.Name = "UserControlImportDate";
            this.Size = new System.Drawing.Size(1028, 99);
            this.Load += new System.EventHandler(this.UserControlImportDate_Load);
            this.ResumeLayout(false);

        }

        #endregion

        public UserControlImport_Column userControlImport_ColumnDay;
        public UserControlImport_Column userControlImport_ColumnMonth;
        public UserControlImport_Column userControlImport_ColumnYear;
        public UserControlImport_Column userControlImport_ColumnSupplement;
    }
}
