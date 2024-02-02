CREATE TABLE [dbo].[ProcessingPlantCOQBatches] (
    [ProcessingPlantCOQBatchId]  INT        IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQId]       INT        NOT NULL,
    [BatchId]                    INT        NOT NULL,
    [SumDiffMCubeAt15Degree]     FLOAT (53) NULL,
    [SumDiffUsBarrelsAt15Degree] FLOAT (53) NULL,
    [SumDiffMTVac]               FLOAT (53) NULL,
    [SumDiffMTAir]               FLOAT (53) NULL,
    [SumDiffLongTonsAir]         FLOAT (53) NULL,
    CONSTRAINT [PK_ProcessingPlantCOQBatches] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQBatchId] ASC)
);

