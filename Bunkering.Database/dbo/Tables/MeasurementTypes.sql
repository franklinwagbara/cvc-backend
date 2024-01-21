CREATE TABLE [dbo].[MeasurementTypes] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_MeasurementTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

