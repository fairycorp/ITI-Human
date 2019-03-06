--SetupConfig: {}
create proc FRK.sStorageLinkedProductCreate (
	@ActorId int,
	@StorageId int,
	@ProductId int,
	@UnitPrice int,
	@Stock int,
	@StorageLinkedProductIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into FRK.tStorageLinkedProduct (StorageId, ProductId, UnitPrice, Stock)
		values (@StorageId, @ProductId, @UnitPrice, @Stock);

	set @StorageLinkedProductIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;