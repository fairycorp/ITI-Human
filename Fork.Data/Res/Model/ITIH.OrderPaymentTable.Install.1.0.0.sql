create table ITIH.tOrderPayment (
	OrderPaymentId int not null identity(0, 1),
	OrderFinalDueId int not null,
	OrderedProductId int not null,
	Amount int not null,
	PaymentTime datetime2 default sysutcdatetime()

	constraint PK_tOrderPaymentTable_OrderPaymentId primary key (OrderPaymentId),
	constraint FK_tOrderPaymentTable_OrderedProductId foreign key (OrderedProductId) references ITIH.tOrderedProduct (OrderedProductId),
	constraint FK_tOrderPaymentTable_OrderFinalDueId foreign key (OrderFinalDueId) references ITIH.tOrderFinalDue (OrderFinalDueId)
);

insert into ITIH.tOrderPayment (OrderedProductId, OrderFinalDueId, Amount)
	values (0, 0, 0);