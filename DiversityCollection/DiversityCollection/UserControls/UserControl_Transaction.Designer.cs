namespace DiversityCollection.UserControls
{
    partial class UserControl_Transaction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_Transaction));
            this.groupBoxTransaction = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelTransaction = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxTransaction = new System.Windows.Forms.PictureBox();
            this.checkBoxIsOnLoan = new System.Windows.Forms.CheckBox();
            this.buttonOpenTransaction = new System.Windows.Forms.Button();
            this.textBoxTransaction = new System.Windows.Forms.TextBox();
            this.labelTransactionAccessionNumber = new System.Windows.Forms.Label();
            this.textBoxTransactionAccessionNumber = new System.Windows.Forms.TextBox();
            this.groupBoxTransaction.SuspendLayout();
            this.tableLayoutPanelTransaction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTransaction)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxTransaction
            // 
            this.groupBoxTransaction.AccessibleName = "Transaction";
            this.groupBoxTransaction.Controls.Add(this.tableLayoutPanelTransaction);
            this.groupBoxTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxTransaction.ForeColor = System.Drawing.Color.Brown;
            this.groupBoxTransaction.Location = new System.Drawing.Point(0, 0);
            this.groupBoxTransaction.MinimumSize = new System.Drawing.Size(0, 47);
            this.groupBoxTransaction.Name = "groupBoxTransaction";
            this.groupBoxTransaction.Size = new System.Drawing.Size(509, 235);
            this.groupBoxTransaction.TabIndex = 1;
            this.groupBoxTransaction.TabStop = false;
            this.groupBoxTransaction.Text = "Transaction";
            // 
            // tableLayoutPanelTransaction
            // 
            this.tableLayoutPanelTransaction.ColumnCount = 5;
            this.tableLayoutPanelTransaction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTransaction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTransaction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTransaction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTransaction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTransaction.Controls.Add(this.pictureBoxTransaction, 0, 0);
            this.tableLayoutPanelTransaction.Controls.Add(this.checkBoxIsOnLoan, 3, 2);
            this.tableLayoutPanelTransaction.Controls.Add(this.buttonOpenTransaction, 4, 0);
            this.tableLayoutPanelTransaction.Controls.Add(this.textBoxTransaction, 1, 0);
            this.tableLayoutPanelTransaction.Controls.Add(this.labelTransactionAccessionNumber, 0, 1);
            this.tableLayoutPanelTransaction.Controls.Add(this.textBoxTransactionAccessionNumber, 2, 1);
            this.tableLayoutPanelTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTransaction.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanelTransaction.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanelTransaction.Name = "tableLayoutPanelTransaction";
            this.tableLayoutPanelTransaction.RowCount = 3;
            this.tableLayoutPanelTransaction.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTransaction.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTransaction.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTransaction.Size = new System.Drawing.Size(503, 216);
            this.tableLayoutPanelTransaction.TabIndex = 0;
            // 
            // pictureBoxTransaction
            // 
            this.pictureBoxTransaction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTransaction.Image = global::DiversityCollection.Resource.Transaction;
            this.pictureBoxTransaction.Location = new System.Drawing.Point(3, 9);
            this.pictureBoxTransaction.Margin = new System.Windows.Forms.Padding(3, 9, 0, 0);
            this.pictureBoxTransaction.Name = "pictureBoxTransaction";
            this.pictureBoxTransaction.Size = new System.Drawing.Size(17, 20);
            this.pictureBoxTransaction.TabIndex = 1;
            this.pictureBoxTransaction.TabStop = false;
            // 
            // checkBoxIsOnLoan
            // 
            this.checkBoxIsOnLoan.AutoSize = true;
            this.tableLayoutPanelTransaction.SetColumnSpan(this.checkBoxIsOnLoan, 2);
            this.checkBoxIsOnLoan.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxIsOnLoan.Location = new System.Drawing.Point(428, 196);
            this.checkBoxIsOnLoan.Name = "checkBoxIsOnLoan";
            this.checkBoxIsOnLoan.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxIsOnLoan.Size = new System.Drawing.Size(72, 17);
            this.checkBoxIsOnLoan.TabIndex = 2;
            this.checkBoxIsOnLoan.Text = "Is on loan";
            this.checkBoxIsOnLoan.ThreeState = true;
            this.checkBoxIsOnLoan.UseVisualStyleBackColor = true;
            this.checkBoxIsOnLoan.Visible = false;
            // 
            // buttonOpenTransaction
            // 
            this.buttonOpenTransaction.Dock = System.Windows.Forms.DockStyle.Top;
            this.buttonOpenTransaction.Image = global::DiversityCollection.Resource.Transaction;
            this.buttonOpenTransaction.Location = new System.Drawing.Point(476, 3);
            this.buttonOpenTransaction.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.buttonOpenTransaction.Name = "buttonOpenTransaction";
            this.buttonOpenTransaction.Size = new System.Drawing.Size(24, 24);
            this.buttonOpenTransaction.TabIndex = 3;
            this.buttonOpenTransaction.UseVisualStyleBackColor = true;
            this.buttonOpenTransaction.Click += new System.EventHandler(this.buttonOpenTransaction_Click);
            // 
            // textBoxTransaction
            // 
            this.tableLayoutPanelTransaction.SetColumnSpan(this.textBoxTransaction, 3);
            this.textBoxTransaction.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxTransaction.ForeColor = System.Drawing.Color.Brown;
            this.textBoxTransaction.Location = new System.Drawing.Point(23, 6);
            this.textBoxTransaction.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.textBoxTransaction.Name = "textBoxTransaction";
            this.textBoxTransaction.ReadOnly = true;
            this.textBoxTransaction.Size = new System.Drawing.Size(447, 20);
            this.textBoxTransaction.TabIndex = 4;
            // 
            // labelTransactionAccessionNumber
            // 
            this.labelTransactionAccessionNumber.AccessibleName = "CollectionSpecimenTransaction.AccessionNumber";
            this.labelTransactionAccessionNumber.AutoSize = true;
            this.tableLayoutPanelTransaction.SetColumnSpan(this.labelTransactionAccessionNumber, 2);
            this.labelTransactionAccessionNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTransactionAccessionNumber.Location = new System.Drawing.Point(3, 32);
            this.labelTransactionAccessionNumber.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.labelTransactionAccessionNumber.Name = "labelTransactionAccessionNumber";
            this.labelTransactionAccessionNumber.Size = new System.Drawing.Size(46, 13);
            this.labelTransactionAccessionNumber.TabIndex = 5;
            this.labelTransactionAccessionNumber.Text = "Acc.Nr.:";
            // 
            // textBoxTransactionAccessionNumber
            // 
            this.tableLayoutPanelTransaction.SetColumnSpan(this.textBoxTransactionAccessionNumber, 3);
            this.textBoxTransactionAccessionNumber.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxTransactionAccessionNumber.Location = new System.Drawing.Point(55, 29);
            this.textBoxTransactionAccessionNumber.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.textBoxTransactionAccessionNumber.Name = "textBoxTransactionAccessionNumber";
            this.textBoxTransactionAccessionNumber.Size = new System.Drawing.Size(445, 20);
            this.textBoxTransactionAccessionNumber.TabIndex = 6;
            // 
            // UserControl_Transaction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTransaction);
            this.Name = "UserControl_Transaction";
            this.Size = new System.Drawing.Size(509, 235);
            this.groupBoxTransaction.ResumeLayout(false);
            this.tableLayoutPanelTransaction.ResumeLayout(false);
            this.tableLayoutPanelTransaction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTransaction)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxTransaction;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTransaction;
        private System.Windows.Forms.PictureBox pictureBoxTransaction;
        private System.Windows.Forms.CheckBox checkBoxIsOnLoan;
        private System.Windows.Forms.Button buttonOpenTransaction;
        private System.Windows.Forms.TextBox textBoxTransaction;
        private System.Windows.Forms.Label labelTransactionAccessionNumber;
        private System.Windows.Forms.TextBox textBoxTransactionAccessionNumber;
    }
}
