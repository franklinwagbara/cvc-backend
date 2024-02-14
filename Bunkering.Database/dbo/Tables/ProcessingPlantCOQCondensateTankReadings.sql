CREATE TABLE [dbo].[ProcessingPlantCOQCondensateTankReadings] (
    [ProcessingPlantCOQCondensateTankReadingId] INT            IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQCondensateSBatchTankId]  INT            NOT NULL,
    [MeasurementType]                           NVARCHAR (MAX) NOT NULL,
    [Ullage]                                    FLOAT (53)     NOT NULL,
    [TankTemp]                                  FLOAT (53)     NOT NULL,
    [Tov]                                       FLOAT (53)     NOT NULL,
    [Bsw]                                       FLOAT (53)     NOT NULL,
    [WaterGuage]                                FLOAT (53)     NOT NULL,
    [ObsvWater]                                 FLOAT (53)     NOT NULL,
    [ApiAt60]                                   FLOAT (53)     NOT NULL,
    [Vcf]                                       FLOAT (53)     NOT NULL,
    [LtBblFactor]                               FLOAT (53)     NOT NULL,
    [GrossUsBarrelsAtTankTemp]                  FLOAT (53)     NOT NULL,
    [GrossUsBarrelsAt60]                        FLOAT (53)     NOT NULL,
    [GrossLongTons]                             FLOAT (53)     NOT NULL,
    [BswBarrelsAt60]                            FLOAT (53)     NOT NULL,
    [BswLongTons]                               FLOAT (53)     NOT NULL,
    [NettUsBarrelsAt60]                         FLOAT (53)     NOT NULL,
    [NettLongTons]                              FLOAT (53)     NOT NULL,
    [NettMetricTons]                            FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_ProcessingPlantCOQCondensateTankReadings] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQCondensateTankReadingId] ASC),
    CONSTRAINT [FK_ProcessingPlantCOQCondensateTankReadings_ProcessingPlantCOQCondensateSBatchTanks_ProcessingPlantCOQCondensateSBatchTankId] FOREIGN KEY ([ProcessingPlantCOQCondensateSBatchTankId]) REFERENCES [dbo].[ProcessingPlantCOQCondensateSBatchTanks] ([ProcessingPlantCOQCondensateSBatchTankId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessingPlantCOQCondensateTankReadings_ProcessingPlantCOQCondensateSBatchTankId]
    ON [dbo].[ProcessingPlantCOQCondensateTankReadings]([ProcessingPlantCOQCondensateSBatchTankId] ASC);

