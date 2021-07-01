CREATE PROCEDURE [dbdocs].[spAddFileInfo](
     @FileName           NVARCHAR(MAX)
    ,@FilePath           NVARCHAR(MAX)
    ,@ObjectType         NVARCHAR(MAX) = NULL
    ,@ObjectSchema       NVARCHAR(MAX) = NULL
    ,@ObjectName         NVARCHAR(MAX) = NULL
    ,@HasTagDbDocs       BIT
    ,@HasTagDiagram      BIT
    ,@HasTagChangeLog    BIT
) AS
BEGIN

    INSERT INTO [dbdocs].[FilesInfo](
         [FileName]
        ,FilePath
        ,ObjectType
        ,ObjectSchema
        ,ObjectName
        ,HasTagDbDocs
        ,HasTagDiagram
        ,HasTagChangeLog
    ) 
    VALUES(
         @FileName       
        ,@FilePath       
        ,@ObjectType     
        ,@ObjectSchema   
        ,@ObjectName
        ,@HasTagDbDocs   
        ,@HasTagDiagram  
        ,@HasTagChangeLog
    )

END