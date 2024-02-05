CREATE TABLE [dbo].[ProcessingPlantCOQLiquidDynamicMeters] (
    [ProcessingPlantCOQLiquidDynamicMeterId] INT        IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQLiquidDynamicBatchId] INT        NOT NULL,
    [MeterId]                                INT        NOT NULL,
    [Temperature]                            FLOAT (53) NOT NULL,
    [Density]                                FLOAT (53) NOT NULL,
    [MeterFactor]                            FLOAT (53) NOT NULL,
    [Ctl]                                    FLOAT (53) NOT NULL,
    [Cpl]                                    FLOAT (53) NOT NULL,
    [WTAir]                                  FLOAT (53) NOT NULL,
    [MCubeAt15Degree]                        FLOAT (53) NOT NULL,
    [UsBarrelsAt15Degree]                    FLOAT (53) NOT NULL,
    [MTVac]                                  FLOAT (53) NOT NULL,
    [MTAir]                                  FLOAT (53) NOT NULL,
    [LongTonsAir]                            FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ProcessingPlantCOQLiquidDynamicMeters] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQLiquidDynamicMeterId] ASC),
    CONSTRAINT [FK_ProcessingPlantCOQLiquidDynamicMeters_ProcessingPlantCOQLiquidDynamicBatches_ProcessingPlantCOQLiquidDynamicBatchId] FOREIGN KEY ([ProcessingPlantCOQLiquidDynamicBatchId]) REFERENCES [dbo].[ProcessingPlantCOQLiquidDynamicBatches] ([ProcessingPlantCOQLiquidDynamicBatchId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessingPlantCOQLiquidDynamicMeters_ProcessingPlantCOQLiquidDynamicBatchId]
    ON [dbo].[ProcessingPlantCOQLiquidDynamicMeters]([ProcessingPlantCOQLiquidDynamicBatchId] ASC);

