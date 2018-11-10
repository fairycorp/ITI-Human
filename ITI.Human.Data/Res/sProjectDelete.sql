--SetupConfig: {}
create proc ITIH.sProjectDelete (
	@ActorId int,
	@ProjectId int,
	@StorageId int = 0,
	@Success bit = 0 output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	set @StorageId = (select StorageId from ITIH.tStorage where ProjectId = @ProjectId);
	
	if (@StorageId != 0)
		delete from ITIH.tStorageLinkedProduct where StorageId = @StorageId;
		delete from ITIH.tStorage where StorageId = @StorageId;

	delete from ITIH.tProject where ProjectId = @ProjectId;

	declare @DoesIdStillExist int;
	set @DoesIdStillExist = (select ProjectId from tProject where ProjectId = @ProjectId);

	if (@DoesIdStillExist is null)
		set @Success = 1;

	--<PostCreate />
	
	--[endsp]
end;