create table ITIH.tProjectType (
	TypeId int not null identity(0, 1),
	[Name] nvarchar(256) not null

	constraint PK_tProjectType_TypeId primary key (TypeId)
);

insert into ITIH.tProjectType ([Name]) values (N'');
insert into ITIH.tProjectType ([Name]) values ('PFH');
