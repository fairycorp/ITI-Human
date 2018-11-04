create view ITIH.vOrders
as
	select
		OrderId = o.OrderId,
		UserId = o.UserId,
		UserName = u.UserName
	from tOrder o
		join CK.tUser u on u.UserId = o.UserId
	where OrderId <> 0;