namespace Parminox
{
    partial class WorksForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorksForm));
            this.btClose = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.tbHeaderTitle = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTitle = new System.Windows.Forms.TextBox();
            this.wdGrid = new System.Windows.Forms.DataGridView();
            this.btClear = new System.Windows.Forms.Button();
            this.wfPictureBox = new System.Windows.Forms.PictureBox();
            this.Col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.wdGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wfPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // btClose
            // 
            this.btClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btClose.Location = new System.Drawing.Point(385, 597);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(128, 32);
            this.btClose.TabIndex = 6;
            this.btClose.Text = "Закрыть";
            this.btClose.UseVisualStyleBackColor = true;
            // 
            // btSave
            // 
            this.btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSave.Enabled = false;
            this.btSave.Location = new System.Drawing.Point(251, 597);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(128, 32);
            this.btSave.TabIndex = 5;
            this.btSave.Text = "Сохранить";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // tbHeaderTitle
            // 
            this.tbHeaderTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbHeaderTitle.Font = new System.Drawing.Font("Times New Roman", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbHeaderTitle.Location = new System.Drawing.Point(7, 100);
            this.tbHeaderTitle.Margin = new System.Windows.Forms.Padding(4);
            this.tbHeaderTitle.Name = "tbHeaderTitle";
            this.tbHeaderTitle.Size = new System.Drawing.Size(407, 25);
            this.tbHeaderTitle.TabIndex = 3;
            this.tbHeaderTitle.TextChanged += new System.EventHandler(this.tbHeaderTitle_TextChanged);
            this.tbHeaderTitle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTitle_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            this.label2.Location = new System.Drawing.Point(13, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Название для заголовка:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Полное название:";
            // 
            // tbTitle
            // 
            this.tbTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTitle.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbTitle.Location = new System.Drawing.Point(7, 30);
            this.tbTitle.Margin = new System.Windows.Forms.Padding(4);
            this.tbTitle.Name = "tbTitle";
            this.tbTitle.Size = new System.Drawing.Size(503, 25);
            this.tbTitle.TabIndex = 0;
            this.tbTitle.TextChanged += new System.EventHandler(this.tbTitle_TextChanged);
            this.tbTitle.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbTitle_KeyPress);
            // 
            // wdGrid
            // 
            this.wdGrid.AllowUserToAddRows = false;
            this.wdGrid.AllowUserToDeleteRows = false;
            this.wdGrid.AllowUserToResizeColumns = false;
            this.wdGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            this.wdGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.wdGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.wdGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.wdGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col1,
            this.Col2});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.wdGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.wdGrid.Dock = System.Windows.Forms.DockStyle.Right;
            this.wdGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.wdGrid.Location = new System.Drawing.Point(527, 0);
            this.wdGrid.MultiSelect = false;
            this.wdGrid.Name = "wdGrid";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.wdGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.wdGrid.RowHeadersWidth = 24;
            this.wdGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            this.wdGrid.RowsDefaultCellStyle = dataGridViewCellStyle7;
            this.wdGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.wdGrid.Size = new System.Drawing.Size(305, 641);
            this.wdGrid.TabIndex = 0;
            this.wdGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.wdGrid_CellEnter);
            this.wdGrid.CellStateChanged += new System.Windows.Forms.DataGridViewCellStateChangedEventHandler(this.wdGrid_CellStateChanged);
            this.wdGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.wdGrid_CellValueChanged);
            this.wdGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.wdGrid_EditingControlShowing);
            this.wdGrid.Leave += new System.EventHandler(this.wdGrid_Leave);
            // 
            // btClear
            // 
            this.btClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btClear.Image = global::Parminox.Properties.Resources.Clear;
            this.btClear.Location = new System.Drawing.Point(481, 559);
            this.btClear.Name = "btClear";
            this.btClear.Size = new System.Drawing.Size(32, 32);
            this.btClear.TabIndex = 8;
            this.btClear.UseVisualStyleBackColor = true;
            this.btClear.Click += new System.EventHandler(this.btClear_Click);
            // 
            // wfPictureBox
            // 
            this.wfPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.wfPictureBox.BackColor = System.Drawing.SystemColors.Window;
            this.wfPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.wfPictureBox.Location = new System.Drawing.Point(435, 100);
            this.wfPictureBox.Name = "wfPictureBox";
            this.wfPictureBox.Size = new System.Drawing.Size(30, 75);
            this.wfPictureBox.TabIndex = 4;
            this.wfPictureBox.TabStop = false;
            // 
            // Col1
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Times New Roman", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            this.Col1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Col1.HeaderText = "Инструмент";
            this.Col1.Name = "Col1";
            this.Col1.ReadOnly = true;
            this.Col1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Col1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Col1.Width = 172;
            // 
            // Col2
            // 
            this.Col2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.DarkSalmon;
            this.Col2.DefaultCellStyle = dataGridViewCellStyle4;
            this.Col2.HeaderText = "..";
            this.Col2.Name = "Col2";
            this.Col2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // WorksForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(832, 641);
            this.Controls.Add(this.btClear);
            this.Controls.Add(this.wfPictureBox);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbHeaderTitle);
            this.Controls.Add(this.wdGrid);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTitle);
            this.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(40, 50);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(768, 389);
            this.Name = "WorksForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Произведение";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorksForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.wdGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wfPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox wfPictureBox;
        private System.Windows.Forms.TextBox tbHeaderTitle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTitle;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.DataGridView wdGrid;
        private System.Windows.Forms.Button btClear;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col2;
    }
}