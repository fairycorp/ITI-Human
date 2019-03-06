create table FRK.tUserDetails (
	UserDetailsId int not null identity(0, 1),
	UserId int not null,
	FirstName nvarchar(128) not null default N'',
	LastName nvarchar(128) not null default N'',
	BirthDate datetime2 not null default '00010101'

	constraint PK_tUserDetails_UserDetailsId primary key (UserDetailsId),
	constraint FK_tUserDetails_UserId foreign key (UserId) references CK.tUser (UserId)
);

insert into FRK.tUserDetails (UserId) values (0);
