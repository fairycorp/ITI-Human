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
		StorageId = sto.StorageId
	from ITIH.tProject p
		join ITIH.tProjectType typ on typ.TypeId = p.TypeId
		join ITIH.tSemester sem on sem.SemesterId = p.SemesterId
		join ITIH.tStorage sto on sto.ProjectId = p.ProjectId
	where p.ProjectId <> 0;