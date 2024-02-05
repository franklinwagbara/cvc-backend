CREATE TABLE [dbo].[ProcessingPlantCOQLiquidDynamicBatches] (
    [ProcessingPlantCOQLiquidDynamicBatchId] INT        IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQId]                   INT        NOT NULL,
    [BatchId]                                INT        NOT NULL,
    [SumDiffMCubeAt15Degree]                 FLOAT (53) NULL,
    [SumDiffUsBarrelsAt15Degree]             FLOAT (53) NULL,
    [SumDiffMTVac]                           FLOAT (53) NULL,
    [SumDiffMTAir]                           FLOAT (53) NULL,
    [SumDiffLongTonsAir]                     FLOAT (53) NULL,
    CONSTRAINT [PK_ProcessingPlantCOQLiquidDynamicBatches] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQLiquidDynamicBatchId] ASC)
);

