-- SetupConfig: {}
create proc ITIH.sStudentCreate (
	@ActorId int,
	@UserId int,
	@SemesterId int,
	@StudentIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into ITIH.tStudent (UserId, SemesterId)
		values (@UserId, @SemesterId);

	set @StudentIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;