-- SetupConfig: {}
create proc ITIH.sOrderUpdate (
	@ActorId int,
	@OrderId int,
	@CurrentState int,
	@HasBeenEntirelyDelivered bit,
	@DeliveryResult bit output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	update tOrder set CurrentState = @CurrentState;
	update tOrder set HasBeenEntirelyDelivered = @HasBeenEntirelyDelivered;

	set @DeliveryResult = (select HasBeenEntirelyDelivered from tOrder where OrderId = @OrderId);

	--<PostCreate />

	--[endsp]
end;