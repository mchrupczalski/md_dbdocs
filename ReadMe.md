# md_dbdocs
Markdown database documentation done right.  

Supported database objects:
- FUNCTION
- PROCEDURE
- ROLE
- SCHEMA
- TABLE
- TYPE
- VIEW

### How to use

#### Project setup
Each *.sql file within the project should contain a header comments section, written in [YAML](https://yaml.org).  
Header comments template can be added to new files added to projects developed with [Azure Data Studio](https://azure.microsoft.com/en-gb/services/developer-tools/data-studio/) with use of extension [psioniq File Header](https://marketplace.visualstudio.com/items?itemName=psioniq.psi-header) configured as per [this file]().  

Header comments definition:
| Section          | Item               | Sub-Item  | Auto   | Description
|---               |---                 |---        |---     |---
| # File Details   | File_Name          |           | TRUE   | File name
| # File Details   | Created            | WeekDay   | TRUE   | Week day name when file was created
| # File Details   | Created            | Date      | TRUE   | Date when file was created
| # File Details   | Author             | Name      | TRUE*  | Name of the person who created the file (*must be defined in 'psioniq File Header')
| # File Details   | Author             | Initials  | TRUE*  | Initials of the person who created the file (*must be defined in 'psioniq File Header')
| # File Details   | Author             | Email     | TRUE*  | Contact email to the person who created the file (*must be defined in 'psioniq File Header')
| # File Details   | Last_Modified      | WeekDay   | TRUE   | Week day name when file was last modified
| # File Details   | Last_Modified      | Date      | TRUE   | Date when file was last modified
| # File Details   | Modified_By        | Name      | TRUE*  | Name of the person who modified the file (*must be defined in 'psioniq File Header')
| # File Details   | Modified_By        | Initials  | TRUE*  | Initials of the person who modified the file (*must be defined in 'psioniq File Header')
| # File Details   | Modified_By        | Email     | TRUE*  | Contact email to the person who modified the file (*must be defined in 'psioniq File Header')
| # Object Details | Module             |           |        | Name of logical entity an object belongs to
| # Object Details | Schema_Name        |           |        | Name of schema an object belongs to
| # Object Details | Object_Name        |           | TRUE*  | Name of the database object (*default - file name)
| # Object Details | Description        |           |        | Object extended property ('MS_Description')
| # Object Details | Fields             | List item |        | TABLE or VIEW specific. Key, Value pairs, where Key is a field name and Value is field extended description ('MS_Description')
| # Object Details | Parameters         | List item |        | FUNCTION or PROCEDURE specific. Key, Value pairs, where Key is a parameter name (with @) and Value is parameter extended description ('MS_Description')
| # Object Details | Return_Value       |           |        | FUNCTION or PROCEDURE specific. Description of what is being returned with the t-sql 'RETURN' statement 
| # Notes          | Programming_Notes  | List item |        | Developer notes
| # Notes          | \<diagram>         |           |        | See [Supported Tags](#supported-tags)
| # Change_Log     | \<change_log>      |           |        | See [Supported Tags](#supported-tags)

SQL File Header Example:
```
/*
 * <dbdocs>
 * # File Details:
 *     File_Name: test_file.sql
 *     Created: 
 *         WeekDay: Sunday
 *         Date: 20/06/2021 15:32:08
 *     Author:
 *         Name: Mateusz Chrupczalski
 *         Initials: MC
 *         Email: m.chrupczalski@outlook.com
 * # -----
 *     Last_Modified:
 *         WeekDay: Sunday
 *         Date: 20/06/2021 16:02:38
 *     Modified_By:
 *         Name: Mateusz Chrupczalski
 *         Initials: MC
 *         Email: m.chrupczalski@outlook.com
 * 
 * # Object Details:
 *     Module: Core
 *     Schema_Name: lu
 *     Object_Name: testTable
 *     Description: This is table description
 *     Fields:
 *         - Field1: Record Id
 *         - Field2: Some data field
 * 
 *     Parameters: 
 *         - Parameter1: Parameter1_Description
 * 
 *     Return_Value: Returned_value_description
 * 
 * # Notes:
 *     Programming_Notes: 
 *         - Note1
 *         - Note2
 * 
 *     <diagram>    
 *         ::: mermaid
 *         flowchart TD;
 *         A1((Start)) --> A2[\ProcName\];
 *         subgraph INFO[Get Tables in Procedure and Last Import Date];
 *         B1[Select data from sys tables] --- B2[Get data from dat.LogDataImports];
 *         end;
 *         A2 --> INFO;
 *         INFO --> C1[\Return data\];
 *         C1 --> D1((End));
 *         :::
 *     </diagram>
 * 
 * # Change Log:
 * <change_log>
 * Date                 | By |	Comments
 * -------------------	| -- |	---------------------------------------------------------
 * 
 * -----
 * </change_log>
 * </dbdocs>
 */
```

#### Supported tags

| tag           | closing tag       | description
|---            |---                |---
| \<dbdocs>     | \<\dbdocs>        | main tag, containing all info and other tags
| \<diagram>    |  \<\diagram>      | [mermaid](https://mermaid-js.github.io/mermaid/#/) diagram code
| \<change_log> | \<\change_log>    | change log table

#### Prerequisites
- [Node.js & npm](https://www.npmjs.com/get-npm)
  - Download and install
- [sethen/markdown-include](https://github.com/sethen/markdown-include)
  - In the Command Prompt run: 
    - npm install markdown-include -g


#### Setup
#### Extend database objects
#### Create readme for database

---
OLD DOCS
# How to Use:
1. Create main readme file which defines general document structure
   1. use: 
      1. **#inlcude "path/to/file.md"** - to merge file into output
      2. **!heading** - at the end of #header, to include header in the table of content
2. Create config.json file for markdown-include with the following content:
    ```
    {
        "build" : "DB_Docs_Info.md",
        "files" : ["ReadMe.md"],
        "tableOfContents": {
            "heading": "# Table of Contents"
        }
    }
    ```
3. Run application and follow prompts in console


# Examples
### _ReadMe.md
```
# PDM Database Documentation


#include "info_files/01_Document_info.md"

---

#include "info_files/02_Schemas.md"

---
```

### include_config.json
```
{
	"build" : "ReadMe.md",
	"files" : ["_ReadMe.md"],
	"tableOfContents": {
		"heading": "# Table of Contents"
	}
}
```