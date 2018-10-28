--SetupConfig: {}
create proc ITIH.sSemesterCreate (
	@ActorId int,
	@Name nvarchar(256),
	@SemesterIdResult int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into ITIH.tSemester ([Name]) values (@Name);

	set @SemesterIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;