using System.Drawing;

namespace DiversityWorkbench.UserControls
{
    partial class UserControlWebservice
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
            components = new System.ComponentModel.Container();
            splitContainerMain = new System.Windows.Forms.SplitContainer();
            groupBoxQueryResults = new System.Windows.Forms.GroupBox();
            lblSearching = new System.Windows.Forms.Label();
            listBoxQueryResult = new System.Windows.Forms.ListBox();
            tableLayoutPanelQueryButtons = new System.Windows.Forms.TableLayoutPanel();
            buttonQuery = new System.Windows.Forms.Button();
            buttonSetQueryConditionsUpDown = new System.Windows.Forms.Button();
            labelMaxResults = new System.Windows.Forms.Label();
            maskedTextBoxMaxResults = new System.Windows.Forms.MaskedTextBox();
            linkLabelWebservice = new System.Windows.Forms.LinkLabel();
            groupBoxQueryConditions = new System.Windows.Forms.GroupBox();
            tableLayoutPanelQueryCondition1 = new System.Windows.Forms.TableLayoutPanel();
            labelQueryCondition1 = new System.Windows.Forms.Label();
            textBoxQueryCondition1 = new System.Windows.Forms.TextBox();
            toolTipQueryList = new System.Windows.Forms.ToolTip(components);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            groupBoxQueryResults.SuspendLayout();
            tableLayoutPanelQueryButtons.SuspendLayout();
            groupBoxQueryConditions.SuspendLayout();
            tableLayoutPanelQueryCondition1.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainerMain.Location = new System.Drawing.Point(0, 0);
            splitContainerMain.Margin = new System.Windows.Forms.Padding(4);
            splitContainerMain.Name = "splitContainerMain";
            splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(groupBoxQueryResults);
            splitContainerMain.Panel1.Controls.Add(linkLabelWebservice);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(groupBoxQueryConditions);
            splitContainerMain.Size = new System.Drawing.Size(423, 502);
            splitContainerMain.SplitterDistance = 456;
            splitContainerMain.TabIndex = 0;
            // 
            // groupBoxQueryResults
            // 
            groupBoxQueryResults.Controls.Add(lblSearching);
            groupBoxQueryResults.Controls.Add(listBoxQueryResult);
            groupBoxQueryResults.Controls.Add(tableLayoutPanelQueryButtons);
            groupBoxQueryResults.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxQueryResults.Location = new System.Drawing.Point(0, 26);
            groupBoxQueryResults.Margin = new System.Windows.Forms.Padding(4);
            groupBoxQueryResults.Name = "groupBoxQueryResults";
            groupBoxQueryResults.Padding = new System.Windows.Forms.Padding(4);
            groupBoxQueryResults.RightToLeft = System.Windows.Forms.RightToLeft.No;
            groupBoxQueryResults.Size = new System.Drawing.Size(423, 430);
            groupBoxQueryResults.TabIndex = 2;
            groupBoxQueryResults.TabStop = false;
            groupBoxQueryResults.Text = "Query results";
            // 
            // lblSearching
            // 
            lblSearching.BackColor = System.Drawing.Color.PeachPuff;
            lblSearching.ForeColor = System.Drawing.Color.DarkSlateGray;
            lblSearching.Location = new System.Drawing.Point(327, 100);
            lblSearching.Name = "lblSearching";
            lblSearching.TextAlign = ContentAlignment.MiddleCenter;
            lblSearching.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            lblSearching.Size = new System.Drawing.Size(150, 40);
            lblSearching.TabIndex = 4;
            lblSearching.Text = "Loading";
            lblSearching.Visible = false;
            // 
            // listBoxQueryResult
            // 
            listBoxQueryResult.BackColor = System.Drawing.Color.WhiteSmoke;
            listBoxQueryResult.Dock = System.Windows.Forms.DockStyle.Fill;
            listBoxQueryResult.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            listBoxQueryResult.FormattingEnabled = true;
            listBoxQueryResult.IntegralHeight = false;
            listBoxQueryResult.ItemHeight = 14;
            listBoxQueryResult.Location = new System.Drawing.Point(4, 20);
            listBoxQueryResult.Margin = new System.Windows.Forms.Padding(4);
            listBoxQueryResult.Name = "listBoxQueryResult";
            listBoxQueryResult.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            listBoxQueryResult.Size = new System.Drawing.Size(415, 376);
            listBoxQueryResult.TabIndex = 2;
            // 
            // tableLayoutPanelQueryButtons
            // 
            tableLayoutPanelQueryButtons.ColumnCount = 8;
            tableLayoutPanelQueryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQueryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQueryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQueryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQueryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelQueryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQueryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQueryButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQueryButtons.Controls.Add(buttonQuery, 7, 1);
            tableLayoutPanelQueryButtons.Controls.Add(buttonSetQueryConditionsUpDown, 0, 1);
            tableLayoutPanelQueryButtons.Controls.Add(labelMaxResults, 1, 1);
            tableLayoutPanelQueryButtons.Controls.Add(maskedTextBoxMaxResults, 4, 1);
            tableLayoutPanelQueryButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            tableLayoutPanelQueryButtons.Location = new System.Drawing.Point(4, 396);
            tableLayoutPanelQueryButtons.Margin = new System.Windows.Forms.Padding(4);
            tableLayoutPanelQueryButtons.Name = "tableLayoutPanelQueryButtons";
            tableLayoutPanelQueryButtons.RowCount = 2;
            tableLayoutPanelQueryButtons.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelQueryButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelQueryButtons.Size = new System.Drawing.Size(415, 30);
            tableLayoutPanelQueryButtons.TabIndex = 3;
            // 
            // buttonQuery
            // 
            buttonQuery.Dock = System.Windows.Forms.DockStyle.Right;
            buttonQuery.Image = ResourceWorkbench.Query;
            buttonQuery.Location = new System.Drawing.Point(383, 4);
            buttonQuery.Margin = new System.Windows.Forms.Padding(4, 4, 0, 0);
            buttonQuery.Name = "buttonQuery";
            buttonQuery.Size = new System.Drawing.Size(32, 26);
            buttonQuery.TabIndex = 0;
            buttonQuery.UseVisualStyleBackColor = true;
            buttonQuery.Click += buttonQuery_Click;
            // 
            // buttonSetQueryConditionsUpDown
            // 
            buttonSetQueryConditionsUpDown.Dock = System.Windows.Forms.DockStyle.Left;
            buttonSetQueryConditionsUpDown.Image = ResourceWorkbench.ArrowDown;
            buttonSetQueryConditionsUpDown.Location = new System.Drawing.Point(0, 4);
            buttonSetQueryConditionsUpDown.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);
            buttonSetQueryConditionsUpDown.Name = "buttonSetQueryConditionsUpDown";
            buttonSetQueryConditionsUpDown.Size = new System.Drawing.Size(22, 26);
            buttonSetQueryConditionsUpDown.TabIndex = 1;
            buttonSetQueryConditionsUpDown.Tag = "True";
            buttonSetQueryConditionsUpDown.UseVisualStyleBackColor = true;
            buttonSetQueryConditionsUpDown.Click += buttonSetQueryConditionsUpDown_Click;
            // 
            // labelMaxResults
            // 
            labelMaxResults.AutoSize = true;
            tableLayoutPanelQueryButtons.SetColumnSpan(labelMaxResults, 2);
            labelMaxResults.Dock = System.Windows.Forms.DockStyle.Fill;
            labelMaxResults.Location = new System.Drawing.Point(30, 0);
            labelMaxResults.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelMaxResults.Name = "labelMaxResults";
            labelMaxResults.Size = new System.Drawing.Size(73, 30);
            labelMaxResults.TabIndex = 4;
            labelMaxResults.Text = "max. results:";
            labelMaxResults.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // maskedTextBoxMaxResults
            // 
            maskedTextBoxMaxResults.Location = new System.Drawing.Point(111, 4);
            maskedTextBoxMaxResults.Margin = new System.Windows.Forms.Padding(4);
            maskedTextBoxMaxResults.Mask = "###";
            maskedTextBoxMaxResults.Name = "maskedTextBoxMaxResults";
            maskedTextBoxMaxResults.Size = new System.Drawing.Size(29, 23);
            maskedTextBoxMaxResults.TabIndex = 5;
            maskedTextBoxMaxResults.Text = "50";
            maskedTextBoxMaxResults.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // linkLabelWebservice
            // 
            linkLabelWebservice.Dock = System.Windows.Forms.DockStyle.Top;
            linkLabelWebservice.Location = new System.Drawing.Point(0, 0);
            linkLabelWebservice.Margin = new System.Windows.Forms.Padding(4);
            linkLabelWebservice.Name = "linkLabelWebservice";
            linkLabelWebservice.Size = new System.Drawing.Size(423, 26);
            linkLabelWebservice.TabIndex = 3;
            linkLabelWebservice.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            linkLabelWebservice.LinkClicked += linkLabelWebservice_LinkClicked;
            // 
            // groupBoxQueryConditions
            // 
            groupBoxQueryConditions.Controls.Add(tableLayoutPanelQueryCondition1);
            groupBoxQueryConditions.Dock = System.Windows.Forms.DockStyle.Fill;
            groupBoxQueryConditions.Location = new System.Drawing.Point(0, 0);
            groupBoxQueryConditions.Margin = new System.Windows.Forms.Padding(4);
            groupBoxQueryConditions.Name = "groupBoxQueryConditions";
            groupBoxQueryConditions.Padding = new System.Windows.Forms.Padding(4, 0, 4, 4);
            groupBoxQueryConditions.RightToLeft = System.Windows.Forms.RightToLeft.No;
            groupBoxQueryConditions.Size = new System.Drawing.Size(423, 42);
            groupBoxQueryConditions.TabIndex = 3;
            groupBoxQueryConditions.TabStop = false;
            groupBoxQueryConditions.Text = "Query conditions";
            // 
            // tableLayoutPanelQueryCondition1
            // 
            tableLayoutPanelQueryCondition1.ColumnCount = 2;
            tableLayoutPanelQueryCondition1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelQueryCondition1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelQueryCondition1.Controls.Add(labelQueryCondition1, 0, 0);
            tableLayoutPanelQueryCondition1.Controls.Add(textBoxQueryCondition1, 1, 0);
            tableLayoutPanelQueryCondition1.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPanelQueryCondition1.Location = new System.Drawing.Point(4, 16);
            tableLayoutPanelQueryCondition1.Margin = new System.Windows.Forms.Padding(4);
            tableLayoutPanelQueryCondition1.Name = "tableLayoutPanelQueryCondition1";
            tableLayoutPanelQueryCondition1.RowCount = 1;
            tableLayoutPanelQueryCondition1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelQueryCondition1.Size = new System.Drawing.Size(415, 30);
            tableLayoutPanelQueryCondition1.TabIndex = 0;
            // 
            // labelQueryCondition1
            // 
            labelQueryCondition1.AutoSize = true;
            labelQueryCondition1.Dock = System.Windows.Forms.DockStyle.Fill;
            labelQueryCondition1.Location = new System.Drawing.Point(4, 7);
            labelQueryCondition1.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            labelQueryCondition1.Name = "labelQueryCondition1";
            labelQueryCondition1.Size = new System.Drawing.Size(73, 23);
            labelQueryCondition1.TabIndex = 0;
            labelQueryCondition1.Text = "Search Input";
            // 
            // textBoxQueryCondition1
            // 
            textBoxQueryCondition1.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxQueryCondition1.Location = new System.Drawing.Point(85, 4);
            textBoxQueryCondition1.Margin = new System.Windows.Forms.Padding(4);
            textBoxQueryCondition1.Name = "textBoxQueryCondition1";
            textBoxQueryCondition1.Size = new System.Drawing.Size(326, 23);
            textBoxQueryCondition1.TabIndex = 1;
            // 
            // UserControlWebservice
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(splitContainerMain);
            Margin = new System.Windows.Forms.Padding(4);
            Name = "UserControlWebservice";
            Size = new System.Drawing.Size(423, 502);
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            groupBoxQueryResults.ResumeLayout(false);
            groupBoxQueryResults.PerformLayout();
            tableLayoutPanelQueryButtons.ResumeLayout(false);
            tableLayoutPanelQueryButtons.PerformLayout();
            groupBoxQueryConditions.ResumeLayout(false);
            tableLayoutPanelQueryCondition1.ResumeLayout(false);
            tableLayoutPanelQueryCondition1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.GroupBox groupBoxQueryResults;
        public System.Windows.Forms.ListBox listBoxQueryResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelQueryButtons;
        private System.Windows.Forms.Button buttonSetQueryConditionsUpDown;
        private System.Windows.Forms.GroupBox groupBoxQueryConditions;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelQueryCondition1;
        private System.Windows.Forms.Label labelQueryCondition1;
        private System.Windows.Forms.TextBox textBoxQueryCondition1;
        private System.Windows.Forms.LinkLabel linkLabelWebservice;
        private System.Windows.Forms.Label labelMaxResults;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxMaxResults;
        private System.Windows.Forms.ToolTip toolTipQueryList;
        public System.Windows.Forms.Button buttonQuery;
        private System.Windows.Forms.Label lblSearching;
    }
}
