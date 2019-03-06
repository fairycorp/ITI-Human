create view FRK.vStorageLinkedProducts
as
	select
		StorageLinkedProductId = slp.StorageLinkedProductId,
		StorageId = slp.StorageId,
		ProductId = slp.ProductId,
		ProductName = p.[Name],
		ProductDesc = p.[Desc],
		ProductAvatarUrl = p.[Url],
		UnitPrice = slp.UnitPrice,
		Stock = slp.Stock,
		CreditState = slp.CreditState
	from FRK.tStorageLinkedProduct slp
		join FRK.tProduct p on p.ProductId = slp.ProductId
	where slp.StorageLinkedProductId <> 0;