﻿CREATE TABLE [dbo].[ProcessingPlantCOQCondensateSBatchTanks] (
    [ProcessingPlantCOQCondensateSBatchTankId] INT        IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQCondensateSBatchId]     INT        NOT NULL,
    [TankId]                                   INT        NOT NULL,
    [DiffGrossUsBarrelsAtTankTemp]             FLOAT (53) NULL,
    [DiffGrossBarrelsAt60]                     FLOAT (53) NULL,
    [DiffGrossLongTons]                        FLOAT (53) NULL,
    [DiffBswBarrelsAt60]                       FLOAT (53) NULL,
    [DiffBswLongTons]                          FLOAT (53) NULL,
    [DiffNettUsBarrelsAt60]                    FLOAT (53) NULL,
    [DiffNettLongTons]                         FLOAT (53) NULL,
    [DiffNettMetricTons]                       FLOAT (53) NULL,
    CONSTRAINT [PK_ProcessingPlantCOQCondensateSBatchTanks] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQCondensateSBatchTankId] ASC),
    CONSTRAINT [FK_ProcessingPlantCOQCondensateSBatchTanks_ProcessingPlantCOQCondensateSBatches_ProcessingPlantCOQCondensateSBatchId] FOREIGN KEY ([ProcessingPlantCOQCondensateSBatchId]) REFERENCES [dbo].[ProcessingPlantCOQCondensateSBatches] ([ProcessingPlantCOQCondensateSBatchId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessingPlantCOQCondensateSBatchTanks_ProcessingPlantCOQCondensateSBatchId]
    ON [dbo].[ProcessingPlantCOQCondensateSBatchTanks]([ProcessingPlantCOQCondensateSBatchId] ASC);
