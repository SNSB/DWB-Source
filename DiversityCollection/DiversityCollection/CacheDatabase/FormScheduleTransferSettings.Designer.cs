namespace DiversityCollection.CacheDatabase
{
    partial class FormScheduleTransferSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScheduleTransferSettings));
            tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            dateTimePickerTimerTime = new System.Windows.Forms.DateTimePicker();
            checkBoxSaturday = new System.Windows.Forms.CheckBox();
            checkBoxFriday = new System.Windows.Forms.CheckBox();
            checkBoxThursday = new System.Windows.Forms.CheckBox();
            checkBoxWednesday = new System.Windows.Forms.CheckBox();
            checkBoxTuesday = new System.Windows.Forms.CheckBox();
            checkBoxMonday = new System.Windows.Forms.CheckBox();
            checkBoxSunday = new System.Windows.Forms.CheckBox();
            labelTime = new System.Windows.Forms.Label();
            labelProtocol = new System.Windows.Forms.Label();
            labelHeader = new System.Windows.Forms.Label();
            userControlDialogPanel = new DiversityWorkbench.UserControls.UserControlDialogPanel();
            tableLayoutPanelProtocol = new System.Windows.Forms.TableLayoutPanel();
            textBoxProtocol = new System.Windows.Forms.TextBox();
            labelTransferErrors = new System.Windows.Forms.Label();
            buttonTransferErrorsClear = new System.Windows.Forms.Button();
            textBoxTransferErros = new System.Windows.Forms.TextBox();
            buttonFeedback = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            helpProvider = new System.Windows.Forms.HelpProvider();
            tableLayoutPanel.SuspendLayout();
            tableLayoutPanelProtocol.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            tableLayoutPanel.ColumnCount = 4;
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel.Controls.Add(dateTimePickerTimerTime, 1, 2);
            tableLayoutPanel.Controls.Add(checkBoxSaturday, 0, 8);
            tableLayoutPanel.Controls.Add(checkBoxFriday, 0, 7);
            tableLayoutPanel.Controls.Add(checkBoxThursday, 0, 6);
            tableLayoutPanel.Controls.Add(checkBoxWednesday, 0, 5);
            tableLayoutPanel.Controls.Add(checkBoxTuesday, 0, 4);
            tableLayoutPanel.Controls.Add(checkBoxMonday, 0, 3);
            tableLayoutPanel.Controls.Add(checkBoxSunday, 0, 9);
            tableLayoutPanel.Controls.Add(labelTime, 0, 2);
            tableLayoutPanel.Controls.Add(labelProtocol, 2, 2);
            tableLayoutPanel.Controls.Add(labelHeader, 0, 1);
            tableLayoutPanel.Controls.Add(userControlDialogPanel, 0, 10);
            tableLayoutPanel.Controls.Add(tableLayoutPanelProtocol, 2, 3);
            tableLayoutPanel.Controls.Add(buttonFeedback, 3, 1);
            tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            helpProvider.SetHelpKeyword(tableLayoutPanel, "cachedatabase_transfersettings_dc#scheduled-transfer");
            tableLayoutPanel.Location = new System.Drawing.Point(4, 3);
            tableLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tableLayoutPanel.Name = "tableLayoutPanel";
            tableLayoutPanel.RowCount = 11;
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            helpProvider.SetShowHelp(tableLayoutPanel, true);
            tableLayoutPanel.Size = new System.Drawing.Size(499, 250);
            tableLayoutPanel.TabIndex = 1;
            // 
            // dateTimePickerTimerTime
            // 
            dateTimePickerTimerTime.Dock = System.Windows.Forms.DockStyle.Left;
            dateTimePickerTimerTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            dateTimePickerTimerTime.Location = new System.Drawing.Point(40, 23);
            dateTimePickerTimerTime.Margin = new System.Windows.Forms.Padding(0);
            dateTimePickerTimerTime.Name = "dateTimePickerTimerTime";
            dateTimePickerTimerTime.ShowUpDown = true;
            dateTimePickerTimerTime.Size = new System.Drawing.Size(74, 23);
            dateTimePickerTimerTime.TabIndex = 15;
            // 
            // checkBoxSaturday
            // 
            checkBoxSaturday.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxSaturday, 2);
            checkBoxSaturday.Location = new System.Drawing.Point(4, 163);
            checkBoxSaturday.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            checkBoxSaturday.Name = "checkBoxSaturday";
            checkBoxSaturday.Size = new System.Drawing.Size(72, 19);
            checkBoxSaturday.TabIndex = 16;
            checkBoxSaturday.Text = "Saturday";
            checkBoxSaturday.UseVisualStyleBackColor = true;
            // 
            // checkBoxFriday
            // 
            checkBoxFriday.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxFriday, 2);
            checkBoxFriday.Location = new System.Drawing.Point(4, 141);
            checkBoxFriday.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            checkBoxFriday.Name = "checkBoxFriday";
            checkBoxFriday.Size = new System.Drawing.Size(58, 19);
            checkBoxFriday.TabIndex = 17;
            checkBoxFriday.Text = "Friday";
            checkBoxFriday.UseVisualStyleBackColor = true;
            // 
            // checkBoxThursday
            // 
            checkBoxThursday.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxThursday, 2);
            checkBoxThursday.Location = new System.Drawing.Point(4, 119);
            checkBoxThursday.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            checkBoxThursday.Name = "checkBoxThursday";
            checkBoxThursday.Size = new System.Drawing.Size(74, 19);
            checkBoxThursday.TabIndex = 18;
            checkBoxThursday.Text = "Thursday";
            checkBoxThursday.UseVisualStyleBackColor = true;
            // 
            // checkBoxWednesday
            // 
            checkBoxWednesday.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxWednesday, 2);
            checkBoxWednesday.Location = new System.Drawing.Point(4, 97);
            checkBoxWednesday.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            checkBoxWednesday.Name = "checkBoxWednesday";
            checkBoxWednesday.Size = new System.Drawing.Size(87, 19);
            checkBoxWednesday.TabIndex = 19;
            checkBoxWednesday.Text = "Wednesday";
            checkBoxWednesday.UseVisualStyleBackColor = true;
            // 
            // checkBoxTuesday
            // 
            checkBoxTuesday.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxTuesday, 2);
            checkBoxTuesday.Location = new System.Drawing.Point(4, 75);
            checkBoxTuesday.Margin = new System.Windows.Forms.Padding(4, 3, 4, 0);
            checkBoxTuesday.Name = "checkBoxTuesday";
            checkBoxTuesday.Size = new System.Drawing.Size(69, 19);
            checkBoxTuesday.TabIndex = 20;
            checkBoxTuesday.Text = "Tuesday";
            checkBoxTuesday.UseVisualStyleBackColor = true;
            // 
            // checkBoxMonday
            // 
            checkBoxMonday.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxMonday, 2);
            checkBoxMonday.Location = new System.Drawing.Point(4, 53);
            checkBoxMonday.Margin = new System.Windows.Forms.Padding(4, 7, 4, 0);
            checkBoxMonday.Name = "checkBoxMonday";
            checkBoxMonday.Size = new System.Drawing.Size(70, 19);
            checkBoxMonday.TabIndex = 21;
            checkBoxMonday.Text = "Monday";
            checkBoxMonday.UseVisualStyleBackColor = true;
            // 
            // checkBoxSunday
            // 
            checkBoxSunday.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(checkBoxSunday, 2);
            checkBoxSunday.Location = new System.Drawing.Point(4, 185);
            checkBoxSunday.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkBoxSunday.Name = "checkBoxSunday";
            checkBoxSunday.Size = new System.Drawing.Size(65, 19);
            checkBoxSunday.TabIndex = 22;
            checkBoxSunday.Text = "Sunday";
            checkBoxSunday.UseVisualStyleBackColor = true;
            // 
            // labelTime
            // 
            labelTime.AutoSize = true;
            labelTime.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTime.Location = new System.Drawing.Point(4, 23);
            labelTime.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            labelTime.Name = "labelTime";
            labelTime.Size = new System.Drawing.Size(36, 23);
            labelTime.TabIndex = 23;
            labelTime.Text = "Time:";
            labelTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelProtocol
            // 
            labelProtocol.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(labelProtocol, 2);
            labelProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            labelProtocol.Location = new System.Drawing.Point(118, 23);
            labelProtocol.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelProtocol.Name = "labelProtocol";
            labelProtocol.Size = new System.Drawing.Size(377, 23);
            labelProtocol.TabIndex = 25;
            labelProtocol.Text = "Protocol of last transfer";
            labelProtocol.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelHeader
            // 
            labelHeader.AutoSize = true;
            tableLayoutPanel.SetColumnSpan(labelHeader, 3);
            labelHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            labelHeader.Location = new System.Drawing.Point(4, 0);
            labelHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelHeader.Name = "labelHeader";
            labelHeader.Size = new System.Drawing.Size(468, 23);
            labelHeader.TabIndex = 26;
            labelHeader.Text = "Please set the time and day(s) when the transfer should be executed";
            labelHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // userControlDialogPanel
            // 
            tableLayoutPanel.SetColumnSpan(userControlDialogPanel, 4);
            userControlDialogPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            userControlDialogPanel.Location = new System.Drawing.Point(5, 239);
            userControlDialogPanel.Margin = new System.Windows.Forms.Padding(5, 3, 5, 3);
            userControlDialogPanel.Name = "userControlDialogPanel";
            userControlDialogPanel.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            userControlDialogPanel.Size = new System.Drawing.Size(489, 8);
            userControlDialogPanel.TabIndex = 27;
            // 
            // tableLayoutPanelProtocol
            // 
            tableLayoutPanelProtocol.ColumnCount = 2;
            tableLayoutPanel.SetColumnSpan(tableLayoutPanelProtocol, 2);
            tableLayoutPanelProtocol.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelProtocol.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            tableLayoutPanelProtocol.Controls.Add(textBoxProtocol, 0, 0);
            tableLayoutPanelProtocol.Controls.Add(labelTransferErrors, 0, 1);
            tableLayoutPanelProtocol.Controls.Add(buttonTransferErrorsClear, 1, 1);
            tableLayoutPanelProtocol.Controls.Add(textBoxTransferErros, 0, 2);
            tableLayoutPanelProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanelProtocol.Location = new System.Drawing.Point(114, 46);
            tableLayoutPanelProtocol.Margin = new System.Windows.Forms.Padding(0);
            tableLayoutPanelProtocol.Name = "tableLayoutPanelProtocol";
            tableLayoutPanelProtocol.RowCount = 3;
            tableLayoutPanel.SetRowSpan(tableLayoutPanelProtocol, 7);
            tableLayoutPanelProtocol.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPanelProtocol.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProtocol.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableLayoutPanelProtocol.Size = new System.Drawing.Size(385, 185);
            tableLayoutPanelProtocol.TabIndex = 32;
            // 
            // textBoxProtocol
            // 
            tableLayoutPanelProtocol.SetColumnSpan(textBoxProtocol, 2);
            textBoxProtocol.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxProtocol.Location = new System.Drawing.Point(10, 3);
            textBoxProtocol.Margin = new System.Windows.Forms.Padding(10, 3, 4, 3);
            textBoxProtocol.Multiline = true;
            textBoxProtocol.Name = "textBoxProtocol";
            textBoxProtocol.ReadOnly = true;
            textBoxProtocol.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxProtocol.Size = new System.Drawing.Size(371, 108);
            textBoxProtocol.TabIndex = 24;
            // 
            // labelTransferErrors
            // 
            labelTransferErrors.AutoSize = true;
            labelTransferErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            labelTransferErrors.Location = new System.Drawing.Point(4, 114);
            labelTransferErrors.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelTransferErrors.Name = "labelTransferErrors";
            labelTransferErrors.Size = new System.Drawing.Size(353, 23);
            labelTransferErrors.TabIndex = 29;
            labelTransferErrors.Text = "Errors that occured during the transfers";
            labelTransferErrors.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            labelTransferErrors.Visible = false;
            // 
            // buttonTransferErrorsClear
            // 
            buttonTransferErrorsClear.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonTransferErrorsClear.FlatAppearance.BorderSize = 0;
            buttonTransferErrorsClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonTransferErrorsClear.Image = Resource.Delete;
            buttonTransferErrorsClear.Location = new System.Drawing.Point(361, 114);
            buttonTransferErrorsClear.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonTransferErrorsClear.Name = "buttonTransferErrorsClear";
            buttonTransferErrorsClear.Size = new System.Drawing.Size(20, 23);
            buttonTransferErrorsClear.TabIndex = 31;
            toolTip.SetToolTip(buttonTransferErrorsClear, "Clear error protocol");
            buttonTransferErrorsClear.UseVisualStyleBackColor = true;
            buttonTransferErrorsClear.Visible = false;
            buttonTransferErrorsClear.Click += buttonTransferErrorsClear_Click;
            // 
            // textBoxTransferErros
            // 
            textBoxTransferErros.BackColor = System.Drawing.Color.Pink;
            tableLayoutPanelProtocol.SetColumnSpan(textBoxTransferErros, 2);
            textBoxTransferErros.Dock = System.Windows.Forms.DockStyle.Fill;
            textBoxTransferErros.ForeColor = System.Drawing.Color.Red;
            textBoxTransferErros.Location = new System.Drawing.Point(10, 140);
            textBoxTransferErros.Margin = new System.Windows.Forms.Padding(10, 3, 4, 3);
            textBoxTransferErros.Multiline = true;
            textBoxTransferErros.Name = "textBoxTransferErros";
            textBoxTransferErros.ReadOnly = true;
            textBoxTransferErros.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textBoxTransferErros.Size = new System.Drawing.Size(371, 42);
            textBoxTransferErros.TabIndex = 30;
            textBoxTransferErros.Visible = false;
            // 
            // buttonFeedback
            // 
            buttonFeedback.Dock = System.Windows.Forms.DockStyle.Fill;
            buttonFeedback.FlatAppearance.BorderSize = 0;
            buttonFeedback.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonFeedback.Image = Resource.Feedback;
            buttonFeedback.Location = new System.Drawing.Point(476, 0);
            buttonFeedback.Margin = new System.Windows.Forms.Padding(0, 0, 4, 0);
            buttonFeedback.Name = "buttonFeedback";
            buttonFeedback.Size = new System.Drawing.Size(19, 23);
            buttonFeedback.TabIndex = 33;
            toolTip.SetToolTip(buttonFeedback, "Send a feedback to the software developer");
            buttonFeedback.UseVisualStyleBackColor = true;
            buttonFeedback.Click += buttonFeedback_Click;
            // 
            // FormScheduleTransferSettings
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(507, 256);
            Controls.Add(tableLayoutPanel);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            helpProvider.SetHelpKeyword(this, "cachedatabase_transfersettings_dc#scheduled-transfer");
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FormScheduleTransferSettings";
            Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            helpProvider.SetShowHelp(this, true);
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = " Schedule transfer settings";
            FormClosing += FormScheduleTransferSettings_FormClosing;
            KeyDown += Form_KeyDown;
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            tableLayoutPanelProtocol.ResumeLayout(false);
            tableLayoutPanelProtocol.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.DateTimePicker dateTimePickerTimerTime;
        private System.Windows.Forms.CheckBox checkBoxSaturday;
        private System.Windows.Forms.CheckBox checkBoxFriday;
        private System.Windows.Forms.CheckBox checkBoxThursday;
        private System.Windows.Forms.CheckBox checkBoxWednesday;
        private System.Windows.Forms.CheckBox checkBoxTuesday;
        private System.Windows.Forms.CheckBox checkBoxMonday;
        private System.Windows.Forms.CheckBox checkBoxSunday;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.TextBox textBoxProtocol;
        private System.Windows.Forms.Label labelProtocol;
        private System.Windows.Forms.Label labelHeader;
        private DiversityWorkbench.UserControls.UserControlDialogPanel userControlDialogPanel;
        private System.Windows.Forms.Label labelTransferErrors;
        private System.Windows.Forms.TextBox textBoxTransferErros;
        private System.Windows.Forms.Button buttonTransferErrorsClear;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelProtocol;
        private System.Windows.Forms.Button buttonFeedback;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}