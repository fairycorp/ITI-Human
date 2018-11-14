-- SetupConfig: {}
create proc ITIH.sOrderFinalDueCreate (
	@ActorId int,
	@OrderId int,
	@Total float,
	@Paid float,
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