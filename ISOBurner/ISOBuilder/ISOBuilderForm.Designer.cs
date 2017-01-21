namespace ISOBuilder
{
    partial class ISOBuilderForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label label3;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label4;
            System.Windows.Forms.Label label5;
            System.Windows.Forms.Label label9;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ISOBuilderForm));
            this._lblMediaType = new System.Windows.Forms.Label();
            this._lblDst = new System.Windows.Forms.Label();
            this._backgroundISOWorker = new System.ComponentModel.BackgroundWorker();
            this._tabBuild = new System.Windows.Forms.TabControl();
            this._tabSelectFiles = new System.Windows.Forms.TabPage();
            this._ckMakeBootable = new System.Windows.Forms.CheckBox();
            this._btnAbout = new System.Windows.Forms.Button();
            this._btnAddFile = new System.Windows.Forms.Button();
            this._btnAddFolder = new System.Windows.Forms.Button();
            this._dataGridView = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fullNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this._tabPageBuild = new System.Windows.Forms.TabPage();
            this._ckbForceTermination = new System.Windows.Forms.CheckBox();
            this._cbxAfterCompletion = new System.Windows.Forms.ComboBox();
            this._tabFormat = new System.Windows.Forms.TabControl();
            this._TabPageFile = new System.Windows.Forms.TabPage();
            this._btnBuildFile = new System.Windows.Forms.Button();
            this._ckUseUIReport = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._ckbISO9660 = new System.Windows.Forms.CheckBox();
            this._ckbJoliet = new System.Windows.Forms.CheckBox();
            this._ckbUDF = new System.Windows.Forms.CheckBox();
            this._ckWorker = new System.Windows.Forms.CheckBox();
            this._SaveAs = new System.Windows.Forms.Button();
            this._txtVolName = new System.Windows.Forms.TextBox();
            this._cbxMediaTypes = new System.Windows.Forms.ComboBox();
            this._lblDest = new System.Windows.Forms.Label();
            this._tabPageBurn = new System.Windows.Forms.TabPage();
            this._btnBurn = new System.Windows.Forms.Button();
            this._ckbCloseMedia = new System.Windows.Forms.CheckBox();
            this._lblFileImage = new System.Windows.Forms.Label();
            this._btnSelectImage = new System.Windows.Forms.Button();
            this._ckbEject = new System.Windows.Forms.CheckBox();
            this._txtCrtDiskType = new System.Windows.Forms.TextBox();
            this._lblBurner = new System.Windows.Forms.Label();
            this._cbxSpeed = new System.Windows.Forms.ComboBox();
            this._cbxBurners = new System.Windows.Forms.ComboBox();
            this._lblCrtDiskType = new System.Windows.Forms.Label();
            this._lblSpeed = new System.Windows.Forms.Label();
            this._tabPageformat = new System.Windows.Forms.TabPage();
            this._btnFormat = new System.Windows.Forms.Button();
            this._ckbRepair = new System.Windows.Forms.CheckBox();
            this._ckQuick = new System.Windows.Forms.CheckBox();
            this._ckbEjectFormat = new System.Windows.Forms.CheckBox();
            this._txtDiskTypeFormat = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this._cbxFormatBurners = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this._lblUpdate = new System.Windows.Forms.Label();
            this._progBar = new System.Windows.Forms.ProgressBar();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._backgroundWorkerBurn = new System.ComponentModel.BackgroundWorker();
            this._backgroundWorkerErase = new System.ComponentModel.BackgroundWorker();
            label3 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            this._tabBuild.SuspendLayout();
            this._tabSelectFiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this._tabPageBuild.SuspendLayout();
            this._tabFormat.SuspendLayout();
            this._TabPageFile.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._tabPageBurn.SuspendLayout();
            this._tabPageformat.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(263, 99);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(76, 13);
            label3.TabIndex = 6;
            label3.Text = "Volume Name:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(35, 235);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(381, 16);
            label1.TabIndex = 3;
            label1.Text = "Drag and drop files and folders or use the Add buttons";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(109, 154);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(39, 13);
            label4.TabIndex = 26;
            label4.Text = "Image:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(109, 154);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(39, 13);
            label5.TabIndex = 26;
            label5.Text = "Image:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(3, 235);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(175, 13);
            label9.TabIndex = 22;
            label9.Text = "&Action After Successful Completion:";
            // 
            // _lblMediaType
            // 
            this._lblMediaType.AutoSize = true;
            this._lblMediaType.Location = new System.Drawing.Point(6, 15);
            this._lblMediaType.Name = "_lblMediaType";
            this._lblMediaType.Size = new System.Drawing.Size(66, 13);
            this._lblMediaType.TabIndex = 6;
            this._lblMediaType.Text = "Media Type:";
            // 
            // _lblDst
            // 
            this._lblDst.AutoSize = true;
            this._lblDst.Location = new System.Drawing.Point(78, 47);
            this._lblDst.Name = "_lblDst";
            this._lblDst.Size = new System.Drawing.Size(63, 13);
            this._lblDst.TabIndex = 15;
            this._lblDst.Text = "Destination:";
            // 
            // _backgroundISOWorker
            // 
            this._backgroundISOWorker.WorkerReportsProgress = true;
            this._backgroundISOWorker.WorkerSupportsCancellation = true;
            // 
            // _tabBuild
            // 
            this._tabBuild.Controls.Add(this._tabSelectFiles);
            this._tabBuild.Controls.Add(this._tabPageBuild);
            this._tabBuild.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabBuild.Location = new System.Drawing.Point(0, 0);
            this._tabBuild.Name = "_tabBuild";
            this._tabBuild.SelectedIndex = 0;
            this._tabBuild.Size = new System.Drawing.Size(488, 361);
            this._tabBuild.TabIndex = 0;
            // 
            // _tabSelectFiles
            // 
            this._tabSelectFiles.AllowDrop = true;
            this._tabSelectFiles.Controls.Add(this._ckMakeBootable);
            this._tabSelectFiles.Controls.Add(label1);
            this._tabSelectFiles.Controls.Add(this._btnAbout);
            this._tabSelectFiles.Controls.Add(this._btnAddFile);
            this._tabSelectFiles.Controls.Add(this._btnAddFolder);
            this._tabSelectFiles.Controls.Add(this._dataGridView);
            this._tabSelectFiles.Controls.Add(this.bindingNavigator1);
            this._tabSelectFiles.Location = new System.Drawing.Point(4, 22);
            this._tabSelectFiles.Name = "_tabSelectFiles";
            this._tabSelectFiles.Padding = new System.Windows.Forms.Padding(3);
            this._tabSelectFiles.Size = new System.Drawing.Size(480, 335);
            this._tabSelectFiles.TabIndex = 0;
            this._tabSelectFiles.Text = "Select Files";
            this._tabSelectFiles.UseVisualStyleBackColor = true;
            this._tabSelectFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabPage1_DragDrop);
            this._tabSelectFiles.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabPage1_DragEnter);
            // 
            // _ckMakeBootable
            // 
            this._ckMakeBootable.AutoSize = true;
            this._ckMakeBootable.Location = new System.Drawing.Point(260, 275);
            this._ckMakeBootable.Name = "_ckMakeBootable";
            this._ckMakeBootable.Size = new System.Drawing.Size(130, 17);
            this._ckMakeBootable.TabIndex = 4;
            this._ckMakeBootable.Text = "Make &Bootable Image";
            this._ckMakeBootable.UseVisualStyleBackColor = true;
            this._ckMakeBootable.Click += new System.EventHandler(this._cbxMakeBootable_Click);
            // 
            // _btnAbout
            // 
            this._btnAbout.Location = new System.Drawing.Point(496, 271);
            this._btnAbout.Name = "_btnAbout";
            this._btnAbout.Size = new System.Drawing.Size(75, 23);
            this._btnAbout.TabIndex = 2;
            this._btnAbout.Text = "&About";
            this._btnAbout.UseVisualStyleBackColor = true;
            this._btnAbout.Click += new System.EventHandler(this._btnAbout_Click);
            // 
            // _btnAddFile
            // 
            this._btnAddFile.Location = new System.Drawing.Point(118, 271);
            this._btnAddFile.Name = "_btnAddFile";
            this._btnAddFile.Size = new System.Drawing.Size(89, 23);
            this._btnAddFile.TabIndex = 2;
            this._btnAddFile.Text = "Add &File";
            this._btnAddFile.UseVisualStyleBackColor = true;
            this._btnAddFile.Click += new System.EventHandler(this._btnAddFile_Click);
            // 
            // _btnAddFolder
            // 
            this._btnAddFolder.Location = new System.Drawing.Point(8, 271);
            this._btnAddFolder.Name = "_btnAddFolder";
            this._btnAddFolder.Size = new System.Drawing.Size(96, 23);
            this._btnAddFolder.TabIndex = 2;
            this._btnAddFolder.Text = "Add &Directory";
            this._btnAddFolder.UseVisualStyleBackColor = true;
            this._btnAddFolder.Click += new System.EventHandler(this._btnAddFolder_Click);
            // 
            // _dataGridView
            // 
            this._dataGridView.AllowUserToAddRows = false;
            this._dataGridView.AllowUserToDeleteRows = false;
            this._dataGridView.AutoGenerateColumns = false;
            this._dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.fullNameDataGridViewTextBoxColumn});
            this._dataGridView.DataSource = this._bindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this._dataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this._dataGridView.Location = new System.Drawing.Point(3, 28);
            this._dataGridView.Name = "_dataGridView";
            this._dataGridView.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this._dataGridView.Size = new System.Drawing.Size(474, 194);
            this._dataGridView.TabIndex = 1;
            // 
            // Type
            // 
            this.Type.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Type.FillWeight = 101.5228F;
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Type.Width = 70;
            // 
            // fullNameDataGridViewTextBoxColumn
            // 
            this.fullNameDataGridViewTextBoxColumn.DataPropertyName = "FullName";
            this.fullNameDataGridViewTextBoxColumn.FillWeight = 98.47713F;
            this.fullNameDataGridViewTextBoxColumn.HeaderText = "FullName";
            this.fullNameDataGridViewTextBoxColumn.Name = "fullNameDataGridViewTextBoxColumn";
            this.fullNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // _bindingSource
            // 
            this._bindingSource.AllowNew = true;
            this._bindingSource.DataSource = typeof(FileImage.UniqueListFileSystemInfo);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BindingSource = this._bindingSource;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.bindingNavigator1.Location = new System.Drawing.Point(3, 3);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(474, 25);
            this.bindingNavigator1.TabIndex = 0;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(36, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 21);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Enabled = false;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // _tabPageBuild
            // 
            this._tabPageBuild.Controls.Add(this._ckbForceTermination);
            this._tabPageBuild.Controls.Add(this._cbxAfterCompletion);
            this._tabPageBuild.Controls.Add(label9);
            this._tabPageBuild.Controls.Add(this._tabFormat);
            this._tabPageBuild.Controls.Add(this._lblUpdate);
            this._tabPageBuild.Controls.Add(this._progBar);
            this._tabPageBuild.Location = new System.Drawing.Point(4, 22);
            this._tabPageBuild.Name = "_tabPageBuild";
            this._tabPageBuild.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageBuild.Size = new System.Drawing.Size(480, 335);
            this._tabPageBuild.TabIndex = 1;
            this._tabPageBuild.Text = "Build/Burn Image";
            this._tabPageBuild.UseVisualStyleBackColor = true;
            // 
            // _ckbForceTermination
            // 
            this._ckbForceTermination.AutoSize = true;
            this._ckbForceTermination.Location = new System.Drawing.Point(346, 234);
            this._ckbForceTermination.Name = "_ckbForceTermination";
            this._ckbForceTermination.Size = new System.Drawing.Size(111, 17);
            this._ckbForceTermination.TabIndex = 24;
            this._ckbForceTermination.Text = "&Force Termination";
            this._ckbForceTermination.UseVisualStyleBackColor = true;
            // 
            // _cbxAfterCompletion
            // 
            this._cbxAfterCompletion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbxAfterCompletion.FormattingEnabled = true;
            this._cbxAfterCompletion.Location = new System.Drawing.Point(189, 232);
            this._cbxAfterCompletion.Name = "_cbxAfterCompletion";
            this._cbxAfterCompletion.Size = new System.Drawing.Size(142, 21);
            this._cbxAfterCompletion.TabIndex = 23;
            this._cbxAfterCompletion.SelectedValueChanged += new System.EventHandler(this._cbxAfterCompletion_SelectedValueChanged);
            // 
            // _tabFormat
            // 
            this._tabFormat.Controls.Add(this._TabPageFile);
            this._tabFormat.Controls.Add(this._tabPageBurn);
            this._tabFormat.Controls.Add(this._tabPageformat);
            this._tabFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this._tabFormat.Location = new System.Drawing.Point(3, 3);
            this._tabFormat.Name = "_tabFormat";
            this._tabFormat.SelectedIndex = 0;
            this._tabFormat.Size = new System.Drawing.Size(474, 209);
            this._tabFormat.TabIndex = 21;
            this._tabFormat.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this._tabTarget_Selecting);
            // 
            // _TabPageFile
            // 
            this._TabPageFile.Controls.Add(this._lblMediaType);
            this._TabPageFile.Controls.Add(this._btnBuildFile);
            this._TabPageFile.Controls.Add(this._ckUseUIReport);
            this._TabPageFile.Controls.Add(this.groupBox1);
            this._TabPageFile.Controls.Add(this._ckWorker);
            this._TabPageFile.Controls.Add(this._lblDst);
            this._TabPageFile.Controls.Add(this._SaveAs);
            this._TabPageFile.Controls.Add(this._txtVolName);
            this._TabPageFile.Controls.Add(this._cbxMediaTypes);
            this._TabPageFile.Controls.Add(this._lblDest);
            this._TabPageFile.Controls.Add(label3);
            this._TabPageFile.Location = new System.Drawing.Point(4, 22);
            this._TabPageFile.Name = "_TabPageFile";
            this._TabPageFile.Padding = new System.Windows.Forms.Padding(3);
            this._TabPageFile.Size = new System.Drawing.Size(466, 183);
            this._TabPageFile.TabIndex = 0;
            this._TabPageFile.Text = "Create File";
            this._TabPageFile.UseVisualStyleBackColor = true;
            // 
            // _btnBuildFile
            // 
            this._btnBuildFile.Location = new System.Drawing.Point(378, 12);
            this._btnBuildFile.Name = "_btnBuildFile";
            this._btnBuildFile.Size = new System.Drawing.Size(71, 23);
            this._btnBuildFile.TabIndex = 7;
            this._btnBuildFile.Text = "&Create file";
            this._btnBuildFile.UseVisualStyleBackColor = true;
            this._btnBuildFile.Click += new System.EventHandler(this._btnBuildFile_Click);
            // 
            // _ckUseUIReport
            // 
            this._ckUseUIReport.AutoSize = true;
            this._ckUseUIReport.Checked = true;
            this._ckUseUIReport.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ckUseUIReport.Location = new System.Drawing.Point(134, 150);
            this._ckUseUIReport.Name = "_ckUseUIReport";
            this._ckUseUIReport.Size = new System.Drawing.Size(94, 17);
            this._ckUseUIReport.TabIndex = 13;
            this._ckUseUIReport.Text = "Use &UI Report";
            this._ckUseUIReport.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._ckbISO9660);
            this.groupBox1.Controls.Add(this._ckbJoliet);
            this.groupBox1.Controls.Add(this._ckbUDF);
            this.groupBox1.Location = new System.Drawing.Point(9, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 54);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "File Systems";
            // 
            // _ckbISO9660
            // 
            this._ckbISO9660.AutoSize = true;
            this._ckbISO9660.Checked = true;
            this._ckbISO9660.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ckbISO9660.Location = new System.Drawing.Point(11, 19);
            this._ckbISO9660.Name = "_ckbISO9660";
            this._ckbISO9660.Size = new System.Drawing.Size(68, 17);
            this._ckbISO9660.TabIndex = 10;
            this._ckbISO9660.Text = "ISO9660";
            this._ckbISO9660.UseVisualStyleBackColor = true;
            // 
            // _ckbJoliet
            // 
            this._ckbJoliet.AutoSize = true;
            this._ckbJoliet.Checked = true;
            this._ckbJoliet.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ckbJoliet.Location = new System.Drawing.Point(99, 19);
            this._ckbJoliet.Name = "_ckbJoliet";
            this._ckbJoliet.Size = new System.Drawing.Size(50, 17);
            this._ckbJoliet.TabIndex = 10;
            this._ckbJoliet.Text = "Joliet";
            this._ckbJoliet.UseVisualStyleBackColor = true;
            // 
            // _ckbUDF
            // 
            this._ckbUDF.AutoSize = true;
            this._ckbUDF.Location = new System.Drawing.Point(174, 19);
            this._ckbUDF.Name = "_ckbUDF";
            this._ckbUDF.Size = new System.Drawing.Size(48, 17);
            this._ckbUDF.TabIndex = 10;
            this._ckbUDF.Text = "UDF";
            this._ckbUDF.UseVisualStyleBackColor = true;
            // 
            // _ckWorker
            // 
            this._ckWorker.AutoSize = true;
            this._ckWorker.Location = new System.Drawing.Point(9, 150);
            this._ckWorker.Name = "_ckWorker";
            this._ckWorker.Size = new System.Drawing.Size(120, 17);
            this._ckWorker.TabIndex = 13;
            this._ckWorker.Text = "Use Worker &Thread";
            this._ckWorker.UseVisualStyleBackColor = false;
            // 
            // _SaveAs
            // 
            this._SaveAs.Location = new System.Drawing.Point(9, 42);
            this._SaveAs.Name = "_SaveAs";
            this._SaveAs.Size = new System.Drawing.Size(61, 23);
            this._SaveAs.TabIndex = 8;
            this._SaveAs.Text = "Save &As";
            this._SaveAs.UseVisualStyleBackColor = true;
            this._SaveAs.Click += new System.EventHandler(this._SaveAs_Click);
            // 
            // _txtVolName
            // 
            this._txtVolName.AcceptsReturn = true;
            this._txtVolName.Location = new System.Drawing.Point(349, 96);
            this._txtVolName.Name = "_txtVolName";
            this._txtVolName.Size = new System.Drawing.Size(100, 20);
            this._txtVolName.TabIndex = 11;
            // 
            // _cbxMediaTypes
            // 
            this._cbxMediaTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbxMediaTypes.FormattingEnabled = true;
            this._cbxMediaTypes.Location = new System.Drawing.Point(79, 12);
            this._cbxMediaTypes.Name = "_cbxMediaTypes";
            this._cbxMediaTypes.Size = new System.Drawing.Size(167, 21);
            this._cbxMediaTypes.TabIndex = 12;
            // 
            // _lblDest
            // 
            this._lblDest.AutoSize = true;
            this._lblDest.Location = new System.Drawing.Point(147, 47);
            this._lblDest.Name = "_lblDest";
            this._lblDest.Size = new System.Drawing.Size(22, 13);
            this._lblDest.TabIndex = 9;
            this._lblDest.Text = "xxx";
            // 
            // _tabPageBurn
            // 
            this._tabPageBurn.Controls.Add(this._btnBurn);
            this._tabPageBurn.Controls.Add(this._ckbCloseMedia);
            this._tabPageBurn.Controls.Add(this._lblFileImage);
            this._tabPageBurn.Controls.Add(label4);
            this._tabPageBurn.Controls.Add(this._btnSelectImage);
            this._tabPageBurn.Controls.Add(this._ckbEject);
            this._tabPageBurn.Controls.Add(this._txtCrtDiskType);
            this._tabPageBurn.Controls.Add(this._lblBurner);
            this._tabPageBurn.Controls.Add(this._cbxSpeed);
            this._tabPageBurn.Controls.Add(this._cbxBurners);
            this._tabPageBurn.Controls.Add(this._lblCrtDiskType);
            this._tabPageBurn.Controls.Add(this._lblSpeed);
            this._tabPageBurn.Location = new System.Drawing.Point(4, 22);
            this._tabPageBurn.Name = "_tabPageBurn";
            this._tabPageBurn.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageBurn.Size = new System.Drawing.Size(466, 183);
            this._tabPageBurn.TabIndex = 1;
            this._tabPageBurn.Text = "Burn Image";
            this._tabPageBurn.UseVisualStyleBackColor = true;
            // 
            // _btnBurn
            // 
            this._btnBurn.Location = new System.Drawing.Point(374, 12);
            this._btnBurn.Name = "_btnBurn";
            this._btnBurn.Size = new System.Drawing.Size(75, 23);
            this._btnBurn.TabIndex = 29;
            this._btnBurn.Text = "&Burn Image";
            this._btnBurn.UseVisualStyleBackColor = true;
            this._btnBurn.Click += new System.EventHandler(this._btnBurn_Click);
            // 
            // _ckbCloseMedia
            // 
            this._ckbCloseMedia.AutoSize = true;
            this._ckbCloseMedia.Checked = true;
            this._ckbCloseMedia.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ckbCloseMedia.Location = new System.Drawing.Point(239, 65);
            this._ckbCloseMedia.Name = "_ckbCloseMedia";
            this._ckbCloseMedia.Size = new System.Drawing.Size(83, 17);
            this._ckbCloseMedia.TabIndex = 28;
            this._ckbCloseMedia.Text = "Close media";
            this._ckbCloseMedia.UseVisualStyleBackColor = true;
            // 
            // _lblFileImage
            // 
            this._lblFileImage.AutoSize = true;
            this._lblFileImage.Location = new System.Drawing.Point(155, 153);
            this._lblFileImage.Name = "_lblFileImage";
            this._lblFileImage.Size = new System.Drawing.Size(22, 13);
            this._lblFileImage.TabIndex = 27;
            this._lblFileImage.Text = "xxx";
            // 
            // _btnSelectImage
            // 
            this._btnSelectImage.Location = new System.Drawing.Point(9, 149);
            this._btnSelectImage.Name = "_btnSelectImage";
            this._btnSelectImage.Size = new System.Drawing.Size(82, 23);
            this._btnSelectImage.TabIndex = 25;
            this._btnSelectImage.Text = "Select &Image";
            this._btnSelectImage.UseVisualStyleBackColor = true;
            this._btnSelectImage.Click += new System.EventHandler(this._btnSelectImage_Click);
            // 
            // _ckbEject
            // 
            this._ckbEject.AutoSize = true;
            this._ckbEject.Checked = true;
            this._ckbEject.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ckbEject.Location = new System.Drawing.Point(239, 16);
            this._ckbEject.Name = "_ckbEject";
            this._ckbEject.Size = new System.Drawing.Size(106, 17);
            this._ckbEject.TabIndex = 23;
            this._ckbEject.Text = "Eject when done";
            this._ckbEject.UseVisualStyleBackColor = true;
            // 
            // _txtCrtDiskType
            // 
            this._txtCrtDiskType.Location = new System.Drawing.Point(107, 111);
            this._txtCrtDiskType.Name = "_txtCrtDiskType";
            this._txtCrtDiskType.ReadOnly = true;
            this._txtCrtDiskType.Size = new System.Drawing.Size(203, 20);
            this._txtCrtDiskType.TabIndex = 21;
            // 
            // _lblBurner
            // 
            this._lblBurner.AutoSize = true;
            this._lblBurner.Location = new System.Drawing.Point(6, 15);
            this._lblBurner.Name = "_lblBurner";
            this._lblBurner.Size = new System.Drawing.Size(41, 13);
            this._lblBurner.TabIndex = 17;
            this._lblBurner.Text = "Burner:";
            // 
            // _cbxSpeed
            // 
            this._cbxSpeed.DisplayMember = "Value";
            this._cbxSpeed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbxSpeed.FormattingEnabled = true;
            this._cbxSpeed.Location = new System.Drawing.Point(86, 61);
            this._cbxSpeed.Name = "_cbxSpeed";
            this._cbxSpeed.Size = new System.Drawing.Size(132, 21);
            this._cbxSpeed.TabIndex = 20;
            this._cbxSpeed.ValueMember = "Key";
            // 
            // _cbxBurners
            // 
            this._cbxBurners.DisplayMember = "Value";
            this._cbxBurners.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbxBurners.FormattingEnabled = true;
            this._cbxBurners.Location = new System.Drawing.Point(53, 12);
            this._cbxBurners.Name = "_cbxBurners";
            this._cbxBurners.Size = new System.Drawing.Size(165, 21);
            this._cbxBurners.TabIndex = 18;
            this._cbxBurners.ValueMember = "Key";
            this._cbxBurners.SelectedIndexChanged += new System.EventHandler(this._cbxBurners_SelectedIndexChanged);
            // 
            // _lblCrtDiskType
            // 
            this._lblCrtDiskType.AutoSize = true;
            this._lblCrtDiskType.Location = new System.Drawing.Point(6, 114);
            this._lblCrtDiskType.Name = "_lblCrtDiskType";
            this._lblCrtDiskType.Size = new System.Drawing.Size(95, 13);
            this._lblCrtDiskType.TabIndex = 19;
            this._lblCrtDiskType.Text = "Current Disk Type:";
            // 
            // _lblSpeed
            // 
            this._lblSpeed.AutoSize = true;
            this._lblSpeed.Location = new System.Drawing.Point(6, 64);
            this._lblSpeed.Name = "_lblSpeed";
            this._lblSpeed.Size = new System.Drawing.Size(74, 13);
            this._lblSpeed.TabIndex = 19;
            this._lblSpeed.Text = "Speed (KB/s):";
            // 
            // _tabPageformat
            // 
            this._tabPageformat.Controls.Add(this._btnFormat);
            this._tabPageformat.Controls.Add(this._ckbRepair);
            this._tabPageformat.Controls.Add(this._ckQuick);
            this._tabPageformat.Controls.Add(this._ckbEjectFormat);
            this._tabPageformat.Controls.Add(this._txtDiskTypeFormat);
            this._tabPageformat.Controls.Add(this.label11);
            this._tabPageformat.Controls.Add(this._cbxFormatBurners);
            this._tabPageformat.Controls.Add(this.label12);
            this._tabPageformat.Location = new System.Drawing.Point(4, 22);
            this._tabPageformat.Name = "_tabPageformat";
            this._tabPageformat.Padding = new System.Windows.Forms.Padding(3);
            this._tabPageformat.Size = new System.Drawing.Size(466, 183);
            this._tabPageformat.TabIndex = 2;
            this._tabPageformat.Text = "Format Disk";
            this._tabPageformat.UseVisualStyleBackColor = true;
            // 
            // _btnFormat
            // 
            this._btnFormat.Location = new System.Drawing.Point(374, 12);
            this._btnFormat.Name = "_btnFormat";
            this._btnFormat.Size = new System.Drawing.Size(75, 23);
            this._btnFormat.TabIndex = 29;
            this._btnFormat.Text = "&Format Image";
            this._btnFormat.UseVisualStyleBackColor = true;
            this._btnFormat.Click += new System.EventHandler(this._btnFormat_Click);
            // 
            // _ckbRepair
            // 
            this._ckbRepair.AutoSize = true;
            this._ckbRepair.Location = new System.Drawing.Point(263, 65);
            this._ckbRepair.Name = "_ckbRepair";
            this._ckbRepair.Size = new System.Drawing.Size(179, 17);
            this._ckbRepair.TabIndex = 23;
            this._ckbRepair.Text = "Repair Unrecognized RW media";
            this._ckbRepair.UseVisualStyleBackColor = true;
            this._ckbRepair.CheckedChanged += new System.EventHandler(this._ckRepair_CheckedChanged);
            // 
            // _ckQuick
            // 
            this._ckQuick.AutoSize = true;
            this._ckQuick.Checked = true;
            this._ckQuick.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ckQuick.Location = new System.Drawing.Point(129, 65);
            this._ckQuick.Name = "_ckQuick";
            this._ckQuick.Size = new System.Drawing.Size(89, 17);
            this._ckQuick.TabIndex = 23;
            this._ckQuick.Text = "Quick Format";
            this._ckQuick.UseVisualStyleBackColor = true;
            // 
            // _ckbEjectFormat
            // 
            this._ckbEjectFormat.AutoSize = true;
            this._ckbEjectFormat.Location = new System.Drawing.Point(9, 65);
            this._ckbEjectFormat.Name = "_ckbEjectFormat";
            this._ckbEjectFormat.Size = new System.Drawing.Size(106, 17);
            this._ckbEjectFormat.TabIndex = 23;
            this._ckbEjectFormat.Text = "Eject when done";
            this._ckbEjectFormat.UseVisualStyleBackColor = true;
            // 
            // _txtDiskTypeFormat
            // 
            this._txtDiskTypeFormat.Location = new System.Drawing.Point(107, 111);
            this._txtDiskTypeFormat.Name = "_txtDiskTypeFormat";
            this._txtDiskTypeFormat.ReadOnly = true;
            this._txtDiskTypeFormat.Size = new System.Drawing.Size(203, 20);
            this._txtDiskTypeFormat.TabIndex = 21;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 13);
            this.label11.TabIndex = 17;
            this.label11.Text = "Burner:";
            // 
            // _cbxFormatBurners
            // 
            this._cbxFormatBurners.DisplayMember = "Value";
            this._cbxFormatBurners.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbxFormatBurners.FormattingEnabled = true;
            this._cbxFormatBurners.Location = new System.Drawing.Point(53, 12);
            this._cbxFormatBurners.Name = "_cbxFormatBurners";
            this._cbxFormatBurners.Size = new System.Drawing.Size(165, 21);
            this._cbxFormatBurners.TabIndex = 18;
            this._cbxFormatBurners.ValueMember = "Key";
            this._cbxFormatBurners.SelectedIndexChanged += new System.EventHandler(this._cbxFormatBurners_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 114);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Current Disk Type:";
            // 
            // _lblUpdate
            // 
            this._lblUpdate.AutoSize = true;
            this._lblUpdate.Location = new System.Drawing.Point(7, 313);
            this._lblUpdate.Name = "_lblUpdate";
            this._lblUpdate.Size = new System.Drawing.Size(22, 13);
            this._lblUpdate.TabIndex = 6;
            this._lblUpdate.Text = "xxx";
            // 
            // _progBar
            // 
            this._progBar.Location = new System.Drawing.Point(6, 277);
            this._progBar.Maximum = 200;
            this._progBar.Name = "_progBar";
            this._progBar.Size = new System.Drawing.Size(449, 23);
            this._progBar.Step = 1;
            this._progBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this._progBar.TabIndex = 5;
            // 
            // _openFileDialog
            // 
            this._openFileDialog.Filter = "All files|*.*";
            // 
            // _saveFileDialog
            // 
            this._saveFileDialog.Filter = "iso files|*.iso|mdf files|*.mdf|bwt files|*.bwt|All files|*.*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(155, 153);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Burner:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 114);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Current Disk Type:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(74, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Speed (KB/s):";
            // 
            // _backgroundWorkerBurn
            // 
            this._backgroundWorkerBurn.WorkerReportsProgress = true;
            this._backgroundWorkerBurn.WorkerSupportsCancellation = true;
            this._backgroundWorkerBurn.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerBurn_DoWork);
            this._backgroundWorkerBurn.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerBurn_RunWorkerCompleted);
            this._backgroundWorkerBurn.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerBurn_ProgressChanged);
            // 
            // _backgroundWorkerErase
            // 
            this._backgroundWorkerErase.WorkerReportsProgress = true;
            this._backgroundWorkerErase.WorkerSupportsCancellation = true;
            this._backgroundWorkerErase.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerErase_DoWork);
            this._backgroundWorkerErase.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerErase_RunWorkerCompleted);
            this._backgroundWorkerErase.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerErase_ProgressChanged);
            // 
            // ISOBuilderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 361);
            this.Controls.Add(this._tabBuild);
            this.Name = "ISOBuilderForm";
            this.Text = "Build/Burn ISO files";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ISOBuilderForm_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ISOBuilderForm_FormClosing);
            this._tabBuild.ResumeLayout(false);
            this._tabSelectFiles.ResumeLayout(false);
            this._tabSelectFiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this._tabPageBuild.ResumeLayout(false);
            this._tabPageBuild.PerformLayout();
            this._tabFormat.ResumeLayout(false);
            this._TabPageFile.ResumeLayout(false);
            this._TabPageFile.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this._tabPageBurn.ResumeLayout(false);
            this._tabPageBurn.PerformLayout();
            this._tabPageformat.ResumeLayout(false);
            this._tabPageformat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker _backgroundISOWorker;
        private System.Windows.Forms.TabControl _tabBuild;
        private System.Windows.Forms.TabPage _tabSelectFiles;
        private System.Windows.Forms.DataGridView _dataGridView;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.TabPage _tabPageBuild;
        private System.Windows.Forms.Button _btnBuildFile;
        private System.Windows.Forms.Label _lblUpdate;
        private System.Windows.Forms.ProgressBar _progBar;
        private System.Windows.Forms.Button _SaveAs;
        private System.Windows.Forms.BindingSource _bindingSource;
        private System.Windows.Forms.OpenFileDialog _openFileDialog;
        private System.Windows.Forms.Button _btnAddFile;
        private System.Windows.Forms.Button _btnAddFolder;
        private System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog;
        private System.Windows.Forms.SaveFileDialog _saveFileDialog;
        private System.Windows.Forms.CheckBox _ckbISO9660;
        private System.Windows.Forms.CheckBox _ckbJoliet;
        private System.Windows.Forms.CheckBox _ckbUDF;
        private System.Windows.Forms.TextBox _txtVolName;
        private System.Windows.Forms.Label _lblDest;
        private System.Windows.Forms.ComboBox _cbxMediaTypes;
        private System.Windows.Forms.CheckBox _ckWorker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox _ckUseUIReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn fullNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.Button _btnAbout;
        private System.Windows.Forms.Label _lblDst;
        private System.Windows.Forms.ComboBox _cbxBurners;
        private System.Windows.Forms.Label _lblBurner;
        private System.Windows.Forms.ComboBox _cbxSpeed;
        private System.Windows.Forms.Label _lblSpeed;
        private System.Windows.Forms.Label _lblMediaType;
        private System.Windows.Forms.TabControl _tabFormat;
        private System.Windows.Forms.TabPage _TabPageFile;
        private System.Windows.Forms.TabPage _tabPageBurn;
        private System.Windows.Forms.CheckBox _ckMakeBootable;
        private System.Windows.Forms.Label _lblCrtDiskType;
        private System.Windows.Forms.TextBox _txtCrtDiskType;
        private System.Windows.Forms.CheckBox _ckbEject;
        private System.Windows.Forms.Button _btnSelectImage;
        private System.Windows.Forms.Label _lblFileImage;
        private System.Windows.Forms.CheckBox _ckbCloseMedia;
        private System.Windows.Forms.Button _btnBurn;
        private System.Windows.Forms.TabPage _tabPageformat;
        private System.Windows.Forms.Button _btnFormat;
        private System.Windows.Forms.CheckBox _ckbEjectFormat;
        private System.Windows.Forms.TextBox _txtDiskTypeFormat;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox _cbxFormatBurners;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.ComponentModel.BackgroundWorker _backgroundWorkerBurn;
        private System.ComponentModel.BackgroundWorker _backgroundWorkerErase;
        private System.Windows.Forms.CheckBox _ckQuick;
        private System.Windows.Forms.CheckBox _ckbRepair;
        private System.Windows.Forms.ComboBox _cbxAfterCompletion;
        private System.Windows.Forms.CheckBox _ckbForceTermination;
    }
}

