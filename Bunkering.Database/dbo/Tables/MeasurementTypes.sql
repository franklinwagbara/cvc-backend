CREATE TABLE [dbo].[MeasurementTypes] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_MeasurementTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

