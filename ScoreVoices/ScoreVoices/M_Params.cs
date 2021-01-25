using System;
using System.Collections.Generic;
using System.IO;
//using System.Text;

namespace Parminox  // M_ParamsNamespace
{
    public class M_Params
    {
        private List<string> ParamsArray = new List<string>();
        private string p_name = String.Empty;
        private string p_body = String.Empty;
        private bool mModified = false;
        private bool mAssigned = false;
        private int NewSectionLine = -1;
        private string ParamsFilename;
        //===========================================
        //        private static bool isSectionj(string s_line)
        //        {
        //            if ((s_line[0] == '[') & (s_line[s_line.Length - 1] == ']')) return (true); else return (false); 
        //        }
        //===========================================
        public bool Assigned { get { return mAssigned; } }
        public bool Modified { get { return mModified; } }
        //===========================================
        //===========================================
        //===========================================
        public M_Params(string FileName)
        {
            p_name = String.Empty;
            p_body = String.Empty;
            mModified = false;
            AssignFile(FileName);
        }
        ~M_Params()
        {
            ParamsArray.Clear();
        }
        private void DivideToParams(string s_line)
        {
            p_name = String.Empty;
            p_body = String.Empty;
            int i = 0;
            char ich;
            for (; i < s_line.Length; i++)
            {
                ich = s_line[i];
                if (ich == '=') break;
                p_name = p_name + ich;
            }
            i++;
            for (; i < s_line.Length; i++) p_body = p_body + s_line[i];
        }
        //===========================================
        public void CopyToArray(List<string> DestStringArray)
        {
            DestStringArray.Clear();
            for ( int i = 0; i < ParamsArray.Count; i++) DestStringArray.Add(ParamsArray[i]);
        }

        public void CopyFromArray(List<string> SourceStringArray)
        {
            ParamsArray.Clear();
            for (int i = 0; i < SourceStringArray.Count; i++) ParamsArray.Add(SourceStringArray[i]);
            mModified = true;
        }
        //===========================================
        public string FindNextSection(ref int StartPosition)
        {
            return FindNextSection(ref ParamsArray, ref StartPosition);
        }

        public string FindNextSection(ref List<string> PP_Array, ref int StartPositionb)
        {
            string gss = String.Empty;
            for (int i = StartPositionb; i < PP_Array.Count; i++)
            {
                gss = PP_Array[i];
                if ((gss[0] == '[') & (gss[gss.Length - 1] == ']'))
                {
                    StartPositionb = i + 1;
                    gss = gss.Remove(0,1);
                    gss = gss.Remove(gss.Length - 1,1);
                    return (gss);
                }
            }
            return String.Empty;
        }
        //===========================================
        private void AssignFile(string Filename)
        {
            ParamsFilename = Filename;
            ParamsArray.Clear();
            if (!File.Exists(Filename)) return;
            StreamReader GoReader = null;
            string vss = String.Empty;
            try
            {
                GoReader = new StreamReader(Filename, new System.Text.UTF8Encoding());
                while (!GoReader.EndOfStream)
                {
                    vss = GoReader.ReadLine();
                    if (String.IsNullOrEmpty(vss)) continue;
                    ParamsArray.Add(vss);
                }
                GoReader.Close();
                GoReader.Dispose();
                mModified = false;
                mAssigned = true;
            }
            catch { if (GoReader != null) GoReader.Dispose(); }
        }
        //===========================================            

