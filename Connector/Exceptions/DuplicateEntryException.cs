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
 *      Path:           /Exceptions/DuplicateEntryException.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Minor improvements, new code-format and clean-up.
 * 
 * *********************************************************************************************************************
 * An exception thrown when a tuple with a duplicate value is inserted into a database.
 * *********************************************************************************************************************
 */
using System;
using System.Runtime.Serialization;

namespace UberLib.Connector
{
    /// <summary>
    /// An exception thrown when a tuple with a duplicate value is inserted into a database.
    /// </summary>
    [Serializable]
    public class DuplicateEntryException : Exception
    {
        // Fields ******************************************************************************************************
        private string attribute;
        // Methods - Constructors **************************************************************************************
        public DuplicateEntryException(string attribute) : base() { this.attribute = attribute; }
        public DuplicateEntryException(string message, string attribute) : base(message) { this.attribute = attribute; }
        public DuplicateEntryException(string message, Exception innerException, string attribute) : base(message, innerException) { this.attribute = attribute; }
        public DuplicateEntryException(SerializationInfo info, StreamingContext context, string attribute) : base(info, context) { this.attribute = attribute; }
        // Methods - Properties ****************************************************************************************
        /// <summary>
        /// The attribute which has a non-unique value.
        /// </summary>
        public string Attribute
        {
            get
            {
                return attribute;
            }
            internal set
            {
                attribute = value;
            }
        }
    }
}
