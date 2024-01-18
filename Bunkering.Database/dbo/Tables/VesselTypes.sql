CREATE TABLE [dbo].[VesselTypes] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_VesselTypes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

