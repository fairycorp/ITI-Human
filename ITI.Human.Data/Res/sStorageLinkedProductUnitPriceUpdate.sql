--SetupConfig: {}
create proc ITIH.sStorageLinkedProductUnitPriceUpdate (
	@ActorId int,
	@StorageLinkedProductId int,
	@UnitPrice float,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @previousUnitPrice int;
	declare @newUnitPrice int;
	set @previousUnitPrice = (select UnitPrice from ITIH.tStorageLinkedProduct where StorageLinkedProductId = @StorageLinkedProductId);

	update ITIH.tStorageLinkedProduct set UnitPrice = @UnitPrice where StorageLinkedProductId = @StorageLinkedProductId;

	set @newUnitPrice = (select UnitPrice from ITIH.tStorageLinkedProduct where StorageLinkedProductId = @StorageLinkedProductId);

	if (@newUnitPrice != @previousUnitPrice)
		set @Success = 1;
		insert into ITIH.tUpdateTrack (ActorId) values (@ActorId);
		set @updateTrack = scope_identity();
		insert into ITIH.tSLPUpdateTrack (StorageLinkedProductId) values (@StorageLinkedProductId);
		set @SLPUpdateTrack = scope_identity();
		insert into ITIH.tSLPUnitPriceUpdateTrack (SLPUpdateTrackId, PreviousUnitPrice, NewUnitPrice) values (@SLPUpdateTrack, @previousUnitPrice, @newUnitPrice);

	--<PostCreate />

	--[endsp]
end;