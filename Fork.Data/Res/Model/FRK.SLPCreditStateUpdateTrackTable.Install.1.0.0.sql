create table FRK.tSLPCreditStateUpdateTrack (
	SLPCreditStateUpdateTrackId int not null identity(0, 1),
	SLPUpdateTrackId int not null,
	PreviousCreditState bit not null,
	NewCreditState bit not null

	constraint PK_tSLPCreditStateUpdateTrack_SLPCreditStateUpdateTrackId primary key (SLPCreditStateUpdateTrackId),
	constraint FK_tSLPCreditStateUpdateTrack_SLPUpdateTrackId foreign key (SLPUpdateTrackId) references FRK.tStorageLinkedProductUpdateTrack (SLPUpdateTrackId)
);

insert into FRK.tSLPCreditStateUpdateTrack (SLPUpdateTrackId, PreviousCreditState, NewCreditState)
	values (0, 0, 0);