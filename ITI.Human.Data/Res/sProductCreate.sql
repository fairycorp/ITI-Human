--SetupConfig: {}
create proc ITIH.sProductCreate (
	@ActorId int,
	@Name nvarchar(256),
	@Desc nvarchar(512),
	@Url nvarchar(max),
	@ProductIdResult int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into ITIH.tProduct ([Name], [Desc], [Url])
		values (@Name, @Desc, @Url);

	set @ProductIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;