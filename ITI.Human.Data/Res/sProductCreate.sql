--SetupConfig: {}
create proc ITIH.sProductCreate (
	@ActorId int,
	@Name nvarchar(256),
	@Desc nvarchar(512),
	@Price int,
	@ProductIdResult int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into ITIH.tProduct ([Name], [Desc], Price)
		values (@Name, @Desc, @Price);

	set @ProductIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;