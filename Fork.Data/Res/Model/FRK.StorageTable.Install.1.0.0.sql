create table FRK.tStorage (
	StorageId int not null identity(0, 1),
	ProjectId int not null,
	OpenedStall bit not null default 0

	constraint PK_tStorage_StorageId primary key (StorageId),
	constraint FK_tStorage_ProjectId foreign key (ProjectId) references FRK.tProject (ProjectId)
)

insert into FRK.tStorage (ProjectId) values (0);
