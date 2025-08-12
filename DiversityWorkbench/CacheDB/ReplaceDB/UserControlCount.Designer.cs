namespace DiversityWorkbench.CacheDB.ReplaceDB
{
    partial class UserControlCount
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
            this.labelHeaderSource = new System.Windows.Forms.Label();
            this.labelHeaderOld = new System.Windows.Forms.Label();
            this.labelHeaderNew = new System.Windows.Forms.Label();
            this.buttonState = new System.Windows.Forms.Button();
            this.labelSource = new System.Windows.Forms.Label();
            this.labelOld = new System.Windows.Forms.Label();
            this.labelNew = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labelHeader = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 4;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel.Controls.Add(this.labelHeaderSource, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.labelHeaderOld, 2, 0);
            this.tableLayoutPanel.Controls.Add(this.labelHeaderNew, 3, 0);
            this.tableLayoutPanel.Controls.Add(this.buttonState, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.labelSource, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.labelOld, 2, 1);
            this.tableLayoutPanel.Controls.Add(this.labelNew, 3, 1);
            this.tableLayoutPanel.Controls.Add(this.labelHeader, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(352, 101);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // labelHeaderSource
            // 
            this.labelHeaderSource.AutoSize = true;
            this.labelHeaderSource.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelHeaderSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeaderSource.Location = new System.Drawing.Point(20, 0);
            this.labelHeaderSource.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeaderSource.Name = "labelHeaderSource";
            this.labelHeaderSource.Size = new System.Drawing.Size(212, 13);
            this.labelHeaderSource.TabIndex = 0;
            this.labelHeaderSource.Text = " Source";
            this.labelHeaderSource.Visible = false;
            // 
            // labelHeaderOld
            // 
            this.labelHeaderOld.AutoSize = true;
            this.labelHeaderOld.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelHeaderOld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeaderOld.ForeColor = System.Drawing.Color.Red;
            this.labelHeaderOld.Location = new System.Drawing.Point(232, 0);
            this.labelHeaderOld.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeaderOld.Name = "labelHeaderOld";
            this.labelHeaderOld.Size = new System.Drawing.Size(60, 13);
            this.labelHeaderOld.TabIndex = 1;
            this.labelHeaderOld.Text = "old ";
            this.labelHeaderOld.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelHeaderOld.Visible = false;
            // 
            // labelHeaderNew
            // 
            this.labelHeaderNew.AutoSize = true;
            this.labelHeaderNew.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelHeaderNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeaderNew.ForeColor = System.Drawing.Color.Green;
            this.labelHeaderNew.Location = new System.Drawing.Point(292, 0);
            this.labelHeaderNew.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeaderNew.Name = "labelHeaderNew";
            this.labelHeaderNew.Size = new System.Drawing.Size(60, 13);
            this.labelHeaderNew.TabIndex = 2;
            this.labelHeaderNew.Text = "new ";
            this.labelHeaderNew.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelHeaderNew.Visible = false;
            // 
            // buttonState
            // 
            this.buttonState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonState.FlatAppearance.BorderSize = 0;
            this.buttonState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonState.Location = new System.Drawing.Point(0, 13);
            this.buttonState.Margin = new System.Windows.Forms.Padding(0);
            this.buttonState.Name = "buttonState";
            this.buttonState.Size = new System.Drawing.Size(20, 88);
            this.buttonState.TabIndex = 3;
            this.buttonState.UseVisualStyleBackColor = true;
            // 
            // labelSource
            // 
            this.labelSource.AutoSize = true;
            this.labelSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSource.Location = new System.Drawing.Point(23, 13);
            this.labelSource.Name = "labelSource";
            this.labelSource.Size = new System.Drawing.Size(206, 88);
            this.labelSource.TabIndex = 4;
            this.labelSource.Text = "Source";
            this.labelSource.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelOld
            // 
            this.labelOld.AutoSize = true;
            this.labelOld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOld.ForeColor = System.Drawing.Color.Red;
            this.labelOld.Location = new System.Drawing.Point(235, 13);
            this.labelOld.Name = "labelOld";
            this.labelOld.Size = new System.Drawing.Size(54, 88);
            this.labelOld.TabIndex = 5;
            this.labelOld.Text = "-";
            this.labelOld.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNew
            // 
            this.labelNew.AutoSize = true;
            this.labelNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNew.ForeColor = System.Drawing.Color.Green;
            this.labelNew.Location = new System.Drawing.Point(295, 13);
            this.labelNew.Name = "labelNew";
            this.labelNew.Size = new System.Drawing.Size(54, 88);
            this.labelNew.TabIndex = 6;
            this.labelNew.Text = "-";
            this.labelNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeader
            // 
            this.labelHeader.AutoSize = true;
            this.labelHeader.BackColor = System.Drawing.SystemColors.ControlLight;
            this.labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeader.Location = new System.Drawing.Point(0, 0);
            this.labelHeader.Margin = new System.Windows.Forms.Padding(0);
            this.labelHeader.Name = "labelHeader";
            this.labelHeader.Size = new System.Drawing.Size(20, 13);
            this.labelHeader.TabIndex = 7;
            // 
            // UserControlCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "UserControlCount";
            this.Size = new System.Drawing.Size(352, 101);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelHeaderSource;
        private System.Windows.Forms.Label labelHeaderOld;
        private System.Windows.Forms.Label labelHeaderNew;
        private System.Windows.Forms.Button buttonState;
        private System.Windows.Forms.Label labelSource;
        private System.Windows.Forms.Label labelOld;
        private System.Windows.Forms.Label labelNew;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelHeader;
    }
}
