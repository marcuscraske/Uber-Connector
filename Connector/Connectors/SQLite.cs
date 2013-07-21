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
 *      Path:           /Connectors/SQLite.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Code cleanup, minor improvements and new comment header.
 *                                      Added support for prepared-statements.
 * 
 * *********************************************************************************************************************
 * A connector for interfacing with SQLite data-sources.
 * 
 * Credit due to the following:
 * -    An excellent tutorial:
 *      http://www.switchonthecode.com/tutorials/csharp-tutorial-writing-a-dotnet-wrapper-for-sqlite
 *  -   Help with calling:
 *      http://www.hackchina.com/en/r/151107/Sqlite3.cs__html
 * *********************************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace UberLib.Connector.Connectors
{
    /// <summary>
    /// A connector for interfacing with SQLite data-sources.
    /// </summary>
    public class SQLite : Connector
    {
        // Constants ***************************************************************************************************
        const int SQLITE_OK = 0;
        const int SQLITE_ROW = 100;
        const int SQLITE_DONE = 101;
        const int SQLITE_INTEGER = 1;
        const int SQLITE_FLOAT = 2;
        const int SQLITE_TEXT = 3;
        const int SQLITE_BUSY = 5;
        // Fields ******************************************************************************************************
        private IntPtr _dbp = IntPtr.Zero; // Pointer to the database
        private bool _sqlite_open = false; // Declares if the database is open
        private string _path = null; // The path of the database
        // Methods - Properties ****************************************************************************************
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

        // Methods - External Library Points ***************************************************************************
        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_open")]
        static extern int sqlite3_open(string filename, out IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_close")]
        static extern int sqlite3_close(IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_prepare_v2")]
        static extern int sqlite3_prepare_v2(IntPtr db, string zSql, int nByte, out IntPtr ppStmpt, out IntPtr pzTail);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_step")]
        static extern int sqlite3_step(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_exec", CallingConvention = CallingConvention.Cdecl)]
        static extern int sqlite3_exec(IntPtr hDb, string sql, IntPtr callback, IntPtr args, out IntPtr errorMessage);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_finalize")]
        static extern int sqlite3_finalize(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_errmsg")]
        static extern string sqlite3_errmsg(IntPtr db);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_count")]
        static extern int sqlite3_column_count(IntPtr stmHandle);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_name")]
        static extern string sqlite3_column_name(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_type")]
        static extern int sqlite3_column_type(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_int")]
        static extern int sqlite3_column_int(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_text")]
        static extern string sqlite3_column_text(IntPtr stmHandle, int iCol);

        [DllImport("sqlite3.dll", EntryPoint = "sqlite3_column_double")]
        static extern double sqlite3_column_double(IntPtr stmHandle, int iCol);
        // Methods - Implemented ***************************************************************************************
        public override void connect()
        {
            if (sqlite3_open(_path, out _dbp) != SQLITE_OK)
                throw new Exception("Failed to create or open database file, check the path is correct!");
            _sqlite_open = true;
        }
        public override void disconnect()
        {
            if (_sqlite_open)
                sqlite3_close(_dbp);
        }
        /// <summary>
        /// Changes the database, which is a file in SQLite, used; this will automatically connect.
        /// </summary>
        /// <param name="file">The path of the file.</param>
        public override void changeDatabase(string file)
        {
            if (_sqlite_open)
                disconnect();
            _path = file;
            connect();
        }
        public override Result queryRead(string query)
        {
            if (!_sqlite_open)
                throw new QueryException("The database has not been opened!");
            // Logging
            loggingQueriesCount++;
            loggingAddQuery(query);
            // Read
            IntPtr sP = _sqlite_prepare(query, ref query);
            Result result = new Result();
            try
            {
                int columns = sqlite3_column_count(sP);
                // Get the column names
                string[] columnNames = new string[columns];
                for (int i = 0; i < columns; i++)
                {
                    try
                    {
                        columnNames[i] = sqlite3_column_name(sP, i) ?? "invalid_null_column_name_" + i;
                    }
                    catch
                    {
                        columnNames[i] = "invalid_error_" + i;
                    }
                }
                // Add rows to result set
                ResultRow row;
                while (sqlite3_step(sP) == SQLITE_ROW) // Iterate through each row
                {
                    row = new ResultRow();
                    for (int i = 0; i < columns; i++) // Iterate through each column
                    {
                        switch (sqlite3_column_type(sP, i))
                        {
                            case SQLITE_INTEGER:
                                row.attributes.Add(columnNames[i], sqlite3_column_int(sP, i));
                                break;
                            case SQLITE_TEXT:
                                row.attributes.Add(columnNames[i], sqlite3_column_text(sP, i));
                                break;
                            case SQLITE_FLOAT:
                                row.attributes.Add(columnNames[i], sqlite3_column_double(sP, i));
                                break;
                            default: // Unhandled/unknown data-type
                                row.attributes.Add(columnNames[i], null);
                                break;
                        }
                    }
                    // Add row to result set
                    result.tuples.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw new QueryException("Failed to read query '" + query + "'!", ex);
            }
            finally
            {
                _sqlite_finalize(sP);
            }
            return result;
        }
        public override int queryCount(string query)
        {
            if (!_sqlite_open)
                throw new QueryException("The database has not been opened!");
            // Logging
            loggingQueriesCount++;
            loggingAddQuery(query);
            // Perform count of tuples
            IntPtr sP = _sqlite_prepare(query, ref query);
            int columns = sqlite3_column_count(sP);
            // Check there is only one column
            if (columns > 1)
            {
                _sqlite_finalize(sP);
                throw new QueryException("Invalid query!");
            }
            // Iterate to the first row and return the value
            int num = 0;
            while (sqlite3_step(sP) == SQLITE_ROW) // Iterate through each row
            {
                if (sqlite3_column_type(sP, 0) != SQLITE_INTEGER)
                {
                    _sqlite_finalize(sP);
                    throw new QueryException("Invalid query!");
                }
                num = sqlite3_column_int(sP, 0);
                break; // Exit the iteration - we'll ignore other rows
            }
            _sqlite_finalize(sP);
            return num;
        }
        public override object queryScalar(string query)
        {
            if (!_sqlite_open) throw new QueryException("The database has not been opened!");
            string duplicateQuery = (string)query.Clone();

            object returnObject = null;
            // Logging
            loggingQueriesCount++;
            loggingAddQuery(query);
            // Prepare statements and execute them until the remainder of the SQL reaches zero
            int loops = 0;
            string states = "";
            // Execute each subquery
            foreach (string subquery in query.Split(';'))
            {
                string sq = subquery.Trim(); // Get rid of whitespace
                // Ensure the string is an actual query
                if (sq.Length > 0)
                {
                    // Prepare statement - this will also set duplicateQuery to any remainder SQL which will not be executed
                    IntPtr sP = _sqlite_prepare(subquery + ";", ref duplicateQuery);
                    // Execute the statement
                    states += sqlite3_step(sP) + ",";
                    // Get the data type of the column
                    switch (sqlite3_column_type(sP, 0))
                    {
                        case SQLITE_INTEGER: returnObject = sqlite3_column_int(sP, 0); break;
                        case SQLITE_TEXT: returnObject = sqlite3_column_text(sP, 0); break;
                        case SQLITE_FLOAT: returnObject = sqlite3_column_double(sP, 0); break;
                    }

                    // Finalize the query
                    _sqlite_finalize(sP);
                    loops++;
                }
            }
            // Return the object
            return returnObject;
        }
        public override void queryExecute(string query)
        {
            // Check a connection to the database is open
            if (!_sqlite_open)
                throw new QueryExecuteException("The database has not been opened!");
            IntPtr errorP = IntPtr.Zero;
            // Wait for the query to finish executing
            while (sqlite3_exec(_dbp, query, IntPtr.Zero, IntPtr.Zero, out errorP) == SQLITE_BUSY)
                System.Threading.Thread.Sleep(1);
            // Check no errors occurred
            if (errorP != IntPtr.Zero)
                throw new QueryExecuteException("Failed to execute query - " + Marshal.PtrToStringAnsi(errorP));
        }
        /// <summary>
        /// Prepares to execute an SQL statement.
        /// </summary>
        /// <param name="queryUncompiled">pzTail - a pointer to a duplicate query; when this statement is compiled, the uncompiled text i.e. a second statement is returned.</param>
        private IntPtr _sqlite_prepare(string query, ref string queryUncompiled)
        {
            IntPtr queryP;
            IntPtr sP; // Statement pointer
            if (sqlite3_prepare_v2(_dbp, query, query.Length, out sP, out queryP) != SQLITE_OK)
                throw new QueryException("Failed to prepare query: '" + sqlite3_errmsg(_dbp) + "' for '" + query + "'");

            //f(queryP != IntPtr.Zero) queryUncompiled = Marshal.PtrToStringAnsi(queryP);
            return sP;
        }
        /// <summary>
        /// Finalizes an SQL statement.
        /// </summary>
        private void _sqlite_finalize(IntPtr sP)
        {
            int resultCode;
            while ((resultCode = sqlite3_finalize(sP)) == SQLITE_BUSY)
                System.Threading.Thread.Sleep(10);
            if (resultCode != SQLITE_OK)
                throw new QueryException("Failed to finalize query, error: " + resultCode + "!");
        }
    }
}