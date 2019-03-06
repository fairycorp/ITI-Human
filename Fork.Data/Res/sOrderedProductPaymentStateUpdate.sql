-- SetupConfig: {}
create proc FRK.sOrderedProductPaymentStateUpdate (
	@ActorId int,
	@UpdateDate datetime2,
	@OrderedProductId int,
	@OrderFinalDueId int,
	@PaymentState int,
	@Amount int,
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

	set @previousState = (select PaymentState from tOrderedProduct where OrderedProductId = @OrderedProductId);
	update tOrderedProduct set PaymentState = @PaymentState where OrderedProductId = @OrderedProductId;
	set @newState = (select PaymentState from tOrderedProduct where OrderedProductId = @OrderedProductId);

	if (@previousState != @newState)
		begin;
			set @Success = 1;

			insert into FRK.tUpdateTrack (ActorId, UpdateDate) values (@ActorId, @UpdateDate);

			set @updateTrack = scope_identity();
			insert into FRK.tOrderedProductUpdateTrack (UpdateTrackId, OrderedProductId) values (@updateTrack, @OrderedProductId);

			set @OPUpdateTrack = scope_identity();
			insert into FRK.tOPPaymentStateUpdateTrack (OPUpdateTrackId, PreviousState, NewState) values (@OPUpdateTrack, @previousState, @newState);
			
			if (@PaymentState = 1)
				insert into FRK.tOrderPayment (OrderedProductId, OrderFinalDueId, Amount, PaymentTime) values (@OrderedProductId, @OrderFinalDueId, @Amount, @UpdateDate);

			update FRK.tOrderFinalDue 
				set Paid = 
					((select Paid from FRK.tOrderFinalDue where OrderFinalDueId = @OrderFinalDueId) + @Amount)
				where OrderFinalDueId = @OrderFinalDueId;
		end;

	--<PostCreate />

	--[endsp]
end;