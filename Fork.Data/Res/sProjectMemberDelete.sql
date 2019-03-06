-- SetupConfig: {}
create proc FRK.sProjectMemberDelete (
	@ActorId int,
	@ProjectMemberId int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />
	
	declare @stillExisting int;

	delete from FRK.tProjectMember where ProjectMemberId = @ProjectMemberId;
	set @stillExisting = (select ProjectMemberId from FRK.tProjectMember where ProjectMemberId = @ProjectMemberId);
	
	if (@stillExisting is null)
		begin; 
			set @Success = 1;
		end;

	--<PostCreate />

	--[endsp]
end;