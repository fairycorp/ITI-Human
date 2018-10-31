--SetupConfig: {}
create proc ITIH.sOrderedProductCreate (
	@ActorId int,
	@OrderId int,
	@ProductId int,
	@Amount int,
	@OrderedProductId int output
)
as
begin
	--[beginsp]
	--<PreCreate revert />

	insert into ITIH.tOrderedProduct (OrderId, ProductId, Amount)
		values (@OrderId, @ProductId, @Amount);

	set @OrderedProductId = scope_identity();

	--<PostCreate />
	--[endsp]
end;