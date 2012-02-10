/*
 * Creative Commons Attribution-ShareAlike 3.0 unported
 * Connectors\MSSQL.cs
 * 
 * Provides an interface for Microsoft SQL servers; this is untested and incomplete!!!!!!!!!!
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace UberLib.Connector.Connectors
{
    public class MSSQL : Connector
    {
        private SqlConnection _Raw_Connector = new SqlConnection();
        public override void Connect()
        {
            _Raw_Connector.ConnectionString = "";
            _Raw_Connector.Open();
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
                SqlCommand Command = new SqlCommand(query, _Raw_Connector);
                SqlDataReader Reader = Command.ExecuteReader();
                ResultRow Row;
                int t;
                while (Reader.Read())
                {
                    Row = new ResultRow();
                    for (t = 0; t < Reader.FieldCount; t++) Row.Columns.Add(Reader.GetName(t), Reader.GetValue(t));
                    Result[-1] = Row;
                }
                Reader.Close();
                return Result;
            }
        }
        public override int Query_Count(string query)
        {
            lock(_Raw_Connector)
            {
                return (int)Query_Scalar(query);
            }
        }
        public override object Query_Scalar(string query)
        {
            lock (_Raw_Connector)
            {
                _Logging_Queries_Count++;
                if (_Logging_Enabled) _Logging_Add_Entry(query);
                SqlCommand Command = new SqlCommand(query, _Raw_Connector);
                return Command.ExecuteScalar();
            }
        }
        public override void Query_Execute(string query)
        {
            lock (_Raw_Connector)
            {
                _Logging_Queries_Count++;
                if (_Logging_Enabled) _Logging_Add_Entry(query);
                SqlCommand Command = new SqlCommand(query, _Raw_Connector);
                Command.ExecuteNonQuery();
            }
        }
    }
}