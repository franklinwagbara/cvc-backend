CREATE TABLE [dbo].[TankViewModel] (
    [Id]           INT             IDENTITY (1, 1) NOT NULL,
    [FacilityId]   INT             NOT NULL,
    [Name]         NVARCHAR (MAX)  NOT NULL,
    [Capacity]     DECIMAL (18, 2) NOT NULL,
    [ProductId]    INT             NOT NULL,
    [VesselTypeId] INT             NULL,
    CONSTRAINT [PK_TankViewModel] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TankViewModel_VesselTypes_VesselTypeId] FOREIGN KEY ([VesselTypeId]) REFERENCES [dbo].[VesselTypes] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TankViewModel_VesselTypeId]
    ON [dbo].[TankViewModel]([VesselTypeId] ASC);

