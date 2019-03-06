create table FRK.tProduct (
	ProductId int not null identity(0, 1),
	[Name] nvarchar(256) not null,
	[Desc] nvarchar(512) not null,
	[Url] varchar(max) default 'https://image.noelshack.com/fichiers/2019/03/1/1547459850-barcode512.png'

	constraint PK_ITIH_tProduct primary key (ProductId),
);

insert into FRK.tProduct ([Name], [Desc], [Url])
	values (N'', N'', N'');