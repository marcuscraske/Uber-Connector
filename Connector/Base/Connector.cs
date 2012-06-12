/*
 * License:     Creative Commons Attribution-ShareAlike 3.0 unported
 * File:        Base\Connector.cs
 * Authors:     limpygnome              limpygnome@gmail.com
 * 
 * A shared interface used by each type of Connector.
 */

using System;
using System.Data;
using System.Collections.Generic;

namespace UberLib.Connector
{
    public class Connector
    {
        #region "Logging"
        /*
         * Logging should always add to counter regardless of being enabled, however queries should only be logged if logging is enabled.
         */
        internal bool _Logging_Enabled = false;
        internal List<string> _Logging_Queries = null;
        internal int _Logging_Queries_Count = 0;
        internal void _Logging_Add_Entry(string query)
        {
            if (_Logging_Queries == null) _Logging_Queries = new List<string>();
            _Logging_Queries.Add(query);
        }
        #endregion

        #region "Properties"
        public string[] Logging_Queries()
        {
            if (_Logging_Queries != null)
                return _Logging_Queries.ToArray();
            else return new string[]{ };
        }
        public int Logging_Queries_Count()
        {
            return _Logging_Queries_Count;
        }
        #endregion

        #region "Methods"
        /// <summary>
        /// Connects to the data-source.
        /// </summary>
        /// <exception cref="UberLib.Connector.ConnectionFailureException">Thrown when the connector fails to connect to the data-source.</exception>
        /// <returns></returns>
        public virtual void Connect() { throw new NotImplementedException(); }
        /// <summary>
        /// Disconnects from the data-source.
        /// </summary>
        public virtual void Disconnect() { throw new NotImplementedException(); }
        /// <summary>
        /// Changes the current database being utilized.
        /// </summary>
        /// <param name="database"></param>
        public virtual void ChangeDatabase(string database) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a query consisting of possibly multiple rows and columns.
        /// </summary>
        /// <param name="query"></param>
        /// <exception cref="UberLib.Connector.QueryException">Thrown when the query failes to be read.</exception>
        /// <returns></returns>
        public virtual Result Query_Read(string query) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a query and returns a count.
        /// </summary>
        /// <param name="query"></param>
        /// <exception cref="UberLib.Connector.QueryException">Thrown when the query failes to be read.</exception>
        /// <returns></returns>
        public virtual int Query_Count(string query) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a query and returns a single-object.
        /// </summary>
        /// <param name="query"></param>
        /// <exception cref="UberLib.Connector.QueryException">Thrown when the query failes to be read.</exception>
        /// <returns></returns>
        public virtual object Query_Scalar(string query) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a query without returning anything.
        /// </summary>
        /// <param name="query"></param>
        /// <exception cref="UberLib.Connector.QueryExecuteException">Thrown when the query failes to be executed.</exception>
        public virtual void Query_Execute(string query) { throw new NotImplementedException(); }
        /// <summary>
        /// Executes a query but replaces @Name in the query with the corresponding value for the key "Name" in the parameters dictionary.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        public virtual void Query_Execute_Parameters(string query, Dictionary<string, object> parameters) { throw new NotImplementedException(); }
        /// <summary>
        /// Checks the connection is valid and ready for queries; this is meant for applications using Connectors over a prolonged amount of time.
        /// </summary>
        /// <returns>An indication if the connector is yet ready.</returns>
        public virtual bool CheckConnectionIsReady() { throw new NotImplementedException(); }
        #endregion
    }
}