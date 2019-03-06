-- SetupConfig: {}
create proc ITIH.sOrderedProductDelete (
	@ActorId int,
	@OrderedProductId int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	delete from tOrderedProduct where OrderedProductId = @OrderedProductId;

	declare @DoesIdStillExist int;
	set @DoesIdStillExist = 
		(select OrderedProductId 
			from tOrderedProduct
			where OrderedProductId = @OrderedProductId);

	if (@DoesIdStillExist is null)
		set @Success = 1;

	--<PostCreate />

	--[endsp]
end;