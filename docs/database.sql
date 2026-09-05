USE [RaceDayDB]

/****** Table: Category ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Category](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [varchar](50) NOT NULL,
	[Description] [varchar](255) NULL,
PRIMARY KEY CLUSTERED ([CategoryId] ASC)
) ON [PRIMARY]

/****** Table: Enrolment ******/
CREATE TABLE [dbo].[Enrolment](
	[EnrolmentId] [int] IDENTITY(1,1) NOT NULL,
	[ParticipantId] [int] NOT NULL,
	[EventCategoryId] [int] NOT NULL,
	[EnrolmentDate] [date] NOT NULL,
	[EnrolmentStatus] [varchar](40) NOT NULL,
PRIMARY KEY CLUSTERED ([EnrolmentId] ASC)
) ON [PRIMARY]

/****** Table: Event ******/
CREATE TABLE [dbo].[Event](
	[EventId] [int] IDENTITY(1,1) NOT NULL,
	[OrganiserId] [int] NOT NULL,
	[EventName] [varchar](100) NOT NULL,
	[Description] [varchar](500) NULL,
	[EventDate] [date] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[Location] [varchar](160) NOT NULL,
	[Status] [varchar](30) NOT NULL,
PRIMARY KEY CLUSTERED ([EventId] ASC)
) ON [PRIMARY]

/****** Table: EventCategory ******/
CREATE TABLE [dbo].[EventCategory](
	[EventCategoryId] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[EntryFee] [decimal](10, 5) NOT NULL,
	[MaximumParticipants] [int] NOT NULL,
PRIMARY KEY CLUSTERED ([EventCategoryId] ASC)
) ON [PRIMARY]

/****** Table: Result ******/
CREATE TABLE [dbo].[Result](
	[ResultId] [int] IDENTITY(1,1) NOT NULL,
	[EnrolmentId] [int] NOT NULL,
	[FinishTime] [time](7) NULL,
	[Position] [int] NULL,
	[ResultStatus] [varchar](30) NULL,
	[RecordDate] [date] NULL,
PRIMARY KEY CLUSTERED ([ResultId] ASC)
) ON [PRIMARY]

/****** Table: Route ******/
CREATE TABLE [dbo].[Route](
	[RouteId] [int] IDENTITY(1,1) NOT NULL,
	[EventId] [int] NOT NULL,
	[DistanceKM] [decimal](6, 2) NOT NULL,
	[RouteDescription] [varchar](500) NULL,
	[RouteMapURL] [varchar](500) NULL,
PRIMARY KEY CLUSTERED ([RouteId] ASC)
) ON [PRIMARY]

/****** Table: User ******/
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[PasswordHash] [varchar](255) NOT NULL,
	[Role] [varchar](20) NOT NULL,
	[PhoneNumber] [varchar](20) NULL,
PRIMARY KEY CLUSTERED ([UserId] ASC)
) ON [PRIMARY]

-- ================= SAMPLE DATA =================

SET IDENTITY_INSERT [dbo].[Category] ON
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description]) VALUES (1, N'Running', N'Road running events')
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description]) VALUES (2, N'Walking', N'Community walking events')
INSERT [dbo].[Category] ([CategoryId], [CategoryName], [Description]) VALUES (3, N'Cycling', N'Road cycling events')
SET IDENTITY_INSERT [dbo].[Category] OFF

SET IDENTITY_INSERT [dbo].[User] ON
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [PasswordHash], [Role], [PhoneNumber]) VALUES (1, N'Thabang', N'Mametsa', N'thabang@raceday.co.za', N'hashed_password_1', N'Organiser', N'0715689903')
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [PasswordHash], [Role], [PhoneNumber]) VALUES (2, N'Lerato', N'Motsepe', N'lerato@raceday.co.za', N'hashed_password_2', N'Organiser', N'0768059920')
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [PasswordHash], [Role], [PhoneNumber]) VALUES (3, N'Kabelo', N'Mashiane', N'kabelo@gmail.com', N'hashed_password_3', N'Participant', N'0701711985')
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [PasswordHash], [Role], [PhoneNumber]) VALUES (4, N'Naledi', N'Mokoena', N'naledi@gmail.com', N'hashed_password_4', N'Participant', N'0714319970')
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [PasswordHash], [Role], [PhoneNumber]) VALUES (5, N'Sipho', N'Dlamini', N'sipho@gmail.com', N'hashed_password_5', N'Participant', N'0834659732')
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [PasswordHash], [Role], [PhoneNumber]) VALUES (6, N'Palesa', N'Ndlovu', N'palesa@gmail.com', N'hashed_password_6', N'Participant', N'0761089291')
INSERT [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [PasswordHash], [Role], [PhoneNumber]) VALUES (7, N'Tshepo', N'Molefe', N'tshepo@gmail.com', N'hashed_password_7', N'Participant', N'0839791018')
SET IDENTITY_INSERT [dbo].[User] OFF

