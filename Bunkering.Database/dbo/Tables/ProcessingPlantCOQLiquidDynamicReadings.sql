CREATE TABLE [dbo].[ProcessingPlantCOQLiquidDynamicReadings] (
    [ProcessingPlantCOQLiquidDynamicReadingId] INT        IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQLiquidDynamicId]        INT        NOT NULL,
    [Batch]                                    INT        NOT NULL,
    [Temperature]                              FLOAT (53) NOT NULL,
    [Density]                                  FLOAT (53) NOT NULL,
    [MeterFactor]                              FLOAT (53) NOT NULL,
    [Ctl]                                      FLOAT (53) NOT NULL,
    [Cpl]                                      FLOAT (53) NOT NULL,
    [WTAir]                                    FLOAT (53) NOT NULL,
    [MCubeAt15Degree]                          FLOAT (53) NOT NULL,
    [UsBarrelsAt15Degree]                      FLOAT (53) NOT NULL,
    [MTVac]                                    FLOAT (53) NOT NULL,
    [MTAir]                                    FLOAT (53) NOT NULL,
    [LongTonsAir]                              FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ProcessingPlantCOQLiquidDynamicReadings] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQLiquidDynamicReadingId] ASC),
    CONSTRAINT [FK_ProcessingPlantCOQLiquidDynamicReadings_ProcessingPlantCOQLiquidDynamics_ProcessingPlantCOQLiquidDynamicId] FOREIGN KEY ([ProcessingPlantCOQLiquidDynamicId]) REFERENCES [dbo].[ProcessingPlantCOQLiquidDynamics] ([ProcessingPlantCOQLiquidDynamicId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessingPlantCOQLiquidDynamicReadings_ProcessingPlantCOQLiquidDynamicId]
    ON [dbo].[ProcessingPlantCOQLiquidDynamicReadings]([ProcessingPlantCOQLiquidDynamicId] ASC);

