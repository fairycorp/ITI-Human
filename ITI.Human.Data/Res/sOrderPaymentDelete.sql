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

	update ITIH.tOrderedProduct set PaymentState = 0 where OrderedProductId = @OrderedProductId;

	delete from ITIH.tOrderPayment where OrderedProductId = @OrderedProductId;
	set @stillExisting = (select OrderPaymentId from tOrderPayment where OrderedProductId = @OrderedProductId);

	if (@stillExisting is null)
		set @Success = 1;

	--<PostCreate />

	--[endsp]
end;