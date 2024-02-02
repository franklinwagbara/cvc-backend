CREATE TABLE [dbo].[ProcessingPlantCOQLiquidDynamics] (
    [ProcessingPlantCOQLiquidDynamicId] INT IDENTITY (1, 1) NOT NULL,
    [ProcessingPlantCOQId]              INT NOT NULL,
    [MeterId]                           INT NOT NULL,
    CONSTRAINT [PK_ProcessingPlantCOQLiquidDynamics] PRIMARY KEY CLUSTERED ([ProcessingPlantCOQLiquidDynamicId] ASC)
);

