--SetupConfig: {}
create proc ITIH.sOrderedProductCreate (
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

	insert into ITIH.tOrderedProduct (OrderId, StorageLinkedProductId, Quantity)
		values (@OrderId, @StorageLinkedProductId, @Quantity);

	set @OrderedProductId = scope_identity();

	--<PostCreate />
	--[endsp]
end;