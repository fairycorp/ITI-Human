create view FRK.vProjectMembers
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
	from FRK.tProjectMember pm
		left join FRK.tProject p on p.ProjectId = pm.ProjectId
		left join CK.tUser u on u.UserId = pm.UserId
		left join FRK.tProjectRank pr on pr.ProjectRankId = pm.ProjectRankId
		left join FRK.tUserAvatars ua on ua.UserId = pm.UserId
