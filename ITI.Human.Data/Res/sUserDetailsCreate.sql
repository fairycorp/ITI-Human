-- SetupConfig: {}
create proc ITIH.sUserDetailsCreate (
	@ActorId int,
	@UserId int,
	@FirstName nvarchar(126),
	@LastName nvarchar(126),
	@BirthDate datetime2,
	@UserDetailsIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into ITIH.tUserDetails (UserId, FirstName, LastName, BirthDate)
		values (@UserId, @FirstName, @LastName, @BirthDate);

	set @UserDetailsIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;