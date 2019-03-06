--SetupConfig:{}
create proc ITIH.sStallUpdate (
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

	set @previousState = (select OpenedStall from ITIH.tStorage where StorageId = @StorageId);
	update ITIH.tStorage set OpenedStall = @OpenedStall where StorageId = @StorageId;
	set @newState = (select OpenedStall from ITIH.tStorage where StorageId = @StorageId);

	if (@previousState != @newState)
		set @Success = 1;

	--<PostCreate />
	
	--[endsp]
end;