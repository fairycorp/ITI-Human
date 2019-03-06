create table ITIH.tSLPStockUpdateTrack (
	SLPStockUpdateTrackId int not null identity(0, 1),
	SLPUpdateTrackId int not null,
	PreviousStock int not null,
	NewStock int not null

	constraint PK_tSLPStockUpdateTrack_SLPStockUpdateTrackId primary key (SLPStockUpdateTrackId),
	constraint FK_tSLPStockUpdateTrack_SLPUpdateTrackId foreign key (SLPUpdateTrackId) references ITIH.tStorageLinkedProductUpdateTrack (SLPUpdateTrackId)
);

insert into ITIH.tSLPStockUpdateTrack (SLPUpdateTrackId, PreviousStock, NewStock)
	values(0, 0, 0);