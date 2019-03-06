create view FRK.vUserProfile
as
	select
		UserId = ud.UserId,
		UserName = u.UserName,
		Firstname = ud.FirstName,
		Lastname = ud.LastName
	from FRK.tUserDetails ud
		join CK.tUser u on u.UserId = ud.UserId
	where ud.UserId <> 0;