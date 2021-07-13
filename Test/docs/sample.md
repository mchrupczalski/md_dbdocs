Test:  
Home:  
Table: 1360723900 - dat.UserDataSlots  
Type: 2080726465 - app.UDTT_PartToParent  
Procedure: 48719226 - lib.spMergeStgToSap_Relay  
Function: 1970106059 - pop.fn_DateToYW  
View: 1650104919 - esch.vw_ENGSchedule  

## 3.1.1.1 - UserDataSlots.Tables - dat.UserDataSlots
<br />

**Module:** UserDataSlots  
**Schema:** dat  
**Name:** UserDataSlots  
**Type:** USER_TABLE  
**Description:** This is table to store data

<br />

### Columns:

ID | Key | ColumnName | DataType | MaxLen | Precision | Scale | IsNullable | IsIdentity | IsComputed | DefaultVal | ExtendedDesc |
--- | :---: | --- | --- | :---: | :---: | :---: | :---: | :---: | :---: | :---: | --- |
4|![fk](.\assets\fk.svg)|BomApplicationId|nvarchar|8|0|0|![false](.\assets\xx.png)|![false](.\assets\xx.png)|![false](.\assets\xx.png)||asDASDSAd
6||ExcelFileModDate|datetime2|8|27|7|![false](.\assets\xx.png)|![false](.\assets\xx.png)|![false](.\assets\xx.png)||asdqweqwe
5||ExcelFileName|nvarchar|256|0|0|![false](.\assets\xx.png)|![false](.\assets\xx.png)|![false](.\assets\xx.png)||adfsertert
7||IsActive|bit|1|1|0|![false](.\assets\xx.png)|![false](.\assets\xx.png)|![false](.\assets\xx.png)|((1))|t345gergdfg
3|![fk](.\assets\fk.svg)|TableId|nvarchar|100|0|0|![false](.\assets\xx.png)|![false](.\assets\xx.png)|![false](.\assets\xx.png)||dfgrturyrt
1|![pk](.\assets\pk.svg)|UDSId|int|4|10|0|![false](.\assets\xx.png)|![true](.\assets\ok.png)|![false](.\assets\xx.png)||dfgbcv
2|![fk](.\assets\fk.svg)|UDSInfoId|int|4|10|0|![false](.\assets\xx.png)|![false](.\assets\xx.png)|![false](.\assets\xx.png)||3543gfdgdfb

<br />

### Unique Keys:

ColId	|	ColName	|	SysNamed	|	KeyName
:---:	|	:---:	|	:---:	|	---
1	|	UDSId	|		|	PK_UserDataSlots_UDSIs

<br />

### Relations:

PrimarySchema	|	PrimaryTable	|	PrimaryColumn	|	ForeignSchema	|	ForeignTable	|	ForeignColumn	|	SysNamed	|	ConstraintName
:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	---
dat	|	UserDataSlots	|	BomApplicationId	|	lu	|	BomApplicationTypes	|	BomApplicationId	|	![err](.\assets\error.png)	|	FK_BomApplicationTypes_BomApplicationId_UserDataSlots_BomApplicationId
dat	|	UserDataSlots	|	TableId	|	lu	|	DataSlotsTables	|	TableId	|		|	FK_DataSlotsTables_TableId_UserDataSlots_TableId
dat	|	UserDataSlots	|	UDSInfoId	|	dat	|	UserDataSlotsInfo	|	UDSInfoId	|		|	FK_UserDataSlotsInfo_UDSInfoId_UserDataSlots_UDSInfoId
app	|	zcs11rep	|	UDSId	|	dat	|	UserDataSlots	|	UDSId	|		|	FK_zcs11rep_UDSId_UserDataSlots_UDSId
app	|	zmdoc03	|	UDSId	|	dat	|	UserDataSlots	|	UDSId	|		|	FK_zmdoc03_UDSId_UserDataSlots_UDSId

<br />

### Dependents:
>### app.spUserDataSlot_Create
><br />  
>
>ColumnId	|	ColumnName	|	is_caller_dependent	|	is_ambiguous	|	is_selected	|	is_updated	|	is_select_all	|	is_all_columns_found	|	is_insert_all
>:---:	|	---	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:
>2	|	UDSInfoId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>3	|	TableId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>4	|	BomApplicationId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>5	|	ExcelFileName	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>6	|	ExcelFileModDate	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
---
<br />

>### app.spUserDataSlot_Delete
><br />  
>
>ColumnId	|	ColumnName	|	is_caller_dependent	|	is_ambiguous	|	is_selected	|	is_updated	|	is_select_all	|	is_all_columns_found	|	is_insert_all
>:---:	|	---	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:
>1	|	UDSId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>3	|	TableId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>7	|	IsActive	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
---
<br />

>### app.spUserDataSlot_GetEditInfo
><br />  
>
>ColumnId	|	ColumnName	|	is_caller_dependent	|	is_ambiguous	|	is_selected	|	is_updated	|	is_select_all	|	is_all_columns_found	|	is_insert_all
>:---:	|	---	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:
>1	|	UDSId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>2	|	UDSInfoId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>3	|	TableId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>4	|	BomApplicationId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>5	|	ExcelFileName	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>6	|	ExcelFileModDate	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
---
<br />

>### app.spUserDataSlot_GetInfo
><br />  
>
>ColumnId	|	ColumnName	|	is_caller_dependent	|	is_ambiguous	|	is_selected	|	is_updated	|	is_select_all	|	is_all_columns_found	|	is_insert_all
>:---:	|	---	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:
>2	|	UDSInfoId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>3	|	TableId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>4	|	BomApplicationId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>7	|	IsActive	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
---
<br />

>### app.spUserDataSlotInfo_Delete
><br />  
>
>ColumnId	|	ColumnName	|	is_caller_dependent	|	is_ambiguous	|	is_selected	|	is_updated	|	is_select_all	|	is_all_columns_found	|	is_insert_all
>:---:	|	---	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:	|	:---:
>1	|	UDSId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>2	|	UDSInfoId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>3	|	TableId	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
>7	|	IsActive	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)	|	![false](.\assets\xx.png)	|	![true](.\assets\ok.png)	|	![false](.\assets\xx.png)
---
<br />

### Change Log:
 Date               	| By	| Comments
 -------------------	| :---: | ---------------------------------------------------------
 11/06/2021 23:24:32	| MC	| Yet another correction to the query, previous was returning false results
 05/06/2021 02:41:47	| MC	| Added Select and Join of Validity dates
 01/06/2021 22:06:54	| MC	| Added Description and Usage count to results
 01/06/2021 19:24:32    | MC	| Added xPlant Status
 31/05/2021 23:21:42	| MC	| Fix #123 - "No parents found" + Replaced dynamic SQL