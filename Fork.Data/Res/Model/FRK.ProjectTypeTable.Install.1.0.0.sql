create table FRK.tProjectType (
	TypeId int not null identity(0, 1),
	[Name] nvarchar(256) not null

	constraint PK_tProjectType_TypeId primary key (TypeId)
);

insert into FRK.tProjectType ([Name]) values (N'');
insert into FRK.tProjectType ([Name]) values ('PFH');
