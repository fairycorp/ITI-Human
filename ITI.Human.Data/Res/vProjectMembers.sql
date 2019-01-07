create view ITIH.vProjectMembers
as
	select
		ProjectMemberId = pm.ProjectMemberId,
		ProjectId = pm.ProjectId,
		ProjectName = p.[Name],
		ProjectRankId = pm.ProjectRankId,
		ProjectRankName = pr.[Name],
		UserId = pm.UserId,
		UserName = u.UserName
	from ITIH.tProjectMember pm
		join ITIH.tProject p on p.ProjectId = pm.ProjectId
		join CK.tUser u on u.UserId = pm.UserId
		join ITIH.tProjectRank pr on pr.ProjectRankId = pm.ProjectRankId