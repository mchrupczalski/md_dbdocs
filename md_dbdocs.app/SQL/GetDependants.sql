DECLARE @ObjectId INT = <<object_id>>;

-- use object type to determine if final result should include NULL values in referenced_minor_name (0 in referenced_minor_id)
DECLARE @ObjectType NVARCHAR(max) = (SELECT RTRIM([type]) FROM sys.objects WHERE object_id = @ObjectId)

-- user table data type schema & name in sys.objects is incorrect
DECLARE @ObjectSchema NVARCHAR(max) = (
    CASE WHEN @ObjectType = 'TT' THEN (SELECT s.name FROM sys.table_types AS t INNER JOIN sys.schemas AS s ON t.schema_id = s.schema_id WHERE(t.type_table_object_id = @ObjectId))
        ELSE (SELECT s.name FROM sys.objects AS o INNER JOIN sys.schemas AS s ON o.schema_id = s.schema_id WHERE(o.object_id = @ObjectId))
    END
)

DECLARE @ObjectName NVARCHAR(max) = (
    CASE WHEN @ObjectType = 'TT' THEN (SELECT t.name FROM sys.table_types AS t WHERE(t.type_table_object_id = @ObjectId))
        ELSE (SELECT o.name FROM sys.objects AS o WHERE(o.object_id = @ObjectId))
    END
)

DECLARE @ObjectFullName NVARCHAR(max) = CONCAT(@ObjectSchema,'.',@ObjectName)


SELECT
    d.referenced_entity_name
    ,d.referenced_minor_id
    ,d.referenced_minor_name
    ,d.is_caller_dependent
    ,d.is_ambiguous
    ,d.is_selected
    ,d.is_updated
    ,d.is_select_all
    ,d.is_all_columns_found
    ,d.is_insert_all
FROM sys.dm_sql_referenced_entities(@ObjectFullName, 'OBJECT') AS d