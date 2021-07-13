## 3.1.1.1 - UserDataSlots.Tables - dat.UserDataSlots
<br />

**Module:** UserDataSlots  
**Schema:** dat  
**Table:** UserDataSlots  
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

### Dependencies:

