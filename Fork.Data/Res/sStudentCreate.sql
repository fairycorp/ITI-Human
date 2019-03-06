-- SetupConfig: {}
create proc FRK.sStudentCreate (
	@ActorId int,
	@UserId int,
	@SemesterId int,
	@StudentIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into FRK.tStudent (UserId, SemesterId)
		values (@UserId, @SemesterId);

	set @StudentIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;