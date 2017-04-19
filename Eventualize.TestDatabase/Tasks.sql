CREATE TABLE [dbo].[Tasks]
(
	[PK] INT NOT NULL PRIMARY KEY IDENTITY,
	[Id] uniqueidentifier not null,
	[Title] varchar(50) not null,
	[Description] varchar(200) null,
	[Version] int not null,
	[LastEventDate] datetime2 not null, 
    [LastEventNumber] BIGINT NOT NULL, 
    [LastModifierId] VARCHAR(50) NOT NULL
)
