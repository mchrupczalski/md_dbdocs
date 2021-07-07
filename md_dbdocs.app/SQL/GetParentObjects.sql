-- create table with supported object types
DECLARE @ObjTypes TABLE
(
     ObjectTypeId           NVARCHAR(5)
    ,ObjectTypeDesc         NVARCHAR(MAX)
    ,HasColumns             BIT
    ,HasParameters          BIT
    ,ExtProp_UseSecondId    BIT                 -- Extended properties for 'User Defined Table Data Type' columns are linked to secondary id
    ,IsMajorType            BIT                 -- Type for objects like Tables, Views, Procedures
)
INSERT INTO @ObjTypes
VALUES ('FN'    ,'SQL_SCALAR_FUNCTION'              ,0,1,0,1)
      ,('IF'    ,'SQL_INLINE_TABLE_VALUED_FUNCTION' ,0,1,0,1)
      ,('TF'    ,'SQL_TABLE_VALUED_FUNCTION'        ,0,1,0,1)
      ,('U'     ,'USER_TABLE'                       ,1,0,0,1)
      ,('P'     ,'SQL_STORED_PROCEDURE'             ,0,1,0,1)
      ,('V'     ,'VIEW'                             ,1,0,0,1)
      ,('SCH'   ,'SCHEMA'                           ,0,0,0,1)
      ,('TTYPE' ,'TABLE_TYPE'                       ,1,0,1,1)
      ,('R'     ,'DATABASE_ROLE'                    ,0,0,0,1)
;

-- select major items details
WITH CTE_Data AS(
    -- get tables, functions, procedures and views
    SELECT
         ao.schema_id   AS SchemaId
        ,ao.object_id   AS ObjectId
        ,NULL           AS UserTypeId
        ,s.name         AS SchemaName
        ,ao.name        AS ObjectName
        ,ao.[type]      AS ObjectTypeId
        ,ao.type_desc   AS ObjectTypeDesc
        ,ao.create_date AS ObjectCreateDate
        ,ao.modify_date AS ObjectModDate
        ,x.[value]      AS ExtendedDesc
    FROM sys.all_objects AS ao
        INNER JOIN sys.schemas AS s
            ON ao.schema_id = s.schema_id
        LEFT JOIN (SELECT major_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND minor_id = 0)) AS x
            ON ao.object_id = x.major_id
        INNER JOIN @ObjTypes AS ot
            ON ao.[type] = ot.ObjectTypeId COLLATE Latin1_General_CI_AS
    WHERE(
        (ao.is_ms_shipped = 0) AND (ot.IsMajorType = 1)
    )

    -- add schemas
    UNION ALL

    SELECT
         s.schema_id    AS SchemaId
        ,s.schema_id    AS ObjectId
        ,NULL           AS UserTypeId
        ,s.name         AS SchemaName
        ,s.name         AS ObjectName
        ,'SCH'          AS ObjectTypeId
        ,'SCHEMA'       AS ObjectTypeDesc
        ,NULL           AS ObjectCreateDate
        ,NULL           AS ObjectModDate
        ,x.[value]      AS ExtendedDesc
    FROM sys.schemas AS s
        LEFT JOIN (SELECT major_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND class_desc = 'SCHEMA' AND minor_id = 0)) AS x
            ON s.schema_id = x.major_id

    -- add user table types
    UNION ALL

    SELECT
         s.schema_id                AS SchemaId
        ,t.type_table_object_id     AS ObjectId
        ,t.user_type_id             AS UserTypeId
        ,s.name                     AS SchemaName
        ,t.name                     AS ObjectName
        ,'TTYPE'                    AS ObjectTypeId
        ,'TABLE_TYPE'               AS ObjectTypeDesc
        ,NULL                       AS ObjectCreateDate
        ,NULL                       AS ObjectModDate
        ,x.[value]                  AS ExtendedDesc
    FROM sys.table_types AS t
        INNER JOIN sys.schemas AS s
            ON t.schema_id = s.schema_id
        LEFT JOIN (SELECT major_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND class_desc = 'TYPE' AND minor_id = 0)) AS x
            ON t.user_type_id = x.major_id

    -- add roles
    UNION ALL

    SELECT
         NULL           AS SchemaId
        ,p.principal_id AS ObjectId
        ,NULL           AS UserTypeId
        ,NULL           AS SchemaName
        ,p.name         AS ObjectName
        ,p.[type]       AS ObjectTypeId
        ,p.type_desc    AS ObjectTypeDesc
        ,p.create_date  AS ObjectCreateDate
        ,p.modify_date  AS ObjectModDate
        ,x.[value]      AS ExtendedDesc
    FROM sys.database_principals AS p
        LEFT JOIN (SELECT major_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND class_desc = 'DATABASE_PRINCIPAL' AND minor_id = 0)) AS x
            ON p.principal_id = x.major_id
    WHERE(
        (p.[type] = 'R' /* -- DATABASE_ROLE */)
    )
)

SELECT *
FROM CTE_Data