create table ITIH.tOPCurrentStateUpdateTrack (
	OPCurrentStateUpdateTrackId int not null identity(0, 1),
	OPUpdateTrackId int not null,
	PreviousState int not null,
	NewState int not null

	constraint PK_tOPCurrentStateUpdateTrack_OPCurrentStateUpdateTrackId primary key (OPCurrentStateUpdateTrackId),
	constraint FK_tOPCurrentStateUpdateTrack_OPUpdateTrackId foreign key (OPUpdateTrackId) references ITIH.tOrderedProductUpdateTrack (OPUpdateTrackId)
);

insert into ITIH.tOPCurrentStateUpdateTrack (OPUpdateTrackId, PreviousState, NewState)
	values(0, 0, 0);