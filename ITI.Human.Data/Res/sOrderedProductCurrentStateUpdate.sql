-- SetupConfig: {}
create proc ITIH.sOrderedProductCurrentStateUpdate (
	@ActorId int,
	@UpdateDate datetime2,
	@OrderedProductId int,
	@CurrentState int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @previousState int;
	declare @newState int;
	declare @updateTrack int;
	declare @OPUpdateTrack int;

	set @previousState = (select CurrentState from tOrderedProduct where OrderedProductId = @OrderedProductId);
	update tOrderedProduct set CurrentState = @CurrentState where OrderedProductId = @OrderedProductId;
	set @newState = (select CurrentState from tOrderedProduct where OrderedProductId = @OrderedProductId);

	if (@previousState != @newState)
		begin;
			set @Success = 1;

			insert into ITIH.tUpdateTrack (ActorId, UpdateDate) values (@ActorId, @UpdateDate);

			set @updateTrack = scope_identity();
			insert into ITIH.tOrderedProductUpdateTrack (UpdateTrackId, OrderedProductId) values (@updateTrack, @OrderedProductId);

			set @OPUpdateTrack = scope_identity();
			insert into ITIH.tOPCurrentStateUpdateTrack (OPUpdateTrackId, PreviousState, NewState) values (@OPUpdateTrack, @previousState, @newState);
		end;

	--<PostCreate />

	--[endsp]
end;