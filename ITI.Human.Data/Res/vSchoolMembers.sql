create view ITIH.vSchoolMembers
as
	select
		UserId = sm.UserId,
		UserName = u.UserName,
		FirstName = ud.FirstName,
		LastName = ud.LastName,
		BirthDate = ud.BirthDate,
		SchoolMemberId = sm.SchoolMemberId,
		SchoolStatusId = sm.SchoolStatusId,
		SchoolStatusName = ss.SchoolStatusName
	from ITIH.tSchoolMember sm
		join CK.tUser u on u.UserId = sm.UserId
		join ITIH.tUserDetails ud on ud.UserId = sm.UserId
		join ITIH.tSchoolStatus ss on ss.SchoolStatusId = sm.SchoolStatusId
	where sm.SchoolMemberId <> 0;