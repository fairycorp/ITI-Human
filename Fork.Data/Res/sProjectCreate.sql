--SetupConfig: {}
create proc ITIH.sProjectCreate (
	@ActorId int,
	@TypeId int,
	@SemesterId int,
	@Name nvarchar(256),
	@Headline nvarchar(256),
	@Pitch nvarchar(512),
	@ProjectIdResult int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into ITIH.tProject (TypeId, SemesterId, [Name], Headline, Pitch)
		values (@TypeId, @SemesterId, @Name, @Headline, @Pitch);

	set @ProjectIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;