CREATE TABLE [dbo].[LiquidDynamicMeterReadings] (
    [LiquidDynamicMeterReadingId]            INT            IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQLiquidDynamicMeterId] INT            NOT NULL,
    [MeasurementType]                        NVARCHAR (MAX) NOT NULL,
    [MCube]                                  FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_LiquidDynamicMeterReadings] PRIMARY KEY CLUSTERED ([LiquidDynamicMeterReadingId] ASC),
    CONSTRAINT [FK_LiquidDynamicMeterReadings_ProcessingPlantCOQLiquidDynamicMeters_ProcessingPlantCOQLiquidDynamicMeterId] FOREIGN KEY ([ProcessingPlantCOQLiquidDynamicMeterId]) REFERENCES [dbo].[ProcessingPlantCOQLiquidDynamicMeters] ([ProcessingPlantCOQLiquidDynamicMeterId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_LiquidDynamicMeterReadings_ProcessingPlantCOQLiquidDynamicMeterId]
    ON [dbo].[LiquidDynamicMeterReadings]([ProcessingPlantCOQLiquidDynamicMeterId] ASC);

