namespace DiversityWorkbench.UserControls
{
    partial class UserControlToolTipTime
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
            this.groupBoxToolTip = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelToolTip = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownToolTipTime = new System.Windows.Forms.NumericUpDown();
            this.labelToolTipTime = new System.Windows.Forms.Label();
            this.labelToolTipTimeExplained = new System.Windows.Forms.Label();
            this.groupBoxToolTip.SuspendLayout();
            this.tableLayoutPanelToolTip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownToolTipTime)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxToolTip
            // 
            this.groupBoxToolTip.Controls.Add(this.tableLayoutPanelToolTip);
            this.groupBoxToolTip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxToolTip.Location = new System.Drawing.Point(0, 0);
            this.groupBoxToolTip.Name = "groupBoxToolTip";
            this.groupBoxToolTip.Size = new System.Drawing.Size(166, 44);
            this.groupBoxToolTip.TabIndex = 31;
            this.groupBoxToolTip.TabStop = false;
            this.groupBoxToolTip.Text = "Set time for ToolTip";
            // 
            // tableLayoutPanelToolTip
            // 
            this.tableLayoutPanelToolTip.ColumnCount = 3;
            this.tableLayoutPanelToolTip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelToolTip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelToolTip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelToolTip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelToolTip.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelToolTip.Controls.Add(this.numericUpDownToolTipTime, 1, 0);
            this.tableLayoutPanelToolTip.Controls.Add(this.labelToolTipTime, 0, 0);
            this.tableLayoutPanelToolTip.Controls.Add(this.labelToolTipTimeExplained, 2, 0);
            this.tableLayoutPanelToolTip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelToolTip.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelToolTip.Name = "tableLayoutPanelToolTip";
            this.tableLayoutPanelToolTip.RowCount = 1;
            this.tableLayoutPanelToolTip.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelToolTip.Size = new System.Drawing.Size(160, 25);
            this.tableLayoutPanelToolTip.TabIndex = 2;
            // 
            // numericUpDownToolTipTime
            // 
            this.numericUpDownToolTipTime.Location = new System.Drawing.Point(36, 3);
            this.numericUpDownToolTipTime.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.numericUpDownToolTipTime.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numericUpDownToolTipTime.Name = "numericUpDownToolTipTime";
            this.numericUpDownToolTipTime.Size = new System.Drawing.Size(30, 20);
            this.numericUpDownToolTipTime.TabIndex = 2;
            this.numericUpDownToolTipTime.Click += new System.EventHandler(this.numericUpDownToolTipTime_Click);
            // 
            // labelToolTipTime
            // 
            this.labelToolTipTime.AutoSize = true;
            this.labelToolTipTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelToolTipTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelToolTipTime.Location = new System.Drawing.Point(3, 6);
            this.labelToolTipTime.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelToolTipTime.Name = "labelToolTipTime";
            this.labelToolTipTime.Size = new System.Drawing.Size(33, 19);
            this.labelToolTipTime.TabIndex = 3;
            this.labelToolTipTime.Text = "Time:";
            this.labelToolTipTime.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelToolTipTimeExplained
            // 
            this.labelToolTipTimeExplained.AutoSize = true;
            this.labelToolTipTimeExplained.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelToolTipTimeExplained.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelToolTipTimeExplained.Location = new System.Drawing.Point(72, 6);
            this.labelToolTipTimeExplained.Margin = new System.Windows.Forms.Padding(6, 6, 3, 0);
            this.labelToolTipTimeExplained.Name = "labelToolTipTimeExplained";
            this.labelToolTipTimeExplained.Size = new System.Drawing.Size(85, 19);
            this.labelToolTipTimeExplained.TabIndex = 4;
            this.labelToolTipTimeExplained.Text = "s";
            // 
            // UserControlToolTipTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxToolTip);
            this.Name = "UserControlToolTipTime";
            this.Size = new System.Drawing.Size(166, 44);
            this.groupBoxToolTip.ResumeLayout(false);
            this.tableLayoutPanelToolTip.ResumeLayout(false);
            this.tableLayoutPanelToolTip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownToolTipTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxToolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelToolTip;
        private System.Windows.Forms.NumericUpDown numericUpDownToolTipTime;
        private System.Windows.Forms.Label labelToolTipTime;
        private System.Windows.Forms.Label labelToolTipTimeExplained;
    }
}
