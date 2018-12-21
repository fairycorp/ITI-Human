-- SetupConfig: {}
create proc ITIH.sUserAvatarCreate (
	@ActorId int,
	@UserId int,
	@Url nvarchar(max),
	@UserAvatarIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into ITIH.tUserAvatars (UserId, [Url])
		values (@UserId, @Url);

	set @UserAvatarIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;