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
 *      Path:           /Connectors/MySQL.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Code cleanup, minor improvements and new comment header.
 *                                      Added support for prepared-statements.
 *                                      Merged commonly used code for reading into method 'queryReadInteral'.
 *                      2013-07-24      Added null support.
 * 
 * *********************************************************************************************************************
 * A connector for interfacing with MySQL data-sources.
 * *********************************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using MySql.Data.MySqlClient;
using System.IO;

namespace UberLib.Connector.Connectors
{
    /// <summary>
    /// A connector for interfacing with MySQL data-sources.
    /// </summary>
    public class MySQL : Connector
    {
        // Constants ***************************************************************************************************
        private const int BYTE_BUFFER_SIZE = 4098;
        // Fields ******************************************************************************************************
        private MySqlConnection rawConnector = new MySqlConnection();
        private string settingsHost = "localhost";
        private int settingsPort = 3306;
        private string settingsUser = "root";
        private string settingsPass = "";
        private string settingsDatabase = "";
        private int settingsTimeoutConnection = 5;
        private bool settingsPooling = false;
        private int settingsPoolingSizeMin = 0;
        private int settingsPoolingSizeMax = 20;
        private string settingsConnectionString = "";
        // Methods - Implemented ***************************************************************************************
        public override void connect()
        {
            try
            {
                rawConnector.ConnectionString = "Host=" + settingsHost + "; Port=" + settingsPort + "; Database=" + settingsDatabase + "; UID=" + settingsUser + "; Password=" + settingsPass + "; Pooling=" + settingsPooling + "; " + (settingsTimeoutConnection > 0 ? "Connection Timeout=" + settingsTimeoutConnection + "; " : "") + (settingsPooling ? "Min Pool Size=" + settingsPoolingSizeMin + "; Max Pool Size=" + settingsPoolingSizeMax + "; " : "") + settingsConnectionString;
                rawConnector.Open();
            }
            catch (Exception ex)
            {
                throw new ConnectionFailureException("Failed to establish a connection using MySQL!", ex);
            }
        }
        public override void disconnect()
        {
            rawConnector.Close();
        }
        public override void changeDatabase(string database)
        {
            rawConnector.ChangeDatabase(database);
        }
        public override Result queryRead(string query)
        {
            lock (rawConnector)
            {
                // Logging
                loggingQueriesCount++;
                loggingAddQuery(query);
                // Execute the query and read the result
                try
                {
                    Result result = new Result();
                    MySqlCommand command = new MySqlCommand(query, rawConnector);
                    queryReadInteral(ref result, ref command);
                    return result;
                }
                catch (Exception ex)
                {
                    handleQueryError(ref query, ex);
                }
                return null;
            }
        }
        public override Result queryRead(PreparedStatement statement)
        {
            lock (rawConnector)
            {
                // Logging
                loggingQueriesCount++;
                loggingAddQuery(statement.query);
                // Execute the query and read the result
                try
                {

                    Result result = new Result();
                    MySqlCommand command = new MySqlCommand(statement.query, rawConnector);
                    // Add parameters
                    foreach (KeyValuePair<string, object> inputs in statement.parameters)
                    {
                        command.Parameters.Add("?" + inputs.Key, inputs.Value.ToString());
                        command.Parameters["?" + inputs.Key].Direction = ParameterDirection.Input;
                    }
                    queryReadInteral(ref result, ref command);
                    return result;
                }
                catch (Exception ex)
                {
                    handleQueryError(ref statement.query, ex);
                }
                return null;
            }
        }
        public override Result queryReadStoredProcedure(PreparedStatement statement,  Dictionary<string, Connector.DataType> outputParameters)
        {
            lock (rawConnector)
            {
                // Logging
                loggingQueriesCount++;
                loggingAddQuery(statement.query);
                // Execute the query and read the result
                try
                {
                    Result result = new Result();
                    MySqlCommand command = new MySqlCommand(statement.query, rawConnector);
                    // Add parameters
                    foreach (KeyValuePair<string, object> inputs in statement.parameters)
                    {
                        command.Parameters.Add("?" + inputs.Key, inputs.Value.ToString());
                        command.Parameters["?" + inputs.Key].Direction = ParameterDirection.Input;
                    }
                    // Add outputs
                    MySqlDbType type;
                    foreach (KeyValuePair<string, Connector.DataType> outputs in outputParameters)
                    {
                        switch (outputs.Value)
                        {
                            case DataType.Binary:
                                type = MySqlDbType.Binary;
                                break;
                            case DataType.Blob:
                                type = MySqlDbType.Blob;
                                break;
                            case DataType.Byte:
                                type = MySqlDbType.Byte;
                                break;
                            case DataType.Date:
                                type = MySqlDbType.Date;
                                break;
                            case DataType.DateTime:
                                type = MySqlDbType.Datetime;
                                break;
                            case DataType.Decimal:
                                type = MySqlDbType.Decimal;
                                break;
                            case DataType.Double:
                                type = MySqlDbType.Double;
                                break;
                            case DataType.Float:
                                type = MySqlDbType.Float;
                                break;
                            case DataType.Int16:
                                type = MySqlDbType.Int16;
                                break;
                            case DataType.Int24:
                                type = MySqlDbType.Int24;
                                break;
                            case DataType.Int32:
                                type = MySqlDbType.Int32;
                                break;
                            case DataType.Int64:
                                type = MySqlDbType.Int64;
                                break;
                            case DataType.LongBlob:
                                type = MySqlDbType.LongBlob;
                                break;
                            case DataType.LongText:
                                type = MySqlDbType.LongText;
                                break;
                            case DataType.MediumBlob:
                                type = MySqlDbType.MediumBlob;
                                break;
                            case DataType.MediumText:
                                type = MySqlDbType.MediumText;
                                break;
                            case DataType.String:
                                type = MySqlDbType.String;
                                break;
                            case DataType.Text:
                                type = MySqlDbType.Text;
                                break;
                            case DataType.Time:
                                type = MySqlDbType.Time;
                                break;
                            case DataType.Timestamp:
                                type = MySqlDbType.Timestamp;
                                break;
                            case DataType.TinyBlob:
                                type = MySqlDbType.TinyBlob;
                                break;
                            case DataType.TinyText:
                                type = MySqlDbType.TinyText;
                                break;
                            case DataType.UByte:
                                type = MySqlDbType.UByte;
                                break;
                            case DataType.UInt16:
                                type = MySqlDbType.UInt16;
                                break;
                            case DataType.UInt24:
                                type = MySqlDbType.UInt24;
                                break;
                            case DataType.UInt32:
                                type = MySqlDbType.UInt32;
                                break;
                            case DataType.UInt64:
                                type = MySqlDbType.UInt64;
                                break;
                            case DataType.Varchar:
                                type = MySqlDbType.VarChar;
                                break;
                            case DataType.Year:
                                type = MySqlDbType.Year;
                                break;
                            default:
                                throw new Exception("Non-supported type provided for stored procedure!");
                        }
                        command.Parameters.Add("?" + outputs.Key, type);
                        command.Parameters["?" + outputs.Key].Direction = ParameterDirection.Output;
                    }
                    // Read
                    queryReadInteral(ref result, ref command);
                    return result;
                }
                catch (Exception ex)
                {
                    handleQueryError(ref statement.query, ex);
                }
                return null;
            }
        }
        private void queryReadInteral(ref Result result, ref MySqlCommand command)
        {
            // Create reader
            MySqlDataReader reader = command.ExecuteReader();
            // Read row-by-row
            byte[] buffer;
            MemoryStream bufferMS;
            int bufferOffset;
            int bytesAvailable;
            ResultRow row;
            int t;
            while (reader.Read())
            {
                row = new ResultRow();
                for (t = 0; t < reader.FieldCount; t++)
                {
                    row.attributes.Add(reader.GetName(t), reader.IsDBNull(t) ? null : reader.GetValue(t));
                    // Check if the column of the row is a byte-array, if so -> add to separate byte dictionary
                    if (reader.GetDataTypeName(t) == "BLOB")
                    {
                        bufferMS = new MemoryStream();
                        try
                        {
                            bufferOffset = 0;
                            bytesAvailable = (int)reader.GetBytes(t, 0, null, 0, 0);
                            while (bufferOffset < bytesAvailable)
                            {
                                reader.GetBytes(t, bufferOffset, buffer = new byte[BYTE_BUFFER_SIZE], 0, BYTE_BUFFER_SIZE);
                                bufferMS.Write(buffer, 0, BYTE_BUFFER_SIZE);
                                bufferOffset += BYTE_BUFFER_SIZE;
                            }
                            bufferMS.Flush();
                            if (row.attributesByteArray == null)
                                row.attributesByteArray = new Dictionary<string, byte[]>();
                            row.attributesByteArray.Add(reader.GetName(t), bufferMS.ToArray());
                        }
                        catch { }
                        finally
                        {
                            bufferMS.Dispose();
                        }
                    }
                }
                result.tuples.Add(row);
            }
            reader.Close();
        }
        public override int queryCount(string query)
        {
            return int.Parse((queryScalar(query) ?? 0).ToString());
        }
        public override object queryScalar(string query)
        {
            lock (rawConnector)
            {
                // Logging
                loggingQueriesCount++;
                loggingAddQuery(query);
                // Execute query and return single value/scalar
                try
                {
                    MySqlCommand command = new MySqlCommand(query, rawConnector);
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    handleQueryError(ref query, ex);
                    return null;
                }
            }
        }
        public override object queryScalar(PreparedStatement statement)
        {
            lock (rawConnector)
            {
                // Logging
                loggingQueriesCount++;
                loggingAddQuery(statement.query);
                // Translate, execute and return single value/scalar
                try
                {
                    MySqlCommand command = new MySqlCommand(statement.query, rawConnector);
                    foreach (KeyValuePair<string, object> key in statement.parameters)
                        command.Parameters.Add("?" + key.Key, key.Value);
                    return command.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    handleExecuteError(ref statement.query, ex);
                    return null;
                }
            }
        }
        public override void queryExecute(string query)
        {
            lock (rawConnector)
            {
                // Logging
                loggingQueriesCount++;
                loggingAddQuery(query);
                // Execute query
                try
                {
                    MySqlCommand command = new MySqlCommand(query, rawConnector);
                    command.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    handleExecuteError(ref query, ex);
                }
            }
        }
        public override void queryExecute(PreparedStatement statement)
        {
            lock (rawConnector)
            {
                // Logging
                loggingQueriesCount++;
                loggingAddQuery(statement.query);
                // Execute statement
                try
                {
                    MySqlCommand command = new MySqlCommand(statement.query, rawConnector);
                    foreach (KeyValuePair<string, object> key in statement.parameters)
                        command.Parameters.Add("?" + key.Key, key.Value);
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    handleExecuteError(ref statement.query, ex);
                }
            }
        }
        private void handleExecuteError(ref string query, Exception ex)
        {
            const string regexPattern = "Duplicate entry '(.*?)' for key '(.*?)'";
            Match match = Regex.Match(ex.Message, regexPattern);
            if (match == null || match.Groups.Count != 3)
                throw new QueryExecuteException("Failed to execute query '" + query + "'!", ex);
            else
                throw new DuplicateEntryException(match.Groups[2].Value);
        }
        private void handleQueryError(ref string query, Exception ex)
        {
            const string regexPattern = "Duplicate entry '(.*?)' for key '(.*?)'";
            Match match = Regex.Match(ex.Message, regexPattern);
            if (match == null || match.Groups.Count != 3)
                throw new QueryException("Failed to read query '" + query + "'!", ex);
            else
                throw new DuplicateEntryException(match.Groups[2].Value);
        }
        public override bool checkConnectionIsReady()
        {
            lock (rawConnector)
            {
                try
                {
                    // Reopen the connector if it is not ready
                    if (rawConnector.State == ConnectionState.Closed || rawConnector.State == ConnectionState.Broken)
                        rawConnector.Open();
                    return true;
                }
                catch { return false; }
            }
        }
        // Methods - Properties ****************************************************************************************
        /// <summary>
        /// The string used for creating the connection to the MySQL server.
        /// </summary>
        public string SettingsConnectionString
        {
            get
            {
                return settingsConnectionString;
            }
            set
            {
                settingsConnectionString = value;
            }
        }
        /// <summary>
        /// The raw MySQL API object used for interfacing with the DBMS.
        /// </summary>
        public MySqlConnection RawConnector
        {
            get
            {
                return rawConnector;
            }
            set
            {
                rawConnector = value;
            }
        }
        /// <summary>
        /// The host of the DBMS.
        /// </summary>
        public string SettingsHost
        {
            get
            {
                return settingsHost;
            }
            set
            {
                if (Uri.CheckHostName(value) == UriHostNameType.Unknown) throw new Exception("Invalid host!");
                settingsHost = value;
            }
        }
        /// <summary>
        /// The port of the DBMS.
        /// </summary>
        public int SettingsPort
        {
            get
            {
                return settingsPort;
            }
            set
            {
                // This is only basic validation
                if (value < 1 || value > 65535) throw new Exception("Invalid port!");
                settingsPort = value;
            }
        }
        /// <summary>
        /// The username of the DBMS.
        /// </summary>
        public string SettingsUser
        {
            get
            {
                return settingsUser;
            }
            set
            {
                settingsUser = value;
            }
        }
        /// <summary>
        /// The password of the DBMS.
        /// </summary>
        public string SettingsPass
        {
            get
            {
                return settingsPass;
            }
            set
            {
                settingsPass = value;
            }
        }
        /// <summary>
        /// The database of the DBMS to use during the connection; can be switched.
        /// </summary>
        public string SettingsDatabase
        {
            get
            {
                return settingsDatabase;
            }
            set
            {
                settingsDatabase = value;
            }
        }
        /// <summary>
        /// The time, in seconds, for the connection to timeout.
        /// </summary>
        public int SettingsTimeoutConnection
        {
            get
            {
                return settingsTimeoutConnection;
            }
            set
            {
                if (value < 0) throw new Exception("Value cannot be less than zero!");
                settingsTimeoutConnection = value;
            }
        }
        /// <summary>
        /// Indicates if a connections pool should be created.
        /// </summary>
        public bool SettingsPooling
        {
            get
            {
                return settingsPooling;
            }
            set
            {
                settingsPooling = value;
            }
        }
        /// <summary>
        /// The minimum number of connections in the connection pool.
        /// </summary>
        public int SettingsPoolingSizeMin
        {
            get
            {
                return settingsPoolingSizeMin;
            }
            set
            {
                if (value < 0) throw new Exception("Value cannot be less than zero!");
                settingsPoolingSizeMin = value;
            }
        }
        /// <summary>
        /// The maximum number of connections in the connection pool.
        /// </summary>
        public int SettingsPoolingSizeMax
        {
            get
            {
                return settingsPoolingSizeMax;
            }
            set
            {
                if (value < 0) throw new Exception("Value cannot be less than zero!");
                settingsPoolingSizeMax = value;
            }
        }
    }
}