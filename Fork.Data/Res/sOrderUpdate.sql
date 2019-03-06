-- SetupConfig: {}
create proc ITIH.sOrderUpdate (
	@ActorId int,
	@OrderId int,
	@CurrentState int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @previousState int;
	declare @newState int;

	set @previousState = (select CurrentState from ITIH.tOrder where OrderId = @OrderId);
	update ITIH.tOrder set CurrentState = @CurrentState where OrderId = @OrderId;
	set @newState = (select CurrentState from ITIH.tOrder where OrderId = @OrderId);

	if (@previousState != @newstate)
		set @Success = 1;

	--<PostCreate />

	--[endsp]
end;