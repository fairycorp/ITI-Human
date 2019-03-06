create view FRK.vSchoolMembers
as
	select
		UserId = sm.UserId,
		AvatarUrl = ua.[Url],
		UserName = u.UserName,
		FirstName = ud.FirstName,
		LastName = ud.LastName,
		BirthDate = ud.BirthDate,
		SchoolMemberId = sm.SchoolMemberId,
		SchoolStatusId = sm.SchoolStatusId,
		SchoolStatusName = ss.SchoolStatusName
	from FRK.tSchoolMember sm
		left join CK.tUser u on u.UserId = sm.UserId
		left join FRK.tUserDetails ud on ud.UserId = sm.UserId
		left join FRK.tUserAvatars ua on ua.UserId = sm.UserId
		left join FRK.tSchoolStatus ss on ss.SchoolStatusId = sm.SchoolStatusId
	where sm.SchoolMemberId <> 0;