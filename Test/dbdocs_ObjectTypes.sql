/*
<dbdocs>
-----
File: dbdocs_ObjectTypes.sql
Created Date: Sunday, 27/06/2021 16:42:53
Author: Mateusz Chrupczalski - MC - ( mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com )
-----
Last Modified: 27/06/2021 06:nn:59
   WeekDay: Sunday
   Date: 27/06/2021 16:42:53
Modified By: Mateusz Chrupczalski - MC - ( <<authoremail1>> )
-----
Type: Function Stored_Procedure Table Trigger
Description: 
Parameters: 
   NA
   NA
Return Value: 
   NA
   NA
-----
Programming Notes: 

-----
Change_Log:
Date               	| By |	Comments
-------------------	| -- |	--------------------------------------------------------------------------------

-----
</dbdocs>
 */


DROP TABLE IF EXISTS [dbo].[dbdocs_ObjectTypes];
GO

CREATE TABLE [dbo].[dbdocs_ObjectTypes](
     ObjectTypeId           NVARCHAR(5)
    ,ObjectTypeDesc         NVARCHAR(MAX)
    ,HasColumns             BIT
    ,HasParameters          BIT
    ,ExtProp_UseSecondId    BIT                 -- Extended properties for 'User Defined Table Data Type' columns are linked to secondary id
    ,IsMajorType            BIT                 -- Type for objects like Tables, Views, Procedures
)

INSERT INTO [dbo].[dbdocs_ObjectTypes]
VALUES ('FN'    ,'SQL_SCALAR_FUNCTION'              ,0,1,0,1)
      ,('IF'    ,'SQL_INLINE_TABLE_VALUED_FUNCTION' ,0,1,0,1)
      ,('TF'    ,'SQL_TABLE_VALUED_FUNCTION'        ,0,1,0,1)
      ,('U'     ,'USER_TABLE'                       ,1,0,0,1)
      ,('P'     ,'SQL_STORED_PROCEDURE'             ,0,1,0,1)
      ,('V'     ,'VIEW'                             ,1,0,0,1)
      ,('uSCH'  ,'SCHEMA'                           ,0,0,0,1)
      ,('uType' ,'TABLE_TYPE'                       ,1,0,1,1)
      ,('R'     ,'DATABASE_ROLE'                    ,0,0,0,1)
      ,('COL'   ,'COLUMN'                           ,0,0,0,0)
      ,('tCOL'  ,'TYPE_COLUMN'                      ,0,0,1,0)
      ,('PAR'   ,'PARAMETER'                        ,0,0,0,0)