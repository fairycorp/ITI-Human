create table ITIH.tSchoolMember (
	SchoolMemberId int not null identity(0, 1),
	UserId int not null,
	SchoolStatusId int not null

	constraint PK_tSchoolMember_SchoolMemberId primary key (SchoolMemberId),
	constraint FK_tSchoolMember_UserId foreign key (UserId) references CK.tUser (UserId),
	constraint FK_tSchoolMember_SchoolStatusId foreign key (SchoolStatusId) references ITIH.tSchoolStatus (SchoolStatusId)
);

insert into ITIH.tSchoolMember (UserId, SchoolStatusId) values (0, 0);