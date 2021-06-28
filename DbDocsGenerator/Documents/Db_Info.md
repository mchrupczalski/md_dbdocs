# PDM Database Documentation <!-- omit in toc -->

***
### Content: <!-- omit in toc -->
- [1 - Document Info](#1---document-info)
  - [1.1 - Document structure](#11---document-structure)
  - [1.2 - Purpose](#12---purpose)
  - [1.3 - Requirements](#13---requirements)
  - [1.4 - General Info](#14---general-info)
- [2 - Schemas](#2---schemas)
  - [2.1 - app](#21---app)
  - [2.2 - dat](#22---dat)
  - [2.3 - err](#23---err)
  - [2.4 - lib](#24---lib)
  - [2.5 - lu](#25---lu)
  - [2.6 - sap](#26---sap)
  - [2.7 - stg](#27---stg)
- [3 - Modules](#3---modules)
  - [3.1 - Core](#31---core)
    - [3.1.1 - Core Tables - Overview](#311---core-tables---overview)
      - [3.1.1.1 - Core Tables - dat.LogAgentCalls](#3111---core-tables---datlogagentcalls)
    - [3.1.2 - Core Stored Procedures](#312---core-stored-procedures)
      - [3.1.2.1 - Core Stored Procedures - app.spInfoTablesInProcedure](#3121---core-stored-procedures---appspinfotablesinprocedure)
---

## 1 - Document Info

### 1.1 - Document structure
This document is divided in per application module.  

### 1.2 - Purpose

### 1.3 - Requirements

### 1.4 - General Info
Deploy with publish.  
SQL File Header in YAML parsed to create extended properties and markdown documentation.  
Run UpdateDbDocs console application from DbDocsGenerator project.  
UpdateDbDocs app parse all *.sql files in specified folder and sub folders extracting yaml info from the header.  
XML info need to be structured as follows:  
```
/* -- Db Object info --
<xml>
</xml>
-- Db Object info -- */
```
The above structure can be automatically included in each new file created with [Azure Data Studio](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15) or [Visual Studio Code](https://code.visualstudio.com) by using [psioniq File Header](https://marketplace.visualstudio.com/items?itemName=psioniq.psi-header) extension.  
The extension configuration file can be found [here](assets/psioniq_config.txt) (This file is prepopulated with information, please replace with your info before using it).  
UpdateDbDocs then query object info from the server and produces markdown description of an object.  
The markdown file with the object description is stored in output directory ./Docs/Objects with file name as "<object_type>.<schema_name>.<object_name>.md".  
Each *.md file should be injected to this document via [markdown include](https://github.com/sethen/markdown-include).


---

## 2 - Schemas
### 2.1 - app
- user access schema
- exposes data to application
### 2.2 - dat
- used for application data tables
### 2.3 - err
- error reporting
### 2.4 - lib
- library of functions
- contains supporting procedures (procedures not called directly)
### 2.5 - lu
- predefined data for lookup fields
### 2.6 - sap
- contains data coming from SAP exports
### 2.7 - stg
- temporary data storage for incoming data from the master database (EveImport)

---

## 3 - Modules
### 3.1 - Core
- Provides logic for general functionality of the database
#### 3.1.1 - Core Tables - Overview
![core tables diagram](assets/Core.png)
##### 3.1.1.1 - Core Tables - dat.LogAgentCalls

#### 3.1.2 - Core Stored Procedures
##### 3.1.2.1 - Core Stored Procedures - app.spInfoTablesInProcedure
- **Description**
  - Procedure checks what tables are used in a Stored Procedures and get details of last data load
- **Input Parameters**
  - @ProcName - Stored Procedure name to retrieve info about
- **Flow Chart**
::: mermaid
flowchart TD;
    A1((Start)) --> A2[\ProcName\];
    subgraph INFO[Get Tables in Procedure and Last Import Date];
      B1[Select data from sys tables] --- B2[Get data from dat.LogDataImports];
    end;
    A2 --> INFO;
    INFO --> C1[\Return hhhhh data\];
    C1 --> D1((End));
:::

