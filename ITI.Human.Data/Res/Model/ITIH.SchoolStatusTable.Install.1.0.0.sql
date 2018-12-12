create table ITIH.tSchoolStatus (
	SchoolStatusId int not null identity(0, 1),
	[Name] nvarchar(126) not null

	constraint PK_tSchoolStatus_SchoolStatusId primary key (SchoolStatusId)
);

insert into ITIH.tSchoolStatus ([Name]) values (N'');
insert into ITIH.tSchoolStatus ([Name]) values ('Directeur');
insert into ITIH.tSchoolStatus ([Name]) values ('Professeur');
insert into ITIH.tSchoolStatus ([Name]) values ('Administrant');
insert into ITIH.tSchoolStatus ([Name]) values ('Élève');
