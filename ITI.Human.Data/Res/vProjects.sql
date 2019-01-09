create view ITIH.vProjects
as
	select
		ProjectId = p.ProjectId,
		ProjectTypeId = p.TypeId,
		ProjectTypeName = typ.[Name],
		ProjectName = p.[Name],
		ProjectHeadline = p.Headline,
		ProjectPitch = p.Pitch,
		SemesterId = p.SemesterId,
		SemesterName = sem.[Name],
		StorageId = case when (p.SemesterId < 4) then 0 else sto.StorageId end
	from ITIH.tProject p
		left join ITIH.tProjectType typ on typ.TypeId = p.TypeId
		left join ITIH.tSemester sem on sem.SemesterId = p.SemesterId
		left join ITIH.tStorage sto on sto.ProjectId = p.ProjectId
	where p.ProjectId <> 0;