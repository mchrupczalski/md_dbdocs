/*
<dbdocs>
# File Details:
   File_Name: new head.sql
   Created:
       WeekDay: Friday
       Date: 02/07/2021 16:08:22
   Author:
       Name: Mateusz Chrupczalski
       Initials: MC
       Email: mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com
# -----
   Last_Modified:
       WeekDay: Friday
       Date: 02/07/2021 16:33:29
   Modified_By:
       Name: Mateusz Chrupczalski
       Initials: MC
       Email: mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com

# Object Details:
   Module: 

   Schema_Name: 
   Object_Name: 
   Description: 

   Fields:
       - Field1: Field1_Description
       - Field2: Field2_Description

   Parameters:
       - Param1: Param1_Description
       - Param2: Param2_Description

   Return_Value: Returned_Value_Description

# Notes:
   Programming Notes: 
       - Note1
       - Note2

   <diagram>
   </diagram>

# Change_Log:
<change_Log>
HISTORY:
Date             | By  | Comments
---------------- | --- | ---------------------------------------------------------
02/07/2021 16:33 | MC | 
-----
</change_Log>
</dbdocs>
*/













/*
SELECT o.[type], o.type_desc
FROM sys.objects AS o
GROUP BY o.[type], o.type_desc

TT	- TYPE_TABLE
FN	- SQL_SCALAR_FUNCTION
IF	- SQL_INLINE_TABLE_VALUED_FUNCTION
SQ	- SERVICE_QUEUE
F 	- FOREIGN_KEY_CONSTRAINT
U 	- USER_TABLE
D 	- DEFAULT_CONSTRAINT
PK	- PRIMARY_KEY_CONSTRAINT
V 	- VIEW
S 	- SYSTEM_TABLE
IT	- INTERNAL_TABLE
P 	- SQL_STORED_PROCEDURE
*/

SELECT * FROM sys.dm_sql_referenced_entities ('lib.spAgentCall', 'object')

SELECT * FROM sys.dm_sql_referencing_entities ('lib.spAgentCall', 'object')