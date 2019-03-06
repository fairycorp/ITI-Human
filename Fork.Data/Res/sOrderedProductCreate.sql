--SetupConfig: {}
create proc FRK.sOrderedProductCreate (
	@ActorId int,
	@OrderId int,
	@StorageLinkedProductId int,
	@Quantity int,
	@OrderedProductId int output
)
as
begin
	--[beginsp]
	--<PreCreate revert />

	insert into FRK.tOrderedProduct (OrderId, StorageLinkedProductId, Quantity)
		values (@OrderId, @StorageLinkedProductId, @Quantity);

	set @OrderedProductId = scope_identity();

	--<PostCreate />
	--[endsp]
end;