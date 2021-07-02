CREATE PROCEDURE [dbdocs].[spAddDbDocsMajorObjects](
     @FileName           NVARCHAR(MAX)
    ,@ObjectType         NVARCHAR(MAX) = NULL
    ,@ObjectSchema       NVARCHAR(MAX) = NULL
    ,@ObjectName         NVARCHAR(MAX) = NULL
    ,@ObjectDesc         NVARCHAR(MAX) = NULL
    ,@ObjectModule       NVARCHAR(MAX) = NULL
) AS
BEGIN

    INSERT INTO [dbdocs].[DbDocsMajorObjects](
         [FileName]
        ,ObjectType
        ,ObjectSchema
        ,ObjectName
        ,ObjectDesc
        ,ObjectModule
    ) 
    VALUES( 
         @FileName
        ,@ObjectType
        ,@ObjectSchema   
        ,@ObjectName
        ,@ObjectDesc
        ,@ObjectModule
    )

END