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
 *      Path:           /Exceptions/ConnectionFailureException.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Minor improvements, new code-format and clean-up.
 * 
 * *********************************************************************************************************************
 * An exception thrown when a connection cannot be made to a data-source.
 * *********************************************************************************************************************
 */
using System;
using System.Runtime.Serialization;

namespace UberLib.Connector
{
    /// <summary>
    /// An exception thrown when a connection cannot be made to a data-source.
    /// </summary>
    [Serializable]
    public class ConnectionFailureException : Exception
    {
        // Methods - Constructors **************************************************************************************
        public ConnectionFailureException() : base() { }
        public ConnectionFailureException(string message) : base(message) { }
        public ConnectionFailureException(string message, Exception innerException) : base(message, innerException) { }
        public ConnectionFailureException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}