USE [repaem]
GO
SET IDENTITY_INSERT [dbo].[BlackList] ON 

INSERT [dbo].[BlackList] ([Id], [ClientId], [Comment], [PhoneNumber], [ManagerId]) VALUES (3, 3, N'Не пришел на репетицию', N'0956956757', N'1')
INSERT [dbo].[BlackList] ([Id], [ClientId], [Comment], [PhoneNumber], [ManagerId]) VALUES (4, 6, N'Порвали барабан', N'0503080414', N'2')
SET IDENTITY_INSERT [dbo].[BlackList] OFF
SET IDENTITY_INSERT [dbo].[Cities] ON 

INSERT [dbo].[Cities] ([Id], [Name]) VALUES (1, N'Киев')
INSERT [dbo].[Cities] ([Id], [Name]) VALUES (2, N'Кременчуг')
SET IDENTITY_INSERT [dbo].[Cities] OFF
SET IDENTITY_INSERT [dbo].[Comments] ON 

INSERT [dbo].[Comments] ([Id], [ClientId], [Text], [Rating], [RepBaseId]) VALUES (1, NULL, N'Отличная база!', NULL, N'1')
INSERT [dbo].[Comments] ([Id], [ClientId], [Text], [Rating], [RepBaseId]) VALUES (2, 6, N'That is bad for me', 2, N'1')
INSERT [dbo].[Comments] ([Id], [ClientId], [Text], [Rating], [RepBaseId]) VALUES (3, 1, N'Ничего так!', 5, N'1')
INSERT [dbo].[Comments] ([Id], [ClientId], [Text], [Rating], [RepBaseId]) VALUES (4, 3, N'Фуфло полное', 1, N'3')
INSERT [dbo].[Comments] ([Id], [ClientId], [Text], [Rating], [RepBaseId]) VALUES (5, 6, N'Looks great!', 4.5, N'3')
INSERT [dbo].[Comments] ([Id], [ClientId], [Text], [Rating], [RepBaseId]) VALUES (6, 1, N'Отстой', 2, N'3')
SET IDENTITY_INSERT [dbo].[Comments] OFF
SET IDENTITY_INSERT [dbo].[Distincts] ON 

INSERT [dbo].[Distincts] ([Id], [Name], [CityId]) VALUES (1, N'Дарница', 1)
INSERT [dbo].[Distincts] ([Id], [Name], [CityId]) VALUES (2, N'Майдан', 1)
INSERT [dbo].[Distincts] ([Id], [Name], [CityId]) VALUES (3, N'Автозаводской', 2)
INSERT [dbo].[Distincts] ([Id], [Name], [CityId]) VALUES (4, N'Пивзавод', 2)
SET IDENTITY_INSERT [dbo].[Distincts] OFF
SET IDENTITY_INSERT [dbo].[Invoices] ON 

INSERT [dbo].[Invoices] ([Id], [Sum], [Status], [ManagerId], [Date]) VALUES (2, 200, 1, N'1', CAST(0x0000A1B500000000 AS DateTime))
INSERT [dbo].[Invoices] ([Id], [Sum], [Status], [ManagerId], [Date]) VALUES (4, 200, 0, N'1', CAST(0x0000A1E100000000 AS DateTime))
INSERT [dbo].[Invoices] ([Id], [Sum], [Status], [ManagerId], [Date]) VALUES (6, 100, 1, N'2', CAST(0x0000A1B500000000 AS DateTime))
INSERT [dbo].[Invoices] ([Id], [Sum], [Status], [ManagerId], [Date]) VALUES (7, 50, 1, N'2', CAST(0x0000A13900000000 AS DateTime))
SET IDENTITY_INSERT [dbo].[Invoices] OFF
SET IDENTITY_INSERT [dbo].[Managers] ON 

INSERT [dbo].[Managers] ([Id], [Name], [CityId], [Email], [PhoneNumber], [Password]) VALUES (1, N'Вася', 1, N'tsdaemon@gmail.com', N'+380956956757', N'8d34d02b-3a46-0803-9d5b-6b64a5dc0f06')
INSERT [dbo].[Managers] ([Id], [Name], [CityId], [Email], [PhoneNumber], [Password]) VALUES (3, N'Петя', 2, N'tsdaemon@gmail.com', N'+380503080414', N'8d34d02b-3a46-0803-9d5b-6b64a5dc0f06')
SET IDENTITY_INSERT [dbo].[Managers] OFF
SET IDENTITY_INSERT [dbo].[Musicians] ON 

