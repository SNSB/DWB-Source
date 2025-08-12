namespace DiversityWorkbench.XslEditor
{
    partial class FormXslTableEditor
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
            this.tableLayoutPanelDesigner = new System.Windows.Forms.TableLayoutPanel();
            this.toolStripRows = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRowAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRowRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripColumns = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonColumnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonColumnRemove = new System.Windows.Forms.ToolStripButton();
            this.panelRows = new System.Windows.Forms.Panel();
            this.panelColumns = new System.Windows.Forms.Panel();
            this.labelZoom = new System.Windows.Forms.Label();
            this.numericUpDownZoom = new System.Windows.Forms.NumericUpDown();
            this.panelTableEditor = new System.Windows.Forms.Panel();
            this.tableLayoutPanelRowColumnEditor = new System.Windows.Forms.TableLayoutPanel();
            this.userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            this.tableLayoutPanelDesigner.SuspendLayout();
            this.toolStripRows.SuspendLayout();
            this.toolStripColumns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoom)).BeginInit();
            this.panelTableEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelDesigner
            // 
            this.tableLayoutPanelDesigner.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanelDesigner.ColumnCount = 3;
            this.tableLayoutPanelDesigner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelDesigner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelDesigner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDesigner.Controls.Add(this.toolStripRows, 0, 2);
            this.tableLayoutPanelDesigner.Controls.Add(this.toolStripColumns, 2, 0);
            this.tableLayoutPanelDesigner.Controls.Add(this.panelRows, 1, 2);
            this.tableLayoutPanelDesigner.Controls.Add(this.panelColumns, 2, 1);
            this.tableLayoutPanelDesigner.Controls.Add(this.labelZoom, 0, 1);
            this.tableLayoutPanelDesigner.Controls.Add(this.numericUpDownZoom, 0, 0);
            this.tableLayoutPanelDesigner.Controls.Add(this.panelTableEditor, 2, 2);
            this.tableLayoutPanelDesigner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDesigner.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelDesigner.Name = "tableLayoutPanelDesigner";
            this.tableLayoutPanelDesigner.RowCount = 3;
            this.tableLayoutPanelDesigner.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDesigner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelDesigner.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelDesigner.Size = new System.Drawing.Size(893, 462);
            this.tableLayoutPanelDesigner.TabIndex = 1;
            // 
            // toolStripRows
            // 
            this.toolStripRows.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStripRows.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripRows.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRowAdd,
            this.toolStripButtonRowRemove});
            this.toolStripRows.Location = new System.Drawing.Point(0, 45);
            this.toolStripRows.Name = "toolStripRows";
            this.toolStripRows.Size = new System.Drawing.Size(24, 444);
            this.toolStripRows.TabIndex = 2;
            this.toolStripRows.Text = "toolStrip1";
            // 
            // toolStripButtonRowAdd
            // 
            this.toolStripButtonRowAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRowAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonRowAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRowAdd.Name = "toolStripButtonRowAdd";
            this.toolStripButtonRowAdd.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonRowAdd.Text = "Add a new row";
            this.toolStripButtonRowAdd.Click += new System.EventHandler(this.toolStripButtonRowAdd_Click);
            // 
            // toolStripButtonRowRemove
            // 
            this.toolStripButtonRowRemove.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonRowRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRowRemove.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonRowRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRowRemove.Name = "toolStripButtonRowRemove";
            this.toolStripButtonRowRemove.Size = new System.Drawing.Size(21, 20);
            this.toolStripButtonRowRemove.Text = "Remove row";
            this.toolStripButtonRowRemove.Click += new System.EventHandler(this.toolStripButtonRowRemove_Click);
            // 
            // toolStripColumns
            // 
            this.toolStripColumns.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripColumns.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonColumnAdd,
            this.toolStripButtonColumnRemove});
            this.toolStripColumns.Location = new System.Drawing.Point(64, 0);
            this.toolStripColumns.Name = "toolStripColumns";
            this.toolStripColumns.Size = new System.Drawing.Size(829, 25);
            this.toolStripColumns.TabIndex = 1;
            this.toolStripColumns.Text = "toolStripColumns";
            // 
            // toolStripButtonColumnAdd
            // 
            this.toolStripButtonColumnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonColumnAdd.Image = global::DiversityWorkbench.Properties.Resources.Add;
            this.toolStripButtonColumnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonColumnAdd.Name = "toolStripButtonColumnAdd";
            this.toolStripButtonColumnAdd.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonColumnAdd.Text = "Add column";
            this.toolStripButtonColumnAdd.Click += new System.EventHandler(this.toolStripButtonColumnAdd_Click);
            // 
            // toolStripButtonColumnRemove
            // 
            this.toolStripButtonColumnRemove.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonColumnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonColumnRemove.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.toolStripButtonColumnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonColumnRemove.Name = "toolStripButtonColumnRemove";
            this.toolStripButtonColumnRemove.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonColumnRemove.Text = "Remove column";
            this.toolStripButtonColumnRemove.Click += new System.EventHandler(this.toolStripButtonColumnRemove_Click);
            // 
            // panelRows
            // 
            this.panelRows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRows.Location = new System.Drawing.Point(24, 45);
            this.panelRows.Margin = new System.Windows.Forms.Padding(0);
            this.panelRows.Name = "panelRows";
            this.panelRows.Size = new System.Drawing.Size(40, 444);
            this.panelRows.TabIndex = 5;
            // 
            // panelColumns
            // 
            this.panelColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelColumns.Location = new System.Drawing.Point(64, 25);
            this.panelColumns.Margin = new System.Windows.Forms.Padding(0);
            this.panelColumns.Name = "panelColumns";
            this.panelColumns.Size = new System.Drawing.Size(829, 20);
            this.panelColumns.TabIndex = 6;
            // 
            // labelZoom
            // 
            this.labelZoom.AutoSize = true;
            this.tableLayoutPanelDesigner.SetColumnSpan(this.labelZoom, 2);
            this.labelZoom.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelZoom.Location = new System.Drawing.Point(3, 25);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new System.Drawing.Size(58, 13);
            this.labelZoom.TabIndex = 7;
            this.labelZoom.Text = "Zoom %";
            this.labelZoom.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // numericUpDownZoom
            // 
            this.tableLayoutPanelDesigner.SetColumnSpan(this.numericUpDownZoom, 2);
            this.numericUpDownZoom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.numericUpDownZoom.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownZoom.Location = new System.Drawing.Point(3, 5);
            this.numericUpDownZoom.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.numericUpDownZoom.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownZoom.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numericUpDownZoom.Name = "numericUpDownZoom";
            this.numericUpDownZoom.Size = new System.Drawing.Size(58, 20);
            this.numericUpDownZoom.TabIndex = 8;
            this.numericUpDownZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDownZoom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDownZoom.ValueChanged += new System.EventHandler(this.numericUpDownZoom_ValueChanged);
            // 
            // panelTableEditor
            // 
            this.panelTableEditor.AutoScroll = true;
            this.panelTableEditor.Controls.Add(this.tableLayoutPanelRowColumnEditor);
            this.panelTableEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTableEditor.Location = new System.Drawing.Point(64, 45);
            this.panelTableEditor.Margin = new System.Windows.Forms.Padding(0);
            this.panelTableEditor.Name = "panelTableEditor";
            this.panelTableEditor.Size = new System.Drawing.Size(829, 444);
            this.panelTableEditor.TabIndex = 9;
            // 
            // tableLayoutPanelRowColumnEditor
            // 
            this.tableLayoutPanelRowColumnEditor.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanelRowColumnEditor.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanelRowColumnEditor.ColumnCount = 3;
            this.tableLayoutPanelRowColumnEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRowColumnEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRowColumnEditor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRowColumnEditor.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelRowColumnEditor.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelRowColumnEditor.Name = "tableLayoutPanelRowColumnEditor";
            this.tableLayoutPanelRowColumnEditor.RowCount = 3;
            this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelRowColumnEditor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelRowColumnEditor.Size = new System.Drawing.Size(178, 111);
            this.tableLayoutPanelRowColumnEditor.TabIndex = 3;
            // 
            // userControlDialogPanel
            // 
            this.userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDialogPanel.Location = new System.Drawing.Point(0, 462);
            this.userControlDialogPanel.Name = "userControlDialogPanel";
            this.userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.userControlDialogPanel.Size = new System.Drawing.Size(893, 27);
            this.userControlDialogPanel.TabIndex = 2;
            // 
            // FormXslTableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 489);
            this.Controls.Add(this.tableLayoutPanelDesigner);
            this.Controls.Add(this.userControlDialogPanel);
            this.Name = "FormXslTableEditor";
            this.Text = "FormXslTableEditor";
            this.tableLayoutPanelDesigner.ResumeLayout(false);
            this.tableLayoutPanelDesigner.PerformLayout();
            this.toolStripRows.ResumeLayout(false);
            this.toolStripRows.PerformLayout();
            this.toolStripColumns.ResumeLayout(false);
            this.toolStripColumns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZoom)).EndInit();
            this.panelTableEditor.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDesigner;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelRowColumnEditor;
        private System.Windows.Forms.ToolStrip toolStripRows;
        private System.Windows.Forms.ToolStripButton toolStripButtonRowAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonRowRemove;
        private System.Windows.Forms.ToolStrip toolStripColumns;
        private System.Windows.Forms.ToolStripButton toolStripButtonColumnAdd;
        private System.Windows.Forms.ToolStripButton toolStripButtonColumnRemove;
        private System.Windows.Forms.Panel panelRows;
        private System.Windows.Forms.Panel panelColumns;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelZoom;
        private System.Windows.Forms.NumericUpDown numericUpDownZoom;
        private System.Windows.Forms.Panel panelTableEditor;
    }
}