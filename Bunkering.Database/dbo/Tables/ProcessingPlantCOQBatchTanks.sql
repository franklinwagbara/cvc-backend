CREATE TABLE [dbo].[ProcessingPlantCOQBatchTanks] (
    [ProcessingPlantCOQBatchTankId] INT IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQBatchId]     INT NOT NULL,
    [TankId]                        INT NOT NULL,
    CONSTRAINT [PK_ProcessingPlantCOQTanks] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQBatchTankId] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessingPlantCOQBatchTanks_ProcessingPlantCOQBatchId]
    ON [dbo].[ProcessingPlantCOQBatchTanks]([ProcessingPlantCOQBatchId] ASC);

