create view FRK.vOrders
as
	select
		OrderId = o.OrderId,
		UserId = o.UserId,
		StorageId = o.StorageId,
		UserName = u.UserName,
		ClassroomId = o.ClassroomId,
		ClassroomName = c.[Name],
		CreationDate = o.CreationDate,
		CurrentState = o.CurrentState
	from FRK.tOrder o
		join CK.tUser u on u.UserId = o.UserId
		join FRK.tClassroom c on c.ClassroomId = o.ClassroomId
	where OrderId <> 0;