create view ITIH.vStorageLinkedProducts
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
	from ITIH.tStorageLinkedProduct slp
		join ITIH.tProduct p on p.ProductId = slp.ProductId
	where slp.StorageLinkedProductId <> 0;