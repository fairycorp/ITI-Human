--SetupConfig:{}
create proc FRK.sUserBalanceCreate (
	@ActorId int,
	@UserId int,
	@ProjectId int,
	@UserBalanceId int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into FRK.tUserBalance (UserId, ProjectId) values (@UserId, @ProjectId);

	set @UserBalanceId = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;