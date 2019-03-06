--SetupConfig: {}
create proc FRK.sStorageLinkedProductUnitPriceUpdate (
	@ActorId int,
	@UpdateDate datetime2,
	@StorageLinkedProductId int,
	@UnitPrice int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @previousUnitPrice float;
	declare @newUnitPrice float;
	declare @updateTrack int;
	declare @SLPUpdateTrack int;

	set @previousUnitPrice = (select UnitPrice from FRK.tStorageLinkedProduct where StorageLinkedProductId = @StorageLinkedProductId);
	update FRK.tStorageLinkedProduct set UnitPrice = @UnitPrice where StorageLinkedProductId = @StorageLinkedProductId;
	set @newUnitPrice = (select UnitPrice from FRK.tStorageLinkedProduct where StorageLinkedProductId = @StorageLinkedProductId);

	if (@newUnitPrice != @previousUnitPrice)
		begin;
			set @Success = 1;

			insert into FRK.tUpdateTrack (ActorId, UpdateDate) values (@ActorId, @UpdateDate);

			set @updateTrack = scope_identity();
			insert into FRK.tStorageLinkedProductUpdateTrack (UpdateTrackId, StorageLinkedProductId) values (@updateTrack, @StorageLinkedProductId);

			set @SLPUpdateTrack = scope_identity();
			insert into FRK.tSLPUnitPriceUpdateTrack (SLPUpdateTrackId, PreviousUnitPrice, NewUnitPrice) values (@SLPUpdateTrack, @previousUnitPrice, @newUnitPrice);
		end;

	--<PostCreate />

	--[endsp]
end;