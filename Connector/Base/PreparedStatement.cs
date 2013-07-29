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
 *      Path:           /Base/PreparedStatement.cs
 * 
 *      Change-Log:
 *                      2013-07-20      Initial class created.
 *                      2013-07-29      Added Parameters property.
 * 
 * *********************************************************************************************************************
 * Holds the collection of parameters used for a prepared statement.
 * *********************************************************************************************************************
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace UberLib.Connector
{
    /// <summary>
    /// Holds the collection of parameters used for a prepared statement.
    /// </summary>
    public class PreparedStatement
    {
        // Fields ******************************************************************************************************
        internal Dictionary<string, object> parameters;
        internal string query;
        // Methods - Constructors
        /// <summary>
        /// Creates a new instance of a prepared statement.
        /// 
        /// Note: the query property will be null; this must be set!
        /// </summary>
        public PreparedStatement()
        {
            this.parameters = new Dictionary<string, object>();
            this.query = null;
        }
        /// <summary>
        /// Creates a new instance of a prepared statement.
        /// </summary>
        /// <param name="query">The query to be executed or the name of the procedure to be called.</param>
        public PreparedStatement(string query)
        {
            this.parameters = new Dictionary<string, object>();
            this.query = query;
        }
        // Methods - Properties ****************************************************************************************
        /// <summary>
        /// The query of the prepared statement; this can also be the name of a procedure.
        /// 
        /// Parameters in the query should be marked with ?name where name is the name of the attribute/parameter.
        /// 
        /// E.g. for the attribute 'test':
        /// INSERT INTO foo (test) VALUES(?test);
        /// </summary>
        public string Query
        {
            get
            {
                return query;
            }
            set
            {
                query = value;
            }
        }
        /// <summary>
        /// Gets/sets a parameter of the prepared statement.
        /// </summary>
        /// <param name="key">The name of the parameter, without @.</param>
        /// <returns>The value of a parameter or null if it does not exist.</returns>
        public object this[string key]
        {
            get
            {
                return parameters.ContainsKey(key) ? parameters[key] : null;
            }
            set
            {
                parameters[key] = value;
            }
        }
        /// <summary>
        /// Gets/sets the internal parameter storage.
        /// </summary>
        public Dictionary<string, object> Parameters
        {
            get
            {
                return this.parameters;
            }
            set
            {
                this.parameters = value;
            }
        }
    }
}
