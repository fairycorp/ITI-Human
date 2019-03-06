create view FRK.vOrderedProducts
as
	select
		OrderedProductId = op.OrderedProductId,
		OrderId = op.OrderId,
		StorageLinkedProductId = slp.StorageLinkedProductId,
		[Name] = p.[Name],
		[Desc] = p.[Desc],
		Quantity = op.Quantity,
		UnitPrice = slp.UnitPrice,
		CurrentState = op.CurrentState,
		PaymentState = op.PaymentState
	from FRK.tOrderedProduct op
		join FRK.tStorageLinkedProduct slp on slp.StorageLinkedProductId = op.StorageLinkedProductId
		join FRK.tProduct p on p.ProductId = slp.ProductId
	where op.OrderId <> 0;