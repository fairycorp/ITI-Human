-- SetupConfig: {}
create proc FRK.sOrderFinalDueCreate (
	@ActorId int,
	@OrderId int,
	@Total int,
	@Paid int,
	@OrderFinalDueIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into FRK.tOrderFinalDue (OrderId, Total, Paid)
		values (@OrderId, @Total, @Paid);

	set @OrderFinalDueIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;