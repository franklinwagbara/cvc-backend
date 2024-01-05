CREATE TABLE [dbo].[Products] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (MAX) NOT NULL,
    [ProductType] NVARCHAR (MAX) DEFAULT (N'') NOT NULL,
    [IsDeleted]   BIT            DEFAULT (CONVERT([bit],(0))) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC)
);





