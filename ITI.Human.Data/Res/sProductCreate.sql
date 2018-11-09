--SetupConfig: {}
create proc ITIH.sProductCreate (
	@ActorId int,
	@Name nvarchar(256),
	@Desc nvarchar(512),
	@ProductIdResult int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into ITIH.tProduct ([Name], [Desc])
		values (@Name, @Desc);

	set @ProductIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;