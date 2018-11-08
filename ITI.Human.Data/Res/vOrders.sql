create view ITIH.vOrders
as
	select
		OrderId = o.OrderId,
		UserId = o.UserId,
		UserName = u.UserName,
		ClassroomName = c.[Name],
		CreationDate = o.CreationDate,
		CurrentState = o.CurrentState
	from tOrder o
		join CK.tUser u on u.UserId = o.UserId
		join ITIH.tClassroom c on c.ClassroomId = o.ClassroomId
	where OrderId <> 0;