create table ITIH.tStorage (
	StorageId int not null identity(0, 1),
	ProjectId int not null

	constraint PK_tStorage_StorageId primary key (StorageId),
	constraint FK_tStorage_ProjectId foreign key (ProjectId) references ITIH.tProject (ProjectId)
)

insert into ITIH.tStorage (ProjectId) values (0);