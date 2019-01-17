-- SetupConfig: {}
create procedure ITIH.sProjectMemberCreate (
	@ActorId int,
	@ProjectId int,
	@ProjectRankId int,
	@UserId int,
	@ProjectMemberIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />
	
	insert into ITIH.tProjectMember (ProjectId, ProjectRankId, UserId)
		values (@ProjectId, @ProjectRankId, @UserId);

	set @ProjectMemberIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;