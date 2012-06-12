/*
 * Creative Commons Attribution-ShareAlike 3.0 unported
 * \ExampleMySQL\Program.cs
 * 
 * An example of using MySQL with the Connector library.
 */

// Standard imports
using System;
using System.Collections.Generic;

// Import base Connector classes
using UberLib.Connector;
// Import Connector wrappers - for connecting to data-sources
using UberLib.Connector.Connectors;

namespace ExampleMySQL
{
    class Program
    {
        static void Main(string[] args)
        {
            // We start by making the MySQL object - which inherits Connector
            MySQL connection = new MySQL();

            // Next we set the specific connection properties for this object;
            // these are also default settings and do not need to be specified
            // -- just an example of how-to though
            connection.Settings_Host = "127.0.0.1";             // IP of host
            connection.Settings_Port = 3306;                    // Port of MySQL DB on host
            connection.Settings_Database = "name_of_database";  // Database name
            connection.Settings_User = "root";                  // Database username
            connection.Settings_Pass = "";                      // Database password

            // Next we connect to the data-source, which is a MySQL database in this scenario
            connection.Connect(); // This method is inherited from Connector class

            // We'll do an example query on the following table called "products":
            /*
             *  pid     | title     | category
             *  --------|-----------|------------
             * 0        | ball      | sports
             * 1        | racket    | sports
             * 2        | paper     | office
             * 3        | pan       | kitchen
             */

            // In this scenario we're going to create a list of products from the database
            List<Product> products = new List<Product>();
            Product prodTemp;
            // Go through each row in a query - this is very OOP
            foreach (ResultRow product in connection.Query_Read("SELECT * FROM products ORDER BY pid  ASC"))
            {
                prodTemp = new Product();
                // Copy values
                prodTemp.productID = (int)product.Get("pid");   // .Get(<column>) will always return
                                                                // the .NET equiv. of the data-type

                prodTemp.title = product["title"];              // [<value>] will always return string
                prodTemp.category = product["category"];
                // Add to list
                products.Add(prodTemp);
            }

            // We can now just print-out our products - this technique is useful for e.g. MVC
            foreach (Product product in products)
                Console.WriteLine(product.productID + "\t" + product.category + "\t" + product.title);

            // Disconnect data-source
            connection.Disconnect();
        }
    }
    public struct Product
    {
        public int productID;
        public string title, category;
    }
}
