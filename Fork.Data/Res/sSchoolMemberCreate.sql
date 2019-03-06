-- SetupConfig: {}
create proc ITIH.sSchoolMemberCreate (
	@ActorId int,
	@UserId int,
	@SchoolStatusId int,
	@SchoolMemberIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into ITIH.tSchoolMember (UserId, SchoolStatusId)
		values (@UserId, @SchoolStatusId);

	set @SchoolMemberIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;