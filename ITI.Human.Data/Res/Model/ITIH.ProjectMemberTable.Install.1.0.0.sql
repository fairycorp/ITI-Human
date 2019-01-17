create table ITIH.tProjectMember (
	ProjectMemberId int not null identity(0, 1),
	ProjectId int not null,
	ProjectRankId int not null,
	UserId int not null,

	constraint PK_tProjectMember_ProjectMemberId primary key (ProjectMemberId),
	constraint FK_tProjectMember_ProjectId foreign key (ProjectId) references ITIH.tProject (ProjectId),
	constraint FK_tProjectMember_RankId foreign key (ProjectRankId) references ITIH.tProjectRank (ProjectRankId),
	constraint FK_tProjectMember_UserId foreign key (UserId) references CK.tUser (UserId)
);

insert into ITIH.tProjectMember (ProjectId, ProjectRankId, UserId) values (0, 0, 0);