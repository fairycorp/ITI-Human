create table ITIH.tOrderedProduct (
	OrderedProductId int not null identity(0, 1),
	OrderId int not null,
	StorageLinkedProductId int not null,
	Quantity int not null default 1,
	CurrentState int not null default 0
	
	constraint PK_ITIH_tOrderedProduct primary key (OrderedProductId),
	constraint FK_ITIH_tOrderedProduct_OrderId foreign key (OrderId) references ITIH.tOrder (OrderId),
	constraint FK_ITIH_tOrderedProduct_StorageLinkedProductId foreign key (StorageLinkedProductId) references ITIH.tStorageLinkedProduct (StorageLinkedProductId)
);

insert into ITIH.tOrderedProduct (OrderId, StorageLinkedProductId, Quantity)
	values (0, 0, 0);