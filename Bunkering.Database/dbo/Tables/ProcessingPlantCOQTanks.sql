CREATE TABLE [dbo].[ProcessingPlantCOQTanks] (
    [ProcessingPlantCOQTankId] INT IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQId]     INT NOT NULL,
    [TankId]                   INT NOT NULL,
    CONSTRAINT [PK_ProcessingPlantCOQTanks] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQTankId] ASC)
);

