using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;   //System.Data.SqlClient;

namespace Parminox
{
    public class VoicePairItem
    {
        public string VoiceName { get; set; }
        public string VoiceText { get; set; }

        public VoicePairItem()
        {
            VoiceName = String.Empty;
            VoiceText = String.Empty;
        }

        public VoicePairItem(string voicename, string voicetext)
        {
            VoiceName = voicename;
            VoiceText = voicetext;
        }
    }

    public class ScoreItem
    {
        public int HashCode { get; set; }
        public string FullName { get; set; }
        public string HeaderName { get; set; }
        private List<VoicePairItem> _voicepairlist;
        
        public List<VoicePairItem> VoicePairList
        {
            get { return _voicepairlist; }
            set
            {
                _voicepairlist = (value == null) ? new List<VoicePairItem>() : new List<VoicePairItem>(value);
            }
        }

        public ScoreItem()  
        {
            this.HashCode = 0;
            this.FullName = String.Empty;
            this.HeaderName = String.Empty;
            this.VoicePairList = new List<VoicePairItem>();
        }

        public ScoreItem(int hashcode, string fullname, string headername, List<VoicePairItem> voicedatalist)
        {
            this.HashCode = hashcode;
            this.FullName = fullname;
            this.VoicePairList = new List<VoicePairItem>(voicedatalist);
            this.HeaderName = headername;
        }

        public ScoreItem(ScoreItem si)
        {
            this.HashCode = si.HashCode;
            this.FullName = si.FullName;
            this.VoicePairList = new List<VoicePairItem>(si.VoicePairList);
            this.HeaderName = si.HeaderName;
        }

        public void Initialize()
        {
            this.HashCode = 0;
            this.FullName = HeaderName = String.Empty;
            this._voicepairlist = new List<VoicePairItem>();
        }
    }
    //========================================================================
    public class IntStringPair
    {
        public int Int_Value { get; set; }
        public string String_Value { get; set; }

        public IntStringPair()
        {
            Int_Value = 0;
            String_Value = null;
        }

        public IntStringPair(int int_value, string string_value)
        {
            Int_Value = int_value;
            String_Value = string_value;
        }

        public IntStringPair(IntStringPair value)
        {
            Int_Value = value.Int_Value;
            String_Value = value.String_Value;
        }
    }
    //========================================================================
    //========================================================================
    public delegate void DGDrawScoreHeaderWithFrame(Graphics ScoreGraphics, string HeaderText, Rectangle HeaderRect, bool DrawFrame);
    //========================================================================
    public static class GI
    {
        public static List<ScoreItem> ScoreLibrary = new List<ScoreItem>();
        public static List<IntStringPair> ProjectList;
        public static readonly string db_name = "Parminox.db";
        //public static string DBConnectionString = String.Empty;
        public static SQL_Class M_Connection = null;

