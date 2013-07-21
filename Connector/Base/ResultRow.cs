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
 *      Path:           /Base/ResultRow.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Code cleanup, minor improvements and new comment header.
 *                                      Moved from Result.cs into a separate file.
 * 
 * *********************************************************************************************************************
 * A model for representing a result's tuple.
 * *********************************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Collections;

namespace UberLib.Connector
{
    /// <summary>
    /// A model for representing a result's tuple.
    /// </summary>
    public class ResultRow
    {
        // Fields ******************************************************************************************************
        /// <summary>
        /// Stores the values of the tuple, associated with their attribute name.
        /// </summary>
        public Dictionary<string, object> attributes;
        /// <summary>
        /// Similar to columns field, but for binary data (blob).
        /// </summary>
        public Dictionary<string, byte[]> attributesByteArray;
        // Methods - Constructors **************************************************************************************
        public ResultRow()
        {
            attributes = new Dictionary<string, object>();
            attributesByteArray = null;
        }
        // Methods - Properties ****************************************************************************************
        /// <summary>
        /// Gets the dot-net (.NET) object equivalent of an attribute's value.
        /// </summary>
        /// <param name="attribute">The identifier/name of the attribute.</param>
        /// <returns>The string representation of the attribute; returns an empty-string if the value is null.</returns>
        /// <exception cref="UberLib.Connector.AttributeNotFoundException">Thrown when the specified attribute cannot be found.</exception>
        public string this[string attribute]
        {
            get
            {
                if (!attributes.ContainsKey(attribute))
                    throw new AttributeNotFoundException("The attribute '" + attribute + "' is not in the query result!", attribute);
                return (attributes[attribute] ?? "").ToString();
            }
            internal set
            {
                attributes[attribute] = value;
            }
        }
        // Methods - Accessors *****************************************************************************************
        /// <summary>
        /// Gets the dot-net (.NET) object equivalent of an attribute's value.
        /// </summary>
        /// <param name="attribute">The identifier/name of the attribute.</param>
        /// <returns>.Dot-net object equivalent; simply type-cast.</returns>
        /// <exception cref="UberLib.Connector.AttributeNotFoundException">Thrown when the specified attribute cannot be found.</exception>
        public object get(string attribute)
        {
            if (!attributes.ContainsKey(attribute))
                throw new AttributeNotFoundException("The attribute '" + attribute + "' is not in the query result!", attribute);
            return attributes[attribute];
        }
        /// <summary>
        /// Gets an attribute's value as a type by automatically casting it; returns the default value of the type
        /// if the attribute cannot be found.
        /// </summary>
        /// <typeparam name="T">The data-type to be returned.</typeparam>
        /// <param name="attribute">The attribute of which to return.</param>
        /// <returns>The value of the attribute as the specified data-type.</returns>
        public T get2<T>(string attribute)
        {
            return attributes.ContainsKey(attribute) ? (T)attributes[attribute] : default(T);
        }
        /// <summary>
        /// Gets the byte-array/binary-data of an attribute; this will only be available for attributes with
        /// actual binary data, cannot be used for conversion purposes.
        /// </summary>
        /// <param name="attribute">The identifier/name of the attribute.</param>
        /// <returns>Byte array of the binary data for the specified attribute.</returns>
        /// <exception cref="UberLib.Connector.AttributeNotFoundException">Thrown when the specified attribute cannot be found.</exception>
        public byte[] getByteArray(string attribute)
        {
            if (attributesByteArray != null && attributesByteArray.ContainsKey(attribute))
                return attributesByteArray[attribute];
            else if (attributes.ContainsKey(attribute))
                return new byte[] { };
            else
                throw new AttributeNotFoundException("The attribute '" + attribute + "' is not in the query result!", attribute);
        }
    }
}