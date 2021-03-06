
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 09/13/2013 21:39:59
-- Generated from EDMX file: C:\Users\tsdaemon\Documents\Visual Studio 2010\Projects\aspdev.repaem\repaem.in.ua\repaem.in.ua\repaem.in.ua\Models\Data\BaseDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [db00757cc4ea1a4c4fbaada1f700fed8fd];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Cities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cities];
GO
IF OBJECT_ID(N'[dbo].[RepBases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RepBases];
GO
IF OBJECT_ID(N'[dbo].[BlackLists]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlackLists];
GO
IF OBJECT_ID(N'[dbo].[Repetitions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Repetitions];
GO
IF OBJECT_ID(N'[dbo].[Rooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rooms];
GO
IF OBJECT_ID(N'[dbo].[Prices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Prices];
GO
IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO
IF OBJECT_ID(N'[dbo].[Photos]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Photos];
GO
IF OBJECT_ID(N'[dbo].[PhotoToRoom]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhotoToRoom];
GO
IF OBJECT_ID(N'[dbo].[PhotoToRepBase]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PhotoToRepBase];
GO
IF OBJECT_ID(N'[dbo].[Invoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Invoices];
GO
IF OBJECT_ID(N'[dbo].[UserSessions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSessions];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NULL,
    [CityId] int  NOT NULL,
    [Email] nvarchar(128)  NOT NULL,
    [PhoneNumber] nvarchar(16)  NOT NULL,
    [Password] uniqueidentifier  NOT NULL,
    [BandName] nvarchar(256)  NULL,
    [Role] nvarchar(max)  NOT NULL,
    [PhoneChecked] bit  NOT NULL
);
GO

-- Creating table 'Cities'
CREATE TABLE [dbo].[Cities] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'RepBases'
CREATE TABLE [dbo].[RepBases] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CityId] int  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [ManagerId] int  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Lat] float  NOT NULL,
    [Long] float  NOT NULL
);
GO

-- Creating table 'BlackLists'
CREATE TABLE [dbo].[BlackLists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientId] int  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [ManagerId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Repetitions'
CREATE TABLE [dbo].[Repetitions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TimeStart] int  NOT NULL,
    [MusicianId] int  NOT NULL,
    [Sum] int  NOT NULL,
    [RepBaseId] int  NOT NULL,
    [RoomId] int  NOT NULL,
    [TimeEnd] int  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [Status] int  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'Rooms'
CREATE TABLE [dbo].[Rooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RepBaseId] int  NOT NULL,
    [Price] int  NULL,
    [Description] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Prices'
CREATE TABLE [dbo].[Prices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StartTime] int  NOT NULL,
    [EndTime] int  NOT NULL,
    [Sum] float  NOT NULL,
    [RoomId] int  NOT NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] int  NULL,
    [Text] nvarchar(max)  NULL,
    [Rating] float  NULL,
    [RepBaseId] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Host] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Photos'
CREATE TABLE [dbo].[Photos] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ImageSrc] nvarchar(max)  NOT NULL,
    [ThumbnailSrc] nvarchar(max)  NULL,
    [IsLogo] bit  NOT NULL
);
GO

-- Creating table 'PhotoToRoom'
CREATE TABLE [dbo].[PhotoToRoom] (
    [PhotoId] int  NOT NULL,
    [RoomId] int  NOT NULL
);
GO

-- Creating table 'PhotoToRepBase'
CREATE TABLE [dbo].[PhotoToRepBase] (
    [PhotoId] int  NOT NULL,
    [RepBaseId] int  NOT NULL
);
GO

-- Creating table 'Invoices'
CREATE TABLE [dbo].[Invoices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Sum] int  NOT NULL,
    [Status] tinyint  NOT NULL,
    [UserId] int  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'UserSessions'
CREATE TABLE [dbo].[UserSessions] (
    [Id] uniqueidentifier  NOT NULL,
    [UserId] int  NOT NULL,
    [Expires] datetime  NOT NULL,
    [Host] nvarchar(max)  NOT NULL,
    [Cookie] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Cities'
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [PK_Cities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RepBases'
ALTER TABLE [dbo].[RepBases]
ADD CONSTRAINT [PK_RepBases]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'BlackLists'
ALTER TABLE [dbo].[BlackLists]
ADD CONSTRAINT [PK_BlackLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Repetitions'
ALTER TABLE [dbo].[Repetitions]
ADD CONSTRAINT [PK_Repetitions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Rooms'
ALTER TABLE [dbo].[Rooms]
ADD CONSTRAINT [PK_Rooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Prices'
ALTER TABLE [dbo].[Prices]
ADD CONSTRAINT [PK_Prices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Photos'
ALTER TABLE [dbo].[Photos]
ADD CONSTRAINT [PK_Photos]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [PhotoId], [RoomId] in table 'PhotoToRoom'
ALTER TABLE [dbo].[PhotoToRoom]
ADD CONSTRAINT [PK_PhotoToRoom]
    PRIMARY KEY CLUSTERED ([PhotoId], [RoomId] ASC);
GO

-- Creating primary key on [PhotoId], [RepBaseId] in table 'PhotoToRepBase'
ALTER TABLE [dbo].[PhotoToRepBase]
ADD CONSTRAINT [PK_PhotoToRepBase]
    PRIMARY KEY CLUSTERED ([PhotoId], [RepBaseId] ASC);
GO

-- Creating primary key on [Id] in table 'Invoices'
ALTER TABLE [dbo].[Invoices]
ADD CONSTRAINT [PK_Invoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSessions'
ALTER TABLE [dbo].[UserSessions]
ADD CONSTRAINT [PK_UserSessions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------