
CREATE TABLE [dbdocs].[ObjectTypes](
     ObjectTypeId           NVARCHAR(5)
    ,ObjectTypeDesc         NVARCHAR(MAX)
    ,HasColumns             BIT
    ,HasParameters          BIT
    ,ExtProp_UseSecondId    BIT                 -- Extended properties for 'User Defined Table Data Type' columns are linked to secondary id
    ,IsMajorType            BIT                 -- Type for objects like Tables, Views, Procedures
)

INSERT INTO [dbdocs].[ObjectTypes]
VALUES ('FN'    ,'SQL_SCALAR_FUNCTION'              ,0,1,0,1)
      ,('IF'    ,'SQL_INLINE_TABLE_VALUED_FUNCTION' ,0,1,0,1)
      ,('TF'    ,'SQL_TABLE_VALUED_FUNCTION'        ,0,1,0,1)
      ,('U'     ,'USER_TABLE'                       ,1,0,0,1)
      ,('P'     ,'SQL_STORED_PROCEDURE'             ,0,1,0,1)
      ,('V'     ,'VIEW'                             ,1,0,0,1)
      ,('uSCH'  ,'SCHEMA'                           ,0,0,0,1)
      ,('uType' ,'TABLE_TYPE'                       ,1,0,1,1)
      ,('R'     ,'DATABASE_ROLE'                    ,0,0,0,1)
      ,('COL'   ,'COLUMN'                           ,0,0,0,0)
      ,('tCOL'  ,'TYPE_COLUMN'                      ,0,0,1,0)
      ,('PAR'   ,'PARAMETER'                        ,0,0,0,0)