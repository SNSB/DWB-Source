namespace DiversityAgents.Forms
{
    partial class FormNewAgent
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNewAgent));
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            labelGivenName = new System.Windows.Forms.Label();
            textBoxGivenName = new System.Windows.Forms.TextBox();
            labelInheritedName = new System.Windows.Forms.Label();
            textBoxInheritedName = new System.Windows.Forms.TextBox();
            labelType = new System.Windows.Forms.Label();
            comboBoxType = new System.Windows.Forms.ComboBox();
            errorProvider1 = new System.Windows.Forms.ErrorProvider(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // userControlDialogPanel
            // 
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(0, 56);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(607, 31);
            userControlDialogPanel.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 3;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.Controls.Add(labelGivenName, 1, 0);
            tableLayoutPanel.Controls.Add(textBoxGivenName, 1, 1);
            tableLayoutPanel.Controls.Add(labelInheritedName, 2, 0);
            tableLayoutPanel.Controls.Add(textBoxInheritedName, 2, 1);
            tableLayoutPanel.Controls.Add(labelType, 0, 0);
            tableLayoutPanel.Controls.Add(comboBoxType, 0, 1);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 2;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.Size = new System.Drawing.Size(607, 57);
            tableLayoutPanel.TabIndex = 1;
            // 
            // labelGivenName
            // 
            labelGivenName.AutoSize = true;
            labelGivenName.Dock = System.Windows.Forms.DockStyle.Fill;
            labelGivenName.Location = new System.Drawing.Point(152, 7);
            labelGivenName.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            labelGivenName.Name = "labelGivenName";
            labelGivenName.Size = new System.Drawing.Size(209, 15);
            labelGivenName.TabIndex = 0;
            labelGivenName.Text = "First name:";
            labelGivenName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxGivenName
            // 
            textBoxGivenName.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxGivenName.Location = new System.Drawing.Point(152, 25);
            textBoxGivenName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxGivenName.Name = "textBoxGivenName";
            textBoxGivenName.Size = new System.Drawing.Size(209, 23);
            textBoxGivenName.TabIndex = 1;
            textBoxGivenName.TextChanged += textBoxGivenName_TextChanged;
            // 
            // labelInheritedName
            // 
            labelInheritedName.AutoSize = true;
            labelInheritedName.Dock = System.Windows.Forms.DockStyle.Fill;
            labelInheritedName.Location = new System.Drawing.Point(369, 0);
            labelInheritedName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelInheritedName.Name = "labelInheritedName";
            labelInheritedName.Size = new System.Drawing.Size(234, 22);
            labelInheritedName.TabIndex = 2;
            labelInheritedName.Text = "Last name:";
            labelInheritedName.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // textBoxInheritedName
            // 
            textBoxInheritedName.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxInheritedName.Location = new System.Drawing.Point(369, 25);
            textBoxInheritedName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textBoxInheritedName.Name = "textBoxInheritedName";
            textBoxInheritedName.Size = new System.Drawing.Size(234, 23);
            textBoxInheritedName.TabIndex = 3;
            textBoxInheritedName.TextChanged += textBoxInheritedName_TextChanged;
            // 
            // labelType
            // 
            labelType.AutoSize = true;
            labelType.Dock = System.Windows.Forms.DockStyle.Fill;
            labelType.Location = new System.Drawing.Point(4, 0);
            labelType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelType.Name = "labelType";
            labelType.Size = new System.Drawing.Size(140, 22);
            labelType.TabIndex = 4;
            labelType.Text = "Type:";
            labelType.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // comboBoxType
            // 
            comboBoxType.Dock = System.Windows.Forms.DockStyle.Fill;
            comboBoxType.FormattingEnabled = true;
            comboBoxType.Location = new System.Drawing.Point(4, 25);
            comboBoxType.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            comboBoxType.Name = "comboBoxType";
            comboBoxType.Size = new System.Drawing.Size(140, 23);
            comboBoxType.TabIndex = 5;
            comboBoxType.SelectedIndexChanged += comboBoxType_SelectedIndexChanged;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // FormNewAgent
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(607, 87);
            Controls.Add(tableLayoutPanel);
            Controls.Add(userControlDialogPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            helpProvider.SetHelpKeyword(this, "agent_da");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormNewAgent";
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Create a  new agent";
            HelpRequested += FormNewAgent_HelpRequested;
            KeyDown += Form_KeyDown;
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Label labelGivenName;
        private System.Windows.Forms.TextBox textBoxGivenName;
        private System.Windows.Forms.Label labelInheritedName;
        private System.Windows.Forms.TextBox textBoxInheritedName;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.ComboBox comboBoxType;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}