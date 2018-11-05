create view ITIH.vOrders
as
	select
		OrderId = o.OrderId,
		UserId = o.UserId,
		UserName = u.UserName,
		CurrentMode = o.CurrentMode,
		ClassroomName = c.[Name],
		CreationDate = o.CreationDate
	from tOrder o
		join CK.tUser u on u.UserId = o.UserId
		join ITIH.tClassroom c on c.ClassroomId = o.ClassroomId
	where OrderId <> 0;