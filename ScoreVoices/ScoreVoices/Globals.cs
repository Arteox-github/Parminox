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
    public delegate void DGDrawScoreHeaderWithFrame(Graphics ScoreGraphics, string HeaderText, Rectangle HeaderRect, bool DrawFrame);
    //========================================================================
    public static class GI
    {
        public static List<ScoreItem> ScoreLibrary = new List<ScoreItem>();
        public static List<IntStringPair> ProjectList;
        public static readonly string db_name = "Parminox.db";
        public static string DBConnectionString = String.Empty;

        public static DGDrawScoreHeaderWithFrame DrawHeaderDelegate { get; set; }
        public static string CurrentDirectory;
        public static int ScreenDpiX;
        public static int ScreenDpiY;
        public static Rectangle ScreenDimensionRectangle;
        public static int WorkToShowHashcode;
        public static List<string> VoiceList = new List<string>();
        public static LibraryForm LF;
        public static WorksForm WF;
        //public static readonly string ParminoxDateTimeFormat = "yyyy.MM.dd HH:mm";
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
        //  ==============================================================
        private static void SetupSQLiteDatabase(string PathName)
        {
            DBConnectionString = $@"Data Source={PathName}; Version=3;";
            bool loadfromini = (!File.Exists(PathName));
            if (loadfromini)
            {
                try
                {
                    SQLiteConnection.CreateFile(PathName);
                }
                catch
                {
                    MessageBox.Show("Невозможно создать новую базу данных. Работа программы невозможна.",
                            "Ошибка сервера SQLite.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DBConnectionString = String.Empty;
                    return;
                }                
            }

            List<VoicePairItem> tempvoiceslist = new List<VoicePairItem>();
            using (var conn = new SQLiteConnection(DBConnectionString, true))
            {
                using (var comm = new SQLiteCommand(String.Empty, conn))
                {
                    try
                    {
                        conn.Open();  //  await
                        comm.CommandText = @"BEGIN TRANSACTION;";
                        comm.ExecuteNonQuery();

                        comm.CommandText = @"CREATE TABLE IF NOT EXISTS [WorksTable](" +
                             @"[HashCode] INTEGER  PRIMARY KEY NOT NULL, " +
                             @"[FullName] NVARCHAR(1024) NOT NULL UNIQUE, " +
                             @"[HeaderName] NVARCHAR(128)  NOT NULL, " +
                             @"[Modified] LONG);";
                        comm.ExecuteNonQuery();

                        comm.CommandText = @"CREATE TABLE IF NOT EXISTS [VoicesTable](" +
                             @"[RowPosition] INTEGER PRIMARY KEY NOT NULL," +
                             @"[VoiceName] NVARCHAR(64) NOT NULL UNIQUE);";
                        comm.ExecuteNonQuery();

                        comm.CommandText = @"CREATE TABLE IF NOT EXISTS [VoicesSetsTable](" +
                             @"[VoiceName] NVARCHAR(64) NOT NULL," +
                             @"[TextValue] NVARCHAR(64) NOT NULL," +
                             @"[HostHashCode] INT); ";
                        comm.ExecuteNonQuery();

                        if (loadfromini)
                        {
                            // Filling Works List
                            M_Params mWorksList = new M_Params(CurrentDirectory + "INI\\works.ini");
                            M_Params mSettings = new M_Params(CurrentDirectory + "INI\\settings.ini");
                            if (!mWorksList.Assigned || !mSettings.Assigned)
                            {
                                return;  //  goes to FINALLY section
                            }

                            //+++++++++++++++++++++++++++
                            //  Filling VoiceTable
                            string vname = String.Empty;
                            VoiceList.Clear();
                            for (int i = 1; ; i++)
                            {
                                vname = mSettings.GetParameter("voices", i.ToString());
                                if (String.IsNullOrEmpty(vname)) break;
                                comm.CommandText = $@"INSERT INTO [VoicesTable] " +
                                    $@"([RowPosition], [VoiceName]) " +
                                    $@"VALUES ('{i.ToString()}', '{vname}');";
                                comm.ExecuteNonQuery();
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
                            //  main circle of passing ScoreItems to DB
                            for (;;)
                            {
                                issad = mWorksList.FindNextSection(ref isec);
                                if (String.IsNullOrEmpty(issad)) break;
                                //tHashCode = GI.GenerateHashCode();
                                tHashCode = int.Parse(issad);
                                tFullName = mWorksList.GetParameter(issad, "FullName").Replace("'", "\"");
                                tHeaderName = mWorksList.GetParameter(issad, "ShortName").Replace("'", "\"");
                                tModified = Convert.ToDateTime(mWorksList.GetParameter(issad, "Modified"));

                                comm.CommandText = $@"INSERT INTO [WorksTable] " +
                                    $@"([HashCode], [FullName], [HeaderName], [Modified]) VALUES (" +
                                    $@"'{tHashCode.ToString()}', " + 
                                    $@"'{tFullName}', " +
                                    $@"'{tHeaderName}', " +
                                    $@"'{tModified.ToBinary().ToString()}');";
                                // DateTime il = DateTime.FromBinary((long)reader["Modified"]);
                                comm.ExecuteNonQuery();

                                // filling voice list
                                tempvoiceslist = DivideToVoicesArrayList(mWorksList.GetParameter(issad, "Voices"));
                                for (int i = 0; i < tempvoiceslist.Count; i++)
                                {
                                    sss = tempvoiceslist[i].VoiceName;
                                    for (;;)
                                    {
                                        //   Remove all "stars"
                                        if (sss[0] != '*') break;
                                        sss = sss.Remove(0, 1);
                                    }
                                    comm.CommandText = $@"INSERT INTO [VoicesSetsTable] " +
                                        @"([VoiceName], [TextValue], [HostHashCode]) VALUES (" +
                                        $@"'{sss}', " +
                                        $@"'{tempvoiceslist[i].VoiceText}', " +
                                        $@"'{tHashCode.ToString()}')";
                                    comm.ExecuteNonQuery();

                                    exvl = VoiceList.FindIndex(x => x.Equals(sss) || x.Equals("*" + sss));
                                    if (exvl == -1)
                                    {
                                        sss = "*" + sss;
                                        comm.CommandText = $@"INSERT INTO [VoicesTable] " +
                                            $@"([RowPosition], [VoiceName]) " +
                                            $@"VALUES ('{VoiceList.Count + 1}', '{sss}');";
                                        comm.ExecuteNonQuery();
                                        VoiceList.Add(sss);
                                    }
                                }
                                ScoreLibrary.Add(new ScoreItem(tHashCode, tFullName, tHeaderName, tempvoiceslist));
                                tempvoiceslist.Clear();
                            }  //  main circle of passing ScoreItems to DB
                            //++++++++++++++++++++++++++++++++++++++++++++++++++++
                            comm.CommandText = @"COMMIT TRANSACTION";
                            comm.ExecuteNonQuery();
                            //++++++++++++++++++++++++++++++++++++++++++++++++++++
                        }
                        //=============================================
                        else    //  load from DB
                        {
                            //  retrieveing scoreitems from DB to 
                            comm.CommandText = @"SELECT [HashCode], [FullName], [HeaderName] " +
                                @"FROM [WorksTable] " +
                                @"ORDER BY [FullName];";
                            using (SQLiteDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ScoreLibrary.Add(new ScoreItem(Convert.ToInt32(reader[0]), reader[1] as string, reader[2] as string, new List<VoicePairItem>()));
                                }
                            }
                            //  retrieveing Voice Sets Data from DB to ScoreLibrary
                            for (int ilib = 0; ilib < ScoreLibrary.Count; ilib++)
                            {
                                comm.CommandText = @"SELECT [VoiceName], [TextValue] " +
                                @"FROM [VoicesSetsTable] " +
                                $@"WHERE [HostHashCode] = '{ScoreLibrary[ilib].HashCode.ToString()}';";
                                using (SQLiteDataReader reader = comm.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        ScoreLibrary[ilib].VoicePairList.Add(new VoicePairItem(reader[0] as string, reader[1] as string));
                                    }
                                }
                            }
                            //  retrieveing voicelist from DB
                            VoiceList.Clear();
                            comm.CommandText = @"SELECT [VoiceName]  FROM [VoicesTable] ORDER BY [RowPosition];";
                            using (SQLiteDataReader reader = comm.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    VoiceList.Add(reader["VoiceName"].ToString());
                                }
                            }
                        }
                    }
                    catch
                    {
                        comm.CommandText = @"ROLLBACK TRANSACTION;";
                        comm.ExecuteNonQuery();
                        DBConnectionString = String.Empty;
                        MessageBox.Show("Ошибка в функционировании базы данных. Работа программы невозможна.",
                            "Ошибка доступа к базе данных.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (ScoreLibrary.Count > 0) ScoreLibrary.Clear();
                        if (VoiceList.Count > 0) VoiceList.Clear();
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
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
                for (;;)
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
            if (String.IsNullOrEmpty(DBConnectionString)) return false;
            ScoreItem b_item = new ScoreItem(baseitem);
            bool no_error = false;
            using (var conn = new SQLiteConnection(DBConnectionString))
            {
                using (var comm = new SQLiteCommand(String.Empty, conn))
                {
                    DateTime _modified = DateTime.Now;
                    try
                    {
                        conn.Open();
                        comm.CommandText = @"BEGIN TRANSACTION;";
                        comm.ExecuteNonQuery();
                        string s_hashcode = b_item.HashCode.ToString();
                        int w_index = FindScoreByHashcode(b_item.HashCode);

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

                                        comm.CommandText = @"UPDATE [WorksTable] SET " +
                                        s_fullname +
                                        s_headername +
                                        $@"[Modified] = '{_modified.ToString(StandardDateTimeFormat)} '" +
                                        @" WHERE rowid IN (SELECT rowid FROM [WorksTable] " +
                                        $@"WHERE [HashCode] = '{s_hashcode}' LIMIT 1);";
                                        comm.ExecuteNonQuery();

                                        if (b_item.VoicePairList.Count > 0)
                                        {
                                            comm.CommandText = @"DELETE FROM [VoicesSetsTable] " +
                                                 $@"WHERE [HostHashCode] = '{s_hashcode}';";
                                            comm.ExecuteNonQuery();
                                            for (int vsi = 0; vsi < b_item.VoicePairList.Count; vsi++)
                                            {
                                                comm.CommandText = @"INSERT INTO [VoicesSetsTable] " +
                                                      $@"([VoiceName], [TextValue], [HostHashCode]) VALUES (" +
                                                $@"'{b_item.VoicePairList[vsi].VoiceName}', " +
                                                $@"'{b_item.VoicePairList[vsi].VoiceText}', " +
                                                $@"'{s_hashcode}');";
                                                comm.ExecuteNonQuery();
                                            }
                                            //++++++++++++++++++++
                                            ScoreLibrary[w_index].VoicePairList = b_item.VoicePairList;
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
                                    int _hashcode = GI.GenerateHashCode();
                                    comm.CommandText = @"INSERT INTO [WorksTable] " +
                                                @"([HashCode], [FullName], [HeaderName], [Modified]) VALUES " +
                                                $@"('{_hashcode.ToString()}', " +
                                                $@"'{b_item.FullName}', " +
                                                $@"'{b_item.HeaderName}', " +
                                                $@"'{_modified.ToString(StandardDateTimeFormat)}');";
                                    comm.ExecuteNonQuery();
                                    if (b_item.VoicePairList.Count > 0)
                                    {
                                        for (int i_ins = 0; i_ins < b_item.VoicePairList.Count; i_ins++)
                                        {
                                            comm.CommandText = @"INSERT INTO [VoicesSetsTable] " +
                                                @"([VoiceName], [TextValue], [HostHashCode]) VALUES (" +
                                                $@"'{b_item.VoicePairList[i_ins].VoiceName}', " +
                                                $@"'{b_item.VoicePairList[i_ins].VoiceText}', " +
                                                $@"'{_hashcode.ToString()}')";
                                            comm.ExecuteNonQuery();
                                        }
                                    }
                                    ScoreLibrary.Add(new ScoreItem(_hashcode, b_item.FullName, b_item.HeaderName, b_item.VoicePairList));
                                    break;
                                }
                            case 2:    //  Delete
                                {
                                    comm.CommandText = @"DELETE FROM [WorksTable] " +
                                        @"WHERE rowid IN (SELECT rowid FROM [WorksTable] " +
                                        $@"WHERE [HashCode] = '{s_hashcode}' LIMIT 1);";
                                    comm.ExecuteNonQuery();

                                    comm.CommandText = @"DELETE FROM [VoicesSetsTable] " +
                                        $@"WHERE [HostHashCode] = '{s_hashcode}';";
                                    comm.ExecuteNonQuery();
                                    if (w_index >= 0)
                                    {
                                        ScoreLibrary.RemoveAt(w_index);
                                        RemovePhantomProjectItems();
                                    }
                                    break;
                                }
                        }   //  switch
                        comm.CommandText = @"COMMIT TRANSACTION;";
                        comm.ExecuteNonQuery();
                        no_error = true;
                    }   //  try
                    catch
                    {
                        comm.CommandText = @"ROLLBACK TRANSACTION;";
                        comm.ExecuteNonQuery();
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
            return no_error;
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
            if (String.IsNullOrEmpty(DBConnectionString)) return false;
            string _init_value = init_value;
            string _new_value = new_value;
            bool no_error = true;
            bool is_removing = (String.IsNullOrEmpty(new_value));
            using (var conn = new SQLiteConnection(DBConnectionString))
            {
                using (var comm = new SQLiteCommand(String.Empty, conn))
                {
                    try
                    {
                        conn.Open();
                        //  updating instrument name
                        comm.CommandText = @"BEGIN TRANSACTION;";
                        comm.ExecuteNonQuery();
                        //++++++++++++++++++++++++++++++++++++++++++++++
                        //    updating instrument name throughout VoicesSetTable
                        //++++++++++++++++++++++++++++++++++++++++++++++
                        string csstr = (is_removing) ?
                            @"DELETE FROM [VoicesTable]"
                            :
                            $@"UPDATE [VoicesTable] SET [VoiceName] = '{_new_value}'";
                        comm.CommandText = csstr + @" WHERE rowid IN (SELECT rowid FROM [VoicesTable] " +
                            $@"WHERE [VoiceName] = '{_init_value}' LIMIT 1);";
                        comm.ExecuteNonQuery();

                        comm.CommandText = @"COMMIT TRANSACTION";
                        comm.ExecuteNonQuery();
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
                    catch
                    {
                        comm.CommandText = @"ROLLBACK TRANSACTION;";
                        comm.ExecuteNonQuery();
                        no_error = false;
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
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

        public static bool FlushVoiceList(List<string> listcopy)
        {
            if (String.IsNullOrEmpty(DBConnectionString)) return false;
            bool no_error = true;
            using (var conn = new SQLiteConnection(DBConnectionString))
            {
                using (var comm = new SQLiteCommand(String.Empty, conn))
                {
                    try
                    {
                        conn.Open();
                        comm.CommandText = @"BEGIN TRANSACTION;";
                        comm.ExecuteNonQuery();

                        comm.CommandText = @"DROP TABLE [VoicesTable];";
                        comm.ExecuteNonQuery();

                        comm.CommandText = @"CREATE TABLE [VoicesTable](" +
                             @"[RowPosition] INTEGER PRIMARY KEY NOT NULL," +
                             @"[VoiceName] NVARCHAR(64) NOT NULL UNIQUE);";
                        comm.ExecuteNonQuery();

                        for (int i = 0; i < listcopy.Count; i++)
                        {
                            comm.CommandText = @"INSERT INTO [VoicesTable] (RowPosition, VoiceName) " +
                                $@"VALUES ({i + 1}, '{listcopy[i]}');";
                            comm.ExecuteNonQuery();
                        }

                        comm.CommandText = @"COMMIT TRANSACTION";
                        comm.ExecuteNonQuery();

                        VoiceList.Clear();
                        for (int ivl = 0; ivl < listcopy.Count; ivl++)
                        {
                            VoiceList.Add(listcopy[ivl]);
                        }
                        //===============================
                    }   //  try
                    catch
                    {
                        comm.CommandText = @"ROLLBACK TRANSACTION;";
                        comm.ExecuteNonQuery();
                        no_error = false;
                    }
                    finally
                    {
                        if (conn.State == System.Data.ConnectionState.Open)
                        {
                            conn.Close();
                        }
                    }
                }
            }
            return no_error;
        }

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
           */


        /*


    M_Params mSettings = new M_Params(SettingsFilename);
    mSettings.RemoveSection("voices");
    for (int i = 0; i < VoiceList.Count; i++) mSettings.StoreParameter("voices", (i + 1).ToString(), VoiceList[i]);
    mSettings.FlushFile();
}
    */

        //===============================================================================================

        /*
        public static void SortScoreItemList()
        {
            ScoreLibrary.Sort((x1, x2) => x1.FullName.CompareTo(x2.FullName));
            //ScoreItemList = ScoreItemList.OrderBy(x => x.FullName).ToList();  //  +   using System.Linq;
        }
        */
    }
}