create table ITIH.tOPPaymentStateUpdateTrack (
	OPPaymentStateUpdateTrackId int not null identity(0, 1),
	OPUpdateTrackId int not null,
	PreviousState int not null,
	NewState int not null

	constraint PK_tOPPaymentStateUpdateTrack_OPPaymentStateUpdateTrackId primary key (OPPaymentStateUpdateTrackId),
	constraint FK_tOPPaymentStateUpdateTrack_OPUpdateTrackId foreign key (OPUpdateTrackId) references ITIH.tOrderedProductUpdateTrack (OPUpdateTrackId)
);

insert into ITIH.tOPPaymentStateUpdateTrack (OPUpdateTrackId, PreviousState, NewState)
	values(0, 0, 0);