create table ITIH.tProject (
	ProjectId int not null identity(0, 1),
	TypeId int not null,
	SemesterId int not null,
	[Name] nvarchar(256) not null,
	Headline nvarchar(256) not null,
	Pitch nvarchar(512) not null,
);

insert into ITIH.tProject (TypeId, SemesterId, [Name], Headline, Pitch)
	values (0, 0, N'', N'', N'');