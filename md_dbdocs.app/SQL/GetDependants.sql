DECLARE @ObjectId INT = 281768061;

DECLARE @ObjectSchema NVARCHAR(max) = (
    SELECT s.name
    FROM sys.objects AS o
        INNER JOIN sys.schemas AS s
        ON o.schema_id = s.schema_id
    WHERE(o.object_id = @ObjectId)
)

DECLARE @ObjectName NVARCHAR(max) = (
    SELECT o.name
    FROM sys.objects AS o
    WHERE(o.object_id = @ObjectId)
)

DECLARE @ObjectFullName NVARCHAR(max) = CONCAT(@ObjectSchema,'.',@ObjectName)

DECLARE @Dependants TABLE(
     Id         INT IDENTITY(1,1)
    ,refSchema  SYSNAME
    ,refObject  SYSNAME
    ,refId      INT
)

-- https://docs.microsoft.com/en-us/sql/relational-databases/system-dynamic-management-views/sys-dm-sql-referencing-entities-transact-sql?view=sql-server-ver15
-- get objects depending on it
INSERT INTO @Dependants(refSchema,refObject,refId)
SELECT referencing_schema_name, referencing_entity_name, referencing_id 
FROM sys.dm_sql_referencing_entities(@ObjectFullName, 'OBJECT')
UNION ALL
SELECT referencing_schema_name, referencing_entity_name, referencing_id 
FROM sys.dm_sql_referencing_entities(@ObjectFullName, 'TYPE')



-- https://docs.microsoft.com/en-us/sql/relational-databases/system-dynamic-management-views/sys-dm-sql-referenced-entities-transact-sql?view=sql-server-ver15
-- get dependecies info

DROP TABLE IF EXISTS #Report;
SELECT 
    N'' AS Dependant
    ,d.referenced_minor_id
    ,d.referenced_minor_name
    ,d.is_caller_dependent
    ,d.is_ambiguous
    ,d.is_selected
    ,d.is_updated
    ,d.is_select_all
    ,d.is_all_columns_found
    ,d.is_insert_all
INTO #Report
FROM sys.dm_sql_referenced_entities('no_name', 'OBJECT') AS d


DECLARE @MaxLoop INT = (SELECT COUNT(*) FROM @Dependants)
DECLARE @Loop INT = 1

WHILE(@Loop <= @MaxLoop) BEGIN

    DECLARE @DepName NVARCHAR(max) = (SELECT CONCAT(refSchema,'.',refObject) FROM @Dependants WHERE Id = @Loop)
    
    INSERT INTO #Report
    SELECT
        @DepName
        ,d.referenced_minor_id
        ,d.referenced_minor_name
        ,d.is_caller_dependent
        ,d.is_ambiguous
        ,d.is_selected
        ,d.is_updated
        ,d.is_select_all
        ,d.is_all_columns_found
        ,d.is_insert_all
    FROM sys.dm_sql_referenced_entities(@DepName, 'OBJECT') AS d
    WHERE(
        (d.referenced_schema_name = @ObjectSchema)
        AND
        (d.referenced_entity_name = @ObjectName)
    )

    SET @Loop = @Loop + 1

END

SELECT * FROM #Report