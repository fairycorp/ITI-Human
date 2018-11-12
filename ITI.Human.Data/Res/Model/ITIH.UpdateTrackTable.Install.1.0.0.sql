create table ITIH.tUpdateTrack (
	UpdateTrackId int not null identity(0, 1),
	ActorId int not null,
	UpdateDate datetime2 not null

	constraint PK_tUpdateTrack_UpdateTrackId primary key (UpdateTrackId),
	constraint FK_tUpdateTrack_ActorId foreign key (ActorId) references CK.tActor (ActorId)
);

insert into ITIH.tUpdateTrack (ActorId, UpdateDate)
	values(0, sysutcdatetime());