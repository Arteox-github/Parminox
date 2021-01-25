namespace Parminox
{
    partial class MF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MF));
            this.MFPanel = new System.Windows.Forms.Panel();
            this.MFPictureBox = new System.Windows.Forms.PictureBox();
            this.PopupMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PopupAddWork = new System.Windows.Forms.ToolStripMenuItem();
            this.smAddEmptyColumn = new System.Windows.Forms.ToolStripMenuItem();
            this.PopupDeleteWork = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.PopupEditInstrumentList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.PopupPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.PopupNavigationHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.PopupAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.PopupExit = new System.Windows.Forms.ToolStripMenuItem();
            this.MFStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MFStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.MFStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.printDialog = new System.Windows.Forms.PrintDialog();
            this.printDocument = new System.Drawing.Printing.PrintDocument();
            this.MyPrintPreviewDialog = new System.Windows.Forms.PrintPreviewDialog();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemoveLast = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Button();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.headerTextBox = new System.Windows.Forms.TextBox();
            this.cbDrop = new System.Windows.Forms.ComboBox();
            this.lbTopField = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.MFPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MFPictureBox)).BeginInit();
            this.PopupMenu.SuspendLayout();
            this.MFStatusStrip.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MFPanel
            // 
            this.MFPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MFPanel.AutoScroll = true;
            this.MFPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MFPanel.Controls.Add(this.MFPictureBox);
            this.MFPanel.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MFPanel.Location = new System.Drawing.Point(0, 49);
            this.MFPanel.Name = "MFPanel";
            this.MFPanel.Size = new System.Drawing.Size(811, 561);
            this.MFPanel.TabIndex = 2;
            // 
            // MFPictureBox
            // 
            this.MFPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MFPictureBox.ContextMenuStrip = this.PopupMenu;
            this.MFPictureBox.ErrorImage = null;
            this.MFPictureBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.MFPictureBox.InitialImage = null;
            this.MFPictureBox.Location = new System.Drawing.Point(24, 33);
            this.MFPictureBox.Name = "MFPictureBox";
            this.MFPictureBox.Size = new System.Drawing.Size(276, 130);
            this.MFPictureBox.TabIndex = 0;
            this.MFPictureBox.TabStop = false;
            this.MFPictureBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.MFPictureBox_MouseDoubleClick);
            this.MFPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MFPictureBox_MouseDown);
            this.MFPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MFPictureBox_MouseMove);
            this.MFPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MFPictureBox_MouseUp);
            // 
            // PopupMenu
            // 
            this.PopupMenu.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PopupMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PopupAddWork,
            this.smAddEmptyColumn,
            this.PopupDeleteWork,
            this.toolStripSeparator1,
            this.PopupEditInstrumentList,
            this.toolStripSeparator2,
            this.PopupPrint,
            this.toolStripSeparator3,
            this.PopupNavigationHelp,
            this.PopupAbout,
            this.toolStripSeparator4,
            this.PopupExit});
            this.PopupMenu.Name = "PopupMenu";
            this.PopupMenu.Size = new System.Drawing.Size(370, 204);
            this.PopupMenu.Opening += new System.ComponentModel.CancelEventHandler(this.PopupMenu_Opening);
            // 
            // PopupAddWork
            // 
            this.PopupAddWork.Name = "PopupAddWork";
            this.PopupAddWork.Size = new System.Drawing.Size(369, 22);
            this.PopupAddWork.Text = "&Добавить произведение в проект концерта (F2)";
            this.PopupAddWork.Click += new System.EventHandler(this.PopupAddWork_Click);
            // 
            // smAddEmptyColumn
            // 
            this.smAddEmptyColumn.Name = "smAddEmptyColumn";
            this.smAddEmptyColumn.Size = new System.Drawing.Size(369, 22);
            this.smAddEmptyColumn.Text = "Добавить пустую &колонку с текстом";
            this.smAddEmptyColumn.Click += new System.EventHandler(this.smAddEmptyColumn_Click);
            // 
            // PopupDeleteWork
            // 
            this.PopupDeleteWork.Name = "PopupDeleteWork";
            this.PopupDeleteWork.Size = new System.Drawing.Size(369, 22);
            this.PopupDeleteWork.Text = "&Удалить колонку из проекта концерта (Del)";
            this.PopupDeleteWork.Click += new System.EventHandler(this.PopupDeleteWork_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(366, 6);
            // 
            // PopupEditInstrumentList
            // 
            this.PopupEditInstrumentList.Name = "PopupEditInstrumentList";
            this.PopupEditInstrumentList.Size = new System.Drawing.Size(369, 22);
            this.PopupEditInstrumentList.Text = "Редактировать список &инструментов (F8)";
            this.PopupEditInstrumentList.Click += new System.EventHandler(this.PopupEditInstrumentList_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(366, 6);
            // 
            // PopupPrint
            // 
            this.PopupPrint.Name = "PopupPrint";
            this.PopupPrint.Size = new System.Drawing.Size(369, 22);
            this.PopupPrint.Text = "&Печать (F12)";
            this.PopupPrint.Click += new System.EventHandler(this.PopupPrint_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(366, 6);
            // 
            // PopupNavigationHelp
            // 
            this.PopupNavigationHelp.Name = "PopupNavigationHelp";
            this.PopupNavigationHelp.Size = new System.Drawing.Size(369, 22);
            this.PopupNavigationHelp.Text = "&Справка (F1)";
            this.PopupNavigationHelp.Click += new System.EventHandler(this.PopupNavigationHelp_Click);
            // 
            // PopupAbout
            // 
            this.PopupAbout.Name = "PopupAbout";
            this.PopupAbout.Size = new System.Drawing.Size(369, 22);
            this.PopupAbout.Text = "&О программе";
            this.PopupAbout.Click += new System.EventHandler(this.PopupAbout_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(366, 6);
            // 
            // PopupExit
            // 
            this.PopupExit.Name = "PopupExit";
            this.PopupExit.Size = new System.Drawing.Size(369, 22);
            this.PopupExit.Text = "&Закрыть программу";
            this.PopupExit.Click += new System.EventHandler(this.PopupExit_Click);
            // 
            // MFStatusStrip
            // 
            this.MFStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.MFStatusStrip.Location = new System.Drawing.Point(0, 611);
            this.MFStatusStrip.Name = "MFStatusStrip";
            this.MFStatusStrip.Size = new System.Drawing.Size(811, 26);
            this.MFStatusStrip.TabIndex = 4;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.toolStripStatusLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(29, 21);
            this.toolStripStatusLabel1.Text = "ts1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("Times New Roman", 11F);
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(25, 21);
            this.toolStripStatusLabel2.Text = "ts2";
            // 
            // MFStripStatusLabel1
            // 
            this.MFStripStatusLabel1.Name = "MFStripStatusLabel1";
            this.MFStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // MFStripStatusLabel2
            // 
            this.MFStripStatusLabel2.Name = "MFStripStatusLabel2";
            this.MFStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // printDialog
            // 
            this.printDialog.AllowCurrentPage = true;
            this.printDialog.Document = this.printDocument;
            this.printDialog.ShowNetwork = false;
            // 
            // printDocument
            // 
            this.printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.PrintPageHandler);
            // 
            // MyPrintPreviewDialog
            // 
            this.MyPrintPreviewDialog.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.MyPrintPreviewDialog.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.MyPrintPreviewDialog.ClientSize = new System.Drawing.Size(400, 300);
            this.MyPrintPreviewDialog.Document = this.printDocument;
            this.MyPrintPreviewDialog.Enabled = true;
            this.MyPrintPreviewDialog.Icon = ((System.Drawing.Icon)(resources.GetObject("MyPrintPreviewDialog.Icon")));
            this.MyPrintPreviewDialog.Name = "printPreviewDialog";
            this.MyPrintPreviewDialog.ShowIcon = false;
            this.MyPrintPreviewDialog.UseAntiAlias = true;
            this.MyPrintPreviewDialog.Visible = false;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Image = global::Parminox.Properties.Resources.Add;
            this.btnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdd.Location = new System.Drawing.Point(645, 9);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(32, 32);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.TabStop = false;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemoveLast
            // 
            this.btnRemoveLast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveLast.Image = global::Parminox.Properties.Resources.Delete;
            this.btnRemoveLast.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRemoveLast.Location = new System.Drawing.Point(681, 9);
            this.btnRemoveLast.Name = "btnRemoveLast";
            this.btnRemoveLast.Size = new System.Drawing.Size(32, 32);
            this.btnRemoveLast.TabIndex = 3;
            this.btnRemoveLast.TabStop = false;
            this.btnRemoveLast.UseVisualStyleBackColor = true;
            this.btnRemoveLast.Click += new System.EventHandler(this.btnRemoveLast_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Image = global::Parminox.Properties.Resources.Print;
            this.btnPrint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrint.Location = new System.Drawing.Point(734, 9);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(32, 32);
            this.btnPrint.TabIndex = 5;
            this.btnPrint.TabStop = false;
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInfo.Image = global::Parminox.Properties.Resources.Info;
            this.btnInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnInfo.Location = new System.Drawing.Point(770, 9);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(32, 32);
            this.btnInfo.TabIndex = 7;
            this.btnInfo.TabStop = false;
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePicker.DropDownAlign = System.Windows.Forms.LeftRightAlignment.Right;
            this.dateTimePicker.Location = new System.Drawing.Point(176, 15);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(463, 25);
            this.dateTimePicker.TabIndex = 1;
            this.dateTimePicker.TabStop = false;
            this.dateTimePicker.Value = new System.DateTime(2016, 4, 22, 22, 56, 0, 0);
            this.dateTimePicker.CloseUp += new System.EventHandler(this.dateTimePicker_CloseUp);
            this.dateTimePicker.ValueChanged += new System.EventHandler(this.dateTimePicker_ValueChanged);
            // 
            // headerTextBox
            // 
            this.headerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerTextBox.Location = new System.Drawing.Point(300, 15);
            this.headerTextBox.Name = "headerTextBox";
            this.headerTextBox.Size = new System.Drawing.Size(339, 25);
            this.headerTextBox.TabIndex = 8;
            this.headerTextBox.TabStop = false;
            this.headerTextBox.TextChanged += new System.EventHandler(this.headerTextBox_TextChanged);
            // 
            // cbDrop
            // 
            this.cbDrop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDrop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbDrop.Font = new System.Drawing.Font("Times New Roman", 10F);
            this.cbDrop.FormattingEnabled = true;
            this.cbDrop.Items.AddRange(new object[] {
            "Показать дату концерта:",
            "Показать текст:",
            "Пустое поле"});
            this.cbDrop.Location = new System.Drawing.Point(10, 16);
            this.cbDrop.Name = "cbDrop";
            this.cbDrop.Size = new System.Drawing.Size(160, 23);
            this.cbDrop.TabIndex = 1;
            this.cbDrop.TabStop = false;
            this.cbDrop.SelectedIndexChanged += new System.EventHandler(this.cbDrop_SelectedIndexChanged);
            // 
            // lbTopField
            // 
            this.lbTopField.AutoSize = true;
            this.lbTopField.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Italic);
            this.lbTopField.Location = new System.Drawing.Point(11, 1);
            this.lbTopField.Name = "lbTopField";
            this.lbTopField.Size = new System.Drawing.Size(76, 15);
            this.lbTopField.TabIndex = 9;
            this.lbTopField.Text = "Угловое поле:";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(722, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(4, 24);
            this.panel1.TabIndex = 10;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ButtonsPanel.Controls.Add(this.panel1);
            this.ButtonsPanel.Controls.Add(this.lbTopField);
            this.ButtonsPanel.Controls.Add(this.cbDrop);
            this.ButtonsPanel.Controls.Add(this.headerTextBox);
            this.ButtonsPanel.Controls.Add(this.btnInfo);
            this.ButtonsPanel.Controls.Add(this.btnPrint);
            this.ButtonsPanel.Controls.Add(this.btnRemoveLast);
            this.ButtonsPanel.Controls.Add(this.btnAdd);
            this.ButtonsPanel.Controls.Add(this.dateTimePicker);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(811, 47);
            this.ButtonsPanel.TabIndex = 3;
            // 
            // MF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(811, 637);
            this.Controls.Add(this.MFStatusStrip);
            this.Controls.Add(this.ButtonsPanel);
            this.Controls.Add(this.MFPanel);
            this.Font = new System.Drawing.Font("Times New Roman", 11.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(732, 475);
            this.Name = "MF";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parminox";
            this.Activated += new System.EventHandler(this.MF_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MF_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MF_KeyDown);
            this.MFPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MFPictureBox)).EndInit();
            this.PopupMenu.ResumeLayout(false);
            this.MFStatusStrip.ResumeLayout(false);
            this.MFStatusStrip.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            this.ButtonsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox MFPictureBox;
        private System.Windows.Forms.Panel MFPanel;
        private System.Windows.Forms.StatusStrip MFStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel MFStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel MFStripStatusLabel2;
        private System.Windows.Forms.PrintDialog printDialog;
        private System.Drawing.Printing.PrintDocument printDocument;
        private System.Windows.Forms.PrintPreviewDialog MyPrintPreviewDialog;
        private System.Windows.Forms.ContextMenuStrip PopupMenu;
        private System.Windows.Forms.ToolStripMenuItem PopupAddWork;
        private System.Windows.Forms.ToolStripMenuItem PopupDeleteWork;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem PopupEditInstrumentList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem PopupPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem PopupAbout;
        private System.Windows.Forms.ToolStripMenuItem PopupNavigationHelp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem PopupExit;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnRemoveLast;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnInfo;
        private System.Windows.Forms.TextBox headerTextBox;
        private System.Windows.Forms.ComboBox cbDrop;
        private System.Windows.Forms.Label lbTopField;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel ButtonsPanel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripMenuItem smAddEmptyColumn;
    }
}

