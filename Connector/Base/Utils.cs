/*
 * License:     Creative Commons Attribution-ShareAlike 3.0 unported
 * File:        Base\Utils.cs
 * Authors:     limpygnome              limpygnome@gmail.com
 * 
 * Provides various utilities for escaping data etc.
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace UberLib.Connector
{
    public static class Utils
    {
        #region "Tables"
        public static bool Table_Create(string name, Connector Connector)
        {
            try
            {
                Connector.Query_Execute("CREATE TABLE `" + name + "`;");
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool Table_Create(string[] names, Connector Connector)
        {
            try
            {
                string t = "";
                foreach (string s in names) t += "CREATE TABLE `" + s + "`;";
                Connector.Query_Execute(t);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool Table_Remove(string name, Connector Connector)
        {
            try
            {
                Connector.Query_Execute("DROP TABLE `" + name + "`;");
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static bool Table_Remove(string[] names, Connector Connector)
        {
            try
            {
                string t = "";
                foreach (string s in names) t += "DROP TABLE `" + s + "`;";
                Connector.Query_Execute(t);
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region "Escaping"
        private static string ch_slash = ((char)0x5c).ToString();
        private static string ch_slash_r = @"\\";
        private static string ch_null = ((char)0x00).ToString();
        private static string ch_null_r = @"\0";
        private static string ch_bs = ((char)0x08).ToString();
        private static string ch_bs_r = @"\b";
        private static string ch_tab = ((char)0x09).ToString();
        private static string ch_tab_r = @"\t";
        private static string ch_lf = ((char)0x0a).ToString();
        private static string ch_lf_r = @"\n";
        private static string ch_cr = ((char)0x0d).ToString();
        private static string ch_cr_r = @"\r";
        private static string ch_eof = ((char)0x1a).ToString();
        private static string ch_eof_r = @"\z";
        private static string ch_dq = ((char)0x22).ToString();
        private static string ch_dq_r = @"""""";
        private static string ch_sq = ((char)0x27).ToString();
        private static string ch_sq_r = @"''";
        public static string ch_mla = ((char)'\u02BC').ToString();
        public static string ch_mla_r = @"ʼʼ";
        public static string Escape(string data)
        {
            StringBuilder bufferData = new StringBuilder(data);
            bufferData.Replace(ch_slash, ch_slash_r);       // Slash
            bufferData.Replace(ch_null, ch_null_r);         // Null
            bufferData.Replace(ch_bs, ch_bs_r);             // Backspace
            bufferData.Replace(ch_tab, ch_tab_r);           // Tab
            bufferData.Replace(ch_lf, ch_lf_r);             // New-line
            bufferData.Replace(ch_cr, ch_cr_r);             // Carriage-return
            bufferData.Replace(ch_eof, ch_eof_r);           // End-of-file
            bufferData.Replace(ch_dq, ch_dq_r);               // Double-quotation
            bufferData.Replace(ch_sq, ch_sq_r);             // Single-quotation
            bufferData.Replace(ch_mla, ch_mla_r);             // Modifier-letter-apostrophe
            return bufferData.ToString();
        }
        #endregion
    }
}