CREATE TABLE [dbo].[Tanks] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [FacilityId] INT             NOT NULL,
    [ProductId]  INT             NOT NULL,
    [Name]       NVARCHAR (MAX)  NOT NULL,
    [Capacity]   DECIMAL (18, 2) NOT NULL,
    CONSTRAINT [PK_Tanks] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tanks_Facilities_FacilityId] FOREIGN KEY ([FacilityId]) REFERENCES [dbo].[Facilities] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Tanks_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Tanks_FacilityId]
    ON [dbo].[Tanks]([FacilityId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Tanks_ProductId]
    ON [dbo].[Tanks]([ProductId] ASC);

