/*
 * License:     Creative Commons Attribution-ShareAlike 3.0 unported
 * File:        Exceptions\QueryException.cs
 * Authors:     limpygnome              limpygnome@gmail.com
 * 
 * Thrown when a query causes an error.
 */
using System;
using System.Runtime.Serialization;

namespace UberLib.Connector
{
    [Serializable]
    public class QueryExecuteException : Exception
    {
        public QueryExecuteException() : base() { }
        public QueryExecuteException(string message) : base(message) { }
        public QueryExecuteException(string message, Exception innerException) : base(message, innerException) { }
        public QueryExecuteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}