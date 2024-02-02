CREATE TABLE [dbo].[ProcessingPlantCOQTanks] (
    [ProcessingPlantCOQTankId]   INT        IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQId]       INT        NOT NULL,
    [TankId]                     INT        NOT NULL,
    [SumDiffLongTonsAir]         FLOAT (53) NULL,
    [SumDiffMCubeAt15Degree]     FLOAT (53) NULL,
    [SumDiffMTAir]               FLOAT (53) NULL,
    [SumDiffMTVac]               FLOAT (53) NULL,
    [SumDiffUsBarrelsAt15Degree] FLOAT (53) NULL,
    CONSTRAINT [PK_ProcessingPlantCOQTanks] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQTankId] ASC)
);



