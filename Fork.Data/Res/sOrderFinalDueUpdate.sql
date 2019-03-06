-- SetupConfig: {}
create proc FRK.sOrderFinalDueUpdate (
	@ActorId int,
	@OrderFinalDueId int,
	@Paid int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @previousPaid float;
	declare @newPaid float;

	set @previousPaid = (select Paid from FRK.tOrderFinalDue where OrderFinalDueId = @OrderFinalDueId);
	update FRK.tOrderFinalDue set Paid = (@previousPaid + @Paid) where OrderFinalDueId = @OrderFinalDueId;
	set @newPaid = (select Paid from FRK.tOrderFinalDue where OrderFinalDueId = @OrderFinalDueId);

	if (@previousPaid != @newPaid and @Paid > 0)
		set @Success = 1;

	--<PostCreate />

	--[endsp]
end;