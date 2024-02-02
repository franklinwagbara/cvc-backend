CREATE TABLE [dbo].[Meters] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [PlantId] INT            NOT NULL,
    [Name]    NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_Meters] PRIMARY KEY CLUSTERED ([Id] ASC)
);

