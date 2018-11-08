-- SetupConfig: {}
create proc ITIH.sOrderUpdate (
	@ActorId int,
	@OrderId int,
	@CurrentState int,
	@CurrentStateResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	update ITIH.tOrder set CurrentState = @CurrentState;

	set @CurrentStateResult = (select CurrentState from ITIH.tOrder where OrderId = @OrderId);

	--<PostCreate />

	--[endsp]
end;