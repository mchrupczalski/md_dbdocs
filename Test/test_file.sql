/*
<dbdocs>
-----
File: test_file.sql
Created Date: Sunday, 20/06/2021 15:32:08
Author: Mateusz Chrupczalski - MC - ( mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com )
-----
Last Modified: 20/06/2021 08:nn:17
   WeekDay: Sunday
   Date: 20/06/2021 18:00:48
Modified By: Mateusz Chrupczalski - MC - ( <<authoremail1>> )
-----
Type: Function Stored_Procedure Table Trigger
Description: 
Parameters: 
   NA
   NA
Return Value: 
   NA
   NA
-----
Programming Notes: 

Modified By: Mateusz Chrupczalski - MC - ( <<authoremail1>> )
Change_Log:
Date               	| By |	Comments
-------------------	| -- |	--------------------------------------------------------------------------------

-----
</dbdocs>
 */




-- v1

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
        - 01:
            Name: WorkCentreId
            Desc: ID of workcentre
        - 02:
            Name: WorkCentreDesc
            Desc: Workcentre desciption / name

    Parameters: 
        - 01:
            Name: Parameter1
            Desc: Parameter1_Description
    
    FlowDiagram:
        "::: mermaid
        flowchart TD;
        A1((Start)) --> A2[\ProcName\];
        subgraph INFO[Get Tables in Procedure and Last Import Date];
        B1[Select data from sys tables] --- B2[Get data from dat.LogDataImports];
        end;
        A2 --> INFO;
        INFO --> C1[\Return data\];
        C1 --> D1((End));
        :::"

    Return_Value: 
        - Returned_value_description

# Notes:
    Programming_Notes: 
        - Note1

<dbdocs-ignore>
# Change_Log:
Date                | By |	Comments
-------------------	| -- |	---------------------------------------------------------

-----
</dbdocs-ignore>
</dbdocs>
*/


-- v2

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
        - WorkCentreId: ID of workcentre
        - WorkCentreDesc: Workcentre desciption / name

    Parameters: 
        - Parameter1: Parameter1_Description

    Return_Value: 
        - Returned_value_description

# Notes:
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

    Programming_Notes: 
        - Note1

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
