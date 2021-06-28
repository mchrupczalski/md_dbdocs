/*
 * File: Table.dbdocs_updateReport.sql
 * Created Date: Thursday, 24/06/2021 10:11:21
 * Author: Mateusz Chrupczalski - MC - ( mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com )
 * -----
 * Last Modified: Thursday, 24/06/2021 10:11:39
 * Modified By: Mateusz Chrupczalski - MC - ( mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com )
 * -----
 * Type: Function Stored_Procedure Table Trigger
 * Description: 
 * Parameters: 
 *    NA
 *    NA
 * Return Value: 
 *    NA
 *    NA
 * -----
 * Programming Notes: 
 * 
 * -----
 * HISTORY:
 * Date               	By	Comments
 * -------------------	---	---------------------------------------------------------
 */



DROP TABLE IF EXISTS [dbo].[dbdocs_updateReport];
GO

CREATE TABLE [dbo].[dbdocs_updateReport](
      [Module]              NVARCHAR(100)       NULL
    , [ParentObjectType]    SYSNAME         NOT NULL
    , [ParentObjectName]    SYSNAME         NOT NULL
    , [ParentObjectSchema]  SYSNAME         NOT NULL
    , [ChildObjectType]     NVARCHAR(100)   NOT NULL
    , [ChildObjectName]     SYSNAME         NOT NULL
    , [OldDesc]             NVARCHAR(MAX)       NULL
    , [NewDesc]             NVARCHAR(MAX)       NULL
    , [UpdateResult]        NVARCHAR(100)   NOT NULL
    , [FilePath]            NVARCHAR(MAX)   NOT NULL    
);
GO