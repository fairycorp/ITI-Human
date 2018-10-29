create view ITIH.vOrderedProducts
as
	select
		OrderedProductId = op.OrderedProductId,
		OrderId = op.OrderId,
		ProductId = p.ProductId,
		[Name] = p.[Name],
		[Desc] = p.[Desc],
		Price = p.Price,
		HasBeenDelivered = op.HasBeenDelivered
	from tOrderedProduct op
		join tProduct p on p.ProductId = op.ProductId