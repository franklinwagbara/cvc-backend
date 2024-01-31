CREATE TABLE [dbo].[PlantTanks] (
    [PlantTankId] INT             IDENTITY (1, 1) NOT NULL,
    [PlantId]     INT             NOT NULL,
    [TankName]    NVARCHAR (MAX)  NOT NULL,
    [Product]     NVARCHAR (MAX)  NOT NULL,
    [Capacity]    DECIMAL (18, 2) NOT NULL,
    [Position]    NVARCHAR (MAX)  NULL,
    [IsDeleted]   BIT             NOT NULL,
    CONSTRAINT [PK_PlantTanks] PRIMARY KEY CLUSTERED ([PlantTankId] ASC),
    CONSTRAINT [FK_PlantTanks_Plants_PlantId] FOREIGN KEY ([PlantId]) REFERENCES [dbo].[Plants] ([Id]) ON DELETE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_PlantTanks_PlantId]
    ON [dbo].[PlantTanks]([PlantId] ASC);

