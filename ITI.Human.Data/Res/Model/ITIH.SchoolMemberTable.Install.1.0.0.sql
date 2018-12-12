create table ITIH.tSchoolMember (
	SchoolMemberId int not null identity(0, 1),
	SchoolStatusId int not null,
	UserId int not null,

	constraint PK_tSchoolMember_SchoolMemberId primary key (SchoolMemberId),
	constraint FK_tSchoolMember_SchoolStatusId foreign key (SchoolStatusId) references ITIH.tSchoolStatus (SchoolStatusId),
	constraint FK_tSchoolMember_UserId foreign key (UserId) references CK.tUser (UserId)
);

insert into ITIH.tSchoolMember (SchoolStatusId, UserId) values (0, 0);