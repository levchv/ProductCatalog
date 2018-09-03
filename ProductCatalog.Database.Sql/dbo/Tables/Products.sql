CREATE TABLE [dbo].[Products] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Code]        NVARCHAR (50)  NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
    [Price]       FLOAT (53)     NOT NULL,
    [Photo]       NVARCHAR (MAX) NULL,
    [LastUpdated] DATETIME       NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC),
    UNIQUE NONCLUSTERED ([Code] ASC)
);

