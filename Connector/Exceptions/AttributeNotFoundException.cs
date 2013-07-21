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
 *      Path:           /Exceptions/AttributeNotFoundException.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Created initial class.
 * 
 * *********************************************************************************************************************
 * An exception thrown when a specified attribute cannot be found.
 * *********************************************************************************************************************
 */
using System;
using System.Runtime.Serialization;

namespace UberLib.Connector
{
    /// <summary>
    /// An exception thrown when a specified attribute cannot be found.
    /// </summary>
    [Serializable]
    public class AttributeNotFoundException : Exception
    {
        // Fields ******************************************************************************************************
        private string attribute;
        // Methods - Constructors **************************************************************************************
        public AttributeNotFoundException() : base() { }
        public AttributeNotFoundException(string message) : base(message) { }
        public AttributeNotFoundException(string message, string attribute) : base(message) { }
        public AttributeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        // Methods - Properties ****************************************************************************************
        /// <summary>
        /// The attribute identifier/key which could not be found.
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