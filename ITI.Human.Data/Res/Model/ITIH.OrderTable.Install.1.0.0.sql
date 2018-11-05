create table ITIH.tOrder (
	OrderId int not null identity(0, 1),
	UserId int not null,
	ClassroomId int not null default 0,
	CurrentMode int not null default 0,
	CreationDate datetime2 not null,
	CurrentState int not null default 0,
	HasBeenEntirelyDelivered bit not null default 0

	constraint PK_ITIH_tOrder primary key (OrderId),
	constraint FK_ITIH_tOrder_UserId foreign key (UserId) references CK.tUser (UserId),
	constraint FK_ITIH_Order_ClassroomId foreign key (ClassroomId) references ITIH.tClassroom (ClassroomId)
);

insert into ITIH.tOrder (UserId, CreationDate)
    values (0, sysutcdatetime());