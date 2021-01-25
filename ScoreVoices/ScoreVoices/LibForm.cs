using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Parminox
{
    public partial class LibraryForm : Form
    {
        private int FormMode = 0;
        private List<string> SortingList = new List<string>();
        private string CellEditInitValue;
        private int EditRowIndex;

        public LibraryForm()
        {
            InitializeComponent();
            GI.WorkToShowHashcode = 0;
        }

        private void btCreate_Click(object sender, EventArgs e)
        {
            switch (FormMode)
            {
                case 0:  // create new score
                    CreateNewScoreItem();
                    break;
                case 1:   // save list of voices
                    SaveVoices();
                    break;
            }  //  end of switch
        }

        private void CreateNewScoreItem()
        {
            if ((GI.WF ?? (GI.WF = new WorksForm())).ProcessDialog(this, -1) == System.Windows.Forms.DialogResult.OK)
            {
                tbSearch.Clear();
                FillWorksList();
                dGrid.Focus();
            }
        }

        private void SaveVoices()
        {
            List<string> dGridListCopy = new List<string>();
            for (int i = 0; i < dGrid.RowCount; i++)
            {
                dGridListCopy.Add(dGrid[0, i].Value as string);
            }

            if (GI.FlushVoiceList(dGridListCopy))
            {
                MessageBox.Show("Список инструментов сохранён.", "Состав оркестра", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btCreate.Enabled = false;
                PopupInstrumentSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Список инструментов не обновлён.", "Ошибка записи в базу данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            dGrid.Focus();
        }

        public DialogResult ProcessDialog(Form Sender, int mode)
        {
            FormMode = mode;
            bool bmode = FormMode == 0;
            btCreate.Text = bmode ? "Создать" : "Сохранить список";
            this.Text = bmode ? "Библиотека произведений" : "Состав оркестра";
            dGrid.Columns[0].HeaderCell.Style.Font = new Font("Times New Roman", 11.25f, FontStyle.Italic);
            dGrid.Columns[0].HeaderText = bmode ? "Название произведения" : "Название инструмента";
            btCreate.Enabled = bmode;
            dGrid.AllowUserToAddRows = false;
            dGrid.AllowUserToDeleteRows = false;
            dGrid.ReadOnly = bmode; 
            tbSearch.Enabled = bmode;
            SearchLabel.Enabled = bmode; 

            switch (mode)
            {
                case 0:    // serving Score Library list
                    tbSearch.Clear();
                    FillWorksList();
                    dGrid.Columns[0].SortMode = DataGridViewColumnSortMode.Automatic;
                    break;
                case 1:  // serving Instrument list
                    dGrid.Rows.Clear();
                    dGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                    int wgt;

                    for (int ivc = 0; ivc < GI.VoiceList.Count; ivc++)
                    {
                        wgt = dGrid.Rows.Add();
                        dGrid.Rows[wgt].Cells[0].Value = GI.VoiceList[ivc];
                        dGrid.Rows[wgt].Cells[0].ReadOnly = false;
                    }
                    SetCurrentRow(dGrid.CurrentRow);
                    PopupInstrumentSave.Enabled = false;
                    break;
            }
            SetCurrentRow(dGrid.CurrentRow);
            return this.ShowDialog(Sender);
        }

        private void SetCurrentRow(DataGridViewRow RowToSelect)
        {
            DataGridViewRow rts = RowToSelect ?? (dGrid.RowCount > 0 ? dGrid.Rows[0] : null);
            if (rts != null)
            {
                dGrid.CurrentCell = rts.Cells[0];
                dGrid.CurrentCell.Selected = true;
            }
            else
            {
                dGrid.ClearSelection();
            }
        }

        private void dGrid_Enter(object sender, EventArgs e)
        {
            SetCurrentRow(dGrid.CurrentRow);
        }

        private void FillWorksList() 
        {
            dGrid.SuspendLayout();
            dGrid.Rows.Clear();
            int rowcn = -1;
            string issad;
            string filter =tbSearch.Text.Trim();

            for (int ilib = 0; ilib <  GI.ScoreLibrary.Count; ilib++)
            {
                issad = GI.ScoreLibrary[ilib].FullName;
                if (!String.IsNullOrWhiteSpace(filter))
                {
                    if (issad.IndexOf(filter, StringComparison.OrdinalIgnoreCase) < 0) continue;
                }
                rowcn = dGrid.Rows.Add();
                dGrid.Rows[rowcn].Cells[0].Value = issad;
            }
            if (dGrid.RowCount > 0)
            {
                dGrid.Sort(dGrid.Columns[0], ListSortDirection.Ascending);
                int rowtoshow = 0;
                if (GI.WorkToShowHashcode != 0)
                {
                    int showindex = GI.FindScoreByHashcode(GI.WorkToShowHashcode);
                    if (showindex >= 0)
                    {
                        string wtss = GI.ScoreLibrary[showindex].FullName;
                        for (int iwts = 0; iwts < dGrid.Rows.Count - 1; iwts++)
                        {
                            if (wtss.Equals(dGrid.Rows[iwts].Cells[0].Value as string))
                            {
                                rowtoshow = iwts;
                                break;
                            }
                        }
                    }                    
                }
                dGrid.SuspendLayout();
                SetCurrentRow(dGrid.Rows[rowtoshow]);
                if (!dGrid.Focused)
                {
                    dGrid.ClearSelection();
                }
                dGrid.ResumeLayout();
                dGrid.PerformLayout();
            }
            else
            {
                dGrid.Columns[0].HeaderText = "---";
                dGrid.ClearSelection();
            }
            dGrid.ResumeLayout();
        }

        private void SelectWork(int LibraryIndex)
        {
            int z = GI.ScoreLibrary[LibraryIndex].HashCode;
            GI.WorkToShowHashcode = z;
            GI.ProjectList.Add(new IntStringPair(z, String.Empty));
            this.DialogResult = DialogResult.OK;
        }

        private void SelectWork()
        {
            SelectWork(GI.FindScoreByFullname(dGrid.CurrentRow.Cells[0].Value as string));
        }

        private void RemoveFromLibrary()
        {
            string wsdel = dGrid.CurrentRow.Cells[0].Value as string;
            int wsi = GI.FindScoreByFullname(wsdel);
            if (wsi >= 0)
            {
                if (MessageBox.Show("Вы собираетесь удалить произведение из базы данных. Продолжить?",
                    wsdel, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                if (GI.ProcessScoreItemToDatabase(GI.ScoreLibrary[wsi], 2))  //  REMOVE
                {
                    if (wsi >= GI.ScoreLibrary.Count) wsi--;
                    GI.WorkToShowHashcode = GI.ScoreLibrary[wsi].HashCode;
                    tbSearch.Clear();
                    FillWorksList();
                }
                else
                {
                    MessageBox.Show("Список произведений не обновлён.", "Ошибка записи в базу данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                dGrid.Focus();
            }
        }

        private void InsertInstrument()
        {
            int curindex = dGrid.CurrentRow != null ? dGrid.CurrentRow.Index : -1;
            curindex++;
            dGrid.Rows.Insert(curindex, 1);
            SetCurrentRow(dGrid.Rows[curindex]);
            dGrid.BeginEdit(false);
        }

        private void RemoveInstrument(int delindex)
        {
            if (((dGrid.CurrentRow == null)) || (delindex < 0) || (delindex == dGrid.NewRowIndex)) return;
            string inm = dGrid.Rows[delindex].Cells[0].Value as string;
            if (MessageBox.Show($"Вы собираетесь удалить \"{inm}\" из списка. Продолжить?",
                "Удаление инструмента", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            if (GI.RenameDeleteInstrument(inm, String.Empty))
            {
                dGrid.Rows.RemoveAt(delindex);
                CheckVoiceListIdentity();
            }
            else
            {
                MessageBox.Show($"Инструмент {inm} из базы данных не удалён.", "Ошибка записи в базу данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (FormMode == 1)
            {
                ProcessCellEndEdit(e.RowIndex);
                CheckVoiceListIdentity();
            }
        }

        //+++++++++++++++
        private void ProcessCellEndEdit(int cur_index)
        {
            string _newvalue = (dGrid[0, cur_index].Value as string);
            if (!String.IsNullOrEmpty(_newvalue))
            {
                _newvalue = _newvalue.Trim();
            }

            if (!String.IsNullOrEmpty(_newvalue))
            {
                  //  if Trim() succeeded
                if (!_newvalue.Equals(dGrid[0, cur_index].Value as string))
                {
                    dGrid[0, cur_index].Value = _newvalue;
                }
                
                // check if old and new value are ident
                if (!String.IsNullOrEmpty(CellEditInitValue) && CellEditInitValue.Equals(_newvalue)) return;

                //  check if name is already exists
                for (int chi = 0; chi < dGrid.RowCount; chi++)
                {
                    if (chi == cur_index) continue;
                    if (!_newvalue.Equals(dGrid[0, chi].Value as string)) continue;
                    //  name is already exists
                    MessageBox.Show($"Инструмент с названием \"{_newvalue}\" уже есть в списке.",
                        "Дублирование названия", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    if (!String.IsNullOrEmpty(CellEditInitValue))
                    {
                        dGrid[0, cur_index].Value = CellEditInitValue;
                    }
                    else
                    {
                        InvokeRowRemoving(cur_index);
                    }
                    return;
                }

                if (String.IsNullOrEmpty(CellEditInitValue))
                {     
                    // init value is empty but new value is not  >>  new instrument on input
                    CellEditInitValue = _newvalue;
                }
                else
                {
                    //  rename instrument
                    if (MessageBox.Show($"Заменить название инструмента \"{ CellEditInitValue}\" на \"{_newvalue}\"?",
                        "Изменение названия инструмента", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (GI.RenameDeleteInstrument(CellEditInitValue, _newvalue))
                        {
                            // no errors
                            CellEditInitValue = _newvalue;
                        }
                        else
                        // databse error
                        {
                            dGrid[0, cur_index].Value = CellEditInitValue;
                            MessageBox.Show("Название инструмента в базе данных не обновлёно.", "Ошибка записи в базу данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        //  MessageBox >> answer "No"
                        dGrid[0, cur_index].Value = CellEditInitValue;
                    }
                }   //  if (String.IsNullOrEmpty(CellEditInitValue))
            }
            else
            // new value is empty
            {
                if (!String.IsNullOrEmpty(CellEditInitValue))
                {
                    dGrid[0, cur_index].Value = CellEditInitValue;
                }
                else
                {
                    InvokeRowRemoving(cur_index);
                }
            }
        }

        //============================================
        private void dGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int)Keys.F1)
            {
                ShowHelp();
            }
            switch (FormMode)
            {
                case 0:  // Library mode
                    switch (e.KeyValue)
                    {
                        case (int)Keys.F2:   //  edit title
                            e.Handled = true;
                            EditWork();
                            break;
                        case (int)Keys.Delete:   //  remove from database
                            e.Handled = true;
                            RemoveFromLibrary();
                            break;
                        case (int)Keys.Enter:
                            e.Handled = true;
                            SelectWork();
                            break;
                        case (int)Keys.I:
                            e.Handled = true;
                            WorkInfo();
                            break;
                    } // switch (e.KeyValue)
                    break;
                    //============================================================================
                case 1:   //  Instrument list mode
                    string dgs = String.Empty;
                    int curindex = dGrid.CurrentRow.Index;
                    switch (e.KeyValue)
                    {
                        case (int)Keys.Enter: 
                            e.Handled = true;
                            if (dGrid.CurrentRow != null)
                            {
                                if (!dGrid.IsCurrentCellInEditMode)
                                {
                                    dGrid.BeginEdit(false);
                                }
                            }
                            break;
                        case (int)Keys.Delete:
                            e.Handled = true;
                            RemoveInstrument(curindex);
                            break;
                        case (int)Keys.Up:
                            if (dGrid.CurrentRow != null)
                            {
                                if (((ModifierKeys & Keys.Control) == Keys.Control) && (curindex > 0))
                                {
                                    e.Handled = true;
                                    if ((curindex == 0) || (curindex == dGrid.NewRowIndex)) return;
                                    dgs = dGrid.CurrentRow.Cells[0].Value as string;
                                    dGrid.Rows[curindex].Cells[0].Value = dGrid.Rows[curindex - 1].Cells[0].Value;
                                    curindex--;
                                    dGrid.Rows[curindex].Cells[0].Value = dgs;
                                    CheckVoiceListIdentity();
                                    SetCurrentRow(dGrid.Rows[curindex]);
                                }
                            }
                            break;
                        case (int)Keys.Down:
                            if (dGrid.CurrentRow != null)
                            {
                                    if (((ModifierKeys & Keys.Control) == Keys.Control) && (curindex >= 0))
                                {
                                    e.Handled = true;
                                    if ((curindex >= dGrid.Rows.Count - 1) || (curindex + 1) == dGrid.NewRowIndex) return;
                                    dgs = dGrid.CurrentRow.Cells[0].Value as string;
                                    dGrid.Rows[curindex].Cells[0].Value = dGrid.Rows[curindex + 1].Cells[0].Value;
                                    curindex++;
                                    dGrid.Rows[curindex].Cells[0].Value = dgs;
                                    SetCurrentRow(dGrid.Rows[curindex]);
                                    CheckVoiceListIdentity();
                                }
                            }
                            break;
                        case (int)Keys.Insert:
                            e.Handled = true;
                            InsertInstrument();
                            break;
                        case (int)Keys.Escape:
                            if (dGrid.CurrentRow != null)
                            {
                                if (!String.IsNullOrEmpty(CellEditInitValue))
                                {
                                    e.Handled = true;
                                    dGrid.Rows[dGrid.CurrentRow.Index].Cells[0].Value = CellEditInitValue;
                                }
                            }
                            break;
                    }  // switch (e.KeyValue)
                    break;
            }   // switch (FormMode)
        }

        private void dGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (FormMode == 0) SelectWork();
        }

        private void LibraryForm_Activated(object sender, EventArgs e)
        {
            dGrid.Focus();
        }

        private void LibraryForm_Shown(object sender, EventArgs e)
        {
            if ((FormMode == 1) && (dGrid.RowCount == 0))
                MessageBox.Show("Список инструментов пуст.\nНажмите \"Insert\", чтобы добавить первый.",
                    "Список инструментов пуст", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void EditWork(int LibraryIndex)
        {
            if ((GI.WF ?? (GI.WF = new WorksForm())).ProcessDialog(this, LibraryIndex) == System.Windows.Forms.DialogResult.OK)
            {
                FillWorksList();
            }
            dGrid.Focus();
        }

        private void EditWork()
        {
            EditWork(GI.FindScoreByFullname(dGrid.CurrentRow.Cells[0].Value as string));
        }

        private void hlpButton_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void ShowHelp()
        {
            switch (FormMode)
            {
                case 0: MessageBox.Show("Управление:\rEnter или двойной щелчок мыши - Добавить в проект концерта\rF2 - Редактировать\rDel - Удалить из базы данных",
                    "Список произведений", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                case 1: MessageBox.Show("Управление:\rCtrl+<Вверх>,  Ctrl+<Вниз> - Переместить по списку\rInsert - Добавить в список\rDel - Удалить из списка\n\nИнструмент, название которого начинается со звёздочки ('*'), появится в распечатке концерта, только если он занят в концерте.",
                    "Список инструментов", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
            }  //  end of switch
            dGrid.Focus();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            if (tbSearch.Focused)
            {
                FillWorksList();
                tbSearch.Focus();
            }
        }

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space) if (tbSearch.Text.Length == 0) e.Handled = true;
        }

        private void dGrid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            EditRowIndex = e.RowIndex;
            CellEditInitValue = dGrid[0, EditRowIndex].Value as string;
        }

        private void CheckVoiceListIdentity()
        {
            bool change_enable = (GI.VoiceList.Count != dGrid.RowCount);            
            if (!change_enable)
            {
                string wss;
                for (int i = 0; i < GI.VoiceList.Count; i++)
                {
                    wss = dGrid[0, i].Value as string;
                    if (String.IsNullOrEmpty(wss) || !wss.Equals(GI.VoiceList[i]))
                    {
                        change_enable = true;
                        break;
                    }
                }
            }

            if (btCreate.Enabled != change_enable)
            {
                btCreate.Enabled = change_enable;
                PopupInstrumentSave.Enabled = change_enable;
            }
        }

        private void InvokeRowRemoving(int RemoveIndex)
        {
            this.BeginInvoke(new Action(() =>
            {
                dGrid.Rows.RemoveAt(RemoveIndex);
                CheckVoiceListIdentity();
            }));
        }

        private void dGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SetCurrentRow(dGrid.Rows[e.RowIndex]);
        }

        private void dGrid_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int curindex = dGrid.CurrentRow != null ? dGrid.CurrentRow.Index : -1;
                int rin = dGrid.HitTest(e.X, e.Y).RowIndex;
                if ((rin >= 0) && (rin != curindex))
                {
                    dGrid.ClearSelection();
                    SetCurrentRow(dGrid.Rows[rin]);
                }
                bool rowexists = rin >= 0;
                switch (FormMode)
                {
                    case 0:
                        PopupLibrarySelect.Enabled =
                        PopupLibraryInfo.Enabled =
                        PopupLibraryEdit.Enabled =
                        PopupLibraryDelete.Enabled = rowexists;
                        PopupLibrary.Show(MousePosition);
                        break;
                    case 1:
                        if (!dGrid.IsCurrentCellInEditMode)
                        {
                            PopupInstrumentRemove.Enabled = PopupInstrumentEdit.Enabled = rowexists;
                            PopupInstruments.Show(MousePosition);
                        }
                        break;
                    default: break;
                }
            }
        }

        private void PopupInstrumentEdit_Click(object sender, EventArgs e)
        {
            dGrid.BeginEdit(false);
        }

        private void PopupInstrumentAdd_Click(object sender, EventArgs e)
        {
            InsertInstrument();
        }

        private void PopupInstrumentRemove_Click(object sender, EventArgs e)
        {
            RemoveInstrument(dGrid.CurrentRow.Index);
        }

        private void PopupInstrumentSave_Click(object sender, EventArgs e)
        {
            SaveVoices();
        }

        private void PopupLibraryCreateNew_Click(object sender, EventArgs e)
        {
            CreateNewScoreItem();
        }

        private void PopupLibrarySelect_Click(object sender, EventArgs e)
        {
            SelectWork();
        }

        private void PopupLibraryEdit_Click(object sender, EventArgs e)
        {
            EditWork();
        }

        private void PopuLibraryDelete_Click(object sender, EventArgs e)
        {
            RemoveFromLibrary();
        }

        private void PopupLibraryInfo_Click(object sender, EventArgs e)
        {
            WorkInfo();
        }

        private void WorkInfo()
        {
            int vi = GI.FindScoreByFullname(dGrid.CurrentCell.Value as string);
            string vldd = String.Empty;
            for (int i = 0; i < GI.ScoreLibrary[vi].VoicePairList.Count; i++)
            {
                vldd += $"\n{GI.ScoreLibrary[vi].VoicePairList[i].VoiceName}: {GI.ScoreLibrary[vi].VoicePairList[i].VoiceText}";
            }
            MessageBox.Show($"{ GI.ScoreLibrary[vi].FullName}\n{vldd}", "Информация о произведении",
                 MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void dGrid_Leave(object sender, EventArgs e)
        {
            if (dGrid.IsCurrentCellInEditMode)
            {
                dGrid.CancelEdit();
            }
            dGrid.ClearSelection();
        }

        private void LibraryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dGrid.IsCurrentCellInEditMode)
            {
                dGrid.CancelEdit();
            }

            if (FormMode == 1 && btCreate.Enabled)
            {
                DialogResult dr = MessageBox.Show("Сохранить изменения в списке?", "Список интрументов изменён",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    SaveVoices();
                }
                else
                {
                    if (dr == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }


    }
}
