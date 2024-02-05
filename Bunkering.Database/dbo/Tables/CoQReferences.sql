CREATE TABLE [dbo].[CoQReferences] (
    [Id]         INT IDENTITY (1, 1) NOT NULL,
    [DepotCoQId] INT NULL,
    [PlantCoQId] INT NULL,
    [IsDeleted]  BIT NOT NULL,
    CONSTRAINT [PK_CoQReferences] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CoQReferences_CoQs] FOREIGN KEY ([DepotCoQId]) REFERENCES [dbo].[CoQs] ([Id]),
    CONSTRAINT [FK_CoQReferences_ProcessingPlantCOQS] FOREIGN KEY ([PlantCoQId]) REFERENCES [dbo].[ProcessingPlantCOQS] ([ProcessingPlantCOQId])
);

