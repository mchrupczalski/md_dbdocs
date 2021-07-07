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
        public static string GetObjectTypeId(string inputString)
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

            string[] functions = { "FUNCTION", "FN", "SQL_SCALAR_FUNCTION", "IF", "SQL_INLINE_TABLE_VALUED_FUNCTION" }; // out: FN
            string[] procedures = { "PROCEDURE", "P", "SQL_STORED_PROCEDURE" }; // out: SP
            string[] roles = { "ROLE", "R", "DATABASE_ROLE" }; // out: RL
            string[] schemas = { "SCHEMA", "SCH" }; // out: SC
            string[] tables = { "TABLE", "U", "USER_TABLE" }; // out: TB
            string[] types = { "TYPE", "TTYPE", "TABLE_TYPE" }; // out: TT
            string[] views = { "VIEW", "V" }; // out: VW

            string output = "";
            string inputUp = inputString.ToUpper().Trim();


            if (functions.Contains(inputUp)){output = "FN";}
            else if (procedures.Contains(inputUp)){output = "SP";}
            else if (roles.Contains(inputUp)) { output = "RL"; }
            else if (schemas.Contains(inputUp)) { output = "SC"; }
            else if (tables.Contains(inputUp)) { output = "TB"; }
            else if (types.Contains(inputUp)) { output = "TT"; }
            else if (views.Contains(inputUp)) { output = "VW"; }

            return output;
        }
    }
}
