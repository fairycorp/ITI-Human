create view ITIH.vUserProfile
as
	select
		UserId = ud.UserId,
		UserName = u.UserName,
		Firstname = ud.FirstName,
		Lastname = ud.LastName
	from ITIH.tUserDetails ud
		join CK.tUser u on u.UserId = ud.UserId
	where ud.UserId <> 0;