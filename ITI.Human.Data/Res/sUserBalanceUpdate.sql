--SetupConfig:{}
create proc ITIH.sUserBalanceUpdate (
	@ActorId int,
	@UserBalanceId int,
	@Amount int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	declare @previousBalance nvarchar(512);
	declare @newBalance nvarchar(256);

	set @previousBalance = (select Balance from ITIH.tUserBalance where UserBalanceId = @UserBalanceId);
	update ITIH.tUserBalance set Balance = (@previousBalance + @Amount) where UserBalanceId = @UserBalanceId;
	set @newBalance = (select Balance from ITIH.tUserBalance where UserBalanceId = @UserBalanceId);

	if (@previousBalance != @newBalance)
		set @Success = 1;

	--<PostCreate />
	
	--[endsp]
end;