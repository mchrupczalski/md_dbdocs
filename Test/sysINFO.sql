/*
<dbdocs>
-----
File: sysINFO.sql
Created Date: Sunday, 27/06/2021 17:25:50
Author: Mateusz Chrupczalski - MC - ( mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com )
-----
Last Modified: 27/06/2021 06:nn:14
   WeekDay: Sunday
   Date: 27/06/2021 17:25:50
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

-----
Change_Log:
Date               	| By |	Comments
-------------------	| -- |	--------------------------------------------------------------------------------

-----
</dbdocs>
 */


SELECT * FROM sys.tables

SELECT * FROM sys.schemas

SELECT * FROM sys.views

SELECT * FROM sys.columns

SELECT * FROM sys.extended_properties

SELECT * FROM sys.types

SELECT * FROM sys.table_types

SELECT * FROM sys.all_columns 

SELECT * FROM sys.database_principals

SELECT * FROM sys.parameters

SELECT * FROM sys.default_constraints

SELECT * FROM sys.key_constraints

SELECT * FROM sys.all_objects
SELECT DISTINCT [type], type_desc FROM sys.all_objects
SELECT * FROM sys.all_objects WHERE [type] IN ('PK','F')

SELECT * FROM sys.index_columns

SELECT * FROM sys.foreign_key_columns

/*
EXEC sys.sp_addextendedproperty 
@name = N'MS_Description', 
@value = N'Input parameter. Enter a valid ProductID.', 
@level0type = N'SCHEMA', @level0name = [pop],
@level1type = N'FUNCTION', @level1name = [fn_tbl_Report_CellOrders_Stats],
@level2type = N'PARAMETER', @level2name ='@WorkCentreID';
GO
*/

/*
SELECT
    kc.name,
    c.NAME
FROM 
    sys.key_constraints kc
INNER JOIN 
    sys.index_columns ic ON kc.parent_object_id = ic.object_id  and kc.unique_index_id = ic.index_id
INNER JOIN 
    sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE
    kc.type = 'F'
*/