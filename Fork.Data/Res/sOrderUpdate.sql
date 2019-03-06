-- SetupConfig: {}
create proc FRK.sOrderUpdate (
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

	set @previousState = (select CurrentState from FRK.tOrder where OrderId = @OrderId);
	update FRK.tOrder set CurrentState = @CurrentState where OrderId = @OrderId;
	set @newState = (select CurrentState from FRK.tOrder where OrderId = @OrderId);

	if (@previousState != @newstate)
		set @Success = 1;

	--<PostCreate />

	--[endsp]
end;