using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md_dbdocs.app.Helpers
{
    public static class SqlObjectTypeTranslator
    {    
        // this takes object type as input from both server and project files and returns Id for it
        public string GetObjectTypeId(string inputString)
        {
            /* project files
             * FUNCTION
             * PROCEDURE
             * ROLE
             * SCHEMA
             * TABLE
             * TYPE
             * VIEW
             */

            /* server objects can be:
             * 
             * FN    - SQL_SCALAR_FUNCTION
             * IF    - SQL_INLINE_TABLE_VALUED_FUNCTION
             * P     - SQL_STORED_PROCEDURE
             * R     - DATABASE_ROLE
             * SCH   - SCHEMA
             * TTYPE - TABLE_TYPE
             * U 	 - USER_TABLE
             * V 	 - VIEW
             */

            /* return string can be:
             * FN - Functions
             * SP - Procedures
             * TB - Tables
             * VW - Views
             * TT - Type
             * RL - Role
             * SC - Schema
             */

            string[] functions = { "FUNCTION", "FN", "SQL_SCALAR_FUNCTION", "IF", "SQL_INLINE_TABLE_VALUED_FUNCTION" };
            string[] procedures = { "PROCEDURE", "P", "SQL_STORED_PROCEDURE" };
            string[] roles = { "ROLE", "R", "DATABASE_ROLE" };
            string[] schemas = { "SCHEMA", "SCH" };
            string[] tables = { "TABLE", "U", "USER_TABLE" };
            string[] types = { "TYPE", "TTYPE", "TABLE_TYPE" };
            string[] views = { "VIEW", "V" };

            string output = "";


            if (functions.Contains(inputString.ToUpper()))
            {
                output = "FN";
            }
            else if (procedures.Contains)
            {

            }

        }
    }
}
