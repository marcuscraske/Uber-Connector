/*
 * License:     Creative Commons Attribution-ShareAlike 3.0 unported
 * File:        Exceptions\DuplicateEntryException.cs
 * Authors:     limpygnome              limpygnome@gmail.com
 * 
 * Thrown when a duplicate piece of data is inserted into the database for a unique column.
 */
using System;
using System.Runtime.Serialization;

namespace UberLib.Connector
{
    [Serializable]
    public class DuplicateEntryException : Exception
    {
        private string column;
        public DuplicateEntryException(string column) : base() { this.column = column; }
        public DuplicateEntryException(string message, string column) : base(message) { this.column = column; }
        public DuplicateEntryException(string message, Exception innerException, string column) : base(message, innerException) { this.column = column; }
        public DuplicateEntryException(SerializationInfo info, StreamingContext context, string column) : base(info, context) { this.column = column; }
        public string Column
        {
            get
            {
                return column;
            }
        }
    }
}
