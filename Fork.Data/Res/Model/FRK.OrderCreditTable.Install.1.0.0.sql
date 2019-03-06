create table FRK.tOrderCredit (
	OrderCreditId int not null identity(0, 1),
	ProjectId int not null,
	UserId int not null,
	Amount int not null,
	CreditTime datetime2 default sysutcdatetime()

	constraint PK_tOrderCredit_OrderCreditId primary key (OrderCreditId),
	constraint FK_tOrderCredit_ProjectId foreign key (ProjectId) references FRK.tProject (ProjectId),
	constraint FK_tOrderCredit_UserId foreign key (UserId) references CK.tUser (UserId)
);

insert into FRK.tOrderCredit (ProjectId, UserId, Amount)
	values (0, 0, 0);