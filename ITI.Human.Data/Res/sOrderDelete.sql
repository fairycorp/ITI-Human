-- SetupConfig: {}
create proc ITIH.sOrderDelete (
	@ActorId int,
	@OrderId int,
	@Success bit = 0 output
)
as
begin
	--[beginsp]

	--<PreCreate revert />

	delete from tOrderedProduct where OrderId = @OrderId;
	delete from tOrder where OrderId = @OrderId;

	declare @DoesIdStillExist int;
	set @DoesIdStillExist = (select OrderId from tOrder where OrderId = @OrderId);

	if (@DoesIdStillExist is null)
		set @Success = 1;

	--<PostCreate />

	--[endsp]
end;