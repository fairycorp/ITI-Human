create view FRK.vUserBalance
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
	from FRK.tUserBalance uB
		left join CK.tUser u on u.UserId = uB.UserId
		left join FRK.tUserDetails uD on uD.UserId = uB.UserId
		left join FRK.tUserAvatars uA on uA.UserId = uB.UserId
	where uB.UserBalanceId <> 0;