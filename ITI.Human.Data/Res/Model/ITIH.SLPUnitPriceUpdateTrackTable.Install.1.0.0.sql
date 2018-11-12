create table ITIH.tSLPUnitPriceUpdateTrack (
	SLPUnitPriceUpdateTrackId int not null identity(0, 1),
	SLPUpdateTrackId int not null,
	PreviousUnitPrice float not null,
	NewUnitPrice float not null

	constraint PK_tSLPUnitPriceUpdateTrack_SLPUnitPriceUpdateTrackId primary key (SLPUnitPriceUpdateTrackId),
	constraint FK_tSLPUnitPriceUpdateTrack_SLPUpdateTrackId foreign key (SLPUpdateTrackId) references ITIH.tStorageLinkedProductUpdateTrack (SLPUpdateTrackId)
);

insert into ITIH.tSLPUnitPriceUpdateTrack (SLPUpdateTrackId, PreviousUnitPrice, NewUnitPrice)
	values(0, 0, 0);