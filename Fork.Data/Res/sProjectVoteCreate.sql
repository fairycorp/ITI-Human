--SetupConfig: {}
create proc ITIH.sProjectVoteCreate (
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

	insert into ITIH.tProjectVotes (ProjectId, UserId, Note)
		values (@ProjectId, @UserId, @Note);

	set @ProjectVoteIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;