/*
 * License:     Creative Commons Attribution-ShareAlike 3.0 unported
 * File:        Exceptions\ConnectionFailureException.cs
 * Authors:     limpygnome              limpygnome@gmail.com
 * 
 * Thrown when a connection cannot be made to a data-source.
 */
using System;
using System.Runtime.Serialization;

namespace UberLib.Connector
{
    [Serializable]
    public class ConnectionFailureException : Exception
    {
        public ConnectionFailureException() : base() { }
        public ConnectionFailureException(string message) : base(message) { }
        public ConnectionFailureException(string message, Exception innerException) : base(message, innerException) { }
        public ConnectionFailureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}