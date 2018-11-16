-- SetupConfig: {}
create proc ITIH.sOrderedProductPaymentStateUpdate (
	@ActorId int,
	@UpdateDate datetime2,
	@OrderedProductId int,
	@OrderFinalDueId int,
	@PaymentState int,
	@Amount float,
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

			insert into ITIH.tUpdateTrack (ActorId, UpdateDate) values (@ActorId, @UpdateDate);

			set @updateTrack = scope_identity();
			insert into ITIH.tOrderedProductUpdateTrack (UpdateTrackId, OrderedProductId) values (@updateTrack, @OrderedProductId);

			set @OPUpdateTrack = scope_identity();
			insert into ITIH.tOPPaymentStateUpdateTrack (OPUpdateTrackId, PreviousState, NewState) values (@OPUpdateTrack, @previousState, @newState);
			
			insert into ITIH.tOrderPayment (OrderFinalDueId, Amount, PaymentTime) values (@OrderFinalDueId, @Amount, @UpdateDate);
			update ITIH.tOrderFinalDue 
				set Paid = 
					((select Paid from ITIH.tOrderFinalDue where OrderFinalDueId = @OrderFinalDueId) + @Amount)
				where OrderFinalDueId = @OrderFinalDueId;
		end;

	--<PostCreate />

	--[endsp]
end;