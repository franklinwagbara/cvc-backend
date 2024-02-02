CREATE TABLE [dbo].[Meters] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [PlantId]   INT            NOT NULL,
    [Name]      NVARCHAR (MAX) NOT NULL,
    [DeletedAt] DATETIME2 (7)  DEFAULT ('0001-01-01T00:00:00.0000000') NULL,
    CONSTRAINT [PK_Meters] PRIMARY KEY CLUSTERED ([Id] ASC)
);



