CREATE TABLE [dbo].[ProcessingPlantCOQTankReadings] (
    [ProcessingPlantCOQTankId]  INT            IDENTITY (1, 1) NOT NULL,
    [MeasurementType]           NVARCHAR (MAX) NOT NULL,
    [ReadingM]                  FLOAT (53)     NOT NULL,
    [Temperature]               FLOAT (53)     NOT NULL,
    [Density]                   FLOAT (53)     NOT NULL,
    [SpecificGravityObs]        FLOAT (53)     NOT NULL,
    [BarrelsAtTankTables]       FLOAT (53)     NOT NULL,
    [VolumeCorrectionFactor]    FLOAT (53)     NOT NULL,
    [WTAir]                     FLOAT (53)     NOT NULL,
    [BarrelsToMCube]            FLOAT (53)     NOT NULL,
    [MCubeAt15Degree]           FLOAT (53)     NOT NULL,
    [UsBarrelsAt15Degree]       FLOAT (53)     NOT NULL,
    [MTVac]                     FLOAT (53)     NOT NULL,
    [MTAir]                     FLOAT (53)     NOT NULL,
    [LongTonsAir]               FLOAT (53)     NOT NULL,
    [ProcessingPlantCOQTankId1] INT            NOT NULL,
    CONSTRAINT [PK_ProcessingPlantCOQTankReadings] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQTankId] ASC),
    CONSTRAINT [FK_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTanks_ProcessingPlantCOQTankId1] FOREIGN KEY ([ProcessingPlantCOQTankId1]) REFERENCES [dbo].[ProcessingPlantCOQTanks] ([ProcessingPlantCOQTankId]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ProcessingPlantCOQTankReadings_ProcessingPlantCOQTankId1]
    ON [dbo].[ProcessingPlantCOQTankReadings]([ProcessingPlantCOQTankId1] ASC);

