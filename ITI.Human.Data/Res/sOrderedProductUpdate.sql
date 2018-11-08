-- SetupConfig: {}
create proc ITIH.sOrderedProductUpdate (
	@ActorId int,
	@OrderedProductId int,
	@CurrentState int,
	@CurrentStateResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	update tOrderedProduct set CurrentState = @CurrentState;

	set @CurrentStateResult = (select CurrentState from tOrderedProduct where OrderedProductId = @OrderedProductId);

	--<PostCreate />

	--[endsp]
end;