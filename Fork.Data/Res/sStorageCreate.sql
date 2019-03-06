--SetupConfig:{}
create proc FRK.sStorageCreate (
	@ActorId int,
	@ProjectId int,
	@StorageIdResult int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into FRK.tStorage (ProjectId) values (@ProjectId);

	set @StorageIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;