namespace DiversityCollection.Forms
{
    partial class FormImportWizardCheckError
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportWizardCheckError));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.labelHeader = new System.Windows.Forms.Label();
            this.buttonInsert = new System.Windows.Forms.Button();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonIgnore = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.labelAction = new System.Windows.Forms.Label();
            this.labelTable = new System.Windows.Forms.Label();
            this.labelProblem = new System.Windows.Forms.Label();
            this.textBoxAction = new System.Windows.Forms.TextBox();
            this.textBoxTable = new System.Windows.Forms.TextBox();
            this.textBoxProblem = new System.Windows.Forms.TextBox();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 4;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.buttonInsert, 2, 5);
            this.tableLayoutPanelMain.Controls.Add(this.buttonUpdate, 3, 5);
            this.tableLayoutPanelMain.Controls.Add(this.buttonIgnore, 0, 5);
            this.tableLayoutPanelMain.Controls.Add(this.dataGridView, 0, 4);
            this.tableLayoutPanelMain.Controls.Add(this.labelAction, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.labelTable, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.labelProblem, 0, 3);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxAction, 1, 1);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxTable, 1, 2);
            this.tableLayoutPanelMain.Controls.Add(this.textBoxProblem, 1, 3);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 6;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(777, 355);
            this.tableLayoutPanelMain.TabIndex = 0;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.tableLayoutPanelMain.SetColumnSpan(this.labelHeader, 4);
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(3, 6);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(771, 13);
            this.labelHeader.TabIndex = 0;
            this.labelHeader.Text = "A conflict occurred during the import of your data. Please choose one of the opti" +
    "ons below";
            // 
            // buttonInsert
            // 
            this.buttonInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonInsert.Image = global::DiversityCollection.Resource.Insert;
            this.buttonInsert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonInsert.Location = new System.Drawing.Point(403, 328);
            this.buttonInsert.Name = "buttonInsert";
            this.buttonInsert.Size = new System.Drawing.Size(195, 24);
            this.buttonInsert.TabIndex = 1;
            this.buttonInsert.Text = "Insert the values in a new dataset";
            this.buttonInsert.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonInsert.UseVisualStyleBackColor = true;
            this.buttonInsert.Click += new System.EventHandler(this.buttonInsert_Click);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUpdate.Image = global::DiversityCollection.Resource.Update;
            this.buttonUpdate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonUpdate.Location = new System.Drawing.Point(604, 328);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(170, 24);
            this.buttonUpdate.TabIndex = 2;
            this.buttonUpdate.Text = "Update the selected dataset";
            this.buttonUpdate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonIgnore
            // 
            this.tableLayoutPanelMain.SetColumnSpan(this.buttonIgnore, 2);
            this.buttonIgnore.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonIgnore.Image = global::DiversityCollection.Resource.Delete;
            this.buttonIgnore.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonIgnore.Location = new System.Drawing.Point(3, 328);
            this.buttonIgnore.Name = "buttonIgnore";
            this.buttonIgnore.Size = new System.Drawing.Size(160, 24);
            this.buttonIgnore.TabIndex = 3;
            this.buttonIgnore.Text = "Ignore the values in the file";
            this.buttonIgnore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonIgnore.UseVisualStyleBackColor = true;
            this.buttonIgnore.Click += new System.EventHandler(this.buttonIgnore_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanelMain.SetColumnSpan(this.dataGridView, 4);
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 85);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(771, 237);
            this.dataGridView.TabIndex = 4;
            this.dataGridView.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_RowHeaderMouseClick);
            // 
            // labelAction
            // 
            this.labelAction.AutoSize = true;
            this.labelAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelAction.Location = new System.Drawing.Point(3, 25);
            this.labelAction.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelAction.Name = "labelAction";
            this.labelAction.Size = new System.Drawing.Size(84, 19);
            this.labelAction.TabIndex = 6;
            this.labelAction.Text = "Intended action:";
            this.labelAction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelTable
            // 
            this.labelTable.AutoSize = true;
            this.labelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTable.Location = new System.Drawing.Point(3, 44);
            this.labelTable.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(84, 19);
            this.labelTable.TabIndex = 5;
            this.labelTable.Text = "Table:";
            this.labelTable.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelProblem
            // 
            this.labelProblem.AutoSize = true;
            this.labelProblem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelProblem.Location = new System.Drawing.Point(3, 63);
            this.labelProblem.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelProblem.Name = "labelProblem";
            this.labelProblem.Size = new System.Drawing.Size(84, 19);
            this.labelProblem.TabIndex = 7;
            this.labelProblem.Text = "Problem:";
            this.labelProblem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxAction
            // 
            this.textBoxAction.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxAction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxAction, 3);
            this.textBoxAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAction.Location = new System.Drawing.Point(90, 28);
            this.textBoxAction.Name = "textBoxAction";
            this.textBoxAction.ReadOnly = true;
            this.textBoxAction.Size = new System.Drawing.Size(684, 13);
            this.textBoxAction.TabIndex = 8;
            this.textBoxAction.Text = "Insert";
            // 
            // textBoxTable
            // 
            this.textBoxTable.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxTable, 3);
            this.textBoxTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTable.Location = new System.Drawing.Point(90, 47);
            this.textBoxTable.Name = "textBoxTable";
            this.textBoxTable.ReadOnly = true;
            this.textBoxTable.Size = new System.Drawing.Size(684, 13);
            this.textBoxTable.TabIndex = 9;
            this.textBoxTable.Text = "CollectionSpecimen";
            // 
            // textBoxProblem
            // 
            this.textBoxProblem.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxProblem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelMain.SetColumnSpan(this.textBoxProblem, 3);
            this.textBoxProblem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProblem.Location = new System.Drawing.Point(90, 66);
            this.textBoxProblem.Name = "textBoxProblem";
            this.textBoxProblem.ReadOnly = true;
            this.textBoxProblem.Size = new System.Drawing.Size(684, 13);
            this.textBoxProblem.TabIndex = 10;
            this.textBoxProblem.Text = "Several Datasets";
            // 
            // FormImportWizardCheckError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 355);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.helpProvider.SetHelpKeyword(this, "Import wizard");
            this.helpProvider.SetHelpNavigator(this, System.Windows.Forms.HelpNavigator.KeywordIndex);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormImportWizardCheckError";
            this.helpProvider.SetShowHelp(this, true);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error handling";
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.Label labelHeader;
        private System.Windows.Forms.Button buttonInsert;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonIgnore;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label labelAction;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.Label labelProblem;
        private System.Windows.Forms.TextBox textBoxAction;
        private System.Windows.Forms.TextBox textBoxTable;
        private System.Windows.Forms.TextBox textBoxProblem;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}