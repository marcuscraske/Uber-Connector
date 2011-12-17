/*
 * Creative Commons Attribution-ShareAlike 3.0 unported
 * Connectors\SQLite.cs
 * 
 * Provides an interface for an SQLite database; credit goes to switch-on-the-code.com for an excellent
 * tutorial:
 * http://www.switchonthecode.com/tutorials/csharp-tutorial-writing-a-dotnet-wrapper-for-sqlite
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace UberLib.Connector.Connectors
{
    public class SQLite : Connector
    {
        #region "Constants"
        const int SQLITE_OK = 0;
        const int SQLITE_ROW = 100;
        const int SQLITE_DONE = 101;
        const int SQLITE_INTEGER = 1;
        const int SQLITE_FLOAT = 2;
        const int SQLITE_TEXT = 3;
        #endregion

        #region "Variables"
        private IntPtr _dbp = IntPtr.Zero; // Pointer to the database
        private bool _sqlite_open = false; // Declares if the database is open
        private string _path = null; // The path of the database
        #endregion

        #region "Properties"
        /// <summary>
        /// Returns a boolean stating if a connection to the database/file has been made.
        /// </summary>
        public bool FileOpen
        {
            get
            { return _sqlite_open; }
        }
        /// <summary>
        /// Retrieves the pointer used for the current database.
        /// </summary>
        public IntPtr DatabasePointer
        {
            get
            { return _dbp; }
        }
        #endregion

        #region "External methods"
        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_open")]
        static extern int sqlite3_open(string filename, out IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_close")]
        static extern int sqlite3_close(IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_prepare_v2")]
        static extern int sqlite3_prepare_v2(IntPtr db, string zSql,
            int nByte, out IntPtr ppStmpt, IntPtr pzTail);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_step")]
        static extern int sqlite3_step(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_finalize")]
        static extern int sqlite3_finalize(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_errmsg")]
        static extern string sqlite3_errmsg(IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_count")]
        static extern int sqlite3_column_count(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_origin_name")]
        static extern string sqlite3_column_origin_name(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_type")]
        static extern int sqlite3_column_type(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_int")]
        static extern int sqlite3_column_int(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_text")]
        static extern string sqlite3_column_text(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_double")]
        static extern double sqlite3_column_double(IntPtr stmHandle, int iCol);
        #endregion

        #region "Methods"
        #region "-- Connection"
        public override void Connect()
        {
            if (sqlite3_open(_path, out _dbp) != SQLITE_OK)
                throw new Exception("Failed to create database file, check the path is correct!");
            _sqlite_open = true;
        }
        public override void Disconnect()
        {
            if (_sqlite_open)
                sqlite3_close(_dbp);
        }
        #endregion
        #region "-- Queries"
        public override void Query_Execute(string query)
        {
            if (!_sqlite_open) throw new Exception("The database has not been opened!");
            IntPtr sP;
            sP = _sqlite_prepare(query);
            if (sqlite3_step(sP) != SQLITE_DONE)
                throw new Exception("Failed to execute non-query!");
            _sqlite_finalize(sP);
            _Logging_Queries_Count++;
            if (_Logging_Enabled) _Logging_Add_Entry(query);
        }
        public override Result Query_Read(string query)
        {
            if (!_sqlite_open) throw new Exception("The database has not been opened!");
            _Logging_Queries_Count++;
            if (_Logging_Enabled) _Logging_Add_Entry(query);
            IntPtr sP = _sqlite_prepare(query);
            int columns = sqlite3_column_count(sP);
            Result result = new Result();
            // Get the column names
            string[] columnNames = new string[columns];
            for (int i = 0; i < columns; i++) columnNames[i] = sqlite3_column_origin_name(sP, i);
            // Add rows to result set
            ResultRow row;
            while (sqlite3_step(sP) == SQLITE_ROW) // Iterate through each row
            {
                row = new ResultRow();
                for (int i = 0; i < columns; i++) // Iterate through each column
                    switch (sqlite3_column_type(sP, i))
                    {
                        case SQLITE_INTEGER: row.Columns.Add(columnNames[i], sqlite3_column_int(sP, i)); break;
                        case SQLITE_TEXT: row.Columns.Add(columnNames[i], sqlite3_column_text(sP, i)); break;
                        case SQLITE_FLOAT: row.Columns.Add(columnNames[i], sqlite3_column_double(sP, i)); break;
                        default: row.Columns.Add(columnNames[i], null); break; // Unknown data-type
                    }
                result.Rows.Add(row); // Add row to result set
            }
            _sqlite_finalize(sP);
            return result;
        }
        public override int Query_Count(string query)
        {
            if (!_sqlite_open) throw new Exception("The database has not been opened!");
            _Logging_Queries_Count++;
            if (_Logging_Enabled) _Logging_Add_Entry(query);
            IntPtr sP = _sqlite_prepare(query);
            int columns = sqlite3_column_count(sP);
            // Check there is only one column
            if (columns > 1)
            {
                _sqlite_finalize(sP);
                throw new Exception("Invalid query!");
            }
            // Iterate to the first row and return the value
            int num = 0;
            while (sqlite3_step(sP) == SQLITE_ROW) // Iterate through each row
            {
                if (sqlite3_column_type(sP, 0) != SQLITE_INTEGER)
                {
                    _sqlite_finalize(sP);
                    throw new Exception("Invalid query!");
                }
                num = sqlite3_column_int(sP, 0);
                break; // Exit the iteration - we'll ignore other rows
            }
            _sqlite_finalize(sP);
            return num;
        }
        public override object Query_Scalar(string query)
        {
            if (!_sqlite_open) throw new Exception("The database has not been opened!");
            _Logging_Queries_Count++;
            if (_Logging_Enabled) _Logging_Add_Entry(query);
            IntPtr sP = _sqlite_prepare(query);
            int columns = sqlite3_column_count(sP);
            // Check there is only one column
            if (columns > 1)
            {
                _sqlite_finalize(sP);
                throw new Exception("Invalid query!");
            }
            // Iterate to the first row and return the value
            object obj = null;
            while (sqlite3_step(sP) == SQLITE_ROW) // Iterate through each row
            {
                switch (sqlite3_column_type(sP, 0))
                {
                    case SQLITE_INTEGER: obj = sqlite3_column_int(sP, 0); break;
                    case SQLITE_TEXT: obj = sqlite3_column_text(sP, 0); break;
                    case SQLITE_FLOAT: obj = sqlite3_column_double(sP, 0); break;
                }
                break; // Exit the iteration - we'll ignore other rows
            }
            _sqlite_finalize(sP);
            return obj;
        }
        #endregion
        /// <summary>
        /// Changes the database, which is a file in SQLite, used; this will automatically connect.
        /// </summary>
        /// <param name="file"></param>
        public override void ChangeDatabase(string file)
        {
            if (_sqlite_open)
                Disconnect();
            _path = file;
            Connect();
        }
        /// <summary>
        /// Prepares to execute an SQL statement.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private IntPtr _sqlite_prepare(string query)
        {
            IntPtr sP; // Statement pointer
            if (sqlite3_prepare_v2(_dbp, query, query.Length, out sP, IntPtr.Zero) != SQLITE_OK)
                throw new Exception("Failed to prepare query: " + sqlite3_errmsg(_dbp));
            return sP;
        }
        /// <summary>
        /// Finalizes an SQL statement.
        /// </summary>
        /// <param name="sP"></param>
        private void _sqlite_finalize(IntPtr sP)
        {
            if (sqlite3_finalize(sP) != SQLITE_OK)
                throw new Exception("Failed to finalize query!");
        }
        #endregion
    }
}