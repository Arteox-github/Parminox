﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Parminox
{
    public partial class WorksForm : Form
    {
        private int ScoreIndex;
        private int kavicount = 0;
        public WorksForm()
        {
            InitializeComponent();
            using (Graphics ScreenGraphics = this.CreateGraphics())
            {
                wfPictureBox.Width = GI.ScreenDimensionRectangle.Width + 1;
                wfPictureBox.Height = GI.ScreenDimensionRectangle.Y + 1;
            }
            wfPictureBox.Image = new Bitmap(wfPictureBox.ClientSize.Width, wfPictureBox.ClientSize.Height);
            //------------------------
        }

        private void FillVoiceList()
        {
            btClose.Enabled = true;
            btSave.Enabled = false;
            wdGrid.Rows.Clear();
            string tooltipstring = "Если вы хотите добавить инструмент, которого нет в списке, \n" +
                    "добавьте его в основной список инструментов, \n" +
                    "начав его название со звёздочки(*), тогда он будет появляться в распечатке\n" +
                    "только для этого произведения.";
            int pos;
            for (int i = 0; i < GI.VoiceList.Count; i++)
            {
                pos = wdGrid.Rows.Add();
                wdGrid.Rows[pos].Cells[0].Value = GI.VoiceList[i];
                wdGrid.Rows[pos].Cells[0].ReadOnly = true;
                wdGrid.Rows[pos].Cells[1].ReadOnly = false;
            }
            if (wdGrid.RowCount > 0)
            {
                wdGrid.Rows[wdGrid.Rows.Count - 1].Cells[0].ToolTipText = tooltipstring;
            }
        }

        public DialogResult ProcessDialog(Form Sender, int scoreindex)
        {
            ScoreIndex = scoreindex;
            bool isnew = ScoreIndex < 0;
            tbTitle.Text = isnew ? String.Empty : GI.ScoreLibrary[ScoreIndex].FullName;
            tbHeaderTitle.Text = isnew ? String.Empty : GI.ScoreLibrary[ScoreIndex].HeaderName;
            FillVoiceList();
            if (!isnew)
            {
                List<VoicePairItem> voiceslist = new List<VoicePairItem>(GI.ScoreLibrary[ScoreIndex].VoicePairList);
                string wsv1;
                for (int vi = 0; vi < voiceslist.Count; vi++)
                {
                    for (int vr = 0; vr < wdGrid.RowCount; vr++)
                    {
                        wsv1 = wdGrid.Rows[vr].Cells[0].Value as string;
                        if (wsv1[0] == '*')
                        {
                            wsv1 = wsv1.Remove(0, 1);
                        }
                        if (voiceslist[vi].VoiceName.Equals(wsv1))
                        {
                            wdGrid.Rows[vr].Cells[1].Value = voiceslist[vi].VoiceText;
                            break;
                        }
                    }
                }
            }
            btSave.Enabled = false;
            return this.ShowDialog(Sender);
        }

        private void SaveItem()
        {
            int _tempScoreIndex = ScoreIndex;
            string title_text = tbTitle.Text.Trim();
            if (!tbTitle.Text.Equals(title_text))
            {
                tbTitle.Text = title_text;
            }
            string header_text = tbHeaderTitle.Text.TrimEnd();
            if (!tbHeaderTitle.Text.Equals(header_text))
            {
                tbHeaderTitle.Text = header_text;
            }
            if (String.IsNullOrEmpty(title_text))
            {
                MessageBox.Show("Не указано основное название произведения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                if (!tbTitle.Focused) tbTitle.Focus(); 
                return;
            }
            if (String.IsNullOrEmpty(tbHeaderTitle.Text))
            {
                MessageBox.Show("Не указано название произведения для заголовка колонки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                if (!tbHeaderTitle.Focused) tbHeaderTitle.Focus();
                return;
            }
            //----------------------------------------------------
            //  creating list from wdGrid data
            string wsv1, wsv2;
            List<VoicePairItem> vpi = new List<VoicePairItem>();
            vpi.Clear();
            for (int iv = 0; iv < wdGrid.RowCount; iv++)
            {
                if (iv == wdGrid.NewRowIndex) break;
                wsv2 = wdGrid.Rows[iv].Cells[1].Value as string;
                if (String.IsNullOrWhiteSpace(wsv2)) continue;
                wsv2 = wsv2.Trim();
                wdGrid.Rows[iv].Cells[1].Value = wsv2;
                wsv1 = wdGrid.Rows[iv].Cells[0].Value as string;
                if (wsv1[0] == '*')
                {
                    wsv1 = wsv1.Remove(0, 1);
                }
                vpi.Add(new VoicePairItem(wsv1, wsv2));
                // if (!String.IsNullOrEmpty(ssin)) ssin = ssin + ';';
                // ssin = ssin + (wdGrid.Rows[iv].Cells[0].Value as string) + '>' + wsv2;
            }
            //----------------------------------------------------
            //----------------------------------------------------
            ScoreItem sitt = new ScoreItem(0, title_text, header_text, vpi);
            if (_tempScoreIndex >= 0)
            {
                sitt.HashCode = GI.ScoreLibrary[_tempScoreIndex].HashCode;
                if (!sitt.FullName.Equals(GI.ScoreLibrary[_tempScoreIndex].FullName))
                {
                    if (MessageBox.Show("Вы изменили название произведения.\nХотите сохранить его как новое?",
                        "Сохранение редактирования: " + title_text,
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        _tempScoreIndex = -1;
                        sitt.HashCode = 0;
                    }
                }
            }
            //  checking if score item already exists
            int itex = GI.FindScoreByFullname(sitt.FullName);
            if ((itex >= 0) && (itex != _tempScoreIndex))
            {
                if (MessageBox.Show("Внимание!\nПроизведение с таким названием уже существует в базе данных.\n" +
                        "Хотите заменить его?", "Сохранение редактирования: " + sitt.FullName,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    sitt.HashCode = GI.ScoreLibrary[itex].HashCode;
                }
                else
                {
                    tbTitle.Focus();
                    return;
                }
            }
            //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            int mode = 1;   // 1 = insert new
            if (sitt.HashCode != 0)
            {
                //  upgating existing scoreitem
                ScoreItem sitemp = new ScoreItem(GI.ScoreLibrary[GI.FindScoreByHashcode(sitt.HashCode)]);
                if (sitt.FullName.Equals(sitemp.FullName))
                {
                    sitt.FullName = string.Empty;
                }
                //-----
                if (sitt.HeaderName.Equals(sitemp.HeaderName))
                {
                    sitt.HeaderName = string.Empty;
                }
                //-----
                if (VoicePairItemListIdent(sitt.VoicePairList, sitemp.VoicePairList))
                {
                    sitt.VoicePairList.Clear();
                }
                //-----
                if (String.IsNullOrEmpty(sitt.FullName) &&
                     String.IsNullOrEmpty(sitt.HeaderName) &&
                          (sitt.VoicePairList.Count == 0))
                {
                    btSave.Enabled = false;
                    tbTitle.Focus();
                    return;
                }
                if (MessageBox.Show("Вы уверены, что хотите перезаписать данные?",
                               "Сохранение редактирования: " + title_text,
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    tbTitle.Focus();
                    return;
                }
                mode = 0;   //  update
            }
            //-------------------------------------------------------
            if (GI.ProcessScoreItemToDatabase(sitt, mode))
            {
                btSave.Enabled = false;
                GI.WorkToShowHashcode = sitt.HashCode;  //   GI.FindScoreByFullname(title_text).;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Список произведений не обновлён.", "Ошибка записи в базу данных.", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbTitle.Focus();
            }
        }
        //================================================================
        private bool VoicePairItemListIdent(List<VoicePairItem> list1, List<VoicePairItem> list2)
        {
            if (list1.Count != list2.Count) return false;
            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].VoiceName.Equals(list2[i].VoiceName)) return false;
                if (!list1[i].VoiceText.Equals(list2[i].VoiceText)) return false;
            }
            return true;
        }
        //================================================================
        private void btSave_Click(object sender, EventArgs e)
        {
            SaveItem();
        }

        private void tbHeaderTitle_TextChanged(object sender, EventArgs e)
        {
            DrawPreviewHeader(tbHeaderTitle.Text);
            btSave.Enabled = true;
        }

        private void DrawPreviewHeader(string sss)
        {
            wfPictureBox.SuspendLayout();
            using (Graphics dhgr = Graphics.FromImage(wfPictureBox.Image))
            {
                dhgr.Clear(Color.White);
                GI.DrawHeaderDelegate(dhgr, sss, new Rectangle(0, 0, wfPictureBox.Image.Size.Width, wfPictureBox.Image.Size.Height), false);
            }
            wfPictureBox.ResumeLayout();
            wfPictureBox.Invalidate();
        }

        private void tbTitle_TextChanged(object sender, EventArgs e)
        {
            btSave.Enabled = true;
        }

        private void wdGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress += new KeyPressEventHandler(tbTitle_KeyPress);   //   (wdGrid_EditingControlKeyPress);
        }

        private void tbTitle_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '\'':
                    {
                        if (!e.Handled)
                        {
                            ProcessKavi();
                            e.KeyChar = (char)0;
                            e.Handled = true;
                        }
                        return;
                    }
                 //case ';':
                //case '>':
                //case '=':
                //case '*':
           } // end of switch
        }

        private void ProcessKavi()
        {
            kavicount++;
            if (kavicount > 3)
            {
                MessageBox.Show("Одинарные кавычки использовать нельзя. Пользуйтесь другими символами.", "Ввод недопустимого символа",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                kavicount = 0;
            }
        }

        private void WorksForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (wdGrid.IsCurrentCellInEditMode)
            {
                wdGrid.EndEdit();
            }

            if (btSave.Enabled)
                if (MessageBox.Show("Изменения не сохранены. Вы уверены, что хотите закрыть окно?", "Изменения не сохранены",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                       e.Cancel = true;
        }

        private void btClear_Click(object sender, EventArgs e)
        {
            bool icc = false;
            wdGrid.SuspendLayout();
            for (int i = 0; i < wdGrid.RowCount; i++)
            {
                if (String.IsNullOrEmpty(wdGrid.Rows[i].Cells[1].Value as string)) continue;
                wdGrid.Rows[i].Cells[1].Value = String.Empty;
                icc = true;
            }
            wdGrid.ResumeLayout();
            if (icc & !btSave.Enabled)
            {
                btSave.Enabled = true;
            }
            wdGrid.Focus();
        }

        private void wdGrid_Leave(object sender, EventArgs e)
        {
            wdGrid.ClearSelection();
        }

        private void wdGrid_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            if (e.Cell == null || e.StateChanged != DataGridViewElementStates.Selected || e.Cell.ColumnIndex != 0 || !this.IsHandleCreated) return;  //   || e.Cell.ColumnIndex != 0
            this.BeginInvoke(new Action(() =>
            {
                wdGrid.CurrentCell = wdGrid.Rows[e.Cell.RowIndex].Cells[1];
                wdGrid.BeginEdit(false);
            }));
        }

        private void wdGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            btSave.Enabled = true;
        }

        private void wdGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!wdGrid.IsCurrentCellInEditMode) wdGrid.BeginEdit(false);
        }


    }
}
