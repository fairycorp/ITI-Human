-- SetupConfig: {}
create proc ITIH.sOrderCreditCreate (
	@ActorId int,
	@OrderedProductId int,
	@Amount int,
	@CreditTime datetime2,
	@OrderCreditId int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into ITIH.tOrderCredit (OrderedProductId, Amount, CreditTime)
		values (@OrderedProductId, @Amount, @CreditTime);

	set @OrderCreditId = scope_identity();

	--<PostCreate />

	--[endsp]
end;