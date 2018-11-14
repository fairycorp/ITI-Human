-- SetupConfig: {}
create proc ITIH.sOrderFinalDueUpdate (
	@ActorId int,
	@OrderFinalDueId int,
	@Paid float,
	@PaymentTime datetime2,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	declare @previousPaid float;
	declare @newPaid float;

	set @previousPaid = (select Paid from ITIH.tOrderFinalDue where OrderFinalDueId = @OrderFinalDueId);
	update ITIH.tOrderFinalDue set Paid = (@previousPaid + @Paid) where OrderFinalDueId = @OrderFinalDueId;
	set @newPaid = (select Paid from ITIH.tOrderFinalDue where OrderFinalDueId = @OrderFinalDueId);

	if (@previousPaid != @newPaid)
		insert into ITIH.tOrderPayment (OrderFinalDueId, Amount) values (@OrderFinalDueId, @Paid);

	--<PostCreate />

	--[endsp]
end;