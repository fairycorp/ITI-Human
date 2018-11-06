--SetupConfig: {}
create proc ITIH.sStorageLinkedProductCreate (
	@ActorId int,
	@StorageId int,
	@ProductId int,
	@UnitPrice float,
	@Quantity int,
	@StorageLinkedProductIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into ITIH.tStorageLinkedProduct (StorageId, ProductId, UnitPrice, Quantity)
		values (@StorageId, @ProductId, @UnitPrice, @Quantity);

	set @StorageLinkedProductIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;