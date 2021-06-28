/*
 * File: Type.dbDocs_Fields.sql
 * Created Date: Thursday, 24/06/2021 10:23:49
 * Author: Mateusz Chrupczalski - MC - ( mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com )
 * -----
 * Last Modified: Thursday, 24/06/2021 10:25:24
 * Modified By: Mateusz Chrupczalski - MC - ( mateusz.chrupczalski@edwardsvacuum.com; m.chrupczalski@outlook.com )
 * -----
 * Type: Function Stored_Procedure Table Trigger
 * Description: 
 * Parameters: 
 *    NA
 *    NA
 * Return Value: 
 *    NA
 *    NA
 * -----
 * Programming Notes: 
 * 
 * -----
 * HISTORY:
 * Date               	By	Comments
 * -------------------	---	---------------------------------------------------------
 */

DROP TYPE IF EXISTS [dbo].[dbDocs_Fields];
GO

CREATE TYPE [dbo].[dbDocs_Fields] AS TABLE(
	 [FieldID]   SYSNAME        NOT NULL
	,[FieldDesc] NVARCHAR(150)      NULL
);
GO
