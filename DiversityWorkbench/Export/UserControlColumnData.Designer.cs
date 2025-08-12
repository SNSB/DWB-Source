namespace DiversityWorkbench.Export
{
    partial class UserControlColumnData
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.labelSourceTable = new System.Windows.Forms.Label();
            this.labelSourceColumnOrUnitvalue = new System.Windows.Forms.Label();
            this.buttonTransformation = new System.Windows.Forms.Button();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.textBoxPostfix = new System.Windows.Forms.TextBox();
            this.labelPrefix = new System.Windows.Forms.Label();
            this.labelPostfix = new System.Windows.Forms.Label();
            this.panelBorder = new System.Windows.Forms.Panel();
            this.tableLayoutPanel.SuspendLayout();
            this.panelBorder.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 3;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel.Controls.Add(this.labelSourceTable, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.labelSourceColumnOrUnitvalue, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.buttonTransformation, 1, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxPrefix, 0, 3);
            this.tableLayoutPanel.Controls.Add(this.textBoxPostfix, 2, 3);
            this.tableLayoutPanel.Controls.Add(this.labelPrefix, 0, 2);
            this.tableLayoutPanel.Controls.Add(this.labelPostfix, 2, 2);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 4;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(107, 98);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelSourceTable
            // 
            this.labelSourceTable.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelSourceTable, 3);
            this.labelSourceTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelSourceTable.Location = new System.Drawing.Point(3, 0);
            this.labelSourceTable.Name = "labelSourceTable";
            this.labelSourceTable.Size = new System.Drawing.Size(101, 13);
            this.labelSourceTable.TabIndex = 0;
            this.labelSourceTable.Text = "label1";
            // 
            // labelSourceColumnOrUnitvalue
            // 
            this.labelSourceColumnOrUnitvalue.AutoSize = true;
            this.tableLayoutPanel.SetColumnSpan(this.labelSourceColumnOrUnitvalue, 3);
            this.labelSourceColumnOrUnitvalue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSourceColumnOrUnitvalue.Location = new System.Drawing.Point(3, 13);
            this.labelSourceColumnOrUnitvalue.Name = "labelSourceColumnOrUnitvalue";
            this.labelSourceColumnOrUnitvalue.Size = new System.Drawing.Size(101, 13);
            this.labelSourceColumnOrUnitvalue.TabIndex = 1;
            this.labelSourceColumnOrUnitvalue.Text = "label1";
            // 
            // buttonTransformation
            // 
            this.buttonTransformation.Image = global::DiversityWorkbench.Properties.Resources.Conflict;
            this.buttonTransformation.Location = new System.Drawing.Point(41, 46);
            this.buttonTransformation.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTransformation.Name = "buttonTransformation";
            this.buttonTransformation.Size = new System.Drawing.Size(24, 24);
            this.buttonTransformation.TabIndex = 2;
            this.buttonTransformation.UseVisualStyleBackColor = true;
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.Location = new System.Drawing.Point(3, 49);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(35, 20);
            this.textBoxPrefix.TabIndex = 3;
            // 
            // textBoxPostfix
            // 
            this.textBoxPostfix.Location = new System.Drawing.Point(68, 49);
            this.textBoxPostfix.Name = "textBoxPostfix";
            this.textBoxPostfix.Size = new System.Drawing.Size(36, 20);
            this.textBoxPostfix.TabIndex = 4;
            // 
            // labelPrefix
            // 
            this.labelPrefix.AutoSize = true;
            this.labelPrefix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPrefix.Location = new System.Drawing.Point(3, 26);
            this.labelPrefix.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelPrefix.Name = "labelPrefix";
            this.labelPrefix.Size = new System.Drawing.Size(38, 20);
            this.labelPrefix.TabIndex = 5;
            this.labelPrefix.Text = "Prefix";
            this.labelPrefix.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelPostfix
            // 
            this.labelPostfix.AutoSize = true;
            this.labelPostfix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPostfix.Location = new System.Drawing.Point(65, 26);
            this.labelPostfix.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.labelPostfix.Name = "labelPostfix";
            this.labelPostfix.Size = new System.Drawing.Size(39, 20);
            this.labelPostfix.TabIndex = 6;
            this.labelPostfix.Text = "Postfix";
            this.labelPostfix.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // panelBorder
            // 
            this.panelBorder.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelBorder.Controls.Add(this.tableLayoutPanel);
            this.panelBorder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBorder.Location = new System.Drawing.Point(0, 0);
            this.panelBorder.Name = "panelBorder";
            this.panelBorder.Size = new System.Drawing.Size(111, 102);
            this.panelBorder.TabIndex = 1;
            // 
            // UserControlColumnData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBorder);
            this.Name = "UserControlColumnData";
            this.Size = new System.Drawing.Size(111, 102);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.panelBorder.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelSourceTable;
        private System.Windows.Forms.Label labelSourceColumnOrUnitvalue;
        private System.Windows.Forms.Button buttonTransformation;
        private System.Windows.Forms.TextBox textBoxPrefix;
        private System.Windows.Forms.TextBox textBoxPostfix;
        private System.Windows.Forms.Label labelPrefix;
        private System.Windows.Forms.Label labelPostfix;
        private System.Windows.Forms.Panel panelBorder;
    }
}
