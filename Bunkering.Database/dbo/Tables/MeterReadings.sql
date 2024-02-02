CREATE TABLE [dbo].[MeterReadings] (
    [MeterReadingId]                           INT        IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQLiquidDynamicReadingId] INT        NOT NULL,
    [MCube]                                    FLOAT (53) NOT NULL,
    CONSTRAINT [PK_MeterReadings] PRIMARY KEY CLUSTERED ([MeterReadingId] ASC)
);

