--SetupConfig: {}
create proc ITIH.sProductUpdate (
	@ActorId int,
	@ProductId int,
	@Name nvarchar(256),
	@Desc nvarchar(512),
	@Success bit = 0 output
)
as
begin
	--[beginsp]
	
	--<PreCreate revert />

	declare @previousName nvarchar(256);
	declare @previousDesc nvarchar(512);
	declare @newName nvarchar(256);
	declare @newDesc nvarchar(512);

	set @previousName = (select [Name] from ITIH.tProduct where ProductId = @ProductId);
	set @previousDesc = (select [Desc] from ITIH.tProduct where ProductId = @ProductId);

	if (@Name = null)
		set @Name = @previousName;
		
	if (@Desc = null)
		set @Desc = @previousDesc;

	update ITIH.tProduct set [Name] = @Name where ProductId = @ProductId;
	update ITIH.tProduct set [Desc] = @Name where ProductId = @ProductId;

	set @newName = (select [Name] from ITIH.tProduct where ProductId = @ProductId);
	set @newDesc = (select [Desc] from ITIH.tProduct where ProductId = @ProductId);

	if (@Name = null)
		if (@newName != @previousName)
			set @Success = 0;
		else
			set @Success = 1;

	if (@Desc = null)
		if (@newDesc != @previousDesc)
			set @Success = 0;
		else
			set @Success = 1;

	--<PostCreate />
	
	--[endsp]
end;