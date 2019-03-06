-- SetupConfig: {}
create proc ITIH.sOrderPaymentDelete (
	@ActorId int,
	@OrderedProductId int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @stillExisting int;
	declare @updateTrack int;
	declare @OPUpdateTrack int;

	update ITIH.tOrderedProduct set PaymentState = 0 where OrderedProductId = @OrderedProductId;

	delete from ITIH.tOrderPayment where OrderedProductId = @OrderedProductId;
	set @stillExisting = (select OrderPaymentId from tOrderPayment where OrderedProductId = @OrderedProductId);

	if (@stillExisting is null)
		begin;
			set @Success = 1;

			insert into ITIH.tUpdateTrack (ActorId, UpdateDate) values (@ActorId, sysutcdatetime());

			set @updateTrack = scope_identity();
			insert into ITIH.tOrderedProductUpdateTrack (UpdateTrackId, OrderedProductId) values (@updateTrack, @OrderedProductId);

			set @OPUpdateTrack = scope_identity();
			insert into ITIH.tOPPaymentStateUpdateTrack (OPUpdateTrackId, PreviousState, NewState) values (@OPUpdateTrack, 1, 0);
		end;

	--<PostCreate />

	--[endsp]
end;