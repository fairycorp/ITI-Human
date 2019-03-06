--SetupConfig:{}
create proc FRK.sStallUpdate (
	@ActorId int,
	@StorageId int,
	@OpenedStall bit,
	@Success bit = 0 output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	declare @previousState bit;
	declare @newState bit;

	set @previousState = (select OpenedStall from FRK.tStorage where StorageId = @StorageId);
	update FRK.tStorage set OpenedStall = @OpenedStall where StorageId = @StorageId;
	set @newState = (select OpenedStall from FRK.tStorage where StorageId = @StorageId);

	if (@previousState != @newState)
		set @Success = 1;

	--<PostCreate />
	
	--[endsp]
end;