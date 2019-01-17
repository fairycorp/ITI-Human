create table ITIH.tProjectRank (
	ProjectRankId int not null identity(0, 1),
	[Name] nvarchar(256) not null

	constraint PK_tProjectRank_ProjectRankId primary key (ProjectRankId)
);

insert into ITIH.tProjectRank ([Name]) values (N'');
insert into ITIH.tProjectRank ([Name]) values ('Chef de projet');
insert into ITIH.tProjectRank ([Name]) values ('Standard');