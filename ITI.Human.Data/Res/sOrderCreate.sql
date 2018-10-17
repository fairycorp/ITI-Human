-- SetupConfig: {}
create proc ITIH.sOrderCreate (
	@ActorId int,
	@UserId int,
	@CreationDate datetime2,
	@OrderIdResult int output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	insert into ITIH.tOrder (UserId, CreationDate)
		values (@UserId, @CreationDate);

	set @OrderIdResult = scope_identity();

	--<PostCreate />

	--[endsp]
end;