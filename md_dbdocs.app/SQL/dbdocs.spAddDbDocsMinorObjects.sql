CREATE PROCEDURE [dbdocs].[spAddDbDocsMinorObjects]
(
     @MajorObjType   NVARCHAR(MAX)
    ,@MajorObjSchema NVARCHAR(MAX)
    ,@MajorObjName   NVARCHAR(MAX)
    ,@ChildObjTypeId NVARCHAR(MAX)
    ,@ChildObjName   NVARCHAR(MAX)
    ,@ChildObjDesc   NVARCHAR(MAX)
)
AS
BEGIN

    INSERT INTO [dbdocs].[DbDocsMinorObjects]
    (
         MajorObjType
        ,MajorObjSchema
        ,MajorObjName
        ,ChildObjTypeId
        ,ChildObjName
        ,ChildObjDesc
    )
    VALUES
    (
         @MajorObjType
        ,@MajorObjSchema
        ,@MajorObjName
        ,@ChildObjTypeId
        ,@ChildObjName
        ,@ChildObjDesc
    )

END