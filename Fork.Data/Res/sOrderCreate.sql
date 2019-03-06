-- SetupConfig: {}
create proc FRK.sOrderCreate (
	@ActorId int,
	@StorageId int,
	@UserId int,
	@ClassroomId int,
	@CreationDate datetime2,
	@OrderIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into FRK.tOrder (StorageId, UserId, ClassroomId, CreationDate)
		values (@StorageId, @UserId, @ClassroomId, @CreationDate);

	set @OrderIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;