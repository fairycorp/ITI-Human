--SetupConfig:{}
create proc ITIH.sUserBalanceCreate (
	@ActorId int,
	@UserId int,
	@ProjectId int,
	@UserBalanceId int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into ITIH.tUserBalance (UserId, ProjectId) values (@UserId, @ProjectId);

	set @UserBalanceId = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;