        public static DGDrawScoreHeaderWithFrame DrawHeaderDelegate { get; set; }
        public static string CurrentDirectory;
        public static int ScreenDpiX;
        public static int ScreenDpiY;
        public static Rectangle ScreenDimensionRectangle;
        public static int WorkToShowHashcode;
        public static List<string> VoiceList = new List<string>();
        public static LibraryForm LF;
        public static WorksForm WF;
        public static readonly string StandardDateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        //========================================================================================
        public static void InitializeComponents()
        {
            CurrentDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\";  //  Application.StartupPath;
            ScoreLibrary = new List<ScoreItem>();
            ProjectList = new List<IntStringPair>();
            SetupSQLiteDatabase(CurrentDirectory + db_name);
        }
        //  ==============================================================
        //  ==============================================================
        //  ==============================================================
        private static string GetErrorText(int error_code)
        {
            switch (error_code)
            {
                case 159: return "Ошибка создания базы данных";
                case 114: return "Ошибка OpenConnection";
                case 110: return "Ошибка BeginTransaction";
                case 111: return "Ошибка CommitTransaction";
                case 112: return "Ошибка RollbackTransaction";
                case 128: return "Ошибка ExecuteNonQuery";
                case 91: return "Ошибка ExecuteReader";
                default: return "Неизвестная ошибка";
            }
        }
        //  ==============================================================
        private static void SetupSQLiteDatabase(string PathName)
        {
            bool db_missing = (!File.Exists(PathName));
            try
            {
                M_Connection = new SQL_Class(PathName);
            }
            catch (Exception)
            {
                MessageBox.Show("Невозможно создать новую базу данных. Работа программы невозможна.",
                        "Ошибка сервера SQLite.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<VoicePairItem> tempvoiceslist = new List<VoicePairItem>();
            try
            {
                M_Connection.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS [WorksTable](" +
                             @"[HashCode] INTEGER  PRIMARY KEY NOT NULL, " +
                             @"[FullName] NVARCHAR(1024) NOT NULL UNIQUE, " +
                             @"[HeaderName] NVARCHAR(128)  NOT NULL, " +
                             @"[Modified] LONG);", true, false, false);
                M_Connection.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS [VoicesTable](" +
                             @"[RowPosition] INTEGER PRIMARY KEY NOT NULL," +
                             @"[VoiceName] NVARCHAR(64) NOT NULL UNIQUE);", false, false, false);
                M_Connection.ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS [VoicesSetsTable](" +
                             @"[VoiceName] NVARCHAR(64) NOT NULL," +
                             @"[TextValue] NVARCHAR(64) NOT NULL," +
                             @"[HostHashCode] INT); ", false, true, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка {ex.Message}.",
                            "Ошибка создания таблиц в базе SQLite.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //---------------------------------
            if (db_missing)
            {
                // Filling Works List
                M_Params mWorksList = new M_Params(CurrentDirectory + "INI\\works.ini");
                M_Params mSettings = new M_Params(CurrentDirectory + "INI\\settings.ini");
                if (!mWorksList.Assigned || !mSettings.Assigned)
                {
                    MessageBox.Show("Источники данных не найдены.",
                            "Инициализация программы", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //+++++++++++++++++++++++++++
                //  Filling VoiceTable
                string vname = String.Empty;
                VoiceList.Clear();
                try
                {
                    for (int i = 1; ; i++)
                    {
                        vname = mSettings.GetParameter("voices", i.ToString());
                        if (String.IsNullOrEmpty(vname)) break;
                        M_Connection.ExecuteNonQuery(
                            $@"INSERT INTO [VoicesTable] " +
                            $@"([RowPosition], [VoiceName]) " +
                            $@"VALUES ('{i.ToString()}', '{vname}');", true, false, false);
                        VoiceList.Add(vname);
                    }
                    //+++++++++++++++++++++++++++
                    int isec = 0;
                    string issad = String.Empty;
                    string ModifiedTime = String.Empty;
                    string tFullName, tHeaderName, tVoices = String.Empty;
                    int tHashCode = 0;
                    DateTime tModified = DateTime.Now;
                    ScoreLibrary.Clear();
                    int exvl;
                    string sss = null;
                    //  main loop of passing ScoreItems to DB
                    for ( ; ; )
                    {
                        issad = mWorksList.FindNextSection(ref isec);
                        if (String.IsNullOrEmpty(issad)) break;
                        //tHashCode = GI.GenerateHashCode();
                        tHashCode = int.Parse(issad);
                        tFullName = mWorksList.GetParameter(issad, "FullName").Replace("'", "\"");   //  ' to "
                        tHeaderName = mWorksList.GetParameter(issad, "ShortName").Replace("'", "\"");
                        tModified = Convert.ToDateTime(mWorksList.GetParameter(issad, "Modified"));
                        M_Connection.ExecuteNonQuery(
                            $@"INSERT INTO [WorksTable] " +
                            $@"([HashCode], [FullName], [HeaderName], [Modified]) VALUES (" +
                            $@"'{tHashCode.ToString()}', " +
                            $@"'{tFullName}', " +
                            $@"'{tHeaderName}', " +
                            $@"'{tModified.ToBinary().ToString()}');", true, false, false);
                        // DateTime il = DateTime.FromBinary((long)reader["Modified"]);
                        //---------------------
                        // filling voice list
                        tempvoiceslist = DivideToVoicesArrayList(mWorksList.GetParameter(issad, "Voices"));
                        for (int i = 0; i < tempvoiceslist.Count; i++)
                        {
                            sss = tempvoiceslist[i].VoiceName;
                            for ( ; ; )
                            {
                                //   Remove leading "stars"
                                if (sss[0] != '*') break;
                                sss = sss.Remove(0, 1);
                            }
                            M_Connection.ExecuteNonQuery(
                                 $@"INSERT INTO [VoicesSetsTable] " +
                                @"([VoiceName], [TextValue], [HostHashCode]) VALUES (" +
                                $@"'{sss}', " +
                                $@"'{tempvoiceslist[i].VoiceText}', " +
                                $@"'{tHashCode.ToString()}')", true, false, false);
                            exvl = VoiceList.FindIndex(x => x.Equals(sss) || x.Equals("*" + sss));
                            if (exvl == -1)
                            {
                                sss = "*" + sss;
                                M_Connection.ExecuteNonQuery(
                                     $@"INSERT INTO [VoicesTable] " +
                                    $@"([RowPosition], [VoiceName]) " +
                                    $@"VALUES ('{VoiceList.Count + 1}', '{sss}');", true, false, false);
                                VoiceList.Add(sss);
                            }
                        }
                        ScoreLibrary.Add(new ScoreItem(tHashCode, tFullName, tHeaderName, tempvoiceslist));
                        tempvoiceslist.Clear();
                    }  //  main loop of passing ScoreItems to DB
                       //++++++++++++++++++++++++++++++++++++++++++++++++++++
                    M_Connection.CommitTransaction();
                    M_Connection.CloseConnection();
                    //++++++++++++++++++++++++++++++++++++++++++++++++++++
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка {ex.Message}.",
                            "Ошибка перенесения информации в базу данных из INI-файлов.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }                
            }
            //=============================================
            else    //  load from DB
            {
                //  retrieving scoreitems from DB
                try
                {
                    M_Connection.ExecuteReaderWithCallBackAction(
                    @"SELECT [HashCode], [FullName], [HeaderName] " +
                    @"FROM [WorksTable] " +
                    @"ORDER BY [FullName];", true,
                    (r) =>
                    {
                        while (r.Read())
                        {
                            ScoreLibrary.Add(new ScoreItem(Convert.ToInt32(r[0]), r[1] as string, r[2] as string, new List<VoicePairItem>()));
                        }
                    });
                    //  retrieving Voice Sets Data from DB to ScoreLibrary
                    for (int ilib = 0; ilib < ScoreLibrary.Count; ilib++)
                    {
                        M_Connection.ExecuteReaderWithCallBackAction(
                            @"SELECT [VoiceName], [TextValue] " +
                            @"FROM [VoicesSetsTable] " +
                            $@"WHERE [HostHashCode] = '{ScoreLibrary[ilib].HashCode.ToString()}';", true,
                            (r) =>
                            {
                                while (r.Read())
                                {
                                    ScoreLibrary[ilib].VoicePairList.Add(new VoicePairItem(r[0] as string, r[1] as string));
                                }
                            });
                    }
                    //  retrieving voicelist from DB
                    VoiceList.Clear();
                    M_Connection.ExecuteReaderWithCallBackAction(
                         @"SELECT [VoiceName]  FROM [VoicesTable] ORDER BY [RowPosition];", true,
                         (r) =>
                         {
                             while (r.Read())
                             {
                                 VoiceList.Add(r["VoiceName"].ToString());
                             }
                         });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка { ex.Message}. Работа программы невозможна.",
                           "Ошибка чтения данных из базы данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (ScoreLibrary.Count > 0) ScoreLibrary.Clear();
                    if (VoiceList.Count > 0) VoiceList.Clear();
                }
            }
        }
        //============================================================
        //============================================================
        //============================================================
        //============================================================
        public static int GenerateHashCode()
        {
            using (System.Security.Cryptography.RNGCryptoServiceProvider rg = new System.Security.Cryptography.RNGCryptoServiceProvider())
            {
                byte[] rno = new byte[4];
                int result;
                int i;
                for ( ; ; )
                {
                    rg.GetBytes(rno);
                    result = BitConverter.ToInt32(rno, 0);  // & 0x7FFFFFFF;
                    if (result == 0) continue;
                    i = ScoreLibrary.FindIndex(x => x.HashCode == result);
                    if (i < 0) break;
                }
                return result;
            }
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public static bool ProcessScoreItemToDatabase(ScoreItem baseitem, int mode)
        {
            if (!M_Connection.Valid) return false;
            ScoreItem b_item = new ScoreItem(baseitem);
            DateTime _modified = DateTime.Now;
            string s_hashcode = b_item.HashCode.ToString();
            int w_index = FindScoreByHashcode(b_item.HashCode);
            string ExceptionMessageBoxHeader = "Ошибка обновления данных произведения в базе данных.";
            try
            {
                switch (mode)
                {
                    case 0:      //  Update
                        {
                            string s_fullname = String.Empty;
                            string s_headername = String.Empty;
                            if (w_index >= 0)
                            {
                                if (!String.IsNullOrEmpty(b_item.FullName))
                                {
                                    s_fullname = $@"[FullName] = '{b_item.FullName}', ";
                                }
                                if (!String.IsNullOrEmpty(b_item.HeaderName))
                                {
                                    s_headername = $@"[HeaderName] = '{b_item.HeaderName}', ";
                                }
                                M_Connection.ExecuteNonQuery(
                                     @"UPDATE [WorksTable] SET " +
                                     s_fullname +
                                     s_headername +
                                     $@"[Modified] = '{_modified.ToString(StandardDateTimeFormat)} '" +
                                     @" WHERE rowid IN (SELECT rowid FROM [WorksTable] " +
                                     $@"WHERE [HashCode] = '{s_hashcode}' LIMIT 1);", true, false, false);
                                if (b_item.VoicePairList.Count > 0)   //  Delete old >> Insert new
                                {
                                    M_Connection.ExecuteNonQuery(
                                         @"DELETE FROM [VoicesSetsTable] " +
                                         $@"WHERE [HostHashCode] = '{s_hashcode}';", true, false, false);
                                    for (int vsi = 0; vsi < b_item.VoicePairList.Count; vsi++)
                                    {
                                        M_Connection.ExecuteNonQuery(
                                             @"INSERT INTO [VoicesSetsTable] " +
                                              $@"([VoiceName], [TextValue], [HostHashCode]) VALUES (" +
                                              $@"'{b_item.VoicePairList[vsi].VoiceName}', " +
                                              $@"'{b_item.VoicePairList[vsi].VoiceText}', " +
                                              $@"'{s_hashcode}');", true, false, false);
                                        //++++++++++++++++++++
                                        ScoreLibrary[w_index].VoicePairList = b_item.VoicePairList;
                                    }
                                }
                                if (!String.IsNullOrEmpty(b_item.FullName))
                                {
                                    ScoreLibrary[w_index].FullName = b_item.FullName;
                                }
                                if (!String.IsNullOrEmpty(b_item.HeaderName))
                                {
                                    ScoreLibrary[w_index].HeaderName = b_item.HeaderName;
                                }
                            }
                            break;
                        }
                    case 1:     //  Insert
                        {
                            ExceptionMessageBoxHeader = "Ошибка добавления произведения в базу данных.";
                            int _hashcode = GI.GenerateHashCode();
                            M_Connection.ExecuteNonQuery(
                                @"INSERT INTO [WorksTable] " +
                                        @"([HashCode], [FullName], [HeaderName], [Modified]) VALUES " +
                                        $@"('{_hashcode.ToString()}', " +
                                        $@"'{b_item.FullName}', " +
                                        $@"'{b_item.HeaderName}', " +
                                        $@"'{_modified.ToString(StandardDateTimeFormat)}');", true, false, false);
                            if (b_item.VoicePairList.Count > 0)
                            {
                                for (int i_ins = 0; i_ins < b_item.VoicePairList.Count; i_ins++)
                                {
                                    M_Connection.ExecuteNonQuery(
                                         @"INSERT INTO [VoicesSetsTable] " +
                                        @"([VoiceName], [TextValue], [HostHashCode]) VALUES (" +
                                        $@"'{b_item.VoicePairList[i_ins].VoiceName}', " +
                                        $@"'{b_item.VoicePairList[i_ins].VoiceText}', " +
                                        $@"'{_hashcode.ToString()}')", true, false, false);
                                }
                            }
                            ScoreLibrary.Add(new ScoreItem(_hashcode, b_item.FullName, b_item.HeaderName, b_item.VoicePairList));
                            break;
                        }
                    case 2:    //  Delete
                        {
                            ExceptionMessageBoxHeader = "Ошибка удаления произведения из базы данных.";
                            M_Connection.ExecuteNonQuery(
                                @"DELETE FROM [WorksTable] " +
                                @"WHERE rowid IN (SELECT rowid FROM [WorksTable] " +
                                $@"WHERE [HashCode] = '{s_hashcode}' LIMIT 1);", true, false, false);
                            M_Connection.ExecuteNonQuery(
                                @"DELETE FROM [VoicesSetsTable] " +
                                $@"WHERE [HostHashCode] = '{s_hashcode}';", false, false, false);
                            if (w_index >= 0)
                            {
                                ScoreLibrary.RemoveAt(w_index);
                                RemovePhantomProjectItems();
                            }
                            break;
                        }
                }   //  switch
                //M_Connection.CommitTransaction();
                M_Connection.CloseConnection();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка { ex.Message}. Работа программы остановлена.",
                          ExceptionMessageBoxHeader, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //==============================================================================
        public static void RemovePhantomProjectItems()
        {
            int gstroke;     // removing possibly deleted items
            for (int dil = GI.ProjectList.Count - 1; dil >= 0; dil--)
            {
                gstroke = GI.ProjectList[dil].Int_Value;
                if (gstroke == 0) continue;
                gstroke = GI.ScoreLibrary.FindIndex(x => x.HashCode == gstroke);    // GI.FindScoreByHashcode(gstroke);
                if (gstroke < 0) GI.ProjectList.RemoveAt(dil);
            }
            return;
        }
        //==============================================================================
        //==============================================================================
        public static bool RenameDeleteInstrument(string init_value, string new_value)
        {
            if (!M_Connection.Valid ) return false;
            string _init_value = init_value;
            string _new_value = new_value;
            bool no_error = true;
            bool is_removing = (String.IsNullOrEmpty(new_value));

            try
            {
                //  updating instrument name
                //++++++++++++++++++++++++++++++++++++++++++++++
                //    updating instrument name throughout VoicesSetTable
                //++++++++++++++++++++++++++++++++++++++++++++++
                string csstr = (is_removing) ?
                    @"DELETE FROM [VoicesTable]" : $@"UPDATE [VoicesTable] SET [VoiceName] = '{_new_value}'";
                M_Connection.ExecuteNonQuery(
                     csstr + @" WHERE rowid IN (SELECT rowid FROM [VoicesTable] " +
                    $@"WHERE [VoiceName] = '{_init_value}' LIMIT 1);", true, true, true);
                // "COMMIT TRANSACTION" + "BEGIN TRANSACTION;";;
                //++++++++++++++++++++++++++++++++++++++++++++++
                // updating Instrument List
                for (int i = 0; i < VoiceList.Count; i++)
                {
                    if (!_init_value.Equals(VoiceList[i])) continue;
                    if (is_removing)
                    {
                        VoiceList.RemoveAt(i);
                    }
                    else
                    {
                        VoiceList[i] = _new_value;
                    }
                    break;
                }
                // updating instrument name through ScoreLibrary
                if (!is_removing)
                {
                    for (int iic = 0; iic < ScoreLibrary.Count; iic++)
                    {
                        for (int ivlc = 0; ivlc < ScoreLibrary[iic].VoicePairList.Count; ivlc++)
                        {
                            if (!_init_value.Equals(ScoreLibrary[iic].VoicePairList[ivlc].VoiceName)) continue;
                            ScoreLibrary[iic].VoicePairList[ivlc].VoiceName = _new_value;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка { ex.Message}. Работа программы приостановлена.",
                           "Ошибка обновления списка инструментов в базе данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                no_error = false;
            }
            return no_error;
        }
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public static int FindScoreByFullname(string fullname)
        {
            return ScoreLibrary.FindIndex(x => x.FullName.Equals(fullname));
        }

        public static int FindScoreByHashcode(int hashcode)
        {
            return ScoreLibrary.FindIndex(x => x.HashCode == hashcode);
        }
        //==================================================================
        public static bool FlushVoiceList(List<string> listcopy)
        {
            if (!M_Connection.Valid) return false;
            try
            {
                //  BEGIN TRANSACTION + COMMIT TRANSACTION
                M_Connection.ExecuteNonQuery(@"DROP TABLE [VoicesTable];", true, false, false);
                M_Connection.ExecuteNonQuery(
                    @"CREATE TABLE [VoicesTable](" +
                     @"[RowPosition] INTEGER PRIMARY KEY NOT NULL," +
                     @"[VoiceName] NVARCHAR(64) NOT NULL UNIQUE);", false, false, false);
                VoiceList.Clear();

                if (listcopy.Count > 0)
                {
                    for (int i = 0; i < listcopy.Count; i++)
                    {
                        M_Connection.ExecuteNonQuery(
                             @"INSERT INTO [VoicesTable] (RowPosition, VoiceName) " +
                            $@"VALUES ({i + 1}, '{listcopy[i]}');", false, true, true);
                    }
                    for (int ivl = 0; ivl < listcopy.Count; ivl++)
                    {
                        VoiceList.Add(listcopy[ivl]);
                    }
                }
                //M_Connection.CommitTransaction();
                M_Connection.CloseConnection();
                return true;
                //===============================
            }   //  try
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка { ex.Message}. Работа программы приостановлена.",
                           "Ошибка сохранения списка инструментов в базе данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        //==================================================================
        public static List<VoicePairItem> DivideToVoicesArrayList(string SourceString)   // List<string> VoiceName, List<string> VoiceText)
        {
            List<VoicePairItem> pairs = new List<VoicePairItem>();
            int iposition = 0;
            VoicePairItem tempit = null;

            for (; iposition < SourceString.Length;)
            {
                tempit = GetListPair(SourceString, ref iposition);
                if (tempit != null)
                {
                    pairs.Add(new VoicePairItem(((VoicePairItem)tempit).VoiceName, ((VoicePairItem)tempit).VoiceText));
                }
            }
            return pairs;
        }
        //==================================================================
        public static VoicePairItem GetListPair(string gpivis, ref int ipos)
        {
            string gpcoms1 = String.Empty;
            string gpcoms2 = String.Empty;
            char ich;
            for (; ipos < gpivis.Length;)
            {
                ich = gpivis[ipos];
                ipos++;
                if (ich == '>') break;
                if (ich == ';') return (null);
                gpcoms1 = gpcoms1 + ich;
            }
            if (String.IsNullOrEmpty(gpcoms1)) return (null);
            for (; ipos < gpivis.Length;)
            {
                ich = gpivis[ipos];
                ipos++;
                if (ich == ';') break;
                gpcoms2 = gpcoms2 + ich;
            }
            if (String.IsNullOrEmpty(gpcoms2)) return (null);
            return new VoicePairItem(gpcoms1, gpcoms2);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //----------------------------ARCHIEVE---------------------------------------------------
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        /*
       public static void FlushScoreItemsListToINI()
       {
           M_Params mWorksList = new M_Params(WorksFilename);
           mWorksList.ClearAll();
           string whcode;
           for (int i = 0; i < GI.ScoreLibrary.Count; i++)
           {
               whcode = GI.ScoreLibrary[i].HashCode;
               mWorksList.StoreParameter(whcode, "FullName", GI.ScoreLibrary[i].FullName);
               mWorksList.StoreParameter(whcode, "ShortName", GI.ScoreLibrary[i].HeaderName);
               mWorksList.StoreParameter(whcode, "Voices", GI.ScoreLibrary[i].Voices);
               mWorksList.StoreParameter(whcode, "Modified", GI.ScoreLibrary[i].Modified.ToString(StandardDateTimeFormat));
           }
           mWorksList.FlushFile();
       }
       //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        M_Params mSettings = new M_Params(SettingsFilename);
        mSettings.RemoveSection("voices");
        for (int i = 0; i < VoiceList.Count; i++) mSettings.StoreParameter("voices", (i + 1).ToString(), VoiceList[i]);
        mSettings.FlushFile();
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public static void SortScoreItemList()
        {
            ScoreLibrary.Sort((x1, x2) => x1.FullName.CompareTo(x2.FullName));
            //ScoreItemList = ScoreItemList.OrderBy(x => x.FullName).ToList();  //  +   using System.Linq;
        }
        */
    }
}