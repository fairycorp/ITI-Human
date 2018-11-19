--SetupConfig: {}
create proc ITIH.sStorageLinkedProductStockUpdate (
	@ActorId int,
	@UpdateDate datetime2,
	@StorageLinkedProductId int,
	@Stock int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @previousStock int;
	declare @newStock int;
	declare @updateTrack int;
	declare @SLPUpdateTrack int;

	set @previousStock = (select Stock from ITIH.tStorageLinkedProduct where StorageLinkedProductId = @StorageLinkedProductId);
	update ITIH.tStorageLinkedProduct set Stock = @Stock where StorageLinkedProductId = @StorageLinkedProductId;
	set @newStock = (select Stock from ITIH.tStorageLinkedProduct where StorageLinkedProductId = @StorageLinkedProductId);

	if (@newStock != @previousStock)
		begin;
			set @Success = 1;

			insert into ITIH.tUpdateTrack (ActorId, UpdateDate) values (@ActorId, @UpdateDate);

			set @updateTrack = scope_identity();
			insert into ITIH.tStorageLinkedProductUpdateTrack (UpdateTrackId, StorageLinkedProductId) values (@updateTrack, @StorageLinkedProductId);

			set @SLPUpdateTrack = scope_identity();
			insert into ITIH.tSLPStockUpdateTrack (SLPUpdateTrackId, PreviousStock, NewStock) values (@SLPUpdateTrack, @previousStock, @newStock);
		end;

	--<PostCreate />

	--[endsp]
end;