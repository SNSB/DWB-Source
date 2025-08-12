namespace DiversityWorkbench.UserControls
{
    partial class UserControlHtmlColors
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
            this.buttonText = new System.Windows.Forms.Button();
            this.buttonBackground = new System.Windows.Forms.Button();
            this.buttonLink = new System.Windows.Forms.Button();
            this.buttonVLink = new System.Windows.Forms.Button();
            this.buttonALink = new System.Windows.Forms.Button();
            this.buttonTitle = new System.Windows.Forms.Button();
            this.colorDialog = new System.Windows.Forms.ColorDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonText
            // 
            this.buttonText.BackColor = System.Drawing.Color.White;
            this.buttonText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonText.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonText.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonText.Location = new System.Drawing.Point(164, 0);
            this.buttonText.Margin = new System.Windows.Forms.Padding(0);
            this.buttonText.Name = "buttonText";
            this.buttonText.Size = new System.Drawing.Size(84, 24);
            this.buttonText.TabIndex = 3;
            this.buttonText.Text = "Text";
            this.toolTip.SetToolTip(this.buttonText, "Click to select text color");
            this.buttonText.UseVisualStyleBackColor = false;
            this.buttonText.Click += new System.EventHandler(this.buttonText_Click);
            // 
            // buttonBackground
            // 
            this.buttonBackground.BackColor = System.Drawing.Color.White;
            this.buttonBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonBackground.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonBackground.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBackground.Location = new System.Drawing.Point(82, 0);
            this.buttonBackground.Margin = new System.Windows.Forms.Padding(0);
            this.buttonBackground.Name = "buttonBackground";
            this.buttonBackground.Size = new System.Drawing.Size(82, 24);
            this.buttonBackground.TabIndex = 2;
            this.buttonBackground.Text = "Background";
            this.toolTip.SetToolTip(this.buttonBackground, "Click to select background color");
            this.buttonBackground.UseVisualStyleBackColor = false;
            this.buttonBackground.Click += new System.EventHandler(this.buttonBackground_Click);
            // 
            // buttonLink
            // 
            this.buttonLink.BackColor = System.Drawing.Color.White;
            this.buttonLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLink.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLink.ForeColor = System.Drawing.Color.Blue;
            this.buttonLink.Location = new System.Drawing.Point(0, 24);
            this.buttonLink.Margin = new System.Windows.Forms.Padding(0);
            this.buttonLink.Name = "buttonLink";
            this.buttonLink.Size = new System.Drawing.Size(82, 24);
            this.buttonLink.TabIndex = 4;
            this.buttonLink.Text = "Link";
            this.toolTip.SetToolTip(this.buttonLink, "Click to select the color of links");
            this.buttonLink.UseVisualStyleBackColor = false;
            this.buttonLink.Click += new System.EventHandler(this.buttonLink_Click);
            // 
            // buttonVLink
            // 
            this.buttonVLink.BackColor = System.Drawing.Color.White;
            this.buttonVLink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonVLink.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonVLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonVLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonVLink.ForeColor = System.Drawing.Color.Purple;
            this.buttonVLink.Location = new System.Drawing.Point(82, 24);
            this.buttonVLink.Margin = new System.Windows.Forms.Padding(0);
            this.buttonVLink.Name = "buttonVLink";
            this.buttonVLink.Size = new System.Drawing.Size(82, 24);
            this.buttonVLink.TabIndex = 5;
            this.buttonVLink.Text = "Visited link";
            this.toolTip.SetToolTip(this.buttonVLink, "Click to select the color of visited links");
            this.buttonVLink.UseVisualStyleBackColor = false;
            this.buttonVLink.Click += new System.EventHandler(this.buttonVLink_Click);
            // 
            // buttonALink
            // 
            this.buttonALink.BackColor = System.Drawing.Color.White;
            this.buttonALink.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonALink.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonALink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonALink.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonALink.ForeColor = System.Drawing.Color.Fuchsia;
            this.buttonALink.Location = new System.Drawing.Point(164, 24);
            this.buttonALink.Margin = new System.Windows.Forms.Padding(0);
            this.buttonALink.Name = "buttonALink";
            this.buttonALink.Size = new System.Drawing.Size(84, 24);
            this.buttonALink.TabIndex = 6;
            this.buttonALink.Text = "Active link";
            this.toolTip.SetToolTip(this.buttonALink, "Click to select the color of active links");
            this.buttonALink.UseVisualStyleBackColor = false;
            this.buttonALink.Click += new System.EventHandler(this.buttonALink_Click);
            // 
            // buttonTitle
            // 
            this.buttonTitle.BackColor = System.Drawing.Color.White;
            this.buttonTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTitle.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.buttonTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTitle.ForeColor = System.Drawing.Color.Navy;
            this.buttonTitle.Location = new System.Drawing.Point(0, 0);
            this.buttonTitle.Margin = new System.Windows.Forms.Padding(0);
            this.buttonTitle.Name = "buttonTitle";
            this.buttonTitle.Size = new System.Drawing.Size(82, 24);
            this.buttonTitle.TabIndex = 1;
            this.buttonTitle.Text = "Title";
            this.toolTip.SetToolTip(this.buttonTitle, "Click to select title color");
            this.buttonTitle.UseVisualStyleBackColor = false;
            this.buttonTitle.Click += new System.EventHandler(this.buttonTitle_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel1.Controls.Add(this.buttonText, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonTitle, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonLink, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonALink, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonBackground, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.buttonVLink, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(248, 48);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // UserControlHtmlColors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserControlHtmlColors";
            this.Size = new System.Drawing.Size(248, 48);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonText;
        private System.Windows.Forms.Button buttonBackground;
        private System.Windows.Forms.Button buttonLink;
        private System.Windows.Forms.Button buttonVLink;
        private System.Windows.Forms.Button buttonALink;
        private System.Windows.Forms.Button buttonTitle;
        private System.Windows.Forms.ColorDialog colorDialog;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

    }
}
