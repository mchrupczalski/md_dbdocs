DECLARE @ObjectId INT = <<object_id>>;

SELECT
     sp.name            AS PrimarySchema
    ,op.name            AS PrimaryTable
    ,cp.name            AS PrimaryColumn
    ,sc.name            AS ForeignSchema
    ,oc.name            AS ForeignTable
    ,cc.name            AS ForeignColumn
    ,fk.is_system_named AS SysNamed
    ,fk.name            AS ConstraintName
FROM sys.foreign_key_columns AS fkc
    INNER JOIN sys.foreign_keys AS fk
        ON fkc.constraint_object_id = fk.object_id
    INNER JOIN sys.columns AS cp
        ON fkc.parent_object_id = cp.object_id
        AND fkc.parent_column_id = cp.column_id
    INNER JOIN sys.columns AS cc
        ON fkc.referenced_object_id = cc.object_id
        AND fkc.referenced_column_id = cc.column_id
    INNER JOIN sys.objects AS op
        ON fkc.parent_object_id = op.object_id
    INNER JOIN sys.objects AS oc
        ON fkc.referenced_object_id = oc.object_id
    INNER JOIN sys.schemas AS sp
        ON op.schema_id = sp.schema_id
    INNER JOIN sys.schemas AS sc
        ON oc.schema_id = sc.schema_id
WHERE(
    (fkc.parent_object_id = @ObjectId)
    OR
    (fkc.referenced_object_id = @ObjectId)
)
ORDER BY
     op.name
    ,cp.name
    ,oc.name
    ,cc.name