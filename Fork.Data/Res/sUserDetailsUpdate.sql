create proc FRK.sUserDetailsUpdate (
	@ActorId int,
	@UserId int,
	@FirstName nvarchar(128),
	@LastName nvarchar(128),
	@BirthDate datetime2,
	@Success bit output
)
as
begin
	UPDATE FRK.tUserDetails SET FirstName = @FirstName WHERE UserId = @UserId;
	UPDATE FRK.tUserDetails SET LastName = @LastName WHERE UserId = @UserId;
	UPDATE FRK.tUserDetails SET BirthDate = @BirthDate WHERE UserId = @UserId;

	set @Success = 1;
end;