CREATE TABLE [dbo].[ProcessingPlantCOQTankReadings] (
    [ProcessingPlantCOQTankReadingId] INT            IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQBatchTankId]   INT            NOT NULL,
    [MeasurementType]                 NVARCHAR (MAX) NOT NULL,
    [ReadingM]                        FLOAT (53)     NOT NULL,
    [Temperature]                     FLOAT (53)     NOT NULL,
    [Density]                         FLOAT (53)     NOT NULL,
    [SpecificGravityObs]              FLOAT (53)     NOT NULL,
    [BarrelsAtTankTables]             FLOAT (53)     NOT NULL,
    [VolumeCorrectionFactor]          FLOAT (53)     NOT NULL,
    [WTAir]                           FLOAT (53)     NOT NULL,
    [BarrelsToMCube]                  FLOAT (53)     NOT NULL,
    [MCubeAt15Degree]                 FLOAT (53)     NOT NULL,
    [UsBarrelsAt15Degree]             FLOAT (53)     NOT NULL,
    [MTVac]                           FLOAT (53)     NOT NULL,
    [MTAir]                           FLOAT (53)     NOT NULL,
    [LongTonsAir]                     FLOAT (53)     NOT NULL,
    CONSTRAINT [PK_ProcessingPlantCOQTankReadings_1] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQTankReadingId] ASC)
);








GO


