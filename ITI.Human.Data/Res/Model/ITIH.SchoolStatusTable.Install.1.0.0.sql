create table ITIH.tSchoolStatus (
	SchoolStatusId int not null identity(0, 1),
	SchoolStatusName nvarchar(126) not null

	constraint PK_tSchoolStatus_SchoolStatusId primary key (SchoolStatusId)
);

insert into ITIH.tSchoolStatus (SchoolStatusName) values (N'');
insert into ITIH.tSchoolStatus (SchoolStatusName) values ('Directeur');
insert into ITIH.tSchoolStatus (SchoolStatusName) values ('Professeur');
insert into ITIH.tSchoolStatus (SchoolStatusName) values ('Administrant');
insert into ITIH.tSchoolStatus (SchoolStatusName) values ('Intervenant');
insert into ITIH.tSchoolStatus (SchoolStatusName) values ('Étudiant');
insert into ITIH.tSchoolStatus (SchoolStatusName) values ('Tiers');
