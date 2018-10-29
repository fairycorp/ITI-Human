-- SetupConfig: {}
create proc ITIH.sOrderedProductUpdate (
	@ActorId int,
	@OrderedProductId int,
	@HasBeenDelivered bit,
	@CurrentStateResult bit output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	update tOrderedProduct set HasBeenDelivered = @HasBeenDelivered;

	set @CurrentStateResult = (select @HasBeenDelivered from tOrderedProduct where OrderedProductId = @OrderedProductId);

	--<PostCreate />

	--[endsp]
end;