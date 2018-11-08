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
	from tOrderedProduct op
		join tStorageLinkedProduct slp on slp.StorageLinkedProductId = op.StorageLinkedProductId
		join tProduct p on p.ProductId = slp.ProductId
	where op.OrderId <> 0;