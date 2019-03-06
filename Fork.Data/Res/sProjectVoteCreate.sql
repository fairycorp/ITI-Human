--SetupConfig: {}
create proc FRK.sProjectVoteCreate (
	@ActorId int,
	@ProjectId int,
	@UserId int,
	@Note int,
	@ProjectVoteIdResult int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into FRK.tProjectVotes (ProjectId, UserId, Note)
		values (@ProjectId, @UserId, @Note);

	set @ProjectVoteIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;