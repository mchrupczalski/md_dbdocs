
WITH CTE_Data AS(
    -- get tables, functions, procedures and views
    SELECT
         ao.schema_id   AS SchemaId
        ,ao.object_id   AS ObjectId
        ,NULL           AS ObjectId_Secondary
        ,s.name         AS SchemaName
        ,ao.name        AS ObjectName
        ,ao.[type]      AS ObjectType
        ,ao.type_desc   AS ObjectTypeDesc
        ,ao.create_date AS ObjectCreateDate
        ,ao.modify_date AS ObjectModDate
        ,x.[value]      AS OldExtProperty
    FROM sys.all_objects AS ao
        INNER JOIN sys.schemas AS s
            ON ao.schema_id = s.schema_id
        LEFT JOIN (SELECT major_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND minor_id = 0)) AS x
            ON ao.object_id = x.major_id
        INNER JOIN dbdocs.ObjectTypes AS ot
            ON ao.[type] = ot.ObjectTypeId COLLATE Latin1_General_CI_AS
    WHERE(
        (ao.is_ms_shipped = 0) AND (ot.IsMajorType = 1)
    )

    -- add schemas
    UNION ALL

    SELECT
         s.schema_id    AS SchemaId
        ,s.schema_id    AS ObjectId
        ,NULL           AS ObjectId_Secondary
        ,s.name         AS SchemaName
        ,s.name         AS ObjectName
        ,'SCH'          AS ObjectType
        ,'SCHEMA'       AS ObjectTypeDesc
        ,NULL           AS ObjectCreateDate
        ,NULL           AS ObjectModDate
        ,x.[value]      AS OldExtProperty
    FROM sys.schemas AS s
        LEFT JOIN (SELECT major_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND class_desc = 'SCHEMA' AND minor_id = 0)) AS x
            ON s.schema_id = x.major_id

    -- add user table types
    UNION ALL

    SELECT
         s.schema_id                AS SchemaId
        ,t.type_table_object_id     AS ObjectId
        ,t.user_type_id             AS ObjectId_Secondary
        ,s.name                     AS SchemaName
        ,t.name                     AS ObjectName
        ,'TTYPE'                    AS ObjectType
        ,'TABLE_TYPE'               AS ObjectTypeDesc
        ,NULL                       AS ObjectCreateDate
        ,NULL                       AS ObjectModDate
        ,x.[value]                  AS OldExtProperty
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
        ,NULL           AS ObjectId_Secondary
        ,NULL           AS SchemaName
        ,p.name         AS ObjectName
        ,p.[type]       AS ObjectType
        ,p.type_desc    AS ObjectTypeDesc
        ,p.create_date  AS ObjectCreateDate
        ,p.modify_date  AS ObjectModDate
        ,x.[value]                  AS OldExtProperty
    FROM sys.database_principals AS p
        LEFT JOIN (SELECT major_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND class_desc = 'DATABASE_PRINCIPAL' AND minor_id = 0)) AS x
            ON p.principal_id = x.major_id
    WHERE(
        (p.[type] = 'R' /* -- DATABASE_ROLE */)
    )
)

SELECT d.* INTO [dbdocs].[MajorObjectsInfo] 
FROM CTE_Data AS d
WHERE d.SchemaName NOT LIKE 'dbdocs'