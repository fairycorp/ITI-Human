-- SetupConfig: {}
create proc ITIH.sProjectMemberDelete (
	@ActorId int,
	@ProjectMemberId int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />
	
	declare @stillExisting int;

	delete from ITIH.tProjectMember where ProjectMemberId = @ProjectMemberId;
	set @stillExisting = (select ProjectMemberId from ITIH.tProjectMember where ProjectMemberId = @ProjectMemberId);
	
	if (@stillExisting is null)
		begin; 
			set @Success = 1;
		end;

	--<PostCreate />

	--[endsp]
end;