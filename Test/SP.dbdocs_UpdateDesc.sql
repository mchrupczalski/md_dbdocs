/*
 * File: SP.dbdocs_UpdateDesc.sql
 * Created Date: Thursday, 24/06/2021 10:21:47
 * Author: Mateusz Chrupczalski - MC - ( mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com )
 * -----
 * Last Modified: Thursday, 24/06/2021 13:30:20
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
        
        SELECT o.[type], o.type_desc
        FROM sys.objects AS o
        GROUP BY o.[type], o.type_desc

        TT	- TYPE_TABLE
        FN	- SQL_SCALAR_FUNCTION
        IF	- SQL_INLINE_TABLE_VALUED_FUNCTION
        SQ	- SERVICE_QUEUE
        F 	- FOREIGN_KEY_CONSTRAINT
        U 	- USER_TABLE
        D 	- DEFAULT_CONSTRAINT
        PK	- PRIMARY_KEY_CONSTRAINT
        V 	- VIEW
        S 	- SYSTEM_TABLE
        IT	- INTERNAL_TABLE
        P 	- SQL_STORED_PROCEDURE
        
 * 
 * -----
 * HISTORY:
 * Date               	By	Comments
 * -------------------	---	---------------------------------------------------------
 */


/*
DROP PROCEDURE IF EXISTS [dbo].[dbdocs_UpdateDesc];
GO

CREATE PROCEDURE [dbo].[dbdocs_UpdateDesc]
     @ObjectModule      NVARCHAR(100)
    ,@ObjectSchema      SYSNAME
    ,@ObjectName        SYSNAME
    ,@ObjectDesc        NVARCHAR(MAX)
    ,@TableFields       dbo.dbDocs_Fields READONLY
    ,@FilePath          NVARCHAR(MAX)
AS

    BEGIN
        
        

    END

;
GO
*/


DECLARE @ObjectModule      NVARCHAR(100)
DECLARE @ObjectSchema      SYSNAME
DECLARE @ObjectName        SYSNAME
DECLARE @ObjectDesc        NVARCHAR(MAX)
DECLARE @TableFields       dbo.dbDocs_Fields
DECLARE @FilePath          NVARCHAR(MAX)

SET @ObjectModule = 'Core'
SET @ObjectSchema = 'dat'
SET @ObjectName = 'LogDataImports'
SET @ObjectDesc = 'This is table desc from dbdocs'

INSERT INTO @TableFields
VALUES ('LogId', 'Record identificator from dbdocs')
      ,('ImportDateTime', 'When was it imported from dbdocs')

SET @FilePath = 'C:\path\to\file.sql'


-- Get object details
DECLARE @ObjectDetails TABLE(IsObject BIT, ObjectType NVARCHAR(100));

INSERT INTO @ObjectDetails
SELECT 
    (CASE WHEN COUNT(o.name) > 0 THEN 1 ELSE 0 END) AS Result, 
    (CASE WHEN o.type IN ('TT','U') THEN 'table'
            WHEN o.type IN ('P') THEN 'procedure'
            WHEN o.type IN ('P') THEN 'procedure'
    END) AS ObjectType
FROM sys.objects AS o
    INNER JOIN sys.schemas AS s
        ON o.schema_id = s.schema_id
WHERE(
    (o.name = @ObjectName) 
    AND 
    (s.name = @ObjectSchema)
)
GROUP BY o.type;

-- check if object exists and get its type
DECLARE @IsObject BIT = (SELECT IsObject FROM @ObjectDetails);
DECLARE @ObjectType NVARCHAR(100) = (SELECT ObjectType FROM @ObjectDetails);

-- get object extended properties
DECLARE @ExtProp TABLE(ObjName NVARCHAR(MAX), ObjType NVARCHAR(MAX), ObjDesc NVARCHAR(MAX))
INSERT INTO @ExtProp
SELECT 
     f.objname
    ,f.objtype
    ,CAST(f.[value] AS NVARCHAR(MAX))
FROM fn_listextendedproperty(NULL, 'schema', @ObjectSchema, @ObjectType, @ObjectName, NULL, default) AS f
WHERE(f.name = 'MS_Description');

-- populate report table
INSERT INTO [dbo].[dbdocs_updateReport](
     [Module]
    ,[ParentObjectType]
    ,[ParentObjectName]
    ,[ParentObjectSchema]
    ,[ChildObjectType]
    ,[ChildObjectName]
    ,[OldDesc]
    ,[NewDesc]
    ,[UpdateResult]
    ,[FilePath]
)
SELECT
     @ObjectModule      AS Module
    ,@ObjectType        AS ParentObjectType
    ,@ObjectName        AS ParentObjectName
    ,@ObjectSchema      AS ParentObjectSchema
    ,e.ObjType          AS ChildObjectType
    ,e.ObjName          AS ChildObjectName
    ,e.ObjDesc          AS OldDesc
    ,@ObjectDesc        AS NewDesc
    ,CASE WHEN e.ObjDesc = @ObjectDesc THEN 'The Same'
          WHEN e.ObjDesc IS NULL AND @ObjectDesc IS NOT NULL THEN 'Added'
          WHEN e.ObjDesc IS NOT NULL AND @ObjectDesc IS NULL THEN 'Removed'
          WHEN e.ObjDesc IS NOT NULL AND @ObjectDesc IS NOT NULL THEN 'Updated'
          WHEN e.ObjDesc IS NULL AND @ObjectDesc IS NULL THEN 'Not Defined'
     END                AS UpdateResult
    ,@FilePath          AS FilePath
FROM @ExtProp AS e



-- get object child extended properties
DELETE FROM @ExtProp;

SELECT f.name, f.objname, f.objtype, f.[value] 
FROM fn_listextendedproperty(NULL, 'schema', @ObjectSchema, @ObjectType, @ObjectName, 'column', default) AS f