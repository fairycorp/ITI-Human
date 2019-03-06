create view FRK.vProjects
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
		StorageId = case when (p.SemesterId < 4) then 0 else sto.StorageId end,
		OpenedStall = case when (p.SemesterId < 4) then 0 else sto.OpenedStall end
	from FRK.tProject p
		left join FRK.tProjectType typ on typ.TypeId = p.TypeId
		left join FRK.tSemester sem on sem.SemesterId = p.SemesterId
		left join FRK.tStorage sto on sto.ProjectId = p.ProjectId
	where p.ProjectId <> 0;