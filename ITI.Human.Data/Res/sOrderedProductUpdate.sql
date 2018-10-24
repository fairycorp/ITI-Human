-- SetupConfig: {}
create proc ITIH.sOrderedProductUpdate (
	@ActorId int,
	@OrderedProductId int,
	@HasBeenDelivered bit,
	@Result bit output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @initialDeliverStatus int;
	declare @changedDeliverStatus int;

	set @initialDeliverStatus = 
		(select HasBeenDelivered from tOrderedProduct where OrderedProductId = @OrderedProductId);

	update tOrderedProduct set HasBeenDelivered = @HasBeenDelivered;

	set @changedDeliverStatus = 
		(select HasBeenDelivered from tOrderedProduct where OrderedProductId = @OrderedProductId);

	if (@initialDeliverStatus != @changedDeliverStatus)
		set @Result = 1;
	else
		set @Result = 0;

	--<PostCreate />

	--[endsp]
end;