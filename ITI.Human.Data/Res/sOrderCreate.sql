-- SetupConfig: {}
create proc ITIH.sOrderCreate (
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

	insert into ITIH.tOrder (StorageId, UserId, ClassroomId, CreationDate)
		values (@StorageId, @UserId, @ClassroomId, @CreationDate);

	set @OrderIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;