/*
 * Creative Commons Attribution-ShareAlike 3.0 unported
 * Connectors\MySQL.cs
 * 
 * Provides an interface for MySQL servers.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace UberLib.Connector.Connectors
{
    public class MySQL : Connector
    {
        #region "Variables"
        private MySqlConnection _Raw_Connector = new MySqlConnection();
        private string _Settings_Host = "localhost";
        private int _Settings_Port = 3306;
        private string _Settings_User = "root";
        private string _Settings_Pass = "";
        private string _Settings_Database = "";
        private int _Settings_Timeout_Command = 20;
        private int _Settings_Timeout_Connection = 5;
        private bool _Settings_Pooling = false;
        private int _Settings_Pooling_Size_Min = 0;
        private int _Settings_Pooling_Size_Max = 20;
        private string _Settings_ConnectionString = "";
        #endregion

        #region "Properties"
        public string Settings_Connection_String
        {
            get
            {
                return _Settings_ConnectionString;
            }
            set
            {
                _Settings_ConnectionString = value;
            }
        }
        public MySqlConnection Raw_Connector
        {
            get
            {
                return _Raw_Connector;
            }
            set
            {
                _Raw_Connector = value;
            }
        }
        public string Settings_Host
        {
            get
            {
                return _Settings_Host;
            }
            set
            {
                if (Uri.CheckHostName(value) == UriHostNameType.Unknown) throw new Exception("Invalid host!");
                _Settings_Host = value;
            }
        }
        public int Settings_Port
        {
            get
            {
                return _Settings_Port;
            }
            set
            {
                // This is only basic validation
                if (value < 1 || value > 65535) throw new Exception("Invalid port!");
                _Settings_Port = value;
            }
        }
        public string Settings_User
        {
            get
            {
                return _Settings_User;
            }
            set
            {
                _Settings_User = value;
            }
        }
        public string Settings_Pass
        {
            get
            {
                return _Settings_Pass;
            }
            set
            {
                _Settings_Pass = value;
            }
        }
        public string Settings_Database
        {
            get
            {
                return _Settings_Database;
            }
            set
            {
                _Settings_Database = value;
            }
        }
        public int Settings_Timeout_Command
        {
            get
            {
                return _Settings_Timeout_Command;
            }
            set
            {
                if (value < 0) throw new Exception("Value cannot be less than zero!");
                _Settings_Timeout_Command = value;
            }
        }
        public int Settings_Timeout_Connection
        {
            get
            {
                return _Settings_Timeout_Connection;
            }
            set
            {
                if (value < 0) throw new Exception("Value cannot be less than zero!");
                _Settings_Timeout_Connection = value;
            }
        }
        public bool Settings_Pooling
        {
            get
            {
                return _Settings_Pooling;
            }
            set
            {
                _Settings_Pooling = value;
            }
        }
        public int Settings_Pooling_Size_Min
        {
            get
            {
                return _Settings_Pooling_Size_Min;
            }
            set
            {
                if (value < 0) throw new Exception("Value cannot be less than zero!");
                _Settings_Pooling_Size_Min = value;
            }
        }
        public int Settings_Pooling_Size_Max
        {
            get
            {
                return _Settings_Pooling_Size_Max;
            }
            set
            {
                if (value < 0) throw new Exception("Value cannot be less than zero!");
                _Settings_Pooling_Size_Max = value;
            }
        }
        public int Logging_Queries_Count
        {
            get
            {
                return _Logging_Queries_Count;
            }
        }
        public List<string> Logging_Queries
        {
            get
            {
                return _Logging_Queries;
            }
        }
        public bool Logging_Enabled
        {
            get
            {
                return _Logging_Enabled;
            }
            set
            {
                _Logging_Enabled = value;
            }
        }
        #endregion

        #region "Methods"
        public override void Connect()
        {
            _Raw_Connector.ConnectionString = "Host=" + _Settings_Host + "; Port=" + _Settings_Port + "; Database=" + _Settings_Database + "; UID=" + _Settings_User + "; Password=" + _Settings_Pass + "; Pooling=" + _Settings_Pooling + "; " + (_Settings_Timeout_Command > 0 ? "Default Command Timeout=" + _Settings_Timeout_Command + "; " : "") + (_Settings_Timeout_Connection > 0 ? "Connection Timeout=" + _Settings_Timeout_Connection + "; " : "") + (_Settings_Pooling ? "Min Pool Size=" + _Settings_Pooling_Size_Min + "; Max Pool Size=" + _Settings_Pooling_Size_Max + "; " : "") + _Settings_ConnectionString;
            _Raw_Connector.Open();
            if (_Logging_Enabled) _Logging_Queries = new List<string>();
        }
        public override void Disconnect()
        {
            _Raw_Connector.Close();
        }
        public override void ChangeDatabase(string database)
        {
            _Raw_Connector.ChangeDatabase(database);
        }
        public override Result Query_Read(string query)
        {
            lock (_Raw_Connector)
            {
                _Logging_Queries_Count++;
                if (_Logging_Enabled) _Logging_Add_Entry(query);
                Result Result = new Result();
                MySqlCommand Command = new MySqlCommand(query, _Raw_Connector);
                MySqlDataReader Reader = Command.ExecuteReader();
                ResultRow Row;
                int t;
                while (Reader.Read())
                {
                    Row = new ResultRow();
                    for (t = 0; t < Reader.FieldCount; t++) Row.Columns.Add(Reader.GetName(t), Reader.GetValue(t).ToString());
                    Result[-1] = Row;
                }
                Reader.Close();
                return Result;
            }
        }
        public override int Query_Count(string query)
        {
            return int.Parse((Query_Scalar(query) ?? 0).ToString());
        }
        public override object Query_Scalar(string query)
        {
            lock (_Raw_Connector)
            {
                _Logging_Queries_Count++;
                if (_Logging_Enabled) _Logging_Add_Entry(query);
                MySqlCommand Command = new MySqlCommand(query, _Raw_Connector);
                return Command.ExecuteScalar();
            }
        }
        public override void Query_Execute(string query)
        {
            lock (_Raw_Connector)
            {
                _Logging_Queries_Count++;
                if (_Logging_Enabled) _Logging_Add_Entry(query);
                MySqlCommand Command = new MySqlCommand(query, _Raw_Connector);
                Command.ExecuteNonQuery();
            }
        }
        public override bool CheckConnectionIsReady()
        {
            lock (_Raw_Connector)
            {
                try
                {
                    // Reopen the connector if it is not ready
                    if (_Raw_Connector.State == ConnectionState.Closed || _Raw_Connector.State == ConnectionState.Broken)
                        _Raw_Connector.Open();
                    return true;
                }
                catch { return false; }
            }
        }
        #endregion
    }
}