create view ITIH.vProjectMembers
as
	select
		ProjectMemberId = pm.ProjectMemberId,
		ProjectId = pm.ProjectId,
		ProjectName = p.[Name],
		ProjectRankId = pm.ProjectRankId,
		ProjectRankName = pr.[Name],
		UserId = pm.UserId,
		UserName = u.UserName,
		AvatarUrl = ua.[Url]
	from ITIH.tProjectMember pm
		left join ITIH.tProject p on p.ProjectId = pm.ProjectId
		left join CK.tUser u on u.UserId = pm.UserId
		left join ITIH.tProjectRank pr on pr.ProjectRankId = pm.ProjectRankId
		left join ITIH.tUserAvatars ua on ua.UserId = pm.UserId