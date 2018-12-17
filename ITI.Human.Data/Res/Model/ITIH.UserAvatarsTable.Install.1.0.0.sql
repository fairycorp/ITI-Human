create table ITIH.tUserAvatars (
	UserAvatarId int not null identity(0, 1),
	UserId int not null,
	[Image] varbinary(8000) not null

	constraint PK_tUserAvatars_UserAvatarId primary key (UserAvatarId),
	constraint FK_tUserAvatars_UserId foreign key (UserId) references CK.tUser
);

insert into ITIH.tUserAvatars (UserId, [Image]) values (0, 0x);