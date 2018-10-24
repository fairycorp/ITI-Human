-- SetupConfig: {}
create proc ITIH.sOrderUpdate (
	@ActorId int,
	@OrderId int,
	@HasBeenEntirelyDelivered bit,
	@CurrentStateResult bit output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	update tOrder set HasBeenEntirelyDelivered = @HasBeenEntirelyDelivered;

	set @CurrentStateResult = (select HasBeenEntirelyDelivered from tOrder where OrderId = @OrderId);

	--<PostCreate />

	--[endsp]
end;