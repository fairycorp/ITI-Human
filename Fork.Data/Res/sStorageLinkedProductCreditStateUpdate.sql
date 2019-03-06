--SetupConfig: {}
create proc FRK.sStorageLinkedProductCreditStateUpdate (
	@ActorId int,
	@UpdateDate datetime2,
	@StorageLinkedProductId int,
	@CreditState bit,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @previousState int;
	declare @newState int;
	declare @updateTrack int;
	declare @SLPUpdateTrack int;

	set @previousState = (select CreditState from FRK.tStorageLinkedProduct where StorageLinkedProductId = @StorageLinkedProductId);
	update FRK.tStorageLinkedProduct set CreditState = @CreditState where StorageLinkedProductId = @StorageLinkedProductId;
	set @newState = (select CreditState from FRK.tStorageLinkedProduct where StorageLinkedProductId = @StorageLinkedProductId);

	if (@previousState != @newState)
		begin;
			set @Success = 1;

			insert into FRK.tUpdateTrack (ActorId, UpdateDate) values (@ActorId, @UpdateDate);

			set @updateTrack = scope_identity();
			insert into FRK.tStorageLinkedProductUpdateTrack (UpdateTrackId, StorageLinkedProductId) values (@updateTrack, @StorageLinkedProductId);

			set @SLPUpdateTrack = scope_identity();
			insert into FRK.tSLPCreditStateUpdateTrack (SLPUpdateTrackId, PreviousCreditState, NewCreditState) values (@SLPUpdateTrack, @previousState, @newState);
		end;

	--<PostCreate />

	--[endsp]
end;