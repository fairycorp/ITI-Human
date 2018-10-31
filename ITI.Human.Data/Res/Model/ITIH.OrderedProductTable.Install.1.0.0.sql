create table ITIH.tOrderedProduct (
	OrderedProductId int not null identity(0, 1),
	OrderId int not null,
	ProductId int not null,
	Amount int not null default 1,
	HasBeenDelivered bit not null default 0
	
	constraint PK_ITIH_tOrderedProduct primary key (OrderedProductId),
	constraint FK_ITIH_tOrderedProduct_OrderId foreign key (OrderId) references ITIH.tOrder (OrderId),
	constraint FK_ITIH_tOrderedProduct_ProductId foreign key (ProductId) references ITIH.tProduct (ProductId)
);

insert into ITIH.tOrderedProduct (OrderId, ProductId)
	values (0, 0);