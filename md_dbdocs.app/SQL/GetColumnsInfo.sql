-- Replace <<TableId>> with object Id
DECLARE @ObjectId INT = <<TableId>>;

-- Get columns info
WITH CTE_Columns AS 
(
    SELECT
         c.object_id                            AS ObjectId
        ,c.column_id                            AS ColumnId
        ,c.name                                 AS ColumnName
        ,t.name                                 AS DataType
        ,c.max_length                           AS MaxLen
        ,c.[precision]                          AS [Precision]
        ,c.scale                                AS Scale
        ,c.is_nullable                          AS IsNullable
        ,c.is_identity                          AS IsIdentity
        ,c.is_computed                          AS IsComputed
        ,CAST(d.definition AS NVARCHAR)         AS DefaultVal
    FROM sys.columns AS c
        INNER JOIN sys.types AS t
            ON c.system_type_id = t.system_type_id AND c.user_type_id = t.user_type_id
        LEFT JOIN sys.default_constraints AS d
            ON c.object_id = d.parent_object_id AND c.column_id = d.parent_column_id
    WHERE(
        (c.object_id = @ObjectId)
    )
),
-- Get Extended Description
CTE_XtDesc AS 
(
    SELECT
         minor_id   AS ColId
        ,[value]    AS ExtendedDesc
    FROM sys.extended_properties 
    WHERE(
        name = 'MS_Description' 
        AND class_desc = 'OBJECT_OR_COLUMN' 
        AND minor_id > 0
        AND major_id = @ObjectId
    )
),
-- Get Primary Key
CTE_PK AS
(
    SELECT 
         c.ColumnId
        ,CAST(CASE WHEN kc.[type] IS NULL THEN 0
              ELSE 1
         END AS BIT) AS IsPK
    FROM CTE_Columns AS c
        LEFT JOIN sys.index_columns AS ic
            ON c.ObjectId = ic.object_id
            AND c.ColumnId = ic.column_id
        LEFT JOIN sys.key_constraints AS kc
            ON ic.object_id = kc.parent_object_id
            AND ic.index_id = kc.unique_index_id
),
CTE_FK AS 
(
    SELECT
         c.ColumnId
        ,CAST(CASE WHEN fk.[type] IS NULL THEN 0
              ELSE 1
         END AS BIT) AS IsFK
        ,fc.parent_object_id
        ,fc.referenced_object_id
    FROM CTE_Columns AS c
        LEFT JOIN sys.foreign_key_columns AS fc
            --ON fk.object_id = fc.constraint_object_id
            ON c.ObjectId = fc.parent_object_id
            AND c.ColumnId = fc.parent_column_id
        LEFT JOIN sys.foreign_keys AS fk
            ON fc.constraint_object_id = fk.object_id
)

SELECT 
     c.ColumnId
    ,c.ColumnName
    ,c.DataType
    ,c.MaxLen
    ,c.[Precision]
    ,c.Scale
    ,c.IsNullable
    ,c.IsIdentity
    ,c.IsComputed
    ,c.DefaultVal
    ,x.ExtendedDesc
    ,p.IsPK
    ,f.IsFK
FROM CTE_Columns AS c
    LEFT JOIN CTE_XtDesc AS x
        ON c.ColumnId = x.ColId
    LEFT JOIN CTE_PK AS p
        ON c.ColumnId = p.ColumnId
    LEFT JOIN CTE_FK AS f
        ON c.ColumnId = f.ColumnId