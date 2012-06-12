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
        public static string Escape(string data)
        {
            return data
                .Replace(ch_slash, ch_slash_r)  // Slashes
                .Replace(ch_null, ch_null_r)    // Null
                .Replace(ch_bs, ch_bs_r)        // Backspace
                //.Replace(ch_tab, ch_tab_r)      // Tab
                //.Replace(ch_lf, ch_lf_r)        // New-line
                //.Replace(ch_cr, ch_cr_r)        // Carriage-return
                .Replace(ch_eof, ch_eof_r)      // End-of-file
                //.Replace(ch_dq, ch_dq_r)        // Double-quotation
                .Replace(ch_sq, ch_sq_r);       // Single quotation
        }
        #endregion
    }
}