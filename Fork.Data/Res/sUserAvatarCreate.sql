-- SetupConfig: {}
create proc FRK.sUserAvatarCreate (
	@ActorId int,
	@UserId int,
	@Url nvarchar(max),
	@UserAvatarIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into FRK.tUserAvatars (UserId, [Url])
		values (@UserId, @Url);

	set @UserAvatarIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;