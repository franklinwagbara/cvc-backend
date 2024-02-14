CREATE TABLE [dbo].[CondensateDynamicMeterReadings] (
    [CondensateDynamicMeterReadingId]           INT            IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQCondensateDBatchMeterId] INT            NOT NULL,
    [MeasurementType]                           NVARCHAR (MAX) NOT NULL,
    [MReadingBbl]                               FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_CondensateDynamicMeterReadings] PRIMARY KEY CLUSTERED ([CondensateDynamicMeterReadingId] ASC),
    CONSTRAINT [FK_CondensateDynamicMeterReadings_ProcessingPlantCOQCondensateDBatchMeters_ProcessingPlantCOQCondensateDBatchMeterId] FOREIGN KEY ([ProcessingPlantCOQCondensateDBatchMeterId]) REFERENCES [dbo].[ProcessingPlantCOQCondensateDBatchMeters] ([ProcessingPlantCOQCondensateDBatchMeterId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_CondensateDynamicMeterReadings_ProcessingPlantCOQCondensateDBatchMeterId]
    ON [dbo].[CondensateDynamicMeterReadings]([ProcessingPlantCOQCondensateDBatchMeterId] ASC);

