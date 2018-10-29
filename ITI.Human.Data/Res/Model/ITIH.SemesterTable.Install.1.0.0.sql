create table ITIH.tSemester (
	SemesterId int not null identity(0, 1),
	[Name] nvarchar(256)
);

insert into ITIH.tSemester ([Name]) values (N'');
insert into ITIH.tSemester ([Name]) values ('S1');
insert into ITIH.tSemester ([Name]) values ('S2');
insert into ITIH.tSemester ([Name]) values ('S3');
insert into ITIH.tSemester ([Name]) values ('S4');
insert into ITIH.tSemester ([Name]) values ('S5');