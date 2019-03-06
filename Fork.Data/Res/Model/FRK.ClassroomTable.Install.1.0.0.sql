create table FRK.tClassroom (
	ClassroomId int not null identity(0, 1),
	[Name] nvarchar(126) not null

	constraint PK_ITIH_tClassroom primary key (ClassroomId)
)

insert into FRK.tClassroom ([Name]) values (N'');
insert into FRK.tClassroom ([Name]) values ('E01');
insert into FRK.tClassroom ([Name]) values ('E02');
insert into FRK.tClassroom ([Name]) values ('E03');
insert into FRK.tClassroom ([Name]) values ('E04');
insert into FRK.tClassroom ([Name]) values ('E05');
insert into FRK.tClassroom ([Name]) values ('E06');
insert into FRK.tClassroom ([Name]) values ('E07');
insert into FRK.tClassroom ([Name]) values ('E08');
insert into FRK.tClassroom ([Name]) values ('E09');
insert into FRK.tClassroom ([Name]) values ('E0S');
insert into FRK.tClassroom ([Name]) values ('E0T');
