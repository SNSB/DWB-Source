namespace DiversityWorkbench.Export
{
    partial class UserControlTable
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxTable = new System.Windows.Forms.PictureBox();
            this.labelTable = new System.Windows.Forms.Label();
            this.panelSpacer = new System.Windows.Forms.Panel();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAddDependent = new System.Windows.Forms.Button();
            this.buttonAddParallel = new System.Windows.Forms.Button();
            this.buttonAddParallelMulti = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 10;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.pictureBoxTable, 5, 0);
            this.tableLayoutPanel.Controls.Add(this.labelTable, 6, 0);
            this.tableLayoutPanel.Controls.Add(this.panelSpacer, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonDelete, 9, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonAddDependent, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonAddParallel, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonAddParallelMulti, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 1;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(757, 28);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // pictureBoxTable
            // 
            this.pictureBoxTable.Location = new System.Drawing.Point(92, 6);
            this.pictureBoxTable.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.pictureBoxTable.Name = "pictureBoxTable";
            this.pictureBoxTable.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxTable.TabIndex = 4;
            this.pictureBoxTable.TabStop = false;
            this.pictureBoxTable.Click += new System.EventHandler(this.pictureBoxTable_Click);
            // 
            // labelTable
            // 
            this.labelTable.AllowDrop = true;
            this.labelTable.AutoSize = true;
            this.labelTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTable.Location = new System.Drawing.Point(108, 0);
            this.labelTable.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelTable.Name = "labelTable";
            this.labelTable.Size = new System.Drawing.Size(626, 28);
            this.labelTable.TabIndex = 5;
            this.labelTable.Text = "label1";
            this.labelTable.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTable.Click += new System.EventHandler(this.labelTable_Click);
            this.labelTable.DragDrop += new System.Windows.Forms.DragEventHandler(this.labelTable_DragDrop);
            this.labelTable.DragEnter += new System.Windows.Forms.DragEventHandler(this.labelTable_DragEnter);
            this.labelTable.DragOver += new System.Windows.Forms.DragEventHandler(this.labelTable_DragOver);
            // 
            // panelSpacer
            // 
            this.panelSpacer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelSpacer.Location = new System.Drawing.Point(72, 0);
            this.panelSpacer.Margin = new System.Windows.Forms.Padding(0);
            this.panelSpacer.Name = "panelSpacer";
            this.panelSpacer.Size = new System.Drawing.Size(20, 28);
            this.panelSpacer.TabIndex = 6;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(737, 2);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(20, 24);
            this.buttonDelete.TabIndex = 7;
            this.toolTip.SetToolTip(this.buttonDelete, "Remove this table from the list");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Visible = false;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAddDependent
            // 
            this.buttonAddDependent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddDependent.Image = global::DiversityWorkbench.Properties.Resources.AddChild;
            this.buttonAddDependent.Location = new System.Drawing.Point(48, 2);
            this.buttonAddDependent.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.buttonAddDependent.Name = "buttonAddDependent";
            this.buttonAddDependent.Size = new System.Drawing.Size(24, 24);
            this.buttonAddDependent.TabIndex = 9;
            this.toolTip.SetToolTip(this.buttonAddDependent, "Add a dependent table");
            this.buttonAddDependent.UseVisualStyleBackColor = true;
            this.buttonAddDependent.Click += new System.EventHandler(this.buttonAddDependent_Click);
            // 
            // buttonAddParallel
            // 
            this.buttonAddParallel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddParallel.Image = global::DiversityWorkbench.ResourceWorkbench.New;
            this.buttonAddParallel.Location = new System.Drawing.Point(24, 2);
            this.buttonAddParallel.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.buttonAddParallel.Name = "buttonAddParallel";
            this.buttonAddParallel.Size = new System.Drawing.Size(24, 24);
            this.buttonAddParallel.TabIndex = 10;
            this.toolTip.SetToolTip(this.buttonAddParallel, "Add a parallel table");
            this.buttonAddParallel.UseVisualStyleBackColor = true;
            this.buttonAddParallel.Click += new System.EventHandler(this.buttonAddParallel_Click);
            // 
            // buttonAddParallelMulti
            // 
            this.buttonAddParallelMulti.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddParallelMulti.Image = global::DiversityWorkbench.ResourceWorkbench.NewMulti;
            this.buttonAddParallelMulti.Location = new System.Drawing.Point(0, 2);
            this.buttonAddParallelMulti.Margin = new System.Windows.Forms.Padding(0, 2, 0, 2);
            this.buttonAddParallelMulti.Name = "buttonAddParallelMulti";
            this.buttonAddParallelMulti.Size = new System.Drawing.Size(24, 24);
            this.buttonAddParallelMulti.TabIndex = 11;
            this.toolTip.SetToolTip(this.buttonAddParallelMulti, "Add as many parallel tables as there are parallel data");
            this.buttonAddParallelMulti.UseVisualStyleBackColor = true;
            this.buttonAddParallelMulti.Click += new System.EventHandler(this.buttonAddParallelMulti_Click);
            // 
            // UserControlTable
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlTable";
            this.Size = new System.Drawing.Size(757, 28);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.UserControlTable_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.UserControlTable_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.UserControlTable_DragOver);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.PictureBox pictureBoxTable;
        private System.Windows.Forms.Label labelTable;
        private System.Windows.Forms.Panel panelSpacer;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAddDependent;
        private System.Windows.Forms.Button buttonAddParallel;
        private System.Windows.Forms.Button buttonAddParallelMulti;
    }
}
