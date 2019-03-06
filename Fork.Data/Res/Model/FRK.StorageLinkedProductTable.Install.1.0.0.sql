create table FRK.tStorageLinkedProduct (
	StorageLinkedProductId int not null identity(0, 1),
	StorageId int not null,
	ProductId int not null,
	UnitPrice int not null default 0,
	Stock int not null default 0,
	CreditState bit not null default 1

	constraint PK_tStorageLinkedProduct_StorageLinkedProductId primary key (StorageLinkedProductId),
	constraint FK_tStorageLinkedProduct_StorageId foreign key (StorageId) references FRK.tStorage (StorageId),
	constraint FK_tStorageLinkedProduct_ProductId foreign key (ProductId) references FRK.tProduct (ProductId)
);

insert into FRK.tStorageLinkedProduct (StorageId, ProductId)
	values(0, 0);