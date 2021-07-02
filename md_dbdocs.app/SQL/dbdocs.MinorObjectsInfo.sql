
WITH CTE_Data AS (

    -- get columns info
    SELECT
         c.object_id                    AS MajorObjectId
        ,m.ObjectType                   AS MajorObjectType
        ,m.ObjectTypeDesc               AS MajorObjectTypeDesc
        ,m.SchemaName                   AS MajorObjectSchemaName
        ,m.ObjectName                   AS MajorObjectName
        ,c.column_id                    AS MinorObjectId
        ,c.name                         AS MinorObjectName
        ,'COL'                          AS MinorObjectTypeId
        ,'COLUMN'                       AS MinorObjectTypeDesc
        ,c.system_type_id               AS DataTypeId
        ,t.name                         AS DataTypeName
        ,c.max_length                   AS MaxLen
        ,c.[precision]                  AS [Precision]
        ,c.scale                        AS Scale
        ,c.is_nullable                  AS IsNullable
        ,c.is_identity                  AS IsIdentity
        ,c.is_computed                  AS IsComputed
        ,NULL                           AS IsOutput
        ,NULL                           AS IsReadOnly
        ,CAST(d.definition AS NVARCHAR) AS DefaultVal
        ,x.[value]                      AS OldExtProperty
    FROM sys.columns AS c
        INNER JOIN dbdocs.MajorObjectsInfo AS m
            ON c.object_id = m.ObjectId
        INNER JOIN dbdocs.ObjectTypes AS o
            ON m.ObjectType = o.ObjectTypeId COLLATE Latin1_General_CI_AS
        INNER JOIN sys.types AS t
            ON c.system_type_id = t.system_type_id AND c.user_type_id = t.user_type_id
        LEFT JOIN sys.default_constraints AS d
            ON c.object_id = d.parent_object_id AND c.column_id = d.parent_column_id
        LEFT JOIN (SELECT major_id, minor_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND class_desc = 'OBJECT_OR_COLUMN' AND minor_id > 0)) AS x
            ON c.object_id = x.major_id AND c.column_id = x.minor_id
    WHERE(
        (o.HasColumns = 1) AND (o.ExtProp_UseSecondId = 0)
    )

    -- get columns info for user table types
    UNION ALL
    SELECT
         c.object_id                    AS MajorObjectId
        ,m.ObjectType                   AS MajorObjectType
        ,m.ObjectTypeDesc               AS MajorObjectTypeDesc
        ,m.SchemaName                   AS MajorObjectSchemaName
        ,m.ObjectName                   AS MajorObjectName
        ,c.column_id                    AS MinorObjectId
        ,c.name                         AS MinorObjectName
        ,'TCOL'                         AS MinorObjectTypeId
        ,'TYPE_COLUMN'                  AS MinorObjectTypeDesc
        ,c.system_type_id               AS DataTypeId
        ,t.name                         AS DataTypeName
        ,c.max_length                   AS MaxLen
        ,c.[precision]                  AS [Precision]
        ,c.scale                        AS Scale
        ,c.is_nullable                  AS IsNullable
        ,c.is_identity                  AS IsIdentity
        ,c.is_computed                  AS IsComputed
        ,NULL                           AS IsOutput
        ,NULL                           AS IsReadOnly
        ,CAST(d.definition AS NVARCHAR) AS DefaultVal
        ,x.[value]                      AS OldExtProperty
    FROM sys.columns AS c
        INNER JOIN dbdocs.MajorObjectsInfo AS m
            ON c.object_id = m.ObjectId
        INNER JOIN dbdocs.ObjectTypes AS o
            ON m.ObjectType = o.ObjectTypeId COLLATE Latin1_General_CI_AS
        INNER JOIN sys.types AS t
            ON c.system_type_id = t.system_type_id AND c.user_type_id = t.user_type_id
        LEFT JOIN sys.default_constraints AS d
            ON c.object_id = d.parent_object_id AND c.column_id = d.parent_column_id
        LEFT JOIN (SELECT major_id, minor_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND class_desc = 'TYPE_COLUMN' AND minor_id > 0)) AS x
            ON m.ObjectId_Secondary = x.major_id AND c.column_id = x.minor_id
    WHERE(
        (o.HasColumns = 1) AND (o.ExtProp_UseSecondId = 1)
    )

    -- get parameters info
    UNION ALL
    SELECT
         c.object_id            AS MajorObjectId
        ,m.ObjectType           AS MajorObjectType
        ,m.ObjectTypeDesc       AS MajorObjectTypeDesc
        ,m.SchemaName           AS MajorObjectSchemaName
        ,m.ObjectName           AS MajorObjectName
        ,c.parameter_id         AS MinorObjectId
        ,c.name                 AS MinorObjectName
        ,'PAR'                  AS MinorObjectTypeId
        ,'PARAMETER'            AS MinorObjectTypeDesc
        ,c.system_type_id       AS DataTypeId
        ,t.name                 AS DataTypeName
        ,c.max_length           AS MaxLen
        ,c.[precision]          AS [Precision]
        ,c.scale                AS Scale
        ,c.is_nullable          AS IsNullable
        ,NULL                   AS IsIdentity
        ,NULL                   AS IsComputed
        ,c.is_output            AS IsOutput
        ,c.is_readonly          AS IsReadOnly
        ,c.default_value        AS DefaultVal
        ,x.[value]              AS OldExtProperty
    FROM sys.parameters AS c
        INNER JOIN dbdocs.MajorObjectsInfo AS m
            ON c.object_id = m.ObjectId
        INNER JOIN dbdocs.ObjectTypes AS o
            ON m.ObjectType = o.ObjectTypeId COLLATE Latin1_General_CI_AS
        INNER JOIN sys.types AS t
            ON c.system_type_id = t.system_type_id AND c.user_type_id = t.user_type_id
        LEFT JOIN (SELECT major_id, minor_id, [value] FROM sys.extended_properties WHERE (name = 'MS_Description' AND class_desc = 'PARAMETER' AND minor_id > 0)) AS x
            ON c.object_id = x.major_id AND c.parameter_id = x.minor_id
    WHERE(
        (o.HasParameters = 1)
    )
)

SELECT d.* INTO [dbdocs].[MinorObjectsInfo]
FROM CTE_Data AS d
WHERE d.MajorObjectSchemaName NOT LIKE 'dbdocs'