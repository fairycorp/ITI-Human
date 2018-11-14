create table ITIH.tOrderPaymentTable (
	OrderPaymentId int not null identity(0, 1),
	OrderFinalDueId int not null,
	Amount float not null,
	PaymentTime datetime2 default sysutcdatetime()

	constraint PK_tOrderPaymentTable_OrderPaymentId primary key (OrderPaymentId),
	constraint FK_tOrderPaymentTable_OrderFinalDueId foreign key (OrderFinalDueId) references ITIH.tOrderFinalDue (OrderFinalDueId)
);

insert into ITIH.tOrderPaymentTable (OrderFinalDueId, Amount)
	values (0, 0);