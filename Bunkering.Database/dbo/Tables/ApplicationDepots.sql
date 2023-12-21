CREATE TABLE [dbo].[ApplicationDepots] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [DepotId]   INT             NOT NULL,
    [AppId]     INT             NOT NULL,
    [Volume]    DECIMAL (18, 2) NOT NULL,
    [ProductId] INT             NOT NULL,
    CONSTRAINT [PK_ApplicationDepots] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ApplicationDepots_Applications_AppId] FOREIGN KEY ([AppId]) REFERENCES [dbo].[Applications] ([Id]),
    CONSTRAINT [FK_ApplicationDepots_Applications_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Applications] ([Id]),
    CONSTRAINT [FK_ApplicationDepots_Depots_DepotId] FOREIGN KEY ([DepotId]) REFERENCES [dbo].[Depots] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_ApplicationDepots_ProductId]
    ON [dbo].[ApplicationDepots]([ProductId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationDepots_DepotId]
    ON [dbo].[ApplicationDepots]([DepotId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationDepots_AppId]
    ON [dbo].[ApplicationDepots]([AppId] ASC);

