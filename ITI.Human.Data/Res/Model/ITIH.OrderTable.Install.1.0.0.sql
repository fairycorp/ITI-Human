create table ITIH.tOrder (
	OrderId int not null identity(0, 1),
	UserId int not null,
	CreationDate datetime2 not null

	constraint PK_ITIH_tOrder primary key (OrderId),
	constraint FK_ITIH_tOrder_UserId foreign key (UserId) references CK.tUser (UserId)
);

insert into ITIH.tOrder (UserId, CreationDate)
    values (0, sysutcdatetime());