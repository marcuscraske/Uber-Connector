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
 *      Path:           /Base/Core.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Code cleanup, minor improvements and new comment header.
 *                      2013-07-28      Class is now abstract, added connector type property and enum, added a few new
 *                                      comments.
 * 
 * *********************************************************************************************************************
 * The base model used to represent a data-source and its core operations.
 * *********************************************************************************************************************
 */
using System;
using System.Data;
using System.Collections.Generic;

namespace UberLib.Connector
{
    /// <summary>
    /// The base model used to represent a data-source and its core operations.
    /// </summary>
    public abstract class Connector
    {
        // Enums *******************************************************************************************************
        /// <summary>
        /// An enum to represent a univeral standard of data-types between the different connectors.
        /// </summary>
        public enum DataType
        {
            Binary,
            Timestamp,
            Time,
            Date,
            DateTime,
            Year,
            TinyBlob,
            Blob,
            MediumBlob,
            LongBlob,
            Int16,
            Int24,
            Int32,
            Int64,
            Byte,
            Float,
            Double,
            UByte,
            UInt16,
            UInt24,
            UInt32,
            UInt64,
            Decimal,
            String,
            Varchar,
            Text,
            TinyText,
            MediumText,
            LongText
        }
        public enum ConnectorType
        {
            MySQL,
            SQLite,
            Unknown
        }
        // Fields - Logging ********************************************************************************************
        internal bool           loggingEnabled = false;         // Indicates if logging is enabled.
        internal List<string>   loggingQueries = null;          // A list of queries executed; logged when logging is enabled.
        internal int            loggingQueriesCount = 0;        // The number of queries counted; this is always incremented regardless of logging being enabled.
        // Methods - Connection ****************************************************************************************
        /// <summary>
        /// Connects to the data-source.
        /// </summary>
        /// <exception cref="UberLib.Connector.ConnectionFailureException">Thrown when the connector fails to connect to the data-source.</exception>
        public virtual void connect() { throw new NotImplementedException(); }
        /// <summary>
        /// Disconnects from the data-source.
        /// </summary>
        public virtual void disconnect() { throw new NotImplementedException(); }
        /// <summary>
        /// Changes the current database being utilized.
        /// </summary>
        /// <param name="database">The database to switch to.</param>
        public virtual void changeDatabase(string database) { throw new NotImplementedException(); }
        // Methods - Query Related *************************************************************************************
        /// <summary>
        /// Executes and returns the result of tuples formed by the query.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <exception cref="UberLib.Connector.QueryException">Thrown when the query failes to be read.</exception>
        /// <exception cref="UberLib.Connector.DuplicateEntryException">Thrown when a duplicate value for a column is inserted.</exception>
        /// <returns>Result from the query's execution.</returns>
        public virtual Result queryRead(string query) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes and returns the result of tuples formed by the query.
        /// </summary>
        /// <param name="statement">The prepared statement to be executed.</param>
        /// <returns>Result from the query's execution.</returns>
        public virtual Result queryRead(PreparedStatement statement) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes and reads a stored procedure.
        /// </summary>
        /// <param name="statement">The prepared statement with the input parameters and query as the procedure name.</param>
        /// <param name="outputParameters">The output attributes (key) and data-types (value).</param>
        /// <exception cref="UberLib.Connector.QueryException">Thrown when the query failes to be read.</exception>
        /// <returns>Result from the procedure's call.</returns>
        public virtual Result queryReadStoredProcedure(PreparedStatement statement, Dictionary<string, DataType> outputParameters) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a query and returns a count.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <exception cref="UberLib.Connector.QueryException">Thrown when the query failes to be read.</exception>
        /// <exception cref="UberLib.Connector.DuplicateEntryException">Thrown when a duplicate value for a column is inserted.</exception>
        /// <returns>The integer scalar result from the query.</returns>
        public virtual int queryCount(string query) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a query and returns a single-object.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <exception cref="UberLib.Connector.QueryException">Thrown when the query failes to be read.</exception>
        /// <exception cref="UberLib.Connector.DuplicateEntryException">Thrown when a duplicate value for a column is inserted.</exception>
        /// <returns>The scalar value of the query.</returns>
        public virtual object queryScalar(string query) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a prepared statement and returns a single-object.
        /// </summary>
        /// <param name="query">The prepared-statement to be executed.</param>
        /// <exception cref="UberLib.Connector.QueryException">Thrown when the query failes to be read.</exception>
        /// <exception cref="UberLib.Connector.DuplicateEntryException">Thrown when a duplicate value for a column is inserted.</exception>
        /// <returns>The scalar value of the query.</returns>
        public virtual object queryScalar(PreparedStatement statement) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a query without returning anything.
        /// </summary>
        /// <param name="query">The query to be executed.</param>
        /// <exception cref="UberLib.Connector.QueryExecuteException">Thrown when the query failes to be executed.</exception>
        /// <exception cref="UberLib.Connector.DuplicateEntryException">Thrown when a duplicate value for a column is inserted.</exception>
        public virtual void queryExecute(string query) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a prepared statement.
        /// </summary>
        /// <param name="statement">The prepared-statement to be executed.</param>
        /// <exception cref="UberLib.Connector.QueryExecuteException">Thrown when the query failes to be executed.</exception>
        /// <exception cref="UberLib.Connector.DuplicateEntryException">Thrown when a duplicate value for a column is inserted.</exception>
        public virtual void queryExecute(PreparedStatement statement) { throw new NotImplementedException(); }
        /// <summary>
        /// Checks the connection is valid and ready for queries; this is meant for applications using Connectors over a prolonged amount of time.
        /// </summary>
        /// <returns>An indication if the connector is yet ready.</returns>
        public virtual bool checkConnectionIsReady() { throw new NotImplementedException(); }
        // Methods - Logging *******************************************************************************************
        /// <summary>
        /// Adds a new query to be logged; the query is not added if logging is not enabled.
        /// </summary>
        /// <param name="query">The query, as a string, to be logged.</param>
        internal void loggingAddQuery(string query)
        {
            if (!loggingEnabled)
                return;
            if (loggingQueries == null)
                loggingQueries = new List<string>();
            loggingQueries.Add(query);
        }
        // Methods - Properties ****************************************************************************************
        /// <summary>
        /// The queries executed using this connector; returns an empty string array if logging is not enabled.
        /// </summary>
        public string[] LoggingQueries
        {
            get
            {
                if (loggingQueries != null)
                    return loggingQueries.ToArray();
                else return new string[] { };
            }
        }
        /// <summary>
        /// The number of queries executed using this connector; logging does not need to be enabled.
        /// </summary>
        public int LoggingQueriesCount
        {
            get
            {
                return loggingQueriesCount;
            }
        }
        /// <summary>
        /// Used to set/get if logging is enabled (true) or disabled (false); if logging is enabled, all queries
        /// executed are logged as strings. If the query uses parameters, the values will not be recorded!
        /// </summary>
        public bool LoggingEnabled
        {
            get
            {
                return loggingEnabled;
            }
        }
        /// <summary>
        /// The type of connector.
        /// </summary>
        public virtual ConnectorType Type
        {
            get
            {
                return ConnectorType.Unknown;
            }
        }
    }
}