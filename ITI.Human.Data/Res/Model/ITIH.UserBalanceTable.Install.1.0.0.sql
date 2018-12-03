create table ITIH.tUserBalance (

	UserBalanceId int not null identity(0, 1),
	UserId int not null,
	Balance int not null default 0

	constraint PK_tUserBalance_UserBalanceId primary key (UserBalanceId),
	constraint FK_tUserBalance_UserId foreign key (UserId) references CK.tUser(UserId)
);

insert into ITIH.tUserBalance(UserId) values (0);