namespace DiversityWorkbench.UserControls
{
    partial class UserControlUpdateResult
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
            this.tableLayoutPanelUpdateResult = new System.Windows.Forms.TableLayoutPanel();
            this.buttonResult = new System.Windows.Forms.Button();
            this.textBoxVersionOld = new System.Windows.Forms.TextBox();
            this.textBoxVersionNew = new System.Windows.Forms.TextBox();
            this.textBoxResultFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOptional = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanelUpdateResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelUpdateResult
            // 
            this.tableLayoutPanelUpdateResult.ColumnCount = 8;
            this.tableLayoutPanelUpdateResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUpdateResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelUpdateResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUpdateResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanelUpdateResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelUpdateResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelUpdateResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelUpdateResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanelUpdateResult.Controls.Add(this.buttonResult, 7, 0);
            this.tableLayoutPanelUpdateResult.Controls.Add(this.textBoxVersionOld, 1, 0);
            this.tableLayoutPanelUpdateResult.Controls.Add(this.textBoxVersionNew, 3, 0);
            this.tableLayoutPanelUpdateResult.Controls.Add(this.textBoxResultFile, 6, 0);
            this.tableLayoutPanelUpdateResult.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelUpdateResult.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanelUpdateResult.Controls.Add(this.label3, 5, 0);
            this.tableLayoutPanelUpdateResult.Controls.Add(this.buttonOptional, 4, 0);
            this.tableLayoutPanelUpdateResult.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelUpdateResult.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelUpdateResult.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.tableLayoutPanelUpdateResult.Name = "tableLayoutPanelUpdateResult";
            this.tableLayoutPanelUpdateResult.RowCount = 1;
            this.tableLayoutPanelUpdateResult.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelUpdateResult.Size = new System.Drawing.Size(540, 28);
            this.tableLayoutPanelUpdateResult.TabIndex = 0;
            // 
            // buttonResult
            // 
            this.buttonResult.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonResult.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonResult.Location = new System.Drawing.Point(463, 3);
            this.buttonResult.Name = "buttonResult";
            this.buttonResult.Size = new System.Drawing.Size(74, 23);
            this.buttonResult.TabIndex = 0;
            this.buttonResult.Text = "Not started";
            this.buttonResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.toolTip.SetToolTip(this.buttonResult, "Script has not been executed yet, click on button \"Start update\"");
            this.buttonResult.UseVisualStyleBackColor = true;
            this.buttonResult.Click += new System.EventHandler(this.buttonResult_Click);
            // 
            // textBoxVersionOld
            // 
            this.textBoxVersionOld.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxVersionOld.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBoxVersionOld.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVersionOld.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVersionOld.Location = new System.Drawing.Point(39, 8);
            this.textBoxVersionOld.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.textBoxVersionOld.Name = "textBoxVersionOld";
            this.textBoxVersionOld.ReadOnly = true;
            this.textBoxVersionOld.Size = new System.Drawing.Size(64, 13);
            this.textBoxVersionOld.TabIndex = 1;
            this.textBoxVersionOld.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxVersionNew
            // 
            this.textBoxVersionNew.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxVersionNew.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBoxVersionNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxVersionNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxVersionNew.Location = new System.Drawing.Point(131, 8);
            this.textBoxVersionNew.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.textBoxVersionNew.Name = "textBoxVersionNew";
            this.textBoxVersionNew.ReadOnly = true;
            this.textBoxVersionNew.Size = new System.Drawing.Size(64, 13);
            this.textBoxVersionNew.TabIndex = 2;
            this.textBoxVersionNew.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxResultFile
            // 
            this.textBoxResultFile.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxResultFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.textBoxResultFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxResultFile.Location = new System.Drawing.Point(336, 8);
            this.textBoxResultFile.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
            this.textBoxResultFile.Name = "textBoxResultFile";
            this.textBoxResultFile.ReadOnly = true;
            this.textBoxResultFile.Size = new System.Drawing.Size(121, 13);
            this.textBoxResultFile.TabIndex = 3;
            this.toolTip.SetToolTip(this.textBoxResultFile, "Script has not been executed yet, click on button \"Start update\"");
            this.textBoxResultFile.Click += new System.EventHandler(this.buttonResult_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 29);
            this.label1.TabIndex = 7;
            this.label1.Text = "From";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(109, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 29);
            this.label2.TabIndex = 8;
            this.label2.Text = "to";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(281, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 29);
            this.label3.TabIndex = 9;
            this.label3.Text = "Protocol:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonOptional
            // 
            this.buttonOptional.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOptional.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOptional.Image = global::DiversityWorkbench.Properties.Resources.Delete;
            this.buttonOptional.Location = new System.Drawing.Point(201, 3);
            this.buttonOptional.Name = "buttonOptional";
            this.buttonOptional.Size = new System.Drawing.Size(74, 23);
            this.buttonOptional.TabIndex = 10;
            this.buttonOptional.Text = "Optional";
            this.buttonOptional.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonOptional.UseVisualStyleBackColor = true;
            this.buttonOptional.Click += new System.EventHandler(this.buttonOptional_Click);
            // 
            // UserControlUpdateResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanelUpdateResult);
            this.Name = "UserControlUpdateResult";
            this.Size = new System.Drawing.Size(540, 29);
            this.tableLayoutPanelUpdateResult.ResumeLayout(false);
            this.tableLayoutPanelUpdateResult.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox textBoxVersionOld;
        public System.Windows.Forms.TextBox textBoxVersionNew;
        private System.Windows.Forms.Button buttonResult;
        private System.Windows.Forms.TextBox textBoxResultFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelUpdateResult;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Button buttonOptional;
    }
}
