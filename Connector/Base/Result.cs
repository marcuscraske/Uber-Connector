/*
 * License:     Creative Commons Attribution-ShareAlike 3.0 unported
 * File:        Base\Result.cs
 * Authors:     limpygnome              limpygnome@gmail.com
 * 
 * Provides a shared data-set structure for returned data.
 */
using System;
using System.Collections.Generic;
using System.Collections;

namespace UberLib.Connector
{
    public class Result : IEnumerable
    {
        public List<ResultRow> Rows = new List<ResultRow>();
        public ResultRow this[int row]
        {
            get
            {
                return Rows[row];
            }
            set
            {
                if (row == -1) Rows.Add(value);
                else Rows[row] = value;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Rows.GetEnumerator();
        }
    }
    public class ResultRow
    {
        public Dictionary<string, object> Columns = new Dictionary<string, object>();
        public Dictionary<string, byte[]> ColumnsByteArray = null;
        public string this[string column]
        {
            get
            {
                if (!Columns.ContainsKey(column))
                    throw new Exception("The column '" + column + "' is not in the query result!");
                return (Columns[column] ?? "").ToString();
            }
            set
            {
                Columns[column] = value;
            }
        }
        /// <summary>
        /// Gets the actual .NET object equiv of the columns value.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public object Get(string column)
        {
            return Columns[column];
        }
        /// <summary>
        /// Gets the byte-array of the object.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public byte[] GetByteArray(string column)
        {
            if (ColumnsByteArray != null && ColumnsByteArray.ContainsKey(column))
                return ColumnsByteArray[column];
            else if (Columns.ContainsKey(column))
                return null;
            else
                throw new Exception("The column '" + column + "' is not in the query result!");
        }
    }
}
