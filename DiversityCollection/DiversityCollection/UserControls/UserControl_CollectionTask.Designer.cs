namespace DiversityCollection.UserControls
{
    partial class UserControl_CollectionTask
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl_CollectionTask));
            this.groupBoxCollectionTask = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelTask = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanelTaskHeader = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxType = new System.Windows.Forms.PictureBox();
            this.labelTaskHaeder = new System.Windows.Forms.Label();
            this.numericUpDownDisplayOrder = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanelTaskModule = new System.Windows.Forms.TableLayoutPanel();
            this.userControlModuleRelatedEntryValue = new DiversityWorkbench.UserControls.UserControlModuleRelatedEntry();
            this.pictureBoxTaskModule = new System.Windows.Forms.PictureBox();
            this.labelModuleTitle = new System.Windows.Forms.Label();
            this.tableLayoutPanelTaskModuleList = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxTaskModuleList = new System.Windows.Forms.ComboBox();
            this.pictureBoxTaskModuleList = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelTaskResult = new System.Windows.Forms.TableLayoutPanel();
            this.labelResult = new System.Windows.Forms.Label();
            this.comboBoxResult = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanelTaskNumber = new System.Windows.Forms.TableLayoutPanel();
            this.labelTaskNumber = new System.Windows.Forms.Label();
            this.textBoxTaskNumber = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelTaskBool = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxTaskBool = new System.Windows.Forms.CheckBox();
            this.labelTaskBool = new System.Windows.Forms.Label();
            this.tableLayoutPanelTaskDate = new System.Windows.Forms.TableLayoutPanel();
            this.labelTaskStart = new System.Windows.Forms.Label();
            this.dateTimePickerTaskStart = new System.Windows.Forms.DateTimePicker();
            this.labelTaskEnd = new System.Windows.Forms.Label();
            this.dateTimePickerTaskEnd = new System.Windows.Forms.DateTimePicker();
            this.buttonTaskDateDelete = new System.Windows.Forms.Button();
            this.buttonTaskDateEndDelete = new System.Windows.Forms.Button();
            this.tableLayoutPanelTaskUri = new System.Windows.Forms.TableLayoutPanel();
            this.buttonUrlOpen = new System.Windows.Forms.Button();
            this.textBoxURI = new System.Windows.Forms.TextBox();
            this.labelURI = new System.Windows.Forms.Label();
            this.tableLayoutPanelTaskDescription = new System.Windows.Forms.TableLayoutPanel();
            this.labelDescription = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelTaskNotes = new System.Windows.Forms.TableLayoutPanel();
            this.labelNotes = new System.Windows.Forms.Label();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.groupBoxCollectionTask.SuspendLayout();
            this.flowLayoutPanelTask.SuspendLayout();
            this.tableLayoutPanelTaskHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplayOrder)).BeginInit();
            this.tableLayoutPanelTaskModule.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaskModule)).BeginInit();
            this.tableLayoutPanelTaskModuleList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaskModuleList)).BeginInit();
            this.tableLayoutPanelTaskResult.SuspendLayout();
            this.tableLayoutPanelTaskNumber.SuspendLayout();
            this.tableLayoutPanelTaskBool.SuspendLayout();
            this.tableLayoutPanelTaskDate.SuspendLayout();
            this.tableLayoutPanelTaskUri.SuspendLayout();
            this.tableLayoutPanelTaskDescription.SuspendLayout();
            this.tableLayoutPanelTaskNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListDataWithholding
            // 
            this.imageListDataWithholding.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListDataWithholding.ImageStream")));
            this.imageListDataWithholding.Images.SetKeyName(0, "Stop3.ico");
            this.imageListDataWithholding.Images.SetKeyName(1, "Stop3Grey.ico");
            // 
            // groupBoxCollectionTask
            // 
            this.groupBoxCollectionTask.Controls.Add(this.flowLayoutPanelTask);
            this.groupBoxCollectionTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxCollectionTask.Location = new System.Drawing.Point(0, 0);
            this.groupBoxCollectionTask.Name = "groupBoxCollectionTask";
            this.groupBoxCollectionTask.Size = new System.Drawing.Size(539, 174);
            this.groupBoxCollectionTask.TabIndex = 0;
            this.groupBoxCollectionTask.TabStop = false;
            this.groupBoxCollectionTask.Text = "CollectionTask";
            // 
            // flowLayoutPanelTask
            // 
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskHeader);
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskModule);
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskModuleList);
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskResult);
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskNumber);
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskBool);
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskDate);
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskUri);
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskDescription);
            this.flowLayoutPanelTask.Controls.Add(this.tableLayoutPanelTaskNotes);
            this.flowLayoutPanelTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelTask.Enabled = false;
            this.flowLayoutPanelTask.Location = new System.Drawing.Point(3, 16);
            this.flowLayoutPanelTask.Name = "flowLayoutPanelTask";
            this.flowLayoutPanelTask.Size = new System.Drawing.Size(533, 155);
            this.flowLayoutPanelTask.TabIndex = 2;
            this.flowLayoutPanelTask.SizeChanged += new System.EventHandler(this.flowLayoutPanelTask_SizeChanged);
            // 
            // tableLayoutPanelTaskHeader
            // 
            this.tableLayoutPanelTaskHeader.ColumnCount = 3;
            this.tableLayoutPanelTaskHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskHeader.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskHeader.Controls.Add(this.pictureBoxType, 0, 0);
            this.tableLayoutPanelTaskHeader.Controls.Add(this.labelTaskHaeder, 1, 0);
            this.tableLayoutPanelTaskHeader.Controls.Add(this.numericUpDownDisplayOrder, 2, 0);
            this.tableLayoutPanelTaskHeader.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanelTaskHeader.Name = "tableLayoutPanelTaskHeader";
            this.tableLayoutPanelTaskHeader.RowCount = 1;
            this.tableLayoutPanelTaskHeader.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskHeader.Size = new System.Drawing.Size(200, 25);
            this.tableLayoutPanelTaskHeader.TabIndex = 0;
            // 
            // pictureBoxType
            // 
            this.pictureBoxType.Image = global::DiversityCollection.Resource.NULL;
            this.pictureBoxType.Location = new System.Drawing.Point(0, 4);
            this.pictureBoxType.Margin = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.pictureBoxType.Name = "pictureBoxType";
            this.pictureBoxType.Size = new System.Drawing.Size(20, 21);
            this.pictureBoxType.TabIndex = 13;
            this.pictureBoxType.TabStop = false;
            // 
            // labelTaskHaeder
            // 
            this.labelTaskHaeder.AutoSize = true;
            this.labelTaskHaeder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaskHaeder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTaskHaeder.Location = new System.Drawing.Point(23, 0);
            this.labelTaskHaeder.Name = "labelTaskHaeder";
            this.labelTaskHaeder.Size = new System.Drawing.Size(121, 25);
            this.labelTaskHaeder.TabIndex = 14;
            this.labelTaskHaeder.Text = "label1";
            this.labelTaskHaeder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numericUpDownDisplayOrder
            // 
            this.numericUpDownDisplayOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericUpDownDisplayOrder.Location = new System.Drawing.Point(147, 3);
            this.numericUpDownDisplayOrder.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.numericUpDownDisplayOrder.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numericUpDownDisplayOrder.Name = "numericUpDownDisplayOrder";
            this.numericUpDownDisplayOrder.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownDisplayOrder.TabIndex = 25;
            this.numericUpDownDisplayOrder.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // tableLayoutPanelTaskModule
            // 
            this.tableLayoutPanelTaskModule.ColumnCount = 3;
            this.tableLayoutPanelTaskModule.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskModule.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelTaskModule.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskModule.Controls.Add(this.userControlModuleRelatedEntryValue, 2, 0);
            this.tableLayoutPanelTaskModule.Controls.Add(this.pictureBoxTaskModule, 1, 0);
            this.tableLayoutPanelTaskModule.Controls.Add(this.labelModuleTitle, 0, 0);
            this.tableLayoutPanelTaskModule.Location = new System.Drawing.Point(206, 0);
            this.tableLayoutPanelTaskModule.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTaskModule.Name = "tableLayoutPanelTaskModule";
            this.tableLayoutPanelTaskModule.RowCount = 1;
            this.tableLayoutPanelTaskModule.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskModule.Size = new System.Drawing.Size(200, 25);
            this.tableLayoutPanelTaskModule.TabIndex = 1;
            // 
            // userControlModuleRelatedEntryValue
            // 
            this.userControlModuleRelatedEntryValue.CanDeleteConnectionToModule = true;
            this.userControlModuleRelatedEntryValue.DependsOnUri = "";
            this.userControlModuleRelatedEntryValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlModuleRelatedEntryValue.Domain = "";
            this.userControlModuleRelatedEntryValue.LinkDeleteConnectionToModuleToTableGrant = false;
            this.userControlModuleRelatedEntryValue.Location = new System.Drawing.Point(87, 1);
            this.userControlModuleRelatedEntryValue.Margin = new System.Windows.Forms.Padding(0, 1, 3, 1);
            this.userControlModuleRelatedEntryValue.Module = null;
            this.userControlModuleRelatedEntryValue.Name = "userControlModuleRelatedEntryValue";
            this.userControlModuleRelatedEntryValue.ShowHtmlUnitValues = false;
            this.userControlModuleRelatedEntryValue.ShowInfo = false;
            this.userControlModuleRelatedEntryValue.Size = new System.Drawing.Size(110, 23);
            this.userControlModuleRelatedEntryValue.SupressEmptyRemoteValues = false;
            this.userControlModuleRelatedEntryValue.TabIndex = 18;
            // 
            // pictureBoxTaskModule
            // 
            this.pictureBoxTaskModule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTaskModule.Location = new System.Drawing.Point(66, 3);
            this.pictureBoxTaskModule.Name = "pictureBoxTaskModule";
            this.pictureBoxTaskModule.Size = new System.Drawing.Size(18, 19);
            this.pictureBoxTaskModule.TabIndex = 19;
            this.pictureBoxTaskModule.TabStop = false;
            // 
            // labelModuleTitle
            // 
            this.labelModuleTitle.AutoSize = true;
            this.labelModuleTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelModuleTitle.Location = new System.Drawing.Point(3, 0);
            this.labelModuleTitle.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelModuleTitle.MinimumSize = new System.Drawing.Size(60, 0);
            this.labelModuleTitle.Name = "labelModuleTitle";
            this.labelModuleTitle.Size = new System.Drawing.Size(60, 25);
            this.labelModuleTitle.TabIndex = 20;
            this.labelModuleTitle.Text = "Module:";
            this.labelModuleTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanelTaskModuleList
            // 
            this.tableLayoutPanelTaskModuleList.ColumnCount = 2;
            this.tableLayoutPanelTaskModuleList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.tableLayoutPanelTaskModuleList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskModuleList.Controls.Add(this.comboBoxTaskModuleList, 1, 0);
            this.tableLayoutPanelTaskModuleList.Controls.Add(this.pictureBoxTaskModuleList, 0, 0);
            this.tableLayoutPanelTaskModuleList.Location = new System.Drawing.Point(0, 31);
            this.tableLayoutPanelTaskModuleList.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTaskModuleList.Name = "tableLayoutPanelTaskModuleList";
            this.tableLayoutPanelTaskModuleList.RowCount = 1;
            this.tableLayoutPanelTaskModuleList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskModuleList.Size = new System.Drawing.Size(200, 25);
            this.tableLayoutPanelTaskModuleList.TabIndex = 2;
            // 
            // comboBoxTaskModuleList
            // 
            this.comboBoxTaskModuleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxTaskModuleList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTaskModuleList.FormattingEnabled = true;
            this.comboBoxTaskModuleList.Location = new System.Drawing.Point(24, 2);
            this.comboBoxTaskModuleList.Margin = new System.Windows.Forms.Padding(0, 2, 3, 3);
            this.comboBoxTaskModuleList.Name = "comboBoxTaskModuleList";
            this.comboBoxTaskModuleList.Size = new System.Drawing.Size(173, 21);
            this.comboBoxTaskModuleList.TabIndex = 1;
            // 
            // pictureBoxTaskModuleList
            // 
            this.pictureBoxTaskModuleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTaskModuleList.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxTaskModuleList.Name = "pictureBoxTaskModuleList";
            this.pictureBoxTaskModuleList.Size = new System.Drawing.Size(18, 19);
            this.pictureBoxTaskModuleList.TabIndex = 2;
            this.pictureBoxTaskModuleList.TabStop = false;
            // 
            // tableLayoutPanelTaskResult
            // 
            this.tableLayoutPanelTaskResult.ColumnCount = 2;
            this.tableLayoutPanelTaskResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskResult.Controls.Add(this.labelResult, 0, 0);
            this.tableLayoutPanelTaskResult.Controls.Add(this.comboBoxResult, 1, 0);
            this.tableLayoutPanelTaskResult.Location = new System.Drawing.Point(200, 31);
            this.tableLayoutPanelTaskResult.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTaskResult.Name = "tableLayoutPanelTaskResult";
            this.tableLayoutPanelTaskResult.RowCount = 1;
            this.tableLayoutPanelTaskResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskResult.Size = new System.Drawing.Size(197, 25);
            this.tableLayoutPanelTaskResult.TabIndex = 3;
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelResult.Location = new System.Drawing.Point(3, 0);
            this.labelResult.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelResult.MinimumSize = new System.Drawing.Size(60, 0);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(60, 25);
            this.labelResult.TabIndex = 20;
            this.labelResult.Text = "Result:";
            this.labelResult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBoxResult
            // 
            this.comboBoxResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxResult.FormattingEnabled = true;
            this.comboBoxResult.Location = new System.Drawing.Point(63, 3);
            this.comboBoxResult.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.comboBoxResult.Name = "comboBoxResult";
            this.comboBoxResult.Size = new System.Drawing.Size(131, 21);
            this.comboBoxResult.TabIndex = 21;
            // 
            // tableLayoutPanelTaskNumber
            // 
            this.tableLayoutPanelTaskNumber.ColumnCount = 2;
            this.tableLayoutPanelTaskNumber.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskNumber.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskNumber.Controls.Add(this.labelTaskNumber, 0, 0);
            this.tableLayoutPanelTaskNumber.Controls.Add(this.textBoxTaskNumber, 1, 0);
            this.tableLayoutPanelTaskNumber.Location = new System.Drawing.Point(0, 56);
            this.tableLayoutPanelTaskNumber.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTaskNumber.Name = "tableLayoutPanelTaskNumber";
            this.tableLayoutPanelTaskNumber.RowCount = 1;
            this.tableLayoutPanelTaskNumber.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskNumber.Size = new System.Drawing.Size(200, 25);
            this.tableLayoutPanelTaskNumber.TabIndex = 4;
            // 
            // labelTaskNumber
            // 
            this.labelTaskNumber.AutoSize = true;
            this.labelTaskNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaskNumber.Location = new System.Drawing.Point(3, 0);
            this.labelTaskNumber.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelTaskNumber.MinimumSize = new System.Drawing.Size(60, 0);
            this.labelTaskNumber.Name = "labelTaskNumber";
            this.labelTaskNumber.Size = new System.Drawing.Size(60, 25);
            this.labelTaskNumber.TabIndex = 0;
            this.labelTaskNumber.Text = "Number";
            this.labelTaskNumber.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxTaskNumber
            // 
            this.textBoxTaskNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxTaskNumber.Location = new System.Drawing.Point(63, 3);
            this.textBoxTaskNumber.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxTaskNumber.Name = "textBoxTaskNumber";
            this.textBoxTaskNumber.Size = new System.Drawing.Size(134, 20);
            this.textBoxTaskNumber.TabIndex = 1;
            // 
            // tableLayoutPanelTaskBool
            // 
            this.tableLayoutPanelTaskBool.ColumnCount = 2;
            this.tableLayoutPanelTaskBool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskBool.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskBool.Controls.Add(this.checkBoxTaskBool, 1, 0);
            this.tableLayoutPanelTaskBool.Controls.Add(this.labelTaskBool, 0, 0);
            this.tableLayoutPanelTaskBool.Location = new System.Drawing.Point(200, 56);
            this.tableLayoutPanelTaskBool.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTaskBool.Name = "tableLayoutPanelTaskBool";
            this.tableLayoutPanelTaskBool.RowCount = 1;
            this.tableLayoutPanelTaskBool.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskBool.Size = new System.Drawing.Size(200, 25);
            this.tableLayoutPanelTaskBool.TabIndex = 5;
            // 
            // checkBoxTaskBool
            // 
            this.checkBoxTaskBool.AutoSize = true;
            this.checkBoxTaskBool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxTaskBool.Location = new System.Drawing.Point(63, 4);
            this.checkBoxTaskBool.Margin = new System.Windows.Forms.Padding(0, 4, 3, 3);
            this.checkBoxTaskBool.Name = "checkBoxTaskBool";
            this.checkBoxTaskBool.Size = new System.Drawing.Size(134, 18);
            this.checkBoxTaskBool.TabIndex = 0;
            this.checkBoxTaskBool.UseVisualStyleBackColor = true;
            // 
            // labelTaskBool
            // 
            this.labelTaskBool.AutoSize = true;
            this.labelTaskBool.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaskBool.Location = new System.Drawing.Point(3, 0);
            this.labelTaskBool.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelTaskBool.MinimumSize = new System.Drawing.Size(60, 0);
            this.labelTaskBool.Name = "labelTaskBool";
            this.labelTaskBool.Size = new System.Drawing.Size(60, 25);
            this.labelTaskBool.TabIndex = 1;
            this.labelTaskBool.Text = "bool";
            this.labelTaskBool.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanelTaskDate
            // 
            this.tableLayoutPanelTaskDate.ColumnCount = 6;
            this.tableLayoutPanelTaskDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTaskDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskDate.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTaskDate.Controls.Add(this.labelTaskStart, 0, 0);
            this.tableLayoutPanelTaskDate.Controls.Add(this.dateTimePickerTaskStart, 1, 0);
            this.tableLayoutPanelTaskDate.Controls.Add(this.labelTaskEnd, 3, 0);
            this.tableLayoutPanelTaskDate.Controls.Add(this.dateTimePickerTaskEnd, 4, 0);
            this.tableLayoutPanelTaskDate.Controls.Add(this.buttonTaskDateDelete, 2, 0);
            this.tableLayoutPanelTaskDate.Controls.Add(this.buttonTaskDateEndDelete, 5, 0);
            this.tableLayoutPanelTaskDate.Location = new System.Drawing.Point(0, 81);
            this.tableLayoutPanelTaskDate.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTaskDate.Name = "tableLayoutPanelTaskDate";
            this.tableLayoutPanelTaskDate.RowCount = 1;
            this.tableLayoutPanelTaskDate.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskDate.Size = new System.Drawing.Size(258, 25);
            this.tableLayoutPanelTaskDate.TabIndex = 0;
            // 
            // labelTaskStart
            // 
            this.labelTaskStart.AutoSize = true;
            this.labelTaskStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaskStart.Location = new System.Drawing.Point(3, 0);
            this.labelTaskStart.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelTaskStart.MinimumSize = new System.Drawing.Size(60, 0);
            this.labelTaskStart.Name = "labelTaskStart";
            this.labelTaskStart.Size = new System.Drawing.Size(60, 25);
            this.labelTaskStart.TabIndex = 14;
            this.labelTaskStart.Text = "Start:";
            this.labelTaskStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerTaskStart
            // 
            this.dateTimePickerTaskStart.CalendarForeColor = System.Drawing.SystemColors.ControlDark;
            this.dateTimePickerTaskStart.CustomFormat = "yyyy-MM-dd  HH:mm:ss";
            this.dateTimePickerTaskStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerTaskStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTaskStart.Location = new System.Drawing.Point(63, 3);
            this.dateTimePickerTaskStart.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.dateTimePickerTaskStart.Name = "dateTimePickerTaskStart";
            this.dateTimePickerTaskStart.Size = new System.Drawing.Size(48, 20);
            this.dateTimePickerTaskStart.TabIndex = 15;
            // 
            // labelTaskEnd
            // 
            this.labelTaskEnd.AutoSize = true;
            this.labelTaskEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelTaskEnd.Location = new System.Drawing.Point(134, 0);
            this.labelTaskEnd.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelTaskEnd.Name = "labelTaskEnd";
            this.labelTaskEnd.Size = new System.Drawing.Size(29, 25);
            this.labelTaskEnd.TabIndex = 16;
            this.labelTaskEnd.Text = "End:";
            this.labelTaskEnd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // dateTimePickerTaskEnd
            // 
            this.dateTimePickerTaskEnd.CustomFormat = "yyyy-MM-dd  HH:mm:ss";
            this.dateTimePickerTaskEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dateTimePickerTaskEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTaskEnd.Location = new System.Drawing.Point(163, 3);
            this.dateTimePickerTaskEnd.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.dateTimePickerTaskEnd.Name = "dateTimePickerTaskEnd";
            this.dateTimePickerTaskEnd.Size = new System.Drawing.Size(76, 20);
            this.dateTimePickerTaskEnd.TabIndex = 17;
            // 
            // buttonTaskDateDelete
            // 
            this.buttonTaskDateDelete.FlatAppearance.BorderSize = 0;
            this.buttonTaskDateDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTaskDateDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonTaskDateDelete.Location = new System.Drawing.Point(111, 3);
            this.buttonTaskDateDelete.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonTaskDateDelete.Name = "buttonTaskDateDelete";
            this.buttonTaskDateDelete.Size = new System.Drawing.Size(17, 19);
            this.buttonTaskDateDelete.TabIndex = 18;
            this.buttonTaskDateDelete.UseVisualStyleBackColor = true;
            // 
            // buttonTaskDateEndDelete
            // 
            this.buttonTaskDateEndDelete.FlatAppearance.BorderSize = 0;
            this.buttonTaskDateEndDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonTaskDateEndDelete.Image = global::DiversityCollection.Resource.Delete;
            this.buttonTaskDateEndDelete.Location = new System.Drawing.Point(239, 3);
            this.buttonTaskDateEndDelete.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.buttonTaskDateEndDelete.Name = "buttonTaskDateEndDelete";
            this.buttonTaskDateEndDelete.Size = new System.Drawing.Size(16, 19);
            this.buttonTaskDateEndDelete.TabIndex = 19;
            this.buttonTaskDateEndDelete.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelTaskUri
            // 
            this.tableLayoutPanelTaskUri.ColumnCount = 3;
            this.tableLayoutPanelTaskUri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskUri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskUri.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelTaskUri.Controls.Add(this.buttonUrlOpen, 2, 0);
            this.tableLayoutPanelTaskUri.Controls.Add(this.textBoxURI, 1, 0);
            this.tableLayoutPanelTaskUri.Controls.Add(this.labelURI, 0, 0);
            this.tableLayoutPanelTaskUri.Location = new System.Drawing.Point(258, 81);
            this.tableLayoutPanelTaskUri.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTaskUri.Name = "tableLayoutPanelTaskUri";
            this.tableLayoutPanelTaskUri.RowCount = 1;
            this.tableLayoutPanelTaskUri.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskUri.Size = new System.Drawing.Size(200, 25);
            this.tableLayoutPanelTaskUri.TabIndex = 4;
            // 
            // buttonUrlOpen
            // 
            this.buttonUrlOpen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonUrlOpen.FlatAppearance.BorderSize = 0;
            this.buttonUrlOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUrlOpen.Image = global::DiversityCollection.Resource.Browse;
            this.buttonUrlOpen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonUrlOpen.Location = new System.Drawing.Point(180, 0);
            this.buttonUrlOpen.Margin = new System.Windows.Forms.Padding(0);
            this.buttonUrlOpen.Name = "buttonUrlOpen";
            this.buttonUrlOpen.Size = new System.Drawing.Size(20, 25);
            this.buttonUrlOpen.TabIndex = 10;
            this.buttonUrlOpen.UseVisualStyleBackColor = true;
            // 
            // textBoxURI
            // 
            this.textBoxURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxURI.Location = new System.Drawing.Point(63, 3);
            this.textBoxURI.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.textBoxURI.Name = "textBoxURI";
            this.textBoxURI.Size = new System.Drawing.Size(117, 20);
            this.textBoxURI.TabIndex = 9;
            // 
            // labelURI
            // 
            this.labelURI.AutoSize = true;
            this.labelURI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelURI.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelURI.Location = new System.Drawing.Point(3, 0);
            this.labelURI.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.labelURI.MinimumSize = new System.Drawing.Size(60, 0);
            this.labelURI.Name = "labelURI";
            this.labelURI.Size = new System.Drawing.Size(60, 25);
            this.labelURI.TabIndex = 8;
            this.labelURI.Text = "URI:";
            this.labelURI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanelTaskDescription
            // 
            this.tableLayoutPanelTaskDescription.ColumnCount = 2;
            this.tableLayoutPanelTaskDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskDescription.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskDescription.Controls.Add(this.labelDescription, 0, 0);
            this.tableLayoutPanelTaskDescription.Controls.Add(this.textBoxDescription, 1, 0);
            this.tableLayoutPanelTaskDescription.Location = new System.Drawing.Point(0, 106);
            this.tableLayoutPanelTaskDescription.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTaskDescription.Name = "tableLayoutPanelTaskDescription";
            this.tableLayoutPanelTaskDescription.RowCount = 1;
            this.tableLayoutPanelTaskDescription.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskDescription.Size = new System.Drawing.Size(200, 25);
            this.tableLayoutPanelTaskDescription.TabIndex = 2;
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelDescription.Location = new System.Drawing.Point(3, 6);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelDescription.MinimumSize = new System.Drawing.Size(60, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(63, 19);
            this.labelDescription.TabIndex = 4;
            this.labelDescription.Text = "Description:";
            this.labelDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxDescription.Location = new System.Drawing.Point(66, 3);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(131, 19);
            this.textBoxDescription.TabIndex = 5;
            // 
            // tableLayoutPanelTaskNotes
            // 
            this.tableLayoutPanelTaskNotes.ColumnCount = 2;
            this.tableLayoutPanelTaskNotes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTaskNotes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskNotes.Controls.Add(this.labelNotes, 0, 0);
            this.tableLayoutPanelTaskNotes.Controls.Add(this.textBoxNotes, 1, 0);
            this.tableLayoutPanelTaskNotes.Location = new System.Drawing.Point(200, 106);
            this.tableLayoutPanelTaskNotes.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelTaskNotes.Name = "tableLayoutPanelTaskNotes";
            this.tableLayoutPanelTaskNotes.RowCount = 1;
            this.tableLayoutPanelTaskNotes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTaskNotes.Size = new System.Drawing.Size(200, 25);
            this.tableLayoutPanelTaskNotes.TabIndex = 3;
            // 
            // labelNotes
            // 
            this.labelNotes.AutoSize = true;
            this.labelNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNotes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelNotes.Location = new System.Drawing.Point(3, 6);
            this.labelNotes.Margin = new System.Windows.Forms.Padding(3, 6, 0, 0);
            this.labelNotes.MinimumSize = new System.Drawing.Size(60, 0);
            this.labelNotes.Name = "labelNotes";
            this.labelNotes.Size = new System.Drawing.Size(60, 19);
            this.labelNotes.TabIndex = 6;
            this.labelNotes.Text = "Notes:";
            this.labelNotes.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNotes.Location = new System.Drawing.Point(63, 3);
            this.textBoxNotes.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(134, 19);
            this.textBoxNotes.TabIndex = 7;
            // 
            // UserControl_CollectionTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxCollectionTask);
            this.Name = "UserControl_CollectionTask";
            this.Size = new System.Drawing.Size(539, 174);
            this.groupBoxCollectionTask.ResumeLayout(false);
            this.flowLayoutPanelTask.ResumeLayout(false);
            this.tableLayoutPanelTaskHeader.ResumeLayout(false);
            this.tableLayoutPanelTaskHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDisplayOrder)).EndInit();
            this.tableLayoutPanelTaskModule.ResumeLayout(false);
            this.tableLayoutPanelTaskModule.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaskModule)).EndInit();
            this.tableLayoutPanelTaskModuleList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTaskModuleList)).EndInit();
            this.tableLayoutPanelTaskResult.ResumeLayout(false);
            this.tableLayoutPanelTaskResult.PerformLayout();
            this.tableLayoutPanelTaskNumber.ResumeLayout(false);
            this.tableLayoutPanelTaskNumber.PerformLayout();
            this.tableLayoutPanelTaskBool.ResumeLayout(false);
            this.tableLayoutPanelTaskBool.PerformLayout();
            this.tableLayoutPanelTaskDate.ResumeLayout(false);
            this.tableLayoutPanelTaskDate.PerformLayout();
            this.tableLayoutPanelTaskUri.ResumeLayout(false);
            this.tableLayoutPanelTaskUri.PerformLayout();
            this.tableLayoutPanelTaskDescription.ResumeLayout(false);
            this.tableLayoutPanelTaskDescription.PerformLayout();
            this.tableLayoutPanelTaskNotes.ResumeLayout(false);
            this.tableLayoutPanelTaskNotes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCollectionTask;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTask;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskHeader;
        private System.Windows.Forms.PictureBox pictureBoxType;
        private System.Windows.Forms.Label labelTaskHaeder;
        private System.Windows.Forms.NumericUpDown numericUpDownDisplayOrder;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskModule;
        private DiversityWorkbench.UserControls.UserControlModuleRelatedEntry userControlModuleRelatedEntryValue;
        private System.Windows.Forms.PictureBox pictureBoxTaskModule;
        private System.Windows.Forms.Label labelModuleTitle;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskModuleList;
        private System.Windows.Forms.ComboBox comboBoxTaskModuleList;
        private System.Windows.Forms.PictureBox pictureBoxTaskModuleList;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskResult;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.ComboBox comboBoxResult;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskNumber;
        private System.Windows.Forms.Label labelTaskNumber;
        private System.Windows.Forms.TextBox textBoxTaskNumber;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskBool;
        private System.Windows.Forms.CheckBox checkBoxTaskBool;
        private System.Windows.Forms.Label labelTaskBool;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskDate;
        private System.Windows.Forms.Label labelTaskStart;
        private System.Windows.Forms.DateTimePicker dateTimePickerTaskStart;
        private System.Windows.Forms.Label labelTaskEnd;
        private System.Windows.Forms.DateTimePicker dateTimePickerTaskEnd;
        private System.Windows.Forms.Button buttonTaskDateDelete;
        private System.Windows.Forms.Button buttonTaskDateEndDelete;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskUri;
        private System.Windows.Forms.Button buttonUrlOpen;
        private System.Windows.Forms.TextBox textBoxURI;
        private System.Windows.Forms.Label labelURI;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskDescription;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTaskNotes;
        private System.Windows.Forms.Label labelNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
    }
}
