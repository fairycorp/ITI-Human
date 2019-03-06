--SetupConfig: {}
create proc ITIH.sProjectTypeCreate (
	@ActorId int,
	@Name nvarchar(256),
	@ProjectTypeIdResult int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into ITIH.tProjectType ([Name]) values (@Name);

	set @ProjectTypeIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;