--SetupConfig: {}
create proc ITIH.sProductLinkedProjectCreate (
	@ActorId int,
	@ProductId int,
	@ProjectId int,
	@Availability bit = 1,
	@ProductLinkedProjectIdResult int output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	insert into ITIH.tProductLinkedProject (ProductId, ProjectId, [Availability])
		values (@ProductId, @ProjectId, @Availability);

	set @ProductLinkedProjectIdResult = scope_identity();

	--<PostCreate />
	
	--[endsp]
end;