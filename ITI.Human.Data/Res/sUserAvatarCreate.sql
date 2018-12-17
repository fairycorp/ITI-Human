-- SetupConfig: {}
create proc ITIH.sUserAvatarCreate (
	@ActorId int,
	@UserId int,
	@Image varbinary(8000),
	@UserAvatarIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into ITIH.tUserAvatars (UserId, [Image])
		values (@UserId, @Image);

	set @UserAvatarIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;