create view ITIH.vOrderedProducts
as
	select
		OrderedProductId = op.OrderedProductId,
		ProductId = p.ProductId,
		[Name] = p.[Name],
		[Desc] = p.[Desc],
		Price = p.Price
	from tOrderedProduct op
		join tProduct p on p.ProductId = op.ProductId