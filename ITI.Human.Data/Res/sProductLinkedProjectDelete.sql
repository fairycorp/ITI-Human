--SetupConfig: {}
create proc ITIH.sProductLinkedProjectDelete (
	@ActorId int,
	@ProductLinkedProjectId int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]
	--<PreCreate revert />

	delete from tProductLinkedProject where ProductLinkedProjectId = @ProductLinkedProjectId;

	declare @DoesIdStillExist int;
	set @DoesIdStillExist = 
		(select ProductLinkedProjectId
			from tProductLinkedProject
			where ProductLinkedProjectId = @ProductLinkedProjectId);

	if (@DoesIdStillExist is null)
		set @Success = 1;

	--<PostCreate />
	--[endsp]
end;