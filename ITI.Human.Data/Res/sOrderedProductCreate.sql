--SetupConfig: {}
create proc ITIH.sOrderedProductCreate (
	@ActorId int,
	@OrderId int,
	@ProductId int,
	@OrderedProductId int output
)
as
begin
	--[beginsp]
	--<PreCreate revert />

	insert into ITIH.tOrderedProduct (OrderId, ProductId)
		values (@OrderId, @ProductId);

	set @OrderedProductId = scope_identity();

	--<PostCreate />
	--[endsp]
end;