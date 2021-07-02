CREATE TABLE [dbdocs].[DbDocsMajorObjects](
     FileId             INT                 NOT NULL    IDENTITY(1,1)
    ,[FileName]         NVARCHAR(MAX)       NOT NULL
    ,ObjectType         NVARCHAR(MAX)           NULL
    ,ObjectSchema       NVARCHAR(MAX)           NULL 
    ,ObjectName         NVARCHAR(MAX)           NULL
    ,ObjectDesc         NVARCHAR(MAX)           NULL
    ,ObjectModule       NVARCHAR(MAX)           NULL 

    ,CONSTRAINT PK_DbDocsMajorObjects_FileName PRIMARY KEY([FileId])
)