        public void FlushFile(string NewFilename)
        {
            if (NewFilename != ParamsFilename) ParamsFilename = NewFilename;
            FlushFile();
        }
        public void FlushFile()
    {            
            StreamWriter GoWriter = null;
            try
            {
                GoWriter = new StreamWriter(ParamsFilename, false, new System.Text.UTF8Encoding())
                {
                    AutoFlush = false
                };  // bool - append
                for (int i = 0; i < ParamsArray.Count; i++) GoWriter.WriteLine(ParamsArray[i]);
                GoWriter.Close();
                GoWriter.Dispose();
                mModified = false;
            }
            catch { if (GoWriter != null) GoWriter.Dispose(); }
        }
        //===========================================
        public int FindSectionIndex(string SectionName)
        {
            return(ParamsArray.IndexOf('[' + SectionName + ']'));
        }
        //===========================================
        public int FindParameterLine(string Section, string ParamName)
        {
            if (String.IsNullOrWhiteSpace(Section)) return -1;
            NewSectionLine = -1;
            int index = ParamsArray.IndexOf('[' + Section + ']');
            if (index < 0) return (-1);
            index++;
            NewSectionLine = index;
            string sss;
            for (; index < ParamsArray.Count; index++, NewSectionLine++)
            {
                sss = ParamsArray[index];
                if ((sss[0] == '[') & (sss[sss.Length - 1] == ']')) break;
                DivideToParams(sss);
                if (ParamName != p_name) continue;
                return (index);
            }
            return (-1);
        }
        //===========================================
        public void RemoveParameter(string Section, string ParamName)
        {
            int index = FindParameterLine(Section, ParamName);
            if (index >= 0)
            {
                ParamsArray.RemoveAt(index);
                mModified = true;
            }
        }
        //===========================================
        public void RemoveSection(string Section)
        {
            int index = ParamsArray.IndexOf('[' + Section + ']');
            if (index < 0) return;
            ParamsArray.RemoveAt(index);
            string sss;
            for (; index < ParamsArray.Count; )
            {
                sss = ParamsArray[index];
                if ((sss[0] == '[') & (sss[sss.Length - 1] == ']')) break;
                ParamsArray.RemoveAt(index);
            }
            mModified = true;
        }
        //===========================================
        public void ClearAll()
        {
            if (ParamsArray.Count > 0)
            {
                ParamsArray.Clear();
                mModified = true;
            }
        }
        //===========================================
        public string GetParameter(string Section, string ParamName)
        {
            if (FindParameterLine(Section, ParamName) >= 0) return p_body; else return String.Empty;
        }
        //===========================================
        public string GetParameter(string Section, string ParamName, string defaultvalue)
        {
            if (FindParameterLine(Section, ParamName) >= 0) return p_body; else return defaultvalue;
        }
        //===========================================
        public int GetParameter(string Section, string ParamName, int defaultvalue)
        {
            if (FindParameterLine(Section, ParamName) < 0) return defaultvalue;
            int result = defaultvalue;
            try { result = Convert.ToInt32(p_body); } catch { return defaultvalue; }
            return result;
        }
        //===========================================
        public float GetParameter(string Section, string ParamName, float defaultvalue)
        {
            if (FindParameterLine(Section, ParamName) < 0) return defaultvalue;
            float result = defaultvalue;
            try { result = Convert.ToSingle(p_body); } catch { return defaultvalue; }
            return result;
        }
        //===========================================
        public bool GetParameter(string Section, string ParamName, bool defaultvalue)
        {
            if (FindParameterLine(Section, ParamName) < 0) return defaultvalue;
            switch (p_body)
            {
                case "0": return false;
                case "1": return true;
                default: return defaultvalue;
            }
        }
        //===========================================
        public void StoreParameter(string Section, string ParamName, string ParamText)
        {
            string comline = ParamName + '=' + ParamText;
            int index = FindParameterLine(Section, ParamName);
            if (index >= 0)
            {
                if (ParamsArray[index] != comline)
                {
                    ParamsArray[index] = comline;
                    mModified = true;
                }
            }
            else
            {
                if (NewSectionLine >= 0) ParamsArray.Insert(NewSectionLine, comline);
                else
                {
                    ParamsArray.Add('[' + Section + ']');
                    ParamsArray.Add(comline);
                }
                mModified = true;
            }
        }
        //===========================================
        public void StoreParameter(string Section, string ParamName, bool ParamBool)
        {
            string ssb = "0";
            if (ParamBool) ssb = "1";
            StoreParameter(Section, ParamName, ssb);
        }
        //===========================================
        //===========================================
        //===========================================
        //===========================================

        //===========================================

    }  //  Class

//===========================================







}