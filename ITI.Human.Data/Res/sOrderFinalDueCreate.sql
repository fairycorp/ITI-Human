-- SetupConfig: {}
create proc ITIH.sOrderFinalDueCreate (
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

	insert into ITIH.tOrderFinalDue (OrderId, Total, Paid)
		values (@OrderId, @Total, @Paid);

	set @OrderFinalDueIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;