INSERT [dbo].[Musicians] ([Id], [Name], [CityId], [Email], [PhoneNumber], [Password], [BandName]) VALUES (1, N'Анатолий Стегний', 1, N'tsdaemon@gmail.com', N'+380956956757', N'8d34d02b-3a46-0803-9d5b-6b64a5dc0f06', N'Час Ночи')
INSERT [dbo].[Musicians] ([Id], [Name], [CityId], [Email], [PhoneNumber], [Password], [BandName]) VALUES (3, N'Вася Пупкин', 2, N'vasya@vasya.com', N'+380675664534', N'8d34d02b-3a46-0803-9d5b-6b64a5dc0f06', N'Ария')
INSERT [dbo].[Musicians] ([Id], [Name], [CityId], [Email], [PhoneNumber], [Password], [BandName]) VALUES (6, N'Ringo Star', 1, N'ringo@beatles.com', N'+456789403312', N'8d34d02b-3a46-0803-9d5b-6b64a5dc0f06', N'The Beatles')
SET IDENTITY_INSERT [dbo].[Musicians] OFF
SET IDENTITY_INSERT [dbo].[RepBases] ON 

INSERT [dbo].[RepBases] ([Id], [Name], [CityId], [Address], [Coordinates], [DistinctId], [ManagerId], [CreationDate], [Description]) VALUES (1, N'Пьяный матрос', 1, N'Металистов, 5', N'+380956956757', 1, N'1', CAST(0x0000A1E100000000 AS DateTime), NULL)
INSERT [dbo].[RepBases] ([Id], [Name], [CityId], [Address], [Coordinates], [DistinctId], [ManagerId], [CreationDate], [Description]) VALUES (3, N'Трезвый матрос', 2, N'Ковальова, 47', N'+380956956757', 1, N'2', CAST(0x0000A1E100000000 AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[RepBases] OFF
SET IDENTITY_INSERT [dbo].[Rooms] ON 

INSERT [dbo].[Rooms] ([Id], [RepBaseId], [Price], [Description]) VALUES (1, 1, 20, N'Йойойо самая лучшая комната в мире!')
INSERT [dbo].[Rooms] ([Id], [RepBaseId], [Price], [Description]) VALUES (2, 3, 40, N'Гитарный стек №1 - Mesa Boogie Rectifier Solo Head + Cabinet BlackStar Blackfire (4*12, Celestion Vintage 30)
Гитарный стек №2 - Peavey 5150 EVH (block letter) + Cabinet Peavey 5150 (4*12, Peavey Sheffield 1200)
Бас-гитарный стек - Ampeg SVT-3PRO + Ampeg SVT-410HE
Ударная установка - Sonor Select Force Series SEF Stage 3 Set 11237 (Autumn Fade) + набор тарелок Meinl MCS-3 (14",16",20")
Вокальная линия - Yamaha MSR-400 + микшерный пульт Yamaha MG-82CX
Микрофоны - Audio-Technica PRO31
+ коммутация, клавишная стойка, гитарные стойки, микрофонные стойки, микрофон для бас-бочки ')
INSERT [dbo].[Rooms] ([Id], [RepBaseId], [Price], [Description]) VALUES (3, 1, 30, N'Гитарный стек №1 - Mesa Boogie Rectifier Solo Head + Cabinet BlackStar Blackfire (4*12, Celestion Vintage 30)
Гитарный стек №2 - Peavey 5150 EVH (block letter) + Cabinet Peavey 5150 (4*12, Peavey Sheffield 1200)
Бас-гитарный стек - Ampeg SVT-3PRO + Ampeg SVT-410HE
Ударная установка - Sonor Select Force Series SEF Stage 3 Set 11237 (Autumn Fade) + набор тарелок Meinl MCS-3 (14",16",20")
Вокальная линия - Yamaha MSR-400 + микшерный пульт Yamaha MG-82CX
Микрофоны - Audio-Technica PRO31
+ коммутация, клавишная стойка, гитарные стойки, микрофонные стойки, микрофон для бас-бочки ')
INSERT [dbo].[Rooms] ([Id], [RepBaseId], [Price], [Description]) VALUES (4, 3, NULL, N'Барабанная установка: Yamaha Rydeen
Тарелки:" Zildjian"/ "Solar by Sabian" Crash ("Solar") 16, 16 Crash; 20 Ride;14 Hi-Hat.
Усиление для бас гитары: Ibanez SW 65, динамик 12"( 65 Ватт ) + звуковая линия.
Усиление для гитар:
Ibanez TBx 150R ( 2 x 12" 150 Ватт )
Vox AD 50 Vt ( 2 x 10" 50 Ватт )
Звук:
Топы: "EVM Cs2152" ( 2x400 Ватт/4Ом )
Усилитель: Park Audio Vx700/ 4Ом ( 700 Ватт ),
Мониторы: "Soundking" динамик 8.5" ( 2x200 Ватт/4Ом )
Активный микшерный пульт:
"Soundking Skae101ee" ( 2x200 Bатт, 4 Ом, 4 микрофонных +2 стереопары + обработка DigRev,
7-ми полосный эквалайзер на каждый монитор) Микшерный пульт: Yamaha Mg 12/4Fx ( 12 канальный пульт с процессором эффектов )
Микрофон: Shure Sm58, Shure C606
Стойки для гитар:" Hercules mini" 2шт, "Rockstand" 2шт.
Стойки под микрофон : "Rockstand" 2шт.
Стационарный компьютер для работы с Audio/ Video, Midi файлами. ')
SET IDENTITY_INSERT [dbo].[Rooms] OFF
