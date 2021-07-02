/*
<dbdocs>
# File Details:
    File_Name: test_file.sql
    Created: 
        WeekDay: Sunday
        Date: 20/06/2021 15:32:08
    Author:
        Name: Mateusz Chrupczalski
        Initials: MC
        Email: mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com
# -----
    Last_Modified:
        WeekDay: Sunday
        Date: 20/06/2021 16:02:38
    Modified_By:
        Name: Mateusz Chrupczalski
        Initials: MC
        Email: mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com

# Object Details:
    Module: Core
    
    Schema_Name: lu
    Object_Name: WorkCentres
    Description: This is table description

    Fields:
        WorkCentreId: ID of workcentre
        WorkCentreDesc: Workcentre desciption / name

    Parameters: 
        Parameter1: Parameter1_Description

    Return_Value: 
        - Returned_value_description

# Notes:
    Programming_Notes: 
        - Note1
        - Note2

    <diagram>    
        ::: mermaid
        flowchart TD;
        A1((Start)) --> A2[\ProcName\];
        subgraph INFO[Get Tables in Procedure and Last Import Date];
        B1[Select data from sys tables] --- B2[Get data from dat.LogDataImports];
        end;
        A2 --> INFO;
        INFO --> C1[\Return data\];
        C1 --> D1((End));
        :::
    </diagram>

<change_log>
# Change_Log:
Date                | By |	Comments
-------------------	| -- |	---------------------------------------------------------

-----
</change_log>
</dbdocs>
*/


CREATE TABLE [lu].[WorkCentres]
(
    [WorkCentreId]    NVARCHAR(6)   NOT NULL 
  , [WorkCentreDesc]  NVARCHAR(50)  NOT NULL

  , CONSTRAINT PK_WorkCentres_WorkCentreId PRIMARY KEY([WorkCentreId])
)
