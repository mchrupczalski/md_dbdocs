DECLARE @ObjectId INT = <<object_id>>;


SELECT 
     kc.name            AS KeyName
    ,kc.is_system_named AS SysNamed
    ,ic.column_id       AS ColId
    ,c.name             AS ColName
FROM sys.key_constraints AS kc
    INNER JOIN sys.index_columns AS ic
        ON kc.parent_object_id = ic.object_id
    INNER JOIN sys.columns AS c
        ON ic.column_id = c.column_id
        AND ic.object_id = c.object_id
WHERE (kc.parent_object_id = @ObjectId)