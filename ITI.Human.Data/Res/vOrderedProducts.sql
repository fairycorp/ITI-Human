create view ITIH.vOrderedProducts
as
	select
		OrderedProductId = op.OrderedProductId,
		OrderId = op.OrderId,
		StorageLinkedProductId = slp.StorageLinkedProductId,
		[Name] = p.[Name],
		[Desc] = p.[Desc],
		Quantity = op.Quantity,
		UnitPrice = slp.UnitPrice
	from ITIH.tOrderedProduct op
		join ITIH.tStorageLinkedProduct slp on slp.StorageLinkedProductId = op.StorageLinkedProductId
		join ITIH.tProduct p on p.ProductId = slp.ProductId
	where op.OrderId <> 0;