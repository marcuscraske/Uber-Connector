/*                       ____               ____________
 *                      |    |             |            |
 *                      |    |             |    ________|
 *                      |    |             |   |
 *                      |    |             |   |    
 *                      |    |             |   |    ____
 *                      |    |             |   |   |    |
 *                      |    |_______      |   |___|    |
 *                      |            |  _  |            |
 *                      |____________| |_| |____________|
 *                        
 *      Author(s):      limpygnome (Marcus Craske)              limpygnome@gmail.com
 * 
 *      License:        Creative Commons Attribution-ShareAlike 3.0 Unported
 *                      http://creativecommons.org/licenses/by-sa/3.0/
 * 
 *      Path:           /Base/SQLUtils.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Code cleanup, minor improvements and new comment header.
 *                                      Renamed from Utils to SQLUtils.
 * 
 * *********************************************************************************************************************
 * A class of commonly-used utility methods.
 * *********************************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace UberLib.Connector
{
    /// <summary>
    /// A class of commonly-used utility methods.
    /// </summary>
    public static class SQLUtils
    {
        // Methods - Tables ********************************************************************************************
        public static void tableCreate(string name, Connector connector)
        {
            connector.queryExecute("CREATE TABLE `" + name + "`;");
        }
        public static void tableCreate(string[] names, Connector connector)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (string table in names)
                buffer.Append("CREATE TABLE `").Append(table).Append("`;");
            connector.queryExecute(buffer.ToString());
        }
        public static void tableRemove(string name, Connector connector)
        {
            connector.queryExecute("DROP TABLE `" + name + "`;");
        }
        public static void tableRemove(string[] names, Connector connector)
        {
            StringBuilder buffer = new StringBuilder();
            foreach (string table in names)
                buffer.Append("DROP TABLE `").Append(table).Append("`;");
            connector.queryExecute(buffer.ToString());
        }
        // Constants - Escaping ****************************************************************************************
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
        private static string ch_dq_r = @"\""";
        private static string ch_sq = ((char)0x27).ToString();
        private static string ch_sq_r = @"\'";
        public static string ch_mla = ((char)'\u02BC').ToString();
        public static string ch_mla_r = @"ʼʼ";
        // Methods - Escaping ******************************************************************************************
        public static string escape(string data)
        {
            StringBuilder bufferData = new StringBuilder(data);
            bufferData.Replace(ch_slash, ch_slash_r);       // Slash
            bufferData.Replace(ch_null, ch_null_r);         // Null
            bufferData.Replace(ch_bs, ch_bs_r);             // Backspace
            bufferData.Replace(ch_tab, ch_tab_r);           // Tab
            bufferData.Replace(ch_lf, ch_lf_r);             // New-line
            bufferData.Replace(ch_cr, ch_cr_r);             // Carriage-return
            bufferData.Replace(ch_eof, ch_eof_r);           // End-of-file
            bufferData.Replace(ch_dq, ch_dq_r);             // Double-quotation
            bufferData.Replace(ch_sq, ch_sq_r);             // Single-quotation
            bufferData.Replace(ch_mla, ch_mla_r);           // Modifier-letter-apostrophe
            return bufferData.ToString();
        }
    }
}