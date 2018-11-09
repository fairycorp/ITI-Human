--SetupConfig: {}
create proc ITIH.sStorageLinkedProductCreate (
	@ActorId int,
	@StorageId int,
	@ProductId int,
	@UnitPrice float,
	@Stock int,
	@StorageLinkedProductIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into ITIH.tStorageLinkedProduct (StorageId, ProductId, UnitPrice, Stock)
		values (@StorageId, @ProductId, @UnitPrice, @Stock);

	set @StorageLinkedProductIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;