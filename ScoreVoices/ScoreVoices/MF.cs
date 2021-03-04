using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing;
using System.Windows.Forms;

namespace Parminox
{
    public partial class MF : Form
    {
        public const string FontName = "Times New Roman";  // "Arial"; // "Book Antiqua";  // "Constantia"
        private string MFDateNumber;
        private string MFMonth;
        private string MFDayOfWeek;
        public readonly float FPrinterScaling = 100f / 96f; //   1,0416666666666666666666666666667f
        public readonly int PrintableAreaMargin = 15;
        public SizeF PrintableAreaSizeF;
        public Rectangle PrinterDimentionRectangle;
        public readonly int MaxLinesPerList = 34;
        public readonly int MaxColumnsPerList = 12;
        private bool TooMuchVoicesWarninigFired = false;
        public Font Header_Font;
        private int Emphasized = 0;

        public MF()
        {
            GI.DrawHeaderDelegate = DrawScoreHeaderWithFrame;
            this.Font = new Font(FontName, 16, FontStyle.Regular);
            InitializeComponent();
            MFStatusStrip.Items[0].Text = "DB";
            Header_Font = new Font(FontName, 14f, FontStyle.Italic | FontStyle.Bold);
            ShowMessageWithDelegate("Загрузка данных...", () => GI.InitializeComponents());
            PrintableAreaSizeF.Width = 21.0f - ((float)PrintableAreaMargin * 2.54f / 100f) * 2f;
            PrintableAreaSizeF.Height = 29.7f - ((float)PrintableAreaMargin * 2.54f / 100f) * 2f;
            using (Graphics ScreenGraphics = this.CreateGraphics())
            {
                GI.ScreenDpiX = (int)ScreenGraphics.DpiX;
                GI.ScreenDpiY = (int)ScreenGraphics.DpiY;
                GI.ScreenDimensionRectangle = GetDimentionRectangle(ScreenGraphics, ScreenGraphics.DpiX, ScreenGraphics.DpiY, 1);
            }
            this.KeyPreview = true;
            printDocument.PrintController = new StandardPrintController();
            printDocument.PrinterSettings = new PrinterSettings();
            MyPrintPreviewDialog.PrintPreviewControl.AutoZoom = true;
            MyPrintPreviewDialog.Height = Screen.PrimaryScreen.Bounds.Height - 48; //   (int)(590f * (297f / 210f));
            MyPrintPreviewDialog.Width = (int)(MyPrintPreviewDialog.Height * (210f / 297f));
            printDocument.DefaultPageSettings.Margins.Left = PrintableAreaMargin;
            printDocument.DefaultPageSettings.Margins.Top = PrintableAreaMargin;
            printDocument.OriginAtMargins = true;
            MFPictureBox.Width = 8 + GI.ScreenDimensionRectangle.X + (GI.ScreenDimensionRectangle.Width * MaxColumnsPerList);
            MFPictureBox.Height = 8 + GI.ScreenDimensionRectangle.Y + (GI.ScreenDimensionRectangle.Height * MaxLinesPerList);
            MFPictureBox.Left = 0;
            MFPictureBox.Top = 0;
            MFPanel.VerticalScroll.SmallChange = 26;
            MFPanel.VerticalScroll.Visible = true;
            this.Width = MFPictureBox.Width + SystemInformation.VerticalScrollBarWidth + 20;  // 5 borders * 4

            MFPictureBox.Image = new Bitmap(MFPictureBox.Width, MFPictureBox.Height);

            this.MouseWheel += ServeMouseWheel;

            GI.ProjectList.Clear();
            btnRemoveLast.Enabled = false;
            SetupDate(DateTime.Now);
            headerTextBox.Left = dateTimePicker.Left;
            headerTextBox.Width = dateTimePicker.Width;
            ServeTopRightField(cbDrop.SelectedIndex);  //  Calls ConstructPage() there
        }

