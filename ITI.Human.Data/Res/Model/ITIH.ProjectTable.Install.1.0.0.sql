﻿create table ITIH.tProject (
	ProjectId int not null identity(0, 1),
	TypeId int not null,
	SemesterId int not null,
	[Name] nvarchar(256) not null,
	Headline nvarchar(256) not null,
	Pitch nvarchar(512) not null,

	constraint PK_tProject_ProjectId primary key (ProjectId),
	constraint FK_tProject_TypeId foreign key (TypeId) references ITIH.tProjectType (TypeId),
	constraint FK_tProject_SemesterId foreign key (SemesterId) references ITIH.tSemester (SemesterId)
);

insert into ITIH.tProject (TypeId, SemesterId, [Name], Headline, Pitch)
	values (0, 0, N'', N'', N'');