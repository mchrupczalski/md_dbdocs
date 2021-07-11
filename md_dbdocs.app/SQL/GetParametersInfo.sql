-- Replace <<ProcedureId>> with object Id
DECLARE @ObjectId INT = <<ProcedureId>>;

SELECT
     c.parameter_id         AS ParameterId
    ,REPLACE(c.name,'@','') AS ParameterName
    ,t.name                 AS DataType
    ,c.max_length           AS MaxLen
    ,c.[precision]          AS [Precision]
    ,c.scale                AS Scale
    ,c.is_nullable          AS IsNullable
    ,c.is_output            AS IsOutput
    ,c.is_readonly          AS IsReadOnly
    ,c.default_value        AS DefaultVal
    ,x.[value]              AS ExtendedDesc
FROM sys.parameters AS c
    INNER JOIN sys.types AS t
        ON c.system_type_id = t.system_type_id 
        AND c.user_type_id = t.user_type_id
    LEFT JOIN (SELECT major_id, minor_id, [value] 
               FROM sys.extended_properties 
               WHERE (name = 'MS_Description' AND class_desc = 'PARAMETER')) AS x
        ON c.object_id = x.major_id 
        AND c.parameter_id = x.minor_id
WHERE(
    (c.object_id = @ObjectId)
)