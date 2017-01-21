namespace ISOBuilder
{
    partial class ISOBuilder
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
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ISOBuilder));
            System.Windows.Forms.Label label3;
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this._tabBuild = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this._btnAddFile = new System.Windows.Forms.Button();
            this._btnAddFolder = new System.Windows.Forms.Button();
            this._dataGridView = new System.Windows.Forms.DataGridView();
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
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this._txtVolName = new System.Windows.Forms.TextBox();
            this._ckbUDF = new System.Windows.Forms.CheckBox();
            this._ckbJoliet = new System.Windows.Forms.CheckBox();
            this._ckbISO9660 = new System.Windows.Forms.CheckBox();
            this._lblDest = new System.Windows.Forms.Label();
            this._SaveAs = new System.Windows.Forms.Button();
            this._btnBuildFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this._openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this._folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this._saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this._cbxMediaType = new System.Windows.Forms.ComboBox();
            this.itemtypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.locationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._bindingSource = new System.Windows.Forms.BindingSource(this.components);
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            this._tabBuild.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(249, 108);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(63, 13);
            label2.TabIndex = 6;
            label2.Text = "Media Type";
            // 
            // _tabBuild
            // 
            this._tabBuild.Controls.Add(this.tabPage1);
            this._tabBuild.Controls.Add(this.tabPage2);
            this._tabBuild.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tabBuild.Location = new System.Drawing.Point(0, 0);
            this._tabBuild.Name = "_tabBuild";
            this._tabBuild.SelectedIndex = 0;
            this._tabBuild.Size = new System.Drawing.Size(583, 266);
            this._tabBuild.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this._btnAddFile);
            this.tabPage1.Controls.Add(this._btnAddFolder);
            this.tabPage1.Controls.Add(this._dataGridView);
            this.tabPage1.Controls.Add(this.bindingNavigator1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(575, 240);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Select Files";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // _btnAddFile
            // 
            this._btnAddFile.Location = new System.Drawing.Point(150, 194);
            this._btnAddFile.Name = "_btnAddFile";
            this._btnAddFile.Size = new System.Drawing.Size(75, 23);
            this._btnAddFile.TabIndex = 2;
            this._btnAddFile.Text = "Add File";
            this._btnAddFile.UseVisualStyleBackColor = true;
            this._btnAddFile.Click += new System.EventHandler(this._btnAddFile_Click);
            // 
            // _btnAddFolder
            // 
            this._btnAddFolder.Location = new System.Drawing.Point(19, 194);
            this._btnAddFolder.Name = "_btnAddFolder";
            this._btnAddFolder.Size = new System.Drawing.Size(75, 23);
            this._btnAddFolder.TabIndex = 2;
            this._btnAddFolder.Text = "Add Folder";
            this._btnAddFolder.UseVisualStyleBackColor = true;
            this._btnAddFolder.Click += new System.EventHandler(this._btnAddFolder_Click);
            // 
            // _dataGridView
            // 
            this._dataGridView.AllowUserToDeleteRows = false;
            this._dataGridView.AutoGenerateColumns = false;
            this._dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemtypeDataGridViewTextBoxColumn,
            this.locationDataGridViewTextBoxColumn});
            this._dataGridView.DataSource = this._bindingSource;
            this._dataGridView.Dock = System.Windows.Forms.DockStyle.Top;
            this._dataGridView.Location = new System.Drawing.Point(3, 28);
            this._dataGridView.Name = "_dataGridView";
            this._dataGridView.ReadOnly = true;
            this._dataGridView.Size = new System.Drawing.Size(569, 150);
            this._dataGridView.TabIndex = 1;
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
            this.bindingNavigator1.Size = new System.Drawing.Size(569, 25);
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
            this.bindingNavigatorAddNewItem.Click += new System.EventHandler(this.bindingNavigatorAddNewItem_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this._cbxMediaType);
            this.tabPage2.Controls.Add(this._txtVolName);
            this.tabPage2.Controls.Add(this._ckbUDF);
            this.tabPage2.Controls.Add(this._ckbJoliet);
            this.tabPage2.Controls.Add(this._ckbISO9660);
            this.tabPage2.Controls.Add(this._lblDest);
            this.tabPage2.Controls.Add(this._SaveAs);
            this.tabPage2.Controls.Add(this._btnBuildFile);
            this.tabPage2.Controls.Add(label3);
            this.tabPage2.Controls.Add(label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.progressBar1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(575, 240);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Build File";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // _txtVolName
            // 
            this._txtVolName.AcceptsReturn = true;
            this._txtVolName.Location = new System.Drawing.Point(96, 101);
            this._txtVolName.Name = "_txtVolName";
            this._txtVolName.Size = new System.Drawing.Size(100, 20);
            this._txtVolName.TabIndex = 11;
            // 
            // _ckbUDF
            // 
            this._ckbUDF.AutoSize = true;
            this._ckbUDF.Location = new System.Drawing.Point(369, 26);
            this._ckbUDF.Name = "_ckbUDF";
            this._ckbUDF.Size = new System.Drawing.Size(48, 17);
            this._ckbUDF.TabIndex = 10;
            this._ckbUDF.Text = "UDF";
            this._ckbUDF.UseVisualStyleBackColor = true;
            // 
            // _ckbJoliet
            // 
            this._ckbJoliet.AutoSize = true;
            this._ckbJoliet.Checked = true;
            this._ckbJoliet.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ckbJoliet.Location = new System.Drawing.Point(272, 28);
            this._ckbJoliet.Name = "_ckbJoliet";
            this._ckbJoliet.Size = new System.Drawing.Size(50, 17);
            this._ckbJoliet.TabIndex = 10;
            this._ckbJoliet.Text = "Joliet";
            this._ckbJoliet.UseVisualStyleBackColor = true;
            // 
            // _ckbISO9660
            // 
            this._ckbISO9660.AutoSize = true;
            this._ckbISO9660.Checked = true;
            this._ckbISO9660.CheckState = System.Windows.Forms.CheckState.Checked;
            this._ckbISO9660.Location = new System.Drawing.Point(142, 26);
            this._ckbISO9660.Name = "_ckbISO9660";
            this._ckbISO9660.Size = new System.Drawing.Size(68, 17);
            this._ckbISO9660.TabIndex = 10;
            this._ckbISO9660.Text = "ISO9660";
            this._ckbISO9660.UseVisualStyleBackColor = true;
            // 
            // _lblDest
            // 
            this._lblDest.AutoSize = true;
            this._lblDest.Location = new System.Drawing.Point(8, 66);
            this._lblDest.Name = "_lblDest";
            this._lblDest.Size = new System.Drawing.Size(0, 13);
            this._lblDest.TabIndex = 9;
            // 
            // _SaveAs
            // 
            this._SaveAs.Location = new System.Drawing.Point(8, 22);
            this._SaveAs.Name = "_SaveAs";
            this._SaveAs.Size = new System.Drawing.Size(75, 23);
            this._SaveAs.TabIndex = 8;
            this._SaveAs.Text = "Save As";
            this._SaveAs.UseVisualStyleBackColor = true;
            this._SaveAs.Click += new System.EventHandler(this._SaveAs_Click);
            // 
            // _btnBuildFile
            // 
            this._btnBuildFile.Location = new System.Drawing.Point(476, 24);
            this._btnBuildFile.Name = "_btnBuildFile";
            this._btnBuildFile.Size = new System.Drawing.Size(75, 23);
            this._btnBuildFile.TabIndex = 7;
            this._btnBuildFile.Text = "Build file";
            this._btnBuildFile.UseVisualStyleBackColor = true;
            this._btnBuildFile.Click += new System.EventHandler(this._btnBuildFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "label1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(8, 168);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(543, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // _openFileDialog
            // 
            this._openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this._openFileDialog_FileOk);
            // 
            // _cbxMediaType
            // 
            this._cbxMediaType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cbxMediaType.FormattingEnabled = true;
            this._cbxMediaType.Location = new System.Drawing.Point(332, 108);
            this._cbxMediaType.Name = "_cbxMediaType";
            this._cbxMediaType.Size = new System.Drawing.Size(219, 21);
            this._cbxMediaType.TabIndex = 12;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(5, 108);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(73, 13);
            label3.TabIndex = 6;
            label3.Text = "Volume Name";
            // 
            // itemtypeDataGridViewTextBoxColumn
            // 
            this.itemtypeDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemtypeDataGridViewTextBoxColumn.DataPropertyName = "Itemtype";
            this.itemtypeDataGridViewTextBoxColumn.FillWeight = 101.5229F;
            this.itemtypeDataGridViewTextBoxColumn.HeaderText = "Itemtype";
            this.itemtypeDataGridViewTextBoxColumn.Name = "itemtypeDataGridViewTextBoxColumn";
            this.itemtypeDataGridViewTextBoxColumn.ReadOnly = true;
            this.itemtypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // locationDataGridViewTextBoxColumn
            // 
            this.locationDataGridViewTextBoxColumn.DataPropertyName = "Location";
            this.locationDataGridViewTextBoxColumn.FillWeight = 98.4772F;
            this.locationDataGridViewTextBoxColumn.HeaderText = "Location";
            this.locationDataGridViewTextBoxColumn.Name = "locationDataGridViewTextBoxColumn";
            this.locationDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // _bindingSource
            // 
            this._bindingSource.AllowNew = true;
            this._bindingSource.DataSource = typeof(Helper.CustomList);
            this._bindingSource.DataError += new System.Windows.Forms.BindingManagerDataErrorEventHandler(this._bindingSource_DataError);
            // 
            // ISOBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 266);
            this.Controls.Add(this._tabBuild);
            this.Name = "ISOBuilder";
            this.Text = "Build ISO files";
            this.Load += new System.EventHandler(this.Form1_Load);
            this._tabBuild.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl _tabBuild;
        private System.Windows.Forms.TabPage tabPage1;
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
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button _btnBuildFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn itemtypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn locationDataGridViewTextBoxColumn;
        private System.Windows.Forms.ComboBox _cbxMediaType;
    }
}

