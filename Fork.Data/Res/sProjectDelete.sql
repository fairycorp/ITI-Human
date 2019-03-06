--SetupConfig: {}
create proc FRK.sProjectDelete (
	@ActorId int,
	@ProjectId int,
	@StorageId int = 0,
	@Success bit = 0 output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	set @StorageId = (select StorageId from FRK.tStorage where ProjectId = @ProjectId);
	
	if (@StorageId != 0)
		delete from FRK.tStorageLinkedProduct where StorageId = @StorageId;
		delete from FRK.tStorage where StorageId = @StorageId;

	delete from FRK.tProject where ProjectId = @ProjectId;

	declare @DoesIdStillExist int;
	set @DoesIdStillExist = (select ProjectId from tProject where ProjectId = @ProjectId);

	if (@DoesIdStillExist is null)
		set @Success = 1;

	--<PostCreate />
	
	--[endsp]
end;