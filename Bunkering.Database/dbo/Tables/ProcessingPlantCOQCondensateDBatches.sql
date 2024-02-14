CREATE TABLE [dbo].[ProcessingPlantCOQCondensateDBatches] (
    [ProcessingPlantCOQCondensateDBatchId] INT        IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQId]                 INT        NOT NULL,
    [BatchId]                              INT        NOT NULL,
    [SumDiffGrossUsBarrelsAtTankTemp]      FLOAT (53) NULL,
    [SumDiffGrossBarrelsAt60]              FLOAT (53) NULL,
    [SumDiffGrossLongTons]                 FLOAT (53) NULL,
    [SumDiffBswBarrelsAt60]                FLOAT (53) NULL,
    [SumDiffBswLongTons]                   FLOAT (53) NULL,
    [SumDiffNettUsBarrelsAt60]             FLOAT (53) NULL,
    [SumDiffNettLongTons]                  FLOAT (53) NULL,
    [SumDiffNettMetricTons]                FLOAT (53) NULL,
    CONSTRAINT [PK_ProcessingPlantCOQCondensateDBatches] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQCondensateDBatchId] ASC)
);