SET IDENTITY_INSERT [dbo].[Event] ON
INSERT [dbo].[Event] ([EventId], [OrganiserId], [EventName], [Description], [EventDate], [StartTime], [Location], [Status]) VALUES (2, 1, N'Johannesburg City Run', N'Annual road running event', CAST(N'2026-10-10' AS Date), CAST(N'07:00:00' AS Time), N'Johannesburg', N'Upcoming')
INSERT [dbo].[Event] ([EventId], [OrganiserId], [EventName], [Description], [EventDate], [StartTime], [Location], [Status]) VALUES (3, 1, N'Soweto Community Walk', N'Community health and fitness walk', CAST(N'2026-10-17' AS Date), CAST(N'08:00:00' AS Time), N'Soweto', N'Upcoming')
INSERT [dbo].[Event] ([EventId], [OrganiserId], [EventName], [Description], [EventDate], [StartTime], [Location], [Status]) VALUES (4, 2, N'Cape Town Cycle Challenge', N'Road cycling challenge', CAST(N'2026-11-07' AS Date), CAST(N'06:30:00' AS Time), N'Cape Town', N'Upcoming')
SET IDENTITY_INSERT [dbo].[Event] OFF

SET IDENTITY_INSERT [dbo].[EventCategory] ON
INSERT [dbo].[EventCategory] ([EventCategoryId], [EventId], [CategoryId], [EntryFee], [MaximumParticipants]) VALUES (1, 2, 1, 150.00000, 200)
INSERT [dbo].[EventCategory] ([EventCategoryId], [EventId], [CategoryId], [EntryFee], [MaximumParticipants]) VALUES (2, 3, 2, 50.00000, 300)
INSERT [dbo].[EventCategory] ([EventCategoryId], [EventId], [CategoryId], [EntryFee], [MaximumParticipants]) VALUES (3, 4, 3, 250.00000, 150)
SET IDENTITY_INSERT [dbo].[EventCategory] OFF

SET IDENTITY_INSERT [dbo].[Enrolment] ON
INSERT [dbo].[Enrolment] ([EnrolmentId], [ParticipantId], [EventCategoryId], [EnrolmentDate], [EnrolmentStatus]) VALUES (1, 3, 1, CAST(N'2026-09-01' AS Date), N'Confirmed')
INSERT [dbo].[Enrolment] ([EnrolmentId], [ParticipantId], [EventCategoryId], [EnrolmentDate], [EnrolmentStatus]) VALUES (2, 4, 2, CAST(N'2026-09-02' AS Date), N'Confirmed')
SET IDENTITY_INSERT [dbo].[Enrolment] OFF

-- ================= CONSTRAINTS =================

SET ANSI_PADDING ON
ALTER TABLE [dbo].[User] ADD UNIQUE NONCLUSTERED ([Email] ASC)

ALTER TABLE [dbo].[Enrolment]  WITH CHECK ADD FOREIGN KEY([EventCategoryId])
REFERENCES [dbo].[EventCategory] ([EventCategoryId])
ALTER TABLE [dbo].[Enrolment]  WITH CHECK ADD FOREIGN KEY([ParticipantId])
REFERENCES [dbo].[User] ([UserId])
ALTER TABLE [dbo].[Event]  WITH CHECK ADD FOREIGN KEY([OrganiserId])
REFERENCES [dbo].[User] ([UserId])
ALTER TABLE [dbo].[EventCategory]  WITH CHECK ADD FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Category] ([CategoryId])
ALTER TABLE [dbo].[EventCategory]  WITH CHECK ADD FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([EventId])
ALTER TABLE [dbo].[Result]  WITH CHECK ADD FOREIGN KEY([EnrolmentId])
REFERENCES [dbo].[Enrolment] ([EnrolmentId])
ALTER TABLE [dbo].[Route]  WITH CHECK ADD FOREIGN KEY([EventId])
REFERENCES [dbo].[Event] ([EventId])