        private void MF_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Header_Font != null)
            {
                Header_Font.Dispose();
            }
        }
        //===============================================================
        public void ServeMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta == 0) return;
            if (e.Delta > 0)
            {
                // The user scrolled up.
                PictureScroll(-24);
            }
            else
            {
                // The user scrolled down.
                PictureScroll(24);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (this.ActiveControl == null)
            {

                switch (keyData)
                {
                    case Keys.Left:
                        if (Emphasized > 0 && GI.ProjectList.Count > 1)
                        {
                            Emphasized--;
                            MFConstructPage();
                        }
                        return true;

                    case (Keys.Left | Keys.Control):
                        if (Emphasized > 0 && GI.ProjectList.Count > 1)
                        {
                            MoveEmphasizedIndex(Emphasized, Emphasized - 1);
                        }
                        return true;

                    case Keys.Right:
                        if (Emphasized >= 0 && GI.ProjectList.Count > 1 && Emphasized <= GI.ProjectList.Count - 2)
                        {
                            Emphasized++;
                            MFConstructPage();
                        }
                        return true;

                    case (Keys.Right | Keys.Control):
                        if (Emphasized >= 0 && GI.ProjectList.Count > 1 && Emphasized <= GI.ProjectList.Count - 2)
                        {
                            MoveEmphasizedIndex(Emphasized, Emphasized + 1);
                        }
                        return true;

                    case Keys.Down:
                        PictureScroll(24);
                        return true;

                    case Keys.Up:
                        PictureScroll(-24);
                        return true;
                }
            }            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MoveEmphasizedIndex (int sourceindex, int destindex)
        {
            if (sourceindex.Equals(destindex) || sourceindex < 0 || destindex < 0 || destindex >= GI.ProjectList.Count || sourceindex >= GI.ProjectList.Count) return;
            IntStringPair vp1 = new IntStringPair(GI.ProjectList[sourceindex]);
            int ind_to = sourceindex;
            int ind_from;
            if (sourceindex > destindex)
            {
                //moving forward (right)
                ind_from = sourceindex - 1;
                for (;;)
                {
                    GI.ProjectList[ind_to] = new IntStringPair(GI.ProjectList[ind_from]);
                    ind_to--;
                    if (ind_to.Equals(destindex)) break;
                    ind_from--;
                }
            }
            else
            {
                // moving backwards (left)
                ind_from = sourceindex + 1;
                for (;;)
                {
                    GI.ProjectList[ind_to] = new IntStringPair(GI.ProjectList[ind_from]);
                    ind_to++;
                    if (ind_to.Equals(destindex)) break;
                    ind_from++;
                }
            }
            GI.ProjectList[destindex] = new IntStringPair(vp1);
            Emphasized = destindex;
            MFConstructPage();

        }

        private void PictureScroll(int delta)
        {
            Point pmv = MFPanel.AutoScrollPosition;
            pmv.Y = -pmv.Y + delta;
            MFPanel.AutoScrollPosition = pmv;
        }
        //===============================================================
        private bool CleanupEmphasize()
        {
            if (GI.ProjectList.Count > 0)
            {
                if (Emphasized < 0)
                {
                    Emphasized = 0;
                }
                else
                if (Emphasized >= GI.ProjectList.Count - 1)
                {
                    Emphasized = GI.ProjectList.Count - 1;
                }
            }
            else
            {
                Emphasized = -1;
            }
            return Emphasized >= 0;
        }

        private void RemoveLastListing()
        {
            if (CleanupEmphasize())
            {
                int ix = GI.ProjectList[Emphasized].Int_Value;
                string mt = null;
                if (ix != 0)
                {
                    mt = $"Вы собираетесь удалить \n  \"{ GI.ScoreLibrary[GI.FindScoreByHashcode(ix)].FullName}\"\n из проекта концерта. Продолжить?";
                }
                else
                {
                    mt = $"Вы собираетесь удалить колонку с текстом\n  \"{ GI.ProjectList[Emphasized].String_Value}\"\n из проекта концерта. Продолжить?";
                }

                if (MessageBox.Show(mt, "Удаление из проекта", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    GI.ProjectList.RemoveAt(Emphasized);
                    TooMuchVoicesWarninigFired = false;
                    MFConstructPage();
                    btnRemoveLast.Enabled = GI.ProjectList.Count > 0;
                }
            }
            this.ActiveControl = null;
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private Rectangle GetDimentionRectangle(Graphics CurrentGraphics, float ForceDPX, float ForceDPY, float Scal)
        {
            // A4 Size = 210×297 мм
            float gdpx = CurrentGraphics.DpiX / (CurrentGraphics.DpiX / ForceDPX);
            float gdpy = CurrentGraphics.DpiY / (CurrentGraphics.DpiY / ForceDPY);
            float rx = 1.575f * gdpx * Scal;   // 4 sm / 2.54 = 1.575
            float ry = 1.496f * gdpy * Scal;     //  3.8 sm / 2.54 = 1.496
            //float rw =  0.5327f * gdpy * Scal;  // 1.4 sm / 2.54 = 0.572      0.539 -- 0.5327
            float rw = ((PrintableAreaSizeF.Width / 2.54f - 1.575f) / MaxColumnsPerList) * gdpx * Scal;
            //float rh = 0.288f * gdpy * Scal;   //  0.7 sm / 2.54 = 0.296
            float rh = ((PrintableAreaSizeF.Height / 2.54f - 1.496f) / MaxLinesPerList) * gdpy * Scal;
            return new Rectangle((int)rx, (int)ry, (int)rw, (int)rh);
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        bool TooMuchVoicesWarninigGoFire = false;
        private void MFConstructPage()
        {
            MFPictureBox.SuspendLayout();
            TooMuchVoicesWarninigGoFire = false;
            ConstructPage(Graphics.FromImage(MFPictureBox.Image), 0);  //  Mode: 0 - screen
            MFPictureBox.ResumeLayout();
            MFPictureBox.PerformLayout();
            MFPictureBox.Invalidate();

            if (TooMuchVoicesWarninigGoFire)
            {
                ServeTooManyVoicesInList();
            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private void ConstructPage(Graphics gf, int Mode)   //  Mode: 0 - screen, 1 - printer
        {
            int ix = 0;
            Rectangle p = (Mode == 0) ? GI.ScreenDimensionRectangle : GetDimentionRectangle(gf, 96, 96, FPrinterScaling);
            gf.Clear(Color.White);
            // Drawing Date Rectangle
            gf.DrawRectangle(Pens.Black, 2, 2, p.X - 2, p.Y - 2);
            switch (cbDrop.SelectedIndex)
            {
                case 0:    //  print Date
                    {
                        using (Font dFont = new Font(FontName, 38, FontStyle.Bold))
                        {
                            SizeF datenum = gf.MeasureString(MFDateNumber, dFont);
                            gf.DrawString(MFDateNumber, dFont, Brushes.Black, (p.X - datenum.Width) / 2f, 0);
                        }
                        using (Font mFont = new Font(FontName, 18, FontStyle.Bold))
                        {
                            SizeF datemonth = gf.MeasureString(MFMonth, mFont);
                            gf.DrawString(MFMonth, mFont, Brushes.Black, (p.X - datemonth.Width) / 2f, p.Y / 2.7f);
                        }
                        using (Font wFont = new Font(FontName, 9, FontStyle.Bold))
                        {
                            SizeF dateweek = gf.MeasureString(MFDayOfWeek, wFont);
                            gf.DrawString(MFDayOfWeek, wFont, Brushes.Black, (p.X - dateweek.Width) / 2f, p.Y / 1.8f);
                        }
                        break;
                    }
                case 1:   //  print text
                    {
                        if (string.IsNullOrEmpty(headerTextBox.Text)) break;
                        using (Font tFont = new Font(FontName, 24, FontStyle.Bold))
                        using ( StringFormat sfrm = new StringFormat() {
                                Alignment = StringAlignment.Center,
                                LineAlignment = StringAlignment.Center})
                        {
                            RectangleF r = new RectangleF(2, 2, p.X - 4, p.Y - 4);
                            SizeF textlen = gf.MeasureString(headerTextBox.Text, tFont);
                            if (textlen.Width < p.X - 4)
                            {
                                gf.DrawString(headerTextBox.Text, tFont, Brushes.Black, r, sfrm);
                            }
                            else
                            {
                                using (Font wFont = new Font(FontName, 16, FontStyle.Bold))
                                {
                                    gf.DrawString(headerTextBox.Text, wFont, Brushes.Black, r, sfrm);
                                }
                            }
                        }
                        break;
                    }
                default: break;
            }
            //============================================================
            int arcnt = GI.ProjectList.Count + 2;
            List<IntStringPair> p_heap = new List<IntStringPair>();
            string vs;
            int vi;
            for (int i = 0; i < GI.VoiceList.Count; i++)
            {
                vs = GI.VoiceList[i];
                if (vs[0] == '*')
                {
                    vi = 1;
                    vs = vs.Remove(0, 1);
                }
                else
                {
                    vi = 0;
                }
                p_heap.Add(new IntStringPair(vi, vs));
            }
            //Filling ProjectInstrumentList
            List<VoicePairItem> voiceslist;
            for (int pl_index = 0; pl_index < GI.ProjectList.Count; pl_index++)  //  i - loop for counting GI.ProjectList.Count (columns)
            {
                ix = GI.ProjectList[pl_index].Int_Value;
                if (ix == 0) continue;
                int vint = GI.ScoreLibrary.FindIndex(x => x.HashCode == ix);  //   GI.FindScoreByHashcode(ix);
                voiceslist = new List<VoicePairItem>(GI.ScoreLibrary[vint].VoicePairList);

                for (int vl_index = 0; vl_index < voiceslist.Count; vl_index++)
                {
                    int vv = p_heap.FindIndex(x => x.String_Value.Equals(voiceslist[vl_index].VoiceName));
                    if (vv >= 0)
                    {
                        if (p_heap[vv].Int_Value == 1) p_heap[vv].Int_Value = 0;
                    }
                    else
                    {
                        p_heap.Add(new IntStringPair(0, voiceslist[vl_index].VoiceName));
                    }
                }
            }
            // removing rows with leading '*'
            for (int dni = p_heap.Count - 1; dni >= 0; dni--)
            {
                if (p_heap[dni].Int_Value == 1) p_heap.RemoveAt(dni);
            }
            // Drawing ProjectInstrumentList
            Rectangle pirect = new Rectangle(2, 0, p.X - 2, p.Height - 2);
            for (int vl = 0; (vl < p_heap.Count); vl++)
            {
                pirect.Y = 2 + p.Y + (p.Height * vl);
                DrawCell(gf, p_heap[vl].String_Value, pirect, true, FontName, 12, -1);
            }
            if (Mode == 0)  //  screen
            {
                int lines_existing = p_heap.Count;
                MFStatusStrip.Items[1].Text = $"Количество инструментов в списке: {lines_existing.ToString()} " +
                    $"(максимум {MaxLinesPerList.ToString()})";
                if (lines_existing > MaxLinesPerList)
                {
                    if (!TooMuchVoicesWarninigFired)
                    {
                        TooMuchVoicesWarninigGoFire = true;
                    }
                }
                if (CleanupEmphasize())
                { 
                    Rectangle gr = new Rectangle(2 + p.X + (p.Width * Emphasized), 2, p.Width - 1, p.Y + (p.Height * p_heap.Count) - 2);
                    gf.FillRectangle(new SolidBrush(Color.FromArgb(0xFF, 216, 237, 216)), gr);
                }
            }
            //==============================================
            // Drawing Cells
            int pilicount = p_heap.Count;
            if (GI.ProjectList.Count > 0)
            {
                Rectangle headrect = new Rectangle(0, 2, p.Width - 2, p.Y - 2);
                pirect = new Rectangle(0, 0, p.Width - 2, p.Height - 2);
                string drawstring = String.Empty;
                int vint = 0;
                for (int i = 0; i < GI.ProjectList.Count; i++)
                {
                    headrect.X = pirect.X = 2 + p.X + (p.Width * i);
                    ix = GI.ProjectList[i].Int_Value;
                    if (ix != 0)
                    {
                        // drawing header
                        vint = GI.FindScoreByHashcode(ix);
                        drawstring = GI.ScoreLibrary[vint].HeaderName;
                        DrawScoreHeaderWithFrame(gf, drawstring, headrect, true);
                        int cindex;
                        for (int icl = 0; icl < pilicount; icl++)
                        {
                            drawstring = null;
                            cindex = GI.ScoreLibrary[vint].VoicePairList.FindIndex(x =>
                            {
                                if (!x.VoiceName.Equals(p_heap[icl].String_Value))
                                {
                                    return false;
                                }
                                drawstring = x.VoiceText;
                                return true;
                            });
                            pirect.Y = 2 + p.Y + (p.Height * icl);
                            DrawCell(gf, drawstring, pirect, true);
                        }
                    }
                    else
                    {
                        //  drawing empty column text
                        drawstring = GI.ProjectList[i].String_Value;
                        SizeF tsz = gf.MeasureString(drawstring, Header_Font);
                        Rectangle empheadrect = new Rectangle(headrect.X, headrect.Y + headrect.Width / 3, headrect.Width, (int)tsz.Width + 8);
                        int empmaxheight = headrect.Height + p.Height * pilicount;
                        if (empheadrect.Height > empmaxheight)
                        {
                            empheadrect.Height = empmaxheight;
                            int tot_length = empmaxheight - 4;
                            SizeF c_size;
                            drawstring += "...";
                            for (;;)
                            {
                                drawstring = drawstring.Remove(drawstring.Length - 4, 1);
                                c_size = gf.MeasureString(drawstring, Header_Font);
                                if (c_size.Width <= tot_length) break;
                            }
                        }
                        DrawScoreHeaderWithFrame(gf, drawstring, empheadrect, false);
                        gf.DrawRectangle(Pens.Black, headrect.X, headrect.Y, headrect.Width, empmaxheight);
                    }
                }
            }
            gf.DrawRectangle(Pens.Black, 0, 0, p.X + (p.Width * GI.ProjectList.Count) + 2,
                p.Y + (p.Height * p_heap.Count) + 2);
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public void DrawScoreHeaderWithFrame(Graphics ScoreGraphics, string HeaderText, Rectangle HeaderRect, bool DrawFrame)
        {
            System.Drawing.Drawing2D.GraphicsState transState = ScoreGraphics.Save();
            ScoreGraphics.TranslateTransform(HeaderRect.X, HeaderRect.X + HeaderRect.Height + 1);
            ScoreGraphics.RotateTransform(270);
            ScoreGraphics.DrawString(HeaderText, Header_Font, Brushes.Black, new RectangleF(HeaderRect.X, HeaderRect.Y, HeaderRect.Height - 1, HeaderRect.Width - 1));
            ScoreGraphics.Restore(transState);
            if (DrawFrame) ScoreGraphics.DrawRectangle(Pens.Black, HeaderRect.X, HeaderRect.Y, HeaderRect.Width, HeaderRect.Height);
        }

        private void DrawCell(Graphics ingr, string sss, Rectangle inRect, Boolean inBold)
        {
            float fSize = 16f;
            float Y_correction = 1f;
            if (!String.IsNullOrEmpty(sss))
            {
                if (sss.Length == 1)
                {
                    if (sss == "+")
                    {
                        fSize = 26f;
                        Y_correction = 1.4f;
                    }
                }
                else
                {
                    SizeF linsz;
                    for (;;)
                    {
                        linsz = ingr.MeasureString(sss, new Font(FontName, fSize, FontStyle.Bold));
                        if ((linsz.Width <= inRect.Width - 3f) || (fSize <= 9f)) break;
                        fSize -= 1f;
                        Y_correction -= 2f / 7f;
                    }
                }
            }            
            DrawCell(ingr, sss, inRect, inBold, FontName, fSize, Y_correction);
        }

        private void DrawCell(Graphics ingr, string sss, Rectangle inRect, Boolean inBold, string DrawSellFontName, float FontSize, float Y_TextCorrection)
        {
            ingr.DrawRectangle(Pens.Black, inRect);
            if (String.IsNullOrEmpty(sss)) return;
            using (Font inFont = new Font(DrawSellFontName, FontSize, inBold ? FontStyle.Bold : FontStyle.Regular))
            {
                SizeF instrum = ingr.MeasureString(sss, inFont);
                ingr.DrawString(sss, inFont, Brushes.Black, inRect.X + ((inRect.Width - instrum.Width) / 2), Y_TextCorrection + inRect.Y + ((inRect.Height + 2 - instrum.Height) / 2));
            }
        }
        //======================================================
        private void ServeTooManyVoicesInList()
        {
            if (!TooMuchVoicesWarninigFired)
            {
                MessageBox.Show("Внимание, не все инструменты отображаются в списке!", "Список инструментов переполнен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TooMuchVoicesWarninigFired = true;
                TooMuchVoicesWarninigGoFire = false;
            }
        }
        //======================================================
        //======================================================
        //======================================================
        private void ShowLF(int showmode)
        {
            (GI.LF ?? (GI.LF = new LibraryForm())).ProcessDialog(this, showmode);
            if (showmode == 0) // add work mode
            {
                btnRemoveLast.Enabled = GI.ProjectList.Count > 0;
                TooMuchVoicesWarninigFired = false;
            }
            MFConstructPage();
        }

        private readonly string[] MonthList = new string[]
        {
            "Января", "Февраля", "Марта", "Апреля", "Мая", "Июня", "Июля", "Августа",
            "Сентября", "Октября", "Ноября", "Декабря"
        };

        private readonly string[] DayOfWeekList = new string[]
        {
            "Воскресенье", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота"
        };

        private void SetupDate(DateTime MyDateTime)
        {
            MFDateNumber = MyDateTime.Day.ToString();
            MFMonth = MonthList[MyDateTime.Month - 1];
            MFDayOfWeek = '(' + DayOfWeekList[Convert.ToInt32(MyDateTime.DayOfWeek)] + ')';
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddNewColumn(0);
        }

        private void AddNewColumn(int mode)
        {
            if (GI.ProjectList.Count >= MaxColumnsPerList)
            {
                MessageBox.Show("Внимание, нет места на листе. Добавление новой колонки невозможно.", "Лист заполнен", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            switch (mode)
            {
                case 0:  // add new work
                default:
                    ShowLF(0);
                    break;
                case 1:  // add new empty column
                    string ecstring = ServeEmptyColumnText(null);
                    if (!String.IsNullOrEmpty(ecstring))
                    {
                        GI.ProjectList.Add(new IntStringPair(0, ecstring));
                        btnRemoveLast.Enabled = GI.ProjectList.Count > 0;
                        MFConstructPage();
                    }
                    break;
            }
            this.ActiveControl = null;
        }

        private void btnRemoveLast_Click(object sender, EventArgs e)
        {
            RemoveLastListing();            
        }

        private void dateTimePicker_CloseUp(object sender, EventArgs e)
        {
            SetupDate(dateTimePicker.Value);
            MFConstructPage();
            this.ActiveControl = null;
        }

        private void dateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            SetupDate(dateTimePicker.Value);
            MFConstructPage();
        }

        private void MF_Activated(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private DialogResult AskBeforeClosing()
        {
            return MessageBox.Show("Вы уверены, что хотите закрыть приложение?", "Проект концерта", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            GoPrinting();
            this.ActiveControl = null;
        }

        private void GoPrinting()
        {
            printDocument.DocumentName = "Концерт " + MFDateNumber + " " + MFMonth;
            if (printDialog.ShowDialog() == DialogResult.OK)
                if (MyPrintPreviewDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    printDocument.Print();
        }

        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            ConstructPage(e.Graphics, 1);  //  1 - printer mode
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            ShowHelp();
            this.ActiveControl = null;
        }

        private void ShowInfo()
        {
            MessageBox.Show("Программа была создана в 2016 году для оркестра\nГосударственной Академической Капеллы Санкт-Петербурга и Ольги Тимошенко.\n\nАвтор программы: Павел Серебряков\n\u00a9All Rights Reserved (St-Petersburg, 2016)",
                "О программе",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShowHelp()
        {
            MessageBox.Show("Управление:\n\n" +
                "- Добавить произведение в проект концерта:  F2\n" +
                "- Удалить выделенную колонку:  Del\n" +
                "- Распечатать страницу:  F12\n" +
                "   ----\n" +
                "- Двойной щелчок мыши по списку инструментов или F8\n    вызывает его редактирование\n" +
                "- Двойной щелчок мыши по пустой колонке вызывает\n    редактирование её текста\n" +
                "- Ctrl + <влево/вправо> - перемещение колонки"
                ,
                "Управление",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbDrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            ServeTopRightField(cbDrop.SelectedIndex);
            MFConstructPage();
        }
        //=======================================================
        string HeaderTextboxBuffer = String.Empty;
        private void ServeTopRightField(int fIndex)
        {
            switch (fIndex)
            {
                case 0:  //  dateTimePicker
                    {
                        dateTimePicker.Visible = true;
                        if (headerTextBox.Enabled && headerTextBox.Visible)
                        {
                            HeaderTextboxBuffer = headerTextBox.Text;
                        }
                        headerTextBox.Visible = false;
                        dateTimePicker.Focus();
                        break;
                    }
                case 1:  //  some text
                    {
                        dateTimePicker.Visible = false;
                        headerTextBox.Visible = true;
                        headerTextBox.Text = HeaderTextboxBuffer;
                        headerTextBox.Enabled = true;
                        headerTextBox.Focus();
                        break;
                    }
                case 2:  //  empty field
                    {
                        {
                            dateTimePicker.Visible = false;
                            if (headerTextBox.Enabled && headerTextBox.Visible)
                            {
                                HeaderTextboxBuffer = headerTextBox.Text;
                            }
                            headerTextBox.Visible = true;
                            headerTextBox.Enabled = false;
                            headerTextBox.Text = String.Empty;
                            this.ActiveControl = null;
                            break;
                        }
                    }
                default:
                    {
                        cbDrop.SelectedIndex = 0;
                        fIndex = 0;
                        goto case 0;
                    }
            }
        }

        private void cbDrop_DropDownClosed(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void headerTextBox_TextChanged(object sender, EventArgs e)
        {
            MFConstructPage();
        }

        private void MFPictureBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if ((e.Y > (1 + GI.ScreenDimensionRectangle.Y)) && (e.X < (1 + GI.ScreenDimensionRectangle.X)))
            {
                ShowLF(1);
            }
            else
            {
                EditEmptyColumnText(GetProjectItemPositionIndex(e.Location));
            }
            this.ActiveControl = null;
        }

        private void EditEmptyColumnText(int index)
        {
            if (index < 0 || index >= GI.ProjectList.Count || GI.ProjectList[index].Int_Value != 0) return;
            string ns = ServeEmptyColumnText(GI.ProjectList[index].String_Value);
            if (!ns.Equals(GI.ProjectList[index].String_Value))
            {
                GI.ProjectList[index].String_Value = ns;
                MFConstructPage();
            }
        }

        private void PopupAddWork_Click(object sender, EventArgs e)
        {
            ShowLF(0);
        }

        private void PopupEditInstrumentList_Click(object sender, EventArgs e)
        {
            ShowLF(1);
        }

        private void PopupDeleteWork_Click(object sender, EventArgs e)
        {
            RemoveLastListing();
        }

        private void PopupAbout_Click(object sender, EventArgs e)
        {
            ShowInfo();
        }

        private void PopupPrint_Click(object sender, EventArgs e)
        {
            GoPrinting();
        }

        private void PopupMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PopupDeleteWork.Enabled = GI.ProjectList.Count > 0;
        }

        private void MF_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.F2: AddNewColumn(0); break;
                case Keys.Delete: RemoveLastListing(); break;
                case Keys.F8: ShowLF(1); break;
                case Keys.F12: GoPrinting(); break;
                case Keys.F1: ShowHelp(); break;
                case Keys.Enter:
                    if (Emphasized >= 0 && GI.ProjectList[Emphasized].Int_Value == 0)
                    {
                        EditEmptyColumnText(Emphasized);
                    }
                    this.ActiveControl = null;
                    break;
                default: break;
            }
        }

        private void PopupNavigationHelp_Click(object sender, EventArgs e)
        {
            ShowHelp();
        }

        private void PopupExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void smAddEmptyColumn_Click(object sender, EventArgs e)
        {
            AddNewColumn(1);  //  new empty column with text
        }
        //========================================      
        private void ShowMessageWithDelegate(string Text, Action Function)
        {
            string messagetext = Text;
            Graphics gf = this.CreateGraphics();
            SizeF textlen = gf.MeasureString(messagetext, this.Font);
            gf.Dispose();

            Form modform = new Form()
            {
                StartPosition = FormStartPosition.CenterScreen,
                Width = (int)textlen.Width + (20 * 2),
                Height = (int)textlen.Height + (20 * 2),
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                ControlBox = false
            };
            Label modlabel = new Label()
            {
                Text = messagetext,
                Font = new Font(FontName, 12f, FontStyle.Italic | FontStyle.Bold),
                Dock = DockStyle.Fill,
                //Width = modform.Width,
                //Height = modform.Height,
                TextAlign = ContentAlignment.MiddleCenter,
                Visible = true
            };
            modform.Controls.Add(modlabel);
            modform.Show();
            modlabel.Refresh();
            //---
            Function();
            //---
            modform.Close();
            modlabel.Dispose();
            modform.Dispose();
        }

        private string ServeEmptyColumnText(string InputValue)
        {
            string ws = String.Empty;
            if (!String.IsNullOrEmpty(InputValue))
            {
                ws = InputValue.Trim();
            }
            int _formwidth = 288;
            int _lineheight = 18;

            Form modform = new Form()
            {
                StartPosition = FormStartPosition.CenterScreen,
                Width = _formwidth,
                Height = 28 + _lineheight * 4 + 3 * 2 + 8,
                FormBorderStyle = FormBorderStyle.FixedToolWindow
                //ControlBox = false
            };

            Label modlabel = new Label()
            {
                Top = 8,
                Left = 6,
                Height = _lineheight + 4,
                AutoSize = true,
                TextAlign = ContentAlignment.BottomLeft,
                Text = "Текст заголовка колонки:"
            };

            TextBox modtextbox = new TextBox()
            {
                Top = 8 + _lineheight + 4,
                Left = 4,
                Width = modform.ClientSize.Width - (4 * 2),
                Text = ws
            };
            modtextbox.KeyDown += ModTextBox_KeyDown;

            Button modbtnOK = new Button()
            {
                Width = 48,
                Top = 8 + _lineheight * 2 + 10,
                Left = modform.ClientSize.Width - 48 - 12,
                Text = "OK",
                DialogResult = DialogResult.OK
            };

            Button modbtnCancel = new Button()
            {
                Width = 72,
                Top = 8 + _lineheight * 2 + 10,
                Left = modbtnOK.Left - 72 - 2,
                Text = "Отмена",
                DialogResult = DialogResult.Cancel
            };

            modform.Controls.Add(modlabel);
            modform.Controls.Add(modtextbox);
            modform.Controls.Add(modbtnOK);
            modform.Controls.Add(modbtnCancel);
            modform.ActiveControl = modtextbox;

            string res = ws;
            if (modform.ShowDialog() == DialogResult.OK)
            {
                res = modtextbox.Text;
                if (!String.IsNullOrEmpty(res))
                {
                    res = res.Trim();
                }
            }
            modform.Controls.Clear();
            modbtnOK.Dispose();
            modbtnCancel.Dispose();
            modlabel.Dispose();
            modtextbox.KeyDown -= ModTextBox_KeyDown;
            modtextbox.Dispose();
            modform.Dispose();
            return res;
        }

        private void ModTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyValue)
            {
                case (int)Keys.Enter:
                    (sender as TextBox).FindForm().DialogResult = DialogResult.OK;
                    e.Handled = true;
                    break;
                case (int)Keys.Escape:
                    (sender as TextBox).FindForm().DialogResult = DialogResult.Cancel;
                    e.Handled = true;
                    break;
            }   //  switch
        }

        private void MFPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (GI.ProjectList.Count > 0 && e.Button.Equals(MouseButtons.Left))
            {
                int val = GetProjectItemPositionIndex(e.Location);
                if (val >= 0 && val < GI.ProjectList.Count)
                {
                    dragstartindex = draggingindex = val;
                    dragging = true;
                    ProjectListCopy = new List<IntStringPair>(GI.ProjectList);
                    if (val != Emphasized)
                    {
                        Emphasized = val;
                        MFConstructPage();
                    }
                }
            }
            this.ActiveControl = null;
        }

        private int GetProjectItemPositionIndex(Point p)
        {
            if (p.X < GI.ScreenDimensionRectangle.X + 2 ||
                p.Y >= GI.ScreenDimensionRectangle.Y + 2 + (GI.ScreenDimensionRectangle.Height * MaxLinesPerList))
            {
                return -1;
            }
            else
            {
                return (p.X - GI.ScreenDimensionRectangle.X - 1) / GI.ScreenDimensionRectangle.Width;
            }
        }

        private bool IsPointInPictureBounds(Point p)
        {
            Rectangle r = new Rectangle
            {
                X = 0 - MFPanel.AutoScrollPosition.X,
                Y = 0 - MFPanel.AutoScrollPosition.Y,
                Width = MFPanel.Size.Width,
                Height = MFPanel.Size.Height
            };
            return r.Contains(p);
        }
        //===============================================================================
        //===============================================================================
        //===============================================================================
        //===============================================================================
        // *** DRAGGING ROUTINES ***
        bool dragging = false;
        int dragstartindex;
        int draggingindex;
        public static List<IntStringPair> ProjectListCopy;

        private void MFPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (dragging && e.Button.Equals(MouseButtons.Left))
            {
                int val = GetProjectItemPositionIndex(e.Location);
                if (val < 0 || val >= GI.ProjectList.Count)
                {
                    CancelDragging();
                }
                dragging = false;
                this.ActiveControl = null;
            }
        }

        private void MFPictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging && e.Button.Equals(MouseButtons.Left))
            {
                if(IsPointInPictureBounds(e.Location))
                {
                    int val = GetProjectItemPositionIndex(e.Location);
                    if (val >= 0 && val < GI.ProjectList.Count)
                    {
                        if (!val.Equals(draggingindex))
                        {

                            MoveEmphasizedIndex(draggingindex, val);
                            draggingindex = val;
                        }
                    }
                }
                else
                {
                    CancelDragging();
                    dragging = false;
                    this.ActiveControl = null;
                }
            }         
        }

        private void CancelDragging()
        {
            if (ProjectListCopy.Count > 0)
            {
                GI.ProjectList = new List<IntStringPair>(ProjectListCopy);
                Emphasized = dragstartindex;
                MFConstructPage();
            }
        }
        
        //===============================================================================
        //===============================================================================
        //===============================================================================
        //===============================================================================

    }
}
