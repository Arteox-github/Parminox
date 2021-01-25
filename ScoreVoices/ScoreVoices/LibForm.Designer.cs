namespace Parminox
{
    partial class LibraryForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibraryForm));
            this.LBoxPanel = new System.Windows.Forms.Panel();
            this.SearchPanel = new System.Windows.Forms.Panel();
            this.SearchLabel = new System.Windows.Forms.Label();
            this.hlpButton = new System.Windows.Forms.Button();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.dGrid = new System.Windows.Forms.DataGridView();
            this.col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LibCancelButton = new System.Windows.Forms.Button();
            this.btCreate = new System.Windows.Forms.Button();
            this.PopupLibrary = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PopupLibrarySelect = new System.Windows.Forms.ToolStripMenuItem();
            this.PopupLibraryInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.PopupLibraryEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.PopupLibraryCreateNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.PopupLibraryDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.PopupInstruments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PopupInstrumentAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.PopupInstrumentRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.PopupInstrumentEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.PopupInstrumentSave = new System.Windows.Forms.ToolStripMenuItem();
            this.LBoxPanel.SuspendLayout();
            this.SearchPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGrid)).BeginInit();
            this.PopupLibrary.SuspendLayout();
            this.PopupInstruments.SuspendLayout();
            this.SuspendLayout();
            // 
            // LBoxPanel
            // 
            this.LBoxPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.LBoxPanel.Controls.Add(this.SearchPanel);
            this.LBoxPanel.Controls.Add(this.dGrid);
            this.LBoxPanel.Location = new System.Drawing.Point(1, 1);
            this.LBoxPanel.Margin = new System.Windows.Forms.Padding(4);
            this.LBoxPanel.Name = "LBoxPanel";
            this.LBoxPanel.Size = new System.Drawing.Size(990, 563);
            this.LBoxPanel.TabIndex = 0;
            // 
            // SearchPanel
            // 
            this.SearchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SearchPanel.Controls.Add(this.SearchLabel);
            this.SearchPanel.Controls.Add(this.hlpButton);
            this.SearchPanel.Controls.Add(this.tbSearch);
            this.SearchPanel.Location = new System.Drawing.Point(3, 4);
            this.SearchPanel.Name = "SearchPanel";
            this.SearchPanel.Size = new System.Drawing.Size(980, 37);
            this.SearchPanel.TabIndex = 1;
            // 
            // SearchLabel
            // 
            this.SearchLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchLabel.AutoSize = true;
            this.SearchLabel.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SearchLabel.Location = new System.Drawing.Point(14, 8);
            this.SearchLabel.Name = "SearchLabel";
            this.SearchLabel.Size = new System.Drawing.Size(54, 17);
            this.SearchLabel.TabIndex = 1;
            this.SearchLabel.Text = "Поиск:";
            // 
            // hlpButton
            // 
            this.hlpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.hlpButton.Image = global::Parminox.Properties.Resources.Info;
            this.hlpButton.Location = new System.Drawing.Point(944, 1);
            this.hlpButton.Name = "hlpButton";
            this.hlpButton.Size = new System.Drawing.Size(31, 31);
            this.hlpButton.TabIndex = 3;
            this.hlpButton.UseVisualStyleBackColor = true;
            this.hlpButton.Click += new System.EventHandler(this.hlpButton_Click);
            // 
            // tbSearch
            // 
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.Location = new System.Drawing.Point(73, 4);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(856, 25);
            this.tbSearch.TabIndex = 0;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            this.tbSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSearch_KeyPress);
            // 
            // dGrid
            // 
            this.dGrid.AllowUserToResizeColumns = false;
            this.dGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.NullValue = null;
            this.dGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SunkenHorizontal;
            this.dGrid.ColumnHeadersHeight = 24;
            this.dGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col1});
            this.dGrid.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.dGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dGrid.Location = new System.Drawing.Point(3, 45);
            this.dGrid.Margin = new System.Windows.Forms.Padding(4);
            this.dGrid.MultiSelect = false;
            this.dGrid.Name = "dGrid";
            this.dGrid.RowHeadersWidth = 24;
            this.dGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dGrid.Size = new System.Drawing.Size(980, 510);
            this.dGrid.TabIndex = 0;
            this.dGrid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dGrid_CellBeginEdit);
            this.dGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGrid_CellContentClick);
            this.dGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGrid_CellDoubleClick);
            this.dGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGrid_CellEndEdit);
            this.dGrid.Enter += new System.EventHandler(this.dGrid_Enter);
            this.dGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dGrid_KeyDown);
            this.dGrid.Leave += new System.EventHandler(this.dGrid_Leave);
            this.dGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dGrid_MouseDown);
            // 
            // col1
            // 
            this.col1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.col1.HeaderText = "Название";
            this.col1.MinimumWidth = 40;
            this.col1.Name = "col1";
            // 
            // LibCancelButton
            // 
            this.LibCancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LibCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.LibCancelButton.Location = new System.Drawing.Point(850, 571);
            this.LibCancelButton.Margin = new System.Windows.Forms.Padding(4);
            this.LibCancelButton.Name = "LibCancelButton";
            this.LibCancelButton.Size = new System.Drawing.Size(128, 32);
            this.LibCancelButton.TabIndex = 1;
            this.LibCancelButton.Text = "Закрыть";
            this.LibCancelButton.UseVisualStyleBackColor = true;
            // 
            // btCreate
            // 
            this.btCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btCreate.Location = new System.Drawing.Point(13, 571);
            this.btCreate.Margin = new System.Windows.Forms.Padding(4);
            this.btCreate.Name = "btCreate";
            this.btCreate.Size = new System.Drawing.Size(128, 32);
            this.btCreate.TabIndex = 2;
            this.btCreate.Text = "Создать";
            this.btCreate.UseVisualStyleBackColor = true;
            this.btCreate.Click += new System.EventHandler(this.btCreate_Click);
            // 
            // PopupLibrary
            // 
            this.PopupLibrary.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PopupLibrarySelect,
            this.PopupLibraryInfo,
            this.PopupLibraryEdit,
            this.PopupLibraryCreateNew,
            this.toolStripSeparator2,
            this.PopupLibraryDelete});
            this.PopupLibrary.Name = "PopupLibrary";
            this.PopupLibrary.Size = new System.Drawing.Size(320, 120);
            // 
            // PopupLibrarySelect
            // 
            this.PopupLibrarySelect.Name = "PopupLibrarySelect";
            this.PopupLibrarySelect.Size = new System.Drawing.Size(319, 22);
            this.PopupLibrarySelect.Text = "&Добавить в проект концерта    (Enter)";
            this.PopupLibrarySelect.Click += new System.EventHandler(this.PopupLibrarySelect_Click);
            // 
            // PopupLibraryInfo
            // 
            this.PopupLibraryInfo.Name = "PopupLibraryInfo";
            this.PopupLibraryInfo.Size = new System.Drawing.Size(319, 22);
            this.PopupLibraryInfo.Text = "&Информация о произведении  (I)";
            this.PopupLibraryInfo.Click += new System.EventHandler(this.PopupLibraryInfo_Click);
            // 
            // PopupLibraryEdit
            // 
            this.PopupLibraryEdit.Name = "PopupLibraryEdit";
            this.PopupLibraryEdit.Size = new System.Drawing.Size(319, 22);
            this.PopupLibraryEdit.Text = "&Редактировать    (F2)";
            this.PopupLibraryEdit.Click += new System.EventHandler(this.PopupLibraryEdit_Click);
            // 
            // PopupLibraryCreateNew
            // 
            this.PopupLibraryCreateNew.Name = "PopupLibraryCreateNew";
            this.PopupLibraryCreateNew.Size = new System.Drawing.Size(319, 22);
            this.PopupLibraryCreateNew.Text = "Создать &новое";
            this.PopupLibraryCreateNew.Click += new System.EventHandler(this.PopupLibraryCreateNew_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(316, 6);
            // 
            // PopupLibraryDelete
            // 
            this.PopupLibraryDelete.Name = "PopupLibraryDelete";
            this.PopupLibraryDelete.Size = new System.Drawing.Size(319, 22);
            this.PopupLibraryDelete.Text = "&Удалить произведение из библиотеки    (Del)";
            this.PopupLibraryDelete.Click += new System.EventHandler(this.PopuLibraryDelete_Click);
            // 
            // PopupInstruments
            // 
            this.PopupInstruments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PopupInstrumentAdd,
            this.PopupInstrumentRemove,
            this.PopupInstrumentEdit,
            this.toolStripSeparator1,
            this.PopupInstrumentSave});
            this.PopupInstruments.Name = "PopupInstruments";
            this.PopupInstruments.Size = new System.Drawing.Size(244, 98);
            // 
            // PopupInstrumentAdd
            // 
            this.PopupInstrumentAdd.Name = "PopupInstrumentAdd";
            this.PopupInstrumentAdd.Size = new System.Drawing.Size(243, 22);
            this.PopupInstrumentAdd.Text = "&Добавить инструмент    (Insert)";
            this.PopupInstrumentAdd.Click += new System.EventHandler(this.PopupInstrumentAdd_Click);
            // 
            // PopupInstrumentRemove
            // 
            this.PopupInstrumentRemove.Name = "PopupInstrumentRemove";
            this.PopupInstrumentRemove.Size = new System.Drawing.Size(243, 22);
            this.PopupInstrumentRemove.Text = "&Удалить инструмент    (Del)";
            this.PopupInstrumentRemove.Click += new System.EventHandler(this.PopupInstrumentRemove_Click);
            // 
            // PopupInstrumentEdit
            // 
            this.PopupInstrumentEdit.Name = "PopupInstrumentEdit";
            this.PopupInstrumentEdit.Size = new System.Drawing.Size(243, 22);
            this.PopupInstrumentEdit.Text = "&Редактировать    (Enter)";
            this.PopupInstrumentEdit.Click += new System.EventHandler(this.PopupInstrumentEdit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(240, 6);
            // 
            // PopupInstrumentSave
            // 
            this.PopupInstrumentSave.Name = "PopupInstrumentSave";
            this.PopupInstrumentSave.Size = new System.Drawing.Size(243, 22);
            this.PopupInstrumentSave.Text = "&Сохранить список";
            this.PopupInstrumentSave.Click += new System.EventHandler(this.PopupInstrumentSave_Click);
            // 
            // LibraryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(994, 610);
            this.Controls.Add(this.LibCancelButton);
            this.Controls.Add(this.LBoxPanel);
            this.Controls.Add(this.btCreate);
            this.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(40, 50);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(828, 446);
            this.Name = "LibraryForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Библиотека произведений";
            this.Activated += new System.EventHandler(this.LibraryForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LibraryForm_FormClosing);
            this.Shown += new System.EventHandler(this.LibraryForm_Shown);
            this.LBoxPanel.ResumeLayout(false);
            this.SearchPanel.ResumeLayout(false);
            this.SearchPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGrid)).EndInit();
            this.PopupLibrary.ResumeLayout(false);
            this.PopupInstruments.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel LBoxPanel;
        private System.Windows.Forms.Button LibCancelButton;
        private System.Windows.Forms.DataGridView dGrid;
        private System.Windows.Forms.Button btCreate;
        private System.Windows.Forms.DataGridViewTextBoxColumn col1;
        private System.Windows.Forms.Button hlpButton;
        private System.Windows.Forms.Panel SearchPanel;
        private System.Windows.Forms.Label SearchLabel;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.ContextMenuStrip PopupLibrary;
        private System.Windows.Forms.ToolStripMenuItem PopupLibrarySelect;
        private System.Windows.Forms.ToolStripMenuItem PopupLibraryEdit;
        private System.Windows.Forms.ContextMenuStrip PopupInstruments;
        private System.Windows.Forms.ToolStripMenuItem PopupInstrumentAdd;
        private System.Windows.Forms.ToolStripMenuItem PopupInstrumentRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem PopupInstrumentSave;
        private System.Windows.Forms.ToolStripMenuItem PopupLibraryDelete;
        private System.Windows.Forms.ToolStripMenuItem PopupInstrumentEdit;
        private System.Windows.Forms.ToolStripMenuItem PopupLibraryCreateNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem PopupLibraryInfo;
    }
}