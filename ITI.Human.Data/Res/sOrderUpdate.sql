-- SetupConfig: {}
create proc ITIH.sOrderUpdate (
	@ActorId int,
	@OrderId int,
	@HasBeenEntirelyDelivered bit,
	@Result bit output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @initialDeliverStatus int;
	declare @changedDeliverStatus int;

	set @initialDeliverStatus = 
		(select HasBeenEntirelyDelivered from tOrder where OrderId = @OrderId);

	update tOrder set HasBeenEntirelyDelivered = @HasBeenEntirelyDelivered;

	set @initialDeliverStatus = 
		(select HasBeenEntirelyDelivered from tOrder where OrderId = @OrderId);

	if (@initialDeliverStatus != @changedDeliverStatus)
		set @Result = 1;
	else
		set @Result = 0;

	--<PostCreate />

	--[endsp]
end;