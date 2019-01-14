create view ITIH.vUserBalance
as
	select
		UserBalanceId = uB.UserBalanceId,
		ProjectId = uB.ProjectId,
		UserId = uB.UserId,
		UserName = u.UserName,
		FirstName = uD.FirstName,
		LastName = uD.LastName,
		AvatarUrl = uA.[Url],
		Balance = uB.Balance
	from ITIH.tUserBalance uB
		left join CK.tUser u on u.UserId = uB.UserId
		left join ITIH.tUserDetails uD on uD.UserId = uB.UserId
		left join ITIH.tUserAvatars uA on uA.UserId = uB.UserId
	where uB.UserBalanceId <> 0;