create table ITIH.tUserAvatars (
	UserAvatarId int not null identity(0, 1),
	UserId int not null,
	[Url] varchar(max) not null

	constraint PK_tUserAvatars_UserAvatarId primary key (UserAvatarId),
	constraint FK_tUserAvatars_UserId foreign key (UserId) references CK.tUser (UserId)
);

insert into ITIH.tUserAvatars (UserId, [Url])
	values (0, 'https://image.noelshack.com/fichiers/2019/03/1/1547459724-unknown-user.png');