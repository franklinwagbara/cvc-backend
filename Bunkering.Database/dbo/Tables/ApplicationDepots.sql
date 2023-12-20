CREATE TABLE [dbo].[ApplicationDepots] (
    [Id]        INT          NOT NULL,
    [DepotId]   INT          NOT NULL,
    [AppId]     INT          NOT NULL,
    [Volume]    DECIMAL (18) NOT NULL,
    [ProductId] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

