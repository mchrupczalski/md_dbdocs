CREATE TABLE [dbdocs].[FilesInfo](
     FileId             INT                 NOT NULL    IDENTITY(1,1)
    ,[FileName]         NVARCHAR(MAX)       NOT NULL
    ,FilePath           NVARCHAR(MAX)       NOT NULL
    ,ObjectType         NVARCHAR(MAX)           NULL
    ,ObjectSchema       NVARCHAR(MAX)           NULL 
    ,ObjectName         NVARCHAR(MAX)           NULL
    ,HasTagDbDocs       BIT                 NOT NULL
    ,HasTagDiagram      BIT                 NOT NULL
    ,HasTagChangeLog    BIT                 NOT NULL

    ,CONSTRAINT PK_FilesInfo_FileName PRIMARY KEY([FileId])
)