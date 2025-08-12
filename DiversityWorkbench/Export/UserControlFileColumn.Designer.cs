namespace DiversityWorkbench.Export
{
    partial class UserControlFileColumn
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
            this.textBoxHeader = new System.Windows.Forms.TextBox();
            this.panelFileColumnSeparator = new System.Windows.Forms.Panel();
            this.buttonMoveLeft = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonMoveRight = new System.Windows.Forms.Button();
            this.labelTableColumn = new System.Windows.Forms.Label();
            this.buttonTransform = new System.Windows.Forms.Button();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.labelPostfix = new System.Windows.Forms.Label();
            this.textBoxPostfix = new System.Windows.Forms.TextBox();
            this.labelSource = new System.Windows.Forms.Label();
            this.labelUnitSource = new System.Windows.Forms.Label();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.numericUpDownPosition = new System.Windows.Forms.NumericUpDown();
            this.buttonMoveToPosition = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Controls.Add(this.textBoxHeader, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.panelFileColumnSeparator, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonMoveLeft, 1, 5);
            this.tableLayoutPanel.Controls.Add(this.buttonDelete, 4, 5);
            this.tableLayoutPanel.Controls.Add(this.buttonMoveRight, 5, 5);
            this.tableLayoutPanel.Controls.Add(this.labelTableColumn, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.buttonTransform, 3, 5);
            this.tableLayoutPanel.Controls.Add(this.labelPrefix, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxPrefix, 1, 4);
            this.tableLayoutPanel.Controls.Add(this.labelPostfix, 5, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxPostfix, 5, 4);
            this.tableLayoutPanel.Controls.Add(this.labelSource, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelUnitSource, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.buttonFilter, 2, 7);
            this.tableLayoutPanel.Controls.Add(this.numericUpDownPosition, 2, 5);
            this.tableLayoutPanel.Controls.Add(this.buttonMoveToPosition, 2, 6);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 8;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(164, 137);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // textBoxHeader
            // 
            this.tableLayoutPanel.SetColumnSpan(this.textBoxHeader, 5);
            this.textBoxHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHeader.Location = new System.Drawing.Point(11, 3);
            this.textBoxHeader.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.textBoxHeader.Name = "textBoxHeader";
            this.textBoxHeader.Size = new System.Drawing.Size(150, 20);
            this.textBoxHeader.TabIndex = 0;
            this.toolTip.SetToolTip(this.textBoxHeader, "The header text of the column");
            this.textBoxHeader.TextChanged += new System.EventHandler(this.textBoxHeader_TextChanged);
            // 
            // panelFileColumnSeparator
            // 
            this.panelFileColumnSeparator.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panelFileColumnSeparator.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFileColumnSeparator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFileColumnSeparator.Location = new System.Drawing.Point(1, 1);
            this.panelFileColumnSeparator.Margin = new System.Windows.Forms.Padding(1);
            this.panelFileColumnSeparator.Name = "panelFileColumnSeparator";
            this.tableLayoutPanel.SetRowSpan(this.panelFileColumnSeparator, 7);
            this.panelFileColumnSeparator.Size = new System.Drawing.Size(6, 115);
            this.panelFileColumnSeparator.TabIndex = 4;
            this.toolTip.SetToolTip(this.panelFileColumnSeparator, "Separate to previous column");
            this.panelFileColumnSeparator.Click += new System.EventHandler(this.panelFileColumnSeparator_Click);
            // 
            // buttonMoveLeft
            // 
            this.buttonMoveLeft.Image = global::DiversityWorkbench.Properties.Resources.ArrowPrevious;
            this.buttonMoveLeft.Location = new System.Drawing.Point(11, 90);
            this.buttonMoveLeft.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.buttonMoveLeft.Name = "buttonMoveLeft";
            this.tableLayoutPanel.SetRowSpan(this.buttonMoveLeft, 2);
            this.buttonMoveLeft.Size = new System.Drawing.Size(20, 23);
            this.buttonMoveLeft.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonMoveLeft, "Move column to left");
            this.buttonMoveLeft.UseVisualStyleBackColor = true;
            this.buttonMoveLeft.Click += new System.EventHandler(this.buttonMoveLeft_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonDelete.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(104, 90);
            this.buttonDelete.Name = "buttonDelete";
            this.tableLayoutPanel.SetRowSpan(this.buttonDelete, 2);
            this.buttonDelete.Size = new System.Drawing.Size(24, 24);
            this.buttonDelete.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonDelete, "Remove this column");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonMoveRight
            // 
            this.buttonMoveRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonMoveRight.Image = global::DiversityWorkbench.Properties.Resources.ArrowNext;
            this.buttonMoveRight.Location = new System.Drawing.Point(141, 90);
            this.buttonMoveRight.Name = "buttonMoveRight";
            this.tableLayoutPanel.SetRowSpan(this.buttonMoveRight, 2);
            this.buttonMoveRight.Size = new System.Drawing.Size(20, 24);
            this.buttonMoveRight.TabIndex = 6;
            this.toolTip.SetToolTip(this.buttonMoveRight, "Move column to right");
            this.buttonMoveRight.UseVisualStyleBackColor = true;
            this.buttonMoveRight.Click += new System.EventHandler(this.buttonMoveRight_Click);
            // 
            // labelTableColumn
            // 
            this.labelTableColumn.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelTableColumn, 3);
            this.labelTableColumn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTableColumn.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelTableColumn.Location = new System.Drawing.Point(34, 48);
            this.labelTableColumn.Margin = new System.Windows.Forms.Padding(0);
            this.labelTableColumn.Name = "labelTableColumn";
            this.tableLayoutPanel.SetRowSpan(this.labelTableColumn, 2);
            this.labelTableColumn.Size = new System.Drawing.Size(100, 39);
            this.labelTableColumn.TabIndex = 7;
            this.labelTableColumn.Text = "Column";
            this.labelTableColumn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.labelTableColumn, "Column or datafield within the source");
            // 
            // buttonTransform
            // 
            this.buttonTransform.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonTransform.Image = global::DiversityWorkbench.Properties.Resources.Conflict;
            this.buttonTransform.Location = new System.Drawing.Point(74, 90);
            this.buttonTransform.Name = "buttonTransform";
            this.tableLayoutPanel.SetRowSpan(this.buttonTransform, 2);
            this.buttonTransform.Size = new System.Drawing.Size(24, 24);
            this.buttonTransform.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonTransform, "Transformations of the column");
            this.buttonTransform.UseVisualStyleBackColor = true;
            this.buttonTransform.Click += new System.EventHandler(this.buttonTransform_Click);
            // 
            // labelPrefix
            // 
            this.labelPrefix.AutoSize = true;
            this.labelPrefix.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelPrefix.Location = new System.Drawing.Point(11, 48);
            this.labelPrefix.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(23, 13);
            this.labelPrefix.TabIndex = 9;
            this.labelPrefix.Text = "Pre";
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxPrefix.Location = new System.Drawing.Point(11, 64);
            this.textBoxPrefix.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(23, 20);
            this.textBoxPrefix.TabIndex = 10;
            this.toolTip.SetToolTip(this.textBoxPrefix, "Text inserted in front of the content of the column");
            this.textBoxPrefix.TextChanged += new System.EventHandler(this.textBoxPrefix_TextChanged);
            // 
            // labelPostfix
            // 
            this.labelPostfix.AutoSize = true;
            this.labelPostfix.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelPostfix.Location = new System.Drawing.Point(134, 48);
            this.labelPostfix.Margin = new System.Windows.Forms.Padding(0);
            this.labelPostfix.Name = "labelPostfix";
            this.labelPostfix.Size = new System.Drawing.Size(30, 13);
            this.labelPostfix.TabIndex = 11;
            this.labelPostfix.Text = "Post";
            // 
            // textBoxPostfix
            // 
            this.textBoxPostfix.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxPostfix.Location = new System.Drawing.Point(137, 64);
            this.textBoxPostfix.Name = "textBoxPostfix";
            this.textBoxPostfix.Size = new System.Drawing.Size(24, 20);
            this.textBoxPostfix.TabIndex = 12;
            this.toolTip.SetToolTip(this.textBoxPostfix, "Text inserted behind the content of the column");
            this.textBoxPostfix.TextChanged += new System.EventHandler(this.textBoxPostfix_TextChanged);
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelSource, 5);
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelSource.Location = new System.Drawing.Point(11, 23);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(150, 12);
            this.labelSource.TabIndex = 15;
            this.labelSource.Text = "Table";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.toolTip.SetToolTip(this.labelSource, "Source for the data");
            // 
            // labelUnitSource
            // 
            this.labelUnitSource.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelUnitSource, 5);
            this.labelUnitSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelUnitSource.Location = new System.Drawing.Point(11, 35);
            this.labelUnitSource.Name = "labelUnitSource";
            this.labelUnitSource.Size = new System.Drawing.Size(150, 13);
            this.labelUnitSource.TabIndex = 16;
            // 
            // buttonFilter
            // 
            this.tableLayoutPanel.SetColumnSpan(this.buttonFilter, 3);
            this.buttonFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFilter.Image = global::DiversityWorkbench.Properties.Resources.Filter;
            this.buttonFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonFilter.Location = new System.Drawing.Point(37, 117);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(94, 20);
            this.buttonFilter.TabIndex = 17;
            this.buttonFilter.Text = "Filter";
            this.buttonFilter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Visible = false;
            // 
            // numericUpDownPosition
            // 
            this.numericUpDownPosition.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDownPosition.Location = new System.Drawing.Point(34, 87);
            this.numericUpDownPosition.Margin = new System.Windows.Forms.Padding(0);
            this.numericUpDownPosition.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.numericUpDownPosition.Name = "numericUpDownPosition";
            this.numericUpDownPosition.Size = new System.Drawing.Size(34, 16);
            this.numericUpDownPosition.TabIndex = 18;
            this.numericUpDownPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip.SetToolTip(this.numericUpDownPosition, "The current position");
            this.numericUpDownPosition.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // buttonMoveToPosition
            // 
            this.buttonMoveToPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMoveToPosition.Image = global::DiversityWorkbench.Properties.Resources.ArrowDown;
            this.buttonMoveToPosition.Location = new System.Drawing.Point(37, 103);
            this.buttonMoveToPosition.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.buttonMoveToPosition.Name = "buttonMoveToPosition";
            this.buttonMoveToPosition.Size = new System.Drawing.Size(28, 14);
            this.buttonMoveToPosition.TabIndex = 19;
            this.toolTip.SetToolTip(this.buttonMoveToPosition, "Move the column to the given position");
            this.buttonMoveToPosition.UseVisualStyleBackColor = true;
            this.buttonMoveToPosition.Click += new System.EventHandler(this.buttonMoveToPosition_Click);
            // 
            // UserControlFileColumn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlFileColumn";
            this.Size = new System.Drawing.Size(164, 137);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosition)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.TextBox textBoxHeader;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Panel panelFileColumnSeparator;
        private System.Windows.Forms.Button buttonMoveLeft;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonMoveRight;
        private System.Windows.Forms.Label labelTableColumn;
        private System.Windows.Forms.Button buttonTransform;
        private System.Windows.Forms.Label labelPrefix;
        private System.Windows.Forms.TextBox textBoxPrefix;
        private System.Windows.Forms.Label labelPostfix;
        private System.Windows.Forms.TextBox textBoxPostfix;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelUnitSource;
        private System.Windows.Forms.HelpProvider helpProvider;
        public System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.NumericUpDown numericUpDownPosition;
        private System.Windows.Forms.Button buttonMoveToPosition;
    }
}
