CREATE TABLE [dbo].[ProcessingPlantCOQCondensateDBatchMeters] (
    [ProcessingPlantCOQCondensateDBatchMeterId] INT        IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQCondensateDBatchId]      INT        NOT NULL,
    [MeterId]                                   INT        NOT NULL,
    [Temperature]                               FLOAT (53) NOT NULL,
    [Pressure]                                  FLOAT (53) NOT NULL,
    [MeterFactor]                               FLOAT (53) NOT NULL,
    [Ctl]                                       FLOAT (53) NOT NULL,
    [Cpl]                                       FLOAT (53) NOT NULL,
    [ApiAt60]                                   FLOAT (53) NOT NULL,
    [Vcf]                                       FLOAT (53) NOT NULL,
    [Bsw]                                       FLOAT (53) NOT NULL,
    [GrossLtBblFactor]                          FLOAT (53) NOT NULL,
    [GrossUsBarrelsAt60]                        FLOAT (53) NOT NULL,
    [GrossLongTons]                             FLOAT (53) NOT NULL,
    [BswBarrelsAt60]                            FLOAT (53) NOT NULL,
    [BswLongTons]                               FLOAT (53) NOT NULL,
    [NettUsBarrelsAt60]                         FLOAT (53) NOT NULL,
    [NettLongTons]                              FLOAT (53) NOT NULL,
    [NettMetricTons]                            FLOAT (53) NOT NULL,
    CONSTRAINT [PK_ProcessingPlantCOQCondensateDBatchMeters] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQCondensateDBatchMeterId] ASC),
    CONSTRAINT [FK_ProcessingPlantCOQCondensateDBatchMeters_ProcessingPlantCOQCondensateDBatches_ProcessingPlantCOQCondensateDBatchId] FOREIGN KEY ([ProcessingPlantCOQCondensateDBatchId]) REFERENCES [dbo].[ProcessingPlantCOQCondensateDBatches] ([ProcessingPlantCOQCondensateDBatchId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessingPlantCOQCondensateDBatchMeters_ProcessingPlantCOQCondensateDBatchId]
    ON [dbo].[ProcessingPlantCOQCondensateDBatchMeters]([ProcessingPlantCOQCondensateDBatchId] ASC);

