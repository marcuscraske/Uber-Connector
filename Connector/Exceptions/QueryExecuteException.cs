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
 *      Path:           /Exceptions/QueryExecuteException.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Minor improvements, new code-format and clean-up.
 * 
 * *********************************************************************************************************************
 * An exception thrown when an exception occurs when a query is executed.
 * *********************************************************************************************************************
 */
using System;
using System.Runtime.Serialization;

namespace UberLib.Connector
{
    /// <summary>
    /// An exception thrown when an exception occurs when a query is executed.
    /// </summary>
    [Serializable]
    public class QueryExecuteException : Exception
    {
        // Methods - Constructors **************************************************************************************
        public QueryExecuteException() : base() { }
        public QueryExecuteException(string message) : base(message) { }
        public QueryExecuteException(string message, Exception innerException) : base(message, innerException) { }
        public QueryExecuteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}