create table ITIH.tProduct (
	ProductId int not null identity(0, 1),
	[Name] nvarchar(256) not null,
	[Desc] nvarchar(512) not null,
	Price float not null

	constraint PK_ITIH_tProduct primary key (ProductId),
);

insert into ITIH.tProduct ([Name], [Desc], Price)
	values (N'', N'', 0);