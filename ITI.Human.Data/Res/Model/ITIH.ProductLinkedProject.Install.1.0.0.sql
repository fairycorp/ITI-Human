create table ITIH.tProductLinkedProject (
	ProductLinkedProjectId int not null identity(0, 1),
	ProductId int not null,
	ProjectId int not null,
	[Availability] bit not null

	constraint PK_tProductLinkedProject primary key (ProductLinkedProjectId),
	constraint FK_tProductLinkedProject_ProductId foreign key (ProductId) references ITIH.tProduct (ProductId),
	constraint FK_tProductLinkedProject_ProjectId foreign key (ProjectId) references ITIH.tProject (ProjectId)
);

insert into ITIH.tProductLinkedProject (ProductId, ProjectId, [Availability])
	values (0, 0, 0);