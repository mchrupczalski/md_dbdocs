CREATE TABLE [dbdocs].[DbDocsMinorObjects](
     Id             INT             NOT NULL        IDENTITY(1,1)
    ,MajorObjType   NVARCHAR(MAX)   NOT NULL
    ,MajorObjSchema NVARCHAR(MAX)   NOT NULL
    ,MajorObjName   NVARCHAR(MAX)   NOT NULL
    ,ChildObjTypeId NVARCHAR(MAX)   NOT NULL
    ,ChildObjName   NVARCHAR(MAX)   NOT NULL
    ,ChildObjDesc   NVARCHAR(MAX)   NOT NULL

    ,CONSTRAINT PK_DbDocsMinorObjects_Id PRIMARY KEY([Id])
)