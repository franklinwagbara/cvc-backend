CREATE TABLE [dbo].[ApplicationDepots] (
    [Id]        INT             IDENTITY (1, 1) NOT NULL,
    [DepotId]   INT             DEFAULT ((0)) NOT NULL,
    [AppId]     INT             DEFAULT ((0)) NOT NULL,
    [Volume]    DECIMAL (18, 2) DEFAULT ((0)) NOT NULL,
    [ProductId] INT             DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ApplicationDepots] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ApplicationDepots_Applications_AppId] FOREIGN KEY ([AppId]) REFERENCES [dbo].[Applications] ([Id]),
    CONSTRAINT [FK_ApplicationDepots_Applications_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Applications] ([Id])
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

