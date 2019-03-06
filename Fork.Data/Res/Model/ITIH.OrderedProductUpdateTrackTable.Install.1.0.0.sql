create table ITIH.tOrderedProductUpdateTrack (
	OPUpdateTrackId int not null identity(0, 1),
	UpdateTrackId int not null,
	OrderedProductId int not null

	constraint PK_tOrderedProductUpdateTrack_OPUpdateTrackId primary key (OPUpdateTrackId),
	constraint FK_tOrderedProductUpdateTrack_UpdateTrackId foreign key (UpdateTrackId) references ITIH.tUpdateTrack (UpdateTrackId),
	constraint FK_tOrderedProductUpdateTrack_OrderedProductId foreign key (OrderedProductId) references ITIH.tOrderedProduct (OrderedProductId)
);

insert into ITIH.tOrderedProductUpdateTrack (UpdateTrackId, OrderedProductId)
	values(0, 0);