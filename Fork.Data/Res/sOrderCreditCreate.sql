-- SetupConfig: {}
create proc FRK.sOrderCreditCreate (
	@ActorId int,
	@ProjectId int,
	@UserId int,
	@Amount int,
	@CreditTime datetime2,
	@OrderCreditId int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into FRK.tOrderCredit (ProjectId, UserId, Amount, CreditTime)
		values (@ProjectId, @UserId, @Amount, @CreditTime);

	set @OrderCreditId = scope_identity();

	--<PostCreate />

	--[endsp]
end;