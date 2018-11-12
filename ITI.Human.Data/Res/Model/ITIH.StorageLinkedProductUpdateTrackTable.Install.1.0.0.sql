create table ITIH.tStorageLinkedProductUpdateTrack (
	SLPUpdateTrackId int not null identity(0, 1),
	UpdateTrackId int not null,
	StorageLinkedProductId int not null

	constraint PK_tSLPUpdateTrack_SLPUpdateTrackId primary key (SLPUpdateTrackId),
	constraint FK_tSLPUpdateTrack_UpdateTrackId foreign key (UpdateTrackId) references ITIH.tUpdateTrack (UpdateTrackId),
	constraint FK_tSLPUpdateTrack_StorageLinkedProductId foreign key (StorageLinkedProductId) references ITIH.tStorageLinkedProduct (StorageLinkedProductId)
);

insert into ITIH.tStorageLinkedProductUpdateTrack (UpdateTrackId, StorageLinkedProductId)
	values(0, 0);