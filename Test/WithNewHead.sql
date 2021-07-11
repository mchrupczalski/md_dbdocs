SELECT * FROM sys.columns WHERE object_id = 1360723900

SELECT * FROM sys.foreign_keys WHERE parent_object_id = 1360723900

SELECT * FROM sys.foreign_key_columns WHERE parent_object_id = 1360723900


SELECT
    c.column_id
    ,c.name
    ,fk.[type]
    ,fk.name
FROM sys.columns AS c
    LEFT JOIN sys.foreign_key_columns AS fc
        ON c.object_id = fc.parent_object_id
        AND c.column_id = fc.parent_column_id
    LEFT JOIN sys.foreign_keys AS fk
        ON fc.constraint_object_id = fk.object_id
WHERE c.object_id = 1360723900