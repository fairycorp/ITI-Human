-- SetupConfig: {}
create proc ITIH.sOrderPaymentCreate (
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

	insert into ITIH.tOrderPayment (OrderFinalDueId, OrderedProductId, Amount, PaymentTime)
		values (@OrderFinalDueId, @OrderedProductId, @Amount, @PaymentTime);

	set @OrderPaymentId = scope_identity();

	--<PostCreate />

	--[endsp]
end;