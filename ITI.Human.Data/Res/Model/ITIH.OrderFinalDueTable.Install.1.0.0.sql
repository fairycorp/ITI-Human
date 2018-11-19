create table ITIH.tOrderFinalDue (
	OrderFinalDueId int not null identity(0, 1),
	OrderId int not null,
	Total int not null,
	Paid int not null default 0

	constraint PK_ITIH_tOrderFinalDue_OrderFinalDueId primary key (OrderFinalDueId),
	constraint FK_ITIH_tOrderFinalDue_OrderId foreign key (OrderId) references ITIH.tOrder (OrderId)
);

insert into ITIH.tOrderFinalDue (OrderId, Total)
    values (0, 0);