﻿create table ITIH.tProjectType (
	TypeId int not null identity(0, 1),
	[Name] nvarchar(256) not null
);

insert into ITIH.tProjectType ([Name]) values (N'');
insert into ITIH.tProjectType ([Name]) values ('HFP');
insert into ITIH.tProjectType ([Name]) values ('ITP');