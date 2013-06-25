
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 06/25/2013 15:03:06
-- Generated from EDMX file: C:\Users\stea.KYIV\Documents\Visual Studio 2012\Projects\test\repaem.in.ua\repaem.in.ua\repaem.in.ua\Models\Data\BaseDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [repaem];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Musicians]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Musicians];
GO
IF OBJECT_ID(N'[dbo].[Cities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cities];
GO
IF OBJECT_ID(N'[dbo].[RepBases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RepBases];
GO
IF OBJECT_ID(N'[dbo].[BlackList]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BlackList];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
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
IF OBJECT_ID(N'[dbo].[Distincts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Distincts];
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
IF OBJECT_ID(N'[dbo].[Managers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Managers];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Musicians'
CREATE TABLE [dbo].[Musicians] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(256)  NULL,
    [CityId] int  NOT NULL,
    [Email] nvarchar(128)  NOT NULL,
    [PhoneNumber] nvarchar(16)  NOT NULL,
    [Password] uniqueidentifier  NOT NULL,
    [BandName] nvarchar(256)  NULL
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
    [DistinctId] int  NULL,
    [ManagerId] nvarchar(max)  NOT NULL,
    [CreationDate] datetime  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Lat] float  NOT NULL,
    [Long] float  NOT NULL
);
GO

-- Creating table 'BlackList'
CREATE TABLE [dbo].[BlackList] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientId] int  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [ManagerId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TimeStart] datetime  NOT NULL,
    [MusicianId] int  NOT NULL,
    [Sum] float  NOT NULL,
    [RepBaseId] int  NOT NULL,
    [RoomId] int  NOT NULL,
    [TimeEnd] datetime  NOT NULL,
    [Comment] nvarchar(max)  NULL
);
GO

-- Creating table 'Rooms'
CREATE TABLE [dbo].[Rooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [RepBaseId] int  NOT NULL,
    [Price] int  NULL,
    [Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Prices'
CREATE TABLE [dbo].[Prices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Sum] float  NOT NULL,
    [RoomId] int  NOT NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ClientId] int  NULL,
    [Text] nvarchar(max)  NULL,
    [Rating] float  NULL,
    [RepBaseId] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Distincts'
CREATE TABLE [dbo].[Distincts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [CityId] int  NOT NULL
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
    [ManagerId] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'Managers'
CREATE TABLE [dbo].[Managers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [CityId] int  NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [PhoneNumber] nvarchar(max)  NOT NULL,
    [Password] uniqueidentifier  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Musicians'
ALTER TABLE [dbo].[Musicians]
ADD CONSTRAINT [PK_Musicians]
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

-- Creating primary key on [Id] in table 'BlackList'
ALTER TABLE [dbo].[BlackList]
ADD CONSTRAINT [PK_BlackList]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
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

-- Creating primary key on [Id] in table 'Distincts'
ALTER TABLE [dbo].[Distincts]
ADD CONSTRAINT [PK_Distincts]
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

-- Creating primary key on [Id] in table 'Managers'
ALTER TABLE [dbo].[Managers]
ADD CONSTRAINT [PK_Managers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------