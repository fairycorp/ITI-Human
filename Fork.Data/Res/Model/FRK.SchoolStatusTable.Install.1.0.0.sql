create table FRK.tSchoolStatus (
	SchoolStatusId int not null identity(0, 1),
	SchoolStatusName nvarchar(126) not null

	constraint PK_tSchoolStatus_SchoolStatusId primary key (SchoolStatusId)
);

insert into FRK.tSchoolStatus (SchoolStatusName) values (N'');
insert into FRK.tSchoolStatus (SchoolStatusName) values ('Professeur');
insert into FRK.tSchoolStatus (SchoolStatusName) values ('Administrant');
insert into FRK.tSchoolStatus (SchoolStatusName) values ('Étudiant');
insert into FRK.tSchoolStatus (SchoolStatusName) values ('Tiers');
