create table ITIH.tOrderCredit (
	OrderCreditId int not null identity(0, 1),
	OrderedProductId int not null,
	Amount int not null,
	CreditTime datetime2 default sysutcdatetime()

	constraint PK_tOrderCredit_OrderCreditId primary key (OrderCreditId),
	constraint FK_tOrderCredit_OrderedProductId foreign key (OrderedProductId) references ITIH.tOrderedProduct (OrderedProductId)
);

insert into ITIH.tOrderCredit (OrderedProductId, Amount)
	values (0, 0);