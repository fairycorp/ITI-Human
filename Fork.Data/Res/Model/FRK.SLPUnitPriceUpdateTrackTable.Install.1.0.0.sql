create table FRK.tSLPUnitPriceUpdateTrack (
	SLPUnitPriceUpdateTrackId int not null identity(0, 1),
	SLPUpdateTrackId int not null,
	PreviousUnitPrice int not null,
	NewUnitPrice int not null

	constraint PK_tSLPUnitPriceUpdateTrack_SLPUnitPriceUpdateTrackId primary key (SLPUnitPriceUpdateTrackId),
	constraint FK_tSLPUnitPriceUpdateTrack_SLPUpdateTrackId foreign key (SLPUpdateTrackId) references FRK.tStorageLinkedProductUpdateTrack (SLPUpdateTrackId)
);

insert into FRK.tSLPUnitPriceUpdateTrack (SLPUpdateTrackId, PreviousUnitPrice, NewUnitPrice)
	values(0, 0, 0);