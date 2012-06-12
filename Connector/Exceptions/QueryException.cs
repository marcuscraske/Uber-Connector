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
    public class QueryException : Exception
    {
        public QueryException() : base() { }
        public QueryException(string message) : base(message) { }
        public QueryException(string message, Exception innerException) : base(message, innerException) { }
        public QueryException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}