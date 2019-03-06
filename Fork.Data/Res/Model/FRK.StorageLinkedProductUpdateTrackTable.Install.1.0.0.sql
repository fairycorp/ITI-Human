create table FRK.tStorageLinkedProductUpdateTrack (
	SLPUpdateTrackId int not null identity(0, 1),
	UpdateTrackId int not null,
	StorageLinkedProductId int not null

	constraint PK_tSLPUpdateTrack_SLPUpdateTrackId primary key (SLPUpdateTrackId),
	constraint FK_tSLPUpdateTrack_UpdateTrackId foreign key (UpdateTrackId) references FRK.tUpdateTrack (UpdateTrackId),
	constraint FK_tSLPUpdateTrack_StorageLinkedProductId foreign key (StorageLinkedProductId) references FRK.tStorageLinkedProduct (StorageLinkedProductId)
);

insert into FRK.tStorageLinkedProductUpdateTrack (UpdateTrackId, StorageLinkedProductId)
	values(0, 0);