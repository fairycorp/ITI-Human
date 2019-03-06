-- SetupConfig: {}
create proc FRK.sOrderPaymentCreate (
	@ActorId int,
	@OrderFinalDueId int,
	@OrderedProductId int,
	@Amount int,
	@PaymentTime datetime2,
	@OrderPaymentId int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into FRK.tOrderPayment (OrderFinalDueId, OrderedProductId, Amount, PaymentTime)
		values (@OrderFinalDueId, @OrderedProductId, @Amount, @PaymentTime);

	set @OrderPaymentId = scope_identity();

	--<PostCreate />

	--[endsp]
end;